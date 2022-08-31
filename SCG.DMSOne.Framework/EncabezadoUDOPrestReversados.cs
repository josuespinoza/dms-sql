using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOPrestReversados : IEncabezadoUDO
    {

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_Prestamo")]
        public string Prestamo { get; set; }

        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_PREST_REV"; }
        }

        #endregion

    }
}
