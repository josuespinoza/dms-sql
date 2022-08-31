using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;
using SAPbobsCOM;

namespace SCG.DMSOne.Framework
{
    public class UDOCampana : UDO
    {
         public UDOCampana(Company company, string udoId) : base(company, udoId)
        {
        }

         public UDOCampana(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOCampana Encabezado { get; set; }

        public ListaVehiculosUDOCampana ListaVehiculos { get; set; }
    }
}
