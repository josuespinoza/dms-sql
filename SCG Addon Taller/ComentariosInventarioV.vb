Imports SAPbouiCOM
Imports System

Partial Public Class ComentariosInventarioV

    Dim oMatrixTmp As SAPbouiCOM.Matrix
    Dim otmpForm As SAPbouiCOM.Form

    '''''''''''''''''''''''''''''''''''''''''''''''''
    Private sInput As String = ""
    Private sInputST As String = ""
    Private sTitle As String = ""
    Dim oEditComentariosCV As SAPbouiCOM.EditText
    Private strTitulo As String
    Public Property Titulo() As String
        Get
            Return strTitulo
        End Get
        Set(ByVal value As String)
            strTitulo = value
        End Set
    End Property


    Public Function ShowInput(ByVal Message As String, Optional ByVal boolRechCV As Boolean = False) As String
        Utilitarios.bLoadInvVehiEvents = True

        m_SBO_Application.MessageBox(Message, 2, My.Resources.Resource.Si, My.Resources.Resource.No)
        ShowInput = sInput
    End Function

    Public Overridable Sub ItemEvents(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Select Case pVal.EventType
                Case BoEventTypes.et_ITEM_PRESSED
                    e_ItemPressed(pVal, BubbleEvent)
                Case BoEventTypes.et_FORM_LOAD
                    e_FormLoad(pVal, BubbleEvent)
            End Select

        Catch ex As Exception
            m_SBO_Application.MessageBox(ex.Message)
        End Try

    End Sub

    Protected Overridable Sub e_FormLoad(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.BeforeAction = False Then
            Dim query As String
            Dim strComentarios As SAPbouiCOM.StaticText
            Dim cboCliente As SAPbouiCOM.ComboBox
            Dim oform As SAPbouiCOM.Form = m_SBO_Application.Forms.Item(pVal.FormUID)

            oform.Items.Add("eInputST", SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oform.Items.Item("eInputST").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height
            oform.Items.Item("eInputST").Left = oform.Items.Item("7").Left
            oform.Items.Item("eInputST").Width = 100

            oform.Items.Add("eInputCB", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oform.Items.Item("eInputCB").Top = oform.Items.Item("7").Top
            oform.Items.Item("eInputCB").Left = oform.Items.Item("7").Left + 110
            oform.Items.Item("eInputCB").Width = 150
            oform.Items.Item("eInputCB").DisplayDesc = True

            oform.Items.Add("eInput", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oform.Items.Item("eInput").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height + 20
            oform.Items.Item("eInput").Left = oform.Items.Item("7").Left
            oform.Items.Item("eInput").Width = 290

            Dim oEditV As SAPbouiCOM.EditText
            oEditV = DirectCast(FormIV.Items.Item("strVal").Specific, SAPbouiCOM.EditText)
            oEditV.Value = "False"

            strComentarios = DirectCast(oform.Items.Item("eInputST").Specific, SAPbouiCOM.StaticText)
            cboCliente = DirectCast(oform.Items.Item("eInputCB").Specific, SAPbouiCOM.ComboBox)
            query = "select SlpCode, SlpName Name from OSLP where SlpCode <> '-1'"

            Utilitarios.CargarValidValuesEnCombos(cboCliente.ValidValues, query)

            strComentarios.Caption = My.Resources.Resource.TXTObservaciones

            oform = Nothing
        End If
    End Sub

    Protected Overridable Sub e_ItemPressed(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oEditV As SAPbouiCOM.EditText
        oEditV = DirectCast(FormIV.Items.Item("strVal").Specific, SAPbouiCOM.EditText)
        If pVal.ItemUID = "1" And pVal.BeforeAction Then
            Dim oEditComentariosIV As SAPbouiCOM.EditText
            Dim oEditClienteIV As SAPbouiCOM.EditText
            Dim cboCliente As SAPbouiCOM.ComboBox

            oEditComentariosIV = DirectCast(FormIV.Items.Item("txtH_RC").Specific, SAPbouiCOM.EditText)
            oEditComentariosIV.Value = String.Empty

            oEditClienteIV = DirectCast(FormIV.Items.Item("txtH_Cli").Specific, SAPbouiCOM.EditText)
            oEditClienteIV.Value = String.Empty

            sInput = m_SBO_Application.Forms.Item(pVal.FormUID).Items.Item("eInput").Specific.String
            cboCliente = DirectCast(m_SBO_Application.Forms.Item(pVal.FormUID).Items.Item("eInputCB").Specific, SAPbouiCOM.ComboBox)

            oEditComentariosIV.Value = sInput
            oEditClienteIV.Value = cboCliente.Selected.Description
            oEditV.Value = "True"
            Utilitarios.bLoadInvVehiEvents = False
        Else

            If pVal.ItemUID = "2" AndAlso pVal.BeforeAction Then
                oEditV.Value = "False"
            End If



        End If
    End Sub

End Class
