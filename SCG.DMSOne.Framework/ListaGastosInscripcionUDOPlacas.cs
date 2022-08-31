using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaGastosInscripcionUDOPlacas : ILineasUDO
    {
        public ListaGastosInscripcionUDOPlacas() 
        {
            TablaLigada = "SCGD_GAS_INS";
        }

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }
    }
}
