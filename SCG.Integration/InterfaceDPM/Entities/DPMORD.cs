using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class DPMORD
    {
        public String FileHeaderID { get; set; }
        public Int32 OrderCoordination { get; set; }

        public String DealerAccount { get; set; }
        public String DBSWarehouse { get; set; }
        public String OrderActivity { get; set; }
        public DateTime  OrderDate { get; set; }
        public DateTime  OrderTime { get; set; }
        public String OrderType { get; set; }
        public Int32 OrderSource { get; set; }
        public String OriginalOrderLineID { get; set; }
        public String PartNumber { get; set; }
        public Double OrderQuantity { get; set; }
        public String OrderReferenceID { get; set; }
    }
}
