Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework

Public Class NotaCreditoProveedor
#Region "Definiciones"

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company
    'DocumentoProcesoCompra
    Private m_oDocumentoProcesoCompra As DocumentoProcesoCompra
    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"
    Public Shared EsNotaCredito As Boolean = False
#End Region

#Region "Constructor"
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        SBO_Application = SBOAplication
        SBO_Company = ocompany
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
    Public Property FormNotaCredito As Form
        Get
            Return _FormNotaCredito
        End Get
        Set(ByVal value As Form)
            _FormNotaCredito = value
        End Set
    End Property
    Private _FormNotaCredito As SAPbouiCOM.Form
#End Region
#Region "Nuevos metodos"
    Public Sub ManejaNotaCredito(ByRef DocEntry As String)
        Try
            If Not String.IsNullOrEmpty(DocEntry) Then
                CalculoCantidades.ExisteOrdenCompra = ExisteOrdenCompra(DocEntry)
                ProcesaNotaCredito(DocEntry)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ExisteOrdenCompra(ByVal DocEntry As Integer) As Boolean
        Dim Query As String = String.Empty
        Dim Cuenta As Integer = 0
        Try
            ExisteOrdenCompra = False
            Query = DMS_Connector.Queries.GetStrQueryFormat("ExisteOrdenCompraNC")
            Query = String.Format(Query, DocEntry)
            Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)
            If Cuenta > 0 Then
                ExisteOrdenCompra = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Sub ItemPress(ByRef pval As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Try
            If pval.BeforeAction Then
                If pval.ItemUID = "1" Then
                    EsNotaCredito = True
                    CalculoCantidades.AccionSeleccionada = False
                    CalculoCantidades.AbreDocumentos = False
                End If
            Else
                EsNotaCredito = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Public Sub ProcesaNotaCredito(ByVal p_strDocEntry As String)
        Try
            '**********DataContract****************
            Dim oLineaNotaCreditoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oTipoOTList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            '********Listas genericas*************
            Dim oSucursalList As List(Of String) = New Generic.List(Of String)
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim oCodigoMarcaList As List(Of String) = New Generic.List(Of String)
            Dim oBaseEntryList As List(Of Integer) = New Generic.List(Of Integer)
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List()
            '**********Declaración Variables*****************
            Dim blnProcesaNotaCredito As Boolean = False
            '*************Clases**************************
            Dim clsDocumentoProcesoCompra As DocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
            Dim CancelStatus As SAPbobsCOM.CancelStatusEnum
            '********Carga información lineas de entrada mercancia*************
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                blnProcesaNotaCredito = CargaNotaCredito(CInt(p_strDocEntry), oLineaNotaCreditoList, oSucursalList, oNoOrdenList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, oBaseEntryList, CancelStatus)
            End If
            If blnProcesaNotaCredito Then
                '**********************************************
                '*********** Actualiza Valores Cotizacion******
                '**********************************************
                ActualizaValoresCotizacion(oNoOrdenList, oLineaNotaCreditoList, CancelStatus)
                '**********************************************
                '*********** Genera Asiento Servicio Externo******
                '**********************************************
                'ManejarAsientoServicioExterno(oLineaFacturaProveedorList, oConfiguracionGeneralList, oSucursalList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, oBaseEntryList)
                '**********************************************
                '*********** Maneja Tracking******
                '**********************************************
                'If oConfiguracionGeneralList.Item(0).UsaOTInterna Then
                clsDocumentoProcesoCompra.ManejarTracking(oNoOrdenList, oLineaNotaCreditoList, oDatosGeneralesList, TipoDocumentoMarketing.NotaCredito)
                'End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        End Try
    End Sub


    Public Sub ManejarAsientoServicioExterno(ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                             ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List, _
                                             ByRef p_oSucursalList As List(Of String), _
                                             ByRef p_oCodigoMarcaList As List(Of String), _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                             ByRef p_oBaseEntryList As List(Of Integer))
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
                    ProcesaAsientoServicioExterno(oServicioExternoList, oAsientoServicioExternoList)
                    '************Verifica si genera asiento para servicio externo****************
                    If oAsientoServicioExternoList.Count > 0 Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoAsientoServExt, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                        '****************Maneja transacción**************
                        ResetTransaction()
                        StartTransaction()
                        If CrearAsiento(p_oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno) > 0 Then
                            '*****************Realiza commit ala transaccion**************
                            CommitTransaction()
                            '*****************Mensaje asiento generado correctamente*****************
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        End Try
    End Sub

    Public Sub ObtieneCostoEntradaMercancia(ByRef p_oLineasDocumentoMarketingList As DocumentoMarketing_List, _
                                            ByRef p_oBaseEntryList As List(Of Integer))
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
                oDocumentoMarketingBase = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                                                    SAPbobsCOM.Documents)
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

    Public Sub ManejaCantidadesyCosto(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_rowNotaCredito As DocumentoMarketing, ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum)
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
            If CancelStatus = CancelStatusEnum.csCancellation Then
                TipoMovimiento = CalculoCantidades.TipoMovimiento.Cancelacion
            Else
                TipoMovimiento = CalculoCantidades.TipoMovimiento.Creacion
            End If

            If p_rowNotaCredito.SinMovimientoInventario = BoYesNoEnum.tYES Then
                GeneraMovimientoInventario = False
            Else
                GeneraMovimientoInventario = True
            End If

            CantidadOfertaVentas = p_oCotizacion.Lines.Quantity
            CantidadAbiertaDocumentoCompra = p_rowNotaCredito.Cantidad

            CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
            CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
            CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value

            CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoAPARDocumentTypes.bodt_PurchaseCreditNote, TipoMovimiento, GeneraMovimientoInventario, CantidadOfertaVentas, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)

            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente
            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida

            CostoOfertaVentas = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
            CostoDocumentoCompra = p_rowNotaCredito.Costo

            CalculoCantidades.RecalcularCostos(BoObjectTypes.oPurchaseCreditNotes, TipoMovimiento, GeneraMovimientoInventario, CantidadOfertaVentas, CostoOfertaVentas, CantidadAbiertaDocumentoCompra, CostoDocumentoCompra)

            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = CostoOfertaVentas

            'Select Case p_rowNotaCredito.TipoArticulo
            '    Case TipoArticulo.ServicioExterno
            '        If DMS_Connector.Configuracion.ParamGenAddon.U_CostSExFP = "Y" Then
            '            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
            '        End If
            'End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ActualizaValoresCotizacion(ByRef p_oNoOrdenList As Generic.List(Of String), _
                                        ByRef p_oLineaNotaCreditoList As DocumentoMarketing_List, ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum)
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*************Objetos SAP *******************
            Dim oListaCotizacion As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '***********Listas Genericas **********
            Dim oDocEntryCotizacionList As List(Of String) = New List(Of String)
            Dim tempListNotaCreProveedorXOT As List(Of DocumentoMarketing) = New List(Of DocumentoMarketing)
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim strCampo As String = String.Empty
            Dim strNoOT As String = String.Empty
            Dim blnUsaIdRepXOrd As Boolean = False
            Dim blnProcesaLinea As Boolean = False
            Dim blnActualizaCotizacion As Boolean = False
            Dim intResultado As Integer = 1
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            CargarDocEntryCotizacion(p_oNoOrdenList, oDocEntryCotizacionList)
            For Each rowDocEntry As String In oDocEntryCotizacionList
                If Not String.IsNullOrEmpty(rowDocEntry) Then
                    oCotizacion = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                    intDocEntry = Convert.ToInt32(rowDocEntry)
                    If oCotizacion.GetByKey(intDocEntry) Then
                        strNoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                        tempListNotaCreProveedorXOT = p_oLineaNotaCreditoList.FindAll(Function(row) row.NoOrden.Trim = strNoOT)
                        For Each rowNotaCredito As DocumentoMarketing In tempListNotaCreProveedorXOT
                            blnActualizaCotizacion = False
                            If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                                If oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowNotaCredito.NoOrden Then
                                    strCampo = String.Empty
                                    blnUsaIdRepXOrd = False
                                    If Not String.IsNullOrEmpty(rowNotaCredito.ID) Then
                                        strCampo = "U_SCGD_ID"
                                        blnUsaIdRepXOrd = False
                                    ElseIf Not String.IsNullOrEmpty(rowNotaCredito.IdRepxOrd) Then
                                        strCampo = "U_SCGD_IdRepxOrd"
                                        blnUsaIdRepXOrd = True
                                    End If
                                    For contador As Integer = 0 To oCotizacion.Lines.Count - 1
                                        oCotizacion.Lines.SetCurrentLine(contador)
                                        blnProcesaLinea = False
                                        If blnUsaIdRepXOrd Then
                                            If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value = rowNotaCredito.IdRepxOrd Then
                                                blnProcesaLinea = True
                                            End If
                                        Else
                                            If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value.ToString.Trim() = rowNotaCredito.ID Then
                                                blnProcesaLinea = True
                                            End If
                                        End If
                                        If blnProcesaLinea Then
                                            ManejaCantidadesyCosto(oCotizacion, rowNotaCredito, CancelStatus)
                                            blnActualizaCotizacion = True
                                            p_oLineaNotaCreditoList.Remove(rowNotaCredito)
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
        Catch ex As Exception
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Sub

    Public Function CargaNotaCredito(ByVal p_intDocEntry As Integer, _
                                     ByRef p_oLineaNotaCreditoList As DocumentoMarketing_List, _
                                     ByRef p_oSucursalList As Generic.List(Of String), _
                                     ByRef p_oNoOrdenList As Generic.List(Of String), _
                                     ByRef p_oCodigoMarcaList As Generic.List(Of String), _
                                     ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                     ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                     ByRef p_oBaseEntryList As Generic.List(Of Integer), ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Dim oNotaCredito As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaNotaCredito As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strCentroCosto As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim strCodigoMarca As String = String.Empty
            Dim blnProcesaNotaCredito As Boolean = False
            Dim strMonedaLocal As String = String.Empty

            '****Consulta moneda local*********
            strMonedaLocal = ConsultaMonedaLocal()
            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oNotaCredito = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes),  _
                                                     SAPbobsCOM.Documents)
                '************Carga Objeto Entrada Mercancia********************************
                If oNotaCredito.GetByKey(p_intDocEntry) Then
                    CancelStatus = oNotaCredito.CancelStatus
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oNotaCredito.DocEntry
                        .DocNum = oNotaCredito.DocNum
                        .FechaContabilizacion = oNotaCredito.DocDate
                        .FechaCreacion = oNotaCredito.CreationDate
                        .CardCode = oNotaCredito.CardCode
                        .CardName = oNotaCredito.CardName
                        .MonedaLocal = strMonedaLocal
                        .Observaciones = oNotaCredito.Comments
                        If Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = oNotaCredito.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                        End If
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la Entrada Mercancia***********************
                    For rowEntrada As Integer = 0 To oNotaCredito.Lines.Count - 1
                        oNotaCredito.Lines.SetCurrentLine(rowEntrada)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strSucursal = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oNotaCredito.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            If intTipoArticulo = TipoArticulo.ServicioExterno Or intTipoArticulo = TipoArticulo.Repuesto Or intTipoArticulo = TipoArticulo.Suministro Then
                                oLineaNotaCredito = New DocumentoMarketing()
                                With oLineaNotaCredito
                                    .ItemCode = oNotaCredito.Lines.ItemCode
                                    .BodegaOrigen = oNotaCredito.Lines.WarehouseCode
                                    .TipoArticulo = intTipoArticulo
                                    .Cantidad = oNotaCredito.Lines.Quantity
                                    .BaseDocType = oNotaCredito.Lines.BaseType
                                    .BaseDocEntry = oNotaCredito.Lines.BaseEntry
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                        .NoOrden = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                        .TipoOT = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                    ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                        .TipoOT = oNotaCredito.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value) Then
                                        .CodigoProyecto = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value
                                    End If
                                    .Costo = oNotaCredito.Lines.LineTotal
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()) Then
                                        .Sucursal = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                    ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                        .Sucursal = oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value.ToString()) Then
                                        .CodigoMarca = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                    ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                        .CodigoMarca = oNotaCredito.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                                        .IdRepxOrd = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                        .ID = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                    End If
                                    If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString()) Then
                                        .CentroCosto = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                    Else
                                        .CentroCosto = DevuelveValorArticulo(oNotaCredito.Lines.ItemCode, "U_SCGD_CodCtroCosto")
                                    End If
                                    .SinMovimientoInventario = oNotaCredito.Lines.WithoutInventoryMovement
                                End With
                                p_oLineaNotaCreditoList.Add(oLineaNotaCredito)
                                '***************Agrega Sucursal al List*************************
                                If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                    strSucursal = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()
                                    If Not p_oSucursalList.Contains(strSucursal) Then
                                        p_oSucursalList.Add(strSucursal)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    strSucursal = oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
                                    If Not p_oSucursalList.Contains(strSucursal) Then
                                        p_oSucursalList.Add(strSucursal)
                                    End If
                                End If
                                '**************Agrega NoOrden al List******************
                                If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    strNoOrden = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                    If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                        p_oNoOrdenList.Add(strNoOrden)
                                    End If
                                End If
                                '**************Agrega Codigo Marca al List******************
                                If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value) Then
                                    strCodigoMarca = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                    If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                        p_oCodigoMarcaList.Add(strCodigoMarca)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                    strCodigoMarca = oNotaCredito.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                    If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                        p_oCodigoMarcaList.Add(strCodigoMarca)
                                    End If
                                End If
                                '**************Agrega TipoOT al List******************
                                If Not String.IsNullOrEmpty(oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    oTipoOT = New ConfiguracionOrdenTrabajo
                                    With oTipoOT
                                        .TipoOT = oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                    End With
                                    If Not p_oTipoOTList.Contains(oTipoOT) Then
                                        p_oTipoOTList.Add(oTipoOT)
                                    End If
                                ElseIf Not String.IsNullOrEmpty(oNotaCredito.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                    oTipoOT = New ConfiguracionOrdenTrabajo
                                    With oTipoOT
                                        .TipoOT = oNotaCredito.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                    End With
                                    If Not p_oTipoOTList.Contains(oTipoOT) Then
                                        p_oTipoOTList.Add(oTipoOT)
                                    End If
                                End If
                                '**************Agrega Base Entry al List******************
                                If Not p_oBaseEntryList.Contains(oNotaCredito.Lines.BaseEntry) Then
                                    p_oBaseEntryList.Add(oNotaCredito.Lines.BaseEntry)
                                End If
                                blnProcesaNotaCredito = True
                            End If

                        End If
                    Next
                End If
            End If
            Return blnProcesaNotaCredito
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oNotaCredito)
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
            SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
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
                                ByVal p_intTipoArticulo As Integer) As Integer
        Try
            '************Objetos*********************
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
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
                oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not dateFechaContabilizacion = Nothing Then
                    oJournalEntry.ReferenceDate = dateFechaContabilizacion
                End If
                If Not String.IsNullOrEmpty(strNoOrden) Then
                    oJournalEntry.Reference = strNoOrden
                End If

                Select Case p_intTipoArticulo
                    Case TipoArticulo.Servicio
                        oJournalEntry.Memo = String.Empty
                    Case TipoArticulo.ServicioExterno
                        oJournalEntry.Memo = My.Resources.Resource.AsientoFacturaProveedores + intDocNum.ToString()
                    Case TipoArticulo.OtrosCostosGastos
                        oJournalEntry.Memo = String.Empty
                End Select


                For Each rowAsiento As Asiento In p_oAsientoList
                    '*********************
                    'Cuenta Credito
                    '*********************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaCredito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Credit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCCredit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden

                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    oJournalEntry.Lines.Add()

                    '*****************
                    'Cuenta Debito
                    '*****************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaDebito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Debit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCDebit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden

                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    oJournalEntry.Lines.Add()
                Next

                If oJournalEntry.Add <> 0 Then
                    intAsientoGenerado = 0
                    SBO_Company.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intError, strMensajeError)
                Else
                    SBO_Company.GetNewObjectCode(strAsientoGenerado)
                    If Not String.IsNullOrEmpty(strAsientoGenerado) Then
                        intAsientoGenerado = CInt(strAsientoGenerado)
                    Else
                        intAsientoGenerado = 0
                    End If
                End If
            End If
            Return intAsientoGenerado
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
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
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowServicioExterno As DocumentoMarketing In p_oServicioExternoList
                strCuentaDebito = String.Empty
                strCuentaCredito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicioExterno.NoOrden
                    .Costo = rowServicioExterno.Costo
                    .Moneda = Nothing
                    '******Cuenta debito y cuenta credito************
                    If Not String.IsNullOrEmpty(rowServicioExterno.Almacen) Then
                        strCuentaDebito = ObtenerCuentaAlmacen(rowServicioExterno.Almacen, Account.TransferAc)
                        strCuentaCredito = ObtenerCuentaAlmacen(rowServicioExterno.Almacen, Account.ExpensesAc)
                    End If
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
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.Aplicado = False Then
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
                                                           strIDSucursales),
                                                           SBO_Company.CompanyDB,
                                                           SBO_Company.Server)
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
End Class
