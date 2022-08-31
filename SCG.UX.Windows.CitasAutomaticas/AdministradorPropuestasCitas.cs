using System;
using System.Collections.Generic;
using System.Text;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public class AdministradorPropuestasCitas
    {
        public IAdministradorFiltros AdministradorFiltros { get; set; }
        public ICollection<IFiltro> Filtros { get; set; }
        public ICollection<CategoriaFiltro> CategoriasFiltros { get; set; }
        public ICollection<IAgenda> Agendas { get; set; }
        public TimeSpan HoraInicioJornada { get; set; }
        public TimeSpan HoraFinJornada { get; set; }

        public AdministradorPropuestasCitas(IAdministradorFiltros administradorFiltros , ICollection<IFiltro> filtros, ICollection<CategoriaFiltro> categoriasFiltros, ICollection<IAgenda> agendas)
        {
            Agendas = agendas;
            AdministradorFiltros = administradorFiltros;
            Filtros = filtros;
            CategoriasFiltros = categoriasFiltros;
            HoraInicioJornada = new TimeSpan(8,0,0);
            HoraFinJornada = new TimeSpan(17,30,0);
        }
    }
}
