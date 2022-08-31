Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.ProgramacionCitasDataSetTableAdapters
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System.Collections.Generic

Public Class GoodIssueCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company
    Private m_strUnidad As String
    Private m_strIDEntrada As String
    Private m_decTipoCambio As Decimal
    Private m_objBLSBO As New BLSBO.GlobalFunctionsSBO
    Private m_strFecha As String
    Private m_dtFecha As Date
    Private m_strMonedaSistema As String
    Private m_strMonedaLocal As String
    Private Const mc_strGoodIssues As String = "@SCGD_GOODISSUE"
    Private m_oFormGoodIssue As Form
    Private m_strIDVehiculo As String
    Private WithEvents SBO_Application As Application
    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private m_objConfiguracionGeneral As ConfiguracionesGeneralesAddon
    Private m_strNumeroEntrada() As String
    Private m_oitem As Item
    Private m_oMatriz As Matrix
    Public m_strNumeroSalida As String = String.Empty
    Private m_intTamañoMatriz As Integer
    Private m_blnAsignarCuentaCuentaSalidaAutomatica As Boolean = False
    Private m_ReversaSalidaMercancia As ReversarSalidaMercanciaCls
    Private m_decMontoSistema As Decimal
    Private m_decMontoLocal As Decimal
    Private g_strNoUnidad As String = String.Empty
    Private g_strDocEntry As String = String.Empty
    Private m_strNumeroAsiento() As String
    Public n As NumberFormatInfo
    Private oDataTableDimensionesContablesDMS As DataTable
    Private oDataTableDimensiones As Data.DataTable
    Private oDataTableConfiguracionDocumentosDimensiones As DataTable
    Private ListaConfiguracion As Hashtable
    Private oDataTableConsulta As DataTable
    Private m_oFormSalida As Form

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        n = DIHelper.GetNumberFormatInfo(p_oCompania)
    End Sub

    ''************************************************************
    '


    ''' <summary>
    ''' Se cargan la configuracion general del Addon DMS, para generar el asiento una vez que se crea la
    '''salida del Vehiculo en la facturacion del Contrato de Venta
    ''' </summary>
    ''' <param name="p_SBO_Aplication"></param>
    ''' <param name="p_oCompania"></param>
    ''' <param name="p_configuracionGenerales"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company, ByVal p_configuracionGenerales As SCGDataAccess.ConfiguracionesGeneralesAddon)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        m_objConfiguracionGeneral = p_configuracionGenerales

        n = DIHelper.GetNumberFormatInfo(p_oCompania)
    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub CargaFormularioGoodIssue(ByVal p_strIDGoodReceipt As String, Optional blnVieneKardex As Boolean = False)

        Dim oitem As SAPbouiCOM.Item
        Dim oMatriz As SAPbouiCOM.Matrix

        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.FormType = "SCGD_GOODISSUE"

            strXMLACargar = My.Resources.Resource.GOODISSUE
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGoodIssue = SBO_Application.Forms.AddEx(fcp)

            ' Inabilita el check de reversado
            m_oFormGoodIssue.Items.Item("chk_Rever").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)

            m_oFormGoodIssue.PaneLevel = 1
            m_strIDEntrada = p_strIDGoodReceipt
            m_objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            m_objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            oitem = m_oFormGoodIssue.Items.Item("mtx_0")
            oMatriz = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)
            oMatriz.Columns.Item("col_1").DataBind.SetBound(True, "@SCGD_GILINES", "U_SCGD_DocEntrada")
            oMatriz.Columns.Item("col_2").DataBind.SetBound(True, "@SCGD_GILINES", "U_SCGD_AsEntrada")
            oMatriz.Columns.Item("col_3").DataBind.SetBound(True, "@SCGD_GILINES", "U_SCGD_Monto")
            oMatriz.Columns.Item("col_4").DataBind.SetBound(True, "@SCGD_GILINES", "U_SCGD_MontoSist")

            If Not String.IsNullOrEmpty(m_strIDEntrada) Then

                If Not blnVieneKardex Then
                    Call CargarDatos()
                    m_oFormGoodIssue.Select()
                    m_oFormGoodIssue.ActiveItem = "13"
                    Utilitarios.FormularioDeshabilitado(m_oFormGoodIssue, False)
                Else
                    Call CargarSalida(p_strIDGoodReceipt)
                End If


            Else
                Utilitarios.FormularioDeshabilitado(m_oFormGoodIssue, True)

                m_oFormGoodIssue.EnableMenu("1282", False)

            End If

            Call InhabilitarButtonsPorUsuario(m_oFormGoodIssue)

            Dim oitemB As SAPbouiCOM.Item
            Dim oButton As SAPbouiCOM.Button


            oitemB = m_oFormGoodIssue.Items.Item("btnCtn")
            oButton = oitemB.Specific
            oButton.Image = System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP"

            'habilita el campo de fecha en caso de que no tenga factura asociada
            Dim editNoFact As EditText = DirectCast(m_oFormGoodIssue.Items.Item("31").Specific, EditText)

            Dim valor As String = CType(editNoFact.Value, String)


            If valor = String.Empty Or valor = "0" Then

                m_oFormGoodIssue.Items.Item("21").Enabled = True

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub



    Public Sub InhabilitarButtonsPorUsuario(ByRef p_form As SAPbouiCOM.Form)


        Dim strConectionString As String = ""
        Dim cnConexion As SqlClient.SqlConnection
        Dim dstSalidaModoConsulta As New SalidadModoConsultaporUsuario
        Dim dtaSalidaModoConsulta As New SalidadModoConsultaporUsuarioTableAdapters.SCGD_GI_CONSULTATableAdapter
        Dim strUsuarioSBO As String

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                 m_oCompany.CompanyDB, _
                                 strConectionString)

        cnConexion = New SqlClient.SqlConnection
        cnConexion.ConnectionString = strConectionString
        cnConexion.Open()
        dtaSalidaModoConsulta.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaSalidaModoConsulta.Connection = cnConexion

        strUsuarioSBO = SBO_Application.Company.UserName

        dtaSalidaModoConsulta.Fill(dstSalidaModoConsulta.SCGD_GI_CONSULTA, strUsuarioSBO)

        If Not dstSalidaModoConsulta.SCGD_GI_CONSULTA.Rows.Count = 0 Then

            p_form.Items.Item("1").Enabled = False
            p_form.Items.Item("2").Enabled = False
            p_form.Items.Item("btnPrint").Enabled = False
            p_form.Items.Item("btn_Genera").Enabled = False
            p_form.Mode = BoFormMode.fm_OK_MODE

        End If
        cnConexion.Close()

    End Sub

    Public Sub HabilitarBoton1(ByRef p_form As SAPbouiCOM.Form)


        Dim strConectionString As String = ""
        Dim cnConexion As SqlClient.SqlConnection
        Dim dstSalidaModoConsulta As New SalidadModoConsultaporUsuario
        Dim dtaSalidaModoConsulta As New SalidadModoConsultaporUsuarioTableAdapters.SCGD_GI_CONSULTATableAdapter
        Dim strUsuarioSBO As String

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                 m_oCompany.CompanyDB, _
                                 strConectionString)

        cnConexion = New SqlClient.SqlConnection
        cnConexion.ConnectionString = strConectionString
        cnConexion.Open()
        dtaSalidaModoConsulta.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaSalidaModoConsulta.Connection = cnConexion

        strUsuarioSBO = SBO_Application.Company.UserName

        dtaSalidaModoConsulta.Fill(dstSalidaModoConsulta.SCGD_GI_CONSULTA, strUsuarioSBO)

        If Not dstSalidaModoConsulta.SCGD_GI_CONSULTA.Rows.Count = 0 Then

            p_form.Items.Item("1").Enabled = True

        End If
        cnConexion.Close()

    End Sub

    Public Sub DesHabilitarBoton1(ByRef p_form As SAPbouiCOM.Form)


        Dim strConectionString As String = ""
        Dim cnConexion As SqlClient.SqlConnection
        Dim dstSalidaModoConsulta As New SalidadModoConsultaporUsuario
        Dim dtaSalidaModoConsulta As New SalidadModoConsultaporUsuarioTableAdapters.SCGD_GI_CONSULTATableAdapter
        Dim strUsuarioSBO As String


        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                 m_oCompany.CompanyDB, _
                                 strConectionString)

        cnConexion = New SqlClient.SqlConnection
        cnConexion.ConnectionString = strConectionString
        cnConexion.Open()
        dtaSalidaModoConsulta.Connection = New SqlClient.SqlConnection(strConectionString)
        dtaSalidaModoConsulta.Connection = cnConexion

        strUsuarioSBO = SBO_Application.Company.UserName

        dtaSalidaModoConsulta.Fill(dstSalidaModoConsulta.SCGD_GI_CONSULTA, strUsuarioSBO)

        If Not dstSalidaModoConsulta.SCGD_GI_CONSULTA.Rows.Count = 0 Then

            p_form.Items.Item("1").Enabled = False

        End If
        cnConexion.Close()

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

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                              ByRef pVal As SAPbouiCOM.ItemEvent, _
                                              ByRef BubbleEvent As Boolean)
        Dim oform As SAPbouiCOM.Form

        Try

            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If Not oform Is Nothing _
                AndAlso oform.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
                oCFLEvento = pVal
                Dim sCFL_ID As String
                sCFL_ID = oCFLEvento.ChooseFromListUID
                Dim oCFL As SAPbouiCOM.ChooseFromList

                Dim oConditions As SAPbouiCOM.Conditions
                Dim oCondition As SAPbouiCOM.Condition

                oCFL = oform.ChooseFromLists.Item(sCFL_ID)

                oConditions = SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.Alias = "Levels"
                oCondition.Operation = BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = "1"

                oCFL.SetConditions(oConditions)


                If Not oCFLEvento.BeforeAction _
                       AndAlso oCFLEvento.ActionSuccess Then
                    Dim oDataTable As SAPbouiCOM.DataTable
                    oDataTable = oCFLEvento.SelectedObjects

                    If Not oDataTable Is Nothing Then

                        Dim editFormatCode As EditText = DirectCast(oform.Items.Item("txtFormatC").Specific, EditText)
                        Dim editDescripcion As EditText = DirectCast(oform.Items.Item("txtDscp").Specific, EditText)

                        oform.DataSources.DBDataSources.Item("@SCGD_GOODISSUE").SetValue("U_NCuenCnt", 0, oDataTable.GetValue("AcctCode", 0))
                        editFormatCode.Value = oDataTable.GetValue("FormatCode", 0)
                        editDescripcion.Value = oDataTable.GetValue("AcctName", 0)


                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Sub AgregarValoresCuenta(ByVal p_form As SAPbouiCOM.Form)

        Dim editFormatCode As EditText = DirectCast(p_form.Items.Item("txtFormatC").Specific, EditText)
        Dim editDescripcion As EditText = DirectCast(p_form.Items.Item("txtDscp").Specific, EditText)

        Dim strAcctCode As String = p_form.DataSources.DBDataSources.Item("@SCGD_GOODISSUE").GetValue("U_NCuenCnt", 0).Trim

        If Not strAcctCode = "" Then

            editFormatCode.Value = Utilitarios.EjecutarConsulta("Select FormatCode from OACT with(nolock) where AcctCode = '" & strAcctCode & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            editDescripcion.Value = Utilitarios.EjecutarConsulta("Select AcctName from OACT with(nolock) where AcctCode = '" & strAcctCode & "'", m_oCompany.CompanyDB, m_oCompany.Server)

        Else

            editFormatCode.Value = String.Empty
            editDescripcion.Value = String.Empty

        End If

    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If oForm.Mode = BoFormMode.fm_FIND_MODE AndAlso pVal.BeforeAction Then Return

            If pVal.BeforeAction Then

                Dim strUnidad As String = oForm.Items.Item("7").Specific.string
                g_strNoUnidad = strUnidad.Trim


                Select Case pVal.ItemUID
                    Case "1"

                        Dim strCuentaInventario As String = String.Empty
                        Dim strCuentaCostos As String = String.Empty
                        Dim strTipoVehiculo As String = String.Empty
                        Dim strInvFacturado As String = String.Empty

                        strInvFacturado = m_objConfiguracionGeneral.InventarioVehiculoVendido

                        strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", g_strNoUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim

                        'Comparo el inventario de la Unidad con el Inventario "Post Venta"
                        If strTipoVehiculo = strInvFacturado Then
                            strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo_Ven FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", g_strNoUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                        End If

                        strCuentaInventario = m_objConfiguracionGeneral.CuentaStock(strTipoVehiculo)
                        strCuentaCostos = m_objConfiguracionGeneral.CuentaCosto(strTipoVehiculo)

                        If String.IsNullOrEmpty(strCuentaInventario) Or String.IsNullOrEmpty(strCuentaCostos) Then

                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCuentasInventarioCostos, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False

                        End If

                        If CargarTipoCambio() = False Then

                            SBO_Application.MessageBox(My.Resources.Resource.TipoCambioNoActualizado)
                            BubbleEvent = False

                        End If

                        m_oitem = m_oFormGoodIssue.Items.Item("mtx_0")
                        m_oMatriz = DirectCast(m_oitem.Specific, SAPbouiCOM.Matrix)

                        m_intTamañoMatriz = m_oMatriz.RowCount

                        For i As Integer = 0 To m_oMatriz.RowCount - 1

                            ReDim Preserve m_strNumeroEntrada(i)
                            m_strNumeroEntrada(i) = m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").GetValue("U_SCGD_DocEntrada", i)
                            m_strNumeroEntrada(i) = m_strNumeroEntrada(i).Trim()

                            ReDim Preserve m_strNumeroAsiento(i)
                            m_strNumeroAsiento(i) = m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").GetValue("U_SCGD_AsEntrada", i).ToString.Trim()


                        Next i

                        Select Case pVal.FormMode
                            Case SAPbouiCOM.BoFormMode.fm_ADD_MODE

                                Dim strMonedaSistema As String = m_objBLSBO.RetornarMonedaSistema()
                                Dim strMonedaLocal As String = m_objBLSBO.RetornarMonedaLocal()

                                Dim blnUtilizaCosteoAccesorios As String = Utilitarios.EjecutarConsulta("Select U_UsaAxC from dbo.[@SCGD_ADMIN] with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

                                If Utilitarios.ConsultaCosteos(strUnidad, m_oCompany.CompanyDB, m_oCompany.Server, strMonedaSistema, strMonedaLocal, blnUtilizaCosteoAccesorios) Then
                                    If SBO_Application.MessageBox(Text:=My.Resources.Resource.PreguntaSalida, DefaultBtn:=2, Btn1Caption:=My.Resources.Resource.Si, Btn2Caption:="No") = 1 Then
                                        If SBO_Application.MessageBox(My.Resources.Resource.NopuedeModificar, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                            BubbleEvent = False
                                        End If
                                    Else
                                        BubbleEvent = False
                                    End If
                                Else
                                    If SBO_Application.MessageBox(My.Resources.Resource.NopuedeModificar, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                        BubbleEvent = False
                                    End If
                                End If
                        End Select

                End Select

            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "1"

                        Dim oCompanyService As SAPbobsCOM.CompanyService
                        Dim oGeneralService As SAPbobsCOM.GeneralService
                        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
                        Dim oGeneralData As SAPbobsCOM.GeneralData

                        For i As Integer = 0 To m_intTamañoMatriz - 1

                            If Not String.IsNullOrEmpty(m_strNumeroEntrada(i)) Then

                                Dim strNumeroSalida(i) As String
                                strNumeroSalida(i) = Utilitarios.EjecutarConsulta("Select DocEntry from [@SCGD_GILINES] with(nolock) where U_SCGD_DocEntrada = '" & m_strNumeroEntrada(i) & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                                If Not String.IsNullOrEmpty(strNumeroSalida(i)) Then
                                    g_strDocEntry = strNumeroSalida(i).ToString.Trim()
                                End If

                                oCompanyService = m_oCompany.GetCompanyService()
                                oGeneralService = oCompanyService.GetGeneralService("SCGD_GOODENT")
                                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                                oGeneralParams.SetProperty("DocEntry", m_strNumeroEntrada(i))
                                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                                oGeneralData.SetProperty("U_SCGD_DocSalida", strNumeroSalida(i))
                                oGeneralService.Update(oGeneralData)

                            End If

                        Next i

                        Call AgregarValoresCuenta(oForm)

                        Select Case pVal.FormMode
                            Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                                'CrearAsientos()
                                If Not String.IsNullOrEmpty(g_strDocEntry) And Not String.IsNullOrEmpty(g_strNoUnidad) Then
                                    CrearAsientoSalidaVehiculo(g_strDocEntry, g_strNoUnidad)
                                End If

                        End Select

                    Case "btnPrint"
                        If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            Call ImprimirReporteCostoVehiculo(FormUID, pVal, BubbleEvent)
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCrearDocumentoAntesImprimir, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        End If
                    Case "btn_Genera"
                        'CrearAsientos()
                        Dim strNoUnidad As String = oForm.Items.Item("7").Specific.string
                        Dim strNoSalida As String = oForm.DataSources.DBDataSources.Item("@SCGD_GOODISSUE").GetValue("DocEntry", 0).Trim()
                        Dim strAsientoSalida As String = oForm.DataSources.DBDataSources.Item("@SCGD_GOODISSUE").GetValue("U_As_Sali", 0).Trim()

                        If Not String.IsNullOrEmpty(strNoSalida) And Not String.IsNullOrEmpty(strNoUnidad) And (strAsientoSalida = "-1" Or String.IsNullOrEmpty(strAsientoSalida)) Then
                            CrearAsientoSalidaVehiculo(strNoSalida, strNoUnidad)
                        End If

                End Select

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub


    'Agregado 19/11/2010: Devulve el numero de entrada para cargar desde matriz de salida
    Public Function DevolverDatoGoodReceipt(ByVal p_strFormID As String, Optional ByVal p_intNoFila As Integer = -1) As String

        Dim oForm As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intFila As Integer
        Dim strIDEntrada As String = String.Empty

        oForm = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oForm.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)
        If p_intNoFila = -1 Then
            intFila = oMatriz.GetNextSelectedRow()
            If intFila > -1 Then
                strIDEntrada = oMatriz.Columns.Item("col_1").Cells.Item(intFila).Specific.String()
                oMatriz.ClearSelections()
            End If
        Else
            strIDEntrada = oMatriz.Columns.Item("col_1").Cells.Item(p_intNoFila).Specific.String()
        End If
        Return strIDEntrada

    End Function

    Private Function CargarTipoCambio() As Boolean

        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item("SCGD_GOODISSUE")

        m_strFecha = DirectCast(oform.Items.Item("21").Specific, EditText).Value
        If (String.IsNullOrEmpty(m_strFecha)) Then
            m_strFecha = Date.Now.ToString("yyyyMMdd")
        End If
        m_dtFecha = Date.ParseExact(m_strFecha, "yyyyMMdd", Nothing)
        m_dtFecha = New Date(m_dtFecha.Year, m_dtFecha.Month, m_dtFecha.Day, 0, 0, 0)

        m_objBLSBO.Set_Compania(m_oCompany)
        m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()
        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
        If m_strMonedaLocal <> m_strMonedaSistema Then
            m_decTipoCambio = m_objBLSBO.RetornarTipoCambioMoneda(m_strMonedaSistema, m_dtFecha, m_strConectionString, False)
            If m_decTipoCambio = -1 Then
                Return False
            End If
        Else
            m_decTipoCambio = 1
        End If

        Return True

    End Function

    'Agregado 20/12/2012 - Johnny Vargas: Metodo para manejo de costo total en trazabilidad del vehículo

    Public Sub ActualizaCostoVehiculo(ByVal p_strIDVehiculo As String, _
                                      ByVal p_decCostoSistema As Decimal, _
                                      ByVal p_decCostoLocal As Decimal,
                                      Optional ByVal SumarCosto As Boolean = False)

        Dim n As NumberFormatInfo

        m_objBLSBO.Set_Compania(m_oCompany)
        m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()
        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

        Dim l_decCostoVehiculo As Decimal
        Dim l_decCostoVehiculoS As Decimal

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildTrazabilidad As SAPbobsCOM.GeneralData
        Dim oChildrenTrazabilidad As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim dcCostoAcumulado As Decimal = 0
        Dim dcCostoAcumuladoS As Decimal = 0

        oCompanyService = m_oCompany.GetCompanyService()
        oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
        oGeneralParams.SetProperty("Code", p_strIDVehiculo)
        oGeneralData = oGeneralService.GetByParams(oGeneralParams)

        oChildrenTrazabilidad = oGeneralData.Child("SCGD_VEHITRAZA")

        If oGeneralData.Child("SCGD_VEHITRAZA").Count = 0 Then
            oChildTrazabilidad = oChildrenTrazabilidad.Add()
        Else
            oChildTrazabilidad = oChildrenTrazabilidad.Item(0)
        End If

        If SumarCosto Then
            Dim ValorTraza As String = ""
            ValorTraza = oChildTrazabilidad.GetProperty("U_ValVeh")

            If Not String.IsNullOrEmpty(ValorTraza) Then
                dcCostoAcumulado = Decimal.Parse(ValorTraza)
            Else
                dcCostoAcumulado = 0
            End If


            Dim ValorTrazaS As String = ""
            ValorTrazaS = oChildTrazabilidad.GetProperty("U_ValVehS")

            If Not String.IsNullOrEmpty(ValorTrazaS) Then
                dcCostoAcumuladoS = Decimal.Parse(ValorTrazaS)
            Else
                dcCostoAcumuladoS = 0
            End If
        End If

        l_decCostoVehiculo = p_decCostoLocal
        If SumarCosto Then l_decCostoVehiculo = l_decCostoVehiculo + dcCostoAcumulado

        l_decCostoVehiculoS = p_decCostoSistema
        If SumarCosto Then l_decCostoVehiculoS = l_decCostoVehiculoS + dcCostoAcumuladoS

        oChildTrazabilidad.SetProperty("U_ValVeh", l_decCostoVehiculo.ToString())
        oChildTrazabilidad.SetProperty("U_ValVehS", l_decCostoVehiculoS.ToString())

        oGeneralService.Update(oGeneralData)

    End Sub

    ''' <summary>
    ''' se crea el asiento para un numero de entrada especifico
    ''' se obtiene cuando se crea el ingreso contable del vehiculo usado
    ''' </summary>
    ''' <param name="p_intNumeroSalida"></param>
    ''' <remarks></remarks>
    Public Sub CrearAsientoParaNumeroSalidaEspecifico(ByVal p_intNumeroSalida As Integer, Optional ByVal p_fechaDocumento As Date = Nothing)

        m_blnAsignarCuentaCuentaSalidaAutomatica = True

        CrearAsiento(p_intNumeroSalida, p_fechaDocumento)

    End Sub

    ' Nuevo metodo para crear asientos por numero de salida
    Public Sub CrearAsientoPorNoSalida(ByVal p_strNoSalida As String, ByVal p_strNoUnidad As String, Optional ByVal p_dateFechaContrato As Date = Nothing, _
                                       Optional ByVal p_blnUsaDimension As Boolean = False, _
                                       Optional ByVal p_ListaConfiguracion As Hashtable = Nothing,
                                       Optional ByVal p_UsaCompTran As Boolean = True, _
                                       Optional ByVal p_DtLines As Data.DataTable = Nothing)

        m_blnAsignarCuentaCuentaSalidaAutomatica = True

        CrearAsientoSalidaVehiculo(p_strNoSalida, p_strNoUnidad, p_dateFechaContrato, p_blnUsaDimension, Nothing, p_UsaCompTran, p_DtLines)

    End Sub

    Public Sub ActualizarCampoCuentaContableSalidaVehiculo(ByVal p_docentry As Integer, ByVal p_unidad As String, _
                                                           ByVal p_tipovehiculo As String, ByVal p_contraCuenta As String)


        Utilitarios.EjecutarConsulta("Update [@SCGD_GOODISSUE] set U_NCuenCnt = '" & p_contraCuenta & "'where DocEntry = " & p_docentry, m_oCompany.CompanyDB, m_oCompany.Server)

    End Sub

    Public Sub CrearAsientos()

        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String
        Dim cmdGoodEntries As New SqlClient.SqlCommand
        Dim drdGoodEntries As SqlClient.SqlDataReader
        Dim blnSeguirCreando As Boolean = True

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
        cn_Coneccion.ConnectionString = strConectionString
        cn_Coneccion.Open()

        cmdGoodEntries.Connection = cn_Coneccion
        strConsulta = "Select DocEntry from [@SCGD_GOODISSUE] where U_As_Sali is null"
        cmdGoodEntries.CommandType = CommandType.Text
        cmdGoodEntries.CommandText = strConsulta
        drdGoodEntries = cmdGoodEntries.ExecuteReader()

        Do While drdGoodEntries.Read

            CrearAsiento(drdGoodEntries.GetInt32(0))

        Loop

        drdGoodEntries.Close()

    End Sub

    Public Function CrearAsiento(ByVal p_intDocEntry As Integer, Optional ByVal p_fechaDocumento As Date = Nothing) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim strNoAsiento As String

        Dim decTotal As Decimal
        Dim strCuenta As String = String.Empty
        Dim strContraCuenta As String = String.Empty

        Dim strCuentaSeleccionada As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty

        Dim strConectionString As String = String.Empty
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = String.Empty
        Dim cmdContraCuentas As New SqlClient.SqlCommand
        Dim drdContraCuentas As SqlClient.SqlDataReader
        Dim blnEntradaInvalida As Boolean = False
        Dim strDocEntrada As String = String.Empty
        Dim AñoFechaCorte As String = String.Empty
        Dim MesFechaCorte As String = String.Empty
        Dim DiaFechaCorte As String = String.Empty
        Dim HoraCreacion As String = String.Empty

        Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls
        Dim blnUsaDimensiones As Boolean = False

        Try

            strNoAsiento = 0
            m_oCompany.StartTransaction()
            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            m_strUnidad = Utilitarios.EjecutarConsulta("Select U_Unidad from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server)
            strDocEntrada = Utilitarios.EjecutarConsulta("Select U_Doc_Entr from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdContraCuentas.Connection = cn_Coneccion

            cmdContraCuentas.CommandType = CommandType.Text
            cmdContraCuentas.CommandText = "Update [@SCGD_GOODRECEIVE] set Status = 'C' where U_Unidad =  '" & m_strUnidad & "'"
            cmdContraCuentas.ExecuteNonQuery()

            AñoFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(YEAR,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry <> " & p_intDocEntry & " and U_Unidad = '" & m_strUnidad & "' order by DocEntry DESC", m_oCompany.CompanyDB, m_oCompany.Server)
            MesFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(MONTH,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry <> " & p_intDocEntry & " and U_Unidad = '" & m_strUnidad & "' order by DocEntry DESC", m_oCompany.CompanyDB, m_oCompany.Server)
            DiaFechaCorte = Utilitarios.EjecutarConsulta("Select Datepart(DAY,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry <> " & p_intDocEntry & " and U_Unidad = '" & m_strUnidad & "' order by DocEntry DESC", m_oCompany.CompanyDB, m_oCompany.Server)
            HoraCreacion = Utilitarios.EjecutarConsulta("Select Case len(cast(CreateTime as nvarchar)) when 3 then '0' + Substring(cast(CreateTime as nvarchar),0,2) + ':' + Substring(cast(CreateTime as nvarchar),2,4) + ':59' " & _
                                                   "when 4 then Substring(cast(CreateTime as nvarchar),0,3) + ':' + Substring(cast(CreateTime as nvarchar),3,4) + ':59' when 1 then '00:0' + cast(CreateTime as nvarchar) + ':59' " & _
                                                   "else '00:' + cast(CreateTime as nvarchar) + ':59' end as Hora from [@SCGD_GOODISSUE] with(nolock) where DocEntry <> " & p_intDocEntry & " and U_Unidad = '" & m_strUnidad & "' order by DocEntry DESC", m_oCompany.CompanyDB, m_oCompany.Server)

            If MesFechaCorte.Length = 1 Then
                MesFechaCorte = "0" & MesFechaCorte
            End If

            If DiaFechaCorte.Length = 1 Then
                DiaFechaCorte = "0" & DiaFechaCorte
            End If

            If Not String.IsNullOrEmpty(HoraCreacion) Then
                HoraCreacion = " " & HoraCreacion
            End If


            m_dtFecha = New Date(Utilitarios.EjecutarConsulta("Select Datepart(YEAR,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                                           Utilitarios.EjecutarConsulta("Select Datepart(MONTH,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                                           Utilitarios.EjecutarConsulta("Select Datepart(DAY,Isnull(U_Fech_Con,Getdate())) from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                                           0, 0, 0)

            m_objBLSBO.Set_Compania(m_oCompany)
            m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()
            m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
            If Not String.IsNullOrEmpty(m_strUnidad) Then
                strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo_Ven from [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '" & m_strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                If String.IsNullOrEmpty(strTipoVehiculo) Or strTipoVehiculo = "'NULL'" Then
                    strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo from [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '" & m_strUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                End If

                If Not m_objConfiguracionGeneral Is Nothing Then
                    strCuenta = m_objConfiguracionGeneral.CuentaStock(strTipoVehiculo)
                    strContraCuenta = m_objConfiguracionGeneral.CuentaCosto(strTipoVehiculo)
                Else
                    strCuenta = Utilitarios.EjecutarConsulta("Select U_Stock from [@SCGD_ADMIN4] with (nolock) where U_Tipo  = '" & strTipoVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                    strContraCuenta = Utilitarios.EjecutarConsulta("Select U_Costo from [@SCGD_ADMIN4] with (nolock) where U_Tipo = '" & strTipoVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                End If


                strCuentaSeleccionada = Utilitarios.EjecutarConsulta("Select U_NCuenCnt from [@SCGD_GOODISSUE] with(nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server)

                'verifico si las cuentas son las mismas
                If Not strContraCuenta = strCuentaSeleccionada And strCuentaSeleccionada <> String.Empty Then

                    strContraCuenta = strCuentaSeleccionada

                End If

                If p_fechaDocumento <> Nothing Then
                    oJournalEntry.ReferenceDate = p_fechaDocumento

                Else
                    oJournalEntry.ReferenceDate = m_dtFecha
                End If


                oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"

                strConsulta = String.Format(
                    "Select	GR.U_As_Entr, Case GR.U_As_Entr when 0 then SUM (U_Tot_Loc) else Case JDT1.U_SCGD_ImpNeg " & _
                    "when 'Y' then Isnull(Sum( JDT1.Debit),0) * -1 else Isnull(Sum( JDT1.Debit),0) end end Monto from [@SCGD_GOODISSUE] " & _
                    "inner join [@SCGD_GOODRECEIVE] GR ON [@SCGD_GOODISSUE].U_Unidad = GR.U_Unidad left outer join JDT1 " & _
                    "on GR.U_As_Entr = JDT1.TransID where [@SCGD_GOODISSUE].DocEntry = '{0}' and GR.U_Fec_Cont >= '' and " & _
                    "GR.[U_SCGD_DocSalida] = '{0}' and GR.U_As_Entr <> -1 group by GR.U_As_Entr, JDT1.U_SCGD_ImpNeg", p_intDocEntry)

                cmdContraCuentas.Connection = cn_Coneccion

                cmdContraCuentas.CommandType = CommandType.Text
                cmdContraCuentas.CommandText = strConsulta
                drdContraCuentas = cmdContraCuentas.ExecuteReader()

                decTotal = 0

                Dim strUsaDimensiones As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

                If Not String.IsNullOrEmpty(strUsaDimensiones) Then

                    If strUsaDimensiones = "Y" Then

                        Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '" & m_strUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                        ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)

                        oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))

                        If oDataTableDimensiones.Rows.Count <> 0 Then

                            blnUsaDimensiones = True

                        End If
                    End If
                End If

                Do While drdContraCuentas.Read
                    decTotal += drdContraCuentas.GetDecimal(1)
                Loop

                If Not String.IsNullOrEmpty(strCuenta) And
                    Not String.IsNullOrEmpty(strContraCuenta) Then

                    'encabezado del asiento
                    oJournalEntry.Reference = m_strUnidad
                    oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoSalida & " " & m_strUnidad

                    'Crea la linea Credito para el asiento de salida
                    oJournalEntry.Lines.AccountCode = strCuenta
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    If blnUsaDimensiones Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                    End If
                    oJournalEntry.Lines.Credit = decTotal
                    'Crea la linea Debito para el asiento de salida
                    oJournalEntry.Lines.Add()
                    oJournalEntry.Lines.AccountCode = strContraCuenta
                    oJournalEntry.Lines.Debit = decTotal

                    If blnUsaDimensiones Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                    End If

                End If

                drdContraCuentas.Close()

                If decTotal <= 0 Then

                    blnEntradaInvalida = True

                Else
                    If oJournalEntry.Add <> 0 Then
                        If decTotal <= 0 Then
                            blnEntradaInvalida = True
                        Else
                            strNoAsiento = "0"
                            m_oCompany.GetLastError(intError, strMensajeError)
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            Throw New ExceptionsSBO(intError, strMensajeError)
                        End If
                    Else
                        m_oCompany.GetNewObjectCode(strNoAsiento)

                        'aqui se agrega la cuenta cuando se hace la salida automatica
                        If m_blnAsignarCuentaCuentaSalidaAutomatica Then

                            cmdContraCuentas.CommandType = CommandType.Text
                            cmdContraCuentas.CommandText = "Update [@SCGD_GOODISSUE] set U_As_Sali = " & strNoAsiento & ", U_NCuenCnt = '" & strContraCuenta & "' where docentry = " & p_intDocEntry
                            cmdContraCuentas.ExecuteNonQuery()
                            cmdContraCuentas.Connection.Close()

                            m_blnAsignarCuentaCuentaSalidaAutomatica = False

                        Else
                            cmdContraCuentas.CommandType = CommandType.Text
                            cmdContraCuentas.CommandText = "Update [@SCGD_GOODISSUE] set U_As_Sali = " & strNoAsiento & " where docentry = " & p_intDocEntry
                            cmdContraCuentas.ExecuteNonQuery()
                            cmdContraCuentas.Connection.Close()
                        End If

                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        End If
                    End If
                End If
            Else
                blnEntradaInvalida = True
            End If

            If blnEntradaInvalida Then
                If cn_Coneccion.State <> ConnectionState.Open Then
                    Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
                    cn_Coneccion.ConnectionString = strConectionString
                    cn_Coneccion.Open()
                End If
                cmdContraCuentas.Connection = cn_Coneccion
                cmdContraCuentas.CommandType = CommandType.Text
                strNoAsiento = "-1"
                cmdContraCuentas.CommandText = "Update [@SCGD_GOODISSUE] set U_As_Sali = " & strNoAsiento & " where DocEntry = " & p_intDocEntry
                cmdContraCuentas.ExecuteNonQuery()
                cmdContraCuentas.Connection.Close()
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
            End If
            Return CInt(strNoAsiento)

        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try
    End Function

    Private Sub CargarDatos()

        Dim strValorSeleccionado As String = String.Empty
        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty
        Dim strIDFactura As String = String.Empty
        Dim strNumContratoVen As String = String.Empty
        Dim strFecha As String = String.Empty
        Dim strcantidadGoodReceipts As String = String.Empty
        Dim intCantidad As Integer

        Dim strCuentaCostos As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty
        Dim strTipoVehiculo2 As String = String.Empty

        Dim strFechaCorte As String = String.Empty
        Dim strMarca As String = String.Empty
        Dim strEstilo As String = String.Empty
        Dim strModelo As String = String.Empty
        Dim strVin As String = String.Empty

        Dim n As NumberFormatInfo

        n = DIHelper.GetNumberFormatInfo(m_oCompany)

        Dim oeditFormatCode As EditText = m_oFormGoodIssue.Items.Item("txtFormatC").Specific
        Dim oeditDescripcion As EditText = m_oFormGoodIssue.Items.Item("txtDscp").Specific


        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        m_oFormGoodIssue.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE

        'Desactivo el check
        Dim oitemC As SAPbouiCOM.Item

        oitemC = m_oFormGoodIssue.Items.Item("chk_Rever")

        oitemC.Enabled = False


        'consulta a la GoodReceive para traerse los valores del vehiculo
        If Utilitarios.ValidaExisteDataTable(m_oFormGoodIssue, "tConsulta") Then
            oDataTableConsulta = m_oFormGoodIssue.DataSources.DataTables.Item("tConsulta")
        Else
            oDataTableConsulta = m_oFormGoodIssue.DataSources.DataTables.Add("tConsulta")
        End If

        oDataTableConsulta.ExecuteQuery(String.Format("SELECT U_Unidad, U_Marca, U_Estilo, U_Modelo, U_VIN FROM dbo.[@SCGD_GOODRECEIVE] with(nolock) where DocEntry = {0}", m_strIDEntrada))

        m_strUnidad = oDataTableConsulta.GetValue("U_Unidad", 0).ToString.Trim
        strMarca = oDataTableConsulta.GetValue("U_Marca", 0).ToString.Trim
        strEstilo = oDataTableConsulta.GetValue("U_Estilo", 0).ToString.Trim
        strModelo = oDataTableConsulta.GetValue("U_Modelo", 0).ToString.Trim
        strVin = oDataTableConsulta.GetValue("U_VIN", 0).ToString.Trim

        m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Unidad", 0, m_strUnidad)
        m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Marca", 0, strMarca)
        m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Estilo", 0, strEstilo)
        m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Modelo", 0, strModelo)
        m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_VIN", 0, strVin)


        strFechaCorte = Date.Now.ToString("yyyyMMdd")
        strcantidadGoodReceipts = Utilitarios.EjecutarConsulta(String.Format("Select Count(*) from [@SCGD_GOODRECEIVE] with(nolock) where U_Unidad = '{0}' and U_As_Entr <> -1 and U_Fec_Cont <= '{1}' and U_SCGD_Trasl <> 'Y' and Status = 'O' and (U_SCGD_DocSalida = '' or U_SCGD_DocSalida is null)", m_strUnidad, strFechaCorte), m_oCompany.CompanyDB, m_oCompany.Server)

        If IsNumeric(strcantidadGoodReceipts) Then
            intCantidad = CInt(strcantidadGoodReceipts)
        End If

        If intCantidad <= 1 Then
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Doc_Entr", 0, m_strIDEntrada)

            strValorSeleccionado = Utilitarios.EjecutarConsulta("Select U_As_Entr from [@SCGD_GOODRECEIVE] with(nolock) where DocEntry = " & m_strIDEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_As_Entr", 0, strValorSeleccionado)
        End If

        'Agregado 18/11/2010: Carga entradas relacionadas a la unidad especificada

        Dim dstSalidas As LineasSalidaDataset = New LineasSalidaDataset()

        Dim adpSalidas As LineasSalidaDatasetTableAdapters.SCGTA_TB_SalidasVehiculosTableAdapter = New LineasSalidaDatasetTableAdapters.SCGTA_TB_SalidasVehiculosTableAdapter()
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
        adpSalidas.CadenaConexion = strConectionString
        adpSalidas.Fill(dstSalidas.SCGTA_TB_SalidasVehiculos, m_strUnidad)

        Dim strDocEntrada As String
        Dim strAsEntrada As String
        Dim strMonto As String
        Dim strMontoSist As String

        Dim intPosicion As Integer = 0

        Dim oitem As SAPbouiCOM.Item
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strMontoLocal As String = String.Empty
        Dim strMontoSistema As String = String.Empty

        oitem = m_oFormGoodIssue.Items.Item("mtx_0")
        oMatriz = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

        m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").Clear()

        For Each salidaRow As LineasSalidaDataset.SCGTA_TB_SalidasVehiculosRow In dstSalidas.SCGTA_TB_SalidasVehiculos

            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").InsertRecord(intPosicion)

            strDocEntrada = salidaRow.DocEntry
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("U_SCGD_DocEntrada", intPosicion, strDocEntrada)
            strAsEntrada = salidaRow.U_As_Entr
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("U_SCGD_AsEntrada", intPosicion, strAsEntrada)
            strMonto = salidaRow.U_GASTRA
            strMontoLocal = Utilitarios.ObtenerFormatoSAP(salidaRow.U_GASTRA, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("U_SCGD_Monto", intPosicion, strMontoLocal)
            strMontoSist = salidaRow.U_GASTRA_S
            strMontoSistema = Utilitarios.ObtenerFormatoSAP(salidaRow.U_GASTRA_S, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("U_SCGD_MontoSist", intPosicion, strMontoSistema)
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("LineId", intPosicion, (intPosicion + 1).ToString())
            m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").SetValue("VisOrder", intPosicion, intPosicion.ToString())

            intPosicion = intPosicion + 1

        Next

        oMatriz.LoadFromDataSource()

        'Monto Local
        If intCantidad <= 1 Then
            strValorSeleccionado = Utilitarios.EjecutarConsulta(String.Format("Select U_GASTRA from [@SCGD_GOODRECEIVE] with(nolock) where DocEntry = {0}", m_strIDEntrada), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                m_decMontoLocal = Decimal.Parse(strValorSeleccionado)
            End If

            strValorSeleccionado = Utilitarios.ObtenerFormatoSAP(m_decMontoLocal, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Cos_Loc", 0, strValorSeleccionado)

        Else
            strValorSeleccionado = Utilitarios.EjecutarConsulta(String.Format("Select SUM(U_GASTRA) U_Tot_Loc from [@SCGD_GOODRECEIVE] with(nolock) where U_Unidad = '{0}' and U_SCGD_Trasl = 'N' and  U_Fec_Cont <= '{1}' and U_As_Entr <> -1 and Status = 'O' and (U_SCGD_DocSalida = '' or U_SCGD_DocSalida is null)", m_strUnidad, strFechaCorte), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                m_decMontoLocal = Decimal.Parse(strValorSeleccionado)
            End If

            strValorSeleccionado = Utilitarios.ObtenerFormatoSAP(m_decMontoLocal, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Cos_Loc", 0, strValorSeleccionado)

        End If

        'Monto Sistema
        If intCantidad <= 1 Then
            strValorSeleccionado = Utilitarios.EjecutarConsulta(String.Format("Select U_GASTRA_S from [@SCGD_GOODRECEIVE] with(nolock) where DocEntry = {0}", m_strIDEntrada), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                m_decMontoSistema = Decimal.Parse(strValorSeleccionado)
            End If

            strValorSeleccionado = Utilitarios.ObtenerFormatoSAP(m_decMontoSistema, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Cos_Sis", 0, strValorSeleccionado)

        Else
            strValorSeleccionado = Utilitarios.EjecutarConsulta(String.Format("Select SUM(U_GASTRA_S) from [@SCGD_GOODRECEIVE] with(nolock) where U_Unidad = '{0}' and U_SCGD_Trasl = 'N' and U_Fec_Cont <= '{1}' and U_As_Entr <> -1 and Status = 'O' and (U_SCGD_DocSalida = '' or U_SCGD_DocSalida is null)", m_strUnidad, strFechaCorte), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strValorSeleccionado) Then
                m_decMontoSistema = Decimal.Parse(strValorSeleccionado)
            End If

            strValorSeleccionado = Utilitarios.ObtenerFormatoSAP(m_decMontoSistema, strSeparadorMilesSAP, strSeparadorDecimalesSAP)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Cos_Sis", 0, strValorSeleccionado)

        End If


        'Valores del Vehiculo
        oDataTableConsulta = m_oFormGoodIssue.DataSources.DataTables.Item("tConsulta")
        oDataTableConsulta.ExecuteQuery(String.Format("SELECT Code,U_Tipo_Ven,U_Tipo,U_CTOVTA,U_NUMFAC FROM dbo.[@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad))

        m_strIDVehiculo = oDataTableConsulta.GetValue("Code", 0).ToString.Trim
        strTipoVehiculo = oDataTableConsulta.GetValue("U_Tipo_Ven", 0).ToString.Trim
        strTipoVehiculo2 = oDataTableConsulta.GetValue("U_Tipo", 0).ToString.Trim
        strNumContratoVen = oDataTableConsulta.GetValue("U_CTOVTA", 0).ToString.Trim
        strIDFactura = oDataTableConsulta.GetValue("U_NUMFAC", 0).ToString.Trim


        If Not String.IsNullOrEmpty(m_strIDVehiculo) Then

            If String.IsNullOrEmpty(strTipoVehiculo) Then
                strTipoVehiculo = strTipoVehiculo2
            End If

            strCuentaCostos = m_objConfiguracionGeneral.CuentaCosto(strTipoVehiculo)

            oeditFormatCode.Value = Utilitarios.EjecutarConsulta("Select FormatCode from OACT with(nolock) where AcctCode = '" & strCuentaCostos & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            oeditDescripcion.Value = Utilitarios.EjecutarConsulta("Select AcctName from OACT with(nolock) where AcctCode = '" & strCuentaCostos & "'", m_oCompany.CompanyDB, m_oCompany.Server)

            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_ID_Veh", 0, m_strIDVehiculo)
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_NCuenCnt", 0, strCuentaCostos)

            If strNumContratoVen <> 0 Then
                m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_NoCont", 0, strNumContratoVen)
                m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_NoFact", 0, strIDFactura)
            End If

            strFecha = Utilitarios.EjecutarConsulta("Select datepart(Year,DocDate) from OINV with(nolock) where Docentry = '" & strIDFactura & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            strValorSeleccionado = Utilitarios.EjecutarConsulta("Select datepart(Month,DocDate) from OINV with(nolock) where Docentry = '" & strIDFactura & "'", m_oCompany.CompanyDB, m_oCompany.Server)


            If strValorSeleccionado.Length = 1 AndAlso Not String.IsNullOrEmpty(strValorSeleccionado) Then
                strFecha = strFecha & "0" & strValorSeleccionado
            Else
                strFecha = strFecha & strValorSeleccionado
            End If

            strValorSeleccionado = Utilitarios.EjecutarConsulta("Select datepart(Day,DocDate) from OINV with(nolock) where Docentry = '" & strIDFactura & "'", m_oCompany.CompanyDB, m_oCompany.Server)

            If strValorSeleccionado.Length = 1 AndAlso Not String.IsNullOrEmpty(strValorSeleccionado) Then
                strFecha = strFecha & "0" & strValorSeleccionado
            Else
                strFecha = strFecha & strValorSeleccionado

            End If

            If Not String.IsNullOrEmpty(strFecha) Then
                m_oFormGoodIssue.Items.Item("21").Enabled = True
                m_oFormGoodIssue.Items.Item("21").Specific.String = strFecha
                m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Fech_Con", 0, strFecha)
            Else
                strFecha = Date.Now.Year.ToString
                If Date.Now.Month.ToString.Length = 1 Then
                    strFecha = strFecha & "0" & Date.Now.Month.ToString
                Else
                    strFecha = strFecha & Date.Now.Month.ToString
                End If
                If Date.Now.Day.ToString.Length = 1 Then
                    strFecha = strFecha & "0" & Date.Now.Day.ToString
                Else
                    strFecha = strFecha & Date.Now.Day.ToString
                End If
                m_oFormGoodIssue.Items.Item("21").Enabled = True
                m_oFormGoodIssue.Items.Item("21").Specific.String = strFecha
                m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Fech_Con", 0, strFecha)

            End If
        Else
            strFecha = Date.Now.Year.ToString
            If Date.Now.Month.ToString.Length = 1 Then
                strFecha = strFecha & "0" & Date.Now.Month.ToString
            Else
                strFecha = strFecha & Date.Now.Month.ToString
            End If
            If Date.Now.Day.ToString.Length = 1 Then
                strFecha = strFecha & "0" & Date.Now.Day.ToString
            Else
                strFecha = strFecha & Date.Now.Day.ToString
            End If
            m_oFormGoodIssue.DataSources.DBDataSources.Item(mc_strGoodIssues).SetValue("U_Fech_Con", 0, strFecha)
        End If

    End Sub

    Public Function DevolverIDContrato(ByVal p_strIDForm As String) As String

        Dim oform As SAPbouiCOM.Form
        Dim strIDContrato As String

        oform = SBO_Application.Forms.Item(p_strIDForm)
        strIDContrato = oform.DataSources.DBDataSources.Item(mc_strGoodIssues).GetValue("U_NoCont", 0)

        Return strIDContrato

    End Function

    Public Function DevolverIDEntrada(ByVal p_strIDForm As String) As String

        Dim oform As SAPbouiCOM.Form
        Dim strIDEntrada As String

        oform = SBO_Application.Forms.Item(p_strIDForm)
        strIDEntrada = oform.DataSources.DBDataSources.Item(mc_strGoodIssues).GetValue("U_Doc_Entr", 0)
        strIDEntrada = strIDEntrada.Trim
        Return strIDEntrada

    End Function

    <CLSCompliant(False)> _
    Public Sub ImprimirReporteCostoVehiculo(ByVal FormUID As String, _
                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = String.Empty
        Dim strDBDMSOne As String = String.Empty
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim strBarraTitulo As String

        Dim strCodUnidad As String = String.Empty
        Dim StrDocNum As String = String.Empty

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        strCodUnidad = oForm.DataSources.DBDataSources.Item(mc_strGoodIssues).GetValue("U_Unidad", 0).TrimEnd()
        StrDocNum = oForm.DataSources.DBDataSources.Item(mc_strGoodIssues).GetValue("DocNum", 0).Trim

        strParametros = String.Format("{0},{1}", strCodUnidad, StrDocNum)

        strParametros = strParametros.Replace(" ", "°")

        'strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptBalanceNegocio & ".rpt"
        strDireccionReporte = m_objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptBalanceNegocio & ".rpt"

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strBarraTitulo = My.Resources.Resource.TituloGoodIssue.Replace(" ", "°")

        strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub


    Public Sub ReversarSalidaMercancia(ByVal FormUID As String, _
                               ByRef pVal As SAPbouiCOM.ItemEvent, _
                               ByRef BubbleEvent As Boolean)
        Try
            Dim oform As SAPbouiCOM.Form
            Dim strUnidad As String
            Dim intAsSalidaMercancia As Integer
            Dim strFechaReversion As String

            oform = SBO_Application.Forms.Item(FormUID)

            'strUnidad = oform.DataSources.DBDataSources.Item(mc_strGoodIssues).GetValue("U_Unidad", 0)
            'strUnidad = strUnidad.Trim()

            intAsSalidaMercancia = Convert.ToInt32(oform.DataSources.item(mc_strGoodIssues).getvalue("U_As_Sali", 0))
            ' strFechaReversion = oform.DataSources.item(mc_strGoodIssues).getvalue("U_FechaRev ", 0)

            m_ReversaSalidaMercancia.ReversarAsientoSalidaMercancia(intAsSalidaMercancia, strFechaReversion)

        Catch ex As Exception

        End Try


    End Sub




    Public Sub CrearAsientoSalidaVehiculo(ByVal p_strNoSalida As String, ByVal p_strNoUnidad As String, Optional ByVal p_dateFechaContrato As Date = Nothing, _
                                        Optional ByVal p_blnDimension As Boolean = False, _
                                        Optional ByVal p_ListaConfiguracion As Hashtable = Nothing, _
                                        Optional ByRef p_UsaCompTran As Boolean = True, _
                                        Optional ByVal p_DtLines As Data.DataTable = Nothing)

        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try
            Dim oDataTable As SAPbouiCOM.DataTable
            Dim intDocEntry As Integer = 0
            Dim strDocNum As String = String.Empty
            Dim strNoUnidad As String = String.Empty
            Dim strContraCuenta As String = String.Empty
            Dim strDocEntrada As String = String.Empty
            Dim intNoAsiento As Integer = 0

            Dim oCompanyServiceEntrada As SAPbobsCOM.CompanyService
            Dim oGeneralServiceEntrada As SAPbobsCOM.GeneralService
            Dim oGeneralDataEntrada As SAPbobsCOM.GeneralData
            Dim oGeneralParamsEntrada As SAPbobsCOM.GeneralDataParams

            Dim oCompanyServiceSalida As SAPbobsCOM.CompanyService
            Dim oGeneralServiceSalida As SAPbobsCOM.GeneralService
            Dim oGeneralDataSalida As SAPbobsCOM.GeneralData
            Dim oGeneralParamsSalida As SAPbobsCOM.GeneralDataParams
            Dim strConectionString As String = String.Empty
            Dim strConsulta As String
            Dim cmdComando As New SqlClient.SqlCommand

            Dim dtGILines As System.Data.DataTable
            Dim rowGILines As System.Data.DataRow


            'Cambio Costeo Local
            Dim blnCosteoLocal As String = String.Empty
            blnCosteoLocal = Utilitarios.EjecutarConsulta("Select U_CosteoLocal from dbo.[@SCGD_ADMIN] with (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
            If String.IsNullOrEmpty(blnCosteoLocal) Then
                blnCosteoLocal = "N"
            End If

            If Not p_DtLines Is Nothing Then
                dtGILines = p_DtLines
            Else
                dtGILines = Utilitarios.EjecutarConsultaDataTable(String.Format("select DocEntry, U_SCGD_DocEntrada, U_SCGD_AsEntrada from dbo.[@SCGD_GILINES] with (nolock) where DocEntry= '{0}'",
                                                                 p_strNoSalida),
                                                              m_oCompany.CompanyDB,
                                                              m_oCompany.Server)
            End If


            'Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            'cn_Coneccion.ConnectionString = strConectionString
            'cn_Coneccion.Open()

            'cmdComando.Connection = cn_Coneccion
            'cmdComando.CommandType = CommandType.Text

            'For Each rowGILines In dtGILines.Rows
            '    If rowGILines.Item("U_SCGD_AsEntrada") <> "-1" Then
            '        cmdComando.CommandText = "Update [@SCGD_GOODRECEIVE] set Status = 'C' where DocEntry =  '" & rowGILines.Item("U_SCGD_DocEntrada") & "'"
            '        cmdComando.ExecuteNonQuery()
            '    End If
            'Next

            'Inicio Transaccion SBO
            If p_UsaCompTran Then
                m_oCompany.StartTransaction()
            End If

            ' Actualiza Entradas de Mercancias
            For Each rowGILines In dtGILines.Rows
                strDocEntrada = rowGILines.Item("U_SCGD_DocEntrada").ToString.Trim()
                If Not String.IsNullOrEmpty(strDocEntrada) Then
                    oCompanyServiceEntrada = m_oCompany.GetCompanyService()
                    oGeneralServiceEntrada = oCompanyServiceEntrada.GetGeneralService("SCGD_GOODENT")
                    oGeneralParamsEntrada = oGeneralServiceEntrada.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParamsEntrada.SetProperty("DocEntry", strDocEntrada)
                    oGeneralDataEntrada = oGeneralServiceEntrada.GetByParams(oGeneralParamsEntrada)
                    oGeneralDataEntrada.SetProperty("U_SCGD_DocSalida", p_strNoSalida)
                    oGeneralServiceEntrada.Update(oGeneralDataEntrada)
                    oGeneralServiceEntrada.Close(oGeneralParamsEntrada)
                End If
            Next

            'Crea Asiento de Salida de Vehiculo
            intNoAsiento = CrearAsientoSalida(dtGILines, p_strNoSalida, p_strNoUnidad, strContraCuenta, blnCosteoLocal, p_dateFechaContrato, p_blnDimension)

            'Actualiza Salida de mercancia
            If intNoAsiento > 0 Then
                oCompanyServiceSalida = m_oCompany.GetCompanyService()
                oGeneralServiceSalida = oCompanyServiceSalida.GetGeneralService("SCGD_GOODISSUE")
                oGeneralParamsSalida = oGeneralServiceSalida.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsSalida.SetProperty("DocEntry", p_strNoSalida)
                oGeneralDataSalida = oGeneralServiceSalida.GetByParams(oGeneralParamsSalida)
                oGeneralDataSalida.SetProperty("U_As_Sali", intNoAsiento.ToString.Trim())
                If m_blnAsignarCuentaCuentaSalidaAutomatica = True Then
                    oGeneralDataSalida.SetProperty("U_NCuenCnt", strContraCuenta)
                End If
                oGeneralServiceSalida.Update(oGeneralDataSalida)
            End If
            If p_UsaCompTran Then
                If intNoAsiento > 0 Then
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                Else
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                End If
                'If cn_Coneccion.State = ConnectionState.Open Then
                '    cn_Coneccion.Close()
                'End If
            End If

        Catch ex As Exception
            If p_UsaCompTran Then
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            End If
            'If cn_Coneccion.State = ConnectionState.Open Then
            '    cn_Coneccion.Close()
            'End If
        End Try
    End Sub
    Public Function ObternerFechaServer() As String
        Try
            Dim l_fhaActual As String

            l_fhaActual = Utilitarios.EjecutarConsulta("select GETDATE()", m_oCompany.CompanyDB, m_oCompany.Server)

            Return l_fhaActual
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    Public Function CrearAsientoSalida(ByVal p_dtGILines As System.Data.DataTable, ByVal p_strDocEntry As String, ByVal p_strNoUnidad As String, ByRef p_strContraCuenta As String, ByVal p_blnCosteoLocal As String, Optional ByVal p_dateFechaContrato As Date = Nothing,
                                       Optional ByVal p_blnUsaDimension As Boolean = False, Optional ByVal p_listaConfiguracion As Hashtable = Nothing, _
                                       Optional ByVal p_GetCuenta As Boolean = True) As Integer
        Try

            Dim oJE_Lines As SAPbobsCOM.JournalEntries_Lines
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim oListaLineasAsiento As New List(Of ListaLineaAsiento)()
            Dim oListaAsiento As New List(Of ListaLineaAsiento)()

            Dim rowGILines As System.Data.DataRow
            Dim strAsiento As String = String.Empty

            Dim strAsientoGenerado As String = "0"
            Dim strFCCurrencyTemp As String = String.Empty
            Dim strMonedaLocal As String = String.Empty

            Dim strCuenta As String = String.Empty
            Dim strContraCuenta As String = String.Empty
            Dim strCuentaSeleccionada As String = String.Empty
            Dim strTipoVehiculo As String = String.Empty

            Dim dateFechaConta As Date = Nothing
            Dim strFechaFormateada As String
            Dim strFechaConta As String

            Dim intError As Integer
            Dim strMensajeError As String = String.Empty
            Dim formato As String
            Dim dateFechaRegistro As Date = Nothing
            Dim blnUsaSalidaLocal As Boolean = False
            Dim strUsaSalidaLocal As String = String.Empty

            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls
            Dim blnAgregarDimension As Boolean = False
            Dim blnUsaDimensiones As Boolean = False

            'Usa Salida Local
            strUsaSalidaLocal = Utilitarios.EjecutarConsulta("Select U_SCGD_CSLoc from [@SCGD_ADMIN] with (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
            If Not String.IsNullOrEmpty(strUsaSalidaLocal) Then
                If strUsaSalidaLocal = "Y" Then
                    blnUsaSalidaLocal = True
                Else
                    blnUsaSalidaLocal = False
                End If
            Else
                blnUsaSalidaLocal = False
            End If

            strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo_Ven from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & p_strNoUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            If String.IsNullOrEmpty(strTipoVehiculo) Then
                strTipoVehiculo = Utilitarios.EjecutarConsulta("Select U_Tipo from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & p_strNoUnidad & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            End If

            If p_dateFechaContrato = Nothing Then
                strFechaConta = Utilitarios.EjecutarConsulta(
                                String.Format("Select convert(date,U_Fech_Con) from [@SCGD_GOODISSUE] with (nolock) where DocEntry = '{0}' ", p_strDocEntry),
                                                                        m_oCompany.CompanyDB,
                                                                        m_oCompany.Server)
                strFechaFormateada = Utilitarios.EjecutarConsulta(
                                String.Format(" Select LEFT(CONVERT(VARCHAR, U_Fech_Con, 103),10) from [@SCGD_GOODISSUE] with (nolock) where DocEntry ='{0}' ", p_strDocEntry),
                                                                        m_oCompany.CompanyDB,
                                                                        m_oCompany.Server)

                If strFechaConta.Contains("-") Then
                    strFechaFormateada = strFechaFormateada.Replace("/", "-")
                End If

                formato = Utilitarios.ObtieneFormatoFecha(SBO_Application, m_oCompany)
                If formato.ToCharArray()(formato.Length - 1) = "y" AndAlso formato.ToCharArray()(formato.Length - 2) = "y" AndAlso formato.ToCharArray()(formato.Length - 3) <> "y" Then
                    formato = formato & "yy"
                End If


                If IsDate(strFechaConta) Then dateFechaRegistro = Date.ParseExact(strFechaFormateada, formato, CultureInfo.CurrentCulture)
            End If

            'Valida si a nivel general si se usan dimensiones, ya que en la parametrizacion de documentos de Venta puede que el documento se le haya 
            'marcado para no generar dimensiones

            Dim strUsaDimensiones As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] with (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

            If strUsaDimensiones = "Y" Then
                p_blnUsaDimension = True
            End If

            '******************************************************************************************
            'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
            If p_blnUsaDimension Then

                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '" & p_strNoUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)

                oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))

                '******************************************************************************************
                If oDataTableDimensiones.Rows.Count <> 0 Then

                    blnAgregarDimension = True

                End If
            End If

            If dateFechaRegistro <> Nothing Or p_dateFechaContrato <> Nothing Then
                'Carga de cuentas contables
                If Not m_objConfiguracionGeneral Is Nothing Then
                    strCuenta = m_objConfiguracionGeneral.CuentaStock(strTipoVehiculo)
                    strContraCuenta = m_objConfiguracionGeneral.CuentaCosto(strTipoVehiculo)
                Else
                    strCuenta = Utilitarios.EjecutarConsulta("Select U_Stock from [@SCGD_ADMIN4] with (nolock) where U_Tipo  = '" & strTipoVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                    strContraCuenta = Utilitarios.EjecutarConsulta("Select U_Costo from [@SCGD_ADMIN4] with (nolock) where U_Tipo = '" & strTipoVehiculo & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                End If

                If p_GetCuenta Then
                    If Not String.IsNullOrEmpty(p_strDocEntry) Then
                        strCuentaSeleccionada = Utilitarios.EjecutarConsulta("Select U_NCuenCnt from [@SCGD_GOODISSUE] with (nolock) where DocEntry = " & CInt(p_strDocEntry), m_oCompany.CompanyDB, m_oCompany.Server)
                    End If

                    'verifico si las cuentas son las mismas
                    If Not strContraCuenta = strCuentaSeleccionada And Not String.IsNullOrEmpty(strCuentaSeleccionada) Then
                        strContraCuenta = strCuentaSeleccionada
                    End If
                End If
                p_strContraCuenta = strContraCuenta
                strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM with (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

                If Not String.IsNullOrEmpty(strCuenta) Then
                    oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

                    For Each rowGILines In p_dtGILines.Rows
                        strAsiento = rowGILines.Item("U_SCGD_AsEntrada").ToString.Trim()
                        If Not String.IsNullOrEmpty(strAsiento) And strAsiento <> "-1" Then
                            oJournalEntry.GetByKey(strAsiento)
                            oJE_Lines = oJournalEntry.Lines
                            For i As Integer = 0 To oJE_Lines.Count - 1
                                oJE_Lines.SetCurrentLine(i)
                                If oJE_Lines.AccountCode = strCuenta Then
                                    strFCCurrencyTemp = oJE_Lines.FCCurrency.ToString.Trim()
                                    'Usa Costeo Local
                                    If blnUsaSalidaLocal Then
                                        oListaLineasAsiento.Add(New ListaLineaAsiento() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .Debit = Decimal.Parse(oJE_Lines.Debit), .Credit = Decimal.Parse(oJE_Lines.Credit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                    Else
                                        If p_blnCosteoLocal = "Y" Then
                                            oListaLineasAsiento.Add(New ListaLineaAsiento() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .Debit = Decimal.Parse(oJE_Lines.Debit), .Credit = Decimal.Parse(oJE_Lines.Credit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                        Else
                                            If String.IsNullOrEmpty(strFCCurrencyTemp) Then
                                                oListaLineasAsiento.Add(New ListaLineaAsiento() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .Debit = Decimal.Parse(oJE_Lines.Debit), .Credit = Decimal.Parse(oJE_Lines.Credit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                            ElseIf Not String.IsNullOrEmpty(strFCCurrencyTemp) Then
                                                oListaLineasAsiento.Add(New ListaLineaAsiento() With {.Account = oJE_Lines.AccountCode.ToString.Trim(), .FCCurrency = oJE_Lines.FCCurrency.ToString.Trim(), .FCDebit = Decimal.Parse(oJE_Lines.FCDebit), .FCCredit = Decimal.Parse(oJE_Lines.FCCredit), .ImpNeg = oJE_Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value.ToString.Trim()})
                                            End If
                                        End If
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

                For Each C1 As ListaLineaAsiento In oListaLineasAsiento

                    decMontoTemp = 0
                    blnAgregar = False
                    strMoneda = String.Empty
                    If Not String.IsNullOrEmpty(C1.FCCurrency) Then
                        strMoneda = C1.FCCurrency
                    Else
                        strMoneda = strMonedaLocal
                    End If

                    For Each C2 As ListaLineaAsiento In oListaLineasAsiento

                        If Not String.IsNullOrEmpty(C1.FCCurrency) And Not String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                            If C2.FCDebit <> 0 Then
                                If Not C2.ImpNeg = "Y" Then
                                    decMontoTemp += C2.FCDebit
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            ElseIf C2.FCCredit > 0 Then
                                If C2.ImpNeg = "Y" Then
                                    decMontoTemp += (C2.FCCredit * -1)
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            End If
                        ElseIf String.IsNullOrEmpty(C1.FCCurrency) And String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                            If C2.Debit <> 0 Then
                                If Not C2.ImpNeg = "Y" Then
                                    decMontoTemp += C2.Debit
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            ElseIf C2.Credit > 0 Then
                                If C2.ImpNeg = "Y" Then
                                    decMontoTemp += (C2.Credit * -1)
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            End If
                        End If
                    Next
                    If blnAgregar And decMontoTemp > 0 Then
                        If strMonedaLocal = strMoneda Then
                            oListaAsiento.Add(New ListaLineaAsiento() With {.FCCurrency = strMonedaLocal, .Debit = decMontoTemp, .Credit = decMontoTemp, .Aplicado = True})
                        Else
                            oListaAsiento.Add(New ListaLineaAsiento() With {.FCCurrency = strMoneda, .FCDebit = decMontoTemp, .FCCredit = decMontoTemp, .Aplicado = True})
                        End If
                    End If
                Next

                If oListaAsiento.Count() > 0 Then

                    strAsientoGenerado = "0"

                    oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoSalida & " " & p_strNoUnidad
                    oJournalEntry.Reference = p_strNoUnidad
                    oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"

                    If p_dateFechaContrato <> Nothing Then
                        oJournalEntry.ReferenceDate = p_dateFechaContrato
                    ElseIf dateFechaRegistro <> Nothing Then
                        oJournalEntry.ReferenceDate = dateFechaRegistro
                    End If


                    For Each row As ListaLineaAsiento In oListaAsiento

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

                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If
                        oJournalEntry.Lines.Add()
                        '*****************
                        'Cuenta Debito
                        '*****************
                        oJournalEntry.Lines.AccountCode = strContraCuenta
                        oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                        oJournalEntry.Lines.Reference1 = p_strNoUnidad

                        If strMonedaLocal = row.FCCurrency Then
                            oJournalEntry.Lines.Debit = row.Debit
                            oJournalEntry.Lines.FCDebit = 0
                        Else
                            oJournalEntry.Lines.FCDebit = row.FCDebit
                            oJournalEntry.Lines.FCCurrency = row.FCCurrency

                        End If

                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If

                        oJournalEntry.Lines.Add()

                    Next


                    If oJournalEntry.Add <> 0 Then
                        strAsientoGenerado = "0"
                        m_oCompany.GetLastError(intError, strMensajeError)
                        Utilitarios.DestruirObjeto(oJournalEntry)

                        SBO_Application.StatusBar.SetText(strMensajeError, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                        Throw New ExceptionsSBO(intError, strMensajeError)
                    Else
                        m_oCompany.GetNewObjectCode(strAsientoGenerado)
                    End If

                End If

            End If

            Utilitarios.DestruirObjeto(oJournalEntry)
            Return CInt(strAsientoGenerado)


        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message.ToString, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try


    End Function

    Private Sub CargarSalida(ByVal p_strItem As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Dim strIdVehiculo As String
        If m_oFormGoodIssue IsNot Nothing Then

            strIdVehiculo = p_strItem

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strItem

            Call m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GOODISSUE").Query(oConditions)
            Call m_oFormGoodIssue.DataSources.DBDataSources.Item("@SCGD_GILINES").Query(oConditions)
            m_oFormGoodIssue.Items.Item("mtx_0").Specific.LoadFromDataSource()
            m_oFormGoodIssue.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

        End If

    End Sub

#End Region

End Class

' Clase para la definición de la lista
Public Class ListaLineaAsiento

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
