Imports SCG.DMSOne.Framework.MenuManager

Public Class ConfiguracionTransaccionesCompraCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43520"

    Private Const mc_strUIDContratoVenta As String = "SCGD_UIDListCont"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_TRAN_COMP As String = "@SCGD_TRAN_COMP"
    Private Const mc_strCode As String = "Code"
    Private Const mc_strName As String = "Name"
    Private Const mc_strAcctCod As String = "U_Acc_Cod"
    Private Const mc_strAcc_Nam As String = "U_Acc_Nam"

    'Nombres de los campos de texto
    Private Const mc_strUIDCodigo As String = "3"
    Private Const mc_strUIDNombre As String = "5"
    Private Const mc_strUIDCodigoCuenta As String = "7"
    Private Const mc_strUIDNombreCuenta As String = "9"

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private SBO_Application As SAPbouiCOM.Application

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()
        
        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu("SCGD_TRC", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_TRC", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_TRC", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 25, False, True, "SCGD_CFG"))
        End If

    End Sub

    Protected Friend Sub CargaFormulario()
        '*******************************************************************    
        'Propósito: Se encarga de establecer los filtros para los eventos de la
        '            aplicacion que se van a manejar y posteriormente se los
        '            agrega al objeto aplicacion donde se esta almacenando la
        '            aplicacion SBO que esta corriendo
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_TRANS_C"

            strXMLACargar = My.Resources.Resource.TRANSForm
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Call CargarCamposVisualizacion()

            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, True)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        '*******************************************************************    
        'Propósito:  Se encarga de cargar las formas desde el archivo XML,
        '             tomando como parámetro el nombre del archivo.
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If

        Return oXMLDoc.InnerXml

    End Function

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

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        If oCFLEvento.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects

            If (pval.ItemUID = mc_strUIDCodigoCuenta) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    Call AsignarCuenta(oDataTable, pval.FormUID)

                End If

            End If
        ElseIf oCFLEvento.BeforeAction AndAlso pval.ItemUID = "7" Then
            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.BracketOpenNum = 1
            oCondition.Alias = "FatherNum"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCondition.BracketCloseNum = 1
            oCFL.SetConditions(oConditions)
        End If

    End Sub

    Private Sub AsignarCuenta(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        oform.DataSources.DBDataSources.Item(mc_strSCG_TRAN_COMP).SetValue(mc_strAcctCod, 0, oDataTable.GetValue("AcctCode", 0))
        oform.DataSources.DBDataSources.Item(mc_strSCG_TRAN_COMP).SetValue(mc_strAcc_Nam, 0, oDataTable.GetValue("AcctName", 0))
        If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If
    End Sub

    Public Sub HabilitarCampos(ByVal p_strFormID As String, ByVal blnModoNuevo As Boolean)

        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        If blnModoNuevo Then
            oform.Items.Item(mc_strUIDNombreCuenta).Enabled = False
        Else

            oform = SBO_Application.Forms.Item(p_strFormID)

            oform.Items.Item(mc_strUIDNombre).Enabled = True
            oform.Items.Item(mc_strUIDCodigo).Enabled = False
            oform.Items.Item(mc_strUIDCodigoCuenta).Enabled = True
            oform.Items.Item(mc_strUIDNombreCuenta).Enabled = False

        End If


    End Sub

    Private Sub CargarCamposVisualizacion()
        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
'        Dim oItem As SAPbouiCOM.Item

        Try

            cboCombo = DirectCast(m_oFormGenCotizacion.Items.Item("cboView").Specific, SAPbouiCOM.ComboBox)
            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues

            cboCombo.ValidValues.Add("-", "-")
            cboCombo.ValidValues.Add("ACCEXT", My.Resources.TransaccionesCompra.ACCEXT)
            cboCombo.ValidValues.Add("ACCINT", My.Resources.TransaccionesCompra.ACCINT)
            cboCombo.ValidValues.Add("AGENCIA", My.Resources.TransaccionesCompra.AGENCIA)
            cboCombo.ValidValues.Add("BODALM", My.Resources.TransaccionesCompra.BODALM)
            cboCombo.ValidValues.Add("CIF", My.Resources.TransaccionesCompra.CIF)
            cboCombo.ValidValues.Add("COMAPE", My.Resources.TransaccionesCompra.COMAPE)
            cboCombo.ValidValues.Add("COMFOR", My.Resources.TransaccionesCompra.COMFOR)
            cboCombo.ValidValues.Add("COMNEG", My.Resources.TransaccionesCompra.COMNEG)
            cboCombo.ValidValues.Add("DESALM", My.Resources.TransaccionesCompra.DESALM)
            cboCombo.ValidValues.Add("FLELOC", My.Resources.TransaccionesCompra.FLELOC)
            cboCombo.ValidValues.Add("FLETE", My.Resources.TransaccionesCompra.FLETE)
            cboCombo.ValidValues.Add("FOB", My.Resources.TransaccionesCompra.FOB)
            cboCombo.ValidValues.Add("IMPVTA", My.Resources.TransaccionesCompra.IMPVTA)
            cboCombo.ValidValues.Add("OTROS_FP", My.Resources.TransaccionesCompra.OTROS_FP)
            cboCombo.ValidValues.Add("REDEST", My.Resources.TransaccionesCompra.REDEST)
            cboCombo.ValidValues.Add("RESERVA", My.Resources.TransaccionesCompra.RESERVA)
            cboCombo.ValidValues.Add("SEGFAC", My.Resources.TransaccionesCompra.SEGFAC)
            cboCombo.ValidValues.Add("SEGLOC", My.Resources.TransaccionesCompra.SEGLOC)
            cboCombo.ValidValues.Add("TALLER", My.Resources.TransaccionesCompra.TALLER)
            cboCombo.ValidValues.Add("TRASLA", My.Resources.TransaccionesCompra.TRASLA)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try
    End Sub

#End Region


End Class
