using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public interface IFiltro
    {
        string Descripcion { get; set; }
        string Filtro { get; set; }
        int CodigoCategoriaFiltro { get; set; }
        string Condicion { get; set; }
        bool Activo { get; set; }
        IDictionary<int,IConfiguracionFiltroAgenda> ConfiguracionesPorAgenda { get; }
    }
}
