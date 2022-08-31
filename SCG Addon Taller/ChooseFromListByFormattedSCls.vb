Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Public Class ChooseFromListByFormattedSCls

#Region "Enums"

#End Region

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private Const mc_strCBOMarca As String = "cboMarca"
    Private Const mc_strCBOModelo As String = "cboModelo"
    Private Const mc_strCBOEst As String = "cboEst"

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Public Sub ManejadorEventoItemPressedCFLbyFS(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oForm As SAPbouiCOM.Form
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            Dim intCount As Integer
            Dim strCodigoMarca As String = Nothing
            Dim strCodigoEstilo As String = Nothing

            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                AndAlso pVal.ItemUID = 1 Or (pVal.ItemUID = 4 AndAlso pVal.Row >= 1) Then

                oMatrix = DirectCast(oForm.Items.Item("4").Specific, SAPbouiCOM.Matrix)

                If Not oMatrix Is Nothing Then

                    For intCount = 1 To oMatrix.RowCount - 1
                        If oMatrix.IsRowSelected(intCount) Then

                            strCodigoEstilo = oMatrix.Columns.Item(3).Cells.Item(intCount).Specific.value

                            ObtenerMarcaDelEstilo(strCodigoMarca, strCodigoEstilo)

                            oForm = SBO_Application.Forms.GetForm("SCGD_DET_1", 0)

                            Utilitarios.blnAutoMarcaEstiloModelo = True
                            oItem = oForm.Items.Item(mc_strCBOMarca)
                            oCombo = oItem.Specific
                            oCombo.Select(strCodigoMarca)

                            Utilitarios.blnAutoMarcaEstiloModelo = True
                            oItem = oForm.Items.Item(mc_strCBOEst)
                            oCombo = oItem.Specific
                            oCombo.Select(strCodigoEstilo)

                        End If
                    Next

                End If

            ElseIf Not oForm Is Nothing _
                    AndAlso (pVal.ItemUID = 2) Then

                Call oForm.Close()

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.ToString, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub ObtenerMarcaEstiloDelModelo(ByRef p_strMarca As String, ByRef p_strEstilo As String, ByVal p_strModelo As String)

        p_strEstilo = Utilitarios.EjecutarConsulta("SELECT U_Cod_Esti FROM [@SCGD_MODELO] WHERE Code=" & p_strModelo, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

        p_strMarca = Utilitarios.EjecutarConsulta("SELECT U_Cod_Marc FROM [@SCGD_ESTILO] WHERE Code=" & p_strEstilo, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

    End Sub

    Private Sub ObtenerMarcaDelEstilo(ByRef p_strMarca As String, ByRef p_strEstilo As String)

        p_strMarca = Utilitarios.EjecutarConsulta("SELECT U_Cod_Marc FROM [@SCGD_ESTILO] WHERE Code=" & p_strEstilo, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)

    End Sub

#End Region


End Class
