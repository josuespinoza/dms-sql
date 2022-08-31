Imports DMSOneFramework
Imports SAPbouiCOM
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Xml
Imports DMSOneFramework.SCGCommon
Imports DMS_Addon.My.Resources
Imports DMS_Connector.Business_Logic
Imports SAPbobsCOM

Public Class DimensionContableDMSOTs

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private SBO_Application As Application

    Public n As NumberFormatInfo

    Private _dtEncabezado As DataTable

    Public intError As Integer
    Public strMensajeError As String

    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property


    Private dcCosto As Decimal = 0
    Private dcCostoS As Decimal = 0

#End Region

#Region "Constantes"

    Private Const _NombreTablaPadreSBO = "@SCGD_DIMENSION_OT"
    Private Const _NombreTablaTipoInventarionDimension = "@SCGD_LINEAS_DIMENOT"

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company, ByVal p_strUISCGD_DimensionContableDMSOTs As String)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLConfiguracionDimensionesDMSOT
        MenuPadre = "SCGD_CDE"
        Nombre = My.Resources.Resource.TituloConfiguracionDimensionesDMSOT
        IdMenu = p_strUISCGD_DimensionContableDMSOTs
        Posicion = 4
        FormType = p_strUISCGD_DimensionContableDMSOTs
    End Sub

#End Region


#Region "DimensionesContables"


    Private m_oFormRecosteoMultiple As Form
    Private m_dbRecosteo As DBDataSource

