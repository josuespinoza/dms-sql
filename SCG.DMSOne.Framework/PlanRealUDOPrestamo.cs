using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class PlanRealUDOPrestamo:ILineaUDO
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

        [UDOBind("U_Int_Mora")]
        public float Moratorio { get; set; }

        [UDOBind("U_Sal_Fin")]
        public float SaldoFinal { get; set; }

        [UDOBind("U_Pagado")]
        public string Pagado { get; set; }

        [UDOBind("U_Cap_Pend")]
        public float CapPend { get; set; }

        [UDOBind("U_Int_Pend")]
        public float IntPend { get; set; }

        [UDOBind("U_Mor_Pend")]
        public float MoraPend { get; set; }

        [UDOBind("U_Dias_Int")]
        public int DiasInt { get; set; }

        [UDOBind("U_Dias_Mor")]
        public int DiasMora { get; set; }

    }
}
