using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.Requisiciones.UI;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones
{
    public partial class ListaUbicaciones : IFormularioSBO
    {
        #region ...Metodos...

        /// <summary>
        /// Carga la matriz de ubicaciones y cantidades disponibles
        /// </summary>
        /// <param name="p_CodigoBodega"></param>
        /// <param name="p_ItemCode"></param>
        public void CargarMatriz(string p_CodigoBodega, string p_ItemCode, string p_strTipoRequisicion, string p_Busqueda)
        {

            string strQueryTraslado = "SELECT T0.AbsEntry AS 'UbiCode', T0.BinCode AS 'Ubicacion', ISNULL(T1.OnHandQty,0) AS 'OnHandQty' FROM OBIN T0 WITH (nolock) INNER JOIN OIBQ T1 WITH (nolock) ON T0.WhsCode = T1.WhsCode AND T0.AbsEntry = T1.BinAbs WHERE T0.WhsCode = '{0}' AND T1.ItemCode = '{1}' {2} ";
            string strQueryDevolucion = "SELECT T0.AbsEntry AS 'UbiCode', T0.BinCode AS 'Ubicacion', ISNULL((SELECT S1.OnHandQty FROM OIBQ S1 WITH (nolock) WHERE S1.WhsCode = T0.WhsCode AND S1.BinAbs = T0.AbsEntry AND S1.ItemCode = '{0}'),0) AS 'OnHandQty' FROM OBIN T0 WITH (nolock) WHERE T0.WhsCode = '{1}' {2} ";
            string strQueryBusquedaUbicacion = string.Empty;
            string strQuery = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(p_Busqueda))
                {
                    strQueryBusquedaUbicacion = "AND T0.AbsEntry like '{0}%'";
                    strQueryBusquedaUbicacion = string.Format(strQueryBusquedaUbicacion, p_Busqueda);
                }

                if (p_strTipoRequisicion.Equals("1"))
                {
                    //Requisición de traslado
                    strQuery = string.Format(strQueryTraslado, p_CodigoBodega, p_ItemCode, strQueryBusquedaUbicacion);
                }
                else
                { 
                    //Requisición de devolución
                    strQuery = string.Format(strQueryDevolucion, p_ItemCode, p_CodigoBodega, strQueryBusquedaUbicacion);
                }

                oForm = ApplicationSBO.Forms.Item("SCGD_SLUB");
                dtConsulta = oForm.DataSources.DataTables.Item(strDtConsulta);

                dtConsulta.ExecuteQuery(strQuery);

                if (dtUbicaciones == null)
                {
                    dtUbicaciones = oForm.DataSources.DataTables.Item(strDtUbicaciones);
                }
                if (dtUbicaciones != null)
                {
                    for (int i = 0; i <= dtUbicaciones.Rows.Count - 1; i++)
                    {
                        dtUbicaciones.Rows.Remove(i);
                    }
                }
                for (int i = 0; i <= dtConsulta.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dtConsulta.GetValue("UbiCode", i).ToString()))
                    {
                        dtUbicaciones.Rows.Add(1);
                        dtUbicaciones.SetValue("colCodUbi", i, dtConsulta.GetValue("UbiCode", i));
                        dtUbicaciones.SetValue("colDesUbi", i, dtConsulta.GetValue("Ubicacion", i));
                        dtUbicaciones.SetValue("colQtyHnd", i, dtConsulta.GetValue("OnHandQty", i));
                    }
                }
                if (dtUbicaciones.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(p_Busqueda))
                    {
                        for (int i = 0; i <= dtUbicaciones.Rows.Count - 1; i++)
                        {
                            if (dtUbicaciones.GetValue("colCodUbi", i).ToString() == "0" &&
                                string.IsNullOrEmpty(dtUbicaciones.GetValue("colDesUbi", i).ToString()))
                            {
                                dtUbicaciones.Rows.Remove(i);
                            }
                        }
                    }
                    g_oMtxUbicaciones = (SAPbouiCOM.Matrix)oForm.Items.Item(strMtxUbi).Specific;
                    g_oMtxUbicaciones.LoadFromDataSource();
                }
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        /// <summary>
        /// Carga el numero de OT en el Formulario
        /// </summary>
        public void CargaCodigos(ref SAPbouiCOM.ItemEvent pval, string codBodega, string itemCode, string lineNum)
        {
            if (string.IsNullOrEmpty(g_oEditCodBod.Value))
                g_oEditCodBod.Value = codBodega;
            if (string.IsNullOrEmpty(g_oEditItemCode.Value))
                g_oEditItemCode.Value = itemCode;
            if (string.IsNullOrEmpty(g_oEditLineNum.Value))
                g_oEditLineNum.Value = lineNum;
        }

        public void SeleccionarUbicacion(string FormUID)
        {
            int contador = 0;
            SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(FormUID);
            SAPbouiCOM.Form oFormRequisiciones = ApplicationSBO.Forms.Item(strUFormRequisicion);

            g_oEditCodBod = (SAPbouiCOM.EditText)oForm.Items.Item("txtIDBod").Specific;
            g_oEditItemCode = (SAPbouiCOM.EditText)oForm.Items.Item("txtItmCode").Specific;
            g_oEditBusqueda = (SAPbouiCOM.EditText)oForm.Items.Item("txtBus").Specific;
            g_oEditLineNum = (SAPbouiCOM.EditText)oForm.Items.Item("txtLineNum").Specific;

            string ubiCodeSelected = string.Empty;
            int linea = 0;

            g_oMtxUbicaciones = (SAPbouiCOM.Matrix)oForm.Items.Item(strMtxUbi).Specific;
            g_oMtxUbicaciones.FlushToDataSource();
            DBDataSource dbDataSource = oFormRequisiciones.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ");

            for (int i = 1; i <= g_oMtxUbicaciones.RowCount; i++)
            {
                if (g_oMtxUbicaciones.IsRowSelected(i))
                {
                    if (!String.IsNullOrEmpty(g_oEditLineNum.Value))
                    {
                        ubiCodeSelected = ((SAPbouiCOM.EditText)g_oMtxUbicaciones.Columns.Item("colCodUbi").Cells.Item(i).Specific).Value;
                        g_oMtxRequisiciones = (SAPbouiCOM.Matrix)oFormRequisiciones.Items.Item("mtxReq").Specific;
                        Int32.TryParse(g_oEditLineNum.Value, out linea);

                        g_oMtxRequisiciones.FlushToDataSource();
                        for (int j = 0; j <= dbDataSource.Size; j++)
                        {
                            if (MatrixRequisiciones.ColumnaLineNumOrigen.ObtieneValorColumnaDataTable(j, dbDataSource) == linea.ToString())
                            {
                                string strTipoRequisicion = string.Empty;
                                strTipoRequisicion = oFormRequisiciones.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_CodTipoReq", 0).ToString().Trim();
                                if (strTipoRequisicion == "1")
                                {
                                    MatrixRequisiciones.ColumnaDeUbicacion.AsignaValorDataSource(Convert.ToInt32(ubiCodeSelected), j, dbDataSource);
                                }
                                else
                                {
                                    MatrixRequisiciones.ColumnaAUbicacion.AsignaValorDataSource(Convert.ToInt32(ubiCodeSelected), j, dbDataSource);
                                }
                                
                                j = g_oMtxRequisiciones.RowCount;
                                i = g_oMtxUbicaciones.RowCount;
                            }
                        }
                    }
                }
                else
                {
                    contador = contador + 1;
                }
            }
            if (contador >= g_oMtxUbicaciones.RowCount)
            {
                ApplicationSBO.StatusBar.SetText(Resource.ErrSelectUbicacion, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning);
            }
            else
            {
                g_oMtxRequisiciones.LoadFromDataSource();
                oForm.Close();
            }
        }

        #endregion

        #region ...Eventos...

        public void ManejadorEventoDobleClick(ref SAPbouiCOM.ItemEvent pVal, String FormUID, ref Boolean BubbleEvent)
        {
            try
            {
                if (pVal.EventType == BoEventTypes.et_DOUBLE_CLICK)
                {
                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnSel":
                            case "mtxUbi":
                                SeleccionarUbicacion(FormUID);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        public void ManejadorEventoItemPress(ref SAPbouiCOM.ItemEvent pval, String FormUID, ref Boolean BubbleEvent)
        {
            try
            {
                if (pval.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (pval.ActionSuccess)
                    {
                        g_oEditCodBod = (SAPbouiCOM.EditText) oForm.Items.Item("txtIDBod").Specific;
                        g_oEditItemCode = (SAPbouiCOM.EditText) oForm.Items.Item("txtItmCode").Specific;
                        g_oEditBusqueda = (SAPbouiCOM.EditText) oForm.Items.Item("txtBus").Specific;

                        string ubiCodeSelected = string.Empty;
                        int linea = 0;

                        switch (pval.ItemUID)
                        {
                            case "btnBuscar":
                                string codBod = g_oEditCodBod.Value;
                                string itemCode = g_oEditItemCode.Value;
                                string strUbicacionBusqueda = g_oEditBusqueda.Value;
                                string strTipoRequisicion = string.Empty;

                                SAPbouiCOM.Form oFormRequisiciones = ApplicationSBO.Forms.Item(strUFormRequisicion);

                                strTipoRequisicion = oFormRequisiciones.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_CodTipoReq", 0).ToString().Trim();

                                CargarMatriz(codBod, itemCode, strTipoRequisicion, strUbicacionBusqueda);
                                break;
                            case "btnSel":
                                SeleccionarUbicacion(FormUID);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        #endregion

    }
}
