using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DMSOneFramework.SCGCommon;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class SolicitudEspecificos : IFormularioSBO, IUsaMenu
    {
        #region ...Constructor...

        public SolicitudEspecificos(IApplication applicationSBO, SAPbobsCOM.ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                SBOCompany = (SAPbobsCOM.Company)companySBO;
                n = DIHelper.GetNumberFormatInfo(companySBO);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        #endregion

        #region ...Eventos...

        public void ManejadorEventoFormDataLoad(ref bool BubbleEvent)
        {
            string numOT;
            SAPbobsCOM.EmployeesInfo employee;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbouiCOM.Matrix mtxLines;
            try
            {
                FormularioSBO.Freeze(true);
                numOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_NumeroOT", 0).Trim();
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", numOT);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                g_oEditAsesorCode = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtOwCode").Specific;
                g_oEditAsesor = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtAsesor").Specific;
                g_oEditCliOTCode = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtCLiCode").Specific;
                g_oEditCliOT = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtCli").Specific;
                g_oEditPlaca = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtPlaca").Specific;
                g_oEditMarca = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtMarca").Specific;
                g_oEditEstilo = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtEstilo").Specific;
                g_oEditVIN = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtVIN").Specific;
                g_oEditNoUnid = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtUnid").Specific;
                g_oEditAno = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtAno").Specific;
                g_oEditNoVisita = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtVisi").Specific;
                g_oEditTipoOT = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtTipoOT").Specific;
                g_oEditComments = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtObs").Specific;
                g_oStaticCurr = (SAPbouiCOM.StaticText)FormularioSBO.Items.Item("stMoneda").Specific;

                CotNum = oGeneralData.GetProperty("U_DocEntry").ToString().Trim();
                g_oEditAsesorCode.Value = oGeneralData.GetProperty("U_Ase").ToString().Trim();
                g_oEditCliOTCode.Value = oGeneralData.GetProperty("U_CodCOT").ToString().Trim();
                g_oEditCliOT.Value = oGeneralData.GetProperty("U_NCliOT").ToString().Trim();
                g_oEditPlaca.Value = oGeneralData.GetProperty("U_Plac").ToString().Trim();
                g_oEditMarca.Value = oGeneralData.GetProperty("U_Marc").ToString().Trim();
                g_oEditEstilo.Value = oGeneralData.GetProperty("U_Esti").ToString().Trim();
                g_oEditVIN.Value = oGeneralData.GetProperty("U_VIN").ToString().Trim();
                g_oEditNoUnid.Value = oGeneralData.GetProperty("U_NoUni").ToString().Trim();
                g_oEditAno.Value = oGeneralData.GetProperty("U_Ano").ToString().Trim();
                g_oEditNoVisita.Value = oGeneralData.GetProperty("U_NoVis").ToString().Trim();

                dtQuery.ExecuteQuery(string.Format(g_strConsultaTipoOT, oGeneralData.GetProperty("U_TipOT").ToString().Trim()));
                g_oEditTipoOT.Value = dtQuery.GetValue(0, 0).ToString().Trim();

                g_oEditComments.Value = oGeneralData.GetProperty("U_Obse").ToString().Trim();
                g_oStaticCurr.Caption = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_Moneda", 0).Trim();

                employee = (SAPbobsCOM.EmployeesInfo)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo);
                if (employee.GetByKey(Convert.ToInt32(g_oEditAsesorCode.Value)))
                {
                    g_oEditAsesor.Value = string.IsNullOrEmpty(employee.MiddleName) ? string.Format("{0} {1}", employee.FirstName, employee.LastName) : string.Format("{0} {1} {2}", employee.FirstName, employee.MiddleName, employee.LastName);
                }

                string m_strConsulta = string.Format(g_strConsultaConfSucursal, oGeneralData.GetProperty("U_Sucu").ToString().Trim());
                g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                g_dtConfSucursal.ExecuteQuery(m_strConsulta);

                m_strConsulta = string.Format(g_strConsultaAprobacion, numOT, oGeneralData.GetProperty("U_Sucu").ToString().Trim());
                g_dtAprobacion = FormularioSBO.DataSources.DataTables.Item(g_strdtAprobacion);
                g_dtAprobacion.ExecuteQuery(m_strConsulta);

                for (int index = 0; index <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").Size - 1; index++)
                {
                    dtSolicitud.Rows.Add();
                    dtSolicitud.SetValue("Linea", index, index);
                }

                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                mtxLines = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxLines").Specific;

                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("Status", 0).Trim() == "C" ||
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("Canceled", 0).Trim() == "Y")
                {
                    FormularioSBO.Items.Item("btnOK").Enabled = false;
                    FormularioSBO.Items.Item("btnCancelS").Enabled = false;
                    mtxLines.Columns.Item("ColICodeE").Editable = false;
                    mtxLines.Columns.Item("ColSiNEx").Editable = false;
                    mtxLines.Columns.Item("ColNuevo").Editable = false;
                    mtxLines.Columns.Item("ConIngPE").Editable = false;
                    mtxLines.Columns.Item("ColTraNul").Editable = false;
                    mtxLines.Columns.Item("ColFreeT").Editable = false;
                    mtxLines.Columns.Item("ColBod").Editable = false;
                }
                else
                {
                    FormularioSBO.Items.Item("btnOK").Enabled = true;
                    FormularioSBO.Items.Item("btnCancelS").Enabled = true;
                    mtxLines.Columns.Item("ColICodeE").Editable = true;
                    mtxLines.Columns.Item("ColSiNEx").Editable = true;
                    mtxLines.Columns.Item("ColNuevo").Editable = true;
                    mtxLines.Columns.Item("ConIngPE").Editable = true;
                    mtxLines.Columns.Item("ColTraNul").Editable = true;
                    mtxLines.Columns.Item("ColFreeT").Editable = true;
                    mtxLines.Columns.Item("ColBod").Editable = true;
                }
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ManejadorEventoItemPress(ref SAPbouiCOM.ItemEvent pval, ref bool BubbleEvent)
        {
            bool updateCot = false;
            string m_strImpuestosSuministros = string.Empty;
            string m_strImpuestosRepuestos = string.Empty;
            string m_strCambiaBodega = string.Empty;
            string m_strAprobacion = string.Empty;
            string strMensaje;
            int intError;

            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralDataCollection m_childs;
            SAPbobsCOM.GeneralData m_childdata;
            SAPbouiCOM.Matrix mtxLines;

            int qtyCloseLines = 0;

            try
            {
                mtxLines = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxLines").Specific;

                if (pval.ItemUID == "btnOK")
                {
                    if (pval.BeforeAction)
                    {
                        FormularioSBO.AutoManaged = true;
                        if (FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            if (!String.IsNullOrEmpty(CotNum))
                            {
                                for (int i = 0; i <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").Size - 1; i++)
                                {
                                    if (!String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_ItmCodeE", i).Trim()) && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_Status", i).Trim() == "O")
                                    {
                                        updateCot = true;
                                        break;
                                    }
                                }
                                if (updateCot)
                                {
                                    oCompanyService = CompanySBO.GetCompanyService();
                                    oGeneralService = oCompanyService.GetGeneralService("SCGD_SolEs");
                                    oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                                    oGeneralParams.SetProperty("DocEntry", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("DocEntry", 0).Trim());
                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                                    m_childs = oGeneralData.Child("SCGD_SOL_ESP_LIN");

                                    SAPbobsCOM.Documents oQuotation;

                                    g_intEstadoCotizacion = (int)CotizacionEstado.Modificada;

                                    oQuotation = (Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);

                                    if (oQuotation.GetByKey(Convert.ToInt32(CotNum)))
                                    {
                                        g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                                        m_strImpuestosSuministros = g_dtConfSucursal.GetValue("U_Imp_Suminis", 0).ToString().Trim();
                                        m_strImpuestosRepuestos = g_dtConfSucursal.GetValue("U_Imp_Repuestos", 0).ToString().Trim();
                                        m_strCambiaBodega = g_dtConfSucursal.GetValue("U_CamBodEsp", 0).ToString().Trim();
                                        g_dtAprobacion = FormularioSBO.DataSources.DataTables.Item(g_strdtAprobacion);
                                        m_strAprobacion = g_dtAprobacion.GetValue("U_ItmAprob", 0).ToString().Trim();

                                        if (m_strAprobacion == "Y")
                                        {
                                            m_strAprobacion = "1";
                                        }
                                        else
                                        {
                                            m_strAprobacion = "3";
                                        }

                                        for (int index = 0; index <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").Size - 1; index++)
                                        {
                                            if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_ItmCodeE", index)) &&
                                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_Status", index).Trim() == "O")
                                            {
                                                m_childdata = m_childs.Item(0);
                                                for (int y = 0; y <= m_childs.Count - 1; y++)
                                                {
                                                    m_childdata = m_childs.Item(y);
                                                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("LineId", index).Trim() == m_childdata.GetProperty("LineId").ToString().Trim())
                                                    {
                                                        break;
                                                    }
                                                }

                                                oQuotation.Lines.Add();
                                                oQuotation.Lines.UnitPrice = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColPrec").Cells.Item(index + 1).Specific).Value, n);
                                                oQuotation.Lines.UnitPrice = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColPrec").Cells.Item(index + 1).Specific).Value, n);
                                                oQuotation.Lines.Quantity = double.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_Cantidad", index).Trim(), n);
                                                oQuotation.Lines.DiscountPercent = Utilitarios.GetItemDiscount((SAPbobsCOM.Company)CompanySBO, oQuotation.CardCode, oQuotation.Lines.ItemCode);
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_NumeroOT", 0).Trim();
                                                oQuotation.Lines.Currency = dtSolicitud.GetValue("U_Moneda", index).ToString();
                                                oQuotation.Lines.ItemCode = dtSolicitud.GetValue("U_ItmCodeE", index).ToString();
                                                oQuotation.Lines.ItemDescription = dtSolicitud.GetValue("U_NombEsp", index).ToString();

                                                switch (Convert.ToInt32(dtSolicitud.GetValue("U_TipoArt", index).ToString()))
                                                {
                                                    case g_strRepuesto:
                                                        oQuotation.Lines.TaxCode = m_strImpuestosRepuestos;
                                                        oQuotation.Lines.VatGroup = m_strImpuestosRepuestos;
                                                        break;
                                                    case g_strSuministro:
                                                        oQuotation.Lines.TaxCode = m_strImpuestosSuministros;
                                                        oQuotation.Lines.VatGroup = m_strImpuestosSuministros;
                                                        break;
                                                }

                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantPendiente).Value = oQuotation.Lines.Quantity;
                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantSolicitada).Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantRecibida).Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantPendienteDevolucion).Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantPendienteTraslado).Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item(g_strColCantPendienteBodega).Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_strAprobacion;
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = string.Format("{0}-{1}-{2}", oQuotation.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim(), oQuotation.Lines.LineNum, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_NumeroOT", 0).Trim());
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oQuotation.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = dtSolicitud.GetValue("U_TipoArt", index).ToString();
                                                oQuotation.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = dtSolicitud.GetValue("U_CCosEsp", index).ToString();
                                                if (m_strCambiaBodega == "1") {oQuotation.Lines.UserFields.Fields.Item("U_SCGD_CamEsp").Value = m_strCambiaBodega;}
                                                oQuotation.Lines.WarehouseCode = dtSolicitud.GetValue("U_BodeEsp", index).ToString();
                                                string tmpbodega = oQuotation.Lines.WarehouseCode.ToString();
                                                oQuotation.Lines.FreeText = ((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColFreeT").Cells.Item(index + 1).Specific).Value.Trim();
                                                
                                                m_childdata.SetProperty("U_Status", "C");
                                                m_childdata.SetProperty("U_TipoArt", dtSolicitud.GetValue("U_TipoArt", index).ToString());
                                                m_childdata.SetProperty("U_CCosEsp", dtSolicitud.GetValue("U_CCosEsp", index).ToString());
                                                m_childdata.SetProperty("U_BodeEsp", dtSolicitud.GetValue("U_CCosEsp", index).ToString());
                                                m_childdata.SetProperty("U_Moneda", dtSolicitud.GetValue("U_Moneda", index).ToString());
                                                m_childdata.SetProperty("U_NombEsp", dtSolicitud.GetValue("U_NombEsp", index).ToString());
                                                m_childdata.SetProperty("U_ItmCodeE", dtSolicitud.GetValue("U_ItmCodeE", index).ToString());

                                                m_childdata.SetProperty("U_PrecAcor", double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColPrec").Cells.Item(index + 1).Specific).Value, n));
                                                m_childdata.SetProperty("U_FechResp", DateTime.Now);
                                                m_childdata.SetProperty("U_HoraResp", DateTime.Now);
                                                m_childdata.SetProperty("U_UserResp", ApplicationSBO.Company.UserName);
                                                m_childdata.SetProperty("U_BodeEsp", dtSolicitud.GetValue("U_BodeEsp", index).ToString());

                                                m_childdata.SetProperty("U_SinExist", ((SAPbouiCOM.CheckBox)mtxLines.Columns.Item("ColSiNEx").Cells.Item(index + 1).Specific).Caption.Trim());
                                                m_childdata.SetProperty("U_Nuevo", ((SAPbouiCOM.CheckBox)mtxLines.Columns.Item("ColNuevo").Cells.Item(index + 1).Specific).Caption.Trim());
                                                m_childdata.SetProperty("U_IngPE", ((SAPbouiCOM.CheckBox)mtxLines.Columns.Item("ConIngPE").Cells.Item(index + 1).Specific).Caption.Trim());
                                                m_childdata.SetProperty("U_TranNul", ((SAPbouiCOM.CheckBox)mtxLines.Columns.Item("ColTraNul").Cells.Item(index + 1).Specific).Caption.Trim());
                                                m_childdata.SetProperty("U_ObsSol", ((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColFreeT").Cells.Item(index + 1).Specific).Value.Trim());
                                            }
                                        }
                                    }

                                    if (ProcesaCotizacion(ref pval, ref oQuotation, ref BubbleEvent))
                                    {
                                        if (CompanySBO.InTransaction && BubbleEvent)
                                        {
                                            for (int i = 0; i <= m_childs.Count - 1; i++)
                                            {
                                                m_childdata = m_childs.Item(i);
                                                if (m_childdata.GetProperty("U_Status").ToString().Trim() == "C")
                                                {
                                                    qtyCloseLines += 1;
                                                }
                                            }

                                            oGeneralData.SetProperty("U_PrecTot", double.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_PrecTot", 0).Trim(), n));

                                            if (m_childs.Count == qtyCloseLines)
                                            {
                                                oGeneralData.SetProperty("U_FechResp", DateTime.Now);
                                                oGeneralData.SetProperty("U_HoraResp", DateTime.Now);
                                                oGeneralData.SetProperty("U_UserResp", ApplicationSBO.Company.UserName);

                                                oGeneralData.SetProperty("U_Estado", ((int)EstadosSolicitudEspecíficos.Respondido).ToString());

                                                oGeneralService.Update(oGeneralData);
                                                CompanySBO.GetLastError(out intError, out strMensaje);
                                                if (intError != 0)
                                                {
                                                    ApplicationSBO.SetStatusBarMessage(string.Format("{0}: {1}", intError, strMensaje), BoMessageTime.bmt_Short, true);
                                                    if (CompanySBO.InTransaction)
                                                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                    BubbleEvent = false;
                                                }
                                                else
                                                {
                                                    oGeneralService.Close(oGeneralParams);
                                                    if (CompanySBO.InTransaction)
                                                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);

                                                    CargarFormulario(oGeneralData.GetProperty("DocEntry").ToString());
                                                }
                                            }
                                            else
                                            {
                                                oGeneralService.Update(oGeneralData);
                                                CompanySBO.GetLastError(out intError, out strMensaje);
                                                if (intError != 0)
                                                {
                                                    ApplicationSBO.SetStatusBarMessage(string.Format("{0}: {1}", intError, strMensaje), BoMessageTime.bmt_Short, true);
                                                    if (CompanySBO.InTransaction)
                                                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                    BubbleEvent = false;
                                                }
                                                else
                                                {
                                                    if (CompanySBO.InTransaction)
                                                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);

                                                    CargarFormulario(oGeneralData.GetProperty("DocEntry").ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            CompanySBO.GetLastError(out intError, out strMensaje);
                                            if (intError != 0)
                                            {
                                                ApplicationSBO.SetStatusBarMessage(string.Format("{0}: {1}", intError, strMensaje), BoMessageTime.bmt_Short, true);
                                                if (CompanySBO.InTransaction)
                                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                BubbleEvent = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        CompanySBO.GetLastError(out intError, out strMensaje);
                                        if (intError != 0)
                                        {
                                            ApplicationSBO.SetStatusBarMessage(string.Format("{0}: {1}", intError, strMensaje), BoMessageTime.bmt_Short, true);
                                            if (CompanySBO.InTransaction)
                                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            BubbleEvent = false;
                                        }
                                    }
                                }
                                else
                                {
                                    ApplicationSBO.StatusBar.SetText(Resource.SolicitudEspeSinProcesar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                }
                            }
                        }
                    }
                }
                else if (pval.ItemUID == "btnCancelS" && pval.Action_Success)
                {
                    CancelarSolicitudEspecifico();
                }
                else if (pval.ItemUID == "btnPrint" && pval.Action_Success)
                {
                    ImprimirReporte();
                }

            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);

                BubbleEvent = false;
                throw ex;
            }
        }

        public void ManejadorEventoChooseFromList(ref SAPbouiCOM.ItemEvent pval, ref bool BubbleEvent)
        {
            SAPbouiCOM.Matrix omatrix;
            SAPbouiCOM.IChooseFromListEvent oCFLEvento;
            oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pval;
            string sCFL_ID = null;
            sCFL_ID = oCFLEvento.ChooseFromListUID;
            SAPbouiCOM.Form oForm;
            oForm = (SAPbouiCOM.Form)FormularioSBO;
            SAPbouiCOM.ChooseFromList oCFL = oForm.ChooseFromLists.Item(sCFL_ID);

            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;

            try
            {
                omatrix = (SAPbouiCOM.Matrix)(oForm.Items.Item(g_strmtxLineasSol).Specific);

                if (oCFLEvento.ActionSuccess)
                {
                    SAPbouiCOM.DataTable oDataTable = default(SAPbouiCOM.DataTable);
                    oDataTable = oCFLEvento.SelectedObjects;

                    if ((pval.ItemUID == g_strmtxLineasSol) && !(pval.FormMode == (int)BoFormMode.fm_FIND_MODE || pval.FormMode == (int)BoFormMode.fm_VIEW_MODE))
                    {                            
                        if ((oCFLEvento.SelectedObjects != null))
                         {
                            if (pval.ColUID == "ColICodeE")
                            {
                                AsignarValoresEspecifico(ref oDataTable, pval.Row);
                            }
                            if (pval.ColUID == "ColBod")
                            {
                                dtSolicitud.SetValue("U_BodeEsp", pval.Row - 1, oDataTable.GetValue("WhsCode", 0).ToString().Trim());
                            }
                        }

                    }
                }
                else
                {
                    if ((pval.ItemUID == g_strmtxLineasSol) && !(pval.FormMode == (int)BoFormMode.fm_FIND_MODE || pval.FormMode == (int)BoFormMode.fm_VIEW_MODE))
                    {
                        if (pval.ColUID == "ColICodeE")
                         {

                             oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                             oCondition = oConditions.Add();

                             oCondition.BracketOpenNum = 1;
                             oCondition.Alias = "U_SCGD_TipoArticulo";
                             oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                             oCondition.CondVal = ((int)TipoArticulo.Repuesto).ToString();
                             oCondition.BracketCloseNum = 1;
                             oCondition.Relationship = BoConditionRelationship.cr_AND;

                             oCondition = oConditions.Add();

                             oCondition.BracketOpenNum = 1;
                             oCondition.Alias = "validFor";
                             oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                             oCondition.CondVal = "Y";
                             oCondition.BracketCloseNum = 1;

                             oCFL.SetConditions(oConditions);

                             omatrix.FlushToDataSource();
                            
                         }
                        if (pval.ColUID == "ColBod")
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        #endregion

        #region ...Metodos...

        private void CancelarSolicitudEspecifico()
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            try
            {
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_SolEs");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("DocEntry", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("DocEntry", 0).Trim());

                if (!CompanySBO.InTransaction)
                    CompanySBO.StartTransaction();
                oGeneralService.Cancel(oGeneralParams);

                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);

                CargarFormulario(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("DocEntry", 0).Trim());
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                throw ex;
            }
        }

        private void AsignarValoresEspecifico(ref SAPbouiCOM.DataTable p_dtValues, int p_rowNum)
        {
            string numOT;
            string m_strUsaListaPrecCliente;
            string strCodListPrecio;
            string strDocEntry;
            string g_strUsaConsultaSegunConf;
            string m_strMonedaDocumento;
            string m_strMonedaItemAnt;
            var strItmCurr = String.Empty;
            var strItmCode = string.Empty;
            var strItmName = string.Empty;
            var strItmPrec = string.Empty;
            var strBodega = string.Empty;
            var strTipArt = string.Empty;
            var strCCosEsp = string.Empty;

            string monedaSis;
            string monedaLoc;
            string monedaItm;
            double precioLineaAnt = 0;
            double precioLinea = 0;
            double precioTot = 0;
            double tipoCambio = 0;
            double quantity = 0;
            double precioLineaDec;
            double precioLineaAntDec = 0;

            try
            {
                monedaSis = Utilitarios.RetornarMonedaSistema(ref SBOCompany);
                monedaLoc = Utilitarios.RetornarMonedaLocal(ref SBOCompany);

                numOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_NumeroOT", 0).Trim();
                m_strMonedaDocumento = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_Moneda", 0).Trim();
                dtQueryConf = FormularioSBO.DataSources.DataTables.Item(strDtQueryConf);
                dtQuery = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
                dtSolicitud = FormularioSBO.DataSources.DataTables.Item(strDtSolicitud);

                string m_strConsulta = String.Format("Select DocEntry,U_CodLisPre,U_UseLisPreCli from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs=(Select U_SCGD_idSucursal from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}') ", numOT);
                dtQueryConf.ExecuteQuery(m_strConsulta);
                strDocEntry = dtQueryConf.GetValue("DocEntry", 0).ToString().Trim();
                m_strUsaListaPrecCliente = dtQueryConf.GetValue("U_UseLisPreCli", 0).ToString().Trim();

                if (m_strUsaListaPrecCliente.Equals("Y"))
                {
                    dtQuery.ExecuteQuery(string.Format(m_strConsultaListaPreciosCliente, g_oEditCliOTCode.Value));
                    strCodListPrecio = dtQuery.GetValue("ListNum", 0).ToString();
                }
                else
                {
                    strCodListPrecio = dtQueryConf.GetValue("U_CodLisPre", 0).ToString();
                }

                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArti, strCodListPrecio, strDocEntry, p_dtValues.GetValue("ItemCode", 0).ToString().Trim());
                dtQuery.Rows.Clear();
                dtQuery.ExecuteQuery(g_strUsaConsultaSegunConf);

                strItmPrec = dtQuery.GetValue("prec", 0).ToString().Trim();
                strItmCurr = dtQuery.GetValue("mone", 0).ToString().Trim();
                strBodega = dtQuery.GetValue("bode", 0).ToString().Trim();
                strTipArt = dtQuery.GetValue("tiar", 0).ToString().Trim();
                strItmCode = p_dtValues.GetValue("ItemCode", 0).ToString().Trim();
                strItmName = p_dtValues.GetValue("ItemName", 0).ToString().Trim();
                strCCosEsp = dtQuery.GetValue("ccos", 0).ToString().Trim();

                dtSolicitud.SetValue("U_PrecAcor", p_rowNum - 1, double.Parse(strItmPrec, n));
                dtSolicitud.SetValue("U_Moneda", p_rowNum - 1, strItmCurr);
                dtSolicitud.SetValue("U_BodeEsp", p_rowNum - 1, strBodega);
                dtSolicitud.SetValue("U_TipoArt", p_rowNum - 1, strTipArt);
                dtSolicitud.SetValue("U_ItmCodeE", p_rowNum - 1, strItmCode);
                dtSolicitud.SetValue("U_NombEsp", p_rowNum - 1, strItmName);
                dtSolicitud.SetValue("U_CCosEsp", p_rowNum - 1, strCCosEsp);

                if (String.IsNullOrEmpty(strBodega) || String.IsNullOrEmpty(strTipArt))
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ElItem + " " + strItmCode + " " + Resource.ItemMalConfig, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning);
                    return;
                }

                quantity = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_Cantidad", p_rowNum - 1).Trim(), n);
                precioLineaAnt = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_PrecAcor", p_rowNum - 1).Trim(), n);
                m_strMonedaItemAnt = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").GetValue("U_Moneda", p_rowNum - 1).Trim();
                precioTot = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_PrecTot", 0).Trim(), n);
                precioLinea = Convert.ToDouble(strItmPrec);

                if (!string.IsNullOrEmpty(strItmCurr))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_Moneda", p_rowNum - 1, strItmCurr);
                //dtSolicitud.SetValue("U_Moneda", p_rowNum, strItmCurr);
                if (!string.IsNullOrEmpty(strItmPrec))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_PrecAcor", p_rowNum - 1, precioLinea.ToString(n));
                //dtSolicitud.SetValue("U_PrecAcor", p_rowNum, precioLinea.ToString(n));
                if (!string.IsNullOrEmpty(strItmName))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_NombEsp", p_rowNum - 1, strItmName);
                //dtSolicitud.SetValue("U_NombEsp", p_rowNum, strItmName);
                if (!string.IsNullOrEmpty(strItmCode))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_ItmCodeE", p_rowNum - 1, strItmCode);
                //dtSolicitud.SetValue("U_ItmCodeE", p_rowNum, strItmCode);
                if (!string.IsNullOrEmpty(strBodega))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_BodeEsp", p_rowNum - 1, strBodega);
                //dtSolicitud.SetValue("U_BodeEsp", p_rowNum, strBodega);
                if (!string.IsNullOrEmpty(strTipArt))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_TipoArt", p_rowNum - 1, strTipArt);
                //dtSolicitud.SetValue("U_TipoArt", p_rowNum, strTipArt);
                if (!string.IsNullOrEmpty(strCCosEsp))
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").SetValue("U_CCosEsp", p_rowNum - 1, strCCosEsp);
                //dtSolicitud.SetValue("U_CCosEsp", p_rowNum, strCCosEsp);

                if (string.IsNullOrEmpty(strItmCurr))
                {
                    strItmCurr = monedaLoc;
                }

                monedaItm = strItmCurr;

                tipoCambio = Utilitarios.RetornarTipoCambioMonedaRS(monedaSis, monedaItm, DateTime.Now, SBOCompany);
                precioLineaDec = Utilitarios.ManejoMultimoneda(precioLinea, monedaLoc, monedaSis, monedaItm, m_strMonedaDocumento, tipoCambio, DateTime.Now, n, SBOCompany);
                precioLineaAntDec = Utilitarios.ManejoMultimoneda(precioLineaAnt, monedaLoc, monedaSis, m_strMonedaItemAnt, m_strMonedaDocumento, tipoCambio, DateTime.Now, n, SBOCompany);
                precioLinea = Convert.ToDouble(precioLineaDec);
                precioLineaAnt = Convert.ToDouble(precioLineaAntDec);

                var precTotLin = quantity * precioLinea;
                var precTotLinAnt = quantity * precioLineaAnt;

                //Calcula el TOTAL DE TOTALES
                precioTot = precioTot + precTotLin - precTotLinAnt;

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").SetValue("U_PrecTot", 0, precioTot.ToString(n));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RecalcularTotal(ref SAPbouiCOM.ItemEvent pVal)
        {
            double cantidad = 0;
            double precioLinea = 0;
            double precioTot = 0;
            double tipoCambio = 0;
            SAPbouiCOM.Matrix mtxLines;
            string monedaSis;
            string monedaLoc;
            string monedaItm;
            double precioLineaDec = 0;
            string m_strMonedaDocumento;
            try
            {
                monedaSis = Utilitarios.RetornarMonedaSistema(ref SBOCompany);
                monedaLoc = Utilitarios.RetornarMonedaLocal(ref SBOCompany);
                mtxLines = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxLines").Specific;

                m_strMonedaDocumento = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("U_Moneda", 0).Trim();

                for (int i = 1; i <= mtxLines.RowCount; i++)
                {
                    monedaItm = ((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColCurr").Cells.Item(i).Specific).Value.Trim();
                    cantidad = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColQty").Cells.Item(i).Specific).Value, n);
                    precioLinea = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("ColPrec").Cells.Item(i).Specific).Value, n);

                    if (string.IsNullOrEmpty(monedaItm))
                        monedaItm = monedaLoc;

                    tipoCambio = Utilitarios.RetornarTipoCambioMonedaRS(monedaSis, monedaItm, DateTime.Now, SBOCompany);
                    precioLineaDec = Utilitarios.ManejoMultimoneda(precioLinea, monedaLoc, monedaSis, monedaItm, m_strMonedaDocumento, tipoCambio, DateTime.Now, n, SBOCompany);
                    precioLinea = Convert.ToDouble(precioLineaDec);

                    precioLinea = cantidad * precioLinea;
                    precioTot += precioLinea;
                }

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").SetValue("U_PrecTot", 0, precioTot.ToString(n));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ProcesaCotizacion(ref ItemEvent pval, ref SAPbobsCOM.Documents p_oCotizacion, ref bool BoobleEvent)
        {
            bool result = false;
            TransferenciasStock m_objTransferencia;
            SAPbouiCOM.DataTable m_dtConfigSucursal;
            string m_strGeneraOT = string.Empty;

            bool blnEsCliente = false;
            bool m_blnDraft;
            string m_strDraft = string.Empty;
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
            string m_strMensajeError = string.Empty;
            int m_intError;

            try
            {
                m_objTransferencia = new TransferenciasStock((Application)ApplicationSBO, CompanySBO);

                m_intDocEntry = Convert.ToInt32(CotNum);

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

                m_strGeneraOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString().Trim();
                m_strNoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                m_strPlaca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim();
                m_strVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim();
                m_strDescMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim();
                m_strDescEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim();
                m_strDescModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString().Trim();
                
                if (Utilitarios.IsNumeric(p_oCotizacion.DocumentsOwner.ToString().Trim()))
                {
                    m_strAsesor = p_oCotizacion.DocumentsOwner.ToString().Trim();
                }
                else
                {
                    m_strAsesor = "";
                }
                m_strCodigoCliente = p_oCotizacion.UserFields.Fields.Item("CardCode").Value.ToString().Trim();

                g_intEstadoCotizacion = (int)CotizacionEstado.SinCambio;
                if (m_strGeneraOT == "1")
                {
                    ProcesaLineasCotizacion(ref p_oCotizacion, p_oCotizacion, ref m_strBodegaRepuestos, ref m_strBodegaSuministros, ref m_strBodegaServExt, ref m_strBodegaProceso);
                    ApplicationSBO.StatusBar.SetText(Resource.FinalizandoOperaciones, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                    m_objTransferencia.CrearTrasladoAddOnNuevo(ref g_listRepuestos, ref g_listSuministros, ref g_listServiciosExternos, ref g_listEliminarRepuestos, ref g_listEliminarSuministros, m_intDocEntry, m_strNoOT, m_strBodegaRepuestos, m_strBodegaSuministros, m_strBodegaServExt, m_strBodegaProceso, m_strIDTransferencia, true, ref m_strDocEntriesTransferenciasREP, ref m_strDocEntriesTransferenciasSUM, m_strDescMarca, m_strDescEstilo, m_strDescModelo, m_strPlaca, m_strVIN, m_strAsesor, m_strCodigoCliente, false, m_blnDraft, p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim());

                    ApplicationSBO.StatusBar.SetText(Resource.ActualizandoCotizacion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    if (!CompanySBO.InTransaction)
                        CompanySBO.StartTransaction();

                    if (p_oCotizacion.Update() != 0)
                    {
                        CompanySBO.GetLastError(out m_intError, out m_strMensajeError);
                        throw new ExceptionsSBO(m_intError, m_strMensajeError);
                    }
                    else
                    {
                        result = true;
                        ApplicationSBO.StatusBar.SetText(Resource.ProcesoFinalizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                    }
                }
            }
            catch (Exception ex)
            {
                BoobleEvent = false;
                throw ex; // Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return result;
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

            string m_strServicosExternosInventariables = string.Empty;
            string m_strDuracionEstandar = string.Empty;
            string m_strBodegaProcesoPorTipo = string.Empty;

            bool m_blnDisminuirCantidad = false;
            bool g_blnProcesarSi = false;
            bool g_blnProcesarNo = false;
            bool g_blnTipoNoAdmitido;
            bool g_blnTiempoStandar;
            bool g_blnLineaEliminada;

            double m_dblCantAdicional = 0;
            int m_intEstadoAprobadoItem_Local;
            bool m_blnEsLineaNueva = false;
            TransferenciasStock g_objTransferenciasStock;
            bool m_blnMensajeDevolverEnviado = false;
            string m_strValidacionTiempoEstandar = string.Empty;
            string m_strTiempoEstandar = string.Empty;
            string m_strConsulta = string.Empty;
            string m_strCodeConsulta = string.Empty;
            #endregion "Variables"

            string strSEInventariable = string.Empty;
            try
            {
                g_objTransferenciasStock = new TransferenciasStock((Application)ApplicationSBO, CompanySBO);

                m_strConsulta = string.Format("Select Code from [@SCGD_PERMISOS_PV] with (nolock) where Code = 'SCGD_RED' and U_Usuario = '{0}'", ApplicationSBO.Company.UserName);
                dtQuery.ExecuteQuery(m_strConsulta);
                m_strCodeConsulta = dtQuery.GetValue(0, 0).ToString().Trim();

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
                    string tmpbodega = p_oCotizacion.Lines.WarehouseCode.ToString();
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

                            if ((m_intTipoArticulo > 0 && m_intTipoArticulo != (int)TipoArticulo.Repuesto) || (m_intTipoArticulo == (int)TipoArticulo.Repuesto && m_intGenerico != 0))
                            {
                                switch (m_intTipoArticulo)
                                {
                                    case (int)TipoArticulo.Paquete:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, true, m_strSucursal, ref m_strCentroCosto, true);
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Repuesto:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, true);
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Servicio:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, false, m_strSucursal, ref m_strCentroCosto, true);
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
                                            m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, true);
                                        }
                                        else
                                        {
                                            m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, true);
                                        }
                                        g_blnTipoNoAdmitido = true;
                                        break;

                                    case (int)TipoArticulo.Suministro:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, true);
                                        g_blnTipoNoAdmitido = true;
                                        break;
                                    case (int)TipoArticulo.OtrosIngresos:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, false);
                                        break;
                                    case (int)TipoArticulo.OtrosGastos_Costos:
                                        m_blnArtBienConfig = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, true, m_strSucursal, ref m_strCentroCosto, false);
                                        g_blnTipoNoAdmitido = true;
                                        break;
                                }

                                if (m_blnArtBienConfig)
                                {
                                    if (m_intTipoArticulo != (int)TipoArticulo.OtrosIngresos && m_intTipoArticulo != (int)TipoArticulo.OtrosGastos_Costos)
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
                                                m_strBodegaRepuestos = p_oCotizacionAnterior.Lines.WarehouseCode; //m_dtBodegasXCentroCosto.GetValue("Repuestos", y).ToString().Trim();
                                                p_strBodegaRepuestos = m_strBodegaRepuestos;

                                                m_strBodegaSuministros = m_dtBodegasXCentroCosto.GetValue("Suministros", y).ToString().Trim();
                                                p_strBodegaSuministros = m_strBodegaSuministros;

                                                m_strBodegaServExt = m_dtBodegasXCentroCosto.GetValue("ServExt", y).ToString().Trim();
                                                p_strBodegaServExternos = m_strBodegaServExt;

                                                if (string.IsNullOrEmpty(m_strBodegaProcesoPorTipo))
                                                {
                                                    m_strBodegaProceso =
                                                    m_dtBodegasXCentroCosto.GetValue("Proceso", y).ToString().Trim();
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

                                        if (m_intTipoArticulo == (int)TipoArticulo.Repuesto ||
                                            m_intTipoArticulo == (int)TipoArticulo.Suministro)
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
                                                if ((m_intEstadoRealTraslado != (int)OrdenTrabajo.ResultadoValidacionPorItem.Comprar && m_intEstadoRealTraslado != (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf &&
                                                    int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim()) == (int)EstadosTraslado.NoProcesado) &&
                                                    (m_intTipoArticulo == (int)TipoArticulo.Repuesto || m_intTipoArticulo == (int)TipoArticulo.Suministro))
                                                {
                                                    if (m_blnDraft && m_intEstadoRealTraslado == (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega)
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.PendienteBodega;
                                                    }
                                                    else
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = m_intEstadoRealTraslado;
                                                    }
                                                }

                                                else if (m_intEstadoRealTraslado == (int)OrdenTrabajo.ResultadoValidacionPorItem.Comprar)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = (int)EstadosTraslado.No;
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = "PARA COMPRAR";
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = "Y";
                                                }

                                                else if (m_intEstadoRealTraslado == (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf && (m_intTipoArticulo == (int)TipoArticulo.Suministro || m_intTipoArticulo == (int)TipoArticulo.Repuesto))
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                                                }

                                                if (p_oCotizacion.Lines.Quantity != m_dblCantidadItem && m_dblCantidadItem != 0)
                                                {
                                                    p_oCotizacion.Lines.Quantity = m_dblCantidadItem;
                                                }

                                                switch (m_intEstadoRealTraslado)
                                                {
                                                    case (int)OrdenTrabajo.ResultadoValidacionPorItem.NoAprobar:
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

                                                    case (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf:
                                                        {
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = m_dblCantidadItem.ToString(n);
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0;
                                                            break;
                                                        }

                                                    case (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega:
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
                                            string m_strEstadoTrasladoNuevaCotizacion = string.Empty;

                                            double m_dblCantAntiguaCotizacion;
                                            string m_strValidaReduceCantidad = string.Empty;

                                            m_dblCantNuevaCotizacion = p_oCotizacion.Lines.Quantity;
                                            m_dblCantAntiguaCotizacion = p_oCotizacionAnterior.Lines.Quantity;
                                            m_strEstadoTrasladoNuevaCotizacion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();

                                            if (m_dblCantAntiguaCotizacion < m_dblCantNuevaCotizacion &&
                                                m_strEstadoTrasladoNuevaCotizacion != OrdenTrabajo.ResultadoValidacionPorItem.SinCambio.ToString().Trim() &&
                                                m_strEstadoTrasladoNuevaCotizacion != OrdenTrabajo.ResultadoValidacionPorItem.PendBodega.ToString().Trim())
                                            {
                                                p_oCotizacion.Lines.Quantity = p_oCotizacionAnterior.Lines.Quantity;
                                            }

                                            else if (m_dblCantAntiguaCotizacion > m_dblCantNuevaCotizacion)
                                            {
                                                m_strValidaReduceCantidad =
                                                    m_dtADMIN.GetValue("U_ReduceCant", 0).ToString().Trim();

                                                if (string.IsNullOrEmpty(m_strValidaReduceCantidad))
                                                {
                                                    m_strValidaReduceCantidad = "N";
                                                }

                                                if (m_strEstadoTrasladoNuevaCotizacion == EstadosTraslado.Si.ToString().Trim())
                                                {
                                                    if (m_strValidaReduceCantidad == "Y")
                                                    {
                                                        if (string.IsNullOrEmpty(m_strCodeConsulta) == false)
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
                                                        if (string.IsNullOrEmpty(m_strCodeConsulta) == false)
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

                                            if ((m_intEstadoTrasladoAct == (int)EstadosTraslado.NoProcesado &&
                                                m_intEstadoRealTraslado != (int)OrdenTrabajo.ResultadoValidacionPorItem.Comprar) ||
                                                m_intEstadoTrasladoAct == (int)EstadosTraslado.PendienteTraslado ||
                                                (m_dblCantAdicional > 0 && m_blnDisminuirCantidad == false))
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
                                                            if (g_intRealizarTraslados == OrdenTrabajo.RealizarTraslado.Si)
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
                                                            else if (g_intRealizarTraslados == OrdenTrabajo.RealizarTraslado.No)
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
                                            else if ((m_blnDraft && m_intEstadoRealTraslado == (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega &&
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
                                                            if (g_intRealizarTraslados == OrdenTrabajo.RealizarTraslado.Si)
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
                                                            else if (g_intRealizarTraslados == OrdenTrabajo.RealizarTraslado.No)
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

                                                if (p_oCotizacionAnterior.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value != p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value)
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
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = OrdenTrabajo.LineaAProcesar.Si;
                                            }
                                        }
                                        else if (g_blnProcesarNo)
                                        {
                                            if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == EstadosAprobacion.NoAprobado.ToString().Trim() &&
                                                p_oCotizacion.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                                            {
                                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = OrdenTrabajo.LineaAProcesar.No;
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
                                        ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ItemMalConfig), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                    }
                                }
                            }
                            else
                            {
                                if (g_blnTipoNoAdmitido)
                                {
                                    ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ItemMalConfig), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                }
                            }
                        }
                        else
                        {
                            ApplicationSBO.StatusBar.SetText(string.Format("{0} {1} {2}", Resource.ElItem, m_strItemCode, Resource.ValTiempoEstandar), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }


                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return false;
        }

        private string DevuelveValorItem(string p_strItemCode, string p_UDF)
        {
            SAPbobsCOM.IItems oItem;
            string m_strValorRetorno = string.Empty;

            try
            {
                oItem = (SAPbobsCOM.IItems)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oItem.GetByKey(p_strItemCode);
                m_strValorRetorno = oItem.UserFields.Fields.Item(p_UDF).Value.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_strValorRetorno;
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

                m_dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);

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
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
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
                m_dtConsultas = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);

                m_oItem = (SAPbobsCOM.Items)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
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
                        m_dtConsultas.ExecuteQuery(
                             String.Format("SELECT WhsCode FROM OITW WHERE ItemCode = '{0}'AND WhsCode = '{1}'",
                                                           p_strItemCode,
                                                           m_strBodegaProcesoCtroCosto));

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
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return true;
        }

        private void RevisaStock(Document_Lines p_oLines, int p_intDocEntry, string p_strBodegaRepuestos, string p_strBodegaSuministros, int p_intTipoArticulo,
                                int p_intGenerico, bool p_blnDraft, ref double p_dblCantidadItem, ref int p_intEstadoTraslado, ref int p_intCantidadItemsPaquete,
                                ref int p_intCantidadItemsTotal, ref int p_intEstadoPaquete, ref bool p_blnRechazarItem, bool p_blnActualizarCantidad, double p_dblCantidadAdicional = 0)
        {
            string m_strEstadoAprobacion = string.Empty;
            string m_strEstadoTraslado = string.Empty;
            int m_intEstadoAprobacion;
            int m_intEstadoTraslado;

            OrdenTrabajo.ResultadoValidacionPorItem m_strValidaciónResultado;
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
                                case OrdenTrabajo.ResultadoValidacionPorItem.NoAprobar:
                                    p_blnRechazarItem = true;
                                    break;

                                case OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion:
                                    if (p_blnDraft)
                                    {
                                        p_intEstadoTraslado = (int)EstadosTraslado.PendienteBodega;
                                    }
                                    else
                                    {
                                        p_intEstadoTraslado = (int)EstadosTraslado.Si;
                                    }
                                    p_dblCantidadItem = m_dblCantidad;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                                    break;

                                case OrdenTrabajo.ResultadoValidacionPorItem.PendTransf:
                                    p_intEstadoTraslado = (int)EstadosTraslado.PendienteTraslado;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                                    p_dblCantidadItem = m_dblCantidad;
                                    break;

                                case OrdenTrabajo.ResultadoValidacionPorItem.Comprar:
                                    if (p_blnActualizarCantidad == false)
                                    {
                                        p_dblCantidadItem = p_oLines.Quantity;
                                    }
                                    else
                                    {
                                        m_dblCantidad = p_dblCantidadAdicional;
                                    }
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.Comprar;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                                    p_oLines.UserFields.Fields.Item("U_SCGD_CPen").Value = p_oLines.Quantity;
                                    p_oLines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y";
                                    break;

                                case OrdenTrabajo.ResultadoValidacionPorItem.SinCambio:
                                    {
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
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
                                        p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
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
                                        p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
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

                            if (m_strValidaciónResultado == OrdenTrabajo.ResultadoValidacionPorItem.PendTransf)
                            {
                                p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                                g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                p_dblCantidadItem = m_dblCantidad;
                            }
                            else
                            {
                                if (p_blnDraft == true)
                                {
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                }
                                else
                                {
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;

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
                                case OrdenTrabajo.ResultadoValidacionPorItem.SinCambio:
                                    if (p_blnDraft)
                                    {
                                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                                    }
                                    else
                                    {
                                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                                        p_dblCantidadItem = p_oLines.Quantity;
                                        p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    }
                                    break;
                                case OrdenTrabajo.ResultadoValidacionPorItem.PendBodega:
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                                    break;
                            }
                        }
                        else if (p_intTipoArticulo == (int)TipoArticulo.Suministro)
                        {
                            m_dblCantidad = p_oLines.Quantity;

                            m_strValidaciónResultado = ValidarCantidadDisponibleSuministros(p_oLines.ItemCode, p_oLines.ItemDescription, m_dblCantidad, p_strBodegaSuministros, p_oLines.LineNum, p_intDocEntry, p_blnDraft);

                            if (m_strValidaciónResultado == OrdenTrabajo.ResultadoValidacionPorItem.PendTransf)
                            {
                                p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                                g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                p_dblCantidadItem = p_oLines.Quantity;
                            }
                            else if (m_strValidaciónResultado == OrdenTrabajo.ResultadoValidacionPorItem.PendBodega)
                            {
                                if (p_blnDraft)
                                {
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                }
                                else
                                {
                                    p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                                    g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                                    p_dblCantidadItem = p_oLines.Quantity;
                                }
                            }
                            else
                            {
                                p_dblCantidadItem = p_oLines.Quantity;
                                p_intEstadoTraslado = (int)OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                                g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
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
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private OrdenTrabajo.ResultadoValidacionPorItem ValidarCantidadDisponibleRepuestos(
            string p_ItemCode,
            string p_ItemDescription,
            int p_LineNum,
            int p_DocEntry,
            ref double p_dblCantidadItem,
            string p_StrBodegaRepuestos,
            bool p_blnActualizarCantidad,
            int p_intEstadoTraslado,
            bool p_blnDraft)
        {
            double m_dblCantidad;
            double m_dblCantidadLineasAnteriores;
            int m_intMsjResult;
            OrdenTrabajo.ResultadoValidacionPorItem m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.SinCambio;
            OrdenTrabajo.ListaCantidadesAnteriores m_objCantidadAnterior = new OrdenTrabajo.ListaCantidadesAnteriores();

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
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.Comprar;
                            break;
                        case 2:
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.NoAprobar;
                            break;
                        case 3:
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                            break;
                    }
                }

                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) < p_dblCantidadItem && p_intEstadoTraslado == (int)EstadosTraslado.NoProcesado)
                {
                    m_intMsjResult = ApplicationSBO.MessageBox(string.Format("{0} {1} {2}", Resource.ElItem, p_ItemCode, Resource.SinInventario), 1, Resource.PendTraslado, Resource.Rechazar, Resource.Trasladar);

                    switch (m_intMsjResult)
                    {
                        case 1:
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                            break;
                        case 2:
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.NoAprobar;
                            break;
                        case 3:
                            m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.ModifCantiCotizacion;
                            p_dblCantidadItem = m_dblCantidad;
                            break;

                    }
                }

                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) <= p_dblCantidadItem && p_intEstadoTraslado == (int)EstadosTraslado.PendienteTraslado)
                {
                    m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.SinCambio;
                }

                else
                {
                    if (p_blnDraft)
                    {
                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                        m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                    }
                    else
                    {
                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                        m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.SinCambio;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
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
                        if (m_objCotizacion.Lines.LineNum <= p_intLineNum && m_objCotizacion.Lines.ItemCode == p_strItemCode)
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
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
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
                m_objItem = (SAPbobsCOM.IItems)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
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
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
            return m_dblStock;
        }

        private OrdenTrabajo.ResultadoValidacionPorItem ValidarCantidadDisponibleSuministros(
            string p_strItemCode,
            string p_strDescription,
            double p_dblCantidad,
            string p_strBodegaSuministros,
            int p_intLineNum,
            int p_intDocEntry,
            bool p_blnDraft)
        {

            double m_dblCantidad;
            double m_dblCantidadLineasAnteriores;
            OrdenTrabajo.ResultadoValidacionPorItem m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.SinCambio;

            try
            {
                m_dblCantidad = DevuelveStockDisponibleXItem(p_strItemCode, p_strBodegaSuministros, CompanySBO);
                m_dblCantidadLineasAnteriores = DevuelveCantidadLineasAnteriores(p_strItemCode, p_intLineNum, p_intDocEntry, CompanySBO);
                if ((m_dblCantidad - m_dblCantidadLineasAnteriores) <= 0)
                {
                    m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                }
                else if ((m_dblCantidad - m_dblCantidadLineasAnteriores) < p_dblCantidad)
                {
                    m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendTransf;
                }
                else
                {
                    if (p_blnDraft == true)
                    {
                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.No;
                        m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.PendBodega;
                    }
                    else
                    {
                        g_intRealizarTraslados = OrdenTrabajo.RealizarTraslado.Si;
                        m_lResultado = OrdenTrabajo.ResultadoValidacionPorItem.SinCambio;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

            return m_lResultado;
        }

        public void ImprimirReporte()
        {
            string strDireccionReporte;
            string strPathExe;
            string strBarraTitulo;
            string strReporte;
            int intReporteAImprimir = 0;
            int intParamReporte = 0;

            SAPbouiCOM.Matrix oMatrix = default(SAPbouiCOM.Matrix);
            string strNoSol = null;

            strNoSol = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").GetValue("DocEntry", 0).Trim();
            string strParametros = string.Empty;
            dtQuery = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
            dtQuery.ExecuteQuery("select U_Reportes from [@SCGD_ADMIN] with (nolock) where Code = 'DMS'");
            if (!string.IsNullOrEmpty(strNoSol))
            {
                strReporte = "";
                intParamReporte = Convert.ToInt32(strNoSol);

                strBarraTitulo = Resource.txtSolicitudEspecificos;

                strBarraTitulo = strBarraTitulo.Replace(" ", "°");

                strReporte = string.Format("{0}\\{1}", dtQuery.GetValue("U_Reportes", 0), Resource.rptSolEsp);
                //strReporte += ".rpt";
                strDireccionReporte = "";
                strDireccionReporte += strReporte;

                strParametros = intParamReporte.ToString();

                strDireccionReporte = strDireccionReporte.Replace(" ", "°");
                strPathExe = System.IO.Directory.GetCurrentDirectory() + "\\SCG Visualizador de Reportes.exe ";

                string parametrosExe = strBarraTitulo + " " + strDireccionReporte + " " + DBUser + "," + DBPassword + "," +
                          CompanySBO.Server + "," + CompanySBO.CompanyDB + " " + strParametros;

                ProcessStartInfo startInfo = new ProcessStartInfo(strPathExe) { WindowStyle = ProcessWindowStyle.Maximized, Arguments = parametrosExe };
                Process.Start(startInfo);
            }
            else
            {
                ApplicationSBO.StatusBar.SetText(Resource.ErrorPrintReport, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning);
            }
        }
        #endregion
    }
}
