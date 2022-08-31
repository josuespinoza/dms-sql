using System;

namespace SCG.Requisiciones.UI
{
    public class InformacionLineasMovimientos
    {
        public int DocEntry { get; set; }
        public int LineId { get; set; }
        public int VisOrder { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public double CantidadTransferida { get; set; }
        public int CodigoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime Fecha { get; set; }
    }
}