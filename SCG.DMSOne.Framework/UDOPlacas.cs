using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOPlacas : UDO  
    {
        public UDOPlacas(Company company, string udoId) : base(company, udoId)
        {
        }

        public UDOPlacas(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOPlacas encabezado { get; set; }

        public ListaRevisionVehicularUDOPlacas ListaRevisionVehicular { get; set; }

        public ListaDocumentosLegalesUDOPlacas ListaDocumentosLegales { get; set; }

        public ListaInscripcionUDOPlacas ListaInscripcion { get; set; }

        public ListaGastosInscripcionUDOPlacas ListaGastosInscripcion { get; set; }

    }
}
