using System.Security.Permissions;

namespace DMS_Connector
{
    public partial class Queries
    {
        private const string strLineasPaquetes = "SELECT TT.\"Code\", TT.\"Quantity\", OI.\"U_SCGD_Duracion\", \"U_SCGD_TipoArticulo\" FROM OITT IT ¿#? INNER JOIN ITT1 TT   ON IT.\"Code\" = TT.\"Father\" INNER JOIN OITM OI ¿#? ON OI.\"ItemCode\" = TT.\"Code\" WHERE IT.\"Code\" = '{0}' ";
        private const string strEquipoAgenda = " SELECT HE.\"U_SCGD_Equipo\" FROM \"@SCGD_AGENDA\" AG ¿#? INNER JOIN OHEM HE ¿#? ON AG.\"U_CodAsesor\" = HE.\"empID\" WHERE \"DocEntry\" = '{0}' ";

        private const string strCitasXFecha = " SELECT CI.\"DocEntry\", CI.\"U_Num_Serie\", CI.\"U_NumCita\", CI.\"U_FechaCita\", CI.\"U_HoraCita\", CI.\"U_Cod_Agenda\", CI.\"U_Cod_Sucursal\", CI.\"U_Num_Cot\", SUM(IT.\"U_SCGD_Duracion\") AS \"U_SCGD_Duracion\" FROM \"@SCGD_CITA\" CI LEFT OUTER JOIN OQUT QU ON QU.\"DocEntry\" = CI.\"U_Num_Cot\" AND QU.\"U_SCGD_NoSerieCita\" IS NOT NULL AND QU.\"U_SCGD_NoCita\" IS NOT NULL LEFT OUTER JOIN QUT1 Q1 ON Q1.\"DocEntry\" = QU.\"DocEntry\" AND Q1.\"U_SCGD_Aprobado\" IN (1,4) INNER JOIN OITM IT ON IT.\"ItemCode\" = Q1.\"ItemCode\" WHERE CI.\"U_Cod_Agenda\" = '{0}' AND CI.\"U_Cod_Sucursal\" = '{1}' AND (\"U_FechaCita\" >= '{2}' AND \"U_FechaCita\" <= '{3}') AND (\"U_HoraCita\" >= '{4}' AND \"U_HoraCita\" <= '{5}') AND CI.\"U_Estado\" <> '{6}' GROUP BY CI.\"DocEntry\", CI.\"U_NumCita\", CI.\"U_Num_Serie\", CI.\"U_FechaCita\", CI.\"U_HoraCita\", CI.\"U_Cod_Agenda\", CI.\"U_Cod_Sucursal\", CI.\"U_Num_Cot\" ";

        private const string strCitasXFechaHora = " SELECT \"DocEntry\", \"U_Num_Serie\", \"U_NumCita\", \"U_Cod_Sucursal\", \"U_Cod_Agenda\", \"U_FechaCita\", \"U_HoraCita\", \"U_Estado\", \"U_Num_Cot\" FROM \"@SCGD_CITA\" ¿#? WHERE (\"U_FechaCita\" >= '{0}' AND \"U_FechaCita\" <= '{1}') AND (\"U_HoraCita\" >= {2} AND \"U_HoraCita\" <= {3}) AND \"U_Cod_Sucursal\" = '{4}' AND \"U_Cod_Agenda\" = '{5}' AND \"U_Estado\" <> '{6}' {7} ORDER BY \"U_FechaCita\" ASC, \"U_HoraCita\" ASC ";
        private const string strCitasCotizacion = " SELECT CI.\"DocEntry\", \"U_Num_Serie\", \"U_NumCita\", \"U_Cod_Sucursal\", \"U_Cod_Agenda\", \"U_FechaCita\", \"U_HoraCita\", CI.\"U_Estado\", \"U_Num_Cot\" FROM \"@SCGD_CITA\" CI ¿#? LEFT OUTER JOIN OQUT QU ¿#? ON QU.\"DocEntry\" = CI.\"U_Num_Cot\" AND QU.\"U_SCGD_NoSerieCita\" IS NOT NULL AND QU.\"U_SCGD_NoCita\" IS NOT NULL INNER JOIN QUT1 Q1 ON Q1.\"DocEntry\" = QU.\"DocEntry\" AND Q1.\"U_SCGD_Aprobado\" IN (1,4) AND IFNULL(\"U_SCGD_EstAct\", 0) <> 3 WHERE (\"U_FechaCita\" >='{0}' and \"U_FechaCita\" <='{1}') AND (\"U_HoraCita\" >= {2} and \"U_HoraCita\" <= {3}) AND \"U_Cod_Sucursal\" = '{4}' AND \"U_Cod_Tecnico\" = '{5}' AND CI.\"U_Estado\" <> '{6}' AND \"U_Num_Serie\" IS NOT NULL ORDER BY \"U_FechaCita\" ASC, \"U_HoraCita\" ASC ";

