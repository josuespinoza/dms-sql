using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework.UDOOrden
{
    public class ControlColaboradorLineaUDOOrden
    {
        [UDOBind("U_Colab")]
        public string U_Colab { get; set; }

        [UDOBind("U_FIni")]
        public DateTime U_FIni { get; set; }

        [UDOBind("U_FFin")]
        public DateTime U_FFin { get; set; }

        [UDOBind("U_TMin")]
        public double U_TMin { get; set; }

        [UDOBind("U_RePro")]
        public string U_RePro { get; set; }

        [UDOBind("U_NoFas")]
        public string U_NoFas { get; set; }

        [UDOBind("U_Estad")]
        public string U_Estad { get; set; }

        [UDOBind("U_IdAct")]
        public string U_IdAct { get; set; }

        [UDOBind("U_CosRe")]
        public double U_CosRe { get; set; }

        [UDOBind("U_CosEst")]
        public double U_CosEst { get; set; }

        [UDOBind("U_ReAsig")]
        public string U_ReAsig { get; set; }

    }
}
