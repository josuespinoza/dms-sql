using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaUDOTrasladoCostos : ILineasUDO
    {
        public ListaUDOTrasladoCostos()
        {
            TablaLigada = "SCGD_TR_COSTOLINEAS";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}