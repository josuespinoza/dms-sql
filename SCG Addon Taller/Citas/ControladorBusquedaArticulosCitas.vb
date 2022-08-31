Imports SAPbouiCOM
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Linq

Public Module ControladorBusquedaArticulosCitas
    Private n As NumberFormatInfo

    Private QueryArticulos As String =
                   " select top(100) '' AS 'Chk', oi.ItemCode as Code, oi.ItemName as 'Dsc', cfnb.U_Rep as Whs, " +
                   " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as Stock, " +
                   " 1 as Qty, it.Price as Price, it.Currency as Curr, oi.U_SCGD_T_Fase as Phase, oi.U_SCGD_Duracion as Dura,oi.CodeBars AS 'BarCode', oi.U_SCGD_TipoArticulo AS 'Type', oi.""TreeType"" " +
                   " from OITM as oi with (nolock) " +
                   " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                   " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                   " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' "
    Private QueryArticulosEspecificos As String =
                    "  select top(100) '' AS 'Chk', oi.ItemCode as Code, oi.ItemName as 'Dsc', cfnb.U_Rep as Whs, " +
                    " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as Stock, " +
                    " 1 as Qty, it.Price as Price, it.Currency as Curr,  oi.U_SCGD_T_Fase as Phase, Art.U_Duracion as Dura,oi.CodeBars AS 'BarCode', oi.U_SCGD_TipoArticulo AS 'Type', oi.""TreeType"" " +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " inner join [@SCGD_ARTXESP] as Art with(nolock) on oi.ItemCode = art.U_ItemCode  " +
                    " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}'  {2}  "

    Private QueryServiciosExternos As String = " select top(100) '' AS 'Chk', oi.ItemCode as Code, oi.ItemName as 'Dsc', cfnb.U_Rep as Whs, " +
                   " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as Stock, " +
                   " 1 as Qty, it.Price as Price, it.Currency as Curr,  oi.U_SCGD_T_Fase as Phase, oi.U_SCGD_Duracion as Dura,oi.CodeBars AS 'BarCode', oi.U_SCGD_TipoArticulo AS 'Type', oi.""TreeType"" " +
                   " from OITM as oi with (nolock) " +
                   " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                   " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode  " +
                   " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' and oi.U_SCGD_TipoArticulo in(3,4,5)  "

    Private QueryEspecificosConfigurados As String = "  Select Count(U_ItemCode) as U_ItemCode from [@SCGD_ARTXESP] as art where U_TipoArt in (1,2) "

    Enum TiposArticulo
        Repuesto = 1
        Servicio = 2
        Suministro = 3
        ServicioExterno = 4
        Paquete = 5
        Otros = 6
        Accesorio = 7
        Vehiculo = 8
        Tramite = 9
        ArticuloCita = 10
        OtrosCostos = 11
        OtrosIngresos = 12
    End Enum

    Private MonedaLocal As String = String.Empty
    Private MonedaSistema As String = String.Empty

    Sub New()
        Try
            n = DIHelper.GetNumberFormatInfo(DMS_Connector.Company.CompanySBO)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores mínimos predeterminados para el formulario de búsqueda de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="FormUIDPadre">FormUID del formulario desde el cual se abrió el buscador, este datos es muy importante
    ''' ya que al momento de agregar líneas va a permitir saber a cual instancia del formulario
    ''' de citas se le deben agregar las líneas</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="CodigoCliente">Código del cliente</param>
    ''' <param name="CodigoInternoVehiculo">Código interno del vehículo</param>
    ''' <remarks></remarks>
    Public Sub CargarValoresPredeterminados(ByRef oFormulario As SAPbouiCOM.Form, ByVal FormUIDPadre As String, ByVal Sucursal As String, ByVal CodigoCliente As String, ByVal CodigoInternoVehiculo As String)
        Dim CodigoEstilo As String = String.Empty
        Dim CodigoModelo As String = String.Empty
        Try
            CargarConfiguracionesMatriz(oFormulario)
            oFormulario.DataSources.UserDataSources.Item("PadreUID").ValueEx = FormUIDPadre
            oFormulario.DataSources.UserDataSources.Item("Branch").ValueEx = Sucursal
            oFormulario.DataSources.UserDataSources.Item("Customer").ValueEx = CodigoCliente
            oFormulario.DataSources.UserDataSources.Item("Vehicle").ValueEx = CodigoInternoVehiculo
            oFormulario.DataSources.UserDataSources.Item("PreCli").ValueEx = ControladorCitas.ObtenerListaPrecios(Sucursal, CodigoCliente)
            ObtenerEstiloModeloVehiculo(CodigoInternoVehiculo, CodigoEstilo, CodigoModelo)
            oFormulario.DataSources.UserDataSources.Item("Style").ValueEx = CodigoEstilo
            oFormulario.DataSources.UserDataSources.Item("Model").ValueEx = CodigoModelo
            CargarArticulos(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el estilo y modelo del vehículo especificado
    ''' </summary>
    ''' <param name="DocEntryVehiculo">DocEntry o código interno del vehículo</param>
    ''' <param name="CodigoEstilo">Variable donde se va a guardar el código del estilo</param>
    ''' <param name="CodigoModelo">Variable donde se va a guardar el código del modelo</param>
    ''' <remarks></remarks>
    Private Sub ObtenerEstiloModeloVehiculo(ByVal DocEntryVehiculo As String, ByRef CodigoEstilo As String, ByRef CodigoModelo As String)
        Dim oRecordSet As SAPbobsCOM.Recordset
        Dim Query As String = " SELECT U_Cod_Esti,U_Cod_Mode  FROM ""@SCGD_VEHICULO"" WHERE DocEntry = {0} "
        Try
            If Not String.IsNullOrEmpty(DocEntryVehiculo) Then
                oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                Query = String.Format(Query, DocEntryVehiculo)
                oRecordSet.DoQuery(Query)
                If oRecordSet.RecordCount > 0 Then
                    CodigoEstilo = oRecordSet.Fields.Item(0).Value.ToString()
                    CodigoModelo = oRecordSet.Fields.Item(1).Value.ToString()
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la matriz con los artículos (A partir de los datos predeterminados o a través de los campos de búsqueda)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CargarArticulos(ByRef oFormulario As SAPbouiCOM.Form)
        Dim UsaConfiguracionEstiloModelo As String = String.Empty
        Dim FiltroEstiloModelo As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim Query As String = String.Empty
        Dim DocEntrySucursal As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim ListaPrecios As String = String.Empty
        Dim DocEntryVehiculo As String = String.Empty
        Dim Estilo As String = String.Empty
        Dim Modelo As String = String.Empty
        Dim CodigoArticulo As String = String.Empty
        Dim DescripcionArticulo As String = String.Empty
        Dim CodigoBarras As String = String.Empty
        
        Try
            oMatrix = oFormulario.Items.Item("mtxArt").Specific
            oMatrix.FlushToDataSource()
            oDataTable = oFormulario.DataSources.DataTables.Item("Items")

            Sucursal = oFormulario.DataSources.UserDataSources.Item("Branch").ValueEx
            ListaPrecios = oFormulario.DataSources.UserDataSources.Item("PreCli").ValueEx
            DocEntryVehiculo = oFormulario.DataSources.UserDataSources.Item("Vehicle").ValueEx
            Estilo = oFormulario.DataSources.UserDataSources.Item("Style").ValueEx
            Modelo = oFormulario.DataSources.UserDataSources.Item("Model").ValueEx

            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                DocEntrySucursal = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).DocEntry
            End If

            UsaConfiguracionEstiloModelo = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim()
            FiltroEstiloModelo = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()

            'Obtiene los filtros de búsqueda digitados por el usuario en pantalla
            CodigoArticulo = oFormulario.DataSources.UserDataSources.Item("Code").ValueEx
            DescripcionArticulo = oFormulario.DataSources.UserDataSources.Item("Desc").ValueEx
            CodigoBarras = oFormulario.DataSources.UserDataSources.Item("BarCode").ValueEx

            Query = ObtenerQueryArticulos(UsaConfiguracionEstiloModelo, ListaPrecios, DocEntrySucursal, Estilo, Modelo, FiltroEstiloModelo, CodigoArticulo, DescripcionArticulo, CodigoBarras)

            oDataTable.ExecuteQuery(Query)
            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el query que se debe utilizar para la búsqueda de artículos a partir de las configuraciones
    ''' y de los filtros utilizados
    ''' </summary>
    ''' <param name="UsaConfiguracionEstiloModelo">Variable que indica si se debe usar la configuración por estilo y modelo en la búsqueda de artículos</param>
    ''' <param name="ListaPrecios">Lista de precios que se debe obtener en la consulta</param>
    ''' <param name="DocEntrySucursal">DocEntry de la sucursal</param>
    ''' <param name="Estilo">Código del estilo</param>
    ''' <param name="Modelo">Código del modelo</param>
    ''' <param name="FiltroEstiloModelo">Variable que indica si se usa el filtro por estilo o por modelo (Son exclusivos)</param>
    ''' <param name="CodigoArticulo">Código del artículo</param>
    ''' <param name="DescripcionArticulo">Descripción del artículo</param>
    ''' <param name="CodigoBarras">Código de barras</param>
    ''' <returns>Query que se debe utilizar para obtener el listado de artículos</returns>
    ''' <remarks></remarks>
    Private Function ObtenerQueryArticulos(ByVal UsaConfiguracionEstiloModelo As String, ByVal ListaPrecios As String, ByVal DocEntrySucursal As String, ByVal Estilo As String, ByVal Modelo As String, ByVal FiltroEstiloModelo As String, ByVal CodigoArticulo As String, ByVal DescripcionArticulo As String, ByVal CodigoBarras As String) As String
        Dim Query As String = String.Empty
        Dim FiltroQuery As String = String.Empty
        Dim FiltrosBusqueda As String = String.Empty
        Dim FiltroArticulo As String = String.Empty
        Dim FiltroDescripcion As String = String.Empty
        Dim FiltroCodigoBarras As String = String.Empty
        Dim CantidadEspecificosConfigurados As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(CodigoArticulo) Then
                FiltrosBusqueda += String.Format(" and oi.ItemCode like '{0}%' ", CodigoArticulo)
            End If

            If Not String.IsNullOrEmpty(DescripcionArticulo) Then
                FiltrosBusqueda += String.Format(" and oi.ItemName like '{0}%' ", DescripcionArticulo)
            End If

            If Not String.IsNullOrEmpty(CodigoBarras) Then
                FiltrosBusqueda += String.Format(" and oi.CodeBars like '{0}%'", CodigoBarras)
            End If

            'Prioridad 1 Query estándar sin filtro por específico ni modelo
            Query = String.Format(QueryArticulos, ListaPrecios, DocEntrySucursal)
            Query += FiltrosBusqueda

            'Prioridad 2 Query con filtro por estilo y modelo
            'Si se usa la configuración por estilo modelo, se cambia el query estándar por el específico
            If UsaConfiguracionEstiloModelo = "Y" Then
                If FiltroEstiloModelo = "E" Then
                    If Not String.IsNullOrEmpty(Estilo) Then
                        FiltroQuery = String.Format(" and art.[U_CodEsti] = '{0}' ", Estilo)
                    End If
                Else
                    If Not String.IsNullOrEmpty(Modelo) Then
                        FiltroQuery = String.Format(" and art.[U_CodMod] = '{0}' ", Modelo)
                    End If
                End If

                If Not String.IsNullOrEmpty(FiltroQuery) Then
                    CantidadEspecificosConfigurados = DMS_Connector.Helpers.EjecutarConsulta(String.Format("{0} {1}", QueryEspecificosConfigurados, FiltroQuery))
                Else
                    CantidadEspecificosConfigurados = "0"
                End If

                If CantidadEspecificosConfigurados <> "0" Then
                    'Si existen específicos configurados se reemplaza el query estándar por el de específicos
                    Query = String.Format(QueryArticulosEspecificos, ListaPrecios, DocEntrySucursal, FiltroQuery)
                    Query += FiltrosBusqueda
                    Query += " UNION "
                    Query += String.Format(QueryServiciosExternos, ListaPrecios, DocEntrySucursal, FiltroQuery)
                    Query += FiltrosBusqueda
                End If
            End If

            Return Query
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function
    
    ''' <summary>
    ''' Carga las configuraciones propias de la matriz (Columnas fijas, campos editables, ...)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CargarConfiguracionesMatriz(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oLinkedButton As SAPbouiCOM.LinkedButton
        Try
            'Fija las primeras tres columnas
            oMatrix = oFormulario.Items.Item("mtxArt").Specific
            oMatrix.CommonSetting.FixedColumnsCount = 2
            oMatrix.Columns.Item("Dsc").Editable = True
            
            'Define que automáticamente se cambie el tamaño de las columnas
            'oMatrix.AutoResizeColumns()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos ItemEvent de SAP para el formulario de disponibilidad de empleados
    ''' </summary>
    ''' <param name="FormUID">ID del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable booleana de SAP para definir si se debe continuar con el proceso o no</param>
    ''' <remarks></remarks>
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If pVal.FormTypeEx = "SCGD_ISSC" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    Select Case pVal.EventType
                        Case BoEventTypes.et_ITEM_PRESSED
                            ItemPressed(oFormulario, pVal, BubbleEvent)
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <returns></returns>
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
                'Implementar manejo del BeforeAction aquí
            Else
                Select Case pVal.ItemUID
                    Case "btnSrch"
                        GuardarSeleccionados(oFormulario, pVal, BubbleEvent)
                        BuscarArticulos(oFormulario)
                        MarcarSeleccionados(oFormulario, pVal, BubbleEvent)
                    Case "btnAdd"
                        GuardarSeleccionados(oFormulario, pVal, BubbleEvent)
                        AgregarArticulosMatrizCita(oFormulario)
                    Case "btnCanc"
                        oFormulario.Close()
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de volver a marcar los artículos seleccionados en la matriz
    ''' cuando se cambian los filtros o se utiliza la búsqueda.
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el manejo del evento o no</param>
    ''' <remarks>Un ejemplo sería, cuando se marca un artículo, luego se aplica un filtro
    ''' y el artículo marcado desaparece visualmente, luego se quita el filtro y vuelve a aparecer
    ''' debido a que ya fue seleccionado, se debe marcar y evitar que se pierdan los datos seleccionados</remarks>
    Private Sub MarcarSeleccionados(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oDataTableArticulos As DataTable
        Dim oDataTableSeleccionados As DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxArt").Specific
            oDataTableArticulos = oFormulario.DataSources.DataTables.Item("Items")
            oDataTableSeleccionados = oFormulario.DataSources.DataTables.Item("Selected")

            For i As Integer = 0 To oDataTableSeleccionados.Rows.Count - 1
                For j As Integer = 0 To oDataTableArticulos.Rows.Count - 1
                    If oDataTableSeleccionados.GetValue("Code", i) = oDataTableArticulos.GetValue("Code", j) Then
                        oDataTableArticulos.SetValue("Chk", j, oDataTableSeleccionados.GetValue("Chk", i))
                        Exit For
                    End If
                Next
            Next

            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Guarda la lista de artículos marcados antes de aplicar un filtro de búsqueda o de hacer clic en el botón agregar
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub GuardarSeleccionados(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oDataTableArticulos As DataTable
        Dim oDataTableSeleccionados As DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim NumeroLineaExistente As Integer
        Dim LineaMarcada As String = String.Empty
        Dim NumeroUltimaLinea As Integer
        Try
            oMatrix = oFormulario.Items.Item("mtxArt").Specific
            oMatrix.FlushToDataSource()
            oDataTableArticulos = oFormulario.DataSources.DataTables.Item("Items")
            oDataTableSeleccionados = oFormulario.DataSources.DataTables.Item("Selected")

            For i As Integer = 0 To oDataTableArticulos.Rows.Count - 1
                LineaMarcada = oDataTableArticulos.GetValue("Chk", i)
                NumeroLineaExistente = -1

                For j As Integer = 0 To oDataTableSeleccionados.Rows.Count - 1
                    If oDataTableSeleccionados.GetValue("Code", j) = oDataTableArticulos.GetValue("Code", i) Then
                        NumeroLineaExistente = j
                        Exit For
                    End If
                Next

                If NumeroLineaExistente >= 0 Then
                    If LineaMarcada = "Y" Then
                        'Ya existe la línea, se actualiza el valor seleccionado
                        oDataTableSeleccionados.SetValue("Chk", NumeroLineaExistente, oDataTableArticulos.GetValue("Chk", i))
                        oDataTableSeleccionados.SetValue("Code", NumeroLineaExistente, oDataTableArticulos.GetValue("Code", i))
                        oDataTableSeleccionados.SetValue("Dsc", NumeroLineaExistente, oDataTableArticulos.GetValue("Dsc", i))
                        oDataTableSeleccionados.SetValue("Whs", NumeroLineaExistente, oDataTableArticulos.GetValue("Whs", i))
                        oDataTableSeleccionados.SetValue("Stock", NumeroLineaExistente, oDataTableArticulos.GetValue("Stock", i))
                        oDataTableSeleccionados.SetValue("Qty", NumeroLineaExistente, oDataTableArticulos.GetValue("Qty", i))
                        oDataTableSeleccionados.SetValue("Price", NumeroLineaExistente, oDataTableArticulos.GetValue("Price", i))
                        oDataTableSeleccionados.SetValue("Curr", NumeroLineaExistente, oDataTableArticulos.GetValue("Curr", i))
                        oDataTableSeleccionados.SetValue("Phase", NumeroLineaExistente, oDataTableArticulos.GetValue("Phase", i))
                        oDataTableSeleccionados.SetValue("Dura", NumeroLineaExistente, oDataTableArticulos.GetValue("Dura", i))
                        oDataTableSeleccionados.SetValue("BarCode", NumeroLineaExistente, oDataTableArticulos.GetValue("BarCode", i))
                        oDataTableSeleccionados.SetValue("Type", NumeroLineaExistente, oDataTableArticulos.GetValue("Type", i))
                        oDataTableSeleccionados.SetValue("TreeType", NumeroLineaExistente, oDataTableArticulos.GetValue("TreeType", i))
                    Else
                        'Se desmarcó la línea, se elimina la línea de la tabla de seleccionados
                        oDataTableSeleccionados.Rows.Remove(NumeroLineaExistente)
                    End If
                Else
                    If LineaMarcada = "Y" Then
                        'Es un artículo nuevo, se agrega una nueva línea
                        oDataTableSeleccionados.Rows.Add()

                        NumeroUltimaLinea = oDataTableSeleccionados.Rows.Count - 1

                        oDataTableSeleccionados.SetValue("Chk", NumeroUltimaLinea, oDataTableArticulos.GetValue("Chk", i))
                        oDataTableSeleccionados.SetValue("Code", NumeroUltimaLinea, oDataTableArticulos.GetValue("Code", i))
                        oDataTableSeleccionados.SetValue("Dsc", NumeroUltimaLinea, oDataTableArticulos.GetValue("Dsc", i))
                        oDataTableSeleccionados.SetValue("Whs", NumeroUltimaLinea, oDataTableArticulos.GetValue("Whs", i))
                        oDataTableSeleccionados.SetValue("Stock", NumeroUltimaLinea, oDataTableArticulos.GetValue("Stock", i))
                        oDataTableSeleccionados.SetValue("Qty", NumeroUltimaLinea, oDataTableArticulos.GetValue("Qty", i))
                        oDataTableSeleccionados.SetValue("Price", NumeroUltimaLinea, oDataTableArticulos.GetValue("Price", i))
                        oDataTableSeleccionados.SetValue("Curr", NumeroUltimaLinea, oDataTableArticulos.GetValue("Curr", i))
                        oDataTableSeleccionados.SetValue("Phase", NumeroUltimaLinea, oDataTableArticulos.GetValue("Phase", i))
                        oDataTableSeleccionados.SetValue("Dura", NumeroUltimaLinea, oDataTableArticulos.GetValue("Dura", i))
                        oDataTableSeleccionados.SetValue("BarCode", NumeroUltimaLinea, oDataTableArticulos.GetValue("BarCode", i))
                        oDataTableSeleccionados.SetValue("Type", NumeroUltimaLinea, oDataTableArticulos.GetValue("Type", i))
                        oDataTableSeleccionados.SetValue("TreeType", NumeroUltimaLinea, oDataTableArticulos.GetValue("TreeType", i))
                    End If
                End If
            Next

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca los artículos de acuerdo a los filtros
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub BuscarArticulos(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            CargarArticulos(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agregar los artículos a la matriz de artículos del formulario padre 
    ''' desde el cual se abrió este formulario (Desde la instancia del formulario de citas)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario de búsqueda</param>
    ''' <remarks></remarks>
    Private Sub AgregarArticulosMatrizCita(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oFormularioCita As SAPbouiCOM.Form
        Dim FormUIDPadre As String = String.Empty
        Dim oDataTableSeleccionados As SAPbouiCOM.DataTable
        Dim oDataTableCita As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim UltimaLinea As Integer
        Dim Sucursal As String = String.Empty
        Dim TipoArticulo As String = String.Empty
        Dim Cantidad As Double = 0
        Dim Precio As Double = 0
        Dim TipoPaquete As String = String.Empty
        Dim UsaPrecioArticuloPadre As String
        Dim Moneda As String = String.Empty
        Dim strImpuesto As String = String.Empty
        Dim strItemCode As String = String.Empty
        Dim strCardCode As String = String.Empty
      
        Try
            oDataTableSeleccionados = oFormulario.DataSources.DataTables.Item("Selected")
            UsaPrecioArticuloPadre = DMS_Connector.Helpers.EjecutarConsulta("Select TreePricOn from OADM")
            FormUIDPadre = oFormulario.DataSources.UserDataSources.Item("PadreUID").ValueEx
            oFormularioCita = ObtenerFormulario(FormUIDPadre)
            If oFormularioCita IsNot Nothing Then
                Sucursal = oFormulario.DataSources.UserDataSources.Item("Branch").ValueEx
                oDataTableCita = oFormularioCita.DataSources.DataTables.Item("listServicios")
                strCardCode = oFormularioCita.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0)

                oMatrix = oFormularioCita.Items.Item("mtxArtic").Specific
                oMatrix.FlushToDataSource()

                'Agregar los artículos a la matriz del formulario citas
                For i As Integer = 0 To oDataTableSeleccionados.Rows.Count - 1
                    If (oDataTableCita.Rows.Count = 1 AndAlso String.IsNullOrEmpty(oDataTableCita.GetValue("codigo", 0))) Or (oDataTableCita.IsEmpty() AndAlso oDataTableCita.Rows.Count = 1) Then
                        UltimaLinea = 0
                        oDataTableCita.Rows.Add()
                    Else
                        UltimaLinea = oDataTableCita.Rows.Count - 1
                        oDataTableCita.Rows.Add()
                        If Not String.IsNullOrEmpty(oDataTableCita.GetValue("codigo", UltimaLinea)) Then
                            UltimaLinea = oDataTableCita.Rows.Count - 1
                        End If
                    End If

                    oDataTableCita.SetValue("codigo", UltimaLinea, oDataTableSeleccionados.GetValue("Code", i))
                    strItemCode = oDataTableSeleccionados.GetValue("Code", i)
                    oDataTableCita.SetValue("descripcion", UltimaLinea, oDataTableSeleccionados.GetValue("Dsc", i))
                    oDataTableCita.SetValue("cantidad", UltimaLinea, oDataTableSeleccionados.GetValue("Qty", i))
                    Moneda = oDataTableSeleccionados.GetValue("Curr", i)
                    If String.IsNullOrEmpty(Moneda) Then
                        If String.IsNullOrEmpty(MonedaLocal) Then
                            Moneda = ObtenerMonedaLocal()
                        Else
                            Moneda = MonedaLocal
                        End If
                    End If
                    oDataTableCita.SetValue("moneda", UltimaLinea, Moneda)
                    oDataTableCita.SetValue("tipo", UltimaLinea, oDataTableSeleccionados.GetValue("Type", i))
                    oDataTableCita.SetValue("duracion", UltimaLinea, oDataTableSeleccionados.GetValue("Dura", i))
                    TipoArticulo = oDataTableSeleccionados.GetValue("Type", i)
                    strImpuesto = String.Empty
                    If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                        If Not String.IsNullOrEmpty(strCardCode) And Not String.IsNullOrEmpty(strItemCode) Then
                            strImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(oFormularioCita, strCardCode, strItemCode)
                            If Not String.IsNullOrEmpty(strImpuesto) Then
                                oDataTableCita.SetValue("impuesto", UltimaLinea, strImpuesto)
                            End If
                        End If
                        If String.IsNullOrEmpty(strImpuesto) Then
                            oDataTableCita.SetValue("impuesto", UltimaLinea, ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                        End If
                    Else
                        oDataTableCita.SetValue("impuesto", UltimaLinea, ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                    End If
                    oDataTableCita.SetValue("hijo", UltimaLinea, "N")
                    oDataTableCita.SetValue("padre", UltimaLinea, String.Empty)
                    TipoPaquete = oDataTableSeleccionados.GetValue("TreeType", i)
                    oDataTableCita.SetValue("paquete", UltimaLinea, TipoPaquete)
                    If TipoArticulo = TiposArticulo.Paquete AndAlso (TipoPaquete = "S" Or TipoPaquete = "T") Then
                        'Solamente se muestra el precio del artículo padre si esta habilitada la configuración
                        'o si la lista de materiales es de tipo modelo
                        If UsaPrecioArticuloPadre = "Y" Or TipoPaquete = "T" Then
                            oDataTableCita.SetValue("precio", UltimaLinea, oDataTableSeleccionados.GetValue("Price", i))
                        Else
                            oDataTableCita.SetValue("precio", UltimaLinea, 0)
                        End If
                        AgregarLineasHijas(oFormulario, oDataTableCita, oDataTableSeleccionados.GetValue("Code", i), TipoPaquete, Sucursal, UsaPrecioArticuloPadre)
                    Else
                        oDataTableCita.SetValue("precio", UltimaLinea, oDataTableSeleccionados.GetValue("Price", i))
                    End If

                    oDataTableCita.SetValue("barras", UltimaLinea, oDataTableSeleccionados.GetValue("BarCode", i))
                Next

                oMatrix.LoadFromDataSource()
                ControladorCitas.ConvertirMontosDesdeBusqueda(oFormularioCita)
                If oFormularioCita.Mode = BoFormMode.fm_OK_MODE Then
                    oFormularioCita.Mode = BoFormMode.fm_UPDATE_MODE
                End If
            End If
            oFormulario.Close()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            ControladorCitas.ActualizarFormatoTabla(oMatrix, oDataTableCita)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la moneda local
    ''' </summary>
    ''' <returns>Moneda local en formato texto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerMonedaLocal() As String
        Try
            DMS_Connector.Helpers.GetCurrencies(MonedaLocal, MonedaSistema)
            Return MonedaLocal
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Agrega las líneas hijas de la lista de materiales al datatable
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="oDataTable">DataTable de artículos</param>
    ''' <param name="CodigoArticuloPadre">Código del artículo padre (Principal de la lista de materiales)</param>
    ''' <param name="TipoPaquete">Tipo de paquete</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="UsaPrecioArticuloPadre">Configuración de SAP que indica si se muestra el precio del artículo padre o de los artículos hijos</param>
    ''' <remarks></remarks>
    Private Sub AgregarLineasHijas(ByRef oFormulario As SAPbouiCOM.Form, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal CodigoArticuloPadre As String, ByVal TipoPaquete As String, ByVal Sucursal As String, ByVal UsaPrecioArticuloPadre As String)
        Dim UltimaLinea As Integer
        Dim TipoArticulo As String
        Dim ListaMateriales As SAPbobsCOM.ProductTrees
        Dim MaestroArticulo As SAPbobsCOM.Items
        Dim oItemPriceParams As SAPbobsCOM.ItemPriceParams
        Dim oItemPriceReturnParams As SAPbobsCOM.ItemPriceReturnParams
        Dim ListaPrecios As String = String.Empty
        Dim CodigoCliente As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(CodigoArticuloPadre) Then
                ListaMateriales = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees)
                MaestroArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                oItemPriceParams = DMS_Connector.Company.CompanySBO.GetCompanyService.GetDataInterface(SAPbobsCOM.CompanyServiceDataInterfaces.csdiItemPriceParams)


                If ListaMateriales.GetByKey(CodigoArticuloPadre) Then
                    'Recorre toda la lista de materiales y agrega los artículos al DataTable
                    For i As Integer = 0 To ListaMateriales.Items.Count - 1
                        UltimaLinea = oDataTable.Rows.Count - 1
                        oDataTable.Rows.Add()
                        If Not String.IsNullOrEmpty(oDataTable.GetValue("codigo", UltimaLinea)) Then
                            UltimaLinea = oDataTable.Rows.Count - 1
                        End If

                        ListaMateriales.Items.SetCurrentLine(i)

                        If MaestroArticulo.GetByKey(ListaMateriales.Items.ItemCode) Then
                            oDataTable.SetValue("codigo", UltimaLinea, ListaMateriales.Items.ItemCode)
                            oDataTable.SetValue("descripcion", UltimaLinea, MaestroArticulo.ItemName)
                            oDataTable.SetValue("cantidad", UltimaLinea, ListaMateriales.Items.Quantity)
                            If UsaPrecioArticuloPadre = "Y" AndAlso TipoPaquete <> "T" Then
                                oDataTable.SetValue("moneda", UltimaLinea, ListaMateriales.Items.Currency)
                                oDataTable.SetValue("precio", UltimaLinea, 0)
                            Else
                                CodigoCliente = oFormulario.DataSources.UserDataSources.Item("Customer").ValueEx
                                ListaPrecios = oFormulario.DataSources.UserDataSources.Item("PreCli").ValueEx
                                oItemPriceParams.ItemCode = MaestroArticulo.ItemCode
                                oItemPriceParams.PriceList = ListaPrecios
                                oItemPriceReturnParams = DMS_Connector.Company.CompanySBO.GetCompanyService.GetDataInterface(SAPbobsCOM.CompanyServiceDataInterfaces.csdiItemPriceReturnParams)
                                oItemPriceReturnParams = DMS_Connector.Company.CompanySBO.GetCompanyService().GetItemPrice(oItemPriceParams)
                                oDataTable.SetValue("moneda", UltimaLinea, oItemPriceReturnParams.Currency)
                                oDataTable.SetValue("precio", UltimaLinea, oItemPriceReturnParams.Price)
                            End If
                            TipoArticulo = MaestroArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value
                            oDataTable.SetValue("tipo", UltimaLinea, TipoArticulo) 'U_SCGD_TipoArticulo
                            oDataTable.SetValue("duracion", UltimaLinea, MaestroArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value) 'U_SCGD_Duracion
                            oDataTable.SetValue("impuesto", UltimaLinea, ControladorCitas.ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                            oDataTable.SetValue("hijo", UltimaLinea, "Y")
                            oDataTable.SetValue("padre", UltimaLinea, CodigoArticuloPadre)

                            If MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                oDataTable.SetValue("paquete", UltimaLinea, "S")
                            End If

                            If MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree Then
                                oDataTable.SetValue("paquete", UltimaLinea, "T")
                            End If

                            If TipoArticulo = TiposArticulo.Paquete AndAlso (MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Or MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree) Then
                                AgregarLineasHijas(oFormulario, oDataTable, ListaMateriales.Items.ItemCode, oDataTable.GetValue("paquete", UltimaLinea), Sucursal, UsaPrecioArticuloPadre)
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
