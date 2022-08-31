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
Imports SCG.DMSOne.Framework


Partial Public Class SalidaMultiple

#Region "Declaraciones"


    Private m_oCompany As SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private m_strDireccionConfiguracion As String

    Private Const mc_strUIDSalidaMultiple As String = "SCGD_SMU"
    Private Const mc_strUIDVehículos As String = "SCGD_MNO"

    'Nombres de campos del datasource
    Private Const mc_strIDContrato As String = "U_CTOVTA"
    Private Const mc_strMarca As String = "U_Des_Marc"
    Private Const mc_strEstilo As String = "U_Des_Esti"
    Private Const mc_strModelo As String = "U_Des_Mode"
    Private Const mc_strVIN As String = "U_Num_VIN"
    Private Const mc_strUnidad As String = "U_Cod_Unid"

    Private Const mc_Unidad As String = "U_Unidad"

    Private m_oFormSalidaMultiple As SAPbouiCOM.Form

    Private m_decTipoCambio As Decimal
    Private m_strMonedaLocal As String
    Private m_strMonedaSistema As String
    Public m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private strMonedaLocal As String = ""
    Private strMonedaSistema As String = ""

    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Private udoSalidaVehiculo As SCG.DMSOne.Framework.UDOSalidaVehiculo

    Private _dtEncabezado As SAPbouiCOM.DataTable


    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property

#End Region

#Region "MatrizRecosteo"

    Private Const mc_strGoodReceipts As String = "@SCGD_GOODRECEIVE"

    Private Const mc_strStatus As String = "Status"
    Private Const mc_strMarcaR As String = "U_Des_Marc"
    Private Const mc_strEstiloR As String = "U_Des_Esti"
    Private Const mc_strModeloR As String = "U_Des_Mode"
    Private Const mc_strVINR As String = "U_Num_VIN"
    Private Const mc_strAsientoEntrada As String = "U_As_Entr"
    Private Const mc_strUnidadR As String = "U_Unidad"
    Private Const mc_strGastra As String = "U_GASTRA"
    Private Const mc_strGastra_S As String = "U_GASTRA_S"


    Private m_oFormRecosteoMultiple As SAPbouiCOM.Form
    Private m_dbRecosteo As SAPbouiCOM.DBDataSource


#End Region

