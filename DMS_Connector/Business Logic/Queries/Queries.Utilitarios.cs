
namespace DMS_Connector
{
    public partial class Queries
    {
        #region "Utiliarios"
        
        private const string strGetIdSucursal = "SELECT \"Branch\" FROM \"OUSR\" ¿#? WHERE \"USER_CODE\" = '{0}'";

        private const string strGetIdEmpleado = "SELECT \"userId\" FROM \"OUSR\" ¿#? WHERE \"USER_CODE\" = '{0}'";

        private const string strDevuelveTransacciones = "SELECT \"Code\" FROM \"@SCGD_TRAN_COMP\" ¿#? WHERE \"U_View\" = '{0}'";

        private const string strNombreBDSucursales = "SELECT \"Code\", \"U_BDSucursal\" FROM \"@SCGD_SUCURSALES\" ¿#?";

        private const string strUserIdBySlpCode = "SELECT \"USER_CODE\" FROM \"OUSR\" ¿#? WHERE \"userId\" = (SELECT \"userID\" FROM OHEM ¿#? WHERE \"salesPrson\" = '{0}')";

        private const string SQL_strGetEmpId = "SELECT ISNULL(\"empid\",0) FROM \"OHEM\" WITH(NOLOCK) WHERE \"userId\" = {0} ";
        private const string HANA_strGetEmpId = "SELECT IFNULL(\"empid\",0) FROM \"OHEM\" WHERE \"userId\" = {0} ";

        private const string strGetSlpCode = "SELECT \"salesPrson\" FROM \"OHEM\" ¿#? WHERE \"userId\" = {0} ";

        private const string strGetSlpName = "SELECT \"SlpName\" FROM \"OSLP\" ¿#? WHERE \"SlpCode\" = {0}";

        private const string strGetVisOrder = "SELECT \"VisOrder\" FROM \"{0}\" ¿#? WHERE \"DocEntry\" = {3} AND \"LineNum\" = {1} AND \"ItemCode\" = '{2}'";

        private const string strVerificaCampanaPorUnidad = "SELECT SAPcnp.\"CpnNo\", SAPcnp.\"Name\" FROM \"@SCGD_CAMPANA\" cnp ¿#? RIGHT OUTER JOIN \"@SCGD_VEHIXCAMP\" vcnp ¿#? ON cnp.\"DocEntry\" = vcnp.\"DocEntry\" INNER JOIN OCPN SAPcnp ¿#? ON SAPcnp.\"CpnNo\" = cnp.\"U_CampSap\" WHERE (vcnp.\"U_Unidad\" = '{0}' OR vcnp.\"U_Vin\" = '{1}') AND SAPcnp.\"Status\" = 'O' AND vcnp.\"U_Estado\" = '1' GROUP BY SAPcnp.\"CpnNo\", SAPcnp.\"Name\"";
        
        #endregion

        #region "Gestor Menú"

        private const string strPermisosUsuario = "SELECT DISTINCT \"Code\" FROM \"@SCGD_PERMISOS_PV\" ¿#? WHERE \"U_Usuario\" = '{0}'";

        private const string SQL_strNombreMenu = "SELECT DISTINCT N.\"Code\", ISNULL(L.\"U_Menu\", N.\"Name\") AS \"U_Menu\" FROM \"@SCGD_NIVELES_PV\" N WITH(NOLOCK) LEFT OUTER JOIN \"@SCGD_NPV_LENG\" L WITH(NOLOCK) ON N.\"Code\" = L.\"Code\" WHERE (\"U_Idioma\" = {0} OR ({0} NOT IN (SELECT \"U_Idioma\" FROM \"@SCGD_NPV_LENG\" WITH(NOLOCK) WHERE \"Code\" = N.\"Code\") AND \"U_Idioma\" = 2))";
        private const string HANA_strNombreMenu = "SELECT DISTINCT N.\"Code\", IFNULL(L.\"U_Menu\", N.\"Name\") AS \"U_Menu\" FROM \"@SCGD_NIVELES_PV\" N LEFT OUTER JOIN \"@SCGD_NPV_LENG\" L ON N.\"Code\" = L.\"Code\" WHERE (\"U_Idioma\" = {0} OR ({0} NOT IN (SELECT \"U_Idioma\" FROM \"@SCGD_NPV_LENG\" WHERE \"Code\" = N.\"Code\") AND \"U_Idioma\" = 2))";

        #endregion
    }
}
