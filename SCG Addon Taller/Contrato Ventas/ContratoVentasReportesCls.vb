Option Explicit On

Imports System.Globalization
Imports System.IO
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework


Public Class ContratoVentasReportesCls

#Region "Declariones"
    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As SAPbouiCOM.Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    'objeto form 
    Private oForm As SAPbouiCOM.Form

    Private m_strDireccionConfiguracion As String
    Public n As NumberFormatInfo

    'objeto datatable 
    Private _dt As DataTable

    Private _strParametros As String 

    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Public EditTextCdV As EditTextSBO

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As SAPbouiCOM.Application)

        'declaracion de objetos acplication , company y decimaels 
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "Properties"

    <System.CLSCompliant(False)> _
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property

    Public Property dt As DataTable
        Get
            Return _dt
        End Get
        Set(ByVal value As DataTable)
            _dt = value
        End Set
    End Property

    Public Property StrParametros As String
        Get
            Return _strParametros
        End Get
        Set(ByVal value As String)
            _strParametros = value
        End Set
    End Property

#End Region

#Region "Métodos"

    'Metodo para cargar la pantalla de reportes de contratos de Venta
    Public Sub CargarFormularioReportes()
        'variables a utilizar
        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String

        Try
            'parametros para el form que se abrirá
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_REP_CV"

            'se designa el XML que se cargara
            strXMLACargar = My.Resources.Resource.ReportesContratoVenta
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = m_SBO_Application.Forms.AddEx(fcp)

            'Para linkear edittext de interfaz se utilizan datatables
            Dim datatable As SAPbouiCOM.DataTable = oForm.DataSources.DataTables.Add("REPORTE")
            datatable.Columns.Add(UID:="CdV", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
            'datatable.Columns.Add(UID:="vend", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
            datatable.Columns.Add(UID:="fechaDesde", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_Date)
            datatable.Columns.Add(UID:="fechaHasta", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_Date)
            datatable.Columns.Add(UID:="proyectado", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
            datatable.Rows.Add(1)
            datatable.SetValue(Column:="CdV", rowIndex:=0, Value:="")
            'datatable.SetValue(Column:="vend", rowIndex:=0, Value:="")
            datatable.SetValue(Column:="fechaDesde", rowIndex:=0, Value:=Date.Now.ToString("yyyMMdd"))
            datatable.SetValue(Column:="fechaHasta", rowIndex:=0, Value:=Date.Now.ToString("yyyMMdd"))
            datatable.SetValue(Column:="proyectado", rowIndex:=0, Value:="N")

            Dim item As Item
            Dim chk As CheckBox
            Dim txt As EditText

            item = oForm.Items.Item("txtCdV")
            txt = DirectCast(item.Specific, EditText)
            txt.DataBind.Bind(UID:="REPORTE", columnUid:="CdV")

            'item = oForm.Items.Item("txtVend")
            'txt = DirectCast(item.Specific, EditText)
            'txt.DataBind.Bind(UID:="REPORTE", columnUid:="vend")

            item = oForm.Items.Item("txtDesde")
            txt = DirectCast(item.Specific, EditText)
            txt.DataBind.Bind(UID:="REPORTE", columnUid:="fechaDesde")

            item = oForm.Items.Item("txtHasta")
            txt = DirectCast(item.Specific, EditText)
            txt.DataBind.Bind(UID:="REPORTE", columnUid:="fechaHasta")

            item = oForm.Items.Item("ckCosPro")
            chk = DirectCast(item.Specific, CheckBox)
            chk.ValOff = "N"
            chk.ValOn = "Y"
            chk.DataBind.Bind(UID:="REPORTE", columnUid:="proyectado")

            dt = datatable
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'CARGA EL XML DE LA PANTALLA 
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

    'Metodo para agregar items al menu
    Protected Friend Sub AddMenuItems()
        Dim strEtiquetaMenu As String = ""
        'Opciones de menus para Reportes de Contratos de Venta
        'Carpeta Reportes
        If Utilitarios.MostrarMenu("SCGD_RCV", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_RCV", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_RCV", SAPbouiCOM.BoMenuType.mt_POPUP, strEtiquetaMenu, 100, False, True, "SCGD_CTT"))

        End If
        'Generar reportes Contrato Ventas
        If Utilitarios.MostrarMenu("SCGD_GRC", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_GRC", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_GRC", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 1, False, True, "SCGD_RCV"))

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

            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")
            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strParametrosEjecutar = strBarraTitulo + " " + strDireccionReporte + " " + m_oCompany.DbUserName + "," + CatchingEvents.DBPassword + "," +
            m_oCompany.Server(+"," + m_oCompany.CompanyDB + " " + strParametros)

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)
            Limpiar()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'limpiar el datatable
    Public Sub Limpiar()

        'limpiar el datatable
        dt.SetValue("CdV", 0, "")

    End Sub

#End Region

