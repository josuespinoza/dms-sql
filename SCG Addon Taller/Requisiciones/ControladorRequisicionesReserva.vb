Imports SAPbouiCOM
Imports SAPbobsCOM
Imports System.Linq
Imports System.Collections.Generic
Imports DMS_Connector.Business_Logic.DataContract

Public Module ControladorRequisicionesReserva

#Region "Propiedades"

    Private Enum TipoMovimiento
        Requisicion = 1
        Comprar = 2
        Trasladar = 3
        Rechazar = 4
    End Enum

    Private Enum enumTrasladadoOTHija

        scgOTHijaSI = 1
        scgOTHijaNO = 2

    End Enum

    Private Enum CodigoEstadoRequisicion
        Pendiente = 1
        Trasladado = 2
    End Enum

    Private Enum Trasladado
        NoProcesado = 0
        NO = 1
        SI = 2
        PendienteTraslado = 3
        PendienteBodega = 4
    End Enum

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
        OtrosCostos = 11
        OtrosIngresos = 12
    End Enum

    Private Enum ArticuloAprobado
        scgSi = 1
        scgNo = 2
        scgFalta = 3
        scgCambioOT = 4
    End Enum

    Private Enum TipoRequisicion
        Traslado = 1
        Devolucion = 2
        Reserva = 3
        DevolucionReserva = 4
    End Enum

    Private Enum ProcesamientoLinea
        Requisicion = 1
        RequisicionDevolucion = 2
        NingunaAccion = 3
        TrasladoBodega = 4
        AgregarControlColaborador = 5
        EliminarControlColaborador = 6
        AnularRequisicion = 7
        AnularTrasladoBodega = 8
        ProcesaServicioExterno = 9
        AnulaServicioExterno = 10
        AnularRequisicionDevolucion = 11
    End Enum

