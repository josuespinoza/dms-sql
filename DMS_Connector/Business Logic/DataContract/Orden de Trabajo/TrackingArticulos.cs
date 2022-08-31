using System;

namespace DMS_Connector.Business_Logic.DataContract.Orden_de_Trabajo
{
    public class TrackingArticulos
    {
        public string Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32 LogInst { get; set; }
        public string U_NoOrden { get; set; }
        public string U_ItemCode { get; set; }
        public string U_ID { get; set; }
        public DateTime? U_FechaSol { get; set; }
        public DateTime? U_FechaCom { get; set; }
        public DateTime? U_FechaEnt { get; set; }
        public string U_CardCode { get; set; }
        public string U_CardName { get; set; }
        public Int32 U_DocEntry { get; set; }
        public Int32 U_DocNum { get; set; }
        public string U_Descripcion { get; set; }
        public string U_Observ { get; set; }
        public double U_CanSol { get; set; }
        public double U_CanRec { get; set; }
        public DateTime? U_FechaDoc { get; set; }
        public Int32 U_TipoDoc { get; set; }
    }
}
