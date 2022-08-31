Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO

'Clase para manejo de funcionalidad de formulario de configuraciones del modulo de financiamiento

Partial Public Class ConfiguracionFormulario

    'Manejo de eventos de los Choose From List de cuentas de la pantalla de configuraciones de financiamiento

    Public Sub CFLEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByVal strUDF As String)

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

            If pVal.BeforeAction Then

                Select Case pVal.ItemUID

                    Case EditTextCodImp.UniqueId

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add()
                        If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "Category"
                            oCondition.CondVal = "O"
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

                    Case Else

                        oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "FatherNum"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = BoConditionRelationship.cr_AND

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = "Postable"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "Y"
                        oCondition.BracketCloseNum = 2
                        oCFL.SetConditions(oConditions)

                        FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                End Select

            End If

            If pVal.ActionSuccess Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    oDataTable = oCFLEvento.SelectedObjects

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CONF_FINANC").SetValue(strUDF, 0, oDataTable.GetValue(0, 0))

                End If

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Public Sub ManejadorEventoLoad(ByVal FormUID As String, _
                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                            ByRef BubbleEvent As Boolean, _
                             ByVal p_DBUser As String, _
                             ByVal p_DBPassword As String)

        General.DBPassword = p_DBPassword
        General.DBUser = p_DBUser
        
        Try
            
            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then
              
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