#Region "Eventos"

    Public Sub ManejadorEventoClick(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                    ByRef BubbleEvent As Boolean,
                                    ByVal comp As SAPbobsCOM.Company,
                                    ByVal strUserName As String,
                                    ByVal strPass As String)

        Try
            'obtengo el form del que sucedio el evento
            oForm = m_SBO_Application.Forms.Item(FormUID)
            m_oCompany = comp

            'ACTION SUCCESS
            If pval.ItemUID = "btnImp1" _
                And pval.ActionSuccess = True _
                And pval.BeforeAction = False Then

                Dim strVendedor As String = ""
                Dim strFechaDesde As String = ""
                Dim strFechaHasta As String = ""
                Dim strProyectado As String = ""
                Me.StrParametros = ""

                'obtengo los parametros del datatable
                strVendedor = dt.GetValue("CdV", 0)
                strFechaDesde = dt.GetValue("fechaDesde", 0)
                strFechaHasta = dt.GetValue("fechaHasta", 0)
                strProyectado = dt.GetValue("proyectado", 0)

                'verifico que los parametros sean correctos
                If Not String.IsNullOrEmpty(strVendedor) _
                    And Not String.IsNullOrEmpty(strFechaDesde) _
                    And Not String.IsNullOrEmpty(strFechaHasta) _
                    And Not String.IsNullOrEmpty(strProyectado) _
                    And pval.BeforeAction = False _
                    And pval.ActionSuccess = True Then

                    'Obtengo las fechas ingresadas
                    Dim strFechaDesdeFormateada As String = ""
                    Dim strFechaHastaFormateada As String = ""




                    strFechaDesdeFormateada = Utilitarios.RetornaFechaFormatoRegional(strFechaDesde)
                    strFechaHastaFormateada = Utilitarios.RetornaFechaFormatoRegional(strFechaHasta)

                    If Not String.IsNullOrEmpty(strFechaDesdeFormateada.ToString()) _
                        And Not String.IsNullOrEmpty(strFechaHastaFormateada.ToString()) Then

                        'Verifico las fechas, hasta mayor que la desde
                        If CDate(strFechaDesdeFormateada) <= CDate(strFechaHastaFormateada) Then
                            If strProyectado = "F" _
                            Or strProyectado = "N" _
                            Or strProyectado = "Y" Then
                                StrParametros = strProyectado & "," & strVendedor & "," & strFechaDesdeFormateada.ToString() & "," & strFechaHastaFormateada.ToString()
                            End If
                        End If
                    End If

                End If

                'parametros
                If Not String.IsNullOrEmpty(StrParametros) Then
                    Call ImprimirReporte(My.Resources.Resource.rptComisionVehiculo, My.Resources.Resource.TituloReportesContratoVenta, StrParametros)
                Else
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorReporteCV, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

                End If
            End If

        Catch ex As Exception
            'manejo de errores
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

#End Region

    'No implementado
#Region "Choose from list"
    'no implementado
    '<System.CLSCompliant(False)> _
    'Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
    '                                         ByVal FormUID As String, _
    '                                         ByRef BubbleEvent As Boolean)


    '    Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
    '    oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
    '    Dim sCFL_ID As String
    '    sCFL_ID = oCFLEvento.ChooseFromListUID
    '    Dim oForm As SAPbouiCOM.Form
    '    oForm = m_SBO_Application.Forms.Item(FormUID)
    '    Dim oCFL As SAPbouiCOM.ChooseFromList
    '    oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

    '    Dim oDataTable As SAPbouiCOM.DataTable

    '    If oCFLEvento.BeforeAction = False Then
    '        oDataTable = oCFLEvento.SelectedObjects
    '        If pval.ItemUID = "txtCdV" Then

    '            Dim val As New ContratoVentasCls.CVentaUDT

    '            If Not oCFLEvento.SelectedObjects Is Nothing Then
    '                Call AsignaValoresEditTextUIVendedor(oDataTable.GetValue("SlpCode", 0), oDataTable.GetValue("SlpName", 0), oForm)
    '            End If
    '        End If
    '    End If

    'End Sub

    '<System.CLSCompliant(False)> _
    'Public Sub AsignaValoresEditTextUIVendedor(ByVal p_strCodeVendedor As String, _
    '                                           ByVal p_strNameVendedor As String, _
    '                                   ByRef oForm As SAPbouiCOM.Form)
    '    Try

    '        'Dim txt As EditText

    '        EditTextCdV.AsignaValorUserDataSource(p_strCodeVendedor)

    '        dt.SetValue("CFLV", 0, p_strCodeVendedor)
    '        dt.SetValue("CdV", 0, p_strCodeVendedor)
    '        dt.SetValue("vend", 0, p_strNameVendedor)

    '        'item = oForm.Items.Item("txtCFLV")
    '        'txt = DirectCast(item.Specific, EditText)

    '        'Dim item As Item = oForm.Items.Item("txtCFLV")
    '        'Dim edittext As EditText = CType(item.Specific, EditText)

    '        'edittext.Value = CInt(p_strCodeVendedor)

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)

    '    End Try

    'End Sub

    'Public Sub CrearChooseFromListVendedor(ByVal oForm As SAPbouiCOM.Form)

    '    Dim oCP As SAPbouiCOM.FormCreationParams
    '    Dim oItem As SAPbouiCOM.Item
    '    Dim oEdit As SAPbouiCOM.EditText

    '    oItem = oForm.Items.Item("txtCdV")
    '    oEdit = oItem.Specific

    '    oCP = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
    '    oCP.UniqueID = "CFL_Vend"
    '    oCP.FormType = "CFL_Vend"
    '    oCP.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
    '    oForm.DataSources.UserDataSources.Add("REPORTE", SAPbouiCOM.BoDataType.dt_SHORT_TEXT)

    '    oEdit.DataBind.SetBound(True, "", "REPORTE")
    '    oEdit.ChooseFromListUID = "CFL_Vend"
    '    oEdit.ChooseFromListAlias = "empID"

    'End Sub

#End Region

End Class
