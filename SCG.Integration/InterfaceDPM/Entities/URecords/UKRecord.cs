using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities.URecords
{
    public class UKRecord
    {
        public UKRecord()
        {
        }

        public String RecordCode { get; set; }
        public String MainAccount1_2 { get; set; }
        public String SourceAccount1_2 { get; set; }
        public String MainAccount3_6 { get; set; }
        public String IDRecord { get; set; }
        public String CriticalCode { get; set; }
        public String InventoryClass { get; set; }
        public String TypeRecord { get; set; }
        public String SourceAccount3_6 { get; set; }
        public String TotalPartsCostLast12 { get; set; }
        public String TotalPartsCostLast13to24 { get; set; }
        public String TotalCostMonth { get; set; }
        public String CurrentInventory { get; set; }
        public String NoSalesInventory { get; set; }
        public String Warehouse { get; set; }
        public String WarehouseType { get; set; }

        public void ToString(ref StringBuilder p_sb)
        {
            String espacio = " ";
            String cero = "0";
            try
            {
                p_sb.Append(RecordCode);
                p_sb.Append(MainAccount1_2);
                p_sb.Append(SourceAccount1_2);
                p_sb.Append(MainAccount3_6);
                p_sb.Append(IDRecord);
                p_sb.Append(CriticalCode);
                p_sb.Append(InventoryClass);
                for (int i = 0; i < 8; i++)
                {
                    p_sb.Append(espacio);
                }
                p_sb.Append(TypeRecord);
                p_sb.Append(SourceAccount3_6);
                p_sb.Append(TotalPartsCostLast12);
                p_sb.Append(TotalPartsCostLast13to24);
                p_sb.Append(TotalCostMonth);
                p_sb.Append(CurrentInventory);
                p_sb.Append(NoSalesInventory);
                for (int i = 0; i < 6; i++)
                {
                    p_sb.Append(espacio);
                }
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
