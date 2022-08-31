using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework.UDOOrden
{
    public class ControlColaboradorUDOOrden
    {
        public ControlColaboradorUDOOrden()
        {
            TablaLigada = "SCGD_CTRLCOL";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}
