Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO

Partial Public Class NumeracionSeries

    Public Sub CargaFormularioSeries()

        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim strConsulta As String = ""
        
        Try
            'Parámetros del formulario
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_NSE"
            StrFormType = fcp.FormType

            'XML a cargar
            strXMLACargar = My.Resources.Resource.XMLFormularioNumeracionSeries
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = m_SBO_Application.Forms.AddEx(fcp)
            oForm.Mode = BoFormMode.fm_OK_MODE

            'Asociación de la matrix al formulario
            Call LinkMatriz()

            oMatrix = DirectCast(oForm.Items.Item("mtxSerie").Specific, SAPbouiCOM.Matrix)
            dtNumeracion = oForm.DataSources.DataTables.Item("numSeries")

            Select Case IntTipoConfiguracion

                Case TipoConfiguracionSerie.OrdenVenta
                    strConsulta = "Select ""Series"", ""SeriesName"" From ""NNM1"" where ""ObjectCode"" = '17'"

                Case TipoConfiguracionSerie.OrdenCompra
                    strConsulta = "Select ""Series"", ""SeriesName"" From ""NNM1"" where ""ObjectCode"" = '22'"

                Case TipoConfiguracionSerie.OfertaVenta
                    strConsulta = "Select ""Series"", ""SeriesName"" From ""NNM1"" where ""ObjectCode"" = '23'"

                Case TipoConfiguracionSerie.OfertaCompra
                    strConsulta = "Select ""Series"", ""SeriesName"" From ""NNM1"" where ""ObjectCode"" = '540000006'"

                Case TipoConfiguracionSerie.InvBodega
                    strConsulta = "Select ""Series"", ""SeriesName"" From ""NNM1"" where ""ObjectCode"" = '67'"

            End Select


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

        dtNumeracion = oForm.DataSources.DataTables.Add("numSeries")
        dtNumeracion.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtNumeracion.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizNumeracion = New MatrixNumeracionSeries("mtxSerie", oForm, "numSeries")
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
            dtNumeracion.Clear()
            If Not String.IsNullOrEmpty(strConsulta) Then
                dtNumeracion.ExecuteQuery(strConsulta)
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

                End Select

            End If



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ButtonAgregarItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrizNumeracionS As SAPbouiCOM.Matrix
            oMatrizNumeracionS = DirectCast(oForm.Items.Item("mtxSerie").Specific, SAPbouiCOM.Matrix)

            If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
                If oMatrizNumeracionS.RowCount = 0 Then
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarNumeracion, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                End If

            ElseIf pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

                If oMatrizNumeracionS.RowCount > 0 Then

                    For i As Integer = 1 To oMatrizNumeracionS.RowCount

                        If oMatrizNumeracionS.IsRowSelected(i) Then

                            Dim strserieCode As String = dtNumeracion.GetValue("Series", i - 1).ToString()
                            Dim strSerieName As String = dtNumeracion.GetValue("SeriesName", i - 1).ToString()

                            Select Case IntTipoConfiguracion

                                Case TipoConfiguracionSerie.OrdenVenta
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_SerOrV", 0, strserieCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_DesSOrV", 0, strSerieName)

                                    If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                        _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                    End If

                                Case TipoConfiguracionSerie.OrdenCompra
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_SerOrC", 0, strserieCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_DesSOrC", 0, strSerieName)
                                    If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                        _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                    End If

                                Case TipoConfiguracionSerie.OfertaVenta
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_SerOfV", 0, strserieCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_DesSOfV", 0, strSerieName)
                                    If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                        _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                    End If

                                Case TipoConfiguracionSerie.OfertaCompra
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_SerOfC", 0, strserieCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_DesSOfC", 0, strSerieName)
                                    If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                        _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                    End If

                                Case TipoConfiguracionSerie.InvBodega
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_SerInv", 0, strserieCode)
                                    _formConfiguracion.DataSources.DBDataSources.Item("@SCGD_CONF_SUCURSAL").SetValue("U_DesSInv", 0, strSerieName)
                                    If Not _formConfiguracion.Mode = BoFormMode.fm_ADD_MODE Then
                                        _formConfiguracion.Mode = BoFormMode.fm_UPDATE_MODE
                                    End If

                            End Select

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

End Class
