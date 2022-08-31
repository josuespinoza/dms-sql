namespace DMS_Connector.Business_Logic.Queries
{
    public class QueriesRecursos
    {
        #region "Propiedades"

        public string SQL_strConsultaCosteosVehi { get; set; }
        public string SQL_strConsultaCosteosAsientos { get; set; }
        public string SQL_strConsultaCosteosAsientos2 { get; set; }
        public string SQL_strConsultaCosteosFacRes { get; set; }
        public string SQL_strConsultaCosteosNotCre { get; set; }
        public string SQL_strConsultaCosteosFacCli { get; set; }
        public string SQL_strConsultaCosteosSalMer { get; set; }

        //public string HANA_strConsultaCosteos { get; set; }
        public string HANA_strConsultaCosteosVehi { get; set; }
        public string HANA_strConsultaCosteosAsientos { get; set; }
        public string HANA_strConsultaCosteosAsientos2 { get; set; }
        public string HANA_strConsultaCosteosFacRes { get; set; }
        public string HANA_strConsultaCosteosNotCre { get; set; }
        public string HANA_strConsultaCosteosFacCli { get; set; }
        public string HANA_strConsultaCosteosSalMer { get; set; }

        public string SQL_strConsultaDocumentos { get; set; }
        public string HANA_strConsultaDocumentos { get; set; }

        public string strExpeXModelo { get; set; }

        public string HANA_strQueryNombreEmpleadoXOT { get; set; }
        public string SQL_strQueryNombreEmpleadoXOT { get; set; }

        public string HANA_strQueryAsignacionesOTInterna { get; set; }
        public string SQL_strQueryAsignacionesOTInterna { get; set; }

        public string SQL_strQueryOcupacionSemanal { get; set; }
        public string SQL_strQueryOcupacionMensual { get; set; }

        public string SQL_strQueryInconsisRequisiciones { get; set; }
        public string SQL_strQueryInconsisCompras { get; set; }

        public string SQL_EdicionRegistroTiempo { get; set; }
        public string HANA_EdicionRegistroTiempo { get; set; }

        public string SQL_ExisteOrdenCompraNC { get; set; }
        public string HANA_ExisteOrdenCompraNC { get; set; }

        #endregion

        #region "Construtor"

        public QueriesRecursos()
        {
            SQL_strConsultaCosteosVehi = Resource.SQL_QueryCosteosVehi;
            SQL_strConsultaCosteosAsientos = Resource.SQL_QueryCosteosAsientos;
            SQL_strConsultaCosteosAsientos2 = Resource.SQL_QueryCosteosAsientos2;
            SQL_strConsultaCosteosFacRes = Resource.SQL_QueryCosteosFacRes;
            SQL_strConsultaCosteosNotCre = Resource.SQL_QueryCosteosNotCre;
            SQL_strConsultaCosteosFacCli = Resource.SQL_QueryCosteosFacCli;
            SQL_strConsultaCosteosSalMer = Resource.SQL_QueryCosteosSalMer;

            HANA_strConsultaCosteosVehi = Resource.HANA_QueryCosteosVehi;
            HANA_strConsultaCosteosAsientos = Resource.HANA_QueryCosteosAsientos;
            HANA_strConsultaCosteosAsientos2 = Resource.HANA_QueryCosteosAsientos2;
            HANA_strConsultaCosteosFacRes = Resource.HANA_QueryCosteosFacRes;
            HANA_strConsultaCosteosNotCre = Resource.HANA_QueryCosteosNotCre;
            HANA_strConsultaCosteosFacCli = Resource.HANA_QueryCosteosFacCli;
            HANA_strConsultaCosteosSalMer = Resource.String1HANA_QueryCosteosSalMer;

            SQL_strConsultaDocumentos = Resource.SQL_QueryDocumentos;
            HANA_strConsultaDocumentos = Resource.HANA_QueryDocumentos;
            strExpeXModelo = Resource.QueryExpeXModelo;

            HANA_strQueryNombreEmpleadoXOT = Resource.HANA_QueryNombreEmpleadoXOT;
            SQL_strQueryNombreEmpleadoXOT = Resource.SQL_QueryNombreEmpleadoXOT;

            HANA_strQueryAsignacionesOTInterna = Resource.HANA_AsignacionesOTInterna;
            SQL_strQueryAsignacionesOTInterna = Resource.SQL_AsignacionesOTInterna;

            SQL_strQueryOcupacionSemanal = Resource.SQL_QueryOcupacionSemanal;
            SQL_strQueryOcupacionMensual = Resource.SQL_QueryOcupacionMensual;
            SQL_strQueryInconsisRequisiciones = Resource.SQL_QueryInconsisRequisiciones;
            SQL_strQueryInconsisCompras = Resource.SQL_QueryInconsisCompras;

            //Registro de tiempo
            SQL_EdicionRegistroTiempo = Resource.SQL_EdicionRegistroTiempo;
            HANA_EdicionRegistroTiempo = Resource.HANA_EdicionRegistroTiempo;

            SQL_ExisteOrdenCompraNC = Resource.SQL_ExisteOrdenCompraNC;
            HANA_ExisteOrdenCompraNC = Resource.HANA_ExisteOrdenCompraNC;
        }

        #endregion
    }
}
