using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOGrupoPlacasRelacion : IEncabezadoUDO
    {
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_Num_Grupo")]
        public int NumeroGrupo { get; set; }

        [UDOBind("U_Num_Unid")]
        public string Unidad { get; set; }

        [UDOBind("U_Num_Exp")]
        public string NumeroExpediente { get; set; }

        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_GRUPO_REL"; }
        }

        #endregion
    }
}
