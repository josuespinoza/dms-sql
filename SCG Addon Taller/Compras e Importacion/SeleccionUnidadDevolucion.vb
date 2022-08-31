Imports SAPbouiCOM
Imports System
Imports System.Globalization


Partial Public Class SeleccionUnidadDevolucion

#Region "Variables"

    Friend m_oDevolucionDeVehiculos As DevolucionDeVehiculos
    Dim n As NumberFormatInfo

#End Region

#Region "Metodos"

    Private Sub CargarMatriz()
        Try
            Dim l_strPedido As String
            Dim l_strRecepcion As String
            Dim l_strUnidad As String
            Dim l_strCaracter As String
            Dim l_strCodDevueltos As String

            l_strPedido = DirectCast(_formularioSBO.Items.Item("txtPedi").Specific, SAPbouiCOM.EditText).Value.Trim()
            l_strRecepcion = DirectCast(_formularioSBO.Items.Item("txtRece").Specific, SAPbouiCOM.EditText).Value.Trim()
            l_strUnidad = DirectCast(_formularioSBO.Items.Item("txtUnid").Specific, SAPbouiCOM.EditText).Value.Trim()
            l_strCodDevueltos = Utilitarios.EjecutarConsulta("SELECT U_Devol_Veh from [@SCGD_ADMIN] where code = 'DMS'", _companySBO.CompanyDB, _companySBO.Server)

            _formularioSBO.Freeze(True)

            oMatrix = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            Dim strConsulta = "Select  EV.DocEntry rece, EU.U_Num_Ped pedi, EU.U_Cod_Uni unid,VE.U_Des_Marc marc, ve.U_Des_Esti esti, ve.U_Des_Mode mode, ve.U_Num_VIN vin, VE.U_Num_Mot moto,TV.Code tipo," &
                                " EU.U_Monto_Gr mont, EV.U_Moneda mone, EV.U_TipoCambio rate , EU.U_Num_Asiento asie,'' stat, ve.Code code " &
                                " from [@SCGD_ENTRADA_UNID] EU with(nolock)" &
                                " join [@SCGD_ENTRADA_VEH] EV  with(nolock) on EU.DocEntry = EV.DocEntry" &
                                " left join [@SCGD_PEDIDOS] PE  with(nolock) on PE.DocEntry = EU.U_Num_Ped" &
                                " join [@SCGD_VEHICULO] VE with (nolock) on ve.Code = EU.U_ID_Veh" &
                                " left join [@SCGD_TIPOVEHICULO] TV with(nolock) on Ve.U_Tipo = TV.Code" &
                                " where " &
                                " PE.Canceled <> 'Y' " &
                                " AND EU.U_Num_Asiento is not null" &
                                " AND VE.U_Dispo <> '{0}' "

            strConsulta = String.Format(strConsulta, l_strCodDevueltos)

            If Not String.IsNullOrEmpty(l_strRecepcion) Then
                strConsulta = strConsulta & String.Format(" AND EV.DocEntry = '{0}' ", l_strRecepcion)
            End If
            If Not String.IsNullOrEmpty(l_strPedido) Then
                strConsulta = strConsulta & String.Format(" AND EU.U_Num_Ped = '{0}'", l_strPedido)
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

            strConsulta = strConsulta & " order by EV.DocEntry"

            oMatrix.Clear()

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

    Private Sub SeleccionarVehiculos()
        Try

            Dim oMat As SAPbouiCOM.Matrix
            Dim intSelect As Integer
            Dim l_strPedido As String
            Dim l_strRecepcion As String
            Dim l_strUnidad As String
            Dim l_intPosicion As Integer
            Dim l_intTamano As Integer
            Dim l_decMonto As Decimal
            Dim l_decTipoC As Decimal

            oMat = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            dtSeleccionados = _formularioSBO.DataSources.DataTables.Item("dtSeleccionados")
            dtSeleccionados.Rows.Clear()

            oMat.FlushToDataSource()

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
                l_decMonto = dtVehiculos.GetValue("mont", intSelect - 1)
                dtSeleccionados.SetValue("mont", l_intTamano, l_decMonto.ToString(n))
                dtSeleccionados.SetValue("mone", l_intTamano, dtVehiculos.GetValue("mone", intSelect - 1).ToString.Trim)
                l_decTipoC = dtVehiculos.GetValue("rate", intSelect - 1)
                dtSeleccionados.SetValue("rate", l_intTamano, l_decTipoC.ToString(n))
                dtSeleccionados.SetValue("asie", l_intTamano, dtVehiculos.GetValue("asie", intSelect - 1).ToString.Trim)
                dtSeleccionados.SetValue("code", l_intTamano, dtVehiculos.GetValue("code", intSelect - 1).ToString.Trim)

                dtSeleccionados.SetValue("line", l_intTamano, l_intPosicion)

                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Loop



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

#End Region

#Region "Eventos"

#End Region


    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                ByVal FormUID As String,
                                ByRef BubbleEvent As Boolean)
        Try
            If Not pval.FormTypeEx = "SCGD_SUD" Then Return

            If pval.EventType = BoEventTypes.et_ITEM_PRESSED Then

                Select Case pval.ItemUID
                    Case "btnAcep"
                        ButtonAceptarItemPressed(FormUID, pval, BubbleEvent)
                    Case "btnCanc"
                        ButtonCancelarColorItemPressed(FormUID, pval, BubbleEvent)
                    Case "btnActu"
                        ButtonActualizaItemPressed(FormUID, pval, BubbleEvent)
                    Case "mtxVeh"
                        'If pval.ColUID = "V_-1" Then


                End Select

            End If



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Private Sub ButtonAceptarItemPressed(ByVal FormUID As String, ByVal pval As ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pval.BeforeAction Then

            ElseIf pval.ActionSuccess Then

                m_oDevolucionDeVehiculos = New DevolucionDeVehiculos(_applicationSBO, _companySBO, CatchingEvents.mc_strDevolucionDeVehiculos)

                SeleccionarVehiculos()
                m_oDevolucionDeVehiculos.AgregarVehiculos(dtSeleccionados)
                FormularioSBO.Close()
            End If



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Private Sub ButtonCancelarColorItemPressed(ByVal FormUID As String, ByVal pval As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            _formularioSBO.Close()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Private Sub ButtonActualizaItemPressed(ByVal FormUID As String, ByVal pval As ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pval.ActionSuccess Then
                CargarMatriz()
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

End Class
