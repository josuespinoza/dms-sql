using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOPrestReversados : UDO
    {

        public UDOPrestReversados(Company company, string udoId) : base(company, udoId)
        {
        }

        public UDOPrestReversados(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOPrestReversados Encabezado { get; set; }

        public ListaPagosReversados ListaPagosReversados { get; set; }

    }
}
