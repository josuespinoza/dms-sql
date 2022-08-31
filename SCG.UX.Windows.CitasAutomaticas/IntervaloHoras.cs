using System;

namespace SCG.UX.Windows.CitasAutomaticas
{
    internal struct IntervaloHoras
    {
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public IntervaloHoras(TimeSpan horaInicio, TimeSpan horaFin) : this()
        {
            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }
    }
}