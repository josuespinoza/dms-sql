using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOVehiculos : UDO
    {
        public UDOVehiculos(Company company) : this(company, null)
        {
        }

        public UDOVehiculos(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_VEH", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOVehiculos Encabezado { get; set; }
        public ListaAccesoriosUDOVehiculos ListaAccesorios { get; set; }
        public static ListaTrazabilidadUDOVehiculo ListaTrazabilidad { get; set; }
    }
}