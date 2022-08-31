using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.ServicioPostVenta.CalculoCostos
{
    /// <summary>
    /// Clase encargada de operaciones de costeo de servicios
    /// </summary>
    public static class CostoManoObra
    {

        public enum TrabajaFinSemana
        {
            Si,
            No
        }

        /// <summary>
        /// Calcula el costo estándar y real de un determinado servicio ligado a una orden de trabajo
        /// tomando en cuenta el horario de la sucursal, feriados, horas extra. Cálculo complejo.
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <param name="FechaHoraInicio">Fecha de inicio del servicio incluida la hora</param>
        /// <param name="FechaHoraFinal">Fecha de fin del servicio incluida la hora</param>
        /// <param name="DuracionEstandar">Duración estándar del servicio en minutos sin decimales</param>
        /// <param name="SalarioPorHora">Salario por hora del empleado que realiza el servicio</param>
        /// <param name="TarifaHorasExtra">Tarifa o multiplicador para horas extra</param>
        /// <param name="CostoEstandar">Costo estándar de la actividad para la línea</param>
        /// <param name="CostoReal">Costo real de la actividad para la línea</param>
        /// <param name="CantidadHorasEstandar">Cantidad de horas estandar procesadas por el servicio</param>
        /// <param name="CantidadHorasExtra">Cantidad de horas extras procesadas por el servicio</param>
        public static void CalcularCostoCompuesto(string CodigoSucursal, DateTime FechaHoraInicio, DateTime FechaHoraFinal, int DuracionEstandar, double SalarioPorHora, double TarifaHorasExtra, ref double CostoEstandar, ref double CostoReal, ref double CantidadHorasEstandar, ref double CantidadHorasExtra, TrabajaFinSemana TrabajaFinSemana)
        {
            try
            {
                //Limpiamos las variables para eliminar los registros anteriores
                CantidadHorasEstandar = 0;
                CantidadHorasExtra = 0;
                //Obtiene la cantidad de horas de cada tipo ya sean estándar o extras
                ObtenerHorasLaborables(CodigoSucursal, FechaHoraInicio, FechaHoraFinal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                //Realiza el cálculo de los costos de acuerdo a la cantidad y tipo de horas
                CostoReal = (CantidadHorasEstandar * SalarioPorHora) + ((CantidadHorasExtra * SalarioPorHora) * TarifaHorasExtra);
                CostoEstandar = (DuracionEstandar / 60.00) * SalarioPorHora;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calcula el costo estándar y real de un determinado servicio ligado a una orden de trabajo
        /// sin tomar en cuenta el horario, ni días feriados, ni horas extras. Cálculo directo.
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <param name="FechaHoraInicio">Fecha de inicio del servicio incluida la hora</param>
        /// <param name="FechaHoraFinal">Fecha de fin del servicio incluida la hora</param>
        /// <param name="DuracionEstandar">Duración estándar del servicio en minutos sin decimales</param>
        /// <param name="SalarioPorHora">Salario por hora del empleado que realiza el servicio</param>
        /// <param name="TarifaHorasExtra">Tarifa o multiplicador para horas extra</param>
        /// <param name="CostoEstandar">Costo estándar de la actividad para la línea</param>
        /// <param name="CostoReal">Costo real de la actividad para la línea</param>
        /// <param name="CantidadHorasEstandar">Cantidad de horas estandar procesadas por el servicio</param>
        /// <param name="CantidadHorasExtra">Cantidad de horas extras procesadas por el servicio</param>
        public static void CalcularCostoSimple(string CodigoSucursal, DateTime FechaHoraInicio, DateTime FechaHoraFinal, int DuracionEstandar, double SalarioPorHora, double TarifaHorasExtra, ref double CostoEstandar, ref double CostoReal, ref double CantidadHorasEstandar, ref double CantidadHorasExtra)
        {
            TimeSpan DiferenciaTiempo;
            try
            {
                //Limpiamos las variables para eliminar los registros anteriores
                CantidadHorasEstandar = 0;
                CantidadHorasExtra = 0;

                //Verifica que las fechas estén en el orden correcto, de lo contrario invierte el orden
                ReorganizarFechas(ref FechaHoraInicio, ref FechaHoraFinal);

                DiferenciaTiempo = FechaHoraFinal - FechaHoraInicio;
                CantidadHorasEstandar = Math.Abs(DiferenciaTiempo.TotalHours);

                //Realiza el cálculo de los costos de acuerdo a la cantidad y tipo de horas
                CostoReal = (CantidadHorasEstandar * SalarioPorHora) + ((CantidadHorasExtra * SalarioPorHora) * TarifaHorasExtra);
                CostoEstandar = (DuracionEstandar / 60.00) * SalarioPorHora;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }


        /// <summary>
        /// Obtiene la cantidad de horas de cada tipo ya sean estándar o extras
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <param name="FechaHoraInicio">Fecha de inicio del servicio incluida la hora</param>
        /// <param name="FechaHoraFinalizacion">Fecha de fin del servicio incluida la hora</param>
        /// <param name="CantidadHorasEstandar">Cantidad de horas estandar procesadas por el servicio</param>
        /// <param name="CantidadHorasExtra">Cantidad de horas extras procesadas por el servicio</param>
        public static void ObtenerHorasLaborables(string CodigoSucursal, DateTime FechaHoraInicio, DateTime FechaHoraFinalizacion, ref double CantidadHorasEstandar, ref double CantidadHorasExtra, TrabajaFinSemana TrabajaFinSemana)
        {
            List<DateTime> ListaFeriados;
            DateTime HoraApertura;
            DateTime HoraCierre;
            DateTime FechaAjustada;
            DayOfWeek DiaSeleccionado;
            bool ProcesarSiguienteDia = false;
            try
            {
                //Verifica que exista al menos un horario configurado
                if (HorariosConfigurados(CodigoSucursal))
                {
                    //Verifica que las fechas estén en el orden correcto, de lo contrario revierte el orden
                    ReorganizarFechas(ref FechaHoraInicio, ref FechaHoraFinalizacion);
                    //Obtiene el listado de días feriados configurados
                    ListaFeriados = DMS_Connector.Configuracion.ObtenerFeriados();
                    FechaAjustada = new DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraInicio.Hour, FechaHoraInicio.Minute, FechaHoraInicio.Second);
                    do
                    {
                        DiaSeleccionado = FechaAjustada.DayOfWeek;
                        //Si el día de la semana tiene el horario configurado y no es un día feriado, se procesan las horas
                        if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).HorarioSucursal[DiaSeleccionado].HorarioConfigurado && (!ListaFeriados.Contains(FechaAjustada.Date)))
                        {
                            //Obtiene los horarios de apertura y cierre del día
                            HoraApertura = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).HorarioSucursal[DiaSeleccionado].HoraApertura;
                            HoraApertura = new DateTime(FechaAjustada.Year, FechaAjustada.Month, FechaAjustada.Day, HoraApertura.Hour, HoraApertura.Minute, 0);
                            HoraCierre = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).HorarioSucursal[DiaSeleccionado].HoraCierre;
                            HoraCierre = new DateTime(FechaAjustada.Year, FechaAjustada.Month, FechaAjustada.Day, HoraCierre.Hour, HoraCierre.Minute, 0);
                            if (ProcesarSiguienteDia)
                            {
                                //Cuando son múltiples días, se reajusta la fecha para que la fecha de inicio concuerde con la fecha de apertura del siguiente día
                                FechaAjustada = new DateTime(HoraApertura.Year, HoraApertura.Month, HoraApertura.Day, HoraApertura.Hour, HoraApertura.Minute, HoraApertura.Second);
                            }
                            //Calcula la cantidad de horas estándar y extras del día
                            ProcesarHorasDiarias(HoraApertura, HoraCierre, FechaAjustada, FechaHoraFinalizacion, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                        }

                        if (FechaHoraFinalizacion.Date > FechaAjustada.Date)
                        {
                            //Son múltiples días, se ajusta la fecha de inicio y se repite el ciclo
                            FechaAjustada = FechaAjustada.AddDays(1);
                            ProcesarSiguienteDia = true;
                        }
                        else
                        {
                            //Era solamente un día, se finaliza el ciclo
                            ProcesarSiguienteDia = false;
                        }
                    } while (ProcesarSiguienteDia);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        public static void ReorganizarFechas(ref DateTime FechaHoraInicio, ref DateTime FechaHoraFinalizacion)
        {
            DateTime FechaTemporal;
            try
            {
                if (FechaHoraInicio > FechaHoraFinalizacion)
                {
                    FechaTemporal = new DateTime(FechaHoraFinalizacion.Year, FechaHoraFinalizacion.Month, FechaHoraFinalizacion.Day, FechaHoraFinalizacion.Hour, FechaHoraFinalizacion.Minute, FechaHoraFinalizacion.Second);
                    FechaHoraFinalizacion = new DateTime(FechaHoraInicio.Year, FechaHoraInicio.Month, FechaHoraInicio.Day, FechaHoraInicio.Hour, FechaHoraInicio.Minute, FechaHoraInicio.Second);
                    FechaHoraInicio = new DateTime(FechaTemporal.Year, FechaTemporal.Month, FechaTemporal.Day, FechaTemporal.Hour, FechaTemporal.Minute, FechaTemporal.Second);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Procesa la cantidad de horas estándar y extras de un día específico
        /// </summary>
        /// <param name="HoraApertura">Hora de apertura del día</param>
        /// <param name="HoraCierre">Hora de cierre del día</param>
        /// <param name="FechaHoraInicio">Fecha ajustada de inicio del servicio (Cuando son múltiples días se ajusta)</param>
        /// <param name="FechaHoraFinalizacion">Fecha de finalización del servicio</param>
        /// <param name="CantidadHorasEstandar">Variable que tomará el valor de las horas estándar obtenidas</param>
        /// <param name="CantidadHorasExtra">Variable que tomará el valor de las horas extras obtenidas</param>

        private static void ProcesarHorasDiarias(DateTime HoraApertura, DateTime HoraCierre, DateTime FechaHoraInicio, DateTime FechaHoraFinalizacion, ref double CantidadHorasEstandar, ref double CantidadHorasExtra, TrabajaFinSemana TrabajaFinSemana)
        {
            TimeSpan DiferenciaTiempo = new TimeSpan();
            try
            {
                //Horas estándar
                //Solamente se toman en cuenta las horas dentro del horario o que inician antes de horario
                //y finalizan después de la hora de apertura
                if (FechaHoraFinalizacion < HoraCierre)
                {
                    if (FechaHoraFinalizacion > FechaHoraInicio && FechaHoraFinalizacion > HoraApertura)
                    {
                        DiferenciaTiempo = FechaHoraFinalizacion - FechaHoraInicio;
                    }
                }
                else
                {
                    if (HoraCierre > FechaHoraInicio)
                    {
                        DiferenciaTiempo = HoraCierre - FechaHoraInicio;
                    }
                }

                //Horas en días laborales estándar Lunes a Viernes
                if (FechaHoraInicio.DayOfWeek != DayOfWeek.Saturday && FechaHoraInicio.DayOfWeek != DayOfWeek.Sunday)
                {
                    CantidadHorasEstandar += DiferenciaTiempo.TotalHours;
                }

                //Horas en días laborales fines de semana
                if ((FechaHoraInicio.DayOfWeek == DayOfWeek.Saturday) && (FechaHoraInicio.Date == FechaHoraFinalizacion.Date))
                {
                    if (TrabajaFinSemana == CostoManoObra.TrabajaFinSemana.No)
                    {
                        //Si el empleado no trabaja fines de semana, se le debe tomar cualquier hora dentro de horario como extra
                        CantidadHorasExtra += DiferenciaTiempo.TotalHours;
                    }
                    else
                    {
                        CantidadHorasEstandar += DiferenciaTiempo.TotalHours;
                    }
                }

                //Horas extra
                //Todas las horas que finalizan después del cierre de la sucursal pero antes de media noche
                if (FechaHoraFinalizacion.Date == HoraCierre.Date && FechaHoraFinalizacion > HoraCierre)
                {
                    if (FechaHoraInicio > HoraCierre)
                    {
                        if (FechaHoraFinalizacion > FechaHoraInicio)
                        {
                            DiferenciaTiempo = FechaHoraFinalizacion - FechaHoraInicio;
                        }
                        else
                        {
                            DiferenciaTiempo = FechaHoraInicio - FechaHoraFinalizacion;
                        }
                    }
                    else
                    {
                        DiferenciaTiempo = FechaHoraFinalizacion - HoraCierre;
                    }

                    //Horas extra en días laborales Lunes a Viernes
                    if (FechaHoraInicio.DayOfWeek != DayOfWeek.Saturday && FechaHoraInicio.DayOfWeek != DayOfWeek.Sunday)
                    {
                        CantidadHorasExtra += DiferenciaTiempo.TotalHours;
                    }

                    //Horas extra fines de semana, solo se contabiliza si se finaliza el mismo sábado
                    if ((FechaHoraInicio.DayOfWeek == DayOfWeek.Saturday) && (FechaHoraInicio.Date == FechaHoraFinalizacion.Date))
                    {
                        CantidadHorasExtra += DiferenciaTiempo.TotalHours;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Valida que exista al menos un horario configurado
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <returns>True = Existe al menos un horario configurado. False = No existen horarios configurados</returns>
        private static bool HorariosConfigurados(string CodigoSucursal)
        {
            bool Resultado = false;
            try
            {
                if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).HorarioSucursal.Count > 0)
                {
                    foreach (var KeyValue in DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).HorarioSucursal)
                    {
                        if (KeyValue.Value.HorarioConfigurado)
                        {
                            Resultado = true;
                            break;
                        }
                    }
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return false;
            }
        }

        /// <summary>
        /// Obtiene el salario por hora del empleado de acuerdo a lo configurado en el maestro de empleados
        /// </summary>
        /// <param name="empID">Código único del empleado</param>
        /// <returns>Salario por hora en formato decimal</returns>
        public static double ObtenerSalarioPorHora(int empID, ref TrabajaFinSemana TrabajaFinSemana)
        {
            SAPbobsCOM.EmployeesInfo Empleado;
            double SalarioPorHora = 0;
            string TrabajaFinSemanaTexto;
            try
            {
                Empleado = (SAPbobsCOM.EmployeesInfo)DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo);
                Empleado.GetByKey(empID);
                SalarioPorHora = double.Parse(Empleado.UserFields.Fields.Item("U_SCGD_sALXHORA").Value.ToString());
                TrabajaFinSemanaTexto = Empleado.UserFields.Fields.Item("U_SCGD_WorksWeekends").Value.ToString();
                if (TrabajaFinSemanaTexto == "Y")
                {
                    TrabajaFinSemana = CostoManoObra.TrabajaFinSemana.Si;
                }
                else
                {
                    TrabajaFinSemana = CostoManoObra.TrabajaFinSemana.No;
                }

                return SalarioPorHora;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return 0;
            }
        }

        /// <summary>
        /// Obtiene la tarifa o multiplicador para calcular las horas extra
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <returns>Tarifa en formato decimal</returns>
        public static double ObtenerTarifaHorasExtra(string CodigoSucursal)
        {
            double TarifaHorasExtra = 0;
            try
            {
                TarifaHorasExtra = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).U_ExtraHourRate;
                return TarifaHorasExtra;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la duración estándar de una determinada actividad
        /// </summary>
        /// <param name="DocEntryCotizacion">DocEntry de la oferta de ventas ligada a la orden de trabajo</param>
        /// <param name="IDActividad">ID único de la actividad</param>
        /// <returns>Duración estándar de la actividad en formato entero</returns>
        public static int ObtenerDuracionEstandar(string DocEntryCotizacion, string IDActividad)
        {
            int DuracionEstandar = 0;
            string Query = " SELECT TOP 1 T1.\"U_SCGD_Duracion\" FROM \"QUT1\" T0 INNER JOIN \"OITM\" T1 ON T0.\"ItemCode\" = T1.\"ItemCode\" WHERE T0.\"DocEntry\" = '{0}' AND T0.\"U_SCGD_ID\" = '{1}' ";
            try
            {
                Query = string.Format(Query, DocEntryCotizacion, IDActividad);
                DuracionEstandar = Convert.ToInt32(DMS_Connector.Helpers.EjecutarConsulta(Query));
                return DuracionEstandar;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Valida si se utiliza el cálculo estándar o el cálculo basado en el horario del taller (Incluido manejo de horas extra)
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <returns>True = Utiliza cálculo basado en horario de taller. False = Utiliza cálculo estándar.</returns>
        public static bool UsaCalculoSobreHorarioTaller(string CodigoSucursal)
        {
            bool Resultado = false;
            try
            {
                if (DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(x => x.U_Sucurs == CodigoSucursal).U_CalHT.Trim() == "Y")
                {
                    Resultado = true;
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

    }
}