        private const string strGetTiempoAsignadoXNumCitaSucu = " SELECT \"U_SCGD_TiOtor\" AS \"TiempoOtorgado\" FROM QUT1 Q ¿#? INNER JOIN OQUT OQ ¿#? ON q.\"DocEntry\" = oq.\"DocEntry\" WHERE OQ.\"U_SCGD_NoSerieCita\" = '{0}' AND OQ.\"U_SCGD_NoCita\" = '{1}' AND OQ.\"U_SCGD_idSucursal\" = '{2}' AND Q.\"U_SCGD_TiOtor\" <> 0 ";
        private const string strGetTiempoAsignadoXNumCita = " SELECT \"U_SCGD_TiOtor\" AS \"TiempoOtorgado\" FROM QUT1 Q ¿#? INNER JOIN OQUT OQ ¿#? ON q.\"DocEntry\" = oq.\"DocEntry\" WHERE OQ.\"U_SCGD_NoSerieCita\" = '{0}' AND OQ.\"U_SCGD_NoCita\" = '{1}' AND Q.\"U_SCGD_TiOtor\" <> 0 ";
        private const string strGetTiempoAsignadoXOT = "SELECT \"U_SCGD_TiOtor\" AS \"TiempoOtorgado\" FROM QUT1 Q ¿#? INNER JOIN OQUT OQ ¿#? ON q.\"DocEntry\" = oq.\"DocEntry\" WHERE Q.\"U_SCGD_NoOT\" = '{0}' AND OQ.\"U_SCGD_idSucursal\" = '{1}' AND Q.\"U_SCGD_TiOtor\" <> 0 ";

        private const string strGetDurSerXEmp = "SELECT SUM(I.\"U_SCGD_Duracion\") FROM OQUT QU ¿#? INNER JOIN QUT1 Q1 ¿#? ON Q1.\"DocEntry\" = QU.\"DocEntry\" INNER JOIN OITM I ¿#? ON Q1.\"ItemCode\" = I.\"ItemCode\" WHERE Q1.\"U_SCGD_Aprobado\" IN (1,4) AND I.\"U_SCGD_TipoArticulo\" = 2 AND q1.\"U_SCGD_NoOT\" = '{0}' AND Q1.\"U_SCGD_EmpAsig\" = '{1}' ";
        private const string strGetDiscTiemOtor = " SELECT DISTINCT (qu.\"U_SCGD_TiOtor\") FROM QUT1 qu ¿#? INNER JOIN OQUT oq ¿#? ON qu.\"DocEntry\" = oq.\"DocEntry\" WHERE oq.\"U_SCGD_NoCita\" = '{0}' AND oq.\"U_SCGD_NoSerieCita\" = '{1}' AND qu.\"U_SCGD_TiOtor\" <> 0 ";
        private const string strGetAgendaSusp = " SELECT AGS.\"DocEntry\", AGS.\"U_Cod_Sucur\", AGS.\"U_Cod_Agenda\", AGS.\"U_Fha_Desde\", AGS.\"U_Hora_Desde\", AGS.\"U_Fha_Hasta\", AGS.\"U_Hora_Hasta\", AGS.\"U_Estado\", AGS.\"U_Observ\" FROM \"@SCGD_AGENDA_SUSP\" AGS ¿#? WHERE AGS.\"U_Fha_Desde\" = '{0}' AND AGS.\"U_Cod_Sucur\" = '{1}' AND AGS.\"U_Cod_Agenda\" = '{2}' AND AGS.\"U_Estado\" = 'Y' ";

