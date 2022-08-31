Imports SAPbouiCOM

Partial Public Class SeleccionarGastosCostosOT

    Public Sub CargarMatrizGastos(ByVal oForm As Form, ByVal FormUID As String)
        Dim strConsulta As String = ""
        Dim oMatrizTodos As SAPbouiCOM.Matrix

        Dim strFiltroDescripcion As String = "  and oi.ItemName like '{0}%' "
        Dim strFiltroCodigo As String = "       and oi.ItemCode like '{0}%' "

        Dim blnCambios As Boolean

        Dim dtLocal As DataTable

        Dim dcPrecio As Decimal

        Try
            blnCambios = False
            strConsulta = "select   oi.ItemCode, ItemName , it.Price, it.Currency " +
            " from OITM  oi " +
            " left join ITM1 it on oi.ItemCode = it.ItemCode " +
            " where(oi.U_SCGD_TipoArticulo = 11 ) " +
            " Group by oi.ItemCode, ItemName , it.Price, it.Currency"
            '" and		it.PriceList in " +
            '" (		                        select ListNum from OCRD where CardCode = '{0}' ) "

            If Not String.IsNullOrEmpty(txtCod.ObtieneValorUserDataSource) Then
                strConsulta += String.Format(strFiltroCodigo, txtCod.ObtieneValorUserDataSource())
                blnCambios = True
            End If
            If Not String.IsNullOrEmpty(txtDes.ObtieneValorUserDataSource) Then
                strConsulta += String.Format(strFiltroDescripcion, txtDes.ObtieneValorUserDataSource())
                blnCambios = True
            End If

            dtGastosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)

            ' If blnCambios Then

            oForm.Freeze(True)

            dtLocal = oForm.DataSources.DataTables.Item("local")
            strConsulta = String.Format(strConsulta, IncluirRepuestosOT.CodeCliente)
            dtLocal.ExecuteQuery(strConsulta)

            dtGastosTodos.Rows.Clear()


            For i As Integer = 0 To dtLocal.Rows.Count - 1
                dtGastosTodos.Rows.Add(1)

                If Not String.IsNullOrEmpty(dtLocal.GetValue("Price", i)) Then
                    dcPrecio = Decimal.Parse(dtLocal.GetValue("Price", i))
                Else
                    dcPrecio = 0
                End If
                dtGastosTodos.SetValue("cod", i, dtLocal.GetValue("ItemCode", i))
                dtGastosTodos.SetValue("des", i, dtLocal.GetValue("ItemName", i))
                ' dtGastosTodos.SetValue("can", i, dcCantidad.ToString(n))
                dtGastosTodos.SetValue("pre", i, dcPrecio.ToString(n))
                'dtGastosTodos.SetValue("mon", i, dtLocal.GetValue("Currency", i))
            Next

            oMatrizTodos = DirectCast(oForm.Items.Item(strMatrizGasTodos).Specific, SAPbouiCOM.Matrix)
            oMatrizTodos.LoadFromDataSource()

            oForm.Freeze(False)
            'ElseIf Not blnCambios Then
            'Dim hora As String
            '' CargaRepuestos(dtRepuestosTodos, True, FormUID)

            'End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub
    
    Public Sub AgregaGastosCotizacion(ByVal FormUID As String, ByVal Validacion As Boolean, ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim objIncluirGastosOT As New IncluirGastosCostosOT(ApplicationSBO, CompanySBO, CatchingEvents.strMenuIncluirGastosOT)

        Try

            oForm = ApplicationSBO.Forms.Item(FormUID)
            oMatrix = DirectCast(oForm.Items.Item(strMatrizGasTodos).Specific, Matrix)

            oMatrix.FlushToDataSource()

            dtGastosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)

            objIncluirGastosOT.IncluirGastosSeleccionados(dtGastosTodos, Validacion, BubbleEvent)

            If Not Validacion Then oForm.Close()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' Ordena la lista de mayor a menor para eliminarlos de la matriz de repustos seleccionados

    Public Shared Sub OrdenaLista(ByVal lsListaEliminar As Generic.IList(Of Integer), ByRef lsListaOrdenada As Generic.IList(Of Integer))

        Dim posicion As Integer
        Dim ExisteUnMayor As Boolean
        Dim ValorIngresar As Integer

        posicion = 0
        ExisteUnMayor = False
        ValorIngresar = 0

        If lsListaEliminar.Count = 0 Then
            'Return lsListaOrdenada
            Exit Sub
        End If

        For Each val1 As Integer In lsListaEliminar
            ValorIngresar = val1
            For Each val2 As Integer In lsListaEliminar
                If val2 > val1 Then
                    ExisteUnMayor = True
                    Exit For
                End If
            Next
            posicion += 1
            If Not ExisteUnMayor Then
                lsListaOrdenada.Add(ValorIngresar)
                lsListaEliminar.RemoveAt(posicion - 1)
                OrdenaLista(lsListaEliminar, lsListaOrdenada)
                Exit For
            Else
                ExisteUnMayor = False
            End If
        Next

    End Sub

    Private Sub EjecutarFiltros(ByVal oForm As Form, ByVal FormUID As String)

        Dim strConsulta As String = ""
        Dim oMatrizTodos As SAPbouiCOM.Matrix

        Dim strFiltroDescripcion As String = "  and oi.ItemName like '{0}%' "
        Dim strFiltroCodigo As String = "       and oi.ItemCode like '{0}%' "

        Dim blnCambios As Boolean

        Dim dtLocal As DataTable

        Dim dcCantidad As Decimal
        Dim dcPrecio As Decimal

        Try
            blnCambios = False
            strConsulta = "select '' as sel, oi.ItemCode, ItemName , 1 as cantidad, it.Price, it.Currency " +
            " from OITM  oi " +
            " Left join ITM1 it on oi.ItemCode = it.ItemCode " +
            " where(oi.U_SCGD_TipoArticulo = 11 ) "
            '" and		it.PriceList in " +
            '" (		                        select ListNum from OCRD where CardCode = '{0}' ) "
  
 
            If Not String.IsNullOrEmpty(txtCod.ObtieneValorUserDataSource) Then
                strConsulta += String.Format(strFiltroCodigo, txtCod.ObtieneValorUserDataSource())
                blnCambios = True
            End If
            If Not String.IsNullOrEmpty(txtDes.ObtieneValorUserDataSource) Then
                strConsulta += String.Format(strFiltroDescripcion, txtDes.ObtieneValorUserDataSource())
                blnCambios = True
            End If

            dtGastos = oForm.DataSources.DataTables.Item(strDataTableTodos)

            '  If blnCambios Then

            oForm.Freeze(True)

            dtGastosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)
            dtLocal = oForm.DataSources.DataTables.Item("local")

            strConsulta = String.Format(strConsulta, IncluirRepuestosOT.CodeCliente)
            strConsulta = strConsulta + "Group by oi.ItemCode, ItemName , it.Price, it.Currency"

            dtLocal.ExecuteQuery(strConsulta)

            dtGastosTodos.Rows.Clear()


            For i As Integer = 0 To dtLocal.Rows.Count - 1
                dtGastosTodos.Rows.Add(1)

                If Not String.IsNullOrEmpty(dtLocal.GetValue("cantidad", i)) Then
                    dcCantidad = Decimal.Parse(dtLocal.GetValue("cantidad", i))
                Else
                    dcCantidad = 0
                End If
                If Not String.IsNullOrEmpty(dtLocal.GetValue("Price", i)) Then
                    dcPrecio = Decimal.Parse(dtLocal.GetValue("Price", i))
                Else
                    dcPrecio = 0
                End If

                dtGastosTodos.SetValue("cod", i, dtLocal.GetValue("ItemCode", i))
                dtGastosTodos.SetValue("des", i, dtLocal.GetValue("ItemName", i))
                ' dtGastosTodos.SetValue("can", i, dcCantidad.ToString(n))
                dtGastosTodos.SetValue("pre", i, dcPrecio.ToString(n))
                ' dtGastosTodos.SetValue("mon", i, dtLocal.GetValue("Currency", i))
            Next

            oMatrizTodos = DirectCast(oForm.Items.Item(strMatrizGasTodos).Specific, SAPbouiCOM.Matrix)
            oMatrizTodos.LoadFromDataSource()

            oForm.Freeze(False)
            'ElseIf Not blnCambios Then

            '    CargaRepuestos(dtRepuestosTodos, True, FormUID)

            ' End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub
End Class


