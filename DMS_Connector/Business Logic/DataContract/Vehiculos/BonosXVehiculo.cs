using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Vehiculos
{
    public class BonosXVehiculo
    {
        public String Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Bono { get; set; }
        public Double? U_Monto { get; set; }
    }
}
