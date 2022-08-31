using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class OtrosCostosXContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_CodCos { get; set; }
        public Double? U_Monto { get; set; }
        public String U_Unidad { get; set; } 
    }
}
