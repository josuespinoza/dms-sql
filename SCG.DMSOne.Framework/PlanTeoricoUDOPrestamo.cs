using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class PlanTeoricoUDOPrestamo : ILineaUDO
    {

        [UDOBind("U_Numero")]
        public int NumeroPago { get; set; }

        [UDOBind("U_Fecha")]
        public DateTime FechaPago { get; set; }

        [UDOBind("U_Sal_Ini")]
        public float SaldoInicial { get; set; }

        [UDOBind("U_Cuota")]
        public float Cuota { get; set; }

        [UDOBind("U_Capital")]
        public float Capital { get; set; }

        [UDOBind("U_Interes")]
        public float Interes { get; set; }

        [UDOBind("U_Sal_Fin")]
        public float SaldoFinal { get; set; }

    }
}
