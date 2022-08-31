Imports SAPbouiCOM
Imports DMSOneFramework.SCGDataAccess
Imports SCG.DMSOne.Framework
Imports System.Globalization
Imports DMSOneFramework.SCGCommon
Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports SAPbobsCOM

Partial Public Class CosteoDeEntradas : Implements IUsaPermisos


#Region "Declaraciones"


    Dim m_strTablaPedidos As String = "@SCGD_COST_LIN"
    Dim m_StrTablaArticulos As String = "@SCGD_COST_ART"
    Dim n As NumberFormatInfo

    Dim m_blnCopiaMontoDePedido As Boolean = False

    Private m_strMonedaOrigen As String
    Private m_strMonedaDestino As String

    Private m_decTCOrigen As Decimal
    Private m_decTCDestino As Decimal

    Dim m_strActualizaTipoTrans As String

    Public m_strFacturaProv As String = String.Empty

    Public mc_strDocEntry As String
    'Public m_oFormularioCostoPorEntradas As CostosPorEntrada

    Enum mo_AsigTransaccion
        e_todas = 1
        e_sinAsing = 2
        e_Ninguna = 3
    End Enum


#End Region

#Region "Metodos Funciones"

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub AsignarCFLButton(ByVal p_strControl As String, ByVal p_strCFL As String)

        Try

            Dim oitem As SAPbouiCOM.Item
            Dim oButton As SAPbouiCOM.Button

            oitem = FormularioSBO.Items.Item(p_strControl)
            oButton = CType(oitem.Specific, SAPbouiCOM.Button)

            oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
            oButton.ChooseFromListUID = p_strCFL

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaCFLColumn(ByVal p_strMatriz As String, ByVal p_strColumn As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oitem = FormularioSBO.Items.Item(p_strMatriz)
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item(p_strColumn).ChooseFromListUID = p_strCFL
            oMatrix.Columns.Item(p_strColumn).ChooseFromListAlias = p_Alias
            '-----------------------------------------------
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Private Sub AsignarCFLText(ByVal p_strControl As String, ByVal p_strCFL As String, ByVal p_strAlias As String)

        Try

            Dim oitem As SAPbouiCOM.Item
            Dim oText As SAPbouiCOM.EditText

            oitem = FormularioSBO.Items.Item(p_strControl)
            oText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)

            'oText.Type = SAPbouiCOM.BoFieldsType.ft_AlphaNumeric
            oText.ChooseFromListUID = p_strCFL
            oText.ChooseFromListAlias = p_strAlias


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaValoresEncabezado(ByRef oDataTable As SAPbouiCOM.DataTable)
        Try
            Dim l_intCodEntrada As Integer
            Dim l_strSQL As String
            Dim numlinea As Integer
            Dim intNuevoRegisto As Integer = 0


            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            l_strSQL = "  SELECT EN.DocEntry, EN.U_Cod_Prov, EN.U_Name_Prov, EN.U_Contact, EN.U_Moneda, EN.U_TipoCambio " +
             " FROM [@SCGD_ENTRADA_VEH] EN " +
             " where EN.DocEntry = '{0}'"

            l_intCodEntrada = oDataTable.GetValue("DocEntry", 0)

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQL, l_intCodEntrada))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("DocEntry", 0)) Then

                'txtCodProv.AsignaValorDataSource(dtLocal.GetValue("U_Cod_Prov", 0))
                'txtNamProv.AsignaValorDataSource(dtLocal.GetValue("U_Name_Prov", 0))
                'cboContac.AsignaValorDataSource(dtLocal.GetValue("U_Contact", 0))
                cboMoneda.AsignaValorDataSource(dtLocal.GetValue("U_Moneda", 0))

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    Public Sub AsignaValoresPedidos(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Try
            Dim l_intCodEntrada As Integer
            Dim l_strSQL As String
            Dim numlinea As Integer
            Dim intNuevoRegisto As Integer = 0
            Dim l_decCostVehiculo As Decimal

            MatrixCostPedidos.Matrix.FlushToDataSource()

            dtPedidos = FormularioSBO.DataSources.DataTables.Item("dtPed")

            l_strSQL = "  SELECT EN.DocEntry,  EN.U_Cod_Prov,EN.U_Name_Prov,EN.U_Contact,EN.U_Moneda,EN.U_TipoCambio, LI.LineId, LI.U_Num_Ped ,LI.U_Cod_Art ,LI.U_Desc_Art ,LI.U_Ano_Veh ,LI.U_Cost_Veh ,LI.U_Cant_Ent ,LI.U_Cod_Col ,LI.U_Total_L, LI.U_Line_Ref"
            l_strSQL &= " FROM [@SCGD_ENTRADA_VEH] EN "
            l_strSQL &= " inner join [@SCGD_ENTRADA_LINEAS] LI on EN.DocEntry = LI.DocEntry"
            l_strSQL &= " where EN.DocEntry = '{0}'"

            l_intCodEntrada = oDataTable.GetValue("DocEntry", 0)

            dtPedidos.Clear()
            dtPedidos.ExecuteQuery(String.Format(l_strSQL, l_intCodEntrada))

            numlinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size
            intNuevoRegisto = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size

            If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", 0).Trim()) AndAlso
                 FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1 = 0 Then
                numlinea = 0
            End If

            If Not String.IsNullOrEmpty(dtPedidos.GetValue("DocEntry", 0)) Then

                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCosteoEntradaCargaMontos, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    m_blnCopiaMontoDePedido = True
                Else
                    m_blnCopiaMontoDePedido = False
                End If

                For i As Integer = 0 To dtPedidos.Rows.Count - 1
                    If Not String.IsNullOrEmpty(dtPedidos.GetValue("U_Cod_Art", i)) Then

                        l_decCostVehiculo = dtPedidos.GetValue("U_Cost_Veh", i)

                        If numlinea = 0 Then
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", 0, dtPedidos.GetValue("U_Num_Ped", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", 0, dtPedidos.GetValue("DocEntry", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", 0, dtPedidos.GetValue("U_Cod_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", 0, dtPedidos.GetValue("U_Desc_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", 0, dtPedidos.GetValue("U_Cod_Col", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", 0, dtPedidos.GetValue("U_Cant_Ent", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", 0, dtPedidos.GetValue("U_Ano_Veh", i))

                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", 0, dtPedidos.GetValue("U_Line_Ref", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", 0, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", 0, 0)

                            If m_blnCopiaMontoDePedido Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, l_decCostVehiculo.ToString(n))
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, 0)
                            End If

                            numlinea = numlinea + 1
                        Else
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).InsertRecord(intNuevoRegisto)
                            intNuevoRegisto += 1
                            ' g_LineaMatriz = numlinea + 1
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", intNuevoRegisto - 1, dtPedidos.GetValue("U_Num_Ped", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", intNuevoRegisto - 1, dtPedidos.GetValue("DocEntry", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cod_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", intNuevoRegisto - 1, dtPedidos.GetValue("U_Desc_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cod_Col", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cant_Ent", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", intNuevoRegisto - 1, dtPedidos.GetValue("U_Ano_Veh", i))

                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", intNuevoRegisto - 1, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", intNuevoRegisto - 1, 0)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", intNuevoRegisto - 1, dtPedidos.GetValue("U_Line_Ref", i))

                            If m_blnCopiaMontoDePedido Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", intNuevoRegisto - 1, l_decCostVehiculo.ToString(n))
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", intNuevoRegisto - 1, 0)
                            End If
                        End If
                    End If
                Next
            End If

            MatrixCostPedidos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaValoresVehiculos(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Try
            Dim l_intCodEntrada As Integer
            Dim l_strSQL As String
            Dim numLinea As Integer
            Dim intNuevoRegistro As Integer = 0
            Dim l_strCodUnid As String
            Dim l_StrNumCuenta As String

            dtVehiculos = FormularioSBO.DataSources.DataTables.Item("dtVeh")

            l_strSQL = " SELECT EN.DocEntry, UN.U_ID_Veh, UN.LineID ,UN.U_Cod_Uni ,UN.U_Cod_Mar ,UN.U_Cod_Est ,UN.U_Cod_Mod ,UN.U_Num_Vin, UN.U_Ano_Veh, UN.U_Cod_Art , U_Num_Ped, UN.U_Num_Mot, UN.U_Cod_Col,UN.U_Line_Ref, UN.U_Cod_Tip, VE.U_Tipo" +
            " FROM [@SCGD_ENTRADA_VEH] EN  with (nolock) " +
            " inner join [@SCGD_ENTRADA_UNID] UN  with (nolock) on EN.DocEntry = UN.DocEntry" +
            " Inner join [@SCGD_VEHICULO] VE  with (nolock) on VE.Code = UN.U_ID_Veh " +
            " where EN.DocEntry = '{0}'"

            l_intCodEntrada = oDataTable.GetValue("DocEntry", 0)

            dtVehiculos.Clear()
            dtVehiculos.ExecuteQuery(String.Format(l_strSQL, l_intCodEntrada))

            MatrixCostArticulos.Matrix.FlushToDataSource()

            numLinea = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
            intNuevoRegistro = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size

            If Not String.IsNullOrEmpty(dtVehiculos.GetValue("DocEntry", 0)) Then
                For i As Integer = 0 To dtVehiculos.Rows.Count - 1
                    If Not String.IsNullOrEmpty(dtVehiculos.GetValue("U_Cod_Uni", i)) Then

                        If numLinea = 0 Then
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_ID_Unid", 0, dtVehiculos.GetValue("U_ID_Veh", i))

                            l_strCodUnid = ObtenerCodUnid(dtVehiculos.GetValue("U_ID_Veh", i))

                            If String.IsNullOrEmpty(l_strCodUnid) Then
                                l_strCodUnid = dtVehiculos.GetValue("U_Cod_Uni", i)
                            End If

                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Pedido", 0, dtVehiculos.GetValue("U_Num_Ped", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Entrada", 0, dtVehiculos.GetValue("DocEntry", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Unid", 0, l_strCodUnid)
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Marca", 0, dtVehiculos.GetValue("U_Cod_Mar", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Estilo", 0, dtVehiculos.GetValue("U_Cod_Est", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Modelo", 0, dtVehiculos.GetValue("U_Cod_Mod", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Color", 0, dtVehiculos.GetValue("U_Cod_Col", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_VIN", 0, dtVehiculos.GetValue("U_Num_Vin", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Motor", 0, dtVehiculos.GetValue("U_Num_Mot", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Ano_Veh", 0, dtVehiculos.GetValue("U_Ano_Veh", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Art", 0, dtVehiculos.GetValue("U_Cod_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Line_Ref", 0, dtVehiculos.GetValue("U_Line_Ref", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", 0, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", 0, Nothing)

                            l_StrNumCuenta = ObtenerNumeroCuenta(dtVehiculos.GetValue("U_Tipo", 0))
                            If Not String.IsNullOrEmpty(l_StrNumCuenta) Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", 0, l_StrNumCuenta)
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", 0, Nothing)
                            End If
                            numLinea = numLinea + 1
                        Else
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).InsertRecord(intNuevoRegistro)
                            intNuevoRegistro += 1

                            l_strCodUnid = ObtenerCodUnid(dtVehiculos.GetValue("U_ID_Veh", i))

                            If String.IsNullOrEmpty(l_strCodUnid) Then
                                l_strCodUnid = dtVehiculos.GetValue("U_Cod_Uni", i)
                            End If

                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_ID_Unid", intNuevoRegistro - 1, dtVehiculos.GetValue("U_ID_Veh", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Pedido", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Num_Ped", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Entrada", intNuevoRegistro - 1, dtVehiculos.GetValue("DocEntry", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Unid", intNuevoRegistro - 1, l_strCodUnid)
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Marca", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Cod_Mar", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Estilo", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Cod_Est", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Modelo", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Cod_Mod", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Color", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Cod_Col", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_VIN", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Num_Vin", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Motor", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Num_Mot", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Ano_Veh", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Ano_Veh", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Art", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Cod_Art", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Line_Ref", intNuevoRegistro - 1, dtVehiculos.GetValue("U_Line_Ref", i))
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", intNuevoRegistro - 1, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", intNuevoRegistro - 1, Nothing)

                            l_StrNumCuenta = ObtenerNumeroCuenta(dtVehiculos.GetValue("U_Tipo", 0))
                            If Not String.IsNullOrEmpty(l_StrNumCuenta) Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", intNuevoRegistro - 1, l_StrNumCuenta)
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", intNuevoRegistro - 1, Nothing)
                            End If


                        End If
                    End If
                Next
            End If

            MatrixCostArticulos.Matrix.LoadFromDataSource()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub



    Public Sub AgregarVehiculosSeleccionados(ByVal p_dtSeleccionados As SAPbouiCOM.DataTable)
        Try
            Dim l_intTamano As Integer
            Dim l_decMontoAs As Decimal
            Dim l_decTipoC As Decimal
            Dim l_strNumCuenta As String

            FormularioSBO = ApplicationSBO.Forms.Item("SCGD_CostEnt")

            MatrixCostArticulos = New MatrizCosteoArticulos("mtx_Vehi", _formularioSBO, "@SCGD_COST_ART")

            MatrixCostArticulos.Matrix.FlushToDataSource()

            l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size
            'l_intTamano = l_intTamano + 1

            For i As Integer = 0 To p_dtSeleccionados.Rows.Count - 1
                If l_intTamano = 1 AndAlso
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Unid", l_intTamano - 1) = String.Empty Then

                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_ID_Unid", 0, p_dtSeleccionados.GetValue("code", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Entrada", 0, p_dtSeleccionados.GetValue("rece", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Pedido", 0, p_dtSeleccionados.GetValue("pedi", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Unid", 0, p_dtSeleccionados.GetValue("unid", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Marca", 0, p_dtSeleccionados.GetValue("cMar", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Estilo", 0, p_dtSeleccionados.GetValue("cEst", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Modelo", 0, p_dtSeleccionados.GetValue("cMod", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Color", 0, p_dtSeleccionados.GetValue("col", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_VIN", 0, p_dtSeleccionados.GetValue("vin", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Motor", 0, p_dtSeleccionados.GetValue("moto", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Ano_Veh", 0, p_dtSeleccionados.GetValue("ano", i)) 'año
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Art", 0, p_dtSeleccionados.GetValue("arti", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Line_Ref", 0, p_dtSeleccionados.GetValue("line", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", 0, m_strImpuestoSocio)
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", 0, Nothing)

                    l_strNumCuenta = ObtenerNumeroCuenta(p_dtSeleccionados.GetValue("tipo", 0))
                    If Not String.IsNullOrEmpty(l_strNumCuenta) Then
                        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", 0, l_strNumCuenta)
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", 0, Nothing)
                    End If

                    l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size
                Else
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).InsertRecord(l_intTamano)

                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_ID_Unid", l_intTamano, p_dtSeleccionados.GetValue("code", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Entrada", l_intTamano, p_dtSeleccionados.GetValue("rece", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Pedido", l_intTamano, p_dtSeleccionados.GetValue("pedi", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Unid", l_intTamano, p_dtSeleccionados.GetValue("unid", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Marca", l_intTamano, p_dtSeleccionados.GetValue("cMar", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Estilo", l_intTamano, p_dtSeleccionados.GetValue("cEst", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Modelo", l_intTamano, p_dtSeleccionados.GetValue("cMod", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Color", l_intTamano, p_dtSeleccionados.GetValue("col", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_VIN", l_intTamano, p_dtSeleccionados.GetValue("vin", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Motor", l_intTamano, p_dtSeleccionados.GetValue("moto", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Ano_Veh", l_intTamano, p_dtSeleccionados.GetValue("ano", i)) 'año
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Art", l_intTamano, p_dtSeleccionados.GetValue("arti", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Line_Ref", l_intTamano, p_dtSeleccionados.GetValue("line", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", l_intTamano, m_strImpuestoSocio)
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", l_intTamano, Nothing)

                    l_strNumCuenta = ObtenerNumeroCuenta(p_dtSeleccionados.GetValue("tipo", i))
                    If Not String.IsNullOrEmpty(l_strNumCuenta) Then
                        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", l_intTamano, l_strNumCuenta)
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Num_Cta", l_intTamano, Nothing)
                    End If

                    l_intTamano = l_intTamano + 1
                End If
            Next

            MatrixCostArticulos.Matrix.LoadFromDataSource()

            AgregarDatosPedidos(p_dtSeleccionados)
            CopiarValoresDePedidos()

            ActualizaCostosValores(Nothing, Nothing)
            If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Public Sub AgregarDatosPedidos(ByRef oDataTable As SAPbouiCOM.DataTable)
        Try

            Dim l_strLineRef As String
            Dim l_strNumPedido As String
            Dim l_strNumRecepcion As String
            Dim l_intNumLinea As Integer = 0
            Dim l_decCostVehiculo As Decimal = 0
            Dim l_blnAgregado As Boolean = False

            Dim l_strSQL As String

            dtPedidos = _formularioSBO.DataSources.DataTables.Item("dtPed")
            MatrixCostPedidos = New MatrizCosteoPedidos("mtx_Pedido", _formularioSBO, m_strTablaPedidos)

            l_strSQL = "  SELECT EN.DocEntry,  EN.U_Cod_Prov,EN.U_Name_Prov,EN.U_Contact,EN.U_Moneda,EN.U_TipoCambio, LI.LineId, LI.U_Num_Ped ,LI.U_Cod_Art ,LI.U_Desc_Art ,LI.U_Ano_Veh ,LI.U_Cost_Veh ,LI.U_Cant_Ent ,LI.U_Cod_Col ,LI.U_Total_L, LI.U_Line_Ref"
            l_strSQL &= " FROM [@SCGD_ENTRADA_VEH] EN "
            l_strSQL &= " inner join [@SCGD_ENTRADA_LINEAS] LI on EN.DocEntry = LI.DocEntry"
            l_strSQL &= " where EN.DocEntry = '{0}'"
            l_strSQL &= " AND LI.U_Num_Ped = '{1}'"
            l_strSQL &= " AND LI.U_Line_Ref = '{2}'"

            MatrixCostPedidos.Matrix.FlushToDataSource()
            dtPedidos = _formularioSBO.DataSources.DataTables.Item("dtPed")

            If oDataTable.Rows.Count > 0 Then

                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCosteoEntradaCargaMontos, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    m_blnCopiaMontoDePedido = True
                Else
                    m_blnCopiaMontoDePedido = False
                End If

                For i As Integer = 0 To oDataTable.Rows.Count - 1

                    l_strLineRef = oDataTable.GetValue("line", i)
                    l_strNumPedido = oDataTable.GetValue("pedi", i)
                    l_strNumRecepcion = oDataTable.GetValue("rece", i)

                    For j As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                        Dim test1 As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Line_Ref", j).Trim
                        Dim test2 As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", j).Trim
                        Dim test3 As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Entrada", j).Trim

                        If l_strLineRef.Equals(test1) AndAlso
                            l_strNumPedido.Equals(test2) Then

                            l_blnAgregado = True
                            Exit For
                        End If

                    Next

                    If l_blnAgregado = False Then

                        dtPedidos.Clear()
                        dtPedidos.ExecuteQuery(String.Format(l_strSQL, l_strNumRecepcion, l_strNumPedido, l_strLineRef))

                        l_intNumLinea = _formularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size

                        l_decCostVehiculo = dtPedidos.GetValue("U_Cost_Veh", 0)
                        If l_intNumLinea = 1 AndAlso
                            _formularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", 0).Trim.Equals(String.Empty) Then

                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", 0, dtPedidos.GetValue("U_Num_Ped", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", 0, dtPedidos.GetValue("DocEntry", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", 0, dtPedidos.GetValue("U_Cod_Art", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", 0, dtPedidos.GetValue("U_Desc_Art", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", 0, dtPedidos.GetValue("U_Cod_Col", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", 0, dtPedidos.GetValue("U_Cant_Ent", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", 0, dtPedidos.GetValue("U_Ano_Veh", 0))

                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", 0, dtPedidos.GetValue("U_Line_Ref", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", 0, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", 0, 0)

                            If m_blnCopiaMontoDePedido Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, l_decCostVehiculo.ToString(n))
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, 0)
                            End If

                        Else
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).InsertRecord(l_intNumLinea)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", l_intNumLinea, dtPedidos.GetValue("U_Num_Ped", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", l_intNumLinea, dtPedidos.GetValue("DocEntry", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", l_intNumLinea, dtPedidos.GetValue("U_Cod_Art", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", l_intNumLinea, dtPedidos.GetValue("U_Desc_Art", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", l_intNumLinea, dtPedidos.GetValue("U_Cod_Col", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", l_intNumLinea, dtPedidos.GetValue("U_Cant_Ent", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", l_intNumLinea, dtPedidos.GetValue("U_Ano_Veh", 0))

                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", l_intNumLinea, dtPedidos.GetValue("U_Line_Ref", 0))
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", l_intNumLinea, m_strImpuestoSocio)
                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", l_intNumLinea, 0)

                            If m_blnCopiaMontoDePedido Then
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", l_intNumLinea, l_decCostVehiculo.ToString(n))
                            Else
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", l_intNumLinea, 0)
                            End If

                            l_intNumLinea = l_intNumLinea + 1
                        End If
                    Else
                        l_blnAgregado = False


                        'If Not String.IsNullOrEmpty(dtPedidos.GetValue("DocEntry", 0)) Then

                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", 0, dtPedidos.GetValue("U_Num_Ped", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", 0, dtPedidos.GetValue("DocEntry", 0))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", 0, dtPedidos.GetValue("U_Cod_Art", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", 0, dtPedidos.GetValue("U_Desc_Art", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", 0, dtPedidos.GetValue("U_Cod_Col", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", 0, dtPedidos.GetValue("U_Cant_Ent", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", 0, dtPedidos.GetValue("U_Ano_Veh", i))

                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", 0, dtPedidos.GetValue("U_Line_Ref", i))
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", 0, m_strImpuestoSocio)
                        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", 0, 0)

                        '    If m_blnCopiaMontoDePedido Then
                        '        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, l_decCostVehiculo.ToString(n))
                        '    Else
                        '        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, 0)
                        '    End If

                        '    l_intNumLinea = l_intNumLinea + 1

                        'End If
                    End If
                Next
            End If

            MatrixCostPedidos.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub




    'Public Sub AsignaValoresPedidos(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
    '    Try
    '        Dim l_intCodEntrada As Integer
    '        Dim l_strSQL As String
    '        Dim numlinea As Integer
    '        Dim intNuevoRegisto As Integer = 0
    '        Dim l_decCostVehiculo As Decimal

    '        MatrixCostPedidos.Matrix.FlushToDataSource()

    '        dtPedidos = FormularioSBO.DataSources.DataTables.Item("dtPed")

    '        l_strSQL = "  SELECT EN.DocEntry,  EN.U_Cod_Prov,EN.U_Name_Prov,EN.U_Contact,EN.U_Moneda,EN.U_TipoCambio, LI.LineId, LI.U_Num_Ped ,LI.U_Cod_Art ,LI.U_Desc_Art ,LI.U_Ano_Veh ,LI.U_Cost_Veh ,LI.U_Cant_Ent ,LI.U_Cod_Col ,LI.U_Total_L, LI.U_Line_Ref"
    '        l_strSQL &= " FROM [@SCGD_ENTRADA_VEH] EN "
    '        l_strSQL &= " inner join [@SCGD_ENTRADA_LINEAS] LI on EN.DocEntry = LI.DocEntry"
    '        l_strSQL &= " where EN.DocEntry = '{0}'"

    '        l_intCodEntrada = oDataTable.GetValue("DocEntry", 0)

    '        dtPedidos.Clear()
    '        dtPedidos.ExecuteQuery(String.Format(l_strSQL, l_intCodEntrada))

    '        numlinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size
    '        intNuevoRegisto = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size

    '        If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", 0).Trim()) AndAlso
    '             FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1 = 0 Then
    '            numlinea = 0
    '        End If

    '        If Not String.IsNullOrEmpty(dtPedidos.GetValue("DocEntry", 0)) Then

    '            If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCosteoEntradaCargaMontos, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
    '                m_blnCopiaMontoDePedido = True
    '            Else
    '                m_blnCopiaMontoDePedido = False
    '            End If

    '            For i As Integer = 0 To dtPedidos.Rows.Count - 1
    '                If Not String.IsNullOrEmpty(dtPedidos.GetValue("U_Cod_Art", i)) Then

    '                    l_decCostVehiculo = dtPedidos.GetValue("U_Cost_Veh", i)

    '                    If numlinea = 0 Then
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", 0, dtPedidos.GetValue("U_Num_Ped", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", 0, dtPedidos.GetValue("DocEntry", 0))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", 0, dtPedidos.GetValue("U_Cod_Art", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", 0, dtPedidos.GetValue("U_Desc_Art", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", 0, dtPedidos.GetValue("U_Cod_Col", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", 0, dtPedidos.GetValue("U_Cant_Ent", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", 0, dtPedidos.GetValue("U_Ano_Veh", i))

    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", 0, dtPedidos.GetValue("U_Line_Ref", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", 0, m_strImpuestoSocio)
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", 0, 0)

    '                        If m_blnCopiaMontoDePedido Then
    '                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, l_decCostVehiculo.ToString(n))
    '                        Else
    '                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", 0, 0)
    '                        End If

    '                        numlinea = numlinea + 1
    '                    Else
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).InsertRecord(intNuevoRegisto)
    '                        intNuevoRegisto += 1
    '                        ' g_LineaMatriz = numlinea + 1
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Pedido", intNuevoRegisto - 1, dtPedidos.GetValue("U_Num_Ped", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Entrada", intNuevoRegisto - 1, dtPedidos.GetValue("DocEntry", 0))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Art", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cod_Art", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Nam_Art", intNuevoRegisto - 1, dtPedidos.GetValue("U_Desc_Art", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Color", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cod_Col", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cant", intNuevoRegisto - 1, dtPedidos.GetValue("U_Cant_Ent", i))
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Ano_Veh", intNuevoRegisto - 1, dtPedidos.GetValue("U_Ano_Veh", i))

    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Cod_Imp", intNuevoRegisto - 1, m_strImpuestoSocio)
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Impuesto", intNuevoRegisto - 1, 0)
    '                        FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Line_Ref", intNuevoRegisto - 1, dtPedidos.GetValue("U_Line_Ref", i))

    '                        If m_blnCopiaMontoDePedido Then
    '                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", intNuevoRegisto - 1, l_decCostVehiculo.ToString(n))
    '                        Else
    '                            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", intNuevoRegisto - 1, 0)
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If

    '        MatrixCostPedidos.Matrix.LoadFromDataSource()

    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, _applicationSbo)
    '    End Try
    'End Sub




    Private Function ObtenerNumeroCuenta(ByVal p_strTipo As String)
        Try
            Dim l_strSQL As String
            Dim l_strCuenta As String

            l_strSQL = " select U_Transito from [@SCGD_ADMIN4] with (nolock) WHERE U_Tipo = '{0}' "

            dtCuenta = FormularioSBO.DataSources.DataTables.Item("dtCuenta")
            dtCuenta.Clear()

            dtCuenta.ExecuteQuery(String.Format(l_strSQL, p_strTipo))

            If Not String.IsNullOrEmpty(dtCuenta.GetValue("U_Transito", 0)) Then
                l_strCuenta = dtCuenta.GetValue("U_Transito", 0)
            End If

            Return l_strCuenta

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ObtenerCodUnid(ByVal p_StrID As String) As String
        Try

            Dim l_strSQL As String
            Dim l_StrCodUnid As String = String.Empty

            l_strSQL = "SELECT Code, U_Cod_Unid From [@SCGD_VEHICULO] with (nolock) where Code = '{0}'"

            dtVehi = FormularioSBO.DataSources.DataTables.Item("dtVehi")
            dtVehi.Clear()

            dtVehi.ExecuteQuery(String.Format(l_strSQL, p_StrID.Trim))

            If Not String.IsNullOrEmpty(dtVehi.GetValue("U_Cod_Unid", 0)) Then
                l_StrCodUnid = dtVehi.GetValue("U_Cod_Unid", 0)
            End If

            Return l_StrCodUnid

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub CopiarValoresDePedidos()
        Dim l_strNumPed As String
        Dim l_strNumRef As String
        Dim l_StrCodArt As String

        Dim l_decMonto As Decimal

        Try

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Art", i)) Then

                    l_strNumPed = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", i).Trim
                    l_StrCodArt = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Art", i).Trim
                    l_strNumRef = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Line_Ref", i).Trim

                    l_decMonto = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Mnt_Linea", i).Trim, n)

                    For j As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1

                        If l_StrCodArt.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Art", j).Trim) AndAlso
                            l_strNumPed.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Pedido", j).Trim) AndAlso
                            l_strNumRef.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Line_Ref", j).Trim) Then

                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Mnt_Total", j, l_decMonto.ToString(n))

                        End If
                    Next
                End If
            Next

            MatrixCostArticulos.Matrix.LoadFromDataSource()
            MatrixCostPedidos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CopiarImpuestoAPedidos()
        Dim l_strNumPed As String
        Dim l_strNumRef As String
        Dim l_StrCodArt As String

        Dim l_decMonto As Decimal
        Dim l_strCodImpueto As String

        Try

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Art", i)) Then

                    l_strNumPed = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Pedido", i).Trim
                    l_StrCodArt = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Art", i).Trim
                    l_strNumRef = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Line_Ref", i).Trim

                    l_strCodImpueto = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Cod_Imp", i).Trim

                    For j As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1

                        If l_StrCodArt.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Art", j).Trim) AndAlso
                            l_strNumPed.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Pedido", j).Trim) AndAlso
                            l_strNumRef.Equals(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Line_Ref", j).Trim) Then

                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Impuesto", j, l_strCodImpueto)

                        End If
                    Next
                End If
            Next

            MatrixCostArticulos.Matrix.LoadFromDataSource()
            MatrixCostPedidos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function GeneraFacturaProveedor() As String

        Dim l_intError As Integer
        Dim l_strNuevaFac As Integer
        Dim l_strSerieFacturaProv
        Dim l_Error As Integer
        Dim strMensaje As String
        Dim l_FhaCont As Date
        Dim l_FhaDoc As Date
        Dim l_fhaVenc As Date
        Dim l_strAplicaCosteo As String

        Dim l_oFactura As SAPbobsCOM.Documents
        Dim l_oFacturaLineas As SAPbobsCOM.Document_Lines

        Dim l_strResult As String = String.Empty

        Try
            l_FhaCont = Date.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", n)
            l_FhaDoc = Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", n)
            l_fhaVenc = Date.ParseExact(txtFhaVenc.ObtieneValorDataSource, "yyyyMMdd", n)

            l_oFactura = CType(_companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
            l_oFacturaLineas = l_oFactura.Lines

            l_oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service

            l_oFactura.CardCode = txtCodProv.ObtieneValorDataSource()
            l_oFactura.CardName = txtNamProv.ObtieneValorDataSource()
            l_oFactura.DocCurrency = cboMoneda.ObtieneValorDataSource()

            l_oFactura.DocDate = l_FhaCont
            l_oFactura.TaxDate = l_FhaDoc
            l_oFactura.DocDueDate = l_fhaVenc

            l_oFactura.DocRate = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
            l_oFactura.NumAtCard = txtNumRef.ObtieneValorDataSource()
            l_oFactura.Comments = txtObs.ObtieneValorDataSource()

            If Not String.IsNullOrEmpty(cboEncarg.ObtieneValorDataSource()) Then
                l_oFactura.SalesPersonCode = cboEncarg.ObtieneValorDataSource()
            End If

            If Not String.IsNullOrEmpty(txtCodTitular.ObtieneValorDataSource()) Then
                l_oFactura.DocumentsOwner = txtCodTitular.ObtieneValorDataSource()
            End If

            If m_strUsaCosteoAuto.Equals("Y") Then
                If cbxAplicaCosteo.ObtieneValorDataSource = "Y" Then
                    l_strAplicaCosteo = "Y"
                Else
                    l_strAplicaCosteo = "N"
                End If
            Else
                l_strAplicaCosteo = m_strAplicaCosteo
            End If

            l_oFactura.UserFields.Fields.Item("U_SCGD_AplicaCosteo").Value = l_strAplicaCosteo

            If (GenLineas(l_oFacturaLineas)) Then
                l_Error = l_oFactura.Add()
                If l_Error = 0 Then
                    _companySbo.GetNewObjectCode(l_strResult)
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoFacturaCreada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                Else
                    _companySbo.GetLastError(l_Error, strMensaje)
                    Throw New ExceptionsSBO(l_Error, strMensaje)
                End If
            End If

            Return l_strResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Public Function GenDraft() As String

        Dim l_intError As Integer
        Dim l_strNuevaFac As Integer
        Dim l_strSerieFacturaProv
        Dim l_Error As Integer
        Dim strMensaje As String
        Dim l_FhaCont As Date
        Dim l_FhaDoc As Date
        Dim l_fhaVenc As Date


        Dim l_oFactura As SAPbobsCOM.Documents
        Dim l_oFacturaLineas As SAPbobsCOM.Document_Lines
        Dim l_strAplicaCosteo As String
        Dim l_strResult As String = String.Empty

        Try
            l_FhaCont = Date.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", n)
            l_FhaDoc = Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", n)
            l_fhaVenc = Date.ParseExact(txtFhaVenc.ObtieneValorDataSource, "yyyyMMdd", n)


            l_oFactura = CType(_companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts), SAPbobsCOM.Documents)
            l_oFactura.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices

            l_oFacturaLineas = l_oFactura.Lines

            l_oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service

            l_oFactura.CardCode = txtCodProv.ObtieneValorDataSource()
            l_oFactura.CardName = txtNamProv.ObtieneValorDataSource()
            l_oFactura.DocCurrency = cboMoneda.ObtieneValorDataSource()

            l_oFactura.NumAtCard = txtNumRef.ObtieneValorDataSource()
            l_oFactura.Comments = txtObs.ObtieneValorDataSource()

            If Not String.IsNullOrEmpty(cboEncarg.ObtieneValorDataSource()) Then
                l_oFactura.SalesPersonCode = cboEncarg.ObtieneValorDataSource()
            End If

            If Not String.IsNullOrEmpty(txtCodTitular.ObtieneValorDataSource()) Then
                l_oFactura.DocumentsOwner = txtCodTitular.ObtieneValorDataSource()
            End If

            l_oFactura.DocDate = l_FhaCont
            l_oFactura.TaxDate = l_FhaDoc
            l_oFactura.DocDueDate = l_fhaVenc

            l_oFactura.DocRate = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
            l_oFactura.UserFields.Fields.Item("U_SCGD_DocCost").Value = txtDocEntry.ObtieneValorDataSource

            If m_strUsaCosteoAuto.Equals("Y") Then
                If cbxAplicaCosteo.ObtieneValorDataSource = "Y" Then
                    l_strAplicaCosteo = "Y"
                Else
                    l_strAplicaCosteo = "N"
                End If
            Else
                l_strAplicaCosteo = m_strAplicaCosteo
            End If

            l_oFactura.UserFields.Fields.Item("U_SCGD_AplicaCosteo").Value = l_strAplicaCosteo

            If (GenLineas(l_oFacturaLineas)) Then
                l_Error = l_oFactura.Add()
                If l_Error = 0 Then
                    _companySbo.GetNewObjectCode(l_strResult)
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoFacturaCreada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                Else
                    _companySbo.GetLastError(l_Error, strMensaje)
                    Throw New ExceptionsSBO(l_Error, strMensaje)
                End If
            End If

            Return l_strResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Public Sub ActualizaDocumentoCosteo(ByVal p_strDocCost As String)
        Try
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildPago As SAPbobsCOM.GeneralData
            Dim oChildrenPago As SAPbobsCOM.GeneralDataCollection
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams


            Dim l_strNumFactura As String = m_strFacturaProv


            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CDP")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strDocCost)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oGeneralData.SetProperty("U_NumFactura", m_strFacturaProv)
            oGeneralData.SetProperty("U_NumDraft", String.Empty)

            oGeneralService.Update(oGeneralData)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function GenLineas(ByRef l_oFacturaLineas As SAPbobsCOM.Document_Lines) As Boolean
        Try
            Dim l_blnResult As Boolean = False
            Dim l_strMsjE As String
            Dim l_strMsjW As String
            With FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos)
                For i As Integer = 0 To .Size - 1
                    l_strMsjE = String.Empty
                    l_strMsjW = String.Empty
                    If (String.IsNullOrEmpty(.GetValue("U_Cod_Unid", i).Trim)) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoCodigoUnidad, .GetValue("LineId", i))
                    ElseIf (Decimal.Parse(.GetValue("U_Mnt_Total", i).Trim, n) = 0) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoMonto, .GetValue("LineId", i))
                    ElseIf (String.IsNullOrEmpty(.GetValue("U_Cod_Trasacc", i).Trim)) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoTransaccion, .GetValue("LineId", i))
                    ElseIf (Not String.IsNullOrEmpty(.GetValue("U_NumFactura", i).Trim)) Then
                        l_strMsjW = String.Format(My.Resources.Resource.CosteoUnidadFacturada, .GetValue("LineId", i))
                    Else

                        l_oFacturaLineas.ItemDescription = .GetValue("U_Cod_Art", i).Trim()
                        l_oFacturaLineas.TaxCode = .GetValue("U_Cod_Impuesto", i).Trim()
                        l_oFacturaLineas.VatGroup = .GetValue("U_Cod_Impuesto", i).Trim()
                        l_oFacturaLineas.UnitPrice = Decimal.Parse(.GetValue("U_Mnt_Total", i).Trim, n)
                        l_oFacturaLineas.Currency = cboMoneda.ObtieneValorDataSource()
                        l_oFacturaLineas.UserFields.Fields.Item("U_SCGD_Cod_Unid").Value = .GetValue("U_Cod_Unid", i).Trim()
                        l_oFacturaLineas.UserFields.Fields.Item("U_SCGD_Cod_Tran").Value = .GetValue("U_Cod_Trasacc", i).Trim()
                        l_oFacturaLineas.AccountCode = .GetValue("U_Num_Cta", i).Trim
                        l_oFacturaLineas.Add()

                    End If

                    If (Not String.IsNullOrEmpty(l_strMsjE)) Then
                        ApplicationSBO.StatusBar.SetText(l_strMsjE, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return l_blnResult
                    End If

                    If (Not String.IsNullOrEmpty(l_strMsjW)) Then
                        ApplicationSBO.StatusBar.SetText(l_strMsjW, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                        Return l_blnResult
                    End If


                Next
                l_blnResult = True
                Return l_blnResult
            End With

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function GenerarFacturasPorUnidad() As Boolean

        Dim l_blnResult As Boolean = True
        Dim l_intError As Integer
        Dim l_strNuevaFac As Integer
        Dim l_strSerieFacturaProv
        Dim l_Error As Integer
        Dim strMensaje As String
        Dim l_FhaCont As Date
        Dim l_FhaDoc As Date
        Dim l_fhaVenc As Date
        Dim l_strMonto As String
        Dim l_decMonto As Decimal
        Dim l_strNumDocumento As String
        Dim l_strCodUnid As String
        Dim l_strAplicaCosteo As String
        Dim l_strMsjE As String
        Dim l_strMsjW As String
        Dim l_oFactura As SAPbobsCOM.Documents
        Dim l_oFacturaLineas As SAPbobsCOM.Document_Lines

        Dim l_strResult As String = String.Empty
        Dim l_blnAgrega As Boolean = False

        Try

            l_strNumDocumento = txtDocEntry.ObtieneValorDataSource

            l_FhaCont = Date.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", n)
            l_FhaDoc = Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", n)
            l_fhaVenc = Date.ParseExact(txtFhaVenc.ObtieneValorDataSource, "yyyyMMdd", n)

            With FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos)
                For i As Integer = 0 To .Size - 1
                    l_strMsjE = String.Empty
                    l_strMsjW = String.Empty
                    If (String.IsNullOrEmpty(.GetValue("U_Cod_Unid", i).Trim)) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoCodigoUnidad, .GetValue("LineId", i))
                    ElseIf (Decimal.Parse(.GetValue("U_Mnt_Total", i).Trim, n) = 0) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoMonto, .GetValue("LineId", i))
                    ElseIf (String.IsNullOrEmpty(.GetValue("U_Cod_Trasacc", i).Trim)) Then
                        l_strMsjE = String.Format(My.Resources.Resource.CosteoTransaccion, .GetValue("LineId", i))
                    ElseIf (Not String.IsNullOrEmpty(.GetValue("U_NumFactura", i).Trim)) Then
                        l_strMsjW = String.Format(My.Resources.Resource.CosteoUnidadFacturada, .GetValue("LineId", i))
                    Else
                        l_oFactura = CType(_companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
                        l_oFacturaLineas = l_oFactura.Lines
                        l_oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
                        l_oFactura.CardCode = txtCodProv.ObtieneValorDataSource()
                        l_oFactura.CardName = txtNamProv.ObtieneValorDataSource()
                        l_oFactura.DocCurrency = cboMoneda.ObtieneValorDataSource()
                        l_oFactura.DocDate = l_FhaCont
                        l_oFactura.TaxDate = l_FhaDoc
                        l_oFactura.DocDueDate = l_fhaVenc
                        l_oFactura.DocRate = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
                        l_oFactura.NumAtCard = txtNumRef.ObtieneValorDataSource()
                        l_oFactura.Comments = txtObs.ObtieneValorDataSource()

                        If Not String.IsNullOrEmpty(cboEncarg.ObtieneValorDataSource()) Then
                            l_oFactura.SalesPersonCode = cboEncarg.ObtieneValorDataSource()
                        End If

                        If Not String.IsNullOrEmpty(txtCodTitular.ObtieneValorDataSource()) Then
                            l_oFactura.DocumentsOwner = txtCodTitular.ObtieneValorDataSource()
                        End If

                        If m_strUsaCosteoAuto.Equals("Y") Then
                            If cbxAplicaCosteo.ObtieneValorDataSource = "Y" Then
                                l_strAplicaCosteo = "Y"
                            Else
                                l_strAplicaCosteo = "N"
                            End If
                        Else
                            l_strAplicaCosteo = m_strAplicaCosteo
                        End If

                        l_oFactura.UserFields.Fields.Item("U_SCGD_AplicaCosteo").Value = l_strAplicaCosteo
                        l_oFacturaLineas.ItemDescription = .GetValue("U_Cod_Art", i).Trim()
                        l_oFacturaLineas.TaxCode = .GetValue("U_Cod_Impuesto", i).Trim()
                        l_oFacturaLineas.VatGroup = .GetValue("U_Cod_Impuesto", i).Trim()
                        l_oFacturaLineas.UnitPrice = Decimal.Parse(.GetValue("U_Mnt_Total", i).Trim, n)
                        l_oFacturaLineas.Currency = cboMoneda.ObtieneValorDataSource()
                        l_oFacturaLineas.UserFields.Fields.Item("U_SCGD_Cod_Unid").Value = .GetValue("U_Cod_Unid", i).Trim()
                        l_oFacturaLineas.UserFields.Fields.Item("U_SCGD_Cod_Tran").Value = .GetValue("U_Cod_Trasacc", i).Trim()
                        l_oFacturaLineas.AccountCode = .GetValue("U_Num_Cta", i).Trim
                        l_strCodUnid = .GetValue("U_Cod_Unid", i).Trim()

                        If CompanySBO.InTransaction = False Then
                            CompanySBO.StartTransaction()
                        End If

                        l_Error = l_oFactura.Add()

                        If l_Error = 0 Then
                            CompanySBO.GetNewObjectCode(l_strResult)
                            If ActualizaLineasCosteoEntradas(l_strNumDocumento, l_strCodUnid, l_strResult, "U_NumFactura") Then
                                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoFacturaCreada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                            Else
                                If CompanySBO.InTransaction Then
                                    CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                End If
                            End If
                        Else
                            If CompanySBO.InTransaction Then
                                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            _companySbo.GetLastError(l_Error, strMensaje)
                        End If

                    End If

                    If (Not String.IsNullOrEmpty(l_strMsjE)) Then
                        l_blnResult = False
                        ApplicationSBO.StatusBar.SetText(l_strMsjE, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    End If

                    If (Not String.IsNullOrEmpty(l_strMsjW)) Then
                        ApplicationSBO.StatusBar.SetText(l_strMsjW, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    End If
                Next
            End With
            Return l_blnResult

        Catch ex As Exception

            l_blnResult = False
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

            Return l_blnResult

            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function


    Private Function ActualizaLineasCosteoEntradas(ByVal p_strNumDocumento As String,
                                                   ByVal p_strCodUnidad As String,
                                                   ByVal p_strValor As String,
                                                   ByVal p_strCampo As String) As Boolean

        Dim l_blnResult As Boolean = False

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildCosteo As SAPbobsCOM.GeneralData
        Dim oChildrenCosteo As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixCostArticulos.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CDP")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strNumDocumento)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oChildrenCosteo = oGeneralData.Child("SCGD_COST_ART")

            For j As Integer = 0 To oChildrenCosteo.Count - 1
                oChildCosteo = oChildrenCosteo.Item(j)

                If oChildCosteo.GetProperty("U_Cod_Unid").Equals(p_strCodUnidad) AndAlso
                   String.IsNullOrEmpty(oChildCosteo.GetProperty("U_NumFactura")) Then

                    oChildCosteo.SetProperty(p_strCampo, p_strValor)

                    oGeneralService.Update(oGeneralData)
                    l_blnResult = True

                    Exit For
                End If
            Next

            Return l_blnResult

        Catch ex As Exception
            l_blnResult = False
            Return l_blnResult
        End Try
    End Function

    Private Function ActualizaCampoCosteoEntradas(ByVal p_strNumDocumento As String,
                                               ByVal p_strValor As String,
                                               ByVal p_strCampo As String) As Boolean

        Dim l_blnResult As Boolean = False

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildCosteo As SAPbobsCOM.GeneralData
        Dim oChildrenCosteo As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixCostArticulos.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CDP")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strNumDocumento)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oGeneralData.SetProperty(p_strCampo, p_strValor)

            oGeneralService.Update(oGeneralData)

            Return l_blnResult

        Catch ex As Exception
            Return l_blnResult = False
        End Try
    End Function

    Private Function ActualizaEstadoDocCosteoEntradas(ByVal p_strNumDocumento As String,
                                                        ByVal p_blnValor As Boolean) As Boolean

        Dim l_blnResult As Boolean = False

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildCosteo As SAPbobsCOM.GeneralData
        Dim oChildrenCosteo As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixCostArticulos.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CDP")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strNumDocumento)
            'oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            If p_blnValor Then
                oGeneralService.Close(oGeneralParams)
            End If

            Return l_blnResult

        Catch ex As Exception
            Return l_blnResult = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function


    Private Function DevuelveValorItem(ByVal strSucur As String, _
                           ByVal strUDfName As String) As String
        Try

            Dim strSQL As String
            Dim strResult As String
            strSQL = "SELECT {0} FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{1}'"
            strSQL = String.Format(strSQL, strUDfName, strSucur)

            strResult = Utilitarios.EjecutarConsulta(strSQL, _companySbo.CompanyDB, _companySbo.Server)

            If String.IsNullOrEmpty(strResult) Then
                strResult = -1
            End If

            Return strResult
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try



    End Function

    Public Sub CalcularProrrateo()
        Try

            Dim l_decTotal As Decimal
            Dim l_intCant As Integer
            Dim l_MontoPorUnidad As Decimal

            l_intCant = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size

            l_decTotal = Decimal.Parse(txtMontoPror.ObtieneValorDataSource(), n)

            If l_intCant <= 0 Then
                l_intCant = 1
            End If

            MatrixCostArticulos.Matrix.FlushToDataSource()
            l_MontoPorUnidad = l_decTotal / l_intCant

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Unid", i)) Then
                    FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Mnt_Total", i, l_MontoPorUnidad.ToString(n))
                End If
            Next

            MatrixCostArticulos.Matrix.LoadFromDataSource()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaTipoTransferenciaAuto()
        Try
            Dim l_strTrans
            Dim l_intSize As Integer

            l_strTrans = cboTrans.ObtieneValorDataSource()

            MatrixCostArticulos.Matrix.FlushToDataSource()
            l_intSize = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1

            Select Case m_strActualizaTipoTrans
                Case mo_AsigTransaccion.e_todas

                    For i As Integer = 0 To l_intSize
                        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", i, l_strTrans)
                    Next

                Case mo_AsigTransaccion.e_sinAsing

                    For i As Integer = 0 To l_intSize
                        If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Trasacc", i).Trim) Then
                            FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Cod_Trasacc", i, l_strTrans)
                        End If
                    Next

            End Select

            MatrixCostArticulos.Matrix.LoadFromDataSource()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function ValidarDatos(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim l_Result As Boolean = True
            Dim strSQLMoneda As String

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()



            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            If pVal.ItemUID = btnCalcula.UniqueId Then

                If String.IsNullOrEmpty(txtMontoPror.ObtieneValorDataSource) OrElse
                    Decimal.Parse(txtMontoPror.ObtieneValorDataSource, n) <= 0 Then

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasProrrateoCero, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                End If
            ElseIf pVal.ItemUID = "1" Then
                If String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                ElseIf String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaCont, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                ElseIf String.IsNullOrEmpty(txtFhaVenc.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaVence, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                ElseIf String.IsNullOrEmpty(txtCodProv.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinProveedor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False

                ElseIf FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1 <= 0 AndAlso
                String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Unid", 0)) Then

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False

                End If

            End If
            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarCrearFactura(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try

            Dim l_Result As Boolean = True
            Dim strSQLMoneda As String

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()

            strSQLMoneda = "select Currency from OCRD where CardCode = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(strSQLMoneda, txtCodProv.ObtieneValorDataSource))

            If _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCosteoEntradasDocumentoSinActualizar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_Result = False
                BubbleEvent = False
            ElseIf FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1 <= 0 AndAlso
                String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Unid", 0)) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_Result = False
                BubbleEvent = False
                'ElseIf String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                '    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                '    l_Result = False
                '    BubbleEvent = False
                'ElseIf String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource) Then
                '    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaCont, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                '    l_Result = False
                '    BubbleEvent = False
                'ElseIf String.IsNullOrEmpty(txtFhaVenc.ObtieneValorDataSource) Then
                '    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinFechaVence, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                '    l_Result = False
                '    BubbleEvent = False
            ElseIf dtLocal.GetValue("Currency", 0) <> "##" Then
                If dtLocal.GetValue("Currency", 0) <> cboMoneda.ObtieneValorDataSource Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasMonedaProv, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                End If
            ElseIf Not String.IsNullOrEmpty(txtCodFactProv.ObtieneValorDataSource) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasTieneFactura, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_Result = False
            ElseIf Not String.IsNullOrEmpty(txtCodDraft.ObtieneValorDataSource) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasTieneBorrador, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_Result = False
            ElseIf ValidarImpuestoLineas() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasSinImpuesto, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_Result = False
                BubbleEvent = False
            ElseIf ValidarCuentasLineas() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteosEntradaSinCuenta, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_Result = False
                BubbleEvent = False
            ElseIf ValidarTipoTransaccion() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteosEntradaSinTransaccion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_Result = False
                BubbleEvent = False

            End If

            'If l_Result Then
            '    If m_strGeneraDraft.Equals(My.Resources.Resource.No) Then
            '        dtLocal.Clear()
            '        dtLocal.ExecuteQuery(String.Format(strSQLFactura, txtCodFactProv.ObtieneValorDataSource))
            '        If String.IsNullOrEmpty(dtLocal.GetValue("DocEntry", 0)) Then
            '            ApplicationSBO.StatusBar.SetText("Ya existe una factura asociada a este documento", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            '            l_Result = False
            '            BubbleEvent = False
            '        End If
            '        'ElseIf m_strGeneraDraft.Equals(My.Resources.Resource.Si) Then
            '        '    If ApplicationSBO.MessageBox("Existe un borrador creado para este documento, desea continual", 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
            '        '        BubbleEvent = False
            '        '    End If
            '    End If
            'End If


            Return l_Result

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarCuentasLineas() As Boolean
        Try

            Dim l_blnResult As Boolean = False

            MatrixCostArticulos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Num_Cta", i).Trim()) Then
                    l_blnResult = True
                    Exit For
                End If
            Next

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarImpuestoLineas() As Boolean
        Try

            Dim l_blnResult As Boolean = False

            MatrixCostArticulos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Impuesto", i).Trim()) Then
                    l_blnResult = True
                    Exit For
                End If
            Next

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarTipoTransaccion() As Boolean
        Try

            Dim l_blnResult As Boolean = False

            MatrixCostArticulos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Trasacc", i).Trim()) Then
                    l_blnResult = True
                    Exit For
                End If
            Next

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarCargaTipoTransaccion(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim l_Result As Boolean = True
            Dim l_intSize As Integer = 0

            If pVal.ItemUID = cboTrans.UniqueId Then

                If FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size <= 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasNoHayArticulos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                    Exit Function
                End If
            End If

            Dim l_resutlMsg As String
            l_resutlMsg = ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCosteoEntradasComboTrasanccion, 3,
                                                    My.Resources.Resource.MensajeTodas,
                                                    My.Resources.Resource.MensajeSinAsignar,
                                                    My.Resources.Resource.MensajeNinguna)

            If l_resutlMsg = 1 Then
                m_strActualizaTipoTrans = mo_AsigTransaccion.e_todas
            ElseIf l_resutlMsg = 2 Then
                m_strActualizaTipoTrans = mo_AsigTransaccion.e_sinAsing
            ElseIf l_resutlMsg = 3 Then
                m_strActualizaTipoTrans = mo_AsigTransaccion.e_Ninguna
            End If


            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub ActualizaCostosValores(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_decCosto As Decimal
            Dim l_decTotalDoc As Decimal
            Dim l_decPorImp As Decimal
            Dim l_decSumTotal As Decimal
            Dim l_DecSumImp As Decimal
            Dim l_strIndImp As String
            
            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()

            For i As Integer = 0 To MatrixCostArticulos.Matrix.RowCount - 1
                With FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos)
                    l_strIndImp = .GetValue("U_Cod_Impuesto", i)
                    l_decPorImp = Utilitarios.RetornaImpuestoVenta(l_strIndImp, DateTime.Now)
                    
                    l_decCosto = Decimal.Parse(.GetValue("U_Mnt_Total", i).Trim, n)
                    l_DecSumImp = l_DecSumImp + ((l_decPorImp / 100) * l_decCosto)
                    l_decSumTotal = l_decSumTotal + l_decCosto
                    dtLocal.Clear()
                End With
            Next

            txtTotal.AsignaValorDataSource(l_decSumTotal.ToString(n))
            txtImp.AsignaValorDataSource(l_DecSumImp.ToString(n))
            l_decTotalDoc = l_decSumTotal + l_DecSumImp
            txtTotalD.AsignaValorDataSource(l_decTotalDoc.ToString(n))
            txtCant.AsignaValorDataSource(MatrixCostArticulos.Matrix.RowCount - 1)
        Catch ex As Exception

            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ManejaTipoCambio(ByRef bubbleEvent As Boolean) As Boolean
        Try
            Dim l_blnResult As Boolean = True
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_FhaConta As Date

            Dim l_decTipoCam As Decimal
            Dim l_strMonLocal As String
            Dim l_StrMonSist As String
            Dim decTipoC As Decimal
            Dim strTipoC As Decimal

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If


            If m_strMonedaOrigen <> cboMoneda.ObtieneValorDataSource Then

                If cboMoneda.ObtieneValorDataSource() = l_strMonLocal Then
                    txtTipoC.AsignaValorDataSource(1)
                    FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = False
                Else

                    If Not String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource) Then
                        l_FhaConta = DateTime.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                    ElseIf Not String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                        l_FhaConta = DateTime.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                    Else
                        l_FhaConta = Date.Now
                    End If

                    l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

                    dtLocal.Clear()
                    dtLocal.ExecuteQuery(l_strSQLTipoC)

                    If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        cboMoneda.AsignaValorDataSource(m_strMonedaOrigen)
                        bubbleEvent = False
                        l_blnResult = False
                    Else
                        strTipoC = dtLocal.GetValue("Rate", 0)
                        decTipoC = Decimal.Parse(strTipoC)
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COSTEO_ENT").SetValue("U_Doc_Rate", 0, decTipoC.ToString(n))

                    End If
                    FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
                End If
            End If

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function CargaTipoCambio()
        Try
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_FhaConta As Date
            'Dim l_fhaContabilizacion As Date
            'Dim l_fhaDocumento As Date

            Dim decTC As Decimal
            Dim strTC As String

            Dim l_strMonLocal As String
            Dim l_StrMonSist As String
            Dim l_StrMoneda As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"
            l_StrMoneda = cboMoneda.ObtieneValorDataSource()

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            If l_StrMoneda = l_strMonLocal Then
                txtTipoC.AsignaValorDataSource(1)
            ElseIf l_StrMoneda = l_StrMonSist Then
                If Not String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource()) Then
                    l_FhaConta = (DateTime.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                ElseIf Not String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                    l_FhaConta = DateTime.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                Else
                    l_FhaConta = Date.Now
                End If

                l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

                dtLocal.Clear()
                dtLocal.ExecuteQuery(l_strSQLTipoC)

                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    cboMoneda.AsignaValorDataSource(m_strMonedaOrigen)
                Else
                    strTC = dtLocal.GetValue("Rate", 0)
                    decTC = Decimal.Parse(strTC)

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COSTEO_ENT").SetValue("U_Doc_Rate", 0, decTC.ToString(n))

                End If
                FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub ValidaTipoCambio(ByRef BubbleEvent As Boolean)
        Try
            Const strConsulta As String = "Select * From ORTT with(nolock) Where RateDate = '{0}'"
            Dim strSQLSys As String

            Dim strFecha As String
            Dim dtFecha As DateTime
            Dim strTipoCambio As String
            Dim dtSistema As System.Data.DataTable
            Dim strTipoCamb As String
            Dim strMon As String
            Dim oForm As SAPbouiCOM.Form

            Dim strSQLMonedaSis As String = "select MainCurncy, SysCurrncy  from OADM"
            Dim strSQLTipoC As String = "select  AD.SysCurrncy, TT.Rate from OADM AD inner JOIN ORTT TT ON TT.Currency = AD.SysCurrncy"
            strSQLTipoC &= " where TT.RateDate = '{0}'"

            dtFecha = Today.Date

            dtSistema = Utilitarios.EjecutarConsultaDataTable(strSQLMonedaSis, _companySbo.CompanyDB, _companySbo.Server)

            strFecha = Utilitarios.RetornaFechaFormatoDB(dtFecha, _companySbo.Server)
            strSQLTipoC = String.Format(strSQLTipoC, strFecha)

            strTipoCamb = Utilitarios.EjecutarConsulta(strSQLTipoC, _companySbo.CompanyDB, _companySbo.Server)

            Dim strLocal As String
            Dim strSistema As String

            strLocal = dtSistema.Rows(0).Item("MainCurncy").ToString
            strSistema = dtSistema.Rows(0).Item("SysCurrncy").ToString

            If Not strLocal.Equals(strSistema) Then

                If String.IsNullOrEmpty(strTipoCamb) Then
                    _applicationSbo.MessageBox(My.Resources.Resource.TipoCambioNoActualizado, BoMessageTime.bmt_Short, My.Resources.Resource.btnOk)
                    BubbleEvent = False

                End If

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejoCambioMoneda()
        Try

            Dim l_decTotalEncabBase As Decimal
            Dim l_decTotalEncabDestino As Decimal

            Dim l_strMonDestino As String
            Dim l_strMonOrigen As String
            Dim l_strMonLocal As String = String.Empty
            Dim l_strMonSistema As String = String.Empty

            Dim l_decTCOrigen As Decimal
            Dim l_decTCDestino As Decimal
            Dim l_StrSQLSys As String

            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_strMonSistema = dtLocal.GetValue("SysCurrncy", 0)
            End If

            l_strMonOrigen = m_strMonedaOrigen
            l_strMonDestino = m_strMonedaDestino

            Dim l_strTCOrigen As String
            Dim l_strTCDestino As String

            If m_decTCOrigen = 0 Then
                l_strTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                'l_decTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                l_decTCOrigen = Decimal.Parse(l_strTCOrigen)
            Else
                l_decTCOrigen = m_decTCOrigen
            End If

            If m_decTCDestino = 0 Then
                l_strTCDestino = ObtieneTipoCambio(l_strMonDestino, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                'l_decTCDestino = ObtieneTipoCambio(l_strMonDestino, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                'l_decTCDestino = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
                l_decTCDestino = Decimal.Parse(l_strTCDestino)
            Else
                l_decTCDestino = m_decTCDestino
            End If

            MatrixCostArticulos.Matrix.FlushToDataSource()
            MatrixCostPedidos.Matrix.FlushToDataSource()

            Dim l_decPedidoCostoBase(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1) As Decimal
            Dim l_decPedidoCostoDestino(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1) As Decimal

            Dim l_decUnidadCostoBase(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1) As Decimal
            Dim l_decUnidadCostoDestino(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1) As Decimal

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                l_decPedidoCostoBase(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).GetValue("U_Mnt_Linea", i), n)
            Next

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                l_decUnidadCostoBase(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Mnt_Total", i), n)
            Next

            l_decTotalEncabBase = Decimal.Parse(txtTotal.ObtieneValorDataSource, n)
            l_decTotalEncabDestino = 0

            If l_strMonDestino = l_strMonOrigen Then

                If l_decTCDestino = 0 Then
                    l_decTCDestino = 1
                End If
                If l_decTCOrigen = 0 Then
                    l_decTCOrigen = 1
                End If


                For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                    l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i)
                Next

                For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                    l_decUnidadCostoDestino(i) = l_decUnidadCostoBase(i)
                Next

            ElseIf l_strMonOrigen <> l_strMonDestino Then
                If l_decTCDestino = 0 Then
                    l_decTCDestino = 1
                End If
                If l_decTCOrigen = 0 Then
                    l_decTCOrigen = 1
                End If

                If l_strMonOrigen = l_strMonLocal Then

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                        l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i) / l_decTCDestino
                    Next

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                        l_decUnidadCostoDestino(i) = l_decUnidadCostoBase(i) / l_decTCDestino
                    Next

                    l_decTotalEncabDestino = l_decTotalEncabBase / l_decTCDestino

                ElseIf l_strMonDestino = l_strMonLocal Then
                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                        l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i) * l_decTCOrigen
                    Next

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                        l_decUnidadCostoDestino(i) = l_decUnidadCostoBase(i) * l_decTCOrigen
                    Next

                Else
                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                        l_decPedidoCostoDestino(i) = (l_decPedidoCostoBase(i) * l_decTCOrigen) / l_decTCDestino
                    Next

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                        l_decUnidadCostoDestino(i) = (l_decUnidadCostoBase(i) * l_decTCOrigen) / l_decTCDestino
                    Next

                End If

            End If

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).Size - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Mnt_Linea", i, l_decPedidoCostoDestino(i).ToString(n))
            Next

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).SetValue("U_Mnt_Total", i, l_decUnidadCostoDestino(i).ToString(n))
            Next


            MatrixCostArticulos.Matrix.LoadFromDataSource()
            MatrixCostPedidos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal p_StrMoneda As String, ByVal p_strFecha As Date) As String
        Try

            Dim l_strTipoC As String
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_StrMonLocal As String
            Dim l_StrMonSist As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            ' FormularioSBO.Freeze(True)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_StrMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            If cboMoneda.ObtieneValorDataSource = l_StrMonLocal Then
                l_strTipoC = 1
            Else
                l_strSQLTipoC = String.Format(l_strSQLTipoC,
                                          Utilitarios.RetornaFechaFormatoDB(p_strFecha, _companySbo.Server),
                                          p_StrMoneda)
                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
                dtLocal.Clear()
                dtLocal.ExecuteQuery(l_strSQLTipoC)

                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) Then
                    l_strTipoC = -1
                Else
                    l_strTipoC = dtLocal.GetValue("Rate", 0)
                End If

            End If

            Return l_strTipoC

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub AgregarLineaArticulos(Optional ByVal p_blnCarga As Boolean = False, Optional ByVal p_codigomarca As String = "")

        Dim intSize As Integer
        MatrixCostArticulos.Matrix.FlushToDataSource()

        intSize = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).Size
        FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).InsertRecord(intSize)

        MatrixCostArticulos.Matrix.LoadFromDataSource()

    End Sub

    Public Sub EliminarLineaArticulo()
        Try
            Dim intSelect As Integer
            Dim oMat As SAPbouiCOM.Matrix
            Dim l_list As New List(Of Integer)


            FormularioSBO.Freeze(True)
            MatrixCostArticulos.Matrix.FlushToDataSource()

            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)
            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Do While intSelect > -1

                l_list.Add(intSelect)

                'MatrixCostArticulos.Matrix.FlushToDataSource()
                'FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).RemoveRecord(intSelect - 1)

                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Loop


            l_list.Reverse()
            Dim num As Integer

            For Each num In l_list
                FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).RemoveRecord(num - 1)
            Next


            MatrixCostArticulos.Matrix.LoadFromDataSource()

            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If

            FormularioSBO.Freeze(False)


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Public Sub ManejadorEventoCombo(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim l_tipo As String
            Dim strValorSel As String
            Dim strValor As String

            If pVal.ActionSuccess Then

                ' dtVehiculos = FormularioSBO.DataSources.DataTables.Item(strdtVehiculos)
                'MatrizVehiculos.Matrix.FlushToDataSource()

                Select Case pVal.ItemUID
                    Case cboTipo.UniqueId
                        oItem = FormularioSBO.Items.Item("cboTipo")
                        oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

                        l_tipo = oCombo.Selected.Value

                        If l_tipo = "A" Then
                            FormularioSBO.Items.Item("mtx_Vehi").Visible = True
                            FormularioSBO.Items.Item("mtx_Pedido").Visible = False

                            ' FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = True
                        ElseIf l_tipo = "P" Then
                            FormularioSBO.Items.Item("mtx_Vehi").Visible = False
                            FormularioSBO.Items.Item("mtx_Pedido").Visible = True

                            '  FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = False
                        End If
                    Case cboTrans.UniqueId
                        AsignaTipoTransferenciaAuto()

                    Case cboMoneda.UniqueId
                        m_strMonedaDestino = cboMoneda.ObtieneValorDataSource

                        If ManejaTipoCambio(BubbleEvent) Then

                            m_decTCDestino = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)

                            ManejoCambioMoneda()
                            ActualizaCostosValores(pVal, BubbleEvent)

                        End If

                        'Call ActualizaTipoCambio()
                        ' ActualizaTipoCambio(cboMoneda.ObtieneValorDataSource())

                    Case MatrixCostArticulos.UniqueId
                        Select Case pVal.ColUID
                            Case "col_Mar"
                                MatrixCostArticulos.Matrix.FlushToDataSource()
                                strValorSel = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Marca", pVal.Row - 1).Trim
                                CargarComboEstilos(FormularioSBO, strValorSel)
                            Case "col_Est"
                                MatrixCostArticulos.Matrix.FlushToDataSource()
                                strValorSel = FormularioSBO.DataSources.DBDataSources.Item(m_StrTablaArticulos).GetValue("U_Cod_Estilo", pVal.Row - 1).Trim
                                CargarComboModelo(FormularioSBO, strValorSel)
                        End Select
                        'Case cboSource.UniqueId
                        'Call ManejaSourceMoneda(pVal)
                End Select

            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case cboTrans.UniqueId
                        If Not ValidarCargaTipoTransaccion(pVal, BubbleEvent) Then
                            BubbleEvent = False
                            Exit Sub
                        End If

                    Case cboMoneda.UniqueId
                        m_strMonedaOrigen = cboMoneda.ObtieneValorDataSource
                        m_decTCOrigen = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)

                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim strCFL_Id As String
        Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent

        oCFLEvent = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        strCFL_Id = oCFLEvent.ChooseFromListUID
        oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)

        If oCFLEvent.ActionSuccess Then

            Dim oDataTable As SAPbouiCOM.DataTable
            oDataTable = oCFLEvent.SelectedObjects

            If Not oCFLEvent.SelectedObjects Is Nothing Then
                If Not oDataTable Is Nothing And _formularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

                    Select Case pval.ItemUID

                        Case btnCopy.UniqueId

                            AsignaValoresEncabezado(oDataTable)
                            CargarInfoProveedor()

                            AsignaValoresPedidos(FormUID, pval, oDataTable)
                            AsignaValoresVehiculos(FormUID, pval, oDataTable)
                            CopiarValoresDePedidos()
                            ActualizaCostosValores(pval, BubbleEvent)

                            CargaTipoCambio()

                            ManejaEstadoBntFactura(False)

                        Case "mtx_Vehi"

                            If pval.ColUID = "col_Imp" Then
                                AsignaValoresColImpuestoVeh(FormUID, pval, oDataTable)

                            ElseIf pval.ColUID = "col_Cta" Then
                                AsignaValoresColNumCuenta(FormUID, pval, oDataTable)

                            End If

                        Case "mtx_Pedido"
                            If pval.ColUID = "col_Imp" Then
                                AsignaValoresColImpuestoPed(FormUID, pval, oDataTable)
                            End If

                        Case txtCodProv.UniqueId
                            AsignaValoresTxtProvedor(FormUID, pval, oDataTable)

                        Case txtNamTitular.UniqueId
                            AsignaValoresTxtTitular(FormUID, pval, oDataTable)

                    End Select

                End If
            End If

        ElseIf oCFLEvent.BeforeAction Then

            Select Case pval.ItemUID

                Case txtCodProv.UniqueId
                    oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add

                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "CardType"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = "S"
                    oCondition.BracketCloseNum = 1

                    oCFL.SetConditions(oConditions)

                Case btnCopy.UniqueId

                    oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add

                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "Status"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = "O"
                    oCondition.BracketCloseNum = 1

                    If Not String.IsNullOrEmpty(txtCodProv.ObtieneValorDataSource) Then

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = "U_Cod_Prov"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = txtCodProv.ObtieneValorDataSource
                        oCondition.BracketCloseNum = 2

                    End If

                    oCFL.SetConditions(oConditions)

                Case "mtx_Vehi"

                    If pval.ColUID = "col_Imp" Then

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add()
                        If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "Category"
                            oCondition.CondVal = "I"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Locked"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        Else
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "ValidForAR"
                            oCondition.CondVal = "Y"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1

                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Lock"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        End If
                        oCFL.SetConditions(oConditions)

                    End If
            End Select

        End If
    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)


        If pVal.ActionSuccess Then
            Select Case pVal.ItemUID

                Case btnFactura.UniqueId
                    Dim l_strFacPro As String
                    Dim l_strDocCosteo As String

                    l_strDocCosteo = txtDocEntry.ObtieneValorDataSource()

                    If m_strGeneraDraft.Equals("Y") Then
                        l_strFacPro = GenDraft()
                        ActualizaCampoCosteoEntradas(l_strDocCosteo, l_strFacPro, "U_NumDraft")
                    ElseIf m_strGeneraDraft.Equals("N") Then

                        If m_strFactPorUnid.Equals("Y") Then

                            If GenerarFacturasPorUnidad() Then

                                ActualizaCampoCosteoEntradas(l_strDocCosteo, "-1", "U_NumFactura")

                            End If

                        ElseIf m_strFactPorUnid.Equals("N") Then

                            l_strFacPro = GeneraFacturaProveedor()
                            If Not String.IsNullOrEmpty(l_strFacPro) Then

                                If m_strGeneraDraft.Equals("N") Then

                                    ActualizaCampoCosteoEntradas(l_strDocCosteo, l_strFacPro, "U_NumFactura")
                                    ActualizaEstadoDocCosteoEntradas(l_strDocCosteo, True)

                                ElseIf m_strGeneraDraft.Equals("Y") Then
                                    ActualizaCampoCosteoEntradas(l_strDocCosteo, l_strFacPro, "U_NumDraft")
                                End If

                            Else
                                BubbleEvent = False
                                Exit Sub
                            End If

                        End If
                    End If

                    RecargarFormulario(CInt(l_strDocCosteo))
                    ManejadorEventoFormDataLoad(FormularioSBO)

                Case btnCalcula.UniqueId
                    CalcularProrrateo()
                    Call ActualizaCostosValores(pVal, BubbleEvent)

                Case txtFhaCont.UniqueId
                    AsignaFechaVencimiento(m_strGroupNum)

                Case btnMas.UniqueId
                    AgregarLineaArticulos()

                Case btnMenos.UniqueId
                    EliminarLineaArticulo()
                Case cbxAplicaCosteo.UniqueId
                    ManejaCheckCosteo(pVal, BubbleEvent)
                Case btnCopy.UniqueId
                    CargarFormularioSeleccionVehiculos()

            End Select


        ElseIf pVal.BeforeAction Then
            Select Case pVal.ItemUID
                Case btnCalcula.UniqueId
                    If Not ValidarDatos(pVal, BubbleEvent) Then
                        BubbleEvent = False
                    End If

                Case btnFactura.UniqueId
                    If Not ValidarCrearFactura(pVal, BubbleEvent) Then
                        BubbleEvent = False
                    End If

                Case btnMenos.UniqueId
                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCosteoEntradasEliminarLinea, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                        BubbleEvent = False
                    End If
                Case "1"
                    If _formularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse
                        _formularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                        If Not ValidarDatos(pVal, BubbleEvent) Then
                            BubbleEvent = False
                        End If

                    End If

            End Select

        End If
    End Sub

    Private Sub RecargarFormulario(ByVal p_strDocCosteo As Integer)
        Dim oConditions As Conditions
        Dim oCondition As Condition
        
        Try
            oConditions = DirectCast(DMS_Connector.Company.ApplicationSBO.CreateObject(BoCreatableObjectType.cot_Conditions), Conditions)
            oCondition = oConditions.Add()
            oCondition.Alias = "DocEntry"
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strDocCosteo
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COST_ART").Query(oConditions)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COST_LIN").Query(oConditions)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_COSTEO_ENT").Query(oConditions)
            MatrixCostArticulos.Matrix.LoadFromDataSource()
            MatrixCostPedidos.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Throw
        End Try


    End Sub

    Public Sub ManejadorEventoValidate(ByVal FormUID As String, ByRef pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pval.BeforeAction Then

            ElseIf pval.ActionSuccess Then
                If pval.ItemUID = MatrixCostArticulos.UniqueId Then
                    Select Case pval.ColUID
                        Case "col_Tot", "col_Imp"
                            ActualizaCostosValores(pval, BubbleEvent)
                    End Select
                ElseIf pval.ItemUID = MatrixCostPedidos.UniqueId Then
                    Select Case pval.ColUID

                        Case "col_Tot"
                            CopiarValoresDePedidos()
                            ActualizaCostosValores(pval, BubbleEvent)
                        Case MatrixCostArticulos.ColumnaColImp.UniqueId
                            CopiarImpuestoAPedidos()
                            ActualizaCostosValores(pval, BubbleEvent)

                    End Select
                ElseIf pval.ItemUID = txtFhaCont.UniqueId Then
                    AsignaFechaVencimiento(m_strGroupNum)

                    If ManejaTipoCambio(BubbleEvent) Then
                        m_decTCDestino = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
                        ' ManejoCambioMoneda()
                        ActualizaCostosValores(pval, BubbleEvent)
                    End If


                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item

            If pval.BeforeAction Then

                Select Case pval.MenuUID
                    Case "1284"
                        If _applicationSbo.MessageBox(My.Resources.Resource.MensajeCosteoEntradasCancelarDoc, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                            If Not CancelarDocCosteo(BubbleEvent) Then
                                BubbleEvent = False
                                Exit Sub
                            End If
                        Else
                            BubbleEvent = False
                            Exit Sub
                        End If
                    Case "SCGD_CDP"
                        ValidaTipoCambio(BubbleEvent)
                End Select

            End If

            Select Case pval.MenuUID

                Case "1282"                 'BOTON NUEVO

                    FormularioSBO.Freeze(True)

                    CargarMonedaLocal()
                    CargarSerieDocumento()

                    If Not FormularioSBO Is Nothing Then

                        FormularioSBO = ApplicationSBO.Forms.Item("SCGD_CostEnt")
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next
                    End If

                    FormularioSBO.EnableMenu("1282", False)

                    FormularioSBO.Items.Item(txtDocNum.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(cboStatus.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(txtCodFactProv.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(cbxCancelar.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(txtCodDraft.UniqueId).Enabled = False

                    txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

                    If m_strGeneraDraft.Equals("Y") Then

                        FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = True
                        FormularioSBO.Items.Item("lkbDraft").Visible = True
                        FormularioSBO.Items.Item("lblDraft").Visible = True

                    ElseIf m_strGeneraDraft.Equals("N") Then

                        FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = False
                        FormularioSBO.Items.Item("lkbDraft").Visible = False
                        FormularioSBO.Items.Item("lblDraft").Visible = False

                    End If

                    ManejaEstadoBntFactura(False)

                    FormularioSBO.Freeze(False)

                Case "1281"
                    FormularioSBO.Freeze(True)
                    If Not FormularioSBO Is Nothing Then

                        FormularioSBO = ApplicationSBO.Forms.Item("SCGD_CostEnt")
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next
                    End If

                    FormularioSBO.Freeze(False)
            End Select

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try

            Dim l_strCodFact As String
            Dim oItem As SAPbouiCOM.Item
            Dim oLink As SAPbouiCOM.LinkedButton

            l_strCodFact = txtCodFactProv.ObtieneValorDataSource()

            ' Call CargarMonedaSocio(l_strCardCode)
            Call CargarInfoProveedor()

            If cboStatus.ObtieneValorDataSource() = "C" Then
                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE

            ElseIf Not String.IsNullOrEmpty(l_strCodFact) Then
                If Not FormularioSBO Is Nothing Then
                    FormularioSBO = ApplicationSBO.Forms.Item("SCGD_CostEnt")

                    FormularioSBO.Freeze(True)
                    For Each oItem In FormularioSBO.Items
                        oItem.AffectsFormMode = False
                    Next
                    FormularioSBO.Freeze(False)
                End If

                FormularioSBO.Items.Item("mtx_Vehi").Enabled = False
                FormularioSBO.Items.Item("mtx_Pedido").Enabled = False

            Else

                If Not String.IsNullOrEmpty(txtDocEntry.ObtieneValorDataSource) Then
                    FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = False
                Else
                    FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = True
                End If

                If Not FormularioSBO Is Nothing Then
                    FormularioSBO = ApplicationSBO.Forms.Item("SCGD_CostEnt")

                    FormularioSBO.Freeze(True)
                    For Each oItem In FormularioSBO.Items
                        oItem.Enabled = True
                    Next
                    FormularioSBO.Freeze(False)
                End If

                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                FormularioSBO.Items.Item(txtDocNum.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cboSerie.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cboStatus.UniqueId).Enabled = False
                FormularioSBO.Items.Item(txtCodFactProv.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cbxCancelar.UniqueId).Enabled = False

                FormularioSBO.Items.Item("lkbFact").Enabled = True
                FormularioSBO.EnableMenu("1282", True)

            End If

            If Not String.IsNullOrEmpty(txtCodFactProv.ObtieneValorDataSource) Then
                FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = False
                FormularioSBO.Items.Item("lkbDraft").Visible = False
                FormularioSBO.Items.Item("lblDraft").Visible = False

            ElseIf Not String.IsNullOrEmpty(txtCodDraft.ObtieneValorDataSource) Then
                FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = True
                FormularioSBO.Items.Item("lkbDraft").Visible = True
                FormularioSBO.Items.Item("lblDraft").Visible = True

            End If


            'oLink = CType(oItem.Specific, SAPbouiCOM.LinkedButton)

            'If Not String.IsNullOrEmpty(txtCodFactProv.ObtieneValorDataSource) Then
            '    oLink.LinkedObjectType = 18
            '    oLink.LinkTo = txtCodFactProv.UniqueId()


            '    FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = False
            '    FormularioSBO.Items.Item(txtCodFactProv.UniqueId).Visible = True

            'ElseIf Not String.IsNullOrEmpty(txtCodDraft.ObtieneValorDataSource) Then
            '    oLink.LinkedObjectType = 112
            '    oLink.LinkTo = txtCodDraft.UniqueId()

            '    FormularioSBO.Items.Item(txtCodDraft.UniqueId).Visible = True
            '    FormularioSBO.Items.Item(txtCodFactProv.UniqueId).Visible = False
            'End If

            ManejaEstadoBntFactura()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Private Function CancelarDocCosteo(ByRef BubbleEvent As Boolean)
        Try
            Dim l_blnRes As Boolean = True
            Dim l_StrSQL As String

            If Not String.IsNullOrEmpty(txtCodFactProv.ObtieneValorDataSource) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCosteoEntradasDocumentosAsociados, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_blnRes = False
            End If

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Protected Friend Sub CargarComboEstilos(ByRef oForm As SAPbouiCOM.Form,
                                        ByVal p_strIDValSelect As String)
        Try
            Dim l_strSQL As String
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim str_DescDataRow As String
            oForm.Freeze(True)

            l_strSQL = "SELECT Code, Name FROM [@SCGD_ESTILO] WHERE U_Cod_Marc = '{0}'"
            oMatriz = DirectCast(oForm.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQL, p_strIDValSelect))

            If oMatriz.Columns.Item("col_Est").ValidValues.Count > 0 Then
                For i As Integer = 0 To oMatriz.Columns.Item("col_Est").ValidValues.Count - 1
                    oMatriz.Columns.Item("col_Est").ValidValues.Remove(oMatriz.Columns.Item("col_Est").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                str_DescDataRow = dtLocal.GetValue("Name", i)
                If str_DescDataRow.Length > 60 Then
                    Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                    oMatriz.Columns.Item("col_Est").ValidValues.Add(dtLocal.GetValue("Code", i), strDescripcion)
                Else
                    oMatriz.Columns.Item("col_Est").ValidValues.Add(dtLocal.GetValue("Code", i), str_DescDataRow)
                End If
            Next

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Protected Friend Sub CargarComboModelo(ByRef oForm As SAPbouiCOM.Form,
                                        ByVal p_strIDValSelect As String)
        Try
            Dim l_strSQL As String
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim str_DescDataRow As String
            oForm.Freeze(True)

            l_strSQL = "SELECT Code,Name,U_Cod_Esti,U_Descripcion ,U_CodigoFabrica FROM [@SCGD_MODELO] where U_Cod_Esti = '{0}'"
            oMatriz = DirectCast(oForm.Items.Item("mtx_Vehi").Specific, SAPbouiCOM.Matrix)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQL, p_strIDValSelect))

            If oMatriz.Columns.Item("col_Mod").ValidValues.Count > 0 Then
                For i As Integer = 0 To oMatriz.Columns.Item("col_Mod").ValidValues.Count - 1
                    oMatriz.Columns.Item("col_Mod").ValidValues.Remove(oMatriz.Columns.Item("col_Mod").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                str_DescDataRow = dtLocal.GetValue("U_Descripcion", i)
                If str_DescDataRow.Length > 60 Then
                    Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                    oMatriz.Columns.Item("col_Mod").ValidValues.Add(dtLocal.GetValue("Code", i), strDescripcion)
                Else
                    oMatriz.Columns.Item("col_Mod").ValidValues.Add(dtLocal.GetValue("Code", i), str_DescDataRow)
                End If
            Next

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejaEstadoBntFactura(Optional ByVal p_blnEnableBtn As Boolean = True)
        Try
            If Not String.IsNullOrEmpty(txtCodFactProv.ObtieneValorDataSource) OrElse
                Not String.IsNullOrEmpty(txtCodDraft.ObtieneValorDataSource) OrElse
                p_blnEnableBtn = False OrElse
                cbxCancelar.ObtieneValorDataSource = "Y" OrElse
                cboStatus.ObtieneValorDataSource = "C" Then

                FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = False


            ElseIf Not String.IsNullOrEmpty(txtDocEntry.ObtieneValorDataSource) Then
                FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = True

            ElseIf String.IsNullOrEmpty(txtDocEntry.ObtieneValorDataSource) Then
                FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = True

            Else

                FormularioSBO.Items.Item(btnFactura.UniqueId).Enabled = True

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


#End Region


End Class


