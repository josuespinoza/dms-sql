using System;
using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaPagosReversados : ILineasUDO
    {

        public ListaPagosReversados()
        {
            TablaLigada = "SCGD_PAGOS_REV";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion

    }
}
