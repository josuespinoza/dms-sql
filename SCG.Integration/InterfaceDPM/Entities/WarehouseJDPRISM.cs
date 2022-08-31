using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class WarehouseJDPRISM
    {
        public String WHHeaderRecordCode { get; set; }
        public String DealerAccount { get; set; }
        public String DBSWarehouse { get; set; }
        public int FiscalMonth { get; set; }
        public DateTime NextPartsMonthEndDate { get; set; }
        public int WarehouseType { get; set; }
        public int WhereDataIsToBeLoaded { get; set; }

        public void ToString(ref StringBuilder p_sb)
        {
            String espacio = "\t";
            try
            {
                p_sb.Append(WHHeaderRecordCode).Append(espacio);
                p_sb.Append(DealerAccount).Append(espacio);
                p_sb.Append(DBSWarehouse).Append(espacio);
                p_sb.Append(FiscalMonth.ToString()).Append(espacio);
                p_sb.Append(espacio);//p_sb.Append(NextPartsMonthEndDate.ToString( "dd/MM/yyyy")).Append(espacio);
                p_sb.Append(WarehouseType.ToString()).Append(espacio);
                p_sb.Append(WhereDataIsToBeLoaded.ToString()).Append(espacio);
                p_sb.Append("\r\n");
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    }
}
