using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector
{
    public partial class Queries
    {
        #region "Carga Addon"
        private const string strCMDS = "SELECT \"Code\", \"Canceled\" FROM \"@SCGD_CDMS\" ¿#? WHERE \"Code\" IN (1,2,3,4)";

        private const string strSucursalesOT = "SELECT SUC.\"Name\", SUC.\"Code\" FROM \"@SCGD_SUCURSALES\" SUC ¿#? INNER JOIN OUSR SR ¿#? ON SUC.\"Code\" = SR.\"Branch\" WHERE SR.\"USER_CODE\" = '{0}'";
        private const string strSucursalesOTMult = " SELECT TOP 1 usr.\"BPLId\" AS \"Code\", br.\"BPLName\" AS \"Name\" FROM USR6 usr ¿#? INNER JOIN OBPL br ¿#? ON usr.\"BPLId\" = br.\"BPLId\" WHERE \"UserCode\" = '{0}' ";

        #endregion
    }
}
