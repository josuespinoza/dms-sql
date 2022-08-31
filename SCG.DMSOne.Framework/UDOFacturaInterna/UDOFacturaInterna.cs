using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOFacturaInterna : UDO
    {
        public UDOFacturaInterna(Company company) : this(company, null)
        {
        }

        public UDOFacturaInterna(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_FAC_INT", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOFacturaInterna Encabezado { get; set; }
    }
}
