//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DMS_Connector.Business_Logic.DataContract.Requisiciones;
//using DMS_Connector.Data_Access;
//using SAPbobsCOM;
//using SAPbouiCOM;
//using SCG.Requisiciones.UI;

//namespace SCG.Requisiciones
//{
//    public class ManejadorRequisicionesTraslados
//    {
//        #region ...Declaraciones...

//        private SAPbobsCOM.Company _company;
//        private Application _application;
//        private const string strNoOT = "U_SCGD_NoOrden";
//        private string strIdSucursal;
//        private const string strRequisicion = "@SCGD_REQUISICIONES";
//        private const string strCRec = "U_SCGD_CRec";
//        private const string strCPenBod = "U_SCGD_CPBo";
//        private const string strCDe = "U_SCGD_CPDe";
//        private bool m_blnConfOTSAP = false;

//        #endregion

//        #region ...Propiedades...

//        public SAPbobsCOM.Company Company
//        {
//            get { return _company; }
//        }

//        public Application ApplicationSBO
//        {
//            get { return _application; }
//        }

//        #endregion

//        #region ...Constructor...

//        public ManejadorRequisicionesTraslados(SAPbobsCOM.Company company, Application application, bool p_blnConfInterna)
//        {
//            _company = company;
//            _application = application;
//            m_blnConfOTSAP = p_blnConfInterna;
//        }

//        #endregion

//        #region ...Metodos...

//        public Boolean TrasladoRealizado(ref SAPbobsCOM.Documents p_oCotizacion, ref List<LineaRequisicion> p_lineastransferidas, List<StockTransfer> p_TransferenciasList, string TipoReq)
//        {
//            SAPbouiCOM.Form oForm = default(SAPbouiCOM.Form);
//            string strTipoArticulo;
//            string mensaje;
//            var result = true;
//            try
//            {
//                oForm = ApplicationSBO.Forms.Item("SCGD_FormRequisicion");

//                if (ActualizarLineasDeLaCotizacion(ref p_oCotizacion, ref p_lineastransferidas, TipoReq))
//                {
//                    foreach (StockTransfer stockTransfer in p_TransferenciasList)
//                    {
//                        stockTransfer.Lines.SetCurrentLine(0);
//                        strTipoArticulo = stockTransfer.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim();
//                        strTipoArticulo = strTipoArticulo == ((int)GeneralEnums.TipoArticulo.Repuesto).ToString() ? Resource.strRepuesto : Resource.strSuministro;

//                        //mensaje = string.Format("{0} {1}", Resource.MensajeTraslado, strTipoArticulo);
//                        //Utilitarios.CreaMensajeSBO(mensaje, stockTransfer.DocEntry.ToString(), p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString(), false, oForm.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_IDSuc", 0).Trim(), false, GeneralEnums.RolesMensajeria.EncargadoProduccion, false);
//                    }
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                result = false;
//                //Revisar
//                //Utilitarios.ManejadorErrores(ex);
//            }
//            return result;
//        }

//        public Boolean ActualizarLineasDeLaCotizacion(ref SAPbobsCOM.Documents p_oCotizacion, ref List<LineaRequisicion> p_oRequisicionList, string strTipoReq)
//        {
//            SAPbobsCOM.Document_Lines m_oLineasCotizacion = default(SAPbobsCOM.Document_Lines);
//            bool result = true;
//            double dblCantidadRequisicion = 0;
//            double dblCantidadPendienteBodega = 0;
//            double dblCantidadPendiente = 0;
//            double dblCantidadPendienteDevolucion = 0;
//            double dblCPenDev = 0;
//            double cPenBod = 0;
//            double cRec = 0;
//            double cPenDev = 0;

//            try
//            {
//                m_oLineasCotizacion = p_oCotizacion.Lines;

//                foreach (LineaRequisicion linea in p_oRequisicionList)
//                {
//                    for (int i = 0; i <= m_oLineasCotizacion.Count - 1; i++)
//                    {
//                        m_oLineasCotizacion.SetCurrentLine(i);

//                        if (linea.U_SCGD_LNumOr == m_oLineasCotizacion.LineNum && linea.U_SCGD_CantRec == linea.U_SCGD_CantSol)
//                        {
//                            if (!(m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value == "2"))
//                            {
//                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2;
//                            }
//                        }

//                        if (linea.U_SCGD_LNumOr == m_oLineasCotizacion.LineNum && (linea.U_SCGD_CodArticulo == m_oLineasCotizacion.ItemCode))
//                        {
//                            bool blnModificarCantidadRecibida = false;

//                            if (m_blnConfOTSAP)
//                            {
//                                dblCantidadRequisicion = linea.U_SCGD_CantATransf;

//                                if (m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value != null)
//                                    dblCantidadPendienteDevolucion = (double)m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value;

//                                dblCPenDev = dblCantidadPendienteDevolucion;

//                                if ((strTipoReq.Contains("Trans")))
//                                {
//                                    if (dblCantidadPendienteDevolucion != 0)
//                                    {
//                                        dblCantidadPendienteDevolucion -= dblCantidadRequisicion;
//                                        blnModificarCantidadRecibida = true;
//                                    }
//                                    m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value = dblCantidadPendienteDevolucion;

//                                    if (m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value != null)
//                                        dblCantidadPendienteBodega = (double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value;

//                                    dblCantidadPendienteBodega -= dblCantidadRequisicion;

//                                    if (dblCantidadPendienteBodega >= 0)
//                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = dblCantidadPendienteBodega;
//                                    else
//                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0;

