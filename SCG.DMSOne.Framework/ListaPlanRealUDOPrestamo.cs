using System;
using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaPlanRealUDOPrestamo : ILineasUDO
    {
        public ListaPlanRealUDOPrestamo()
        {
            TablaLigada = "SCGD_PLAN_REAL";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion
    }
}
