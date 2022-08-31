Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI

Partial Public Class VehiculoSeleccionMarcaEstilo

    Public Shared m_strCodModelos As String
    Public Shared m_strCodEstilo As String
    Public Shared m_StrCodMarca As String
    Public Shared m_intPos As String
    Public Shared m_StrUnidCode As String

    Public Sub CargaFormulario(ByVal p_intPos As Integer,
                               ByVal p_strCodMarca As String,
                               ByVal p_strCodeEstilo As String,
                               ByVal p_strCodModelo As String,
                               ByVal p_strUnidCode As String)

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim strConsulta As String = ""
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Try


            'Parámetros del formulario
            fcp = _applicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed
            fcp.FormType = "SCGD_SME"
            StrFormType = fcp.FormType

            'XML a cargar
            strXMLACargar = My.Resources.Resource.XMLFormularioSeleccionMarcaEstiloModelo

            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = _applicationSBO.Forms.AddEx(fcp)
            oForm.Mode = BoFormMode.fm_OK_MODE
            oForm.AutoManaged = False
            oForm.SupportedModes = 0

            LigarControles()

            oForm.DataSources.DataTables.Add("dtMarca")
            oForm.DataSources.DataTables.Add("dtEstilo")
            oForm.DataSources.DataTables.Add("dtModelo")
            oForm.DataSources.DataTables.Add("dtLocal")

            m_StrCodMarca = p_strCodMarca
            m_strCodEstilo = p_strCodeEstilo
            m_strCodModelos = p_strCodModelo
            m_intPos = p_intPos
            m_StrUnidCode = p_strUnidCode

            oForm.Freeze(True)

            CargarCombos()

            If Not String.IsNullOrEmpty(m_StrCodMarca) Then

                oItem = oForm.Items.Item(cboMarca.UniqueId)
                oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                oCombo.Select(m_StrCodMarca.Trim, SAPbouiCOM.BoSearchKey.psk_ByValue)

            End If
            If Not String.IsNullOrEmpty(m_strCodEstilo) Then

                oItem = oForm.Items.Item(cboEstilo.UniqueId)
                oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
                oCombo.Select(m_strCodEstilo.Trim, SAPbouiCOM.BoSearchKey.psk_ByValue)

            End If

            If Not String.IsNullOrEmpty(m_strCodModelos) Then

                oItem = oForm.Items.Item(cboModelo.UniqueId)
                oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
                oCombo.Select(m_strCodModelos.Trim, SAPbouiCOM.BoSearchKey.psk_ByValue)

            End If

            txtArticulo.AsignaValorUserDataSource(m_StrUnidCode)

            oForm.Freeze(False)

        Catch ex As Exception
            oForm.Freeze(False)
            Call Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Public txtArticulo As EditTextSBO
    Public cboMarca As ComboBoxSBO
    Public cboEstilo As ComboBoxSBO
    Public cboModelo As ComboBoxSBO

    Private Sub LigarControles()
        Try

            Dim m_udsLocal As UserDataSources = oForm.DataSources.UserDataSources
            m_udsLocal.add("codUni", BoDataType.dt_LONG_TEXT, 150)
            m_udsLocal.add("codMar", BoDataType.dt_LONG_TEXT, 150)
            m_udsLocal.add("codEst", BoDataType.dt_LONG_TEXT, 150)
            m_udsLocal.add("codMod", BoDataType.dt_LONG_TEXT, 150)

            txtArticulo = New EditTextSBO("txtUnid", True, "", "codUni", oForm)
            cboMarca = New ComboBoxSBO("cboMarca", oForm, True, "", "codMar")
            cboEstilo = New ComboBoxSBO("cboEstilo", oForm, True, "", "codEst")
            cboModelo = New ComboBoxSBO("cboModelo", oForm, True, "", "codMod")

            txtArticulo.AsignaBinding()
            cboMarca.AsignaBinding()
            cboModelo.AsignaBinding()
            cboEstilo.AsignaBinding()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
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

    Private Sub CargarCombos()
        Try

            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            dtLocal = oForm.DataSources.DataTables.Item("dtLocal")

            dtLocal.Clear()
            dtLocal.ExecuteQuery("  select ""Code"", ""Name"" from ""@SCGD_MARCA"" order by ""Name"" ASC")

            oItem = oForm.Items.Item("cboMarca")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

            dtLocal.Clear()
            dtLocal.ExecuteQuery(" select ""Code"", ""Name"" from ""@SCGD_ESTILO"" ")

            oItem = oForm.Items.Item("cboEstilo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = "SCGD_SME" Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                ManejadorEventoItemPress(pVal, FormUID, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                ManejadorEventoComboSelect(pVal, FormUID, BubbleEvent)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                               ByVal FormUID As String,
                               ByRef BubbleEvent As Boolean)
        Try

            If Not pval.FormTypeEx = StrFormType Then Return

            If pval.EventType = BoEventTypes.et_ITEM_PRESSED Then

                Select Case pval.ItemUID
                    Case "btnAceptar"

                        SeleccionarMarcaEstiloModelo()
                        oForm.Close()

                    Case "btnCancel"
                        oForm.Close()
                End Select

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoComboSelect(ByRef pval As SAPbouiCOM.ItemEvent,
                           ByVal FormUID As String,
                           ByRef BubbleEvent As Boolean)
        Try
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            Dim strValorSeleccionado As String = String.Empty


            If pval.ActionSuccess Then


                Select Case pval.ItemUID
                    Case "cboMarca2"
                        oCombo = DirectCast(oForm.Items.Item("cboMarca").Specific, SAPbouiCOM.ComboBox)
                        If oCombo.Selected IsNot Nothing Then
                            strValorSeleccionado = oCombo.Selected.Value
                        End If

                        If oCombo.ValidValues.Count <= 1 Then
                            'Call CargarComboMarca(oForm)
                        End If

                    Case "cboMarca"

                        CargarComboEstilos(oForm, False)


                    Case "cboEstilo"
                        CargarComboModelos(oForm, False)

                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Protected Friend Sub CargarComboEstilos(ByRef oForm As SAPbouiCOM.Form,
                                           ByVal p_blnSeleccionaValor As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim intRecIndex As Integer
            Dim l_strSQL As String = "Select ""Code"", ""Name"" from ""@SCGD_ESTILO"" "

            Dim strCodMarca As String
            Dim strCodEstilo As String


            oItem = oForm.Items.Item("cboMarca")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            strCodMarca = CStr(oCombo.Value).Trim

            oItem = oForm.Items.Item("cboEstilo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            strCodEstilo = CStr(oCombo.Value).Trim

            oForm.Freeze(True)

            If oCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To oCombo.ValidValues.Count - 1
                    oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            dtEstilo = oForm.DataSources.DataTables.Item("dtEstilo")
            dtEstilo.Clear()

            If String.IsNullOrEmpty(strCodMarca) Then
                dtEstilo.ExecuteQuery(l_strSQL & " Order By Name ASC")
            Else
                l_strSQL = l_strSQL & String.Format(" Where U_Cod_Marc = '" & strCodMarca & "' Order By Name ASC")
                dtEstilo.ExecuteQuery(l_strSQL)
            End If

            If Not String.IsNullOrEmpty(dtEstilo.GetValue("Code", 0)) Then

                For i As Integer = 0 To dtEstilo.Rows.Count - 1
                    oCombo.ValidValues.Add(dtEstilo.GetValue("Code", i), dtEstilo.GetValue("Name", i))
                Next

            End If


            'If p_blnSeleccionaValor Then
            '    oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Esti", 0, p_strIDValSelect)
            'Else
            '    oForm.DataSources.DBDataSources.Item(mc_strVehiculo).SetValue("U_Cod_Esti", 0, strVal)
            'End If
            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Protected Friend Sub CargarComboModelos(ByRef oForm As SAPbouiCOM.Form,
                                       ByVal p_blnSeleccionaValor As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim intRecIndex As Integer
            Dim l_strSQL As String = "  Select ""Code"", ""U_Descripcion"" from ""@SCGD_MODELO""  "

            Dim strCodEstilo As String
            Dim strCodModelo As String


            oItem = oForm.Items.Item("cboEstilo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            strCodEstilo = CStr(oCombo.Value).Trim

            oItem = oForm.Items.Item("cboModelo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            strCodModelo = CStr(oCombo.Value).Trim

            ' oForm.Freeze(True)

            If oCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To oCombo.ValidValues.Count - 1
                    oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            dtModelo = oForm.DataSources.DataTables.Item("dtModelo")
            dtModelo.Clear()

            If Not String.IsNullOrEmpty(strCodEstilo) Then
                l_strSQL = l_strSQL & String.Format("  where U_Cod_Esti = '" & strCodEstilo & "' Order By U_Descripcion")
                dtModelo.ExecuteQuery(l_strSQL)
            End If


            If Not String.IsNullOrEmpty(dtModelo.GetValue("Code", 0)) Then

                For i As Integer = 0 To dtModelo.Rows.Count - 1
                    oCombo.ValidValues.Add(dtModelo.GetValue("Code", i), dtModelo.GetValue("U_Descripcion", i))
                Next
            End If


            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Private Sub SeleccionarMarcaEstiloModelo()
        Try

            Dim l_strMarca As String
            Dim l_strEstilo As String
            Dim l_strModelo As String

            Dim l_strDescMarca As String
            Dim l_strDescEstilo As String
            Dim l_strDescModelo As String


            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            MatrizVeh.Matrix.FlushToDataSource()

            oItem = oForm.Items.Item("cboMarca")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            l_strMarca = oCombo.Value.Trim
            If Not String.IsNullOrEmpty(oCombo.Value.Trim) Then
                l_strDescMarca = oCombo.Selected.Description.Trim
            End If

            oItem = oForm.Items.Item("cboEstilo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            l_strEstilo = oCombo.Value.Trim
            If Not String.IsNullOrEmpty(oCombo.Value.Trim) Then
                l_strDescEstilo = oCombo.Selected.Description.Trim
            End If

            oItem = oForm.Items.Item("cboModelo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            l_strModelo = oCombo.Value.Trim
            If Not String.IsNullOrEmpty(oCombo.Value.Trim) Then
                l_strDescModelo = oCombo.Selected.Description.Trim
            End If



            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Cod_Mar", m_intPos, l_strMarca)
            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Cod_Est", m_intPos, l_strEstilo)
            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Cod_Mod", m_intPos, l_strModelo)

            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Des_Mar", m_intPos, l_strDescMarca)
            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Des_Est", m_intPos, l_strDescEstilo)
            _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Des_Mod", m_intPos, l_strDescModelo)

            If _formConfiguracion.Mode = BoFormMode.fm_OK_MODE Then
                _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
            End If


            MatrizVeh.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub


End Class
