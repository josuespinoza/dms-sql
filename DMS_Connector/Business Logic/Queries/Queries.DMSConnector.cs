namespace DMS_Connector
{
    public partial class Queries
    {
        #region "Configuraciones Iniciales"

        private const string strTipoOT = "SELECT \"Code\", \"Name\", \"U_Interna\", \"U_UsaDim\", \"U_UsaDimAEM\", \"U_UsaDimAFP\" FROM \"@SCGD_TIPO_ORDEN\" ¿#? Order by \"Code\" ";
        
        private const string strTrasladado = "SELECT \"FldValue\", \"Descr\" FROM UFD1 WHERE \"TableID\" = 'QUT1' AND \"FieldID\" IN (SELECT \"FieldID\" FROM CUFD WHERE \"AliasID\" = 'SCGD_Traslad' AND \"TableID\" = 'QUT1')";
        
        private const string strAprobado = "SELECT \"FldValue\", \"Descr\" FROM UFD1 WHERE \"TableID\" = 'QUT1' AND \"FieldID\" IN (SELECT \"FieldID\" FROM CUFD WHERE \"AliasID\" = 'SCGD_Aprobado' AND \"TableID\" = 'QUT1')";
        
        private const string strDocEntrySucursales = "SELECT \"DocEntry\" FROM \"@SCGD_CONF_SUCURSAL\" ¿#? Order by \"DocEntry\" ";

        private const string strDocEntryConfMensajeria = "SELECT \"DocEntry\" FROM \"@SCGD_CONF_MSJ\" ¿#? Order by \"DocEntry\" ";

        private const string strDocEntryConfNumeracion = "SELECT \"DocEntry\" FROM \"@SCGD_NUMERACION\" ¿#? Order by \"DocEntry\" ";

        private const string strDocEntryConfDimensiones = "SELECT \"DocEntry\" FROM \"@SCGD_DIMEN\" ¿#? Order by \"DocEntry\" ";

        private const string strDocEntryConfDimensionesOT = "SELECT \"DocEntry\" FROM \"@SCGD_DIMENSION_OT\" ¿#? Order by \"DocEntry\" ";

        #endregion
    }
}
