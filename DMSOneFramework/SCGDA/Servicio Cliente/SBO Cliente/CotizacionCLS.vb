Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports SCG.SBOFramework
Imports SCG.SBOFramework.DI
Imports System.Globalization

Namespace SCGDataAccess

    Public Class CotizacionCLS

#Region "Declaraciones"

#Region "Objetos"

        Private objCotizacion As SAPbobsCOM.Documents
        Private oCompany As SAPbobsCOM.Company

        Private objUtilitariosCls As New SCGDataAccess.Utilitarios(strConexionADO)

#End Region

#Region "Variables"

        Private m_strImpuestoRepuestos As String
        Private m_strImpuestoSuministros As String
        Private m_strImpuestoServicios As String
        Private m_strImpuestoServiciosExternos As String
        Private m_strIDSerieNumeracion As String
        Private _UsaListaPreciosCliente As Boolean

        Private udoSolicitudOTEspecial As SCG.DMSOne.Framework.UDOSolOTEsp

        Public dtPaquetes As New CotizacionPadrePaquetesDataSet

        Private drwPaquetes As CotizacionPadrePaquetesDataSet.CotizacionPadrePaquetesRow

        Private dstLineasSolOTEspecial As LineasSolOTEspecialDataSet
        Private adpLineasSolOTEspecial As LineasSolicitudOTEspecialDataAdapter
        Private drwLineasSolOTEspecial As LineasSolOTEspecialDataSet.LineasSolicitudOTEspecialRow

        Private strArticulo As String = String.Empty
        Private strDescripcion As String = String.Empty
        Private strSolicitud As String = String.Empty

        Public n As NumberFormatInfo

#End Region

#Region "Constantes"

        'Constantes aplicables a la cotización
        Private Const mc_strEmpleadoRecibe As String = "U_SCGD_Emp_Recibe"
        Private Const mc_strNumUnidad As String = "U_SCGD_Cod_Unidad"
        Private Const mc_strNumVehiculo As String = "U_SCGD_Num_Vehiculo"
        Private Const mc_strNoOrden As String = "U_SCGD_Numero_OT"
        Private Const mc_strGenerarOT As String = "U_SCGD_Genera_OT"
        Private Const mc_strTipoOT As String = "U_SCGD_Tipo_OT"
        Private Const mc_strNoVisita As String = "U_SCGD_No_Visita"
        Private Const mc_strEstadoCotizacion As String = "U_SCGD_Estado_Cot"
        Private Const mc_strEstadoCotizacionID As String = "U_SCGD_Estado_CotID"
        Private Const mc_strNoSerieCita As String = "U_SCGD_NoSerieCita"
        Private Const mc_strNoCita As String = "U_SCGD_NoCita"
        Private Const mc_strCardNameOrig As String = "U_SCGD_CardNameOrig"
        Private Const mc_strCardCodeOrig As String = "U_SCGD_CardCodeOrig"
        Private Const mc_strOTPadre As String = "U_SCGD_OT_Padre"
        Private Const mc_strAno_Vehi As String = "U_SCGD_Ano_Vehi"
        Private Const mc_strCod_Marca As String = "U_SCGD_Cod_Marca"
        Private Const mc_strCod_Modelo As String = "U_SCGD_Cod_Modelo"
        Private Const mc_strNum_VIN As String = "U_SCGD_Num_VIN"
        Private Const mc_strCosto As String = "U_SCGD_Costo"
        Private Const mc_strCPendiente As String = "U_SCGD_CPen"
        Private Const mc_strCSolicitada As String = "U_SCGD_CSol"
        Private Const mc_strCRecibida As String = "U_SCGD_CRec"
        Private Const mc_strCPendienteDevolucion As String = "U_SCGD_CPDe"
        Private Const mc_strCPendienteTraslado As String = "U_SCGD_CPTr"
        Private Const mc_strCPendienteBodega As String = "U_SCGD_CPBo"
        Private Const mc_strCompra As String = "U_SCGD_Compra"
        Private Const mc_strTipoArt As String = "U_SCGD_TipArt"
        Private Const mc_strNum_Placa As String = "U_SCGD_Num_Placa"
        Private Const mc_strCod_Estilo As String = "U_SCGD_Cod_Estilo"
        Private Const mc_strDes_Marc As String = "U_SCGD_Des_Marc"
        Private Const mc_strDes_Mode As String = "U_SCGD_Des_Mode"
        Private Const mc_strDes_Esti As String = "U_SCGD_Des_Esti"
        Private Const mc_strNoOtRef As String = "U_SCGD_NoOtRef"
        Private Const mc_strFechaRecepcion As String = "U_SCGD_Fech_Recep"
        Private Const mc_strHoraRecepcion As String = "U_SCGD_Hora_Recep"
        Private Const mc_strFechaCompromiso As String = "U_SCGD_Fech_Comp"
        Private Const mc_strHoraCompromiso As String = "U_SCGD_Hora_Comp"
        Private Const mc_strCCliOT As String = "U_SCGD_CCliOT"
        Private Const mc_strNCliOT As String = "U_SCGD_NCliOT"
        Private Const mc_stridSucursal As String = "U_SCGD_idSucursal"
        Private Const mc_strEntregado As String = "U_SCGD_Entregado"

        'Constantes aplicables a las líneas de la cotizacion
        Private Const mc_strItemAprobado As String = "U_SCGD_Aprobado"
        Private Const mc_strEmpRealiza As String = "U_SCGD_EmpAsig"
        Private Const mc_strGenerico As String = "U_SCGD_Generico"
        Private Const mc_strTrasladado As String = "U_SCGD_Traslad"
        Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"

        Private Const mc_strListaPrecios As String = "ListaPrecios"
        Private Const strUsaListaCliente As String = "UsaListaPreciosCliente"

#End Region

#Region "Enums"

        Private Enum TiposArticulos

            scgRepuesto = 1
            scgActividad = 2
            scgSuministro = 3
            scgServicioExt = 4
            scgPaquete = 5
            scgNinguno = 0

        End Enum

#End Region

#End Region

#Region "Construtores"

        Public Sub New(ByVal p_OCompany As SAPbobsCOM.Company)

            oCompany = p_OCompany
            objCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

        End Sub

        Public Sub New(ByVal p_OCompany As SAPbobsCOM.Company, ByVal p_intNoCotizacion As Integer)

            oCompany = p_OCompany
            objCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            If Not objCotizacion.GetByKey(p_intNoCotizacion) Then

                Throw New Exception("No se ha encontrado la cotización que se desea modificar")

            End If

        End Sub

#End Region

#Region "Propiedades"

        Public WriteOnly Property ImpuestoRepuestos() As String

            Set(ByVal value As String)

                m_strImpuestoRepuestos = value

            End Set

        End Property

        Public WriteOnly Property ImpuestoSuministros() As String

            Set(ByVal value As String)

                m_strImpuestoSuministros = value

            End Set

        End Property

        Public WriteOnly Property ImpuestoServicios() As String

            Set(ByVal value As String)

                m_strImpuestoServicios = value

            End Set

        End Property

        Public WriteOnly Property ImpuestoServiciosExternos() As String

            Set(ByVal value As String)

                m_strImpuestoServiciosExternos = value

            End Set

        End Property

        Public WriteOnly Property SerieDocumento() As String

            Set(ByVal value As String)

                m_strIDSerieNumeracion = value

            End Set

        End Property

        Public Property UsaListaPreciosCliente() As Boolean
            Get
                Return _UsaListaPreciosCliente
            End Get
            Set(ByVal value As Boolean)
                _UsaListaPreciosCliente = value
            End Set
        End Property

