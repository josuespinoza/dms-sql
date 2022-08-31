Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBL.Requisiciones
Imports SAPbouiCOM
Imports SCG.Requisiciones
Imports SCG.Requisiciones.UI
Imports DMSOneFramework.SCGCommon


Namespace SCGBusinessLogic

    Public Class TransferenciaItems

#Region "Declaraciones"
        'variable para cargar el DocEntry de la cotizacion
        Public intCodigoCotizacion As Integer
        Private intNoCotizacionByCancelacion As Integer

        Private Const mc_strUsaMensajeriaXCentroCosto As String = "UsaMensajeriaXCentroCosto"


#Region "Variables"

        'Dim m_objSBO_Application As SAPbouiCOM.Application
        Dim m_objCompany As SAPbobsCOM.Company
        Private m_strBodegaProcesoPorTipo As String

#End Region

#Region "Constantes"

#Region "Configuration Properties"

        Private Const mc_strBodegaRepuestos As String = "BodegaRepuestos"
        Private Const mc_strBodegaSuministros As String = "BodegaSuministros"
        Private Const mc_strBodegaServiciosExternos As String = "BodegaServiciosExternos"
        Private Const mc_strBodegaProceso As String = "BodegaProceso"
        Private Const mc_strIDSerieDocumentosTraslado As String = "IDSerieDocumentosTraslado"

#End Region

#Region "Fields"

        Private Const mc_strArroba As String = "@"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strTipoArticulo As String = "U_SCGD_TipoArticulo"
        Private Const mc_strIsGenerico As String = "U_SCGD_Generico"
        Private Const mc_strCodCentroCostoUDF As String = "U_SCGD_CodCtroCosto"
        Private Const mc_strCodCentroCostoDMS As String = "CodCentroCosto"
        Private Const mc_strAprobado As String = "U_SCGD_Aprobado"
        Private Const mc_strTraslad As String = "U_SCGD_Traslad"
        Private Const mc_strU_NoOrden As String = "U_SCGD_Numero_OT"
        Private Const mc_strParaTransfXCancelacion = "ParaTransfXCancelacion"
        Private Const mc_strItemCode As String = "ItemCode"
        Private Const mc_strTipoOT As String = "U_SCGD_Tipo_OT"
        Private Const mc_strIDSucur As String = "U_SCGD_idSucursal"

        Private Const mc_strU_Placa As String = "U_SCGD_Num_Placa"
        Private Const mc_strU_VIN As String = "U_SCGD_Num_VIN"
        Private Const mc_strU_Marca As String = "U_SCGD_Des_Marca"
        Private Const mc_strU_Estilo As String = "U_SCGD_Des_Estilo"
        Private Const mc_strU_Modelo As String = "U_SCGD_Des_Modelo"
        Private Const mc_strEmpRealiza As String = "U_SCGD_Emp_Realiza"
        Private Const mc_strNombEmpleado As String = "U_SCGD_NombEmpleado"
        Private Const mc_strIntCodigoCotizacion As String = "U_SCGD_CodCotizacion"

#End Region

#Region "Sps"

        Private Const mc_strSCGTA_SP_UPDOrden As String = "SCGTA_SP_UpdOrdenTrabajo"
        Private Const mc_strSCGTA_SP_SELItemRecibidos As String = "SCGTA_SP_SELItemRecibidos"
        Private Const mc_strSCGTA_SP_SelSuministrosXorden = "SCGTA_SP_SelSuministrosxOrden"
        Private Const mc_strSCGTA_SP_SelBodegaProcesoByCC As String = "SCGTA_SP_SelBodegaProcesoByCC"
        Private Const mc_strSCGTA_SP_SELBodegaProcesoXCCXItem As String = "SCGTA_SP_SELBodegaProcesoXCCXItem"
        Private Const mc_strSCGTA_SP_SELBodegaProcesoXCCXTipoOrden As String = "SCGTA_SP_SELBodegaProcesoXCCXTipo"
        Private Const mc_strSCGTA_SP_SELCentroBeneficioXItem As String = "SCGTA_SP_SELCentroBeneficioXItem"
        Private Const mc_strSCGTA_SP_SELCentroBeneficioXTipoOrden As String = "SCGTA_SP_SELCentroBeneficioXTipoOrden"

#End Region

#End Region

#Region "Estruturas"

        Public Structure LineasTransferenciaStock

            Dim strItemCode As String
            Dim strItemDescription As String
            Dim decCantidad As Decimal
            Dim strNoBodegaOrig As String
            Dim strNoBodegaDest As String
            Dim intTipoArticulo As Integer
            Dim intLineNum As Integer
            Dim tipo As String
            Public intCCosto As Integer
            Dim intReqOriPen As Integer
            Dim strIDlinea As String


        End Structure

#End Region

#Region "Enumeradores"

        Public Enum scgTiposMovimientoXBodega

            TransfRepuestos = 0
            TransfSuministros = 1
            TransfServiciosEx = 2
            TransfItemsEliminar = 3

        End Enum

#End Region

#Region "Objetos"

        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_cnnNewConexion As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

        Private m_adpTransItemsSBO As SqlClient.SqlDataAdapter

#End Region

#End Region

#Region "Constructor"

        Public Sub New(ByRef p_objCompany As SAPbobsCOM.Company, Optional ByVal p_blnTallerEnSAP As Boolean = False)

            m_objCompany = p_objCompany

            If Not p_blnTallerEnSAP Then
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
            End If

        End Sub

#End Region

