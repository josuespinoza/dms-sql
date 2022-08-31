using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using CheckBox = SAPbouiCOM.CheckBox;
using Form = SAPbouiCOM.Form;

namespace SCG.ServicioPostVenta
{
    public partial class FinalizaActividad
    {
        private UserDataSources UDS_FinAct;
        public static SAPbouiCOM.EditText txtColab;
        public static SAPbouiCOM.EditText txtAct;
        public static SAPbouiCOM.EditText txtFechaIn;
        public static SAPbouiCOM.EditText txtFechaFin;
        public static SAPbouiCOM.EditText txtHorIni;
        public static SAPbouiCOM.EditText txtHorFin;
        public static SAPbouiCOM.EditText txtMinutos;
        public static SAPbouiCOM.EditText txtAux;
        public static CheckBoxSBO chkRango;
        public static CheckBoxSBO chkMinutos;
        public static CheckBoxSBO chkInicio;
        public static CheckBoxSBO chkFinal;
        public static string gstrUsaTiempoEstandar, gstrUsaTiempoReal;


        /// <summary>
        /// Form Dataload
        /// </summary>
        public void ManejadorEventoFormDataLoad()
        {

            try
            {
                UDS_FinAct = FormularioSBO.DataSources.UserDataSources;
                UDS_FinAct.Add("chkRan", BoDataType.dt_LONG_TEXT, 100);
                UDS_FinAct.Add("chkMin", BoDataType.dt_LONG_TEXT, 100);
                UDS_FinAct.Add("chkIni", BoDataType.dt_LONG_TEXT, 100);
                UDS_FinAct.Add("chkFin", BoDataType.dt_LONG_TEXT, 100);
                chkRango = new CheckBoxSBO("chkRanHor", true, "", "chkRan", FormularioSBO);
                chkRango.AsignaBinding();
                chkMinutos = new CheckBoxSBO("chkMin", true, "", "chkMin", FormularioSBO);
                chkMinutos.AsignaBinding();
                chkInicio = new CheckBoxSBO("chkInicio", true, "", "chkIni", FormularioSBO);
                chkInicio.AsignaBinding();
                chkFinal = new CheckBoxSBO("chkFinal", true, "", "chkFin", FormularioSBO);
                chkFinal.AsignaBinding();

                txtMinutos = (EditText)FormularioSBO.Items.Item("txtMin").Specific;
                txtHorFin = (EditText)FormularioSBO.Items.Item("txtHorFi").Specific;
                txtHorIni = (EditText)FormularioSBO.Items.Item("txtHorIni").Specific;
                txtFechaFin = (EditText)FormularioSBO.Items.Item("txtFeFi").Specific;
                txtFechaIn = (EditText)FormularioSBO.Items.Item("txtFeIni").Specific;
                txtColab = (EditText)FormularioSBO.Items.Item("txtTec").Specific;
                txtAct = (EditText)FormularioSBO.Items.Item("txtAct").Specific;
                txtAux = (EditText)FormularioSBO.Items.Item("txtAux").Specific;

                if (!string.IsNullOrEmpty(idSucursal))
                {
                    gstrUsaTiempoEstandar = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == idSucursal).U_TiempoEst_C.Trim();
                    gstrUsaTiempoReal = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == idSucursal).U_TiempoReal_C.Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AsignaValoresTxtFinalizaAct(string p_strNom, string p_strDes)
        {
            txtColab.Value = p_strNom;
            txtAct.Value = p_strDes;
        }

        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_CLOSE:
                    if (pVal.BeforeAction)
                        ApplicationSBO.Forms.ActiveForm.Mode = BoFormMode.fm_OK_MODE;
                    break;
            }
        }

