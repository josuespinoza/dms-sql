namespace DMS_Connector.Data_Access
{
    public class GeneralEnums
    {
        public enum scgTipoCuenta
        {
            scgCuentaStock = 1,
            scgCuentaTransito = 2,
            scgCuentaCosto = 3,
            scgCuentaIngreso = 4,
            scgAlmacenSucursal = 5,
            scgAlmacenTramites = 6,
            scgAlmacenLogistica = 7,
            scgCuentaDevolucion = 8
        }

        public enum scgItemsFactura
        {
            PrecioVehículo = 1,
            PrecioAccesorios = 2,
            gastosIncripcion = 3,
            GastosPrenda = 4
        }

        public enum scgTipoSeries
        {
            FacturaVentas = 1,
            NotasCreditoUsados = 2,
            NotasCreditoDescuentos = 3,
            DocumentosDeuda = 4,
            FacturaProveedor = 5,
            NotasCreditoOtros = 6,
            DocumentosDeudaOtros = 7,
            PrimaVenta = 8,
            NotasCreditoReversion = 9,
            FacturaAccesorios = 10,
            FacturaExentaDeudoresVehiculoUsado = 11,
            FacturaExentaConsignados = 12,
            FacturaProveedoresDocumentoReciboUsadoSociedades = 13,
            FacturaProveedoresDocumentoReciboUsadoPrivado = 14,
            TramitesFacturables = 15,
            NotaCreditoReciboUsadoSociedades = 16,
            NotaCreditoReciboUsadoPrivado = 17,
            NotaCreditoReversionTramites = 18,
            NotaCreditoReversionAccesorios = 19,
            FacturaComisionConsignados = 20,
            NotaCreditoComisionConsignados = 21,
            NotaDebitoClienteReversionNCUsados = 22,
            FacturaGastos = 23,
            NotaCreditoReversionGastos = 24,
            NotaDebitoReversionNCDescuento = 25,
            NotaCreditoReversionFacturaDeudaUsado = 26
        }

        public enum scgMoneda
        {
            MonedaLocal = 1,
            MonedaExtranjera = 2
        }

        public enum scgTipoDocumentosCV
        {
            FacturaVentas = 1,
            FacturaDeudaUsado = 2,
            NotaDebito = 3,
            NotasCreditoDescuento = 4,
            NotasCreditoUsados = 5,
            AsientoAjusteCosto = 6,
            NotaDebitoDeudaUsado = 7,
            PrimaVenta = 8,
            AsientoSalidaAccesorios = 9,
            NotaCreditoDesglosedeCobro = 10,
            FacturaAccesorios = 11,
            FacturaGastosAdicionales = 12,
            AsientoFinancExterno = 13,
            AsientoTramites = 14,
            AsientoBonos = 15,
            AsientoOtrosCostos = 16,
            AsientoComisiones = 17,
            FacturaExentaVehiculoUsado = 18,
            FacturaProveedorVehiculoUsado = 19
        }

        public enum scgTipoPropiedadAdmin4
        {
            Transito,
            Stock,
            Costo,
            Ingreso,
            AccXAlm,
            Bod_Tram,
            Bod_Log,
            Devolucion
        }

        public enum CotizacionEstado
        {
            Creada = 1,
            Modificada = 2,
            SinCambio = 3
        }

        public enum EstadoActividades
        {
            NoIniciado = 1,
            Iniciado = 2,
            Suspendido = 3,
            Finalizado = 4
        }

        public enum TipoAdicional
        {
            Repuesto = 1,
            Servicio = 2,
            ServicioExterno = 4,
            Suministro = 3,
            Gastos = 5
        }

        public enum TipoArticulo
        {
            Repuesto = 1,
            Servicio = 2,
            Suministro = 3,
            ServExterno = 4,
            Paquete = 5,
            Ninguno = 0,
            OtrosGastos_Costos = 11,
            OtrosIngresos = 12
        }

        public enum EstadosTraslado
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4
        }

        public enum EstadosAprobacion
        {
            Aprobado = 1,
            NoAprobado = 2,
            FaltoAprobacion = 3
        }

        public enum ResultadoValidacionPorItem
        {
            SinCambio = 0,
            NoAprobar = 1,
            ModifCantiCotizacion = 2,
            PendTransf = 3,
            Comprar = 4,
            PendBodega = 5
        }

        public enum RealizarTraslado
        {
            No = 0,
            Si = 1
        }

        public enum EstadoOT
        {
            NoIniciada = 1,
            Iniciada = 2,
            Suspendida = 3,
            Finalizada = 4,
            Cancelada = 5,
            Cerrada = 6,
            Facturada = 7,
            Entregada = 8
        }

        public enum LineaAProcesar
        {
            Si = 1,
            No = 2
        }

        public enum EstadosSBO
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4,
            SolicitudTrasladoRequisicion = 5,
            SolicitudDevolucionRequisicion = 6
        }

        public enum EstadosRepuestosDMS
        {
            Pendiente = 1,
            Solicitado = 2,
            Recibido = 3,
            PendientePorDev = 4,
            PendienteTraslado = 5,
            PendienteBodegaDraft = 6,
            PendienteTrasladoRequisicion = 7,
            PendienteDevolucionRequisicion = 8
        }

        public enum RolesMensajeria
        {
            EncargadoRepuestos = 1,
            EncargadoProduccion = 2,
            EncargadoSolEspec = 3,
            EncargadoCompras = 4,
            EncargadoSOE = 5,
            EncargadoSuministros = 6
        }

        public enum EstadoRequisicion
        {
            Pendiente = 1,
            Trasladado = 2,
            Cancelado = 3
        }
    }
}
