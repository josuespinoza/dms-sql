using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SAPbouiCOM;
using SAPbobsCOM;
using SCG.SBOFramework.UI;
using ChooseFromList = SAPbouiCOM.ChooseFromList;
using ComboBox = SAPbouiCOM.ComboBox;
using Company = SAPbobsCOM.Company;
using Form = SAPbouiCOM.Form;
using Items = SAPbobsCOM.Items;
using System.Timers;
//using DMS_Addon; 


namespace SCG.ServicioPostVenta
{
    public partial class DocumentoCompra
    {
        private UserDataSources UDS_DocumentoCompra;
        public static EditTextSBO txtProveedorCode;
        public static EditTextSBO txtComentarios;
        public static EditTextSBO txtProveedorName;
        public SAPbouiCOM.EditText txt_Proveedor;
        private static System.Timers.Timer oTimer;

        public void ManejadorEventoFormDataLoad(ItemEvent pVal, bool bubbleEvent, ref DataTable p_dtItemsSeleccionados, ref string idSucursal, TipoAdicional tipo, IForm formularioSbo)
        {
            try
            {
                if (pVal.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                    CultureInfo cultureInfo = Resource.Culture;
                    DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    Resource.Culture = cultureInfo;
                    FormularioSBO.Freeze(true);
                    g_oformOT = formularioSbo;
                    g_tipoAdicional = tipo;

                    UDS_DocumentoCompra = FormularioSBO.DataSources.UserDataSources;
                    UDS_DocumentoCompra.Add("codep", BoDataType.dt_LONG_TEXT, 100);
                    UDS_DocumentoCompra.Add("namep", BoDataType.dt_LONG_TEXT, 100);
                    UDS_DocumentoCompra.Add("Coment", BoDataType.dt_LONG_TEXT, 250);

                    txtProveedorCode = new EditTextSBO("txtCProv", true, "", "codep", FormularioSBO);
                    txtProveedorCode.AsignaBinding();
                    txtProveedorName = new EditTextSBO("txtProv", true, "", "namep", FormularioSBO);
                    txtProveedorName.AsignaBinding();
                    txtComentarios = new EditTextSBO("txtComen", true, "", "Coment", FormularioSBO);
                    txtComentarios.AsignaBinding();

                    CargaDataTableEnMatriz(p_dtItemsSeleccionados, idSucursal, tipo);

                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargaDataTableEnMatriz(DataTable p_dtRepuestosSeleccionados, string idSucursal, TipoAdicional tipo)
        {
            SAPbouiCOM.DataTable m_dtItemsSeleccionados;
            SAPbouiCOM.DataTable m_dtCantidades;
            SAPbouiCOM.Matrix oMatrix;
            int m_intPosicion = 0;

            string m_strCodigo = string.Empty;
            string m_strDescripcion = string.Empty;
            double m_dblCantidad = 0.0;
            string m_strAlmacen = string.Empty;
            double m_dblPrecio = 0.0;
            string m_strMoneda = string.Empty;
            string m_strIdArticulo = string.Empty;
            string m_strIdImpuestos = string.Empty;

            try
            {
                oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(g_strmtxDocCompra).Specific;
                oMatrix.FlushToDataSource();

                m_dtCantidades = FormularioSBO.DataSources.DataTables.Item(g_strdtCantidad);
                m_dtCantidades.Rows.Clear();

                m_dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtDocCompra);
                m_dtItemsSeleccionados.Rows.Clear();

                var query = string.Format(g_strConsultaIndicadorImpuestos, idSucursal);
                if (dtConsulta == null)
                {
                    dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                }
                dtConsulta.ExecuteQuery(query);
                if (dtConsulta.Rows.Count > 0)
                {
                    switch (tipo)
                    {
                        case TipoAdicional.Repuesto:
                            m_strIdImpuestos = dtConsulta.GetValue("CompraRep", 0).ToString();
                            break;
                        case TipoAdicional.Servicio:
                            m_strIdImpuestos = dtConsulta.GetValue("Servicios", 0).ToString();
                            break;
                        case TipoAdicional.ServicioExterno:
                            m_strIdImpuestos = dtConsulta.GetValue("CompraSE", 0).ToString();
                            break;
                        case TipoAdicional.Suministro:
                            m_strIdImpuestos = dtConsulta.GetValue("Suministros", 0).ToString();
                            break;
                        case TipoAdicional.Gastos:
                            m_strIdImpuestos = dtConsulta.GetValue("Gastos", 0).ToString();
                            break;
                    }
                }

                for (int i = 0; i <= p_dtRepuestosSeleccionados.Rows.Count - 1; i++)
                {
                    m_strCodigo = p_dtRepuestosSeleccionados.GetValue("code", i).ToString().Trim();
                    m_strDescripcion = p_dtRepuestosSeleccionados.GetValue("desc", i).ToString().Trim();
                    m_dblCantidad = double.Parse(p_dtRepuestosSeleccionados.GetValue("cant", i).ToString());
                    m_strAlmacen = p_dtRepuestosSeleccionados.GetValue("alma", i).ToString().Trim();
                    m_dblPrecio = double.Parse(p_dtRepuestosSeleccionados.GetValue("prec", i).ToString());
                    m_strMoneda = p_dtRepuestosSeleccionados.GetValue("mone", i).ToString().Trim();
                    m_strIdArticulo = p_dtRepuestosSeleccionados.GetValue("idit", i).ToString().Trim();

                    m_dtCantidades.Rows.Add(1);
                    m_dtCantidades.SetValue("Id", m_intPosicion, m_strIdArticulo);
                    m_dtCantidades.SetValue("Cant", m_intPosicion, m_dblCantidad);

                    m_dtItemsSeleccionados.Rows.Add(1);
                    m_dtItemsSeleccionados.SetValue("sele", m_intPosicion, "");
                    m_dtItemsSeleccionados.SetValue("code", m_intPosicion, m_strCodigo);
                    m_dtItemsSeleccionados.SetValue("desc", m_intPosicion, m_strDescripcion);
                    m_dtItemsSeleccionados.SetValue("cant", m_intPosicion, m_dblCantidad);
                    m_dtItemsSeleccionados.SetValue("alma", m_intPosicion, m_strAlmacen);
                    m_dtItemsSeleccionados.SetValue("prec", m_intPosicion, m_dblPrecio);
                    m_dtItemsSeleccionados.SetValue("mone", m_intPosicion, m_strMoneda);
                    m_dtItemsSeleccionados.SetValue("idit", m_intPosicion, m_strIdArticulo);
                    if (!string.IsNullOrEmpty(m_strIdImpuestos))
                        m_dtItemsSeleccionados.SetValue("tax", m_intPosicion, m_strIdImpuestos);

                    m_intPosicion++;

                }
                oMatrix.LoadFromDataSource();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent, ref SCG.ServicioPostVenta.OrdenTrabajo p_oFormOT)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent, ref p_oFormOT);
                    break;
                case BoEventTypes.et_CHOOSE_FROM_LIST:
                    ManejadorEventosChooseFromList(FormUID, pVal, ref BubbleEvent);
                    break;
            }
        }

