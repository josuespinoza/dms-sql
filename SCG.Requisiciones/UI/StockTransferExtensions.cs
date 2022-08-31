using SAPbobsCOM;

namespace SCG.Requisiciones.UI
{
    public static class StockTransferExtensions
    {
        public static void SetUdf(this StockTransfer stockTransfer, object valor, string udf)
        {
            if (valor != null)
                stockTransfer.UserFields.Fields.Item(udf).Value = valor ;
        }
        public static void SetUdf(this StockTransfer_Lines stockTransferLines, object valor, string udf)
        {
            stockTransferLines.UserFields.Fields.Item(udf).Value = valor;
        }
    }
}