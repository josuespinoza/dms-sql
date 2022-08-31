using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Requisiciones
{
    public class LineaRequisicion
    {
        public Int16 U_HoraM { get; set; }
        public Int16 U_TipoM { get; set; }
        public Int16 U_ReqOriPen { get; set; }
        public Int32 DocEntry { get; set; }
        public Int32 LineId { get; set; }
        public Int32 VisOrder { get; set; }
        public Int32 U_SCGD_CodTipoArt { get; set; }
        public Int32 U_SCGD_CCosto { get; set; }
        public Int32 U_SCGD_CodEst { get; set; }
        public Int32 U_SCGD_Chk { get; set; }
        public Int32 U_SCGD_LNumOr { get; set; }
        public Int32 U_SCGD_DocOr { get; set; }
        public DateTime? U_FechaM { get; set; }
        public Double U_SCGD_CantDispo { get; set; }
        public Double U_SCGD_CantSol { get; set; }
        public Double U_SCGD_CantRec { get; set; }
        public Double U_SCGD_CantATransf { get; set; }
        public Double U_SCGD_CantPen { get; set; }
        public Double U_SCGD_COrig { get; set; }
        public Double U_SCGD_CAju { get; set; }
        public String U_SCGD_DescArticulo { get; set; }
        public String U_SCGD_CodBodOrigen { get; set; }
        public String U_SCGD_CodBodDest { get; set; }
        public String U_SCGD_TipoArticulo { get; set; }
        public String U_SCGD_Estado { get; set; }
        public String U_SCGD_CodArticulo { get; set; }
        public String U_SCGD_Lidsuc { get; set; }
        public String U_DeUbic { get; set; }
        public String U_AUbic { get; set; }
        public String U_Obs_Req { get; set; }
        public String U_SCGD_ID { get; set; }
        public Int32 DataSourceOffset { get; set; }
        public String U_DesDeUbic { get; set; }
        public String U_DesAUbic { get; set; }
    }
}