Imports SAPbouiCOM

Public Class MaestroEmpleados
#Region "... Declaraciones ..."

    Private m_oCompany As SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private Const mc_strStNationalID As String = "stNatID"
    Private Const mc_strtxtNationalID As String = "txtstNatID"
    Private Const mc_strOHEM As String = "OHEM"
    Private Const mc_strUDFNationalID As String = "U_SCGD_NationalID"

#End Region

#Region "... Constructor ..."

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal p_SBOAplication As Application)

        m_oCompany = ocompany
        SBO_Application = p_SBOAplication

    End Sub

#End Region

#Region "...Eventos..."

    Public Sub ManejadorEventoLoad(ByVal FormUID As String, _
                           ByRef pVal As SAPbouiCOM.ItemEvent, _
                           ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If pVal.BeforeAction Then
                Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                If usaInterFazFord Then
                    Dim userDS As UserDataSources = oForm.DataSources.UserDataSources
                    userDS.Add("txtstNatID", BoDataType.dt_LONG_TEXT, 100)
                    AgregaTipoSN(oForm, SBO_Application)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "...Metodos..."

    Public Shared Function AgregaTipoSN(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim result As Boolean = True
        Dim oEdit As SAPbouiCOM.EditText
        Dim oStaticText As SAPbouiCOM.StaticText
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            intTop = oform.Items.Item("49").Top
            intLeft = oform.Items.Item("49").Left
            intWidth = oform.Items.Item("49").Width
            intHeight = oform.Items.Item("49").Height

            oItem = oform.Items.Add(mc_strtxtNationalID, SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            oItem.DisplayDesc = True

            oEdit = oItem.Specific
            Call oEdit.DataBind.SetBound(True, mc_strOHEM, mc_strUDFNationalID)

            intTop = oform.Items.Item("14").Top
            intLeft = oform.Items.Item("14").Left
            intWidth = oform.Items.Item("14").Width
            intHeight = oform.Items.Item("14").Height

            oItem = Nothing
            oItem = oform.Items.Add(mc_strStNationalID, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = intTop + 16
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True

            oStaticText = oItem.Specific
            oStaticText.Item.LinkTo = mc_strtxtNationalID
            oStaticText.Caption = My.Resources.Resource.TXTNationalID '"Tipo Socio de Negocio"

        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

#End Region

End Class
