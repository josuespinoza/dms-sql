using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework.UDOOrden
{
    public class UDOOrden : UDO
    {
         public UDOOrden(Company company) : this(company, null)
        {
        }

         public UDOOrden(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_OT", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOOrden Encabezado { get; set; }
        public ControlColaboradorUDOOrden ListaControlColaborador { get; set; }
    }
}
