using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Vehiculos
{
    public class TrazabilizadXVehiculo
    {
        public String Code { get; set; }
        public Int32 LineId { get; set; }
        public Int32? LogInst { get; set; }
        public String U_Cod_Unid { get; set; }
        public String U_NumDoc_I { get; set; }
        public DateTime? U_FhaDoc_I { get; set; }
        public String U_NumCV_I { get; set; }
        public DateTime? U_FhaCV_I { get; set; }
        public String U_CodVen_I { get; set; }
        public Double? U_TotDoc_I { get; set; }
        public String U_Obs_I { get; set; }
        public String U_NumCV_V { get; set; }
        public DateTime? U_FhaCV_V { get; set; }
        public String U_CodCli_V { get; set; }
        public String U_CodVen_V { get; set; }
        public String U_NumFac_V { get; set; }
        public String U_Obs_V { get; set; }
        public Double? U_TotCV_V { get; set; }
        public Double? U_ValVeh { get; set; }
        public DateTime? U_FhaFac_V { get; set; }
        public DateTime? U_FFCom { get; set; }
        public DateTime? U_FGuia { get; set; }
        public String U_NoGuia { get; set; }
        public String U_NumCo { get; set; }
        public DateTime? U_FecEntCV { get; set; }
        public Double? U_Km_Ingreso { get; set; }
        public Double? U_Km_Venta { get; set; } 
    }
}
