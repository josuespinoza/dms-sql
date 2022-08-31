Partial Public Class EmbarqueVehiculos

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)


        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)

        Dim sCFL_ID As String
        Dim oForm As SAPbouiCOM.Form
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oDataTable As SAPbouiCOM.DataTable

        sCFL_ID = oCFLEvento.ChooseFromListUID
        oForm = ApplicationSBO.Forms.Item(FormUID)
        oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim intPosicion As Integer

        If oCFLEvento.ActionSuccess AndAlso pval.ItemUID = "mtxArt" AndAlso pval.ColUID = "ColCod" Then

            oDataTable = oCFLEvento.SelectedObjects
            If Not oCFLEvento.SelectedObjects Is Nothing Then
                MatrizArticulos.Matrix.FlushToDataSource()
                intPosicion = dtArticulos.Rows.Count
                dtArticulos.SetValue("cod", intPosicion - 1, oDataTable.GetValue("ItemCode", 0))
                dtArticulos.SetValue("des", intPosicion - 1, oDataTable.GetValue("ItemName", 0))
                dtArticulos.Rows.Add(1)
                dtArticulos.SetValue("cod", intPosicion, "")
                MatrizArticulos.Matrix.LoadFromDataSource()
            End If
            'se condiciona la columna ya que este alias funciona para el chooseFromList de Cuentas de Stocks y
            'no para el choosefromlist de Almacenes
        ElseIf oCFLEvento.BeforeAction AndAlso pval.ItemUID = "mtxArt" AndAlso pval.ColUID = "ColCod" Then

            oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "U_SCGD_TipoArticulo"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = "8"
            oCondition.BracketCloseNum = 1
            oCFL.SetConditions(oConditions)

        End If

    End Sub
End Class
