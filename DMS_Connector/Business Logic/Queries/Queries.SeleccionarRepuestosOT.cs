namespace DMS_Connector
{
    public partial class Queries
    {
        private const string strConsultaRepuestos = " SELECT TOP(100) OI.\"ItemCode\", CFNB.\"U_Rep\" BOD, OW.\"OnHand\" STK, OI.\"ItemName\", IT.\"Price\", IT.\"Currency\", OI.\"CodeBars\" FROM OITM OI INNER JOIN  ITM1 IT  ON OI.\"ItemCode\" = IT.\"ItemCode\" INNER JOIN \"@SCGD_CONF_BODXCC\" CFNB ON OI.\"U_SCGD_CodCtroCosto\" = CFNB.\"U_CC\" INNER JOIN OITW OW ON OW.\"WhsCode\" = CFNB.\"U_Rep\" AND OW.\"ItemCode\" = OI.\"ItemCode\" WHERE CFNB.\"DocEntry\" = {0} AND OI.\"U_SCGD_TipoArticulo\" = '1' AND IT.\"PriceList\" = '{1}' ";
    }
}
