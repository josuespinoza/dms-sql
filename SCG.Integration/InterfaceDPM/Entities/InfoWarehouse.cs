using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class InfoWarehouse
    {
        public String ItemCode { get; set; }
        public String WhsCode { get; set; }
        public Double OnHand { get; set; }
        public Double IsCommited { get; set; }
        public Double OnOrder { get; set; }
        public Double Available { get; set; }
        public String WhsProcess { get; set; }
    }
}
