using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    class AdministradorAgendaSemanal
    {
        private const int OffSetFilaCitaTodoElDia = 1;
        private const int OffSetFilaDiasDeLaSemana = 1;
        private const int OffSetColumnaHoras = 1;

        /// <summary>
        /// Fecha a partir de la cuál va a generar los días de la semana de la agenda semanal.
        /// </summary>
        private DateTime _fecha;
        /// <summary>
        /// Panel que va a utilizar para dibujar la agenda semanal
        /// </summary>
        private TableLayoutPanel _tableLayoutPanel;
        /// <summary>
        /// Lista de elementos a los cuales se les va a proponer fecha de próximo servicio
        /// </summary>
        public IEnumerable<IElementoCita> ElementosCitas { get; set; }
        /// <summary>
        /// Hora de inicio de la jornada laboral
        /// </summary>
        public TimeSpan HoraInicioJornada { get; set; }
        /// <summary>
        /// Hora de finalización de la jornada laboral
        /// </summary>
        public TimeSpan HoraFinJornada { get; set; }

        public ManipuladorEditarElementoCita EditaElementoCita { get; set; }
        private IList<IntervaloHoras> _intervalos;

        private string _diasDeLaSemana = "Lunes|Martes|Miércoles|Jueves|Viernes|Sábado|Domingo";
        private int _diaDeLaSemanaActual;

        private Control ControlHoraDelDia(int noIntervalo)
        {
            var label = new Label();

            var fechaIncioIntervalo = new DateTime(_intervalos[noIntervalo].HoraInicio.Ticks);
            var fechaFinIntervalo = new DateTime(_intervalos[noIntervalo].HoraFin.Ticks);

            label.AutoSize = true;
            label.Anchor = AnchorStyles.None;
            label.Text = string.Format("{0:hh:mm tt}\n{1:hh:mm tt}", fechaIncioIntervalo, fechaFinIntervalo);
            return label;
        }

        private Control ControlDiaDeLaSemana(int dia)
        {
            var label = new Label();
            label.AutoSize = true;
            label.Anchor = AnchorStyles.None;
            label.Text = _diasDeLaSemana.Split('|')[dia];
            if (
                (_fecha.DayOfWeek == DayOfWeek.Monday && dia == 0)
                || (_fecha.DayOfWeek == DayOfWeek.Tuesday && dia == 1)
                || (_fecha.DayOfWeek == DayOfWeek.Wednesday && dia == 2)
                || (_fecha.DayOfWeek == DayOfWeek.Thursday && dia == 3)
                || (_fecha.DayOfWeek == DayOfWeek.Friday && dia == 4)
                || (_fecha.DayOfWeek == DayOfWeek.Saturday && dia == 5)
                || (_fecha.DayOfWeek == DayOfWeek.Sunday && dia == 6)
                )
            {
                label.BackColor = Color.Yellow;
                _diaDeLaSemanaActual = dia;
            }
            return label;
        }

        public void DibujaDiasHorasAgenda(IAgenda agenda)
        {
            _intervalos = CalculaIntervalos(agenda);
            _tableLayoutPanel.Visible = false;
            _tableLayoutPanel.Controls.Clear();
            _tableLayoutPanel.ColumnCount = 8;
            _tableLayoutPanel.RowCount = _intervalos.Count + OffSetFilaDiasDeLaSemana + OffSetFilaCitaTodoElDia;
            _tableLayoutPanel.ColumnStyles.Clear();
            _tableLayoutPanel.RowStyles.Clear();

            for (int i = 0 + OffSetFilaDiasDeLaSemana; i < _tableLayoutPanel.RowCount; i++)
            {
                for (int j = 0 + OffSetColumnaHoras; j < _tableLayoutPanel.ColumnCount; j++)
                {
                    CitasEnIntervaloSemanal enIntervaloSemanal;
                    enIntervaloSemanal = new CitasEnIntervaloSemanal();
                    enIntervaloSemanal.Visible = true;
                    if (EditaElementoCita != null)
                        enIntervaloSemanal.EditaElementoCita = EditaElementoCita;
                    _tableLayoutPanel.Controls.Add(enIntervaloSemanal, j, i);
                    
                }
            }
            float width = new CitasEnIntervaloSemanal().Width;
            for (int i = 0; i < _tableLayoutPanel.ColumnCount; i++)
            {
                _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, width));
            }

            _tableLayoutPanel.ColumnStyles[0].SizeType = SizeType.AutoSize;
            _tableLayoutPanel.ColumnStyles[0].Width = 8;

            float height = new CitasEnIntervaloSemanal().Height;
            for (int i = 0; i < _tableLayoutPanel.RowCount; i++)
            {
                _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            }
            _tableLayoutPanel.RowStyles[0].SizeType = SizeType.AutoSize;

            //Agrega Días de la semana
            for (int i = 0 + OffSetColumnaHoras; i < _tableLayoutPanel.ColumnCount; i++)
            {
                _tableLayoutPanel.Controls.Add(ControlDiaDeLaSemana(i - 1), i, 0);
            }
            //Agrega Intervalos de la Agenda
            for (int i = 0 + OffSetFilaDiasDeLaSemana + OffSetFilaCitaTodoElDia; i < _tableLayoutPanel.RowCount; i++)
            {
                _tableLayoutPanel.Controls.Add(ControlHoraDelDia(i - OffSetFilaDiasDeLaSemana - OffSetFilaCitaTodoElDia), 0, i);
            }
            _tableLayoutPanel.Visible = true;
        }


        private IList<IntervaloHoras> CalculaIntervalos(IAgenda agenda)
        {
            TimeSpan momento = HoraInicioJornada;
            var intervalo = new TimeSpan(0, agenda.Intervalo, 0);
            var intervalos = new List<IntervaloHoras>();
            while (momento < HoraFinJornada)
            {
                var item = new IntervaloHoras {HoraInicio = momento};
                momento = momento.Add(intervalo);
                TimeSpan finIntervalo = momento.Subtract(new TimeSpan(0, 1, 0));
                item.HoraFin = HoraFinJornada.Ticks < finIntervalo.Ticks ? HoraFinJornada : finIntervalo;
                intervalos.Add(item);
            }
            return intervalos;
        }

        protected virtual int NoIntervalo(DateTime fecha)
        {
            if (fecha.TimeOfDay < HoraInicioJornada) return 0;
            int i = 0;
            foreach (IntervaloHoras intervalo in _intervalos)
            {
                if (fecha.TimeOfDay >= intervalo.HoraInicio && fecha.TimeOfDay <= intervalo.HoraFin) return i + OffSetFilaCitaTodoElDia;
                i++;
            }
            return _intervalos.Count - 1 + OffSetFilaCitaTodoElDia;
        }

        /// <summary>
        /// Devuelve el no de la columna según el día de la semana.
        /// Para agenda semanal.
        /// </summary>
        /// <param name="dayOfWeek">Día de la semana</param>
        /// <returns></returns>
        protected virtual int DiaDeLaSemana(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                //case DayOfWeek.Sunday:
                default:
                    return 6;
            }
        }


        public virtual void CargaEnAgenda(IAgenda agenda)
        {
            _tableLayoutPanel.Visible = false;
            foreach (IElementoCita elementoCita in ElementosCitas)
            {
                CargaEnAgenda(elementoCita, agenda);
            }
            _tableLayoutPanel.Visible = true;
        }

        public void CargaEnAgenda(IElementoCita elementoCita, IAgenda agenda)
        {
            DateTime primerDia = _fecha.AddDays(- DiaDeLaSemanaActual);
            DateTime ultimoDia = _fecha.AddDays(6 - DiaDeLaSemanaActual);
            if (elementoCita.FechaProximoServicio >= primerDia && elementoCita.FechaProximoServicio <= ultimoDia && elementoCita.GenerarCita)
            {
                int dia = DiaDeLaSemana(elementoCita.FechaProximoServicio.Value.DayOfWeek);
                int intervalo;
                intervalo = elementoCita.TodoElDia ? 0 : NoIntervalo(elementoCita.FechaProximoServicio.Value);
                CitasEnIntervaloSemanal enIntervaloSemanal = (CitasEnIntervaloSemanal)_tableLayoutPanel.GetControlFromPosition(dia + 1, intervalo + 1);
                enIntervaloSemanal.Agenda = agenda;
                enIntervaloSemanal.ElementosCitas.Add(elementoCita);
                enIntervaloSemanal.CargaElementosCitas();
                elementoCita.EnAgenda = true;
            }
            else
            {
                elementoCita.EnAgenda = false;
            }
        }

        public string DiasDeLaSemana
        {
            get { return _diasDeLaSemana; }
            set { _diasDeLaSemana = value; }
        }

        public int DiaDeLaSemanaActual
        {
            get { return _diaDeLaSemanaActual; }
        }

        public TableLayoutPanel TableLayoutPanel
        {
            set { _tableLayoutPanel = value; }
        }

        public DateTime Fecha
        {
            set { _fecha = value ; }
        }
    }
}
