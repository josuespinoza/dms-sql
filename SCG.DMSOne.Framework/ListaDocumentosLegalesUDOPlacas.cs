using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class ListaDocumentosLegalesUDOPlacas : ILineasUDO
    {
        public ListaDocumentosLegalesUDOPlacas() 
        {
            TablaLigada = "SCGD_DOC_LEG";
        }

        public List<ILineaUDO> LineasUDO { get; set; }
        public string TablaLigada { get; private set; }
    }
}
