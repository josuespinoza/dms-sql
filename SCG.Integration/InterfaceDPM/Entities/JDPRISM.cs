using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class JDPRISM
    {
        public String FirmCode { get; set; }
        public DateTime DataInitialDate { get; set; }
        public String Path { get; set; }
        public String LoadType { get; set; }
        public String MainAccount { get; set; }
        //public String RecordCode { get; set; }
        //public String PartNumber { get; set; }
        //public String RecordCode { get; set; }
        //public String PartNumber { get; set; }
        public DateTime FileDate { get; set; }
        public List<InfoWarehouse> infoWarehouse { get; set; }
    }
}
