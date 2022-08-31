using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class GastosInscripcionUDOPlacas : ILineaUDO
    {
        [UDOBind("U_Gasto")]
        public string Gasto { get; set; }

        [UDOBind("U_Num_Doc")]
        public string NumeroDocumento { get; set; }

        [UDOBind("U_Fech_Doc")]
        public DateTime FechaDocumento { get; set; }

        [UDOBind("U_Monto")]
        public string Monto { get; set; }

        [UDOBind("U_Observ")]
        public string Observacion { get; set; }

        [UDOBind("U_Ingresa")]
        public string UsuarioIngresa { get; set; }

        [UDOBind("U_Modific")]
        public string UsuarioModifica { get; set; }

        [UDOBind("U_Fech_Mod")]
        public DateTime FechaModificacion { get; set; }

        [UDOBind("U_Fech_Cre")]
        public DateTime FechaCreacion { get; set; }

        [UDOBind("U_Cod_Gas")]
        public string CodigoGasto { get; set; }
        
    }
}
