using System;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Numeraciones
{
    public class Numeracion_Lineas
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Sucu { get; set; }
        public String U_Ini { get; set; }
        public String U_Fin { get; set; }
        public String U_Sig { get; set; } 
    }
}
