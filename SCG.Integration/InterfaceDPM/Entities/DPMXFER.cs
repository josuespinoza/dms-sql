using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class DPMXFER
    {
        public String FileHeaderID { get; set; }
        public Int32 TransferCoordination { get; set; }

        public String PartNumber { get; set; }
        public Double TransferQuantity { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime TransferTime { get; set; }
        public String FromDealerAccount { get; set; }
        public String FromWarehouse { get; set; }
        public String ToDealerAccount { get; set; }
        public String ToWarehouse { get; set; }
    }
}
