Imports SAPbouiCOM
Imports SCG.DMSOne.Framework
Imports System.Collections.Generic
Imports SCG.SBOFramework.DI
Imports System.IO
Imports Microsoft.Office.Interop
Imports SAPbobsCOM

Public Class Campaña


#Region "Estructuras"

    Private Structure VehiculoCnp
        Public s_strUnidad As String
        Public s_strPlaca As String
        Public s_strVIN As String
    End Structure

#End Region

#Region "Declaraciones"

    Private SBO_Application As SAPbouiCOM.Application
    Private m_ocompany As SAPbobsCOM.Company

    Private g_mVehiculosCampana As MatrizVehiculosCampana
    Private g_dtVehiculosCampana As SAPbouiCOM.DataTable
    Private g_dtVehiculosGuardadosCampanas As SAPbouiCOM.DataTable
    Private g_dtVehiculoCargaMasiva As SAPbouiCOM.DataTable

    Private Const g_strUIDlblPorTramitar As String = "lblPTra"
    Private Const g_strUIDlblTramitada As String = "lblTram"

    Private Const g_strUIDtxtPorTramitar As String = "txtPTra"
    Private Const g_strUIDtxtTramitada As String = "txtTram"

    Private Const g_strUIDbtnAdd As String = "btnAdd"
    Private Const g_strUIDbtnEli As String = "btnEli"
    Private Const g_strUIDbtnCargM As String = "btnCarMas"

    Private Const g_strUIDmtxVehiculos As String = "mtxVehi"

    Private Const g_strUIDTabVehiculos As String = "tbVehi"

    Private Const g_strUIDMatrizSN As String = "1320000034"
    Private Const g_strUID1 As String = "1320000001"
    Private Const g_strUIDCodCnpSAP As String = "1320000004"
    Private Const g_strUIDColCardCode As String = "1320000001"
    Private Const g_strUIDColCardName As String = "1320000003"
    Private Const g_strUIDColGroupName As String = "1320000005"
    Private Const g_strUIDColStatus As String = "1320000009"
    Private Const g_strUIDColPhone1 As String = "1320000019"
    Private Const g_strUIDColCellular As String = "1320000021"
    Private Const g_strUIDColFax As String = "1320000023"
    Private Const g_strUIDEditTextStatus As String = "1320000017"
    Private Const g_strUIDColUnidad As String = "U_SCGD_Unidad"
    Private Const g_strUIDTabSN As String = "1320000026"

    Private Const g_strUIDColDMSStatus As String = "Col_es"

    Private Const g_strDTVehiculo As String = "dtVehiculo"
    Private Const g_strDTVehiculoGuardado As String = "dtVehiculoG"
    Private Const g_strdtVIN As String = "dtVIN"
    Private Const g_strVehiculoCargaMasiva As String = "dtCargaMasiva"

    Private Const g_intPanel As Integer = 5

    Private Const g_strMatrizReferencia As String = "1320000037"
    Private Const g_strButtonReferencia As String = "1320000038"

    Private Const g_strVehiculosXCampana As String = "@SCGD_VEHIXCAMP"

    Private g_intRowEvento As Integer = -1

    Private g_strCodeSap As String = String.Empty
    Private g_strDireccion As String = String.Empty

    Private g_dtUnidadesExcelSBO As SAPbouiCOM.DataTable
    Private g_oForm As SAPbouiCOM.Form

    Private g_lsUnidadesAEliminar As Generic.IList(Of VehiculoCnp) = New List(Of VehiculoCnp)
    Private g_lsUnidadesAIngresar As Generic.IList(Of VehiculoCnp) = New List(Of VehiculoCnp)

    Private g_lsUnidadesExistentes As Generic.IList(Of VehiculoCnp) = New List(Of VehiculoCnp)

    Private g_blActualizar As Boolean = False
    Private g_blOrdenado As Boolean = False

    Private g_intVehiculosIngresados As Integer = 0

    'estados
    Private Const g_strPendiente As String = "1"
    Private Const g_strRealizada As String = "2"
    Private Const g_strSuspendida As String = "3"

#End Region



#Region "Constructor"
    Private _insertaVehiculos As Boolean

    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, _
                    ByVal ocompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        m_ocompany = ocompany

    End Sub
#End Region