        private void ValidaFechas(ref bool bubbleEvent)
        {
            int intTime;
            string strTime;
            string strDate;
            DateTime dtFechaIni;
            DateTime dtFechaFin;

            if (chkRango.Especifico.Checked)
            {
                if (chkInicio.Especifico.Checked && chkFinal.Especifico.Checked)
                {
                    strTime = txtHorIni.Value;
                    if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                    strDate = txtFechaIn.Value;
                    if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                    {
                        dtFechaIni = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);

                        strTime = txtHorFin.Value;
                        if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                        strDate = txtFechaFin.Value;
                        if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                        {
                            dtFechaFin = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);

                            if (dtFechaFin <= dtFechaIni)
                            {
                                ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinMenor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                            }
                            else if (AsigUniMec == "Y" && EstadoAct != "2")
                            {
                                if (OrdenTrabajo.ObtieneOcupacionMecanico(strCodeEmp) > 0)
                                {
                                    ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciadaControl, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                    bubbleEvent = false;
                                }
                            }
                        }
                        else
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinIncorrecta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            bubbleEvent = false;
                        }

                    }
                    else
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaInicioIncorrecta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        bubbleEvent = false;
                    }

                }
                else if (chkInicio.Especifico.Checked)
                {
                    strTime = txtHorIni.Value;
                    if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                    strDate = txtFechaIn.Value;
                    if (string.IsNullOrEmpty(strTime) || string.IsNullOrEmpty(strDate))
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaInicioIncorrecta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        bubbleEvent = false;
                    }
                    else if (AsigUniMec == "Y" && EstadoAct != "2")
                    {
                        if (OrdenTrabajo.ObtieneOcupacionMecanico(strCodeEmp) > 0)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciadaControl, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            bubbleEvent = false;
                        }
                    }
                }
                else if (chkFinal.Especifico.Checked)
                {
                    if (strHoraIni.Length > 3)
                        dtFechaIni = new DateTime(Convert.ToInt32(strFechaIni.Substring(0, 4)), Convert.ToInt32(strFechaIni.Substring(4, 2).ToString()), Convert.ToInt32(strFechaIni.Substring(6, 2).ToString()), Convert.ToInt32(strHoraIni.Substring(0, 2)), Convert.ToInt32(strHoraIni.Substring(2, 2)), 00);
                    else
                        dtFechaIni = DateTime.Now;

                    strTime = txtHorFin.Value;
                    if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                    strDate = txtFechaFin.Value;
                    if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                    {
                        dtFechaFin = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);

                        if (dtFechaFin <= dtFechaIni)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinMenor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            bubbleEvent = false;
                        }
                        else if (AsigUniMec == "Y" && EstadoAct != "2")
                        {
                            if (OrdenTrabajo.ObtieneOcupacionMecanico(strCodeEmp) > 0)
                            {
                                ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciadaControl, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                bubbleEvent = false;
                            }
                        }
                    }
                    else
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinIncorrecta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        bubbleEvent = false;
                    }

                }
                else
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ValidacionSeleccionOpcion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    bubbleEvent = false;
                }

            }
            else if (chkMinutos.Especifico.Checked)
            {
                if (!int.TryParse(txtMinutos.Value, out intTime))
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ValidacionMinutos, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    bubbleEvent = false;
                }
                else if (intTime <= 0)
                {
                    ApplicationSBO.StatusBar.SetText(Resource.ValidacionMinutosMenorCero, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    bubbleEvent = false;
                }
                else if (AsigUniMec == "Y" && EstadoAct != "2")
                {
                    if (OrdenTrabajo.ObtieneOcupacionMecanico(strCodeEmp) > 0)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorIniciarIniciadaControl, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        bubbleEvent = false;
                    }
                }

            }
            else
            {
                ApplicationSBO.StatusBar.SetText(Resource.ValidacionSeleccionOpcion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                bubbleEvent = false;
            }
        }

        /// <summary>
        /// Manejo ItemPress
        /// </summary>
        /// <param name="formUID"></param>
        /// <param name="pVal"></param>
        /// <param name="bubbleEvent"></param>
        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {

            SAPbouiCOM.Form oForm;

            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnAcep":
                                ValidaFechas(ref bubbleEvent);
                                break;
                        }
                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {

                            case "btnCan":
                                oForm.Close();
                                break;
                            case "chkRanHor":
                                ManejoCheck(pVal);
                                break;
                            case "chkMin":
                                ManejoCheck(pVal);
                                break;
                            case "chkInicio":
                                ManejoCheck(pVal);
                                break;
                            case "chkFinal":
                                ManejoCheck(pVal);
                                break;
                            case "btnAcep":
                                InsertaControlColaborador(oForm);
                                break;

                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserta Control Colaborador
        /// </summary>
        /// <param name="oForm"></param>
        private void InsertaControlColaborador(SAPbouiCOM.Form oForm)
        {
            try
            {
                if (chkRango.Especifico.Checked)
                {
                    ManejoInsertarRango(oForm);

                }
                else if (chkMinutos.Especifico.Checked)
                {
                    ManejoInsertaMinutos(oForm);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Manejo inserta Min
        /// </summary>
        private void ManejoInsertaMinutos(SAPbouiCOM.Form oForm)
        {
            SAPbobsCOM.GeneralDataCollection m_childs;
            SAPbobsCOM.GeneralDataParams m_oGeneralParams;
            SAPbobsCOM.CompanyService m_oCompanySercice;
            SAPbobsCOM.Documents m_objCotizacion;
            SAPbobsCOM.GeneralService m_GeneralService;
            SAPbobsCOM.GeneralData m_GenralData;
            SAPbobsCOM.Document_Lines m_oLineasCotizacion;
            DateTime m_Fechaini;
            DateTime m_Horaini;
            DateTime m_FechaFi;
            DateTime m_HoraFi;
            SAPbobsCOM.GeneralData m_childdata = null;
            int i;
            int j;
            int intError;
            string strError;
            int m_Minutos = 0;
            int m_MinutosSinSumatoria = 0;
            bool updateTime;
            double m_dblCostoReal;
            string strHora, strMinutos;
            string m_strMinutosGuardados = string.Empty;
            string strUsaTiempoEstandar = string.Empty, strUsaTiempoReal = string.Empty;
            double dblCostoActividad = 0;

            try
            {
                updateTime = false;
                m_objCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                m_objCotizacion.GetByKey(Convert.ToInt32(strDocEntry));
                m_oLineasCotizacion = m_objCotizacion.Lines;
                m_oCompanySercice = CompanySBO.GetCompanyService();
                m_GeneralService = m_oCompanySercice.GetGeneralService("SCGD_OT");
                m_oGeneralParams = (SAPbobsCOM.GeneralDataParams)m_GeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                m_oGeneralParams.SetProperty("Code", strNoOT);
                m_GenralData = m_GeneralService.GetByParams(m_oGeneralParams);
                m_childs = m_GenralData.Child("SCGD_CTRLCOL");
                m_Minutos = Convert.ToInt32(txtMinutos.Value);
                m_MinutosSinSumatoria = Convert.ToInt32(txtMinutos.Value);
                

                if (strHoraIni.Length > 3)
                    m_Fechaini = new DateTime(Convert.ToInt32(strFechaIni.Substring(0, 4)), Convert.ToInt32(strFechaIni.Substring(4, 2).ToString()), Convert.ToInt32(strFechaIni.Substring(6, 2)), Convert.ToInt32(strHoraIni.Substring(0, 2)), Convert.ToInt32(strHoraIni.Substring(2, 2)), 0);
                else
                    m_Fechaini = DateTime.Now;

                m_FechaFi = m_Fechaini.AddMinutes(m_Minutos);
                m_HoraFi = m_FechaFi;
                
                for (i = 0; i < m_oLineasCotizacion.Count; i++)
                {
                    m_oLineasCotizacion.SetCurrentLine(i);

                    if (strIDAct == m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                    {
                        
                        m_childdata = m_childs.Item(m_childs.Count - 1);
                        if (idlinea <= (int)m_childdata.GetProperty("LineId"))
                            for (j = 0; j < m_childs.Count; j++)
                            {
                                m_childdata = m_childs.Item(j);
                                if ((int)m_childdata.GetProperty("LineId") == idlinea)
                                    break;
                            }
                        else
                        {
                            
                            
                            m_childdata = m_childs.Add();
                            m_childdata.SetProperty("U_Colab", strCodeEmp);
                            m_childdata.SetProperty("U_IdAct", strIDAct);
                            m_childdata.SetProperty("U_NoFas", NoFase);
                            m_childdata.SetProperty("U_CodFas", CodFase);
                            strHora = DateTime.Now.Hour.ToString();
                            if (strHora.Length == 1) strHora = string.Format("0{0}", strHora);
                            strMinutos = DateTime.Now.Minute.ToString();
                            if (strMinutos.Length == 1) strMinutos = string.Format("0{0}", strMinutos);
                            strHora = string.Format("{0}:{1}", strHora, strMinutos);
                            m_childdata.SetProperty("U_FechPro", DateTime.Now);
                            m_childdata.SetProperty("U_HoraIni", strHora);
                        }
                        m_strMinutosGuardados = m_childdata.GetProperty("U_TMin").ToString();
                        
                        //Si la línea ya contenía minutos, se suman los minutos mas el tiempo que se le agrega
                        if (!string.IsNullOrEmpty(m_strMinutosGuardados))
                        {
                            int m_intMinutosGuardados = 0;
                            if (int.TryParse(m_strMinutosGuardados, out m_intMinutosGuardados))
                            {
                                //Sumamos los minutos de la línea mas los que le estamos agregando
                                m_Minutos += m_intMinutosGuardados;
                                m_FechaFi = m_Fechaini.AddMinutes(m_Minutos);
                                m_HoraFi = m_FechaFi;
                                
                            }
                        }
                        m_dblCostoReal = ObtieneCostosReal(strCodeEmp, m_Minutos);
                        m_childdata.SetProperty("U_DFIni", m_Fechaini);
                        m_childdata.SetProperty("U_HFIni", m_Fechaini);
                        m_childdata.SetProperty("U_DFFin", m_FechaFi);
                        m_childdata.SetProperty("U_HFFin", m_HoraFi);
                        m_childdata.SetProperty("U_TMin", m_Minutos);
                        m_childdata.SetProperty("U_CosRe", m_dblCostoReal);
                        m_childdata.SetProperty("U_Estad", "4");
                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EstAct").Value = "4";

                        if (gstrUsaTiempoEstandar == "Y")
                        {
                            dblCostoActividad = (double)(m_childdata.GetProperty("U_CosEst"));
                        }
                        else if (gstrUsaTiempoReal == "Y")
                        {
                            dblCostoActividad = m_dblCostoReal;
                        }

                        OrdenTrabajo.ActualizarActividadCotizacion(ref m_objCotizacion, strIDAct, "4", CodFase, dblCostoActividad, m_MinutosSinSumatoria);
                        OrdenTrabajo.ManejarEstadoOT(true, false, false, ref m_GenralData);
                        OrdenTrabajo.ManejarEstadoOT(false, true, false, ref m_GenralData);
                        updateTime = true;
                        break;
                    }

                }

                if (updateTime)
                {
                    CompanySBO.StartTransaction();
                    if (m_objCotizacion.Update() == 0)
                    {
                        m_GeneralService.Update(m_GenralData);
                        if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        ApplicationSBO.StatusBar.SetText(Resource.ActividadFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        OrdenTrabajo.recargarActividades(strNoOT, ApplicationSBO);
                        oForm.Mode = BoFormMode.fm_OK_MODE;
                        oForm.Close();
                    }
                    else
                    {
                        CompanySBO.GetLastError(out intError, out strError);
                        throw new Exception(string.Format("{0}: {1}", intError, strError));
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
        /// Inserta a la hora de checkear el rango
        /// </summary>
        /// <param name="p_objCotizacion"></param>
        /// <param name="p_oLineasCotizacion"></param>
        /// <param name="p_oGenralData"></param>
        /// <param name="p_Childs"></param>
        /// <param name="p_Childdata"></param>
        /// <param name="p_GeneralService"></param>
        private void ManejoInsertarRango(SAPbouiCOM.Form oform)
        {
            SAPbobsCOM.GeneralDataCollection m_childs;
            SAPbobsCOM.GeneralDataParams m_oGeneralParams;
            SAPbobsCOM.CompanyService m_oCompanySercice;
            SAPbobsCOM.Documents m_objCotizacion;
            SAPbobsCOM.GeneralService m_GeneralService;
            SAPbobsCOM.GeneralData m_GenralData;
            SAPbobsCOM.Document_Lines m_oLineasCotizacion;
            SAPbobsCOM.GeneralData m_childdata = null;

            string m_strFechaIni = string.Empty;
            string m_strFechaFin = string.Empty;
            string m_strHoraini = string.Empty;
            string m_strHorFin = string.Empty;
            DateTime m_Fechaini;
            DateTime m_Horaini;
            DateTime m_FechaFi;
            DateTime m_HoraFi;
            TimeSpan m_diferenciaMin;
            double m_Minutos = 0;
            double m_dblCostoReal;
            int intError = 0;
            string strError = string.Empty;
            int i;
            int j;
            int m_error = 0;
            bool updateTime;
            string strEstadoOT;
            string strHora, strMinutos;
            double dblCostoActividad = 0;

            try
            {
                strEstadoOT = string.Empty;
                updateTime = false;
                m_objCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                m_objCotizacion.GetByKey(Convert.ToInt32(strDocEntry));
                m_oLineasCotizacion = m_objCotizacion.Lines;

                m_oCompanySercice = CompanySBO.GetCompanyService();
                m_GeneralService = m_oCompanySercice.GetGeneralService("SCGD_OT");
                m_oGeneralParams = (SAPbobsCOM.GeneralDataParams)m_GeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                m_oGeneralParams.SetProperty("Code", strNoOT);
                m_GenralData = m_GeneralService.GetByParams(m_oGeneralParams);
                m_childs = m_GenralData.Child("SCGD_CTRLCOL");

                if (chkInicio.Especifico.Checked && chkFinal.Especifico.Checked)
                {
                    m_strFechaIni = txtFechaIn.Value;
                    m_strHoraini = txtHorIni.Value;
                    m_Fechaini = new DateTime(Convert.ToInt32(m_strFechaIni.Substring(0, 4)), Convert.ToInt32(m_strFechaIni.Substring(4, 2)), Convert.ToInt32(m_strFechaIni.Substring(6, 2)), Convert.ToInt32(m_strHoraini.Substring(0, 2)), Convert.ToInt32(m_strHoraini.Substring(2, 2)), 0);
                    m_Horaini = new DateTime(1900, 01, 01, Convert.ToInt32(m_strHoraini.Substring(0, 2)), Convert.ToInt32(m_strHoraini.Substring(2, 2)), 00);
                    m_strFechaFin = txtFechaFin.Value;
                    m_strHorFin = txtHorFin.Value;
                    m_FechaFi = new DateTime(Convert.ToInt32(m_strFechaFin.Substring(0, 4)), Convert.ToInt32(m_strFechaFin.Substring(4, 2)), Convert.ToInt32(m_strFechaFin.Substring(6, 2)), Convert.ToInt32(m_strHorFin.Substring(0, 2)), Convert.ToInt32(m_strHorFin.Substring(2, 2)), 0);
                    m_HoraFi = new DateTime(1900, 01, 01, Convert.ToInt32(m_strHorFin.Substring(0, 2)), Convert.ToInt32(m_strHorFin.Substring(2, 2)), 00);

                    m_diferenciaMin = m_FechaFi - m_Fechaini;

                    m_Minutos = m_diferenciaMin.TotalMinutes;
                    
                    for (i = 0; i < m_oLineasCotizacion.Count; i++)
                    {
                        m_oLineasCotizacion.SetCurrentLine(i);

                        if (strIDAct == m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                        {
                            m_childdata = m_childs.Item(m_childs.Count - 1);
                            if (idlinea <= (int)m_childdata.GetProperty("LineId"))
                                for (j = 0; j < m_childs.Count; j++)
                                {
                                    m_childdata = m_childs.Item(j);
                                    if ((int)m_childdata.GetProperty("LineId") == idlinea)
                                        break;
                                }
                            else
                            {
                                m_childdata = m_childs.Add();
                                m_childdata.SetProperty("U_Colab", strCodeEmp);
                                m_childdata.SetProperty("U_IdAct", strIDAct);
                                m_childdata.SetProperty("U_NoFas", NoFase);
                                m_childdata.SetProperty("U_CodFas", CodFase);
                                strHora = DateTime.Now.Hour.ToString();
                                if (strHora.Length == 1) strHora = string.Format("0{0}", strHora);
                                strMinutos = DateTime.Now.Minute.ToString();
                                if (strMinutos.Length == 1) strMinutos = string.Format("0{0}", strMinutos);
                                strHora = string.Format("{0}:{1}", strHora, strMinutos);
                                m_childdata.SetProperty("U_FechPro", DateTime.Now);
                                m_childdata.SetProperty("U_HoraIni", strHora);
                            }
                            m_dblCostoReal = ObtieneCostosReal(strCodeEmp, m_Minutos);
                            m_childdata.SetProperty("U_DFIni", m_Fechaini);
                            m_childdata.SetProperty("U_HFIni", m_Horaini);
                            m_childdata.SetProperty("U_DFFin", m_FechaFi);
                            m_childdata.SetProperty("U_HFFin", m_HoraFi);
                            m_childdata.SetProperty("U_TMin", m_Minutos);
                            m_childdata.SetProperty("U_Estad", "4");
                            m_childdata.SetProperty("U_CosRe", m_dblCostoReal);
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EstAct").Value = "4";

                            if (gstrUsaTiempoEstandar == "Y")
                            {
                                dblCostoActividad = (double)(m_childdata.GetProperty("U_CosEst"));
                            }
                            else if (gstrUsaTiempoReal == "Y")
                            {
                                dblCostoActividad = m_dblCostoReal;
                            }

                            OrdenTrabajo.ActualizarActividadCotizacion(ref m_objCotizacion, strIDAct, "4", CodFase, dblCostoActividad, m_Minutos);
                            OrdenTrabajo.ManejarEstadoOT(true, false, false, ref m_GenralData);
                            OrdenTrabajo.ManejarEstadoOT(false, true, false, ref m_GenralData);
                            updateTime = true;
                            break;
                        }

                    }

                }
                else if (chkInicio.Especifico.Checked)
                {
                    m_strFechaIni = txtFechaIn.Value;
                    m_strHoraini = txtHorIni.Value;
                    m_Fechaini = new DateTime(Convert.ToInt32(m_strFechaIni.Substring(0, 4)), Convert.ToInt32(m_strFechaIni.Substring(4, 2)), Convert.ToInt32(m_strFechaIni.Substring(6, 2)), 0, 0, 0);
                    m_Horaini = new DateTime(1900, 01, 01, Convert.ToInt32(m_strHoraini.Substring(0, 2)), Convert.ToInt32(m_strHoraini.Substring(2, 2)), 0);
                    for (i = 0; i <= m_oLineasCotizacion.Count - 1; i++)
                    {
                        m_oLineasCotizacion.SetCurrentLine(i);

                        if (strIDAct == m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                        {
                            m_childdata = m_childs.Item(m_childs.Count - 1);
                            if (idlinea <= (int)m_childdata.GetProperty("LineId"))
                                for (j = 0; j <= m_childs.Count - 1; j++)
                                {
                                    m_childdata = m_childs.Item(j);
                                    if ((int)m_childdata.GetProperty("LineId") == idlinea)
                                        break;
                                }
                            else
                            {
                                m_childdata = m_childs.Add();
                                m_childdata.SetProperty("U_Colab", strCodeEmp);
                                m_childdata.SetProperty("U_IdAct", strIDAct);
                                m_childdata.SetProperty("U_NoFas", NoFase);
                                m_childdata.SetProperty("U_CodFas", CodFase);
                                strHora = DateTime.Now.Hour.ToString();
                                if (strHora.Length == 1) strHora = string.Format("0{0}", strHora);
                                strMinutos = DateTime.Now.Minute.ToString();
                                if (strMinutos.Length == 1) strMinutos = string.Format("0{0}", strMinutos);
                                strHora = string.Format("{0}:{1}", strHora, strMinutos);
                                m_childdata.SetProperty("U_FechPro", DateTime.Now);
                                m_childdata.SetProperty("U_HoraIni", strHora);

                            }
                            m_childdata.SetProperty("U_DFIni", m_Fechaini);
                            m_childdata.SetProperty("U_HFIni", m_Horaini);
                            m_childdata.SetProperty("U_Estad", "2");
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EstAct").Value = "2";
                            m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2";
                            OrdenTrabajo.ObtieneDescripcionEstado("2", ref strEstadoOT, null);
                            m_objCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = strEstadoOT;

                            OrdenTrabajo.ActualizarActividadCotizacion(ref m_objCotizacion, strIDAct, "2", CodFase);
                            OrdenTrabajo.ManejarEstadoOT(true, false, false, ref m_GenralData);
                            updateTime = true;
                            break;
                        }

                    }
                }
                else if (chkFinal.Especifico.Checked)
                {
                    if (strHoraIni.Length > 3)
                        m_Fechaini = new DateTime(Convert.ToInt32(strFechaIni.Substring(0, 4)), Convert.ToInt32(strFechaIni.Substring(4, 2).ToString()), Convert.ToInt32(strFechaIni.Substring(6, 2)), Convert.ToInt32(strHoraIni.Substring(0, 2)), Convert.ToInt32(strHoraIni.Substring(2, 2)), 0);
                    else
                        m_Fechaini = DateTime.Now;

                    m_strFechaFin = txtFechaFin.Value;
                    m_strHorFin = txtHorFin.Value;
                    m_FechaFi = new DateTime(Convert.ToInt32(m_strFechaFin.Substring(0, 4)), Convert.ToInt32(m_strFechaFin.Substring(4, 2)), Convert.ToInt32(m_strFechaFin.Substring(6, 2)), Convert.ToInt32(m_strHorFin.Substring(0, 2)), Convert.ToInt32(m_strHorFin.Substring(2, 2)), 0);
                    m_HoraFi = new DateTime(1900, 01, 01, Convert.ToInt32(m_strHorFin.Substring(0, 2)), Convert.ToInt32(m_strHorFin.Substring(2, 2)), 00);

                    m_diferenciaMin = m_FechaFi - m_Fechaini;

                    m_Minutos = m_diferenciaMin.TotalMinutes;

                    for (i = 0; i <= m_oLineasCotizacion.Count - 1; i++)
                    {
                        m_oLineasCotizacion.SetCurrentLine(i);

                        if (strIDAct == m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                        {
                            m_childdata = m_childs.Item(m_childs.Count - 1);
                            if (idlinea <= (int)m_childdata.GetProperty("LineId"))
                                for (j = 0; j <= m_childs.Count - 1; j++)
                                {
                                    m_childdata = m_childs.Item(j);
                                    if ((int)m_childdata.GetProperty("LineId") == idlinea)
                                        break;
                                }
                            else
                            {
                                m_childdata = m_childs.Add();
                                m_childdata.SetProperty("U_Colab", strCodeEmp);
                                m_childdata.SetProperty("U_IdAct", strIDAct);
                                m_childdata.SetProperty("U_NoFas", NoFase);
                                m_childdata.SetProperty("U_CodFas", CodFase);
                                strHora = DateTime.Now.Hour.ToString();
                                if (strHora.Length == 1) strHora = string.Format("0{0}", strHora);
                                strMinutos = DateTime.Now.Minute.ToString();
                                if (strMinutos.Length == 1) strMinutos = string.Format("0{0}", strMinutos);
                                strHora = string.Format("{0}:{1}", strHora, strMinutos);
                                m_childdata.SetProperty("U_FechPro", DateTime.Now);
                                m_childdata.SetProperty("U_HoraIni", strHora);
                            }
                            m_dblCostoReal = ObtieneCostosReal(strCodeEmp, m_Minutos);
                            m_childdata.SetProperty("U_DFFin", m_FechaFi);
                            m_childdata.SetProperty("U_HFFin", m_HoraFi);
                            m_childdata.SetProperty("U_TMin", m_Minutos);
                            m_childdata.SetProperty("U_Estad", "4");
                            m_childdata.SetProperty("U_CosRe", m_dblCostoReal);
                            if (string.IsNullOrEmpty(strHoraIni))
                            {
                                m_childdata.SetProperty("U_DFIni", m_Fechaini);
                                m_childdata.SetProperty("U_HFIni", m_Fechaini);
                            }
                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EstAct").Value = "4";

                            if (gstrUsaTiempoEstandar == "Y")
                            {
                                dblCostoActividad = (double)(m_childdata.GetProperty("U_CosEst"));
                            }
                            else if (gstrUsaTiempoReal == "Y")
                            {
                                dblCostoActividad = m_dblCostoReal;
                            }

                            OrdenTrabajo.ActualizarActividadCotizacion(ref m_objCotizacion, strIDAct, "4", CodFase, dblCostoActividad, m_Minutos);
                            OrdenTrabajo.ManejarEstadoOT(true, false, false, ref m_GenralData);
                            OrdenTrabajo.ManejarEstadoOT(false, true, false, ref m_GenralData);
                            updateTime = true;
                            break;
                        }

                    }

                }
                if (updateTime)
                {
                    CompanySBO.StartTransaction();
                    if (m_objCotizacion.Update() == 0)
                    {
                        m_GeneralService.Update(m_GenralData);
                        if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                        ApplicationSBO.StatusBar.SetText(Resource.ActividadFinalizada, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                        OrdenTrabajo.recargarActividades(strNoOT, ApplicationSBO);
                        oform.Mode = BoFormMode.fm_OK_MODE;
                        oform.Close();
                    }
                    else
                    {
                        CompanySBO.GetLastError(out intError, out strError);
                        throw new Exception(string.Format("{0}: {1}", intError, strError));
                    }
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction) CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        private double ObtieneCostosReal(string p_strColaborador, double p_dblMinutos)
        {
            string strConsulta = " select U_SCGD_sALXHORA as sal from OHEM with (nolock) where empID IN ({0}) ";
            double dblSalario = 0;
            double dblCostoActividad = 0;

            strConsulta = string.Format(strConsulta, p_strColaborador);
            g_dtConsulta.ExecuteQuery(strConsulta);

            dblSalario = double.Parse(g_dtConsulta.GetValue("sal", 0).ToString().Trim());

            dblCostoActividad = (p_dblMinutos / 60) * dblSalario;

            return dblCostoActividad;
        }

        /// <summary>
        /// Manejo del check
        /// </summary>
        /// <param name="p_formUid"></param>
        /// <param name="pVal"></param>
        private void ManejoCheck(ItemEvent pVal)
        {
            SAPbouiCOM.Item item;
            try
            {
                switch (pVal.ItemUID)
                {
                    case "chkInicio":
                        if (chkInicio.Especifico.Checked)
                        {
                            FormularioSBO.Items.Item("txtMin").Enabled = false;
                            FormularioSBO.Items.Item("txtHorIni").Enabled = true;
                            FormularioSBO.Items.Item("txtFeIni").Enabled = true;
                        }
                        else
                        {
                            txtHorIni.Value = "";
                            txtFechaIn.Value = "";
                            txtAux.Value = " ";
                            FormularioSBO.Items.Item("txtHorIni").Enabled = false;
                            FormularioSBO.Items.Item("txtFeIni").Enabled = false;

                        }
                        break;
                    case "chkFinal":

                        if (chkFinal.Especifico.Checked)
                        {
                            FormularioSBO.Items.Item("txtMin").Enabled = false;
                            FormularioSBO.Items.Item("txtHorFi").Enabled = true;
                            FormularioSBO.Items.Item("txtFeFi").Enabled = true;
                        }
                        else
                        {
                            txtHorFin.Value = "";
                            txtFechaFin.Value = "";
                            txtAux.Value = " ";
                            FormularioSBO.Items.Item("txtHorFi").Enabled = false;
                            FormularioSBO.Items.Item("txtFeFi").Enabled = false;
                        }
                        break;

                    case "chkRanHor":
                        txtMinutos.Value = "";
                        txtAux.Value = " ";
                        chkInicio.ItemSBO.Enabled = true;
                        chkFinal.ItemSBO.Enabled = true;
                        chkRango.AsignaValorUserDataSource("Y");
                        chkMinutos.AsignaValorUserDataSource("N");
                        FormularioSBO.Items.Item("txtMin").Enabled = false;
                        break;

                    case "chkMin":
                        FormularioSBO.Items.Item("txtMin").Enabled = true;
                        chkRango.AsignaValorUserDataSource("N");
                        chkMinutos.AsignaValorUserDataSource("Y");
                        txtFechaFin.Value = string.Empty;
                        txtHorIni.Value = string.Empty;
                        txtFechaIn.Value = string.Empty;
                        txtHorFin.Value = string.Empty;
                        chkInicio.AsignaValorUserDataSource("N");
                        chkFinal.AsignaValorUserDataSource("N");
                        txtAux.Value = " ";
                        chkInicio.ItemSBO.Enabled = false;
                        chkFinal.ItemSBO.Enabled = false;
                        FormularioSBO.Items.Item("txtHorFi").Enabled = false;
                        FormularioSBO.Items.Item("txtHorIni").Enabled = false;
                        FormularioSBO.Items.Item("txtFeFi").Enabled = false;
                        FormularioSBO.Items.Item("txtFeIni").Enabled = false;
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
