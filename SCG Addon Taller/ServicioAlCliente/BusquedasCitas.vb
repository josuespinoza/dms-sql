Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions

Partial Public Class BusquedasCitas

    Public Sub ButtonBuscarItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction Then

        End If

        If pVal.ActionSuccess Then

            Dim strConsultaSELECT As String = "SELECT C.DocEntry, " +
                                                    "Q.U_SCGD_NoSerieCita + ' - ' + Q.U_SCGD_NoCita as NoCita, " +
                                                    "C.U_FechaCita, " +
                                                    "C.U_HoraCita, " +
                                                    "Q.DocEntry, " +
                                                    "Q.U_SCGD_Numero_OT, " +
                                                    "T.Name, " +
                                                    "S.Name as Sucursal, " +
                                                    "Q.U_SCGD_Cod_Unidad, " +
                                                    "Q.U_SCGD_Num_Placa, " +
                                                    "CE.U_Descripcion as Confirmacion, " +
                                                    "Q.U_SCGD_Gorro_Veh, " +
                                                    "Q.U_SCGD_No_Visita, " +
                                                    "Q.CardCode, " +
                                                    "Q.CardName, " +
                                                    "Q.U_SCGD_Des_Marc, " +
                                                    "Q.U_SCGD_Des_Esti, " +
                                                    "Q.U_SCGD_Des_Mode, " +
                                                    "C.U_Name_Tecnico, " +
                                                    "C.U_Name_Asesor " +
                                                    "FROM [@SCGD_CITA]  AS C with(nolock)" +
                                                    "LEFT OUTER JOIN [OQUT]  AS Q with(nolock) ON Q.DocEntry = C.U_Num_Cot " +
                                                    "LEFT OUTER JOIN [@SCGD_SUCURSALES] AS S with(nolock) ON C.U_Cod_Sucursal = S.Code " +
                                                    "LEFT OUTER JOIN [@SCGD_TIPO_ORDEN] AS T with(nolock) ON Q.U_SCGD_Tipo_OT = T.Code " +
                                                    "LEFT OUTER JOIN [@SCGD_CITA_ESTADOS] as CE with(nolock) ON C.U_Estado = CE.Code "


            Dim strWhere As String = "Where C.DocEntry is not null and Q.U_SCGD_NoCita is not null and Q.DocEntry is not null "
            Dim strOrder As String = " Order BY C.DocEntry "

            Dim strNoCitaAb As String = EditTextNoCitaAb.ObtieneValorUserDataSource.Trim
            Dim strNoCita As String = EditTextNoCita.ObtieneValorUserDataSource.Trim
            Dim strNoUnidad As String = EditTextNoUnidad.ObtieneValorUserDataSource.Trim
            Dim strPlaca As String = EditTextPlaca.ObtieneValorUserDataSource.Trim
            Dim strNoOt As String = EditTextNoOt.ObtieneValorUserDataSource.Trim
            Dim strNoCono As String = EditTextNoCono.ObtieneValorUserDataSource.Trim
            Dim strNoVisita As String = EditTextNoVisita.ObtieneValorUserDataSource.Trim
            Dim strConfirmacion As String = ComboBoxConfirmacion.ObtieneValorUserDataSource.Trim
            Dim strMarca As String = ComboBoxMarca.ObtieneValorUserDataSource.Trim
            Dim strEstilo As String = ComboBoxEstilo.ObtieneValorUserDataSource.Trim
            Dim strModelo As String = ComboBoxModelo.ObtieneValorUserDataSource.Trim
            Dim strMecanico As String = ComboBoxMecanico.ObtieneValorUserDataSource.Trim
            Dim strSucursal As String = ComboBoxSucursal.ObtieneValorUserDataSource.Trim
            Dim strDesde As String = EditTextDesde.ObtieneValorUserDataSource().Trim
            Dim strHasta As String = EditTextHasta.ObtieneValorUserDataSource().Trim
            Dim strDiasPrev As String = EditTextDiasPrev.ObtieneValorUserDataSource().Trim
            Dim strCodAsesor As String = EditTextCodAsesor.ObtieneValorUserDataSource().Trim
            Dim strCodAgenda As String = ComboBoxAgenda.ObtieneValorUserDataSource().Trim
            Dim strCheckAge As String = CheckBoxUAge.ObtieneValorUserDataSource().Trim()

            Try

                If Not String.IsNullOrEmpty(strNoCitaAb) Then strWhere += String.Format(" AND Q.U_SCGD_NoSerieCita LIKE '%{0}%'", strNoCitaAb)
                If Not String.IsNullOrEmpty(strNoCita) Then strWhere += String.Format(" AND Q.U_SCGD_NoCita LIKE '{0}%'", strNoCita)
                If Not String.IsNullOrEmpty(strNoUnidad) Then strWhere += String.Format(" AND Q.U_SCGD_Cod_Unidad LIKE '{0}%'", strNoUnidad)
                If Not String.IsNullOrEmpty(strPlaca) Then strWhere += String.Format(" AND Q.U_SCGD_Num_Placa LIKE '{0}%'", strPlaca)
                If Not String.IsNullOrEmpty(strNoOt) Then strWhere += String.Format(" AND Q.U_SCGD_Numero_OT LIKE '{0}%'", strNoOt)
                If Not String.IsNullOrEmpty(strNoCono) Then strWhere += String.Format(" AND Q.U_SCGD_Gorro_Veh LIKE '{0}%'", strNoCono)
                If Not String.IsNullOrEmpty(strNoVisita) Then strWhere += String.Format(" AND Q.U_SCGD_No_Visita LIKE '{0}%'", strNoVisita)
                If Not String.IsNullOrEmpty(strConfirmacion) Then strWhere += String.Format(" AND C.U_Estado = '{0}'", strConfirmacion)

                If Not String.IsNullOrEmpty(strMarca) Then strWhere += String.Format(" AND Q.U_SCGD_Cod_Marca LIKE '{0}%'", strMarca)
                If Not String.IsNullOrEmpty(strEstilo) Then strWhere += String.Format(" AND Q.U_SCGD_Cod_Estilo LIKE '{0}%'", strEstilo)
                If Not String.IsNullOrEmpty(strModelo) Then strWhere += String.Format(" AND Q.U_SCGD_Cod_Modelo LIKE '{0}%'", strModelo)
                If Not String.IsNullOrEmpty(strMecanico) Then strWhere += String.Format(" AND OwnerCode LIKE '{0}%'", strMecanico)
                If Not String.IsNullOrEmpty(strSucursal) Then strWhere += String.Format(" AND C.U_Cod_Sucursal = '{0}'", strSucursal)
                If Not String.IsNullOrEmpty(strCodAsesor) Then strWhere += String.Format(" AND C.U_Cod_Asesor = '{0}'", strCodAsesor)
                If Not String.IsNullOrEmpty(strCodAgenda) AndAlso strCheckAge = "Y" Then
                    strWhere += String.Format(" AND C.U_Cod_Agenda = '{0}'", strCodAgenda)
                End If
                If Not String.IsNullOrEmpty(strDesde) Then
                    'strDesde = Date.ParseExact(strDesde, "yyyyMMdd", Nothing)

                    If Not String.IsNullOrEmpty(strDiasPrev) AndAlso Not strDiasPrev.Equals("0") Then
                        Dim intDiasPrev As Integer = Integer.Parse(strDiasPrev)
                        Dim dateDiasPrev As Date = Date.ParseExact(strDesde, "yyyyMMdd", Nothing)

                        intDiasPrev = intDiasPrev * -1
                        dateDiasPrev = dateDiasPrev.AddDays(intDiasPrev)
                        'strDiasPrev = Utilitarios.RetornaFechaFormatoDB(dateDiasPrev, CompanySBO.Server)
                        strDiasPrev = dateDiasPrev.ToString("yyyyMMdd")
                        strWhere += String.Format(" AND C.U_FechaCita = '{0}'", strDiasPrev)
                    Else
                        'strDesde = Utilitarios.RetornaFechaFormatoDB(strDesde, CompanySBO.Server)
                        strWhere += String.Format(" AND C.U_FechaCita >= '{0}'", strDesde)
                    End If

                End If

                If String.IsNullOrEmpty(strDesde) AndAlso (Not String.IsNullOrEmpty(strDiasPrev) AndAlso Not strDiasPrev.Equals("0")) Then

                    Dim intDiasPrev As Integer = Integer.Parse(strDiasPrev)
                    Dim dateDiasPrev As Date = Date.Today

                    dateDiasPrev = dateDiasPrev.AddDays(intDiasPrev)
                    'strDiasPrev = Utilitarios.RetornaFechaFormatoDB(dateDiasPrev, CompanySBO.Server)
                    strDiasPrev = dateDiasPrev.ToString("yyyyMMdd")
                    strWhere += String.Format(" AND C.U_FechaCita = '{0}'", strDiasPrev)

                End If

                If Not String.IsNullOrEmpty(strHasta) Then
                    'strHasta = Utilitarios.RetornaFechaFormatoDB(Date.ParseExact(strHasta, "yyyyMMdd", Nothing), CompanySBO.Server)
                    strWhere += String.Format(" AND C.U_FechaCita <= '{0}'", strHasta)
                End If

                CargaMatriz(strConsultaSELECT + strWhere + strOrder, DataTableBusqueda, MatrixBusqueda)

            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub CheckBoxConfirmacionItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxConfirmacion.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(ComboBoxConfirmacion.UniqueId, True)
            ElseIf CheckBoxConfirmacion.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxConfirmacion.UniqueId, False)
                ComboBoxConfirmacion.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxMarcaItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxMarca.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(ComboBoxMarca.UniqueId, True)
            ElseIf CheckBoxMarca.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxMarca.UniqueId, False)
                ComboBoxMarca.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxEstiloItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess Then
            If CheckBoxEstilo.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                If Not String.IsNullOrEmpty(ComboBoxMarca.ObtieneValorUserDataSource.Trim) Then
                    Call CargarValidValuesEnCombos(FormularioSBO, String.Format("SELECT Code, Name FROM [@SCGD_ESTILO] with(nolock) WHERE U_Cod_Marc = '{0}'  ORDER BY Code ASC", ComboBoxMarca.ObtieneValorUserDataSource.Trim), "cboEsti")
                End If

                ManejoComponente(ComboBoxEstilo.UniqueId, True)

            ElseIf CheckBoxEstilo.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxEstilo.UniqueId, False)
                ComboBoxEstilo.AsignaValorUserDataSource("")
            End If
        End If
    End Sub

    Public Sub CheckBoxAgendaItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess Then
            If CheckBoxUAge.ObtieneValorUserDataSource.Trim.Equals("Y") Then

                Call CargarValidValuesEnCombos(FormularioSBO, "select DocEntry as Code, U_Agenda as Name from [@SCGD_AGENDA] with (nolock) ORDER BY DocEntry ASC", "cboAgen")

                ManejoComponente(ComboBoxAgenda.UniqueId, True)

            ElseIf CheckBoxUAge.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxAgenda.UniqueId, False)
                ComboBoxAgenda.AsignaValorUserDataSource("")
            End If
        End If
    End Sub

    Public Sub CheckBoxModeloItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxModelo.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                If Not String.IsNullOrEmpty(ComboBoxEstilo.ObtieneValorUserDataSource.Trim) Then
                    Call CargarValidValuesEnCombos(FormularioSBO, String.Format("SELECT Code, Name FROM [@SCGD_MODELO] with(nolock) WHERE [U_Cod_Esti] = '{0}'  ORDER BY Code ASC", ComboBoxEstilo.ObtieneValorUserDataSource.Trim), "cboMod")
                End If

                ManejoComponente(ComboBoxModelo.UniqueId, True)

            ElseIf CheckBoxModelo.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxModelo.UniqueId, False)
                ComboBoxModelo.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxMecanicoItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxMecanico.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(ComboBoxMecanico.UniqueId, True)
            ElseIf CheckBoxMecanico.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxMecanico.UniqueId, False)
                ComboBoxMecanico.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxSucursalItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxSucursal.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(ComboBoxSucursal.UniqueId, True)
            ElseIf CheckBoxSucursal.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(ComboBoxSucursal.UniqueId, False)
                ComboBoxSucursal.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxDesdeItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxDesde.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(EditTextDesde.UniqueId, True)
            ElseIf CheckBoxDesde.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(EditTextDesde.UniqueId, False)
                EditTextDesde.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxHastaItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxHasta.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(EditTextHasta.UniqueId, True)
                ManejoComponente(EditTextDiasPrev.UniqueId, False)
                EditTextDiasPrev.AsignaValorUserDataSource("0")
                CheckBoxDiasPrev.AsignaValorUserDataSource("N")
            ElseIf CheckBoxHasta.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(EditTextHasta.UniqueId, False)
                EditTextHasta.AsignaValorUserDataSource("")
            End If

        End If
    End Sub

    Public Sub CheckBoxDiasPrevItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            If CheckBoxDiasPrev.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                ManejoComponente(EditTextDiasPrev.UniqueId, True)
                ManejoComponente(EditTextHasta.UniqueId, False)
                EditTextHasta.AsignaValorUserDataSource("")
                CheckBoxHasta.AsignaValorUserDataSource("N")
            ElseIf CheckBoxDiasPrev.ObtieneValorUserDataSource.Trim.Equals("N") Then
                ManejoComponente(EditTextDiasPrev.UniqueId, False)
                EditTextDiasPrev.AsignaValorUserDataSource("0")
            End If

        End If
    End Sub

    Public Sub SetActiveItem(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

            FormularioSBO.ActiveItem = EditTextNoCitaAb.UniqueId

        End If

    End Sub

    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                            ByVal strQuery As String, _
                                                            ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            'Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, strConectionString)
            'cn_Coneccion.ConnectionString = strConectionString

            'cn_Coneccion.Open()
            'cmdEjecutarConsulta.Connection = cn_Coneccion
            'cmdEjecutarConsulta.CommandType = CommandType.Text
            'cmdEjecutarConsulta.CommandText = strQuery
            'drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, strQuery)
            ' ''Agrega los ValidValues
            'Do While drdResultadoConsulta.Read
            '    If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
            '        cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
            '    End If
            'Loop

            'drdResultadoConsulta.Close()
            'cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

    Private Sub ManejoComponente(ByVal strComponente As String, ByVal Valor As Boolean)
        If Valor = False Then
            Dim oItem As SAPbouiCOM.Item
            oItem = FormularioSBO.Items.Item(EditTextNoCitaAb.UniqueId)
            oItem.Click(BoCellClickType.ct_Regular)
        End If

        FormularioSBO.Items.Item(strComponente).Enabled = Valor
    End Sub

    Private Sub CargaMatriz(ByVal Consulta As String, ByVal dataTable As SAPbouiCOM.DataTable, ByVal matrix As MatrixSBO)
        Try

            dataTable.ExecuteQuery(Consulta)
            matrix.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

End Class
