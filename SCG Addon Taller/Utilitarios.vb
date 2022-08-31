Imports System.Xml
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports SAPbobsCOM
Imports System.Data.SqlClient
Imports DMSOneFramework.SCGCommon
Imports SAPbouiCOM
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports System.IO
Imports System.Linq
Imports DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
Imports DMS_Connector.Business_Logic.DataContract.Configuracion.Parametrizaciones_Generales
Imports DMS_Connector.Business_Logic.DataContract.SAPDocumento

Public Class Utilitarios

#Region "Declaraciones"

    Private Structure Cuentas

        Const CuentaIngresos As String = "RevenuesAc"
        Const CuentaCostos As String = "SaleCostAc"
        Const CuentaGastos As String = "ExpensesAc"

    End Structure

    Public Structure TipoCentroCosto
        Const U_Rep As String = "U_Rep"
        Const U_Ser As String = "U_Ser"
        Const U_Sum As String = "U_Sum"
        Const U_SE As String = "U_SE"
        Const U_Pro As String = "U_Pro"
    End Structure

    Public Shared blnAutoMarcaEstiloModelo As Boolean = False
    Private Shared dtSucursales As Data.DataTable = Nothing
    Private Shared ltPermisosMenu As List(Of String) = Nothing
    Private Shared ltDescripcionMenu As List(Of DescripcionMenu) = Nothing
    Public Shared bLoadInputEvents As Boolean = False
    Public Shared bLoadInvVehiEvents As Boolean = False
    Public Shared sbTipoCambio As SBObob = Nothing
    Private Shared intIdSucursal As Integer

    Public Structure DescripcionMenu
        Dim strMenu As String
        Dim strDescripcion As String
    End Structure

    Public Structure MenusPlanVentas

        Dim strMenu As String
        Dim strEstado As String
        Dim strCodigo As String
        Dim intNivel As String
        Dim blnPorEmpleado As Boolean
        Dim blnUsaMenu As Boolean

    End Structure

    Public Structure ListadoValidValues
        Dim strCode As String
        Dim strName As String
        Dim blnExistente As Boolean
    End Structure

    Public Const mc_strCodUnidadAsientos As String = "U_SCGD_Cod_Unidad"
    Public Const mc_strTransaccionAsientos As String = "U_SCGD_Cod_Tran"
    Public Shared m_strDocumentoMensaje As String = ""
    Public Shared strTipoDocumentoServicio As String = "S"
    Public Shared strTipoDocumentoArticulo As String = "I"

    Private Shared siteName As String = String.Empty

    Public Enum RecibeMensaje
        EncargadoTaller = 0
        Bodeguero = 1
        Asesor = 2
        EncargadoRepuestos = 3
        EncargadoSuministros
    End Enum

    Public Enum TipoMensaje
        scgPeticionRepuestos = 1
        scgPeticionSuministros = 2
        scgDevolucionRepuestos = 3
        scgDevolucionSuministros = 4
    End Enum

    Public Enum RolesMensajeria
        EncargadoRepuestos = 1
        EncargadoProduccion = 2
        EncargadoSolEspec = 3
        EncargadoCompras = 4
        EncargadoSOE = 5
        EncargadoSuministros = 6
    End Enum

    Public Enum TiposArticulos
        scgRepuesto = 1
        scgActividad = 2
        scgSuministro = 3
        scgServicioExt = 4
        scgPaquete = 5
        scgNinguno = 0
        scgOtrosGastos_Costos = 11
        scgOtrosIngresos = 12
    End Enum

    Public Enum ArticuloAprobado

        scgSi = 1
        scgNo = 2
        scgFalta = 3

    End Enum


    Public Enum Account
        ExpensesAc = 0
        TransferAc = 1
        CtaDebitoMO = 2
        CtaDebitoCosto = 3
        CtaDotacionSE = 4
        CtaGastosSE = 5
        CtaDifPrecioSE = 6
        CtaCostosBVSE = 7
        CuentaSys_C = 8
        CtaAcreGast = 9
        CtaDebGast = 10
        SaleCostAc = 11
    End Enum
#End Region

