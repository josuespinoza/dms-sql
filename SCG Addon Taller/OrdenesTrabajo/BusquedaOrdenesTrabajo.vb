Partial Public Class BusquedaOrdenesTrabajo

#Region "Declaraciones"

    Private Const strBtnBuscar As String = "btnBuscar"
    Private Const strchkEstado As String = "chkEst"
    Private Const strchkMarca As String = "chkMar"
    Private Const strchkEstilo As String = "chkEsti"
    Private Const strchkModelo As String = "chkMod"
    Private Const strchkAsesor As String = "chkAse"
    Private Const strchkRecepcion As String = "chkAbie"
    Private Const strchkCompromiso As String = "chkProc"
    Private Const strchkCerrada As String = "chkCer"

    Private Const strdtBusqueda As String = "busqueda"

    Private Const strConsultaSELECT As String = " SELECT Q.DocEntry, " +
                                                    " Q.U_SCGD_Numero_OT, " +
                                                    " Q.U_SCGD_Numero_OT as NoOt, " +
                                                    " T.Name, " +
                                                    " Q.U_SCGD_Cod_Unidad, " +
                                                    " Q.U_SCGD_Num_Placa, " +
                                                    " Q.U_SCGD_Estado_Cot, " +
                                                    " Q.U_SCGD_Gorro_Veh, " +
                                                    " Q.U_SCGD_No_Visita, " +
                                                    " Q.CardCode, " +
                                                    " Q.CardName, " +
                                                    " Q.U_SCGD_Des_Marc, " +
                                                    " Q.U_SCGD_Des_Esti, " +
                                                    " H.firstname + ' ' + H.lastName asesor, " +
                                                    " Q.U_SCGD_Fech_Recep, " +
                                                    " Q.U_SCGD_Fech_Comp, " +
                                                    " Q.U_SCGD_FCierre, " +
                                                    " S.Name as Sucursal " +
                                                    " FROM OQUT  AS Q with(nolock) " +
                                                    " INNER JOIN OHEM AS H with(nolock) ON Q.OwnerCode = H.empID " +
                                                    " INNER JOIN [@SCGD_TIPO_ORDEN] AS T with(nolock) ON Q.U_SCGD_Tipo_OT = T.Code " +
                                                    " INNER JOIN [@SCGD_SUCURSALES] AS S with(nolock) ON S.Code = Q.U_SCGD_idSucursal "

    Private Const strConsultaTodasOT As String = strConsultaSELECT + " WHERE U_SCGD_Numero_OT IS NOT NULL ORDER BY Q.DocEntry "
    Private Const strConsultaORDER As String = " ORDER BY Q.DocEntry "

    Public dtBusquedas As SAPbouiCOM.DataTable
    Public mtxBusquedas As MatrizBusquedaOT

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga del formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFormulario()
        Try
            'inicia DT
            IniciaDataTable()
            'carga COMBOS
            Call CargarValidValuesEnCombos(FormularioSBO, " SELECT Code, Name FROM [@SCGD_ESTADOS_OT] with(nolock) ORDER BY Code ASC", "cboEst")
            Call CargarValidValuesEnCombos(FormularioSBO, " SELECT Code, Name FROM [@SCGD_MARCA] with(nolock) ORDER BY Code ASC", "cboMar")
            Call CargarValidValuesEnCombos(FormularioSBO, " SELECT EmpId, ISNULL(firstName,'')  + ' ' + isnull(middleName,'')  + ' ' + ISNULL(lastName,'') FROM [OHEM] with (nolock) Where Active = 'Y' and U_SCGD_TipoEmp = 'A' and firstName is not null ORDER BY empId ASC ", "cboAse")
            Call CargarValidValuesEnCombos(FormularioSBO, " SELECT Code, Name FROM [@SCGD_SUCURSALES] with(nolock) ORDER BY Code ASC", "cboSucur")
            'carga matriz de busquedas 
            'CargaMatriz(strConsultaTodasOT)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Manejo de eventos de tipo ItemPresed
    ''' </summary>
    ''' <param name="FormUID">UID del formulario</param>
    ''' <param name="pVal">Objeto de tipo evento</param>
    ''' <param name="BubbleEvent">Evento burbuja de la aplicacion</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)

        If pVal.BeforeAction = True And pVal.ActionSuccess = False Then
            'Before action

        ElseIf pVal.BeforeAction = False And pVal.ActionSuccess = True Then
            FormularioSBO.Freeze(True)

            'Action Succes
            mtxBusquedas.Matrix.FlushToDataSource()

            FormularioSBO.ActiveItem = "txtNoOT"

            Select Case pVal.ItemUID

                Case strBtnBuscar
                    'Aplica los filtros de la busqueda 
                    Buscar()
                Case strchkEstado
                    If chkEstado.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("cboEst", True)
                    ElseIf chkEstado.ObtieneValorUserDataSource.Trim = "N" Then
                        cboEstado.AsignaValorUserDataSource("")
                        ManejoComponente("cboEst", False)
                    End If
                Case strchkMarca
                    If chkMarca.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("cboMar", True)
                    ElseIf chkMarca.ObtieneValorUserDataSource.Trim = "N" Then
                        cboMarca.AsignaValorUserDataSource("")
                        ManejoComponente("cboMar", False)
                    End If
              
                Case strchkAsesor
                    If chkAsesor.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("cboAse", True)
                    ElseIf chkAsesor.ObtieneValorUserDataSource.Trim = "N" Then
                        cboAsesor.AsignaValorUserDataSource("")
                        ManejoComponente("cboAse", False)
                    End If
                Case strchkRecepcion
                    If chkRecepcion.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("txtRece1", True)
                        ManejoComponente("txtRece2", True)
                    ElseIf chkRecepcion.ObtieneValorUserDataSource.Trim = "N" Then
                        txtRecepcion1.AsignaValorUserDataSource("")
                        txtRecepcion2.AsignaValorUserDataSource("")
                        ManejoComponente("txtRece1", False)
                        ManejoComponente("txtRece2", False)
                    End If
                Case strchkCompromiso
                    If chkCompromiso.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("txtComp1", True)
                        ManejoComponente("txtComp2", True)
                    ElseIf chkCompromiso.ObtieneValorUserDataSource.Trim = "N" Then
                        txtCompromiso1.AsignaValorUserDataSource("")
                        txtCompromiso2.AsignaValorUserDataSource("")
                        ManejoComponente("txtComp1", False)
                        ManejoComponente("txtComp2", False)
                    End If
                Case strchkCerrada
                    If chkCerrado.ObtieneValorUserDataSource.Trim = "Y" Then
                        ManejoComponente("txtCerr1", True)
                        ManejoComponente("txtCerr2", True)
                    ElseIf chkCerrado.ObtieneValorUserDataSource.Trim = "N" Then
                        txtCerrado1.AsignaValorUserDataSource("")
                        txtCerrado2.AsignaValorUserDataSource("")
                        ManejoComponente("txtCerr1", False)
                        ManejoComponente("txtCerr2", False)
                    End If

                Case chkSucursal.UniqueId
                    If chkSucursal.ObtieneValorUserDataSource.Trim.Equals("Y") Then
                        ManejoComponente(cboSucursal.UniqueId, True)

                    ElseIf chkSucursal.ObtieneValorUserDataSource.Trim.Equals("N") Then
                        cboSucursal.AsignaValorUserDataSource("")
                        ManejoComponente(cboSucursal.UniqueId, False)

                    End If
            End Select
            FormularioSBO.Freeze(False)
        End If

    End Sub

    ''' <summary>
    ''' Ejecuta las busquedas de acuerdo a los filtros que se ingresan
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Buscar()
        Dim strWhere As String = "WHERE U_SCGD_Numero_OT IS NOT NULL "
        Try
            If Not String.IsNullOrEmpty(txtNoOT.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_Numero_OT LIKE '{0}%'", txtNoOT.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtNoUnidad.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_Cod_Unidad LIKE '{0}%'", txtNoUnidad.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtPlaca.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_Num_Placa LIKE '{0}%'", txtPlaca.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtNoCono.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_Gorro_Veh LIKE '{0}%'", txtNoCono.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtNoVisita.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_No_Visita LIKE '{0}%'", txtNoVisita.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(cboEstado.ObtieneValorUserDataSource.Trim) Then
                Dim strEstado As String = ""
                strEstado = Utilitarios.EjecutarConsulta(String.Format("SELECT NAME FROM [@SCGD_ESTADOS_OT] with(nolock) WHERE CODE LIKE '{0}%'", cboEstado.ObtieneValorUserDataSource()), CompanySBO.CompanyDB, CompanySBO.Server)
                strWhere += String.Format(" AND U_SCGD_Estado_Cot = '{0}'", strEstado)
            End If

            If Not String.IsNullOrEmpty(cboMarca.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_Cod_Marca LIKE '{0}%'", cboMarca.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(cboAsesor.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND OwnerCode LIKE '{0}%'", cboAsesor.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(cboSucursal.ObtieneValorUserDataSource.Trim) Then strWhere += String.Format(" AND U_SCGD_idSucursal = '{0}'", cboSucursal.ObtieneValorUserDataSource.Trim)

            If Not String.IsNullOrEmpty(txtRecepcion1.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_Fech_Recep >= '{0}'", txtRecepcion1.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtRecepcion2.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_Fech_Recep <= '{0}'", txtRecepcion2.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtCompromiso1.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_Fech_Comp >= '{0}'", txtCompromiso1.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtCompromiso2.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_Fech_Comp <= '{0}'", txtCompromiso2.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtCerrado1.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_FCierre >= '{0}'", txtCerrado1.ObtieneValorUserDataSource.Trim)
            If Not String.IsNullOrEmpty(txtCerrado2.ObtieneValorUserDataSource().Trim) Then strWhere += String.Format(" AND U_SCGD_FCierre <= '{0}'", txtCerrado2.ObtieneValorUserDataSource.Trim)


            CargaMatriz(strConsultaSELECT + strWhere + strConsultaORDER)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el valor para el datatable de busqueda 
    ''' que esta asociado a la matriz en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IniciaDataTable()
        dtBusquedas = FormularioSBO.DataSources.DataTables.Add(strdtBusqueda)
        dtBusquedas.Columns.Add("docentry", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("noot", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("no_ot", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("tipot", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("nouni", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("placa", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("est", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("cono", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("visita", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("codcli", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("nomcl", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("mar", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("esti", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("asesor", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("fape", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("fpro", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("fcier", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        dtBusquedas.Columns.Add("sucur", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 100)
        mtxBusquedas = New MatrizBusquedaOT("mtxBusq", FormularioSBO, strdtBusqueda)
        mtxBusquedas.CreaColumnas()
        mtxBusquedas.LigaColumnas()

        Dim usaOTSap As Boolean = Utilitarios.ValidarOTInternaConfiguracion(CompanySBO)
        Dim oMatriz As SAPbouiCOM.Matrix
        oMatriz = DirectCast(FormularioSBO.Items.Item("mtxBusq").Specific, SAPbouiCOM.Matrix)
        oMatriz.Columns.Item("ColNoOT").Visible = Not usaOTSap
        oMatriz.Columns.Item("ColNoOTS").Visible = usaOTSap
    End Sub

    ''' <summary>
    ''' Carga la matriz con todas las ot 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaMatriz(ByVal Consulta As String)
        Try
            dtBusquedas.ExecuteQuery(Consulta)
            mtxBusquedas.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ManejadorEventoLinkPress(ByRef pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByRef formOT As SCG.ServicioPostVenta.OrdenTrabajo)

        If (pval.ItemUID = "mtxBusq" AndAlso pval.ColUID = "ColNoOTS") Then
            Dim oform As SAPbouiCOM.Form = ApplicationSBO.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)
            If (formOT IsNot Nothing) Then
                Dim oGestorFormularios As GestorFormularios
                oGestorFormularios = New GestorFormularios(ApplicationSBO)
                If Not oGestorFormularios.FormularioAbierto(formOT, activarSiEstaAbierto:=True) Then
                    formOT.FormularioSBO = oGestorFormularios.CargaFormulario(formOT)
                End If
            End If
            Dim OtId As String = String.Empty
            Dim oMatriz As SAPbouiCOM.Matrix

            oMatriz = DirectCast(oform.Items.Item("mtxBusq").Specific, SAPbouiCOM.Matrix)
            OtId = (oMatriz.Columns.Item("ColNoOTS").Cells.Item(pval.Row).Specific).Value.ToString().Trim()

            formOT.CargarOT(OtId)
        End If

    End Sub


#End Region

End Class
