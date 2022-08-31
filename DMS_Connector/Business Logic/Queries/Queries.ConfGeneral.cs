namespace DMS_Connector
{
    public partial class Queries
    {
        private const string strCountCNAp = "SELECT COUNT(\"U_CNAp\") FROM \"@SCGD_MSJS1\" ¿#? WHERE \"U_CNAp\" = '{0}'";

        private const string strCboEtapaCV = "SELECT \"Num\", \"Descript\" from \"OOST\" ¿#? ";

        private const string strCboBodAcc = "SELECT \"WhsCode\", \"WhsName\" FROM \"OWHS\" ¿#? ";

        private const string strCboPeriodo = "SELECT \"GroupNum\", \"PymntGroup\" from \"OCTG\" ¿#? where \"OpenRcpt\" = 'N' Order By \"PymntGroup\" ";

        private const string SQL_strNoLock = " WITH(NOLOCK)";
        private const string HANA_strNoLock = "";

        
        #region "Cargar combos"
        private const string SQL_strNumeracionesFacturas = " Select Series, ((Case ObjectCode when '13' then '{0}' when '14' then '{1}' when '18' then '{2}' when '19' then '{1}' end) + ' - ' + SeriesName + (Case DocSubType when '--' then '' else ' - ' + DocSubType  end) ) Nombre from NNM1 with (nolock) where ObjectCode in ('13','14','18','19') order by Nombre ";
        private const string HANA_strNumeracionesFacturas = " SELECT \"Series\", ((CASE \"ObjectCode\" WHEN '13' THEN '{0}' WHEN '14' THEN '{1}' WHEN '18' THEN '{2}' END) || ' - ' || \"SeriesName\" || (CASE \"DocSubType\" WHEN '--' THEN '' ELSE ' - ' || \"DocSubType\" END)) AS \"Nombre\" FROM NNM1 WHERE \"ObjectCode\" IN ('13','14','18') ORDER BY \"Nombre\" ";

        private const string strOOST = "Select \"Num\", \"Descript\" from \"OOST\" ¿#? ";

        private const string strDisponibilidad = " Select \"Code\",\"Name\" From \"@SCGD_DISPONIBILIDAD\" ¿#? Order by \"Name\" ";

        private const string strTipoVehiculo = "Select \"Code\", \"Name\" From \"@SCGD_TIPOVEHICULO\" ¿#? Order by \"Name\" ";

        private const string strTipoVehiculoWhere = "Select \"Code\", \"Name\" From \"@SCGD_TIPOVEHICULO\" ¿#? Where \"Code\" != {0} Order by \"Name\" ";

        private const string strOTRC = " Select \"TrnsCode\", \"TrnsCodDsc\" from OTRC ¿#?  ";

        private const string strTranComp = "Select \"Code\", \"Name\" From \"@SCGD_TRAN_COMP\" ¿#? where \"Canceled\" = 'N' Order by \"Name\" ";

        private const string strGruoposSN = " SELECT \"GroupCode\", \"GroupName\" FROM \"OCRG\" ¿#? WHERE \"GroupType\" = 'C' AND \"Locked\" = 'N' ";

        private const string strExpense = " Select \"ExpnsCode\", \"ExpnsName\" from \"OEXD\" ¿#? where \"RevAcct\" is not null ";

        #endregion
    }
}
