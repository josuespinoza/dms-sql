using System;
using System.Collections.Generic;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Numeraciones
{
    public class Numeracion
    {
        public Int32 DocEntry { get; set; }
        public Int32? DocNum { get; set; }
        public Int32? Period { get; set; }
        public Int32? Series { get; set; }
        public Int32? LogInst { get; set; }
        public Int32? UserSign { get; set; }
        public String Status { get; set; }
        public String DataSource { get; set; }
        public String U_Objeto { get; set; }
        public List<Numeracion_Lineas> Numeracion_Lineas { get; set; } 
    }
}
