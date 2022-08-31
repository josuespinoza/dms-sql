using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class BuscadorProveedores
    {
        private UserDataSources UDS_CompraProveedores;
        public static EditTextSBO txtProveedorCode;
        public static EditTextSBO txtProveedorName;
        public  DocumentoCompra m_objDocCompra;

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
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable m_dtProveedores;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.EditText oEditText;
            
            int m_intPosicion = 0;
            string m_strCodigo = string.Empty;
            string m_strNombre = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                   
                        oForm = ApplicationSBO.Forms.Item(formUID);

                        if (pVal.BeforeAction)
                        {
                            
                        }
                        else if (pVal.ActionSuccess)
                        {
                            switch (pVal.ItemUID)
                            {
                                case "mtxProv":
                                    if (pVal.ColUID == "Col_sele" && pVal.Row > 0)
                                    {
                                        oMatrix = (SAPbouiCOM.Matrix) oForm.Items.Item(g_strmtxProveedores).Specific;
                                        oMatrix.FlushToDataSource();

                                        m_intPosicion = pVal.Row - 1;

                                        m_dtProveedores = oForm.DataSources.DataTables.Item(g_strdtProveedores);

                                        m_strCodigo = m_dtProveedores.GetValue("codi", m_intPosicion).ToString().Trim();
                                        m_strNombre = m_dtProveedores.GetValue("nomb", m_intPosicion).ToString().Trim();

                                        if (string.IsNullOrEmpty(m_strCodigo) == false)
                                        {
                                            DocumentoCompra.g_ProvCode = m_strCodigo;
                                            DocumentoCompra.g_ProvName = m_strNombre;
                                        }
                                        else
                                        {
                                            DocumentoCompra.g_ProvCode = string.Empty;
                                            DocumentoCompra.g_ProvName = string.Empty;
                                        }
                                    }
                                    break;
                                case "btnSel":

                                    m_objDocCompra.CargaProveedor();

                                    oForm.Items.Item("2").Click();
                                    break;
                                case "btnBuscar":

                                    AplicarFiltros(oForm);

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

        public void ManejadorEventoFormDataLoad(ItemEvent pVal, bool bubbleEvent)
        {
            try
            {
                if (pVal.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    FormularioSBO.Freeze(true);

                    CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                    CultureInfo cultureInfo = Resource.Culture;
                    DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    Resource.Culture = cultureInfo;
                    UDS_CompraProveedores = FormularioSBO.DataSources.UserDataSources;
                    UDS_CompraProveedores.Add("codep", BoDataType.dt_LONG_TEXT, 100);
                    UDS_CompraProveedores.Add("namep", BoDataType.dt_LONG_TEXT, 100);

                    //FormularioSBO.DataSources.UserDataSources.Add("provee", SAPbouiCOM.BoDataType.dt_LONG_TEXT);

                    //txt_Proveedor = (EditText)FormularioSBO.Items.Item("txtProv").Specific;
                    //txt_Proveedor.DataBind.SetBound(true, "", "provee");

                    txtProveedorCode = new EditTextSBO("txtCode", true, "", "codep", FormularioSBO);
                    txtProveedorCode.AsignaBinding();
                    txtProveedorName = new EditTextSBO("txtNomb", true, "", "namep", FormularioSBO);
                    txtProveedorName.AsignaBinding();

                    //ChooseFromListCollection oCFLs;
                    //ChooseFromList oCFL;
                    //ChooseFromListCreationParams oCFL_CreationParams;
                    //Conditions oCons;
                    //Condition oCon;

                    //oCFLs = FormularioSBO.ChooseFromLists;

                    //oCFL_CreationParams = (ChooseFromListCreationParams)ApplicationSBO.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);

                    //oCFL_CreationParams.MultiSelection = false;
                    //oCFL_CreationParams.ObjectType = "2";
                    //oCFL_CreationParams.UniqueID = "CFL_Pr";
                    //oCFL = oCFLs.Add(oCFL_CreationParams);

                    //oCons = oCFL.GetConditions();
                    //oCon = oCons.Add();
                    //oCon.Alias = "CardType";
                    //oCon.Operation = BoConditionOperation.co_EQUAL;
                    //oCon.CondVal = "P";

                    //oCFL.SetConditions(oCons);

                    //txt_Proveedor = (EditText)FormularioSBO.Items.Item("txtProv").Specific;

                    //txt_Proveedor.ChooseFromListUID = "CFL_Pr";
                    //txt_Proveedor.ChooseFromListAlias = "CardName";

                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void AplicarFiltros(Form oForm)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtTabla;
            string m_strConsulta = string.Empty;
            string m_strConsultaFiltros = string.Empty;
            string m_strCode = string.Empty;
            string m_strName = string.Empty;
            bool m_blnCode = false;
            bool m_blnName = false;

            try
            {
                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxProveedores).Specific;
                oMatrix.FlushToDataSource();

                dtTabla = oForm.DataSources.DataTables.Item(g_strdtProveedores);

                m_strCode = txtProveedorCode.ObtieneValorUserDataSource();
                m_strCode = m_strCode.Trim();

                if (string.IsNullOrEmpty(m_strCode) == false)
                {
                    m_blnCode = true;
                }

                m_strName = txtProveedorName.ObtieneValorUserDataSource();
                m_strName = m_strName.Trim();

                if (string.IsNullOrEmpty(m_strName) == false)
                {
                    m_blnName = true;
                }

                m_strConsulta = g_strConsultaFiltros;

                if (m_blnCode)
                {
                    m_strConsultaFiltros = string.Format(g_strConsultaFiltrosCode, m_strCode);

                    m_strConsulta = string.Format("{0} {1}", m_strConsulta, m_strConsultaFiltros);
                }

                if (m_blnName)
                {
                    m_strConsultaFiltros = string.Format(g_strConsultaFiltrosName, m_strName);

                    m_strConsulta = string.Format("{0} {1}", m_strConsulta, m_strConsultaFiltros);
                }

                dtTabla.ExecuteQuery(m_strConsulta);
                oMatrix.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

    }
}
