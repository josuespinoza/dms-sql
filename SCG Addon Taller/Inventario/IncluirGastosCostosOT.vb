Imports DMSOneFramework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess

Partial Public Class IncluirGastosCostosOT

#Region "Declaraciones"

    Public _hsAprobado As New Hashtable

    'obj global
    Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO


    Private Enum EstadosAprobacion
        Aprobado = 1
        NoAprobado = 2
        FaltoAprobacion = 3
    End Enum


#End Region

#Region "Propiedades"


    Public Property hsAprobado As Hashtable
        Get
            Return _hsAprobado
        End Get
        Set(ByVal value As Hashtable)
            _hsAprobado = value
        End Set
    End Property

#End Region


#Region "METODOS"

    Public Sub CargarGastos()

        Dim BloquearControles As Boolean = False
        Dim strNoOT As String
        Dim strEstado As String
        Dim strConsulta As String = "  select '' as sel,'Y' as per, q1.ItemCode, q1.Dscription, " +
                                     " convert(numeric(19,6), q1.Quantity) as Quantity, convert(numeric(19,6), " +
                                     " q1.Price) as Price, q1.Currency, convert(varchar(100),q1.U_SCGD_Aprobado) as U_SCGD_Aprobado, q1.TaxCode as TaxCode, " +
                                     " q1.LineNum as lnum , q1.U_SCGD_NoFacPro fact, q1.U_SCGD_NoAsGastos AsientoG, q1.U_SCGD_Costo" +
                                     " from OQUT oq " +
                                     " inner join QUT1 q1 " +
                                     " on oq.DocEntry = q1.DocEntry " +
                                     " inner join OITM oi on oi.ItemCode = q1.ItemCode " +
                                     " where U_SCGD_Numero_OT = '{0}' " +
                                     " and oi.U_SCGD_TipoArticulo = 11  "

        Dim strConsultaEstado As String = "Select code from [@SCGD_ESTADOS_OT] where Name = '{0}'"
        Try
            strEstado = Utilitarios.EjecutarConsulta(String.Format(strConsultaEstado, txtEsOT.ObtieneValorUserDataSource.Trim()), CompanySBO.CompanyDB, CompanySBO.Server)

            strNoOT = txtNoOrden.ObtieneValorUserDataSource()

            dtLocal = FormularioSBO.DataSources.DataTables.Item(strDTLocal)
            dtGastos = FormularioSBO.DataSources.DataTables.Item(strDTGastos)

            If Not String.IsNullOrEmpty(strNoOT) Then

                dtLocal.Rows.Clear()
                dtGastos.Rows.Clear()

                strConsulta = String.Format(strConsulta, strNoOT.Trim())
                dtLocal.ExecuteQuery(strConsulta)

                If Not String.IsNullOrEmpty(dtLocal.GetValue("ItemCode", 0)) Then

                    ActualizaDescripcionesPorUDF(dtLocal, "U_SCGD_Aprobado", FormularioSBO)


                    For i As Integer = 0 To dtLocal.Rows.Count - 1
                        dtGastos.Rows.Add(1)
                        dtGastos.SetValue("sel", i, dtLocal.GetValue("sel", i))
                        dtGastos.SetValue("per", i, My.Resources.Resource.Si)
                        dtGastos.SetValue("cod", i, dtLocal.GetValue("ItemCode", i))
                        dtGastos.SetValue("des", i, dtLocal.GetValue("Dscription", i))
                        dtGastos.SetValue("can", i, dtLocal.GetValue("Quantity", i))
                        dtGastos.SetValue("mon", i, strMoneda)                          'le pone la moneda de la cotizacion
                        dtGastos.SetValue("pre", i, dtLocal.GetValue("Price", i))
                        dtGastos.SetValue("cos", i, dtLocal.GetValue("U_SCGD_Costo", i))
                        dtGastos.SetValue("apr", i, dtLocal.GetValue("U_SCGD_Aprobado", i))
                        dtGastos.SetValue("imp", i, dtLocal.GetValue("TaxCode", i))
                        dtGastos.SetValue("lnum", i, dtLocal.GetValue("lnum", i))
                        dtGastos.SetValue("fac", i, dtLocal.GetValue("fact", i))
                        dtGastos.SetValue("asi", i, dtLocal.GetValue("AsientoG", i))
                    Next

                    MatrizGastosOT.Matrix.LoadFromDataSource()

                End If
            Else

                dtLocal.Clear()
                dtGastos.Rows.Clear()

                MatrizGastosOT.Matrix.LoadFromDataSource()
            End If

            If (strEstado = "1" Or strEstado = "2") Then

                FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            Else
                FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            End If
            
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub CargaValoresUDF(ByVal oForm As Form)

        Dim strConsultaAprob As String = " select FldValue, Descr from UFD1 where TableID = 'QUT1' and FieldID in (select FieldID from CUFD where AliasID = 'SCGD_Aprobado') "

        Try

            hsAprobado.Clear()

            dtAprobado = oForm.DataSources.DataTables.Item("tAprobado")

            dtAprobado.ExecuteQuery(strConsultaAprob)

            hsAprobado.Clear()

            For i As Integer = 0 To dtAprobado.Rows.Count - 1
                hsAprobado.Add(dtAprobado.GetValue("FldValue", i), dtAprobado.GetValue("Descr", i))
            Next


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    Private Sub ActualizaDescripcionesPorUDF(ByRef dtRepuestos As SAPbouiCOM.DataTable,
                                  ByVal colAprobacion As String,
                                  ByVal oForm As Form)
        Dim keyAprob As String = ""
        Dim keyTras As String = ""

        Try
            CargaValoresUDF(oForm)
            For i As Integer = 0 To dtRepuestos.Rows.Count - 1

                keyAprob = hsAprobado(dtRepuestos.GetValue(colAprobacion, i).ToString())

                dtRepuestos.SetValue(colAprobacion, i, keyAprob)
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub IncluirGastosSeleccionados(ByVal dtSeleccionados As SAPbouiCOM.DataTable,
                                         ByVal Validacion As Boolean,
                                         ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim oEditText As EditText
        Dim Posicion As Integer = 0
        Dim dcPrecio As Decimal
        Dim strAprobacion As String
        Dim dcCantidad As Decimal
        Dim dcPrecioF As Decimal
        Dim strConsultaAprobaciones As String =
            " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap inner join [@SCGD_CONF_SUCURSAL] as csu on csu.DocEntry = cap.DocEntry " & _
            " where csu.U_Sucurs in ( select U_SCGD_idSucursal from [OQUT] where U_SCGD_Numero_OT = '{0}') " & _
            " and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] where U_SCGD_Numero_OT = '{0}')"
        Dim strNoOT As String = String.Empty

        Try
            ' If Validacion Then ValidaPrecios(dtSeleccionados, BubbleEvent)

            '   If Validacion Then Exit Try
            oForm = ApplicationSBO.Forms.Item("SCGD_AGOT")
            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
            Posicion = dtGastos.Rows.Count

            oEditText = DirectCast(oForm.Items.Item("txtNoOrden").Specific, EditText)
            strNoOT = oEditText.Value.Trim()

            CargaValoresUDF(oForm)

            If Utilitarios.EjecutarConsulta(
                String.Format(strConsultaAprobaciones, strNoOT),
                CompanySBO.CompanyDB,
                CompanySBO.Server).Trim() = "Y" Then

                strAprobacion = EstadosAprobacion.Aprobado
            Else
                strAprobacion = EstadosAprobacion.FaltoAprobacion
            End If

            For i As Integer = 0 To dtSeleccionados.Rows.Count - 1
                If dtSeleccionados.GetValue("sel", i) = "Y" Then
                    dtGastos.Rows.Add(1)

                    If Not String.IsNullOrEmpty(dtSeleccionados.GetValue("pre", i)) Then
                        dcPrecioF = Decimal.Parse(dtSeleccionados.GetValue("pre", i))
                    Else
                        dcPrecioF = 0
                    End If

                    dtGastos.SetValue("per", Posicion, My.Resources.Resource.No)
                    dtGastos.SetValue("cod", Posicion, dtSeleccionados.GetValue("cod", i))
                    dtGastos.SetValue("des", Posicion, dtSeleccionados.GetValue("des", i))
                    dtGastos.SetValue("mon", Posicion, strMoneda)
                    dtGastos.SetValue("can", Posicion, 1)

                    'dcPrecio = ManejoMultiMoneda(dcPrecioF,
                    '                              dtSeleccionados.GetValue("mon", i),
                    '                              strMoneda,
                    '                              dcTCCot)

                    dtGastos.SetValue("pre", Posicion, dcPrecioF.ToString(n))
                    dtGastos.SetValue("apr", Posicion, hsAprobado(strAprobacion))

                    Posicion += 1
                End If
            Next

            oForm.Items.Item("1").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oForm.Items.Item("btnDoc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            oMatrix.LoadFromDataSource()

            If oForm.Mode = BoFormMode.fm_OK_MODE Then
                oForm.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub EliminarGastosSeleccionados(ByVal FormUID As String)

        Dim oMatrix As IMatrix
        Dim oForm As Form

        Dim lsListaOrdenada As Generic.IList(Of Integer) = New Generic.List(Of Integer)

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)
            dtGastos = FormularioSBO.DataSources.DataTables.Item(strDTGastos)

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxGas").Specific, Matrix)
            oMatrix.FlushToDataSource()

            SeleccionarGastosCostosOT.OrdenaLista(lsListaEliminar, lsListaOrdenada)

            For Each Str As String In lsListaOrdenada
                Posicion = Integer.Parse(Str)

                If dtGastos.GetValue("sel", Posicion - 1).ToString = "Y" Then

                    Select Case dtGastos.GetValue("per", Posicion - 1).ToString
                        Case My.Resources.Resource.No
                            dtGastos.Rows.Remove(Posicion - 1)
                        Case My.Resources.Resource.Si
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeSeleccionGastos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            dtGastos.SetValue("sel", Posicion - 1, "N")
                    End Select
                End If
            Next

            ValidaCambios()

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub AgregaGastosDocumento(ByVal FormUID As String, ByVal Validacion As Boolean, ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim oForm As Form
        'Dim objCrearDocumento As New CrearDocumentosGastosCostos(ApplicationSBO, CompanySBO)
        Dim l_strNoOrder As String
        Dim l_strUnidCode As String
        Dim l_StrTipoOrden As String
        Dim l_strDocEntry As String

        Try

            oForm = ApplicationSBO.Forms.Item(FormUID)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)

            oMatrix.FlushToDataSource()

            l_strNoOrder = txtNoOrden.ObtieneValorUserDataSource()
            l_strUnidCode = txtNoUni.ObtieneValorUserDataSource()
            l_StrTipoOrden = txtTiOr.ObtieneValorUserDataSource()
            l_strDocEntry = txtDocE.ObtieneValorUserDataSource()

            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)

            m_oFormularioCrearDocumentos.IncluirGastosSeleccionados(dtGastos, l_strUnidCode, l_strNoOrder, l_StrTipoOrden, l_strDocEntry, Validacion, BubbleEvent)

            'If Not Validacion Then oForm.Close()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AplicaBusquedaCotizacion(ByVal FormUID As String)

        Dim oForm As Form
        Dim oItem As SAPbouiCOM.Item
        Try

            oForm = ApplicationSBO.Forms.Item(FormUID)
            oItem = oForm.Items.Item("btnBus")
            oItem.Click()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ActualizaCotizacion(ByVal FormUID As String)

        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim m_intDocEntry As Integer = 0
        Dim oForm As Form
        Dim intError As Integer
        Dim strMensaje As String
        Dim oMatrix As IMatrix
        Dim strAprobacion As String = String.Empty
        Dim strImpuestosRepuestos As String
        Dim strShipToCode As String
        Dim strConsultaAprobaciones As String =
        " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap inner join [@SCGD_CONF_SUCURSAL] as csu on csu.DocEntry = cap.DocEntry " & _
        " where csu.U_Sucurs in ( select U_SCGD_idSucursal from [OQUT] where U_SCGD_Numero_OT = '{0}') " & _
        " and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] where U_SCGD_Numero_OT = '{0}')"
        Dim m_strNoOrden As String = String.Empty
        Dim strCadenaConexionBDTaller As String = String.Empty
        Dim m_strIDSerieDocTrasnf As String = String.Empty
        Dim m_strDocEntrysTransfREP As String = String.Empty

        Dim blnProcesoLineas As Boolean = False

        Try


            oForm = ApplicationSBO.Forms.Item(FormUID)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, IMatrix)
            oMatrix.FlushToDataSource()

            If Not String.IsNullOrEmpty(txtDocE.ObtieneValorUserDataSource) Then
                m_intDocEntry = Integer.Parse(txtDocE.ObtieneValorUserDataSource())

                m_strNoOrden = txtNoOrden.ObtieneValorUserDataSource().Trim()
                oCotizacion = CargaObjetoCotizacion(m_intDocEntry)


                strImpuestosRepuestos = Utilitarios.EjecutarConsulta(String.Format(
                                                                     "select U_Imp_Gastos from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value),
                                                                        CompanySBO.CompanyDB,
                                                                        CompanySBO.Server).Trim()

                strShipToCode = Utilitarios.EjecutarConsulta(String.Format(
                                                             "select ShipToDef from OCRD crd inner join OQUT qut on crd.CardCode = qut.CardCode where qut.DocEntry = '{0}'", oCotizacion.DocEntry),
                                                                        CompanySBO.CompanyDB,
                                                                        CompanySBO.Server).Trim()

                If Not oCotizacion Is Nothing Then

                    oLineasCotizacion = oCotizacion.Lines

                    If Utilitarios.EjecutarConsulta(
                        String.Format(strConsultaAprobaciones, m_strNoOrden),
                        CompanySBO.CompanyDB,
                        CompanySBO.Server).Trim() = "Y" Then

                        strAprobacion = "1"
                    Else
                        strAprobacion = "3"
                    End If

                    For m As Integer = 0 To dtGastos.Rows.Count - 1

                        If dtGastos.GetValue("per", m) = My.Resources.Resource.Si Then
                            For k As Integer = 0 To oLineasCotizacion.Count - 1
                                oLineasCotizacion.SetCurrentLine(k)

                                If oLineasCotizacion.ItemCode = dtGastos.GetValue("cod", m) AndAlso
                                    oLineasCotizacion.LineNum = dtGastos.GetValue("lnum", m) Then

                                    oLineasCotizacion.UnitPrice = dtGastos.GetValue("pre", m)
                                    oLineasCotizacion.Quantity = dtGastos.GetValue("can", m)
                                    Exit For
                                End If
                            Next

                        ElseIf dtGastos.GetValue("per", m) = My.Resources.Resource.No Then
                            oLineasCotizacion.Add()
                            oLineasCotizacion.ItemCode = dtGastos.GetValue("cod", m)
                            oLineasCotizacion.ItemDescription = dtGastos.GetValue("des", m)
                            oLineasCotizacion.Quantity = Double.Parse(dtGastos.GetValue("can", m))
                            oLineasCotizacion.UnitPrice = Double.Parse(dtGastos.GetValue("pre", m).ToString())
                            oLineasCotizacion.TaxCode = strImpuestosRepuestos
                            oLineasCotizacion.VatGroup = strImpuestosRepuestos
                            oLineasCotizacion.DiscountPercent = 0
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = strAprobacion
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                            oLineasCotizacion.ShipToCode = strShipToCode
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NoOT").Value = txtNoOrden.ObtieneValorUserDataSource()

                        End If
                    Next

                    Dim cantidad As Integer

                    cantidad = oCotizacion.Lines.Count

                    If oCotizacion.Update() <> 0 Then
                        CompanySBO.GetLastError(intError, strMensaje)
                        If intError <> 0 Then
                            Throw New ExceptionsSBO(intError, strMensaje)
                        End If
                    End If

                    CargarGastos()
                End If
            End If

            ValidaCambios()

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        Finally
            m_blnActualizaCot = False
        End Try
    End Sub

    Private Function CargaObjetoCotizacion(ByVal p_NumCotizacion As Integer) As SAPbobsCOM.Documents

        Dim oCotizacion As SAPbobsCOM.Documents

        Try
            oCotizacion = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If oCotizacion.GetByKey(p_NumCotizacion) Then

                Return oCotizacion

            End If

        Catch ex As Exception

            Throw ex

        End Try
        Return Nothing
    End Function


#End Region

    Public Sub EjecutaBusqueda(ByVal FormUID As String)

        FormularioSBO = ApplicationSBO.Forms.Item(FormUID)
        FormularioSBO.Freeze(True)
        Dim oItem As SAPbouiCOM.Item
        oItem = FormularioSBO.Items.Item("btnBus")
        oItem.Click()
        FormularioSBO.Freeze(False)
    End Sub
End Class


