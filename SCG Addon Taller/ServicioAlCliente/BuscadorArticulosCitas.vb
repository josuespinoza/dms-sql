Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports SAPbobsCOM


Partial Public Class BuscadorArticulosCitas


    Public g_strDocEntry As String
    Public g_strCodListPrecio As String
    Private UDS_SeleccionaRepuestos As UserDataSources
    Private txtCode As EditTextSBO
    Private txtDescripcion As EditTextSBO
    Private txtCodeBar As EditTextSBO


    ''' <summary>
    ''' Carga Ventana
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoFormDataLoad()


        Dim m_strUsaListaPrecCliente As String
        Dim dtConf As System.Data.DataTable
        Dim dtEstiMod As System.Data.DataTable
        Dim m_strConsulta As String
        Dim m_strConsultaListaPreciosCliente As String = "Select ListNum from OCRD where CardCode = '{0}'"
        Dim m_objMatrix As Matrix
        Dim oitem As SAPbouiCOM.Item
        Dim m_strConsultaArtEsp As String
        Try
            If Not FormularioSBO Is Nothing Then

                UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
                UDS_SeleccionaRepuestos.Add("code", BoDataType.dt_LONG_TEXT, 100)
                UDS_SeleccionaRepuestos.Add("desc", BoDataType.dt_LONG_TEXT, 100)
                UDS_SeleccionaRepuestos.Add("CodBar", BoDataType.dt_LONG_TEXT, 100)


                txtCode = New EditTextSBO("txtCode", True, "", "code", FormularioSBO)
                txtCode.AsignaBinding()
                txtDescripcion = New EditTextSBO("txtDesc", True, "", "desc", FormularioSBO)
                txtDescripcion.AsignaBinding()
                txtCodeBar = New EditTextSBO("txtCodBar", True, "", "CodBar", FormularioSBO)
                txtCodeBar.AsignaBinding()


                FormularioSBO.Freeze(True)
                m_strConsulta = String.Format("Select DocEntry,U_CodLisPre,U_UseLisPreCli from [@SCGD_CONF_SUCURSAL] where U_Sucurs='{0}' ", idSucursal)
                dtConf = Utilitarios.EjecutarConsultaDataTable(m_strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                g_strDocEntry = dtConf.Rows(0)("DocEntry").ToString()
                m_strUsaListaPrecCliente = dtConf.Rows(0)("U_UseLisPreCli").ToString()

                If (m_strUsaListaPrecCliente.Equals("Y")) Then
                    g_strCodListPrecio = Utilitarios.EjecutarConsulta(String.Format(m_strConsultaListaPreciosCliente, strCodCliente), m_oCompany.CompanyDB, m_oCompany.Server)

                Else
                    g_strCodListPrecio = dtConf.Rows(0)("U_CodLisPre").ToString()
                End If

                oitem = FormularioSBO.Items.Item(g_strmtxAdicionales)
                m_objMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)
                m_objMatrix.FlushToDataSource()


                If (g_strUsaConfEstiMode = "N") Then
                    g_strConsultaArticulos = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)
                    g_dtAdicionales.ExecuteQuery(g_strConsultaArticulos)
                Else
                    dtEstiMod = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_Cod_Esti,U_Cod_Mode  from [@SCGD_VEHICULO] where DocEntry ={0}", g_CodVehi), m_oCompany.CompanyDB, m_oCompany.Server)

                    If (g_strFiltroEstiMod = "E") Then
                        g_strCodUsa = dtEstiMod.Rows(0)("U_Cod_Esti").ToString().Trim()
                        g_FiltroAUsar = " and art.[U_CodEsti] = '{0}'"
                    Else
                        g_strCodUsa = dtEstiMod.Rows(0)("U_Cod_Mode").ToString().Trim()
                        g_FiltroAUsar = " and art.[U_CodMod] = '{0}'"
                    End If
                    dtArtTab = Utilitarios.EjecutarConsultaDataTable(g_strConsultaExistenciaArticulos + String.Format(g_FiltroAUsar, g_strCodUsa), m_oCompany.CompanyDB, m_oCompany.Server)

                    If dtArtTab.Rows(0)("U_ItemCode").ToString() <> "0" Then

                        If (String.IsNullOrEmpty(g_strCodUsa) = False) Then
                            m_strConsultaArtEsp = String.Format(g_strConsultaTablaArtEsp, g_strCodListPrecio, g_strDocEntry, String.Format(g_FiltroAUsar, g_strCodUsa))
                            m_strConsultaArtEsp = m_strConsultaArtEsp + "Union" + String.Format(g_strConsultaServExternos, g_strCodListPrecio, g_strDocEntry, String.Format(g_FiltroAUsar, g_strCodUsa))
                            g_dtAdicionales.ExecuteQuery(m_strConsultaArtEsp)

                        Else
                            g_strConsultaArticulos = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)
                            g_dtAdicionales.ExecuteQuery(g_strConsultaArticulos)
                        End If
                    Else
                        g_strConsultaArticulos = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)
                        g_dtAdicionales.ExecuteQuery(g_strConsultaArticulos)
                    End If





                End If


                FormularioSBO.Freeze(False)

                m_objMatrix.LoadFromDataSource()

            End If



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub



    ''' <summary>
    ''' Manejador ItemEvent
    ''' </summary>
    ''' <param name="oform"></param>
    ''' <param name="p_val"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ApplicationSBOOnItemEvent(ByVal oform As String, ByVal p_val As ItemEvent, ByRef BubbleEvent As Boolean, ByRef p_FormCitas As CitasReservacion)

        Select Case p_val.EventType

            Case BoEventTypes.et_ITEM_PRESSED

                ManejadorEventosItemPressed(oform, p_val, BubbleEvent, p_FormCitas)
        End Select


    End Sub


    ''' <summary>
    ''' Manejador del Item Pressed
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pval"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, pval As ItemEvent, ByRef BubbleEvent As Boolean, ByRef p_CitasForm As CitasReservacion)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim dtAdicionales As SAPbouiCOM.DataTable
        Dim dtAdicionalesSeleccionados As SAPbouiCOM.DataTable
        Dim oForm As SAPbouiCOM.Form
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Dim oitem As SAPbouiCOM.Item

        Try
            If (String.IsNullOrEmpty(FormUID) = False) Then


                oForm = ApplicationSBO.Forms.Item(FormUID)
                If (pval.BeforeAction) Then

                Else
                    Select Case pval.ItemUID

                        Case "mtxArt"
                            oitem = FormularioSBO.Items.Item(g_strmtxAdicionales)
                            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)
                            oMatrix.FlushToDataSource()

                            If (pval.ColUID = "Col_Sel" And pval.Row > 0) Then
                                dtAdicionales = oForm.DataSources.DataTables.Item(g_strdtAdicionales)
                                dtAdicionalesSeleccionados = oForm.DataSources.DataTables.Item(g_strdtAdicionalesSeleccionados)

                                If (pval.Row - 1 <= dtAdicionales.Rows.Count - 1) Then
                                    ''oitem = FormularioSBO.Items.Item(pval.Row)
                                    oCheckBox = DirectCast(oMatrix.Columns.Item("Col_Sel").Cells.Item(pval.Row).Specific, SAPbouiCOM.CheckBox)

                                    If (oCheckBox.Checked) Then

                                        SeleccionarAdicionales(dtAdicionales, dtAdicionalesSeleccionados, pval.Row - 1)

                                    Else
                                        Dim m_strCode As String = dtAdicionales.GetValue("code", pval.Row - 1).ToString().Trim()
                                        EliminarAdicionalesDeDataBle(dtAdicionalesSeleccionados, m_strCode)

                                    End If

                                End If

                            End If

                        Case "btnAgre"
                            AgregarAdicionales(FormUID, p_CitasForm)
                        Case "btnBus"
                            BuscarAdicionales(oForm)
                        Case "btnCan"
                            oForm.Close()
                    End Select

                End If

            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Guardar Adicionales en Datatable
    ''' </summary>
    ''' <param name="p_dtAdicionales"></param>
    ''' <param name="p_dtSeleccionados"></param>
    ''' <param name="p_Position"></param>
    ''' <remarks></remarks>
    Private Sub SeleccionarAdicionales(ByRef p_dtAdicionales As SAPbouiCOM.DataTable, ByRef p_dtSeleccionados As SAPbouiCOM.DataTable, ByVal p_Position As Integer)
        Dim intTamano As Integer = p_dtSeleccionados.Rows.Count

        Dim Code As String = p_dtAdicionales.GetValue("code", p_Position).ToString().Trim()
        Dim Descripcion As String = p_dtAdicionales.GetValue("desc", p_Position).ToString().Trim()
        Dim Bodega As String = p_dtAdicionales.GetValue("bode", p_Position).ToString().Trim()
        Dim Precio As String = p_dtAdicionales.GetValue("prec", p_Position).ToString().Trim()
        Dim Cantidad As String = p_dtAdicionales.GetValue("cant", p_Position).ToString().Trim()
        Dim Moneda As String = p_dtAdicionales.GetValue("mone", p_Position).ToString().Trim()
        Dim Duracion As String = p_dtAdicionales.GetValue("dura", p_Position).ToString().Trim()
        Dim NoFase As String = p_dtAdicionales.GetValue("nofa", p_Position).ToString().Trim()
        '' Dim CodeBar As String = p_dtAdicionales.GetValue("CodBar", p_Position).ToString().Trim()

        p_dtSeleccionados.Rows.Add(1)
        p_dtSeleccionados.SetValue("Cod", intTamano, Code)
        p_dtSeleccionados.SetValue("Desc", intTamano, Descripcion)
        p_dtSeleccionados.SetValue("Bod", intTamano, Bodega)
        p_dtSeleccionados.SetValue("Prec", intTamano, Precio)
        p_dtSeleccionados.SetValue("Cant", intTamano, Cantidad)
        p_dtSeleccionados.SetValue("Mon", intTamano, Moneda)
        p_dtSeleccionados.SetValue("Dur", intTamano, Duracion)
        p_dtSeleccionados.SetValue("NoF", intTamano, NoFase)
        ''  p_dtSeleccionados.SetValue("CodBar", intTamano, NoFase)

    End Sub

    ''' <summary>
    ''' Elimina adicionales del datatable 
    ''' </summary>
    ''' <param name="dtAdicionales"></param>
    ''' <param name="p_strCode"></param>
    ''' <remarks></remarks>
    Private Sub EliminarAdicionalesDeDataBle(ByRef dtAdicionales As SAPbouiCOM.DataTable, ByVal p_strCode As String)

        For i As Integer = 0 To dtAdicionales.Rows.Count - 1
            If (dtAdicionales.GetValue("Cod", i).ToString().Trim() = p_strCode) Then
                dtAdicionales.Rows.Remove(i)
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Filtro de Adicionales
    ''' </summary>
    ''' <param name="p_oForm"></param>
    ''' <remarks></remarks>
    Private Sub BuscarAdicionales(ByVal p_oForm As Form)

        Dim m_strConsultaAdicionales As String = String.Empty
        Dim m_strConsultaServExternos As String = String.Empty
        Dim m_strConsultaArtEsp As String
        Dim m_strFiltroCode As String = " and oi.ItemCode like '{0}%' "
        Dim m_strFiltroDescription As String = " and oi.ItemName like '{0}%' "
        Dim m_strFiltroCodeBar As String = " and oi.CodeBars like '{0}%'"
        Dim m_strCode As String = String.Empty
        Dim m_strDescription As String = String.Empty
        Dim m_strCodeBar As String = String.Empty
        Dim m_blnCode As Boolean = False
        Dim m_blnDescription As Boolean = False
        Dim m_blnCodeBar As Boolean = False
        Dim m_objMatrix As SAPbouiCOM.Matrix
        Dim oitem As SAPbouiCOM.Item


        Try



            m_strCode = txtCode.ObtieneValorUserDataSource.ToString.Trim
            m_strDescription = txtDescripcion.ObtieneValorUserDataSource.ToString.Trim
            m_strCodeBar = txtCodeBar.ObtieneValorUserDataSource.ToString.Trim

            oitem = FormularioSBO.Items.Item(g_strmtxAdicionales)
            m_objMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            If (String.IsNullOrEmpty(m_strCode) = False) Then
                m_strFiltroCode = String.Format(m_strFiltroCode, m_strCode)
                m_blnCode = True
            End If


            If (String.IsNullOrEmpty(m_strDescription) = False) Then
                m_strFiltroDescription = String.Format(m_strFiltroDescription, m_strDescription)
                m_blnDescription = True
            End If


            If (String.IsNullOrEmpty(m_strCodeBar) = False) Then
                m_strFiltroCodeBar = String.Format(m_strFiltroCodeBar, m_strCodeBar)
                m_blnCodeBar = True
            End If


            If (g_strUsaConfEstiMode = "N") Then

                m_strConsultaAdicionales = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)

                If (m_blnCode) Then
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCode)
                End If
                If (m_blnDescription) Then
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroDescription)
                End If
                If (m_blnCodeBar) Then
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCodeBar)
                End If


                If (m_blnCode Or m_blnDescription Or m_blnCodeBar) Then
                    g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                    m_objMatrix.LoadFromDataSource()
                Else
                    g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                    m_objMatrix.LoadFromDataSource()
                End If
            Else
                If dtArtTab.Rows(0)("U_ItemCode").ToString() <> "0" Then
                    If (String.IsNullOrEmpty(g_strCodUsa) = False) Then
                        m_strConsultaServExternos = String.Format(g_strConsultaServExternos, g_strCodListPrecio, g_strDocEntry, String.Format(g_FiltroAUsar, g_strCodUsa))
                        m_strConsultaArtEsp = String.Format(g_strConsultaTablaArtEsp, g_strCodListPrecio, g_strDocEntry, String.Format(g_FiltroAUsar, g_strCodUsa))

                        If (m_blnCode) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaArtEsp, m_strFiltroCode) + "UNION" + String.Format(" {0} {1} ", m_strConsultaServExternos, m_strFiltroCode)
                        End If
                        If (m_blnDescription) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaArtEsp, m_strFiltroDescription) + "UNION" + String.Format(" {0} {1} ", m_strConsultaServExternos, m_strFiltroDescription)
                        End If
                        If (m_blnCodeBar) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaArtEsp, m_strFiltroCodeBar) + "UNION" + String.Format(" {0} {1} ", m_strConsultaServExternos, m_strFiltroCodeBar)
                        End If


                        If (m_blnCode Or m_blnDescription Or m_blnCodeBar) Then
                            g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                            m_objMatrix.LoadFromDataSource()
                        Else
                            m_strConsultaAdicionales = m_strConsultaArtEsp + "UNION" + m_strConsultaServExternos
                            g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                            m_objMatrix.LoadFromDataSource()
                        End If
                    Else
                        m_strConsultaAdicionales = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)

                        If (m_blnCode) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCode)
                        End If
                        If (m_blnDescription) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroDescription)
                        End If
                        If (m_blnCodeBar) Then
                            m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCodeBar)
                        End If


                        If (m_blnCode Or m_blnDescription Or m_blnCodeBar) Then
                            g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                            m_objMatrix.LoadFromDataSource()
                        Else
                            g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                            m_objMatrix.LoadFromDataSource()
                        End If
                    End If
                Else
                    m_strConsultaAdicionales = String.Format(g_strConsultaArticulos, g_strCodListPrecio, g_strDocEntry)

                    If (m_blnCode) Then
                        m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCode)
                    End If
                    If (m_blnDescription) Then
                        m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroDescription)
                    End If
                    If (m_blnCodeBar) Then
                        m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCodeBar)
                    End If


                    If (m_blnCode Or m_blnDescription Or m_blnCodeBar) Then
                        g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                        m_objMatrix.LoadFromDataSource()
                    Else
                        g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales)
                        m_objMatrix.LoadFromDataSource()
                    End If
                End If
            End If

          






        Catch ex As Exception

        End Try


    End Sub

    ''' <summary>
    ''' Agregar Adicionales
    ''' </summary>
    ''' <param name="oform"></param>
    ''' <remarks></remarks>
    Private Sub AgregarAdicionales(ByVal oformUID As String, ByRef p_FormCitas As CitasReservacion)
        Dim dtAdicionalesSeleccionados As SAPbouiCOM.DataTable
        Dim oItem As ItemEvent
        Dim oform As SAPbouiCOM.Form

        Try
            oform = ApplicationSBO.Forms.Item(oformUID)
            dtAdicionalesSeleccionados = oform.DataSources.DataTables.Item(g_strdtAdicionalesSeleccionados)
            p_FormCitas.AsignaValoresMatriz(oformUID, oItem, dtAdicionalesSeleccionados, True)
            p_FormCitas.CalculaTotales()
            p_FormCitas.CalculaFechaFinCita()
            p_FormCitas.CalculaTiempoDeServicio(True)
            p_FormCitas.CambiarModoActualizar()
            oform.Close()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


   




End Class
