using System;

namespace DMS_Connector.Business_Logic.DataContract.Orden_de_Trabajo
{
    public class ControlColaborador
    {
        public string Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32 LogInst { get; set; }
        public string U_Colab { get; set; }
        public double U_TMin { get; set; }
        public string U_RePro { get; set; }
        public string U_NoFas { get; set; }
        public string U_Estad { get; set; }
        public string U_IdAct { get; set; }
        public double U_CosRe { get; set; }
        public double U_CosEst { get; set; }
        public string U_ReAsig { get; set; }
        public string U_HoraIni { get; set; }
        public DateTime? U_FechPro { get; set; }
        public string U_CodFas { get; set; }
        public DateTime? U_DFIni { get; set; }
        public DateTime? U_HFIni { get; set; }
        public DateTime? U_DFFin { get; set; }
        public DateTime? U_HFFin { get; set; }
    }
}