using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
{
    public class Aprobaciones_Sucursal
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_TipoOT { get; set; }
        public String U_ItmAprob { get; set; }
        public String U_EspAprob { get; set; }
    }
}
