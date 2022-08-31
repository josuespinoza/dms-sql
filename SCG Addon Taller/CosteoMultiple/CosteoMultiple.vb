Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.UX.Windows
Imports System.Data.SqlClient
Imports SCG.SBOFramework
Imports SCG.SBOFramework.DI
Imports System.Collections.Generic
Imports System.Globalization

Partial Public Class CosteoMultiple

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private m_objGoodReceiptusado As ObjetoGoodReceiptCls

    Private Const mc_strUIDrCosteoMultiple As String = "SCGD_CMU"
    Private Const mc_strUIDVehículos As String = "SCGD_MNO"

    Private Const mc_strSCG_VEHICULO As String = "@SCGD_VEHICULO"
    Private Const mc_strEstadoInventario As String = "U_TIPINV"

    Public n As NumberFormatInfo

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDContrato As String = "col_Cont"
    Private Const mc_strUIDUnid As String = "col_Unid"
    Private Const mc_strUIDMarca As String = "col_Mar"
    Private Const mc_strUIDEstilo As String = "col_Est"


    Private Const mc_strSinCostear As String = "'S'"
    Private Const mc_strNoCosteable As String = "N"
    Private Const mc_strCosteado As String = "C"

    Private m_dbVehiculos As SAPbouiCOM.DBDataSource

    Private intDocEntryT As Integer
    Private intSerieT As Integer

    'Nombres de campos del datasource
    Private Const mc_strIDContrato As String = "U_CTOVTA"
    Private Const mc_strMarca As String = "U_Des_Marc"
    Private Const mc_strEstilo As String = "U_Des_Esti"
    Private Const mc_strModelo As String = "U_Des_Mode"
    Private Const mc_strVIN As String = "U_Num_VIN"
    Private Const mc_strUnidad As String = "U_Cod_Unid"

    Public Const mc_strFolder1 As String = "FOLDER1"
    Public Const mc_strFolder2 As String = "FOLDER2"


    Private m_oFormCosteoMultiples As SAPbouiCOM.Form

    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon


    Private m_cnConeccionTransaccion As New SqlClient.SqlConnection
    Private m_tnTransaccion As SqlClient.SqlTransaction

    Private m_decTipoCambio As Decimal
    Private m_strMonedaLocal As String
    Private m_strMonedaSistema As String
    Public m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private m_dtsGoodReceipt As GoodReceiptDataset
    Private m_dttGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVEDataTable
    Private m_dttGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESDataTable
    Private m_dtrGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVERow
    Private m_dtrGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESRow

    Private strMonedaLocal As String = ""
    Private strMonedaSistema As String = ""

    Private ListaCodigoTransaccion As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaMontoMoneda As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaCantidadLocal As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)
    Private ListaNombreTransaccionLocal As Generic.IList(Of String) = New Generic.List(Of String)

    Private ListaCodigoTransaccionSistema As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaMontoMonedaSistema As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaCantidadSistema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)
    Private ListaNombreTransaccionSistema As Generic.IList(Of String) = New Generic.List(Of String)

    Private CIFLocal As Decimal
    Private CIFSistema As Decimal

    Private g_blnLineaAgregada As Boolean = False

    Private blnVehiculosSinCostear As Boolean = False
    Private blnVehiculosRecosteo As Boolean = False

    Private blnUnidadesServicioTaller As Boolean = False

    Public EditTextFechEventoDL As EditText

    Private blnFechaContabilizacion As Boolean = True

    'datasets y datarows

    Private dtsDocumentosFormularios As New RecosteoDataSet
    Private dtaDocumentosFormularios As New RecosteoDataSetTableAdapters.FormulariosTableAdapter

    Private dtsDocumentosFacturaClientes As New RecosteoDataSet
    Private dtaDocumentosFacturaClientes As New RecosteoDataSetTableAdapters.FacturaClientesTableAdapter

    Private dtsSaldosIniciales As New RecosteoDataSet
    Private dtaSaldosIniciales As New RecosteoDataSetTableAdapters.SaldosInicialesTableAdapter

    Private dtsAsientos As New RecosteoDataSet
    Private dtaAsientos As New RecosteoDataSetTableAdapters.AsientosTableAdapter

    Private dtsAsientosSalidasInventario As New RecosteoDataSet
    Private dtaAsientosSalidasInventario As New RecosteoDataSetTableAdapters.AsientoSalidaInventarioTableAdapter

    'Agregado Erick Sanabria 28.09.2012 (DataSet Y DataAdapter Notas de Crédito Proveedores) 
    Private dtsNotasCreditoProveedor As New RecosteoDataSet
    Private dtaNotasCreditoProveedor As New RecosteoDataSetTableAdapters.NotaCreditoProveedorDataAdapter
    'Agregado Erick Sanabria 28.09.2012 (DataSet Y DataAdapter Notas de Crédito Proveedores) 

    'Agregado Erick Sanabria 28.09.2012 (DataRow Notas de Crédito Proveedores) 
    Private drwNotaCreditoProveedor As RecosteoDataSet.NotaCreditoProveedorRow
    'Agregado Erick Sanabria 28.09.2012 (DataRow Notas de Crédito Proveedores) 

    Private drwAsientosSalidas As RecosteoDataSet.AsientoSalidaInventarioRow

    Private drwDocumentosF As RecosteoDataSet.FormulariosRow

    Private drwAsientos As RecosteoDataSet.AsientosRow

    Private drwSaldoInicial As RecosteoDataSet.SaldosInicialesRow

    Private drwDocumentosFacturaClientes As RecosteoDataSet.FacturaClientesRow

    Private _dtEncabezado As SAPbouiCOM.DataTable

    Private udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo

    Private strUtilizaCosteoAccesorios As String = String.Empty

    Private strTipoDocumentoServicio As String = "S"

    Private strTipoDocumentoArticulo As String = "I"

    Private blnUtilizaCosteoAccesorios As Boolean = False


    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property


    Private dcCosto As Decimal = 0
    Private dcCostoS As Decimal = 0

#End Region

#Region "Recosteo"

    Private Const mc_strGoodReceipts As String = "@SCGD_GOODRECEIVE"

    Private Const mc_strStatus As String = "Status"
    Private Const mc_strMarcaR As String = "U_Des_Marc"
    Private Const mc_strEstiloR As String = "U_Des_Esti"
    Private Const mc_strModeloR As String = "U_Des_Mode"
    Private Const mc_strVINR As String = "U_Num_VIN"
    Private Const mc_strAsientoEntrada As String = "U_As_Entr"
    Private Const mc_strUnidadR As String = "U_Unidad"

    Private m_oFormRecosteoMultiple As SAPbouiCOM.Form
    Private m_dbRecosteo As SAPbouiCOM.DBDataSource

#End Region

