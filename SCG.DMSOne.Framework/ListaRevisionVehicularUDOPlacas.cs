using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaRevisionVehicularUDOPlacas : ILineasUDO
    {
        public ListaRevisionVehicularUDOPlacas() 
        {
            TablaLigada = "SCGD_REV_VEH";
        }

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }
    }
}
