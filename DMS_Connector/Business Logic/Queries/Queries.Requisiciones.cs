namespace DMS_Connector
{
    public partial class Queries
    {
        #region ...FormularioRequisiciones...
        private const string strConsultaFRExisteUbi = " SELECT COUNT(ubi.\"AbsEntry\") AS \"cantidad\" FROM OBIN ubi ¿#? LEFT OUTER JOIN OIBQ qt ¿#? ON ubi.\"WhsCode\" = qt.\"WhsCode\" AND ubi.\"AbsEntry\" = qt.\"BinAbs\" WHERE ubi.\"WhsCode\" = '{0}' AND qt.\"ItemCode\" = '{1}' AND ubi.\"AbsEntry\" LIKE '{2}%' ";
        private const string strConsultaFRCotDocCur = " SELECT \"DocStatus\" FROM OQUT ¿#? WHERE \"U_SCGD_Numero_OT\" = '{0}' ";
        private const string strFRCotDocEntry = " SELECT Q.\"DocEntry\" FROM OQUT Q ¿#? WHERE Q.\"U_SCGD_Numero_OT\" = '{0}' ";
        #endregion


        #region ...ListaUbicaciones...
        private const string strLUCargaMatrizSinFiltros = " SELECT ubi.\"AbsEntry\" AS \"UbiCode\", ubi.\"BinCode\" AS \"Ubicacion\", qt.\"OnHandQty\" FROM OBIN ubi ¿#? LEFT OUTER JOIN OIBQ qt ¿#? ON ubi.\"WhsCode\" = qt.\"WhsCode\" AND ubi.\"AbsEntry\" = qt.\"BinAbs\" WHERE ubi.\"WhsCode\" = '{0}' AND qt.\"ItemCode\" = '{1}' ";
        private const string strLUCargaMatrizConFiltros = " SELECT ubi.\"AbsEntry\" AS \"UbiCode\", ubi.\"BinCode\" AS \"Ubicacion\", qt.\"OnHandQty\" FROM OBIN ubi ¿#? LEFT OUTER JOIN OIBQ qt ¿#? ON ubi.\"WhsCode\" = qt.\"WhsCode\" AND ubi.\"AbsEntry\" = qt.\"BinAbs\" WHERE ubi.\"WhsCode\" = '{0}' AND qt.\"ItemCode\" = '{1}' AND ubi.\"AbsEntry\" LIKE '{2}%' ";
        #endregion

        #region ...ListadoRequisiciones...
        private const string strLREcargadosBodega = " SELECT DISTINCT ln.\"U_Usr_UsrName\" AS \"Code\", ln.\"U_Usr_Name\" AS \"Name\" FROM \"@SCGD_CONF_MSJ\" enc ¿#? INNER JOIN \"@SCGD_CONF_MSJLN\" ln ¿#? ON enc.\"DocEntry\" = ln.\"DocEntry\" WHERE (enc.\"U_IdRol\" = 1 OR enc.\"U_IdRol\" = 6) ";
        private const string strLRRolesMensajeria = " SELECT DISTINCT mln.\"U_IDRol\" AS \"Rol\" FROM \"@SCGD_CONF_MSJLN\" mln ¿#? INNER JOIN \"@SCGD_CONF_MSJ\" m ¿#? ON mln.\"DocEntry\" = m.\"DocEntry\" WHERE mln.\"U_Usr_UsrName\" = '{0}' AND (mln.\"U_IDRol\" = 1 OR mln.\"U_IDRol\" = 6) AND m.\"U_IdSuc\" = '{1}' ";
        private const string strLRBDSucursal = " SELECT \"U_BDSucursal\" FROM \"@SCGD_SUCURSALES\" ¿#? WHERE \"Code\" = '{0}' ";
        #endregion

        #region ...Ubicaciones...
        private const string strDescripcionUbicacion = "SELECT \"BinCode\" FROM \"OBIN\" ¿#? WHERE \"AbsEntry\" = {0}";
        #endregion

        #region ...SeriesArticulo...
        private const string OfertaContieneSeries = " SELECT COUNT(*) AS \"Cuenta\" FROM \"QUT1\" T0 ¿#? INNER JOIN \"OITM\" T1 ¿#? ON T0.\"ItemCode\" = T1.\"ItemCode\" WHERE T0.\"DocEntry\" = '{0}' AND T1.\"ManSerNum\" = 'Y' ";
        #endregion

    }
}