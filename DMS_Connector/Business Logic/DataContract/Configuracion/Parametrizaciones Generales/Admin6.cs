using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Parametrizaciones_Generales
{
    public class Admin6
    {
        public String Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Tipo { get; set; }
        public String U_Cod_Item { get; set; }
        public Int32? U_Serie { get; set; }
        public Int32? U_SerieEx { get; set; } 
    }
}
