using System;
using System.Collections.Generic;
using SAPbobsCOM;

namespace SCG.Requisiciones.UI
{
    public class StockTransferTransferenciaLineas : TransferenciaLineasBase
    {
        public StockTransfer StockTransfer { get; set; }

        public StockTransferTransferenciaLineas(StockTransfer stockTransfer)
        {
            InformacionLineasRequisicion = new List<InformacionLineaRequisicion>();
            Error = string.Empty;
            StockTransfer = stockTransfer;
        }

        public override void CopyToInformacionLineasMovimientos(InformacionLineasMovimientos lineasMovimientos)
        {
            lineasMovimientos.CodigoDocumento =
                StockTransfer.DocEntry;
            lineasMovimientos.NumeroDocumento = StockTransfer.DocNum;
            lineasMovimientos.Fecha = StockTransfer.DocDate;
            lineasMovimientos.TipoDocumento = "67";
        }
    }
}