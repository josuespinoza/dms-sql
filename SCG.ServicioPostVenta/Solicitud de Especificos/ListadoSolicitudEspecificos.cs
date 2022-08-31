using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class ListadoSolicitudEspecificos : IFormularioSBO, IUsaMenu
    {
        #region Metodos

        /// <summary>
        /// Carga la matriz de Requisiciones
        /// </summary>
        /// <param name="p_CodigoBodega"></param>
        /// <param name="p_ItemCode"></param>
        public void CargarMatriz()
        {
            SAPbouiCOM.Form oForm;
            DateTime dtiFechIniSol;
            DateTime dtiFechFinSol;
            DateTime dtiFechIniRes;
            DateTime dtiFechFinRes;
            var query = string.Empty;
            bool usaFiltro = false;
            try
            {
                FormularioSBO.Freeze(true);
                DateTime dateReq = new DateTime();
                g_oMatrixListaSol = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(g_strmtxTareas).Specific;
                g_oMatrixListaSol.FlushToDataSource();
                g_oEditNoSol = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoSol").Specific;
                g_oEditNoOT = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoOT").Specific;
                g_oEditPlaca = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtPlaca").Specific;
                g_oEditUnidad = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtUnidad").Specific;

                g_oComboMarca = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbMarca").Specific);
                g_oComboEstilo = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbEstilo").Specific);
                g_oComboModelo = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbModelo").Specific);
                g_oComboSucursal = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbSucu").Specific);
                g_oComboEstado = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbStatus").Specific);

                string noSol = String.IsNullOrEmpty(g_oEditNoSol.Value) ? string.Empty : g_oEditNoSol.Value;
                string noOT = String.IsNullOrEmpty(g_oEditNoOT.Value) ? string.Empty : g_oEditNoOT.Value;
                string noUnidad = String.IsNullOrEmpty(g_oEditUnidad.Value) ? string.Empty : g_oEditUnidad.Value;
                string noPlaca = String.IsNullOrEmpty(g_oEditPlaca.Value) ? string.Empty : g_oEditPlaca.Value;
                string strModelo = String.IsNullOrEmpty(g_oComboModelo.Value) ? string.Empty : g_oComboModelo.Value;
                string idSucu = String.IsNullOrEmpty(g_oComboSucursal.Value) ? string.Empty : g_oComboSucursal.Value;
                string idStatus = String.IsNullOrEmpty(g_oComboEstado.Value) ? string.Empty : g_oComboEstado.Value;
                string strMarca = String.IsNullOrEmpty(g_oComboMarca.Value) ? string.Empty : g_oComboMarca.Value;
                string strEstilo = String.IsNullOrEmpty(g_oComboEstilo.Value) ? string.Empty : g_oComboEstilo.Value;
                string strFechIniSol = String.IsNullOrEmpty(EditTextFechaSIni.ObtieneValorUserDataSource()) ? string.Empty : EditTextFechaSIni.ObtieneValorUserDataSource();
                string strFechFinSol = String.IsNullOrEmpty(EditTextFechaSFin.ObtieneValorUserDataSource()) ? string.Empty : EditTextFechaSFin.ObtieneValorUserDataSource();
                string strFechIniRes = String.IsNullOrEmpty(EditTextFechaRIni.ObtieneValorUserDataSource()) ? string.Empty : EditTextFechaRIni.ObtieneValorUserDataSource();
                string strFechFinRes = String.IsNullOrEmpty(EditTextFechaRFin.ObtieneValorUserDataSource()) ? string.Empty : EditTextFechaRFin.ObtieneValorUserDataSource();

                dtiFechIniSol = string.IsNullOrEmpty(strFechIniSol) ? new DateTime() : DateTime.ParseExact(strFechIniSol, "yyyyMMdd", CultureInfo.InvariantCulture);
                dtiFechFinSol = string.IsNullOrEmpty(strFechFinSol) ? new DateTime() : DateTime.ParseExact(strFechFinSol, "yyyyMMdd", CultureInfo.InvariantCulture);
                dtiFechIniRes = string.IsNullOrEmpty(strFechIniRes) ? new DateTime() : DateTime.ParseExact(strFechIniRes, "yyyyMMdd", CultureInfo.InvariantCulture);
                dtiFechFinRes = string.IsNullOrEmpty(strFechFinRes) ? new DateTime() : DateTime.ParseExact(strFechFinRes, "yyyyMMdd", CultureInfo.InvariantCulture);

                query = queryListSol;

                if (!string.IsNullOrEmpty(noSol))
                {
                    query = query.Contains(" Where ")
                        ? String.Format(" {0} and se.DocNum like '%{1}%' ", query, noSol)
                        : String.Format(" {0} Where se.DocNum like '%{1}%' ", query, noSol);
                    usaFiltro = true;
                }
                if (!string.IsNullOrEmpty(noOT))
                {
                    query = query.Contains(" Where ")
                        ? String.Format(" {0} and se.U_NumeroOT like '%{1}%' ", query, noOT)
                        : String.Format(" {0} Where se.U_NumeroOT like '%{1}%' ", query, noOT);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(noUnidad))
                {
                    query = query.Contains(" Where ")
                        ? String.Format(" {0} and q.U_SCGD_Cod_Unidad like '%{1}%' ", query, noUnidad)
                        : String.Format(" {0} Where q.U_SCGD_Cod_Unidad like '%{1}%' ", query, noUnidad);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(noPlaca))
                {
                    query = query.Contains(" Where ")
                        ? String.Format(" {0} and q.U_SCGD_Num_Placa like '%{1}%' ", query, noPlaca)
                        : String.Format(" {0} Where q.U_SCGD_Num_Placa like '%{1}%' ", query, noPlaca);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(strModelo) && g_oChkModelo.Checked)
                {
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and q.U_SCGD_Cod_Modelo = '{1}' ", query, strModelo)
                        : String.Format("{0} Where q.U_SCGD_Cod_Modelo = '{1}' ", query, strModelo);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(idSucu))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and q.U_SCGD_idSucursal = '{1}' ", query, idSucu)
                        : String.Format("{0} Where q.U_SCGD_idSucursal = '{1}' ", query, idSucu);

                if (!string.IsNullOrEmpty(idStatus) && g_oChkEstado.Checked)
                {
                    if (idStatus == "0" || idStatus == "1")
                        query = query.Contains(" Where ")
                            ? String.Format("{0} and se.U_Estado = '{1}' and se.Canceled = 'N'", query, idStatus)
                            : String.Format("{0} Where se.U_Estado = '{1}' and se.Canceled = 'N'", query, idStatus);
                    else
                        query = query.Contains(" Where ")
                            ? String.Format("{0} and se.Canceled = 'Y' ", query)
                            : String.Format("{0} Where se.Canceled = 'Y' ", query);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(strMarca) && g_oChkMarca.Checked)
                {
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and q.U_SCGD_Cod_Marca = '{1}' ", query, strMarca)
                        : String.Format("{0} Where q.U_SCGD_Cod_Marca = '{1}' ", query, strMarca);
                    usaFiltro = true;
                }

                if (!string.IsNullOrEmpty(strEstilo) && g_oChkEstilo.Checked)
                {
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and q.U_SCGD_Cod_Estilo = '{1}' ", query, strEstilo)
                        : String.Format("{0} Where q.U_SCGD_Cod_Estilo = '{1}' ", query, strEstilo);
                    usaFiltro = true;
                }


                if (g_oChkDateSol.Checked)
                {
                    if (!string.IsNullOrEmpty(strFechIniSol))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and se.U_FechaSol >= '{1} 00:00:00.000' ", query, dtiFechIniSol.ToString("yyyy-MM-dd"))
                                    : String.Format("{0} Where se.U_FechaSol >= '{1} 00:00:00.000' ", query, dtiFechIniSol.ToString("yyyy-MM-dd"));

                    if (!string.IsNullOrEmpty(strFechFinSol))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and se.U_FechaSol < '{1} 23:59:59.999' ", query, dtiFechFinSol.ToString("yyyy-MM-dd"))
                                    : String.Format("{0} Where se.U_FechaSol < '{1} 23:59:59.999' ", query, dtiFechFinSol.ToString("yyyy-MM-dd"));
                    usaFiltro = true;
                }

                if (g_oChkDateRes.Checked)
                {
                    if (!string.IsNullOrEmpty(strFechIniRes))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and se.U_FechResp >= '{1} 00:00:00.000' ", query, dtiFechIniRes.ToString("yyyy-MM-dd"))
                                    : String.Format("{0} Where se.U_FechResp >= '{1} 00:00:00.000' ", query, dtiFechIniRes.ToString("yyyy-MM-dd"));

                    if (!string.IsNullOrEmpty(strFechFinRes))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and se.U_FechResp < '{1} 23:59:59.999' ", query, dtiFechFinRes.ToString("yyyy-MM-dd"))
                                    : String.Format("{0} Where se.U_FechResp < '{1} 23:59:59.999' ", query, dtiFechFinRes.ToString("yyyy-MM-dd"));
                    usaFiltro = true;
                }

                dtQueryLista.Clear();
                dtQueryLista = FormularioSBO.DataSources.DataTables.Item(strDtQueryLista);
                query = string.Format("{0} order by se.U_FechaSol , se.U_HoraSol ", query);

                if (!usaFiltro)
                {
                    query = query.Replace("select ", "select Top(100) ");
                }
                dtQueryLista.ExecuteQuery(query);

                g_dtSolicitudes.Rows.Clear();
                if (g_dtSolicitudes == null)
                {
                    g_dtSolicitudes = FormularioSBO.DataSources.DataTables.Item(g_strdtSolicitudes);
                }
                if (g_dtSolicitudes != null)
                {
                    for (int i = 0; i <= g_dtSolicitudes.Rows.Count - 1; i++)
                    {
                        g_dtSolicitudes.Rows.Remove(i);
                    }
                }
                for (int i = 0; i <= dtQueryLista.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dtQueryLista.GetValue("ColNoOT", i).ToString()))
                    {
                        g_dtSolicitudes.Rows.Add(1);
                        g_dtSolicitudes.SetValue("ColDocE", i, dtQueryLista.GetValue("ColDocE", i));
                        g_dtSolicitudes.SetValue("ColDocN", i, dtQueryLista.GetValue("ColDocN", i));
                        g_dtSolicitudes.SetValue("ColNoOT", i, dtQueryLista.GetValue("ColNoOT", i));
                        
                        g_dtSolicitudes.SetValue("ColFecha", i, Convert.ToDateTime(dtQueryLista.GetValue("ColFecha", i)).ToString("dd/MM/yyyy"));
                        String hora = dtQueryLista.GetValue("ColHora", i).ToString();
                        switch (hora.Length)
                        {
                            case 1:
                                hora = "000" + hora;
                                break;
                            case 2:
                                hora = "00" + hora;
                                break;
                            case 3:
                                hora = "0" + hora;
                                break;
                            case 4:
                                hora = hora;
                                break;
                        }
                        g_dtSolicitudes.SetValue("ColHora", i, hora);
                        g_dtSolicitudes.SetValue("ColSolBy", i, dtQueryLista.GetValue("ColSolBy", i));
                        g_dtSolicitudes.SetValue("ColMarca", i, dtQueryLista.GetValue("ColMarca", i));
                        g_dtSolicitudes.SetValue("ColEstilo", i, dtQueryLista.GetValue("ColEstilo", i));
                        g_dtSolicitudes.SetValue("ColModelo", i, dtQueryLista.GetValue("ColModelo", i));
                        g_dtSolicitudes.SetValue("ColUnidad", i, dtQueryLista.GetValue("ColUnidad", i));
                        g_dtSolicitudes.SetValue("ColPlaca", i, dtQueryLista.GetValue("ColPlaca", i));
                    }
                }
                g_oMatrixListaSol.LoadFromDataSource();
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

  

        public void ManejadorEventoItemPress(ref SAPbouiCOM.ItemEvent pval, String FormUID, ref Boolean BubbleEvent)
        {
            try
            {
                if (pval.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    FormularioSBO.Freeze(true);
                    if (pval.ActionSuccess)
                    {
                        switch (pval.ItemUID)
                        {
                            case "btnUpdate":
                                CargarMatriz();
                                break;

                            case "chkDateS":
                                if (g_oChkDateSol.Checked)
                                {
                                    FormularioSBO.Items.Item("txtFecIniS").Enabled = true;
                                    FormularioSBO.Items.Item("txtFecFinS").Enabled = true;
                                }
                                else
                                {
                                    FormularioSBO.Items.Item("txtNoSol").Click();
                                    FormularioSBO.Items.Item("txtFecIniS").Enabled = false;
                                    FormularioSBO.Items.Item("txtFecFinS").Enabled = false;
                                }
                                break;

                            case "chkDateR":
                                if (g_oChkDateRes.Checked)
                                {
                                    FormularioSBO.Items.Item("txtFecIniR").Enabled = true;
                                    FormularioSBO.Items.Item("txtFecFinR").Enabled = true;
                                }
                                else
                                {
                                    FormularioSBO.Items.Item("txtNoSol").Click();
                                    FormularioSBO.Items.Item("txtFecIniR").Enabled = false;
                                    FormularioSBO.Items.Item("txtFecFinR").Enabled = false;
                                }
                                break;
                            case "chkMarca":
                                FormularioSBO.Items.Item("txtNoSol").Click();
                                FormularioSBO.Items.Item("cbMarca").Enabled = g_oChkMarca.Checked;
                                break;
                            case "chkEstilo":
                                FormularioSBO.Items.Item("txtNoSol").Click();
                                FormularioSBO.Items.Item("cbEstilo").Enabled = g_oChkEstilo.Checked;
                                break;
                            case "chkModelo":
                                FormularioSBO.Items.Item("txtNoSol").Click();
                                FormularioSBO.Items.Item("cbModelo").Enabled = g_oChkModelo.Checked;
                                break;
                            case "chkStatus":
                                FormularioSBO.Items.Item("txtNoSol").Click();
                                FormularioSBO.Items.Item("cbStatus").Enabled = g_oChkEstado.Checked;
                                break;
                        }
                    }
                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        public void ManejadorEventoComboSelected(ItemEvent pVal, ref Boolean BubbleEvent)
        {
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.Item sboItem;
            SAPbouiCOM.ComboBox sboComboM;
            SAPbouiCOM.ComboBox sboComboE;
            SAPbouiCOM.DataTable dtActividadesIngCtrlCol;

            if (pVal.ActionSuccess)
            {
                switch (pVal.ItemUID)
                {
                    case "cbMarca":
                        sboComboM = (SAPbouiCOM.ComboBox)FormularioSBO.Items.Item("cbMarca").Specific;
                        CargaEstilos(sboComboM.Selected.Value.Trim());
                        break;
                    case "cbEstilo":
                        sboComboE = (SAPbouiCOM.ComboBox)FormularioSBO.Items.Item("cbEstilo").Specific;
                        CargaModelos(sboComboE.Selected.Value.Trim());
                        break;
                    case "cbModelo":
                        break;
                }
            }
        }

        public void ManejadorEventoLinkPress(ref SAPbouiCOM.ItemEvent pval, ref Boolean BubbleEvent, ref SolicitudEspecificos formSolEsp)
        {
            var SolID = string.Empty;
            SAPbouiCOM.Matrix oMatriz;

            oMatriz = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxListSoE").Specific;
            SolID = ((SAPbouiCOM.EditText)oMatriz.Columns.Item("ColDocE").Cells.Item(pval.Row).Specific).Value.ToString().Trim();

            formSolEsp.SolNum = SolID;
            formSolEsp.CargarFormulario(SolID);
        }

        #endregion

    }
}
