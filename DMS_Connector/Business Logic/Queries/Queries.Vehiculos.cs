namespace DMS_Connector
{
    public partial class Queries
    {
        #region "Datos Vehiculo"
        private const string SQL_strGetDatosVehiculos = "SELECT U_Cod_Unid Unidad, U_Des_Marc Marca, U_Des_Mode Modelo, U_Des_Esti Estilo, U_Ano_Vehi Año, U_Num_Plac Placa, COL.Name Color, U_Num_VIN VIN, TIPO.Name Tipo FROM [@SCGD_VEHICULO] VEHI WITH (nolock) LEFT OUTER JOIN [@SCGD_TIPOVEHICULO] AS TIPO WITH (nolock) ON TIPO.Code = VEHI.U_Tipo left outer join dbo.[@SCGD_COLOR] COL WITH (nolock) on COL.Code = VEHI.U_Cod_Col Where VEHI.Code = '{0}'";
        private const string HANA_strGetDatosVehiculos = "SELECT \"U_Cod_Unid\" AS \"Unidad\", \"U_Des_Marc\" AS \"Marca\", \"U_Des_Mode\" AS \"Modelo\", \"U_Des_Esti\" AS \"Estilo\", \"U_Ano_Vehi\" AS \"Año\", \"U_Num_Plac\" AS \"Placa\", COL.\"Name\" AS \"Color\", \"U_Num_VIN\" AS \"VIN\", TIPO.\"Name\" AS \"Tipo\" FROM \"@SCGD_VEHICULO\" VEHI LEFT OUTER JOIN \"@SCGD_TIPOVEHICULO\" TIPO ON TIPO.\"Code\" = VEHI.\"U_Tipo\" LEFT OUTER JOIN \"@SCGD_COLOR\" COL ON COL.\"Code\" = VEHI.\"U_Cod_Col\" WHERE VEHI.\"Code\" = '{0}'";
        #endregion

        #region "Consulta Costeos"

        private const string strConsultaGoodReceive = "SELECT Count(1) FROM \"@SCGD_GOODRECEIVE\" ¿#? WHERE \"U_As_Entr\" <> - 1 AND \"Status\" = 'O' AND \"U_Unidad\" = '{0}'";
        
        #endregion

        #region "Carga Combos"

        private const string strCbMarca = "SELECT \"Code\", \"Name\" FROM \"@SCGD_MARCA\" ¿#? {0}";

        private const string strCbEstilo = "SELECT \"Code\", \"Name\" FROM \"@SCGD_ESTILO\" ¿#? {0} ORDER BY \"Name\"";

        private const string strCbModelo = "SELECT \"Code\", \"U_Descripcion\" FROM \"@SCGD_MODELO\" ¿#? {0} ORDER BY \"U_Descripcion\" ";

        private const string strCbUbicaciones = "SELECT \"Code\", \"Name\" FROM \"@SCGD_UBICACIONES\" ¿#? {0} ";

        private const string strCbTipoVehiculo = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TIPOVEHICULO\" ¿#? {0}";

        private const string strCbEstado = "SELECT \"Code\", \"Name\" FROM \"@SCGD_ESTADO\" ¿#? {0}";

        private const string strCbDisponibilidad = "SELECT \"Code\", \"Name\" FROM \"@SCGD_DISPONIBILIDAD\" ¿#? {0}";

        private const string strCbCategoriaVehiculo = "SELECT \"Code\", \"Name\" FROM \"@SCGD_CATEGORIA_VEHI\" ¿#? {0}";

        private const string strCbTipoContrato = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TIPOCONTRATO\" ¿#? {0}";

        private const string strCbMarcaMotor = "SELECT \"Code\", \"Name\" FROM \"@SCGD_MARCA_MOTOR\" ¿#? {0}";

        private const string strCbTrasmision = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TRANSMISION\" ¿#? {0}";

        private const string strCbCarroceria = "SELECT \"Code\", \"Name\" FROM \"@SCGD_CARROCERIA\" ¿#? {0}";

        private const string strCbTraccion = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TRACCION\" ¿#? {0}";

        private const string strCbCabina = "SELECT \"Code\", \"Name\" FROM \"@SCGD_CABINA\" ¿#? {0}";

        private const string strCbCombustible = "SELECT \"Code\", \"Name\" FROM \"@SCGD_COMBUSTIBLE\" ¿#? {0}";

        private const string strCbTecho = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TECHO\" ¿#? {0}";

        private const string strCbClasificacion = "SELECT \"Code\", \"U_Desc\" FROM \"@SCGD_CLASIFICACION\" ¿#? {0}";

        private const string strCbMoneda = "SELECT \"CurrCode\", \"CurrName\" FROM \"OCRN\" ¿#? {0}";

        private const string strCbBonos = "SELECT \"Code\", \"Name\" FROM \"@SCGD_TIPOBONO\" ¿#? ORDER BY \"Code\"";

        private const string strWhereCode = " WHERE \"{0}\" = '{1}'";
        
        #endregion

        #region "Consultas Generales"

        //private const string strAutoKeyVEH = "SELECT \"AutoKey\" FROM ONNM ¿#? WHERE \"ObjectCode\" = 'SCGD_VEH'";

        private const string strAutoKeyVEH = "SELECT COALESCE(MAX(\"DocEntry\"),0)+1 FROM \"@SCGD_VEHICULO\" ¿#? ";

        //private const string strAutoKeyVEH = "SELECT COALESCE(MAX(\"DocEntry\"),0)+1 FROM \"@SCGD_VEHICULO\" ¿#? ";

        private const string strCodeVehCodUnid = "SELECT \"Code\" FROM \"@SCGD_VEHICULO\" ¿#? WHERE \"U_Cod_Unid\" = '{0}'";

        private const string strCodeVehPlaca = "SELECT \"Code\" FROM \"@SCGD_VEHICULO\" ¿#? WHERE \"U_Num_Plac\" = '{0}'";

        private const string strWhereCodeDi = "{0} and \"Code\" != '{1}'";

        private const string strMarcaComercialVehiculo = "SELECT ARTV.\"U_ArtVent\" FROM \"@SCGD_VEHICULO\" VEH ¿#? INNER JOIN \"@SCGD_CONF_ART_VENTA\" ARTV ON VEH.\"U_ArtVent\" = ARTV.\"Code\" WHERE VEH.\"U_Cod_Unid\" = '{0}'";

        #endregion

        #region "Componentes"
        private const string strComponentesPorDefecto = "SELECT  Count(1) CountN FROM \"@SCGD_ACCXMODE\" ¿#? INNER JOIN OITM ¿#? ON OITM.\"ItemCode\" = \"U_Accesorio\" WHERE \"@SCGD_ACCXMODE\".\"U_Modelo\" = '{0}'";

        private const string strComponentesPorDefectoDatos = "SELECT \"U_Accesorio\", \"U_ItemName\", OITM.\"ItemName\" FROM \"@SCGD_ACCXMODE\" ¿#? INNER JOIN OITM ¿#? ON OITM.\"ItemCode\" = \"U_Accesorio\" WHERE \"@SCGD_ACCXMODE\".\"U_Modelo\" = '{0}'";

        private const string strCountExpeXMode = "SELECT COUNT(1) AS \"CountN\" FROM \"@SCGD_ESPEXMODE\" ¿#? {0}";
        
        #endregion
    }
}