        private const string strCitAgeEsta = " SELECT \"DocEntry\", \"U_HoraCita\", \"U_NumCita\", \"U_Num_Serie\", \"U_Cod_Unid\", \"U_CardCode\", \"U_CardName\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_Cod_Agenda\" = '{0}' AND \"U_FechaCita\" = '{1}' AND (\"U_Estado\" <> '{2}' OR \"U_Estado\" IS NULL) ";
        private const string strCitasXNumXSerie = " SELECT \"DocEntry\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_NumCita\" = '{0}' AND \"U_Num_Serie\" = '{1}' ";

        private const string strCitasXTecFhaServ = " SELECT CI.\"DocEntry\", CI.\"U_HoraCita\", CI.\"U_NumCita\", CI.\"U_Num_Serie\", CI.\"U_Cod_Unid\", CI.\"U_CardCode\", CI.\"U_CardName\", CI.\"U_FhaServ\", CI.\"U_HoraServ\", CI.\"U_Cod_Tecnico\" FROM \"@SCGD_CITA\" CI INNER JOIN OQUT QU ON QU.\"DocEntry\" = CI.\"U_Num_Cot\" WHERE \"U_Cod_Tecnico\" = '{0}' AND CI.\"U_FhaServ\" = '{1}' AND (CI.\"U_Estado\" <> '{2}' OR CI.\"U_Estado\" IS NULL) AND (QU.\"U_SCGD_Numero_OT\" IS NULL OR QU.\"U_SCGD_Numero_OT\" = '') ORDER BY CI.\"U_HoraCita\" ASC ";
        private const string stCitaIDXnumCita = "SELECT \"DocEntry\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_NumCita\" = '{0}' AND \"U_Num_Serie\" = '{1}' ";
        private const string strCitaAgeXNFechNEst = " SELECT \"DocEntry\", \"U_HoraCita\", \"U_HoraCita_Fin\", \"U_NumCita\", \"U_Num_Serie\", \"U_Cod_Unid\", \"U_CardCode\", \"U_CardName\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_Cod_Agenda\" = '{0}' AND \"U_FechaCita\" <> '{1}' AND \"U_FhaCita_Fin\" = '{1}' AND (\"U_Estado\" <> '{2}' OR \"U_Estado\" IS NULL) ";
        private const string strCitaXFhaServXTec = " SELECT CI.\"DocEntry\", CI.\"U_HoraServ\", CI.\"U_HoraServ_Fin\", CI.\"U_NumCita\", CI.\"U_Num_Serie\", CI.\"U_Cod_Unid\", CI.\"U_CardCode\", CI.\"U_CardName\", CI.\"U_FhaServ\", CI.\"U_FhaServ_Fin\" FROM \"@SCGD_CITA\" CI ¿#? INNER JOIN OQUT QU ¿#? ON QU.\"DocEntry\" = CI.\"U_Num_Cot\" WHERE CI.\"U_Cod_Tecnico\" = '{0}' AND CI.\"U_FhaServ\" < '{1}' AND CI.\"U_FhaServ_Fin\" >= '{1}' AND (CI.\"U_Estado\" <> '{2}' OR CI.\"U_Estado\" IS NULL) AND (QU.\"U_SCGD_Numero_OT\" IS NULL OR QU.\"U_SCGD_Numero_OT\" = '') ";
        private const string strGetDuracioCitaXNumCita = " SELECT SUM(I.\"U_SCGD_Duracion\" * q1.\"Quantity\") FROM \"@SCGD_CITA\" C ¿#? INNER JOIN OQUT Q ¿#? ON C.\"U_Num_Cot\" = Q.\"DocEntry\" INNER JOIN QUT1 Q1 ¿#? ON Q.\"DocEntry\" = Q1.\"DocEntry\" INNER JOIN OITM I ¿#? ON Q1.\"ItemCode\" = I.\"ItemCode\" AND Q1.\"U_SCGD_Aprobado\" IN (1,4) AND I.\"U_SCGD_TipoArticulo\" = 2 WHERE Q.\"U_SCGD_NoSerieCita\" = '{0}' AND Q.\"U_SCGD_NoCita\" = '{1}' AND C.\"U_Cod_Sucursal\" = '{2}' ";
        private const string strGetDuracionCitaXOT = "SELECT SUM(I.\"U_SCGD_Duracion\" * Q1.\"Quantity\") FROM OQUT QU ¿#? INNER JOIN QUT1 Q1 ¿#? ON Q1.\"DocEntry\" = QU.\"DocEntry\" INNER JOIN OITM I ¿#? ON Q1.\"ItemCode\" = I.\"ItemCode\" WHERE Q1.\"U_SCGD_Aprobado\" IN (1,4) AND I.\"U_SCGD_TipoArticulo\" = 2 AND q1.\"U_SCGD_NoOT\" = '{0}' AND Q1.\"U_SCGD_EmpAsig\" = '{1}' ";
        private const string strGetCitaNumCot = " SELECT \"U_Num_Cot\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_Num_Serie\" = '{0}' AND \"U_NumCita\" = '{1}' AND \"U_Cod_Sucursal\" = '{2}' ";
        private const string strCiteVehi = " SELECT C.\"U_CardCode\", C.\"U_CardName\", C.\"U_Cod_Unid\", V.\"U_Num_Plac\" FROM \"@SCGD_CITA\" C ¿#? INNER JOIN \"@SCGD_VEHICULO\" V ¿#? ON C.\"U_Cod_Unid\" = V.\"U_Cod_Unid\" WHERE \"U_Num_Serie\" = '{0}' AND \"U_NumCita\" = '{1}' ";
        private const string strCotXOT = " SELECT QU.\"U_SCGD_NoSerieCita\", QU.\"U_SCGD_NoCita\", QU.\"U_SCGD_Cod_Unidad\", QU.\"U_SCGD_Num_Placa\", QU.\"CardCode\", QU.\"CardName\" FROM OQUT QU ¿#? WHERE \"U_SCGD_Numero_OT\" = '{0}' ";
        private const string strAgenXCita = " Select \"A\".\"U_Agenda\" from \"@SCGD_CITA\" as \"C\" ¿#? inner join \"@SCGD_AGENDA\" as \"A\" ¿#? on \"C\".\"U_Cod_Agenda\" = \"A\".\"DocEntry\" where \"C\".\"DocEntry\" = '{0}' ";

