using System;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Dimensiones
{
    public class DimensionesOT_Lineas
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Dim1 { get; set; }
        public String U_Dim2 { get; set; }
        public String U_Dim3 { get; set; }
        public String U_Dim4 { get; set; }
        public String U_Dim5 { get; set; }
        public String U_CodMar { get; set; }
        public String U_DesMar { get; set; } 
    }
}
