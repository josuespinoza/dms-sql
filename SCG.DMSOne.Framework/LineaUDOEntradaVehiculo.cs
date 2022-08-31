using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class LineaUDOEntradaVehiculo : ILineaUDO
    {
        [UDOBind("DocEntry", SoloLectura = true ) ]
        public int  DocEntry { get; set; }

        [UDOBind("U_Concepto")]
        public string Concepto { get; set; }

        [UDOBind("U_Mon_Loc")]
        public float Mon_Loc { get; set; }

        [UDOBind("U_Mon_Sis")]
        public float Mon_Sis { get; set; }

        [UDOBind("U_Mon_Reg")]
        public string Mon_Reg { get; set; }

        [UDOBind("U_Tip_Cam")]
        public float Tip_Cam { get; set; }

        [UDOBind("U_NoFP")]
        public string NoFP { get; set; }

        [UDOBind("U_No_FC")]
        public string No_FC { get; set; }

        [UDOBind("U_NoAsient")]
        public string NoAsient { get; set; }

        [UDOBind("U_Cuenta")]
        public string Cuenta { get; set; }

        [UDOBind("U_Cod_Tran")]
        public string Cod_Tran { get; set; }

    }
}