#Region "Implementaciones SCG"

        Public Function CrearTrasladoAddOn(ByRef p_oDoc As SAPbobsCOM.Documents) As String

            Dim adpConf As ConfiguracionDataAdapter
            Dim dstConf As New ConfiguracionDataSet

            Dim strIDSerieDocTrasnf As String = ""
            Dim strNoOrden As String = ""
            Dim intTipoOrden As Integer
            Dim idSucu As String = String.Empty

            Dim strCollecDocEntrys As String = ""
            Dim strDocEntry As String = ""

            Dim lstItemsEliminar As New Generic.List(Of LineasTransferenciaStock)

            Dim strConexionDBSucursal As String = ""

            strConectionString = DMSOneFramework.SCGDataAccess.DAConexion.ConnectionString

            adpConf = New ConfiguracionDataAdapter(strConexionDBSucursal)

            adpConf.Fill(dstConf)

            strNoOrden = CStr(p_oDoc.UserFields.Fields.Item(mc_strU_NoOrden).Value)
            intTipoOrden = p_oDoc.UserFields.Fields.Item(mc_strTipoOT).Value
            idSucu = p_oDoc.UserFields.Fields.Item(mc_strIDSucur).Value
            m_strBodegaProcesoPorTipo = RetornaBodegaProcesoByTipoOrden(intTipoOrden)
            ''Genera la Lista de los Items que se van a trasladar de regreso a su bodega de origen
            GeneraLista(scgTiposMovimientoXBodega.TransfItemsEliminar, lstItemsEliminar, p_oDoc.Lines)

            strDocEntry = ""


            '--------agregado para transferencias de Stock a borrador
            Dim blnDraft As Boolean = False

            If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "CreaDraftTransferenciasStock", "") Then
                blnDraft = True
            Else
                blnDraft = False
            End If
            '---------------------------------------------------------

            Dim intAsesor As Integer = p_oDoc.SalesPersonCode

            If lstItemsEliminar.Count <> 0 Then
                strDocEntry = CrearSBOTransferenciaItems(lstItemsEliminar, strNoOrden, strIDSerieDocTrasnf, intAsesor, blnDraft)
            End If

            If strDocEntry <> "" Then
                strCollecDocEntrys &= ","
            End If

            strCollecDocEntrys &= strDocEntry

            Return strCollecDocEntrys

        End Function

        Public Sub CrearTrasladoByCancel(ByVal p_strNoOrden As String, ByVal p_strNoBodegaRepu As String, _
                                    ByVal p_strNoBodegaSumi As String, ByVal p_strNoBodegaSeEx As String, _
                                    ByVal p_strNoBodegaProceso As String, ByVal p_strIDSerieDocTrasnf As String, ByVal blnDraft As Boolean, ByVal p_NoCotizacion As Integer)


            Dim lstItems As New Generic.List(Of LineasTransferenciaStock)
            Dim drdRepuestos As SqlClient.SqlDataReader
            Dim drdSuministros As SqlClient.SqlDataReader

            Dim adpMensajeria As New SCGDataAccess.MensajeriaSBOTallerDataAdapter
            Dim intDoceEntry As Integer

            ' Se incluye para mensajeria por centro de costo
            Dim adpConf As ConfiguracionDataAdapter
            Dim dstConf As New ConfiguracionDataSet

            Dim strConexionDBSucursal As String = String.Empty
            Dim blnUsaMensajeriaXCentroCosto As Boolean = False
            Dim strDocEntry As String = String.Empty

            strConectionString = DMSOneFramework.SCGDataAccess.DAConexion.ConnectionString

            adpConf = New ConfiguracionDataAdapter(strConexionDBSucursal)

            adpConf.Fill(dstConf)


            If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionUsaCentroCosto(dstConf.SCGTA_TB_Configuracion, mc_strUsaMensajeriaXCentroCosto, "") Then
                blnUsaMensajeriaXCentroCosto = True
            Else
                blnUsaMensajeriaXCentroCosto = False
            End If

            'Generar Transferencias de Stock por Repuestos (Cancelacion)
            drdRepuestos = ObtenerRepuestosRecibidos(p_strNoOrden)
            GeneraListaByReader(lstItems, drdRepuestos)

            intNoCotizacionByCancelacion = p_NoCotizacion

            If lstItems.Count <> 0 Then
                strDocEntry = CrearSBOTransferenciaItems(lstItems, p_strNoOrden, p_strIDSerieDocTrasnf, blnDraft, True)
                intDoceEntry = Int(strDocEntry.Split(",")(0))
                If blnDraft Then
                    'If blnUsaMensajeriaXCentroCosto = True Then
                    '    adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacionXCentroCosto(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Bodeguero, p_strNoOrden, intDoceEntry)
                    'Else
                    '    adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Bodeguero, p_strNoOrden, intDoceEntry)
                    'End If
                    adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos, p_strNoOrden, intDoceEntry)
                Else
                    If blnUsaMensajeriaXCentroCosto = True Then
                        adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacionXCentroCosto(My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos, p_strNoOrden, strDocEntry, False)
                    Else
                        adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos, p_strNoOrden, intDoceEntry)
                    End If

                End If
            End If

            drdRepuestos.Close()
            drdRepuestos = Nothing

            MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

            'Generar Transferencias de Stock por Suministros (Cancelacion)
            drdSuministros = ObtenerSuministrosRecibidos(p_strNoOrden)
            GeneraListaByReader(lstItems, drdSuministros)

            If lstItems.Count <> 0 Then
                strDocEntry = CrearSBOTransferenciaItems(lstItems, p_strNoOrden, p_strIDSerieDocTrasnf, blnDraft, True)
                intDoceEntry = Int(strDocEntry.Split(",")(0))
                'adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos, p_strNoOrden, intDoceEntry)
                If blnDraft Then
                    adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoSuministros, p_strNoOrden, intDoceEntry)
                Else
                    If blnUsaMensajeriaXCentroCosto = True Then
                        adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacionXCentroCosto(My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoSuministros, p_strNoOrden, strDocEntry, False)
                    Else
                        adpMensajeria.CreaMensajeDMS_SBO_TransferenciaXCancelacion(My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoSuministros, p_strNoOrden, intDoceEntry)
                    End If
                End If
            End If

            drdSuministros.Close()
            drdSuministros = Nothing

        End Sub

        Public Sub CrearTrasladosCancelacionOT(ByVal p_strNoOrden As String, ByVal p_strIDSerieDocTrasnf As String,
                                               ByVal blnDraft As Boolean, ByVal p_NoCotizacion As Integer, ByVal p_strSucursal As String,
                                               ByRef p_dtRepuestos As SAPbouiCOM.DataTable, ByRef p_dtSuministros As SAPbouiCOM.DataTable,
                                               ByRef p_dtBodegasXCtrCosto As SAPbouiCOM.DataTable, ByRef oForm As SAPbouiCOM.Form, ByRef SBOApplication As SAPbouiCOM.Application,
                                               ByVal p_strCodCli As String, ByVal p_strNomCli As String, ByRef DocEntryRequisicionRepuestos As String, ByRef DocEntryRequisicionSuministros As String)


            Dim m_lstItems As New Generic.List(Of LineasTransferenciaStock)

            Dim intDoceEntry As Integer
            Dim blnUsaMensajeriaXCentroCosto As Boolean = False
            Dim strDocEntry As String = String.Empty
            Dim ErrorCode As Integer = 0
            Dim ErrorMessage As String = String.Empty

            ObtenerItemsRecibidosDataTable(p_NoCotizacion, "1", p_dtRepuestos, blnDraft)
            GeneraListaDeDataTable(m_lstItems, p_dtRepuestos, p_dtBodegasXCtrCosto, p_strSucursal)
            intNoCotizacionByCancelacion = p_NoCotizacion
            Dim msj As String

            If m_lstItems.Count <> 0 Then
                strDocEntry = CrearSBOTransferenciaItems(m_lstItems, p_strNoOrden, p_strIDSerieDocTrasnf, blnDraft, True, p_strSucursal, p_strCodCli, p_strNomCli, p_NoCotizacion)
                intDoceEntry = Int(strDocEntry.Split(",")(0))
                DocEntryRequisicionRepuestos = intDoceEntry
                If Not SCGDataAccess.Utilitarios.ValidaExisteDataTable(oForm, "dtConsulta") Then
                    oForm.DataSources.DataTables.Add("dtConsulta")
                End If
                If blnDraft Then
                    msj = My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden
                    SCGDataAccess.Utilitarios.CreaMensajeSBO(msj, intDoceEntry, m_objCompany, p_strNoOrden, blnDraft, Convert.ToInt32(SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos).ToString(), p_strSucursal, oForm, "dtConsulta", False, SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos, False)
                Else
                    If blnUsaMensajeriaXCentroCosto = True Then

                    Else
                        msj = My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden
                        SCGDataAccess.Utilitarios.CreaMensajeSBO(msj, intDoceEntry, m_objCompany, p_strNoOrden, blnDraft, Convert.ToInt32(SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos).ToString(), p_strSucursal, oForm, "dtConsulta", False, SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos, False)
                    End If
                End If
            End If

            ObtenerItemsRecibidosDataTable(p_NoCotizacion, "3", p_dtSuministros, blnDraft)
            GeneraListaDeDataTable(m_lstItems, p_dtSuministros, p_dtBodegasXCtrCosto, p_strSucursal)

            If m_lstItems.Count <> 0 Then
                strDocEntry = CrearSBOTransferenciaItems(m_lstItems, p_strNoOrden, p_strIDSerieDocTrasnf, blnDraft, True, p_strSucursal, p_strCodCli, p_strNomCli, p_NoCotizacion)
                intDoceEntry = Int(strDocEntry.Split(",")(0))
                DocEntryRequisicionSuministros = intDoceEntry
                If blnDraft Then
                    msj = My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT & ": " & p_strNoOrden
                    SCGDataAccess.Utilitarios.CreaMensajeSBO(msj, intDoceEntry, m_objCompany, p_strNoOrden, blnDraft, Convert.ToInt32(SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoSuministros).ToString(), p_strSucursal, oForm, "dtConsulta", False, SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos, False)
                Else
                    If blnUsaMensajeriaXCentroCosto = True Then

                    Else
                        msj = My.Resources.ResourceFrameWork.MensajeTransferenciaStockOT & ": " & p_strNoOrden
                        SCGDataAccess.Utilitarios.CreaMensajeSBO(msj, intDoceEntry, m_objCompany, p_strNoOrden, blnDraft, Convert.ToInt32(SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoSuministros).ToString(), p_strSucursal, oForm, "dtConsulta", False, SCGDataAccess.Utilitarios.RolesMensajeria.EncargadoRepuestos, False)
                    End If

                End If
            End If

        End Sub

        ''' <summary>
        ''' Crea un documento Requisición
        ''' </summary>
        ''' <param name="m_intNumCotiz"></param>
        ''' <param name="p_lstLineasTransStock"></param>
        ''' <param name="p_strNoOrden"></param>
        ''' <param name="p_strIDBodegaOrig"></param>
        ''' <param name="p_strNoSerie"></param>
        ''' <param name="p_strMarca"></param>
        ''' <param name="p_strEstilo"></param>
        ''' <param name="p_strModelo"></param>
        ''' <param name="p_strPlaca"></param>
        ''' <param name="p_strVIN"></param>
        ''' <param name="p_strAsesor"></param>
        ''' <param name="p_blnEliminar"></param>
        ''' <param name="p_strCodCliente"></param>
        ''' <param name="p_strNombCliente"></param>
        ''' <param name="p_blnAjusteOTEspecial"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CrearDocumentoTransferenciaRequisicion(ByVal p_intNumCotiz As Integer, ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), ByVal p_strNoOrden As String, _
                                                          ByVal p_strIDBodegaOrig As String, ByVal p_strNoSerie As String, _
                       ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String, _
                       ByVal p_strPlaca As String, ByVal p_strVIN As String, ByVal p_strAsesor As String, ByVal p_blnEliminar As Boolean, _
                       ByVal p_strCodCliente As String, ByVal p_strNombCliente As String, ByVal p_blnAjusteOTEspecial As Boolean, ByVal tipo As String, ByVal idSucursal As String) As List(Of RequisicionTraslado)

            Dim listaPorBodegas As List(Of List(Of LineasTransferenciaStock))
            Dim grupoPorBodega As List(Of LineasTransferenciaStock)
            CrearDocumentoTransferenciaRequisicion = New List(Of RequisicionTraslado)(10)

            listaPorBodegas = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

            For Each grupoPorBodega In listaPorBodegas
                Dim encabezado As EncabezadoRequisicion = New EncabezadoRequisicion()
                Dim data As EncabezadoTrasladoDMSData = New EncabezadoTrasladoDMSData()
                Dim listaLineas As List(Of InformacionLineaRequisicion)
                Dim req As RequisicionTraslado = New RequisicionTraslado(m_objCompany)

                encabezado.CodigoCliente = p_strCodCliente
                encabezado.NoOrden = p_strNoOrden
                encabezado.NombreCliente = p_strNombCliente
                data.TipoTransferencia = 1
                data.NumCotizacionOrigen = p_intNumCotiz
                data.Serie = p_strNoSerie
                encabezado.Comentarios = My.Resources.ResourceFrameWork.OT_Referencia & p_strNoOrden & " - " & My.Resources.ResourceFrameWork.MensajeTrasferenciaDraftCancelacion
                encabezado.Usuario = m_objCompany.UserName
                encabezado.IDSucursal = idSucursal
                encabezado.TipoArticulo = grupoPorBodega.Item(0).intTipoArticulo

                If p_blnEliminar Then
                    encabezado.TipoRequisicion = My.Resources.ResourceFrameWork.Devolucion
                    data.TipoTransferencia = 2
                    encabezado.Comentarios &= " * * " & My.Resources.ResourceFrameWork.Devolucion & " * * "
                End If

                listaLineas = New List(Of InformacionLineaRequisicion)(grupoPorBodega.Count)
                For Each linea As LineasTransferenciaStock In grupoPorBodega
                    Dim informacionLineaRequisicion As InformacionLineaRequisicion = New InformacionLineaRequisicion()
                    informacionLineaRequisicion.CodigoArticulo = linea.strItemCode
                    informacionLineaRequisicion.DescripcionArticulo = linea.strItemDescription
                    informacionLineaRequisicion.CodigoBodegaOrigen = linea.strNoBodegaOrig
                    informacionLineaRequisicion.CodigoBodegaDestino = linea.strNoBodegaDest
                    informacionLineaRequisicion.CantidadSolicitada = linea.decCantidad
                    informacionLineaRequisicion.LineNumOrigen = linea.intLineNum
                    informacionLineaRequisicion.DocumentoOrigen = p_intNumCotiz
                    informacionLineaRequisicion.DescripcionTipoArticulo = linea.tipo
                    informacionLineaRequisicion.CentroCosto = linea.intCCosto
                    informacionLineaRequisicion.LineaReqOrPen = linea.intReqOriPen
                    informacionLineaRequisicion.LineaIDSucursal = linea.strIDlinea
                    informacionLineaRequisicion.Estado = My.Resources.ResourceFrameWork.Pendiente
                    informacionLineaRequisicion.CodigoTipoArticulo = linea.intTipoArticulo

                    listaLineas.Add(informacionLineaRequisicion)

                Next

                encabezado.Data = data.Serialize()
                req.EncabezadoRequisicion = encabezado

                req.LineasRequisicion = listaLineas

                Dim crea As Integer = req.Crea()
                If crea <> 0 Then CrearDocumentoTransferenciaRequisicion.Add(req)

            Next

        End Function

        Private Function CrearSBOTransferenciaItems(ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), _
                        ByVal p_strNoOrden As String, ByVal p_strNoSerie As String, ByRef p_blnDraft As Boolean, Optional ByVal p_blnCancelacion As Boolean = False, Optional ByRef idSucursal As String = "", Optional ByVal p_strCodCli As String = "",
                        Optional p_strNombCli As String = "", Optional p_intDocEntry As Integer = -1) As String

            Dim oTransfStockDoc As SAPbobsCOM.StockTransfer

            Dim objBLSBO As New BLSBO.GlobalFunctionsSBO
            Dim intSBOResult As Integer
            Dim strErrMsg As String = ""
            Dim intNewDocEntry As Integer
            Dim strDocEntryResult As String = ""
            Dim glstItemsXBodegaOrigen As Generic.List(Of Generic.List(Of LineasTransferenciaStock))
            Dim lstActual As Generic.List(Of LineasTransferenciaStock)

            Try

                If p_blnDraft Then

                    Dim strDocEntryResultadoDraft As String = String.Empty
                    glstItemsXBodegaOrigen = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

                    For Each lstActual In glstItemsXBodegaOrigen

                        '                        strDocEntryResultadoDraft = CrearDocumentoDrafTransferencia(lstActual, p_strNoOrden, lstActual(0).strNoBodegaOrig, "", p_strNoSerie)

                        strDocEntryResultadoDraft = CrearDocumentoTransferenciaRequisicion(p_intDocEntry, lstActual, p_strNoOrden, lstActual(0).strNoBodegaOrig, p_strNoSerie, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, True, p_strCodCli, p_strNombCli, False, String.Empty, idSucursal).Item(0).EncabezadoRequisicion.DocEntry
                        Return strDocEntryResultadoDraft

                    Next
                    Exit Function

                End If
                'decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(Today, m_objCompany)

                'If decTipoCambio <> -1 Then

                glstItemsXBodegaOrigen = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

                For Each lstActual In glstItemsXBodegaOrigen

                    oTransfStockDoc = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

                    With oTransfStockDoc

                        .FromWarehouse = lstActual(0).strNoBodegaOrig
                        .Series = p_strNoSerie
                        .UserFields.Fields.Item(mc_strU_NoOrden).Value = p_strNoOrden
                        If p_blnCancelacion Then
                            .UserFields.Fields.Item("U_SCGD_TipoTransf").Value = 2
                            .Comments = My.Resources.ResourceFrameWork.MensajeOtReferencia & ": " & p_strNoOrden & " - " & My.Resources.ResourceFrameWork.MensajeTransferenciaCancelacion
                        Else
                            .Comments = My.Resources.ResourceFrameWork.MensajeOtReferencia & ": " & p_strNoOrden
                        End If

                    End With

                    CargarLineasTraslado(oTransfStockDoc, lstActual)

                    intSBOResult = oTransfStockDoc.Add()

                    If intSBOResult <> 0 Then

                        strErrMsg = m_objCompany.GetLastErrorDescription()

                        Throw New ExceptionsSBO(intSBOResult, strErrMsg)

                    Else

                        intNewDocEntry = m_objCompany.GetNewObjectKey

                    End If

                    If intNewDocEntry <> 0 Then
                        strDocEntryResult &= CStr(intNewDocEntry) & ","
                    End If

                Next

                'End If

                If strDocEntryResult <> "" Then
                    strDocEntryResult = strDocEntryResult.Substring(0, strDocEntryResult.Length - 1)
                End If

                '******************
                '*****************************
                If Not oTransfStockDoc Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTransfStockDoc)
                    oTransfStockDoc = Nothing
                End If
                Return strDocEntryResult

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Sub CargarLineasTraslado(ByRef p_oTrasfStockDoc As SAPbobsCOM.StockTransfer, _
                                        ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock))

            Dim udtLineasTSCurrent As LineasTransferenciaStock
            Dim intCont As Integer

            If p_lstLineasTransStock.Count <> 0 Then

                udtLineasTSCurrent = p_lstLineasTransStock(0)

                With p_oTrasfStockDoc

                    .Lines.ItemCode = udtLineasTSCurrent.strItemCode
                    .Lines.ItemDescription = udtLineasTSCurrent.strItemDescription
                    .Lines.Quantity = udtLineasTSCurrent.decCantidad
                    .Lines.WarehouseCode = udtLineasTSCurrent.strNoBodegaDest

                End With

                For intCont = 1 To p_lstLineasTransStock.Count - 1

                    udtLineasTSCurrent = p_lstLineasTransStock(intCont)

                    With p_oTrasfStockDoc

                        .Lines.Add()

                        .Lines.ItemCode = udtLineasTSCurrent.strItemCode
                        .Lines.ItemDescription = udtLineasTSCurrent.strItemDescription
                        .Lines.Quantity = udtLineasTSCurrent.decCantidad
                        .Lines.WarehouseCode = udtLineasTSCurrent.strNoBodegaDest

                    End With

                Next

            End If

        End Sub

        Private Sub GeneraLista(ByVal p_scgTiposMovimientosXBodega As scgTiposMovimientoXBodega, _
                    ByRef p_lstItems As Generic.List(Of LineasTransferenciaStock), _
                    ByRef p_oDocLines As SAPbobsCOM.Document_Lines)

            Dim intCont As Integer
            Dim udtLineaTransf As New LineasTransferenciaStock
            Dim strTipoItem As String
            Dim strIsGenerico As String
            Dim intCentroCosto As Integer
            Dim dblStockDisp As Double
            Dim strNoBodegaOrig As String = ""
            Dim strNoBodegaDest As String = ""

            For intCont = 0 To p_oDocLines.Count - 1

                With p_oDocLines

                    .SetCurrentLine(intCont)

                    If p_scgTiposMovimientosXBodega <> scgTiposMovimientoXBodega.TransfItemsEliminar Then

                        If .UserFields.Fields.Item(mc_strAprobado).Value = 1 And .UserFields.Fields.Item(mc_strTraslad).Value = 0 Then

                            strTipoItem = DevuelveValorItem(.ItemCode, mc_strTipoArticulo)

                            strIsGenerico = DevuelveValorItem(.ItemCode, mc_strIsGenerico)

                            intCentroCosto = DevuelveValorItem(.ItemCode, mc_strCodCentroCostoUDF)

                            If strIsGenerico = 1 Then


                                If String.IsNullOrEmpty(m_strBodegaProcesoPorTipo) Then
                                    strNoBodegaOrig = RetornaBodegaProceso(intCentroCosto)
                                Else
                                    strNoBodegaOrig = m_strBodegaProcesoPorTipo
                                End If
                                strNoBodegaDest = RetornaBodegaXTipo(intCentroCosto, strTipoItem)

                                dblStockDisp = DevuelveStockDisponibleItem(.ItemCode, strNoBodegaOrig)

                                If dblStockDisp > 0 Then

                                    udtLineaTransf.strItemCode = .ItemCode
                                    udtLineaTransf.strItemDescription = .ItemDescription
                                    udtLineaTransf.decCantidad = .Quantity
                                    udtLineaTransf.strNoBodegaOrig = strNoBodegaOrig
                                    udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                                    If mc_strTipoArticulo = "3" Or mc_strTipoArticulo = "1" Then
                                        udtLineaTransf.intTipoArticulo = mc_strTipoArticulo
                                    End If

                                    If (.UserFields.Fields.Item(mc_strTraslad).Value = 4) Then
                                        udtLineaTransf.intReqOriPen = 1
                                    Else
                                        udtLineaTransf.intReqOriPen = 2
                                    End If

                                    p_lstItems.Add(udtLineaTransf)

                                    .UserFields.Fields.Item(mc_strTraslad).Value = 1

                                End If

                            End If

                        End If

                    Else

                        If .UserFields.Fields.Item(mc_strAprobado).Value = 2 And .UserFields.Fields.Item(mc_strTraslad).Value = 1 Then

                            strTipoItem = DevuelveValorItem(.ItemCode, mc_strTipoArticulo)

                            intCentroCosto = DevuelveValorItem(.ItemCode, mc_strCodCentroCostoUDF)

                            If strTipoItem Like "[1,3]" Then

                                If String.IsNullOrEmpty(m_strBodegaProcesoPorTipo) Then
                                    strNoBodegaOrig = RetornaBodegaProceso(intCentroCosto)
                                Else
                                    strNoBodegaOrig = m_strBodegaProcesoPorTipo
                                End If
                                strNoBodegaDest = RetornaBodegaXTipo(intCentroCosto, strTipoItem)

                                udtLineaTransf.strItemCode = .ItemCode
                                udtLineaTransf.strItemDescription = .ItemDescription
                                udtLineaTransf.decCantidad = .Quantity

                                Select Case strTipoItem
                                    Case 1
                                        udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                                    Case 3
                                        udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                                End Select

                                udtLineaTransf.strNoBodegaOrig = strNoBodegaOrig

                                p_lstItems.Add(udtLineaTransf)

                                .UserFields.Fields.Item(mc_strTraslad).Value = 0

                            End If

                        End If

                    End If

                End With

            Next

        End Sub

        Private Sub GeneraListaByReader(ByRef p_lstItems As Generic.List(Of LineasTransferenciaStock), _
                    ByRef p_drdItems As SqlClient.SqlDataReader)

            Dim udtLineaTransf As New LineasTransferenciaStock
            Dim strTipoItem As String
            Dim intCentroCosto As Integer
            Dim strNoBodegaOrig As String = ""
            Dim strNoBodegaDest As String = ""

            p_lstItems.Clear()

            AbrirConexionNew()

            While p_drdItems.Read

                With p_drdItems

                    strTipoItem = DevuelveValorItem(.Item("ItemCode"), mc_strTipoArticulo)
                    intCentroCosto = DevuelveValorItem(.Item("ItemCode"), mc_strCodCentroCostoUDF)

                    If strTipoItem Like "[1,3]" Then

                        If String.IsNullOrEmpty(m_strBodegaProcesoPorTipo) Then
                            strNoBodegaOrig = RetornaBodegaProcesoToCancel(intCentroCosto)
                        Else
                            strNoBodegaOrig = m_strBodegaProcesoPorTipo
                        End If
                        strNoBodegaDest = RetornaBodegaXTipoToCancel(intCentroCosto, strTipoItem)


                        udtLineaTransf.strItemCode = .Item("ItemCode")
                        udtLineaTransf.strItemDescription = .Item("ItemName")
                        udtLineaTransf.decCantidad = .Item("Cantidad")
                        udtLineaTransf.tipo = strTipoItem
                        udtLineaTransf.intCCosto = intCentroCosto
                        udtLineaTransf.intTipoArticulo = strTipoItem
                        udtLineaTransf.intReqOriPen = 2
                        Select Case strTipoItem
                            Case 1
                                udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                            Case 3
                                udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                                '  Case 4
                                'udtLineaTransf.strNoBodegaDest = p_strNoBodegaSeEx
                        End Select

                        udtLineaTransf.strNoBodegaOrig = strNoBodegaOrig

                        p_lstItems.Add(udtLineaTransf)

                    End If

                End With

            End While

            CerrarConexionNew()

        End Sub

        Private Sub GeneraListaDeDataTable(ByRef p_lstItems As List(Of LineasTransferenciaStock), ByRef p_dtItems As DataTable, ByVal p_dtBodegasXCtrCosto As DataTable, ByVal p_strSucursal As String)

            Dim udtLineaTransf As New LineasTransferenciaStock
            Dim strTipoItem As String
            Dim strCentroCosto As String = String.Empty
            Dim strNoBodegaOrig As String = String.Empty
            Dim strNoBodegaDest As String = String.Empty

            Dim oItemArticulo As SAPbobsCOM.IItems

            Try
                oItemArticulo = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

                p_lstItems.Clear()

                For i As Integer = 0 To p_dtItems.Rows.Count - 1

                    oItemArticulo.GetByKey(p_dtItems.GetValue("ItemCode", i).ToString().Trim())

                    strTipoItem = oItemArticulo.UserFields.Fields.Item(mc_strTipoArticulo).Value
                    strCentroCosto = oItemArticulo.UserFields.Fields.Item(mc_strCodCentroCostoUDF).Value

                    If strTipoItem Like "[1,3]" AndAlso Not String.IsNullOrEmpty(strCentroCosto) Then

                        'Validacion para que no haga requisicion por develoucion si el articulo esta pendiente de Traslado
                        If Not (p_dtItems.GetValue("Trasladado", i).ToString.Trim() = 3) Then

                            If String.IsNullOrEmpty(m_strBodegaProcesoPorTipo) Then
                                For x As Integer = 0 To p_dtBodegasXCtrCosto.Rows.Count - 1
                                    If p_dtBodegasXCtrCosto.GetValue("Sucursal", x).ToString().Trim() = p_strSucursal And
                                        p_dtBodegasXCtrCosto.GetValue("CentroCosto", x).ToString().Trim() = strCentroCosto.ToString().Trim() Then
                                        strNoBodegaOrig = p_dtBodegasXCtrCosto.GetValue("Proceso", x).ToString().Trim()
                                        Exit For
                                    End If
                                Next
                            Else
                                strNoBodegaOrig = m_strBodegaProcesoPorTipo
                            End If
                            strNoBodegaDest = RetornaBodegaXTipoToCancelSAP(strCentroCosto, strTipoItem, p_strSucursal, p_dtBodegasXCtrCosto)

                            udtLineaTransf.strItemCode = p_dtItems.GetValue("ItemCode", i).ToString().Trim()
                            udtLineaTransf.strItemDescription = p_dtItems.GetValue("ItemName", i).ToString().Trim()
                            udtLineaTransf.decCantidad = p_dtItems.GetValue("Cantidad", i).ToString().Trim()
                            udtLineaTransf.intLineNum = p_dtItems.GetValue("LineNum", i).ToString().Trim()
                            udtLineaTransf.intTipoArticulo = p_dtItems.GetValue("TipArt", i).ToString.Trim

                            If strTipoItem = 1 Then
                                udtLineaTransf.tipo = My.Resources.ResourceFrameWork.Repuesto
                            Else
                                udtLineaTransf.tipo = My.Resources.ResourceFrameWork.Suministro
                            End If

                            udtLineaTransf.intCCosto = strCentroCosto
                            udtLineaTransf.intTipoArticulo = strTipoItem

                            If (p_dtItems.GetValue("Trasladado", i).ToString.Trim() = 4) Then
                                udtLineaTransf.intReqOriPen = 2
                            Else
                                udtLineaTransf.intReqOriPen = 1
                            End If


                            Select Case strTipoItem
                                Case 1
                                    udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                                Case 3
                                    udtLineaTransf.strNoBodegaDest = strNoBodegaDest
                            End Select

                            udtLineaTransf.strNoBodegaOrig = strNoBodegaOrig
                            udtLineaTransf.strIDlinea = p_dtItems.GetValue("IDrepXord", i).ToString().Trim()

                            p_lstItems.Add(udtLineaTransf)

                        End If

                    End If
                Next

                SCGDataAccess.Utilitarios.DestruirObjeto(oItemArticulo)

            Catch ex As Exception

            End Try

        End Sub

        Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                           ByVal strColName As String) As String

            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim valorCol As String

            oItemArticulo = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(strItemcode)
            valorCol = oItemArticulo.UserFields.Fields.Item(strColName).Value

            Return valorCol

        End Function

        Private Function DevuelveStockDisponibleItem(ByVal strItemcode As String, _
                                           ByVal strWhsCode As String) As Double

            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim oItemWhsInfo As SAPbobsCOM.IItemWarehouseInfo
            Dim intCount As Integer
            Dim dblStock As Double

            oItemArticulo = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(strItemcode)

            oItemWhsInfo = oItemArticulo.WhsInfo

            For intCount = 0 To oItemWhsInfo.Count - 1
                With oItemWhsInfo

                    .SetCurrentLine(intCount)

                    If .WarehouseCode = strWhsCode Then

                        dblStock = (.InStock) - .Committed

                        Exit For

                    End If

                End With
            Next

            Return dblStock

        End Function

        Private Function ObtenerRepuestosRecibidos(ByVal p_strNoOrden As String) As SqlClient.SqlDataReader
            Dim cmdItems As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELItemRecibidos, m_cnnSCGTaller)
            Dim drdItems As SqlClient.SqlDataReader

            With cmdItems
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                drdItems = .ExecuteReader(CommandBehavior.CloseConnection)
            End With

            Return drdItems

        End Function

        Private Sub ObtenerItemsRecibidosDataTable(ByVal p_intDocEntry As Integer, ByVal p_blnTipoArticulo As String, ByRef p_dtItems As SAPbouiCOM.DataTable, Optional ByVal p_blnDraft As Boolean = True)

            Dim m_strConsulta As String = String.Empty
            Dim m_strConsultaCompleta As String = String.Empty

            If p_blnTipoArticulo = "1" Then
                m_strConsulta =
                    "select ItemCode , Dscription as ItemName, U_SCGD_CRec as Cantidad, U_SCGD_Traslad as Trasladado, U_SCGD_ID as IDrepXord, LineNum, U_SCGD_TipArt As TipArt " +
                     " from QUT1 with(nolock) where DocEntry = {0}  and U_SCGD_TipArt = '1' and U_SCGD_Aprobado = 1 and U_SCGD_CRec > 0 "
            ElseIf p_blnTipoArticulo = "3" Then
                m_strConsulta =
                    "select ItemCode , Dscription as ItemName, U_SCGD_CRec as Cantidad, U_SCGD_Traslad as Trasladado, U_SCGD_ID as IDrepXord, LineNum, U_SCGD_TipArt As TipArt " +
                     " from QUT1 with(nolock) where DocEntry = {0}  and U_SCGD_TipArt = '3' and U_SCGD_Aprobado = 1 and U_SCGD_CRec > 0 "
            End If
            m_strConsultaCompleta = String.Format(m_strConsulta, p_intDocEntry)

            p_dtItems.ExecuteQuery(m_strConsultaCompleta)

        End Sub

        Private Function ObtenerSuministrosRecibidos(ByVal p_strNoOrden As String) As SqlClient.SqlDataReader

            Dim cmdItems As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelSuministrosXorden, m_cnnSCGTaller)
            Dim drdItems As SqlClient.SqlDataReader

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If


            With cmdItems
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                .Parameters.Add(mc_strArroba & mc_strParaTransfXCancelacion, SqlDbType.Int, 1).Value = 1
                drdItems = .ExecuteReader(CommandBehavior.CloseConnection)
            End With

            Return drdItems

        End Function

        Private Function RetornaBodegaProceso(ByVal p_intCentroCosto As Integer) As String
            Dim cmdBodega As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelBodegaProcesoByCC, m_cnnSCGTaller)
            Dim strBodegaProceso As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If

            With cmdBodega
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strCodCentroCostoDMS, SqlDbType.Int).Value = p_intCentroCosto
                strBodegaProceso = CType(.ExecuteScalar, String)
            End With

            Return strBodegaProceso

        End Function

        Private Function RetornaBodegaProcesoToCancel(ByVal p_intCentroCosto As Integer) As String
            Dim cmdBodega As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelBodegaProcesoByCC, m_cnnNewConexion)
            Dim strBodegaProceso As String

            With cmdBodega
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strCodCentroCostoDMS, SqlDbType.Int).Value = p_intCentroCosto
                strBodegaProceso = CType(.ExecuteScalar, String)
            End With

            Return strBodegaProceso

        End Function

        ''' <summary>
        ''' Retorna el Centro de Beneficio (Norma Reparto SBO) configurado
        ''' para un tipo de orden dado
        ''' </summary>
        ''' <param name="p_intCodTipoOrden">Código del tipo de orden</param>
        ''' <returns>Centro de beneficio asociado</returns>
        ''' <remarks>Si no tiene ningun centro de beneficio (norma de reparto) configurado devuelve
        ''' la cadena vacía</remarks>
        Public Function RetornaCentroBeneficioByTipoOrden(ByVal p_intCodTipoOrden As Integer) As String
            Dim cmdCB As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCentroBeneficioXTipoOrden, m_cnnSCGTaller)
            Dim cb As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If

            With cmdCB
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@codTipoOrden", SqlDbType.Int).Value = p_intCodTipoOrden
                cb = CType(.ExecuteScalar, String)
            End With

            Return cb

        End Function

        ''' <summary>
        ''' Retorna el Centro de Beneficio (Norma Reparto SBO) configurado
        ''' para un articulo. Se obtiene el Centro de Costo asociado al artículo
        ''' en SBO y si tiene asignado un Centro de Costo se busca el Centro de Beneficio
        ''' configurado para dicho Centro de Costo
        ''' </summary>
        ''' <param name="p_strItemCode">Código del artículo</param>
        ''' <returns>Centro de beneficio asociado</returns>
        ''' <remarks>Si no tiene ningun centro de beneficio (norma de reparto) configurado devuelve
        ''' la cadena vacía</remarks>
        Public Function RetornaCentroBeneficioByItem(ByVal p_strItemCode As String) As String
            Dim cmdCB As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCentroBeneficioXItem, m_cnnSCGTaller)
            Dim cb As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If

            With cmdCB
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@ItemCode", SqlDbType.NVarChar, 100).Value = p_strItemCode
                cb = CType(.ExecuteScalar, String)
            End With

            Return cb

        End Function


        Public Function RetornaBodegaProcesoByItem(ByVal p_strItemCode As String) As String
            Dim cmdBodega As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELBodegaProcesoXCCXItem, m_cnnSCGTaller)
            Dim strBodegaProceso As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If

            With cmdBodega
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strItemCode, SqlDbType.NVarChar, 20).Value = p_strItemCode
                strBodegaProceso = CType(.ExecuteScalar, String)
            End With

            Return strBodegaProceso

        End Function

        Public Function RetornaBodegaProcesoByTipoOrden(ByVal p_strTipoOrden As String) As String
            Dim cmdBodega As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELBodegaProcesoXCCXTipoOrden, m_cnnSCGTaller)
            Dim strBodegaProceso As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
            End If

            With cmdBodega
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strItemCode, SqlDbType.NVarChar, 20).Value = p_strTipoOrden
                strBodegaProceso = CType(.ExecuteScalar, String)
            End With

            Return strBodegaProceso

        End Function

        Public Sub CerrarConexion()

            If Not m_cnnSCGTaller Is Nothing AndAlso m_cnnSCGTaller.State = ConnectionState.Open Then
                m_cnnSCGTaller.Close()
            End If

        End Sub

        Public Sub AbrirConexionNew()

            m_cnnNewConexion = objDAConexion.ObtieneConexion

        End Sub

        Public Sub CerrarConexionNew()

            If Not m_cnnNewConexion Is Nothing AndAlso m_cnnNewConexion.State = ConnectionState.Open Then
                m_cnnNewConexion.Close()
            End If

        End Sub

        Private Function RetornaBodegaXTipo(ByVal p_intCentroCosto As Integer, ByVal p_strTipoItem As String) As String
            Dim cmdBodega As New SqlClient.SqlCommand
            Dim strBodega As String
            Dim strTipoBuscar As String

            If (m_cnnSCGTaller.State = ConnectionState.Closed) Then
                m_cnnSCGTaller.Open()
                m_cnnSCGTaller.Close()
            End If

            Select Case p_strTipoItem
                Case 1
                    strTipoBuscar = "Repuestos"
                Case 2
                    strTipoBuscar = "Servicios"
                Case 3
                    strTipoBuscar = "Suministros"
                Case Else
                    strTipoBuscar = "ServiciosEx"
            End Select


            With cmdBodega
                .Connection = m_cnnSCGTaller
                .CommandType = CommandType.Text

                .CommandText = "SELECT " & strTipoBuscar & " FROM SCGTA_TB_ConfBodegasXCentroCosto WHERE IDCentroCosto=@CodCentroCosto"

                .Parameters.Add(mc_strArroba & mc_strCodCentroCostoDMS, SqlDbType.Int).Value = p_intCentroCosto

                strBodega = CType(.ExecuteScalar, String)

            End With

            Return strBodega

        End Function

        Private Function RetornaBodegaXTipoToCancel(ByVal p_intCentroCosto As Integer, ByVal p_strTipoItem As String) As String
            Dim cmdBodega As New SqlClient.SqlCommand
            Dim strBodega As String
            Dim strTipoBuscar As String

            Select Case p_strTipoItem
                Case 1
                    strTipoBuscar = "Repuestos"
                Case 2
                    strTipoBuscar = "Servicios"
                Case 3
                    strTipoBuscar = "Suministros"
                Case Else
                    strTipoBuscar = "ServiciosEx"
            End Select


            With cmdBodega
                .Connection = m_cnnNewConexion
                .CommandType = CommandType.Text

                .CommandText = "SELECT " & strTipoBuscar & " FROM SCGTA_TB_ConfBodegasXCentroCosto WHERE IDCentroCosto=@CodCentroCosto"

                .Parameters.Add(mc_strArroba & mc_strCodCentroCostoDMS, SqlDbType.Int).Value = p_intCentroCosto

                strBodega = CType(.ExecuteScalar, String)

            End With

            Return strBodega

        End Function

        Private Function RetornaBodegaXTipoToCancelSAP(ByVal p_strCentroCosto As String, ByVal p_strTipoItem As String,
                                                       ByVal p_strSucursal As String, ByVal p_dtBodegasXCtrCosto As DataTable) As String
            Dim strBodega As String
            Dim strTipoBuscar As String

            Select Case p_strTipoItem
                Case 1
                    strTipoBuscar = "Repuestos"
                Case 2
                    strTipoBuscar = "Servicios"
                Case 3
                    strTipoBuscar = "Suministros"
                Case Else
                    strTipoBuscar = "ServExt"
            End Select

            For i As Integer = 0 To p_dtBodegasXCtrCosto.Rows.Count - 1
                If p_dtBodegasXCtrCosto.GetValue("Sucursal", i).ToString().Trim() = p_strSucursal And
                    p_dtBodegasXCtrCosto.GetValue("CentroCosto", i).ToString().Trim() = p_strCentroCosto Then
                    strBodega = p_dtBodegasXCtrCosto.GetValue(strTipoBuscar, i).ToString().Trim()
                End If
            Next

            Return strBodega

        End Function

        Private Function ClasificaListaXBodegaOrigen(ByRef p_lstLineasTranf As Generic.List(Of LineasTransferenciaStock)) As Generic.List(Of Generic.List(Of LineasTransferenciaStock))
            Dim objLineaParametro As LineasTransferenciaStock
            Dim objLineaAgregada As LineasTransferenciaStock

            Dim glstArrayReturn As New Generic.List(Of Generic.List(Of LineasTransferenciaStock))

            Dim glstListaClasif As Generic.List(Of LineasTransferenciaStock)
            Dim glstListaNueva As Generic.List(Of LineasTransferenciaStock)

            Dim blnExiste As Boolean = False

            For Each objLineaParametro In p_lstLineasTranf

                For Each glstListaClasif In glstArrayReturn

                    For Each objLineaAgregada In glstListaClasif

                        If objLineaParametro.strNoBodegaOrig = objLineaAgregada.strNoBodegaOrig Then
                            glstListaClasif.Add(objLineaParametro)
                            blnExiste = True
                            Exit For
                        End If

                    Next

                    If blnExiste Then
                        Exit For
                    End If

                Next

                If Not blnExiste Then

                    glstListaNueva = New Generic.List(Of LineasTransferenciaStock)
                    glstListaNueva.Add(objLineaParametro)

                    glstArrayReturn.Add(glstListaNueva)

                End If

            Next

            Return glstArrayReturn

        End Function

        ''' <summary>
        ''' Crea un documento Draft de tipo Transferencia de Stock dependiendo de las listas de 
        ''' repuestos y suministros
        ''' </summary>
        ''' <param name="p_lstLineasTransStock">Lista de repuestos o suministros</param>
        ''' <param name="p_strNoOrden"></param>
        ''' <param name="p_strIDBodegaOrig"></param>
        ''' <param name="p_CardCode"></param>
        ''' <param name="p_strNoSerie"></param>
        ''' <param name="p_strMarca"></param>
        ''' <param name="p_strEstilo"></param>
        ''' <param name="p_strModelo"></param>
        ''' <param name="p_strPlaca"></param>
        ''' <param name="p_strVIN"></param>
        ''' <param name="p_strCliente"></param>
        ''' <param name="p_strAsesor"></param>
        ''' <remarks></remarks>
        Public Function CrearDocumentoDrafTransferencia(ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), _
                        ByVal p_strNoOrden As String, ByVal p_strIDBodegaOrig As String, ByVal p_CardCode As String, ByVal p_strNoSerie As String) As String

            Try

                Dim objDocumentoDraftTranferencia As SAPbobsCOM.Documents
                Dim Verificar As Long
                '                Dim intSBOResult As Integer
                Dim strReturn As String = String.Empty
                Dim intNewDocEntryDraft As Integer
                Dim strErrMsg As String = ""
                Dim strDocEntryResult As String = ""

                objDocumentoDraftTranferencia = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)

                objDocumentoDraftTranferencia.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer

                With objDocumentoDraftTranferencia

                    .CardCode = p_CardCode
                    .DocDate = Date.Now
                    .TaxDate = Date.Now
                    .Series = p_strNoSerie
                    .UserFields.Fields.Item(mc_strU_NoOrden).Value = p_strNoOrden
                    .UserFields.Fields.Item(mc_strIntCodigoCotizacion).Value = intCodigoCotizacion

                    .Comments &= My.Resources.ResourceFrameWork.OT_Referencia & p_strNoOrden & " - " & My.Resources.ResourceFrameWork.MensajeTrasferenciaDraftCancelacion
                    .JournalMemo &= " - " & My.Resources.ResourceFrameWork.DocumentoBorrador
                    .SalesPersonCode = BuscarEncargadoCotizacion(intNoCotizacionByCancelacion)
                    ' .Comments &= " * * " & My.Resources.ResourceFrameWork.Devolucion & " * * "
                    .UserFields.Fields.Item("U_TipoTransferencia").Value = 2

                End With

                For i As Int16 = 0 To p_lstLineasTransStock.Count - 1

                    With objDocumentoDraftTranferencia.Lines

                        .ItemCode = p_lstLineasTransStock(i).strItemCode
                        .WarehouseCode = p_lstLineasTransStock(i).strNoBodegaDest
                        .ItemDescription = p_lstLineasTransStock(i).strItemDescription
                        .Quantity = p_lstLineasTransStock(i).decCantidad
                        .UserFields.Fields.Item("U_LinenumOrigen").Value = p_lstLineasTransStock(i).intLineNum

                        objDocumentoDraftTranferencia.Lines.Add()

                    End With

                Next i

                Verificar = objDocumentoDraftTranferencia.Add

                If Verificar <> 0 Then

                    strErrMsg = m_objCompany.GetLastErrorDescription()

                    Throw New ExceptionsSBO(Verificar, strErrMsg)

                Else

                    intNewDocEntryDraft = m_objCompany.GetNewObjectKey

                End If

                If intNewDocEntryDraft <> 0 Then
                    strDocEntryResult &= CStr(intNewDocEntryDraft) & ","
                End If

                Dim strDocEntry As String = intNewDocEntryDraft

                '                ActualizarBodegaEnDraft(p_lstLineasTransStock(0).strNoBodegaOrig, strDocEntry)

                If strDocEntryResult <> "" Then
                    strDocEntryResult = strDocEntryResult.Substring(0, strDocEntryResult.Length - 1)
                End If

                Return strDocEntryResult

            Catch ex As Exception

            End Try
            Return String.Empty
        End Function

        '        ''' <summary>
        '        ''' Actualiza el campo Filler o FromWarehouse en la tabla ODRF en SBO
        '        ''' </summary>
        '        ''' <param name="p_strIDBodegaOrig">Bodega origen</param>
        '        ''' <param name="p_DocEntryDraft">DocEntry del documento preliminar</param>
        '        ''' <remarks></remarks>
        '        Public Sub ActualizarBodegaEnDraft(ByVal p_strIDBodegaOrig As String, ByVal p_DocEntryDraft As String)
        '
        '            Dim oRecordset As SAPbobsCOM.Recordset
        '
        '            Dim strConsulta As String = "UPDATE  ODRF SET  Filler = '" & p_strIDBodegaOrig & "' WHERE DocEntry = '" & p_DocEntryDraft & "'"
        '----------------------
        '            oRecordset = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '            oRecordset.DoQuery(strConsulta)
        '
        '            oRecordset = Nothing
        '
        '        End Sub

        Public Function BuscarEncargadoCotizacion(ByVal p_DocEntryCotizacion As Integer) As Integer

            Try
                Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
                '                Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines

                m_oBuscarCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If m_oBuscarCotizacion.GetByKey(p_DocEntryCotizacion) Then
                    Dim codeSlPerson As Integer = m_oBuscarCotizacion.SalesPersonCode

                    Return codeSlPerson

                End If
            Catch ex As Exception

                Throw ex

            End Try
        End Function