#End Region


    <CLSCompliant(False)> _
    Public Sub ManejadorEventoLoad(ByVal p_Form As Form, _
                                   ByRef BubbleEvent As Boolean)

        Dim p_matriz As Matrix
        Try
            p_matriz = p_Form.Items.Item("mtxOT").Specific
        Catch ex As Exception
            m_oCompany.GetLastError(intError, strMensajeError)
            Throw New Exception(strMensajeError)
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
                                                  ByRef pVal As ItemEvent, _
                                                  ByRef BubbleEvent As Boolean)

        Try

            If pVal.ItemUID = "1" Then

                Dim NumeroLineasMatriz As Integer = MatrixLineasDimensionOT.Matrix.RowCount

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then

                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then

                        If ValidarCampoSucursal(FormularioSBO) Then
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

                        MatrixLineasDimensionOT.Matrix.FlushToDataSource()

                        MatrixLineasDimensionOT.Matrix.LoadFromDataSource()

                    End If

                End If


            ElseIf pVal.ItemUID = "btnCargar" Then

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then

                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then

                        If ValidarCampoSucursal(FormularioSBO) Then
                            BubbleEvent = False
                            Exit Sub
                        End If

                        CargarMarcas(MatrixLineasDimensionOT)
                    End If

                End If

            ElseIf pVal.ItemUID = "btnAdd" Then

                If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                    If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then
                        AgregarLineaMarca(MatrixLineasDimensionOT)
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
            Throw New ExceptionsSBO(strMensajeError, ex)

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
    ''' Permite habilitar/deshabilitar el combo box tipo de inventario campo U_Tip_Inv
    ''' </summary>
    ''' <param name="Habilitado">True = Habilita el combo box, False = Deshabilita el combo box</param>
    ''' <remarks></remarks>
    Private Sub HabilitarCboTipoInventario(ByVal Habilitado As Boolean)
        Try
            Dim ocombo As SAPbouiCOM.ComboBox
            ocombo = DirectCast(FormularioSBO.Items.Item("cboSuc").Specific, SAPbouiCOM.ComboBox)
            ocombo.Item.Enabled = Habilitado
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub ManejarModoActualizarAlCambiarDimensiones(ByRef p_form As Form)

        If Not p_form.Mode = BoFormMode.fm_ADD_MODE Then
            If Not p_form.Mode = BoFormMode.fm_FIND_MODE Then
                p_form.Freeze(True)
                p_form.Mode = BoFormMode.fm_UPDATE_MODE
                p_form.Freeze(False)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Valida que no exista un maestro de dimensiones para el tipo de inventario que se desea crear
    ''' </summary>
    ''' <param name="p_TipoInventario">Tipo de inventario en formato entero</param>
    ''' <returns>True = Ya existe un maestro para este tipo de inventario, False = No existe un maestro para este tipo de inventario</returns>
    ''' <remarks></remarks>
    Public Function ValidarExistenciaDimensionDMS(ByVal p_form As SAPbouiCOM.Form) As Boolean
        Dim strConsultaDocEntry As String = "SELECT Count(T0.""DocEntry"") FROM ""@SCGD_DIMENSION_OT"" T0 INNER JOIN ""@SCGD_LINEAS_DIMENOT"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_CodSuc"" = '{0}' "
        Dim strTipoInventario As String = String.Empty
        Dim strResultado As String = String.Empty
        Try
            If p_form.Mode = BoFormMode.fm_ADD_MODE Then
                strTipoInventario = p_form.DataSources.DBDataSources.Item(_NombreTablaPadreSBO).GetValue("U_CodSuc", 0).Trim
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


    Public Function ValidarCampoSucursal(ByVal p_form As Form, Optional ByVal p_blnValidar As Boolean = False) As Boolean

        Dim numlinea As Integer = MatrixLineasDimensionOT.Matrix.RowCount


        If p_form.DataSources.DBDataSources.Item(_NombreTablaPadreSBO).GetValue("U_CodSuc", 0) = String.Empty Then

            SBO_Application.SetStatusBarMessage(My.Resources.Resource.MensajeSucursalTaller, BoMessageTime.bmt_Short, True)

            Return True

        Else

            Return False


        End If

    End Function

    Private Sub CargarMarcas(ByRef p_matriz As MatrizLineasDimensionesOT)

        Dim dtMarcas As Data.DataTable
        Dim row As DataRow
        Dim xmlDocMatrix As XmlDocument
        Dim XmlNode As XmlNode
        Dim matrixXml As String
        Dim ListaMarcas As List(Of String) = New List(Of String)

        Dim intNuevoRegisto As Integer = 0

        intNuevoRegisto = FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).Size

        Dim numlinea As Integer = p_matriz.Matrix.RowCount

        dtMarcas = Utilitarios.EjecutarConsultaDataTable("Select ""Code"", ""Name"" from ""@SCGD_MARCA"" ")
        matrixXml = p_matriz.Matrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)

        'lleno la lista con los valores de la matriz, la columna Codigo Marca
        For Each node As XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
            Dim elementoCodigoMarca As XmlNode
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


    Public Sub AgregarLineaMarca(ByRef p_matriz As MatrizLineasDimensionesOT, Optional ByVal p_blnCarga As Boolean = False, Optional ByVal p_codigomarca As String = "")

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

    Public Sub ManejadorEventoChooseFromList(ByRef pval As ItemEvent, _
                                           ByVal FormUID As String, ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As IChooseFromListEvent
        oCFLEvento = CType(pval, IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID

        Dim intNumeroLinea As Integer = 0

        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

        Dim oDataTable As DataTable
        Dim blnAddLinea As Boolean = False

        Dim oCondition As Condition
        Dim oConditions As Conditions

        Dim CantidadLineas As Integer

        intNumeroLinea = pval.Row

        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE Then

            If pval.ActionSuccess = True AndAlso pval.BeforeAction = False Then

                If pval.ColUID = "colDim1" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension1 As String = oDataTable.GetValue("PrcCode", 0)

                            MatrixLineasDimensionOT.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim1", intNumeroLinea - 1, dimension1)

                        End If

                    End If



                ElseIf pval.ColUID = "colDim2" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension2 As String = oDataTable.GetValue("OcrCode", 0)

                            MatrixLineasDimensionOT.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim2", intNumeroLinea - 1, dimension2)


                        End If

                    End If


                ElseIf pval.ColUID = "colDim3" Then

                    If oCFLEvento.BeforeAction = False Then

                        oDataTable = oCFLEvento.SelectedObjects

                        If Not oCFLEvento.SelectedObjects Is Nothing Then

                            Dim dimension3 As String = oDataTable.GetValue("OcrCode", 0)

                            MatrixLineasDimensionOT.Matrix.FlushToDataSource()
                            FormularioSBO.DataSources.DBDataSources.Item(_NombreTablaTipoInventarionDimension).SetValue("U_Dim3", intNumeroLinea - 1, dimension3)

                        End If

                    End If

                End If

                MatrixLineasDimensionOT.Matrix.LoadFromDataSource()

            ElseIf pval.BeforeAction = True Then
                oConditions = SBO_Application.CreateObject(BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "DimCode"
                oCondition.Operation = BoConditionOperation.co_EQUAL
                Select Case pval.ColUID
                    Case "colDim1"
                        oCondition.CondVal = 1
                    Case "colDim2"
                        oCondition.CondVal = 2
                    Case "colDim3"
                        oCondition.CondVal = 3
                    Case "colDim4"
                        oCondition.CondVal = 4
                End Select
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)
            End If
        End If
    End Sub


    Public Sub CargarConfiguracionDocumentos()

        dtConfiguraciones.Rows.Clear()
        For Each tipoOt As TipoOT In DMS_Connector.Configuracion.TipoOt.OrderBy(Function(tipOT) tipOT.Code)
            dtConfiguraciones.Rows.Add()
            dtConfiguraciones.SetValue("Code", dtConfiguraciones.Rows.Count - 1, tipoOt.Code)
            dtConfiguraciones.SetValue("Name", dtConfiguraciones.Rows.Count - 1, tipoOt.Name)
            dtConfiguraciones.SetValue("U_UsaDim", dtConfiguraciones.Rows.Count - 1, tipoOt.U_UsaDim)
            dtConfiguraciones.SetValue("U_UsaDimAEM", dtConfiguraciones.Rows.Count - 1, tipoOt.U_UsaDimAEM)
            dtConfiguraciones.SetValue("U_UsaDimAFP", dtConfiguraciones.Rows.Count - 1, tipoOt.U_UsaDimAFP)
        Next
        MatrixLineasConfiguracionOT.Matrix.LoadFromDataSource()

    End Sub

    Private Sub GuardarCamposConfiguraciones(ByRef p_dataTable As DataTable)
        Dim strCode As String
        MatrixLineasConfiguracionOT.Matrix.FlushToDataSource()

        For m As Integer = 0 To p_dataTable.Rows.Count - 1
            strCode = p_dataTable.GetValue("Code", m).ToString.Trim
            If DMS_Connector.Configuracion.TipoOt.Any(Function(tipOt) tipOt.Code = strCode) Then
                If Not p_dataTable.GetValue("U_UsaDim", m).ToString.Trim.Equals(DMS_Connector.Configuracion.TipoOt.FirstOrDefault(Function(tipOt) tipOt.Code = strCode).U_UsaDim.Trim) OrElse Not p_dataTable.GetValue("U_UsaDimAEM", m).ToString.Trim.Equals(DMS_Connector.Configuracion.TipoOt.FirstOrDefault(Function(tipOt) tipOt.Code = strCode).U_UsaDimAEM.Trim) OrElse Not p_dataTable.GetValue("U_UsaDimAFP", m).ToString.Trim.Equals(DMS_Connector.Configuracion.TipoOt.FirstOrDefault(Function(tipOt) tipOt.Code = strCode).U_UsaDimAFP.Trim) Then
                    Dim strQueryUpdate As String = String.Format("UPDATE ""@SCGD_TIPO_ORDEN"" SET ""U_UsaDim"" = '{0}' , ""U_UsaDimAEM"" = '{2}' , ""U_UsaDimAFP"" = '{3}' Where ""Code"" = {1} ",
                                                       p_dataTable.GetValue("U_UsaDim", m), p_dataTable.GetValue("Code", m), p_dataTable.GetValue("U_UsaDimAEM", m), p_dataTable.GetValue("U_UsaDimAFP", m))
                    Utilitarios.EjecutarConsulta(strQueryUpdate)
                End If
            End If
        Next
        DMS_Connector.Configuracion.CargaTipoOT()
        Call CargarConfiguracionDocumentos()

    End Sub

End Class

Public Class LineasConfiguracionOT

    Private _strTipoOt As String

    Public Property TipoOT() As String
        Get
            Return _strTipoOt

        End Get
        Set(ByVal value As String)

            _strTipoOt = value

        End Set
    End Property

    Private _strUsaDim As String

    Public Property UsaDim() As String
        Get
            Return _strUsaDim
        End Get
        Set(ByVal value As String)
            _strUsaDim = value
        End Set
    End Property

    Private _strUsaDimAEM As String

    Public Property UsaDimAEM() As String
        Get
            Return _strUsaDimAEM
        End Get
        Set(ByVal value As String)
            _strUsaDimAEM = value
        End Set
    End Property

    Private _strUsaDimAFP As String

    Public Property UsaDimAFP() As String
        Get
            Return _strUsaDimAFP
        End Get
        Set(ByVal value As String)
            _strUsaDimAFP = value
        End Set
    End Property

End Class
