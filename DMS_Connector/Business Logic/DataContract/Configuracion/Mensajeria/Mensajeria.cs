using System;
using System.Collections.Generic;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria
{
    public class Mensajeria
    {
        public Int32 DocEntry { get; set; }
        public Int32? DocNum { get; set; }
        public Int32? Period { get; set; }
        public Int32? Series { get; set; }
        public String Handwrtten { get; set; }
        public String Canceled { get; set; }
        public Int32? LogInst { get; set; }
        public Int32? UserSign { get; set; }
        public String Transfered { get; set; }
        public String Status { get; set; }
        public String DataSource { get; set; }
        public String U_IdSuc { get; set; }
        public String U_IdRol { get; set; }
        public List<Mensajeria_Lineas> Mensajeria_Lineas { get; set; }
    }
}
