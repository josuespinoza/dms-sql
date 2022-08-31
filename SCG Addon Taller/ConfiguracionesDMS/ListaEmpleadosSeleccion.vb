Option Explicit On

Imports System.Globalization
Imports System.IO
Imports System.Collections.Generic
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Partial Public Class ListaEmpleadosSeleccion : Implements IFormularioSBO

#Region "Metodos"

    Public Function CargarMatriz(Optional ByVal query As String = "") As Boolean

        Dim strConsulta As String = ""
        Dim oMatrizMSJ As SAPbouiCOM.Matrix
        Dim dtConfigUsrRol As SAPbouiCOM.DataTable
        Dim userIds As List(Of String)
        Dim filtro As String
        FormConfMSJ = _applicationSBO.Forms.Item(strFormMsjUId)

        If Not String.IsNullOrEmpty(query) Then
            strConsulta = query
            filtro = " and u.USERID not in ({0}) "
        Else
            strConsulta = " SELECT u.USERID, h.empID, u.U_NAME, u.USER_CODE FROM OUSR u with (nolock)" & _
                          "right join OHEM h with (nolock) on u.USERID=h.userId {0} order by USER_CODE "
            filtro = " Where u.USERID not in ({0}) "
        End If

        oMatrizMSJ = DirectCast(FormConfMSJ.Items.Item("mtx_MSJ").Specific, SAPbouiCOM.Matrix)
        dtConfigUsrRol = FormConfMSJ.DataSources.DataTables.Item(g_strdtConfRol)

        If oMatrizMSJ.RowCount > 0 Then
            userIds = New List(Of String)
            oMatrizMSJ.FlushToDataSource()
            For i As Integer = 0 To dtConfigUsrRol.Rows.Count - 1
                userIds.Add(dtConfigUsrRol.GetValue("UsrID", i))
            Next

            Dim filters As String = String.Empty
            For Each usrId As String In userIds
                If Not String.IsNullOrEmpty(usrId) Then
                    filters = String.Format(" {0} '{1}',", filters, usrId)
                End If
            Next
            filtro = String.Format(filtro, If(String.IsNullOrEmpty(filters), "''", filters.Substring(0, filters.Length - 1)))
            strConsulta = String.Format(strConsulta, filtro)

        Else
            strConsulta = String.Format(strConsulta, String.Empty)
        End If
        If FormularioSBO Is Nothing Then
            oForm = _applicationSBO.Forms.Item(FormUID)
        Else
            oForm = FormularioSBO
        End If

        oMatrix = DirectCast(oForm.Items.Item("mtx_User").Specific, SAPbouiCOM.Matrix)

        Try
            oMatrix.Clear()
            dtUsuariosConsulta.Rows.Clear()
            dtSelEmpMatriz.Rows.Clear()
            If Not String.IsNullOrEmpty(strConsulta) Then
                dtUsuariosConsulta.ExecuteQuery(strConsulta)
            End If

            CargaUsuariosEnDt()

            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSBO)
            Return False
        End Try

    End Function

    Private Sub CargaUsuariosEnDt()
        For i As Integer = 0 To dtUsuariosConsulta.Rows.Count - 1
            dtSelEmpMatriz.Rows.Add()
            dtSelEmpMatriz.SetValue("Col_Name", i, dtUsuariosConsulta.GetValue("U_NAME", i))
            dtSelEmpMatriz.SetValue("Col_UCode", i, dtUsuariosConsulta.GetValue("USERID", i))
            dtSelEmpMatriz.SetValue("Col_UN", i, dtUsuariosConsulta.GetValue("USER_CODE", i))
            dtSelEmpMatriz.SetValue("Col_EmId", i, dtUsuariosConsulta.GetValue("empID", i))
        Next
    End Sub

    'inserta usuarios por nivel y sucursal
    Public Sub InsertaNivXUsuarios(ByRef pval As SAPbouiCOM.ItemEvent, ByVal str_Usuarios As List(Of String), ByVal str_Names As List(Of String),
                                   ByVal str_EmpIDs As List(Of String), ByVal str_CodNivAprob As String,
                                   ByVal oMatrizMSJ As SAPbouiCOM.Matrix,
                                   ByVal p_dtConfigLineas As SAPbouiCOM.DataTable)

        Dim EditValue As SAPbouiCOM.EditText
        Dim UltimoLineID As Integer = 0
        Dim Posicion As Integer = 0

        If oMatrizMSJ.RowCount > 0 Then
            EditValue = DirectCast(oMatrizMSJ.GetCellSpecific("Col_LineId", oMatrizMSJ.RowCount), SAPbouiCOM.EditText)
            UltimoLineID = Integer.Parse(EditValue.Value.ToString().Trim())
        Else
            UltimoLineID = 0
        End If

        Posicion = p_dtConfigLineas.Rows.Count

        For x As Integer = 0 To str_Usuarios.Count - 1
            p_dtConfigLineas.Rows.Add(1)
            p_dtConfigLineas.SetValue("name", Posicion, str_Names(x))
            p_dtConfigLineas.SetValue("usua", Posicion, str_Usuarios(x))
            p_dtConfigLineas.SetValue("code", Posicion, str_EmpIDs(x))

            Posicion = Posicion + 1
        Next

        oMatrizMSJ.LoadFromDataSource()
    End Sub

    ''' <summary>
    ''' Carga el id de rol y id de sucursal en el Formulario
    ''' </summary>
    Public Sub CargaSucRol(ByRef pval As SAPbouiCOM.ItemEvent, ByVal strIdRol As String, ByVal strIdSuc As String, ByVal strDE As String)
        If (g_oEditIDRol.Value = "") Then
            g_oEditIDRol.Value = strIdRol
        End If
        If (g_oEditIDSuc.Value = "") Then
            g_oEditIDSuc.Value = strIdSuc
        End If
        If (g_oEditDE.Value = "") Then
            g_oEditDE.Value = strDE
        End If
    End Sub

#End Region

#Region "Eventos"

    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                        ByRef BubbleEvent As Boolean,
                                        ByVal comp As SAPbobsCOM.Company)
        Try
            '***********          ACTION SUCCESS          ***********
            If pval.ActionSuccess = True And pval.BeforeAction = False Then
                Select Case pval.ItemUID
                    Case "btnSel"
                        oForm = _applicationSBO.Forms.Item(FormUID)

                        FormConfMSJ = _applicationSBO.Forms.Item(strFormMsjUId)

                        _companySbo = comp

                        Dim oMatrizMSJ As SAPbouiCOM.Matrix
                        Dim oMatrizUser As SAPbouiCOM.Matrix
                        Dim str_Usuarios As New List(Of String)
                        Dim str_EmpIDs As New List(Of String)
                        Dim str_Names As New List(Of String)
                        Dim newRowNumber As Integer
                        Dim dtConfigUsrRol As SAPbouiCOM.DataTable
                        Dim contador = 0

                        oMatrizMSJ = DirectCast(FormConfMSJ.Items.Item("mtx_MSJ").Specific, SAPbouiCOM.Matrix)
                        oMatrizUser = DirectCast(oForm.Items.Item("mtx_User").Specific, SAPbouiCOM.Matrix)
                        oMatrizUser.FlushToDataSource()
                        dtConfigUsrRol = FormConfMSJ.DataSources.DataTables.Item(g_strdtConfRol)
                        oMatrizMSJ.FlushToDataSource()

                        'se recorre la matriz de usuarios para obtener los seleccionados
                        For i As Integer = 1 To oMatrizUser.RowCount
                            If oMatrizUser.IsRowSelected(i) Then
                                newRowNumber = dtConfigUsrRol.Rows.Count

                                If dtConfigUsrRol.Rows.Count = 1 Then
                                    dtConfigUsrRol.SetValue("UsrID", 0, oMatrizUser.Columns.Item("Col_UCode").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("Name", 0, oMatrizUser.Columns.Item("Col_Name").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("EmpId", 0, oMatrizUser.Columns.Item("Col_EmId").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("UserName", 0, oMatrizUser.Columns.Item("Col_UN").Cells.Item(i).Specific.Value)
                                    'dtConfigUsrRol.SetValue("RolId", 0, g_oEditIDRol)
                                    dtConfigUsrRol.Rows.Add(1)
                                Else
                                    If Not String.IsNullOrEmpty(dtConfigUsrRol.GetValue("UsrID", newRowNumber - 1)) Then
                                        newRowNumber = newRowNumber + 1
                                        dtConfigUsrRol.Rows.Add(1)
                                    End If
                                    dtConfigUsrRol.SetValue("UsrID", newRowNumber - 1, oMatrizUser.Columns.Item("Col_UCode").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("Name", newRowNumber - 1, oMatrizUser.Columns.Item("Col_Name").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("EmpId", newRowNumber - 1, oMatrizUser.Columns.Item("Col_EmId").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("UserName", newRowNumber - 1, oMatrizUser.Columns.Item("Col_UN").Cells.Item(i).Specific.Value)
                                    'dtConfigUsrRol.SetValue("RolId", newRowNumber - 1, g_oEditIDRol)
                                    dtConfigUsrRol.Rows.Add(1)
                                End If
                            Else
                                contador = contador + 1
                            End If
                        Next

                        If contador >= oMatrizUser.RowCount Then
                            _applicationSBO.StatusBar.SetText(My.Resources.Resource.ErrSelectUSerAdd, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                        Else
                            oMatrizMSJ.LoadFromDataSource()
                            oForm.Close()
                        End If

                    Case "btnBuscar"
                        Dim strUsuario As String
                        Dim strConsulta As String

                        'strConsulta = " SELECT u.USERID, h.empID, U_NAME FROM OUSR u with (nolock)" & _
                        '        "right join OHEM h with (nolock) on u.USERID=h.userId order by USER_CODE "

                        strConsulta = " SELECT u.USERID, h.empID, u.U_NAME, u.USER_CODE FROM OUSR u with (nolock) " & _
                                   "right join OHEM h with (nolock) on u.USERID=h.userId " +
                                   "WHERE u.U_NAME like '%{0}%' or u.USER_CODE like '%{0}%' {1} order by USER_CODE "
                        oForm = _applicationSBO.Forms.Item(FormUID)
                        strUsuario = DirectCast(oForm.Items.Item("txtUserC").Specific, SAPbouiCOM.EditText).Value
                        If Not String.IsNullOrEmpty(strUsuario) Then
                            strConsulta = String.Format(strConsulta, strUsuario, "{0}")
                        Else
                            strConsulta = String.Format(strConsulta, String.Empty, "{0}")
                        End If
                        Call CargarMatriz(strConsulta)
                End Select
            End If
        Catch ex As Exception
            'manejo de errores
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoDobleClick(ByRef pVal As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Try
            If pVal.Before_Action Then
                Select Case pVal.ItemUID
                    Case "btnSel", "mtx_User"
                        oForm = _applicationSBO.Forms.Item(FormUID)
                        FormConfMSJ = _applicationSBO.Forms.Item(strFormMsjUId)

                        Dim oMatrizMSJ As SAPbouiCOM.Matrix
                        Dim oMatrizUser As SAPbouiCOM.Matrix
                        Dim str_Usuarios As New List(Of String)
                        Dim str_EmpIDs As New List(Of String)
                        Dim str_Names As New List(Of String)
                        Dim newRowNumber As Integer
                        Dim dtConfigUsrRol As SAPbouiCOM.DataTable
                        Dim contador = 0

                        oMatrizMSJ = DirectCast(FormConfMSJ.Items.Item("mtx_MSJ").Specific, SAPbouiCOM.Matrix)
                        oMatrizUser = DirectCast(oForm.Items.Item("mtx_User").Specific, SAPbouiCOM.Matrix)
                        oMatrizUser.FlushToDataSource()
                        dtConfigUsrRol = FormConfMSJ.DataSources.DataTables.Item(g_strdtConfRol)
                        oMatrizMSJ.FlushToDataSource()

                        'se recorre la matriz de usuarios para obtener los seleccionados
                        For i As Integer = 1 To oMatrizUser.RowCount
                            If oMatrizUser.IsRowSelected(i) Then
                                newRowNumber = dtConfigUsrRol.Rows.Count

                                If dtConfigUsrRol.Rows.Count = 1 Then
                                    dtConfigUsrRol.SetValue("UsrID", 0, oMatrizUser.Columns.Item("Col_UCode").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("Name", 0, oMatrizUser.Columns.Item("Col_Name").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("EmpId", 0, oMatrizUser.Columns.Item("Col_EmId").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("UserName", 0, oMatrizUser.Columns.Item("Col_UN").Cells.Item(i).Specific.Value)
                                    'dtConfigUsrRol.SetValue("RolId", 0, g_oEditIDRol)
                                    dtConfigUsrRol.Rows.Add(1)
                                Else
                                    If Not String.IsNullOrEmpty(dtConfigUsrRol.GetValue("UsrID", newRowNumber - 1)) Then
                                        newRowNumber = newRowNumber + 1
                                        dtConfigUsrRol.Rows.Add(1)
                                    End If
                                    dtConfigUsrRol.SetValue("UsrID", newRowNumber - 1, oMatrizUser.Columns.Item("Col_UCode").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("Name", newRowNumber - 1, oMatrizUser.Columns.Item("Col_Name").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("EmpId", newRowNumber - 1, oMatrizUser.Columns.Item("Col_EmId").Cells.Item(i).Specific.Value)
                                    dtConfigUsrRol.SetValue("UserName", newRowNumber - 1, oMatrizUser.Columns.Item("Col_UN").Cells.Item(i).Specific.Value)
                                    'dtConfigUsrRol.SetValue("RolId", newRowNumber - 1, g_oEditIDRol)
                                    dtConfigUsrRol.Rows.Add(1)
                                End If
                            Else
                                contador = contador + 1
                            End If
                        Next

                        If contador >= oMatrizUser.RowCount Then
                            _applicationSBO.StatusBar.SetText(My.Resources.Resource.ErrSelectUSerAdd, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                        Else
                            oMatrizMSJ.LoadFromDataSource()
                            oForm.Close()
                        End If
                End Select
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try

    End Sub


#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
