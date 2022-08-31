using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class UsadosXContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Cod_Unid { get; set; }
        public String U_Marca { get; set; }
        public String U_Estilo { get; set; }
        public String U_Motor { get; set; }
        public String U_VIN { get; set; }
        public String U_Anio { get; set; }
        public String U_Placa { get; set; }
        public String U_Color { get; set; }
        public String U_Tipo { get; set; }
        public Int32? U_RTV_MM { get; set; }
        public Int32? U_RTV_AA { get; set; }
        public Double? U_Val_Rec { get; set; }
        public Double? U_Aj_Cos { get; set; }
        public String U_Gravamen { get; set; }
        public DateTime? U_Fec_Av { get; set; }
        public DateTime? U_Der_Cir { get; set; }
        public DateTime? U_Gra_Fec { get; set; }
        public String U_TraUs { get; set; }
        public String U_MoUs { get; set; }
        public String U_CoUs { get; set; }
        public String U_Cod_Mod_Us { get; set; }
        public String U_Cod_Col_Us { get; set; }
        public String U_Cod_Trans_Us { get; set; }
        public String U_Cod_Comb_Us { get; set; }
        public String U_Cod_Estilo_Us { get; set; }
        public String U_Cod_Marca_Us { get; set; }
        public String U_Cod_Clasif_Us { get; set; }
        public String U_Des_Clasif_Us { get; set; }
        public Double? U_Val_Venta { get; set; }
        public String U_CatUs { get; set; }
        public Double? U_KmUs { get; set; }
        public String U_Cod_Prov { get; set; }
        public String U_Nom_Prov { get; set; }
        public String U_Existe { get; set; }
        public String U_N_FP { get; set; }
        public String U_N_AsAd { get; set; }
        public String U_CABYS_AE { get; set; }
        public String U_CABYS_TI { get; set; }
        public String U_CABYS_CH { get; set; }
    }
}
