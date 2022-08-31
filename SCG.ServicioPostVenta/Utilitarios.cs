using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using DMSOneFramework.SCGCommon;
using DMS_Connector;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria;
using DMS_Connector.Data_Access;
using SAPbobsCOM;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using DataTable = SAPbouiCOM.DataTable;
using Items = SAPbobsCOM.Items;

namespace SCG.ServicioPostVenta
{
    public class Utilitarios
    {
        #region ...Declaraciones...

        public static SAPbobsCOM.SBObob sbTipoCambio = null;
        #endregion

        public enum EstadoSolicitudEspecificos
        {
            Solicitado = 0,
            Respondido = 1
        }

        public enum RolesMensajeria
        {
            EncargadoRepuestos = 1,
            EncargadoProduccion = 2,
            EncargadoSolEspec = 3,
            EncargadoCompras = 4,
            EncargadoSOE = 5,
            EncargadoSuministros = 6
        }

        public enum TipoDocumentoMarketing
        {
            OfertaCompra = 540000006,
            OrdenCompra = 22,
            EntradaMercancia = 20,
            FacturaProveedor = 18,
            NotaCredito = 19,
            DevolucionMercancia = 21
        }

        public enum TiposArticulos
        {
            scgRepuesto = 1,
            scgActividad = 2,
            scgSuministro = 3,
            scgServicioExt = 4,
            scgPaquete = 5,
            scgNinguno = 0,
            scgOtrosGastos_Costos = 11,
            scgOtrosIngresos = 12
        }

        //public const int encBodega = Convert.ToInt32(RolesMensajeria.EncargadoBodega);

        public static void CargaComboBox(
            string p_strConsulta,
            string p_strColumna1,
            string p_strColumna2,
            DataTable p_dt,
            ref ComboBox p_cboCombo,
            bool p_blnAgregarTodas,
            bool p_blnCampoVacio =  false)
        {
            string m_strConsulta;
            bool m_blnAgregadaTodas = false;

            if (String.IsNullOrEmpty(p_strConsulta) == false)
            {
                m_strConsulta = p_strConsulta;
                p_dt.ExecuteQuery(m_strConsulta);

                if (String.IsNullOrEmpty(p_dt.GetValue(p_strColumna1, 0).ToString()) == false)
                {
                    if (p_cboCombo.ValidValues.Count > 0)
                    {
                        for (int i = 0; i == p_cboCombo.ValidValues.Count - 1; i++)
                        {
                            p_cboCombo.ValidValues.Remove(p_cboCombo.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue);
                        }
                    }

                    if (p_blnCampoVacio)
                    {
                        p_cboCombo.ValidValues.Add(String.Empty, String.Empty);
                    }

                    if (p_blnAgregarTodas && m_blnAgregadaTodas == false)
                    {
                        p_cboCombo.ValidValues.Add(String.Empty, Resource.Todas);
                        m_blnAgregadaTodas = true;
                    }
                    for (int x = 0; x <= p_dt.Rows.Count - 1; x++)
                    {
                        p_cboCombo.ValidValues.Add(p_dt.GetValue(p_strColumna1, x).ToString(), p_dt.GetValue(p_strColumna2, x).ToString());
                    }
                }
            }
        }

        public static void CargaComboBox(
            string p_strConsulta, string p_strColumna1, string p_strColumna2,
            DataTable p_dt, ref Column p_Column)
        {
            string m_strConsulta;

            if (String.IsNullOrEmpty(p_strConsulta) == false)
            {
                m_strConsulta = p_strConsulta;
                p_dt.ExecuteQuery(m_strConsulta);

                if (String.IsNullOrEmpty(p_dt.GetValue(p_strColumna1, 0).ToString()) == false)
                {
                    if (p_Column.ValidValues.Count > 0)
                    {
                        int Total = p_Column.ValidValues.Count;
                        for (int i = 0; i <= Total - 1; i++)
                        {
                            p_Column.ValidValues.Remove(p_Column.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue);
                        }
                    }
                    for (int x = 0; x <= p_dt.Rows.Count - 1; x++)
                    {
                        p_Column.ValidValues.Add(p_dt.GetValue(p_strColumna1, x).ToString(), p_dt.GetValue(p_strColumna2, x).ToString());
                    }
                }
            }
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return Single.TryParse(s, out output);
        }

