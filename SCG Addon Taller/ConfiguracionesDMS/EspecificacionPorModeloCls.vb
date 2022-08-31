Option Explicit On

Imports System.Globalization
Imports System.IO
Imports DMSOneFramework.CitasTableAdapters
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Partial Public Class EspecificacionPorModeloCls

    Public dtListaAccesorios As SAPbouiCOM.DataTable
    Public MatrizAcc As EspecificacionesMatrizAccesorios

    Public dtAccVehiculo As SAPbouiCOM.DataTable
    Public MatrizAccVehi As EspecificacionesMatrizAccesoriosVeh


    Public Sub CargarFormulario()

        blnCambio = False
        Dim strRes = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim

        If strRes = "E" Then
            m_blnUsaModelo = False
        ElseIf strRes = "M" Then
            m_blnUsaModelo = True
        End If

        dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal")

        FormularioSBO.EnableMenu("1282", False)
        FormularioSBO.EnableMenu("1281", False)
        FormularioSBO.EnableMenu("4870", False)
        FormularioSBO.EnableMenu("772", False)

        If m_blnUsaModelo = False Then
            FormularioSBO.Items.Item("cboModelo").Enabled = False
        End If

        dtListaAccesorios = FormularioSBO.DataSources.DataTables.Add("listaAcc")

        dtListaAccesorios.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAccesorios.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizAcc = New EspecificacionesMatrizAccesorios("mtxListAcc", FormularioSBO, "listaAcc")
        MatrizAcc.CreaColumnas()
        MatrizAcc.LigaColumnas()

        dtListaAccesorios.ExecuteQuery("SELECT ""ItemCode"", ""ItemName"" FROM ""OITM"" WHERE ""U_SCGD_TipoArticulo"" = 7")
        MatrizAcc.Matrix.LoadFromDataSource()

        '-----------------------------------------------------------------------
        dtAccVehiculo = FormularioSBO.DataSources.DataTables.Add("listaAccVehi")

        dtAccVehiculo.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
        dtAccVehiculo.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizAccVehi = New EspecificacionesMatrizAccesoriosVeh("mtxAccVeh", FormularioSBO, "listaAccVehi")
        MatrizAccVehi.CreaColumnas()
        MatrizAccVehi.LigaColumnas()

        ' AddChooseFromList(FormularioSBO, "4", "CFL_Itm")
        ' AgregaCFLItems()


        Call AgregaButtonPic(FormularioSBO, "btnArtVent", 278, 69, 0, 0, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "")


    End Sub

    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, _
                              ByVal strNombrectrl As String, _
                              ByVal intLeft As Integer, _
                              ByVal intTop As Integer, _
                              ByVal intFromPane As Integer, _
                              ByVal intTopane As Integer, _
                              ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
                              ByVal PathImagen As String, _
                              ByVal UDO As String) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 20
            oitem.Height = 20
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function



    Private Function ValidarExisteAccesorio(ByVal p_strCodigoAcc As String) As Boolean
        Dim result As Boolean = True

        Try
            If Not String.IsNullOrEmpty(p_strCodigoAcc) Then
                For i As Integer = 0 To dtAccVehiculo.Rows.Count - 1
                    If p_strCodigoAcc = dtAccVehiculo.GetValue("code", i) Then
                        result = False
                        Exit For
                    End If
                Next

            End If
            Return result
        Catch ex As Exception

        End Try
    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub AgregaCFLItems()

        Dim oItem As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText

        oItem = FormularioSBO.Items.Item(EditTextCodItmInv.UniqueId)
        oEdit = oItem.Specific

        FormularioSBO.DataSources.UserDataSources.Add("NumDocum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT)

        oEdit.DataBind.SetBound(True, "", "NumDocum")
        oEdit.ChooseFromListUID = "CFL_Itm"
        oEdit.ChooseFromListAlias = "ItemCode"

    End Sub
End Class