        #region "...BuscadorArtuiculosCitas..."

        private const string strGetArticulosCita = " SELECT TOP 100 ' ' AS \"sele\", oi.\"ItemCode\" AS \"code\", oi.\"ItemName\" AS \"desc\", cfnb.\"U_Rep\" AS \"bode\", (SELECT \"OnHand\" FROM OITW ¿#? WHERE oitw.\"WhsCode\" = cfnb.\"U_Rep\" AND oitw.\"ItemCode\" = oi.\"ItemCode\") AS \"csto\", 1 AS \"cant\", it.\"Price\" AS \"prec\", it.\"Currency\" AS \"mone\", oi.\"U_SCGD_T_Fase\" AS \"nofa\", oi.\"U_SCGD_Duracion\" AS \"dura\", oi.\"CodeBars\" FROM OITM oi ¿#? INNER JOIN \"@SCGD_CONF_BODXCC\" cfnb ¿#? ON oi.\"U_SCGD_CodCtroCosto\" = cfnb.U_CC INNER JOIN ITM1 it ¿#? ON oi.\"ItemCode\" = it.\"ItemCode\" WHERE it.\"PriceList\" = '{0}' AND cfnb.\"DocEntry\" = '{1}' ";
        private const string strGetArticulosEspCitas = " SELECT TOP 100 ' ' AS \"sele\", oi.\"ItemCode\" AS \"code\", oi.\"ItemName\" AS \"desc\", cfnb.\"U_Rep\" AS \"bode\", (SELECT \"OnHand\" FROM OITW ¿#? WHERE oitw.\"WhsCode\" = cfnb.\"U_Rep\" AND oitw.\"ItemCode\" = oi.\"ItemCode\") AS \"csto\", 1 AS \"cant\", it.\"Price\" AS \"prec\", it.\"Currency\" AS \"mone\", oi.\"U_SCGD_T_Fase\" AS \"nofa\", Art.\"U_Duracion\" AS \"dura\", oi.\"CodeBars\" FROM OITM oi ¿#? INNER JOIN \"@SCGD_CONF_BODXCC\" cfnb ¿#? ON oi.\"U_SCGD_CodCtroCosto\" = cfnb.U_CC INNER JOIN ITM1 it ¿#? ON oi.\"ItemCode\" = it.\"ItemCode\" INNER JOIN \"@SCGD_ARTXESP\" Art ¿#? ON oi.\"ItemCode\" = art.\"U_ItemCode\" WHERE it.\"PriceList\" = '{0}' AND cfnb.\"DocEntry\" = '{1}' {2} ";
        private const string strServiciosExternosCitas = " SELECT TOP 100 ' ' AS \"sele\", oi.\"ItemCode\" AS \"code\", oi.\"ItemName\" AS \"desc\", cfnb.\"U_Rep\" AS \"bode\", (SELECT \"OnHand\" FROM OITW ¿#? WHERE oitw.\"WhsCode\" = cfnb.\"U_Rep\" AND oitw.\"ItemCode\" = oi.\"ItemCode\") AS \"csto\", 1 AS \"cant\", it.\"Price\" AS \"prec\", it.\"Currency\" AS \"mone\", oi.\"U_SCGD_T_Fase\" AS \"nofa\", oi.\"U_SCGD_Duracion\" AS \"dura\", oi.\"CodeBars\" FROM OITM ¿#? oi INNER JOIN \"@SCGD_CONF_BODXCC\" cfnb ¿#? ON oi.\"U_SCGD_CodCtroCosto\" = cfnb.U_CC INNER JOIN ITM1 it ¿#? ON oi.\"ItemCode\" = it.\"ItemCode\" WHERE it.\"PriceList\" = '{0}' AND cfnb.\"DocEntry\" = '{1}' AND oi.\"U_SCGD_TipoArticulo\" IN (3,4,5) ";


