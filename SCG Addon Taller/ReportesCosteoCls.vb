Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Globalization
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM

Public Class ReportesCosteoCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Public Const mc_strFormID As String = "SCGD_Rep_Cost"
    Public Const mc_strbtnTransit As String = "btnTransit"
    Public Const mc_strbtnImp_Tra As String = "btnImp_Tra"
    Public Const mc_strbtnInventa As String = "btnInventa"
    Public Const mc_strbtnRecoste As String = "btnRecoste"
    Public Const mc_strbtnFinaliz As String = "btnFinaliz"
    Public Const mc_strcboTipos As String = "cboTipos"
    Public Const mc_strtxtInicio As String = "txtInicio"
    Public Const mc_strtxtFin As String = "txtFin"
    Public Const mc_strtxtUnidad As String = "txtUnidad"
    Public Const mc_strdatTransit As String = "datTransit"
    Public Const mc_strdatInventa As String = "datInventa"
    Public Const mc_strbtnCerrar As String = "btnCerrar"
    Public Const mc_strtxtDesdeT As String = "txtDesdeT"
    Public Const mc_strUIDVehiculos As String = "SCGD_MNO"
    Public Const mc_strUIDReportes As String = "SCGD_RPC"
    Private m_strTransaccionAsientos As String
    Private WithEvents SBO_Application As Application
    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private objConfiguracionGeneral As ConfiguracionesGeneralesAddon

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu(mc_strUIDReportes, SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu(mc_strUIDReportes, SBO_Application.Language)
            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDReportes, BoMenuType.mt_STRING, strEtiquetaMenu, 20, False, True, mc_strUIDVehiculos))
        End If
    End Sub

    Protected Friend Sub CargaFormulario()

        Try

            Dim fcp As FormCreationParams
            Dim strXMLACargar As String
            Dim minDate As Date = New Date(1900, 1, 1)

            fcp = SBO_Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            fcp.FormType = mc_strFormID

            strXMLACargar = My.Resources.Resource.ReportesCosteoVehiculos
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select Code,Name From [@SCGD_TIPOVEHICULO] Order by Name", mc_strcboTipos, True)
            m_oFormGenCotizacion.PaneLevel = 1

            Dim dataTable As DataTable = m_oFormGenCotizacion.DataSources.DataTables.Add("AAA")
            dataTable.Columns.Add(UID:="Resumido", ColFieldType:=BoFieldsType.ft_Integer)
            dataTable.Columns.Add(UID:="FDesdeT", ColFieldType:=BoFieldsType.ft_Date)
            dataTable.Columns.Add(UID:="FHastaT", ColFieldType:=BoFieldsType.ft_Date)
            dataTable.Columns.Add(UID:="FHastaI", ColFieldType:=BoFieldsType.ft_Date)
            dataTable.Columns.Add(UID:="FDesdeN", ColFieldType:=BoFieldsType.ft_Date)
            dataTable.Columns.Add(UID:="FHastaN", ColFieldType:=BoFieldsType.ft_Date)
            dataTable.Rows.Add(1)
            dataTable.SetValue(Column:="Resumido", rowIndex:=0, Value:=0)
            dataTable.SetValue(Column:="FDesdeT", rowIndex:=0, Value:=minDate.ToString("yyyyMMdd"))
            dataTable.SetValue(Column:="FHastaT", rowIndex:=0, Value:=Date.Now.ToString("yyyyMMdd"))
            dataTable.SetValue(Column:="FHastaI", rowIndex:=0, Value:=Date.Now.ToString("yyyyMMdd"))
            dataTable.SetValue(Column:="FDesdeN", rowIndex:=0, Value:=minDate.ToString("yyyyMMdd"))
            dataTable.SetValue(Column:="FHastaN", rowIndex:=0, Value:=Date.Now.ToString("yyyyMMdd"))

            Dim item As Item
            Dim checkBox As CheckBox
            Dim txtBox As EditText

            item = m_oFormGenCotizacion.Items.Item("chkRes")
            checkBox = DirectCast(item.Specific, CheckBox)
            checkBox.ValOff = "0"
            checkBox.ValOn = "1"
            checkBox.DataBind.Bind(UID:="AAA", columnUid:="Resumido")

            item = m_oFormGenCotizacion.Items.Item(mc_strtxtDesdeT)
            txtBox = DirectCast(item.Specific, EditText)
            txtBox.DataBind.Bind(UID:="AAA", columnUid:="FDesdeT")

            item = m_oFormGenCotizacion.Items.Item(mc_strdatTransit)
            txtBox = DirectCast(item.Specific, EditText)
            txtBox.DataBind.Bind(UID:="AAA", columnUid:="FHastaT")

            item = m_oFormGenCotizacion.Items.Item(mc_strdatInventa)
            txtBox = DirectCast(item.Specific, EditText)
            txtBox.DataBind.Bind(UID:="AAA", columnUid:="FHastaI")

            item = m_oFormGenCotizacion.Items.Item(mc_strtxtInicio)
            txtBox = DirectCast(item.Specific, EditText)
            txtBox.DataBind.Bind(UID:="AAA", columnUid:="FDesdeN")

            item = m_oFormGenCotizacion.Items.Item(mc_strtxtFin)
            txtBox = DirectCast(item.Specific, EditText)
            txtBox.DataBind.Bind(UID:="AAA", columnUid:="FHastaN")


            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)
            'm_strTransaccionAsientos = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "OTROS_AC", m_strDireccionConfiguracion)
            m_strTransaccionAsientos = objConfiguracionGeneral.TransaccionAsientoAjuste

            If m_oFormGenCotizacion IsNot Nothing Then

                CargarValidValuesEnCombos(m_oFormGenCotizacion, _
                                           "select AcctCode, isnull(FormatCode, AcctCode) from ( SELECT  MAX([OACT].[AcctCode]) as AcctCode, MAX([OACT].[FormatCode] + '-' + OACT.[Segment_1] + '-' + oact.[Segment_2]) as FormatCode FROM    [@SCGD_ADMIN4] INNER	JOIN OACT ON [@SCGD_ADMIN4].[U_Transito] = [OACT].[AcctCode] GROUP BY [AcctCode],[FormatCode],[Segment_1], [Segment_2]) T", _
                                           "cbAcct", False)
            End If



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
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

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)
            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case mc_strbtnTransit
                        Call ImprimirReporteInventarioTransito(FormUID, pVal, BubbleEvent)
                    Case mc_strbtnImp_Tra
                        Call ImprimirReporteInventarioTransitoEntreFechas(FormUID, pVal, BubbleEvent)
                    Case mc_strbtnRecoste
                        Call ImprimirReporteRecosteos(FormUID, pVal, BubbleEvent)
                    Case mc_strbtnCerrar
                        SBO_Application.Forms.Item(FormUID).Close()
                    Case mc_strbtnFinaliz
                        Call ImprimirReporteNegociosFinalizados(FormUID, pVal, BubbleEvent)
                    Case mc_strbtnInventa
                        Call ImprimirReporteInventarioxTipo(FormUID, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                      ByVal strQuery As String, _
                                                      ByRef strIDItem As String, ByVal agregarNinguno As Boolean)
        '*******************************************************************    
        'Propósito: Se encarga de cargar los values y descriptions de los 
        '           combos que utilizan catalogos en UserTables.
        'Acepta:    oForm As SAPbouiCOM.Form,
        '           ByVal strQuery As String,
        '           ByRef strIDItem As String
        '
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 21 Nov 2006
        '********************************************************************

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            'Agrega los ValidValues
            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, drdResultadoConsulta.GetString(1).Trim)
                End If
            Loop
            If (agregarNinguno) Then
                cboCombo.ValidValues.Add("--", My.Resources.Resource.Ninguno)
                cboCombo.Select("--")
            ElseIf cboCombo.ValidValues.Count <> 0 Then
                cboCombo.Select(Index:=0, SearchKey:=BoSearchKey.psk_Index)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteInventarioTransito(ByVal FormUID As String, _
                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                ByRef BubbleEvent As Boolean)

        ReporteInventarioEnTransito(FormUID, False)

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteInventarioTransitoEntreFechas(ByVal FormUID As String, _
                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                            ByRef BubbleEvent As Boolean)

        ReporteInventarioEnTransito(FormUID, True)
    End Sub

    Private Sub ReporteInventarioEnTransito(ByVal FormUID As String, ByVal usaFecha As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim strFechaFin As String
        Dim strFechaInicio As String
        Dim fechaInicio As Date
        Dim fechaFin As Date
        Dim acctCode As String
        Dim blsbo As New BLSBO.GlobalFunctionsSBO
        Dim monedaLocal As String = String.Empty
        Dim monedaSistema As String = String.Empty
        Dim tipoCambio As Double
        Dim item As Item
        Dim cbCuenta As ComboBox
        Dim chkRsumido As CheckBox
        Dim unidad As String = String.Empty

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        strFechaFin = DirectCast(oForm.Items.Item(mc_strdatTransit).Specific, EditText).Value
        strFechaInicio = DirectCast(oForm.Items.Item(mc_strtxtDesdeT).Specific, EditText).Value
        unidad = oForm.Items.Item(mc_strtxtUnidad).Specific.String
        If String.IsNullOrEmpty(unidad) Then unidad = "-1"
        item = oForm.Items.Item("cbAcct")
        cbCuenta = DirectCast(item.Specific, ComboBox)
        item = oForm.Items.Item("chkRes")
        chkRsumido = DirectCast(item.Specific, CheckBox)
        acctCode = cbCuenta.Selected.Value.ToString()
        If (Not usaFecha) OrElse (usaFecha AndAlso Not String.IsNullOrEmpty(strFechaFin) AndAlso Not String.IsNullOrEmpty(strFechaInicio)) Then
            'parametros reporte:
            'fechaCorte, simboloML, simboloME, rate, resumido, compania, usuario
            '
            '

            blsbo.Set_Compania(m_oCompany)
            blsbo.MonedasSistema(monedaLocal, monedaSistema)
            monedaLocal = monedaLocal.Trim()
            monedaSistema = monedaSistema.Trim()

            If usaFecha Then
                fechaFin = Date.ParseExact(strFechaFin, "yyyyMMdd", Nothing)
                fechaFin = New Date(fechaFin.Year, fechaFin.Month, fechaFin.Day, 23, 59, 59)
                fechaInicio = Date.ParseExact(strFechaInicio, "yyyyMMdd", Nothing)
                fechaInicio = New Date(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, 0, 0, 0)
            Else
                fechaFin = Date.Now
                fechaInicio = New Date(1900, 1, 1)
            End If
            Dim pe As String
            Dim pd As String = "0"
            '            Dim deci As Double
            Dim n As NumberFormatInfo = New NumberFormatInfo()
            n.NumberDecimalSeparator = "°"
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)

            If monedaLocal <> monedaSistema Then
                tipoCambio = blsbo.RetornarTipoCambioMoneda(monedaSistema, fechaFin, m_strConectionString, False)
            Else
                tipoCambio = 1
            End If

            Dim strings As String() = tipoCambio.ToString(n).Split("°")

            pe = strings(0)
            If strings.Length > 1 Then
                pd = strings(1)
            End If
            strParametros = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", monedaLocal, monedaSistema, chkRsumido.Checked, m_oCompany.CompanyName, m_oCompany.UserName, pe, pd, acctCode, fechaFin, fechaInicio, unidad)

            strParametros = strParametros.Replace(" ", "°")

            'strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptVehiculosTransitoporFecha & ".rpt"
            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptVehiculosTransitoporFecha & ".rpt"

            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= My.Resources.Resource.TituloVehiculosTransito.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)
        Else
            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeIngresarFecha, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteInventarioxTipo(ByVal FormUID As String, _
                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                            ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strFecha As String
        Dim datFecha As Date = Nothing

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        oCombo = DirectCast(oForm.Items.Item(mc_strcboTipos).Specific, SAPbouiCOM.ComboBox)
        strParametros = oCombo.Selected.Value
        strFecha = oForm.Items.Item(mc_strdatInventa).Specific.Value
        If strParametros <> "--" Then 'AndAlso IsDate(strFecha) Then


            datFecha = Date.ParseExact(strFecha, "yyyyMMdd", Nothing)
            datFecha = New Date(datFecha.Year, datFecha.Month, datFecha.Day, 0, 0, 0)

            'datFecha = CDate(strFecha)
            'strFecha = CStr(fechaFin.Year) + CStr(fechaFin.Month) + CStr(fechaFin.Day) ' + " 23:59:59"
            strParametros = String.Format("{0},{1},{2},{3}", strParametros, m_oCompany.CompanyName, m_oCompany.UserName, datFecha)
            strParametros = strParametros.Replace(" ", "°")

            'strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptInventarioContable & ".rpt"
            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptInventarioContable & ".rpt"

            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= My.Resources.Resource.TituloInventarioContable.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)
        Else
            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeSeleccionarTipo, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteRecosteos(ByVal FormUID As String, _
                        ByRef pVal As SAPbouiCOM.ItemEvent, _
                        ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        strParametros = m_strTransaccionAsientos & "," & "," & m_oCompany.CompanyName & "," & m_oCompany.UserName
        ' strParametros = "_"
        strParametros = strParametros.Replace(" ", "°")

        'strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptRecosteos & ".rpt"
        strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptRecosteos & ".rpt"

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.TituloRecosteos.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteNegociosFinalizados(ByVal FormUID As String, _
                    ByRef pVal As SAPbouiCOM.ItemEvent, _
                    ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim strFechaInicio As String
        Dim datFechaInicio As Date
        Dim strFechaFin As String
        Dim datFechaFin As Date

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        'strFechaInicio = oForm.Items.Item(mc_strtxtInicio).Specific.Value
        'strFechaFin = oForm.Items.Item(mc_strtxtFin).Specific.Value

        strFechaFin = oForm.Items.Item(mc_strtxtFin).Specific.Value
        strFechaInicio = oForm.Items.Item(mc_strtxtInicio).Specific.Value

        'strFechaFin = DirectCast(oForm.Items.Item(mc_strtxtFin).Specific, EditText).Value
        'strFechaInicio = DirectCast(oForm.Items.Item(mc_strtxtInicio).Specific, EditText).Value
        'If IsDate(strFechaInicio) AndAlso IsDate(strFechaFin) Then

        datFechaFin = Date.ParseExact(strFechaFin, "yyyyMMdd", Nothing)
        datFechaFin = New Date(datFechaFin.Year, datFechaFin.Month, datFechaFin.Day, 23, 59, 59)
        datFechaInicio = Date.ParseExact(strFechaInicio, "yyyyMMdd", Nothing)
        datFechaInicio = New Date(datFechaInicio.Year, datFechaInicio.Month, datFechaInicio.Day, 0, 0, 0)

        'datFechaFin = CDate(strFechaFin)
        'datFechaInicio = CDate(strFechaInicio)
        strParametros = datFechaInicio & "," & datFechaFin
        strParametros = strParametros.Replace(" ", "°")

        ' strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptBalanceNegociosEntreFechas & ".rpt"
        strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptBalanceNegociosEntreFechas & ".rpt"

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.TituloBalanceNegociosEntreFechas.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)
        'Else
        'SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeIngresarFecha, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        'End If
    End Sub

#End Region


End Class