#End Region

    Sub New()
        Try
            'Implementar manejo del constructor aquí
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#Region "Métodos"

    Public Sub ProcesarRequisicionReserva(ByRef oFormulario As SAPbouiCOM.Form, ByRef Cotizacion As SAPbobsCOM.Documents, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String, ByVal EsCancelacion As Boolean, ByRef ErrorProcesando As Boolean)
        'Dim Cotizacion As SAPbobsCOM.Documents
        Dim CotizacionCache As SAPDocumento.oDocumento = New SAPDocumento.oDocumento()
        Dim PaqueteList As Paquete_List = New Paquete_List()
        Dim ConfiguracionesSucursal As ConfiguracionSucursal_List = New ConfiguracionSucursal_List()
        Dim BodegasCentroCosto As BodegaCentroCosto_List = New BodegaCentroCosto_List()
        Dim ListaRequisiciones As List(Of SAPbobsCOM.GeneralData)
        Dim RequisicionDataList As RequisicionData_List = New RequisicionData_List()
        Dim LineaDocumento As SAPDocumento.oLineasDocumento
        Dim Articulo As SAPbobsCOM.IItems
        Dim MensajeCCOT As Boolean
        Dim CotizacionInicial As SAPDocumento.oDocumento
        Dim oPaqueteListResultado As Paquete_List = New Paquete_List()
        Try
            'Cotizacion = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            Articulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            CotizacionInicial = CargarCotizacionInicial(Cotizacion.DocEntry)
            'If Cotizacion.GetByKey(DocEntryCotizacion) Then
            'CancelarLineasRequisicion(oFormulario, Cotizacion, NumeroSerieCita, ConsecutivoCita, EsCancelacion, ErrorProcesando)
            CargarCotizacionActual(Cotizacion, CotizacionCache, PaqueteList, EsCancelacion)
            CargaConfiguracionSucursal(CotizacionCache, ConfiguracionesSucursal, BodegasCentroCosto)

            '********************************
            'Valida Paquetes
            '*******************************
            ManejarPaquete(PaqueteList, oPaqueteListResultado)

            For i As Integer = 0 To Cotizacion.Lines.Count - 1
                Cotizacion.Lines.SetCurrentLine(i)
                LineaDocumento = New SAPDocumento.oLineasDocumento()
                AsignaValorACotizacionDataContract(Cotizacion, LineaDocumento, EsCancelacion)
                ValidaPaquete(LineaDocumento, oPaqueteListResultado, Cotizacion, NumeroSerieCita, ConsecutivoCita)
                If Articulo.GetByKey(LineaDocumento.ItemCode) Then
                    ValidaArticulo(LineaDocumento, Articulo, BodegasCentroCosto, ConfiguracionesSucursal, MensajeCCOT, NumeroSerieCita, ConsecutivoCita)

                    If oFormulario.Mode = BoFormMode.fm_ADD_MODE Then
                        ValidaDisponibilidadArticulo(LineaDocumento, Articulo, ConfiguracionesSucursal, EsCancelacion)
                        If LineaDocumento.TipoMovimiento = TipoMovimiento.Requisicion Or LineaDocumento.TipoMovimiento = TipoMovimiento.Trasladar Or LineaDocumento.TipoMovimiento = TipoMovimiento.Comprar Then
                            DatosLineasCotizacion(LineaDocumento, CotizacionCache, NumeroSerieCita, ConsecutivoCita)
                        End If
                        ManejaLineasCrear(LineaDocumento, CotizacionCache, RequisicionDataList, Nothing)
                    Else
                        If ValidaProcesoActualizar(Cotizacion, LineaDocumento, ConfiguracionesSucursal, CotizacionInicial) Then
                            If TipoProcesamientoActualizar(LineaDocumento) <> ProcesamientoLinea.NingunaAccion Then
                                If LineaDocumento.EsAdicional Then
                                    ValidaDisponibilidadArticulo(LineaDocumento, Articulo, ConfiguracionesSucursal, EsCancelacion)
                                End If
                                If LineaDocumento.TipoMovimiento = TipoMovimiento.Requisicion Or LineaDocumento.TipoMovimiento = TipoMovimiento.Trasladar Or LineaDocumento.TipoMovimiento = TipoMovimiento.Comprar Then
                                    DatosLineasCotizacion(LineaDocumento, CotizacionCache, NumeroSerieCita, ConsecutivoCita)
                                End If
                                ManejaLineasActualizar(LineaDocumento, Articulo, CotizacionCache, RequisicionDataList, Nothing, ConfiguracionesSucursal, EsCancelacion)
                            End If
                        End If
                    End If

                    ReplicaValorACotizacion(Cotizacion, LineaDocumento, EsCancelacion)
                End If
            Next

            If ConfiguracionesSucursal.Item(0).UsaUbicaciones = True Then
                CargaUbicaciones(RequisicionDataList)
            End If

            ListaRequisiciones = New List(Of SAPbobsCOM.GeneralData)

            If RequisicionDataList.Count > 0 Then
                ManejaRequisicion(RequisicionDataList, ListaRequisiciones, NumeroSerieCita, ConsecutivoCita)
            End If

            CrearRequisicion(ListaRequisiciones)
            'End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    Public Function ValidaPaquete(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                  ByRef p_oPaqueteListResultado As Paquete_List, _
                                  ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String)
        Try
            For Each rowPaquete As Paquete In p_oPaqueteListResultado
                If rowPaquete.ItemCode = p_rowCotizacion.ItemCode And rowPaquete.TreeType = p_rowCotizacion.TreeType Then
                    Select Case rowPaquete.AprobadoPadre
                        Case ArticuloAprobado.scgSi
                            If rowPaquete.TreeTypePadre = SAPbobsCOM.BoItemTreeTypes.iSalesTree And rowPaquete.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, NumeroSerieCita, ConsecutivoCita)
                                p_rowCotizacion.PaquetePadre = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, NumeroSerieCita, ConsecutivoCita)
                            Else
                                p_rowCotizacion.PaquetePadre = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, NumeroSerieCita, ConsecutivoCita)
                            End If
                        Case ArticuloAprobado.scgNo
                            If rowPaquete.TreeTypePadre = SAPbobsCOM.BoItemTreeTypes.iSalesTree And rowPaquete.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, NumeroSerieCita, ConsecutivoCita)
                            End If
                        Case ArticuloAprobado.scgFalta
                    End Select
                    p_oPaqueteListResultado.Remove(rowPaquete)
                    Exit For
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Sub ManejarPaquete(ByRef p_oPaqueteList As Paquete_List, _
                             ByRef p_oPaqueteListResultado As Paquete_List)
        Try
            CargarPaquete(p_oPaqueteList, p_oPaqueteListResultado)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CargarPaquete(ByRef p_oPaqueteList As Paquete_List, _
                             ByRef p_oPaqueteListResultado As Paquete_List)
        '*************Objeto SAP ***************************
        Dim oDocumentoPaquete As SAPbobsCOM.ProductTrees
        Try
            '************Data Contract ****************************
            Dim oPaqueteResultado As Paquete
            oDocumentoPaquete = CType(DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees),  _
                                          SAPbobsCOM.ProductTrees)
            For Each rowPaquetePadre As Paquete In p_oPaqueteList
                If Not String.IsNullOrEmpty(rowPaquetePadre.ItemCodePadre) Then
                    If oDocumentoPaquete.GetByKey(rowPaquetePadre.ItemCodePadre) Then
                        For cont As Integer = 0 To oDocumentoPaquete.Items.Count - 1
                            oDocumentoPaquete.Items.SetCurrentLine(cont)
                            oPaqueteResultado = New Paquete()
                            With oPaqueteResultado
                                .AprobadoPadre = rowPaquetePadre.AprobadoPadre
                                .ItemCode = oDocumentoPaquete.Items.ItemCode
                                .ItemCodePadre = rowPaquetePadre.ItemCodePadre
                                .LineNumCotizacionPadre = rowPaquetePadre.LineNumCotizacionPadre
                                .TreeTypePadre = rowPaquetePadre.TreeTypePadre
                                Select Case rowPaquetePadre.TreeTypePadre
                                    Case SAPbobsCOM.BoItemTreeTypes.iSalesTree
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient
                                    Case SAPbobsCOM.BoItemTreeTypes.iTemplateTree
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree
                                    Case Else
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree
                                End Select
                            End With
                            p_oPaqueteListResultado.Add(oPaqueteResultado)
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oDocumentoPaquete)
        End Try
    End Sub

    Public Sub RestablecerEstadosLineas(ByRef Cotizacion As SAPbobsCOM.Documents)
        Try
            For i As Integer = 0 To Cotizacion.Lines.Count - 1
                Cotizacion.Lines.SetCurrentLine(i)
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = Trasladado.NoProcesado
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function CargarCotizacionInicial(ByVal p_intDocEntry As Integer) As SAPDocumento.oDocumento
        '*****Objetos SAP *****
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*****DataContract *****
            Dim oDocumento As SAPDocumento.oDocumento
            Dim oLineasDocumento As List(Of SAPDocumento.oLineasDocumento)
            If p_intDocEntry > 0 Then
                oCotizacion = CType(DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                          SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(p_intDocEntry) Then
                    oDocumento = New SAPDocumento.oDocumento()
                    oLineasDocumento = New List(Of SAPDocumento.oLineasDocumento)()
                    For rowCotizacion As Integer = 0 To oCotizacion.Lines.Count - 1
                        oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                        With oLineasDocumento
                            .Add(New SAPDocumento.oLineasDocumento())
                            With .Item(rowCotizacion)
                                .DocEntry = oCotizacion.Lines.DocEntry
                                .LineNum = oCotizacion.Lines.LineNum
                                .ItemCode = oCotizacion.Lines.ItemCode
                                .OriginalQuantity = oCotizacion.Lines.Quantity
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                                    .IdRepxOrd = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                .AprobadoOriginal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                .TrasladadoOriginal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value.ToString()) Then
                                    .OTHija = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                    .TipoArticulo = CInt(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                                End If
                            End With
                        End With
                    Next
                    oDocumento.Lineas = oLineasDocumento
                    Return oDocumento
                End If
                Return Nothing
            End If
            Return Nothing
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Function

    Private Function ValidaProcesoActualizar(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, ByRef oCotizacionInicial As SAPDocumento.oDocumento) As Boolean
        Try
            For Each rowCotizacionInicial As SAPDocumento.oLineasDocumento In oCotizacionInicial.Lineas
                With rowCotizacionInicial
                    If p_rowCotizacion.ItemCode = .ItemCode And p_rowCotizacion.ID = .ID And p_rowCotizacion.LineNum = .LineNum AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                        If (p_rowCotizacion.Aprobado <> .AprobadoOriginal) Or p_rowCotizacion.Quantity <> .OriginalQuantity _
                            Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And p_rowCotizacion.Trasladado = Trasladado.NoProcesado) _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo And p_rowCotizacion.Trasladado = Trasladado.SI) _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo And p_rowCotizacion.Comprar = "Y") _
                            Or Not p_rowCotizacion.EmpleadoAsignado Is Nothing Then

                            p_rowCotizacion.OriginalQuantity = .OriginalQuantity
                            '******************************
                            ' Valida si pertenece a OT Hija
                            '******************************
                            If p_rowCotizacion.OTHija = enumTrasladadoOTHija.scgOTHijaNO Then
                                '*********************
                                ' Valida cantidades
                                '*********************
                                Select Case p_rowCotizacion.Quantity
                                    Case Is < .OriginalQuantity
                                        If p_oConfiguracionSucursalList.Item(0).UsuarioDisminuye Then
                                            p_rowCotizacion.OriginalQuantity = .OriginalQuantity
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                p_rowCotizacion.RequisicionDevolucion = True
                                            End If
                                        Else
                                            p_oCotizacion.Lines.Quantity = .OriginalQuantity
                                            DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.CantidadNoDisminuye + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                            Return False
                                        End If
                                    Case Is > .OriginalQuantity
                                        If p_rowCotizacion.TipoArticulo <> TipoArticulo.Servicio Then
                                            DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.LacantidadDelItem + "   " + p_rowCotizacion.ItemCode + ")    " + p_rowCotizacion.Description + My.Resources.Resource.CantidadNoAumenta + vbCrLf + My.Resources.Resource.AgregueLineaParaCantidad)
                                            p_oCotizacion.Lines.Quantity = .OriginalQuantity
                                            Return False
                                        End If
                                End Select
                                '*********************
                                ' Valida Aprobado
                                '*********************
                                Select Case p_rowCotizacion.Aprobado
                                    Case Is = .AprobadoOriginal
                                        Select Case p_rowCotizacion.TipoArticulo
                                            Case TipoArticulo.Repuesto
                                                Select Case p_rowCotizacion.Aprobado
                                                    Case ArticuloAprobado.scgSi
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf (p_rowCotizacion.Trasladado = Trasladado.NoProcesado Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado) Then
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI AndAlso p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                                            DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + ")      " + p_rowCotizacion.Description)
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        Else
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        End If
                                                    Case ArticuloAprobado.scgNo
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Comprar = "Y" And p_rowCotizacion.Quantity = p_rowCotizacion.CantidadRecibida Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        End If
                                                End Select
                                            Case TipoArticulo.Servicio
                                                If String.IsNullOrEmpty(rowCotizacionInicial.ID) And rowCotizacionInicial.IdRepxOrd = 0 Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                End If
                                            Case TipoArticulo.Suministro
                                                Select Case p_rowCotizacion.Aprobado
                                                    Case ArticuloAprobado.scgSi
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf (p_rowCotizacion.Trasladado = Trasladado.NoProcesado Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado) Then
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI AndAlso p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                                            DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + "      " + p_rowCotizacion.Description)
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        Else
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        End If
                                                    Case ArticuloAprobado.scgNo
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Comprar = "Y" And p_rowCotizacion.Quantity = p_rowCotizacion.CantidadRecibida Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        End If
                                                End Select
                                            Case TipoArticulo.ServicioExterno
                                                If p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And p_rowCotizacion.Trasladado = Trasladado.NoProcesado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                End If
                                            Case TipoArticulo.OtrosCostos
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            Case TipoArticulo.OtrosIngresos
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            Case TipoArticulo.Otros
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                        End Select
                                    Case Is <> .AprobadoOriginal
                                        If p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo Then
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                    DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion > 0 Then
                                                    DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                                DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + ")      " + p_rowCotizacion.Description)
                                                p_rowCotizacion.RequisicionDevolucion = True
                                                oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                Return True
                                            End If
                                        ElseIf p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi Then
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                    DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            End If
                                        End If
                                End Select
                                Return True
                            ElseIf p_rowCotizacion.OTHija = enumTrasladadoOTHija.scgOTHijaSI Then
                                oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.PerteneceOTHija + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                Return False
                            End If
                        End If
                        oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                        Return False
                    End If
                End With
            Next
            If p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                p_rowCotizacion.EsAdicional = True
                Return True
            End If
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function TipoProcesamientoActualizar(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento) As Integer
        Try
            With p_rowCotizacion
                '********************************
                'Se valida según tipo de articulo
                '*******************************
                Select Case CInt(p_rowCotizacion.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        If p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        Else
                                            .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                            Return ProcesamientoLinea.NingunaAccion
                                        End If

                                    Case Trasladado.NO
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.TrasladoBodega
                                        Return ProcesamientoLinea.TrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.Requisicion
                                        Return ProcesamientoLinea.Requisicion
                                End Select
                            Case ArticuloAprobado.scgNo
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                        Return ProcesamientoLinea.RequisicionDevolucion
                                    Case Trasladado.NO
                                        If .Comprar = "Y" And .CantidadRecibida = .Quantity Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        End If
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularRequisicion
                                        Return ProcesamientoLinea.AnularRequisicion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularTrasladoBodega
                                        Return ProcesamientoLinea.AnularTrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                End Select
                            Case ArticuloAprobado.scgFalta
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                            Case ArticuloAprobado.scgCambioOT
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case TipoArticulo.Servicio
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                            Case ArticuloAprobado.scgNo
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case TipoArticulo.ServicioExterno
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                If String.IsNullOrEmpty(.ID) Then
                                    .ProcesamientoLinea = ProcesamientoLinea.ProcesaServicioExterno
                                    Return ProcesamientoLinea.ProcesaServicioExterno
                                End If
                            Case ArticuloAprobado.scgNo
                                .ProcesamientoLinea = ProcesamientoLinea.AnulaServicioExterno
                                Return ProcesamientoLinea.AnulaServicioExterno
                        End Select
                    Case TipoArticulo.Suministro
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                Select Case .Trasladado
                                    Case Trasladado.SI

                                        If p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        Else
                                            .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                            Return ProcesamientoLinea.NingunaAccion
                                        End If
                                    Case Trasladado.NO
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.TrasladoBodega
                                        Return ProcesamientoLinea.TrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.Requisicion
                                        Return ProcesamientoLinea.Requisicion
                                End Select
                            Case ArticuloAprobado.scgNo
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                        Return ProcesamientoLinea.RequisicionDevolucion
                                    Case Trasladado.NO
                                        If .Comprar = "Y" And .CantidadRecibida = .Quantity Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        End If
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularRequisicion
                                        Return ProcesamientoLinea.AnularRequisicion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularTrasladoBodega
                                        Return ProcesamientoLinea.AnularTrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                End Select
                            Case ArticuloAprobado.scgFalta
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                            Case ArticuloAprobado.scgCambioOT
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case Else
                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                        Return ProcesamientoLinea.NingunaAccion
                End Select
            End With
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Sub ManejaLineasActualizar(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                      ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                      ByRef p_oCotizacionActual As SAPDocumento.oDocumento, _
                                      ByRef p_oRequisicionDataList As RequisicionData_List, _
                                      ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                      ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, ByVal EsCancelacion As Boolean)
        Try
            '*****************Variables **********
            Dim blnLineaModificada As Boolean = False
            With p_rowCotizacion
                '********************************
                'Se valida según tipo de articulo
                '*******************************
                Select Case CInt(.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.Requisicion
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                'ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    If Not .RequisicionDevolucion Then
                                        .Trasladado = Trasladado.PendienteBodega
                                        .Comprar = "N"
                                        .CantidadRecibida = 0
                                        .CantidadPendiente = 0
                                        .CantidadSolicitada = 0
                                        .CantidadPendienteBodega = .Quantity
                                        .CantidadPendienteTraslado = 0
                                        .CantidadPendienteDevolucion = 0
                                        '********************************
                                        'Carga Requisicion Data Contract
                                        '********************************
                                        AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Reserva)
                                        blnLineaModificada = True
                                    End If
                                ElseIf .TipoMovimiento = TipoMovimiento.Comprar Then
                                    .Trasladado = Trasladado.NO
                                    .Comprar = "Y"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = .Quantity
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                ElseIf .TipoMovimiento = TipoMovimiento.Rechazar Then
                                    .Aprobado = ArticuloAprobado.scgNo
                                    .Trasladado = Trasladado.NoProcesado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                ElseIf .TipoMovimiento = TipoMovimiento.Trasladar Then
                                    .Trasladado = Trasladado.PendienteTraslado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = .Quantity
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.RequisicionDevolucion
                                '.RequisicionDevolucion = True
                                If .RequisicionDevolucion Then

                                    If .OriginalQuantity = .Quantity Then
                                        .CantidadPendienteDevolucion = .Quantity
                                    Else
                                        .CantidadPendienteDevolucion = .OriginalQuantity - .Quantity
                                    End If
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = .CantidadRecibida - .CantidadPendienteDevolucion
                                    '.CantidadPendiente = .CantidadPendiente
                                    '.CantidadSolicitada = .CantidadSolicitada
                                    '.CantidadPendienteBodega = .CantidadPendienteBodega
                                    '.CantidadPendienteTraslado = .CantidadPendienteTraslado
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Devolucion)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.TrasladoBodega
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList, EsCancelacion)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Reserva)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.AnularRequisicion
                            Case ProcesamientoLinea.AnularTrasladoBodega
                                .Trasladado = Trasladado.NoProcesado
                                .Comprar = "N"
                                .CantidadRecibida = 0
                                .CantidadPendiente = 0
                                .CantidadSolicitada = 0
                                .CantidadPendienteBodega = 0
                                .CantidadPendienteTraslado = 0
                                .CantidadPendienteDevolucion = 0
                                blnLineaModificada = True
                            Case ProcesamientoLinea.AnularRequisicionDevolucion
                        End Select
                        '************************************************************************************
                        'Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        '************************************************************************************
                        If blnLineaModificada = False Then
                            .CantidadRecibida = -1
                            .CantidadPendiente = -1
                            .CantidadSolicitada = -1
                            .CantidadPendienteBodega = -1
                            .CantidadPendienteTraslado = -1
                            .CantidadPendienteDevolucion = -1
                        End If
                    Case TipoArticulo.Servicio
                        'Select Case .ProcesamientoLinea
                        '    Case ProcesamientoLinea.AgregarControlColaborador
                        '        '********************************
                        '        'Carga Control Colaborador Data Contract
                        '        '********************************
                        '        If Not String.IsNullOrEmpty(p_rowCotizacion.EmpleadoAsignado) Then
                        '            AgregarControlColaboradorDataContract(p_rowCotizacion, p_oControlColaboradorList)
                        '        End If
                        'End Select
                    Case TipoArticulo.ServicioExterno
                        'Select Case .ProcesamientoLinea
                        '    Case ProcesamientoLinea.ProcesaServicioExterno
                        '        .Trasladado = Trasladado.No
                        '        .Comprar = "Y"
                        '        .CantidadRecibida = 0
                        '        .CantidadPendiente = .Quantity
                        '        .CantidadSolicitada = 0
                        '        .CantidadPendienteBodega = 0
                        '        .CantidadPendienteTraslado = 0
                        '        .CantidadPendienteDevolucion = 0
                        '        blnLineaModificada = True
                        '    Case ProcesamientoLinea.AnulaServicioExterno
                        '        If .Quantity = .CantidadPendiente Then
                        '            .Trasladado = Trasladado.NoProcesado
                        '            .Comprar = "N"
                        '            .CantidadRecibida = 0
                        '            .CantidadPendiente = 0
                        '            .CantidadSolicitada = 0
                        '            .CantidadPendienteBodega = 0
                        '            .CantidadPendienteTraslado = 0
                        '            .CantidadPendienteDevolucion = 0
                        '            blnLineaModificada = True
                        '        End If
                        'End Select
                        ''************************************************************************************
                        ''Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        ''************************************************************************************
                        'If blnLineaModificada = False Then
                        '    .CantidadRecibida = -1
                        '    .CantidadPendiente = -1
                        '    .CantidadSolicitada = -1
                        '    .CantidadPendienteBodega = -1
                        '    .CantidadPendienteTraslado = -1
                        '    .CantidadPendienteDevolucion = -1
                        'End If
                    Case TipoArticulo.Suministro
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.Requisicion
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList, EsCancelacion)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    If Not .RequisicionDevolucion Then
                                        .Trasladado = Trasladado.PendienteBodega
                                        .Comprar = "N"
                                        .CantidadRecibida = 0
                                        .CantidadPendiente = 0
                                        .CantidadSolicitada = 0
                                        .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                        .CantidadPendienteTraslado = 0
                                        .CantidadPendienteDevolucion = 0
                                        '********************************
                                        'Carga Requisicion Data Contract
                                        '********************************
                                        AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Reserva)
                                        blnLineaModificada = True
                                    End If
                                ElseIf .TipoMovimiento = TipoMovimiento.Trasladar Then
                                    .Trasladado = Trasladado.PendienteTraslado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = .Quantity
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.RequisicionDevolucion
                                '.RequisicionDevolucion = True
                                If .RequisicionDevolucion Then

                                    If .OriginalQuantity = .Quantity Then
                                        .CantidadPendienteDevolucion = .Quantity
                                    Else
                                        .CantidadPendienteDevolucion = .OriginalQuantity - .Quantity
                                    End If

                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = .CantidadRecibida - .CantidadPendienteDevolucion
                                    '.CantidadPendiente = .CantidadPendiente
                                    '.CantidadSolicitada = .CantidadSolicitada
                                    '.CantidadPendienteBodega = .CantidadPendienteBodega
                                    '.CantidadPendienteTraslado = .CantidadPendienteTraslado

                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Devolucion)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.TrasladoBodega
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList, EsCancelacion)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.AnularRequisicion
                            Case ProcesamientoLinea.AnularTrasladoBodega
                                .Trasladado = Trasladado.NoProcesado
                                .Comprar = "N"
                                .CantidadRecibida = 0
                                .CantidadPendiente = 0
                                .CantidadSolicitada = 0
                                .CantidadPendienteBodega = 0
                                .CantidadPendienteTraslado = 0
                                .CantidadPendienteDevolucion = 0
                                blnLineaModificada = True
                            Case ProcesamientoLinea.AnularRequisicionDevolucion
                        End Select
                        '************************************************************************************
                        'Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        '************************************************************************************
                        If blnLineaModificada = False Then
                            .CantidadRecibida = -1
                            .CantidadPendiente = -1
                            .CantidadSolicitada = -1
                            .CantidadPendienteBodega = -1
                            .CantidadPendienteTraslado = -1
                            .CantidadPendienteDevolucion = -1
                        End If
                End Select
            End With
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub CancelarLineasRequisicion(ByRef oFormulario As SAPbouiCOM.Form, ByRef Cotizacion As SAPbobsCOM.Documents, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String, ByVal EsCancelacion As Boolean, ByRef ErrorProcesando As Boolean)
        Dim ListaRequisiciones As List(Of String) = New List(Of String)
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty

        Try
            If EsCancelacion Then
                SerieCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
                NumeroCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value


                Cotizacion.Update()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ObtenerListaRequisiciones(ByRef ListaRequisiciones As List(Of String))
        Dim Query As String = "SELECT T0.""DocEntry"" AS 'NumeroRequisicion', T1.""U_SCGD_DocEntry"" AS 'NumeroTransferencia' FROM ""@SCGD_REQUISICIONES"" T0 INNER JOIN ""@SCGD_MOVS_REQ"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' "
        Try

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ReplicaValorACotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                        ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, ByVal EsCancelacion As Boolean)
        Try
            '************************************
            'Asigna Valores Lineas Cotizacion
            '************************************
            With p_rowCotizacion
                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                p_oCotizacion.Lines.Quantity = .Quantity
                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.TipoArticulo.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = Convert.ToString(.TipoArticulo)
                End If
                If Not String.IsNullOrEmpty(.Sucursal) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                End If
                If Not String.IsNullOrEmpty(.Comprar) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = .Comprar
                End If
                If Not String.IsNullOrEmpty(.FaseProduccion) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = .FaseProduccion
                End If
                If Not String.IsNullOrEmpty(.CentroCosto) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                End If
                If Not String.IsNullOrEmpty(.IdRepxOrd.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = .IdRepxOrd
                End If
                If Not String.IsNullOrEmpty(.ID) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                End If
                If Not String.IsNullOrEmpty(.OTHija.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                End If
                If Not String.IsNullOrEmpty(.DuracionEstandar.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
                Else
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = 0
                End If
                If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                End If
                If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                End If
                If Not String.IsNullOrEmpty(.EstadoActividad) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                End If
                If Not String.IsNullOrEmpty(.PaquetePadre) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = .PaquetePadre
                End If
                If Not String.IsNullOrEmpty(.Resultado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = .Resultado
                End If

                If .CantidadRecibida <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                If .CantidadSolicitada <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                If .CantidadPendiente <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                If .CantidadPendienteBodega <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                If .CantidadPendienteTraslado <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                If .CantidadPendienteDevolucion <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion

                If String.IsNullOrEmpty(.NoOrden) Then
                    'Todas las líneas procesadas previo a la orden de trabajo llevan el indicador de reserva (Prepicking)
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Prepicking").Value = "Y"
                End If

                If EsCancelacion Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = Trasladado.NoProcesado
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                End If

            End With
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub DatosLineasCotizacion(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, ByRef p_oCotizacionActual As SAPDocumento.oDocumento, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String)
        Try
            If String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                p_rowCotizacion.NoOrden = p_oCotizacionActual.NoOrden
            End If
            '********************************
            'Se valida según tipo de articulo
            '********************************
            If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                Select Case CInt(p_rowCotizacion.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                    Case TipoArticulo.Servicio
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                        If String.IsNullOrEmpty(p_rowCotizacion.EstadoActividad) Then
                            p_rowCotizacion.EstadoActividad = "1"
                        End If
                    Case TipoArticulo.ServicioExterno
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                        p_rowCotizacion.Comprar = "Y"
                    Case TipoArticulo.Suministro
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                    Case TipoArticulo.Paquete
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ValidaDisponibilidadArticulo(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                            ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                            ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, ByVal EsCancelacion As Boolean)
        Try
            '********Variables *****************
            Dim dblStockDisponible As Double = 0
            Dim intTipoMovimiento As Integer = 0
            '********************************
            'Se valida stock disponible
            '*******************************
            Select Case CInt(p_rowCotizacion.TipoArticulo)
                Case TipoArticulo.Repuesto
                    If EsCancelacion AndAlso p_rowCotizacion.Trasladado = Trasladado.SI Then
                        dblStockDisponible = ArticuloEnStock(p_oArticulo, p_rowCotizacion.BodegaReservas)
                    Else
                        dblStockDisponible = ArticuloEnStock(p_oArticulo, p_rowCotizacion.BodegaRepuesto)
                    End If

                    p_rowCotizacion.CantidadStock = dblStockDisponible
                    If dblStockDisponible < p_rowCotizacion.Quantity Then
                        'If Not EsCancelacion AndAlso p_rowCotizacion.Aprobado <> Aprobado.No AndAlso String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                        '    DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.El_Item & p_rowCotizacion.ItemCode & " " & p_rowCotizacion.Description & My.Resources.Resource.NoHayStock, 1, "OK")
                        'End If

                        'p_rowCotizacion.TipoMovimiento = TipoMovimiento.Rechazar
                        intTipoMovimiento = DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.El_Item & p_rowCotizacion.ItemCode & " " & p_rowCotizacion.Description & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar, My.Resources.Resource.Trasladar)
                        If intTipoMovimiento > 0 Then
                            Select Case intTipoMovimiento
                                Case 1
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Comprar
                                Case 2
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Rechazar
                                Case 3
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Trasladar
                            End Select
                        End If
                    Else
                        If p_oConfiguracionSucursal.Item(0).UsaRequisiciones = True Then
                            p_rowCotizacion.TipoMovimiento = TipoMovimiento.Requisicion
                        End If
                    End If
                Case TipoArticulo.Suministro
                    dblStockDisponible = ArticuloEnStock(p_oArticulo, p_rowCotizacion.BodegaSuministro)
                    p_rowCotizacion.CantidadStock = dblStockDisponible
                    If dblStockDisponible < p_rowCotizacion.Quantity Then
                        p_rowCotizacion.TipoMovimiento = TipoMovimiento.Trasladar
                    Else
                        If p_oConfiguracionSucursal.Item(0).UsaRequisiciones = True Then
                            p_rowCotizacion.TipoMovimiento = TipoMovimiento.Requisicion
                        End If
                    End If
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function ArticuloEnStock(ByRef p_oArticulo As SAPbobsCOM.IItems, ByRef p_strBodegaOrigen As String) As Double
        Try
            Dim oItemWhsInfo As SAPbobsCOM.IItemWarehouseInfo
            Dim dblStock As Double = 0
            Dim contador As Integer

            oItemWhsInfo = p_oArticulo.WhsInfo

            For contador = 0 To oItemWhsInfo.Count - 1
                With oItemWhsInfo
                    .SetCurrentLine(contador)
                    If .WarehouseCode = p_strBodegaOrigen Then
                        dblStock = .InStock - .Committed
                        Exit For
                    End If
                End With
            Next
            Return dblStock
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function CrearRequisicion(ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData)) As Boolean
        Try
            Dim oControladorRequisicion As ControladorRequisicion = New ControladorRequisicion(DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO)
            If Not p_oListaRequisicionGeneralData Is Nothing Then
                If p_oListaRequisicionGeneralData.Count > 0 Then
                    Return oControladorRequisicion.CrearRequisicionGeneralData(p_oListaRequisicionGeneralData, False)
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function CancelarRequisicionesPendientes(ByVal DocEntryCotizacion As String, ByVal SerieCita As String, ByVal NumeroCita As String) As Boolean
        Dim Query As String = " SELECT T0.""DocEntry"" FROM ""@SCGD_REQUISICIONES"" T0 WHERE T0.""U_SCGD_NoOrden"" IS NULL AND T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' AND T0.""U_SCGD_CodEst"" = '1' "
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim Resultado As Boolean = False
        Dim DocEntry As String = String.Empty
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim sCmp As SAPbobsCOM.CompanyService
        Dim LineasRequisicion As SAPbobsCOM.GeneralDataCollection
        Dim LineaRequisicion As SAPbobsCOM.GeneralData
        Dim ListaRequisiciones As List(Of String) = New List(Of String)
        Dim oDocumento As SAPbobsCOM.Documents
        Dim BaseDoc As String = String.Empty
        Dim BaseDocAnterior As String = String.Empty
        Dim BaseLine As String = String.Empty
        Dim ActualizarCotizacion As Boolean = False
        Dim CodigoError As Integer = 0
        Dim DescripcionError As String = String.Empty
        Dim EsError As Boolean = False
        Try
            If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                oDocumento = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations)
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset)
                Query = String.Format(Query, SerieCita, NumeroCita)
                oRecordset.DoQuery(Query)
                sCmp = DMS_Connector.Company.CompanySBO.GetCompanyService
                oGeneralService = sCmp.GetGeneralService("SCGD_REQ")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

                'Iniciar la transacción
                DMS_Connector.Company.CompanySBO.StartTransaction()

                While Not oRecordset.EoF
                    DocEntry = oRecordset.Fields.Item("DocEntry").Value.ToString()
                    oGeneralParams.SetProperty("DocEntry", DocEntry)
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                    'Encabezados
                    oGeneralData.SetProperty("U_SCGD_CodEst", "3")
                    oGeneralData.SetProperty("U_SCGD_Est", My.Resources.Resource.Cancelado)

                    'Lineas
                    LineasRequisicion = oGeneralData.Child("SCGD_LINEAS_REQ")
                    BaseDoc = LineasRequisicion.Item(0).GetProperty("U_SCGD_DocOr")

                    If String.IsNullOrEmpty(BaseDocAnterior) Then
                        oDocumento.GetByKey(BaseDoc)
                    Else
                        If Not BaseDoc = BaseDocAnterior Then
                            CodigoError = oDocumento.Update()
                            If CodigoError <> 0 Then
                                EsError = True
                                DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            End If
                            oDocumento.GetByKey(BaseDoc)
                        End If
                    End If

                    BaseDocAnterior = BaseDoc

                    For i As Integer = 0 To LineasRequisicion.Count - 1
                        LineaRequisicion = LineasRequisicion.Item(i)
                        BaseLine = LineaRequisicion.GetProperty("U_SCGD_LNumOr")
                        'oDocumento.Lines.SetCurrentLine(BaseLine)
                        LineaRequisicion.SetProperty("U_SCGD_CantPen", "0")
                        LineaRequisicion.SetProperty("U_SCGD_CodEst", "3")
                        LineaRequisicion.SetProperty("U_SCGD_Estado", My.Resources.Resource.Cancelado)
                        LineaRequisicion.SetProperty("U_TipoM", "2")
                        LineaRequisicion.SetProperty("U_FechaM", DateTime.Now)
                        LineaRequisicion.SetProperty("U_HoraM", DateTime.Now)
                        For j As Integer = 0 To oDocumento.Lines.Count - 1
                            oDocumento.Lines.SetCurrentLine(j)
                            If oDocumento.Lines.LineNum = BaseLine Then
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = Aprobado.No
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = Trasladado.NoProcesado
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                oDocumento.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                Exit For
                            End If
                        Next
                    Next
                    oGeneralService.Update(oGeneralData)
                    oRecordset.MoveNext()
                    ActualizarCotizacion = True
                End While

                If ActualizarCotizacion Then
                    CodigoError = oDocumento.Update()
                End If

                If CodigoError <> 0 Then
                    EsError = True
                    DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If

                If EsError Then
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    Return False
                Else
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            If DMS_Connector.Company.CompanySBO.InTransaction Then
                DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function ValidaArticulo(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                   ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                   ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List, _
                                   ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, _
                                   ByRef p_blnMensajeCCOT As Boolean, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String) As Boolean
        Try
            '*******Variables ******
            Dim intTipoArticulo As Integer = 0
            Dim strCentroCosto As String = String.Empty
            Dim strFaseProduccion As String = String.Empty

            If Not String.IsNullOrEmpty(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value) Then
                intTipoArticulo = CInt(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value)
            End If
            '********************************
            'Se valida según tipo de articulo
            '*******************************
            Select Case intTipoArticulo
                Case TipoArticulo.Repuesto
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaRepuesto = row.BodegaRepuesto
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    p_rowCotizacion.BodegaOrigen = row.BodegaRepuesto
                                    p_rowCotizacion.BodegaDestino = row.BodegaReservas
                                    'p_rowCotizacion.BodegaDestino = row.BodegaProceso
                                    p_rowCotizacion.BodegaReservas = row.BodegaReservas
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.Servicio
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        strFaseProduccion = p_oArticulo.UserFields.Fields.Item("U_SCGD_T_Fase").Value.ToString.Trim()
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) And Not String.IsNullOrEmpty(strFaseProduccion) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaServicio = row.BodegaServicio
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    If p_rowCotizacion.DuracionEstandar = 0 Then
                                        p_rowCotizacion.DuracionEstandar = p_oArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value
                                    End If
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Servicio
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.ServicioExterno
                    If p_oConfiguracionSucursal.Item(0).UsaServiciosExternosInventariables = True Then
                        If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                            If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                                strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                                p_blnMensajeCCOT = True
                            Else
                                strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                            End If
                            If Not String.IsNullOrEmpty(strCentroCosto) Then
                                For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                    If row.CentroCosto = strCentroCosto Then
                                        p_rowCotizacion.BodegaServicioExterno = row.BodegaServicioExterno
                                        p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                        p_rowCotizacion.CentroCosto = row.CentroCosto
                                        p_rowCotizacion.TipoArticulo = TipoArticulo.ServicioExterno
                                        p_rowCotizacion.Procesar = True
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                        Return False
                    Else
                        If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                            If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                                strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                                p_blnMensajeCCOT = True
                            Else
                                strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                            End If
                            If Not String.IsNullOrEmpty(strCentroCosto) Then
                                For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                    If row.CentroCosto = strCentroCosto Then
                                        p_rowCotizacion.BodegaServicioExterno = row.BodegaServicioExterno
                                        p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                        p_rowCotizacion.CentroCosto = row.CentroCosto
                                        p_rowCotizacion.TipoArticulo = TipoArticulo.ServicioExterno
                                        p_rowCotizacion.Procesar = True
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                        Return False
                    End If
                Case TipoArticulo.Suministro
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaSuministro = row.BodegaSuministro
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    p_rowCotizacion.BodegaOrigen = row.BodegaSuministro
                                    'p_rowCotizacion.BodegaDestino = row.BodegaProceso
                                    p_rowCotizacion.BodegaDestino = row.BodegaReservas
                                    p_rowCotizacion.BodegaReservas = row.BodegaReservas
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.Paquete
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.OtrosCostos
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                        p_rowCotizacion.TipoArticulo = TipoArticulo.OtrosCostos
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.OtrosIngresos
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                       And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                       And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                        End If
                        p_rowCotizacion.TipoArticulo = TipoArticulo.OtrosIngresos
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.Otros
                    If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                        p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, NumeroSerieCita, ConsecutivoCita)
                    End If
                    p_rowCotizacion.TipoArticulo = TipoArticulo.Otros
                    p_rowCotizacion.Procesar = False
                    Return True
                Case TipoArticulo.ArticuloCita
                    p_rowCotizacion.NoOrden = String.Empty
                    p_rowCotizacion.TipoArticulo = TipoArticulo.ArticuloCita
                    p_rowCotizacion.Procesar = False
                    Return True
                Case Else
                    Return False
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub CargaUbicaciones(ByRef p_oRequisicionDataLineas As RequisicionData_List)
        Try
            For Each rowRequisicion As RequisicionData In p_oRequisicionDataLineas
                If rowRequisicion.RequisicionDevolucion Then
                    rowRequisicion.BodegaUbicacion = rowRequisicion.BodegaDestino
                Else
                    rowRequisicion.BodegaUbicacion = rowRequisicion.BodegaOrigen
                End If
            Next
            CargaUbicacionesDefecto(p_oRequisicionDataLineas, DMS_Connector.Company.CompanySBO)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CargaUbicacionDefectoAlmacen(ByRef p_rowRequisicion As RequisicionData, _
                                           ByRef p_oCompany As SAPbobsCOM.Company)
        '***** Objetos SAP 
        Dim oIWarehouses As IWarehouses
        Try
            oIWarehouses = p_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)
            If oIWarehouses.GetByKey(p_rowRequisicion.BodegaUbicacion) Then
                If oIWarehouses.EnableBinLocations = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If oIWarehouses.DefaultBin > 0 Then
                        If p_rowRequisicion.RequisicionDevolucion Then
                            p_rowRequisicion.UbicacionDestino = oIWarehouses.DefaultBin.ToString().Trim()
                            p_rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oIWarehouses.DefaultBin)
                        Else
                            p_rowRequisicion.UbicacionOrigen = oIWarehouses.DefaultBin.ToString().Trim()
                            p_rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oIWarehouses.DefaultBin)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIWarehouses)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIWarehouses)
        End Try
    End Sub

    Public Function CargaBinCode(ByRef p_intAbsEntry As Integer) As String
        '****** Variable ***********
        Dim strBinCode As String = String.Empty
        Try
            If p_intAbsEntry > 0 Then
                strBinCode = Utilitarios.EjecutarConsulta(String.Format("SELECT ""BinCode"" FROM ""OBIN"" WHERE ""AbsEntry"" = {0}", p_intAbsEntry))
            End If
            Return strBinCode
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    Public Sub CargaUbicacionesDefecto(ByRef p_oRequisicionDataLineas As RequisicionData_List,
                                       ByRef p_oCompany As SAPbobsCOM.Company)
        '************Explicacion **************
        ' La jerarquia en SAP para ubicaciones es la siguiente 
        'Default Bin Location of Item > Default Bin Location of Item Group > Default Bin Location of Warehouse
        '***** Objetos SAP *****
        Dim oArticulo As SAPbobsCOM.IItems
        Try
            '***** Variables *****
            Dim blnSiguienteJerarquia As Boolean = False

            oArticulo = p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            For Each rowRequisicion As RequisicionData In p_oRequisicionDataLineas
                blnSiguienteJerarquia = False
                If oArticulo.GetByKey(rowRequisicion.ItemCode) Then
                    If oArticulo.WhsInfo.Count > 0 Then
                        For cont As Integer = 0 To oArticulo.WhsInfo.Count - 1
                            oArticulo.WhsInfo.SetCurrentLine(cont)
                            If oArticulo.WhsInfo.WarehouseCode = rowRequisicion.BodegaUbicacion Then
                                If oArticulo.WhsInfo.DefaultBin > 0 Then
                                    If rowRequisicion.RequisicionDevolucion Then
                                        rowRequisicion.UbicacionDestino = oArticulo.WhsInfo.DefaultBin.ToString().Trim()
                                        rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oArticulo.WhsInfo.DefaultBin)
                                    Else
                                        rowRequisicion.UbicacionOrigen = oArticulo.WhsInfo.DefaultBin.ToString().Trim()
                                        rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oArticulo.WhsInfo.DefaultBin)
                                    End If
                                    blnSiguienteJerarquia = False
                                Else
                                    blnSiguienteJerarquia = True
                                End If
                                Exit For
                            End If
                        Next
                    Else
                        blnSiguienteJerarquia = True
                    End If
                    If blnSiguienteJerarquia Then
                        If oArticulo.ItemsGroupCode > 0 Then
                            blnSiguienteJerarquia = CargaUbicacionDefectoGrupoArticulo(CInt(oArticulo.ItemsGroupCode), rowRequisicion, p_oCompany)
                            If blnSiguienteJerarquia Then
                                CargaUbicacionDefectoAlmacen(rowRequisicion, p_oCompany)
                            End If
                        Else
                            CargaUbicacionDefectoAlmacen(rowRequisicion, p_oCompany)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oArticulo)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oArticulo)
        End Try
    End Sub

    Public Function CargaUbicacionDefectoGrupoArticulo(ByRef p_intGroupCode As Integer,
                                                       ByRef p_rowRequisicion As RequisicionData, _
                                                       ByRef p_oCompany As SAPbobsCOM.Company) As Boolean
        '***** Objetos SAP 
        Dim oIItemGroup As IItemGroups
        Dim oBodega As SAPbobsCOM.Warehouses
        Try
            oIItemGroup = p_oCompany.GetBusinessObject(BoObjectTypes.oItemGroups)
            oBodega = p_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)

            If oBodega.GetByKey(p_rowRequisicion.BodegaUbicacion) AndAlso oBodega.EnableBinLocations = BoYesNoEnum.tYES Then
                If oIItemGroup.GetByKey(p_intGroupCode) Then
                    If oIItemGroup.WarehouseInfo.Count > 0 Then
                        For cont As Integer = 0 To oIItemGroup.WarehouseInfo.Count - 1
                            oIItemGroup.WarehouseInfo.SetCurrentLine(cont)
                            If oIItemGroup.WarehouseInfo.WarehouseCode = p_rowRequisicion.BodegaUbicacion Then
                                If oIItemGroup.WarehouseInfo.DefaultBin > 0 Then
                                    If p_rowRequisicion.RequisicionDevolucion Then
                                        p_rowRequisicion.UbicacionDestino = oIItemGroup.WarehouseInfo.DefaultBin.ToString().Trim()
                                        p_rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oIItemGroup.WarehouseInfo.DefaultBin)
                                    Else
                                        p_rowRequisicion.UbicacionOrigen = oIItemGroup.WarehouseInfo.DefaultBin.ToString().Trim()
                                        p_rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oIItemGroup.WarehouseInfo.DefaultBin)
                                    End If
                                    Return False
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIItemGroup)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIItemGroup)
        End Try
    End Function

    Private Function AsignaValorACotizacionDataContract(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, ByVal EsCancelacion As Boolean) As Boolean
        Try
            With p_rowCotizacion
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                .DocEntry = p_oCotizacion.Lines.DocEntry
                .LineNum = p_oCotizacion.Lines.LineNum
                .ItemCode = p_oCotizacion.Lines.ItemCode
                .Quantity = p_oCotizacion.Lines.Quantity
                .TreeType = p_oCotizacion.Lines.TreeType
                .VisOrder = p_oCotizacion.Lines.VisualOrder
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.ItemDescription) Then
                    .Description = p_oCotizacion.Lines.ItemDescription
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                    .IdRepxOrd = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                    .ID = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                End If
                .Aprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                If EsCancelacion Then
                    .Aprobado = ArticuloAprobado.scgNo
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                End If
                .Trasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value) Then
                    .Comprar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                    .OTHija = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                    .DuracionEstandar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                Else
                    .DuracionEstandar = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                    .EmpleadoAsignado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                    .NombreEmpleado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                    .EstadoActividad = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                    .TipoArticulo = CInt(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                End If
                .CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                .CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                .CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                .CantidadPendienteBodega = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value
                .CantidadPendienteTraslado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value
                .CantidadPendienteDevolucion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
            End With
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub ManejaLineasCrear(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                 ByRef p_oCotizacionActual As SAPDocumento.oDocumento, _
                                 ByRef p_oRequisicionDataList As RequisicionData_List, _
                                 ByRef p_oControlColaboradorList As ControlColaborador_List)
        Try
            '********************************
            'Se valida según tipo de articulo
            '*******************************
            Select Case CInt(p_rowCotizacion.TipoArticulo)
                Case TipoArticulo.Repuesto
                    Select Case p_rowCotizacion.TipoMovimiento
                        Case TipoMovimiento.Requisicion
                            p_rowCotizacion.Trasladado = Trasladado.PendienteBodega
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                            '********************************
                            'Carga Requisicion Data Contract
                            '********************************
                            AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Reserva)
                        Case TipoMovimiento.Comprar
                            p_rowCotizacion.Trasladado = Trasladado.NO
                            p_rowCotizacion.Comprar = "Y"
                            p_rowCotizacion.Resultado = "Para Comprar"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = 0
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                        Case TipoMovimiento.Trasladar
                            'p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado
                            'p_rowCotizacion.Comprar = "N"
                            'p_rowCotizacion.CantidadRecibida = 0
                            'p_rowCotizacion.CantidadPendiente = 0
                            'p_rowCotizacion.CantidadSolicitada = 0
                            'p_rowCotizacion.CantidadPendienteBodega = 0
                            'p_rowCotizacion.CantidadPendienteTraslado = p_rowCotizacion.Quantity
                            'p_rowCotizacion.CantidadPendienteDevolucion = 0
                        Case TipoMovimiento.Rechazar
                            p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo
                            p_rowCotizacion.Trasladado = Trasladado.NoProcesado
                    End Select
                Case TipoArticulo.Servicio
                    ''***************************************
                    ''Carga Control Colaborador Data Contract
                    ''***************************************
                    'If Not String.IsNullOrEmpty(p_rowCotizacion.EmpleadoAsignado) Then
                    '    AgregarControlColaboradorDataContract(p_rowCotizacion, p_oControlColaboradorList)
                    'End If
                Case TipoArticulo.ServicioExterno
                    'p_rowCotizacion.Trasladado = Trasladado.No
                    'p_rowCotizacion.Comprar = "Y"
                    'p_rowCotizacion.CantidadRecibida = 0
                    'p_rowCotizacion.CantidadPendiente = p_rowCotizacion.Quantity
                    'p_rowCotizacion.CantidadSolicitada = 0
                    'p_rowCotizacion.CantidadPendienteBodega = 0
                    'p_rowCotizacion.CantidadPendienteTraslado = 0
                    'p_rowCotizacion.CantidadPendienteDevolucion = 0
                Case TipoArticulo.Suministro
                    Select Case p_rowCotizacion.TipoMovimiento
                        Case TipoMovimiento.Requisicion
                            p_rowCotizacion.Trasladado = Trasladado.PendienteBodega
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                            '********************************
                            'Carga Requisicion Data Contract
                            '********************************
                            AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Reserva)
                        Case Else
                            p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = 0
                            p_rowCotizacion.CantidadPendienteTraslado = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                    End Select
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AgregarRequisicionDataContract(ByRef p_rowCotizacion As SAPDocumento.oLineasDocumento, _
                                              ByRef p_rowCotizacionActual As SAPDocumento.oDocumento, _
                                              ByRef p_oRequisicionDataLineas As RequisicionData_List,
                                              ByRef p_intTipoRequisicion As Integer)
        Try
            '********************Variables ****************************
            Dim oRequisicionData As RequisicionData = New RequisicionData()
            With oRequisicionData
                .TipoArticulo = p_rowCotizacion.TipoArticulo
                '*****************************
                'Datos encabezado
                '*****************************
                .NoOrden = p_rowCotizacionActual.NoOrden
                .CodigoCliente = p_rowCotizacionActual.CardCode
                .NombreCliente = p_rowCotizacionActual.CardName
                .CodigoTipoRequisicion = p_intTipoRequisicion
                If p_intTipoRequisicion = TipoRequisicion.Devolucion Then
                    .CodigoTipoRequisicion = TipoRequisicion.DevolucionReserva
                End If
                .TipoDocumento = My.Resources.Resource.DocGeneraReq
                .Usuario = DMS_Connector.Company.CompanySBO.UserName
                .Comentario = My.Resources.Resource.OT_Referencia & p_rowCotizacionActual.NoOrden & " " & My.Resources.Resource.Asesor & p_rowCotizacionActual.CodigoAsesor
                .Data = String.Empty
                .SucursalID = p_rowCotizacionActual.Sucursal
                .CodigoEstadoRequisicion = CodigoEstadoRequisicion.Pendiente
                .EstadoRequisicion = My.Resources.Resource.Pendiente
                '*****************************
                'Datos lineas
                '*****************************
                .ItemCode = p_rowCotizacion.ItemCode
                If Not String.IsNullOrEmpty(p_rowCotizacion.Description) Then
                    .Description = p_rowCotizacion.Description
                End If
                .TipoArticulo = p_rowCotizacion.TipoArticulo
                .CentroCosto = p_rowCotizacion.CentroCosto
                .CodigoEstadoLinea = CodigoEstadoRequisicion.Pendiente
                .EstadoLinea = My.Resources.Resource.Pendiente
                .LineNumOrigen = p_rowCotizacion.LineNum
                .DocumentoOrigen = p_rowCotizacionActual.DocEntry
                .LineaSucursalID = p_rowCotizacionActual.Sucursal
                .ID = p_rowCotizacion.ID
                Select Case p_intTipoRequisicion
                    Case TipoRequisicion.Traslado
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                .BodegaOrigen = p_rowCotizacion.BodegaRepuesto
                                '.BodegaDestino = p_rowCotizacion.BodegaProceso
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                .BodegaOrigen = p_rowCotizacion.BodegaSuministro
                                '.BodegaDestino = p_rowCotizacion.BodegaProceso
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.RequisicionReserva
                        .RequisicionDevolucion = False
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadRecibida = 0
                        If DMS_Connector.Company.CompanySBO.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                    Case TipoRequisicion.Devolucion
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                '.BodegaOrigen = p_rowCotizacion.BodegaProceso
                                .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                .BodegaDestino = p_rowCotizacion.BodegaRepuesto
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                '.BodegaOrigen = p_rowCotizacion.BodegaProceso
                                .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                .BodegaDestino = p_rowCotizacion.BodegaSuministro
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        '.TipoRequisicion = My.Resources.Resource.Devolucion
                        .TipoRequisicion = My.Resources.Resource.DevolucionReserva
                        .RequisicionDevolucion = True
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadRecibida = 0
                        If DMS_Connector.Company.CompanySBO.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                    Case TipoRequisicion.Reserva
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                .BodegaOrigen = p_rowCotizacion.BodegaRepuesto
                                '.BodegaDestino = p_rowCotizacion.BodegaProceso
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                .BodegaOrigen = p_rowCotizacion.BodegaSuministro
                                '.BodegaDestino = p_rowCotizacion.BodegaProceso
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.RequisicionReserva
                        .RequisicionDevolucion = False
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadRecibida = 0
                        If DMS_Connector.Company.CompanySBO.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                End Select
            End With
            p_oRequisicionDataLineas.Add(oRequisicionData)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejaRequisicion(ByRef p_oRequisicionDataList As RequisicionData_List, _
                                 ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData), ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String)
        Try
            '******Data Contract *************
            Dim oControladorRequisicion As ControladorRequisicion
            Dim oRequisicionData As RequisicionData
            Dim oRequisicionDataList As RequisicionData_List
            Dim blnProcesar As Boolean = False

            If p_oRequisicionDataList.Count > 0 Then
                p_oListaRequisicionGeneralData = New List(Of SAPbobsCOM.GeneralData)
                oControladorRequisicion = New ControladorRequisicion(DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO)
                For Each rowRequisicion1 As RequisicionData In p_oRequisicionDataList
                    oRequisicionDataList = New RequisicionData_List()
                    For Each rowRequisicion2 As RequisicionData In p_oRequisicionDataList
                        If Not rowRequisicion2.Aplicado Then
                            If rowRequisicion1.TipoArticulo = rowRequisicion2.TipoArticulo And rowRequisicion1.TipoRequisicion = rowRequisicion2.TipoRequisicion Then
                                oRequisicionData = New RequisicionData()
                                rowRequisicion2.Aplicado = True
                                oRequisicionData = rowRequisicion2
                                oRequisicionDataList.Add(oRequisicionData)
                                blnProcesar = True
                            End If
                        End If
                    Next
                    If blnProcesar Then
                        oControladorRequisicion.CrearRequisicion(oRequisicionDataList, p_oListaRequisicionGeneralData, NumeroSerieCita, ConsecutivoCita)
                        blnProcesar = False
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CargarCotizacionActual(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oCotizacionActual As SAPDocumento.oDocumento, ByRef p_oPaqueteList As Paquete_List, ByVal EsCancelacion As Boolean)
        '*****Objetos SAP *****
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
        Dim strNumeroSerieCita As String = String.Empty
        Dim strNumeroCita As String = String.Empty

        Try
            '***********Data Contract ************************
            Dim oPaquete As Paquete
            '**********************************
            'Carga Encabezado de la Cotizacion
            '**********************************
            With p_oCotizacionActual
                .DocEntry = p_oCotizacion.DocEntry
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .IDSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
                    .GeneraOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value) Then
                    .EstadoCotizacionID = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value) Then
                    .GeneraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
                    .OTPadre = p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
                    .NoOTReferencia = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
                    .NumeroVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value) Then
                    .NumeroVehiculo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                    .CodigoUnidad = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.DocumentsOwner.ToString()) Then
                    .CodigoAsesor = p_oCotizacion.DocumentsOwner
                Else
                    .CodigoAsesor = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                    .TipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                Else
                    .TipoOT = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
                    .CodigoProyecto = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                End If
                .CotizacionCancelled = p_oCotizacion.Cancelled
                .CotizacionDocumentStatus = p_oCotizacion.DocumentStatus
                .CardCode = p_oCotizacion.CardCode
                .CardName = p_oCotizacion.CardName
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
                    .NoVisita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value) Then
                    strNumeroSerieCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()
                    strNumeroCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value.ToString.Trim()
                    .NoSerieCita = strNumeroSerieCita
                    .NoCita = strNumeroCita
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
                    .Cono = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
                    .Year = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
                    .DescripcionMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
                    .DescripcionModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
                    .DescripcionEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
                    .CodigoMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
                    .CodigoEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
                    .CodigoModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString.Trim()) Then
                    .Kilometraje = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
                    .Placa = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
                    .NombreClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
                    .CodigoClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value Is Nothing Then
                    .FechaRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value Is Nothing Then
                    .HoraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value Is Nothing Then
                    .FechaCompromiso = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value Is Nothing Then
                    .HoraCompromiso = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString.Trim()) Then
                    .NivelGasolina = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Comments) Then
                    .Observaciones = p_oCotizacion.Comments
                End If
                'If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value) Then
                '    .Observaciones = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value.ToString.Trim()
                'End If
                .FechaCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value
                .HoraCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value
                .HorasServicio = Convert.ToDouble(p_oCotizacion.UserFields.Fields.Item("U_SCGD_HoSr").Value)
            End With
            'If p_oCotizacionActual.GeneraRecepcion = ImprimeOR.SI Then p_blnImprimeReporteRecepcion = True
            For rowCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                '********************************
                'Carga Paquete Data Contract
                '********************************
                If p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree _
                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree _
                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iProductionTree Then
                    oPaquete = New Paquete()
                    With oPaquete
                        .ItemCodePadre = p_oCotizacion.Lines.ItemCode
                        .TreeTypePadre = p_oCotizacion.Lines.TreeType
                        .AprobadoPadre = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                        If EsCancelacion Then
                            .AprobadoPadre = ArticuloAprobado.scgNo
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                        End If
                        .LineNumCotizacionPadre = p_oCotizacion.Lines.LineNum
                    End With
                    p_oPaqueteList.Add(oPaquete)
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
            Utilitarios.DestruirObjeto(oBusinessPartner)
        End Try
    End Sub

    Private Function CargaConfiguracionSucursal(ByVal p_oCotizacionActual As SAPDocumento.oDocumento, ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List) As Boolean
        Try
            '*********Data Contract ************
            Dim oConfiguracionSucursal As ConfiguracionSucursal = New ConfiguracionSucursal()
            '*********Variables ************
            Dim strCentroCostoPorTipoOT As String = String.Empty
            '*********Objetos System ************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            'Obtiene la configuración por sucursal OT
            oDataTableConfiguracionSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(p_oCotizacionActual.IDSucursal, DMS_Connector.Company.CompanySBO)
            For Each oDataRowConfiguracionSucursal In oDataTableConfiguracionSucursal.Rows
                With oConfiguracionSucursal
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaOfeVenta")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaOfeVenta") = "Y" Then
                            .UsaOfertaCompra = True
                            .UsaOrdenCompra = False
                        Else
                            .UsaOfertaCompra = False
                            .UsaOrdenCompra = True
                        End If
                    ElseIf Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaOrdVenta")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaOrdVenta") = "Y" Then
                            .UsaOfertaCompra = False
                            .UsaOrdenCompra = True
                        Else
                            .UsaOfertaCompra = True
                            .UsaOrdenCompra = False
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Requis")) Then
                        If oDataRowConfiguracionSucursal.Item("U_Requis") = "Y" Then
                            .UsaRequisiciones = True
                        Else
                            .UsaRequisiciones = False
                        End If
                    Else
                        .UsaRequisiciones = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_AsigAutCol")) Then
                        'Verifico el valor para RealizarAsignacionAutomaticaColaborador
                        If oDataRowConfiguracionSucursal.Item("U_AsigAutCol") = "Y" Then
                            .AsignacionAutomaticaColaborador = True
                        Else
                            .AsignacionAutomaticaColaborador = False
                        End If
                    Else
                        .AsignacionAutomaticaColaborador = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SEInvent")) Then
                        'Verifico el valor para Servicios Externos Inventariables
                        If oDataRowConfiguracionSucursal.Item("U_SEInvent") = "Y" Then
                            .UsaServiciosExternosInventariables = True
                        Else
                            .UsaServiciosExternosInventariables = False
                        End If
                    Else
                        .UsaServiciosExternosInventariables = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                        'Verifico el valor para SerieNumeracionTransferencia
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                            .SerieNumeracionTrasnferencia = oDataRowConfiguracionSucursal.Item("U_SerInv")
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                        'Verifico el valor para Numero de copias
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                            .CantidadCopiasOT = oDataRowConfiguracionSucursal.Item("U_CopiasOT")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                        'Verifico el valor para Bodega Repuesto
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                            .BodegaRepuesto = oDataRowConfiguracionSucursal.Item("U_NoBodRep")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                        'Verifico el valor para Bodega Proceso
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                            .BodegaProceso = oDataRowConfiguracionSucursal.Item("U_NoBodPro")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                        'Verifico el valor para Bodega Servicios Externos
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                            .BodegaServicioExterno = oDataRowConfiguracionSucursal.Item("U_NoBodSE")
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                        'Verifico el valor para Bodega Suministros
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                            .BodegaSuministro = oDataRowConfiguracionSucursal.Item("U_NoBodSum")
                        End If
                    End If
                End With
                '*************************************
                'Carga Centro de Costo por OT
                '*************************************
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal) Then
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal).Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code = p_oCotizacionActual.TipoOT) Then
                        strCentroCostoPorTipoOT = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal).Configuracion_Tipo_Orden.First(Function(tipoOT) tipoOT.U_Code = p_oCotizacionActual.TipoOT).U_CodCtCos.Trim()
                    End If
                End If
                If Not String.IsNullOrEmpty(strCentroCostoPorTipoOT) Then
                    oConfiguracionSucursal.CentroCostoTipoOT = strCentroCostoPorTipoOT.Trim()
                End If
                '*************************************
                'Usa Ubicaciones
                '*************************************
                If DMS_Connector.Company.CompanySBO.Version > 900000 Then
                    If DMS_Connector.Configuracion.ParamGenAddon.U_UsaUbicD = "Y" Then
                        oConfiguracionSucursal.UsaUbicaciones = True
                    End If
                End If
                '*************************************************************
                'Valida si el usuario puede disminuir la cantidad de los items
                '*************************************************************

                If DMS_Connector.Configuracion.ParamGenAddon.U_ReduceCant.ToUpper.Equals("Y") Then
                    oConfiguracionSucursal.UsuarioDisminuye = ValidaUsuarioDisminuye()
                Else
                    oConfiguracionSucursal.UsuarioDisminuye = True
                End If

                p_oConfiguracionSucursalList.Add(oConfiguracionSucursal)
                '****************************
                'Carga Bodega Centro Costo
                '****************************
                Utilitarios.ObtenerAlmacenXCentroCosto(p_oCotizacionActual.IDSucursal, p_oBodegaCentroCostoList)
                Return True
            Next
            Return False
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function ValidaUsuarioDisminuye() As Boolean
        Try
            Return DMS_Connector.Helpers.PermisosMenu("SCGD_RED")
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

#End Region

End Module