        #endregion

        #region "...BusquedaCitas..."

        private const string strBusquedaCitas = " SELECT C.\"DocEntry\", CONCAT(CONCAT(Q.\"U_SCGD_NoSerieCita\", ' - '), Q.\"U_SCGD_NoCita\") AS \"NoCita\", C.\"U_FechaCita\", C.\"U_HoraCita\", Q.\"DocEntry\", Q.\"U_SCGD_Numero_OT\", T.\"Name\", S.\"Name\" AS \"Sucursal\", Q.\"U_SCGD_Cod_Unidad\", Q.\"U_SCGD_Num_Placa\", CE.\"U_Descripcion\" AS \"Confirmacion\", Q.\"U_SCGD_Gorro_Veh\", Q.\"U_SCGD_No_Visita\", Q.\"CardCode\", Q.\"CardName\", Q.\"U_SCGD_Des_Marc\", Q.\"U_SCGD_Des_Esti\", Q.\"U_SCGD_Des_Mode\", C.\"U_Name_Tecnico\", C.\"U_Name_Asesor\" FROM \"@SCGD_CITA\" C ¿#? LEFT OUTER JOIN \"OQUT\" Q ¿#? ON Q.\"DocEntry\" = C.\"U_Num_Cot\" LEFT OUTER JOIN \"@SCGD_SUCURSALES\" S ¿#? ON C.\"U_Cod_Sucursal\" = S.\"Code\" LEFT OUTER JOIN OHEM H ¿#? ON Q.\"OwnerCode\" = H.\"empID\" LEFT OUTER JOIN \"@SCGD_TIPO_ORDEN\" T ¿#? ON Q.\"U_SCGD_Tipo_OT\" = T.\"Code\" LEFT OUTER JOIN \"@SCGD_CITA_ESTADOS\" CE ¿#? ON C.\"U_Estado\" = CE.\"Code\" ";

