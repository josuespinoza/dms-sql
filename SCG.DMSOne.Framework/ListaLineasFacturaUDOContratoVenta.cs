using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaLineasFacturaUDOContratoVenta : ILineasUDO
    {
        public ListaLineasFacturaUDOContratoVenta()
        {
            TablaLigada = "SCGD_LINEASSUM";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}