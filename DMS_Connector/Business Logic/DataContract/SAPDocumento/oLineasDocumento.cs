using System;

namespace DMS_Connector.Business_Logic.DataContract.SAPDocumento
{
    public class oLineasDocumento
    {
        public Int32? DocEntry { get; set; }
        public Int32 DocNum { get; set; }
        public String CodigoSucursal { get; set; }
        public Int32? Estado { get; set; }
        public Boolean UsaFinPropio { get; set; }
        public Boolean UsaFinExterno { get; set; }
        public String ComentarioRechazo { get; set; }
        public String NoOrden { get; set; }
        public String IDSucursal { get; set; }
        public Int32? LineNum { get; set; }
        public String ItemCode { get; set; }
        public String Description { get; set; }
        public Double? Quantity { get; set; }
        public Double? OriginalQuantity { get; set; }
        public Double? CantidadRecibida { get; set; }
        public Double? CantidadSolicitada { get; set; }
        public Double? CantidadPendiente { get; set; }
        public Double? CantidadPendienteBodega { get; set; }
        public Double? CantidadPendienteTraslado { get; set; }
        public Double? CantidadPendienteDevolucion { get; set; }
        public Double? Price { get; set; }
        public Double? LineTotal { get; set; }
        public SAPbobsCOM.BoItemTreeTypes TreeType { get; set; }
        public Int32? IdRepxOrd { get; set; }
        public Int32? Aprobado { get; set; }
        public Int32? Trasladado { get; set; }
        public Int32? AprobadoOriginal { get; set; }
        public Int32? TrasladadoOriginal { get; set; }
        public Double? Costo { get; set; }
        public Int32? OTHija { get; set; }
        public String Entregado { get; set; }
        public Int32? TipoArticulo { get; set; }
        public String Comprar { get; set; }
        public String ID { get; set; }
        public String CentroCosto { get; set; }
        public Boolean Procesar { get; set; }
        public Int32? ProcesarInteger { get; set; }
        public String BodegaOrigen { get; set; }
        public String BodegaDestino { get; set; }
        public String BodegaRepuesto { get; set; }
        public String BodegaServicio { get; set; }
        public String BodegaSuministro { get; set; }
        public String BodegaServicioExterno { get; set; }
        public String BodegaProceso { get; set; }
        public String BodegaReservas { get; set; }
        public Boolean UsaUbicaciones { get; set; }
        public String UbicacionDBP { get; set; }
        public Int32? DuracionEstandar { get; set; }
        public String EstadoActividad { get; set; }
        public String EmpleadoAsignado { get; set; }
        public String NombreEmpleado { get; set; }
        public Double? CostoReal { get; set; }
        public Double? CostoEstandar { get; set; }
        public String FechaInicioActividad { get; set; }
        public String FechaFinalActividad { get; set; }
        public String HoraInicio { get; set; }
        public String FaseProduccion { get; set; }
        public String PertenecePaquete { get; set; }
        public Double? CantidadStock { get; set; }
        public Int32? TipoMovimiento { get; set; }
        public String Resultado { get; set; }
        public String UbicacionOrigen { get; set; }
        public String UbicacionDestino { get; set; }
        public String PaquetePadre { get; set; }
        public Int32? VisOrder { get; set; }
        public Boolean RequisicionDevolucion { get; set; }
        public Boolean EsAdicional { get; set; }
        public Int32? ProcesamientoLinea { get; set; }
        public String Sucursal { get; set; }
        public String VatGroup { get; set; }
        public String TaxCode { get; set; }
        public String FreeText { get; set; }
        public String CostingCode { get; set; }
        public String CostingCode2 { get; set; }
        public String CostingCode3 { get; set; }
        public String CostingCode4 { get; set; }
        public String CostingCode5 { get; set; }

        public String TipoOT { get; set; }
        public String CodigoProyecto { get; set; }
        public String TipoRepuesto { get; set; }
        public String Currency { get; set; }
        public Boolean RepuestoReparacion { get; set; }
        public String LineDscPrcnt { get; set; }
        //Propiedades Internas
        public int intPosicion { get; set; }

        //Interface John Deere
        public Double ReserveQ_PT { get; set; }
        public Int32 ReservedHits_WO { get; set; }
        public Int32 ReservedHits_PT { get; set; }

        public Double CurrrentMTDSales { get; set; }
        public Int32 CurrentMTDHits { get; set; }
        public Double CurrentMTDLostSales { get; set; }
        public Int32 CurrentMTDLostHits { get; set; }

        public Double Sales_Month { get; set; }
        public Int32 Hits_Month { get; set; }
        public Double LostSales_Month { get; set; }
        public Int32 LostHits_Month { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
