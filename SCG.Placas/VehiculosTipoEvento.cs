using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;

namespace SCG.Placas
{
    public partial class VehiculosTipoEvento 
    {


        public void ComboBoxGestionSelected(ItemEvent pval)
        {
             string codGestion;
            Item sboItem;
            ComboBox sboCombo;
            
            if (pval.BeforeAction==false && pval.ActionSuccess)
            {

                if (pval.ItemUID == ComboBoxGestion.UniqueId)
                {

                    codGestion = ComboBoxGestion.ObtieneValorUserDataSource();

                    sboItem = FormularioSBO.Items.Item("cmbTipEven");
                    sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                    General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}", codGestion), Conexion);

                    ComboBoxEvento.AsignaValorUserDataSource("");
                }
            }
        }

        public void CFLCargaGrupo(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent CFLCargaGrupo = (IChooseFromListEvent)pval;

            DataTable DataTable;

            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;

            if (pval.ActionSuccess)
            {

                if (CFLCargaGrupo.SelectedObjects != null)
                {
                    DataTable = CFLCargaGrupo.SelectedObjects;
                    
                    EditTextNumGrupo.AsignaValorUserDataSource(DataTable.GetValue("DocNum", 0).ToString().Trim());
                }
            }
        }

        public void LimpiarDatos()
        {
            EditTextFechInicio.AsignaValorUserDataSource("");
            EditTextFechaFin.AsignaValorUserDataSource("");
            ComboBoxGestion.AsignaValorUserDataSource("");
            ComboBoxEvento.AsignaValorUserDataSource("");
            EditTextNumGrupo.AsignaValorUserDataSource("");
        }

        public void ButtonSBOImprimirReporteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string fechaInicio = EditTextFechInicio.ObtieneValorUserDataSource();
            string fechaFin = EditTextFechaFin.ObtieneValorUserDataSource();
            string codigoGestion = ComboBoxGestion.ObtieneValorUserDataSource();
            string codigoEvento = ComboBoxEvento.ObtieneValorUserDataSource();
            string numeroGrupo = EditTextNumGrupo.ObtieneValorUserDataSource();
            string tipoReporte = CheckBoxTipo.ObtieneValorUserDataSource();
            string direccionR = "";
            string parametros;
            
            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                if(string.IsNullOrEmpty(codigoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(codigoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                SAPbobsCOM.Company m_oCompany = (Company)CompanySBO;
                
                string descripGestion = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GESTION] where Code = '{0}'", codigoGestion), Conexion);
                string descripEvento = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);
                
                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    DateTime fechaI = DateTime.ParseExact(fechaInicio, "yyyyMMdd", null);
                    fechaInicio = fechaI.ToString("yyyy-MM-dd");
                }

                if (string.IsNullOrEmpty(fechaInicio))
                {
                    fechaInicio = "1900-01-01";
                }
                
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    DateTime fechaF = DateTime.ParseExact(fechaFin, "yyyyMMdd", null);
                    fechaFin = fechaF.ToString("yyyy-MM-dd");
                }

                if (string.IsNullOrEmpty(fechaFin))
                {
                    fechaFin = "2999-01-01";
                }

                if(string.IsNullOrEmpty(codigoGestion))
                {
                    codigoGestion = "-1";
                }

                if(string.IsNullOrEmpty(codigoEvento))
                {
                    codigoEvento = "-1";
                }

                if(string.IsNullOrEmpty(numeroGrupo))
                {
                    numeroGrupo = "-1";
                }

                // Dependiendo del valor del checkbox escoge al tipo de reporte a invocar
                if (tipoReporte.Equals("N") || string.IsNullOrEmpty(tipoReporte))
                {
                    direccionR = DireccionReportes + My.Resources.Resource.NombreRptVehiculosTipoEvento + ".rpt";

                    parametros = fechaInicio + "," + fechaFin + "," + codigoGestion + "," + descripGestion + "," +
                                    codigoEvento + "," + descripEvento + "," + numeroGrupo;

                    General.ImprimirReporte(m_oCompany, direccionR, My.Resources.Resource.rptVehiculosTipoEvento, parametros, UsuarioBD,
                                                            ContraseñaBD);
                }

                else if (tipoReporte.Equals("Y"))
                {
                    string seguimiento = General.EjecutarConsulta(
                        string.Format("select U_Seguimiento from [@SCGD_GESTION] as GES where GES.Code = '{0}'",
                                      codigoGestion), Conexion);

                    //********************************************************

                    if (seguimiento.Equals("1")) //Indica que el tipo de seguimiento es de RTV por lo que se llama al reporte para estos eventos
                    {
                        direccionR = DireccionReportes + My.Resources.Resource.NombreRptVehiculosTipoEventoComplemento1 + ".rpt";
                    }

                    else if (seguimiento.Equals("2")) //Indica que el tipo de seguimiento es de Documentos Legales por lo que se llama al reporte para estos eventos
                    {
                        direccionR = DireccionReportes + My.Resources.Resource.NombreRptVehiculosTipoEventoComplemento2 + ".rpt";
                    }

                    else if (seguimiento.Equals("3")) //Indica que el tipo de seguimiento es de Inscripcion por lo que se llama al reporte para estos eventos
                    {
                        direccionR = DireccionReportes + My.Resources.Resource.NombreRptVehiculosTipoEventoComplemento3 + ".rpt";
                    }

                    //*******************************************************+

                    parametros = fechaInicio + "," + fechaFin + "," + codigoGestion + "," + descripGestion + "," +
                                    codigoEvento + "," + descripEvento + "," + numeroGrupo;

                    General.ImprimirReporte(m_oCompany, direccionR, My.Resources.Resource.rptVehiculosTipoEventoComplemento, parametros, UsuarioBD,
                                                            ContraseñaBD);
                }

                LimpiarDatos();
            }
        }
    }
}
