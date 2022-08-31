using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOSolOTEsp : UDO
    {

        public UDOSolOTEsp(Company company)
            : this(company, null)
        {
        }

        public UDOSolOTEsp(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_SOTESP", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOSolOTEsp Encabezado { get; set; }
        public ListaLineasUDOSolOTEsp ListaLineas { get; set; }
    }
}
