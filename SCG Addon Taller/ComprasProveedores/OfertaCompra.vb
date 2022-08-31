Imports System.Collections.Generic

Module OfertaCompra

    Private ListaCantidadesAbiertas As Dictionary(Of String, Double)
    Private EsCompraDMS As Boolean = False
    Private DocEntry As String = String.Empty

    Sub New()
        Try
            ListaCantidadesAbiertas = New Dictionary(Of String, Double)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = "540000988" Then
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        ItemPressed(FormUID, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ValidarEliminarLinea(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean, ByVal NumeroFila As Integer)
        Dim Formulario As SAPbouiCOM.Form
        Dim NumeroOrdenTrabajo As String = String.Empty
        Dim IDActividad As String = String.Empty
        Dim Matriz As SAPbouiCOM.Matrix
        Dim DocType As String = String.Empty
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            If Formulario.TypeEx = "540000988" AndAlso NumeroFila >= 0 Then
                NumeroOrdenTrabajo = Formulario.DataSources.DBDataSources.Item("OPQT").GetValue("U_SCGD_Numero_OT", 0).Trim()
                DocType = Formulario.DataSources.DBDataSources.Item("OPQT").GetValue("DocType", 0).Trim()
                If DocType = "I" AndAlso Not String.IsNullOrEmpty(NumeroOrdenTrabajo) Then
                    Matriz = Formulario.Items.Item("38").Specific
                    IDActividad = CType(Matriz.Columns.Item("U_SCGD_ID").Cells.Item(NumeroFila).Specific, SAPbouiCOM.EditText).Value
                    If Not String.IsNullOrEmpty(IDActividad) Then
                        BubbleEvent = False
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorEliminarLineaCompraOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "1"
                        GuardarEstadoPrevioLineas(FormUID, BubbleEvent)
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "1"
                        RestablecerCantidadesCanceladasCerradas()
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CancelarCompra(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                GuardarEstadoPrevioLineas(FormUID, BubbleEvent)
            Else
                RestablecerCantidadesCanceladasCerradas()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub GuardarEstadoPrevioLineas(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Dim DocumentoCompra As SAPbobsCOM.Documents
        Dim EstadoLinea As SAPbobsCOM.BoStatus
        Dim NumeroOT As String = String.Empty
        Dim Formulario As SAPbouiCOM.Form
        Dim IDActividad As String = String.Empty
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            ListaCantidadesAbiertas = New Dictionary(Of String, Double)
            DocEntry = String.Empty
            If Formulario.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                DocumentoCompra = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations)
                DocEntry = Formulario.DataSources.DBDataSources.Item("OPQT").GetValue("DocEntry", 0).ToString().Trim()
                If Not String.IsNullOrEmpty(DocEntry) Then
                    If DocumentoCompra.GetByKey(DocEntry) Then
                        For i As Integer = 0 To DocumentoCompra.Lines.Count - 1
                            DocumentoCompra.Lines.SetCurrentLine(i)
                            EstadoLinea = DocumentoCompra.Lines.LineStatus
                            NumeroOT = DocumentoCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                            IDActividad = DocumentoCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                            If Not String.IsNullOrEmpty(NumeroOT) AndAlso Not String.IsNullOrEmpty(IDActividad) AndAlso EstadoLinea = SAPbobsCOM.BoStatus.bost_Open Then
                                ListaCantidadesAbiertas.Add(IDActividad, DocumentoCompra.Lines.RemainingOpenInventoryQuantity)
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub RestablecerCantidadesCanceladasCerradas()
        Dim DocumentoCompra As SAPbobsCOM.Documents
        Dim OfertaVentas As SAPbobsCOM.Documents
        Dim oDocumento As DMS_Connector.Business_Logic.DataContract.SAPDocumento.oDocumento
        Dim Query As String = "SELECT TOP 1 T0.""U_DocEntry"" FROM ""@SCGD_OT"" T0 WHERE T0.""Code"" = '{0}'"
        Dim DocEntryOfertaVentas As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Dim IDActividad As String = String.Empty
        Dim NumeroLineaOferta As Integer
        Dim ProcesarDocumento As Boolean = False
        Dim CantidadRecibida As Double = 0
        Dim CantidadPendiente As Double = 0
        Dim CantidadSolicitada As Double = 0
        Dim CantidadAbiertaDocumentoCompra As Double = 0
        Try
            DocumentoCompra = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations)
            If ListaCantidadesAbiertas.Count <= 0 Then
                Return
            End If

            If Not DocumentoCompra.GetByKey(DocEntry) Then
                Return
            End If

            NumeroOT = DocumentoCompra.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            Query = String.Format(Query, NumeroOT)
            DocEntryOfertaVentas = DMS_Connector.Helpers.EjecutarConsulta(Query)

            If String.IsNullOrEmpty(DocEntryOfertaVentas) Then
                Return
            Else
                oDocumento = DMS_Connector.Helpers.CargaCotizacionConPosiciones(DocEntryOfertaVentas, OfertaVentas)
            End If

            For i As Integer = 0 To DocumentoCompra.Lines.Count - 1
                DocumentoCompra.Lines.SetCurrentLine(i)
                IDActividad = DocumentoCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                If ListaCantidadesAbiertas.ContainsKey(IDActividad) AndAlso DocumentoCompra.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Open Then
                    NumeroLineaOferta = DMS_Connector.Helpers.GetLinePosition(oDocumento.Lineas, IDActividad)
                    If NumeroLineaOferta >= 0 Then
                        OfertaVentas.Lines.SetCurrentLine(NumeroLineaOferta)
                        CantidadAbiertaDocumentoCompra = ListaCantidadesAbiertas.Item(IDActividad)
                        CantidadSolicitada = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                        CantidadPendiente = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                        CantidadRecibida = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value

                        CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations, TipoMovimiento.Cierre, False, OfertaVentas.Lines.Quantity, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)

                        OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
                        OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente
                        OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida

                        ProcesarDocumento = True
                    End If
                End If
            Next

            If ProcesarDocumento Then
                If OfertaVentas.Update() = 0 Then
                    DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.PedidoCancelado, SAPbouiCOM.BoMessageTime.bmt_Short, False)
                Else
                    DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(String.Format("{0}: {1}", DMS_Connector.Company.CompanySBO.GetLastErrorCode(), DMS_Connector.Company.CompanySBO.GetLastErrorDescription()), SAPbouiCOM.BoMessageTime.bmt_Short, True)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
