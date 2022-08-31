using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using ICompany = SAPbobsCOM.ICompany;
using System.Timers;
using System.Windows.Forms;
using Form = SAPbouiCOM.Form;

namespace SCG.ServicioPostVenta.CreaciónOTEspecial
{
    public partial class OTEspecial
    {
        private static System.Timers.Timer oTimer;

        public OTEspecial(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;

                n = DIHelper.GetNumberFormatInfo(companySBO);

            }
            catch (Exception)
            {
                throw;
            }

        }


        private void CargarTiposOtEspeciales()
        {
            SAPbouiCOM.DataTable dtConsulta;
            try
            {
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDataTableConsulta);
                g_sboItem = FormularioSBO.Items.Item(mc_strTipoOtEspeciales);
                g_sboCombo = (SAPbouiCOM.ComboBox)g_sboItem.Specific;

                string query = " select U_Code as code, U_Name as 'desc' " +
                                " from [@SCGD_CONF_TIP_ORDEN] as lin " +
                                " inner join [@SCGD_CONF_SUCURSAL] as suc " +
                                " on suc.DocEntry = lin.DocEntry " +
                                " where suc.U_Sucurs in " +
                                " (select U_SCGD_idSucursal " +
                                " from OQUT " +
                                " where U_SCGD_Numero_OT = '{0}') ";

                query = string.Format(query, g_strNOOT);

                Utilitarios.CargaComboBox(query, "code", "desc", dtConsulta, ref g_sboCombo, false);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ManejadorEventoFormDataLoad(string p_strSucu)
        {
            SAPbouiCOM.DataTable dtConsulta;
            SAPbouiCOM.DataTable dtLineas;
            SAPbouiCOM.Matrix mtxLineas;

            try
            {
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDataTableConsulta);
                dtLineas = FormularioSBO.DataSources.DataTables.Item(strDataTableLineas);

                string Query1 = String.Format(" SELECT QUT1.U_SCGD_ID FROM QUT1 with (nolock) INNER JOIN OQUT with (nolock) on QUT1.DocEntry = OQUT.DocEntry " +
                                              " WHERE OQUT.U_SCGD_Numero_OT is null and OQUT.U_SCGD_No_Visita in (SELECT U_SCGD_No_Visita FROM OQUT with (nolock) " +
                                              " WHERE oqut.U_SCGD_Numero_OT = '{0}') ", g_strNOOT);

                string Query2 = String.Format(" SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, " +
                                              " QUT1.DiscPrcnt, QUT1.U_SCGD_ID, QUT1.U_SCGD_Costo, QUT1.TaxCode, " +
                       " QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra, OITM.U_SCGD_TipoArticulo, QUT1.U_SCGD_Comprar " +
                                              " FROM QUT1 with (nolock) " +
                                              "INNER JOIN OITM on OITM.ItemCode = QUT1.ItemCode" +
                                              " WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " +
                                              " and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and QUT1.U_SCGD_ID not in ({1}) ", g_strDocE, Query1);
                
                string Query3 = String.Format(" SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, " +
                                             " QUT1.DiscPrcnt, QUT1.U_SCGD_ID, QUT1.U_SCGD_Costo, QUT1.TaxCode, " +
                      " QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra, OITM.U_SCGD_TipoArticulo, QUT1.U_SCGD_Comprar " +
                                             " FROM QUT1 with (nolock) " +
                                             " INNER JOIN OITM on OITM.ItemCode = QUT1.ItemCode" +
                                             " WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " +
                                             " and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and QUT1.U_SCGD_Traslad not in (3, 4) and QUT1.U_SCGD_ID not in ({1}) ", g_strDocE, Query1);

                if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == p_strSucu).U_HjaCanPen.Trim().Equals( "Y"))
                {
                    dtConsulta.ExecuteQuery(Query3);
                }
                else
                {
                    dtConsulta.ExecuteQuery(Query2);
                }
                
                mtxLineas = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(mc_strMatizCotLines).Specific;

                for (int i = 0; i <= dtConsulta.Rows.Count - 1; i++)
                {
                    if (string.IsNullOrEmpty(dtConsulta.GetValue("ItemCode", i).ToString().Trim()) == false)
                    {
                        dtLineas.Rows.Add(1);

                        dtLineas.SetValue("col_Code", i, dtConsulta.GetValue("ItemCode", i));
                        dtLineas.SetValue("col_Name", i, dtConsulta.GetValue("Dscription", i));
                        dtLineas.SetValue("col_Quant", i, dtConsulta.GetValue("Quantity", i));
                        dtLineas.SetValue("col_Curr", i, dtConsulta.GetValue("Currency", i));
                        dtLineas.SetValue("col_Price", i, dtConsulta.GetValue("Price", i));
                        dtLineas.SetValue("col_Obs", i, dtConsulta.GetValue("FreeTxt", i));
                        dtLineas.SetValue("col_DEnt", i, dtConsulta.GetValue("DocEntry", i));
                        dtLineas.SetValue("col_LNum", i, dtConsulta.GetValue("LineNum", i));
                        dtLineas.SetValue("col_PrcDes", i, dtConsulta.GetValue("DiscPrcnt", i));
                        dtLineas.SetValue("col_IDLine", i, dtConsulta.GetValue("U_SCGD_ID", i));
                        dtLineas.SetValue("col_Costo", i, dtConsulta.GetValue("U_SCGD_Costo", i));
                        dtLineas.SetValue("col_IndImp", i, dtConsulta.GetValue("TaxCode", i));
                        dtLineas.SetValue("col_CPend", i, dtConsulta.GetValue("U_SCGD_CPen", i));
                        dtLineas.SetValue("col_CSol", i, dtConsulta.GetValue("U_SCGD_CSol", i));
                        dtLineas.SetValue("col_CRec", i, dtConsulta.GetValue("U_SCGD_CRec", i));
                        dtLineas.SetValue("col_PenDev", i, dtConsulta.GetValue("U_SCGD_CPDe", i));
                        dtLineas.SetValue("col_PenTra", i, dtConsulta.GetValue("U_SCGD_CPTr", i));
                        dtLineas.SetValue("col_PenBod", i, dtConsulta.GetValue("U_SCGD_CPBo", i));
                        dtLineas.SetValue("col_Compra", i, dtConsulta.GetValue("U_SCGD_Compra", i));
                        dtLineas.SetValue("col_TipArt", i, dtConsulta.GetValue("U_SCGD_TipoArticulo", i));
                        dtLineas.SetValue("col_Comprar", i, dtConsulta.GetValue("U_SCGD_Comprar", i));
                    }
                }