//                                    if (blnModificarCantidadRecibida)
//                                    {
//                                        if (dblCPenDev > 0)
//                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value + dblCantidadRequisicion;
//                                        else
//                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value - dblCantidadRequisicion;
//                                    }
//                                    else
//                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = (Double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value + dblCantidadRequisicion;
//                                }
//                                else
//                                {
//                                    cPenBod = Convert.ToDouble(m_oLineasCotizacion.UserFields.Fields.Item(strCPenBod).Value.ToString());
//                                    cRec = Convert.ToDouble(m_oLineasCotizacion.UserFields.Fields.Item(strCRec).Value.ToString());
//                                    cPenDev = Convert.ToDouble(m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value.ToString());

//                                    if (dblCantidadPendienteDevolucion != 0)
//                                    {
//                                        dblCantidadPendienteDevolucion -= dblCantidadRequisicion;
//                                        blnModificarCantidadRecibida = true;
//                                    }
//                                    m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value = dblCantidadPendienteDevolucion;

//                                    if (dblCantidadPendienteDevolucion == 0)
//                                    {
//                                        if (m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim() == "2")
//                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
//                                    }

//                                    if (blnModificarCantidadRecibida)
//                                    {
//                                        cPenDev = Convert.ToDouble(m_oLineasCotizacion.UserFields.Fields.Item(strCDe).Value.ToString());
//                                        if (dblCPenDev > 0)
//                                        {
//                                            if ((cPenBod + cRec + cPenDev) > 0)
//                                                m_oLineasCotizacion.Quantity = cPenBod + cRec + cPenDev;
//                                        }
//                                        else
//                                            m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value + dblCantidadRequisicion;
//                                    }
//                                    else
//                                    {
//                                        m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value + dblCantidadRequisicion;
//                                    }
//                                }
//                            }
//                            break;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                result = false;
//                //Revisar
//                //Utilitarios.ManejadorErrores(ex);
//            }
//            return result;
//        }

//        public void LineasCanceladas(ref SAPbobsCOM.Documents p_oCotizacion, RequisicionData p_oRequisicion, ref List<LineaRequisicion> p_LineasCanceladas)
//        {
//            string m_strIdRepxOrden = null;
//            string m_intIdSucursal = null;
//            string m_strBaseDeDatos = null;
//            string m_strItemcode = null;
//            string m_strtipoArticulo = null;
//            string m_strNoOrden = null;
//            string m_strLineNum = null;
//            int m_intDocEntry = 0;

//            if (p_LineasCanceladas != null)
//            {
//                var lineas = p_LineasCanceladas;

//                foreach (LineaRequisicion lineaRequisicion in lineas)
//                {
//                    for (int i = 0; i <= p_oCotizacion.Lines.Count - 1; i++)
//                    {
//                        p_oCotizacion.Lines.SetCurrentLine(i);

//                        double cPenBod = Convert.ToDouble(p_oCotizacion.Lines.UserFields.Fields.Item(strCPenBod).Value);
//                        double cRec = Convert.ToDouble(p_oCotizacion.Lines.UserFields.Fields.Item(strCRec).Value);
//                        double cPenDev = Convert.ToDouble(p_oCotizacion.Lines.UserFields.Fields.Item(strCDe).Value);

//                        if (p_oCotizacion.Lines.LineNum == lineaRequisicion.U_SCGD_LNumOr)
//                        {
//                            if (p_oRequisicion.TipoRequisicion.Contains("Trans"))
//                            {
//                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2;
//                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0;
//                            }
//                            else
//                            {
//                                if (m_blnConfOTSAP)
//                                {
//                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1;

//                                    if (cPenBod == 0)
//                                    {
//                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2;
//                                    }
//                                    else if (cPenBod > 0)
//                                    {
//                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4;
//                                    }

//                                    p_oCotizacion.Lines.UserFields.Fields.Item(strCRec).Value = cRec + cPenDev;
//                                    p_oCotizacion.Lines.UserFields.Fields.Item(strCDe).Value = 0;
//                                    p_oCotizacion.Lines.Quantity = cRec + cPenBod + cPenDev;
//                                }
//                            }
//                            break;
//                        }
//                    }
//                }
//            }
//        }

//        public string LocalizationNeeded(LineaRequisicion p_lineaRequisicion, TipoMensaje tipomensaje)
//        {
//            EstadosLineas estado;
//            switch (tipomensaje)
//            {
//                case UI.TipoMensaje.EstadoLinea:
//                    estado = (EstadosLineas)p_lineaRequisicion.U_SCGD_CodEst;
//                    switch (estado)
//                    {
//                        case EstadosLineas.Cancelado:
//                            return Resource.strCancelado;
//                        case EstadosLineas.Pendiente:
//                            return Resource.strPendiente;
//                        case EstadosLineas.Trasladado:
//                            return Resource.strTrasladado;
//                    }
//                    break;
//                case TipoMensaje.EstadoFormulario:
//                    estado = (EstadosLineas)p_lineaRequisicion.U_SCGD_CodEst;
//                    switch (estado)
//                    {
//                        case EstadosLineas.Cancelado:
//                            return Resource.strCancelado;
//                        case EstadosLineas.Pendiente:
//                            return Resource.strPendiente;
//                        case EstadosLineas.Trasladado:
//                            return Resource.strTrasladado;
//                    }
//                    break;
//                    //Revisar
//                //case TipoMensaje.ErrorNoSePuedeTrasladar:
//                //    return string.Format(Resource.NoSePuedeTrasladar, p_lineaRequisicion.DataSourceOffset + 1);
//                //case TipoMensaje.MayorQueCantidadPendiente:
//                //    return string.Format(Resource.CantidadTransferirError, p_lineaRequisicion.DataSourceOffset + 1);
//                //case TipoMensaje.NoSePuedeCancelarLinea:
//                //    return string.Format(Resource.NoCancelarLinea, p_lineaRequisicion.DataSourceOffset + 1);

//            }
//            return string.Empty;
//        }

//        #endregion
//    }
//}
