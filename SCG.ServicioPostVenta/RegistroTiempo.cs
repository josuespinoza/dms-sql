using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DMS_Connector.Data_Access;

namespace SCG.ServicioPostVenta
{
    /// <summary>
    /// Clase encargada del manejo del formulario Registro de Tiempo
    /// </summary>
    public static class RegistroTiempo
    {
        public static OrdenTrabajo OrdenTrabajoAbierta { get; set; }

        private enum MetodoCosteo
        {
            SinConfigurar,
            TiempoEstandar,
            TiempoReal
        }
         
        /// <summary>
        /// Valida si el usuario conectado tiene permisos para abrir el formulario
        /// </summary>
        /// <returns></returns>
        private static bool PermisosValidos()
        {
            bool Resultado = false;
            try
            {
                if (DMS_Connector.Helpers.PermisosMenu("SCGD_TIMER"))
                {
                    Resultado = true;
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
        /// Abre una instancia del formulario Registro de Tiempo
        /// </summary>
        /// <param name="CodigoSucursal">Código de la sucursal</param>
        /// <param name="NumeroOT">Número de orden de trabajo. Ejemplo: 100-01, 1050-03, ...</param>
        /// <param name="EstadoOrdenTrabajo">Estado de la orden de trabajo (No Iniciada, Iniciada, ...)</param>
        public static void AbrirFormulario(OrdenTrabajo OrdenTrabajo, string CodigoSucursal, string NumeroOT, GeneralEnums.EstadoOT EstadoOrdenTrabajo, string DocEntryCotizacion)
        {
            SAPbouiCOM.FormCreationParams PaqueteCreacion;
            XmlDocument Documento;
            string Path = string.Empty;
            SAPbouiCOM.Form Formulario;

            try
            {
                if (PermisosValidos())
                {
                    if (!string.IsNullOrEmpty(NumeroOT))
                    {
                        OrdenTrabajoAbierta = OrdenTrabajo;
                        if (EstadoOrdenTrabajo == GeneralEnums.EstadoOT.NoIniciada || EstadoOrdenTrabajo == GeneralEnums.EstadoOT.Iniciada || EstadoOrdenTrabajo == GeneralEnums.EstadoOT.Suspendida)
                        {
                            if (!string.IsNullOrEmpty(CodigoSucursal) && !string.IsNullOrEmpty(NumeroOT))
                            {
                                Documento = new XmlDocument();
                                Path = string.Format("{0}{1}", System.Environment.CurrentDirectory, Resource.XMLTimeRecord);
                                Documento.Load(Path);
                                PaqueteCreacion = (SAPbouiCOM.FormCreationParams)DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                                PaqueteCreacion.XmlData = Documento.InnerXml;
                                Formulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(PaqueteCreacion);
                                Formulario.DataSources.UserDataSources.Item("Branch").ValueEx = CodigoSucursal;
                                Formulario.DataSources.UserDataSources.Item("NoOT").ValueEx = NumeroOT;
                                Formulario.DataSources.UserDataSources.Item("QDocEntry").ValueEx = DocEntryCotizacion;
                                CargarListaServicios(ref Formulario);
                            }
                        }
                        else
                        {
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoEdicionTiempo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
                else
                {
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorPermisosFormulario, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Carga la lista de servicios en estado suspendido o finalizado
        /// </summary>
        /// <param name="Formulario">Instancia del formulario registro de tiempos</param>
        private static void CargarListaServicios(ref SAPbouiCOM.Form Formulario)
        {
            string Query = string.Empty;
            SAPbouiCOM.DataTable Registros;
            SAPbouiCOM.Matrix MatrizRegistros;
            string NumeroOrdenTrabajo = string.Empty;
            try
            {
                Query = DMS_Connector.Queries.GetStrQueryFormat("EdicionRegistroTiempo");
                MatrizRegistros = (SAPbouiCOM.Matrix)Formulario.Items.Item("Records").Specific;
                Registros = Formulario.DataSources.DataTables.Item("Registros");
                NumeroOrdenTrabajo = Formulario.DataSources.UserDataSources.Item("NoOT").ValueEx;
                if (!string.IsNullOrEmpty(NumeroOrdenTrabajo))
                {
                    Query = string.Format(Query, NumeroOrdenTrabajo, Convert.ToInt32(GeneralEnums.EstadoActividades.Suspendido), Convert.ToInt32(GeneralEnums.EstadoActividades.Finalizado));
                    Registros.ExecuteQuery(Query);
                    MatrizRegistros.LoadFromDataSource();
                    MatrizRegistros.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Manejador de eventos del tipo ItemEvent
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        public static void ItemEvent(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {
                if (pVal.FormTypeEx == "SCGD_TIMEL")
                {
                    switch (pVal.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                            ItemPressed(FormUID, pVal, ref BubbleEvent);
                            break;
                        case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS:
                            LostFocus(FormUID, pVal, ref BubbleEvent);
                            break;
                        case SAPbouiCOM.BoEventTypes.et_VALIDATE:
                            Validate(FormUID, pVal, ref BubbleEvent);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Manejador de eventos del tipo Validate
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        public static void Validate(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {
                if (pVal.BeforeAction)
                {
                    switch (pVal.ItemUID)
                    {
                        case "Records":
                            ValidarFechas(FormUID, pVal, ref BubbleEvent);
                            break;
                    }
                }
                else
                {
                    switch (pVal.ItemUID)
                    {
                        case "Records":
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Manejador de eventos del tipo Validate
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        public static void ValidarFechas(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.Form Formulario;
            SAPbouiCOM.Matrix MatrizRegistros;
            string Valor = string.Empty;
            try
            {
                Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID);
                MatrizRegistros = (SAPbouiCOM.Matrix)Formulario.Items.Item("Records").Specific;
                
                if (pVal.BeforeAction)
                {
                    switch (pVal.ColUID)
                    {
                        case "SDate":
                            Valor = ((SAPbouiCOM.EditText)MatrizRegistros.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific).Value;
                            if (string.IsNullOrEmpty(Valor))
                            {
                                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorFechaEnBlanco, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                BubbleEvent = false;
                            }
                            break;
                        case "STime":
                            Valor = ((SAPbouiCOM.EditText)MatrizRegistros.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific).Value;
                            if (string.IsNullOrEmpty(Valor))
                            {
                                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorFechaEnBlanco, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                BubbleEvent = false;
                            }
                            break;
                        case "EDate":
                            Valor = ((SAPbouiCOM.EditText)MatrizRegistros.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific).Value;
                            if (string.IsNullOrEmpty(Valor))
                            {
                                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorFechaEnBlanco, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                BubbleEvent = false;
                            }
                            break;
                        case "ETime":
                            Valor = ((SAPbouiCOM.EditText)MatrizRegistros.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific).Value;
                            if (string.IsNullOrEmpty(Valor))
                            {
                                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(Resource.ErrorFechaEnBlanco, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                BubbleEvent = false;
                            }
                            break;
                    }
                }
                else
                {
                    switch (pVal.ColUID)
                    {
                        case "Records":
                            //Implementar manejo del BeforeAction False aquí
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }



        /// <summary>
        /// Manejador de eventos del tipo Validate
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        public static void LostFocus(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {
                if (pVal.BeforeAction)
                {
                    switch (pVal.ItemUID)
                    {
                        case "Records":
                            //Implementar validaciones aquí
                            break;
                    }
                }
                else
                {
                    switch (pVal.ItemUID)
                    {
                        case "Records":
                            RecalcularMontos(FormUID);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Manejador de eventos ItemPressed
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        public static void ItemPressed(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {
                if (pVal.BeforeAction)
                {
                    switch (pVal.ItemUID)
                    {
                        case "Save":
                            //Implementar validaciones aquí
                            break;
                    }
                }
                else
                {
                    switch (pVal.ItemUID)
                    {
                        case "Save":
                            GuardarCambios(FormUID, pVal, ref BubbleEvent);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Guarda los cambios registrados
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        /// <param name="pVal">Variable que contiene la información del evento</param>
        /// <param name="BubbleEvent">Variable que indica si se debe o no continuar procesando el evento</param>
        private static void GuardarCambios(string FormUID, SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.Form Formulario;
            SAPbouiCOM.DataTable Registros;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChild;
            SAPbobsCOM.GeneralDataCollection oChildrenCollection;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            string Sucursal = string.Empty;
            string NumeroOT = string.Empty;
            string LineIdRegistro = string.Empty;
            string LineIdCtrlCol = string.Empty;
            DateTime FechaHoraInicio;
            DateTime FechaHoraFinalizacion;
            double CostoReal = 0;
            double TotalMinutosReales = 0;
            double TotalMinutosAnterior = 0;
            double SumatoriaMinutosReales = 0;
            string FechaInicioTexto = string.Empty;
            string HoraInicioTexto = string.Empty;
            string FechaFinTexto = string.Empty;
            string HoraFinTexto = string.Empty;
            string empID = string.Empty;
            string IDActividad = string.Empty;
            int CodigoError;
            string MensajeError = string.Empty;
            MetodoCosteo MetodoCosteoServicio = MetodoCosteo.SinConfigurar;
            double CostoLineaOfertaVentas;
            string DocEntryCotizacion = string.Empty;

            try
            {
                Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID);
                Registros = Formulario.DataSources.DataTables.Item("Registros");
                NumeroOT = Formulario.DataSources.UserDataSources.Item("NoOT").ValueEx;
                Sucursal = Formulario.DataSources.UserDataSources.Item("Branch").ValueEx;

                if (!Registros.IsEmpty)
                {
                    RecalcularMontos(FormUID);

                    MetodoCosteoServicio = ObtenerMetodoCosteo(Sucursal);

                    if (MetodoCosteoServicio == MetodoCosteo.SinConfigurar)
                    {
                        //Mensaje de error, el método de costeo (Tiempo estándar o tiempo real no esta configurado correctamente)
                        BubbleEvent = false;
                        if (DMS_Connector.Company.CompanySBO.InTransaction) DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(Resource.MetodoCosteoSinConfigurar, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    }
                    else
                    {
                        SAPbobsCOM.Documents OfertaVentas = (SAPbobsCOM.Documents)DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                        DocEntryCotizacion = Formulario.DataSources.UserDataSources.Item("QDocEntry").ValueEx;
                        if (OfertaVentas.GetByKey(Convert.ToInt32(DocEntryCotizacion)))
                        {
                            oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService();
                            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                            oGeneralParams = (SAPbobsCOM.GeneralDataParams)
                            oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                            oGeneralParams.SetProperty("Code", NumeroOT);
                            oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oChildrenCollection = oGeneralData.Child("SCGD_CTRLCOL");

                            for (int i = 0; i < Registros.Rows.Count; i++)
                            {
                                FechaInicioTexto = ((DateTime)Registros.GetValue("StartDate", i)).ToString("yyyyMMdd");
                                HoraInicioTexto = Registros.GetValue("StartTime", i).ToString();
                                if (!string.IsNullOrEmpty(HoraInicioTexto))
                                {
                                    HoraInicioTexto = HoraInicioTexto.PadLeft(4, '0');
                                }
                                FechaFinTexto = ((DateTime)Registros.GetValue("EndDate", i)).ToString("yyyyMMdd");
                                HoraFinTexto = Registros.GetValue("EndTime", i).ToString();
                                if (!string.IsNullOrEmpty(HoraFinTexto))
                                {
                                    HoraFinTexto = HoraFinTexto.PadLeft(4, '0');
                                }

                                if (EsFechaValida(FechaInicioTexto, HoraInicioTexto, FechaFinTexto, HoraFinTexto))
                                {
                                    FechaHoraInicio = DateTime.ParseExact(string.Format("{0} {1}", FechaInicioTexto, HoraInicioTexto), "yyyyMMdd HHmm", null);
                                    FechaHoraFinalizacion = DateTime.ParseExact(string.Format("{0} {1}", FechaFinTexto, HoraFinTexto), "yyyyMMdd HHmm", null);
                                    LineIdRegistro = Registros.GetValue("LineId", i).ToString();
                                    CostoReal = (double)Registros.GetValue("RealCost", i);
                                    TotalMinutosReales = (double)Registros.GetValue("TotalMinutes", i);

                                    for (int j = 0; j < oChildrenCollection.Count; j++)
                                    {
                                        oChild = oChildrenCollection.Item(j);
                                        LineIdCtrlCol = oChild.GetProperty("LineId").ToString().Trim();
                                        if (!string.IsNullOrEmpty(LineIdRegistro) && LineIdRegistro == LineIdCtrlCol)
                                        {
                                            empID = oChild.GetProperty("U_Colab").ToString();
                                            IDActividad = oChild.GetProperty("U_IdAct").ToString();
                                            oChild.SetProperty("U_DFIni", FechaHoraInicio);
                                            oChild.SetProperty("U_HFIni", FechaHoraInicio);
                                            oChild.SetProperty("U_DFFin", FechaHoraFinalizacion);
                                            oChild.SetProperty("U_HFFin", FechaHoraFinalizacion);
                                            oChild.SetProperty("U_CosRe", CostoReal);
                                            TotalMinutosAnterior = (double)oChild.GetProperty("U_TMin");
                                            oChild.SetProperty("U_TMin", TotalMinutosReales);
                                            if (MetodoCosteoServicio == MetodoCosteo.TiempoEstandar)
                                            {
                                                CostoLineaOfertaVentas = ObtenerSumatoriaCostoEstandar(ref oChildrenCollection, IDActividad);
                                            }
                                            else
                                            {
                                                //Se suman todas las líneas del mismo ID
                                                CostoLineaOfertaVentas = CostoReal + ObtenerSumatoriaCostoReal(ref oChildrenCollection, LineIdRegistro, IDActividad);
                                            }
                                            SumatoriaMinutosReales = ObtenerSumatoriaTiempoReal(ref oChildrenCollection, IDActividad);
                                            ActualizarLineaCotizacion(ref OfertaVentas, IDActividad, CostoLineaOfertaVentas, SumatoriaMinutosReales);
                                            break;
                                        }
                                    }
                                }
                            }

                            DMS_Connector.Company.CompanySBO.StartTransaction();
                            if (OfertaVentas.Update() == 0)
                            {
                                oGeneralService.Update(oGeneralData);
                                if (DMS_Connector.Company.CompanySBO.InTransaction) DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            }
                            else
                            {
                                DMS_Connector.Company.CompanySBO.GetLastError(out CodigoError, out MensajeError);
                                throw new Exception(string.Format("{0}: {1}", CodigoError, MensajeError));
                            }

                            Formulario.Close();
                            OrdenTrabajoAbierta.recargarActividades(NumeroOT, DMS_Connector.Company.ApplicationSBO);
                        }
                    }
                }
                else
                {
                    Formulario.Close();
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                if (DMS_Connector.Company.CompanySBO.InTransaction)
                    DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            }
        }



        private static void ActualizarLineaCotizacion(ref SAPbobsCOM.Documents OfertaVentas, string IDActividad, double Costo, double SumatoriaMinutosReales)
        {
            SAPbobsCOM.Document_Lines Lineas;
            try
            {
                Lineas = OfertaVentas.Lines;
                for (int i = 0; i <= Lineas.Count - 1; i++)
                {
                    Lineas.SetCurrentLine(i);
                    if (Lineas.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == IDActividad)
                    {
                        Lineas.UserFields.Fields.Item("U_SCGD_Costo").Value = Costo;
                        Lineas.UserFields.Fields.Item("U_SCGD_TiempoReal").Value = SumatoriaMinutosReales;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        private static double ObtenerSumatoriaTiempoReal(ref SAPbobsCOM.GeneralDataCollection ChildrenCollection, string IDActividadBuscado)
        {
            double TiempoRealTotal = 0;
            string IDActividad = string.Empty;
            SAPbobsCOM.GeneralData oChild;

            try
            {
                for (int j = 0; j < ChildrenCollection.Count; j++)
                {
                    oChild = ChildrenCollection.Item(j);

                    IDActividad = oChild.GetProperty("U_IdAct").ToString();

                    if (IDActividad == IDActividadBuscado)
                    {
                        TiempoRealTotal += (double)oChild.GetProperty("U_TMin");
                    }
                }
                return TiempoRealTotal;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        private static double ObtenerSumatoriaCostoReal(ref SAPbobsCOM.GeneralDataCollection ChildrenCollection, string LineIdExcluido, string IDActividadBuscado)
        {
            double CostoRealTotal = 0;
            string IDActividad = string.Empty;
            SAPbobsCOM.GeneralData oChild;
            string empID = string.Empty;
            string LineId = string.Empty;
            try
            {
                for (int j = 0; j < ChildrenCollection.Count; j++)
                {
                    oChild = ChildrenCollection.Item(j);
                    LineId = oChild.GetProperty("LineId").ToString();
                    IDActividad = oChild.GetProperty("U_IdAct").ToString();
                    empID = oChild.GetProperty("U_Colab").ToString();
                    if (LineId != LineIdExcluido)
                    {
                        if (IDActividad == IDActividadBuscado)
                        {
                            CostoRealTotal += (double)oChild.GetProperty("U_CosRe");
                        }
                    }

                }
                return CostoRealTotal;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        private static double ObtenerSumatoriaCostoEstandar(ref SAPbobsCOM.GeneralDataCollection ChildrenCollection, string IDActividadBuscado)
        {
            double CostoEstandarTotal = 0;
            string IDActividad = string.Empty;
            SAPbobsCOM.GeneralData oChild;
            string empID = string.Empty;
            List<string> EmpleadosAsignados;
            try
            {
                EmpleadosAsignados = new List<string>();
                for (int j = 0; j < ChildrenCollection.Count; j++)
                {
                    oChild = ChildrenCollection.Item(j);
                    IDActividad = oChild.GetProperty("U_IdAct").ToString();
                    empID = oChild.GetProperty("U_Colab").ToString();
                    if (IDActividad == IDActividadBuscado)
                    {
                        if (!EmpleadosAsignados.Contains(empID))
                        {
                            EmpleadosAsignados.Add(empID);
                            //Solamente se toma el último costo estándar del último empleado asignado
                            CostoEstandarTotal = (double)oChild.GetProperty("U_CosEst");
                        }
                    }
                }
                return CostoEstandarTotal;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        private static MetodoCosteo ObtenerMetodoCosteo(string Sucursal)
        {
            string TiempoReal = string.Empty;
            string TiempoEstandar = string.Empty;
            string PrecioOfertaVentas = string.Empty;
            MetodoCosteo Resultado = MetodoCosteo.TiempoEstandar;
            try
            {
                TiempoReal = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(x => x.U_Sucurs == Sucursal).U_TiempoReal_C.Trim();
                TiempoEstandar = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(x => x.U_Sucurs == Sucursal).U_TiempoEst_C.Trim();
                PrecioOfertaVentas = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(x => x.U_Sucurs == Sucursal).U_TiempoOFV_C.Trim();

                if (TiempoEstandar == "Y" && TiempoReal == "N")
                {
                    Resultado = MetodoCosteo.TiempoEstandar;
                }

                if (TiempoReal == "Y" && TiempoEstandar == "N")
                {
                    Resultado = MetodoCosteo.TiempoReal;
                }

                //Verifica que los métodos de costeo estén configurados y sean excluyentes entre sí
                if ((string.IsNullOrEmpty(TiempoEstandar) && string.IsNullOrEmpty(TiempoReal)) || (TiempoEstandar == "N" && TiempoReal == "N") || (TiempoEstandar == "Y" && TiempoReal == "Y"))
                {
                    if ((string.IsNullOrEmpty(PrecioOfertaVentas) || PrecioOfertaVentas == "N") || (TiempoEstandar == "Y" && TiempoReal == "Y"))
                    {
                        Resultado = MetodoCosteo.SinConfigurar;
                    }
                    else
                    {
                        if (PrecioOfertaVentas == "Y")
                        {
                            Resultado = MetodoCosteo.TiempoEstandar;
                        }
                    }
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }


        //public void SuspenderActividad(string p_strRazon, string p_strComentario, IApplication applicationSbo, ICompany companySbo, DateTime p_dtFechaFin, Boolean p_suspendeOT = false)
        //{
        //    SAPbobsCOM.CompanyService oCompanyService;
        //    SAPbobsCOM.GeneralService oGeneralService;
        //    SAPbobsCOM.GeneralData oGeneralData;
        //    SAPbobsCOM.GeneralData oChildCC;
        //    SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
        //    SAPbobsCOM.GeneralDataParams oGeneralParams;

        //    Matrix m_objMatrix;
        //    DateTime m_dtFechaInicio;
        //    TimeSpan m_dtFechaDiferencia;
        //    DateTime m_dtHoraInicio;

        //    int intError;
        //    string strError;
        //    string m_strNoOT = string.Empty;
        //    string IDActividad = string.Empty;
        //    string DocEntryCotizacion = string.Empty;
        //    string strSuspensionHorario = "8";
        //    double m_dblCostoReal = 0;
        //    bool m_blnActividadSuspendida = false;
        //    bool m_blnSuspendeAct = true;
        //    SAPbouiCOM.Form oFormOT;
        //    double SalarioPorHora = 0;
        //    double TarifaHorasExtra = 0;
        //    int DuracionEstandar = 0;
        //    int empID;
        //    string Sucursal = string.Empty;
        //    double CostoEstandar = 0;
        //    double CantidadHorasEstandar = 0;
        //    double CantidadHorasExtra = 0;
        //    bool UsaCalculoSobreHorarioTaller = false;

        //    try
        //    {
        //        oFormOT = applicationSbo.Forms.Item("SCGD_ORDT");

        //        GuardaRazonSuspension(p_strRazon, p_strComentario, oFormOT);

        //        m_objMatrix = (Matrix)oFormOT.Items.Item("mtxColab").Specific;
        //        m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
        //        DocEntryCotizacion = oFormOT.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();

        //        g_blnSuspenderActividad = true;

        //        m_objMatrix.FlushToDataSource();
        //        SAPbobsCOM.Documents m_objCotizacion = (Documents)companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
        //        oCompanyService = CompanySBO.GetCompanyService();
        //        oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
        //        oGeneralParams = (SAPbobsCOM.GeneralDataParams)
        //        oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
        //        oGeneralParams.SetProperty("Code", m_strNoOT);
        //        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
        //        Sucursal = oGeneralData.GetProperty("U_Sucu").ToString();
        //        oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

        //        if (m_objCotizacion.GetByKey(Convert.ToInt32(DocEntryCotizacion)))
        //        {
        //            TarifaHorasExtra = CalculoCostos.CostoManoObra.ObtenerTarifaHorasExtra(Sucursal);
        //            UsaCalculoSobreHorarioTaller = CalculoCostos.CostoManoObra.UsaCalculoSobreHorarioTaller(Sucursal);
        //            for (int i = 1; i <= m_objMatrix.RowCount; i++)
        //            {
        //                if ((p_suspendeOT || m_objMatrix.IsRowSelected(i)) &&
        //                    oFormOT.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim() ==
        //                    g_strEstado_Iniciado)
        //                {
        //                    oChildCC = oChildrenCtrlCol.Item(i - 1);
        //                    IDActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
        //                    m_dtFechaInicio = DateTime.Parse(oChildCC.GetProperty("U_DFIni").ToString());
        //                    m_dtHoraInicio = DateTime.Parse(oChildCC.GetProperty("U_HFIni").ToString());
        //                    m_dtFechaInicio = new DateTime(m_dtFechaInicio.Year, m_dtFechaInicio.Month, m_dtFechaInicio.Day, m_dtHoraInicio.Hour, m_dtHoraInicio.Minute, m_dtHoraInicio.Second);
        //                    m_dtFechaDiferencia = p_dtFechaFin - m_dtFechaInicio;
        //                    empID = Convert.ToInt32(oChildCC.GetProperty("U_Colab").ToString().Trim());
        //                    SalarioPorHora = CalculoCostos.CostoManoObra.ObtenerSalarioPorHora(empID);
        //                    DuracionEstandar = CalculoCostos.CostoManoObra.ObtenerDuracionEstandar(DocEntryCotizacion, IDActividad);

        //                    if (UsaCalculoSobreHorarioTaller)
        //                    {
        //                        CalculoCostos.CostoManoObra.CalcularCostoCompuesto(Sucursal, m_dtFechaInicio, p_dtFechaFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref m_dblCostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra);
        //                        oChildCC.SetProperty("U_TMin", (CantidadHorasEstandar + CantidadHorasExtra) * 60.0);
        //                    }
        //                    else
        //                    {
        //                        m_dblCostoReal = ObtieneCostosReal(oChildCC.GetProperty("U_Colab").ToString().Trim(), (double)m_dtFechaDiferencia.TotalMinutes, oFormOT);
        //                        oChildCC.SetProperty("U_TMin", m_dtFechaDiferencia.TotalMinutes);
        //                    }

        //                    oChildCC.SetProperty("U_DFFin", p_dtFechaFin);
        //                    oChildCC.SetProperty("U_HFFin", p_dtFechaFin);

        //                    oChildCC.SetProperty("U_Estad", g_strEstado_Suspendido);
        //                    oChildCC.SetProperty("U_CosRe", m_dblCostoReal);

        //                    Si la actividad es suspendida por horario se debe marcar como Y el campo U_SuspensionHorario
        //                    ya que esta información se utiliza al graficar la agenda
        //                    if (p_strRazon == strSuspensionHorario)
        //                    {
        //                        oChildCC.SetProperty("U_SuspensionHorario", "Y");
        //                    }
        //                    else
        //                    {
        //                        oChildCC.SetProperty("U_SuspensionHorario", "N");
        //                    }


        //                    Si la actividad es suspendida por horario se debe marcar como Y el campo U_SuspensionHorario
        //                    ya que esta información se utiliza al graficar la agenda
        //                    if (p_strRazon == strSuspensionHorario)
        //                    {
        //                        oChildCC.SetProperty("U_SuspensionHorario", "Y");
        //                    }
        //                    else
        //                    {
        //                        oChildCC.SetProperty("U_SuspensionHorario", "N");
        //                    }

        //                    ActualizarActividadCotizacion(ref m_objCotizacion, IDActividad, g_strEstado_Suspendido, string.Empty, 0, m_dtFechaDiferencia.TotalMinutes);
        //                    m_blnActividadSuspendida = true;
        //                }
        //            }

        //            if (p_suspendeOT && !m_blnActividadSuspendida)
        //            {
        //                m_blnActividadSuspendida = true;
        //                m_blnSuspendeAct = false;
        //            }

        //            if (m_blnActividadSuspendida)
        //            {
        //                ManejarEstadoOT(false, true, false, ref oGeneralData);

        //                var estado = ValidaEstadoOT(ref oGeneralData);
        //                var descEstado = string.Empty;
        //                ObtieneDescripcionEstado(estado.ToString(), ref descEstado, oFormOT);

        //                m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = estado.ToString();
        //                m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = descEstado;

        //                if (!companySbo.InTransaction)
        //                    companySbo.StartTransaction();

        //                if (m_objCotizacion.Update() == 0)
        //                {
        //                    oGeneralService.Update(oGeneralData);
        //                    if (CompanySBO.InTransaction)
        //                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
        //                }
        //                else
        //                {
        //                    if (CompanySBO.InTransaction)
        //                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);

        //                    CompanySBO.GetLastError(out intError, out strError);
        //                    ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intError, strError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
        //                }

        //                recargarActividades(m_strNoOT, ApplicationSBO);

        //                if (!p_suspendeOT)
        //                    applicationSbo.StatusBar.SetText(Resource.ActividadSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
        //                else
        //                    applicationSbo.StatusBar.SetText(Resource.OTSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

        //                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (CompanySBO.InTransaction)
        //            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
        //        ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
        //    }
        //    finally
        //    {
        //        if (CompanySBO.InTransaction)
        //            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
        //    }
        //}


        /// <summary>
        /// Recalcula los costos de los servicios de acuerdo a las nuevas fechas seleccionadas
        /// </summary>
        /// <param name="FormUID">ID único del formulario</param>
        private static void RecalcularMontos(string FormUID)
        {
            SAPbouiCOM.Form Formulario;
            SAPbouiCOM.DataTable Registros;
            SAPbouiCOM.Matrix MatrizRegistros;
            DateTime FechaHoraInicio;
            DateTime FechaHoraFin;
            double SalarioPorHora = 0;
            double TarifaHorasExtra = 1;
            double CostoEstandar = 0;
            double CostoReal = 0;
            double CantidadHorasEstandar = 0;
            double CantidadHorasExtra = 0;
            string empID = string.Empty;
            string FechaInicioTexto = string.Empty;
            string HoraInicioTexto = string.Empty;
            string FechaFinTexto = string.Empty;
            string HoraFinTexto = string.Empty;
            string CodigoSucursal = string.Empty;
            int DuracionEstandar = 0;
            bool UsaCalculoSobreHorarioTaller = false;
            CalculoCostos.CostoManoObra.TrabajaFinSemana TrabajaFinSemana;
            try
            {
                Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID);
                MatrizRegistros = (SAPbouiCOM.Matrix)Formulario.Items.Item("Records").Specific;
                MatrizRegistros.FlushToDataSource();
                CodigoSucursal = Formulario.DataSources.UserDataSources.Item("Branch").ValueEx;
                Registros = Formulario.DataSources.DataTables.Item("Registros");
                UsaCalculoSobreHorarioTaller = CalculoCostos.CostoManoObra.UsaCalculoSobreHorarioTaller(CodigoSucursal);

                if (Registros.IsEmpty)
                {
                    return;
                }

                for (int i = 0; i < Registros.Rows.Count; i++)
                {
                    FechaInicioTexto = ((DateTime)Registros.GetValue("StartDate", i)).ToString("yyyyMMdd");
                    HoraInicioTexto = Registros.GetValue("StartTime", i).ToString();
                    if (!string.IsNullOrEmpty(HoraInicioTexto))
                    {
                        HoraInicioTexto = HoraInicioTexto.PadLeft(4, '0');
                    }
                    FechaFinTexto = ((DateTime)Registros.GetValue("EndDate", i)).ToString("yyyyMMdd");
                    HoraFinTexto = Registros.GetValue("EndTime", i).ToString();
                    if (!string.IsNullOrEmpty(HoraFinTexto))
                    {
                        HoraFinTexto = HoraFinTexto.PadLeft(4, '0');
                    }
                    DuracionEstandar = (int)Registros.GetValue("StandardDuration", i);
                    SalarioPorHora = (double)Registros.GetValue("HourlyWage", i);
                    TarifaHorasExtra = (double)Registros.GetValue("ExtraHourRate", i);
                    empID = Registros.GetValue("empID", i).ToString();
                    if (EsFechaValida(FechaInicioTexto, HoraInicioTexto, FechaFinTexto, HoraFinTexto))
                    {
                        FechaHoraInicio = DateTime.ParseExact(string.Format("{0} {1}", FechaInicioTexto, HoraInicioTexto), "yyyyMMdd HHmm", null);
                        FechaHoraFin = DateTime.ParseExact(string.Format("{0} {1}", FechaFinTexto, HoraFinTexto), "yyyyMMdd HHmm", null);

                        //Cambia el orden de las fechas en caso de que el usuario las haya digitado al revés (Fecha de Inicio en el campo Fecha de Finalización)
                        CalculoCostos.CostoManoObra.ReorganizarFechas(ref FechaHoraInicio, ref FechaHoraFin);
                        Registros.SetValue("StartDate", i, FechaHoraInicio);
                        Registros.SetValue("StartTime", i, FechaHoraInicio.ToString("HHmm"));
                        Registros.SetValue("EndDate", i, FechaHoraFin);
                        Registros.SetValue("EndTime", i, FechaHoraFin.ToString("HHmm"));
                        if (UsaCalculoSobreHorarioTaller)
                        {
                            if (Registros.GetValue("WorksWeekends", i).ToString() == "Y")
                            {
                                TrabajaFinSemana = CalculoCostos.CostoManoObra.TrabajaFinSemana.Si;
                            }
                            else
                            {
                                TrabajaFinSemana = CalculoCostos.CostoManoObra.TrabajaFinSemana.No;
                            }

                            CalculoCostos.CostoManoObra.CalcularCostoCompuesto(CodigoSucursal, FechaHoraInicio, FechaHoraFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref CostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                        }
                        else
                        {
                            CalculoCostos.CostoManoObra.CalcularCostoSimple(CodigoSucursal, FechaHoraInicio, FechaHoraFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref CostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra);
                        }
                    }

                    Registros.SetValue("StandardCost", i, CostoEstandar);
                    Registros.SetValue("RealCost", i, CostoReal);
                    Registros.SetValue("TotalMinutes", i, (CantidadHorasEstandar + CantidadHorasExtra) * 60.0);
                }
                Formulario.Freeze(true);
                MatrizRegistros.LoadFromDataSource();
                Formulario.Freeze(false);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        /// <summary>
        /// Método que valida que las fechas esten completas y en el formato correcto
        /// </summary>
        /// <param name="FechaInicio">Fecha de inicio del servicio</param>
        /// <param name="HoraInicio">Hora de inicio del servicio</param>
        /// <param name="FechaFinal">Fecha de finalización del servicio</param>
        /// <param name="HoraFinal">Hora de finalización del servicio</param>
        /// <returns>True = Fechas válidas. False = Fechas inválidas o en formato incorrecto</returns>
        private static bool EsFechaValida(string FechaInicio, string HoraInicio, string FechaFinal, string HoraFinal)
        {
            bool Resultado = true;
            DateTime FechaHoraInicio;
            DateTime FechaHoraFin;
            try
            {
                if (string.IsNullOrEmpty(FechaInicio) || string.IsNullOrEmpty(HoraInicio) || string.IsNullOrEmpty(FechaFinal) || string.IsNullOrEmpty(HoraFinal))
                {
                    Resultado = false;
                }
                else
                {
                    FechaHoraInicio = DateTime.ParseExact(string.Format("{0} {1}", FechaInicio, HoraInicio), "yyyyMMdd HHmm", null);
                    FechaHoraFin = DateTime.ParseExact(string.Format("{0} {1}", FechaFinal, HoraFinal), "yyyyMMdd HHmm", null);
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return false;
            }
        }
    }
}
