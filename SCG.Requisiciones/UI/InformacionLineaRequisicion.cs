using System;
using System.IO;
using System.Xml.Serialization;

namespace SCG.Requisiciones.UI
{
    public class InformacionLineaRequisicion
    {
        public int DocEntry { get; set; }
        public int DataSourceOffset { get; set; }
        public int LineId { get; set; }
        public int VisOrder { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public string CodigoBodegaOrigen { get; set; }
        public string CodigoBodegaDestino { get; set; }
        public double CantidadRecibida { get; set; }
        public double CantidadSolicitada { get; set; }
        public double CantidadPendiente { get; set; }
        public double CantidadATransferir { get; set; }
        public int CodigoTipoArticulo { get; set; }
        public string DescripcionTipoArticulo { get; set; }
        public int CodigoEstado { get; set; }
        public string Estado { get; set; }
        public int CentroCosto { get; set; }
        public bool Seleccionada { get; set; }
        public int DocumentoOrigen { get; set; }
        public int LineNumOrigen { get; set; }
        public double CantidadOriginal { get; set; }
        public double CantidadAjuste { get; set; }
        public string DeUbicacion { get; set; }
        public string AUbicacion { get; set; }
        public string LineaIDSucursal { get; set; }
        public string IDLinea { get; set; }
        public int LineaReqOrPen { get; set; }   
    }
}