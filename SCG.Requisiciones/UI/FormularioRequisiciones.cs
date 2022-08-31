using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;
using DMS_Connector.Business_Logic.DataContract.Requisiciones;

namespace SCG.Requisiciones.UI
{
    public partial class FormularioRequisiciones : IFormularioSBO, IUsaMenu
    {

        private RequisicionData oRequisicionData;
        string g_NoSerieCita = string.Empty;
        string g_NoCita = string.Empty;

        #region "CFL Codigo Empleado"

        protected virtual void CodigoClienteCFLEvent(string formUId, IItemEvent pVal, ref bool bubbleEvent)
        {
            var chooseFromListEvent = (IChooseFromListEvent)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            //bubbleEvent = true;
            if (chooseFromListEvent.BeforeAction)
            {
            }
            else
            {
                if (dataTable != null && pVal.ActionSuccess)
                {
                }
            }
        }

        #endregion

        #region "CFL Bodega Origen"

        protected virtual void AlmacenOrigenCFLEvent(string formUId, IItemEvent pVal, ref bool bubbleEvent)
        {
            var chooseFromListEvent = (IChooseFromListEvent)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            //bubbleEvent = true;
            if (chooseFromListEvent.BeforeAction)
            {
            }
            else
            {
                if (dataTable != null && pVal.ActionSuccess)
                {
                    int intNumeroLinea = pVal.Row;
                    // FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").SetValue("U_DeUbic", intNumeroLinea - 1, String.Empty);
                }
            }
        }


        #endregion

        #region "CFL Nombre Empleado"

        protected virtual void NombreClienteCFLEvent(string formUId, IItemEvent pVal, ref bool bubbleEvent)
        {
            var chooseFromListEvent = (IChooseFromListEvent)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            //bubbleEvent = true;
            if (chooseFromListEvent.BeforeAction)
            {
            }
            else
            {
                if (dataTable != null && pVal.ActionSuccess)
                {

                }
            }
        }

        #endregion

        #region "CFL De Ubicacion Origen"

        protected virtual void DeUbicacionCFLEvent(string formUId, ItemEvent pVal, ref bool bubbleEvent, ref ListaUbicaciones m_oFormSeleccionUbicaciones)
        {
            ICompany company = CompanySBO;
            var chooseFromListEvent = (IChooseFromListEvent)pVal;
            int intNumeroLinea = pVal.Row;
            string strBodegaUsada;
            string strUbicacion;

            if (company.Version > 900000)
            {
                if (chooseFromListEvent.BeforeAction)
                {
                    if (!Utilitarios.ValidarSiFormularioAbierto(strFormListaUbi, false, (SAPbouiCOM.Application)ApplicationSBO))
                    {
                        string strItemCode = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodArticulo", intNumeroLinea - 1).Trim();
                        string strLineNum = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_LNumOr", intNumeroLinea - 1).Trim();
                        string strTipoReQ = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_CodTipoReq", 0).Trim();

                        if (strTipoReQ == "1")
                        {
                            strBodegaUsada = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodBodOrigen", intNumeroLinea - 1).Trim();
                            strUbicacion = MatrixRequisiciones.ColumnaDeUbicacion.ObtieneValorColumnaMatrix(intNumeroLinea).Trim();
                        }
                        else
                        {
                            strBodegaUsada = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodBodDest", intNumeroLinea - 1).Trim();
                            strUbicacion = MatrixRequisiciones.ColumnaAUbicacion.ObtieneValorColumnaMatrix(intNumeroLinea).Trim();
                        }

                        if (!SabersiexisteUbicacion(strBodegaUsada, strItemCode, strLineNum, strUbicacion, strTipoReQ))
                        {
                            CargarFormularioSelUbicaciones(ref m_oFormSeleccionUbicaciones, ref pVal, ref bubbleEvent, strBodegaUsada, strItemCode, strLineNum, strUbicacion, strTipoReQ);
                        }
                    }
                }
            }
        }

        private Boolean SabersiexisteUbicacion(string p_strBodegaUsada, string p_strItemCode, string p_strLineNum, string p_strUbicacion, string strTipoReQ)
        {
            var query = "select count(ubi.AbsEntry) as cantidad " +
                        "from " +
                        "OBIN ubi left outer join OIBQ qt on ubi.WhsCode = qt.WhsCode and ubi.AbsEntry = qt.BinAbs " +
                        "where ubi.WhsCode = '{0}' and qt.ItemCode = '{1}' and ubi.AbsEntry like '{2}%'";
            int LineNum;
            string strresultado;
            int intresultado;

            try
            {
                //Le pongo al parametro un espacio si biene vacion para que el query no me traiga mas resultados por el like
                if (string.IsNullOrEmpty(p_strUbicacion)) p_strUbicacion = " ";

                query = string.Format(query, p_strBodegaUsada, p_strItemCode, p_strUbicacion);

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtConsulta");
                dtLocal.ExecuteQuery(query);

                LineNum = Convert.ToInt16(p_strLineNum);
                strresultado = dtLocal.GetValue("cantidad", 0).ToString();

                if (string.IsNullOrEmpty(strresultado))
                {
                    intresultado = 0;
                }
                else
                {
                    intresultado = Convert.ToInt32(strresultado);
                }

                //Comparo el resultado para saber si el codigo digitado es el unico y existe
                if (intresultado == 1)
                {
                    if (strTipoReQ == "1")
                    {
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").SetValue("U_DeUbic", LineNum, p_strUbicacion);
                    }
                    else
                    {
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").SetValue("U_AUbic", LineNum, p_strUbicacion);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region "CFL de Ubicacion Destino"

        protected virtual void AUbicacionCFLEvent(string formUId, ItemEvent pVal, ref bool bubbleEvent)
        {
            ICompany company = CompanySBO;
            var chooseFromListEvent = (IChooseFromListEvent)pVal;
            var dataTable = chooseFromListEvent.SelectedObjects;

            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
            int intNumeroLinea = pVal.Row;

            if (company.Version > 900000)
            {
                if (chooseFromListEvent.BeforeAction)
                {
                    string strAlmacenDestino = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodBodDest", intNumeroLinea - 1).Trim();
                    SAPbouiCOM.Conditions oCons = null;
                    SAPbouiCOM.Condition oCon = null;
                    SAPbouiCOM.ChooseFromList oCFLITem = null;

                    oCFLITem = FormularioSBO.ChooseFromLists.Item("CFL_AUbic");
                    oCons = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    oCon = oCons.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "WhsCode";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = strAlmacenDestino;
                    oCon.BracketCloseNum = 1;
                    oCFLITem.SetConditions(oCons);
                }
                else
                {
                    if (dataTable != null && pVal.ActionSuccess)
                    {
                        string strAlmacenDestino = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodBodDest", intNumeroLinea - 1).Trim();
                        string strItemCode = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_CodArticulo", intNumeroLinea - 1).Trim();
                    }
                }

            }
        }

        #endregion

        #region "Folder Item Pressed"

        protected virtual void FolderMovimientosItemPressed(string formUId, IItemEvent pVal, ref bool bubbleEvent)
        {
            //bubbleEvent = true;

            if (pVal.BeforeAction == false)
            {
                if (pVal.ItemUID == FolderRequisiciones.UniqueId && FormularioSBO.PaneLevel != 1)
                    FormularioSBO.PaneLevel = 1;
                else if (pVal.ItemUID == FolderMovimientos.UniqueId && FormularioSBO.PaneLevel != 2)
                    FormularioSBO.PaneLevel = 2;
            }
        }

        #endregion

        #region "FormData Events"

        protected virtual void DataLoadEvent(BusinessObjectInfo businessObjectInfo, ref bool bubbleEvent)
        {
            //bubbleEvent = true
            CheckBoxSelTodo.AsignaValorUserDataSource("N");

            if (!businessObjectInfo.BeforeAction && businessObjectInfo.ActionSuccess)
            {
                ActualizaLineasAlCargar();
                MatrixRequisiciones.Especifico.LoadFromDataSource();
                MatrixMovimientos.EliminaPrimeraLinea();
                MatrixMovimientos.Especifico.LoadFromDataSource();
            }
        }

        protected void ActualizaLineasAlCargar()
        {
            ICompany company = CompanySBO;
            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
            ManejadorEstadoLinea manejadorEstadoLinea = new ManejadorEstadoLinea(company);
            if (MatrixRequisiciones.FormularioSBO != null)
            {
                DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);
                DBDataSource formDataSource = FormularioSBO.DataSources.DBDataSources.Item((UDORequisiciones.TablaEncabezado));
                bool algunaPendiente = false;
                bool todasCanceladas = true;
                bool algunaTrasladada = false;
                string estado;
                NumberFormatInfo numberFormatInfo = DIHelper.GetNumberFormatInfo(company);
                for (int i = 0; i < dbDataSource.Size; i++)
                {
                    manejadorArticulos.ItemCode = MatrixRequisiciones.ColumnaCodigoArticulo.ObtieneValorColumnaDataTable(i, dbDataSource);
                    manejadorArticulos.WhsCode = MatrixRequisiciones.ColumnaCodigoBodegaOrigen.ObtieneValorColumnaDataTable(i, dbDataSource);
                    float cantidadDisponible = manejadorArticulos.CantidadDisponible();
                    MatrixRequisiciones.ColumnaDisponible.AsignaValorDataSource(cantidadDisponible, i, dbDataSource);

                    manejadorEstadoLinea.CantidadSolicitada = float.Parse(MatrixRequisiciones.ColumnaCantidadSolicitada.ObtieneValorColumnaDataTable(i,dbDataSource), numberFormatInfo);
                    manejadorEstadoLinea.CantidadRecibida = float.Parse(MatrixRequisiciones.ColumnaCantidadRecibida.ObtieneValorColumnaDataTable(i,dbDataSource), numberFormatInfo);

                    //manejadorEstadoLinea.CantidadAjuste = float.Parse(MatrixRequisiciones.ColumnaCantidadAjuste.ObtieneValorColumnaDataTable(i,
                    //dbDataSource), numberFormatInfo);

                    float cantidadPendiente = manejadorEstadoLinea.CantidadSolicitada - manejadorEstadoLinea.CantidadRecibida;
                    
                    //MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).SetValue("U_SCGD_CantPen",i,cantidadPendiente.ToString(numberFormatInfo));

                    MatrixRequisiciones.ColumnaCantidadPendiente.AsignaValorDataSource(cantidadPendiente, i, dbDataSource);

                    MatrixRequisiciones.ColumnaCantidadAjuste.AsignaValorDataSource(0, i, dbDataSource);

                    var codEst = MatrixRequisiciones.ColumnaCodigoEstado.ObtieneValorColumnaDataTable(i, dbDataSource);
                    manejadorEstadoLinea.EstadoActual = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), (String.IsNullOrEmpty(codEst) == true ? "1" : codEst));
                    //manejadorEstadoLinea.EstadoActual = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), MatrixRequisiciones.ColumnaCodigoEstado.ObtieneValorColumnaDataTable(i, dbDataSource));
                    manejadorEstadoLinea.CalculaEstado();
                    algunaPendiente |= manejadorEstadoLinea.EstadoActual == EstadosLineas.Pendiente;
                    todasCanceladas &= manejadorEstadoLinea.EstadoActual == EstadosLineas.Cancelado;
                    algunaTrasladada |= manejadorEstadoLinea.EstadoActual == EstadosLineas.Trasladado;

                    MatrixRequisiciones.ColumnaCodigoEstado.AsignaValorDataSource((int)(manejadorEstadoLinea.EstadoActual), i, dbDataSource);
                    estado = manejadorEstadoLinea.EstadoActual.ToString();
                    var infLinea = new InformacionLineaRequisicion();
                    MatrixRequisiciones.LineaFromDBDataSource(i, infLinea);
                    estado = Localize(infLinea, TipoMensaje.EstadoLinea, estado);
                    MatrixRequisiciones.ColumnaEstado.AsignaValorDataSource(estado, i, dbDataSource);
                    MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(0, i, dbDataSource);
                }
                EstadosLineas estadoFormulario;
                if (todasCanceladas)
                {
                    estadoFormulario = EstadosLineas.Cancelado;
                }
                else if (algunaPendiente)
                    estadoFormulario = EstadosLineas.Pendiente;
                else
                    estadoFormulario = EstadosLineas.Trasladado;

                //                MatrixRequisiciones.ColumnaCantidadATransferir.Columna.Editable = estadoFormulario == EstadosLineas.Pendiente;
                //                try
                //                {
                //AgregarMenu();
                //                }
                //                catch (Exception e)
                //                {
                //                    Console.WriteLine(e);
                //                }
                BoModeVisualBehavior behavior = estadoFormulario == EstadosLineas.Pendiente ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                ButtonSBOTrasladar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, behavior);
                ButtonSBOTrasladar.ItemSBO.Enabled = estadoFormulario == EstadosLineas.Pendiente;
                ButtonCancelar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, behavior);
                ButtonCancelar.ItemSBO.Enabled = estadoFormulario == EstadosLineas.Pendiente;
                //Manejo del botón Generador de reportes

                ButtonGenerarReporte.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, BoModeVisualBehavior.mvb_True);
                ButtonGenerarReporte.ItemSBO.Enabled = true;

