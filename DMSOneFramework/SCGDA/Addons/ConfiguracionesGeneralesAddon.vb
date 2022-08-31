Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.BLSBO
Imports System.Data.SqlClient

Namespace SCGDataAccess

    Public Class ConfiguracionesGeneralesAddon

#Region "Declaraciones"

        Private m_dtsSeries As SeriesDataset.Series_CVDataTable
        Private m_dtsCuentasAdicionales As CuentasAdicionalesDataset.CuentasAdicionales_CVDataTable
        Private m_dtsGenerales As ConfiguracionesGeneralesDataset.Generales_CVDataTable
        Private m_dtsCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVDataTable
        Private m_dtsImpuestos As ImpuestosDataset.Impuesto_CVDataTable
        Private m_dtsGastosAdicionales As ItemsGastosDataset.GatosVentas_CVDataTable
        Private m_dtsLineasFactura As ItemsVentasDataset.ItemVentas_CVDataTable

        Private m_dtaSeries As SeriesDatasetTableAdapters.Series_CVTableAdapter
        Private m_dtaCuentasAdicionales As CuentasAdicionalesDatasetTableAdapters.CuentasAdicionales_CVTableAdapter
        Private m_dtaGenerales As ConfiguracionesGeneralesDatasetTableAdapters.Generales_CVTableAdapter
        Private m_dtaCuentasInventario As CuentasInventarioDatasetTableAdapters.CuentasInventario_CVTableAdapter
        Private m_dtaImpuestos As ImpuestosDatasetTableAdapters.Impuesto_CVTableAdapter
        Private m_dtaGastosAdicionales As ItemsGastosDatasetTableAdapters.GatosVentas_CVTableAdapter
        Private m_dtaLineasFactura As ItemsVentasDatasetTableAdapters.ItemVentas_CVTableAdapter

        Private m_drwGenerales As ConfiguracionesGeneralesDataset.Generales_CVRow

        Private m_cnConeccion As SqlClient.SqlConnection

        Private m_strTipoInventario As String

#End Region

