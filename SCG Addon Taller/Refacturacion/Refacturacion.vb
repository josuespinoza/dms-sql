Imports System.Globalization
Imports System.Collections.Generic
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO
Imports SCG.DMSOne.Framework
Imports SCG.SBOFramework.DI
Imports DMSOneFramework.SCGCommon




Partial Public Class Refacturacion

    Private strFacturas As String
    Private strFactViejas() As String
    Private strFactNuevas() As String
    Private strFecha As String
    Private oDataTable As SAPbouiCOM.DataTable
    Private strFechaContaContrato As String


    Public Sub ButtonSBOBuscarCFL(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Try

            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID
            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions
            ' Dim oDataTable As SAPbouiCOM.DataTable

            Dim strEstadoFacturado As String

            If pVal.BeforeAction = True Then

                strEstadoFacturado = Utilitarios.EjecutarConsulta("Select Max(U_Nivel) From [@SCGD_NIVELES_PV]", CompanySBO.CompanyDB, CompanySBO.Server)

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_Estado"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strEstadoFacturado
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_Reversa"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "N"
                oCondition.BracketCloseNum = 2

                oCFL.SetConditions(oConditions)

            ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    oDataTable = oCFLEvento.SelectedObjects

                    'strFechaContaContrato = oDataTable.GetValue("U_SCGD_FDc", 0)

                    EditTextContrato.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0))

                    'Call CargarFacturasRefacturar(False, oDataTable.GetValue("DocEntry", 0))


                    CheckBoxAutoFacturas.AsignaValorUserDataSource("N")
                    CheckBoxRefacturarTodos.AsignaValorUserDataSource("N")
                    EditTextAnoVeh.Especifico.Value = Nothing
                    'ButtonRefacturar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

                End If

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

    End Sub

    Private Sub CargarFacturasRefacturar(ByVal p_blnUsaFiltroFechas As Boolean, Optional ByVal strContrato As String = "", Optional ByVal p_strFechaInicio As String = "", Optional ByVal p_strFechaFin As String = "")

        Dim strConsulta As String = String.Empty
        Dim strFactura As String = String.Empty
        Dim strNoContrato As String = String.Empty
        Dim strFechaContContrato As String = String.Empty
        Dim dtFechaContContrato As DateTime
        Try

            MatrixFacturas.Matrix.Clear()
            dataTableFacturas.Rows.Clear()

            dataTableContrato.Rows.Clear()
            dataTableContrato = FormularioSBO.DataSources.DataTables.Item("Contrato")

            If p_blnUsaFiltroFechas = True Then
                strConsulta = "SELECT  OINV.DocEntry, CV.DocNum, CV.U_SCGD_FDc, CV.U_No_Fac FROM [OINV]INNER JOIN [@SCGD_CVENTA] as CV on OINV.U_SCGD_NoContrato = CV.DocNum Where DocStatus = 'O' And ObjType = '13' AND ((CV.U_SCGD_FDc >= '" & p_strFechaInicio & "' and cv.U_SCGD_FDc  <= '" & p_strFechaFin & "' )) order by CV.DocNum, OINV.DocEntry"
            Else
                If Not String.IsNullOrEmpty(strContrato) Then
                    strConsulta = "SELECT OINV.DocEntry, CV.DocNum, CV.U_SCGD_FDc, CV.U_No_Fac FROM [OINV]INNER JOIN [@SCGD_CVENTA] as CV on OINV.U_SCGD_NoContrato = CV.DocNum Where U_SCGD_NoContrato = '" & strContrato & "' And DocStatus = 'O' And ObjType = '13' order by CV.DocNum, OINV.DocEntry"
                Else
                    Exit Sub
                End If
            End If

            'strConsulta = "SELECT DocEntry From [OINV] Where U_SCGD_NoContrato = '" & strContrato & "' And DocStatus = 'O' And ObjType = '13'"


            dataTableContrato.ExecuteQuery(strConsulta)

            If dataTableContrato.Rows.Count > 0 Then
                For i As Integer = 0 To dataTableContrato.Rows.Count - 1

                    strFactura = dataTableContrato.GetValue("DocEntry", i)
                    strNoContrato = dataTableContrato.GetValue("DocNum", i)
                    strFechaContContrato = dataTableContrato.GetValue("U_SCGD_FDc", i)

                    If Not String.IsNullOrEmpty(strFactura) AndAlso Not strFactura = "0" Then

                        dataTableFacturas.Rows.Add()
                        dataTableFacturas.SetValue("vieja", i, strFactura)


                        If Not String.IsNullOrEmpty(strNoContrato) AndAlso Not strNoContrato = "0" Then

                            dataTableFacturas.SetValue("NoContrato", i, strNoContrato)

                        End If

                        If Not String.IsNullOrEmpty(strFechaContContrato) Then

                            dtFechaContContrato = Convert.ToDateTime(strFechaContContrato)
                            dataTableFacturas.SetValue("FechaContabilizacion", i, dtFechaContContrato)

                        End If

                        dataTableFacturas.SetValue("col_Refac", i, "N")
                        dataTableFacturas.SetValue("NumFactura", i, dataTableContrato.GetValue("U_No_Fac", i))

                    End If


                Next

                MatrixFacturas.Matrix.LoadFromDataSource()

                If dataTableFacturas.Rows.Count > 0 Then
                    ButtonRefacturar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                Else
                    ButtonRefacturar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                End If
            End If



        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

    End Sub


    Private Sub CargarFacturasRefacturarByFecha(ByRef BubbleEvent As Boolean)
        Try
            Dim strFechaInicio As String
            Dim strFechaFin As String

            strFechaInicio = EditTextFechaInicio.ObtieneValorUserDataSource().ToString()
            strFechaFin = EditTextFechaFin.ObtieneValorUserDataSource().ToString()

            If String.IsNullOrEmpty(strFechaInicio) Then

                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaInicio, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Exit Sub

            Else
                If String.IsNullOrEmpty(strFechaFin) Then

                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaFin, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                Else

                    CargarFacturasRefacturar(True, "", strFechaInicio, strFechaFin)

                    EditTextContrato.AsignaValorUserDataSource("")
                    CheckBoxAutoFacturas.AsignaValorUserDataSource("N")
                    CheckBoxRefacturarTodos.AsignaValorUserDataSource("N")
                    EditTextAnoVeh.Especifico.Value = Nothing
                End If
            End If
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try
    End Sub


    Public Sub ButtonSBORefacturar(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim intLineaFactura As Integer
        Dim strRefacturarTodas As String
        Dim strFactura As String
        Dim strConsulta As String
        Dim strFacturaMsj As String = ""

        Dim strFechaRefactura As String
        Dim dtFechaRefactura As Date
        Dim tsDifDias As TimeSpan
        Dim intDiasDif As Integer
        Dim dtFechaContaContrato As Date
        Dim strEstadoPeriodo As String = ""

        Dim strSeleccion As String = ""
        Dim blnValidaRefacturar As Boolean = False
        Dim intContSelecciones As Integer = 0

        Dim strFechaContaCV As String

        Try

            intLineaFactura = MatrixFacturas.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            strRefacturarTodas = CheckBoxRefacturarTodos.ObtieneValorUserDataSource()

            If dataTableFacturas.Rows.Count > 0 Then

                For i As Integer = 0 To dataTableFacturas.Rows.Count - 1


                    strSeleccion = dataTableFacturas.GetValue("col_Refac", i)

                    If strSeleccion = "Y" Then
                        intContSelecciones = intContSelecciones + 1
                    End If

                Next

                If intContSelecciones > 0 Or strRefacturarTodas = "Y" Then
                    blnValidaRefacturar = True
                Else
                    blnValidaRefacturar = False
                End If
            Else
                blnValidaRefacturar = False
            End If



            If pVal.BeforeAction = True AndAlso pVal.ActionSuccess = False Then '#1

                If blnValidaRefacturar = False AndAlso Not strRefacturarTodas = "Y" Then '#2  intLineaFactura = -1

                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorLineaRefacturar, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                    Exit Sub

                Else

                    strFecha = EditTextFecha.ObtieneValorUserDataSource().ToString()

                    If String.IsNullOrEmpty(strFecha) Then '#3

                        BubbleEvent = False
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaRefacturacion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        Exit Sub

                    Else
                        strFechaRefactura = strFecha

                        'Incluir aqui validacion 



                        For i As Integer = 0 To dataTableFacturas.Rows.Count - 1

                            strSeleccion = ""
                            strSeleccion = dataTableFacturas.GetValue("col_Refac", i)

                            If strSeleccion = "Y" Or strRefacturarTodas = "Y" Then
                                strFechaContaCV = dataTableFacturas.GetValue("FechaContabilizacion", i)

                                'Incluir aqui validacion 
                                If Not String.IsNullOrEmpty(strFechaContaCV) Then
                                    dtFechaContaContrato = Convert.ToDateTime(strFechaContaCV)
                                    dtFechaContaContrato = New Date(dtFechaContaContrato.Year, dtFechaContaContrato.Month, dtFechaContaContrato.Day, 0, 0, 0)


                                    If Not String.IsNullOrEmpty(strFechaRefactura) Then
                                        dtFechaRefactura = Date.ParseExact(strFechaRefactura, "yyyyMMdd", Nothing)
                                        dtFechaRefactura = New Date(dtFechaRefactura.Year, dtFechaRefactura.Month, dtFechaRefactura.Day, 0, 0, 0)

                                        tsDifDias = dtFechaRefactura - dtFechaContaContrato
                                        intDiasDif = tsDifDias.Days
                                    End If
                                End If

                                If intDiasDif < 0 Then
                                    BubbleEvent = False
                                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaRefacturacionNC, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                    Exit Sub
                                End If
                            End If



                        Next



                        If dtFechaRefactura > Now.Date Then

                            BubbleEvent = False
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaRefacturacionPosterior, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            Exit Sub

                        Else

                            strEstadoPeriodo = Utilitarios.EjecutarConsulta("SELECT PeriodStat FROM dbo.[OFPR] WHERE '" & dtFechaRefactura.ToString("yyyyMMdd") & "' >= F_RefDate AND '" & dtFechaRefactura.ToString("yyyyMMdd") & "' <= T_RefDate", CompanySBO.CompanyDB, CompanySBO.Server)

                            If strEstadoPeriodo <> "N" Then

                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorFechaRefacturacionPeriodoContable, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub

                            End If

                            strFacturas = ""

                            If strRefacturarTodas = "Y" Then '#4

                                For i As Integer = 0 To MatrixFacturas.Matrix.RowCount - 1

                                    strFactura = dataTableFacturas.GetValue("vieja", i)

                                    If i = 0 Then

                                        strFacturas = strFactura

                                    Else

                                        strFacturas = strFacturas & "," & strFactura

                                    End If

                                Next



                            Else
                                'Original inicio
                                'strFacturas = dataTableFacturas.GetValue("vieja", intLineaFactura - 1)
                                'Original Fin

                                For i As Integer = 0 To dataTableFacturas.Rows.Count - 1
                                    strSeleccion = ""

                                    strSeleccion = dataTableFacturas.GetValue("col_Refac", i)

                                    If strSeleccion = "Y" Then
                                        strFactura = dataTableFacturas.GetValue("vieja", i)

                                        If strFacturas = "" Then

                                            strFacturas = strFactura
                                        Else
                                            strFacturas = strFacturas & "," & strFactura

                                        End If

                                    End If

                                Next

                            End If '#4

                            If Not String.IsNullOrEmpty(strFacturas) Then '#5

                                'Pagos asociados a la factura

                                strConsulta = "SELECT OINV.DocEntry AS Factura, RCT2.DocNum AS Valor " & _
                                                "FROM INV1 INNER JOIN OINV " & _
                                                "ON INV1.DocEntry = OINV.DocEntry " & _
                                                "INNER JOIN RCT2 ON INV1.DocEntry = RCT2.DocEntry " & _
                                                "INNER JOIN ORCT ON RCT2.DocNum = ORCT.DocEntry " & _
                                                "WHERE (ORCT.Canceled = 'N') AND RCT2.InvType = 13 AND INV1.DocEntry IN(" & strFacturas & ") AND OINV.DocEntry IN(" & strFacturas & ")"

                                Call EjecutaConsultaValidacion(BubbleEvent, strFacturaMsj, strConsulta)

                                If BubbleEvent = False Then

                                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & strFacturaMsj & " " & My.Resources.Resource.MensajeFacturaReversionUltimoPago, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                                    Exit Sub

                                End If

                                'Documentos que provienen de la factura

                                strConsulta = "SELECT OINV.DocEntry AS Factura, INV1.TrgetEntry AS Valor " & _
                                                "FROM OINV INNER JOIN " & _
                                                "INV1 ON OINV.DocEntry = INV1.DocEntry " & _
                                                "WHERE OINV.DocEntry IN(" & strFacturas & ") AND OINV.DocType = 'I'"

                                Call EjecutaConsultaValidacion(BubbleEvent, strFacturaMsj, strConsulta)

                                If BubbleEvent = False Then

                                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeFacturaReversion & " " & strFacturaMsj & " " & My.Resources.Resource.MensajeFacturaReversionUltimoNotaCredito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                                    Exit Sub

                                End If

                            ElseIf String.IsNullOrEmpty(strFacturas) Then

                                BubbleEvent = False
                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorNoLineasRefact, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                                Exit Sub

                            End If '#5


                        End If




                    End If '#3

                End If '#2

            ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

                Call GenerarRefacturacion(strRefacturarTodas, intLineaFactura)

            End If '#1

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

    End Sub

    Private Sub GenerarRefacturacion(ByVal strRefacturarTodas As String, ByVal intLineaFactura As Integer)

        Dim strFactura As String
        Dim strConsulta As String
        Dim intCantFacts As Integer
        Dim blnVarias As Boolean
        Dim fact As String
        Dim strFactUnidad As String
        Dim intPosArray As Integer = 0
        Dim strNoContrato As String

        Dim strSeleccion As String = ""
        Dim strAnoVehi As String = ""

        Dim strNoFactura As String = String.Empty

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oDataChilds As SAPbobsCOM.GeneralDataCollection
        Dim oChild As SAPbobsCOM.GeneralData

        Try

            CompanySBO.StartTransaction()

            'intCantFacts = MatrixFacturas.Matrix.RowCount
            'If intCantFacts = 1 Then
            '    blnVarias = False
            'Else
            '    blnVarias = True
            'End If

            If strRefacturarTodas = "Y" Then


                For i As Integer = 0 To MatrixFacturas.Matrix.RowCount - 1

                    strFactura = dataTableFacturas.GetValue("vieja", i)

                    strNoContrato = dataTableFacturas.GetValue("NoContrato", i)

                    'Numero de Factura ligado al Contrato
                    strNoFactura = dataTableFacturas.GetValue("NumFactura", i)

                    If Not String.IsNullOrEmpty(strFactura) And Not String.IsNullOrEmpty(strNoContrato) Then

                        If Not IsNumeric(strNoFactura) Then
                            blnVarias = True
                        Else
                            blnVarias = False
                        End If

                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ProcesandoRefacturacion & strFactura, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Call Refacturar(blnVarias, strFactura, i, i, strNoContrato)

                    End If



                Next


            Else

                ' Call Refacturar(blnVarias, strFacturas, intLineaFactura - 1, 0)

                For i As Integer = 0 To dataTableFacturas.Rows.Count - 1
                    strSeleccion = ""

                    strSeleccion = dataTableFacturas.GetValue("col_Refac", i)

                    'Numero de Factura ligado al Contrato
                    strNoFactura = dataTableFacturas.GetValue("NumFactura", i)

                    If strSeleccion = "Y" Then
                        strFactura = dataTableFacturas.GetValue("vieja", i)
                        strNoContrato = dataTableFacturas.GetValue("NoContrato", i)

                        If Not String.IsNullOrEmpty(strFactura) And Not String.IsNullOrEmpty(strNoContrato) Then

                            If Not IsNumeric(strNoFactura) Then
                                blnVarias = True
                            Else
                                blnVarias = False
                            End If

                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ProcesandoRefacturacion & strFactura, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            Call Refacturar(blnVarias, strFactura, i, i, strNoContrato)
                        End If

                    Else

                    End If

                Next

            End If

            MatrixFacturas.Matrix.LoadFromDataSource()

            dataTableUnidades.Rows.Clear()
            dataTableUnidades = FormularioSBO.DataSources.DataTables.Item("Unidades")

            strConsulta = "Select Code, U_NUMFAC From [@SCGD_VEHICULO] Where U_NUMFAC IN(" & strFacturas & ")"

            dataTableUnidades.ExecuteQuery(strConsulta)
            
            For i As Integer = 0 To dataTableUnidades.Rows.Count - 1
                'Obtener año de vehiculo
                strAnoVehi = EditTextAnoVeh.ObtieneValorUserDataSource()

                strFactUnidad = dataTableUnidades.GetValue("U_NUMFAC", i)

                intPosArray = 0


                For Each fact In strFactViejas

                    If strFactUnidad = fact Then

                        oCompanyService = CompanySBO.GetCompanyService()
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
                        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                        oGeneralParams.SetProperty("Code", dataTableUnidades.GetValue("Code", i))
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                        oGeneralData.SetProperty("U_NUMFAC", strFactNuevas(intPosArray))

                        If Not String.IsNullOrEmpty(strAnoVehi) Then
                            oGeneralData.SetProperty("U_Ano_Vehi", strAnoVehi)
                        End If

                        oDataChilds = oGeneralData.Child("SCGD_VEHITRAZA")

                        For Each oChild In oDataChilds
                            oChild.SetProperty("U_NumFac_V", strFactNuevas(intPosArray))
                        Next

                        oGeneralService.Update(oGeneralData)
                        Exit For

                    End If

                    intPosArray += 1

                Next

            Next

            ButtonRefacturar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            
            CheckBoxRefacturarTodos.AsignaValorUserDataSource("N")

            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.OperacionFinalizada, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)

            CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

    End Sub

    Private Sub EjecutaConsultaValidacion(ByRef blnFacturar As Boolean, ByRef strFactura As String, ByVal strConsulta As String)

        Dim strFact As String
        Dim intFact As Integer = 0
        Dim strEstado As String = ""
        Dim strValor As String

        Try

            dataTableValidaFact.Rows.Clear()
            dataTableValidaFact = FormularioSBO.DataSources.DataTables.Item("Valida")

            dataTableValidaFact.ExecuteQuery(strConsulta)

            If dataTableValidaFact.Rows.Count > 0 Then

                For i As Integer = 0 To dataTableValidaFact.Rows.Count - 1

                    strFact = dataTableValidaFact.GetValue("Factura", i)

                    If Not String.IsNullOrEmpty(strFact) Then

                        intFact = Integer.Parse(strFact)

                        If intFact > 0 Then

                            strValor = dataTableValidaFact.GetValue("Valor", i)

                            If Not String.IsNullOrEmpty(strValor) AndAlso Not strValor = "0" Then

                                strFactura = dataTableValidaFact.GetValue("Factura", i)

                                blnFacturar = False

                                Exit Sub

                            End If

                        End If

                    End If

                Next

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

    End Sub

    Private Sub Refacturar(ByVal blnVarias As Boolean, ByVal strFactura As String, ByVal intPosicion As Integer, ByVal intContador As Integer, ByVal p_strContrato As String)

        Dim oFactura As SAPbobsCOM.Documents
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNuevaFactura As String = ""
        'Dim strContrato As String
        Dim dtFecha As Date

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        'strContrato = EditTextContrato.ObtieneValorUserDataSource()

        If Not String.IsNullOrEmpty(strFecha) Then
            dtFecha = Date.ParseExact(strFecha, "yyyyMMdd", Nothing)
            dtFecha = New Date(dtFecha.Year, dtFecha.Month, dtFecha.Day, 0, 0, 0)
        End If

        oFactura = CType(CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

        If oFactura.GetByKey(CInt(strFactura)) Then

            'Actualiza Factura Vieja quitandole el contrato de venta asociado

            oFactura.UserFields.Fields.Item("U_SCGD_NoContrato").Value = ""

            If oFactura.Update() <> 0 Then
                CompanySBO.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            End If

            'Nota de Crédito de Reversión

            Call GenerarNotaCreditoReversion(oFactura, intPosicion, dtFecha)

            'Nueva Factura de Venta

            Call GenerarNuevaFactura(oFactura, intPosicion, strNuevaFactura, dtFecha, p_strContrato)

            'Ligar Nueva Factura a Contrato de Venta
            oCompanyService = CompanySBO.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strContrato)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            
            If blnVarias = False Then
                oGeneralData.SetProperty("U_No_Fac", strNuevaFactura)
            End If

            'oGeneralData.SetProperty("U_SCGD_FDc", dtFecha)


            oGeneralService.Update(oGeneralData)

            
            'Carga de arreglos de facturas viejas y nuevas para modificar unidades

            ReDim Preserve strFactViejas(intContador)
            strFactViejas(intContador) = strFactura

            ReDim Preserve strFactNuevas(intContador)
            strFactNuevas(intContador) = strNuevaFactura

        End If

    End Sub

    Private Sub GenerarNotaCreditoReversion(ByVal oFactura As SAPbobsCOM.Documents, ByVal intPosicion As Integer, ByVal dtFecha As Date)

        Dim objDocumentoNC As SAPbobsCOM.Documents
        Dim objFacturaLines As SAPbobsCOM.Document_Lines
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNotaCredito As String

        objDocumentoNC = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)

        If Not oFactura.DocumentStatus = BoStatus.bost_Close Then

            Dim strIndicador As String = Utilitarios.DevuelveCodIndicadores(ApplicationSBO, "3")
            If Not String.IsNullOrEmpty(strIndicador) Then
                objDocumentoNC.Indicator = strIndicador
            End If
            objDocumentoNC.DocDate = dtFecha
            objDocumentoNC.PaymentGroupCode = oFactura.PaymentGroupCode
            objDocumentoNC.Comments = My.Resources.Resource.ComentarioReversaFact & oFactura.DocEntry

            objFacturaLines = oFactura.Lines

            For i As Integer = 0 To objFacturaLines.Count - 1

                objFacturaLines.SetCurrentLine(i)

                With objDocumentoNC

                    .Lines.Quantity = objFacturaLines.Quantity
                    .Lines.BaseType = 13
                    .Lines.BaseLine = objFacturaLines.LineNum
                    .Lines.BaseEntry = oFactura.DocEntry

                    objDocumentoNC.Lines.Add()

                End With

            Next

            For i As Integer = 0 To oFactura.Expenses.Count - 1

                oFactura.Expenses.SetCurrentLine(i)

                If oFactura.Expenses.LineTotal > 0 Then
                    With objDocumentoNC

                        .Expenses.BaseDocEntry = oFactura.DocEntry
                        .Expenses.BaseDocLine = oFactura.Expenses.LineNum
                        .Expenses.BaseDocType = 13

                        objDocumentoNC.Expenses.Add()

                    End With
                End If

            Next

            Dim Verificar As Integer = objDocumentoNC.Add

            If Verificar <> 0 Then

                CompanySBO.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)

            Else

                strNotaCredito = CompanySBO.GetNewObjectKey
                dataTableFacturas.SetValue("reversa", intPosicion, strNotaCredito)

            End If

        End If

    End Sub

    Private Sub GenerarNuevaFactura(ByVal oFactura As SAPbobsCOM.Documents, ByVal intPosicion As Integer, ByRef strNuevaFactura As String, ByVal dtFecha As Date, ByVal p_strContrato As String)

        Dim oNuevaFactura As SAPbobsCOM.Documents
        'Dim strContrato As String
        Dim intError As Integer
        Dim strMensajeError As String = ""

        'strContrato = EditTextContrato.ObtieneValorUserDataSource()

        oNuevaFactura = CType(CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)

        If Not oFactura.DocumentStatus = BoStatus.bost_Close Then

            oNuevaFactura.DocType = oFactura.DocType
            oNuevaFactura.CardCode = oFactura.CardCode
            oNuevaFactura.Comments = oFactura.Comments
            oNuevaFactura.DocCurrency = oFactura.DocCurrency
            oNuevaFactura.PaymentGroupCode = oFactura.PaymentGroupCode
            oNuevaFactura.DocDate = dtFecha
            oNuevaFactura.Series = oFactura.Series
            oNuevaFactura.SalesPersonCode = oFactura.SalesPersonCode
            oNuevaFactura.DiscountPercent = oFactura.DiscountPercent
            oNuevaFactura.Indicator = oFactura.Indicator
            oNuevaFactura.UserFields.Fields.Item("U_SCGD_NoContrato").Value = p_strContrato
            oNuevaFactura.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = oFactura.UserFields.Fields.Item("U_SCGD_Num_Placa").Value
            oNuevaFactura.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = oFactura.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value

            For i As Integer = 0 To oFactura.Lines.Count - 1

                oFactura.Lines.SetCurrentLine(i)

                With oNuevaFactura

                    .Lines.UserFields.Fields.Item("U_SCGD_Cod_Unid").Value = oFactura.Lines.UserFields.Fields.Item("U_SCGD_Cod_Unid").Value
                    .Lines.ItemCode = oFactura.Lines.ItemCode
                    .Lines.ItemDescription = oFactura.Lines.ItemDescription
                    .Lines.TaxCode = oFactura.Lines.TaxCode
                    .Lines.VatGroup = oFactura.Lines.VatGroup
                    .Lines.AccountCode = oFactura.Lines.AccountCode
                    .Lines.DiscountPercent = oFactura.Lines.DiscountPercent
                    .Lines.UnitPrice = oFactura.Lines.UnitPrice

                    oNuevaFactura.Lines.Add()

                End With

            Next

            For i As Integer = 0 To oFactura.Expenses.Count - 1

                oFactura.Expenses.SetCurrentLine(i)

                If oFactura.Expenses.LineTotal > 0 Then

                    With oNuevaFactura

                        .Expenses.ExpenseCode = oFactura.Expenses.ExpenseCode
                        .Expenses.TaxCode = oFactura.Expenses.TaxCode
                        .Expenses.VatGroup = oFactura.Expenses.TaxCode
                        .Expenses.LineTotal = oFactura.Expenses.LineTotal

                        oNuevaFactura.Expenses.Add()

                    End With

                End If

            Next

            Dim Verificar As Integer = oNuevaFactura.Add

            If Verificar <> 0 Then

                CompanySBO.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)

            Else

                strNuevaFactura = CompanySBO.GetNewObjectKey
                dataTableFacturas.SetValue("nueva", intPosicion, strNuevaFactura)

            End If

        End If

    End Sub

    Private Sub HabilitaCampos()
        Dim strUsaFiltroCV As String

        strUsaFiltroCV = CheckBoxUsaFiltroCV.ObtieneValorUserDataSource()

        If Not strUsaFiltroCV = "Y" Then
            EditTextFechaInicio.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            EditTextFechaFin.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            ButtonBuscar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            'ButtonCargar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            EditTextFechaInicio.AsignaValorUserDataSource("")
            EditTextFechaFin.AsignaValorUserDataSource("")
        Else
            EditTextFechaInicio.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            EditTextFechaFin.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            ButtonBuscar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            ' ButtonCargar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            EditTextContrato.AsignaValorUserDataSource("")
        End If


    End Sub

    Private Sub SeleccionFacturas(ByRef pVal As SAPbouiCOM.ItemEvent)
        Dim strAutoFacturas As String
        Dim strNoContratoVentaBase As String
        Dim strNoContratoVentaTemp As String

        strAutoFacturas = CheckBoxAutoFacturas.ObtieneValorUserDataSource()


        Dim PosicionMatriz As Integer = 0
        PosicionMatriz = pVal.Row - 1

        If dataTableFacturas.Rows.Count > PosicionMatriz And dataTableFacturas.Rows.Count > 0 Then
            If dataTableFacturas.GetValue("col_Refac", PosicionMatriz) = "N" Then
                dataTableFacturas.SetValue("col_Refac", PosicionMatriz, "Y")

                If strAutoFacturas = "Y" Then
                    strNoContratoVentaBase = dataTableFacturas.GetValue("NoContrato", PosicionMatriz)

                    For i As Integer = 0 To dataTableFacturas.Rows.Count - 1
                        strNoContratoVentaTemp = dataTableFacturas.GetValue("NoContrato", i)
                        If strNoContratoVentaBase = strNoContratoVentaTemp Then
                            dataTableFacturas.SetValue("col_Refac", i, "Y")
                        End If
                    Next

                End If

            ElseIf dataTableFacturas.GetValue("col_Refac", PosicionMatriz) = "Y" Then
                dataTableFacturas.SetValue("col_Refac", PosicionMatriz, "N")
            End If

            MatrixFacturas.Matrix.LoadFromDataSource()

        End If


    End Sub


    Private Sub SeleccionTodasFacturas()
        Dim strRefacturarTodos As String

        strRefacturarTodos = CheckBoxRefacturarTodos.ObtieneValorUserDataSource()


        If dataTableFacturas.Rows.Count > 0 And strRefacturarTodos = "Y" Then

            For i As Integer = 0 To dataTableFacturas.Rows.Count - 1
                dataTableFacturas.SetValue("col_Refac", i, "Y")
            Next

            MatrixFacturas.Matrix.LoadFromDataSource()

        ElseIf dataTableFacturas.Rows.Count > 0 And strRefacturarTodos = "N" Then
            For i As Integer = 0 To dataTableFacturas.Rows.Count - 1
                dataTableFacturas.SetValue("col_Refac", i, "N")
            Next

            MatrixFacturas.Matrix.LoadFromDataSource()
        End If







    End Sub


    Private Sub BusquedaFacturas(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim strUsaFiltroCV As String

        strUsaFiltroCV = CheckBoxUsaFiltroCV.ObtieneValorUserDataSource()

        FormularioSBO.Freeze(True)

        If Not strUsaFiltroCV = "Y" Then
            CargarFacturasRefacturarByFecha(BubbleEvent)
        Else

            Dim strNoCV As String = EditTextContrato.ObtieneValorUI()
            If Not String.IsNullOrEmpty(strNoCV) Then
                CargarFacturasRefacturar(False, strNoCV)
                ' ButtonSBOBuscarCFL(FormUID, pVal, BubbleEvent)
            End If

        End If

        FormularioSBO.Freeze(False)

    End Sub

End Class
