Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports SCG.SBOFramework

'Clase para manejar la funcionalidad del formulario de plan de pagos del modulo de financiamiento

Partial Public Class PlanPagosFormulario

    Private m_strMonedaLocal As String = ""

    Public g_decSaldoInicial() As Decimal
    Public g_decInteres() As Decimal
    Public g_decCuota() As Decimal
    Public g_decCapital() As Decimal
    Public g_decSaldoFinal() As Decimal
    Public g_intNumero() As Integer
    Public g_dtFechaPago() As Date
    Public g_decMoratorios() As Decimal
    Public g_strPagado() As String
    Public g_strNotaCred() As String
    Public g_strDocInt() As String
    Public g_strDocFac() As String
    Public g_strBorrador() As String
    Public g_decCapPend() As Decimal
    Public g_decIntPend() As Decimal
    Public g_decMoraPend() As Decimal
    Public g_intDiasInt() As Integer
    Public g_intDiasMora() As Integer

    'Realiza los calculos para el plan de pagos si el tipo de cuota es Nivelada o Variable (Intereses, Capital, Cuota)
    'Se abona de cuota lo mismo en cada pago, conforme avanzan los pagos se abona menos a intereses y más a capital

    Public Sub CalculoNivelada(ByVal intPlazo As Integer, ByVal decSaldo As Decimal, ByVal decInteresNormal As Decimal, _
                               ByVal dtFechaInicio As Date, ByVal intDiaPago As Integer, ByVal blnCambioIntNormal As Boolean, ByVal strCambiaPlazo As String, _
                               Optional ByVal intDiasInt As Integer = 30, Optional ByVal decCuotaOriginal As Decimal = 0, _
                               Optional ByVal blnPagoExtra As Boolean = False, Optional ByVal decCapPend As Decimal = 0, Optional ByVal decIntPend As Decimal = 0, Optional ByVal decMoraPend As Decimal = 0)

        Dim decCuota As Decimal
        Dim intDifDiasInicio As Integer = 0

        Try

            If strCambiaPlazo = "Y" Then

                decCuota = decCuotaOriginal

            Else

                Call CuotaNivelada(decSaldo, decInteresNormal, intPlazo, decCuota)

            End If

            For i As Integer = 0 To intPlazo - 1

                If Not strCambiaPlazo = "Y" Then

                    ReDim Preserve g_intNumero(i)
                    g_intNumero(i) = i + 1

                End If

                ReDim Preserve g_decSaldoInicial(i)
                g_decSaldoInicial(i) = decSaldo

                ReDim Preserve g_decCuota(i)
                g_decCuota(i) = decCuota

                If i = 0 Then

                    g_decSaldoInicial(i) = g_decSaldoInicial(i) + decCapPend

                    g_decCuota(i) = decCuota + decCapPend + decIntPend + decMoraPend

                End If

                Call InteresNivelada(decSaldo, decInteresNormal, i, blnCambioIntNormal, intDiasInt, intDiaPago, dtFechaInicio, blnPagoExtra)

                Call CapitalNivelada(i)

                If i = 0 Then

                    g_decCapital(i) = g_decCapital(i) - decCapPend - decIntPend - decMoraPend

                End If

                If strCambiaPlazo = "Y" AndAlso g_decCapital(i) > decSaldo Then

                    g_decCapital(i) = decSaldo
                    g_decCuota(i) = g_decCapital(i) + g_decInteres(i)

                End If

                decSaldo = decSaldo - g_decCapital(i)

                ReDim Preserve g_decSaldoFinal(i)
                g_decSaldoFinal(i) = decSaldo

                Call FechasPago(dtFechaInicio, intDiaPago, i)

                ReDim Preserve g_decMoratorios(i)
                g_decMoratorios(i) = 0

                If strCambiaPlazo = "Y" AndAlso g_decSaldoInicial(i) = 0 Then

                    ReDim Preserve g_strPagado(i)
                    g_strPagado(i) = "Y"

                Else

                    ReDim Preserve g_strPagado(i)
                    g_strPagado(i) = "N"

                End If

                ReDim Preserve g_strNotaCred(i)
                g_strNotaCred(i) = ""

                ReDim Preserve g_strDocInt(i)
                g_strDocInt(i) = ""

                ReDim Preserve g_strDocFac(i)
                g_strDocFac(i) = ""

                ReDim Preserve g_strBorrador(i)
                g_strBorrador(i) = ""

                ReDim Preserve g_decCapPend(i)
                g_decCapPend(i) = 0

                ReDim Preserve g_decIntPend(i)
                g_decIntPend(i) = 0

                ReDim Preserve g_decMoraPend(i)
                g_decMoraPend(i) = 0

                If i = 0 Then

                    g_decCapPend(i) = decCapPend
                    g_decIntPend(i) = decIntPend
                    g_decMoraPend(i) = decMoraPend

                End If

                ReDim Preserve g_intDiasInt(i)

                If i = 0 AndAlso blnCambioIntNormal = False AndAlso blnPagoExtra = False Then

                    If intDiaPago > dtFechaInicio.Day Then

                        intDifDiasInicio = intDiaPago - dtFechaInicio.Day
                        g_intDiasInt(i) = 30 + intDifDiasInicio

                    ElseIf intDiaPago < dtFechaInicio.Day Then

                        g_intDiasInt(i) = 30 - dtFechaInicio.Day + intDiaPago

                    ElseIf intDiaPago = dtFechaInicio.Day Then

                        g_intDiasInt(i) = 30

                    End If

                ElseIf i = 0 AndAlso blnCambioIntNormal = False AndAlso blnPagoExtra = True Then

                    g_intDiasInt(i) = intDiasInt

                ElseIf i = 0 AndAlso blnCambioIntNormal = True Then

                    g_intDiasInt(i) = intDiasInt

                Else

                    g_intDiasInt(i) = 30

                End If

                ReDim Preserve g_intDiasMora(i)
                g_intDiasMora(i) = 0

            Next

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Realiza los calculos para el plan de pagos si el tipo de cuota es Decreciente (Intereses, Capital, Cuota)
    'Se abona conforme avanzan los pagos menos de cuota, lo mismo de capital y menos de intereses

    Public Sub CalculoDecreciente(ByVal intPlazo As Integer, ByVal decSaldo As Decimal, ByVal decInteresNormal As Decimal, _
                               ByVal dtFechaInicio As Date, ByVal intDiaPago As Integer, ByVal strCambiaPlazo As String, _
                               Optional ByVal decCapitalExtraOrd As Decimal = 0, Optional ByVal blnCambiaIntNormal As Boolean = False, Optional ByVal decCapPend As Decimal = 0, _
                               Optional ByVal decIntPend As Decimal = 0, Optional ByVal decMoraPend As Decimal = 0)

        Dim decCapital As Decimal

        Try

            If strCambiaPlazo = "Y" Then

                decCapital = decCapitalExtraOrd

            Else

                Call CapitalDecreciente(decSaldo, intPlazo, decCapital)

            End If

            For i As Integer = 0 To intPlazo - 1

                If Not strCambiaPlazo = "Y" Then

                    ReDim Preserve g_intNumero(i)
                    g_intNumero(i) = i + 1

                End If

                ReDim Preserve g_decSaldoInicial(i)
                g_decSaldoInicial(i) = decSaldo

                Call InteresDecreciente(decInteresNormal, decSaldo, i)

                ReDim Preserve g_decCapital(i)
                g_decCapital(i) = decCapital

                Call CuotaDecreciente(decCapital, i)

                If i = 0 Then

                    g_decSaldoInicial(i) = g_decSaldoInicial(i) + decCapPend

                    g_decCuota(i) = g_decCuota(i) + decCapPend + decIntPend + decMoraPend

                End If

                If strCambiaPlazo = "Y" AndAlso g_decCapital(i) > decSaldo Then

                    g_decCapital(i) = decSaldo
                    g_decCuota(i) = g_decCapital(i) + g_decInteres(i)

                End If

                decSaldo = decSaldo - g_decCapital(i)

                ReDim Preserve g_decSaldoFinal(i)
                g_decSaldoFinal(i) = decSaldo

                Call FechasPago(dtFechaInicio, intDiaPago, i)

                ReDim Preserve g_decMoratorios(i)
                g_decMoratorios(i) = 0

                If strCambiaPlazo = "Y" AndAlso g_decSaldoInicial(i) = 0 Then

                    ReDim Preserve g_strPagado(i)
                    g_strPagado(i) = "Y"

                Else

                    ReDim Preserve g_strPagado(i)
                    g_strPagado(i) = "N"

                End If

                ReDim Preserve g_strNotaCred(i)
                g_strNotaCred(i) = ""

                ReDim Preserve g_strDocInt(i)
                g_strDocInt(i) = ""

                ReDim Preserve g_strDocFac(i)
                g_strDocFac(i) = ""

                ReDim Preserve g_strBorrador(i)
                g_strBorrador(i) = ""

                ReDim Preserve g_decCapPend(i)
                g_decCapPend(i) = 0

                ReDim Preserve g_decIntPend(i)
                g_decIntPend(i) = 0

                ReDim Preserve g_decMoraPend(i)
                g_decMoraPend(i) = 0

                If i = 0 Then

                    g_decCapPend(i) = decCapPend
                    g_decIntPend(i) = decIntPend
                    g_decMoraPend(i) = decMoraPend

                End If

                ReDim Preserve g_intDiasInt(i)
                g_intDiasInt(i) = 30

                ReDim Preserve g_intDiasMora(i)
                g_intDiasMora(i) = 0

            Next

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Realiza los calculos para el plan de pagos si el tipo de cuota es Pago Global (Intereses, Capital, Cuota)
    'Durante el préstamo se abona solo interes, solo en la última cuota se abona todo el monto financiado

    Public Sub CalculoGlobal(ByVal intPlazo As Integer, ByVal decSaldo As Decimal, ByVal decInteresNormal As Decimal, _
                               ByVal dtFechaInicio As Date, ByVal intDiaPago As Integer)

        Try

            For i As Integer = 0 To intPlazo - 1

                ReDim Preserve g_intNumero(i)
                g_intNumero(i) = i + 1

                ReDim Preserve g_decSaldoInicial(i)
                g_decSaldoInicial(i) = decSaldo

                Call InteresGlobal(decSaldo, decInteresNormal, i)

                Call CuotaGlobal(decSaldo, i, intPlazo)

                Call CapitalGlobal(decSaldo, i, intPlazo)

                ReDim Preserve g_decSaldoFinal(i)

                If i < intPlazo - 1 Then

                    g_decSaldoFinal(i) = decSaldo

                ElseIf i = intPlazo - 1 Then

                    g_decSaldoFinal(i) = 0

                End If

                Call FechasPago(dtFechaInicio, intDiaPago, i)

                ReDim Preserve g_decMoratorios(i)
                g_decMoratorios(i) = 0

                ReDim Preserve g_strPagado(i)
                g_strPagado(i) = "N"

                ReDim Preserve g_strNotaCred(i)
                g_strNotaCred(i) = ""

                ReDim Preserve g_strDocInt(i)
                g_strDocInt(i) = ""

                ReDim Preserve g_strDocFac(i)
                g_strDocFac(i) = ""

                ReDim Preserve g_strBorrador(i)
                g_strBorrador(i) = ""

                ReDim Preserve g_decCapPend(i)
                g_decCapPend(i) = 0

                ReDim Preserve g_decIntPend(i)
                g_decIntPend(i) = 0

                ReDim Preserve g_decMoraPend(i)
                g_decMoraPend(i) = 0

                ReDim Preserve g_intDiasInt(i)
                g_intDiasInt(i) = 30

                ReDim Preserve g_intDiasMora(i)
                g_intDiasMora(i) = 0

            Next

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Determina las fechas de todos los pagos del plan según la fecha de inicio del préstamo y el día del mes en que se realizan los pagos, se guarda en un arreglo

    Private Sub FechasPago(ByVal dtFechaInicio As Date, ByVal intDiaPago As Integer, ByVal intPosicion As Integer)

        Dim intDiaFecha As Integer

        Try

            ReDim Preserve g_dtFechaPago(intPosicion)

            g_dtFechaPago(intPosicion) = dtFechaInicio.AddMonths(intPosicion + 1)
            intDiaFecha = DateTime.DaysInMonth(g_dtFechaPago(intPosicion).Year, g_dtFechaPago(intPosicion).Month)
            If intDiaFecha >= intDiaPago Then
                intDiaFecha = intDiaPago
            End If

            g_dtFechaPago(intPosicion) = New Date(g_dtFechaPago(intPosicion).Year, g_dtFechaPago(intPosicion).Month, intDiaFecha)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de la cuota nivelada, si la tasa de interes normal es 0 entonces cuota=saldo/plazo, y si es > 0 entonces cuota se calcula mediante formula compuesta

    Private Sub CuotaNivelada(ByVal decSaldo As Decimal, ByVal decInteresNormal As Decimal, ByVal intPlazo As Integer, ByRef decCuota As Decimal)

        Dim decInteres As Decimal
        Dim decSubFormula As Decimal

        Try

            decInteres = decInteresNormal / 12

            If decInteres = 0 Then

                decCuota = decSaldo / intPlazo

            Else

                decSubFormula = Math.Pow((1 + decInteres), -intPlazo)

                decCuota = decSaldo / ((1 - decSubFormula) / decInteres)

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de interes nivelado, interes compuesto, la formula es: ((saldo a financiar * % tasa de interes normal anual) / 360) * cantidad de días entre los pagos

    Private Sub InteresNivelada(ByVal decSaldo As Decimal, ByVal decInteresNormal As Decimal, ByVal intPosicion As Integer, ByVal blnCambioIntNormal As Boolean, ByVal intDiasInt As Integer, _
                                ByVal intDiaPago As Integer, ByVal dtFechaPrestamo As Date, ByVal blnPagoExtra As Boolean)

        Dim intDiferenciaDias As Integer = 0
        Dim sngInteresDiasSobra As Decimal

        Try

            ReDim Preserve g_decInteres(intPosicion)
            g_decInteres(intPosicion) = ((decSaldo * decInteresNormal) / 360) * 30

            If intPosicion = 0 AndAlso blnCambioIntNormal = True Then

                g_decCuota(0) = g_decCuota(0) - g_decInteres(0)

                g_decInteres(0) = ((decSaldo * decInteresNormal) / 360) * intDiasInt

                g_decCuota(0) = g_decCuota(0) + g_decInteres(0)

            ElseIf intPosicion = 0 AndAlso blnCambioIntNormal = False AndAlso blnPagoExtra = False Then

                If intDiaPago > dtFechaPrestamo.Day Then

                    intDiferenciaDias = intDiaPago - dtFechaPrestamo.Day

                    sngInteresDiasSobra = ((decSaldo * decInteresNormal) / 360) * intDiferenciaDias

                    g_decInteres(0) = g_decInteres(0) + sngInteresDiasSobra
                    g_decCuota(0) = g_decCuota(0) + sngInteresDiasSobra

                ElseIf intDiaPago < dtFechaPrestamo.Day Then

                    intDiferenciaDias = 30 - dtFechaPrestamo.Day + intDiaPago

                    sngInteresDiasSobra = ((decSaldo * decInteresNormal) / 360) * intDiferenciaDias

                    g_decCuota(0) = g_decCuota(0) - g_decInteres(0)

                    g_decInteres(0) = sngInteresDiasSobra

                    g_decCuota(0) = g_decCuota(0) + g_decInteres(0)

                End If

            ElseIf intPosicion = 0 AndAlso blnCambioIntNormal = False AndAlso blnPagoExtra = True Then

                intDiferenciaDias = intDiasInt - 30

                If intDiferenciaDias > 0 Then

                    sngInteresDiasSobra = ((decSaldo * decInteresNormal) / 360) * intDiferenciaDias

                    g_decInteres(0) = g_decInteres(0) + sngInteresDiasSobra
                    g_decCuota(0) = g_decCuota(0) + sngInteresDiasSobra

                ElseIf intDiferenciaDias < 0 Then

                    g_decCuota(0) = g_decCuota(0) - g_decInteres(0)

                    sngInteresDiasSobra = ((decSaldo * decInteresNormal) / 360) * intDiasInt

                    g_decInteres(0) = sngInteresDiasSobra

                    g_decCuota(0) = g_decCuota(0) + g_decInteres(0)

                End If

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de capital nivelado por pago, cuota - intereses

    Private Sub CapitalNivelada(ByVal intPosicion As Decimal)

        Try

            ReDim Preserve g_decCapital(intPosicion)
            g_decCapital(intPosicion) = g_decCuota(intPosicion) - g_decInteres(intPosicion)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de interes decreciente, interes sencillo, (tasa de interes normal / 12) * saldo

    Private Sub InteresDecreciente(ByVal decInteresNormal As Decimal, ByVal decSaldo As Decimal, ByVal intPosicion As Integer)

        Dim decInteres As Decimal

        Try

            decInteres = decInteresNormal / 12

            ReDim Preserve g_decInteres(intPosicion)
            g_decInteres(intPosicion) = decSaldo * decInteres

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de capital decreciente, saldo / plazo

    Private Sub CapitalDecreciente(ByVal decSaldo As Decimal, ByVal intPlazo As Integer, ByRef decCapital As Decimal)

        Try

            decCapital = decSaldo / intPlazo

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de cuota decreciente, capital + intereses 

    Private Sub CuotaDecreciente(ByVal decCapital As Decimal, ByVal intPosicion As Integer)

        Try

            ReDim Preserve g_decCuota(intPosicion)
            g_decCuota(intPosicion) = decCapital + g_decInteres(intPosicion)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de interes global, interes sencillo, (tasa de interes normal / 12) * saldo

    Private Sub InteresGlobal(ByVal decSaldo As Decimal, ByVal decIntNormal As Decimal, ByVal intPosicion As Integer)

        Dim decInteres As Decimal

        Try

            decInteres = decIntNormal / 12

            ReDim Preserve g_decInteres(intPosicion)
            g_decInteres(intPosicion) = decSaldo * decInteres

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de cuota global, si el pago no es el último paga solo intereses, si es el último paga interes más el monto del financiamiento

    Private Sub CuotaGlobal(ByVal decSaldo As Decimal, ByVal intPosicion As Integer, ByVal intPlazo As Integer)

        Try

            ReDim Preserve g_decCuota(intPosicion)

            If intPosicion < intPlazo - 1 Then

                g_decCuota(intPosicion) = g_decInteres(intPosicion)

            ElseIf intPosicion = intPlazo - 1 Then

                g_decCuota(intPosicion) = decSaldo + g_decInteres(intPosicion)

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Calculo de capital global, si el pago no es el último no paga nada, si es el último paga el monto del financiamiento

    Private Sub CapitalGlobal(ByVal decSaldo As Decimal, ByVal intPosicion As Integer, ByVal intPlazo As Integer)

        Try

            ReDim Preserve g_decCapital(intPosicion)

            If intPosicion < intPlazo - 1 Then

                g_decCapital(intPosicion) = 0

            ElseIf intPosicion = intPlazo - 1 Then

                g_decCapital(intPosicion) = decSaldo

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Manejo de evento de botón de imprimir reporte de plan de pagos, ya sea el plan de pagos teórico, real, o antes de estar generado el préstamo

    Public Sub ButtonSBOImprimirPlanItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strParametros As String
        Dim strTituloReporte As String = ""

        If pVal.BeforeAction = True Then

            If String.IsNullOrEmpty(g_strPrestamo) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

        ElseIf pVal.BeforeAction = False Then

            If g_blnImprimeCreado = True Then

                If g_strTipoPlan = "T" Then

                    strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptPlanTeorico & ".rpt"

                    strTituloReporte = My.Resources.Resource.TituloRepPlanTeorico

                ElseIf g_strTipoPlan = "R" Then

                    strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptPlanReal & ".rpt"

                    strTituloReporte = My.Resources.Resource.TituloRepPlanReal

                End If

            ElseIf g_blnImprimeCreado = False Then

                strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptPlanTemporal & ".rpt"

                strTituloReporte = My.Resources.Resource.TituloRepPlanTemporal

            End If

            strParametros = g_strPrestamo

            Call General.ImprimirReporte(_companySbo, strDireccionReporte, strTituloReporte, strParametros, StrUsuarioBD, StrContraseñaBD)

        End If

    End Sub

    'Manejo de evento de cierre de formulario de plan de pagos para borrar datos de tabla de préstamo temporal y plan de pagos temporal si se carga antes de crear el préstamo

    Public Sub PlanPagosFormClose(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            If g_blnImprimeCreado = False Then

                General.EjecutarConsulta("Delete From [@SCGD_PREST_TEMP] where Code = '" & g_strPrestamo & "'", StrConexion)

                General.EjecutarConsulta("Delete From [@SCGD_PLAN_TEMP] where U_Pres_Temp = '" & g_strPrestamo & "'", StrConexion)

            End If

        End If

    End Sub

End Class
