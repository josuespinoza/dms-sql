using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class LineaUDOSalidaVehiculo : ILineaUDO
    {
        [UDOBind("DocEntry", SoloLectura = true ) ]
        public int  DocEntry { get; set; }
               
        [UDOBind("U_SCGD_DocEntrada")]
        public string DocumentoEntrada { get; set; }

        [UDOBind("U_SCGD_AsEntrada")]
        public string AsientoEntrada { get; set; }

        [UDOBind("U_SCGD_Monto")]
        public float  MontoLocalEntrada { get; set; }

        [UDOBind("U_SCGD_MontoSist")]
        public float MontoSistemaEntrada { get; set; }
              


       }
}

