using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOGrupoPlacas : UDO
    {
        public UDOGrupoPlacas(Company company, string udoId)
            : base(company, udoId)
        {
        }

        public UDOGrupoPlacas(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOGrupoPlacas Encabezado { get; set; }
    }
}
