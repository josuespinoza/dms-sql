using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas
{
    public class HistorialContrato
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Usuario { get; set; }
        public DateTime? U_Hora { get; set; }
        public String U_Comentario { get; set; }
        public String U_Nivel { get; set; }
        public String U_Niv_Code { get; set; }
        public DateTime? U_Fecha { get; set; } 
    }
}
