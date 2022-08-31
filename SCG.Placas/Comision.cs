using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;

namespace SCG.Placas
{
    public partial class Comision
    {
        public void ButtonSBOImprimirReporteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                string fechaInicio = EditTextFechaInicio.ObtieneValorUserDataSource();
                string fechaFinal = EditTextfechaFinal.ObtieneValorUserDataSource();

                if(string.IsNullOrEmpty(fechaInicio))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFechaInicio, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if(string.IsNullOrEmpty(fechaFinal))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFechaFinal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.ActionSuccess && pval.BeforeAction == false)
            {
                string fechaInicio = EditTextFechaInicio.ObtieneValorUserDataSource();
                string fechaFinal = EditTextfechaFinal.ObtieneValorUserDataSource();

                DateTime dtFechaInicio = DateTime.ParseExact(fechaInicio, "yyyyMMdd",null);
                DateTime dtFechaFinal = DateTime.ParseExact(fechaFinal, "yyyyMMdd", null);

                fechaInicio = dtFechaInicio.ToString("yyyy-MM-dd");
                fechaFinal = dtFechaFinal.ToString("yyyy-MM-dd");

                SAPbobsCOM.Company m_oCompany = (Company)CompanySBO;

                string direccionR = DireccionReportes + My.Resources.Resource.NombreRptComision + ".rpt";

                string parametros = fechaInicio + "," + fechaFinal;

                General.ImprimirReporte(m_oCompany, direccionR, My.Resources.Resource.rptComision, parametros, UsuarioBD,
                                                       ContraseñaBD);

                EditTextFechaInicio.AsignaValorUserDataSource("");
                EditTextfechaFinal.AsignaValorUserDataSource("");
            }
        }
    }
}
