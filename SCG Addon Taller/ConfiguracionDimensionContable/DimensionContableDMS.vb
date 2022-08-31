Imports DMSOneFramework
Imports SAPbouiCOM
Imports System.Globalization

Public Class DimensionContableDMS

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private SBO_Application As SAPbouiCOM.Application

   Public n As NumberFormatInfo

   Private _dtEncabezado As SAPbouiCOM.DataTable

    Public intError As Integer
    Public strMensajeError As String

    Private ListaActualConfiguracion As Generic.List(Of String) = New Generic.List(Of String)
    Private ListaModificadaConfiguracion As Generic.List(Of String) = New Generic.List(Of String)

    Private ListaValoresConfiguracion As Hashtable
    Private ListaNuevosValoresConfiguracion As Hashtable
    
    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property

#End Region

#Region "Constantes"

    Private Const _NombreTablaPadreSBO = "@SCGD_DIMEN"
    Private Const _NombreTablaTipoInventarionDimension = "@SCGD_LINEAS_DIMEN"

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company, ByVal p_strUISCGD_DimensionContableDMS As String)
        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLConfiguracionDimensionesDMS
        MenuPadre = "SCGD_CDE"
        Nombre = My.Resources.Resource.TituloConfiguracionDimensionesDMS
        IdMenu = p_strUISCGD_DimensionContableDMS
        Posicion = 3
        FormType = p_strUISCGD_DimensionContableDMS
    End Sub

#End Region


#Region "DimensionesContables"


    Private m_oFormRecosteoMultiple As SAPbouiCOM.Form
    Private m_dbRecosteo As SAPbouiCOM.DBDataSource

