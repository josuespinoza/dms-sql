Option Explicit On

Imports System.Collections.Generic
Imports DMSOneFramework
Imports DMSOneFramework.SCGBL.Requisiciones
Imports DMSOneFramework.EstadoxRepuestosDatasetTableAdapters
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Requisiciones
Imports SCG.Requisiciones.UI
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SuministrosDatasetTableAdapters

Namespace Requisiciones
    Public Class ManejadorRequisicionesTraslados
        Private _company As SAPbobsCOM.Company
        Private _application As Application
        Private _cotizacion As CotizacionCLS
        Private Const mc_strNoOT As String = "U_SCGD_NoOrden"
        Private mc_strIdSucursal As String
        Private Const mc_strRequisicion As String = "@SCGD_REQUISICIONES"
        Private Const mc_strCRec As String = "U_SCGD_CRec"
        Private Const mc_strCPenBod As String = "U_SCGD_CPBo"
        Private m_blnConfOTSAP As Boolean = False

        Public ReadOnly Property Company() As SAPbobsCOM.Company
            Get
                Return _company
            End Get
        End Property

        Public Property Cotizacion() As CotizacionCLS
            Get
                Return _cotizacion
            End Get
            Set(ByVal value As CotizacionCLS)
                _cotizacion = value
            End Set
        End Property

        Public ReadOnly Property Application() As Application
            Get
                Return _application
            End Get
        End Property

        Public Sub New(ByVal company As SAPbobsCOM.Company, ByVal application As Application, ByVal p_blnConfInterna As Boolean)
            _company = company
            _application = application
            m_blnConfOTSAP = p_blnConfInterna
        End Sub

        Public Enum EstadosSBO
            NoProcesado = 0
            No = 1
            Si = 2
            PendienteTraslado = 3
            PendienteBodega = 4
            SolicitudTrasladoRequisicion = 5
            SolicitudDevolucionRequisicion = 6
        End Enum

        Public Enum EstadosRepuestosDMS
            Pendiente = 1
            Solicitado = 2
            Recibido = 3
            PendientePorDev = 4
            PendienteTraslado = 5
            PendienteBodegaDraft = 6
            PendienteTrasladoRequisicion = 7
            PendienteDevolucionRequisicion = 8
        End Enum


        Public Sub TrasladoRealizado(ByVal lineastransferidas As List(Of TransferenciaLineasBase), ByVal TipoReq As String, ByRef codigoError As Integer, ByRef mensajeError As String)

            Try
                Dim oForm As SAPbouiCOM.Form
                Dim mensajeria As MensajeriaCls
                mensajeria = New MensajeriaCls(Application, Company)
                oForm = Application.Forms.Item("SCGD_FormRequisicion")
                codigoError = 0
                mensajeError = String.Empty

                For Each req As StockTransferTransferenciaLineas In lineastransferidas
                    If Not req.HuboError Then
                        If codigoError = 0 Then
                            ActualizarLineasDeLaCotizacion(req, TipoReq, codigoError, mensajeError)
                            Dim strTipoArticulo As String = req.InformacionLineasRequisicion.Item(0).DescripcionTipoArticulo
                            If Not m_blnConfOTSAP Then
                                ActualizarLineasOrdenTrabajo(req)
                                Dim pIntTipoMensaje As Integer = req.InformacionLineasRequisicion.Item(0).CodigoTipoArticulo

                                Dim rm As MensajeriaSBOTallerDataAdapter.RecibeMensaje
                                EnviarMensajePorActualizacionQty(req.StockTransfer.DocEntry, req.EncabezadoRequisicion.NoOrden, pIntTipoMensaje)
                                If pIntTipoMensaje = 1 Then
                                    rm = MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos
                                Else
                                    If pIntTipoMensaje = 3 Then rm = MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoSuministros
                                End If
                                CreaMensajeSBO_DMS(My.Resources.Resource.MensajeTraslado + " " + strTipoArticulo, req.EncabezadoRequisicion.NoOrden, req.InformacionLineasRequisicion.Item(0).DocumentoOrigen, rm, -1, -1)
                            Else
                                Dim mensaje As String
                                mensaje = My.Resources.Resource.MensajeTraslado + " " + strTipoArticulo
                                mensajeria.CreaMensajeSBO(mensaje, req.StockTransfer.DocEntry, Company, req.EncabezadoRequisicion.NoOrden, False, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), oForm.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_IDSuc", 0).ToString(), False)
                                'mensajeria.CreaMensajeSBO(mensaje, req.StockTransfer.DocEntry, Company, req.EncabezadoRequisicion.NoOrden, False, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), oForm.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_IDSuc", 0).ToString(), oForm, "dtConsulta", False)
                                'CreaMensajeSBO(mensaje, Company, req.EncabezadoRequisicion.NoOrden, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoBodega), oForm)
                            End If
                        End If
                    End If
                Next

            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, _application)
            End Try

        End Sub

        Public Function ValidarDisponibilidad(ByVal strItemcode As String, ByVal intBodega As Integer) As Boolean


            Dim decDisponibilidadBodega As Decimal = Utilitarios.EjecutarConsulta(String.Format("SELECT WhsCode FROM OITW WHERE ItemCode = '{0}' AND WhsCode = '{1}'", strItemcode, intBodega), _company.CompanyDB, _company.Server)

        End Function

        Public Sub AjusteCantidadRealizado(ByVal lineastransferidas As List(Of TransferenciaLineasBase))
            For Each req As StockTransferTransferenciaLineas In lineastransferidas
                ActualizarCantidadDeLaCotizacion(req)
                If Not m_blnConfOTSAP Then
                    ActualizarLineasOrdenTrabajo(req, True)
                End If

            Next
        End Sub

        Public Sub AjusteCantidadRealizado(ByVal oChildrenLineasReq As SAPbobsCOM.GeneralDataCollection, ByRef codError As Integer, ByRef msjError As String)
            Try

                If oChildrenLineasReq.Count > 0 Then
                    For Each LineaRequisicion As SAPbobsCOM.GeneralData In oChildrenLineasReq

                    Next
                End If

                '//Revisa los estados de las líneas
                'If (oChildrenLineasReq.Count > 0) Then
                '{
                '    foreach (SAPbobsCOM.GeneralData LineaRequisicion in oChildrenLineasReq)
                '    {
                '        numLinea = Convert.ToInt32(LineaRequisicion.GetProperty("LineId"));
                '        codEstadoLinea = Convert.ToInt32(LineaRequisicion.GetProperty("U_SCGD_CodEst"));
                '        if (codEstadoLinea == (int)EstadosLineas.Pendiente)
                '        {
                '            PendientesTraslado = true;
                '            TodasCanceladas = false;
                '        }
                '        else if (codEstadoLinea == (int)EstadosLineas.Trasladado)
                '        {
                '            TodasCanceladas = false;
                '        }
                '    }


                '    //Determina el estado general de la requisición
                '        If (PendientesTraslado) Then
                '    {
                '        //Pendiente
                '        //Existe una o más líneas pendientes de traslado
                '        codEstadoRequisicion = (int)EstadosLineas.Pendiente;
                '    }
                '        ElseIf (TodasCanceladas) Then
                '    {
                '        //Cancelado
                '        //Todas las líneas están canceladas
                '        codEstadoRequisicion = (int)EstadosLineas.Cancelado;
                '    }
                '        Else
                '    {
                '        //Trasladado
                '        //Una o más líneas trasladadas sin pendientes
                '        codEstadoRequisicion = (int)EstadosLineas.Trasladado;
                '    }
                '}
            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, _application)
            End Try
        End Sub

        Private Sub EnviarMensajePorActualizacionQty(ByVal p_strDocEntryTrasf As String, ByVal m_strNoOrden As String, ByVal p_intTipoMensaje As Integer)

            Try
                Dim clsMensajeria As New MensajeriaCls(_application, _company)
                Dim oForm As SAPbouiCOM.Form

                oForm = _application.Forms.Item("SCGD_FormRequisicion")

                clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasf, m_strNoOrden, p_intTipoMensaje, False, oForm, "dtConsulta", mc_strIdSucursal)

            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, _application)
                Throw ex
            End Try

        End Sub

        Public Sub CreaMensajeSBO_DMS(ByVal p_strMensaje As String, ByVal p_strOT As String _
                            , ByVal p_intNoCotizacion As Integer, ByVal p_destinatario As MensajeriaCls.RecibeMensaje, ByVal p_CodEmpleado As Integer _
                            , ByVal p_strNoVisita As String)

            Try
                Dim strCadenaConexion As String = String.Empty
                Dim m_adpMensajeria As MensajeriaSBOTallerDataAdapter
                Utilitarios.DevuelveCadenaConexionBDTaller(_application, mc_strIdSucursal, strCadenaConexion)

                m_adpMensajeria = New MensajeriaSBOTallerDataAdapter(strCadenaConexion)
                m_adpMensajeria.InsertarMensajeSBO_DMS(p_strMensaje, p_strOT, p_intNoCotizacion, p_destinatario, p_CodEmpleado, p_strNoVisita)

            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, _application)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub ActualizarCantidadDeLaCotizacion(ByVal req As StockTransferTransferenciaLineas, Optional ByVal blnActualizaAprobado As Boolean = False)
            Try
                Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
                Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines

                m_oBuscarCotizacion = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If m_oBuscarCotizacion.GetByKey(req.InformacionLineasRequisicion.Item(0).DocumentoOrigen) Then

                    m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                    For Each linea As InformacionLineaRequisicion In req.InformacionLineasRequisicion
                        For i As Integer = 0 To m_oLineasCotizacion.Count - 1
                            m_oLineasCotizacion.SetCurrentLine(i)
                            If m_oLineasCotizacion.LineNum = linea.LineNumOrigen Then
                                If linea.CantidadRecibida = linea.CantidadSolicitada Then
                                    m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                End If
                                m_oLineasCotizacion.Quantity = linea.CantidadSolicitada
                                Exit For
                            End If
                        Next
                    Next
                    m_oBuscarCotizacion.Update()
                End If

            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, Application)
            End Try

        End Sub

        Public Sub ActualizarLineasDeLaCotizacion(ByVal req As StockTransferTransferenciaLineas, ByVal strTipoReq As String, ByRef codigoError As Integer, ByRef mensajeError As String) ', Optional ByVal blnActualizaCantidad As Boolean = False)

            Try
                Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
                Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines

                Dim decCantidadRequisicion As Decimal = 0
                Dim decCantidadPendienteBodega As Decimal = 0
                Dim decCantidadPendiente As Decimal = 0
                Dim strCantidadPendienteBodega As String
                Dim strCantidadPendienteDevolucion As String
                Dim decCantidadPendienteDevolucion As Decimal = 0
                Dim decCPenDev As Decimal = 0
                Dim cPenBod As Integer
                Dim cRec As Integer
                Dim cPenDev As Integer
                Dim CodigoTipoRequisicion As String = String.Empty

                m_oBuscarCotizacion = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                CodigoTipoRequisicion = req.EncabezadoRequisicion.CodigoTipoRequisicion

                If m_oBuscarCotizacion.GetByKey(req.InformacionLineasRequisicion.Item(0).DocumentoOrigen) Then

                    m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                    For Each linea As InformacionLineaRequisicion In req.InformacionLineasRequisicion
                        For i As Integer = 0 To m_oLineasCotizacion.Count - 1
                            m_oLineasCotizacion.SetCurrentLine(i)


                            If linea.LineNumOrigen = m_oLineasCotizacion.LineNum AndAlso linea.CantidadRecibida = linea.CantidadSolicitada Then 'Or linea.LineNumOrigen = m_oLineasCotizacion.LineNum AndAlso linea.Then Then
                                If Not (m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2) Then
                                    m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                End If
                            End If

                            If linea.LineNumOrigen = m_oLineasCotizacion.LineNum AndAlso (linea.CodigoArticulo = m_oLineasCotizacion.ItemCode) Then

                                Dim blnModificarCantidadRecibida As Boolean = False

                                If m_blnConfOTSAP Then

                                    decCantidadRequisicion = linea.CantidadATransferir
                                    strCantidadPendienteBodega = m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value
                                    strCantidadPendienteDevolucion = m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value

                                    If Not String.IsNullOrEmpty(strCantidadPendienteDevolucion) Then decCantidadPendienteDevolucion = Decimal.Parse(strCantidadPendienteDevolucion)

                                    decCPenDev = decCantidadPendienteDevolucion
                                    'If (strTipoReq.Contains("Trans") Or strTipoReq.Contains("Res")) Then
                                    If (CodigoTipoRequisicion = 1 Or CodigoTipoRequisicion = 3) Then
                                        If Not decCantidadPendienteDevolucion = 0 Then
                                            decCantidadPendienteDevolucion -= decCantidadRequisicion
                                            blnModificarCantidadRecibida = True
                                        End If
                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = Double.Parse(decCantidadPendienteDevolucion)


                                        If Not String.IsNullOrEmpty(strCantidadPendienteBodega) Then decCantidadPendienteBodega = Decimal.Parse(strCantidadPendienteBodega)

                                        decCantidadPendienteBodega -= decCantidadRequisicion

                                        If decCantidadPendienteBodega >= 0 Then
                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = Double.Parse(decCantidadPendienteBodega)
                                        Else
                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                        End If

                                        If blnModificarCantidadRecibida Then
                                            If decCPenDev > 0 Then
                                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadRequisicion
                                            Else
                                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value -= decCantidadRequisicion
                                            End If
                                        Else
                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadRequisicion
                                        End If
                                    Else
                                        'If Not decCantidadPendienteDevolucion = 0 Then
                                        '    decCantidadPendienteDevolucion -= decCantidadRequisicion
                                        '    blnModificarCantidadRecibida = True
                                        'End If
                                        'm_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = Double.Parse(decCantidadPendienteDevolucion)
                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = linea.CantidadPendiente
                                        If linea.CantidadPendiente = 0 Then
                                            If Not (m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value >= linea.CantidadRecibida) Then
                                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                            Else
                                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                            End If

                                        End If
                                        'If blnModificarCantidadRecibida Then
                                        '    If decCPenDev > 0 Then
                                        '        cPenBod = m_oLineasCotizacion.UserFields.Fields.Item(mc_strCPenBod).Value
                                        '        cRec = m_oLineasCotizacion.UserFields.Fields.Item(mc_strCRec).Value
                                        '        cPenDev = m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value

                                        '        If (cPenBod + cRec + cPenDev) > 0 Then
                                        '            'm_oLineasCotizacion.Quantity = cPenBod + cRec + cPenDev
                                        '        End If
                                        '    Else
                                        '        'm_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadRequisicion
                                        '    End If
                                        'Else
                                        '    'm_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadRequisicion
                                        'End If
                                    End If


                                End If
                                Exit For
                            End If
                        Next
                    Next

                    codigoError = m_oBuscarCotizacion.Update()

                    If codigoError <> 0 Then
                        mensajeError = _company.GetLastErrorDescription
                    End If

                    Utilitarios.DestruirObjeto(m_oBuscarCotizacion)
                End If

            Catch ex As Exception
                Call Utilitarios.ManejadorErrores(ex, Application)
            End Try
        End Sub

        Private Sub ActualizarLineasOrdenTrabajo(ByVal stockTransferTransferenciaLineas As StockTransferTransferenciaLineas, Optional ByVal blnAjusteCantidad As Boolean = False)

            Try
                Dim strCadenaConexionBDTaller As String = ""
                Dim intRowsCountRepuestosXOrden As Integer
                Dim contRepuestosXOrden As Integer

                Dim decCosto As Decimal = 0

                ' Private m_dstOrdenTrabajoAnterior As OrdenTrabajoDataset
                Dim m_adpOrdenTrabajo As New DMSOneFramework.SCGDataAccess.OrdenTrabajoDataAdapter
                Dim m_dstOrdenTrabajo As New DMSOneFramework.OrdenTrabajoDataset

                Dim m_dstEstadosRepuesto As EstadoxRepuestosDataset = New EstadoxRepuestosDataset()
                Dim m_adpEstadosRepuesto As RepuestosxEstadoDataAdapter
                Dim m_drwEstadoXRepuesto As DMSOneFramework.EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow

                Dim m_dstRepuestosxOrden As DMSOneFramework.RepuestosxOrdenDataset
                Dim m_adpRepuestosxOrden As DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter
                Dim m_drwRepuestosXOrden As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

                Dim m_dstSuministrosxOrden As DMSOneFramework.SuministrosDataset
                Dim m_adpSuministrosxOrden As DMSOneFramework.SCGDataAccess.SuministrosDataAdapter

                Dim m_dstActividadesxOrden As DMSOneFramework.ActividadesXFaseDataset
                Dim m_adpActividadesxOrden As ActividadesXFaseDataAdapter

                'Dim m_dstAsignacionesColaboradores As New DMSOneFramework.ColaboradorDataset

                Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strCadenaConexionBDTaller)

                'Actualización de la cotización

                m_dstActividadesxOrden = Nothing
                m_dstRepuestosxOrden = Nothing
                m_dstSuministrosxOrden = Nothing

                Dim oForm As SAPbouiCOM.Form = _application.Forms.GetForm("SCGD_FormRequisicion", 0)

                mc_strIdSucursal = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_SCGD_idSucursal FROM OQUT with (nolock) WHERE U_SCGD_Numero_OT ='{0}' ",
                                                                      oForm.DataSources.DBDataSources.Item(mc_strRequisicion).GetValue(mc_strNoOT, 0).TrimEnd()),
                                                                  _company.CompanyDB, _company.Server)
                Utilitarios.DevuelveCadenaConexionBDTaller(Application, mc_strIdSucursal, strCadenaConexionBDTaller)

                m_dstRepuestosxOrden = New DMSOneFramework.RepuestosxOrdenDataset
                m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)

                m_dstSuministrosxOrden = New DMSOneFramework.SuministrosDataset
                m_adpSuministrosxOrden = New SuministrosDataAdapter(strCadenaConexionBDTaller)

                m_dstActividadesxOrden = New DMSOneFramework.ActividadesXFaseDataset
                m_adpActividadesxOrden = New ActividadesXFaseDataAdapter(strCadenaConexionBDTaller)

                m_adpEstadosRepuesto = New RepuestosxEstadoDataAdapter(strCadenaConexionBDTaller)

                m_dstOrdenTrabajo.EnforceConstraints = False
                m_adpOrdenTrabajo.Fill_x_OrdenTrabajo(m_dstRepuestosxOrden, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden)

                m_adpEstadosRepuesto.Fill(m_dstEstadosRepuesto, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden)

                'Dim actualizaEstado As RepuestosXEstadoQueriesAdapter = New RepuestosXEstadoQueriesAdapter()
                Dim actualizaCantSum As CantidadSuministrosQueryAdapater = New CantidadSuministrosQueryAdapater()

                'actualizaEstado.Conexion = strCadenaConexionBDTaller
                actualizaCantSum.Conexion = strCadenaConexionBDTaller

                intRowsCountRepuestosXOrden = m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows.Count


                For Each linea As InformacionLineaRequisicion In stockTransferTransferenciaLineas.InformacionLineasRequisicion

                    decCosto = 0

                    If Not blnAjusteCantidad Then
                        For cont As Integer = 0 To stockTransferTransferenciaLineas.StockTransfer.Lines.Count() - 1
                            stockTransferTransferenciaLineas.StockTransfer.Lines.SetCurrentLine(cont)

                            If linea.CodigoArticulo = stockTransferTransferenciaLineas.StockTransfer.Lines.ItemCode Then

                                decCosto = CDec(Utilitarios.EjecutarConsulta("Select isnull(StockPrice,0) from [WTR1] with (nolock) where DocEntry = '" & stockTransferTransferenciaLineas.StockTransfer.DocEntry & "' and ItemCode= '" & stockTransferTransferenciaLineas.StockTransfer.Lines.ItemCode & "'", _company.CompanyDB, _company.Server))

                                'decCosto = CDec(stockTransferTransferenciaLineas.StockTransfer.Lines.Price)
                                Exit For
                            End If

                        Next
                    End If

                    If intRowsCountRepuestosXOrden > 0 Then
                        For contRepuestosXOrden = 0 To intRowsCountRepuestosXOrden - 1
                            m_drwRepuestosXOrden = m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows(contRepuestosXOrden)
                            If linea.CodigoArticulo = m_drwRepuestosXOrden.NoRepuesto And linea.LineNumOrigen = m_drwRepuestosXOrden.LineNumOriginal Then

                                If linea.CodigoTipoArticulo = 1 Then
                                    'm_adpEstadosRepuesto.UpdateRepuestosXEstadoReq(m_dstEstadosRepuesto, linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadPendiente, linea.CantidadRecibida)
                                    'Dim cantidadNueva As Decimal = linea.CantidadSolicitada - linea.
                                    m_adpEstadosRepuesto.UpdateRepuestosXEstadoRequisiciones(linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadPendiente, linea.CantidadRecibida)
                                    m_adpRepuestosxOrden.UpdateCantidadXAjuste(linea.CodigoArticulo, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CantidadSolicitada, linea.LineNumOrigen)
                                    'Utilitarios.EjecutarConsulta("UPDATE [SCGTA_TB_RepuestosxOrden] set [Cantidad] = " & linea.CantidadSolicitada & " WHERE [NoRepuesto] = '" & linea.CodigoArticulo & "' and [NoOrden] = '" & stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden & "' and [LineNumOriginal] = " & linea.LineNumOrigen, _company.CompanyDB, _company.Server)

                                    If Not blnAjusteCantidad Then
                                        m_adpRepuestosxOrden.UpdateCostoRepuesto(stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.LineNumOrigen, decCosto)
                                    End If

                                End If

                            End If
                        Next
                    End If

                    If linea.CodigoTipoArticulo = 3 Then
                        m_adpSuministrosxOrden.UpdateSuministrosXEstadoRequisiciones(linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadRecibida)
                        'actualizaCantSum.ActualizaCantidadSuministroRequisiciones(linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadRecibida)
                    End If
                Next

                'Se comenta la forma original
                'For Each linea As InformacionLineaRequisicion In stockTransferTransferenciaLineas.InformacionLineasRequisicion
                '    '                For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                '    '
                '    '                    drwRep.CantidadRecibida = linea.CantidadRecibida
                '    '                    If linea.LineNumOrigen = drwRep.LineNum AndAlso linea.CantidadRecibida = linea.CantidadSolicitada Then
                '    '
                '    '                        With drwRep
                '    '                            .CodEstadoRep = EstadosRepuestosDMS.Recibido
                '    '                        End With
                '    '
                '    '                        Exit For
                '    '                    End If
                '    '                Next
                '    If linea.CodigoTipoArticulo = 1 Then
                '        actualizaEstado.ActualizaEstadoRepuestosRequisiciones(linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadPendiente, linea.CantidadRecibida)
                '    ElseIf linea.CodigoTipoArticulo = 3 Then
                '        actualizaCantSum.ActualizaCantidadSuministroRequisiciones(linea.LineNumOrigen, stockTransferTransferenciaLineas.EncabezadoRequisicion.NoOrden, linea.CodigoArticulo, linea.CantidadRecibida)
                '    Else
                '        Throw New InvalidOperationException("El tipo del artículo es inválido")
                '    End If
                'Next
            Catch ex As Exception
                Utilitarios.ManejadorErrores(ex, Application)
            End Try
            ' m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosxOrden)

        End Sub

        Public Sub ActualizaTransferenciaStock(ByVal encabezadorequisicion As EncabezadoRequisicion, ByVal stocktransfer As StockTransfer)
            If stocktransfer IsNot Nothing AndAlso encabezadorequisicion IsNot Nothing AndAlso Not String.IsNullOrEmpty(encabezadorequisicion.Data) Then
                Dim encT As EncabezadoTrasladoDMSData = encabezadorequisicion.Data.Deserialize()
                stocktransfer.SetUdf(encT.Marca, TransferenciaItems.mc_strU_Marca)
                stocktransfer.SetUdf(encT.Estilo, TransferenciaItems.mc_strU_Estilo)
                stocktransfer.SetUdf(encT.Modelo, TransferenciaItems.mc_strU_Modelo)
                stocktransfer.SetUdf(encT.Vin, TransferenciaItems.mc_strU_VIN)
                stocktransfer.SetUdf(encT.NumCotizacionOrigen, TransferenciaItems.mc_strIntCodigoCotizacion)
                stocktransfer.SetUdf(encT.TipoTransferencia, TransferenciaItems.mc_strTipoTransferenciaUdf)
                If Not String.IsNullOrEmpty(encT.Serie) Then
                    stocktransfer.Series = encT.Serie
                End If
            End If
        End Sub

        Public Sub ActualizaLineaTransferenciaStock(ByVal linearequisicion As InformacionLineaRequisicion, ByVal stocktransferlines As StockTransfer_Lines)
        End Sub

        Public Sub LineasCanceladas(ByVal lineas As List(Of InformacionLineaRequisicion), ByVal encabezado As EncabezadoRequisicion, ByRef codigoError As Integer, ByRef mensajeError As String)

            'Dim encabezadoTrasladoDmsData As EncabezadoTrasladoDMSData = encabezado.Data.Deserialize()
            Dim cotizacion As Documents
            Dim m_strIdRepxOrden As String
            Dim m_intIdSucursal As String
            Dim m_strBaseDeDatos As String
            Dim m_strItemcode As String
            Dim m_strtipoArticulo As String
            Dim m_strNoOrden As String
            Dim m_strLineNum As String
            Dim m_intDocEntry As Integer
            Dim CodigoTipoRequisicion As Integer

            cotizacion = Company.GetBusinessObject(BoObjectTypes.oQuotations)

            cotizacion.GetByKey(lineas(0).DocumentoOrigen)
            m_intIdSucursal = cotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
            m_strNoOrden = cotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString()
            m_strBaseDeDatos = Utilitarios.EjecutarConsulta(String.Format("Select U_BDSucursal from [@SCGD_SUCURSALES] with (nolock) where Code = '{0}'", m_intIdSucursal), _company.CompanyDB, _company.Server)
            CodigoTipoRequisicion = encabezado.CodigoTipoRequisicion

            For Each lineaRequisicion As InformacionLineaRequisicion In lineas
                For i As Integer = 0 To cotizacion.Lines.Count - 1
                    cotizacion.Lines.SetCurrentLine(i)

                    Dim cPenBod As Integer = cotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value
                    Dim cRec As Double = cotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value
                    Dim cPenDev As Double = cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
                    Dim Traslad As Integer = cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                    If cotizacion.Lines.LineNum = lineaRequisicion.LineNumOrigen Then
                        'If encabezado.TipoRequisicion.Contains("Trans") Or encabezado.TipoRequisicion.Contains("Res") Then
                        If CodigoTipoRequisicion = 1 Or CodigoTipoRequisicion = 3 Then

                            If Not m_blnConfOTSAP Then

                                m_strIdRepxOrden = cotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                m_strItemcode = cotizacion.Lines.ItemCode
                                m_strtipoArticulo = Utilitarios.EjecutarConsulta(String.Format("Select U_SCGD_TipoArticulo from OITM with (nolock) where ItemCode = '{0}'", m_strItemcode), _company.CompanyDB, _company.Server)

                                Select Case m_strtipoArticulo
                                    Case 1
                                        Utilitarios.EjecutarConsulta(String.Format(" Delete from {2}.dbo.SCGTA_TB_RepuestosxOrden where  ID = '{0}' and NoOrden = '{1}'", m_strIdRepxOrden, m_strNoOrden, m_strBaseDeDatos), _company.CompanyDB, _company.Server)
                                    Case 3
                                        Utilitarios.EjecutarConsulta(String.Format(" Delete from {2}.dbo.SCGTA_TB_SuministroxOrden where  ID = '{0}' and NoOrden = '{1}'", m_strIdRepxOrden, m_strNoOrden, m_strBaseDeDatos), _company.CompanyDB, _company.Server)
                                End Select

                            End If

                            If Traslad <> 2 Then
                                cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2
                                cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                cotizacion.Lines.UserFields.Fields.Item(mc_strCPenBod).Value = 0
                            End If
                            cotizacion.Lines.UserFields.Fields.Item("U_SCGD_ItemRecha").Value = "N"

                        Else
                            If m_blnConfOTSAP Then
                                cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1
                                Dim trs As String = cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString.Trim()

                                If cPenBod = 0 Then
                                    cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                ElseIf cPenBod > 0 Then
                                    cotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4
                                End If

                                cotizacion.Lines.UserFields.Fields.Item(mc_strCRec).Value = cRec + cPenDev
                                cotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                cotizacion.Lines.Quantity = cRec + cPenBod + cPenDev
                            End If
                        End If

                        Exit For
                    End If

                Next

            Next

            codigoError = cotizacion.Update()

            If codigoError <> 0 Then
                mensajeError = _company.GetLastErrorDescription
            End If

            Utilitarios.DestruirObjeto(cotizacion)

        End Sub

        Public Function LocalizationNeeded(ByVal informacionlinearequisicion As InformacionLineaRequisicion, ByVal tipomensaje As TipoMensaje) As String

            Select Case tipomensaje
                Case UI.TipoMensaje.EstadoLinea
                    Dim estado As EstadosLineas = informacionlinearequisicion.CodigoEstado
                    Select Case estado
                        Case EstadosLineas.Cancelado
                            Return My.Resources.Resource.Cancelado
                        Case EstadosLineas.Pendiente
                            Return My.Resources.Resource.Pendiente
                        Case EstadosLineas.Trasladado
                            Return My.Resources.Resource.Trasladado
                    End Select
                Case UI.TipoMensaje.EstadoFormulario
                    Dim estado As EstadosLineas = informacionlinearequisicion.CodigoEstado
                    Select Case estado
                        Case EstadosLineas.Cancelado
                            Return My.Resources.Resource.Cancelado
                        Case EstadosLineas.Pendiente
                            Return My.Resources.Resource.Pendiente
                        Case EstadosLineas.Trasladado
                            Return My.Resources.Resource.Trasladado
                    End Select
                Case UI.TipoMensaje.ErrorNoSePuedeTrasladar
                    Return String.Format(My.Resources.Resource.NoSePuedeTrasladar, informacionlinearequisicion.DataSourceOffset + 1)
                Case UI.TipoMensaje.MayorQueCantidadPendiente
                    Return String.Format(My.Resources.Resource.CantidadTransferirError, informacionlinearequisicion.DataSourceOffset + 1)
                Case UI.TipoMensaje.NoSePuedeCancelarLinea
                    Return String.Format(My.Resources.Resource.NoCancelarLinea, informacionlinearequisicion.DataSourceOffset + 1)
            End Select

            Return String.Empty

        End Function
    End Class
End Namespace
