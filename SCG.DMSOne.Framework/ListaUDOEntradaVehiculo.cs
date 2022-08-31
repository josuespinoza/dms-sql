using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaUDOEntradaVehiculo : ILineasUDO
    {
        public ListaUDOEntradaVehiculo()
        {
            TablaLigada = "SCGD_GRLINES";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}