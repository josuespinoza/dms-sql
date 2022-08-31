Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO

'Clase para manejar funcionalidad de formulario de histórico de pagos de modulo de financiamiento

Partial Public Class HistoricoPagos

    'Maneja evento de Choose From List de cliente

    Public Sub CFLCliente(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

        Try

            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID
            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions

            Dim oDataTable As SAPbouiCOM.DataTable

            If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    EditTextCodCliente.AsignaValorUserDataSource("")
                    EditTextNombreCliente.AsignaValorUserDataSource("")

                    oDataTable = oCFLEvento.SelectedObjects

                    EditTextCodCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardCode", 0))
                    EditTextNombreCliente.AsignaValorUserDataSource(oDataTable.GetValue("CardName", 0))

                End If

            ElseIf pVal.BeforeAction = True Then

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardType"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "C"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Maneja evento de botón imprimir reporte de histórico de pagos, validaciones y carga de reporte

    Public Sub ButtonSBOImprimirHistoricoItemPresed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String
        Dim strCliente As String = ""
        Dim strFecha As String = ""
        Dim dtFecha As Date
        Dim strParametros As String
        Dim strPrestamo As String
        Dim strTodos As String

        strCliente = EditTextCodCliente.ObtieneValorUserDataSource()

        strFecha = EditTextFecha.ObtieneValorUserDataSource()

        strTodos = ChkTodos.ObtieneValorUserDataSource()

        If pVal.BeforeAction = True Then

            If ((String.IsNullOrEmpty(strCliente) OrElse String.IsNullOrEmpty(strFecha)) And strTodos = "N") Or (strTodos = "Y" And String.IsNullOrEmpty(strFecha)) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCargaReporte, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            strPrestamo = General.EjecutarConsulta("Select DocEntry From [@SCGD_PRESTAMO] Where U_Cod_Cli = '" & strCliente & "'", StrConexion)
            dtFecha = Date.ParseExact(strFecha, "yyyyMMdd", Nothing)
            dtFecha = New Date(dtFecha.Year, dtFecha.Month, dtFecha.Day, 0, 0, 0)
            Select Case strTodos
                Case "N"

                    If Not String.IsNullOrEmpty(strPrestamo) Then
                        strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptCuotasPagadas & ".rpt"

                        strParametros = strCliente & "," & dtFecha

                        Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloRepHistorico, strParametros, StrUsuarioBD, StrContraseñaBD)

                    ElseIf String.IsNullOrEmpty(strPrestamo) Then

                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorClientePrestamo, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

                    End If
                Case "Y"
                    strDireccionReporte = StrDireccionReportes & My.Resources.Resource.rptCuotasPagadasTodos & ".rpt"

                    strParametros = dtFecha

                    Call General.ImprimirReporte(_companySbo, strDireccionReporte, My.Resources.Resource.TituloRepHistorico, strParametros, StrUsuarioBD, StrContraseñaBD)
            End Select
        End If

    End Sub

End Class
