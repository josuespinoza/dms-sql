using System.Collections.Generic;

namespace SCG.Requisiciones.UI
{
    public abstract class TransferenciaLineasBase
    {
        public List<InformacionLineaRequisicion> InformacionLineasRequisicion { get; set; }
        public EncabezadoRequisicion EncabezadoRequisicion { get; set; }
        public string Error { get; set; }

        public virtual bool HuboError
        {
            get { return !string.IsNullOrEmpty(Error); }
        }

        public abstract void CopyToInformacionLineasMovimientos(InformacionLineasMovimientos lineasMovimientos);

    }
}