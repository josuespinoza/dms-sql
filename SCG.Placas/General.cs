using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using System.Data;

namespace SCG.Placas
{
    public class General
    {

        public static string EjecutarConsulta(string p_strConsulta, string strConectionString)
        {
            return DMS_Connector.Helpers.EjecutarConsulta(p_strConsulta);
        }

        public static string EjecutarConsultaMultipleResultados(string p_strConsulta, string strConectionString)
        {
            string strValor = "";
            foreach (DataRow drRow in DMS_Connector.Helpers.EjecutarConsultaDataTable(p_strConsulta).Rows)
            {
                if (drRow[0] != DBNull.Value)
                    strValor = string.Format("{0}*{1}", strValor, drRow[0]);
            }
            return strValor;
        }

        public static string EjecutarConsultaMultipleResultadosFilasColumnas(string p_strConsulta, string strConectionString)
        {

            string strValor = "";
            System.Data.DataTable dtConsulta;
            dtConsulta = DMS_Connector.Helpers.EjecutarConsultaDataTable(p_strConsulta);
            foreach (DataRow drRow in dtConsulta.Rows)
            {
                for (int index = 0; index <= dtConsulta.Columns.Count - 1; index++)
                    strValor = string.Format("{0}*{1}", strValor, drRow[index]);
                strValor = string.Format("{0}{1}", strValor, "@");
            }
            return strValor;
        }


        public static void CargarValidValuesEnCombos(ValidValues oValidValues, string strQuery, string strConectionString)
        {
            try
            {
                if (oValidValues.Count > 0)
                {
                    for (int i = 0; i <= oValidValues.Count - 1; i++)
                        oValidValues.Remove(oValidValues.Item(0).Value);
                }
                foreach (DataRow drRow in DMS_Connector.Helpers.EjecutarConsultaDataTable(strQuery).Rows)
                    if (drRow[0] != DBNull.Value && drRow[1] != DBNull.Value)
                        oValidValues.Add(drRow[0].ToString().Trim(), drRow[1].ToString().Trim());
            }
            catch (Exception)
            {
            }

        }

        public static string[] ObtenerSeguridadEventos(IApplication ApplicationSBO, string Conexion)
        {
            string usuarioSBO;
            string resultado;
            string[] resultados = new string[] { };

            usuarioSBO = ApplicationSBO.Company.UserName;
            
            resultado = EjecutarConsultaMultipleResultados(string.Format("Select U_Seguimiento from [@SCGD_SEG_EVENTOS] where U_Usuario = '{0}'", usuarioSBO), Conexion);

            int tamanoConsulta = resultado.Length;
            if (tamanoConsulta > 1)
            {
                resultado = resultado.Substring(1, tamanoConsulta - 1);
                resultados = resultado.Split('*');
            }
            return resultados;
        }

        public static string ObtenerSeguridadGastos(IApplication ApplicationSBO, string Conexion)
        {
            string usuarioSBO;
            string resultado;

            usuarioSBO = ApplicationSBO.Company.UserName;
            
            resultado = EjecutarConsulta(string.Format("Select Count(*) from [@SCGD_SEG_GASTOS] where U_Usuario = '{0}'", usuarioSBO), Conexion);

            return resultado;
        }

        public static void ImprimirReporte(SAPbobsCOM.Company company, string direccionReporte, string barraTitulo, string parametros, string usuarioBD, string contraseñaBD)
        {
            string pathExe;
            string parametrosExe;

            if (string.IsNullOrEmpty(barraTitulo))
            {
                barraTitulo = My.Resources.Resource.rptReporte;
            }

            barraTitulo = barraTitulo.Replace(" ", "°");
            direccionReporte = direccionReporte.Replace(" ", "°");
            parametros = parametros.Replace(" ", "°");

            pathExe = Directory.GetCurrentDirectory() + "\\SCG Visualizador de Reportes.exe";

            parametrosExe = barraTitulo + " " + direccionReporte + " " + usuarioBD + "," + contraseñaBD + "," +
                          company.Server + "," + company.CompanyDB + " " + parametros;

            ProcessStartInfo startInfo = new ProcessStartInfo(pathExe) { WindowStyle = ProcessWindowStyle.Maximized, Arguments = parametrosExe };

            Process.Start(startInfo);
        }

        public static void ObtenerSeparadoresNumerosSAP(ref string separardorMilesSAP, ref string separadorDecimalesSAP, string strConectionString)
        {
            
            try
            {
                DMS_Connector.Helpers.GetSeparadoresSAP(ref separardorMilesSAP, ref separadorDecimalesSAP);
            }
            catch (Exception)
            {
            }
        }

        public static string CambiarValoresACultureActual(string valorNumero, string separardorMilesSAP, string separadorDecimalesSAP)
        {
            string sepDecSAP = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (!separadorDecimalesSAP.Equals(sepDecSAP))
            {
                valorNumero = valorNumero.Replace(sepDecSAP, separadorDecimalesSAP);
                if (string.IsNullOrEmpty(valorNumero))
                    valorNumero = "0";
            }

            return valorNumero;
        }
    }
}
