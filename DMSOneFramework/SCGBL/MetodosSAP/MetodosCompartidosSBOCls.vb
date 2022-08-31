Imports System
Imports System.IO
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.BLSBO
Imports SAPbobsCOM
Imports DMSOneFramework.SCGCommon
Imports System.Globalization
Imports SCG.SBOFramework

Namespace SCGBusinessLogic
    Public Class MetodosCompartidosSBOCls
#Region "Declaraciones"

        Private Enum EstadosCotizacion

            scgAprobada = 1
            scgFaltaAprobacion = 2
            scgNoAprobada = 3

        End Enum

        Private Const mc_strOrdenDeTrabajo As String = "U_SCGD_Numero_OT"
        Private Const mc_strFase As String = "U_SCGD_T_Fase"
        Private Const mc_strFecha As String = "U_Fecha_O"
        Private Const mc_strTipoDeRequisiscion As String = "U_TipoReq"
        Private Const mc_strCardCode As String = "CardCode"

        Private Const mc_strMarca As String = "U_SCGD_Des_Marc"
        Private Const mc_strModelo As String = "U_SCGD_Des_Mode"
        Private Const mc_strEstilo As String = "U_SCGD_Des_Esti"
        Private Const mc_strPlaca As String = "U_SCGD_Num_Placa"
        Private Const mc_strNoVIN As String = "U_SCGD_Num_VIN"
        Private Const mc_strAnio As String = "U_SCGD_Ano_Vehi"
        Private Const mc_strSeccRep As String = "U_SeccRep"
        Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"

        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strSeccion As String = "Seccion"
        Private Const mc_stridRepuestosxOrden As String = "idRepuestosxOrden"

        Private Const mc_stridSucursal As String = "U_SCGD_idSucursal"
        Private Const mc_strComponente As String = "Descripcion Rep"

        Private Const mc_strEstadoCotizacion As String = "U_SCGD_Estado_Cot"
        Private Const mc_strEstadoCotizacionID As String = "U_SCGD_Estado_CotID"

        Private Const mc_strCantSolicitados As String = "CantSolicitados"

        Private Const mc_strEstadoTraslado As String = "U_SCGD_Traslad"
        Private Const mc_strItemAprobado As String = "U_SCGD_Aprobado"
        Private Const mc_strOtFinalizada As String = "U_SCGD_OTFinalizada"

        Private Const mc_strCodEspecifico As String = "U_SCGD_CodEspecifico"
        Private Const mc_strNombEspecifico As String = "U_SCGD_NombEspecific"

        Private Const mc_srtPrecioCompraReal As String = "PrecioCompraReal"

        Private Const mc_strGuion As String = "-"
        ' Public objSCGMSGBox As New Proyecto_SCGMSGBox.SCGMSGBox("Sistema de Taller")

        Public Enum EstadoDeTransaccion
            Commit
            Rollback
        End Enum

        Public Shared n As NumberFormatInfo


#Region "Objetos"

        Shared m_oCotizacion As SAPbobsCOM.Documents


#End Region

#End Region

#Region "Constructor"
        Public Sub New(ByVal Comp As String, ByVal Serv As String, ByVal bd As String,
                       ByVal user As String, ByVal pass As String)

            _CompanyL = Comp
            _ServerL = Serv
            _DBSBOL = bd
            _UserDBL = user
            _PassDBL = pass

            'n = DIHelper.GetNumberFormatInfo(oCompany)

        End Sub

