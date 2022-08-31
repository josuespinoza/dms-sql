Imports DMSOneFramework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess
Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic

Partial Public Class PedidoDeVehiculos : Implements IUsaPermisos




#Region "Declaraciones"

    Dim m_strMatPedidos As String = "mtx_Ped"

    Dim m_strTablaPedidos As String = "@SCGD_PEDIDOS"
    Dim m_strTablePedidosLineas As String = "@SCGD_PEDIDOS_LINEAS"

    Private m_strMonOrigen As String
    Private m_strMonDestino As String

#End Region

#Region "Metodos / Funciones"

    Public Sub CargarMonedaSocio(ByVal p_strCardCode As String)
        Try
            Dim l_strSQLProv As String
            Dim l_StrSQLSys As String
            Dim l_strMoneda As String
            Dim l_strMonSys As String
            Dim l_strMonLoc As String

            l_strSQLProv = "Select  CardCode, CardName, Currency  from OCRD OC where CardCode = '{0}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(l_StrSQLSys)
            l_strMonLoc = dtLocal.GetValue("MainCurncy", 0)
            l_strMonSys = dtLocal.GetValue("SysCurrncy", 0)

            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQLProv, p_strCardCode))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CardCode", 0)) Then
                l_strMoneda = dtLocal.GetValue("Currency", 0)

                If l_strMoneda = My.Resources.Resource.MonedasTodas Then
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Enabled = True
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
                    cboMoneda.AsignaValorDataSource(l_strMonLoc)
                Else
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
                    cboMoneda.AsignaValorDataSource(l_strMoneda)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    'Public Sub ActualizaTipoCambio(ByRef bubbleEvent As Boolean)
    '    Try

    '        Dim l_strSQLTipoC As String
    '        Dim l_StrSQLSys As String
    '        Dim l_FhaConta As Date

    '        Dim l_decTipoCam As Decimal
    '        Dim l_strMonLocal As String
    '        Dim l_StrMonSist As String
    '        Dim l_strMonProv As String

    '        l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
    '        l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

    '        dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
    '        dtLocal.Clear()
    '        dtLocal.ExecuteQuery(l_StrSQLSys)

    '        If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
    '            l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
    '            l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
    '        End If

    '        If m_strMonOrigen <> cboMoneda.ObtieneValorDataSource Then

    '            If cboMoneda.ObtieneValorDataSource() = l_strMonLocal Then
    '                txtTipoCam.AsignaValorDataSource(1)
    '                FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = False
    '            Else

    '                If Not String.IsNullOrEmpty(txtFhaPedi.ObtieneValorDataSource) Then
    '                    l_FhaConta = DateTime.ParseExact(txtFhaPedi.ObtieneValorDataSource, "yyyyMMdd", Nothing)
    '                Else
    '                    l_FhaConta = Date.Now
    '                End If

    '                l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

    '                dtLocal.Clear()
    '                dtLocal.ExecuteQuery(l_strSQLTipoC)

    '                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
    '                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
    '                    bubbleEvent = False
    '                Else

    '                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_DocRate", 0, dtLocal.GetValue("Rate", 0))
    '                    FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = True
    '                End If

    '            End If

    '        End If


    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, _applicationSbo)
    '    End Try
    'End Sub

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


    Public Function ManejaTipoCambio(ByRef bubbleEvent As Boolean) As Boolean
        Try

            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_FhaConta As Date

            Dim l_decTC As Decimal
            Dim l_strTC As Decimal

            Dim l_strMonLocal As String
            Dim l_StrMonSist As String
            Dim l_strMonProv As String
            Dim l_blnResult As Boolean = True

            l_strSQLTipoC = "Select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "Select MainCurncy, SysCurrncy  from OADM"


            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            ' If m_strMonOrigen <> cboMoneda.ObtieneValorDataSource Then

            If cboMoneda.ObtieneValorDataSource() = l_strMonLocal Then
                txtTipoCam.AsignaValorDataSource(1)
                FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = False
            ElseIf m_strMonOrigen = m_strMonDestino Then



            Else

                If Not String.IsNullOrEmpty(txtFhaPedi.ObtieneValorDataSource) Then
                    l_FhaConta = DateTime.ParseExact(txtFhaPedi.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                Else
                    l_FhaConta = Date.Now
                End If

                l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

                dtLocal.Clear()
                dtLocal.ExecuteQuery(l_strSQLTipoC)

                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    cboMoneda.AsignaValorDataSource(m_strMonOrigen)
                    bubbleEvent = False
                    l_blnResult = False

                Else
                    l_strTC = dtLocal.GetValue("Rate", 0)
                    l_decTC = Decimal.Parse(l_strTC)
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_DocRate", 0, l_decTC.ToString(n))
                End If
                FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = True
            End If
            ' End If
            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function



    Public Sub ManejoCambioDeMoneda()
        Try
            Dim l_strMonLocal As String
            Dim l_strMonSistema As String
            Dim l_strMonOrigen As String
            Dim l_strMonDestido As String

            Dim l_decTCOrigen As Decimal
            Dim l_decTCDestino As Decimal

            'Dim l_decTipoCam As String
            Dim l_decTipoCamSis As String
            Dim l_StrSQLSys As String

            Dim l_decSumaBase As Decimal
            Dim l_decSumaDestino As Decimal

            matrixPedidoVehiculos.Matrix.FlushToDataSource()


            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_strMonSistema = dtLocal.GetValue("SysCurrncy", 0)
            End If

            l_strMonOrigen = m_strMonOrigen
            l_strMonDestido = m_strMonDestino

            l_decTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(txtFhaPedi.ObtieneValorDataSource, "yyyyMMdd", Nothing))
            l_decTCDestino = ObtieneTipoCambio(l_strMonDestido, Date.ParseExact(txtFhaPedi.ObtieneValorDataSource, "yyyyMMdd", Nothing))

            Dim l_decPrecioLineas(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1) As Decimal
            Dim l_decTotalLineas(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1) As Decimal

            Dim l_decPrecioDestino(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1) As Decimal
            Dim l_decTotalDestino(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1) As Decimal

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                l_decPrecioLineas(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cost_Art", i).Trim, n)
                l_decTotalLineas(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cost_Tot", i).Trim, n)
            Next

            l_decSumaBase = Decimal.Parse(txtTotal.ObtieneValorDataSource, n)

            If l_strMonDestido = l_strMonOrigen Then

                For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                    l_decPrecioDestino(i) = l_decPrecioLineas(i)
                    l_decTotalDestino(i) = l_decTotalLineas(i)
                Next

                l_decSumaDestino = l_decSumaBase

            ElseIf l_strMonDestido <> l_strMonOrigen Then
                If l_decTCDestino = 0 Then
                    l_decTCDestino = 1
                End If
                If l_decTCOrigen = 0 Then
                    l_decTCOrigen = 1
                End If

                If l_strMonOrigen = l_strMonLocal Then

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                        l_decPrecioDestino(i) = l_decPrecioLineas(i) / l_decTCDestino
                        l_decTotalDestino(i) = l_decTotalLineas(i) / l_decTCDestino
                    Next

                    l_decSumaDestino = l_decSumaBase / l_decTCDestino

                ElseIf l_strMonDestido = l_strMonLocal Then

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                        l_decPrecioDestino(i) = l_decPrecioLineas(i) * l_decTCOrigen
                        l_decTotalDestino(i) = l_decTotalLineas(i) * l_decTCOrigen
                    Next
                    l_decSumaDestino = l_decSumaBase * l_decTCOrigen
                Else

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                        l_decPrecioDestino(i) = (l_decPrecioLineas(i) * l_decTCOrigen) / l_decTCDestino
                        l_decTotalDestino(i) = (l_decTotalLineas(i) * l_decTCOrigen) / l_decTCDestino
                    Next
                    l_decSumaDestino = (l_decSumaBase * l_decTCOrigen) / l_decTCDestino
                End If
            End If

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cost_Art", i, l_decPrecioDestino(i).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cost_Tot", i, l_decTotalDestino(i).ToString(n))
            Next

            txtTotal.AsignaValorDataSource(l_decSumaDestino.ToString(n))

            matrixPedidoVehiculos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal p_StrMoneda As String, ByVal p_strFecha As Date) As Decimal
        Try

            Dim l_decTipoC As Double


            Dim l_strSQLTipoC As String
            Dim l_strSQLProv As String
            Dim l_StrSQLSys As String

            Dim l_strMonLocal As String
            Dim l_StrMonSist As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"


            l_strSQLTipoC = String.Format(l_strSQLTipoC,
                                          Utilitarios.RetornaFechaFormatoDB(p_strFecha, _companySbo.Server),
                                          p_StrMoneda)
            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_strSQLTipoC)

            If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) Then
                l_decTipoC = -1
            Else
                l_decTipoC = dtLocal.GetValue("Rate", 0)
                'l_decTipoC = Double.Parse(dtLocal.GetValue("Rate", 0), n)
            End If


            Return l_decTipoC

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function


    Public Sub ActualizaMontosColumnas()
        Try

            Dim l_DecMonto As Decimal
            Dim l_decMontoLinea As Decimal
            Dim l_intCant As Integer
            Dim l_intCantRec As Integer
            Dim l_intCantPen As Integer
            Dim l_intCantTotal As Integer
            Dim l_intPendTotal As Integer
            Dim l_intReciTotal As Integer
            Dim l_DecTotal As Decimal

            matrixPedidoVehiculos.Matrix.FlushToDataSource()

            FormularioSBO.Freeze(True)

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                l_intCant = 0

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant", i)) Then
                    l_intCant = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant", i), n)
                End If
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Pen_Rec", i)) Then
                    l_intCantPen = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Pen_Rec", i), n)
                End If
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant_Rec", i)) Then
                    l_intCantRec = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant_Rec", i), n)
                End If

                l_intCantTotal = l_intCantTotal + l_intCant
                l_intCantPen = l_intCant - l_intCantRec


                l_DecMonto = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cost_Art", i), n)
                l_decMontoLinea = l_DecMonto * l_intCant
                l_DecTotal = l_DecTotal + l_decMontoLinea

                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cost_Tot", i, l_decMontoLinea.ToString(n))

            Next

            FormularioSBO.DataSources.DBDataSources.Item(m_strTablaPedidos).SetValue("U_Total_Doc", 0, l_DecTotal.ToString(n))

            matrixPedidoVehiculos.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub ActualizaCantidades()

        Dim l_intCantidadLinea As Integer
        Dim l_intPendienteLinea As Integer
        Dim l_intRecibidoLinea As Integer

        Dim l_intCantidadTotal As Integer
        Dim l_intPendienteTotal As Integer
        Dim l_intRecibidoTotal As Integer


        Try
            _formularioSBO.Freeze(True)
            matrixPedidoVehiculos.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size - 1
                l_intCantidadLinea = 0

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant", i)) Then
                    l_intCantidadLinea = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant", i), n)
                End If
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Pen_Rec", i)) Then
                    l_intPendienteLinea = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Pen_Rec", i), n)
                End If
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant_Rec", i)) Then
                    l_intRecibidoLinea = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant_Rec", i), n)
                End If

                If l_intRecibidoLinea = 0 Then
                    l_intPendienteLinea = l_intCantidadLinea
                    l_intRecibidoLinea = 0
                Else
                    l_intPendienteLinea = l_intCantidadLinea - l_intRecibidoLinea
                End If

                l_intCantidadTotal += l_intCantidadLinea
                l_intPendienteTotal += l_intPendienteLinea
                l_intRecibidoTotal += l_intRecibidoLinea

                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Pen_Rec", i, l_intPendienteLinea.ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cant_Rec", i, l_intRecibidoLinea.ToString(n))
            Next

            txtCant.AsignaValorDataSource(l_intCantidadTotal)
            txtCantRecibida.AsignaValorDataSource(l_intRecibidoTotal)
            txtCantPendiente.AsignaValorDataSource(l_intPendienteTotal)


            matrixPedidoVehiculos.Matrix.LoadFromDataSource()

            _formularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AgregarLineaPedido(Optional ByVal p_blnCarga As Boolean = False, Optional ByVal p_codigomarca As String = "")

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intSize As Integer

        oMatriz = DirectCast(oForm.Items.Item("mtx_Ped").Specific, SAPbouiCOM.Matrix)

        oMatriz.FlushToDataSource()

        'If matrixPedidoVehiculos.Matrix.RowCount = 0 Then
        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).InsertRecord(intSize)
        'Else
        '    intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size
        '    FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).InsertRecord(intSize)
        'End If

        intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size
        FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).InsertRecord(intSize)



        oMatriz.LoadFromDataSource()

    End Sub

    Public Function ValidarDatos(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try

            Dim l_Result As Boolean = True
            Dim l_intSize As Integer

            matrixPedidoVehiculos.Matrix.FlushToDataSource()

            l_intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size

            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse
                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                If String.IsNullOrEmpty(txtCodProv.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesNoTieneProveedor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                ElseIf l_intSize = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesSinLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                ElseIf String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cod_Art", 0).Trim) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesSinLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    l_Result = False
                    BubbleEvent = False
                End If

                If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cod_Art", l_intSize - 1).Trim) Then
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).RemoveRecord(l_intSize - 1)
                End If

            End If

            ' matrixPedidoVehiculos.Matrix.LoadFromDataSource()

            Return l_Result

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Function

    Private Function ValidarCancelar(ByRef pval As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try

            Dim l_blnRes As Boolean
            Dim l_strSQL As String
            Dim l_strPedido As String

            l_strSQL = " Select EV.DocEntry, EL.U_Num_Ped, EL.U_Cant_Ent "
            l_strSQL &= " from [@SCGD_ENTRADA_VEH] EV inner join [@SCGD_ENTRADA_LINEAS] EL on EV.DocEntry = el.DocEntry"
            l_strSQL &= " where EL.U_Num_Ped = '{0}' and EV.Canceled = 'N' and U_Cant_Ent > 0"

            l_strPedido = txtDocNum.ObtieneValorDataSource

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strPedido))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("DocEntry", 0)) Then
                If dtLocal.Rows.Count <> 0 AndAlso dtLocal.GetValue("DocEntry", 0) <> 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesNoCancelarPedido, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                Else
                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePedidoUnidadesCancelarPedido, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                        BubbleEvent = False
                    End If
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub AsignaValoresArticulos(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim oMat As SAPbouiCOM.Matrix


            matrixPedidoVehiculos.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cod_Art", pVal.Row - 1, oDataTable.GetValue("ItemCode", 0))
            FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Desc_Art", pVal.Row - 1, oDataTable.GetValue("ItemName", 0))
            matrixPedidoVehiculos.Matrix.LoadFromDataSource()

            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Ped").Specific, SAPbouiCOM.Matrix)
            oMat.Columns.Item("col_Cod").Cells.Item(pVal.Row).Click()

            AgregarLineaSiguente()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresTitular(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            FormularioSBO.Freeze(True)

            txtCodTitu.AsignaValorDataSource(oDataTable.GetValue("empID", 0))
            txtNamTitu.AsignaValorDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))

            FormularioSBO.Freeze(False)


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresProveedor(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim oitems As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim l_strSQL As String

        Try

            FormularioSBO.Freeze(True)

            txtCodProv.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
            txtDesProv.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()

            l_strSQL = "Select CntctCode, Name from OCPR	where CardCode = '{0}'"

            dtLocal.ExecuteQuery(String.Format(l_strSQL, oDataTable.GetValue("CardCode", 0)))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CntctCode", 0)) Then

                oitems = oForm.Items.Item(cboContac.UniqueId)
                oCombo = CType(oitems.Specific, SAPbouiCOM.ComboBox)

                If oCombo.ValidValues.Count <> 0 Then
                    For i As Integer = 0 To oCombo.ValidValues.Count - 1
                        oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                    Next
                End If

                For i As Integer = 0 To dtLocal.Rows.Count - 1
                    oCombo.ValidValues.Add(dtLocal.GetValue("CntctCode", i), dtLocal.GetValue("Name", i))
                Next
                cboContac.AsignaValorDataSource(dtLocal.GetValue("CntctCode", 0))
            End If

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If


            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AgregarPrimerLinea()
        Try
            Try

                _formularioSBO.Freeze(True)

                matrixPedidoVehiculos.Matrix.FlushToDataSource()
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cod_Art", 0, String.Empty)
                matrixPedidoVehiculos.Matrix.LoadFromDataSource()

                _formularioSBO.Freeze(False)

            Catch ex As Exception
                Utilitarios.ManejadorErrores(ex, _applicationSbo)
            End Try

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Public Sub AgregarLineaSiguente()

        Dim intSize As Integer

        Try
            _formularioSBO.Freeze(True)

            matrixPedidoVehiculos.Matrix.FlushToDataSource()
            intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size

            If Not String.IsNullOrEmpty(_formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue(matrixPedidoVehiculos.ColumnaCod.ColumnaLigada, intSize - 1).Trim) Then
                intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).Size
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).InsertRecord(intSize)
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).SetValue("U_Cod_Art", intSize, String.Empty)
           End If

            matrixPedidoVehiculos.Matrix.LoadFromDataSource()


            _formularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        Finally
            _formularioSBO.Freeze(False)
        End Try

    End Sub

    Public Sub EliminarLineaPedido()
        Try
            Dim intSelect As Integer
            Dim l_List As New List(Of Integer)()


            FormularioSBO.Freeze(True)
            matrixPedidoVehiculos.Matrix.FlushToDataSource()

            intSelect = matrixPedidoVehiculos.Matrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

            Do While intSelect > -1
                Dim test As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cod_Art", intSelect - 1)

                l_List.Add(intSelect)

                'FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).RemoveRecord(intSelect - 1)
                intSelect = matrixPedidoVehiculos.Matrix.GetNextSelectedRow(intSelect, BoOrderType.ot_RowOrder)

            Loop

            l_List.Reverse()
            Dim num As Integer = 0

            For Each num In l_List
                Dim test As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cod_Art", num - 1)
                FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).RemoveRecord(num - 1)
            Next

            matrixPedidoVehiculos.Matrix.LoadFromDataSource()

            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If

            FormularioSBO.Freeze(False)

            matrixPedidoVehiculos.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


