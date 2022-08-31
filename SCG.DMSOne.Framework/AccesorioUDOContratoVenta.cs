using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class AccesorioUDOContratoVenta : ILineaUDO
    {
        [UDOBind("U_Acc")]
        public string CodigoAccesorio { get; set; }

        [UDOBind("U_N_Acc")]
        public string NombreAccesorio { get; set; }

        [UDOBind("U_SCGD_AccPrecio")]
        public float Precio { get; set; }
    }
}