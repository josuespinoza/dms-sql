using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaDesgloceCobroUDOContratoVenta : ILineasUDO
    {
        public ListaDesgloceCobroUDOContratoVenta()
        {
            TablaLigada = "SCGD_LINEASRES";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}