#Region "Procedimientos y funciones"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsulta(ByRef p_strConsulta As String, _
                                     ByRef p_strDatabaseName As String, _
                                     ByRef p_strServerName As String) As String

        Dim cmdEjecutarConsulta As New SqlCommand
        Dim strConectionString As String = ""
        Dim strValor As String = ""
        Try
            Configuracion.CrearCadenaDeconexion(p_strServerName, p_strDatabaseName, strConectionString)
            Using cn_Coneccion As New SqlConnection
                cn_Coneccion.ConnectionString = strConectionString
                cn_Coneccion.Open()
                cmdEjecutarConsulta.Connection = cn_Coneccion
                cmdEjecutarConsulta.CommandType = CommandType.Text
                cmdEjecutarConsulta.CommandText = p_strConsulta

                Using drdResultadoConsulta As SqlDataReader = cmdEjecutarConsulta.ExecuteReader
                    Do While drdResultadoConsulta.Read
                        If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                            strValor = drdResultadoConsulta.Item(0)
                            Exit Do
                        End If
                    Loop
                End Using
            End Using
        Catch
            Throw
        End Try
        Return strValor
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsulta(ByRef p_strConsulta As String) As String
        Return DMS_Connector.Helpers.EjecutarConsulta(p_strConsulta)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oLineaDocumentoMarketing"></param>
    ''' <param name="oOrdenDeCompra"></param>
    ''' <param name="FechaFactura"></param>
    ''' <param name="NoFactura"></param>
    ''' <param name="SerieFactura"></param>
    ''' <param name="NoOrdenTrabajo"></param>
    ''' <param name="m_oCompany"></param>
    ''' <param name="p_bNuevoTrack"></param>
    ''' <param name="p_bNotaCredito"></param>
    ''' <remarks></remarks>
    Shared Sub ActualizarLineaTrackinginterno(ByVal oLineaDocumentoMarketing As Document_Lines, _
                                          ByRef oOrdenDeCompra As Documents, _
                                          ByVal FechaFactura As Date, _
                                          ByVal NoFactura As Integer, _
                                          ByVal SerieFactura As Integer, _
                                          ByRef NoOrdenTrabajo As String, ByVal m_oCompany As SAPbobsCOM.Company, ByVal p_bNuevoTrack As Boolean, ByVal p_bNotaCredito As Boolean)

        Dim oCompanyService As CompanyService
        Dim oGeneralService As GeneralService
        Dim oGeneralParams As GeneralDataParams
        Dim oGeneralData As GeneralData
        Dim strConsultaDocEntry As String = " Select DocEntry  from [@SCGD_REPTR]  where U_IdRep = '{0}' and U_NoOrden = '{1}'"
        Dim strConsultaResultado As String
        Dim strEtiquetadeSerie As String = ""
        Dim dateFechaEntre As Date
        Dim strNoFActura As String
        Dim strNoOrdenCompra As String
        Dim intCantSum As Integer
        Dim dCostoRepuesto As Double
        Dim dPrecioCompraReal As Double
        Dim dDescuento As Double
        Dim dMontoDescuento As Double
        Dim strObservaciones As String
        Dim mc_strGuion As String = "-"

        strConsultaResultado = EjecutarConsulta(String.Format(strConsultaDocEntry, oLineaDocumentoMarketing.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim, NoOrdenTrabajo))
        If Not String.IsNullOrEmpty(strConsultaResultado) Then

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_REPTR")

            oGeneralParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)

            oGeneralParams.SetProperty("DocEntry", Convert.ToInt32(strConsultaResultado))
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            If Not p_bNuevoTrack Then

                dateFechaEntre = New Date(FechaFactura.Year, FechaFactura.Month, FechaFactura.Day, _
                                               DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)

                Call ComprasCls.DevuelveEtiquetaDeSerie(SerieFactura, m_oCompany, strEtiquetadeSerie)

                strNoFActura = strEtiquetadeSerie & mc_strGuion & NoFactura

                Call ComprasCls.DevuelveEtiquetaDeSerie(oOrdenDeCompra.Series, m_oCompany, strEtiquetadeSerie)

                strNoOrdenCompra = strEtiquetadeSerie & mc_strGuion & oOrdenDeCompra.DocNum

                intCantSum = oLineaDocumentoMarketing.Quantity

                dCostoRepuesto = CDec(oLineaDocumentoMarketing.Quantity * oLineaDocumentoMarketing.Price)

                dPrecioCompraReal = CDec(dCostoRepuesto - ((dCostoRepuesto) * (oLineaDocumentoMarketing.DiscountPercent / 100)))

                dDescuento = CDec(oLineaDocumentoMarketing.DiscountPercent)

                dMontoDescuento = dCostoRepuesto - dPrecioCompraReal

                strObservaciones = oOrdenDeCompra.Comments


                oGeneralData.SetProperty("U_NoFact", strNoFActura)
                oGeneralData.SetProperty("U_NoCom", strNoOrdenCompra)
                oGeneralData.SetProperty("U_CantSum", intCantSum)
                oGeneralData.SetProperty("U_PreComRe", dPrecioCompraReal)
                oGeneralData.SetProperty("U_Desc", dDescuento)
                oGeneralData.SetProperty("U_MontDesc", dMontoDescuento)
                oGeneralData.SetProperty("U_Obser", strObservaciones)
                oGeneralData.SetProperty("U_CostRep", dCostoRepuesto)
                oGeneralData.SetProperty("U_FechEn", RetornaFechaFormatoDB(dateFechaEntre, m_oCompany.Server))


                oGeneralService.Update(oGeneralData)

            Else

                dateFechaEntre = New Date(FechaFactura.Year, FechaFactura.Month, FechaFactura.Day, _
                                            DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)


                dCostoRepuesto = CDec(oLineaDocumentoMarketing.Price / oLineaDocumentoMarketing.Quantity)

                dDescuento = CDec(oLineaDocumentoMarketing.DiscountPercent)

                strObservaciones = oOrdenDeCompra.Comments

                Call ComprasCls.DevuelveEtiquetaDeSerie(SerieFactura, m_oCompany, strEtiquetadeSerie)

                strNoFActura = strEtiquetadeSerie & mc_strGuion & NoFactura

                intCantSum = oLineaDocumentoMarketing.Quantity


                oGeneralData.SetProperty("U_NoFact", strNoFActura)
                oGeneralData.SetProperty("U_CantSum", intCantSum)
                oGeneralData.SetProperty("U_Desc", dDescuento)
                oGeneralData.SetProperty("U_Obser", strObservaciones)
                oGeneralData.SetProperty("U_CostRep", dCostoRepuesto)
                oGeneralData.SetProperty("U_FechEn", Utilitarios.RetornaFechaFormatoDB(dateFechaEntre, m_oCompany.Server))

                oGeneralService.Update(oGeneralData)

            End If
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objDocumento"></param>
    ''' <remarks></remarks>
    Shared Sub DestruirObjeto(ByRef objDocumento As Object)
        DMS_Connector.Helpers.DestruirObjeto(objDocumento)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cardCode"></param>
    ''' <param name="p_ocompany"></param>
    ''' <param name="p_strGroupCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidadSocioNegociosLeasing(ByVal cardCode As String, ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_strGroupCode As String) As Boolean
        Dim oSocio As BusinessPartners
        Dim lid As Boolean
        oSocio = p_ocompany.GetBusinessObject(BoObjectTypes.oBusinessPartners)
        lid = False
        If oSocio.GetByKey(cardCode) Then
            If oSocio.GroupCode = p_strGroupCode Then
                lid = True
            End If
            DestruirObjeto(oSocio)

        End If
        Return lid

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strConEjeBan"></param>
    ''' <param name="strNrOC"></param>
    ''' <param name="strNrOL"></param>
    ''' <param name="p_SBO_Application"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidaInfoLeasing(ByVal strConEjeBan As String, ByVal strNrOC As String, ByVal strNrOL As String, p_SBO_Application As SAPbouiCOM.Application) As Boolean
        Dim bool As Boolean = True
        If String.IsNullOrEmpty(strConEjeBan) Then
            p_SBO_Application.StatusBar.SetText(My.Resources.Resource.FaltaIngresoContactoEjecutivoBanco, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            bool = False
        ElseIf String.IsNullOrEmpty(strNrOC) Then
            p_SBO_Application.StatusBar.SetText(My.Resources.Resource.FaltaIngresoNroOrdenCompra, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            bool = False
        ElseIf String.IsNullOrEmpty(strNrOL) Then
            p_SBO_Application.StatusBar.SetText(My.Resources.Resource.FaltaIngresoNroOperacionLeasing, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            bool = False
        End If
        Return bool
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsultaDataTable(ByRef p_strConsulta As String) As Data.DataTable

        Return DMS_Connector.Helpers.EjecutarConsultaDataTable(p_strConsulta)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsultaDataTable(ByRef p_strConsulta As String, _
                                     ByRef p_strDatabaseName As String, _
                                     ByRef p_strServerName As String) As System.Data.DataTable

        Dim strConectionString As String
        Dim dt As New System.Data.DataTable
        Try
            Configuracion.CrearCadenaDeconexion(p_strServerName, p_strDatabaseName, strConectionString)

            Using cn_Coneccion As New SqlClient.SqlConnection(strConectionString)

                cn_Coneccion.Open()

                Using cmdEjecutarConsulta As New SqlClient.SqlCommand

                    cmdEjecutarConsulta.Connection = cn_Coneccion

                    cmdEjecutarConsulta.CommandType = CommandType.Text
                    cmdEjecutarConsulta.CommandText = p_strConsulta

                    Dim drdResultadoConsulta As SqlClient.SqlDataReader = cmdEjecutarConsulta.ExecuteReader

                    dt.Load(drdResultadoConsulta)

                End Using

            End Using

        Catch
            Throw
        Finally

        End Try

        Return dt

    End Function

    'se recorre la tabla, en busca de la condicion enviada, 
    'para retornar el campo seleccionado 
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tabla"></param>
    ''' <param name="CampoSeleccionar"></param>
    ''' <param name="CampoCondicion"></param>
    ''' <param name="ValorCondicion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function SeleccionaCampo(ByVal tabla As DataTable, _
                                    ByVal CampoSeleccionar As String, _
                                    ByVal CampoCondicion As String, _
                                    ByVal ValorCondicion As String) As String
        Try
            Dim valorSeleccionado As String = ""

            If tabla.Rows.Count > 0 Then
                For i As Integer = 0 To tabla.Rows.Count - 1
                    If tabla.GetValue(CampoCondicion, i) = ValorCondicion Then
                        valorSeleccionado = tabla.GetValue(CampoSeleccionar, i)
                        Return valorSeleccionado
                    End If
                Next
                Return valorSeleccionado
            End If

            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsultaDecimal(ByRef p_strConsulta As String, _
                                    ByRef p_strDatabaseName As String, _
                                    ByRef p_strServerName As String) As Decimal

        Try
            Return DMS_Connector.Helpers.EjecutarConsultaDecimal(p_strConsulta)

        Catch ex As Exception
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsultaPrecios(ByRef p_strConsulta As String, _
                                     ByRef p_strDatabaseName As String, _
                                     ByRef p_strServerName As String) As Decimal
        Dim drdResultadoConsulta As SqlClient.SqlDataReader = Nothing
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim tipoC As Decimal = -1

        Try
            Configuracion.CrearCadenaDeconexion(p_strServerName, p_strDatabaseName, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdEjecutarConsulta.Connection = cn_Coneccion

            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = p_strConsulta
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                    tipoC = CDec(drdResultadoConsulta.Item(0))
                    Exit Do
                End If
            Loop
        Catch
            Throw
        Finally
            If Not drdResultadoConsulta Is Nothing Then drdResultadoConsulta.Close()
            cmdEjecutarConsulta.Connection.Close()
        End Try
        Return tipoC

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Precio"></param>
    ''' <param name="strMonedaVehiculo"></param>
    ''' <param name="strMonedaSistema"></param>
    ''' <param name="strMonedaLocal"></param>
    ''' <param name="strMonedaCV"></param>
    ''' <param name="strTipoCambioSistema"></param>
    ''' <param name="strTipoCambioCV_Local"></param>
    ''' <param name="fecha"></param>
    ''' <param name="n"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function RetornaPrecioXTipoMoneda(ByVal Precio As Decimal, _
                                             ByVal strMonedaVehiculo As String, _
                                             ByVal strMonedaSistema As String, _
                                             ByVal strMonedaLocal As String, _
                                             ByVal strMonedaCV As String, _
                                             ByVal strTipoCambioSistema As String, _
                                             ByVal strTipoCambioCV_Local As String, _
                                             ByVal fecha As Date, _
                                             ByVal n As NumberFormatInfo) As Decimal

        Dim dcPrecioConvertido As Decimal

        If String.IsNullOrEmpty(strTipoCambioSistema) Then
            strTipoCambioSistema = 1
        End If

        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
            strTipoCambioCV_Local = 1
        End If

        '*****************************************
        'CASOS:     M_CV = ML
        '               M_C = ML
        '               M_C = MS
        '               M_C = MO
        '           M_CV = MS
        '               M_C = ML
        '               M_C = MS
        '               M_C = MO
        '           M_CV = MO
        '               M_C = ML
        '               M_C = MS
        '               M_C = MO
        '*****************************************
        Select Case strMonedaCV
            Case strMonedaLocal

                Select Case strMonedaVehiculo

                    Case strMonedaLocal, ""

                        'local = local
                        dcPrecioConvertido = Decimal.Parse(Precio)

                    Case strMonedaSistema

                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                            strTipoCambioSistema = 1
                        End If
                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                        'local = costo * tc_sistema
                        'dcPrecioConvertido = Decimal.Parse(Precio, n)
                        'dcPrecioConvertido = Decimal.Parse(Precio)
                        dcPrecioConvertido = Precio
                        dcPrecioConvertido = dcPrecioConvertido * tc_MonedaSistema
                        'dcPrecioConvertido = dcPrecioConvertido * tc_MonedaSistema

                    Case Else

                        Dim costoOtro As Integer

                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                            strTipoCambioCV_Local = 1
                        End If
                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local, n)

                        'local = costo * tc_cv
                        dcPrecioConvertido = Decimal.Parse(Precio)
                        costoOtro = dcPrecioConvertido * tc_MonedaCV

                        dcPrecioConvertido = costoOtro

                End Select

            Case strMonedaSistema

                Select Case strMonedaVehiculo

                    Case strMonedaLocal, ""

                        'Dim costoLocal As Integer
                        Dim costoLocal As Decimal

                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                            strTipoCambioSistema = 1
                        End If
                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                        'sistema = costo / tc_ms
                        'dcPrecioConvertido = Decimal.Parse(Precio)
                        dcPrecioConvertido = Precio
                        costoLocal = dcPrecioConvertido / tc_MonedaSistema

                        dcPrecioConvertido = Decimal.Parse(costoLocal)

                    Case strMonedaSistema

                        'sistema = sistema
                        ' dcPrecioConvertido = Decimal.Parse(Precio, n)
                        dcPrecioConvertido = Precio


                    Case Else

                        'Dim costoOtro As Integer
                        Dim costoOtro As Decimal

                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                            strTipoCambioSistema = 1
                        End If
                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                            strTipoCambioCV_Local = 1
                        End If
                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local, n)

                        'sistema = (costo * tc_mcv) / tc_ms
                        dcPrecioConvertido = Precio
                        'dcPrecioConvertido = Decimal.Parse(Precio)

                        costoOtro = dcPrecioConvertido * tc_MonedaCV
                        costoOtro = costoOtro / tc_MonedaSistema

                        dcPrecioConvertido = costoOtro

                End Select

            Case Else

                Select Case strMonedaVehiculo

                    Case strMonedaLocal, ""
                        'Dim costoLocal As Integer
                        Dim costoLocal As Decimal

                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                            strTipoCambioCV_Local = 1
                        End If
                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local, n)

                        'sistema = costo / tc_mcv
                        'dcPrecioConvertido = Decimal.Parse(Precio)
                        dcPrecioConvertido = Precio
                        costoLocal = dcPrecioConvertido / tc_MonedaCV

                        dcPrecioConvertido = costoLocal

                    Case strMonedaSistema

                        ' Dim costoSistema As Integer
                        Dim costoSistema As Decimal

                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                            strTipoCambioSistema = 1
                        End If
                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                            strTipoCambioCV_Local = 1
                        End If
                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local, n)

                        'sistema = (costo * tc_ms) / tc_mcv
                        'dcPrecioConvertido = Decimal.Parse(Precio)
                        dcPrecioConvertido = Precio

                        costoSistema = dcPrecioConvertido * tc_MonedaSistema
                        costoSistema = costoSistema / tc_MonedaCV

                        dcPrecioConvertido = costoSistema

                    Case Else

                        'otro = otro
                        'dcPrecioConvertido = Decimal.Parse(Precio)
                        dcPrecioConvertido = Precio

                End Select

        End Select

        Return dcPrecioConvertido

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strFormUID"></param>
    ''' <param name="blnselectIfOpen"></param>
    ''' <param name="SBO_Application"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                                ByVal blnselectIfOpen As Boolean, _
                                                ByVal SBO_Application As Application) As Boolean
        '*******************************************************************    
        'Propósito:  
        'Acepta:    
        'Retorna:   
        'Desarrollador: Yeiner Aguirre 0.
        'Fecha: 
        '*******************************************************************
        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As Form

        Dim a As Integer = SBO_Application.Forms.Count

        While (Not blnFound AndAlso intI < SBO_Application.Forms.Count)

            frmForma = SBO_Application.Forms.Item(intI)
            If frmForma.UniqueID = strFormUID Then
                blnFound = True
                If (blnselectIfOpen) Then
                    If Not (frmForma.Selected) Then
                        SBO_Application.Forms.Item(strFormUID).Select()
                    End If
                End If
            Else
                intI += 1
            End If

        End While
        Return blnFound

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strTransaccion"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DevuelveTransaccionesAVisualizar(ByRef p_strTransaccion As String) As String

        Dim dtCodesTran As Data.DataTable
        Dim strValor As String = String.Empty
        dtCodesTran = EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strDevuelveTransacciones"), p_strTransaccion))

        For Each drCodeTran As DataRow In dtCodesTran.Rows
            If String.IsNullOrEmpty(strValor) Then
                strValor &= drCodeTran.Item(0)
            Else
                strValor &= "," & drCodeTran.Item(0)
            End If
        Next

        Return strValor

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strTipoOT"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DevuelveTransaccionFacturaInterna(ByRef p_strTipoOT As String, _
                             ByRef p_strDatabaseName As String, _
                             ByRef p_strServerName As String) As String

        Dim strConectionString As String = String.Empty
        Dim strValor As String = String.Empty


        Configuracion.CrearCadenaDeconexion(p_strServerName, p_strDatabaseName, strConectionString)

        Using cn_Coneccion As New SqlClient.SqlConnection(strConectionString)

            cn_Coneccion.Open()

            Using cmdEjecutarConsulta As New SqlClient.SqlCommand

                cmdEjecutarConsulta.Connection = cn_Coneccion

                cmdEjecutarConsulta.CommandType = CommandType.Text
                cmdEjecutarConsulta.CommandText = "Select Tran_Comp from dbo.SCGTA_TB_Conf_Ot_Iterna with(nolock) where ID_Tipo_Ot = '" & p_strTipoOT & "'"

                Using drdResultadoConsulta As SqlClient.SqlDataReader = cmdEjecutarConsulta.ExecuteReader

                    Do While drdResultadoConsulta.Read
                        If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                            strValor = drdResultadoConsulta.Item(0)
                        End If
                    Loop


                End Using

            End Using

        End Using

        Return strValor

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="seccion"></param>
    ''' <param name="clave"></param>
    ''' <param name="valor"></param>
    ''' <param name="p_configXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function SacarValorObjectKey(ByVal seccion As String, _
                                  ByVal clave As String, _
                                  ByRef valor As String, _
                                  ByRef p_configXML As Xml.XmlDocument) As Boolean

        Dim n As Xml.XmlNode
        n = p_configXML.SelectSingleNode(seccion)

        If Not n Is Nothing Then

            valor = (n.InnerText)

        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ParseKey(ByVal key As String, ByVal path As String) As String
        Dim xml As XmlDocument = New XmlDocument()
        Dim xmlNL As XmlNodeList
        xml.LoadXml(key)
        xmlNL = xml.SelectNodes(path)
        Return xmlNL.Item(0).InnerText
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strSeparardorMilesSAP"></param>
    ''' <param name="p_strSeparadorDecimalesSAP"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <remarks></remarks>
    Shared Sub ObtenerSeparadoresNumerosSAP(ByRef p_strSeparardorMilesSAP As String, _
                                                    ByRef p_strSeparadorDecimalesSAP As String, _
                                                    ByRef p_strDatabaseName As String, _
                                                    ByRef p_strServerName As String)

        DMS_Connector.Helpers.GetSeparadoresSAP(p_strSeparardorMilesSAP, p_strSeparadorDecimalesSAP)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strValorNumero"></param>
    ''' <param name="p_strSeparadorMilesSAP"></param>
    ''' <param name="p_strSeparadorDecilesSAP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function CambiarValoresACultureActual(ByVal p_strValorNumero As String, _
                                                 ByVal p_strSeparadorMilesSAP As String, _
                                                 ByVal p_strSeparadorDecilesSAP As String) As String

        Dim strSeparadorDecimales As String

        strSeparadorDecimales = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        p_strValorNumero = p_strValorNumero.Replace(p_strSeparadorMilesSAP, "")
        p_strValorNumero = p_strValorNumero.Replace(p_strSeparadorDecilesSAP, strSeparadorDecimales)
        If p_strValorNumero = "" Then
            p_strValorNumero = "0"
        End If

        Return p_strValorNumero

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strDecimal"></param>
    ''' <param name="n"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ConvierteDecimal(ByVal strDecimal As String, _
                                     ByVal n As NumberFormatInfo) As Decimal
        'Variable a obtener el valor final
        Dim dcDecimal As Decimal

        'Elimino espacios al string 
        strDecimal = strDecimal.Trim()

        'Convierto el valor del string a decimal
        If Not String.IsNullOrEmpty(strDecimal) Then
            dcDecimal = Decimal.Parse(strDecimal, n)
        Else
            dcDecimal = 0
        End If

        'Retorna el decimal
        Return dcDecimal

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="p_strCadenaConexionBDTaller"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function DevuelveCadenaConexionBDTaller(ByVal p_ocompany As Application, _
                                                   ByRef p_strCadenaConexionBDTaller As String) As Boolean

        Try

            Dim intIdSucursal As Integer
            Dim strNombreBDTaller As String

            intIdSucursal = ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO) 'Obtiene el id de sucursal que se necesita para sacar el nombre de la BD de Taller

            DevuelveNombreBDTaller(p_ocompany, intIdSucursal, strNombreBDTaller)

            If strNombreBDTaller <> "" Then
                'Crea la cadena para conectarse a la BD de Taller
                If Configuracion.CrearCadenaDeconexion(p_ocompany.Company.ServerName.ToLower, _
                                                       strNombreBDTaller, _
                                                       p_strCadenaConexionBDTaller) Then

                    Return True

                Else

                    p_ocompany.StatusBar.SetText(My.Resources.Resource.CompañiaSinSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                    Return False

                End If
            Else
                p_ocompany.StatusBar.SetText(My.Resources.Resource.CompañiaSinSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                Return False

            End If

        Catch ex As Exception
            Throw ex

        Finally


        End Try

    End Function

    ''' <summary>
    ''' Sobrecarga del metodo que recibe el ID de sucursal por parametro
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="idSucursal">ID de sucursal</param>
    ''' <param name="p_strCadenaConexionBDTaller">String de conexion</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DevuelveCadenaConexionBDTaller(ByVal p_ocompany As Application, _
                                                   ByVal p_strIdSucursal As String,
                                                   ByRef p_strCadenaConexionBDTaller As String) As Boolean

        Try

            Dim strNombreBDTaller As String

            DevuelveNombreBDTaller(p_ocompany, p_strIdSucursal, strNombreBDTaller)

            If strNombreBDTaller <> "" Then

                If Configuracion.CrearCadenaDeconexion(p_ocompany.Company.ServerName.ToLower, _
                                                       strNombreBDTaller, _
                                                       p_strCadenaConexionBDTaller) Then

                    Return True

                Else

                    p_ocompany.StatusBar.SetText(My.Resources.Resource.CompañiaSinSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                    Return False

                End If
            Else
                p_ocompany.StatusBar.SetText(My.Resources.Resource.CompañiaSinSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                Return False

            End If

        Catch ex As Exception
            Throw ex

        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="p_strNombreBDTaller"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function DevuelveNombreBDTaller(ByVal p_ocompany As Application, ByRef p_strNombreBDTaller As String) As Boolean

        Try

            Dim intIdSucursal As Integer

            intIdSucursal = ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO) 'Obtiene el id de sucursal que se necesita para sacar el nombre de la BD de Taller

            If dtSucursales Is Nothing Then
                dtSucursales = EjecutarConsultaDataTable(DMS_Connector.Queries.GetStrSpecificQuery("strNombreBDSucursales"))
            End If

            Dim drIndUsado() As DataRow = dtSucursales.Select(String.Format(" Code = '{0}'", intIdSucursal))
            If drIndUsado.Length <> 0 Then
                p_strNombreBDTaller = drIndUsado(0).Item("U_BDSucursal")
            Else
                p_strNombreBDTaller = String.Empty
            End If

            If Not String.IsNullOrEmpty(p_strNombreBDTaller) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Sobrecarga del metodo que recibe como parametro el ID de sucursal
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="idSucursal">ID de sucursal</param>
    ''' <param name="p_strNombreBDTaller">Nombre de base de datos sucursal</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DevuelveNombreBDTaller(ByVal p_ocompany As SAPbouiCOM.Application, ByVal p_strIdSucursal As String, ByRef p_strNombreBDTaller As String) As Boolean

        Try
            If dtSucursales Is Nothing Then
                dtSucursales = EjecutarConsultaDataTable(DMS_Connector.Queries.GetStrSpecificQuery("strNombreBDSucursales"))
            End If

            Dim drIndUsado() As DataRow = dtSucursales.Select(String.Format(" Code = '{0}'", p_strIdSucursal))
            If drIndUsado.Length <> 0 Then
                p_strNombreBDTaller = drIndUsado(0).Item("U_BDSucursal")
            Else
                p_strNombreBDTaller = String.Empty
            End If

            If Not String.IsNullOrEmpty(p_strNombreBDTaller) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function DevuelveOTsInternas(ByVal p_ocompany As Application) As List(Of String)
        Dim lsOtInterna As List(Of String) = New List(Of String)()
        Try
            lsOtInterna.AddRange((From tipoOt In DMS_Connector.Configuracion.TipoOt.Where(Function(x) x.U_Interna.Trim = "Y") Select TipoOT.Code).Cast(Of String)())
        Catch ex As Exception
            Throw ex
        End Try
        Return lsOtInterna
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="p_strDireccionReportes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function DevuelveDireccionReportes(ByVal p_ocompany As Application, ByRef p_strDireccionReportes As String) As Boolean

        Try
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim) Then
                p_strDireccionReportes = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function ObtieneIdSucursal(ByVal p_ocompany As Application) As Integer
        'Obtiene el id de la sucursal de la tabla OUSR a partir del nombre del usuario de SBO
        Try
            If intIdSucursal = 0 Then
                intIdSucursal = CInt(EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGetIdSucursal"), p_ocompany.Company.UserName)))
            End If
            Return intIdSucursal

        Catch ex As Exception
            Throw ex

        Finally

        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function ObtieneUserId(ByVal p_ocompany As Application) As Integer
        Try
            Return CInt(EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGetIdEmpleado"), p_ocompany.Company.UserName)))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="p_strSlpCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function ObtieneUserId(ByVal p_ocompany As Application, _
                                  ByVal p_strSlpCode As String) As String
        Try
            Return EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strUserIdBySlpCode"), p_strSlpCode))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function ObtieneEmpid(ByVal p_ocompany As Application) As Integer
        Try
            Dim strValor As String = EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrQueryFormat("strGetEmpId"), ObtieneUserId(p_ocompany)))
            If Not String.IsNullOrEmpty(strValor) Then
                Return CInt(strValor)
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function ObtieneSlpCode(ByVal p_ocompany As Application) As String
        Try
            Return EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGetSlpCode"), ObtieneUserId(p_ocompany)))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strSlpCode"></param>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Shared Function ObtieneSlpName(ByVal p_strSlpCode As String, _
                                   ByVal p_ocompany As Application) As String
        Try
            Return EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGetSlpName"), p_strSlpCode))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strMenu"></param>
    ''' <param name="p_strUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function MostrarMenu(ByVal p_strMenu As String, Optional ByVal p_strUsuario As String = "") As Boolean
        Return DMS_Connector.Helpers.PermisosMenu(p_strMenu)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strMenu"></param>
    ''' <param name="p_intIdioma"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function PermisosMenu(ByVal p_strMenu As String, ByVal p_intIdioma As Integer) As String

        Dim dtPermisosMenu As Data.DataTable = Nothing

        If ltDescripcionMenu Is Nothing Then
            ltDescripcionMenu = New List(Of DescripcionMenu)
            dtPermisosMenu = EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrQueryFormat("strNombreMenu"), p_intIdioma))
            For Each drPermisosMenu As DataRow In dtPermisosMenu.Rows
                ltDescripcionMenu.Add(New DescripcionMenu() With {.strMenu = drPermisosMenu.Item(0).ToString.Trim, .strDescripcion = drPermisosMenu.Item(1).ToString.Trim})
            Next
        End If
        Return ltDescripcionMenu.FirstOrDefault(Function(x) x.strMenu = p_strMenu.Trim).strDescripcion

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Shared Function ObtieneEmpname(ByVal p_ocompany As Application, Optional p_UserId As String = "") As String
        Dim intIdUsuario As Integer
        Dim strUserName As String
        Dim oEmployeesInfo As EmployeesInfo

        Try
            If String.IsNullOrEmpty(p_UserId) Then
                intIdUsuario = Utilitarios.ObtieneEmpid(p_ocompany)
            Else
                intIdUsuario = p_UserId
            End If

            oEmployeesInfo = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oEmployeesInfo)
            If oEmployeesInfo.GetByKey(intIdUsuario) Then
                strUserName = String.Format("{0} {1}", oEmployeesInfo.FirstName, oEmployeesInfo.LastName)
            End If
            Return strUserName

        Catch ex As Exception
            Throw ex
        Finally
            DestruirObjeto(oEmployeesInfo)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strIDVehiculo"></param>
    ''' <param name="p_strTipo"></param>
    ''' <param name="strMarca"></param>
    ''' <param name="strEstilo"></param>
    ''' <param name="strColor"></param>
    ''' <param name="strAño"></param>
    ''' <param name="strUnidad"></param>
    ''' <param name="strSerie"></param>
    ''' <param name="strPlaca"></param>
    ''' <param name="p_strServerName"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <remarks></remarks>
    Shared Sub ObtieneDatosVehiculos(ByVal p_strIDVehiculo As String, _
                                          ByRef p_strTipo As String, _
                                          ByRef strMarca As String, _
                                          ByRef strEstilo As String, _
                                          ByRef strColor As String, _
                                          ByRef strAño As String, _
                                          ByRef strUnidad As String, _
                                          ByRef strSerie As String, _
                                          ByRef strPlaca As String, _
                                          ByVal p_strServerName As String, _
                                          ByVal p_strDatabaseName As String)

        For Each dataRow As DataRow In EjecutarConsultaDataTable(String.Format(DMS_Connector.Queries.GetStrQueryFormat("strGetDatosVehiculos"), p_strIDVehiculo.Trim)).Rows
            If dataRow.Item("Tipo") IsNot DBNull.Value Then
                p_strTipo = String.Format(" {0}", dataRow.Item("Tipo"))
            End If
            If dataRow.Item("Marca") IsNot DBNull.Value Then
                strMarca = String.Format(" {0}", dataRow.Item("Marca"))
            End If
            If dataRow.Item("Placa") IsNot DBNull.Value Then
                strPlaca = String.Format(" {0}", dataRow.Item("Placa"))
            End If
            If dataRow.Item("Estilo") IsNot DBNull.Value Then
                strEstilo = String.Format(" {0} {1}", dataRow.Item("Estilo"), dataRow.Item("Modelo"))
            End If
            If dataRow.Item("Color") IsNot DBNull.Value Then
                strColor = String.Format(" {0}", dataRow.Item("Color"))
            End If
            If dataRow.Item("Año") IsNot DBNull.Value Then
                strAño = String.Format(" {0} {1}", My.Resources.Resource.Año, dataRow.Item("Año"))
            End If
            If dataRow.Item("Unidad") IsNot DBNull.Value Then
                strUnidad = String.Format(" {0}", dataRow.Item("Unidad"))
            End If
            If dataRow.Item("VIN") IsNot DBNull.Value Then
                strSerie = String.Format(" {0} {1}", My.Resources.Resource.VIN, dataRow.Item("VIN"))
            End If
            Exit For
        Next

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_Company"></param>
    ''' <param name="p_strCuenta"></param>
    ''' <param name="p_strCodigoTransaccion"></param>
    ''' <param name="p_strComentarios"></param>
    ''' <param name="p_decMontoAsiento"></param>
    ''' <param name="p_datFechaRegisto"></param>
    ''' <param name="p_strTransaccion1"></param>
    ''' <param name="p_strTransaccion2"></param>
    ''' <param name="p_blnUsaFecha"></param>
    ''' <param name="p_strMoneda"></param>
    ''' <param name="p_strTransaccionLineas"></param>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_oMatrix"></param>
    ''' <param name="p_fechaDocumento"></param>
    ''' <param name="p_objConfiguracionGeneral"></param>
    ''' <param name="p_strRef1"></param>
    ''' <param name="p_strRef2"></param>
    ''' <param name="p_monedaBD"></param>
    ''' <param name="p_blnUsaDimensiones"></param>
    ''' <param name="p_oDatatableDimensionesOT"></param>
    ''' <param name="p_ClsLineasDocumentos"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function CrearAsientoAjuste(ByVal p_Company As SAPbobsCOM.Company, _
                                 ByVal p_strCuenta As String, _
                                 ByVal p_strCodigoTransaccion As String, _
                                 ByVal p_strComentarios As String, _
                                 ByVal p_decMontoAsiento As Decimal, _
                                 ByVal p_datFechaRegisto As Date, _
                                 ByVal p_strTransaccion1 As String, _
                                 ByVal p_strTransaccion2 As String, _
                                 ByVal p_blnUsaFecha As Boolean, _
                                 ByVal p_strMoneda As String, _
                                 ByVal p_strTransaccionLineas As String, _
                                 ByVal p_oForm As Form, _
                                 ByVal p_oMatrix As Matrix, _
                                 ByVal p_fechaDocumento As Date, _
                                 ByVal p_objConfiguracionGeneral As ConfiguracionesGeneralesAddon, _
                                 Optional ByVal p_strRef1 As String = "", _
                                 Optional ByVal p_strRef2 As String = "", _
                                 Optional ByVal p_monedaBD As String = "",
                                 Optional ByVal p_blnUsaDimensiones As Boolean = False, _
                                 Optional ByVal p_oDatatableDimensionesOT As DataTable = Nothing, _
                                 Optional ByRef p_ClsLineasDocumentos As AgregarDimensionLineasDocumentosCls = Nothing) As Integer

        Dim oJournalEntry As JournalEntries
        Dim strMonedaLocal As String
        Dim strMonedaSistema As String

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim strNoAsiento As String

        Dim decAjuste As Decimal

        Dim strContraCuenta As String
        Dim strTipo As String

        Dim n As NumberFormatInfo = New NumberFormatInfo
        strNoAsiento = 0
        DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)

        oJournalEntry = p_Company.GetBusinessObject(BoObjectTypes.oJournalEntries)
        If p_blnUsaFecha Then
            oJournalEntry.ReferenceDate = p_datFechaRegisto
        End If
        oJournalEntry.TransactionCode = p_strCodigoTransaccion
        If p_strComentarios.Length() > 50 Then
            oJournalEntry.Memo &= p_strComentarios.Substring(0, 50)
        Else
            oJournalEntry.Memo &= p_strComentarios
        End If

        oJournalEntry.DueDate = p_fechaDocumento
        oJournalEntry.ReferenceDate = p_fechaDocumento
        oJournalEntry.TaxDate = p_fechaDocumento

        oJournalEntry.Reference = p_strTransaccion1

        'Cuenta
        oJournalEntry.Lines.AccountCode = p_strCuenta


        'se agrego para valoracion de moneda y conversion 
        If strMonedaLocal = p_strMoneda Then
            oJournalEntry.Lines.Debit = p_decMontoAsiento
        Else
            oJournalEntry.Lines.FCDebit = p_decMontoAsiento
            oJournalEntry.Lines.FCCurrency = p_strMoneda

        End If
        oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
        oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_strTransaccion1

        oJournalEntry.Lines.Reference1 = p_strRef1
        oJournalEntry.Lines.Reference2 = p_strRef2
        oJournalEntry.Lines.VatLine = BoYesNoEnum.tNO
        oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

        oJournalEntry.Lines.Add()

        For i As Integer = 0 To p_oMatrix.RowCount - 1

            strTipo = p_oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Tipo", i).Trim
            strTipo = EjecutarConsulta("Select Code from [@SCGD_TIPOVEHICULO] where Name = '" & strTipo & "'", p_Company.CompanyDB, p_Company.Server)

            strContraCuenta = p_objConfiguracionGeneral.CuentaInventarioTransito(strTipo)
            'strContraCuenta = DMS_Connector.Configuracion.ParamGenAddon.Admin4.FirstOrDefault(Function(admin4) admin4.U_Tipo.Trim() = strTipo).U_Transito

            oJournalEntry.Lines.AccountCode = strContraCuenta
            oJournalEntry.Lines.VatLine = BoYesNoEnum.tNO
            oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

            decAjuste = Utilitarios.ConvierteDecimal(p_oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Aj_Cos", i), n)
            Dim strMonedaContrato As String = p_oForm.DataSources.DBDataSources.Item("@SCGD_CVENTA").GetValue("U_Moneda", 0).Trim()

            decAjuste = CalcularCostosPorCambioMoneda(p_Company, p_strMoneda, decAjuste, strMonedaContrato, 1, p_datFechaRegisto)

            If strMonedaLocal = p_strMoneda Then
                oJournalEntry.Lines.Credit = decAjuste
            Else
                oJournalEntry.Lines.FCCredit = decAjuste
                oJournalEntry.Lines.FCCurrency = p_strMoneda
            End If

            oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Unid", i).Trim

            oJournalEntry.Lines.Reference1 = p_strRef1
            oJournalEntry.Lines.Reference2 = p_strRef2

            If p_blnUsaDimensiones Then
                Dim strCodigoMarca As String = p_oForm.DataSources.DBDataSources.Item("@SCGD_USADOXCONT").GetValue("U_Cod_Marca_Us", i).Trim
                p_oDatatableDimensionesOT = (p_ClsLineasDocumentos.DatatableDimensionesContables(p_oForm, strTipo, strCodigoMarca, p_oDatatableDimensionesOT))
                If p_oDatatableDimensionesOT.Rows.Count <> 0 Then
                    p_ClsLineasDocumentos.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, p_oDatatableDimensionesOT)

                End If
            End If
            oJournalEntry.Lines.Add()
        Next


        If oJournalEntry.Add <> 0 Then
            p_Company.GetLastError(intError, strMensajeError)
            Throw New ExceptionsSBO(intError, strMensajeError)
        Else
            p_Company.GetNewObjectCode(strNoAsiento)
        End If
        Return CInt(strNoAsiento)

    End Function

    'Agregado 22/11/2011: Metodo General para Imprimir Reportes DMS Interno
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strDireccionReporte"></param>
    ''' <param name="p_strBarraTitulo"></param>
    ''' <param name="p_strParametros"></param>
    ''' <param name="p_strUsuarioBD"></param>
    ''' <param name="p_strContraseñaBD"></param>
    ''' <param name="p_strBaseDatos"></param>
    ''' <param name="p_strServidor"></param>
    ''' <remarks></remarks>
    Shared Sub ImprimirReporte(ByVal p_strDireccionReporte As String, ByVal p_strBarraTitulo As String, ByVal p_strParametros As String, _
                               ByVal p_strUsuarioBD As String, ByVal p_strContraseñaBD As String, ByVal p_strBaseDatos As String, ByVal p_strServidor As String)

        Dim strPathExe As String

        Try

            p_strBarraTitulo = p_strBarraTitulo.Replace(" ", "°")

            p_strDireccionReporte = p_strDireccionReporte.Replace(" ", "°")

            p_strServidor = p_strServidor.Replace(" ", "°")

            p_strBaseDatos = p_strBaseDatos.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= p_strBarraTitulo & " " & p_strDireccionReporte & " " & p_strUsuarioBD & "," & p_strContraseñaBD & "," & p_strServidor & "," & p_strBaseDatos & " " & p_strParametros

            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_Company"></param>
    ''' <param name="p_strCuenta"></param>
    ''' <param name="p_strContraCuenta"></param>
    ''' <param name="p_strCodigoTransaccion"></param>
    ''' <param name="p_strComentarios"></param>
    ''' <param name="p_decMontoAsiento"></param>
    ''' <param name="p_datFechaRegisto"></param>
    ''' <param name="p_strTransaccion1"></param>
    ''' <param name="p_strTransaccion2"></param>
    ''' <param name="p_blnUsaFecha"></param>
    ''' <param name="p_blnTipo"></param>
    ''' <param name="p_strMoneda"></param>
    ''' <param name="p_strTransaccionLineas"></param>
    ''' <param name="p_strRef1"></param>
    ''' <param name="p_strRef2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Shared Function CrearAsiento(ByVal p_Company As SAPbobsCOM.Company, _
                                 ByVal p_strCuenta As String, _
                                 ByVal p_strContraCuenta As String, _
                                 ByVal p_strCodigoTransaccion As String, _
                                 ByVal p_strComentarios As String, _
                                 ByVal p_decMontoAsiento As Decimal, _
                                 ByVal p_datFechaRegisto As Date, _
                                 ByVal p_strTransaccion1 As String, _
                                 ByVal p_strTransaccion2 As String, _
                                 ByVal p_blnUsaFecha As Boolean, _
                                 ByVal p_blnTipo As Boolean, _
                                 ByVal p_strMoneda As String, _
                                 ByVal p_strTransaccionLineas As String, _
                                 Optional ByVal p_strRef1 As String = "", _
                                 Optional ByVal p_strRef2 As String = "") As Integer

        Dim oJournalEntry As JournalEntries
        Dim strMonedaLocal As String
        Dim strMonedaSistema As String

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim strNoAsiento As String

        strNoAsiento = 0

        DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)

        oJournalEntry = p_Company.GetBusinessObject(BoObjectTypes.oJournalEntries)

        If p_blnUsaFecha Then
            oJournalEntry.ReferenceDate = p_datFechaRegisto
        End If
        oJournalEntry.TransactionCode = p_strCodigoTransaccion
        If p_strComentarios.Length() > 50 Then
            oJournalEntry.Memo &= p_strComentarios.Substring(0, 50)
        Else
            oJournalEntry.Memo &= p_strComentarios
        End If

        'Cuenta
        oJournalEntry.Lines.AccountCode = p_strCuenta

        If p_blnTipo Then
            If strMonedaLocal = p_strMoneda Then
                oJournalEntry.Lines.Debit = p_decMontoAsiento
            Else
                oJournalEntry.Lines.FCDebit = p_decMontoAsiento
                oJournalEntry.Lines.FCCurrency = p_strMoneda

            End If
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_strTransaccion1

        Else
            If strMonedaLocal = p_strMoneda Then
                oJournalEntry.Lines.Credit = p_decMontoAsiento
            Else
                oJournalEntry.Lines.FCCredit = p_decMontoAsiento
                oJournalEntry.Lines.FCCurrency = p_strMoneda
            End If
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_strTransaccion1
        End If
        oJournalEntry.Lines.Reference1 = p_strRef1
        oJournalEntry.Lines.Reference2 = p_strRef2
        oJournalEntry.Lines.VatLine = BoYesNoEnum.tNO

        oJournalEntry.Lines.Add()
        oJournalEntry.Lines.AccountCode = p_strContraCuenta
        oJournalEntry.Lines.VatLine = BoYesNoEnum.tNO
        If Not p_blnTipo Then
            If strMonedaLocal = p_strMoneda Then
                oJournalEntry.Lines.Debit = p_decMontoAsiento
            Else
                oJournalEntry.Lines.FCDebit = p_decMontoAsiento
                oJournalEntry.Lines.FCCurrency = p_strMoneda

            End If
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_strTransaccion2
        Else
            If strMonedaLocal = p_strMoneda Then
                oJournalEntry.Lines.Credit = p_decMontoAsiento
            Else
                oJournalEntry.Lines.FCCredit = p_decMontoAsiento
                oJournalEntry.Lines.FCCurrency = p_strMoneda
            End If
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strTransaccionAsientos).Value = p_strTransaccionLineas
            oJournalEntry.Lines.UserFields.Fields.Item(mc_strCodUnidadAsientos).Value = p_strTransaccion2
        End If
        oJournalEntry.Lines.Reference1 = p_strRef1
        oJournalEntry.Lines.Reference2 = p_strRef2

        If oJournalEntry.Add <> 0 Then
            p_Company.GetLastError(intError, strMensajeError)
            Throw New ExceptionsSBO(intError, strMensajeError)
        Else
            p_Company.GetNewObjectCode(strNoAsiento)
        End If
        Return CInt(strNoAsiento)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="blnEstado"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Shared Sub FormularioSoloLectura(ByVal oForm As Form, ByVal blnEstado As Boolean)

        Dim oItem As Item

        For Each oItem In oForm.Items
            oItem.AffectsFormMode = blnEstado
        Next

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SBO_Application"></param>
    ''' <param name="p_blnUsaConfigTallerInterno"></param>
    ''' <param name="p_IdSucusal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Shared Function DevuelveConversionUnidadesTiempo(ByVal SBO_Application As Application, _
                                                            Optional ByVal p_blnUsaConfigTallerInterno As Boolean = False, _
                                                            Optional ByVal p_IdSucusal As String = "") As Double

        Dim strConectionString As String = ""
        Dim dblValorRetorno As Double

        DevuelveCadenaConexionBDTaller(SBO_Application, p_IdSucusal, strConectionString)
        Dim cmdCommand As New SqlCommand
        Dim cnnConnection As New SqlConnection(strConectionString)

        Try

            If Not p_blnUsaConfigTallerInterno Then
                DevuelveCadenaConexionBDTaller(SBO_Application, p_IdSucusal, strConectionString)

                If Not String.IsNullOrEmpty(strConectionString) Then
                    If cnnConnection.State = ConnectionState.Closed Then
                        cnnConnection.Open()
                    End If
                    cmdCommand.Connection = cnnConnection
                    cmdCommand.CommandType = CommandType.Text

                    cmdCommand.CommandText = "declare @UnidTiempo as numeric(4,0) " & _
                    "set @UnidTiempo = (select case valor when '' then '0' else valor end as Valor from SCGTA_TB_Configuracion " & _
                    "where propiedad = 'UnidadTiempo')" & _
                    "select TiempoMinutosUnidadTiempo from SCGTA_TB_UnidadTiempo where codigounidadtiempo = @UnidTiempo"

                    dblValorRetorno = cmdCommand.ExecuteScalar
                Else
                    Return dblValorRetorno
                End If

                If cnnConnection.State = ConnectionState.Open Then
                    cmdCommand.Dispose()
                    cnnConnection.Close()
                End If

            Else
                dblValorRetorno = 0
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Where(Function(fSucu) fSucu.U_Sucurs.Trim() = p_IdSucusal.Trim()).Count > 0 Then
                    dblValorRetorno = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(fSucu) fSucu.U_Sucurs.Trim() = p_IdSucusal.Trim()).U_UniTpMint
                End If
                Return dblValorRetorno
            End If
        Catch ex As Exception
            Throw
        End Try



        Return dblValorRetorno

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="blnEstado"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Shared Sub FormularioDeshabilitado(ByVal oForm As Form, ByVal blnEstado As Boolean)

        Dim oItem As Item

        For Each oItem In oForm.Items
            If oItem.Type = BoFormItemTypes.it_COMBO_BOX Or _
            oItem.Type = BoFormItemTypes.it_CHECK_BOX Or _
            oItem.Type = BoFormItemTypes.it_EDIT Or _
            oItem.Type = BoFormItemTypes.it_EXTEDIT Then
                Try
                    oItem.Enabled = blnEstado
                Catch ex As COMException
                    If ex.ErrorCode <> -7022 Then Throw
                End Try

            End If
        Next

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strServerName"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_intIdioma"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MenusPlandeVentas(ByVal p_strServerName As String, ByVal p_strDatabaseName As String, ByVal p_intIdioma As Integer) As Dictionary(Of String, MenusPlanVentas)
        Dim udoMenus As New Dictionary(Of String, MenusPlanVentas)
        Try
            For Each admin9 As Admin9 In DMS_Connector.Configuracion.ParamGenAddon.Admin9.OrderBy(Function(fAdmin9) fAdmin9.U_Prio)
                With admin9
                    udoMenus.Add(admin9.U_Codigo, New MenusPlanVentas() With {
                    .strCodigo = IIf(Not String.IsNullOrEmpty(admin9.U_Codigo), admin9.U_Codigo, ""),
                    .strMenu = IIf(Not String.IsNullOrEmpty(admin9.U_Name), admin9.U_Name, ""),
                    .intNivel = IIf(Not String.IsNullOrEmpty(admin9.U_Prio), admin9.U_Prio, ""),
                    .strEstado = IIf(Not String.IsNullOrEmpty(admin9.U_Estado), admin9.U_Estado, ""),
                    .blnPorEmpleado = (Not String.IsNullOrEmpty(admin9.U_PEmp) AndAlso admin9.U_PEmp.Trim = "Y"),
                    .blnUsaMenu = (Not String.IsNullOrEmpty(admin9.U_UMenu) AndAlso admin9.U_UMenu.Trim = "Y")
                })
                End With
            Next
        Catch ex As Exception

        End Try

        Return udoMenus
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strIDForm"></param>
    ''' <param name="p_strIDTextBox"></param>
    ''' <param name="sbo_applicaction"></param>
    ''' <param name="TextAnterior"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Shared Sub Fecha(ByVal p_strIDForm As String, _
                            ByVal p_strIDTextBox As String, _
                            ByRef sbo_applicaction As Application, _
                            ByRef TextAnterior As String)

        Dim oText As EditText
        Dim strFechaOriginal As String
        Dim strFechaFinal As String
        oText = DirectCast(sbo_applicaction.Forms.Item(p_strIDForm).Items.Item(p_strIDTextBox).Specific, EditText)
        strFechaOriginal = oText.String

        If Not IsDate(strFechaOriginal) Then
            strFechaFinal = Now.ToShortDateString
            TextAnterior = p_strIDTextBox
            oText.String = strFechaFinal
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oValidValues"></param>
    ''' <param name="strQuery"></param>
    ''' <param name="p_AddBlankValue"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Overloads Shared Sub CargarValidValuesEnCombos(ByRef oValidValues As SAPbouiCOM.ValidValues, ByVal strQuery As String, Optional ByVal p_AddBlankValue As Boolean = False)
        Try
            'Borra los ValidValues
            While oValidValues.Count > 0
                oValidValues.Remove(oValidValues.Item(0).Value, BoSearchKey.psk_ByValue)
            End While
            If p_AddBlankValue Then
                oValidValues.Add(" ", " ")
            End If
            For Each dRow As DataRow In EjecutarConsultaDataTable(strQuery).Rows
                If dRow.Item(0) IsNot DBNull.Value AndAlso dRow.Item(1) IsNot DBNull.Value Then
                    If dRow.Item(1).ToString.Trim.Length > 60 Then
                        oValidValues.Add(dRow.Item(0).ToString.Trim, dRow.Item(1).ToString.Trim.Substring(0, 60))
                    Else
                        oValidValues.Add(dRow.Item(0).ToString.Trim, dRow.Item(1).ToString.Trim)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oValidValues"></param>
    ''' <param name="p_lstListaValores"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Overloads Shared Sub CargarValidValuesEnCombos(ByRef oValidValues As SAPbouiCOM.ValidValues, _
                                                               ByVal p_lstListaValores As List(Of ListadoValidValues))
        Try
            'Borra los ValidValues
            While oValidValues.Count > 0
                oValidValues.Remove(oValidValues.Item(0).Value, BoSearchKey.psk_ByValue)
            End While
            ''Agrega los ValidValues
            For Each oValidValue As ListadoValidValues In p_lstListaValores
                If oValidValue.strName.Trim.Length > 60 Then
                    oValidValues.Add(oValidValue.strCode, oValidValue.strName.Trim.Substring(0, 60))
                Else
                    oValidValues.Add(oValidValue.strCode, oValidValue.strName.Trim)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SBO_Application"></param>
    ''' <param name="p_oCompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtieneFormatoFecha(ByVal SBO_Application As Application, ByVal p_oCompany As SAPbobsCOM.Company) As String
        Dim separador As String
        Dim formato As String
        separador = DMS_Connector.Company.AdminInfo.DateSeparator
        Select Case DMS_Connector.Company.AdminInfo.DateTemplate
            Case BoDateTemplate.dt_DDMMYY
                formato = String.Format("dd{0}MM{0}yy", separador)
            Case BoDateTemplate.dt_DDMMCCYY
                formato = String.Format("dd{0}MM{0}yyyy", separador)
            Case BoDateTemplate.dt_MMDDYY
                formato = String.Format("MM{0}dd{0}yy", separador)
            Case BoDateTemplate.dt_MMDDCCYY
                formato = String.Format("MM{0}dd{0}yyyy", separador)
            Case BoDateTemplate.dt_CCYYMMDD
                formato = String.Format("yyyy{0}MM{0}dd", separador)
            Case BoDateTemplate.dt_DDMonthYYYY
                formato = String.Format("dd{0}MMMM{0}yy", separador)
            Case Else
                Throw New InvalidOperationException("El formato de la fecha especificada para la compañia no es válido.")
        End Select
        Return formato
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="company"></param>
    ''' <param name="nombreTabla"></param>
    ''' <param name="cadenaConexion"></param>
    ''' <param name="lineNum"></param>
    ''' <param name="itemCode"></param>
    ''' <param name="docEntry"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtieneVisOrder(ByVal company As SAPbobsCOM.Company, ByVal nombreTabla As String, ByVal cadenaConexion As String, ByVal lineNum As Integer, ByVal itemCode As String, ByVal docEntry As Integer) As Integer
        Dim visOrder As Nullable(Of Integer)
        Try
            visOrder = EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGetVisOrder"), nombreTabla, lineNum, itemCode, docEntry))
            If visOrder.HasValue Then Return visOrder.Value
            Throw New ApplicationException("Error obteniendo visorder")
        Catch
            Throw
        End Try

    End Function

    '***************************************ObtieneLineNumErroneo**********************************************************************

    ''' <summary>
    ''' devuelve el valor de la propiedad en la tabla de configuracion
    ''' </summary>
    ''' <param name="dtbConfiguracion"></param>
    ''' <param name="strPropiedad"></param>
    ''' <param name="strValor"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DevuelveValorDeParametosConfiguracion(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                    ByVal strPropiedad As String, _
                                                                    ByRef strValor As String) As Boolean

        Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

        Try

            drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
            strValor = ""
            If Not drwConfiguracion Is Nothing _
               AndAlso drwConfiguracion.Valor <> "" Then
                If drwConfiguracion.Valor = 1 Then
                    Return True
                Else
                    Return False
                End If
                'strValor = drwConfiguracion.Valor
            Else
                Return False
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strConsulta"></param>
    ''' <param name="p_strDatabaseName"></param>
    ''' <param name="p_strServerName"></param>
    ''' <param name="cn"></param>
    ''' <param name="tran"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function EjecutarConsultaCodigos(ByRef p_strConsulta As String, _
                                     ByRef p_strDatabaseName As String, _
                                     ByRef p_strServerName As String, ByVal cn As SqlClient.SqlConnection, _
                                     ByRef tran As SqlClient.SqlTransaction) As String

        Dim drdResultadoConsulta As SqlClient.SqlDataReader = Nothing
        Dim cmdEjecutar As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strValor As String = ""
        '        Dim m_adpAct As SqlClient.SqlDataAdapter

        Try
            If cn.State = ConnectionState.Open Then
                cn_Coneccion = cn

            Else
                cn_Coneccion = cn
                cn_Coneccion.Open()
            End If

            cmdEjecutar.Connection = cn_Coneccion
            'cmdEjecutarConsulta.Transaction = tran
            cmdEjecutar.CommandType = CommandType.Text
            cmdEjecutar.CommandText = p_strConsulta
            drdResultadoConsulta = cmdEjecutar.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                    strValor = drdResultadoConsulta.Item(0)
                    Exit Do
                End If
            Loop
        Catch
            Throw
        Finally
            If Not drdResultadoConsulta Is Nothing Then drdResultadoConsulta.Close()
            cmdEjecutar.Connection.Close()
            cn_Coneccion.Close()
        End Try
        Return strValor

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNumberFomatInfo(ByVal oCompany As SAPbobsCOM.Company) As NumberFormatInfo

        Dim n As NumberFormatInfo = New NumberFormatInfo
        n.CurrencyDecimalSeparator = DMS_Connector.Company.AdminInfo.DecimalSeparator
        n.CurrencyGroupSeparator = DMS_Connector.Company.AdminInfo.ThousandsSeparator
        n.CurrencyDecimalDigits = DMS_Connector.Company.AdminInfo.PriceAccuracy
        n.NumberDecimalDigits = DMS_Connector.Company.AdminInfo.AccuracyofQuantities
        Return n

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <param name="sboApplication"></param>
    ''' <remarks></remarks>
    'Shared Sub ManejadorErrores(ByVal ex As Exception, ByVal sboApplication As SAPbouiCOM.Application)
    '    'SCG.ServicioPostVenta.Utilitarios.OnError(ex, sboApplication)

    '    Dim cultura As String = CargarCulturaActual()
    '    Dim tipoSkin As Integer = CargarTipoSkin()
    '    Dim MyProcs() As Process
    '    MyProcs = Process.GetProcessesByName("SAP Business One")
    '    Dim currentProcess As Process = Process.GetCurrentProcess()
    '    Dim MyWindow As WindowWrapper = Nothing

    '    If MyProcs.Length <> 0 Then
    '        For i As Integer = 0 To MyProcs.Length - 1
    '            If MyProcs(i).SessionId = currentProcess.SessionId Then
    '                MyWindow = New WindowWrapper(MyProcs(i).MainWindowHandle)
    '                Exit For
    '            End If
    '        Next
    '    End If
    '    Dim m As MuestraMessgeBoxSBO = New MuestraMessgeBoxSBO(sboApplication)
    '    If MyWindow IsNot Nothing Then ManejoErrores(ex, sboApplication.Company.Name, cultura, MyWindow, AddressOf m.MessageBxPreg, AddressOf m.MessageBxExc, tipoSkin)
    'End Sub

    Public Shared Sub ManejadorErrores(ByVal ex As Exception, ByVal sboApplication As SAPbouiCOM.Application)
        DMS_Connector.Helpers.ManejoErrores(ex)
    End Sub

    ''' <summary>
    ''' Carga el tipo de skin de SAP
    ''' </summary>
    ''' <param name="sboApplication"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CargarTipoSkin() As Integer
        Return CInt(DMS_Connector.Company.ApplicationSBO.SkinStyle)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CargarCulturaActual() As String

        DMS_Connector.Helpers.SetCulture(Threading.Thread.CurrentThread.CurrentUICulture, Threading.Thread.CurrentThread.CurrentUICulture)

        Return Threading.Thread.CurrentThread.CurrentUICulture.Name

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="unidad"></param>
    ''' <param name="compañiaBD"></param>
    ''' <param name="servidorCompañia"></param>
    ''' <param name="strMonedaSistema"></param>
    ''' <param name="strMonedaLocal"></param>
    ''' <param name="blnUtilizaCosteoAccesorios"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ConsultaCosteos(ByVal unidad As String, ByVal compañiaBD As String, ByVal servidorCompañia As String, _
                                    ByVal strMonedaSistema As String, ByVal strMonedaLocal As String, Optional ByVal blnUtilizaCosteoAccesorios As String = "") As Boolean

        Dim tipoDoc As String
        Dim query As String
        Dim strFecha As String

        If blnUtilizaCosteoAccesorios = "Y" Then
            tipoDoc = strTipoDocumentoArticulo
        Else
            tipoDoc = Nothing
        End If

        strFecha = RetornaFechaFormatoDB(Now.Date.ToString(), servidorCompañia)
        strFecha = strFecha & " 23:59:59"

        query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosVehi"), unidad)
        If EjecutarConsulta(query) = 0 Then
            query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosAsientos"), unidad, strMonedaSistema, strMonedaLocal, strFecha)
            If EjecutarConsulta(query) = 0 Then
                query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosAsientos2"), unidad, strMonedaSistema, strMonedaLocal, strFecha)
                If EjecutarConsulta(query) = 0 Then
                    query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosFacRes"), unidad, strFecha, strTipoDocumentoServicio, tipoDoc)
                    If EjecutarConsulta(query) = 0 Then
                        query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosNotCre"), unidad, strFecha, strTipoDocumentoServicio, tipoDoc)
                        If EjecutarConsulta(query) = 0 Then
                            query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosFacCli"), unidad, strFecha, strTipoDocumentoServicio)
                            If EjecutarConsulta(query) = 0 Then
                                query = String.Format(DMS_Connector.Queries.GetStrQueryFormat("strConsultaCosteosSalMer"), unidad, strFecha)
                                If EjecutarConsulta(query) = 0 Then
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return True
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_Company"></param>
    ''' <param name="p_oApplication"></param>
    ''' <param name="p_strIdSuc"></param>
    ''' <remarks></remarks>
    Shared Sub EnviarMensajeMovimientoAccs(ByVal p_Company As SAPbobsCOM.Company, ByVal p_oApplication As SAPbouiCOM.Application, ByVal p_strIdSuc As String)

        Dim oMsg As Messages
        Dim strMensaje As String
        Dim intResultado As Integer
        Dim strError As String = String.Empty

        Dim strEncargadoAccesorios As String = ""
        Dim strArregloEncargados() As String
        Dim intIndicearreglo As Integer

        Dim intindiceUsuarios As Integer

        Dim adpConf As ConfiguracionDataAdapter
        Dim g_dstConfiguracion As New ConfiguracionDataSet

        Try
            Dim strConexion As String = String.Empty

            If Not ValidarOTInternaConfiguracion(p_Company) Then
                DevuelveCadenaConexionBDTaller(p_oApplication, strConexion)
                adpConf = New ConfiguracionDataAdapter(strConexion)
                adpConf.Fill(g_dstConfiguracion)

                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "EncargadoAccesorios", strEncargadoAccesorios)

            Else
                strEncargadoAccesorios = DevuelveUsuariosMensajeria(p_Company, 1, p_strIdSuc)

            End If

            If Not String.IsNullOrEmpty(strEncargadoAccesorios) Then

                strArregloEncargados = Split(strEncargadoAccesorios, ",")

                For intIndicearreglo = 0 To strArregloEncargados.Length - 1

                    strArregloEncargados(intIndicearreglo) = Trim(strArregloEncargados(intIndicearreglo))

                Next intIndicearreglo

                strMensaje = My.Resources.Resource.MovimientoMercancia

                oMsg = p_Company.GetBusinessObject(BoObjectTypes.oMessages)
                oMsg.Priority = BoMsgPriorities.pr_High
                oMsg.MessageText = strMensaje
                oMsg.Subject = My.Resources.Resource.DocumentoPreliminarCreado

                For intindiceUsuarios = 0 To strArregloEncargados.Length - 1

                    oMsg.Recipients.Add()
                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                    oMsg.Recipients.UserCode = Trim(strArregloEncargados(intindiceUsuarios))
                    oMsg.Recipients.NameTo = Trim(strArregloEncargados(intindiceUsuarios))
                    oMsg.Recipients.SendInternal = BoYesNoEnum.tYES

                Next intindiceUsuarios

                oMsg.AddDataColumn(My.Resources.Resource.DocumentoPreliminar, My.Resources.Resource.MovimientoMercancia, BoObjectTypes.oDrafts, m_strDocumentoMensaje)
                intResultado = oMsg.Add

                If (intResultado <> 0) Then
                    p_Company.GetLastError(intResultado, strError)
                    Throw New ExceptionsSBO(intResultado, strError)
                End If

            End If

        Catch ex As Exception

            Call ManejadorErrores(ex, p_oApplication)

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="company"></param>
    ''' <param name="p_intRol"></param>
    ''' <param name="p_strIdSuc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DevuelveUsuariosMensajeria(ByVal company As SAPbobsCOM.Company, ByVal p_intRol As Integer, ByVal p_strIdSuc As String) As String
        Dim strUsuarios As String = String.Empty
        If DMS_Connector.Configuracion.ConfMensajeria.Where(Function(cMensajeria) cMensajeria.U_IdRol = CStr(p_intRol) AndAlso cMensajeria.U_IdSuc.Trim = p_strIdSuc.Trim()).Count > 0 Then
            For Each mensajeria As DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria.Mensajeria In DMS_Connector.Configuracion.ConfMensajeria.Where(Function(cMensajeria) cMensajeria.U_IdRol = CStr(p_intRol) AndAlso cMensajeria.U_IdSuc.Trim = p_strIdSuc.Trim())
                For Each mensajeriaLineas As DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria.Mensajeria_Lineas In mensajeria.Mensajeria_Lineas
                    strUsuarios = String.Format("{0}{1},", strUsuarios, mensajeriaLineas.U_Usr_UsrName.Trim)
                Next
            Next
        End If
        Return strUsuarios.TrimEnd(",")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="company"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function UsaInterfazFord(ByVal company As SAPbobsCOM.Company) As Boolean
        Dim m_strConfOT As String
        m_strConfOT = DMS_Connector.Configuracion.ParamGenAddon.U_Usa_IFord.Trim
        If Not String.IsNullOrEmpty(m_strConfOT) Then
            If m_strConfOT = "Y" Then
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="company"></param>
    ''' <param name="cardCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ValidaIFTipoSN(ByVal company As SAPbobsCOM.Company, ByVal cardCode As String) As Boolean
        If Not String.IsNullOrEmpty(DevuelveValorSN(Nothing, cardCode, "U_SCGD_CusType")) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Valida DT
    ''' </summary>
    ''' <param name="p_form"></param>
    ''' <param name="strDtName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ValidaExisteDataTable(ByRef p_form As Form, ByVal strDtName As String) As Boolean
        Dim ExisteDataTable As Boolean = False
        If p_form.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To p_form.DataSources.DataTables.Count - 1
                If p_form.DataSources.DataTables.Item(i).UniqueID = strDtName Then
                    ExisteDataTable = True
                End If
            Next
        End If
        Return ExisteDataTable
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <param name="strMonedaBD"></param>
    ''' <param name="p_montoDocumento"></param>
    ''' <param name="strMonedaContrato"></param>
    ''' <param name="strTipoCambioMoneda"></param>
    ''' <param name="dtFecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function CalcularCostosPorCambioMoneda(ByVal p_ocompany As SAPbobsCOM.Company, ByVal strMonedaBD As String, ByVal p_montoDocumento As Decimal, _
                                                  ByVal strMonedaContrato As String, ByVal strTipoCambioMoneda As String, ByVal dtFecha As String) As Decimal

        Dim n As NumberFormatInfo
        Dim m_strMonedaLocal As String
        Dim m_strMonedaSistema As String
        Dim decTipoCambioOrigen As Decimal
        Dim valor As Decimal
        Dim decTipoCambioDestino As Decimal

        DMS_Connector.Helpers.GetCurrencies(m_strMonedaLocal, m_strMonedaSistema)
        n = GetNumberFomatInfo(p_ocompany)

        If Not String.IsNullOrEmpty(strMonedaContrato) AndAlso strMonedaContrato <> m_strMonedaLocal Then
            decTipoCambioOrigen = CDec(DMS_Connector.Helpers.GetCurrencyRate(strMonedaContrato, CDate(dtFecha)))
        Else
            decTipoCambioOrigen = 1
        End If

        If decTipoCambioOrigen = 0 Or decTipoCambioOrigen = -1 Then
            decTipoCambioOrigen = 1
        End If

        If Not String.IsNullOrEmpty(strMonedaBD) AndAlso strMonedaBD <> m_strMonedaLocal Then
            decTipoCambioDestino = DMS_Connector.Helpers.GetCurrencyRate(strMonedaBD, CDate(dtFecha))
        Else
            decTipoCambioDestino = 1
        End If

        If decTipoCambioDestino = 0 Or decTipoCambioDestino = -1 Then
            decTipoCambioDestino = 1
        End If

        If Not String.IsNullOrEmpty(strMonedaBD) Then
            If Trim(strMonedaBD) = Trim(strMonedaContrato) Then
                Return p_montoDocumento
            ElseIf Trim(strMonedaBD) <> Trim(strMonedaContrato) Then
                If Trim(strMonedaContrato) = m_strMonedaLocal Then
                    p_montoDocumento = p_montoDocumento / decTipoCambioDestino
                    valor = FormatNumber(p_montoDocumento, n.NumberDecimalDigits)
                    Return CDec(valor)
                ElseIf Trim(strMonedaBD) = m_strMonedaLocal Then
                    p_montoDocumento = p_montoDocumento * decTipoCambioOrigen
                    valor = FormatNumber(p_montoDocumento, n.NumberDecimalDigits)
                    Return CDec(valor)
                End If
            End If
        Else
            Return p_montoDocumento
        End If

    End Function

    ''' <summary>
    ''' Agregado 05/09/2012: Manejo de Códigos de Indicadores
    ''' </summary>
    ''' <param name="_application"></param>
    ''' <param name="LineId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Shared Function DevuelveCodIndicadores(ByVal _application As Application, ByVal LineId As String) As String
        Dim strIndicador As String
        Try
            strIndicador = String.Empty
            If DMS_Connector.Configuracion.ParamGenAddon.Admin8.Any(Function(admin8) admin8.LineId = LineId) Then
                strIndicador = DMS_Connector.Configuracion.ParamGenAddon.Admin8.FirstOrDefault(Function(admin8) admin8.LineId = LineId).U_Cod_Ind.Trim()
            End If
            Return strIndicador
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FechaSinFormato"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function RetornaFechaFormatoRegional(ByVal FechaSinFormato As String) As String

        'Obtengo la Formato de fecha y el separador de la configuracion global de la maquina
        Dim FormatoFecha As String = ""
        Dim SeparadorFecha As String = ""
        Dim dt As Date

        FormatoFecha = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
        SeparadorFecha = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator

        If Not String.IsNullOrEmpty(FormatoFecha) _
            And Not String.IsNullOrEmpty(SeparadorFecha) _
            And Not String.IsNullOrEmpty(FechaSinFormato) Then

            'doy formato a fecha el string 
            dt = CDate(FechaSinFormato)
            'fecha a retornar formateada
            Dim dtFechaFormateada As Date
            'convierto el string a fecha ya formateada
            dtFechaFormateada = Date.ParseExact(dt, FormatoFecha, Nothing)
            'la formateo de modo yyyyMMdd
            dtFechaFormateada = New Date(dtFechaFormateada.Year, dtFechaFormateada.Month, dtFechaFormateada.Day)

            Dim strFechaFormateada As String = ""
            strFechaFormateada = String.Format(dtFechaFormateada.Year & "{0}" & dtFechaFormateada.Month & "{0}" & dtFechaFormateada.Day, SeparadorFecha)

            'retorno fecha formateada
            Return strFechaFormateada
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NombreServidor"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function RetornaFormatoFechaDB(ByVal NombreServidor As String) As String
        Dim dtUserOptions As System.Data.DataTable
        Const strConsultaFormatoSQL As String = "dbcc useroptions"

        dtUserOptions = EjecutarConsultaDataTable(strConsultaFormatoSQL, "master", NombreServidor)

        Dim dateformat As String = dtUserOptions.Rows.Item(2).Item(1).ToString()
        Dim SeparadorFecha As String = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator

        Dim strDia As String = "dd"
        Dim strMes As String = "MM"
        Dim strAno As String = "yyyy"

        Select Case dateformat
            Case "dmy"

                dateformat = String.Format(strDia & "{0}" & strMes & "{0}" & strAno, SeparadorFecha)

            Case "dym"

                dateformat = String.Format(strDia & "{0}" & strAno & "{0}" & strMes, SeparadorFecha)

            Case "mdy"

                dateformat = String.Format(strMes & "{0}" & strDia & "{0}" & strAno, SeparadorFecha)

            Case "myd"

                dateformat = String.Format(strMes & "{0}" & strAno & "{0}" & strDia, SeparadorFecha)

            Case "ymd"

                dateformat = String.Format(strAno & "{0}" & strMes & "{0}" & strDia, SeparadorFecha)

            Case "ydm"

                dateformat = String.Format(strAno & "{0}" & strDia & "{0}" & strMes, SeparadorFecha)

        End Select

        Return dateformat

    End Function

    ''' <summary>
    ''' Obtiene formato en BD y retorna un string con la fecha formateada
    ''' </summary>
    ''' <param name="dtFecha">Fecha obtenida de interfaz</param>
    ''' <param name="NombreServidor">Servidor de BD</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function RetornaFechaFormatoDB(ByVal dtFecha As Date, ByVal NombreServidor As String, Optional ByVal usaHora As Boolean = False) As String

        'Obtengo la Formato de fecha y el separador de la configuracion global de la maquina
        Dim SeparadorFecha As String
        Dim SeparadorHora As String
        'fecha a retornar formateada
        Dim strFechaFormateada As String = String.Empty
        Dim FormatoServer As String
        Dim dtUserOptions As Data.DataTable

        'Const strConsultaFormatoSQL As String = "select dateformat from syslanguages where langid = (select value from master..sysconfigures where comment = 'default language')"
        Const strConsultaFormatoSQL As String = "dbcc useroptions"
        Dim strDia As String
        Dim strMes As String
        Dim strAno As String
        Dim strHora As String
        Dim strMinutos As String
        Dim strSeg As String

        SeparadorFecha = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator
        SeparadorHora = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator

        If Not String.IsNullOrEmpty(NombreServidor) _
            And Not String.IsNullOrEmpty(SeparadorFecha) _
            And Not String.IsNullOrEmpty(dtFecha) Then

            strMes = String.Format("{0:D2}", dtFecha.Month)
            strDia = String.Format("{0:D2}", dtFecha.Day)
            strAno = dtFecha.Year.ToString()

            strHora = String.Format("{0:D2}", dtFecha.Hour)
            strMinutos = String.Format("{0:D2}", dtFecha.Minute)
            strSeg = String.Format("{0:D2}", dtFecha.Second)


            dtUserOptions = EjecutarConsultaDataTable(strConsultaFormatoSQL, "master", NombreServidor)

            FormatoServer = dtUserOptions.Rows.Item(2).Item(1).ToString()

            Select Case FormatoServer
                Case "dmy"
                    strFechaFormateada = String.Format(strDia & "{0}" & strMes & "{0}" & strAno, SeparadorFecha)
                Case "dym"
                    strFechaFormateada = String.Format(strDia & "{0}" & strAno & "{0}" & strMes, SeparadorFecha)
                Case "mdy"
                    strFechaFormateada = String.Format(strMes & "{0}" & strDia & "{0}" & strAno, SeparadorFecha)
                Case "myd"
                    strFechaFormateada = String.Format(strMes & "{0}" & strAno & "{0}" & strDia, SeparadorFecha)
                Case "ymd"
                    strFechaFormateada = String.Format(strAno & "{0}" & strMes & "{0}" & strDia, SeparadorFecha)
                Case "ydm"
                    strFechaFormateada = String.Format(strAno & "{0}" & strDia & "{0}" & strMes, SeparadorFecha)
            End Select

            If usaHora = True Then
                strFechaFormateada = String.Format(strFechaFormateada & " " & strHora & "{0}" & strMinutos & "{0}" & strSeg, SeparadorHora)
            End If

            'retorno fecha formateada
            Return strFechaFormateada
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Verifica la campaña asociada a cada vehiculo
    ''' </summary>
    ''' <param name="p_strUnidad">numero unidad</param>
    ''' <param name="p_strPlaca">numero placa</param>
    ''' <param name="p_strVIN">numero VIN</param>
    ''' <param name="p_application">objeto SBOApplication</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VerificaCampanaPorUnidad(ByVal p_strUnidad As String, ByVal p_strVIN As String, ByVal p_application As Application, ByRef p_Multiples As Boolean, Optional ByRef p_NumCampana As String = "", Optional ByRef p_DescCampana As String = "") As String

        Dim m_strConsultaCampana As String = DMS_Connector.Queries.GetStrSpecificQuery("strVerificaCampanaPorUnidad")
        Dim dtCampana As New Data.DataTable
        Dim m_strMSJ As String = String.Empty
        Dim m_blnBandera As Boolean = False
        Dim m_contador As Integer = 0

        Try
            dtCampana.Clear()
            If String.IsNullOrEmpty(p_strUnidad) Then p_strUnidad = "-1"
            If String.IsNullOrEmpty(p_strVIN) Then p_strVIN = "-1"

            dtCampana = EjecutarConsultaDataTable(String.Format(m_strConsultaCampana, p_strUnidad, p_strVIN))
            For Each dr As DataRow In dtCampana.Rows
                p_application.MessageBox(My.Resources.Resource.LaUnidad + " " + p_strUnidad + ", " + My.Resources.Resource.PreguntaCampanaUnidad + dr.Item("CpnNo").ToString() + " " + dr.Item("Name").ToString(), 1, My.Resources.Resource.btnOk)
                m_strMSJ = m_strMSJ + dr.Item("CpnNo").ToString() + " - " + dr.Item("Name").ToString() + ", "
                p_NumCampana = dr.Item("CpnNo").ToString()
                p_DescCampana = dr.Item("Name").ToString()
                m_blnBandera = True
                m_contador = m_contador + 1
            Next
            If m_contador > 1 Then p_Multiples = True

            m_strMSJ = My.Resources.Resource.AsociadoCampana + m_strMSJ

            If m_blnBandera Then
                Return m_strMSJ
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_Hora"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function FormatoHora(ByVal p_Hora As String) As String
        Try
            Select Case p_Hora.Length
                Case 3
                    p_Hora = "0" & p_Hora
                    p_Hora = p_Hora.Insert(2, ":")
                Case 4
                    p_Hora = p_Hora.Insert(2, ":")
            End Select
            Return p_Hora


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_Hora"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function FormatoHora2(ByVal p_Hora As String) As String
        Try
            Select Case p_Hora.Length
                Case 3
                    p_Hora = "0" & p_Hora
                Case Else
                    p_Hora = p_Hora
            End Select
            Return p_Hora


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PrecioSinProcesar"></param>
    ''' <param name="strMonedaLocal"></param>
    ''' <param name="strMonedaSistema"></param>
    ''' <param name="strMonedaDoc2"></param>
    ''' <param name="strMonedaDoc1"></param>
    ''' <param name="strTipoCambioDoc1"></param>
    ''' <param name="dtFechaDoc1"></param>
    ''' <param name="n"></param>
    ''' <param name="m_oCompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ManejoMultimoneda(ByVal PrecioSinProcesar As Decimal, _
                                      ByVal strMonedaLocal As String, _
                                      ByVal strMonedaSistema As String, _
                                      ByVal strMonedaDoc2 As String, _
                                      ByVal strMonedaDoc1 As String, _
                                      ByVal strTipoCambioDoc1 As String, _
                                      ByVal dtFechaDoc1 As Date, _
                                      ByVal n As NumberFormatInfo, _
                                      ByVal m_oCompany As SAPbobsCOM.Company) As Decimal



        Dim rsTipoCambio As Recordset = Nothing
        Dim dcPrecioProcesado As Decimal = 0
        Dim dcTCDoc1 As Decimal = 0
        Dim strTipoCambioSistema As String = ""
        Dim dcTCMS As Decimal = 0
        Dim strTCME As String = 0
        Dim dcTCME As Decimal = 0

        Try

            If sbTipoCambio Is Nothing Then
                sbTipoCambio = m_oCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            End If
            rsTipoCambio = m_oCompany.GetBusinessObject(BoObjectTypes.BoRecordset)

            If strMonedaLocal <> strMonedaSistema Then
                strTipoCambioSistema = DMS_Connector.Helpers.GetCurrencyRate(strMonedaSistema, dtFechaDoc1)
            End If

            If String.IsNullOrEmpty(strTipoCambioSistema) Then strTipoCambioSistema = 1
            dcTCMS = Decimal.Parse(strTipoCambioSistema)

            If String.IsNullOrEmpty(strTipoCambioDoc1) Then strTipoCambioDoc1 = 1
            dcTCDoc1 = Convert.ToDecimal(strTipoCambioDoc1.ToString(n))

            Select Case strMonedaDoc1
                Case strMonedaLocal
                    Select Case strMonedaDoc2
                        Case strMonedaLocal, ""
                            dcPrecioProcesado = PrecioSinProcesar
                        Case strMonedaSistema
                            dcPrecioProcesado = PrecioSinProcesar * dcTCMS
                        Case Else
                            strTCME = DMS_Connector.Helpers.GetCurrencyRate(strMonedaDoc2, dtFechaDoc1)
                            If String.IsNullOrEmpty(strTCME) Then strTCME = 1
                            dcTCME = Decimal.Parse(strTCME)
                            dcPrecioProcesado = PrecioSinProcesar * dcTCME
                    End Select
                Case strMonedaSistema
                    Select Case strMonedaDoc2
                        Case strMonedaLocal, ""
                            dcPrecioProcesado = PrecioSinProcesar / dcTCDoc1
                        Case strMonedaSistema
                            dcPrecioProcesado = PrecioSinProcesar
                        Case Else
                            rsTipoCambio = sbTipoCambio.GetCurrencyRate(strMonedaDoc2, dtFechaDoc1)
                            strTCME = rsTipoCambio.Fields.Item(0).Value
                            If String.IsNullOrEmpty(strTCME) Then strTCME = 1
                            dcTCME = Decimal.Parse(strTCME)
                            dcPrecioProcesado = (PrecioSinProcesar * dcTCME) / dcTCDoc1
                    End Select
                Case Else
                    Select Case strMonedaDoc2
                        Case strMonedaLocal, ""
                            dcPrecioProcesado = PrecioSinProcesar / dcTCDoc1
                        Case strMonedaSistema
                            dcPrecioProcesado = (PrecioSinProcesar * dcTCMS) / dcTCDoc1
                        Case Else
                            dcPrecioProcesado = PrecioSinProcesar
                    End Select
            End Select
            Return dcPrecioProcesado

        Catch ex As Exception
            Throw ex
        Finally
            DestruirObjeto(rsTipoCambio)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_CompanyDB"></param>
    ''' <param name="p_Server"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function RetornaFechaActual(ByVal p_CompanyDB As String, ByVal p_Server As String) As DateTime
        Try
            Return DMS_Connector.Helpers.GetDBServerDate()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strCentroCosto"></param>
    ''' <param name="p_strTipoItem"></param>
    ''' <param name="intIdSucursal"></param>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBodegaXCentroCosto(ByVal p_strCentroCosto As String, _
                                                    ByVal p_strTipoItem As String, _
                                                    ByVal intIdSucursal As String, _
                                                    ByVal p_ocompany As Application) As String
        Dim bodegasCentro As Bodegas_CentroCosto
        Dim strBodegaReturn As String

        Try
            If Not String.IsNullOrEmpty(p_strCentroCosto) Then
                bodegasCentro = New Bodegas_CentroCosto() With {
                    .DocEntry = 0}
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(sucursal) sucursal.U_Sucurs.Trim.Equals(CStr(intIdSucursal))) Then
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(sucursal) sucursal.U_Sucurs.Trim.Equals(CStr(intIdSucursal))).Bodegas_CentroCosto.Any(Function(centroCosto) centroCosto.U_CC.Trim().Equals(p_strCentroCosto)) Then
                        bodegasCentro = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(sucursal) sucursal.U_Sucurs.Trim.Equals(CStr(intIdSucursal))).Bodegas_CentroCosto.FirstOrDefault(Function(centroCosto) centroCosto.U_CC.Trim().Equals(p_strCentroCosto))
                    End If
                End If
                If bodegasCentro.DocEntry <> 0 Then
                    Select Case p_strTipoItem
                        Case "BodegaRepuestos"
                            strBodegaReturn = bodegasCentro.U_Rep.Trim
                        Case "BodegaSuministros"
                            strBodegaReturn = bodegasCentro.U_Sum.Trim
                        Case "BodegaServicios"
                            strBodegaReturn = bodegasCentro.U_Ser.Trim
                        Case "BodegaServiciosExternos"
                            strBodegaReturn = bodegasCentro.U_SE.Trim
                        Case "BodegaProceso"
                            strBodegaReturn = bodegasCentro.U_Pro.Trim
                    End Select
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strBodegaReturn
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oCompany"></param>
    ''' <param name="strCardCode"></param>
    ''' <param name="strUDfName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DevuelveValorSN(ByVal p_oCompany As SAPbobsCOM.Company, _
                                              ByVal strCardCode As String, _
                                              ByVal strUDfName As String) As String
        Dim oBusinessPartners As BusinessPartners
        Dim valorUDF As String

        Try
            oBusinessPartners = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners)
            If oBusinessPartners.GetByKey(strCardCode) Then
                valorUDF = oBusinessPartners.UserFields.Fields.Item(strUDfName).Value
            End If
            Return valorUDF
        Catch ex As Exception
            Throw
        Finally
            DestruirObjeto(oBusinessPartners)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_oCompany"></param>
    ''' <param name="strItemcode"></param>
    ''' <param name="strUDfName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DevuelveValorItem(ByVal p_oCompany As SAPbobsCOM.Company, _
                                              ByVal strItemcode As String, _
                                              ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oItems)
        If oItemArticulo.GetByKey(strItemcode) Then
            valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value
        End If

        Return valorUDF

    End Function

    Public Shared Function DevuelveValorArticulo(ByVal strItemcode As String, _
                                           ByVal strUDfName As String) As String
        Try
            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim valorUDF As String = String.Empty

            oItemArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
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
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_form"></param>
    ''' <param name="p_oCompany"></param>
    ''' <param name="p_strTabla"></param>
    ''' <param name="p_nombreCampoVIN"></param>
    ''' <param name="p_fila"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidarLongitudVIN(ByVal p_form As Form, ByVal p_oCompany As SAPbobsCOM.Company, _
                                            Optional ByVal p_strTabla As String = "", Optional ByVal p_nombreCampoVIN As String = "", _
                                            Optional ByVal p_fila As Integer = 0) As Boolean
        Dim strNumeroVin As String
        strNumeroVin = p_form.DataSources.DBDataSources.Item(p_strTabla).GetValue(p_nombreCampoVIN, p_fila).Trim
        If Not String.IsNullOrEmpty(strNumeroVin) Then
            If strNumeroVin.Length <> 17 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_transferenciaAutomatica"></param>
    ''' <param name="strItemCode"></param>
    ''' <param name="strAlmacenOrigen"></param>
    ''' <param name="strAlmacenDestino"></param>
    ''' <param name="p_oCompany"></param>
    ''' <param name="p_informacionLineaRequisicion"></param>
    ''' <param name="p_lstLineasTransStock"></param>
    ''' <param name="p_cantidad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DevolverUbicacionArticuloPorDefecto(p_transferenciaAutomatica As Boolean, strItemCode As String, strAlmacenOrigen As String, strAlmacenDestino As String, _
                                                               ByVal p_oCompany As SAPbobsCOM.Company, Optional ByRef p_informacionLineaRequisicion As SCG.Requisiciones.UI.InformacionLineaRequisicion = Nothing, _
                                                                Optional ByRef p_lstLineasTransStock As StockTransfer = Nothing, Optional p_cantidad As Decimal = 0)


        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim intGrupoArticulo As String
        Dim intCentroCosto As String = 0

        oItemArticulo = p_oCompany.GetBusinessObject(BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemCode)
        intGrupoArticulo = oItemArticulo.ItemsGroupCode
        intCentroCosto = oItemArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value

        Dim dtUbicacionesDefecto As Data.DataTable


        Dim query As String = "SELECT   OWHS.BinActivat, OWHS.WhsCode, OWHS.WhsName, OWHS.DftBinAbs UbicacionDefectoAlmacen, OITW.DftBinAbs UbicacionDefectoItem, " & _
                              " OIGW.DftBinAbs UbicacionDefectoGrupoArticulo " & _
                              " FROM OWHS INNER JOIN " & _
                              " OITW ON OWHS.WhsCode = OITW.WhsCode INNER JOIN " & _
                              " OIGW ON OWHS.WhsCode = OIGW.WhsCode " & _
                              " where OITW.ItemCode = '" & strItemCode & "'and OWHS.WhsCode in ('" & strAlmacenOrigen & "','" & strAlmacenDestino & "') " & _
                              " and OIGW.ItmsGrpCod  = '" & intGrupoArticulo & "'"

        dtUbicacionesDefecto = EjecutarConsultaDataTable(query, p_oCompany.CompanyDB, p_oCompany.Server)

        For Each drw As DataRow In dtUbicacionesDefecto.Rows

            If drw.Item("WhsCode") = strAlmacenOrigen Then
                'valido si el alamacen usa ubicaciones
                If drw.Item("BinActivat") = "Y" Then

                    If Not IsDBNull(drw.Item("UbicacionDefectoItem")) AndAlso Not drw.Item("UbicacionDefectoItem") = 0 Then

                        If Not p_transferenciaAutomatica Then
                            p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoItem")

                        Else

                            p_lstLineasTransStock.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batFromWarehouse
                            p_lstLineasTransStock.Lines.BinAllocations.BinAbsEntry = drw.Item("UbicacionDefectoItem")
                            p_lstLineasTransStock.Lines.BinAllocations.Quantity = p_cantidad
                            p_lstLineasTransStock.Lines.BinAllocations.Add()

                        End If


                    ElseIf Not IsDBNull(drw.Item("UbicacionDefectoGrupoArticulo")) AndAlso Not drw.Item("UbicacionDefectoGrupoArticulo") = 0 Then

                        If Not p_transferenciaAutomatica Then

                            p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoGrupoArticulo")

                        Else
                            p_lstLineasTransStock.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batFromWarehouse
                            p_lstLineasTransStock.Lines.BinAllocations.BinAbsEntry = drw.Item("UbicacionDefectoGrupoArticulo")
                            p_lstLineasTransStock.Lines.BinAllocations.Quantity = p_cantidad
                            p_lstLineasTransStock.Lines.BinAllocations.Add()

                        End If

                    ElseIf Not IsDBNull(drw.Item("UbicacionDefectoAlmacen")) AndAlso Not drw.Item("UbicacionDefectoAlmacen") = 0 Then

                        If Not p_transferenciaAutomatica Then

                            p_informacionLineaRequisicion.DeUbicacion = drw.Item("UbicacionDefectoAlmacen")

                        Else

                            p_lstLineasTransStock.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batFromWarehouse
                            p_lstLineasTransStock.Lines.BinAllocations.BinAbsEntry = drw.Item("UbicacionDefectoAlmacen")
                            p_lstLineasTransStock.Lines.BinAllocations.Quantity = p_cantidad
                            p_lstLineasTransStock.Lines.BinAllocations.Add()

                        End If

                    End If
                End If
            End If
        Next

        oItemArticulo = Nothing

    End Function

    ''' <summary>
    ''' Retorna la cuenta configurada para un determinado Item
    ''' </summary>
    ''' <param name="p_itemCode">Código del Item</param>
    ''' <param name="strAlmacen">Almacén del Item</param>
    ''' <param name="p_strCuenta">Cuenta a consultar</param>
    ''' <param name="p_oCompany">Objeto compañía</param>
    ''' <returns>Cuenta del item</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerCuentaItem(ByVal p_itemCode As String, ByVal strAlmacen As String, ByVal p_strCuenta As String, ByVal p_oCompany As SAPbobsCOM.Company, Optional ByRef p_oItem As SAPbobsCOM.IItems = Nothing) As String

        Dim cuentaContable As String
        Dim oIWarehouses As IWarehouses
        Dim oItemGroup As IItemGroups
        Try
            cuentaContable = ""
            If p_oItem Is Nothing Then
                p_oItem = p_oCompany.GetBusinessObject(BoObjectTypes.oItems)
                If Not p_oItem.GetByKey(p_itemCode) Then Exit Try
            End If
            Select Case p_oItem.GLMethod
                Case BoGLMethods.glm_WH
                    oIWarehouses = p_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)
                    If oIWarehouses.GetByKey(strAlmacen) Then
                        Select Case p_strCuenta
                            Case Cuentas.CuentaIngresos
                                cuentaContable = oIWarehouses.RevenuesAccount
                            Case Cuentas.CuentaCostos
                                cuentaContable = oIWarehouses.CostOfGoodsSold
                            Case Cuentas.CuentaGastos
                                cuentaContable = oIWarehouses.ExpenseAccount
                        End Select
                    End If
                Case BoGLMethods.glm_ItemClass
                    oItemGroup = p_oCompany.GetBusinessObject(BoObjectTypes.oItemGroups)
                    If oItemGroup.GetByKey(p_oItem.ItemsGroupCode) Then
                        Select Case p_strCuenta
                            Case Cuentas.CuentaIngresos
                                cuentaContable = oItemGroup.RevenuesAccount
                            Case Cuentas.CuentaCostos
                                cuentaContable = oItemGroup.CostAccount
                            Case Cuentas.CuentaGastos
                                cuentaContable = oItemGroup.ExpensesAccount
                        End Select
                    End If
                Case BoGLMethods.glm_ItemLevel
                    For index As Integer = 0 To p_oItem.WhsInfo.Count - 1
                        p_oItem.WhsInfo.SetCurrentLine(index)
                        If p_oItem.WhsInfo.WarehouseCode = strAlmacen Then
                            Select Case p_strCuenta
                                Case Cuentas.CuentaIngresos
                                    cuentaContable = p_oItem.WhsInfo.RevenuesAccount
                                Case Cuentas.CuentaCostos
                                    cuentaContable = p_oItem.WhsInfo.CostAccount
                                Case Cuentas.CuentaGastos
                                    cuentaContable = p_oItem.WhsInfo.ExpensesAccount
                            End Select
                            Exit For
                        End If
                    Next
            End Select
            Return cuentaContable
        Catch ex As Exception
            Throw ex
        Finally
            DestruirObjeto(oIWarehouses)
            DestruirObjeto(oItemGroup)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_IdSucursal"></param>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerConsultaConfiguracionPorSucursal(ByVal p_IdSucursal As String, ByVal p_ocompany As SAPbobsCOM.Company) As Data.DataTable

        Dim oDataTableConfiguracionesSucursal As Data.DataTable
        Dim drRow As DataRow
        oDataTableConfiguracionesSucursal = New Data.DataTable()
        If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IdSucursal)) Then
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("DocEntry", Type.GetType("System.Int32")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("DocNum", Type.GetType("System.Int32")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Sucurs", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SerOfC", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SerOrC", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SerOfV", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SerOrV", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SerInv", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ArtCita", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DesSOfC", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DesSOfV", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DesSOrV", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DesSOrC", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DesSInv", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_HoraInicio", Type.GetType("System.DateTime")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_HoraFin", Type.GetType("System.DateTime")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CodCitaCancel", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CodCitaNueva", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CodCitaTarde", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CodCitaAnula", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CantMinTarde", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CantHorasValida", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UsaDurEC", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Imp_Serv", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Imp_Repuestos", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Imp_Suminis", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Imp_ServExt", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CosteoMO_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CosteoMO_I", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_TiempoEst_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_TiempoReal_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Moneda_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CuentaSys_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DescCuenta_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CtaAcreGast", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CtaDebGast", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_MonDocGastos", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_GenASGastos", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_GenFAGastos", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DescCtaAcreGast", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_DescCtaDebGast", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_USolOTEsp", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ValKm", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ValHS", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SDocCot", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Imp_Gastos", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Entrega_Rep", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ValReqPen", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_TiempoOFV_C", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_Requis", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseParts", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseServ", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseSE", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseSum", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_AsigTecOT", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ValTiemEst", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_FinOTCanSol", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CambPreTall", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_AsigUniMec", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseLisPreCli", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_GenOTEsp", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ValOTCreEsp", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CopiasOT", Type.GetType("System.Double")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UnidadTiemp", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UseCliFilter", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CitCliInac", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UsaOrdVenta", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UsaOfeVenta", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_ListaPrecios", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CodLisPre", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_AsigAutCol", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_SEInvent", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CostoSimp", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_CostoDet", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_UniTpMint", Type.GetType("System.Double")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_NoBodRep", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_NoBodPro", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_NoBodSum", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_NoBodSe", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_AgendaColor", Type.GetType("System.String")))
            oDataTableConfiguracionesSucursal.Columns.Add(New Data.DataColumn("U_GenReqCOV", Type.GetType("System.String")))
            With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_IdSucursal))
                drRow = oDataTableConfiguracionesSucursal.NewRow()
                drRow("DocEntry") = .DocEntry
                drRow("DocNum") = .DocNum
                drRow("U_Sucurs") = .U_Sucurs
                drRow("U_SerOfC") = .U_SerOfC
                drRow("U_SerOrC") = .U_SerOrC
                drRow("U_SerOfV") = .U_SerOfV
                drRow("U_SerOrV") = .U_SerOrV
                drRow("U_SerInv") = .U_SerInv
                drRow("U_ArtCita") = .U_ArtCita
                drRow("U_DesSOfC") = .U_DesSOfC
                drRow("U_DesSOfV") = .U_DesSOfV
                drRow("U_DesSOrV") = .U_DesSOrV
                drRow("U_DesSOrC") = .U_DesSOrC
                drRow("U_DesSInv") = .U_DesSInv
                drRow("U_HoraInicio") = .U_HoraInicio
                drRow("U_HoraFin") = .U_HoraFin
                drRow("U_CodCitaCancel") = .U_CodCitaCancel
                drRow("U_CodCitaNueva") = .U_CodCitaNueva
                drRow("U_CodCitaTarde") = .U_CodCitaTarde
                drRow("U_CodCitaAnula") = .U_CodCitaAnula
                drRow("U_CantMinTarde") = .U_CantMinTarde
                drRow("U_CantHorasValida") = .U_CantHorasValida
                drRow("U_UsaDurEC") = .U_UsaDurEC
                drRow("U_Imp_Serv") = .U_Imp_Serv
                drRow("U_Imp_Repuestos") = .U_Imp_Repuestos
                drRow("U_Imp_Suminis") = .U_Imp_Suminis
                drRow("U_Imp_ServExt") = .U_Imp_ServExt
                drRow("U_CosteoMO_C") = .U_CosteoMO_C
                drRow("U_CosteoMO_I") = .U_CosteoMO_I
                drRow("U_TiempoEst_C") = .U_TiempoEst_C
                drRow("U_TiempoReal_C") = .U_TiempoReal_C
                drRow("U_Moneda_C") = .U_Moneda_C
                drRow("U_CuentaSys_C") = .U_CuentaSys_C
                drRow("U_DescCuenta_C") = .U_DescCuenta_C
                drRow("U_CtaAcreGast") = .U_CtaAcreGast
                drRow("U_CtaDebGast") = .U_CtaDebGast
                drRow("U_MonDocGastos") = .U_MonDocGastos
                drRow("U_GenASGastos") = .U_GenASGastos
                drRow("U_GenFAGastos") = .U_GenFAGastos
                drRow("U_DescCtaAcreGast") = .U_DescCtaAcreGast
                drRow("U_DescCtaDebGast") = .U_DescCtaDebGast
                drRow("U_USolOTEsp") = .U_USolOTEsp
                drRow("U_ValKm") = .U_ValKm
                drRow("U_ValHS") = .U_ValHS
                drRow("U_SDocCot") = .U_SDocCot
                drRow("U_Imp_Gastos") = .U_Imp_Gastos
                drRow("U_Entrega_Rep") = .U_Entrega_Rep
                drRow("U_ValReqPen") = .U_ValReqPen
                drRow("U_TiempoOFV_C") = .U_TiempoOFV_C
                drRow("U_Requis") = .U_Requis
                drRow("U_UseParts") = .U_UseParts
                drRow("U_UseServ") = .U_UseServ
                drRow("U_UseSE") = .U_UseSE
                drRow("U_UseSum") = .U_UseSum
                drRow("U_AsigTecOT") = .U_AsigTecOT
                drRow("U_ValTiemEst") = .U_ValTiemEst
                drRow("U_FinOTCanSol") = .U_FinOTCanSol
                drRow("U_CambPreTall") = .U_CambPreTall
                drRow("U_AsigUniMec") = .U_AsigUniMec
                drRow("U_UseLisPreCli") = .U_UseLisPreCli
                drRow("U_GenOTEsp") = .U_GenOTEsp
                drRow("U_ValOTCreEsp") = .U_ValOTCreEsp
                drRow("U_CopiasOT") = .U_CopiasOT
                drRow("U_UnidadTiemp") = .U_UnidadTiemp
                drRow("U_UseCliFilter") = .U_UseCliFilter
                drRow("U_CitCliInac") = .U_CitCliInac
                drRow("U_UsaOrdVenta") = .U_UsaOrdVenta
                drRow("U_UsaOfeVenta") = .U_UsaOfeVenta
                drRow("U_ListaPrecios") = .U_ListaPrecios
                drRow("U_CodLisPre") = .U_CodLisPre
                drRow("U_AsigAutCol") = .U_AsigAutCol
                drRow("U_SEInvent") = .U_SEInvent
                drRow("U_CostoSimp") = .U_CostoSimp
                drRow("U_CostoDet") = .U_CostoDet
                drRow("U_UniTpMint") = .U_UniTpMint
                drRow("U_NoBodRep") = .U_NoBodRep
                drRow("U_NoBodPro") = .U_NoBodPro
                drRow("U_NoBodSum") = .U_NoBodSum
                drRow("U_NoBodSe") = .U_NoBodSE
                drRow("U_AgendaColor") = .U_AgendaColor
                drRow("U_GenReqCOV") = .U_GenReqCOV
            End With
            oDataTableConfiguracionesSucursal.Rows.Add(drRow)
        End If
        Return oDataTableConfiguracionesSucursal
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_decValor"></param>
    ''' <param name="p_strSeparadorMilesSAP"></param>
    ''' <param name="p_strSeparadorDecimalesSAP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ObtenerFormatoSAP(ByRef p_decValor As Decimal, ByRef p_strSeparadorMilesSAP As String, ByRef p_strSeparadorDecimalesSAP As String) As String

        Dim strValorSeleccionado As String
        Try
            strValorSeleccionado = p_decValor
            If p_strSeparadorDecimalesSAP <> "," Then
                strValorSeleccionado = strValorSeleccionado.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
            Else
                strValorSeleccionado = strValorSeleccionado.Replace(p_strSeparadorDecimalesSAP, p_strSeparadorMilesSAP)
            End If
            Return strValorSeleccionado
        Catch ex As Exception
            Return String.Empty
        End Try


    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_ocompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ValidarOTInternaConfiguracion(ByVal p_ocompany As SAPbobsCOM.Company) As Boolean
        Dim strConfOT As String
        strConfOT = DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim
        If Not String.IsNullOrEmpty(strConfOT) Then
            If strConfOT = "Y" Then
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_dt"></param>
    ''' <param name="p_strIdSucursal"></param>
    ''' <param name="p_strObjeto"></param>
    ''' <param name="p_oCompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ObtieneNumeracionPorSucursalObjeto(ByVal p_dt As DataTable,
                                                       ByVal p_strIdSucursal As String,
                                                       ByVal p_strObjeto As String,
                                                       ByVal p_oCompany As SAPbobsCOM.Company) As Integer
        Dim intDocEntry As Integer = 0
        Dim oCompanyService As CompanyService
        Dim oGeneralService As GeneralService
        Dim oGeneralData As GeneralData
        Dim oGeneralParams As GeneralDataParams
        Dim oChildrenLineasNum As GeneralDataCollection
        Dim oGeneralDataLinea As GeneralData
        Dim strSucursal As String
        Dim strSiguiente As String
        Dim intSiguienteResultado As Integer = 0
        Dim strInicio As String = String.Empty
        Dim strFin As String = String.Empty
        Dim intSiguiente As Integer

        If DMS_Connector.Configuracion.Numeracion.Any(Function(numeracion) numeracion.U_Objeto.Trim.Equals(p_strObjeto)) Then
            intDocEntry = DMS_Connector.Configuracion.Numeracion.FirstOrDefault(Function(numeracion) numeracion.U_Objeto.Trim.Equals(p_strObjeto)).DocEntry
        End If
        If intDocEntry > 0 Then
            oCompanyService = p_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_ONNM")
            oGeneralParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", intDocEntry)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oChildrenLineasNum = oGeneralData.Child("SCGD_LINEAS_NUM")

            For i As Integer = 0 To oChildrenLineasNum.Count - 1
                oGeneralDataLinea = oChildrenLineasNum.Item(i)

                strSucursal = oGeneralDataLinea.GetProperty("U_Sucu").ToString.Trim()
                strInicio = oGeneralDataLinea.GetProperty("U_Ini").ToString.Trim()
                strFin = oGeneralDataLinea.GetProperty("U_Fin").ToString.Trim()
                strSiguiente = oGeneralDataLinea.GetProperty("U_Sig").ToString.Trim()

                If strSucursal = p_strIdSucursal Then
                    intSiguiente = Integer.Parse(strSiguiente)
                    intSiguienteResultado = intSiguiente
                    intSiguiente += 1
                    oGeneralDataLinea.SetProperty("U_Sig", intSiguiente.ToString)
                    Exit For
                End If
            Next
            oGeneralService.Update(oGeneralData)
            Return intSiguienteResultado
        End If
    End Function
