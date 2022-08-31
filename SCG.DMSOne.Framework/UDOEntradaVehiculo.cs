using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOEntradaVehiculo : UDO
    {
        public UDOEntradaVehiculo(Company company)
            : this(company, null)
        {
        }

        public UDOEntradaVehiculo(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_GOODENT", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOEntradaVehiculo Encabezado { get; set; }
        public ListaUDOEntradaVehiculo ListaLineas { get; set; }
    }
}