using System;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria
{
    public class Mensajeria_Lineas
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_IDRol { get; set; }
        public String U_IDUSR { get; set; }
        public String U_Usr_Name { get; set; }
        public String U_EmpCode { get; set; }
        public String U_Usr_UsrName { get; set; } 
    }
}
