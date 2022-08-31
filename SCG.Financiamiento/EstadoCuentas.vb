Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO

'Clase para manejar la funcionalidad del formulario de reporte de estado de cuentas del modulo de financiamiento

Partial Public Class EstadoCuentas

    'Manejo de evento de Choose From List de préstamo

    Public Sub CFLPrestamo(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

        Try

            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID
            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim oDataTable As SAPbouiCOM.DataTable

            Dim strFecha As String = ""
            Dim dtFecha As Date

            If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    EditTextPrestamo.AsignaValorUserDataSource("")
                    EditTextCliente.AsignaValorUserDataSource("")
                    EditTextFecha.AsignaValorUserDataSource("")
                    EditTextContrato.AsignaValorUserDataSource("")

                    oDataTable = oCFLEvento.SelectedObjects

                    strFecha = oDataTable.GetValue("U_Fec_Pres", 0)
                    If Not String.IsNullOrEmpty(strFecha) Then
                        dtFecha = Date.Parse(strFecha)
                    End If

                    EditTextPrestamo.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0))
                    EditTextCliente.AsignaValorUserDataSource(oDataTable.GetValue("U_Des_Cli", 0))
                    If Not String.IsNullOrEmpty(strFecha) Then
                        EditTextFecha.AsignaValorUserDataSource(dtFecha.ToString("yyyyMMdd"))
                    End If
                    EditTextContrato.AsignaValorUserDataSource(oDataTable.GetValue("U_Cont_Ven", 0))

                End If

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Manejo de evento de botón de imprimir reporte de estado de cuentas, validaciones y carga de reporte según datos ingresados

    Public Sub ButtonSBOImprimirEstadoItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String
        Dim strPrestamo As String = ""
        Dim strParametros As String
        Dim strTodos As String
        strPrestamo = EditTextPrestamo.ObtieneValorUserDataSource()
        strTodos = ChkTodos.ObtieneValorUserDataSource()
        If pVal.BeforeAction = True Then

            If (String.IsNullOrEmpty(strPrestamo) And strTodos = "N") Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            Select strTodos
                Case "N"
                    strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptEstadosCuenta & ".rpt"

                    strParametros = strPrestamo

                    Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloRepEstados, strParametros, StrUsuarioBD, StrContraseñaBD)
                Case "Y"
                    strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptEstadosCuentaTodos & ".rpt"
                    strParametros = " "
                    Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloRepEstados, strParametros, StrUsuarioBD, StrContraseñaBD)
            End Select
        End If

    End Sub

End Class