                if (formDataSource.GetValue("U_SCGD_CodTipoReq", 0).Trim() == "2")
                {
                    MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;
                    MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = true;
                    MatrixRequisiciones.ColumnaCantidadAjuste.Columna.Editable = false;
                    ButtonSBOAjusteCantidad.ItemSBO.Enabled = false;
                }
                else
                {
                    MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = true;
                    MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
                    
                    if (estadoFormulario == EstadosLineas.Pendiente)
                    {
                        MatrixRequisiciones.ColumnaCantidadAjuste.Columna.Editable = true;
                        ButtonSBOAjusteCantidad.ItemSBO.Enabled = true;
                    }
                    else
                    {
                        MatrixRequisiciones.ColumnaCantidadAjuste.Columna.Editable = false;
                        ButtonSBOAjusteCantidad.ItemSBO.Enabled = false;
                    }
                }

                estado = Localize(new InformacionLineaRequisicion { CodigoEstado = (int)estadoFormulario }, TipoMensaje.EstadoFormulario, estadoFormulario.ToString());
                EditTextEstado.AsignaValorDataSource(estado);
                formDataSource.SetValue("U_SCGD_CodEst", 0, ((int)estadoFormulario).ToString());
            }
        }

        #endregion

        #region "Item Pressed"
        
        protected virtual void ButtonSBOTrasladarItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            ICompany company = CompanySBO;
            SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(formUid);
            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
            List<InformacionLineaRequisicion> informacionLineasRequisicions = MatrixRequisiciones.SelectedRows2Collection();
            InformacionLineaRequisicion info = new InformacionLineaRequisicion();
            Documents cotizacion;
            cotizacion = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
            List<LineasCotizacion> lineas;
            var encabezado = EncabezadoRequisicionFromDBDataSource();
            string error = string.Empty;
            bool boolGenerarRollback = false;
            int codigoError = 0;
            string mensajeError = string.Empty;
            string strDocEntry = string.Empty;
            NumberFormatInfo n;
            n = DIHelper.GetNumberFormatInfo(CompanySBO);
            string strActualizaDocumentos = string.Empty;
            bool boolActualizarDocumentos = true;
            
            //General service para actualizar el UDO
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData oReqMovLinea;
            SAPbobsCOM.GeneralData oReqLinea;
            SAPbobsCOM.GeneralData oRequisicion;
            SAPbobsCOM.GeneralDataCollection oChildrenMovimientos;
            SAPbobsCOM.GeneralDataCollection oChildrenLineasReq;

            int codEstRequisicion = -1;
            string txtEstadoRequisicion = string.Empty;


            if (pVal.BeforeAction)
            {
                if (informacionLineasRequisicions != null && informacionLineasRequisicions.Count > 0)
                {
                    //if (encabezado.TipoRequisicion.Contains("Trans") || encabezado.TipoRequisicion.Contains("Res") || encabezado.CodigoTipoRequisicion == 3)
                    //if (encabezado.TipoRequisicion.Contains("Trans") || encabezado.CodigoTipoRequisicion == 3)
                    //{
                    //    if (!ValidaCotizacionAbierta(formUid))
                    //    {
                    //        error = Resource.txtErrorCotizacionNoAbierta;
                    //    }
                    //    else if (!ValidaOTAbierta(formUid))
                    //    {
                    //        error = Resource.txtErrorOTNoAbierta;
                    //    }
                    //}

                    if (cotizacion.GetByKey(informacionLineasRequisicions[0].DocumentoOrigen) )
                    {
                        if (encabezado.TipoRequisicion.Contains("Trans") || encabezado.CodigoTipoRequisicion == 3)
                        {
                            if (cotizacion.DocumentStatus == BoStatus.bost_Close)
                            {
                                error = Resource.txtErrorCotizacionNoAbierta;
                            }
                            else if (!ValidaOTAbierta(formUid))
                            {
                                error = Resource.txtErrorOTNoAbierta;
                            }
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            g_NoSerieCita = cotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString();
                            g_NoCita = cotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value.ToString();
                            lineas = CargarLineasCotizacion(cotizacion.Lines);

                            foreach (var inf in informacionLineasRequisicions)
                            {

                                //if (lineas.Any(x => x.LineNum == inf.LineNumOrigen) && (encabezado.TipoRequisicion.Contains("Trans") || encabezado.TipoRequisicion.Contains("Res")))
                                if (lineas.Any(x => x.LineNum == inf.LineNumOrigen) && (encabezado.TipoRequisicion.Contains("Trans") || encabezado.CodigoTipoRequisicion == 3))
                                {
                                    if (lineas.First(x => x.LineNum == inf.LineNumOrigen).Aprobado == 2)
                                    {
                                        error = string.Format(Resource.AprobadoNo, inf.DataSourceOffset + 1);
                                        error = Localize(inf, TipoMensaje.NoSePuedenBodegasIguales, error);
                                        break;
                                    }
                                    else if (lineas.First(x => x.LineNum == inf.LineNumOrigen).Trasladado == 2)
                                    {
                                        error = string.Format(Resource.YaSeTraslado, inf.DataSourceOffset + 1);
                                        error = Localize(inf, TipoMensaje.NoSePuedenBodegasIguales, error);
                                        break;
                                    }
                                }
                                if (manejadorArticulos.CantidadDisponibleItemEspecifico(inf.CodigoArticulo, inf.CodigoBodegaOrigen))
                                {
                                    error = string.Format(Resource.txtErrorNoTrasReq, inf.DataSourceOffset + 1, inf.CodigoBodegaOrigen);
                                    error = Localize(inf, TipoMensaje.NoSePuedenBodegasIguales, error);
                                    break;
                                }
                                if (inf.CodigoBodegaOrigen == inf.CodigoBodegaDestino)
                                {
                                    error = string.Format(Resource.txtErrorBodegasIguales, inf.CodigoBodegaDestino, inf.CodigoBodegaOrigen);
                                    error = Localize(inf, TipoMensaje.NoSePuedenBodegasIguales, error);
                                    break;
                                }
                                if ((EstadosLineas)inf.CodigoEstado != EstadosLineas.Pendiente)
                                {
                                    error = string.Format(Resource.txtErrorTraslLinea, inf.DataSourceOffset + 1);
                                    error = Localize(inf, TipoMensaje.ErrorNoSePuedeTrasladar, error);
                                    break;
                                }
                                if (inf.CantidadATransferir == 0.0 || inf.CantidadATransferir > inf.CantidadPendiente)
                                {
                                    error = string.Format(Resource.txtErrorTrasQti, inf.DataSourceOffset + 1);
                                    error = Localize(inf, TipoMensaje.MayorQueCantidadPendiente, error);
                                    break;
                                }
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_UsaUbicD.Trim().Equals("Y"))
                                {
                                    info = inf;
                                    error = ValidaUbicacionesLinea(ref info, ref pVal);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        break;
                                    }
                                }
                            }
                        }                      
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        bubbleEvent = false;
                    }
                }
                else
                {
                    ApplicationSBO.StatusBar.SetText(Resource.SeleccioneLinea, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    bubbleEvent = false;
                }
            }
            else
            {
                try
                {
                    EncabezadoRequisicion encabezadoRequisicion = EncabezadoRequisicionFromDBDataSource();
                    Requisicion.LineasRequisicion = informacionLineasRequisicions;
                    Requisicion.EncabezadoRequisicion = encabezadoRequisicion;

                    //Inicia el bloque de transacciones
                    if (!company.InTransaction)
                    {
                        company.StartTransaction();
                    }

                    //Instancia un objecto de tipo General Service con el DocEntry de la requisición abierta
                    //y sus tablas hijas "@SCGD_LINEAS_REQ" , "@SCGD_MOVS_REQ".
                    var dataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado);
                    strDocEntry = dataSource.GetValue("DocEntry", 0);
                    oCompanyService = CompanySBO.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");
                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("DocEntry", strDocEntry);
                    oRequisicion = oGeneralService.GetByParams(oGeneralParams);
                    oChildrenMovimientos = oRequisicion.Child("SCGD_MOVS_REQ");
                    oChildrenLineasReq = oRequisicion.Child("SCGD_LINEAS_REQ");

                    strActualizaDocumentos = dataSource.GetValue("U_ActualizaDoc", 0);

                    if (strActualizaDocumentos.ToUpper().Trim() == "N")
                    { 
                        //No se debe actualizar la cotización, ni la OT, 
                        //solamente se procesa la requisición y se realiza el traslado de stocks.
                        boolActualizarDocumentos = false;

                    }
                    
                    List<TransferenciaLineasBase> lineasTransferidas = Requisicion.Traslada(g_NoSerieCita, g_NoCita);

                    foreach (var resultadoTransferencias in lineasTransferidas)
                    {
                        if (!resultadoTransferencias.HuboError)
                        {
                            foreach (var lineaTransferida in resultadoTransferencias.InformacionLineasRequisicion)
                            {
                                InformacionLineasMovimientos informacionLineasMovimientos = new InformacionLineasMovimientos();
                                resultadoTransferencias.CopyToInformacionLineasMovimientos(informacionLineasMovimientos);
                                informacionLineasMovimientos.CodigoArticulo = lineaTransferida.CodigoArticulo;
                                informacionLineasMovimientos.DescripcionArticulo = lineaTransferida.DescripcionArticulo;
                                informacionLineasMovimientos.CantidadTransferida = lineaTransferida.CantidadATransferir;

                                //Verifica si la primer línea de la tabla hija "@SCGD_MOVS_REQ" esta en blanco
                                //de ser así se utiliza, de lo contrario se agrega una línea nueva.
                                if (oChildrenMovimientos.Count == 1)
                                {
                                    oReqMovLinea = oChildrenMovimientos.Item(0);

                                    string strCodArticulo = oReqMovLinea.GetProperty("U_SCGD_CodArticulo").ToString().Trim();
                                    string strDocEntryTmp = oReqMovLinea.GetProperty("U_SCGD_DocEntry").ToString().Trim();
                                    if (strDocEntryTmp != "0" && strCodArticulo != "-1")
                                    {
                                        oReqMovLinea = oChildrenMovimientos.Add();
                                    }
                                }
                                else
                                {
                                    oReqMovLinea = oChildrenMovimientos.Add();
                                }

                                //Completa la información de las columnas de la tabla hija "@SCGD_MOVS_REQ" con los datos de la transferencia realizada
                                oReqMovLinea.SetProperty("U_SCGD_CodArticulo", informacionLineasMovimientos.CodigoArticulo);
                                oReqMovLinea.SetProperty("U_SCGD_DescArticulo", informacionLineasMovimientos.DescripcionArticulo);
                                oReqMovLinea.SetProperty("U_SCGD_DocEntry", informacionLineasMovimientos.CodigoDocumento.ToString());
                                oReqMovLinea.SetProperty("U_SCGD_DocNum", informacionLineasMovimientos.NumeroDocumento.ToString());
                                oReqMovLinea.SetProperty("U_SCGD_TipoDoc", informacionLineasMovimientos.TipoDocumento);
                                oReqMovLinea.SetProperty("U_SCGD_CantTransf", informacionLineasMovimientos.CantidadTransferida);
                                oReqMovLinea.SetProperty("U_SCGD_FechaDoc", informacionLineasMovimientos.Fecha);

                                lineaTransferida.CantidadRecibida += informacionLineasMovimientos.CantidadTransferida;
                                lineaTransferida.CantidadPendiente = lineaTransferida.CantidadSolicitada - lineaTransferida.CantidadRecibida;
                                double redondeoCantidadRecibida = Math.Round(lineaTransferida.CantidadRecibida, n.NumberDecimalDigits);
                                double redondeoCantidadPendiente = Math.Round(lineaTransferida.CantidadPendiente, n.NumberDecimalDigits);
                                lineaTransferida.CantidadRecibida = redondeoCantidadRecibida;
                                lineaTransferida.CantidadPendiente = redondeoCantidadPendiente;

                                //Completa la información de la línea para la tabla hija "@SCGD_LINEAS_REQ"
                                oReqLinea = oChildrenLineasReq.Item(lineaTransferida.DataSourceOffset);
                                oReqLinea.SetProperty("U_SCGD_CantRec", lineaTransferida.CantidadRecibida);
                                oReqLinea.SetProperty("U_SCGD_CantPen", lineaTransferida.CantidadPendiente);
                                oReqLinea.SetProperty("U_SCGD_CantATransf", 0);
                                oReqLinea.SetProperty("U_FechaM", DateTime.Now);
                                oReqLinea.SetProperty("U_HoraM", DateTime.Now);
                                oReqLinea.SetProperty("U_TipoM", "1");
                                oReqLinea.SetProperty("U_DeUbic", lineaTransferida.DeUbicacion);
                                oReqLinea.SetProperty("U_AUbic", lineaTransferida.AUbicacion);

                                //Guarda la cantidad disponible al momento de generar algún movimiento sobre la línea
                                //este valor es informativo y se actualiza cada vez que se abre el documento
                                double cantidadDisponible = 0;
                                cantidadDisponible = ObtenerCantidadDisponible(informacionLineasMovimientos.CodigoArticulo, oReqLinea.GetProperty("U_SCGD_CodBodOrigen").ToString());
                                oReqLinea.SetProperty("U_SCGD_CantDispo",cantidadDisponible);

                                //Guarda el estado de la línea
                                int codEstadoLinea = EstadoLineaRequisicion(oReqLinea);
                                string txtEstadoLinea = string.Empty;

                                if (codEstadoLinea != -1)
                                {
                                    oReqLinea.SetProperty("U_SCGD_CodEst", codEstadoLinea);
                                    EstadosLineas estadoLinea = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), codEstadoLinea.ToString());
                                    txtEstadoLinea = Localize(new InformacionLineaRequisicion { CodigoEstado = codEstadoLinea }, TipoMensaje.EstadoFormulario, estadoLinea.ToString());
                                    if (!string.IsNullOrEmpty(txtEstadoLinea))
                                    {
                                        oReqLinea.SetProperty("U_SCGD_Estado", txtEstadoLinea);
                                    }
                                }
                                
                                CheckBoxSelTodo.AsignaValorUserDataSource("N");
                            }
                        }
                        else
                        {
                            boolGenerarRollback = true;
                            ApplicationSBO.StatusBar.SetText(resultadoTransferencias.Error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }

                    //Completa la información de los encabezados de la requisición
                    codEstRequisicion = EstadoRequisicion(oChildrenLineasReq);
                    
                    if (codEstRequisicion != -1)
                    {
                        EstadosLineas estadoFormulario = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), codEstRequisicion.ToString());
                        oRequisicion.SetProperty("U_SCGD_CodEst", codEstRequisicion);
                        txtEstadoRequisicion = Localize(new InformacionLineaRequisicion { CodigoEstado = codEstRequisicion },TipoMensaje.EstadoFormulario, estadoFormulario.ToString());
                        if (!string.IsNullOrEmpty(txtEstadoRequisicion))
                        {
                            oRequisicion.SetProperty("U_SCGD_Est", txtEstadoRequisicion);
                        }
                    }
                   
                    //----------------------------------------------------------
                    //Actualiza la oferta de ventas y la transferencia de stocks
                    //----------------------------------------------------------
                    if (boolGenerarRollback == false)
                    {

                        oGeneralService.Update(oRequisicion);
                        bool EsDevolucion = false;

                        if ((!encabezado.TipoRequisicion.Contains("Trans") && !(encabezado.CodigoTipoRequisicion == 3)) || encabezado.CodigoTipoRequisicion == 2 || encabezado.CodigoTipoRequisicion == 4)
                        {
                            EsDevolucion = true;
                        }

                        if (m_blnActualizaCot && boolActualizarDocumentos)
                        {
                            ActualizaCotizacion(lineasTransferidas[0].InformacionLineasRequisicion[0].DocumentoOrigen.ToString(), oChildrenLineasReq, ref codigoError, ref mensajeError, false, EsDevolucion);
                            
                        }
                        if (codigoError != 0)
                        {
                            boolGenerarRollback = true;
                        }
                        else
                        {
                            if (m_blnActualizaCot && boolActualizarDocumentos)
                            {
                                ActualizaTransferencias(oChildrenMovimientos, ref codigoError, ref mensajeError);
                                m_blnActualizaCot = false;
                            }
                            if (codigoError != 0)
                            {
                                boolGenerarRollback = true;
                            }
                            else
                            {
                                if (TrasladoRealizado != null && boolActualizarDocumentos)
                                {
                                    TrasladoRealizado(lineasTransferidas, encabezadoRequisicion.TipoRequisicion, ref codigoError, ref mensajeError);
                                }
                                if (codigoError != 0)
                                {
                                    boolGenerarRollback = true;
                                }
                            }
                            
                        }
                    }
                    
                    //True = Se revierten los cambios. False = Se aplican los cambios
                    if (boolGenerarRollback == true)
                    {
                        if (!string.IsNullOrEmpty(mensajeError))
                        {
                            ApplicationSBO.StatusBar.SetText(mensajeError, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                        
                        //Se produjo algún tipo de error o excepción y se procede a revertir los cambios
                        if (company.InTransaction)
                        {
                            company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                        
                    }
                    else
                    {
                        //Proceso exitoso, se procede a aplicar los cambios en la base de datos
                        if (company.InTransaction)
                        {
                            company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }

                        CargaRequisicion(strDocEntry);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                    //Revierte los cambios realizados
                    if (company.InTransaction)
                    {
                        company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                }
            }
        }

        public void CargaRequisicion(string strReqDocEntry)
        {
            SAPbouiCOM.Conditions oConditions;
            SAPbouiCOM.Condition oCondition;

            try
            {
                if (FormularioSBO != null)
                {
                    FormularioSBO.Freeze(true);
                    oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    oCondition = oConditions.Add();

                    oCondition.Alias = "DocEntry";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = strReqDocEntry;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_MOVS_REQ").Query(oConditions);
                    //ManejadorEventoFormDataLoad((SAPbouiCOM.Form)FormularioSBO);

                    MatrixRequisiciones.Matrix.LoadFromDataSource();
                    MatrixMovimientos.Matrix.LoadFromDataSource();

                    FormularioSBO.Refresh();
                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                    ActualizaLineasAlCargar();

                    MatrixRequisiciones.Especifico.LoadFromDataSource();
                    MatrixMovimientos.EliminaPrimeraLinea();
                    MatrixMovimientos.Especifico.LoadFromDataSource();
                    
                    
                    CargarObjRequisicion();

                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void CargarObjRequisicion()
        {
            try
            {
                if (FormularioSBO != null)
                {
                    oRequisicionData = new RequisicionData();
                    oRequisicionData.CodigoCliente = EditTextCodigoCliente.ObtieneValorDataSource();
                    oRequisicionData.NombreCliente = EditTextNombreCliente.ObtieneValorDataSource();
                    oRequisicionData.DocEntry = int.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("DocEntry", 0));
                    oRequisicionData.DocNum = Convert.ToInt32(EditTextNoRequisicion.ObtieneValorDataSource());
                    oRequisicionData.CreateDate = DateTimeFromString(EditTextFecha.ObtieneValorDataSource(), EditTextHora.ObtieneValorDataSource());
                    oRequisicionData.NoOrden = EditTextNoOrden.ObtieneValorDataSource();
                    oRequisicionData.Usuario = EditTextUsuario.ObtieneValorDataSource();
                    oRequisicionData.TipoDocumento = EditTextTipoDocumento.ObtieneValorDataSource();
                    oRequisicionData.TipoRequisicion = EditTextTipoRequisicion.ObtieneValorDataSource();
                    oRequisicionData.Data = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Data", 0);
                    oRequisicionData.Comentario = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comm", 0);
                    oRequisicionData.ComentariosUser = EditTextComentariosUsuario.ObtieneValorDataSource();// FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comen", 0);
                    oRequisicionData.Placa = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Placa", 0).Trim();
                    oRequisicionData.Marca = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Marca", 0).Trim();
                    oRequisicionData.Estilo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Estilo", 0).Trim();
                    oRequisicionData.VIN = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_VIN", 0).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_Serie", 0)))
                        oRequisicionData.Serie = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_Serie", 0).Trim());
                    oRequisicionData.CodigoTipoRequisicion = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_CodTipoReq", 0).Trim());
                    oRequisicionData.SucursalID = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_IDSuc", 0).Trim();
                    oRequisicionData.EstadoRequisicion = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_CodEst", 0).Trim();

                    oRequisicionData.LineasRequisicion = CargarLineasReq();
                }
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        private List<LineaRequisicion> CargarLineasReq()
        {
            List<LineaRequisicion> lstLineas = new List<LineaRequisicion>();
            LineaRequisicion linea;
            NumberFormatInfo n;
            n = DIHelper.GetNumberFormatInfo(CompanySBO);

            try
            {
                for (int i = 0; i <= FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).Size - 1; i++)
                {
                    linea = new LineaRequisicion();
                    linea.DataSourceOffset = i;
                    linea.DocEntry = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("DocEntry", i).Trim());
                    linea.LineId = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("LineId", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_AUbic", i)))
                        linea.U_AUbic = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_AUbic", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_DeUbic", i)))
                        linea.U_DeUbic = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_DeUbic", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_FechaM", i)))
                        linea.U_FechaM = DateTime.ParseExact(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_FechaM", i).Trim(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_HoraM", i)))
                        linea.U_HoraM = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_HoraM", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_Obs_Req", i)))
                        linea.U_Obs_Req = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_Obs_Req", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_ReqOriPen", i)))
                        linea.U_ReqOriPen = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_ReqOriPen", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CAju", i)))
                        linea.U_SCGD_CAju = Convert.ToDouble(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CAju", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CCosto", i)))
                        linea.U_SCGD_CCosto = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CCosto", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_COrig", i)))
                        linea.U_SCGD_COrig = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_COrig", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantATransf", i)))
                        linea.U_SCGD_CantATransf = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantATransf", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantDispo", i)))
                        linea.U_SCGD_CantDispo = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantDispo", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantPen", i)))
                        linea.U_SCGD_CantPen = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantPen", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantRec", i)))
                        linea.U_SCGD_CantRec = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantRec", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantSol", i)))
                        linea.U_SCGD_CantSol = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantSol", i).Trim(), n);
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Chk", i)))
                        linea.U_SCGD_Chk = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Chk", i).Trim() == "0" ? 0 : 1;
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodArticulo", i)))
                        linea.U_SCGD_CodArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodArticulo", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodDest", i)))
                        linea.U_SCGD_CodBodDest = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodDest", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodOrigen", i)))
                        linea.U_SCGD_CodBodOrigen = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodOrigen", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodEst", i)))
                        linea.U_SCGD_CodEst = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodEst", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodTipoArt", i)))
                        linea.U_SCGD_CodTipoArt = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodTipoArt", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DescArticulo", i)))
                        linea.U_SCGD_DescArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DescArticulo", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DocOr", i)))
                        linea.U_SCGD_DocOr = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DocOr", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Estado", i)))
                        linea.U_SCGD_Estado = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Estado", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_ID", i)))
                        linea.U_SCGD_ID = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_ID", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_LNumOr", i)))
                        linea.U_SCGD_LNumOr = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_LNumOr", i).Trim());
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Lidsuc", i)))
                        linea.U_SCGD_Lidsuc = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Lidsuc", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_TipoArticulo", i)))
                        linea.U_SCGD_TipoArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_TipoArticulo", i).Trim();
                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_TipoM", i)))
                        linea.U_TipoM = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_TipoM", i).Trim());

                    linea.VisOrder = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("VisOrder", i).Trim());
                    lstLineas.Add(linea);
                }
                return lstLineas;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstLineas;
        }

        /// <summary>
        /// Obtiene la cantidad disponible del artículo en la bodega
        /// </summary>
        /// <param name="strItemCode">Código del artículo, campo "ItemCode" de la tabla "OITM".</param>
        /// <param name="strBodega">Código de la bodega en formato texto.</param>
        /// <returns>Cantidad disponible en la bodega en formato "double".</returns>
        private double ObtenerCantidadDisponible(string strItemCode, string strBodega)
        {
            ICompany company = CompanySBO;
            ManejadorArticulos mnjArticulos = new ManejadorArticulos(company);
            double cantidadDisponible = 0;

            try
            {
                mnjArticulos.ItemCode = strItemCode;
                mnjArticulos.WhsCode = strBodega;
                cantidadDisponible = Convert.ToDouble(mnjArticulos.CantidadDisponible());
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                return 0;
            }

            return cantidadDisponible;
        }

        /// <summary>
        /// Devuelve el código del estado de la línea después de recalcular las cantidades recibidas.
        /// </summary>
        /// <param name="oLineaRequisicion">Línea de la requisición.</param>
        /// <returns>Código del estado de la línea en formato entero.</returns>
        private int EstadoLineaRequisicion(SAPbobsCOM.GeneralData oLineaRequisicion)
        {
            int codEstadoLinea = -1;
            int numLinea = 1;
            float cantidadPendiente = 0;
            ICompany company = CompanySBO;
            NumberFormatInfo numberFormatInfo = DIHelper.GetNumberFormatInfo(company);
            ManejadorEstadoLinea mnjEstadoLinea = new ManejadorEstadoLinea(company);

            try
            {
                numLinea = Convert.ToInt32(oLineaRequisicion.GetProperty("LineId"));
                mnjEstadoLinea.CantidadSolicitada = float.Parse(oLineaRequisicion.GetProperty("U_SCGD_CantSol").ToString(), numberFormatInfo);
                mnjEstadoLinea.CantidadRecibida = float.Parse(oLineaRequisicion.GetProperty("U_SCGD_CantRec").ToString(),numberFormatInfo);
                cantidadPendiente = mnjEstadoLinea.CantidadSolicitada - mnjEstadoLinea.CantidadRecibida;
                mnjEstadoLinea.EstadoActual = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), oLineaRequisicion.GetProperty("U_SCGD_CodEst").ToString());
                mnjEstadoLinea.CalculaEstado();
                codEstadoLinea = (int)Enum.Parse(typeof(EstadosLineas), mnjEstadoLinea.EstadoActual.ToString());
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message + " Line: " + numLinea, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                return -1;
            }

            return codEstadoLinea;
        }

        /// <summary>
        /// Recorre las líneas de la tabla hija y determina el estado general de la requisición
        /// </summary>
        /// <param name="oChildrenLineasReq">Colección con las líneas de la tabla "@SCGD_LINEAS_REQ"</param>
        /// <returns>Código del estado de la requisición en formato entero</returns>
        private int EstadoRequisicion(SAPbobsCOM.GeneralDataCollection oChildrenLineasReq)
        {
            int codEstadoRequisicion = -1;
            int codEstadoLinea = -1;
            int numLinea = 1;
            bool TodasCanceladas = true;
            bool PendientesTraslado = false;
            
            try
            {
                //Revisa los estados de las líneas
                if (oChildrenLineasReq.Count > 0)
                {
                    foreach (SAPbobsCOM.GeneralData LineaRequisicion in oChildrenLineasReq)
                    {
                        numLinea = Convert.ToInt32(LineaRequisicion.GetProperty("LineId"));
                        codEstadoLinea = Convert.ToInt32(LineaRequisicion.GetProperty("U_SCGD_CodEst"));
                        if (codEstadoLinea == (int)EstadosLineas.Pendiente)
                        {
                            PendientesTraslado = true;
                            TodasCanceladas = false;
                        }
                        else if (codEstadoLinea == (int)EstadosLineas.Trasladado)
                        {
                            TodasCanceladas = false;
                        }
                    }


                    //Determina el estado general de la requisición
                    if (PendientesTraslado)
                    {
                        //Pendiente
                        //Existe una o más líneas pendientes de traslado
                        codEstadoRequisicion = (int)EstadosLineas.Pendiente;
                    }
                    else if (TodasCanceladas)
                    {
                        //Cancelado
                        //Todas las líneas están canceladas
                        codEstadoRequisicion = (int)EstadosLineas.Cancelado;
                    }
                    else 
                    {
                        //Trasladado
                        //Una o más líneas trasladadas sin pendientes
                        codEstadoRequisicion = (int)EstadosLineas.Trasladado;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message + " Line: " + numLinea, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                return -1;
            }

            return codEstadoRequisicion;
        }

        private List<LineasCotizacion> CargarLineasCotizacion(Document_Lines lines)
        {
            List<LineasCotizacion> lineas;
            LineasCotizacion line;
            lineas = new List<LineasCotizacion>();
            for (int index = 0; index <= lines.Count - 1; index++)
            {
                lines.SetCurrentLine(index);
                line = new LineasCotizacion();
                line.Aprobado = (int)lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;
                line.Trasladado = (int)lines.UserFields.Fields.Item("U_SCGD_Traslad").Value;
                line.LineNum = lines.LineNum;
                lineas.Add(line);
            }
            return lineas;
        }

        public Boolean ValidaCotizacionAbierta(string formUid)
        {
            bool result = true;
            try
            {
                SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(formUid);
                bool usaConfOTSap = Utilitarios.ValidaUsaOTSap();

                if (usaConfOTSap)
                {
                    String strNumOT = ((SAPbouiCOM.EditText)oForm.Items.Item("edtNoOrden").Specific).Value.Trim();
                    String query = "select DocStatus from OQUT WITH (nolock) where U_SCGD_Numero_OT = '{0}'";
                    query = string.Format(query, strNumOT);
                    dtLocal = oForm.DataSources.DataTables.Item("dtLocal");
                    dtLocal.ExecuteQuery(query);
                    if (dtLocal.Rows.Count > 0)
                    {
                        if (dtLocal.GetValue("DocStatus", 0).ToString().Trim() == "C")
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            return result;
        }

        public Boolean ValidaOTAbierta(string formUid)
        {
            bool result = true;
            try
            {
                SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(formUid);
                String strNumOT = ((SAPbouiCOM.EditText)FormularioSBO.Items.Item("edtNoOrden").Specific).Value.Trim();
                String idSucursal = ((SAPbouiCOM.ComboBox)FormularioSBO.Items.Item("cboSucur").Specific).Value.Trim();
                dtLocal = oForm.DataSources.DataTables.Item("dtLocal");
                bool usaConfOTSap = Utilitarios.ValidaUsaOTSap();
                var estado = string.Empty;

                if (!string.IsNullOrEmpty(strNumOT))
                {
                    if (usaConfOTSap)
                    {
                        var queryConf = "select U_ValReqPen from [@SCGD_CONF_SUCURSAL] WITH (nolock) where U_Sucurs = '{0}'";
                        queryConf = string.Format(queryConf, idSucursal);
                        dtLocal.ExecuteQuery(queryConf);
                        if (dtLocal.GetValue(0, 0).ToString().Trim() == "Y")
                        {
                            var query = "select U_EstO from [@SCGD_OT] WITH (nolock)  where U_NoOT = '{0}' ";
                            query = string.Format(query, strNumOT);
                            dtLocal.ExecuteQuery(query);
                            if (dtLocal.Rows.Count > 0)
                            {
                                estado = dtLocal.GetValue("U_EstO", 0).ToString().Trim();
                                if (estado != "1" && estado != "2" && estado != "3")
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                result = false;
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            return result;
        }

        //manejo del boton de ajuste de cantidades en la requisicion y en la cotizacion
        protected virtual void ButtonDeAjusteCantidades(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            ICompany company = CompanySBO;
            SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(formUid);
            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
            List<InformacionLineaRequisicion> informacionLineasRequisiciones;
            InformacionLineaRequisicion info = new InformacionLineaRequisicion();
            Documents cotizacion;
            cotizacion = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
            List<LineasCotizacion> lineas;
            var encabezado = EncabezadoRequisicionFromDBDataSource();
            string error = string.Empty;
            bool boolGenerarRollback = false;
            int codigoError = 0;
            string mensajeError = string.Empty;
            string strDocEntry = string.Empty;
            List<TransferenciaLineasBase> lineasTransferidas = new List<TransferenciaLineasBase>();
            bool boolActualizarDocumentos = true;
            string strActualizaDocumentos = string.Empty;

            //General service para actualizar el UDO
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData oReqMovLinea;
            SAPbobsCOM.GeneralData oReqLinea;
            SAPbobsCOM.GeneralData oRequisicion;
            SAPbobsCOM.GeneralDataCollection oChildrenMovimientos;
            SAPbobsCOM.GeneralDataCollection oChildrenLineasReq;

            NumberFormatInfo n;
            n = DIHelper.GetNumberFormatInfo(CompanySBO);

            int codEstRequisicion = -1;
            string txtEstadoRequisicion = string.Empty;

            try
            {
                if (!pVal.BeforeAction)
                {
                    bool blnUsaConfiguracionInternaTaller = Utilitarios.ValidaUsaOTSap();

                    if (!blnUsaConfiguracionInternaTaller)
                    {
                        AjusteCantidadesTallerAfuera(formUid, pVal, ref bubbleEvent);
                    }
                    else
                    {
                        EncabezadoRequisicion encabezadoRequisicion = EncabezadoRequisicionFromDBDataSource();
                        Requisicion.EncabezadoRequisicion = encabezadoRequisicion;
                        informacionLineasRequisiciones = MatrixRequisiciones.SelectedRows2Collection();
                        Requisicion.LineasRequisicion = informacionLineasRequisiciones;

                        //Inicia el bloque de transacciones
                        if (!company.InTransaction)
                        {
                            company.StartTransaction();
                        }

                        //Instancia un objecto de tipo General Service con el DocEntry de la requisición abierta
                        //y sus tablas hijas "@SCGD_LINEAS_REQ" , "@SCGD_MOVS_REQ".
                        var dataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado);
                        strDocEntry = dataSource.GetValue("DocEntry", 0);
                        strActualizaDocumentos = dataSource.GetValue("U_ActualizaDoc", 0);
                        oCompanyService = CompanySBO.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");
                        oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                        oGeneralParams.SetProperty("DocEntry", strDocEntry);
                        oRequisicion = oGeneralService.GetByParams(oGeneralParams);
                        oChildrenLineasReq = oRequisicion.Child("SCGD_LINEAS_REQ");

                        if (strActualizaDocumentos.ToUpper().Trim() == "N")
                        {
                            //No se debe actualizar la cotización, ni la OT, 
                            //solamente se procesa la requisición y se realiza el traslado de stocks.
                            boolActualizarDocumentos = false;
                        }
                        
                        if (informacionLineasRequisiciones != null)
                        {
                            foreach (InformacionLineaRequisicion lineaRequisicion in informacionLineasRequisiciones)
                            {
                                EstadosLineas estadoLinea = (EstadosLineas)lineaRequisicion.CodigoEstado;
                                double cantidadSolicitada = 0;
                                double cantidadPendiente = 0;

                                //Completa la información de la línea para la tabla hija "@SCGD_LINEAS_REQ"
                                oReqLinea = oChildrenLineasReq.Item(lineaRequisicion.LineId - 1);

                                //Las líneas trasladadas no requieren ajustes de cantidades
                                if (estadoLinea != EstadosLineas.Trasladado)
                                {
                                    cantidadSolicitada = lineaRequisicion.CantidadSolicitada;
                                    cantidadPendiente = lineaRequisicion.CantidadPendiente;

                                    if (lineaRequisicion.CantidadAjuste < lineaRequisicion.CantidadSolicitada && lineaRequisicion.CantidadAjuste < lineaRequisicion.CantidadPendiente)
                                    {
                                        cantidadSolicitada = lineaRequisicion.CantidadSolicitada - lineaRequisicion.CantidadAjuste;
                                        cantidadPendiente = cantidadSolicitada - lineaRequisicion.CantidadRecibida;
                                    }
                                    else if (lineaRequisicion.CantidadAjuste == lineaRequisicion.CantidadPendiente)
                                    {
                                        cantidadSolicitada = lineaRequisicion.CantidadSolicitada - lineaRequisicion.CantidadAjuste;
                                        cantidadPendiente = cantidadSolicitada - lineaRequisicion.CantidadRecibida;
                                    }
                                    else if (lineaRequisicion.CantidadAjuste > lineaRequisicion.CantidadPendiente)
                                    {
                                        //Error, se intenta realizar un ajuste por una cantidad superior a la pendiente
                                        mensajeError = string.Format(Resource.txtCntAjusteExcede, lineaRequisicion.LineId);
                                        boolGenerarRollback = true;
                                    }

                                    if (cantidadSolicitada < 0)
                                    {
                                        cantidadSolicitada = 0;
                                    }

                                    if (cantidadPendiente < 0)
                                    {
                                        cantidadPendiente = 0;
                                    }

                                    //Completa la información de la línea para la tabla hija "@SCGD_LINEAS_REQ"
                                    oReqLinea.SetProperty("U_SCGD_CantSol", cantidadSolicitada);
                                    oReqLinea.SetProperty("U_SCGD_CantPen", cantidadPendiente);
                                    oReqLinea.SetProperty("U_SCGD_CantATransf", 0);
                                    oReqLinea.SetProperty("U_FechaM", DateTime.Now);
                                    oReqLinea.SetProperty("U_HoraM", DateTime.Now);
                                    oReqLinea.SetProperty("U_TipoM", "3");

                                    //Guarda la cantidad disponible al momento de generar algún movimiento sobre la línea
                                    //este valor es informativo y se actualiza cada vez que se abre el documento
                                    double cantidadDisponible = 0;
                                    cantidadDisponible = ObtenerCantidadDisponible(lineaRequisicion.CodigoArticulo, oReqLinea.GetProperty("U_SCGD_CodBodOrigen").ToString());
                                    oReqLinea.SetProperty("U_SCGD_CantDispo", cantidadDisponible);

                                    //Guarda el estado de la línea
                                    int codEstadoLinea = EstadoLineaRequisicion(oReqLinea);

                                    if (codEstadoLinea != -1)
                                    {
                                        string txtEstadoLinea = string.Empty;
                                        oReqLinea.SetProperty("U_SCGD_CodEst", codEstadoLinea);
                                        EstadosLineas estadoNuevo = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), codEstadoLinea.ToString());
                                        txtEstadoLinea = Localize(new InformacionLineaRequisicion { CodigoEstado = codEstadoLinea }, TipoMensaje.EstadoFormulario, estadoNuevo.ToString());
                                        if (!string.IsNullOrEmpty(txtEstadoLinea))
                                        {
                                            oReqLinea.SetProperty("U_SCGD_Estado", txtEstadoLinea);
                                        }
                                    }
                                }
                            }
                            CheckBoxSelTodo.AsignaValorUserDataSource("N");

                            //Completa la información de los encabezados de la requisición
                            codEstRequisicion = EstadoRequisicion(oChildrenLineasReq);

                            if (codEstRequisicion != -1)
                            {
                                EstadosLineas estadoFormulario = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), codEstRequisicion.ToString());
                                oRequisicion.SetProperty("U_SCGD_CodEst", codEstRequisicion);
                                txtEstadoRequisicion = Localize(new InformacionLineaRequisicion { CodigoEstado = codEstRequisicion },
                                          TipoMensaje.EstadoFormulario, estadoFormulario.ToString());
                                if (!string.IsNullOrEmpty(txtEstadoRequisicion))
                                {
                                    oRequisicion.SetProperty("U_SCGD_Est", txtEstadoRequisicion);
                                }
                            }
                            else
                            {
                                boolGenerarRollback = true;
                            }
                        }

                        //----------------------------------------------------------
                        //Actualiza la oferta de ventas y la transferencia de stocks
                        //----------------------------------------------------------
                        if (boolGenerarRollback == false)
                        {
                            oGeneralService.Update(oRequisicion);

                            int intDocEntry = Convert.ToInt32(strDocEntry.Trim());
                            bool EsDevolucion = false;
                            //if ((!encabezado.TipoRequisicion.Contains("Trans") && !encabezado.TipoRequisicion.Contains("Res"))  || encabezado.CodigoTipoRequisicion == 2)
                            if ((!encabezado.TipoRequisicion.Contains("Trans") && !(encabezado.CodigoTipoRequisicion == 3))  || encabezado.CodigoTipoRequisicion == 2 || encabezado.CodigoTipoRequisicion == 4)
                            {
                                EsDevolucion = true;
                            }

                            if (m_blnActualizaCot)
                            {
                                //ActualizaCotizacion(oChildrenLineasReq, ref codigoError, ref mensajeError, true, EsDevolucion);
                                m_blnActualizaCot = false;
                            }

                            if (boolActualizarDocumentos == true)
                            {
                                ActualizarCantidadAjusteCotizacion(oChildrenLineasReq, EsDevolucion, ref codigoError, ref mensajeError);
                            }
                          

                            if (codigoError != 0)
                            {
                                boolGenerarRollback = true;
                            }
                        }

                        //---------------------------------------------------------------
                        //Aplica los cambios en la base de datos
                        //---------------------------------------------------------------
                        //True = Se revierten los cambios. False = Se aplican los cambios
                        if (boolGenerarRollback == true)
                        {
                            ApplicationSBO.StatusBar.SetText(mensajeError, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            //Se produjo algún tipo de error o excepción y se procede a revertir los cambios
                            if (company.InTransaction)
                            {
                                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            }

                        }
                        else
                        {
                            //Proceso exitoso, se procede a aplicar los cambios en la base de datos
                            if (company.InTransaction)
                            {
                                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            }

                            //Refresca la pantalla con los datos actualizados
                            CargaRequisicion(strDocEntry);

                            ApplicationSBO.StatusBar.SetText("Ajuste Realizado Satisfactoriamente", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        }
                    
                    }
                }//Fin de If !BeforeAction

            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                //Revierte los cambios realizados
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
            }
        }

        protected virtual bool ActualizarCantidadAjusteCotizacion(SAPbobsCOM.GeneralDataCollection oChildrenLineasReq, bool EsDevolucion, ref int codigoError, ref string mensajeError)
        {
            ICompany company = CompanySBO;
            bool resultado = false;
            SAPbobsCOM.Documents oCotizacion;
            SAPbobsCOM.Document_Lines oLineasCotizacion;
            SAPbobsCOM.GeneralData oReqLinea;

            try
            {
                string strEntregado = CheckBoxEntregado.ObtieneValorDataSource();

                if (string.IsNullOrEmpty(strEntregado))
                {
                    strEntregado = "N";
                }

                string strNumOT = EditTextNoOrden.ObtieneValorDataSource();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();

                dtLocal.ExecuteQuery(string.Format("select DocEntry from [OQUT] WITH (NOLOCK) WHERE U_SCGD_Numero_OT = '{0}'", strNumOT));
                int intDocEntry = (int)dtLocal.GetValue("DocEntry", 0);

                bool blnUsaOTSAP = false;
                blnUsaOTSAP = Utilitarios.ValidaUsaOTSap();

                if (string.IsNullOrEmpty(strNumOT) && intDocEntry == 0)
                {
                    intDocEntry = (int)oChildrenLineasReq.Item(0).GetProperty("U_SCGD_DocOr");
                }

                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                if (oCotizacion.GetByKey(intDocEntry))
                {
                    oLineasCotizacion = oCotizacion.Lines;

                    for (int i = 0; i < oChildrenLineasReq.Count; i++)
                    {
                        oReqLinea = oChildrenLineasReq.Item(i);
                        for (int j = 0; j < oLineasCotizacion.Count; j++)
                        {
                            oLineasCotizacion.SetCurrentLine(j);
                            int numLinea = (int)oReqLinea.GetProperty("U_SCGD_LNumOr");
                            
                            //Verifica que la línea de la requisición corresponda a la línea de la cotización
                            if ((int)oReqLinea.GetProperty("U_SCGD_LNumOr") == oLineasCotizacion.LineNum)
                            {
                                //if (m_blnValidaEntregado)
                                //{
                                //    //oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Entregado").Value = strEntregado;
                                //}

                                if (blnUsaOTSAP)
                                {
                                    double cantidadRecibida = 0;
                                    double cantidadSolicitada = 0;

                                    cantidadRecibida = (double)oReqLinea.GetProperty("U_SCGD_CantRec");
                                    cantidadSolicitada = (double)oReqLinea.GetProperty("U_SCGD_CantSol");
                                    //Si la cantidad solicita es igual a la cantidad recibida, se cambia el estado de la línea a Trasladado
                                    if (cantidadRecibida == cantidadSolicitada && EsDevolucion == false)
                                    {   //Estado Trasladado
                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2;
                                    }

                                    //Ajusta la cantidad en la oferta de ventas
                                    oLineasCotizacion.Quantity = (double)oReqLinea.GetProperty("U_SCGD_CantSol");

                                    double cantidadPendiente = 0;
                                    cantidadPendiente = (double)oReqLinea.GetProperty("U_SCGD_CantPen");

                                    if (EsDevolucion)
                                    {
                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value = cantidadPendiente;
                                    }
                                    else
                                    {
                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = cantidadPendiente;
                                    }

                                    if (oReqLinea.GetProperty("U_Obs_Req").ToString().Trim() != oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString().Trim())
                                    {
                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = oReqLinea.GetProperty("U_Obs_Req").ToString().Trim();
                                    }
                                }
                            }
                        }
                    }

                    codigoError = oCotizacion.Update();

                    if (codigoError != 0)
                    {
                        mensajeError = company.GetLastErrorDescription();
                    }
                }


                return resultado;
            }
            catch (Exception ex)
            {
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                return resultado;
            }
        }

        //manejo del boton de ajuste de cantidades en la requisicion y en la cotizacion
        protected virtual void AjusteCantidadesTallerAfuera(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            string error = string.Empty;
            //bubbleEvent = true;
            if (!pVal.BeforeAction)
            {
                Boolean blnRealizarAjuste = false;
                List<InformacionLineaRequisicion> informacionLineasRequisicions = MatrixRequisiciones.SelectedRows2Collection();
                Requisicion.LineasRequisicion = informacionLineasRequisicions;
                EncabezadoRequisicion encabezadoRequisicion = EncabezadoRequisicionFromDBDataSource();
                Requisicion.LineasRequisicion = informacionLineasRequisicions;
                Requisicion.EncabezadoRequisicion = encabezadoRequisicion;
                List<TransferenciaLineasBase> lineasTransferidas = Requisicion.Traslada();

                if (informacionLineasRequisicions != null)
                {
                    foreach (var resultadoTransferencias in lineasTransferidas)
                    {
                        foreach (var lineaTransferida in resultadoTransferencias.InformacionLineasRequisicion)
                        {
                            EstadosLineas estadoLinea = (EstadosLineas)lineaTransferida.CodigoEstado;
                            if (estadoLinea != EstadosLineas.Trasladado)//  && linea.CantidadRecibida != 0)
                            {
                                if (lineaTransferida.CantidadAjuste < lineaTransferida.CantidadSolicitada && lineaTransferida.CantidadAjuste < lineaTransferida.CantidadPendiente)
                                {
                                    lineaTransferida.CantidadSolicitada = lineaTransferida.CantidadSolicitada - lineaTransferida.CantidadAjuste;
                                    MatrixRequisiciones.ColumnaCantidadSolicitada.AsignaValorDataSource(lineaTransferida.CantidadSolicitada, lineaTransferida.DataSourceOffset);
                                    lineaTransferida.CantidadPendiente = lineaTransferida.CantidadSolicitada - lineaTransferida.CantidadRecibida;
                                    MatrixRequisiciones.ColumnaCantidadPendiente.AsignaValorDataSource(lineaTransferida.CantidadPendiente, lineaTransferida.DataSourceOffset);
                                    MatrixRequisiciones.ColumnaCantidadATransferir.AsignaValorDataSource(0, lineaTransferida.DataSourceOffset);
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaFechaMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, DateTime.Now.ToString("yyyyMMdd"));
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaHoraMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, DateTime.Now.ToString("HHmm"));
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaTipoMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, "3");
                                    blnRealizarAjuste = true;
                                }

                                else if (lineaTransferida.CantidadAjuste == lineaTransferida.CantidadPendiente)
                                {
                                    lineaTransferida.CantidadSolicitada = lineaTransferida.CantidadSolicitada - lineaTransferida.CantidadAjuste;
                                    MatrixRequisiciones.ColumnaCantidadSolicitada.AsignaValorDataSource(lineaTransferida.CantidadSolicitada, lineaTransferida.DataSourceOffset);
                                    lineaTransferida.CantidadPendiente = lineaTransferida.CantidadSolicitada - lineaTransferida.CantidadRecibida;
                                    MatrixRequisiciones.ColumnaCantidadPendiente.AsignaValorDataSource(lineaTransferida.CantidadPendiente, lineaTransferida.DataSourceOffset);
                                    MatrixRequisiciones.ColumnaCantidadATransferir.AsignaValorDataSource(0, lineaTransferida.DataSourceOffset);
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaFechaMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, DateTime.Now.ToString("yyyyMMdd"));
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaHoraMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, DateTime.Now.ToString("HHmm"));
                                    MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada).SetValue(MatrixRequisiciones.ColumnaLineaTipoMovimiento.ColumnaLigada, lineaTransferida.DataSourceOffset, "3");
                                    blnRealizarAjuste = true;

                                }
                                else if (lineaTransferida.CantidadAjuste > lineaTransferida.CantidadPendiente)
                                {
                                    error = string.Format("La Cantidad de Ajuste excede a la cantidad pendiente. Linea {0}.",
                                                     lineaTransferida.DataSourceOffset + 1);

                                    error = Localize(lineaTransferida, TipoMensaje.NoSePuedeRealizarAjuste, error);
                                    ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                                    blnRealizarAjuste = false;
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(error))
                {
                    ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    bubbleEvent = false;
                }

                if (blnRealizarAjuste)
                {
                    ActualizaLineasAlCargar();
                    MatrixRequisiciones.Matrix.LoadFromDataSource();

                    ActualizaEnBD();
                    if (m_blnActualizaCot)
                    {
                        ActualizaCotizacion();
                        ActualizaTransferencias();
                    }
                    
                    if (AjusteCantidadRealizado != null)
                        AjusteCantidadRealizado(lineasTransferidas);
                    ApplicationSBO.StatusBar.SetText("Ajuste Realizado Satisfactoriamente", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                }

                CheckBoxSelTodo.AsignaValorUserDataSource("N");
            }
        }

        protected virtual void ButtonSBOCancelarItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            ICompany company = CompanySBO;
            SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(formUid);
            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
            List<InformacionLineaRequisicion> informacionLineasRequisicions = MatrixRequisiciones.SelectedRows2Collection();
            InformacionLineaRequisicion info = new InformacionLineaRequisicion();
            Documents cotizacion;
            cotizacion = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
            List<LineasCotizacion> lineas;
            string error = string.Empty;
            bool boolGenerarRollback = false;
            int codigoError = 0;
            string mensajeError = string.Empty;
            string strDocEntry = string.Empty;
            bool boolActualizarDocumentos = true;
            string strActualizaDocumentos = string.Empty;

            //General service para actualizar el UDO
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData oReqMovLinea;
            SAPbobsCOM.GeneralData oReqLinea;
            SAPbobsCOM.GeneralData oRequisicion;
            SAPbobsCOM.GeneralDataCollection oChildrenMovimientos;
            SAPbobsCOM.GeneralDataCollection oChildrenLineasReq;

            int codEstRequisicion = -1;
            string txtEstadoRequisicion = string.Empty;

            List<InformacionLineaRequisicion> canceladas = new List<InformacionLineaRequisicion>(informacionLineasRequisicions.Count);
            var encabezado = EncabezadoRequisicionFromDBDataSource();

            try
            {
                if (pVal.BeforeAction)
                {
                    error = string.Empty;
                    //if (!encabezado.TipoRequisicion.Contains("Trans") && !encabezado.TipoRequisicion.Contains("Res"))
                    if (!encabezado.TipoRequisicion.Contains("Trans") && !(encabezado.CodigoTipoRequisicion == 3))
                    {
                        if (!ValidaCotizacionAbierta(formUid))
                        {
                            error = Resource.txtErrorCotizacionNoAbierta;
                            ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            bubbleEvent = false;
                            return;

                        }

                        if (!ValidaOTAbierta(formUid))
                        {
                            error = Resource.txtErrorOTNoAbierta;
                            ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            bubbleEvent = false;
                            return;
                        }
                    }

                    if (informacionLineasRequisicions.Count < 1)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.SeleccioneLinea, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        bubbleEvent = false;
                    }

                    foreach (var linea in informacionLineasRequisicions)
                    {
                        if (linea.CantidadRecibida > 0)
                        {
                            error = string.Format(Resource.ErrorCancelarLineaConRecibidos,linea.LineId);
                            ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            bubbleEvent = false;
                            return;
                        }
                    }
                }
                else
                {
                    //Inicia el bloque de transacciones
                    if (!company.InTransaction)
                    {
                        company.StartTransaction();
                    }

                    //Instancia un objecto de tipo General Service con el DocEntry de la requisición abierta
                    //y sus tablas hijas "@SCGD_LINEAS_REQ"
                    var dataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado);
                    strDocEntry = dataSource.GetValue("DocEntry", 0);
                    strActualizaDocumentos = dataSource.GetValue("U_ActualizaDoc", 0);
                    oCompanyService = CompanySBO.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");
                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("DocEntry", strDocEntry);
                    oRequisicion = oGeneralService.GetByParams(oGeneralParams);
                    oChildrenLineasReq = oRequisicion.Child("SCGD_LINEAS_REQ");

                    if (strActualizaDocumentos.ToUpper().Trim() == "N")
                    {
                        //No se debe actualizar la cotización, ni la OT, 
                        //solamente se procesa la requisición y se realiza el traslado de stocks.
                        boolActualizarDocumentos = false;

                    }

                    foreach (var linea in informacionLineasRequisicions)
                    {
                        EstadosLineas estadoLinea = (EstadosLineas)linea.CodigoEstado;
                        if (estadoLinea != EstadosLineas.Pendiente && linea.CantidadRecibida != 0)
                        {
                            error = string.Format("No se puede cancelar la línea: {0}.", linea.DataSourceOffset + 1);
                            Localize(linea, TipoMensaje.NoSePuedeCancelarLinea, error);
                            ApplicationSBO.StatusBar.SetText(error);
                            boolGenerarRollback = true;
                        }

                        string estado = EstadosLineas.Cancelado.ToString();
                        linea.CodigoEstado = (int)EstadosLineas.Cancelado;
                        estado = Localize(linea, TipoMensaje.EstadoLinea, estado);

                        oReqLinea = oChildrenLineasReq.Item(linea.LineId - 1);
                        oReqLinea.SetProperty("U_SCGD_CodEst", (int)EstadosLineas.Cancelado);
                        oReqLinea.SetProperty("U_SCGD_Estado", estado);
                        oReqLinea.SetProperty("U_FechaM", DateTime.Now);
                        oReqLinea.SetProperty("U_HoraM", DateTime.Now);
                        oReqLinea.SetProperty("U_TipoM", "2");

                        canceladas.Add(linea);

                        CheckBoxSelTodo.AsignaValorUserDataSource("N");
                    }

                    //Completa la información de los encabezados de la requisición
                    codEstRequisicion = EstadoRequisicion(oChildrenLineasReq);

                    if (codEstRequisicion != -1)
                    {
                        EstadosLineas estadoFormulario = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), codEstRequisicion.ToString());
                        oRequisicion.SetProperty("U_SCGD_CodEst", codEstRequisicion);
                        txtEstadoRequisicion = Localize(new InformacionLineaRequisicion { CodigoEstado = codEstRequisicion },
                                  TipoMensaje.EstadoFormulario, estadoFormulario.ToString());
                        if (!string.IsNullOrEmpty(txtEstadoRequisicion))
                        {
                            oRequisicion.SetProperty("U_SCGD_Est", txtEstadoRequisicion);
                        }
                    }

                    if (canceladas.Count != 0)
                    {
                        if (boolGenerarRollback == false)
                        {
                            oGeneralService.Update(oRequisicion);

                            bool EsDevolucion = false;
                            //if ((!encabezado.TipoRequisicion.Contains("Trans") && !encabezado.TipoRequisicion.Contains("Res")) || encabezado.CodigoTipoRequisicion == 2)
                            if ((!encabezado.TipoRequisicion.Contains("Trans") && !(encabezado.CodigoTipoRequisicion == 3)) || encabezado.CodigoTipoRequisicion == 2 || encabezado.CodigoTipoRequisicion == 4)
                            {
                                EsDevolucion = true;
                            }

                            if (m_blnActualizaCot)
                            {
                                //ActualizaCotizacion(oChildrenLineasReq, ref codigoError, ref mensajeError, false, EsDevolucion);
                                m_blnActualizaCot = false;
                            }
                            
                            if (codigoError != 0)
                            {
                                boolGenerarRollback = true;
                            }
                            else
                            {
                                if (LineasCanceladas != null && boolActualizarDocumentos == true)
                                    LineasCanceladas(canceladas, encabezado, ref codigoError, ref mensajeError);
                                if (codigoError != 0)
                                {
                                    boolGenerarRollback = true;
                                }
                            }

                            //Proceso exitoso, se procede a aplicar los cambios en la base de datos
                            if (company.InTransaction)
                            {
                                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            }

                            CargaRequisicion(strDocEntry);
                        }
                        else
                        {
                            //Ocurrieron errores, se procede a realizar un RollBack
                            if (company.InTransaction)
                            {
                                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            }
                        }

                    }
                    else
                    {
                        //No se seleccionaron líneas, se finaliza el bloque de transacciones
                        if (company.InTransaction)
                        {
                            company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                    }

                    if (!string.IsNullOrEmpty(mensajeError))
                    {
                        ApplicationSBO.StatusBar.SetText(mensajeError, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }

                }

            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                //Revierte los cambios realizados
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
            }
        }

        //Manejo del evento item pressed del boton Generar Reporte de requisiciones
        private void ButtonSBOGenerarReporteItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            //Declaracion de variables 
            string strParametros = "";

            if (!pVal.BeforeAction)
            {
                strParametros = EditTextNoRequisicion.ObtieneValorDataSource();

                string direccionR = DireccionReportes + Resource.rptReporteRequisicion;

                ImprimirReporte(CompanySBO, direccionR, Resource.TitulorptRequisiciones, strParametros, BDUser, BDPass, CompanySBO.CompanyDB, CompanySBO.Server);
            }
        }

        public void CargaRequisicion(string idRequisicion, string formUID)
        {
            FormularioSBO.Freeze(true);
            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
            ((SAPbouiCOM.EditText)(FormularioSBO.Items.Item("edtNoReq").Specific)).Value = idRequisicion;
            FormularioSBO.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
            FormularioSBO.Freeze(false);
        }

        #endregion

        #region "Metodos"

        ///<summary>
        ///Carga el formulario de Seleccion de empleados
        ///</summary>
        private void CargarFormularioSelUbicaciones(ref ListaUbicaciones m_oFormSeleccionUbicaciones, ref SAPbouiCOM.ItemEvent pVal, ref Boolean BubbleEvent, string idBod, string itemCode, string lineNum, string p_strUbicacion, string p_strTipoRequisicion)
        {
            string strPath;
            SAPbouiCOM.Form oForm;

            try
            {
                oForm = ApplicationSBO.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount);
                oGestorFormularios = new GestorFormularios((SAPbouiCOM.Application)ApplicationSBO);
                oFormListaUbi = m_oFormSeleccionUbicaciones;// new ListaUbicaciones((SAPbouiCOM.Application)ApplicationSBO, CompanySBO);
                oFormListaUbi.FormType = strFormListaUbi;
                oFormListaUbi.Titulo = Resource.TituloListaUbicaciones;

                strPath = System.Windows.Forms.Application.StartupPath + Resource.XMLFormSeleccionUbicaciones;
                oFormListaUbi.NombreXml = strPath;
                oFormListaUbi.FormularioSBO = oGestorFormularios.CargaFormulario(oFormListaUbi);
                oFormListaUbi.CargarMatriz(idBod, itemCode, p_strTipoRequisicion, p_strUbicacion);
                oFormListaUbi.CargaCodigos(ref pVal, idBod, itemCode, lineNum);
                oFormListaUbi.MatrixRequisiciones = MatrixRequisiciones;
                BubbleEvent = false;
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public EncabezadoRequisicion EncabezadoRequisicionFromDBDataSource()
        {
            EncabezadoRequisicion encabezadoRequisicion = new EncabezadoRequisicion();
            if (FormularioSBO != null)
            {
                encabezadoRequisicion.CodigoCliente = EditTextCodigoCliente.ObtieneValorDataSource();
                encabezadoRequisicion.NombreCliente = EditTextNombreCliente.ObtieneValorDataSource();
                encabezadoRequisicion.DocEntry = int.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("DocEntry", 0));
                encabezadoRequisicion.DocNum = EditTextNoRequisicion.ObtieneValorDataSource();
                encabezadoRequisicion.CreateDate = DateTimeFromString(EditTextFecha.ObtieneValorDataSource(), EditTextHora.ObtieneValorDataSource());
                encabezadoRequisicion.NoOrden = EditTextNoOrden.ObtieneValorDataSource();
                encabezadoRequisicion.Usuario = EditTextUsuario.ObtieneValorDataSource();
                encabezadoRequisicion.TipoDocumento = EditTextTipoDocumento.ObtieneValorDataSource();
                encabezadoRequisicion.TipoRequisicion = EditTextTipoRequisicion.ObtieneValorDataSource();
                encabezadoRequisicion.Data = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Data", 0);
                encabezadoRequisicion.Comentarios = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comm", 0);
                encabezadoRequisicion.ComentariosUser = EditTextComentariosUsuario.ObtieneValorDataSource();// FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comen", 0);
                encabezadoRequisicion.Placa = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Placa", 0).ToString().Trim();
                encabezadoRequisicion.Marca = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Marca", 0).ToString().Trim();
                encabezadoRequisicion.Estilo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Estilo", 0).ToString().Trim();
                encabezadoRequisicion.VIN = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_VIN", 0).ToString().Trim();
                int codigoTipoRequisicion;
                if (int.TryParse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_CodTipoReq", 0).ToString().Trim(), out codigoTipoRequisicion))
                {
                    encabezadoRequisicion.CodigoTipoRequisicion = codigoTipoRequisicion;
                }
            }
            return encabezadoRequisicion;
        }

        protected virtual DateTime DateTimeFromString(string fecha, string hora)
        {
            //validacion para manejo de hora en requisiciones
            string s = string.Empty;
            switch (hora.Length)
            {
                case 1:
                    s = "000" + hora;
                    break;
                case 2:
                    s = "00" + hora;
                    break;
                case 3:
                    s = "0" + hora;
                    break;
                case 4:
                    s = hora;
                    break;
            }
            DateTime dateTimeFromString = DateTime.ParseExact(fecha + s, "yyyyMMddHHmm", null);
            return dateTimeFromString;
        }

        protected void ActualizaEnBD()
        {
            var dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaMovimientos);
            if (dbDataSource.Size == 0)
            {
                var dataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado);
                string docentry = dataSource.GetValue("DocEntry", 0);
                dbDataSource.InsertRecord(0);
                dbDataSource.SetValue("U_SCGD_CodArticulo", 0, "-1");
                dbDataSource.SetValue("DocEntry", 0, docentry);
                dbDataSource.SetValue("LineId", 0, "0");
                dbDataSource.SetValue("VisOrder", 0, "0");
                MatrixMovimientos.Matrix.LoadFromDataSource();
            }
            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
            ButtonOk.ItemSBO.Click(BoCellClickType.ct_Regular);
        }

        public static void ImprimirReporte(SAPbobsCOM.ICompany company, string direccionReporte, string barraTitulo, string parametros, string usuarioBD, string contraseñaBD, string BD, string servidor)
        {
            string pathExe;
            string parametrosExe;

            if (string.IsNullOrEmpty(barraTitulo))
            {
                barraTitulo = Resource.rptReporteRequisicion;
            }

            barraTitulo = barraTitulo.Replace(" ", "°");
            direccionReporte = direccionReporte.Replace(" ", "°");
            parametros = parametros.Replace(" ", "°");

            pathExe = Directory.GetCurrentDirectory() + "\\SCG Visualizador de Reportes.exe";

            parametrosExe = barraTitulo + " " + direccionReporte + " " + usuarioBD + "," + contraseñaBD + "," +
                          servidor + "," + BD + " " + parametros;

            ProcessStartInfo startInfo = new ProcessStartInfo(pathExe) { WindowStyle = ProcessWindowStyle.Maximized, Arguments = parametrosExe };

            Process.Start(startInfo);
        }

        public void SeleccionaTodo(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.BeforeAction)
            {
                string strSeleccionTodas = string.Empty;
                DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);

                strSeleccionTodas = CheckBoxSelTodo.ObtieneValorUserDataSource();

                if (strSeleccionTodas == "Y")
                {
                    for (int i = 0; i < dbDataSource.Size; i++)
                    {
                        MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(1, i, dbDataSource);
                    }
                }
                else
                {
                    for (int i = 0; i < dbDataSource.Size; i++)
                    {
                        MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(0, i, dbDataSource);
                    }
                }
                MatrixRequisiciones.Especifico.LoadFromDataSource();
            }

            else
            {
                bubbleEvent = false;
            }
        }

        public void ActualizaCotizacion(string DocEntry, SAPbobsCOM.GeneralDataCollection oChildrenLineasReq, ref int codigoError, ref string mensajeError, bool esAjuste, bool esDevolucion)
        {
            int l_intDocEntry = 0;
            ICompany company = CompanySBO;
            SAPbobsCOM.Documents oCotizacion;
            SAPbobsCOM.Document_Lines oLineasCotizacion;
            SAPbobsCOM.GeneralData oReqLinea;

            try
            {
                string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();
                string l_strNumOT = EditTextNoOrden.ObtieneValorDataSource();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();

                dtLocal.ExecuteQuery(string.Format("select DocEntry from [OQUT] WITH (NOLOCK) WHERE U_SCGD_Numero_OT = '{0}'", l_strNumOT));
                l_intDocEntry = (int)dtLocal.GetValue("DocEntry", 0);

                if (l_strEntregado == "")
                    l_strEntregado = "N";

                string m_strConfOT = "N";
                bool m_blnConfOTSAP = false;
                int m_intLineNum = 0;

                double dblCantidadDM = 0;
                double dblCantidadSolicitado = 0;
                double dblCantidadPendienteDev = 0;

                m_blnConfOTSAP = Utilitarios.ValidaUsaOTSap();

                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);

                if (l_intDocEntry == 0)
                {
                    if (!string.IsNullOrEmpty(DocEntry))
                    {
                        l_intDocEntry = int.Parse(DocEntry);
                    }
                }

                if (oCotizacion.GetByKey(l_intDocEntry))
                {
                    oLineasCotizacion = oCotizacion.Lines;
                    
                    for (int i = 0; i < oChildrenLineasReq.Count; i++)
                    {
                        oReqLinea = oChildrenLineasReq.Item(i);
                        for (int j = 0; j < oLineasCotizacion.Count; j++)
                        {
                            oLineasCotizacion.SetCurrentLine(j);
                            int cantidadLinea = oLineasCotizacion.Count;
                            int numLinea = (int)oReqLinea.GetProperty("U_SCGD_LNumOr");
                            double cantidadPendienteBodega = 0;
                            if ((int)oReqLinea.GetProperty("U_SCGD_LNumOr") == oLineasCotizacion.LineNum)
                            {
                                if (m_blnValidaEntregado)
                                {
                                    oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
                                }
                                if (m_blnConfOTSAP)
                                {
                                    dblCantidadDM = oLineasCotizacion.Quantity;
                                    dblCantidadSolicitado = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;

                                    

                                    if (dblCantidadDM > dblCantidadSolicitado)
                                    {
                                        dblCantidadDM = dblCantidadSolicitado;
                                    }

                                    if (!esDevolucion)
                                    {
                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value) + dblCantidadDM;

                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = dblCantidadSolicitado - dblCantidadDM;

                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;

                                        dblCantidadPendienteDev = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;

                                        if (dblCantidadPendienteDev > 0)
                                        {
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                        }
                                    }
                                    else
                                    {
                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = oReqLinea.GetProperty("U_SCGD_CantPen");
                                        if ((double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value == 0)
                                        {
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = "0";
                                        }
                                    }
                                    
                                }
                                if (oReqLinea.GetProperty("U_Obs_Req").ToString().Trim() != oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString().Trim())
                                {
                                    oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = oReqLinea.GetProperty("U_Obs_Req").ToString().Trim();
                                }

                                if  (oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value == oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value)
                                {
                                    if (!esDevolucion)
                                    {
                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = "2";
                                        oCotizacion.Lines.Quantity = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                                    }
                                    
                                }

                            }
                        }

                    }
                }

                codigoError = oCotizacion.Update();

                //Información de errores de SAP en DI API se pasa a los métodos superiores por referencia
                if (codigoError != 0)
                {
                    mensajeError = company.GetLastErrorDescription();
                }
            }
            catch (Exception ex)
            {
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public void ActualizaCotizacion()
        {
            int l_intDocEntry = 0;
            ICompany company = CompanySBO;
            SAPbobsCOM.Documents oCotizacion;
            SAPbobsCOM.Document_Lines oLineasCotizacion;

            try
            {
                string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();
                string l_strNumOT = EditTextNoOrden.ObtieneValorDataSource();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();

                dtLocal.ExecuteQuery(string.Format("select DocEntry from [OQUT] WITH (NOLOCK) WHERE U_SCGD_Numero_OT = '{0}'", l_strNumOT));
                l_intDocEntry = (int)dtLocal.GetValue("DocEntry", 0);

                if (l_strEntregado == "")
                    l_strEntregado = "N";

                string m_strConfOT = "N";
                bool m_blnConfOTSAP = false;
                int m_intLineNum = 0;

                double dblCantidadDM = 0;
                double dblCantidadSolicitado = 0;
                double dblCantidadPendienteDev = 0;

                m_blnConfOTSAP = Utilitarios.ValidaUsaOTSap();

                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                if (oCotizacion.GetByKey(l_intDocEntry))
                {
                    oLineasCotizacion = oCotizacion.Lines;
                    DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);
                    for (int i = 0; i < dbDataSource.Size; i++)
                    {
                        for (int j = 0; j < oLineasCotizacion.Count; j++)
                        {
                            oLineasCotizacion.SetCurrentLine(j);
                            if (int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_LNumOr", i).Trim()) == oLineasCotizacion.LineNum)
                            {
                                if (m_blnValidaEntregado)
                                {
                                    oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
                                }
                                if (m_blnConfOTSAP)
                                {
                                    dblCantidadDM = oLineasCotizacion.Quantity;
                                    dblCantidadSolicitado = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;

                                    if (dblCantidadDM > dblCantidadSolicitado)
                                    {
                                        dblCantidadDM = dblCantidadSolicitado;
                                    }

                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value) + dblCantidadDM;

                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = dblCantidadSolicitado - dblCantidadDM;

                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;

                                    dblCantidadPendienteDev = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;

                                    if (dblCantidadPendienteDev > 0)
                                    {
                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                    }
                                }
                                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim() != oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString().Trim())
                                {
                                    oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim();
                                }

                                if (oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value == oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value)
                                {
                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = "2";
                                    oCotizacion.Lines.Quantity = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                                }

                            }
                        }

                    }
                }

                oCotizacion.Update();
                
            }
            catch (Exception ex)
            {
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public void ActualizaTransferencias()
        {
            int l_intDocEntry = 0;
            string l_strDocEntry = "";
            ICompany company = CompanySBO;
            SAPbobsCOM.StockTransfer oTrans;

            try
            {

                string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();

                if (l_strEntregado == "")
                    l_strEntregado = "N";

                if (m_blnValidaEntregado)
                {

                    oTrans = (SAPbobsCOM.StockTransfer)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                    DBDataSource dbTranferencias =
                        MatrixMovimientos.FormularioSBO.DataSources.DBDataSources.Item(MatrixMovimientos.TablaLigada);

                    for (int i = 0; i < dbTranferencias.Size; i++)
                    {
                        l_strDocEntry = dbTranferencias.GetValue("U_SCGD_DocEntry", i).Trim();
                        l_intDocEntry = int.Parse(l_strDocEntry);

                        oTrans.GetByKey(l_intDocEntry);

                        oTrans.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
                        oTrans.Update();
                    }
                }

            }
            catch(Exception ex)
            {
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public void ActualizaTransferencias(SAPbobsCOM.GeneralDataCollection oChildrenMovimientos, ref int codigoError, ref string mensajeError)
        {
            int l_intDocEntry = 0;
            string l_strDocEntry = "";
            ICompany company = CompanySBO;
            SAPbobsCOM.StockTransfer oTrans;
            SAPbobsCOM.GeneralData oReqLineaMov;

            try
            {
                codigoError = 0;
                mensajeError = string.Empty;

                string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();

                if (l_strEntregado == "")
                    l_strEntregado = "N";

                if (m_blnValidaEntregado)
                {

                    oTrans = (SAPbobsCOM.StockTransfer)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                    for (int i = 0; i < oChildrenMovimientos.Count; i++)
                    {
                        oReqLineaMov = oChildrenMovimientos.Item(i);
                        l_strDocEntry = oReqLineaMov.GetProperty("U_SCGD_DocEntry").ToString().Trim();
                        l_intDocEntry = int.Parse(l_strDocEntry);

                        oTrans.GetByKey(l_intDocEntry);
                        oTrans.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
                        codigoError = oTrans.Update();
                        //Información de errores de SAP en DI API se pasa a los métodos superiores por referencia
                        if (codigoError != 0)
                        {
                            mensajeError = company.GetLastErrorDescription();
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                if (company.InTransaction)
                {
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public void LlenarComboSucursal()
        {

            SAPbouiCOM.DataTable dtSucursales = null;
            SAPbouiCOM.DataRows drw;
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;

            String strCodigo;
            String strNombre;


            oItem = FormularioSBO.Items.Item("cboSucur");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {

                int CantidadValidValues = cboCombo.ValidValues.Count - 1;

                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }


            if (FormularioSBO.DataSources.DataTables.Count > 0)
            {
                for (int j = 0; j < FormularioSBO.DataSources.DataTables.Count; j++)
                {
                    if (FormularioSBO.DataSources.DataTables.Item(j).UniqueID == "dtSucursales")
                    {
                        blnExisteTabla = true;
                    }
                }
            }

            if (!blnExisteTabla)
            {
                dtSucursales = FormularioSBO.DataSources.DataTables.Add("dtSucursales");
                blnExisteTabla = true;
            }

            dtSucursales = FormularioSBO.DataSources.DataTables.Item("dtSucursales");
            dtSucursales.Clear();

            dtSucursales.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_SUCURSALES]"));

            if (dtSucursales.Rows.Count != 0)
            {
                for (int i = 0; i < dtSucursales.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtSucursales.GetValue("Code", i));
                    strNombre = Convert.ToString(dtSucursales.GetValue("Name", i));

                    cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }
        }

        private string ValidaUbicacionesLinea(ref InformacionLineaRequisicion inf, ref ItemEvent pVal)
        {
            string error = string.Empty;
            Boolean usaUbicaciones = false;
            try
            {
                SAPbouiCOM.DataTable dtConsulta;
                SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(pVal.FormUID);
                var query = string.Empty;
                dtConsulta = Utilitarios.ValidarDataTable(ref oForm, "dtConsulta") ? oForm.DataSources.DataTables.Item("dtConsulta") : oForm.DataSources.DataTables.Add("dtConsulta");

                //Validacion para ubicacion de origen
                query = string.Format("select BinActivat from OWHS with(nolock) where WhsCode = '{0}'", inf.CodigoBodegaOrigen);
                dtConsulta.ExecuteQuery(query);
                if (dtConsulta.Rows.Count > 0)
                {
                    //Valida si la bodega de origen usa ubicaciones
                    if (dtConsulta.GetValue("BinActivat", 0).ToString() == "Y")
                    {
                        if (string.IsNullOrEmpty(inf.DeUbicacion))
                        {
                            error = String.Format(Resource.txtErrorLnUbicOri, inf.DataSourceOffset + 1);
                            return error;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(inf.DeUbicacion))
                        {
                            error = String.Format(Resource.txtErrorNoBodUbicOri, inf.DataSourceOffset + 1);
                            return error;
                        }
                    }
                }

                //Validacion para ubicacion destino
                query = string.Format("select BinActivat from OWHS with(nolock) where WhsCode = '{0}'", inf.CodigoBodegaDestino);
                dtConsulta.ExecuteQuery(query);
                if (dtConsulta.Rows.Count > 0)
                {
                    //Valida si la bodega de origen usa ubicaciones
                    if (dtConsulta.GetValue("BinActivat", 0).ToString() == "Y")
                    {
                        if (string.IsNullOrEmpty(inf.AUbicacion))
                        {
                            error = String.Format(Resource.txtErrorLnUbicDest, inf.DataSourceOffset + 1);
                            return error;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(inf.AUbicacion))
                        {
                            error = String.Format(Resource.txtErrorNoBodUbicDest, inf.DataSourceOffset + 1);
                            return error;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            return error;
        }
        #endregion
    }
}