#Region "Eventos"

    ''' <summary>
    ''' Manejo del evento itempressed
    ''' </summary>
    ''' <param name="FormUID">identificador del formulario</param>
    ''' <param name="pVal">objeto evento</param>
    ''' <param name="BubbleEvent">evento burbuja</param>
    ''' <remarks></remarks>
    Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        'Dim m_oForm As SAPbouiCOM.Form
        Dim m_oEditText As SAPbouiCOM.EditText
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oColumn As SAPbouiCOM.Column

        Try
            g_oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            g_oForm.Freeze(True)


            'Manejo del BeforeAction en el ItemPressed
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    'tab de vehiculos
                    Case g_strUIDTabVehiculos
                        g_oForm.PaneLevel = 5
                    Case g_strUID1
                        m_oEditText = DirectCast(g_oForm.Items.Item(g_strUIDCodCnpSAP).Specific, SAPbouiCOM.EditText)
                        g_strCodeSap = m_oEditText.Value.ToString.Trim()

                        Select Case g_oForm.Mode
                            Case BoFormMode.fm_UPDATE_MODE
                                g_blActualizar = True
                            Case Else
                                g_blActualizar = False
                        End Select
                End Select

                If pVal.Row = 0 Then
                    Select Case pVal.ColUID
                        Case "Col_uni"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_uni")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_mar"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_mar")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_pla"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_pla")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_est"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_est")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_mod"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_mod")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_cli"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_cli")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_es"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_es")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_ano"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_ano")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                        Case "Col_vin"
                            oMatrix = g_oForm.Items.Item("mtxVehi").Specific
                            oColumn = oMatrix.Columns.Item("Col_vin")
                            If (g_blOrdenado = False) Then
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending)
                                g_blOrdenado = True
                            Else
                                oColumn.TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending)
                                g_blOrdenado = False
                            End If
                    End Select
                End If
                

                'Manejo de ActionSuccess en el ItemPressed
            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    'boton 1
                    Case g_strUID1
                        If g_oForm.Mode = BoFormMode.fm_ADD_MODE And Not g_blActualizar Then
                            CrearCampanaDMS(g_strCodeSap, g_oForm)
                        ElseIf g_blActualizar Then
                            ActualizaCampanaDMS(g_strCodeSap, g_oForm)
                        End If

                        'boton eliminar
                    Case g_strUIDbtnEli

                        If Not g_oForm.Mode = BoFormMode.fm_ADD_MODE AndAlso Not g_oForm.Mode = BoFormMode.fm_UPDATE_MODE Then g_oForm.Mode = BoFormMode.fm_UPDATE_MODE

                        EliminarVehiculo(g_intRowEvento, g_oForm)

                        'matriz vehiculos
                    Case g_strUIDmtxVehiculos

                        Select Case pVal.ColUID
                            Case "Col_num"
                                g_intRowEvento = pVal.Row
                            Case "Col_es"
                                If Not g_oForm.Mode = BoFormMode.fm_ADD_MODE AndAlso Not g_oForm.Mode = BoFormMode.fm_UPDATE_MODE Then g_oForm.Mode = BoFormMode.fm_UPDATE_MODE
                        End Select

                        'boton carga masiva
                    Case g_strUIDbtnCargM
                        If g_oForm.Mode = BoFormMode.fm_OK_MODE Then g_oForm.Mode = BoFormMode.fm_UPDATE_MODE
                        g_oForm = g_oForm
                        Dim tr As System.Threading.Thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf ManejoOpenFileDialog))
                        tr.SetApartmentState(Threading.ApartmentState.STA)
                        tr.Start()

                End Select

            End If
            g_oForm.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    'obtiene la direccion del archivo, manejo del FileDialog
    Private Sub ManejoOpenFileDialog()
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\"
        openFileDialog1.Filter = "Archivos Excel(*.xlsx ; *.xls)|*.xlsx;*.xls"
        'openFileDialog1.Filter = "Archivos Excel(*.xlsx )|*.xlsx"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.Title = My.Resources.Resource.TituloBuscadorExcel
        openFileDialog1.Multiselect = False
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    ' Insert code to read the stream here.
                    g_strDireccion = openFileDialog1.FileName
                End If
            Catch Ex As Exception
                'Error al cargar el excel
                SBO_Application.StatusBar.SetText(
                            My.Resources.Resource.ErrorCargaExcel, _
                            SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open.
                If (myStream IsNot Nothing) Then
                    myStream.Close()

                    'se carga el excel en un datatable
                    Call CargaExcel()
                End If
            End Try
            'StrDireccion = openFileDialog1.FileName
            'Call CargaExcel(oForm)
        End If

    End Sub


    Private Sub CargaExcel()
        Try
            Dim strDir As String = ""
            Dim inserto As Boolean = False

            'declaracion de objetos excel
            Dim oAppExcel As Excel.Application
            Dim oLibroExcel As Excel.Workbook
            Dim oHojaExcel As Excel.Worksheet

            If Not String.IsNullOrEmpty(g_strDireccion) Then

                oAppExcel = New Excel.ApplicationClass
                oLibroExcel = oAppExcel.Workbooks.Open(g_strDireccion, 0, True, 5, "", "", True, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", False, False, 0, True, False, Microsoft.Office.Interop.Excel.XlCorruptLoad.xlNormalLoad)
                oHojaExcel = oLibroExcel.Worksheets(1)

                Dim contadorCelda As Integer = 1
                Dim m_strVIN As String = ""
                Dim strPrecio As String = ""
                'limpiar el datatable 
                g_dtUnidadesExcelSBO.Rows.Clear()
                'Realizando carga excel
                SBO_Application.StatusBar.SetText(
                            My.Resources.Resource.RealizandoCargaExcel, _
                            SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                'carga el datatable con los datos de excel
                While Not String.IsNullOrEmpty(oHojaExcel.Cells(contadorCelda + 1, 1).Value)
                    m_strVIN = oHojaExcel.Cells(contadorCelda + 1, 1).Value
                    g_dtUnidadesExcelSBO.Rows.Add(1)
                    g_dtUnidadesExcelSBO.SetValue("vin", contadorCelda - 1, m_strVIN)
                    contadorCelda = contadorCelda + 1
                End While

                oLibroExcel.Close()
                oAppExcel.Quit()

                'insertar unidades existentes
                If Not g_dtUnidadesExcelSBO Is Nothing Then

                    If g_dtUnidadesExcelSBO.Rows.Count > 0 Then
                        'Inserta las unidades en el Contrato de Ventas
                        inserto = CargaVehiculosMasivos(g_dtUnidadesExcelSBO)
                        If inserto Then
                            SBO_Application.StatusBar.SetText(String.Format(My.Resources.Resource.MensajeCargaMasiva, g_intVehiculosIngresados.ToString(), g_dtUnidadesExcelSBO.Rows.Count.ToString()), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub



    ''' <summary>
    ''' Manejo del evento de carga del formulario
    ''' </summary>
    ''' <param name="FormUID">identificador del formulario</param>
    ''' <param name="pVal">objeto evento</param>
    ''' <param name="BubbleEvent">evento burbuja</param>
    ''' <remarks></remarks>
    Sub ManejadorEventoLoad(ByVal FormUID As String, _
                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                            ByRef BubbleEvent As Boolean)

        'Dim m_oForm As SAPbouiCOM.Form
        Dim oitem As SAPbouiCOM.Item
        Dim onewitem As SAPbouiCOM.Item
        Dim ofolder As SAPbouiCOM.Folder
        Dim strEtiquetaTab As String

        Try
            g_oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.BeforeAction Then

                'agregar tab de vehiculos
                oitem = g_oForm.Items.Item("1320000028")
                onewitem = g_oForm.Items.Add(g_strUIDTabVehiculos, SAPbouiCOM.BoFormItemTypes.it_FOLDER)
                onewitem.Left = oitem.Left + oitem.Width
                onewitem.Width = oitem.Width
                onewitem.Top = oitem.Top
                onewitem.Height = oitem.Height
                onewitem.AffectsFormMode = False

                ofolder = onewitem.Specific

                strEtiquetaTab = My.Resources.Resource.CapVehiculos
                ofolder.Caption = strEtiquetaTab

                ofolder.GroupWith("1320000028")

                'agregar controles 
                AgregaControlesTabVehiculos(g_oForm)

                InizializaTablaVehiculos(g_oForm)

                CargarValidValuesEnCombos(g_oForm, "select code, name from [@SCGD_ESTVEHIXCAMP]", g_strUIDmtxVehiculos)

                g_dtVehiculosGuardadosCampanas = g_oForm.DataSources.DataTables.Add(g_strDTVehiculoGuardado)
                g_dtUnidadesExcelSBO = g_oForm.DataSources.DataTables.Add(g_strdtVIN)
                g_dtUnidadesExcelSBO.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)

                g_dtVehiculoCargaMasiva = g_oForm.DataSources.DataTables.Add(g_strVehiculoCargaMasiva)

                g_lsUnidadesAIngresar.Clear()
                g_lsUnidadesAEliminar.Clear()

                ManejaEstadoComponentes(g_oForm)

            ElseIf pVal.ActionSuccess Then
                LimpiaInfoCampanasDMS()
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Maneja los eventos ChooseFromList 
    ''' Agrega una única unidad a la matriz de vehículos.
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <param name="pVal">objeto evento</param>
    ''' <param name="BubbleEvent">evento burbuja</param>
    ''' <remarks></remarks>
    Sub ManejadorEventoChooseFromList(ByVal FormUID As String, ByVal pVal As ItemEvent, ByVal BubbleEvent As Boolean)

        Dim m_oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim m_oCFL As SAPbouiCOM.ChooseFromList
        Dim m_sCFL_ID As String
        Dim m_oDataTable As SAPbouiCOM.DataTable

        'Dim m_oForm As SAPbouiCOM.Form
        Dim m_oMatrix As SAPbouiCOM.Matrix
        Dim m_intFila As Integer = 0
        Dim m_intIndSN As Integer = 0

        Dim oCol As SAPbouiCOM.Column

        Try
            m_oCFLEvento = pVal
            m_sCFL_ID = m_oCFLEvento.ChooseFromListUID
            g_oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If m_oCFLEvento.ActionSuccess Then

                m_oDataTable = m_oCFLEvento.SelectedObjects
                m_oMatrix = DirectCast(g_oForm.Items.Item("mtxVehi").Specific, SAPbouiCOM.Matrix)

                g_dtVehiculosCampana = g_oForm.DataSources.DataTables.Item(g_strDTVehiculo)
                g_dtVehiculosCampana.Rows.Clear()

                m_oMatrix.FlushToDataSource()

                m_intFila = g_dtVehiculosCampana.Rows.Count - 1

                If Not m_oDataTable Is Nothing Then
                    Select Case pVal.ItemUID

                        Case g_strUIDbtnAdd

                            If g_oForm.Mode = BoFormMode.fm_OK_MODE Then g_oForm.Mode = BoFormMode.fm_UPDATE_MODE

                            If Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("uni", m_intFila)) And
                                Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("pla", m_intFila)) And
                                Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("vin", m_intFila)) Then
                                g_dtVehiculosCampana.Rows.Add(1)
                                m_intFila += 1
                            End If

                            IngresaUnidad(m_oDataTable.GetValue("U_Cod_Unid", 0),
                                          m_oDataTable.GetValue("U_Num_Plac", 0),
                                          m_oDataTable.GetValue("U_Num_VIN", 0),
                                          m_oDataTable.GetValue("U_Des_Marc", 0),
                                          m_oDataTable.GetValue("U_Des_Esti", 0),
                                          m_oDataTable.GetValue("U_Des_Mode", 0),
                                          m_oDataTable.GetValue("U_CardCode", 0),
                                          "",
                                          m_oDataTable.GetValue("U_Ano_Vehi", 0),
                                          False)


                            If String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("uni", m_intFila)) And
                                 String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("pla", m_intFila)) And
                                 String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("vin", m_intFila)) Then
                                g_dtVehiculosCampana.Rows.Remove(m_intFila)
                                m_intFila -= 1
                            End If

                            m_oMatrix.LoadFromDataSource()

                    End Select

                End If
            ElseIf m_oCFLEvento.Before_Action Then



            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Sub ManejoFormDataLoad(ByVal p_oForm As Form, ByRef BubbleEvent As Boolean)
        Dim m_oEdit As SAPbouiCOM.EditText
        Dim m_strCodeCnpSap As String = String.Empty

        Try
            m_oEdit = DirectCast(p_oForm.Items.Item(g_strUIDCodCnpSAP).Specific, SAPbouiCOM.EditText)
            m_strCodeCnpSap = m_oEdit.Value

            g_lsUnidadesAIngresar.Clear()
            g_lsUnidadesAEliminar.Clear()

            CargaVehiculosAlNavegarCampana(m_strCodeCnpSap, p_oForm)

            ManejaEstadoComponentes(p_oForm)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                            ByVal strQuery As String, _
                                                            ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try
            oItem = oForm.Items.Item(strIDItem)
            oMatrix = CType(oItem.Specific, SAPbouiCOM.Matrix)

            Configuracion.CrearCadenaDeconexion(m_ocompany.Server, m_ocompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If oMatrix.Columns.Item("Col_es").ValidValues.Count > 0 Then
                For intRecIndex = 0 To oMatrix.Columns.Item("Col_es").ValidValues.Count - 1
                    oMatrix.Columns.Item("Col_es").ValidValues.Remove(oMatrix.Columns.Item("Col_es").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

                    oMatrix.Columns.Item("Col_es").ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Crea la campaña de DMS
    ''' </summary>
    ''' <param name="strCodeSap">Codigo de campaña de SAP</param>
    ''' <param name="p_oForm">Objeto Formulario</param>
    ''' <remarks></remarks>
    Private Sub CrearCampanaDMS(ByVal strCodeSap As String, ByVal p_oForm As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim udoCampana As UDOCampana
        Dim EncabezadoCampana As EncabezadoUDOCampana
        Dim VehiculoCampana As VehiculoUDOCampana
        Dim m_oVehiculo As VehiculoCnp

        Try
            p_oForm.Freeze(True)
            udoCampana = New UDOCampana(m_ocompany, "SCGD_CAMPANA")
            EncabezadoCampana = New EncabezadoUDOCampana
            udoCampana.ListaVehiculos = New ListaVehiculosUDOCampana()
            udoCampana.ListaVehiculos.LineasUDO = New List(Of ILineaUDO)()

            EncabezadoCampana.CodCampSap = strCodeSap

            udoCampana.Encabezado = EncabezadoCampana
            g_dtVehiculosCampana.Rows.Clear()
            oMatrix = DirectCast(p_oForm.Items.Item("mtxVehi").Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()

            For i As Integer = 0 To g_dtVehiculosCampana.Rows.Count - 1
                m_oVehiculo.s_strUnidad = g_dtVehiculosCampana.GetValue("uni", i)
                m_oVehiculo.s_strPlaca = g_dtVehiculosCampana.GetValue("pla", i)
                m_oVehiculo.s_strVIN = g_dtVehiculosCampana.GetValue("vin", i)

                If g_lsUnidadesAIngresar.Contains(m_oVehiculo) Then

                    VehiculoCampana = New VehiculoUDOCampana()
                    VehiculoCampana.Unidad = g_dtVehiculosCampana.GetValue("uni", i)
                    VehiculoCampana.Placa = g_dtVehiculosCampana.GetValue("pla", i)
                    VehiculoCampana.VIN = g_dtVehiculosCampana.GetValue("vin", i)
                    VehiculoCampana.Marca = g_dtVehiculosCampana.GetValue("mar", i)
                    VehiculoCampana.Estilo = g_dtVehiculosCampana.GetValue("est", i)
                    VehiculoCampana.Modelo = g_dtVehiculosCampana.GetValue("mod", i)
                    VehiculoCampana.Cliente = g_dtVehiculosCampana.GetValue("cli", i)
                    VehiculoCampana.Estado = g_dtVehiculosCampana.GetValue("es", i)
                    VehiculoCampana.Ano = g_dtVehiculosCampana.GetValue("ano", i)

                    udoCampana.ListaVehiculos.LineasUDO.Add(VehiculoCampana)
                End If
            Next

            LimpiaInfoCampanasDMS()

            udoCampana.Insert()

            If udoCampana.LastErrorCode <> 0 Then
                If m_ocompany.InTransaction Then
                    m_ocompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            End If
            p_oForm.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Private Sub ActualizaCampanaDMS(ByVal p_strCodeSap As String, ByVal p_oForm As Form)

        Dim m_oMatrix As SAPbouiCOM.Matrix

        Dim oCompanyService As CompanyService
        Dim oGeneralService As GeneralService
        Dim oGeneralParams As GeneralDataParams
        Dim oGeneralData As GeneralData
        Dim oGeneralDataChildCollection As GeneralDataCollection
        Dim oGeneralDataChild As GeneralData

        Dim m_strDocEntryCampana As String = String.Empty
        Dim m_strConsultaCodeCnp As String = "select DocEntry from [@SCGD_CAMPANA] where U_CampSap = '{0}'"

        Dim m_intUnidadesAEliminar As Integer = 0
        Dim m_intUnidadesAIngresar As Integer = 0

        Dim m_oEdit As SAPbouiCOM.EditText
        Dim m_intCont As Integer = 0
        Dim m_intContTram As Integer = 0

        Try
            g_dtVehiculosCampana.Rows.Clear()
            m_oMatrix = DirectCast(p_oForm.Items.Item("mtxVehi").Specific, SAPbouiCOM.Matrix)
            m_oMatrix.FlushToDataSource()

            m_strDocEntryCampana = Utilitarios.EjecutarConsulta(String.Format(m_strConsultaCodeCnp, p_strCodeSap),
                                                            m_ocompany.CompanyDB, m_ocompany.Server)

            oCompanyService = m_ocompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CAMPANA")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", m_strDocEntryCampana)

            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            m_intUnidadesAEliminar = g_lsUnidadesAEliminar.Count
            m_intUnidadesAIngresar = g_lsUnidadesAIngresar.Count

            oGeneralDataChildCollection = oGeneralData.Child("SCGD_VEHIXCAMP")

            If m_intUnidadesAEliminar > 0 Then EliminaLineasCampanaDMS(oGeneralDataChildCollection, m_intUnidadesAEliminar, oGeneralService)

            If m_intUnidadesAIngresar > 0 Then IngresaLineasCampanaDMS(oGeneralDataChildCollection, m_intUnidadesAIngresar)

            For i As Integer = 0 To g_dtVehiculosCampana.Rows.Count - 1
                If Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("uni", i)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("pla", i)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("vin", i)) Then

                    oGeneralDataChildCollection = oGeneralData.Child("SCGD_VEHIXCAMP")
                    oGeneralDataChild = oGeneralDataChildCollection.Item(i)
                    oGeneralDataChild.SetProperty("U_Estado", g_dtVehiculosCampana.GetValue("es", i))
                    m_intCont = m_intCont + 1

                    If g_dtVehiculosCampana.GetValue("es", i) = g_strRealizada Then
                        m_intContTram = m_intContTram + 1
                    End If
                End If
            Next



            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtPorTramitar).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = m_intCont

            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtTramitada).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = m_intContTram

            oGeneralService.Update(oGeneralData)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Private Sub IngresaLineasCampanaDMS(ByRef p_oGeneralDataChildCollection As GeneralDataCollection, ByVal p_intUnidadesAIngresar As Integer)

        Dim m_oChild As SAPbobsCOM.GeneralData
        Dim m_oVehiculos As VehiculoCnp

        Try

            For Each m_oVehiculo As VehiculoCnp In g_lsUnidadesAIngresar
                For i As Integer = 0 To g_dtVehiculosCampana.Rows.Count - 1

                    If m_oVehiculo.s_strUnidad = g_dtVehiculosCampana.GetValue("uni", i) And
                        m_oVehiculo.s_strPlaca = g_dtVehiculosCampana.GetValue("pla", i) And
                        m_oVehiculo.s_strVIN = g_dtVehiculosCampana.GetValue("vin", i) Then

                        m_oChild = p_oGeneralDataChildCollection.Add()
                        m_oChild.SetProperty("U_Unidad", g_dtVehiculosCampana.GetValue("uni", i))
                        m_oChild.SetProperty("U_Placa", g_dtVehiculosCampana.GetValue("pla", i))
                        m_oChild.SetProperty("U_Vin", g_dtVehiculosCampana.GetValue("vin", i))
                        m_oChild.SetProperty("U_Marca", g_dtVehiculosCampana.GetValue("mar", i))
                        m_oChild.SetProperty("U_Estilo", g_dtVehiculosCampana.GetValue("est", i))
                        m_oChild.SetProperty("U_Modelo", g_dtVehiculosCampana.GetValue("mod", i))
                        m_oChild.SetProperty("U_Cliente", g_dtVehiculosCampana.GetValue("cli", i))
                        m_oChild.SetProperty("U_Estado", g_dtVehiculosCampana.GetValue("es", i))
                        m_oChild.SetProperty("U_Ano", g_dtVehiculosCampana.GetValue("ano", i))
                        Exit For

                    End If

                Next
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Private Sub EliminaLineasCampanaDMS(ByRef oGeneralDataChildCollection As GeneralDataCollection, ByVal m_intUnidadesAEliminar As Integer,
                                        ByRef oGeneralService As SAPbobsCOM.GeneralService)

        Dim m_intContador As Integer = 0
        Dim m_intUnidadesEliminadas As Integer = 0
        Dim m_oVehiculo As VehiculoCnp

        Try
            m_intContador = 0
            m_intUnidadesEliminadas = 0

            For i As Integer = 0 To oGeneralDataChildCollection.Count

                m_oVehiculo.s_strUnidad = oGeneralDataChildCollection.Item(i).GetProperty("U_Unidad")
                m_oVehiculo.s_strPlaca = oGeneralDataChildCollection.Item(i).GetProperty("U_Placa")
                m_oVehiculo.s_strVIN = oGeneralDataChildCollection.Item(i).GetProperty("U_Vin")

                If g_lsUnidadesAEliminar.Contains(m_oVehiculo) Then

                    oGeneralDataChildCollection.Remove(i)
                    m_intUnidadesEliminadas = m_intUnidadesEliminadas + 1

                End If
                If m_intUnidadesEliminadas = m_intUnidadesAEliminar Then Exit For
                m_intContador = m_intContador + 1
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el cliente asociada a la unidad que se incorpora a la matriz
    ''' </summary>
    ''' <param name="p_strCodCliente">Cod cliente a insertar</param>
    ''' <param name="p_oForm">Objeto formularo</param>
    ''' <remarks></remarks>
    Private Sub AgregaCliente(ByVal p_strCodCliente As String, ByVal p_strUnidad As String, ByRef p_oForm As SAPbouiCOM.Form)

        Dim m_oMatrix As SAPbouiCOM.Matrix
        Dim m_intIndice As Integer = 0

        Try

            m_oMatrix = DirectCast(p_oForm.Items.Item(g_strUIDMatrizSN).Specific, SAPbouiCOM.Matrix)

            m_intIndice = m_oMatrix.RowCount

            'p_oForm.DataSources.DBDataSources.Item("CPN1").SetValue(m_intIndice, g_strUIDColCardCode, p_strCodCliente)
            'p_oForm.DataSources.DBDataSources.Item("CPN1").SetValue(m_intIndice, g_strUIDColUnidad, p_strUnidad)

            p_oForm.Items.Item(g_strUIDTabSN).Click(SAPbouiCOM.BoCellClickType.ct_Regular)

            m_oMatrix.Columns.Item(g_strUIDColCardCode).Cells.Item(m_intIndice).Specific.String =
                p_strCodCliente

            m_oMatrix.Columns.Item(g_strUIDColUnidad).Cells.Item(m_intIndice).Specific.String =
                p_strUnidad

            p_oForm.Items.Item(g_strUIDTabVehiculos).Click(SAPbouiCOM.BoCellClickType.ct_Regular)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la unidad de la matriz de vehículos y el cliente asociado a esa unidad
    ''' </summary>
    ''' <param name="p_intIndiceEliminar">El indice a eliminar</param>
    ''' <param name="p_oForm">Formulario</param>
    ''' <remarks></remarks>
    Private Sub EliminarVehiculo(ByRef p_intIndiceEliminar As Integer, ByRef p_oForm As Form)

        Dim m_oMatrixVehiculos As SAPbouiCOM.Matrix
        Dim m_oMatrixSN As SAPbouiCOM.Matrix
        Dim m_strUnidadEliminar As String = String.Empty
        Dim m_strPlacaEliminar As String = String.Empty
        Dim m_strVINEliminar As String = String.Empty
        Dim m_oVehiculoCNP As VehiculoCnp

        Try
            If Not p_intIndiceEliminar = -1 Then

                m_oMatrixVehiculos = DirectCast(p_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)
                m_oMatrixVehiculos.FlushToDataSource()

                m_oMatrixSN = DirectCast(p_oForm.Items.Item(g_strUIDMatrizSN).Specific, SAPbouiCOM.Matrix)

                If Not g_dtVehiculosCampana.Rows.Count < p_intIndiceEliminar Then

                    m_strUnidadEliminar = g_dtVehiculosCampana.GetValue("uni", p_intIndiceEliminar - 1)
                    m_strPlacaEliminar = g_dtVehiculosCampana.GetValue("pla", p_intIndiceEliminar - 1)
                    m_strVINEliminar = g_dtVehiculosCampana.GetValue("vin", p_intIndiceEliminar - 1)

                    For i As Integer = 1 To m_oMatrixSN.RowCount - 1

                        If m_oMatrixSN.Columns.Item(g_strUIDColUnidad).Cells.Item(i).Specific.String = m_strUnidadEliminar Then
                            m_oMatrixSN.DeleteRow(i)
                            Exit For
                        End If

                    Next

                    g_dtVehiculosCampana.Rows.Remove(p_intIndiceEliminar - 1)
                    m_oMatrixVehiculos.LoadFromDataSource()

                    p_intIndiceEliminar = -1

                    AumentaDisminuyeVehiPTram(1, p_oForm, False)

                    m_oVehiculoCNP.s_strUnidad = m_strUnidadEliminar
                    m_oVehiculoCNP.s_strPlaca = m_strPlacaEliminar
                    m_oVehiculoCNP.s_strVIN = m_strVINEliminar

                    If g_lsUnidadesExistentes.Contains(m_oVehiculoCNP) Then g_lsUnidadesExistentes.Remove(m_oVehiculoCNP)
                    If g_lsUnidadesAIngresar.Contains(m_oVehiculoCNP) Then g_lsUnidadesAIngresar.Remove(m_oVehiculoCNP)

                    If Not g_lsUnidadesAEliminar.Contains(m_oVehiculoCNP) Then g_lsUnidadesAEliminar.Add(m_oVehiculoCNP)


                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Aumenta o disminuye el numero en vehículos por tramitar
    ''' </summary>
    ''' <param name="p_intCantidad">cantidad a aumentar o disminuir</param>
    ''' <param name="p_oForm">objeto formulario</param>
    ''' <param name="p_Suma">true = sumar -- false = restar</param>
    ''' <remarks></remarks>
    Private Sub AumentaDisminuyeVehiPTram(ByVal p_intCantidad As Integer, ByVal p_oForm As SAPbouiCOM.Form, ByVal p_Suma As Boolean)

        Dim m_oEdit As SAPbouiCOM.EditText
        Dim m_strCantidadActual As String = String.Empty
        Dim m_intCantidadActual As Integer = 0

        Try

            m_oEdit = DirectCast(p_oForm.Items.Item(g_strUIDtxtPorTramitar).Specific, SAPbouiCOM.EditText)
            m_strCantidadActual = m_oEdit.Value
            If Not String.IsNullOrEmpty(m_strCantidadActual) Then
                m_intCantidadActual = Integer.Parse(m_strCantidadActual)
            End If

            If p_Suma Then
                m_oEdit.Value = m_intCantidadActual + p_intCantidad
            Else
                m_oEdit.Value = m_intCantidadActual - p_intCantidad
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los vehiculos de la campaña DMS de acuerdo a la campaña asociada a la 
    ''' campaña de SAP
    ''' </summary>
    ''' <param name="p_strCodeCnpSap">Codigo de la campaña de sap</param>
    ''' <param name="p_oForm">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub CargaVehiculosAlNavegarCampana(ByVal p_strCodeCnpSap As String, ByVal p_oForm As Form)

        Dim m_strConsultaVehiculos As String =
        " select U_Unidad as uni, U_Placa as pla, U_Marca as mar, U_Estilo as est, U_Modelo as mod," +
        " U_Cliente as cli, U_Ano as ano, U_Estado as es , U_Vin as vin " +
        " from [@SCGD_VEHIXCAMP] vxc " +
        " inner join [@SCGD_CAMPANA] can on vxc.DocEntry = can.DocEntry " +
        " where can.U_CampSap = '{0}' "

        Dim m_oMatrix As SAPbouiCOM.Matrix
        Dim m_oEdit As SAPbouiCOM.EditText
        Dim m_intContador As Integer = 0
        Dim m_intContadorTramitados As Integer = 0
        Dim m_oVehiculoCNP As VehiculoCnp

        Try

            g_lsUnidadesExistentes.Clear()

            m_oMatrix = DirectCast(p_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)

            g_dtVehiculosCampana.Rows.Clear()
            g_dtVehiculosGuardadosCampanas.Rows.Clear()

            g_dtVehiculosGuardadosCampanas.ExecuteQuery(String.Format(m_strConsultaVehiculos, p_strCodeCnpSap))

            For i As Integer = 0 To g_dtVehiculosGuardadosCampanas.Rows.Count - 1
                m_oVehiculoCNP = New VehiculoCnp

                g_dtVehiculosCampana.Rows.Add(1)
                g_dtVehiculosCampana.SetValue("uni", i, g_dtVehiculosGuardadosCampanas.GetValue("uni", i))
                g_dtVehiculosCampana.SetValue("pla", i, g_dtVehiculosGuardadosCampanas.GetValue("pla", i))
                g_dtVehiculosCampana.SetValue("vin", i, g_dtVehiculosGuardadosCampanas.GetValue("vin", i))
                g_dtVehiculosCampana.SetValue("mar", i, g_dtVehiculosGuardadosCampanas.GetValue("mar", i))
                g_dtVehiculosCampana.SetValue("est", i, g_dtVehiculosGuardadosCampanas.GetValue("est", i))
                g_dtVehiculosCampana.SetValue("mod", i, g_dtVehiculosGuardadosCampanas.GetValue("mod", i))
                g_dtVehiculosCampana.SetValue("cli", i, g_dtVehiculosGuardadosCampanas.GetValue("cli", i))
                g_dtVehiculosCampana.SetValue("es", i, g_dtVehiculosGuardadosCampanas.GetValue("es", i))
                g_dtVehiculosCampana.SetValue("ano", i, g_dtVehiculosGuardadosCampanas.GetValue("ano", i))

                If Not String.IsNullOrEmpty(g_dtVehiculosGuardadosCampanas.GetValue("uni", i)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosGuardadosCampanas.GetValue("pla", i)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosGuardadosCampanas.GetValue("vin", i)) Then
                    m_intContador = m_intContador + 1

                    If g_dtVehiculosGuardadosCampanas.GetValue("es", i) = g_strRealizada Then
                        m_intContadorTramitados = m_intContadorTramitados + 1
                    End If

                End If

                m_oVehiculoCNP.s_strUnidad = g_dtVehiculosGuardadosCampanas.GetValue("uni", i)
                m_oVehiculoCNP.s_strPlaca = g_dtVehiculosGuardadosCampanas.GetValue("pla", i)
                m_oVehiculoCNP.s_strVIN = g_dtVehiculosGuardadosCampanas.GetValue("vin", i)

                g_lsUnidadesExistentes.Add(m_oVehiculoCNP)

            Next

            m_oMatrix.LoadFromDataSource()


            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtPorTramitar).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = m_intContador

            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtTramitada).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = m_intContadorTramitados

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Carga vehículos de forma masiva
    ''' </summary>
    ''' <param name="p_dtUnidadesExcelSBO">DataTable con las unidades que se cargan del excel</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargaVehiculosMasivos(ByVal p_dtUnidadesExcelSBO As DataTable) As Boolean

        Dim m_oMatrix As SAPbouiCOM.Matrix
        Dim m_strConsultaVehiculos As String = "select U_Cod_Unid as uni, U_Num_Plac as pla, U_Num_VIN as vin, U_Des_Marc as mar, U_Des_Esti as est, U_Des_Mode as mod, " +
                                                "U_CardCode as cli, U_Ano_Vehi as ano from [@SCGD_VEHICULO] where U_Num_VIN in ({0})"
        Dim m_strVIN As String = String.Empty
        Dim m_intCont As Integer = 0
        Dim m_intCargaMasivaCont As Integer = 0
        Dim m_intTamanoActual As Integer = 0
        Dim m_intTamanoFinal As Integer = 0

        Try
            g_oForm.Freeze(True)
            For i As Integer = 0 To p_dtUnidadesExcelSBO.Rows.Count - 1
                If m_intCont <> 0 Then
                    m_strVIN += ", "
                End If
                m_strVIN += "'" + p_dtUnidadesExcelSBO.GetValue("vin", i) + "'"
                m_intCont = 1
            Next

            m_oMatrix = DirectCast(g_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)
            m_oMatrix.FlushToDataSource()

            m_intTamanoActual = g_dtVehiculosCampana.Rows.Count - 1

            If m_intTamanoActual < 0 Then m_intTamanoActual = 0

            g_dtVehiculoCargaMasiva.Rows.Clear()
            g_dtVehiculoCargaMasiva.ExecuteQuery(String.Format(m_strConsultaVehiculos, m_strVIN))

            m_intTamanoFinal = g_dtVehiculoCargaMasiva.Rows.Count - 1 + m_intTamanoActual

            g_intVehiculosIngresados = 0

            For i As Integer = 0 To g_dtVehiculoCargaMasiva.Rows.Count - 1

                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoUnidadCargaMasiva + (i + 1).ToString + My.Resources.Resource.DeUnTotalDe + g_dtVehiculoCargaMasiva.Rows.Count.ToString, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                IngresaUnidad(g_dtVehiculoCargaMasiva.GetValue("uni", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("pla", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("vin", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("mar", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("est", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("mod", m_intCargaMasivaCont),
                              g_dtVehiculoCargaMasiva.GetValue("cli", m_intCargaMasivaCont),
                              g_strPendiente,
                              g_dtVehiculoCargaMasiva.GetValue("ano", m_intCargaMasivaCont),
                              True)
                m_intCargaMasivaCont = m_intCargaMasivaCont + 1
            Next

            m_oMatrix.LoadFromDataSource()
            g_oForm.Freeze(False)
            Return True
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    'Ingresar unidades a la matriz de vehículos
    Private Function IngresaUnidad(ByVal p_strUnidad As String,
                                   ByVal p_strPlaca As String,
                                   ByVal p_strVIN As String,
                                   ByVal p_strMarca As String,
                                   ByVal p_strEstilo As String,
                                   ByVal p_strModelo As String,
                                   ByVal p_strCliente As String,
                                   ByVal p_strEstado As String,
                                   ByVal p_strAno As String,
                                   ByVal p_CargaMasiva As Boolean)

        Dim m_oVehiculoCNP As VehiculoCnp
        Dim m_intPosicion As Integer = 0

        Try
            m_oVehiculoCNP.s_strUnidad = p_strUnidad
            m_oVehiculoCNP.s_strPlaca = p_strPlaca
            m_oVehiculoCNP.s_strVIN = p_strVIN

            m_intPosicion = g_dtVehiculosCampana.Rows.Count - 1

            If g_lsUnidadesAEliminar.Contains(m_oVehiculoCNP) Then g_lsUnidadesAEliminar.Remove(m_oVehiculoCNP)

            If Not g_lsUnidadesExistentes.Contains(m_oVehiculoCNP) AndAlso
                Not g_lsUnidadesAIngresar.Contains(m_oVehiculoCNP) Then

                g_lsUnidadesAIngresar.Add(m_oVehiculoCNP)

                If Not m_intPosicion = 0 AndAlso
                    Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("uni", m_intPosicion)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("pla", m_intPosicion)) Or
                    Not String.IsNullOrEmpty(g_dtVehiculosCampana.GetValue("vin", m_intPosicion)) Then

                    g_dtVehiculosCampana.Rows.Add(1)
                    m_intPosicion = g_dtVehiculosCampana.Rows.Count - 1

                End If

                g_dtVehiculosCampana.SetValue("uni", m_intPosicion, p_strUnidad)
                g_dtVehiculosCampana.SetValue("pla", m_intPosicion, p_strPlaca)
                g_dtVehiculosCampana.SetValue("vin", m_intPosicion, p_strVIN)
                g_dtVehiculosCampana.SetValue("mar", m_intPosicion, p_strMarca)
                g_dtVehiculosCampana.SetValue("est", m_intPosicion, p_strEstilo)
                g_dtVehiculosCampana.SetValue("mod", m_intPosicion, p_strModelo)
                g_dtVehiculosCampana.SetValue("cli", m_intPosicion, p_strCliente)
                If Not String.IsNullOrEmpty(p_strCliente) Then
                    AgregaCliente(p_strCliente, p_strUnidad, g_oForm)
                End If
                g_dtVehiculosCampana.SetValue("es", m_intPosicion, g_strPendiente)
                g_dtVehiculosCampana.SetValue("ano", m_intPosicion, p_strAno)

                AumentaDisminuyeVehiPTram(1, g_oForm, True)

                If p_CargaMasiva Then g_intVehiculosIngresados = g_intVehiculosIngresados + 1

            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.LaUnidad + " " + p_strUnidad + My.Resources.Resource.YaIngresoAlaCampana, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Function

    Public Sub LimpiaInfoCampanasDMS()

        Dim m_oMatrix As SAPbouiCOM.Matrix
        Dim m_oEdit As SAPbouiCOM.EditText
        Try

            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtPorTramitar).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = String.Empty

            m_oEdit = DirectCast(g_oForm.Items.Item(g_strUIDtxtTramitada).Specific, SAPbouiCOM.EditText)
            m_oEdit.Value = String.Empty

            m_oMatrix = DirectCast(g_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)
            m_oMatrix.FlushToDataSource()

            g_dtVehiculosCampana.Rows.Clear()

            m_oMatrix.LoadFromDataSource()

            g_lsUnidadesExistentes.Clear()
            g_lsUnidadesAIngresar.Clear()
            g_lsUnidadesAEliminar.Clear()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

#End Region

#Region "Agrega Componentes a form"


    ''' <summary>
    ''' Agrega los controles al tab de vehiculos 
    ''' </summary>
    ''' <param name="m_oForm">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub AgregaControlesTabVehiculos(ByVal m_oForm As SAPbouiCOM.Form)

        Dim oButton As SAPbouiCOM.Button
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oitem_Matriz As SAPbouiCOM.Item
        Dim intLeftActual As Integer = 0
        Dim intTopActual As Integer = 0

        Dim intTop_Matriz As Integer = 0
        Dim intLeft_Matriz As Integer = 0
        Dim intHeight_Matriz As Integer = 0
        Dim intWidth_Matriz As Integer = 0

        Try

            'item de referencia
            oitem = m_oForm.Items.Item(g_strButtonReferencia)
            intTopActual = oitem.Top
            intLeftActual = oitem.Left - 5

            oitem_Matriz = m_oForm.Items.Item(g_strMatrizReferencia)
            intTop_Matriz = oitem_Matriz.Top
            intLeft_Matriz = oitem_Matriz.Left
            intHeight_Matriz = oitem_Matriz.Height
            intWidth_Matriz = oitem_Matriz.Width

            intTopActual = intTopActual + 35

            'ChooseFromList
            AgregaChooseFromListVehiculos(m_oForm)

            m_oForm.DataSources.DBDataSources.Add(g_strVehiculosXCampana)
            m_oForm.DataSources.DBDataSources.Add("@SCGD_VEHICULO")

            'EditText y Label por trámitar
            oitem = AgregaStatics(m_oForm, g_strUIDlblPorTramitar, My.Resources.Resource.LabelPTram, intLeftActual, intTopActual, g_intPanel, g_intPanel, g_strUIDtxtPorTramitar)
            oitem.Enabled = False
            oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            intTopActual = intTopActual + 15

            oitem = AgregaEditText(m_oForm, g_strUIDtxtPorTramitar, intLeftActual, intTopActual, g_intPanel, g_intPanel, g_strUIDlblPorTramitar)
            oitem.Enabled = False
            oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            intTopActual = intTopActual + 17

            'EditText y Label Tramitados
            oitem = AgregaStatics(m_oForm, g_strUIDlblTramitada, My.Resources.Resource.LabelTrami, intLeftActual, intTopActual, g_intPanel, g_intPanel, g_strUIDtxtTramitada)
            oitem.Enabled = False
            oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            intTopActual = intTopActual + 15

            oitem = AgregaEditText(m_oForm, g_strUIDtxtTramitada, intLeftActual, intTopActual, g_intPanel, g_intPanel, g_strUIDlblTramitada)
            oitem.Enabled = False
            oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'oEdit = oitem.Specific
            'Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotAñoVehiclo)

            'agrega los botones
            'Boton Agregar
            intTopActual = intTopActual + 25

            oitem = AgregaButton(m_oForm, g_strUIDbtnAdd, intLeftActual, intTopActual, g_intPanel, g_intPanel, My.Resources.Resource.ButtonAgregar, SAPbouiCOM.BoButtonTypes.bt_Caption, True)
            oitem.Width = 80
            oitem.Height = 19
            oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

            'Boton Eliminar
            intTopActual = intTopActual + 25

            oitem = AgregaButton(m_oForm, g_strUIDbtnEli, intLeftActual, intTopActual, g_intPanel, g_intPanel, My.Resources.Resource.ButtonEliminar, SAPbouiCOM.BoButtonTypes.bt_Caption, False)
            oitem.Width = 80
            oitem.Height = 19
            oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

            'Boton Carga Masiva
            intTopActual = intTopActual + 25

            oitem = AgregaButton(m_oForm, g_strUIDbtnCargM, intLeftActual, intTopActual, g_intPanel, g_intPanel, My.Resources.Resource.ButtonCargaMas, SAPbouiCOM.BoButtonTypes.bt_Caption, False)
            oitem.Width = 80
            oitem.Height = 19
            oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

            'agrega matriz
            oitem = AgregaMatriz(m_oForm, g_strUIDmtxVehiculos, intLeft_Matriz, intTop_Matriz, intHeight_Matriz, intWidth_Matriz, g_intPanel, g_intPanel)
            oitem.Enabled = True
            oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oMatrix = oitem.Specific
            oMatrix.Columns.Item(g_strUIDColDMSStatus).DataBind.SetBound(True, g_strVehiculosXCampana, "U_Estado")

            'AsignaCFLColumn(m_oForm, "mtxVehi", "Col_uni", "CFL_Veh", "U_Cod_Unid")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub ManejaEstadoComponentes(ByRef p_oForm As Form)
        Dim m_oItem As SAPbouiCOM.Item
        Dim m_oEditText As SAPbouiCOM.EditText
        Dim m_oButton As SAPbouiCOM.Button
        Dim m_oCombo As SAPbouiCOM.ComboBox
        Dim m_oMatrix As SAPbouiCOM.Matrix

        Try

            m_oCombo = DirectCast(p_oForm.Items.Item(g_strUIDEditTextStatus).Specific, SAPbouiCOM.ComboBox)

            If m_oCombo.Value.Trim = "C" Then
                m_oItem = p_oForm.Items.Item(g_strUIDbtnAdd)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                m_oItem = p_oForm.Items.Item(g_strUIDbtnCargM)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                m_oItem = p_oForm.Items.Item(g_strUIDbtnEli)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

                m_oMatrix = DirectCast(p_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)
                m_oMatrix.Columns.Item("Col_es").Editable = False
            Else
                m_oItem = p_oForm.Items.Item(g_strUIDbtnAdd)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                m_oItem = p_oForm.Items.Item(g_strUIDbtnCargM)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                m_oItem = p_oForm.Items.Item(g_strUIDbtnEli)
                m_oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

                m_oMatrix = DirectCast(p_oForm.Items.Item(g_strUIDmtxVehiculos).Specific, SAPbouiCOM.Matrix)
                m_oMatrix.Columns.Item("Col_es").Editable = True
            End If


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega labels al formulario
    ''' </summary>
    ''' <param name="oform">formularo</param>
    ''' <param name="strNombrectrl">Nombre del componente</param>
    ''' <param name="strCaption">Caption a desplegar</param>
    ''' <param name="intLeft">Posicon en izquierda</param>
    ''' <param name="intTop">posicion arriba</param>
    ''' <param name="intFromPane">From Pane</param>
    ''' <param name="intTopane">To Pane</param>
    ''' <param name="strLinkTo">Asocionado al txt</param>
    ''' <param name="isVIN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AgregaStatics(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal strCaption As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String, _
                                   Optional ByVal isVIN As Boolean = False) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oStatic As SAPbouiCOM.StaticText
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oitem.Left = intLeft
            oitem.Top = intTop
            If isVIN Then
                oitem.Width = 40
            End If
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            'oitem.LinkTo = strLinkTo
            oStatic = oitem.Specific
            oStatic.Caption = strCaption


            Return oitem

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Agrega edittext al formulario
    ''' </summary>
    ''' <param name="oform">formularo</param>
    ''' <param name="strNombrectrl">Nombre del componente</param>
    ''' <param name="intLeft">Posicon en izquierda</param>
    ''' <param name="intTop">posicion arriba</param>
    ''' <param name="intFromPane">From Pane</param>
    ''' <param name="intTopane">To Pane</param>
    ''' <param name="strLinkTo">Asocionado al txt</param>
    ''' <remarks></remarks>
    Private Function AgregaEditText(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            If Not String.IsNullOrEmpty(strLinkTo) Then oitem.LinkTo = strLinkTo
            oEditText = oitem.Specific

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Agrega boton al formulario
    ''' </summary>
    ''' <param name="oform">formularo</param>
    ''' <param name="strNombrectrl">Nombre del componente</param>
    ''' <param name="intLeft">Posicon en izquierda</param>
    ''' <param name="intTop">posicion arriba</param>
    ''' <param name="intFromPane">From Pane</param>
    ''' <param name="intTopane">To Pane</param>
    ''' <param name="ButtonType">Tipo de boton</param>
    ''' <remarks></remarks>
    Private Function AgregaButton(ByRef oform As SAPbouiCOM.Form, _
                                    ByVal strNombrectrl As String, _
                                    ByVal intLeft As Integer, _
                                    ByVal intTop As Integer, _
                                    ByVal intFromPane As Integer, _
                                    ByVal intTopane As Integer, _
                                    ByVal strCaption As String, _
                                    ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
                                    ByVal CFL As Boolean) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane

            oButton = oitem.Specific
            oButton.Type = ButtonType
            oButton.Caption = strCaption

            If CFL Then oButton.ChooseFromListUID = "CFL_Veh"

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Agrega boton al formulario
    ''' </summary>
    ''' <param name="m_oForm">formularo</param>
    ''' <param name="strNombrectrl">Nombre del componente</param>
    ''' <param name="intLeft">Posicon en izquierda</param>
    ''' <param name="intTop">posicion arriba</param>
    ''' <param name="intHeight">Alto</param>
    ''' <param name="intWidth">Ancho</param>
    ''' <param name="intFromPane">From Pane</param>
    ''' <param name="intTopane">To Pane</param>
    ''' <remarks></remarks>
    Private Function AgregaMatriz(ByRef m_oForm As SAPbouiCOM.Form, ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intHeight As Integer, _
                                   ByVal intWidth As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oColumns As SAPbouiCOM.Columns
        Try

            oitem = m_oForm.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_MATRIX)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.Height = intHeight
            oitem.Width = intWidth + 5
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oMatrix = oitem.Specific
            oMatrix.SelectionMode = BoMatrixSelect.ms_Single
            oMatrix.Columns.Add("Col_num", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_uni", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_pla", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_mar", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_est", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_mod", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_cli", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_es", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oMatrix.Columns.Add("Col_ano", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oMatrix.Columns.Add("Col_vin", SAPbouiCOM.BoFormItemTypes.it_EDIT)

            oColumns = oMatrix.Columns
            With oColumns.Item("Col_num")
                .TitleObject.Caption = My.Resources.Resource.SignoNumeral
                .Editable = False
                
            End With
            With oColumns.Item("Col_uni")
                .TitleObject.Caption = My.Resources.Resource.CapNoUnidad
                .Width = 80
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_pla")
                .TitleObject.Caption = My.Resources.Resource.CapPlaca
                .Width = 50
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_vin")
                .TitleObject.Caption = My.Resources.Resource.CapVIN
                .Width = 70
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_mar")
                .TitleObject.Caption = My.Resources.Resource.CapMarca
                .Width = 50
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_est")
                .TitleObject.Caption = My.Resources.Resource.CapEstilo
                .Width = 50
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_mod")
                .TitleObject.Caption = My.Resources.Resource.CapModelo
                .Width = 50
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_cli")
                .TitleObject.Caption = My.Resources.Resource.CapCliente
                .Width = 70
                .Editable = False
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_es")
                .TitleObject.Caption = My.Resources.Resource.CapEstado
                .Width = 60
                .DisplayDesc = True
                .TitleObject.Sortable = True
            End With
            With oColumns.Item("Col_ano")
                .TitleObject.Caption = My.Resources.Resource.CapAño
                .Width = 40
                .Editable = False
                .TitleObject.Sortable = True
            End With

            Return oitem
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    ''' <summary>
    ''' Agrega el choosefromlist al formulario
    ''' </summary>
    ''' <param name="oform">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub AgregaChooseFromListVehiculos(ByVal m_oForm As Form)

        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition

        Try

            If Not m_oForm Is Nothing Then
                oCFLs = m_oForm.ChooseFromLists

                Dim oCFL As SAPbouiCOM.ChooseFromList
                Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
                oCFLCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

                ' Adding 2 CFL, one for the button and one for the edit text.
                oCFLCreationParams.MultiSelection = False
                oCFLCreationParams.ObjectType = "SCGD_VEH"
                oCFLCreationParams.UniqueID = "CFL_Veh"
                oCFL = oCFLs.Add(oCFLCreationParams)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Public Sub AsignaCFLColumn(ByVal m_oForm As Form, ByVal p_strMatriz As String, ByVal p_strColumn As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oitem = m_oForm.Items.Item(p_strMatriz)
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item(p_strColumn).ChooseFromListUID = p_strCFL
            oMatrix.Columns.Item(p_strColumn).ChooseFromListAlias = p_Alias
            '-----------------------------------------------
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

#End Region

    Private Sub InizializaTablaVehiculos(ByVal p_oForm As SAPbouiCOM.Form)
        Try

            'agregar datatable al form 
            g_dtVehiculosCampana = p_oForm.DataSources.DataTables.Add(g_strDTVehiculo)
            'g_dtVehiculosCampana.Columns.Add("num", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("uni", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("mar", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("es", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("mod", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("pla", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("cli", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("est", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("ano", BoFieldsType.ft_AlphaNumeric, 100)
            g_dtVehiculosCampana.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)

            'agregar la matriz
            g_mVehiculosCampana = New MatrizVehiculosCampana(g_strUIDmtxVehiculos, p_oForm, g_strDTVehiculo)
            g_mVehiculosCampana.CreaColumnas()
            g_mVehiculosCampana.LigaColumnas()
        Catch ex As Exception

        End Try
    End Sub

End Class

