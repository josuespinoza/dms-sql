using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOContratoVenta : UDO
    {
        public UDOContratoVenta(Company company, string udoId) : base(company, udoId)
        {
        }

        public UDOContratoVenta(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, udoId, getAutoKeyMethod)
        {
        }

        public EncabezadoUDOContratoVenta Encabezado { get; set; }

        /// <summary>
        /// @SCG_ACCXCONT
        /// </summary>
        public ListaAccesoriosUDOContratoVenta ListaAccesorios { get; set; }

        /// <summary>
        /// @SCG_LINEASRES
        /// </summary>
        public ListaDesgloceCobroUDOContratoVenta ListaDesgloceCobro { get; set; }

        /// <summary>
        /// @SCG_LINEASSUM
        /// </summary>
        public ListaLineasFacturaUDOContratoVenta ListaLineasFactura { get; set; }
    }
}