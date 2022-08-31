using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class VehiculosXContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Cod_Unid { get; set; }
        public String U_Des_Marc { get; set; }
        public String U_Des_Mode { get; set; }
        public String U_Des_Esti { get; set; }
        public Int32? U_Ano_Vehi { get; set; }
        public String U_Num_Plac { get; set; }
        public String U_Des_Col { get; set; }
        public String U_Num_VIN { get; set; }
        public String U_Num_Mot { get; set; }
        public String U_Transmi { get; set; }
        public Double? U_Pre_Vta { get; set; }
        public Double? U_Pagos { get; set; }
        public String U_Impuesto { get; set; }
        public Double? U_Desc_Veh { get; set; }
        public Double? U_Pre_Tot { get; set; }
        public Double? U_Mon_Acc { get; set; }
        public Double? U_Gas_Loc { get; set; }
        public Double? U_Otro_Gas { get; set; }
        public Double? U_Bono { get; set; }
        public Double? U_MDesc { get; set; }
        public Double? U_PreNet { get; set; }
        public String U_TipIn { get; set; }
        public String U_ColIn { get; set; }
        public String U_Obser { get; set; }
        public Double? U_Km_Venta { get; set; }
        public String U_CABYS_AE { get; set; }
        public String U_CABYS_TI { get; set; }
        public String U_CABYS_CH { get; set; }
    }
}
