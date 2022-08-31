Imports System

<Serializable()> _
Public Class DocumentoMarketing
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property LineNum() As Integer
        Get
            Return intLineNum
        End Get
        Set(ByVal value As Integer)
            intLineNum = value
        End Set
    End Property
    Private intLineNum As Integer

    Public Property VisOrder() As Integer
        Get
            Return intVisOrder
        End Get
        Set(ByVal value As Integer)
            intVisOrder = value
        End Set
    End Property
    Private intVisOrder As Integer

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property ItemDescripcion() As String
        Get
            Return strItemDescripcion
        End Get
        Set(ByVal value As String)
            strItemDescripcion = value
        End Set
    End Property
    Private strItemDescripcion As String

    Public Property TipoArticulo() As Integer
        Get
            Return intTipoArticulo
        End Get
        Set(ByVal value As Integer)
            intTipoArticulo = value
        End Set
    End Property
    Private intTipoArticulo As Integer

    Public Property Costo() As Double
        Get
            Return dblCosto
        End Get
        Set(ByVal value As Double)
            dblCosto = value
        End Set
    End Property
    Private dblCosto As Double

    Public Property CostoFactura() As Double
        Get
            Return dblCostoFactura
        End Get
        Set(ByVal value As Double)
            dblCostoFactura = value
        End Set
    End Property
    Private dblCostoFactura As Double

    Public Property Cantidad() As Double
        Get
            Return dblCantidad
        End Get
        Set(ByVal value As Double)
            dblCantidad = value
        End Set
    End Property
    Private dblCantidad As Double

    Public Property Comprar() As String
        Get
            Return strComprar
        End Get
        Set(ByVal value As String)
            strComprar = value
        End Set
    End Property
    Private strComprar As String

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String

    Public Property Sucursal() As String
        Get
            Return strSucursal
        End Get
        Set(ByVal value As String)
            strSucursal = value
        End Set
    End Property
    Private strSucursal As String

    Public Property Procesar() As Boolean
        Get
            Return blnProcesar
        End Get
        Set(ByVal value As Boolean)
            blnProcesar = value
        End Set
    End Property
    Private blnProcesar As Boolean

    Public Property IdRepxOrd() As Integer
        Get
            Return intIdRepxOrd
        End Get
        Set(ByVal value As Integer)
            intIdRepxOrd = value
        End Set
    End Property
    Private intIdRepxOrd As Integer

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property NoOrdenPadre() As String
        Get
            Return strNoOrdenPadre
        End Get
        Set(ByVal value As String)
            strNoOrdenPadre = value
        End Set
    End Property
    Private strNoOrdenPadre As String

    Public Property NoOrdenHija() As String
        Get
            Return strNoOrdenHija
        End Get
        Set(ByVal value As String)
            strNoOrdenHija = value
        End Set
    End Property
    Private strNoOrdenHija As String

    Public Property DocEntryTarget() As Integer
        Get
            Return intDocEntryTarget
        End Get
        Set(ByVal value As Integer)
            intDocEntryTarget = value
        End Set
    End Property
    Private intDocEntryTarget As Integer

    Public Property DocTypeTarget() As Integer
        Get
            Return intDocTypeTarget
        End Get
        Set(ByVal value As Integer)
            intDocTypeTarget = value
        End Set
    End Property
    Private intDocTypeTarget As Integer

    Public Property BaseDocEntry() As Integer
        Get
            Return intBaseDocEntry
        End Get
        Set(ByVal value As Integer)
            intBaseDocEntry = value
        End Set
    End Property
    Private intBaseDocEntry As Integer

    Public Property BaseDocType() As Integer
        Get
            Return intBaseDocType
        End Get
        Set(ByVal value As Integer)
            intBaseDocType = value
        End Set
    End Property
    Private intBaseDocType As Integer

    Public Property TipoDocumentoMarketing() As Integer
        Get
            Return intTipoDocumentoMarketing
        End Get
        Set(ByVal value As Integer)
            intTipoDocumentoMarketing = value
        End Set
    End Property
    Private intTipoDocumentoMarketing As Integer

    Public Property DocEntryOfertaCompra() As Integer
        Get
            Return intDocEntryOfertaCompra
        End Get
        Set(ByVal value As Integer)
            intDocEntryOfertaCompra = value
        End Set
    End Property
    Private intDocEntryOfertaCompra As Integer

    Public Property DocEntryOrdenCompra() As Integer
        Get
            Return intDocEntryOrdenCompra
        End Get
        Set(ByVal value As Integer)
            intDocEntryOrdenCompra = value
        End Set
    End Property
    Private intDocEntryOrdenCompra As Integer

    Public Property DocEntryEntradaMercancia() As Integer
        Get
            Return intDocEntryEntradaMercancia
        End Get
        Set(ByVal value As Integer)
            intDocEntryEntradaMercancia = value
        End Set
    End Property
    Private intDocEntryEntradaMercancia As Integer

    Public Property DocEntryFacturaProveedor() As Integer
        Get
            Return intDocEntryFacturaProveedor
        End Get
        Set(ByVal value As Integer)
            intDocEntryFacturaProveedor = value
        End Set
    End Property
    Private intDocEntryFacturaProveedor As Integer

    Public Property TipoOT() As String
        Get
            Return strTipoOT
        End Get
        Set(ByVal value As String)
            strTipoOT = value
        End Set
    End Property
    Private strTipoOT As String

    Public Property BodegaOrigen() As String
        Get
            Return strBodegaOrigen
        End Get
        Set(ByVal value As String)
            strBodegaOrigen = value
        End Set
    End Property
    Private strBodegaOrigen As String


    Public Property CodigoProyecto() As String
        Get
            Return strCodigoProyecto
        End Get
        Set(ByVal value As String)
            strCodigoProyecto = value
        End Set
    End Property
    Private strCodigoProyecto As String

    Public Property CodigoMarca() As String
        Get
            Return strCodigoMarca
        End Get
        Set(ByVal value As String)
            strCodigoMarca = value
        End Set
    End Property
    Private strCodigoMarca As String

    Public Property UsaDimensiones() As Boolean
        Get
            Return blnUsaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensiones = value
        End Set
    End Property
    Private blnUsaDimensiones As Boolean

    Public Property CostingCode() As String
        Get
            Return strCostingCode
        End Get
        Set(ByVal value As String)
            strCostingCode = value
        End Set
    End Property
    Private strCostingCode As String

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property
    Private strCostingCode2 As String


    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property
    Private strCostingCode3 As String

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property
    Private strCostingCode4 As String

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property
    Private strCostingCode5 As String

    Public Property MonedaManoObra() As String
        Get
            Return strMonedaManoObra
        End Get
        Set(ByVal value As String)
            strMonedaManoObra = value
        End Set
    End Property
    Private strMonedaManoObra As String

    Public Property CuentaCreditoManoObra() As String
        Get
            Return strCuentaCreditoManoObra
        End Get
        Set(ByVal value As String)
            strCuentaCreditoManoObra = value
        End Set
    End Property
    Private strCuentaCreditoManoObra As String

    Public Property MonedaOtrosGastos() As String
        Get
            Return strMonedaOtrosGastos
        End Get
        Set(ByVal value As String)
            strMonedaOtrosGastos = value
        End Set
    End Property
    Private strMonedaOtrosGastos As String

    Public Property CuentaCreditoOtrosGastos() As String
        Get
            Return strCuentaCreditoOtrosGastos
        End Get
        Set(ByVal value As String)
            strCuentaCreditoOtrosGastos = value
        End Set
    End Property
    Private strCuentaCreditoOtrosGastos As String

    Public Property Almacen() As String
        Get
            Return strAlmacen
        End Get
        Set(ByVal value As String)
            strAlmacen = value
        End Set
    End Property
    Private strAlmacen As String

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property CostoAplicado() As Boolean
        Get
            Return blnCostoAplicado
        End Get
        Set(ByVal value As Boolean)
            blnCostoAplicado = value
        End Set
    End Property
    Private blnCostoAplicado As Boolean

    Public Property TrackingAplicado() As Boolean
        Get
            Return blnTrackingAplicado
        End Get
        Set(ByVal value As Boolean)
            blnTrackingAplicado = value
        End Set
    End Property
    Private blnTrackingAplicado As Boolean

    Private WithoutInventoryMovement As SAPbobsCOM.BoYesNoEnum
    Public Property SinMovimientoInventario() As SAPbobsCOM.BoYesNoEnum
        Get
            Return WithoutInventoryMovement
        End Get
        Set(ByVal value As SAPbobsCOM.BoYesNoEnum)
            WithoutInventoryMovement = value
        End Set
    End Property
End Class
