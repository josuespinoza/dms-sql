Imports SAPbouiCOM
Imports System.Collections.Generic


Public Module ControladorRestablecerCantidadesPendientes
    Enum Aprobado
        Sí = 1
        No = 2
        FaltaAprobación = 3
        CambioOT = 4
    End Enum

    Enum Trasladado
        NoProcesado = 0
        No = 1
        Sí = 2
        PendienteTraslado = 3
        PendienteBodega = 4
    End Enum

    Enum TipoArticulo
        Repuesto = 1
        Suministro = 3
    End Enum

    Enum TipoTransaccion
        Requisición = 1
        Compra = 2
    End Enum

    ''' <summary>
    ''' Constructor del módulo
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        'Implementar el constructor aquí
    End Sub

    ''' <summary>
    ''' Manejador de eventos tipo ItemEvent
    ''' </summary>
    ''' <param name="FormUID">FormUID en formato texto</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable BubbleEvent para indicar si se debe continuar procesando o no el evento</param>
    ''' <remarks></remarks>
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If pVal.FormTypeEx = "SCGD_SRCP" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    If pVal.BeforeAction Then
                        'Sin implementar
                    Else
                        Select Case pVal.EventType
                            Case BoEventTypes.et_ITEM_PRESSED
                                ItemPressed(oFormulario, pVal, BubbleEvent)
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID">FormUID en formato texto</param>
    ''' <returns>Si se encuentra el formulario, se devuelve. De lo contrario retorna Nothing.</returns>
    ''' <remarks></remarks>
    Private Function ObtenerFormulario(ByVal FormUID As String) As SAPbouiCOM.Form
        Try
            Return DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Manejador de eventos ItemPressed
    ''' </summary>
    ''' <param name="oFormulario">Objeto Formulario de SAP</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable Booleana para indicar a SAP si se debe continuar manejando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressed(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ItemUID
                Case "btnCorre"
                    ProcesarDocumentos(oFormulario)
                Case "btnBusca"
                    CargarDocumentos(oFormulario)
                Case "chkCom", "chkReq", "chkAmb", "chkEsp", "chkAll"
                    ManejadorCheckBoxes(oFormulario, pVal)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Administra el estado de los CheckBox y los actualiza cada vez que se selecciona uno
    ''' </summary>
    ''' <param name="oFormulario">Objeto Formulario de SAP</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorCheckBoxes(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent)
        Dim oEditText As SAPbouiCOM.EditText
        Try
            oEditText = oFormulario.Items.Item("txtDocN").Specific

            'Activa o desactiva los checkbox de acuerdo al seleccionado
            'solamente uno puede estar seleccionado al mismo tiempo con excepción del Check para marcar todas las filas
            Select Case pVal.ItemUID
                Case "chkCom"
                    oFormulario.DataSources.UserDataSources.Item("Requisi").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Ambas").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Especif").ValueEx = "N"
                Case "chkReq"
                    oFormulario.DataSources.UserDataSources.Item("Compras").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Ambas").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Especif").ValueEx = "N"
                Case "chkAmb"
                    oFormulario.DataSources.UserDataSources.Item("Compras").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Requisi").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Especif").ValueEx = "N"
                Case "chkEsp"
                    oFormulario.DataSources.UserDataSources.Item("Compras").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Requisi").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Ambas").ValueEx = "N"
                    oFormulario.Items.Item("lblDocN").Visible = True
                    oEditText.Item.Visible = True
                Case "chkAll"
                    MarcarDesmarcarTodasLineas(oFormulario)
            End Select

            'Si el CheckBox no es "Específico" se limpia el número de documento
            If pVal.ItemUID <> "chkEsp" Then
                oEditText.Value = String.Empty
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Marca o desmarca todas las líneas de la matriz
    ''' </summary>
    ''' <param name="oFormulario">Objeto Formulario de SAP</param>
    ''' <remarks></remarks>
    Private Sub MarcarDesmarcarTodasLineas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim Check As String = String.Empty
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxIncon").Specific
            oDataTable = oFormulario.DataSources.DataTables.Item("DTISSUE")

            If Not oDataTable.IsEmpty Then
                Check = oFormulario.DataSources.UserDataSources.Item("All").ValueEx
                For i As Integer = 0 To oDataTable.Rows.Count - 1
                    oDataTable.SetValue("Check", i, Check)
                Next
                oMatrix.LoadFromDataSource()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca los documentos con inconsistencias y los carga en la matriz
    ''' </summary>
    ''' <param name="oFormulario">Objeto Formulario de SAP</param>
    ''' <remarks></remarks>
    Private Sub CargarDocumentos(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim Query As String = String.Empty
        Try
            oFormulario.Freeze(True)

            oDataTable = oFormulario.DataSources.DataTables.Item("DTISSUE")
            oMatrix = oFormulario.Items.Item("mtxIncon").Specific

            'Obtiene el query de acuerdo al CheckBox marcado (Compras, Requisiciones, Todas, Específico)
            Query = ObtenerQueryPorParametros(oFormulario)
            If Not String.IsNullOrEmpty(Query) Then
                oDataTable.ExecuteQuery(Query)
            End If

            oMatrix.LoadFromDataSource()

            'Cuando no se encontraron documentos con inconsistencias, se muestra un mensaje al usuario
            If oDataTable.IsEmpty() Then
                DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.SinInconsistencias)
            End If

            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Redimensiona las columnas de la matriz para que se adapten automáticamente al tamaño
    ''' del formulario y de la matriz aprovechando al máximo el espacio
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <remarks></remarks>
    Public Sub RedimensionarColumnas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxIncon").Specific
            oMatrix.AutoResizeColumns()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el query de acuerdo al CheckBox marcado (Compras, Requisiciones, Ambas o Específico)
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <returns>Query en formato texto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerQueryPorParametros(ByRef oFormulario As SAPbouiCOM.Form) As String
        Dim Query As String = String.Empty
        Dim Compras As String = String.Empty
        Dim Requisiciones As String = String.Empty
        Dim Ambas As String = String.Empty
        Dim Especifico As String = String.Empty
        Dim DocNum As String = String.Empty
        Dim EditText As SAPbouiCOM.EditText
        Try
            Compras = oFormulario.DataSources.UserDataSources.Item("Compras").ValueEx
            Requisiciones = oFormulario.DataSources.UserDataSources.Item("Requisi").ValueEx
            Ambas = oFormulario.DataSources.UserDataSources.Item("Ambas").ValueEx
            Especifico = oFormulario.DataSources.UserDataSources.Item("Especif").ValueEx

            'Busca un documento por el DocNum sin importar si es una requisición o una compra
            If Especifico = "Y" Then
                EditText = oFormulario.Items.Item("txtDocN").Specific
                DocNum = EditText.Value.Trim()

                If Not String.IsNullOrEmpty(DocNum) Then
                    Query = String.Format("{0} AND T0.DocNum = '{2}' UNION ALL {1} AND T0.DocNum = '{2}' ", DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisCompras"), DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisRequisiciones"), DocNum)
                Else
                    'Mostrar mensaje de error indicando que no se digitó un número de documento válido
                    Query = String.Empty
                End If
            End If

            'Query para compras y requisiciones
            If Ambas = "Y" Then
                Query = String.Format("{0} UNION ALL {1}", DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisCompras"), DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisRequisiciones"))
            End If

            'Query que solo consulta las requisiciones
            If Requisiciones = "Y" Then
                Query = DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisRequisiciones")
            End If

            'Query que solamente consulta las compras
            If Compras = "Y" Then
                Query = DMS_Connector.Queries.GetStrQueryFormat("strQueryInconsisCompras")
            End If

            Return Query
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Método encargado del manejo del botón corregir documentos
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <remarks></remarks>
    Private Sub ProcesarDocumentos(ByRef oFormulario As SAPbouiCOM.Form)
        Dim dcOfertasVentas As New Dictionary(Of Integer, RestablecerCantidades)
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            'Paso 1: Enviar los datos hacia el DataTable para que refresque la información
            oMatrix = oFormulario.Items.Item("mtxIncon").Specific
            oMatrix.FlushToDataSource()

            'Paso 2: Generar un listado agrupado por número de documento
            AgruparLineasPorNumeroDocumento(oFormulario, dcOfertasVentas)

            'Paso 3: Recorrer el listado y procesar las líneas con problemas
            CorregirDocumentos(oFormulario, dcOfertasVentas)

            'Paso 4: Mostrar los resultados en pantalla al usuario
            MostrarResultados(oFormulario, dcOfertasVentas)
            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza la columna "Resultados" de la matriz indicando si el proceso fue exitoso o si se produjo algún error
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <param name="dcOfertasVentas">Diccionario con la información de las líneas agrupada por número de oferta de ventas (DocEntry)</param>
    ''' <remarks></remarks>
    Private Sub MostrarResultados(ByRef oFormulario As SAPbouiCOM.Form, ByRef dcOfertasVentas As Dictionary(Of Integer, RestablecerCantidades))
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            oDataTable = oFormulario.DataSources.DataTables.Item("DTISSUE")

            For Each KeyValue As KeyValuePair(Of Integer, RestablecerCantidades) In dcOfertasVentas
                For Each LineaPorRestablecer As LineaRestablecerCantidades In KeyValue.Value.LineasDocumento
                    oDataTable.SetValue("Check", LineaPorRestablecer.DataTableLine, "N")
                    oDataTable.SetValue("Remarks", LineaPorRestablecer.DataTableLine, KeyValue.Value.Resultado)
                Next
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Corrige todos los documentos almacenados en el objeto diccionario
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <param name="dcOfertasVentas">Diccionario con la información de las líneas agrupada por número de oferta de ventas (DocEntry)</param>
    ''' <remarks></remarks>
    Private Sub CorregirDocumentos(ByRef oFormulario As SAPbouiCOM.Form, ByRef dcOfertasVentas As Dictionary(Of Integer, RestablecerCantidades))
        Dim OfertaVentas As SAPbobsCOM.Documents

        Try
            If dcOfertasVentas.Count > 0 Then
                OfertaVentas = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                For Each KeyValue As KeyValuePair(Of Integer, RestablecerCantidades) In dcOfertasVentas
                    If OfertaVentas.GetByKey(KeyValue.Key) Then
                        For Each LineaPorRestablecer As LineaRestablecerCantidades In KeyValue.Value.LineasDocumento
                            CorregirCantidadesLinea(OfertaVentas, LineaPorRestablecer)
                        Next
                        If OfertaVentas.Update() <> 0 Then
                            KeyValue.Value.Resultado = DMS_Connector.Company.CompanySBO.GetLastErrorDescription().Substring(0, 254)
                        Else
                            KeyValue.Value.Resultado = My.Resources.Resource.Exitoso
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cambia los valores de la línea del documento indicado, corrigiendo cualquier inconsistencia detectada
    ''' </summary>
    ''' <param name="OfertaVentas">Objeto que contiene la oferta de ventas que se está procesando</param>
    ''' <param name="LineaPorRestablecer">Objeto que contiene la información de la línea que se detectó que tiene problemas</param>
    ''' <remarks></remarks>
    Private Sub CorregirCantidadesLinea(ByRef OfertaVentas As SAPbobsCOM.Documents, ByRef LineaPorRestablecer As LineaRestablecerCantidades)
        Dim CantidadPendienteBodega As Double
        Dim CantidadEntrada As Double
        Dim CantidadRecibida As Double
        Dim CantidadSolicitada As Double

        Try
            OfertaVentas.Lines.SetCurrentLine(LineaPorRestablecer.LineNum)

            CantidadPendienteBodega = OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CPBo").Value
            CantidadEntrada = LineaPorRestablecer.CantidadEntrada
            CantidadRecibida = OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CRec").Value
            CantidadSolicitada = OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CSol").Value

            Select Case LineaPorRestablecer.TipoTransaccion
                Case TipoTransaccion.Requisición
                    'Calcula los nuevos valores
                    CantidadRecibida = CantidadRecibida + CantidadEntrada
                    CantidadPendienteBodega = CantidadPendienteBodega - CantidadEntrada

                    'Asigna los valores ya calculados a los campos
                    OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CRec").Value = CantidadRecibida
                    OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CPBo").Value = CantidadPendienteBodega

                    'Actualiza el estado del Traslado
                    If CantidadPendienteBodega = 0 Then
                        OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_Traslad").Value = Trasladado.Sí
                    Else
                        OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_Traslad").Value = Trasladado.PendienteBodega
                    End If
                Case TipoTransaccion.Compra
                    'Calcula los nuevos valores
                    CantidadRecibida = CantidadRecibida + CantidadEntrada
                    CantidadSolicitada = CantidadSolicitada - CantidadEntrada

                    'La cantidad solicitada no puede ser menor a cero
                    If Not CantidadSolicitada > 0 Then
                        CantidadSolicitada = 0
                    End If

                    'La cantidad recibida no puede ser mayor a la cantidad del pedido
                    If CantidadRecibida > OfertaVentas.Lines.Quantity Then
                        CantidadRecibida = OfertaVentas.Lines.Quantity
                    End If

                    'Asigna los valores ya calculados a los campos
                    OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CRec").Value = CantidadRecibida
                    OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CSol").Value = CantidadSolicitada
                    OfertaVentas.Lines.UserFields().Fields().Item("U_SCGD_CPen").Value = 0
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recorre el DataTable que contiene la información de las líneas y las agrupa por número de documento (DocEntry de la oferta de ventas)
    ''' y guarda las líneas en un listado dentro de esa agrupación
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <param name="dcOfertasVentas">Diccionario con la información de las líneas agrupada por número de oferta de ventas (DocEntry)</param>
    ''' <remarks></remarks>
    Private Sub AgruparLineasPorNumeroDocumento(ByRef oFormulario As SAPbouiCOM.Form, ByRef dcOfertasVentas As Dictionary(Of Integer, RestablecerCantidades))
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim DocEntry As Integer
        Dim LineNum As Integer
        Dim ItemCode As String
        Dim ItemName As String
        Dim CantidadEntrada As Double
        Dim LineaMarcada As String = String.Empty
        Dim TipoArticulo As Integer
        Dim TipoTransaccion As Integer

        Try
            oDataTable = oFormulario.DataSources.DataTables.Item("DTISSUE")

            For i As Integer = 0 To oDataTable.Rows.Count - 1
                LineaMarcada = oDataTable.GetValue("Check", i)
                If Not String.IsNullOrEmpty(LineaMarcada) AndAlso LineaMarcada = "Y" Then
                    DocEntry = Integer.Parse(oDataTable.GetValue("DocEntry", i))
                    LineNum = Integer.Parse(oDataTable.GetValue("LineNum", i))
                    ItemCode = oDataTable.GetValue("ItemCode", i)
                    ItemName = oDataTable.GetValue("Dscription", i)
                    CantidadEntrada = Double.Parse(oDataTable.GetValue("CantidadEntrada", i))
                    TipoArticulo = Integer.Parse(oDataTable.GetValue("ArtType", i))
                    TipoTransaccion = Integer.Parse(oDataTable.GetValue("TrnsType", i))

                    'Agrega la línea al Diccionario y las agrupa por documento
                    If DocEntry > 0 Then
                        If Not dcOfertasVentas.ContainsKey(DocEntry) Then
                            dcOfertasVentas.Add(DocEntry, New RestablecerCantidades())
                        End If
                        dcOfertasVentas.Item(DocEntry).LineasDocumento.Add(New LineaRestablecerCantidades(DocEntry, LineNum, ItemCode, ItemName, CantidadEntrada, TipoArticulo, TipoTransaccion, i))
                    End If
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


End Module


