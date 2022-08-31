using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
{
    public class Configuracion_OT_Interna
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Tipo_OT { get; set; }
        public String U_Tran_Com { get; set; }
        public String U_NumCuent { get; set; } 
    }
}
