using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public interface IConfiguracionFiltroAgenda : IEquatable<IConfiguracionFiltroAgenda>
    {
        int IdAgenda { get; set; }
        string Agenda { get; set; }
        bool IniciaActivo { get; set; }
        Color Color { get; set; }

    }
}
