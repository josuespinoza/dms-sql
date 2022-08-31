Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ConfiguracionLineasDesgloceCobroCls

#Region "Declaraciones"

    Private m_intEstadoFormulario As Integer

    Private m_strCodigoVehiculo As String
    Private m_strLineasEliminadas As String

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43520"

    Private Const mc_strUIDContratoVenta As String = "SCGD_UIDListCont"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_CONFLINEASRES As String = "@SCGD_CONFLINEASRES"
    Private Const mc_strCode As String = "Code"
    Private Const mc_strName As String = "Name"
    Private Const mc_strTipoDoc As String = "U_T_Doc"
    Private Const mc_strTipo As String = "U_Tipo"
    Private Const mc_strCanceled As String = "Canceled"
    Private Const mc_strCod_Ser As String = "U_Cod_Ser"
    Private Const mc_strCodigoImpuesto As String = "U_Impuesto"
    Private Const mc_strNombreImpuesto As String = "U_Nam_Imp"
    Private Const mc_strCodigoCuenta As String = "U_AcctCode"
    Private Const mc_strNombreCuenta As String = "U_AcctName"

    'Nombres de los campos de texto
    Private Const mc_strUIDSeries As String = "21"
    Private Const mc_strUIDImpuesto As String = "17"
    Private Const mc_strUIDNameImpuesto As String = "19"
    Private Const mc_strUIDCodCuenta As String = "txtCodCuen"
    Private Const mc_strUIDNameCuenta As String = "txtNamCuen"
    Private Const mc_strUIDCode As String = "3"
    Private Const mc_strUIDTipoDoc As String = "11"
    Private Const mc_strUIDTipo As String = "9"

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private SBO_Application As SAPbouiCOM.Application

    Public Enum enumTiposDocumentos
        scgNinguno = 0
        scgNotaCredito = 1
        scgNotaDebito = 2
    End Enum

    Private Enum enumTiposLineas
        scgNotaResta = 1
        scgNotaSuma = 2
    End Enum

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

        If Utilitarios.MostrarMenu("SCGD_AD", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_AD", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_AD", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 20, False, True, "SCGD_CFG"))

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
            fcp.FormType = "SCGD_CONFLINEASRES"

            strXMLACargar = My.Resources.Resource.CONFLINEASRES
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, True)

            ' Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_CONFLINEASSUM)
            ' Call m_oFormGenCotizacion.EnableMenu("1282", False)

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
        m_strCodigoVehiculo = ""
        Return oXMLDoc.InnerXml

    End Function

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = mc_strUIDTipoDoc) Then

                Call HabilitarCampos(FormUID)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressed" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ManejadorEventoComboSelect(ByVal FormUID As String, _
                                               ByRef pVal As SAPbouiCOM.ItemEvent, _
                                               ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = mc_strUIDTipoDoc) Then

                Call HabilitarCampos(FormUID)
            ElseIf Not oForm Is Nothing _
                                AndAlso pVal.ActionSuccess _
                                AndAlso (pVal.ItemUID = mc_strUIDTipo) Then
                Call CambiarDocumento(FormUID)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoComboSelect" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Protected Friend Function CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                        ByVal strQuery As String, _
                                                        ByRef strIDItem As String, _
                                                        ByVal p_strValorSeleccionado As String) As Boolean

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim blnEliminarValor As Boolean = True

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
                    If p_strValorSeleccionado = drdResultadoConsulta.Item(0).ToString.Trim Then
                        blnEliminarValor = False
                    End If
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

            Return blnEliminarValor
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

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

        If oCFLEvento.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects

            If (pval.ItemUID = mc_strUIDImpuesto) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    Call AsignarImpuesto(oDataTable, pval.FormUID)

                End If
            ElseIf (pval.ItemUID = mc_strUIDCodCuenta) Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    Call AsignarCuenta(oDataTable, pval.FormUID)

                End If

            End If

        End If

    End Sub

    Private Sub AsignarImpuesto(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strCodigoImpuesto, 0, oDataTable.GetValue("Code", 0))
        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strNombreImpuesto, 0, oDataTable.GetValue("Name", 0))

    End Sub

    Private Sub AsignarCuenta(ByRef oDataTable As SAPbouiCOM.DataTable, ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strCodigoCuenta, 0, oDataTable.GetValue("AcctCode", 0))
        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strNombreCuenta, 0, oDataTable.GetValue("AcctName", 0))

    End Sub

    Public Sub HabilitarCampos(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oItemTipoDoc As SAPbouiCOM.ComboBox
        Dim oItemTipo As SAPbouiCOM.ComboBox
        Dim intIDTipoDoc As enumTiposDocumentos
        Dim intIDTipo As enumTiposLineas
        Dim strSerieSeleccionada As String

        oform = SBO_Application.Forms.Item(p_strFormID)
        oItemTipoDoc = DirectCast(oform.Items.Item(mc_strUIDTipoDoc).Specific, SAPbouiCOM.ComboBox)
        oItemTipo = DirectCast(oform.Items.Item(mc_strUIDTipo).Specific, SAPbouiCOM.ComboBox)

        oform.Items.Item(mc_strUIDImpuesto).Enabled = True
        oform.Items.Item(mc_strUIDNameImpuesto).Enabled = False
        oform.Items.Item(mc_strUIDCodCuenta).Enabled = True
        oform.Items.Item(mc_strUIDNameCuenta).Enabled = False

        If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            oform.Items.Item(mc_strUIDCode).Enabled = False
        Else
            oform.Items.Item(mc_strUIDCode).Enabled = True
        End If

        If oItemTipo.Selected IsNot Nothing Then
            intIDTipo = oItemTipo.Selected.Value
        End If
        If oItemTipoDoc.Selected IsNot Nothing Then
            intIDTipoDoc = CInt(oItemTipoDoc.Selected.Value)
            Select Case intIDTipoDoc
                Case enumTiposDocumentos.scgNinguno
                    If intIDTipo = enumTiposLineas.scgNotaResta Then
                        oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strCod_Ser, 0, "")
                        oform.Items.Item(mc_strUIDSeries).Enabled = False
                    Else
                        oItemTipoDoc.Select(CStr(enumTiposDocumentos.scgNotaDebito))
                    End If
                    oform.Items.Item(mc_strUIDCodCuenta).Enabled = False
                Case enumTiposDocumentos.scgNotaCredito
                    If intIDTipo = enumTiposLineas.scgNotaResta Then
                        strSerieSeleccionada = oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).GetValue(mc_strCod_Ser, 0)
                        oform.Items.Item(mc_strUIDSeries).Enabled = True
                        If CargarValidValuesEnCombos(oform, "SELECT     Series, SeriesName FROM NNM1 where ObjectCode = '14'", mc_strUIDSeries, strSerieSeleccionada.Trim) Then
                            oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strCod_Ser, 0, "")
                        End If

                    Else
                        oItemTipoDoc.Select(CStr(enumTiposDocumentos.scgNotaDebito))
                    End If
                Case enumTiposDocumentos.scgNotaDebito
                    If intIDTipo = enumTiposLineas.scgNotaSuma Then
                        strSerieSeleccionada = oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).GetValue(mc_strCod_Ser, 0)
                        oform.Items.Item(mc_strUIDSeries).Enabled = True
                        If CargarValidValuesEnCombos(oform, "SELECT     Series, SeriesName from NNM1 where ObjectCode = '13' and DocSubType = 'DN'", mc_strUIDSeries, strSerieSeleccionada.Trim) Then
                            oform.DataSources.DBDataSources.Item(mc_strSCG_CONFLINEASRES).SetValue(mc_strCod_Ser, 0, "")
                        End If
                    Else
                        oItemTipoDoc.Select(CStr(enumTiposDocumentos.scgNinguno))
                    End If
            End Select
        End If


    End Sub

    Private Sub CambiarDocumento(ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form
        Dim oItemTipoDoc As SAPbouiCOM.ComboBox
        Dim oItemTipo As SAPbouiCOM.ComboBox
        Dim intIDTipo As enumTiposLineas
        Dim intIDTipoDoc As enumTiposLineas
        oform = SBO_Application.Forms.Item(p_strFormID)
        oItemTipoDoc = DirectCast(oform.Items.Item(mc_strUIDTipoDoc).Specific, SAPbouiCOM.ComboBox)
        oItemTipo = DirectCast(oform.Items.Item(mc_strUIDTipo).Specific, SAPbouiCOM.ComboBox)

        oform.Items.Item(mc_strUIDImpuesto).Enabled = True
        oform.Items.Item(mc_strUIDNameImpuesto).Enabled = False

        If oItemTipo.Selected IsNot Nothing Then
            intIDTipo = oItemTipo.Selected.Value
        End If
        If oItemTipoDoc.Selected IsNot Nothing Then
            intIDTipoDoc = CInt(oItemTipoDoc.Selected.Value)
        End If
        Select Case intIDTipo
            Case enumTiposLineas.scgNotaResta
                If intIDTipoDoc = enumTiposLineas.scgNotaSuma Then

                    oItemTipoDoc.Select(CStr(enumTiposDocumentos.scgNinguno))

                End If
            Case enumTiposLineas.scgNotaSuma
                If intIDTipoDoc <> enumTiposLineas.scgNotaSuma Then

                    oItemTipoDoc.Select(CStr(enumTiposDocumentos.scgNotaDebito))

                End If

        End Select
    End Sub
#End Region


End Class
