using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaAccesoriosUDOVehiculos : ILineasUDO
    {
        public ListaAccesoriosUDOVehiculos()
        {
            TablaLigada = "SCGD_ACCXVEH";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}