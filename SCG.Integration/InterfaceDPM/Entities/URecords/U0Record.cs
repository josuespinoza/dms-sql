using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities.URecords
{
    public class U0Record
    {
        public U0Record()
        {
        }

        public String RecordCode { get; set; }
        public String MainAccount1_2 { get; set; }
        public String SourceAccount1_2 { get; set; }
        public String MainAccount3_6 { get; set; }
        public String IDRecord { get; set; }
        public String InterfaceVersion { get; set; }
        public String Date { get; set; }
        public String TypeRecord { get; set; }
        public String SourceAccount3_6 { get; set; }
        public String SalesCounter { get; set; }
        public String SalesShop { get; set; }
        public String SalesInternal { get; set; }
        public String ReturnCounter { get; set; }
        public String ReturnShop { get; set; }
        public String Warehouse { get; set; }
        public String WarehouseType { get; set; }

        public void ToString(ref StringBuilder p_sb)
        {
            String espacio = "\t";
            String vacio = "";
            String cero = "0";
            try
            {
                p_sb.Append(RecordCode);
                p_sb.Append(MainAccount1_2);
                p_sb.Append(SourceAccount1_2);
                p_sb.Append(MainAccount3_6);
                p_sb.Append(IDRecord);
                p_sb.Append(InterfaceVersion);
                p_sb.Append(Date);
                p_sb.Append("   ");
                p_sb.Append(TypeRecord);
                p_sb.Append(SourceAccount3_6);
                p_sb.Append(SalesCounter );
                p_sb.Append(SalesShop);
                p_sb.Append(SalesInternal);
                p_sb.Append(ReturnCounter);
                p_sb.Append(ReturnShop);
                p_sb.Append("      ");
                p_sb.Append(Warehouse);
                p_sb.Append(WarehouseType);

                p_sb.Append("\n");
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    }
}
