using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class PMM
    {
        public String FirmCode { get; set; }
        public String U_FirstMainAcc { get; set; }
        public String U_FirstSourceAcc { get; set; }
        public String U_LastMainAcc { get; set; }
        public String U_LastSourceAcc { get; set; }
        public String U_PMMVer { get; set; }
        public String U_CriticalCode { get; set; }
        public String U_InvenClass { get; set; }
        public String Path { get; set; }
        public List<InfoWarehouse> infoWarehouse { get; set; }
    }
}