#End Region

        Public Shared Function IniciaTransaccion() As Boolean

            Try

                Call oCompany.StartTransaction()

                Return True
            Catch ex As Exception
                Throw ex
                'Call MsgBox(ex.Message)
                Return False
            Finally

            End Try
        End Function

        Public Shared Sub IniciarCotizacion(ByVal p_intNumCotizacion As Integer)

            m_oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            m_oCotizacion.GetByKey(p_intNumCotizacion)

        End Sub

        'Public Shared Sub FinaliziarCotizacion()

        '    m_oCotizacion.Update()

        'End Sub

        Public Shared Function FinalizaTransaccion(ByVal endtype As EstadoDeTransaccion) As Boolean

            Try

                If oCompany.InTransaction Then
                    Call oCompany.EndTransaction(endtype)
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
                'Call MsgBox(ex.Message)
                Return False
            Finally

            End Try
        End Function

        Public Shared Function GeneraOfertaDeCompra(ByVal NoOrdenTrabajo As String, _
                                                  ByVal Fecha As Date, _
                                                  ByVal CodProveedor As String, _
                                                  ByVal Marca As String, _
                                                  ByVal Modelo As String, _
                                                  ByVal Nochasis As String, _
                                                  ByVal Anio As String, _
                                                  ByVal dtbLineasOrdenCompra As DataTable, _
                                                  ByVal idSucursal As String, _
                                                  ByRef NoPedidoenSAP As String, _
                                                  ByVal NoSerie As Integer, _
                                                  ByRef DocNum As Integer, _
                                                  ByVal p_blnGeneraXML As Boolean, _
                                                  ByVal p_strRutaXML As String, _
                                                  ByVal p_strAsesor As String, _
                                                  ByVal p_strEstilo As String, _
                                                  ByVal p_strPlaca As String, _
                                                  ByVal p_strBodegaProceso As String, _
                                                  ByVal p_strCentroBeneficio As String, _
                                                  ByVal p_strDetalle As String, _
                                                  ByVal p_codMarca As String, _
                                                  ByVal p_strTipoOT As String, _
                                                  ByVal p_codTipoArt As String) As Boolean

            Dim intIndice As Integer

            Try
                Dim objOfertaDeCompra As SAPbobsCOM.Documents
                Dim objTransferencias As New TransferenciaItems(G_objCompany)
                Dim lngError As Long
                Dim strError As String = ""
                Dim strEtiquetaSerie As String = ""
                Dim strBodega As String = ""
                Dim strCentroBeneficio As String = String.Empty

                strBodega = p_strBodegaProceso
                strCentroBeneficio = p_strCentroBeneficio
                intIndice = 0

                objOfertaDeCompra = DirectCast(oCompany.GetBusinessObject(540000006),  _
                                              SAPbobsCOM.Documents)


                '---------------------------------------Manejo de indicadores: 09/05/2012------------------------------------------------
                'Obtiene el indicador por default para el tipo de documento: Oferta de venta
                'Oferta de Venta [Tipo 11]
                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores("11", CompanyL, ServerL, Dbsbol, UserDbl, PassDbl)

                If Not String.IsNullOrEmpty(strIndicador) Then

                    objOfertaDeCompra.Indicator = strIndicador

                End If

                objOfertaDeCompra.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO

                'proyectos sap
                Dim strProyecto As String = String.Empty
                strProyecto = Utilitarios.DevuelveCodeProyecto(NoOrdenTrabajo, CompanyL, ServerL, Dbsbol, UserDbl, PassDbl)

                If Not String.IsNullOrEmpty(strProyecto) Then objOfertaDeCompra.Project = strProyecto

                With objOfertaDeCompra.UserFields.Fields

                    .Item(mc_strOrdenDeTrabajo).Value = NoOrdenTrabajo
                    If Marca <> "(Empty)" And Marca <> "(Nothing)" Then
                        .Item(mc_strMarca).Value = Marca
                    End If
                    If Modelo <> "(Empty)" And Modelo <> "(Nothing)" Then
                        .Item(mc_strModelo).Value = Modelo
                    End If
                    If p_strPlaca <> "(Empty)" And p_strPlaca <> "(Nothing)" Then
                        .Item(mc_strPlaca).Value = p_strPlaca
                    End If
                    If p_strEstilo <> "(Empty)" And p_strEstilo <> "(Nothing)" Then
                        .Item(mc_strEstilo).Value = p_strEstilo
                    End If
                    If Nochasis <> "(Empty)" And Nochasis <> "(Nothing)" Then
                        .Item(mc_strNoVIN).Value = Nochasis
                    End If
                    .Item(mc_strAnio).Value = Anio
                    .Item(mc_stridSucursal).Value = idSucursal


                    If p_codMarca <> "(Empty)" And p_codMarca <> "(Nothing)" Then
                        .Item("U_SCGD_Cod_Marca").Value = p_codMarca
                    End If

                End With
                If p_strAsesor <> "" Then
                    objOfertaDeCompra.DocumentsOwner = p_strAsesor
                End If
                objOfertaDeCompra.Comments = My.Resources.ResourceFrameWork.OT_Referencia & ": " & NoOrdenTrabajo & " , " & p_strDetalle
                objOfertaDeCompra.CardCode = CodProveedor
                objOfertaDeCompra.DocDate = Fecha
                objOfertaDeCompra.RequriedDate = Fecha
                objOfertaDeCompra.Series = NoSerie

                objOfertaDeCompra.Lines.ItemCode = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto)
                objOfertaDeCompra.Lines.ItemDescription = dtbLineasOrdenCompra.Rows(intIndice)(mc_strComponente)
                objOfertaDeCompra.Lines.Quantity = dtbLineasOrdenCompra.Rows(intIndice)(mc_strCantSolicitados)

                If String.IsNullOrEmpty(strBodega) Then
                    strBodega = objTransferencias.RetornaBodegaProcesoByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                End If

                objOfertaDeCompra.Lines.WarehouseCode = strBodega

                If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransferencias.RetornaCentroBeneficioByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                If Not String.IsNullOrEmpty(strCentroBeneficio) Then objOfertaDeCompra.Lines.CostingCode = strCentroBeneficio
                'objOrdenDeCompra.Lines.ItemDescription = dtbLineasOrdenCompra.Rows(intIndice)(mc_strComponente)

                objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden)

                ManejaCotizacionAdicionales(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden), dtbLineasOrdenCompra)


                If dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico") IsNot System.Convert.DBNull Then
                    objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strCodEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico")
                End If
                If dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico") IsNot System.Convert.DBNull Then
                    objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strNombEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico")
                End If

                'agrego el numero de orden a las lineas de la Oferta de Compra
                If dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden) IsNot System.Convert.DBNull Then
                    objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden)
                End If

                If Not dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) Is System.DBNull.Value Then
                    If dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) > 0 Then
                        objOfertaDeCompra.Lines.UnitPrice = dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal)
                    End If
                End If

                'proyectos
                If Not String.IsNullOrEmpty(strProyecto) Then objOfertaDeCompra.Lines.ProjectCode = strProyecto

                objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_strTipoOT
                objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = p_codMarca
                objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = idSucursal
                objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = p_codTipoArt

                For i As Integer = 0 To m_oCotizacion.Lines.Count - 1
                    m_oCotizacion.Lines.SetCurrentLine(i)
                    If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden) Then
                        objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                        Exit For
                    End If
                Next

                For intIndice = 1 To dtbLineasOrdenCompra.Rows.Count - 1

                    objOfertaDeCompra.Lines.Add()

                    strBodega = p_strBodegaProceso
                    strCentroBeneficio = p_strCentroBeneficio

                    objOfertaDeCompra.Lines.ItemCode = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto)
                    objOfertaDeCompra.Lines.ItemDescription = dtbLineasOrdenCompra.Rows(intIndice)(mc_strComponente)

                    If String.IsNullOrEmpty(strBodega) Then
                        strBodega = objTransferencias.RetornaBodegaProcesoByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                    End If

                    objOfertaDeCompra.Lines.WarehouseCode = strBodega

                    If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransferencias.RetornaCentroBeneficioByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                    If Not String.IsNullOrEmpty(strCentroBeneficio) Then objOfertaDeCompra.Lines.CostingCode = strCentroBeneficio

                    objOfertaDeCompra.Lines.Quantity = dtbLineasOrdenCompra.Rows(intIndice)(mc_strCantSolicitados)

                    objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden)

                    If dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico") IsNot System.Convert.DBNull Then
                        objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strCodEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico")
                    End If
                    If dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico") IsNot System.Convert.DBNull Then
                        objOfertaDeCompra.Lines.UserFields.Fields.Item(mc_strNombEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico")
                    End If


                    'agrego el numero de orden a las lineas de la Oferta de Compra
                    If dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden) IsNot System.Convert.DBNull Then
                        objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden)
                    End If

                    If Not dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) Is System.DBNull.Value Then
                        If dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) > 0 Then
                            objOfertaDeCompra.Lines.UnitPrice = dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal)
                        End If
                    End If

                    'proyectos
                    If Not String.IsNullOrEmpty(strProyecto) Then objOfertaDeCompra.Lines.ProjectCode = strProyecto

                    objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_strTipoOT
                    objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = p_codMarca
                    objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = idSucursal
                    objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = p_codTipoArt

                    For i As Integer = 0 To m_oCotizacion.Lines.Count - 1
                        m_oCotizacion.Lines.SetCurrentLine(i)
                        If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden) Then
                            objOfertaDeCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                            Exit For
                        End If
                    Next

                Next intIndice

                objTransferencias.CerrarConexion()

                lngError = objOfertaDeCompra.Add()

                If lngError = 0 Then

                    Dim strDocEntry As String = ""

                    Call oCompany.GetNewObjectCode(strDocEntry)

                    objOfertaDeCompra.GetByKey(CInt(strDocEntry))

                    If oCompany.InTransaction Then
                        oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If

                    Call DevuelveEtiquetaDeSerie(NoSerie, oCompany, strEtiquetaSerie)

                    DocNum = CInt(objOfertaDeCompra.DocNum)

                    NoPedidoenSAP = strEtiquetaSerie & mc_strGuion & CStr(objOfertaDeCompra.DocNum)

                    If p_blnGeneraXML Then

                        Call GenerarXML(objOfertaDeCompra, p_strRutaXML)

                    End If

                End If

                If Not objOfertaDeCompra Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objOfertaDeCompra)
                    objOfertaDeCompra = Nothing
                End If

                If lngError = -2028 Then

                    MsgBox(lngError & " " & My.Resources.ResourceFrameWork.MensajeOFNoPuedeSerCreadaRepNoInventario, MsgBoxStyle.Information, "<SCG> DMS ONE")

                    'Call escribirArchivo(m_strPath, strMensaje)

                    Return False
                ElseIf lngError = -10 Then

                    MsgBox(lngError & " " & My.Resources.ResourceFrameWork.MensajeOFNoCreadaTipoCambio, MsgBoxStyle.Information, "<SCG> DMS ONE")

                    'Call escribirArchivo(m_strPath, strMensaje)

                    Return False

                ElseIf lngError <> 0 Then

                    Call oCompany.GetLastError(lErrCode, strError)

                    Dim strMensaje As String = My.Resources.ResourceFrameWork.Fecha & ": " & CStr(System.DateTime.Now) & vbCrLf & _
                                            My.Resources.ResourceFrameWork.LaOrden & ": " & NoOrdenTrabajo & " " & My.Resources.ResourceFrameWork.DeLaFecha & ": " & CStr(Fecha) & " " & My.Resources.ResourceFrameWork.MensajeNoseHaCreadoPor & ":" & vbCrLf & _
                                               lErrCode & " " & strError & vbCrLf

                    MsgBox(strMensaje)

                    Return False

                Else

                    Return True

                End If



            Catch ex As Exception
                MsgBox(ex.Message)
                Return False
            Finally
            End Try
        End Function

        Private Shared Sub ManejaCotizacionAdicionales(ByVal p_strNoOrden As String, ByVal p_dtbLineasOrdenCompra As DataTable)

            Dim oCotizacion As SAPbobsCOM.Documents
            Dim strConsulta As String = " Select DocEntry from SCGTA_VW_OQUT with (nolock) where U_SCGD_Numero_OT = '{0}' "
            Dim strNumeroCotizacion As String
            Dim intNumeroCotizacion As Integer = 0

            Dim cont As Integer = 0
            Dim strIdRepuestosxOrden As String = String.Empty
            Dim strCantSolicitada As String = String.Empty

            n = DIHelper.GetNumberFormatInfo(G_objCompany)


            strNumeroCotizacion = Utilitarios.EjecutarConsulta(String.Format(strConsulta, p_strNoOrden), strConexionADO)

            If Not String.IsNullOrEmpty(strNumeroCotizacion) Then intNumeroCotizacion = Integer.Parse(strNumeroCotizacion)

            oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If oCotizacion.GetByKey(intNumeroCotizacion) Then

                For cont = 0 To p_dtbLineasOrdenCompra.Rows.Count() - 1

                    strIdRepuestosxOrden = p_dtbLineasOrdenCompra.Rows(cont)(mc_stridRepuestosxOrden).ToString.Trim()
                    strCantSolicitada = p_dtbLineasOrdenCompra.Rows(cont)(mc_strCantSolicitados)
                    If String.IsNullOrEmpty(strCantSolicitada) Then
                        strCantSolicitada = 0
                    End If

                    For indCot As Integer = 0 To oCotizacion.Lines.Count - 1

                        oCotizacion.Lines.SetCurrentLine(indCot)

                        If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = strIdRepuestosxOrden Then

                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = Decimal.Parse(strCantSolicitada).ToString(n)
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                            Exit For
                        End If
                    Next
                Next

                oCotizacion.Update()
                If Not oCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                    oCotizacion = Nothing
                End If
            End If
        End Sub


        Public Shared Function GeneraOrdenDeCompra(ByVal NoOrdenTrabajo As String, _
                                                    ByVal Fecha As Date, _
                                                    ByVal CodProveedor As String, _
                                                    ByVal Marca As String, _
                                                    ByVal Modelo As String, _
                                                    ByVal Nochasis As String, _
                                                    ByVal Anio As String, _
                                                    ByVal dtbLineasOrdenCompra As DataTable, _
                                                    ByVal idSucursal As String, _
                                                    ByRef NoPedidoenSAP As String, _
                                                    ByVal NoSerie As Integer, _
                                                    ByRef DocNum As Integer, _
                                                    ByVal p_blnGeneraXML As Boolean, _
                                                    ByVal p_strRutaXML As String, _
                                                    ByVal p_strAsesor As String, _
                                                    ByVal p_strEstilo As String, _
                                                    ByVal p_strPlaca As String, _
                                                    ByVal p_strBodegaProceso As String, _
                                                    ByVal p_strCentroBeneficio As String, _
                                                    ByVal p_strDetalle As String, _
                                                    ByVal p_codMarca As String, _
                                                  ByVal p_strTipoOT As String, _
                                                  ByVal p_codTipoArt As String) As Boolean
            Dim intIndice As Integer

            Try

                Dim objOrdenDeCompra As SAPbobsCOM.Documents
                Dim objTransferencias As New TransferenciaItems(G_objCompany)
                Dim lngError As Long
                Dim strError As String = ""
                Dim strEtiquetaSerie As String = ""
                Dim strBodega As String = ""
                Dim strCentroBeneficio As String = String.Empty

                strBodega = p_strBodegaProceso
                strCentroBeneficio = p_strCentroBeneficio
                intIndice = 0


                objOrdenDeCompra = DirectCast(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                SAPbobsCOM.Documents)


                Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores("8", CompanyL, ServerL, Dbsbol, UserDbl, PassDbl)

                If Not String.IsNullOrEmpty(strIndicador) Then

                    objOrdenDeCompra.Indicator = strIndicador

                End If

                objOrdenDeCompra.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO


                'proyectos sap
                Dim strProyecto As String = String.Empty
                strProyecto = Utilitarios.DevuelveCodeProyecto(NoOrdenTrabajo, CompanyL, ServerL, Dbsbol, UserDbl, PassDbl)

                If Not String.IsNullOrEmpty(strProyecto) Then objOrdenDeCompra.Project = strProyecto


                With objOrdenDeCompra.UserFields.Fields

                    .Item(mc_strOrdenDeTrabajo).Value = NoOrdenTrabajo
                    If Marca <> "(Empty)" And Marca <> "(Nothing)" Then
                        .Item(mc_strMarca).Value = Marca
                    End If
                    If Modelo <> "(Empty)" And Modelo <> "(Nothing)" Then
                        .Item(mc_strModelo).Value = Modelo
                    End If
                    If p_strPlaca <> "(Empty)" And p_strPlaca <> "(Nothing)" Then
                        .Item(mc_strPlaca).Value = p_strPlaca
                    End If
                    If p_strEstilo <> "(Empty)" And p_strEstilo <> "(Nothing)" Then
                        .Item(mc_strEstilo).Value = p_strEstilo
                    End If
                    If Nochasis <> "(Empty)" And Nochasis <> "(Nothing)" Then
                        .Item(mc_strNoVIN).Value = Nochasis
                    End If
                    .Item(mc_strAnio).Value = Anio
                    .Item(mc_stridSucursal).Value = idSucursal

                    If p_codMarca <> "(Empty)" And p_codMarca <> "(Nothing)" Then
                        .Item("U_SCGD_Cod_Marca").Value = p_codMarca
                    End If


                End With
                If p_strAsesor <> "" Then
                    objOrdenDeCompra.DocumentsOwner = p_strAsesor
                End If
                objOrdenDeCompra.Comments = My.Resources.ResourceFrameWork.OT_Referencia & ": " & NoOrdenTrabajo & " , " & p_strDetalle
                objOrdenDeCompra.CardCode = CodProveedor
                objOrdenDeCompra.DocDate = Fecha
                objOrdenDeCompra.Series = NoSerie

                objOrdenDeCompra.Lines.ItemCode = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto)
                objOrdenDeCompra.Lines.ItemDescription = dtbLineasOrdenCompra.Rows(intIndice)(mc_strComponente)
                objOrdenDeCompra.Lines.Quantity = dtbLineasOrdenCompra.Rows(intIndice)(mc_strCantSolicitados)

                If String.IsNullOrEmpty(strBodega) Then
                    strBodega = objTransferencias.RetornaBodegaProcesoByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                End If

                objOrdenDeCompra.Lines.WarehouseCode = strBodega

                If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransferencias.RetornaCentroBeneficioByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                If Not String.IsNullOrEmpty(strCentroBeneficio) Then objOrdenDeCompra.Lines.CostingCode = strCentroBeneficio

                objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden)


                ManejaCotizacionAdicionales(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden), dtbLineasOrdenCompra)

                If dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico") IsNot System.Convert.DBNull Then
                    objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strCodEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico")
                End If
                If dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico") IsNot System.Convert.DBNull Then
                    objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strNombEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico")
                End If

                'agrego el numero de orden a las lineas de la Orden de Compra
                If dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden) IsNot System.Convert.DBNull Then
                    objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden)
                End If

                If Not dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) Is System.DBNull.Value Then
                    If dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) > 0 Then
                        objOrdenDeCompra.Lines.UnitPrice = dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal)
                    End If
                End If
                'proyectos
                If Not String.IsNullOrEmpty(strProyecto) Then objOrdenDeCompra.Lines.ProjectCode = strProyecto

                objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_strTipoOT
                objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = p_codMarca
                objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = idSucursal
                objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = p_codTipoArt

                For i As Integer = 0 To m_oCotizacion.Lines.Count - 1
                    m_oCotizacion.Lines.SetCurrentLine(i)
                    If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden) Then
                        objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                        Exit For
                    End If
                Next

                For intIndice = 1 To dtbLineasOrdenCompra.Rows.Count - 1

                    objOrdenDeCompra.Lines.Add()

                    strBodega = p_strBodegaProceso
                    strCentroBeneficio = p_strCentroBeneficio

                    objOrdenDeCompra.Lines.ItemCode = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto)
                    objOrdenDeCompra.Lines.ItemDescription = dtbLineasOrdenCompra.Rows(intIndice)(mc_strComponente)

                    If String.IsNullOrEmpty(strBodega) Then
                        strBodega = objTransferencias.RetornaBodegaProcesoByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                    End If
                    objOrdenDeCompra.Lines.WarehouseCode = strBodega

                    If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransferencias.RetornaCentroBeneficioByItem(dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoRepuesto))
                    If Not String.IsNullOrEmpty(strCentroBeneficio) Then objOrdenDeCompra.Lines.CostingCode = strCentroBeneficio

                    objOrdenDeCompra.Lines.Quantity = dtbLineasOrdenCompra.Rows(intIndice)(mc_strCantSolicitados)

                    objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden)


                    If dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico") IsNot System.Convert.DBNull Then
                        objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strCodEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("CodEspecifico")
                    End If
                    If dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico") IsNot System.Convert.DBNull Then
                        objOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strNombEspecifico).Value = dtbLineasOrdenCompra.Rows(intIndice)("NomEspecifico")
                    End If

                    'agrego el numero de orden a las lineas de la Orden de Compra
                    If dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden) IsNot System.Convert.DBNull Then
                        objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_strNoOrden)
                    End If

                    If Not dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) Is System.DBNull.Value Then
                        If dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal) > 0 Then
                            objOrdenDeCompra.Lines.UnitPrice = dtbLineasOrdenCompra.Rows(intIndice)(mc_srtPrecioCompraReal)
                        End If
                    End If

                    'proyectos
                    If Not String.IsNullOrEmpty(strProyecto) Then objOrdenDeCompra.Lines.ProjectCode = strProyecto

                    objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_strTipoOT
                    objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = p_codMarca
                    objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = idSucursal
                    objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = p_codTipoArt

                    For i As Integer = 0 To m_oCotizacion.Lines.Count - 1
                        m_oCotizacion.Lines.SetCurrentLine(i)
                        If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = dtbLineasOrdenCompra.Rows(intIndice)(mc_stridRepuestosxOrden) Then
                            objOrdenDeCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                            Exit For
                        End If
                    Next

                Next intIndice

                objTransferencias.CerrarConexion()

                lngError = objOrdenDeCompra.Add()

                If lngError = 0 Then

                    Dim strDocEntry As String = ""

                    Call oCompany.GetNewObjectCode(strDocEntry)

                    objOrdenDeCompra.GetByKey(CInt(strDocEntry))

                    If oCompany.InTransaction Then
                        oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If

                    Call DevuelveEtiquetaDeSerie(NoSerie, oCompany, strEtiquetaSerie)

                    DocNum = CInt(objOrdenDeCompra.DocNum)

                    NoPedidoenSAP = strEtiquetaSerie & mc_strGuion & CStr(objOrdenDeCompra.DocNum)

                    If p_blnGeneraXML Then

                        Call GenerarXML(objOrdenDeCompra, p_strRutaXML)

                    End If

                End If

                If Not objOrdenDeCompra Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objOrdenDeCompra)
                    objOrdenDeCompra = Nothing
                End If

                If lngError = -2028 Then

                    Call oCompany.GetLastError(lngError, strError)

                    MsgBox(lngError & " " & My.Resources.ResourceFrameWork.MensajeOCNoPuedeSerCreadaRepNoInventario, MsgBoxStyle.Information, "<SCG> DMS ONE")

                    'Call escribirArchivo(m_strPath, strMensaje)

                    Return False
                ElseIf lngError = -10 Then

                    Call oCompany.GetLastError(lngError, strError)

                    MsgBox(lngError & " " & My.Resources.ResourceFrameWork.MensajeOCProveedorInactivo, MsgBoxStyle.Information, "<SCG> DMS ONE")

                    'Call escribirArchivo(m_strPath, strMensaje)

                    Return False

                ElseIf lngError <> 0 Then

                    Call oCompany.GetLastError(lErrCode, strError)

                    Dim strMensaje As String = My.Resources.ResourceFrameWork.Fecha & ": " & CStr(System.DateTime.Now) & vbCrLf & _
                                            My.Resources.ResourceFrameWork.LaOrden & ": " & NoOrdenTrabajo & " " & My.Resources.ResourceFrameWork.DeLaFecha & ": " & CStr(Fecha) & " " & My.Resources.ResourceFrameWork.MensajeNoseHaCreadoPor & ":" & vbCrLf & _
                                               lErrCode & " " & strError & vbCrLf

                    MsgBox(strMensaje)

                    Return False

                Else

                    Return True

                End If


                'End If



            Catch ex As Exception
                MsgBox(ex.Message)
                Return False
            Finally
            End Try
        End Function

        Public Shared Function DevuelveEtiquetaDeSerie(ByVal intSeries As Integer, _
                                                ByVal oCompany As SAPbobsCOM.Company, _
                                                ByRef strEtiquetadeSeries As String) As Boolean

            Dim strConsultaEtiquetadeSerie As String = "Select SeriesName" & _
                                                           " From SCGTA_VW_NNM1 with (nolock) " & _
                                                           " Where Series =" & CStr(intSeries)


            Try

                'Dim oRxecordSet As SAPbobsCOM.Rxecordset
                Dim adpDocMarketing As New SCGDataAccess.AccesoSBODataAdapter()
                Dim dstDocumento As New DataSet
                'oRxecordSet = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRxecordset)

                'Call oRxecordSet.DoQuery(strConsultaEtiquetadeSerie)
                adpDocMarketing.Fill(dstDocumento, strConsultaEtiquetadeSerie)

                'If oRxecordSet.RecordCount > 0 Then
                If dstDocumento.Tables.Count > 0 Then
                    'Call oRxecordSet.MoveFirst()

                    'strEtiquetadeSeries = oRxecordSet.Fields.Item("SeriesName").Value
                    strEtiquetadeSeries = dstDocumento.Tables(0).Rows(0)(0)

                    Return True
                End If

            Catch ex As Exception
                MsgBox(ex.Message)
                Return False
            End Try
        End Function

        'Public Shared Sub ReporteErroresSBO(ByVal strTextoMensaje As String, _
        '                                      ByVal pathAttachment As String)

        '    Dim strUsuarioReporte As String
        '    Dim sErrMsg As String = ""
        '    Dim lRetCode As Long
        '    Dim oMsg As SAPbobsCOM.Messages
        '    Dim strMensajeError As String = ""

        '    Try

        '        strUsuarioReporte = "manager" 'Configuration.ConfigurationSettings.AppSettings("UsuarioReporteErrores")

        '        oMsg = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
        '        oMsg.MessageText = "Algunas salidas de inventario no han sido creadas:  " & strTextoMensaje & Chr(13) '& "Mensaje de Error:  " & strMensajeError
        '        oMsg.Subject = "Error en proceso de Interfaz de Mixit"
        '        oMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High

        '        'there are two recipients in this message
        '        oMsg.Recipients.Add()

        '        oMsg.Attachments.Add()

        '        'set values for the first recipients
        '        oMsg.Recipients.SetCurrentLine(0)
        '        oMsg.Recipients.UserCode = strUsuarioReporte
        '        oMsg.Recipients.NameTo = strUsuarioReporte
        '        oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

        '        oMsg.Attachments.Item(0).FileName = pathAttachment

        '        'send the message
        '        lRetCode = oMsg.Add()
        '        If lRetCode <> 0 Then ' If the addition failed
        '            oCompany.GetLastError(lErrCode, sErrMsg)
        '            If lErrCode <> 0 Then
        '                'clsExceptionHandler.handException(lErrCode, sErrMsg, Application.StartupPath, gc_NombreAplicacion)
        '            End If
        '        End If

        '        strMensajeError = " "

        '    Catch ex As Exception
        '        Throw ex
        '        'clsExceptionHandler.handException(ex, Application.StartupPath, gc_NombreAplicacion)
        '    End Try
        'End Sub

        Public Shared Sub PonerIDRepXOrdALineas(ByVal p_intNumeroCotizacion As Integer, _
                                                     ByVal p_intLineNum As Integer, _
                                                     Optional ByVal p_strIDItem As String = "")


            Dim oCotizacion As SAPbobsCOM.Documents
            Dim intError As Integer
            Dim strMensaje As String = ""
            oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            oCotizacion.HandWritten = SAPbobsCOM.BoYesNoEnum.tYES

            If oCotizacion.GetByKey(p_intNumeroCotizacion) Then

                If oCotizacion.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
                    oCotizacion.Lines.SetCurrentLine(p_intLineNum)
                    oCotizacion.Lines.UserFields.Fields.Item("U_IdRepXOrd").Value = p_strIDItem
                End If

                intError = oCotizacion.Update()
                If intError <> 0 Then
                    oCompany.GetLastError(intError, strMensaje)
                    Throw New ExceptionsSBO(intError, strMensaje)
                End If
                If Not oCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                    oCotizacion = Nothing
                End If
            End If

        End Sub

        Public Shared Function AgregarItemCotizacion(ByVal p_intNumeroCotizacion As Integer, _
                                           ByVal p_strNoItem As String, _
                                           ByVal p_intCantidad As Double, _
                                           ByVal p_strObservaciones As String, _
                                           ByVal p_strImpuesto As String, _
                                           Optional ByVal p_dblPrecio As Double = -1, _
                                           Optional ByVal p_strCurrency As String = "", _
                                           Optional ByVal p_strItemCodeEspecifico As String = "", _
                                           Optional ByVal p_strItemNameEspecifico As String = "", _
                                           Optional ByVal p_intIDEmpleado As Integer = -1, _
                                           Optional ByVal p_strNombreEmpleado As String = "", _
                                           Optional ByVal p_DuracionEstandard As String = "", _
                                           Optional ByVal p_ShipToCode As String = "") As Integer

            Try

                ' Dim oCotizacion As SAPbobsCOM.Documents
                Dim dbDiscount As Double
                Dim strMensaje As String
                'oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                'oCotizacion.HandWritten = SAPbobsCOM.BoYesNoEnum.tYES

                'If oCotizacion.GetByKey(p_intNumeroCotizacion) Then

                Dim oItemSbo As Items
                oItemSbo = G_objCompany.GetBusinessObject(BoObjectTypes.oItems)

                If m_oCotizacion.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then

                    m_oCotizacion.Lines.Add()

                    oItemSbo.GetByKey(p_strNoItem)

                    m_oCotizacion.Lines.ItemCode = p_strNoItem

                    'Dim strShipCode As String = m_oCotizacion.ShipToCode

                    If Not String.IsNullOrEmpty(oItemSbo.BarCode) Then m_oCotizacion.Lines.BarCode = oItemSbo.BarCode
                    m_oCotizacion.Lines.Quantity = p_intCantidad
                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 3 'Se pone el estado como falto de aprobación

                    m_oCotizacion.Lines.ShipToCode = p_ShipToCode

                    If p_strObservaciones IsNot Nothing Or p_strObservaciones <> "" Then
                        m_oCotizacion.Lines.FreeText = p_strObservaciones
                    End If
                    If p_strItemCodeEspecifico <> "" Then
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodEspecifico").Value = p_strItemCodeEspecifico
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEspecific").Value = p_strItemNameEspecifico
                    End If
                    If p_intIDEmpleado <> -1 Then
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = p_intIDEmpleado.ToString()
                        If Not String.IsNullOrEmpty(p_strNombreEmpleado) Then
                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = p_strNombreEmpleado
                        End If
                    End If
                    If p_dblPrecio <> -1 Then

                        If p_strCurrency = "" Then
                            p_strCurrency = m_oCotizacion.DocCurrency
                        Else
                            m_oCotizacion.Lines.Currency = p_strCurrency
                        End If
                        dbDiscount = getItemDiscount(m_oCotizacion.CardCode, m_oCotizacion.Lines.ItemCode)

                        If m_oCotizacion.Lines.Price = 0 Then

                            m_oCotizacion.Lines.UnitPrice = p_dblPrecio
                            m_oCotizacion.Lines.DiscountPercent = dbDiscount

                        Else

                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PrecioAcordad").Value = m_oCotizacion.Lines.UnitPrice

                        End If
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PrecioAcordad").Value = CInt(p_dblPrecio)

                    End If

                    If p_strImpuesto <> "" Then
                        m_oCotizacion.Lines.TaxCode = p_strImpuesto
                    End If



                    'se agrega la duracion estandard a la linea de la cotizacion
                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = p_DuracionEstandard

                    '    oCotizacion.Lines.Add()
                    'intError = m_oCotizacion.Update()
                    'If intError <> 0 Then
                    '    oCompany.GetLastError(intError, strMensaje)
                    '    Throw New Exception(strMensaje)
                    'End If
                    'm_oCotizacion.GetByKey(p_intNumeroCotizacion)
                    m_oCotizacion.Lines.SetCurrentLine(m_oCotizacion.Lines.Count - 1)
                    Return m_oCotizacion.Lines.LineNum

                Else

                    strMensaje = My.Resources.ResourceFrameWork.MensajeNopuedeAgregarItems
                    Throw New Exception(strMensaje)
                End If




                'Else

                'strMensaje = "Documento no encontrado"
                'Throw New Exception(strMensaje)

                'End If

            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Shared Function ActualizarItemsCotizacionEstadoTrasl(ByRef p_drwRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow) As Boolean

            Dim oItems As SAPbobsCOM.Document_Lines
            Dim intCont As Integer
            Dim blnExiste As Boolean = False

            For intCont = 0 To m_oCotizacion.Lines.Count - 1

                m_oCotizacion.Lines.SetCurrentLine(intCont)

                oItems = m_oCotizacion.Lines

                With oItems

                    If .ItemCode = p_drwRepuestosxOrden.NoRepuesto And .LineNum = p_drwRepuestosxOrden.LineNum Then

                        If .UserFields.Fields.Item(mc_strItemAprobado).Value = 1 And .UserFields.Fields.Item(mc_strEstadoTraslado).Value = 0 Then

                            .UserFields.Fields.Item(mc_strEstadoTraslado).Value = 1

                            blnExiste = True

                        End If

                    End If

                End With

            Next

            Return blnExiste

        End Function

        Public Overloads Shared Function EliminarItemCotizacion(ByVal p_intLineNum As Integer, _
                                                                Optional ByVal p_strObservaciones As String = "") As Boolean

            Try

                Dim intNo As Int16 = 2
                Dim blnEliminado As Boolean = False

                m_oCotizacion.Lines.SetCurrentLine(p_intLineNum)
                If m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value <> intNo Then
                    m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = intNo
                    blnEliminado = True
                End If
                m_oCotizacion.Lines.UserFields.Fields.Item(mc_strEstadoTraslado).Value = 0
                m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PrecioAcordad").Value = CInt(m_oCotizacion.Lines.UnitPrice)
                m_oCotizacion.Lines.UnitPrice = 0
                If p_strObservaciones <> "" Then
                    m_oCotizacion.Lines.FreeText += p_strObservaciones
                End If

                Return blnEliminado

            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Overloads Shared Function AgregarEspecificoAItemCotizacion(ByVal p_intLineNum As Integer, _
                                                                          ByVal p_strCodEspecifico As String, _
                                                                          ByVal p_strNomEspecifico As String, _
                                                                          ByVal p_decPrecio As Decimal, _
                                                                          ByVal p_intCantidad As Integer, _
                                                                          Optional ByVal P_strCurrency As String = "") As Boolean


            Try

                Dim intNo As Int16 = 2
                Dim blnEliminado As Boolean = False

                m_oCotizacion.Lines.SetCurrentLine(p_intLineNum)
                m_oCotizacion.Lines.UserFields.Fields.Item("U_CodEspecifico").Value = p_strCodEspecifico
                m_oCotizacion.Lines.UserFields.Fields.Item("U_NombEspecifico").Value = p_strNomEspecifico
                m_oCotizacion.Lines.UserFields.Fields.Item("U_PrecioAcordado").Value = CInt(p_decPrecio)
                m_oCotizacion.Lines.UnitPrice = p_decPrecio
                m_oCotizacion.Lines.Quantity = p_intCantidad
                Return blnEliminado

            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Shared Sub TransfItemsCotizacion()
            Dim objTranfItemsSBO As TransferenciaItems

            Try

                objTranfItemsSBO = New TransferenciaItems(G_objCompany)

                objTranfItemsSBO.CrearTrasladoAddOn(m_oCotizacion)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        Public Shared Sub ActualizarCotizacion()
            Dim intResults As Integer
            Dim strResults As String

            Try

                intResults = m_oCotizacion.Update

                If intResults <> 0 Then

                    strResults = G_objCompany.GetLastErrorDescription

                    Throw New ExceptionsSBO(intResults, strResults)

                End If

            Catch ex As Exception

                Throw ex

            End Try
        End Sub

        Shared objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        'Actualizo el tiempo real de la OT
        Public Shared Function ActualizarTiempoReal(ByVal IdOT As String, _
                                                         ByVal ID As String, _
                                                         ByVal p_decTiempoReal As Decimal)
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim oLineasCot As SAPbobsCOM.Document_Lines
            Dim intCont As Integer = 0

            oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            oCotizacion.GetByKey(IdOT)

            oLineasCot = oCotizacion.Lines

            For intCont = 0 To oCotizacion.Lines.Count - 1
                oCotizacion.Lines.SetCurrentLine(intCont)
                oLineasCot = oCotizacion.Lines
                With oLineasCot
                    If oLineasCot.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = ID Then

                        .UserFields.Fields.Item("U_SCGD_TiempoReal").Value = Convert.ToString(p_decTiempoReal)

                    End If
                End With
            Next

            oCotizacion.Update()
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        End Function


        'Actualizo los costos por servicios de OT
        Public Shared Function ActualizarCostosServicios(ByVal IdOT As String, _
                                                         ByVal ID As String, _
                                                         ByVal CostoReal As String, _
                                                         ByVal CostoStandar As String)
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim oLineasCot As SAPbobsCOM.Document_Lines
            Dim intCont As Integer = 0
            Dim strCostoLocal As String = ""

            oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            oCotizacion.GetByKey(IdOT)

            oLineasCot = oCotizacion.Lines

            For intCont = 0 To oCotizacion.Lines.Count - 1

                oCotizacion.Lines.SetCurrentLine(intCont)

                oLineasCot = oCotizacion.Lines

                With oLineasCot

                    If oLineasCot.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = ID Then

                        'If objUtilitarios.TraerValorTiempo() Then
                        ''Se actualiza metodo para ajustar el correcto funcionamiento del costeo de servicios.
                        Dim objUtilitarios2 As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
                        Dim strConfServicios As String = objUtilitarios2.TraerConfiguracionServicios()
                        If Not String.IsNullOrEmpty(strConfServicios) Then
                            If strConfServicios.Trim = "1" Then
                                strCostoLocal = CostoStandar
                            ElseIf strConfServicios.Trim = "2" Then
                                strCostoLocal = CostoReal
                            Else
                                strCostoLocal = "0"
                            End If
                            .UserFields.Fields.Item("U_SCGD_Costo").Value = strCostoLocal
                        Else
                            strCostoLocal = "0"
                        End If
                    End If

                End With

            Next
            oCotizacion.Update()
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        End Function

        Public Shared Function ActualizarEstadoCotizacion(ByVal p_strNoCotizacion As String, ByVal p_strEstadoTxt As String, Optional ByVal p_blnCancelar As Boolean = False) As Integer
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim intResults As Integer
            Dim strResults As String

            Try

                oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                oCotizacion.GetByKey(p_strNoCotizacion)

                oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = p_strEstadoTxt

                Select Case p_strEstadoTxt
                    Case My.Resources.ResourceFrameWork.EstadoOrdenNoIniciada
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "1"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenProceso
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "2"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenSuspendido
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "3"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenFinalizada
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "4"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenCancelada
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "5"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenCerrada
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "6"
                    Case My.Resources.ResourceFrameWork.EstadoOrdenFacturada
                        oCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "7"

                End Select



                intResults = oCotizacion.Update

                If intResults <> 0 Then

                    strResults = G_objCompany.GetLastErrorDescription
                    If Not oCotizacion Is Nothing Then
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                        oCotizacion = Nothing
                    End If
                    Return intResults

                End If

                If p_blnCancelar And oCotizacion.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
                    oCotizacion.Cancel()
                End If
                If Not oCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                    oCotizacion = Nothing
                End If
                Return intResults

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Shared Function Actualiza_ValorOTFin_LineasCotizacion(ByVal p_strNoCotizacion As String) As Integer
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim intResults As Integer
            Dim strResults As String
            Dim oItems As SAPbobsCOM.Document_Lines
            Dim intCont As Integer
            Dim blnExiste As Boolean = False

            Try
                oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                oCotizacion.GetByKey(p_strNoCotizacion)

                For intCont = 0 To oCotizacion.Lines.Count - 1

                    oCotizacion.Lines.SetCurrentLine(intCont)
                    oItems = oCotizacion.Lines

                    With oItems
                        If String.IsNullOrEmpty(.UserFields.Fields.Item(mc_strOtFinalizada).Value.ToString()) And .UserFields.Fields.Item(mc_strItemAprobado).Value = 1 Then

                            .UserFields.Fields.Item(mc_strOtFinalizada).Value = "Y"

                        End If
                    End With
                Next
                intResults = oCotizacion.Update()
                If Not oCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                    oCotizacion = Nothing
                End If
                Return intResults

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Shared Sub ActualizarObservacionCotizacion(ByVal p_strNoCotizacion As String, ByVal p_strObservaciones As String)
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim intResults As Integer
            Dim strResults As String

            Try

                oCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                oCotizacion.GetByKey(p_strNoCotizacion)

                oCotizacion.Comments = p_strObservaciones

                intResults = oCotizacion.Update
                If Not oCotizacion Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                    oCotizacion = Nothing
                End If
                If intResults <> 0 Then

                    strResults = G_objCompany.GetLastErrorDescription

                    Throw New ExceptionsSBO(intResults, strResults)

                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Private Shared Sub GenerarXML(ByVal p_oDocumento As SAPbobsCOM.Documents, ByVal p_strRuta As String)

            If p_strRuta.Substring(p_strRuta.Length - 1, 1) <> "\" Then
                p_oDocumento.SaveXML(p_strRuta & "\OCO" & CStr(p_oDocumento.DocNum) & ".xml")
            Else
                p_oDocumento.SaveXML(p_strRuta & "OCO" & CStr(p_oDocumento.DocNum) & ".xml")
            End If

        End Sub
        ''' <summary>
        ''' Obtiene descuento de item para el SN
        ''' </summary>
        ''' <param name="p_cardCode">Codigo de SN</param>
        ''' <param name="p_itemCode">Codigo del Item</param>
        ''' <returns>Porcentaje de descuento</returns>
        ''' <remarks></remarks>
        Private Shared Function getItemDiscount(ByVal p_cardCode As String, ByVal p_itemCode As String) As Double

            Dim oSpecialPrice As SAPbobsCOM.SpecialPrices
            Dim oDiscountGroups As SAPbobsCOM.DiscountGroups
            Dim oBusinessPartners As SAPbobsCOM.BusinessPartners
            Dim oItem As SAPbobsCOM.Items
            Dim oItemProperties As SAPbobsCOM.ItemProperties
            Dim dbDiscount As Double = 0
            Dim count As Integer = 0

            Dim blnFirst As Boolean = True
            oSpecialPrice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices)
            oBusinessPartners = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oItem = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemProperties = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItemProperties)

            If oSpecialPrice.GetByKey(p_itemCode, p_cardCode) Then
                dbDiscount = oSpecialPrice.DiscountPercent
            End If

            If dbDiscount = 0 AndAlso oItem.GetByKey(p_itemCode) AndAlso oBusinessPartners.GetByKey(p_cardCode) Then
                oDiscountGroups = oBusinessPartners.DiscountGroups
                For index As Integer = 0 To oDiscountGroups.Count - 1
                    oDiscountGroups.SetCurrentLine(index)
                    Select Case oDiscountGroups.BaseObjectType
                        Case DiscountGroupBaseObjectEnum.dgboItemGroups
                            If oItem.ItemsGroupCode = oDiscountGroups.ObjectEntry Then
                                dbDiscount = oDiscountGroups.DiscountPercentage
                                Exit For
                            End If
                        Case DiscountGroupBaseObjectEnum.dgboItemProperties
                            If oItemProperties.GetByKey(oDiscountGroups.ObjectEntry) Then
                                If oItem.Properties(oItemProperties.Number) = BoYesNoEnum.tYES Then
                                    If Not blnFirst Then
                                        Select Case oBusinessPartners.DiscountRelations
                                            Case DiscountGroupRelationsEnum.dgrLowestDiscount
                                                If oDiscountGroups.DiscountPercentage < dbDiscount Then
                                                    dbDiscount = oDiscountGroups.DiscountPercentage
                                                End If
                                            Case DiscountGroupRelationsEnum.dgrHighestDiscount
                                                If oDiscountGroups.DiscountPercentage > dbDiscount Then
                                                    dbDiscount = oDiscountGroups.DiscountPercentage
                                                End If
                                            Case DiscountGroupRelationsEnum.dgrAverageDiscount
                                                dbDiscount += oDiscountGroups.DiscountPercentage
                                                count += 1

                                            Case DiscountGroupRelationsEnum.dgrDiscountTotals
                                                dbDiscount += oDiscountGroups.DiscountPercentage
                                            Case DiscountGroupRelationsEnum.dgrMultipliedDiscount

                                        End Select
                                    Else
                                        dbDiscount = oDiscountGroups.DiscountPercentage
                                        count += 1
                                        blnFirst = False
                                    End If
                                End If
                            End If
                        Case (DiscountGroupBaseObjectEnum.dgboManufacturer)
                            If oItem.Manufacturer = oDiscountGroups.ObjectEntry Then
                                dbDiscount = oDiscountGroups.DiscountPercentage
                                Exit For
                            End If
                        Case DiscountGroupBaseObjectEnum.dgboItems

                    End Select

                Next
                If count <> 0 AndAlso oBusinessPartners.DiscountRelations = DiscountGroupRelationsEnum.dgrAverageDiscount Then
                    dbDiscount /= count
                End If
            End If

            Utilitarios.DestruirObjeto(oSpecialPrice)
            Utilitarios.DestruirObjeto(oDiscountGroups)
            Utilitarios.DestruirObjeto(oBusinessPartners)
            Utilitarios.DestruirObjeto(oItem)
            Utilitarios.DestruirObjeto(oItemProperties)
            Return dbDiscount

        End Function

#Region "Propiedades para conexion"
        'propiedades de conexion para utilizacion en la funcion 
        'Devuelve Codigo Indicadores
        Private Shared _CompanyL As String
        Private Shared _ServerL As String
        Private Shared _DBSBOL As String
        Private Shared _UserDBL As String
        Private Shared _PassDBL As String

        'nombre de la compania
        Public Shared Property CompanyL As String
            Get
                Return _CompanyL
            End Get
            Set(ByVal value As String)
                _CompanyL = value
            End Set
        End Property

        'servidor 
        Public Shared Property ServerL As String
            Get
                Return _ServerL
            End Get
            Set(ByVal value As String)
                _ServerL = value
            End Set
        End Property

        'base de datos de SAP
        Public Shared Property Dbsbol As String
            Get
                Return _DBSBOL
            End Get
            Set(ByVal value As String)
                _DBSBOL = value
            End Set
        End Property

        'usuario de Base de datos
        Public Shared Property UserDbl As String
            Get
                Return _UserDBL
            End Get
            Set(ByVal value As String)
                _UserDBL = value
            End Set
        End Property

        'Contrasena de SAP
        Public Shared Property PassDbl As String
            Get
                Return _PassDBL
            End Get
            Set(ByVal value As String)
                _PassDBL = value
            End Set
        End Property


#End Region

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace

