using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class TramitesXContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Cod_Tram { get; set; }
        public String U_Des_Tram { get; set; }
        public Int32? U_Cant { get; set; }
        public Double? U_Pre_Uni { get; set; }
        public Double? U_Costo { get; set; }
        public String U_Imp_Com { get; set; }
        public String U_ProvTram { get; set; }
        public String U_Comprar { get; set; }
        public String U_Ord_Comp { get; set; }
        public Double? U_Pre_Tot { get; set; }
        public String U_SCGD_Fct { get; set; }
        public String U_Imp_Vent { get; set; }
        public String U_CABYS_AE { get; set; }
        public String U_CABYS_TI { get; set; }
        public String U_CABYS_CH { get; set; }
    }
}
