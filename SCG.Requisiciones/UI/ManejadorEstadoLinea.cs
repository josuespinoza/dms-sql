using SAPbobsCOM;

namespace SCG.Requisiciones.UI
{
    public class ManejadorEstadoLinea
    {
        public string ItemCode { get; set; }
        public float CantidadSolicitada { get; set; }
        public float CantidadRecibida { get; set; }
        public EstadosLineas EstadoActual { get; set; }
        public float CantidadAjuste { get; set; }

        public ICompany CompanySBO { get; private set; }

        public ManejadorEstadoLinea(ICompany companySBO)
        {
            CompanySBO = companySBO;
        }

        public void CalculaEstado()
        {
            if (EstadoActual != EstadosLineas.Cancelado)
                EstadoActual = CantidadSolicitada == CantidadRecibida ? EstadosLineas.Trasladado : EstadosLineas.Pendiente;
        }

        
    }
}