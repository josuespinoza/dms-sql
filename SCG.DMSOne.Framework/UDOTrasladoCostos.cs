using SAPbobsCOM;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class UDOTrasladoCostos : UDO
    {
        public UDOTrasladoCostos(Company company) 
            : this(company, null )
        {
        }

        public UDOTrasladoCostos(Company company, GetAutoKeyMethod getAutoKeyMethod)
            : base(company, "SCGD_TRCU", getAutoKeyMethod)
        {
        }

        public EncabezadoUDOTrasladoCostos Encabezado { get; set; }

        public ListaUDOTrasladoCostos  ListaLineas { get; set; }
    }
}