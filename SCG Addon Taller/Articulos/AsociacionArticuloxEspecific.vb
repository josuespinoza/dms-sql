Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class AsociacionArticuloxEspecific : Implements IFormularioSBO, IUsaMenu

    Public Sub CFLArticulos(ByVal FormUID As String, ByRef pval As SAPbouiCOM.ItemEvent)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID
        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim oDataTable As SAPbouiCOM.DataTable

        If pval.ActionSuccess = True AndAlso pval.BeforeAction = False Then

            If Not oCFLEvento.SelectedObjects Is Nothing Then

                oDataTable = oCFLEvento.SelectedObjects

                EditTextArticulo.AsignaValorUserDataSource(oDataTable.GetValue("ItemCode", 0))
                EditTextDescArticulo.AsignaValorUserDataSource(oDataTable.GetValue("ItemName", 0))

            End If

        ElseIf pval.BeforeAction = True Then

            oConditions = CType(ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions), SAPbouiCOM.Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "U_SCGD_TipoArticulo"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_LESS_EQUAL
            oCondition.CondVal = "4"
            oCondition.BracketCloseNum = 1

            oCFL.SetConditions(oConditions)

        End If

    End Sub

    Public Sub CargarMatrixSBO(ByVal FormUID As String, ByRef pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pval.BeforeAction = False AndAlso pval.ActionSuccess = True Then
            CargarMatrix()
        End If

    End Sub

    Public Sub ButtonSBOCrearEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            RegistrarArticulosEspecific()

        End If

    End Sub

    Public Sub CheckSBOSelectAllEItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            Dim valorCheck As String = CheckBoxSelecAllE.ObtieneValorUserDataSource

            If valorCheck.Equals("Y") Then

                SeleccionarTodas("Y")
                CheckBoxSelecAllE.AsignaValorUserDataSource("Y")

            ElseIf valorCheck.Equals("N") Then

                SeleccionarTodas("N")
                CheckBoxSelecAllE.AsignaValorUserDataSource("N")

            End If

        End If

    End Sub

    Public Sub CheckSBOSelectAllMItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

            Dim valorCheck As String = CheckBoxSelecAllE.ObtieneValorUserDataSource

            If valorCheck.Equals("Y") Then

                SeleccionarTodas("Y")
                CheckBoxSelecAllE.AsignaValorUserDataSource("Y")

            ElseIf valorCheck.Equals("N") Then

                SeleccionarTodas("N")
                CheckBoxSelecAllE.AsignaValorUserDataSource("N")

            End If

        End If

    End Sub

    Public Sub CargarMatrix()

        Dim consulta As String = ""
        DataTableEspecific = FormularioSBO.DataSources.DataTables.Item("Especific")

        If Especificacion.Equals("E") Then

            consulta = "Select Code, Name from [@SCGD_ESTILO]"
            CargarDataTableEspecific(DataTableEspecific, consulta)

            Dim itemCode As String = EditTextArticulo.ObtieneValorUserDataSource().Trim

            CargarMatrixEsp(MatrixEstilo, DataTableEspecific, itemCode)

        ElseIf Especificacion.Equals("M") Then

            consulta = "Select Code, Name from [@SCGD_MODELO]"
            CargarDataTableEspecific(DataTableEspecific, consulta)

            Dim itemCode As String = EditTextArticulo.ObtieneValorUserDataSource().Trim

            CargarMatrixEsp(MatrixModelo, DataTableEspecific, itemCode)

        End If

    End Sub

    Public Sub CargarDataTableEspecific(ByRef dataTable As DataTable, ByVal consulta As String)

        dataTable.Clear()
        dataTable.ExecuteQuery(consulta)

    End Sub

    Public Sub CargaDataTableEspecificacion(ByVal dataTableEspc As DataTable, ByVal dataTable As DataTable, ByVal dataTableConsulta As DataTable, ByVal varCodeEsp As String, ByVal varDesEsp As String, ByVal usaDuracion As Boolean)

        Dim tamanoEspc As Integer = dataTable.Rows.Count
        Dim tamanoConsultaE As Integer = dataTableConsulta.Rows.Count
        Dim bandera As Boolean

        dataTableEspc.Rows.Clear()

        For i As Integer = 0 To tamanoEspc - 1

            Dim codEstiloE As String = dataTable.GetValue("Code", i).ToString.Trim

            bandera = False

            For z As Integer = 0 To tamanoConsultaE - 1

                Dim codEstiloC As String = dataTableConsulta.GetValue(varCodeEsp, z).ToString.Trim

                If codEstiloE.Equals(codEstiloC) Then

                    dataTableEspc.Rows.Add()
                    dataTableEspc.SetValue("selec", i, "Y")
                    dataTableEspc.SetValue("cod", i, codEstiloC)

                    If usaDuracion = True Then

                        dataTableEspc.SetValue("duraE", i, dataTableConsulta.GetValue("U_Duracion", z).ToString.Trim)

                    ElseIf usaDuracion = False Then

                        dataTableEspc.SetValue("duraE", i, 0)

                    End If

                    dataTableEspc.SetValue("desc", i, dataTableConsulta.GetValue(varDesEsp, z).ToString.Trim)
                    bandera = True

                    Exit For

                End If

            Next

            If bandera = False Then

                dataTableEspc.Rows.Add()
                dataTableEspc.SetValue("selec", i, "N")
                dataTableEspc.SetValue("cod", i, codEstiloE)
                dataTableEspc.SetValue("desc", i, dataTable.GetValue("Name", i).ToString.Trim)
                dataTableEspc.SetValue("duraE", i, 0)
                bandera = True

            End If

        Next


    End Sub

    Public Sub CargarMatrixEsp(ByVal matrix As MatrixSBO, ByVal dataTable As DataTable, ByVal itemCode As String)

        'Dim dataTableSel As DataTable
        tipoArticulo = Utilitarios.EjecutarConsulta(String.Format("Select U_SCGD_TipoArticulo from OITM where ItemCode = '{0}'", itemCode), CompanySBO.CompanyDB, CompanySBO.Server)
        Dim consulta As String

        If Not String.IsNullOrEmpty(tipoArticulo) Then

            Dim tipoArt As Integer = CType(tipoArticulo, Integer)

            If tipoArt = TiposArticulo.Repuesto Then


                consulta = String.Format("Select U_CodeEstilo, U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_ARTXESPECIFIC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.Clear()
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then
                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixEstilo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableEstilo, dataTable, DataTableConsulta, "U_CodeEstilo", "U_DesEstilo", False)

                ElseIf Especificacion.Equals("M") Then
                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixModelo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableModelo, dataTable, DataTableConsulta, "U_CodeModelo", "U_DesModelo", False)

                End If

                matrix.Matrix.Clear()
                matrix.Matrix.LoadFromDataSource()


            ElseIf tipoArt = TiposArticulo.Servicio Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_Duracion, U_CodeModelo, U_DesModelo from [@SCGD_SERVXESPECIFIC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.Clear()
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixEstilo.columnaDuracion.Columna.Editable = True

                    CargaDataTableEspecificacion(DataTableEstilo, dataTable, DataTableConsulta, "U_CodeEstilo", "U_DesEstilo", True)

                ElseIf Especificacion.Equals("M") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixModelo.columnaDuracion.Columna.Editable = True

                    CargaDataTableEspecificacion(DataTableModelo, dataTable, DataTableConsulta, "U_CodeModelo", "U_DesModelo", True)

                End If

                matrix.Matrix.Clear()
                matrix.Matrix.LoadFromDataSource()

            ElseIf tipoArt = TiposArticulo.ServicioExterno Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_SERVEXESPECIFI] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.Clear()
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixEstilo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableEstilo, dataTable, DataTableConsulta, "U_CodeEstilo", "U_DesEstilo", False)

                ElseIf Especificacion.Equals("M") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixModelo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableModelo, dataTable, DataTableConsulta, "U_CodeModelo", "U_DesModelo", False)

                End If

                matrix.Matrix.Clear()
                matrix.Matrix.LoadFromDataSource()

            ElseIf tipoArt = TiposArticulo.Suministro Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_SUMXESPECIFC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.Clear()
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixEstilo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableEstilo, dataTable, DataTableConsulta, "U_CodeEstilo", "U_DesEstilo", False)

                ElseIf Especificacion.Equals("M") Then

                    'Deshabilita la columna de duración para la respectiva matrix
                    MatrixModelo.columnaDuracion.Columna.Editable = False

                    CargaDataTableEspecificacion(DataTableModelo, dataTable, DataTableConsulta, "U_CodeModelo", "U_DesModelo", False)

                End If

                matrix.Matrix.Clear()
                matrix.Matrix.LoadFromDataSource()

            End If

        End If

    End Sub

    Public Sub RegistrarArticulosEspecific()

        Dim consulta As String
        Dim itemCode As String
        Dim maximoRegistro As String

        itemCode = EditTextArticulo.ObtieneValorUserDataSource()
        DataTableConsulta.Clear()

        If Not String.IsNullOrEmpty(tipoArticulo) Then

            Dim tipoArt As Integer = CType(tipoArticulo, Integer)

            If tipoArt = TiposArticulo.Repuesto Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_ARTXESPECIFIC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then

                    MatrixEstilo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableEstilo, DataTableConsulta, "[@SCGD_ARTXESPECIFIC]", itemCode, tipoArt, "U_CodeEstilo", "U_DesEstilo")

                ElseIf Especificacion.Equals("M") Then

                    MatrixModelo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableModelo, DataTableConsulta, "[@SCGD_ARTXESPECIFIC]", itemCode, tipoArt, "U_CodeModelo", "U_DesModelo")

                End If

            ElseIf tipoArt = TiposArticulo.Servicio Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_Duracion, U_CodeModelo, U_DesModelo from [@SCGD_SERVXESPECIFIC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.ExecuteQuery(consulta)

                If Especificacion.Equals("E") Then

                    MatrixEstilo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableEstilo, DataTableConsulta, "[@SCGD_SERVXESPECIFIC]", itemCode, tipoArt, "U_CodeEstilo", "U_DesEstilo")

                ElseIf Especificacion.Equals("M") Then

                    MatrixModelo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableModelo, DataTableConsulta, "[@SCGD_SERVXESPECIFIC]", itemCode, tipoArt, "U_CodeModelo", "U_DesModelo")

                End If
                
            ElseIf tipoArt = TiposArticulo.Suministro Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_SUMXESPECIFC] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.ExecuteQuery(consulta)
                
                If Especificacion.Equals("E") Then

                    MatrixEstilo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableEstilo, DataTableConsulta, "[@SCGD_SUMXESPECIFC]", itemCode, tipoArt, "U_CodeEstilo", "U_DesEstilo")

                ElseIf Especificacion.Equals("M") Then

                    MatrixModelo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableModelo, DataTableConsulta, "[@SCGD_SUMXESPECIFC]", itemCode, tipoArt, "U_CodeModelo", "U_DesModelo")

                End If

            ElseIf tipoArt = TiposArticulo.ServicioExterno Then

                consulta = String.Format("Select U_CodeEstilo ,U_DesEstilo, U_CodeModelo, U_DesModelo from [@SCGD_SERVEXESPECIFI] where U_ItemCode = '{0}'", itemCode)
                DataTableConsulta.ExecuteQuery(consulta)
                
                If Especificacion.Equals("E") Then

                    MatrixEstilo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableEstilo, DataTableConsulta, "[@SCGD_SERVEXESPECIFI]", itemCode, tipoArt, "U_CodeEstilo", "U_DesEstilo")

                ElseIf Especificacion.Equals("M") Then

                    MatrixModelo.Matrix.FlushToDataSource()
                    InsertarArticulos(DataTableModelo, DataTableConsulta, "[@SCGD_SERVEXESPECIFI]", itemCode, tipoArt, "U_CodeModelo", "U_DesModelo")

                End If

            End If

        End If

    End Sub

    Public Sub InsertarArticulos(ByVal dataTableMatrix As DataTable, ByVal dataTableCons As DataTable, ByVal tabla As String, ByVal itemCode As String, ByVal tipArticulo As Integer, ByVal varCodeEsp As String, ByVal varDesEsp As String)

        Dim tamanoM As Integer = dataTableMatrix.Rows.Count
        Dim tamanoC As Integer = dataTableCons.Rows.Count
        Dim seleccion As String
        Dim existe As Boolean
        Dim codeArtM As String
        Dim codeArtC As String
        Dim descArtM As String
        Dim duracionM As String
        Dim duracionC As String
        Dim resultadoMaximoR As String
        Dim maximoRegistro As Integer
        Dim queryInsercion As String
        Dim queryUpDate As String
        Dim queryDelete As String
        Dim queryMaximoReg As String

        For i As Integer = 0 To tamanoM - 1

            existe = False
            seleccion = dataTableMatrix.GetValue("selec", i).ToString.Trim

            If seleccion.Equals("Y") Then

                codeArtM = dataTableMatrix.GetValue("cod", i).ToString.Trim

                For z As Integer = 0 To tamanoC - 1

                    codeArtC = dataTableCons.GetValue(varCodeEsp, z).ToString.Trim

                    If codeArtM.Equals(codeArtC) Then

                        'Actualiza la duración estandar que es única para los Servicios
                        If tipArticulo = TiposArticulo.Servicio Then

                            duracionM = dataTableMatrix.GetValue("duraE", i).ToString.Trim
                            duracionC = dataTableCons.GetValue("U_Duracion", z).ToString.Trim

                            If Not duracionM.Equals(duracionC) Then

                                queryUpDate = String.Format("Update {0} set  U_Duracion = '{1}' where U_ItemCode = '{2}' and " + varCodeEsp + " = '{3}'", tabla, CType(duracionM, Decimal), itemCode, codeArtM)
                                Utilitarios.EjecutarConsulta(queryUpDate, CompanySBO.CompanyDB, CompanySBO.Server)

                            End If

                        End If

                        existe = True
                        Exit For

                    End If

                Next

                If existe = False Then

                    queryMaximoReg = String.Format("Select max(cast(Code as int)) from {0}", tabla)
                    resultadoMaximoR = Utilitarios.EjecutarConsulta(queryMaximoReg, CompanySBO.CompanyDB, CompanySBO.Server)

                    If Not String.IsNullOrEmpty(resultadoMaximoR) Then
                        maximoRegistro = CType(resultadoMaximoR, Integer)
                    Else
                        maximoRegistro = 0

                    End If

                    If tipArticulo = TiposArticulo.Servicio Then
                        queryInsercion = String.Format("Insert into {0} (Code, Name, U_ItemCode, " + varCodeEsp + ", " + varDesEsp + ", U_Duracion)VALUES('{1}','{2}','{3}','{4}','{5}','{6}')", tabla, maximoRegistro + 1, maximoRegistro + 1, itemCode, codeArtM, dataTableMatrix.GetValue("desc", i).ToString.Trim, CType(dataTableMatrix.GetValue("duraE", i).ToString.Trim, Decimal))
                        Utilitarios.EjecutarConsulta(queryInsercion, CompanySBO.CompanyDB, CompanySBO.Server)

                    Else

                        queryInsercion = String.Format("Insert into {0} (Code, Name, U_ItemCode, " + varCodeEsp + ", " + varDesEsp + ")VALUES('{1}','{2}','{3}','{4}','{5}')", tabla, maximoRegistro + 1, maximoRegistro + 1, itemCode, codeArtM, dataTableMatrix.GetValue("desc", i).ToString.Trim)
                        Utilitarios.EjecutarConsulta(queryInsercion, CompanySBO.CompanyDB, CompanySBO.Server)

                    End If

                End If

            ElseIf seleccion.Equals("N") Then

                codeArtM = dataTableMatrix.GetValue("cod", i).ToString.Trim

                For z As Integer = 0 To tamanoC - 1

                    codeArtC = dataTableCons.GetValue(varCodeEsp, z).ToString.Trim

                    If codeArtM.Equals(codeArtC) Then

                        queryDelete = String.Format("Delete from {0} where " + varCodeEsp + " = '{1}' and U_ItemCode = '{2}'", tabla, codeArtM, itemCode)
                        Utilitarios.EjecutarConsulta(queryDelete, CompanySBO.CompanyDB, CompanySBO.Server)
                        Exit For

                    End If

                Next

            End If

        Next

    End Sub

    Public Sub ManejoTabs()

        Especificacion = Utilitarios.EjecutarConsulta("Select U_EspVehic from [@SCGD_ADMIN]", CompanySBO.CompanyDB, CompanySBO.Server)

        If Especificacion.Equals("E") Then

            FormularioSBO.Items.Item("fldModelo").Enabled = False
            FormularioSBO.Items.Item("mtxModelo").Enabled = False
            FormularioSBO.Items.Item("fldEstilo").Click()

        ElseIf Especificacion.Equals("M") Then

            FormularioSBO.Items.Item("fldEstilo").Enabled = False
            FormularioSBO.Items.Item("mtxEstilo").Enabled = False
            FormularioSBO.Items.Item("fldModelo").Click()

        Else

            FormularioSBO.Items.Item("fldModelo").Enabled = False
            FormularioSBO.Items.Item("fldEstilo").Enabled = False
            FormularioSBO.Items.Item("mtxEstilo").Enabled = False
            FormularioSBO.Items.Item("mtxModelo").Enabled = False
        End If


    End Sub

    Public Sub SeleccionarTodas(ByVal valor As String)

        If Especificacion.Equals("E") Then

            Dim tamannoE As Integer = DataTableEstilo.Rows.Count

            If tamannoE > 0 Then

                For i As Integer = 0 To tamannoE - 1

                    DataTableEstilo.SetValue("selec", i, valor)

                Next

                MatrixEstilo.Matrix.LoadFromDataSource()

            End If

        ElseIf Especificacion.Equals("M") Then

            Dim tamannoM As Integer = DataTableModelo.Rows.Count

            If tamannoM > 0 Then

                For i As Integer = 0 To tamannoM - 1

                    DataTableModelo.SetValue("selec", i, valor)

                Next

                MatrixModelo.Matrix.LoadFromDataSource()

            End If

        End If

    End Sub

End Class
