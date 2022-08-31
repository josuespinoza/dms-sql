using System;
using System.Collections.Generic;

namespace DMS_Connector.Business_Logic.DataContract.SAPDocumento
{
    public class oDocumento
    {
        public Int32? DocEntry { get; set; }
        public Int32? DocNum { get; set; }
        public String NoOrden { get; set; }
        public String Sucursal { get; set; }
        public String IDSucursal { get; set; }
        public Int32? GeneraOT { get; set; }
        public String EstadoCotizacionID { get; set; }
        public DateTime? FechaCreacionOT { get; set; }
        public DateTime? HoraCreacionOT { get; set; }
        public String GeneraRecepcion { get; set; }
        public String OTPadre { get; set; }
        public String NoOTReferencia { get; set; }
        public String NumeroVIN { get; set; }
        public String CodigoUnidad { get; set; }
        public Int32? NumeroVehiculo { get; set; }
        public Int32? CodigoAsesor { get; set; }
        public Int32? TipoOT { get; set; }
        public String CodigoMarca { get; set; }
        public String CodigoProyecto { get; set; }
        public SAPbobsCOM.BoYesNoEnum CotizacionCancelled { get; set; }
        public SAPbobsCOM.BoStatus CotizacionDocumentStatus { get; set; }
        public String CardCode { get; set; }
        public SAPbobsCOM.BoCardTypes CardType { get; set; }
        public String NoVisita { get; set; }
        public String EstadoCotizacion { get; set; }
        public String NoSerieCita { get; set; }
        public String CardName { get; set; }
        public String NombreAsesor { get; set; }
        public String Cono { get; set; }
        public String Year { get; set; }
        public String DescripcionMarca { get; set; }
        public String DescripcionModelo { get; set; }
        public String DescripcionEstilo { get; set; }
        public String CodigoModelo { get; set; }
        public String CodigoEstilo { get; set; }
        public Double? HorasServicio { get; set; }
        public Int32? Kilometraje { get; set; }
        public String Placa { get; set; }
        public String NombreClienteOT { get; set; }
        public String CodigoClienteOT { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? HoraRecepcion { get; set; }
        public DateTime? FechaCompromiso { get; set; }
        public DateTime? HoraCompromiso { get; set; }
        public Int32? NivelGasolina { get; set; }
        public String Observaciones { get; set; }
        public Int32? CodeMaestroVehiculo { get; set; }
        public String NoCita { get; set; }
        public String DocCurrency { get; set; }
        public String IDEstOTTC { get; set; }
        public Double CantidadPaneles { get; set; }
        public String CodTiemposFase { get; set; }
        public String CodControlProcesos { get; set; }
        public Double m2 { get; set; }
        public String Serie { get; set; }
        public String Comments { get; set; }
        public String SlpCode { get; set; }
        public String DiscountPercent { get; set; }
        public List<oLineasDocumento> Lineas { get; set; }
    }
}