#End Region

#Region "Eventos Formularios"

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)

            Case BoEventTypes.et_CHOOSE_FROM_LIST
                ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)

            Case BoEventTypes.et_COMBO_SELECT
                ManejadorEventoCombo(FormUID, pVal, BubbleEvent)

        End Select

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)

        Dim strCodProveedor As String
        Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim strCFL_Id As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim oDataTable As SAPbouiCOM.DataTable

        oCFLEvent = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        strCFL_Id = oCFLEvent.ChooseFromListUID
        oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)

        If oCFLEvent.ActionSuccess Then

            oDataTable = oCFLEvent.SelectedObjects

            If Not oCFLEvent.SelectedObjects Is Nothing Then

                If Not oDataTable Is Nothing And
                    _formularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

                    If pval.ItemUID = m_strMatPedidos Then
                        Select Case pval.ColUID

                            Case matrixPedidoVehiculos.ColumnaCod.UniqueId
                                AsignaValoresArticulos(FormUID, pval, oDataTable)

                        End Select
                    ElseIf pval.ItemUID = txtCodProv.UniqueId Then

                        m_strMonOrigen = cboMoneda.ObtieneValorDataSource()
                        AsignaValoresProveedor(FormUID, pval, oDataTable)
                        CargarMonedaSocio(txtCodProv.ObtieneValorDataSource)
                        m_strMonDestino = cboMoneda.ObtieneValorDataSource()

                        ManejaTipoCambio(BubbleEvent)
                        ManejoCambioDeMoneda()

                    ElseIf pval.ItemUID = txtNamTitu.UniqueId Then
                        AsignaValoresTitular(FormUID, pval, oDataTable)
                    End If
                End If
            End If

        ElseIf oCFLEvent.BeforeAction Then
            If pval.ItemUID = m_strMatPedidos Then
                Select Case pval.ColUID
                    Case "col_Cod"
                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "8"
                        oCondition.BracketCloseNum = 1
                        oCFL.SetConditions(oConditions)
                End Select
            ElseIf pval.ItemUID = "txtCodProv" Then

                oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add()

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardType"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL
                oCondition.CondVal = "S"
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "frozenFor"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = "Y"

                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)

                ' m_strMonOrigen = cboMoneda.ObtieneValorDataSource()


            End If
        End If
    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Try
            Dim key As String

            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "1"
                        If Not ValidarDatos(pVal, BubbleEvent) Then
                            BubbleEvent = False
                            Exit Sub
                        End If

                    Case btnMenos.UniqueId
                        If ValidarEliminarLineas(BubbleEvent) Then
                            BubbleEvent = False
                        End If
                        'EliminarLineaPedido(pVal, BubbleEvent)

                End Select
            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "1"
                        If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then
                            If BubbleEvent Then
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesCreado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)

                                FormularioSBO.Freeze(True)
                                CargarMonedaLocal()
                                CargarSerieDocumento()
                                txtFhaPedi.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))
                                FormularioSBO.Freeze(False)

                            End If
                        End If

                    Case btnMas.UniqueId
                        AgregarLineaSiguente()

                    Case btnMenos.UniqueId
                        EliminarLineaPedido()
                        ActualizaMontosColumnas()
                        ActualizaCantidades()
                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    Public Function ValidarEliminarLineas(ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim intSeleccionado As Integer
            Dim l_blnRest As Boolean = False
            Dim l_strDocEntry As String
            Dim l_strNumLinea As String
            Dim l_strCodArt As String
            Dim l_strCantSol As String
            Dim l_strCantRec As String
            Dim l_strCantPen As String
            Dim l_intRec As Integer
            Dim l_strLineId As String
            Dim l_List As New List(Of Integer)()


            matrixPedidoVehiculos.Matrix.FlushToDataSource()
            intSeleccionado = matrixPedidoVehiculos.Matrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

            If intSeleccionado = -1 Then
                _applicationSbo.SetStatusBarMessage("No hay lineas seleccionadas para eliminar", BoMessageTime.bmt_Short, True)
                BubbleEvent = False
                l_blnRest = True
            Else
                l_strDocEntry = txtDocEntry.ObtieneValorDataSource

                If Not String.IsNullOrEmpty(l_strDocEntry) Then


                    Do While intSeleccionado > -1



                        l_strCantSol = _formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant", intSeleccionado - 1).Trim
                        l_strCantRec = _formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cant_Rec", intSeleccionado - 1).Trim
                        l_strCantPen = _formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Pen_Rec", intSeleccionado - 1).Trim
                        l_strCodArt = _formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cod_Art", intSeleccionado - 1).Trim
                        l_strLineId = _formularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("LineId", intSeleccionado - 1).Trim

                        If Not String.IsNullOrEmpty(l_strCantRec) Then
                            l_intRec = Integer.Parse(l_strCantRec)
                        Else
                            l_intRec = 0
                        End If

                        If ValidaLineaPertenceAEntrada(l_strDocEntry, l_strLineId) Then
                            _applicationSbo.SetStatusBarMessage("No puede eliminar la linea, se encuentra asociada a una entrada.", BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                            l_blnRest = True
                            Exit Do
                        End If

                        intSeleccionado = matrixPedidoVehiculos.Matrix.GetNextSelectedRow(intSeleccionado, BoOrderType.ot_RowOrder)

                    Loop
                End If

            End If

            If ApplicationSBO.MessageBox("Desea eliminar las lineas seleccionadas del pedido", 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                BubbleEvent = False
                l_blnRest = True
            End If

            Return l_blnRest
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidaLineaPertenceAEntrada(ByVal p_strDocEntry As String, ByVal p_strLineId As String) As Boolean

        Try
            Dim l_strSQL As String
            Dim l_blnResult As Boolean = False

            dtLocal = _formularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()

            l_strSQL = "Select ent.DocEntry, LIN.LineId, LIN.VisOrder " &
                        " from [@SCGD_ENTRADA_LINEAS] LIN with (nolock)" &
                        " inner join [@SCGD_ENTRADA_VEH]  ENT with (nolock) on ENT.DocEntry = LIN.DocEntry" &
                        " where LIn.U_Num_Ped = '{0}' " &
                        " and LIN.U_Line_Ref = '{1}' " &
                        " and ENT.Canceled = 'N' " &
                        " and LIn.U_Cant_Ent > 0 "

            If Not String.IsNullOrEmpty(p_strDocEntry) AndAlso
                Not String.IsNullOrEmpty(p_strLineId) Then


                l_strSQL = String.Format(l_strSQL, p_strDocEntry, p_strLineId)
                dtLocal.ExecuteQuery(l_strSQL)


                If dtLocal.GetValue("DocEntry", 0) <> 0  Then
                    l_blnResult = True
                End If
            End If

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub ManejadorEventoCombo(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim l_tipo As String
            Dim strValor As String

            If pVal.ActionSuccess Then

                Select Case pVal.ItemUID


                    Case cboMoneda.UniqueId
                        m_strMonDestino = cboMoneda.ObtieneValorDataSource()

                        If ManejaTipoCambio(BubbleEvent) Then
                            ManejoCambioDeMoneda()
                        End If

                End Select

            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case cboMoneda.UniqueId
                        If String.IsNullOrEmpty(txtFhaPedi.ObtieneValorDataSource) Then
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajePedidoUnidadesSinFechaPedido, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        Else
                            m_strMonOrigen = cboMoneda.ObtieneValorDataSource()
                        End If



                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarMonedaLocal(Optional ByVal p_blnNuevo As Boolean = True)
        Try
            Dim l_StrSQLSys As String

            Dim l_strMonLocal As String
            Dim l_strMonSist As String

            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_strMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If

            FormularioSBO.Items.Item(cboMoneda.UniqueId).Visible = True
            FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = False

            If p_blnNuevo Then
                cboMoneda.AsignaValorDataSource(l_strMonLocal)
                txtTipoCam.AsignaValorDataSource(1)
            Else
                If cboMoneda.ObtieneValorDataSource <> l_strMonLocal Then
                    FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = True
                Else
                    FormularioSBO.Items.Item(txtTipoCam.UniqueId).Visible = False
                    txtTipoCam.AsignaValorDataSource(1)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventoValidate(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_intPedidas As Integer
            Dim l_intRecibidas As Integer

            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then

                If pVal.ItemUID = "mtx_Ped" Then
                    Select Case pVal.ColUID
                        Case matrixPedidoVehiculos.ColumnaCos.UniqueId, matrixPedidoVehiculos.ColumnaTot.UniqueId
                            ActualizaMontosColumnas()
                        Case matrixPedidoVehiculos.ColumnaCan.UniqueId
                            ActualizaMontosColumnas()
                            ActualizaCantidades()
                    End Select
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try

            If pval.BeforeAction Then
                Select Case pval.MenuUID
                    Case 1284
                        ValidarCancelar(pval, BubbleEvent)
                    Case "SCGD_PDV"
                        ValidaTipoCambio(BubbleEvent)
                End Select
            End If

            Dim oItem As SAPbouiCOM.Item


            Select Case pval.MenuUID

                Case 1281   'Buscar

                    If Not FormularioSBO Is Nothing Then
                        oForm = ApplicationSBO.Forms.Item("SCGD_PDV")

                        FormularioSBO.Freeze(True)
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next
                        FormularioSBO.Freeze(False)
                    End If

                Case 1282       'NUEVO
                    FormularioSBO.Freeze(True)

                    CargarMonedaLocal()
                    CargarSerieDocumento()
                    AgregarPrimerLinea()

                    txtFhaPedi.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

                    FormularioSBO.EnableMenu("1282", False)

                    If Not FormularioSBO Is Nothing Then
                        oForm = ApplicationSBO.Forms.Item("SCGD_PDV")

                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next

                    End If

                    FormularioSBO.Items.Item(txtDocNum.UniqueId).Enabled = False
                    FormularioSBO.Items.Item(cbxCancelado.UniqueId).Enabled = False


                    FormularioSBO.Freeze(False)
                    'Case Else
                    '    Call oForm.EnableMenu("1282", True)
                Case 1290, 1288, 1289, 1291
                    FormularioSBO.EnableMenu("1282", True)
            End Select

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try
            Dim l_strCodSucursal As String
            Dim l_strCotizacion As String
            Dim l_strCodVehiculo As String
            Dim l_strDocNum As String
            Dim l_strCardCode As String
            Dim oItem As SAPbouiCOM.Item

            l_strDocNum = txtDocNum.ObtieneValorDataSource()
            l_strCardCode = txtCodProv.ObtieneValorDataSource()

            'Call CargarMonedaSocio(l_strCardCode)
            'Call ObtieneTipoCambio(cboMoneda.ObtieneValorDataSource, )

            If cboEstado.ObtieneValorDataSource() = "C" Then
                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
            Else
                If Not FormularioSBO Is Nothing Then
                    oForm = ApplicationSBO.Forms.Item("SCGD_PDV")

                    FormularioSBO.Freeze(True)
                    For Each oItem In FormularioSBO.Items
                        oItem.Enabled = True
                    Next
                    FormularioSBO.Freeze(False)
                End If
                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                FormularioSBO.Items.Item(txtDocNum.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cboSeries.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cboEstado.UniqueId).Enabled = False
                FormularioSBO.Items.Item(cbxCancelado.UniqueId).Enabled = False

            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

#End Region









End Class


