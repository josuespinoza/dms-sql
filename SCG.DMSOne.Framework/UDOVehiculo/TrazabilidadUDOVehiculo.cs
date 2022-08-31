using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class TrazabilidadUDOVehiculo : ILineaUDO
    {

        //[UDOBind("U_ValVeh")]
        //public float CostoVeh { get; set; }

        [UDOBind("U_NumDoc_I")]
        public string NotaCred { get; set; }

        [UDOBind("U_FhaDoc_I")]
        public DateTime FechaNC { get; set; }

        [UDOBind("U_NumCV_I")]
        public string ContIng { get; set; }

        [UDOBind("U_FhaCV_I")]
        public DateTime FechaCVIng { get; set; }

        [UDOBind("U_CodVen_I")]
        public string VendIng { get; set; }

        [UDOBind("U_TotDoc_I")]
        public double ValorRecibo { get; set; }

        [UDOBind("U_Obs_I")]
        public string ObsIng { get; set; }

        [UDOBind("U_ValVeh")]
        public double ValorVeh { get; set; }

        [UDOBind("U_ValVehS")]
        public double ValorVehS { get; set; }

        [UDOBind("U_NumCV_V")]
        public string ContVta { get; set; }

        [UDOBind("U_FhaCV_V")]
        public DateTime FechaCVVta { get; set; }

        [UDOBind("U_NumFac_V")]
        public string Factura { get; set; }

        [UDOBind("U_FhaFac_V")]
        public DateTime FechaFact { get; set; }

        [UDOBind("U_CodVen_V")]
        public string VendVta { get; set; }

        [UDOBind("U_TotCV_V")]
        public double PrecioVta { get; set; }

        [UDOBind("U_FecEntCV")]
        public DateTime FechaEntrega { get; set; }

        [UDOBind("U_Km_Ingreso")]
        public int KmIngreso { get; set; }

        [UDOBind("U_Km_Venta")]
        public int KmVenta { get; set; }

        //[UDOBind("U_Obs_V")]
        //public string ObsVta { get; set; }

    }
}
