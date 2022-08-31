using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class VehiculoUDOCampana : ILineaUDO
    {
        [UDOBind("U_Unidad")]
        public string  Unidad { get; set; }

        [UDOBind("U_Placa")]
        public string Placa { get; set; }

        [UDOBind("U_Vin")]
        public string  VIN { get; set; }

        [UDOBind("U_Marca")]
        public string  Marca { get; set; }

        [UDOBind("U_Estilo")]
        public string Estilo { get; set; }

        [UDOBind("U_Modelo")]
        public string  Modelo { get; set; }

        [UDOBind("U_Cliente")]
        public string Cliente { get; set; }

        [UDOBind("U_Estado")]
        public string Estado { get; set; }

        [UDOBind("U_Ano")]
        public string Ano { get; set; }

    }
}
