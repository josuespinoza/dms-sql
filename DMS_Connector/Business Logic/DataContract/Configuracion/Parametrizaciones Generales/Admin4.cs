using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Parametrizaciones_Generales
{
    public class Admin4
    {
        public String Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Tipo { get; set; }
        public String U_Transito { get; set; }
        public String U_Stock { get; set; }
        public String U_Costo { get; set; }
        public String U_Ingreso { get; set; }
        public String U_AccXAlm { get; set; }
        public String U_Bod_Tram { get; set; }
        public String U_Bod_Log { get; set; }
        public String U_Devolucion { get; set; } 
    }
}
