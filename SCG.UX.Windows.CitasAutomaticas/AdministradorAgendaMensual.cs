using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public class AdministradorAgendaMensual
    {
        /// <summary>
        /// Fecha a partir de la cuál va a generar los días de la semana de la agenda semanal.
        /// </summary>
        public DateTime Fecha { get; set; }
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

        private readonly IList<IntervaloDias> _intervalos = new List<IntervaloDias>(6);

        private string _diasDeLaSemana = "Lunes|Martes|Miércoles|Jueves|Viernes|Sábado|Domingo";

        public DateTime PrimerDiaAgenda { get; set; }
        public DateTime UltimoDiaAgenda { get; set; }

        public void DibujaDiasAgenda(IAgenda agenda)
        {
            CalculaIntervalos();
            _tableLayoutPanel.Visible = false;
            _tableLayoutPanel.Controls.Clear();
            _tableLayoutPanel.ColumnCount = 8;
            _tableLayoutPanel.RowCount = _intervalos.Count + 1;
            _tableLayoutPanel.ColumnStyles.Clear();
            _tableLayoutPanel.RowStyles.Clear();

            for (int i = 1; i < _tableLayoutPanel.RowCount; i++)
            {
                for (int j = 1; j < _tableLayoutPanel.ColumnCount; j++)
                {
                    CitasEnIntervaloMensual citasEnIntervaloMensual;
                    citasEnIntervaloMensual = new CitasEnIntervaloMensual();
                    citasEnIntervaloMensual.Fecha = _intervalos[i - 1].PrimerDia.AddDays(j - 1);
                    citasEnIntervaloMensual.Visible = true;
                    if (EditaElementoCita != null)
                        citasEnIntervaloMensual.EditaElementoCita = EditaElementoCita;
                    _tableLayoutPanel.Controls.Add(citasEnIntervaloMensual, j, i);
                }
            }

            float height = new CitasEnIntervaloMensual().Height;
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute,20));
            for (int i = 1; i < _tableLayoutPanel.RowCount; i++)
            {
                _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            }

            float width = new CitasEnIntervaloMensual().Width;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
            for (int i = 1; i < _tableLayoutPanel.ColumnCount; i++)
            {
                _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, width));
            }

            //Agrega Días de la semana
            for (int i = 1; i < _tableLayoutPanel.ColumnCount; i++)
                _tableLayoutPanel.Controls.Add(ControlDiaDeLaSemana(i - 1), i, 0);

            //Agrega Intervalos de la Agenda
            for (int i = 1; i < _tableLayoutPanel.RowCount; i++)
            {
                _tableLayoutPanel.Controls.Add(ControlIntervaloDia(i - 1), 0, i);
            }

            _tableLayoutPanel.Visible = true;
        }

        public void CargaEnAgenda(IAgenda agenda)
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
            var primeroDelMes = new DateTime(Fecha.Year, Fecha.Month, 1);
            var ultimoDelMes = new DateTime(Fecha.Year, Fecha.Month, DateTime.DaysInMonth(Fecha.Year, Fecha.Month));

            if (elementoCita.FechaProximoServicio >= primeroDelMes && elementoCita.FechaProximoServicio <= ultimoDelMes && elementoCita.GenerarCita)
            {
                int dia = DiaDeLaSemana(elementoCita.FechaProximoServicio.Value.DayOfWeek);
                int intervalo = NoIntervalo(elementoCita.FechaProximoServicio.Value);
                CitasEnIntervaloMensual citasEnIntervaloMensual = (CitasEnIntervaloMensual)_tableLayoutPanel.GetControlFromPosition(dia + 1, intervalo + 1);
                citasEnIntervaloMensual.Agenda = agenda;
                citasEnIntervaloMensual.ElementosCitas.Add(elementoCita);
                citasEnIntervaloMensual.CargaElementosCitas();
                elementoCita.EnAgenda = true;
            }
            else
            {
                elementoCita.EnAgenda = false;
            }
        }

        protected virtual int NoIntervalo(DateTime fecha)
        {
            int i = 0;
            foreach (IntervaloDias intervalo in _intervalos)
            {
                if (fecha >= intervalo.PrimerDia && fecha <= intervalo.UltimoDia) return i;
                i++;
            }
            return _intervalos.Count - 1;
        }


        private Control ControlIntervaloDia(int noIntervalo)
        {
            var label = new VerticalLabel();
            label.Height =80;
            label.Width = 20;
            label.Text = string.Format("{0:dd MMM} - {1:dd MMM}", _intervalos[noIntervalo].PrimerDia, _intervalos[noIntervalo].UltimoDia);
            label.Anchor = AnchorStyles.None;
            return label;
        }

        private void CalculaIntervalos()
        {
            _intervalos.Clear();
            var primeroDelMes = new DateTime(Fecha.Year, Fecha.Month, 1);
            int diaDeLaSemana = DiaDeLaSemana(primeroDelMes.DayOfWeek);
            TimeSpan diasRestar = new TimeSpan(diaDeLaSemana,0,0,0,0);
            PrimerDiaAgenda = primeroDelMes.Subtract(diasRestar);
            UltimoDiaAgenda = primeroDelMes.AddDays(42 - diaDeLaSemana);
            UltimoDiaAgenda = UltimoDiaAgenda.AddSeconds(86399);
            DateTime momento = PrimerDiaAgenda;
            while (momento < UltimoDiaAgenda)
            {
                var item = new IntervaloDias();
                item.PrimerDia = momento;
                momento = momento.AddDays(6);
                item.UltimoDia = momento.AddSeconds(86399);
                momento = momento.AddDays(1);
                _intervalos.Add(item);
            }
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

        private Control ControlDiaDeLaSemana(int dia)
        {
            var label = new Label();
            label.AutoSize = true;
            label.Anchor = AnchorStyles.None;
            label.Text = _diasDeLaSemana.Split('|')[dia];
//            label.BackgroundImage = Image.FromFile(@"D:\Users\rassiel\Desktop\backDia.jpg");
//            label.BackgroundImageLayout = ImageLayout.Stretch;
            label.Dock = DockStyle.Fill;
            label.Margin = new Padding(0);
            label.TextAlign = ContentAlignment.MiddleCenter;
            return label;
        }

        public TableLayoutPanel TableLayoutPanel
        {
            get { return _tableLayoutPanel; }
            set { _tableLayoutPanel = value; }
        }

        public string DiasDeLaSemana
        {
            get { return _diasDeLaSemana; }
            set { _diasDeLaSemana = value; }
        }
    }
}