using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaTrazabilidadUDOVehiculo : ILineasUDO
    {

        public ListaTrazabilidadUDOVehiculo()
        {
            TablaLigada = "SCGD_VEHITRAZA";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion

    }
}
