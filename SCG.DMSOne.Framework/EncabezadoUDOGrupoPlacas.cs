using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOGrupoPlacas : IEncabezadoUDO
    {
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_Fech_G")]
        public DateTime FechaGrupo { get; set; }

        [UDOBind("U_Desc_G")]
        public string DescGrupo { get; set; }

        [UDOBind("U_Total_G")]
        public string TotalGrupo { get; set; }

        public string TablaLigada
        {
            get { return "SCGD_GRUPO_PLACA"; }
        }
    }
}
