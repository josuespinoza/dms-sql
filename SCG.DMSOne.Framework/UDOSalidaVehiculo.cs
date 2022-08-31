using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOSalidaVehiculo : UDO
    {
        public UDOSalidaVehiculo(Company company) : this(company, null)
        {
        }

        public UDOSalidaVehiculo(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_GOODISSUE", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOSalidaVehiculo Encabezado { get; set; }

        public ListaLineasUDOSalidaVehiculo  ListaLineas { get; set; }
    }
}