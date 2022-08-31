Imports SAPbouiCOM
Imports System
Imports System.Globalization
Imports SCG.SBOFramework

Partial Class SeleccionLineasPedidos

    Private Sub CargarMatriz()
        Try
            Dim l_strPedido As String

            l_strPedido = DirectCast(_formularioSBO.Items.Item("txtPedido").Specific, SAPbouiCOM.EditText).Value.Trim()

            _formularioSBO.Freeze(True)

            oMatrix = DirectCast(_formularioSBO.Items.Item("mtxPed").Specific, SAPbouiCOM.Matrix)

            Dim strConsulta =
                " Select pe.DocEntry pedi, pl.U_Cod_Art cart, pl.U_Desc_art arti, pl.U_Ano_Veh ano,co.Name colo, pl.U_Cant cant, pl.U_Pen_Rec pend, pe.U_Cod_Prov cpro,pe.U_Name_Prov prov, PL.U_Cost_Art mont, CO.Code codCol, PL.LineId line, pe.U_DocCurr curr " &
                    " from " &
                    " [@SCGD_PEDIDOS_LINEAS] PL with(nolock) " &
                    " inner join [@SCGD_PEDIDOS] PE with(nolock) on Pe.DocEntry = PL.DocEntry " &
                    " left outer join [@SCGD_COLOR] CO with (nolock) on Pl.U_Cod_Col = Co.Code " &
                    " where (PE.Status = 'O' " &
                    " AND PL.U_Cant - PL.U_Cant_Rec > 0 )"

            ' strConsulta = String.Format(strConsulta, l_strCodDevueltos)

            If Not String.IsNullOrEmpty(l_strPedido) Then
                strConsulta = strConsulta & String.Format(" AND pe.DocEntry = '{0}' ", l_strPedido)
            End If
            If Not String.IsNullOrEmpty(_codProv) Then
                strConsulta = strConsulta & String.Format(" AND pe.U_Cod_Prov = '{0}'", _codProv)
            End If

            oMatrix.Clear()
            dtPedidos = _formularioSBO.DataSources.DataTables.Item("dtPedidos")
            dtPedidos.Rows.Clear()

            If Not String.IsNullOrEmpty(strConsulta) Then
                dtPedidos.ExecuteQuery(strConsulta)
            End If

            oMatrix.LoadFromDataSource()
            _formularioSBO.Freeze(False)

        Catch ex As Exception
            _formularioSBO.Freeze(False)
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Private Function SeleccionarLineasPedidos() As Boolean
        Try
            SeleccionarLineasPedidos = True
            Dim oMat As SAPbouiCOM.Matrix
            Dim intSelect As Integer
            Dim l_intPosicion As Integer
            Dim l_intTamano As Integer
            Dim strMonedaPedido As String
            Dim strMonedaEntrada As String
            Dim strMonedaPrimerPedido As String
            Dim strMonedaOtrosPedidos As String
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String
            Dim strTipoCambioEntrada As String
            Dim strFecha As String
            Dim oFormEntrada As SAPbouiCOM.Form
            Dim boolMonedaValida = True

            strMonedaPrimerPedido = String.Empty
            _monedaPedido = String.Empty

            oFormEntrada = _applicationSBO.Forms.Item("SCGD_EDV")
            n = DIHelper.GetNumberFormatInfo(_companySBO)
            oMat = DirectCast(_formularioSBO.Items.Item("mtxPed").Specific, SAPbouiCOM.Matrix)

            dtPedidos = _formularioSBO.DataSources.DataTables.Item("dtPedidos")
            dtPedidos.Rows.Clear()


            dtSeleccionados = _formularioSBO.DataSources.DataTables.Item("dtSeleccion")
            'dtSeleccionados.Rows.Clear()
            strMonedaEntrada = oFormEntrada.DataSources.DBDataSources.Item("@SCGD_ENTRADA_VEH").GetValue("U_Moneda", 0).Trim()




            strFecha = oFormEntrada.DataSources.DBDataSources.Item("@SCGD_ENTRADA_VEH").GetValue("U_Fha_Doc", 0).Trim()
            strFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", CultureInfo.InvariantCulture)
            oMat.FlushToDataSource()

            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            If intSelect > -1 Then
                strMonedaPrimerPedido = dtPedidos.GetValue("curr", intSelect - 1).ToString.Trim
            End If

            Do While intSelect > -1

                l_intPosicion = intSelect
                l_intTamano = dtSeleccionados.Rows.Count

                dtSeleccionados.Rows.Add(1)
                dtSeleccionados.SetValue("pedi", l_intTamano, dtPedidos.GetValue("pedi", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cart", l_intTamano, dtPedidos.GetValue("cart", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("arti", l_intTamano, dtPedidos.GetValue("arti", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("ano", l_intTamano, dtPedidos.GetValue("ano", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("colo", l_intTamano, dtPedidos.GetValue("codCol", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cant", l_intTamano, dtPedidos.GetValue("cant", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("pend", l_intTamano, dtPedidos.GetValue("pend", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cpro", l_intTamano, dtPedidos.GetValue("prov", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("prov", l_intTamano, dtPedidos.GetValue("prov", intSelect - 1).ToString.Trim)

                strMonedaPedido = dtPedidos.GetValue("curr", intSelect - 1).ToString.Trim
                strMonedaOtrosPedidos = dtPedidos.GetValue("curr", intSelect - 1).ToString.Trim

                If (strMonedaPedido = strMonedaEntrada) Then
                    dtSeleccionados.SetValue("mont", l_intTamano, dtPedidos.GetValue("mont", intSelect - 1))
                Else
                    DMS_Connector.Helpers.GetCurrencies(strMonedaLocal, strMonedaSistema)

                    strTipoCambioEntrada = oFormEntrada.DataSources.DBDataSources.Item("@SCGD_ENTRADA_VEH").GetValue("U_TipoCambio", 0)
                    strTipoCambioEntrada = Utilitarios.ConvierteDecimal(strTipoCambioEntrada, n)
                    Dim dcPrecio = dtPedidos.GetValue("mont", intSelect - 1)
                    strFecha = Utilitarios.RetornaFechaFormatoRegional(strFecha)
                    Dim strPrecioxTipoMoneda As Decimal = Utilitarios.ManejoMultimoneda(dcPrecio, strMonedaLocal, strMonedaSistema, strMonedaPedido, strMonedaEntrada, strTipoCambioEntrada, Date.Parse(strFecha), n, _companySBO)
                    dtSeleccionados.SetValue("mont", l_intTamano, Convert.ToDouble(strPrecioxTipoMoneda))
                End If

                dtSeleccionados.SetValue("line", l_intTamano, dtPedidos.GetValue("line", intSelect - 1))


                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)

                If boolMonedaValida = True Then
                    boolMonedaValida = ValidarMoneda(String.Empty, strMonedaPedido, strMonedaPrimerPedido, strMonedaOtrosPedidos)
                End If

            Loop

            SeleccionarLineasPedidos = boolMonedaValida

            If boolMonedaValida And Not String.IsNullOrEmpty(strMonedaPrimerPedido) Then
                _monedaPedido = strMonedaPrimerPedido
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Function

    Private Function ValidarMoneda(ByVal strCodProveedor As String, ByVal strMonedaPedido As String, ByVal strMonedaPrimerPedido As String, ByVal strMonedaOtrosPedido As String) As Boolean
        Try
            ValidarMoneda = True
            If strMonedaPrimerPedido = strMonedaOtrosPedido Then
                ValidarMoneda = True
            Else
                ValidarMoneda = False
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub
    Private Sub AsignaChooseFromList(ByVal p_strUId As String, ByVal p_strCFLId As String, ByVal p_strAlias As String)
        Try

            Dim oitem As SAPbouiCOM.Item
            Dim oText As SAPbouiCOM.EditText

            oitem = _formularioSBO.Items.Item(p_strUId)
            oText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)

            oText.ChooseFromListUID = p_strCFLId
            '  oText.ChooseFromListAlias = p_strAlias

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try


    End Sub


End Class
