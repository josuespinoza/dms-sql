using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;

namespace SCG.Placas
{
    public partial class ContratoTraspaso
    {
        public void CFLCargaContratoVenta(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent CFLContratoVenta = (IChooseFromListEvent)pval;

            DataTable DataTable;

            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;

            if (pval.ActionSuccess)
            {

                if (CFLContratoVenta.SelectedObjects != null)
                {
                    DataTable = CFLContratoVenta.SelectedObjects;

                    EditTextContratoV.AsignaValorUserDataSource(DataTable.GetValue("DocNum", 0).ToString().Trim());
                }
            }
        }

        public void ButtonSBOImprimirReporteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            

            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                string numCV = EditTextContratoV.ObtieneValorUserDataSource();

                if(string.IsNullOrEmpty(numCV))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorRptTraspasoContratoNumCV, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                string numCV = EditTextContratoV.ObtieneValorUserDataSource();

                string precios = "";

                string resultado =
                    General.EjecutarConsultaMultipleResultadosFilasColumnas(
                        string.Format(
                            "select VXC.U_Pre_Vta, lower(OCRN.ChkName ) as moneda from [@SCGD_CVENTA] as CV inner join [@SCGD_VEHIXCONT] as VXC on CV.DocEntry = VXC.DocEntry inner join OCRN on CV.U_Moneda = OCRN.CurrCode where CV.DocNum  = '{0}'", numCV), Conexion);

                string[] arrayResut = resultado.Split('@');

                int tamanoArrayR = arrayResut.Length;
                
                //For para pasar por parámetro al reporte el detalle de los que costó cada vehículo
                for (int i = 0; i <= tamanoArrayR - 1; i++)
                {
                    string[] arrayLine = arrayResut[i].Split('*');
                    int tammanoArrayL = arrayLine.Length;

                    for (int j = 0; j <= tammanoArrayL - 1; j++)
                    {
                        decimal x;
                        bool isNumero = decimal.TryParse(arrayLine[j], out x);

                        if (isNumero)
                        {
                            precios = precios + NumeroALetras(arrayLine[j]) + " ";
                        }

                        else if (!string.IsNullOrEmpty(arrayLine[j]))
                        {
                            precios = precios + "de " + arrayLine[j] + " del vehículo ";
                        }
                    }

                    if (!string.IsNullOrEmpty(arrayResut[i]))
                    {
                        precios = precios + NumeroALetras((i + 1).ToString());

                        if ((i + 2) != tamanoArrayR)
                        {
                            precios = precios + " y ";
                        }
                    }
                }

                
                SAPbobsCOM.Company m_oCompany = (Company)CompanySBO;

                string direccionR = DireccionReportes + My.Resources.Resource.NombreRptTraspasoContrato + ".rpt";

                string parametros = numCV + "," + precios;

                General.ImprimirReporte(m_oCompany, direccionR, My.Resources.Resource.rptContratoTraspaso, parametros, UsuarioBD,
                                                       ContraseñaBD);

                EditTextContratoV.AsignaValorUserDataSource("");
            }
        }

        public static string NumeroALetras(string num)
        {

            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }

            catch
            {
                return "";
            }



            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

            if (decimales > 0)
            {
                dec = " con " + decimales.ToString() + "/100";
            }

            res = toText(Convert.ToDouble(entero)) + dec;

            return res;
        }

        private static string toText(double value)
        {
            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "cero";

            else if (value == 1) Num2Text = "uno";

            else if (value == 2) Num2Text = "dos";

            else if (value == 3) Num2Text = "tres";

            else if (value == 4) Num2Text = "cuatro";

            else if (value == 5) Num2Text = "cinco";

            else if (value == 6) Num2Text = "seis";

            else if (value == 7) Num2Text = "siete";

            else if (value == 8) Num2Text = "ocho";

            else if (value == 9) Num2Text = "nueve";

            else if (value == 10) Num2Text = "diez";

            else if (value == 11) Num2Text = "once";

            else if (value == 12) Num2Text = "doce";

            else if (value == 13) Num2Text = "trece";

            else if (value == 14) Num2Text = "catorce";

            else if (value == 15) Num2Text = "quince";

            else if (value < 20) Num2Text = "dieci" + toText(value - 10);

            else if (value == 20) Num2Text = "veinte";

            else if (value < 30) Num2Text = "veinti" + toText(value - 20);

            else if (value == 30) Num2Text = "treinta";

            else if (value == 40) Num2Text = "cuarenta";

            else if (value == 50) Num2Text = "cincuenta";

            else if (value == 60) Num2Text = "sesenta";

            else if (value == 70) Num2Text = "setenta";

            else if (value == 80) Num2Text = "ochenta";

            else if (value == 90) Num2Text = "noventa";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " y " + toText(value % 10);

            else if (value == 100) Num2Text = "cien";

            else if (value < 200) Num2Text = "ciento " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "cientos";

            else if (value == 500) Num2Text = "quinientos";

            else if (value == 700) Num2Text = "setecientos";

            else if (value == 900) Num2Text = "novecientos";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "mil";

            else if (value < 2000) Num2Text = "mil " + toText(value % 1000);

            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " mil";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "un millon";

            else if (value < 2000000) Num2Text = "un millon " + toText(value % 1000000);

            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " millones ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "un billon";

            else if (value < 2000000000000) Num2Text = "un billon " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " billones";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }

            return Num2Text;

        }
    }
}
