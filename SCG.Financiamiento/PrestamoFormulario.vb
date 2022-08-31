Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO
Imports SCG.DMSOne.Framework
Imports SCG.SBOFramework.DI

'Clase para manejar funcionalidad de formulario de préstamo

Partial Public Class PrestamoFormulario

    Private m_blnEjecutarMetodo As Boolean
    Private m_blnCalculadoIntMora As Boolean
    Private m_decCuotaMora As Decimal
    Private m_decCapital As Decimal
    Private m_decInteres As Decimal
    Private m_intDiasInt As Integer
    Private m_decSaldoFinal As Decimal
    Private m_blnPermitirMoraMenor As Boolean
    Private m_dtFechaPagoCalculo As Date
    Private m_strCodPrestRev As String
    Private g_strFechaPago As String
    Private g_strMontoAbonar As String
    Private g_strPais As String
    Private g_strNBanco As String
    Private g_strSucursal As String
    Private g_strCuenta As String
    Private g_strNoCheque As String
    Private g_strEndoso As String
    Private g_intPosicion As Integer
    Private dbMontoAsientoRevalorizacion As Double

    Private g_strPrestamoBase As String

    Private g_strChequeAplicado As String
    'Private g_blnPlanAbierto As Boolean

    'Manejo de evento de Check Box de cancelar el cobro de intereses moratorios

    Public Sub CheckBoxSBOCancelarMoraItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

        Dim strCancelarMora As String

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            strCancelarMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0)
            strCancelarMora = strCancelarMora.Trim()

            If strCancelarMora = "Y" Then

                If CheckBoxCheque.ObtieneValorDataSource = "N" OrElse CheckBoxPagoDeuda.ObtieneValorUserDataSource().Trim = "Y" Then
                    m_blnCalculadoIntMora = True
                End If

                Call ValidarMoratorios()

            Else
                ButtonCalcular.ItemSBO.Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                If CheckBoxCheque.ObtieneValorDataSource = "Y" Then
                    FormularioSBO.Items.Item("chkCheq").Click()
                End If

                If CheckBoxPagoDeuda.ObtieneValorUserDataSource().Trim = "Y" Then
                    ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    ButtonCalcular.ItemSBO.Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Dim decMon As Decimal = EditTextMontoAbo.ObtieneValorUserDataSource
                    decMon += EditTextAboMor.ObtieneValorUserDataSource()
                    EditTextMontoAbo.AsignaValorUserDataSource(decMon)
                    ButtonCalcular.ItemSBO.Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

                End If

                m_blnCalculadoIntMora = False

            End If

        End If

    End Sub

    'Si se decide cancelar el cobro de intereses moratorios se pone en 0 el monto por mora y se mantiene el cobro de la cuota como si pagara en la fecha adecuada

    Private Sub ValidarMoratorios()

        Dim strNumero As String
        Dim intNumero As Integer = 0

        Dim strAbonoMoratorios As String
        Dim decAbonoMoratorios As Decimal
        Dim strAbonoCapital As String
        Dim decAbonoCapital As Decimal
        Dim strSaldoInicial As String
        Dim decSaldoInicial As Decimal
        Dim decSaldoRestante As Decimal

        Dim strEstado As String

        If m_blnCalculadoIntMora = True Then

            m_blnCalculadoIntMora = False

            m_blnPermitirMoraMenor = True

            strNumero = EditTextNumero.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(strNumero) Then intNumero = Integer.Parse(strNumero)

            strEstado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Estado", 0).Trim()

            If intNumero > 0 AndAlso strEstado = "1" Then
                If CheckBoxPagoDeuda.ObtieneValorUserDataSource.Trim = "N" Then


                    strAbonoMoratorios = EditTextAboMor.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strAbonoMoratorios) Then decAbonoMoratorios = Decimal.Parse(strAbonoMoratorios, n)

                    strAbonoCapital = EditTextAboCap.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strAbonoCapital) Then decAbonoCapital = Decimal.Parse(strAbonoCapital, n)

                    decAbonoCapital = decAbonoCapital + decAbonoMoratorios + EditTextRecargoCobranza.ObtieneValorUserDataSource()

                    EditTextAboCap.AsignaValorUserDataSource(decAbonoCapital.ToString(n))

                    strSaldoInicial = EditTextSalIni.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strSaldoInicial) Then decSaldoInicial = Decimal.Parse(strSaldoInicial, n)

                    decSaldoRestante = decSaldoInicial - decAbonoCapital

                    EditTextSalFin.AsignaValorUserDataSource(decSaldoRestante.ToString(n))

                    EditTextAboMor.AsignaValorUserDataSource("0")
                    EditTextDiasMora.AsignaValorUserDataSource("0")
                    EditTextRecargoCobranza.AsignaValorUserDataSource("0")
                Else
                    decAbonoCapital = EditTextMontoAbo.ObtieneValorUserDataSource - EditTextAboMor.ObtieneValorUserDataSource
                    EditTextMontoAbo.AsignaValorUserDataSource(decAbonoCapital)
                    EditTextAboMor.AsignaValorUserDataSource("0")
                    EditTextDiasMora.AsignaValorUserDataSource("0")
                    EditTextRecargoCobranza.AsignaValorUserDataSource("0")
                End If

            End If

        End If

    End Sub

    'Manejo de evento de botón de plan de pagos, tanto real como teórico, se consultan los datos generales y de la matriz de los planes de pago del préstamo

    'Public Sub ButtonSBOPlanItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

    '    Dim n As NumberFormatInfo

    '    Dim strCliente As String
    '    Dim strEnteFinanciero As String
    '    Dim strMoneda As String
    '    Dim intPlazo As Integer
    '    Dim strSaldo As String
    '    Dim decSaldo As Decimal
    '    Dim strFechaInicio As String
    '    Dim dtFechaInicio As Date
    '    Dim strIntNormal As String
    '    Dim decInteres As Decimal
    '    Dim strDiaPago As String
    '    Dim intDiaPago As Integer
    '    Dim strTipoCuota As String
    '    Dim strIntMora As String
    '    Dim decIntMora As Decimal
    '    Dim intDifDiasInicio As Integer
    '    Dim strTipoPlan As String = ""
    '    Dim strPrestamo As String
    '    Dim strPrecioVenta As String
    '    Dim decPrecioVenta As Decimal
    '    Dim strPrima As String
    '    Dim decPrima As Decimal

    '    n = DIHelper.GetNumberFormatInfo(CompanySBO)

    '    If pVal.ActionSuccess Then

    '        strCliente = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Cli", 0)
    '        strCliente = strCliente.Trim()
    '        strEnteFinanciero = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Ent_Fin", 0)
    '        strEnteFinanciero = strEnteFinanciero.Trim()
    '        strMoneda = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Moneda", 0)
    '        strMoneda = strMoneda.Trim()
    '        intPlazo = Plazo(FormularioSBO)

    '        strSaldo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Mon_Fin", 0)
    '        strSaldo = strSaldo.Trim()
    '        If Not String.IsNullOrEmpty(strSaldo) Then
    '            decSaldo = Decimal.Parse(strSaldo, n)
    '        End If

    '        strFechaInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0)
    '        strFechaInicio = strFechaInicio.Trim()
    '        If Not String.IsNullOrEmpty(strFechaInicio) Then
    '            dtFechaInicio = Date.ParseExact(strFechaInicio, "yyyyMMdd", Nothing)
    '            dtFechaInicio = New Date(dtFechaInicio.Year, dtFechaInicio.Month, dtFechaInicio.Day, 0, 0, 0)
    '        End If

    '        strIntNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0)
    '        strIntNormal = strIntNormal.Trim()
    '        If Not String.IsNullOrEmpty(strIntNormal) Then
    '            decInteres = Decimal.Parse(strIntNormal, n)
    '        End If

    '        strDiaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_DiaPago", 0)
    '        strDiaPago = strDiaPago.Trim()
    '        If Not String.IsNullOrEmpty(strDiaPago) Then
    '            intDiaPago = Integer.Parse(strDiaPago)
    '        Else
    '            intDiaPago = 0
    '        End If
    '        strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Tipo", 0)
    '        strTipoCuota = strTipoCuota.Trim()

    '        strIntMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0)
    '        strIntMora = strIntMora.Trim()
    '        If Not String.IsNullOrEmpty(strIntMora) Then
    '            decIntMora = Decimal.Parse(strIntMora, n)
    '        End If

    '        strPrecioVenta = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Pre_Vta", 0)
    '        strPrecioVenta = strPrecioVenta.Trim()
    '        If Not String.IsNullOrEmpty(strPrecioVenta) Then
    '            decPrecioVenta = Decimal.Parse(strPrecioVenta, n)
    '        End If
    '        strPrima = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Prima", 0)
    '        strPrima = strPrima.Trim()
    '        If Not String.IsNullOrEmpty(strPrima) Then
    '            decPrima = Decimal.Parse(strPrima, n)
    '        End If

    '        If Not String.IsNullOrEmpty(strCliente) AndAlso Not String.IsNullOrEmpty(strEnteFinanciero) AndAlso decSaldo > 0 AndAlso intPlazo > 0 AndAlso Not String.IsNullOrEmpty(strFechaInicio) _
    '            AndAlso Not String.IsNullOrEmpty(strMoneda) AndAlso Not String.IsNullOrEmpty(strIntNormal) AndAlso intDiaPago > 0 AndAlso Not String.IsNullOrEmpty(strTipoCuota) AndAlso Not String.IsNullOrEmpty(strIntMora) Then

    '            '_formPlanPlagos.StrConexion = StrConexion

    '            'If Not General.FormularioAbierto(_formPlanPlagos, True, _applicationSbo) Then

    '            '    'g_blnPlanAbierto = True

    '            '    _formPlanPlagos.FormularioSBO = CargaFormulario(_formPlanPlagos)

    '            '    strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0)
    '            '    strPrestamo = strPrestamo.Trim()

    '            '    _formPlanPlagos.dataTablePlanes = _formPlanPlagos.FormularioSBO.DataSources.DataTables.Item("Planes")
    '            '    Dim strConsulta As String = ""
    '            '    If pVal.ItemUID = ButtonTeorico.UniqueId Then

    '            '        strConsulta = "Select U_Numero, U_Fecha, U_Sal_Ini, U_Cuota, U_Capital, U_Interes, U_Sal_Fin From [@SCGD_PLAN_TEORICO] Where DocEntry = '" & strPrestamo & "'"

    '            '        strTipoPlan = "T"

    '            '    ElseIf pVal.ItemUID = ButtonReal.UniqueId Then

    '            '        strConsulta = "Select U_Numero, U_Fecha, U_Sal_Ini, U_Cuota, U_Capital, U_Interes, U_Sal_Fin, U_Int_Mora, U_Pagado, U_Cred_Cap, U_Doc_Int, U_DocFac, U_BorrPag, U_Cap_Pend, U_Int_Pend, U_Mor_Pend, U_Dias_Int, U_Dias_Mor From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "'"

    '            '        strTipoPlan = "R"

    '            '    End If

    '            '    _formPlanPlagos.dataTablePlanes.ExecuteQuery(strConsulta)

    '            '    For i As Integer = 0 To _formPlanPlagos.dataTablePlanes.Rows.Count - 1

    '            '        ReDim Preserve _formPlanPlagos.g_intNumero(i)
    '            '        _formPlanPlagos.g_intNumero(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Numero", i)
    '            '        ReDim Preserve _formPlanPlagos.g_dtFechaPago(i)
    '            '        _formPlanPlagos.g_dtFechaPago(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Fecha", i)
    '            '        ReDim Preserve _formPlanPlagos.g_decSaldoInicial(i)
    '            '        _formPlanPlagos.g_decSaldoInicial(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Sal_Ini", i)
    '            '        ReDim Preserve _formPlanPlagos.g_decCuota(i)
    '            '        _formPlanPlagos.g_decCuota(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Cuota", i)
    '            '        ReDim Preserve _formPlanPlagos.g_decCapital(i)
    '            '        _formPlanPlagos.g_decCapital(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Capital", i)
    '            '        ReDim Preserve _formPlanPlagos.g_decInteres(i)
    '            '        _formPlanPlagos.g_decInteres(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Interes", i)
    '            '        ReDim Preserve _formPlanPlagos.g_decSaldoFinal(i)
    '            '        _formPlanPlagos.g_decSaldoFinal(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Sal_Fin", i)
    '            '        If pVal.ItemUID = ButtonTeorico.UniqueId Then

    '            '            ReDim Preserve _formPlanPlagos.g_decMoratorios(i)
    '            '            _formPlanPlagos.g_decMoratorios(i) = 0
    '            '            ReDim Preserve _formPlanPlagos.g_strPagado(i)
    '            '            _formPlanPlagos.g_strPagado(i) = "N"
    '            '            ReDim Preserve _formPlanPlagos.g_strNotaCred(i)
    '            '            _formPlanPlagos.g_strNotaCred(i) = ""
    '            '            ReDim Preserve _formPlanPlagos.g_strDocInt(i)
    '            '            _formPlanPlagos.g_strDocInt(i) = ""
    '            '            ReDim Preserve _formPlanPlagos.g_strDocFac(i)
    '            '            _formPlanPlagos.g_strDocFac(i) = ""
    '            '            ReDim Preserve _formPlanPlagos.g_strBorrador(i)
    '            '            _formPlanPlagos.g_strBorrador(i) = ""
    '            '            ReDim Preserve _formPlanPlagos.g_decCapPend(i)
    '            '            _formPlanPlagos.g_decCapPend(i) = 0
    '            '            ReDim Preserve _formPlanPlagos.g_decIntPend(i)
    '            '            _formPlanPlagos.g_decIntPend(i) = 0
    '            '            ReDim Preserve _formPlanPlagos.g_decMoraPend(i)
    '            '            _formPlanPlagos.g_decMoraPend(i) = 0
    '            '            ReDim Preserve _formPlanPlagos.g_intDiasMora(i)
    '            '            _formPlanPlagos.g_intDiasMora(i) = 0
    '            '            ReDim Preserve _formPlanPlagos.g_intDiasInt(i)
    '            '            If i = 0 AndAlso intDiaPago > dtFechaInicio.Day Then
    '            '                intDifDiasInicio = intDiaPago - dtFechaInicio.Day
    '            '                _formPlanPlagos.g_intDiasInt(i) = 30 + intDifDiasInicio
    '            '            Else
    '            '                _formPlanPlagos.g_intDiasInt(i) = 30
    '            '            End If

    '            '        ElseIf pVal.ItemUID = ButtonReal.UniqueId Then

    '            '            ReDim Preserve _formPlanPlagos.g_decMoratorios(i)
    '            '            _formPlanPlagos.g_decMoratorios(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Int_Mora", i)
    '            '            ReDim Preserve _formPlanPlagos.g_strPagado(i)
    '            '            _formPlanPlagos.g_strPagado(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Pagado", i)
    '            '            ReDim Preserve _formPlanPlagos.g_strNotaCred(i)
    '            '            _formPlanPlagos.g_strNotaCred(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Cred_Cap", i)
    '            '            ReDim Preserve _formPlanPlagos.g_strDocInt(i)
    '            '            _formPlanPlagos.g_strDocInt(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Doc_Int", i)
    '            '            ReDim Preserve _formPlanPlagos.g_strDocFac(i)
    '            '            _formPlanPlagos.g_strDocFac(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_DocFac", i)
    '            '            ReDim Preserve _formPlanPlagos.g_strBorrador(i)
    '            '            _formPlanPlagos.g_strBorrador(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_BorrPag", i)
    '            '            ReDim Preserve _formPlanPlagos.g_decCapPend(i)
    '            '            _formPlanPlagos.g_decCapPend(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Cap_Pend", i)
    '            '            ReDim Preserve _formPlanPlagos.g_decIntPend(i)
    '            '            _formPlanPlagos.g_decIntPend(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Int_Pend", i)
    '            '            ReDim Preserve _formPlanPlagos.g_decMoraPend(i)
    '            '            _formPlanPlagos.g_decMoraPend(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Mor_Pend", i)
    '            '            ReDim Preserve _formPlanPlagos.g_intDiasInt(i)
    '            '            _formPlanPlagos.g_intDiasInt(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Dias_Int", i)
    '            '            ReDim Preserve _formPlanPlagos.g_intDiasMora(i)
    '            '            _formPlanPlagos.g_intDiasMora(i) = _formPlanPlagos.dataTablePlanes.GetValue("U_Dias_Mor", i)

    '            '        End If

    '            '    Next

    '            '    Call _formPlanPlagos.CargarPlanPagos(strCliente, strEnteFinanciero, decSaldo, intPlazo, dtFechaInicio, strMoneda, decInteres, decIntMora, strTipoCuota, decPrecioVenta, decPrima, True, strPrestamo, strTipoPlan)
    '            '    Call _formPlanPlagos.CargarColumnasPlanPagos(_formPlanPlagos.dataTablePlanes.Rows.Count)

    '            'End If

    '        Else

    '            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargarPlan, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)

    '        End If

    '    End If

    'End Sub

    'Manejo de evento de botón actualizar, se maneja en caso de que el tipo de cuota utilizada sea variable y cambie la tasa de interes normal anual

    Public Sub ButtonSBOActualizarItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim udoPrestamo As UDOPrestamo

        If pVal.BeforeAction = True Then

            Select Case FormularioSBO.Mode
                Case BoFormMode.fm_UPDATE_MODE
                    'If Not m_blnEjecutarMetodo = False Then

                    Dim n As NumberFormatInfo

                    Dim strIntNormal As String
                    Dim decIntNormal As Decimal = 0
                    Dim strNumero As String
                    Dim intNumero As Integer = 0
                    Dim strFechaTeorica As String = ""
                    Dim strFechaTeoricaFormateada As String = ""
                    Dim dtFechaTeorica As Date
                    Dim strTipoCuota As String
                    Dim strPrestamo As String
                    Dim strFormato As String

                    n = DIHelper.GetNumberFormatInfo(CompanySBO)

                    strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0)
                    strPrestamo = strPrestamo.Trim()

                    strIntNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0)
                    strIntNormal = strIntNormal.Trim()
                    If Not String.IsNullOrEmpty(strIntNormal) Then
                        decIntNormal = Decimal.Parse(strIntNormal, n)
                        decIntNormal = decIntNormal / 100
                    End If

                    If EditTextNumero.ObtieneValorUserDataSource.Trim <> "1" Then

                        For index = 0 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Size - 1
                            If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", index).Trim = "N" OrElse FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", index).Trim = "P" Then
                                intNumero = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Numero", index).Trim
                                Exit For
                            End If
                        Next
                    Else
                        intNumero = EditTextNumero.ObtieneValorUserDataSource.Trim
                    End If
                    If intNumero >= 1 Then
                        dtFechaTeorica = Date.ParseExact(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim(), "yyyyMMdd", Nothing)
                        strFechaTeorica = dtFechaTeorica
                    End If

                    strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0)
                    strTipoCuota = strTipoCuota.Trim()

                    If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE AndAlso FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Estado", 0) <> 2 Then

                        If String.IsNullOrEmpty(strPrestamo) OrElse String.IsNullOrEmpty(strIntNormal) OrElse intNumero = 0 OrElse String.IsNullOrEmpty(strFechaTeorica) OrElse String.IsNullOrEmpty(strTipoCuota) Then
                            BubbleEvent = False
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorActualizar, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

                        Else
                            'If Not g_blnPlanAbierto Then
                            '    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPlanPagos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            '    BubbleEvent = False
                            'End If

                            Call CalculoCuotasIntNormal(intNumero, strTipoCuota, decIntNormal, dtFechaTeorica)

                            m_blnCalculadoIntMora = False

                        End If

                    End If
                    ManejarEstadoPrestamo()

                    m_blnEjecutarMetodo = True

                Case BoFormMode.fm_ADD_MODE
                    Dim dbMontoAsientoRevalorizacionLocal As Double = 0
                    Dim intDocEntryJE As Integer = -1
                    g_strPrestamoBase = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_PreBa", 0).Trim()
                    If String.IsNullOrEmpty(g_strPrestamoBase) Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCreacionPrestamo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                        BubbleEvent = False
                    ElseIf IsNumeric(Convert.ToDouble(EditTextMontoFin.ObtieneValorDataSource(), n)) Then
                        If Convert.ToDouble(EditTextMontoFin.ObtieneValorDataSource(), n) <> dbMontoAsientoRevalorizacion Then dbMontoAsientoRevalorizacionLocal = Convert.ToDouble(EditTextMontoFin.ObtieneValorDataSource(), n) - dbMontoAsientoRevalorizacion
                        If dbMontoAsientoRevalorizacionLocal <> 0 Then
                            BubbleEvent = CrearAsientoRevalorizacion(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Moneda", 0).Trim(), _
                                                               Convert.ToDouble(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_PreBa", 0), n), _
                                                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Cli", 0).Trim(), _
                                                                  dbMontoAsientoRevalorizacionLocal, intDocEntryJE)
                            If intDocEntryJE <> -1 Then
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_AsientoRe", 0, intDocEntryJE)
                            Else
                                dbMontoAsientoRevalorizacion = 0
                            End If
                        End If

                    Else
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MontoNoValido, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                        BubbleEvent = False
                    End If
            End Select


        End If

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            Select Case FormularioSBO.Mode
                Case BoFormMode.fm_UPDATE_MODE

                    Dim strTipoCuo As String

                    strTipoCuo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0)
                    strTipoCuo = strTipoCuo.Trim()

                    If strTipoCuo = "3" Then

                        FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                        CheckBoxCancelarMora.ItemSBO.Enabled = False
                        CheckBoxPagoDeuda.ItemSBO.Enabled = False
                        EditTextMontoAbo.ItemSBO.Enabled = False

                    End If

                    If strTipoCuo = "1" Then

                        EditTextIntNormal.ItemSBO.Enabled = False

                    End If

                Case BoFormMode.fm_ADD_MODE

                    Dim oCompanyService As SAPbobsCOM.CompanyService
                    Dim oGeneralService As SAPbobsCOM.GeneralService
                    Dim oGeneralData As SAPbobsCOM.GeneralData
                    Dim oGeneralDataParams As SAPbobsCOM.GeneralDataParams
                    Dim oDataChildsReal As SAPbobsCOM.GeneralDataCollection
                    Dim oChildReal As SAPbobsCOM.GeneralData
                    Dim oDataChildsTeorico As SAPbobsCOM.GeneralDataCollection
                    Dim oChildTeorico As SAPbobsCOM.GeneralData
                    Dim decIntNormalUDO As Decimal
                    Dim intPlazoUDO As Integer
                    Dim decMontoFinUDO As Decimal
                    Dim dtFechaPrestUDO As Date
                    Dim intDiaPagoUDO As Integer
                    Dim strTipoCuotaUDO As String
                    Dim strDocEntryUDO As String
                    Dim intDocEntry As Integer = 0

                    m_formPlanPlagos = New PlanPagosFormulario(ApplicationSBO, CompanySBO, StrUsuarioBD, StrContraseñaBD, My.Resources.Resource.XMLPlanPagos)

                    strDocEntryUDO = General.EjecutarConsulta(" select MAX(DocEntry) from [@SCGD_PRESTAMO] ", StrConexion).Trim()
                    If Not String.IsNullOrEmpty(strDocEntryUDO) Then
                        intDocEntry = Integer.Parse(strDocEntryUDO)
                    End If
                    If Not String.IsNullOrEmpty(g_strPrestamoBase) Then
                        ActualizaPrestamoBase(g_strPrestamoBase)
                        g_strPrestamoBase = String.Empty
                    End If
                    If intDocEntry > 0 Then

                        oCompanyService = CompanySBO.GetCompanyService()
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")

                        oGeneralDataParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)

                        oGeneralDataParams.SetProperty("DocEntry", intDocEntry)
                        oGeneralData = oGeneralService.GetByParams(oGeneralDataParams)

                        decIntNormalUDO = oGeneralData.GetProperty("U_Interes")
                        intPlazoUDO = oGeneralData.GetProperty("U_Plazo")
                        decMontoFinUDO = oGeneralData.GetProperty("U_Mon_Fin")
                        dtFechaPrestUDO = oGeneralData.GetProperty("U_Fec_Pres")
                        intDiaPagoUDO = oGeneralData.GetProperty("U_DiaPago")
                        strTipoCuotaUDO = oGeneralData.GetProperty("U_Tipo_Cuo")

                        decIntNormalUDO = decIntNormalUDO / 100

                        If strTipoCuotaUDO = "1" OrElse strTipoCuotaUDO = "2" Then
                            Call m_formPlanPlagos.CalculoNivelada(intPlazoUDO, decMontoFinUDO, decIntNormalUDO, dtFechaPrestUDO, intDiaPagoUDO, False, "N")
                        ElseIf strTipoCuotaUDO = "4" Then
                            Call m_formPlanPlagos.CalculoDecreciente(intPlazoUDO, decMontoFinUDO, decIntNormalUDO, dtFechaPrestUDO, intDiaPagoUDO, "N")
                        ElseIf strTipoCuotaUDO = "3" Then
                            Call m_formPlanPlagos.CalculoGlobal(intPlazoUDO, decMontoFinUDO, decIntNormalUDO, dtFechaPrestUDO, intDiaPagoUDO)
                        End If

                        oDataChildsReal = oGeneralData.Child("SCGD_PLAN_REAL")
                        oDataChildsTeorico = oGeneralData.Child("SCGD_PLAN_TEORICO")

                        For i As Integer = 0 To intPlazoUDO - 1
                            If i > 0 Then
                                oChildTeorico = oDataChildsTeorico.Add()
                            Else
                                oChildTeorico = oDataChildsTeorico.Item(0)
                            End If
                            oChildTeorico.SetProperty("U_Numero", m_formPlanPlagos.g_intNumero(i))
                            oChildTeorico.SetProperty("U_Fecha", m_formPlanPlagos.g_dtFechaPago(i))
                            oChildTeorico.SetProperty("U_Sal_Ini", m_formPlanPlagos.g_decSaldoInicial(i).ToString(n))
                            oChildTeorico.SetProperty("U_Cuota", m_formPlanPlagos.g_decCuota(i).ToString(n))
                            oChildTeorico.SetProperty("U_Capital", m_formPlanPlagos.g_decCapital(i).ToString(n))
                            oChildTeorico.SetProperty("U_Interes", m_formPlanPlagos.g_decInteres(i).ToString(n))
                            oChildTeorico.SetProperty("U_Sal_Fin", m_formPlanPlagos.g_decSaldoFinal(i).ToString(n))

                            If i > 0 Then
                                oChildReal = oDataChildsReal.Add()
                            Else
                                oChildReal = oDataChildsReal.Item(0)
                            End If
                            oChildReal.SetProperty("U_Numero", m_formPlanPlagos.g_intNumero(i))
                            oChildReal.SetProperty("U_Fecha", m_formPlanPlagos.g_dtFechaPago(i))
                            oChildReal.SetProperty("U_Sal_Ini", m_formPlanPlagos.g_decSaldoInicial(i).ToString(n))
                            oChildReal.SetProperty("U_Cuota", m_formPlanPlagos.g_decCuota(i).ToString(n))
                            oChildReal.SetProperty("U_Capital", m_formPlanPlagos.g_decCapital(i).ToString(n))
                            oChildReal.SetProperty("U_Interes", m_formPlanPlagos.g_decInteres(i).ToString(n))
                            oChildReal.SetProperty("U_Int_Mora", m_formPlanPlagos.g_decMoratorios(i).ToString(n))
                            oChildReal.SetProperty("U_Sal_Fin", m_formPlanPlagos.g_decSaldoFinal(i).ToString(n))
                            oChildReal.SetProperty("U_Pagado", m_formPlanPlagos.g_strPagado(i).ToString(n))
                            oChildReal.SetProperty("U_Cap_Pend", m_formPlanPlagos.g_decCapPend(i).ToString(n))
                            oChildReal.SetProperty("U_Int_Pend", m_formPlanPlagos.g_decIntPend(i).ToString(n))
                            oChildReal.SetProperty("U_Mor_Pend", m_formPlanPlagos.g_decMoraPend(i).ToString(n))
                            oChildReal.SetProperty("U_Dias_Int", m_formPlanPlagos.g_intDiasInt(i))
                            oChildReal.SetProperty("U_Dias_Mor", m_formPlanPlagos.g_intDiasMora(i))

                        Next
                    End If

                    oGeneralService.Update(oGeneralData)
                    ManejaControlesRevalorización(False)
                    ManejaControlesChequesPostFechados(True, False, True)

            End Select


        End If

    End Sub

    ''' <summary>
    ''' Funcion que crea Asiento de revalorizacion por tema de diferencia al capital
    ''' </summary>
    ''' <param name="strMoneda">Moneda del prestamo</param>
    ''' <param name="intNumPrestamo">Numero de prestamo</param>
    ''' <param name="strCodCli">Codigo del cliente</param>
    ''' <param name="dbMontoAbono">Monto del abono</param>
    ''' <param name="intDocEntryJE">Numero de asiento creado</param>
    ''' <returns>Booleano con valor sobre creacion del asiento</returns>
    ''' <remarks></remarks>
    Private Function CrearAsientoRevalorizacion(ByVal strMoneda As String, ByVal intNumPrestamo As Integer, ByVal strCodCli As String, ByVal dbMontoAbono As Double, ByRef intDocEntryJE As Integer) As Boolean

        Dim m_strMonedaLocal = General.RetornarMonedaLocal(CompanySBO)
        Dim sStr As String
        Dim vRs As SAPbobsCOM.Recordset
        Dim vBOB As SAPbobsCOM.SBObob
        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim oBusinessPartners As SAPbobsCOM.BusinessPartners

        Try
            vBOB = CompanySBO.GetBusinessObject(BoObjectTypes.BoBridge)
            vRs = CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset)
            oBusinessPartners = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oJournalEntry = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.ReferenceDate = Date.Now

            oJournalEntry.Memo = String.Format(My.Resources.Resource.ReferenciaAsientoRevalorizacion, intNumPrestamo)

            If strMoneda = m_strMonedaLocal Then
                If Not String.IsNullOrEmpty(dataTableConsulta.GetValue("U_AsRe_Loc", 0)) Then
                    vRs = vBOB.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", dataTableConsulta.GetValue("U_AsRe_Loc", 0), BoQueryConditions.bqc_Equal)
                Else
                    ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.CuentaNoDefinida)
                    Return False
                End If
            Else
                If Not String.IsNullOrEmpty(dataTableConsulta.GetValue("U_AsRe_Sis", 0)) Then
                    vRs = vBOB.GetObjectKeyBySingleValue(BoObjectTypes.oChartOfAccounts, "FormatCode", dataTableConsulta.GetValue("U_AsRe_Sis", 0), BoQueryConditions.bqc_Equal)
                Else
                    ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.CuentaNoDefinida)
                    Return False
                End If
            End If

            sStr = vRs.Fields.Item(0).Value

            oJournalEntry.Lines.AccountCode = sStr
            oJournalEntry.Lines.Reference1 = String.Format(My.Resources.Resource.ReferenciaAsientoRevalorizacionLinea, intNumPrestamo)
            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Credit = dbMontoAbono
            Else
                oJournalEntry.Lines.FCCredit = dbMontoAbono
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If

            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            oJournalEntry.Lines.Add()

            'Linea SN
            vRs = vBOB.GetObjectKeyBySingleValue(BoObjectTypes.oBusinessPartners, "CardCode", strCodCli, BoQueryConditions.bqc_Equal)
            sStr = vRs.Fields.Item(0).Value

            oJournalEntry.Lines.ShortName = sStr
            oJournalEntry.Lines.Reference1 = String.Format(My.Resources.Resource.ReferenciaAsientoRevalorizacionLinea, intNumPrestamo)
            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Debit = dbMontoAbono
            Else
                oJournalEntry.Lines.FCDebit = dbMontoAbono
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If

            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            oJournalEntry.Lines.Add()

            If oJournalEntry.Add = 0 Then
                CompanySBO.GetNewObjectCode(intDocEntryJE)
                Return True
            Else
                Dim intError As Integer
                Dim strMensajeError As String
                CompanySBO.GetLastError(intError, strMensajeError)
                ApplicationSBO.SetStatusBarMessage(strMensajeError, BoMessageTime.bmt_Short, True)
                intDocEntryJE = -1
                Return False
            End If

        Catch ex As Exception

            ApplicationSBO.SetStatusBarMessage(ex.Message)
            Return False

        Finally
            General.DestruirObjeto(oJournalEntry)
            General.DestruirObjeto(vRs)
        End Try

    End Function

    Private Sub ActualizaPrestamoBase(ByVal p_strDocEntryBase As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralDataParams As SAPbobsCOM.GeneralDataParams
        Dim intDocEntry As Integer

        Try
            If Not String.IsNullOrEmpty(p_strDocEntryBase) Then
                intDocEntry = Integer.Parse(p_strDocEntryBase)
            End If
            oCompanyService = CompanySBO.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")

            oGeneralDataParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

            oGeneralDataParams.SetProperty("DocEntry", p_strDocEntryBase)
            oGeneralData = oGeneralService.GetByParams(oGeneralDataParams)

            oGeneralData.SetProperty("U_Reval", "Y")
            oGeneralData.SetProperty("U_Estado", "3")
            oGeneralData.SetProperty("U_Des_Est", My.Resources.Resource.EstadoCancelado)

            oGeneralService.Update(oGeneralData)

        Catch ex As Exception

        End Try
    End Sub

    'Manejo de evento de botón de reversar pagos, se valida que los pagos a reversar no tengan depósitos asociados, que el período contable esté abierto y el tipo de cambio exista para fecha de reversión

    Public Sub ButtonSBOReversarItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim intLineaPago As Integer
        Dim strEstadoPeriodo As String
        Dim dtFechaReversion As Date
        Dim strMonedaSistema As String
        Dim strMonedaLocal As String
        Dim strTipoCambio As String
        Dim dataTablePagosReversar As SAPbouiCOM.DataTable
        Dim strNumeroPago As String
        Dim strPagoRecibido As String
        Dim strPagos As String = ""
        Dim intPosicion As Integer = 0
        Dim strConsulta As String
        Dim strPago As String = ""
        Dim strPrestamo As String

        intLineaPago = MatrixPagosReversar.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

        If pVal.BeforeAction = True Then

            If intLineaPago = -1 Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionPago, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            Else

                'Validación de depositos asociados a los pagos

                strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocNum", 0).Trim()

                dataTablePagosReversar = FormularioSBO.DataSources.DataTables.Item("ReversadosMatrix")

                For i As Integer = intLineaPago - 1 To MatrixPagosReversar.Matrix.RowCount - 1

                    strNumeroPago = dataTablePagosReversar.GetValue("numero", i)

                    strPagoRecibido = General.EjecutarConsulta("Select U_Cred_Cap from [dbo].[@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & strNumeroPago & "'", StrConexion)

                    If Not String.IsNullOrEmpty(strPagoRecibido) Then

                        If intPosicion = 0 Then

                            strPagos = strPagoRecibido

                        Else

                            strPagos = strPagos & "," & strPagoRecibido

                        End If

                        intPosicion += 1

                    End If

                Next

                If Not String.IsNullOrEmpty(strPagos) Then

                    strConsulta = "Select RcptNum As Pago, CheckKey As Valor From [OCHH] Where RcptNum IN(" & strPagos & ") And Deposited = 'C'"

                    Call EjecutaConsultaValidacion(BubbleEvent, strPago, strConsulta)

                    If BubbleEvent = False Then

                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorReversionPagos & strPago & My.Resources.Resource.ErrorDepositos, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        Exit Sub

                    End If

                    strConsulta = "Select RctAbs As Pago, AbsId As Valor From [OCRH] Where RctAbs IN(" & strPagos & ") And Deposited = 'Y'"

                    Call EjecutaConsultaValidacion(BubbleEvent, strPago, strConsulta)

                    If BubbleEvent = False Then

                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorReversionPagos & strPago & My.Resources.Resource.ErrorDepositos, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        Exit Sub

                    End If

                End If


                dtFechaReversion = Now.Date

                strEstadoPeriodo = General.EjecutarConsulta("SELECT PeriodStat FROM dbo.[OFPR] WHERE '" & dtFechaReversion.ToString("yyyyMMdd") & "' >= F_RefDate AND '" & dtFechaReversion.ToString("yyyyMMdd") & "' <= T_RefDate", StrConexion)

                If strEstadoPeriodo <> "N" Then

                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorPeriodoContableReversion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    FormularioSBO.Refresh()
                    Exit Sub

                End If

                strMonedaSistema = General.RetornarMonedaSistema(_companySbo)
                strMonedaLocal = General.RetornarMonedaLocal(_companySbo)

                If strMonedaLocal.Trim() <> strMonedaSistema.Trim() Then
                    strTipoCambio = General.EjecutarConsulta("SELECT Rate FROM ORTT WHERE Currency = '" & strMonedaSistema & "' AND RateDate='" & dtFechaReversion.ToString("yyyyMMdd") & "'", StrConexion)

                    If String.IsNullOrEmpty(strTipoCambio) Then

                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambio, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        FormularioSBO.Refresh()
                        Exit Sub

                    End If

                End If

            End If

        ElseIf pVal.BeforeAction = False Then

            Call ReversarPagos(intLineaPago)

        End If

    End Sub

    'Ejecuta consulta de validación para determinar si los pagos recibidos tienen depósitos asociados

    Private Sub EjecutaConsultaValidacion(ByRef blnReversar As Boolean, ByRef strPago As String, ByVal strConsulta As String)

        Dim strPagoDT As String
        Dim intPagoDT As Integer = 0
        Dim strValor As String

        Try

            dataTableDepositos.Rows.Clear()
            dataTableDepositos = FormularioSBO.DataSources.DataTables.Item("Depositos")

            dataTableDepositos.ExecuteQuery(strConsulta)

            If dataTableDepositos.Rows.Count > 0 Then

                For i As Integer = 0 To dataTableDepositos.Rows.Count - 1

                    strPagoDT = dataTableDepositos.GetValue("Pago", i)

                    If Not String.IsNullOrEmpty(strPagoDT) Then

                        intPagoDT = Integer.Parse(strPagoDT)

                        If intPagoDT > 0 Then

                            strValor = dataTableDepositos.GetValue("Valor", i)

                            If Not String.IsNullOrEmpty(strValor) AndAlso Not strValor = "0" Then

                                strPago = dataTableDepositos.GetValue("Pago", i)

                                blnReversar = False

                                Exit Sub

                            End If

                        End If

                    End If

                Next

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Manejo de evento de botón de imprimir pago generado

    Public Sub ButtonSBOImprimirPagoItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String
        Dim strPrestamo As String
        Dim strNumeroPago As String
        Dim strParametros As String
        Dim strUsuarioSBO As String

        strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0)
        strPrestamo = strPrestamo.Trim()

        strNumeroPago = EditTextNumero.ObtieneValorUserDataSource()

        strUsuarioSBO = ApplicationSBO.Company.UserName

        strUsuarioSBO = General.EjecutarConsulta("SELECT U_NAME FROM OUSR WHERE USER_CODE = '" & strUsuarioSBO & "'", StrConexion)

        If pVal.BeforeAction = True Then

            If String.IsNullOrEmpty(strPrestamo) OrElse String.IsNullOrEmpty(strNumeroPago) OrElse String.IsNullOrEmpty(strUsuarioSBO) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

        ElseIf pVal.BeforeAction = False Then

            strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptCancelacionMensualCliente & ".rpt"

            strParametros = strPrestamo & "," & strNumeroPago & "," & strUsuarioSBO

            Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloRepPagoMensual, strParametros, StrUsuarioBD, StrContraseñaBD)

        End If

    End Sub

    'Manejo de botón de imprimir pagos reversados

    Public Sub ButtonSBOImprimirReversadosItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String
        Dim strParametros As String
        Dim strUsuarioSBO As String

        strUsuarioSBO = ApplicationSBO.Company.UserName

        strUsuarioSBO = General.EjecutarConsulta("SELECT U_NAME FROM OUSR WHERE USER_CODE = '" & strUsuarioSBO & "'", StrConexion)

        If pVal.BeforeAction = True Then

            If String.IsNullOrEmpty(m_strCodPrestRev) OrElse String.IsNullOrEmpty(strUsuarioSBO) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

        ElseIf pVal.BeforeAction = False Then

            strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptPagosReversados & ".rpt"

            strParametros = m_strCodPrestRev & "," & strUsuarioSBO

            Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloPagosReversados, strParametros, StrUsuarioBD, StrContraseñaBD)

        End If

    End Sub

    Public Sub ButtonSBOAgregaChequeItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strFechaVencimiento As String
        Dim intPosicion As Integer

        Try
            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then

                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

                If Not String.IsNullOrEmpty(EditTextFeVen.ObtieneValorUserDataSource()) And Not String.IsNullOrEmpty(EditTextImp.ObtieneValorUserDataSource) _
                    And Not String.IsNullOrEmpty(ComboBoxNBan.ObtieneValorUserDataSource) And Not String.IsNullOrEmpty(EditTextCuen.ObtieneValorUserDataSource) _
                    And Not String.IsNullOrEmpty(EditTextNChe.ObtieneValorUserDataSource) Then

                    oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
                    oMatrix.FlushToDataSource()

                    intPosicion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size

                    If intPosicion = 1 Then
                        strFechaVencimiento = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_FVen", 0)
                        strFechaVencimiento = strFechaVencimiento.Trim()
                        If String.IsNullOrEmpty(strFechaVencimiento) Then
                            intPosicion = 0
                        Else
                            intPosicion = 1
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").InsertRecord(intPosicion)
                        End If
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").InsertRecord(intPosicion)
                    End If

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_FVen", intPosicion, EditTextFeVen.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Imp", intPosicion, EditTextImp.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Pai", intPosicion, ComboBoxPai.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_NBan", intPosicion, ComboBoxNBan.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Suc", intPosicion, ComboBoxSuc.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Cta", intPosicion, EditTextCuen.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_NCh", intPosicion, EditTextNChe.ObtieneValorUserDataSource)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_End", intPosicion, ComboBoxEnd.ObtieneValorUserDataSource)

                    oMatrix.LoadFromDataSource()

                    ManejaControlesChequesPostFechados(True, False, True)

                    m_blnEjecutarMetodo = False
                Else
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltanDatosCheque, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Public Sub ButtonSBOActualizaChequeItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strFechaVencimiento As String

        If pVal.BeforeAction Then

        ElseIf pVal.ActionSuccess Then

            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_FVen", g_intPosicion, EditTextFeVen.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Imp", g_intPosicion, EditTextImp.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Pai", g_intPosicion, ComboBoxPai.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_NBan", g_intPosicion, ComboBoxNBan.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Suc", g_intPosicion, ComboBoxSuc.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Cta", g_intPosicion, EditTextCuen.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_NCh", g_intPosicion, EditTextNChe.ObtieneValorUserDataSource)
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_End", g_intPosicion, ComboBoxEnd.ObtieneValorUserDataSource)

            oMatrix.LoadFromDataSource()

            ButtonActCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            ManejaControlesChequesPostFechados(True, False, True)

        End If

    End Sub


    Public Sub ButtonSBOEliminaChequeItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strFechaVencimiento As String
        Dim intRegistoEliminar As Integer

        If pVal.BeforeAction Then

        ElseIf pVal.ActionSuccess Then

            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For i As Integer = 1 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size
                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Sel", i - 1).Trim() = "Y" Then
                    intRegistoEliminar = i
                    Exit For
                End If
            Next

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").RemoveRecord(intRegistoEliminar - 1)

            oMatrix.LoadFromDataSource()

            m_blnEjecutarMetodo = False

        End If

    End Sub

    Public Sub ButtonSBOAplicaChequeItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oFolder As SAPbouiCOM.Folder
        Dim intRegistoSeleccionado As Integer

        If pVal.BeforeAction Then
            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For i As Integer = 1 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size
                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Sel", i - 1).Trim() = "Y" Then
                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Apli", i - 1).Trim() = "Y" Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorChequePostfechado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                    End If
                    Exit For
                End If
            Next
        ElseIf pVal.ActionSuccess Then

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For i As Integer = 1 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size
                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Sel", i - 1).Trim() = "Y" Then
                    intRegistoSeleccionado = i
                    Exit For
                End If
            Next

            If intRegistoSeleccionado > 0 Then

                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

                g_strChequeAplicado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("LineId", intRegistoSeleccionado - 1)
                g_strFechaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_FVen", intRegistoSeleccionado - 1)
                g_strMontoAbonar = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Imp", intRegistoSeleccionado - 1)
                g_strPais = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Pai", intRegistoSeleccionado - 1)
                g_strNBanco = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_NBan", intRegistoSeleccionado - 1)
                g_strSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Suc", intRegistoSeleccionado - 1)
                g_strCuenta = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Cta", intRegistoSeleccionado - 1)
                g_strNoCheque = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_NCh", intRegistoSeleccionado - 1)
                g_strEndoso = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_End", intRegistoSeleccionado - 1)
                EditTextFechaPago.AsignaValorUserDataSource(g_strFechaPago)
                EditTextMontoAbo.AsignaValorUserDataSource(g_strMontoAbonar)

                oFolder = DirectCast(FormularioSBO.Items.Item("Folder2").Specific, SAPbouiCOM.Folder)
                oFolder.Select()

                CheckBoxCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

                oMatrix.LoadFromDataSource()

                m_blnEjecutarMetodo = False

                FormularioSBO.Items.Item("btnCalcPag").Click()
                CheckBoxCheque.AsignaValorDataSource("Y")

                ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            End If

        End If

    End Sub

    Public Sub ButtonSBORevalorizaciónItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oItem As SAPbouiCOM.Item

        Dim strDocNum As String
        Dim strContrato As String
        Dim strEnteFinanciero As String
        Dim strCliente As String
        Dim strClienteNombre As String
        Dim strEmpleado As String
        Dim strEmpleadoNombre As String
        Dim strUnidad As String
        Dim strCodMoneda As String
        Dim strDesMoneda As String

        Dim strPrecioVenta As String
        Dim decPrecioVenta As Decimal
        Dim strPlazo As String
        Dim strFechaInicio As String
        Dim strDiaPago As String
        Dim strIntMoratorio As String
        Dim decIntMoratorio As Decimal
        Dim strMontoFinanciar As String
        Dim decMontoFinanciar As Decimal
        Dim strInteresNormal As String
        Dim decInteresNormal As Decimal
        Dim strCodTipoCuota As String
        Dim strTipoCuota As String
        Dim strMontoCanActual As String
        Dim decMontoCanActual As Decimal

        Dim strFechaPago As String
        Dim dtFechaPago As Date
        Dim strMontoSaldoInicial As String
        Dim decMontoSaldoInicial As Decimal
        Dim strMontoSaldoRestante As String
        Dim decMontoSaldoRestante As Decimal

        Try
            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then

                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

                strContrato = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cont_Ven", 0).Trim()
                strEnteFinanciero = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_ent_Fin", 0).Trim()
                strCliente = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Cli", 0).Trim()
                strClienteNombre = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Cli", 0).Trim()
                strEmpleado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Emp", 0).Trim()
                strEmpleadoNombre = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Emp", 0).Trim()
                strUnidad = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Unid", 0).Trim()
                strDesMoneda = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Mon", 0).Trim()
                strCodMoneda = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Moneda", 0).Trim()

                strPrecioVenta = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Pre_Vta", 0).Trim()
                decPrecioVenta = General.ConvierteDecimal(strPrecioVenta, n)
                strMontoFinanciar = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Mon_Fin", 0).Trim()
                decMontoFinanciar = Decimal.Parse(strPrecioVenta, n)
                strInteresNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).Trim()
                decInteresNormal = Decimal.Parse(strPrecioVenta, n)
                strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
                strFechaInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0).Trim()
                strDiaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_DiaPago", 0).Trim()
                strIntMoratorio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0).Trim()
                decIntMoratorio = General.ConvierteDecimal(strIntMoratorio, n)
                strCodTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Des_Tipo", 0).Trim()
                strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()
                strMontoCanActual = EditTextMontoCancelar.ObtieneValorUserDataSource()
                decMontoCanActual = General.ConvierteDecimal(strMontoCanActual, n)

                For index As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Size - 1
                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", index).Trim <> "Y" Then
                        dbMontoAsientoRevalorizacion = Convert.ToDouble(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", index), n) + Convert.ToDouble(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", index), n)
                        Exit For
                    End If
                Next

                EditTextNumero.AsignaValorUserDataSource(0)
                EditTextFechaUltimo.AsignaValorUserDataSource("")
                EditTextFechaPago.AsignaValorUserDataSource("")
                EditTextSalIni.AsignaValorUserDataSource(0)
                EditTextMontoAbo.AsignaValorUserDataSource(0)
                EditTextAboCap.AsignaValorUserDataSource(0)
                EditTextCapPend.AsignaValorUserDataSource(0)
                EditTextAboInt.AsignaValorUserDataSource(0)
                EditTextDiasInt.AsignaValorUserDataSource(0)
                EditTextIntPend.AsignaValorUserDataSource(0)
                EditTextAboMor.AsignaValorUserDataSource(0)
                EditTextDiasMora.AsignaValorUserDataSource(0)
                EditTextMoraPend.AsignaValorUserDataSource(0)
                EditTextSalFin.AsignaValorUserDataSource(0)

                strDocNum = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocNum", 0).Trim()

                oItem = FormularioSBO.Items.Item("1")
                oItem.Click()

                FormularioSBO.Mode = BoFormMode.fm_ADD_MODE
                EditTextMontoFin.ItemSBO.Enabled = True

                ManejaControlesRevalorización(True)
                ManejaControlesChequesPostFechados(False, True, True)

                FormularioSBO.Items.Item("txtIntNor").Click()

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Estado", 0, "1")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Est", 0, My.Resources.Resource.EstadoActivo)

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_PreBa", 0, strDocNum)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Cont_Ven", 0, strContrato)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_ent_Fin", 0, strEnteFinanciero)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Cod_Cli", 0, strCliente)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Cli", 0, strClienteNombre)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Cod_Emp", 0, strEmpleado)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Emp", 0, strEmpleadoNombre)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Cod_Unid", 0, strUnidad)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Moneda", 0, strCodMoneda)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Mon", 0, strDesMoneda)

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Pre_Vta", 0, decPrecioVenta.ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Mon_Fin", 0, decMontoCanActual.ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Interes", 0, strInteresNormal)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Plazo", 0, strPlazo)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Fec_Pres", 0, strFechaInicio)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_DiaPago", 0, strDiaPago)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Int_Mora", 0, decIntMoratorio.ToString(n))
                FormularioSBO.Items.Item("txtPlazo").Click()
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Tipo", 0, strCodTipoCuota)
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Tipo_Cuo", 0, strTipoCuota)

                FormularioSBO.Items.Item("Folder1").Click()
                FormularioSBO.PaneLevel = 1

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Public Sub CheckBoxSBOAbonaChequeItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix

        If pVal.BeforeAction Then

        ElseIf pVal.ActionSuccess Then

            If CheckBoxCheque.ObtieneValorDataSource() = "N" Then

                CheckBoxCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                EditTextMontoAbo.AsignaValorUserDataSource("0")
                EditTextFechaPago.AsignaValorUserDataSource("")
                EditTextAboCap.AsignaValorUserDataSource("0")
                EditTextAboInt.AsignaValorUserDataSource("0")
                EditTextDiasInt.AsignaValorUserDataSource("0")
                EditTextAboMor.AsignaValorUserDataSource("0")
                EditTextDiasMora.AsignaValorUserDataSource("0")
                EditTextMoraPend.AsignaValorUserDataSource("0")
                EditTextSalFin.AsignaValorUserDataSource("0")
                m_dtFechaPagoCalculo = Nothing
                g_strChequeAplicado = ""

            End If

        End If

    End Sub

    Public Sub MatrixChequesPostItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strFechaVencimiento As String

        FormularioSBO.Freeze(True)

        If pVal.BeforeAction Then

        ElseIf pVal.ActionSuccess Then

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            Dim intPosicion As Integer
            g_intPosicion = pVal.Row

            g_intPosicion = g_intPosicion - 1

            If g_intPosicion >= 0 And pVal.ColUID <> "col_Sel" Then
                If g_intPosicion <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size - 1 Then

                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Apli", g_intPosicion).Trim.ToString = "N" Then

                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                        EditTextFeVen.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_FVen", g_intPosicion))
                        EditTextImp.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Imp", g_intPosicion))
                        ComboBoxPai.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Pai", g_intPosicion))
                        ComboBoxNBan.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_NBan", g_intPosicion))
                        ComboBoxSuc.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Suc", g_intPosicion))
                        EditTextCuen.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Cta", g_intPosicion))
                        EditTextNChe.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_NCh", g_intPosicion))
                        ComboBoxEnd.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_End", g_intPosicion))

                        ButtonActCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                        ButtonAgreCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        ButtonEliCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                        ButtonAplicaCheque.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

                        m_blnEjecutarMetodo = False

                    End If

                End If
            ElseIf g_intPosicion >= 0 And pVal.ColUID = "col_Sel" Then

                For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size - 1
                    If i <> g_intPosicion Then FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Sel", i, "N")
                Next

            End If

            oMatrix.LoadFromDataSource()

        End If

        FormularioSBO.Freeze(False)

    End Sub
    'Manejo de evento de botón calcular los montos del pago a abonar antes de realizar el abono
    'Validaciones: completitud de los datos del préstamo, fecha del pago debe ser posterior a la del pago anterior, manejo de pagos menores a la cuota establecida
    'Manejo de diferentes calculos dependiendo de la fecha de pago ingresada y el monto de la cuota ingresado; y de configuraciones de disminución de plazo por pago extraordinario
    'o cancelación de cobro de intereses moratorios. No tiene afectación contable ni en el plan de pagos
    Public Sub ButtonSBOCalcularItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim n As NumberFormatInfo

        Dim strFechaPago As String
        Dim strNumero As String
        Dim intNumero As Integer = 0
        Dim strAbono As String
        Dim decAbono As Decimal = 0

        Dim strFechaAnterior As String
        Dim dtFechaAnterior As Date
        Dim dtFechaPago As Date
        Dim strTipoCuota As String

        Dim strFechaTeoricaPlan As String
        Dim dtFechaTeoricaPlan As Date
        Dim strCuotaPlan As String
        Dim decCuotaPlan As Decimal
        Dim strInteresPlan As String
        Dim decInteresPlan As Decimal = 0
        Dim strCapitalPlan As String
        Dim decCapitalPlan As Decimal = 0
        Dim strSaldoFinalPlan As String
        Dim decSaldoFinalPlan As Decimal = 0

        Dim strIntNormalPres As String
        Dim decIntNormalPres As Decimal = 0
        Dim strDocEntryPrestamo As String

        Dim strMontoMora As String
        Dim decMontoMora As Decimal = 0

        Dim strPermPagarMenos As String
        Dim strPlazo As String
        Dim intPlazo As Integer = 0

        Dim blnMoraActual As Boolean = False

        Dim strCapPendPlan As String
        Dim decCapPendPlan As Decimal = 0
        Dim strIntPendPlan As String
        Dim decIntPendPlan As Decimal = 0
        Dim strMoraPendPlan As String
        Dim decMoraPendPlan As Decimal = 0
        Dim strDiasIntPlan As String
        Dim intDiasIntPlan As Integer = 0

        Dim strPagoAnterior As String
        Dim intPagoAnterior As Integer

        Dim strCancelarMora As String

        n = DIHelper.GetNumberFormatInfo(CompanySBO)

        strDocEntryPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()

        strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
        If Not String.IsNullOrEmpty(strPlazo) Then intPlazo = Integer.Parse(strPlazo)

        strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()
        strIntNormalPres = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).Trim()

        If Not String.IsNullOrEmpty(strIntNormalPres) Then
            decIntNormalPres = Decimal.Parse(strIntNormalPres)
            decIntNormalPres = decIntNormalPres / 100
        End If

        strFechaPago = EditTextFechaPago.ObtieneValorUserDataSource().ToString()

        strNumero = EditTextNumero.ObtieneValorUserDataSource()
        If Not String.IsNullOrEmpty(strNumero) Then intNumero = Integer.Parse(strNumero)

        If Not String.IsNullOrEmpty(strFechaPago) Then
            dtFechaPago = Date.ParseExact(strFechaPago, "yyyyMMdd", Nothing)
            dtFechaPago = New Date(dtFechaPago.Year, dtFechaPago.Month, dtFechaPago.Day, 0, 0, 0)
        End If


        strFechaTeoricaPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strFechaTeoricaPlan) Then
            dtFechaTeoricaPlan = Date.ParseExact(strFechaTeoricaPlan, "yyyyMMdd", Nothing)
            dtFechaTeoricaPlan = New Date(dtFechaTeoricaPlan.Year, dtFechaTeoricaPlan.Month, dtFechaTeoricaPlan.Day, 0, 0, 0)
        End If

        strCuotaPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strCuotaPlan) Then decCuotaPlan = Decimal.Parse(strCuotaPlan, n)

        strInteresPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Interes", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strInteresPlan) Then decInteresPlan = Decimal.Parse(strInteresPlan, n)

        strCapitalPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strCapitalPlan) Then decCapitalPlan = Decimal.Parse(strCapitalPlan, n)

        strCapPendPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cap_Pend", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strCapPendPlan) Then decCapPendPlan = Decimal.Parse(strCapPendPlan, n)

        strIntPendPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Pend", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strIntPendPlan) Then decIntPendPlan = Decimal.Parse(strIntPendPlan, n)

        strMoraPendPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Mor_Pend", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strMoraPendPlan) Then decMoraPendPlan = Decimal.Parse(strMoraPendPlan, n)

        strDiasIntPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strDiasIntPlan) Then intDiasIntPlan = Integer.Parse(strDiasIntPlan)

        strSaldoFinalPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", intNumero - 1).Trim()
        If Not String.IsNullOrEmpty(strSaldoFinalPlan) Then decSaldoFinalPlan = Decimal.Parse(strSaldoFinalPlan, n)

        If intNumero = 1 Then

            strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0)
            strFechaAnterior = strFechaAnterior.Trim()
            If Not String.IsNullOrEmpty(strFechaAnterior) Then
                dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
            End If

        ElseIf intNumero > 1 Then

            strPagoAnterior = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strDocEntryPrestamo & "' And U_Pagado = 'Y' And U_Cuota > 0 And U_Numero < " & intNumero.ToString() & " ORDER BY U_Numero DESC", StrConexion)
            If Not String.IsNullOrEmpty(strPagoAnterior) Then intPagoAnterior = Integer.Parse(strPagoAnterior)

            strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intPagoAnterior - 1).Trim()
            If Not String.IsNullOrEmpty(strFechaAnterior) Then
                dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
            End If

        End If

        strAbono = EditTextMontoAbo.ObtieneValorUserDataSource()
        If Not String.IsNullOrEmpty(strAbono) Then decAbono = Decimal.Parse(strAbono, n)

        If pVal.BeforeAction Then

            If String.IsNullOrEmpty(strFechaPago) OrElse String.IsNullOrEmpty(strNumero) OrElse decAbono <= 0 OrElse String.IsNullOrEmpty(strIntNormalPres) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCalcular, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

            End If
            strPermPagarMenos = dataTableConsulta.GetValue("U_Pago_Men", 0)

            If (decAbono < decCuotaPlan AndAlso (strPermPagarMenos = "N" Or strPermPagarMenos = "")) Then

                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorPagoMenor, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False

            End If

        ElseIf pVal.ActionSuccess Then

            strCancelarMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0).Trim()

            If Not dtFechaPago = m_dtFechaPagoCalculo Then Call ValidarMoratorios()

            'If dtFechaPago >= dtFechaTeoricaPlan AndAlso _
            '    m_blnCalculadoIntMora = False AndAlso _
            '    Not strCancelarMora = "Y" Then

            If (dtFechaPago >= dtFechaTeoricaPlan AndAlso _
                m_blnCalculadoIntMora = False) OrElse strTipoCuota = "1" Then

                m_decCuotaMora = decCuotaPlan
                m_decCapital = decCapitalPlan
                m_decInteres = decInteresPlan
                m_intDiasInt = intDiasIntPlan
                m_decSaldoFinal = decSaldoFinalPlan

                If m_intDiasInt > 0 Then
                    Call CalcularInteresesMoratorios(m_decCuotaMora, strTipoCuota, intNumero, dtFechaPago, dtFechaTeoricaPlan, False, _
                                                     decMontoMora, m_intDiasInt, m_decInteres, decIntNormalPres, m_decSaldoFinal, m_decCapital, strCancelarMora)
                End If

                m_blnCalculadoIntMora = True



                m_dtFechaPagoCalculo = dtFechaPago

            End If

            If dtFechaPago <= dtFechaTeoricaPlan OrElse (dtFechaPago > dtFechaTeoricaPlan AndAlso m_blnCalculadoIntMora = True) Then

                If dtFechaPago >= dtFechaTeoricaPlan AndAlso m_blnCalculadoIntMora = True Then

                    decCuotaPlan = m_decCuotaMora
                    decCapitalPlan = m_decCapital
                    decInteresPlan = m_decInteres
                    intDiasIntPlan = m_intDiasInt
                    decSaldoFinalPlan = m_decSaldoFinal

                    strMontoMora = EditTextAboMor.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strMontoMora) Then decMontoMora = Decimal.Parse(strMontoMora, n)

                    EditTextAboCap.AsignaValorUserDataSource(decCapitalPlan.ToString(n))
                    EditTextAboInt.AsignaValorUserDataSource(decInteresPlan.ToString(n))
                    EditTextSalFin.AsignaValorUserDataSource(decSaldoFinalPlan.ToString(n))
                    EditTextDiasInt.AsignaValorUserDataSource(intDiasIntPlan.ToString())

                End If
                If strTipoCuota = "1" Then
                    Call ManejoPagosMenores(decAbono, decMontoMora, decInteresPlan, decSaldoFinalPlan, decCuotaPlan, False, intNumero, dtFechaPago, dtFechaTeoricaPlan, strTipoCuota, decIntNormalPres, decCapitalPlan, decCapPendPlan, decIntPendPlan, decMoraPendPlan, , False, CDbl(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n)))
                ElseIf dtFechaPago < dtFechaTeoricaPlan AndAlso Math.Abs(decAbono - decCuotaPlan) <= Math.Pow(10, -1 * 1) Then

                    If strTipoCuota = "1" OrElse strTipoCuota = "2" Then

                        Call CalcularPagosAdelantadosNivelada(dtFechaPago, intNumero, decCuotaPlan, False, decIntNormalPres, decCapPendPlan, decIntPendPlan, decMoraPendPlan)

                    End If

                ElseIf Math.Abs(decAbono - decCuotaPlan) > Math.Pow(10, -1 * 1) AndAlso decAbono > decCuotaPlan Then

                    If dtFechaPago < dtFechaTeoricaPlan AndAlso (strTipoCuota = "1" OrElse strTipoCuota = "2") Then

                        Call CalcularPagosAdelantadosNivelada(dtFechaPago, intNumero, decCuotaPlan, False, decIntNormalPres, decCapPendPlan, decIntPendPlan, decMoraPendPlan)

                        strInteresPlan = EditTextAboInt.ObtieneValorUserDataSource()
                        If Not String.IsNullOrEmpty(strInteresPlan) Then
                            decInteresPlan = Decimal.Parse(strInteresPlan, n)
                        End If

                    End If

                    Call CalcularPagosExtraordinarios(decAbono, decCuotaPlan, intNumero, False, strTipoCuota, dtFechaTeoricaPlan, decInteresPlan, dtFechaPago, decIntNormalPres, decMontoMora, decCapPendPlan, decIntPendPlan, decMoraPendPlan)

                ElseIf (Math.Abs(decCuotaPlan - decAbono) > Math.Pow(10, -1 * 1) AndAlso decAbono < decCuotaPlan) OrElse FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero - 1).Trim = "P" Then

                    Call ManejoPagosMenores(decAbono, decMontoMora, decInteresPlan, decSaldoFinalPlan, decCuotaPlan, False, intNumero, dtFechaPago, dtFechaTeoricaPlan, strTipoCuota, decIntNormalPres, decCapitalPlan, decCapPendPlan, decIntPendPlan, decMoraPendPlan, , False, CDbl(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n)))

                End If

            End If

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Chk", 0, "N")

        End If

    End Sub

    'Manejo de evento de botón abonar pago
    'Validaciones: compeltitud de datos, fecha posterior a la de pago anterior, fecha igual o anterior a la fecha de sistema, cuentas configuradas para el pago recibido y monedas,
    'Período contable y tipo de cambio para realizar el abono en fecha indicada, calculo de monto de mora, manejo de pagos menores
    'Llama a metodo de realizar el abono

    Public Sub ButtonSBOAbonarItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim n As NumberFormatInfo

        Dim strFechaPago As String
        Dim strNumero As String
        Dim intNumero As Integer = 0
        Dim strAbono As String
        Dim decAbono As Decimal = 0
        Dim strCliente As String = ""
        Dim strMoneda As String
        Dim strMonedaCliente As String = ""
        Dim strMonedaDebCap As String = ""
        Dim strMonedaDebFinancia As String = ""
        Dim strCuentaDebCap As String = ""
        Dim strCuentaValidaCap As String = ""
        Dim strCuentaDebFinancia As String = ""
        Dim strCuentaValidaFinancia As String = ""
        Dim strFechaAnterior As String
        Dim dtFechaAnterior As Date
        Dim tsDifDias As TimeSpan
        Dim intDiasDif As Integer
        Dim dtFechaPago As Date
        Dim strIntNormal As String
        Dim decIntNormal As Decimal = 0
        Dim strPrestamo As String
        Dim strPagado As String = ""
        Dim strFechaTeorica As String
        Dim dtFechaTeorica As Date
        Dim strPermPagarMenos As String
        Dim strCuota As String
        Dim decCuota As Decimal = 0
        Dim strPlazo As String
        Dim intPlazo As Integer
        Dim strTipoCuota As String
        Dim strEstadoPagoSig As String = ""
        Dim strEstadoPeriodo As String = ""
        Dim strMonedaSistema As String
        Dim strMonedaLocal As String
        Dim strTipoCambio As String
        Dim strPagoAnterior As String
        Dim intPagoAnterior As Integer
        Dim strCancelarMora As String
        Dim strGeneraAsiento As String

        Try
            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()
            strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()
            strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
            'strIntNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).Trim().ToString(n)

            If Not String.IsNullOrEmpty(strPlazo) Then intPlazo = Integer.Parse(strPlazo)

            strIntNormal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).Trim()

            If Not String.IsNullOrEmpty(strIntNormal) Then
                decIntNormal = Decimal.Parse(strIntNormal)
                decIntNormal = decIntNormal / 100
            End If

            strNumero = EditTextNumero.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(strNumero) Then intNumero = Integer.Parse(strNumero)

            strFechaPago = EditTextFechaPago.ObtieneValorUserDataSource().ToString()
            If Not String.IsNullOrEmpty(strFechaPago) Then
                dtFechaPago = Date.ParseExact(strFechaPago, "yyyyMMdd", Nothing)
                dtFechaPago = New Date(dtFechaPago.Year, dtFechaPago.Month, dtFechaPago.Day, 0, 0, 0)
            End If

            strFechaTeorica = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strFechaTeorica) Then
                dtFechaTeorica = Date.ParseExact(strFechaTeorica, "yyyyMMdd", Nothing)
                dtFechaTeorica = New Date(dtFechaTeorica.Year, dtFechaTeorica.Month, dtFechaTeorica.Day, 0, 0, 0)
            End If

            If intNumero = 1 Then
                strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0)
                strFechaAnterior = strFechaAnterior.Trim()
                If Not String.IsNullOrEmpty(strFechaAnterior) Then
                    dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                    dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
                End If
            ElseIf intNumero > 1 Then
                strPagoAnterior = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Cuota > 0 And U_Numero < " & intNumero.ToString() & " ORDER BY U_Numero DESC", StrConexion)
                If Not String.IsNullOrEmpty(strPagoAnterior) Then
                    intPagoAnterior = Integer.Parse(strPagoAnterior)
                End If

                strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intPagoAnterior - 1)
                strFechaAnterior = strFechaAnterior.Trim()
                If Not String.IsNullOrEmpty(strFechaAnterior) Then
                    dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                    dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
                End If
            End If

            strAbono = EditTextMontoAbo.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAbono) Then decAbono = Decimal.Parse(strAbono, n)

            strCliente = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Cod_Cli", 0).Trim()
            strMoneda = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Moneda", 0).Trim()
            strMonedaSistema = General.RetornarMonedaSistema(_companySbo)
            strMonedaLocal = General.RetornarMonedaLocal(_companySbo)
            strGeneraAsiento = dataTableConsulta.GetValue("U_Gen_As", 0)

            If strGeneraAsiento = "Y" Then

                If strMoneda = strMonedaLocal Then
                    strCuentaDebFinancia = dataTableConsulta.GetValue("U_Fin_Loc", 0)
                ElseIf strMoneda = strMonedaSistema Then
                    strCuentaDebFinancia = dataTableConsulta.GetValue("U_Fin_Sis", 0)
                End If

                strCuentaValidaFinancia =
                    General.EjecutarConsulta(
                        String.Format("Select AcctCode from dbo.[OACT] where FormatCode = '{0}' And Postable = 'Y'",
                                      strCuentaDebFinancia),
                                  StrConexion)

            End If

            If strMoneda = strMonedaLocal Then
                strCuentaDebCap = dataTableConsulta.GetValue("U_Cuo_Loc", 0)
            ElseIf strMoneda = strMonedaSistema Then
                strCuentaDebCap = dataTableConsulta.GetValue("U_Cuo_Sis", 0)
            End If

            strCuentaValidaCap =
                General.EjecutarConsulta(
                    String.Format("Select AcctCode from dbo.[OACT] where FormatCode = '{0}' And Postable = 'Y'",
                                  strCuentaDebCap),
                              StrConexion)

            If pVal.BeforeAction Then

                If String.IsNullOrEmpty(strFechaPago) OrElse String.IsNullOrEmpty(strNumero) OrElse decAbono <= 0 OrElse String.IsNullOrEmpty(strIntNormal) Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorAbonar, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                End If

                tsDifDias = dtFechaPago - dtFechaAnterior
                intDiasDif = tsDifDias.Days

                If dtFechaPago > Now.Date Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaPagoPosterior, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                End If

                If String.IsNullOrEmpty(strCuentaValidaCap) OrElse (strGeneraAsiento = "Y" AndAlso String.IsNullOrEmpty(strCuentaValidaFinancia)) Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorConfiguracion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                End If

                strMonedaCliente = General.EjecutarConsulta(String.Format("Select Currency from dbo.[OCRD] where CardCode = '{0}'", strCliente), StrConexion)
                strMonedaDebCap = General.EjecutarConsulta(String.Format("Select ActCurr from dbo.[OACT] where AcctCode = '{0}'", strCuentaValidaCap), StrConexion)
                strMonedaDebFinancia = General.EjecutarConsulta(String.Format("Select ActCurr from dbo.[OACT] where AcctCode = '{0}'", strCuentaValidaFinancia), StrConexion)

                If Not ((strMonedaCliente = "##" OrElse strMonedaDebCap = "##") OrElse (Not strMonedaDebFinancia = "##" AndAlso strGeneraAsiento = "Y")) _
                    AndAlso Not ((strMonedaCliente = strMoneda OrElse strMonedaDebCap = strMoneda) OrElse (Not strMonedaDebFinancia = strMoneda AndAlso strGeneraAsiento = "Y")) Then

                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorMoneda, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    Exit Sub

                End If

                strEstadoPeriodo =
                    General.EjecutarConsulta(
                        String.Format("SELECT PeriodStat FROM dbo.[OFPR] WHERE '{0}' >= F_RefDate AND '{1}' <= T_RefDate",
                                      dtFechaPago.ToString("yyyyMMdd"), dtFechaPago.ToString("yyyyMMdd")),
                        StrConexion)

                If strEstadoPeriodo <> "N" Then

                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorPeriodoContablePago, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    FormularioSBO.Refresh()
                    Exit Sub

                End If

                If strMonedaLocal.Trim() <> strMonedaSistema.Trim() Then
                    strTipoCambio =
                        General.EjecutarConsulta(
                            String.Format("SELECT Rate FROM ORTT WHERE Currency = '{0}' AND RateDate='{1}'",
                                          strMonedaSistema, dtFechaPago.ToString("yyyyMMdd")),
                                      StrConexion)

                    If String.IsNullOrEmpty(strTipoCambio) Then

                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambio, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        FormularioSBO.Refresh()
                        Exit Sub

                    End If
                End If


                If intNumero <> 0 AndAlso Not String.IsNullOrEmpty(strPrestamo) Then

                    strPagado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero - 1).Trim()

                    If strPagado = "N" Then

                        strCancelarMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0).Trim()

                        If Not dtFechaPago = m_dtFechaPagoCalculo Then Call ValidarMoratorios()

                        If dtFechaPago > dtFechaTeorica AndAlso m_blnCalculadoIntMora = False AndAlso Not strCancelarMora = "Y" AndAlso Not CheckBoxPagoDeuda.ObtieneValorUserDataSource = "Y" Then

                            m_dtFechaPagoCalculo = Nothing
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCalculoMora, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Exit Sub

                        End If

                        strCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumero - 1).Trim()

                        If Not String.IsNullOrEmpty(strCuota) Then decCuota = Decimal.Parse(strCuota, n)

                        If dtFechaPago > dtFechaTeorica AndAlso m_blnCalculadoIntMora = True Then decCuota = m_decCuotaMora

                        strPermPagarMenos = dataTableConsulta.GetValue("U_Pago_Men", 0)

                        If intNumero < intPlazo Then
                            strEstadoPagoSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero).Trim()
                        End If

                        If (decAbono < decCuota AndAlso (strPermPagarMenos = "N" Or strPermPagarMenos = "")) OrElse (decAbono < decCuota AndAlso intNumero = intPlazo) OrElse (decAbono < decCuota AndAlso strEstadoPagoSig = "Y") _
                            OrElse (decAbono < decCuota AndAlso (m_blnCalculadoIntMora = True AndAlso m_blnPermitirMoraMenor = False)) Then

                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorPagoMenor, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Exit Sub

                        End If

                    End If

                End If

            ElseIf pVal.ActionSuccess Then

                Dim oMatrix As SAPbouiCOM.Matrix

                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

                oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
                oMatrix.FlushToDataSource()

                If CheckBoxCheque.ObtieneValorDataSource().Trim() = "Y" Then
                    For i As Integer = 1 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").Size
                        If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").GetValue("U_Sel", i - 1).Trim() = "Y" Then
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Apli", i - 1, "Y")
                            Exit For
                        End If
                    Next
                End If

                oMatrix.LoadFromDataSource()


                Call RealizarAbono(strFechaPago, strNumero, decAbono, strCliente, strMoneda, strCuentaValidaCap, decIntNormal, strCuentaValidaFinancia, strGeneraAsiento)

                If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Estado", 0) <> 2 Then
                    Call CargarMontoActualCancelar(strPrestamo)
                End If

                m_dtFechaPagoCalculo = Nothing

                If strTipoCuota = "3" Then

                    FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                    CheckBoxCancelarMora.ItemSBO.Enabled = False
                    CheckBoxPagoDeuda.ItemSBO.Enabled = False
                    EditTextMontoAbo.ItemSBO.Enabled = False

                End If

                If strTipoCuota = "1" Then

                    EditTextIntNormal.ItemSBO.Enabled = False

                End If

                If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then
                    FormularioSBO.Items.Item("1").Click()
                Else
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                    FormularioSBO.Items.Item("1").Click(BoCellClickType.ct_Regular)
                End If

                If strTipoCuota <> "1" Then
                    Call CargarPagosReversar(strPrestamo)
                    FormularioSBO.Items.Item("Folder3").Enabled = True
                    FormularioSBO.Items.Item("Folder3").Visible = True
                Else
                    FormularioSBO.Items.Item("Folder3").Enabled = False
                    FormularioSBO.Items.Item("Folder3").Visible = False
                End If


                Call CargarDatosPago(strPrestamo)
                CheckBoxPagoDeuda.AsignaValorUserDataSource("N")
            End If
        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    'Maneja la realización del abono, determina si se cobran intereses moratorios, si es un pago adelantado, pago extraordinario, o pago menor, según la fecha y monto de cuota ingresados
    'Se genera el borrador del pago recibido en SBO

    Private Sub RealizarAbono(ByVal strFechaPago As String, ByVal strNumero As String, ByVal decAbono As Decimal, ByVal strCliente As String, ByVal strMoneda As String, _
                              ByVal strCuentaDebito As String, ByVal decIntNormal As Decimal, ByVal strCuentaFinancia As String, ByVal strGeneraAsiento As String)

        Dim n As NumberFormatInfo

        Dim strFechaTeorica As String
        Dim dtFechaTeorica As Date
        Dim strCuota As String
        Dim decCuota As Decimal
        Dim dtFechaPago As Date
        Dim intNumero As Integer
        Dim strPrestamo As String
        Dim strComentario As String
        Dim strAboCapital As String
        Dim decAboCapital As Decimal
        Dim strAboInteres As String
        Dim decAboInteres As Decimal
        Dim strAboMora As String
        Dim decAboMora As Decimal

        Dim blnPagoGenerado As Boolean = False
        Dim blnAsientoIntGenerado As Boolean = False

        Dim strPagoRecibido As String = ""
        Dim strAsientoIntereses As String = ""

        Dim strTipoCuota As String

        Dim strInteres As String
        Dim decInteres As Decimal = 0
        Dim strCapital As String
        Dim decCapital As Decimal = 0
        Dim strSaldoFinal As String
        Dim decSaldoFinal As Decimal = 0

        Dim strMontoMora As String
        Dim decMontoMora As Decimal = 0
        Dim blnMoraActual As Boolean = False

        Dim strCapPend As String
        Dim decCapPend As Decimal = 0
        Dim strIntPend As String
        Dim decIntPend As Decimal = 0
        Dim strMoraPend As String
        Dim decMoraPend As Decimal = 0
        Dim strDiasInt As String
        Dim intDiasInt As Integer = 0
        Dim strDiasIntM As String
        Dim intDiasIntM As Integer = 0

        Dim strAboCapPend As String
        Dim decAboCapPend As Decimal
        Dim strAboIntPend As String
        Dim decAboIntPend As Decimal
        Dim strAboMoraPend As String
        Dim decAboMoraPend As Decimal
        Dim decAboTotalCap As Decimal
        Dim decAboTotalInt As Decimal
        Dim decAboTotalMora As Decimal
        Dim decAbonoTotal As Decimal

        Dim intNumSigPago As Integer
        Dim strDesEstado As String

        Dim strCancelarMora As String
        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxReal").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            If Not String.IsNullOrEmpty(strFechaPago) Then
                dtFechaPago = Date.ParseExact(strFechaPago, "yyyyMMdd", Nothing)
                dtFechaPago = New Date(dtFechaPago.Year, dtFechaPago.Month, dtFechaPago.Day, 0, 0, 0)
            End If

            If Not String.IsNullOrEmpty(strNumero) Then intNumero = Integer.Parse(strNumero)

            intNumSigPago = intNumero + 1

            strFechaTeorica = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strFechaTeorica) Then
                dtFechaTeorica = Date.ParseExact(strFechaTeorica, "yyyyMMdd", Nothing)
                dtFechaTeorica = New Date(dtFechaTeorica.Year, dtFechaTeorica.Month, dtFechaTeorica.Day, 0, 0, 0)
            End If

            strCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strCuota) Then decCuota = Decimal.Parse(strCuota, n)

            strInteres = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Interes", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strInteres) Then decInteres = Decimal.Parse(strInteres, n)

            strCapital = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strCapital) Then decCapital = Decimal.Parse(strCapital, n)

            strMontoMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Mora", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strMontoMora) Then decMontoMora = Decimal.Parse(strMontoMora, n)

            strSaldoFinal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strSaldoFinal) Then decSaldoFinal = Decimal.Parse(strSaldoFinal, n)

            strCapPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cap_Pend", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strCapPend) Then decCapPend = Decimal.Parse(strCapPend, n)

            strIntPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Pend", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strIntPend) Then decIntPend = Decimal.Parse(strIntPend, n)

            strMoraPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Mor_Pend", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strMoraPend) Then decMoraPend = Decimal.Parse(strMoraPend, n)

            strDiasInt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strDiasInt) Then intDiasInt = Integer.Parse(strDiasInt)

            strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()
            If strTipoCuota <> "1" Then
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumero - 1, "Y")
            End If

            strCancelarMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0).Trim()

            If dtFechaPago > dtFechaTeorica Then

                If intDiasInt > 0 Then
                    Call CalcularInteresesMoratorios(decCuota, strTipoCuota, intNumero, dtFechaPago, dtFechaTeorica, True, decMontoMora, _
                                                     intDiasInt, decInteres, decIntNormal, decSaldoFinal, decCapital, strCancelarMora, intNumSigPago)

                End If

                If Not strCancelarMora = "Y" Then FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cobra_Mora", intNumero - 1, "Y")
                If strCancelarMora = "Y" Then FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cobra_Mora", intNumero - 1, "N")


            End If

            If strTipoCuota = "1" Then
                Call ManejoPagosMenores(decAbono, decMontoMora, decInteres, decSaldoFinal, decCuota, True, intNumero, dtFechaPago, dtFechaTeorica, strTipoCuota, decIntNormal, decCapital, decCapPend, decIntPend, decMoraPend, intNumSigPago, False, CDbl(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n)))
            ElseIf dtFechaPago < dtFechaTeorica AndAlso Math.Abs(decAbono - decCuota) <= Math.Pow(10, -1 * 1) Then

                If strTipoCuota = "1" OrElse strTipoCuota = "2" Then

                    Call CalcularPagosAdelantadosNivelada(dtFechaPago, intNumero, decCuota, True, decIntNormal, decCapPend, decIntPend, decMoraPend)

                ElseIf strTipoCuota = "3" Or strTipoCuota = "4" Then

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumero - 1, dtFechaPago.ToString("yyyyMMdd"))

                End If

            ElseIf Math.Abs(decAbono - decCuota) > Math.Pow(10, -1 * 1) AndAlso decAbono > decCuota Then

                If dtFechaPago < dtFechaTeorica AndAlso (strTipoCuota = "1" OrElse strTipoCuota = "2") Then

                    Call CalcularPagosAdelantadosNivelada(dtFechaPago, intNumero, decCuota, True, decIntNormal, decCapPend, decIntPend, decMoraPend)

                    strInteres = EditTextAboInt.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strInteres) Then
                        decInteres = Decimal.Parse(strInteres, n)
                    End If

                End If

                Call CalcularPagosExtraordinarios(decAbono, decCuota, intNumero, True, strTipoCuota, dtFechaTeorica, decInteres, dtFechaPago, decIntNormal, decMontoMora, decCapPend, decIntPend, decMoraPend, intNumSigPago)

            ElseIf (Math.Abs(decCuota - decAbono) > Math.Pow(10, -1 * 1) AndAlso decAbono < decCuota) OrElse FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero - 1).ToString.Trim = "P" Then

                Call ManejoPagosMenores(decAbono, decMontoMora, decInteres, decSaldoFinal, decCuota, True, intNumero, dtFechaPago, dtFechaTeorica, strTipoCuota, decIntNormal, decCapital, decCapPend, decIntPend, decMoraPend, intNumSigPago, False, CDbl(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n)))

            End If

            strAboCapital = EditTextAboCap.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboCapital) Then
                decAboCapital = Decimal.Parse(strAboCapital, n)
            Else
                decAboCapital = 0
            End If

            strAboCapPend = EditTextCapPend.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboCapPend) Then
                decAboCapPend = Decimal.Parse(strAboCapPend, n)
            Else
                decAboCapPend = 0
            End If

            decAboTotalCap = decAboCapPend + decAboCapital

            strAboInteres = EditTextAboInt.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboInteres) Then
                decAboInteres = Decimal.Parse(strAboInteres, n)
            Else
                decAboInteres = 0
            End If

            strAboIntPend = EditTextIntPend.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboIntPend) Then
                decAboIntPend = Decimal.Parse(strAboIntPend, n)
            Else
                decAboIntPend = 0
            End If

            decAboTotalInt = decAboIntPend + decAboInteres

            strAboMora = EditTextAboMor.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboMora) Then
                decAboMora = Decimal.Parse(strAboMora, n)
            Else
                decAboMora = 0
            End If

            strAboMoraPend = EditTextMoraPend.ObtieneValorUserDataSource()
            If Not String.IsNullOrEmpty(strAboMoraPend) Then
                decAboMoraPend = Decimal.Parse(strAboMoraPend, n)
            Else
                decAboMoraPend = 0
            End If

            decAboTotalMora = decAboMoraPend + decAboMora

            decAbonoTotal = decAboTotalCap + decAboTotalInt + decAboTotalMora + General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource(), n)

            strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()

            strComentario = My.Resources.Resource.DocumentoGenerado & strNumero & My.Resources.Resource.DelPrestamo & strPrestamo

            Call GenerarBorradorPagoRecibido(strCliente, dtFechaPago, strCuentaDebito, decAbonoTotal, decAboTotalCap, strMoneda, strCuentaFinancia, strComentario, strGeneraAsiento, blnPagoGenerado, strPrestamo, strNumero, strPagoRecibido, _
                                              decAboTotalInt, decAboTotalMora, General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString, n), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size)

            If blnPagoGenerado = True Then
                If strTipoCuota <> "1" Then
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_BorrPag", intNumero - 1, strPagoRecibido)
                Else

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_BorrPag", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, strPagoRecibido)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_NumPago", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_NumCuota", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, strNumero)

                    If CheckBoxCheque.ObtieneValorDataSource.Trim = "Y" Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_ChkAp", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, "Y")
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_ChkAp", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, "N")
                    End If
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").SetValue("U_Reversado", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size - 1, "N")
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").InsertRecord(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagos", intNumero - 1, strPrestamo)
                End If
            End If

            m_blnEjecutarMetodo = False

            If strTipoCuota <> "1" Then

                strSaldoFinal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strSaldoFinal) Then
                    decSaldoFinal = Decimal.Parse(strSaldoFinal, n)
                End If

            Else
                decSaldoFinal = 0
                For index As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Size - 1
                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", index).Trim() <> "Y" Then
                        decSaldoFinal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", index)
                        Exit For
                    End If
                Next
            End If
            
            If decSaldoFinal <= 0 OrElse CheckBoxPagoDeuda.ObtieneValorUserDataSource.Trim = "Y" Then

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Estado", 0, "2")

                strDesEstado = General.EjecutarConsulta("Select Name from [@SCGD_EST_PREST] where Code = '2'", StrConexion)

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Est", 0, strDesEstado)

            End If

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ChkAp", intNumero - 1, g_strChequeAplicado)
            g_strChequeAplicado = ""
            'FormularioSBO.Items.Item("1").Click()

            _applicationSbo.StatusBar.SetText(My.Resources.Resource.PagoMensual, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)

            'ButtonAbonar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            'ButtonCalcular.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ButtonImprimirPago.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            'ButtonReversar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_ModPlazo", 0, "N")
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Can_Mora", 0, "N")

            If strTipoCuota = "3" Then

                FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                CheckBoxCancelarMora.ItemSBO.Enabled = False
                CheckBoxPagoDeuda.ItemSBO.Enabled = False
                EditTextMontoAbo.ItemSBO.Enabled = False

            End If

            If strTipoCuota = "1" Then

                EditTextIntNormal.ItemSBO.Enabled = False

            End If

            m_blnCalculadoIntMora = False

            m_strCodPrestRev = String.Empty

            oMatrix.LoadFromDataSource()

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Realiza la reversión del pago seleccionado y los pagos ya realizados que estén posteriores a este; se genera la reversión del pago recibido generado, el asiento de intereses,
    'el borrador del pago recibido para cada uno de los pagos en reversión. Se recalcula el plan de pagos real para dejarlo como estaba antes del pago reversado, dejando este pago
    'en estado sin abonar nuevamente

    Public Sub ReversarPagos(ByVal intPosReversar As Integer)

        Dim n As NumberFormatInfo
        Dim dataTablePagosReversar As DataTable
        Dim strPrestamo As String
        Dim strNumeroPago As String
        Dim strPagoRecibido As String
        Dim strAsientoIntereses As String
        Dim intPagoRecibido As Integer
        Dim intAsientoIntereses As Integer
        Dim strTipoCuota As String
        Dim strConsulta As String
        Dim strPagoAsociado As String
        Dim intPagoAsociado As Integer
        Dim strPagoPlazo As String
        Dim intPagoPlazo As Integer = 0
        Dim blnPagoPlazo As Boolean = False
        Dim intNumeroPago As Integer
        Dim strDiaPago As String
        Dim intDiaPago As Integer
        Dim strFechaTeorica As String
        Dim dtFechaTeorica As Date
        Dim strFechaInicio As String
        Dim dtFechaInicio As Date
        Dim strPagoAnterior As String
        Dim intPagoAnterior As Integer
        Dim strFechaAnterior As String
        Dim dtFechaAnterior As Date
        Dim intDiasInt As Integer
        Dim strSaldoInicial As String
        Dim decSaldoInicial As Decimal
        Dim strIntNormal As String
        Dim decIntNormal As Decimal
        Dim strCapPend As String
        Dim decCapPend As Decimal = 0
        Dim strIntPend As String
        Dim decIntPend As Decimal = 0
        Dim decMoraPend As Decimal = 0
        Dim strPorcMora As String
        Dim decPorcMora As Decimal
        Dim intPosicion As Integer = 0
        Dim intExtensionFor As Integer = 0
        Dim decIntereses As Decimal
        Dim decCuota As Decimal
        Dim strPagoAde As String = ""
        Dim strPagoExtra As String = ""
        Dim strCapitalAde As String
        Dim decCapitalAde As Decimal
        Dim decSaldoInicialAde As Decimal
        Dim decSaldoFinalAde As Decimal
        Dim blnPagoAdeAnterior As Boolean = False
        Dim strCambiaPlazo As String = "N"
        Dim strMontoNivelado As String = ""
        Dim decMontoNivelado As Decimal = 0
        Dim strDesEstado As String
        Dim strEstado As String
        Dim strBorrador As String
        Dim intBorrador As Integer
        Dim strFactura As String

        Dim strChequeAplicado As String
        Dim intrChequeAplicado As Integer

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oMatrixChk As SAPbouiCOM.Matrix

        Try
            If Not CompanySBO.InTransaction Then
                CompanySBO.StartTransaction()
            End If

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxReal").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()
            oMatrixChk = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)
            oMatrixChk.FlushToDataSource()

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            dataTablePagosReversar = FormularioSBO.DataSources.DataTables.Item("ReversadosMatrix")
            strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocNum", 0).Trim()
            strTipoCuota = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Tipo_Cuo", 0).Trim()
            Call CrearUDOPagosReversados(strPrestamo, intPosReversar, MatrixPagosReversar.Matrix.RowCount, dataTablePagosReversar)

            For i As Integer = intPosReversar - 1 To MatrixPagosReversar.Matrix.RowCount - 1

                strNumeroPago = dataTablePagosReversar.GetValue("numero", i)

                If Not String.IsNullOrEmpty(strNumeroPago) Then
                    intNumeroPago = Integer.Parse(strNumeroPago)
                End If

                strPagoRecibido = General.EjecutarConsulta("Select U_Cred_Cap from [dbo].[@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & strNumeroPago & "'", StrConexion)
                strAsientoIntereses = General.EjecutarConsulta("Select U_Doc_Int from [dbo].[@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & strNumeroPago & "'", StrConexion)
                strFactura = General.EjecutarConsulta("Select U_DocFAc from [dbo].[@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & strNumeroPago & "'", StrConexion)
                strBorrador = General.EjecutarConsulta("Select U_BorrPag from [dbo].[@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & strNumeroPago & "'", StrConexion)

                If String.IsNullOrEmpty(strBorrador) Then
                    strBorrador = General.EjecutarConsulta("Select DocEntry from [OPDF] where U_SCGD_Prestamo = '" & strPrestamo & "' And U_SCGD_NumPago = '" & strNumeroPago & "'", StrConexion)
                End If

                If Not String.IsNullOrEmpty(strPagoRecibido) Then

                    intPagoRecibido = Integer.Parse(strPagoRecibido)

                    Call GenerarReversionPago(intPagoRecibido)

                End If

                If Not String.IsNullOrEmpty(strAsientoIntereses) Then

                    intAsientoIntereses = Integer.Parse(strAsientoIntereses)

                    Call GenerarReversionAsiento(intAsientoIntereses, strPrestamo, My.Resources.Resource.ComentarioDocumentoReversaIntereses)

                End If

                If Not String.IsNullOrEmpty(strFactura) Then

                    intAsientoIntereses = Integer.Parse(strFactura)

                    Call GenerarReversionFactura(intAsientoIntereses, strPrestamo, My.Resources.Resource.ComentarioDocumentoReversaIntereses)

                End If

                If Not String.IsNullOrEmpty(strBorrador) Then

                    intBorrador = Integer.Parse(strBorrador)

                    Call GenerarReversionBorrador(intBorrador)

                End If

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumeroPago - 1, "N")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cred_Cap", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Doc_Int", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_DocFac", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_BorrPag", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Ade", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Sig", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Ext", intNumeroPago - 1, "")
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cobra_Mora", intNumeroPago - 1, "")

                strChequeAplicado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ChkAp", intNumeroPago - 1).Trim()

                If Not String.IsNullOrEmpty(strChequeAplicado) Then
                    intrChequeAplicado = Integer.Parse(strChequeAplicado)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CHEPOSFECH").SetValue("U_Apli", intrChequeAplicado - 1, "N")
                End If

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ChkAp", intNumeroPago - 1, "")

                dataTablePagosAsociados = FormularioSBO.DataSources.DataTables.Item("PagosAsociados")

                dataTablePagosAsociados.Clear()

                strConsulta = "SELECT U_Numero FROM [dbo].[@SCGD_PLAN_REAL] WHERE DocEntry = '" & strPrestamo & "' AND U_Pago_Aso = '" & strNumeroPago & "' ORDER BY U_Numero DESC"

                dataTablePagosAsociados.ExecuteQuery(strConsulta)

                For iAsoc As Integer = 0 To dataTablePagosAsociados.Rows.Count - 1

                    strPagoAsociado = dataTablePagosAsociados.GetValue("U_Numero", iAsoc)

                    If Not String.IsNullOrEmpty(strPagoAsociado) Then

                        intPagoAsociado = Integer.Parse(strPagoAsociado)

                        If intPagoAsociado > 0 Then

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intPagoAsociado - 1, "N")

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Aso", intPagoAsociado - 1, "")

                            If blnPagoPlazo = False Then

                                intPagoPlazo = intPagoAsociado

                                blnPagoPlazo = True

                            End If

                        End If

                    End If

                Next

            Next

            strDiaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_DiaPago", 0)
            strDiaPago = strDiaPago.Trim()
            If Not String.IsNullOrEmpty(strDiaPago) Then
                intDiaPago = Integer.Parse(strDiaPago)
            End If

            strIntNormal = General.EjecutarConsulta("Select U_Interes From [@SCGD_PRESTAMO] Where DocEntry = '" & strPrestamo & "'", StrConexion)
            If Not String.IsNullOrEmpty(strIntNormal) Then
                decIntNormal = Decimal.Parse(strIntNormal)
                decIntNormal = decIntNormal / 100
            End If

            strNumeroPago = dataTablePagosReversar.GetValue("numero", intPosReversar - 1)

            If Not String.IsNullOrEmpty(strNumeroPago) Then
                intNumeroPago = Integer.Parse(strNumeroPago)
            End If

            If intNumeroPago > 1 Then

                'Se obtiene el plazo

                If intPagoPlazo = 0 AndAlso blnPagoPlazo = False Then

                    strPagoPlazo = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Sal_Ini = 0 And U_Pago_Aso IS NOT NULL ORDER BY U_Numero", StrConexion)

                    If Not String.IsNullOrEmpty(strPagoPlazo) Then
                        intPagoPlazo = Integer.Parse(strPagoPlazo)
                        intPagoPlazo = intPagoPlazo - 1
                    ElseIf String.IsNullOrEmpty(strPagoPlazo) Then
                        strPagoPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0)
                        strPagoPlazo = strPagoPlazo.Trim()
                        If Not String.IsNullOrEmpty(strPagoPlazo) Then
                            intPagoPlazo = Integer.Parse(strPagoPlazo)
                        End If
                    End If

                End If

                intExtensionFor = intPagoPlazo - 1

                intPagoPlazo = intPagoPlazo - intNumeroPago + 1

                'Se obtiene fecha de inicio, dias de interes para primer pago, saldo inicial

                strFechaTeorica = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_TEORICO").GetValue("U_Fecha", intNumeroPago - 1)
                strFechaTeorica = strFechaTeorica.Trim()
                If Not String.IsNullOrEmpty(strFechaTeorica) Then

                    dtFechaTeorica = Date.ParseExact(strFechaTeorica, "yyyyMMdd", Nothing)
                    dtFechaTeorica = New Date(dtFechaTeorica.Year, dtFechaTeorica.Month, dtFechaTeorica.Day, 0, 0, 0)

                    dtFechaInicio = dtFechaTeorica.AddMonths(-1)

                End If

                strPagoAnterior = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Cuota > 0 And U_Numero < " & intNumeroPago.ToString() & " ORDER BY U_Numero DESC", StrConexion)
                If Not String.IsNullOrEmpty(strPagoAnterior) Then
                    intPagoAnterior = Integer.Parse(strPagoAnterior)
                End If

                strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intPagoAnterior - 1)
                strFechaAnterior = strFechaAnterior.Trim()
                If Not String.IsNullOrEmpty(strFechaAnterior) Then
                    dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                    dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
                End If

                Call DeterminarDiasEntrePagos(dtFechaTeorica, dtFechaAnterior, intDiasInt)

                strSaldoInicial = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", intPagoAnterior - 1)
                strSaldoInicial = strSaldoInicial.Trim()
                If Not String.IsNullOrEmpty(strSaldoInicial) Then
                    decSaldoInicial = Decimal.Parse(strSaldoInicial, n)
                End If

            ElseIf intNumeroPago = 1 Then

                strPagoPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0)
                strPagoPlazo = strPagoPlazo.Trim()
                If Not String.IsNullOrEmpty(strPagoPlazo) Then
                    intPagoPlazo = Integer.Parse(strPagoPlazo)
                End If

                intExtensionFor = intPagoPlazo - 1

                strSaldoInicial = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Mon_Fin", 0)
                strSaldoInicial = strSaldoInicial.Trim()
                If Not String.IsNullOrEmpty(strSaldoInicial) Then
                    decSaldoInicial = Decimal.Parse(strSaldoInicial, n)
                End If

                strFechaInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0)
                strFechaInicio = strFechaInicio.Trim()
                If Not String.IsNullOrEmpty(strFechaInicio) Then
                    dtFechaInicio = Date.ParseExact(strFechaInicio, "yyyyMMdd", Nothing)
                    dtFechaInicio = New Date(dtFechaInicio.Year, dtFechaInicio.Month, dtFechaInicio.Day, 0, 0, 0)
                End If

                If intDiaPago > dtFechaInicio.Day Then

                    intDiasInt = 30 + intDiaPago - dtFechaInicio.Day

                ElseIf intDiaPago < dtFechaInicio.Day Then

                    intDiasInt = 30 - dtFechaInicio.Day + intDiaPago

                Else

                    intDiasInt = 30

                End If

            End If

            strCapPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cap_Pend", intNumeroPago - 1)
            strCapPend = strCapPend.Trim()
            If Not String.IsNullOrEmpty(strCapPend) Then
                decCapPend = Decimal.Parse(strCapPend, n)
            End If
            strIntPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Pend", intNumeroPago - 1)
            strIntPend = strIntPend.Trim()
            If Not String.IsNullOrEmpty(strIntPend) Then
                decIntPend = Decimal.Parse(strIntPend, n)
            End If
            strPorcMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0)
            strPorcMora = strPorcMora.Trim()
            If Not String.IsNullOrEmpty(strPorcMora) Then
                decPorcMora = Decimal.Parse(strPorcMora, n)
                decPorcMora = decPorcMora / 100
            End If

            decMoraPend = decCapPend * decPorcMora

            decSaldoInicial = decSaldoInicial - decCapPend

            'Manejo para pagos anteriores adelantados

            If intNumeroPago > 1 AndAlso (strTipoCuota = "1" OrElse strTipoCuota = "2") Then

                strPagoAde = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pago_Ade", intPagoAnterior - 1)
                strPagoAde = strPagoAde.Trim()

                strPagoExtra = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pago_Ext", intPagoAnterior - 1)
                strPagoExtra = strPagoExtra.Trim()

                If strPagoAde = "Y" AndAlso Not strPagoExtra = "Y" Then

                    strCapitalAde = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cap_Sig", intPagoAnterior - 1)
                    strCapitalAde = strCapitalAde.Trim()
                    If Not String.IsNullOrEmpty(strCapitalAde) Then
                        decCapitalAde = Decimal.Parse(strCapitalAde, n)
                    End If

                    decSaldoInicialAde = decSaldoInicial + decCapPend

                    decIntereses = ((decSaldoInicialAde * decIntNormal) / 360) * intDiasInt

                    decCuota = decCapitalAde + decIntereses + decCapPend + decIntPend + decMoraPend

                    decSaldoFinalAde = decSaldoInicialAde - decCapitalAde - decCapPend

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumeroPago - 1, dtFechaTeorica.ToString("yyyyMMdd"))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", intNumeroPago - 1, decSaldoInicialAde.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumeroPago - 1, decCuota.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumeroPago - 1, decCapitalAde.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumeroPago - 1, decIntereses.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", intNumeroPago - 1, "0")
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumeroPago - 1, decSaldoFinalAde.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumeroPago - 1, "N")
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", intNumeroPago - 1, strCapPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", intNumeroPago - 1, decIntPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", intNumeroPago - 1, decMoraPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumeroPago - 1, intDiasInt.ToString())
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Mor", intNumeroPago - 1, "0")

                    intPagoPlazo = intPagoPlazo - 1

                    decSaldoInicial = decSaldoFinalAde

                    dtFechaInicio = dtFechaTeorica

                    intDiasInt = 30

                    decCapPend = 0

                    decIntPend = 0

                    decMoraPend = 0

                    intNumeroPago = intNumeroPago + 1

                    blnPagoAdeAnterior = True

                End If

            End If

            'Manejo para pagos extraordinarios con disminucion de plazo en pago anterior

            If intNumeroPago > 1 Then

                strPagoExtra = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pago_Ext", intPagoAnterior - 1)
                strPagoExtra = strPagoExtra.Trim()

                If strPagoExtra = "Y" Then

                    strMontoNivelado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Mon_Niv", intPagoAnterior - 1)
                    strMontoNivelado = strMontoNivelado.Trim()
                    If Not String.IsNullOrEmpty(strMontoNivelado) Then
                        decMontoNivelado = Decimal.Parse(strMontoNivelado, n)
                    End If

                    If decMontoNivelado > 0 Then

                        strCambiaPlazo = "Y"

                    End If

                End If

            End If

            If strTipoCuota = "1" OrElse strTipoCuota = "2" Then

                Call _formPlanPlagos.CalculoNivelada(intPagoPlazo, decSaldoInicial, decIntNormal, dtFechaInicio, intDiaPago, False, strCambiaPlazo, intDiasInt, decMontoNivelado, True, decCapPend, decIntPend, decMoraPend)

            ElseIf strTipoCuota = "3" Then

                Call _formPlanPlagos.CalculoGlobal(intPagoPlazo, decSaldoInicial, decIntNormal, dtFechaInicio, intDiaPago)

            ElseIf strTipoCuota = "4" Then

                Call _formPlanPlagos.CalculoDecreciente(intPagoPlazo, decSaldoInicial, decIntNormal, dtFechaInicio, intDiaPago, strCambiaPlazo, decMontoNivelado, False, decCapPend, decIntPend, decMoraPend)

            End If

            For i As Integer = intNumeroPago - 1 To intExtensionFor

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", i, _formPlanPlagos.g_dtFechaPago(intPosicion).ToString("yyyyMMdd"))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", i, _formPlanPlagos.g_decSaldoInicial(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", i, _formPlanPlagos.g_decCuota(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", i, _formPlanPlagos.g_decCapital(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", i, _formPlanPlagos.g_decInteres(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", i, _formPlanPlagos.g_decMoratorios(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", i, _formPlanPlagos.g_decSaldoFinal(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", i, _formPlanPlagos.g_strPagado(intPosicion))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", i, _formPlanPlagos.g_decCapPend(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", i, _formPlanPlagos.g_decIntPend(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", i, _formPlanPlagos.g_decMoraPend(intPosicion).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", i, _formPlanPlagos.g_intDiasInt(intPosicion).ToString())
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Mor", i, _formPlanPlagos.g_intDiasMora(intPosicion).ToString())
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", i, 0)

                If i = intNumeroPago - 1 AndAlso decCapPend > 0 AndAlso (strTipoCuota = "1" OrElse strTipoCuota = "2") AndAlso blnPagoAdeAnterior = False Then

                    decIntereses = (((decSaldoInicial + decCapPend) * decIntNormal) / 360) * intDiasInt

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", i, decIntereses.ToString(n))

                    decCuota = _formPlanPlagos.g_decCuota(intPosicion).ToString(n) - _formPlanPlagos.g_decInteres(intPosicion).ToString(n)

                    decCuota = decCuota + decIntereses

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", i, decCuota.ToString(n))

                End If

                intPosicion = intPosicion + 1

            Next

            strEstado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Estado", 0)
            strEstado = strEstado.Trim()

            If strEstado = "2" Then

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Estado", 0, "1")

                strDesEstado = General.EjecutarConsulta("Select Name from [@SCGD_EST_PREST] where Code = '1'", StrConexion)

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Des_Est", 0, strDesEstado)

            End If

            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

            m_blnEjecutarMetodo = False

            oMatrix.LoadFromDataSource()
            oMatrixChk.LoadFromDataSource()

            FormularioSBO.Items.Item("1").Click(BoCellClickType.ct_Regular)

            If strTipoCuota <> "1" Then
                Call CargarPagosReversar(strPrestamo)
                FormularioSBO.Items.Item("Folder3").Enabled = True
                FormularioSBO.Items.Item("Folder3").Visible = True
            Else
                FormularioSBO.Items.Item("Folder3").Enabled = False
                FormularioSBO.Items.Item("Folder3").Visible = False
            End If


            Call CargarDatosPago(strPrestamo)

            Call CargarMontoActualCancelar(strPrestamo)

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_ModPlazo", 0, "N")
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").SetValue("U_Can_Mora", 0, "N")

            m_blnCalculadoIntMora = False

            If strTipoCuota = "3" Then

                FormularioSBO.Items.Item("chkModPlaz").Enabled = False
                CheckBoxCancelarMora.ItemSBO.Enabled = False
                CheckBoxPagoDeuda.ItemSBO.Enabled = False
                EditTextMontoAbo.ItemSBO.Enabled = False

            End If

            If strTipoCuota = "1" Then

                EditTextIntNormal.ItemSBO.Enabled = False

            End If

            ButtonImprimirReversados.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            _applicationSbo.StatusBar.SetText(My.Resources.Resource.PagosReversados, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)

            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            Throw ex

        End Try

    End Sub

    'Se crea el UDO de pagos reversados, ingresando datos generales del préstamo y datos de los pagos reversados

    Private Sub CrearUDOPagosReversados(ByVal strPrestamo As String, ByVal intPosInicial As Integer, ByVal intPosFinal As Integer, ByVal dataTablePagosReversados As SAPbouiCOM.DataTable)

        Dim n As NumberFormatInfo

        Dim udoPrestReversados As UDOPrestReversados
        Dim encabezadoPrestReversados As EncabezadoUDOPrestReversados
        Dim pagosReversados As PagosReversadosUDOPrestRev

        Dim strNumero As String
        Dim intNumero As Integer
        Dim strFecha As String
        Dim dtFecha As Date
        Dim strCuota As String
        Dim decCuota As Decimal
        Dim strCapital As String
        Dim decCapital As Decimal
        Dim strInteres As String
        Dim decInteres As Decimal
        Dim strIntMora As String
        Dim decIntMora As Decimal
        Dim strCapPend As String
        Dim decCapPend As Decimal
        Dim strIntPend As String
        Dim decIntPend As Decimal
        Dim strMoraPend As String
        Dim decMoraPend As Decimal
        Dim strDiasInt As String
        Dim intDiasInt As Integer
        Dim strDiasMora As String
        Dim intDiasMora As Integer

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            udoPrestReversados = New UDOPrestReversados(CompanySBO, "SCGD_PREST_REV")
            encabezadoPrestReversados = New EncabezadoUDOPrestReversados
            udoPrestReversados.ListaPagosReversados = New ListaPagosReversados()
            udoPrestReversados.ListaPagosReversados.LineasUDO = New List(Of ILineaUDO)()

            encabezadoPrestReversados.Prestamo = strPrestamo

            udoPrestReversados.Encabezado = encabezadoPrestReversados

            For i As Integer = intPosInicial - 1 To intPosFinal - 1

                strNumero = dataTablePagosReversados.GetValue("numero", i)
                If Not String.IsNullOrEmpty(strNumero) Then
                    intNumero = Integer.Parse(strNumero)
                End If

                strFecha = dataTablePagosReversados.GetValue("fecha", i)
                If Not String.IsNullOrEmpty(strFecha) Then
                    dtFecha = Date.Parse(strFecha)
                End If

                strCuota = dataTablePagosReversados.GetValue("cuota", i)
                If Not String.IsNullOrEmpty(strCuota) Then
                    decCuota = Decimal.Parse(strCuota, n)
                End If

                strCapital = dataTablePagosReversados.GetValue("capital", i)
                If Not String.IsNullOrEmpty(strCapital) Then
                    decCapital = Decimal.Parse(strCapital, n)
                End If

                strInteres = dataTablePagosReversados.GetValue("interes", i)
                If Not String.IsNullOrEmpty(strInteres) Then
                    decInteres = Decimal.Parse(strInteres, n)
                End If

                strIntMora = dataTablePagosReversados.GetValue("intMora", i)
                If Not String.IsNullOrEmpty(strIntMora) Then
                    decIntMora = Decimal.Parse(strIntMora, n)
                End If

                strCapPend = dataTablePagosReversados.GetValue("capPend", i)
                If Not String.IsNullOrEmpty(strCapPend) Then
                    decCapPend = Decimal.Parse(strCapPend, n)
                End If

                strIntPend = dataTablePagosReversados.GetValue("intPend", i)
                If Not String.IsNullOrEmpty(strIntPend) Then
                    decIntPend = Decimal.Parse(strIntPend, n)
                End If

                strMoraPend = dataTablePagosReversados.GetValue("moraPend", i)
                If Not String.IsNullOrEmpty(strMoraPend) Then
                    decMoraPend = Decimal.Parse(strMoraPend, n)
                End If

                strDiasInt = dataTablePagosReversados.GetValue("diasInt", i)
                If Not String.IsNullOrEmpty(strDiasInt) Then
                    intDiasInt = Integer.Parse(strDiasInt)
                End If

                strDiasMora = dataTablePagosReversados.GetValue("diasMora", i)
                If Not String.IsNullOrEmpty(strDiasMora) Then
                    intDiasMora = Integer.Parse(strDiasMora)
                End If

                pagosReversados = New PagosReversadosUDOPrestRev()

                pagosReversados.NumeroPago = intNumero
                pagosReversados.FechaPago = dtFecha
                pagosReversados.Cuota = decCuota
                pagosReversados.Capital = decCapital
                pagosReversados.Interes = decInteres
                pagosReversados.Moratorio = decIntMora
                pagosReversados.CapPend = decCapPend
                pagosReversados.IntPend = decIntPend
                pagosReversados.MoraPend = decMoraPend
                pagosReversados.DiasInt = intDiasInt
                pagosReversados.DiasMora = intDiasMora

                udoPrestReversados.ListaPagosReversados.LineasUDO.Add(pagosReversados)

            Next

            udoPrestReversados.Insert()

            If udoPrestReversados.LastErrorCode <> 0 Then
                If CompanySBO.InTransaction() Then
                    CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            Else
                m_strCodPrestRev = udoPrestReversados.Encabezado.DocEntry
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Se maneja la realización de pagos menores, esto se da cuando se va a cancelar un pago y se ingresa una cuota menor a la que se debe abonar, abonando primero los intereses
    'moratorios, luego intereses normales, y de último a capital hasta donde alcance el pago menor; lo que hace falta de abonar queda como pendiente para el siguiente pago

    Private Sub ManejoPagosMenores(ByVal decAbono As Decimal, ByVal decMontoMora As Decimal, ByVal decIntereses As Decimal, _
                                   ByVal decSaldoFinal As Decimal, ByVal decCuota As Decimal, ByVal blnRealizaPago As Boolean, ByVal intNumero As Integer, _
                                   ByVal dtFechaPago As Date, ByVal dtFechaTeorica As Date, ByVal strTipoCuota As String, ByVal decIntNormal As Decimal, _
                                   ByVal decCapital As Decimal, ByVal decCapPend As Decimal, ByVal decIntPend As Decimal, ByVal decMoraPend As Decimal, _
                                   Optional ByVal intNumSigPago As Integer = 0, Optional ByVal blnRecursivo As Boolean = False, Optional ByVal dbRecargoCobranza As Double = 0)

        Dim decSobraAbono As Decimal = 0
        Dim decAboMora As Decimal = 0
        Dim decAboInt As Decimal = 0
        Dim decAboCap As Decimal = 0
        Dim decAboCuota As Decimal = 0
        Dim intPlazo As Integer = 0
        Dim intPosicion As Integer = 0
        Dim decCapitalFaltante As Decimal
        Dim decIntSobra As Decimal
        Dim decMoraSobra As Decimal
        Dim decSaldoInicialSig As Decimal
        Dim decSumaCapSig As Decimal
        Dim decSumaMoraSig As Decimal
        Dim decSumaCuotaSig As Decimal
        Dim decSaldoFinalSig As Decimal
        Dim strCapSig As String
        Dim decCapSig As Decimal = 0
        Dim decIntSig As Decimal = 0
        Dim decMoraSig As Decimal = 0
        Dim strCuotaSig As String
        Dim decCuotaSig As Decimal = 0
        Dim strPorcMora As String
        Dim decPorcMora As Decimal = 0
        Dim strIntereses As String
        Dim strCapital As String
        Dim strSaldoFinal As String
        Dim decAboMoraPend As Decimal = 0
        Dim dbAbonoRecargoporCobranza As Double = 0
        Dim decAboIntPend As Decimal = 0
        Dim decAboCapPend As Decimal = 0
        Dim strDiasInt As String
        Dim intDiasInt As Integer
        'Dim oMatrix As SAPbouiCOM.Matrix
        Dim n As NumberFormatInfo

        Dim strMontoMora As String = String.Empty

        Try

            'oMatrix = DirectCast(FormularioSBO.Items.Item("mtxReal").Specific, SAPbouiCOM.Matrix)
            'oMatrix.FlushToDataSource()

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            decAboCuota = decAbono

            If dtFechaPago < dtFechaTeorica Then

                If strTipoCuota = "2" Then

                    Call CalcularPagosAdelantadosNivelada(dtFechaPago, intNumero, decCuota, blnRealizaPago, decIntNormal, decCapPend, decIntPend, decMoraPend)

                    strIntereses = EditTextAboInt.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strIntereses) Then decIntereses = Decimal.Parse(strIntereses, n)

                    strCapital = EditTextAboCap.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strCapital) Then decCapital = Decimal.Parse(strCapital, n)

                    strSaldoFinal = EditTextSalFin.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(strSaldoFinal) Then decSaldoFinal = Decimal.Parse(strSaldoFinal, n)

                End If

            End If

            decMontoMora += General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Mora", intNumero - 1), n)
            If decMontoMora < 0 Then decMontoMora = 0
            dbRecargoCobranza -= CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ReCo", intNumero - 1).ToString(n), n))
            If dbRecargoCobranza < 0 Then dbRecargoCobranza = 0

            decSobraAbono = decAbono
            decAbono -= dbRecargoCobranza

            'Recargo Cobranza
            If decAbono > 0 Then
                dbAbonoRecargoporCobranza = dbRecargoCobranza
                decSobraAbono = decAbono
                decAbono = decAbono - decMoraPend

                If decAbono > 0 Then

                    decAboMoraPend = decMoraPend
                    decSobraAbono = decAbono
                    decAbono = decAbono - decMontoMora

                    'Mora Actual
                    If decAbono > 0 Then

                        decAboMora = decMontoMora
                        decSobraAbono = decAbono
                        decAbono = decAbono - decIntPend

                        'Interes Pendiente
                        If decAbono > 0 Then

                            decAboIntPend = decIntPend
                            decSobraAbono = decAbono
                            decAbono = decAbono - decIntereses

                            'Interes Actual
                            If decAbono > 0 Then

                                decAboInt = decIntereses
                                decSobraAbono = decAbono
                                decAbono = decAbono - decCapPend

                                'Capital Pendiente
                                If decAbono > 0 Then

                                    decAboCapPend = decCapPend
                                    decSobraAbono = decAbono
                                    decAbono = decAbono - decCapital

                                    'Capital Actual
                                    If decAbono > 0 Then

                                        If strTipoCuota <> "1" Then
                                            decAboCap = decAbono
                                            decCapitalFaltante = decCapital - decAboCap
                                            decSaldoFinal = decSaldoFinal + decCapitalFaltante
                                            decIntSobra = 0
                                            decMoraSobra = 0
                                        Else
                                            decAboCap = decCapital
                                            decCapitalFaltante = decCapital - decAboCap
                                            decSaldoFinal = decSaldoFinal + decCapitalFaltante
                                            decIntSobra = 0
                                            decMoraSobra = 0
                                        End If


                                        'Capital Actual
                                    Else

                                        decAboCap = decSobraAbono
                                        decCapitalFaltante = decCapital - decAboCap
                                        decSaldoFinal = decSaldoFinal + decCapitalFaltante
                                        decIntSobra = 0
                                        decMoraSobra = 0

                                    End If

                                    'Capital Pendiente
                                Else

                                    decAboCapPend = decSobraAbono
                                    decAboCap = 0
                                    decCapitalFaltante = decCapital + (decCapPend - decAboCapPend)
                                    decSaldoFinal = decSaldoFinal + decCapitalFaltante
                                    decIntSobra = 0
                                    decMoraSobra = 0

                                End If

                                'Interes Actual
                            Else

                                decAboInt = decSobraAbono
                                decAboCapPend = 0
                                decAboCap = 0
                                decCapitalFaltante = decCapital + decCapPend
                                decSaldoFinal = decSaldoFinal + decCapitalFaltante
                                decIntSobra = decIntereses - decAboInt
                                decMoraSobra = 0

                            End If

                            'Interes Pendiente
                        Else

                            decAboIntPend = decSobraAbono
                            decAboInt = 0
                            decAboCapPend = 0
                            decAboCap = 0
                            decCapitalFaltante = decCapital + decCapPend
                            decSaldoFinal = decSaldoFinal + decCapitalFaltante
                            decIntSobra = decIntereses + (decIntPend - decAboIntPend)
                            decMoraSobra = 0

                        End If

                        'Mora Actual
                    Else

                        decAboMora = decSobraAbono
                        decAboIntPend = 0
                        decAboInt = 0
                        decAboCapPend = 0
                        decAboCap = 0
                        decCapitalFaltante = decCapital + decCapPend
                        decSaldoFinal = decSaldoFinal + decCapitalFaltante
                        decIntSobra = decIntereses + decIntPend
                        decMoraSobra = decMontoMora - decAboMora

                    End If

                    'Mora Pendiente
                Else

                    decAboMoraPend = decSobraAbono
                    decAboMora = 0
                    decAboIntPend = 0
                    decAboInt = 0
                    decAboCapPend = 0
                    decAboCap = 0
                    decCapitalFaltante = decCapital + decCapPend
                    decSaldoFinal = decSaldoFinal + decCapitalFaltante
                    decIntSobra = decIntereses + decIntPend
                    decMoraSobra = decMontoMora + (decMoraPend - decAboMoraPend)

                End If

            Else
                dbAbonoRecargoporCobranza = decSobraAbono
                decAboMoraPend = 0
                decAboMora = 0
                decAboIntPend = 0
                decAboInt = 0
                decAboCapPend = 0
                decAboCap = 0
                decCapitalFaltante = decCapital + decCapPend
                decSaldoFinal = decSaldoFinal + decCapitalFaltante
                decIntSobra = decIntereses + decIntPend
                decMoraSobra = decMontoMora + (decMoraPend - decAboMoraPend)
            End If

            'Mora Pendiente
            EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))

            If Not blnRecursivo Then
                EditTextMontoAbo.AsignaValorUserDataSource(decAboCuota.ToString(n))
                EditTextAboCap.AsignaValorUserDataSource(decAboCap.ToString(n))
                EditTextAboInt.AsignaValorUserDataSource(decAboInt.ToString(n))
                EditTextAboMor.AsignaValorUserDataSource(decAboMora.ToString(n))
                EditTextCapPend.AsignaValorUserDataSource(decAboCapPend.ToString(n))
                EditTextIntPend.AsignaValorUserDataSource(decAboIntPend.ToString(n))
                EditTextMoraPend.AsignaValorUserDataSource(decAboMoraPend.ToString(n))
                EditTextRecargoCobranza.AsignaValorUserDataSource(dbAbonoRecargoporCobranza.ToString(n))
            Else
                EditTextAboCap.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextAboCap.ObtieneValorUserDataSource.ToString(n), n) + decAboCap).ToString(n))
                EditTextAboInt.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextAboInt.ObtieneValorUserDataSource.ToString(n), n) + decAboInt).ToString(n))
                EditTextAboMor.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextAboMor.ObtieneValorUserDataSource.ToString(n), n) + decAboMora).ToString(n))
                EditTextCapPend.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextCapPend.ObtieneValorUserDataSource.ToString(n), n) + decAboCapPend).ToString(n))
                EditTextIntPend.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextIntPend.ObtieneValorUserDataSource.ToString(n), n) + decAboIntPend).ToString(n))
                EditTextMoraPend.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextMoraPend.ObtieneValorUserDataSource.ToString(n), n) + decAboMoraPend).ToString(n))
                EditTextRecargoCobranza.AsignaValorUserDataSource(CDec(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n) + dbAbonoRecargoporCobranza).ToString(n))
            End If

            If blnRealizaPago = True Then

                If strTipoCuota <> "1" Then

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumero - 1, decAboCuota.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, decAboCap.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero - 1, decAboInt.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", intNumero - 1, decAboMora.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", intNumero - 1, decAboCapPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", intNumero - 1, decAboIntPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", intNumero - 1, decAboMoraPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, decSaldoFinal.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumero - 1, dtFechaPago.ToString("yyyyMMdd"))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", intNumero - 1, dbAbonoRecargoporCobranza.ToString(n))

                    If Not FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0) = intNumero Then

                        strPorcMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0)
                        strPorcMora = strPorcMora.Trim()
                        If Not String.IsNullOrEmpty(strPorcMora) Then
                            decPorcMora = Decimal.Parse(strPorcMora, n)
                            decPorcMora = decPorcMora / 100
                        End If


                        decMoraSig = decCapitalFaltante * decPorcMora

                        strCapSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumSigPago - 1)

                        strCapSig = strCapSig.Trim()
                        If Not String.IsNullOrEmpty(strCapSig) Then
                            decCapSig = Decimal.Parse(strCapSig, n)
                        End If

                        strCuotaSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumSigPago - 1)
                        strCuotaSig = strCuotaSig.Trim()
                        If Not String.IsNullOrEmpty(strCuotaSig) Then
                            decCuotaSig = Decimal.Parse(strCuotaSig, n)
                        End If

                        decSaldoInicialSig = decSaldoFinal

                        strDiasInt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumSigPago - 1)
                        strDiasInt = strDiasInt.Trim()
                        If Not String.IsNullOrEmpty(strDiasInt) Then
                            intDiasInt = Decimal.Parse(strDiasInt)
                        End If

                        decIntSig = ((decSaldoInicialSig * decIntNormal) / 360) * intDiasInt

                        decSumaMoraSig = decMoraSig + decMoraSobra

                        decSumaCapSig = decCapSig + decCapitalFaltante
                        'decSumaMoraSig = decMoraSig + decMoraSobra
                        decSumaCuotaSig = decCapSig + decIntSig + decCapitalFaltante + decIntSobra + decSumaMoraSig
                        decSaldoFinalSig = decSaldoInicialSig - decSumaCapSig

                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", intNumSigPago - 1, decSaldoInicialSig.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumSigPago - 1, decSumaCuotaSig.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumSigPago - 1, decIntSig.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", intNumSigPago - 1, decCapitalFaltante.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", intNumSigPago - 1, decIntSobra.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", intNumSigPago - 1, decSumaMoraSig.ToString(n))
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumSigPago - 1, decSaldoFinalSig.ToString(n))

                    End If
                Else

                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero - 1).Trim <> "P" Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ToCuota", intNumero - 1, (CDbl(decAboCap) + CDbl(decAboInt) + CDbl(decAboMora) + CDbl(dbAbonoRecargoporCobranza)).ToString(n))
                    ElseIf Not blnRecursivo Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ToCuota", intNumero - 1, (CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ToCuota", intNumero - 1), n)) + (CDbl(General.ConvierteDecimal(EditTextAboCap.ObtieneValorUserDataSource().ToString(n), n)) + CDbl(General.ConvierteDecimal(EditTextAboInt.ObtieneValorUserDataSource().ToString(n), n)) + CDbl(General.ConvierteDecimal(EditTextAboMor.ObtieneValorUserDataSource().ToString(n), n)) + CDbl(General.ConvierteDecimal(EditTextRecargoCobranza.ObtieneValorUserDataSource().ToString(n), n)))).ToString(n))
                    End If

                    'Campos nuevos para pagos parciales
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ToCapPagado", intNumero - 1, (CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ToCapPagado", intNumero - 1), n)) + decAboCap).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ToIntPagado", intNumero - 1, (CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ToIntPagado", intNumero - 1), n)) + decAboInt).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ToMoPagado", intNumero - 1, (CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ToMoPagado", intNumero - 1), n)) + decAboMora).ToString(n))

                    If decCapitalFaltante <= 0.1 Then
                        decCapitalFaltante = 0
                    End If
                    If decIntSobra <= 0.1 Then
                        decIntSobra = 0
                    End If
                    If decMoraSobra <= 0.1 Then
                        decMoraSobra = 0
                    End If
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, CDec(decCapitalFaltante).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero - 1, CDec(decIntSobra).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", intNumero - 1, CDec(decMoraSobra).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", intNumero - 1, 0)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", intNumero - 1, 0)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", intNumero - 1, 0)
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", intNumero - 1, (General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ReCo", intNumero - 1), n) + dbAbonoRecargoporCobranza).ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, (General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumero - 1), n) - General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_ToCapPagado", intNumero - 1), n)).ToString(n))
                    If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Mor", intNumero - 1)) OrElse FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Mor", intNumero - 1) = 0 Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Mor", intNumero - 1, EditTextDiasMora.ObtieneValorUserDataSource())
                    End If
                    If decCapitalFaltante <= 0 Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumero - 1, "Y")
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumero - 1, "P")
                    End If

                    If Math.Truncate(decAbono) > 0 AndAlso FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero - 1).Trim = "Y" Then
                        ManejoAbonoCuotaSig(decAbono, intNumero + 1, strTipoCuota, blnRealizaPago)
                    End If

                End If
            Else
                If Math.Truncate(decAbono) > 0 Then
                    ManejoAbonoCuotaSig(decAbono, intNumero + 1, strTipoCuota, blnRealizaPago)
                End If
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub ManejoAbonoCuotaSig(ByVal decAbono As Double, ByVal intNumero As Integer, ByVal strTipoCuota As String, ByVal blnRealizaPago As Boolean)

        Dim intPlazoPres As Integer
        Dim decPorcMoraPres As Double
        Dim strCancelarMora As String
        Dim intDiasMora As Integer
        Dim dtFechaPago As Date
        Dim dtFechaTeorica As Date
        Dim dbCargoCobranza As Double
        Dim dbCapitalVencido As Double
        Dim decMora As Double
        Dim intDiasInt As Integer
        Dim decMontoMora As Double
        Dim intDiasSigPAgo As Integer
        Dim intNumPosicSigPago As Integer
        Dim decIntereses As Double
        Dim decSaldoFinal As Double
        Dim decCuota As Double
        Dim decIntNormal As Double
        Dim decCapital As Double
        Dim strFechaTeoricaPlan As String
        Dim strFechaPago As String

        Try

            intPlazoPres = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()

            If intNumero <= intPlazoPres Then

                decPorcMoraPres = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0).Trim(), n) / 100
                strCancelarMora = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0).Trim()
                strFechaPago = EditTextFechaPago.ObtieneValorUserDataSource().ToString()
                If Not String.IsNullOrEmpty(strFechaPago) Then
                    dtFechaPago = Date.ParseExact(strFechaPago, "yyyyMMdd", Nothing)
                    dtFechaPago = New Date(dtFechaPago.Year, dtFechaPago.Month, dtFechaPago.Day, 0, 0, 0)
                End If
                strFechaTeoricaPlan = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strFechaTeoricaPlan) Then
                    dtFechaTeorica = Date.ParseExact(strFechaTeoricaPlan, "yyyyMMdd", Nothing)
                    dtFechaTeorica = New Date(dtFechaTeorica.Year, dtFechaTeorica.Month, dtFechaTeorica.Day, 0, 0, 0)
                End If
                intDiasInt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero - 1)
                intDiasSigPAgo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero - 1)
                decIntereses = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Interes", intNumero - 1).ToString(n), n)
                decSaldoFinal = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Fin", intNumero - 1).ToString(n), n)
                decCuota = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumero - 1).ToString(n), n)
                decIntNormal = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Interes", 0).ToString(n), n) / 100
                decCapital = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1).ToString(n), n)

                Call DeterminarDiasEntrePagos(dtFechaPago, dtFechaTeorica, intDiasMora)
                EditTextDiasMora.AsignaValorUserDataSource(intDiasMora.ToString())
                If intDiasMora > 0 Then

                    If Not strCancelarMora = "Y" Then
                        dbCapitalVencido = General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1), n)
                        decMontoMora = (dbCapitalVencido * decPorcMoraPres) * (intDiasMora / 360)
                    End If

                    intNumPosicSigPago = intNumero
                    If FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Can_Mora", 0).Trim() = "N" Then
                        dbCargoCobranza += dataTableConsulta.GetValue("U_MontM", 0)
                    End If

                    'While intDiasMora >= intDiasSigPAgo

                    '    intDiasMora -= intDiasSigPAgo

                    '    intDiasSigPAgo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumPosicSigPago)
                    '    intNumPosicSigPago += 1

                    'End While

                End If

                ManejoPagosMenores(decAbono, decMontoMora, decIntereses, decSaldoFinal, decCuota, blnRealizaPago, intNumero, dtFechaPago, dtFechaTeorica, strTipoCuota, decIntNormal, decCapital, 0, 0, 0, intNumero + 1, True, dbCargoCobranza)
            Else
                If Not blnRealizaPago Then
                    EditTextMontoAbo.AsignaValorUserDataSource(EditTextMontoAbo.ObtieneValorUserDataSource - decAbono)
                    ButtonCalcular.ItemSBO.Click()
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Se recalcula el plan de pagos a partir del primer pago pendiente de abonar en adelante cuando se realiza un cambio de la tasa de interes normal anual

    Private Sub CalculoCuotasIntNormal(ByVal intNumero As Integer, ByVal strTipoCuota As String, ByVal decIntNormal As Decimal, ByVal dtFechaPago As Date)

        Dim n As NumberFormatInfo

        Dim strPlazo As String
        Dim intPlazo As Integer = 0
        Dim strDiaPago As String
        Dim intDiaPago As Integer = 0
        Dim intCantPagos As Integer
        Dim intPosicion As Integer = 0
        Dim strSaldo As String
        Dim decSaldo As Decimal
        Dim dtFechaTeorica As Date
        Dim strDiasInt As String
        Dim intDiasInt As Integer
        Dim strPagoAdelantado As String = ""
        Dim strCapPend As String
        Dim decCapPend As Decimal
        Dim strIntPend As String
        Dim decIntPend As Decimal
        Dim strMoraPend As String
        Dim decMoraPend As Decimal
        Dim strPagoCancelado As String
        Dim strPrestamo As String

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strDiaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_DiaPago", 0).Trim()
            If Not String.IsNullOrEmpty(strDiaPago) Then intDiaPago = Integer.Parse(strDiaPago)

            strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
            If Not String.IsNullOrEmpty(strPlazo) Then intPlazo = Integer.Parse(strPlazo)

            strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()

            strPagoCancelado = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Sal_Ini = 0 And U_Pago_Aso IS NOT NULL ORDER BY U_Numero", StrConexion)

            If Not String.IsNullOrEmpty(strPagoCancelado) Then
                intPlazo = Integer.Parse(strPagoCancelado)
                intPlazo = intPlazo - 1
            End If

            intCantPagos = intPlazo - intNumero + 1

            dtFechaTeorica = dtFechaPago.AddMonths(-1)

            If intNumero <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").Size Then
                strSaldo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strSaldo) Then decSaldo = Decimal.Parse(strSaldo, n)

                strCapPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cap_Pend", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strCapPend) Then decCapPend = Decimal.Parse(strCapPend, n)

                strIntPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Int_Pend", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strIntPend) Then decIntPend = Decimal.Parse(strIntPend, n)

                strMoraPend = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Mor_Pend", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strMoraPend) Then decMoraPend = Decimal.Parse(strMoraPend, n)

                If strTipoCuota = "2" Then

                    strDiasInt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero - 1).Trim()

                    If Not String.IsNullOrEmpty(strDiasInt) Then
                        intDiasInt = Integer.Parse(strDiasInt)
                    End If

                    decSaldo = decSaldo - decCapPend

                    Call _formPlanPlagos.CalculoNivelada(intCantPagos, decSaldo, decIntNormal, dtFechaTeorica, intDiaPago, True, "N", intDiasInt, 0, False, decCapPend, decIntPend, decMoraPend)

                ElseIf strTipoCuota = "3" Then

                    Call _formPlanPlagos.CalculoGlobal(intCantPagos, decSaldo, decIntNormal, dtFechaTeorica, intDiaPago)

                ElseIf strTipoCuota = "4" Then

                    decSaldo = decSaldo - decCapPend

                    Call _formPlanPlagos.CalculoDecreciente(intCantPagos, decSaldo, decIntNormal, dtFechaTeorica, intDiaPago, "N", 0, True, decCapPend, decIntPend, decMoraPend)

                End If
            End If



        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' Determina cantidad de dias de diferencia entre una fecha y la otra
    ''' </summary>
    ''' <param name="dtFechaPago">Fecha de pago</param>
    ''' <param name="dtFechaTeorica">Fecha de Pago Teórica</param>
    ''' <param name="intCantDias">Retorno Cantidad de dias de diferencia</param>
    ''' <remarks></remarks>
    Private Sub DeterminarDiasEntrePagos(ByVal dtFechaPago As Date, ByVal dtFechaTeorica As Date, ByRef intCantDias As Integer)

        Dim intCantMeses As Integer
        Dim intCantAños As Integer

        Try

            If dtFechaPago.Month = dtFechaTeorica.Month AndAlso dtFechaPago.Year = dtFechaTeorica.Year Then

                intCantDias = dtFechaPago.Day - dtFechaTeorica.Day

            ElseIf dtFechaPago.Month > dtFechaTeorica.Month AndAlso dtFechaPago.Year = dtFechaTeorica.Year Then

                intCantMeses = dtFechaPago.Month - dtFechaTeorica.Month

                If intCantMeses > 1 Then
                    intCantMeses = intCantMeses - 1
                    intCantDias = (30 - dtFechaTeorica.Day) + dtFechaPago.Day + (30 * intCantMeses)
                Else
                    intCantDias = (30 - dtFechaTeorica.Day) + dtFechaPago.Day
                End If

            ElseIf dtFechaPago.Year > dtFechaTeorica.Year Then

                intCantAños = dtFechaPago.Year - dtFechaTeorica.Year

                intCantAños = intCantAños - 1
                intCantMeses = (12 - dtFechaTeorica.Month) + dtFechaPago.Month + (12 * intCantAños)
                intCantMeses = intCantMeses - 1
                intCantDias = (30 - dtFechaTeorica.Day) + dtFechaPago.Day + (30 * intCantMeses)

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calcula los montos de un pago adelantado, esto se da cuando un cliente paga antes de la fecha en que le corresponde abonar, se le debe cobrar menos de intereses porque
    'se le cobran menos días de intereses, el resto del pago se abona a capital, y al siguiente pago debe pagar menos de capital pero más días de intereses por la diferencia
    'de días entre los pagos

    Private Sub CalcularPagosAdelantadosNivelada(ByVal dtFechaPago As Date, ByVal intNumero As Integer, ByVal decCuota As Decimal, ByVal blnRealizaPago As Boolean, ByVal decIntNormal As Decimal, _
                                                 ByVal decCapPend As Decimal, ByVal decIntPend As Decimal, ByVal decMoraPend As Decimal)

        Dim n As NumberFormatInfo

        Dim decIntereses As Decimal
        Dim intDiaInt As Integer
        Dim strSaldoInicial As String
        Dim decSaldoInicial As Decimal = 0
        Dim strFechaAnterior As String = ""
        Dim dtFechaAnterior As Date
        Dim strDiaPago As String = ""
        Dim intDiaPago As Integer = 0
        Dim decCapital As Decimal
        Dim decSaldoFinal As Decimal

        Dim strFechaSigPago As String
        Dim dtFechaSigPago As Date
        Dim strCapAnterior As String
        Dim decCapAnterior As Decimal
        Dim decDiferenciaCap As Decimal
        Dim decCapitalSig As Decimal
        Dim strCapitalSig As String
        Dim strPlazo As String
        Dim intPlazo As Integer
        Dim intNumeroSigPago As Integer
        Dim intCantMeses As Integer
        Dim intCantAños As Integer
        Dim blnIntSigCanc As Boolean = False
        Dim strFechaPrestamo As String
        Dim dtFechaPrestamo As Date
        Dim intDifDiasInicial As Integer = 0
        Dim strPagoAnterior As String
        Dim intPagoAnterior As Integer
        Dim strPrestamo As String
        Dim strCobraMoratorios As String = ""
        'Dim oMatrix As SAPbouiCOM.Matrix

        Try
            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strCapAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strCapAnterior) Then decCapAnterior = Decimal.Parse(strCapAnterior, n)

            strDiaPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_DiaPago", 0).Trim()
            If Not String.IsNullOrEmpty(strDiaPago) Then intDiaPago = Integer.Parse(strDiaPago)

            strFechaPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Fec_Pres", 0).Trim()
            If Not String.IsNullOrEmpty(strFechaPrestamo) Then
                dtFechaPrestamo = Date.ParseExact(strFechaPrestamo, "yyyyMMdd", Nothing)
                dtFechaPrestamo = New Date(dtFechaPrestamo.Year, dtFechaPrestamo.Month, dtFechaPrestamo.Day, 0, 0, 0)
            End If

            If intNumero = 1 Then

                If dtFechaPago.Month = dtFechaPrestamo.Month Then
                    intDiaInt = dtFechaPago.Day - dtFechaPrestamo.Day
                Else
                    intDiaInt = 30 - dtFechaPrestamo.Day + dtFechaPago.Day
                End If

            ElseIf intNumero > 1 Then

                strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocNum", 0)
                strPrestamo = strPrestamo.Trim()

                strPagoAnterior = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Cuota > 0 And U_Numero < " & intNumero.ToString() & " ORDER BY U_Numero DESC", StrConexion)
                If Not String.IsNullOrEmpty(strPagoAnterior) Then
                    intPagoAnterior = Integer.Parse(strPagoAnterior)
                End If

                strCobraMoratorios = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cobra_Mora", intPagoAnterior - 1)
                strCobraMoratorios = strCobraMoratorios.Trim()

                If strCobraMoratorios = "Y" Or String.IsNullOrEmpty(strCobraMoratorios) Then

                    strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intPagoAnterior - 1)
                    strFechaAnterior = strFechaAnterior.Trim()

                ElseIf strCobraMoratorios = "N" Then

                    strFechaAnterior = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_TEORICO").GetValue("U_Fecha", intPagoAnterior - 1)
                    strFechaAnterior = strFechaAnterior.Trim()

                End If

                If Not String.IsNullOrEmpty(strFechaAnterior) Then
                    dtFechaAnterior = Date.ParseExact(strFechaAnterior, "yyyyMMdd", Nothing)
                    dtFechaAnterior = New Date(dtFechaAnterior.Year, dtFechaAnterior.Month, dtFechaAnterior.Day, 0, 0, 0)
                End If

                If dtFechaPago.Month = dtFechaAnterior.Month AndAlso dtFechaPago.Year = dtFechaAnterior.Year Then

                    intDiaInt = dtFechaPago.Day - dtFechaAnterior.Day

                ElseIf dtFechaPago.Month > dtFechaAnterior.Month AndAlso dtFechaPago.Year = dtFechaAnterior.Year Then

                    intCantMeses = dtFechaPago.Month - dtFechaAnterior.Month

                    If intCantMeses > 1 Then

                        intCantMeses = intCantMeses - 1
                        intDiaInt = (30 - dtFechaAnterior.Day) + dtFechaPago.Day + (30 * intCantMeses)

                    Else

                        intDiaInt = (30 - dtFechaAnterior.Day) + dtFechaPago.Day

                    End If

                ElseIf dtFechaPago.Year > dtFechaAnterior.Year Then

                    intCantAños = dtFechaPago.Year - dtFechaAnterior.Year

                    intCantAños = intCantAños - 1
                    intCantMeses = (12 - dtFechaAnterior.Month) + dtFechaPago.Month + (12 * intCantAños)
                    intCantMeses = intCantMeses - 1
                    intDiaInt = (30 - dtFechaAnterior.Day) + dtFechaPago.Day + (30 * intCantMeses)

                End If

            End If

            If intDiaInt < 0 Then intDiaInt = 0

            strSaldoInicial = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strSaldoInicial) Then decSaldoInicial = Decimal.Parse(strSaldoInicial, n)

            strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
            If Not String.IsNullOrEmpty(strPlazo) Then intPlazo = Integer.Parse(strPlazo)

            decIntereses = ((decSaldoInicial * decIntNormal) / 360) * intDiaInt

            decCapital = decCuota - decIntereses - decCapPend - decIntPend - decMoraPend

            decSaldoFinal = decSaldoInicial - decCapital - decCapPend

            If blnRealizaPago = True Then

                'Valores en pago actual

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Ade", intNumero - 1, "Y")

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero - 1, decIntereses.ToString(n))
                EditTextAboInt.AsignaValorUserDataSource(decIntereses.ToString(n))

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, decCapital.ToString(n))
                EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumero - 1, intDiaInt.ToString(n))
                EditTextDiasInt.AsignaValorUserDataSource(intDiaInt.ToString(n))

                If decSaldoFinal < 0 AndAlso intNumero = intPlazo Then

                    decCuota = decCuota + decSaldoFinal
                    decSaldoFinal = 0
                    decCapital = decCuota - decIntereses - decCapPend - decIntPend - decMoraPend

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, decSaldoFinal)
                    EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumero - 1, decCuota)
                    EditTextMontoAbo.AsignaValorUserDataSource(decCuota.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, decCapital)
                    EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))

                End If

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, decSaldoFinal.ToString(n))
                EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumero - 1, dtFechaPago.ToString("yyyyMMdd"))

                'Valores para siguiente pago

                intNumeroSigPago = intNumero + 1

                If Not intNumeroSigPago > intPlazo Then

                    strFechaSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero)
                    strFechaSigPago = strFechaSigPago.Trim()
                    If Not String.IsNullOrEmpty(strFechaSigPago) Then
                        dtFechaSigPago = Date.ParseExact(strFechaSigPago, "yyyyMMdd", Nothing)
                        dtFechaSigPago = New Date(dtFechaSigPago.Year, dtFechaSigPago.Month, dtFechaSigPago.Day, 0, 0, 0)
                    End If

                    If dtFechaSigPago.Year = dtFechaPago.Year Then

                        intCantMeses = dtFechaSigPago.Month - dtFechaPago.Month

                    ElseIf dtFechaSigPago.Year > dtFechaPago.Year Then

                        intCantAños = dtFechaSigPago.Year - dtFechaPago.Year
                        intCantAños = intCantAños - 1
                        intCantMeses = (12 - dtFechaPago.Month) + dtFechaSigPago.Month + (12 * intCantAños)

                    End If

                    intDiaInt = (intDiaPago - dtFechaPago.Day) + (30 * intCantMeses)

                    decIntereses = ((decSaldoFinal * decIntNormal) / 360) * intDiaInt

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero, decIntereses.ToString(n))

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumero, intDiaInt)

                    decDiferenciaCap = decCapital - decCapAnterior
                    strCapitalSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero)
                    strCapitalSig = strCapitalSig.Trim()
                    If Not String.IsNullOrEmpty(strCapitalSig) Then
                        decCapitalSig = Decimal.Parse(strCapitalSig, n)
                    End If
                    decCapitalSig = decCapitalSig - decDiferenciaCap

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero, decCapitalSig.ToString(n))

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Sig", intNumero - 1, decCapitalSig.ToString(n))

                    decCuota = decIntereses + decCapitalSig

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumero, decCuota.ToString(n))

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", intNumero, decSaldoFinal.ToString(n))

                    decSaldoFinal = decSaldoFinal - decCapitalSig

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero, decSaldoFinal.ToString(n))

                End If

            ElseIf blnRealizaPago = False Then

                EditTextAboInt.AsignaValorUserDataSource(decIntereses.ToString(n))

                EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))

                EditTextDiasInt.AsignaValorUserDataSource(intDiaInt.ToString(n))

                If decSaldoFinal < 0 AndAlso intNumero = intPlazo Then

                    decCuota = decCuota + decSaldoFinal
                    decSaldoFinal = 0
                    decCapital = decCuota - decIntereses - decCapPend - decIntPend - decMoraPend

                    EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))
                    EditTextMontoAbo.AsignaValorUserDataSource(decCuota.ToString(n))
                    EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))

                End If

                EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calcula los pagos extraordinarios, estos son pagos que son más grandes que la cuota que deben abonar, se abona los intereses primero y lo que sobra va a capital.
    'El plan de pagos se recalcula con base en el capital pendiente de abonar (saldo), se tienen dos posibilidades, recalcular las cuotas con base en el saldo pendiente
    'y el plazo restante, o mantener la cuota y disminuir el plazo del plan de pagos

    Private Sub CalcularPagosExtraordinarios(ByVal decAbono As Decimal, ByVal decCuota As Decimal, ByVal intNumero As Integer, ByVal blnRealizaPago As Boolean, ByVal strTipoCuota As String, ByVal dtFechaTeorica As Date, _
                                             ByVal decInteres As Decimal, ByVal dtFechaPago As Date, ByVal decIntNormal As Decimal, ByVal decMoraActual As Decimal, ByVal decCapPend As Decimal, ByVal decIntPend As Decimal, ByVal decMoraPend As Decimal, _
                                             Optional ByVal intNumSigPago As Integer = 0)

        Dim n As NumberFormatInfo

        Dim decCapital As Decimal = 0
        Dim strSaldoInicial As String
        Dim decSaldoInicial As Decimal = 0
        Dim decSaldoFinal As Decimal = 0
        Dim strPlazo As String
        Dim intPlazo As Integer = 0
        Dim intCantPagosPend As Integer
        Dim intPosicion As Integer = 0
        Dim strCambiaPlazo As String = ""
        Dim strCapExtraOrdDecreciente As String
        Dim decCapExtraDecreciente As Decimal = 0
        Dim strCuotaOriginalNivelada As String
        Dim decCuotaOriginalNivelada As Decimal = 0
        Dim strDiasInt As String
        Dim intDiasInt As Integer
        Dim strPrestamo As String
        Dim strPagoAsociado As String
        Dim strPagoCancelado As String

        Dim intDiasInteres As Integer

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
            If Not String.IsNullOrEmpty(strPlazo) Then intPlazo = Integer.Parse(strPlazo)

            strPrestamo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("DocEntry", 0).Trim()

            strPagoCancelado = General.EjecutarConsulta("Select TOP 1 U_Numero From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Pagado = 'Y' And U_Sal_Ini = 0 And U_Pago_Aso IS NOT NULL ORDER BY U_Numero", StrConexion)

            If Not String.IsNullOrEmpty(strPagoCancelado) Then
                intPlazo = Integer.Parse(strPagoCancelado)
                intPlazo = intPlazo - 1
            End If

            decCapital = decAbono - decInteres - decMoraActual - decCapPend - decIntPend - decMoraPend

            strSaldoInicial = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumero - 1).Trim()
            If Not String.IsNullOrEmpty(strSaldoInicial) Then decSaldoInicial = Decimal.Parse(strSaldoInicial, n)

            decSaldoFinal = decSaldoInicial - decCapital - decCapPend

            If decCapital > decSaldoInicial Then

                decCapital = decSaldoInicial
                decAbono = decCapital + decInteres + decMoraActual + decCapPend + decIntPend + decMoraPend
                decSaldoFinal = 0
                EditTextMontoAbo.AsignaValorUserDataSource(decAbono.ToString(n))

            End If

            If Not intNumero = intPlazo Then

                If blnRealizaPago = True Then

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Ext", intNumero - 1, "Y")

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumero - 1, dtFechaPago.ToString("yyyyMMdd"))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, decCapital.ToString(n))
                    EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero - 1, decInteres.ToString(n))
                    EditTextAboInt.AsignaValorUserDataSource(decInteres.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cap_Pend", intNumero - 1, decCapPend.ToString(n))
                    EditTextCapPend.AsignaValorUserDataSource(decCapPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Pend", intNumero - 1, decIntPend.ToString(n))
                    EditTextIntPend.AsignaValorUserDataSource(decIntPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mor_Pend", intNumero - 1, decMoraPend.ToString(n))
                    EditTextMoraPend.AsignaValorUserDataSource(decMoraPend.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumero - 1, decAbono.ToString(n))
                    EditTextMontoAbo.AsignaValorUserDataSource(decAbono.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, decSaldoFinal.ToString(n))
                    EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))

                    If decSaldoFinal > 0 Then

                        strCambiaPlazo = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_ModPlazo", 0)
                        strCambiaPlazo = strCambiaPlazo.Trim()

                        intCantPagosPend = intPlazo - intNumSigPago + 1

                        strCuotaOriginalNivelada = General.EjecutarConsulta("Select U_Cuota From [@SCGD_PLAN_REAL] Where DocEntry = '" & strPrestamo & "' And U_Numero = '" & intNumSigPago & "'", StrConexion)
                        If Not String.IsNullOrEmpty(strCuotaOriginalNivelada) Then
                            decCuotaOriginalNivelada = Decimal.Parse(strCuotaOriginalNivelada)
                        End If

                        If strTipoCuota = "1" OrElse strTipoCuota = "2" Then

                            strDiasInt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumSigPago - 1)
                            strDiasInt = strDiasInt.Trim()
                            If Not String.IsNullOrEmpty(strDiasInt) Then
                                intDiasInt = Integer.Parse(strDiasInt)
                            End If

                            Call _formPlanPlagos.CalculoNivelada(intCantPagosPend, decSaldoFinal, decIntNormal, dtFechaTeorica, dtFechaTeorica.Day, False, strCambiaPlazo, intDiasInt, decCuotaOriginalNivelada, True)

                        ElseIf strTipoCuota = "4" Then

                            strCapExtraOrdDecreciente = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumSigPago - 1)
                            strCapExtraOrdDecreciente = strCapExtraOrdDecreciente.Trim()
                            If Not String.IsNullOrEmpty(strCapExtraOrdDecreciente) Then
                                decCapExtraDecreciente = Decimal.Parse(strCapExtraOrdDecreciente, n)
                            End If

                            Call _formPlanPlagos.CalculoDecreciente(intCantPagosPend, decSaldoFinal, decIntNormal, dtFechaTeorica, dtFechaTeorica.Day, strCambiaPlazo, decCapExtraDecreciente)

                        End If

                        If strCambiaPlazo = "Y" Then

                            If strTipoCuota = "1" OrElse strTipoCuota = "2" Then

                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mon_Niv", intNumero - 1, decCuotaOriginalNivelada.ToString(n))

                            ElseIf strTipoCuota = "4" Then

                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Mon_Niv", intNumero - 1, decCapExtraDecreciente.ToString(n))

                            End If

                        End If

                        For i As Integer = intNumSigPago - 1 To intPlazo - 1

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", i, _formPlanPlagos.g_decSaldoInicial(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", i, _formPlanPlagos.g_decCuota(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", i, _formPlanPlagos.g_decCapital(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", i, _formPlanPlagos.g_decInteres(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", i, _formPlanPlagos.g_decMoratorios(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", i, _formPlanPlagos.g_intDiasInt(intPosicion).ToString())
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", i, _formPlanPlagos.g_decSaldoFinal(intPosicion).ToString(n))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", i, _formPlanPlagos.g_strPagado(intPosicion))
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", intNumero - 1, EditTextRecargoCobranza.ObtieneValorUserDataSource())

                            If strCambiaPlazo = "Y" AndAlso _formPlanPlagos.g_decSaldoInicial(intPosicion) = 0 AndAlso _formPlanPlagos.g_strPagado(intPosicion) = "Y" Then

                                strPagoAsociado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pago_Aso", i)
                                strPagoAsociado = strPagoAsociado.Trim()

                                If String.IsNullOrEmpty(strPagoAsociado) Then

                                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Aso", i, intNumero.ToString())

                                End If

                            End If

                            intPosicion = intPosicion + 1

                        Next

                    ElseIf decSaldoFinal <= 0 Then

                        For i As Integer = intNumSigPago - 1 To intPlazo - 1

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", i, "0")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", i, "Y")
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", i, 0)
                            strPagoAsociado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pago_Aso", i)
                            strPagoAsociado = strPagoAsociado.Trim()

                            If String.IsNullOrEmpty(strPagoAsociado) Then

                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pago_Aso", i, intNumero.ToString())

                            End If

                        Next

                    End If

                ElseIf blnRealizaPago = False Then

                    EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))
                    EditTextAboInt.AsignaValorUserDataSource(decInteres.ToString(n))
                    EditTextMontoAbo.AsignaValorUserDataSource(decAbono.ToString(n))
                    EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))
                    EditTextCapPend.AsignaValorUserDataSource(decCapPend.ToString(n))
                    EditTextIntPend.AsignaValorUserDataSource(decIntPend.ToString(n))
                    EditTextMoraPend.AsignaValorUserDataSource(decMoraPend.ToString(n))


                End If

            ElseIf intNumero = intPlazo Then

                EditTextMontoAbo.AsignaValorUserDataSource(decCuota.ToString(n))

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Realiza el calculo de intereses moratorios, esto se da cuando un cliente abona después de la fecha en que debe hacerlo, se calcula con base en la tasa de interes moratorio
    'mensual y los días de atraso que tuvo con respecto a la fecha en que debió abonar, además se debe abonar más interes normal ya que son más días de intereses por lo que la cuota
    'aumenta, para el siguiente pago se cobra menos intereses normales porque son menos días de interes normal lo que se debe cobrar por la cantidad de días

    Private Sub CalcularInteresesMoratorios(ByRef decCuota As Decimal, ByVal strTipoCuota As String, ByVal intNumero As Integer, ByVal dtFechaPago As Date, _
                                            ByVal dtFechaTeorica As Date, ByVal blnRealizaPago As Boolean, ByRef decMontoMora As Decimal, _
                                            ByRef intDiasInt As Integer, ByRef decIntereses As Decimal, ByVal decTasaIntNormal As Decimal, ByRef decSaldoFinal As Decimal, _
                                            ByRef decCapital As Decimal, ByVal strCancelarMora As String, Optional ByRef intNumSigPago As Integer = 0)

        Dim n As NumberFormatInfo

        Dim decMora As Decimal
        Dim strPorcMoraPres As String
        Dim decPorcMoraPres As Decimal = 0
        Dim strPagoAdelantado As String = ""
        Dim intDiasMora As Integer
        Dim strSaldoInicial As String
        Dim decSaldoInicial As Decimal
        Dim decInteresNormalDiasMora As Decimal
        Dim strPlazoPres As String
        Dim intPlazoPres As Integer
        Dim strDiasSigPago As String
        Dim intDiasSigPago As Integer
        Dim strInteresSigPago As String
        Dim decInteresSigPago As Decimal
        Dim strDiasIntSigPago As String
        Dim intDiasIntSigPago As Integer
        Dim strCapSigPago As String
        Dim decCapSigPago As Decimal
        Dim strSalIniSigPago As String
        Dim decSalIniSigPago As Decimal
        Dim strCuotaSigPago As String
        Dim decCuotaSigPago As Decimal
        Dim strEstadoPagoSig As String
        Dim intNumPosicSigPago As Integer
        Dim decMoraSigPago As Decimal
        Dim decMontoMoraSigPago As Decimal
        Dim intDiasMoraSigPago As Integer
        Dim intDiasMoraTotal As Integer
        Dim strMontoMora As String = String.Empty
        Dim dbCargoCobranza As Double = 0
        Dim dbCapitalVencido As Double = 0

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strPlazoPres = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0).Trim()
            If Not String.IsNullOrEmpty(strPlazoPres) Then intPlazoPres = Decimal.Parse(strPlazoPres)

            strPorcMoraPres = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Int_Mora", 0).Trim()
            If Not String.IsNullOrEmpty(strPorcMoraPres) Then
                decPorcMoraPres = Decimal.Parse(strPorcMoraPres, n)
                decPorcMoraPres = decPorcMoraPres / 100
            End If

            Call DeterminarDiasEntrePagos(dtFechaPago, dtFechaTeorica, intDiasMora)

            If Not strCancelarMora = "Y" Then
                ' dbCargoCobranza = dataTableConsulta.GetValue("U_MontM", 0)
                If strTipoCuota <> "1" Then
                    decMora = (decCuota * decPorcMoraPres) / intDiasInt
                    decMontoMora = decMora * intDiasMora
                Else
                    dbCapitalVencido = CDbl(General.ConvierteDecimal(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumero - 1).ToString(n), n))
                    'decMora = (dbCapitalVencido * decPorcMoraPres) / intDiasInt
                    decMontoMora = (dbCapitalVencido * decPorcMoraPres) * (intDiasMora / 360)
                End If
            End If

            If strTipoCuota = "2" Then

                strSaldoInicial = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumero - 1).Trim()
                If Not String.IsNullOrEmpty(strSaldoInicial) Then decSaldoInicial = Decimal.Parse(strSaldoInicial, n)

                decInteresNormalDiasMora = ((decSaldoInicial * decTasaIntNormal) / 360) * intDiasMora

                decIntereses = decIntereses + decInteresNormalDiasMora

                intDiasInt = intDiasInt + intDiasMora

                decCuota = decCuota + decInteresNormalDiasMora

            End If

            decCuota = decCuota + decMontoMora

            intDiasMoraTotal = intDiasMora

            If intNumero < intPlazoPres Then

                strEstadoPagoSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumero).Trim()

                If strEstadoPagoSig = "N" Then

                    strDiasSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumero).Trim()
                    If Not String.IsNullOrEmpty(strDiasSigPago) Then intDiasSigPago = Integer.Parse(strDiasSigPago)

                    intNumPosicSigPago = intNumero

                    'Se determina si existen pagos entre las fechas de mora y del plan de pagos
                    If strTipoCuota <> "1" Then
                        While intDiasMora >= intDiasSigPago

                            If intNumPosicSigPago < intPlazoPres Then

                                strEstadoPagoSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumPosicSigPago)
                                strEstadoPagoSig = strEstadoPagoSig.Trim()

                                If strEstadoPagoSig = "N" Then

                                    If strTipoCuota = "4" OrElse strTipoCuota = "3" Then

                                        strInteresSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Interes", intNumPosicSigPago).Trim()
                                        If Not String.IsNullOrEmpty(strInteresSigPago) Then decInteresSigPago = Decimal.Parse(strInteresSigPago, n)

                                        strDiasIntSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumPosicSigPago).Trim()
                                        If Not String.IsNullOrEmpty(strDiasIntSigPago) Then intDiasIntSigPago = Integer.Parse(strDiasIntSigPago)

                                        decIntereses = decIntereses + decInteresSigPago
                                        intDiasInt = intDiasInt + intDiasIntSigPago
                                        decCuota = decCuota + decInteresSigPago

                                    End If

                                    strCapSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumPosicSigPago).Trim()
                                    If Not String.IsNullOrEmpty(strCapSigPago) Then decCapSigPago = Decimal.Parse(strCapSigPago, n)

                                    decCapital = decCapital + decCapSigPago
                                    decCuota = decCuota + decCapSigPago

                                    strCuotaSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumPosicSigPago).Trim()
                                    If Not String.IsNullOrEmpty(strCuotaSigPago) Then decCuotaSigPago = Decimal.Parse(strCuotaSigPago, n)


                                    dbCargoCobranza += dataTableConsulta.GetValue("U_MontM", 0)

                                    decMoraSigPago = (decCuotaSigPago * decPorcMoraPres) / intDiasSigPago
                                    intDiasMoraSigPago = intDiasMora - intDiasSigPago
                                    decMontoMoraSigPago = decMoraSigPago * intDiasMoraSigPago


                                    decMontoMora = decMontoMora + decMontoMoraSigPago
                                    If strTipoCuota <> "1" Then
                                        decCuota = decCuota + decMontoMoraSigPago
                                    End If

                                    decSaldoFinal = decSaldoFinal - decCapSigPago

                                    If blnRealizaPago = True AndAlso strTipoCuota <> "1" Then
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Ini", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumPosicSigPago, "0")
                                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Pagado", intNumPosicSigPago, "Y")
                                    End If

                                    intNumPosicSigPago = intNumPosicSigPago + 1

                                    If intNumPosicSigPago < intPlazoPres Then

                                        strEstadoPagoSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumPosicSigPago).Trim()

                                        If strEstadoPagoSig = "N" Then

                                            intDiasMora = intDiasMora - intDiasSigPago

                                            strDiasSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumPosicSigPago).Trim()
                                            If Not String.IsNullOrEmpty(strDiasSigPago) Then intDiasSigPago = Integer.Parse(strDiasSigPago)

                                        End If

                                    End If

                                ElseIf strEstadoPagoSig = "Y" Then

                                    m_blnPermitirMoraMenor = False

                                    Exit While

                                End If

                            ElseIf intNumPosicSigPago = intPlazoPres Then

                                m_blnPermitirMoraMenor = False

                                Exit While

                            End If

                        End While
                    Else
                        intNumPosicSigPago = intNumero
                        If Not strCancelarMora = "Y" Then
                            dbCargoCobranza += dataTableConsulta.GetValue("U_MontM", 0)
                        End If
                        'While intDiasMora >= intDiasSigPago

                        '    intDiasMora -= intDiasSigPago
                        '    intNumPosicSigPago += 1
                        '    intDiasSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Dias_Int", intNumPosicSigPago)

                        'End While
                    End If


                    'intNumSigPago = intNumPosicSigPago + 1

                    'Manejo de pago siguiente con menos dias de interes

                    If blnRealizaPago = True AndAlso intNumPosicSigPago < intPlazoPres Then

                        strEstadoPagoSig = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Pagado", intNumPosicSigPago).Trim()

                        If strEstadoPagoSig = "N" Then

                            If intDiasMora > 0 AndAlso intDiasMora < intDiasSigPago AndAlso (strTipoCuota = "2") Then

                                strCapSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Capital", intNumPosicSigPago).Trim()
                                If Not String.IsNullOrEmpty(strCapSigPago) Then decCapSigPago = Decimal.Parse(strCapSigPago, n)

                                strSalIniSigPago = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Sal_Ini", intNumPosicSigPago).Trim()
                                If Not String.IsNullOrEmpty(strSalIniSigPago) Then decSalIniSigPago = Decimal.Parse(strSalIniSigPago, n)

                                intDiasSigPago = intDiasSigPago - intDiasMora

                                decInteresSigPago = ((decSalIniSigPago * decTasaIntNormal) / 360) * intDiasSigPago

                                decCuotaSigPago = decCapSigPago + decInteresSigPago

                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumPosicSigPago, decCuotaSigPago.ToString(n))
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumPosicSigPago, decInteresSigPago.ToString(n))
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumPosicSigPago, intDiasSigPago.ToString())

                            End If

                        End If

                    End If

                End If

            End If

            'EditTextMontoAbo.AsignaValorUserDataSource(decCuota.ToString(n))
            'EditTextAboCap.AsignaValorUserDataSource(decCapital.ToString(n))
            'EditTextAboInt.AsignaValorUserDataSource(decIntereses.ToString(n))
            'EditTextDiasInt.AsignaValorUserDataSource(intDiasInt.ToString())
            'EditTextSalFin.AsignaValorUserDataSource(decSaldoFinal.ToString(n))
            If intDiasMoraTotal > 0 Then
                EditTextAboMor.AsignaValorUserDataSource(decMontoMora.ToString(n))
                EditTextDiasMora.AsignaValorUserDataSource(intDiasMoraTotal.ToString())
                EditTextRecargoCobranza.AsignaValorUserDataSource(dbCargoCobranza)
            Else
                EditTextAboMor.AsignaValorUserDataSource(0)
                EditTextDiasMora.AsignaValorUserDataSource(0)
                EditTextRecargoCobranza.AsignaValorUserDataSource(0)
            End If


            If blnRealizaPago = True Then
                If strTipoCuota <> "1" Then
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Cuota", intNumero - 1, decCuota.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Capital", intNumero - 1, decCapital.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Interes", intNumero - 1, decIntereses.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Int", intNumero - 1, intDiasInt.ToString())
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Int_Mora", intNumero - 1, decMontoMora.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_ReCo", intNumero - 1, EditTextRecargoCobranza.ObtieneValorUserDataSource())
                    If Not strCancelarMora = "Y" Then
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Mor", intNumero - 1, intDiasMoraTotal.ToString())
                    Else
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Dias_Mor", intNumero - 1, "0")
                    End If

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Fecha", intNumero - 1, dtFechaPago.ToString("yyyyMMdd"))
                End If
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").SetValue("U_Sal_Fin", intNumero - 1, decSaldoFinal.ToString(n))
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Se crea el Borrador del Pago Recibido en SBO, agregando numero de préstamo y pago asociado al borrador

    Private Sub GenerarBorradorPagoRecibido(ByVal strCliente As String, ByVal dtFechaPago As Date, ByVal strCuentaDebito As String, ByVal decCuota As Decimal, ByVal decCapital As Decimal, ByVal strMoneda As String, _
                                    ByVal strCuentaFinancia As String, ByVal strComentario As String, ByVal strGeneroAsiento As String, ByRef blnPagoGenerado As Boolean, _
                                    ByVal strPrestamo As String, ByVal strNumero As String, ByRef strPagoRecibido As String, ByVal decAboTotalInt As Decimal, ByVal decAboTotalMora As Decimal, ByVal decAboCobranza As Decimal, ByVal p_intNumPago As Integer)

        Dim oPagoRecibido As SAPbobsCOM.Payments

        Dim intError As Integer
        Dim strError As String = ""

        Dim strMonedaLocal As String
        Dim strTipoCambio As String
        Dim decTipoCambio As Decimal
        Dim decMontoPago As Decimal
        Dim dtFechaInicio As Date

        Try

            If strGeneroAsiento = "Y" Then
                decMontoPago = decCapital
            Else
                decMontoPago = decCuota
            End If

            oPagoRecibido = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)

            oPagoRecibido.CardCode = strCliente
            oPagoRecibido.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
            oPagoRecibido.DocType = BoRcptTypes.rCustomer
            oPagoRecibido.DocDate = dtFechaPago
            oPagoRecibido.CashAccount = strCuentaDebito
            strMonedaLocal = General.RetornarMonedaLocal(_companySbo)
            If Not strMoneda = strMonedaLocal Then
                strTipoCambio = General.EjecutarConsulta("Select Rate From [ORTT] Where Currency = '" & strMoneda & "' And RateDate = '" & dtFechaPago.ToString("yyyyMMdd") & "'", StrConexion)
                If Not String.IsNullOrEmpty(strTipoCambio) Then
                    decTipoCambio = Decimal.Parse(strTipoCambio)
                    oPagoRecibido.DocRate = decTipoCambio
                End If
            End If

            oPagoRecibido.DocCurrency = strMoneda
            If strGeneroAsiento = "Y" Then
                oPagoRecibido.ControlAccount = strCuentaFinancia
            End If
            oPagoRecibido.Remarks = strComentario
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_Prestamo").Value = strPrestamo
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPago").Value = strNumero
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_MIn").Value = CDbl(decAboTotalInt)
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_MInMo").Value = CDbl(decAboTotalMora)
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_MRC").Value = CDbl(decAboCobranza)
            oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPagoC").Value = p_intNumPago

            If CheckBoxCheque.ObtieneValorDataSource() = "N" Or String.IsNullOrEmpty(CheckBoxCheque.ObtieneValorDataSource()) Then
                oPagoRecibido.CashSum = decMontoPago
            ElseIf CheckBoxCheque.ObtieneValorDataSource() = "Y" Then

                Dim oMatrix As SAPbouiCOM.Matrix

                oMatrix = DirectCast(FormularioSBO.Items.Item("mtxChPF").Specific, SAPbouiCOM.Matrix)

                Dim intRegistoEliminar = oMatrix.GetNextSelectedRow()

                intRegistoEliminar = intRegistoEliminar

                oPagoRecibido.Checks.AccounttNum = g_strCuenta.Trim()
                oPagoRecibido.Checks.BankCode = g_strNBanco.Trim()
                oPagoRecibido.Checks.Branch = g_strSucursal.Trim()
                oPagoRecibido.Checks.CheckNumber = g_strNoCheque.Trim()
                oPagoRecibido.Checks.CheckSum = Decimal.Parse(g_strMontoAbonar, n)
                'oPagoRecibido.Checks.Details = 
                If Not String.IsNullOrEmpty(g_strFechaPago) Then
                    dtFechaInicio = Date.ParseExact(g_strFechaPago, "yyyyMMdd", Nothing)
                    dtFechaInicio = New Date(dtFechaInicio.Year, dtFechaInicio.Month, dtFechaInicio.Day, 0, 0, 0)
                    oPagoRecibido.Checks.DueDate = dtFechaInicio
                End If
                If g_strEndoso.Trim() = "Y" Then
                    oPagoRecibido.Checks.Trnsfrable = 1
                ElseIf g_strEndoso.Trim = "N" Then
                    oPagoRecibido.Checks.Trnsfrable = 0
                End If
            End If

            intError = oPagoRecibido.Add()
            If intError <> 0 Then

                _companySbo.GetLastError(intError, strError)
                If _companySbo.InTransaction() Then
                    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                __applicationSbo.SetStatusBarMessage(String.Format("{0}:{1}", intError, strError))
                Throw New Exception(String.Format("{0}:{1}", intError, strError))

            Else

                _companySbo.GetNewObjectCode(strPagoRecibido)

                blnPagoGenerado = True

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Se elimina el Borrador del Pago Recibido en caso de que se reverse un pago con borrador asociado

    Private Sub GenerarReversionBorrador(ByVal intBorrador As Integer)

        Dim oPagoBorrador As SAPbobsCOM.Payments

        Dim intError As Integer
        Dim strError As String = ""

        Try

            oPagoBorrador = General.CargarPagoRecibido(intBorrador, CompanySBO, SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)

            If Not oPagoBorrador Is Nothing Then

                If oPagoBorrador.Remove <> 0 Then
                    _companySbo.GetLastError(intError, strError)
                    If _companySbo.InTransaction() Then
                        _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    Throw New Exception(String.Format("{0}: {1}", intError, strError))
                End If

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Se genera la reversión de un Pago Recibido ya efectuado en SBO, esto mediante un asiento de reversión del asiento generado por el Pago Recibido

    Private Sub GenerarReversionPago(ByVal intPagoRecibido As Integer)
        Dim oPagoRecibido As Payments
        Dim intError As Integer
        Dim strError As String
        Try
            oPagoRecibido = _companySbo.GetBusinessObject(BoObjectTypes.oIncomingPayments)
            If oPagoRecibido.GetByKey(intPagoRecibido) Then
                If Not oPagoRecibido.Cancel() = 0 Then
                    Throw New Exception()
                End If
            Else
                Throw New Exception()
            End If

        Catch ex As Exception
            _companySbo.GetLastError(intError, strError)
            Throw New Exception(String.Format("{0}: {1}", intError, strError))
        End Try

    End Sub

    'Se genera la reversión de un asiento contable, poniendo el crédito en el débito, y el débito en el crédito, esto del asiento que se esté reversando

    Private Sub GenerarReversionAsiento(ByVal intAsiento As Integer, ByVal strPrestamo As String, ByVal strCom As String, Optional ByVal intPago As Integer = 0)

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim objAsiento As SAPbobsCOM.JournalEntries
        Dim objAsientoLines As SAPbobsCOM.JournalEntries_Lines
        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim strComentario As String
        Dim intDocumento As Integer
        Try

            objAsiento = CargarAsiento(intAsiento)

            objAsientoLines = objAsiento.Lines

            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            If Not objAsiento Is Nothing Then

                If intPago > 0 Then
                    intDocumento = intPago
                Else
                    intDocumento = intAsiento
                End If

                oJournalEntry.ReferenceDate = Now.Date
                strComentario = String.Format(strCom, intDocumento, strPrestamo)
                oJournalEntry.Memo = strComentario

                For i As Integer = 0 To objAsientoLines.Count - 1

                    objAsientoLines.SetCurrentLine(i)

                    With objAsientoLines

                        oJournalEntry.Lines.ShortName = .ShortName
                        oJournalEntry.Lines.AccountCode = .AccountCode
                        oJournalEntry.Lines.Debit = .Credit
                        oJournalEntry.Lines.FCDebit = .FCCredit
                        oJournalEntry.Lines.Credit = .Debit
                        oJournalEntry.Lines.FCCredit = .FCDebit
                        If Not String.IsNullOrEmpty(.FCCurrency) Then
                            oJournalEntry.Lines.FCCurrency = .FCCurrency
                        End If
                        oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                        oJournalEntry.Lines.Add()

                    End With

                Next

                If oJournalEntry.Add <> 0 Then
                    _companySbo.GetLastError(intError, strMensajeError)
                    If _companySbo.InTransaction() Then
                        _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    Throw New Exception(String.Format("{0}: {1}", intError, strMensajeError))
                End If

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub GenerarReversionFactura(ByVal intFactura As Integer, ByVal strPrestamo As String, ByVal strCom As String, Optional ByVal intPago As Integer = 0)

        Dim oFactura As SAPbobsCOM.Documents
        Dim oNotaCredito As SAPbobsCOM.Documents


        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim decTotalIntereses As Decimal
        Dim strMonedaLocal As String
        Dim strMoneda As String
        Dim strCodImpuestos As String = General.EjecutarConsulta(" Select U_CodImp from [@SCGD_CONF_FINANC] ", StrConexion)
        strCodImpuestos = strCodImpuestos.Trim()

        Dim strSeries As String = General.EjecutarConsulta(" Select U_NumNC From [@SCGD_CONF_FINANC] ", StrConexion).Trim

        Try

            strMonedaLocal = General.RetornarMonedaLocal(_companySbo)

            oFactura = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            oNotaCredito = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)

            oFactura.GetByKey(intFactura)

            'oFactura.DocDate = dtFechaPago
            oNotaCredito.Comments = oFactura.Comments
            oNotaCredito.Series = strSeries
            oNotaCredito.CardCode = oFactura.CardCode
            oNotaCredito.DocType = oFactura.DocType
            oNotaCredito.DocCurrency = oFactura.DocCurrency
            strMoneda = oFactura.DocCurrency

            For index As Integer = 0 To oFactura.Lines.Count - 1
                oFactura.Lines.SetCurrentLine(index)
                oNotaCredito.Lines.AccountCode = oFactura.Lines.AccountCode
                If strMoneda = strMonedaLocal Then
                    oNotaCredito.Lines.LineTotal = oFactura.Lines.LineTotal
                    oNotaCredito.Lines.Currency = strMoneda
                    oNotaCredito.Lines.TaxCode = oFactura.Lines.TaxCode
                    oNotaCredito.Lines.VatGroup = oFactura.Lines.VatGroup
                    oNotaCredito.Lines.BaseType = "13"
                    oNotaCredito.Lines.BaseEntry = intFactura
                    oNotaCredito.Lines.BaseLine = oFactura.Lines.LineNum
                Else
                    oNotaCredito.Lines.RowTotalFC = oFactura.Lines.RowTotalFC
                    oNotaCredito.Lines.Currency = strMoneda
                    oNotaCredito.Lines.TaxCode = oFactura.Lines.TaxCode
                    oNotaCredito.Lines.VatGroup = oFactura.Lines.VatGroup
                    oNotaCredito.Lines.BaseType = "13"
                    oNotaCredito.Lines.BaseEntry = intFactura
                    oNotaCredito.Lines.BaseLine = oFactura.Lines.LineNum
                End If
                oNotaCredito.Lines.ItemDescription = oFactura.Lines.ItemDescription
                oNotaCredito.Lines.Add()
            Next


            If oNotaCredito.Add <> 0 Then
                _companySbo.GetLastError(intError, strMensajeError)
                If _companySbo.InTransaction() Then
                    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                Throw New Exception(String.Format("{0}: {1}", intError, strMensajeError))

            End If

        Catch ex As Exception

            Throw ex

        End Try
    End Sub

    'Carga y retorna el objeto asiento con base en el id de dicho asiento

    Private Function CargarAsiento(ByVal p_NumAsiento As Integer) As SAPbobsCOM.JournalEntries

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Try
            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            If oJournalEntry.GetByKey(p_NumAsiento) Then

                Return oJournalEntry

            End If

        Catch ex As Exception

            Throw ex

        End Try
        Return Nothing
    End Function

    'Retorna el plazo de un préstamo

    Private Function Plazo(ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim strPlazo As String
        Dim intPlazo As Integer

        Dim n As NumberFormatInfo

        Try

            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            strPlazo = oForm.DataSources.DBDataSources.Item("@SCGD_PRESTAMO").GetValue("U_Plazo", 0)
            strPlazo = strPlazo.Trim()

            If Not String.IsNullOrEmpty(strPlazo) Then

                intPlazo = Integer.Parse(strPlazo)

            Else

                intPlazo = 0

            End If

            Return intPlazo

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    'Se crea el asiento de préstamo una vez que se está facturando el contrato de ventas con financiamiento propio, esto si se genera asiento de monto a financiar
    'IMPORTANTE: Hace falta validar con expertor contables y de finanzas este asiento y las afectaciones contables del mismo

    Public Sub CrearAsientoPrestamo(ByVal dtFecha As Date, ByVal strMoneda As String, ByVal strContrato As String, ByVal decMontoFinanciar As Decimal, ByVal strPrestamo As String, _
                                    ByVal strMemo As String, ByVal strRef1 As String, ByVal strRef2 As String, _
                                    ByVal m_strMonedaLocal As String, ByVal strCuentaDebito As String, ByVal strCliente As String)

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strNoAsiento As String = ""

        Try

            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.ReferenceDate = dtFecha

            oJournalEntry.Memo = strMemo

            oJournalEntry.Lines.AccountCode = strCuentaDebito

            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Debit = decMontoFinanciar
            Else
                oJournalEntry.Lines.FCDebit = decMontoFinanciar
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If

            oJournalEntry.Lines.Reference1 = strRef1
            oJournalEntry.Lines.Reference2 = strRef2

            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            oJournalEntry.Lines.Add()

            oJournalEntry.Lines.ShortName = strCliente

            If strMoneda = m_strMonedaLocal Then
                oJournalEntry.Lines.Credit = decMontoFinanciar
            Else
                oJournalEntry.Lines.FCCredit = decMontoFinanciar
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If

            oJournalEntry.Lines.Reference1 = strRef1
            oJournalEntry.Lines.Reference2 = strRef2

            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            oJournalEntry.Lines.Add()

            If oJournalEntry.Add <> 0 Then
                strNoAsiento = "0"
                _companySbo.GetLastError(intError, strMensajeError)
                If _companySbo.InTransaction() Then
                    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                Throw New DI.SboUncessfullOperationException(intError, strMensajeError, "PrestamoFormulario.CrearAsientoPrestamo")
            Else
                _companySbo.GetNewObjectCode(strNoAsiento)

                oCompanyService = _companySbo.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", strPrestamo)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData.SetProperty("U_Asiento", strNoAsiento)
                oGeneralService.Update(oGeneralData)

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub CheckBoxSBOPagoDeudaItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)
        Try
            If pVal.Action_Success Then
                If CheckBoxPagoDeuda.ObtieneValorUserDataSource() = "Y" Then
                    ManejarEstadoPagoTotal(False)
                    CalcularAbonoTotal()
                Else
                    ManejarEstadoPagoTotal(True)
                    CalculoPagoNormal()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CalculoPagoNormal()
        Dim intNumero As Integer
        Dim dtFechaTeoricaPlan As Date
        intNumero = EditTextNumero.ObtieneValorUserDataSource()

        If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim()) Then
            dtFechaTeoricaPlan = Date.ParseExact(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Fecha", intNumero - 1).Trim(), "yyyyMMdd", Nothing)
            dtFechaTeoricaPlan = New Date(dtFechaTeoricaPlan.Year, dtFechaTeoricaPlan.Month, dtFechaTeoricaPlan.Day, 0, 0, 0)
        End If
        EditTextFechaPago.AsignaValorUserDataSource(dtFechaTeoricaPlan.ToString(("yyyyMMdd")))
        EditTextMontoAbo.AsignaValorUserDataSource(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLAN_REAL").GetValue("U_Cuota", intNumero - 1).Trim())
        ButtonCalcular.ItemSBO.Click(SAPbouiCOM.BoCellClickType.ct_Regular)

    End Sub
    Private Sub ManejarEstadoPagoTotal(ByVal p_Estado As Boolean)
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, p_Estado)
        CheckBoxDisminucion.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, p_Estado)
        CheckBoxCheque.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, p_Estado)
        EditTextFechaPago.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, p_Estado)
        EditTextMontoAbo.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, p_Estado)
    End Sub

    Private Sub CalcularAbonoTotal()

        Dim dtToday As Date = Date.Today
        Dim decMontoMora As Decimal = 0
        Dim decTotalAbonar As Decimal

        EditTextFechaPago.AsignaValorUserDataSource(dtToday.ToString("yyyyMMdd"))
        EditTextMontoAbo.AsignaValorUserDataSource(EditTextSalIni.ObtieneValorUserDataSource)
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        ButtonCalcular.ItemSBO.Click()
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

        decTotalAbonar += General.ConvierteDecimal(EditTextSalIni.ObtieneValorUserDataSource, n)
        decTotalAbonar += General.ConvierteDecimal(EditTextCapPend.ObtieneValorUserDataSource, n)
        decTotalAbonar += General.ConvierteDecimal(EditTextAboInt.ObtieneValorUserDataSource, n)
        decTotalAbonar += General.ConvierteDecimal(EditTextIntPend.ObtieneValorUserDataSource, n)
        decTotalAbonar += General.ConvierteDecimal(EditTextAboMor.ObtieneValorUserDataSource, n)
        decTotalAbonar += General.ConvierteDecimal(EditTextMoraPend.ObtieneValorUserDataSource, n)

        EditTextMontoAbo.AsignaValorUserDataSource(decTotalAbonar.ToString(n))
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        ButtonCalcular.ItemSBO.Click()
        ButtonCalcular.ItemSBO.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

    End Sub

    ''' <summary>
    ''' Reversa los pagos de los prestamos con tipo de cuotas niveladas
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Private Sub ReversarPagosCuotaNivelada(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim intRowSelected As Integer
        Dim oPagoRecibido As SAPbobsCOM.Payments
        Dim dbMontoInteres As Double
        Dim dbMontoInteresMora As Double
        Dim dbMontoRecargoCobranza As Double
        Dim dbMontoCap As Double
        Dim blnCuotaPendiente As Boolean
        Dim intNumCuota As Integer
        Dim intPrestamo As Integer
        Dim intDocEntry As Integer

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralDataP As SAPbobsCOM.GeneralData
        Dim oGeneralDataPR As SAPbobsCOM.GeneralData
        Dim oGeneralDataCollectionP As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralDataCollectionPR As SAPbobsCOM.GeneralDataCollection

        Try
            oForm = _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS")
            oMatrix = DirectCast(oForm.Items.Item("mtxPagos").Specific, SAPbouiCOM.Matrix)
            intRowSelected = oMatrix.GetNextSelectedRow()
            
            If pVal.BeforeAction Then
                If intRowSelected <> -1 AndAlso oForm.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").GetValue("U_Reversado", intRowSelected - 1).Trim = "Y" Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.PagoReversado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    Exit Sub
                ElseIf intRowSelected = oForm.DataSources.DBDataSources.Item("@SCGD_PAGO_PRESTAMO").Size Then
                    BubbleEvent = False
                    Exit Sub
                End If
            Else

                intPrestamo = oForm.Items.Item("txtNumPres").Specific.Value
                oCompanyService = CompanySBO.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", intPrestamo)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralDataCollectionP = oGeneralData.Child("SCGD_PAGO_PRESTAMO")
                oGeneralDataCollectionPR = oGeneralData.Child("SCGD_PLAN_REAL")

                With oGeneralDataCollectionP

                    For index As Integer = .Count - 1 To intRowSelected - 1 Step -1
                        oGeneralDataP = .Item(index)
                        With oGeneralDataP
                            If Not String.IsNullOrEmpty(.GetProperty("U_Pago")) AndAlso CInt(.GetProperty("U_Pago")) <> 0 Then
                                oPagoRecibido = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
                                If Not String.IsNullOrEmpty(.GetProperty("U_Pago")) AndAlso .GetProperty("U_Pago") <> 0 Then intDocEntry = .GetProperty("U_Pago")
                            Else
                                oPagoRecibido = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)
                                If Not String.IsNullOrEmpty(.GetProperty("U_BorrPag")) AndAlso .GetProperty("U_BorrPag") <> 0 Then intDocEntry = .GetProperty("U_BorrPag")
                            End If
                            
                            blnCuotaPendiente = False

                            If index >= intRowSelected - 1 AndAlso .GetProperty("U_Reversado").Trim = "N" AndAlso oPagoRecibido.GetByKey(intDocEntry) Then
                                With oPagoRecibido
                                    intNumCuota = .UserFields.Fields.Item("U_SCGD_NumPago").Value
                                    dbMontoInteres = .UserFields.Fields.Item("U_SCGD_MIn").Value
                                    dbMontoInteresMora = .UserFields.Fields.Item("U_SCGD_MInMo").Value
                                    dbMontoRecargoCobranza = .UserFields.Fields.Item("U_SCGD_MRC").Value
                                    dbMontoCap = .CashSum + .TransferSum
                                    For index2 As Integer = 0 To .Checks.Count - 1
                                        With .Checks
                                            .SetCurrentLine(index2)
                                            dbMontoCap += .CheckSum
                                        End With
                                    Next
                                    For index2 As Integer = 0 To .CreditCards.Count - 1
                                        With .CreditCards
                                            .SetCurrentLine(index2)
                                            dbMontoCap += .CreditSum
                                        End With
                                    Next
                                    dbMontoCap -= (dbMontoInteres + dbMontoInteresMora + dbMontoRecargoCobranza)
                                End With

                                With oGeneralDataCollectionPR
                                    For index2 As Integer = .Count - 1 To intNumCuota - 1 Step -1
                                        oGeneralDataPR = .Item(index2)
                                        With oGeneralDataPR
                                            If .GetProperty("U_Pagado").Trim <> "N" Then

                                                If CDbl(.GetProperty("U_ToCapPagado")) > dbMontoCap AndAlso (CDbl(.GetProperty("U_ToCapPagado")) - dbMontoCap) > 0.1 Then
                                                    .SetProperty("U_ToCapPagado", CDbl(.GetProperty("U_ToCapPagado") - dbMontoCap))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota") - dbMontoCap))
                                                    .SetProperty("U_Capital", CDbl(.GetProperty("U_Capital") + dbMontoCap))
                                                    dbMontoCap = 0
                                                    blnCuotaPendiente = True
                                                Else
                                                    dbMontoCap -= CDbl(.GetProperty("U_ToCapPagado"))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - CDbl(.GetProperty("U_ToCapPagado")))
                                                    .SetProperty("U_Capital", CDbl(.GetProperty("U_Capital")) + CDbl(.GetProperty("U_ToCapPagado")))
                                                    .SetProperty("U_ToCapPagado", 0)
                                                End If

                                                If CDbl(.GetProperty("U_ToIntPagado")) > dbMontoInteres AndAlso (CDbl(.GetProperty("U_ToIntPagado")) - dbMontoInteres) > 0.1 Then
                                                    .SetProperty("U_ToIntPagado", CDbl(.GetProperty("U_ToIntPagado") - dbMontoInteres))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - dbMontoInteres)
                                                    .SetProperty("U_Interes", CDbl(.GetProperty("U_Interes")) + dbMontoInteres)
                                                    dbMontoInteres = 0
                                                    blnCuotaPendiente = True
                                                Else
                                                    dbMontoInteres -= CDbl(.GetProperty("U_ToIntPagado"))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - CDbl(.GetProperty("U_ToIntPagado")))
                                                    .SetProperty("U_Interes", CDbl(.GetProperty("U_Interes")) + CDbl(.GetProperty("U_ToIntPagado")))
                                                    .SetProperty("U_ToIntPagado", 0)
                                                End If
                                                
                                                If CDbl(.GetProperty("U_ToMoPagado")) > dbMontoInteresMora AndAlso (CDbl(.GetProperty("U_ToMoPagado")) - dbMontoInteresMora) > 0.1 Then
                                                    .SetProperty("U_ToMoPagado", CDbl(.GetProperty("U_ToMoPagado") - dbMontoInteresMora))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - dbMontoInteresMora)
                                                    If CDbl(.GetProperty("U_Capital")) <= 0 Then
                                                        .SetProperty("U_Int_Mora", CDbl(.GetProperty("U_Int_Mora")) + dbMontoInteresMora)
                                                    Else
                                                        .SetProperty("U_Int_Mora", 0)
                                                    End If
                                                    dbMontoInteresMora = 0
                                                    blnCuotaPendiente = True
                                                Else
                                                    dbMontoInteresMora -= CDbl(.GetProperty("U_ToMoPagado"))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - CDbl(.GetProperty("U_ToMoPagado")))
                                                    If CDbl(.GetProperty("U_Capital")) <= 0 Then
                                                        .SetProperty("U_Int_Mora", CDbl(.GetProperty("U_Int_Mora")) + CDbl(.GetProperty("U_ToMoPagado")))
                                                    Else
                                                        .SetProperty("U_Int_Mora", 0)
                                                    End If
                                                    .SetProperty("U_ToMoPagado", 0)
                                                    .SetProperty("U_Dias_Mor", 0)
                                                End If
                                                
                                                If CDbl(.GetProperty("U_ReCo")) > dbMontoRecargoCobranza AndAlso (CDbl(.GetProperty("U_ReCo")) - dbMontoRecargoCobranza) > 0.1 Then
                                                    .SetProperty("U_ReCo", .GetProperty("U_ReCo") - dbMontoRecargoCobranza)
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - dbMontoRecargoCobranza)
                                                    dbMontoRecargoCobranza = 0
                                                    blnCuotaPendiente = True
                                                Else
                                                    dbMontoRecargoCobranza -= CDbl(.GetProperty("U_ReCo"))
                                                    .SetProperty("U_ToCuota", CDbl(.GetProperty("U_ToCuota")) - CDbl(.GetProperty("U_ReCo")))
                                                    .SetProperty("U_ReCo", 0)
                                                End If

                                                .SetProperty("U_Sal_Fin", CDbl(.GetProperty("U_Sal_Ini")) - CDbl(.GetProperty("U_ToCapPagado")))
                                                
                                                If blnCuotaPendiente Then
                                                    .SetProperty("U_Pagado", "P")
                                                    blnCuotaPendiente = False
                                                    Exit For
                                                Else
                                                    .SetProperty("U_Pagado", "N")
                                                    .SetProperty("U_ToCuota", 0)
                                                    .SetProperty("U_Int_Mora", 0)
                                                    If dbMontoCap <= 0 AndAlso dbMontoInteres <= 0 AndAlso dbMontoInteresMora <= 0 AndAlso dbMontoRecargoCobranza <= 0 Then
                                                        Exit For
                                                    Else
                                                        Continue For
                                                    End If
                                                End If
                                            End If
                                        End With
                                    Next
                                End With
                            End If
                        End With
                        
                    Next
                    CompanySBO.StartTransaction()
                    For index As Integer = intRowSelected - 1 To .Count - 2
                        oGeneralDataP = .Item(index)
                        With oGeneralDataP
                            If .GetProperty("U_Reversado") = "N" Then
                                If Not String.IsNullOrEmpty(.GetProperty("U_Pago")) AndAlso CInt(.GetProperty("U_Pago")) <> 0 Then
                                    Call GenerarReversionPago(.GetProperty("U_Pago"))
                                    .SetProperty("U_Pago", 0)
                                End If
                                If Not String.IsNullOrEmpty(.GetProperty("U_NumAsie")) AndAlso CInt(.GetProperty("U_NumAsie")) <> 0 Then
                                    Call GenerarReversionAsiento(.GetProperty("U_NumAsie"), intPrestamo, My.Resources.Resource.ComentarioDocumentoReversaIntereses)
                                    .SetProperty("U_NumAsie", 0)
                                End If
                                If Not String.IsNullOrEmpty(.GetProperty("U_DocFac")) AndAlso CInt(.GetProperty("U_DocFac")) <> 0 Then
                                    Call GenerarReversionFactura(.GetProperty("U_DocFac"), intPrestamo, My.Resources.Resource.ComentarioDocumentoReversaIntereses)
                                    .SetProperty("U_DocFac", 0)
                                End If
                                If Not String.IsNullOrEmpty(.GetProperty("U_BorrPag")) AndAlso CInt(.GetProperty("U_BorrPag")) <> 0 Then
                                    Call GenerarReversionBorrador(.GetProperty("U_BorrPag"))
                                    .SetProperty("U_BorrPag", 0)
                                End If
                                .SetProperty("U_Reversado", "Y")
                            End If
                        End With
                    Next
                    oGeneralService.Update(oGeneralData)
                    If CompanySBO.InTransaction Then
                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                        RecargarPagos(FormUID, pVal, BubbleEvent)
                        oForm.Items.Item("txtNumPres").Enabled = False
                    End If
                End With
            End If

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            _applicationSbo.SetStatusBarMessage(ex.Message)
        Finally
            General.DestruirObjeto(oPagoRecibido)
        End Try

    End Sub

    Private Sub GeneraDocumentoIntereses(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim intRowSelected As Integer
        Dim strGeneraFactura As String
        Dim intPrestamo As Integer
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralDataP As SAPbobsCOM.GeneralData
        Dim oGeneralDataCollectionP As SAPbobsCOM.GeneralDataCollection

        Dim oPagoRecibido As SAPbobsCOM.Payments

        Dim dtFechaPago As Date
        Dim strTipoCuo As String
        Dim strComentario As String
        Dim strNumeroPago As String
        Dim strCliente As String
        Dim strMoneda As String
        Dim strRef1 As String
        Dim strRef2 As String
        Dim strNumeroPagoC As String
        Dim strCuentaCredInt As String
        Dim strCuentaValidaInt As String
        Dim decAboTotalInt As Decimal
        Dim decAboTotalMora As Decimal
        Dim dbRecargoCobranza As Double
        Dim strMonedaLocal As String
        Dim strMonedaSistema As String
        Dim strCuentaCredMora As String
        Dim strMonedaCredInt As String
        Dim strCuentaValidaMora As String
        Dim strMonedaCredMora As String
        Dim strGeneraAsiento As String
        Dim blnAsientoIntGenerado As Boolean = False
        Dim strAsientoIntereses As String = ""

        Try
            oForm = _applicationSbo.Forms.Item("SCGD_PAGOS_PRESTAMOS")
            oMatrix = DirectCast(oForm.Items.Item("mtxPagos").Specific, SAPbouiCOM.Matrix)
            intRowSelected = oMatrix.GetNextSelectedRow()

            intPrestamo = oForm.Items.Item("txtNumPres").Specific.Value
            oCompanyService = CompanySBO.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_Prestamo")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", intPrestamo)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oGeneralDataCollectionP = oGeneralData.Child("SCGD_PAGO_PRESTAMO")

            With oGeneralDataCollectionP
                oGeneralDataP = .Item(intRowSelected - 1)
                With oGeneralDataP
                    If Not String.IsNullOrEmpty(.GetProperty("U_Pago")) AndAlso CInt(.GetProperty("U_Pago")) <> 0 Then
                        oPagoRecibido = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
                        strGeneraFactura = General.EjecutarConsulta(" Select U_GenFD from [@SCGD_CONF_FINANC] ", StrConexion)
                        strGeneraFactura = strGeneraFactura.Trim

                        If strGeneraFactura = "Y" Then
                            If String.IsNullOrEmpty(.GetProperty("U_DocFac")) OrElse CInt(.GetProperty("U_DocFac")) = 0 Then
                                If oPagoRecibido.GetByKey(CInt(.GetProperty("U_Pago"))) Then
                                    strMonedaLocal = General.RetornarMonedaLocal(_companySbo)
                                    strMonedaSistema = General.RetornarMonedaSistema(_companySbo)

                                    dtFechaPago = oPagoRecibido.DocDate
                                    strTipoCuo = oGeneralData.GetProperty("U_Tipo_Cuo").ToString.Trim
                                    strNumeroPago = oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPago").Value.Trim()
                                    strNumeroPagoC = oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPagoC").Value
                                    If strTipoCuo <> "1" Then
                                        strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.DelPrestamo & intPrestamo
                                    Else
                                        strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.NumPagoPrestamo & strNumeroPagoC & My.Resources.Resource.DelPrestamo & intPrestamo
                                    End If
                                    strCliente = oPagoRecibido.CardCode
                                    strMoneda = oPagoRecibido.DocCurrency
                                    strRef1 = My.Resources.Resource.Prestamo & intPrestamo
                                    strRef2 = My.Resources.Resource.Pago & strNumeroPago
                                    If strMoneda = strMonedaLocal Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    ElseIf strMoneda = strMonedaSistema Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    End If

                                    strCuentaValidaInt = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredInt & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredInt = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaInt & "'", StrConexion)

                                    strCuentaValidaMora = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredMora & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredMora = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaMora & "'", StrConexion)

                                    decAboTotalInt = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MIn").Value
                                    decAboTotalMora = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MInMo").Value
                                    dbRecargoCobranza = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MRC").Value

                                    strGeneraAsiento = General.EjecutarConsulta("Select U_Gen_As From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                    _companySbo.StartTransaction()
                                    Call GenerarFacturaIntereses(dtFechaPago, strComentario, "", strCliente, strMoneda, strRef1, strRef2, strCuentaValidaInt, decAboTotalInt, strCuentaValidaMora, decAboTotalMora, strGeneraAsiento, blnAsientoIntGenerado, strAsientoIntereses, dbRecargoCobranza, BubbleEvent)
                                    If blnAsientoIntGenerado Then
                                        .SetProperty("U_DocFac", CInt(strAsientoIntereses))
                                        oGeneralService.Update(oGeneralData)
                                        If _companySbo.InTransaction Then
                                            _companySbo.EndTransaction(BoWfTransOpt.wf_Commit)
                                        End If
                                    Else
                                        If _companySbo.InTransaction Then
                                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                                        End If
                                    End If

                                End If

                            Else
                                _applicationSbo.SetStatusBarMessage("Documento ya generado")
                            End If

                        Else
                            If String.IsNullOrEmpty(.GetProperty("U_NumAsie")) OrElse CInt(.GetProperty("U_NumAsie")) = 0 Then
                                If oPagoRecibido.GetByKey(CInt(.GetProperty("U_Pago"))) Then
                                    strMonedaLocal = General.RetornarMonedaLocal(_companySbo)
                                    strMonedaSistema = General.RetornarMonedaSistema(_companySbo)

                                    dtFechaPago = oPagoRecibido.DocDate
                                    strTipoCuo = oGeneralData.GetProperty("U_Tipo_Cuo").ToString.Trim
                                    strNumeroPago = oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPago").Value.Trim()
                                    strNumeroPagoC = oPagoRecibido.UserFields.Fields.Item("U_SCGD_NumPagoC").Value.Trim()
                                    If strTipoCuo <> "1" Then
                                        strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.DelPrestamo & intPrestamo
                                    Else
                                        strComentario = My.Resources.Resource.DocumentoGenerado & strNumeroPago & My.Resources.Resource.NumPagoPrestamo & strNumeroPagoC & My.Resources.Resource.DelPrestamo & intPrestamo
                                    End If
                                    strCliente = oPagoRecibido.CardCode
                                    strMoneda = oPagoRecibido.DocCurrency
                                    strRef1 = My.Resources.Resource.Prestamo & intPrestamo
                                    strRef2 = My.Resources.Resource.Pago & strNumeroPago
                                    If strMoneda = strMonedaLocal Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Loc From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    ElseIf strMoneda = strMonedaSistema Then

                                        strCuentaCredInt = General.EjecutarConsulta("Select U_Int_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                        strCuentaCredMora = General.EjecutarConsulta("Select U_Mor_Sis From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)

                                    End If

                                    strCuentaValidaInt = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredInt & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredInt = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaInt & "'", StrConexion)

                                    strCuentaValidaMora = General.EjecutarConsulta("Select AcctCode from dbo.[OACT] where FormatCode = '" & strCuentaCredMora & "' And Postable = 'Y'", StrConexion)
                                    strMonedaCredMora = General.EjecutarConsulta("Select ActCurr from dbo.[OACT] where AcctCode = '" & strCuentaValidaMora & "'", StrConexion)

                                    decAboTotalInt = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MIn").Value
                                    decAboTotalMora = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MInMo").Value
                                    dbRecargoCobranza = oPagoRecibido.UserFields.Fields.Item("U_SCGD_MRC").Value

                                    strGeneraAsiento = General.EjecutarConsulta("Select U_Gen_As From [@SCGD_CONF_FINANC] Where Code='1'", StrConexion)
                                    _companySbo.StartTransaction()
                                    Call GenerarAsientoIntereses(dtFechaPago, strComentario, "", strCliente, strMoneda, strRef1, strRef2, strCuentaValidaInt, decAboTotalInt, strCuentaValidaMora, decAboTotalMora, strGeneraAsiento, blnAsientoIntGenerado, strAsientoIntereses, dbRecargoCobranza)
                                    If blnAsientoIntGenerado Then
                                        .SetProperty("U_NumAsie", CInt(strAsientoIntereses))
                                        oGeneralService.Update(oGeneralData)
                                        If _companySbo.InTransaction Then
                                            _companySbo.EndTransaction(BoWfTransOpt.wf_Commit)
                                        End If
                                    Else
                                        If _companySbo.InTransaction Then
                                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                                        End If
                                    End If

                                End If
                            Else
                                _applicationSbo.SetStatusBarMessage("Documento ya generado")
                            End If

                        End If

                    Else
                        _applicationSbo.SetStatusBarMessage("Pago no creado")
                    End If
                End With
            End With


        Catch ex As Exception
            If _companySbo.InTransaction Then
                _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            _applicationSbo.SetStatusBarMessage(ex.Message)
        End Try

    End Sub

    Private Sub GenerarAsientoIntereses(ByVal dtFechaPago As Date, ByVal strComentario As String, ByVal strCuentaDebitoCuota As String, ByVal strCliente As String, ByVal strMoneda As String, _
                                      ByVal strRef1 As String, ByVal strRef2 As String, ByVal strCuentaIntNormal As String, ByVal decIntNormal As Decimal, ByVal strCuentaIntMora As String, _
                                      ByVal decIntMora As Decimal, ByVal strGeneraAsiento As String, ByRef blnAsientoIntGenerado As Boolean, ByRef strAsientoInteres As String, _
                                      ByVal dbRecargoCobranza As Double)

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim decTotalIntereses As Decimal
        Dim strMonedaLocal As String

        Try

            strMonedaLocal = General.RetornarMonedaLocal(_companySbo)

            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            oJournalEntry.ReferenceDate = dtFechaPago
            oJournalEntry.Memo = strComentario

            If strGeneraAsiento = "Y" Then
                oJournalEntry.Lines.AccountCode = strCuentaDebitoCuota
            Else
                oJournalEntry.Lines.ShortName = strCliente
            End If
            decTotalIntereses = decIntNormal + decIntMora + dbRecargoCobranza
            If strMoneda = strMonedaLocal Then
                oJournalEntry.Lines.Debit = decTotalIntereses
            Else
                oJournalEntry.Lines.FCDebit = decTotalIntereses
                oJournalEntry.Lines.FCCurrency = strMoneda
            End If
            oJournalEntry.Lines.Reference1 = strRef1
            oJournalEntry.Lines.Reference2 = strRef2
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
            oJournalEntry.Lines.Add()

            If decIntNormal > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntNormal
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = decIntNormal
                Else
                    oJournalEntry.Lines.FCCredit = decIntNormal
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If decIntMora > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = decIntMora
                Else
                    oJournalEntry.Lines.FCCredit = decIntMora
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If dbRecargoCobranza > 0 Then
                oJournalEntry.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oJournalEntry.Lines.Credit = dbRecargoCobranza
                Else
                    oJournalEntry.Lines.FCCredit = dbRecargoCobranza
                    oJournalEntry.Lines.FCCurrency = strMoneda
                End If
                oJournalEntry.Lines.Reference1 = strRef1
                oJournalEntry.Lines.Reference2 = strRef2
                oJournalEntry.Lines.LineMemo = My.Resources.Resource.RecargoCobranza
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.Add()
            End If

            If oJournalEntry.Add <> 0 Then
                _companySbo.GetLastError(intError, strMensajeError)
                If _companySbo.InTransaction() Then
                    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            Else
                _companySbo.GetNewObjectCode(strAsientoInteres)
                blnAsientoIntGenerado = True
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Private Sub GenerarFacturaIntereses(ByVal dtFechaPago As Date, ByVal strComentario As String, ByVal strCuentaDebitoCuota As String, ByVal strCliente As String, ByVal strMoneda As String, _
                                        ByVal strRef1 As String, ByVal strRef2 As String, ByVal strCuentaIntNormal As String, ByVal decIntNormal As Decimal, ByVal strCuentaIntMora As String, _
                                        ByVal decIntMora As Decimal, ByVal strGeneraAsiento As String, ByRef blnAsientoIntGenerado As Boolean, ByRef strAsientoInteres As String, _
                                        ByVal dbRecargoCobranza As Double, ByRef BubbleEvent As Boolean)

        Dim oFactura As SAPbobsCOM.Documents

        Dim intError As Integer
        Dim strMensajeError As String = ""

        Dim decTotalIntereses As Decimal
        Dim strMonedaLocal As String

        Dim strNumDoc As String = General.EjecutarConsulta(" Select U_NumDoc from [@SCGD_CONF_FINANC] ", StrConexion)
        strNumDoc = strNumDoc.Trim()


        Dim strCodImpuestos As String = General.EjecutarConsulta(" Select U_CodImp from [@SCGD_CONF_FINANC] ", StrConexion)
        strCodImpuestos = strCodImpuestos.Trim()

        Try

            strMonedaLocal = General.RetornarMonedaLocal(_companySbo)

            oFactura = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

            oFactura.DocDate = dtFechaPago
            oFactura.Comments = strComentario

            oFactura.CardCode = strCliente
            oFactura.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service
            oFactura.DocCurrency = strMoneda
            oFactura.Series = strNumDoc

            decTotalIntereses = decIntNormal + decIntMora

            If decIntNormal > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntNormal
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = decIntNormal
                Else
                    oFactura.Lines.RowTotalFC = decIntNormal
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.InteresNormal
                oFactura.Lines.Add()
            End If

            If decIntMora > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = decIntMora
                Else
                    oFactura.Lines.RowTotalFC = decIntMora
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.InteresMora
                oFactura.Lines.Add()
            End If

            If dbRecargoCobranza > 0 Then
                oFactura.Lines.AccountCode = strCuentaIntMora
                If strMoneda = strMonedaLocal Then
                    oFactura.Lines.LineTotal = dbRecargoCobranza
                Else
                    oFactura.Lines.RowTotalFC = dbRecargoCobranza
                End If
                oFactura.Lines.Currency = strMoneda
                oFactura.Lines.TaxCode = strCodImpuestos
                oFactura.Lines.VatGroup = strCodImpuestos
                oFactura.Lines.ItemDescription = My.Resources.Resource.RecargoCobranza
                oFactura.Lines.Add()
            End If

            If oFactura.Add <> 0 Then
                _companySbo.GetLastError(intError, strMensajeError)
                _applicationSbo.SetStatusBarMessage(strMensajeError, BoMessageTime.bmt_Long, True)
                If _companySbo.InTransaction() Then
                    _companySbo.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                BubbleEvent = False
            Else
                _companySbo.GetNewObjectCode(strAsientoInteres)
                blnAsientoIntGenerado = True
            End If

        Catch ex As Exception
            BubbleEvent = False
            Throw ex

        End Try


    End Sub


End Class