#Region "Metodos"

    Protected Friend Sub CargaFormularioCosteoMultiplesUnidades(Optional ByVal blnInicial As Boolean = False)

        Dim ocombo As SAPbouiCOM.ComboBox
        Dim strTipoParaTaller As String = String.Empty

        Try

            Dim strConexionDBSucursal As String = ""
            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(_applicationSbo.Company.ServerName, _applicationSbo.Company.DatabaseName, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            If blnInicial = True Then

                strTipoParaTaller = objConfiguracionGeneral.InventarioVehiculoVendido

                m_oCompany = _companySbo

                ocombo = DirectCast(FormularioSBO.Items.Item("cboTipo").Specific, SAPbouiCOM.ComboBox)
                Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, "Select Code,Name From [@SCGD_TIPOVEHICULO] with(nolock) where Code <> '" & strTipoParaTaller.Trim & "' Order by Name")

                EditTextFecha.AsignaValorUserDataSource(Date.Now.ToString("yyyMMdd"))

            End If

            blnUnidadesServicioTaller = True

            Call CargarMatrixVehiculosSinCostear()

            blnUnidadesServicioTaller = False



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
            m_oFormCosteoMultiples.Freeze(False)

        End Try
    End Sub

    Protected Friend Sub CargaFormularioListadoGR()

        Try

            Call CargarMatrixRecosteo()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

        End Try
    End Sub

    Public Function CargarMatrixVehiculosSinCostear() As Boolean

        Dim strTipoParaTaller As String
        Dim strNoDisponible As String
        Dim strUnidad As String

        Dim blnUnidadesFacturadas As Boolean = False

        Dim strFacturadas As String
        Dim strTipo As String
        Dim dataTablaSC As DataTable
        Dim dataTablaConfiguracionGen As DataTable
        Dim strUnidadDT As String
        Dim strMarcaDT As String
        Dim strEstiloDT As String
        Dim strContratoDT As String

        Dim strSeleccionTodas As String

        Dim strFechaInicio As String
        Dim strFechaFin As String
        Dim strFechaInicioFormateada As String
        Dim strFechaFinFormateada As String
        Dim dtFechaInicio As DateTime
        Dim dtFechaFin As DateTime
        Dim strNumRecepcion As String
        Dim strCodPedido As String
        Dim strValorInvetarioConf As String

        blnVehiculosSinCostear = True
        blnVehiculosRecosteo = False


        Dim strConsulta As String ' = "Select U_Cod_Unid,U_Des_Marc,U_Des_Esti,U_CTOVTA From [@SCGD_VEHICULO] Where "

        strConsulta = " Select VEH.U_Cod_Unid,VEH.U_Des_Marc,VEH.U_Des_Esti,VEH.U_CTOVTA, VEH.U_DocRecepcion, U_DocPedido  " +
                        " FROM [@SCGD_VEHICULO] VEH with(nolock) " +
                        " WHERE "

        Dim strOrderBy As String = " ORDER BY VEH.U_Cod_Unid"

        Try
            
            FormularioSBO.Freeze(True)

            strUnidad = EditTextUnidad.ObtieneValorUserDataSource()
            strFacturadas = CheckBoxFacturadas.ObtieneValorUserDataSource()
            strNumRecepcion = EditTextRecepcion.ObtieneValorUserDataSource()
            strCodPedido = EditTextPedido.ObtieneValorUserDataSource()

            If strFacturadas = "Y" Then
                blnUnidadesFacturadas = True
                blnUnidadesServicioTaller = False
            End If

            strNoDisponible = objConfiguracionGeneral.DisponibilidadVehiculoVendido
            strTipoParaTaller = objConfiguracionGeneral.InventarioVehiculoVendido

            strConsulta = strConsulta + mc_strUnidad + " Is Not Null And " &
                                                         mc_strUnidad + "<>'' And " &
                                                         mc_strEstadoInventario & "=" & mc_strSinCostear


            If Not String.IsNullOrEmpty(strUnidad) Then
                strConsulta = strConsulta & " And " & mc_strUnidad & " LIKE '%" & strUnidad & "%'"
            End If

            If blnUnidadesFacturadas Then

                strFechaInicioFormateada = String.Empty
                strFechaFinFormateada = String.Empty

                strFechaInicio = EditTextFechaInicio.ObtieneValorUserDataSource()
                strFechaFin = EditTextFechaFin.ObtieneValorUserDataSource()

                If Not String.IsNullOrEmpty(strFechaInicio) Then
                    dtFechaInicio = Date.ParseExact(strFechaInicio, "yyyyMMdd", Nothing)
                    strFechaInicioFormateada = Utilitarios.RetornaFechaFormatoDB(dtFechaInicio, CompanySBO.Server, True)
                End If

                If Not String.IsNullOrEmpty(strFechaFin) Then
                    dtFechaFin = Date.ParseExact(strFechaFin, "yyyyMMdd", Nothing)
                    strFechaFinFormateada = Utilitarios.RetornaFechaFormatoDB(dtFechaFin, CompanySBO.Server, True)
                End If

                If String.IsNullOrEmpty(strFechaInicioFormateada) And String.IsNullOrEmpty(strFechaFinFormateada) Then
                    strConsulta = strConsulta +
                        String.Format(" And U_Tipo = {0} And U_CTOVTA Is Not Null And  U_NUMFAC Is Not Null",
                                      strTipoParaTaller)
                Else
                    strConsulta = strConsulta +
                        String.Format(" And U_Tipo = {0} And U_CTOVTA Is Not Null And  U_NUMFAC Is Not Null " & _
                                      " And  U_FechaVen >= '{1}' and U_FechaVen <= '{2}' ",
                                      strTipoParaTaller, strFechaInicioFormateada, strFechaFinFormateada)
                End If

                blnUnidadesServicioTaller = False

            End If

            strTipo = ComboBoxTipo.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(strTipo) Then

                strConsulta = strConsulta & " And"

                If blnUnidadesFacturadas Then
                    strConsulta = strConsulta & " U_Tipo_Ven=" & strTipo
                Else
                    strConsulta = strConsulta & " U_Tipo=" & strTipo
                End If
           
            End If

            If blnUnidadesServicioTaller Then

                strConsulta = strConsulta + " And (U_CTOVTA Is Null OR U_CTOVTA >= 0) " &
                                            " And (U_NUMFAC Is Null OR U_NUMFAC >= 0) "
            End If

            If Not String.IsNullOrEmpty(strNumRecepcion) Then
                strConsulta = strConsulta + " AND (U_DocRecepcion = " & strNumRecepcion & ")"
            End If
            If Not String.IsNullOrEmpty(strCodPedido) Then
                strConsulta = strConsulta + "AND (U_DocPedido = '" & strCodPedido & "')"
            End If

            MatrixSinCostear.Matrix.Clear()
            dataTableSinCostear.Rows.Clear()

            dataTablaSC = FormularioSBO.DataSources.DataTables.Item("SC")
            dataTablaSC.Rows.Clear()

            strConsulta = strConsulta & strOrderBy
            dataTablaSC.ExecuteQuery(strConsulta)

            If dataTablaSC.Rows.Count > 0 Then

                strUnidadDT = dataTablaSC.GetValue("U_Cod_Unid", 0)

                If Not String.IsNullOrEmpty(strUnidadDT) AndAlso Not strUnidadDT = "0" Then

                    strSeleccionTodas = CheckBoxSelTodas.ObtieneValorUserDataSource()

                    For i As Integer = 0 To dataTablaSC.Rows.Count - 1

                        strUnidadDT = dataTablaSC.GetValue("U_Cod_Unid", i)
                        strMarcaDT = dataTablaSC.GetValue("U_Des_Marc", i)
                        strEstiloDT = dataTablaSC.GetValue("U_Des_Esti", i)
                        strContratoDT = dataTablaSC.GetValue("U_CTOVTA", i)

                        dataTableSinCostear.Rows.Add()

                        If strSeleccionTodas = "Y" Then
                            dataTableSinCostear.SetValue("seleccion", i, "Y")
                        Else
                            dataTableSinCostear.SetValue("seleccion", i, "N")
                        End If
                        If Not String.IsNullOrEmpty(strUnidadDT) And Not strUnidadDT = "0" Then
                            dataTableSinCostear.SetValue("unidad", i, strUnidadDT)
                        End If
                        If Not String.IsNullOrEmpty(strMarcaDT) And Not strMarcaDT = "0" Then
                            dataTableSinCostear.SetValue("marca", i, strMarcaDT)
                        End If
                        If Not String.IsNullOrEmpty(strEstiloDT) And Not strEstiloDT = "0" Then
                            dataTableSinCostear.SetValue("estilo", i, strEstiloDT)
                        End If
                        If Not String.IsNullOrEmpty(strContratoDT) And Not strContratoDT = "0" Then
                            dataTableSinCostear.SetValue("contrato", i, strContratoDT)
                        End If

                    Next

                End If

            End If

            MatrixSinCostear.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

            Return True

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

            Return False
        End Try

    End Function

    Public Function CargarMatrixRecosteo()

        Dim strUnidad As String
        Dim dataTablaRC As DataTable
        Dim strUnidadDT As String
        Dim strMarcaDT As String
        Dim strEstiloDT As String
        Dim strVINDT As String
        Dim strDocEntryDT As String
        Dim strRecepcion As String
        Dim strCodPedido As String


        blnVehiculosRecosteo = True
        blnVehiculosSinCostear = False


        Dim strConsulta As String = String.Empty

        strConsulta = " Select GR.U_Unidad, GR.U_Marca, GR.U_Estilo, GR.U_VIN, GR.DocEntry  from [@SCGD_GOODRECEIVE] GR with (nolock)" +
                        " Where GR.Status='O' And GR.U_As_Entr Is Not Null"

        Dim strOrderBy As String = " ORDER BY GR.U_Unidad"

        Try
            strUnidad = String.Empty
            strRecepcion = String.Empty

            FormularioSBO.Freeze(True)

            strUnidad = EditTextUnidad.ObtieneValorUserDataSource()
            strRecepcion = EditTextRecepcion.ObtieneValorUserDataSource()
            strCodPedido = EditTextPedido.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(strUnidad) Then
                strConsulta = strConsulta & " And U_Unidad like'%" & strUnidad & "%'"
            End If

            Dim valorTipoInventario As String = ComboBoxTipo.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(valorTipoInventario) Then
                strConsulta = strConsulta & " And U_Tipo='" & valorTipoInventario & "'"
            End If

            If Not String.IsNullOrEmpty(strRecepcion) Then
                strConsulta = strConsulta + " AND (GR.U_DocRecep = '" & strRecepcion & "')"
            End If
            If Not String.IsNullOrEmpty(strCodPedido) Then
                strConsulta = strConsulta + " AND (GR.U_DocPedido = '" & strCodPedido & "')"

            End If

            MatrixRecosteo.Matrix.Clear()
            dataTableRecosteo.Rows.Clear()

            dataTablaRC = FormularioSBO.DataSources.DataTables.Item("RC")
            dataTablaRC.Rows.Clear()

            strConsulta = strConsulta & strOrderBy

            dataTablaRC.ExecuteQuery(strConsulta)

            If dataTablaRC.Rows.Count > 0 Then

                strDocEntryDT = dataTablaRC.GetValue("DocEntry", 0)

                If Not String.IsNullOrEmpty(strDocEntryDT) AndAlso Not strDocEntryDT = "0" Then

                    Dim strSeleccionTodasRecost As String = CheckBoxSelRecost.ObtieneValorUserDataSource()

                    For i As Integer = 0 To dataTablaRC.Rows.Count - 1

                        strUnidadDT = dataTablaRC.GetValue("U_Unidad", i)
                        strMarcaDT = dataTablaRC.GetValue("U_Marca", i)
                        strEstiloDT = dataTablaRC.GetValue("U_Estilo", i)
                        strVINDT = dataTablaRC.GetValue("U_VIN", i)
                        strDocEntryDT = dataTablaRC.GetValue("DocEntry", i)

                        dataTableRecosteo.Rows.Add()

                        If strSeleccionTodasRecost = "Y" Then
                            dataTableRecosteo.SetValue("seleccion", i, "Y")
                        Else
                            dataTableRecosteo.SetValue("seleccion", i, "N")
                        End If

                        If Not String.IsNullOrEmpty(strUnidadDT) And Not strUnidadDT = "0" Then
                            dataTableRecosteo.SetValue("unidad", i, strUnidadDT)
                        End If

                        If Not String.IsNullOrEmpty(strMarcaDT) And Not strMarcaDT = "0" Then
                            dataTableRecosteo.SetValue("marca", i, strMarcaDT)
                        End If

                        If Not String.IsNullOrEmpty(strEstiloDT) And Not strEstiloDT = "0" Then
                            dataTableRecosteo.SetValue("estilo", i, strEstiloDT)
                        End If

                        If Not String.IsNullOrEmpty(strVINDT) And Not strVINDT = "0" Then
                            dataTableRecosteo.SetValue("vin", i, strVINDT)
                        End If

                        If Not String.IsNullOrEmpty(strDocEntryDT) And Not strDocEntryDT = "0" Then
                            dataTableRecosteo.SetValue("doc", i, strDocEntryDT)
                        End If

                    Next i
                End If

            End If

            MatrixRecosteo.Matrix.LoadFromDataSource()
            FormularioSBO.Freeze(False)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

            Return False
        End Try
    End Function

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim strSeleccionTodas As String = ""
            Dim strUnidad As String = ""
            Dim oMatrix As SAPbouiCOM.Matrix = Nothing
            Dim oForm As SAPbouiCOM.Form

            oForm = _applicationSbo.Forms.Item(FormUID)

            If Not oForm Is Nothing Then
                If pVal.BeforeAction Then

                ElseIf pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case "btnActuali"
                            If blnVehiculosSinCostear = True Then
                                oMatrix = DirectCast(oForm.Items.Item("mtx_VehSin").Specific, SAPbouiCOM.Matrix)
                            ElseIf blnVehiculosRecosteo = True Then
                                oMatrix = DirectCast(oForm.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix)
                            End If
                            
                            If Not oMatrix Is Nothing Then
                                If blnVehiculosSinCostear = True Then
                                    Call CargaFormularioCosteoMultiplesUnidades()
                                ElseIf blnVehiculosRecosteo = True Then
                                    Call CargaFormularioListadoGR()
                                End If
                            End If
                        Case CheckBoxSelTodas.UniqueId
                            FormularioSBO.Freeze(True)
                            strSeleccionTodas = CheckBoxSelTodas.ObtieneValorUserDataSource()
                            MatrixSinCostear.Matrix.FlushToDataSource()

                            If dataTableSinCostear.Rows.Count > 0 Then
                                strUnidad = dataTableSinCostear.GetValue("unidad", 0)

                                If Not String.IsNullOrEmpty(strUnidad) AndAlso Not strUnidad = "0" Then
                                    For i As Integer = 0 To dataTableSinCostear.Rows.Count - 1
                                        If strSeleccionTodas = "Y" Then
                                            dataTableSinCostear.SetValue("seleccion", i, "Y")
                                        ElseIf strSeleccionTodas = "N" Then
                                            dataTableSinCostear.SetValue("seleccion", i, "N")
                                        End If
                                    Next
                                    MatrixSinCostear.Matrix.LoadFromDataSource()
                                End If
                            End If
                            FormularioSBO.Freeze(False)
                        Case CheckBoxSelRecost.UniqueId
                            FormularioSBO.Freeze(True)

                            strSeleccionTodas = CheckBoxSelRecost.ObtieneValorUserDataSource()
                            MatrixRecosteo.Matrix.FlushToDataSource()
                            If dataTableRecosteo.Rows.Count > 0 Then
                                strUnidad = dataTableRecosteo.GetValue("unidad", 0)
                                If Not String.IsNullOrEmpty(strUnidad) AndAlso Not strUnidad = "0" Then
                                    For j As Integer = 0 To dataTableRecosteo.Rows.Count - 1
                                        If strSeleccionTodas = "Y" Then
                                            dataTableRecosteo.SetValue("seleccion", j, "Y")
                                        ElseIf strSeleccionTodas = "N" Then
                                            dataTableRecosteo.SetValue("seleccion", j, "N")
                                        End If
                                    Next j
                                    MatrixRecosteo.Matrix.LoadFromDataSource()
                                End If
                            End If
                            FormularioSBO.Freeze(False)
                        Case CheckBoxFacturadas.UniqueId
                            If CheckBoxFacturadas.ObtieneValorUserDataSource = "N" Then
                                oForm.Items.Item(EditTextFechaInicio.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                                oForm.Items.Item(EditTextFechaFin.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            ElseIf CheckBoxFacturadas.ObtieneValorUserDataSource = "Y" Then
                                oForm.Items.Item(EditTextFechaInicio.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                                oForm.Items.Item(EditTextFechaFin.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                            End If
                    End Select
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosChooseFromList(ByVal FormUID As String, _
                                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                 ByRef BubbleEvent As Boolean)
        Try

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim strCFL_Id As String

            Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)

            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(strCFL_Id)

            If oCFLEvent.ActionSuccess Then

                Dim oDataTable As SAPbouiCOM.DataTable
                oDataTable = oCFLEvent.SelectedObjects

                If Not oCFLEvent.SelectedObjects Is Nothing Then
                    If Not oDataTable Is Nothing And FormularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then
                        Select Case pVal.ItemUID
                            Case EditTextRecepcion.UniqueId
                                EditTextRecepcion.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0).ToString)

                            Case EditTextPedido.UniqueId
                                EditTextPedido.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0).ToString)
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosTab(ByRef oTmpForm As SAPbouiCOM.Form, _
                                ByRef pval As SAPbouiCOM.ItemEvent)


        If pval.ItemUID = mc_strFolder1 Then

            FormularioSBO.Freeze(True)

            CargaFormularioCosteoMultiplesUnidades()
            FormularioSBO.PaneLevel = 1

            FormularioSBO.Freeze(False)

        ElseIf pval.ItemUID = mc_strFolder2 Then

            FormularioSBO.Freeze(True)

            CargaFormularioListadoGR()
            FormularioSBO.PaneLevel = 2

            FormularioSBO.Freeze(False)

        End If

    End Sub

    Private Sub AgregaCFLRecepcion(ByVal p_strTextId As String, ByVal p_strCFLId As String, ByVal p_strAlias As String)

        Dim oItem As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText

        oItem = FormularioSBO.Items.Item(p_strTextId)

        oEdit = oItem.Specific

        oEdit.ChooseFromListUID = p_strCFLId
        oEdit.ChooseFromListAlias = p_strAlias

    End Sub

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form,
                                  ByVal p_strTipoObjeto As String,
                                  ByVal p_strCFLId As String)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            oCFLs = oform.ChooseFromLists
            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams

            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = p_strTipoObjeto
            oCFLCreationParams.UniqueID = p_strCFLId
            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub EntradasMultiples(ByRef p_form As SAPbouiCOM.Form, ByRef p_matriz As SAPbouiCOM.Matrix, ByVal pval As SAPbouiCOM.ItemEvent)

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim matrixXml As String

        Dim ListaCodigoUnidad As Generic.IList(Of String) = New Generic.List(Of String)
        Dim blnUsaDimension As Boolean = False

        If blnVehiculosSinCostear = True Then

            p_matriz = DirectCast(p_form.Items.Item("mtx_VehSin").Specific, SAPbouiCOM.Matrix)

        ElseIf blnVehiculosRecosteo = True Then

            p_matriz = (DirectCast(p_form.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix))

        End If

        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0

        Try

            Dim strUtilizaCosteoAccesorios As String = Utilitarios.EjecutarConsulta("Select U_UsaAxC from dbo.[@SCGD_ADMIN]", _companySbo.CompanyDB, _companySbo.Server)

            If Not String.IsNullOrEmpty(strUtilizaCosteoAccesorios) Then
                If strUtilizaCosteoAccesorios = "Y" Then
                    blnUtilizaCosteoAccesorios = True
                Else
                    blnUtilizaCosteoAccesorios = False
                End If
            End If




            Dim strValorDimension As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] WITH (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

            If strValorDimension = "Y" Then
                blnUsaDimension = True
            End If

            If CargarTipoCambio(p_form) Then

                Dim fecha As Date
                Dim strFecha As String = EditTextFecha.ObtieneValorUserDataSource()
                If Not String.IsNullOrEmpty(strFecha) AndAlso Not strFecha = "0" Then
                    fecha = Date.ParseExact(strFecha, "yyyyMMdd", Nothing)
                    fecha = New Date(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0)
                End If

                For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                    Dim elementoSel As Xml.XmlNode
                    Dim elementoUnidad As Xml.XmlNode
                    Dim elementoMarca As Xml.XmlNode
                    Dim elementoEstilo As Xml.XmlNode
                    Dim elementoContrato As Xml.XmlNode
                    Dim elementoDocEntrada As Xml.XmlNode


                    elementoSel = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Sel']")
                    elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Unid']")
                    elementoMarca = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Mar']")
                    elementoEstilo = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Est']")
                    elementoContrato = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Cont']")
                    elementoDocEntrada = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_DocEn']")

                    counter = counter + 1


                    If Not elementoSel.InnerText = String.Empty And elementoSel.InnerText = "Y" Then

                        'verifico si la unidad es seleccionada varias veces en el recosteo
                        If Not ListaCodigoUnidad.Contains(elementoUnidad.InnerText) Then

                            If VerificarSaldoInicial(elementoUnidad.InnerText, fecha) Then

                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCosteoMultiple & " " & elementoUnidad.InnerText, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                CrearEntradas(p_form, counter, pval, elementoDocEntrada.InnerText, blnUsaDimension)

                            Else


                                m_strMonedaSistema = RetornarMonedaSistema()
                                m_strMonedaLocal = RetornarMonedaLocal()

                                If Utilitarios.ConsultaCosteos(elementoUnidad.InnerText, _companySbo.CompanyDB, _companySbo.Server, m_strMonedaSistema, m_strMonedaLocal, strUtilizaCosteoAccesorios) Then

                                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCosteoMultiple & " " & elementoUnidad.InnerText, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                    CrearEntradas(p_form, counter, pval, elementoDocEntrada.InnerText, blnUsaDimension)

                                Else

                                    _applicationSbo.MessageBox(My.Resources.Resource.LaUnidad & " " & elementoUnidad.InnerText & " " & My.Resources.Resource.MensajeCosteoMultipleUnidadSinCosteosPendientes, DefaultBtn:=1)


                                End If
                            End If
                            ListaCodigoUnidad.Add(elementoUnidad.InnerText)
                        End If

                    End If

                Next

                _applicationSbo.MessageBox(My.Resources.Resource.MensajeCosteoSatisfactorio, Btn1Caption:="OK")

                EditTextFecha.AsignaValorUserDataSource(Date.Now.ToString("yyyMMdd"))

            End If

        Catch ex As Exception
            ListaCodigoUnidad.Clear()
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

        Finally

            If blnVehiculosSinCostear = True Then
                CheckBoxSelTodas.AsignaValorUserDataSource("N")
                CargaFormularioCosteoMultiplesUnidades()
            ElseIf blnVehiculosRecosteo = True Then
                CheckBoxSelRecost.AsignaValorUserDataSource("N")
                CargaFormularioListadoGR()
            End If

            If ListaCodigoUnidad.Count <> 0 Then
                ListaCodigoUnidad.Clear()
            End If

        End Try

    End Sub

    Public Sub CrearEntradas(ByVal p_form As SAPbouiCOM.Form, ByVal p_counter As Integer, ByVal pval As SAPbouiCOM.ItemEvent, Optional ByVal p_Entrada As String = Nothing, _
                             Optional p_blnDimension As Boolean = False)

        Dim dt As Date
        Dim strUnidad As String = String.Empty
        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Try

            Dim strFecha As String = EditTextFecha.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strFecha) AndAlso Not strFecha = "0" Then
                dt = Date.ParseExact(strFecha, "yyyyMMdd", Nothing)
                dt = New Date(dt.Year, dt.Month, dt.Day, 0, 0, 0)
            End If

            If strMonedaLocal <> strMonedaSistema Then
                m_decTipoCambio = RetornarTipoCambioMoneda(m_strMonedaSistema, dt, m_strConectionString, False)
            Else
                m_decTipoCambio = 1
            End If


            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, _companySbo.CompanyDB, _companySbo.Server)
            udoEntrada = New SCG.DMSOne.Framework.UDOEntradaVehiculo(_companySbo)
            
            'inicializo la variable para la clase Costeo
            objCosteo = New CosteoCls(_companySbo, _applicationSbo, strMonedaLocal, strMonedaSistema, p_blnDimension)
            
            If blnVehiculosSinCostear = True Then
                strUnidad = dataTableSinCostear.GetValue("unidad", p_counter - 1)
            ElseIf blnVehiculosRecosteo = True Then
                strUnidad = dataTableRecosteo.GetValue("unidad", p_counter - 1)
            End If

            g_blnLineaAgregada = False

            '************************************************************
            objCosteo.CargarDataTableCosteoVehiculo(dataTableValoresCosteo, strUnidad, strFecha, udoEntrada, blnVehiculosSinCostear, blnVehiculosRecosteo, p_Entrada, blnUtilizaCosteoAccesorios, strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_decTipoCambio, dt)
            '************************************************************
            Exit Sub
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function VerificarSaldoInicial(ByVal p_unidad As String, ByVal p_date As Date) As Boolean
        Dim dtsSaldosIni As New RecosteoDataSet
        Dim dtaSaldosIni As New RecosteoDataSetTableAdapters.SaldosInicialesTableAdapter

        Dim strConectionString As String = ""
        Dim cnConeccionBD As SqlClient.SqlConnection

        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _
                                             _companySbo.CompanyDB, _
                                             strConectionString)


        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString
        dtaSaldosIni.Connection = cnConeccionBD
        cnConeccionBD.Open()

        'Saldos iniciales
        dtaSaldosIni.SetTimeOut(240)
        dtaSaldosIni.FillSaldosIniciales(dtsSaldosIni.SaldosIniciales, strMonedaSistema, p_unidad, p_date, strMonedaLocal)

        If dtsSaldosIni.SaldosIniciales.Rows.Count > 0 Then

            cnConeccionBD.Close()
            Return True
        Else
            cnConeccionBD.Close()
            Return False
        End If

    End Function

    Public Sub Costeo(ByVal m_strUnidad As String, ByVal udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, ByVal p_date As Date)

        Dim decSaldoInicialLocal As Decimal
        Dim decSaldoInicialSistema As Decimal

        Dim decTotalesMonedaLocal As Decimal
        Dim decTotalesMonedaSistema As Decimal

        Dim strConectionString As String = ""
        Dim cnConeccionBD As SqlClient.SqlConnection

        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _
                                             _companySbo.CompanyDB, _
                                             strConectionString)


        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString


        dtaDocumentosFormularios.Connection = cnConeccionBD
        dtaAsientos.Connection = cnConeccionBD
        dtaSaldosIniciales.Connection = cnConeccionBD
        dtaAsientosSalidasInventario.Connection = cnConeccionBD
        dtaDocumentosFacturaClientes.Connection = cnConeccionBD
        dtaNotasCreditoProveedor.Connection = cnConeccionBD
        cnConeccionBD.Open()

        Try

            CIFLocal = 0
            CIFSistema = 0

            Dim strSeparadorDecimalesSAP As String = String.Empty
            Dim strSeparadorMilesSAP As String = String.Empty

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, _companySbo.CompanyDB, _companySbo.Server)

            Dim strSysCurrency As String = RetornarMonedaSistema()
            Dim strMainCurrency As String = RetornarMonedaLocal()

            'lleno el dataset con los formularios y sus transacciones
            If strUtilizaCosteoAccesorios = "Y" Then

                dtaDocumentosFormularios.SetTimeOut(240)
                dtaDocumentosFormularios.FillFormularios(dtsDocumentosFormularios.Formularios, m_strUnidad, strTipoDocumentoServicio, strTipoDocumentoArticulo, p_date)

                'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores
                dtaNotasCreditoProveedor.SetTimeOut(240)
                dtaNotasCreditoProveedor.FillNotasCreditoProveedor(dtsNotasCreditoProveedor.NotaCreditoProveedor, m_strUnidad, strTipoDocumentoServicio, strTipoDocumentoArticulo, p_date)
                'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores

            Else

                dtaDocumentosFormularios.SetTimeOut(240)
                dtaDocumentosFormularios.FillFormularios(dtsDocumentosFormularios.Formularios, m_strUnidad, strTipoDocumentoServicio, Nothing, p_date)

                'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores
                dtaNotasCreditoProveedor.SetTimeOut(240)
                dtaNotasCreditoProveedor.FillNotasCreditoProveedor(dtsNotasCreditoProveedor.NotaCreditoProveedor, m_strUnidad, strTipoDocumentoServicio, Nothing, p_date)
                'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores
            End If

            'MsgBox(dtaDocumentosFormularios.Adapter.SelectCommand.CommandTimeout)


            'se llena el DataSet con las facturas de clientes
            dtaDocumentosFacturaClientes.SetTimeOut(240)
            dtaDocumentosFacturaClientes.FillFacturaClientes(dtsDocumentosFacturaClientes.FacturaClientes, m_strUnidad, p_date)

            'Saldos iniciales
            dtaSaldosIniciales.SetTimeOut(240)
            dtaSaldosIniciales.FillSaldosIniciales(dtsSaldosIniciales.SaldosIniciales, strMonedaSistema, m_strUnidad, p_date, strMonedaLocal)


            dtaAsientos.SetTimeOut(240)
            dtaAsientos.FillAsientos(dtsAsientos.Asientos, strMainCurrency, strSysCurrency, m_strUnidad, p_date, strMonedaLocal)
            'MsgBox(dtaAsientos.Adapter.SelectCommand.CommandTimeout)

            'lleno el dataset con los Asientos generados en Salidas de inventario
            dtaAsientosSalidasInventario.SetTimeOut(240)
            dtaAsientosSalidasInventario.FillAsientoSalidaInventario(dtsAsientosSalidasInventario.AsientoSalidaInventario, m_strUnidad, p_date)


            For Each drw As RecosteoDataSet.SaldosInicialesRow In dtsSaldosIniciales.SaldosIniciales
                If drw.MonedaRegistro = m_strMonedaLocal Then
                    decSaldoInicialLocal = drw.Local
                Else
                    decSaldoInicialSistema = drw.Systema
                End If
            Next

            If dtsDocumentosFormularios.Formularios.Rows.Count > 0 Then

                For Each drwDocumentosF In dtsDocumentosFormularios.Formularios.Rows

                    VerificarItem(drwDocumentosF, dtsDocumentosFormularios)

                Next

            End If

            'factura de clientes
            If dtsDocumentosFacturaClientes.FacturaClientes.Rows.Count > 0 Then

                For Each drwDocumentosFacturaClientes In dtsDocumentosFacturaClientes.FacturaClientes.Rows

                    VerificarItem(drwDocumentosFacturaClientes, dtsDocumentosFacturaClientes)

                Next

            End If

            'Agregado Erick Sanabria 28.09.2012 
            If dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows.Count > 0 Then
                For Each drwNotaCreditoProveedor In dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows
                    VerificarItem(drwNotaCreditoProveedor, dtsNotasCreditoProveedor)
                Next
            End If
            'Agregado Erick Sanabria 28.09.2012 

            If dtsAsientos.Asientos.Rows.Count > 0 Then

                For Each drwAsientos In dtsAsientos.Asientos.Rows
                    VerificarItem(drwAsientos, dtsAsientos)
                Next

            End If

            If dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows.Count > 0 Then

                For Each drwAsientosSalidas In dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows
                    VerificarItem(drwAsientosSalidas, dtsAsientosSalidasInventario, True, "TALLER")
                Next

            End If


            decTotalesMonedaLocal = CalcularMontosTotales(ListaCantidadLocal)
            decTotalesMonedaLocal += decSaldoInicialLocal

            decTotalesMonedaSistema = CalcularMontosTotales(ListaCantidadSistema)
            decTotalesMonedaSistema += decSaldoInicialSistema

            Agregar_a_Campos(ListaNombreTransaccionLocal, udoEntrada, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema, ListaMontoMoneda)
            Agregar_a_Campos(ListaNombreTransaccionSistema, udoEntrada, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema)

            AgregarTotales(udoEntrada, CIFLocal, CIFSistema, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema, m_strUnidad)

            For Each drwSaldoInicial In dtsSaldosIniciales.SaldosIniciales.Rows

                AgregarLineaCosto(drwSaldoInicial.TransID, drwSaldoInicial.Memo, drwSaldoInicial.Rate, drwSaldoInicial.Local, drwSaldoInicial.Systema, "", "", drwSaldoInicial.MonedaRegistro, udoEntrada, "")

            Next

            For Each drwDocumentosF In dtsDocumentosFormularios.Formularios.Rows

                If drwDocumentosF.IsFCNull Then
                    AgregarLineaCosto(drwDocumentosF.TransId, drwDocumentosF.Memo, drwDocumentosF.Rate, drwDocumentosF.Local, drwDocumentosF.Systema, drwDocumentosF.FP, "", drwDocumentosF.MonedaRegistro, udoEntrada, drwDocumentosF.AcctCode)

                Else
                    AgregarLineaCosto(drwDocumentosF.TransId, drwDocumentosF.Memo, drwDocumentosF.Rate, drwDocumentosF.Local, drwDocumentosF.Systema, drwDocumentosF.FP, drwDocumentosF.FC, drwDocumentosF.MonedaRegistro, udoEntrada, drwDocumentosF.AcctCode)

                End If

            Next

            For Each drwDocumentosFacturaClientes In dtsDocumentosFacturaClientes.FacturaClientes.Rows

                If drwDocumentosFacturaClientes.IsFPNull Then
                    AgregarLineaCosto(drwDocumentosFacturaClientes.TransID, drwDocumentosFacturaClientes.Memo, drwDocumentosFacturaClientes.Rate, drwDocumentosFacturaClientes.Local, drwDocumentosFacturaClientes.Systema, "", drwDocumentosFacturaClientes.FC, drwDocumentosFacturaClientes.MonedaRegistro, udoEntrada, drwDocumentosFacturaClientes.AcctCode)

                Else
                    AgregarLineaCosto(drwDocumentosFacturaClientes.TransID, drwDocumentosFacturaClientes.Memo, drwDocumentosFacturaClientes.Rate, drwDocumentosFacturaClientes.Local, drwDocumentosFacturaClientes.Systema, drwDocumentosFacturaClientes.FP, drwDocumentosFacturaClientes.FC, drwDocumentosFacturaClientes.MonedaRegistro, udoEntrada, drwDocumentosFacturaClientes.AcctCode)

                End If

            Next

            'Agregado Erick Sanabria 28.09.2012  Agregar Lineas de Costo Notas de Crédito
            For Each drwNotaCreditoProveedor In dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows
                If drwNotaCreditoProveedor.IsFCNull Then
                    Call AgregarLineaCosto(drwNotaCreditoProveedor.TransId, drwNotaCreditoProveedor.Memo, drwNotaCreditoProveedor.Rate, drwNotaCreditoProveedor.Local, drwNotaCreditoProveedor.Systema, "", "", drwNotaCreditoProveedor.MonedaRegistro, udoEntrada, drwNotaCreditoProveedor.AcctCode)
                Else
                    Call AgregarLineaCosto(drwNotaCreditoProveedor.TransId, drwNotaCreditoProveedor.Memo, drwNotaCreditoProveedor.Rate, drwNotaCreditoProveedor.Local, drwNotaCreditoProveedor.Systema, drwNotaCreditoProveedor.FP, drwNotaCreditoProveedor.FC, drwNotaCreditoProveedor.MonedaRegistro, udoEntrada, drwNotaCreditoProveedor.AcctCode)
                End If
            Next
            'Agregado Erick Sanabria 28.09.2012  Agregar Lineas de Costo Notas de Crédito

            For Each drwAsientos In dtsAsientos.Asientos.Rows
                AgregarLineaCosto(drwAsientos.TransId, drwAsientos.Memo, drwAsientos.Rate, drwAsientos.Local, drwAsientos.Systema, "", "", drwAsientos.MonedaRegistro, udoEntrada, drwAsientos.AcctCode)

            Next

            For Each drwAsientosSalidas In dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows

                AgregarLineaCosto(drwAsientosSalidas.TransId, drwAsientosSalidas.Memo, drwAsientosSalidas.Rate, drwAsientosSalidas.Local, drwAsientosSalidas.Systema, "", "", drwAsientosSalidas.MonedaRegistro, udoEntrada, drwAsientosSalidas.AcctCode)

            Next


            LimpiarListas()

            cnConeccionBD.Close()
            '*************************************Fin Nuevo Recosteo*********************************

            Dim tLocal As Decimal
            Dim tLocalAsientos As Decimal

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Private Sub AgregarLineaCosto(ByVal p_strTransID As String, _
                                  ByVal p_strMemo As String, _
                                  ByVal p_decRate As Decimal, _
                                  ByVal p_decLocal As Decimal, _
                                  ByVal p_decSistema As Decimal, _
                                  ByVal p_strFacturaProveedor As String, _
                                  ByVal p_strFacturaCliente As String, _
                                  ByVal p_strMoneda As String, _
                                  ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, _
                                  ByVal p_strCuenta As String)

        If g_blnLineaAgregada = False Then

            udoEntrada.ListaLineas = New SCG.DMSOne.Framework.ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

            g_blnLineaAgregada = True

        End If

        Dim lineaEntrada As SCG.DMSOne.Framework.LineaUDOEntradaVehiculo = New SCG.DMSOne.Framework.LineaUDOEntradaVehiculo()

        lineaEntrada.Concepto = p_strMemo
        lineaEntrada.Cuenta = p_strCuenta
        lineaEntrada.Mon_Loc = p_decLocal
        lineaEntrada.Mon_Sis = p_decSistema
        lineaEntrada.Mon_Reg = p_strMoneda
        lineaEntrada.NoAsient = p_strTransID
        lineaEntrada.Tip_Cam = p_decRate
        lineaEntrada.Cuenta = p_strCuenta
        lineaEntrada.No_FC = p_strFacturaCliente
        lineaEntrada.NoFP = p_strFacturaProveedor
        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

    End Sub

    Private Sub AgregarTotales(ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, ByVal p_CIFLocal As Decimal, ByVal p_CIFSistema As Decimal, _
                                         ByVal p_strSeparadorDecimalesSAP As String, ByVal p_strSeparadorMilesSAP As String, _
                                         ByVal p_decSaldoInicialLocal As Decimal, ByVal p_decSaldoInicialSistema As Decimal, _
                                         ByVal p_decTotalLocal As Decimal, ByVal p_decTotalSistema As Decimal, _
                                         ByVal Unidad As String)



        Dim dcSumTotalL2 As Decimal = p_decTotalLocal + (p_decTotalSistema * m_decTipoCambio)
        Dim dcSumTotalL As Decimal = Utilitarios.ConvierteDecimal(dcSumTotalL2, n)

        Dim dcSumTotalS As Decimal = 0

        udoEntrada.Encabezado.GASTRA = dcSumTotalL

        If m_decTipoCambio <> 0 Then

            Dim SumTotalS As Decimal = p_decTotalSistema + (p_decTotalLocal / m_decTipoCambio)
            dcSumTotalS = Utilitarios.ConvierteDecimal(SumTotalS, n)
            udoEntrada.Encabezado.GASTRA_S = dcSumTotalS

        Else

            dcSumTotalS = Utilitarios.ConvierteDecimal(p_decTotalSistema, n)
            udoEntrada.Encabezado.GASTRA_S = dcSumTotalS

        End If

        Dim objGoodI As New GoodIssueCls(_applicationSbo, m_oCompany)

        Dim strCode = Utilitarios.EjecutarConsulta(String.Format("SELECT Code FROM [@SCGD_VEHICULO] with(nolock) WHERE U_Cod_Unid = '{0}'", Unidad), m_oCompany.CompanyDB, m_oCompany.Server)

        If Not String.IsNullOrEmpty(strCode) Then

            objGoodI.ActualizaCostoVehiculo(strCode, dcSumTotalS, dcSumTotalL, True)

        End If

        Dim strCIFLocal As Decimal = Utilitarios.ConvierteDecimal(p_CIFLocal, n)
        Dim strCIFSistema As Decimal = Utilitarios.ConvierteDecimal(p_CIFSistema, n)
        Dim strTipoCambio As Decimal = Utilitarios.ConvierteDecimal(m_decTipoCambio, n)

        Dim strSaldoInicialLocal As Decimal = Utilitarios.ConvierteDecimal(p_decSaldoInicialLocal, n)
        Dim strSaldoInicialSistema As Decimal = Utilitarios.ConvierteDecimal(p_decSaldoInicialSistema, n)
        Dim strTotalLocal As Decimal = Utilitarios.ConvierteDecimal(p_decTotalLocal, n)
        Dim strTotalSistema As Decimal = Utilitarios.ConvierteDecimal(p_decTotalSistema, n)

        udoEntrada.Encabezado.Cambio = strTipoCambio
        udoEntrada.Encabezado.CIF_L = strCIFLocal
        udoEntrada.Encabezado.CIF_S = strCIFSistema
        udoEntrada.Encabezado.VALHAC = strSaldoInicialLocal
        udoEntrada.Encabezado.VALHAC_S = strSaldoInicialSistema
        udoEntrada.Encabezado.Tot_Loc = strTotalLocal
        udoEntrada.Encabezado.Tot_Sis = strTotalSistema

    End Sub


    Private Sub Agregar_a_Campos(Of U As {Generic.IList(Of String)})(ByVal p As U, _
                                                             ByVal udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, _
                                                              ByVal p_strSeparadorDecimalesSAP As String, ByVal p_strSeparadorMilesSAP As String, _
                                                              ByVal p_decSaldoInicialLocal As Decimal, ByVal p_decSaldoInicialSistema As Decimal, _
                                                              ByVal p_decTotalLocal As Decimal, ByVal p_decTotalSistema As Decimal, _
                                                             Optional ByVal lista As U = Nothing)

        Dim p_strCampoNombreTrasaccion As String = String.Empty
        Dim p_decMontoLocal As Decimal
        Dim p_decMontoSistema As Decimal
        Dim Moneda As String = String.Empty

        If Not p.Count = 0 Then

            For i As Integer = 0 To p.Count - 1

                If Not lista Is Nothing Then
                    Moneda = lista.Item(i)
                End If

                If Moneda = strMonedaLocal Then

                    Dim s As String = ListaNombreTransaccionLocal.Item(i)

                    p_strCampoNombreTrasaccion = s

                    p_decMontoLocal = ListaCantidadLocal.Item(i)

                    Dim p_decMontoLocal1 As Decimal

                    p_decMontoLocal1 = Utilitarios.ConvierteDecimal(p_decMontoLocal, n)

                    Select Case p_strCampoNombreTrasaccion

                        Case "FOB"
                            udoEntrada.Encabezado.FOB = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "FLETE"
                            udoEntrada.Encabezado.FLETE = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "SEGFAC"
                            udoEntrada.Encabezado.SEGFAC = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "COMFOR"
                            udoEntrada.Encabezado.COMFOR = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "COMNEG"
                            udoEntrada.Encabezado.COMNEG = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "ACCINT"
                            udoEntrada.Encabezado.ACCINT = p_decMontoLocal1
                        Case "ACCEXT"
                            udoEntrada.Encabezado.ACCEXT = p_decMontoLocal1
                        Case "COMAPE" 'Comisión Apertura
                            udoEntrada.Encabezado.COMAPE = p_decMontoLocal1
                        Case "SEGLOC" 'Seguros locales
                            udoEntrada.Encabezado.SEGLOC = p_decMontoLocal1
                        Case "TRASLA" 'Traslado
                            udoEntrada.Encabezado.TRASLA = p_decMontoLocal1
                        Case "REDEST" 'Redestino
                            udoEntrada.Encabezado.REDEST = p_decMontoLocal1
                        Case "BODALM" 'Bodega almacen fiscal
                            udoEntrada.Encabezado.BODALM = p_decMontoLocal1
                        Case "DESALM" 'Desalmacenaje
                            udoEntrada.Encabezado.DESALM = p_decMontoLocal1
                        Case "IMPVTA" 'Impuesto
                            udoEntrada.Encabezado.IMPVTA = p_decMontoLocal1
                        Case "AGENCIA" 'Agencia
                            udoEntrada.Encabezado.AGENCIA = p_decMontoLocal1
                        Case "FLELOC" 'Flete Local
                            udoEntrada.Encabezado.FLELOC = p_decMontoLocal1
                        Case "RESERVA"   'Reserva
                            udoEntrada.Encabezado.RESERVA = p_decMontoLocal1
                        Case "OTROS_FP"
                            udoEntrada.Encabezado.OTROS = p_decMontoLocal1
                        Case "TALLER"
                            udoEntrada.Encabezado.TALLER = p_decMontoLocal1
                        Case "CIF"
                            udoEntrada.Encabezado.CIF_L = p_decMontoLocal1
                            CIFLocal = CIFLocal + p_decMontoLocal
                    End Select

                Else

                    Dim StrNombreTransaccion As String = ListaNombreTransaccionSistema.Item(i)

                    p_strCampoNombreTrasaccion = StrNombreTransaccion
                    p_decMontoSistema = ListaCantidadSistema.Item(i)

                    Dim p_decMontoSistema1 As String

                    p_decMontoSistema1 = Utilitarios.ConvierteDecimal(p_decMontoSistema, n)

                    Select Case p_strCampoNombreTrasaccion

                        Case "ACCINT"
                            udoEntrada.Encabezado.ACCINT_S = p_decMontoSistema1
                        Case "ACCEXT"
                            udoEntrada.Encabezado.ACCEXT_S = p_decMontoSistema1
                        Case "COMAPE" 'Comisión Apertura
                            udoEntrada.Encabezado.COMAPE_S = p_decMontoSistema1
                        Case "SEGLOC" 'Seguros locales
                            udoEntrada.Encabezado.SEGLOC_S = p_decMontoSistema1
                        Case "TRASLA" 'Traslado
                            udoEntrada.Encabezado.TRASLA_S = p_decMontoSistema1
                        Case "REDEST" 'Redestino
                            udoEntrada.Encabezado.REDEST_S = p_decMontoSistema1
                        Case "BODALM" 'Bodega almacen fiscal
                            udoEntrada.Encabezado.BODALM_S = p_decMontoSistema1
                        Case "DESALM" 'Desalmacenaje
                            udoEntrada.Encabezado.DESALM_S = p_decMontoSistema1
                        Case "IMPVTA" 'Impuesto
                            udoEntrada.Encabezado.IMPVTA_S = p_decMontoSistema1
                        Case "AGENCIA" 'Agencia
                            udoEntrada.Encabezado.AGENCI_S = p_decMontoSistema1
                        Case "FLELOC" 'Flete Local
                            udoEntrada.Encabezado.FLELOC_S = p_decMontoSistema1
                        Case "RESERVA"   'Reserva
                            udoEntrada.Encabezado.RESERV_S = p_decMontoSistema1
                        Case "FOB"
                            udoEntrada.Encabezado.FOB_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "FLETE"
                            udoEntrada.Encabezado.FLETE_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "SEGFAC"
                            udoEntrada.Encabezado.SEGFAC_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "COMFOR"
                            udoEntrada.Encabezado.COMFOR_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "COMNEG"
                            udoEntrada.Encabezado.COMNEG_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "OTROS_FP"
                            udoEntrada.Encabezado.OTROS_S = p_decMontoSistema1
                        Case "TALLER"
                            udoEntrada.Encabezado.TALLER_S = p_decMontoSistema1
                        Case "CIF"
                            udoEntrada.Encabezado.CIF_S = p_decMontoSistema1
                            CIFSistema = CIFSistema + p_decMontoSistema
                    End Select
                End If
            Next

        End If

    End Sub

    Private Sub LimpiarListas()
        ListaCodigoTransaccion.Clear()
        ListaMontoMoneda.Clear()
        ListaCantidadLocal.Clear()
        ListaNombreTransaccionLocal.Clear()

        ListaCodigoTransaccionSistema.Clear()
        ListaMontoMonedaSistema.Clear()
        ListaCantidadSistema.Clear()
        ListaNombreTransaccionSistema.Clear()
    End Sub

    Private Function CalcularMontosTotales(Of U As {Generic.IList(Of Decimal)})(ByVal p As U) As Decimal

        Dim decTotales As Decimal
        Dim decMonto As Decimal

        For i As Integer = 0 To p.Count - 1

            decMonto = p.Item(i)

            decTotales = decTotales + decMonto

        Next

        Return decTotales

    End Function

    Private Sub VerificarItem(Of U As {System.Data.DataRow})(ByRef p_drw As U, ByVal dtsF As RecosteoDataSet, Optional ByVal blnAsientoSalidaInventario As Boolean = False, Optional ByVal p_NombreTransaccion As String = "")

        Dim strCodigoTransaccion As String
        Dim strNombreTransaccion As String

        Dim strMoneda As String = p_drw.Item("MonedaRegistro")
        Dim decLocal As Decimal = p_drw.Item("Local")
        Dim decSistema As Decimal = p_drw.Item("Systema")


        If Not p_drw.Item("U_SCGD_Cod_Tran") Is DBNull.Value Then
            strCodigoTransaccion = p_drw.Item("U_SCGD_Cod_Tran")
        End If

        If Not p_drw.Item("NombreTransaccion") Is DBNull.Value Then
            strNombreTransaccion = p_drw.Item("NombreTransaccion")
        Else
            If blnAsientoSalidaInventario Then
                strNombreTransaccion = p_NombreTransaccion
            End If
        End If

        If strMoneda = strMonedaLocal Then

            If ListaNombreTransaccionLocal.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionLocal.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionLocal.Item(posit) = strNombreTransaccion And ListaMontoMoneda.Item(posit) = strMoneda Then
                    ListaCantidadLocal.Item(posit) = ListaCantidadLocal.Item(posit) + decLocal
                End If

            Else

                ListaCodigoTransaccion.Add(strCodigoTransaccion)
                ListaMontoMoneda.Add(strMoneda)
                ListaCantidadLocal.Add(decLocal)
                ListaNombreTransaccionLocal.Add(strNombreTransaccion)

            End If

        ElseIf strMoneda = strMonedaSistema Then

            If ListaNombreTransaccionSistema.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionSistema.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionSistema.Item(posit) = strNombreTransaccion And ListaMontoMonedaSistema.Item(posit) = strMoneda Then
                    ListaCantidadSistema.Item(posit) = ListaCantidadSistema.Item(posit) + decSistema
                End If
            Else
                ListaCodigoTransaccionSistema.Add(strCodigoTransaccion)
                ListaMontoMonedaSistema.Add(strMoneda)
                ListaCantidadSistema.Add(decSistema)
                ListaNombreTransaccionSistema.Add(strNombreTransaccion)

            End If

        Else
            If ListaNombreTransaccionSistema.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionSistema.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionSistema.Item(posit) = strNombreTransaccion Then
                    ListaCantidadSistema.Item(posit) = ListaCantidadSistema.Item(posit) + decSistema
                End If
            Else
                ListaCodigoTransaccionSistema.Add(strCodigoTransaccion)
                ListaMontoMonedaSistema.Add(strMoneda)
                ListaCantidadSistema.Add(decSistema)
                ListaNombreTransaccionSistema.Add(strNombreTransaccion)

            End If

        End If

    End Sub

    Public Sub DevolverDatosVehiculo(ByRef p_strUnidad As String, _
                                      ByRef p_strVIN As String, _
                                      ByRef p_strMarca As String, _
                                      ByRef p_strEstilo As String, _
                                      ByRef p_strModelo As String, _
                                      ByVal p_form As SAPbouiCOM.Form, _
                                      ByRef p_strIDVehiculo As String, _
                                      ByRef p_tipoVehiculo As String, _
                                      ByRef fila As Integer,
                                      ByRef p_strDocRecepcion As String,
                                      ByRef p_strDocPedido As String)

        Dim dataTable As DataTable
        Dim strConsulta As String

        Dim strTipoVendido As String = objConfiguracionGeneral.InventarioVehiculoVendido

        If fila > 0 Then

            p_strUnidad = dataTableSinCostear.GetValue("unidad", fila - 1)
            p_strMarca = dataTableSinCostear.GetValue("marca", fila - 1)
            p_strEstilo = dataTableSinCostear.GetValue("estilo", fila - 1)

            dataTable = FormularioSBO.DataSources.DataTables.Item("Veh")
            dataTable.Rows.Clear()

            strConsulta = "Select U_Des_Mode, U_Num_VIN, Code, U_Tipo, U_Tipo_Ven, U_DocRecepcion, U_DocPedido From dbo.[@SCGD_Vehiculo] with (nolock) Where U_Cod_Unid = '" + p_strUnidad + "'"

            dataTable.ExecuteQuery(strConsulta)

            p_strModelo = dataTable.GetValue("U_Des_Mode", 0)
            p_strVIN = dataTable.GetValue("U_Num_VIN", 0)
            p_strIDVehiculo = dataTable.GetValue("Code", 0)
            p_strDocRecepcion = dataTable.GetValue("U_DocRecepcion", 0)
            p_strDocPedido = dataTable.GetValue("U_DocPedido", 0)
            p_tipoVehiculo = dataTable.GetValue("U_Tipo", 0)

            If p_tipoVehiculo = strTipoVendido Then

                p_tipoVehiculo = dataTable.GetValue("U_Tipo_Ven", 0)

            End If

        End If

    End Sub

    Public Function RetornarMonedaLocal() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        Try

            oSBObob = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetLocalCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function

    Public Function RetornarMonedaSistema() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        oSBObob = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        oRecordset = oSBObob.GetSystemCurrency()
        strResult = oRecordset.Fields.Item(0).Value

        Return strResult

    End Function

    Private Function CargarTipoCambio(ByVal p_oform As SAPbouiCOM.Form) As Boolean

        Dim strConectionString As String = String.Empty
        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, strConectionString)

        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, strConectionString)

        m_objBLSBO.Set_Compania(_companySbo)
        strMonedaSistema = RetornarMonedaSistema()
        strMonedaLocal = RetornarMonedaLocal()
        If strMonedaLocal <> strMonedaSistema Then
            m_decTipoCambio = RetornarTipoCambioMoneda(strMonedaSistema, m_objUtilitarios.CargarFechaHoraServidor(), strConectionString, False)
            If m_decTipoCambio = -1 Then
                _applicationSbo.MessageBox(My.Resources.Resource.TipoCambioNoActualizado)
                Return False
            End If
        Else
            m_decTipoCambio = 1
        End If

        Return True

    End Function

    Public Function RetornarTipoCambioMoneda(ByVal Moneda As String, ByVal p_Hoy As Date, ByVal strConectionString As String, ByVal blnBDExterna As Boolean) As Decimal

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim sToday As String
        Dim dblResult As Double = -1

        Try
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()
            sToday = p_Hoy
            cmdEjecutarConsulta.Connection = cn_Coneccion

            cmdEjecutarConsulta.CommandType = CommandType.Text
            If blnBDExterna Then
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM SCGTA_VW_ORTT with(nolock) WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
            Else
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM ORTT with(nolock) WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"

            End If
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                    dblResult = drdResultadoConsulta.GetDecimal(0)
                    If dblResult = 0 Then dblResult = -1
                    Exit Do
                End If
            Loop
        Catch
            Throw
        Finally
            drdResultadoConsulta.Close()
            cmdEjecutarConsulta.Connection.Close()
        End Try
        Return dblResult
    End Function