        internal static void CargaComboTraslado(ref Column p_Column)
        {
            try
            {
                if (p_Column.ValidValues.Count > 0)
                {
                    int Total = p_Column.ValidValues.Count;
                    for (int i = 0; i <= Total - 1; i++)
                    {
                        p_Column.ValidValues.Remove(p_Column.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue);
                    }
                }

                p_Column.ValidValues.Add("0", Resource.NoProcesado);
                p_Column.ValidValues.Add("1", Resource.No);
                p_Column.ValidValues.Add("2", Resource.Si);
                p_Column.ValidValues.Add("3", Resource.PendienteTraslado);
                p_Column.ValidValues.Add("4", Resource.PendienteBodega);

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void CargaComboAprobado(ref Column p_Column)
        {
            try
            {
                if (p_Column.ValidValues.Count > 0)
                {
                    int Total = p_Column.ValidValues.Count;
                    for (int i = 0; i <= Total - 1; i++)
                    {
                        p_Column.ValidValues.Remove(p_Column.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue);
                    }
                }

                p_Column.ValidValues.Add("1", Resource.Si);
                p_Column.ValidValues.Add("2", Resource.No);
                p_Column.ValidValues.Add("3", Resource.FaltoAprobacion);
                p_Column.ValidValues.Add("4", Resource.CambioOT);

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void CargaComboProduccion(ref ComboBox m_objCombo)
        {
            try
            {
                int m_intTamaño = m_objCombo.ValidValues.Count;

                for (int i = 0; i <= m_intTamaño - 1; i++)
                {
                    m_objCombo.ValidValues.Remove(m_objCombo.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue);
                }

                m_objCombo.ValidValues.Add(String.Empty, String.Empty);
                m_objCombo.ValidValues.Add("3", Resource.txtSuspender);
                m_objCombo.ValidValues.Add("4", Resource.Finalizar);
                m_objCombo.ValidValues.Add("5", Resource.Cancelar);

                m_objCombo.Select(String.Empty, BoSearchKey.psk_ByValue);

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void CargaComboEstadoCompra(ref Column p_Column)
        {
            try
            {
                if (p_Column.ValidValues.Count > 0)
                {
                    int Total = p_Column.ValidValues.Count;
                    for (int i = 0; i <= Total - 1; i++)
                    {
                        p_Column.ValidValues.Remove(p_Column.ValidValues.Item(0).Value,
                                                    BoSearchKey.psk_ByValue);
                    }
                }

                p_Column.ValidValues.Add("1", Resource.Comprar);
                p_Column.ValidValues.Add("0", String.Empty);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void RetornaFechaFormatoDB(DateTime p_dtFecha, Form p_oForm, ref string p_strFechaFormateada)
        {
            try
            {
                string SeparadorFecha = String.Empty;
                string SeparadorHora = String.Empty;
                string strFechaFormateada = String.Empty;
                string FormatoServer = String.Empty;
                DataTable dtUserOptions;

                string strConsultaFormatoSQL = "dbcc useroptions";
                string strDia = String.Empty;
                string strMes = String.Empty;
                string strAno = String.Empty;
                string strHora = String.Empty;
                string strMinutos = String.Empty;
                string strSeg = String.Empty;

                SeparadorFecha = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
                SeparadorHora = Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator;

                if (String.IsNullOrEmpty(SeparadorFecha) == false && String.IsNullOrEmpty(p_dtFecha.ToString()) == false)
                {
                    strMes = String.Format("{0:D2}", p_dtFecha.Month);
                    strDia = String.Format("{0:D2}", p_dtFecha.Day);
                    strAno = p_dtFecha.Year.ToString();

                    strHora = String.Format("{0:D2}", p_dtFecha.Hour);
                    strMinutos = String.Format("{0:D2}", p_dtFecha.Minute);
                    strSeg = String.Format("{0:D2}", p_dtFecha.Second);

                    dtUserOptions = p_oForm.DataSources.DataTables.Item("tConsulta");
                    dtUserOptions.ExecuteQuery(strConsultaFormatoSQL);

                    FormatoServer = dtUserOptions.GetValue(1, 2).ToString().Trim();

                    switch (FormatoServer)
                    {
                        case "dmy":
                            strFechaFormateada = String.Format(strDia + "{0}" + strMes + "{0}" + strAno, SeparadorFecha);
                            break;
                        case "dym":
                            strFechaFormateada = String.Format(strDia + "{0}" + strAno + "{0}" + strMes, SeparadorFecha);
                            break;
                        case "mdy":
                            strFechaFormateada = String.Format(strMes + "{0}" + strDia + "{0}" + strAno, SeparadorFecha);
                            break;
                        case "myd":
                            strFechaFormateada = String.Format(strMes + "{0}" + strAno + "{0}" + strDia, SeparadorFecha);
                            break;
                        case "ymd":
                            strFechaFormateada = String.Format(strAno + "{0}" + strMes + "{0}" + strDia, SeparadorFecha);
                            break;
                        case "ydm":
                            strFechaFormateada = String.Format(strAno + "{0}" + strDia + "{0}" + strMes, SeparadorFecha);
                            break;
                    }
                    p_strFechaFormateada = strFechaFormateada;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void DevuelveSerie(int intSeries, Form p_oForm, Company oCompany, ref string strEtiquetadeSeries)
        {
            DataTable dtConsulta;
            try
            {
                string strConsultaEtiquetadeSerie = "Select SeriesName" +
                                                    " From NNM1" +
                                                    " Where Series =" + intSeries;


                dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta");

                dtConsulta.ExecuteQuery(strConsultaEtiquetadeSerie);

                strEtiquetadeSeries = dtConsulta.GetValue("SeriesName", 0).ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void EnviarMensaje(String p_strMensaje, string p_strUserCode, SAPbobsCOM.Company p_companySBO)
        {
            Messages oMsg;
            int intResultado;
            int intError;
            String strError = String.Empty;
            try
            {
                oMsg = (Messages) p_companySBO.GetBusinessObject(BoObjectTypes.oMessages);
                oMsg.MessageText = p_strMensaje;
                oMsg.Subject = oMsg.MessageText;

                oMsg.Recipients.Add();
                oMsg.Recipients.SetCurrentLine(0);
                oMsg.Recipients.UserCode = p_strUserCode;
                oMsg.Recipients.NameTo = p_strUserCode;
                oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;

                intResultado = oMsg.Add();
                if (intResultado != 0)
                {
                    p_companySBO.GetLastError(out intError, out strError);
                    throw new ExceptionsSBO(intError, strError);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void CreaMensajeSBO(String p_strMensaje, String p_strDocEntry, Company p_ocompany, String p_strNoOrden, Boolean blnDraft, String strIdSuc, Boolean p_bNewUpdate, DMS_Connector.Data_Access.GeneralEnums.RolesMensajeria _pRol, Boolean p_FinishOT)
        {
            Messages oMsg;
            //DataTable dtConsulta;
            int intResultado;
            string strError;
            int intError;
            var intindiceUsuarios = 0;
            Mensajeria_Lineas linea = default(Mensajeria_Lineas);
            List<Mensajeria_Lineas> lstLineas = default(List<Mensajeria_Lineas>);
            string strRolCode = string.Empty;

            try
            {
                strRolCode = Convert.ToInt32(_pRol).ToString();

                if (Configuracion.ConfMensajeria.Any(x => x.U_IdSuc == strIdSuc && x.U_IdRol == strRolCode))
                {
                    lstLineas = Configuracion.ConfMensajeria.First(x => x.U_IdSuc == strIdSuc && x.U_IdRol == strRolCode).Mensajeria_Lineas;

                    if (lstLineas.Count >= 1)
                    {
                        if (!String.IsNullOrEmpty(lstLineas[0].U_Usr_UsrName))
                        {
                            string rolID = ((int)(GeneralEnums.RolesMensajeria.EncargadoProduccion)).ToString();

                            switch (_pRol)
                            {
                                case GeneralEnums.RolesMensajeria.EncargadoProduccion:
                                    //Crea el mensaje
                                    if (p_bNewUpdate)
                                    {
                                        oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                        oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                        oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;
                                    }
                                    else
                                    {
                                        if (blnDraft)
                                        {
                                            oMsg =
                                                (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                            oMsg.MessageText = String.Format(Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden);
                                            oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;
                                        }
                                        else
                                        {
                                            oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                            oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                            oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;
                                        }
                                    }
                                    for (intindiceUsuarios = 0; intindiceUsuarios <= lstLineas.Count - 1; intindiceUsuarios++)
                                    {
                                        linea = lstLineas[intindiceUsuarios];
                                        //for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                        oMsg.Recipients.Add();
                                        oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                        oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                    }
                                    //verifica que el documento creado sea un draft
                                    if (!p_FinishOT)
                                    {
                                        if (!p_bNewUpdate)
                                        {
                                            if (!blnDraft)
                                            {
                                                oMsg.AddDataColumn(Resource.MensajeFavorRevisar,
                                                                   Resource.Traslado + "," + Resource.Referencia + ": " +
                                                                   p_strDocEntry.ToString(),
                                                                   BoObjectTypes.oStockTransfer,
                                                                   p_strDocEntry.ToString());
                                            }
                                        }
                                    }

                                    intResultado = oMsg.Add();
                                    if (intResultado != 0)
                                    {
                                        p_ocompany.GetLastError(out intError, out strError);
                                        throw new ExceptionsSBO(intError, strError);
                                    }
                                    break;
                                case GeneralEnums.RolesMensajeria.EncargadoRepuestos:
                                case GeneralEnums.RolesMensajeria.EncargadoSuministros:
                                    //Crea el mensaje
                                    if (blnDraft)
                                    {
                                        oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                        oMsg.MessageText = String.Format(Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden);
                                        oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;
                                    }
                                    else
                                    {
                                        oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                        oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                        oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;
                                    }

                                    for (intindiceUsuarios = 0; intindiceUsuarios <= lstLineas.Count - 1; intindiceUsuarios++)
                                    {
                                        linea = lstLineas[intindiceUsuarios];
                                        //for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                        //{
                                        oMsg.Recipients.Add();
                                        oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                        oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                    }
                                    //verifica que el documento creado sea un draft
                                    if (!p_bNewUpdate)
                                    {
                                        if (!blnDraft)
                                        {
                                            oMsg.AddDataColumn(Resource.MensajeFavorRevisar, Resource.Traslado + "," + Resource.Referencia + ": " +
                                                               p_strDocEntry, BoObjectTypes.oStockTransfer, p_strDocEntry);
                                        }
                                    }

                                    intResultado = oMsg.Add();
                                    if (intResultado != 0)
                                    {
                                        p_ocompany.GetLastError(out intError, out strError);
                                        throw new ExceptionsSBO(intError, strError);
                                    }
                                    break;
                                case GeneralEnums.RolesMensajeria.EncargadoSOE:
                                case GeneralEnums.RolesMensajeria.EncargadoCompras:
                                case GeneralEnums.RolesMensajeria.EncargadoSolEspec:
                                    //Crea el mensaje
                                    oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                    oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                    oMsg.Subject = oMsg.MessageText.Length > 50
                                                               ? oMsg.MessageText.Substring(0, 50)
                                                               : oMsg.MessageText;

                                    for (intindiceUsuarios = 0; intindiceUsuarios <= lstLineas.Count - 1; intindiceUsuarios++)
                                    {
                                        linea = lstLineas[intindiceUsuarios];
                                        //for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                        //{
                                        oMsg.Recipients.Add();
                                        oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                        oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim();
                                        oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                    }
                                    intResultado = oMsg.Add();
                                    if (intResultado != 0)
                                    {
                                        p_ocompany.GetLastError(out intError, out strError);
                                        throw new ExceptionsSBO(intError, strError);
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores(ex, DMS_Connector.Company.ApplicationSBO);
            }
        }

        public static void CreaMensajeSBO(String p_strMensaje, String p_strDocEntry, Company p_ocompany, String p_strNoOrden, Boolean blnDraft, String p_strRolCode, String strIdSuc, Form p_oForm, String p_strLocalDT, Boolean p_bNewUpdate, RolesMensajeria _pRol, Boolean p_FinishOT, Application _aplicationSBO = null)
        //Crea mensaje en SAP para el bodeguero sobre creacion de un documento de traslado
        {
            try
            {

                Messages oMsg;
                DataTable dtConsulta;
                int intResultado;
                string strError;
                int intError;
                var intindiceUsuarios = 0;
                var query = String.Empty;
                string rolEncargadoProduccion = ((int)RolesMensajeria.EncargadoProduccion).ToString();
                string rolEncargadoBodega = ((int)RolesMensajeria.EncargadoRepuestos).ToString();
                string rolEncargadoSuministros = ((int)RolesMensajeria.EncargadoSuministros).ToString();
                string rolEncargadoCompras = ((int)RolesMensajeria.EncargadoCompras).ToString();
                string rolEncargadoSOE = ((int)RolesMensajeria.EncargadoSOE).ToString();
                string rolEncargadoSolEspec = ((int)RolesMensajeria.EncargadoSolEspec).ToString();

                query = "select l.U_EmpCode code, l.U_Usr_Name name, l.U_Usr_UsrName userId " +
                        "from [@SCGD_CONF_MSJ] m " +
                        "inner join  [@SCGD_CONF_MSJLN] l on m.DocEntry=l.DocEntry " +
                        "where m.U_IdRol = '{0}' and m.U_IdSuc = '{1}' ";

                query = String.Format(query, p_strRolCode, strIdSuc);
                if (String.IsNullOrEmpty(p_strLocalDT))
                {
                    dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta");
                }
                else
                {
                    dtConsulta = p_oForm.DataSources.DataTables.Item(p_strLocalDT);
                }

                dtConsulta.ExecuteQuery(query);

                if (dtConsulta.Rows.Count >= 1)
                {
                    if (!String.IsNullOrEmpty(dtConsulta.GetValue("userId", 0).ToString()))
                    {
                        string rolID = ((int)(RolesMensajeria.EncargadoProduccion)).ToString();

                        switch (_pRol)
                        {
                            case RolesMensajeria.EncargadoProduccion:
                                //Crea el mensaje
                                if (p_bNewUpdate)
                                {
                                    oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                    oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                    oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;
                                }
                                else
                                {
                                    if (blnDraft)
                                    {
                                        oMsg =
                                            (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                        oMsg.MessageText = String.Format(Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden);
                                        oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;
                                    }
                                    else
                                    {
                                        oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                        oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                        oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;
                                    }
                                }
                                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                {
                                    oMsg.Recipients.Add();
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", i).ToString().Trim();
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", i).ToString().Trim();
                                    oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                }
                                //verifica que el documento creado sea un draft
                                if (!p_FinishOT)
                                {
                                    if (!p_bNewUpdate)
                                    {
                                        if (!blnDraft)
                                        {
                                            oMsg.AddDataColumn(Resource.MensajeFavorRevisar,
                                                               Resource.Traslado + "," + Resource.Referencia + ": " +
                                                               p_strDocEntry.ToString(),
                                                               BoObjectTypes.oStockTransfer,
                                                               p_strDocEntry.ToString());
                                        }
                                    }
                                }

                                intResultado = oMsg.Add();
                                if (intResultado != 0)
                                {
                                    p_ocompany.GetLastError(out intError, out strError);
                                    throw new ExceptionsSBO(intError, strError);
                                }
                                break;
                            case Utilitarios.RolesMensajeria.EncargadoRepuestos:
                            case Utilitarios.RolesMensajeria.EncargadoSuministros:
                                //Crea el mensaje
                                if (blnDraft)
                                {
                                    oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                    oMsg.MessageText = String.Format(Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden);
                                    oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;
                                }
                                else
                                {
                                    oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                    oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                    oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;
                                }
                                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                {
                                    oMsg.Recipients.Add();
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim();
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim();
                                    oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                }
                                //verifica que el documento creado sea un draft
                                if (!p_bNewUpdate)
                                {
                                    if (!blnDraft)
                                    {
                                        oMsg.AddDataColumn(Resource.MensajeFavorRevisar, Resource.Traslado + "," + Resource.Referencia + ": " +
                                                           p_strDocEntry, BoObjectTypes.oStockTransfer, p_strDocEntry);
                                    }
                                }

                                intResultado = oMsg.Add();
                                if (intResultado != 0)
                                {
                                    p_ocompany.GetLastError(out intError, out strError);
                                    throw new ExceptionsSBO(intError, strError);
                                }
                                break;
                            case RolesMensajeria.EncargadoSOE:
                            case RolesMensajeria.EncargadoCompras:
                            case RolesMensajeria.EncargadoSolEspec:
                                //Crea el mensaje
                                oMsg = (Messages)p_ocompany.GetBusinessObject(BoObjectTypes.oMessages);
                                oMsg.MessageText = String.Format("{0} {1}: {2}", p_strMensaje, Resource.OT, p_strNoOrden);
                                oMsg.Subject = oMsg.MessageText.Length > 50
                                                           ? oMsg.MessageText.Substring(0, 50)
                                                           : oMsg.MessageText;

                                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                {
                                    oMsg.Recipients.Add();
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios);
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim();
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim();
                                    oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                                }
                                intResultado = oMsg.Add();
                                if (intResultado != 0)
                                {
                                    p_ocompany.GetLastError(out intError, out strError);
                                    throw new ExceptionsSBO(intError, strError);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores(ex, _aplicationSBO);
            }
        }

        public static void ManejadorErrores(Exception ex, Application sboApplication)
        {
            DMS_Connector.Helpers .ManejoErrores( ex);
        }

        public static int CargarTipoSkin(Application sboApplication)
        {

            var tipo = sboApplication.GetType();
            var metodo = tipo.GetMethod("get_SkinStyle");
            Object skin;

            if (metodo != null)
            {
                skin = metodo.Invoke(sboApplication, null);
            }
            else
            {
                return 0;
            }
            return Convert.ToInt32(skin);
        }
        
        /// <summary>
        /// VAlida si existe un data table con el nombre especificado
        /// </summary>
        /// <param name="oForm">Formulario SBO</param>
        /// <param name="dtName">Nombre Data Table a consultar</param>
        /// <returns>bool que indica si existe o no un data table con ese nombre</returns>
        public static Boolean ValidaSiDataTableExiste(Form oForm, string dtName)
        {
            var result = false;
            for (int i = 0; i < oForm.DataSources.DataTables.Count; i++)
            {
                if (oForm.DataSources.DataTables.Item(i).UniqueID == dtName)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Obtiene descuento de item para el SN
        /// </summary>
        /// <param name="p_cardCode">Codigo de SN</param>
        /// <param name="p_itemCode">Codigo del Item</param>
        /// <returns>Porcentaje de descuento</returns>
        /// <remarks></remarks>
        public static double GetItemDiscount(SAPbobsCOM.Company p_oCompany, string p_cardCode, string p_itemCode)
        {
            SAPbobsCOM.SpecialPrices oSpecialPrice;
            SAPbobsCOM.DiscountGroups oDiscountGroups;
            SAPbobsCOM.BusinessPartners oBusinessPartners;
            SAPbobsCOM.Items oItem;
            SAPbobsCOM.ItemProperties oItemProperties;
            double dbDiscount = 0;
            int count = 0;
            bool blnExit = false;
            bool blnFirst = true;
            oSpecialPrice = (SAPbobsCOM.SpecialPrices)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices);
            oBusinessPartners = (SAPbobsCOM.BusinessPartners)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            oItem = (SAPbobsCOM.Items)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
            oItemProperties = (SAPbobsCOM.ItemProperties)p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItemProperties);

            if (oSpecialPrice.GetByKey(p_itemCode, p_cardCode))
            {
                dbDiscount = oSpecialPrice.DiscountPercent;
            }

            if (dbDiscount == 0 && oItem.GetByKey(p_itemCode) && oBusinessPartners.GetByKey(p_cardCode))
            {
                oDiscountGroups = oBusinessPartners.DiscountGroups;
                for (int index = 0; index <= oDiscountGroups.Count - 1; index++)
                {
                    oDiscountGroups.SetCurrentLine(index);
                    switch (oDiscountGroups.BaseObjectType)
                    {
                        case DiscountGroupBaseObjectEnum.dgboItemGroups:
                            if (oItem.ItemsGroupCode == Int32.Parse((oDiscountGroups.ObjectEntry)))
                            {
                                dbDiscount = oDiscountGroups.DiscountPercentage;
                                blnExit = true;
                            }
                            break;
                        case DiscountGroupBaseObjectEnum.dgboItemProperties:
                            if (oItemProperties.GetByKey(Int32.Parse(oDiscountGroups.ObjectEntry)))
                            {
                                if (oItem.Properties[oItemProperties.Number] == BoYesNoEnum.tYES)
                                {
                                    if (!blnFirst)
                                    {
                                        switch (oBusinessPartners.DiscountRelations)
                                        {
                                            case DiscountGroupRelationsEnum.dgrLowestDiscount:
                                                if (oDiscountGroups.DiscountPercentage < dbDiscount)
                                                {
                                                    dbDiscount = oDiscountGroups.DiscountPercentage;
                                                }
                                                break;
                                            case DiscountGroupRelationsEnum.dgrHighestDiscount:
                                                if (oDiscountGroups.DiscountPercentage > dbDiscount)
                                                {
                                                    dbDiscount = oDiscountGroups.DiscountPercentage;
                                                }
                                                break;
                                            case DiscountGroupRelationsEnum.dgrAverageDiscount:
                                                dbDiscount += oDiscountGroups.DiscountPercentage;
                                                count += 1;

                                                break;
                                            case DiscountGroupRelationsEnum.dgrDiscountTotals:
                                                dbDiscount += oDiscountGroups.DiscountPercentage;
                                                break;
                                            case DiscountGroupRelationsEnum.dgrMultipliedDiscount:

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        dbDiscount = oDiscountGroups.DiscountPercentage;
                                        count += 1;
                                        blnFirst = false;
                                    }
                                }
                            }
                            break;
                        case (DiscountGroupBaseObjectEnum.dgboManufacturer):
                            if (oItem.Manufacturer == Int32.Parse(oDiscountGroups.ObjectEntry))
                            {
                                dbDiscount = oDiscountGroups.DiscountPercentage;
                                blnExit = true;
                            }
                            break;
                        case DiscountGroupBaseObjectEnum.dgboItems:

                            break;
                    }
                    if (blnExit) break;
                }
                if (count != 0 && oBusinessPartners.DiscountRelations == DiscountGroupRelationsEnum.dgrAverageDiscount)
                {
                    dbDiscount /= count;
                }
            }
            return dbDiscount;
        }

        /// <summary>
        /// Retorna moneda local
        /// </summary>
        /// <returns>Retorna moneda local</returns>
        /// <remarks></remarks>
        public static string RetornarMonedaLocal(ref SAPbobsCOM.Company p_companySBO)
        {
            SAPbobsCOM.SBObob oSBObob;
            string sToday = null;
            SAPbobsCOM.Recordset oRecordset;
            string strResult;

            try
            {
                oSBObob = (SAPbobsCOM.SBObob)p_companySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                oRecordset = (SAPbobsCOM.Recordset)p_companySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                oRecordset = oSBObob.GetLocalCurrency();
                strResult = oRecordset.Fields.Item(0).Value.ToString().Trim();

                return strResult;

            }
            catch (Exception ex)
            {
                return "-1";
            }

        }

        /// <summary>
        /// Retorna moneda Sistema
        /// </summary>
        /// <returns>Retorna moneda Sistema</returns>
        /// <remarks></remarks>
        public static string RetornarMonedaSistema(ref SAPbobsCOM.Company p_companySBO)
        {
            SAPbobsCOM.SBObob oSBObob;
            string sToday = null;
            SAPbobsCOM.Recordset oRecordset;
            string strResult;


            try
            {
                oSBObob = (SAPbobsCOM.SBObob)p_companySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                oRecordset = (SAPbobsCOM.Recordset)p_companySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                oRecordset = oSBObob.GetSystemCurrency();
                strResult = oRecordset.Fields.Item(0).Value.ToString().Trim();

                return strResult;

            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        public static double ManejoMultimoneda(double PrecioSinProcesar, string strMonedaLocal, string strMonedaSistema, string strMonedaDoc2, string strMonedaDoc1, double dblTipoCambioDoc1, System.DateTime dtFechaDoc1, System.Globalization.NumberFormatInfo n, SAPbobsCOM.Company m_oCompany)
        {
            SAPbobsCOM.Recordset rsTipoCambio = null;
            double dcPrecioProcesado = 0;
            double dcTCDoc1 = 0;
            double dblTipoCambioSistema = 0;
            double dcTCMS = 0;
            double dblTCME = 0;
            double dcTCME = 0;


            try
            {
                if (sbTipoCambio == null)
                {
                    sbTipoCambio = (SAPbobsCOM.SBObob)m_oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
                }
                rsTipoCambio = (SAPbobsCOM.Recordset)m_oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (strMonedaLocal != strMonedaSistema)
                {
                    rsTipoCambio = sbTipoCambio.GetCurrencyRate(strMonedaSistema, dtFechaDoc1);
                    dblTipoCambioSistema = (double)rsTipoCambio.Fields.Item(0).Value;
                }

                if (dblTipoCambioSistema == 0)
                {
                     dblTipoCambioSistema = 1;
                }
                   
                dcTCMS = dblTipoCambioSistema;

                if (dblTipoCambioDoc1 == 0)
                {
                     dblTipoCambioDoc1 = 1;
                }
                   
                dcTCDoc1 = dblTipoCambioDoc1;

                if (strMonedaDoc1 == strMonedaLocal)
                {
                    if (strMonedaDoc2 == strMonedaLocal || string.IsNullOrEmpty(strMonedaDoc2))
                    {
                        dcPrecioProcesado = PrecioSinProcesar;
                    }
                    else if (strMonedaDoc2 == strMonedaSistema)
                    { dcPrecioProcesado = PrecioSinProcesar * dcTCMS; }
                    else
                    {
                        rsTipoCambio = sbTipoCambio.GetCurrencyRate(strMonedaDoc2, dtFechaDoc1);
                        dblTCME = (double)rsTipoCambio.Fields.Item(0).Value;

                        if (dblTCME == 0)
                        {
                             dblTCME = 1;
                        }
                           
                        dcTCME = dblTCME;
                        dcPrecioProcesado = PrecioSinProcesar * dcTCME;
                    }
                }
                else if (strMonedaDoc1 == strMonedaSistema)
                {
                    if (strMonedaDoc2 == strMonedaLocal || string.IsNullOrEmpty(strMonedaDoc2))
                    {
                        dcPrecioProcesado = PrecioSinProcesar / dcTCDoc1;
                    }
                    else if (strMonedaDoc2 == strMonedaSistema)
                    {
                        dcPrecioProcesado = PrecioSinProcesar;
                    }
                    else
                    {
                        rsTipoCambio = sbTipoCambio.GetCurrencyRate(strMonedaDoc2, dtFechaDoc1);
                        dblTCME = (double)rsTipoCambio.Fields.Item(0).Value;
                        if (dblTCME == 0)
                        {
                              dblTCME = 1;
                        }
                          
                        dcTCME = dblTCME;
                        dcPrecioProcesado = (PrecioSinProcesar * dcTCME) / dcTCDoc1;
                    }
                }
                else
                {
                    if (strMonedaDoc2 == strMonedaLocal || string.IsNullOrEmpty(strMonedaDoc2))
                    {
                        dcPrecioProcesado = PrecioSinProcesar / dcTCDoc1;
                    }
                    else if (strMonedaDoc2 == strMonedaSistema)
                    {
                        dcPrecioProcesado = (PrecioSinProcesar * dcTCMS) / dcTCDoc1;
                    }
                    else
                    {
                        dcPrecioProcesado = PrecioSinProcesar;
                    }
                }

                return dcPrecioProcesado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DestruirObjeto(ref rsTipoCambio);
            }
        }

        public static double RetornarTipoCambioMonedaRS(string p_strMonedaSistema, string p_strMonedaLocal, System.DateTime p_DateDoc, SAPbobsCOM.Company oCompany)
        {
            SAPbobsCOM.SBObob oSBObob;
            string sToday = null;
            SAPbobsCOM.Recordset oRecordset = null;
            double dblResult = 0;
            string result = string.Empty;
            string query = null;
            try
            {
                oSBObob = (SAPbobsCOM.SBObob)oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (p_strMonedaLocal != p_strMonedaSistema)
                {
                    oRecordset = oSBObob.GetCurrencyRate(p_strMonedaSistema, p_DateDoc);
                    result = oRecordset.Fields.Item(0).Value.ToString();
                }

                if (string.IsNullOrEmpty(result))
                    result = "1";
                dblResult = double.Parse(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DestruirObjeto(ref oRecordset);
            }
            return dblResult;
        }


        public static string DevuelveValorSN(string strCardCode, string strUDfName)
        {
            BusinessPartners oBusinessPartners = default(BusinessPartners);
            string valorUDF = null;

            try
            {
                oBusinessPartners = (BusinessPartners)DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (oBusinessPartners.GetByKey(strCardCode))
                {
                    valorUDF = oBusinessPartners.UserFields.Fields.Item(strUDfName).Value.ToString();
                }
                return valorUDF;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DestruirObjeto(ref oBusinessPartners);
            }

        }
        
        
        /// <summary>
        /// Libera memmoria al destruir objetos despues de su uso
        /// </summary>
        /// <param name="objDocumento">Objeto a destruir</param>
        public static void DestruirObjeto<T>(ref T objDocumento)
        {
            if ((objDocumento != null))
            {
                Marshal.ReleaseComObject(objDocumento);
                objDocumento = default(T);
            }
        }

        
    }
 
}