#End Region

    Private Property blnAgregarFila As Boolean


    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoLoad(ByVal p_Form As SAPbouiCOM.Form, _
                                   ByRef BubbleEvent As Boolean)

        'Dim oForm As SAPbouiCOM.Form
        'oForm = SBO_Application.Forms.Item(FormUID)
        Dim p_matriz As SAPbouiCOM.Matrix
        p_matriz = p_Form.Items.Item("mtxLinCz").Specific
        Try

        

        Catch ex As Exception
            m_oCompany.GetLastError(intError, strMensajeError)
            Throw New Exception(strMensajeError)

        End Try



    End Sub


    ''' <summary>
    ''' Permite habilitar/deshabilitar el combo box tipo de inventario campo U_Tip_Inv
    ''' </summary>
    ''' <param name="Habilitado">True = Habilita el combo box, False = Deshabilita el combo box</param>
    ''' <remarks></remarks>
    Private Sub HabilitarCboTipoInventario(ByVal Habilitado As Boolean)
        Try
            Dim ocombo As SAPbouiCOM.ComboBox
            ocombo = DirectCast(FormularioSBO.Items.Item("TipInv").Specific, SAPbouiCOM.ComboBox)
            ocombo.Item.Enabled = Habilitado
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos en el menú superior, utilizar el icono buscar, crear, documento anterior, siguiente, entre otros.
    ''' </summary>
    ''' <param name="pval">pval con la información del evento, proviene de SAP</param>
    ''' <param name="formUID">Formulario de SAP</param>
    ''' <param name="BubbleEvent">BubbleEvent, proviene de SAP</param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try

            Select Case pval.MenuUID
                'Botón modo crear/nuevo documento
                Case "1282"
                    HabilitarCboTipoInventario(True)
                    'Botón modo buscar documento
                Case "1281"
                    HabilitarCboTipoInventario(True)
                    'Boton primer documento, documento anterior, siguiente documento y último documento
                Case "1290", "1288", "1291", "1289"
                    HabilitarCboTipoInventario(False)
            End Select
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    ''' <summary>
    ''' Maneja los eventos de tipo et_FORM_DATA_LOAD
    ''' </summary>
    ''' <param name="SAPEventType">Tipo de evento</param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoFormDataLoad(ByVal SAPEventType As SAPbouiCOM.BoEventTypes)
        Try
            If SAPEventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD Then
                HabilitarCboTipoInventario(False)
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                  ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                  ByRef BubbleEvent As Boolean)

        Try
            Dim ocombo As SAPbouiCOM.ComboBox
            ocombo = DirectCast(FormularioSBO.Items.Item("TipInv").Specific, SAPbouiCOM.ComboBox)

            If pVal.ItemUID = "1" Then

                Dim NumeroLineasMatriz As Integer = MatrixLineasDimension.Matrix.RowCount

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then

                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then

                        If ValidarCampoTipoInventario(FormularioSBO) Then
                            BubbleEvent = False
                            Exit Sub
                        ElseIf NumeroLineasMatriz = 0 Then
                            SBO_Application.SetStatusBarMessage(My.Resources.Resource.MensajeMarcasDimensiones, BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                            Exit Sub
                            'ElseIf ValidarExistenciaDimensionDMS(FormularioSBO) Then
                            '    SBO_Application.SetStatusBarMessage(My.Resources.Resource.MensajeYaExisteDimension, BoMessageTime.bmt_Short, True)
                            '    BubbleEvent = False
                            '    Exit Sub
                        End If

                        MatrixLineasDimension.Matrix.FlushToDataSource()

                        MatrixLineasDimension.Matrix.LoadFromDataSource()

                        'ValidarCamposConfiguraciones(dtConfiguraciones)

                    End If
                End If

            ElseIf pVal.ItemUID = "btnCargar" Then

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then

                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then

                        If ValidarCampoTipoInventario(FormularioSBO) Then
                            BubbleEvent = False
                            Exit Sub
                        End If

                        CargarMarcas(MatrixLineasDimension)
                    End If

                End If

            ElseIf pVal.ItemUID = "btnAdd" Then

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then
                        AgregarLineaMarca(MatrixLineasDimension)
                    End If

                End If
            ElseIf pVal.ItemUID = "btnGConf" Then

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then
                        GuardarCamposConfiguraciones(dtConfiguraciones)
                    End If

                End If

            ElseIf pVal.ItemUID = "Folder1" Then

                FormularioSBO.PaneLevel = 1


            ElseIf pVal.ItemUID = "Folder2" Then

                FormularioSBO.PaneLevel = 2

            ElseIf pVal.ItemUID = "mtxDim" Then

                If pVal.ColUID = "colDim1" Then

                    ManejarModoActualizarAlCambiarDimensiones(FormularioSBO)

                ElseIf pVal.ColUID = "colDim2" Then

                    ManejarModoActualizarAlCambiarDimensiones(FormularioSBO)

                ElseIf pVal.ColUID = "colDim3" Then

                    ManejarModoActualizarAlCambiarDimensiones(FormularioSBO)

                ElseIf pVal.ColUID = "colDim4" Then

                    ManejarModoActualizarAlCambiarDimensiones(FormularioSBO)

                ElseIf pVal.ColUID = "colDim5" Then

                    ManejarModoActualizarAlCambiarDimensiones(FormularioSBO)

                End If

            End If

        Catch ex As Exception
            m_oCompany.GetLastError(intError, strMensajeError)
            Throw New SCGCommon.ExceptionsSBO(strMensajeError, ex)

        End Try

    End Sub

    Private Sub ManejarModoActualizarAlCambiarDimensiones(ByRef p_form As SAPbouiCOM.Form)

        If Not p_form.Mode = BoFormMode.fm_ADD_MODE Then
            If Not p_form.Mode = BoFormMode.fm_FIND_MODE Then
                p_form.Freeze(True)
                p_form.Mode = BoFormMode.fm_UPDATE_MODE
                p_form.Freeze(False)
            End If
        End If

    End Sub

    Public Function ValidarCampoTipoInventario(ByVal p_form As SAPbouiCOM.Form, Optional ByVal p_blnValidar As Boolean = False) As Boolean

        Dim numlinea As Integer = MatrixLineasDimension.Matrix.RowCount


        If p_form.DataSources.DBDataSources.Item(_NombreTablaPadreSBO).GetValue("U_Tip_Inv", 0) = String.Empty Then

            SBO_Application.SetStatusBarMessage(My.Resources.Resource.MensajeTipoInventario, BoMessageTime.bmt_Short, True)

            Return True
            
        Else

            Return False


        End If

    End Function

    Private Sub CargarMarcas(ByRef p_matriz As MatrizLineasDimensiones)

        Dim dtMarcas As System.Data.DataTable
        Dim row As System.Data.DataRow
        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim ListaMarcas As Generic.List(Of String) = New Generic.List(Of String)

        Dim intNuevoRegisto As Integer = 0

        intNuevoRegisto = FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).Size

        Dim numlinea As Integer = p_matriz.Matrix.RowCount

        dtMarcas = Utilitarios.EjecutarConsultaDataTable("Select ""Code"", ""Name"" from ""@SCGD_MARCA"" ")
        matrixXml = p_matriz.Matrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)

        'lleno la lista con los valores de la matriz, la columna Codigo Marca
        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            Dim elementoCodigoMarca As Xml.XmlNode
            elementoCodigoMarca = node.SelectSingleNode("Columns/Column/Value[../ID = 'colMarc']")

            If Not elementoCodigoMarca.InnerText = String.Empty Then
                ListaMarcas.Add(elementoCodigoMarca.InnerText)
            End If
        Next

        p_matriz.Matrix.FlushToDataSource()

        SBO_Application.SetStatusBarMessage(My.Resources.Resource.MensajeCargandoMarcas, BoMessageTime.bmt_Short, False)

        For Each row In dtMarcas.Rows

            If Not ListaMarcas.Contains(row.Item("Code")) Then
                If numlinea = 0 Then
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_CodMar", 0, row.Item("Code"))
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_DesMar", 0, row.Item("Name"))
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", 0, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", 0, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", 0, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim4", 0, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim5", 0, Nothing)
                    numlinea = numlinea + 1
                Else
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).InsertRecord(intNuevoRegisto)
                    intNuevoRegisto += 1
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_CodMar", intNuevoRegisto - 1, row.Item("Code"))
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_DesMar", intNuevoRegisto - 1, row.Item("Name"))
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", intNuevoRegisto - 1, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", intNuevoRegisto - 1, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", intNuevoRegisto - 1, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim4", intNuevoRegisto - 1, Nothing)
                    FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim5", intNuevoRegisto - 1, Nothing)
                End If

            End If

        Next

        p_matriz.Matrix.LoadFromDataSource()

    End Sub


    Public Sub AgregarLineaMarca(ByRef p_matriz As MatrizLineasDimensiones, Optional ByVal p_blnCarga As Boolean = False, Optional ByVal p_codigomarca As String = "")

        Dim intNuevoRegisto As Integer = 0

        intNuevoRegisto = FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).Size

        Dim numlinea As Integer = p_matriz.Matrix.RowCount

        If numlinea = 0 Then
            p_matriz.Matrix.AddRow(1)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_CodMar", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_DesMar", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim4", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim5", intNuevoRegisto - 1, Nothing)

            p_matriz.Matrix.LoadFromDataSource()
            p_matriz.Matrix.SetCellFocus(1, 1)

        Else
            p_matriz.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).InsertRecord(intNuevoRegisto)
            intNuevoRegisto += 1
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_CodMar", intNuevoRegisto - 1, p_codigomarca)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_CodMar", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_DesMar", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim4", intNuevoRegisto - 1, Nothing)
            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim5", intNuevoRegisto - 1, Nothing)
            p_matriz.Matrix.LoadFromDataSource()
            p_matriz.Matrix.SetCellFocus(numlinea + 1, 1)
        End If

    End Sub

    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                           ByVal FormUID As String, ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID

        Dim intNumeroLinea As Integer = 0

        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

        Dim oDataTable As SAPbouiCOM.DataTable
        Dim blnAddLinea As Boolean = False

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim CantidadLineas As Integer

        Dim strValorDimension As String = String.Empty

        intNumeroLinea = pval.Row

        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE Then

            If pval.ActionSuccess = True AndAlso pval.BeforeAction = False Then

                If pval.ColUID = "colDim1" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension1 As String = oDataTable.GetValue("PrcCode", 0)

                            MatrixLineasDimension.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", intNumeroLinea - 1, dimension1)

                        End If

                    End If

                ElseIf pval.ColUID = "colDim2" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension2 As String = oDataTable.GetValue("OcrCode", 0)

                            MatrixLineasDimension.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", intNumeroLinea - 1, dimension2)


                        End If

                    End If


                ElseIf pval.ColUID = "colDim3" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension3 As String = oDataTable.GetValue("OcrCode", 0)

                            MatrixLineasDimension.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", intNumeroLinea - 1, dimension3)

                        End If

                    End If

                End If

                MatrixLineasDimension.Matrix.LoadFromDataSource()

            ElseIf pval.BeforeAction = True Then

                Select Case pval.ColUID

                    Case "colDim1"

                        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DimCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = 1

                        oCondition.BracketCloseNum = 1
                        oCFL.SetConditions(oConditions)

                    Case "colDim2"

                        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DimCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = 2

                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)


                    Case "colDim3"

                        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DimCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = 3

                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case "colDim4"

                        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DimCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = 4

                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case "colDim5"

                        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DimCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = 5

                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)



                End Select

            End If
        End If

    End Sub



    ''' <summary>
    ''' Devuelve el valor en pantalla de una celda de la matriz de dimensiones
    ''' </summary>
    ''' <param name="pNombreColumna">Nombre de la columna en formato string</param>
    ''' <param name="pNumeroLinea">Número de línea</param>
    ''' <returns>Valor en pantalla de la celda</returns>
    ''' <remarks></remarks>
    Public Function ObtenerValorDimension(ByVal pNombreColumna, pNumeroLinea) As String

        Dim strValorCelda As String = String.Empty

        Try

            If pNumeroLinea <= MatrixLineasDimension.Matrix.RowCount And pNumeroLinea >= 0 Then
                strValorCelda = MatrixLineasDimension.ObtieneValorColumnaEditText(pNombreColumna, pNumeroLinea)
            End If

            ObtenerValorDimension = strValorCelda

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function



    Public Sub CargarConfiguracionDocumentos()

        Dim strTipoParaTaller As String

        ListaValoresConfiguracion = New Hashtable

        Dim dataTablaSC As DataTable

        Dim strItemCode As String
        Dim strDescripcion As String
        Dim strPorcentajeD As String
        Dim strMoneda As String
        Dim strPrecio As String
        Dim strComentario As String
        Dim strIDR As String
        Dim strCosto As String
        Dim strCantidad As String
        Dim strTax As String
        Dim strSelec As String

        Dim strSeleccionTodas As String


        Dim strConsulta As String = "Select  ""Code"", ""Name"", ""U_Valor"" FROM ""@SCGD_DIMEN_CONF"" Order by ""Code"" "

        dtConfiguraciones.ExecuteQuery(strConsulta)

        For i As Integer = 0 To dtConfiguraciones.Rows.Count - 1

            ListaValoresConfiguracion.Add(dtConfiguraciones.GetValue("Code", i), dtConfiguraciones.GetValue("U_Valor", i))
        Next

        MatrixLineasConfiguracion.Matrix.LoadFromDataSource()


    End Sub

    Private Sub GuardarCamposConfiguraciones(ByRef p_dataTable As SAPbouiCOM.DataTable)

        ListaNuevosValoresConfiguracion = New Hashtable

        MatrixLineasConfiguracion.Matrix.FlushToDataSource()
        MatrixLineasConfiguracion.Matrix.LoadFromDataSource()

        For i As Integer = 0 To p_dataTable.Rows.Count - 1
            ListaNuevosValoresConfiguracion.Add(p_dataTable.GetValue("Code", i), p_dataTable.GetValue("U_Valor", i))
        Next i

        For m As Integer = 0 To p_dataTable.Rows.Count - 1

            If ListaValoresConfiguracion.Item(p_dataTable.GetValue("Code", m)) <> ListaNuevosValoresConfiguracion.Item(p_dataTable.GetValue("Code", m)) Then




                Dim strQueryUpdate As String = String.Format("UPDATE ""@SCGD_DIMEN_CONF"" SET ""U_Valor"" = '{0}' Where ""Code"" = {1} ",
                                                   ListaNuevosValoresConfiguracion.Item(p_dataTable.GetValue("Code", m)), p_dataTable.GetValue("Code", m))


                Utilitarios.EjecutarConsulta(strQueryUpdate)

            End If
        Next
    End Sub

    ''' <summary>
    ''' Valida que no exista un maestro de dimensiones para el tipo de inventario que se desea crear
    ''' </summary>
    ''' <param name="p_TipoInventario">Tipo de inventario en formato entero</param>
    ''' <returns>True = Ya existe un maestro para este tipo de inventario, False = No existe un maestro para este tipo de inventario</returns>
    ''' <remarks></remarks>
    Public Function ValidarExistenciaDimensionDMS(ByVal p_form As SAPbouiCOM.Form) As Boolean
        Dim strConsultaDocEntry As String = "SELECT Count(T0.""DocEntry"") FROM ""@SCGD_DIMEN"" T0 INNER JOIN ""@SCGD_LINEAS_DIMEN"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_Tip_Inv"" = '{0}' "
        Dim strTipoInventario As String = String.Empty
        Dim strResultado As String = String.Empty
        Try
            If p_form.Mode = BoFormMode.fm_ADD_MODE Then
                strTipoInventario = p_form.DataSources.DBDataSources.Item(_NombreTablaPadreSBO).GetValue("U_Tip_Inv", 0).Trim
                strConsultaDocEntry = String.Format(strConsultaDocEntry, strTipoInventario)
                strResultado = Utilitarios.EjecutarConsulta(strConsultaDocEntry)
                If strResultado = "0" Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Function ValidarTipoInventario(ByVal p_TipoInventario As Integer) As Boolean

        Return Not String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(String.Format("Select ""DocEntry"" from ""@SCGD_DIMEN"" where  ""U_Tip_Inv"" = '{0}' ", p_TipoInventario)))

    End Function

End Class