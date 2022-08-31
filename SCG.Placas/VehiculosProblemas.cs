using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;


namespace SCG.Placas
{
    public partial class VehiculosProblemas
    {
        public void ButtonSBOImprimirReporteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                string problemaVehiculo = ComboBoxProblema.ObtieneValorUserDataSource();

                if(string.IsNullOrEmpty(problemaVehiculo))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaProblema,BoMessageTime.bmt_Short,BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                string problemaVehiculo = ComboBoxProblema.ObtieneValorUserDataSource();

                SAPbobsCOM.Company m_oCompany = (Company)CompanySBO;

                string direccionR = DireccionReportes + My.Resources.Resource.NombreRptVehiculosProblemas + ".rpt";

                string parametros = problemaVehiculo;

                General.ImprimirReporte(m_oCompany, direccionR, My.Resources.Resource.rptVehiculosProblemas, parametros, UsuarioBD,
                                                       ContraseñaBD);
            }
        }

    }
}
