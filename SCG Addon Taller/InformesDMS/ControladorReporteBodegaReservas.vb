Imports SAPbouiCOM
Imports System.Collections.Generic

''' <summary>
''' Módulo encargado del funcionamiento del reporte de auditoría de bodega reservas
''' </summary>
''' <remarks></remarks>
Public Module ControladorReporteBodegaReservas

    Private MonedaLocal As String = String.Empty
    Private MonedaSistema As String = String.Empty

    ''' <summary>
    ''' Constructor del módulo
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            'Carga las monedas locales y de sistema para su uso en distintas partes del módulo
            DMS_Connector.Helpers.GetCurrencies(MonedaLocal, MonedaSistema)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores predeterminados
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarValoresPredeterminados(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            CargarValidValues(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores válidos del ComboBox bodega
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CargarValidValues(ByRef oFormulario As SAPbouiCOM.Form)
        Dim ComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = "SELECT DISTINCT T0.""U_Res"", T1.""WhsName"" FROM ""@SCGD_CONF_BODXCC"" T0 INNER JOIN ""OWHS"" T1 ON T0.""U_Res"" = T1.""WhsCode"" Order By T1.""WhsName"" "
        Dim RecordSet As SAPbobsCOM.Recordset
        Try
            ComboBox = oFormulario.Items.Item("Whs").Specific
            RecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            RecordSet.DoQuery(Query)

            While Not RecordSet.EoF
                ComboBox.ValidValues.Add(RecordSet.Fields.Item(0).Value.ToString(), RecordSet.Fields.Item(1).Value.ToString())
                RecordSet.MoveNext()
            End While

            If ComboBox.ValidValues.Count > 0 Then
                ComboBox.Select(0, BoSearchKey.psk_Index)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    ''' <summary>
    ''' Manejador de los eventos ItemEvent de SAP para el formulario Reporte de Bodega de Reservas
    ''' </summary>
    ''' <param name="FormUID">ID del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable booleana de SAP para definir si se debe continuar con el proceso o no</param>
    ''' <remarks></remarks>
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If pVal.FormTypeEx = "SCGD_RABR" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    Select Case pVal.EventType
                        Case BoEventTypes.et_ITEM_PRESSED
                            ItemPressed(oFormulario, pVal, BubbleEvent)
                        Case BoEventTypes.et_COMBO_SELECT

                        Case BoEventTypes.et_CHOOSE_FROM_LIST
                            ChooseFromList(oFormulario, pVal, BubbleEvent)
                        Case BoEventTypes.et_VALIDATE

                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos ChooseFromList
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ChooseFromList(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ItemUID
                Case "ItmCod"
                    ManejadorChooseFromListItemCode(oFormulario, pVal, BubbleEvent)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ChooseFromList código del artículo
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListItemCode(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        'Dim oCondition As SAPbouiCOM.Condition
        'Dim oConditions As SAPbouiCOM.Conditions
        Dim oDataTable As SAPbouiCOM.DataTable

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                'oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                ' oCondition = oConditions.Add()
                ' oCondition.BracketOpenNum = 1
                ' oCondition.Alias = "ItemCode"
                ' oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                ' oCondition.BracketCloseNum = 1
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.UserDataSources.Item("ItmCod").ValueEx = oDataTable.GetValue("ItemCode", 0)
                End If
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID">ID única de la instancia del formulario</param>
    ''' <returns>Si el formulario existe devuelve la instancia, de lo contrario devuelve Nothing</returns>
    ''' <remarks></remarks>
    Private Function ObtenerFormulario(ByVal FormUID As String) As SAPbouiCOM.Form
        Try
            Return DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Manejador de los eventos ItemPressed
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Booleano que indica si se debe continuar procesando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressed(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "Search"
                        Buscar(oFormulario, pVal, BubbleEvent)
                    Case "Expand"
                        ExpandirComprimir(oFormulario, pVal, BubbleEvent)
                    Case "Compress"
                        ExpandirComprimir(oFormulario, pVal, BubbleEvent)
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "Search"

                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Expande todas las líneas de la matriz
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Booleano que indica si se debe continuar procesando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ExpandirComprimir(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Grid As SAPbouiCOM.Grid
        Try
            Grid = oFormulario.Items.Item("Grid").Specific
            If pVal.ItemUID = "Expand" Then
                Grid.Rows.ExpandAll()
            Else
                Grid.Rows.CollapseAll()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca los movimientos de inventarios de acuerdo a los filtros seleccionados y los carga en el Grid
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Booleano que indica si se debe continuar procesando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub Buscar(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim DataTable As SAPbouiCOM.DataTable
        Dim Grid As SAPbouiCOM.Grid
        Dim Query As String = " SELECT T1.ItemCode, T1.Dscription AS 'Dsc', T3.U_SCGD_NoSerieCita + '-' + T3.U_SCGD_NoCita AS 'App', T0.CreateDate AS 'Date', CASE WHEN T1.U_SCGD_ID IS NULL OR T1.U_SCGD_ID = '' THEN '99' WHEN T1.WhsCode = '{0}' AND T1.U_SCGD_ID IS NOT NULL THEN '3' WHEN T1.FromWhsCod = '{0}' AND T1.U_SCGD_ID IS NOT NULL AND T4.U_TransEntry IS NULL THEN '4' WHEN T1.FromWhsCod = '{0}' AND T1.U_SCGD_ID IS NOT NULL AND T4.U_TransEntry IS NOT NULL  THEN '98' END AS 'Type', T0.DocEntry, CASE WHEN T1.WhsCode = '{0}' THEN T1.Quantity ELSE 0 END AS 'In', CASE WHEN T1.FromWhsCod = '{0}' THEN T1.Quantity ELSE 0 END AS 'Out', '{1}' AS 'Curr', T1.StockPrice AS 'Cost' FROM OWTR T0 WITH (nolock) INNER JOIN WTR1 T1 WITH (nolock) ON T0.DocEntry = T1.DocEntry LEFT JOIN QUT1 T2 WITH (nolock) ON T1.ItemCode = T2.ItemCode AND T1.U_SCGD_ID = T2.U_SCGD_ID LEFT JOIN OQUT T3 WITH (nolock) ON T3.DocEntry = T2.DocEntry LEFT JOIN ""@SCGD_OTTA"" T4 WITH (nolock) ON T0.DocEntry = T4.U_TransEntry AND T1.U_SCGD_ID = T4.U_SCGD_ID WHERE (T1.WhsCode = '{0}' OR T1.FromWhsCod = '{0}') "
        Dim Encabezados As List(Of String) = New List(Of String)
        Dim Columna As SAPbouiCOM.EditTextColumn
        Dim WarehouseCode As String = String.Empty
        Dim ComboBoxColumn As SAPbouiCOM.ComboBoxColumn
        Dim FechaDesde As String = String.Empty
        Dim FechaHasta As String = String.Empty
        Dim CodigoArticulo As String = String.Empty

        Try
            oFormulario.Freeze(True)
            WarehouseCode = oFormulario.DataSources.UserDataSources.Item("Whs").ValueEx
            FechaDesde = oFormulario.DataSources.UserDataSources.Item("SDate").ValueEx
            FechaHasta = oFormulario.DataSources.UserDataSources.Item("EDate").ValueEx
            CodigoArticulo = oFormulario.DataSources.UserDataSources.Item("ItmCod").ValueEx

            If Not String.IsNullOrEmpty(WarehouseCode) Then
                Grid = oFormulario.Items.Item("Grid").Specific
                Grid.CommonSetting.FixedColumnsCount = 3

                'Guarda los titulos en una lista
                'ya que al ejecutar el query se borran
                Encabezados.Add(Grid.Columns.Item(0).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(1).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(2).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(3).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(4).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(5).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(6).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(7).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(8).TitleObject.Caption)
                Encabezados.Add(Grid.Columns.Item(9).TitleObject.Caption)

                DataTable = oFormulario.DataSources.DataTables.Item("Details")
                Query = String.Format(Query, WarehouseCode, MonedaLocal)
                If Not String.IsNullOrEmpty(FechaDesde) Then
                    Query += String.Format(" AND T0.""CreateDate"" >= '{0}' ", FechaDesde)
                End If

                If Not String.IsNullOrEmpty(FechaHasta) Then
                    Query += String.Format(" AND T0.""CreateDate"" <= '{0}' ", FechaHasta)
                End If

                If Not String.IsNullOrEmpty(CodigoArticulo) Then
                    Query += String.Format(" AND T1.""ItemCode"" = '{0}' ", CodigoArticulo)
                End If

                Query += " Order By T0.CreateDate, T0.DocEntry Asc "

                DataTable.ExecuteQuery(Query)

                'Define la columna en la cual se expande/comprime el listado
                Grid.CollapseLevel = 1

                'Vuelve a asignar los títulos a los encabezados de las columnas
                'después de ejecutar el query
                Grid.Columns.Item(0).TitleObject.Caption = Encabezados.Item(0)
                Grid.Columns.Item(1).TitleObject.Caption = Encabezados.Item(1)
                Grid.Columns.Item(2).TitleObject.Caption = Encabezados.Item(2)
                Grid.Columns.Item(3).TitleObject.Caption = Encabezados.Item(3)
                Grid.Columns.Item(4).TitleObject.Caption = Encabezados.Item(4)
                Grid.Columns.Item(5).TitleObject.Caption = Encabezados.Item(5)
                Grid.Columns.Item(6).TitleObject.Caption = Encabezados.Item(6)
                Grid.Columns.Item(7).TitleObject.Caption = Encabezados.Item(7)
                Grid.Columns.Item(8).TitleObject.Caption = Encabezados.Item(8)
                Grid.Columns.Item(9).TitleObject.Caption = Encabezados.Item(9)

                'Agrega los LinkButton a las columnas
                Columna = Grid.Columns.Item("ItemCode")
                Columna.LinkedObjectType = "4"
                Columna = Grid.Columns.Item("DocEntry")
                Columna.LinkedObjectType = "67"
                Columna.ColumnSetting.SumType = BoColumnSumType.bst_Manual
                Columna.ColumnSetting.SumValue = "Total"
                Columna = Grid.Columns.Item("In")
                Columna.ColumnSetting.SumType = BoColumnSumType.bst_Auto
                Columna = Grid.Columns.Item("Out")
                Columna.ColumnSetting.SumType = BoColumnSumType.bst_Auto

                'Agrega los valores válidos a las columnas
                Grid.Columns.Item("Type").Type = BoGridColumnType.gct_ComboBox
                ComboBoxColumn = Grid.Columns.Item("Type")
                ComboBoxColumn.ValidValues.Add("3", My.Resources.Resource.RequisicionReserva)
                ComboBoxColumn.ValidValues.Add("4", My.Resources.Resource.DevolucionReserva)
                ComboBoxColumn.ValidValues.Add("98", My.Resources.Resource.TransferenciaAutomatica)
                ComboBoxColumn.ValidValues.Add("99", My.Resources.Resource.TransferenciaManual)
                ComboBoxColumn.DisplayType = BoComboDisplayType.cdt_Description
                AjustarColumnas(Grid)
            Else
                'Mensaje de error, debe seleccionar una bodega
                DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorSeleccionarBodega, BoMessageTime.bmt_Short, True)
            End If
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ajusta el tamaño de las columnas
    ''' </summary>
    ''' <param name="Grid">Objeto grid a la cual se le desean ajustar las columnas</param>
    ''' <remarks></remarks>
    Private Sub AjustarColumnas(ByRef Grid As SAPbouiCOM.Grid)
        Try
            Grid.Columns.Item("ItemCode").Width = 140
            Grid.Columns.Item("Dsc").Width = 200
            Grid.Columns.Item("App").Width = 70
            Grid.Columns.Item("Date").Width = 55
            Grid.Columns.Item("Type").Width = 100
            Grid.Columns.Item("DocEntry").Width = 100
            Grid.Columns.Item("In").Width = 50
            Grid.Columns.Item("Out").Width = 50
            Grid.Columns.Item("Curr").Width = 50
            Grid.Columns.Item("Cost").Width = 100
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
