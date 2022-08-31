using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
{
    public class Bodegas_CentroCosto
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_CC { get; set; }
        public String U_Rep { get; set; }
        public String U_Ser { get; set; }
        public String U_Sum { get; set; }
        public String U_SE { get; set; }
        public String U_Pro { get; set; }
        public String U_Res { get; set; }
        public String U_UsaUbic { get; set; }
        public String U_UbiDBP { get; set; } 
    }
}
