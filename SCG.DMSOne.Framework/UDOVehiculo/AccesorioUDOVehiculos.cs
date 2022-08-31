using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class AccesorioUDOVehiculos : ILineaUDO
    {
        [UDOBind("U_Acc")]
        public string Accesorio { get; set; }

        [UDOBind("U_N_Acc")]
        public string Nombre { get; set; }
    }
}