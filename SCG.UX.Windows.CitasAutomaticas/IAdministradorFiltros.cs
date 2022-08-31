using System;
using System.Collections.Generic;
using System.Text;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public interface IAdministradorFiltros
    {
        IEnumerable<IElementoCita> ElementosCitas();
        IEnumerable<IElementoCita> ElementosCitas(IEnumerable<IFiltro> filtros);
    }
}
