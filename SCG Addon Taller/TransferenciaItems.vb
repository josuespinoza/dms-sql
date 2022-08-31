Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon
Imports System.Collections.Generic
Imports DMSOneFramework.SCGBL.Requisiciones
Imports SCG.Requisiciones
Imports SCG.Requisiciones.UI


Public Class TransferenciaItems

#Region "Declaraciones"

    'variable para cargar el DocEntry de la cotizacion
    Public intCodigoCotizacion As Integer

#Region "Variables"

    Dim m_objSBO_Application As SAPbouiCOM.Application
    Dim m_objCompany As SAPbobsCOM.Company

    Dim m_dtBodegasXCentroCosto As System.Data.DataTable
    Dim strNombreAsesor As String = String.Empty
    Private m_blnUsaConfiguracionInternaTaller As Boolean = False

#End Region

#Region "Constantes"

#Region "Configuration Properties"

    Public Const mc_strBodegaRepuestos As String = "BodegaRepuestos"
    Public Const mc_strBodegaSuministros As String = "BodegaSuministros"
    Public Const mc_strBodegaServiciosExternos As String = "BodegaServiciosExternos"
    Public Const mc_strBodegaProceso As String = "BodegaProceso"
    Public Const mc_strIDSerieDocumentosTraslado As String = "IDSerieDocumentosTraslado"

#End Region

#Region "Fields"

    Private Const mc_strArroba As String = "@"

    Public Const mc_strAprobado As String = "U_SCGD_Aprobado"
    Public Const mc_strTraslad As String = "U_SCGD_Traslad"
    Public Const mc_strU_NoOrden As String = "U_SCGD_Numero_OT"
    Public Const mc_strU_Placa As String = "U_SCGD_Num_Placa"
    Public Const mc_strU_VIN As String = "U_SCGD_Num_VIN"
    Public Const mc_strU_Marca As String = "U_SCGD_Des_Marc"
    Public Const mc_strU_Estilo As String = "U_SCGD_Des_Esti"
    Public Const mc_strU_Modelo As String = "U_SCGD_Des_Mode"
    Public Const mc_strEmpRealiza As String = "U_SCGD_Emp_Realiza"
    Public Const mc_strTipoTransferenciaUdf As String = "U_SCGD_TipoTransf"
    Public Const mc_strNombEmpleado As String = "U_SCGD_NombEmpleado"
    Public Const mc_strIntCodigoCotizacion As String = "U_SCGD_CodCotizacion"
    Private Const mc_strUdfLineNumOrigen As String = "U_LinenumOrigen"

    Private Const m_strUIDEntregado As String = "chkEnt"
    Private Const m_strUDFEntregado As String = "U_SCGD_Entregado"
#End Region

#Region "Sps"

    Private Const mc_strSCGTA_SP_UPDOrden As String = "SCGTA_SP_UpdOrdenTrabajo"


#End Region

#End Region

#Region "Estruturas"

    Public Structure LineasTransferenciaStock

        Dim strItemCode As String
        Dim strItemDescription As String
        Dim decCantidad As Decimal
        Dim strNoBodegaOrig As String
        Dim strNoBodegaDest As String
        Dim intIDColaborador As Integer
        Dim strNombreMecanico As String
        Dim intTipoArticulo As Integer
        'se agrega para poder agregar el lineNum de cada articulo
        Dim intLineNum As Integer
        Public intCCosto As Integer
        Dim strIDLineaSucursal As String
        Dim intReqOriPen As Integer
        Dim strIDLinea As String
    End Structure

    Public Structure LineasCambiarEstado
        Dim strItemCode As String
        Dim decCantidad As Decimal
        Dim intLineNum As String
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
    Private objDAConexion As DAConexion
    Public Shared intCodCCosto As Integer

    Private m_adpTransItemsSBO As SqlClient.SqlDataAdapter

#End Region

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_objSBOAplication As SAPbouiCOM.Application, ByRef p_objCompany As SAPbobsCOM.Company)

        m_objSBO_Application = p_objSBOAplication
        m_objCompany = p_objCompany

    End Sub

    Public Sub New(ByRef p_objSBOAplication As SAPbouiCOM.Application, _
                    ByRef p_objCompany As SAPbobsCOM.Company, _
                    ByVal p_strCadenaConexion As String)

        m_objSBO_Application = p_objSBOAplication
        m_objCompany = p_objCompany
        strConectionString = p_strCadenaConexion

    End Sub

#End Region

