using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal
{
    /// <summary>
    /// Objeto que representa el horario de una sucursal
    /// </summary>
    public class Horario
    {
        public int MinutosAlmuerzo { get; set; }
        public DateTime HoraInicioAlmuerzo { get; set; }
        public DateTime HoraFinAlmuerzo { get; set; }
        public DateTime HoraApertura { get; set; }
        public DateTime HoraCierre { get; set; }
        public bool HorarioConfigurado { get; set; }

        /// <summary>
        /// Constructor del objeto horario
        /// </summary>
        /// <param name="HoraApertura">Hora de apertura de la sucursal</param>
        /// <param name="HoraCierre">Hora de cierre de la sucursal</param>
        /// <param name="HoraInicioAlmuerzo">Hora de inicio del almuerzo</param>
        /// <param name="HoraFinAlmuerzo">Hora de fin del almuerzo</param>
        public Horario(DateTime HoraApertura, DateTime HoraCierre, DateTime HoraInicioAlmuerzo, DateTime HoraFinAlmuerzo)
        {
            //Se utiliza la fecha mínima, ya que esta es diferente entre los objetos COM y .NET ocasionando
            //que no se puedan verificar de la manera tradicional con DateTime.MinValue
            DateTime FechaMinima;
            TimeSpan EspacioTiempo;
            try
            {
                FechaMinima = new DateTime(1899, 12, 30);
                this.HoraApertura = HoraApertura;
                this.HoraCierre = HoraCierre;
                this.HoraInicioAlmuerzo = HoraInicioAlmuerzo;
                this.HoraFinAlmuerzo = HoraFinAlmuerzo;
                if (!(HoraInicioAlmuerzo == FechaMinima) && !(HoraFinAlmuerzo == FechaMinima))
                {
                    EspacioTiempo = HoraFinAlmuerzo - HoraInicioAlmuerzo;
                    this.MinutosAlmuerzo = Convert.ToInt32(EspacioTiempo.TotalMinutes);
                }

                if (!(HoraApertura == FechaMinima) && !(HoraCierre == FechaMinima))
                {
                    HorarioConfigurado = true;
                }
                else
                {
                    HorarioConfigurado = false;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    }
}
