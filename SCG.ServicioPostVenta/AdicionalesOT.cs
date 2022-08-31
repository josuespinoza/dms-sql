using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using DMSOneFramework;

namespace SCG.ServicioPostVenta
{
    public partial class AdicionalesOT
    {
        private int m_intNumLineaFocus = -1;
        private int m_intPrimerLinea = -1;
        private int m_intUltimaLinea = -1;
        private bool m_boolUsaShift = false;
        private string m_strFocusItem = string.Empty;
      

        #region ...Eventos...
        public void ManejadorEventoFormDataLoad(SAPbouiCOM.ItemEvent pval, int p_intTipoFormulario)
        {
            ComboBox m_objCombo;
            Matrix m_objMatrix;
            SAPbouiCOM.Form oForm;
            bool m_UsaConsultaEstiMod = false;
            string m_strConsultaEstiMod = string.Empty;
            ItemsSeleccionados itmSel;
           

            if (pval.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                try
                {
                    CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                    CultureInfo cultureInfo = Resource.Culture;
                    DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    Resource.Culture = cultureInfo;
                    UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources;
                    UDS_SeleccionaRepuestos.Add("code", BoDataType.dt_LONG_TEXT, 100);
                    UDS_SeleccionaRepuestos.Add("desc", BoDataType.dt_LONG_TEXT, 100);
                    UDS_SeleccionaRepuestos.Add("CodBar", BoDataType.dt_LONG_TEXT, 100);

                    txtCode = new EditTextSBO("txtCode", true, "", "code", FormularioSBO);
                    txtCode.AsignaBinding();
                    txtDescripcion = new EditTextSBO("txtDesc", true, "", "desc", FormularioSBO);
                    txtDescripcion.AsignaBinding();
                    txtCodeBar = new EditTextSBO("txtCodBar", true, "", "CodBar", FormularioSBO);
                    txtCodeBar.AsignaBinding();

                    oForm = ApplicationSBO.Forms.Item(FormType);
                    oForm.Freeze(true);

                    ObtieneEstiModYConfListPrecios(oForm, ref m_UsaConsultaEstiMod, ref m_strConsultaEstiMod,
                        p_intTipoFormulario);

                    g_strConsultaRepuestos = String.Format(g_strConsultaRepuestos, strNoOT, g_strCodListPrecio);
                    g_strConsultaServicios = String.Format(g_strConsultaServicios, strNoOT, g_strCodListPrecio);
                    g_strConsultaServiciosExternos = String.Format(g_strConsultaServiciosExternos, strNoOT,
                        g_strCodListPrecio);
                    g_strConsultaSuministros = String.Format(g_strConsultaSuministros, strNoOT, g_strCodListPrecio);

                    m_objMatrix = (Matrix)FormularioSBO.Items.Item(g_strmtxAdicionales).Specific;
                    m_objMatrix.FlushToDataSource();

                    //Se ocultan las columnas de acuerdo al tipo de articulo
                    if (p_intTipoFormulario == 2 || p_intTipoFormulario == 4)
                    {
                        m_objMatrix.Columns.Item("Col_bode").Visible = false;
                        m_objMatrix.Columns.Item("Col_csto").Visible = false;
                        if (p_intTipoFormulario != 2)
                            m_objMatrix.Columns.Item("Col_dura").Visible = false;
                    }
                    else
                        m_objMatrix.Columns.Item("Col_dura").Visible = false;


                    g_strConsultaEstiModConf = m_strConsultaEstiMod;
                    g_UsaConsultaEstiMod = m_UsaConsultaEstiMod;

                    if (g_UsaConsultaEstiMod)
                    {
                        m_strConsultaExistenciaArt = string.Format(m_strConsultaExistenciaArt, p_intTipoFormulario);

                        if (g_strEspecifVehif == "E")
                        {
                            m_strConsultaExistenciaArt = m_strConsultaExistenciaArt +
                                                         string.Format(" and U_CodEsti = '{0}'", g_strEsti);
                            g_dtExisteArt.ExecuteQuery(m_strConsultaExistenciaArt);
                            g_bExisteArticulos = g_dtExisteArt.GetValue("U_ItemCode", 0).ToString().Trim() != "0";
                        }
                        else if (g_strEspecifVehif == "M")
                        {
                            m_strConsultaExistenciaArt = m_strConsultaExistenciaArt +
                                                         string.Format(" and  U_CodMod = {0}", g_strMod);
                            g_dtExisteArt.ExecuteQuery(m_strConsultaExistenciaArt);
                            g_bExisteArticulos = g_dtExisteArt.GetValue("U_ItemCode", 0).ToString().Trim() != "0";
                        }
                    }
                    g_IntTipoAdicional = p_intTipoFormulario;

                    switch (p_intTipoFormulario)
                    {
                        case (int)TipoAdicional.Repuesto:

                            g_dtConsulta = FormularioSBO.DataSources.DataTables.Item(g_strdtConsulta);
                            g_dtConsulta.ExecuteQuery(string.Format(strConsultaConfEspec, strNoOT));
                            btnSolEspec = (SAPbouiCOM.Button)FormularioSBO.Items.Item("btnSolEsp").Specific;

                            if (g_dtConsulta.GetValue(0, 0).ToString() == "Y")
                                btnSolEspec.Item.Visible = true;
                            else
                                btnSolEspec.Item.Visible = false;

                            if (!m_UsaConsultaEstiMod)
                            {
                                g_dtAdicionales.ExecuteQuery(g_strConsultaRepuestos);
                            }
                            else
                            {
                                if (g_bExisteArticulos)
                                    g_dtAdicionales.ExecuteQuery(g_strConsultaEstiModConf);
                                else
                                    g_dtAdicionales.ExecuteQuery(g_strConsultaRepuestos);
                            }
                            break;
                        case (int)TipoAdicional.Servicio:
                            if (!m_UsaConsultaEstiMod)
                            {
                                g_dtAdicionales.ExecuteQuery(g_strConsultaServicios);
                            }
                            else
                            {
                                if (g_bExisteArticulos)
                                    g_dtAdicionales.ExecuteQuery(g_strConsultaEstiModConf);
                                else
                                    g_dtAdicionales.ExecuteQuery(g_strConsultaServicios);
                            }
                            break;
                        case (int)TipoAdicional.ServicioExterno:

                            g_dtAdicionales.ExecuteQuery(g_strConsultaServiciosExternos);

                            break;
                        case (int)TipoAdicional.Suministros:

                            g_dtAdicionales.ExecuteQuery(g_strConsultaSuministros);

                            break;
                    }

                    lstSeleccionados = new List<ItemsSeleccionados>();
                    g_dtAdicionalesSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtAdicionalesSel);

                    for (int index = 0; index <= g_dtAdicionales.Rows.Count - 1; index++)
                    {
                        itmSel = new ItemsSeleccionados();
                        itmSel.ItemCode = g_dtAdicionales.GetValue("code", index).ToString().Trim();
                        itmSel.Posicion = index;
                        lstSeleccionados.Add(itmSel);
                    }

                    m_objMatrix.LoadFromDataSource();
                    oForm.Freeze(false);
                }
                catch (Exception ex)
                {
                    throw;
                    //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                }
            }
        }

        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent);
                    break;
                case BoEventTypes.et_GOT_FOCUS:
                    ManejadorEventosGotFocus(FormUID, pVal, ref BubbleEvent);
                    break;
                case BoEventTypes.et_LOST_FOCUS:
                    if ((pVal.ColUID == "Col_prec" || pVal.ColUID == "Col_cant" || pVal.ColUID == "Col_desc" || pVal.ColUID == "Col_dura") && pVal.Action_Success)
                    {
                        RecalcularPrecioCantidad(FormUID, ref pVal);
                    }
                    ManejadorEventosLostFocus(FormUID, pVal, ref BubbleEvent);
                    break;
                case BoEventTypes.et_KEY_DOWN:
                    ManejadorEventosKeyDown(FormUID, pVal, ref BubbleEvent);
                    break;
            }
        }

        private void RecalcularPrecioCantidad(string formUID, ref ItemEvent pVal)
        {
            double cantidad = 0;
            double precioLinea = 0;
            string strCodigo = string.Empty;
            string strDescripcion = string.Empty;
            string strDuracion = string.Empty;
            SAPbouiCOM.Matrix mtxLines;
            SAPbouiCOM.Form oForm;
            try
            {
                oForm = ApplicationSBO.Forms.Item(formUID);
                FormularioSBO = oForm;
                mtxLines = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(g_strmtxAdicionales).Specific;

                cantidad = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("Col_cant").Cells.Item(pVal.Row).Specific).Value, n);
                precioLinea = double.Parse(((SAPbouiCOM.EditText)mtxLines.Columns.Item("Col_prec").Cells.Item(pVal.Row).Specific).Value, n);
                strCodigo = ((SAPbouiCOM.EditText)mtxLines.Columns.Item("Col_code").Cells.Item(pVal.Row).Specific).Value;
                strDescripcion = ((SAPbouiCOM.EditText)mtxLines.Columns.Item("Col_desc").Cells.Item(pVal.Row).Specific).Value;
                strDuracion = ((SAPbouiCOM.EditText)mtxLines.Columns.Item("Col_dura").Cells.Item(pVal.Row).Specific).Value;

                g_dtAdicionalesSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtAdicionalesSel);

                for (int index = 0; index <= g_dtAdicionalesSeleccionados.Rows.Count - 1; index++)
                {
                    if (g_dtAdicionalesSeleccionados.GetValue("code", index).ToString().Trim() == strCodigo.Trim())
                    {
                        g_dtAdicionalesSeleccionados.SetValue("cant", index, cantidad);
                        g_dtAdicionalesSeleccionados.SetValue("prec", index, precioLinea);
                        g_dtAdicionalesSeleccionados.SetValue("desc", index, strDescripcion);
                        g_dtAdicionalesSeleccionados.SetValue("dura", index, strDuracion);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtAdicionales;
            SAPbouiCOM.DataTable dtAdicionalesSeleccionados;
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.EditText oEditText;
            SAPbouiCOM.CheckBox oCheckBox;

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
                            case "btnAgre":
                                if (m_intNumLineaFocus != -1)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm, m_intNumLineaFocus);
                                    LimpiarVariablesGlobales();
                                }
                                AgregarAdicionales(oForm);
                                break;
                            case "btnBuscar":
                                if (m_intNumLineaFocus != -1)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm, m_intNumLineaFocus);
                                    LimpiarVariablesGlobales();
                                }
                                BuscarAdicionales(oForm);
                                break;
                            case "btnSolEsp":
                                if(m_intNumLineaFocus != -1)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm, m_intNumLineaFocus);
                                    LimpiarVariablesGlobales();
                                }
                                CrearSolicitudEspecificos(oForm);
                                break;
                            case "mtxAdic":
                                if (pVal.ColUID == "Col_sele" && pVal.Action_Success)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm);
                                }
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

        /// <summary>
        /// Maneja los eventos de tipo et_KEY_DOWN
        /// </summary>
        /// <param name="formUID">ID del formulario</param>
        /// <param name="pVal">pVal con la informacion del Item</param>
        /// <param name="bubbleEvent">bubbleEvent de SAP</param>
        private void ManejadorEventosKeyDown(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {

            SAPbouiCOM.Form oForm;
            bool boolValido = false;
       
            try
            {
                //Se presiona la tecla espacio para marcar un checkbox
                //Char 32 = Tecla espacio
                if (pVal.CharPressed == 32)
                {
                    boolValido = true;           
                }

                if (string.IsNullOrEmpty(formUID) == false && boolValido == true)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {

                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "mtxAdic":
                                if (pVal.ColUID == "Col_sele" && pVal.Action_Success)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm);
                                }
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

        /// <summary>
        /// Maneja los eventos de tipo et_GOT_FOCUS
        /// </summary>
        /// <param name="formUID">ID del formulario</param>
        /// <param name="pVal">pVal con la informacion del Item</param>
        /// <param name="bubbleEvent">bubbleEvent de SAP</param>
        private void ManejadorEventosGotFocus(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {

            SAPbouiCOM.Form oForm;

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
                        m_strFocusItem = pVal.ItemUID;
                        switch (pVal.ItemUID)
                        {
                            case "mtxAdic":
                                if (pVal.ColUID == "Col_sele" && pVal.Action_Success)
                                {
                                    //Se guarda en una variable la última línea que se haya seleccionada en la matriz
                                    //esto para ser utilizado por otros métodos que controlan el manejo de eventos con el mouse
                                    m_intNumLineaFocus = pVal.Row;
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm);
                                }

                               
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

        /// <summary>
        /// Maneja los eventos de tipo et_LOST_FOCUS
        /// </summary>
        /// <param name="formUID">ID del formulario</param>
        /// <param name="pVal">pVal con la informacion del Item</param>
        /// <param name="bubbleEvent">bubbleEvent de SAP</param>
        private void ManejadorEventosLostFocus(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {

            SAPbouiCOM.Form oForm;

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
                            case "mtxAdic":
                                if (pVal.ColUID == "Col_sele" && pVal.Action_Success)
                                {
                                    SeleccionAdicionales(pVal, ref bubbleEvent, ref oForm);
                                }
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



        #endregion

        #region ...Metodos...
        private void BuscarAdicionales(Form p_oForm)
        {
            string m_strConsultaAdicionales = string.Empty;
            string m_strFiltroCode = " and oi.ItemCode like '%{0}%' ";
            string m_strFiltroDescription = " and oi.ItemName like '%{0}%' ";
            string m_strFiltroCodeBar = " and oi.CodeBars like '%{0}%'";
            string m_strCode = string.Empty;
            string m_strDescription = string.Empty;
            string m_strCodeBar = string.Empty;
            bool m_blnCode = false;
            bool m_blnDescription = false;
            bool m_blnCodeBar = false;
            SAPbouiCOM.Matrix m_objMatrix;
            ItemsSeleccionados itmSel = new ItemsSeleccionados();
            ItemsSeleccionados itm = new ItemsSeleccionados();
            var code = string.Empty;
            double cant = 0;
            double prec = 0;

            try
            {
                m_strCode = txtCode.ObtieneValorUserDataSource().ToString().Trim();
                m_strDescription = txtDescripcion.ObtieneValorUserDataSource().ToString().Trim();
                m_strCodeBar = txtCodeBar.ObtieneValorUserDataSource().ToString().Trim();

                m_objMatrix = (Matrix)p_oForm.Items.Item(g_strmtxAdicionales).Specific;

                if (string.IsNullOrEmpty(m_strCode) == false)
                {
                    m_strFiltroCode = String.Format(m_strFiltroCode, m_strCode);
                    m_blnCode = true;
                }

                if (string.IsNullOrEmpty(m_strDescription) == false)
                {
                    m_strFiltroDescription = String.Format(m_strFiltroDescription, m_strDescription);
                    m_blnDescription = true;
                }

                if (string.IsNullOrEmpty(m_strCodeBar) == false)
                {
                    m_strFiltroCodeBar = String.Format(m_strFiltroCodeBar, m_strCodeBar);
                    m_blnCodeBar = true;
                }


                switch (g_IntTipoAdicional)
                {
                    case (int)TipoAdicional.Repuesto:
                        if (!g_UsaConsultaEstiMod)
                        {

                            m_strConsultaAdicionales = String.Format(g_strConsultaRepuestos, strNoOT, g_strCodListPrecio);
                        }
                        else
                        {
                            if (g_bExisteArticulos)
                            { m_strConsultaAdicionales = g_strUsaConsultaSegunConf; }
                            else
                            {
                                m_strConsultaAdicionales = String.Format(g_strConsultaRepuestos, strNoOT, g_strCodListPrecio);
                            }
                        }

                        break;
                    case (int)TipoAdicional.Suministros:
                        m_strConsultaAdicionales = String.Format(g_strConsultaSuministros, strNoOT,
                                                                      g_strCodListPrecio);
                        break;
                    case (int)TipoAdicional.Servicio:
                        if (!g_UsaConsultaEstiMod)
                        {
                            m_strConsultaAdicionales = String.Format(g_strConsultaServicios, strNoOT,
                                                                     g_strCodListPrecio);
                        }
                        else
                        {
                            if (g_bExisteArticulos)
                            { m_strConsultaAdicionales = g_strUsaConsultaSegunConf; }
                            else
                            {
                                m_strConsultaAdicionales = String.Format(g_strConsultaServicios, strNoOT, g_strCodListPrecio);
                            }
                        }
                        break;
                    case (int)TipoAdicional.ServicioExterno:
                        m_strConsultaAdicionales = String.Format(g_strConsultaServiciosExternos, strNoOT,
                                                                 g_strCodListPrecio);
                        break;
                }

                if (m_blnCode)
                {
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCode);
                }

                if (m_blnDescription)
                {
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroDescription);
                }

                if (m_blnCodeBar)
                {
                    m_strConsultaAdicionales = String.Format(" {0} {1} ", m_strConsultaAdicionales, m_strFiltroCodeBar);
                }


                if (m_blnCode || m_blnDescription || m_blnCodeBar)
                    g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales);
                else
                    g_dtAdicionales.ExecuteQuery(m_strConsultaAdicionales);

                lstSeleccionados = new List<ItemsSeleccionados>();
                g_dtAdicionalesSeleccionados = p_oForm.DataSources.DataTables.Item(g_strdtAdicionalesSel);

                for (int index = 0; index <= g_dtAdicionales.Rows.Count - 1; index++)
                {
                    itmSel = new ItemsSeleccionados();
                    itmSel.ItemCode = g_dtAdicionales.GetValue("code", index).ToString().Trim();
                    itmSel.Posicion = index;
                    lstSeleccionados.Add(itmSel);
                }

                for (int index = 0; index <= g_dtAdicionalesSeleccionados.Rows.Count - 1; index++)
                {
                    code = g_dtAdicionalesSeleccionados.GetValue("code", index).ToString().Trim();
                    itm = lstSeleccionados.Where(a => a.ItemCode == code).FirstOrDefault();
                    if (!string.IsNullOrEmpty(itm.ItemCode))
                    {
                        cant = Convert.ToDouble(g_dtAdicionalesSeleccionados.GetValue("cant", index).ToString());
                        prec = Convert.ToDouble(g_dtAdicionalesSeleccionados.GetValue("prec", index).ToString());
                        g_dtAdicionales.SetValue("sele", itm.Posicion, "Y");
                        g_dtAdicionales.SetValue("cant", itm.Posicion, cant);
                        g_dtAdicionales.SetValue("prec", itm.Posicion, prec);
                    }
                }
                m_objMatrix.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        /// <summary>
        /// Limpia las variables globales utilizadas durante el manejo de ItemEvent
        /// </summary>
        private void LimpiarVariablesGlobales()
        {
            m_intNumLineaFocus = -1;
            m_intPrimerLinea = -1;
            m_intUltimaLinea = -1;
            m_boolUsaShift = false;
        }

        /// <summary>
        /// Agrega una o más líneas al listado de adicionales
        /// </summary>
        /// <param name="pVal">Variable de SAP con la información del evento</param>
        /// <param name="bubbleEvent">Variable de SAP que determina si SAP debe manejar el evento o no</param>
        /// <param name="p_oForm">Formulario</param>
        /// <param name="numLineaFocus">Último número de línea a la cual se seleccionó la columna Col_sele</param>
        private void SeleccionAdicionales(ItemEvent pVal, ref bool bubbleEvent, ref Form p_oForm, int numLineaFocus = -1)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.CheckBox chkSel;

            string codeSelected = string.Empty;
            int rowNum = 0;
            int numLinea = 0;
            int contador = 0;
            
            bool existe = false;
            bool rangoSeleccionadoValido = true;
          

            try
            {
               
                oMatrix = (SAPbouiCOM.Matrix)p_oForm.Items.Item(g_strmtxAdicionales).Specific;
                oMatrix.FlushToDataSource();

                //--------------------------------------------------------------------------------------------------------
                //Paso 1 atrapar los números de línea desde eventos como mouse clic, shift, tecla espacio, Clic + Arrastre
                //--------------------------------------------------------------------------------------------------------

                //El siguiente bloque de código realiza validaciones al momento de seleccionar varias líneas utilizando la tecla Shift entre ellas
                //1-Que se haya seleccionado una línea antes de hacer clic en Shift
                //2-Ambas líneas no deben ser la misma
                //3-Que la primer línea sea menor a la última seleccionada, en caso de no serlo se intercambian los valores
                if (pVal.Modifiers == BoModifiersEnum.mt_SHIFT && pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (m_boolUsaShift == true)
                    {
                        if (m_intPrimerLinea != -1 && m_intUltimaLinea != -1)
                        {
                            if (pVal.Row > m_intUltimaLinea)
                            {
                                m_intUltimaLinea = pVal.Row;
                            }

                            if (pVal.Row < m_intPrimerLinea)
                            {
                               m_intPrimerLinea = pVal.Row;
                            }

                        }
                    }
                    else
                    {
                        m_intUltimaLinea = pVal.Row;
                    }

                    m_boolUsaShift = true;
                 

                    //No se selecciono una linea anteriormente y se procede a presiona Shift + Clic o ambas líneas son iguales
                    //no es un rango válido y se realiza el ciclo normal solamente para la última línea seleccionada
                    if (m_intPrimerLinea == -1 || m_intPrimerLinea == m_intUltimaLinea)
                    {
                        rangoSeleccionadoValido = false;
                    }
                    else
                    {
                        if (m_intPrimerLinea >= 0 && m_intUltimaLinea >= 0 && m_intPrimerLinea <= oMatrix.RowCount && m_intUltimaLinea <= oMatrix.RowCount)
                        {
                            //Ordenamos las lineas seleccionadas de tal forma que la primer linea sea la menor
                            if (m_intPrimerLinea > m_intUltimaLinea)
                            {
                                int auxiliar = m_intUltimaLinea;
                                m_intUltimaLinea = m_intPrimerLinea;
                                m_intPrimerLinea = auxiliar;
                            }
                        }
                    }
                }
                else
                {
                    //Si no se ha presionado Shift + Clic la linea seleccionada se marca como la primera y se restablece la ultima a valor por defecto
                    //se procesa solamente una vez el ciclo
                    if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && pVal.ItemUID == "mtxAdic")
                    {
                        m_intPrimerLinea = pVal.Row;
                        m_intUltimaLinea = -1;
                        m_boolUsaShift = false;
                    }

                    //Valida al momento de cambiar de casilla o hacia un cuadro de texto la última línea seleccionada
                    if (pVal.EventType == BoEventTypes.et_LOST_FOCUS && m_boolUsaShift == false && m_strFocusItem == "mtxAdic")
                    {
                        if (m_intUltimaLinea != m_intNumLineaFocus && m_intNumLineaFocus != -1)
                        {
                            if (m_intPrimerLinea != -1 && m_intUltimaLinea == -1)
                            {
                                m_intUltimaLinea = pVal.Row;
                            }
                            else
                            {
                                m_intPrimerLinea = pVal.Row;
                            }
                            
                        }
                    }
                }
                            
                g_dtAdicionalesSeleccionados = p_oForm.DataSources.DataTables.Item(g_strdtAdicionalesSel);
                g_dtAdicionales = p_oForm.DataSources.DataTables.Item(g_strdtAdicionales);
               
                //Número de línea seleccionado, en caso de seleccionar varias líneas utilizando Shift + Clic va a ser el número del línea por el cual se va a empezar
                numLinea = pVal.Row;

                //Si se utilizó la tecla Shift para seleccionar varias líneas y están en un rango válido
                if (m_boolUsaShift == true && rangoSeleccionadoValido == true)
                {
                    //Cantidad de líneas seleccionadas
                    contador = m_intUltimaLinea - m_intPrimerLinea;
                    numLinea = m_intPrimerLinea;
                }

                //Validación necesaria para agregar la última línea agregada a una solicitud de específicos usando Clic + Arrastre
                //ya que el evento Mouse Clic no atrapa esta acción, solamente funciona al hacer clic en una ventana externa
                if (numLineaFocus >= 0 && pVal.ItemUID != "mtxAdic" && numLineaFocus <= oMatrix.RowCount)
                {
                    contador = 0;
                    numLinea = numLineaFocus;
                    if (m_intUltimaLinea == -1)
                    {
                        m_intUltimaLinea = numLineaFocus;
                    }

                    if (m_intPrimerLinea >= 0 && m_intUltimaLinea >= 0 && m_intPrimerLinea <= oMatrix.RowCount && m_intUltimaLinea <= oMatrix.RowCount)
                    {
                        //Ordenamos las lineas seleccionadas de tal forma que la primer linea sea la menor
                        if (m_intPrimerLinea > m_intUltimaLinea)
                        {
                            int auxiliar = m_intUltimaLinea;
                            m_intUltimaLinea = m_intPrimerLinea;
                            m_intPrimerLinea = auxiliar;
                        }

                        if (numLinea < m_intPrimerLinea)
                        {
                            m_intPrimerLinea = numLinea;
                        }

                        if (numLinea > m_intUltimaLinea)
                        {
                            m_intUltimaLinea = numLinea;
                        }

                        //Cantidad de líneas seleccionadas
                        contador = m_intUltimaLinea - m_intPrimerLinea;
                        numLinea = m_intPrimerLinea;
                    }
                }

                //--------------------------------------------------------------------------------------------------------
                //Paso 2 Verifica si la línea existe en el listado, si no existe se agrega
                //       en caso de haber desmarcado la casilla se procede a eliminar de la lista
                //--------------------------------------------------------------------------------------------------------
                for (int i = 0; i <= contador && numLinea <= oMatrix.RowCount && numLinea >= 0; i++)
                {
                    chkSel = (SAPbouiCOM.CheckBox)oMatrix.Columns.Item("Col_sele").Cells.Item(numLinea).Specific;
                    codeSelected = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("Col_code").Cells.Item(numLinea).Specific).Value;
                    existe = false;
                    if (chkSel.Checked)
                    {
                        //Valida que no exista en el listado para evitar duplicados
                        for (int index = 0; index <= g_dtAdicionalesSeleccionados.Rows.Count - 1; index++)
                        {
                            if (g_dtAdicionalesSeleccionados.GetValue("code", index).ToString() == codeSelected)
                            {
                                existe = true;
                                break;
                            }
                        }

                        if (existe == false)
                        {
                            g_dtAdicionalesSeleccionados.Rows.Add(1);
                            rowNum = g_dtAdicionalesSeleccionados.Rows.Count;

                            g_dtAdicionalesSeleccionados.SetValue("sele", rowNum - 1, "Y");
                            g_dtAdicionalesSeleccionados.SetValue("code", rowNum - 1, g_dtAdicionales.GetValue("code", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("desc", rowNum - 1, g_dtAdicionales.GetValue("desc", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("bode", rowNum - 1, g_dtAdicionales.GetValue("bode", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("csto", rowNum - 1, g_dtAdicionales.GetValue("csto", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("cant", rowNum - 1, g_dtAdicionales.GetValue("cant", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("prec", rowNum - 1, g_dtAdicionales.GetValue("prec", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("mone", rowNum - 1, g_dtAdicionales.GetValue("mone", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("dura", rowNum - 1, g_dtAdicionales.GetValue("dura", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("nofa", rowNum - 1, g_dtAdicionales.GetValue("nofa", numLinea - 1));
                            g_dtAdicionalesSeleccionados.SetValue("CodBar", rowNum - 1, g_dtAdicionales.GetValue("CodeBars", numLinea - 1));
                        }
                    }
                    else
                    {
                        //Cuando una linea se ha desmarcado, se elimina del listado
                        for (int index = 0; index <= g_dtAdicionalesSeleccionados.Rows.Count - 1; index++)
                        {
                            if (g_dtAdicionalesSeleccionados.GetValue("code", index).ToString() == codeSelected)
                            {
                                rowNum = index;
                                existe = true;
                                break;
                            }
                        }
                        if (existe)
                            g_dtAdicionalesSeleccionados.Rows.Remove(rowNum);
                    }

                    numLinea += 1;
               }

            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void AgregarAdicionales(Form oForm)
        {
            SAPbouiCOM.DataTable dtAdicionalesSeleccionados;
            OrdenTrabajo objOrdenTrabajo = new OrdenTrabajo();

            try
            {
                dtAdicionalesSeleccionados = oForm.DataSources.DataTables.Item(g_strdtAdicionalesSel);
                objOrdenTrabajo.AgregaAdicionales(dtAdicionalesSeleccionados, g_IntTipoAdicional, ApplicationSBO);
                oForm.Close();

                ApplicationSBO.StatusBar.SetText(Resource.InclusionAdicionales, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CrearSolicitudEspecificos(Form oForm)
        {
            OrdenTrabajo objOrdenTrabajo = new OrdenTrabajo();
            SAPbouiCOM.Matrix oMatrix;
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralDataCollection oChildrenLinSolEs;
            SAPbobsCOM.GeneralData oGDChild;

            double precioLinea = 0;
            double precioTot = 0;
            double tipoCambio = 0;
            int lineNumeCont = 0;
            string strDocNum = string.Empty;

            try
            {
                g_dtConsulta = oForm.DataSources.DataTables.Item(g_strdtConsulta);

                oCompanyService = CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_SolEs");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                oChildrenLinSolEs = oGeneralData.Child("SCGD_SOL_ESP_LIN");

                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(g_strmtxAdicionales).Specific;
                oMatrix.FlushToDataSource();
                g_dtAdicionalesSeleccionados = oForm.DataSources.DataTables.Item(g_strdtAdicionalesSel);

                oGeneralData.SetProperty("U_NumeroOT", strNoOT);
                oGeneralData.SetProperty("U_FechaSol", DateTime.Now);
                oGeneralData.SetProperty("U_HoraSol", DateTime.Now);
                oGeneralData.SetProperty("U_UserSol", ApplicationSBO.Company.UserName);
                oGeneralData.SetProperty("U_Estado", ((int)Utilitarios.EstadoSolicitudEspecificos.Solicitado).ToString());
                oGeneralData.SetProperty("U_Moneda", strDocCur);

                if (g_dtAdicionalesSeleccionados.Rows.Count > 0)
                {
                    for (int i = 0; i <= g_dtAdicionalesSeleccionados.Rows.Count - 1; i++)
                    {
                        if (g_dtAdicionalesSeleccionados.GetValue("sele", i).ToString().Trim().Equals("Y"))
                        {
                            oGDChild = oChildrenLinSolEs.Add();
                            oGDChild.SetProperty("U_Cantidad", g_dtAdicionalesSeleccionados.GetValue("cant", i));
                            oGDChild.SetProperty("U_ItmCodeG", g_dtAdicionalesSeleccionados.GetValue("code", i).ToString().Trim());
                            oGDChild.SetProperty("U_ItmNomG", g_dtAdicionalesSeleccionados.GetValue("desc", i).ToString().Trim());
                            oGDChild.SetProperty("U_LineNum", lineNumeCont);
                            oGDChild.SetProperty("U_Moneda", g_dtAdicionalesSeleccionados.GetValue("mone", i).ToString().Trim());
                            oGDChild.SetProperty("U_Status", "O");

                            lineNumeCont += 1;
                        }
                    }
                }

                if (!CompanySBO.InTransaction)
                    CompanySBO.StartTransaction();

                oGeneralService.Add(oGeneralData);

                if (CompanySBO.InTransaction)
                {
                    g_dtConsulta.ExecuteQuery("SELECT MAX(DocNum) as DocNum FROM [@SCGD_SOL_ESPEC] with (nolock)");
                    if (g_dtConsulta.Rows.Count > 0 && !String.IsNullOrEmpty(g_dtConsulta.GetValue("DocNum", 0).ToString()))
                    {
                        strDocNum = g_dtConsulta.GetValue("DocNum", 0).ToString();
                    }

                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
                    oForm.Close();

                    ApplicationSBO.StatusBar.SetText(Resource.ProcesoFinalizado + ", " + Resource.NumeroSol + strDocNum, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                }
            }
            catch (Exception ex)
            {
                if (CompanySBO.InTransaction)
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
                throw;//Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        /// <summary>
        /// Obtiene Configuracion y verifica que consulta a realizar 
        /// </summary>
        /// <param name="oForm"></param>
        private void ObtieneEstiModYConfListPrecios(Form oForm, ref bool p_bUsaConsultaEstiMod, ref string p_strConsultaEstiMod, int p_tipoArt)
        {
            string m_strConsultaEstiMod = string.Empty;
            string m_strConsulta = string.Empty;
            string m_strUsaFiltro = string.Empty;
            try
            {
                dtConfAdmin = oForm.DataSources.DataTables.Item(g_strConfAdmin);
                dtConf = oForm.DataSources.DataTables.Item(g_strConfSucu);
                dtListPreCliente = oForm.DataSources.DataTables.Item(g_strLisPrecCliente);
                dtCodeEstiMode = oForm.DataSources.DataTables.Item(g_strEstMod);

                dtConfAdmin.ExecuteQuery("Select U_UsaAXEV,U_EspVehic,U_UsaFilRep,U_UsaFilSer from [@SCGD_ADMIN] with(nolock)");
                m_strEspecifVehi = dtConfAdmin.GetValue("U_EspVehic", 0).ToString().Trim();
                m_strUsaFilRep = dtConfAdmin.GetValue("U_UsaFilRep", 0).ToString().Trim();
                m_strUsaFilSer = dtConfAdmin.GetValue("U_UsaFilSer", 0).ToString().Trim();
                m_strUsaAsocxEspecif = dtConfAdmin.GetValue("U_UsaAXEV", 0).ToString().Trim();
                g_strEspecifVehif = m_strEspecifVehi;
                m_strConsulta = String.Format("Select DocEntry,U_CodLisPre,U_UseLisPreCli from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs=(Select U_SCGD_idSucursal from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}') ", strNoOT);
                dtConf.ExecuteQuery(m_strConsulta);
                g_strDocEntry = dtConf.GetValue("DocEntry", 0).ToString().Trim();
                m_strUsaListaPrecCliente = dtConf.GetValue("U_UseLisPreCli", 0).ToString().Trim();

                if (m_strUsaListaPrecCliente.Equals("Y"))
                {
                    dtListPreCliente.ExecuteQuery(string.Format(m_strConsultaListaPreciosCliente, strCodCliente));
                    g_strCodListPrecio = dtListPreCliente.GetValue("ListNum", 0).ToString();
                }
                else
                {
                    g_strCodListPrecio = dtConf.GetValue("U_CodLisPre", 0).ToString();
                }


                if (m_strUsaAsocxEspecif.Equals("Y"))
                {
                    p_bUsaConsultaEstiMod = true;

                    dtCodeEstiMode = oForm.DataSources.DataTables.Item(g_strEstMod);
                    m_strConsultaEstiMod = String.Format("Select U_SCGD_Cod_Estilo,U_SCGD_Cod_Modelo from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}' and U_SCGD_idSucursal = (Select U_SCGD_idSucursal  from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}')", strNoOT);
                    dtCodeEstiMode.ExecuteQuery(m_strConsultaEstiMod);
                    g_strEsti = dtCodeEstiMode.GetValue("U_SCGD_Cod_Estilo", 0).ToString().Trim();
                    g_strMod = dtCodeEstiMode.GetValue("U_SCGD_Cod_Modelo", 0).ToString().Trim();


                    if (m_strEspecifVehi.Equals("E"))
                    {

                        switch (p_tipoArt)
                        {
                            case (int)TipoAdicional.Repuesto:
                                if (m_strUsaFilRep.Equals("Y"))
                                {
                                    m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}' and Art.U_TipoArt='{1}'", g_strEsti, (int)TipoAdicional.Repuesto);
                                    g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                              g_strDocEntry, m_strUsaFiltro);
                                }
                                else
                                { p_bUsaConsultaEstiMod = false; }

                                break;
                            case (int)TipoAdicional.Servicio:
                                if (m_strUsaFilSer.Equals("Y"))
                                {
                                    m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}' and Art.U_TipoArt='{1}'", g_strEsti, (int)TipoAdicional.Servicio);
                                    g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                              g_strDocEntry, m_strUsaFiltro);
                                }
                                else
                                { p_bUsaConsultaEstiMod = false; }
                                break;
                            case (int)TipoAdicional.ServicioExterno:
                                m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}' and Art.U_TipoArt='{1}'", g_strEsti, (int)TipoAdicional.ServicioExterno);
                                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                          g_strDocEntry, m_strUsaFiltro);
                                break;
                            case (int)TipoAdicional.Suministros:
                                m_strUsaFiltro = String.Format(" and Art.U_CodEsti='{0}' and Art.U_TipoArt='{1}'", g_strEsti, (int)TipoAdicional.Suministros);
                                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                          g_strDocEntry, m_strUsaFiltro);
                                break;
                        }
                    }
                    else
                    {

                        switch (p_tipoArt)
                        {
                            case (int)TipoAdicional.Repuesto:
                                if (m_strUsaFilRep.Equals("Y"))
                                {
                                    m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}' and Art.U_TipoArt='{1}'", g_strMod, (int)TipoAdicional.Repuesto);
                                    g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                              g_strDocEntry, m_strUsaFiltro);

                                }
                                else
                                { p_bUsaConsultaEstiMod = false; }

                                break;
                            case (int)TipoAdicional.Servicio:
                                if (m_strUsaFilSer.Equals("Y"))
                                {
                                    m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}' and Art.U_TipoArt='{1}' ", g_strMod, (int)TipoAdicional.Servicio);
                                    g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                              g_strDocEntry, m_strUsaFiltro);
                                }
                                else
                                { p_bUsaConsultaEstiMod = false; }
                                break;
                            case (int)TipoAdicional.ServicioExterno:
                                m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}' and Art.U_TipoArt='{1}' ", g_strMod, (int)TipoAdicional.ServicioExterno);
                                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                          g_strDocEntry, m_strUsaFiltro);
                                break;
                            case (int)TipoAdicional.Suministros:
                                m_strUsaFiltro = String.Format(" and Art.U_CodMod='{0}' and Art.U_TipoArt='{1}' ", g_strMod, (int)TipoAdicional.Suministros);
                                g_strUsaConsultaSegunConf = String.Format(g_strConsultaArtiEspXModeEsti, g_strCodListPrecio,
                                                                          g_strDocEntry, m_strUsaFiltro);
                                break;

                        }
                    }

                    if (p_bUsaConsultaEstiMod)
                        p_strConsultaEstiMod = g_strUsaConsultaSegunConf;
                }
                else
                {
                    p_bUsaConsultaEstiMod = false;
                    // g_strUsaConsultaSegunConf = String.Format(strConsulta, strNoOT, g_strCodListPrecio);
                }
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        #endregion
    }
}
