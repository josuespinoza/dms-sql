using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaAccesoriosUDOContratoVenta : ILineasUDO
    {
        public ListaAccesoriosUDOContratoVenta()
        {
            TablaLigada = "SCGD_ACCXCONT";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}