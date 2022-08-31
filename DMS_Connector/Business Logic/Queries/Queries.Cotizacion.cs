
namespace DMS_Connector
{
    public partial class Queries
    {
        #region "Visita"
        private const string strNumeroOTSiguiente = "SELECT COUNT(\"DocEntry\") + 1 FROM \"OQUT\" ¿#? WHERE \"U_SCGD_No_Visita\" = '{0}' AND (\"U_SCGD_Numero_OT\" IS NOT NULL OR \"U_SCGD_Numero_OT\" <> '')";
        #endregion
        #region "Oferta de ventas padre"
        private const string strDocEntryOfertaPadre = "SELECT \"DocEntry\"  FROM \"OQUT\" ¿#? WHERE \"U_SCGD_Numero_OT\" = '{0}'";
        #endregion
        #region "Consulta DocEntry documento marketing"
        private const string strDocEntryMarketing = "SELECT DISTINCT \"DocEntry\" FROM \"'{0}'\" ¿#? WHERE \"U_SCGD_NoOT\" = '{1}'";
        #endregion
        #region "Consulta DocEntry destino"
        private const string strDocEntryMarketingDestinoID = "SELECT \"TargetType\", \"TrgetEntry\"  FROM \"'{0}'\" ¿#? WHERE \"DocEntry\" = {1} AND \"U_SCGD_ID\" = '{2}'";
        private const string strDocEntryMarketingDestinoIdRepXOrd = "SELECT \"TargetType\", \"TrgetEntry\"  FROM \"'{0}'\" ¿#? WHERE \"DocEntry\" = {1} AND \"U_SCGD_IdRepxOrd\" = '{2}'";
        #endregion
        #region "DocEntry Cotizacion"
        private const string strDocEntryCotizacion = "SELECT distinct \"DocEntry\"  FROM \"OQUT\" ¿#? WHERE \"U_SCGD_Numero_OT\" IN ({0})";
        #endregion

        #region "Asignación Multiple"
        private const string strGetContCotLin = " SELECT COUNT(q.\"DocEntry\") FROM QUT1 q ¿#? LEFT OUTER JOIN OITM i ¿#? ON q.\"ItemCode\" = i.\"ItemCode\" WHERE q.\"DocEntry\" = '{0}' AND i.\"U_SCGD_TipoArticulo\" = 2 ";
        private const string strGetConItm = " SELECT COUNT(\"ItemCode\") FROM OITM q ¿#? WHERE q.\"U_SCGD_TipoArticulo\" = 2 AND q.\"ItemCode\" IN ({0}) ";
        private const string strGetFaseItm = " SELECT q.\"U_SCGD_T_Fase\" AS \"NoFase\", fp.\"Name\" AS \"Descripcion\" FROM OITM q ¿#? LEFT OUTER JOIN \"@SCGD_FASEPRODUCCION\" fp ¿#? ON q.\"U_SCGD_T_Fase\" = fp.\"Code\" WHERE q.\"U_SCGD_TipoArticulo\" = 2 AND q.\"ItemCode\" = ('{0}') ";

        #endregion
        #region "Consulta DocEntry Cotizacion por NoOrden"
        private const string strDocEntryCotizacionxNoOrden = "SELECT \"DocEntry\"  FROM \"OQUT\" ¿#? WHERE \"U_SCGD_Numero_OT\" = '{0}'";

        #region "Consulta Bloqueo OT"
        private const string strBloqueoOTCotizacion = "SELECT \"U_SCGD_BloOT\"  FROM \"OQUT\" ¿#? WHERE \"DocEntry\" = '{0}'";
        #endregion
        #endregion

        #region "Factura Interna"
        private const string strCostoManoObraCotizacion = "SELECT SUM(QUT1.U_SCGD_Costo) FROM OQUT with (nolock) INNER JOIN QUT1 with (nolock) on OQUT.DocEntry = QUT1.DocEntry WHERE OQUT.U_SCGD_Numero_OT = '{0}' AND QUT1.U_SCGD_TipArt = 2 AND QUT1.U_SCGD_Aprobado = 1";
        #endregion

        #region "Re Apertura OT"
        private const string strGenOFV = "SELECT DISTINCT T0.\"DocEntry\" AS \"{0}\",  T0.\"U_SCGD_Numero_OT\" AS \"{1}\", T0.\"CardCode\" AS \"{2}\", T0.\"CardName\" AS \"{3}\",T0.\"U_SCGD_Num_Placa\" AS \"{4}\", T0.\"U_SCGD_Des_Marc\" AS \"{5}\", T0.\"U_SCGD_Des_Mode\" AS \"{6}\" FROM OQUT T0 INNER JOIN QUT1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\"";
        #endregion

    }
}
