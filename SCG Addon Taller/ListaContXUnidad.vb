Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ListaContXUnidad

    Private m_oCompany As SAPbobsCOM.Company
    Private SBO_Application As SAPbouiCOM.Application
    Private Const mc_strUIDListaCVXVeh As String = "SCGD_LCU"
    Private m_oForm As SAPbouiCOM.Form
    Private m_dataTableContratos As SAPbouiCOM.DataTable
    Private m_oVehiculos As VehiculosCls

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu(mc_strUIDListaCVXVeh, SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu(mc_strUIDListaCVXVeh, SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDListaCVXVeh, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 50, False, True, "SCGD_MNO"))

        End If

    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    Protected Friend Sub CargaFormularioListaContXUnidad()

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String

        Try

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_CONTXVEH"

            strXMLACargar = My.Resources.Resource.ListaContXUnidad
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oForm = SBO_Application.Forms.AddEx(fcp)

            m_dataTableContratos = m_oForm.DataSources.DataTables.Add("Contratos")
            
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                                            ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)

        Dim strUnidad As String
        Dim strIDVeh As String

        If pVal.ItemUID = "lkUnidad" AndAlso pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            m_oVehiculos = New VehiculosCls(m_oCompany, SBO_Application)

            strUnidad = oForm.Items.Item("txtUnidad").Specific.value
            strUnidad = strUnidad.Trim()
            strIDVeh = Utilitarios.EjecutarConsulta("SELECT DocEntry FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '" & strUnidad & "'", SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

            Call m_oVehiculos.DibujarFormularioDetalleInformacionVehiculo("", _
                                         strIDVeh, _
                                         True, _
                                         "", _
                                         0, True, False, VehiculosCls.ModoFormulario.scgVentas)

        End If

    End Sub

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                                            ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID
        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)
        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
        Dim oDataTable As SAPbouiCOM.DataTable

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim strUnidad As String
        
        If oCFLEvento.BeforeAction = False Then

            oDataTable = oCFLEvento.SelectedObjects

            If pVal.ItemUID = "btnCargarU" Then

                strUnidad = oDataTable.Columns.Item("U_Cod_Unid").Cells.Item(0).Value
                strUnidad = strUnidad.Trim()

                oForm.Items.Item("txtUnidad").Specific.value = strUnidad

                Call CargarMatrix(oForm, strUnidad)

            End If

        ElseIf oCFLEvento.BeforeAction = True Then

            If pVal.ItemUID = "btnCargarU" Then

                oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_Cod_Unid"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)

            End If
            
        End If

    End Sub

    Private Sub CargarMatrix(ByVal oForm As SAPbouiCOM.Form, ByVal strUnidad As String)

        Dim strConsultaContratos As String
        
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oColContrato As SAPbouiCOM.Column
        Dim oColCliente As SAPbouiCOM.Column
        Dim oColMonto As SAPbouiCOM.Column

        Try

            oMatrix = DirectCast(oForm.Items.Item("mtxCont").Specific, SAPbouiCOM.Matrix)

            oColContrato = oMatrix.Columns.Item("col_CV")
            oColCliente = oMatrix.Columns.Item("col_Cli")
            oColMonto = oMatrix.Columns.Item("col_Mon")

            oMatrix.Clear()

            m_dataTableContratos = oForm.DataSources.DataTables.Item("Contratos")

            strConsultaContratos = "SELECT DocNum,U_CardName,U_DocTotal FROM [@SCGD_CVENTA] INNER JOIN [@SCGD_VEHIXCONT] ON [@SCGD_CVENTA].DocEntry=[@SCGD_VEHIXCONT].DocEntry WHERE [@SCGD_VEHIXCONT].U_Cod_Unid='" & strUnidad & "'"

            m_dataTableContratos.ExecuteQuery(strConsultaContratos)

            oColContrato.DataBind.Bind("Contratos", "DocNum")
            oColCliente.DataBind.Bind("Contratos", "U_CardName")
            oColMonto.DataBind.Bind("Contratos", "U_DocTotal")

            oMatrix.LoadFromDataSource()

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Function DevolverIDContrato(ByVal p_intRow As Integer, _
                                        ByVal p_strIDForm As String) As String

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDContrato As String

        oMatriz = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item("mtxCont").Specific, SAPbouiCOM.Matrix)
        strIDContrato = oMatriz.Columns.Item("col_CV").Cells.Item(p_intRow).Specific.String()

        Return strIDContrato

    End Function

End Class
