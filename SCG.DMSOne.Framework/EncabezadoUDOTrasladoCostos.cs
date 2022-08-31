using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOTrasladoCostos : IEncabezadoUDO
    {
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_SCGD_Fec")]
        public DateTime Fecha { get; set; }

        [UDOBind("U_SCGD_Io")]
        public string InventarioOrig { get; set; }

        [UDOBind("U_SCGD_TYN")]
        public string  TransferidoSiNo { get; set; }

        //[UDOBind("U_Estado")]
        //public int EstadoContrato { get; set; }

        //[UDOBind("U_Opcion")]
        //public string OpcionesContrato { get; set; }
          
   
        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_TR_COSTOS"; }
        }

        #endregion
    }
}