Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.DMSOne.Framework
Imports SCG.SBOFramework.DI
Imports System.Collections.Generic
Imports System.Globalization




Public Class TrasladoCostosDeUnidadesCls


#Region "Declaraciones"

    Private m_oFormTraslados As SAPbouiCOM.Form
    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strUIDTraslados As String = "SCGD_TCU"

    Private Const m_Local As String = "L"
    Private Const m_Sistema As String = "S"

    Private WithEvents SBO_Application As SAPbouiCOM.Application

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDUnidad As String = "colUnidad"
    Private Const mc_strUIDInvOrig As String = "colInvOrig"
    Private Const mc_strUIDTotal As String = "colTotal"
    Private Const mc_strUIDCliente As String = "colCliente"

    Private m_dbTraslados As SAPbouiCOM.DBDataSource

    Private ListaTipoTotal As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaTotalLocalSitema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)

    Private ListaTotalMontos As Generic.IList(Of String) = New Generic.List(Of String)
    'Private ListaTotalMontoSistema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)

    'lista que almacen los docentry de las entradas a cambiar
    Private ListaEntradas As Generic.IList(Of String) = New Generic.List(Of String)

    Dim ListaDeEntradasActualizar As List(Of String)
    Dim blnSinTrasladar As Boolean = False


    Private intNumeroLinea As Integer
    Private intTipoInv As String

    Private m_dtsVehiculo As New VehiculosAddonDataset
    Private m_tadVehiculo As New VehiculosAddonDatasetTableAdapters.SCG_VEHICULOTableAdapter
    Private m_tadConsultasVehiculos As New VehiculosAddonDatasetTableAdapters.SCG_VEHICULOTableAdapter
    Private m_objGoodReceiptusado As ObjetoGoodReceiptCls

    Private m_cnConeccionTransaccion As New SqlClient.SqlConnection
    Private m_tnTransaccion As SqlClient.SqlTransaction

    Dim strConectionString As String = String.Empty


    Private m_dtsGoodReceipt As GoodReceiptDataset
    Private m_dttGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVEDataTable
    Private m_dttGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESDataTable
    Private m_dtrGoodReceipt As GoodReceiptDataset.__SCG_GOODRECEIVERow
    Private m_dtrGoodReceiptLines As GoodReceiptDataset.__SCG_GRLINESRow

    Private intDocEntryT As Integer
    Private intSerieT As Integer

    Private m_datFechaContabilizacion As Date
    Private m_decTipoCambio As String

    Dim strMonedaLocal As String = ""
    Dim strMonedaSistema As String = ""

    Private m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private docentryUDOTraslado As Integer

    Private intDocEntryEntradaVehiculo As Integer

    Dim objLineasEntradas As New Generic.List(Of ValoresTrasladoEntradas)
    Dim objValoresLineasEntrada As New ValoresTrasladoEntradas


    Dim ListaDocEntryEntradas As Generic.List(Of String) = New Generic.List(Of String)


    Private intEntradaParaAsiento As Integer

    Private intDocEntryEntrada As Integer

    'manejo de imprimir reportes
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
    Dim m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String

    Private udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo

    Private m_objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Public n As NumberFormatInfo

    Private ExisteDataSourceDimensiones As Boolean = False

    Private oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable

    Private ListaConfiguracionOT As Hashtable

    Public Const mc_strDataTableDimensionesOT As String = "DimensionesContablesDMS"
    
    Public ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls

    Private blnUsaDimensiones As Boolean = False

    Private dataTableEntradasPorUnidad As SAPbouiCOM.DataTable

    Private blnErrorCreacionAsiento As Boolean = False

    Public Structure Totales

        Dim tipo As String
        Dim total As Decimal

        Function Totales() As TrasladoCostosDeUnidadesDataSet.TotalesTodoDataTable
            Throw New NotImplementedException
        End Function

    End Structure

    Public Structure ValoresTrasladoEntradas

        Dim unidad As String
        Dim docEntrada As String
        Dim MontoSis As Decimal
        Dim MontoLocal As Decimal
        Dim Moneda As String
        Dim NoAsiento As String
        Dim TipoCambio As String

        'AgregarLinea("Traslado de Costos Vehiculo", drwTraslado.U_Mon_Sis, drwTraslado.U_Mon_Reg, drwTraslado.U_NoAsient, "", drwTraslado.U_Tip_Cam, ObjetoGoodReceiptCls.enumTipoCargo.CIF, udoEntrada, blnLineaAgregada)

    End Structure



    Public Enum enumTipoCargo

        ComisionApertura = 1
        SeguroLocal = 2
        FOB = 3
        Flete = 4
        SeguroFactura = 5
        ComisionFormalizacion = 6
        ComisionNegocion = 7
        CIF = 8
        Traslado = 8
        Redestino = 9
        BodegaAlmacenaje = 10
        Desalmacenaje = 11
        ImpuestoVenta = 12
        Agencia = 13
        Reserva = 14
        AccesoriosInternos = 15
        AccesoriosExternos = 16
        Otros = 17
        Taller = 18
        FleteLocal = 19
        SaldoInicial = 20

    End Enum


#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        m_datFechaContabilizacion = DMS_Connector.Helpers.GetDBServerDate
        n = DIHelper.GetNumberFormatInfo(p_oCompania)

    End Sub
#End Region


