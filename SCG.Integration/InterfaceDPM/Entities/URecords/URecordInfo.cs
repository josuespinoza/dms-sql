using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities.URecords
{
    public class URecordInfo
    {
        public URecordInfo()
        {
        }
        public String ItemCode { get; set; }

        public String RecordCode { get; set; }
        public String MainAccount1_2 { get; set; }
        public String SourceAccount1_2 { get; set; }
        public String MainAccount3_6 { get; set; }
        public String IDRecord { get; set; }
        public String Date { get; set; }
        public String TypeRecord { get; set; }
        public String SourceAccount3_6 { get; set; }
        //*** U0Record
        public Int32 SalesCounter { get; set; }
        public Int32 SalesShop { get; set; }
        public Int32 SalesInternal { get; set; }
        public Int32 ReturnCounter { get; set; }
        public Int32 ReturnShop { get; set; }
        //*** UIRecord
        public Int32 ReturnInternal { get; set; }
        //*** UJRecord
        public Int32 AverageMonthlyInventoryLast12 { get; set; }
        public Int32 AverageMonthlyInventoryLast13to24 { get; set; }
        public Int32 TotalPartsSalesLast12 { get; set; }
        public Int32 TotalPartsSalesLast13to24 { get; set; }
        public Int32 TotalPartsSalesMonth { get; set; }
        //*** UKRecord
        public Int32 TotalPartsCostLast12 { get; set; }
        public Int32 TotalPartsCostLast13to24 { get; set; }
        public Int32 TotalCostMonth { get; set; }
        public Int32 CurrentInventory { get; set; }
        public Int32 NoSalesInventory { get; set; }
        //*** ULRecord
        public Int32 CounterStockedTotalHits { get; set; }
        public Int32 CounterStockedHits1Pass { get; set; }
        public Int32 CounterStockedHitsLostSales { get; set; }
        //*** UMRecord
        public Int32 CounterNonStockedTotalHits { get; set; }
        public Int32 CounterNonStocked1Pass { get; set; }
        public Int32 CounterNonStockedLostSales { get; set; }
        //*** UNRecord
        public Int32 ShopStockedTotalHits { get; set; }
        public Int32 ShopStocked1Pass { get; set; }
        public Int32 ShopStockedLostSales { get; set; }
        //*** UORecord
        public Int32 ShopNonStockedTotalHits { get; set; }
        public Int32 ShopNonStocked1Pass { get; set; }
        public Int32 ShopNonStockedLostSales { get; set; }
        //*** UPRecord
        public Int32 InternalStockedTotalHits { get; set; }
        public Int32 InternalStocked1Pass { get; set; }
        //*** UQRecord
        public Int32 InternalStockedLostSales { get; set; }
        //*** URRecord
        public Int32 InternalNonStockedTotalHits { get; set; }
        public Int32 InternalNonStocked1Pass { get; set; }
        public Int32 InternalNonStockedLostSales { get; set; }
        //*** USRecord
        public Int32 MTDTotalParts { get; set; }
        //*** UTRecord


        public String Warehouse { get; set; }
        public String WarehouseType { get; set; }
    }
}
