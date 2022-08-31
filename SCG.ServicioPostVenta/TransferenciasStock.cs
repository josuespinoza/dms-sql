using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSOneFramework.SCGBL.Requisiciones;
using DMSOneFramework.SCGCommon;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.Requisiciones;
using SCG.Requisiciones.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public class TransferenciasStock
    {
        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }
        public OrdenTrabajo g_objOT;

        public enum TipoMovimiento
        {
            TransferenciaRepuestos = 0,
            TransferenciaSuministros = 1,
            TransferenciaServiciosExternos = 2,
            TransferenciaItemsEmininar = 3
        }

        public enum EstadosTraslado
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4
        }

        public enum EstadosAprobacion
        {
            Aprobado = 1,
            NoAprobado = 2,
            FaltoAprobacion = 3
        }

        public enum ResultadoValidacionPorItem
        {
            SinCambio = 0,
            NoAprobar = 1,
            ModifCantiCotizacion = 2,
            PendTransf = 3,
            Comprar = 4,
            PendBodega = 5
        }

        public struct LineasTransferenciasStock
        {
            public string strItemCode;
            public string strItemDescription;
            public double dblCantidad;
            public string strBodegaOrigen;
            public string strBodegaDestino;
            public int intIdColaborador;
            public string strNombreMecanico;
            public int intTipoArticulo;
            public int intLineNum;
            public double dblCosto;
            public int intReqOriPen;
            public string strIdLinea;
            public string strIdSucursal;
        }

        public TransferenciasStock(Application applicationSBO, ICompany companySBO)
        {
            CompanySBO = companySBO;
            ApplicationSBO = applicationSBO;
        }

        public void GeneraListasTransferencias(TipoMovimiento p_strTipoMovimiento, ref List<LineasTransferenciasStock> p_lstItemsTransferencia, ref SAPbobsCOM.Documents p_oCotizacion,
            string p_strBodegaRepuestos, string p_strBodegaSuministros, string p_strBodegaServiciosExternos, string p_strBodegaProceso,
            bool p_blnEvaluarAdicionales, int p_intTipoArticulo, int p_intEstadoPaquete, int p_intCantidadLineasPaquete, int p_intItemGenerico,
            bool p_blnActualizarCantidad, bool p_draft, double p_dblCantidadAdicional, int p_intDocEntry)
        {
            int m_intTipoItemAceptado = 0;
            LineasTransferenciasStock m_udtLineaTransferencia = new LineasTransferenciasStock();
            double m_dblStockDisponible;
            double m_dblCantXOtrasLineas;
            double m_dblCantidadValida;
            string m_strBodegaActual = string.Empty;
            int m_intColaborador;

            string m_strValorTrasladado = string.Empty;
            int m_intValorTrasladado;
            string m_strValorAprobado = string.Empty;
            int m_intValorAprobado;

            try
            {
                g_objOT = new OrdenTrabajo();

                switch (p_strTipoMovimiento)
                {
                    case TipoMovimiento.TransferenciaRepuestos:
                        m_intTipoItemAceptado = 1;
                        m_strBodegaActual = p_strBodegaRepuestos;

                        break;
                    case TipoMovimiento.TransferenciaSuministros:
                        m_intTipoItemAceptado = 3;
                        m_strBodegaActual = p_strBodegaSuministros;

                        break;
                    case TipoMovimiento.TransferenciaServiciosExternos:
                        m_intTipoItemAceptado = 4;
                        m_strBodegaActual = p_strBodegaServiciosExternos;

                        break;
                }

                m_strValorAprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString().Trim();
                int.TryParse(m_strValorAprobado, out m_intValorAprobado);
                m_strValorTrasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim();
                int.TryParse(m_strValorTrasladado, out m_intValorTrasladado);
                m_udtLineaTransferencia.strIdLinea = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim();
                m_udtLineaTransferencia.strIdSucursal = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString().Trim();

                if (string.IsNullOrEmpty(m_strBodegaActual) == false)
                {
                    if (m_strBodegaActual != p_strBodegaProceso)
                    {
                        if (p_intTipoArticulo != 5)
                        {
                            if (p_strTipoMovimiento != TipoMovimiento.TransferenciaItemsEmininar)
                            {
                                if ((((m_intValorAprobado == (int)EstadosAprobacion.Aprobado && p_intCantidadLineasPaquete <= 0) ||
                                    (p_intEstadoPaquete == (int)ResultadoValidacionPorItem.NoAprobar && p_intCantidadLineasPaquete > 0)) &&
                                    (m_intValorTrasladado == (int)EstadosTraslado.NoProcesado ||
                                    m_intValorTrasladado == (int)EstadosTraslado.PendienteTraslado ||
                                    m_intValorTrasladado == (int)EstadosTraslado.PendienteBodega)) ||
                                    p_blnActualizarCantidad)
                                {
                                    if (p_intTipoArticulo == m_intTipoItemAceptado)
                                    {
                                        if (p_intItemGenerico == 1)
                                        {
                                            m_dblStockDisponible = g_objOT.DevuelveStockDisponibleXItem(p_oCotizacion.Lines.ItemCode, m_strBodegaActual, CompanySBO);
                                            m_dblCantXOtrasLineas = g_objOT.DevuelveCantidadLineasAnteriores(p_oCotizacion.Lines.ItemCode, p_oCotizacion.Lines.LineNum, p_intDocEntry, CompanySBO);

                                            if ((m_dblStockDisponible - m_dblCantXOtrasLineas) > 0)
                                            {
                                                if (p_blnActualizarCantidad == false)
                                                {
                                                    if ((m_dblStockDisponible - m_dblCantXOtrasLineas) < p_oCotizacion.Lines.Quantity)
                                                    {
                                                        m_dblCantidadValida = m_dblStockDisponible - m_dblCantXOtrasLineas;
                                                    }
                                                    else
                                                    {
                                                        m_dblCantidadValida = p_oCotizacion.Lines.Quantity;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((m_dblStockDisponible - m_dblCantXOtrasLineas) < p_dblCantidadAdicional)
                                                    {
                                                        m_dblCantidadValida = m_dblStockDisponible - m_dblCantXOtrasLineas;
                                                    }
                                                    else
                                                    {
                                                        m_dblCantidadValida = p_dblCantidadAdicional;
                                                    }
                                                }
                                                if (m_dblCantidadValida >= p_oCotizacion.Lines.Quantity)
                                                {
                                                    m_udtLineaTransferencia.strItemCode = p_oCotizacion.Lines.ItemCode;
                                                    m_udtLineaTransferencia.strItemDescription = p_oCotizacion.Lines.ItemDescription;
                                                    m_udtLineaTransferencia.dblCantidad = m_dblCantidadValida;
                                                    m_udtLineaTransferencia.strBodegaDestino = p_strBodegaProceso;
                                                    m_udtLineaTransferencia.strBodegaOrigen = m_strBodegaActual;
                                                    m_udtLineaTransferencia.intTipoArticulo = p_intTipoArticulo;
                                                    if (Utilitarios.IsNumeric(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim()))
                                                    {
                                                        int.TryParse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim(), out m_intColaborador);
                                                        m_udtLineaTransferencia.intIdColaborador = m_intColaborador;
                                                    }
                                                    else
                                                    {
                                                        m_udtLineaTransferencia.intIdColaborador = 0;
                                                    }
                                                    m_udtLineaTransferencia.strNombreMecanico = (string)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value;
                                                    m_udtLineaTransferencia.intLineNum = p_oCotizacion.Lines.LineNum;
                                                    if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString()) != 4)
                                                    {
                                                        m_udtLineaTransferencia.intReqOriPen = 2;
                                                    }
                                                    else
                                                    {
                                                        m_udtLineaTransferencia.intReqOriPen = 1;
                                                    }

                                                    p_lstItemsTransferencia.Add(m_udtLineaTransferencia);
                                                }
                                                if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim() !=
                                                    EstadosTraslado.PendienteTraslado.ToString().Trim() &&
                                                    (p_intTipoArticulo == 1 || p_intTipoArticulo == 4) && m_dblCantidadValida >= p_oCotizacion.Lines.Quantity)
                                                {
                                                    if (Utilitarios.IsNumeric(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim()))
                                                    {
                                                        int.TryParse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim(), out m_intColaborador);
                                                        m_udtLineaTransferencia.intIdColaborador = m_intColaborador;
                                                    }
                                                    else
                                                    {
                                                        m_udtLineaTransferencia.intIdColaborador = 0;
                                                    }
                                                    m_udtLineaTransferencia.strNombreMecanico = (string)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value;
                                                }
                                                if (p_blnEvaluarAdicionales)
                                                {
                                                    if (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString().Trim() !=
                                                    EstadosTraslado.NoProcesado.ToString().Trim() &&
                                                    (p_intTipoArticulo == 1 || p_intTipoArticulo == 4))
                                                    {
                                                        if (Utilitarios.IsNumeric(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim()))
                                                        {
                                                            int.TryParse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim(), out m_intColaborador);
                                                            m_udtLineaTransferencia.intIdColaborador = m_intColaborador;
                                                        }
                                                        else
                                                        {
                                                            m_udtLineaTransferencia.intIdColaborador = 0;
                                                        }
                                                        m_udtLineaTransferencia.strNombreMecanico = (string)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value;
                                                    }
                                                }
                                                //''''''''''''''''se agrega para Documentos Draft''''''''''''''''
                                                if (m_dblCantidadValida >= p_oCotizacion.Lines.Quantity)
                                                {
                                                    if (p_draft)
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4;
                                                    }
                                                    else
                                                    {
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2;
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0;
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0;
                                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = p_oCotizacion.Lines.Quantity;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if ((int)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value != (int)EstadosTraslado.PendienteTraslado)
                                                {
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.No;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.No;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (((m_intValorAprobado == (int)EstadosAprobacion.NoAprobado && p_intCantidadLineasPaquete <= 0) ||
                                    (p_intEstadoPaquete == (int)ResultadoValidacionPorItem.ModifCantiCotizacion && p_intCantidadLineasPaquete > 0)) &&
                                    m_intValorTrasladado == (int)EstadosTraslado.Si)
                                {
                                    if (p_intTipoArticulo == 1 || p_intTipoArticulo == 3)
                                    {
                                        m_udtLineaTransferencia.strItemCode = p_oCotizacion.Lines.ItemCode;
                                        m_udtLineaTransferencia.strItemDescription = p_oCotizacion.Lines.ItemDescription;
                                        m_udtLineaTransferencia.dblCantidad = p_oCotizacion.Lines.Quantity;
                                        switch (p_intTipoArticulo)
                                        {
                                            case 1:
                                                m_udtLineaTransferencia.strBodegaDestino = p_strBodegaRepuestos;
                                                break;
                                            case 3:
                                                m_udtLineaTransferencia.strBodegaDestino = p_strBodegaSuministros;
                                                break;
                                            case 4:
                                                m_udtLineaTransferencia.strBodegaDestino = p_strBodegaServiciosExternos;
                                                break;
                                        }
                                        m_udtLineaTransferencia.strBodegaOrigen = p_strBodegaProceso;
                                        m_udtLineaTransferencia.intTipoArticulo = p_intTipoArticulo;
                                        if (Utilitarios.IsNumeric(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim()))
                                        {
                                            int.TryParse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim(), out m_intColaborador);
                                            m_udtLineaTransferencia.intIdColaborador = m_intColaborador;
                                        }
                                        else
                                        {
                                            m_udtLineaTransferencia.intIdColaborador = 0;
                                        }
                                        m_udtLineaTransferencia.strNombreMecanico = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString().Trim();
                                        if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString()) != 4)
                                        {
                                            m_udtLineaTransferencia.intReqOriPen = 2;
                                        }
                                        else
                                        {
                                            m_udtLineaTransferencia.intReqOriPen = 1;
                                        }
                                        p_lstItemsTransferencia.Add(m_udtLineaTransferencia);

                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.NoProcesado;
                                    }
                                }
                            }
                            if (p_intCantidadLineasPaquete > 0)
                            {
                                p_intCantidadLineasPaquete -= 1;
                            }
                        }
                        else
                        {
                            p_intCantidadLineasPaquete = -1;
                            p_intEstadoPaquete = (int)p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value;
                        }
                    }
                }
                else
                {
                    if (p_strTipoMovimiento == TipoMovimiento.TransferenciaItemsEmininar)
                    {
                        if ((((m_intValorAprobado == (int)EstadosAprobacion.NoAprobado && p_intCantidadLineasPaquete <= 0) ||
                            (p_intEstadoPaquete == (int)ResultadoValidacionPorItem.NoAprobar && p_intCantidadLineasPaquete > 0)) &&
                            m_intValorTrasladado == (int)EstadosTraslado.Si ||
                            m_intValorTrasladado == (int)EstadosTraslado.PendienteBodega) || p_blnActualizarCantidad)
                        {
                            if (p_intTipoArticulo == 1 || p_intTipoArticulo == 3)
                            {
                                m_udtLineaTransferencia.strItemCode = p_oCotizacion.Lines.ItemCode;
                                m_udtLineaTransferencia.strItemDescription = p_oCotizacion.Lines.ItemDescription;
                                if (p_blnActualizarCantidad == false)
                                {
                                    m_udtLineaTransferencia.dblCantidad = p_oCotizacion.Lines.Quantity;
                                }
                                else
                                {
                                    m_udtLineaTransferencia.dblCantidad = p_dblCantidadAdicional;
                                }
                                switch (p_intTipoArticulo)
                                {
                                    case 1:
                                        m_udtLineaTransferencia.strBodegaDestino = p_strBodegaRepuestos;
                                        break;
                                    case 3:
                                        m_udtLineaTransferencia.strBodegaDestino = p_strBodegaSuministros;
                                        break;
                                    case 4:
                                        m_udtLineaTransferencia.strBodegaDestino = p_strBodegaServiciosExternos;
                                        break;
                                }

                                m_udtLineaTransferencia.strBodegaOrigen = p_strBodegaProceso;
                                m_udtLineaTransferencia.intTipoArticulo = p_intTipoArticulo;

                                if (Utilitarios.IsNumeric(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim()))
                                {
                                    int.TryParse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString().Trim(), out m_intColaborador);
                                    m_udtLineaTransferencia.intIdColaborador = m_intColaborador;
                                }
                                else
                                {
                                    m_udtLineaTransferencia.intIdColaborador = 0;
                                }
                                m_udtLineaTransferencia.strNombreMecanico = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString().Trim();
                                if (int.Parse(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString()) != 4)
                                {
                                    m_udtLineaTransferencia.intReqOriPen = 2;
                                }
                                else
                                {
                                    m_udtLineaTransferencia.intReqOriPen = 1;
                                }
                                p_lstItemsTransferencia.Add(m_udtLineaTransferencia);

                                if (p_blnActualizarCantidad == false)
                                {
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = EstadosTraslado.NoProcesado;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public string CrearTrasladoAddOnNuevo(
            ref List<LineasTransferenciasStock> p_slRepuestos,
            ref List<LineasTransferenciasStock> p_slSuministros,
            ref List<LineasTransferenciasStock> p_slServiciosExternos,
            ref List<LineasTransferenciasStock> p_slEliminarRepuestos,
            ref List<LineasTransferenciasStock> p_slEliminarSuministros,
            int p_intDocEntryCotizacion,
            string p_strNoOrden,
            string p_strBodegaRepuesto,
            string p_strBodegaSuministros,
            string p_strBodegaServiciosExtenos,
            string p_strBodegaProceso,
            string p_strIDSerieTransferencia,
            bool p_blnEvaluarAdicionales,
            ref string p_strIDsTrasladosRep,
            ref string p_strIDsTrasladosSumi,
            string p_strMarca,
            string p_strEstilo,
            string p_strModelo,
            string p_strPlaca,
            string p_strVIN,
            string p_strAsesor,
            string p_strCliente,
            bool p_blnAjusteOTEspecial,
            bool p_blnDraft, string p_strIdSucursal)
        {

            string m_strDocEntry = string.Empty;
            string m_strCollectionDocEntry = string.Empty;

            try
            {
                if (p_slRepuestos.Count > 0)
                {
                    p_strIDsTrasladosRep = CrearTransferenciaItems(ref p_slRepuestos, p_intDocEntryCotizacion, p_strNoOrden, p_strIDSerieTransferencia, p_strMarca,
                        p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, false, p_strCliente, p_blnDraft,
                        p_blnAjusteOTEspecial, Resource.Repuesto, p_strIdSucursal, "1"); //RECURSOS
                }

                if (p_slSuministros.Count > 0)
                {
                    p_strIDsTrasladosSumi = CrearTransferenciaItems(ref p_slSuministros, p_intDocEntryCotizacion, p_strNoOrden, p_strIDSerieTransferencia, p_strMarca,
                        p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, false, p_strCliente, p_blnDraft,
                        p_blnAjusteOTEspecial, Resource.Suministro, p_strIdSucursal, "3"); //RECURSOS
                }

                if (p_slServiciosExternos.Count > 0)
                {
                    CrearTransferenciaItems(ref p_slServiciosExternos, p_intDocEntryCotizacion, p_strNoOrden, p_strIDSerieTransferencia, p_strMarca,
                        p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, false, p_strCliente, p_blnDraft,
                        p_blnAjusteOTEspecial, "", p_strIdSucursal, ""); //RECURSOS
                }

                m_strDocEntry = string.Empty;
                if (p_slEliminarRepuestos.Count > 0)
                {
                    m_strDocEntry =
                        p_strIDsTrasladosRep = CrearTransferenciaItems(ref p_slEliminarRepuestos, p_intDocEntryCotizacion, p_strNoOrden, p_strIDSerieTransferencia, p_strMarca,
                        p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, true, p_strCliente, p_blnDraft,
                        p_blnAjusteOTEspecial, Resource.Repuesto, p_strIdSucursal, "1"); //RECURSOS
                }

                if (string.IsNullOrEmpty(m_strDocEntry) == false)
                {
                    m_strCollectionDocEntry = string.Format(" {0} , {1} ", m_strCollectionDocEntry, m_strDocEntry);
                }

                m_strDocEntry = string.Empty;
                if (p_slEliminarSuministros.Count > 0)
                {
                    m_strDocEntry =
                        p_strIDsTrasladosSumi = CrearTransferenciaItems(
                        ref p_slEliminarSuministros, p_intDocEntryCotizacion, p_strNoOrden, p_strIDSerieTransferencia, p_strMarca,
                        p_strEstilo, p_strModelo, p_strPlaca, p_strVIN, p_strAsesor, true, p_strCliente, p_blnDraft,
                        p_blnAjusteOTEspecial, Resource.Suministro, p_strIdSucursal, "3"); //RECURSOS
                }

                if (string.IsNullOrEmpty(m_strDocEntry) == false)
                {
                    m_strCollectionDocEntry = string.Format(" {0} , {1} ", m_strCollectionDocEntry, m_strDocEntry);
                }

                return m_strCollectionDocEntry;
            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                //return null;
            }
        }

        public string CrearTransferenciaItems(ref List<LineasTransferenciasStock> p_lsLineasTransferencia, int p_intDocEntryCotizacion,
            string p_strNoOrden, string p_strNoSerie, string p_strMarca, string p_strEstilo, string p_strModelo, string p_strPlaca,
            string p_strVIN, string p_strAsesor, bool p_blnElimnar, string p_strCliente, bool p_blnDraft, bool p_blnAjusteOTEspecial,
            string p_strTipo, string p_strIdSucursal, string p_intTipoArt)
        {
            SAPbobsCOM.StockTransfer m_objStockTransfer;

            List<List<LineasTransferenciasStock>> m_objListasPorBodegaOrigen;
            List<LineasTransferenciasStock> m_objListaUnica;

            List<RequisicionTraslado> m_objListaRetornada;
            string m_strDraft = string.Empty;
            string m_strTotalDocEntrys = string.Empty;

            int m_intResultado;
            string m_strNewDocEntry;

            try
            {
                if (p_blnDraft)
                {
                    m_objListasPorBodegaOrigen = ClasificaListaXBodegaOrigen(p_lsLineasTransferencia);

                    m_objListaUnica = new List<LineasTransferenciasStock>();
                    for (int x = 0; x <= m_objListasPorBodegaOrigen.Count - 1; x++)
                    {
                        m_objListaUnica = m_objListasPorBodegaOrigen[x];
                        m_objListaRetornada = CreaRequisición(ref m_objListaUnica, p_intDocEntryCotizacion, p_strNoOrden, p_strNoSerie, p_strAsesor, p_blnElimnar, p_strCliente, string.Empty, p_strTipo, p_blnAjusteOTEspecial, p_strIdSucursal, p_intTipoArt);

                        if (m_objListasPorBodegaOrigen.Count >= 1)
                        {
                            for (int i = 0; i <= m_objListasPorBodegaOrigen.Count - 1; i++)
                            {
                                if (i == 0)
                                {
                                    m_strDraft = string.Format("{0}", m_objListaRetornada[i].EncabezadoRequisicion.DocEntry.ToString().Trim());
                                }
                                else
                                {
                                    m_strDraft = string.Format("{0} , {1}", m_strDraft, m_objListaRetornada[i].EncabezadoRequisicion.DocEntry.ToString().Trim());
                                }
                            }
                        }
                    }
                    return m_strDraft;
                }
                else
                {
                    m_objListasPorBodegaOrigen = ClasificaListaXBodegaOrigen(p_lsLineasTransferencia);
                    m_objListaUnica = new List<LineasTransferenciasStock>();
                    for (int x = 0; x <= m_objListasPorBodegaOrigen.Count - 1; x++)
                    {
                        m_objListaUnica = m_objListasPorBodegaOrigen[x];

                        m_objStockTransfer = (StockTransfer)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                        m_objStockTransfer.CardCode = p_strCliente;
                        m_objStockTransfer.FromWarehouse = m_objListaUnica[0].strBodegaOrigen;
                        //m_objStockTransfer.Series = p_strNoSerie;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = p_strNoOrden;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = p_strMarca;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = p_strEstilo;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = p_strModelo;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = p_strPlaca;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = p_strVIN;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_TipoTransf").Value = 1;
                        m_objStockTransfer.UserFields.Fields.Item("U_SCGD_idSucursal").Value = p_strIdSucursal;
                        if (p_blnAjusteOTEspecial)
                        {
                            m_objStockTransfer.Comments = Resource.MensajeAjusteOTEspecial;
                        }
                        m_objStockTransfer.Comments += String.Format("{0} {1} {2} {3}", Resource.OT_Referencia, p_strNoOrden, Resource.Asesor, p_strAsesor);

                        if (p_blnElimnar)
                        {
                            m_objStockTransfer.UserFields.Fields.Item("U_SCGD_TipoTransf").Value = 2;
                            m_objStockTransfer.Comments += String.Format(" * * {0} * * ", Resource.Devolucion);
                        }

                        CargarLineasTraslado(ref m_objStockTransfer, ref m_objListaUnica);

                        m_intResultado = m_objStockTransfer.Add();

                        if (m_intResultado != 0)
                        {
                            throw new ExceptionsSBO(m_intResultado, CompanySBO.GetLastErrorDescription());
                        }
                        else
                        {
                            m_strNewDocEntry = CompanySBO.GetNewObjectKey();
                        }

                        if (m_strNewDocEntry != "0")
                        {
                            if (string.IsNullOrEmpty(m_strTotalDocEntrys) == true)
                            {
                                m_strTotalDocEntrys = string.Format("{0}", m_strNewDocEntry);
                            }
                            else
                            {
                                m_strTotalDocEntrys = string.Format("{0} , {1} ", m_strTotalDocEntrys, m_strNewDocEntry);
                            }
                        }
                    }
                    return m_strTotalDocEntrys;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                //return null;
            }
        }

        public List<RequisicionTraslado> CreaRequisición(
            ref List<LineasTransferenciasStock> p_lsLineasTransferenciaStock,
            int p_intDocEntryCotizacion,
            string p_strNoOrden,
            string p_strNoSerie,
            string p_strAsesor,
            bool p_blnEliminar,
            string p_strCodigoCliente,
            string p_strNombreCliente,
            string p_strTipo,
            bool p_blnAjusteOTEspecial, string p_strIdSucursal, string p_intTipoArt)
        {
            List<List<LineasTransferenciasStock>> m_lsListasPorBodegas;
            RequisicionTraslado objRequisicion = new RequisicionTraslado(CompanySBO);

            List<RequisicionTraslado> objRetornoCreaRequisición = new List<RequisicionTraslado>();

            try
            {
                m_lsListasPorBodegas = ClasificaListaXBodegaOrigen(p_lsLineasTransferenciaStock);

                //for (int x = 0; x <= m_lsListaPorBodegas.Count; x++)
                foreach (List<LineasTransferenciasStock> m_objListaUnicaXBodega in m_lsListasPorBodegas)
                {
                    EncabezadoRequisicion m_objEncabezado = new EncabezadoRequisicion();
                    EncabezadoTrasladoDMSData m_objData = new EncabezadoTrasladoDMSData();
                    List<InformacionLineaRequisicion> m_listaLineas;
                    RequisicionTraslado m_objRequisicion = new RequisicionTraslado(CompanySBO);

                    m_objRequisicion.TipoRequisicion = Resource.RequisicionTraslado;
                    m_objRequisicion.DocumentoGenera = Resource.DocGeneraReq;

                    m_objEncabezado.CodigoCliente = p_strCodigoCliente;
                    m_objEncabezado.NombreCliente = p_strNombreCliente;
                    m_objEncabezado.NoOrden = p_strNoOrden;
                    m_objEncabezado.CodigoTipoRequisicion = 1;
                    m_objEncabezado.Comentarios = String.Format("{0} {1} {2} {3}", Resource.OT_Referencia, p_strNoOrden, Resource.Asesor, p_strAsesor);
                    m_objEncabezado.Usuario = ApplicationSBO.Company.UserName;
                    m_objEncabezado.IDSucursal = p_strIdSucursal;
                    m_objEncabezado.TipoArticulo = p_intTipoArt; 

                    m_objData.TipoTransferencia = 1;
                    m_objData.Serie = p_strNoSerie;
                    m_objData.NumCotizacionOrigen = p_intDocEntryCotizacion;

                    if (p_blnEliminar)
                    {
                        m_objEncabezado.TipoRequisicion = Resource.Devolucion;
                        m_objData.TipoTransferencia = 2;
                        m_objEncabezado.Comentarios = Resource.Devolucion;
                    }
                    if (p_blnAjusteOTEspecial)
                    {
                        m_objEncabezado.Comentarios = Resource.MensajeAjusteOTEspecial;
                    }

                    m_listaLineas = new List<InformacionLineaRequisicion>();
                    foreach (LineasTransferenciasStock m_objLinea in m_objListaUnicaXBodega)
                    {
                        InformacionLineaRequisicion objInformacionLinea = new InformacionLineaRequisicion();
                        objInformacionLinea.CodigoArticulo = m_objLinea.strItemCode;
                        objInformacionLinea.DescripcionArticulo = m_objLinea.strItemDescription;
                        objInformacionLinea.CodigoBodegaOrigen = m_objLinea.strBodegaOrigen;
                        objInformacionLinea.CodigoBodegaDestino = m_objLinea.strBodegaDestino;
                        objInformacionLinea.CantidadSolicitada = m_objLinea.dblCantidad;
                        objInformacionLinea.CantidadOriginal = m_objLinea.dblCantidad;
                        objInformacionLinea.LineNumOrigen = m_objLinea.intLineNum;
                        objInformacionLinea.LineaIDSucursal = m_objLinea.strIdSucursal;
                        objInformacionLinea.IDLinea = m_objLinea.strIdLinea;
                        objInformacionLinea.DocumentoOrigen = p_intDocEntryCotizacion;
                        objInformacionLinea.DescripcionTipoArticulo = p_strTipo;
                        objInformacionLinea.CentroCosto = (int)m_objLinea.dblCosto;
                        objInformacionLinea.CodigoTipoArticulo = m_objLinea.intTipoArticulo;
                        objInformacionLinea.LineaReqOrPen = m_objLinea.intReqOriPen;
                        m_listaLineas.Add(objInformacionLinea);
                    }

                    //m_objEncabezado.Data = m_objData.Serialize();
                    objRequisicion.EncabezadoRequisicion = m_objEncabezado;
                    objRequisicion.LineasRequisicion = m_listaLineas;

                    if (objRequisicion.Crea() != 0)
                    {
                        objRetornoCreaRequisición.Add(objRequisicion);
                    }
                }
                return objRetornoCreaRequisición;
            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                //return null;
            }
        }

        private List<List<LineasTransferenciasStock>> ClasificaListaXBodegaOrigen(
            List<LineasTransferenciasStock> p_lsLineasTransferencia)
        {
            List<List<LineasTransferenciasStock>> m_objListasRetorno = new List<List<LineasTransferenciasStock>>();
            List<LineasTransferenciasStock> m_objListaNueva;
            bool m_blnExiste = false;

            try
            {
                foreach (LineasTransferenciasStock objLineaTransferencia in p_lsLineasTransferencia)
                {
                    foreach (List<LineasTransferenciasStock> objListaEspecifica in m_objListasRetorno)
                    {
                        foreach (LineasTransferenciasStock objLineaRetorno in objListaEspecifica)
                        {
                            if (objLineaTransferencia.strBodegaOrigen == objLineaRetorno.strBodegaOrigen)
                            {
                                objListaEspecifica.Add(objLineaTransferencia);
                                m_blnExiste = true;
                                break;
                            }
                        }
                        if (m_blnExiste == false)
                        {
                            break;
                        }
                    }

                    if (m_blnExiste == false)
                    {
                        m_objListaNueva = new List<LineasTransferenciasStock>();
                        m_objListaNueva.Add(objLineaTransferencia);

                        m_objListasRetorno.Add(m_objListaNueva);
                    }
                }
                return m_objListasRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
                //return null;
            }
        }

        private void CargarLineasTraslado(ref SAPbobsCOM.StockTransfer p_objTransferenciaStock, ref List<LineasTransferenciasStock> p_lsLineasTransferenciaStock)
        {
            LineasTransferenciasStock udtLineasTransferenciaStockActual;
            try
            {
                if (p_lsLineasTransferenciaStock.Count != 0)
                {
                    for (int i = 0; i <= p_lsLineasTransferenciaStock.Count - 1; i++)
                    {
                        udtLineasTransferenciaStockActual = p_lsLineasTransferenciaStock[i];
                        p_objTransferenciaStock.Lines.ItemCode = udtLineasTransferenciaStockActual.strItemCode;
                        if (string.IsNullOrEmpty(udtLineasTransferenciaStockActual.strItemDescription) == false)
                        {
                            p_objTransferenciaStock.Lines.ItemDescription = udtLineasTransferenciaStockActual.strItemDescription;
                        }
                        p_objTransferenciaStock.Lines.Quantity = udtLineasTransferenciaStockActual.dblCantidad;
                        p_objTransferenciaStock.Lines.WarehouseCode = udtLineasTransferenciaStockActual.strBodegaDestino;
                        if (string.IsNullOrEmpty(udtLineasTransferenciaStockActual.strNombreMecanico) == false)
                        {
                            p_objTransferenciaStock.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = udtLineasTransferenciaStockActual.strNombreMecanico;
                        }
                        if (udtLineasTransferenciaStockActual.intIdColaborador != 0)
                        {
                            p_objTransferenciaStock.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value = udtLineasTransferenciaStockActual.intIdColaborador;
                        }
                        p_objTransferenciaStock.Lines.Add();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

    }
}
