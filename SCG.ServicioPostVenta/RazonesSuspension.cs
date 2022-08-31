using System;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using CheckBox = SAPbouiCOM.CheckBox;
using ComboBox = SAPbouiCOM.ComboBox;
using Form = SAPbouiCOM.Form;

namespace SCG.ServicioPostVenta
{
    public partial class RazonesSuspension
    {
        private UserDataSources UDS_SeleccionaRepuestos;
        public static ComboBoxSBO cboRazonesSuspension;
        public static EditTextSBO txtComentarios;
        public Boolean SupendeOT { get; set; }
        public static CheckBoxSBO chkTFin;
        public static SAPbouiCOM.EditText txtFFin;
        public static SAPbouiCOM.EditText txtHFin;
        public static SAPbouiCOM.CheckBox chkTFin2;

        public void ManejadorEventoFormLoad(bool p_suspendeOT = false)
        {
            ComboBox m_objCombo;
            SAPbouiCOM.DataTable m_dtConsultaCombos;

            try
            {
                //FormularioSBO.Freeze(true);
                SupendeOT = p_suspendeOT;
                UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources;
                UDS_SeleccionaRepuestos.Add("TFin", BoDataType.dt_LONG_TEXT, 100);
                UDS_SeleccionaRepuestos.Add("come", BoDataType.dt_LONG_TEXT, 100);
                UDS_SeleccionaRepuestos.Add("razo", BoDataType.dt_LONG_TEXT, 100);

                chkTFin = new CheckBoxSBO("chkTFin", true, "", "TFin", FormularioSBO);
                chkTFin.AsignaBinding();
                txtComentarios = new EditTextSBO("txtComent", true, "", "come", FormularioSBO);
                txtComentarios.AsignaBinding();
                cboRazonesSuspension = new ComboBoxSBO("cboRaz", FormularioSBO, true, "", "razo");
                cboRazonesSuspension.AsignaBinding();

                txtFFin = (EditText)FormularioSBO.Items.Item("txtFFin").Specific;
                txtHFin = (EditText)FormularioSBO.Items.Item("txtHFin").Specific;
                chkTFin2 = (CheckBox)FormularioSBO.Items.Item("chkTFin").Specific;

                if (OrdenTrabajo.OcultarCamposFechaSuspencio())
                {
                    txtFFin.Item.Enabled = false;
                    txtHFin.Item.Enabled = false;
                    chkTFin2.Item.Enabled = false;
                }

                m_dtConsultaCombos = FormularioSBO.DataSources.DataTables.Add("dtConsulCbo");

                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboRaz").Specific;

                Utilitarios.CargaComboBox("select Code, Name from [@SCGD_RAZSUSPEN] with (nolock)", "Code", "Name", m_dtConsultaCombos, ref m_objCombo, false);

                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;

            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
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

        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtActividades;
            SAPbouiCOM.DataTable dtActividadesIngCtrlCol;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.EditText oEditText;
            SAPbouiCOM.CheckBox oCheckBox;
            string m_strColaborador = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(formUID))
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnok":
                                string m_strRazon = string.Empty;

                                m_strRazon = cboRazonesSuspension.ObtieneValorUserDataSource();

                                if (string.IsNullOrEmpty(m_strRazon))
                                {
                                    ApplicationSBO.StatusBar.SetText(Resource.ErrorRazonSuspension, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                    bubbleEvent = false;
                                    return;
                                }
                                if (!SupendeOT)
                                    ValidaFechas(ref bubbleEvent);
                                break;
                        }
                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnok":
                                SuspendeActividad(oForm);
                                break;
                            case "chkTFin":
                                ManejoCheck(pVal);
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void SuspendeActividad(Form oForm)
        {
            SAPbouiCOM.DataTable dtControlColaborador;
            string m_strRazon = string.Empty;
            string m_strComentario = string.Empty;
            DateTime dtFechaFin;
            string m_strFechaFin = string.Empty;
            string m_strHorFin = string.Empty;

            try
            {
                m_strRazon = cboRazonesSuspension.ObtieneValorUserDataSource();
                m_strComentario = txtComentarios.ObtieneValorUserDataSource();
                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                if (chkTFin.Especifico.Checked)
                {
                    m_strFechaFin = txtFFin.Value;
                    m_strHorFin = txtHFin.Value;
                    dtFechaFin = new DateTime(Convert.ToInt32(m_strFechaFin.Substring(0, 4)), Convert.ToInt32(m_strFechaFin.Substring(4, 2)), Convert.ToInt32(m_strFechaFin.Substring(6, 2)), Convert.ToInt32(m_strHorFin.Substring(0, 2)), Convert.ToInt32(m_strHorFin.Substring(2, 2)), 0);
                    oForm.Close();
                    OrdenTrabajo.SuspenderActividad(m_strRazon, m_strComentario, ApplicationSBO, CompanySBO, dtFechaFin, SupendeOT);
                }
                else
                {
                    oForm.Close();
                    OrdenTrabajo.SuspenderActividad(m_strRazon, m_strComentario, ApplicationSBO, CompanySBO, DateTime.Now, SupendeOT);
                }
            }
            catch (Exception ex)
            {
                throw;//Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void ValidaFechas(ref bool bubbleEvent)
        {
            int intTime;
            string strTime;
            string strDate;
            DateTime dtFechaIni;
            DateTime dtFechaFin;

            if (chkTFin.Especifico.Checked)
            {
                strTime = strHoraIni;
                if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                strDate = strFechaIni;
                if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                {
                    dtFechaIni = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);

                    strTime = txtHFin.Value;
                    if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                    strDate = txtFFin.Value;
                    if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                    {
                        dtFechaFin = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);

                        if (dtFechaFin <= dtFechaIni)
                        {
                            ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinMenor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            bubbleEvent = false;
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
                    //ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaInicioIncorrecta, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    bubbleEvent = false;
                }
            }
            else
            {
                strTime = strHoraIni;
                if (strTime.Length == 3) strTime = string.Format("0{0}", strTime);
                strDate = strFechaIni;
                if (!string.IsNullOrEmpty(strTime) && !string.IsNullOrEmpty(strDate))
                {
                    dtFechaIni = new DateTime(Convert.ToInt32(strDate.Substring(0, 4)), Convert.ToInt32(strDate.Substring(4, 2)), Convert.ToInt32(strDate.Substring(6, 2)), Convert.ToInt32(strTime.Substring(0, 2)), Convert.ToInt32(strTime.Substring(2, 2)), 0);
                    if (DateTime.Now <= dtFechaIni)
                    {
                        ApplicationSBO.StatusBar.SetText(Resource.ValidacionFechaFinMenor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        bubbleEvent = false;
                    }
                }
                else
                    bubbleEvent = false;
            }

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
                    case "chkTFin":
                        if (chkTFin.Especifico.Checked)
                        {
                            FormularioSBO.Items.Item("txtHFin").Enabled = true;
                            FormularioSBO.Items.Item("txtFFin").Enabled = true;
                        }
                        else
                        {
                            txtComentarios.ItemSBO.Click();
                            FormularioSBO.Items.Item("txtHFin").Enabled = false;
                            FormularioSBO.Items.Item("txtFFin").Enabled = false;
                        }
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