#End Region

#Region "Nuevos metodos"

    Shared Sub StartTransaction(ByRef p_SBO_Company As SAPbobsCOM.Company, _
                                ByRef p_SBO_Application As SAPbouiCOM.Application)
        Try
            If Not p_SBO_Company.InTransaction Then
                p_SBO_Company.StartTransaction()
            End If
        Catch ex As Exception
            p_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Shared Sub ResetTransaction(ByRef p_SBO_Company As SAPbobsCOM.Company, _
                                ByRef p_SBO_Application As SAPbouiCOM.Application)
        Try
            If p_SBO_Company.InTransaction Then
                p_SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            p_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Shared Sub CommitTransaction(ByRef p_SBO_Company As SAPbobsCOM.Company, _
                                 ByRef p_SBO_Application As SAPbouiCOM.Application)
        Try
            If p_SBO_Company.InTransaction Then
                p_SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            p_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Shared Sub RollbackTransaction(ByRef p_SBO_Company As SAPbobsCOM.Company, _
                                   ByRef p_SBO_Application As SAPbouiCOM.Application)
        Try
            If p_SBO_Company.InTransaction Then
                p_SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            p_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Shared Sub ObtenerAlmacenXCentroCosto(ByRef p_oSucursalList As List(Of String), _
                                                 ByRef p_ocompany As SAPbobsCOM.Company, _
                                                 ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            For Each strIdSucursal As String In p_oSucursalList
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSuc) confSuc.U_Sucurs.Trim().Equals(strIdSucursal)) Then
                    p_oBodegaCentroCostoList.AddRange(From bodegasCentroCosto In DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSuc) confSuc.U_Sucurs.Trim().Equals(strIdSucursal)).Bodegas_CentroCosto Select New BodegaCentroCosto() With {
                                                         .CentroCosto = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_CC), bodegasCentroCosto.U_CC, String.Empty),
                                                         .BodegaRepuesto = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Rep), bodegasCentroCosto.U_Rep, String.Empty),
                                                         .BodegaServicio = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Ser), bodegasCentroCosto.U_Ser, String.Empty),
                                                         .BodegaSuministro = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Sum), bodegasCentroCosto.U_Sum, String.Empty),
                                                         .BodegaServicioExterno = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_SE), bodegasCentroCosto.U_SE, String.Empty),
                                                         .BodegaProceso = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Pro), bodegasCentroCosto.U_Pro, String.Empty),
                                                         .Sucursal = IIf(Not String.IsNullOrEmpty(strIdSucursal), strIdSucursal, String.Empty)
                    })
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ObtenerAlmacenXCentroCosto(ByVal p_strIDSucursal As String, _
                                              ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSuc) confSuc.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                p_oBodegaCentroCostoList.AddRange(From bodegasCentroCosto In DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSuc) confSuc.U_Sucurs.Trim().Equals(p_strIDSucursal)).Bodegas_CentroCosto Select New BodegaCentroCosto() With {
                                                     .CentroCosto = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_CC), bodegasCentroCosto.U_CC, String.Empty),
                                                     .BodegaRepuesto = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Rep), bodegasCentroCosto.U_Rep, String.Empty),
                                                     .BodegaServicio = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Ser), bodegasCentroCosto.U_Ser, String.Empty),
                                                     .BodegaSuministro = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Sum), bodegasCentroCosto.U_Sum, String.Empty),
                                                     .BodegaServicioExterno = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_SE), bodegasCentroCosto.U_SE, String.Empty),
                                                     .BodegaProceso = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Pro), bodegasCentroCosto.U_Pro, String.Empty),
                                                     .BodegaReservas = IIf(Not String.IsNullOrEmpty(bodegasCentroCosto.U_Res), bodegasCentroCosto.U_Res, String.Empty),
                                                     .Sucursal = IIf(Not String.IsNullOrEmpty(p_strIDSucursal), p_strIDSucursal, String.Empty)
                })
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ObtieneNumeracionPorSucursalObjeto(ByVal p_strIdSucursal As String, _
                                                              ByVal p_strObjeto As String, _
                                                              ByVal p_oCompany As SAPbobsCOM.Company) As Integer
        Dim intDocEntry As Integer = 0
        Dim oCompanyService As CompanyService
        Dim oGeneralService As GeneralService
        Dim oGeneralData As GeneralData
        Dim oGeneralParams As GeneralDataParams
        Dim oChildrenLineasNum As GeneralDataCollection
        Dim oGeneralDataLinea As GeneralData
        Dim strSucursal As String
        Dim strSiguiente As String
        Dim intSiguienteResultado As Integer = 0
        Dim strInicio As String = String.Empty
        Dim strFin As String = String.Empty
        Dim intSiguiente As Integer = 0
        Try
            If DMS_Connector.Configuracion.Numeracion.Any(Function(numeracion) numeracion.U_Objeto.Trim.Equals(p_strObjeto)) Then
                intDocEntry = DMS_Connector.Configuracion.Numeracion.FirstOrDefault(Function(numeracion) numeracion.U_Objeto.Trim.Equals(p_strObjeto)).DocEntry
            End If
            If intDocEntry > 0 Then
                oCompanyService = p_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_ONNM")
                oGeneralParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", intDocEntry)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                oChildrenLineasNum = oGeneralData.Child("SCGD_LINEAS_NUM")

                For i As Integer = 0 To oChildrenLineasNum.Count - 1
                    oGeneralDataLinea = oChildrenLineasNum.Item(i)
                    strSucursal = oGeneralDataLinea.GetProperty("U_Sucu").ToString.Trim()
                    If Not String.IsNullOrEmpty(strSucursal) And Not String.IsNullOrEmpty(p_strIdSucursal) Then
                        If strSucursal = p_strIdSucursal Then
                            strInicio = oGeneralDataLinea.GetProperty("U_Ini").ToString.Trim()
                            strFin = oGeneralDataLinea.GetProperty("U_Fin").ToString.Trim()
                            strSiguiente = oGeneralDataLinea.GetProperty("U_Sig").ToString.Trim()
                            If Not String.IsNullOrEmpty(strSiguiente) Then
                                intSiguiente = Integer.Parse(strSiguiente)
                                For contador As Integer = 0 To 10
                                    If ValidaVisitaSiguiente(intSiguiente.ToString) Then
                                        intSiguienteResultado = intSiguiente
                                        intSiguiente += 1
                                        oGeneralDataLinea.SetProperty("U_Sig", intSiguiente.ToString)
                                        Exit For
                                    End If
                                    intSiguiente += 1
                                Next
                            End If
                            Exit For
                        End If
                    End If
                Next
                oGeneralService.Update(oGeneralData)
                Return intSiguienteResultado
            End If
        Catch ex As Exception
            ManejadorErrores(ex, DMS_Connector.Company.ApplicationSBO)
            Return -1
        End Try
    End Function

    Public Shared Function ValidaVisitaSiguiente(ByRef p_strNoVisita As String) As Boolean
        Dim oForm As SAPbouiCOM.Form
        Dim creationPackage As SAPbouiCOM.FormCreationParams
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsOfertaVisita As DBDataSource
        Try
            If Not String.IsNullOrEmpty(p_strNoVisita) Then
                creationPackage = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                'creationPackage.UniqueID = ""
                creationPackage.FormType = "VisitaSiguiente"
                creationPackage.ObjectType = ""

                oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(creationPackage)
                oForm.DataSources.DBDataSources.Add("OQUT")
                dsOfertaVisita = oForm.DataSources.DBDataSources.Item("OQUT")

                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.Alias = "U_SCGD_No_Visita"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = p_strNoVisita

                dsOfertaVisita.Query(oConditions)

                If dsOfertaVisita.Size > 0 Then Return False
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        Finally
            oForm.Close()
        End Try
    End Function

    Public Shared Sub RegistrarError(ByVal _ex As Exception, _
                                     ByRef _application As Application)
        Try
            Dim strDescription As String = String.Empty
            Dim exception As Exception = New Exception()

            exception = _ex
            _application.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            If exception.InnerException IsNot Nothing Then
                strDescription = String.Format("{0} <br /><br />InnerException: {1} <br /><br /> Stack trace: {2}", exception.Message, exception.InnerException.Message, exception.StackTrace)
            Else
                strDescription = String.Format("{0} <br /><br /> Stack trace: {1}", exception.Message, exception.StackTrace)
            End If

            LogText(exception)
        Catch ex As Exception

        End Try
    End Sub


    Private Shared Sub LogText(ByVal ex As Exception)
        Try
            'obtenemos la carpeta y ejecutable de nuestra aplicación 
            Dim rutaCarpeta As String = AppDomain.CurrentDomain.BaseDirectory.TrimEnd()
            'obtenemos sólo la carpeta (quitamos el ejecutable) 
            Dim carpeta As String = Path.GetFullPath(rutaCarpeta)
            Dim defaultXmlName As String = [String].Format("{0}{1}.xml", "XML Logs - ", DateTime.Now.ToString("dd-MM-yyyy"))
            Dim rutaXML As [String] = Path.Combine(carpeta, defaultXmlName)
            Dim [error] As List(Of [String]) = New List(Of String)()
            Dim lines = New List(Of [String])()
            Dim writer = New System.Xml.Serialization.XmlSerializer(GetType(LogErrores))
            Dim logErrores = New LogErrores()


            [error].Add([String].Format("[{0}] -- Error: {1}", DateTime.Now, ex.Message))
            If ex.InnerException IsNot Nothing Then
                [error].Add("InnerException: " + ex.InnerException.ToString())
            End If
            [error].Add("StackTrace: " + ex.StackTrace)

            If System.IO.File.Exists(rutaXML) Then
                Using reader = File.OpenText(rutaXML)
                    logErrores = DirectCast(writer.Deserialize(reader), LogErrores)
                    Dim err = New [Error]()
                    err.TipoExcepcion = ex.[GetType]().Name.ToString()
                    err.Aplicacion = siteName
                    err.Codigo = System.Runtime.InteropServices.Marshal.GetExceptionCode().ToString()
                    err.CompañiaSBO = ""
                    err.Mensaje = ex.Message
                    err.StackTrace = ex.StackTrace
                    err.Fecha = DateTime.Now.ToString()
                    If logErrores.Errores.Count > 0 Then
                        logErrores.Errores.Add(err)
                    Else
                        Dim errs = New List(Of [Error])()
                        errs.Add(err)
                        logErrores.Errores = errs
                    End If
                End Using
            Else
                logErrores.Idioma = CultureInfo.CurrentCulture.Name
                Dim errs = New List(Of [Error])()
                Dim err = New [Error]()
                err.TipoExcepcion = ex.[GetType]().Name.ToString()
                err.Aplicacion = "SCG DMS One"
                err.Codigo = System.Runtime.InteropServices.Marshal.GetExceptionCode().ToString()
                err.CompañiaSBO = ""
                err.Mensaje = ex.Message
                err.StackTrace = ex.StackTrace
                err.Fecha = DateTime.Now.ToString("dd-MM-yyyy")
                If ex.InnerException IsNot Nothing Then
                    err.InnerException = ex.InnerException.Message
                Else
                    err.InnerException = String.Empty
                End If
                errs.Add(err)
                logErrores.Errores = errs
            End If


            Dim file__1 = New System.IO.StreamWriter(rutaXML)
            writer.Serialize(file__1, logErrores)
            file__1.Close()

        Catch exe As Exception
        End Try
    End Sub

    Shared Function RetornaImpuestoVenta(ByVal p_strImpuestos As String, p_dtFecha As DateTime) As Double
        Return DMS_Connector.Helpers.GetTaxRate(p_strImpuestos, p_dtFecha, DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup.Trim().Equals("Y"))
    End Function

    'Cambio para desarrollo de determinacion de cuentas de mayor apliadas
    Shared Function ObtenerCuentaContableArticulo(ByRef p_intTipoArticulo As Integer, _
                                                  ByRef p_ItemCode As String, _
                                                  ByRef p_intCuenta As Integer, _
                                                  ByRef p_strIDSucursal As String, _
                                                  Optional ByRef p_strAlmacen As String = "") As String
        Try
            '***** Valida si esta activa la configuracion de determinación de cuentas de mayor ampliada *****
            If DMS_Connector.Company.AdminInfo.EnableAdvancedGLAccountDetermination = SAPbobsCOM.BoYesNoEnum.tYES Then
                Return ObtenerCuentaConfiguracionSucursal(p_intTipoArticulo, p_strIDSucursal, p_intCuenta)
            Else
                If Not String.IsNullOrEmpty(p_strAlmacen) And Not String.IsNullOrEmpty(p_ItemCode) Then
                    Return ObtenerCuentaArticulo(p_ItemCode, p_strAlmacen, p_intCuenta)
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            Throw
        End Try
    End Function

    Shared Function ObtenerCuentaContable(ByRef p_intTipoArticulo As Integer, _
                                          ByRef p_intCuenta As Integer, _
                                          ByRef p_strIDSucursal As String, _
                                          Optional ByRef p_strAlmacen As String = "") As String
        Try
            '***** Valida si esta activa la configuracion de determinación de cuentas de mayor ampliada *****
            If DMS_Connector.Company.AdminInfo.EnableAdvancedGLAccountDetermination = SAPbobsCOM.BoYesNoEnum.tYES Then
                Return ObtenerCuentaConfiguracionSucursal(p_intTipoArticulo, p_strIDSucursal, p_intCuenta)
            Else
                If Not String.IsNullOrEmpty(p_strAlmacen) Then
                    Return ObtenerCuentaAlmacen(p_strAlmacen, p_intCuenta)
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            Throw
        End Try
    End Function

    Shared Function ObtenerCuentaArticulo(ByRef p_strItemCode As String, _
                                          ByRef p_strAlmacen As String, _
                                          ByRef p_intCuenta As Integer) As String
        Dim oItemArticulo As SAPbobsCOM.IItems
        Try
            oItemArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_strItemCode)
            '*********Obtiene cuenta según configuración contable del articulo
            Select Case oItemArticulo.GLMethod
                Case SAPbobsCOM.BoGLMethods.glm_WH
                    Return ObtenerCuentaAlmacen(p_strAlmacen, p_intCuenta)
                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                    Return ObtenerCuentaGrupoArticulo(oItemArticulo.ItemsGroupCode, p_intCuenta)
                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel
                    Return ObtenerCuentaNivelArticulo(p_strAlmacen, p_intCuenta, oItemArticulo)
                Case Else
                    Return ObtenerCuentaAlmacen(p_strAlmacen, p_intCuenta)
            End Select
            Return String.Empty
        Catch ex As Exception
            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If oItemArticulo IsNot Nothing Then
                Utilitarios.DestruirObjeto(oItemArticulo)
            End If
        End Try
    End Function

    Shared Function ObtenerCuentaConfiguracionSucursal(ByRef p_intTipoArticulo As Integer, _
                                                       ByVal p_strIDSucursal As String, _
                                                       ByRef p_intCuenta As Integer) As String
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal))
                    Select Case p_intTipoArticulo
                        Case TiposArticulos.scgActividad
                            Select Case p_intCuenta
                                Case Account.SaleCostAc
                                    Return .U_CtaDebitoMO
                            End Select
                        Case TiposArticulos.scgServicioExt
                            Select Case p_intCuenta
                                Case Account.ExpensesAc
                                    Return .U_CtaGastosSE
                                Case Account.TransferAc
                                    Return .U_CtaDotacionSE
                                Case Account.SaleCostAc
                                    Return .U_CtaCostosBVSE
                                Case Account.CtaDifPrecioSE
                                    Return .U_CtaDifPrecioSE
                            End Select
                        Case TiposArticulos.scgOtrosGastos_Costos
                            Select Case p_intCuenta
                                Case Account.ExpensesAc

                                Case Account.TransferAc

                                Case Account.SaleCostAc
                                    Return .U_CtaDebitoCosto
                            End Select
                    End Select
                End With
            End If
            Return String.Empty
        Catch ex As Exception
            Throw
        End Try
    End Function
    Shared Function ObtenerCuentaAlmacen(ByRef p_strAlmacen As String, _
                                         ByRef p_intCuenta As Integer) As String
        Dim oAlmacen As SAPbobsCOM.Warehouses
        Try
            oAlmacen = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses)
            If oAlmacen.GetByKey(p_strAlmacen) Then
                Select Case p_intCuenta
                    Case Account.ExpensesAc
                        Return oAlmacen.ExpenseAccount
                    Case Account.TransferAc
                        Return oAlmacen.TransfersAcc
                    Case Account.SaleCostAc
                        Return oAlmacen.CostOfGoodsSold
                    Case Account.CtaDifPrecioSE
                        Return oAlmacen.PriceDifferencesAccount
                End Select
            End If
            Return String.Empty
        Catch ex As Exception
            Throw
        Finally
            Utilitarios.DestruirObjeto(oAlmacen)
        End Try
    End Function

    Shared Function ObtenerCuentaGrupoArticulo(ByRef p_intGrupoArticulo As Integer, _
                                               ByRef p_intCuenta As Integer) As String
        Dim oGrupoArticulo As SAPbobsCOM.ItemGroups
        Try
            oGrupoArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItemGroups)
            If oGrupoArticulo.GetByKey(p_intGrupoArticulo) Then
                Select Case p_intCuenta
                    Case Account.ExpensesAc
                        Return oGrupoArticulo.ExpenseAccount
                    Case Account.TransferAc
                        Return oGrupoArticulo.TransfersAcc
                    Case Account.SaleCostAc
                        Return oGrupoArticulo.CostAccount
                End Select
            End If
            Return String.Empty
        Catch ex As Exception
            Throw
        Finally
            Utilitarios.DestruirObjeto(oGrupoArticulo)
        End Try
    End Function

    Shared Function ObtenerCuentaNivelArticulo(ByRef p_strAlmacen As String, _
                                               ByRef p_intCuenta As Integer, _
                                               ByRef p_oItem As SAPbobsCOM.IItems) As String
        Try
            For index As Integer = 0 To p_oItem.WhsInfo.Count - 1
                p_oItem.WhsInfo.SetCurrentLine(index)
                If p_oItem.WhsInfo.WarehouseCode = p_strAlmacen Then
                    Select Case p_intCuenta
                        Case Account.ExpensesAc
                            Return p_oItem.WhsInfo.ExpensesAccount
                        Case Account.TransferAc
                            Return p_oItem.WhsInfo.TransfersAcc
                        Case Account.SaleCostAc
                            Return p_oItem.WhsInfo.CostOfGoodsSold
                    End Select
                    Exit For
                End If
            Next
            Return String.Empty
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetListadoValidValues(ByVal _strQuery As String) As List(Of ListadoValidValues)
        Dim lstValidValues As List(Of ListadoValidValues) = New List(Of ListadoValidValues)()

        Try
            For Each row As DataRow In EjecutarConsultaDataTable(_strQuery).Rows
                lstValidValues.Add(New ListadoValidValues() With {.strCode = row(0).ToString().Trim(), .strName = row(1).ToString().Trim()})
            Next
            Return lstValidValues
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oValidValues"></param>
    ''' <param name="p_lstListaValores"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Overloads Shared Sub CargarValidValuesEnCombosVehiculo(ByRef oValidValues As SAPbouiCOM.ValidValues, ByVal p_lstListaValores As List(Of ListadoValidValues), Optional ByVal p_AddBlankValue As Boolean = False)
        Try
            Dim dictionary As Dictionary(Of String, String)
            Dim strDesc As String
            'Borra los ValidValues
            While oValidValues.Count > 0
                oValidValues.Remove(oValidValues.Item(0).Value, BoSearchKey.psk_ByValue)
            End While
            If p_AddBlankValue Then
                oValidValues.Add(" ", " ")
            End If
            dictionary = New Dictionary(Of String, String)()
            ''Agrega los ValidValues
            For Each oValidValue As ListadoValidValues In p_lstListaValores
                If oValidValue.strName.Trim.Length > 60 Then
                    strDesc = oValidValue.strName.Trim.Substring(0, 60)
                Else
                    strDesc = oValidValue.strName.Trim
                End If
                If Not dictionary.ContainsKey(oValidValue.strCode) AndAlso Not dictionary.ContainsValue(strDesc) Then
                    oValidValues.Add(oValidValue.strCode.Trim, strDesc)
                    dictionary.Add(oValidValue.strCode.Trim, strDesc)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Dimension Contable"
    Public Function ValidaUsaDimensionOfertaVentas(ByVal p_strIDSucursal As String, ByVal p_strTipoOT As String) As Boolean
        Dim strUsaDimensiones As String = String.Empty
        Dim strUsaDimensionesOFV As String = String.Empty
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal))
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(p_strTipoOT)) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_strTipoOT)).U_UsaDim) Then strUsaDimensiones = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_strTipoOT)).U_UsaDim
                        If Not String.IsNullOrEmpty(strUsaDimensiones) Then
                            If strUsaDimensiones = "Y" Then
                                If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_strTipoOT)).U_UsaDOFV) Then strUsaDimensionesOFV = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(p_strTipoOT)).U_UsaDOFV
                                If Not String.IsNullOrEmpty(strUsaDimensionesOFV) Then
                                    If strUsaDimensionesOFV = "Y" Then
                                        Return True
                                    End If
                                End If
                            End If
                        End If   
                    End If
                End With
            End If
            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub ObtenerDimensionesOfertaVenta(ByRef p_oDocumentoCotizacion As oDocumento,
                                             ByRef p_blnUsaDimensiones As Boolean,
                                             ByRef p_blnUsaDimensionesOFV As Boolean)
        '*****Objetos SAP ******
        Dim oCotizacion As SAPbobsCOM.Documents
        '*****DataContract *****
        Dim oDocumento As oDocumento
        Dim oLineasDocumento As List(Of oLineasDocumento)
        Try
            oCotizacion = CType(DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
            If oCotizacion.GetByKey(p_oDocumentoCotizacion.DocEntry) Then
                oDocumento = New oDocumento()
                oLineasDocumento = New List(Of oLineasDocumento)()
                For rowCotizacion As Integer = 0 To oCotizacion.Lines.Count - 1
                    oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                    With oLineasDocumento
                        .Add(New oLineasDocumento())
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
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Sub
#End Region


    Public Sub New()

    End Sub
End Class

<Serializable()> _
Public Class LogErrores
    Public Idioma As String
    Public Errores As List(Of [Error])
End Class
Public Class [Error]
    Public TipoExcepcion As String
    Public Codigo As String
    Public Mensaje As String
    Public Aplicacion As String
    Public CompañiaSBO As String
    Public Fecha As String
    Public StackTrace As String
    Public InnerException As String
End Class