#End Region

End Class


Public Class ListaUnidad

    Private strUnidad As String
    Public Property Unidad() As String
        Get
            Return strUnidad

        End Get
        Set(value As String)
            strUnidad = value
        End Set
    End Property


    Private strMarca As String
    Public Property Marca() As String
        Get
            Return strMarca

        End Get
        Set(value As String)
            strMarca = value
        End Set
    End Property

    Private strEstilo As String
    Public Property Estilo() As String
        Get
            Return strEstilo

        End Get
        Set(value As String)
            strEstilo = value
        End Set
    End Property

    Private strModelo As String
    Public Property Modelo() As String
        Get
            Return strModelo

        End Get
        Set(value As String)
            strModelo = value
        End Set
    End Property

    Private strVIN As String
    Public Property VIN() As String
        Get
            Return strVIN

        End Get
        Set(value As String)
            strVIN = value
        End Set
    End Property

    Private strIDVehiculo As String
    Public Property IDVehiculo() As String
        Get
            Return strIDVehiculo

        End Get
        Set(value As String)
            strIDVehiculo = value
        End Set
    End Property

    Private strDocRecepcion As String
    Public Property DocRecepcion() As String
        Get
            Return strDocRecepcion

        End Get
        Set(value As String)
            strDocRecepcion = value
        End Set
    End Property

    Private strtipoVehiculo As String
    Public Property TipoVehiculo() As String
        Get
            Return strtipoVehiculo

        End Get
        Set(value As String)
            strtipoVehiculo = value
        End Set
    End Property
End Class
