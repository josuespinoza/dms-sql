Imports SAPbouiCOM
Imports System

Partial Public Class ListaPreciosSeleccion

    Dim oMatrixTmp As SAPbouiCOM.Matrix
    Dim otmpForm As SAPbouiCOM.Form

    Public Sub CargaFormListaPrecios()

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim strConsulta As String = ""

        Try
            'Parámetros del formulario
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_VSLP"
            StrFormType = fcp.FormType

            'XML a cargar
            strXMLACargar = My.Resources.Resource.XMLFomularioListaPrecios
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = m_SBO_Application.Forms.AddEx(fcp)
            oForm.Mode = BoFormMode.fm_OK_MODE

            'Asociación de la matrix al formulario
            Call LinkMatriz()

            oMatrix = DirectCast(oForm.Items.Item("mtxLisPre").Specific, SAPbouiCOM.Matrix)
            dtLisPre = oForm.DataSources.DataTables.Item("dtLisPre")


            strConsulta = "SELECT ""ListNum"" as ""Code"", ""ListName"" as ""Name"" FROM ""OPLN""  "

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

        dtLisPre = oForm.DataSources.DataTables.Add("dtLisPre")
        dtLisPre.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtLisPre.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizColores = New MatrixNumeracionSeries("mtxLisPre", oForm, "dtLisPre")
        MatrizColores.CreaColumnas()
        MatrizColores.LigaColumnas()

    End Sub

    Public Function CargarMatriz(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                       ByVal oform As SAPbouiCOM.Form, _
                                       ByVal Consulta As String) As Boolean

        Dim strConsulta As String = ""
        strConsulta = Consulta

        Try
            oMatrix.Clear()
            dtLisPre.Clear()
            If Not String.IsNullOrEmpty(strConsulta) Then
                dtLisPre.ExecuteQuery(strConsulta)
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
                        ButtonBuscarListaPreciosItemPressed(FormUID, pval, BubbleEvent)
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
            Dim oMatrizListaPrecios As SAPbouiCOM.Matrix
            oMatrizListaPrecios = DirectCast(oForm.Items.Item("mtxLisPre").Specific, SAPbouiCOM.Matrix)

            If Not pVal.FormTypeEx = "SCGD_VSLP" Then Return

            If pVal.FormTypeEx = "SCGD_VSLP" Then

                If pVal.BeforeAction Then
                    If pVal.ColUID = "V_-1" Then
                        BubbleEvent = False
                        oForm.Freeze(True)
                        If pVal.ItemUID = "mtxLisPre" Then
                            If oMatrizListaPrecios.RowCount > 0 Then
                                For i As Integer = 1 To oMatrizListaPrecios.RowCount
                                    If oMatrizListaPrecios.IsRowSelected(i) Then

                                        Dim strLPCode As String = dtLisPre.GetValue("Code", i - 1).ToString()
                                        Dim strLPName As String = dtLisPre.GetValue("Name", i - 1).ToString()

                                        _formConfiguracion.DataSources.DBDataSources.Item(mc_strConfSuc).SetValue("U_ListaPrecios", 0, strLPName)
                                        _formConfiguracion.DataSources.DBDataSources.Item(mc_strConfSuc).SetValue("U_CodLisPre", 0, strLPCode)

                                        If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                            _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                        End If

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

            Dim oMatrizColoresS As SAPbouiCOM.Matrix
            oMatrizColoresS = DirectCast(oForm.Items.Item("mtxLisPre").Specific, SAPbouiCOM.Matrix)

            If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
                If oMatrizColoresS.RowCount = 0 Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarColor, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                End If

            ElseIf pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then
                If oMatrizColoresS.RowCount > 0 Then
                    For i As Integer = 1 To oMatrizColoresS.RowCount
                        If oMatrizColoresS.IsRowSelected(i) Then

                            Dim strLPCode As String = dtLisPre.GetValue("Code", i - 1).ToString()
                            Dim strLPName As String = dtLisPre.GetValue("Name", i - 1).ToString()

                            _formConfiguracion.DataSources.DBDataSources.Item(mc_strConfSuc).SetValue("U_ListaPrecios", 0, strLPName)
                            _formConfiguracion.DataSources.DBDataSources.Item(mc_strConfSuc).SetValue("U_CodLisPre", 0, strLPCode)

                            'Dim strLPCode As String = dtLisPre.GetValue("Code", i - 1).ToString()
                            '_formConfiguracion.DataSources.DBDataSources.Item(mc_strConfSuc).SetValue("U_ListaPrecios", 0, strLPCode)
                            
                            If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                            End If

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


    Public Sub ButtonBuscarListaPreciosItemPressed(ByVal FOrmuUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Dim strValorBuscar As String
            Dim strSQL As String = " SELECT ""ListNum"" as ""Code"", ""ListName"" as ""Name"" FROM ""OPLN"" "
            Dim strSQLWhere As String = " WHERE ""ListName"" like '%{0}%' "

            If pVal.BeforeAction Then
                strValorBuscar = DirectCast(oForm.Items.Item("txtBuscar").Specific, SAPbouiCOM.EditText).Value.Trim
                oMatrixTmp = DirectCast(oForm.Items.Item("mtxLisPre").Specific, SAPbouiCOM.Matrix)

                oForm.Freeze(True)

                If Not String.IsNullOrEmpty(strValorBuscar) Then
                    strSQL = strSQL & String.Format(strSQLWhere, strValorBuscar)
                    CargarMatriz(oMatrixTmp, oForm, strSQL)
                Else
                    CargarMatriz(oMatrixTmp, oForm, strSQL)
                End If
                oForm.Freeze(False)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

End Class
