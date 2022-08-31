using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.Requisiciones.UI;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones
{
    public partial class ListadoRequisiciones : IFormularioSBO, IUsaMenu
    {
        public void CargaSucursales()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbSucu");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            for (int j = 0; j < FormularioSBO.DataSources.DataTables.Count; j++)
            {
                if (FormularioSBO.DataSources.DataTables.Item(j).UniqueID == strDtSucursales)
                {
                    dtSucursales = FormularioSBO.DataSources.DataTables.Item(strDtSucursales);
                    blnExisteTabla = true;
                }
            }

            if (!blnExisteTabla)
            {
                dtSucursales = FormularioSBO.DataSources.DataTables.Add(strDtSucursales);
            }

            dtSucursales.Clear();
            dtSucursales.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_SUCURSALES]"));

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtSucursales.Rows.Count != 0)
            {
                for (int i = 0; i < dtSucursales.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtSucursales.GetValue("Code", i));
                    strNombre = Convert.ToString(dtSucursales.GetValue("Name", i));
                    cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }

            dtSucursales.Clear();
            var user = CompanySBO.UserName;
            dtSucursales.ExecuteQuery(String.Format("select Branch from OUSR with (nolock) where USER_CODE  = '{0}'", user));

            if (dtSucursales.Rows.Count > 0)
            {
                foreach (SAPbouiCOM.ValidValue validValue in cboCombo.ValidValues)
                {
                    if (validValue.Value == dtSucursales.GetValue("Branch", 0).ToString().Trim())
                    {
                        cboCombo.Select(validValue.Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                    }
                }
            }
        }

        public void CargaEncargadosBodega()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.ComboBox cboComboSucu;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;
            var idSucursal = string.Empty;
            var query =
                " Select distinct ln.U_Usr_UsrName Code, ln.U_Usr_Name Name from [@SCGD_CONF_MSJ] enc with (nolock) " +
                " Inner join [@SCGD_CONF_MSJLN] ln with (nolock) on enc.DocEntry = ln.DocEntry " +
                " where (enc.U_IdRol = 1 or enc.U_IdRol = 6) ";

            oItem = FormularioSBO.Items.Item("cbencBod");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            oItem = FormularioSBO.Items.Item("cbSucu");
            cboComboSucu = (SAPbouiCOM.ComboBox)(oItem.Specific);
            if (!string.IsNullOrEmpty(cboComboSucu.Value))
            {
                query = string.Format("{0} and enc.U_IdSuc = '{1}' ", query, cboComboSucu.Value);
            }

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            for (int j = 0; j < FormularioSBO.DataSources.DataTables.Count; j++)
            {
                if (FormularioSBO.DataSources.DataTables.Item(j).UniqueID == strDtEncBodega)
                {
                    dtEncBodega = FormularioSBO.DataSources.DataTables.Item(strDtEncBodega);
                    blnExisteTabla = true;
                }
            }

            if (!blnExisteTabla)
            {
                dtEncBodega = FormularioSBO.DataSources.DataTables.Add(strDtEncBodega);
            }

            dtEncBodega.Clear();
            dtEncBodega.ExecuteQuery(query);

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtEncBodega.Rows.Count != 0)
            {
                for (int i = 0; i < dtEncBodega.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtEncBodega.GetValue("Code", i));
                    strNombre = Convert.ToString(dtEncBodega.GetValue("Name", i));
                    cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }
        }

        public void CargaTipoArticulos()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.ComboBox cboComboSucu;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbTipArt");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            oItem = FormularioSBO.Items.Item("cbSucu");
            cboComboSucu = (SAPbouiCOM.ComboBox)(oItem.Specific);
            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            strCodigo = "1";
            strNombre = Resource.strRepuesto;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "3";
            strNombre = Resource.strSuministro;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            if (!string.IsNullOrEmpty(cboComboSucu.Value))
            {
                var query = string.Empty;
                if (Utilitarios.ValidaUsaOTSap())
                {
                    query = " select distinct mln.U_IDRol Rol from [@SCGD_CONF_MSJLN] mln with (nolock) " +
                        " inner join [@SCGD_CONF_MSJ] m with (nolock) on mln.DocEntry=m.DocEntry " +
                        " where mln.U_Usr_Name = '{0}' and (mln.U_IDRol =1 or mln.U_IDRol = 6) and m.U_IdSuc = '{1}'";
                    query = string.Format(query, CompanySBO.UserName, cboComboSucu.Value);

                    dtTipoArticulos.ExecuteQuery(query);

                    if (dtTipoArticulos.Rows.Count == 1)
                    {
                        var rol = dtTipoArticulos.GetValue("Rol", 0).ToString();
                        if (rol == "1")
                            cboCombo.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);
                        else
                            cboCombo.Select("3", SAPbouiCOM.BoSearchKey.psk_ByValue);
                    }
                }
                else
                {
                    query = " SELECT U_BDSucursal FROM [@SCGD_SUCURSALES] WITH (nolock) Where Code = '{0}' ";
                    query = string.Format(query, cboComboSucu.Value);
                    dtConsulta.ExecuteQuery(query);
                    string sucur = dtConsulta.GetValue(0, 0).ToString();
                    sucur = sucur.Trim();
                    query = String.Empty;
                    query = "select Propiedad, Valor from {0}.dbo.SCGTA_TB_Configuracion " +
                        " where (Propiedad = 'EncargadoRepuestos' or Propiedad = 'EncargadoSuministros') and valor like '%{1}%'";
                    var user = CompanySBO.UserName;
                    query = string.Format(query, sucur, user);

                    dtTipoArticulos.ExecuteQuery(query);

                    if (dtTipoArticulos.Rows.Count == 1)
                    {
                        String rol = dtTipoArticulos.GetValue(0, 0).ToString();
                        if (rol.Contains(Resource.strRepuestos))
                            cboCombo.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);
                        else
                        {
                            if (rol.Contains(Resource.strSuministros))
                                cboCombo.Select("3", SAPbouiCOM.BoSearchKey.psk_ByValue);
                        }
                    }
                }


            }


        }

        public void CargaTipoRequisicion()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbTipReq");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            strCodigo = "1";
            strNombre = Resource.strTransfer;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "2";
            strNombre = Resource.strDevolucion;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "3";
            strNombre = Resource.RequisicionReserva;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "4";
            strNombre = Resource.RequisicionDevolucionReserva;
            cboCombo.ValidValues.Add(strCodigo, strNombre);
        }

        public void CargaEstadosRequisicion()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbStatus");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            strCodigo = "1";
            strNombre = Resource.strPendiente;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "2";
            strNombre = Resource.strTrasladado;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            strCodigo = "3";
            strNombre = Resource.strCancelado;
            cboCombo.ValidValues.Add(strCodigo, strNombre);

            cboCombo.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);

        }

        public void CargaCombos()
        {
            CargaSucursales();
            CargaTipoArticulos();
            CargaTipoRequisicion();
            CargaEstadosRequisicion();

            //se comenta la carga del encargado de bodega ya que de momento se decidió no implementar este filtro
            //CargaEncargadosBodega();
        }

        /// <summary>
        /// Carga la matriz de Requisiciones
        /// </summary>
        /// <param name="p_CodigoBodega"></param>
        /// <param name="p_ItemCode"></param>
        public void CargarMatriz()
        {
            string CodigoTipoRequisicion = string.Empty;
            string TipoRequisicion = string.Empty;
            SAPbouiCOM.Form oForm;
            try
            {
                FormularioSBO.Freeze(true);
                DateTime dateReq = new DateTime();
                g_oMatrixListaReq = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(strMtxLsReq).Specific;
                g_oMatrixListaReq.FlushToDataSource();
                g_oEditNoReq = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoReq").Specific;
                g_oEditNoOT = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoOT").Specific;
                g_oEditNoCot = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoCot").Specific;
                g_oEditFecIni = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtFecIni").Specific;
                g_oEditFecFin = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtFecFin").Specific;
                g_oComboEstado = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbStatus").Specific);
                g_oComboTipoArticulo = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbTipArt").Specific);
                g_oComboTipoRequisicion = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbTipReq").Specific);
                g_oComboSucursal = (SAPbouiCOM.ComboBox)(FormularioSBO.Items.Item("cbSucu").Specific);

                string noReq = String.IsNullOrEmpty(g_oEditNoReq.Value) ? string.Empty : g_oEditNoReq.Value;
                string noOT = String.IsNullOrEmpty(g_oEditNoOT.Value) ? string.Empty : g_oEditNoOT.Value;
                string noCot = String.IsNullOrEmpty(g_oEditNoCot.Value) ? string.Empty : g_oEditNoCot.Value;
                string tipoArt = String.IsNullOrEmpty(g_oComboTipoArticulo.Value) ? string.Empty : g_oComboTipoArticulo.Value;
                string idSucu = String.IsNullOrEmpty(g_oComboSucursal.Value) ? string.Empty : g_oComboSucursal.Value;
                string tipoReq = String.IsNullOrEmpty(g_oComboTipoRequisicion.Value) ? string.Empty : g_oComboTipoRequisicion.Value;
                string status = String.IsNullOrEmpty(g_oComboEstado.Value) ? string.Empty : g_oComboEstado.Value;
                string fechIni = String.IsNullOrEmpty(g_oEditFecIni.Value) ? string.Empty : g_oEditFecIni.Value;
                string fechFin = String.IsNullOrEmpty(g_oEditFecFin.Value) ? string.Empty : g_oEditFecFin.Value;

                var query = string.Empty;
                query = query = "select r.DocEntry, r.U_SCGD_NoOrden, lr.U_SCGD_CodTipoArt as 'U_SCGD_TipArt', r.U_SCGD_TipoReq, r.U_SCGD_CodTipoReq, r.CreateDate, r.U_SCGD_Est Estado, " +
                    " Case When r.U_SCGD_CodEst<> 1 Then r.U_SCGD_CodEst else (select Case When (select Count(1) from [@SCGD_LINEAS_REQ] with (nolock) where DocEntry = r.DocEntry and U_SCGD_CantSol > 0 ) > 0 then 1 else 2 end) end U_SCGD_CodEst, r.CreateTime " +
                    " from [@SCGD_REQUISICIONES] r with (nolock) inner join [@SCGD_LINEAS_REQ] lr  with (nolock) on r.DocEntry = lr.DocEntry and lr.LineId = 1 " +
                    " inner join OQUT OQ with (nolock) on OQ.DocEntry = lr.U_SCGD_DocOr " ;
                if (!string.IsNullOrEmpty(noReq))
                    query = query.Contains(" Where ")
                                ? String.Format(" {0} and r.DocEntry like '%{1}%' ", query, noReq)
                                : String.Format(" {0} Where r.DocEntry like '%{1}%' ", query, noReq);


                if (!string.IsNullOrEmpty(noOT))
                    query = query.Contains(" Where ") ? String.Format(" {0} and r.U_SCGD_NoOrden like '{1}%' ", query, noOT) : String.Format(" {0} Where r.U_SCGD_NoOrden like '{1}%' ", query, noOT);

                if (!string.IsNullOrEmpty(noCot))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and OQ.DocNum = {1}", query, noCot)
                        : String.Format(" {0} Where OQ.DocNum = {1}", query, noCot);

                if (!string.IsNullOrEmpty(tipoArt))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and lr.U_SCGD_CodTipoArt = '{1}' ", query, tipoArt)
                        : String.Format("{0} Where lr.U_SCGD_CodTipoArt = '{1}' ", query, tipoArt);

                if (!string.IsNullOrEmpty(idSucu))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and r.U_SCGD_IDSuc = '{1}' ", query, idSucu)
                        : String.Format("{0} Where r.U_SCGD_IDSuc = '{1}' ", query, idSucu);

                //if (!string.IsNullOrEmpty(tipoReq))
                //    query = query.Contains(" Where ")
                //        ? String.Format("{0} and r.U_SCGD_TipoReq like '%{1}%' ", query, tipoReq == "1" ? "Transfer" : "Dev")
                //        : String.Format("{0} Where r.U_SCGD_TipoReq like '%{1}%' ", query, tipoReq == "1" ? "Transfer" : "Dev");

                if (!string.IsNullOrEmpty(tipoReq))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and r.U_SCGD_CodTipoReq = '{1}' ", query, tipoReq)
                        : String.Format("{0} Where r.U_SCGD_CodTipoReq = '{1}' ", query, tipoReq);

                if (!string.IsNullOrEmpty(status))
                    query = query.Contains(" Where ")
                        ? String.Format("{0} and (Case When r.U_SCGD_CodEst<> 1 Then r.U_SCGD_CodEst else (select Case When (select Count(1) from [@SCGD_LINEAS_REQ] with (nolock) where DocEntry = r.DocEntry and U_SCGD_CantSol > 0 ) > 0 then 1 else 2 end) end) = '{1}' ", query, status)
                        : String.Format("{0} Where (Case When r.U_SCGD_CodEst<> 1 Then r.U_SCGD_CodEst else (select Case When (select Count(1) from [@SCGD_LINEAS_REQ] with (nolock) where DocEntry = r.DocEntry and U_SCGD_CantSol > 0 ) > 0 then 1 else 2 end) end) = '{1}' ", query, status);
                if (g_oChkDate.Checked)
                {
                    if (!string.IsNullOrEmpty(fechIni))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and r.CreateDate >= '{1} 00:00:00.000' ", query, fechIni)
                                    : String.Format("{0} Where r.CreateDate >= '{1} 00:00:00.000' ", query, fechIni);

                    if (!string.IsNullOrEmpty(fechFin))
                        query = query.Contains(" Where ")
                                    ? String.Format("{0} and r.CreateDate <= '{1} 23:59:59.999' ", query, fechFin)
                                    : String.Format("{0} Where r.CreateDate <= '{1} 23:59:59.999' ", query, fechFin);
                }
                dtConsulta.Clear();
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
                query = string.Format("{0} order by CreateDate , CreateTime ", query);
                dtConsulta.ExecuteQuery(query);

                dtResultados.Rows.Clear();
                if (dtResultados == null)
                {
                    dtResultados = FormularioSBO.DataSources.DataTables.Item(strDtResultados);
                }
                if (dtResultados != null)
                {
                    for (int i = 0; i <= dtResultados.Rows.Count - 1; i++)
                    {
                        dtResultados.Rows.Remove(i);
                    }
                }
                for (int i = 0; i <= dtConsulta.Rows.Count - 1; i++)
                {
                    CodigoTipoRequisicion = dtConsulta.GetValue("U_SCGD_CodTipoReq", i).ToString();
                    if (!string.IsNullOrEmpty(dtConsulta.GetValue("U_SCGD_NoOrden", i).ToString()) || CodigoTipoRequisicion == "3" || CodigoTipoRequisicion == "4" || CodigoTipoRequisicion == "1")
                    {
                        dtResultados.Rows.Add(1);
                        String tipArt = dtConsulta.GetValue("U_SCGD_TipArt", i).ToString();
                        tipArt = tipArt == "3" ? Resource.strSuministro : Resource.strRepuesto;

                        //String tipReq = dtConsulta.GetValue("U_SCGD_TipoReq", i).ToString();
                        //tipReq = (tipReq.Contains("Trans")) ? Resource.strTransfer : Resource.strDevolucion;

                        //if (tipReq.Contains("Res"))
                        //{
                        //    tipReq = Resource.RequisicionReserva;
                        //}                        
                        
                        TipoRequisicion = ObtenerDescripcionRequisicion(CodigoTipoRequisicion);                        

                        String estado = dtConsulta.GetValue("U_SCGD_CodEst", i).ToString();
                        estado = estado == "1" || estado == "0" ? Resource.strPendiente : estado == "2" ? Resource.strTrasladado : Resource.strCancelado;

                        dtResultados.SetValue("ColNoReq", i, dtConsulta.GetValue("DocEntry", i));
                        dtResultados.SetValue("ColNoOT", i, dtConsulta.GetValue("U_SCGD_NoOrden", i));
                        dtResultados.SetValue("ColTipArt", i, tipArt);
                        //dtResultados.SetValue("ColTipReq", i, tipReq);
                        dtResultados.SetValue("ColTipReq", i, TipoRequisicion);
                        dtResultados.SetValue("ColDate", i, Convert.ToDateTime(dtConsulta.GetValue("CreateDate", i)).ToString("dd/MM/yyyy"));
                        String hora = dtConsulta.GetValue("CreateTime", i).ToString();
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
                        dtResultados.SetValue("ColHora", i, hora);
                        dtResultados.SetValue("ColStatus", i, estado);
                    }
                }
                g_oMatrixListaReq.LoadFromDataSource();
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private string ObtenerDescripcionRequisicion(string CodigoTipoRequisicion)
        {
            string Descripcion = string.Empty;
            try
            {
                switch (CodigoTipoRequisicion)
                { 
                    case "1":
                        Descripcion = Resource.strTransfer;
                        break;
                    case "2":
                        Descripcion = Resource.strDevolucion;
                        break;
                    case "3":
                        Descripcion = Resource.RequisicionReserva;
                        break;
                    case "4":
                        Descripcion = Resource.RequisicionDevolucionReserva;
                        break;
                }
                return Descripcion;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return string.Empty;
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
                                CargarMatrizCanc();
                                break;

                            case "chkDate":
                                if (g_oChkDate.Checked)
                                {
                                    FormularioSBO.Items.Item("txtFecIni").Enabled = true;
                                    FormularioSBO.Items.Item("txtFecFin").Enabled = true;
                                }
                                else
                                {
                                    FormularioSBO.Items.Item("txtNoReq").Click();
                                    FormularioSBO.Items.Item("txtFecIni").Enabled = false;
                                    FormularioSBO.Items.Item("txtFecFin").Enabled = false;
                                }
                                break;

                            case "fldCanc":
                                FormularioSBO.PaneLevel = 2;
                                break;
                            case "fldReq":
                                FormularioSBO.PaneLevel = 1;
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

        public void ManejadorEventoLinkPress(ref SAPbouiCOM.ItemEvent pval, String FormUID, ref Boolean BubbleEvent, ref FormularioRequisiciones formReq)
        {
            var reqID = string.Empty;
            SAPbouiCOM.Matrix oMatriz;

            if (pval.ItemUID == strMtxLsReq)
            {
                oMatriz = (SAPbouiCOM.Matrix)ApplicationSBO.Forms.Item(pval.FormTypeEx).Items.Item(strMtxLsReq).Specific;
                reqID = ((SAPbouiCOM.EditText)oMatriz.Columns.Item("ColNoReq").Cells.Item(pval.Row).Specific).Value.ToString().Trim();
            }
            else
            {
                oMatriz = (SAPbouiCOM.Matrix)ApplicationSBO.Forms.Item(pval.FormTypeEx).Items.Item(strMtxListCanc).Specific;
                reqID = ((SAPbouiCOM.EditText)oMatriz.Columns.Item("ColNoReq").Cells.Item(pval.Row).Specific).Value.ToString().Trim();
            }


            formReq.CargaRequisicion(reqID, "SCGD_REQ");
        }

        private void CargarMatrizCanc()
        {
            SAPbouiCOM.Form oForm;
            try
            {
                g_oMatrixListaCanc = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(strMtxListCanc).Specific;
                g_oMatrixListaCanc.FlushToDataSource();
                var query = string.Empty;
                query =
                    "SELECT DISTINCT T1.DocEntry, T1.U_SCGD_NoOrden, T2.U_SCGD_CodArticulo ,T2.U_SCGD_DescArticulo, T2.U_SCGD_CantSol " +
                    "FROM [@SCGD_REQUISICIONES] T1 WITH(NOLOCK) " +
                    "INNER JOIN [@SCGD_LINEAS_REQ] T2 WITH(NOLOCK) ON T1.DocEntry = T2.DocEntry AND T2.U_SCGD_CodEst = 1 AND T1.U_SCGD_CodTipoReq = 1 AND T1.U_SCGD_CodEst = 1 " +
                    "INNER JOIN OQUT T3 WITH(NOLOCK) ON T1.U_SCGD_NoOrden = T3.U_SCGD_Numero_OT and t2.U_SCGD_DocOr = t3.DocEntry " +
                   "INNER JOIN QUT1 T4 WITH(NOLOCK) ON T3.DocEntry = T4.DocEntry AND T2.U_SCGD_LNumOr = T4.LineNum AND T4.U_SCGD_Aprobado = 2 WHERE  U_SCGD_ItemRecha = 'Y'";
                dtListCanc.Clear();
                dtListCanc.ExecuteQuery(query);
                g_oMatrixListaCanc.LoadFromDataSource();

            }
            catch (Exception ex)
            {
                ApplicationSBO.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
