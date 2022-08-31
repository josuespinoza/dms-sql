using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaInscripcionUDOPlacas : ILineasUDO
    {
        public ListaInscripcionUDOPlacas() 
        {
            TablaLigada = "SCGD_INSCRIP";
        }

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }
    }
}
