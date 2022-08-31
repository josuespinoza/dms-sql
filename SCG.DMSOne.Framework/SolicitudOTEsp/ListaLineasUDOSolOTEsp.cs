using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaLineasUDOSolOTEsp: ILineasUDO
    {
        public ListaLineasUDOSolOTEsp()
        {
            TablaLigada = "SCGD_LINEAS_SOT_ESP";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}
