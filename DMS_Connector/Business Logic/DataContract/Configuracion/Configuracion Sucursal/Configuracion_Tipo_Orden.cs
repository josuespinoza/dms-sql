using System;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
{
    public class Configuracion_Tipo_Orden
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32? VisOrder { get; set; }
        public Int32? LogInst { get; set; }
        public Int32? U_Code { get; set; }
        public String U_Name { get; set; }
        public String U_UsaDim { get; set; }
        public String U_Interna { get; set; }
        public String U_UsDmAEM { get; set; }
        public String U_UsDmAFP { get; set; }
        public String U_CodCtCos { get; set; }
        public String U_CodClien { get; set; }
        public String U_UsaLstPre { get; set; }
        public String U_UsaDOFV { get; set; }
    }
}
