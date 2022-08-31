using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOPrestamo : UDO
    {

        public UDOPrestamo(Company company, string udoId) : base(company, udoId)
        {
        }

        public UDOPrestamo(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOPrestamo Encabezado { get; set; }

        public ListaPlanTeoricoUDOPrestamo ListaPlanTeorico { get; set; }

        public ListaPlanRealUDOPrestamo ListaPlanReal { get; set; }

    }
}
