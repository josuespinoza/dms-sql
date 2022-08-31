using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaLineasUDOSalidaVehiculo : ILineasUDO
    {
        public ListaLineasUDOSalidaVehiculo()
        {
            TablaLigada = "SCGD_GILINES";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}