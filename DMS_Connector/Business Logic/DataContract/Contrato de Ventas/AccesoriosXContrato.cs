using System;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class AccesoriosXContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Acc { get; set; }
        public String U_N_Acc { get; set; }
        public Double? U_SCGD_AccPrecio { get; set; }
        public String U_Imp_Acc { get; set; }
        public Int32? U_Cant_Acc { get; set; }
        public Double? U_AccPr_I { get; set; }
        public Double? U_Cost_Acc { get; set; }
        public Double? U_Desc_Acc { get; set; }
        public Double? U_PrTo_Acc { get; set; }
        public String U_Prov_Acc { get; set; }
        public String U_Comprar { get; set; }
        public String U_Ord_Acc { get; set; }
        public String U_Imp_Com { get; set; }
        public String U_CABYS_AE { get; set; }
        public String U_CABYS_TI { get; set; }
        public String U_CABYS_CH { get; set; }
    }
}
