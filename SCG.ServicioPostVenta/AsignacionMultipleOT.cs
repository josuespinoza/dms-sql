using System;
using System.Collections.Generic;
using System.Linq;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;
using SAPbouiCOM;
using SCG.ServicioPostVenta.DataContract.Orden_de_Trabajo;

namespace SCG.ServicioPostVenta
{
    public partial class AsignacionMultipleOT
    {
        private UserDataSources UDS_SeleccionaRepuestos;
        public static CheckBoxSBO chkSelTo;
        public static ComboBoxSBO cboColab;
        //public static ComboBoxSBO cboFases;
        private static int intDocentry;

        public void ManejadorEventoFormDataLoad(string strConfigUniMec, int p_intDocEntry)
        {
            ComboBox m_objCombo;
            Matrix oMatrix;
            try
            {
                FormularioSBO.Freeze(true);
                intDocentry = p_intDocEntry;
                UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources;
                UDS_SeleccionaRepuestos.Add("selTo", BoDataType.dt_LONG_TEXT, 100);
                UDS_SeleccionaRepuestos.Add("colab", BoDataType.dt_LONG_TEXT, 100);
                UDS_SeleccionaRepuestos.Add("AsigUniMec", BoDataType.dt_LONG_TEXT, 1);

                chkSelTo = new CheckBoxSBO("chkSelTo", true, "", "selTo", FormularioSBO);
                chkSelTo.AsignaBinding();
                cboColab = new ComboBoxSBO("cboColabor", FormularioSBO, true, "", "colab");
                cboColab.AsignaBinding();

                //cboFases = new ComboBoxSBO("cboFas", FormularioSBO, true, "", "Fases");
                //cboFases.AsignaBinding();

                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboColabor").Specific;

                ConfgUniMec = strConfigUniMec == "Y";
                UDS_SeleccionaRepuestos.Item("AsigUniMec").Value = ConfgUniMec ? "Y" : "N";
                if (m_objCombo.ValidValues.Count > 0)
                {
                    m_objCombo.Select(0, BoSearchKey.psk_Index);
                }
                else
                    CargarMatriz(FormularioSBO.UniqueID);

                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                throw;// Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void CargarMatriz(String FormUID, String strIdEmp = null)
        {
            List<ControlColaborador> controlColaborador = DevuelveOTUltimasLineas(OrdenTrabajo.NoOT);
            string strIdActCtrlColab = string.Empty;
            string strIdActCot = string.Empty;
            string strIdMecColab = string.Empty;
            string strEstadoActividad = string.Empty;
            string strConsuta;
            int intCount;
            string m_strPermReasignarFinalizada = string.Empty;
            string m_strAgreMecNEstadoAct = string.Empty;
            string strIdSucursal = string.Empty;
            SAPbouiCOM.Matrix oMatrizServAsignados;
            SAPbouiCOM.Form oForm;
            oForm = ApplicationSBO.Forms.Item(FormUID);
            UDS_SeleccionaRepuestos = oForm.DataSources.UserDataSources;

            if (!String.IsNullOrEmpty(OrdenTrabajo.NoOT))
            {
                strConsuta = g_strConsultaAsignacion;

                //Traemos el código de la sucursal
                if (intBranch != null)
                {
                    //Traemos la información del check para determinar si se permite o no traer actividades finalizadas
                    //Y = Si se permite, N = No se permite
                    m_strPermReasignarFinalizada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == intBranch.ToString()).U_AgrgTiempFin.Trim();
                    m_strAgreMecNEstadoAct = DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == intBranch.ToString()).U_AddMecNEstado.Trim();
                }

                //Valida si usa Agregar Mecanico independientemente del Estado
                if (m_strAgreMecNEstadoAct.ToUpper() == "Y")
                {
                    strConsuta = string.Format(strConsuta, intDocentry, ",2,4");
                }
                else 
                {
                    if (!string.IsNullOrEmpty(m_strPermReasignarFinalizada))
                    {
                        //Modifica el query para obtener también las actividades finalizadas
                        if (m_strPermReasignarFinalizada.ToUpper() == "Y")
                        {
                            strConsuta = string.Format(strConsuta, intDocentry, ",4");
                        }
                        else
                        {
                            strConsuta = string.Format(strConsuta, intDocentry, string.Empty);
                        }
                    }
                    else
                    {
                        strConsuta = string.Format(strConsuta, intDocentry, string.Empty);
                    }
                }
                
                
                g_dtAsignaciones.ExecuteQuery(strConsuta);
                if (string.IsNullOrEmpty(strIdEmp))
                {
                    if (UDS_SeleccionaRepuestos.Item("AsigUniMec").Value == "Y")
                    {
                        for (int index = g_dtAsignaciones.Rows.Count; index >= 1; index--)
                        {
                            for (int index2 = controlColaborador.Count - 1; index2 >= 0; index2--)
                            {
                                strIdMecColab = controlColaborador[index2].U_Colab.Trim();
                                strIdActCtrlColab = controlColaborador[index2].U_IdAct.Trim();
                                strEstadoActividad = controlColaborador[index2].U_Estad.Trim();
                                if (strIdMecColab.Equals(strIdEmp) && strIdActCot.Equals(strIdActCtrlColab))
                                {
                                    if (!strEstadoActividad.Equals("3"))
                                    {
                                        g_dtAsignaciones.Rows.Remove(index - 1);
                                        controlColaborador.Remove(controlColaborador[index2]);
                                    }
                                    break;
                                }
                                if (strIdActCot.Equals(strIdActCtrlColab))
                                {
                                    g_dtAsignaciones.Rows.Remove(index - 1);
                                    controlColaborador.Remove(controlColaborador[index2]);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (UDS_SeleccionaRepuestos.Item("AsigUniMec").Value == "Y")
                    {
                        for (int index = g_dtAsignaciones.Rows.Count; index >= 1; index--)
                        {
                            strIdActCot = g_dtAsignaciones.GetValue("idac", index - 1).ToString().Trim();
                            for (int index2 = controlColaborador.Count - 1; index2 >= 0; index2--)
                            {
                                strIdMecColab = controlColaborador[index2].U_Colab.Trim();
                                strIdActCtrlColab = controlColaborador[index2].U_IdAct.Trim();
                                strEstadoActividad = controlColaborador[index2].U_Estad.Trim();
                                if (strIdMecColab.Equals(strIdEmp) && strIdActCot.Equals(strIdActCtrlColab))
                                {
                                    if (!strEstadoActividad.Equals("3"))
                                    {
                                        g_dtAsignaciones.Rows.Remove(index - 1);
                                        controlColaborador.Remove(controlColaborador[index2]);
                                    }
                                    break;
                                }
                                if (strIdActCot.Equals(strIdActCtrlColab))
                                {
                                    g_dtAsignaciones.Rows.Remove(index - 1);
                                    controlColaborador.Remove(controlColaborador[index2]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int index = g_dtAsignaciones.Rows.Count; index >= 1; index--)
                        {
                            strIdActCot = g_dtAsignaciones.GetValue("idac", index - 1).ToString().Trim();
                            for (int index2 = controlColaborador.Count - 1; index2 >= 0; index2--)
                            {
                                strIdActCtrlColab = controlColaborador[index2].U_IdAct.Trim();
                                strIdMecColab = controlColaborador[index2].U_Colab.Trim();
                                strEstadoActividad = controlColaborador[index2].U_Estad.Trim();
                                if (strIdActCot == strIdActCtrlColab && strIdMecColab == strIdEmp)
                                {
                                    if (!strEstadoActividad.Equals("3"))
                                    {
                                        g_dtAsignaciones.Rows.Remove(index - 1);
                                        controlColaborador.Remove(controlColaborador[index2]);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                oMatrizServAsignados = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxTareas).Specific;
                oMatrizServAsignados.LoadFromDataSource();
            }
        }

        private List<ControlColaborador> DevuelveOTUltimasLineas(string p_strNumeroOT)
        {
            OrdenDeTrabajo ordenDeTrabajo;
            List<ControlColaborador> controlColaborador;
            List<string> ltIdActividad;
            string strIDActividad;
            ltIdActividad = new List<string>();
            controlColaborador = new List<ControlColaborador>();
            ordenDeTrabajo = Carga_OT.Carga_OrdenDeTrabajo((SAPbobsCOM.Company)CompanySBO, p_strNumeroOT);
            if (ordenDeTrabajo.ControlColaborador != null)
            {
                controlColaborador = ordenDeTrabajo.ControlColaborador;

                for (int index = controlColaborador.Count - 1; index >= 0; index--)
                {
                    strIDActividad = string.Format("{0}{1}", controlColaborador[index].U_IdAct.Trim(), controlColaborador[index].U_Colab.Trim());
                    if (ltIdActividad.Contains(strIDActividad))
                        controlColaborador.Remove(controlColaborador[index]);
                    else
                        ltIdActividad.Add(strIDActividad);
                }
            }
            return controlColaborador;
        }

        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent);
                    break;
                case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                    ManejadorEventoComboSelected(FormUID, pVal, ref BubbleEvent);
                    break;
            }
        }

        public void ManejadorEventoComboSelected(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.Item sboItem;
            SAPbouiCOM.ComboBox sboCombo;
            SAPbouiCOM.DataTable dtActividadesIngCtrlCol;

            oForm = ApplicationSBO.Forms.Item(FormUID);
            if (pVal.ActionSuccess)
            {
                oForm.Freeze(true);

                //Se setea el check de marcatodo siempre como NO cuando se haga el cambio de mecanico
                chkSelTo.AsignaValorUserDataSource("N");

                switch (pVal.ItemUID)
                {
                    case g_strCboColab:
                        if (!String.IsNullOrEmpty(cboColab.ObtieneValorUserDataSource()))
                        {
                            dtActividadesIngCtrlCol = oForm.DataSources.DataTables.Item(g_strdtActividadesIngCtrlCol);
                            dtActividadesIngCtrlCol.Rows.Clear();
                            CargarMatriz(FormUID, cboColab.ObtieneValorUserDataSource().Trim());
                        }
                        else
                        {
                            LimpiarMatriz(FormUID);
                        }
                        break;
                }
                oForm.Freeze(false);
            }
        }

        private void LimpiarMatriz(String FormUID)
        {
            SAPbouiCOM.Matrix oMatrizServAsignados;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.CheckBox oCheckBox;

            oForm = ApplicationSBO.Forms.Item(FormUID);

            SeleccionarTodasActividadesAsignar(oForm);

            g_dtAsignaciones.Rows.Clear();
            oMatrizServAsignados = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxTareas).Specific;
            oMatrizServAsignados.LoadFromDataSource();
        }

        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtActividades;
            SAPbouiCOM.DataTable dtActividadesIngCtrlCol;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.EditText oEditText;
            SAPbouiCOM.CheckBox oCheckBox;
            bool blnValidaMec = false;
            bool blnValidaAct = false;

            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnAsi":

                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxTareas).Specific;
                                oMatrix.FlushToDataSource();

                                //Valida que se haya seleccionado una actividad
                                if (!String.IsNullOrEmpty(cboColab.ObtieneValorUserDataSource()))
                                {
                                    blnValidaMec = true;
                                }

                                for (int i = 0; i < oMatrix.RowCount; i++)
                                {
                                    oCheckBox = (CheckBox)oMatrix.Columns.Item("Col_sele").Cells.Item(i + 1).Specific;
                                    if (oCheckBox.Checked)
                                    {
                                        blnValidaAct = true;
                                        break;
                                    }
                                }

                                if (!blnValidaMec || !blnValidaAct)
                                {
                                    ApplicationSBO.StatusBar.SetText(Resource.ErrorDebeEscogerMecanico, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                    bubbleEvent = false;
                                }

                                break;
                        }
                    }

                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "chkSelTo":
                                SeleccionarTodasActividadesAsignar(oForm);
                                break;
                            case "mtxTareas":

                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxTareas).Specific;
                                oMatrix.FlushToDataSource();

                                if (pVal.ColUID == "Col_sele" && pVal.Row > 0)
                                {
                                    dtActividades = oForm.DataSources.DataTables.Item(g_strdtAsignacion);
                                    dtActividadesIngCtrlCol = oForm.DataSources.DataTables.Item(g_strdtActividadesIngCtrlCol);
                                    if (pVal.Row - 1 <= dtActividades.Rows.Count - 1)
                                    {
                                        oCheckBox = (CheckBox)oMatrix.Columns.Item("Col_sele").Cells.Item(pVal.Row).Specific;

                                        if (oCheckBox.Checked)
                                        {
                                            SeleccionarActividadAsignar(ref dtActividades, ref dtActividadesIngCtrlCol, pVal.Row - 1);
                                        }
                                        else
                                        {
                                            string IdActividad = dtActividades.GetValue("idac", pVal.Row - 1).ToString().Trim();
                                            EliminarActividadAsignar(ref dtActividadesIngCtrlCol, IdActividad);
                                        }
                                    }
                                }

                                break;
                            case "btnAsi":
                                AsignarActividades(oForm);
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

        private void SeleccionarActividadAsignar(ref DataTable p_dtActividades, ref DataTable p_dtActividadesIngCtrlCol, int p_intPosicion)
        {
            int intTamano = p_dtActividadesIngCtrlCol.Rows.Count;

            string Code = p_dtActividades.GetValue("codi", p_intPosicion).ToString().Trim();
            string Descripcion = p_dtActividades.GetValue("desc", p_intPosicion).ToString().Trim();
            string CodFase = p_dtActividades.GetValue("cfas", p_intPosicion).ToString().Trim();
            string NoFase = p_dtActividades.GetValue("fase", p_intPosicion).ToString().Trim();
            string IdActividad = p_dtActividades.GetValue("idac", p_intPosicion).ToString().Trim();
            string TiempoEstandar = p_dtActividades.GetValue("dura", p_intPosicion).ToString().Trim();

            p_dtActividadesIngCtrlCol.Rows.Add(1);
            p_dtActividadesIngCtrlCol.SetValue("code", intTamano, Code);
            p_dtActividadesIngCtrlCol.SetValue("desc", intTamano, Descripcion);
            p_dtActividadesIngCtrlCol.SetValue("esta", intTamano, "1");
            p_dtActividadesIngCtrlCol.SetValue("nofa", intTamano, NoFase);
            p_dtActividadesIngCtrlCol.SetValue("idac", intTamano, IdActividad);
            p_dtActividadesIngCtrlCol.SetValue("dura", intTamano, TiempoEstandar);
            p_dtActividadesIngCtrlCol.SetValue("cfas", intTamano, CodFase);
        }

        private void SeleccionarTodasActividadesAsignar(Form oForm)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtActividades;
            SAPbouiCOM.DataTable dtActividadesIngCtrlCol;

            oForm.Freeze(true);
            oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxTareas).Specific;
            oMatrix.FlushToDataSource();

            dtActividades = oForm.DataSources.DataTables.Item(g_strdtAsignacion);
            dtActividadesIngCtrlCol = oForm.DataSources.DataTables.Item(g_strdtActividadesIngCtrlCol);

            if (chkSelTo.ObtieneValorUserDataSource() == "Y")
            {
                for (int i = 0; i < dtActividades.Rows.Count; i++)
                {
                    if (dtActividades.GetValue("sele", i).ToString().Trim() != "Y")
                    {
                        dtActividades.SetValue("sele", i, "Y");
                        SeleccionarActividadAsignar(ref dtActividades, ref dtActividadesIngCtrlCol, i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtActividades.Rows.Count; i++)
                {
                    if (dtActividades.GetValue("sele", i).ToString().Trim() == "Y")
                    {
                        dtActividades.SetValue("sele", i, "N");
                        string IdActividad = dtActividades.GetValue("idac", i).ToString().Trim();
                        EliminarActividadAsignar(ref dtActividadesIngCtrlCol, IdActividad);
                    }
                }
            }
            oMatrix.LoadFromDataSource();
            oForm.Freeze(false);
        }

        private void EliminarActividadAsignar(ref DataTable p_dtControlColaborador, string p_strIDAct)
        {
            int intTamano = p_dtControlColaborador.Rows.Count;

            for (int i = 0; i <= intTamano - 1; i++)
            {
                if (p_dtControlColaborador.GetValue("idac", i).ToString().Trim() == p_strIDAct)
                {
                    p_dtControlColaborador.Rows.Remove(i);
                    break;
                }
            }
        }

        private void AsignarActividades(SAPbouiCOM.Form oForm)
        {
            SAPbouiCOM.DataTable dtControlColaborador;
            string m_strColaborador = string.Empty;
            //string m_strFase = string.Empty;

            try
            {
                dtControlColaborador = oForm.DataSources.DataTables.Item(g_strdtActividadesIngCtrlCol);
                //m_strFase =  cboFases.ObtieneValorUserDataSource();
                m_strColaborador = cboColab.ObtieneValorUserDataSource();
                this.OrdenTrabajo.AgregaActividadesDesdeAsignacion(dtControlColaborador, m_strColaborador, ApplicationSBO);

                oForm.Close();

                ApplicationSBO.StatusBar.SetText(Resource.AsignaciónActividades, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
    }
}
