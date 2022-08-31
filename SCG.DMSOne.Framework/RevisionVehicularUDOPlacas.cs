using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class RevisionVehicularUDOPlacas : ILineaUDO
    {
        [UDOBind("U_Gestion")]
        public string Gestion { get; set; }

        [UDOBind("U_Evento")]
        public string Evento { get; set; }

        [UDOBind("U_Fech_Ev")]
        public DateTime FechaEvento { get; set; }

        [UDOBind("U_Num_Ref1")]
        public string NumeroReferencia1 { get; set; }

        [UDOBind("U_Num_Ref2")]
        public string NumeroReferencia2 { get; set; }

        [UDOBind("U_Num_Ref3")]
        public string NumeroReferencia3 { get; set; }

        [UDOBind("U_Num_Ref4")]
        public string NumeroReferencia4 { get; set; }

        [UDOBind("U_Num_Ref5")]
        public string NumeroReferencia5 { get; set; }

        [UDOBind("U_Num_Ref6")]
        public string NumeroReferencia6 { get; set; }

        [UDOBind("U_Fech_In")]
        public string FechaIngreso { get; set; }

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

        [UDOBind("U_Cod_Ges")]
        public string CodigoGestion { get; set; }

        [UDOBind("U_Cod_Eve")]
        public string CodigoEvento { get; set; }
    }
}
