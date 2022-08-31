using System;
using System.Collections.Generic;
using SAPbobsCOM;
using SCG.Requisiciones.UI;
using System.IO;
using System.Linq;


namespace SCG.Requisiciones
{
    public delegate void ActualizaEncabezadoTrasladoHandler(
        EncabezadoRequisicion encabezadoRequisicion, StockTransfer stockTransfer);

    public delegate void ActualizaLineaTrasladoHandler(
        InformacionLineaRequisicion lineaRequisicion, StockTransfer_Lines stockTransferLines);

    public class RequisicionTraslado : Requisicion
    {

        public event ActualizaEncabezadoTrasladoHandler ActualizaEncabezado;
        public event ActualizaLineaTrasladoHandler ActualizaLineaTraslado;
        public RequisicionTraslado(ICompany company)
            : base(company)
        {
            TipoRequisicion = "Transferencia";
            DocumentoGenera = "Transf. Inv";
            TipoDocumentoMovimiento = "67";
        }

        /// <summary>
        /// Realiza la transferencia de Stock
        /// </summary>
        /// <returns></returns>
        public override List<TransferenciaLineasBase> Traslada(string p_NoSerieCitas = "", string p_NoCita = "")
        {
            int serie = 0;
            Dictionary<string, StockTransferTransferenciaLineas> stockTransfers = new Dictionary<string, StockTransferTransferenciaLineas>();
            StockTransfer stockTransfer;
            string surcursal = "";
            

            foreach (var informacionLineasRequisicion in LineasRequisicion)
            {

                if (!stockTransfers.ContainsKey(informacionLineasRequisicion.CodigoBodegaOrigen))
                {
                    stockTransfer = (StockTransfer)Company.GetBusinessObject(BoObjectTypes.oStockTransfer);
                    stockTransfers.Add(informacionLineasRequisicion.CodigoBodegaOrigen, new StockTransferTransferenciaLineas(stockTransfer));
                    stockTransfer.CardCode = EncabezadoRequisicion.CodigoCliente;
                    stockTransfer.FromWarehouse = informacionLineasRequisicion.CodigoBodegaOrigen;
                    stockTransfer.Comments = EncabezadoRequisicion.Comentarios;
                    stockTransfer.SetUdf(EncabezadoRequisicion.NoOrden, "U_SCGD_Numero_OT");
                    surcursal = informacionLineasRequisicion.LineaIDSucursal;
                    if (!string.IsNullOrEmpty(surcursal))
                    {
                        if (DMS_Connector.Configuracion.ConfiguracionSucursales.Any(confSuc => confSuc.U_Sucurs.Trim() == surcursal))
                        {
                            serie = Convert.ToInt32(DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(confSuc => confSuc.U_Sucurs.Trim() == surcursal).U_SerInv);
                            if (serie != 0)
                            {
                                 stockTransfer.Series = serie;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(EncabezadoRequisicion.Placa))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = EncabezadoRequisicion.Placa.ToString();
                    }
                    if (!string.IsNullOrEmpty(EncabezadoRequisicion.Marca))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = EncabezadoRequisicion.Marca.ToString();
                    }
                    if (!string.IsNullOrEmpty(EncabezadoRequisicion.Estilo))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = EncabezadoRequisicion.Estilo.ToString();
                    }
                    if (!string.IsNullOrEmpty(EncabezadoRequisicion.VIN))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = EncabezadoRequisicion.VIN.ToString();
                    }
                    if (!string.IsNullOrEmpty(p_NoSerieCitas))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = p_NoSerieCitas;
                    }
                    if (!string.IsNullOrEmpty(p_NoCita))
                    {
                        stockTransfer.UserFields.Fields.Item("U_SCGD_NoCita").Value = p_NoCita;
                    }


                    if (ActualizaEncabezado != null)
                        ActualizaEncabezado(EncabezadoRequisicion, stockTransfer);
                }
                stockTransfers[informacionLineasRequisicion.CodigoBodegaOrigen].InformacionLineasRequisicion.Add(informacionLineasRequisicion);
                stockTransfer = stockTransfers[informacionLineasRequisicion.CodigoBodegaOrigen].StockTransfer;
                stockTransfer.Lines.ItemCode = informacionLineasRequisicion.CodigoArticulo;
                stockTransfer.Lines.WarehouseCode = informacionLineasRequisicion.CodigoBodegaDestino;
                stockTransfer.Lines.Quantity = informacionLineasRequisicion.CantidadATransferir;
                stockTransfer.Lines.SetUdf(informacionLineasRequisicion.IDLinea, "U_SCGD_ID");