                mtxLineas.LoadFromDataSource();

            }
            catch (Exception)
            {
                throw;
            }
        }


        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent);
                    break;
            }
        }

        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.ComboBox oCombo;
            SAPbouiCOM.Item sboItem;

            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnGeSOTE":
                                sboItem = oForm.Items.Item("cboTipOtE");
                                oCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;

                                if (g_SOOTEsp)
                                {
                                    sboItem = oForm.Items.Item("cboTipOtE");
                                    oCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;

                                    if (string.IsNullOrEmpty(oCombo.Value))
                                    {
                                        BubbleEvent = false;
                                        ApplicationSBO.StatusBar.SetText(Resource.ErrorTipoOrden, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                    }
                                }
                                break;
                        }
                    }
                    else if (pVal.ActionSuccess)
                    {
                        InicializarTimer();
                        switch (pVal.ItemUID)
                        {
                            case "btnGeSOTE":
                                List<string> olsIDLinea = new List<string>();
                                string m_strOTPadre = string.Empty;
                                string m_strNoOT = string.Empty;
                                string m_strSucursal = string.Empty;
                                string m_strTipoOT = string.Empty;
                                string m_strCotHija = string.Empty;
                                string m_strComentarios = String.Empty;
                                Boolean m_blnCreaOTEsp = false;
                                Boolean m_blnCreaOT = false;
                                Boolean m_blnCreaSolOTEspecial = false;
                                bool m_blnCancelPadre = true;
                                bool m_AprobOTXSuc = false;

                                sboItem = oForm.Items.Item("cboTipOtE");
                                oCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;

                                if (g_SOOTEsp)
                                {
                                    CompanySBO.StartTransaction();
                                    m_blnCreaSolOTEspecial = CreaSolicitudOTEsp(oForm, pVal, ref BubbleEvent, g_strDocE, ref m_AprobOTXSuc);

                                    if (m_blnCreaSolOTEspecial)
                                    {
                                        if (m_AprobOTXSuc)
                                        {
                                            m_blnCreaOTEsp = CreaOTEsp(oForm, pVal, ref olsIDLinea, ref m_strOTPadre, ref m_strNoOT, ref m_strSucursal, ref m_strTipoOT, ref m_strCotHija, ref m_blnCancelPadre, ref m_strComentarios);
                                            if (m_blnCreaOTEsp)
                                            {
                                                m_blnCreaOT = CreaOT(m_strNoOT, m_strOTPadre, m_strSucursal, m_strTipoOT, m_strCotHija, oForm, m_blnCancelPadre, m_strComentarios);
                                                if (m_blnCreaOT)
                                                {
                                                    ActualizaDocsCompra(olsIDLinea, m_strOTPadre, m_strNoOT, oForm);

                                                    EnviarMensajes(true, oCombo.Selected.Description, m_strNoOT, m_strSucursal, oForm, m_strCotHija);

                                                    oForm.Items.Item("2").Click();
                                                    ApplicationSBO.StatusBar.SetText(Resource.CreacionOTEspecial, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                                    if (m_blnCancelPadre)
                                                    {
                                                        oForm = ApplicationSBO.Forms.ActiveForm;
                                                        oForm.Mode = BoFormMode.fm_FIND_MODE;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ApplicationSBO.StatusBar.SetText(Resource.MsgSolOtEspSuccess, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                            oForm.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    ApplicationSBO.StatusBar.SetText(Resource.ProcesandoOTEspecial, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                    //CompanySBO.StartTransaction();
                                    m_blnCreaOTEsp = CreaOTEsp(oForm, pVal, ref olsIDLinea, ref m_strOTPadre, ref m_strNoOT, ref m_strSucursal, ref m_strTipoOT, ref m_strCotHija, ref m_blnCancelPadre, ref m_strComentarios);
                                    if (m_blnCreaOTEsp)
                                    {
                                        m_blnCreaOT = CreaOT(m_strNoOT, m_strOTPadre, m_strSucursal, m_strTipoOT, m_strCotHija, oForm, m_blnCancelPadre, m_strComentarios);
                                        if (m_blnCreaOT)
                                        {
                                            ActualizaDocsCompra(olsIDLinea, m_strOTPadre, m_strNoOT, oForm);
                                            EnviarMensajes(false, oCombo.Selected.Description, m_strNoOT, m_strSucursal, oForm, m_strCotHija);
                                            oForm.Items.Item("2").Click();
                                            ApplicationSBO.StatusBar.SetText(Resource.CreacionOTEspecial, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                            if (m_blnCancelPadre)
                                            {
                                                oForm = ApplicationSBO.Forms.ActiveForm;
                                                oForm.Mode = BoFormMode.fm_FIND_MODE;
                                            }
                                        }
                                    }
                                }
                                if (CompanySBO.InTransaction)
                                {
                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                                }
                                break;
                            case "mtxOTLines":
                                if (pVal.ColUID == "Col_sel" && pVal.Row > 0)
                                {

                                }
                                break;
                        }
                        DetenerTimer();
                    }
                }
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
            }
        }

        private void ObtieneDescripcionEstado(string p_idEstado, ref string m_strEstadoIniciadoDes, Form oFormOt = null)
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

        private Boolean CreaOTEsp(Form oForm, ItemEvent pVal, ref List<string> p_olsIDLinea, ref string p_strOtPadre, ref string p_strNoOt, ref string p_strSucursal, ref string p_strTipoOt, ref string p_strCotHija, ref bool p_blnCancelaPadre, ref string p_strComentarios)
        {
            SAPbobsCOM.Documents oCotizacionPadre = null;
            SAPbobsCOM.Documents oCotizacionHija;
            int m_intDocEntry = 0;
            string m_strSiguienteVisita = string.Empty;
            string m_strVisita = string.Empty;
            bool blnArticuloPaquete = false;
            string m_strSucursal, m_strOTPadre, m_strTipoOT = string.Empty;
            SAPbouiCOM.Matrix omtx;
            SAPbouiCOM.ComboBox oCombo;
            SAPbouiCOM.Item sboItem;
            List<int> estadosActividades = new List<int>();
            int estado = 0;
            string strCodigoClientePorTipoOrden = string.Empty;
            string strNombreClientePorTipoOrden = string.Empty;
            string strUsaListaPrecios = string.Empty;

            try
            {
                InicializarTimer();
                p_olsIDLinea = new List<string>();
                sboItem = oForm.Items.Item("cboTipOtE");
                oCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                DateTime m_dtFecha = System.DateTime.Today;
                string m_strNoOT = string.Empty;
                omtx = (Matrix)oForm.Items.Item("mtxOTLines").Specific;
                omtx.FlushToDataSource();
                dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas);
                oCotizacionHija = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
                m_intDocEntry = int.Parse(g_strDocE);
                oCotizacionPadre = CargaObjetoCotizacion(m_intDocEntry);
                m_strSucursal = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                p_strSucursal = m_strSucursal;
                m_strVisita = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_No_Visita").Value.ToString().Trim();
                m_strOTPadre = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                p_strOtPadre = m_strOTPadre;
                m_strTipoOT = oCombo.Value.Trim();
                p_strTipoOt = m_strTipoOT;
                m_strSiguienteVisita = RetornaSiguienteOrden(m_strVisita, oForm);
                m_strNoOT = string.Format("{0}-{1}", m_strVisita, m_strSiguienteVisita);
                p_strNoOt = m_strNoOT;
                CardCodePorTipoOrden(ref strCodigoClientePorTipoOrden, ref strNombreClientePorTipoOrden, ref strUsaListaPrecios, p_strSucursal, p_strTipoOt, oForm);
                oCotizacionHija.DocDate = m_dtFecha;
                oCotizacionHija.RequriedDate = m_dtFecha;
                oCotizacionHija.DocumentsOwner = oCotizacionPadre.DocumentsOwner;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = m_strNoOT;

                if (strCodigoClientePorTipoOrden != string.Empty)
                {
                    oCotizacionHija.CardCode = strCodigoClientePorTipoOrden;
                    oCotizacionHija.CardName = strNombreClientePorTipoOrden;
                    oCotizacionHija.UserFields.Fields.Item("U_SCGD_CCliOT").Value = strCodigoClientePorTipoOrden;
                    oCotizacionHija.UserFields.Fields.Item("U_SCGD_NCliOT").Value = strNombreClientePorTipoOrden;
                }
                else
                {
                    oCotizacionHija.CardCode = oCotizacionPadre.CardCode;
                    oCotizacionHija.CardName = oCotizacionPadre.CardName;
                    oCotizacionHija.UserFields.Fields.Item("U_SCGD_CCliOT").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_CCliOT").Value;
                    oCotizacionHija.UserFields.Fields.Item("U_SCGD_NCliOT").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_NCliOT").Value;
                }

                String comentarios = String.Format("{0}, {1} {2}", oCotizacionPadre.Comments, Resource.Sederivadelaorden, m_strOTPadre);
                comentarios = comentarios.Trim().Length <= 250 ? comentarios.Trim() : comentarios.Trim().Substring(0, 250);
                p_strComentarios = comentarios;
                oCotizacionHija.Comments = comentarios;

                if (DMS_Connector.Company.AdminInfo.EnableBranches == SAPbobsCOM.BoYesNoEnum.tYES)
                {
                    if (!string.IsNullOrEmpty(m_strSucursal))
                    {
                        oCotizacionHija.BPL_IDAssignedToInvoice = int.Parse(m_strSucursal);
                    }
                }

                oCotizacionHija.Series = oCotizacionPadre.Series;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = DateTime.Now;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = DateTime.Now;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_HoSr").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_HoSr").Value;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_idSucursal").Value = m_strSucursal;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_CardCodeOrig").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_CardCodeOrig").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_CardNameOrig").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_CardNameOrig").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_No_Visita").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_No_Visita").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = m_strTipoOT;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_OT_Padre").Value = m_strOTPadre;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_NoOtRef").Value = m_strOTPadre;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = Resource.EstadoOrdenNoIniciada;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "1";
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = "1";
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Gasolina").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString().Trim();
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_GeneraOR").Value.ToString().Trim();

                Boolean blnAgregada = false;
                double dblPrice = 0;
                p_olsIDLinea.Clear();

                for (int i = 0; i <= dtLineas.Rows.Count - 1; i++)
                {
                    if (dtLineas.GetValue("col_Sel", i).ToString().Trim() == "Y")
                    {
                        string strDT = dtLineas.GetValue("col_IDLine", i).ToString().Trim();
                        string strName = dtLineas.GetValue("col_Name", i).ToString().Trim();
                        ApplicationSBO.SetStatusBarMessage("Procesando item: " + strName, BoMessageTime.bmt_Short, false);
                        for (int x = 0; x <= oCotizacionPadre.Lines.Count - 1; x++)
                        {
                            oCotizacionPadre.Lines.SetCurrentLine(x);

                            string strCot = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim();

                            if (strCot == strDT)
                            {
                                break;
                            }
                        }
                        if (blnAgregada == false)
                        {
                            blnAgregada = true;
                        }
                        else
                        {
                            oCotizacionHija.Lines.Add();
                        }

                        p_olsIDLinea.Add(dtLineas.GetValue("col_IDLine", i).ToString().Trim());
                        oCotizacionHija.Lines.ItemCode = oCotizacionPadre.Lines.ItemCode;
                        oCotizacionHija.Lines.ItemDescription = oCotizacionPadre.Lines.ItemDescription;
                        oCotizacionHija.Lines.Quantity = oCotizacionPadre.Lines.Quantity;
                        oCotizacionHija.Lines.Currency = oCotizacionPadre.Lines.Currency;
                        if (string.IsNullOrEmpty(strUsaListaPrecios))
                        {
                            oCotizacionHija.Lines.UnitPrice = oCotizacionPadre.Lines.UnitPrice;
                        }
                        
                        if (string.IsNullOrEmpty(dtLineas.GetValue("col_PrcDes", i).ToString().Trim()))
                        {
                            oCotizacionHija.Lines.DiscountPercent = 0;
                        }
                        else
                        {
                            oCotizacionHija.Lines.DiscountPercent = oCotizacionPadre.Lines.DiscountPercent;
                        }
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value;
                        oCotizacionHija.Lines.TaxCode = oCotizacionPadre.Lines.TaxCode;
                        oCotizacionHija.Lines.VatGroup = oCotizacionPadre.Lines.VatGroup;
                        oCotizacionHija.Lines.CostingCode = oCotizacionPadre.Lines.CostingCode;
                        oCotizacionHija.Lines.CostingCode2 = oCotizacionPadre.Lines.CostingCode2;
                        oCotizacionHija.Lines.CostingCode3 = oCotizacionPadre.Lines.CostingCode3;
                        oCotizacionHija.Lines.CostingCode4 = oCotizacionPadre.Lines.CostingCode4;
                        oCotizacionHija.Lines.CostingCode5 = oCotizacionPadre.Lines.CostingCode5;

                        if (string.IsNullOrEmpty(dtLineas.GetValue("col_Compra", i).ToString().Trim()))
                        {
                            oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N";
                        }
                        else
                        {
                            oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value;
                        }

                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOT;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = m_strTipoOT;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value.ToString();
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value.ToString();

                        if (oCotizacionPadre.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iSalesTree || oCotizacionPadre.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iProductionTree ||
                            oCotizacionPadre.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iTemplateTree
                            && (!String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())))
                        {
                            blnArticuloPaquete = true;
                        }

                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value;
                        oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = "2";
                        oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = "2";
                        oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = "0";
                        oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = "1";

                        if (oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString() == "2")
                        {
                            if (oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value != null)
                            {
                                if (!String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString().Trim()))
                                {
                                    oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value;
                                    oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value;
                                }
                            }
                            oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value;
                            oCotizacionHija.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value;

                            if (!string.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString()))
                            {
                              estadosActividades.Add(Convert.ToInt32(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString()));  
                            }      
                        }
                    }
                }

                estado = 1;
                var todasFin = true;
                var todasSusp = true;
                foreach (int itm in estadosActividades)
                {
                    if (itm == 2)
                    {
                        estado = 2;
                        break;
                    }
                    else if (itm == 3)
                    {
                        estado = 3;
                        todasFin = false;
                    }
                    else if (itm == 4)
                    {
                        estado = 4;
                        todasSusp = false;
                    }
                }
                if (estado == 4 && !todasFin)
                {
                    estado = 3;
                }
                else if (estado == 4 && todasFin)
                {
                    estado = 2;
                }

                var dscEstado = string.Empty;
                ObtieneDescripcionEstado(estado.ToString(), ref dscEstado, oForm);
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = dscEstado;
                oCotizacionHija.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = estado.ToString();
                //se coloca inicia de transaction aqui
                if (!CompanySBO.InTransaction)
                {
                    CompanySBO.StartTransaction();
                }


                if (oCotizacionHija.Add() == 0)
                {
                    CompanySBO.GetNewObjectCode(out p_strCotHija);

                    if (blnArticuloPaquete)
                    {
                        int intCotizacionHija = int.Parse(p_strCotHija);
                        oCotizacionHija.GetByKey(intCotizacionHija);
                        //ActualizarIdLineasHijasPaquetes(intCotizacionHija, oForm, ref oCotizacionPadre, ref oCotizacionHija);
                        ActualizarIdLineasHijasPaquetesNuevo(  ref oCotizacionPadre, ref oCotizacionHija);
                        blnArticuloPaquete = false;
                    }

                    p_blnCancelaPadre = validaCancelarOTPAdre(oCotizacionPadre.Lines);
                    if (p_blnCancelaPadre)
                    {
                        oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = Resource.EstadoCancelada;
                        oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "5";
                    }

                    if (oCotizacionPadre.Update() == 0)
                    {

                        if ((p_blnCancelaPadre) && (oCotizacionPadre.DocumentStatus == BoStatus.bost_Open))
                        {
                            if (oCotizacionPadre.Cancel() != 0)
                            {
                                if (CompanySBO.InTransaction)
                                {
                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                                int error = CompanySBO.GetLastErrorCode();
                                string errorDes = CompanySBO.GetLastErrorDescription();
                                ApplicationSBO.SetStatusBarMessage(String.Format("{0}:{1}", error, errorDes), BoMessageTime.bmt_Short, true);
                                return false;
                            }
                        }
                        return true;

                    }
                    else
                    {
                        if (CompanySBO.InTransaction)
                        {
                            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        int error = CompanySBO.GetLastErrorCode();
                        string errorDes = CompanySBO.GetLastErrorDescription();
                        ApplicationSBO.SetStatusBarMessage(String.Format("{0}:{1}", error, errorDes), BoMessageTime.bmt_Short, true);
                        return false;
                    }

                }
                else
                {
                    if (CompanySBO.InTransaction)
                    {
                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                    int error = CompanySBO.GetLastErrorCode();
                    string errorDes = CompanySBO.GetLastErrorDescription();
                    ApplicationSBO.SetStatusBarMessage(String.Format("{0}:{1}", error, errorDes), BoMessageTime.bmt_Short, true);
                    return false;
                }
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
            }
            finally
            {
                DetenerTimer();
                Utilitarios.DestruirObjeto(ref oCotizacionPadre);
            }
        }

        public void ActualizarIdLineasHijasPaquetes(int p_intNumeroCotizacion, Form oForm, ref SAPbobsCOM.Documents m_oCotizacionPadre, ref SAPbobsCOM.Documents m_oCotizacionEspecial)
        {
            int intSeguirBusquedalinea = 0;
            List<string> ListaId = new List<string>();
            int DocEntryPadre = 0;
            string idlinearepuestoPadre = string.Empty;

            try
            {
                for (int i = 0; i <= m_oCotizacionEspecial.Lines.Count - 1; i++)
                {
                    m_oCotizacionEspecial.Lines.SetCurrentLine(i);

                    string itemcodeOE = m_oCotizacionEspecial.Lines.ItemCode;

                    for (int j = intSeguirBusquedalinea; j <= m_oCotizacionPadre.Lines.Count - 1; j++)
                    {

                        m_oCotizacionPadre.Lines.SetCurrentLine(j);

                        string itemcodePadre = m_oCotizacionPadre.Lines.ItemCode;

                        if (itemcodeOE == itemcodePadre)
                        {

                            switch (m_oCotizacionPadre.Lines.TreeType)
                            {

                                case SAPbobsCOM.BoItemTreeTypes.iSalesTree:

                                    idlinearepuestoPadre = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();

                                    if (!string.IsNullOrEmpty(idlinearepuestoPadre))
                                    {
                                        if (!ListaId.Contains(idlinearepuestoPadre))
                                        {
                                            if (m_oCotizacionEspecial.Lines.ItemCode == m_oCotizacionPadre.Lines.ItemCode && m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString() == string.Empty)
                                            {
                                                m_oCotizacionEspecial.Lines.Quantity = m_oCotizacionPadre.Lines.Quantity;
                                                m_oCotizacionEspecial.Lines.Price = m_oCotizacionPadre.Lines.Price;
                                                m_oCotizacionEspecial.Lines.WarehouseCode = m_oCotizacionPadre.Lines.WarehouseCode;
                                                m_oCotizacionEspecial.Lines.ItemDescription = m_oCotizacionPadre.Lines.ItemDescription;
                                                m_oCotizacionEspecial.Lines.FreeText = m_oCotizacionPadre.Lines.FreeText;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value.ToString();

                                                ListaId.Add(idlinearepuestoPadre);
                                                intSeguirBusquedalinea = j;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                                break;
                                            }
                                            //actualizo la linea padre con Aprobado = No
                                            m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
                                            m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1;
                                            m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                        }
                                    }
                                    else
                                    {
                                        intSeguirBusquedalinea = j;
                                        break;
                                    }
                                    break;


                                case SAPbobsCOM.BoItemTreeTypes.iIngredient:
                                case SAPbobsCOM.BoItemTreeTypes.iNotATree:

                                    idlinearepuestoPadre = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();

                                    if (!string.IsNullOrEmpty(idlinearepuestoPadre))
                                    {
                                        if (!ListaId.Contains(idlinearepuestoPadre))
                                        {
                                            if (m_oCotizacionEspecial.Lines.ItemCode == m_oCotizacionPadre.Lines.ItemCode && m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString() == string.Empty)
                                            {
                                                m_oCotizacionEspecial.Lines.Quantity = m_oCotizacionPadre.Lines.Quantity;
                                                m_oCotizacionEspecial.Lines.Price = m_oCotizacionPadre.Lines.Price;
                                                m_oCotizacionEspecial.Lines.WarehouseCode = m_oCotizacionPadre.Lines.WarehouseCode;
                                                m_oCotizacionEspecial.Lines.ItemDescription = m_oCotizacionPadre.Lines.ItemDescription;
                                                m_oCotizacionEspecial.Lines.FreeText = m_oCotizacionPadre.Lines.FreeText;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value.ToString();

                                                ListaId.Add(idlinearepuestoPadre);
                                                intSeguirBusquedalinea = j;

                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            intSeguirBusquedalinea = j;
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }

                ListaId.Clear();

                m_oCotizacionEspecial.Update();

                if (m_oCotizacionEspecial != null)
                {
                    //'Destruyo el Objeto - Error HRESULT  
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionEspecial);
                    m_oCotizacionEspecial = null;
                }
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
            }
        }

        public void ActualizarIdLineasHijasPaquetesNuevo(  ref SAPbobsCOM.Documents m_oCotizacionPadre, ref SAPbobsCOM.Documents m_oCotizacionEspecial)
        {
            string strIDLineaHija = string.Empty;
            string strIDLineaPadre = string.Empty;
            string strIDLineaPaquetePadreHija = string.Empty;
            string strItemCodeLineaHija = string.Empty;
            string strItemCodeLineaPadre = string.Empty;
            string strIDPaquetePadreLineaPadre = string.Empty;
            try
            {
                for (int i = 0; i <= m_oCotizacionEspecial.Lines.Count - 1; i++)
                {
                    m_oCotizacionEspecial.Lines.SetCurrentLine(i);
                    if (m_oCotizacionEspecial.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iSalesTree)
                    {
                        strIDLineaPaquetePadreHija = m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();
                    }

                    if (m_oCotizacionEspecial.Lines.TreeType == SAPbobsCOM.BoItemTreeTypes.iIngredient)
                    {
                        strItemCodeLineaHija = m_oCotizacionEspecial.Lines.ItemCode;
                        for (int j = 0; j <= m_oCotizacionPadre.Lines.Count - 1; j++)
                        {
                            m_oCotizacionPadre.Lines.SetCurrentLine(j);
                            strItemCodeLineaPadre = m_oCotizacionPadre.Lines.ItemCode;
                            strIDPaquetePadreLineaPadre= m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value.ToString();
                            if (strItemCodeLineaHija == strItemCodeLineaPadre && strIDPaquetePadreLineaPadre == strIDLineaPaquetePadreHija)
                            {
                                m_oCotizacionEspecial.Lines.Quantity = m_oCotizacionPadre.Lines.Quantity;
                                                m_oCotizacionEspecial.Lines.Price = m_oCotizacionPadre.Lines.Price;
                                                m_oCotizacionEspecial.Lines.WarehouseCode = m_oCotizacionPadre.Lines.WarehouseCode;
                                                m_oCotizacionEspecial.Lines.ItemDescription = m_oCotizacionPadre.Lines.ItemDescription;
                                                m_oCotizacionEspecial.Lines.FreeText = m_oCotizacionPadre.Lines.FreeText;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value;
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiempoReal").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NoAva").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TiOtor").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContT").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContC").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value = m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ContR").Value.ToString();
                                                m_oCotizacionEspecial.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = strIDLineaPaquetePadreHija;

                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1;
                                                m_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
                                                break;
                            }
                        }
                    }
                }
                m_oCotizacionEspecial.Update();

                if (m_oCotizacionEspecial != null)
                {
                    //'Destruyo el Objeto - Error HRESULT  
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionEspecial);
                    m_oCotizacionEspecial = null;
                }
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
            }
        }
        private void ActualizaDocsCompra(List<string> olsIDLinea, string m_strOTPadre, string m_strOTHija, Form oForm)
        {
            string strOfertaCompra = "PQT1";
            string strOrdenCompra = "POR1";
            string strEntrada = "PDN1";
            string strFactura = "PCH1";

            string strConsulta = " Update {0} set U_SCGD_NoOT = '{1}' where U_SCGD_NoOT = '{2}' and U_SCGD_ID = '{3}' ";

            SAPbouiCOM.DataTable dtConsulta;

            try
            {

                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);

                foreach (string IDLinea in olsIDLinea)
                {
                    dtConsulta.ExecuteQuery(string.Format(strConsulta, strOfertaCompra, m_strOTHija, m_strOTPadre, IDLinea));
                    dtConsulta.ExecuteQuery(string.Format(strConsulta, strOrdenCompra, m_strOTHija, m_strOTPadre, IDLinea));
                    dtConsulta.ExecuteQuery(string.Format(strConsulta, strEntrada, m_strOTHija, m_strOTPadre, IDLinea));
                    dtConsulta.ExecuteQuery(string.Format(strConsulta, strFactura, m_strOTHija, m_strOTPadre, IDLinea));
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CardCodePorTipoOrden(ref string p_CardCode, ref string p_CardName, ref string p_UsaListaPrecios, string p_IdSucursal, string p_TipoOrden, Form oForm)
        {

            string strConsulta = " SELECT [@SCGD_CONF_TIP_ORDEN].U_CodClien, OCRD.CardName, U_UsaListaPre  " +
             "FROM [@SCGD_CONF_SUCURSAL] INNER JOIN [@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry INNER JOIN " + "OCRD ON [@SCGD_CONF_TIP_ORDEN].U_CodClien = OCRD.CardCode WHERE ([@SCGD_CONF_SUCURSAL].U_Sucurs = '{0}') AND  ([@SCGD_CONF_TIP_ORDEN].U_Code = '{1}')";

            SAPbouiCOM.DataTable dtConsulta;

            try
            {

                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);

                dtConsulta.ExecuteQuery(string.Format(strConsulta, p_IdSucursal, p_TipoOrden));

                if (dtConsulta.Rows.Count != 0)
                {
                    p_CardCode = (string)dtConsulta.GetValue("U_CodClien", 0);
                    p_CardName = (string)dtConsulta.GetValue("CardName", 0);
                    p_UsaListaPrecios = (string)dtConsulta.GetValue("U_UsaListaPre", 0);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Boolean CreaOT(string m_strNoOT, string m_strNoOTPadre, string m_strSucursal, string m_strTipoOT, string m_strCotHija, Form oForm, bool p_blnCancelPadre, string p_strComentarios)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralService oGeneralService2;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData OTPadre;
            SAPbobsCOM.GeneralData OTHija;

            string m_strCode, m_strCodeHija = string.Empty;
            SAPbouiCOM.DataTable dtConsulta;
            string strCodigoClientePorTipoOrden = string.Empty;
            string strNombreClientePorTipoOrden = string.Empty;
            string strEstadoNoIniciado = string.Empty;
            string strUsaListaPrecios = string.Empty;


            try
            {

                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralService2 = null;

                OTPadre = CargaOTPadre(m_strNoOTPadre, m_strSucursal, oForm, ref oGeneralService2);

                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);

                dtConsulta.ExecuteQuery(" select Name from [@SCGD_ESTADOS_OT] with(nolock) where Code = 1 ");
                strEstadoNoIniciado = dtConsulta.GetValue(0, 0).ToString().Trim();

                CardCodePorTipoOrden(ref strCodigoClientePorTipoOrden, ref strNombreClientePorTipoOrden, ref strUsaListaPrecios, m_strSucursal, m_strTipoOT, oForm);
                OTHija = (GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                OTHija.SetProperty("Code", m_strNoOT);
                OTHija.SetProperty("U_DocEntry", m_strCotHija);
                OTHija.SetProperty("U_NoOT", m_strNoOT);
                OTHija.SetProperty("U_NoUni", OTPadre.GetProperty("U_NoUni"));
                OTHija.SetProperty("U_NoCon", OTPadre.GetProperty("U_NoCon"));
                OTHija.SetProperty("U_Ano", OTPadre.GetProperty("U_Ano"));
                OTHija.SetProperty("U_Plac", OTPadre.GetProperty("U_Plac"));
                OTHija.SetProperty("U_Marc", OTPadre.GetProperty("U_Marc"));
                OTHija.SetProperty("U_Esti", OTPadre.GetProperty("U_Esti"));
                OTHija.SetProperty("U_Mode", OTPadre.GetProperty("U_Mode"));
                OTHija.SetProperty("U_CMar", OTPadre.GetProperty("U_CMar"));
                OTHija.SetProperty("U_CEst", OTPadre.GetProperty("U_CEst"));
                OTHija.SetProperty("U_CMod", OTPadre.GetProperty("U_CMod"));
                OTHija.SetProperty("U_NoVis", OTPadre.GetProperty("U_NoVis"));
                OTHija.SetProperty("U_VIN", OTPadre.GetProperty("U_VIN"));
                OTHija.SetProperty("U_km", OTPadre.GetProperty("U_km"));
                OTHija.SetProperty("U_TipOT", m_strTipoOT);
                OTHija.SetProperty("U_Sucu", OTPadre.GetProperty("U_Sucu"));

                if (!string.IsNullOrEmpty((strCodigoClientePorTipoOrden)))
                {
                    OTHija.SetProperty("U_CodCli", strCodigoClientePorTipoOrden);
                    OTHija.SetProperty("U_NCli", strNombreClientePorTipoOrden);
                    OTHija.SetProperty("U_CodCOT", strCodigoClientePorTipoOrden);
                    OTHija.SetProperty("U_NCliOT", strNombreClientePorTipoOrden);
                }
                else
                {
                    OTHija.SetProperty("U_CodCli", OTPadre.GetProperty("U_CodCli"));
                    OTHija.SetProperty("U_NCli", OTPadre.GetProperty("U_NCli"));
                    OTHija.SetProperty("U_CodCOT", OTPadre.GetProperty("U_CodCOT"));
                    OTHija.SetProperty("U_NCliOT", OTPadre.GetProperty("U_NCliOT"));
                }

                OTHija.SetProperty("U_FCom", OTPadre.GetProperty("U_FCom"));
                OTHija.SetProperty("U_HCom", OTPadre.GetProperty("U_HCom"));
                OTHija.SetProperty("U_FApe", OTPadre.GetProperty("U_FApe"));
                OTHija.SetProperty("U_HApe", OTPadre.GetProperty("U_HApe"));
                OTHija.SetProperty("U_FRec", OTPadre.GetProperty("U_FRec"));
                OTHija.SetProperty("U_HRec", OTPadre.GetProperty("U_HRec"));
                OTHija.SetProperty("U_FFact", OTPadre.GetProperty("U_FFact"));
                OTHija.SetProperty("U_FEntr", OTPadre.GetProperty("U_FEntr"));
                OTHija.SetProperty("U_OTRef", OTPadre.GetProperty("U_OTRef"));
                OTHija.SetProperty("U_NGas", OTPadre.GetProperty("U_NGas"));
                OTHija.SetProperty("U_HMot", OTPadre.GetProperty("U_HMot"));
                OTHija.SetProperty("U_DEstO", strEstadoNoIniciado.Trim());
                OTHija.SetProperty("U_EstO", "1");
                OTHija.SetProperty("U_Ase", OTPadre.GetProperty("U_Ase"));
                OTHija.SetProperty("U_EncO", OTPadre.GetProperty("U_EncO"));
                OTHija.SetProperty("U_Obse", p_strComentarios);

                if (p_blnCancelPadre)
                {
                    OTPadre.SetProperty("U_EstO", "5");
                    OTPadre.SetProperty("U_DEstO", Resource.EstadoCancelada);
                }
                ManejoInsertarOThijaControlColaborador(ref OTPadre, ref OTHija, Convert.ToInt32(m_strCotHija));

                oGeneralService.Add(OTHija);
                oGeneralService2.Update(OTPadre);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private GeneralData CargaOTPadre(string p_strNoOtPadre, string p_strSucursal, Form oForm, ref SAPbobsCOM.GeneralService p_oGeneralService)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            //SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.GeneralData OTPadre;
            string m_strCode, m_strCodeHija = string.Empty;
            SAPbouiCOM.DataTable dtConsulta;

            try
            {
                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);
                dtConsulta.ExecuteQuery(string.Format(" select Code from [@SCGD_OT] with (nolock) where U_NoOT = '{0}' and U_Sucu = '{1}' ", p_strNoOtPadre, p_strSucursal));
                m_strCode = dtConsulta.GetValue(0, 0).ToString().Trim();
                oCompanyService = CompanySBO.GetCompanyService();
                p_oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (GeneralDataParams)p_oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", m_strCode);
                OTPadre = p_oGeneralService.GetByParams(oGeneralParams);

                return OTPadre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string RetornaSiguienteOrden(string p_strVisita, Form oForm)
        {
            SAPbouiCOM.DataTable dtConsulta;

            string m_strValue = string.Empty;
            int m_intValue = 0;
            string m_strRetorno = string.Empty;

            try
            {
                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);

                dtConsulta.ExecuteQuery(string.Format("select COUNT (U_SCGD_No_Visita) from OQUT where oqut.U_SCGD_No_Visita = '{0}'", p_strVisita));

                m_strValue = dtConsulta.GetValue(0, 0).ToString().Trim();

                if (string.IsNullOrEmpty(m_strValue) == false)
                {
                    int.TryParse(m_strValue, out m_intValue);
                    m_intValue += 1;

                    if (m_intValue >= 10)
                    {
                        m_strRetorno = m_intValue.ToString().Trim();
                    }
                    else
                    {
                        m_strRetorno = m_intValue.ToString().Trim();
                        m_strRetorno = string.Format("0{0}", m_strRetorno);
                    }
                }
                return m_strRetorno;
            }
            catch (Exception)
            {
                throw;
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
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CreaSolicitudOTEsp(Form oForm, ItemEvent pVal, ref bool BubbleEvent, string m_strCotPadre, ref bool aprobacionesXSuc)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralService oGeneralService2;
            SAPbobsCOM.GeneralData OTPadre;
            SAPbobsCOM.GeneralData SolicitudOT;
            SAPbobsCOM.GeneralDataCollection LineasSolicitudOT;
            SAPbobsCOM.GeneralData LineaSolicitudOT;
            SAPbobsCOM.Documents oCotizacionPadre;
            SAPbouiCOM.DataTable dtQuery;
            SAPbouiCOM.ComboBox oCombo;
            SAPbouiCOM.Item sboItem;
            string m_strNoOTPadre = string.Empty;
            string m_strSucursal = String.Empty;
            string m_strTipoOT = string.Empty;
            SAPbouiCOM.Matrix mtxOTEspecial;
            string strTipArt = string.Empty;

            sboItem = oForm.Items.Item("cboTipOtE");
            oCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;

            if (!Utilitarios.ValidaSiDataTableExiste(oForm, "dtQuery"))
            {
                dtQuery = oForm.DataSources.DataTables.Add("dtQuery");
            }
            else
            {
                dtQuery = oForm.DataSources.DataTables.Item("dtQuery");
            }

            string m_strCode, m_strCodeSolicitudOT, m_strSerie, m_strIDVehiculo = string.Empty;
            SAPbouiCOM.DataTable dtConsulta;

            int m_intDocEntry = 0;

            Boolean blnAgregada = true;
            Boolean blnPasoTodas = true;

            string strCodigoClientePorTipoOrden = string.Empty;
            string strNombreClientePorTipoOrden = string.Empty;
            string strTipoOT = string.Empty;

            try
            {
                mtxOTEspecial = (Matrix)oForm.Items.Item("mtxOTLines").Specific;
                mtxOTEspecial.FlushToDataSource();
                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_SOTESP");
                oGeneralService2 = null;

                dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas);

                m_intDocEntry = int.Parse(g_strDocE);
                oCotizacionPadre = CargaObjetoCotizacion(m_intDocEntry);
                m_strNoOTPadre = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                m_strSucursal = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                m_strTipoOT = oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString().Trim();
                OTPadre = CargaOTPadre(m_strNoOTPadre, m_strSucursal, oForm, ref oGeneralService2);
                dtConsulta = oForm.DataSources.DataTables.Item(strDataTableConsulta);

                /**********************************************************************************************************************************/
                /*validacion de aprobaciones por tipo de orden*/
                var queryValApr = "SELECT U_EspAprob FROM [@SCGD_CONF_APROBAC] CAP with (nolock)" +
                                        "LEFT JOIN [@SCGD_CONF_SUCURSAL] CS with (nolock) ON CAP.DocEntry = CS.DocEntry " +
                    "WHERE CS.U_Sucurs ='{0}' and cap.U_TipoOT = '{1}'";
                var equeryValApr = string.Format(queryValApr, m_strSucursal, m_strTipoOT);
                dtConsulta.ExecuteQuery(equeryValApr);
                var espApr = dtConsulta.GetValue("U_EspAprob", 0).ToString();
                if (!String.IsNullOrEmpty(espApr))
                {
                    if (espApr == "Y")
                        aprobacionesXSuc = true;
                    else
                        aprobacionesXSuc = false;
                }
                else
                    aprobacionesXSuc = false;
                /**********************************************************************************************************************************/

                SolicitudOT = (GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                LineasSolicitudOT = SolicitudOT.Child("SCGD_LINEAS_SOT_ESP");

                var query = String.Format("select (oh.firstName + ' '+oh.lastName) as Asesor, oq.OwnerCode, oq.U_SCGD_Cod_Modelo, oq.U_SCGD_Cod_Marca, oq.U_SCGD_Cod_Estilo,oq.U_SCGD_GeneraOR from OQUT as oq with (nolock) inner join OHEM as oh with(nolock) on oq.OwnerCode = oh.empID where oq.DocEntry='{0}'", OTPadre.GetProperty("U_DocEntry"));
                dtQuery.ExecuteQuery(query);
                SolicitudOT.SetProperty("U_Anno", OTPadre.GetProperty("U_Ano"));

                if (strCodigoClientePorTipoOrden != string.Empty)
                {
                    SolicitudOT.SetProperty("U_Cod_Clie", strCodigoClientePorTipoOrden);
                    SolicitudOT.SetProperty("U_Nom_Clie", strNombreClientePorTipoOrden);
                }
                else
                {
                    SolicitudOT.SetProperty("U_Cod_Clie", OTPadre.GetProperty("U_CodCli"));
                    SolicitudOT.SetProperty("U_Nom_Clie", OTPadre.GetProperty("U_NCli"));
                }

                SolicitudOT.SetProperty("U_CardCodeOrig", OTPadre.GetProperty("U_CodCli"));
                SolicitudOT.SetProperty("U_CardNameOrig", OTPadre.GetProperty("U_NCli"));
                SolicitudOT.SetProperty("U_Cod_Ases", dtQuery.GetValue("OwnerCode", 0).ToString());
                SolicitudOT.SetProperty("U_Cod_Mar", dtQuery.GetValue("U_SCGD_Cod_Marca", 0).ToString());
                SolicitudOT.SetProperty("U_Cod_Est", dtQuery.GetValue("U_SCGD_Cod_Estilo", 0).ToString());
                SolicitudOT.SetProperty("U_Cod_Mod", dtQuery.GetValue("U_SCGD_Cod_Modelo", 0).ToString());
                SolicitudOT.SetProperty("U_Cod_Uni", OTPadre.GetProperty("U_NoUni"));
                String comentarios = String.Format("{0} {1}. {2}", Resource.Sederivadelaorden, m_strNoOTPadre, OTPadre.GetProperty("U_Obse").ToString().Trim());
                SolicitudOT.SetProperty("U_Comment", comentarios.Trim().Length <= 250 ? comentarios.Trim() : comentarios.Trim().Substring(0, 250));
                SolicitudOT.SetProperty("U_CotRef", m_strCotPadre);
                SolicitudOT.SetProperty("U_Des_Mar", OTPadre.GetProperty("U_Marc"));
                SolicitudOT.SetProperty("U_Des_Est", OTPadre.GetProperty("U_Esti"));
                SolicitudOT.SetProperty("U_Des_Mod", OTPadre.GetProperty("U_Mode"));
                SolicitudOT.SetProperty("U_Estad_OT", "1");
                SolicitudOT.SetProperty("U_TipoOrd", oCombo.Selected.Value);
                SolicitudOT.SetProperty("U_NomTipOT", oCombo.Selected.Description);

                dtConsulta.ExecuteQuery(string.Format("select U_SerOfV from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}'", m_strSucursal));
                m_strSerie = "";
                if (!string.IsNullOrEmpty(dtConsulta.GetValue("U_SerOfV", 0).ToString().Trim()))
                {
                    m_strSerie = dtConsulta.GetValue("U_SerOfV", 0).ToString().Trim();
                }
                SolicitudOT.SetProperty("U_Series", m_strSerie);

                SolicitudOT.SetProperty("U_Fec_Ape", DateTime.Now);
                SolicitudOT.SetProperty("U_Fec_Com", OTPadre.GetProperty("U_FCom"));

                dtConsulta.ExecuteQuery(string.Format(" select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}' ", OTPadre.GetProperty("U_NoUni")));
                m_strIDVehiculo = "";
                if (!string.IsNullOrEmpty(dtConsulta.GetValue("Code", 0).ToString().Trim()))
                {
                    m_strIDVehiculo = dtConsulta.GetValue("Code", 0).ToString().Trim();
                }
                SolicitudOT.SetProperty("U_Id_Vehi", m_strIDVehiculo);

                var impRecep = dtQuery.GetValue("U_SCGD_GeneraOR", 0).ToString();
                SolicitudOT.SetProperty("U_ImpRecp", impRecep == "1" ? "Y" : "N");

                SolicitudOT.SetProperty("U_klm", OTPadre.GetProperty("U_km"));


                SolicitudOT.SetProperty("U_NomAse", dtQuery.GetValue("Asesor", 0));
                SolicitudOT.SetProperty("U_OTPadre", m_strNoOTPadre);
                SolicitudOT.SetProperty("U_No_Vis", OTPadre.GetProperty("U_NoVis"));
                SolicitudOT.SetProperty("U_OTRefer", m_strNoOTPadre);
                SolicitudOT.SetProperty("U_Placa", OTPadre.GetProperty("U_Plac"));
                SolicitudOT.SetProperty("U_VIN", OTPadre.GetProperty("U_VIN"));
                SolicitudOT.SetProperty("U_Num_Coti", OTPadre.GetProperty("U_DocEntry"));


                blnAgregada = false;
                blnPasoTodas = true;

                for (int i = 0; i <= dtLineas.Rows.Count - 1; i++)
                {
                    if (dtLineas.GetValue("col_Sel", i).ToString().Trim() == "Y")
                    {
                        for (int x = 0; x <= oCotizacionPadre.Lines.Count - 1; x++)
                        {
                            oCotizacionPadre.Lines.SetCurrentLine(x);

                            string strCot = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim();
                            strTipArt = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString();
                            string strDT = dtLineas.GetValue("col_IDLine", i).ToString().Trim();

                            if (strCot == strDT)
                            {
                                break;
                            }
                        }

                        LineaSolicitudOT = LineasSolicitudOT.Add();
                        LineaSolicitudOT.SetProperty("U_Cant", double.Parse(dtLineas.GetValue("col_Quant", i).ToString().Trim(), n));
                        LineaSolicitudOT.SetProperty("U_Coment", "");
                        LineaSolicitudOT.SetProperty("U_Costo", double.Parse(dtLineas.GetValue("col_Price", i).ToString().Trim()));
                        LineaSolicitudOT.SetProperty("U_Descrip", dtLineas.GetValue("col_Name", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_Tax", dtLineas.GetValue("col_IndImp", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_ItemCode", dtLineas.GetValue("col_Code", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_Moned", dtLineas.GetValue("col_Curr", i).ToString().Trim());

                        string prcDes = dtLineas.GetValue("col_PrcDes", i).ToString().Trim();
                        LineaSolicitudOT.SetProperty("U_PorcDs", (string.IsNullOrEmpty(prcDes) ? "0" : prcDes));

                        LineaSolicitudOT.SetProperty("U_Precio", double.Parse(dtLineas.GetValue("col_Price", i).ToString().Trim()));
                        LineaSolicitudOT.SetProperty("U_CPen", dtLineas.GetValue("col_CPend", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_CSol", dtLineas.GetValue("col_CSol", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_CRec", dtLineas.GetValue("col_CRec", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_CPDe", dtLineas.GetValue("col_PenDev", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_CPTr", dtLineas.GetValue("col_PenTra", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_CPBo", dtLineas.GetValue("col_PenBod", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_ID_Linea", dtLineas.GetValue("col_IDLine", i).ToString().Trim());
                        LineaSolicitudOT.SetProperty("U_TipArtSO", strTipArt);
                        var comprar = dtLineas.GetValue("col_Comprar", i).ToString().Trim();
                        LineaSolicitudOT.SetProperty("U_Compra", comprar != "Y" ? "N" : "Y");
                        LineaSolicitudOT.SetProperty("U_Selec", aprobacionesXSuc ? "Y" : "N");

                    }
                    else
                    {
                        blnPasoTodas = false;
                    }
                }

                oGeneralService.Add(SolicitudOT);
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool validaCancelarOTPAdre(Document_Lines p_lines)
        {
            try
            {
             bool blnCancel = true;
            for (int index = 0; index <= p_lines.Count - 1; index++)
            {
                p_lines.SetCurrentLine(index);
                if (int.Parse(p_lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString()) == 1)
                {
                    blnCancel = false;
                    break;
                }
            }
            return blnCancel;
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
            }
           
        }

        private void EnviarMensajes(Boolean blnSolicitud, String strTipoOrden, String strNoOT, string strIdSuc, SAPbouiCOM.Form p_oForm, string strDocEntry = "")
        {
            //Dim drdUsuariosParaEnviarMensajes As SqlClient.SqlDataReader = Nothing
            //Dim adpUsuarios As New UsuariosOTEspecialDataAdapter
            var intCodConfiguracion = 0;

            try
            {
                if (!blnSolicitud)
                {

                    var mensaj = String.Format(Resource.MensajeOrdenTipo, strTipoOrden);

                    //Mensaje parael asesor
                    Utilitarios.CreaMensajeSBO(mensaj + " " + Resource.MensajeSolicitada, strDocEntry, (SAPbobsCOM.Company)CompanySBO, strNoOT, false, Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), strIdSuc, (SAPbouiCOM.Form)p_oForm, strDataTableConsulta, false, Utilitarios.RolesMensajeria.EncargadoProduccion, false);
                }
                else
                {
                    var strMensajeSolicitud = string.Empty;

                    strMensajeSolicitud = string.Format(Resource.MensajeOrdenTipo, strTipoOrden);

                    var mensaj = String.Format("{0} {1}", Resource.MensajeSolicitudOTEspecial, strMensajeSolicitud);
                    //Mensaje para el asesor
                    Utilitarios.CreaMensajeSBO(mensaj + " " + Resource.MensajeSolicitada, strDocEntry, (SAPbobsCOM.Company)CompanySBO, strNoOT, false, Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSOE).ToString(), strIdSuc, (SAPbouiCOM.Form)p_oForm, strDataTableConsulta, false, Utilitarios.RolesMensajeria.EncargadoSOE, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Crea lineas control colaborador cuando se crea la OT hija
        /// </summary>
        /// <param name="p_oGeneralDataPadre">General data de la OT padre</param>
        /// <param name="p_oGeneralDataHija">General data de la OT Hija</param>
        /// <param name="p_intDocEntry">DocEntry de la cotizacion</param>
        private void ManejoInsertarOThijaControlColaborador(ref SAPbobsCOM.GeneralData p_oGeneralDataPadre, ref SAPbobsCOM.GeneralData p_oGeneralDataHija, int p_intDocEntry)
        {
            SAPbobsCOM.Documents m_objCotizacionHija;
            SAPbobsCOM.Document_Lines m_objLineasCotizacionHija;

            string m_strConsultaDocEntry;
            string m_strConsultaCodePadre;
            string m_strConsultaCodeHija;
            string m_strResultadoCodeHija;
            string m_strResultadoDocEntry;
            string m_strResultadoCodePadre;
            int i = 0;
            int j = 0;
            int y = 0;

            SAPbobsCOM.GeneralDataCollection m_childs = default(SAPbobsCOM.GeneralDataCollection);
            SAPbobsCOM.GeneralData m_childdata = default(SAPbobsCOM.GeneralData);

            SAPbobsCOM.GeneralDataCollection m_childsPadre = default(SAPbobsCOM.GeneralDataCollection);
            SAPbobsCOM.GeneralData m_childdataPadre = default(SAPbobsCOM.GeneralData);
            try
            {
                m_objCotizacionHija = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                m_objCotizacionHija.GetByKey(p_intDocEntry);
                m_objLineasCotizacionHija = m_objCotizacionHija.Lines;

                m_childsPadre = p_oGeneralDataPadre.Child("SCGD_CTRLCOL");
                m_childs = p_oGeneralDataHija.Child("SCGD_CTRLCOL");
                var emp = m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString().Trim();
                var tipArt = m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim();

                for (i = 0; i <= m_objLineasCotizacionHija.Count - 1; i++)
                {
                    m_objLineasCotizacionHija.SetCurrentLine(i);
                    if (m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_EmpAsig").Value != null)
                    {
                        if (!string.IsNullOrEmpty(m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString().Trim()) && m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim() == "2")
                        {
                            for (j = 0; j <= m_childsPadre.Count - 1; j++)
                            {
                                m_childdataPadre = m_childsPadre.Item(j);
                                if (m_childdataPadre.GetProperty("U_IdAct").ToString() == m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                                {
                                    m_childdata = m_childs.Add();
                                    m_childdata.SetProperty("U_Estad", m_childdataPadre.GetProperty("U_Estad"));
                                    m_childdata.SetProperty("U_IdAct", m_childdataPadre.GetProperty("U_IdAct"));
                                    m_childdata.SetProperty("U_NoFas", m_childdataPadre.GetProperty("U_NoFas"));
                                    m_childdata.SetProperty("U_Colab", m_childdataPadre.GetProperty("U_Colab"));
                                    m_childdata.SetProperty("U_TMin", m_childdataPadre.GetProperty("U_TMin"));
                                    m_childdata.SetProperty("U_CosRe", m_childdataPadre.GetProperty("U_CosRe"));
                                    m_childdata.SetProperty("U_CosEst", m_childdataPadre.GetProperty("U_CosEst"));
                                    m_childdata.SetProperty("U_ReAsig", m_childdataPadre.GetProperty("U_ReAsig"));
                                    m_childdata.SetProperty("U_DFIni", m_childdataPadre.GetProperty("U_DFIni"));
                                    m_childdata.SetProperty("U_HFIni", m_childdataPadre.GetProperty("U_HFIni"));
                                    m_childdata.SetProperty("U_DFFin", m_childdataPadre.GetProperty("U_DFFin"));
                                    m_childdata.SetProperty("U_HFFin", m_childdataPadre.GetProperty("U_HFFin"));
                                    m_childdata.SetProperty("U_HoraIni", m_childdataPadre.GetProperty("U_HoraIni"));
                                    m_childdata.SetProperty("U_FechPro", m_childdataPadre.GetProperty("U_FechPro"));
                                    m_childdata.SetProperty("U_CodFas", m_childdataPadre.GetProperty("U_CodFas"));

                                }
                            }

                            for (y = m_childsPadre.Count - 1; y >= 0; y += -1)
                            {
                                m_childdataPadre = m_childsPadre.Item(y);
                                if (m_childdataPadre.GetProperty("U_IdAct").ToString() == m_objLineasCotizacionHija.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                                {
                                    m_childsPadre.Remove(y);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                throw;// Utilitarios.ManejadorErrores(ex, SBO_Application);
            }
        }

        /// <summary>
        /// Crear una instancia del Timer que se ejecuta cada cierto tiempo para limpiar la cola de mensajes
        /// y evitar que el add-on se caiga (Errores del tipo RPC Server Call y similares)
        /// </summary>        
        private static void InicializarTimer()
        {
            try
            {
                //Inicializa un timer que se ejecuta cada 30 segundos
                //y llama al método LimpiarColaMensajes
                oTimer = new System.Timers.Timer(30000);
                //Removemos el manejador para asegurarnos que no se duplique
                //se puede remover en forma segura antes de ser utilizado
                oTimer.Elapsed -= LimpiarColaMensajes;
                oTimer.Elapsed += LimpiarColaMensajes;
                oTimer.AutoReset = true;
                oTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Detiene el timer usado para limpiar la cola de mensajes
        /// utilizada para evitar que el add-on se caida cuando se ejecutan procesos muy largos
        /// </summary>
        private static void DetenerTimer()
        {
            try
            {
                oTimer.Stop();
                oTimer.Dispose();
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Limpia la cola de mensajes en operaciones muy largas que congelan SAP Business One previniendo que el Add-On falle
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void LimpiarColaMensajes(Object source, ElapsedEventArgs e)
        {
            try
            {
                //En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
                //RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
                //la cola de mensajes
                DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, true);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    }
}
