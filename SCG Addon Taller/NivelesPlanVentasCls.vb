Imports SCG.DMSOne.Framework.MenuManager

Public Class NivelesPlanVentasCls

#Region "Declaraciones"
     Private m_oCompany As SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application
#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()


        Dim strEtiquetaMenu As String = ""

        Dim sPath As String

        sPath = Application.StartupPath

        strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_CFG", SBO_Application.Language)
        GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_CFG", SAPbouiCOM.BoMenuType.mt_POPUP, strEtiquetaMenu, 15, False, True, sPath & "\setup.bmp", "43520"))

        'If Utilitarios.MostrarMenu("SCGD_PRM", SBO_Application.Company.UserName) Then


        '    strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_PRM", SBO_Application.Language)

        '    GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_PRM", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 5, False, True, "SCGD_CFG"))

        'End If

    End Sub

    'Protected Friend Sub CargaFormularioPermisos()
    '    '*******************************************************************    
    '    'Propósito: Se encarga de establecer los filtros para los eventos de la
    '    '            aplicacion que se van a manejar y posteriormente se los
    '    '            agrega al objeto aplicacion donde se esta almacenando la
    '    '            aplicacion SBO que esta corriendo
    '    '
    '    'Acepta:    Ninguno
    '    'Retorna:   Ninguno
    '    'Desarrollador: Yeiner
    '    'Fecha: 19 Abril 2006
    '    '********************************************************************
    '    Try

    '        Dim fcp As SAPbouiCOM.FormCreationParams
    '        Dim oMatrix As SAPbouiCOM.Matrix
    '        Dim strXMLACargar As String

    '        fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
    '        fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
    '        fcp.FormType = "SCGD_NIVELES_PV"

    '        strXMLACargar = My.Resources.Resource.NIVELES_PVForm
    '        fcp.XmlData = CargarDesdeXML(strXMLACargar)

    '        m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

    '        Dim strConexionDBSucursal As String = ""

    '        Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_CVENTA)
    '        Call m_oFormGenCotizacion.EnableMenu("1282", False)

    '        m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_CVENTA)

    '        oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
    '        oMatrix.Columns.Item("col_0").Editable = False

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)
    '        'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '    End Try
    'End Sub

    'Private Function CargarDesdeXML(ByRef strFileName As String) As String
    '    '*******************************************************************    
    '    'Propósito:  Se encarga de cargar las formas desde el archivo XML,
    '    '             tomando como parámetro el nombre del archivo.
    '    '
    '    'Acepta:    Ninguno
    '    'Retorna:   Ninguno
    '    'Desarrollador: Yeiner
    '    'Fecha: 19 Abril 2006
    '    '********************************************************************
    '    Dim oXMLDoc As Xml.XmlDataDocument
    '    Dim strPath As String

    '    strPath = Application.StartupPath & "\" & strFileName
    '    oXMLDoc = New Xml.XmlDataDocument

    '    If Not oXMLDoc Is Nothing Then
    '        oXMLDoc.Load(strPath)
    '    End If
    '    m_strLineasEliminadas = ""
    '    m_strCodigoVehiculo = ""
    '    Return oXMLDoc.InnerXml

    'End Function

    'Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
    '                                               ByRef pVal As SAPbouiCOM.ItemEvent, _
    '                                               ByRef BubbleEvent As Boolean)
    '    Try

    '        '            Dim oMatrix As SAPbouiCOM.Matrix
    '        Dim oForm As SAPbouiCOM.Form
    '        oForm = SBO_Application.Forms.Item(FormUID)

    '        If Not oForm Is Nothing Then
    '            If pVal.BeforeAction Then

    '            ElseIf pVal.ActionSuccess Then
    '                Select Case pVal.ItemUID
    '                    Case mc_strUIDEliminar
    '                        Call EliminarUsuarios(FormUID)
    '                    Case "1"
    '                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

    '                            Utilitarios.EjecutarConsulta("update [@SCGD_NIVELES_PV] set UpdateDate = NULL",
    '                                                         m_oCompany.CompanyDB, m_oCompany.Server)
    '                        End If
    '                End Select
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)
    '        'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '    End Try
    'End Sub

    'Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
    '                                          ByVal FormUID As String, _
    '                                          ByRef BubbleEvent As Boolean)


    '    Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
    '    oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)

    '    Dim sCFL_ID As String
    '    sCFL_ID = oCFLEvento.ChooseFromListUID
    '    Dim oForm As SAPbouiCOM.Form
    '    oForm = SBO_Application.Forms.Item(FormUID)
    '    Dim oCFL As SAPbouiCOM.ChooseFromList
    '    oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
    '    Dim oDataTable As SAPbouiCOM.DataTable

    '    If oCFLEvento.BeforeAction = False Then

    '        oDataTable = oCFLEvento.SelectedObjects

    '        If (pval.ItemUID = mc_strUIDAgregar) Then


    '            If Not oCFLEvento.SelectedObjects Is Nothing Then

    '                Call AsignarUsuarios(oDataTable, pval.FormUID)

    '            End If

    '        End If

    '    End If

    'End Sub

    'Private Sub AsignarUsuarios(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
    '    Dim intCantidad As Integer
    '    Dim oform As SAPbouiCOM.Form
    '    Dim oMatriz As SAPbouiCOM.Matrix
    '    Dim intNuevoRegisto As Integer
    '    Dim blnLineasAgregadas As Boolean = False
    '    Dim strUsuario As String

    '    oform = SBO_Application.Forms.Item(p_strFormID)
    '    oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)

    '    For intCantidad = 0 To oDataTable.Rows.Count - 1

    '        intNuevoRegisto = oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").Size
    '        If intNuevoRegisto = 1 Then
    '            strUsuario = oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").GetValue("U_Usuario", 0)
    '            If Not String.IsNullOrEmpty(strUsuario) Then

    '                oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").InsertRecord(intNuevoRegisto)
    '                intNuevoRegisto += 1
    '            Else
    '                intNuevoRegisto = 1
    '            End If
    '        Else
    '            oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").InsertRecord(intNuevoRegisto)
    '            intNuevoRegisto += 1
    '        End If


    '        oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").SetValue("U_Usuario", intNuevoRegisto - 1, oDataTable.GetValue("USER_CODE", intCantidad))
    '        blnLineasAgregadas = True

    '    Next intCantidad
    '    If blnLineasAgregadas Then
    '        oMatriz.LoadFromDataSource()
    '        oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
    '    End If

    'End Sub

    'Private Sub EliminarUsuarios(ByVal p_strFormID As String)

    '    Dim oform As SAPbouiCOM.Form
    '    Dim oMatriz As SAPbouiCOM.Matrix
    '    Dim intRegistoEliminar As Integer
    '    Dim blnLineasEliminadas As Boolean = False

    '    oform = SBO_Application.Forms.Item(p_strFormID)
    '    oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)
    '    intRegistoEliminar = oMatriz.GetNextSelectedRow()
    '    Do While intRegistoEliminar > -1

    '        If String.IsNullOrEmpty(m_strLineasEliminadas) Then
    '            m_strLineasEliminadas = oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").GetValue("LineId", intRegistoEliminar - 1)
    '        Else
    '            m_strLineasEliminadas &= oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").GetValue("LineId", intRegistoEliminar - 1)
    '        End If
    '        m_strCodigoVehiculo = oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").GetValue("Code", intRegistoEliminar - 1)
    '        oform.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV").RemoveRecord(intRegistoEliminar - 1)

    '        blnLineasEliminadas = True
    '        intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)

    '    Loop
    '    If blnLineasEliminadas Then
    '        oMatriz.LoadFromDataSource()
    '        oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
    '    End If

    'End Sub

    'Public Sub EliminarUsuariosBD()

    '    Dim a_strLineasEliminadas() As String
    '    Dim strlineaEliminada As String
    '    Dim strConsulta As String
    '    Dim blnPrimeraLinea As Boolean = True

    '    If Not String.IsNullOrEmpty(m_strLineasEliminadas) Then
    '        a_strLineasEliminadas = m_strLineasEliminadas.Split(",")
    '        strConsulta = "Delete from [@SCGD_PERMISOS_PV] where Code = '" & m_strCodigoVehiculo.Trim() & "' and (LineId = "
    '        For Each strlineaEliminada In a_strLineasEliminadas
    '            If blnPrimeraLinea Then
    '                strConsulta &= strlineaEliminada
    '            Else
    '                strConsulta &= " or LineId = " & strlineaEliminada
    '                blnPrimeraLinea = False
    '            End If
    '        Next
    '        strConsulta &= ")"
    '        Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
    '    End If
    '    m_strLineasEliminadas = ""
    '    m_strCodigoVehiculo = ""

    'End Sub

    'Public Sub LimpiarLineasAEliminar()
    '    m_strLineasEliminadas = ""
    '    m_strCodigoVehiculo = ""
    'End Sub

#End Region


End Class