                //*******************************para Ubicaciones**********************************
                if (Company.Version >= 900000)
                {
                    AgregarUbicaciones(stockTransfer, informacionLineasRequisicion);
                }
                //*********************************************************************************


                if (ActualizaLineaTraslado != null)
                    ActualizaLineaTraslado(informacionLineasRequisicion, stockTransfer.Lines);

                stockTransfer.Lines.Add();
            }
            List<TransferenciaLineasBase> result = new List<TransferenciaLineasBase>();
            foreach (var diccionarioRequisicionesValor in stockTransfers.Values)
            {
                int error = diccionarioRequisicionesValor.StockTransfer.Add();
                if (error == 0)
                {
                    string newObjectKey = Company.GetNewObjectKey();
                    diccionarioRequisicionesValor.StockTransfer.GetByKey(int.Parse(newObjectKey));
                    if (EncabezadoRequisicion.TipoRequisicion.Contains("Res"))
                    {
                        //Genera el movimiento de la bodega reserva hacia la bodega en proceso
                        TransferirReservasBodegaProceso(int.Parse(newObjectKey), EncabezadoRequisicion.NoOrden, EncabezadoRequisicion.DocEntry, LineasRequisicion[0].LineaIDSucursal, p_NoSerieCitas, p_NoCita);
                    }                    
                }
                else
                {
                    diccionarioRequisicionesValor.Error = Company.GetLastErrorDescription();
                }
                diccionarioRequisicionesValor.EncabezadoRequisicion = EncabezadoRequisicion;
                result.Add(diccionarioRequisicionesValor);
            }
            return result;
        }

        public string ObtenerTipoOT(string NumeroOT)
        {
            string Tipo = string.Empty;
            string Query = "SELECT TOP 1 T0.\"U_SCGD_Tipo_OT\" FROM \"OQUT\" T0 WHERE T0.\"U_SCGD_Numero_OT\" = '{0}' Order By T0.\"DocEntry\" DESC ";
            try
            {
                if (!string.IsNullOrEmpty(NumeroOT))
                {
                    Query = string.Format(Query, NumeroOT);
                    Tipo = DMS_Connector.Helpers.EjecutarConsulta(Query);
                }
                
                return Tipo;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }        
        }

        public void TransferirReservasBodegaProceso(int DocEntry, string NumeroOT, int ReqDocEntry, string Sucursal, string NoSerieCita, string NoCita)
        {
            SAPbobsCOM.StockTransfer TransferenciaReserva;
            SAPbobsCOM.StockTransfer TransferenciaProceso;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.CompanyService sCmp;
            SAPbobsCOM.GeneralDataCollection AuditoriaTransferencias;
            SAPbobsCOM.GeneralData LineaAuditoria;
            string NewObjectKey;
            string TipoOT = string.Empty;

            try
            {
                //Solo se genera el movimiento automático de la bodega reserva hacia la
                //bodega proceso cuando la OT ya fue creada, de lo contrario el proceso se 
                //realiza desde la oferta de ventas
                if (!string.IsNullOrEmpty(NumeroOT))
                {
                    if (string.IsNullOrEmpty(TipoOT))
                    {
                        TipoOT = ObtenerTipoOT(NumeroOT);
                    }

                    sCmp = DMS_Connector.Company.CompanySBO.GetCompanyService();
                    oGeneralService = sCmp.GetGeneralService("SCGD_OT");
                    oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", NumeroOT);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    AuditoriaTransferencias = oGeneralData.Child("SCGD_OTTA");
                    TransferenciaReserva = (SAPbobsCOM.StockTransfer)DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oStockTransfer);
                    TransferenciaProceso = (SAPbobsCOM.StockTransfer)DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oStockTransfer);

                    if (TransferenciaReserva.GetByKey(DocEntry))
                    {
                        //Completa los datos del encabezado
                        TransferenciaProceso.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = NumeroOT;
                        TransferenciaProceso.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = NoSerieCita;
                        TransferenciaProceso.UserFields.Fields.Item("U_SCGD_NoCita").Value = NoCita;
                        TransferenciaProceso.CardCode = TransferenciaReserva.CardCode;
                        TransferenciaProceso.Comments = TransferenciaReserva.Comments;

                        //Completa la información de las líneas
                        for (int i = 0; i < TransferenciaReserva.Lines.Count; i++)
                        {
                            TransferenciaReserva.Lines.SetCurrentLine(i);
                            TransferenciaProceso.Lines.ItemCode = TransferenciaReserva.Lines.ItemCode;
                            TransferenciaProceso.Lines.ItemDescription = TransferenciaReserva.Lines.ItemDescription;
                            TransferenciaProceso.Lines.Quantity = TransferenciaReserva.Lines.Quantity;
                            TransferenciaProceso.Lines.FromWarehouseCode = TransferenciaReserva.Lines.WarehouseCode;
                            TransferenciaProceso.Lines.WarehouseCode = ObtenerBodegaProceso(Sucursal, TransferenciaProceso.Lines.ItemCode, TipoOT);
                            TransferenciaProceso.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = TransferenciaReserva.Lines.UserFields.Fields.Item("U_SCGD_ID").Value;
                            TransferenciaProceso.Lines.Add();
                        }

                        TransferenciaProceso.Add();
                        NewObjectKey = DMS_Connector.Company.CompanySBO.GetNewObjectKey();

                        for (int i = 0; i < TransferenciaReserva.Lines.Count; i++)
                        {
                            TransferenciaProceso.Lines.SetCurrentLine(i);
                            LineaAuditoria = AuditoriaTransferencias.Add();
                            LineaAuditoria.SetProperty("U_SCGD_ID", TransferenciaProceso.Lines.UserFields.Fields.Item("U_SCGD_ID").Value);
                            LineaAuditoria.SetProperty("U_BaseEntry", DocEntry.ToString());
                            LineaAuditoria.SetProperty("U_ReqEntry", ReqDocEntry.ToString());
                            LineaAuditoria.SetProperty("U_ItemCode", TransferenciaProceso.Lines.ItemCode);
                            LineaAuditoria.SetProperty("U_Description", TransferenciaProceso.Lines.ItemDescription);
                            LineaAuditoria.SetProperty("U_Quantity", TransferenciaProceso.Lines.Quantity);
                            LineaAuditoria.SetProperty("U_TransEntry", NewObjectKey);
                            LineaAuditoria.SetProperty("U_FromWarehouse", TransferenciaProceso.Lines.FromWarehouseCode);
                            LineaAuditoria.SetProperty("U_Warehouse", TransferenciaProceso.Lines.WarehouseCode);
                            LineaAuditoria.SetProperty("U_Date", DateTime.Now);
                            LineaAuditoria.SetProperty("U_Hour", DateTime.Now);
                            LineaAuditoria.SetProperty("U_User", DMS_Connector.Company.CompanySBO.UserName);
                        }
                    }

                    //Actualiza la tabla con la información de las transferencias en la orden de trabajo
                    //con fines de auditoría y reportes
                    oGeneralService.Update(oGeneralData);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        public string ObtenerBodegaProceso(string Sucursal, string ItemCode, string TipoOT)
        {
            string Bodega = string.Empty;
            string CentroCosto = string.Empty;
            SAPbobsCOM.Items Articulo;
            try
            {
                Articulo = (SAPbobsCOM.Items)DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
    
                if (!string.IsNullOrEmpty(Sucursal))
                { 
                    if (DMS_Connector.Configuracion.ConfiguracionSucursales.Any(confSuc => confSuc.U_Sucurs.Trim() == Sucursal))
                    {
                        if (!string.IsNullOrEmpty(TipoOT) && DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(confSuc => confSuc.U_Sucurs.Trim() == Sucursal).Configuracion_Tipo_Orden.Any(Tipo => Tipo.U_Code == int.Parse(TipoOT)))
                        {
                            CentroCosto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(confSuc => confSuc.U_Sucurs.Trim() == Sucursal).Configuracion_Tipo_Orden.FirstOrDefault(Tipo => Tipo.U_Code == int.Parse(TipoOT)).U_CodCtCos.Trim();
                        }

                        if (string.IsNullOrEmpty(CentroCosto))
                        { 
                            if (Articulo.GetByKey(ItemCode))
                            {
                                CentroCosto = Articulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString().Trim();
                            }
                        }

                        if (!string.IsNullOrEmpty(CentroCosto))
                        {
                            foreach (DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal.Bodegas_CentroCosto ConfiguracionBodega in DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(confSuc => confSuc.U_Sucurs.Trim() == Sucursal).Bodegas_CentroCosto)
                            {
                                if (ConfiguracionBodega.U_CC == CentroCosto)
                                {
                                    Bodega = ConfiguracionBodega.U_Pro;
                                    break;
                                }
                            }
                        }                       
                    }
                }

                return Bodega;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        public override int Crea()
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDORequisiciones.Nombre);
                GeneralData encabezadoGeneralData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                Boolean esDevolucion = false;
                Boolean tieneLineas = false;

                string nombreCliente = null;
                if (string.IsNullOrEmpty(EncabezadoRequisicion.NombreCliente) && !string.IsNullOrEmpty(EncabezadoRequisicion.CodigoCliente))
                {
                    BusinessPartners businessPartners = (BusinessPartners)Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    businessPartners.GetByKey(EncabezadoRequisicion.CodigoCliente);
                    nombreCliente = businessPartners.CardName;
                }
                else
                {
                    nombreCliente = EncabezadoRequisicion.NombreCliente;
                }

                encabezadoGeneralData.SetProperty("U_SCGD_NoOrden", EncabezadoRequisicion.NoOrden);
                encabezadoGeneralData.SetProperty("U_SCGD_CodCliente", EncabezadoRequisicion.CodigoCliente ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_NombCliente", nombreCliente ?? string.Empty);
                TipoRequisicion = EncabezadoRequisicion.TipoRequisicion ?? TipoRequisicion;
                encabezadoGeneralData.SetProperty("U_SCGD_TipoReq", TipoRequisicion ?? string.Empty);
                if (!string.IsNullOrEmpty(TipoRequisicion))
                {
                    encabezadoGeneralData.SetProperty("U_SCGD_CodTipoReq", EncabezadoRequisicion.CodigoTipoRequisicion);
                    if (EncabezadoRequisicion.CodigoTipoRequisicion == 2 || EncabezadoRequisicion.CodigoTipoRequisicion == 4)
                    {
                        esDevolucion = true;
                    }
                    //if (TipoRequisicion.ToLower().Contains("trans"))
                    //{
                    //    encabezadoGeneralData.SetProperty("U_SCGD_CodTipoReq", "1");
                    //}
                    //else if (TipoRequisicion.ToLower().Contains("res"))
                    //{
                    //    encabezadoGeneralData.SetProperty("U_SCGD_CodTipoReq", "3");
                    //}
                    //else
                    //{
                    //    encabezadoGeneralData.SetProperty("U_SCGD_CodTipoReq", "2");
                    //    esDevolucion = true;
                    //}

                }
                encabezadoGeneralData.SetProperty("U_SCGD_TipoDoc", DocumentoGenera ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Usuario", EncabezadoRequisicion.Usuario ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Comm", EncabezadoRequisicion.Comentarios ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Data", EncabezadoRequisicion.Data ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_IDSuc", EncabezadoRequisicion.IDSucursal ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Placa", EncabezadoRequisicion.Placa ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Marca", EncabezadoRequisicion.Marca ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_Estilo", EncabezadoRequisicion.Estilo ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_VIN", EncabezadoRequisicion.VIN ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_TipArt", EncabezadoRequisicion.TipoArticulo ?? string.Empty);
                encabezadoGeneralData.SetProperty("U_SCGD_CodEst", (int)EstadosLineas.Pendiente);
                encabezadoGeneralData.SetProperty("U_SCGD_Est", Resource.strPendiente);

                GeneralDataCollection lineas = encabezadoGeneralData.Child(UDORequisiciones.TablaLineas.TrimStart('@'));
                GeneralData linea;
                foreach (var lineasRequisicion in LineasRequisicion)
                {
                    linea = lineas.Add();
                    // lineaRequisicion.CodigoArticulo ?? string.Empty ---> si CodigoArticulo es null asigna string.Empty 
                    linea.SetProperty("U_SCGD_CodArticulo", lineasRequisicion.CodigoArticulo ?? string.Empty);
                    linea.SetProperty("U_SCGD_DescArticulo", lineasRequisicion.DescripcionArticulo ?? string.Empty);
                    linea.SetProperty("U_SCGD_CodBodOrigen", lineasRequisicion.CodigoBodegaOrigen ?? string.Empty);
                    linea.SetProperty("U_SCGD_CodBodDest", lineasRequisicion.CodigoBodegaDestino ?? string.Empty);
                    linea.SetProperty("U_SCGD_CodTipoArt", lineasRequisicion.CodigoTipoArticulo);
                    linea.SetProperty("U_SCGD_TipoArticulo", lineasRequisicion.DescripcionTipoArticulo ?? string.Empty);
                    linea.SetProperty("U_SCGD_CantSol", lineasRequisicion.CantidadSolicitada);
                    linea.SetProperty("U_SCGD_CantRec", 0);
                    linea.SetProperty("U_SCGD_CCosto", lineasRequisicion.CentroCosto);
                    linea.SetProperty("U_SCGD_CodEst", (int)EstadosLineas.Pendiente);
                    linea.SetProperty("U_SCGD_Estado", lineasRequisicion.Estado ?? string.Empty);
                    linea.SetProperty("U_SCGD_LNumOr", lineasRequisicion.LineNumOrigen);
                    linea.SetProperty("U_SCGD_DocOr", lineasRequisicion.DocumentoOrigen);
                    linea.SetProperty("U_SCGD_COrig", lineasRequisicion.CantidadSolicitada);

                    if (Company.Version >= 900000)
                    {
                        linea.SetProperty("U_AUbic", lineasRequisicion.AUbicacion ?? string.Empty);
                        linea.SetProperty("U_DeUbic", lineasRequisicion.DeUbicacion ?? string.Empty);
                    }

                    linea.SetProperty("U_SCGD_Lidsuc", lineasRequisicion.LineaIDSucursal ?? string.Empty);
                    linea.SetProperty("U_SCGD_ID", lineasRequisicion.IDLinea ?? string.Empty);
                    linea.SetProperty("U_ReqOriPen", lineasRequisicion.LineaReqOrPen);
                    if (lineasRequisicion.CantidadSolicitada > 0)
                    {
                        tieneLineas = true;
                    }
                }
                if (esDevolucion)
                {
                    if (!tieneLineas)
                    {
                        encabezadoGeneralData.SetProperty("U_SCGD_CodEst", (int)EstadosLineas.Trasladado);
                        encabezadoGeneralData.SetProperty("U_SCGD_Est", Resource.strTrasladado);
                    }
                }
                lineas = encabezadoGeneralData.Child(UDORequisiciones.TablaMovimientos.TrimStart('@'));
                linea = lineas.Add();
                linea.SetProperty("U_SCGD_CodArticulo", "-1");

                var generalDataParams = generalService.Add(encabezadoGeneralData);
                if (generalDataParams != null)
                {
                    EncabezadoRequisicion.DocEntry = int.Parse(generalDataParams.GetProperty("DocEntry").ToString());
                    EncabezadoRequisicion.DocNum = EncabezadoRequisicion.DocEntry.ToString();
                    return EncabezadoRequisicion.DocEntry;
                }
            }
            return 0;
        }

        public void AgregarUbicaciones(StockTransfer p_stockTransfer, InformacionLineaRequisicion p_InformacionLineaRequisicion)
        {
            if (!string.IsNullOrEmpty(p_InformacionLineaRequisicion.DeUbicacion))
            {
                p_stockTransfer.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                p_stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt16(p_InformacionLineaRequisicion.DeUbicacion);
                p_stockTransfer.Lines.BinAllocations.Quantity = p_InformacionLineaRequisicion.CantidadATransferir;
                p_stockTransfer.Lines.BinAllocations.Add();
            }

            if (!string.IsNullOrEmpty(p_InformacionLineaRequisicion.AUbicacion))
            {
                p_stockTransfer.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                p_stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt16(p_InformacionLineaRequisicion.AUbicacion);
                p_stockTransfer.Lines.BinAllocations.Quantity = p_InformacionLineaRequisicion.CantidadATransferir;
                p_stockTransfer.Lines.BinAllocations.Add();
            }
        }
    }
}
