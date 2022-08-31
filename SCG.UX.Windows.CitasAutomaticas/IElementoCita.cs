using System;
using System.Drawing;

namespace SCG.UX.Windows.CitasAutomaticas
{

    public interface IElementoCita
    {

        DateTime? FechaUltimoServicio { get; set; }
        DateTime? FechaProximoServicio { get; set; }
        int? FrecuenciaDias { get; set; }
        object CodigoObjeto { get; set; }
        string Descripcion { get; set; }
        bool ModificadoPorUsuario { get; set; }
        bool EnAgenda { get; set; }
        bool GenerarCita { get; set; }
        bool TodoElDia { get; set; }
        IFiltro Filtro { get; set; }
    }
}
