using System;
using System.Collections.Generic;
using SAPbobsCOM;
using SCG.Requisiciones.UI;

namespace SCG.Requisiciones
{
    /// <summary>
    /// Clase abstracta que representa una requisición.
    /// </summary>
    public abstract class Requisicion
    {
        public EncabezadoRequisicion EncabezadoRequisicion { get; set; }
        public List<InformacionLineaRequisicion> LineasRequisicion { get; set; }
        public ICompany Company { get; private set; }
        public string TipoRequisicion { get; set; }
        public string DocumentoGenera { get; set; }
        public string TipoDocumentoMovimiento { get; set; }

        protected Requisicion(ICompany company)
        {
            Company = company;
        }

        public abstract List<TransferenciaLineasBase> Traslada(string p_NoSerieCita = "", string p_NoCita = ""); 
        public abstract int Crea();
    }
}