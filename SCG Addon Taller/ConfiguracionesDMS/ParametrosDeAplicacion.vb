Imports System.Linq
Imports SAPbouiCOM

Partial Public Class ParametrosDeAplicacion

    Public Sub CFLItem(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

        Try
            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            Dim sCFL_ID As String
            sCFL_ID = oCFLEvento.ChooseFromListUID
            Dim oCFL As SAPbouiCOM.ChooseFromList
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions

            Dim oDataTable As SAPbouiCOM.DataTable

            If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    EditTextArtCotizacion.AsignaValorDataSource("")

                    oDataTable = oCFLEvento.SelectedObjects

                    EditTextArtCotizacion.AsignaValorDataSource(oDataTable.GetValue("ItemCode", 0))

                    If Not FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then
                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                    End If

                End If

            ElseIf pVal.BeforeAction = True Then

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "1"
                oCondition.BracketCloseNum = 1
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "2"
                oCondition.BracketCloseNum = 2
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 3
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "3"
                oCondition.BracketCloseNum = 3
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 4
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "4"
                oCondition.BracketCloseNum = 4
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 5
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "5"
                oCondition.BracketCloseNum = 5
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 6
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "6"
                oCondition.BracketCloseNum = 6
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 7
                oCondition.Alias = "U_SCGD_TipoArticulo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "10"
                oCondition.BracketCloseNum = 7

                oCFL.SetConditions(oConditions)

            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Sub ButtonVerOfertaCompraItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            oForm = m_oApplication.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_NSE", False, m_oApplication) Then
                Dim objNumeracion As New NumeracionSeries(m_oCompany, m_oApplication)
                objNumeracion.FormConfiguracion = oForm
                objNumeracion.IntTipoConfiguracion = TipoConfiguracionSerie.OfertaCompra
                Call objNumeracion.CargaFormularioSeries()
            End If

        End If

    End Sub

    Public Sub ButtonVerOrdenCompraItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            oForm = m_oApplication.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_NSE", False, m_oApplication) Then
                Dim objNumeracion As New NumeracionSeries(m_oCompany, m_oApplication)
                objNumeracion.FormConfiguracion = oForm
                objNumeracion.IntTipoConfiguracion = TipoConfiguracionSerie.OrdenCompra
                Call objNumeracion.CargaFormularioSeries()
            End If

        End If

    End Sub

    Public Sub ButtonVerOfertaVentaItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            oForm = m_oApplication.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_NSE", False, m_oApplication) Then
                Dim objNumeracion As New NumeracionSeries(m_oCompany, m_oApplication)
                objNumeracion.FormConfiguracion = oForm
                objNumeracion.IntTipoConfiguracion = TipoConfiguracionSerie.OfertaVenta
                Call objNumeracion.CargaFormularioSeries()
            End If

        End If

    End Sub

    Public Sub ButtonVerOrdenVentaItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            oForm = m_oApplication.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_NSE", False, m_oApplication) Then
                Dim objNumeracion As New NumeracionSeries(m_oCompany, m_oApplication)
                objNumeracion.FormConfiguracion = oForm
                objNumeracion.IntTipoConfiguracion = TipoConfiguracionSerie.OrdenVenta
                Call objNumeracion.CargaFormularioSeries()
            End If

        End If

    End Sub

    Public Sub ButtonVerBodegaInvItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            oForm = m_oApplication.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_NSE", False, m_oApplication) Then
                Dim objNumeracion As New NumeracionSeries(m_oCompany, m_oApplication)
                objNumeracion.FormConfiguracion = oForm
                objNumeracion.IntTipoConfiguracion = TipoConfiguracionSerie.InvBodega
                Call objNumeracion.CargaFormularioSeries()
            End If

        End If

    End Sub

    Public Sub ButtonCrearItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim l_fhaInicio As Date
        Dim l_fhaFinal As Date


        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
            If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then
                If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then blnCargaConf=True
                    Dim strSucursal As String
                    Dim strHoraInicio As String = EditTextHoraInicio.ObtieneValorDataSource()
                    Dim strHoraFinal As String = EditTextHoraFin.ObtieneValorDataSource()

                    strSucursal = ComboBoxSucursal.ObtieneValorDataSource()


                    If String.IsNullOrEmpty(strSucursal) Then
                        BubbleEvent = False
                        m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorFaltaSucursalConfSucursal, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    ElseIf String.IsNullOrEmpty(strHoraInicio) Then
                        BubbleEvent = False
                        m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorConfSucursalFaltaHorario, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    ElseIf String.IsNullOrEmpty(strHoraFinal) Then
                        BubbleEvent = False
                        m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorConfSucursalFaltaHorario, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                    ElseIf Not String.IsNullOrEmpty(strHoraInicio) And Not String.IsNullOrEmpty(strHoraFinal) Then

                        l_fhaInicio = DateTime.Parse("1900-01-01" & " " & FormatoHora(EditTextHoraInicio.ObtieneValorDataSource()))
                        l_fhaFinal = DateTime.Parse("1900-01-01" & " " & FormatoHora(EditTextHoraFin.ObtieneValorDataSource()))

                        If l_fhaInicio > l_fhaFinal Then
                            BubbleEvent = False
                            m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorConfSucursalHoraInvalida, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        End If

                    Else
                        If pVal.FormMode = BoFormMode.fm_ADD_MODE Then
                            If Not DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confS) confS.DocEntry = strSucursal.Trim) Then

                                BubbleEvent = False
                                m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorYaExisteConfSucursal, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                            End If
                        End If
                    End If

                End If
            ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess = True Then

                If FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                    FormularioSBO.Items.Item(ComboBoxSucursal.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                If blnCargaConf AndAlso Not String.IsNullOrEmpty(ComboBoxSucursal.ObtieneValorDataSource()) Then
                    blnCargaConf = False
                    DMS_Connector.Configuracion.Carga_Configuracion_SucursalEspecifica(ComboBoxSucursal.ObtieneValorDataSource())
                    CargarComboTipoOT_PorSucursal()
                End If
                End If
            End If
    End Sub

    Public Sub FormFormClose(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then

            Dim oFormNumeraS As SAPbouiCOM.Form

            Try
                oFormNumeraS = m_oApplication.Forms.GetForm("SCGD_NSE", 0)

                If Not oFormNumeraS Is Nothing Then
                    oFormNumeraS.Close()
                End If

            Catch ex As Exception

            End Try

        End If

    End Sub

    Private Function FormatoHora(ByVal p_Hora As String) As String
        Try
            Select Case p_Hora.Length
                Case 3
                    p_Hora = "0" & p_Hora
            End Select
            p_Hora = p_Hora.Insert(2, ":")
            Return p_Hora
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Private Sub ButtonAgregarLinAprobItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strCodPrimerItem As String
        Dim intPosicion As Integer

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            oForm = m_oApplication.Forms.Item(FormUID)
            oMatrizAprobacioens = DirectCast(oForm.Items.Item(mc_strmtx_Aprobacion).Specific, SAPbouiCOM.Matrix)
            oMatrizAprobacioens.FlushToDataSource()
            intPosicion = oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).Size

            If intPosicion = 1 Then
                strCodPrimerItem = oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).GetValue("U_TipoOT", 0)
                strCodPrimerItem = strCodPrimerItem.Trim()
                If String.IsNullOrEmpty(strCodPrimerItem) Then
                    intPosicion = 0
                    oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_ItmAprob", intPosicion, "N")
                    oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_EspAprob", intPosicion, "N")
                Else
                    intPosicion = 1
                    oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).InsertRecord(intPosicion)
                    oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_ItmAprob", intPosicion, "N")
                    oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_EspAprob", intPosicion, "N")
                End If
            Else
                oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).InsertRecord(intPosicion)
                oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_ItmAprob", intPosicion, "N")
                oForm.DataSources.DBDataSources.Item(tablaConfigAprobaciones).SetValue("U_EspAprob", intPosicion, "N")
            End If

            oMatrizAprobacioens.LoadFromDataSource()

        End If
    End Sub

    Private Sub ButtonEliminarLinAprobItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = m_oApplication.Forms.Item(FormUID)
        oMatriz = DirectCast(oform.Items.Item(mc_strmtx_Aprobacion).Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()

        Do While intRegistoEliminar > -1
            oform.DataSources.DBDataSources.Item(tablaConfigAprobaciones).RemoveRecord(intRegistoEliminar - 1)
            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)
        Loop

        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If
    End Sub

    Private Sub ButtonAddConfBodxCCItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strCodPrimerItem As String
        Dim intPosicion As Integer

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            oForm = m_oApplication.Forms.Item(FormUID)
            oMatrizBodegasCentroCosto = DirectCast(oForm.Items.Item(mc_strmtx_BCC).Specific, SAPbouiCOM.Matrix)
            oMatrizBodegasCentroCosto.FlushToDataSource()
            intPosicion = oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).Size

            If intPosicion = 1 Then
                strCodPrimerItem = oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).GetValue(mc_str_CentroCosto, 0)
                strCodPrimerItem = strCodPrimerItem.Trim()
                If String.IsNullOrEmpty(strCodPrimerItem) Then
                    intPosicion = 0
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Repuestos, intPosicion, String.Empty)
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Servicios, intPosicion, String.Empty)
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Suministros, intPosicion, String.Empty)
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_ServiciosExternos, intPosicion, String.Empty)
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).SetValue(mc_str_Proceso, intPosicion, String.Empty)
                Else
                    intPosicion = 1
                    oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).InsertRecord(intPosicion)
                End If
            Else
                oForm.DataSources.DBDataSources.Item(tablaConfigBodegasCC).InsertRecord(intPosicion)
            End If

            oMatrizBodegasCentroCosto.LoadFromDataSource()

        End If
    End Sub

    Private Sub ButtonDelConfBodxCCItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = m_oApplication.Forms.Item(FormUID)
        oMatriz = DirectCast(oform.Items.Item(mc_strmtx_BCC).Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()

        Do While intRegistoEliminar > -1
            oform.DataSources.DBDataSources.Item(tablaConfigBodegasCC).RemoveRecord(intRegistoEliminar - 1)
            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)
        Loop

        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If
    End Sub

    Private Sub MatrizAprobacionItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

    End Sub

    Public Sub ButtonSeleccionListaPrecios(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        'If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then
        '    'Nothing
        'Else
        If pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            oForm = ApplicationSBO.Forms.Item(FormUID)
            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_VSLP", False, ApplicationSBO) Then
                Dim objSelListPre As New ListaPreciosSeleccion(m_oCompany, ApplicationSBO)
                objSelListPre.FormConfiguracion = oForm
                Call objSelListPre.CargaFormListaPrecios()
            End If
        End If
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
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function


    Private Sub ButtonAddOTIntItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strCodPrimerItem As String
        Dim intPosicion As Integer

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            oForm = m_oApplication.Forms.Item(FormUID)

            If Not oForm.Mode = BoFormMode.fm_FIND_MODE Then
                oMatrizConfOTInt = DirectCast(oForm.Items.Item(mc_strmtx_OTI).Specific, SAPbouiCOM.Matrix)
                oMatrizConfOTInt.FlushToDataSource()
                intPosicion = oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).Size

                If intPosicion = 1 Then
                    strCodPrimerItem = oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).GetValue(mc_str_TipoOTInt, 0)
                    strCodPrimerItem = strCodPrimerItem.Trim()
                    If String.IsNullOrEmpty(strCodPrimerItem) Then
                        intPosicion = 0
                        oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).SetValue(mc_str_TipoOTInt, intPosicion, String.Empty)
                        oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).SetValue(mc_str_NumCuenta, intPosicion, String.Empty)
                        oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).SetValue(mc_str_Tran, intPosicion, String.Empty)
                    Else
                        intPosicion = 1
                        oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).InsertRecord(intPosicion)
                    End If
                Else
                    oForm.DataSources.DBDataSources.Item(tablaConfigOTInt).InsertRecord(intPosicion)
                End If

                oMatrizConfOTInt.LoadFromDataSource()
            End If


        End If
    End Sub

    Private Sub ButtonDelOTIntItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = m_oApplication.Forms.Item(FormUID)

        If Not oform.Mode = BoFormMode.fm_FIND_MODE Then
            oMatriz = DirectCast(oform.Items.Item(mc_strmtx_OTI).Specific, SAPbouiCOM.Matrix)
            intRegistoEliminar = oMatriz.GetNextSelectedRow()

            Do While intRegistoEliminar > -1
                oform.DataSources.DBDataSources.Item(tablaConfigOTInt).RemoveRecord(intRegistoEliminar - 1)
                blnLineasEliminadas = True
                intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)
            Loop

            If blnLineasEliminadas Then
                oMatriz.LoadFromDataSource()
                oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If
        End If

    End Sub

    Private Sub ButtonAddTipOrdentemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strCodPrimerItem As String
        Dim intPosicion As Integer

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then


        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then
            oForm = m_oApplication.Forms.Item(FormUID)

            If Not oForm.Mode = BoFormMode.fm_FIND_MODE Then

                oMatrizTipoOrden = DirectCast(oForm.Items.Item(mc_strmtx_TipoOrden).Specific, SAPbouiCOM.Matrix)
                oMatrizTipoOrden.FlushToDataSource()
                intPosicion = oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).Size

                If intPosicion = 1 Then
                    strCodPrimerItem = oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).GetValue(mc_str_CodTipoOrden, 0)
                    strCodPrimerItem = strCodPrimerItem.Trim()
                    If String.IsNullOrEmpty(strCodPrimerItem) Then
                        intPosicion = 0
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_CodTipoOrden, intPosicion, String.Empty)
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_NombreTipoOrden, intPosicion, String.Empty)
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_UsaDimension, intPosicion, "N")
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_UsaDimensionAEM, intPosicion, "N")
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_UsaDimensionAFP, intPosicion, "N")
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_OTInterna, intPosicion, "N")
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_str_CentroCosto_TipoOrden, intPosicion, String.Empty)
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).SetValue(mc_strcolUsaLtP, intPosicion, "N")
                    Else
                        intPosicion = 1
                        oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).InsertRecord(intPosicion)
                    End If
                Else
                    oForm.DataSources.DBDataSources.Item(tablaConfigTipoOrden).InsertRecord(intPosicion)
                End If

                oMatrizTipoOrden.LoadFromDataSource()
            End If


        End If
    End Sub

    Private Sub ButtonDelTipoOrdenItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = m_oApplication.Forms.Item(FormUID)

        If Not oform.Mode = BoFormMode.fm_FIND_MODE Then

            oMatriz = DirectCast(oform.Items.Item(mc_strmtx_TipoOrden).Specific, SAPbouiCOM.Matrix)
            intRegistoEliminar = oMatriz.GetNextSelectedRow()

            Do While intRegistoEliminar > -1
                oform.DataSources.DBDataSources.Item(tablaConfigTipoOrden).RemoveRecord(intRegistoEliminar - 1)
                blnLineasEliminadas = True
                intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)
            Loop

            If blnLineasEliminadas Then
                oMatriz.LoadFromDataSource()
                oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If
        End If

    End Sub

End Class