        private void ManejadorEventosChooseFromList(string formUID, ItemEvent pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.IChooseFromListEvent oCFLEvento;
            oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pVal;
            SAPbouiCOM.DataTable oDataTable;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.Matrix oMatrix;
            string tax;
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            oForm = ApplicationSBO.Forms.Item(formUID);
            String sCFL_ID;
            sCFL_ID = oCFLEvento.ChooseFromListUID;
            SAPbouiCOM.ChooseFromList oCFL;
            oCFL = oForm.ChooseFromLists.Item(sCFL_ID);

            if (!oCFLEvento.BeforeAction)
            {
                oDataTable = oCFLEvento.SelectedObjects;
                switch (oCFLEvento.ColUID)
                {
                    case "Col_tax":
                        if (oCFLEvento.SelectedObjects != null)
                        {
                            tax = oDataTable.GetValue("Code", 0).ToString().Trim();
                            oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxDocCompra).Specific;
                            oMatrix.FlushToDataSource();
                            oForm.DataSources.DataTables.Item(g_strdtDocCompra).SetValue("tax", pVal.Row - 1, tax);
                            oMatrix.LoadFromDataSource();
                        }
                        break;
                }
            }
            else
            {
                switch (pVal.ItemUID)
                {

                    case "mtxDocCom":
                        switch (oCFLEvento.ColUID)
                        {
                            case "Col_tax":
                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxDocCompra).Specific;
                                oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                                if (!string.IsNullOrEmpty(((EditText)oMatrix.Columns.Item("Col_tax").Cells.Item(pVal.Row).Specific).Value))
                                {
                                    oCondition = oConditions.Add();
                                    oCondition.Alias = "Code";
                                    oCondition.CondVal = ((EditText)oMatrix.Columns.Item("Col_tax").Cells.Item(pVal.Row).Specific).Value;
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN;
                                    oCondition.Relationship = BoConditionRelationship.cr_AND;
                                }

                                //Dependiendo del tipo de Config Impuesto asigno la condicion
                                oCondition = oConditions.Add();
                               
                                if (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup == "Y")
                                {
                                    oCondition.BracketOpenNum = 1;
                                    oCondition.Alias = "Category";
                                    oCondition.CondVal = "I";
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                                    oCondition.BracketCloseNum = 1;

                                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                                    oCondition = oConditions.Add();
                                    oCondition.BracketOpenNum = 2;
                                    oCondition.Alias = "Locked";
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                                    oCondition.CondVal = "N";
                                    oCondition.BracketCloseNum = 2;
                                }
                                else
                                {
                                    oCondition.BracketOpenNum = 1;
                                    oCondition.Alias = "ValidForAP";
                                    oCondition.CondVal = "Y";
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                                    oCondition.BracketCloseNum = 1;

                                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                                    oCondition = oConditions.Add();
                                    oCondition.BracketOpenNum = 2;
                                    oCondition.Alias = "Lock";
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                                    oCondition.CondVal = "N";
                                    oCondition.BracketCloseNum = 2;
                                }

                                oCFL.SetConditions(oConditions);

                                break;
                        }
                        break;
                }
            }
        }

        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool BubbleEvent, ref SCG.ServicioPostVenta.OrdenTrabajo p_oFormOT)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtAdicionales;
            SAPbouiCOM.DataTable dtAdicionalesSeleccionados;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.EditText oEditText;
            SAPbouiCOM.CheckBox oCheckBox;
            string m_strDocEntry = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {

                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnCrear":
                                CreaDocumentoCompra(pVal, ref BubbleEvent, oForm, ref p_oFormOT);
                                break;
                        }
                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "mtxAdic":
                                break;
                            case "btnCrear":
                                CreaDocumentoCompra(pVal, ref BubbleEvent, oForm, ref p_oFormOT);
                                break;
                            case "btnSel":
                                CargarFormularioBuscadorProveedores(pVal, ref BubbleEvent);
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

        private void CreaDocumentoCompra(ItemEvent pval, ref bool bubbleEvent, Form p_oForm, ref SCG.ServicioPostVenta.OrdenTrabajo p_oFormOT)
        {

            SAPbobsCOM.Documents m_objDocumento;
            SAPbouiCOM.DataTable dtItemsSeleccionados;
            SAPbouiCOM.DataTable m_dtCatidad;
            SAPbouiCOM.DataTable m_dtBodegasXCentroCosto;
            SAPbobsCOM.Documents oDocCompra;
            bool m_bEsOrden = false;
            int error;
            string errorDes;
            string m_strDocNum = string.Empty;
            string m_strserie = string.Empty;
            SAPbouiCOM.Matrix oMatrix;
            string strMatriz = "mtxAdic";
            string m_strBodegaProceso = string.Empty;
            int m_intDocEntry = 0;
            SAPbobsCOM.Documents oCotizacion;
            string m_strItemCode = string.Empty;
            string m_strCentroCosto = string.Empty;
            string m_strNumSerie = string.Empty;
            string strComentario = string.Empty;
            string strDocEntryDocCompra = string.Empty;
            string strDocTypeDocCompra = string.Empty;
            int intDocNumDocCompra = 0;

            try
            {
                if (pval.BeforeAction)
                {
                    InicializarTimer();
                    if (string.IsNullOrEmpty(txtProveedorCode.ObtieneValorUserDataSource().ToString().Trim()) == true)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(Resource.ErrorCodigoProveedor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        return;
                    }
                    oMatrix = (SAPbouiCOM.Matrix)p_oForm.Items.Item(g_strmtxDocCompra).Specific;
                    oMatrix.FlushToDataSource();
                    dtItemsSeleccionados = p_oForm.DataSources.DataTables.Item(g_strdtDocCompra);
                    m_dtCatidad = p_oForm.DataSources.DataTables.Item(g_strdtCantidad);


                    for (int index = 0; index < dtItemsSeleccionados.Rows.Count; index++)
                    {
                        if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                        {
                            if (string.IsNullOrEmpty(dtItemsSeleccionados.GetValue("tax", index).ToString().Trim()))
                            {
                                bubbleEvent = false;
                                ApplicationSBO.StatusBar.SetText(string.Format(Resource.FaltaTax, dtItemsSeleccionados.GetValue("code", index).ToString().Trim(), index + 1), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                break;
                            }
                        }

                        if (m_dtCatidad.GetValue("Id", index).ToString().Trim() == dtItemsSeleccionados.GetValue("idit", index).ToString().Trim())
                        {
                            if (double.Parse(dtItemsSeleccionados.GetValue("cant", index).ToString().Trim()) > double.Parse(m_dtCatidad.GetValue("Cant", index).ToString().Trim()))
                            {
                                bubbleEvent = false;
                                ApplicationSBO.StatusBar.SetText(string.Format(Resource.MsjCantidadErronea, dtItemsSeleccionados.GetValue("code", index).ToString().Trim(), index + 1), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                                break;
                            }
                        }

                    }

                    if (bubbleEvent == false)
                    {
                        DetenerTimer();
                    }
                }
                else if (pval.ActionSuccess)
                {
                    dtItemsSeleccionados = p_oForm.DataSources.DataTables.Item(g_strdtDocCompra);
                    m_dtBodegasXCentroCosto = p_oForm.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);

                    if (string.IsNullOrEmpty(g_strOfertaCompra) == false || string.IsNullOrEmpty(g_strOrdenCompra) == false)
                    {
                        if (g_strOfertaCompra == "N" && g_strOrdenCompra == "Y")
                        {
                            m_bEsOrden = true;
                            m_objDocumento = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                            m_strNumSerie = g_strSerieOrden;
                        }
                        else
                        {
                            m_bEsOrden = false;
                            m_objDocumento = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oPurchaseQuotations);
                            m_strNumSerie = g_strSerieOferta;
                        }

                        m_objDocumento.HandWritten = BoYesNoEnum.tNO;
                        m_intDocEntry = int.Parse(g_strDocEntry);
                        oCotizacion = CargaObjetoCotizacion(m_intDocEntry);

                        string m_strNoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim();
                        string m_strProyecto = oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value.ToString().Trim();
                        string m_strModelo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString().Trim();
                        string m_strCodeModelo = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString().Trim();
                        string m_strMarca = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim();
                        string m_strCodeMarca = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString().Trim();
                        string m_strEstilo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim();
                        string m_strCodeEstilo = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString().Trim();
                        string m_strPlaca = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim();
                        string m_strNoChasis = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim();
                        string m_strAno = oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString().Trim();
                        string m_strIdSucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim();
                        int m_intAsesor = oCotizacion.DocumentsOwner;
                        string m_strCodeProveedor = txtProveedorCode.ObtieneValorUserDataSource();
                        string m_strNameProveedor = txtProveedorName.ObtieneValorUserDataSource();
                        string m_strComentarios = txtComentarios.ObtieneValorUserDataSource();
                        DateTime m_dtFecha = System.DateTime.Today;

                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = m_strNoOT;
                        m_objDocumento.Project = m_strProyecto;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = m_strModelo;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = m_strCodeModelo;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = m_strMarca;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = m_strCodeMarca;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = m_strEstilo;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = m_strCodeEstilo;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = m_strPlaca;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = m_strNoChasis;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = m_strAno;
                        m_objDocumento.UserFields.Fields.Item("U_SCGD_idSucursal").Value = m_strIdSucursal;
                        m_objDocumento.DocumentsOwner = m_intAsesor;
                        strComentario = Resource.OT_Referencia + m_strNoOT + "  " + m_strComentarios;
                        m_objDocumento.Comments = strComentario.Trim().Length > 254 ? strComentario.Trim().Substring(0, 254) : strComentario.Trim();
                        m_objDocumento.CardCode = m_strCodeProveedor;
                        m_objDocumento.DocDate = m_dtFecha;
                        m_objDocumento.Series = int.Parse(m_strNumSerie);
                        m_objDocumento.RequriedDate = m_dtFecha;
                        if (DMS_Connector.Company.AdminInfo.EnableBranches == SAPbobsCOM.BoYesNoEnum.tYES)
                        {
                            if (!string.IsNullOrEmpty(m_strIdSucursal))
                            {
                                m_objDocumento.BPL_IDAssignedToInvoice = int.Parse(m_strIdSucursal);
                            }
                        }



                        DevuelveSerie(m_objDocumento.Series, p_oForm, (SAPbobsCOM.Company)CompanySBO, ref  m_strserie);

                        for (int i = 0; i <= dtItemsSeleccionados.Rows.Count - 1; i++)
                        {
                            for (int y = 0; y <= oCotizacion.Lines.Count - 1; y++)
                            {
                                oCotizacion.Lines.SetCurrentLine(y);
                                if (oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == dtItemsSeleccionados.GetValue("idit", i).ToString().Trim())
                                {
                                    break;
                                }
                            }

                            m_strCentroCosto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString().Trim();

                            for (int y = 0; y <= m_dtBodegasXCentroCosto.Rows.Count - 1; y++)
                            {
                                if (m_dtBodegasXCentroCosto.GetValue("Sucursal", y).ToString().Trim() == m_strIdSucursal &&
                                    m_dtBodegasXCentroCosto.GetValue("CentroCosto", y).ToString().Trim() == m_strCentroCosto)
                                {
                                    m_strBodegaProceso = m_dtBodegasXCentroCosto.GetValue("Proceso", y).ToString().Trim();
                                    break;
                                }
                            }

                            if (i > 0)
                            {
                                m_objDocumento.Lines.Add();
                            }

                            m_objDocumento.Lines.ItemCode = dtItemsSeleccionados.GetValue("code", i).ToString().Trim();
                            m_objDocumento.Lines.ItemDescription = dtItemsSeleccionados.GetValue("desc", i).ToString().Trim();
                            m_objDocumento.Lines.WarehouseCode = m_strBodegaProceso;
                            if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR != "Y")
                            {
                                m_objDocumento.Lines.TaxCode = dtItemsSeleccionados.GetValue("tax", i).ToString().Trim();
                                m_objDocumento.Lines.VatGroup = dtItemsSeleccionados.GetValue("tax", i).ToString().Trim();
                            } 
                            m_objDocumento.Lines.Quantity = Convert.ToDouble(dtItemsSeleccionados.GetValue("cant", i));
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = dtItemsSeleccionados.GetValue("idit", i).ToString().Trim();
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_CodEspecifico").Value = string.Empty; //dtItemsSeleccionados.GetValue("", i).ToString().Trim();
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_NombEspecific").Value = string.Empty; //dtItemsSeleccionados.GetValue("", i).ToString().Trim();
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOT;
                            m_objDocumento.Lines.UnitPrice = Convert.ToDouble(dtItemsSeleccionados.GetValue("prec", i));
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString();
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value;
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value;
                            m_objDocumento.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value;

                            double m_dblCantidad = Convert.ToDouble(dtItemsSeleccionados.GetValue("cant", i));
                            double m_dblCantidadSolicitada = Convert.ToDouble(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value);
                            double m_dblCantidadPendiente = Convert.ToDouble(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value);

                            m_dblCantidadSolicitada += m_dblCantidad;
                            m_dblCantidadPendiente -= m_dblCantidad;

                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_dblCantidadSolicitada;
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_dblCantidadPendiente;
                        }

                        if (!CompanySBO.InTransaction)
                        {
                            CompanySBO.StartTransaction();
                        }

                        if (oCotizacion.Update() == 0)
                        {
                            if (m_objDocumento.Add() == 0)
                            {
                                strDocEntryDocCompra = CompanySBO.GetNewObjectKey();
                                strDocTypeDocCompra = CompanySBO.GetNewObjectType();
                                if (CompanySBO.InTransaction)
                                {
                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                                }
                                if (m_bEsOrden)
                                {
                                    oDocCompra = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                                }
                                else
                                {
                                    oDocCompra = (Documents)CompanySBO.GetBusinessObject(BoObjectTypes.oPurchaseQuotations);
                                }
                                if (oDocCompra.GetByKey(Convert.ToInt32(strDocEntryDocCompra)))
                                {
                                    intDocNumDocCompra = oDocCompra.DocNum;
                                }


                                InsertaTrackingCompra(dtItemsSeleccionados, m_strNoOT, m_dtFecha, m_strCodeProveedor,
                                    m_strNameProveedor, strDocEntryDocCompra, strDocTypeDocCompra, intDocNumDocCompra);
                                // RefrescaMatrizArticuloOT(dtItemsSeleccionados, oCotizacion);

                                if ((p_oFormOT != null))
                                    if (!g_objGestorFormularios.FormularioAbierto(p_oFormOT, true))
                                        p_oFormOT.FormularioSBO = g_objGestorFormularios.CargarFormulario(p_oFormOT);


                                p_oFormOT.CargarOT(m_strNoOT);


                                if (g_strOfertaCompra == "N" && g_strOrdenCompra == "Y")
                                {
                                    Utilitarios.CreaMensajeSBO(Resource.MensajeNuevaOrdenCompra, strDocEntryDocCompra, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((Int32)Utilitarios.RolesMensajeria.EncargadoCompras).ToString(), m_strIdSucursal, p_oForm, g_strdtDocCompra, false, Utilitarios.RolesMensajeria.EncargadoCompras, false, (SAPbouiCOM.Application)ApplicationSBO);
                                }
                                else
                                {
                                    Utilitarios.CreaMensajeSBO(Resource.MensajeNuevaOfertaCompra, strDocEntryDocCompra, (SAPbobsCOM.Company)CompanySBO, m_strNoOT, false, ((Int32)Utilitarios.RolesMensajeria.EncargadoCompras).ToString(), m_strIdSucursal, p_oForm, g_strdtDocCompra, false, Utilitarios.RolesMensajeria.EncargadoCompras, false, (SAPbouiCOM.Application)ApplicationSBO);
                                }

                                p_oForm.Close();
                                ApplicationSBO.StatusBar.SetText(Resource.CreacionDocCompra, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                            }
                            else
                            {
                                if (CompanySBO.InTransaction)
                                {
                                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                                error = CompanySBO.GetLastErrorCode();
                                errorDes = CompanySBO.GetLastErrorDescription();
                                ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", error, errorDes), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            }
                        }
                        else
                        {
                            if (CompanySBO.InTransaction)
                            {
                                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            error = CompanySBO.GetLastErrorCode();
                            errorDes = CompanySBO.GetLastErrorDescription();
                            ApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", error, errorDes), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }

                    }

                    DetenerTimer();
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                oTimer.Stop();
                oTimer.Dispose();
                throw;
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

        /// <summary>
        /// Refresca la matriz proveniente de los articulos que se compraron
        /// </summary>
        /// <param name="dtItemsSeleccionados"></param>
        /// <param name="oCotizacion"></param>
        private void RefrescaMatrizArticuloOT(DataTable p_dtItemsSeleccionados, SAPbobsCOM.Documents p_oCotizacion)
        {
            SAPbouiCOM.Matrix m_oMatrixART;
            SAPbouiCOM.DataTable m_dtArt;
            SAPbouiCOM.Item m_objItem;
            SAPbouiCOM.ComboBox m_objCombo;
            string m_strValorCombo = string.Empty;
            string m_strIDArt = string.Empty;
            double m_dblCantidadSolicitada = 0;
            double m_dblCantidadPendiente = 0;
            string m_strItems = string.Empty;

            try
            {
                g_oformOT = ApplicationSBO.Forms.Item(g_strOT);

                switch (g_tipoAdicional)
                {
                    case TipoAdicional.Repuesto:
                        m_objItem = g_oformOT.Items.Item("cboEstR");
                        m_objCombo = (ComboBox)m_objItem.Specific;
                        m_strValorCombo = m_objCombo.Value.Trim();
                        if (string.IsNullOrEmpty(m_strValorCombo))
                            m_strItems = OrdenTrabajo.g_strdtRepuestos;
                        else
                            m_strItems = OrdenTrabajo.g_strdtRepuestosTemporal;

                        m_dtArt = g_oformOT.DataSources.DataTables.Item(m_strItems);
                        m_oMatrixART = (SAPbouiCOM.Matrix)g_oformOT.Items.Item("mtxRep").Specific;
                        for (int i = 0; i < p_dtItemsSeleccionados.Rows.Count; i++)
                        {
                            m_strIDArt = p_dtItemsSeleccionados.GetValue("idit", i).ToString().Trim();

                            for (int y = 0; y <= p_oCotizacion.Lines.Count - 1; y++)
                            {
                                p_oCotizacion.Lines.SetCurrentLine(y);
                                if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == m_strIDArt)
                                {
                                    break;
                                }
                            }
                            m_dblCantidadSolicitada = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                            m_dblCantidadPendiente = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value;

                            for (int j = 0; j < m_dtArt.Rows.Count; j++)
                            {
                                if (m_dtArt.GetValue("idit", j).ToString().Trim() == m_strIDArt)
                                {
                                    m_dtArt.SetValue("pend", j, m_dblCantidadPendiente);
                                    m_dtArt.SetValue("soli", j, m_dblCantidadSolicitada);
                                    m_dtArt.SetValue("sele", j, "N");
                                }
                            }
                        }
                        m_oMatrixART.LoadFromDataSource();
                        g_oformOT.DataSources.DataTables.Item(OrdenTrabajo.g_strdtRepuestosSeleccionados).Rows.Clear();

                        break;

                    case TipoAdicional.ServicioExterno:
                        m_objItem = g_oformOT.Items.Item("cboEstSE");
                        m_objCombo = (ComboBox)m_objItem.Specific;
                        m_strValorCombo = m_objCombo.Value.Trim();
                        if (string.IsNullOrEmpty(m_strValorCombo))
                            m_strItems = OrdenTrabajo.g_strdtServiciosExternos;
                        else
                            m_strItems = OrdenTrabajo.g_strdtServiciosExternosTemporal;

                        m_dtArt = g_oformOT.DataSources.DataTables.Item(m_strItems);
                        m_oMatrixART = (SAPbouiCOM.Matrix)g_oformOT.Items.Item("mtxServE").Specific;
                        for (int i = 0; i < p_dtItemsSeleccionados.Rows.Count; i++)
                        {
                            m_strIDArt = p_dtItemsSeleccionados.GetValue("idit", i).ToString().Trim();

                            for (int y = 0; y <= p_oCotizacion.Lines.Count - 1; y++)
                            {
                                p_oCotizacion.Lines.SetCurrentLine(y);
                                if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == m_strIDArt)
                                {
                                    break;
                                }
                            }

                            m_dblCantidadSolicitada = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                            m_dblCantidadPendiente = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value;

                            for (int j = 0; j < m_dtArt.Rows.Count; j++)
                            {
                                if (m_dtArt.GetValue("idit", j).ToString().Trim() == m_strIDArt)
                                {
                                    m_dtArt.SetValue("pend", j, m_dblCantidadPendiente);
                                    m_dtArt.SetValue("soli", j, m_dblCantidadSolicitada);
                                    m_dtArt.SetValue("sele", j, "N");
                                }
                            }
                        }

                        m_oMatrixART.LoadFromDataSource();
                        g_oformOT.DataSources.DataTables.Item(OrdenTrabajo.g_strdtServiciosExternosSeleccionados).Rows.Clear();

                        break;

                    case TipoAdicional.Suministro:
                        m_objItem = g_oformOT.Items.Item("cboEstSu");
                        m_objCombo = (ComboBox)m_objItem.Specific;
                        m_strValorCombo = m_objCombo.Value.Trim();
                        if (string.IsNullOrEmpty(m_strValorCombo))
                            m_strItems = OrdenTrabajo.g_strdtSuministros;
                        else
                            m_strItems = OrdenTrabajo.g_strdtSuministrosTemporal;

                        m_dtArt = g_oformOT.DataSources.DataTables.Item(m_strItems);
                        m_oMatrixART = (SAPbouiCOM.Matrix)g_oformOT.Items.Item("mtxSum").Specific;
                        for (int i = 0; i < p_dtItemsSeleccionados.Rows.Count; i++)
                        {
                            m_strIDArt = p_dtItemsSeleccionados.GetValue("idit", i).ToString().Trim();

                            for (int y = 0; y <= p_oCotizacion.Lines.Count - 1; y++)
                            {
                                p_oCotizacion.Lines.SetCurrentLine(y);
                                if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() == m_strIDArt)
                                {
                                    break;
                                }
                            }

                            m_dblCantidadSolicitada = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                            m_dblCantidadPendiente = (double)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value;

                            for (int j = 0; j < m_dtArt.Rows.Count; j++)
                            {
                                if (m_dtArt.GetValue("idit", j).ToString().Trim() == m_strIDArt)
                                {
                                    m_dtArt.SetValue("pend", j, m_dblCantidadPendiente);
                                    m_dtArt.SetValue("soli", j, m_dblCantidadSolicitada);
                                    m_dtArt.SetValue("sele", j, "N");

                                }
                            }
                        }

                        m_oMatrixART.LoadFromDataSource();
                        g_oformOT.DataSources.DataTables.Item(OrdenTrabajo.g_strdtSuministrosSeleccionados).Rows.Clear();

                        break;
                }

                //SAPbouiCOM.Form oform = ApplicationSBO.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount);

                //string OtId = string.Empty;
                //SAPbouiCOM.Matrix oMatriz = default(SAPbouiCOM.Matrix);

                //OtId = (oform.Items.Item("SCGD_etOT").Specific).Value.ToString().Trim();
            }
            catch (Exception)
            {
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                throw;
                throw;
            }
        }

        /// <summary>
        /// Inserta Tracking de la compra compra
        /// </summary>
        /// <param name="oGeneralService"></param>
        /// <param name="oGeneralData"></param>
        /// <param name="dtItemsSeleccionados"></param>
        /// <param name="m_strNoOT"></param>
        /// <param name="m_dtFecha"></param>
        /// <param name="m_strCodeProveedor"></param>
        /// <param name="m_strserie"></param>
        /// <param name="mBEsOrden"></param>
        /// <param name="p_oForm"></param>
        /// <param name="oCotizacion"></param>
        private void InsertaTracking(SAPbobsCOM.GeneralService oGeneralService, SAPbobsCOM.GeneralData oGeneralData, DataTable dtItemsSeleccionados, string m_strNoOT, DateTime m_dtFecha, string m_strCodeProveedor,
                                string m_strserie, bool mBEsOrden, Form p_oForm, SAPbobsCOM.Documents oCotizacion)
        {
            double m_dblCantidad;
            string m_strDocNum = string.Empty;
            SAPbouiCOM.DataTable dtConsulta;

            try
            {
                for (int i = 0; i < dtItemsSeleccionados.Rows.Count; i++)
                {
                    if (mBEsOrden)
                    {
                        string m_strConsulta = string.Format(g_strConsultaORdenCompra, " OPOR", dtItemsSeleccionados.GetValue("idit", i).ToString().Trim());
                        dtConsulta = p_oForm.DataSources.DataTables.Item(g_strdtCompra);
                        dtConsulta.ExecuteQuery(m_strConsulta);
                        m_strDocNum = dtConsulta.GetValue("DocNum", 0).ToString();
                    }
                    else
                    {
                        string m_strConsulta = string.Format(g_strConsultaORdenCompra, " OPQT", dtItemsSeleccionados.GetValue("idit", i).ToString().Trim());
                        dtConsulta = p_oForm.DataSources.DataTables.Item(g_strdtCompra);
                        dtConsulta.ExecuteQuery(m_strConsulta);
                        m_strDocNum = dtConsulta.GetValue("DocNum", 0).ToString();
                    }

                    m_dblCantidad = double.Parse(dtItemsSeleccionados.GetValue("cant", i).ToString().Trim());


                    oGeneralData.SetProperty("U_IdRep", dtItemsSeleccionados.GetValue("idit", i).ToString().Trim());
                    oGeneralData.SetProperty("U_ItemCode", dtItemsSeleccionados.GetValue("code", i).ToString().Trim());
                    oGeneralData.SetProperty("U_NoOrden", m_strNoOT);
                    oGeneralData.SetProperty("U_FechSol", m_dtFecha);
                    oGeneralData.SetProperty("U_CodPr", m_strCodeProveedor);
                    oGeneralData.SetProperty("U_CantSol", m_dblCantidad);
                    oGeneralData.SetProperty("U_NoCom", m_strserie + "-" + m_strDocNum);
                    oGeneralData.SetProperty("U_Obser", txtComentarios.ObtieneValorUserDataSource());
                    oGeneralService.Add(oGeneralData);
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

        /// <summary>
        /// Devueleve serie de la compra
        /// </summary>
        /// <param name="intSeries"></param>
        /// <param name="p_oForm"></param>
        /// <param name="oCompany"></param>
        /// <param name="strEtiquetadeSeries"></param>
        public static void DevuelveSerie(int intSeries, SAPbouiCOM.Form p_oForm, SAPbobsCOM.Company oCompany, ref string strEtiquetadeSeries)
        {
            SAPbouiCOM.DataTable dtConsulta;
            try
            {
                string strConsultaEtiquetadeSerie = "Select SeriesName" +
                                                    " From NNM1" +
                                                    " Where Series =" + intSeries;


                dtConsulta = p_oForm.DataSources.DataTables.Item(g_strdtSerie);

                dtConsulta.ExecuteQuery(strConsultaEtiquetadeSerie);

                strEtiquetadeSeries = dtConsulta.GetValue("SeriesName", 0).ToString();
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

        public void CargaProveedor()
        {
            try
            {
                txtProveedorCode.AsignaValorUserDataSource(g_ProvCode);
                txtProveedorName.AsignaValorUserDataSource(g_ProvName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserta Tracking OT
        /// </summary>
        private void InsertaTrackingCompra(DataTable dtItemsSeleccionados,
                                           string p_strNoOT,
                                           DateTime p_dtFecha,
                                           string p_strCodeProveedor,
                                           string p_strNameProveedor,
                                           string p_strDocEntryDocCompra,
                                           string p_strDocTypeDocCompra,
                                           int p_intDocNum)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChildOT;
            SAPbobsCOM.GeneralDataCollection oChildrenOT;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            double m_dblCantidad;
            List<SAPbobsCOM.GeneralData> oGeneralDataList = new List<SAPbobsCOM.GeneralData>();

            oCompanyService = CompanySBO.GetCompanyService();
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
            oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
            oGeneralParams.SetProperty("Code", p_strNoOT);
            oGeneralData = oGeneralService.GetByParams(oGeneralParams);
            oChildrenOT = oGeneralData.Child("SCGD_TRACKXOT");
            try
            {
                for (int i = 0; i < dtItemsSeleccionados.Rows.Count; i++)
                {
                    oChildOT = oChildrenOT.Add();
                    oChildOT.SetProperty("U_NoOrden", p_strNoOT);
                    if (!string.IsNullOrEmpty(dtItemsSeleccionados.GetValue("code", i).ToString()))
                    {
                        oChildOT.SetProperty("U_ItemCode", dtItemsSeleccionados.GetValue("code", i).ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dtItemsSeleccionados.GetValue("idit", i).ToString()))
                    {
                        oChildOT.SetProperty("U_ID", dtItemsSeleccionados.GetValue("idit", i).ToString().Trim());
                    }
                    if (p_dtFecha != new DateTime())
                    {
                        oChildOT.SetProperty("U_FechaDoc", p_dtFecha);
                    }
                    if (!string.IsNullOrEmpty(p_strCodeProveedor))
                    {
                        oChildOT.SetProperty("U_CardCode", p_strCodeProveedor);
                    }
                    if (!string.IsNullOrEmpty(p_strNameProveedor))
                    {
                        oChildOT.SetProperty("U_CardName", p_strNameProveedor);
                    }
                    if (!string.IsNullOrEmpty(p_strDocEntryDocCompra))
                    {
                        oChildOT.SetProperty("U_DocEntry", p_strDocEntryDocCompra);
                    }
                    if (p_intDocNum > 0)
                    {
                        oChildOT.SetProperty("U_DocNum", p_intDocNum);
                    }
                    m_dblCantidad = double.Parse(dtItemsSeleccionados.GetValue("cant", i).ToString().Trim());
                    oChildOT.SetProperty("U_CanSol", m_dblCantidad);
                    oChildOT.SetProperty("U_CanRec", 0);
                    oChildOT.SetProperty("U_TipoDoc", p_strDocTypeDocCompra);
                    oChildOT.SetProperty("U_Observ", txtComentarios.ObtieneValorUserDataSource());
                }
                oGeneralDataList.Add(oGeneralData);
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                if (!CompanySBO.InTransaction)
                {
                    CompanySBO.StartTransaction();
                }
                foreach (SAPbobsCOM.GeneralData rowoGeneralData in oGeneralDataList)
                {
                    oGeneralService.Update(rowoGeneralData);
                }
                if (CompanySBO.InTransaction)
                {
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
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
    }
}