#Region "Metodos"

    Protected Friend Sub CargaFormularioSalidaMultiplesUnidades()

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

            Call CargarMatrixSalidas()

            strTipoParaTaller = objConfiguracionGeneral.InventarioVehiculoVendido

            m_oCompany = _companySbo

            ocombo = DirectCast(FormularioSBO.Items.Item("cboTipo").Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, "Select Code,Name From [@SCGD_TIPOVEHICULO] where Code <> '" & strTipoParaTaller.Trim & "' Order by Name")

            EditTextFecha.AsignaValorUserDataSource(Date.Now.ToString("yyyMMdd"))

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Private Function EnlazaColumnasMatrixaDatasourceRecosteo(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try

            oColumna = oMatrix.Columns.Item("col_Sel")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "Instance")
            oColumna.ValOff = "0"
            oColumna.ValOn = "1"

            oColumna = oMatrix.Columns.Item("col_DocEn")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "DocEntry")

            oColumna = oMatrix.Columns.Item("col_Unid")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, mc_strUnidadR)

            oColumna = oMatrix.Columns.Item("col_Mar")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_Marca")

            oColumna = oMatrix.Columns.Item("col_Est")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_Estilo")

            oColumna = oMatrix.Columns.Item("col_VIN")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_VIN")

            oColumna = oMatrix.Columns.Item("col_ID_V")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_ID_Vehiculo")

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

            Return False

        End Try
    End Function

    Public Function CargarMatrixSalidas() As Boolean

        Dim strUnidad As String
        Dim strTipoParaTaller As String
        Dim strNoDisponible As String
        Dim blnUnidadesFacturadas As Boolean = False
        Dim strTipo As String
        Dim strSeleccionTodas As String
        Dim strDocEntryDT As String
        Dim strUnidadDT As String
        Dim strMarcaDT As String
        Dim strEstiloDT As String
        Dim strVINDT As String
        Dim strIDVeh As String
        Dim DecGastra As Decimal
        Dim DecGastra_S As Decimal
        Dim dataTableSA As DataTable
        Dim strSeparadorDecimalesSAP As String = ""
        Dim strSeparadorMilesSAP As String = ""
        Dim strValorSeleccionado As String

        Dim strFechaInicioFormateada As String
        Dim strFechaFinFormateada As String

        Dim strFechaInicio As String
        Dim strFechaFin As String
        Dim strDocRecepcion As String

        Dim dtFechaInicio As Date
        Dim dtFechaFin As Date

        Dim n As NumberFormatInfo

        n = DIHelper.GetNumberFormatInfo(_companySbo)


        Try

            'Dim consulta As String = "Select DocEntry,U_Unidad,U_Marca,U_Estilo,U_VIN,U_ID_Vehiculo From [@SCGD_GOODRECEIVE] Where " & mc_strStatus & "='O'  And " & mc_strAsientoEntrada & " Is Not Null And " & mc_strAsientoEntrada & "<>-1"
            Dim consulta As String = " Select U_Unidad, U_Marca, U_Estilo, U_VIN, U_ID_Vehiculo, god.U_Tipo, SUM(god.U_GASTRA) AS SUMGASTRA , SUM(U_GASTRA_S) AS SUMGASTRA_S, god.U_DocRecep " & _
                                        " from [@SCGD_GOODRECEIVE] as god " & _
                                        " inner join [@SCGD_VEHICULO] as veh " & _
                                        " on god.U_Unidad = veh.U_Cod_Unid " & _
                                        " Where Status='O'  And U_As_Entr Is Not Null And U_As_Entr<>-1 "

            Dim agrupacionConsulta As String = " group by U_Unidad,U_Marca,U_Estilo,U_VIN,U_ID_Vehiculo, god.U_Tipo, U_DocRecep Order by U_Unidad"

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, _companySbo.CompanyDB, _companySbo.Server)

            FormularioSBO.Freeze(True)

            strUnidad = EditTextUnidad.ObtieneValorUserDataSource()
            strDocRecepcion = EditTextRecepcion.ObtieneValorUserDataSource()


            strNoDisponible = objConfiguracionGeneral.DisponibilidadVehiculoVendido
            strTipoParaTaller = objConfiguracionGeneral.InventarioVehiculoVendido

            If Not String.IsNullOrEmpty(strUnidad) Then
                consulta = String.Format(" {0} And god.U_Unidad like '%{1}%'", consulta, strUnidad)
            End If

            strTipo = ComboBoxTipo.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(strTipo) Then
                consulta = String.Format(" {0} And god.U_Tipo = '{1}'", consulta, strTipo)
            End If


            If CheckBoxFacturada.ObtieneValorUserDataSource = "Y" Then

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

                If Not String.IsNullOrEmpty(strFechaInicioFormateada) And Not String.IsNullOrEmpty(strFechaFinFormateada) Then
                    consulta = String.Format("{0} and veh.U_FechaVen >= '{1}' and veh.U_FechaVen <= '{2}' ",
                                      consulta, strFechaInicioFormateada, strFechaFinFormateada)
                End If
            End If

            If Not String.IsNullOrEmpty(strDocRecepcion) Then
                consulta = String.Format("{0} and veh.U_DocRecepcion = '{1}' ", consulta, strDocRecepcion)
            End If

            consulta += agrupacionConsulta

            MatrixSalidas.Matrix.Clear()
            dataTableSalidas.Rows.Clear()

            dataTableSA = FormularioSBO.DataSources.DataTables.Item("dtSA")
            dataTableSA.Rows.Clear()
            dataTableSA.ExecuteQuery(consulta)


            If dataTableSA.Rows.Count > 0 Then

                'strDocEntryDT = dataTableSA.GetValue("DocEntry", 0)

                ' If Not String.IsNullOrEmpty(strDocEntryDT) AndAlso Not strDocEntryDT = "0" Then

                strSeleccionTodas = CheckBoxSelTodas.ObtieneValorUserDataSource()

                For i As Integer = 0 To dataTableSA.Rows.Count - 1

                    ' strDocEntryDT = dataTableSA.GetValue("DocEntry", i)
                    strUnidadDT = dataTableSA.GetValue("U_Unidad", i)
                    strMarcaDT = dataTableSA.GetValue("U_Marca", i)
                    strEstiloDT = dataTableSA.GetValue("U_Estilo", i)
                    strVINDT = dataTableSA.GetValue("U_VIN", i)
                    strIDVeh = dataTableSA.GetValue("U_ID_Vehiculo", i)
                    DecGastra = dataTableSA.GetValue("SUMGASTRA", i)
                    DecGastra_S = dataTableSA.GetValue("SUMGASTRA_S", i)

                    dataTableSalidas.Rows.Add()

                    If strSeleccionTodas = "Y" Then
                        dataTableSalidas.SetValue("seleccion", i, "Y")
                    Else
                        dataTableSalidas.SetValue("seleccion", i, "N")
                    End If
                    'If Not String.IsNullOrEmpty(strDocEntryDT) And Not strDocEntryDT = "0" Then
                    '    dataTableSalidas.SetValue("entrada", i, strDocEntryDT)
                    'End If
                    If Not String.IsNullOrEmpty(strUnidadDT) And Not strUnidadDT = "0" Then
                        dataTableSalidas.SetValue("unidad", i, strUnidadDT)
                    End If
                    If Not String.IsNullOrEmpty(strMarcaDT) And Not strMarcaDT = "0" Then
                        dataTableSalidas.SetValue("marca", i, strMarcaDT)
                    End If
                    If Not String.IsNullOrEmpty(strEstiloDT) And Not strEstiloDT = "0" Then
                        dataTableSalidas.SetValue("estilo", i, strEstiloDT)
                    End If
                    If Not String.IsNullOrEmpty(strVINDT) And Not strVINDT = "0" Then
                        dataTableSalidas.SetValue("vin", i, strVINDT)
                    End If
                    If Not String.IsNullOrEmpty(strIDVeh) And Not strIDVeh = "0" Then
                        dataTableSalidas.SetValue("id", i, strIDVeh)
                    End If

                    If Not String.IsNullOrEmpty(DecGastra) And Not DecGastra = 0 Then

                        If strSeparadorDecimalesSAP <> "," Then
                            strValorSeleccionado = DecGastra
                            strValorSeleccionado = strValorSeleccionado.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                            dataTableSalidas.SetValue("Gastra", i, strValorSeleccionado)
                        Else
                            strValorSeleccionado = DecGastra
                            strValorSeleccionado = strValorSeleccionado.Replace(strSeparadorDecimalesSAP, strSeparadorMilesSAP)
                            dataTableSalidas.SetValue("Gastra", i, strValorSeleccionado)
                        End If

                    End If

                    If Not String.IsNullOrEmpty(DecGastra_S) And Not DecGastra_S = 0 Then

                        If strSeparadorDecimalesSAP <> "," Then
                            strValorSeleccionado = DecGastra_S
                            strValorSeleccionado = strValorSeleccionado.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                            dataTableSalidas.SetValue("Gastra_S", i, strValorSeleccionado)
                        Else
                            strValorSeleccionado = DecGastra_S
                            strValorSeleccionado = strValorSeleccionado.Replace(strSeparadorDecimalesSAP, strSeparadorMilesSAP)

                            dataTableSalidas.SetValue("Gastra_S", i, strValorSeleccionado)
                        End If

                    End If
                Next
                'End If
            End If

            MatrixSalidas.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

            Return False
        End Try
    End Function

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                  ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                  ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = _applicationSbo.Forms.Item(FormUID)

            Dim strSeleccionTodas As String
            Dim strDocEntry As String

            Dim strQuitarFiltro As String

            If Not oForm Is Nothing Then
                If pVal.BeforeAction Then
                    Select Case pVal.ItemUID

                    End Select
                ElseIf pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case ButtonActualizar.UniqueId
                            Call CargarMatrixSalidas()
                        Case CheckBoxSelTodas.UniqueId
                            FormularioSBO.Freeze(True)

                            strSeleccionTodas = CheckBoxSelTodas.ObtieneValorUserDataSource()

                            MatrixSalidas.Matrix.FlushToDataSource()

                            If dataTableSalidas.Rows.Count > 0 Then

                                For i As Integer = 0 To dataTableSalidas.Rows.Count - 1

                                    If strSeleccionTodas = "Y" Then

                                        dataTableSalidas.SetValue("seleccion", i, "Y")

                                    ElseIf strSeleccionTodas = "N" Then

                                        dataTableSalidas.SetValue("seleccion", i, "N")

                                    End If

                                Next

                                MatrixSalidas.Matrix.LoadFromDataSource()

                            End If

                            FormularioSBO.Freeze(False)
                        Case CheckBoxFacturada.UniqueId
                            If CheckBoxFacturada.ObtieneValorUserDataSource = "N" Then
                                oForm.Items.Item(EditTextFechaInicio.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                                oForm.Items.Item(EditTextFechaFin.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            ElseIf CheckBoxFacturada.ObtieneValorUserDataSource = "Y" Then
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

    Public Sub SetearCamposEncabezado()

        'Para linkear edittext de interfaz se utilizan datatables
        Dim datatable As SAPbouiCOM.DataTable = m_oFormSalidaMultiple.DataSources.DataTables.Add("Encabezado")
        datatable.Columns.Add(UID:="unidad", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="fechaCont", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_Date)
        datatable.Columns.Add(UID:="ckfacturado", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="cboTipo", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)

        datatable.Rows.Add(1)
        datatable.SetValue(Column:="unidad", rowIndex:=0, Value:="")
        datatable.SetValue(Column:="fechaCont", rowIndex:=0, Value:=Date.Now.ToString("yyyMMdd"))
        datatable.SetValue(Column:="ckfacturado", rowIndex:=0, Value:="N")
        datatable.SetValue(Column:="cboTipo", rowIndex:=0, Value:="")


        Dim item As Item
        Dim chk As CheckBox
        Dim txt As EditText
        Dim cb As ComboBox

        item = m_oFormSalidaMultiple.Items.Item("txtUnidad")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="Encabezado", columnUid:="unidad")

        item = m_oFormSalidaMultiple.Items.Item("txt_FecCon")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="Encabezado", columnUid:="fechaCont")

        item = m_oFormSalidaMultiple.Items.Item("chkFac")
        chk = DirectCast(item.Specific, CheckBox)
        chk.ValOff = "N"
        chk.ValOn = "Y"
        chk.DataBind.Bind(UID:="Encabezado", columnUid:="ckfacturado")

        item = m_oFormSalidaMultiple.Items.Item("cboTipo")
        cb = DirectCast(item.Specific, ComboBox)
        cb.DataBind.Bind(UID:="Encabezado", columnUid:="cboTipo")


        dtEncabezado = datatable

    End Sub

    Public Sub AgregarLineaTraslado(ByVal p_unidad As String, ByRef p_udoSalida As UDOSalidaVehiculo, ByRef blnLineaAgregada As Boolean)


        Dim dstSalidas As LineasSalidaDataset = New LineasSalidaDataset()

        Dim adpSalidas As LineasSalidaDatasetTableAdapters.SCGTA_TB_SalidasVehiculosTableAdapter = New LineasSalidaDatasetTableAdapters.SCGTA_TB_SalidasVehiculosTableAdapter()
        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, strConectionString)

        adpSalidas.CadenaConexion = strConectionString
        adpSalidas.Fill(dstSalidas.SCGTA_TB_SalidasVehiculos, p_unidad)

        Dim intPosicion As Integer = 0

        For Each salidaRow As LineasSalidaDataset.SCGTA_TB_SalidasVehiculosRow In dstSalidas.SCGTA_TB_SalidasVehiculos

            If blnLineaAgregada = False Then

                p_udoSalida.ListaLineas = New ListaLineasUDOSalidaVehiculo()
                p_udoSalida.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)
                blnLineaAgregada = True

            End If

            Dim LineaSalida As LineaUDOSalidaVehiculo = New LineaUDOSalidaVehiculo()

            LineaSalida.DocumentoEntrada = salidaRow.DocEntry
            LineaSalida.AsientoEntrada = salidaRow.U_As_Entr
            LineaSalida.MontoLocalEntrada = salidaRow.U_GASTRA
            LineaSalida.MontoSistemaEntrada = salidaRow.U_GASTRA_S
            intPosicion = intPosicion + 1
            p_udoSalida.ListaLineas.LineasUDO.Add(LineaSalida)


        Next

    End Sub

    Public Sub SalidasMultiples(ByRef p_form As SAPbouiCOM.Form, ByRef p_matriz As SAPbouiCOM.Matrix, ByVal pval As SAPbouiCOM.ItemEvent)

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim matrixXml As String

        Dim ListaCodigoUnidad As Generic.IList(Of String) = New Generic.List(Of String)


        p_matriz = (DirectCast(p_form.Items.Item("mtx_Recost").Specific, SAPbouiCOM.Matrix))

        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0

        Try

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
                    Dim elementoVIN As Xml.XmlNode
                    Dim elementoDocEntrada As Xml.XmlNode
                    Dim elementoID_Vehiculo As Xml.XmlNode

                    elementoSel = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Sel']")
                    elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Unid']")
                    elementoMarca = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Mar']")
                    elementoEstilo = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Est']")
                    elementoVIN = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_VIN']")
                    elementoDocEntrada = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_DocEn']")
                    elementoID_Vehiculo = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_ID_V']")

                    counter = counter + 1

                    If Not elementoSel.InnerText = String.Empty And elementoSel.InnerText = "Y" Then

                        'verifico si la unidad es seleccionada varias veces en el recosteo
                        If Not ListaCodigoUnidad.Contains(elementoUnidad.InnerText) Then

                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeSalidaMultiple & " " & elementoUnidad.InnerText, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

                            GenerarSalidaAutomatica(elementoUnidad.InnerText, elementoID_Vehiculo.InnerText, fecha)

                            ListaCodigoUnidad.Add(elementoUnidad.InnerText)

                        End If

                    End If

                Next

                _applicationSbo.MessageBox(My.Resources.Resource.MensajeSalidaSatisfactorio, Btn1Caption:="OK")

                EditTextFecha.AsignaValorUserDataSource(Date.Now.ToString("yyyMMdd"))

                If ListaCodigoUnidad.Count <> 0 Then
                    ListaCodigoUnidad.Clear()
                End If

            End If


        Catch ex As Exception
            ListaCodigoUnidad.Clear()
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)

        Finally

            CheckBoxSelTodas.AsignaValorUserDataSource("N")

            Call CargarMatrixSalidas()

        End Try

    End Sub

    Private Function GenerarSalidaAutomatica(ByVal p_CodUnidad As String, ByVal p_IDVeh As String, ByVal p_date As Date) As Boolean

        Dim strConectionString As String = ""
        Dim dtFecha As Date

        Dim n As NumberFormatInfo

        n = DIHelper.GetNumberFormatInfo(_companySbo)

        Dim cnConeccionBD As SqlClient.SqlConnection

        Configuracion.CrearCadenaDeconexion(_companySbo.Server, _
                                             _companySbo.CompanyDB, _
                                             strConectionString)

        Dim strSeleccionarEntrada As String = String.Empty

        Dim dstGoodIssue As New GoodIssueDataSet
        Dim dtaGoodIssue As New GoodIssueDataSetTableAdapters.SCG_GOODISSUETableAdapter
        Dim drwGoodIssue As GoodIssueDataSet.SCG_GOODISSUERow

        Dim dstGoodReceive As New GoodIssueDataSet
        Dim dtaGoodReceive As New GoodIssueDataSetTableAdapters.SCG_GOODRECEIVETableAdapter
        Dim drwGoodReceive As GoodIssueDataSet.SCG_GOODRECEIVERow

        Dim dtaONNM As New GoodIssueDataSetTableAdapters.ONNMTableAdapter


        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString
        cnConeccionBD.Open()
        dtaGoodIssue.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaGoodIssue.Connection = cnConeccionBD

        dtaGoodReceive.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaGoodReceive.Connection = cnConeccionBD

        dtaONNM.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaONNM.Connection = cnConeccionBD

        Try



            dtaGoodReceive.Fill_DatosVehiculo(dstGoodReceive.SCG_GOODRECEIVE, p_CodUnidad)


            If Not dstGoodReceive.SCG_GOODRECEIVE.Rows.Count = 0 Then


                drwGoodReceive = dstGoodReceive.SCG_GOODRECEIVE.Rows(0)

            Else
                'strSeleccionarEntrada = Utilitarios.EjecutarConsulta("select top(1) docentry from [@SCG_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and status = 'O' and U_As_Entr  is not null", m_oCompany.CompanyDB, m_oCompany.Server)
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.UnidadSinEntrada, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Exit Function

            End If



            Dim strValorSeleccionado As String
            Dim strSeparadorDecimalesSAP As String = ""
            Dim strSeparadorMilesSAP As String = ""
            Dim strIDFactura As String
            Dim strFecha As String = String.Empty
            Dim strAño As String = String.Empty
            Dim strMes As String = String.Empty
            Dim strDia As String = String.Empty

            Dim strcantidadGoodReceipts As String
            Dim intCantidad As Integer
            'Dim FechaActual As String

            Dim AñoFechaCorte As String = ""
            Dim MesFechaCorte As String = ""
            Dim DiaFechaCorte As String = ""
            Dim HoraCreacion As String = ""


            '        Dim m_strUnidad As String
            Dim m_strIDVehiculo As String

            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oChild As SAPbobsCOM.GeneralData
            Dim oChildren As SAPbobsCOM.GeneralDataCollection

            Dim oCompanyServiceEntrada As SAPbobsCOM.CompanyService
            Dim oGeneralServiceEntrada As SAPbobsCOM.GeneralService
            Dim oGeneralDataEntrada As SAPbobsCOM.GeneralData
            Dim oGeneralParamsEntrada As SAPbobsCOM.GeneralDataParams

            Dim strMontoSistema As String

            udoSalidaVehiculo = New UDOSalidaVehiculo(_companySbo)

            udoSalidaVehiculo.Encabezado = New EncabezadoUDOSalidaVehiculo()

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, _companySbo.CompanyDB, _companySbo.Server)

            Dim decMontoLocal As Decimal
            Dim decMontoSistema As Decimal

            drwGoodIssue = dstGoodIssue.SCG_GOODISSUE.NewSCG_GOODISSUERow

            '************************************************************************
            'se agrego para validar las transacciones a la hora de crear las entradas
            'del vehiculo
            udoSalidaVehiculo.Company.StartTransaction()
            '***********************************************************************

            With drwGoodIssue



                Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, _companySbo.CompanyDB, _companySbo.Server)

                'Dim strSeleccionarEntrada As String = Utilitarios.EjecutarConsulta("select top(1) docentry from [@SCG_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and status = 'O' and U_As_Entr  is not null", m_oCompany.CompanyDB, m_oCompany.Server)
                'm_strUnidad = Utilitarios.EjecutarConsulta("Select U_Unidad from [@SCG_GOODRECEIVE] where DocEntry = " & m_strIDEntrada, m_oCompany.CompanyDB, m_oCompany.Server)

                AñoFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(YEAR,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] where U_Unidad = '" & p_CodUnidad & "' order by DocEntry DESC", _companySbo.CompanyDB, _companySbo.Server)
                MesFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(MONTH,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] where U_Unidad = '" & p_CodUnidad & "' order by DocEntry DESC", _companySbo.CompanyDB, _companySbo.Server)
                DiaFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(DAY,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] where U_Unidad = '" & p_CodUnidad & "' order by DocEntry DESC", _companySbo.CompanyDB, _companySbo.Server)
                HoraCreacion = Utilitarios.EjecutarConsulta("Select Case len(cast(CreateTime as nvarchar)) when 3 then '0' + Substring(cast(CreateTime as nvarchar),0,2) + ':' + Substring(cast(CreateTime as nvarchar),2,4) + ':59' " & _
                                                       "when 4 then Substring(cast(CreateTime as nvarchar),0,3) + ':' + Substring(cast(CreateTime as nvarchar),3,4) + ':59' when 1 then '00:0' + cast(CreateTime as nvarchar) + ':59' " & _
                                                       "else '00:' + cast(CreateTime as nvarchar) + ':59' end as Hora from [@SCGD_GOODISSUE] where U_Unidad = '" & p_CodUnidad & "' order by DocEntry DESC", _companySbo.CompanyDB, _companySbo.Server)


                If MesFechaCorte.Length = 1 Then
                    MesFechaCorte = "0" & MesFechaCorte
                End If

                If DiaFechaCorte.Length = 1 Then
                    DiaFechaCorte = "0" & DiaFechaCorte
                End If

                If String.IsNullOrEmpty(AñoFechaCorte) Then
                    strcantidadGoodReceipts = dtaGoodReceive.QueryCantidadLineas(p_CodUnidad)

                Else
                    Dim fecha As Date = Date.ParseExact(AñoFechaCorte & MesFechaCorte & DiaFechaCorte, "yyyyMMdd", Nothing) '& "" & HoraCreacion, "yyyyMMdd", Nothing)
                    strcantidadGoodReceipts = dtaGoodReceive.QueryCantidadLineasFecha(p_CodUnidad, fecha)
                End If

                '.U_Unidad = p_CodUnidad
                udoSalidaVehiculo.Encabezado.CodigoUnidad = p_CodUnidad

                If IsNumeric(strcantidadGoodReceipts) Then
                    intCantidad = CInt(strcantidadGoodReceipts)
                End If

                If intCantidad <= 1 Then
                    '.U_Doc_Entr = drwGoodReceive.DocEntry
                    udoSalidaVehiculo.Encabezado.DocumentoEntrada = drwGoodReceive.DocEntry
                End If

                If Not drwGoodReceive.IsU_MarcaNull Then
                    '.U_Marca =
                    udoSalidaVehiculo.Encabezado.Marca = drwGoodReceive.U_Marca
                End If

                If Not drwGoodReceive.IsU_EstiloNull Then
                    ' .U_Estilo = 
                    udoSalidaVehiculo.Encabezado.Estilo = drwGoodReceive.U_Estilo
                End If

                If Not drwGoodReceive.IsU_ModeloNull Then
                    ' .U_Modelo = 
                    udoSalidaVehiculo.Encabezado.Modelo = drwGoodReceive.U_Modelo
                End If

                If intCantidad <= 1 Then

                    '.U_As_Entr = 
                    udoSalidaVehiculo.Encabezado.AsientoEntrada = drwGoodReceive.U_As_Entr
                End If

                If Not drwGoodReceive.IsU_VINNull Then
                    ' drwGoodIssue.U_VIN = 
                    udoSalidaVehiculo.Encabezado.VIN = drwGoodReceive.U_VIN
                End If

                If intCantidad <= 1 Then
                    strValorSeleccionado = Utilitarios.EjecutarConsulta("Select U_GASTRA from [@SCGD_GOODRECEIVE] where DocEntry = " & drwGoodReceive.DocEntry, _companySbo.CompanyDB, _companySbo.Server)
                    '.U_Cos_Loc = strValorSeleccionado
                    'decMontoLocal = Utilitarios.ConvierteDecimal(strValorSeleccionado, n)
                    'decMontoLocal = (Utilitarios.CambiarValoresACultureActual(strValorSeleccionado, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decMontoLocal = Decimal.Parse(strValorSeleccionado)
                    udoSalidaVehiculo.Encabezado.CostoMonedaLocal = decMontoLocal ' strValorSeleccionado '.U_Cos_Loc
                Else
                    strValorSeleccionado = Utilitarios.EjecutarConsulta("Select SUM(U_GASTRA) U_Tot_Loc from [@SCGD_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and U_SCGD_Trasl = 'N' and  U_Fec_Cont >= '" & AñoFechaCorte & MesFechaCorte & DiaFechaCorte & "' and (U_SCGD_DocSalida is null or U_SCGD_DocSalida = '')", _companySbo.CompanyDB, _companySbo.Server) 'strValorSeleccionado = Utilitarios.EjecutarConsulta("Select SUM(U_GASTRA) U_Tot_Loc from [@SCGD_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and U_SCGD_Trasl = 'N' and  U_Fec_Cont >= '" & AñoFechaCorte & MesFechaCorte & DiaFechaCorte & " " & HoraCreacion & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                    'decMontoLocal = (Utilitarios.CambiarValoresACultureActual(strValorSeleccionado, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    'decMontoLocal = Utilitarios.ConvierteDecimal(strValorSeleccionado, n)
                    decMontoLocal = Decimal.Parse(strValorSeleccionado)
                    udoSalidaVehiculo.Encabezado.CostoMonedaLocal = decMontoLocal 'strValorSeleccionado ' .U_Cos_Loc
                End If

                If intCantidad <= 1 Then
                    strValorSeleccionado = Utilitarios.EjecutarConsulta("Select U_GASTRA_S from [@SCGD_GOODRECEIVE] where DocEntry = " & drwGoodReceive.DocEntry, _companySbo.CompanyDB, _companySbo.Server)
                    ' decMontoSistema = (Utilitarios.CambiarValoresACultureActual(strValorSeleccionado, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decMontoSistema = Decimal.Parse(strValorSeleccionado)
                    'decMontoSistema = Utilitarios.ConvierteDecimal(strValorSeleccionado, n)
                    udoSalidaVehiculo.Encabezado.CostoMonedaSistema = decMontoSistema 'strValorSeleccionado '.U_Cos_Sis
                Else
                    strValorSeleccionado = Utilitarios.EjecutarConsulta("Select SUM(U_GASTRA_S) from [@SCGD_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and U_SCGD_Trasl = 'N' and U_Fec_Cont >= '" & AñoFechaCorte & MesFechaCorte & DiaFechaCorte & "' and (U_SCGD_DocSalida is null or U_SCGD_DocSalida = '')", _companySbo.CompanyDB, _companySbo.Server) 'strValorSeleccionado = Utilitarios.EjecutarConsulta("Select SUM(U_GASTRA) U_Tot_Loc from [@SCGD_GOODRECEIVE] where U_Unidad = '" & p_CodUnidad & "' and U_SCGD_Trasl = 'N' and  U_Fec_Cont >= '" & AñoFechaCorte & MesFechaCorte & DiaFechaCorte & " " & HoraCreacion & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                    'decMontoSistema = (Utilitarios.CambiarValoresACultureActual(strValorSeleccionado, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decMontoSistema = Decimal.Parse(strValorSeleccionado)
                    'decMontoSistema = Utilitarios.ConvierteDecimal(strValorSeleccionado, n)
                    udoSalidaVehiculo.Encabezado.CostoMonedaSistema = decMontoSistema 'strValorSeleccionado '.U_Cos_Sis
                End If

                udoSalidaVehiculo.Encabezado.NumeroVehiculo = p_IDVeh

                m_strIDVehiculo = p_IDVeh

                If m_strIDVehiculo <> "" Then

                    strValorSeleccionado = Utilitarios.EjecutarConsulta("Select U_CTOVTA from [@SCGD_VEHICULO] where Code = " & m_strIDVehiculo, _companySbo.CompanyDB, _companySbo.Server)

                    ' .U_NoCont = strValorSeleccionado
                    udoSalidaVehiculo.Encabezado.NumeroContrato = strValorSeleccionado '.U_NoCont

                    strIDFactura = Utilitarios.EjecutarConsulta("Select U_NUMFAC from [@SCGD_VEHICULO] where Code = " & m_strIDVehiculo, _companySbo.CompanyDB, _companySbo.Server)

                    ' .U_NoFact = strIDFactura
                    udoSalidaVehiculo.Encabezado.NumeroFactura = strIDFactura '.U_NoFact


                    strAño = Utilitarios.EjecutarConsulta("Select datepart(Year,DocDate) from OINV where Docentry = '" & strIDFactura & "'", _companySbo.CompanyDB, _companySbo.Server)
                    strMes = Utilitarios.EjecutarConsulta("Select datepart(Month,DocDate) from OINV where Docentry = '" & strIDFactura & "'", _companySbo.CompanyDB, _companySbo.Server)
                    If strMes.Length = 1 AndAlso Not String.IsNullOrEmpty(strMes) Then
                        strMes = "0" & strMes
                    End If

                    strDia = Utilitarios.EjecutarConsulta("Select datepart(Day,DocDate) from OINV where Docentry = '" & strIDFactura & "'", _companySbo.CompanyDB, _companySbo.Server)
                    If strDia.Length = 1 AndAlso Not String.IsNullOrEmpty(strDia) Then
                        strDia = "0" & strDia
                    End If
                    If Not strAño = String.Empty Or Not strMes = String.Empty Or Not strDia = String.Empty Then
                        dtFecha = New Date(strAño, strMes, strDia)
                    End If


                    If Not String.IsNullOrEmpty(dtFecha) Then

                        ' .U_Fech_Con = Convert.ToDateTime(dtFecha)
                        udoSalidaVehiculo.Encabezado.FechaContabilizacion = Convert.ToDateTime(p_date) '.U_Fech_Con
                    Else
                        strAño = Date.Now.Year.ToString
                        If Date.Now.Month.ToString.Length = 1 Then
                            strMes = "0" & Date.Now.Month.ToString
                        Else
                            strMes = Date.Now.Month.ToString
                        End If
                        If Date.Now.Day.ToString.Length = 1 Then
                            strDia = "0" & Date.Now.Day.ToString
                        Else
                            strDia = Date.Now.Day.ToString
                        End If

                        dtFecha = New Date(strAño, strMes, strDia)

                        ' .U_Fech_Con = Convert.ToDateTime(dtFecha)
                        udoSalidaVehiculo.Encabezado.FechaContabilizacion = Convert.ToDateTime(dtFecha) '.U_Fech_Con
                    End If
                Else
                    strAño = Date.Now.Year.ToString
                    If Date.Now.Month.ToString.Length = 1 Then
                        strMes = "0" & Date.Now.Month.ToString
                    Else
                        strMes = Date.Now.Month.ToString
                    End If
                    If Date.Now.Day.ToString.Length = 1 Then
                        strDia = "0" & Date.Now.Day.ToString
                    Else
                        strDia = Date.Now.Day.ToString
                    End If

                    dtFecha = New Date(strAño, strMes, strDia)

                    udoSalidaVehiculo.Encabezado.FechaContabilizacion = Convert.ToDateTime(dtFecha) '.U_Fech_Con
                End If

            End With


            AgregarLineaTraslado(p_CodUnidad, udoSalidaVehiculo, False)

            udoSalidaVehiculo.Insert()

            '***********************************************************************
            If udoSalidaVehiculo.Company.InTransaction Then
                udoSalidaVehiculo.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
            End If
            '***********************************************************************

            Dim intSalida As String = Convert.ToString(udoSalidaVehiculo.Encabezado.DocEntry)

            'Agregado 23/11/2010: Carga entradas de salida automatica y el campo de la salida en las entradas asociadas

            For Each drwGoodReceive In dstGoodReceive.SCG_GOODRECEIVE

                oCompanyServiceEntrada = _companySbo.GetCompanyService()
                oGeneralServiceEntrada = oCompanyServiceEntrada.GetGeneralService("SCGD_GOODENT")
                oGeneralParamsEntrada = oGeneralServiceEntrada.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsEntrada.SetProperty("DocEntry", drwGoodReceive.DocEntry)
                oGeneralDataEntrada = oGeneralServiceEntrada.GetByParams(oGeneralParamsEntrada)
                oGeneralDataEntrada.SetProperty("U_SCGD_DocSalida", CStr(intSalida))
                oGeneralServiceEntrada.Update(oGeneralDataEntrada)

            Next

            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            Dim m_oGoodIssue As New GoodIssueCls(_applicationSbo, _companySbo, objConfiguracionGeneral)
            m_oGoodIssue.CrearAsientoParaNumeroSalidaEspecifico(intSalida, Convert.ToDateTime(p_date))

            'If Not String.IsNullOrEmpty(m_strIDVehiculo) Then
            '    m_oGoodIssue.ActualizaCostoVehiculo(m_strIDVehiculo, decMontoSistema, decMontoLocal)
            'End If
        Catch ex As Exception

            If udoSalidaVehiculo.Company.InTransaction Then
                udoSalidaVehiculo.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Function




    Private Function CargarTipoCambio(ByVal p_oform As SAPbouiCOM.Form) As Boolean

        Dim strMoneda As String
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

    Public Function RetornarMonedaLocal() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
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
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        oSBObob = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        oRecordset = oSBObob.GetSystemCurrency()
        strResult = oRecordset.Fields.Item(0).Value

        Return strResult

    End Function

    Public Function RetornarTipoCambioMoneda(ByVal Moneda As String, ByVal p_Hoy As Date, ByVal strConectionString As String, ByVal blnBDExterna As Boolean) As Decimal

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Dim strValor As String = ""
        Dim sToday As String
        Dim dblResult As Double = -1

        Try
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()
            sToday = p_Hoy
            cmdEjecutarConsulta.Connection = cn_Coneccion

            cmdEjecutarConsulta.CommandType = CommandType.Text
            If blnBDExterna Then
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM SCGTA_VW_ORTT WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
            Else
                cmdEjecutarConsulta.CommandText = "SELECT Rate FROM ORTT WHERE Currency='" & Moneda & "'" & _
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
