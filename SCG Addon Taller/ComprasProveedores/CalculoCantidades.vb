
Module CalculoCantidades

    Public Enum TipoMovimiento
        Creacion
        Cierre
        Cancelacion
    End Enum

    Public Enum ReabreDocumentos
        No
        SiSinConfirmacion
        SiConConfirmacion
    End Enum

    Public AccionSeleccionada As Boolean = False
    Public AbreDocumentos As Boolean = False
    Public ReopenDocument As ReabreDocumentos
    Public ExisteOrdenCompra As Boolean = False

    Sub New()
        Try
            ObtenerConfiguracionDocumentos()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ObtenerConfiguracionDocumentos()
        Dim Query As String = "SELECT T0.""ReopOrder"", T0.""ForceReOrd"" FROM ""ADP1"" T0 WITH (nolock) WHERE T0.""ObjType"" = '22'"
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim ReopenOrder As String = String.Empty
        Dim ForceReopenOrder As String = String.Empty
        Try
            ReopenDocument = ReabreDocumentos.No
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)
            If oRecordset.RecordCount > 0 Then
                ReopenOrder = oRecordset.Fields.Item("ReopOrder").Value.ToString()
                ForceReopenOrder = oRecordset.Fields.Item("ForceReOrd").Value.ToString()
                If ForceReopenOrder = "Y" Then
                    ReopenDocument = ReabreDocumentos.SiSinConfirmacion
                Else
                    If ReopenOrder = "Y" Then
                        ReopenDocument = ReabreDocumentos.SiConConfirmacion
                    Else
                        ReopenDocument = ReabreDocumentos.No
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ControlDocumentos(ByVal Abrir As Boolean)
        Try
            Select Case ReopenDocument
                Case ReabreDocumentos.No
                    AbreDocumentos = False
                    AccionSeleccionada = True
                Case ReabreDocumentos.SiConConfirmacion
                    If AccionSeleccionada = False Then
                        AbreDocumentos = Abrir
                        AccionSeleccionada = True
                    End If
                Case ReabreDocumentos.SiSinConfirmacion
                    AbreDocumentos = True
                    AccionSeleccionada = True
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub RecalcularCostos(ByVal TipoDocumento As SAPbobsCOM.BoObjectTypes, ByVal TipoMovimiento As TipoMovimiento, ByVal GeneraMovimientoInventario As Boolean, ByRef CantidadOfertaVentas As Double, ByRef CostoOfertaVentas As Double, ByRef CantidadAbiertaDocumentoCompra As Double, ByRef CostoDocumentoCompra As Double)
        Try
            Select Case TipoDocumento
                Case SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            CostoOfertaVentas += CostoDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            CostoOfertaVentas -= CostoDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CostoOfertaVentas -= CostoDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseReturns
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            CostoOfertaVentas -= CostoDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CostoOfertaVentas += CostoDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseInvoices
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            If GeneraMovimientoInventario Then
                                CostoOfertaVentas += CostoDocumentoCompra
                            End If
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            If GeneraMovimientoInventario Then
                                CostoOfertaVentas -= CostoDocumentoCompra
                            End If
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            CostoOfertaVentas -= CostoDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cierre
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CostoOfertaVentas += CostoDocumentoCompra
                    End Select
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub RecalcularCantidades(ByVal TipoDocumento As SAPbobsCOM.BoObjectTypes, ByVal TipoMovimiento As TipoMovimiento, ByVal GeneraMovimientoInventario As Boolean, ByRef CantidadOfertaVentas As Double, ByRef CantidadAbiertaDocumentoCompra As Double, ByRef CantidadSolicitada As Double, ByRef CantidadPendiente As Double, ByRef CantidadRecibida As Double)
        Try
            Select Case TipoDocumento
                Case SAPbobsCOM.BoObjectTypes.oPurchaseQuotations
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            CantidadPendiente += CantidadAbiertaDocumentoCompra
                            CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CantidadPendiente += CantidadAbiertaDocumentoCompra
                            CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseOrders
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            CantidadPendiente += CantidadAbiertaDocumentoCompra
                            CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CantidadPendiente += CantidadAbiertaDocumentoCompra
                            CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            CantidadRecibida += CantidadAbiertaDocumentoCompra
                            CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            CantidadPendiente += CantidadAbiertaDocumentoCompra
                            CantidadRecibida -= CantidadAbiertaDocumentoCompra
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CantidadRecibida -= CantidadAbiertaDocumentoCompra
                            CantidadSolicitada += CantidadAbiertaDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseReturns
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            CantidadRecibida -= CantidadAbiertaDocumentoCompra
                            If AbreDocumentos AndAlso ExisteOrdenCompra Then
                                CantidadSolicitada += CantidadAbiertaDocumentoCompra
                            Else
                                CantidadPendiente += CantidadAbiertaDocumentoCompra
                            End If
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            CantidadRecibida += CantidadAbiertaDocumentoCompra
                            CantidadPendiente -= CantidadAbiertaDocumentoCompra
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseInvoices
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            If GeneraMovimientoInventario Then
                                CantidadRecibida += CantidadAbiertaDocumentoCompra
                                CantidadSolicitada -= CantidadAbiertaDocumentoCompra
                            End If
                        Case CalculoCantidades.TipoMovimiento.Cierre
                            'No Aplica
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            If GeneraMovimientoInventario Then
                                CantidadRecibida -= CantidadAbiertaDocumentoCompra
                                CantidadSolicitada += CantidadAbiertaDocumentoCompra
                            End If
                    End Select
                Case SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes
                    Select Case TipoMovimiento
                        Case CalculoCantidades.TipoMovimiento.Creacion
                            If GeneraMovimientoInventario Then
                                CantidadRecibida -= CantidadAbiertaDocumentoCompra
                                If AbreDocumentos AndAlso ExisteOrdenCompra Then
                                    CantidadSolicitada += CantidadAbiertaDocumentoCompra
                                Else
                                    CantidadPendiente += CantidadAbiertaDocumentoCompra
                                End If
                            End If
                        Case CalculoCantidades.TipoMovimiento.Cierre
                        Case CalculoCantidades.TipoMovimiento.Cancelacion
                            If GeneraMovimientoInventario Then
                                CantidadRecibida += CantidadAbiertaDocumentoCompra
                                CantidadPendiente -= CantidadAbiertaDocumentoCompra
                            End If
                    End Select
            End Select
            AjustarCantidadesMinimasMaximas(CantidadOfertaVentas, CantidadSolicitada, CantidadPendiente, CantidadRecibida)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarCantidadesMinimasMaximas(ByRef CantidadOfertaVentas As Double, ByRef CantidadSolicitada As Double, ByRef CantidadPendiente As Double, ByRef CantidadRecibida As Double)
        Try
            If CantidadSolicitada > CantidadOfertaVentas Then
                CantidadSolicitada = CantidadOfertaVentas
            End If

            If CantidadSolicitada < 0 Then
                CantidadSolicitada = 0
            End If

            If CantidadPendiente > CantidadOfertaVentas Then
                CantidadPendiente = CantidadOfertaVentas
            End If

            If CantidadPendiente < 0 Then
                CantidadPendiente = 0
            End If

            If CantidadRecibida < 0 Then
                CantidadRecibida = 0
            End If

            If CantidadRecibida > CantidadOfertaVentas Then
                CantidadRecibida = CantidadOfertaVentas
                CantidadSolicitada = 0
                CantidadPendiente = 0
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Module

