using System.Collections.Generic;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaPlanTeoricoUDOPrestamo : ILineasUDO
    {

        public ListaPlanTeoricoUDOPrestamo()
        {
            TablaLigada = "SCGD_PLAN_TEORICO";
        }

        #region ILineasUDO Members

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }

        #endregion

    }
}
