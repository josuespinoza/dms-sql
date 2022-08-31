Imports SAPbouiCOM

Public Class MaestroSociosNegocio

#Region "... Declaraciones ..."

    Private m_oCompany As SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private Const mc_strStTipoSN As String = "stTipoSN"
    Private Const mc_strtxtTipoSN As String = "txtTipoSN"
    Private Const mc_strOCRD As String = "OCRD"
    Private Const mc_strUDFCusType As String = "U_SCGD_CusType"

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
                    userDS.Add("txtTipoSN", BoDataType.dt_LONG_TEXT, 100)
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
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oStaticText As SAPbouiCOM.StaticText
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            intTop = oform.Items.Item("350001035").Top
            intLeft = oform.Items.Item("350001035").Left
            intWidth = oform.Items.Item("350001035").Width
            intHeight = oform.Items.Item("350001035").Height

            oItem = oform.Items.Add(mc_strtxtTipoSN, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oItem.Top = intTop + 1
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True
            oItem.DisplayDesc = True

            oCombo = oItem.Specific
            Call oCombo.DataBind.SetBound(True, mc_strOCRD, mc_strUDFCusType)

            intTop = oform.Items.Item("350001034").Top
            intLeft = oform.Items.Item("350001034").Left
            intWidth = oform.Items.Item("350001034").Width
            intHeight = oform.Items.Item("350001034").Height

            oItem = Nothing
            oItem = oform.Items.Add(mc_strStTipoSN, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = intTop + 1
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oItem.Enabled = True
            oItem.Visible = True

            oStaticText = oItem.Specific
            oStaticText.Item.LinkTo = mc_strtxtTipoSN
            oStaticText.Caption = My.Resources.Resource.TXTTipoSN

        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

#End Region

End Class
