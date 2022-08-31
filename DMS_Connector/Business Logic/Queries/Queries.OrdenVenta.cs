using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector
{
    public partial class Queries
    {
        private const string strLinCotCant = " SELECT QUT1.\"DocEntry\", QUT1.\"ItemCode\", QUT1.\"Quantity\", QUT1.\"LineNum\" FROM QUT1 ¿#? INNER JOIN OQUT q ¿#? ON QUT1.\"DocEntry\" = q.\"DocEntry\" WHERE q.\"U_SCGD_Numero_OT\" = '{0}' AND QUT1.\"U_SCGD_Aprobado\" = '1' ";

        #region ...TipoOtInterna
        private const string strLoadLines2 = " SELECT RDR1.\"{1}\" FROM RDR1 ¿#? INNER JOIN ORDR ¿#? ON RDR1.\"DocEntry\" = ORDR.\"DocEntry\" WHERE ORDR.\"U_SCGD_Numero_OT\" IS NULL AND ORDR.\"U_SCGD_No_Visita\" IN (SELECT \"U_SCGD_No_Visita\" FROM ORDR ¿#? WHERE ORDR.\"U_SCGD_Numero_OT\" = '{0}') ";
        private const string strLoadLines = " SELECT RDR1.\"ItemCode\", RDR1.\"Dscription\", RDR1.\"Quantity\", RDR1.\"Currency\", RDR1.\"Price\", RDR1.\"FreeTxt\", RDR1.\"DocEntry\", RDR1.\"LineNum\", RDR1.\"DiscPrcnt\", RDR1.\"{2}\", RDR1.\"U_SCGD_Costo\", RDR1.\"TaxCode\", RDR1.\"U_SCGD_CPen\", RDR1.\"U_SCGD_CSol\", RDR1.\"U_SCGD_CRec\", RDR1.\"U_SCGD_CPDe\", RDR1.\"U_SCGD_CPTr\", RDR1.\"U_SCGD_CPBo\", RDR1.\"U_SCGD_Compra\", RDR1.\"U_SCGD_TipArt\" FROM RDR1 ¿#? WHERE RDR1.\"DocEntry\" = '{0}' AND RDR1.\"U_SCGD_Aprobado\" = 1 AND RDR1.\"LineStatus\" = 'O' AND RDR1.\"{2}\" NOT IN ({1}) ";
        private const string strGetCotData = " SELECT \"DocEntry\", \"DocNum\", \"U_SCGD_idSucursal\" FROM \"OQUT\" ¿#? WHERE \"U_SCGD_Numero_OT\" = '{0}' ";


        #endregion

        #region "DocEntry Orden Venta"
        private const string strDocEntryOrdenVenta = "SELECT distinct \"DocEntry\"  FROM \"ORDR\" ¿#? WHERE \"U_SCGD_Numero_OT\" IN ({0})";
        #endregion
    }
}
