using System;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public struct IntervaloDias
    {
        public DateTime PrimerDia { get; set; }
        public DateTime UltimoDia { get; set; }

        public IntervaloDias(DateTime primerDia, DateTime ultimoDia) : this()
        {
            PrimerDia = primerDia;
            UltimoDia = ultimoDia;
        }
    }
}