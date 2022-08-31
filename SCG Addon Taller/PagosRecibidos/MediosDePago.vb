Option Strict Off
Option Explicit On
Imports System.Collections.Generic
Imports SAPbouiCOM
Imports SCG.UX.Windows
Imports DMSOneFramework.SCGCommon
Imports System.Threading

Friend Class MediosDePago

#Region "Estructuras"

#End Region

#Region "Enumerados"

#End Region

#Region "Declaraciones"

    Private m_SBO_Application As Application
    Private mc_strFormMediosPago As String = "146"
    Private strTablaBancos As String = "ORCT"
    Private strBankCode As String = "BankCode"
    Private strBankName As String = "BankName"
    Private strTxtBankCode As String = "txtBanCo"
    Private strTxtBankDescription As String = "txtBanDe"
    Private strUsaBancoCliente As String = String.Empty


#End Region

#Region "Constructor"


    Private Shared m_oCompany As SAPbobsCOM.Company


    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, _
                    ByVal p_oCompany As SAPbobsCOM.Company)

        m_SBO_Application = p_SBO_Application
        m_oCompany = p_oCompany

    End Sub

#End Region

#Region "Propiedades"

#End Region

#Region "Metodos"

    Public Sub ManejadorEventoLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        Dim oItem As SAPbouiCOM.Item
        Dim oCodigoBanco As SAPbouiCOM.EditText
        Dim oDescripcionBanco As SAPbouiCOM.EditText
        Dim oStaticText As SAPbouiCOM.StaticText

        Try

            strUsaBancoCliente = DMS_Connector.Configuracion.ParamGenAddon.U_UsaBanCl

            If strUsaBancoCliente.ToUpper() = "Y" Then

                If pVal.FormTypeEx = mc_strFormMediosPago Then
                    oForm = m_SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                    If pVal.BeforeAction Then

                        oForm.DataSources.DBDataSources.Add(strTablaBancos)

                        oItem = oForm.Items.Add("lblBanCo", BoFormItemTypes.it_STATIC)
                        oItem.Left = 8
                        oItem.Width = 100
                        oItem.Top = 143
                        oItem.Height = 14
                        oItem.FromPane = 2
                        oItem.ToPane = 2

                        oStaticText = oItem.Specific
                        oStaticText.Caption = My.Resources.Resource.BancoCliente

                        oItem = oForm.Items.Add("txtBanCo", BoFormItemTypes.it_EDIT)
                        oItem.Left = 131
                        oItem.Width = 100
                        oItem.Top = 143
                        oItem.Height = 14
                        oItem.FromPane = 2
                        oItem.ToPane = 2
                        oCodigoBanco = oItem.Specific

                        oItem = oForm.Items.Add("txtBanDe", BoFormItemTypes.it_EDIT)
                        oItem.Left = 235
                        oItem.Width = 215
                        oItem.Top = 143
                        oItem.Height = 14
                        oItem.FromPane = 2
                        oItem.ToPane = 2
                        oDescripcionBanco = oItem.Specific


                        oCodigoBanco.DataBind.SetBound(True, strTablaBancos, "U_SCGD_BancoCli")
                        oDescripcionBanco.DataBind.SetBound(True, strTablaBancos, "U_SCGD_BancoDes")
                        AddChooseFromListBancoCliente(oForm)
                        oCodigoBanco.ChooseFromListUID = "CFLBC"
                        oCodigoBanco.ChooseFromListAlias = strBankCode

                    End If

                End If



            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim sCFL_ID As String
        Dim oCFL As SAPbouiCOM.ChooseFromList

        Try

            strUsaBancoCliente = DMS_Connector.Configuracion.ParamGenAddon.U_UsaBanCl

            If strUsaBancoCliente.ToUpper() = "Y" Then

                If pVal.FormTypeEx = mc_strFormMediosPago Then
                    oForm = m_SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                    If Not oForm Is Nothing AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                        If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

                            oCFLEvento = pVal

                            sCFL_ID = oCFLEvento.ChooseFromListUID

                            oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                            If oCFLEvento.BeforeAction = False Then
                                Dim oDataTable As SAPbouiCOM.DataTable
                                oDataTable = oCFLEvento.SelectedObjects

                                If Not oDataTable Is Nothing Then
                                    If pVal.ItemUID = strTxtBankCode Then
                                        AsignarValoresSeleccionados(oForm, oDataTable, strTxtBankCode, strBankCode)
                                        AsignarValoresSeleccionados(oForm, oDataTable, strTxtBankDescription, strBankName)
                                    End If
                                End If
                            End If

                        End If

                    End If

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' Maneja los eventos de LostFocus
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim strValor As String = String.Empty

        Try
            strUsaBancoCliente = DMS_Connector.Configuracion.ParamGenAddon.U_UsaBanCl

            If strUsaBancoCliente.ToUpper() = "Y" Then

                If pVal.ActionSuccess Then

                    If pVal.ItemUID = strTxtBankCode Then

                        oForm = m_SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                        strValor = oForm.Items.Item(strTxtBankCode).Specific.Value

                        If String.IsNullOrEmpty(strValor) Then
                            oForm.Items.Item(strTxtBankDescription).Specific.Value = String.Empty
                        End If

                    End If
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)


        Dim oForm As SAPbouiCOM.Form
        Dim strValor As String = String.Empty

        Try
            strUsaBancoCliente = DMS_Connector.Configuracion.ParamGenAddon.U_UsaBanCl

            If strUsaBancoCliente.ToUpper() = "Y" Then

                If pVal.ItemUID = "1" Then
                    oForm = m_SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                    strValor = oForm.Items.Item(strTxtBankCode).Specific.Value

                    If String.IsNullOrEmpty(strValor) Then
                        oForm.Items.Item(strTxtBankDescription).Specific.Value = String.Empty
                    End If

                End If

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub



    ''' <summary>
    ''' Asigna los valores del DataTable del ChooseFromList a sus respectivos campos
    ''' </summary>
    ''' <param name="oForm">Formulario de SAP</param>
    ''' <param name="oDataTable">DataTable con los resultados del ChooseFromList</param>
    ''' <param name="strCampo">Nombre del campo de texto del formulario en el cual se va a asignar el valor</param>
    ''' <param name="strColumna">Nombre de la columna del DataTable desde la cual se va a obtener el valor</param>
    ''' <remarks></remarks>
    Private Sub AsignarValoresSeleccionados(ByVal oForm As SAPbouiCOM.Form, ByVal oDataTable As SAPbouiCOM.DataTable, ByVal strCampo As String, ByVal strColumna As String)
        Try
            If Not oDataTable.GetValue(strColumna, 0) Is System.Convert.DBNull Then
                oForm.Items.Item(strCampo).Specific.Value = oDataTable.GetValue(strColumna, 0)
            End If
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Agrega los ChooseFromList al formulario para traer la información de los bancos
    ''' </summary>
    ''' <param name="oform">Formulario al que se desea agrega el ChooseFromList</param>
    ''' <remarks></remarks>
    Private Sub AddChooseFromListBancoCliente(ByVal oform As Form)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "3"
            oCFLCreationParams.UniqueID = "CFLBC"
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = strBankCode
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCFL.SetConditions(oCons)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

End Class