#Region "Eventos Clase"

        ''' <summary>
        ''' Constructor para todos los valores de plan de ventas y las configuraciones generales
        ''' </summary>
        ''' <param name="Tipo">Código del Tipo del vehículo</param>
        ''' <param name="Coneccion">Objeto SqlConnection Inicializado</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Tipo As String, ByVal Coneccion As SqlClient.SqlConnection, ByRef blnEvento As Boolean)

            Try

                m_strTipoInventario = Tipo
                m_cnConeccion = Coneccion

                If m_cnConeccion.State = ConnectionState.Open Then
                    m_cnConeccion.Close()
                End If
                m_cnConeccion.Open()
                Call InicializarGenerales()
                Call InicializarCuentasAdicionales()
                Call InicializarCuentasInventario()
                Call InicializarGastosAdicionales()
                Call InicializarImpuestos()
                Call InicializarLineasFactura()
                Call InicializarSeries()

                'blnEvento = True

                m_cnConeccion.Close()

            Catch ex As Exception

                blnEvento = False
                Throw

            End Try

        End Sub

        ''' <summary>
        ''' Constructor para solo las configuraciones generales
        ''' </summary>
        ''' <param name="Coneccion">Objeto SqlConnection Inicializado</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Coneccion As SqlClient.SqlConnection)

            m_strTipoInventario = Nothing
            m_cnConeccion = Coneccion
            If m_cnConeccion.State <> ConnectionState.Open Then
                m_cnConeccion.Open()
            End If

            Call InicializarGenerales()
            Call InicializarCuentasInventario()

            m_cnConeccion.Close()

        End Sub

        Protected Overrides Sub Finalize()

            MyBase.Finalize()

            m_dtsSeries = Nothing
            m_dtsCuentasAdicionales = Nothing
            m_dtsGenerales = Nothing
            m_dtsCuentasInventario = Nothing
            m_dtsImpuestos = Nothing
            m_dtsGastosAdicionales = Nothing
            m_dtsLineasFactura = Nothing
            m_dtaSeries = Nothing
            m_dtaCuentasAdicionales = Nothing
            m_dtaGenerales = Nothing
            m_dtaCuentasInventario = Nothing
            m_dtaImpuestos = Nothing
            m_dtaGastosAdicionales = Nothing
            m_dtaLineasFactura = Nothing
            m_drwGenerales = Nothing
            If m_cnConeccion.State = ConnectionState.Open Then
                'Corrección temporal para solucionar el problema al cerrar sap o cambiar la compañía
                Try
                    m_cnConeccion.Close()
                Catch ex As System.InvalidOperationException

                End Try

            End If
            m_cnConeccion = Nothing

        End Sub

#End Region

#Region "Enumaraciones"

        Public Enum scgTipoCuenta
            scgCuentaStock = 1
            scgCuentaTransito = 2
            scgCuentaCosto = 3
            scgCuentaIngreso = 4
            scgAlmacenSucursal = 5
            scgAlmacenTramites = 6
            scgAlmacenLogistica = 7
            scgCuentaDevolucion = 8
        End Enum

        Public Enum scgItemsFactura
            PrecioVehículo = 1
            PrecioAccesorios = 2
            gastosIncripcion = 3
            GastosPrenda = 4
        End Enum

        Public Enum scgTipoSeries

            FacturaVentas = 1
            NotasCreditoUsados = 2
            NotasCreditoDescuentos = 3
            DocumentosDeuda = 4
            FacturaProveedor = 5
            NotasCreditoOtros = 6
            DocumentosDeudaOtros = 7
            PrimaVenta = 8
            NotasCreditoReversion = 9
            FacturaAccesorios = 10
            FacturaExentaDeudoresVehiculoUsado = 11
            FacturaExentaConsignados = 12
            FacturaProveedoresDocumentoReciboUsadoSociedades = 13
            FacturaProveedoresDocumentoReciboUsadoPrivado = 14
            TramitesFacturables = 15
            NotaCreditoReciboUsadoSociedades = 16
            NotaCreditoReciboUsadoPrivado = 17
            NotaCreditoReversionTramites = 18
            NotaCreditoReversionAccesorios = 19
            FacturaComisionConsignados = 20
            NotaCreditoComisionConsignados = 21
            NotaDebitoClienteReversionNCUsados = 22
            FacturaGastos = 23
            NotaCreditoReversionGastos = 24
            NotaDebitoReversionNCDescuento = 25
            NotaCreditoReversionFacturaDeudaUsado = 26
        End Enum

        Public Enum scgMoneda

            MonedaLocal = 1
            'MonedaSistema = 2
            MonedaExtranjera = 2

        End Enum

        Public Enum scgTipoDocumentosCV

            FacturaVentas = 1
            FacturaDeudaUsado = 2
            NotaDebito = 3
            NotasCreditoDescuento = 4
            NotasCreditoUsados = 5
            AsientoAjusteCosto = 6
            NotaDebitoDeudaUsado = 7
            PrimaVenta = 8
            AsientoSalidaAccesorios = 9
            NotaCreditoDesglosedeCobro = 10
            FacturaAccesorios = 11
            FacturaGastosAdicionales = 12
            AsientoFinancExterno = 13
            AsientoTramites = 14
            AsientoBonos = 15
            AsientoOtrosCostos = 16
            AsientoComisiones = 17
            FacturaExentaVehiculoUsado = 18
            FacturaProveedorVehiculoUsado = 19
            AsientoPrimerCuotaSeguro = 20
            AsientoComisionConsignado = 21
            FacturaComisionConsignado = 22
        End Enum


#End Region

#Region "Propiedades"

        Public ReadOnly Property Serie(ByVal TipoSerie As scgTipoSeries) As Integer

            Get

                Dim intSerie As Integer
                Dim drwSerie As SeriesDataset.Series_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwSerie = m_dtsSeries.FindByU_Cod_Item(TipoSerie)
                    If drwSerie IsNot Nothing Then
                        If Not drwSerie.IsU_SerieNull Then
                            intSerie = drwSerie.U_Serie
                        Else
                            intSerie = -1
                        End If
                    Else
                        intSerie = -1
                    End If
                Else
                    Throw New ApplicationException("No se ha definido un tipo")
                End If
                Return intSerie

            End Get

        End Property

        Public ReadOnly Property SerieExenta(ByVal TipoSerie As scgTipoSeries) As Integer

            Get

                Dim intSerie As Integer
                Dim drwSerie As SeriesDataset.Series_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwSerie = m_dtsSeries.FindByU_Cod_Item(TipoSerie)
                    If drwSerie IsNot Nothing Then
                        If Not drwSerie.IsU_SerieExNull Then
                            intSerie = drwSerie.U_SerieEx
                        Else
                            intSerie = -1
                        End If
                    Else
                        intSerie = -1
                    End If
                Else
                    Throw New ApplicationException("No se ha definido un tipo")
                End If
                Return intSerie

            End Get

        End Property

        Public ReadOnly Property Impuesto(ByVal TipoDocumento As scgTipoSeries) As String

            Get

                Dim strImpuesto As String = ""
                Dim drwImpuesto As ImpuestosDataset.Impuesto_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwImpuesto = m_dtsImpuestos.FindByU_Cod_Item(TipoDocumento)
                    If drwImpuesto IsNot Nothing Then
                        If Not drwImpuesto.IsU_Cod_ImpNull Then
                            strImpuesto = drwImpuesto.U_Cod_Imp
                        Else
                            strImpuesto = ""
                        End If
                    Else
                        strImpuesto = ""
                    End If
                Else
                    Throw New ApplicationException("No se ha definido un tipo")
                End If

                Return strImpuesto

            End Get

        End Property

        Public ReadOnly Property CuentasAdicionales(ByVal TipoDocumento As scgTipoSeries) As String

            Get

                Dim strCuentasAdicionales As String = ""
                Dim drwCuentasAdicionales As CuentasAdicionalesDataset.CuentasAdicionales_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwCuentasAdicionales = m_dtsCuentasAdicionales.FindByU_Cod_Item(TipoDocumento)
                    If drwCuentasAdicionales IsNot Nothing Then
                        If Not drwCuentasAdicionales.IsU_CuentaNull Then
                            strCuentasAdicionales = drwCuentasAdicionales.U_Cuenta
                        Else
                            strCuentasAdicionales = ""
                        End If
                    Else
                        strCuentasAdicionales = ""
                    End If
                Else
                    Throw New ApplicationException("No se ha definido un tipo")
                End If
                Return strCuentasAdicionales

            End Get

        End Property

        Public ReadOnly Property CuentaStock(ByVal Tipo As String) As String

            Get

                Dim strCuentasInventario As String = ""
                Dim drwCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVRow
                If Not String.IsNullOrEmpty(Tipo) Then
                    drwCuentasInventario = m_dtsCuentasInventario.FindByU_Tipo(Tipo)
                    If drwCuentasInventario IsNot Nothing Then
                        If Not drwCuentasInventario.IsU_StockNull Then
                            strCuentasInventario = drwCuentasInventario.U_Stock
                            'Else
                            '    Throw New ApplicationException("Cuenta de Inventario No Definida")
                        End If
                        '    Else
                        '        Throw New ApplicationException("Cuenta de Inventario No Definida")
                        '    End If
                        'Else
                        '    Throw New ApplicationException("Cuenta de Inventario No Definida")
                    End If
                End If
                Return strCuentasInventario

            End Get

        End Property

        Public ReadOnly Property CuentaInventarioTransito(ByVal Tipo As String) As String

            Get

                Dim strCuentasInventario As String = ""
                Dim drwCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVRow
                'If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                drwCuentasInventario = m_dtsCuentasInventario.FindByU_Tipo(Tipo)
                If drwCuentasInventario IsNot Nothing Then
                    If Not drwCuentasInventario.IsU_TransitoNull Then
                        strCuentasInventario = drwCuentasInventario.U_Transito
                        '    Else
                        '        Throw New ApplicationException("Cuenta de Tránsito No Definida")
                        '    End If
                        'Else
                        '    Throw New ApplicationException("Cuenta de Tránsito No Definida")
                    End If
                    'Else
                    'Throw New ApplicationException("Cuenta de Tránsito No Definida")
                End If
                Return strCuentasInventario

            End Get

        End Property

        Public ReadOnly Property CuentaCosto(ByVal Tipo As String) As String

            Get

                Dim strCuentasInventario As String = ""
                Dim drwCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVRow

                If Not String.IsNullOrEmpty(Tipo) Then
                    drwCuentasInventario = m_dtsCuentasInventario.FindByU_Tipo(Tipo)

                    If drwCuentasInventario IsNot Nothing Then
                        If Not drwCuentasInventario.IsU_CostoNull Then
                            strCuentasInventario = drwCuentasInventario.U_Costo
                            '    Else
                            '        Throw New ApplicationException("Cuenta de Costo No Definida")
                            '    End If
                            'Else
                            '    Throw New ApplicationException("Cuenta de Costo No Definida")
                        End If
                    End If
                    'Else
                    '    Throw New ApplicationException("Cuenta de Costo No Definida")
                End If
                Return strCuentasInventario

            End Get

        End Property

        Public ReadOnly Property CuentaIngreso(ByVal Tipo As String) As String

            Get

                Dim strCuentasInventario As String = ""
                Dim drwCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVRow

                If Not String.IsNullOrEmpty(Tipo) Then
                    drwCuentasInventario = m_dtsCuentasInventario.FindByU_Tipo(Tipo)

                    If drwCuentasInventario IsNot Nothing Then
                        If Not drwCuentasInventario.IsU_IngresoNull Then
                            strCuentasInventario = drwCuentasInventario.U_Ingreso
                        End If
                    End If

                End If
                Return strCuentasInventario

            End Get

        End Property

        Public ReadOnly Property AccesoriosXAlmacen(ByVal Tipo As String) As String

            Get

                Dim strCuentasInventario As String = ""
                Dim drwCuentasInventario As CuentasInventarioDataset.CuentasInventario_CVRow

                If Not String.IsNullOrEmpty(Tipo) Then
                    drwCuentasInventario = m_dtsCuentasInventario.FindByU_Tipo(Tipo)

                    If drwCuentasInventario IsNot Nothing Then
                        If Not drwCuentasInventario.IsU_AccXAlmNull Then
                            strCuentasInventario = drwCuentasInventario.U_AccXAlm
                        End If
                    End If

                End If
                Return strCuentasInventario

            End Get

        End Property

        Public ReadOnly Property GastosAdicionales(ByVal TipoDocumento As scgItemsFactura) As Integer

            Get

                Dim intGastoAdicional As Integer
                Dim drwGastoAdicional As ItemsGastosDataset.GatosVentas_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwGastoAdicional = m_dtsGastosAdicionales.FindByU_Cod_Item(TipoDocumento)
                    If drwGastoAdicional IsNot Nothing Then
                        If Not drwGastoAdicional.IsU_Cod_GANull Then
                            intGastoAdicional = drwGastoAdicional.U_Cod_GA
                        Else
                            Return 0
                            'Throw New ApplicationException("Gasto Adicional " & TipoDocumento.ToString & " No definido")
                        End If
                    Else
                        Return 0
                        'Throw New ApplicationException("Gasto Adicional " & TipoDocumento.ToString & " No definido")
                    End If
                Else
                    Return 0
                    'Throw New ApplicationException("Gasto Adicional " & TipoDocumento.ToString & " No definido")
                End If
                Return intGastoAdicional

            End Get

        End Property

        Public ReadOnly Property LineasFactura(ByVal TipoDocumento As scgItemsFactura) As String

            Get

                Dim strLineaFactura As String = ""
                Dim drwLineaFactura As ItemsVentasDataset.ItemVentas_CVRow
                If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                    drwLineaFactura = m_dtsLineasFactura.FindByU_Cod_Item(TipoDocumento)
                    If drwLineaFactura IsNot Nothing Then
                        If Not drwLineaFactura.IsU_ItemCodeNull Then
                            strLineaFactura = drwLineaFactura.U_ItemCode
                        Else
                            Return String.Empty
                            'Throw New ApplicationException("Ítem Venta " & TipoDocumento.ToString & " No definido")
                        End If
                    Else
                        Return String.Empty
                        'Throw New ApplicationException("Ítem Venta " & TipoDocumento.ToString & " No definido")
                    End If
                Else
                    Return String.Empty
                    'Throw New ApplicationException("Ítem Venta " & TipoDocumento.ToString & " No definido")
                End If
                Return strLineaFactura

            End Get

        End Property

        Public ReadOnly Property TransaccionAsientoAjuste() As String
            Get
                If Not m_drwGenerales.IsU_TRAN_AANull Then
                    Return m_drwGenerales.U_TRAN_AA
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property TransaccionCostoAjuste() As String
            Get
                If Not m_drwGenerales.IsU_OTROS_ACNull Then
                    Return m_drwGenerales.U_OTROS_AC
                Else
                    Return ""
                End If
            End Get
        End Property

        Public ReadOnly Property ChooseFromListVehiculos() As String
            Get

                If Not m_drwGenerales.IsU_CFL_VehiNull Then
                    Return m_drwGenerales.U_CFL_Vehi
                Else
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property DisponibilidadVehiculoRecibido() As String
            Get

                If Not m_drwGenerales.IsU_Disp_RNull Then
                    Return m_drwGenerales.U_Disp_R
                Else
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property DisponibilidadVehiculoVendido() As String
            Get

                If Not m_drwGenerales.IsU_Disp_VNull Then
                    Return m_drwGenerales.U_Disp_V
                Else
                    Throw New ApplicationException("Disponibilidad Vehículo Vendido No Definida")
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property EtapaFinalCRM() As String
            Get

                If Not m_drwGenerales.IsU_Disp_VNull Then
                    Return m_drwGenerales.U_Etap_CRM
                Else
                    Throw New ApplicationException("Etapa Final CRM no Definida")
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property InventarioVehiculoRecibido() As String
            Get

                If Not m_drwGenerales.IsU_Inven_RNull Then
                    Return m_drwGenerales.U_Inven_R
                Else

                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property InventarioVehiculoVendido() As String
            Get

                If Not m_drwGenerales.IsU_Inven_VNull Then
                    Return m_drwGenerales.U_Inven_V
                Else
                    Throw New ApplicationException("Inventario Vehículo No Definido")
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property MontoGastosLocales() As Decimal
            Get

                If Not m_drwGenerales.IsU_Monto_GLNull Then
                    Return m_drwGenerales.U_Monto_GL
                Else
                    Return CDec(0)
                End If

            End Get
        End Property

        Public ReadOnly Property PlacaProvisional() As String
            Get

                If Not m_drwGenerales.IsU_Placa_PrNull Then
                    Return m_drwGenerales.U_Placa_Pr
                Else
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property DireccionReportes() As String
            Get

                If Not m_drwGenerales.IsU_ReportesNull Then
                    Return m_drwGenerales.U_Reportes & "\"
                Else
                    Throw New ApplicationException("Dirección de Reportes No Definido")
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property SerieUnidades() As String
            Get

                If Not m_drwGenerales.IsU_Serie_UNull Then
                    Return m_drwGenerales.U_Serie_U
                Else
                    Return ""
                End If

            End Get
        End Property

        Public ReadOnly Property TipoDocumentoDeuda() As SAPbobsCOM.BoDocumentSubType
            Get

                If Not m_drwGenerales.IsU_Tipo_DDNull Then
                    Return m_drwGenerales.U_Tipo_DD
                Else
                    Return SAPbobsCOM.BoDocumentSubType.bod_None
                End If

            End Get
        End Property

        Public ReadOnly Property CodigoTransaccionAccesorioCosteo() As String
            Get

                If Not m_drwGenerales.IsU_CTCosAccNull Then
                    Return m_drwGenerales.U_CTCosAcc
                Else
                    Throw New ApplicationException(My.Resources.ResourceFrameWork.CosteoAccesorios)
                    Return ""
                End If

            End Get
        End Property

       

#End Region

#Region "Métodos"

        Private Sub InicializarSeries()

            If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                m_dtaSeries = New SeriesDatasetTableAdapters.Series_CVTableAdapter()
                m_dtsSeries = New SeriesDataset.Series_CVDataTable
                m_dtaSeries.Connection = m_cnConeccion
                m_dtaSeries.Fill(m_dtsSeries, m_strTipoInventario)

            Else
                Throw New ApplicationException("El tipo no se ha inicializado")
            End If

        End Sub

        Private Sub InicializarCuentasAdicionales()

            If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                m_dtaCuentasAdicionales = New CuentasAdicionalesDatasetTableAdapters.CuentasAdicionales_CVTableAdapter
                m_dtsCuentasAdicionales = New CuentasAdicionalesDataset.CuentasAdicionales_CVDataTable
                m_dtaCuentasAdicionales.Connection = m_cnConeccion
                m_dtaCuentasAdicionales.Fill(m_dtsCuentasAdicionales, m_strTipoInventario)

            Else
                Throw New ApplicationException("El tipo no se ha inicializado")
            End If

        End Sub

        Private Overloads Sub InicializarGenerales()


            m_dtaGenerales = New ConfiguracionesGeneralesDatasetTableAdapters.Generales_CVTableAdapter
            m_dtsGenerales = New ConfiguracionesGeneralesDataset.Generales_CVDataTable
            m_dtaGenerales.Connection = m_cnConeccion
            m_dtaGenerales.Fill(m_dtsGenerales)
            m_drwGenerales = m_dtsGenerales.FindByCode("DMS")
            If m_drwGenerales Is Nothing Then
                Throw New ApplicationException("Configuración General No Definida")

            End If
        End Sub

        Private Sub InicializarCuentasInventario()

            m_dtaCuentasInventario = New CuentasInventarioDatasetTableAdapters.CuentasInventario_CVTableAdapter
            m_dtsCuentasInventario = New CuentasInventarioDataset.CuentasInventario_CVDataTable
            m_dtaCuentasInventario.Connection = m_cnConeccion
            m_dtaCuentasInventario.Fill(m_dtsCuentasInventario)

        End Sub

        Private Sub InicializarImpuestos()

            If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                m_dtaImpuestos = New ImpuestosDatasetTableAdapters.Impuesto_CVTableAdapter
                m_dtsImpuestos = New ImpuestosDataset.Impuesto_CVDataTable
                m_dtaImpuestos.Connection = m_cnConeccion
                m_dtaImpuestos.Fill(m_dtsImpuestos, m_strTipoInventario)
            Else
                Throw New ApplicationException("El tipo no se ha inicializado")
            End If

        End Sub

        Private Sub InicializarGastosAdicionales()

            If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                m_dtaGastosAdicionales = New ItemsGastosDatasetTableAdapters.GatosVentas_CVTableAdapter
                m_dtsGastosAdicionales = New ItemsGastosDataset.GatosVentas_CVDataTable
                m_dtaGastosAdicionales.Connection = m_cnConeccion
                m_dtaGastosAdicionales.Fill(m_dtsGastosAdicionales, m_strTipoInventario)
            Else
                Throw New ApplicationException("El tipo no se ha inicializado")
            End If

        End Sub

        Private Sub InicializarLineasFactura()

            If Not String.IsNullOrEmpty(m_strTipoInventario) Then
                m_dtaLineasFactura = New ItemsVentasDatasetTableAdapters.ItemVentas_CVTableAdapter
                m_dtsLineasFactura = New ItemsVentasDataset.ItemVentas_CVDataTable
                m_dtaLineasFactura.Connection = m_cnConeccion
                m_dtaLineasFactura.Fill(m_dtsLineasFactura, m_strTipoInventario)

            Else
                Throw New ApplicationException("El tipo no se ha inicializado")
            End If

        End Sub

#End Region

    End Class

End Namespace
