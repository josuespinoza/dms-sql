using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class DesgloceCobroUDOContratoVenta : ILineaUDO
    {
        [UDOBind("U_Cod_Item")]
        public string CodigoArticulo { get; set; }

        [UDOBind("U_Nom_Item")]
        public string DescripcionArticulo { get; set; }

        [UDOBind("U_Descuent")]
        public float Descuento { get; set; }

        [UDOBind("U_Monto")]
        public float Monto { get; set; }

        [UDOBind("U_No_NC")]
        public int NotaCredito { get; set; }

        [UDOBind("U_No_ND")]
        public int NotaDebito { get; set; }
    }
}