#End Region

#Region "Creación de comandos"

        'Private Function CrearSelectCommand() As SqlClient.SqlCommand

        'Try

        '    Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrden)

        '    cmdSel.CommandType = CommandType.StoredProcedure

        '    Return cmdSel

        'Catch ex As Exception
        '    Throw ex
        'End Try


        'End Function

        'Private Function CreateInsertCommand() As SqlClient.SqlCommand

        'Try

        '    Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSOrden)

        '    cmdIns.CommandType = CommandType.StoredProcedure

        '    With cmdIns.Parameters

        '        .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)

        '        .Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

        '        .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

        '        .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita)

        '        .Add(mc_strArroba & mc_strClienteFacturar, SqlDbType.NVarChar, 15, mc_strClienteFacturar)

        '        .Add(mc_strArroba & mc_intAsesor, SqlDbType.Int, 4, mc_intAsesor)

        '        .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 1000, mc_strObservacion)

        '        .Add(mc_strArroba & mc_strMontoReparacion, SqlDbType.Int, 4, mc_strMontoReparacion)

        '        .Add(mc_strArroba & mc_intNoCotizacion, SqlDbType.Int, 4, mc_intNoCotizacion)

        '    End With

        '    Return cmdIns

        'Catch ex As Exception
        '    Throw ex
        'End Try

        'End Function

        'Private Function CrearUpdateCommand() As SqlClient.SqlCommand

        'Try

        '    Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDOrden)

        '    cmdUPD.CommandType = CommandType.StoredProcedure

        '    With cmdUPD.Parameters


        '        .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)
        '        .Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

        '        .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

        '        .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita)

        '        .Add(mc_strArroba & mc_DatFechaApertura, SqlDbType.DateTime, 8, mc_DatFechaApertura)

        '        .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 500, mc_strObservacion)


        '    End With

        '    Return cmdUPD

        'Catch ex As Exception
        '    Throw ex
        'End Try


        'End Function

#End Region

    End Class

End Namespace