        #endregion

        #region "...FrmCalendario..."
        private const string strCitaXFechaFin = " SELECT CI.\"DocEntry\", CI.\"U_Num_Serie\", CI.\"U_NumCita\", CI.\"U_FechaCita\", CI.\"U_HoraCita\", \"U_FhaCita_Fin\", \"U_HoraCita_Fin\", CI.\"U_Cod_Agenda\", CI.\"U_Cod_Sucursal\", CI.\"U_Num_Cot\" FROM \"@SCGD_CITA\" CI WHERE (\"U_FechaCita\" <> \"U_FhaCita_Fin\") AND \"U_FhaCita_Fin\" >= '{0}' AND \"U_FhaCita_Fin\" <= '{1}' AND CI.\"U_Cod_Sucursal\" = '{2}' AND CI.\"U_Cod_Agenda\" = '{3}' AND CI.\"U_Estado\" <> '{4}' AND CI.\"U_NumCita\" IS NOT NULL ";
        #endregion

        #region frmListaCitas

        private const string strConsultaAgendas = "SELECT \"DocEntry\", \"DocNum\", \"U_Agenda\" FROM  \"@SCGD_AGENDA\" ¿#? WHERE \"U_Cod_Sucursal\" = '{0}' AND \"U_EstadoLogico\" = 'Y'"; //Consulta las agendas
        private const string strDocEntryCita = "SELECT \"DocEntry\" FROM \"@SCGD_CITA\" ¿#? WHERE \"U_NumCita\" = '{0}' AND \"U_Num_Serie\" = '{1}'"; //Consulta el DocEntry de una cita
        private const string strNombreBaseDatosTaller = "SELECT \"U_BDSucursal\" FROM  \"@SCGD_SUCURSALES\" ¿#? WHERE \"Code\" = '{0}'"; //Consulta el nombre de la base de datos de taller
        private const string strConsultaAgendaSuspension = "SELECT \"DocEntry\", \"U_Cod_Sucur\",\"U_Cod_Agenda\",\"U_Fha_Desde\",\"U_Hora_Desde\",\"U_Fha_Hasta\",\"U_Hora_Hasta\",\"U_Estado\",\"U_Observ\" FROM \"@SCGD_AGENDA_SUSP\" ¿#? WHERE \"U_Fha_Desde\" BETWEEN '{0}' AND '{1}' AND \"U_Cod_Sucur\" = '{2}' AND \"U_Cod_Agenda\" = '{3}' AND \"U_Estado\" = 'Y'";
        private const string strConsultaMecanicosBloqueados = "SELECT  \"BM\".\"DocEntry\", \"BM\".\"U_IdMec\", \"BM\".\"U_FechI\", \"LBM\".\"U_FechCon\", \"BM\".\"U_HorI\", \"BM\".\"U_FechF\", \"BM\".\"U_HoraF\", \"BM\".\"U_Observ\" FROM \"@SCGD_BLOCMEC\" AS \"BM\" ¿#? INNER JOIN \"@SCGD_LINEAS_BLOME\" AS \"LBM\" ¿#? ON \"BM\".\"DocEntry\" = \"LBM\".\"DocEntry\" WHERE  \"LBM\".\"U_FechCon\" = '{0}' AND \"BM\".\"U_IdSucu\" = '{1}'";
        private const string strConsultaListaSucursales = "SELECT \"Code\", \"Name\"  FROM \"@SCGD_SUCURSALES\" ¿#?"; //Consulta el lista de las sucursales
        private const string strConsultaHorarioAlmuerzo = "SELECT \"U_HorAlI\", \"U_HoraAlF\" FROM \"@SCGD_CONF_SUCURSAL\" ¿#? WHERE \"U_Sucurs\" = '{0}'"; //Consulta el horario de almuerzo de la sucursal

