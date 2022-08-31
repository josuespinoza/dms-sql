Imports SAPbouiCOM


'*******************************************
'*Maneja el formulario SeleccionarRepuestosOT
'*******************************************

Partial Public Class SeleccionarRepuestosOT

#Region "Declaraciones"

    Private oForm As SAPbouiCOM.Form
    Private lsListaAgregar As Generic.IList(Of String) = New Generic.List(Of String)
    Private lsListaEliminar As Generic.IList(Of Integer) = New Generic.List(Of Integer)

  
  
    
#End Region

#Region "Propiedades"
    
#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga repuestos a la matriz dependiendo del numero de Órden de Trabajo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaRepuestos(ByRef dtRepuestosTodos As DataTable, ByVal bandera As Boolean, Optional ByVal FormUID As String = "")



        Dim dtLocal As DataTable
        Dim oMatriz As Matrix

        Dim dcCantidad As Decimal
        Dim dcPrecio As Decimal

        Try
            'strConsulta = m_strConsulta
            'g_strConsultaArtiEspXModeEsti = m_strConsultaArtiEspXModeEsti
            'ObtieneEstiModYConfListaPrecios()

            If Not String.IsNullOrEmpty(FormUID) Then
                oForm = ApplicationSBO.Forms.Item(FormUID)
            Else
                oForm = FormularioSBO
            End If

            dtLocal = oForm.DataSources.DataTables.Item("local")


            ''g_strUsaConsultaSegunConf = String.Format(g_strUsaConsultaSegunConf, IncluirRepuestosOT.NoOT, g_strCodListPrecio)
            dtLocal.ExecuteQuery(g_strUsaConsultaSegunConf)

            oMatriz = DirectCast(oForm.Items.Item(strMatrizRepTodos).Specific, SAPbouiCOM.Matrix)

            dtRepuestosTodos.Rows.Clear()

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                dtRepuestosTodos.Rows.Add(1)

                'If Not String.IsNullOrEmpty(dtLocal.GetValue("cantidad", i)) Then
                '    dcCantidad = Decimal.Parse(dtLocal.GetValue("cantidad", i))
                'Else
                '    dcCantidad = 0
                'End If
                If Not String.IsNullOrEmpty(dtLocal.GetValue("Price", i)) Then
                    dcPrecio = Decimal.Parse(dtLocal.GetValue("Price", i))
                Else
                    dcPrecio = 0
                End If

                dtRepuestosTodos.SetValue("cod", i, dtLocal.GetValue("ItemCode", i))
                dtRepuestosTodos.SetValue("des", i, dtLocal.GetValue("ItemName", i))
                dtRepuestosTodos.SetValue("bod", i, dtLocal.GetValue("BOD", i))
                dtRepuestosTodos.SetValue("onH", i, dtLocal.GetValue("STK", i))
                dtRepuestosTodos.SetValue("can", i, "1")
                dtRepuestosTodos.SetValue("pre", i, dcPrecio.ToString(n))
                dtRepuestosTodos.SetValue("mon", i, dtLocal.GetValue("Currency", i))
                dtRepuestosTodos.SetValue("CodBar", i, dtLocal.GetValue("CodeBars", i))


            Next

            oMatriz.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores en los combos
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <param name="strQuery">Query para obtener la información</param>
    ''' <param name="strIDItem">Identificador del combo (Nombre)</param>
    ''' <remarks></remarks>
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

            Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            If cboCombo.ValidValues.Count = 0 Then
                oForm.Items.Item(strIDItem).Enabled = False
                'oForm.Items.Item("cboTAgen").
            Else
                oForm.Items.Item(strIDItem).Enabled = True
                oForm.Items.Item(strIDItem).DisplayDesc = True
            End If

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Verificar la configuracion para conocer si es por Modelos o Estilos
    ''' </summary>
    ''' <returns>
    ''' E = Estilos
    ''' M = Modelos
    ''' </returns>
    ''' <remarks></remarks>
    Private Function VerificaConfiguracion() As String
        Try
            Dim strConfiguracion As String = ""

            strConfiguracion = Utilitarios.EjecutarConsulta("select U_EspVehic from [@scgd_admin]",
                                                            CompanySBO.CompanyDB, CompanySBO.Server)

            Return strConfiguracion
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    ''' <summary>
    ''' Aplica los filtros ingresados para los repuestos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EjecutarFiltros(ByVal oForm As Form, ByVal FormUID As String)


        Dim oMatrizTodos As SAPbouiCOM.Matrix




        Dim strFiltroGrupo As String = "    and oi.ItmsGrpCod = '{0}' "
        Dim strFiltroFamilia As String = "  and	oi.U_SCGD_Fam = '{0}' "
        Dim strFiltroPropiedad As String = " and QryGroup{0} = 'Y' "

        Dim strFiltroDescripcion As String = "  and {0}.{1}ItemName like '%{2}%' "
        Dim strFiltroCodigo As String = "       and {0}.{1}ItemCode like '%{2}%' "
        Dim strFiltroCodeBars As String = " and {0}.{1}CodeBars like '{2}%'"

        

        Dim blnCambios As Boolean

        Dim dtLocal As DataTable

        Dim dcCantidad As Decimal
        Dim dcPrecio As Decimal

        Try
            blnCambios = False
            g_strConsulta = DMS_Connector.Queries.GetStrSpecificQuery("strConsultaRepuestos")
            g_strConsultaArtiEspXModeEsti = m_strConsultaArtiEspXModeEsti

            '' Se realiza metodo completar la consulta segun la configuracion de la lista de precios
            ObtieneEstiModYConfListaPrecios()

            If chkGrp.ObtieneValorUserDataSource() = "Y" And Not String.IsNullOrEmpty(cboGrp.ObtieneValorUserDataSource()) Then
                g_strUsaConsultaSegunConf += String.Format(strFiltroGrupo, cboGrp.ObtieneValorUserDataSource())
                blnCambios = True
            End If

            If chkFam.ObtieneValorUserDataSource() = "Y" And Not String.IsNullOrEmpty(cboFam.ObtieneValorUserDataSource()) Then
                g_strUsaConsultaSegunConf += String.Format(strFiltroFamilia, cboFam.ObtieneValorUserDataSource())
                blnCambios = True
            End If
            
            If chkPro.ObtieneValorUserDataSource() = "Y" And Not String.IsNullOrEmpty(cboPro.ObtieneValorUserDataSource()) Then
                g_strUsaConsultaSegunConf += String.Format(strFiltroPropiedad, cboPro.ObtieneValorUserDataSource().Trim())
                blnCambios = True
            End If

            If Not String.IsNullOrEmpty(txtCod.ObtieneValorUserDataSource) Then
                If g_strUsaAsocxEspecif.Equals("N") Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodigo, "oi", "", txtCod.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodigo, "art", "U_", txtCod.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt = False Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodigo, "oi", "", txtCod.ObtieneValorUserDataSource())
                    blnCambios = True
                End If
            End If

            If Not String.IsNullOrEmpty(txtDes.ObtieneValorUserDataSource) Then
                If g_strUsaAsocxEspecif.Equals("N") Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroDescripcion, "oi", "", txtDes.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroDescripcion, "art", "U_", txtDes.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt = False Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroDescripcion, "oi", "", txtDes.ObtieneValorUserDataSource())
                    blnCambios = True
                End If
            End If

            If Not String.IsNullOrEmpty(txtCodBar.ObtieneValorUserDataSource) Then
                If g_strUsaAsocxEspecif.Equals("N") Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodeBars, "oi", "", txtCodBar.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodeBars, "art", "U_", txtCodBar.ObtieneValorUserDataSource())
                    blnCambios = True
                ElseIf g_strUsaAsocxEspecif.Equals("Y") And mBExisteArt = False Then
                    g_strUsaConsultaSegunConf += String.Format(strFiltroCodeBars, "oi", "", txtCodBar.ObtieneValorUserDataSource())
                    blnCambios = True
                End If
            End If

            dtRepuestosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)
            oForm.Freeze(True)
            If blnCambios Then



                dtLocal = oForm.DataSources.DataTables.Item("local")
                g_strUsaConsultaSegunConf = String.Format(g_strUsaConsultaSegunConf, _oIncluirRepuestosOT.NoOT, g_strCodListPrecio)
                dtLocal.ExecuteQuery(g_strUsaConsultaSegunConf)

                dtRepuestosTodos.Rows.Clear()


                For i As Integer = 0 To dtLocal.Rows.Count - 1
                    dtRepuestosTodos.Rows.Add(1)

                    If Not String.IsNullOrEmpty(dtLocal.GetValue("Price", i)) Then
                        dcPrecio = Decimal.Parse(dtLocal.GetValue("Price", i))
                    Else
                        dcPrecio = 0
                    End If

                    dtRepuestosTodos.SetValue("cod", i, dtLocal.GetValue("ItemCode", i))
                    dtRepuestosTodos.SetValue("des", i, dtLocal.GetValue("ItemName", i))
                    dtRepuestosTodos.SetValue("can", i, "1")
                    dtRepuestosTodos.SetValue("pre", i, dcPrecio.ToString(n))
                    dtRepuestosTodos.SetValue("bod", i, dtLocal.GetValue("BOD", i))
                    dtRepuestosTodos.SetValue("onH", i, dtLocal.GetValue("STK", i))
                    dtRepuestosTodos.SetValue("mon", i, dtLocal.GetValue("Currency", i))
                    dtRepuestosTodos.SetValue("CodBar", i, dtLocal.GetValue("CodeBars", i))
                Next

                oMatrizTodos = DirectCast(oForm.Items.Item(strMatrizRepTodos).Specific, SAPbouiCOM.Matrix)
                oMatrizTodos.LoadFromDataSource()


            Else

                CargaRepuestos(dtRepuestosTodos, True, FormUID)

            End If
            oForm.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Incluye los repuestos a la matriz de repuestos seleccioandos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub IncluyeRepuesto()

        Dim strCodigo As String = String.Empty
        Dim strDes As String = String.Empty
        Dim strBodega As String = String.Empty
        Dim strOnHand As String = String.Empty
        Dim strCantidad As String = String.Empty
        Dim strPrice As String = String.Empty
        Dim strCurrency As String = String.Empty
        Dim strCodigoBarras As String = String.Empty
        Dim strSeleccion As String = String.Empty
        Dim oMatrizTodos As SAPbouiCOM.Matrix
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim ExistenMarcados As Boolean = False
        Dim Tamano As Integer = 0
        Dim posicion As Integer = 0
        Dim dcCantidad As Decimal
        Dim dcPrecio As Decimal

        Try
            ExistenMarcados = False

            oMatrizTodos = DirectCast(oForm.Items.Item(strMatrizRepTodos).Specific, SAPbouiCOM.Matrix)
            oMatrizTodos.FlushToDataSource()

            oMatriz = DirectCast(oForm.Items.Item(strMatrizRep).Specific, SAPbouiCOM.Matrix)

            dtRepuestosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)
            dtRepuestos = oForm.DataSources.DataTables.Item(strDataTable)
            Tamano = dtRepuestos.Rows.Count

            For Each Str As String In lsListaAgregar
                posicion = Integer.Parse(Str)

                strSeleccion = dtRepuestosTodos.GetValue("sel", posicion - 1)
                strCodigo = dtRepuestosTodos.GetValue("cod", posicion - 1)

                If strSeleccion = "Y" And
                    Not String.IsNullOrEmpty(strCodigo) Then
                    ExistenMarcados = True
                    strCodigo = dtRepuestosTodos.GetValue("cod", posicion - 1)
                    strDes = dtRepuestosTodos.GetValue("des", posicion - 1)
                    strBodega = dtRepuestosTodos.GetValue("bod", posicion - 1)
                    strOnHand = dtRepuestosTodos.GetValue("onH", posicion - 1)
                    strCantidad = dtRepuestosTodos.GetValue("can", posicion - 1)
                    strPrice = dtRepuestosTodos.GetValue("pre", posicion - 1)
                    strCurrency = dtRepuestosTodos.GetValue("mon", posicion - 1)
                    strCodigoBarras = dtRepuestosTodos.GetValue("CodBar", posicion - 1)
                    dtRepuestos.Rows.Add(1)

                    If Not String.IsNullOrEmpty(strCantidad) Then
                        dcCantidad = Decimal.Parse(strCantidad)
                    Else
                        dcCantidad = 0
                    End If
                    If Not String.IsNullOrEmpty(strPrice) Then
                        dcPrecio = Decimal.Parse(strPrice)
                    Else
                        dcPrecio = 0
                    End If

                    dtRepuestos.SetValue("cod", Tamano, strCodigo)
                    dtRepuestos.SetValue("des", Tamano, strDes)
                    dtRepuestos.SetValue("bod", Tamano, strBodega)
                    dtRepuestos.SetValue("onH", Tamano, strOnHand)
                    dtRepuestos.SetValue("can", Tamano, dcCantidad.ToString(n))
                    dtRepuestos.SetValue("pre", Tamano, dcPrecio.ToString(n))
                    dtRepuestos.SetValue("mon", Tamano, strCurrency)
                    dtRepuestos.SetValue("CodBar", Tamano, strCodigoBarras)


                    dtRepuestosTodos.SetValue("sel", posicion - 1, "N")
                    Tamano += 1

                End If
            Next
            lsListaAgregar.Clear()

            If ExistenMarcados Then

                oForm.Freeze(True)

                oMatriz.LoadFromDataSource()
                oMatrizTodos.LoadFromDataSource()

                oForm.Freeze(False)

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega filas a la lista para agregar
    ''' </summary>
    ''' <param name="Fila"></param>
    ''' <remarks></remarks>
    Private Sub AgregaLista(ByVal Fila As String)

        Dim oMatrix As Matrix
        Dim intFila As Integer = 0

        Try
            oMatrix = DirectCast(oForm.Items.Item(strMatrizRepTodos).Specific, Matrix)
            oMatrix.FlushToDataSource()

            dtRepuestosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)

            intFila = Integer.Parse(Fila)

            If Not String.IsNullOrEmpty(dtRepuestosTodos.GetValue("cod", intFila - 1)) Then
                lsListaAgregar.Add(Fila)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    ''' <summary>
    ''' Ordena la lista de mayor a menor para eliminarlos de la matriz de repustos seleccionados
    ''' </summary>
    ''' <param name="lsListaEliminar"></param>
    ''' <remarks></remarks>
    Public Shared Sub OrdenaLista(ByVal lsListaEliminar As Generic.IList(Of Integer), ByRef lsListaOrdenada As Generic.IList(Of Integer))

        Dim posicion As Integer
        Dim ExisteUnMayor As Boolean
        Dim ValorIngresar As Integer

        posicion = 0
        ExisteUnMayor = False
        ValorIngresar = 0

        If lsListaEliminar.Count = 0 Then
            'Return lsListaOrdenada
            Exit Sub
        End If

        For Each val1 As Integer In lsListaEliminar
            ValorIngresar = val1
            For Each val2 As Integer In lsListaEliminar
                If val2 > val1 Then
                    ExisteUnMayor = True
                    Exit For
                End If
            Next
            posicion += 1
            If Not ExisteUnMayor Then
                lsListaOrdenada.Add(ValorIngresar)
                lsListaEliminar.RemoveAt(posicion - 1)
                OrdenaLista(lsListaEliminar, lsListaOrdenada)
                Exit For
            Else
                ExisteUnMayor = False
            End If
        Next


    End Sub

    ''' <summary>
    ''' Saca los repuestos de la matriz de repuestos seleccioandos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExcluyeRepuesto()

        Dim strSeleccion As String = ""
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim ExisteSeleccionado As Boolean = False
        Dim Tamano As Integer = 0
        Dim posicion As Integer = 0

        Dim lsListaOrdenada As Generic.IList(Of Integer) = New Generic.List(Of Integer)

        Try
            ExisteSeleccionado = False

            OrdenaLista(lsListaEliminar, lsListaOrdenada)

            oMatriz = DirectCast(oForm.Items.Item(strMatrizRep).Specific, SAPbouiCOM.Matrix)
            oMatriz.FlushToDataSource()

            dtRepuestos = oForm.DataSources.DataTables.Item(strDataTable)
            Tamano = dtRepuestos.Rows.Count

            For Each Str As String In lsListaOrdenada
                posicion = Integer.Parse(Str)
                strSeleccion = dtRepuestos.GetValue("sel", posicion - 1)

                If strSeleccion = "Y" Then
                    ExisteSeleccionado = True
                    dtRepuestos.Rows.Remove(posicion - 1)
                End If
            Next

            lsListaOrdenada.Clear()
            lsListaEliminar.Clear()

            If ExisteSeleccionado Then
                oForm.Freeze(True)

                oMatriz.LoadFromDataSource()

                oForm.Freeze(False)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    ''' <summary>
    ''' Ingresa los repuestos a la matriz de la cotizacion
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AgregaRepuestosCotizacion(ByVal FormUID As String, ByVal Validacion As Boolean, ByRef BubbleEvent As Boolean)

        'Dim oMatrix As Matrix
        'Dim oForm As Form
        'Dim objIncluirRepuestosOT As New IncluirRepuestosOT(ApplicationSBO, CompanySBO, CatchingEvents.strMenuIncluirRepOT)

        Try

            g_oForm = ApplicationSBO.Forms.Item(FormUID)
            g_oMatrix = DirectCast(oForm.Items.Item(strMatrizRep).Specific, Matrix)
            g_oMatrix.FlushToDataSource()
            dtRepuestos = oForm.DataSources.DataTables.Item(strDataTable)
            _oIncluirRepuestosOT.IncluirRepuestosSeleccionados(dtRepuestos, BubbleEvent)
            g_oForm.Close()

                'If Not Validacion Then oForm.Close()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Manejo de eventos para el formulario
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <param name="pVal">Objeto evento</param>
    ''' <param name="BubbleEvent">Evento burbuja del SDK</param>
    ''' <remarks></remarks>
    Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
        End Select
    End Sub

    ''' <summary>
    ''' Manejo de eventos de tipo ItemPressed
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <param name="pVal">Objeto evento</param>
    ''' <param name="BubbleEvent">Evento burbuja del SDK</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim strNombreTabla As String = ""
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        oForm = ApplicationSBO.Forms.Item(FormUID)

        If pVal.BeforeAction = True Then
            Select Case pVal.ItemUID
                Case "btnSel"
                    MatrizRepuestosSeleccionados.Matrix.FlushToDataSource()
                    If Not String.IsNullOrEmpty(dtRepuestos.GetValue("cod", 0).ToString()) Then
                        _oIncluirRepuestosOT.ValidaPrecios(dtRepuestos, BubbleEvent)
                    Else
                        FormularioSBO.Close()
                    End If
                Case "2"
                    mBExisteArt = False

            End Select
        ElseIf pVal.ActionSuccess Then
            Select Case pVal.ItemUID
                Case "chkGru"

                    oItem = oForm.Items.Item("cboGrp")
                    oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                    If chkGrp.ObtieneValorUserDataSource = "Y" Then
                        ManejaEstadoControl(oForm, "cboGrp", True)
                        If Not oCombo.ValidValues.Count > 0 Then
                            CargarValidValuesEnCombos(oForm,
                                                      "select ItmsGrpCod, ItmsGrpNam from OITB with(nolock) order by ItmsGrpCod ",
                                                      "cboGrp")
                        End If
                    ElseIf chkGrp.ObtieneValorUserDataSource = "N" Then
                        ManejaEstadoControl(oForm, "cboGrp", False)
                        cboGrp.AsignaValorUserDataSource("")
                    End If
                Case "chkFam"

                    oItem = oForm.Items.Item("cboFam")
                    oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                    If chkFam.ObtieneValorUserDataSource = "Y" Then
                        ManejaEstadoControl(oForm, "cboFam", True)
                        If Not oCombo.ValidValues.Count > 0 Then
                            CargarValidValuesEnCombos(oForm,
                                                      "select Code, Name from [@SCGD_FAMILIA] with(nolock) ",
                                                      "cboFam")
                        End If
                    ElseIf chkFam.ObtieneValorUserDataSource = "N" Then
                        ManejaEstadoControl(oForm, "cboFam", False)
                        cboFam.AsignaValorUserDataSource("")
                    End If
                Case "chkPro"

                    oItem = oForm.Items.Item("cboPro")
                    oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                    If chkPro.ObtieneValorUserDataSource = "Y" Then
                        ManejaEstadoControl(oForm, "cboPro", True)
                        If Not oCombo.ValidValues.Count > 0 Then
                            CargarValidValuesEnCombos(oForm,
                                                      "select ItmsTypCod, ItmsGrpNam from OITG with(nolock) order by ItmsTypCod ",
                                                      "cboPro")
                        End If
                    ElseIf chkPro.ObtieneValorUserDataSource = "N" Then
                        ManejaEstadoControl(oForm, "cboPro", False)
                        cboPro.AsignaValorUserDataSource("")
                    End If
                    Case "btnBus"
                    EjecutarFiltros(oForm, FormUID)
                Case "mtxLsRep"
                    oForm.Freeze(True)
                    If pVal.ColUID = "Col_sel" And pVal.Row > 0 Then

                        dtRepuestosTodos = oForm.DataSources.DataTables.Item(strDataTableTodos)
                        oMatrix = DirectCast(oForm.Items.Item(strMatrizRepTodos).Specific, Matrix)
                        oMatrix.FlushToDataSource()

                        If pVal.Row <= dtRepuestosTodos.Rows.Count Then
                            If dtRepuestosTodos.GetValue("sel", pVal.Row - 1) = "Y" Then
                                AgregaLista(pVal.Row.ToString())
                            End If
                        End If
                    End If
                    oForm.Freeze(False)
                Case "mtxRep"
                    If pVal.ColUID = "Col_sel" And pVal.Row > 0 Then

                        dtRepuestos = oForm.DataSources.DataTables.Item(strDataTable)
                        oMatrix = DirectCast(oForm.Items.Item(strMatrizRep).Specific, Matrix)
                        oMatrix.FlushToDataSource()

                        If pVal.Row <= dtRepuestos.Rows.Count Then
                            If dtRepuestos.GetValue("sel", pVal.Row - 1) = "Y" Then
                                lsListaEliminar.Add(pVal.Row)
                            End If
                        End If
                    End If
                Case "btnAgre"
                    IncluyeRepuesto()
                Case "btnElim"
                    ExcluyeRepuesto()
                Case "btnSel"
                    MatrizRepuestosSeleccionados.Matrix.FlushToDataSource()
                    _oIncluirRepuestosOT.IncluirRepuestosSeleccionados(dtRepuestos, BubbleEvent)
                    FormularioSBO.Close()
            End Select

        End If
    End Sub

    ''' <summary>
    ''' Manejo de estados para combox
    ''' </summary>
    ''' <param name="oForm">Formulario</param>
    ''' <param name="NombreControl">Nombre del combo</param>
    ''' <param name="Valor">Valor para activar o no un combo</param>
    ''' <remarks></remarks>
    Private Sub ManejaEstadoControl(ByRef oForm As SAPbouiCOM.Form, ByVal NombreControl As String, ByVal Valor As Boolean)

        If Valor Then
            oForm.Items.Item(NombreControl).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        ElseIf Not Valor Then
            oForm.Items.Item(NombreControl).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        End If

    End Sub

#End Region

End Class
