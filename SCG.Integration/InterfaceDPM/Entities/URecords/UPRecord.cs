using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities.URecords
{
    public class UPRecord
    {
        public UPRecord()
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
        public String InternalStockedTotalHits { get; set; }
        public String InternalStocked1Pass { get; set; }
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
                for (int i = 0; i < 40; i++)
                {
                    p_sb.Append(espacio);
                }
                p_sb.Append(InternalStockedTotalHits);
                p_sb.Append(InternalStocked1Pass);
                for (int i = 0; i < 1; i++)
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
