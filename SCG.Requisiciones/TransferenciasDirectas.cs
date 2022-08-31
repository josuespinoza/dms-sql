using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.Requisiciones
{
    public static class TransferenciasDirectas
    {
        private enum TiposArticulo
        {
            SinAsignar = 0,
            Repuesto = 1,
            Servicio = 2,
            Suministro = 3,
            ServicioExterno = 4,
            Paquete = 5,
            Otros = 6,
            Accesorio = 7,
            Vehiculo = 8,
            Tramite = 9,
            ArticuloCita = 10,
            OtrosCostos = 11,
            OtrosIngresos = 12
        }

        private enum EstadoRequisicion
        {
            Pendiente = 1,
            Trasladada = 2,
            Cancelada = 3
        }

        private enum EstadoLineasRequisicion
        {
            Pendiente = 1,
            Trasladada = 2,
            Cancelada = 3
        }


        private enum EstadoTraslado
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4
        }

        private enum EstadoAprobado
        {
            Si = 1,
            No = 2,
            FaltaAprobacion = 3,
            CambioOrdenTrabajo = 4
        }

        private static bool UsaTransferenciasDirectas(string CodigoSucursal)
        {
            bool resultado = false;
            try
            {
                if (!string.IsNullOrEmpty(CodigoSucursal) && DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == CodigoSucursal).U_DirectTransfer == "Y")
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return resultado;
            }
        }

        public static bool PermiteTransferenciasDirectas(ref SAPbobsCOM.Documents OfertaVentas)
        {
            bool resultado = false;
            string Query = string.Empty;
            int Cuenta = 0;
            string CodigoSucursal = string.Empty;
            SAPbobsCOM.Recordset oRecordset;
            try
            {
                CodigoSucursal = OfertaVentas.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString();
                Query = string.Format(DMS_Connector.Queries.GetStrSpecificQuery("OfertaContieneSeries"), OfertaVentas.DocEntry);
                if (!string.IsNullOrEmpty(CodigoSucursal) && UsaTransferenciasDirectas(CodigoSucursal))
                {
                    oRecordset = (SAPbobsCOM.Recordset)DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery(Query);
                    Cuenta = (int)oRecordset.Fields.Item("Cuenta").Value;
                    if (Cuenta == 0)
                    {
                        resultado = true;
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return resultado;
            }
        }

        public static void CrearTransferencia(ref SAPbobsCOM.GeneralData oRequisicion, ref int ErrorCode, ref string ErrorMessage)
        {
            SAPbobsCOM.StockTransfer Transferencia;
            SAPbobsCOM.GeneralDataCollection oLineasRequisicion;
            SAPbobsCOM.GeneralDataCollection oLineasMovimientos;
            string CodigoSucursal = string.Empty;
            try
            {
                CodigoSucursal = oRequisicion.GetProperty("U_SCGD_IDSuc").ToString();
                if (UsaTransferenciasDirectas(CodigoSucursal))
                {
                    oLineasRequisicion = oRequisicion.Child("SCGD_LINEAS_REQ");
                    oLineasMovimientos = oRequisicion.Child("SCGD_MOVS_REQ");

                    Transferencia = (SAPbobsCOM.StockTransfer)DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                    CompletarEncabezadosTransferencia(ref Transferencia, ref oRequisicion, ref ErrorCode, ref ErrorMessage);
                    CompletarLineasTransferencia(ref Transferencia, ref oLineasRequisicion, ref ErrorCode, ref ErrorMessage);

                    ErrorCode = Transferencia.Add();
                    if (ErrorCode != 0)
                    {
                        ErrorMessage = DMS_Connector.Company.CompanySBO.GetLastErrorDescription();
                    }
                    else
                    {
                        int NewObjectKey = int.Parse(DMS_Connector.Company.CompanySBO.GetNewObjectKey());
                        if (Transferencia.GetByKey(NewObjectKey))
                        {
                            CompletarLineasAuditoriaMovimientos(ref Transferencia, ref oLineasMovimientos, ref ErrorCode, ref ErrorMessage);
                            CerrarRequisicion(ref oRequisicion, ref ErrorCode, ref ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorCode = 96001;
                ErrorMessage = ex.Message;
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        private static void CompletarLineasAuditoriaMovimientos(ref SAPbobsCOM.StockTransfer Transferencia, ref SAPbobsCOM.GeneralDataCollection LineasAuditoriaMovimientos, ref int ErrorCode, ref string ErrorMessage)
        {
            SAPbobsCOM.GeneralData LineaMovimiento;
            try
            {
                //Actualizar la requisición con los nuevos valores
                for (int i = 0; i < Transferencia.Lines.Count; i++)
                {
                    Transferencia.Lines.SetCurrentLine(i);
                    LineaMovimiento = LineasAuditoriaMovimientos.Add();
                    LineaMovimiento.SetProperty("U_SCGD_DocEntry", Transferencia.DocEntry);
                    LineaMovimiento.SetProperty("U_SCGD_DocNum", Transferencia.DocNum);
                    LineaMovimiento.SetProperty("U_SCGD_CodArticulo", Transferencia.Lines.ItemCode);
                    LineaMovimiento.SetProperty("U_SCGD_DescArticulo", Transferencia.Lines.ItemDescription);
                    LineaMovimiento.SetProperty("U_SCGD_CantTransf", Transferencia.Lines.Quantity);
                    LineaMovimiento.SetProperty("U_SCGD_TipoDoc", SAPbobsCOM.BoObjectTypes.oStockTransfer.ToString());
                    LineaMovimiento.SetProperty("U_SCGD_FechaDoc", DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        private static void CompletarLineasTransferencia(ref SAPbobsCOM.StockTransfer Transferencia, ref SAPbobsCOM.GeneralDataCollection LineasRequisicion, ref int ErrorCode, ref string ErrorMessage)
        {
            SAPbobsCOM.GeneralData LineaRequisicion;
            int FromWarehouse;
            int ToWarehouse;
            double Cantidad = 0;
            try
            {
                //Procesar las líneas de la requisición
                for (int i = 0; i < LineasRequisicion.Count; i++)
                {
                    LineaRequisicion = LineasRequisicion.Item(i);
                    Cantidad = (double)LineaRequisicion.GetProperty("U_SCGD_CantSol");
                    //Lineas requisicion
                    LineaRequisicion.SetProperty("U_SCGD_CantPen", 0);
                    LineaRequisicion.SetProperty("U_SCGD_CAju", 0);
                    LineaRequisicion.SetProperty("U_SCGD_CantATransf", 0);
                    LineaRequisicion.SetProperty("U_SCGD_CantRec", Cantidad);
                    LineaRequisicion.SetProperty("U_SCGD_CodEst", (int)EstadoLineasRequisicion.Trasladada);
                    LineaRequisicion.SetProperty("U_SCGD_Estado", Resource.strTrasladado);

                    //Lineas transferencia
                    Transferencia.Lines.ItemCode = LineaRequisicion.GetProperty("U_SCGD_CodArticulo").ToString();
                    Transferencia.Lines.Quantity = Cantidad;
                    Transferencia.Lines.FromWarehouseCode = LineaRequisicion.GetProperty("U_SCGD_CodBodOrigen").ToString();
                    Transferencia.Lines.WarehouseCode = LineaRequisicion.GetProperty("U_SCGD_CodBodDest").ToString();
                    Transferencia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = LineaRequisicion.GetProperty("U_SCGD_ID").ToString();

                    if (DMS_Connector.Company.CompanySBO.Version >= 900000)
                    {
                        //Ubicaciones
                        if (int.TryParse(LineaRequisicion.GetProperty("U_DeUbic").ToString(), out FromWarehouse))
                        {
                            Transferencia.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                            Transferencia.Lines.BinAllocations.BinAbsEntry = FromWarehouse;
                            Transferencia.Lines.BinAllocations.Quantity = Transferencia.Lines.Quantity;
                            Transferencia.Lines.BinAllocations.Add();
                        }

                        if (int.TryParse(LineaRequisicion.GetProperty("U_AUbic").ToString(), out ToWarehouse))
                        {
                            Transferencia.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                            Transferencia.Lines.BinAllocations.BinAbsEntry = ToWarehouse;
                            Transferencia.Lines.BinAllocations.Quantity = Transferencia.Lines.Quantity;
                            Transferencia.Lines.BinAllocations.Add();
                        }
                    }

                    Transferencia.Lines.Add();
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        private static void CompletarEncabezadosTransferencia(ref SAPbobsCOM.StockTransfer Transferencia, ref SAPbobsCOM.GeneralData oRequisicion, ref int ErrorCode, ref string ErrorMessage)
        {
            try
            {
                //Completa los datos de la transferencia
                Transferencia.CardCode = oRequisicion.GetProperty("U_SCGD_CodCliente").ToString();
                Transferencia.Comments = oRequisicion.GetProperty("U_SCGD_Comm").ToString();
                Transferencia.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = oRequisicion.GetProperty("U_SCGD_NoOrden").ToString();
                Transferencia.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = oRequisicion.GetProperty("U_SCGD_Placa").ToString();
                Transferencia.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = oRequisicion.GetProperty("U_SCGD_Marca").ToString();
                Transferencia.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = oRequisicion.GetProperty("U_SCGD_Estilo").ToString();
                Transferencia.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = oRequisicion.GetProperty("U_SCGD_VIN").ToString();

                //Actualiza los estados de la requsición
                oRequisicion.SetProperty("U_SCGD_CodEst", (int)EstadoRequisicion.Trasladada);
                oRequisicion.SetProperty("U_SCGD_Est", Resource.strTrasladado);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }

        public static void AjustarPendientesRequisicion(ref SAPbobsCOM.Documents OfertaVentas, bool EsCancelacion, ref int ErrorCode, ref string ErrorMessage)
        {
            EstadoAprobado Aprobado = EstadoAprobado.No;
            TiposArticulo TipoArticulo = TiposArticulo.SinAsignar;
            EstadoTraslado Trasladado = EstadoTraslado.NoProcesado;
            string TextoAprobado;
            string TextoTipoArticulo;
            string TextoTrasladado;
            string CodigoSucursal = string.Empty;
            double Solicitado = 0;
            double Pendiente = 0;
            double PendienteDevolucion = 0;
            double PendienteTraslado = 0;
            double PendienteBodega = 0;
            double Recibido = 0;
            double Cantidad = 0;

            try
            {
                CodigoSucursal = OfertaVentas.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString();
                if (UsaTransferenciasDirectas(CodigoSucursal))
                {
                    for (int i = 0; i < OfertaVentas.Lines.Count; i++)
                    {
                        OfertaVentas.Lines.SetCurrentLine(i);
                        TextoAprobado = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value.ToString();
                        TextoTrasladado = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString();
                        TextoTipoArticulo = OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString();

                        if (RequiereProcesamiento(TextoAprobado, TextoTrasladado, TextoTipoArticulo, ref Aprobado, ref Trasladado, ref TipoArticulo))
                        {
                            Solicitado = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;
                            Pendiente = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value;
                            PendienteDevolucion = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;
                            PendienteTraslado = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value;
                            PendienteBodega = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value;
                            Recibido = (double)OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value;

                            Cantidad = OfertaVentas.Lines.Quantity;

                            CalcularValoresOfertaVentas(EsCancelacion, ref Aprobado, ref Trasladado, ref TipoArticulo, ref Cantidad, ref Solicitado, ref Pendiente, ref PendienteDevolucion, ref PendienteTraslado, ref PendienteBodega, ref Recibido);

                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ((int)Aprobado).ToString();
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = ((int)Trasladado).ToString();
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = Solicitado;
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = Pendiente;
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = PendienteDevolucion;
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = PendienteTraslado;
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = PendienteBodega;
                            OfertaVentas.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = Recibido;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorCode = 98456;
                ErrorMessage = ex.Message;
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        private static void CalcularValoresOfertaVentas(bool EsCancelacion, ref EstadoAprobado Aprobado, ref EstadoTraslado Trasladado, ref TiposArticulo TipoArticulo, ref double Cantidad, ref double Solicitado, ref double Pendiente, ref double PendienteDevolucion, ref double PendienteTraslado, ref double PendienteBodega, ref double Recibido)
        {
            try
            {
                if ((TipoArticulo == TiposArticulo.Repuesto || TipoArticulo == TiposArticulo.Suministro))
                {
                    if (EsCancelacion)
                    {
                        Aprobado = EstadoAprobado.No;
                        Trasladado = EstadoTraslado.NoProcesado;
                        Solicitado = 0;
                        Pendiente = 0;
                        PendienteDevolucion = 0;
                        PendienteTraslado = 0;
                        PendienteBodega = 0;
                        Recibido = 0;
                    }
                    else
                    {
                        if (Trasladado == EstadoTraslado.PendienteBodega)
                        {
                            switch (Aprobado)
                            {
                                case EstadoAprobado.Si:
                                    Trasladado = EstadoTraslado.Si;
                                    Solicitado = 0;
                                    Pendiente = 0;
                                    PendienteDevolucion = 0;
                                    PendienteTraslado = 0;
                                    PendienteBodega = 0;
                                    Recibido = Cantidad;
                                    break;
                                case EstadoAprobado.No:
                                    Trasladado = EstadoTraslado.NoProcesado;
                                    Solicitado = 0;
                                    Pendiente = 0;
                                    PendienteDevolucion = 0;
                                    PendienteTraslado = 0;
                                    PendienteBodega = 0;
                                    Recibido = 0;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                throw ex;
            }
        }


        private static bool RequiereProcesamiento(string TextoAprobado, string TextoTrasladado, string TextoTipoArticulo, ref EstadoAprobado Aprobado, ref EstadoTraslado Trasladado, ref TiposArticulo TipoArticulo)
        {
            bool Resultado = true;
            try
            {
                if (!string.IsNullOrEmpty(TextoAprobado) && !string.IsNullOrEmpty(TextoTrasladado) && !string.IsNullOrEmpty(TextoTipoArticulo))
                {
                    Aprobado = (EstadoAprobado)Enum.Parse(typeof(EstadoAprobado), TextoAprobado);

                    Trasladado = (EstadoTraslado)Enum.Parse(typeof(EstadoTraslado), TextoTrasladado);

                    TipoArticulo = (TiposArticulo)Enum.Parse(typeof(TiposArticulo), TextoTipoArticulo);                    
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return false;
            }
        }

        private static void CerrarRequisicion(ref SAPbobsCOM.GeneralData oRequisicion, ref int ErrorCode, ref string ErrorMessage)
        {
            SAPbobsCOM.CompanyService oCompanyService;
            SAPbobsCOM.GeneralService oGeneralService;
            try
            {
                oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");
                oGeneralService.Update(oRequisicion);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                ErrorCode = 96002;
                ErrorMessage = ex.Message;
            }
        }
    }
}
