Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO

Partial Public Class VehiculoArticuloVenta

    Dim oMatrixTmp As SAPbouiCOM.Matrix

    Public Sub CargaFormulario()

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim strConsulta As String = ""

        Try
            'Parámetros del formulario
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_VAV"
            StrFormType = fcp.FormType

            'XML a cargar
            strXMLACargar = My.Resources.Resource.XMLVehiculoArticuloVenta
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = m_SBO_Application.Forms.AddEx(fcp)
            oForm.Mode = BoFormMode.fm_OK_MODE

            'Asociación de la matrix al formulario
            Call LinkMatriz()

            oMatrix = DirectCast(oForm.Items.Item("mtxArtic").Specific, SAPbouiCOM.Matrix)
            dtArticulos = oForm.DataSources.DataTables.Item("numArtic")

            strConsulta = "SELECT ""Code"", ""Name"", ""U_ArtVent"" FROM ""@SCGD_CONF_ART_VENTA"" order by ""Name"" "

            Call CargarMatriz(oMatrix, oForm, strConsulta)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
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

    Private Sub LinkMatriz()

        dtArticulos = oForm.DataSources.DataTables.Add("numArtic")
        dtArticulos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtArticulos.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizNumeracion = New MatrixVehiculoArticuloVenta("mtxArtic", oForm, "numArtic")
        MatrizNumeracion.CreaColumnas()
        MatrizNumeracion.LigaColumnas()

    End Sub

    Public Function CargarMatriz(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                   ByVal oform As SAPbouiCOM.Form, _
                                   ByVal Consulta As String) As Boolean

        Dim strConsulta As String = ""
        strConsulta = Consulta

        Try
            oMatrix.Clear()
            dtArticulos.Rows.Clear()

            If Not String.IsNullOrEmpty(strConsulta) Then
                dtArticulos.ExecuteQuery(strConsulta)
            End If

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return False
        End Try

    End Function

    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                ByVal FormUID As String,
                                ByRef BubbleEvent As Boolean)

        Try

            If Not pval.FormTypeEx = StrFormType Then Return
            If pval.EventType = BoEventTypes.et_ITEM_PRESSED Then


                Select Case pval.ItemUID
                    Case "btnAgregar"
                        ButtonAgregarItemPressed(FormUID, pval, BubbleEvent)
                    Case "btnBuscar"
                        ButtonBuscarItemPressed(FormUID, pval, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoDobleClick(ByVal FormUID As String, _
                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                 ByRef BubbleEvent As Boolean)
        Try
            Dim oMatrizArtic As SAPbouiCOM.Matrix
            oMatrizArtic = DirectCast(oForm.Items.Item("mtxArtic").Specific, SAPbouiCOM.Matrix)

            If Not pVal.FormTypeEx = "SCGD_VAV" Then Return

            If pVal.FormTypeEx = "SCGD_VAV" Then

                If pVal.BeforeAction Then
                    If pVal.ColUID = "V_-1" Then
                        BubbleEvent = False
                        oForm.Freeze(True)
                        If pVal.ItemUID = "mtxArtic" Then


                            If oMatrizArtic.RowCount > 0 Then
                                For i As Integer = 1 To oMatrizArtic.RowCount
                                    If oMatrizArtic.IsRowSelected(i) Then

                                        Dim strArtCode As String = dtArticulos.GetValue(0, i - 1).ToString()
                                        Dim strArtName As String = dtArticulos.GetValue(1, i - 1).ToString()
                                        Dim strArtVent As String = dtArticulos.GetValue(2, i - 1).ToString()

                                        Select Case _formConfiguracion.UniqueID
                                            Case "SCGD_DET_1"
                                                _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_ArtVent", 0, strArtCode)
                                                _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_ArtVentDesc", 0, strArtName)

                                            Case "SCGD_EPM"
                                                _formConfiguracion.DataSources.UserDataSources.Item("MarcCom").ValueEx = strArtCode
                                                _formConfiguracion.DataSources.UserDataSources.Item("MarcComD").ValueEx = strArtName
                                                _formConfiguracion.DataSources.UserDataSources.Item("ItemD").ValueEx = strArtVent

                                        End Select

                                        If _formConfiguracion.Mode = BoFormMode.fm_OK_MODE Then
                                            _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                        End If

                                        oForm.Close()

                                        Exit Sub
                                    End If
                                Next
                            End If
                        End If
                        oForm.Freeze(True)
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub


    Public Sub ButtonAgregarItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrizAgreg As SAPbouiCOM.Matrix
            oMatrizAgreg = DirectCast(oForm.Items.Item("mtxArtic").Specific, SAPbouiCOM.Matrix)

            If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
                If oMatrizAgreg.RowCount = 0 Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarMarcaComercial, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                End If

            ElseIf pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

                If oMatrizAgreg.RowCount > 0 Then

                    For i As Integer = 1 To oMatrizAgreg.RowCount

                        If oMatrizAgreg.IsRowSelected(i) Then

                            Dim strArtCode As String = dtArticulos.GetValue(0, i - 1).ToString()
                            Dim strArtName As String = dtArticulos.GetValue(1, i - 1).ToString()
                            Dim strArtVent As String = dtArticulos.GetValue(2, i - 1).ToString()

                            Select Case _formConfiguracion.UniqueID
                                Case "SCGD_DET_1"
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_ArtVent", 0, strArtCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_VEHICULO").SetValue("U_ArtVentDesc", 0, strArtName)
                                    
                                Case "SCGD_EPM"
                                    _formConfiguracion.DataSources.UserDataSources.Item("MarcCom").ValueEx = strArtCode
                                    _formConfiguracion.DataSources.UserDataSources.Item("MarcComD").ValueEx = strArtName
                                    _formConfiguracion.DataSources.UserDataSources.Item("ItemD").ValueEx = strArtVent
                                    
                            End Select

                            If _formConfiguracion.Mode = BoFormMode.fm_OK_MODE Then
                                _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                            End If

                            oForm.Close()

                            Exit Sub

                        End If

                    Next

                End If

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)

        End Try

    End Sub

    Public Sub ButtonBuscarItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim strNombreMarca As String = String.Empty
            Dim l_strSQL As String = " SELECT ""Code"", ""Name"", ""U_ArtVent"" FROM ""@SCGD_CONF_ART_VENTA"" "
            Dim l_strSQLWhere As String = " WHERE ""Name"" LIKE '%{0}%' "
            Dim l_StrOrder As String = " Order By ""Name"" "

            If pVal.ActionSuccess Then
                oMatrixTmp = DirectCast(oForm.Items.Item("mtxArtic").Specific, SAPbouiCOM.Matrix)
                strNombreMarca = DirectCast(oForm.Items.Item("txtBuscar").Specific, SAPbouiCOM.EditText).Value.Trim

                oForm.Freeze(True)

                If Not String.IsNullOrEmpty(strNombreMarca) Then

                    l_strSQL = l_strSQL & String.Format(l_strSQLWhere, strNombreMarca)
                    l_strSQL = l_strSQL + l_StrOrder

                    CargarMatriz(oMatrixTmp, oForm, l_strSQL)
                Else
                    l_strSQL = l_strSQL + l_StrOrder
                    CargarMatriz(oMatrixTmp, oForm, l_strSQL)
                End If

                oForm.Freeze(False)

            End If

           
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub


End Class