#Region "Procesos"

    Public Function CrearTrasladoAddOnNuevo(ByRef lstRepuestos As Generic.List(Of LineasTransferenciaStock), _
                                            ByRef lstSuministros As Generic.List(Of LineasTransferenciaStock), _
                                            ByRef lstServiociosEX As Generic.List(Of LineasTransferenciaStock), _
                                            ByRef lstItemsEliminarRepuestos As Generic.List(Of LineasTransferenciaStock), _
                                            ByRef lstItemsEliminarSuministros As Generic.List(Of LineasTransferenciaStock), _
                                            ByRef lstItemACambiarEstado As Generic.List(Of LineasCambiarEstado), _
                                            ByRef lstItemACambiarEstadoAdicional As Generic.List(Of LineasCambiarEstado), _
                                            ByVal strNoOrden As String, ByVal strNoBodegaRepu As String, ByVal strNoBodegaSumi As String, _
                                            ByVal strNoBodegaSeEx As String, ByVal strNoBodegaProceso As String, ByVal strIDSerieDocTrasnf As String, _
                                            ByRef p_cnnConeccion As SqlClient.SqlConnection, _
                                            ByRef p_trnTransaccion As SqlClient.SqlTransaction, ByVal p_blnEvaluarAdicionales As Boolean, _
                                            ByRef p_strTrasladosRep As String, ByRef p_strTrasladosSuministros As String, _
                                            ByRef p_strTrasladosSuministrosEliminar As String, _
                                            ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String, _
                                            ByVal p_strPlaca As String, ByVal p_strVIN As String, ByVal p_strAsesor As String, ByVal p_strCliente As String,
                                            Optional ByVal p_blnAjusteOTEspecial As Boolean = False,
                                            Optional ByVal p_blnIniciaTransaccion As Boolean = False,
                                            Optional ByVal p_strIdSucursal As String = "") As String

        Dim blnDraft As Boolean = False
        Dim m_strDraft As String = String.Empty

        Dim strUsaUbicacion As String = String.Empty
        Dim blnUsaUbicacion As Boolean = False

        ''Crea transferencias de stock con base en las cantidades completas de una cotizacion
        If Not String.IsNullOrEmpty(p_strIdSucursal) Then
            Utilitarios.DevuelveCadenaConexionBDTaller(m_objSBO_Application, p_strIdSucursal, strConectionString)
        Else
            Utilitarios.DevuelveCadenaConexionBDTaller(m_objSBO_Application, strConectionString)
        End If

        Dim intIdSucursal As Integer
        Dim strNombreBDTaller As String
        'iniacializo variable para la carga de los valores de la sucursal
        Dim oDataTableConfiguracionesSucursal
        Dim oDataRowConfiguracionSucursal As DataRow
        Dim m_blnConfOT As Boolean = False

        Utilitarios.DevuelveNombreBDTaller(m_objSBO_Application, intIdSucursal, strNombreBDTaller)

        Dim m_strSucursal As String = String.Empty
        m_strSucursal = Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO)

        Dim strNombreAsesor As String
        
        m_blnUsaConfiguracionInternaTaller = Utilitarios.ValidarOTInternaConfiguracion(m_objCompany)
        m_blnConfOT = m_blnUsaConfiguracionInternaTaller
        
        If Not m_blnUsaConfiguracionInternaTaller Then
            Dim adpConf As New ConfiguracionDataAdapter(strConectionString)
            Dim dstConf As New ConfiguracionDataSet
            Dim objUtilitariosCls As New Utilitarios

            adpConf.Fill(dstConf)

            If objUtilitariosCls.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "CreaDraftTransferenciasStock", "") Then
                blnDraft = True
            Else
                blnDraft = False
            End If

        Else
            oDataTableConfiguracionesSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(p_strIdSucursal, m_objCompany)

            If oDataTableConfiguracionesSucursal.Rows.Count <> 0 Then
                oDataRowConfiguracionSucursal = oDataTableConfiguracionesSucursal.Rows(0)
            Else
                oDataRowConfiguracionSucursal = Nothing

            End If

            If Not oDataRowConfiguracionSucursal Is Nothing Then

                If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Requis")) Then

                    If oDataRowConfiguracionSucursal.Item("U_Requis") = "Y" Then
                        blnDraft = True
                    Else
                        blnDraft = False
                    End If
                Else
                    blnDraft = False
                End If
            Else
                blnDraft = False
            End If
        End If

        Dim strCollecDocEntrys As String = ""
        Dim strDocEntry As String = ""
        strNombreAsesor = Utilitarios.EjecutarConsulta("Select Isnull(firstName,'') + ' ' + Isnull(lastName,'') from OHEM where empID = " & p_strAsesor, m_objCompany.CompanyDB, m_objCompany.Server)

        If m_objCompany.Version > 900000 Then

            Dim a As String = m_objCompany.Version


            strUsaUbicacion = Utilitarios.EjecutarConsulta("Select U_UsaUbicD From dbo.[@SCGD_ADMIN] with (nolock) ", m_objCompany.CompanyDB, m_objCompany.Server)

            If Not String.IsNullOrEmpty(strUsaUbicacion.Trim) Then

                If strUsaUbicacion = "Y" Then
                    blnUsaUbicacion = True
                End If
            End If
        End If

        If lstRepuestos.Count <> 0 Then
            'p_strTrasladosRep = CrearSBOTransferenciaItems(lstRepuestos, strNoOrden, strNoBodegaRepu, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, False, p_strCliente, blnDraft, p_blnAjusteOTEspecial, My.Resources.Resource.Repuesto, p_strIdSucursal)
            ''SE COMENTA PARA DEJAR EL PROCESO DE UBICACIONES 
            p_strTrasladosRep = CrearSBOTransferenciaItems(lstRepuestos, strNoOrden, strNoBodegaRepu, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, False, p_strCliente, blnDraft, p_blnAjusteOTEspecial, My.Resources.Resource.Repuesto, p_strIdSucursal, blnUsaUbicacion)
        End If

        If lstServiociosEX.Count <> 0 Then
            Call CrearSBOTransferenciaItems(lstServiociosEX, strNoOrden, strNoBodegaProceso, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, False, p_strCliente, blnDraft, p_blnAjusteOTEspecial, String.Empty, p_strIdSucursal)
        End If

        strCollecDocEntrys &= strDocEntry

        strDocEntry = ""

        If lstSuministros.Count <> 0 Then

            p_strTrasladosSuministros = CrearSBOTransferenciaItems(lstSuministros, strNoOrden, strNoBodegaSumi, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, False, p_strCliente, blnDraft, p_blnAjusteOTEspecial, My.Resources.Resource.Suministro, p_strIdSucursal)

        End If

        If strDocEntry <> "" Then
            If strCollecDocEntrys <> "" Then
                strCollecDocEntrys &= ","
            End If
        End If

        strCollecDocEntrys &= strDocEntry

        strDocEntry = ""

        If lstItemsEliminarRepuestos.Count <> 0 Then
            strDocEntry = CrearSBOTransferenciaItems(lstItemsEliminarRepuestos, strNoOrden, strNoBodegaProceso, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, True, p_strCliente, blnDraft, p_blnAjusteOTEspecial, My.Resources.Resource.Repuesto, p_strIdSucursal)
        End If

        If lstItemsEliminarSuministros.Count <> 0 Then
            p_strTrasladosSuministrosEliminar = CrearSBOTransferenciaItems(lstItemsEliminarSuministros, strNoOrden, strNoBodegaProceso, strIDSerieDocTrasnf, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, strNombreAsesor, True, p_strCliente, blnDraft, p_blnAjusteOTEspecial, My.Resources.Resource.Suministro, p_strIdSucursal)
        End If

        If strCollecDocEntrys <> "" AndAlso strDocEntry <> "" Then
            strCollecDocEntrys &= ","
        End If

        strCollecDocEntrys &= strDocEntry

        If Not m_blnConfOT Then
            If lstItemACambiarEstado.Count > 0 Then
                ActualizarEstadoItems(lstItemACambiarEstado, strNoOrden, p_cnnConeccion, p_trnTransaccion, True, p_blnIniciaTransaccion)
            End If

            If lstItemACambiarEstadoAdicional.Count > 0 AndAlso p_blnEvaluarAdicionales Then
                ActualizarEstadoItems(lstItemACambiarEstadoAdicional, strNoOrden, p_cnnConeccion, p_trnTransaccion, False, p_blnIniciaTransaccion)
            End If
        End If

        Return strCollecDocEntrys

    End Function

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
    Private Function CrearDocumentoTransferenciaRequisicion(ByVal m_intNumCotiz As Integer, ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), ByVal p_strNoOrden As String, _
                                                      ByVal p_strIDBodegaOrig As String, ByVal p_strNoSerie As String, _
                                                      ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String, _
                                                      ByVal p_strPlaca As String, ByVal p_strVIN As String, ByVal p_strAsesor As String, ByVal p_blnEliminar As Boolean, _
                                                      ByVal p_strCodCliente As String, ByVal p_strNombCliente As String, ByVal p_blnAjusteOTEspecial As Boolean, _
                                                      ByVal tipo As String, Optional p_strIdSucursal As String = "", Optional p_blnUsaUbicacion As Boolean = False) As List(Of RequisicionTraslado)

        Dim listaPorBodegas As List(Of List(Of LineasTransferenciaStock))
        Dim grupoPorBodega As List(Of LineasTransferenciaStock)
        CrearDocumentoTransferenciaRequisicion = New List(Of RequisicionTraslado)(10)

        listaPorBodegas = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

        ''SE COMENTA PARA EL PROCESO DE UBICACIONES

        'If p_blnUsaUbicacion Then
        '    If Not String.IsNullOrEmpty(p_strIdSucursal) Then
        '        m_dtBodegasXCentroCosto = Utilitarios.LlenarTablaconUbicacionDefectoenBodegoProcesoXCentroCosto(p_strIdSucursal, m_objCompany)
        '    End If
        'End If


        For Each grupoPorBodega In listaPorBodegas
            Dim encabezado As EncabezadoRequisicion = New EncabezadoRequisicion()
            Dim data As EncabezadoTrasladoDMSData = New EncabezadoTrasladoDMSData()
            Dim listaLineas As List(Of InformacionLineaRequisicion)
            Dim req As RequisicionTraslado = New RequisicionTraslado(m_objCompany)

            If p_blnEliminar Then
                req.TipoRequisicion = My.Resources.Resource.Devolucion
                encabezado.CodigoTipoRequisicion = 2
            Else
                req.TipoRequisicion = My.Resources.Resource.RequisicionTraslado
                encabezado.CodigoTipoRequisicion = 1
            End If
            'req.TipoRequisicion = My.Resources.Resource.RequisicionTraslado
            req.DocumentoGenera = My.Resources.Resource.DocGeneraReq

            encabezado.CodigoCliente = p_strCodCliente
            encabezado.NoOrden = p_strNoOrden
            encabezado.NombreCliente = p_strNombCliente
            data.TipoTransferencia = 1
            data.Serie = p_strNoSerie
            data.NumCotizacionOrigen = intCodigoCotizacion
            encabezado.Comentarios = My.Resources.Resource.OT_Referencia & p_strNoOrden & " " & My.Resources.Resource.Asesor & p_strAsesor
            encabezado.Usuario = m_objSBO_Application.Company.UserName
            encabezado.IDSucursal = p_strIdSucursal
            encabezado.TipoArticulo = grupoPorBodega.Item(0).intTipoArticulo

            If Not String.IsNullOrEmpty(p_strPlaca) Then
                encabezado.Placa = p_strPlaca
            End If
            If Not String.IsNullOrEmpty(p_strMarca) Then
                encabezado.Marca = p_strMarca
            End If
            If Not String.IsNullOrEmpty(p_strEstilo) Then
                encabezado.Estilo = p_strEstilo
            End If
            If Not String.IsNullOrEmpty(p_strVIN) Then
                encabezado.VIN = p_strVIN
            End If

            If p_blnEliminar Then
                encabezado.TipoRequisicion = My.Resources.Resource.Devolucion
                data.TipoTransferencia = 2
                encabezado.Comentarios &= " * * " & My.Resources.Resource.Devolucion & " * * "
            End If
            If p_blnAjusteOTEspecial Then
                encabezado.Comentarios = My.Resources.Resource.MensajeAjusteOTEspecial
            End If

            listaLineas = New List(Of InformacionLineaRequisicion)(grupoPorBodega.Count)
            For Each linea As LineasTransferenciaStock In grupoPorBodega
                Dim informacionLineaRequisicion As InformacionLineaRequisicion = New InformacionLineaRequisicion()
                informacionLineaRequisicion.CodigoArticulo = linea.strItemCode
                informacionLineaRequisicion.DescripcionArticulo = linea.strItemDescription
                informacionLineaRequisicion.CodigoBodegaOrigen = linea.strNoBodegaOrig
                informacionLineaRequisicion.CodigoBodegaDestino = linea.strNoBodegaDest
                informacionLineaRequisicion.CantidadSolicitada = linea.decCantidad
                informacionLineaRequisicion.CantidadOriginal = linea.decCantidad
                informacionLineaRequisicion.LineNumOrigen = linea.intLineNum
                informacionLineaRequisicion.DocumentoOrigen = m_intNumCotiz
                informacionLineaRequisicion.DescripcionTipoArticulo = tipo
                informacionLineaRequisicion.CentroCosto = linea.intCCosto
                informacionLineaRequisicion.CodigoTipoArticulo = linea.intTipoArticulo
                informacionLineaRequisicion.LineaIDSucursal = linea.strIDLineaSucursal
                informacionLineaRequisicion.IDLinea = linea.strIDLinea
                informacionLineaRequisicion.LineaReqOrPen = linea.intReqOriPen

                ''SE COMENTA PARA EL PROCESO DE UBICACIONES
                If p_blnUsaUbicacion Then
                    Utilitarios.DevolverUbicacionArticuloPorDefecto(False, linea.strItemCode, linea.strNoBodegaOrig, linea.strNoBodegaDest, m_objCompany, informacionLineaRequisicion, Nothing, linea.decCantidad)
                End If
                listaLineas.Add(informacionLineaRequisicion)
            Next

            'encabezado.Data = data.Serialize()
            req.EncabezadoRequisicion = encabezado
            req.LineasRequisicion = listaLineas

            Dim crea As Integer = req.Crea()
            If crea <> 0 Then CrearDocumentoTransferenciaRequisicion.Add(req)

        Next

    End Function

    'Public Sub DevolverUbicacionArticuloPorDefecto(strItemCode As String, strAlmacenOrigen As String, strAlmacenDestino As String, _
    '                                               ByRef p_informacionLineaRequisicion As InformacionLineaRequisicion)

    '    Dim oItemArticulo As SAPbobsCOM.IItems
    '    Dim intGrupoArticulo As String = String.Empty
    '    Dim intCentroCosto As String = 0

    '    oItemArticulo = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
    '    oItemArticulo.GetByKey(strItemCode)

    '    intGrupoArticulo = oItemArticulo.ItemsGroupCode
    '    intCentroCosto = oItemArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value

    '    Dim dtUbicacionesDefecto As System.Data.DataTable
    '    dtUbicacionesDefecto = New System.Data.DataTable

    '    Dim intUbicacionDefectoProceso As Integer

    '    intUbicacionDefectoProceso = Utilitarios.DevolverUbicacionDefectoProceso(m_dtBodegasXCentroCosto, intCentroCosto)

    '    Dim query As String = "SELECT   OWHS.BinActivat, OWHS.WhsCode, OWHS.WhsName, OWHS.DftBinAbs UbicacionDefectoAlmacen, OITW.DftBinAbs UbicacionDefectoItem, " & _
    '                          " OIGW.DftBinAbs UbicacionDefectoGrupoArticulo " & _
    '                          " FROM OWHS INNER JOIN " & _
    '                          " OITW ON OWHS.WhsCode = OITW.WhsCode INNER JOIN " & _
    '                          " OIGW ON OWHS.WhsCode = OIGW.WhsCode " & _
    '                          " where OITW.ItemCode = '" & strItemCode & "'and OWHS.WhsCode in ('" & strAlmacenOrigen & "','" & strAlmacenDestino & "') " & _
    '                          " and OIGW.ItmsGrpCod  = '" & intGrupoArticulo & "'"

    '    dtUbicacionesDefecto = Utilitarios.EjecutarConsultaDataTable(query, m_objCompany.CompanyDB, m_objCompany.Server)

    '    For Each drw As System.Data.DataRow In dtUbicacionesDefecto.Rows

    '        If drw.Item("WhsCode") = strAlmacenOrigen Then
    '            'valido si el alamacen usa ubicaciones
    '            If drw.Item("BinActivat") = "Y" Then

    '                If Not IsDBNull(drw.Item("UbicacionDefectoItem")) Then

    '                    p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoItem")

    '                ElseIf Not IsDBNull(drw.Item("UbicacionDefectoGrupoArticulo")) Then

    '                    p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoGrupoArticulo")

    '                Else

    '                    p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoAlmacen")

    '                End If

    '            End If


    '        ElseIf drw.Item("WhsCode") = strAlmacenDestino Then

    '            'verifico quel bodega de proceso tenga una ubicacion por defecto
    '            If Not intUbicacionDefectoProceso = 0 Then

    '                p_informacionLineaRequisicion.AUbicacion = intUbicacionDefectoProceso

    '            Else

    '                If drw.Item("BinActivat") = "Y" Then

    '                    If Not IsDBNull(drw.Item("UbicacionDefectoItem")) Then

    '                    ElseIf Not IsDBNull(drw.Item("UbicacionDefectoGrupoArticulo")) Then

    '                        p_informacionLineaRequisicion.AUbicacion = drw.Item("UbicacionDefectoGrupoArticulo")

    '                    Else

    '                        p_informacionLineaRequisicion.AUbicacion = drw.Item("UbicacionDefectoAlmacen")
    '                    End If
    '                End If

    '            End If
    '        End If

    '    Next

    '    oItemArticulo = Nothing

    'End Sub

    Private Function CrearSBOTransferenciaItems(ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), _
                   ByVal p_strNoOrden As String, ByVal p_strIDBodegaOrig As String, ByVal p_strNoSerie As String, _
                   ByVal p_strMarca As String, ByVal p_strEstilo As String, ByVal p_strModelo As String, _
                   ByVal p_strPlaca As String, ByVal p_strVIN As String, ByVal p_strAsesor As String, ByVal p_blnEliminar As Boolean, _
                   ByVal p_strCliente As String, ByVal p_blnDraft As Boolean, ByVal p_blnAjusteOTEspecial As Boolean, ByVal tipo As String, Optional p_IdSucursal As String = "", _
                   Optional p_blnUsaUbicacion As Boolean = False) As String

        Dim oTransfStockDoc As SAPbobsCOM.StockTransfer

        Dim objBLSBO As New BLSBO.GlobalFunctionsSBO
        Dim intSBOResult As Integer
        Dim strErrMsg As String = ""
        Dim intNewDocEntry As Integer
        Dim strDocEntryResult As String = ""
        Dim glstItemsXBodegaOrigen As Generic.List(Of Generic.List(Of LineasTransferenciaStock))
        Dim lstActual As Generic.List(Of LineasTransferenciaStock)

        Dim strRequisiciones As String = ""
        Dim blnReq As Boolean = False

        Try
            'valida si se crea el documento preliminar con base a la propiedad 
            'en la tabla de configuracion

            If p_blnDraft Then

                Dim strDocEntryResultadoDraft As String = String.Empty
                glstItemsXBodegaOrigen = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

                For Each lstActual In glstItemsXBodegaOrigen

                    ''SE COMENTA PARA DEJAR EL PROCESO DE UBICACION
                    strDocEntryResultadoDraft = CrearDocumentoTransferenciaRequisicion(intCodigoCotizacion, lstActual, p_strNoOrden, lstActual(0).strNoBodegaOrig, p_strNoSerie, p_strMarca, p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, p_blnEliminar, p_strCliente, String.Empty, p_blnAjusteOTEspecial, tipo, p_IdSucursal, p_blnUsaUbicacion).Item(0).EncabezadoRequisicion.DocEntry

                    If glstItemsXBodegaOrigen.Count > 1 Then
                        If blnReq = False Then

                            strRequisiciones = strDocEntryResultadoDraft

                        ElseIf blnReq = True Then

                            strRequisiciones = strRequisiciones & "," & strDocEntryResultadoDraft

                        End If

                        blnReq = True

                    End If

                Next

                If glstItemsXBodegaOrigen.Count > 1 Then

                    strDocEntryResultadoDraft = strRequisiciones

                End If

                Return strDocEntryResultadoDraft

            Else

                glstItemsXBodegaOrigen = ClasificaListaXBodegaOrigen(p_lstLineasTransStock)

                For Each lstActual In glstItemsXBodegaOrigen

                    oTransfStockDoc = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

                    With oTransfStockDoc

                        .CardCode = p_strCliente
                        .FromWarehouse = lstActual(0).strNoBodegaOrig
                        .ToWarehouse = lstActual(0).strNoBodegaDest
                        .Series = p_strNoSerie
                        .UserFields.Fields.Item(mc_strU_NoOrden).Value = p_strNoOrden
                        .UserFields.Fields.Item(mc_strU_Marca).Value = p_strMarca
                        .UserFields.Fields.Item(mc_strU_Estilo).Value = p_strEstilo
                        .UserFields.Fields.Item(mc_strU_Modelo).Value = p_strModelo
                        .UserFields.Fields.Item(mc_strU_Placa).Value = p_strPlaca
                        .UserFields.Fields.Item(mc_strU_VIN).Value = p_strVIN
                        .UserFields.Fields.Item(mc_strTipoTransferenciaUdf).Value = 1

                        If p_blnAjusteOTEspecial Then
                            .Comments &= My.Resources.Resource.MensajeAjusteOTEspecial
                        End If
                        .Comments &= My.Resources.Resource.OT_Referencia & p_strNoOrden & " " & My.Resources.Resource.Asesor & p_strAsesor
                        '.Reference1 = p_strAsesor
                        If p_blnEliminar Then
                            .UserFields.Fields.Item(mc_strTipoTransferenciaUdf).Value = 2
                            .Comments &= " * * " & My.Resources.Resource.Devolucion & " * * "
                        End If
                    End With

                    CargarLineasTraslado(oTransfStockDoc, lstActual, p_IdSucursal, lstActual(0).strNoBodegaOrig)

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


                If strDocEntryResult <> "" Then
                    strDocEntryResult = strDocEntryResult.Substring(0, strDocEntryResult.Length - 1)
                End If

                Return strDocEntryResult
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return String.Empty
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
                blnExiste = False
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

    Private Sub CargarLineasTraslado(ByRef p_oTrasfStockDoc As SAPbobsCOM.StockTransfer, _
                                    ByRef p_lstLineasTransStock As Generic.List(Of LineasTransferenciaStock), Optional p_strIdSucursal As String = "", _
                                    Optional p_bodegaOrigen As String = "")


        Dim udtLineasTSCurrent As LineasTransferenciaStock
        Dim intCont As Integer

        If p_lstLineasTransStock.Count <> 0 Then

            udtLineasTSCurrent = p_lstLineasTransStock(0)

            With p_oTrasfStockDoc

                .Lines.ItemCode = udtLineasTSCurrent.strItemCode
                If Not String.IsNullOrEmpty(udtLineasTSCurrent.strItemDescription) Then
                    .Lines.ItemDescription = udtLineasTSCurrent.strItemDescription
                End If
                .Lines.Quantity = udtLineasTSCurrent.decCantidad
                .Lines.WarehouseCode = udtLineasTSCurrent.strNoBodegaDest
                If Not String.IsNullOrEmpty(udtLineasTSCurrent.strNombreMecanico) Then
                    .Lines.UserFields.Fields.Item(mc_strNombEmpleado).Value = udtLineasTSCurrent.strNombreMecanico
                End If
                If udtLineasTSCurrent.intIDColaborador <> 0 Then
                    .Lines.UserFields.Fields.Item(mc_strEmpRealiza).Value = udtLineasTSCurrent.intIDColaborador
                End If

                If m_objCompany.Version > 900000 Then
                    ''SE COMENTA PARA EL PROCESO DE UBICACIONES
                    Utilitarios.DevolverUbicacionArticuloPorDefecto(True, udtLineasTSCurrent.strItemCode, p_bodegaOrigen, udtLineasTSCurrent.strNoBodegaDest, m_objCompany, Nothing, p_oTrasfStockDoc, udtLineasTSCurrent.decCantidad)
                End If


            End With

            For intCont = 1 To p_lstLineasTransStock.Count - 1

                udtLineasTSCurrent = p_lstLineasTransStock(intCont)

                With p_oTrasfStockDoc

                    .Lines.Add()

                    .Lines.ItemCode = udtLineasTSCurrent.strItemCode
                    If Not String.IsNullOrEmpty(udtLineasTSCurrent.strItemDescription) Then
                        .Lines.ItemDescription = udtLineasTSCurrent.strItemDescription
                    End If
                    .Lines.Quantity = udtLineasTSCurrent.decCantidad
                    .Lines.WarehouseCode = udtLineasTSCurrent.strNoBodegaDest
                    If Not String.IsNullOrEmpty(udtLineasTSCurrent.strNombreMecanico) Then
                        .Lines.UserFields.Fields.Item(mc_strNombEmpleado).Value = udtLineasTSCurrent.strNombreMecanico
                    End If
                    If udtLineasTSCurrent.intIDColaborador <> 0 Then
                        .Lines.UserFields.Fields.Item(mc_strEmpRealiza).Value = udtLineasTSCurrent.intIDColaborador
                    End If

                End With

            Next

        End If

    End Sub

    Public Sub GeneraLista(ByVal p_scgTiposMovimientosXBodega As scgTiposMovimientoXBodega, _
                ByRef p_lstItems As Generic.List(Of LineasTransferenciaStock), _
                ByRef p_oDocLines As SAPbobsCOM.Document_Lines, _
                ByVal p_strNoBodegaRepu As String, _
                ByVal p_strNoBodegaSumi As String, _
                ByVal p_strNoBodegaSeEx As String, _
                ByVal p_strNoBodegaProceso As String, _
                ByRef p_lstItemsCambiarEstado As Generic.List(Of LineasCambiarEstado), _
                ByRef p_lstItemsCambiarEstadoAdicional As Generic.List(Of LineasCambiarEstado), _
                ByVal p_blnEvaluarAdicionales As Boolean, ByVal p_intTipoArticulo As Integer, _
                ByVal p_intEstadoPaquete As Integer, ByVal p_intCantidadLineasPaquete As Integer, _
                ByVal p_intItemGenerico As Integer, _
                ByVal p_blnActualizarCantidad As Boolean, _
                ByVal p_blnDraft As Boolean, _
                ByVal p_oForm As SAPbouiCOM.Form, _
                Optional ByVal p_decCantidadAdicional As Decimal = 0, _
                Optional ByVal p_intDocEntry As Integer = 0, _
                Optional ByVal p_intTrasladadoOriginal As Integer = -1, Optional ByVal p_confOTSap As Boolean = False)

        'Genera lista de items por cantidades completas de la cotización

        Dim intTipoItemAceptado As Integer
        Dim udtLineaTransf As LineasTransferenciaStock = Nothing
        Dim udfLineaCambiar As LineasCambiarEstado
        Dim dblStockDisp As Double
        Dim decCantXOtrasLineas As Decimal
        Dim dblCantidadValida As Double
        Dim strNoBodegaActual As String = ""

        Dim objutilitarios As New SCGDataAccess.Utilitarios(strConectionString)

        Select Case p_scgTiposMovimientosXBodega
            Case scgTiposMovimientoXBodega.TransfRepuestos
                intTipoItemAceptado = 1
                strNoBodegaActual = p_strNoBodegaRepu
            Case scgTiposMovimientoXBodega.TransfSuministros
                intTipoItemAceptado = 3
                strNoBodegaActual = p_strNoBodegaSumi
            Case scgTiposMovimientoXBodega.TransfServiciosEx
                intTipoItemAceptado = 4
                strNoBodegaActual = p_strNoBodegaSeEx
        End Select
        If strNoBodegaActual <> "" Then
            If strNoBodegaActual <> p_strNoBodegaProceso Then

                With p_oDocLines

                    If p_intTipoArticulo <> 5 Then
                        If p_scgTiposMovimientosXBodega <> scgTiposMovimientoXBodega.TransfItemsEliminar Then
                            If (((.UserFields.Fields.Item(mc_strAprobado).Value = 1 AndAlso p_intCantidadLineasPaquete <= 0) _
                               Or (p_intEstadoPaquete = 1 AndAlso p_intCantidadLineasPaquete > 0)) _
                                AndAlso (.UserFields.Fields.Item(mc_strTraslad).Value = 0 Or .UserFields.Fields.Item(mc_strTraslad).Value = 3 Or .UserFields.Fields.Item(mc_strTraslad).Value = 4)) Or p_blnActualizarCantidad Then

                                If p_intTipoArticulo = intTipoItemAceptado Then

                                    If p_intItemGenerico = 1 Then

                                        dblStockDisp = DevuelveStockDisponibleItem(.ItemCode, strNoBodegaActual)
                                        decCantXOtrasLineas = DevuelveCantXLineasAnteriores(.ItemCode, .LineNum, p_intDocEntry, p_oForm)

                                        If (dblStockDisp - decCantXOtrasLineas) > 0 Then
                                            If Not p_blnActualizarCantidad Then
                                                If (dblStockDisp - decCantXOtrasLineas) < .Quantity Then
                                                    dblCantidadValida = dblStockDisp - decCantXOtrasLineas
                                                Else
                                                    dblCantidadValida = .Quantity
                                                End If
                                            Else
                                                If (dblStockDisp - decCantXOtrasLineas) < p_decCantidadAdicional Then
                                                    dblCantidadValida = dblStockDisp - decCantXOtrasLineas
                                                Else
                                                    dblCantidadValida = p_decCantidadAdicional
                                                End If
                                            End If
                                            If dblCantidadValida >= .Quantity Then
                                                udtLineaTransf.strItemCode = .ItemCode
                                                udtLineaTransf.intTipoArticulo = p_intTipoArticulo
                                                udtLineaTransf.strItemDescription = .ItemDescription
                                                udtLineaTransf.decCantidad = dblCantidadValida
                                                udtLineaTransf.strNoBodegaDest = p_strNoBodegaProceso
                                                udtLineaTransf.strNoBodegaOrig = strNoBodegaActual
                                                udtLineaTransf.intIDColaborador = IIf(IsNumeric(.UserFields.Fields.Item(mc_strEmpRealiza).Value), .UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                                udtLineaTransf.strNombreMecanico = .UserFields.Fields.Item(mc_strNombEmpleado).Value
                                                udtLineaTransf.intLineNum = .LineNum
                                                udtLineaTransf.strIDLineaSucursal = .UserFields.Fields.Item("U_SCGD_Sucur").Value
                                                udtLineaTransf.strIDLinea = .UserFields.Fields.Item("U_SCGD_ID").Value
                                                If p_intTrasladadoOriginal = 4 Then
                                                    udtLineaTransf.intReqOriPen = 2
                                                Else
                                                    udtLineaTransf.intReqOriPen = 1
                                                End If
                                                p_lstItems.Add(udtLineaTransf)
                                            End If
                                            If .UserFields.Fields.Item(mc_strTraslad).Value = 3 AndAlso _
                                                (p_intTipoArticulo = 1 Or p_intTipoArticulo = 4) AndAlso dblCantidadValida >= .Quantity Then

                                                udfLineaCambiar = New LineasCambiarEstado
                                                udfLineaCambiar.decCantidad = dblCantidadValida
                                                udfLineaCambiar.intLineNum = .LineNum
                                                udfLineaCambiar.strItemCode = .ItemCode
                                                udtLineaTransf.intIDColaborador = IIf(IsNumeric(.UserFields.Fields.Item(mc_strEmpRealiza).Value), .UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                                udtLineaTransf.strNombreMecanico = .UserFields.Fields.Item(mc_strNombEmpleado).Value
                                                udtLineaTransf.strIDLineaSucursal = .UserFields.Fields.Item("U_SCGD_Sucur").Value
                                                udtLineaTransf.strIDLinea = .UserFields.Fields.Item("U_SCGD_ID").Value
                                                p_lstItemsCambiarEstado.Add(udfLineaCambiar)

                                            End If

                                            If p_blnEvaluarAdicionales Then

                                                If .UserFields.Fields.Item(mc_strTraslad).Value = 0 AndAlso _
                                                    (p_intTipoArticulo = 1 Or p_intTipoArticulo = 4) Then

                                                    udfLineaCambiar = New LineasCambiarEstado
                                                    udfLineaCambiar.decCantidad = dblCantidadValida
                                                    udfLineaCambiar.intLineNum = .LineNum
                                                    udfLineaCambiar.strItemCode = .ItemCode
                                                    udtLineaTransf.intIDColaborador = IIf(IsNumeric(.UserFields.Fields.Item(mc_strEmpRealiza).Value), .UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                                    udtLineaTransf.strNombreMecanico = .UserFields.Fields.Item(mc_strNombEmpleado).Value
                                                    udtLineaTransf.strIDLineaSucursal = .UserFields.Fields.Item("U_SCGD_Sucur").Value
                                                    udtLineaTransf.strIDLinea = .UserFields.Fields.Item("U_SCGD_ID").Value

                                                    p_lstItemsCambiarEstadoAdicional.Add(udfLineaCambiar)

                                                End If

                                            End If
                                            If dblCantidadValida >= .Quantity Then
                                                ''''''''''''''''se agrega para Documentos Draft''''''''''''''''
                                                If p_blnDraft Then
                                                    .UserFields.Fields.Item(mc_strTraslad).Value = 4
                                                Else
                                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                                    .UserFields.Fields.Item(mc_strTraslad).Value = 2
                                                    .UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                                    .UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                                    .UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                                    .UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                                    .UserFields.Fields.Item("U_SCGD_CRec").Value = .Quantity
                                                End If
                                            End If
                                        Else
                                            If .UserFields.Fields.Item(mc_strTraslad).Value <> 3 Then
                                                .UserFields.Fields.Item(mc_strTraslad).Value = 1
                                            End If
                                        End If
                                    Else
                                        .UserFields.Fields.Item(mc_strTraslad).Value = 1
                                    End If
                                End If
                            End If
                        Else
                            If ((.UserFields.Fields.Item(mc_strAprobado).Value = 2 AndAlso p_intCantidadLineasPaquete <= 0) _
                                Or (p_intEstadoPaquete = 2 AndAlso p_intCantidadLineasPaquete > 0)) AndAlso .UserFields.Fields.Item(mc_strTraslad).Value = 2 Then

                                If CStr(p_intTipoArticulo) Like "[1,3]" Then

                                    udtLineaTransf.strItemCode = .ItemCode
                                    udtLineaTransf.strItemDescription = .ItemDescription
                                    If p_confOTSap Then
                                        ''Se cambia la cantidad de la requisicion por devolucion de la cantidad original a la cantidad recibida
                                        'udtLineaTransf.dblCantidad = .Quantity
                                        udtLineaTransf.decCantidad = .UserFields.Fields.Item("U_SCGD_CRec").Value
                                    Else
                                        udtLineaTransf.decCantidad = .Quantity
                                    End If

                                    Select Case p_intTipoArticulo
                                        Case 1
                                            udtLineaTransf.strNoBodegaDest = p_strNoBodegaRepu
                                        Case 2
                                            udtLineaTransf.strNoBodegaDest = p_strNoBodegaSumi
                                        Case 3
                                            udtLineaTransf.strNoBodegaDest = p_strNoBodegaSeEx
                                    End Select

                                    udtLineaTransf.strNoBodegaOrig = p_strNoBodegaProceso
                                    udtLineaTransf.intIDColaborador = IIf(IsNumeric(.UserFields.Fields.Item(mc_strEmpRealiza).Value), .UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                    udtLineaTransf.strNombreMecanico = .UserFields.Fields.Item(mc_strNombEmpleado).Value
                                    'udtLineaTransf.intTipoArticulo = p_intTipoArticulo
                                    udtLineaTransf.strIDLineaSucursal = .UserFields.Fields.Item("U_SCGD_Sucur").Value
                                    udtLineaTransf.strIDLinea = .UserFields.Fields.Item("U_SCGD_ID").Value

                                    If p_intTrasladadoOriginal = 4 Then
                                        udtLineaTransf.intReqOriPen = 2
                                    Else
                                        udtLineaTransf.intReqOriPen = 1
                                    End If
                                    p_lstItems.Add(udtLineaTransf)
                                    .UserFields.Fields.Item(mc_strTraslad).Value = 0
                                End If
                            End If
                        End If
                        If p_intCantidadLineasPaquete > 0 Then
                            p_intCantidadLineasPaquete -= 1
                        End If
                    Else
                        p_intCantidadLineasPaquete = objutilitarios.CantidadLineasPaquetes(.ItemCode)
                        p_intEstadoPaquete = .UserFields.Fields.Item(mc_strAprobado).Value
                    End If
                End With
            End If
        Else
            If p_scgTiposMovimientosXBodega = scgTiposMovimientoXBodega.TransfItemsEliminar Then
                With p_oDocLines


                    If (((.UserFields.Fields.Item(mc_strAprobado).Value = 2 AndAlso p_intCantidadLineasPaquete <= 0) _
                        Or (p_intEstadoPaquete = 2 AndAlso p_intCantidadLineasPaquete > 0)) AndAlso .UserFields.Fields.Item(mc_strTraslad).Value = 1 Or .UserFields.Fields.Item(mc_strTraslad).Value = 2 Or .UserFields.Fields.Item(mc_strTraslad).Value = 4) Or p_blnActualizarCantidad Then

                        'If (((.UserFields.Fields.Item(mc_strAprobado).Value = 2 AndAlso p_intCantidadLineasPaquete <= 0) _
                        '                                Or (p_intEstadoPaquete = 2 AndAlso p_intCantidadLineasPaquete > 0)) AndAlso .UserFields.Fields.Item(mc_strTraslad).Value = 2) Or p_blnActualizarCantidad Then


                        If CStr(p_intTipoArticulo) Like "[1,3]" Then

                            udtLineaTransf.strItemCode = .ItemCode
                            udtLineaTransf.strItemDescription = .ItemDescription

                            If p_confOTSap Then
                                If Not p_blnActualizarCantidad Then
                                    ''Se cambia la cantidad de la requisicion por devolucion de la cantidad original a la cantidad recibida
                                    udtLineaTransf.decCantidad = .UserFields.Fields.Item("U_SCGD_CRec").Value
                                Else
                                    udtLineaTransf.decCantidad = p_decCantidadAdicional
                                End If
                            Else
                                If Not p_blnActualizarCantidad Then
                                    udtLineaTransf.decCantidad = .Quantity
                                Else
                                    udtLineaTransf.decCantidad = p_decCantidadAdicional
                                End If
                            End If


                            udtLineaTransf.intLineNum = .LineNum
                            Select Case p_intTipoArticulo
                                Case 1
                                    udtLineaTransf.strNoBodegaDest = p_strNoBodegaRepu
                                Case 3
                                    udtLineaTransf.strNoBodegaDest = p_strNoBodegaSumi
                                Case 4
                                    udtLineaTransf.strNoBodegaDest = p_strNoBodegaSeEx
                            End Select

                            udtLineaTransf.strNoBodegaOrig = p_strNoBodegaProceso
                            udtLineaTransf.intTipoArticulo = p_intTipoArticulo
                            udtLineaTransf.intIDColaborador = IIf(IsNumeric(.UserFields.Fields.Item(mc_strEmpRealiza).Value), .UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                            udtLineaTransf.strNombreMecanico = .UserFields.Fields.Item(mc_strNombEmpleado).Value
                            udtLineaTransf.strIDLineaSucursal = .UserFields.Fields.Item("U_SCGD_Sucur").Value
                            udtLineaTransf.strIDLinea = .UserFields.Fields.Item("U_SCGD_ID").Value
                            If p_intTrasladadoOriginal = 4 Then
                                udtLineaTransf.intReqOriPen = 2
                            Else
                                udtLineaTransf.intReqOriPen = 1
                            End If

                            p_lstItems.Add(udtLineaTransf)
                            If Not p_blnActualizarCantidad Then
                                .UserFields.Fields.Item(mc_strTraslad).Value = 0
                                '.UserFields.Fields.Item("U_SCGD_CPDe").Value = .Quantity
                            End If

                            ''''''''''''''''se agrega para Documentos Draft''''''''''''''''
                            If Not p_blnDraft AndAlso p_decCantidadAdicional > 0 AndAlso p_decCantidadAdicional < .Quantity Then
                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                .UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CRec").Value = .Quantity
                            ElseIf Not p_blnDraft And p_decCantidadAdicional = 0 Then
                                .UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                .UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                            End If


                        End If
                    End If
                End With
            End If
        End If

    End Sub

    Public Sub GeneraListaCambioBodegaProceso(ByVal p_scgTiposMovimientosXBodega As scgTiposMovimientoXBodega, _
                ByRef p_lstItems As Generic.List(Of LineasTransferenciaStock), _
                ByVal p_strNoBodegaRepu As String, _
                ByVal p_strNoBodegaSumi As String, _
                ByVal p_strNoBodegaSeEx As String, _
                ByVal p_strNoBodegaProceso As String, _
                 ByVal p_intTipoArticulo As Integer, _
                ByVal p_intEstadoPaquete As Integer, _
                ByVal p_intCantidadLineasPaquete As Integer, _
                ByVal p_intItemGenerico As Integer, _
                ByVal p_intDocEntry As Integer, _
                ByVal p_strItemCode As String, _
                ByVal p_decCantidad As Decimal)

        'Genera lista de items por cantidades completas de la cotización

        '        Dim intCont As Integer
        Dim intTipoItemAceptado As Integer
        Dim udtLineaTransf As LineasTransferenciaStock = Nothing
        '        Dim udfLineaCambiar As LineasCambiarEstado
        '        Dim dblStockDisp As Double
        '        Dim decCantXOtrasLineas As Decimal
        '        Dim dblCantidadValida As Double
        Dim strNoBodegaActual As String = ""

        'Dim objutilitarios As New SCGDataAccess.Utilitarios(strConectionString)

        Select Case p_scgTiposMovimientosXBodega
            Case scgTiposMovimientoXBodega.TransfRepuestos
                intTipoItemAceptado = 1
                strNoBodegaActual = p_strNoBodegaRepu
            Case scgTiposMovimientoXBodega.TransfSuministros
                intTipoItemAceptado = 3
                strNoBodegaActual = p_strNoBodegaSumi
            Case scgTiposMovimientoXBodega.TransfServiciosEx
                intTipoItemAceptado = 4
                strNoBodegaActual = p_strNoBodegaSeEx
        End Select
        If strNoBodegaActual <> p_strNoBodegaProceso Then

            udtLineaTransf.strItemCode = p_strItemCode
            udtLineaTransf.decCantidad = p_decCantidad
            udtLineaTransf.strNoBodegaDest = p_strNoBodegaProceso
            udtLineaTransf.strNoBodegaOrig = strNoBodegaActual

            p_lstItems.Add(udtLineaTransf)


        End If

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

                    dblStock = .InStock - .Committed

                    Exit For

                End If

            End With
        Next

        Return dblStock

    End Function

    Private Function DevuelveCantXLineasAnteriores(ByVal p_strItemCode As String, ByVal p_intLineNum As Integer, _
                           ByVal p_intDocEntry As Integer, ByVal p_oForm As SAPbouiCOM.Form) As Decimal

        Dim m_dtConsulta As SAPbouiCOM.DataTable
        Dim m_strConsulta As String = " SELECT DocEntry, LineNum, ItemCode, Quantity, OpenQty, U_SCGD_IdRepxOrd, U_SCGD_Aprobado, U_SCGD_Traslad, " & _
                                      " U_SCGD_CodEspecifico FROM QUT1 WHERE DocEntry = '{0}' AND ItemCode = '{1}' ORDER BY LineNum "
        m_strConsulta = String.Format(m_strConsulta, p_intDocEntry, p_strItemCode)
        m_dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta")
        m_dtConsulta.ExecuteQuery(m_strConsulta)

        Dim intContLineas As Decimal
        Dim decCantidadAnterior As Decimal = 0
        Dim strAprobado As String = String.Empty
        Dim strTrasladado As String = String.Empty
        Dim strCantidad As String = String.Empty
        Dim decCantidad As Decimal = 0

        For intContLineas = 0 To m_dtConsulta.Rows.Count - 1
            strAprobado = m_dtConsulta.GetValue("U_SCGD_Aprobado", intContLineas).ToString.Trim()
            strTrasladado = m_dtConsulta.GetValue("U_SCGD_Traslad", intContLineas).ToString.Trim()
            strCantidad = m_dtConsulta.GetValue("Quantity", intContLineas).ToString.Trim()

            If Not String.IsNullOrEmpty(strCantidad) Then
                decCantidad = Decimal.Parse(strCantidad)
            End If

            If m_dtConsulta.GetValue("LineNum", intContLineas).ToString.Trim() < p_intLineNum Then
                If strAprobado = "1" AndAlso (strTrasladado = "0" Or strTrasladado = "3") Then
                    decCantidadAnterior += decCantidad
                End If
            Else
                Exit For
            End If
        Next

        Return decCantidadAnterior

        'Dim objUtilitarios As New SCGDataAccess.Utilitarios(strConectionString)
        'Dim dstCotizacionLineas As Cotizacion_LineasDataset
        'Dim drwCotizacionLinea As Cotizacion_LineasDataset.Cotizacion_LineasRow

        'Dim intContLineas As Integer
        'Dim decCantidadAnterior As Integer = 0

        'dstCotizacionLineas = objUtilitarios.ObtenerItemsCotizaRepetidosByItemCode(p_intDocEntry, p_intLineNum, p_strItemCode)

        'For intContLineas = 0 To dstCotizacionLineas.Cotizacion_Lineas.Rows.Count - 1

        '    drwCotizacionLinea = dstCotizacionLineas.Cotizacion_Lineas.Rows(intContLineas)

        '    If drwCotizacionLinea.LineNum < p_intLineNum Then

        '        If drwCotizacionLinea.U_SCGD_Aprobado = 1 AndAlso (drwCotizacionLinea.U_SCGD_Traslad = 0 Or _
        '                drwCotizacionLinea.U_SCGD_Traslad = 3) Then

        '            decCantidadAnterior += drwCotizacionLinea.Quantity

        '        End If

        '    Else

        '        Exit For

        '    End If

        'Next

        'Return decCantidadAnterior

    End Function

    Private Sub ActualizarEstadoItems(ByVal lstItemActualizar As Generic.List(Of LineasCambiarEstado), _
                                            ByVal p_strNoOrden As String, _
                                            ByVal p_cnnConection As SqlClient.SqlConnection, _
                                            ByRef p_trnTransaccion As SqlClient.SqlTransaction, _
                                            ByVal p_blnEstadoPendiente As Boolean,
                                            Optional ByVal p_blnIniciaTransaccion As Boolean = False)

        Dim adpEstadosRepuestos As New RepuestosxEstadoDataAdapter(True)
        Dim dtbRepuestosCambiar As New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
        Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
        Dim udfRepuesto As LineasCambiarEstado

        For Each udfRepuesto In lstItemActualizar

            drwRepuesto = dtbRepuestosCambiar.NewSCGTA_TB_RepuestosxOrdenRow
            drwRepuesto.Cantidad = udfRepuesto.decCantidad
            drwRepuesto.NoOrden = p_strNoOrden
            drwRepuesto.NoRepuesto = udfRepuesto.strItemCode
            drwRepuesto.LineNum = udfRepuesto.intLineNum
            dtbRepuestosCambiar.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)

        Next

        adpEstadosRepuestos.Update(dtbRepuestosCambiar, p_cnnConection, p_trnTransaccion, p_blnEstadoPendiente, p_blnIniciaTransaccion)

    End Sub


    ' Actualiza el costo de los repuestos en la tabla RepuestosXOrden
    Public Sub ActualizarCostoRepuestosXOrden(ByVal lstItemActualizar As Generic.List(Of LineasTransferenciaStock), _
                                               ByVal p_strDocEntryTransferencia As String, _
                                               ByVal p_strNoOrden As String, _
                                               ByVal p_cnnConection As SqlClient.SqlConnection)
        'ByVal p_trnTransaccion As SqlClient.SqlTransaction

        Dim dtbRepuestosCambiar As New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable
        Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
        Dim lineasRepuestos As LineasTransferenciaStock
        Dim adpRepuestosXOrden As New RepuestosxOrdenDataAdapter()

        Dim strArregloTranferenciaStock() As String = Nothing
        Dim intContStockTransfer As Integer = 0
        Dim oTransfStockRep As SAPbobsCOM.StockTransfer

        Dim intContStockTransferLines As Integer = 0
        Dim drwRepuestoXOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

        Dim intVisOrderLinea As Integer
        Dim intDocEntryLinea As Integer
        Dim decCosto As Decimal

        For Each lineasRepuestos In lstItemActualizar
            drwRepuesto = dtbRepuestosCambiar.NewSCGTA_TB_RepuestosxOrdenRow
            drwRepuesto.NoOrden = p_strNoOrden
            drwRepuesto.NoRepuesto = lineasRepuestos.strItemCode
            drwRepuesto.LineNum = lineasRepuestos.intLineNum
            dtbRepuestosCambiar.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)
        Next

        strArregloTranferenciaStock = p_strDocEntryTransferencia.Split(",")
        For intContStockTransfer = 0 To (strArregloTranferenciaStock.Length) - 1

            oTransfStockRep = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            oTransfStockRep.GetByKey(CInt(strArregloTranferenciaStock(intContStockTransfer)))
            intDocEntryLinea = CInt(oTransfStockRep.DocEntry)

            For intContStockTransferLines = 0 To oTransfStockRep.Lines.Count() - 1
                oTransfStockRep.Lines.SetCurrentLine(intContStockTransferLines)


                intVisOrderLinea = CInt(oTransfStockRep.Lines.LineNum)

                For Each drwRepuestoXOrden In dtbRepuestosCambiar.Rows

                    If oTransfStockRep.Lines.ItemCode = drwRepuestoXOrden.NoRepuesto Then

                        decCosto = CDec(Utilitarios.EjecutarConsulta("select StockPrice from [WTR1]where DocEntry = '" & intDocEntryLinea & "' and VisOrder= '" & intVisOrderLinea & "'", m_objCompany.CompanyDB, m_objCompany.Server))
                        drwRepuestoXOrden.Costo = decCosto
                    End If

                Next
            Next
        Next
        adpRepuestosXOrden.UpdateCostoRepuestosXOrden(dtbRepuestosCambiar, p_cnnConection)

    End Sub


    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                        ByRef pVal As SAPbouiCOM.ItemEvent, _
                                        ByRef BubbleEvent As Boolean)

        Dim l_strValidaEntrega As String = "N"
        Dim l_strSQL As String
        Dim l_strNoOt As String


        l_strSQL = "Select U_Entrega_Rep FROM dbo.[@SCGD_CONF_SUCURSAL]  with(nolock) " +
                    " WHERE U_Sucurs = (select U_SCGD_idSucursal from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}')"

        Dim oForm As SAPbouiCOM.Form
        oForm = m_objSBO_Application.Forms.Item(FormUID)

        If pVal.ActionSuccess Then
            Select Case pVal.ItemUID
                Case "1"

                    l_strNoOt = oForm.DataSources.DBDataSources.Item("OWTR").GetValue("U_SCGD_Numero_OT", 0).Trim()
                    If Not String.IsNullOrEmpty(l_strNoOt) Then
                        l_strValidaEntrega = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, l_strNoOt), m_objCompany.CompanyDB, m_objCompany.Server)
                    End If
                    If l_strValidaEntrega = "Y" AndAlso
                        Not String.IsNullOrEmpty(l_strNoOt) Then
                        m_objSBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None)
                        ActualizaCotizacion(oForm)

                    End If
            End Select

        End If

    End Sub


    Public Sub ActualizaCotizacion(ByRef p_oForm As SAPbouiCOM.Form)
        Try
            Dim l_strDocEntryCot As String
            Dim l_strDocEntryTrans As String

            Dim l_strNumOT As String
            Dim l_strEntregado As String
            Dim l_strItemCode As String
            Dim l_strItemType As String

            Dim strCosteoVeh As String = ""
            Dim decCosteoVeh As Decimal = 0

            l_strNumOT = p_oForm.DataSources.DBDataSources.Item("OWTR").GetValue("U_SCGD_Numero_OT", 0).Trim
            l_strEntregado = p_oForm.DataSources.DBDataSources.Item("OWTR").GetValue("U_SCGD_Entregado", 0).Trim
            l_strDocEntryCot = Utilitarios.EjecutarConsulta(String.Format("Select DocEntry from OQUT where U_SCGD_Numero_OT = '{0}'", l_strNumOT),
                                                         m_objCompany.CompanyDB, m_objCompany.Server)

            l_strDocEntryTrans = p_oForm.DataSources.DBDataSources.Item("OWTR").GetValue("DocEntry", 0).Trim


            Dim oCotizacion As SAPbobsCOM.Documents
            Dim oLineCot As SAPbobsCOM.Document_Lines

            Dim oStock As SAPbobsCOM.StockTransfer
            Dim oStockLines As SAPbobsCOM.StockTransfer_Lines

            oCotizacion = CType(m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
            oCotizacion.GetByKey(l_strDocEntryCot)
            oLineCot = oCotizacion.Lines

            m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            oStock = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            oStock.GetByKey(l_strDocEntryTrans)
            oStockLines = oStock.Lines


            For i As Integer = 0 To oStockLines.Count - 1

                oStockLines.SetCurrentLine(i)

                l_strItemCode = oStockLines.ItemCode()

                For j As Integer = 0 To oLineCot.Count - 1
                    oLineCot.SetCurrentLine(j)
                    If l_strItemCode = oLineCot.ItemCode And oLineCot.UserFields.Fields.Item("U_SCGD_Aprobado").Value = "1" And (oLineCot.UserFields.Fields.Item("U_SCGD_Entregado").Value <> l_strEntregado) And oLineCot.Quantity = oStockLines.Quantity Then
                        oLineCot.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado
                        Exit For
                    End If
                Next
                'End If

            Next
            oCotizacion.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub





    Public Shared Sub AgregaControlCheck(ByVal oform As SAPbouiCOM.Form,
                                      ByVal NombreTablaSBO As String,
                                      ByVal p_SBO_Application As SAPbouiCOM.Application,
                                      Optional ByVal p_Top As Integer = 81,
                                      Optional ByVal p_Left As Integer = 81)

        Dim oitem As SAPbouiCOM.Item
        Dim oitem2 As SAPbouiCOM.Item
        Dim oCheck As SAPbouiCOM.CheckBox
        Dim oStatic As SAPbouiCOM.StaticText
        Dim strEtiqueta As String

        Try

            Dim ItemRef As String = "11" 'item de referencia para tomar el alto y agregar el control de numero de Ot de la Transferencia de Stock
            Dim intTopReF As Integer = 0
            Dim intleftReF As Integer = 0

            If oform.TypeEx = "940" Then
                oitem = oform.Items.Item(ItemRef)
                intTopReF = oitem.Top
                intleftReF = oitem.Left

                oitem = oform.Items.Add(m_strUIDEntregado, SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX)
                oitem.Left = intleftReF + p_Left
                oitem.Top = intTopReF + p_Top
                oitem.FromPane = 0
                oitem.ToPane = 0
                oCheck = oitem.Specific
                oCheck.Caption = My.Resources.Resource.CapEntregado

                Call oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                Call oCheck.DataBind.SetBound(True, NombreTablaSBO, m_strUDFEntregado)

            End If


        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

End Class


