Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports SAPbouiCOM

Partial Public Class AgendasConfiguracion
    Implements IUsaPermisos

#Region "Variables"

    Private m_oCompany As SAPbobsCOM.Company

    Public dtListaAgendas As DataTable
    Public MatrizAgendas As AgendasConfiguracionMatriz

#End Region


#Region "Metodos"
   

    Public Function ValidarCampos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim l_numDocNum As String
        Dim l_strNombreAgenda As String
        Dim l_strIntervalo As String

        Dim l_strAbrev As String = ""

        Dim l_strSQLAgendas As String

        l_strSQLAgendas = "SELECT TOP 1 ""U_Abreviatura"" FROM ""@SCGD_AGENDA"" WHERE ""U_Abreviatura"" = '{0}'"


        Try
            
            l_strAbrev = EditTextAbreviatura.ObtieneValorDataSource()
            l_numDocNum = EditTextAgenda.ObtieneValorDataSource()
            l_strNombreAgenda = EditTextAgendaNomb.ObtieneValorDataSource()
            l_strIntervalo = EditTextIntervalo.ObtieneValorDataSource()

            If String.IsNullOrEmpty(l_strNombreAgenda) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinNombre, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            ElseIf String.IsNullOrEmpty(l_strAbrev) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinAbrev, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            ElseIf l_strAbrev.Length <> 3 Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaAbrevLargo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            ElseIf String.IsNullOrEmpty(l_strIntervalo) Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaSinInterv, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            End If

            If pVal.FormMode = BoFormMode.fm_ADD_MODE Then
                l_strSQLAgendas = String.Format(l_strSQLAgendas, l_strAbrev)
                If Not String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(l_strSQLAgendas)) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaYaExiste, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                End If
            ElseIf pVal.FormMode = BoFormMode.fm_UPDATE_MODE Then
                l_strSQLAgendas = String.Format(l_strSQLAgendas, l_strAbrev) & String.Format(" and ""U_Abreviatura"" <> '{0}' ", m_strAbrevAnt)
                If Not String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(l_strSQLAgendas)) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorAgendaYaExiste, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                End If
            End If

            Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

End Class
