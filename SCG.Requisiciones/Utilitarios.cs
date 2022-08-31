using System;

namespace SCG.Requisiciones
{
    public class Utilitarios
    {
        public static Boolean ValidarDataTable(ref SAPbouiCOM.Form oform, String strDtName)
        {
            var existeDataTable = false;
            if (oform.DataSources.DataTables.Count > 0)
            {
                for (int i = 0; i < oform.DataSources.DataTables.Count - 1; i++)
                {
                    if (oform.DataSources.DataTables.Item(i).UniqueID == strDtName)
                    {
                        existeDataTable = true;
                    }
                }
            }
            return existeDataTable;
        }

        public static Boolean ValidaUsaOTSap()
        {
            return DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim().Equals("Y"); 
        }

        public static Boolean ValidarSiFormularioAbierto(String strFormUID, Boolean blnselectIfOpen, SAPbouiCOM.Application SBO_Application)
        {
            int intI = 0;
            bool blnFound = false;
            SAPbouiCOM.Form frmForma;

            int a = SBO_Application.Forms.Count;
            var conta = SBO_Application.Forms.Count;
            for (int i = 0; i < SBO_Application.Forms.Count; i++)
            {
                frmForma = SBO_Application.Forms.Item(i);
                if (frmForma.UniqueID == strFormUID)
                {
                    if (blnselectIfOpen)
                    {
                        if (!frmForma.Selected)
                        {
                            SBO_Application.Forms.Item(strFormUID).Select();
                        }
                    }
                    else
                        intI += 1;
                    i = SBO_Application.Forms.Count;
                }
            }
            if (blnFound)
                return true;
            else
                return false;
        }

        public static void DevuelveNombreBDTaller(SAPbouiCOM.Application p_ocompany, String p_strIdSucursal, ref String p_strNombreBDTaller, ref SAPbouiCOM.DataTable dtConsulta)
        {
            var query = "select U_BDSucursal FROM [@SCGD_SUCURSALES] WITH (nolock) where Code = '{0}' ";
            query = string.Format(query, p_strIdSucursal);
            dtConsulta.ExecuteQuery(query);
            if (dtConsulta.Rows.Count > 0)
            {
                p_strNombreBDTaller = dtConsulta.GetValue("U_BDSucursal", 0).ToString().Trim();
            }
        }
    }
}
