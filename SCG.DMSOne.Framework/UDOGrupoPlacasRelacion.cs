using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOGrupoPlacasRelacion : UDO
    {
        public UDOGrupoPlacasRelacion(Company company, string udoId)
            : base(company, udoId)
        {
        }

        public UDOGrupoPlacasRelacion(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOGrupoPlacasRelacion Encabezado { get; set; }
    }
}