#Region "Metodos"


    Protected Friend Sub AddMenuItems()
        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu("SCGD_TCU", SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_TCU", SBO_Application.Language)
            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDTraslados, BoMenuType.mt_STRING, strEtiquetaMenu, 20, False, True, "SCGD_MNO"))
        End If
    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String
        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument
        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml
    End Function

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean
        Dim oColumna As SAPbouiCOM.Column
        Try
            oColumna = oMatrix.Columns.Item("colUnidad")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Cod")

            oColumna = oMatrix.Columns.Item("colMarca")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Mar")

            oColumna = oMatrix.Columns.Item("colEstilo")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Est")

            oColumna = oMatrix.Columns.Item("colVin")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Vin")

            oColumna = oMatrix.Columns.Item("colInvOrig")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_InO")

            oColumna = oMatrix.Columns.Item("colDesOrig")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_DsO")

            oColumna = oMatrix.Columns.Item("colInvDest")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Inv")

            oColumna = oMatrix.Columns.Item("colDescr")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Des")

            oColumna = oMatrix.Columns.Item("colCostoLo")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_Cos")

            oColumna = oMatrix.Columns.Item("colCostoSi")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_CSi")

            oColumna = oMatrix.Columns.Item("colNomCuOr")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_NCO")

            oColumna = oMatrix.Columns.Item("colNoCuDes")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_NCD")

            oColumna = oMatrix.Columns.Item("colFrmtCO")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_FCO")

            oColumna = oMatrix.Columns.Item("colFrmtCD")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_FCD")

            oColumna = oMatrix.Columns.Item("colEntrada")
            oColumna.DataBind.SetBound(True, "@SCGD_TR_COSTOLINEAS", "U_SCGD_EN")
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try
    End Function

    Protected Friend Sub CargaFormularioTrasladoCostos(Optional p_blnVieneKardex As Boolean = False, Optional p_DocEntryTRL As Integer = 0)
        Try
            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim ocolumnNC As SAPbouiCOM.Column
            Dim linkBtn As SAPbouiCOM.LinkedButton
            Dim strXMLACargar As String
            Dim oConditions As SAPbouiCOM.Conditions
            Dim oCondition As SAPbouiCOM.Condition
            Dim oItem As SAPbouiCOM.Item
            Dim oMatriz As SAPbouiCOM.Matrix
            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_TCU"
            strXMLACargar = My.Resources.Resource.TRASLForm
            fcp.XmlData = CargarDesdeXML(strXMLACargar)
            m_oFormTraslados = SBO_Application.Forms.AddEx(fcp)
            m_oFormTraslados.DataSources.DBDataSources.Add("@SCGD_TR_COSTOS")
            m_oFormTraslados.DataSources.DBDataSources.Add("@SCGD_TR_COSTOLINEAS")
            oItem = m_oFormTraslados.Items.Item("mtx_01")
            oMatrix = DirectCast(m_oFormTraslados.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto
            oMatriz = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)
            'se hace para que el campo txtDocEnt sirva de busqueda en el formulario
            Dim DocentryItem As SAPbouiCOM.Item = m_oFormTraslados.Items.Item("txtDocEnt")
            DocentryItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 15, BoModeVisualBehavior.mvb_False)
            DocentryItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoFormMode.fm_FIND_MODE, BoModeVisualBehavior.mvb_True)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_oFormTraslados.DataBrowser.BrowseBy = "txtDocEnt"
            If EnlazaColumnasMatrixaDatasource(oMatrix) Then
                m_oFormTraslados.Mode = BoFormMode.fm_FIND_MODE
                m_oFormTraslados.Items.Item("txtDocEnt").Enabled = True
                m_oFormTraslados.Items.Item("btnTras").Enabled = True
                m_oFormTraslados.Items.Item("btnAdd").Enabled = True
            End If
            Call CargarTipoCambio()
            Call CargarCombos(oMatriz)

            ValidarDataTable(m_oFormTraslados)

            If p_blnVieneKardex Then

                CargarTraslado(p_DocEntryTRL)

            End If
            Dim strTablaConsulta As String = "dtConsulta"
            'Dim strTablaConsultaAsientos As String = "dtConsultaAsientos"
            dataTableEntradasPorUnidad = m_oFormTraslados.DataSources.DataTables.Add(strTablaConsulta)


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ImprimeReporteTraslados(ByVal p_form As SAPbouiCOM.Form)
        Dim strID As String = ""
        'strID = obtenerId(p_form)
        strID = p_form.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("DocEntry", 0).Trim()
        If Not String.IsNullOrEmpty(strID) Then
            Call ImprimirReporte(My.Resources.Resource.rptTrasladoCostosUnidades, My.Resources.Resource.TituloReporteTraslados, strID)
        End If
    End Sub

    'Imprimir reportes
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try
            Dim strPathExe As String
            Dim strParametrosEjecutar As String
            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString

            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte & ".rpt"
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")
            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strParametrosEjecutar = strBarraTitulo + " " + strDireccionReporte + " " + m_oCompany.DbUserName + "," + CatchingEvents.DBPassword + "," +
                              m_oCompany.Server + "," + m_oCompany.CompanyDB + " " + strParametros

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    'retorna el id del vehiculo para imprimir el reporte
    Public Function obtenerId(ByVal p_form As SAPbouiCOM.Form) As String
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix
        Dim xmlDocMatrix As Xml.XmlDocument

        p_matriz = p_form.Items.Item("mtx_01").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim elementoUnidad As Xml.XmlNode
        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")
        Next
        If Not elementoUnidad Is Nothing Then

            If String.IsNullOrEmpty(elementoUnidad.InnerText) Then
                Return ""
            Else
                Return elementoUnidad.InnerText
            End If
        End If
        Return ""
    End Function

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                 ByVal FormUID As String, _
                                                 ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim strIDVehiculoUsado As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oeditDocentry As SAPbouiCOM.EditText

        Dim strUnidad As String
        Dim strTipoDestino As String
        Dim strTipOrigen As String
        Dim blnPermiteTransCero As Boolean = False

        oForm = SBO_Application.Forms.Item(FormUID)
        oMatrix = DirectCast(oForm.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)

        If oForm IsNot Nothing Then
            If pval.ItemUID = "1" Then
                oMatrix.FlushToDataSource()
                For i As Integer = 0 To oMatrix.RowCount - 1
                    strUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Cod", i)
                    strUnidad = strUnidad.Trim()
                    Dim clsTraslado As New TrasladoCostosDeUnidadesCls(SBO_Application, m_oCompany)
                    Dim lista As Generic.List(Of Decimal) = clsTraslado.DevolverCostosPorUnidad(strUnidad, True)
                    oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Cos", i, lista.Item(0))
                    oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_CSi", i, lista.Item(1))
                Next
                oMatrix.LoadFromDataSource()
            End If
            If pval.ItemUID = "mtx_01" AndAlso Not pval.BeforeAction Then
                'requiero el numero de linea para hacer el cambio en caso de que cambie el vehiculo
                intNumeroLinea = pval.Row
            End If
            If pval.ItemUID = "btnImp" AndAlso Not pval.BeforeAction Then
                'imprimo el reporte de traslado de costos
                If Not m_oFormTraslados Is Nothing Then
                    Call ImprimeReporteTraslados(m_oFormTraslados)
                End If
            End If
            If pval.ItemUID = "btnAdd" AndAlso Not pval.BeforeAction Then
                If oForm.Mode = BoFormMode.fm_FIND_MODE Then
                    Exit Sub
                End If
                Dim strRealizarTraslado As String = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("U_SCGD_TYN", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Offset)
                strRealizarTraslado.Trim("")
                Dim strCUnidad As String = ""
                If Not strRealizarTraslado = "Y" Then
                    oMatrix.FlushToDataSource()
                    If oMatrix.RowCount = 0 Then
                        oMatrix.AddRow(1, 1)
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").InsertRecord(0)
                    Else
                        Dim v As Integer = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset
                        strCUnidad = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Cod", v)
                        strCUnidad = strCUnidad.Trim()
                        'verifica que exista ya una unidad ingresada en la linea anterior
                        If Not String.IsNullOrEmpty(strCUnidad) Then
                            oMatrix.AddRow(1, intNumeroLinea + 1)
                            oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").InsertRecord(v + 1)
                            oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Cod", v + 1, "")
                            oMatrix.LoadFromDataSource()
                        End If
                    End If
                End If
            End If
            If pval.ItemUID = "btnTras" AndAlso Not pval.BeforeAction Then
                Dim l_strSQLAdmin As String
                Dim l_strPermiteTransCero As String
                l_strSQLAdmin = "Select U_TransCostCero FROM [@SCGD_ADMIN] WHERE Code = 'DMS'"
                l_strPermiteTransCero = Utilitarios.EjecutarConsulta(l_strSQLAdmin, m_oCompany.CompanyDB, m_oCompany.Server)
                If CargarTipoCambio(oForm) Then
                    strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
                    strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()
                    If oForm.Mode = BoFormMode.fm_FIND_MODE Then
                        Exit Sub
                    End If
                    If l_strPermiteTransCero = "Y" Then
                        blnPermiteTransCero = True
                    Else
                        blnPermiteTransCero = False
                    End If
                    If ValidarCostosDistintosACero(oForm, blnPermiteTransCero) Then
                        Exit Sub
                    End If
                    If ValidarCodigoUnidad(oForm) = True Then
                        Exit Sub
                    End If
                    strTipoDestino = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Inv", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset)
                    strTipoDestino.Trim()
                    strTipOrigen = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_InO", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset)
                    strTipOrigen.Trim()
                    If strTipOrigen = strTipoDestino Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCuentaTrasladoCostosSimilares, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Des", intNumeroLinea - 1, "")
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_NCD", intNumeroLinea - 1, "")
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_FCD", intNumeroLinea - 1, "")
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Inv", intNumeroLinea - 1, "")
                        strTipoDestino = String.Empty
                        strTipOrigen = String.Empty
                        Exit Sub
                    End If
                    Dim strRealizarTraslado As String = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("U_SCGD_TYN", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Offset)
                    strRealizarTraslado.Trim("")
                    If Not strRealizarTraslado = "Y" Then
                        If SBO_Application.MessageBox(My.Resources.Resource.MensajeTrasladoUnidades, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                            Try
                                If String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("DocEntry", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Offset)) Then
                                    Dim udoTraslado As UDOTrasladoCostos = New UDOTrasladoCostos(m_oCompany)

                                    Call EncabezadoCosto(udoTraslado, oForm)
                                Else
                                    docentryUDOTraslado = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("DocEntry", oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Offset)

                                End If
                               

                                ' ListaDeEntradasActualizar = LlenarListaEntradasTrasladada(oMatrix)

                                Call CrearTraslado(oForm, blnPermiteTransCero)

                                oForm.Mode = SAPbouiCOM.BoAutoFormMode.afm_Ok

                                oForm.Mode = BoFormMode.fm_ADD_MODE
                              
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeTrasladoSatisfactorio, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

                                '    'actualizo las entradas
                                'ActualizarEntradaTrasladada(ListaDeEntradasActualizar)
                                'ListaDeEntradasActualizar.Clear()

                            Catch ex As Exception
                                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
                            End Try
                        End If
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeUnidadesTrasladadas, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If
            End If
        End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosCombo(ByRef oTmpForm As SAPbouiCOM.Form, _
                                  ByVal pval As SAPbouiCOM.ItemEvent, _
                                  ByVal FormUID As String)
        Try


            Dim strValorSeleccionado As String = String.Empty

            Dim oMatriz As SAPbouiCOM.Matrix
            Dim strTipoDestino As String
            Dim strTipOrigen As String
            Dim ocboEPD As SAPbouiCOM.ComboBox

            Dim ocboEPO As SAPbouiCOM.ComboBox

            oMatriz = DirectCast(oTmpForm.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
            ocboEPD = DirectCast(oMatriz.Columns.Item("colInvDest").Cells.Item(intNumeroLinea).Specific, ComboBox)
            ocboEPO = DirectCast(oMatriz.Columns.Item("colInvOrig").Cells.Item(intNumeroLinea).Specific, ComboBox)

            'If ocboEPD.Value <> Nothing Then

            oMatriz.FlushToDataSource()

            'strTipoDestino = ocboEPD.Selected.Value
            strTipoDestino = oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Inv", intNumeroLinea - 1) 'oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset)
            strTipoDestino.Trim()
            ' strTipOrigen = ocboEPO.Selected.Value
            strTipOrigen = oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_InO", intNumeroLinea - 1) 'oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset)
            strTipOrigen.Trim()


            If strTipOrigen = strTipoDestino Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCuentaTrasladoCostosSimilares, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Des", intNumeroLinea - 1, "")
                oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_NCD", intNumeroLinea - 1, "")
                oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_FCD", intNumeroLinea - 1, "")
                oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Inv", intNumeroLinea - 1, "")
                oMatriz.LoadFromDataSource()
                strTipoDestino = String.Empty
                strTipOrigen = String.Empty
                Exit Sub
            End If

            Dim ctnStock As String

            ctnStock = "Select U_Stock FROM [@SCGD_ADMIN4] WHERE U_Tipo = '" & strTipoDestino & "'"
            Dim strCuenta As String = Utilitarios.EjecutarConsulta(ctnStock, m_oCompany.CompanyDB, m_oCompany.Server)
            Dim DescripcionCuenta As String = Utilitarios.EjecutarConsulta("Select AcctName from OACT where AcctCode = '" & strCuenta & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            Dim strFormatCode As String = Utilitarios.EjecutarConsulta("Select FormatCode from OACT where AcctCode = '" & strCuenta & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Des", intNumeroLinea - 1, strCuenta)
            oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_NCD", intNumeroLinea - 1, DescripcionCuenta)
            oTmpForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_FCD", intNumeroLinea - 1, strFormatCode)

            oMatriz.LoadFromDataSource()

            strTipoDestino = String.Empty
            strTipOrigen = String.Empty

            ' End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                            ByVal FormUID As String, _
                                            ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID

        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)

        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

        Dim oDataTable As SAPbouiCOM.DataTable
        Dim blnAddLinea As Boolean = False

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim utipo As String = String.Empty
        Dim StrTipoPostVenta As String = String.Empty

        Dim itemMatriz As Item = oForm.Items.Item("mtx_01")
        Dim oMatriz As Matrix = DirectCast(itemMatriz.Specific, Matrix)
        Dim CantidadLineas As Integer

        intNumeroLinea = pval.Row
        If pval.ActionSuccess = True AndAlso pval.BeforeAction = False Then

            If pval.ColUID = "colUnidad" Then

                If oCFLEvento.BeforeAction = False Then

                    oDataTable = oCFLEvento.SelectedObjects

                    If Not oCFLEvento.SelectedObjects Is Nothing Then

                        StrTipoPostVenta = Utilitarios.EjecutarConsulta("Select U_Inven_V from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

                        utipo = oDataTable.GetValue("U_Tipo", 0)

                        If StrTipoPostVenta = utipo Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.UnidadTrasladoServicioPostVenta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            oMatriz.FlushToDataSource()
                            Exit Sub
                        End If

                        oMatriz.FlushToDataSource()

                        'valida si la unidad ya existe en la matriz
                        Dim strUnid As String = ""
                        Dim ExisteUnidad As Boolean = False
                        strUnid = oDataTable.GetValue("U_Cod_Unid", 0)
                        strUnid = strUnid.Trim

                        For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Size - 1
                            If strUnid = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Cod", i).Trim() Then
                                ExisteUnidad = True
                                Exit For
                            End If
                        Next

                        If ExisteUnidad Then
                            ExisteUnidad = False
                            Exit Sub
                        End If


                        'utipo = oDataTable.GetValue("U_Tipo", 0)
                        Dim ctnStock As String

                        ctnStock = "Select U_Stock FROM [@SCGD_ADMIN4] WHERE U_Tipo = '" & utipo & "'"
                        Dim strCuenta As String = Utilitarios.EjecutarConsulta(ctnStock, m_oCompany.CompanyDB, m_oCompany.Server)
                        Dim DescripcionCuenta As String = Utilitarios.EjecutarConsulta("Select AcctName from OACT where AcctCode = '" & strCuenta & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        Dim strFormatCode As String = Utilitarios.EjecutarConsulta("Select FormatCode from OACT where AcctCode = '" & strCuenta & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Cod", intNumeroLinea - 1, (oDataTable.GetValue("U_Cod_Unid", 0)))
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_InO", intNumeroLinea - 1, (oDataTable.GetValue("U_Tipo", 0)))

                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Mar", intNumeroLinea - 1, (oDataTable.GetValue("U_Des_Marc", 0)))
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Est", intNumeroLinea - 1, (oDataTable.GetValue("U_Des_Esti", 0)))
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Vin", intNumeroLinea - 1, (oDataTable.GetValue("U_Num_VIN", 0)))


                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_DsO", intNumeroLinea - 1, strCuenta)
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_NCO", intNumeroLinea - 1, DescripcionCuenta)
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_FCO", intNumeroLinea - 1, strFormatCode)

                        'selecciono la unidad y busco los costos para asignarlos 
                        Dim strUnidad As String = oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Cod", intNumeroLinea - 1)
                        strUnidad = strUnidad.Trim()

                        Dim clsTraslado As New TrasladoCostosDeUnidadesCls(SBO_Application, m_oCompany)

                        Dim lista As Generic.List(Of Decimal) = clsTraslado.DevolverCostosPorUnidad(strUnidad, False)

                        Dim strSeparadorDecimalesSAP As String = String.Empty
                        Dim strSeparadorMilesSAP As String = String.Empty

                        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)


                        Dim MontoLocal As String = Utilitarios.ObtenerFormatoSAP(lista.Item(0), strSeparadorMilesSAP, strSeparadorDecimalesSAP)
                        Dim MontoSistema As String = Utilitarios.ObtenerFormatoSAP(lista.Item(1), strSeparadorMilesSAP, strSeparadorDecimalesSAP)


                        'Dim MontoLocal As String = CStr(lista.Item(0)).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorMilesSAP)
                        'Dim MontoSistema As String = CStr(lista.Item(1)).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorMilesSAP)

                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_Cos", intNumeroLinea - 1, MontoLocal)
                        oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").SetValue("U_SCGD_CSi", intNumeroLinea - 1, MontoSistema)

                        CantidadLineas = oMatriz.RowCount

                        'If oMatriz.RowCount = intNumeroLinea Then
                        '    oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").InsertRecord(oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Offset + 1)
                        'End If

                    End If

                End If
                oMatriz.LoadFromDataSource()

                'If CantidadLineas < oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Size Then
                '    oMatriz.DeleteRow(CantidadLineas + 1)
                '    'oForm.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").RemoveRecord(CantidadLineas - 1)
                'End If
            End If

        ElseIf pval.BeforeAction = True Then
            StrTipoPostVenta = Utilitarios.EjecutarConsulta("Select U_Inven_V from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

            Select Case pval.ColUID


                Case "colUnidad"
                    oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add

                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_Tipo"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                    oCondition.CondVal = StrTipoPostVenta

                    oCondition.BracketCloseNum = 1

                    oCFL.SetConditions(oConditions)

            End Select
        End If


    End Sub

#End Region

#Region "Metodos para crear Entrada"


    Public Function ValidarCodigoUnidad(ByVal p_form As SAPbouiCOM.Form) As Boolean

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix

        p_matriz = p_form.Items.Item("mtx_01").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0

        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

            Dim elementoUnidad As Xml.XmlNode
            Dim elementoCuentaOrigen As Xml.XmlNode
            Dim elementoCuentaDestino As Xml.XmlNode
            Dim elementoMontoLocal As Xml.XmlNode
            Dim elementoMontoSistema As Xml.XmlNode

            elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")
            elementoCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesOrig']")
            elementoCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDescr']")
            elementoMontoLocal = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoLo']")
            elementoMontoSistema = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoSi']")

            If elementoUnidad.InnerText = String.Empty Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeExisteLineaSinCodigoUnidad, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return True
            End If

        Next

        Return False

    End Function

    Public Function DevolverCostosPorUnidad(ByVal p_unidad As String, ByVal p_blnSinTrasladar As Boolean) As Generic.List(Of Decimal)

        Dim strConectionString As String = ""
        Dim decTotalLocalUnidad As Decimal = 0
        Dim decTotalSistemaUnidad As Decimal = 0
        Dim decTotalMontoLocal As Decimal = 0
        Dim decTotalMontoSistema As Decimal = 0
        Dim cnConeccionBD As SqlClient.SqlConnection
        Dim strcTotales As Totales = New Totales

        Dim strUnidad As String

        Dim dstTraslado As New TrasladoCostosDeUnidadesDataSet
        Dim dtTraslado As New TrasladoCostosDeUnidadesDataSetTableAdapters.SCGD_GOODRECEIVETableAdapter
        Dim drwTraslado As TrasladoCostosDeUnidadesDataSet.SCGD_GOODRECEIVERow


        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                             m_oCompany.CompanyDB, _
                                             strConectionString)

        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString
        cnConeccionBD.Open()
        dtTraslado.Connection = New SqlClient.SqlConnection(strConectionString)
        dtTraslado.Connection = cnConeccionBD

        dstTraslado.EnforceConstraints = False

        dtTraslado.Fill_Entradas(dstTraslado.SCGD_GOODRECEIVE, p_unidad)



        For Each drwTraslado In dstTraslado.SCGD_GOODRECEIVE.Rows

            If p_blnSinTrasladar Then

                If drwTraslado.U_Mon_Reg = strMonedaLocal Then

                    decTotalMontoLocal = decTotalMontoLocal + drwTraslado.U_Mon_Loc

                ElseIf drwTraslado.U_Mon_Reg = "0.0000" Then

                    decTotalMontoLocal = decTotalMontoLocal + drwTraslado.U_Mon_Loc
                    'decTotalMontoSistema = decTotalMontoSistema + drwTraslado.U_Mon_Sis

                Else
                    decTotalMontoSistema = decTotalMontoSistema + drwTraslado.U_Mon_Sis

                End If


            End If

            decTotalLocalUnidad = decTotalLocalUnidad + drwTraslado.U_Mon_Loc
            decTotalSistemaUnidad = decTotalSistemaUnidad + drwTraslado.U_Mon_Sis

        Next

        'Dim strSeparadorDecimalesSAP As String = String.Empty
        'Dim strSeparadorMilesSAP As String = String.Empty
        'Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        'decTotalLocalUnidad = CDec(Utilitarios.CambiarValoresACultureActual(decTotalLocalUnidad, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
        'decTotalSistemaUnidad = CDec(Utilitarios.CambiarValoresACultureActual(decTotalSistemaUnidad, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
        'decTotalMontoLocal = CDec(Utilitarios.CambiarValoresACultureActual(decTotalMontoLocal, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
        'decTotalMontoSistema = CDec(Utilitarios.CambiarValoresACultureActual(decTotalMontoSistema, strSeparadorMilesSAP, strSeparadorDecimalesSAP))


        ListaTipoTotal.Add(m_Local)
        ListaTotalLocalSitema.Add(decTotalLocalUnidad)
        ListaTotalMontos.Add(decTotalMontoLocal)


        ListaTipoTotal.Add(m_Sistema)
        ListaTotalLocalSitema.Add(decTotalSistemaUnidad)
        ListaTotalMontos.Add(decTotalMontoSistema)

        cnConeccionBD.Close()

        Return ListaTotalLocalSitema
    End Function

    Private Sub CargarCombos(ByVal p_matrix As SAPbouiCOM.Matrix)

        Dim m_strTipoFinal As String = Utilitarios.EjecutarConsulta("SELECT U_Inven_V FROM [@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

        Call Utilitarios.CargarValidValuesEnCombos(p_matrix.Columns.Item("colInvDest").ValidValues, "Select Code,Name From [@SCGD_TIPOVEHICULO] where Code <> '" & m_strTipoFinal.Trim & "' Order by Name")

        Call Utilitarios.CargarValidValuesEnCombos(p_matrix.Columns.Item("colInvOrig").ValidValues, "Select Code,Name From [@SCGD_TIPOVEHICULO] where Code <> '" & m_strTipoFinal.Trim & "' Order by Name")


    End Sub

    Public Function DevolverRowVehiculo(ByVal p_strIdVehiculo As String) As VehiculoDataset.SCGD_VEHICULORow

        Dim dtsVehiculo As New VehiculoDataset
        Dim daVehiculo As New VehiculoDatasetTableAdapters.SCGD_VEHICULOTableAdapter
        Dim drVehiculo As VehiculoDataset.SCGD_VEHICULORow
        Dim cnConeccionBD As SqlClient.SqlConnection

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                           m_oCompany.CompanyDB, _
                                           strConectionString)

        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString
        cnConeccionBD.Open()
        daVehiculo.Connection = New SqlClient.SqlConnection(strConectionString)
        daVehiculo.Connection = cnConeccionBD

        dtsVehiculo.EnforceConstraints = False

        daVehiculo.Fill(dtsVehiculo.SCGD_VEHICULO, p_strIdVehiculo)

        drVehiculo = dtsVehiculo.SCGD_VEHICULO.Rows(0)

        cnConeccionBD.Close()

        Return drVehiculo

    End Function

    Public Function ValidarCostosDistintosACero(ByVal p_form As SAPbouiCOM.Form, ByVal p_blnPermiteTransCero As Boolean) As Boolean

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix
        Dim n As NumberFormatInfo

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        n = DIHelper.GetNumberFormatInfo(m_oCompany)

        p_matriz = p_form.Items.Item("mtx_01").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0

        Dim qt As Integer

        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            Dim elementoUnidad As Xml.XmlNode
            Dim elementoCuentaOrigen As Xml.XmlNode
            Dim elementoCuentaDestino As Xml.XmlNode
            Dim elementoMontoLocal As Xml.XmlNode
            Dim elementoMontoSistema As Xml.XmlNode

            elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")
            elementoCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesOrig']")
            elementoCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDescr']")
            elementoMontoLocal = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoLo']")
            elementoMontoSistema = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoSi']")


            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

            Dim MontoLocal As String = CStr(elementoMontoLocal.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
            Dim MontoSistema As String = CStr(elementoMontoSistema.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

            Dim decMontoLocal As Decimal = Decimal.Parse(MontoLocal)
            Dim decMontoSistema As Decimal = Decimal.Parse(MontoSistema)

            'Dim montoLocal As Decimal = Utilitarios.ConvierteDecimal(elementoMontoLocal.InnerText, n)
            'Dim montoSistema As Decimal = Utilitarios.ConvierteDecimal(elementoMontoSistema.InnerText, n)

            If decMontoLocal = 0 Or decMontoSistema = 0 Then
                If p_blnPermiteTransCero Then
                    If SBO_Application.MessageBox(String.Format(My.Resources.Resource.MensajeTransladoCostosCero, elementoUnidad.InnerText), 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                        Return True
                    End If

                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.TrasladoCostosCeros & " " & elementoUnidad.InnerText, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return True
                End If
                
                'Else
                '    Return False
            End If
        Next

    End Function

   

    Public Sub AsigarValoresSuma(ByRef udoEntrada As UDOEntradaVehiculo, ByVal p_decLocTotal As Decimal, ByVal p_SysTotal As Decimal)
        Try
            m_dtrGoodReceipt.U_GASTRA = p_decLocTotal
            udoEntrada.Encabezado.GASTRA = p_decLocTotal

            m_dtrGoodReceipt.U_GASTRA_S = p_SysTotal
            udoEntrada.Encabezado.GASTRA_S = p_SysTotal
        Catch ex As Exception
        End Try
    End Sub

    Public Sub EncabezadoCosto(ByVal udoTrasladoCosto As UDOTrasladoCostos, ByVal p_oform As SAPbouiCOM.Form)

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix

        p_matriz = p_oform.Items.Item("mtx_01").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0

        Dim blnAddUdo As Boolean = False

        udoTrasladoCosto.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOTrasladoCostos
        udoTrasladoCosto.Encabezado.Fecha = Date.Now
        udoTrasladoCosto.Encabezado.InventarioOrig = p_oform.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").GetValue("U_SCGD_Io", p_oform.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Offset)
        udoTrasladoCosto.Encabezado.TransferidoSiNo = "N"

        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            Dim elementoUnidad As Xml.XmlNode
            Dim elementoCuentaOrigen As Xml.XmlNode
            Dim elementoCuentaDestino As Xml.XmlNode
            Dim elementoMontoLocal As Xml.XmlNode
            Dim elementoMontoSistema As Xml.XmlNode
            Dim elementoDescripcionCuentaOrigen As Xml.XmlNode
            Dim elementoDescripcionNombreCuentaOrigen As Xml.XmlNode
            Dim elementoDescripcionCuentaDestino As Xml.XmlNode
            Dim elementoDescripcionNombreCuentaDestino As Xml.XmlNode

            Dim elementoFormatCodeCuentaOrigen As Xml.XmlNode
            Dim elementoFormatCodeCuentaDestino As Xml.XmlNode

            Dim elementoMarca As Xml.XmlNode
            Dim elementoEstilo As Xml.XmlNode
            Dim elementoVin As Xml.XmlNode
            Dim elementoEntrada As Xml.XmlNode


            elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")

            elementoCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colInvOrig']")
            elementoDescripcionCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesOrig']")
            elementoDescripcionNombreCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colNomCuOr']")
            elementoFormatCodeCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colFrmtCO']")

            elementoCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colInvDest']")
            elementoDescripcionCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDescr']")
            elementoDescripcionNombreCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colNoCuDes']")
            elementoFormatCodeCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colFrmtCD']")


            elementoMarca = node.SelectSingleNode("Columns/Column/Value[../ID = 'colMarca']")
            elementoEstilo = node.SelectSingleNode("Columns/Column/Value[../ID = 'colEstilo']")
            elementoVin = node.SelectSingleNode("Columns/Column/Value[../ID = 'colVin']")
            elementoEntrada = node.SelectSingleNode("Columns/Column/Value[../ID = 'colEntrada']")



            elementoMontoLocal = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoLo']")
            elementoMontoSistema = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoSi']")


            Dim strSeparadorDecimalesSAP As String = String.Empty
            Dim strSeparadorMilesSAP As String = String.Empty

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

            Dim a As String = CStr(elementoMontoLocal.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
            Dim b As String = CStr(elementoMontoSistema.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)


            Dim decMontoLocal As Decimal = Decimal.Parse(a)
            Dim decMontoSistema As Decimal = Decimal.Parse(b)

            Dim p_decMontoLocal1 As String

           
            decMontoLocal = (Utilitarios.CambiarValoresACultureActual(elementoMontoLocal.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
            decMontoSistema = (Utilitarios.CambiarValoresACultureActual(elementoMontoSistema.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))


            AgregarLineaTraslado(elementoUnidad.InnerText, elementoMarca.InnerText, elementoEstilo.InnerText, elementoVin.InnerText, elementoCuentaOrigen.InnerText, elementoDescripcionCuentaOrigen.InnerText, elementoDescripcionNombreCuentaOrigen.InnerText, _
                                 elementoFormatCodeCuentaOrigen.InnerText, elementoCuentaDestino.InnerText, elementoDescripcionCuentaDestino.InnerText, _
                                 elementoDescripcionNombreCuentaDestino.InnerText, elementoFormatCodeCuentaDestino.InnerText, decMontoLocal, _
                                 decMontoSistema, udoTrasladoCosto, blnAddUdo)


            counter = counter + 1

            blnAddUdo = True

        Next

        udoTrasladoCosto.Insert()

        docentryUDOTraslado = udoTrasladoCosto.Encabezado.DocEntry

    End Sub

    Public Sub EncabezadoEntrada(ByVal p_strUnidad As String,
                                 ByVal p_strMarca As String,
                                ByVal p_strEstilo As String,
                                ByVal p_strModelo As String,
                                ByVal p_strVIN As String,
                                ByVal p_strIDVehiculo As String,
                                ByVal p_strTipo As String,
                                ByVal p_strAsiento As String,
                                ByVal udoEntradaVehiculo As UDOEntradaVehiculo)

        Dim cnConeccionBD As SqlClient.SqlConnection

        Dim m_dtaGoodReceipt As New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter
        Dim m_dtaGoodReceiptNumerosSerie As New GoodReceiptDatasetTableAdapters._SCG_GOODRECEIVETableAdapter

        m_dtsGoodReceipt = New GoodReceiptDataset()
        m_dttGoodReceipt = m_dtsGoodReceipt.__SCG_GOODRECEIVE
        m_dttGoodReceiptLines = m_dtsGoodReceipt.__SCG_GRLINES


        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                            m_oCompany.CompanyDB, _
                                            strConectionString)

        cnConeccionBD = New SqlClient.SqlConnection
        cnConeccionBD.ConnectionString = strConectionString
        cnConeccionBD.Open()
        m_dtaGoodReceipt.Connection = New SqlClient.SqlConnection(strConectionString)
        m_dtaGoodReceipt.Connection = cnConeccionBD

        m_dtaGoodReceiptNumerosSerie.Connection = New SqlClient.SqlConnection(strConectionString)
        m_dtaGoodReceiptNumerosSerie.Connection = cnConeccionBD


        udoEntradaVehiculo.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo

        intSerieT = m_dtaGoodReceiptNumerosSerie.SeleccionarSerie
        udoEntradaVehiculo.Encabezado.Series = intSerieT
        intDocEntryT = m_dtaGoodReceiptNumerosSerie.SeleccionarNumeroSiguiente()

        m_dtrGoodReceipt = m_dttGoodReceipt.New__SCG_GOODRECEIVERow
        m_dtrGoodReceipt.DocEntry = intDocEntryT
        m_dtrGoodReceipt.DocNum = intDocEntryT
        udoEntradaVehiculo.Encabezado.DocNum = intDocEntryT

        m_dtrGoodReceipt.Series = intSerieT
        udoEntradaVehiculo.Encabezado.Series = intSerieT

        m_dtrGoodReceipt.U_Unidad = p_strUnidad
        udoEntradaVehiculo.Encabezado.NoUnidad = p_strUnidad

        m_dtrGoodReceipt.U_Marca = p_strMarca
        udoEntradaVehiculo.Encabezado.Marca = p_strMarca

        m_dtrGoodReceipt.U_Estilo = p_strEstilo
        udoEntradaVehiculo.Encabezado.Estilo = p_strEstilo

        m_dtrGoodReceipt.U_Modelo = p_strModelo
        udoEntradaVehiculo.Encabezado.Modelo = p_strModelo

        m_dtrGoodReceipt.U_VIN = p_strVIN
        udoEntradaVehiculo.Encabezado.Vin = p_strVIN

        m_dtrGoodReceipt.U_ID_Vehiculo = p_strIDVehiculo
        udoEntradaVehiculo.Encabezado.ID_Vehiculo = p_strIDVehiculo

        m_dtrGoodReceipt.U_Tipo = p_strTipo
        udoEntradaVehiculo.Encabezado.Tipo = p_strTipo

        m_dtrGoodReceipt.U_Fec_Cont = m_datFechaContabilizacion
        udoEntradaVehiculo.Encabezado.Fec_Cont = m_datFechaContabilizacion

        m_dtrGoodReceipt.CreateDate = m_datFechaContabilizacion
        udoEntradaVehiculo.Encabezado.CreateDate = m_datFechaContabilizacion

        m_dtrGoodReceipt.U_Cambio = m_decTipoCambio
        udoEntradaVehiculo.Encabezado.Cambio = m_decTipoCambio

        m_dtrGoodReceipt.SetU_As_EntrNull()
        udoEntradaVehiculo.Encabezado.AsientoEntrada = p_strAsiento

        m_dtrGoodReceipt.SetU_SCGD_DocSalidaNull()

        udoEntradaVehiculo.Encabezado.EsTraslado = "Y"

        m_dttGoodReceipt.Add__SCG_GOODRECEIVERow(m_dtrGoodReceipt)



    End Sub

    Public Sub AgregarLineaTraslado(ByVal p_Unidad As String, ByVal p_Marca As String, ByVal p_Estilo As String, ByVal p_Vin As String, ByVal p_CuentaOrigen As String, ByVal p_DescripcionCuentaOrigen As String, _
                         ByVal p_DescripcionNombreCuentaOrigen As String, ByVal p_FormatCodeOrigen As String, ByVal p_CuentaDestino As String, _
                         ByVal p_DescripcionCuentaDestino As String, ByVal p_DescripcionNombreCuentaDestino As String, ByVal p_FormatCodeDestino As String, _
                         ByVal p_montoLocal As Decimal, ByVal p_montoSistema As Decimal, _
                         ByVal udoTraslado As UDOTrasladoCostos, ByRef blnLineaAgregada As Boolean)

        If blnLineaAgregada = False Then

            udoTraslado.ListaLineas = New ListaUDOTrasladoCostos()

            udoTraslado.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)
            blnLineaAgregada = True

        End If

        Dim lineaTraslado As LineaUDOTrasladoCostos = New LineaUDOTrasladoCostos()

        lineaTraslado.Codigo = p_Unidad

        lineaTraslado.InventarioOr = p_CuentaOrigen
        lineaTraslado.DescripcionInvOr = p_DescripcionCuentaOrigen
        lineaTraslado.NomCuentaOr = p_DescripcionNombreCuentaOrigen
        lineaTraslado.FormatCodeOrigen = p_FormatCodeOrigen

        lineaTraslado.InventarioDst = p_CuentaDestino
        lineaTraslado.DescripcionInvDst = p_DescripcionCuentaDestino
        lineaTraslado.NomCuentaDst = p_DescripcionNombreCuentaDestino
        lineaTraslado.FormatCodeDestino = p_FormatCodeDestino

        lineaTraslado.Marca = p_Marca
        lineaTraslado.Estilo = p_Estilo
        lineaTraslado.NumeroVin = p_Vin

    

        lineaTraslado.CostoLocal = p_montoLocal
        lineaTraslado.CostoSistema = p_montoSistema


        udoTraslado.ListaLineas.LineasUDO.Add(lineaTraslado)
    End Sub

    Public Sub AgregarLinea(ByVal p_strConcepto As String, _
                          ByVal decMontoLT As Decimal, ByVal decMontoST As Decimal, ByVal strMonedaRegistro As String, _
                          ByVal intNumeroAsiento As Int64, ByVal strCuenta As String, ByVal p_TipoCambioEntrada As String, _
                          ByVal intTipoTransaccion As enumTipoCargo, ByVal udoEntrada As UDOEntradaVehiculo, ByRef blnLineaAgregada As Boolean)

        If blnLineaAgregada = False Then

            udoEntrada.ListaLineas = New ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

            blnLineaAgregada = True

        End If

        Dim lineaEntrada As LineaUDOEntradaVehiculo = New LineaUDOEntradaVehiculo()

        m_dtrGoodReceiptLines = m_dttGoodReceiptLines.New__SCG_GRLINESRow
        m_dtrGoodReceiptLines.DocEntry = intDocEntryT
        m_dtrGoodReceiptLines.SetU_No_FCNull()
        m_dtrGoodReceiptLines.SetU_NoFPNull()

        m_dtrGoodReceiptLines.U_Concepto = p_strConcepto
        lineaEntrada.Concepto = p_strConcepto

        m_dtrGoodReceiptLines.U_Cuenta = strCuenta
        lineaEntrada.Cuenta = strCuenta

        m_dtrGoodReceiptLines.U_Mon_Loc = decMontoLT
        lineaEntrada.Mon_Loc = decMontoLT
        m_dtrGoodReceiptLines.U_Mon_Sis = decMontoST '/ p_TipoCambioEntrada
        lineaEntrada.Mon_Sis = decMontoST '/ p_TipoCambioEntrada

        m_dtrGoodReceiptLines.U_Mon_Reg = strMonedaRegistro
        lineaEntrada.Mon_Reg = "0.0000" 'strMonedaRegistro

        m_dtrGoodReceiptLines.U_NoAsient = intNumeroAsiento
        lineaEntrada.NoAsient = intNumeroAsiento

        m_dtrGoodReceiptLines.U_Tip_Cam = p_TipoCambioEntrada
        lineaEntrada.Tip_Cam = p_TipoCambioEntrada

        m_dttGoodReceiptLines.Add__SCG_GRLINESRow(m_dtrGoodReceiptLines)

        udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

    End Sub

    Public Function RetornarMonedaLocal() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String
        Try
            oSBObob = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
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
        oSBObob = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oRecordset = oSBObob.GetSystemCurrency()
        strResult = oRecordset.Fields.Item(0).Value
        Return strResult
    End Function

    Private Function CargarTipoCambio() As Boolean
        Dim strMoneda As String
        Dim strConectionString As String = String.Empty
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

        m_objBLSBO.Set_Compania(m_oCompany)
        strMonedaSistema = RetornarMonedaSistema()
        strMonedaLocal = RetornarMonedaLocal()
        If strMonedaLocal <> strMonedaSistema Then
            m_decTipoCambio = RetornarTipoCambioMoneda(strMonedaSistema, m_objUtilitarios.CargarFechaHoraServidor(), strConectionString, False)
            If m_decTipoCambio = -1 Then
                'Throw New Exception(My.Resources.Resource.TipoCambioNoActualizado)
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

    Public Function LlenarListaEntradasTrasladada(ByVal p_matriz As SAPbouiCOM.Matrix) As IList(Of String)

        Dim strConectionString As String = ""
        Dim decTotalLocalUnidad As Decimal = 0
        Dim decTotalSistemaUnidad As Decimal = 0
        Dim cnConeccionBD As SqlClient.SqlConnection
        Dim strcTotales As Totales = New Totales

        Dim strUnidad As String

        Dim dstTraslado As New TrasladoCostosDeUnidadesDataSet
        Dim dtTraslado As New TrasladoCostosDeUnidadesDataSetTableAdapters.SCGD_GOODRECEIVETableAdapter
        Dim drwTraslado As TrasladoCostosDeUnidadesDataSet.SCGD_GOODRECEIVERow
        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim counter As Integer = 0

        Dim dtGoodReceive As System.Data.DataTable

        Try
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                                 m_oCompany.CompanyDB, _
                                                 strConectionString)
            cnConeccionBD = New SqlClient.SqlConnection
            cnConeccionBD.ConnectionString = strConectionString
            cnConeccionBD.Open()
            dtTraslado.Connection = New SqlClient.SqlConnection(strConectionString)
            dtTraslado.Connection = cnConeccionBD
            dstTraslado.EnforceConstraints = False
            matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)
            xmlDocMatrix = New Xml.XmlDocument
            xmlDocMatrix.LoadXml(matrixXml)

            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                Dim elementoUnidad As Xml.XmlNode
                elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")

                dtTraslado.Fill_Entradas(dstTraslado.SCGD_GOODRECEIVE, elementoUnidad.InnerText)

                For Each drwTraslado In dstTraslado.SCGD_GOODRECEIVE.Rows
                    If Not ListaEntradas.Contains(drwTraslado.DocEntry) Then
                        ListaEntradas.Add(drwTraslado.DocEntry)
                    End If
                Next
                counter = counter + 1
            Next
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
        cnConeccionBD.Close()
        Return ListaEntradas
    End Function

    Public Sub ActualizarTipoVehiculo(ByVal p_unidad As String, ByVal p_U_tipo As String)
        Try
            Utilitarios.EjecutarConsulta("UPDATE [dbo].[@SCGD_VEHICULO] SET [U_Tipo] = '" & p_U_tipo & "' WHERE U_Cod_Unid = '" & p_unidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Private Sub ActualizarCampoTrasladado(ByVal p_intDocEntry As Integer)
        Try
            Utilitarios.EjecutarConsulta("UPDATE [@SCGD_TR_COSTOS] SET [U_SCGD_TYN] = 'Y' WHERE [DocEntry] = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub ActualizarNumeroAsiento(ByVal p_intNumEntrada As Integer, ByVal p_intNumAsiento As Integer)
        Try
            Utilitarios.EjecutarConsulta("Update [@SCGD_GOODRECEIVE] set U_As_Entr = " & p_intNumAsiento & " where docentry = " & p_intNumEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ActualizarLineaTrasladoEntrada(ByVal p_intDocEntry As Integer, ByVal p_intDocEntryEntradaVehiculo As Integer, ByVal p_strCodUnidad As String)
        Try
            Utilitarios.EjecutarConsulta("UPDATE [dbo].[@SCGD_TR_COSTOLINEAS] SET [U_SCGD_EN] = '" & p_intDocEntryEntradaVehiculo & "' WHERE DocEntry = " & p_intDocEntry & " and U_SCGD_Cod= '" & p_strCodUnidad & "' ", m_oCompany.CompanyDB, m_oCompany.Server)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ActualizarEntradaTrasladada(ByVal p_lista As Generic.List(Of String))
        Try

            For i As Integer = 0 To p_lista.Count - 1

                ActualizaEntradaGR(p_lista.Item(i))

            Next

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ActualizaEntradaGR(ByVal p_strDocEntry As String)
        Try
            Dim oCompanyServiceTraslado As SAPbobsCOM.CompanyService
            Dim oGeneralServiceTraslado As SAPbobsCOM.GeneralService
            Dim oGeneralDataTraslado As SAPbobsCOM.GeneralData
            Dim oGeneralParamsTraslado As SAPbobsCOM.GeneralDataParams

            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                oCompanyServiceTraslado = m_oCompany.GetCompanyService()
                oGeneralServiceTraslado = oCompanyServiceTraslado.GetGeneralService("SCGD_GOODENT")
                oGeneralParamsTraslado = oGeneralServiceTraslado.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsTraslado.SetProperty("DocEntry", p_strDocEntry)
                oGeneralDataTraslado = oGeneralServiceTraslado.GetByParams(oGeneralParamsTraslado)
                oGeneralDataTraslado.SetProperty("U_SCGD_Trasl", "Y")
                oGeneralServiceTraslado.Update(oGeneralDataTraslado)
                oGeneralServiceTraslado.Close(oGeneralParamsTraslado)


            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function CargarTipoCambio(ByVal p_oform As SAPbouiCOM.Form) As Boolean
        Dim strMoneda As String
        Dim strConectionString As String = String.Empty
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
        Dim m_objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
        m_objBLSBO.Set_Compania(m_oCompany)
        strMonedaSistema = RetornarMonedaSistema()
        strMonedaLocal = RetornarMonedaLocal()
        If strMonedaLocal <> strMonedaSistema Then
            m_decTipoCambio = RetornarTipoCambioMoneda(strMonedaSistema, m_objUtilitarios.CargarFechaHoraServidor(), strConectionString, False)
            If m_decTipoCambio = -1 Then
                SBO_Application.MessageBox(My.Resources.Resource.TipoCambioNoActualizado)
                Return False
            End If
        Else
            m_decTipoCambio = 1
        End If
        Return True
    End Function
#End Region

#Region "Metodos Nuevos"
    Public Sub CrearTraslado(ByVal p_form As SAPbouiCOM.Form, Optional p_blnPermiteTrasladoCero As Boolean = False)

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix
        p_matriz = p_form.Items.Item("mtx_01").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)
        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)
        Dim counter As Integer = 0
        Dim Valores As New ValoresTrasladoEntradas
        Dim dtGoodReceive As System.Data.DataTable
        Dim dtEntradasPorVehiculo As SAPbouiCOM.DataTable
        Dim dtVehiculo As System.Data.DataTable
        Dim strTipoInvDestino As String = String.Empty
        Dim strTipoInvOrigen As String = String.Empty
        Dim oMontoTotalAsiento As New List(Of MontoTotalAsiento)()
        Dim blnActualizaTraslado As Boolean = True
        Try
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)

            Call CargarTipoCambio()
            Dim qt As Integer

            ValidarConfiguracionDimensiones(p_form)


            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

                Dim elementoUnidad As Xml.XmlNode
                Dim elementoCuentaOrigen As Xml.XmlNode
                Dim elementoCuentaDestino As Xml.XmlNode
                Dim elementoMontoLocal As Xml.XmlNode
                Dim elementoMontoSistema As Xml.XmlNode
                Dim elementoTrasladoGenerado As Xml.XmlNode

                elementoUnidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colUnidad']")
                elementoCuentaOrigen = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesOrig']")
                elementoCuentaDestino = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDescr']")
                elementoMontoLocal = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoLo']")
                elementoMontoSistema = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCostoSi']")
                elementoTrasladoGenerado = node.SelectSingleNode("Columns/Column/Value[../ID = 'colTGen']")

                If Not elementoUnidad.InnerText = String.Empty Then

                    ListaEntradas.Clear()

                    If elementoTrasladoGenerado.InnerText = "N" Or elementoTrasladoGenerado.InnerText = String.Empty Then

                        Dim strIDUnidad As String = Utilitarios.EjecutarConsulta("Select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '" & elementoUnidad.InnerText & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                        Dim strNoUnidad As String = elementoUnidad.InnerText
                        strNoUnidad = strNoUnidad.Trim()

                        If Not String.IsNullOrEmpty(strNoUnidad) Then

                            'dataTableEntradasPorUnidad.ExecuteQuery(String.Format(strConsultaEntradasPorVehiculo, elementoUnidad.InnerText.Trim()))


                            dtGoodReceive = Utilitarios.EjecutarConsultaDataTable(String.Format("select DocEntry, U_As_Entr,U_Unidad From dbo.[@SCGD_GOODRECEIVE] where U_Unidad='{0}' and  Status= 'O' and U_SCGD_Trasl='N' and (U_SCGD_DocSalida is null or U_SCGD_DocSalida='')",
                                                                       strNoUnidad),
                                                                    m_oCompany.CompanyDB,
                                                                    m_oCompany.Server)
                            LlenarListaGoodReceive(dtGoodReceive)


                            dtVehiculo = Utilitarios.EjecutarConsultaDataTable(String.Format("select Code,U_Cod_Unid,U_Cod_Marc,U_Cod_Esti,U_Cod_Mode, U_Num_VIN,U_Tipo, U_Des_Marc,U_Des_Esti,U_Des_Mode from dbo.[@SCGD_VEHICULO] where Code= '{0}'",
                                                                 strIDUnidad),
                                                              m_oCompany.CompanyDB,
                                                              m_oCompany.Server)

                            strTipoInvDestino = p_form.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_Inv", counter).Trim
                            strTipoInvOrigen = p_form.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").GetValue("U_SCGD_InO", counter).Trim

                            If Not String.IsNullOrEmpty(strTipoInvDestino) And Not String.IsNullOrEmpty(strTipoInvOrigen) Then

                                udoEntrada = New UDOEntradaVehiculo(m_oCompany)

                                udoEntrada.Company.StartTransaction()

                                Dim intAsientoTraslado As Integer = 0

                                intAsientoTraslado = CrearAsientoTraslado(dtGoodReceive, strNoUnidad, strTipoInvOrigen, strTipoInvDestino)

                                If intAsientoTraslado > 0 Then
                                    oMontoTotalAsiento.Clear()
                                    ConsultaAsientoTraslado(oMontoTotalAsiento, intAsientoTraslado.ToString.Trim())
                                    udoEntrada = GenerarGoodReceive(oMontoTotalAsiento, dtVehiculo, strTipoInvDestino, Convert.ToString(intAsientoTraslado))
                                    udoEntrada.Insert()
                                    intDocEntryEntrada = udoEntrada.Encabezado.DocEntry
                                    ActualizaVehiculo(strIDUnidad, strTipoInvDestino)
                                    ActualizaLineaTrasladoCosto(docentryUDOTraslado, intDocEntryEntrada, counter)
                                    ActualizarEntradaTrasladada(ListaEntradas)

                                Else

                                    If p_blnPermiteTrasladoCero And blnErrorCreacionAsiento = False Then
                                        oMontoTotalAsiento.Clear()
                                        oMontoTotalAsiento.Add(New MontoTotalAsiento() With {.LocTotal = 0, .SysTotal = 0})
                                        udoEntrada = GenerarGoodReceive(oMontoTotalAsiento, dtVehiculo, strTipoInvDestino, "-1")
                                        udoEntrada.Insert()
                                        intDocEntryEntrada = udoEntrada.Encabezado.DocEntry
                                        ActualizaVehiculo(strIDUnidad, strTipoInvDestino)
                                        ActualizaLineaTrasladoCosto(docentryUDOTraslado, intDocEntryEntrada, counter)
                                    Else

                                        If udoEntrada.Company.InTransaction Then
                                            udoEntrada.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                        End If

                                        blnActualizaTraslado = False

                                        SBO_Application.MessageBox(My.Resources.Resource.LaUnidad & elementoUnidad.InnerText & " " & My.Resources.Resource.NoTrasladoUnidades, 1, "Ok")

                                    End If

                                End If

                                If udoEntrada.Company.InTransaction Then
                                    udoEntrada.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                    'blnActualizaTraslado = True
                                End If
                                intDocEntryEntrada = 0
                            End If
                        End If
                    End If
                End If
                counter = counter + 1
            Next
            If blnActualizaTraslado = True Then
                ActualizaTrasladoCosto(docentryUDOTraslado)
            End If
        Catch ex As Exception
            If udoEntrada.Company.InTransaction Then
                udoEntrada.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub
    Public Function LlenarListaGoodReceive(p_dataTable As System.Data.DataTable) As IList(Of String)


        For Each linea As System.Data.DataRow In p_dataTable.Rows

            If Not ListaEntradas.Contains(linea.Item("DocEntry")) Then
                ListaEntradas.Add(linea.Item("DocEntry"))
            End If

        Next

        Return ListaEntradas

    End Function


    Public Sub ActualizaVehiculo(ByVal p_strCode As String, ByVal p_U_Tipo As String)
        Try
            Dim oCompanyServiceVehiculo As SAPbobsCOM.CompanyService
            Dim oGeneralServiceVehiculo As SAPbobsCOM.GeneralService
            Dim oGeneralDataVehiculo As SAPbobsCOM.GeneralData
            Dim oGeneralParamsVehiculo As SAPbobsCOM.GeneralDataParams

            If Not String.IsNullOrEmpty(p_strCode) And Not String.IsNullOrEmpty(p_U_Tipo) Then
                oCompanyServiceVehiculo = m_oCompany.GetCompanyService()
                oGeneralServiceVehiculo = oCompanyServiceVehiculo.GetGeneralService("SCGD_VEH")
                oGeneralParamsVehiculo = oGeneralServiceVehiculo.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsVehiculo.SetProperty("Code", p_strCode)
                oGeneralDataVehiculo = oGeneralServiceVehiculo.GetByParams(oGeneralParamsVehiculo)
                oGeneralDataVehiculo.SetProperty("U_Tipo", p_U_Tipo)
                oGeneralDataVehiculo.SetProperty("U_Tipo_Ven", p_U_Tipo)
                oGeneralServiceVehiculo.Update(oGeneralDataVehiculo)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ActualizaLineaTrasladoCosto(ByVal p_strDocEntry As String, ByVal p_strDocEntryEntrada As String, ByVal p_cont As Integer)
        Try
            Dim oCompanyServiceTraslado As SAPbobsCOM.CompanyService
            Dim oGeneralServiceTraslado As SAPbobsCOM.GeneralService
            Dim oGeneralDataTraslado As SAPbobsCOM.GeneralData
            Dim oGeneralParamsTraslado As SAPbobsCOM.GeneralDataParams
            Dim oChildTraslado As SAPbobsCOM.GeneralData
            Dim oChildrenTraslado As SAPbobsCOM.GeneralDataCollection

            If Not String.IsNullOrEmpty(p_strDocEntry) And Not String.IsNullOrEmpty(p_strDocEntryEntrada) Then
                oCompanyServiceTraslado = m_oCompany.GetCompanyService()
                oGeneralServiceTraslado = oCompanyServiceTraslado.GetGeneralService("SCGD_TRCU")
                oGeneralParamsTraslado = oGeneralServiceTraslado.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsTraslado.SetProperty("DocEntry", p_strDocEntry)
                oGeneralDataTraslado = oGeneralServiceTraslado.GetByParams(oGeneralParamsTraslado)
                oChildrenTraslado = oGeneralDataTraslado.Child("SCGD_TR_COSTOLINEAS")
                oChildTraslado = oChildrenTraslado.Item(p_cont)
                oChildTraslado.SetProperty("U_SCGD_EN", p_strDocEntryEntrada)
                oChildTraslado.SetProperty("U_SCGD_TGe", "Y")
                oGeneralServiceTraslado.Update(oGeneralDataTraslado)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub ActualizaTrasladoCosto(ByVal p_strDocEntry As String)
        Try
            Dim oCompanyServiceTraslado As SAPbobsCOM.CompanyService
            Dim oGeneralServiceTraslado As SAPbobsCOM.GeneralService
            Dim oGeneralDataTraslado As SAPbobsCOM.GeneralData
            Dim oGeneralParamsTraslado As SAPbobsCOM.GeneralDataParams

            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                oCompanyServiceTraslado = m_oCompany.GetCompanyService()
                oGeneralServiceTraslado = oCompanyServiceTraslado.GetGeneralService("SCGD_TRCU")
                oGeneralParamsTraslado = oGeneralServiceTraslado.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsTraslado.SetProperty("DocEntry", p_strDocEntry)
                oGeneralDataTraslado = oGeneralServiceTraslado.GetByParams(oGeneralParamsTraslado)
                oGeneralDataTraslado.SetProperty("U_SCGD_TYN", "Y")
                oGeneralServiceTraslado.Update(oGeneralDataTraslado)
            End If
        Catch ex As Exception
        End Try
    End Sub

   


    Public Function CrearAsientoTraslado(ByVal p_dtGoodReceive As System.Data.DataTable,
                                         ByVal p_strNoUnidad As String,
                                         ByVal p_strTipoInvOrigen As String,
                                         ByVal p_strTipoInvDestino As String) As Integer
        Try
            Dim oJE_Lines As SAPbobsCOM.JournalEntries_Lines
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim oListaLineasAsiento As New List(Of ListaLineaAsientoTraslado)()
            Dim oListaAsiento As New List(Of ListaLineaAsientoTraslado)()

            Dim rowGRLines As System.Data.DataRow
            Dim strAsiento As String = String.Empty

            Dim strAsientoGenerado As String = "0"
            Dim strFCCurrencyTemp As String = String.Empty
            Dim strMonedaLocal As String = String.Empty

            Dim strCuenta As String = String.Empty
            Dim strContraCuenta As String = String.Empty
            Dim strCuentaSeleccionada As String = String.Empty
            Dim strTipoVehiculo As String = String.Empty

            Dim dateFechaConta As Date = Nothing
            Dim strFechaConta As String

            Dim intError As Integer
            Dim strMensajeError As String = ""
            Dim formato As String
            Dim dateFechaRegistro As Date = Nothing

            Dim blnAgregarDimension As Boolean = False

            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            m_objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            'Carga de cuentas contables
            strCuenta = m_objConfiguracionGeneral.CuentaStock(p_strTipoInvOrigen)
            strContraCuenta = m_objConfiguracionGeneral.CuentaStock(p_strTipoInvDestino)

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", m_oCompany.CompanyDB, m_oCompany.Server)

            If blnUsaDimensiones Then
                '******************************************************************************************
                'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] where U_Cod_Unid = '" & p_strNoUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContables(m_oFormTraslados, p_strTipoInvDestino, strCodigoMarca, oDataTableDimensionesContablesDMS))

                If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                    blnAgregarDimension = True
                End If
                '******************************************************************************************
            End If

            If Not String.IsNullOrEmpty(strCuenta) Then
                oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

                For Each rowGRLines In p_dtGoodReceive.Rows
                    strAsiento = rowGRLines.Item("U_As_Entr").ToString.Trim()
                    If Not String.IsNullOrEmpty(strAsiento) And strAsiento <> "-1" Then
                        oJournalEntry.GetByKey(strAsiento)
                        oJE_Lines = oJournalEntry.Lines
                        For i As Integer = 0 To oJE_Lines.Count - 1
                            oJE_Lines.SetCurrentLine(i)
                            If oJE_Lines.AccountCode = strCuenta Then
                                strFCCurrencyTemp = oJE_Lines.FCCurrency.ToString.Trim()
                                If String.IsNullOrEmpty(strFCCurrencyTemp) Then
                                    oListaLineasAsiento.Add(New ListaLineaAsientoTraslado() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .Debit = Decimal.Parse(oJE_Lines.Debit), .Credit = Decimal.Parse(oJE_Lines.Credit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                ElseIf Not String.IsNullOrEmpty(strFCCurrencyTemp) Then
                                    oListaLineasAsiento.Add(New ListaLineaAsientoTraslado() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .FCCurrency = oJE_Lines.FCCurrency.ToString.Trim(), .FCDebit = Decimal.Parse(oJE_Lines.FCDebit), .FCCredit = Decimal.Parse(oJE_Lines.FCCredit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                End If
                            End If
                        Next
                    End If
                Next
            End If

            Dim decMontoTemp As Decimal = 0
            Dim blnAgregar As Boolean = False
            Dim blnMonedaLocal As Boolean = False
            Dim strMoneda As String = String.Empty

            For Each C1 As ListaLineaAsientoTraslado In oListaLineasAsiento

                decMontoTemp = 0
                blnAgregar = False
                strMoneda = String.Empty
                If Not String.IsNullOrEmpty(C1.FCCurrency) Then
                    strMoneda = C1.FCCurrency
                Else
                    strMoneda = strMonedaLocal
                End If

                For Each C2 As ListaLineaAsientoTraslado In oListaLineasAsiento

                    If Not String.IsNullOrEmpty(C1.FCCurrency) And Not String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                        If C2.FCDebit <> 0 Then
                            decMontoTemp += C2.FCDebit
                            C2.Aplicado = True
                            blnAgregar = True
                        ElseIf C2.FCCredit > 0 Then
                            If C2.ImpNeg <> "N" Then
                                decMontoTemp += (C2.FCCredit * -1)
                                C2.Aplicado = True
                                blnAgregar = True
                            Else
                                C2.Aplicado = True
                            End If

                        End If
                    ElseIf String.IsNullOrEmpty(C1.FCCurrency) And String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                        If C2.Debit <> 0 Then
                            decMontoTemp += C2.Debit
                            C2.Aplicado = True
                            blnAgregar = True
                        ElseIf C2.Credit > 0 Then
                            If C2.ImpNeg <> "N" Then
                                decMontoTemp += (C2.Credit * -1)
                                C2.Aplicado = True
                                blnAgregar = True
                            Else
                                C2.Aplicado = True
                            End If
                        End If
                    End If
                Next
                If blnAgregar = True And decMontoTemp > 0 Then
                    If strMonedaLocal = strMoneda Then
                        oListaAsiento.Add(New ListaLineaAsientoTraslado() With {.FCCurrency = strMonedaLocal, .Debit = decMontoTemp, .Credit = decMontoTemp, .Aplicado = True})
                    Else
                        oListaAsiento.Add(New ListaLineaAsientoTraslado() With {.FCCurrency = strMoneda, .FCDebit = decMontoTemp, .FCCredit = decMontoTemp, .Aplicado = True})
                    End If
                End If
            Next

            If oListaAsiento.Count() > 0 Then

                strAsientoGenerado = "0"

                oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                oJournalEntry.Memo = My.Resources.Resource.MensajeTrasladoCostosUnidadesAsiento & " " & p_strNoUnidad
                oJournalEntry.Reference = p_strNoUnidad

                For Each row As ListaLineaAsientoTraslado In oListaAsiento

                    '*********************
                    ' Contra cuenta
                    'Cuenta Credito
                    '*********************
                    oJournalEntry.Lines.AccountCode = strCuenta

                    If strMonedaLocal = row.FCCurrency Then
                        oJournalEntry.Lines.Credit = row.Credit
                        oJournalEntry.Lines.FCCredit = 0
                    Else
                        oJournalEntry.Lines.FCCredit = row.FCCredit
                        oJournalEntry.Lines.FCCurrency = row.FCCurrency
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = p_strNoUnidad
                    oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                    If blnAgregarDimension Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                    End If

                    oJournalEntry.Lines.Add()
                    '*****************
                    'Cuenta Debito
                    '*****************
                    oJournalEntry.Lines.AccountCode = strContraCuenta
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.Reference1 = p_strNoUnidad
                    oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                    If strMonedaLocal = row.FCCurrency Then
                        oJournalEntry.Lines.Debit = row.Debit
                        oJournalEntry.Lines.FCDebit = 0
                    Else
                        oJournalEntry.Lines.FCDebit = row.FCDebit
                        oJournalEntry.Lines.FCCurrency = row.FCCurrency
                    End If

                    If blnAgregarDimension Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                    End If

                    oJournalEntry.Lines.Add()
                Next

                If oJournalEntry.Add <> 0 Then
                    strAsientoGenerado = "0"
                    blnErrorCreacionAsiento = True
                    m_oCompany.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intError, strMensajeError)
                Else
                    m_oCompany.GetNewObjectCode(strAsientoGenerado)
                End If
            End If
            Return CInt(strAsientoGenerado)
        Catch ex As Exception
        End Try
    End Function

    Private Function GenerarGoodReceive(ByVal p_oMontoTotalAsiento As List(Of MontoTotalAsiento),
                                        ByVal p_dtVehiculo As System.Data.DataTable,
                                        ByVal p_strTipoInvDestino As String,
                                        ByVal p_strAsiento As String) As UDOEntradaVehiculo
        Dim oNotaCredito As SAPbobsCOM.Documents
        Dim strAsientoNotaCredito As String
        Dim dtrVehiculousado As VehiculoDataset.SCGD_VEHICULORow
        Dim strUnidad As String = String.Empty
        Dim strMarca As String = String.Empty
        Dim strEstilo As String = String.Empty
        Dim strModelo As String = String.Empty
        Dim strVIN As String = String.Empty
        Dim strCode As String = String.Empty
        Dim cnConeccionBD As SqlClient.SqlConnection
        Dim dstTraslado As New TrasladoCostosDeUnidadesDataSet
        Dim dtTraslado As New TrasladoCostosDeUnidadesDataSetTableAdapters.TotalesTodoTableAdapter
        Dim drwTraslado As TrasladoCostosDeUnidadesDataSet.TotalesTodoRow
        Dim dstT As New TrasladoCostosDeUnidadesDataSet
        Dim daT As New TrasladoCostosDeUnidadesDataSetTableAdapters.SCGD_GOODRECEIVETableAdapter
        Dim drwT As TrasladoCostosDeUnidadesDataSet.SCGD_GOODRECEIVERow
        Dim rowVeh As System.Data.DataRow
       
        For Each rowVeh In p_dtVehiculo.Rows
            If Not String.IsNullOrEmpty(rowVeh.Item("U_Cod_Unid").ToString.Trim()) Then
                strUnidad = rowVeh.Item("U_Cod_Unid").ToString.Trim()
            End If
            If Not String.IsNullOrEmpty(rowVeh.Item("U_Des_Marc").ToString.Trim()) Then
                strMarca = rowVeh.Item("U_Des_Marc").ToString.Trim()
            End If
            If Not String.IsNullOrEmpty(rowVeh.Item("U_Des_Esti").ToString.Trim()) Then
                strEstilo = rowVeh.Item("U_Des_Esti").ToString.Trim()
            End If
            If Not String.IsNullOrEmpty(rowVeh.Item("U_Des_Mode").ToString.Trim()) Then
                strModelo = rowVeh.Item("U_Des_Mode").ToString.Trim()
            End If
            If Not String.IsNullOrEmpty(rowVeh.Item("U_Num_VIN").ToString.Trim()) Then
                strVIN = rowVeh.Item("U_Num_VIN").ToString.Trim()
            End If
            If Not String.IsNullOrEmpty(rowVeh.Item("Code").ToString.Trim()) Then
                strCode = rowVeh.Item("Code").ToString.Trim()
            End If
            Exit For
        Next

        Dim udoEntrada As UDOEntradaVehiculo = New UDOEntradaVehiculo(m_oCompany)
        Dim blnLineaAgregada As Boolean = False
        EncabezadoEntrada(strUnidad, strMarca, strEstilo, strModelo, strVIN, strCode, p_strTipoInvDestino, p_strAsiento, udoEntrada)
        If p_oMontoTotalAsiento.Count > 0 Then
            For Each row As MontoTotalAsiento In p_oMontoTotalAsiento
                AgregarLinea(My.Resources.Resource.TrasladoCostoVehiculo, row.LocTotal, row.SysTotal, "", -1, "", m_decTipoCambio, ObjetoGoodReceiptCls.enumTipoCargo.CIF, udoEntrada, blnLineaAgregada)
                AsigarValoresSuma(udoEntrada, row.LocTotal, row.SysTotal)
                Exit For
            Next
        End If
        intDocEntryEntradaVehiculo = udoEntrada.Encabezado.DocNum
        Return udoEntrada
    End Function

    Private Sub ConsultaAsientoTraslado(ByRef p_oMontoTotalAsiento As List(Of MontoTotalAsiento), ByVal p_strAsiento As String)
        Try
            Dim oJE_Lines As SAPbobsCOM.JournalEntries_Lines
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim decLocTotal As Decimal = 0
            Dim decSysTotal As Decimal = 0
            If Not String.IsNullOrEmpty(p_strAsiento) Then
                oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                oJournalEntry.GetByKey(p_strAsiento)
                oJE_Lines = oJournalEntry.Lines
                For i As Integer = 0 To oJE_Lines.Count - 1
                    oJE_Lines.SetCurrentLine(i)
                    decLocTotal += oJE_Lines.Debit
                    decSysTotal += oJE_Lines.DebitSys
                Next
                If decLocTotal > 0 And decSysTotal > 0 Then
                    p_oMontoTotalAsiento.Add(New MontoTotalAsiento() With {.LocTotal = decLocTotal, .SysTotal = decSysTotal})
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ValidarDataTable(ByRef p_form As SAPbouiCOM.Form)

        Dim ExisteDataTable As Boolean = Utilitarios.ValidaExisteDataTable(p_form, mc_strDataTableDimensionesOT)
        
        If Not ExisteDataTable Then
            oDataTableDimensionesContablesDMS = p_form.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
        End If

    End Sub

    Private Sub ValidarConfiguracionDimensiones(ByVal p_form As SAPbouiCOM.Form)

        If DMS_Connector.Configuracion.ParamGenAddon.U_UsaDimC.Trim.Equals("Y") Then

            oDataTableDimensionesContablesDMS = p_form.DataSources.DataTables.Item(mc_strDataTableDimensionesOT)
            blnUsaDimensiones = True

            'hago el llamado para cargar la configuracion de los documentos
            'que usaran Dimensiones
            ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)
            ListaConfiguracionOT = New Hashtable
            ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensiones(p_form)

        End If


    End Sub

    Private Sub CargarTraslado(ByVal p_strItem As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Dim strIdVehiculo As String
        If m_oFormTraslados IsNot Nothing Then

            'oitem = m_oFormTraslados.Items.Item("5")
            'oedit = CType(oitem.Specific, SAPbouiCOM.EditText)

            'strIdVehiculo = p_strItem

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strItem

            'oedit = oitem.Specific
            'oedit.String = strIdVehiculo
            'm_oFormTraslados.Items.Item(mc_strUIDCargar).Visible = False
            Call m_oFormTraslados.DataSources.DBDataSources.Item("@SCGD_TR_COSTOS").Query(oConditions)
            Call m_oFormTraslados.DataSources.DBDataSources.Item("@SCGD_TR_COSTOLINEAS").Query(oConditions)
            m_oFormTraslados.Items.Item("mtx_01").Specific.LoadFromDataSource()
            m_oFormTraslados.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

        End If

    End Sub


#End Region

End Class
' Clase para la definición de la lista
Public Class MontoTotalAsiento
    Public Property LocTotal() As Decimal
        Get
            Return decLocTotal
        End Get
        Set(ByVal value As Decimal)
            decLocTotal = value
        End Set
    End Property
    Private decLocTotal As Decimal

    Public Property SysTotal() As Decimal
        Get
            Return decSysTotal
        End Get
        Set(ByVal value As Decimal)
            decSysTotal = value
        End Set
    End Property
    Private decSysTotal As Decimal
End Class
' Clase para la definición de la lista
Public Class ListaLineaAsientoTraslado

    Public Property FCCurrency() As String
        Get
            Return strFCCurrency
        End Get
        Set(ByVal value As String)
            strFCCurrency = value
        End Set
    End Property
    Private strFCCurrency As String

    Public Property Debit() As Decimal
        Get
            Return decDebit
        End Get
        Set(ByVal value As Decimal)
            decDebit = value
        End Set
    End Property
    Private decDebit As Decimal

    Public Property Credit() As Decimal
        Get
            Return decCredit
        End Get
        Set(ByVal value As Decimal)
            decCredit = value
        End Set
    End Property
    Private decCredit As Decimal

    Public Property FCDebit() As Decimal
        Get
            Return decFCDebit
        End Get
        Set(ByVal value As Decimal)
            decFCDebit = value
        End Set
    End Property
    Private decFCDebit As Decimal


    Public Property FCCredit() As Decimal
        Get
            Return decFCCredit
        End Get
        Set(ByVal value As Decimal)
            decFCCredit = value
        End Set
    End Property
    Private decFCCredit As Decimal

    Public Property Account() As String
        Get
            Return strAccount
        End Get
        Set(ByVal value As String)
            strAccount = value
        End Set
    End Property
    Private strAccount As String

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property ImpNeg() As String
        Get
            Return strImpNeg
        End Get
        Set(ByVal value As String)
            strImpNeg = value
        End Set
    End Property
    Private strImpNeg As String
End Class