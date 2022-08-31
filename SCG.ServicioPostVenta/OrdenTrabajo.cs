using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DMSOneFramework.SCGCommon;
using DMSOneFramework.SCGDataAccess;
using DMS_Connector;
using DMS_Connector.Data_Access;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using SCG.DMSOne;
using ICompany = SAPbobsCOM.ICompany;
using IItems = SAPbobsCOM.IItems;
using Items = SAPbobsCOM.Items;
using SCG.DMSOne.Framework;
using Application = SAPbouiCOM.Application;
using CheckBox = SAPbouiCOM.CheckBox;
using ComboBox = SAPbouiCOM.ComboBox;
using Company = SAPbobsCOM.Company;
using DataTable = SAPbouiCOM.DataTable;
using Form = SAPbouiCOM.Form;

namespace SCG.ServicioPostVenta
{
    public partial class OrdenTrabajo : IFormularioSBO, IUsaMenu
    {
        public void ManejadorEventoItemPress(SAPbouiCOM.ItemEvent pval, string FormUID, ref Boolean BubbleEvent)
        {
            bool m_blnProcesarCotizacion = false;
            int m_intRespuesta = 0;

            if (pval.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                FormularioSBO = ApplicationSBO.Forms.Item(FormUID);
                FormularioSBO.Freeze(true);
                try
                {
                    CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                    CultureInfo cultureInfo = Resource.Culture;
                    DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    Resource.Culture = cultureInfo;
                    SAPbouiCOM.Item m_objItem;
                    SAPbouiCOM.ComboBox m_objCombo;
                    string m_strValorCombo = string.Empty;

                    if (pval.BeforeAction)
                    {
                        switch (pval.EventType)
                        {
                            case BoEventTypes.et_ITEM_PRESSED:

                                switch (pval.ItemUID)
                                {
                                    case "Folder1":
                                        FormularioSBO.PaneLevel = 1;
                                        break;
                                    case "Folder2":
                                        FormularioSBO.PaneLevel = 2;
                                        break;
                                    case "Folder3":
                                        FormularioSBO.PaneLevel = 3;
                                        break;
                                    case "Folder4":
                                        FormularioSBO.PaneLevel = 4;
                                        break;
                                    case "Folder5":
                                        FormularioSBO.PaneLevel = 5;
                                        break;
                                    case "Folder6":
                                        FormularioSBO.PaneLevel = 6;
                                        break;
                                    case "Folder7":
                                        FormularioSBO.PaneLevel = 7;
                                        break;
                                    case "1":

                                        if (FormularioSBO.Mode == BoFormMode.fm_ADD_MODE)
                                        {
                                            ValidaCreacion(ref BubbleEvent);
                                        }
                                        break;
                                    case "btnElim":
                                        break;
                                    case "btnIniA":
                                        ValidaInicioActividad(pval, ref BubbleEvent);
                                        break;
                                    case "btnSuspA":
                                        ValidaSuspensionActividad(pval, ref BubbleEvent);
                                        break;
                                    case "btnFinA":
                                        ValidaFinalizacionActividad(pval, ref BubbleEvent);
                                        break;
                                    case "btnOCom":

                                        break;
                                    case "btnOTEsp":
                                        ValidaOTEspecial(pval, ref BubbleEvent);
                                        break;
                                }
                                break;

                            case BoEventTypes.et_CLICK:
                                break;
                            case BoEventTypes.et_COMBO_SELECT:

                                switch (pval.ItemUID)
                                {
                                    case "136":


                                        break;
                                }
                                break;
                        }
                    }
                    else if (pval.ActionSuccess)
                    {
                        switch (pval.EventType)
                        {
                            case BoEventTypes.et_ITEM_PRESSED:

                                switch (pval.ItemUID)
                                {
                                    case "btnRepD":
                                        PrepararReporteDetalleActividadesOT();
                                        break;
                                    case "btnRepG":
                                        PrepararReporteGeneralActividadesOT();
                                        break;
                                    case "btnOTEsp":
                                        CargarFormularioOTEspecial();
                                        break;
                                    case "btnAsigM":
                                        CargarFormularioAsignacionMultiple(pval);
                                        break;
                                    case "btnElim":
                                        EliminarActividadControlColaborador(pval);
                                        break;
                                    case "btnIniA":
                                        IniciarActividad(pval);
                                        break;
                                    case "btnSuspA":
                                        CargarFormularioRazonesSuspension(pval);
                                        break;
                                    case "btnFinA":
                                        FinalizarLineaServicio(pval, FormUID, ref BubbleEvent);
                                        break;
                                    case "btnAgR":
                                        CargarFormularioAdicionalesOT(pval, (int)TipoAdicional.Repuesto);
                                        break;
                                    case "btnAgS":
                                        CargarFormularioAdicionalesOT(pval, (int)TipoAdicional.Servicio);
                                        break;
                                    case "btnAgSE":
                                        CargarFormularioAdicionalesOT(pval, (int)TipoAdicional.ServicioExterno);
                                        break;
                                    case "btnAddSum":
                                        CargarFormularioAdicionalesOT(pval, (int)TipoAdicional.Suministro);
                                        break;
                                    case "btnTracSOL":
                                        CargarFormularioTrackingSolEspecificos();
                                        break;
                                    case "Edit":
                                        AbrirEditorRegistroTiempo();
                                        break;
                                    case "1":

                                        // Validacion de filtro combo Rpuestos
                                        if (!g_realizofiltroRepuestos)
                                        {
                                            if (ValidaExistenciaCambios(g_strdtRepuestos))
                                            {
                                                ActualizarAdicionales(pval, g_strdtRepuestos, true, false, false, false);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }
                                        else
                                        {
                                            if (ValidaExistenciaCambios(g_strdtRepuestosTemporal))
                                            {
                                                ActualizarAdicionales(pval, g_strdtRepuestosTemporal, true, false, false, false);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }

                                        // Validacion de filtro combo Servicios
                                        if (!g_realizofiltroServicios)
                                        {
                                            if (ValidaExistenciaCambios(g_strdtServicios))
                                            {
                                                ActualizarAdicionales(pval, g_strdtServicios, false, true, false, false);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }
                                        else
                                        {
                                            if (ValidaExistenciaCambios(g_strdtServiciosTemporal))
                                            {
                                                ActualizarAdicionales(pval, g_strdtServiciosTemporal, false, true, false, false);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }

                                        // Validacion de filtro combo Servicios externos
                                        if (!g_realizofiltroServiciosExter)
                                        {
                                            if (ValidaExistenciaCambios(g_strdtServiciosExternos))
                                            {
                                                ActualizarAdicionales(pval, g_strdtServiciosExternos, false, false, true, false);
                                                m_blnProcesarCotizacion = true;
                                            }

                                        }
                                        else
                                        {
                                            if (ValidaExistenciaCambios(g_strdtServiciosExternosTemporal))
                                            {
                                                ActualizarAdicionales(pval, g_strdtServiciosExternosTemporal, false, false, true, false);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }

                                        // Validacion de filtro combo Suministros
                                        if (!g_realizofiltroSuministros)
                                        {
                                            if (ValidaExistenciaCambios(g_strdtSuministros))
                                            {
                                                ActualizarAdicionales(pval, g_strdtSuministros, false, false, false, true);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }
                                        else
                                        {
                                            if (ValidaExistenciaCambios(g_strdtSuministrosTemporal))
                                            {
                                                ActualizarAdicionales(pval, g_strdtSuministrosTemporal, false, false, false, true);
                                                m_blnProcesarCotizacion = true;
                                            }
                                        }

                                        if (m_blnProcesarCotizacion)
                                        {
                                            ProcesaCotización(pval);
                                        }
                                       

                                        break;
                                    case "btnOCom":
                                        CargarFormularioDocumentoCompra(pval, ref BubbleEvent, TipoAdicional.Repuesto);
                                        break;
                                    case "btnComSum":
                                        CargarFormularioDocumentoCompra(pval, ref BubbleEvent, TipoAdicional.Suministro);
                                        break;
                                    case "129":
                                        CargarFormularioDocumentoCompra(pval, ref BubbleEvent, TipoAdicional.ServicioExterno);
                                        break;
                                }
                                break;
                            case BoEventTypes.et_CLICK:
                                break;
                            case BoEventTypes.et_DOUBLE_CLICK:

                                switch (pval.ItemUID)
                                {
                                    case "mtxColab":
                                        ManejaMatriz(pval, ref BubbleEvent, TipoAdicional.Servicio, true);
                                        break;
                                    case "mtxRep":
                                        ManejaMatriz(pval, ref BubbleEvent, TipoAdicional.Repuesto);
                                        break;
                                    case "mtxSum":
                                        ManejaMatriz(pval, ref BubbleEvent, TipoAdicional.Suministro);
                                        break;
                                    case "mtxServE":
                                        ManejaMatriz(pval, ref BubbleEvent, TipoAdicional.ServicioExterno);
                                        break;
                                }
                                break;
                            case BoEventTypes.et_COMBO_SELECT:

                                switch (pval.ItemUID)
                                {
                                    case "136":
                                        m_objItem = FormularioSBO.Items.Item("136");
                                        m_objCombo = (ComboBox)m_objItem.Specific;
                                        m_strValorCombo = m_objCombo.Value.Trim();

                                        //Validacion para la interfaz Ford
                                        var usaInterFazFord = DMS_Connector.Configuracion.ParamGenAddon.U_Usa_IFord;
                                        if (usaInterFazFord == "Y")
                                        {
                                            if (!ValidacionesInterfazFord())
                                            {
                                                BubbleEvent = false;
                                                return;
                                            }
                                        }

                                        switch (m_strValorCombo)
                                        {
                                            case "3":
                                                CargarFormularioRazonesSuspension(pval, true);
                                                break;


                                            case "4":
                                                m_intRespuesta = ApplicationSBO.MessageBox(Resource.msgFinalizacionOT, 1, Resource.Si, Resource.No);

                                                if (m_intRespuesta == 1)
                                                {
                                                    FinalizarOrden(pval, ref BubbleEvent);
                                                }
                                                break;


                                            case "5":
                                                ValidacionesParaCancelarOT(ref BubbleEvent);
                                                if (BubbleEvent)
                                                {
                                                    if (ValidaExistenciaDeArticulosEntregados())
                                                    {
                                                        m_intRespuesta = ApplicationSBO.MessageBox(Resource.msgCancelacionOT, 1, Resource.Si, Resource.No);

                                                        if (m_intRespuesta == 1)
                                                        {
                                                            CancelarOrden(pval, ref BubbleEvent);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE; ApplicationSBO.StatusBar.SetText(Resource.MsjNoSePuedeCancelarOTArticulosEntregados, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                                    }
                                                }
                                                break;


                                            case "":
                                                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                                                break;
                                        }

                                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                                        ValidaModoVistaOT((SAPbouiCOM.Form)FormularioSBO, false);
                                        break;


                                    case "cboEstSE":
                                        m_objItem = FormularioSBO.Items.Item("cboEstSE");
                                        m_objCombo = (ComboBox)m_objItem.Specific;

                                        m_strValorCombo = m_objCombo.Value;
                                        m_strValorCombo = m_strValorCombo.Trim();

                                        ManejaFiltroCombo(g_dtServiciosExt, 4, m_strValorCombo);
                                        break;

                                    case "cboEstSu":
                                        m_objItem = FormularioSBO.Items.Item("cboEstSu");
                                        m_objCombo = (ComboBox)m_objItem.Specific;

                                        m_strValorCombo = m_objCombo.Value;
                                        m_strValorCombo = m_strValorCombo.Trim();

                                        ManejaFiltroCombo(g_dtSuministros, 3, m_strValorCombo);
                                        break;
                                    case "cboEstR":
                                        m_objItem = FormularioSBO.Items.Item("cboEstR");
                                        m_objCombo = (ComboBox)m_objItem.Specific;

                                        m_strValorCombo = m_objCombo.Value;
                                        m_strValorCombo = m_strValorCombo.Trim();

                                        ManejaFiltroCombo(g_dtRepuestos, 1, m_strValorCombo);
                                        break;

                                    case "cboFProS":
                                        m_objItem = FormularioSBO.Items.Item("cboFProS");
                                        m_objCombo = (ComboBox)m_objItem.Specific;

                                        m_strValorCombo = m_objCombo.Value;
                                        m_strValorCombo = m_strValorCombo.Trim();
                                        ManejaFiltroCombo(g_dtServicios, 2, m_strValorCombo);
                                        break;
                                }
                                break;

                            case BoEventTypes.et_LOST_FOCUS:
                                ManejadorEventoLostFocus(pval);
                                break;
                        }

                    }
                    FormularioSBO.Freeze(false);
                }
                catch (Exception ex)
                {
                    throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                }
                FormularioSBO.Freeze(false);
            }
        }

        /// <summary>
        /// Abre una ventana del editor de registro de tiempo
        /// </summary>
        private void AbrirEditorRegistroTiempo()
        {
            string Sucursal;
            string NumeroOT;
            string CodigoEstadoOT;
            string DocEntryCotizacion;
            try
            {
                Sucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                NumeroOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                CodigoEstadoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim();
                DocEntryCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                if (!string.IsNullOrEmpty(Sucursal) && !string.IsNullOrEmpty(NumeroOT) && !string.IsNullOrEmpty(CodigoEstadoOT) && !string.IsNullOrEmpty(DocEntryCotizacion))
                {
                    RegistroTiempo.AbrirFormulario(this, Sucursal, NumeroOT, (GeneralEnums.EstadoOT)Convert.ToInt32(CodigoEstadoOT), DocEntryCotizacion);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        /// <summary>
        /// Validacion para los articulos
        /// </summary>
        /// <returns></returns>
        private void ValidacionesParaCancelarOT(ref bool p_BubbleEvent)
        {
            string m_strNoOT;
            string idSucursal;
            string strdocentry;

            try
            {
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                idSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                strdocentry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();

                if (g_dtConfSucursal.GetValue("U_CanOTSer", 0).ToString().Trim() == "Y" && p_BubbleEvent)
                {
                    CancelaOTsoloServicios(ref p_BubbleEvent);
                }

                if (g_dtConfSucursal.GetValue("U_ValReqPen", 0).ToString().Trim() == "Y" && p_BubbleEvent)
                {
                    ValidaRequisicionesPendientes(ref p_BubbleEvent, m_strNoOT, idSucursal);
                }

                if (g_dtConfSucursal.GetValue("U_PerCanOT", 0).ToString().Trim() == "Y" && p_BubbleEvent)
                {
                    ValidaRepuestosCompraRecibidos(ref p_BubbleEvent, strdocentry);
                }

                if (g_dtConfSucursal.GetValue("U_PCanOTAct", 0).ToString().Trim() == "N" && p_BubbleEvent)
                {
                    ValidaEstadoActividadesOT(ref p_BubbleEvent, m_strNoOT);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Valida que estado de las actividades aprobadas no sean 2  Iniciada o 4 Finalizada al cancelar la OT
        /// </summary>
        /// <param name="p_BubbleEvent"></param>
        /// <param name="m_strNoOT"></param>
        private void ValidaEstadoActividadesOT(ref bool p_BubbleEvent, string m_strNoOT)
        {
            SAPbouiCOM.DBDataSource oDBCTRLCOL;
            SAPbouiCOM.DataTable odtServicios;
            try
            {
                oDBCTRLCOL = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL");

                if (oDBCTRLCOL.Size > 0)
                {
                    //setea el datable en caso de que la matriz haya sido filtrada o no
                    if (!g_realizofiltroServicios)
                    {
                        odtServicios = FormularioSBO.DataSources.DataTables.Item(g_strdtServicios);
                    }
                    else
                    {
                        odtServicios = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosTemporal);
                    }

                    //Recorre datatable
                    for (int x = 0; x < odtServicios.Rows.Count; x++)
                    {
                        //Los servicios permanentes 
                        if (string.Equals(odtServicios.GetValue("perm", x).ToString().Trim(), "Y"))
                        {
                            //Recorre DBDataSourcer
                            for (int i = 0; i < oDBCTRLCOL.Size; i++)
                            {
                                //Valida el id de la actividad
                                if (string.Equals(odtServicios.GetValue("idit", x).ToString().Trim(), oDBCTRLCOL.GetValue("U_IdAct", i).Trim()))
                                {
                                    if (string.Equals(oDBCTRLCOL.GetValue("U_Estad", i).Trim(), "2") || string.Equals(oDBCTRLCOL.GetValue("U_Estad", i).Trim(), "4"))
                                    {
                                        ApplicationSBO.SetStatusBarMessage(string.Format(Resource.ValidaEstadoActividadCancelar, odtServicios.GetValue("desc", x).ToString().Trim()), BoMessageTime.bmt_Short);
                                        p_BubbleEvent = false;
                                        break;
                                    }

                                }

                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidaRepuestosCompraRecibidos(ref bool p_BubbleEvent, string p_strdocentry)
        {
            var query = string.Format(g_strValidaCompraRecibidos, p_strdocentry);
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

            g_dtConsulta.ExecuteQuery(query);

            if ((int)g_dtConsulta.GetValue(0, 0) > 0)
            {
                ApplicationSBO.StatusBar.SetText(Resource.ValidaRepuestosCompraRecibidos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                p_BubbleEvent = false;
            }
        }

        /// <summary>
        /// Valida Existencia De Articulos Entregados
        /// </summary>
        /// <param name="p_Mensaje"></param>
        /// <returns></returns>
        private bool ValidaExistenciaDeArticulosEntregados()
        {
            try
            {
                if (g_dtConfSucursal.GetValue("U_CanOTArAp", 0).ToString().Trim() == "N")
                {
                    if (!ValidaArticulosNoEntregados())
                    {
                        return true;
                    }

                }
                else
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// VAlidaExistencia de Articulos(Repuestos, etc) 
        /// </summary>
        /// <returns></returns>
        private void CancelaOTsoloServicios(ref bool p_BubbleEvent)
        {
            SAPbouiCOM.Form oForm;

            oForm = ApplicationSBO.Forms.Item("SCGD_ORDT");
            try
            {
                for (int i = 0; i < g_dtRepuestos.Rows.Count; i++)
                {
                    if (g_dtRepuestos.GetValue("code", i).ToString().Trim() != "")
                    {
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        p_BubbleEvent = false;
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE; ApplicationSBO.StatusBar.SetText(Resource.MsjNoPuedeCancelarOTExisteArt, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        return;
                    }

                }

                for (int i = 0; i < g_dtServiciosExt.Rows.Count; i++)
                {

                    if (g_dtServiciosExt.GetValue("code", i).ToString().Trim() != "")
                    {
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        p_BubbleEvent = false;
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE; ApplicationSBO.StatusBar.SetText(Resource.MsjNoPuedeCancelarOTExisteArt, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        return;
                    }

                }

                for (int i = 0; i < g_dtSuministros.Rows.Count; i++)
                {

                    if (g_dtSuministros.GetValue("code", i).ToString().Trim() != "")
                    {
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        p_BubbleEvent = false;
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE; ApplicationSBO.StatusBar.SetText(Resource.MsjNoPuedeCancelarOTExisteArt, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        return;
                    }

                }
                p_BubbleEvent = true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Agrega Articulos a data table
        /// </summary>
        /// <param name="oForm"></param>
        /// <param name="p_DtDataSource"></param>
        private void AgregaArticulosADatable(SAPbouiCOM.Form oForm, ref DataTable p_dtTemporal, int p_PosicionDataSource, int p_posiciondt)
        {
            try
            {
                p_dtTemporal.Rows.Add(1);
                p_dtTemporal.SetValue("sele", p_posiciondt, "");
                p_dtTemporal.SetValue("Colab", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("IdAct", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CodFas", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("FIni", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CodFas", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("FFin", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CodFas", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("TMin", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_TMin", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("RePro", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_RePro", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("NoFas", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_NoFas", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("Estad", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("IdAc", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("CosRe", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosRe", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("CosEst", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosEst", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("ReAsig", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_ReAsig", p_PosicionDataSource).Trim());
                p_dtTemporal.SetValue("CodFas", p_posiciondt, oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CodFas", p_PosicionDataSource).Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza las lineas en la cotizacion de los mecanicos recien asignados
        /// </summary>
        private void ActualizaLineasServiciosAsiganados(ref Documents m_oCotizacion)
        {
            string m_strNombreMecanico = string.Empty;
            try
            {
                //m_oLineas = m_oCotizacion.Lines;
                for (int i = 0; i < m_oCotizacion.Lines.Count; i++)
                {
                    m_oCotizacion.Lines.SetCurrentLine(i);

                    for (int j = 0; j < g_dtAdicionalesColaborador.Rows.Count; j++)
                    {
                        if (m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == g_dtAdicionalesColaborador.GetValue("IdAct", j).ToString().Trim())
                        {
                            m_strNombreMecanico = ObtieneNombreMecanico(g_dtAdicionalesColaborador.GetValue("IdCol", j).ToString().Trim());

                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = g_dtAdicionalesColaborador.GetValue("IdCol", j).ToString().Trim();
                            m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = m_strNombreMecanico;
                        }
                    }


                }

                g_dtAdicionalesColaborador.Rows.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene nombre mecanico
        /// </summary>
        /// <param name="p_strIdMec"></param>
        /// <returns></returns>
        private string ObtieneNombreMecanico(string p_strIdMec)
        {
            SAPbouiCOM.DataTable m_dtConsulta;
            string m_strConsulta = " Select (oh.firstName + ' '+oh.lastName) as Mecanico From OHEM as oh with(nolock) where empid = '{0}' ";
            string m_strNombre = string.Empty;

            try
            {
                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                m_dtConsulta.ExecuteQuery(string.Format(m_strConsulta, p_strIdMec));
                m_strNombre = m_dtConsulta.GetValue(0, 0).ToString().Trim();

                return m_strNombre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ObtieneOcupacionMecanico(string p_strIdMec)
        {
            SAPbouiCOM.DataTable m_dtConsulta;
            string m_strConsulta = " Select  Count(*) from [@SCGD_CTRLCOL] with (nolock) Where U_Estad = 2 and U_Colab ='{0}' ";
            int m_strNombre = 0;

            try
            {
                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                m_dtConsulta.ExecuteQuery(string.Format(m_strConsulta, p_strIdMec));
                m_strNombre = int.Parse(m_dtConsulta.GetValue(0, 0).ToString().Trim());
                return m_strNombre;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Maneja filtros de Articulos en Matriz
        /// </summary>
        /// <param name="p_dtArticulos"></param>
        public void ManejaFiltroCombo(DataTable p_dtArticulos, int p_tipoarticulo, string p_strValorCombo, bool blnCompra = false)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.Form oForm;
            string m_strMatriz = string.Empty;
            int m_intCantidad = 0;
            DataTable m_dtDataSource;
            oForm = ApplicationSBO.Forms.Item("SCGD_ORDT");

            try
            {
                if (p_dtArticulos.Rows.Count > 0)
                {
                    switch (p_tipoarticulo)
                    {
                        case 1:

                            if (!string.IsNullOrEmpty(p_strValorCombo) || p_strValorCombo != "")
                            {
                                m_strMatriz = "mtxRep";
                                m_dtDataSource = oForm.DataSources.DataTables.Item(g_strdtRepuestosTemporal);
                                m_dtDataSource.Rows.Clear();
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizRepuestos.TablaLigada = g_strdtRepuestosTemporal;
                                g_objMatrizRepuestos.LigaColumnas();
                                DistribuccionDeArticulosRepuestos(p_dtArticulos, p_strValorCombo, ref m_dtDataSource, ref m_intCantidad);
                                if (m_intCantidad == 0)
                                { m_dtDataSource.Rows.Clear(); g_dtTemporalRepuestos.Rows.Clear(); }
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                g_realizofiltroRepuestos = true;
                                oMatrix.LoadFromDataSource();


                            }
                            else
                            {
                                g_realizofiltroRepuestos = false;
                                m_strMatriz = "mtxRep";
                                p_dtArticulos = oForm.DataSources.DataTables.Item(g_strdtRepuestos);
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizRepuestos.TablaLigada = g_strdtRepuestos;
                                g_objMatrizRepuestos.LigaColumnas();
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                if (!blnCompra)
                                    CargaMatrices(true, false, false, false, false, false);
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(p_strValorCombo) || p_strValorCombo != "")
                            {
                                g_realizofiltroServicios = true;
                                m_strMatriz = "mtxSer";
                                m_dtDataSource = oForm.DataSources.DataTables.Item(g_strdtServiciosTemporal);
                                m_dtDataSource.Rows.Clear();
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizServicios.TablaLigada = g_strdtServiciosTemporal;
                                g_objMatrizServicios.LigaColumnas();
                                DistribuccionDeArticulosServicios(p_dtArticulos, p_strValorCombo, ref m_dtDataSource, ref m_intCantidad);
                                if (m_intCantidad == 0)
                                { m_dtDataSource.Rows.Clear(); g_dtTemporalServicios.Rows.Clear(); }
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                oMatrix.LoadFromDataSource();
                            }
                            else
                            {
                                g_realizofiltroServicios = false;
                                m_strMatriz = "mtxSer";
                                p_dtArticulos = oForm.DataSources.DataTables.Item(g_strdtServicios);
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizServicios.TablaLigada = g_strdtServicios;
                                g_objMatrizServicios.LigaColumnas();
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                if (!blnCompra)
                                    CargaMatrices(false, true, false, false, false, false);
                            }
                            break;

                        case 3:
                            if (!string.IsNullOrEmpty(p_strValorCombo) || p_strValorCombo != "")
                            {
                                g_realizofiltroSuministros = true;
                                m_strMatriz = "mtxSum";
                                m_dtDataSource = oForm.DataSources.DataTables.Item(g_strdtSuministrosTemporal);
                                m_dtDataSource.Rows.Clear();
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizSuministros.TablaLigada = g_strdtSuministrosTemporal;
                                g_objMatrizSuministros.LigaColumnas();
                                DistribuccionDeArticulosSuminitros(p_dtArticulos, p_strValorCombo, ref m_dtDataSource, ref m_intCantidad);
                                if (m_intCantidad == 0)
                                { m_dtDataSource.Rows.Clear(); g_dtTemporalSuministros.Rows.Clear(); }
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                oMatrix.LoadFromDataSource();
                            }
                            else
                            {
                                g_realizofiltroSuministros = false;
                                m_strMatriz = "mtxSum";
                                p_dtArticulos = oForm.DataSources.DataTables.Item(g_strdtSuministros);
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizSuministros.TablaLigada = g_strdtSuministros;
                                g_objMatrizSuministros.LigaColumnas();
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                if (!blnCompra)
                                    CargaMatrices(false, false, false, true, false, false);
                            }
                            break;
                        case 4:
                            if (!string.IsNullOrEmpty(p_strValorCombo) || p_strValorCombo != "")
                            {
                                g_realizofiltroServiciosExter = true;
                                m_strMatriz = "mtxServE";
                                m_dtDataSource = oForm.DataSources.DataTables.Item(g_strdtServiciosExternosTemporal);
                                m_dtDataSource.Rows.Clear();
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizServiciosExt.TablaLigada = g_strdtServiciosExternosTemporal;
                                g_objMatrizServiciosExt.LigaColumnas();
                                DistribuccionDeArticulosServiciosExternos(p_dtArticulos, p_strValorCombo, ref m_dtDataSource, ref m_intCantidad);
                                if (m_intCantidad == 0)
                                { m_dtDataSource.Rows.Clear(); g_dtTemporalServiciosExternos.Rows.Clear(); }
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                oMatrix.LoadFromDataSource();
                            }
                            else
                            {
                                g_realizofiltroServiciosExter = false;
                                m_strMatriz = "mtxServE";
                                p_dtArticulos = oForm.DataSources.DataTables.Item(g_strdtServiciosExternos);
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;
                                oMatrix.FlushToDataSource();
                                g_objMatrizServiciosExt.TablaLigada = g_strdtServiciosExternos;
                                g_objMatrizServiciosExt.LigaColumnas();
                                oForm.Mode = BoFormMode.fm_OK_MODE;
                                if (!blnCompra)
                                    CargaMatrices(false, false, true, false, false, false);
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Distribuye repuestos
        /// </summary>
        /// <param name="p_dtArticulos"></param>
        /// <param name="p_strValorCombo"></param>
        private void DistribuccionDeArticulosRepuestos(DataTable p_dtArticulos, string p_strValorCombo, ref DataTable p_dtTemporal, ref int p_cantidad)
        {

            int m_posicion = 0;
            p_dtTemporal.Rows.Clear();
            try
            {
                for (int i = 0; i < p_dtArticulos.Rows.Count; i++)
                {
                    switch (p_strValorCombo)
                    {
                        case "1":
                            if (p_dtArticulos.GetValue("pend", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;

                            }
                            break;
                        case "2":
                            if (p_dtArticulos.GetValue("soli", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "3":
                            if (p_dtArticulos.GetValue("reci", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "4":
                            if (p_dtArticulos.GetValue("pdev", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "5":
                            if (p_dtArticulos.GetValue("ptra", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "6":
                            if (p_dtArticulos.GetValue("pbod", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                    }
                }

                if (m_posicion != 0)
                {
                    p_cantidad = m_posicion;
                }

                for (int i = 0; i < p_dtTemporal.Rows.Count; i++)
                {
                    if (p_dtTemporal.GetValue("tras", i).ToString().Trim() == "")
                    {
                        p_dtTemporal.Rows.Remove(i);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Distribuye repuestos a temporal Filtro
        /// </summary>
        /// <param name="p_dtArticulos"></param>
        /// <param name="p_strValorCombo"></param>
        /// <param name="p_dtDataSource"></param>
        private void DistribuccionDeArticulosServiciosExternos(DataTable p_dtArticulos, string p_strValorCombo, ref DataTable p_dtTemporal, ref int p_cantidad)
        {
            int m_posicion = 0;
            p_dtTemporal.Rows.Clear();
            try
            {
                for (int i = 0; i < p_dtArticulos.Rows.Count; i++)
                {
                    switch (p_strValorCombo)
                    {
                        case "1":
                            if (p_dtArticulos.GetValue("pend", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;

                            }
                            break;
                        case "2":
                            if (p_dtArticulos.GetValue("soli", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "3":
                            if (p_dtArticulos.GetValue("reci", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "4":
                            if (p_dtArticulos.GetValue("pdev", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "5":
                            if (p_dtArticulos.GetValue("ptra", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "6":
                            if (p_dtArticulos.GetValue("pbod", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));
                                p_dtTemporal.SetValue("esco", m_posicion, p_dtArticulos.GetValue("esco", i));
                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                    }
                }

                if (m_posicion != 0)
                {
                    p_cantidad = m_posicion;
                }

                for (int i = 0; i < p_dtTemporal.Rows.Count; i++)
                {
                    if (p_dtTemporal.GetValue("tras", i).ToString().Trim() == "")
                    {
                        p_dtTemporal.Rows.Remove(i);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Distribuye suministros en data table temporal
        /// </summary>
        /// <param name="pDtArticulos"></param>
        /// <param name="pStrValorCombo"></param>
        /// <param name="mDtDataSource"></param>
        private void DistribuccionDeArticulosSuminitros(DataTable p_dtArticulos, string p_strValorCombo, ref DataTable p_dtTemporal, ref int p_cantidad)
        {
            int m_posicion = 0;
            p_dtTemporal.Rows.Clear();
            try
            {
                for (int i = 0; i < p_dtArticulos.Rows.Count; i++)
                {
                    switch (p_strValorCombo)
                    {
                        case "1":
                            if (p_dtArticulos.GetValue("pend", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;

                            }
                            break;
                        case "2":
                            if (p_dtArticulos.GetValue("soli", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "3":
                            if (p_dtArticulos.GetValue("reci", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "4":
                            if (p_dtArticulos.GetValue("pdev", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "5":
                            if (p_dtArticulos.GetValue("ptra", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                        case "6":
                            if (p_dtArticulos.GetValue("pbod", i).ToString().Trim() != "0")
                            {
                                p_dtTemporal.Rows.Add(1);
                                p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                                p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                                p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                                p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                                p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                                p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                                p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                                p_dtTemporal.SetValue("alma", m_posicion, p_dtArticulos.GetValue("alma", i));
                                p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                                p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                                p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                                p_dtTemporal.SetValue("pend", m_posicion, p_dtArticulos.GetValue("pend", i));
                                p_dtTemporal.SetValue("soli", m_posicion, p_dtArticulos.GetValue("soli", i));
                                p_dtTemporal.SetValue("reci", m_posicion, p_dtArticulos.GetValue("reci", i));
                                p_dtTemporal.SetValue("pdev", m_posicion, p_dtArticulos.GetValue("pdev", i));
                                p_dtTemporal.SetValue("ptra", m_posicion, p_dtArticulos.GetValue("ptra", i));
                                p_dtTemporal.SetValue("pbod", m_posicion, p_dtArticulos.GetValue("pbod", i));
                                p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                                m_posicion = p_dtTemporal.Rows.Count;
                            }
                            break;
                    }
                }

                if (m_posicion != 0)
                {
                    p_cantidad = m_posicion;
                }

                for (int i = 0; i < p_dtTemporal.Rows.Count; i++)
                {
                    if (p_dtTemporal.GetValue("tras", i).ToString().Trim() == "")
                    {
                        p_dtTemporal.Rows.Remove(i);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Distribuye articulos en temporal segun el filtro
        /// </summary>
        /// <param name="p_dtArticulos"></param>
        /// <param name="p_strValorCombo"></param>
        /// <param name="p_dtTemporal"></param>
        private void DistribuccionDeArticulosServicios(DataTable p_dtArticulos, string p_strValorCombo, ref DataTable p_dtTemporal, ref int p_cantidad)
        {
            int m_posicion = 0;
            p_dtTemporal.Rows.Clear();
            try
            {
                for (int i = 0; i < p_dtArticulos.Rows.Count; i++)
                {
                    string mm = p_dtArticulos.GetValue("nofa", i).ToString().Trim();

                    if (p_dtArticulos.GetValue("nofa", i).ToString().Trim() == p_strValorCombo)
                    {
                        p_dtTemporal.Rows.Add(1);
                        p_dtTemporal.SetValue("sele", m_posicion, p_dtArticulos.GetValue("sele", i));
                        p_dtTemporal.SetValue("tras", m_posicion, p_dtArticulos.GetValue("tras", i));
                        p_dtTemporal.SetValue("apro", m_posicion, p_dtArticulos.GetValue("apro", i));
                        p_dtTemporal.SetValue("perm", m_posicion, p_dtArticulos.GetValue("perm", i));
                        p_dtTemporal.SetValue("code", m_posicion, p_dtArticulos.GetValue("code", i));
                        p_dtTemporal.SetValue("desc", m_posicion, p_dtArticulos.GetValue("desc", i));
                        p_dtTemporal.SetValue("cant", m_posicion, p_dtArticulos.GetValue("cant", i));
                        p_dtTemporal.SetValue("prec", m_posicion, p_dtArticulos.GetValue("prec", i));
                        p_dtTemporal.SetValue("mone", m_posicion, p_dtArticulos.GetValue("mone", i));
                        p_dtTemporal.SetValue("esta", m_posicion, p_dtArticulos.GetValue("esta", i));
                        p_dtTemporal.SetValue("dura", m_posicion, p_dtArticulos.GetValue("dura", i));
                        p_dtTemporal.SetValue("nofa", m_posicion, p_dtArticulos.GetValue("nofa", i));
                        p_dtTemporal.SetValue("adic", m_posicion, p_dtArticulos.GetValue("adic", i));
                        p_dtTemporal.SetValue("idit", m_posicion, p_dtArticulos.GetValue("idit", i));

                        m_posicion = p_dtTemporal.Rows.Count;
                    }
                }

                if (m_posicion != 0)
                {
                    p_cantidad = m_posicion;
                }

                for (int i = 0; i < p_dtTemporal.Rows.Count; i++)
                {
                    if (p_dtTemporal.GetValue("tras", i).ToString().Trim() == "")
                    {
                        p_dtTemporal.Rows.Remove(i);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Valida Existencia de actividades en Control colaborador
        /// </summary>
        /// <returns></returns>
        private bool ValidaExistenciaDeActividadesCTRL()
        {
            SAPbouiCOM.Form oForm;
            bool p_bexisteValores = false;

            oForm = ApplicationSBO.Forms.Item("SCGD_ORDT");
            try
            {

                for (int i = 0; i < oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size; i++)
                {
                    if (!string.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", i).Trim()))
                    {
                        p_bexisteValores = true;

                    }
                }

                if (!p_bexisteValores)
                {

                    return false;
                }
                else
                {
                    oForm.Mode = BoFormMode.fm_OK_MODE;
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Valida si existen repuestos, servicios externos y suministros ya entregados
        /// </summary>
        /// <returns></returns>
        private bool ValidaArticulosNoEntregados()
        {
            SAPbouiCOM.Form oForm;

            oForm = ApplicationSBO.Forms.Item("SCGD_ORDT");
            try
            {
                for (int i = 0; i < g_dtRepuestos.Rows.Count; i++)
                {
                    if (g_dtRepuestos.GetValue("esco", i).ToString().Trim() == "1")
                    {
                        if (g_dtRepuestos.GetValue("reci", i).ToString().Trim() != "0")
                        {
                            oForm.Mode = BoFormMode.fm_OK_MODE;
                            return true;
                            break;
                        }
                    }
                }

                for (int i = 0; i < g_dtServiciosExt.Rows.Count - 1; i++)
                {

                    if (g_dtServiciosExt.GetValue("reci", i).ToString().Trim() != "0")
                    {
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        return true;
                        break;
                    }

                }

                for (int i = 0; i < g_dtSuministros.Rows.Count - 1; i++)
                {

                    if (g_dtSuministros.GetValue("reci", i).ToString().Trim() != "0")
                    {
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        return true;
                        break;
                    }

                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CancelarOrden(ItemEvent pval, ref bool BubbleEvent)
        {
            DMSOneFramework.SCGBusinessLogic.TransferenciaItems m_objTransferencia;

            string m_strIdSeriesDocumentosTraslado = string.Empty;

            SAPbouiCOM.DataTable m_dtBodegasXCentroCosto;
            SAPbouiCOM.DataTable m_dtRepuestos;
            SAPbouiCOM.DataTable m_dtSuministros;
            SAPbouiCOM.DataTable m_dtConfigSucursal;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            string m_strDraft = string.Empty;
            bool m_blnDraft = false;
            string m_strDocEntry = string.Empty;
            int m_intDocEntry = 0;
            string strError;
            int intError;
            SAPbobsCOM.Documents m_oCotizacion;
            SAPbouiCOM.DataTable m_dtEstadosOT;
            SAPbouiCOM.StaticText oStatic;
            string m_strCancelada = string.Empty;
            string m_strCodeEstado = string.Empty;
            string m_strDescEstado = string.Empty;
            string m_strCodCli = string.Empty;
            string m_strNomCli = string.Empty;
            int ErrorCode = 0;
            string ErrorMessage = string.Empty;
            string DocEntryRequisicionRepuestos = string.Empty;
            string DocEntryRequisicionSuministros = string.Empty;


            try
            {
                if (pval.ActionSuccess)
                {
                    m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                    string m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                    string m_strSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();

                    m_strIdSeriesDocumentosTraslado = m_dtConfigSucursal.GetValue("U_SerInv", 0).ToString().Trim();

                    oCompanyService = CompanySBO.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", m_strNoOT);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                    SAPbobsCOM.Company objCompanyL = (Company)CompanySBO;

                    m_objTransferencia = new DMSOneFramework.SCGBusinessLogic.TransferenciaItems(ref objCompanyL, true);
                    m_dtBodegasXCentroCosto = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                    m_dtRepuestos = FormularioSBO.DataSources.DataTables.Item(g_strdtTraRepues);
                    m_dtSuministros = FormularioSBO.DataSources.DataTables.Item(g_strdtTraSuminis);

                    m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                    m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                    m_intDocEntry = int.Parse(m_strDocEntry);
                    m_strCodCli = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_CodCli", 0).Trim();
                    m_strNomCli = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NCli", 0).Trim();

                    m_strDraft = m_dtConfigSucursal.GetValue("U_Requis", 0).ToString().Trim();

                    if (m_strDraft == "Y")
                    {
                        m_blnDraft = true;
                    }
                    else
                    {
                        m_blnDraft = false;
                    }

                    m_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item(g_strdtEstadosOT);

                    for (int i = 0; i <= m_dtEstadosOT.Rows.Count - 1; i++)
                    {
                        m_strCodeEstado = m_dtEstadosOT.GetValue("Code", i).ToString().Trim();
                        m_strDescEstado = m_dtEstadosOT.GetValue("Name", i).ToString().Trim();

                        switch (m_strCodeEstado)
                        {
                            case "5":
                                m_strCancelada = m_strDescEstado;
                                break;
                        }
                    }

                    var form = (SAPbouiCOM.Form)FormularioSBO;
                    var application = (SAPbouiCOM.Application)ApplicationSBO;

                    if (!CompanySBO.InTransaction)
                        CompanySBO.StartTransaction();

                    m_objTransferencia.CrearTrasladosCancelacionOT(m_strNoOT, m_strIdSeriesDocumentosTraslado, m_blnDraft, m_intDocEntry, m_strSucursal,
                                                                    ref m_dtRepuestos, ref m_dtSuministros, ref m_dtBodegasXCentroCosto, ref form, ref application, m_strCodCli, m_strNomCli, ref DocEntryRequisicionRepuestos, ref DocEntryRequisicionSuministros);
                    m_oCotizacion = CargaObjetoCotizacion(m_intDocEntry);
                    m_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = m_strCancelada;
                    m_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "5";

                    m_oCotizacion.UserFields.Fields.Item("U_SCGD_Usucan").Value = application.Company.UserName.ToString();


                    for (int x = 0; x < m_oCotizacion.Lines.Count; x++)
                    {
                        m_oCotizacion.Lines.SetCurrentLine(x);
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0;
                        m_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0;
                    }

                    if (SCG.Requisiciones.TransferenciasDirectas.PermiteTransferenciasDirectas(ref m_oCotizacion))
                    {
                        CrearTransferenciasDirectas(DocEntryRequisicionRepuestos, DocEntryRequisicionSuministros, ref ErrorCode, ref ErrorMessage);

                        if (ErrorCode != 0)
                        {
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", ErrorCode, ErrorMessage), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            if (CompanySBO.InTransaction)
                            {
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                        }


                        SCG.Requisiciones.TransferenciasDirectas.AjustarPendientesRequisicion(ref m_oCotizacion, true, ref ErrorCode, ref ErrorMessage);

                        if (ErrorCode != 0)
                        {
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", ErrorCode, ErrorMessage), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            if (CompanySBO.InTransaction)
                            {
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                        }
                    }

                    if (m_oCotizacion.Update() == 0 && ErrorCode == 0)
                    {
                        if (0 == m_oCotizacion.Cancel())
                        {
                            if (CancelarCita(
                                m_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString(),
                                m_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value.ToString(),
                                m_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()))
                            {
                                oGeneralData.SetProperty("U_DEstO", m_strCancelada);
                                oGeneralData.SetProperty("U_EstO", "5");

                                oGeneralService.Update(oGeneralData);
                                if (CompanySBO.InTransaction)
                                {
                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                                    ApplicationSBO.StatusBar.SetText(string.Format(Resource.CancelacionOTExitosa, m_strNoOT), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                }

                                ManejadorEventoFormDataLoad((SAPbouiCOM.Form)FormularioSBO);
                                RecargarFormulario(m_strNoOT);
                            }
                        }
                        else
                        {
                            if (CompanySBO.InTransaction)
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                            CompanySBO.GetLastError(out intError, out strError);
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intError, strError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }
                    else
                    {
                        if (CompanySBO.InTransaction)
                            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                        CompanySBO.GetLastError(out intError, out strError);
                        ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intError, strError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }

                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        /// <summary>
        /// Función que cancela la cita de la OT
        /// </summary>
        /// <param name="p_strSerieCita">Serie de la cita</param>
        /// <param name="p_strNoCita">Número de la cita</param>
        /// <param name="p_strIdSucursal">Id de la sucursal</param>
        /// <returns>Indica si se cancelo la cita</returns>
        private bool CancelarCita(string p_strSerieCita, string p_strNoCita, string p_strIdSucursal)
        {
            bool blnResult;
            string strDocEntry;
            string strEstadoCancelado;
            GeneralService oGeneralService;
            GeneralDataParams oGeneralParams;
            GeneralData oGeneralData;
            try
            {
                blnResult = true;
                if (!string.IsNullOrEmpty(p_strSerieCita) && !string.IsNullOrEmpty(p_strNoCita))
                {
                    strDocEntry = Helpers.EjecutarConsulta(string.Format("Select \"DocEntry\" FROM \"@SCGD_CITA\" WHERE \"U_Num_Serie\" = '{0}' AND \"U_NumCita\" = '{1}' ", p_strSerieCita, p_strNoCita));
                    if (!string.IsNullOrEmpty(strDocEntry))
                    {
                        if (DMS_Connector.Configuracion.ConfiguracionSucursales.Any(x => x.U_Sucurs.Trim().Equals(p_strIdSucursal)))
                        {
                            strEstadoCancelado = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs.Trim().Equals(p_strIdSucursal)).U_CodCitaCancel.Trim();
                            if (!string.IsNullOrEmpty(strEstadoCancelado))
                            {
                                oGeneralService = DMS_Connector.Company.CompanyService.GetGeneralService("SCGD_CIT");
                                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                oGeneralParams.SetProperty("DocEntry", Convert.ToInt32(strDocEntry));
                                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                oGeneralData.SetProperty("U_Estado", strEstadoCancelado);
                                oGeneralService.Update(oGeneralData);
                            }
                            else
                            {
                                throw new Exception(Resource.NoExisteConfTaller);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                blnResult = false;
            }
            return blnResult;
        }

        private void ValidaOTEspecial(ItemEvent pval, ref bool BubbleEvent)
        {
            try
            {
                string m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();

                if (g_strCreaHjaCanPend != "Y")
                {
                    SAPbouiCOM.DataTable m_dtValidaOTEspecial;
                    string m_strConsulta = string.Empty;


                    m_dtValidaOTEspecial = FormularioSBO.DataSources.DataTables.Item(g_strdtValOTEspecial);
                    m_strConsulta = string.Format(g_strConsultaValidacionOTEspecial, m_strDocEntry);

                    m_dtValidaOTEspecial.ExecuteQuery(m_strConsulta);

                    if (int.Parse((m_dtValidaOTEspecial.GetValue("Count", 0).ToString())) > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorValidaOTEspecialCantPend, BoMessageTime.bmt_Short,
                                                         BoStatusBarMessageType.smt_Warning);
                        BubbleEvent = false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ManejaMatriz(ItemEvent pVal, ref bool bubbleEvent, TipoAdicional p_TipoAdicional, bool p_MatrixServicio = false)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtItems;
            string m_strItems = string.Empty;
            string m_strCode = string.Empty;
            string m_strID = string.Empty;
            string m_fechaInicio = string.Empty;
            string m_HoraInicio = string.Empty;
            string m_strCosto = string.Empty;
            string strCodeFase = string.Empty;
            string strNoFase = string.Empty;
            string strEstadoAct = string.Empty;
            DataTable m_dtConsulta;
            string strCostoEstandar = string.Empty;
            bool m_ValorEstandar;
            int m_IdLinea = 0;
            string m_strMatriz = string.Empty;
            bool m_blnEsServExterno = false;
            bool blnActSuspendida;
            string strFinalizaAct2Click = string.Empty;
            string strIdSucursal = string.Empty;
            //Valida si se permite o no agregar tiempo a una actividad finalizada
            bool m_boolAgregarTiempoFinalizada = false;
            string m_strAgregarTiempoFinalizada = string.Empty;
            string m_strEstadoOT = string.Empty;

            try
            {
                switch (p_TipoAdicional)
                {
                    case TipoAdicional.Repuesto:
                        m_strItems = g_strdtRepuestos;
                        break;
                    case TipoAdicional.Suministro:
                        m_strItems = g_strdtSuministros;
                        break;
                    case TipoAdicional.ServicioExterno:
                        m_strItems = g_strdtServiciosExternos;
                        break;
                }

                if (((pVal.ColUID == "Col_code" || pVal.ColUID == "Col_desc") && pVal.Row > 0) && !p_MatrixServicio)
                {
                    dtItems = FormularioSBO.DataSources.DataTables.Item(m_strItems);
                    if (pVal.Row - 1 <= dtItems.Rows.Count - 1)
                    {
                        m_strCode = dtItems.GetValue("code", pVal.Row - 1).ToString().Trim();
                        m_strID = dtItems.GetValue("idit", pVal.Row - 1).ToString().Trim();


                        if (!string.IsNullOrEmpty(m_strCode) && !string.IsNullOrEmpty(m_strID))
                        {
                            CargaFormularioTracking(m_strCode, m_strID);
                        }
                    }
                }

                if ((pVal.ColUID == "Col_col" || pVal.ColUID == "Col_IdAct") && p_MatrixServicio)
                {
                    strIdSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                    m_strEstadoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim();
                    strFinalizaAct2Click = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == strIdSucursal).U_FinalizaAct2Click.Trim();
                    m_strAgregarTiempoFinalizada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == strIdSucursal).U_AgrgTiempFin.Trim();

                    if (m_strAgregarTiempoFinalizada.ToUpper() == "Y")
                    {
                        m_boolAgregarTiempoFinalizada = true;
                    }

                    //Validacion para cargar la ventana de finalizacion de actividad.
                    if (strFinalizaAct2Click == "Y")
                    {
                        m_strMatriz = g_mtxProduccion;
                        oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
                        oMatrix.FlushToDataSource();
                        if (pVal.Row - 1 <= oMatrix.RowCount - 1 && pVal.Row != 0)
                        {
                            if ((FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", pVal.Row - 1).Trim() != "4" || m_boolAgregarTiempoFinalizada == true) && (m_strEstadoOT == "1" || m_strEstadoOT == "2" || m_strEstadoOT == "3"))
                            {
                                m_strID = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", pVal.Row - 1).Trim();
                                m_strCode = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", pVal.Row - 1).Trim();

                                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", pVal.Row - 1).Trim() == "3")
                                {
                                    blnActSuspendida = true;
                                    for (int index = 0; index < FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size; index++)
                                    {
                                        if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index).Trim() == m_strID && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index).Trim() == m_strCode)
                                        {
                                            if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", index).Trim() != "3")
                                            {
                                                blnActSuspendida = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (!blnActSuspendida)
                                    {
                                        ApplicationSBO.StatusBar.SetText(Resource.ErrorSuspenderSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                        return;
                                    }
                                }

                                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                                strCostoEstandar = m_dtConsulta.GetValue("U_TiempoEst_C", 0).ToString();

                                if (strCostoEstandar == "Y")
                                {
                                    m_strCosto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosEst", pVal.Row - 1);
                                    m_ValorEstandar = true;
                                }
                                else
                                {
                                    m_strCosto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosRe", pVal.Row - 1);
                                    m_ValorEstandar = false;
                                }
                                strEstadoAct = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", pVal.Row - 1).Trim();
                                strCodeFase = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CodFas", pVal.Row - 1).Trim();
                                strNoFase = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_NoFas", pVal.Row - 1).Trim();
                                m_fechaInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_DFIni", pVal.Row - 1).Trim();
                                m_HoraInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_HFIni", pVal.Row - 1).Trim();
                                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", pVal.Row - 1).Trim() != "3")
                                    m_IdLinea = int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("LineId", pVal.Row - 1).Trim());
                                else
                                    m_IdLinea = int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("LineId", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size - 1).Trim()) + 1;
                                if (!string.IsNullOrEmpty(m_strCode) && !string.IsNullOrEmpty(m_strID))

                                    CargaFormularioFinalizaAct(m_strCode, m_strID, m_IdLinea, m_ValorEstandar, m_strCosto, m_fechaInicio, m_HoraInicio, strCodeFase, strNoFase, strEstadoAct, pVal.Row);
                            }
                            else
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorFinalizarFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);


                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void EliminarAdicionales(ref DataTable p_dtItemsSeleccionados, string p_strCode)
        {
            int intTamano = p_dtItemsSeleccionados.Rows.Count;

            for (int i = 0; i <= intTamano - 1; i++)
            {
                if (p_dtItemsSeleccionados.GetValue("idit", i).ToString().Trim() == p_strCode)
                {
                    p_dtItemsSeleccionados.Rows.Remove(i);
                    break;
                }
            }
        }

        private void SeleccionarAdicionales(ref DataTable dtItems, ref DataTable p_dtItemsSeleccionados, bool p_blnEsServExterno, SAPbouiCOM.Matrix oMatrix)
        {
            int intTamano = 0;
            string Code = string.Empty;
            string Descripcion = string.Empty;
            string Bodega = string.Empty;
            double Precio = 0.0;
            double Cantidad = 0.0;
            string Moneda = string.Empty;
            string ID = string.Empty;
            string m_strAprobado = string.Empty;
            string m_strTrasladado = string.Empty;
            string m_strPendiente = string.Empty;
            SAPbouiCOM.CheckBox oCheckBox;
            SAPbouiCOM.EditText oEditText;

            try
            {
                oMatrix.FlushToDataSource();
                for (int index = 0; index < dtItems.Rows.Count; index++)
                {
                    if (dtItems.GetValue("sele", index).ToString().Trim().Equals("Y"))
                    {
                        m_strTrasladado = dtItems.GetValue("tras", index).ToString().Trim();
                        m_strAprobado = dtItems.GetValue("apro", index).ToString().Trim();
                        m_strPendiente = dtItems.GetValue("pend", index).ToString().Trim();

                        if ((m_strTrasladado == "1") && m_strAprobado == "1" && m_strPendiente != "0")
                        {
                            intTamano = p_dtItemsSeleccionados.Rows.Count;

                            Code = dtItems.GetValue("code", index).ToString().Trim();
                            Descripcion = dtItems.GetValue("desc", index).ToString().Trim();
                            Bodega = p_blnEsServExterno == false ? dtItems.GetValue("alma", index).ToString().Trim() : string.Empty;
                            Precio = (double)dtItems.GetValue("prec", index);

                            Cantidad = dtItems.GetValue("pend", index).ToString().Trim() != "0" ? (double)dtItems.GetValue("pend", index) : (double)dtItems.GetValue("cant", index);

                            Moneda = dtItems.GetValue("mone", index).ToString().Trim();
                            ID = dtItems.GetValue("idit", index).ToString().Trim();

                            p_dtItemsSeleccionados.Rows.Add(1);
                            p_dtItemsSeleccionados.SetValue("code", intTamano, Code);
                            p_dtItemsSeleccionados.SetValue("desc", intTamano, Descripcion);
                            p_dtItemsSeleccionados.SetValue("alma", intTamano, Bodega);
                            p_dtItemsSeleccionados.SetValue("prec", intTamano, Precio);
                            p_dtItemsSeleccionados.SetValue("cant", intTamano, Cantidad);
                            p_dtItemsSeleccionados.SetValue("mone", intTamano, Moneda);
                            p_dtItemsSeleccionados.SetValue("idit", intTamano, ID);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void FinalizarOrden(ItemEvent p_val, ref bool p_BubbleEvent)
        {
            double m_dblMontoEstandarTotal = 0;
            double m_dblMontoRealTotal = 0;
            double m_dblMontoEstandar = 0;
            double m_dblMontoReal = 0;
            int m_intEstadoFinalizado = 0;
            string m_strDocEntry;
            string m_strNoOT;
            int m_intDocEntry;
            string strError;
            int intError;
            ArrayList aActividades;
            SAPbobsCOM.Documents oCotizacion;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralService oGeneralServiceAva;
            SAPbobsCOM.GeneralData oGeneralDataAva;
            SAPbobsCOM.GeneralDataParams oGeneralParamsAva;

            DateTime fhaActual = DateTime.Now;
            try
            {
                if (p_val.ActionSuccess)
                {
                    string Estado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim();

                    if (Estado == "1" || Estado == "2" || Estado == "3")
                    {
                        ValidaFinalizacionOT(ref p_BubbleEvent);
                    }
                    else
                        p_BubbleEvent = false;

                    if (p_BubbleEvent)
                    {
                        m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                        m_intDocEntry = int.Parse(m_strDocEntry);
                        m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                        List<string> m_lsIds = new List<string>();
                        m_lsIds = ObtieneActividadesSinAsignar();
                        aActividades = new ArrayList();
                        oCompanyService = CompanySBO.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                        oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                        oGeneralParams.SetProperty("Code", m_strNoOT);
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                        oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                        m_intEstadoFinalizado = (int)EstadoOT.Finalizada;

                        oGeneralData.SetProperty("U_FFin", fhaActual);
                        oGeneralData.SetProperty("U_HFin", fhaActual);

                        oGeneralData.SetProperty("U_DEstO", Resource.EstadoFinalizada);
                        oGeneralData.SetProperty("U_EstO", "4");

                        oCotizacion = CargaObjetoCotizacion(m_intDocEntry);

                        FinalizarTodosServicios(m_lsIds, ref oCotizacion, ref oChildrenCtrlCol);                       

                        ObtenerCostoServicioTotal(ref oChildrenCtrlCol, ref m_dblMontoEstandarTotal, ref m_dblMontoRealTotal);

                        oGeneralData.SetProperty("U_MOEsta", m_dblMontoEstandarTotal);
                        oGeneralData.SetProperty("U_MOReal", m_dblMontoRealTotal);

                        if (!CompanySBO.InTransaction)
                            CompanySBO.StartTransaction();

                        oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = m_intEstadoFinalizado.ToString();
                        oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = Resource.EstadoFinalizada;

                        if (oCotizacion.Update() == 0)
                        {
                            oGeneralService.Update(oGeneralData);

                            if (!oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value.ToString().Equals("0"))
                            {
                                oGeneralServiceAva = oCompanyService.GetGeneralService("SCGD_AVA");
                                oGeneralParamsAva = (SAPbobsCOM.GeneralDataParams)oGeneralServiceAva.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                oGeneralParamsAva.SetProperty("DocEntry", oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value);
                                oGeneralDataAva = oGeneralServiceAva.GetByParams(oGeneralParamsAva);
                                oGeneralDataAva.SetProperty("U_RepMeca", oCotizacion.DocTotal);
                                oGeneralServiceAva.Update(oGeneralDataAva);

                                var query = "select usr.USER_CODE from OUSR usr right join OHEM emp on usr.USERID = emp.userId right join OSLP slp on emp.salesPrson = slp.SlpCode where slp.SlpCode = '{0}' ";
                                query = String.Format(query, oGeneralDataAva.GetProperty("U_VenCod"));
                                g_dtConsulta.ExecuteQuery(query);

                                if (g_dtConsulta.Rows.Count > 0 && g_dtConsulta.GetValue(0, 0) != null)
                                {
                                    Utilitarios.EnviarMensaje(String.Format(Resource.MSJOTAvaFinalizada, oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value), g_dtConsulta.GetValue(0, 0).ToString().Trim(), (SAPbobsCOM.Company)CompanySBO);
                                }
                            }

                            if (CompanySBO.InTransaction)
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);

                            ApplicationSBO.StatusBar.SetText(Resource.OrdenFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                            Utilitarios.CreaMensajeSBO(Resource.OrdenFinalizada, m_strDocEntry, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((int)ServicioPostVenta.Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim(), (SAPbouiCOM.Form)FormularioSBO, g_strdtConsulta, false, Utilitarios.RolesMensajeria.EncargadoProduccion, true, (SAPbouiCOM.Application)ApplicationSBO);
                            RecargarFormulario(m_strNoOT);
                            recargarActividades(m_strNoOT, ApplicationSBO);
                        }
                        else
                        {
                            if (CompanySBO.InTransaction)
                            {
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            CompanySBO.GetLastError(out intError, out strError);
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intError, strError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ObtenerCostoServicioTotal(ref SAPbobsCOM.GeneralDataCollection ControlColaborador, ref double CostoEstandarTotal, ref double CostoRealTotal)
        {
            SAPbobsCOM.GeneralData LineaControlColaborador;
            double CostoEstandarLinea = 0;
            double CostoRealLinea = 0;
            List<string> Actividades;
            Dictionary<string, double> ListaCostoEstandar;
            string IDMecanico = string.Empty;
            string IDActividad = string.Empty;
            string Llave = string.Empty;
            try
            {
                Actividades = new List<string>();
                ListaCostoEstandar = new Dictionary<string, double>();
                if (ControlColaborador.Count > 0)
                {
                    for (int i = 0; i < ControlColaborador.Count; i++)
                    {
                        LineaControlColaborador = ControlColaborador.Item(i);
                        IDMecanico = LineaControlColaborador.GetProperty("U_Colab").ToString().Trim();
                        IDActividad = LineaControlColaborador.GetProperty("U_IdAct").ToString().Trim();
                        Llave = string.Format("{0}{1}", IDMecanico, IDActividad);
                        //Solamente se suma el último costo estándar de cada actividad
                        if (!Actividades.Contains(Llave))
                        {
                            CostoEstandarLinea = (double)(LineaControlColaborador.GetProperty("U_CosEst"));
                            Actividades.Add(Llave);
                            //Guarda el costo estándar el último técnico asignado a la actividad
                            if (ListaCostoEstandar.ContainsKey(IDActividad))
                            {
                                ListaCostoEstandar[IDActividad] = CostoEstandarLinea;
                            }
                            else
                            {
                                ListaCostoEstandar.Add(IDActividad, CostoEstandarLinea);
                            }
                        }

                        //Suma el costo real de todas las líneas sin importar el mecánico ni el ID de actividad
                        CostoRealLinea = (double)(LineaControlColaborador.GetProperty("U_CosRe"));
                        CostoRealTotal += CostoRealLinea;
                    }

                    //Realiza la sumatoria de los costos estándar
                    for (int j = 0; j < ListaCostoEstandar.Count; j++)
                    {
                        CostoEstandarTotal += ListaCostoEstandar.Values.ElementAt(j);
                    }
                }
                else
                {
                    CostoEstandarTotal = 0;
                    CostoRealTotal = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ValidaAsignacion(ref bool p_BubbleEvent)
        {
            try
            {
                SAPbouiCOM.DataTable m_dtServicios;
                SAPbouiCOM.DataTable m_dtConsulta;

                string m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                string m_strID = string.Empty;

                string m_strConsulta = "select U_SCGD_ID from QUT1 where U_SCGD_ID not in ({0}) and U_SCGD_NoOT = '{1}' and U_SCGD_TipArt = '2' and U_SCGD_OTHija = '2'";

                m_dtServicios = FormularioSBO.DataSources.DataTables.Item(g_strdtServicios);
                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

                for (int i = 0; i <= m_dtServicios.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        m_strID = string.Format("'{0}'", m_dtServicios.GetValue("idit", i).ToString().Trim());
                    }
                    else
                    {
                        m_strID = string.Format("{0}, '{1}'", m_strID, m_dtServicios.GetValue("idit", i).ToString().Trim());
                    }
                }

                m_strConsulta = string.Format(m_strConsulta, m_strID, m_strNoOT);

                m_dtConsulta.ExecuteQuery(m_strConsulta);

                if (m_dtConsulta.Rows.Count > 0)
                {
                    string m_strRetorno = string.Empty;

                    m_strRetorno = m_dtConsulta.GetValue(0, 0).ToString().Trim();

                    if (string.IsNullOrEmpty(m_strRetorno) == false)
                    {
                        p_BubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorActividadesNoAsignadas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaFinalizacionOT(ref bool p_BubbleEvent)
        {
            SAPbouiCOM.DataTable m_dtConsultaConfiguracion;
            string m_strValidaCantidadesPendientes, m_strValidaCantidadesSolicitadas, m_strValidaEntrega, m_strValidaActividadesSinAsignar = string.Empty;

            var noOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
            var idSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();

            try
            {
                //Validacion previa para saber si Finaliza OT con actividades Pendientes U_FOTAPen -> @SCGD_CONF_SUCURSAL

                if (p_BubbleEvent)
                {
                    m_dtConsultaConfiguracion = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                    m_strValidaActividadesSinAsignar = m_dtConsultaConfiguracion.GetValue("U_FOTAPen", 0).ToString().Trim();
                    if (!string.IsNullOrEmpty(m_strValidaActividadesSinAsignar))
                    {
                        if (m_strValidaActividadesSinAsignar != "Y")
                        {
                            ValidaActividadsinMecanico(ref p_BubbleEvent, noOT);
                            //Valor por default = N, Valida actividades asignadas y finalizadas
                            if (p_BubbleEvent) ValidaActividadesFinalizadas(ref p_BubbleEvent, noOT);
                        }
                    }
                    if (p_BubbleEvent) p_BubbleEvent = ValidaActivadesPendientes();

                    //Validacion cantidades Pendientes
                    if (p_BubbleEvent) ValidaCantidadesPendientes(ref p_BubbleEvent);

                    if (p_BubbleEvent)
                    {
                        m_strValidaCantidadesPendientes = m_dtConsultaConfiguracion.GetValue("U_ValReqPen", 0).ToString().Trim();
                        if (!string.IsNullOrEmpty(m_strValidaCantidadesPendientes))
                        {
                            if (m_strValidaCantidadesPendientes == "Y")
                            {
                                ValidaRequisicionesPendientes(ref p_BubbleEvent, noOT, idSucursal);
                            }
                        }
                    }

                    if (p_BubbleEvent)
                    {
                        m_strValidaCantidadesSolicitadas = m_dtConsultaConfiguracion.GetValue("U_FinOTCanSol", 0).ToString().Trim();
                        if (!string.IsNullOrEmpty(m_strValidaCantidadesSolicitadas) && p_BubbleEvent)
                        {
                            if (m_strValidaCantidadesSolicitadas == "Y")
                            {
                                ValidaCantidadesSolitadas(ref p_BubbleEvent);
                            }
                        }
                    }

                    if (p_BubbleEvent)
                    {
                        m_strValidaEntrega = m_dtConsultaConfiguracion.GetValue("U_Entrega_Rep", 0).ToString().Trim();
                        if (!string.IsNullOrEmpty(m_strValidaEntrega) && p_BubbleEvent)
                        {
                            if (m_strValidaEntrega == "Y")
                            {
                                ValidaEntrega(ref p_BubbleEvent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private bool ValidaActivadesPendientes()
        {
            List<string> ltActividades;
            ltActividades = new List<string>();
            bool blnActividad = true;
            int intRespuesta;

            //Si la OT no tiene actividad retur True
            if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size == 1 &&
                string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", 0).Trim()))
                return true;

            for (int index = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size - 1; index >= 0; index--)
            {
                if (
                    !ltActividades.Contains(string.Format("{0}{1}", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index).Trim(),
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index).Trim())))
                {
                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", index).Trim() != g_strEstado_Finalizado)
                    {
                        blnActividad = false;
                        break;
                    }
                    ltActividades.Add(string.Format("{0}{1}", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index).Trim(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index).Trim()));
                }
            }
            if (!blnActividad)
            {
                intRespuesta = ApplicationSBO.MessageBox(Resource.msgValidaFinalizacionActividadesPendites, 1, Resource.Si, Resource.No);
                if (intRespuesta == 1)
                    blnActividad = true;
            }
            return blnActividad;
        }

        private List<string> ObtieneActividadesSinAsignar()
        {
            string m_strConsulta = " select U_SCGD_ID  from QUT1 where " +
                                   " U_SCGD_NoOT = '{0}' and  " +
                                   " U_SCGD_Sucur = '{1}' and " +
                                   " U_SCGD_Aprobado = '1' and  " +
                                  "  U_SCGD_TipArt = '2' and  " +
                                   " U_SCGD_ID not in (  " +
                                   " select distinct U_IdAct  " +
                                   " from [@SCGD_OT] as otr  " +
                                   " inner join [@SCGD_CTRLCOL] as con  " +
                                   " on otr.Code = con.Code  " +
                                   " where U_NoOT = '{0}' ) ";

            SAPbouiCOM.DataTable dtConsulta;

            string m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
            string strIdSucu = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
            string m_strID = string.Empty;
            List<string> m_lsId = new List<string>();

            try
            {
                m_strConsulta = string.Format(m_strConsulta, m_strNoOT, strIdSucu);
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                dtConsulta.ExecuteQuery(m_strConsulta);

                for (int i = 0; i <= dtConsulta.Rows.Count - 1; i++)
                {
                    m_strID = dtConsulta.GetValue("U_SCGD_ID", i).ToString().Trim();

                    if (string.IsNullOrEmpty(m_strID) == false)
                    {
                        m_lsId.Add(m_strID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_lsId;
        }

        private void ValidaEntrega(ref bool p_BubbleEvent)
        {
            string m_strConsulta = " Select Count(1) as CountN From QUT1 as line with (nolock) Where line.DocEntry = {0} and line.U_SCGD_TipArt in ('1', '3') AND U_SCGD_Aprobado = 1 AND U_SCGD_Compra = 'N' AND line.U_SCGD_Entregado <> 'Y'";
            int intDocentry = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0));
            DataTable dtConsulta;
            try
            {
                m_strConsulta = string.Format(m_strConsulta, intDocentry);
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                dtConsulta.ExecuteQuery(m_strConsulta);
                for (int i = 0; i <= dtConsulta.Rows.Count - 1; i++)
                    if (Convert.ToInt32(dtConsulta.GetValue("CountN", i)) > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorValidaEntrega, BoMessageTime.bmt_Short);
                        p_BubbleEvent = false;
                        break;
                    }
            }
            catch (Exception ex)
            {
            }
        }

        private void ValidaCantidadesSolitadas(ref bool p_BubbleEvent)
        {
            DataTable m_dtRepuestos;
            DataTable m_dtServiciosExternos;
            DataTable m_dtSuministros;

            string m_strCantidad = string.Empty;
            double m_dblcantidad = 0;

            try
            {
                m_dtRepuestos = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestos);
                m_dtServiciosExternos = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosExternos);
                m_dtSuministros = FormularioSBO.DataSources.DataTables.Item(g_strdtSuministros);

                //REPUESTOS
                for (int i = 0; i <= m_dtRepuestos.Rows.Count - 1; i++)
                {
                    m_strCantidad = m_dtRepuestos.GetValue("soli", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoSolicitada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                }

                //SUMINISTROS
                for (int i = 0; i <= m_dtSuministros.Rows.Count - 1; i++)
                {
                    m_strCantidad = m_dtSuministros.GetValue("soli", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoSolicitada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                }

                //SERVICIOS EXTERNOS
                for (int i = 0; i <= m_dtServiciosExternos.Rows.Count - 1; i++)
                {
                    m_strCantidad = m_dtServiciosExternos.GetValue("soli", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoSolicitada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }

                }

            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaRequisicionesPendientes(ref bool p_BubbleEvent, string noOT, string idSucu)
        {
            var query = string.Format(g_strConsultaRequisicionesPendientes, noOT, idSucu);
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

            g_dtConsulta.ExecuteQuery(query);

            if (g_dtConsulta.Rows.Count > 0)
            {
                if (Convert.ToInt32(g_dtConsulta.GetValue(0, 0)) > 0)
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ErrorRequisicionesPendientes, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    p_BubbleEvent = false;
                }
            }

        }

        private void ValidaActividadsinMecanico(ref bool p_BubbleEvent, string noOT)
        {
            var query = string.Format(g_strConsultaActividadessinMecanico, noOT);
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

            g_dtConsulta.ExecuteQuery(query);

            if (g_dtConsulta.Rows.Count > 0)
            {
                if (Convert.ToInt32(g_dtConsulta.GetValue(0, 0)) > 0)
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ErrorActividadesSinMecanico, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    p_BubbleEvent = false;
                }
            }
        }

        private void ValidaActividadesFinalizadas(ref bool p_BubbleEvent, string noOT)
        {
            var query = string.Format(g_strConsultaActividadesSinFinalizar, noOT);
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

            g_dtConsulta.ExecuteQuery(query);

            if (g_dtConsulta.Rows.Count > 0)
            {
                if (Convert.ToInt32(g_dtConsulta.GetValue(0, 0)) > 0)
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ErrorActividadesSinFinalizar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    p_BubbleEvent = false;
                }
            }
        }

        private void ValidaCantidadesPendientes(ref bool p_BubbleEvent)
        {

            SAPbouiCOM.DataTable m_dtRepuestos;
            SAPbouiCOM.DataTable m_dtServiciosExternos;
            SAPbouiCOM.DataTable m_dtSuministros;

            string m_strCantidad = string.Empty;
            double m_dblcantidad = 0;

            try
            {
                m_dtRepuestos = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestos);
                m_dtServiciosExternos = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosExternos);
                m_dtSuministros = FormularioSBO.DataSources.DataTables.Item(g_strdtSuministros);

                //REPUESTOS
                for (int i = 0; i <= m_dtRepuestos.Rows.Count - 1; i++)
                {
                    m_strCantidad = m_dtRepuestos.GetValue("pend", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                    m_strCantidad = m_dtRepuestos.GetValue("pdev", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                    m_strCantidad = m_dtRepuestos.GetValue("ptra", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                    m_strCantidad = m_dtRepuestos.GetValue("pbod", i).ToString().Trim();
                    double.TryParse(m_strCantidad, out m_dblcantidad);

                    if (m_dblcantidad > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        p_BubbleEvent = false;
                        break;
                    }
                }

                if (p_BubbleEvent)
                {
                    //SUMINISTROS
                    for (int i = 0; i <= m_dtSuministros.Rows.Count - 1; i++)
                    {
                        m_strCantidad = m_dtSuministros.GetValue("pend", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtSuministros.GetValue("pdev", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtSuministros.GetValue("ptra", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtSuministros.GetValue("pbod", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                    }
                }

                if (p_BubbleEvent)
                {
                    //SERVICIOS EXTERNOS
                    for (int i = 0; i <= m_dtServiciosExternos.Rows.Count - 1; i++)
                    {
                        m_strCantidad = m_dtServiciosExternos.GetValue("pend", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtServiciosExternos.GetValue("pdev", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtServiciosExternos.GetValue("ptra", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                        m_strCantidad = m_dtServiciosExternos.GetValue("pbod", i).ToString().Trim();
                        double.TryParse(m_strCantidad, out m_dblcantidad);

                        if (m_dblcantidad > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorEstadoPendiente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            p_BubbleEvent = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private bool ValidaExistenciaCambios(string p_strdtItems)
        {
            SAPbouiCOM.DataTable m_dtItems;
            bool result = false;

            try
            {
                m_dtItems = FormularioSBO.DataSources.DataTables.Item(p_strdtItems);

                for (int r = 0; r <= m_dtItems.Rows.Count - 1; r++)
                {
                    if (m_dtItems.GetValue("perm", r).ToString().Trim() == "N")
                    {
                        result = true;
                    }
                    else if (p_strdtItems == "tServiciosExt" || p_strdtItems == "tServiciosExtTemporal")
                    {
                        if (m_dtItems.GetValue("perm", r).ToString().Trim() == "U")
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return result;
        }

        private void ProcesaCotización(ItemEvent pval)
        {
            SAPbobsCOM.Documents oCotizacion;
            TransferenciasStock m_objTransferencia;

            SAPbouiCOM.DataTable m_dtConsulta;
            SAPbouiCOM.DataTable m_dtAdmin;
            SAPbouiCOM.DataTable m_dtConfigSucursal;
            string m_strCardCode = string.Empty;
            string m_strEsCliente = string.Empty;
            string m_strUsaLead = string.Empty;
            string m_strGeneraOT = string.Empty;

            bool blnEsCliente = false;
            bool m_blnDraft;
            string m_strDraft = string.Empty;

            string m_strDocEntriesTransferencias = string.Empty;
            string m_strDocEntry;
            int m_intDocEntry;
            string m_strNoOT = string.Empty;
            string m_strBodegaServExt = string.Empty;
            string m_strBodegaSuministros = string.Empty;
            string m_strBodegaRepuestos = string.Empty;
            string m_strBodegaProceso = string.Empty;
            string m_strIDTransferencia = string.Empty;
            string m_strDocEntriesTransferenciasREP = string.Empty;
            string m_strDocEntriesTransferenciasSUM = string.Empty;
            string m_strPlaca = string.Empty;
            string m_strVIN = string.Empty;
            string m_strDescMarca = string.Empty;
            string m_strDescModelo = string.Empty;
            string m_strDescEstilo = string.Empty;
            string m_strAsesor = string.Empty;
            string m_strCodigoCliente = string.Empty;
            int m_intError;
            string m_strMensajeError = string.Empty;
            int ErrorCode = 0;
            string ErrorMessage = string.Empty;

            try
            {
                m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                m_intDocEntry = int.Parse(m_strDocEntry);
                oCotizacion = CargaObjetoCotizacion(m_intDocEntry);
                m_objTransferencia = new TransferenciasStock((Application)ApplicationSBO, CompanySBO);
                m_dtAdmin = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);
                m_strUsaLead = m_dtAdmin.GetValue("U_UsaLed", 0).ToString().Trim();
                m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                m_strIDTransferencia = m_dtConfigSucursal.GetValue("U_SerInv", 0).ToString().Trim();
                m_strDraft = m_dtConfigSucursal.GetValue("U_Requis", 0).ToString().Trim();

                if (m_strDraft == "Y")
                {
                    m_blnDraft = true;
                }
                else
                {
                    m_blnDraft = false;
                }

                m_strCardCode = oCotizacion.CardCode;
                m_strGeneraOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString().Trim();
                m_strNoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                m_strPlaca = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim();
                m_strVIN = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim();
                m_strDescMarca = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim();
                m_strDescEstilo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim();
                m_strDescModelo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString().Trim();

                if (Utilitarios.IsNumeric(oCotizacion.DocumentsOwner.ToString().Trim()))
                {
                    m_strAsesor = oCotizacion.DocumentsOwner.ToString().Trim();
                }
                else
                {
                    m_strAsesor = "";
                }
                m_strCodigoCliente = oCotizacion.UserFields.Fields.Item("CardCode").Value.ToString().Trim();
                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                g_dtConsulta.ExecuteQuery(string.Format(" Select CardType from OCRD where CardCode = '{0}' ", m_strCardCode.Trim()));

                m_strEsCliente = m_dtConsulta.GetValue(0, 0).ToString().Trim();
                if (m_strEsCliente == "C")
                {
                    blnEsCliente = true;
                }
                else
                {
                    blnEsCliente = false;
                }

                if (blnEsCliente == false && m_strUsaLead == "N")
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ErrorTipoCliente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
                else
                {
                    g_intEstadoCotizacion = (int)CotizacionEstado.SinCambio;
                    if (m_strGeneraOT == "1")
                    {
                        ProcesaLineasCotizacion(ref oCotizacion, oCotizacion, ref m_strBodegaRepuestos, ref m_strBodegaSuministros, ref m_strBodegaServExt, ref m_strBodegaProceso);

                        ApplicationSBO.StatusBar.SetText(Resource.FinalizandoOperaciones, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                        CompanySBO.StartTransaction();

                        m_strDocEntriesTransferencias = m_objTransferencia.CrearTrasladoAddOnNuevo(
                            ref g_listRepuestos,
                            ref g_listSuministros,
                            ref g_listServiciosExternos,
                            ref g_listEliminarRepuestos,
                            ref g_listEliminarSuministros,
                            m_intDocEntry,
                            m_strNoOT,
                            m_strBodegaRepuestos,
                            m_strBodegaSuministros,
                            m_strBodegaServExt,
                            m_strBodegaProceso,
                            m_strIDTransferencia,
                            true,
                            ref m_strDocEntriesTransferenciasREP,
                            ref m_strDocEntriesTransferenciasSUM,
                            m_strDescMarca,
                            m_strDescEstilo,
                            m_strDescModelo,
                            m_strPlaca,
                            m_strVIN,
                            m_strAsesor,
                            m_strCodigoCliente,
                            false,
                            m_blnDraft, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim());

                        oCotizacion.Comments = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Obse", 0);
                        ApplicationSBO.StatusBar.SetText(Resource.ActualizandoCotizacion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                        if (!string.IsNullOrEmpty(m_strDocEntriesTransferenciasREP) || !string.IsNullOrEmpty(m_strDocEntriesTransferenciasSUM))
                        {
                            if (SCG.Requisiciones.TransferenciasDirectas.PermiteTransferenciasDirectas(ref oCotizacion))
                            {
                                CrearTransferenciasDirectas(m_strDocEntriesTransferenciasREP, m_strDocEntriesTransferenciasSUM, ref ErrorCode, ref ErrorMessage);
                                if (ErrorCode != 0)
                                {
                                    throw new ExceptionsSBO(ErrorCode, ErrorMessage);
                                }

                                ApplicationSBO.StatusBar.SetText(Resource.ActualizandoCotizacion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                                SCG.Requisiciones.TransferenciasDirectas.AjustarPendientesRequisicion(ref oCotizacion, false, ref ErrorCode, ref ErrorMessage);
                                if (ErrorCode != 0)
                                {
                                    throw new ExceptionsSBO(ErrorCode, ErrorMessage);
                                }
                            }  
                        }                   

                        if (oCotizacion.Update() != 0)
                        {
                            CompanySBO.GetLastError(out m_intError, out m_strMensajeError);
                            throw new ExceptionsSBO(m_intError, m_strMensajeError);
                        }
                        else
                        {
                            if (CompanySBO.InTransaction)
                            {
                                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                ApplicationSBO.StatusBar.SetText(Resource.ProcesoFinalizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, m_intDocEntry.ToString(), (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((int)Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim(), (SAPbouiCOM.Form)FormularioSBO, g_strdtConsulta, true, Utilitarios.RolesMensajeria.EncargadoProduccion, true, (SAPbouiCOM.Application)ApplicationSBO);
                            }
                        }
                        CargaMatrices(true, true, true, true, false, false);
                    }
                }

            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CrearTransferenciasDirectas(string DocEntryRequisicionRepuestos, string DocEntryRequisicionSuministros, ref int ErrorCode, ref string ErrorMessage)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData RequisicionRepuestos;
            SAPbobsCOM.GeneralData RequisicionSuministros;
            try
            {
                oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                if (!string.IsNullOrEmpty(DocEntryRequisicionRepuestos))
                {
                    oGeneralParams.SetProperty("DocEntry", DocEntryRequisicionRepuestos);
                    RequisicionRepuestos = oGeneralService.GetByParams(oGeneralParams);
                    SCG.Requisiciones.TransferenciasDirectas.CrearTransferencia(ref RequisicionRepuestos, ref ErrorCode, ref ErrorMessage);
                }

                if (!string.IsNullOrEmpty(DocEntryRequisicionSuministros))
                {
                    oGeneralParams.SetProperty("DocEntry", DocEntryRequisicionSuministros);
                    RequisicionSuministros = oGeneralService.GetByParams(oGeneralParams);
                    SCG.Requisiciones.TransferenciasDirectas.CrearTransferencia(ref RequisicionSuministros, ref ErrorCode, ref ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                ErrorCode = 69784;
                ErrorMessage = ex.Message;
            }
        }

        private void ActualizarAdicionales(ItemEvent pval, string p_strdtItems, bool p_blnRepuestos, bool p_blnServicios, bool p_blnServiciosExternos, bool p_blnSuministros)
        {
            SAPbobsCOM.Documents oCotizacion;
            SAPbobsCOM.Document_Lines oLineasCotizacion;

            SAPbouiCOM.DataTable m_dtConfigSucursal;
            SAPbouiCOM.DataTable m_dtAprobacion;

            SAPbouiCOM.DataTable m_dtItems;

            string m_strImpuestosSuministros = string.Empty;
            string m_strImpuestosRepuestos = string.Empty;
            string m_strImpuestosServicios = string.Empty;
            string m_strImpuestosServiciosExternos = string.Empty;
            string m_strAprobacion = string.Empty;

            string m_strDocEntry;
            int m_intDocEntry;
            string m_strSucursalOT = string.Empty;

            string strTipoArticulo = string.Empty;
            int intError;
            string strMensaje;


            try
            {
                m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                m_intDocEntry = int.Parse(m_strDocEntry);

                oCotizacion = CargaObjetoCotizacion(m_intDocEntry);

                m_strSucursalOT = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                if (oCotizacion != null)
                {
                    oLineasCotizacion = oCotizacion.Lines;
                    m_strImpuestosSuministros = m_dtConfigSucursal.GetValue("U_Imp_Suminis", 0).ToString().Trim();
                    m_strImpuestosRepuestos = m_dtConfigSucursal.GetValue("U_Imp_Repuestos", 0).ToString().Trim();
                    m_strImpuestosServicios = m_dtConfigSucursal.GetValue("U_Imp_Serv", 0).ToString().Trim();
                    m_strImpuestosServiciosExternos = m_dtConfigSucursal.GetValue("U_Imp_ServExt", 0).ToString().Trim();

                    m_dtAprobacion = FormularioSBO.DataSources.DataTables.Item(g_strdtAprobacion);
                    m_strAprobacion = m_dtAprobacion.GetValue("U_ItmAprob", 0).ToString().Trim();

                    if (m_strAprobacion == "Y")
                    {
                        m_strAprobacion = "1";
                    }
                    else
                    {
                        m_strAprobacion = "3";
                    }

                    m_dtItems = FormularioSBO.DataSources.DataTables.Item(p_strdtItems);

                    for (int r = 0; r <= m_dtItems.Rows.Count - 1; r++)
                    {
                        if (m_dtItems.GetValue("perm", r).ToString().Trim() == "N")
                        {
                            strTipoArticulo = DevuelveValorItem(m_dtItems.GetValue("code", r).ToString().Trim(), "U_SCGD_TipoArticulo");
                            oLineasCotizacion.Add();
                            oLineasCotizacion.ItemCode = m_dtItems.GetValue("code", r).ToString().Trim();
                            oLineasCotizacion.ItemDescription = m_dtItems.GetValue("desc", r).ToString().Trim();
                            oLineasCotizacion.Quantity = double.Parse(m_dtItems.GetValue("cant", r).ToString().Trim());
                            oLineasCotizacion.UnitPrice = double.Parse(m_dtItems.GetValue("prec", r).ToString().Trim());
                            oLineasCotizacion.Currency = m_dtItems.GetValue("mone", r).ToString().Trim();
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoArticulo;
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NoOT").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Adic").Value = "Y";
                            oLineasCotizacion.DiscountPercent = Utilitarios.GetItemDiscount((SAPbobsCOM.Company)CompanySBO, oCotizacion.CardCode, oLineasCotizacion.ItemCode);

                            if (p_blnServicios)
                            {
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_DurSt").Value = m_dtItems.GetValue("dura", r).ToString().Trim();
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                                {
                                    oLineasCotizacion.TaxCode = m_strImpuestosServicios;
                                    oLineasCotizacion.VatGroup = m_strImpuestosServicios;
                                }
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_strAprobacion;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_FasePro").Value = m_dtItems.GetValue("nofa", r).ToString().Trim();
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value = m_dtItems.GetValue("idit", r).ToString().Trim();
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_strSucursalOT;
                            }
                            if (p_blnRepuestos)
                            {
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendiente).Value = double.Parse(m_dtItems.GetValue("cant", r).ToString().Trim());
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantSolicitada).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantRecibida).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteDevolucion).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteTraslado).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteBodega).Value = 0;
                                oLineasCotizacion.WarehouseCode = m_dtItems.GetValue("alma", r).ToString().Trim();
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                                {
                                    oLineasCotizacion.TaxCode = m_strImpuestosRepuestos;
                                    oLineasCotizacion.VatGroup = m_strImpuestosRepuestos;
                                }
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_strAprobacion;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value = m_dtItems.GetValue("idit", r).ToString().Trim();
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_strSucursalOT;
                            }
                            if (p_blnServiciosExternos)
                            {
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendiente).Value = double.Parse(m_dtItems.GetValue("cant", r).ToString().Trim());
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantSolicitada).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantRecibida).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteDevolucion).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteTraslado).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteBodega).Value = 0;
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                                {
                                oLineasCotizacion.TaxCode = m_strImpuestosServiciosExternos;
                                oLineasCotizacion.VatGroup = m_strImpuestosServiciosExternos;
                                }
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_strAprobacion;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 1;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value = m_dtItems.GetValue("idit", r).ToString().Trim();
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_strSucursalOT;
                            }
                            if (p_blnSuministros)
                            {
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendiente).Value = double.Parse(m_dtItems.GetValue("cant", r).ToString().Trim());
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantSolicitada).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantRecibida).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteDevolucion).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteTraslado).Value = 0;
                                oLineasCotizacion.UserFields.Fields.Item(g_strColCantPendienteBodega).Value = 0;
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                                {
                                    oLineasCotizacion.TaxCode = m_strImpuestosSuministros;
                                    oLineasCotizacion.VatGroup = m_strImpuestosSuministros;
                                }
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_strAprobacion;
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;

                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value = m_dtItems.GetValue("idit", r).ToString().Trim();
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_strSucursalOT;
                            }
                        }
                        else if (m_dtItems.GetValue("perm", r).ToString().Trim() == "U")
                        {
                            for (int index = 0; index < oLineasCotizacion.Count; index++)
                            {
                                oLineasCotizacion.SetCurrentLine(index);

                                if (oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() ==
                                    m_dtItems.GetValue("idit", r).ToString().Trim())
                                {
                                    oLineasCotizacion.UnitPrice = double.Parse(m_dtItems.GetValue("prec", r).ToString().Trim(), n);
                                    break;
                                }
                            }
                        }

                    }
                }

                if (oCotizacion.Update() != 0)
                {
                    CompanySBO.GetLastError(out intError, out strMensaje);
                    if (intError != 0)
                    {
                        ApplicationSBO.SetStatusBarMessage(string.Format("{0}: {1}", intError, strMensaje), BoMessageTime.bmt_Short, true);

                    }
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private SAPbobsCOM.Documents CargaObjetoCotizacion(int p_intDocEntry)
        {
            try
            {
                SAPbobsCOM.Documents oCotizacion;

                oCotizacion = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);

                if (oCotizacion.GetByKey(p_intDocEntry))
                {
                    return oCotizacion;
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return null;
        }

        private bool ProcesaLineasCotizacion(ref SAPbobsCOM.Documents p_oCotizacion, SAPbobsCOM.Documents p_oCotizacionAnterior,
            ref string p_strBodegaRepuestos, ref string p_strBodegaSuministros, ref string p_strBodegaServExternos, ref string p_strBodegaProceso)
        {
            SAPbouiCOM.DataTable m_dtBodegasXCentroCosto;
            SAPbouiCOM.DataTable m_dtADMIN;
            SAPbouiCOM.DataTable m_dtConfSucursal;

            #region "Variables"

            bool m_blnDraft;
            string m_strDraft;
            string m_strSucursal = string.Empty;
            string m_strNoOT = string.Empty;

            string m_strItemCode = string.Empty;
            string m_strTipoArticulo = string.Empty;
            int m_intTipoArticulo;
            string m_strEstadoTrasladado = string.Empty;
            int m_intEstadoTrasladado;
            string m_strEstadoAprobacion = string.Empty;
            int m_intEstadoAprobacion;
            string m_strCentroCosto = string.Empty;
            string m_strGenerico = string.Empty;
            int m_intGenerico = 0;
            double m_dblCantidadItem = 0;

            int m_intEstadoRealTraslado = 0;

            bool m_blnArtBienConfig = false;
            bool m_blmRechazarItem = false;

            string m_strBodegaServExt = string.Empty;
            string m_strBodegaSuministros = string.Empty;
            string m_strBodegaRepuestos = string.Empty;
            string m_strBodegaProceso = string.Empty;

            string m_strCodFase = string.Empty;
            int m_intCodFase;

            int m_intVisOrder = 0;

            int m_intTotalLineasPaquete = 0;

            int m_intCantidadLineasXPaquete = 0;
            int m_intLineaNumFather = -1;
            int m_intEstadoPaquete = 0;

            string m_strDuracionEstandar = string.Empty;
            string m_strBodegaProcesoPorTipo = string.Empty;
            bool m_blnDisminuirCantidad = false;
            double m_dblCantAdicional = 0;
            int m_intEstadoAprobadoItem_Local;
            bool m_blnEsLineaNueva = false;
            TransferenciasStock g_objTransferenciasStock;
            bool m_blnMensajeDevolverEnviado = false;
            string m_strValidacionTiempoEstandar = string.Empty;
            string m_strTiempoEstandar = string.Empty;
            int m_intTotalLineas = 0;
            bool blnCodeConsulta;
            #endregion "Variables"

            string strSEInventariable = string.Empty;
            try
            {
                g_objTransferenciasStock = new TransferenciasStock((Application)ApplicationSBO, CompanySBO);

                blnCodeConsulta = DMS_Connector.Helpers.PermisosMenu("SCGD_RED");
                g_listRepuestos.Clear();
                g_listSuministros.Clear();
                g_listServiciosExternos.Clear();
                g_listEliminarRepuestos.Clear();
                g_listEliminarSuministros.Clear();

                m_dtBodegasXCentroCosto = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                m_dtADMIN = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);
                m_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                m_strSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                m_strNoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();

                m_strDraft = m_dtConfSucursal.GetValue("U_Requis", 0).ToString().Trim();
                m_strValidacionTiempoEstandar = m_dtADMIN.GetValue("U_TiemEsta", 0).ToString().Trim();

                if (m_strDraft == "Y")
                {
                    m_blnDraft = true;
                }
                else
                {
                    m_blnDraft = false;
                }

                ApplicationSBO.StatusBar.SetText(Resource.InicioProc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                for (int m_intNumerLineaCotizacion = 0; m_intNumerLineaCotizacion <= p_oCotizacion.Lines.Count - 1; m_intNumerLineaCotizacion++)
                {
                    p_oCotizacion.Lines.SetCurrentLine(m_intNumerLineaCotizacion);
                    m_dblCantAdicional = 0;
                    m_strItemCode = p_oCotizacion.Lines.ItemCode;
                    m_intVisOrder = p_oCotizacion.Lines.VisualOrder;

                    //VERIFICAR MANEJO VISORDER
                    m_intEstadoRealTraslado = 0;
                    //PROCESANDO 

                    g_blnTipoNoAdmitido = false;
                    g_blnLineaEliminada = false;

                    if (string.IsNullOrEmpty(m_strItemCode) == false)
                    {
                        m_strTipoArticulo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim();
                        int.TryParse(m_strTipoArticulo, out m_intTipoArticulo);

                        if (m_intTipoArticulo == 10)
                        {
                            m_intTipoArticulo = (int)TipoArticulo.Ninguno;
                        }

                        m_strEstadoTrasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();
                        int.TryParse(m_strEstadoTrasladado, out m_intEstadoTrasladado);
                        m_strEstadoAprobacion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim();
                        int.TryParse(m_strEstadoAprobacion, out m_intEstadoAprobacion);

                        m_strGenerico = DevuelveValorItem(m_strItemCode, "U_SCGD_Generico");

                        if (Utilitarios.IsNumeric(m_strGenerico))
                        {
                            m_intGenerico = int.Parse(m_strGenerico);
                        }

                        g_blnTiempoStandar = true;

                        if (m_strValidacionTiempoEstandar.Trim() == "Y")
                        {
                            if (m_intTipoArticulo == (int)TipoArticulo.Servicio)
                            {
                                m_strTiempoEstandar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString().Trim();
                                ValidarTiempoEstandar(m_strTiempoEstandar, ref g_blnTiempoStandar, p_oCotizacion.Lines.ItemCode, p_oCotizacion);
                            }
                        }

                        if (g_blnTiempoStandar)
                        {

                            if ((m_intTipoArticulo > 0 && m_intTipoArticulo != (int)TipoArticulo.Repuesto) ||
                                (m_intTipoArticulo == (int)TipoArticulo.Repuesto && m_intGenerico != 0))
                            {
                                switch (m_intTipoArticulo)
                                {
                                    case (int)TipoArticulo.Paquete:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                         BoYesNoEnum.tNO,
                                                                                         BoYesNoEnum.tYES,
                                                                                         BoYesNoEnum.tNO, true,
                                                                                         m_strSucursal, ref m_strCentroCosto,
                                                                                         true);
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Repuesto:

                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                        BoYesNoEnum.tYES,
                                                                                        BoYesNoEnum.tYES,
                                                                                        BoYesNoEnum.tYES, true,
                                                                                        m_strSucursal, ref m_strCentroCosto,
                                                                                        true);
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Servicio:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                         BoYesNoEnum.tNO,
                                                                                         BoYesNoEnum.tYES,
                                                                                         BoYesNoEnum.tNO, false,
                                                                                         m_strSucursal, ref m_strCentroCosto,
                                                                                         true);

                                        m_strCodFase = DevuelveValorItem(p_oCotizacion.Lines.ItemCode, "U_SCGD_T_Fase");
                                        int.TryParse(m_strCodFase, out m_intCodFase);

                                        if (m_intCodFase == 0)
                                        {
                                            m_blnArtBienConfig = false;
                                        }
                                        if (Utilitarios.IsNumeric(m_strCodFase) == false)
                                        {
                                            m_blnArtBienConfig = false;
                                        }
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.ServExterno:

                                        strSEInventariable = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == m_strSucursal).U_SEInvent.Trim();

                                        if (strSEInventariable == "Y")
                                        {
                                            m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                         BoYesNoEnum.tYES,
                                                                                         BoYesNoEnum.tYES,
                                                                                         BoYesNoEnum.tYES, true,
                                                                                         m_strSucursal, ref m_strCentroCosto,
                                                                                         true);
                                        }
                                        else
                                        {
                                            m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                         BoYesNoEnum.tNO,
                                                                                         BoYesNoEnum.tYES,
                                                                                         BoYesNoEnum.tYES, true,
                                                                                         m_strSucursal, ref m_strCentroCosto,
                                                                                         true);

                                        }
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Suministro:

                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                    BoYesNoEnum.tYES,
                                                                                    BoYesNoEnum.tYES,
                                                                                    BoYesNoEnum.tYES, true,
                                                                                    m_strSucursal, ref m_strCentroCosto,
                                                                                    true);
                                        g_blnTipoNoAdmitido = true;
                                        break;
                                    case (int)TipoArticulo.OtrosIngresos:

                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode,
                                                                                    BoYesNoEnum.tNO,
                                                                                    BoYesNoEnum.tYES,
                                                                                    BoYesNoEnum.tYES, true,
                                                                                    m_strSucursal, ref m_strCentroCosto,
                                                                                    false);
                                        break;
                                    case (int)TipoArticulo.OtrosGastos_Costos:

                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES,
                                                                                    BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, false);
                                        g_blnTipoNoAdmitido = true;
                                        break;
                                }

                                if (m_blnArtBienConfig)
                                {
                                    if (m_intTipoArticulo != (int)TipoArticulo.OtrosIngresos &&
                                        m_intTipoArticulo != (int)TipoArticulo.OtrosGastos_Costos)
                                    {
                                        switch (m_intTipoArticulo)
                                        {
                                            case (int)TipoArticulo.Servicio:

                                                if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").ToString().Trim() == "0" || string.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").ToString().Trim()))
                                                {
                                                    m_strDuracionEstandar = DevuelveValorItem(p_oCotizacion.Lines.ItemCode, "U_SCGD_Duracion");
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = m_strDuracionEstandar;
                                                }

                                                string strEstado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString();
                                                if (string.IsNullOrEmpty(strEstado))
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1";
                                                }

                                                break;
                                            case (int)TipoArticulo.ServExterno:
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y";
                                                break;
                                        }

                                        for (int y = 0; y <= m_dtBodegasXCentroCosto.Rows.Count - 1; y++)
                                        {
                                            if (m_dtBodegasXCentroCosto.GetValue("Sucursal", y).ToString().Trim() == m_strSucursal &&
                                                m_dtBodegasXCentroCosto.GetValue("CentroCosto", y).ToString().Trim() == m_strCentroCosto)
                                            {
                                                m_strBodegaRepuestos = m_dtBodegasXCentroCosto.GetValue("Repuestos", y).ToString().Trim();
                                                p_strBodegaRepuestos = m_strBodegaRepuestos;

                                                m_strBodegaSuministros = m_dtBodegasXCentroCosto.GetValue("Suministros", y).ToString().Trim();
                                                p_strBodegaSuministros = m_strBodegaSuministros;

                                                m_strBodegaServExt = m_dtBodegasXCentroCosto.GetValue("ServExt", y).ToString().Trim();
                                                p_strBodegaServExternos = m_strBodegaServExt;

                                                if (string.IsNullOrEmpty(m_strBodegaProcesoPorTipo))
                                                {
                                                    m_strBodegaProceso = m_dtBodegasXCentroCosto.GetValue("Proceso", y).ToString().Trim();
                                                }
                                                p_strBodegaProceso = m_strBodegaProceso;
                                                break;
                                            }
                                        }

                                        m_dblCantidadItem = 0;

                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOT;
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = m_strCentroCosto;
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = m_strTipoArticulo;
                                        if (string.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()))
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Format("{0}-{1}-{2}", m_strSucursal, p_oCotizacion.Lines.LineNum, m_strNoOT);

                                        if (m_intCantidadLineasXPaquete <= 0)
                                        {
                                            m_intLineaNumFather = -1;
                                            m_intEstadoPaquete = 2;
                                        }

                                        m_blmRechazarItem = false;

                                        if (m_intTipoArticulo == (int)TipoArticulo.Repuesto || m_intTipoArticulo == (int)TipoArticulo.Suministro)
                                        {
                                            if (g_blnPaqueteNoAprobado == true)
                                            {
                                                if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.NoAprobado;
                                                }
                                            }
                                            else
                                            {
                                                if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.Aprobado;
                                                }
                                            }
                                        }

                                        m_intEstadoAprobadoItem_Local = (int)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;

                                        if ((m_intTipoArticulo == (int)TipoArticulo.Repuesto || m_intTipoArticulo == (int)TipoArticulo.Suministro) &&
                                           (m_intEstadoTrasladado == (int)EstadosTraslado.NoProcesado ||
                                            m_intEstadoTrasladado == (int)EstadosTraslado.PendienteTraslado))
                                        {
                                            RevisaStock(p_oCotizacion.Lines, p_oCotizacion.DocEntry, m_strBodegaRepuestos, m_strBodegaSuministros, m_intTipoArticulo, m_intGenerico, m_blnDraft, ref m_dblCantidadItem, ref m_intEstadoRealTraslado, ref m_intCantidadLineasXPaquete, ref m_intTotalLineasPaquete, ref m_intEstadoPaquete, ref m_blmRechazarItem, false);

                                            if (!m_blmRechazarItem)
                                            {
                                                if ((m_intEstadoRealTraslado != (int)ResultadoValidacionPorItem.Comprar && m_intEstadoRealTraslado != (int)ResultadoValidacionPorItem.PendTransf &&
                                                    int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim()) == (int)EstadosTraslado.NoProcesado) &&
                                                    (m_intTipoArticulo == (int)TipoArticulo.Repuesto || m_intTipoArticulo == (int)TipoArticulo.Suministro))
                                                {
                                                    if (m_blnDraft && m_intEstadoRealTraslado == (int)ResultadoValidacionPorItem.PendBodega)
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.PendienteBodega;
                                                    }
                                                    else
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = m_intEstadoRealTraslado;
                                                    }
                                                }
                                                else if (m_intEstadoRealTraslado == (int)ResultadoValidacionPorItem.Comprar)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = (int)EstadosTraslado.No;
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = "PARA COMPRAR";
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = "Y";
                                                }
                                                else if (m_intEstadoRealTraslado == (int)ResultadoValidacionPorItem.PendTransf && (m_intTipoArticulo == (int)TipoArticulo.Suministro || m_intTipoArticulo == (int)TipoArticulo.Repuesto))
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = (int)ResultadoValidacionPorItem.PendTransf;
                                                }

                                                if (p_oCotizacion.Lines.Quantity != m_dblCantidadItem && m_dblCantidadItem != 0)
                                                {
                                                    p_oCotizacion.Lines.Quantity = m_dblCantidadItem;
                                                }

                                                switch (m_intEstadoRealTraslado)
                                                {
                                                    case (int)ResultadoValidacionPorItem.NoAprobar:
                                                        {
                                                            if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString() == "Y")
                                                            {
                                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_dblCantidadItem.ToString(n);
                                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                                            }
                                                            else
                                                            {
                                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_dblCantidadItem.ToString(n);
                                                            }
                                                            break;
                                                        }

                                                    case (int)ResultadoValidacionPorItem.PendTransf:
                                                        {
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = m_dblCantidadItem.ToString(n);
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0;
                                                            break;
                                                        }

                                                    case (int)ResultadoValidacionPorItem.PendBodega:
                                                        {
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = m_dblCantidadItem.ToString(n);
                                                            break;
                                                        }
                                                }//fin del case
                                            }
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.Suministro || m_intTipoArticulo == (int)TipoArticulo.Repuesto)
                                        {
                                            p_oCotizacionAnterior.Lines.SetCurrentLine(m_intVisOrder);

                                            double m_dblCantNuevaCotizacion;
                                            double m_dblCantAntiguaCotizacion;
                                            string m_strEstadoTrasladoNuevaCotizacion = string.Empty;
                                            string m_strValidaReduceCantidad = string.Empty;

                                            m_dblCantNuevaCotizacion = p_oCotizacion.Lines.Quantity;
                                            m_dblCantAntiguaCotizacion = p_oCotizacionAnterior.Lines.Quantity;
                                            m_strEstadoTrasladoNuevaCotizacion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();

                                            if (m_dblCantAntiguaCotizacion < m_dblCantNuevaCotizacion &&
                                                m_strEstadoTrasladoNuevaCotizacion != ResultadoValidacionPorItem.SinCambio.ToString().Trim() &&
                                                m_strEstadoTrasladoNuevaCotizacion != ResultadoValidacionPorItem.PendBodega.ToString().Trim())
                                            {
                                                //Mensaje no se puede aumentar la cantidad de una linea procesada

                                                //VERIFICAR MANEJO DE VIS ORDER Y DEMAS 

                                                p_oCotizacion.Lines.Quantity = p_oCotizacionAnterior.Lines.Quantity;

                                            }
                                            else if (m_dblCantAntiguaCotizacion > m_dblCantNuevaCotizacion)
                                            {
                                                m_strValidaReduceCantidad = m_dtADMIN.GetValue("U_ReduceCant", 0).ToString().Trim();

                                                if (string.IsNullOrEmpty(m_strValidaReduceCantidad))
                                                {
                                                    m_strValidaReduceCantidad = "N";
                                                }

                                                if (m_strEstadoTrasladoNuevaCotizacion == EstadosTraslado.Si.ToString().Trim())
                                                {
                                                    if (m_strValidaReduceCantidad == "Y")
                                                    {

                                                        if (blnCodeConsulta)
                                                        {
                                                            m_blnDisminuirCantidad = true;
                                                            m_dblCantAdicional = (p_oCotizacionAnterior.Lines.Quantity - m_dblCantNuevaCotizacion);
                                                        }
                                                        else
                                                        {
                                                            m_blnDisminuirCantidad = false;
                                                            m_dblCantAdicional = 0;
                                                            p_oCotizacion.Lines.Quantity = p_oCotizacionAnterior.Lines.Quantity;
                                                            ApplicationSBO.StatusBar.SetText(Resource.PermisosDisminuir, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        m_blnDisminuirCantidad = true;
                                                        m_dblCantAdicional = (p_oCotizacionAnterior.Lines.Quantity - m_dblCantNuevaCotizacion);
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_strValidaReduceCantidad == "Y")
                                                    {
                                                        if (blnCodeConsulta)
                                                        {
                                                            p_oCotizacion.Lines.Quantity = p_oCotizacionAnterior.Lines.Quantity;
                                                            ApplicationSBO.StatusBar.SetText(Resource.PermisosDisminuir, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.Servicio)
                                        {
                                            if (g_blnPaqueteNoAprobado)
                                            {
                                                if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.NoAprobado;
                                                    m_intEstadoAprobadoItem_Local = (int)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;
                                                }
                                            }
                                            else
                                            {
                                                if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.Aprobado;
                                                    m_intEstadoAprobadoItem_Local = (int)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;
                                                }
                                            }
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.ServExterno &&
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.Aprobado.ToString().Trim() &&
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim() == EstadosTraslado.NoProcesado.ToString().Trim())
                                        {
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.No;
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = "PARA COMPRAR";
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = "Y";
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.ServExterno &&
                                               p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.NoAprobado.ToString().Trim())
                                        {
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.NoProcesado;
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.Paquete &&
                                              p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.NoAprobado.ToString().Trim())
                                        {
                                            if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                            {
                                                g_blnPaqueteNoAprobado = true;
                                            }
                                        }
                                        else if (m_intTipoArticulo == (int)TipoArticulo.Paquete &&
                                              p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.Aprobado.ToString().Trim())
                                        {
                                            if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                            {
                                                g_blnPaqueteNoAprobado = false;
                                            }
                                        }

                                        if (!m_blmRechazarItem)
                                        {
                                            if ((m_intEstadoAprobadoItem_Local == (int)EstadosAprobacion.Aprobado && m_intCantidadLineasXPaquete <= 0) ||
                                                (m_intEstadoAprobadoItem_Local == (int)EstadosAprobacion.Aprobado && m_intCantidadLineasXPaquete > 0))
                                            {
                                                if (string.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()))
                                                {
                                                    m_blnEsLineaNueva = true;
                                                }
                                                else
                                                {
                                                    m_blnEsLineaNueva = false;
                                                }

                                                if (m_intTipoArticulo == (int)TipoArticulo.Servicio || m_intTipoArticulo == (int)TipoArticulo.ServExterno)
                                                {
                                                    m_intCantidadLineasXPaquete -= 1;
                                                }
                                                else if (m_blnEsLineaNueva || m_intTipoArticulo != (int)TipoArticulo.Paquete)
                                                {
                                                    m_intCantidadLineasXPaquete -= 1;
                                                }
                                            }
                                            else
                                            {
                                                m_blnEsLineaNueva = false;
                                            }

                                            if (m_blnEsLineaNueva == false)
                                            {
                                                if (m_intTipoArticulo != (int)TipoArticulo.Paquete)
                                                {
                                                    if (m_intTipoArticulo == (int)TipoArticulo.Servicio ||
                                                        m_intTipoArticulo == (int)TipoArticulo.ServExterno)
                                                    {
                                                        m_intCantidadLineasXPaquete -= 1;
                                                    }
                                                }
                                                else
                                                {
                                                    m_intEstadoPaquete = m_intEstadoAprobadoItem_Local;
                                                    m_intCantidadLineasXPaquete = -1;
                                                    m_intLineaNumFather = p_oCotizacion.Lines.LineNum;
                                                }
                                            }

                                            string m_strEstadoTrasladoAct = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();
                                            int m_intEstadoTrasladoAct = int.Parse(m_strEstadoTrasladoAct);
                                            string m_strEstadoAprobadoAct = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim();
                                            int m_intEstadoAprobadoAct = int.Parse(m_strEstadoAprobadoAct);

                                            //p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = "4";

                                            if ((m_intEstadoTrasladoAct == (int)EstadosTraslado.NoProcesado &&
                                                m_intEstadoRealTraslado != (int)ResultadoValidacionPorItem.Comprar) ||
                                                m_intEstadoTrasladoAct == (int)EstadosTraslado.PendienteTraslado ||
                                                (m_dblCantAdicional > 0 && m_blnDisminuirCantidad == false))
                                            {
                                                // SAPbobsCOM.Document_Lines objLineas = new SAPbobsCOM.Document_Lines();
                                                //objLineas = p_oCotizacion.Lines;

                                                switch (m_intTipoArticulo)
                                                {
                                                    case g_strRepuesto:
                                                        if (m_dblCantAdicional == 0)
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaRepuestos,
                                                                ref g_listRepuestos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                        }
                                                        else
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaRepuestos,
                                                                ref g_listRepuestos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                true,
                                                                m_blnDraft,
                                                                m_dblCantAdicional,
                                                                p_oCotizacion.DocEntry);
                                                        }
                                                        break;

                                                    case g_strSuministro:
                                                        if (m_dblCantAdicional == 0)
                                                        {
                                                            if (g_intRealizarTraslados == RealizarTraslado.Si)
                                                            {
                                                                g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                            }
                                                            else if (g_intRealizarTraslados == RealizarTraslado.No)
                                                            {
                                                                g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                true,
                                                                m_blnDraft,
                                                                m_dblCantAdicional,
                                                                p_oCotizacion.DocEntry);
                                                        }

                                                        break;

                                                    case g_strServExterno:
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaServiciosExternos,
                                                                ref g_listServiciosExternos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                        break;
                                                }
                                                if (m_intTipoArticulo != (int)TipoArticulo.Paquete && m_intTipoArticulo != (int)TipoArticulo.Servicio && m_intTipoArticulo != (int)TipoArticulo.ServExterno)
                                                {
                                                    m_intCantidadLineasXPaquete -= 1;
                                                }
                                            }
                                            else if ((m_blnDraft && m_intEstadoRealTraslado == (int)ResultadoValidacionPorItem.PendBodega &&
                                                m_intEstadoTrasladoAct == (int)EstadosTraslado.PendienteBodega) ||
                                                m_intEstadoAprobadoAct == (int)EstadosAprobacion.NoAprobado)
                                            {
                                                switch (m_intTipoArticulo)
                                                {
                                                    case g_strRepuesto:
                                                        if (m_dblCantAdicional == 0)
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaRepuestos,
                                                                ref g_listRepuestos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                        }
                                                        else
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaRepuestos,
                                                                ref g_listRepuestos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                true,
                                                                m_blnDraft,
                                                                m_dblCantAdicional,
                                                                p_oCotizacion.DocEntry);
                                                        }
                                                        break;

                                                    case g_strSuministro:
                                                        if (m_dblCantAdicional == 0)
                                                        {
                                                            if (g_intRealizarTraslados == RealizarTraslado.Si)
                                                            {
                                                                g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                            }
                                                            else if (g_intRealizarTraslados == RealizarTraslado.No)
                                                            {
                                                                g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaSuministros,
                                                                ref g_listSuministros,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                true,
                                                                m_blnDraft,
                                                                m_dblCantAdicional,
                                                                p_oCotizacion.DocEntry);
                                                        }

                                                        break;

                                                    case g_strServExterno:
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                                TransferenciasStock.TipoMovimiento.TransferenciaServiciosExternos,
                                                                ref g_listServiciosExternos,
                                                                ref p_oCotizacion,
                                                                m_strBodegaRepuestos,
                                                                m_strBodegaSuministros,
                                                                m_strBodegaServExt,
                                                                m_strBodegaProceso,
                                                                true,
                                                                m_intTipoArticulo,
                                                                m_intEstadoPaquete,
                                                                m_intCantidadLineasXPaquete,
                                                                m_intGenerico,
                                                                false,
                                                                m_blnDraft,
                                                                0,
                                                                p_oCotizacion.DocEntry);
                                                        break;
                                                }
                                                if (m_intTipoArticulo != (int)TipoArticulo.Paquete && m_intTipoArticulo != (int)TipoArticulo.Servicio && m_intTipoArticulo != (int)TipoArticulo.ServExterno)
                                                {
                                                    m_intCantidadLineasXPaquete -= 1;
                                                }
                                            }
                                        }
                                        else //Rechazar
                                        {
                                            if (m_intLineaNumFather != -1)
                                            {
                                                int m_intLineActual;

                                                m_intLineActual = p_oCotizacion.Lines.LineNum;
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.NoAprobado;

                                                p_oCotizacion.Lines.SetCurrentLine(m_intLineActual);

                                                m_intEstadoPaquete = (int)EstadosAprobacion.NoAprobado;

                                                m_intCantidadLineasXPaquete -= 1;

                                            }
                                            else
                                            {
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = EstadosAprobacion.NoAprobado;
                                            }
                                        }
                                        if ((g_blnLineaEliminada && m_intTipoArticulo != (int)TipoArticulo.Servicio && m_intTipoArticulo != (int)TipoArticulo.Paquete && m_intTipoArticulo != (int)TipoArticulo.ServExterno) ||
                                            m_dblCantAdicional > 0 && m_blnDisminuirCantidad)
                                        {
                                            switch (m_intTipoArticulo)
                                            {
                                                case g_strRepuesto:
                                                    if (m_blnDisminuirCantidad == false)
                                                    {
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                            TransferenciasStock.TipoMovimiento.TransferenciaItemsEmininar,
                                                            ref g_listEliminarRepuestos,
                                                            ref p_oCotizacion,
                                                            m_strBodegaRepuestos,
                                                            m_strBodegaSuministros,
                                                            m_strBodegaServExt,
                                                            m_strBodegaProceso,
                                                            true,
                                                            m_intTipoArticulo,
                                                            m_intEstadoPaquete,
                                                            m_intCantidadLineasXPaquete,
                                                            m_intGenerico,
                                                            false,
                                                            m_blnDraft,
                                                            0,
                                                            p_oCotizacion.DocEntry);
                                                    }
                                                    else
                                                    {
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                            TransferenciasStock.TipoMovimiento.TransferenciaItemsEmininar,
                                                            ref g_listEliminarRepuestos,
                                                            ref p_oCotizacion,
                                                            m_strBodegaRepuestos,
                                                            m_strBodegaSuministros,
                                                            m_strBodegaServExt,
                                                            m_strBodegaProceso,
                                                            true,
                                                            m_intTipoArticulo,
                                                            m_intEstadoPaquete,
                                                            m_intCantidadLineasXPaquete,
                                                            m_intGenerico,
                                                            true,
                                                            m_blnDraft,
                                                            m_dblCantAdicional,
                                                            p_oCotizacion.DocEntry);
                                                    }

                                                    if (g_listEliminarRepuestos.Count != 0 && m_blnMensajeDevolverEnviado == false)
                                                    {
                                                        //RECURSOS
                                                        ApplicationSBO.MessageBox("Los Items no Aprobados se van a devolver");
                                                        m_blnMensajeDevolverEnviado = true;
                                                    }

                                                    break;

                                                case g_strSuministro:
                                                    if (m_blnDisminuirCantidad == false)
                                                    {
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                            TransferenciasStock.TipoMovimiento.TransferenciaItemsEmininar,
                                                            ref g_listEliminarSuministros,
                                                            ref p_oCotizacion,
                                                            m_strBodegaRepuestos,
                                                            m_strBodegaSuministros,
                                                            m_strBodegaServExt,
                                                            m_strBodegaProceso,
                                                            true,
                                                            m_intTipoArticulo,
                                                            m_intEstadoPaquete,
                                                            m_intCantidadLineasXPaquete,
                                                            m_intGenerico,
                                                            false,
                                                            m_blnDraft,
                                                            0,
                                                            p_oCotizacion.DocEntry);

                                                    }
                                                    else
                                                    {
                                                        g_objTransferenciasStock.GeneraListasTransferencias(
                                                            TransferenciasStock.TipoMovimiento.TransferenciaItemsEmininar,
                                                            ref g_listEliminarSuministros,
                                                            ref p_oCotizacion,
                                                            m_strBodegaRepuestos,
                                                            m_strBodegaSuministros,
                                                            m_strBodegaServExt,
                                                            m_strBodegaProceso,
                                                            true,
                                                            m_intTipoArticulo,
                                                            m_intEstadoPaquete,
                                                            m_intCantidadLineasXPaquete,
                                                            m_intGenerico,
                                                            true,
                                                            m_blnDraft,
                                                            m_dblCantAdicional,
                                                            p_oCotizacion.DocEntry);
                                                    }
                                                    break;
                                            }
                                            m_intCantidadLineasXPaquete -= 1;
                                        }

                                        if (g_intEstadoCotizacion == (int)CotizacionEstado.SinCambio)
                                        {
                                            if (m_intNumerLineaCotizacion < p_oCotizacion.Lines.Count)
                                            {
                                                p_oCotizacion.Lines.SetCurrentLine(m_intVisOrder);

                                                if (p_oCotizacionAnterior.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value !=
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value)
                                                {
                                                    g_intEstadoCotizacion = (int)CotizacionEstado.Modificada;
                                                }
                                            }
                                        }

                                        if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim()) == (int)EstadosAprobacion.NoAprobado &&
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString().Trim() == "Y")
                                        {
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0;
                                        }

                                        if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim()) == (int)EstadosAprobacion.Aprobado &&
                                            p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iSalesTree)
                                        {
                                            g_blnProcesarSi = true;
                                        }
                                        else if (p_oCotizacion.Lines.TreeType != SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                        {
                                            g_blnProcesarSi = true;
                                        }

                                        if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim()) == (int)EstadosAprobacion.NoAprobado &&
                                            p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iSalesTree)
                                        {
                                            g_blnProcesarNo = true;
                                        }
                                        else if (p_oCotizacion.Lines.TreeType != SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                        {
                                            g_blnProcesarNo = true;
                                        }

                                        if (g_blnProcesarSi)
                                        {
                                            if (p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                            {
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value =
                                                    LineaAProcesar.Si;
                                            }
                                        }
                                        else if (g_blnProcesarNo)
                                        {
                                            if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.NoAprobado.ToString().Trim() &&
                                                p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                            {
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value =
                                                    LineaAProcesar.No;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value;
                                    }

                                }
                                else
                                {
                                    if (g_blnTipoNoAdmitido)
                                    {
                                        ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ItemMalConfig),
                                            BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                                    }
                                }
                            }
                            else
                            {
                                if (g_blnTipoNoAdmitido)
                                {
                                    ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ItemMalConfig),
                                        BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                }
                            }
                        }
                        else
                        {
                            ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ValTiempoEstandar),
                                       BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                        }
                    }


                }
                return false;
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return false;
        }

        private void ValidarTiempoEstandar(string p_strTiempoEstandar, ref bool p_blnTiempoStandar, string p_strItemCode, SAPbobsCOM.Documents p_oCotizacion)
        {
            SAPbouiCOM.DataTable m_dtConsulta;
            SAPbouiCOM.DataTable m_dtAdmin;
            string m_strUsaEspecificaciones = string.Empty;
            string m_strUsaAsociacionXEspecificaciones = string.Empty;
            string m_strDuracion = string.Empty;
            string m_strConsultaModelo = "Select U_Duracion from [@SCGD_SERVXESPECIFIC] where U_ItemCode = '{0}' and U_CodeModelo = '{1}'";
            string m_strConsultaEstilo = "Select U_Duracion from [@SCGD_SERVXESPECIFIC] where U_ItemCode = '{0}' and U_CodeEstilo = '{1}'";
            string m_strConsulta = string.Empty;
            string m_strCodEstilo = string.Empty;
            string m_strCodModelo = string.Empty;

            try
            {
                m_dtAdmin = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);
                m_strUsaAsociacionXEspecificaciones = m_dtAdmin.GetValue("U_UsaAXEV", 0).ToString().Trim();
                m_strUsaEspecificaciones = m_dtAdmin.GetValue("U_EspVehic", 0).ToString().Trim();

                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

                if (m_strUsaAsociacionXEspecificaciones == "Y")
                {
                    if (m_strUsaEspecificaciones == "E")
                    {
                        m_strCodEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString().Trim();
                        m_strConsulta = string.Format(m_strConsultaEstilo, p_strItemCode, m_strCodEstilo);
                        m_dtConsulta.ExecuteQuery(m_strConsulta);
                        m_strDuracion = m_dtConsulta.GetValue(0, 0).ToString().Trim();
                    }
                    else if (m_strUsaEspecificaciones == "M")
                    {
                        m_strCodModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString().Trim();
                        m_strConsulta = string.Format(m_strConsultaModelo, p_strItemCode, m_strCodModelo);
                        m_dtConsulta.ExecuteQuery(m_strConsulta);
                        m_strDuracion = m_dtConsulta.GetValue(0, 0).ToString().Trim();
                    }

                    if (string.IsNullOrEmpty(m_strDuracion) == false)
                    {
                        p_blnTiempoStandar = true;
                    }
                    else
                    {
                        p_blnTiempoStandar = false;

                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(p_strTiempoEstandar))
                    {
                        p_blnTiempoStandar = false;
                    }
                    else
                    {
                        if (int.Parse(p_strTiempoEstandar) == 0)
                        {
                            p_blnTiempoStandar = false;
                        }
                        else
                        {
                            p_blnTiempoStandar = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private string DevuelveValorItem(string p_strItemCode, string p_UDF)
        {
            SAPbobsCOM.IItems oItem;
            string m_strValorRetorno = string.Empty;

            try
            {
                oItem = (IItems)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oItem.GetByKey(p_strItemCode);
                m_strValorRetorno = oItem.UserFields.Fields.Item(p_UDF).Value.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_strValorRetorno;
        }

        private void RevisaStock(Document_Lines p_oLines,
                                int p_intDocEntry,
                                string p_strBodegaRepuestos,
                                string p_strBodegaSuministros,
                                int p_intTipoArticulo,
                                int p_intGenerico,
                                bool p_blnDraft,
                                ref double p_dblCantidadItem,
                                ref int p_intEstadoTraslado,
                                ref int p_intCantidadItemsPaquete,
                                ref int p_intCantidadItemsTotal,
                                ref int p_intEstadoPaquete,
                                ref bool p_blnRechazarItem,
                                bool p_blnActualizarCantidad,
                                double p_dblCantidadAdicional = 0)
        {
            string m_strEstadoAprobacion = string.Empty;
            string m_strEstadoTraslado = string.Empty;
            int m_intEstadoAprobacion;
            int m_intEstadoTraslado;

            ResultadoValidacionPorItem m_strValidaciónResultado;
            double m_dblCantidad;

            try
            {
                m_strEstadoAprobacion = p_oLines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim();
                int.TryParse(m_strEstadoAprobacion, out m_intEstadoAprobacion);
                m_strEstadoTraslado = p_oLines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();
                int.TryParse(m_strEstadoTraslado, out m_intEstadoTraslado);
                if (p_intTipoArticulo != (int)TipoArticulo.Paquete)
                {
                    if ((((m_intEstadoAprobacion == (int)EstadosAprobacion.Aprobado && p_intCantidadItemsPaquete <= 0) ||
                        (p_intEstadoPaquete == (int)EstadosAprobacion.Aprobado && p_intCantidadItemsPaquete > 0)) &&
                        m_intEstadoTraslado == (int)EstadosTraslado.NoProcesado) ||
                        p_blnActualizarCantidad)
                    {
                        if (p_intTipoArticulo == (int)TipoArticulo.Repuesto && p_intGenerico == 1)
                        {
                            if (p_blnActualizarCantidad == false)
                            {
                                m_dblCantidad = p_oLines.Quantity;
                            }
                            else
                            {
                                m_dblCantidad = p_dblCantidadAdicional;
                            }

                            m_strValidaciónResultado = ValidarCantidadDisponibleRepuestos(p_oLines.ItemCode, p_oLines.ItemDescription, p_oLines.LineNum, p_intDocEntry, ref m_dblCantidad, p_strBodegaRepuestos, p_blnActualizarCantidad, m_intEstadoTraslado, p_blnDraft);

                            switch (m_strValidaciónResultado)
                            {
                                case ResultadoValidacionPorItem.NoAprobar:
                                    p_blnRechazarItem = true;
                                    break;
                                case ResultadoValidacionPorItem.ModifCantiCotizacion:
                                    if (p_blnDraft)
                                    {
                                        p_intEstadoTraslado = (int)EstadosTraslado.PendienteBodega;
                                    }
                                    else
                                    {
                                        p_intEstadoTraslado = (int)EstadosTraslado.Si;
                                    }
                                    p_dblCantidadItem = m_dblCantidad;
                                    g_intRealizarTraslados = RealizarTraslado.Si;
                                    break;
                                case ResultadoValidacionPorItem.PendTransf:
                                    p_intEstadoTraslado = (int)EstadosTraslado.PendienteTraslado;
                                    g_intRealizarTraslados = RealizarTraslado.Si;
                                    p_dblCantidadItem = m_dblCantidad;
                                    break;
                                case ResultadoValidacionPorItem.Comprar:
                                    if (p_blnActualizarCantidad == false)
                                    {
                                        p_dblCantidadItem = p_oLines.Quantity;
                                    }
                                    else
                                    {
                                        m_dblCantidad = p_dblCantidadAdicional;
                                    }
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.Comprar;
                                    g_intRealizarTraslados = RealizarTraslado.Si;
                                    p_oLines.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLines.Quantity;
                                    p_oLines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y";
                                    break;

                                case ResultadoValidacionPorItem.SinCambio:
                                    {
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        g_intRealizarTraslados = RealizarTraslado.No;
                                        break;
                                    }

                                default:
                                    if (p_blnDraft)
                                    {
                                        if (p_blnActualizarCantidad == false)
                                        {
                                            p_dblCantidadItem = p_oLines.Quantity;
                                        }
                                        else
                                        {
                                            m_dblCantidad = p_dblCantidadAdicional;
                                        }
                                        p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendBodega;
                                        g_intRealizarTraslados = RealizarTraslado.No;
                                    }
                                    else
                                    {
                                        if (p_blnActualizarCantidad == false)
                                        {
                                            p_dblCantidadItem = p_oLines.Quantity;
                                        }
                                        else
                                        {
                                            m_dblCantidad = p_dblCantidadAdicional;
                                        }
                                        p_intEstadoTraslado = (int)ResultadoValidacionPorItem.ModifCantiCotizacion;
                                        g_intRealizarTraslados = RealizarTraslado.Si;
                                    }
                                    break;
                            }
                        }
                        else if (p_intTipoArticulo == (int)TipoArticulo.Suministro)
                        {
                            if (p_blnActualizarCantidad == false)
                            {
                                m_dblCantidad = p_oLines.Quantity;
                            }
                            else
                            {
                                m_dblCantidad = p_dblCantidadAdicional;
                            }

                            m_strValidaciónResultado = ValidarCantidadDisponibleSuministros(p_oLines.ItemCode, p_oLines.ItemDescription, m_dblCantidad, p_strBodegaSuministros, p_oLines.LineNum, p_intDocEntry, p_blnDraft);

                            if (m_strValidaciónResultado == ResultadoValidacionPorItem.PendTransf)
                            {
                                p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendTransf;
                                g_intRealizarTraslados = RealizarTraslado.No;
                                p_dblCantidadItem = m_dblCantidad;
                            }
                            else
                            {
                                if (p_blnDraft == true)
                                {
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendBodega;
                                    g_intRealizarTraslados = RealizarTraslado.No;
                                }
                                else
                                {
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    g_intRealizarTraslados = RealizarTraslado.Si;

                                }
                            }
                        }
                    }
                    else if (((m_intEstadoAprobacion == (int)EstadosAprobacion.Aprobado && p_intCantidadItemsPaquete <= 0) ||
                        (p_intEstadoPaquete == (int)EstadosAprobacion.Aprobado && p_intCantidadItemsPaquete > 0)) &&
                        m_intEstadoTraslado == (int)EstadosTraslado.PendienteTraslado)
                    {
                        if (p_intTipoArticulo == (int)TipoArticulo.Repuesto && p_intGenerico == 1)
                        {
                            m_dblCantidad = p_oLines.Quantity;

                            m_strValidaciónResultado = ValidarCantidadDisponibleRepuestos(p_oLines.ItemCode, p_oLines.ItemDescription, p_oLines.LineNum, p_intDocEntry, ref m_dblCantidad, p_strBodegaRepuestos, p_blnActualizarCantidad, m_intEstadoTraslado, p_blnDraft);

                            switch (m_strValidaciónResultado)
                            {
                                case ResultadoValidacionPorItem.SinCambio:
                                    if (p_blnDraft)
                                    {
                                        g_intRealizarTraslados = RealizarTraslado.No;
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendTransf;
                                    }
                                    else
                                    {
                                        g_intRealizarTraslados = RealizarTraslado.Si;
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        p_intEstadoTraslado = (int)ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    }
                                    break;
                                case ResultadoValidacionPorItem.PendBodega:
                                    g_intRealizarTraslados = RealizarTraslado.No;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendBodega;
                                    break;
                            }
                        }
                        else if (p_intTipoArticulo == (int)TipoArticulo.Suministro)
                        {
                            m_dblCantidad = p_oLines.Quantity;

                            m_strValidaciónResultado = ValidarCantidadDisponibleSuministros(p_oLines.ItemCode, p_oLines.ItemDescription, m_dblCantidad, p_strBodegaSuministros, p_oLines.LineNum, p_intDocEntry, p_blnDraft);

                            if (m_strValidaciónResultado == ResultadoValidacionPorItem.PendTransf)
                            {
                                p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendTransf;
                                g_intRealizarTraslados = RealizarTraslado.No;
                                p_dblCantidadItem = p_oLines.Quantity;
                            }
                            else if (m_strValidaciónResultado == ResultadoValidacionPorItem.PendBodega)
                            {
                                if (p_blnDraft)
                                {
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.PendBodega;
                                    g_intRealizarTraslados = RealizarTraslado.No;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                }
                                else
                                {
                                    p_intEstadoTraslado = (int)ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    g_intRealizarTraslados = RealizarTraslado.Si;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                }
                            }
                            else
                            {
                                p_dblCantidadItem = p_oLines.Quantity;
                                p_intEstadoTraslado = (int)ResultadoValidacionPorItem.ModifCantiCotizacion;
                                g_intRealizarTraslados = RealizarTraslado.Si;
                            }
                        }
                    }
                    else
                    {
                        p_dblCantidadItem = p_oLines.Quantity;
                    }
                }
                else
                {
                    //MANEJO DE PAQUETES
                    p_intCantidadItemsPaquete = -1;
                    p_intCantidadItemsTotal = p_intCantidadItemsPaquete;
                    p_intEstadoPaquete = (int)p_oLines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private ResultadoValidacionPorItem ValidarCantidadDisponibleSuministros(
            string p_strItemCode,
            string p_strDescription,
            double p_decCantidad,
            string p_strBodegaSuministros,
            int p_intLineNum,
            int p_intDocEntry,
            bool p_blnDraft)
        {

            double m_dblCantidad;
            double m_dblCantidadLineasAnteriores;
            ResultadoValidacionPorItem m_lResultado = ResultadoValidacionPorItem.SinCambio;

            try
            {
                m_dblCantidad = DevuelveStockDisponibleXItem(p_strItemCode, p_strBodegaSuministros, CompanySBO);
                m_dblCantidadLineasAnteriores = DevuelveCantidadLineasAnteriores(p_strItemCode, p_intLineNum, p_intDocEntry, CompanySBO);
                if ((m_dblCantidad - m_dblCantidadLineasAnteriores) <= 0)
                {
                    m_lResultado = ResultadoValidacionPorItem.PendTransf;
                }
                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) < p_decCantidad)
                {
                    m_lResultado = ResultadoValidacionPorItem.PendTransf;
                }
                else
                {
                    if (p_blnDraft == true)
                    {
                        g_intRealizarTraslados = RealizarTraslado.No;
                        m_lResultado = ResultadoValidacionPorItem.PendBodega;
                    }
                    else
                    {
                        g_intRealizarTraslados = RealizarTraslado.Si;
                        m_lResultado = ResultadoValidacionPorItem.SinCambio;
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

            return m_lResultado;
        }

        private ResultadoValidacionPorItem ValidarCantidadDisponibleRepuestos(
            string p_ItemCode,
            string p_ItemDescription,
            int p_LineNum,
            int p_DocEntry,
            ref double p_DecCantidadItem,
            string p_StrBodegaRepuestos,
            bool p_blnActualizarCantidad,
            int p_intEstadoTraslado,
            bool p_blnDraft)
        {
            double m_dblCantidad;
            double m_dblCantidadLineasAnteriores;
            int m_intMsjResult;
            ResultadoValidacionPorItem m_lResultado = ResultadoValidacionPorItem.SinCambio;
            ListaCantidadesAnteriores m_objCantidadAnterior = new ListaCantidadesAnteriores();

            try
            {
                m_dblCantidad = DevuelveStockDisponibleXItem(p_ItemCode, p_StrBodegaRepuestos, CompanySBO);
                m_dblCantidadLineasAnteriores = DevuelveCantidadLineasAnteriores(p_ItemCode, p_LineNum, p_DocEntry, CompanySBO);

                if (m_dblCantidadLineasAnteriores != 0)
                {
                    m_objCantidadAnterior.Cantidad = m_dblCantidadLineasAnteriores;
                    m_objCantidadAnterior.ItemCode = p_ItemCode;
                    m_objCantidadAnterior.LineNum = p_LineNum;

                    g_lstCantidadesAnteriores.Add(m_objCantidadAnterior);
                }

                if ((m_dblCantidad - m_dblCantidadLineasAnteriores) <= 0 && p_intEstadoTraslado == (int)EstadosTraslado.NoProcesado)
                {
                    if (p_blnActualizarCantidad == false)
                    {
                        m_intMsjResult = ApplicationSBO.MessageBox(string.Format("{0} {1} {2}", Resource.ElItem, p_ItemCode, Resource.SinInventario), 1, Resource.Comprar, Resource.Rechazar, Resource.Trasladar);
                    }
                    else
                    {
                        m_intMsjResult = ApplicationSBO.MessageBox(string.Format("{0} {1} {2}", Resource.ElItem, p_ItemCode, Resource.SinInventario), 1, Resource.Comprar, Resource.Rechazar);
                    }
                    switch (m_intMsjResult)
                    {
                        case 1:
                            m_lResultado = ResultadoValidacionPorItem.Comprar;
                            break;
                        case 2:
                            m_lResultado = ResultadoValidacionPorItem.NoAprobar;
                            break;
                        case 3:
                            m_lResultado = ResultadoValidacionPorItem.PendTransf;
                            break;
                    }
                }

                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) < p_DecCantidadItem && p_intEstadoTraslado == (int)EstadosTraslado.NoProcesado)
                {
                    m_intMsjResult = ApplicationSBO.MessageBox(string.Format("{0} {1} {2}", Resource.ElItem, p_ItemCode, Resource.SinInventario), 1, Resource.PendTraslado, Resource.Rechazar, Resource.Trasladar);

                    switch (m_intMsjResult)
                    {
                        case 1:
                            m_lResultado = ResultadoValidacionPorItem.PendTransf;
                            break;
                        case 2:
                            m_lResultado = ResultadoValidacionPorItem.NoAprobar;
                            break;
                        case 3:
                            m_lResultado = ResultadoValidacionPorItem.ModifCantiCotizacion;
                            p_DecCantidadItem = m_dblCantidad;
                            break;
                    }
                }

                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) <= p_DecCantidadItem && p_intEstadoTraslado == (int)EstadosTraslado.PendienteTraslado)
                {
                    m_lResultado = ResultadoValidacionPorItem.SinCambio;
                }

                else
                {
                    if (p_blnDraft)
                    {
                        g_intRealizarTraslados = RealizarTraslado.No;
                        m_lResultado = ResultadoValidacionPorItem.PendBodega;
                    }
                    else
                    {
                        g_intRealizarTraslados = RealizarTraslado.Si;
                        m_lResultado = ResultadoValidacionPorItem.SinCambio;
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_lResultado;
        }

        public double DevuelveCantidadLineasAnteriores(string p_strItemCode, int p_intLineNum, int p_intDocEntry, SAPbobsCOM.ICompany p_CompanySBO)
        {
            SAPbobsCOM.Documents m_objCotizacion;
            string m_strEstadoAprobacion = string.Empty;
            string m_strEstadoTraslado = string.Empty;
            int m_intEstadoAprobacion;
            int m_intEstadoTraslado;
            double m_dblCantAnterior = 0;

            try
            {
                m_objCotizacion = (Documents)p_CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                if (m_objCotizacion.GetByKey(p_intDocEntry))
                {
                    for (int x = 0; x <= m_objCotizacion.Lines.Count - 1; x++)
                    {
                        m_objCotizacion.Lines.SetCurrentLine(x);
                        if (m_objCotizacion.Lines.LineNum < p_intLineNum && m_objCotizacion.Lines.ItemCode == p_strItemCode)
                        {
                            m_strEstadoAprobacion = m_objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim();
                            int.TryParse(m_strEstadoAprobacion, out m_intEstadoAprobacion);

                            m_strEstadoTraslado = m_objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();
                            int.TryParse(m_strEstadoTraslado, out m_intEstadoTraslado);

                            if ((m_intEstadoAprobacion == (int)EstadosAprobacion.Aprobado) &&
                                (m_intEstadoTraslado == (int)EstadosTraslado.NoProcesado ||
                                m_intEstadoTraslado == (int)EstadosTraslado.PendienteTraslado))
                            {
                                m_dblCantAnterior += m_objCotizacion.Lines.Quantity;
                            }
                        }
                    }
                }

                return m_dblCantAnterior;
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_dblCantAnterior;
        }

        public double DevuelveStockDisponibleXItem(string p_ItemCode, string p_StrBodegaRepuestos, SAPbobsCOM.ICompany p_oCompany)
        {
            SAPbobsCOM.IItems m_objItem;
            SAPbobsCOM.ItemWarehouseInfo m_objWareHouseInfo;
            double m_dblStock = 0;

            try
            {
                m_objItem = (IItems)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                m_objItem.GetByKey(p_ItemCode);

                m_objWareHouseInfo = m_objItem.WhsInfo;

                for (int x = 0; x <= m_objWareHouseInfo.Count - 1; x++)
                {
                    m_objWareHouseInfo.SetCurrentLine(x);
                    if (m_objWareHouseInfo.WarehouseCode == p_StrBodegaRepuestos)
                    {
                        m_dblStock = m_objWareHouseInfo.InStock - m_objWareHouseInfo.Committed;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_dblStock;
        }

        private bool ValidaConfiguracionArticulo(string p_strItemCode, BoYesNoEnum p_Inventariable, BoYesNoEnum p_deVenta, BoYesNoEnum p_deCompra,
            bool p_blnTomaEnCuentaVenta, string p_strSucursal, ref string p_strCentroCosto, bool p_blnValidaCentroCosto)
        {
            SAPbouiCOM.DataTable m_dtConsultas;
            SAPbouiCOM.DataTable m_dtBodXCC;
            SAPbobsCOM.Items m_oItem;
            string m_strBodegaProcesoCtroCosto = string.Empty;
            string m_strBodegaProcesoItem = string.Empty;

            try
            {
                m_dtBodXCC = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                m_dtConsultas = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);

                m_oItem = (Items)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                m_oItem.GetByKey(p_strItemCode);

                if (m_oItem.InventoryItem != p_Inventariable)
                {
                    return false;
                }
                if (m_oItem.PurchaseItem != p_deCompra && p_blnTomaEnCuentaVenta)
                {
                    return false;
                }
                if (m_oItem.SalesItem != p_deVenta)
                {
                    return false;
                }

                if (p_blnValidaCentroCosto)
                {
                    p_strCentroCosto = m_oItem.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString().Trim();

                    if (Utilitarios.IsNumeric(p_strCentroCosto))
                    {

                        for (int i = 0; i <= m_dtBodXCC.Rows.Count - 1; i++)
                        {
                            if (m_dtBodXCC.GetValue("Sucursal", i).ToString().Trim() == p_strSucursal &&
                                m_dtBodXCC.GetValue("CentroCosto", i).ToString().Trim() == p_strCentroCosto)
                            {
                                m_strBodegaProcesoCtroCosto = m_dtBodXCC.GetValue("Proceso", i).ToString().Trim();
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(m_strBodegaProcesoCtroCosto))
                        {
                            return false;
                        }

                        m_dtConsultas.Clear();
                        m_dtConsultas.ExecuteQuery(String.Format("SELECT WhsCode FROM OITW WHERE ItemCode = '{0}'AND WhsCode = '{1}'", p_strItemCode, m_strBodegaProcesoCtroCosto));

                        m_strBodegaProcesoItem = m_dtConsultas.GetValue(0, 0).ToString().Trim();

                        if (string.IsNullOrEmpty(m_strBodegaProcesoItem))
                        {
                            return false;
                        }

                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return true;
        }

        private void ValidaInicioActividad(ItemEvent pval, ref bool bubbleEvent)
        {
            string mecanico;
            try
            {
                Matrix m_objMatrix;

                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                string ParaMetro = g_dtConfSucursal.GetValue("U_SolaUna", 0).ToString().Trim();
                int Index = 0;

                for (int i = 1; i <= m_objMatrix.RowCount; i++)
                {
                    if (m_objMatrix.IsRowSelected(i))
                    {
                        switch (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).ToString().Trim())
                        {
                            case g_strEstado_Finalizado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarFinalizada,
                                                                 BoMessageTime.bmt_Short,
                                                                 BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            case g_strEstado_Iniciado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciada,
                                                                 BoMessageTime.bmt_Short,
                                                                 BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            default:
                                if (ParaMetro.ToUpper() == "Y")
                                {
                                    mecanico = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", i - 1).Trim();
                                    Index += ObtieneOcupacionMecanico(mecanico.Trim());
                                    if (Index != 0)
                                    {
                                        ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciadaControl, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                        bubbleEvent = false;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaSuspensionActividad(ItemEvent pval, ref bool bubbleEvent)
        {
            try
            {
                Matrix m_objMatrix;

                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                for (int i = 1; i <= m_objMatrix.RowCount; i++)
                {
                    if (m_objMatrix.IsRowSelected(i))
                    {
                        switch (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim())
                        {
                            case g_strEstado_NoIniciado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorSuspenderNoIniciada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            case g_strEstado_Finalizado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorSuspenderFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            case g_strEstado_Suspendido:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorSuspenderSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaFinalizacionActividad(ItemEvent pval, ref bool bubbleEvent)
        {
            Matrix m_objMatrix;
            string strfechaInicio, strHoraInicio;
            DateTime dtFechaInicio;
            try
            {
                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                for (int i = 1; i <= m_objMatrix.RowCount; i++)
                {
                    if (m_objMatrix.IsRowSelected(i))
                    {
                        switch (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim())
                        {
                            case g_strEstado_Suspendido:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorFinalizarSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            case g_strEstado_NoIniciado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorFinalizarNoIniciada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            case g_strEstado_Finalizado:
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorFinalizarFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;

                        }
                        if (bubbleEvent)
                        {
                            strfechaInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_DFIni", i - 1).Trim();
                            strHoraInicio = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_HFIni", i - 1).Trim();
                            if (strHoraInicio.Length == 3) strHoraInicio = string.Format("0{0}", strHoraInicio);
                            dtFechaInicio = new DateTime(Convert.ToInt32(strfechaInicio.Substring(0, 4)), Convert.ToInt32(strfechaInicio.Substring(4, 2).ToString()), Convert.ToInt32(strfechaInicio.Substring(6, 2).ToString()), Convert.ToInt32(strHoraInicio.Substring(0, 2)), Convert.ToInt32(strHoraInicio.Substring(2, 2)), 00);
                            if (dtFechaInicio > DateTime.Now)
                            {
                                ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinMenor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void IniciarActividad(ItemEvent pval)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            int intError;
            string strError;

            try
            {
                Matrix m_objMatrix;
                string m_strIdActividad = string.Empty;
                string m_strIdCotizacion = string.Empty;
                string m_str_Estado = string.Empty;
                string m_str_NoFase = string.Empty;
                string m_str_CodFase = string.Empty;
                string m_str_Colaborador = string.Empty;
                string m_strNoOT = string.Empty;
                double m_dbl_CostoEstandar = 0.0;
                bool m_blnActividadIniciada = false;
                bool blnActividadYaIniciada = false;

                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                m_strIdCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();

                g_blnIniciarActividad = true;

                m_objMatrix.FlushToDataSource();
                SAPbobsCOM.Documents m_objCotizacion = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);

                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strNoOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                if (m_objCotizacion.GetByKey(Convert.ToInt32(m_strIdCotizacion)))
                {
                    for (int i = 1; i <= m_objMatrix.RowCount; i++)
                    {
                        if (m_objMatrix.IsRowSelected(i) && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim() == g_strEstado_NoIniciado)
                        {
                            oChildCC = oChildrenCtrlCol.Item(i - 1);
                            oChildCC.SetProperty("U_DFIni", DateTime.Now);
                            oChildCC.SetProperty("U_HFIni", DateTime.Now);
                            oChildCC.SetProperty("U_Estad", g_strEstado_Iniciado);
                            m_strIdActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                            m_str_CodFase = oChildCC.GetProperty("U_CodFas").ToString().Trim();
                            ActualizarActividadCotizacion(ref m_objCotizacion, m_strIdActividad, g_strEstado_Iniciado, m_str_CodFase);
                            m_blnActividadIniciada = true;
                        }
                        else if (m_objMatrix.IsRowSelected(i) && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim() == g_strEstado_Suspendido)
                        {
                            blnActividadYaIniciada = false;
                            for (int index = i + 1; index <= m_objMatrix.RowCount; index++)
                            {
                                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", i - 1).Trim() == FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index - 1).Trim() &&
                                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", i - 1).Trim() == FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index - 1).Trim() &&
                                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", index - 1).Trim() != g_strEstado_Suspendido)
                                {
                                    blnActividadYaIniciada = true;
                                    break;
                                }
                            }
                            if (blnActividadYaIniciada)
                            {
                                ApplicationSBO.StatusBar.SetText(Resource.ActividadYaIniciada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                continue;
                            }
                            oChildCC = oChildrenCtrlCol.Item(i - 1);
                            m_strIdActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                            m_str_Estado = g_strEstado_Iniciado;
                            m_str_NoFase = oChildCC.GetProperty("U_NoFas").ToString().Trim();
                            m_str_CodFase = oChildCC.GetProperty("U_CodFas").ToString().Trim();
                            m_dbl_CostoEstandar = double.Parse(oChildCC.GetProperty("U_CosEst").ToString());
                            m_str_Colaborador = oChildCC.GetProperty("U_Colab").ToString().Trim();
                            AgregaActividadControlColaborador(ref oChildrenCtrlCol, m_strIdActividad, m_str_Estado, m_str_NoFase, m_str_CodFase, m_dbl_CostoEstandar, m_str_Colaborador, true);
                            ActualizarActividadCotizacion(ref m_objCotizacion, m_strIdActividad, g_strEstado_Iniciado, m_str_CodFase);
                            m_blnActividadIniciada = true;
                        }
                    }
                    if (m_blnActividadIniciada)
                    {
                        ManejarEstadoOT(true, false, false, ref oGeneralData);

                        var estado = 2;
                        var descEstado = string.Empty;
                        ObtieneDescripcionEstado(estado.ToString(), ref descEstado, (SAPbouiCOM.Form)FormularioSBO);

                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = estado.ToString();
                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = descEstado;

                        CompanySBO.StartTransaction();
                        if (m_objCotizacion.Update() == 0)
                        {
                            oGeneralService.Update(oGeneralData);
                            if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            CompanySBO.GetLastError(out intError, out strError);
                            throw new Exception(string.Format("{0}: {1}", intError, strError));
                        }

                        recargarActividades(m_strNoOT, ApplicationSBO);
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                        ApplicationSBO.StatusBar.SetText(Resource.ActividadIniciada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, m_strIdCotizacion, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((int)Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim(), (SAPbouiCOM.Form)FormularioSBO, g_strdtConsulta, true, Utilitarios.RolesMensajeria.EncargadoProduccion, true, (SAPbouiCOM.Application)ApplicationSBO);
                    }
                }

            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }
            finally
            {

            }
        }

        public void ManejarEstadoOT(bool p_blnIniciar, bool p_blnSuspender, bool p_blnFinalizar, ref GeneralData oGeneralData)
        {
            string m_strEstadoActividad = string.Empty;
            int m_intEstadoActividad = 0;
            bool m_blnActividadIniciada = false;
            DataTable m_dtEstadosOT;
            int intCountSuspendidas;

            string m_strIniciada = string.Empty;
            string m_strSuspendida = string.Empty;
            string m_strFinalizada = string.Empty;
            string m_strCodeEstado = string.Empty;
            string m_strDescEstado = string.Empty;
            List<string> ltActividades;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;

            try
            {
                m_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item(g_strdtEstadosOT);

                for (int i = 0; i <= m_dtEstadosOT.Rows.Count - 1; i++)
                {
                    m_strCodeEstado = m_dtEstadosOT.GetValue("Code", i).ToString().Trim();
                    m_strDescEstado = m_dtEstadosOT.GetValue("Name", i).ToString().Trim();

                    switch (m_strCodeEstado)
                    {
                        case "2":
                            m_strIniciada = m_strDescEstado;
                            break;
                        case "3":
                            m_strSuspendida = m_strDescEstado;
                            break;
                        case "4":
                            m_strFinalizada = m_strDescEstado;
                            break;
                    }

                }

                if (p_blnIniciar)
                {
                    oGeneralData.SetProperty("U_DEstO", m_strIniciada);
                    oGeneralData.SetProperty("U_EstO", "2");
                }
                else
                    if (p_blnSuspender)
                    {
                        ltActividades = new List<string>();
                        intCountSuspendidas = 0;
                        oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");
                        for (int x = oChildrenCtrlCol.Count - 1; x >= 0; x--)
                        {
                            oChildCC = oChildrenCtrlCol.Item(x);
                            if (!ltActividades.Contains(string.Format("{0}{1}", oChildCC.GetProperty("U_IdAct").ToString().Trim(), oChildCC.GetProperty("U_Colab").ToString().Trim())))
                            {
                                ltActividades.Add(string.Format("{0}{1}", oChildCC.GetProperty("U_IdAct").ToString().Trim(), oChildCC.GetProperty("U_Colab").ToString().Trim()));
                                m_strEstadoActividad = oChildCC.GetProperty("U_Estad").ToString().Trim();
                                if (!string.IsNullOrEmpty(m_strEstadoActividad))
                                {
                                    m_intEstadoActividad = int.Parse(m_strEstadoActividad);
                                    switch (m_intEstadoActividad)
                                    {
                                        case (int)EstadoActividades.Iniciado:
                                            m_blnActividadIniciada = true;
                                            break;
                                        case (int)EstadoActividades.Suspendido:
                                        case (int)EstadoActividades.NoIniciado:
                                            intCountSuspendidas++;
                                            break;
                                    }
                                }
                            }
                        }
                        if (!m_blnActividadIniciada && intCountSuspendidas != 0)
                        {
                            oGeneralData.SetProperty("U_DEstO", m_strSuspendida);
                            oGeneralData.SetProperty("U_EstO", "3");
                        }
                    }
                    else
                        if (p_blnFinalizar)
                        {
                            oGeneralData.SetProperty("U_DEstO", m_strFinalizada);
                            oGeneralData.SetProperty("U_EstO", "4");
                        }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void SuspenderActividad(string p_strRazon, string p_strComentario, IApplication applicationSbo, ICompany companySbo, DateTime p_dtFechaFin, Boolean p_suspendeOT = false)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;

            Matrix m_objMatrix;
            DateTime m_dtFechaInicio;
            TimeSpan m_dtFechaDiferencia;
            DateTime m_dtHoraInicio;

            int intError;
            string strError;
            string m_strNoOT = string.Empty;
            string IDActividad = string.Empty;
            string DocEntryCotizacion = string.Empty;
            string strSuspensionHorario = "8";
            double m_dblCostoReal = 0;
            bool m_blnActividadSuspendida = false;
            bool m_blnSuspendeAct = true;
            SAPbouiCOM.Form oFormOT;
            double SalarioPorHora = 0;
            double TarifaHorasExtra = 0;
            int DuracionEstandar = 0;
            int empID;
            string Sucursal = string.Empty;
            double CostoEstandar = 0;
            double CantidadHorasEstandar = 0;
            double CantidadHorasExtra = 0;
            double TotalMinutos = 0;
            bool UsaCalculoSobreHorarioTaller = false;
            CalculoCostos.CostoManoObra.TrabajaFinSemana TrabajaFinSemana = CalculoCostos.CostoManoObra.TrabajaFinSemana.No;

            try
            {
                oFormOT = applicationSbo.Forms.Item("SCGD_ORDT");

                GuardaRazonSuspension(p_strRazon, p_strComentario, oFormOT);

                m_objMatrix = (Matrix)oFormOT.Items.Item("mtxColab").Specific;
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                DocEntryCotizacion = oFormOT.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();

                g_blnSuspenderActividad = true;

                m_objMatrix.FlushToDataSource();
                SAPbobsCOM.Documents m_objCotizacion = (Documents)companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)
                oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strNoOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                Sucursal = oGeneralData.GetProperty("U_Sucu").ToString();
                oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                if (m_objCotizacion.GetByKey(Convert.ToInt32(DocEntryCotizacion)))
                {
                    TarifaHorasExtra = CalculoCostos.CostoManoObra.ObtenerTarifaHorasExtra(Sucursal);
                    UsaCalculoSobreHorarioTaller = CalculoCostos.CostoManoObra.UsaCalculoSobreHorarioTaller(Sucursal);
                    for (int i = 1; i <= m_objMatrix.RowCount; i++)
                    {
                        TotalMinutos = 0;
                        if ((p_suspendeOT || m_objMatrix.IsRowSelected(i)) &&
                            oFormOT.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim() ==
                            g_strEstado_Iniciado)
                        {
                            oChildCC = oChildrenCtrlCol.Item(i - 1);
                            IDActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                            m_dtFechaInicio = DateTime.Parse(oChildCC.GetProperty("U_DFIni").ToString());
                            m_dtHoraInicio = DateTime.Parse(oChildCC.GetProperty("U_HFIni").ToString());
                            m_dtFechaInicio = new DateTime(m_dtFechaInicio.Year, m_dtFechaInicio.Month, m_dtFechaInicio.Day, m_dtHoraInicio.Hour, m_dtHoraInicio.Minute, m_dtHoraInicio.Second);
                            m_dtFechaDiferencia = p_dtFechaFin - m_dtFechaInicio;
                            empID = Convert.ToInt32(oChildCC.GetProperty("U_Colab").ToString().Trim());
                            SalarioPorHora = CalculoCostos.CostoManoObra.ObtenerSalarioPorHora(empID, ref TrabajaFinSemana);
                            DuracionEstandar = CalculoCostos.CostoManoObra.ObtenerDuracionEstandar(DocEntryCotizacion, IDActividad);

                            if (UsaCalculoSobreHorarioTaller)
                            {
                                CalculoCostos.CostoManoObra.CalcularCostoCompuesto(Sucursal, m_dtFechaInicio, p_dtFechaFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref m_dblCostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                                TotalMinutos = (CantidadHorasEstandar + CantidadHorasExtra) * 60.0;
                            }
                            else
                            {
                                m_dblCostoReal = ObtieneCostosReal(oChildCC.GetProperty("U_Colab").ToString().Trim(), (double)m_dtFechaDiferencia.TotalMinutes, oFormOT);
                                TotalMinutos = m_dtFechaDiferencia.TotalMinutes;
                            }
                            oChildCC.SetProperty("U_TMin", TotalMinutos);
                            oChildCC.SetProperty("U_DFFin", p_dtFechaFin);
                            oChildCC.SetProperty("U_HFFin", p_dtFechaFin);

                            oChildCC.SetProperty("U_Estad", g_strEstado_Suspendido);
                            oChildCC.SetProperty("U_CosRe", m_dblCostoReal);

                            //Si la actividad es suspendida por horario se debe marcar como Y el campo U_SuspensionHorario
                            //ya que esta información se utiliza al graficar la agenda
                            if (p_strRazon == strSuspensionHorario)
                            {
                                oChildCC.SetProperty("U_SuspensionHorario", "Y");
                            }
                            else
                            {
                                oChildCC.SetProperty("U_SuspensionHorario", "N");
                            }

                            ActualizarActividadCotizacion(ref m_objCotizacion, IDActividad, g_strEstado_Suspendido, string.Empty, 0, TotalMinutos);
                            m_blnActividadSuspendida = true;
                        }
                    }

                    if (p_suspendeOT && !m_blnActividadSuspendida)
                    {
                        m_blnActividadSuspendida = true;
                        m_blnSuspendeAct = false;
                    }

                    if (m_blnActividadSuspendida)
                    {
                        ManejarEstadoOT(false, true, false, ref oGeneralData);

                        var estado = ValidaEstadoOT(ref oGeneralData);
                        var descEstado = string.Empty;
                        ObtieneDescripcionEstado(estado.ToString(), ref descEstado, oFormOT);

                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = estado.ToString();
                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = descEstado;

                        if (!companySbo.InTransaction)
                            companySbo.StartTransaction();

                        if (m_objCotizacion.Update() == 0)
                        {
                            oGeneralService.Update(oGeneralData);
                            if (CompanySBO.InTransaction)
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            if (CompanySBO.InTransaction)
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);

                            CompanySBO.GetLastError(out intError, out strError);
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intError, strError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }

                        recargarActividades(m_strNoOT, ApplicationSBO);

                        if (!p_suspendeOT)
                            applicationSbo.StatusBar.SetText(Resource.ActividadSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        else
                            applicationSbo.StatusBar.SetText(Resource.OTSuspendida, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }
            finally
            {
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
            }
        }

        private void GuardaRazonSuspension(string RazonSuspension, string Comentarios, SAPbouiCOM.Form Formulario)
        {
            string ConsecutivoTexto = string.Empty;
            int Consecutivo;
            SAPbouiCOM.DataTable Consulta;
            SAPbobsCOM.UserTable TablaUsuario;

            try
            {
                TablaUsuario = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_SUSPENXACT");
                Consulta = Formulario.DataSources.DataTables.Item(g_strdtConsulta);
                Consulta.ExecuteQuery(" select top(1) code from [@SCGD_SUSPENXACT] order by Convert(int,code) desc ");
                ConsecutivoTexto = Consulta.GetValue(0, 0).ToString();

                if (!string.IsNullOrEmpty(ConsecutivoTexto))
                {
                    Consecutivo = int.Parse(ConsecutivoTexto);
                    Consecutivo += 1;
                }
                else
                {
                    Consecutivo = 1;
                }

                TablaUsuario.Code = Consecutivo.ToString();
                TablaUsuario.Name = Consecutivo.ToString();
                TablaUsuario.UserFields.Fields.Item("U_IDAct").Value = IdActividad;
                TablaUsuario.UserFields.Fields.Item("U_IDRaz").Value = RazonSuspension;
                TablaUsuario.UserFields.Fields.Item("U_Fecha").Value = DateTime.Now;
                TablaUsuario.UserFields.Fields.Item("U_Coment").Value = Comentarios;
                TablaUsuario.Add();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        private void FinalizarLineaServicio(SAPbouiCOM.ItemEvent pval, string FormUID, ref Boolean BubbleEvent)
        {
            string DocEntryCotizacion = string.Empty;
            string NumeroOT = string.Empty;
            SAPbouiCOM.Matrix oMatrix;
            SAPbobsCOM.Documents Cotizacion;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            int FilaSeleccionada;
            string EstadoLinea = string.Empty;
            double CostoLineaOfertaVentas;
            double CostoReal = 0;
            double CostoEstandar = 0;
            DateTime FechaInicio;
            DateTime HoraInicio;
            DateTime FechaFin;
            TimeSpan DiferenciaTiempo;
            string IDActividad = string.Empty;
            MetodoCosteo MetodoCosteoServicio = MetodoCosteo.SinConfigurar;
            int empID;
            int CodigoError;
            string MensajeError = string.Empty;
            string Sucursal = string.Empty;
            double SalarioPorHora = 0;
            double TarifaHorasExtra = 0;
            int DuracionEstandar = 0;
            double CantidadHorasEstandar = 0;
            double CantidadHorasExtra = 0;
            bool UsaCalculoSobreHorarioTaller = false;
            double TotalMinutos = 0;
            CalculoCostos.CostoManoObra.TrabajaFinSemana TrabajaFinSemana = CalculoCostos.CostoManoObra.TrabajaFinSemana.No;
            try
            {
                Sucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                MetodoCosteoServicio = ObtenerMetodoCosteo(Sucursal);
                if (MetodoCosteoServicio == MetodoCosteo.SinConfigurar)
                {
                    //Mensaje de error, el método de costeo (Tiempo estándar o tiempo real no esta configurado correctamente)
                    BubbleEvent = false;
                    if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                    DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(Resource.MetodoCosteoSinConfigurar, BoMessageTime.bmt_Short, true);
                }
                else
                {
                    DocEntryCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                    NumeroOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                    oMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                    oMatrix.FlushToDataSource();

                    Cotizacion = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                    oCompanyService = CompanySBO.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", NumeroOT);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                    if (Cotizacion.GetByKey(Convert.ToInt32(DocEntryCotizacion)))
                    {
                        UsaCalculoSobreHorarioTaller = CalculoCostos.CostoManoObra.UsaCalculoSobreHorarioTaller(Sucursal);
                        FilaSeleccionada = oMatrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder);
                        EstadoLinea = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", FilaSeleccionada - 1).Trim();

                        if (FilaSeleccionada != -1 && EstadoLinea == g_strEstado_Iniciado)
                        {
                            oChildCC = oChildrenCtrlCol.Item(FilaSeleccionada - 1);
                            IDActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                            FechaInicio = DateTime.Parse(oChildCC.GetProperty("U_DFIni").ToString());
                            HoraInicio = DateTime.Parse(oChildCC.GetProperty("U_HFIni").ToString());
                            FechaInicio = new DateTime(FechaInicio.Year, FechaInicio.Month, FechaInicio.Day, HoraInicio.Hour, HoraInicio.Minute, HoraInicio.Second);
                            FechaFin = DateTime.Now;
                            empID = Convert.ToInt32(oChildCC.GetProperty("U_Colab").ToString().Trim());
                            SalarioPorHora = CalculoCostos.CostoManoObra.ObtenerSalarioPorHora(empID, ref TrabajaFinSemana);
                            TarifaHorasExtra = CalculoCostos.CostoManoObra.ObtenerTarifaHorasExtra(Sucursal);
                            DuracionEstandar = CalculoCostos.CostoManoObra.ObtenerDuracionEstandar(DocEntryCotizacion, IDActividad);
                            CostoEstandar = Convert.ToDouble(oChildCC.GetProperty("U_CosEst"));

                            if (FechaInicio <= FechaFin)
                            {
                                //Calcula la diferencia de tiempo
                                DiferenciaTiempo = FechaFin - FechaInicio;
                            }
                            else
                            {
                                DiferenciaTiempo = TimeSpan.Zero;
                            }

                            if (UsaCalculoSobreHorarioTaller)
                            {
                                CalculoCostos.CostoManoObra.CalcularCostoCompuesto(Sucursal, FechaInicio, FechaFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref CostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                                TotalMinutos = (CantidadHorasEstandar + CantidadHorasExtra) * 60.0;
                            }
                            else
                            {
                                CalcularCostoLinea(empID, IDActividad, ref CostoEstandar, ref CostoReal, DiferenciaTiempo.TotalMinutes);
                                TotalMinutos = DiferenciaTiempo.TotalMinutes;
                            }

                            oChildCC.SetProperty("U_TMin", TotalMinutos);
                            oChildCC.SetProperty("U_DFFin", FechaFin);
                            oChildCC.SetProperty("U_HFFin", FechaFin);

                            oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);
                            oChildCC.SetProperty("U_CosRe", (double)CostoReal);

                            if (MetodoCosteoServicio == MetodoCosteo.TiempoEstandar)
                            {
                                CostoLineaOfertaVentas = ObtenerSumatoriaCostoEstandar(oMatrix, IDActividad);
                            }
                            else
                            {
                                //Se suman todas las líneas del mismo ID
                                CostoLineaOfertaVentas = CostoReal + ObtenerSumatoriaCostoReal(oMatrix, IDActividad);
                            }

                            ActualizarActividadCotizacion(ref Cotizacion, IDActividad, g_strEstado_Finalizado, string.Empty, CostoLineaOfertaVentas, TotalMinutos);

                            CompanySBO.StartTransaction();
                            if (Cotizacion.Update() == 0)
                            {
                                oGeneralService.Update(oGeneralData);
                                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                            }
                            else
                            {
                                DMS_Connector.Company.CompanySBO.GetLastError(out CodigoError, out MensajeError);
                                throw new Exception(string.Format("{0}: {1}", CodigoError, MensajeError));
                            }

                            recargarActividades(NumeroOT, ApplicationSBO);
                            FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                            ApplicationSBO.StatusBar.SetText(Resource.ActividadFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                            Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, DocEntryCotizacion, (SAPbobsCOM.Company)CompanySBO, NumeroOT, false, Sucursal, true, GeneralEnums.RolesMensajeria.EncargadoProduccion, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        private double ObtenerSumatoriaCostoReal(SAPbouiCOM.Matrix oMatrix, string IDActividad)
        {
            double Sumatoria = 0;
            string Valor = string.Empty;
            try
            {
                for (int i = 1; i <= oMatrix.RowCount; i++)
                {
                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", i - 1).Trim() == IDActividad)
                    {
                        Valor = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosRe", i - 1);
                        Sumatoria += Convert.ToDouble(Valor, n);
                    }
                }
                return Sumatoria;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw;
            }
        }

        private void FinalizarActividad(ItemEvent pval)
        {

            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildCC;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;

            Matrix m_objMatrix;
            DateTime m_dtFechaInicio;
            DateTime m_dtHoraInicio;
            TimeSpan m_dtFechaDiferencia;
            TimeSpan m_HoraDelDia;
            string m_strIdActividad = string.Empty;
            string m_strIdCotizacion = string.Empty;
            string m_strNoOT = string.Empty;
            string strError;
            int intError;
            double m_dblCostoReal = 0;
            bool m_blnActividadFinalizada = false;
            DataTable m_dtConsulta;

            string strCostoReal, strCostoEstandar = string.Empty;
            double dblTotalCostoReal = 0;
            double dblTotalCostoEstandar = 0;
            double dblTotalCosto = 0;

            try
            {

                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                strCostoReal = m_dtConsulta.GetValue("U_TiempoReal_C", 0).ToString();
                strCostoEstandar = m_dtConsulta.GetValue("U_TiempoEst_C", 0).ToString();

                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                m_strIdCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();

                g_blnFinalizarActividad = true;

                m_objMatrix.FlushToDataSource();
                SAPbobsCOM.Documents m_objCotizacion = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strNoOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                if (m_objCotizacion.GetByKey(Convert.ToInt32(m_strIdCotizacion)))
                {
                    for (int i = 1; i <= m_objMatrix.RowCount; i++)
                    {
                        if (m_objMatrix.IsRowSelected(i) && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).Trim() == g_strEstado_Iniciado)
                        {
                            oChildCC = oChildrenCtrlCol.Item(i - 1);

                            m_dtFechaInicio = Convert.ToDateTime(oChildCC.GetProperty("U_DFIni"));
                            m_dtHoraInicio = Convert.ToDateTime(oChildCC.GetProperty("U_HFIni"));

                            m_dtFechaInicio = FormatearFecha(m_dtFechaInicio, m_dtHoraInicio);

                            if (m_dtFechaInicio <= DateTime.Now)
                                m_dtFechaDiferencia = DateTime.Now - m_dtFechaInicio;
                            else
                                m_dtFechaDiferencia = TimeSpan.Zero;

                            m_dblCostoReal = ObtieneCostosReal(oChildCC.GetProperty("U_Colab").ToString().Trim(), (double)m_dtFechaDiferencia.TotalMinutes);

                            oChildCC.SetProperty("U_DFFin", DateTime.Now);
                            oChildCC.SetProperty("U_HFFin", DateTime.Now);
                            oChildCC.SetProperty("U_TMin", m_dtFechaDiferencia.TotalMinutes);
                            oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);
                            oChildCC.SetProperty("U_CosRe", m_dblCostoReal);
                            m_strIdActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();

                            dblTotalCosto = ObtieneCostosPorID(strCostoReal, strCostoEstandar, ref dblTotalCostoReal, ref dblTotalCostoEstandar, m_strIdActividad, m_dblCostoReal);
                            ActualizarActividadCotizacion(ref m_objCotizacion, m_strIdActividad, g_strEstado_Finalizado, string.Empty, dblTotalCosto, m_dtFechaDiferencia.TotalMinutes);
                            m_blnActividadFinalizada = true;
                        }
                    }
                    if (m_blnActividadFinalizada)
                    {
                        m_objCotizacion.Update();

                        CompanySBO.StartTransaction();
                        if (m_objCotizacion.Update() == 0)
                        {
                            oGeneralService.Update(oGeneralData);
                            if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            CompanySBO.GetLastError(out intError, out strError);
                            throw new Exception(string.Format("{0}: {1}", intError, strError));
                        }

                        recargarActividades(m_strNoOT, ApplicationSBO);
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                        ApplicationSBO.StatusBar.SetText(Resource.ActividadFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, m_strIdCotizacion, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((int)Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim(), (SAPbouiCOM.Form)FormularioSBO, g_strdtConsulta, true, Utilitarios.RolesMensajeria.EncargadoProduccion, true, (SAPbouiCOM.Application)ApplicationSBO);

                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }
        }

        /// <summary>
        /// Combina la fecha y hora de dos objetos DateTime
        /// </summary>
        /// <param name="p_dtFecha">Objeto DateTime que representa la fecha sin hora</param>
        /// <param name="p_dtHora">Objeto DateTime que representa la hora del día</param>
        /// <returns>DateTime con la nueva fecha y hora</returns>
        private DateTime FormatearFecha(DateTime p_dtFecha, DateTime p_dtHora)
        {
            DateTime dtFechaFormateada;
            int cantidadHoras = 0;

            try
            {
                //Crea un nuevo objeto a partir de la fecha y hora
                dtFechaFormateada = new DateTime(p_dtFecha.Year, p_dtFecha.Month, p_dtFecha.Day, p_dtHora.Hour, p_dtHora.Minute, p_dtHora.Second);
                p_dtFecha = dtFechaFormateada;
                return p_dtFecha;

            }
            catch (Exception ex)
            {
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
                return p_dtFecha;
            }
        }

        private double ObtieneCostosPorID(string p_strCostoReal, string p_strCostoEstandar, ref double p_dblTotalCostoReal, ref double p_dblTotalCostoEstandar, string p_strIdActividad, double p_dblCostoReal)
        {
            Matrix m_objMatrix;
            double m_dblCosto = 0;
            double m_dblCostoRetorno = 0;
            string m_strCosto = string.Empty;
            bool m_blnCostoReal = false;

            try
            {
                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                if (string.IsNullOrEmpty(p_strCostoEstandar) == false && string.IsNullOrEmpty(p_strCostoReal) == false)
                {
                    if (p_strCostoEstandar == "Y" && p_strCostoReal == "N")
                    {
                        m_blnCostoReal = false;
                    }
                    else if (p_strCostoEstandar == "N" && p_strCostoReal == "Y")
                    {
                        m_blnCostoReal = true;
                    }

                    for (int i = 1; i <= m_objMatrix.RowCount; i++)
                    {
                        if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", i - 1).Trim() == p_strIdActividad)
                        {
                            m_strCosto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosRe", i - 1);
                            m_dblCosto = double.Parse(m_strCosto, n);
                            p_dblTotalCostoReal += m_dblCosto;

                            //No se hace sumatoria porque el costo estandar es unico, independientemente si se suspende la actividad 100 veces
                            m_strCosto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosEst", i - 1);
                            m_dblCosto = double.Parse(m_strCosto, n);
                            p_dblTotalCostoEstandar = m_dblCosto;
                        }
                    }

                    if (m_blnCostoReal)
                    {
                        //La sumatoria de los costos reales + el costo de la actividad que se esta finalizando ya que la misma aun no esta en la tabla
                        p_dblTotalCostoReal += p_dblCostoReal;
                        m_dblCostoRetorno = p_dblTotalCostoReal;
                    }
                    else
                    {
                        m_dblCostoRetorno = p_dblTotalCostoEstandar;
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

            return m_dblCostoRetorno;
        }

        private void FinalizarTodasActividades(List<string> p_lsIds, ref SAPbobsCOM.Documents p_objCotizacion, ref SAPbobsCOM.GeneralDataCollection p_ChildrenCtrCol)
        {
            Matrix m_objMatrix;
            DateTime m_dtFechaInicio;
            DateTime m_dtHoraInicio;
            TimeSpan m_dtFechaDiferencia;
            TimeSpan m_tsHoraDelDia;
            SAPbobsCOM.GeneralData oChildCC;
            DataTable m_dtConsulta;
            List<string> ltActividades;
            string m_strIdActividad = string.Empty;
            string m_strIdCotizacion = string.Empty;
            double m_dblCostoReal = 0;
            string strCostoReal, strCostoEstandar = string.Empty;
            double dblTotalCostoReal = 0;
            double dblTotalCostoEstandar = 0;
            double dblTotalCosto = 0;
            string strCodeEmp;
            string estadoAct;
            string m_strNoOT;
            string NoFase, CodFase;
            double dblCostoEstandar;

            try
            {
                ltActividades = new List<string>();
                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                strCostoReal = m_dtConsulta.GetValue("U_TiempoReal_C", 0).ToString();
                strCostoEstandar = m_dtConsulta.GetValue("U_TiempoEst_C", 0).ToString();
                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;

                m_strIdCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                g_blnFinalizarActividad = true;
                m_objMatrix.FlushToDataSource();

                if (p_objCotizacion.GetByKey(Convert.ToInt32(m_strIdCotizacion)))
                {
                    for (int index = p_ChildrenCtrCol.Count - 1; index >= 0; index--)
                    {
                        oChildCC = p_ChildrenCtrCol.Item(index);
                        m_strIdActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                        estadoAct = oChildCC.GetProperty("U_Estad").ToString().Trim();
                        strCodeEmp = oChildCC.GetProperty("U_Colab").ToString().Trim();
                        if (!ltActividades.Contains(string.Format("{0}{1}", m_strIdActividad, strCodeEmp)))
                        {
                            switch (estadoAct)
                            {
                                case g_strEstado_NoIniciado:
                                case g_strEstado_Iniciado:

                                    //m_dtFechaInicio = DateTime.Parse(oChildCC.GetProperty("U_DFIni").ToString());
                                    //m_dtHoraInicio = DateTime.Parse(oChildCC.GetProperty("U_HFIni").ToString());
                                    //m_tsHoraDelDia = m_dtHoraInicio.TimeOfDay;

                                    m_dtFechaInicio = Convert.ToDateTime(oChildCC.GetProperty("U_DFIni"));
                                    m_dtHoraInicio = Convert.ToDateTime(oChildCC.GetProperty("U_HFIni"));

                                    //Correción para problemas de cálculo de horas cuando la fecha es nula
                                    //y se utiliza el formato de 12 horas en la configuración regional de Windows
                                    if (EsFechaMinimaCOM(m_dtFechaInicio))
                                    {
                                        //Fecha minima del objeto COM
                                        m_dtFechaInicio = DateTime.Now;
                                        m_dtHoraInicio = DateTime.Now;
                                        //Si no hay fecha de inicio o es nula, se toma la fecha DateTime.Now
                                        oChildCC.SetProperty("U_DFIni", DateTime.Now);
                                        oChildCC.SetProperty("U_HFIni", DateTime.Now);
                                    }
                                    else
                                    {
                                        //En la tabla Control Colaborador [@SCGD_CTRLCOL] las columnas
                                        //fecha y hora se guardan en forma independiente por lo que es necesario unirlas
                                        //en un solo objeto DateTime para procesarlo
                                        m_dtFechaInicio = FormatearFecha(m_dtFechaInicio, m_dtHoraInicio);
                                    }

                                    if (m_dtFechaInicio != new DateTime(1899, 12, 30, 0, 0, 0))
                                    {
                                        if (m_dtFechaInicio <= DateTime.Now)
                                            m_dtFechaDiferencia = DateTime.Now - m_dtFechaInicio;
                                        else
                                            m_dtFechaDiferencia = TimeSpan.Zero;
                                    }
                                    else
                                    {
                                        m_dtFechaDiferencia = TimeSpan.Zero;
                                        oChildCC.SetProperty("U_DFIni", DateTime.Now);
                                        oChildCC.SetProperty("U_HFIni", DateTime.Now);
                                    }

                                    m_dblCostoReal = ObtieneCostosReal(oChildCC.GetProperty("U_Colab").ToString().Trim(), (double)m_dtFechaDiferencia.TotalMinutes);
                                    oChildCC.SetProperty("U_DFFin", DateTime.Now);
                                    oChildCC.SetProperty("U_HFFin", DateTime.Now);
                                    oChildCC.SetProperty("U_TMin", m_dtFechaDiferencia.TotalMinutes);
                                    oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);
                                    oChildCC.SetProperty("U_CosRe", m_dblCostoReal);

                                    dblTotalCosto = ObtieneCostosPorID(strCostoReal, strCostoEstandar, ref dblTotalCostoReal, ref dblTotalCostoEstandar, m_strIdActividad, m_dblCostoReal);
                                    ActualizarActividadCotizacion(ref p_objCotizacion, m_strIdActividad, g_strEstado_Finalizado, string.Empty, dblTotalCosto, m_dtFechaDiferencia.TotalMinutes);
                                    ltActividades.Add(string.Format("{0}{1}", m_strIdActividad, strCodeEmp));
                                    break;

                                case g_strEstado_Suspendido:

                                    NoFase = oChildCC.GetProperty("U_NoFas").ToString();
                                    CodFase = oChildCC.GetProperty("U_CodFas").ToString();
                                    dblCostoEstandar = (double)oChildCC.GetProperty("U_CosEst");
                                    oChildCC = p_ChildrenCtrCol.Add();
                                    oChildCC.SetProperty("U_DFIni", DateTime.Now);
                                    oChildCC.SetProperty("U_HFIni", DateTime.Now);
                                    oChildCC.SetProperty("U_DFFin", DateTime.Now);
                                    oChildCC.SetProperty("U_HFFin", DateTime.Now);
                                    oChildCC.SetProperty("U_TMin", TimeSpan.Zero.TotalMinutes);
                                    oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);
                                    oChildCC.SetProperty("U_CosRe", m_dblCostoReal);
                                    oChildCC.SetProperty("U_CosEst", dblCostoEstandar);
                                    oChildCC.SetProperty("U_Colab", strCodeEmp);
                                    oChildCC.SetProperty("U_IdAct", m_strIdActividad);
                                    oChildCC.SetProperty("U_NoFas", NoFase);
                                    oChildCC.SetProperty("U_CodFas", CodFase);
                                    oChildCC.SetProperty("U_FechPro", DateTime.Now);
                                    ActualizarActividadCotizacion(ref p_objCotizacion, m_strIdActividad, g_strEstado_Finalizado, string.Empty, dblTotalCosto, TimeSpan.Zero.TotalMinutes);

                                    ltActividades.Add(string.Format("{0}{1}", m_strIdActividad, strCodeEmp));

                                    break;
                                case g_strEstado_Finalizado:
                                    ltActividades.Add(string.Format("{0}{1}", m_strIdActividad, strCodeEmp));
                                    break;
                            }
                        }
                    }

                    for (int i = 0; i <= p_lsIds.Count - 1; i++)
                    {
                        ActualizarActividadCotizacion(ref p_objCotizacion, p_lsIds[i].Trim(), g_strEstado_Finalizado, string.Empty, dblTotalCosto, TimeSpan.Zero.TotalMinutes);
                    }
                }
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();

                Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, m_strIdCotizacion, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((int)Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim(), (SAPbouiCOM.Form)FormularioSBO, g_strdtConsulta, true, Utilitarios.RolesMensajeria.EncargadoProduccion, true, (SAPbouiCOM.Application)ApplicationSBO);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        enum MetodoCosteo
        {
            SinConfigurar,
            TiempoEstandar,
            TiempoReal
        }

        private void FinalizarTodosServicios(List<string> ListaActividadesSinAsignar, ref SAPbobsCOM.Documents Cotizacion, ref SAPbobsCOM.GeneralDataCollection LineasControlColaborador)
        {
            string DocEntryCotizacion = string.Empty;
            string NumeroOT = string.Empty;
            SAPbouiCOM.Matrix oMatrix;
            SAPbobsCOM.GeneralData oChildCC;
            string EstadoLinea = string.Empty;
            double CostoLineaOfertaVentas;
            double CostoReal = 0;
            double CostoEstandar = 0;
            DateTime FechaInicio;
            DateTime HoraInicio;
            DateTime FechaFin;
            TimeSpan DiferenciaTiempo;
            string IDActividad = string.Empty;
            MetodoCosteo MetodoCosteoServicio = MetodoCosteo.SinConfigurar;
            int empID;
            List<string> ListaActividadesFinalizadas;
            string Sucursal = string.Empty;
            string NumeroFase = string.Empty;
            string CodigoFase = string.Empty;
            double SalarioPorHora = 0;
            double TarifaHorasExtra = 0;
            int DuracionEstandar = 0;
            double CantidadHorasEstandar = 0;
            double CantidadHorasExtra = 0;
            double TotalMinutos = 0;
            bool UsaCalculoSobreHorarioTaller = false;
            CalculoCostos.CostoManoObra.TrabajaFinSemana TrabajaFinSemana = CalculoCostos.CostoManoObra.TrabajaFinSemana.No;
            try
            {
                ListaActividadesFinalizadas = new List<string>();
                Sucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                MetodoCosteoServicio = ObtenerMetodoCosteo(Sucursal);
                if (MetodoCosteoServicio == MetodoCosteo.SinConfigurar)
                {
                    //Mensaje de error, el método de costeo (Tiempo estándar o tiempo real no esta configurado correctamente)
                    if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                    throw new Exception(string.Format("{0}: {1}", "9124", Resource.MetodoCosteoSinConfigurar));
                }
                else
                {
                    DocEntryCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).ToString().Trim();
                    NumeroOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                    TarifaHorasExtra = CalculoCostos.CostoManoObra.ObtenerTarifaHorasExtra(Sucursal);
                    oMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                    oMatrix.FlushToDataSource();

                    Cotizacion = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);

                    if (Cotizacion.GetByKey(Convert.ToInt32(DocEntryCotizacion)))
                    {
                        UsaCalculoSobreHorarioTaller = CalculoCostos.CostoManoObra.UsaCalculoSobreHorarioTaller(Sucursal);
                        //Antes de finalizar las líneas de Control Colaborador, se deben finalizar todas las líneas sin asignar
                        FinalizarLineasSinAsignar(ListaActividadesSinAsignar, ref Cotizacion);

                        //Se recorren las líneas en sentido inverso para finalizar solamente la última línea
                        for (int i = LineasControlColaborador.Count - 1; i >= 0; i--)
                        {
                            oChildCC = LineasControlColaborador.Item(i);
                            EstadoLinea = oChildCC.GetProperty("U_Estad").ToString().Trim();
                            IDActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                            empID = Convert.ToInt32(oChildCC.GetProperty("U_Colab").ToString().Trim());
                            CostoReal = 0;
                            CostoEstandar = Convert.ToDouble(oChildCC.GetProperty("U_CosEst"));
                            NumeroFase = oChildCC.GetProperty("U_NoFas").ToString();
                            CodigoFase = oChildCC.GetProperty("U_CodFas").ToString();
                            FechaInicio = Convert.ToDateTime(oChildCC.GetProperty("U_DFIni"));
                            HoraInicio = Convert.ToDateTime(oChildCC.GetProperty("U_HFIni"));
                            FechaFin = DateTime.Now;
                            SalarioPorHora = CalculoCostos.CostoManoObra.ObtenerSalarioPorHora(empID, ref TrabajaFinSemana);
                            DuracionEstandar = CalculoCostos.CostoManoObra.ObtenerDuracionEstandar(DocEntryCotizacion, IDActividad);
                            TotalMinutos = 0;

                            if (!ListaActividadesFinalizadas.Contains(string.Format("{0}{1}", IDActividad, empID)))
                            {
                                switch (EstadoLinea)
                                {
                                    case g_strEstado_NoIniciado:
                                    case g_strEstado_Iniciado:
                                        //Correción para problemas de cálculo de horas cuando la fecha es nula
                                        //y se utiliza el formato de 12 horas en la configuración regional de Windows
                                        if (EsFechaMinimaCOM(FechaInicio))
                                        {
                                            //Fecha minima del objeto COM
                                            FechaInicio = FechaFin;
                                            HoraInicio = FechaFin;
                                            //Si no hay fecha de inicio o es nula, se toma la fecha DateTime.Now
                                            oChildCC.SetProperty("U_DFIni", FechaInicio);
                                            oChildCC.SetProperty("U_HFIni", HoraInicio);
                                        }
                                        else
                                        {
                                            //En la tabla Control Colaborador [@SCGD_CTRLCOL] las columnas
                                            //fecha y hora se guardan en forma independiente por lo que es necesario unirlas
                                            //en un solo objeto DateTime para procesarlo
                                            FechaInicio = new DateTime(FechaInicio.Year, FechaInicio.Month, FechaInicio.Day, HoraInicio.Hour, HoraInicio.Minute, HoraInicio.Second);
                                        }

                                        if (FechaInicio != new DateTime(1899, 12, 30, 0, 0, 0))
                                        {
                                            if (FechaInicio <= FechaFin)
                                                DiferenciaTiempo = FechaFin - FechaInicio;
                                            else
                                                DiferenciaTiempo = TimeSpan.Zero;
                                        }
                                        else
                                        {
                                            DiferenciaTiempo = TimeSpan.Zero;
                                            //En este caso la fecha de inicio y fin son la misma
                                            oChildCC.SetProperty("U_DFIni", FechaFin);
                                            oChildCC.SetProperty("U_HFIni", FechaFin);
                                        }

                                        if (UsaCalculoSobreHorarioTaller)
                                        {
                                            CalculoCostos.CostoManoObra.CalcularCostoCompuesto(Sucursal, FechaInicio, FechaFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref CostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                                            TotalMinutos = (CantidadHorasEstandar + CantidadHorasExtra) * 60.0;
                                        }
                                        else
                                        {
                                            CalcularCostoLinea(empID, IDActividad, ref CostoEstandar, ref CostoReal, DiferenciaTiempo.TotalMinutes);
                                            TotalMinutos = DiferenciaTiempo.TotalMinutes;
                                        }

                                        if (EstadoLinea == g_strEstado_NoIniciado)
                                        {
                                            TotalMinutos = 0;
                                            CostoReal = 0;
                                        }

                                        oChildCC.SetProperty("U_TMin", TotalMinutos);
                                        oChildCC.SetProperty("U_DFFin", FechaFin);
                                        oChildCC.SetProperty("U_HFFin", FechaFin);

                                        oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);
                                        oChildCC.SetProperty("U_CosRe", CostoReal);

                                        if (MetodoCosteoServicio == MetodoCosteo.TiempoEstandar)
                                        {
                                            CostoLineaOfertaVentas = ObtenerSumatoriaCostoEstandar(oMatrix, IDActividad);
                                        }
                                        else
                                        {
                                            //Se suman todas las líneas del mismo ID
                                            CostoLineaOfertaVentas = ObtenerSumatoriaCostoReal(ref LineasControlColaborador, IDActividad);
                                        }

                                        ActualizarActividadCotizacion(ref Cotizacion, IDActividad, g_strEstado_Finalizado, string.Empty, CostoLineaOfertaVentas, TotalMinutos);
                                        ListaActividadesFinalizadas.Add(string.Format("{0}{1}", IDActividad, empID));
                                        break;
                                    case g_strEstado_Suspendido:
                                        oChildCC = LineasControlColaborador.Add();
                                        FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                                        FechaFin = new DateTime(FechaInicio.Year, FechaInicio.Month, FechaInicio.Day, FechaInicio.Hour, FechaInicio.Minute, 0);
                                        oChildCC.SetProperty("U_DFIni", FechaInicio);
                                        oChildCC.SetProperty("U_HFIni", FechaInicio);
                                        oChildCC.SetProperty("U_DFFin", FechaFin);
                                        oChildCC.SetProperty("U_HFFin", FechaFin);
                                        oChildCC.SetProperty("U_TMin", 0);
                                        oChildCC.SetProperty("U_Estad", g_strEstado_Finalizado);

                                        if (UsaCalculoSobreHorarioTaller)
                                        {
                                            CalculoCostos.CostoManoObra.CalcularCostoCompuesto(Sucursal, FechaInicio, FechaFin, DuracionEstandar, SalarioPorHora, TarifaHorasExtra, ref CostoEstandar, ref CostoReal, ref CantidadHorasEstandar, ref CantidadHorasExtra, TrabajaFinSemana);
                                        }
                                        else
                                        {
                                            CalcularCostoLinea(empID, IDActividad, ref CostoEstandar, ref CostoReal, 0);
                                        }

                                        oChildCC.SetProperty("U_CosRe", CostoReal);
                                        oChildCC.SetProperty("U_CosEst", CostoEstandar);
                                        oChildCC.SetProperty("U_Colab", empID.ToString());
                                        oChildCC.SetProperty("U_IdAct", IDActividad);
                                        oChildCC.SetProperty("U_NoFas", NumeroFase);
                                        oChildCC.SetProperty("U_CodFas", CodigoFase);
                                        oChildCC.SetProperty("U_FechPro", FechaInicio);

                                        if (MetodoCosteoServicio == MetodoCosteo.TiempoEstandar)
                                        {
                                            CostoLineaOfertaVentas = ObtenerSumatoriaCostoEstandar(oMatrix, IDActividad);
                                        }
                                        else
                                        {
                                            //Se suman todas las líneas del mismo ID
                                            CostoLineaOfertaVentas = ObtenerSumatoriaCostoReal(ref LineasControlColaborador, IDActividad);
                                        }

                                        ActualizarActividadCotizacion(ref Cotizacion, IDActividad, g_strEstado_Finalizado, string.Empty, CostoLineaOfertaVentas);

                                        ListaActividadesFinalizadas.Add(string.Format("{0}{1}", IDActividad, empID));
                                        break;
                                    case g_strEstado_Finalizado:
                                        ListaActividadesFinalizadas.Add(string.Format("{0}{1}", IDActividad, empID));
                                        break;
                                }
                            }
                        }
                    }
                    Utilitarios.CreaMensajeSBO(Resource.OrdenActualizada, DocEntryCotizacion, (SAPbobsCOM.Company)CompanySBO, NumeroOT, false, Sucursal, true, GeneralEnums.RolesMensajeria.EncargadoProduccion, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FinalizarLineasSinAsignar(List<string> ListaActividadesSinAsignar, ref SAPbobsCOM.Documents Cotizacion)
        {
            string IDActividad = string.Empty;
            try
            {
                for (int i = 0; i < ListaActividadesSinAsignar.Count; i++)
                {
                    IDActividad = ListaActividadesSinAsignar[i].Trim();
                    ActualizarActividadCotizacion(ref Cotizacion, IDActividad, g_strEstado_Finalizado, string.Empty, 0, 0);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CalcularCostoLinea(int empID, string IDActividad, ref double CostoEstandar, ref double CostoReal, double CantidadMinutos)
        {
            SAPbobsCOM.EmployeesInfo Empleado;
            double SalarioPorHora;
            string Query = "SELECT TOP 1 T1.\"U_SCGD_Duracion\" FROM \"QUT1\" T0 INNER JOIN \"OITM\" T1 ON T0.\"ItemCode\" = T1.\"ItemCode\" WHERE T0.\"U_SCGD_ID\" = '{0}'";
            int DuracionEstandar;
            try
            {
                Empleado = (SAPbobsCOM.EmployeesInfo)DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oEmployeesInfo);
                Empleado.GetByKey(empID);
                SalarioPorHora = double.Parse(Empleado.UserFields.Fields.Item("U_SCGD_sALXHORA").Value.ToString());
                CostoReal = (CantidadMinutos / 60) * SalarioPorHora;

                if (CostoEstandar <= 0)
                {
                    //El costo estándar debe haberse calculado al agregar la actividad
                    //en caso de no estar calculado se vuelve a recalcular
                    Query = string.Format(Query, IDActividad);
                    DuracionEstandar = Convert.ToInt32(DMS_Connector.Helpers.EjecutarConsulta(Query));
                    CostoEstandar = (DuracionEstandar / 60) * SalarioPorHora;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw;
            }
        }

        private MetodoCosteo ObtenerMetodoCosteo(string Sucursal)
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
                throw;
            }
        }

        private static double ObtenerSumatoriaCostoReal(ref SAPbobsCOM.GeneralDataCollection ChildrenCollection, string IDActividadBuscado)
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

                    if (IDActividad == IDActividadBuscado)
                    {
                        CostoRealTotal += (double)oChild.GetProperty("U_CosRe");
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

        private double ObtenerSumatoriaCostoEstandar(SAPbouiCOM.Matrix oMatrix, string IDActividad)
        {
            double Sumatoria = 0;
            string Valor = string.Empty;
            List<string> ListaMecanicosAsignados;
            string IDMecanico = string.Empty;
            try
            {
                ListaMecanicosAsignados = new List<string>();
                for (int i = 1; i <= oMatrix.RowCount; i++)
                {
                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", i - 1).Trim() == IDActividad)
                    {
                        IDMecanico = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", i - 1).Trim();

                        if (!string.IsNullOrEmpty(IDMecanico) && !ListaMecanicosAsignados.Contains(IDMecanico))
                        {
                            Valor = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_CosEst", i - 1);
                            //En caso de tener varios mecánicos asignados, se suma el costo estándar del primer mecánico solamente
                            //es decir se utiliza el primero costo estándar encontrado
                            //Sumatoria += Convert.ToDouble(Valor, n);

                            //Se debe utilizar solamente el último costo estándar
                            Sumatoria = Convert.ToDouble(Valor, n);
                            ListaMecanicosAsignados.Add(IDMecanico);
                        }
                    }
                }

                return Sumatoria;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw;
            }
        }



        /// <summary>
        /// Verifica si la fecha corresponde a la fecha mínima para un objeto de tipo COM Interop. Debe ser utilizada al realizar
        /// intercambio de fechas Nulas entre .NET y COM Interops (Los objetos de SAP).
        /// </summary>
        /// <returns>True = Si es la fecha mínima, False = No es la fecha mínima</returns>
        private bool EsFechaMinimaCOM(DateTime p_dtFechaComparar)
        {
            //Fecha mínima en formato 24 horas = 30/12/1899 00:00:00
            DateTime dtFechaMinima24;
            //Fecha mínima en formato 12 horas = 30/12/1899 12:00:00
            DateTime dtFechaMinima12;

            try
            {
                //Obtiene las fechas mínimas a través de su correspondiente código binario
                dtFechaMinima12 = DateTime.FromBinary(599264784000000000);
                dtFechaMinima24 = DateTime.FromBinary(599264352000000000);

                if (p_dtFechaComparar.Equals(dtFechaMinima12) || p_dtFechaComparar.Equals(dtFechaMinima24))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }

            return false;
        }

        private double ObtieneCostosReal(string p_strColaborador, double p_dblMinutos, Form oFormOt = null)
        {
            string strConsulta = " select U_SCGD_sALXHORA as sal from OHEM with (nolock) where empID IN ({0}) ";
            DataTable dtEmpleados;
            double dblSalario = 0;
            double dblCostoActividad = 0;

            if (oFormOt != null)
            {
                dtEmpleados = oFormOt.DataSources.DataTables.Item(g_strdtEmpleado);
            }
            else
            {
                dtEmpleados = FormularioSBO.DataSources.DataTables.Item(g_strdtEmpleado);
            }

            strConsulta = string.Format(strConsulta, p_strColaborador);
            dtEmpleados.ExecuteQuery(strConsulta);

            dblSalario = double.Parse(dtEmpleados.GetValue("sal", 0).ToString().Trim());

            dblCostoActividad = (p_dblMinutos / 60) * dblSalario;

            return dblCostoActividad;
        }

        public void ActualizarActividadCotizacion(ref Documents m_objCotizacion, string p_strIdActividad, string p_strCodEstadoActividad, string strCodFase, double p_dblCostoActividad = 0, double p_dblMinutos = 0)
        {
            SAPbobsCOM.Document_Lines m_oLineasCotizacion;
            double dblTiempoReal;

            try
            {

                m_oLineasCotizacion = m_objCotizacion.Lines;
                for (int i = 0; i <= m_oLineasCotizacion.Count - 1; i++)
                {
                    m_oLineasCotizacion.SetCurrentLine(i);

                    if (m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == p_strIdActividad)
                    {
                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EstAct").Value = p_strCodEstadoActividad;
                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Costo").Value = p_dblCostoActividad;

                        if (p_strCodEstadoActividad == "4" || p_strCodEstadoActividad == "3")
                        {
                            dblTiempoReal = double.Parse(m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_TiempoReal").Value.ToString());
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_TiempoReal").Value = dblTiempoReal + p_dblMinutos;
                        }

                        if (!string.IsNullOrEmpty(strCodFase))
                        {
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_FasePro").Value = strCodFase;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

        }

        private int ValidaEstadoOT(ref GeneralData oGeneralData)
        {
            if (oGeneralData.GetProperty("U_EstO").ToString().Trim().Equals("3"))
                return 3;
            else
                return 2;
        }

        public void ObtieneDescripcionEstado(string p_idEstado, ref string m_strEstadoIniciadoDes, Form oFormOt = null)
        {
            SAPbouiCOM.DataTable m_dtEstadosOT;
            if (oFormOt != null)
            {
                m_dtEstadosOT = oFormOt.DataSources.DataTables.Item("tEstadosOT");
            }
            else
            {
                m_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item("tEstadosOT");
            }

            for (int i = 0; i <= m_dtEstadosOT.Rows.Count - 1; i++)
            {
                if (m_dtEstadosOT.GetValue("Code", i).ToString().Trim() == p_idEstado)
                {
                    m_strEstadoIniciadoDes = m_dtEstadosOT.GetValue("Name", i).ToString().Trim();
                    break;
                }
            }
        }

        private void EliminarActividadControlColaborador(ItemEvent pval)
        {
            CompanyService oCompanyService;
            GeneralService oGeneralService;
            GeneralData oGeneralData;
            GeneralData oChildCC;
            GeneralDataCollection oChildrenCtrlCol;
            GeneralDataParams oGeneralParams;
            int m_intposicion;
            int intError;
            string strError;
            string m_strNoOT;
            bool blnActividadEliminada;
            Form oFormOT;

            try
            {
                oFormOT = ApplicationSBO.Forms.Item("SCGD_ORDT");
                string m_strIdCotizacion = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                string m_strIdActividad;
                string g_strEstado = string.Empty;
                string m_str_CodFase;
                Matrix m_objMatrix;

                blnActividadEliminada = false;
                m_intposicion = 0;
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                m_objMatrix.FlushToDataSource();
                Documents m_objCotizacion = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strNoOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                if (m_objCotizacion.GetByKey(Convert.ToInt32(m_strIdCotizacion)))
                {
                    for (int i = m_objMatrix.RowCount; i >= 1; i--)
                    {
                        if (m_objMatrix.IsRowSelected(i))
                        {
                            oChildCC = oChildrenCtrlCol.Item(i - 1);
                            if (int.Parse(oChildCC.GetProperty("U_Estad").ToString()) == 1 ||
                                int.Parse(oChildCC.GetProperty("U_Estad").ToString()) == 2)
                            {
                                m_strIdActividad = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                                for (int index = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Size; index >= 1; index--)
                                {
                                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index - 1).Trim() == m_strIdActividad)
                                    {
                                        if (!m_objMatrix.IsRowSelected(index))
                                        {
                                            g_strEstado = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", index - 1).Trim();
                                            g_dtAdicionalesColaborador.Rows.Add(1);
                                            g_dtAdicionalesColaborador.SetValue("IdAct", m_intposicion, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", index - 1).Trim());
                                            g_dtAdicionalesColaborador.SetValue("IdCol", m_intposicion, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index - 1).Trim());
                                            m_intposicion = g_dtAdicionalesColaborador.Rows.Count;
                                            break;
                                        }

                                        EliminaLineaDataTable(m_strIdActividad, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Colab", index - 1).Trim());

                                    }

                                }
                                if (string.IsNullOrEmpty(g_strEstado))
                                {
                                    g_strEstado = g_strEstado_NoIniciado;
                                    g_dtAdicionalesColaborador.Rows.Add(1);
                                    g_dtAdicionalesColaborador.SetValue("IdAct", m_intposicion, m_strIdActividad);
                                    g_dtAdicionalesColaborador.SetValue("IdCol", m_intposicion, "");
                                    m_intposicion = g_dtAdicionalesColaborador.Rows.Count;
                                }
                                m_str_CodFase = oChildCC.GetProperty("U_CodFas").ToString().Trim();
                                oChildrenCtrlCol.Remove(i - 1);
                                ActualizarActividadCotizacion(ref m_objCotizacion, m_strIdActividad, g_strEstado, m_str_CodFase);
                                blnActividadEliminada = true;
                            }
                            else
                            {
                                ApplicationSBO.StatusBar.SetText(Resource.NoSePuedeEliminarAsignacion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            }

                        }
                    }
                    if (blnActividadEliminada)
                    {
                        if (g_dtAdicionalesColaborador.Rows.Count > 0)
                        {
                            ActualizaLineasServiciosAsiganados(ref m_objCotizacion);
                        }
                        ManejarEstadoOT(false, true, false, ref oGeneralData);

                        var estado = ValidaEstadoOT(ref oGeneralData);
                        var descEstado = string.Empty;
                        ObtieneDescripcionEstado(estado.ToString(), ref descEstado, oFormOT);

                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = estado.ToString();
                        m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = descEstado;

                        CompanySBO.StartTransaction();
                        if (m_objCotizacion.Update() == 0)
                        {
                            oGeneralService.Update(oGeneralData);
                            if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            CompanySBO.GetLastError(out intError, out strError);
                            throw new Exception(string.Format("{0}: {1}", intError, strError));
                        }

                        recargarActividades(m_strNoOT, ApplicationSBO);
                        FormularioSBO.Mode = BoFormMode.fm_OK_MODE;

                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }
        }

        /// <summary>
        /// Elimina Linea de Datatable
        /// </summary>
        /// <param name="m_strIdActividad"></param>
        private void EliminaLineaDataTable(string m_strIdActividad, string m_strIdMec)
        {
            try
            {
                for (int i = g_dtAdicionalesColaborador.Rows.Count - 1; i >= 0; i--)
                {
                    if (g_dtAdicionalesColaborador.GetValue("IdAct", i).ToString().Trim() == m_strIdActividad && g_dtAdicionalesColaborador.GetValue("IdCol", i).ToString().Trim() == m_strIdMec)
                    {
                        g_dtAdicionalesColaborador.Rows.Remove(i);
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ManejadorEventoFormDataLoad(SAPbouiCOM.Form p_oForm)
        {
            SAPbouiCOM.DataTable dtConsulta;
            string strCambPreTall;

            try
            {
                string m_strConsulta;
                string m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                string m_strSucursalOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                string strAsesor = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Ase", 0).Trim();
                string m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();

                g_StrNoOT = m_strNoOT;

                ComboBox m_objCombo;

                Matrix m_objMatrix;
                Column m_objColumnColaborador;
                Column m_objColumnActividad;

                if (!String.IsNullOrEmpty(m_strNoOT))
                {
                    FormularioSBO.Freeze(true);

                    CargaMatrices(true, true, true, true, true, true);

                    if (string.IsNullOrEmpty(strAsesor))
                    {
                        strAsesor = "0";
                    }
                    m_objCombo = (ComboBox)FormularioSBO.Items.Item("txtAse").Specific;


                    Utilitarios.CargaComboBox(string.Format(" SELECT empid Code, SUBSTRING (firstName  + ' ' + lastName,0,30) Name FROM OHEM WITH (NOLOCK) where empid = {0}", int.Parse(strAsesor)),
                        "Code", "Name", g_dtConsultaCombos, ref m_objCombo, false);

                    m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                    g_objMatrix = m_objMatrix;

                    m_objColumnColaborador = m_objMatrix.Columns.Item("Col_col");

                    Utilitarios.CargaComboBox(string.Format("  select empID as Code,ISNULL(firstName,'')  + ' ' + isnull(middleName,'')  + ' ' + ISNULL(lastName,'') as Name from OHEM T0 with(nolock) where U_SCGD_T_Fase is not null AND branch = {0} AND Active = 'Y' ", Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim())),
                        "Code", "Name", g_dtConsultaCombos, ref m_objColumnColaborador);

                    m_objColumnActividad = m_objMatrix.Columns.Item("Col_IdAct");

                    Utilitarios.CargaComboBox(
                        string.Format(" Select U_SCGD_ID as Code, Dscription as Name  from QUT1 with(nolock) where docentry = {0} and U_SCGD_TipArt = '2' and U_SCGD_ID is not null and U_SCGD_ID <> '' ",
                            m_strDocEntry), "Code", "Name", g_dtConsultaCombos, ref m_objColumnActividad);


                    m_strConsulta = string.Format(g_strConsultaConfSucursal, m_strSucursalOT);
                    g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                    g_dtConfSucursal.ExecuteQuery(m_strConsulta);

                    m_strConsulta = string.Format(g_strConsultaAprobacion, m_strNoOT, m_strSucursalOT);
                    g_dtAprobacion = FormularioSBO.DataSources.DataTables.Item(g_strdtAprobacion);
                    g_dtAprobacion.ExecuteQuery(m_strConsulta);

                    if (g_dtAprobacion.Rows.Count <= 1 &&
                        string.IsNullOrEmpty(g_dtAprobacion.GetValue(0, 0).ToString().Trim()))
                    {
                        g_dtAprobacion.ExecuteQuery(" select U_AdcApr as U_ItmAprob from [@SCGD_ADMIN]  with(nolock) ");
                    }

                    g_dtRepuestosSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestosSeleccionados);
                    g_dtRepuestosSeleccionados.Rows.Clear();
                    g_dtSuministrosSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtSuministrosSeleccionados);
                    g_dtSuministrosSeleccionados.Rows.Clear();
                    g_dtServiciosExternosSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosExternosSeleccionados);
                    g_dtServiciosExternosSeleccionados.Rows.Clear();

                    ManejaControlesOT();

                    FormularioSBO.EnableMenu("1283", false);

                    string strIdSucurs = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();

                    g_strCreaHjaCanPend = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == strIdSucurs).U_HjaCanPen.Trim();
                    strCambPreTall = string.Empty;

                    if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == strIdSucurs).U_UsaEstadosOTP.Trim() == "Y")
                    {
                        string strIdEstaTC = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_IdEstOTTC", 0).Trim();
                        string[] idArray;
                        idArray = strIdEstaTC.Split(',', '.');
                        strIdEstaTC = idArray[0];

                        if (!string.IsNullOrEmpty(strIdEstaTC))
                        {
                            string strEstadoActual = Helpers.EjecutarConsulta(string.Format("Select \"U_Descripcion\" FROM \"@SCGD_ESTADOS_OT_P\" WHERE \"Code\" = {0} AND \"U_Sucursal\" = '{1}' ", strIdEstaTC, strIdSucurs));
                            SAPbouiCOM.EditText oEstadoTC = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtEsTC").Specific;
                            oEstadoTC.Value = strEstadoActual;
                        }
                        
                        FormularioSBO.Items.Item("txtEsTC").Visible = true;
                        FormularioSBO.Items.Item("lblEsTC").Visible = true;
                        FormularioSBO.Items.Item("txtEsTC").FromPane = 1;
                        FormularioSBO.Items.Item("lblEsTC").ToPane = 1;
                    }
                    else
                    {
                        FormularioSBO.Items.Item("txtEsTC").Visible = false;
                        FormularioSBO.Items.Item("lblEsTC").Visible = false;
                    }

                    foreach (Configuracion_Sucursal confSucursal in DMS_Connector.Configuracion.ConfiguracionSucursales)
                    {
                        if (confSucursal.U_Sucurs == strIdSucurs)
                        {
                            strCambPreTall = confSucursal.U_CambPreTall.Trim();
                            break;
                        }
                    }


                    ManejoEstadoColumnas(strCambPreTall, FormularioSBO, Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim()));

                    CargarPestanasConfig(strIdSucurs);
                    ValidaModoVistaOT((SAPbouiCOM.Form)FormularioSBO,false);
                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        /// <summary>
        /// Metodo para ocultar las pestañas del taller segun configuracion de sucursal.
        /// </summary>
        /// <param name="p_strIdSucurs">Id de sucursal de la OT</param>
        private void CargarPestanasConfig(string p_strIdSucurs)
        {
            string strValor;

            try
            {
                //Suministros
                strValor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == p_strIdSucurs).U_UseSum.Trim();
                if (!string.IsNullOrEmpty(strValor) && strValor == "N")
                {
                    FormularioSBO.Items.Item("Folder7").Visible = false;
                }
                else
                {
                    FormularioSBO.Items.Item("Folder7").Visible = true;
                }

                //Repuestos
                strValor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == p_strIdSucurs).U_UseParts.Trim();
                if (!string.IsNullOrEmpty(strValor) && strValor == "N")
                {
                    FormularioSBO.Items.Item("Folder3").Visible = false;
                }
                else
                {
                    FormularioSBO.Items.Item("Folder3").Visible = true;
                }

                //Servicios
                strValor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == p_strIdSucurs).U_UseServ.Trim();
                if (!string.IsNullOrEmpty(strValor) && strValor == "N")
                {
                    FormularioSBO.Items.Item("Folder4").Visible = false;
                }
                else
                {
                    FormularioSBO.Items.Item("Folder4").Visible = true;
                }

                //Servicios Externos
                strValor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == p_strIdSucurs).U_UseSE.Trim();
                if (!string.IsNullOrEmpty(strValor) && strValor == "N")
                {
                    FormularioSBO.Items.Item("Folder5").Visible = false;
                }
                else
                {
                    FormularioSBO.Items.Item("Folder5").Visible = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ManejaControlesOT()
        {
            bool blnGeneraOTEspeciales;
            bool blnSolicitudOTEspecial;

            try
            {

                SAPbouiCOM.Item m_oItem;
                string m_strEstadoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim();
                string m_strIdSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                string strValidaCreaOTESP = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == m_strIdSucursal).U_ValOTCreEsp.Trim();
                blnGeneraOTEspeciales = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == m_strIdSucursal).U_GenOTEsp.Trim() == "Y";
                blnSolicitudOTEspecial = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == m_strIdSucursal).U_USolOTEsp.Trim() == "Y";
                bool blnEstado;

                if (m_strEstadoOT == "4" || m_strEstadoOT == "5" || m_strEstadoOT == "6" ||
                    m_strEstadoOT == "7" || m_strEstadoOT == "8")
                {
                    blnEstado = false;
                }
                else
                {
                    blnEstado = true;
                }

                m_oItem = FormularioSBO.Items.Item("btnArch");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnOTEsp");


                //Maneja estado de boton para crear OT especiales
                if (blnGeneraOTEspeciales || blnSolicitudOTEspecial)
                {
                    
                    //Activo el boton de crear OT hijas dependiendo de la configuracion
                    if (strValidaCreaOTESP == "Y" && !string.IsNullOrEmpty(strValidaCreaOTESP) && m_strEstadoOT != "5")
                    {
                        m_oItem.Enabled = true;
                    }
                    else
                    {
                        m_oItem.Enabled = blnEstado;
                    }
                }
                else
                {
                    m_oItem.Visible = false;
                }



                m_oItem = FormularioSBO.Items.Item("btnAsigM");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnIniA");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnSuspA");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnFinA");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnElim");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnAgS");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnEliS");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnOCom");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnAgR");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnEliR");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnComSum");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnAddSum");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnEliSum");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("129");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnAgSE");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("btnEliSE");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("mtxColab");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("mtxSer");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("mtxRep");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("mtxSum");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("mtxServE");
                m_oItem.Enabled = blnEstado;

                m_oItem = FormularioSBO.Items.Item("136");
                m_oItem.Enabled = blnEstado;

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ManejaPermisosTab()
        {
            SAPbouiCOM.BoModeVisualBehavior m_blnProduccion,
                 m_blnRepuestos,
                 m_blnServicios,
                 m_blnSuministros,
                 m_blnServiciosExternos,
                 m_blnGastosEIngresos;

            try
            {
                m_blnProduccion = DMS_Connector.Helpers.PermisosMenu("SCGD_PRO") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                m_blnRepuestos = DMS_Connector.Helpers.PermisosMenu("SCGD_REP") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                m_blnServicios = DMS_Connector.Helpers.PermisosMenu("SCGD_SER") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                m_blnSuministros = DMS_Connector.Helpers.PermisosMenu("SCGD_SUM") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                m_blnServiciosExternos = DMS_Connector.Helpers.PermisosMenu("SCGD_SEX") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
                m_blnGastosEIngresos = DMS_Connector.Helpers.PermisosMenu("SCGD_GIN") ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;

                FormularioSBO.Items.Item("Folder2").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnProduccion);
                FormularioSBO.Items.Item("Folder3").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnRepuestos);
                FormularioSBO.Items.Item("Folder4").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnServicios);
                FormularioSBO.Items.Item("Folder7").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnSuministros);
                FormularioSBO.Items.Item("Folder5").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnServiciosExternos);
                FormularioSBO.Items.Item("Folder6").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, m_blnGastosEIngresos);


            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargaMatrices(bool p_blnRep, bool p_blnSer, bool p_blnSE, bool p_blnSum, bool p_blnOtrosGastos, bool p_blnOtrosIngresos)
        {
            string m_strConsulta;
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.Item m_objItem;
            SAPbouiCOM.ComboBox m_objCombo;
            string m_strValorCombo = string.Empty;
            string m_strMatriz = string.Empty;
            string m_strIdSucu = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
            string m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();

            try
            {
                if (!String.IsNullOrEmpty(m_strDocEntry))
                {
                    if (p_blnRep)
                    {

                        if (g_realizofiltroRepuestos)
                        {

                            m_strMatriz = "mtxRep";
                            g_dtRepuestos = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestos);
                            oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
                            oMatrix.FlushToDataSource();
                            g_objMatrizRepuestos.TablaLigada = g_strdtRepuestos;
                            g_objMatrizRepuestos.LigaColumnas();
                        }

                        m_strConsulta = g_strConsultaRepuestos;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtRepuestos.ExecuteQuery(m_strConsulta);
                        g_objMatrizRepuestos.Matrix.LoadFromDataSource();

                        if (g_realizofiltroRepuestos)
                        {
                            m_objItem = FormularioSBO.Items.Item("cboEstR");
                            m_objCombo = (ComboBox)m_objItem.Specific;
                            m_strValorCombo = m_objCombo.Value;
                            m_strValorCombo = m_strValorCombo.Trim();
                            ManejaFiltroCombo(g_dtRepuestos, 1, m_strValorCombo);
                            g_realizofiltroRepuestos = false;
                        }
                    }

                    if (p_blnSum)
                    {

                        if (g_realizofiltroSuministros)
                        {
                            m_strMatriz = "mtxSum";
                            g_dtSuministros = FormularioSBO.DataSources.DataTables.Item(g_strdtSuministros);
                            oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
                            oMatrix.FlushToDataSource();
                            g_objMatrizSuministros.TablaLigada = g_strdtSuministros;
                            g_objMatrizSuministros.LigaColumnas();
                        }

                        m_strConsulta = g_strConsultaSuministros;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtSuministros.ExecuteQuery(m_strConsulta);
                        g_objMatrizSuministros.Matrix.LoadFromDataSource();

                        if (g_realizofiltroSuministros)
                        {
                            m_objItem = FormularioSBO.Items.Item("cboEstSu");
                            m_objCombo = (ComboBox)m_objItem.Specific;
                            m_strValorCombo = m_objCombo.Value;
                            m_strValorCombo = m_strValorCombo.Trim();
                            ManejaFiltroCombo(g_dtSuministros, 3, m_strValorCombo);
                            g_realizofiltroSuministros = false;
                        }
                    }

                    if (p_blnSer)
                    {
                        if (g_realizofiltroServicios)
                        {
                            m_strMatriz = "mtxSer";
                            g_dtServicios = FormularioSBO.DataSources.DataTables.Item(g_strdtServicios);
                            oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
                            oMatrix.FlushToDataSource();
                            g_objMatrizServicios.TablaLigada = g_strdtServicios;
                            g_objMatrizServicios.LigaColumnas();
                        }

                        m_strConsulta = g_strConsultaServicios;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtServicios.ExecuteQuery(m_strConsulta);
                        g_objMatrizServicios.Matrix.LoadFromDataSource();

                        if (g_realizofiltroServicios)
                        {
                            m_objItem = FormularioSBO.Items.Item("cboFProS");
                            m_objCombo = (ComboBox)m_objItem.Specific;
                            m_strValorCombo = m_objCombo.Value;
                            m_strValorCombo = m_strValorCombo.Trim();
                            ManejaFiltroCombo(g_dtServicios, 2, m_strValorCombo);
                            g_realizofiltroServicios = false;
                        }
                    }

                    if (p_blnSE)
                    {

                        if (g_realizofiltroServiciosExter)
                        {
                            m_strMatriz = "mtxServE";
                            g_dtServiciosExt = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosExternos);
                            oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
                            oMatrix.FlushToDataSource();
                            g_objMatrizServiciosExt.TablaLigada = g_strdtServiciosExternos;
                            g_objMatrizServiciosExt.LigaColumnas();
                        }
                        m_strConsulta = g_strConsultaServiciosExt;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtServiciosExt.ExecuteQuery(m_strConsulta);
                        g_objMatrizServiciosExt.Matrix.LoadFromDataSource();

                        if (g_realizofiltroServiciosExter)
                        {
                            m_objItem = FormularioSBO.Items.Item("cboEstSE");
                            m_objCombo = (ComboBox)m_objItem.Specific;
                            m_strValorCombo = m_objCombo.Value;
                            m_strValorCombo = m_strValorCombo.Trim();
                            ManejaFiltroCombo(g_dtServiciosExt, 4, m_strValorCombo);
                            g_realizofiltroServicios = false;
                        }
                    }

                    if (p_blnOtrosGastos)
                    {
                        m_strConsulta = g_strConsultaGastos;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtGastos.ExecuteQuery(m_strConsulta);
                        g_objMatrizGastos.Matrix.LoadFromDataSource();
                    }

                    if (p_blnOtrosIngresos)
                    {
                        m_strConsulta = g_strConsultaIngresos;
                        m_strConsulta = string.Format(m_strConsulta, int.Parse(m_strDocEntry), m_strIdSucu);
                        g_dtIngresos.ExecuteQuery(m_strConsulta);
                        g_objMatrizIngresos.Matrix.LoadFromDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void ManejadorEventoMenuEvent(bool p_blnCrear, bool p_blnBuscar)
        {
            try
            {
                if (p_blnBuscar)
                {
                    FormularioSBO.Items.Item("txtNoOrd").Enabled = true;
                    FormularioSBO.Items.Item("txtCodCli").Enabled = true;
                    FormularioSBO.Items.Item("txtNoOrd").Click();
                    FormularioSBO.Items.Item("txtNoUni").Enabled = true;
                    FormularioSBO.Items.Item("txtNoCon").Enabled = true;
                    FormularioSBO.Items.Item("txtPla").Enabled = true;
                    FormularioSBO.Items.Item("txtMar").Enabled = true;
                    FormularioSBO.Items.Item("txtEst").Enabled = true;
                    FormularioSBO.Items.Item("txtVis").Enabled = true;
                    FormularioSBO.Items.Item("txtEstVi").Enabled = true;
                    FormularioSBO.Items.Item("txtVIN").Enabled = true;
                    FormularioSBO.Items.Item("txtkm").Enabled = true;
                    FormularioSBO.Items.Item("cboTipOT").Enabled = true;
                    FormularioSBO.Items.Item("cboEstW").Enabled = true;
                    FormularioSBO.Items.Item("txtCodCOT").Enabled = true;

                    FormularioSBO.Items.Item("btnArch").Enabled = false;
                    FormularioSBO.Items.Item("btnOTEsp").Enabled = false;
                    FormularioSBO.Items.Item("btnAsigM").Enabled = false;

                    FormularioSBO.Items.Item("btnIniA").Enabled = false;
                    FormularioSBO.Items.Item("btnSuspA").Enabled = false;
                    FormularioSBO.Items.Item("btnFinA").Enabled = false;
                    FormularioSBO.Items.Item("btnElim").Enabled = false;
                    FormularioSBO.Items.Item("cboFProS").Enabled = false;
                    FormularioSBO.Items.Item("btnAgS").Enabled = false;
                    FormularioSBO.Items.Item("btnEliS").Enabled = false;
                    FormularioSBO.Items.Item("cboEstR").Enabled = false;
                    FormularioSBO.Items.Item("btnOCom").Enabled = false;
                    FormularioSBO.Items.Item("btnAgR").Enabled = false;
                    FormularioSBO.Items.Item("btnEliR").Enabled = false;
                    FormularioSBO.Items.Item("cboEstSu").Enabled = false;
                    FormularioSBO.Items.Item("btnComSum").Enabled = false;
                    FormularioSBO.Items.Item("btnAddSum").Enabled = false;
                    FormularioSBO.Items.Item("btnEliSum").Enabled = false;
                    FormularioSBO.Items.Item("cboEstSE").Enabled = false;
                    FormularioSBO.Items.Item("129").Enabled = false;
                    FormularioSBO.Items.Item("btnAgSE").Enabled = false;
                    FormularioSBO.Items.Item("btnEliSE").Enabled = false;
                    FormularioSBO.Items.Item("136").Enabled = false;

                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaCreacion(ref bool bubbleEvent)
        {
            int m_intMsj = 0;

            if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Cotiz", 0).Trim() == "Y"
                || string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_RCot", 0).ToString().Trim()) == false)
            {
                bubbleEvent = false;
                ApplicationSBO.StatusBar.SetText("Ya existe una cotización creada para esta OT", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            else
            {
                m_intMsj = ApplicationSBO.MessageBox("Desea crear una OT?", 1, "SI", "NO");
                if (m_intMsj == 2) { bubbleEvent = false; }
            }
        }

        public void AgregaAdicionales(DataTable dtAdicionales, int p_intTipoFormulario, IApplication applicationSbo)
        {
            try
            {
                string strMatriz = string.Empty;
                SAPbouiCOM.Matrix oMatrix;
                SAPbouiCOM.Form oForm;

                DataTable m_dtDataSource = null;
                string m_strMatriz = string.Empty;


                string m_str_Codigo = string.Empty;
                string m_str_Descripcion = string.Empty;
                string m_str_Cantidad = string.Empty;
                string m_str_Bodega = string.Empty;
                string m_str_Precio = string.Empty;
                string m_str_Moneda = string.Empty;
                string m_str_Estado = string.Empty;
                string m_str_Duracion = string.Empty;
                string m_str_NoFase = string.Empty;

                string m_str_ColPermanente = string.Empty;
                string m_str_ColCodigo = string.Empty;
                string m_str_ColDescripcion = string.Empty;
                string m_str_ColBodega = string.Empty;
                string m_str_ColPrecio = string.Empty;
                string m_str_ColMoneda = string.Empty;
                string m_str_ColCantidad = string.Empty;
                string m_str_ColEstado = string.Empty;
                string m_str_ColDuracion = string.Empty;
                string m_str_ColNoFase = string.Empty;
                string m_str_ColAdicional = string.Empty;

                oForm = applicationSbo.Forms.Item("SCGD_ORDT");

                switch (p_intTipoFormulario)
                {
                    case (int)TipoAdicional.Repuesto:
                        m_strMatriz = "mtxRep";

                        m_dtDataSource = oForm.DataSources.DataTables.Item(!g_realizofiltroRepuestos ? g_strdtRepuestos : g_strdtRepuestosTemporal);

                        m_str_ColPermanente = "perm";
                        m_str_ColCodigo = "code";
                        m_str_ColDescripcion = "desc";
                        m_str_ColBodega = "alma";
                        m_str_ColPrecio = "prec";
                        m_str_ColMoneda = "mone";
                        m_str_ColCantidad = "cant";
                        m_str_ColAdicional = "adic";
                        break;
                    case (int)TipoAdicional.Suministro:
                        m_strMatriz = "mtxSum";
                        m_dtDataSource = oForm.DataSources.DataTables.Item(!g_realizofiltroSuministros ? g_strdtSuministros : g_strdtSuministrosTemporal);

                        m_str_ColPermanente = "perm";
                        m_str_ColCodigo = "code";
                        m_str_ColDescripcion = "desc";
                        m_str_ColBodega = "alma";
                        m_str_ColPrecio = "prec";
                        m_str_ColMoneda = "mone";
                        m_str_ColCantidad = "cant";
                        m_str_ColAdicional = "adic";
                        break;
                    case (int)TipoAdicional.Servicio:
                        m_strMatriz = "mtxSer";

                        m_dtDataSource = oForm.DataSources.DataTables.Item(!g_realizofiltroServicios ? g_strdtServicios : g_strdtServiciosTemporal);

                        m_str_ColPermanente = "perm";
                        m_str_ColCodigo = "code";
                        m_str_ColDescripcion = "desc";
                        m_str_ColBodega = "alma";
                        m_str_ColPrecio = "prec";
                        m_str_ColMoneda = "mone";
                        m_str_ColCantidad = "cant";
                        m_str_ColAdicional = "adic";
                        m_str_ColEstado = "esta";
                        m_str_ColDuracion = "dura";
                        m_str_ColNoFase = "nofa";
                        break;
                    case (int)TipoAdicional.ServicioExterno:
                        m_strMatriz = "mtxServE";

                        m_dtDataSource = oForm.DataSources.DataTables.Item(!g_realizofiltroServiciosExter ? g_strdtServiciosExternos : g_strdtServiciosExternosTemporal);

                        m_str_ColPermanente = "perm";
                        m_str_ColCodigo = "code";
                        m_str_ColDescripcion = "desc";
                        m_str_ColBodega = "alma";
                        m_str_ColPrecio = "prec";
                        m_str_ColMoneda = "mone";
                        m_str_ColCantidad = "cant";
                        m_str_ColAdicional = "adic";
                        m_str_ColEstado = "esta";
                        m_str_ColDuracion = "dura";
                        m_str_ColNoFase = "nofa";
                        break;
                }

                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(m_strMatriz).Specific;

                oMatrix.FlushToDataSource();

                if (dtAdicionales.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtAdicionales.Rows.Count - 1; i++)
                    {
                        if (dtAdicionales.GetValue("sele", i).ToString().Trim().Equals("Y"))
                        {
                            m_str_Codigo = dtAdicionales.GetValue("code", i).ToString().Trim();

                            if (string.IsNullOrEmpty(m_str_Codigo) == false)
                            {
                                m_str_Descripcion = dtAdicionales.GetValue("desc", i).ToString().Trim();
                                m_str_Bodega = dtAdicionales.GetValue("bode", i).ToString().Trim();
                                m_str_Precio = dtAdicionales.GetValue("prec", i).ToString().Trim();
                                m_str_Moneda = dtAdicionales.GetValue("mone", i).ToString().Trim();
                                m_str_Estado = "1";
                                m_str_Duracion = dtAdicionales.GetValue("dura", i).ToString().Trim();
                                m_str_NoFase = dtAdicionales.GetValue("nofa", i).ToString().Trim();
                                m_str_Cantidad = dtAdicionales.GetValue("cant", i).ToString();

                                if (m_dtDataSource != null)
                                {
                                    AgregaAdicional(oForm, ref m_dtDataSource,
                                        m_str_Codigo, m_str_Descripcion, m_str_Bodega, m_str_Precio, m_str_Moneda, m_str_Cantidad, m_str_Estado, m_str_Duracion, m_str_NoFase,
                                        m_str_ColCodigo, m_str_ColDescripcion, m_str_ColBodega, m_str_ColPrecio, m_str_ColMoneda, m_str_ColCantidad, m_str_ColEstado, m_str_ColDuracion, m_str_ColNoFase,
                                        m_str_ColPermanente, m_str_ColAdicional, p_intTipoFormulario);
                                }
                            }
                        }

                    }
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                oMatrix.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void AgregaActividadesDesdeAsignacion(DataTable dtActividades, string p_Colaborador,  IApplication applicationSbo)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralDataCollection oChildrenCtrlCol;
            SAPbobsCOM.GeneralDataParams oGeneralParams;

            SAPbobsCOM.Documents m_oCotizacion;

            int m_intDocEntry = 0;
            string m_strNoOT = string.Empty;

            SAPbouiCOM.Form oForm;
            int m_intposicion = 0;

            Column m_objColumnActividad;
            string m_strDocEntry = string.Empty;
            string m_str_Estado = string.Empty;
            string m_str_NoFase = string.Empty;
            string m_str_CodFase = string.Empty;
            string m_str_IdActtividad = string.Empty;
            string strError;
            int intError;
            double m_dbl_CostoEstandar = 0;

            try
            {
                oForm = applicationSbo.Forms.Item("SCGD_ORDT");
                if (FormularioSBO == null) FormularioSBO = applicationSbo.Forms.Item("SCGD_ORDT");
                m_strNoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("Code", 0).Trim();
                m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                m_intDocEntry = int.Parse(m_strDocEntry);

                m_oCotizacion = CargaObjetoCotizacion(m_intDocEntry);
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strNoOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oChildrenCtrlCol = oGeneralData.Child("SCGD_CTRLCOL");

                if (dtActividades.Rows.Count > 0)
                {
                    ObtieneCostosEstandar(ref dtActividades, p_Colaborador, oForm);

                    for (int i = 0; i <= dtActividades.Rows.Count - 1; i++)
                    {

                        m_str_IdActtividad = dtActividades.GetValue("idac", i).ToString().Trim();

                        if (string.IsNullOrEmpty(m_str_IdActtividad) == false)
                        {
                            m_str_Estado = dtActividades.GetValue("esta", i).ToString().Trim();

                            //Si seleccionó una fase previamente, asigna la fase seleccionada, de lo contrario tomará el del dato maestro del artículo
                            //if (string.IsNullOrEmpty(p_Fase) == false)
                            //{
                                //Consulto el nombre de fase de producción
                            //    m_str_NoFase = DMS_Connector.Helpers.EjecutarConsulta(string.Format(" SELECT \"Name\" FROM \"@SCGD_FASEPRODUCCION\" WITH (NOLOCK) WHERE \"Code\" = {0}", p_Fase));
                            //    m_str_CodFase = p_Fase;
                            //}
                            //else
                            //{
                                m_str_NoFase = dtActividades.GetValue("nofa", i).ToString().Trim();
                                m_str_CodFase = dtActividades.GetValue("cfas", i).ToString().Trim();
                            //}

                            m_dbl_CostoEstandar = Convert.ToDouble(dtActividades.GetValue("cose", i), n);

                            AgregaActividadControlColaborador(ref oChildrenCtrlCol, m_str_IdActtividad, m_str_Estado, m_str_NoFase, m_str_CodFase, m_dbl_CostoEstandar, p_Colaborador, false);
                            g_dtAdicionalesColaborador.Rows.Add(1);
                            g_dtAdicionalesColaborador.SetValue("IdAct", m_intposicion, m_str_IdActtividad);
                            g_dtAdicionalesColaborador.SetValue("IdCol", m_intposicion, p_Colaborador);

                            m_intposicion = g_dtAdicionalesColaborador.Rows.Count;

                        }
                    }
                    if (g_dtAdicionalesColaborador.Rows.Count > 0)
                    {
                        ActualizaLineasServiciosAsiganados(ref m_oCotizacion);
                    }

                    CompanySBO.StartTransaction();
                    if (m_oCotizacion.Update() == 0)
                    {
                        oGeneralService.Update(oGeneralData);
                        if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                    else
                    {
                        CompanySBO.GetLastError(out intError, out strError);
                        throw new Exception(string.Format("{0}: {1}", intError, strError));
                    }

                    recargarActividades(m_strNoOT, applicationSbo);
                    m_objColumnActividad = g_objMatrix.Columns.Item("Col_IdAct");
                    Utilitarios.CargaComboBox(
                        string.Format(" Select U_SCGD_ID as Code, Dscription as Name  from QUT1 with(nolock) where docentry = {0} and U_SCGD_TipArt = '2' ",
                            m_strDocEntry), "Code", "Name", g_dtConsultaCombos, ref m_objColumnActividad);

                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                ApplicationSBO.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
            }

        }

        private void AgregaActividadControlColaborador(ref GeneralDataCollection oChildrenCtrlCol, string m_str_IdActtividad, string m_str_Estado, string m_str_NoFase, string m_str_CodFase, double m_dbl_CostoEstandar, string p_Colaborador, bool p_blnIniciada)
        {
            SAPbobsCOM.GeneralData oChildCC;
            string strHora;
            string strMinutos;

            try
            {
                int intTamano = 0;
                string strIdAct = string.Empty;
                intTamano = oChildrenCtrlCol.Count;

                if (intTamano == 1)
                {
                    oChildCC = oChildrenCtrlCol.Item(0);
                    strIdAct = oChildCC.GetProperty("U_IdAct").ToString().Trim();
                    if (!string.IsNullOrEmpty(strIdAct))
                    {
                        oChildCC = oChildrenCtrlCol.Add();
                    }
                }
                else
                {
                    oChildCC = oChildrenCtrlCol.Add();
                }

                oChildCC.SetProperty("U_Colab", p_Colaborador);
                oChildCC.SetProperty("U_Estad", m_str_Estado);
                oChildCC.SetProperty("U_NoFas", m_str_NoFase);
                oChildCC.SetProperty("U_CodFas", m_str_CodFase);
                oChildCC.SetProperty("U_IdAct", m_str_IdActtividad);
                oChildCC.SetProperty("U_CosEst", m_dbl_CostoEstandar);
                strHora = DateTime.Now.Hour.ToString();
                if (strHora.Length == 1) strHora = string.Format("0{0}", strHora);
                strMinutos = DateTime.Now.Minute.ToString();
                if (strMinutos.Length == 1) strMinutos = string.Format("0{0}", strMinutos);
                strHora = string.Format("{0}:{1}", strHora, strMinutos);
                oChildCC.SetProperty("U_FechPro", DateTime.Now);
                oChildCC.SetProperty("U_HoraIni", strHora);
                if (p_blnIniciada)
                {
                    oChildCC.SetProperty("U_DFIni", DateTime.Now);
                    oChildCC.SetProperty("U_HFIni", DateTime.Now);
                }

            }
            catch (Exception ex)
            {
                throw; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void AgregaAdicional(
            Form oForm, ref DataTable p_dtDataSource,
            string p_strCodigo, string p_strDescripcion, string p_strBodega, string p_strPrecio, string p_strMoneda, string p_strCantidad, string p_strEstado, string p_strDuracion, string p_strNoFase,
            string p_strColCodigo, string p_strColDescripcion, string p_strColBodega, string p_strColPrecio, string p_strColMoneda, string p_strColCantidad, string p_strColEstado, string p_strColDuracion, string p_strColNoFase,
            string p_strColPermanente, string p_strColAdicional, int p_intTipoFormulario)
        {
            try
            {
                int intTamano = 0;
                decimal m_decPrecio;
                string m_strValidacion = string.Empty;

                //n = DIHelper.GetNumberFormatInfo(CompanySBO);

                intTamano = p_dtDataSource.Rows.Count - 1;

                if (string.IsNullOrEmpty(p_strPrecio))
                {
                    m_decPrecio = 0;
                }
                else
                {
                    m_decPrecio = decimal.Parse(p_strPrecio);
                }

                m_strValidacion = p_dtDataSource.GetValue(p_strColCodigo, 0).ToString();
                if (string.IsNullOrEmpty(m_strValidacion) == false || intTamano > 0)
                {
                    intTamano += 1;
                    p_dtDataSource.Rows.Add(1);
                }

                switch (p_intTipoFormulario)
                {
                    case (int)TipoAdicional.Repuesto:
                        p_dtDataSource.SetValue(p_strColPermanente, intTamano, "N");
                        p_dtDataSource.SetValue(p_strColCodigo, intTamano, p_strCodigo);
                        p_dtDataSource.SetValue(p_strColDescripcion, intTamano, p_strDescripcion);
                        p_dtDataSource.SetValue(p_strColBodega, intTamano, p_strBodega);
                        p_dtDataSource.SetValue(p_strColPrecio, intTamano, m_decPrecio.ToString(n));
                        p_dtDataSource.SetValue(p_strColMoneda, intTamano, p_strMoneda);
                        p_dtDataSource.SetValue(p_strColCantidad, intTamano, double.Parse(p_strCantidad));
                        p_dtDataSource.SetValue(p_strColAdicional, intTamano, "Y");

                        break;

                    case (int)TipoAdicional.Suministro:
                        p_dtDataSource.SetValue(p_strColPermanente, intTamano, "N");
                        p_dtDataSource.SetValue(p_strColCodigo, intTamano, p_strCodigo);
                        p_dtDataSource.SetValue(p_strColDescripcion, intTamano, p_strDescripcion);
                        p_dtDataSource.SetValue(p_strColBodega, intTamano, p_strBodega);
                        p_dtDataSource.SetValue(p_strColPrecio, intTamano, m_decPrecio.ToString(n));
                        p_dtDataSource.SetValue(p_strColMoneda, intTamano, p_strMoneda);
                        p_dtDataSource.SetValue(p_strColCantidad, intTamano, double.Parse(p_strCantidad));
                        p_dtDataSource.SetValue(p_strColAdicional, intTamano, "Y");

                        break;
                    case (int)TipoAdicional.Servicio:
                        p_dtDataSource.SetValue(p_strColPermanente, intTamano, "N");
                        p_dtDataSource.SetValue(p_strColCodigo, intTamano, p_strCodigo);
                        p_dtDataSource.SetValue(p_strColDescripcion, intTamano, p_strDescripcion);
                        p_dtDataSource.SetValue(p_strColCantidad, intTamano, double.Parse(p_strCantidad));
                        p_dtDataSource.SetValue(p_strColPrecio, intTamano, m_decPrecio.ToString(n));
                        p_dtDataSource.SetValue(p_strColMoneda, intTamano, p_strMoneda);
                        p_dtDataSource.SetValue(p_strColEstado, intTamano, p_strEstado);
                        p_dtDataSource.SetValue(p_strColDuracion, intTamano, p_strDuracion);
                        p_dtDataSource.SetValue(p_strColNoFase, intTamano, p_strNoFase);
                        p_dtDataSource.SetValue(p_strColAdicional, intTamano, "Y");

                        break;
                    case (int)TipoAdicional.ServicioExterno:

                        p_dtDataSource.SetValue(p_strColPermanente, intTamano, "N");
                        p_dtDataSource.SetValue(p_strColCodigo, intTamano, p_strCodigo);
                        p_dtDataSource.SetValue(p_strColDescripcion, intTamano, p_strDescripcion);
                        p_dtDataSource.SetValue(p_strColCantidad, intTamano, double.Parse(p_strCantidad));
                        p_dtDataSource.SetValue(p_strColPrecio, intTamano, m_decPrecio.ToString(n));
                        p_dtDataSource.SetValue(p_strColMoneda, intTamano, p_strMoneda);
                        p_dtDataSource.SetValue(p_strColAdicional, intTamano, "Y");

                        break;
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ObtieneCostosEstandar(ref DataTable dtActividades, string p_Colaborador, Form oForm)
        {
            string strConsulta = " select U_SCGD_sALXHORA as sal from OHEM where empID IN ({0}) ";
            DataTable dtEmpleados;

            dtEmpleados = oForm.DataSources.DataTables.Item("tEmplea");

            strConsulta = string.Format(strConsulta, p_Colaborador);
            dtEmpleados.ExecuteQuery(strConsulta);

            for (int i = 0; i <= dtActividades.Rows.Count - 1; i++)
            {
                double dblTiempoEstandar = 0;
                double dblSalario = 0;
                double dblCostoActividad = 0;

                dblTiempoEstandar = double.Parse(dtActividades.GetValue("dura", i).ToString());
                dblSalario = double.Parse(dtEmpleados.GetValue("sal", 0).ToString());

                dblCostoActividad = (dblTiempoEstandar / 60) * dblSalario;

                dtActividades.SetValue("cose", i, dblCostoActividad.ToString(n));

            }
        }
        private void PrepararReporteDetalleActividadesOT()
        {
            var noOT = string.Empty;
            noOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
            String direccion = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes;

            if (direccion.EndsWith(@"\\"))
            {
                direccion = direccion.Substring(0, direccion.Length - 2);
            }
            if (direccion.EndsWith(@"\"))
            {
                direccion = direccion.Substring(0, direccion.Length - 1);
            }

            string direccionR = direccion + "\\" + Resource.rptDetalleActividadesOT;
            ImprimirReporte(CompanySBO, direccionR, "Detalle Actividades OT", noOT, DMS_Connector.Company.CompanySBO.DbUserName, PasswordBD, CompanySBO.CompanyDB, CompanySBO.Server);
        }

        private void PrepararReporteGeneralActividadesOT()
        {
            var noOT = string.Empty;
            noOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
            String direccion = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes;

            if (direccion.EndsWith(@"\\"))
            {
                direccion = direccion.Substring(0, direccion.Length - 2);
            }
            if (direccion.EndsWith(@"\"))
            {
                direccion = direccion.Substring(0, direccion.Length - 1);
            }

            string direccionR = direccion + "\\" + Resource.rptGeneralActividadesOT;
            ImprimirReporte(CompanySBO, direccionR, "Reporte General Actividades OT", noOT, DMS_Connector.Company.CompanySBO.DbUserName, PasswordBD, CompanySBO.CompanyDB, CompanySBO.Server);
        }

        /*
         * Imprime el reporte indicado (detalle de actividades de OT o general de actividades de la OT)
         */
        public static void ImprimirReporte(SAPbobsCOM.ICompany company, string direccionReporte, string barraTitulo, string parametros, string usuarioBD, string contraseñaBD, string BD, string servidor)
        {
            string pathExe;
            string parametrosExe;

            if (string.IsNullOrEmpty(barraTitulo))
            {
                barraTitulo = Resource.rptDetalleActividadesOT;
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

        private Boolean ValidacionesInterfazFord()
        {
            string socioNegTip;
            string strDocEntry;

            try
            {
                strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                socioNegTip = Utilitarios.DevuelveValorSN(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_CodCli", 0).Trim(), "U_SCGD_CusType");

                if (string.IsNullOrEmpty(socioNegTip))
                {
                    ApplicationSBO.StatusBar.SetText(Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    return false;
                }

                var query = string.Format(g_strConsultaValidacionInterfazFord, strDocEntry);
                g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                g_dtConsulta.ExecuteQuery(query);

                if (string.IsNullOrEmpty(g_dtConsulta.GetValue("U_SCGD_ServDpto", 0).ToString().Trim()) && (string.IsNullOrEmpty(g_dtConsulta.GetValue("U_SCGD_TipoPago", 0).ToString().Trim())))
                {
                    ApplicationSBO.StatusBar.SetText(Resource.TXTValidaTipoPagoDptoServ, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public Boolean OcultarCamposFechaSuspencio()
        {
            string m_strIDSucursal = string.Empty;

            try
            {
                m_strIDSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();

                if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == m_strIDSucursal).U_FinalizaAct2Click.Trim().Equals("N"))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        

    }
}
