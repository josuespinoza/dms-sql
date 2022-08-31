Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ConfiguracionLineasAdicionalesFacturaCls

#Region "Declaraciones"

    Private m_intEstadoFormulario As Integer

    Private m_strCodigoLineaFactura As String
    Private m_strLineasEliminadas As String

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43520"
    Private Const mc_strListadoContratos As String = "Listado Contratos"
    Private Const mc_strListadoContratosEN As String = "Sales Contract List"
    'Private Const mc_strGeneraOrdenVentasEN As String = "Work Orders"

    Private Const mc_strUIDContratoVenta As String = "SCGD_UIDListCont"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_CONFLINEASSUM As String = "@SCGD_CONFLINEASSUM"
    Private Const mc_strCode As String = "Code"
    Private Const mc_strName As String = "Name"
    Private Const mc_strTipo As String = "U_Tipo"
    Private Const mc_strCanceled As String = "Canceled"
    Private Const mc_strCod_Item As String = "U_Cod_Item"
    Private Const mc_strNam_Item As String = "U_Nam_Item"
    Private Const mc_strCod_GA As String = "U_Cod_GA"
    Private Const mc_strCodigoImpuesto As String = "U_Impuesto"
    Private Const mc_strNombreImpuesto As String = "U_Nam_Imp"
    Private Const mc_strNam_Imp As String = "U_Nam_Imp"

    'Nombres de los campos de texto
    Private Const mc_strUIDGastosAdicionales As String = "13"
    Private Const mc_strUIDCod_Item As String = "11"
    Private Const mc_strUIDmtx_Tipos As String = "mtx_Tipos"
    Private Const mc_strUIDImpuesto As String = "15"
    Private Const mc_strUIDNam_Item As String = "19"
    Private Const mc_strUIDNameImpuesto As String = "7"
    Private Const mc_strUIDCode As String = "3"
    Private Const mc_strUIDTipo As String = "9"
    Private Const mc_strUIDAgregar As String = "btnAgregar"
    Private Const mc_strUIDEliminar As String = "btnElimina"
    Private Const mc_strSCG_COBROXTIPO As String = "@SCGD_COBROXTIPO"
    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private SBO_Application As SAPbouiCOM.Application
    Private m_objConfiguracionesGenerales As SCGDataAccess.ConfiguracionesGeneralesAddon

    Public Enum enumTiposLineas
        scgLineaFactura = 1
        scgGastosAdicionales = 2
    End Enum

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()
        
        Dim strEtiquetaMenu As String
        
        If Utilitarios.MostrarMenu("SCGD_AF", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_AF", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_AF", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 15, False, True, "SCGD_CFG"))
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
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim strConectionString As String = String.Empty

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_ConfLineasSum"

            strXMLACargar = My.Resources.Resource.ConfLineasSum
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, True)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            If cn_Coneccion.State = ConnectionState.Open Then
                cn_Coneccion.Close()
            End If
            cn_Coneccion.ConnectionString = strConectionString
            m_objConfiguracionesGenerales = New SCGDataAccess.ConfiguracionesGeneralesAddon(cn_Coneccion)

            Call CargarDatosGrid()

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
        m_strLineasEliminadas = ""
        m_strCodigoLineaFactura = ""
        Return oXMLDoc.InnerXml

    End Function

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess Then

                Select Case pVal.ItemUID

                    Case mc_strUIDAgregar
                        Call AgregarLinea(FormUID)

                    Case mc_strUIDEliminar
                        Call EliminarLíneas(FormUID)

                End Select


            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressed" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoComboSelect(ByVal FormUID As String, _
                                               ByRef pVal As SAPbouiCOM.ItemEvent, _
                                               ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = mc_strUIDTipo) Then

                Call HabilitarCampos(FormUID)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoComboSelect" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                        ByVal strQuery As String, _
                                                        ByRef strIDItem As String)

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

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)


        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)

        Dim sCFL_ID As String
        Dim oForm As SAPbouiCOM.Form
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        sCFL_ID = oCFLEvento.ChooseFromListUID
        oForm = SBO_Application.Forms.Item(FormUID)
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

        If oCFLEvento.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects

            If (pval.ItemUID = mc_strUIDCod_Item) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    Call AsignarItem(oDataTable, pval.FormUID)

                End If

            ElseIf (pval.ItemUID = mc_strUIDImpuesto) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    Call AsignarImpuesto(oDataTable, pval.FormUID)

                End If
            ElseIf (pval.ItemUID = mc_strUIDmtx_Tipos) Then
                Call AsignarItemLineas(pval.FormUID, oDataTable, pval.Row)
            End If
        Else
            If (pval.ItemUID = mc_strUIDCod_Item) Then

                oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "6"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)
            End If
        End If

    End Sub

    Private Sub AsignarItem(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strCod_Item, 0, oDataTable.GetValue("ItemCode", 0))
        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strNam_Item, 0, oDataTable.GetValue("ItemName", 0))

        If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub AsignarImpuesto(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strCodigoImpuesto, 0, oDataTable.GetValue("Code", 0))
        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strNombreImpuesto, 0, oDataTable.GetValue("Name", 0))
        If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If
    End Sub

    Public Sub HabilitarCampos(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oItem As SAPbouiCOM.ComboBox
        Dim intIDTipoLinea As enumTiposLineas
        Dim oMatrix As SAPbouiCOM.Matrix

        oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDmtx_Tipos).Specific, SAPbouiCOM.Matrix)
        oform = SBO_Application.Forms.Item(p_strFormID)
        oItem = DirectCast(oform.Items.Item(mc_strUIDTipo).Specific, SAPbouiCOM.ComboBox)

        oform.Items.Item(mc_strUIDImpuesto).Enabled = True
        oform.Items.Item(mc_strUIDNam_Item).Enabled = False
        oform.Items.Item(mc_strUIDNameImpuesto).Enabled = False

        If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            oform.Items.Item(mc_strUIDCode).Enabled = False
        Else
            oform.Items.Item(mc_strUIDCode).Enabled = True
        End If

        If oItem.Selected IsNot Nothing Then
            intIDTipoLinea = CInt(oItem.Selected.Value)
            Select Case intIDTipoLinea
                Case enumTiposLineas.scgLineaFactura
                    oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strCod_GA, 0, "")
                    oform.Items.Item(mc_strUIDCod_Item).Enabled = True
                    oform.Items.Item(mc_strUIDGastosAdicionales).Enabled = False
                    oMatrix.Columns.Item("V_Code").Editable = True
                    oMatrix.Columns.Item("V_Gasto").Editable = False

                Case enumTiposLineas.scgGastosAdicionales
                    oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strCod_Item, 0, "")
                    oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASSUM).SetValue(mc_strNam_Item, 0, "")
                    oform.Items.Item(mc_strUIDCod_Item).Enabled = False
                    oform.Items.Item(mc_strUIDGastosAdicionales).Enabled = True
                    Call CargarValidValuesEnCombos(oform, "Select ExpnsCode, ExpnsName from OEXD where RevAcct is not null", mc_strUIDGastosAdicionales)
                    oMatrix.Columns.Item("V_Code").Editable = False
                    oMatrix.Columns.Item("V_Gasto").Editable = True

            End Select
            oMatrix.Columns.Item("V_Tipo").Editable = True

        End If


    End Sub

    Private Sub CargarDatosGrid()

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strTipoTaller As String
        '        strTipoTaller = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "InventarioVendido", m_strDireccionConfiguracion)
        strTipoTaller = m_objConfiguracionesGenerales.InventarioVehiculoVendido

        oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDmtx_Tipos).Specific, SAPbouiCOM.Matrix)
        Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item("V_Tipo").ValidValues, "Select Code, Name from [@SCGD_TIPOVEHICULO] where Code <> '" & strTipoTaller & "'")
        Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item("V_Gasto").ValidValues, "Select ExpnsCode, ExpnsName   from OEXD")
        'm_oFormGenCotizacion.DataSources.DBDataSources.Item("").Query
    End Sub

    Private Sub AgregarLinea(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intNuevoRegisto As Integer
        Dim blnLineasAgregadas As Boolean = False
        Dim strUsuario As String

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(mc_strUIDmtx_Tipos).Specific, SAPbouiCOM.Matrix)



        intNuevoRegisto = oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).Size
        If intNuevoRegisto = 0 Then

            oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).InsertRecord(intNuevoRegisto)
            intNuevoRegisto += 1
            'Else
            '    intNuevoRegisto = 1
            'End If
        Else
            strUsuario = oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).GetValue("U_Tipo", intNuevoRegisto - 1)
            If Not String.IsNullOrEmpty(strUsuario.Trim()) Then
                oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).InsertRecord(intNuevoRegisto)

                intNuevoRegisto += 1
            ElseIf intNuevoRegisto = 1 Then
                oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).SetValue("U_ItemCode", 0, " ")
            End If
        End If

        blnLineasAgregadas = True


        If blnLineasAgregadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub EliminarLíneas(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(mc_strUIDmtx_Tipos).Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()
        Do While intRegistoEliminar > -1

            If String.IsNullOrEmpty(m_strLineasEliminadas) Then
                m_strLineasEliminadas = oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).GetValue("LineId", intRegistoEliminar - 1)
            Else
                m_strLineasEliminadas &= oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).GetValue("LineId", intRegistoEliminar - 1)
            End If
            m_strCodigoLineaFactura = oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).GetValue("Code", intRegistoEliminar - 1)
            oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).RemoveRecord(intRegistoEliminar - 1)

            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)

        Loop
        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub AsignarItemLineas(ByVal p_strFormID As String, ByVal p_oDataTable As SAPbouiCOM.DataTable, _
                                  ByVal p_intFila As Integer)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasActualizadas As Boolean = False
        Dim strItemCode As String = ""
        Dim strItemName As String = ""

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item(mc_strUIDmtx_Tipos).Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = p_intFila
        If intRegistoEliminar > -1 Then


            strItemCode = p_oDataTable.GetValue("ItemCode", 0)
            strItemName = p_oDataTable.GetValue("ItemName", 0)
            oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).SetValue("U_ItemCode", intRegistoEliminar - 1, strItemCode)
            oform.DataSources.DBDataSources.Item(mc_strSCG_COBROXTIPO).SetValue("U_ItemName", intRegistoEliminar - 1, strItemName)

            blnLineasActualizadas = True

        End If
        If blnLineasActualizadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

#End Region


End Class