        #endregion

        #region frmCalendarioColor

        private const string strConsultaConfiguracionSucursal = "SELECT \"U_HoraInicio\", \"U_HoraFin\", \"U_UsaDurEC\" FROM \"@SCGD_CONF_SUCURSAL\" ¿#? WHERE \"U_Sucurs\" = '{0}'";
        private const string strConsultaIntervalosCitas = "SELECT \"U_IntervaloCitas\" FROM \"@SCGD_AGENDA\" ¿#? WHERE \"DocNum\" = '{0}'  AND \"U_Cod_Sucursal\" = '{1}'";
        private const string strConsultaConfiguracionAgenda = "SELECT \"U_Agenda\", \"U_EstadoLogico\", \"U_IntervaloCitas\", \"U_Abreviatura\", \"U_CodAsesor\", \"U_CodTecnico\", \"U_RazonCita\", \"U_ArticuloCita\", \"U_VisibleWeb\", \"U_CantCLunes\", \"U_CantCMartes\", \"U_CantCMiercoles\", \"U_CantCJueves\", \"U_CantCViernes\", \"U_CantCSabado\", \"U_CantCDomingo\", \"U_Num_Art\", \"U_Num_Razon\", \"U_Cod_Sucursal\", \"U_NameAsesor\", \"U_NameTecnico\", \"U_TmpServ\" FROM \"@SCGD_AGENDA\" ¿#? WHERE \"DocEntry\" = '{0}' AND \"U_Cod_Sucursal\" = '{1}'";
        private const string strConsultaCitas = "SELECT CI.\"DocEntry\", CI.\"U_Num_Serie\", CI.\"U_NumCita\", CI.\"U_FechaCita\", CI.\"U_HoraCita\", \"U_FhaCita_Fin\", \"U_HoraCita_Fin\", CI.\"U_Cod_Agenda\", CI.\"U_Cod_Sucursal\", CI.\"U_Num_Cot\" FROM \"@SCGD_CITA\" CI ¿#? WHERE(\"U_FechaCita\" <> \"U_FhaCita_Fin\") AND \"U_FhaCita_Fin\" BETWEEN '{0}' AND '{1}' AND CI.\"U_Cod_Sucursal\"	= '{2}' AND CI.\"U_Cod_Agenda\"	= '{3}' AND CI.\"U_Estado\" <> '{4}' AND CI.\"U_NumCita\" IS NOT NULL"; //Consulta el listado de citas

        #endregion

        #region ControladorOrdenTrabajo
        private const string strConsultaDatosCita = "SELECT T0.\"U_HoraServ\", T0.\"U_FhaServ\" FROM \"@SCGD_CITA\" T0 ¿#? WHERE T0.\"U_Num_Cot\" = '{0}' AND T0.\"U_HoraServ\" IS NOT NULL AND T0.\"U_FhaServ\" IS NOT NULL";
        #endregion

    }
}