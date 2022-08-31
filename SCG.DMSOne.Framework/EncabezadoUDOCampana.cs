using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOCampana : IEncabezadoUDO
    {
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_CampSap")]
        public string CodCampSap { get; set; }

        public string TablaLigada
        {
            get { return "SCGD_CAMPANA"; }
        }
    }
}
