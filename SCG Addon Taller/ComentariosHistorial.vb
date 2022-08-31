Imports SAPbouiCOM
Imports System
Imports System.Collections.Generic

Partial Public Class ComentariosHistorial

    Dim oMatrixTmp As SAPbouiCOM.Matrix
    Dim otmpForm As SAPbouiCOM.Form

    '''''''''''''''''''''''''''''''''''''''''''''''''
    Private sInput As String = ""
    Private sInputST As String = ""
    Private sTitle As String = ""
    Dim oComboRazonRechazo As SAPbouiCOM.ComboBox
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



    Public Function ShowInput(ByVal Title As String, ByVal Message As String, Optional ByVal boolRechCV As Boolean = False) As String
        Utilitarios.bLoadInputEvents = True
        Titulo = Title
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
            Dim strRazon As SAPbouiCOM.StaticText
            Dim strComentarios As SAPbouiCOM.StaticText
            Dim oform As SAPbouiCOM.Form = m_SBO_Application.Forms.Item(pVal.FormUID)

            oform.Items.Add("eInputST", SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oform.Items.Item("eInputST").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height
            oform.Items.Item("eInputST").Left = oform.Items.Item("7").Left
            oform.Items.Item("eInputST").Width = 290

            oform.Items.Add("cboRazon", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oform.Items.Item("cboRazon").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height 
            oform.Items.Item("cboRazon").Left = oform.Items.Item("7").Left + 105
            oform.Items.Item("cboRazon").Width = 290
            oform.Items.Item("cboRazon").DisplayDesc = True

            oform.Items.Add("eInputC", SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oform.Items.Item("eInputC").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height + 20
            oform.Items.Item("eInputC").Left = oform.Items.Item("7").Left
            oform.Items.Item("eInputC").Width = 110

            oform.Items.Add("eInput", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oform.Items.Item("eInput").Top = oform.Items.Item("7").Top + oform.Items.Item("7").Height + 20
            oform.Items.Item("eInput").Left = oform.Items.Item("7").Left + 105
            oform.Items.Item("eInput").Width = 290
            oform.Items.Item("eInput").Height += 16

            CargarComboTipoVehiculo(oform)
            strRazon = DirectCast(oform.Items.Item("eInputST").Specific, SAPbouiCOM.StaticText)
            strRazon.Caption = My.Resources.Resource.RazonRechazo

            strComentarios= DirectCast(oform.Items.Item("eInputC").Specific, SAPbouiCOM.StaticText)
            strComentarios.Caption = My.Resources.Resource.TXTcomentarios

            oform = Nothing
        End If
    End Sub

    Protected Overridable Sub e_ItemPressed(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ItemUID = "1" And pVal.BeforeAction Then
            Dim cboRazonRechazo As SAPbouiCOM.ComboBox
            Dim strRazonRechazo As String = String.Empty
            Dim oEditComentariosCV As SAPbouiCOM.EditText
            oEditComentariosCV = DirectCast(_formCV.Items.Item("txtH_RC").Specific, SAPbouiCOM.EditText)

            sInput = m_SBO_Application.Forms.Item(pVal.FormUID).Items.Item("eInput").Specific.String

            cboRazonRechazo = DirectCast(m_SBO_Application.Forms.Item(pVal.FormUID).Items.Item("cboRazon").Specific, SAPbouiCOM.ComboBox)
            If Not cboRazonRechazo.Selected Is Nothing Then
                strRazonRechazo = cboRazonRechazo.Selected.Description
            Else
                strRazonRechazo = String.Empty
            End If

            If String.IsNullOrEmpty(strRazonRechazo) Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.SeleccionarRazon, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
            Else
                If Not String.IsNullOrEmpty(sInput) Then
                    strRazonRechazo = strRazonRechazo & " - " & sInput
                    If strRazonRechazo.Length > 254 Then
                        strRazonRechazo = strRazonRechazo.Substring(0, 254)
                    End If
                End If

                oEditComentariosCV.Value = oEditComentariosCV.Value & strRazonRechazo
                Utilitarios.bLoadInputEvents = False
            End If

        End If
    End Sub

    Private Function CargarComboTipoVehiculo(ByRef p_oForm As SAPbouiCOM.Form)
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Try
            Dim lstValidValues As List(Of Utilitarios.ListadoValidValues) = New List(Of Utilitarios.ListadoValidValues)()
            oItem = p_oForm.Items.Item("cboRazon")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            lstValidValues = Utilitarios.GetListadoValidValues(String.Format("select ""Code"", ""Name"" FROM ""@SCGD_RRECHAZO"" ORDER BY ""Code"""))
            Utilitarios.CargarValidValuesEnCombosVehiculo(oCombo.ValidValues, lstValidValues)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function



End Class
