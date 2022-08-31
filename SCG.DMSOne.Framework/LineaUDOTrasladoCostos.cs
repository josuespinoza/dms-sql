using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class LineaUDOTrasladoCostos : ILineaUDO
    {
        [UDOBind("DocEntry", SoloLectura = true ) ]
        public int  DocEntry { get; set; }

        [UDOBind("U_SCGD_Cod")]
        public string Codigo { get; set; }

        [UDOBind("U_SCGD_Inv")]
        public string InventarioDst { get; set; }

        [UDOBind("U_SCGD_Des")]
        public string DescripcionInvDst { get; set; }

        [UDOBind("U_SCGD_CSi")]
        public float CostoSistema { get; set; }

        [UDOBind("U_SCGD_Cos")]
        public float CostoLocal { get; set; }

        [UDOBind("U_SCGD_InO")]
        public string InventarioOr { get; set; }

        [UDOBind("U_SCGD_DsO")]
        public string DescripcionInvOr { get; set; }

        [UDOBind("U_SCGD_NCO")]
        public string NomCuentaOr { get; set; }

        [UDOBind("U_SCGD_NCD")]
        public string NomCuentaDst { get; set; }

        [UDOBind("U_SCGD_FCO")]
        public string FormatCodeOrigen { get; set; }

        [UDOBind("U_SCGD_FCD")]
        public string FormatCodeDestino { get; set; }
        
        [UDOBind("U_SCGD_Mar")]
        public string Marca { get; set; }
        
        [UDOBind("U_SCGD_Est")]
        public string Estilo{ get; set; }

        [UDOBind("U_SCGD_Vin")]
        public string NumeroVin { get; set; }
        
        [UDOBind("U_SCGD_EN")]
        public string Entrada { get; set; }
    


       }
}
