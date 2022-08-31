Imports SAPbouiCOM
Imports System
Imports System.Collections.Generic
Imports System.Globalization

Partial Public Class SeleccionLineasRecepcion

    Public Property MOCosteoDeEntradas As CosteoDeEntradas

    Private Sub CargarMatriz(ByVal p_strCodProveedor As String)
        Try
            Dim l_strPedido As String
            Dim l_strRecepcion As String
            Dim l_strUnidad As String
            Dim l_strCaracter As String

            l_strPedido = DirectCast(_formularioSBO.Items.Item("txtPedi").Specific, SAPbouiCOM.EditText).Value.Trim()
            l_strRecepcion = DirectCast(_formularioSBO.Items.Item("txtRece").Specific, SAPbouiCOM.EditText).Value.Trim()
            l_strUnidad = DirectCast(_formularioSBO.Items.Item("txtUnid").Specific, SAPbouiCOM.EditText).Value.Trim()

            _formularioSBO.Freeze(True)

            oMatrix = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            Dim strConsulta As String =
                 " Select EU.U_Num_Ped pedi, EU.DocEntry rece, EU.U_Cod_uni unid , MAR.Name marc, EST.Name esti, MODE.U_Descripcion mode, EU.U_Num_Vin vin, EU.U_Num_Mot moto, EU.U_Cod_Tip tipo, EU.U_ID_Veh code, " &
                 " EU.U_Cod_Mar cMar, EU.U_Cod_Est cEst, EU.U_Cod_Mod cMod, EU.U_Ano_Veh ano, EU.U_Line_Ref line, EU.U_Cod_Art arti, EU.U_Cod_Col col " &
                     " from " &
                     " [@SCGD_ENTRADA_UNID] EU with (nolock) " &
                     " inner join [@SCGD_ENTRADA_VEH] EV with(nolock) on EU.DocEntry = EV.DocEntry" &
                     " left outer join [@SCGD_MARCA] MAR with (nolock) on MAR.Code = EU.U_Cod_Mar" &
                     " left outer join [@SCGD_ESTILO] EST with (nolock) on EST.Code = EU.U_Cod_Est" &
                     " left outer join [@SCGD_MODELO] MODE with (nolock) on MODE.Code = EU.U_Cod_Mod" &
                     " left outer join [@SCGD_TIPOVEHICULO] TV with (nolock) on TV.Code = EU.U_Cod_Tip" &
                     " where(EU.U_Cod_Uni Is Not null) " &
                     " and EU.U_Cod_Uni <> '' " &
                     " and EV.Status = 'O' "

            '', EU.U_Line_Ref line, EU.U_Cod_Art arti " &
            ' strConsulta = String.Format(strConsulta, l_strCodDevueltos)
            If String.IsNullOrEmpty(p_strCodProveedor) Then
                If Not String.IsNullOrEmpty(l_strPedido) Then
                    strConsulta = strConsulta & String.Format(" AND EU.U_Num_Ped = '{0}' ", l_strPedido)
                End If
                If Not String.IsNullOrEmpty(l_strRecepcion) Then
                    strConsulta = strConsulta & String.Format(" AND EU.DocEntry = '{0}'", l_strRecepcion)
                End If
                If Not String.IsNullOrEmpty(l_strUnidad) Then

                    If l_strUnidad.Length > 1 Then

                        l_strCaracter = l_strUnidad.Substring(0, 1)

                        If l_strCaracter.Equals("*") Then
                            l_strUnidad = l_strUnidad.Replace("*", "")

                            strConsulta = strConsulta & String.Format(" AND EU.U_Cod_Uni like '%{0}%'", l_strUnidad)
                        Else
                            strConsulta = strConsulta & String.Format(" AND EU.U_Cod_Uni = '{0}'", l_strUnidad)
                        End If
                    Else
                        strConsulta = strConsulta & String.Format(" AND EU.U_Cod_Uni = '{0}'", l_strUnidad)
                    End If
                End If
            Else
                strConsulta = strConsulta & String.Format(" AND EV.U_Cod_Prov = '{0}' ", p_strCodProveedor)
            End If

            oMatrix.Clear()

            dtVehiculos = _formularioSBO.DataSources.DataTables.Item("dtVehiculos")
            dtVehiculos.Rows.Clear()

            If Not String.IsNullOrEmpty(strConsulta) Then
                dtVehiculos.ExecuteQuery(strConsulta)
            End If

            oMatrix.LoadFromDataSource()
            _formularioSBO.Freeze(False)

        Catch ex As Exception
            _formularioSBO.Freeze(False)
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Private Sub SeleccionarVehiculos(ByRef BubbleEvent As Boolean)
        Try

            Dim oMat As SAPbouiCOM.Matrix
            Dim intSelect As Integer
            Dim l_intPosicion As Integer
            Dim l_intTamano As Integer

            oMat = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            dtSeleccionados = _formularioSBO.DataSources.DataTables.Item("dtSeleccionados")
            dtSeleccionados.Rows.Clear()
            oMat.FlushToDataSource()
            dtVehiculos = _formularioSBO.DataSources.DataTables.Item("dtVehiculos")

            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Do While intSelect > -1

                l_intPosicion = intSelect

                l_intTamano = dtSeleccionados.Rows.Count

                dtSeleccionados.Rows.Add(1)

                dtSeleccionados.SetValue("pedi", l_intTamano, dtVehiculos.GetValue("pedi", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("rece", l_intTamano, dtVehiculos.GetValue("rece", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("unid", l_intTamano, dtVehiculos.GetValue("unid", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("marc", l_intTamano, dtVehiculos.GetValue("marc", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("esti", l_intTamano, dtVehiculos.GetValue("esti", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("mode", l_intTamano, dtVehiculos.GetValue("mode", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("vin", l_intTamano, dtVehiculos.GetValue("vin", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("moto", l_intTamano, dtVehiculos.GetValue("moto", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("tipo", l_intTamano, dtVehiculos.GetValue("tipo", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("code", l_intTamano, dtVehiculos.GetValue("code", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cMar", l_intTamano, dtVehiculos.GetValue("cMar", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cEst", l_intTamano, dtVehiculos.GetValue("cEst", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("cMod", l_intTamano, dtVehiculos.GetValue("cMod", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("ano", l_intTamano, dtVehiculos.GetValue("ano", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("line", l_intTamano, dtVehiculos.GetValue("line", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("arti", l_intTamano, dtVehiculos.GetValue("arti", intSelect - 1).ToString.Trim)
           
                dtSeleccionados.SetValue("col", l_intTamano, dtVehiculos.GetValue("col", intSelect - 1).ToString.Trim)
                ' dtSeleccionados.SetValue("line", l_intTamano, l_intPosicion)
                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Loop

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
            BubbleEvent = False
        End Try
    End Sub

    Private Sub ButtonAceptarItemPressed(ByVal FormUID As String, ByVal pval As ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pval.BeforeAction Then
                SeleccionarVehiculos(BubbleEvent)
            ElseIf pval.ActionSuccess Then

                If (BubbleEvent) Then
                    MOCosteoDeEntradas.AgregarVehiculosSeleccionados(dtSeleccionados)
                    'm_oCosteoDeEntradas.
                    FormularioSBO.Close()
                End If
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub
End Class