#End Region

#Region "Métodos"

        Public Function CancelarCotizacion(ByVal p_intDocEntry As Integer) As Boolean
            Dim intResultado As Integer
            Dim strMensajeError As String = ""
            If objCotizacion.GetByKey(p_intDocEntry) Then
                If String.IsNullOrEmpty(objCotizacion.UserFields.Fields.Item(mc_strNoOrden).Value) Then
                    If objCotizacion.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                        objCotizacion.UserFields.Fields.Item(mc_strNoCita).Value = ""
                        objCotizacion.UserFields.Fields.Item(mc_strNoSerieCita).Value = ""
                        objCotizacion.Comments = My.Resources.ResourceFrameWork.CitaEliminada
                        If objCotizacion.Update() = 0 Then
                            If objCotizacion.Cancel() <> 0 Then
                                RetrocederProceso()
                                oCompany.GetLastError(intResultado, strMensajeError)
                                Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
                            End If
                        Else
                            RetrocederProceso()
                            oCompany.GetLastError(intResultado, strMensajeError)
                            Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
                        End If
                    End If
                End If
            End If
        End Function

        Public Function ManejarCotizacion(ByVal p_drwCita As CitasDataset.SCGTA_TB_CitasRow,
                                          ByVal p_dstItems As QUT1Dataset,
                                          Optional ByVal p_strOTRereferncia As String = "")

            Dim intResultado As Integer
            Dim strMensajeError As String = ""
            Dim blnNueva As Boolean = True

            If p_drwCita.NoCotizacion = -1 Then

                objCotizacion.CardCode = p_drwCita.CardCode
                If m_strIDSerieNumeracion <> "" Then
                    objCotizacion.Series = m_strIDSerieNumeracion
                End If
                If Not (p_drwCita.Observaciones Is DBNull.Value) Then
                    If Not String.IsNullOrEmpty(p_drwCita.Observaciones) Then
                        objCotizacion.Comments = p_drwCita.Razon + " (" + p_drwCita.Observaciones + ")"
                    Else
                        objCotizacion.Comments = p_drwCita.Razon
                    End If
                End If

                objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = p_drwCita.NoVehiculo
                objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = p_drwCita.IDVehiculo
                If Not p_drwCita.IsAnoVehiculoNull Then
                    objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = p_drwCita.AnoVehiculo
                End If

                objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = p_drwCita.CodMarca.Trim(" ")
                objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = p_drwCita.CodModelo.Trim(" ")
                objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = p_drwCita.VIN
                objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = p_drwCita.Placa
                objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = p_drwCita.CodEstilo.Trim(" ")
                objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = p_drwCita.DescMarca
                objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = p_drwCita.DescModelo
                objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = p_drwCita.DescEstilo
                objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = p_strOTRereferncia

                objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = "No iniciada"
                objCotizacion.UserFields.Fields.Item(mc_strNoCita).Value = p_drwCita.NoConsecutivo
                objCotizacion.UserFields.Fields.Item(mc_strNoSerieCita).Value = p_drwCita.NoSerie
                objCotizacion.UserFields.Fields.Item(mc_stridSucursal).Value = Utilitarios.obtieneIDsucursal()


                If p_drwCita.empId <> -1 AndAlso Not p_drwCita.IsempIdNull Then
                    objCotizacion.DocumentsOwner = p_drwCita.empId
                End If

            Else
                blnNueva = False
                objCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                If Not objCotizacion.GetByKey(p_drwCita.NoCotizacion) Then

                    Throw New Exception("No se ha encontrado la cotización que se desea modificar")
                Else
                    If p_drwCita.empId <> -1 AndAlso Not p_drwCita.IsempIdNull Then
                        objCotizacion.DocumentsOwner = p_drwCita.empId
                    End If
                End If

            End If

            Call ActualizarLineasCotizacion(p_dstItems)

            If blnNueva Then
                intResultado = objCotizacion.Add()
            Else
                intResultado = objCotizacion.Update
            End If

            If intResultado <> 0 Then
                oCompany.GetLastError(intResultado, strMensajeError)
                Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
            Else
                If Not blnNueva Then
                    intResultado = objCotizacion.DocEntry
                Else
                    intResultado = CInt(oCompany.GetNewObjectKey())

                End If

            End If

            Return intResultado

        End Function

        Public Function ManejarCotizacion(ByVal p_intCodTipoOrden As Integer, _
                                          ByVal p_strCardCode As String, _
                                          ByVal p_intAsesor As Integer, _
                                          ByVal p_strCardCodeOriginal As String, _
                                          ByVal p_strCardNameOriginal As String, _
                                          ByVal p_drwOrdenNueva As OrdenTrabajoDataset.SCGTA_TB_OrdenRow, _
                                          ByVal p_dstItems As QUT1Dataset, _
                                          ByVal p_strSerieCotizaciones As String) As Integer

            Dim intResultado As Integer
            Dim strMensajeError As String = ""
            Dim blnNueva As Boolean = True
            Dim strComentarios As String
            Dim g_blnLineaAgregada As Boolean = False

            Dim strCardName As String

            objCotizacion.CardCode = p_strCardCode

            If p_strCardNameOriginal <> "" Then
                objCotizacion.CardName = p_strCardNameOriginal
            End If

            If Not String.IsNullOrEmpty(p_strCardCode) Then
                strCardName = Utilitarios.EjecutarConsulta(
                    String.Format("select CardName from OCRD where CardCode = '{0}'", p_strCardCode),
                    strConexionSBO)
            End If

            objCotizacion.UserFields.Fields.Item(mc_strCCliOT).Value = p_strCardCode
            objCotizacion.UserFields.Fields.Item(mc_strNCliOT).Value = strCardName.Trim()

            strComentarios = objCotizacion.Comments + My.Resources.ResourceFrameWork.Sederivadelaorden + p_drwOrdenNueva.NoOrden

            If strComentarios.Length <= 254 Then
                objCotizacion.Comments = strComentarios
            End If

            If Not String.IsNullOrEmpty(p_strSerieCotizaciones.Trim) Then
                objCotizacion.Series = p_strSerieCotizaciones
            End If


            If Not p_drwOrdenNueva.IsNoVehiculoNull Then
                objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = p_drwOrdenNueva.NoVehiculo
            End If
            objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = p_drwOrdenNueva.IDVehiculo

            If Not p_drwOrdenNueva.IsAnoVehiculoNull Then
                objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = p_drwOrdenNueva.AnoVehiculo

            End If
            objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = p_drwOrdenNueva.CodMarca.Trim


            If Not p_drwOrdenNueva.IsCodModeloNull Then
                objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = p_drwOrdenNueva.CodModelo.Trim
            End If
            If Not p_drwOrdenNueva.IsVINNull Then
                objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = p_drwOrdenNueva.VIN
            End If
            If Not p_drwOrdenNueva.IsPlacaNull Then
                objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = p_drwOrdenNueva.Placa

            End If
            If Not p_drwOrdenNueva.IsCodEstiloNull Then
                objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = p_drwOrdenNueva.CodEstilo.Trim

            End If
            If Not p_drwOrdenNueva.IsDescMarcaNull Then
                objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = p_drwOrdenNueva.DescMarca.Trim

            End If
            If Not p_drwOrdenNueva.IsDescModeloNull Then
                objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = p_drwOrdenNueva.DescModelo.Trim
            End If
            If Not p_drwOrdenNueva.IsDescEstiloNull Then
                objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = p_drwOrdenNueva.DescEstilo.Trim
            End If

            If Not p_drwOrdenNueva.IsFecha_aperturaNull Then
                objCotizacion.UserFields.Fields.Item(mc_strFechaRecepcion).Value = p_drwOrdenNueva.Fecha_apertura
            End If

            If Not p_drwOrdenNueva.IsFecha_CompNull Then
                objCotizacion.UserFields.Fields.Item(mc_strFechaCompromiso).Value = p_drwOrdenNueva.Fecha_Comp
            End If

            If Not p_drwOrdenNueva.IsKilometrajeNull Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = p_drwOrdenNueva.Kilometraje
            End If

            If Not p_drwOrdenNueva.IsHorasServicioNull Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_HoSr").Value = p_drwOrdenNueva.HorasServicio
            End If

            objCotizacion.UserFields.Fields.Item(mc_strCardCodeOrig).Value = p_strCardCodeOriginal
            objCotizacion.UserFields.Fields.Item(mc_strCardNameOrig).Value = p_strCardNameOriginal

            objCotizacion.UserFields.Fields.Item(mc_strNoVisita).Value = CStr(p_drwOrdenNueva.NoVisita)
            objCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value = p_intCodTipoOrden
            objCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value = p_drwOrdenNueva.NoOrden
            objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = p_drwOrdenNueva.NoOrden
            objCotizacion.UserFields.Fields.Item(mc_stridSucursal).Value = Utilitarios.EjecutarConsulta(String.Format(" select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}' ", p_drwOrdenNueva.NoOrden.Trim()), strConexionSBO)
            objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = My.Resources.ResourceFrameWork.EstadoOrdenNoIniciada
            objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "1"

            objCotizacion.DocumentsOwner = p_intAsesor

            Call ActualizarLineasCotizacion(p_dstItems)

            intResultado = objCotizacion.Add()

            If intResultado <> 0 Then
                oCompany.GetLastError(intResultado, strMensajeError)
                Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
            Else

                intResultado = CInt(oCompany.GetNewObjectKey())

            End If

            Return intResultado

        End Function

        Public Function ValidarLineasParaSolicitudOT_Especial(ByVal p_dstItems As QUT1Dataset, ByVal p_NoOrden As String) As Boolean

            dstLineasSolOTEspecial = New LineasSolOTEspecialDataSet
            adpLineasSolOTEspecial = New LineasSolicitudOTEspecialDataAdapter

            adpLineasSolOTEspecial.Fill(dstLineasSolOTEspecial, p_NoOrden)

            For Each linea As QUT1Dataset.QUT1Row In p_dstItems.QUT1

                For Each drwLineasSolOTEspecial In dstLineasSolOTEspecial.LineasSolicitudOTEspecial.Rows


                    If linea.ID = drwLineasSolOTEspecial.U_IdRxO And drwLineasSolOTEspecial.Status = "O" Then

                        strArticulo = drwLineasSolOTEspecial.U_ItemCode
                        strDescripcion = drwLineasSolOTEspecial.U_Descrip
                        strSolicitud = drwLineasSolOTEspecial.DocEntry

                        Return True

                    End If
                Next

            Next

        End Function

        Public Function ManejarSolicitudOTEspecial(ByVal p_intCodTipoOrden As Integer, _
                                         ByVal p_strCardCode As String, _
                                         ByVal p_intAsesor As Integer, _
                                         ByVal p_strCardCodeOriginal As String, _
                                         ByVal p_strCardNameOriginal As String, _
                                         ByVal p_drwOrdenNueva As OrdenTrabajoDataset.SCGTA_TB_OrdenRow, _
                                         ByVal p_dstItems As QUT1Dataset, _
                                         ByVal p_strSerieCotizaciones As String, _
                                         ByVal p_NombreAsesor As String, _
                                         ByVal p_NombreTipoOT As String) As Integer

            Dim intResultado As Integer
            Dim strMensajeError As String = ""
            Dim blnNueva As Boolean = True
            Dim strComentarios As String
            Dim g_blnLineaAgregada As Boolean = False
            Dim strConsultaEspecialesAprob As String = String.Empty

            '**********************************************

            If ValidarLineasParaSolicitudOT_Especial(p_dstItems, p_drwOrdenNueva.NoOrden) Then

                System.Windows.Forms.MessageBox.Show(String.Format(My.Resources.ResourceFrameWork.MensajeSoliticitudArticulosUsados, strArticulo, strDescripcion, strSolicitud))

                strArticulo = String.Empty
                strDescripcion = String.Empty
                strSolicitud = String.Empty
                Exit Function

            End If

            udoSolicitudOTEspecial = New SCG.DMSOne.Framework.UDOSolOTEsp(oCompany) ' New SCG.DMSOne.Framework.UDOSolicitudOrdenEspecial(oCompany)
            udoSolicitudOTEspecial.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOSolOTEsp()

            udoSolicitudOTEspecial.Encabezado.CodigoCliente = p_strCardCode

            Dim strQuery As String = String.Format("select CardName from dbo.[OCRD] where CardType = 'C' and cardcode ='{0}'", p_strCardCode).ToString().Trim()
            udoSolicitudOTEspecial.Encabezado.NombreCliente = Utilitarios.EjecutarConsulta(strQuery, strConexionSBO)

            If p_strCardNameOriginal <> String.Empty Then
                udoSolicitudOTEspecial.Encabezado.NombreCliente = p_strCardNameOriginal
            End If

            udoSolicitudOTEspecial.Encabezado.CodigoAsesor = p_intAsesor
            udoSolicitudOTEspecial.Encabezado.NumeroCotizacion = 0
            udoSolicitudOTEspecial.Encabezado.OTReferencia = p_drwOrdenNueva.NoOrden
            udoSolicitudOTEspecial.Encabezado.TipoOrden = p_intCodTipoOrden

            udoSolicitudOTEspecial.Encabezado.CotizacionReferencia = p_drwOrdenNueva.NoCotizacion

            strComentarios = My.Resources.ResourceFrameWork.Sederivadelaorden + p_drwOrdenNueva.NoOrden
            If strComentarios.Length <= 254 Then
                udoSolicitudOTEspecial.Encabezado.Comentarios = strComentarios
            End If

            udoSolicitudOTEspecial.Encabezado.Series = p_strSerieCotizaciones
            udoSolicitudOTEspecial.Encabezado.CodigoUnidad = p_drwOrdenNueva.NoVehiculo
            udoSolicitudOTEspecial.Encabezado.IdVehiculo = p_drwOrdenNueva.IDVehiculo
            udoSolicitudOTEspecial.Encabezado.NombreAsesor = p_NombreAsesor
            udoSolicitudOTEspecial.Encabezado.NombreTipoOT = p_NombreTipoOT

            If Not p_drwOrdenNueva.IsClienteFacturarNull Then
                udoSolicitudOTEspecial.Encabezado.CardCodeOrigen = p_drwOrdenNueva.ClienteFacturar
            End If

            If Not p_drwOrdenNueva.IsCardNameNull Then
                udoSolicitudOTEspecial.Encabezado.CardNameOrigen = p_drwOrdenNueva.CardName
            End If

            If Not p_drwOrdenNueva.IsAnoVehiculoNull Then
                udoSolicitudOTEspecial.Encabezado.Anno = p_drwOrdenNueva.AnoVehiculo
            End If
            If Not p_drwOrdenNueva.IsCodMarcaNull Then
                udoSolicitudOTEspecial.Encabezado.CodigoMarca = p_drwOrdenNueva.CodMarca.Trim
            End If
            If Not p_drwOrdenNueva.IsCodModeloNull Then
                udoSolicitudOTEspecial.Encabezado.CodigoModelo = p_drwOrdenNueva.CodModelo.Trim
            End If
            If Not p_drwOrdenNueva.IsVINNull Then
                udoSolicitudOTEspecial.Encabezado.VIN = p_drwOrdenNueva.VIN
            End If
            If Not p_drwOrdenNueva.IsPlacaNull Then
                udoSolicitudOTEspecial.Encabezado.Placa = p_drwOrdenNueva.Placa
            End If
            If Not p_drwOrdenNueva.IsCodEstiloNull Then
                udoSolicitudOTEspecial.Encabezado.CodigoEstilo = p_drwOrdenNueva.CodEstilo.Trim
            End If
            If Not p_drwOrdenNueva.IsDescMarcaNull Then
                udoSolicitudOTEspecial.Encabezado.DescripcionMarca = p_drwOrdenNueva.DescMarca.Trim
            End If

            If Not p_drwOrdenNueva.IsDescModeloNull Then
                udoSolicitudOTEspecial.Encabezado.DescripcionModelo = p_drwOrdenNueva.DescModelo.Trim
            End If
            If Not p_drwOrdenNueva.IsDescEstiloNull Then
                udoSolicitudOTEspecial.Encabezado.DescripcionEstilo = p_drwOrdenNueva.DescEstilo.Trim
            End If
            If Not p_drwOrdenNueva.IsFecha_aperturaNull Then
                udoSolicitudOTEspecial.Encabezado.FechaApertura = p_drwOrdenNueva.Fecha_apertura
            End If
            If Not p_drwOrdenNueva.IsFecha_compromisoNull Then
                udoSolicitudOTEspecial.Encabezado.FechaCompromiso = p_drwOrdenNueva.Fecha_compromiso
            End If
            If Not p_drwOrdenNueva.IsKilometrajeNull Then
                udoSolicitudOTEspecial.Encabezado.Kilometraje = p_drwOrdenNueva.Kilometraje
            End If
            If Not p_drwOrdenNueva.IsHorasServicioNull Then
                udoSolicitudOTEspecial.Encabezado.HorasServicio = p_drwOrdenNueva.HorasServicio
            End If

            'udoSolicitudOTEspecial.Encabezado.CardCodeOrig = p_strCardCodeOriginal
            ' udoSolicitudOTEspecial.Encabezado.CardNameOrig = p_strCardNameOriginal
            udoSolicitudOTEspecial.Encabezado.NumeroOTPadre = p_drwOrdenNueva.NoOrden
            udoSolicitudOTEspecial.Encabezado.NumeroVisita = CStr(p_drwOrdenNueva.NoVisita)
            udoSolicitudOTEspecial.Encabezado.OTReferencia = p_drwOrdenNueva.NoOrden
            udoSolicitudOTEspecial.Encabezado.EstadoOT = My.Resources.ResourceFrameWork.EstadoOrdenNoIniciada

            udoSolicitudOTEspecial.Encabezado.CotizacionCreada = "N"

            Call CrearLineasSolicitudOTEspecial(p_dstItems, udoSolicitudOTEspecial)

            udoSolicitudOTEspecial.Insert()

            intResultado = CInt(udoSolicitudOTEspecial.Encabezado.DocEntry)

            Return intResultado


        End Function

        Public Function BuscarIdSucursal_En_Cotizacion(ByVal p_NoCotizacion As String) As Integer

            Dim objCotizacion As SAPbobsCOM.Documents

            objCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If objCotizacion.GetByKey(p_NoCotizacion) Then

                Dim IdSucursal As String = objCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                If Not objCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objCotizacion)
                    objCotizacion = Nothing
                End If
                If Not String.IsNullOrEmpty(IdSucursal) Then
                    Return IdSucursal
                Else
                    Return 0
                End If
            End If

        End Function

        Public Sub ActualizarIdLineasHijasPaquetes(ByVal p_intNumeroCotizacion As Integer)

            Dim m_oCotizacionEspecial As SAPbobsCOM.Documents
            Dim m_oLineasCotizacionEspecial As SAPbobsCOM.Document_Lines

            Dim m_oCotizacionPadre As SAPbobsCOM.Documents
            Dim m_oLineasCotizacionPadre As SAPbobsCOM.Document_Lines
            Dim KitPertenece As String = String.Empty
            Dim intSeguirBusquedalinea As Integer = 0

            Dim ListaId As Generic.IList(Of String) = New Generic.List(Of String)

            m_oCotizacionEspecial = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            m_oCotizacionPadre = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If m_oCotizacionEspecial.GetByKey(p_intNumeroCotizacion) Then

                Dim OT_Padre As String = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_OT_Padre").Value

                Dim DocEntryPadre As String = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT where U_SCGD_Numero_OT = '" & OT_Padre & "'", strConexionSBO)

                m_oCotizacionPadre.GetByKey(DocEntryPadre)

                m_oLineasCotizacionPadre = m_oCotizacionPadre.Lines

                m_oLineasCotizacionEspecial = m_oCotizacionEspecial.Lines

                For i As Integer = 0 To m_oLineasCotizacionEspecial.Count - 1

                    m_oLineasCotizacionEspecial.SetCurrentLine(i)

                    Dim itemcodeOE As String = m_oLineasCotizacionEspecial.ItemCode

                    Dim s As String = m_oLineasCotizacionEspecial.TreeType

                    For j As Integer = intSeguirBusquedalinea To m_oLineasCotizacionPadre.Count - 1

                        m_oLineasCotizacionPadre.SetCurrentLine(j)

                        Dim itemcodePadre As String = m_oLineasCotizacionPadre.ItemCode

                        Dim s_Padre As String = m_oLineasCotizacionEspecial.TreeType

                        If itemcodeOE = itemcodePadre Then

                            If m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then

                                Dim idlinearepuestoPadre As Decimal = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

                                Dim idlinearepuestoHijo As Decimal = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

                                If Not idlinearepuestoPadre = 0 Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

                                            m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value


                                            ListaId.Add(idlinearepuestoPadre)
                                            intSeguirBusquedalinea = j

                                            Exit For

                                        End If
                                    End If
                                Else
                                    intSeguirBusquedalinea = j

                                    Exit For
                                End If

                            ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then

                                Dim idlinearepuestoPadre As Decimal = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

                                Dim idlinearepuestoHijo As Decimal = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                If Not idlinearepuestoPadre = 0 Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

                                            m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                            ListaId.Add(idlinearepuestoPadre)
                                            intSeguirBusquedalinea = j

                                            Exit For

                                        End If
                                    End If
                                Else
                                    intSeguirBusquedalinea = j

                                    Exit For
                                End If


                            ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree Then

                                Dim idlinearepuestoPadre As Decimal = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

                                Dim idlinearepuestoHijo As Decimal = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

                                If Not idlinearepuestoPadre = 0 Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

                                            m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value


                                            ListaId.Add(idlinearepuestoPadre)
                                            intSeguirBusquedalinea = j

                                            Exit For

                                        End If

                                    End If
                                Else
                                    intSeguirBusquedalinea = j
                                    Exit For
                                End If
                            End If

                        End If
                    Next
                Next

            End If
            ListaId.Clear()
            m_oCotizacionEspecial.Update()


        End Sub

        Public Sub LlenarDataTablVisOrders()

        End Sub

        Public Sub SolicitudIdLineasHijasPaquetes(ByVal p_OTPadre As String, ByVal p_dstItems As QUT1Dataset)

            Dim m_oCotizacionPadre As SAPbobsCOM.Documents
            Dim m_oLineasCotizacionPadre As SAPbobsCOM.Document_Lines

            Dim drwItem As QUT1Dataset.QUT1Row

            m_oCotizacionPadre = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            Dim DocEntryPadre As String = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT where U_SCGD_Numero_OT = '" & p_OTPadre & "'", strConexionSBO)

            m_oCotizacionPadre.GetByKey(DocEntryPadre)
            m_oLineasCotizacionPadre = m_oCotizacionPadre.Lines


            For i As Integer = 0 To m_oLineasCotizacionPadre.Count - 1

                m_oLineasCotizacionPadre.SetCurrentLine(i)

                drwPaquetes = dtPaquetes.CotizacionPadrePaquetes.NewCotizacionPadrePaquetesRow()

                drwPaquetes("ItemCode") = m_oLineasCotizacionPadre.ItemCode
                drwPaquetes("LineNum") = m_oLineasCotizacionPadre.LineNum

                drwPaquetes("IdRepuestosXOrden") = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value



                If m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then

                    drwPaquetes("TreeType") = "S"
                ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                    drwPaquetes("TreeType") = "I"
                ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree Then
                    drwPaquetes("TreeType") = "N"

                End If

                dtPaquetes.CotizacionPadrePaquetes.Rows.Add(drwPaquetes)
                dtPaquetes.AcceptChanges()

            Next

        End Sub



        Public Sub IniciarProceso()

            If Not oCompany.InTransaction Then

                oCompany.StartTransaction()

            End If

        End Sub

        Public Sub FinalizarProceso()

            If oCompany.InTransaction Then

                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

            End If

        End Sub

        Public Sub RetrocederProceso()

            If oCompany.InTransaction Then

                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

            End If

        End Sub

        Private Sub ActualizarLineasCotizacion(ByVal p_dstItems As QUT1Dataset)

            Dim drwItem As QUT1Dataset.QUT1Row
            Dim blnAgregarFila As Boolean = False
            Dim g_blnLineaAgregada As Boolean = False
            Dim strConsultaTipoArt As String = "Select Distinct(U_SCGD_TipoArticulo) from OITM as oi with(nolock) where ItemCode = '{0}' and ItemName = '{1}'"
            Dim dtResultado As System.Data.DataTable
            'Dim lineaSolicitud As SCG.DMSOne.Framework.LineaUDOSolOTEsp

            n = DIHelper.GetNumberFormatInfo(oCompany)

            For Each drwItem In p_dstItems.QUT1.Rows

                If drwItem.LineNum < 0 Then 'Se crea una linea de la cotización

                    If blnAgregarFila Then

                        objCotizacion.Lines.Add()

                    Else

                        blnAgregarFila = True

                    End If

                    objCotizacion.Lines.ItemCode = drwItem.itemCode

                    objCotizacion.Lines.ItemDescription = drwItem.itemName

                    objCotizacion.Lines.Quantity = drwItem.Quantity

                    If drwItem.IsDescuentoNull Then
                        objCotizacion.Lines.DiscountPercent = 0
                    Else
                        objCotizacion.Lines.DiscountPercent = drwItem.Descuento
                    End If
                    objCotizacion.Lines.DiscountPercent = drwItem.Descuento

                    If Not _UsaListaPreciosCliente Then
                        objCotizacion.Lines.Currency = drwItem.Moneda
                        objCotizacion.Lines.UnitPrice = drwItem.Precio
                    End If
                    If drwItem.FreeTxt IsNot DBNull.Value Then
                        If Not drwItem.IsFreeTxtNull Then

                            If drwItem.FreeTxt IsNot Nothing Or drwItem.FreeTxt <> "" Then
                                If drwItem.FreeTxt.Length <= 100 Then
                                    objCotizacion.Lines.FreeText = drwItem.FreeTxt
                                Else
                                    objCotizacion.Lines.FreeText = drwItem.FreeTxt.Substring(0, 100)
                                End If
                            End If
                        End If
                    End If

                  
                    Select Case drwItem.U_TipoArticulo

                        Case 1
                            If m_strImpuestoRepuestos <> "" Then
                                objCotizacion.Lines.TaxCode = m_strImpuestoRepuestos
                            End If
                        Case 2
                            If m_strImpuestoServicios <> "" Then
                                objCotizacion.Lines.TaxCode = m_strImpuestoServicios
                            End If
                        Case 3
                            If m_strImpuestoSuministros <> "" Then
                                objCotizacion.Lines.TaxCode = m_strImpuestoSuministros
                            End If
                        Case 4
                            If m_strImpuestoServiciosExternos <> "" Then
                                objCotizacion.Lines.TaxCode = m_strImpuestoServiciosExternos
                            End If
                        Case 5
                            If m_strImpuestoRepuestos <> "" Then
                                objCotizacion.Lines.TaxCode = m_strImpuestoRepuestos
                            End If
                    End Select
                    dtResultado = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaTipoArt, drwItem.itemCode, drwItem.itemName), strConexionSBO)

                    If dtResultado.Rows.Count > 0 Then



                        Select Case dtResultado.Rows(0)("U_SCGD_TipoArticulo").ToString

                            Case "1"
                                objCotizacion.Lines.UserFields.Fields.Item(mc_strTipoArt).Value = "1"
                            Case "2"
                                objCotizacion.Lines.UserFields.Fields.Item(mc_strTipoArt).Value = "2"
                            Case "3"
                                objCotizacion.Lines.UserFields.Fields.Item(mc_strTipoArt).Value = "3"
                            Case "4"
                                objCotizacion.Lines.UserFields.Fields.Item(mc_strTipoArt).Value = "4"
                            Case "5"
                                objCotizacion.Lines.UserFields.Fields.Item(mc_strTipoArt).Value = "5"
                        End Select
                    End If

                    If drwItem.ID <> -1 Then
                        objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = drwItem.ID
                    End If
                    Dim strCPendiente As String
                    Dim strCSolicitada As String
                    Dim strCRecibida As String
                    Dim strCPDevolucion As String
                    Dim strCPTraslado As String
                    Dim strCPBodega As String
                    Dim decCPendiente As Decimal
                    Dim decCSolicitada As Decimal
                    Dim decCRecibida As Decimal
                    Dim decCPDevolucion As Decimal
                    Dim decCPTraslado As Decimal
                    Dim decCPBodega As Decimal

                    strCPendiente = drwItem.CPen.ToString(n)
                    strCSolicitada = drwItem.CSol.ToString(n)
                    strCRecibida = drwItem.CRec.ToString(n)
                    strCPDevolucion = drwItem.CPDe.ToString
                    strCPTraslado = drwItem.CPTr.ToString
                    strCPBodega = drwItem.CPBo.ToString

                    If Not String.IsNullOrEmpty(strCPendiente) Then decCPendiente = Decimal.Parse(strCPendiente)
                    If Not String.IsNullOrEmpty(strCSolicitada) Then decCSolicitada = Decimal.Parse(strCSolicitada)
                    If Not String.IsNullOrEmpty(strCRecibida) Then decCRecibida = Decimal.Parse(strCRecibida)
                    If Not String.IsNullOrEmpty(strCPDevolucion) Then decCPDevolucion = Decimal.Parse(strCPDevolucion)
                    If Not String.IsNullOrEmpty(strCPTraslado) Then decCPTraslado = Decimal.Parse(strCPTraslado)
                    If Not String.IsNullOrEmpty(strCPBodega) Then decCPBodega = Decimal.Parse(strCPBodega)

                    'strCantidadSolicitado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                    'If Not String.IsNullOrEmpty(strCantidadSolicitado) Then decCantidadSolicitado = Decimal.Parse(strCantidadSolicitado)
                    'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = Double.Parse(decCantidadPendiente)

                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = drwItem.Costo.ToString
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCPendiente).Value = Double.Parse(decCPendiente)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCSolicitada).Value = Double.Parse(decCSolicitada)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCRecibida).Value = Double.Parse(decCRecibida)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCPendienteDevolucion).Value = Double.Parse(decCPDevolucion)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCPendienteTraslado).Value = Double.Parse(decCPTraslado)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCPendienteBodega).Value = Double.Parse(decCPBodega)
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCompra).Value = drwItem.Compra.ToString

                    If Not drwItem.IsEntregadoNull Then
                        objCotizacion.Lines.UserFields.Fields.Item(mc_strEntregado).Value = drwItem.Entregado.ToString()
                    End If

                Else  'Se actualiza una línea ya existente de la cotización
                    objCotizacion.Lines.SetCurrentLine(drwItem.LineNum)
                    objCotizacion.Lines.Quantity = drwItem.Quantity
                    objCotizacion.Lines.DiscountPercent = drwItem.Descuento

                    If Not _UsaListaPreciosCliente Then
                        objCotizacion.Lines.Currency = drwItem.Moneda
                        objCotizacion.Lines.UnitPrice = drwItem.Precio
                    End If
                    If drwItem.FreeTxt IsNot DBNull.Value Then
                        If drwItem.FreeTxt IsNot Nothing Or drwItem.FreeTxt <> "" Then
                            objCotizacion.Lines.FreeText = drwItem.FreeTxt
                        End If
                    End If
                    blnAgregarFila = True

                End If
            Next

        End Sub


        Private Sub CrearLineasSolicitudOTEspecial(ByVal p_dstItems As QUT1Dataset, Optional ByRef udoOTEspecial As SCG.DMSOne.Framework.UDOSolOTEsp = Nothing)

            Dim drwItem As QUT1Dataset.QUT1Row
            Dim blnAgregarFila As Boolean = False
            Dim g_blnLineaAgregada As Boolean = False
            Dim lineaSolicitud As SCG.DMSOne.Framework.LineaUDOSolOTEsp
            Dim strConsultaTipoArt As String = "Select Distinct(U_SCGD_TipoArticulo) from OITM as oi with(nolock) where ItemCode = '{0}' and ItemName = '{1}'"
            Dim dtResultado As System.Data.DataTable


            For Each drwItem In p_dstItems.QUT1.Rows

                If drwItem.LineNum < 0 Then

                    If g_blnLineaAgregada = False Then

                        udoOTEspecial.ListaLineas = New SCG.DMSOne.Framework.ListaLineasUDOSolOTEsp()

                        udoOTEspecial.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

                        g_blnLineaAgregada = True

                    End If


                    lineaSolicitud = New SCG.DMSOne.Framework.LineaUDOSolOTEsp()

                    lineaSolicitud.ItemCode = drwItem.itemCode
                    lineaSolicitud.Description = drwItem.itemName
                    lineaSolicitud.Cantidad = drwItem.Quantity

                    If drwItem.IsDescuentoNull Then
                        lineaSolicitud.PorcentajeDescuento = 0
                    Else
                        lineaSolicitud.PorcentajeDescuento = drwItem.Descuento
                    End If

                    'lineaSolicitud.PorcentajeDescuento = drwItem.Descuento

                    If Not _UsaListaPreciosCliente Then

                        lineaSolicitud.Moneda = drwItem.Moneda
                        lineaSolicitud.Precio = drwItem.Precio
                    End If
                    If drwItem.FreeTxt IsNot DBNull.Value Then
                        If Not drwItem.IsFreeTxtNull Then

                            If drwItem.FreeTxt IsNot Nothing Or drwItem.FreeTxt <> "" Then
                                If drwItem.FreeTxt.Length <= 100 Then
                                    lineaSolicitud.Comentarios = drwItem.FreeTxt
                                Else
                                    lineaSolicitud.Comentarios = drwItem.FreeTxt.Substring(0, 100)
                                End If
                            End If
                        End If
                    End If
                    Select Case drwItem.U_TipoArticulo

                        Case 1
                            If m_strImpuestoRepuestos <> "" Then

                                lineaSolicitud.Impuestos = m_strImpuestoRepuestos
                            End If
                        Case 2
                            If m_strImpuestoServicios <> "" Then

                                lineaSolicitud.Impuestos = m_strImpuestoServicios
                            End If
                        Case 3
                            If m_strImpuestoSuministros <> "" Then

                                lineaSolicitud.Impuestos = m_strImpuestoSuministros
                            End If
                        Case 4
                            If m_strImpuestoServiciosExternos <> "" Then

                                lineaSolicitud.Impuestos = m_strImpuestoServiciosExternos
                            End If
                        Case 5
                            If m_strImpuestoRepuestos <> "" Then
                                lineaSolicitud.Impuestos = m_strImpuestoRepuestos
                            End If
                    End Select
                    If drwItem.ID <> -1 Then
                        lineaSolicitud.IdRepuestosXOrden = drwItem.ID
                    End If

                    dtResultado = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaTipoArt, drwItem.itemCode, drwItem.itemName), strConexionSBO)

                    If dtResultado.Rows.Count > 0 Then



                        Select Case dtResultado.Rows(0)("U_SCGD_TipoArticulo").ToString

                            Case "1"
                                lineaSolicitud.TipoArticulo = "1"
                            Case "2"
                                lineaSolicitud.TipoArticulo = "2"
                            Case "3"
                                lineaSolicitud.TipoArticulo = "3"
                            Case "4"
                                lineaSolicitud.TipoArticulo = "4"
                            Case "5"
                                lineaSolicitud.TipoArticulo = "5"
                        End Select
                    End If

                    lineaSolicitud.Costo = drwItem.Costo.ToString

                    lineaSolicitud.Seleccionar = "N"

                    lineaSolicitud.CantPendBodega = drwItem.CPBo

                    lineaSolicitud.CantPendDevolucion = drwItem.CPDe

                    lineaSolicitud.CantPendiente = drwItem.CPen

                    lineaSolicitud.CantPendTraslado = drwItem.CPTr

                    lineaSolicitud.CantRecibida = drwItem.CRec

                    lineaSolicitud.CantSolicitada = drwItem.CSol

                    lineaSolicitud.Compra = drwItem.Compra


                    udoOTEspecial.ListaLineas.LineasUDO.Add(lineaSolicitud)

                End If
            Next

        End Sub

        Public Sub CrearCotizacion_OT_EspecialesAprobadas(ByVal p_numeroSolicitud As Integer)

            Dim objCotizacionCreada As CotizacionCLS
            Dim DataTableEncabezado As System.Data.DataTable
            Dim DataTableDetalle As System.Data.DataTable
            Dim datarowSolicitud As System.Data.DataRow
            Dim blnCreaOT As Boolean = False
            Dim strMensajeError As String = String.Empty

            Dim objCotizacion As SAPbobsCOM.Documents
            objCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            Dim strSeparadorDecimalesSAP As String = String.Empty
            Dim strSeparadorMilesSAP As String = String.Empty


            DataTableDetalle = Utilitarios.EjecutarConsultaDataTable(" SELECT     [@SCGD_SOT_ESP].U_Cod_Clie, [@SCGD_SOT_ESP].U_Nom_Clie, [@SCGD_SOT_ESP].U_Cod_Ases, [@SCGD_SOT_ESP].U_Num_Coti, " & _
                                                                    "[@SCGD_SOT_ESP].U_TipoOrd, [@SCGD_SOT_ESP].U_OTRefer, [@SCGD_SOT_ESP].U_Cod_Uni, [@SCGD_SOT_ESP].U_Id_Vehi, [@SCGD_SOT_ESP].U_VIN, " & _
                                                                    "[@SCGD_SOT_ESP].U_Placa, [@SCGD_SOT_ESP].U_Anno, [@SCGD_SOT_ESP].U_klm, [@SCGD_SOT_ESP].U_Cod_Mar, [@SCGD_SOT_ESP].U_Cod_Mod, " & _
                                                                    "[@SCGD_SOT_ESP].U_Cod_Est, [@SCGD_SOT_ESP].U_Des_Mar, [@SCGD_SOT_ESP].U_Des_Mod, [@SCGD_SOT_ESP].U_Des_Est, [@SCGD_SOT_ESP].U_Fec_Ape, " & _
                                                                    "[@SCGD_SOT_ESP].U_Fec_Com, [@SCGD_SOT_ESP].U_No_Vis, [@SCGD_SOT_ESP].U_CardCodeOrig, [@SCGD_SOT_ESP].U_CardNameOrig, " & _
                                                                    "[@SCGD_SOT_ESP].U_OTPadre, [@SCGD_SOT_ESP].U_Estad_OT, [@SCGD_SOT_ESP].U_Series, [@SCGD_SOT_ESP].U_Comment, [@SCGD_SOT_ESP].U_CotCread, " & _
                                                                    "[@SCGD_SOT_ESP].U_Status, [@SCGD_SOT_ESP].U_CotRef, [@SCGD_SOT_ESP].U_NomTipOT, [@SCGD_SOT_ESP].U_NomAse, [@SCGD_SOT_ESP].U_ImpRecp, " & _
                                                                    "[@SCGD_LINEAS_SOT_ESP].U_ItemCode, [@SCGD_LINEAS_SOT_ESP].U_Descrip, [@SCGD_LINEAS_SOT_ESP].U_PorcDs, [@SCGD_LINEAS_SOT_ESP].U_Moned, " & _
                                                                    "[@SCGD_LINEAS_SOT_ESP].U_Precio, [@SCGD_LINEAS_SOT_ESP].U_Coment, [@SCGD_LINEAS_SOT_ESP].U_IdRxO, [@SCGD_LINEAS_SOT_ESP].U_Costo, " & _
                                                                    "[@SCGD_LINEAS_SOT_ESP].U_Cant, [@SCGD_LINEAS_SOT_ESP].U_Tax " & _
                                                                    "FROM         [@SCGD_SOT_ESP] INNER JOIN " & _
                                                                    "[@SCGD_LINEAS_SOT_ESP] ON [@SCGD_SOT_ESP].DocEntry = [@SCGD_LINEAS_SOT_ESP].DocEntry " & _
                                                                    "WHERE     ([@SCGD_SOT_ESP].DocEntry = = " & p_numeroSolicitud & "'", strConexionSBO)


            datarowSolicitud = DataTableDetalle.Rows(0)

            Dim blnAgregarFila As Boolean = False
            Dim intResultado As Integer

            objCotizacion.CardCode = datarowSolicitud.Item("U_Cod_Clie").Trim

            If Not datarowSolicitud.Item("U_Nom_Clie").Trim = String.Empty Then

                objCotizacion.CardName = datarowSolicitud.Item("U_Nom_Clie").Trim
            End If

            objCotizacion.Comments = datarowSolicitud.Item("U_OTRefer").Trim & " " & _
                datarowSolicitud.Item("U_Comment").Trim

            objCotizacion.Series = datarowSolicitud.Item("U_Series").Trim

            objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = datarowSolicitud.Item("U_Cod_Uni").Trim
            objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = datarowSolicitud.Item("U_Id_Vehi").Trim
            objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = datarowSolicitud.Item("U_Anno").Trim
            objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = datarowSolicitud.Item("U_Cod_Mar").Trim
            objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = datarowSolicitud.Item("U_Cod_Mod").Trim
            objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = datarowSolicitud.Item("U_VIN").Trim
            objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = datarowSolicitud.Item("U_Placa").Trim
            objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = datarowSolicitud.Item("U_Cod_Est").Trim
            objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = datarowSolicitud.Item("U_Des_Mar").Trim
            objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = datarowSolicitud.Item("U_Des_Mod").Trim
            objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = datarowSolicitud.Item("U_Des_Est").Trim
            objCotizacion.UserFields.Fields.Item(mc_strFechaRecepcion).Value = datarowSolicitud.Item("U_Fec_Ape").Trim
            objCotizacion.UserFields.Fields.Item(mc_strFechaCompromiso).Value = datarowSolicitud.Item("U_Fec_Com").Trim
            objCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = datarowSolicitud.Item("U_klm").Trim
            objCotizacion.UserFields.Fields.Item(mc_strCardCodeOrig).Value = datarowSolicitud.Item("U_CardCodeOrig").Trim
            objCotizacion.UserFields.Fields.Item(mc_strCardNameOrig).Value = datarowSolicitud.Item("U_CardNameOrig").Trim
            objCotizacion.UserFields.Fields.Item(mc_strNoVisita).Value = datarowSolicitud.Item("U_No_Vis").Trim
            objCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value = datarowSolicitud.Item("U_TipoOrd").Trim
            objCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value = datarowSolicitud.Item("U_OTRefer").Trim
            objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = datarowSolicitud.Item("U_OTRefer").Trim
            objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = "No iniciada"
            objCotizacion.DocumentsOwner = datarowSolicitud.Item("U_Cod_Ases").Trim
            objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "2"

            For Each drw As System.Data.DataRow In DataTableDetalle.Rows

                If blnAgregarFila Then

                    objCotizacion.Lines.Add()

                Else

                    blnAgregarFila = True

                End If

                Dim Precio As String = CStr(drw.Item("U_Precio")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim Costo As String = CStr(drw.Item("U_Costo")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim Cantidad As String = CStr(drw.Item("U_Cant")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim PorcDescuento As String = CStr(drw.Item("U_PorcDs")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

                Dim decPrecio As Decimal = Decimal.Parse(Precio)
                Dim decCosto As Decimal = Decimal.Parse(Costo)
                Dim decCantidad As Decimal = Decimal.Parse(Cantidad)
                Dim decPorcDescuento As Decimal = Decimal.Parse(PorcDescuento)

                objCotizacion.Lines.ItemCode = drw.Item("U_ItemCode")
                objCotizacion.Lines.Quantity = drw.Item("U_Cant")

                If Not drw.IsNull("U_PorcDs") Then
                    objCotizacion.Lines.DiscountPercent = drw.Item("U_PorcDs")
                Else
                    objCotizacion.Lines.DiscountPercent = 0
                End If

                If Not drw.IsNull("U_Moned") Then
                    objCotizacion.Lines.Currency = drw.Item("U_Moned")
                Else
                    objCotizacion.Lines.Currency = ""
                End If

                If Not drw.IsNull("U_Precio") Then
                    objCotizacion.Lines.UnitPrice = drw.Item("U_Precio")
                Else
                    objCotizacion.Lines.UnitPrice = 0
                End If

                objCotizacion.Lines.FreeText = drw.Item("U_Coment")

                objCotizacion.Lines.TaxCode = drw.Item("U_Tax")

                objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = drw.Item("U_IdRxO")

                If Not drw.IsNull("U_Costo") Then
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = drw.Item("U_Costo")
                Else
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = 0
                End If

                blnAgregarFila = True

            Next

            blnCreaOT = True
            If blnCreaOT Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = 1
            End If

            intResultado = objCotizacion.Add()

            If intResultado <> 0 Then
                oCompany.GetLastError(intResultado, strMensajeError)
                Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
            Else

                intResultado = CInt(oCompany.GetNewObjectKey())

                objCotizacionCreada = New CotizacionCLS(oCompany, intResultado)
                If Not objCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objCotizacion)
                    objCotizacion = Nothing
                End If
            End If
            
        End Sub

        Public Function VerificarFilasCotizacionEstadoPendienteBodega(ByVal p_intNumeroCotizacion As Integer) As Boolean

            Dim strCosnsulta As String = " SELECT COUNT(1) " & _
                                       " FROM QUT1 with (nolock) " & _
                                       " WHERE DocEntry = {0} AND " & _
                                       " (U_SCGD_Traslad IN (3,4) AND U_SCGD_Aprobado = 1) "

            If Utilitarios.EjecutarConsulta(String.Format(strCosnsulta, p_intNumeroCotizacion), strConexionSBO) > 0 Then
                Return False
            Else
                Return True
            End If
            
        End Function

        Public Function VerificarFilasCotizacionEnFaltaAprobacion(ByVal p_intNumeroCotizacion As Integer) As Boolean

            Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
            Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines
            Dim blnVerificarLineas As Boolean = False

            m_oBuscarCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If m_oBuscarCotizacion.GetByKey(p_intNumeroCotizacion) Then

                m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                For i As Integer = 0 To m_oLineasCotizacion.Count - 1

                    m_oLineasCotizacion.SetCurrentLine(i)

                    If m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 3 Then
                        Return blnVerificarLineas = True
                        Exit For
                    End If

                Next
                If Not m_oBuscarCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oBuscarCotizacion)
                    m_oBuscarCotizacion = Nothing
                End If
                Return blnVerificarLineas = False

            End If

        End Function

        ''' <summary>
        ''' Verifica que al menos una linea en la cotizacion esto en la columna de Aprobacion
        ''' en "Falta de Aprobacion" o "Aprobado No"
        ''' </summary>
        ''' <param name="p_intNumeroCotizacion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarFilasCotizacionEnFaltaAprobacionOAprobadoNoKits(ByVal p_intNumeroCotizacion As Integer) As Boolean

            Dim m_oBuscarCotizacion As SAPbobsCOM.Documents
            Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines
            Dim blnVerificarLineas As Boolean = False

            m_oBuscarCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If m_oBuscarCotizacion.GetByKey(p_intNumeroCotizacion) Then

                m_oLineasCotizacion = m_oBuscarCotizacion.Lines

                For i As Integer = 0 To m_oLineasCotizacion.Count - 1

                    m_oLineasCotizacion.SetCurrentLine(i)


                    If m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value <> 1 And m_oLineasCotizacion.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient And m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_OTHija").Value <> 1 And m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Procesar").Value = 1 Then
                        Return blnVerificarLineas = True
                        Exit For
                    End If

                Next
                If Not m_oBuscarCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oBuscarCotizacion)
                    m_oBuscarCotizacion = Nothing
                End If
                Return blnVerificarLineas = False

            End If

        End Function

        ''' <summary>
        ''' Valida si existen lineas sin entregar
        ''' </summary>
        ''' <param name="p_intNumeroCotizacion">DocEntry de la OT</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarFilasCotizacionFaltaEntregaRepuestos(ByVal p_intNumeroCotizacion As Integer) As Boolean
            
            If Utilitarios.EjecutarConsulta(String.Format(" SELECT COUNT(QUT1.Docentry) FROM QUT1 WITH (nolock) " &
                                            " INNER JOIN OITM WITH (nolock) ON QUT1.ItemCode = OITM.ItemCode " &
                                            " WHERE OITM.U_SCGD_TipoArticulo IN (1,3) AND QUT1.U_SCGD_Entregado = 'N' " &
                                            " AND QUT1.DocEntry = '{0}' AND QUT1.U_SCGD_Aprobado = 1 ", p_intNumeroCotizacion), strConexionSBO) = 0 Then
                Return True
            Else
                Return False
            End If

        End Function



#End Region

    End Class

End Namespace
