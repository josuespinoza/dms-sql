using System;
using System.Collections.Generic;
using System.Globalization;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ChooseFromList = SAPbouiCOM.ChooseFromList;
using ICompany = SAPbobsCOM.ICompany;
using Company = SAPbobsCOM.Company;

namespace SCG.Placas
{
    public partial class ExpedienteFormulario : IFormularioSBO, IUsaMenu
    {

        protected void DataLoadEvent(BusinessObjectInfo businessObjectInfo, ref bool bubbleEvent)
        {
            if (!businessObjectInfo.BeforeAction && businessObjectInfo.ActionSuccess)
            {

            }
        }

        public void CFLCliente(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent oCFLEvento = (SAPbouiCOM.IChooseFromListEvent) pval;
            string sCFL_ID = oCFLEvento.ChooseFromListUID;
            ChooseFromList oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID);

            SAPbouiCOM.DataTable oDataTable;

            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;

            if (pval.ActionSuccess)
            {
                
                if (oCFLEvento.SelectedObjects != null)
                {

                    oDataTable = oCFLEvento.SelectedObjects;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Cod_Clie", 0, oDataTable.GetValue("CardCode", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Nom_Clie", 0, oDataTable.GetValue("CardName", 0).ToString());

                }

            }
            else if (pval.BeforeAction)
            {

                oConditions = (Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                oCondition = oConditions.Add();

                oCondition.BracketOpenNum = 1;
                oCondition.Alias = "CardType";
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCondition.CondVal = "C";
                oCondition.BracketCloseNum = 1;
                oCFL.SetConditions(oConditions);

            }

        }

        public void CFLUnidad(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pval;
            string sCFL_ID = oCFLEvento.ChooseFromListUID;
            ChooseFromList oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID);

            SAPbouiCOM.DataTable oDataTable;

            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;

            string strTipoVendido;
            string strDispVendido;
            string strCliente;

            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;
            
            if (pval.ActionSuccess)
            {

                if (oCFLEvento.SelectedObjects != null)
                {

                    oDataTable = oCFLEvento.SelectedObjects;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Unid", 0, oDataTable.GetValue("U_Cod_Unid", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_VIN", 0, oDataTable.GetValue("U_Num_VIN", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Moto", 0, oDataTable.GetValue("U_Num_Mot", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Color", 0, oDataTable.GetValue("U_Des_Col", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Marca", 0, oDataTable.GetValue("U_Des_Marc", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Estilo", 0, oDataTable.GetValue("U_Des_Esti", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Modelo", 0, oDataTable.GetValue("U_Des_Mode", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, oDataTable.GetValue("U_CTOVTA", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, oDataTable.GetValue("U_NUMFAC", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Anno", 0, oDataTable.GetValue("U_Ano_Vehi", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Cod_Clie", 0, oDataTable.GetValue("U_CardCode", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Nom_Clie", 0, oDataTable.GetValue("U_CardName", 0).ToString());
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Placa", 0, oDataTable.GetValue("U_Num_Plac", 0).ToString());

                    string numCV = oDataTable.GetValue("U_CTOVTA", 0).ToString().Trim();
                    string descripcionSucursal = General.EjecutarConsulta(string.Format("select U_SlpName from [@SCGD_CVENTA] where DocNum = '{0}'", numCV), Conexion);

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Sucurs", 0, descripcionSucursal);
                }

            }
            else if (pval.BeforeAction)
            {

                strTipoVendido = General.EjecutarConsulta("Select U_Inven_V from [@SCGD_ADMIN] where Code = 'DMS'", Conexion);

                strDispVendido = General.EjecutarConsulta("Select U_Disp_V from [@SCGD_ADMIN] where Code = 'DMS'", Conexion);

                strCliente = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").GetValue("U_Cod_Clie", 0);
                strCliente = strCliente.Trim();

                oConditions = (Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                //oCondition = oConditions.Add();
                //oCondition.BracketOpenNum = 1;
                //oCondition.Alias = "U_Tipo";
                //oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                //oCondition.CondVal = strTipoVendido;
                //oCondition.BracketCloseNum = 1;
                //oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                //oCondition = oConditions.Add();
                //oCondition.BracketOpenNum = 2;
                //oCondition.Alias = "U_Dispo";
                //oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                //oCondition.CondVal = strDispVendido;
                //oCondition.BracketCloseNum = 2;

                oCondition = oConditions.Add();
                oCondition.BracketOpenNum = 1;
                oCondition.Alias = "U_Dispo";
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL;
                oCondition.BracketCloseNum = 1;
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                oCondition = oConditions.Add();
                oCondition.BracketOpenNum = 2;
                oCondition.Alias = "U_Cod_Unid";
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL;
                oCondition.BracketCloseNum = 2;

                if (!string.IsNullOrEmpty(strCliente))
                {

                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;
                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 3;
                    oCondition.Alias = "U_CardCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = strCliente;
                    oCondition.BracketCloseNum = 3;

                }

                oCFL.SetConditions(oConditions);

            }
        }

        public void CFLEnteFinanciero(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pval;
            string sCFL_ID = oCFLEvento.ChooseFromListUID;
            ChooseFromList oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID);

            SAPbouiCOM.DataTable oDataTable;

            if (pval.ActionSuccess)
            {

                if (oCFLEvento.SelectedObjects != null)
                {

                    oDataTable = oCFLEvento.SelectedObjects;

                    EditTextEnteFinanciero.AsignaValorUserDataSource(oDataTable.GetValue("BankName", 0).ToString());

                }

            }

        }

        public void ComboBoxGestionSelected(ItemEvent pval)
        {

            string codGestion;
            Item sboItem;
            ComboBox sboCombo;
            
            if (pval.BeforeAction==false && pval.ActionSuccess)
            {

                if (pval.ItemUID == ComboBoxGestionRV.UniqueId)
                {

                    codGestion = ComboBoxGestionRV.ObtieneValorUserDataSource();

                    if (!string.IsNullOrEmpty(codGestion))
                    {
                        sboItem = FormularioSBO.Items.Item("cmbTipEvRV");
                        sboCombo = (SAPbouiCOM.ComboBox) sboItem.Specific;
                        General.CargarValidValuesEnCombos(sboCombo.ValidValues,
                                                          string.Format(
                                                              "Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}",
                                                              codGestion), Conexion);
                    }

                    ComboBoxEventoRV.AsignaValorUserDataSource("");

                }
                else if (pval.ItemUID == ComboBoxGestionDL.UniqueId)
                {

                    codGestion = ComboBoxGestionDL.ObtieneValorUserDataSource();

                    if(!string.IsNullOrEmpty(codGestion))
                    {
                        sboItem = FormularioSBO.Items.Item("cmbTipEvDL");
                        sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                        General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}", codGestion), Conexion);
                    }

                    ComboBoxEventoDL.AsignaValorUserDataSource("");

                }
                else if (pval.ItemUID == ComboBoxGestionIns.UniqueId)
                {
                    codGestion = ComboBoxGestionIns.ObtieneValorUserDataSource();

                    if (!string.IsNullOrEmpty(codGestion))
                    {
                        sboItem = FormularioSBO.Items.Item("cmbTipEvSI");
                        sboCombo = (SAPbouiCOM.ComboBox) sboItem.Specific;
                        General.CargarValidValuesEnCombos(sboCombo.ValidValues,
                                                          string.Format(
                                                              "Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}",
                                                              codGestion), Conexion);
                    }

                    ComboBoxEventoIns.AsignaValorUserDataSource("");

                }
            }
        }

        /**
         * Método que se encarga de cargar la informacion desde cualquier matrix a los campos para su posterior edición
         */
        private void CargarInformaciondesdeMatrix(ItemEvent pval, MatrixSBO matrix, string tablaSeguimiento, string tipoCombo, string codigoGestion, 
            string codigoEvento,string fecha, string numReferencia1, string numReferencia2, string observac)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {

                Item sboItem;
                ComboBox sboCombo;

                int idRegistro = matrix.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                matrix.Matrix.FlushToDataSource();

                if (idRegistro > 0)
                {
                    string idGestion =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(codigoGestion,
                                                                                                idRegistro - 1).Trim();
                    string idEvento =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(codigoEvento,
                                                                                                idRegistro - 1).Trim();
                    string fechaEvento =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(fecha,
                                                                                                idRegistro - 1).Trim();
                    string numRef1 =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(numReferencia1,
                                                                                                idRegistro - 1).Trim();
                    string numRef2 =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(numReferencia2,
                                                                                                idRegistro - 1).Trim();
                    string observ =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(observac,
                                                                                                idRegistro - 1).Trim();
                    //Carga para la pestaña de Revisión Vehicular
                    if(tipoCombo.Equals("cmbTipEvRV"))
                    {
                        if (!string.IsNullOrEmpty(idGestion))
                        {
                            string numRef3 =
                                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Num_Ref3",
                                                                                                        idRegistro - 1).Trim();
                            string numRef4 =
                                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Num_Ref4",
                                                                                                        idRegistro - 1).Trim();
                            string numRef5 =
                                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Num_Ref5",
                                                                                                        idRegistro - 1).Trim();
                            string numRef6 =
                                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Num_Ref6",
                                                                                                        idRegistro - 1).Trim();
                            string fechaIngreso =
                                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Fech_In",
                                                                                                        idRegistro - 1).Trim();

                            ComboBoxGestionRV.AsignaValorUserDataSource(idGestion);

                            sboItem = FormularioSBO.Items.Item(tipoCombo);
                            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                            General.CargarValidValuesEnCombos(sboCombo.ValidValues,
                                                          string.Format(
                                                              "Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}",
                                                              idGestion), Conexion);

                            ComboBoxEventoRV.AsignaValorUserDataSource(idEvento);

                            EditTextFechEventoRV.AsignaValorUserDataSource(fechaEvento);
                            EditTextNoRef1RV.AsignaValorUserDataSource(numRef1);
                            EditTextNoRef2RV.AsignaValorUserDataSource(numRef2);
                            EditTextNoRef3RV.AsignaValorUserDataSource(numRef3);
                            EditTextNoRef4RV.AsignaValorUserDataSource(numRef4);
                            EditTextNoRef5RV.AsignaValorUserDataSource(numRef5);
                            EditTextNoRef6RV.AsignaValorUserDataSource(numRef6);
                            EditTextFechIngresoRV.AsignaValorUserDataSource(fechaIngreso);
                            EditTextObservacionesRV.AsignaValorUserDataSource(observ);
                        }
                    }

                    //Carga para la pestaña de Documentos Legales
                    else if (tipoCombo.Equals("cmbTipEvDL"))
                    {
                        string prenda =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Prenda", idRegistro - 1).Trim();

                        string instFinanciera =
                           FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Ins_Fin", idRegistro - 1).Trim();

                        if (!string.IsNullOrEmpty(idGestion))
                        {
                            ComboBoxGestionDL.AsignaValorUserDataSource(idGestion);

                            sboItem = FormularioSBO.Items.Item(tipoCombo);
                            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                            General.CargarValidValuesEnCombos(sboCombo.ValidValues,
                                                              string.Format(
                                                                  "Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}",
                                                                  idGestion), Conexion);

                            ComboBoxEventoDL.AsignaValorUserDataSource(idEvento);

                            EditTextFechEventoDL.AsignaValorUserDataSource(fechaEvento);
                            EditTextNoRef1DL.AsignaValorUserDataSource(numRef1);
                            EditTextNoRef2DL.AsignaValorUserDataSource(numRef2);
                            CheckBoxPrenda.AsignaValorUserDataSource(prenda);
                            EditTextEnteFinanciero.AsignaValorUserDataSource(instFinanciera);
                            EditTextObservacionesDL.AsignaValorUserDataSource(observ);
                        }
                    }

                    //Carga para la pestaña de Inscripción    
                    else if (tipoCombo.Equals("cmbTipEvSI"))
                    {
                        if (!string.IsNullOrEmpty(idGestion))
                        {
                            ComboBoxGestionIns.AsignaValorUserDataSource(idGestion);

                            sboItem = FormularioSBO.Items.Item(tipoCombo);
                            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                            General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}", idGestion), Conexion);

                            ComboBoxEventoIns.AsignaValorUserDataSource(idEvento);

                            EditTextFechEventoIns.AsignaValorUserDataSource(fechaEvento);
                            EditTextNoRef1Ins.AsignaValorUserDataSource(numRef1);
                            EditTextNoRef2Ins.AsignaValorUserDataSource(numRef2);
                            EditTextObservacionesIns.AsignaValorUserDataSource(observ);
                        }
                        
                    }

                }
            }
        }

        /**
         * Método que se encarga de cargar los datos de la matrix de Gastos a los campos para su futura edición
         */
        public void CargarInformaciondesdeMatrixGasto(ItemEvent pval, MatrixSBO matrix, string tablaSeguimiento, string tipoCombo, string codigoGasto, string numDocumento,
            string fechaDoc, string monto, string observacion)
        {
            if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                int idRegistro = matrix.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                matrix.Matrix.FlushToDataSource();

                if(idRegistro > 0)
                {
                    string idGasto =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(codigoGasto,
                                                                                                idRegistro - 1).Trim();
                    string numeroDoc =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(numDocumento,
                                                                                                idRegistro - 1).Trim();

                    string fecha =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(fechaDoc, 
                                                                                                idRegistro - 1).Trim();

                    string montoG =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(monto, 
                                                                                                idRegistro - 1).Trim();

                    string observ =
                        FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue(observacion,
                                                                                                idRegistro - 1).Trim();

                    if (!string.IsNullOrEmpty(idGasto))
                    {
                        ComboBoxGastoG.AsignaValorUserDataSource(idGasto);

                        EditTextNoDocG.AsignaValorUserDataSource(numeroDoc);
                        EditTextFechDocG.AsignaValorUserDataSource(fecha);
                        EditTextMontoG.AsignaValorUserDataSource(montoG);
                        EditTextObservacionesG.AsignaValorUserDataSource(observ);
                    }

                }

            }
        }

        public void ButtonSBOAgregarRVItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {

            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;

            tipoGestion = ComboBoxGestionRV.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoRV.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoRV.ObtieneValorUserDataSource();
            
            if (pval.BeforeAction && pval.ActionSuccess==false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction==false && pval.ActionSuccess)
            {

                ref1 = EditTextNoRef1RV.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2RV.ObtieneValorUserDataSource();
                observ = EditTextObservacionesRV.ObtieneValorUserDataSource();

                penultimoEventoAgregado = ultimoEventoAgregado;

                ultimoEventoAgregado = tipoEvento;
                panelUltimoEvento = FormularioSBO.PaneLevel;

                AgregarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2,observ, "@SCGD_REV_VEH");

                MatrixRevVehicular.Matrix.LoadFromDataSource();

                ComboBoxGestionRV.AsignaValorUserDataSource("");
                ComboBoxEventoRV.AsignaValorUserDataSource("");

                EditTextFechEventoRV.AsignaValorUserDataSource("");
                EditTextNoRef1RV.AsignaValorUserDataSource("");
                EditTextNoRef2RV.AsignaValorUserDataSource("");
                EditTextNoRef3RV.AsignaValorUserDataSource("");
                EditTextNoRef4RV.AsignaValorUserDataSource("");
                EditTextNoRef5RV.AsignaValorUserDataSource("");
                EditTextNoRef6RV.AsignaValorUserDataSource("");
                EditTextFechIngresoRV.AsignaValorUserDataSource("");
                EditTextObservacionesRV.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode==BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                
            }

        }

        public void ButtonSBOAgregarDLItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {

            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;

            tipoGestion = ComboBoxGestionDL.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoDL.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoDL.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {

                ref1 = EditTextNoRef1DL.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2DL.ObtieneValorUserDataSource();
                observ = EditTextObservacionesDL.ObtieneValorUserDataSource();

                penultimoEventoAgregado = ultimoEventoAgregado;

                ultimoEventoAgregado = tipoEvento;
                panelUltimoEvento = FormularioSBO.PaneLevel;

                AgregarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2, observ, "@SCGD_DOC_LEG");

                MatrixDocLegales.Matrix.LoadFromDataSource();

                ComboBoxGestionDL.AsignaValorUserDataSource("");
                ComboBoxEventoDL.AsignaValorUserDataSource("");

                EditTextFechEventoDL.AsignaValorUserDataSource("");
                EditTextNoRef1DL.AsignaValorUserDataSource("");
                EditTextNoRef2DL.AsignaValorUserDataSource("");
                EditTextObservacionesDL.AsignaValorUserDataSource("");
                CheckBoxPrenda.AsignaValorUserDataSource("N");
                EditTextEnteFinanciero.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }

        }

        public void ButtonSBOAgregarSIItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {

            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;

            tipoGestion = ComboBoxGestionIns.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoIns.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoIns.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {

                ref1 = EditTextNoRef1Ins.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2Ins.ObtieneValorUserDataSource();
                observ = EditTextObservacionesIns.ObtieneValorUserDataSource();

                penultimoEventoAgregado = ultimoEventoAgregado;

                ultimoEventoAgregado = tipoEvento;
                panelUltimoEvento = FormularioSBO.PaneLevel;

                AgregarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2, observ, "@SCGD_INSCRIP");

                MatrixInscripcion.Matrix.LoadFromDataSource();

                string eventoFinalizacion = General.EjecutarConsulta(
                    string.Format("select U_EvenFin from [@SCGD_EVENTO] where Code = '{0}'", tipoEvento), Conexion);

                if (eventoFinalizacion.Equals("Y"))
                {
                    eventoFinal = true;
                }

                ComboBoxGestionIns.AsignaValorUserDataSource("");
                ComboBoxEventoIns.AsignaValorUserDataSource("");

                EditTextFechEventoIns.AsignaValorUserDataSource("");
                EditTextNoRef1Ins.AsignaValorUserDataSource("");
                EditTextNoRef2Ins.AsignaValorUserDataSource("");
                EditTextObservacionesIns.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }

        }

        public void ButtonSBOAgregarGastosItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {

            NumberFormatInfo n;
            string tipoGasto;
            string fechaDocumento;
            string numDoc;
            string strMonto;
            decimal decMonto=0;
            string observ;
            string descGasto;
            string strUsuarioSBO;
            string strFechaCreacion;
            int intNuevoRegistro;
            string validaGasto;
            DateTime fecha;
            decimal decTotal=0;

            n = DIHelper.GetNumberFormatInfo(CompanySBO);

            tipoGasto = ComboBoxGastoG.ObtieneValorUserDataSource();
            fechaDocumento = EditTextFechDocG.ObtieneValorUserDataSource();
            strMonto = EditTextMontoG.ObtieneValorUserDataSource();
            if (!string.IsNullOrEmpty(strMonto))
            {
                decMonto = decimal.Parse(strMonto, n);
            }

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                
                if (string.IsNullOrEmpty(tipoGasto))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaTipoGasto, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaDocumento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (decMonto <= 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorNumeroNegativo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else 
                {
                    int tamannoMG = MatrixGastos.Matrix.RowCount;
                    string codigoGasto = ComboBoxGastoG.ObtieneValorUserDataSource();
                    string codigoGastoDT;
                    
                    for (int i = 0; i <= tamannoMG - 1; i++)
                    {
                        codigoGastoDT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").GetValue("U_Cod_Gas", i).Trim();

                        if (codigoGasto.Equals(codigoGastoDT))
                        {
                            int fila = i + 1;
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExisteGastoMatrix + fila.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            break;
                        }
                    }
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {

                numDoc = EditTextNoDocG.ObtieneValorUserDataSource();
                observ = EditTextObservacionesG.ObtieneValorUserDataSource();

                descGasto = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GASTOS] where Code = {0}", tipoGasto), Conexion);

                fecha = DateTime.ParseExact(fechaDocumento, "yyyyMMdd", null);

                strUsuarioSBO = ApplicationSBO.Company.UserName;
                //strUsuarioSBO = General.EjecutarConsulta(string.Format("SELECT USER_CODE FROM OUSR WHERE USER_CODE = '{0}'", strUsuarioSBO), Conexion);

                strFechaCreacion = DateTime.Now.ToString("yyyyMMdd");

                intNuevoRegistro = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").Size;
                if (intNuevoRegistro == 1)
                {
                    validaGasto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").GetValue("U_Gasto", 0);
                    validaGasto = validaGasto.Trim();
                    if (!string.IsNullOrEmpty(validaGasto))
                    {
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").InsertRecord(intNuevoRegistro);
                        intNuevoRegistro += 1;
                    }
                    else
                    {
                        intNuevoRegistro = 1;
                    }
                }
                else
                {
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").InsertRecord(intNuevoRegistro);
                    intNuevoRegistro += 1;
                }

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Cod_Gas", intNuevoRegistro - 1, tipoGasto);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Gasto", intNuevoRegistro - 1, descGasto);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Num_Doc", intNuevoRegistro - 1, numDoc);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Fech_Doc", intNuevoRegistro - 1, fecha.ToString("yyyyMMdd"));
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Monto", intNuevoRegistro - 1, decMonto.ToString(n));
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Observ", intNuevoRegistro - 1, observ);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Ingresa", intNuevoRegistro - 1, strUsuarioSBO);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").SetValue("U_Fech_Cre", intNuevoRegistro - 1, strFechaCreacion);

                MatrixGastos.Matrix.LoadFromDataSource();

                EditTextNoDocG.AsignaValorUserDataSource("");
                EditTextFechDocG.AsignaValorUserDataSource("");
                EditTextMontoG.AsignaValorUserDataSource("0");
                EditTextObservacionesG.AsignaValorUserDataSource("");

                for (int i = 0; i <= MatrixGastos.Matrix.RowCount-1; i++)
                {

                    strMonto = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").GetValue("U_Monto", i);
                    strMonto = strMonto.Trim();
                    if (!string.IsNullOrEmpty(strMonto))
                    {
                        decMonto = decimal.Parse(strMonto, n);
                        decTotal = decTotal + decMonto;
                    }

                }

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Total",0,decTotal.ToString(n));

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }

        }
        
        public void ButtonSBOEditarRVItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;
            int idRegistro;

            tipoGestion = ComboBoxGestionRV.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoRV.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoRV.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {

                ref1 = EditTextNoRef1RV.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2RV.ObtieneValorUserDataSource();
                observ = EditTextObservacionesRV.ObtieneValorUserDataSource();

                ultimoEventoAgregado = tipoEvento;

                idRegistro = MatrixRevVehicular.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                EditarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2, observ, "@SCGD_REV_VEH", idRegistro);

                MatrixRevVehicular.Matrix.LoadFromDataSource();

                ComboBoxGestionRV.AsignaValorUserDataSource("");
                ComboBoxEventoRV.AsignaValorUserDataSource("");

                EditTextFechEventoRV.AsignaValorUserDataSource("");
                EditTextNoRef1RV.AsignaValorUserDataSource("");
                EditTextNoRef2RV.AsignaValorUserDataSource("");
                EditTextNoRef3RV.AsignaValorUserDataSource("");
                EditTextNoRef4RV.AsignaValorUserDataSource("");
                EditTextNoRef5RV.AsignaValorUserDataSource("");
                EditTextNoRef6RV.AsignaValorUserDataSource("");
                EditTextFechIngresoRV.AsignaValorUserDataSource("");
                EditTextObservacionesRV.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }
        }

        public void ButtonSBOEditarDLItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;
            int idRegistro;

            tipoGestion = ComboBoxGestionDL.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoDL.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoDL.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                idRegistro = MatrixDocLegales.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                ref1 = EditTextNoRef1DL.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2DL.ObtieneValorUserDataSource();
                observ = EditTextObservacionesDL.ObtieneValorUserDataSource();

                ultimoEventoAgregado = tipoEvento;

                EditarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2, observ, "@SCGD_DOC_LEG", idRegistro);

                MatrixDocLegales.Matrix.LoadFromDataSource();

                ComboBoxGestionDL.AsignaValorUserDataSource("");
                ComboBoxEventoDL.AsignaValorUserDataSource("");

                EditTextFechEventoDL.AsignaValorUserDataSource("");
                EditTextNoRef1DL.AsignaValorUserDataSource("");
                EditTextNoRef2DL.AsignaValorUserDataSource("");
                EditTextObservacionesDL.AsignaValorUserDataSource("");
                CheckBoxPrenda.AsignaValorUserDataSource("N");
                EditTextEnteFinanciero.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }
        }

        public void ButtonSBOEditarInsItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string tipoGestion;
            string tipoEvento;
            string fechaEvento;
            string ref1;
            string ref2;
            string observ;
            int idRegistro;

            tipoGestion = ComboBoxGestionIns.ObtieneValorUserDataSource();
            tipoEvento = ComboBoxEventoIns.ObtieneValorUserDataSource();
            fechaEvento = EditTextFechEventoIns.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(tipoGestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(tipoEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaEvento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                idRegistro = MatrixInscripcion.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                ref1 = EditTextNoRef1Ins.ObtieneValorUserDataSource();
                ref2 = EditTextNoRef2Ins.ObtieneValorUserDataSource();
                observ = EditTextObservacionesIns.ObtieneValorUserDataSource();

                ultimoEventoAgregado = tipoEvento;

                EditarDatosMatrizSeguimiento(tipoGestion, tipoEvento, fechaEvento, ref1, ref2, observ, "@SCGD_INSCRIP", idRegistro);

                MatrixInscripcion.Matrix.LoadFromDataSource();

                ComboBoxGestionIns.AsignaValorUserDataSource("");
                ComboBoxEventoIns.AsignaValorUserDataSource("");

                EditTextFechEventoIns.AsignaValorUserDataSource("");
                EditTextNoRef1Ins.AsignaValorUserDataSource("");
                EditTextNoRef2Ins.AsignaValorUserDataSource("");
                EditTextObservacionesIns.AsignaValorUserDataSource("");

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }
        }

        /**
        * Método que se encarga de realizar la función para editar cuando se preciona el respectivo botón
        */
        public void ButtonSBOEditarGItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            NumberFormatInfo n;
            string tipoGasto;
            string fechaDocumento;
            string numDoc;
            string strMonto;
            decimal decMonto = 0;
            string observ;
            int idRegistro;

            n = DIHelper.GetNumberFormatInfo(CompanySBO);

            tipoGasto = ComboBoxGastoG.ObtieneValorUserDataSource();
            fechaDocumento = EditTextFechDocG.ObtieneValorUserDataSource();
            strMonto = EditTextMontoG.ObtieneValorUserDataSource();

            if (!string.IsNullOrEmpty(strMonto))
            {
                decMonto = decimal.Parse(strMonto, n);
            }

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {

                if (string.IsNullOrEmpty(tipoGasto))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaTipoGasto, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (string.IsNullOrEmpty(fechaDocumento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (decMonto <= 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorNumeroNegativo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else
                {
                    idRegistro = MatrixGastos.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                    int tamannoMG = MatrixGastos.Matrix.RowCount;
                    string codigoGasto = ComboBoxGastoG.ObtieneValorUserDataSource();
                    string codigoGastoDT;

                    for (int i = 0; i <= tamannoMG - 1; i++)
                    {
                        codigoGastoDT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").GetValue("U_Cod_Gas", i).Trim();

                        if (codigoGasto.Equals(codigoGastoDT))
                        {
                            if(idRegistro != i + 1 )
                            {
                                int fila = i + 1;
                                BubbleEvent = false;
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExisteGastoMatrix + fila.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                break;
                            }
                            
                        }
                    }
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                idRegistro = MatrixGastos.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder); 

                numDoc = EditTextNoDocG.ObtieneValorUserDataSource();
                observ = EditTextObservacionesG.ObtieneValorUserDataSource();
                EditarDatosMatrizSeguimientoGasto(tipoGasto, fechaDocumento, numDoc, decMonto, observ, "@SCGD_GAS_INS",
                                                  n, idRegistro);

                if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                }

            }
        }

        public void ButtonSBOBorrarRVItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                int idRegistro = MatrixRevVehicular.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                
                if(idRegistro > 0)
                {
                    MatrixRevVehicular.Matrix.FlushToDataSource();

                    if (idRegistro == MatrixRevVehicular.Matrix.RowCount && panelUltimoEvento == FormularioSBO.PaneLevel)
                    {
                        ultimoEventoAgregado = penultimoEventoAgregado;
                    }

                    BorrarDatosMatrizSeguimiento("@SCGD_REV_VEH", "U_Num_Ref1", idRegistro);
                    MatrixRevVehicular.Matrix.LoadFromDataSource();

                    ComboBoxGestionRV.AsignaValorUserDataSource("");
                    ComboBoxEventoRV.AsignaValorUserDataSource("");

                    EditTextFechEventoRV.AsignaValorUserDataSource("");
                    EditTextNoRef1RV.AsignaValorUserDataSource("");
                    EditTextNoRef2RV.AsignaValorUserDataSource("");
                    EditTextNoRef3RV.AsignaValorUserDataSource("");
                    EditTextNoRef4RV.AsignaValorUserDataSource("");
                    EditTextNoRef5RV.AsignaValorUserDataSource("");
                    EditTextNoRef6RV.AsignaValorUserDataSource("");
                    EditTextFechIngresoRV.AsignaValorUserDataSource("");
                    EditTextObservacionesRV.AsignaValorUserDataSource("");

                    if (MatrixRevVehicular.Matrix.RowCount == 0)
                    {
                        penultimoEventoAgregado = "";
                    }

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                    {
                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                } 
            }
        }

        public void ButtonSBOBorrarDLItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int idRegistro = MatrixDocLegales.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                if (idRegistro > 0)
                {
                    MatrixDocLegales.Matrix.FlushToDataSource();

                    if (idRegistro == MatrixDocLegales.Matrix.RowCount && panelUltimoEvento == FormularioSBO.PaneLevel)
                        ultimoEventoAgregado = penultimoEventoAgregado;

                    BorrarDatosMatrizSeguimiento("@SCGD_DOC_LEG", "U_Num_Ref1", idRegistro);
                    MatrixDocLegales.Matrix.LoadFromDataSource();

                    ComboBoxGestionDL.AsignaValorUserDataSource("");
                    ComboBoxEventoDL.AsignaValorUserDataSource("");

                    EditTextFechEventoDL.AsignaValorUserDataSource("");
                    EditTextNoRef1DL.AsignaValorUserDataSource("");
                    EditTextNoRef2DL.AsignaValorUserDataSource("");
                    EditTextObservacionesDL.AsignaValorUserDataSource("");
                    CheckBoxPrenda.AsignaValorUserDataSource("N");
                    EditTextEnteFinanciero.AsignaValorUserDataSource("");

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                    {
                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                }
            }
        }

        public void ButtonSBOBorrarInsItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
             if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int idRegistro = MatrixInscripcion.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                if (idRegistro > 0)
                {
                    MatrixInscripcion.Matrix.FlushToDataSource();

                    if (idRegistro == MatrixInscripcion.Matrix.RowCount && panelUltimoEvento == FormularioSBO.PaneLevel)
                        ultimoEventoAgregado = penultimoEventoAgregado;

                    BorrarDatosMatrizSeguimiento("@SCGD_INSCRIP", "U_Num_Ref1", idRegistro);

                    MatrixInscripcion.Matrix.LoadFromDataSource();

                    ComboBoxGestionIns.AsignaValorUserDataSource("");
                    ComboBoxEventoIns.AsignaValorUserDataSource("");

                    EditTextFechEventoIns.AsignaValorUserDataSource("");
                    EditTextNoRef1Ins.AsignaValorUserDataSource("");
                    EditTextNoRef2Ins.AsignaValorUserDataSource("");
                    EditTextObservacionesIns.AsignaValorUserDataSource("");

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                    {
                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                }
            }
        }

        public void ButtonSBOBorrarGItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                NumberFormatInfo n;
                n = DIHelper.GetNumberFormatInfo(CompanySBO);
                decimal monto;
                decimal total;
                

                int idRegistro = MatrixGastos.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                if(idRegistro > 0)
                {
                    MatrixGastos.Matrix.FlushToDataSource();

                    string montoDSG = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_GAS_INS").GetValue("U_Monto",idRegistro - 1).Trim();
                    string totalDSP = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").GetValue("U_Total", 0);
                    
                    monto = decimal.Parse(montoDSG,n);
                    total = decimal.Parse(totalDSP,n);

                    total = total - monto;

                    BorrarDatosMatrizSeguimiento("@SCGD_GAS_INS", "U_Observ", idRegistro);

                    MatrixGastos.Matrix.LoadFromDataSource();

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Total", 0,total.ToString(n));

                    ComboBoxGastoG.AsignaValorUserDataSource("");

                    EditTextNoDocG.AsignaValorUserDataSource("");
                    EditTextFechDocG.AsignaValorUserDataSource("");
                    EditTextMontoG.AsignaValorUserDataSource("0");
                    EditTextObservacionesG.AsignaValorUserDataSource("");

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                    {
                        FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                }
            }
        }

        public void ButtonSBOCrearExpedienteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                unidadExpediente = EditTextUnidad.ObtieneValorDataSource();

                string finalizado =
                    General.EjecutarConsulta(
                        string.Format(
                            "select TOP(1) U_Finaliz  from [@SCGD_PLACA] where U_Num_Unid = '{0}' order by DocEntry desc",unidadExpediente),
                        Conexion);

                string docEntryE = General.EjecutarConsulta(string.Format("select TOP(1) DocEntry from [@SCGD_PLACA] where U_Num_Unid= '{0}' order by DocEntry desc", unidadExpediente), Conexion);
                
                int tamannoMRV = MatrixRevVehicular.Matrix.RowCount;
                int tamannoMDL = MatrixDocLegales.Matrix.RowCount;
                int tamannoMIns = MatrixInscripcion.Matrix.RowCount;
                int tamannoMG = MatrixGastos.Matrix.RowCount;

                if (FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE || FormularioSBO.Mode == BoFormMode.fm_ADD_MODE || FormularioSBO.Mode == BoFormMode.fm_OK_MODE)
                {
                    if (string.IsNullOrEmpty(unidadExpediente))
                    {
                        BubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionUnidad, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }

                    else if (!string.IsNullOrEmpty(docEntryE) && FormularioSBO.Mode == BoFormMode.fm_ADD_MODE)
                    {
                        if (finalizado.Equals("N"))
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExisteExpediente + docEntryE, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }

                    else if (FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        if (finalizado.Equals("Y"))
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExpedienteFinalizado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }

                    else if (tamannoMRV == 0 && tamannoMDL == 0 && tamannoMIns == 0 && tamannoMG == 0)
                    {
                        BubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExpedienteFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }

                    if (eventoFinal)
                    {
                        CheckBoxFinalizado.AsignaValorDataSource("Y");
                    }

                    else if (eventoFinal == false)
                    {
                        CheckBoxFinalizado.AsignaValorDataSource("N");
                    }

                    eventoFinal = false;
                }
            }

            else if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                string numPlaca = EditTextPlaca.ObtieneValorDataSource();

                actualizarEstadoVehiculo(unidadExpediente, ultimoEventoAgregado);
                actualizarPlacaVehiculo(unidadExpediente, numPlaca);
                unidadExpediente = "";
            }
        }


        public void ButtonSBOImprimirRepItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string expediente = "";

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                expediente = EditTextCodigo.ObtieneValorDataSource();

                if(string.IsNullOrEmpty(expediente))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarExpediente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int idRegistro = MatrixReportes.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                if (idRegistro > 0)
                {
                    Company m_oCompany = (Company)CompanySBO;

                    MatrixReportes.Matrix.FlushToDataSource();

                    string codeReporte = DataTableReportes.GetValue("codeR", idRegistro - 1).ToString().Trim();
                    string nombreReporte = DataTableReportes.GetValue("nameR", idRegistro - 1).ToString().Trim();

                    string resultadoConsulta = General.EjecutarConsultaMultipleResultados(string.Format("Select U_Usa_Exp from [@SCGD_RPT_PLACAS] where Code = '{0}'", codeReporte), Conexion);

                    string titulo = "";

                    if (codeReporte.Equals("1"))
                        titulo = My.Resources.Resource.rptAcuerdoEntregaPlacaAGV;

                    else if (codeReporte.Equals("2"))
                        titulo = My.Resources.Resource.rptAcuerdoDejarVehiculoAgencia;

                    else if (codeReporte.Equals("3"))
                        titulo = My.Resources.Resource.rptExpedientePlacas;

                    else if (codeReporte.Equals("4"))
                        titulo = My.Resources.Resource.rptPortadaAGV;

                    else if (codeReporte.Equals("5"))
                        titulo = My.Resources.Resource.rptEntregaPlaca;

                    else if (codeReporte.Equals("6"))
                        titulo = My.Resources.Resource.rptEntregaEscritura;

                    if (resultadoConsulta.Length > 1)
                    {
                        int longitudConsulta = resultadoConsulta.Length;
                        resultadoConsulta = resultadoConsulta.Substring(1, longitudConsulta - 1);
                        string[] resultado = resultadoConsulta.Split('*');

                        for (int i = 0; i <= resultado.Length - 1; i++)
                        {
                            //Si se agrega nuevan nuevas configuraciones para utilizar diferentes valores para la carga del reporte se podria realizar dentro del For únicamente obteniendo el valor correspondiente dentro del array y modificando la consulta anteriormente escrita

                            //index que deacuerdo a la consulta representa si usa expediente 
                            if (i == 0 && resultado[i].Equals("Y"))
                            {
                                expediente = EditTextCodigo.ObtieneValorDataSource();

                                string direccionR = DireccionReportes + nombreReporte + ".rpt";

                                General.ImprimirReporte(m_oCompany, direccionR, titulo, expediente, UsuarioBD,
                                                        ContraseñaBD);
                            }
                        }
                    }
                }

                else
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionarReporte, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
        }

        public void ButtonSBOActualizarExpedienteItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                string numUnidad = EditTextUnidad.ObtieneValorDataSource();

                if(!string.IsNullOrEmpty(numUnidad))
                {
                    string resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("select top 1 VEH.U_Num_VIN, VEH.U_Num_Mot, VEH.U_Num_Plac, VEH.U_Des_Col, VEH.U_Des_Marc, VEH.U_Des_Esti, VEH.U_Des_Mode, VEH.U_CTOVTA, VEH.U_NUMFAC, VEH.U_Ano_Vehi, CV.U_CCl_Veh, OC.CardName,CV.DocNum  from [@SCGD_CVENTA] as CV left outer join [@SCGD_VEHIXCONT] as VXC on CV.DocEntry = VXC.DocEntry inner join [@SCGD_VEHICULO] as VEH on VXC.U_Cod_Unid = VEH.U_Cod_Unid left outer join OCRD as OC on CV.U_CCl_Veh = OC.CardCode where VEH.U_Cod_Unid = '{0}' and CV.U_Reversa = 'N'  order by DocNum desc", numUnidad), Conexion);

                    if (!string.IsNullOrEmpty(resultado))
                    {
                        string[] resultadoArray = resultado.Split('@');

                        string[] parametros = resultadoArray[0].Split('*');

                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_VIN", 0, parametros[1]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Moto", 0, parametros[2]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Placa", 0, parametros[3]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Color", 0, parametros[4]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Marca", 0, parametros[5]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Estilo", 0, parametros[6]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Modelo", 0, parametros[7]);

                        if (string.IsNullOrEmpty(parametros[8]))
                        {
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, "0");
                        }

                        if (!string.IsNullOrEmpty(parametros[8]))
                        {
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, parametros[8]);
                        }

                        if (string.IsNullOrEmpty(parametros[9]))
                        {
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, "0");
                        }

                        if (!string.IsNullOrEmpty(parametros[9]))
                        {
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, parametros[9]);
                        }

                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Anno", 0, parametros[10]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Cod_Clie", 0, parametros[11]);
                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Nom_Clie", 0, parametros[12]);

                        string descripcionSucursal = General.EjecutarConsulta(string.Format("select U_SlpName from [@SCGD_CVENTA] where DocNum = '{0}'", parametros[8]), Conexion);

                        FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Sucurs", 0, descripcionSucursal);
                    }
                    else
                    {
                        resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("select VEH.U_Num_VIN, VEH.U_Num_Mot, VEH.U_Num_Plac, VEH.U_Des_Col, VEH.U_Des_Marc, VEH.U_Des_Esti, VEH.U_Des_Mode, VEH.U_CTOVTA, VEH.U_NUMFAC, VEH.U_Ano_Vehi, VEH.U_CardCode, OC.CardName  from [@SCGD_VEHICULO] as VEH left outer join OCRD as OC on VEH.U_CardCode = OC.CardCode where U_Cod_Unid = '{0}'", numUnidad), Conexion);

                        if (!string.IsNullOrEmpty(resultado))
                        {
                            string[] resultadoArray = resultado.Split('@');

                            string[] parametros = resultadoArray[0].Split('*');

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_VIN", 0, parametros[1]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Moto", 0, parametros[2]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Placa", 0, parametros[3]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Color", 0, parametros[4]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Marca", 0, parametros[5]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Estilo", 0, parametros[6]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Modelo", 0, parametros[7]);

                            if (string.IsNullOrEmpty(parametros[8]))
                            {
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, "0");
                            }

                            if (!string.IsNullOrEmpty(parametros[8]))
                            {
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, parametros[8]);
                            }

                            if (string.IsNullOrEmpty(parametros[9]))
                            {
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, "0");
                            }

                            if (!string.IsNullOrEmpty(parametros[9]))
                            {
                                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, parametros[9]);
                            }

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Anno", 0, parametros[10]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Cod_Clie", 0, parametros[11]);
                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Nom_Clie", 0, parametros[12]);

                            string descripcionSucursal = General.EjecutarConsulta(string.Format("select U_SlpName from [@SCGD_CVENTA] where DocNum = '{0}'", parametros[8]), Conexion);

                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Sucurs", 0, descripcionSucursal);
                        }
                    }
                }
                
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE;

                PermisosPlacas();

            }
        }

        /**
         * Método que elimina del datasource la fila de la matrix 
         */
        private  void BorrarDatosMatrizSeguimiento(string tablaSeguimiento,string tipocampo, int idRegistro)
        {
            if (tablaSeguimiento.Equals("@SCGD_INSCRIP"))
            {
                string tipoEvento = FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Cod_Eve", idRegistro - 1);
                string eventoFinalizacion = General.EjecutarConsulta(
                    string.Format("select U_EvenFin from [@SCGD_EVENTO] where Code = '{0}'", tipoEvento), Conexion);

                if (eventoFinalizacion.Equals("Y"))
                {
                    eventoFinal = false;
                }
            }

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).RemoveRecord(idRegistro - 1);

            int nuevoRegistro = FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).Size;

            //Si el datasource queda en 0 inserta una línea vacia para que no presente conflictos con lineas hijas vacías y poder borrar la misma
            if (nuevoRegistro == 0)
            {
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).InsertRecord(nuevoRegistro);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue(tipocampo, nuevoRegistro, "  ");
            }
        }
        
        /**
         * Método que se encarga de agregar los datos a la matrix de seguimiento dada 
         */
        private void AgregarDatosMatrizSeguimiento(string tipoGestion, string tipoEvento, string fechaEvento, string ref1, string ref2, string observ, string tablaSeguimiento)
        {
            string descGestion;
            string descEvento;
            DateTime fecha;
            int intNuevoRegistro;
            string validaEvento;
            string strUsuarioSBO;
            string prenda;
            string ente;
            string ref3;
            string ref4;
            string ref5;
            string ref6;
            DateTime fechaIng;
            string fechaIngreso;
            string fechaCreacion;
            
            descGestion = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GESTION] where Code = {0}", tipoGestion), Conexion);
            descEvento = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_EVENTO] where Code = {0}", tipoEvento), Conexion);

            strUsuarioSBO = ApplicationSBO.Company.UserName;
            //strUsuarioSBO = General.EjecutarConsulta(string.Format("SELECT USER_CODE FROM OUSR WHERE USER_CODE = '{0}'", strUsuarioSBO), Conexion);

            fechaCreacion = DateTime.Now.ToString("yyyyMMdd");

            intNuevoRegistro = FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).Size;
            if (intNuevoRegistro==1)
            {
                validaEvento = FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Evento", 0);
                validaEvento = validaEvento.Trim();
                if (!string.IsNullOrEmpty(validaEvento))
                {
                    FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).InsertRecord(intNuevoRegistro);
                    intNuevoRegistro += 1;
                }
                else
                {
                    intNuevoRegistro = 1;
                }
            }
            else
            {
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).InsertRecord(intNuevoRegistro);
                intNuevoRegistro += 1;
            }

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Cod_Ges", intNuevoRegistro - 1, tipoGestion);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Cod_Eve", intNuevoRegistro - 1, tipoEvento);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Gestion",intNuevoRegistro-1,descGestion);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Evento", intNuevoRegistro - 1, descEvento);
            
            if (!string.IsNullOrEmpty(fechaEvento))
            {
                fecha = DateTime.ParseExact(fechaEvento, "yyyyMMdd", null);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Ev", intNuevoRegistro - 1, fecha.ToString("yyyyMMdd"));
            }

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref1", intNuevoRegistro - 1, ref1);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref2", intNuevoRegistro - 1, ref2);

            if (tablaSeguimiento == "@SCGD_REV_VEH")
            {
                ref3 = EditTextNoRef3RV.ObtieneValorUserDataSource();
                ref4 = EditTextNoRef4RV.ObtieneValorUserDataSource();
                ref5 = EditTextNoRef5RV.ObtieneValorUserDataSource();
                ref6 = EditTextNoRef6RV.ObtieneValorUserDataSource();
                fechaIngreso = EditTextFechIngresoRV.ObtieneValorUserDataSource();

                if (!string.IsNullOrEmpty(fechaIngreso))
                {
                    fechaIng = DateTime.ParseExact(fechaIngreso, "yyyyMMdd", null);
                    FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_In", intNuevoRegistro - 1, fechaIng.ToString("yyyyMMdd"));
                }
                
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref3", intNuevoRegistro - 1, ref3);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref4", intNuevoRegistro - 1, ref4);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref5", intNuevoRegistro - 1, ref5);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref6", intNuevoRegistro - 1, ref6);
            }

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Observ", intNuevoRegistro - 1, observ);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Ingresa", intNuevoRegistro - 1, strUsuarioSBO);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Cre", intNuevoRegistro - 1, fechaCreacion);
            
            if (tablaSeguimiento=="@SCGD_DOC_LEG")
            {

                prenda = CheckBoxPrenda.ObtieneValorUserDataSource();
                ente = EditTextEnteFinanciero.ObtieneValorUserDataSource();

                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Prenda", intNuevoRegistro - 1, prenda);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Ins_Fin", intNuevoRegistro - 1, ente);

            }

        }

        /**
         * Método que se encarga de asignarle al datasource en el registro respectivo los nuevos valores que se editaron
         */
        private void EditarDatosMatrizSeguimiento(string tipoGestion, string tipoEvento, string fechaEvento, string ref1, string ref2, string observ, string tablaSeguimiento, int idRegistro)
        {

            string descGestion;
            string descEvento;
            DateTime fecha;
            string ref3;
            string ref4;
            string ref5;
            string ref6;
            DateTime fechaIng;
            string fechaIngreso;
            string strUsuarioSBO;
            string fechaModificacion;
            string prenda;
            string ente;

            descGestion = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GESTION] where Code = {0}", tipoGestion), Conexion);
            descEvento = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_EVENTO] where Code = {0}", tipoEvento), Conexion);

            strUsuarioSBO = ApplicationSBO.Company.UserName;
            //strUsuarioSBO = General.EjecutarConsulta(string.Format("SELECT USER_CODE FROM OUSR WHERE USER_CODE = '{0}'", strUsuarioSBO), Conexion);

            fechaModificacion = DateTime.Now.ToString("yyyyMMdd");

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Cod_Ges", idRegistro - 1, tipoGestion);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Cod_Eve", idRegistro - 1, tipoEvento);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Gestion", idRegistro - 1, descGestion);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Evento", idRegistro - 1, descEvento);
            
            if (!string.IsNullOrEmpty(fechaEvento))
            {
                fecha = DateTime.ParseExact(fechaEvento, "yyyyMMdd", null);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Ev", idRegistro - 1, fecha.ToString("yyyyMMdd"));
            }
            
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref1", idRegistro - 1, ref1);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref2", idRegistro - 1, ref2);
            
            if (tablaSeguimiento == "@SCGD_REV_VEH")
            {
                ref3 = EditTextNoRef3RV.ObtieneValorUserDataSource();
                ref4 = EditTextNoRef4RV.ObtieneValorUserDataSource();
                ref5 = EditTextNoRef5RV.ObtieneValorUserDataSource();
                ref6 = EditTextNoRef6RV.ObtieneValorUserDataSource();
                fechaIngreso = EditTextFechIngresoRV.ObtieneValorUserDataSource();

                if (!string.IsNullOrEmpty(fechaIngreso))
                {
                    fechaIng = DateTime.ParseExact(fechaIngreso, "yyyyMMdd", null);
                    FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_In", idRegistro - 1, fechaIng.ToString("yyyyMMdd"));
                }

                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref3", idRegistro - 1, ref3);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref4", idRegistro - 1, ref4);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref5", idRegistro - 1, ref5);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Ref6", idRegistro - 1, ref6);
            }

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Observ", idRegistro - 1, observ);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Modific", idRegistro - 1, strUsuarioSBO);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Mod", idRegistro - 1, fechaModificacion);

            if (tablaSeguimiento == "@SCGD_DOC_LEG")
            {

                prenda = CheckBoxPrenda.ObtieneValorUserDataSource();
                ente = EditTextEnteFinanciero.ObtieneValorUserDataSource();

                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Prenda", idRegistro - 1, prenda);
                FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Ins_Fin", idRegistro - 1, ente);

            }

        }

        /**
         * Método que se encarga de asignarle al datasource en el registro respectivo los nuevos valores que se editaron, esto para gastos
         */
        private void EditarDatosMatrizSeguimientoGasto(string tipoGasto, string fechaDocumento, string numDoc, decimal decTotal, string observ, string tablaSeguimiento,
            NumberFormatInfo n, int idRegistro)
        {
            string descGasto;
            DateTime fecha;
            string strUsuarioSBO;
            string fechaModificacion;
            decimal decMonto = 0;
            string strMonto;
            decimal totalGasto = 0;
            
            descGasto = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GASTOS] where Code = {0}", tipoGasto), Conexion);

            fecha = DateTime.ParseExact(fechaDocumento, "yyyyMMdd", null);

            strUsuarioSBO = ApplicationSBO.Company.UserName;
            //strUsuarioSBO = General.EjecutarConsulta(string.Format("SELECT USER_CODE FROM OUSR WHERE USER_CODE = '{0}'", strUsuarioSBO), Conexion);

            fechaModificacion = DateTime.Now.ToString("yyyyMMdd");

            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Cod_Gas", idRegistro - 1, tipoGasto);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Gasto", idRegistro - 1, descGasto);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Num_Doc", idRegistro - 1, numDoc);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Doc", idRegistro - 1, fecha.ToString("yyyyMMdd"));
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Monto", idRegistro - 1, decTotal.ToString(n));
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Observ", idRegistro - 1, observ);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Modific", idRegistro - 1, strUsuarioSBO);
            FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).SetValue("U_Fech_Mod", idRegistro - 1, fechaModificacion);

            MatrixGastos.Matrix.LoadFromDataSource();

            EditTextNoDocG.AsignaValorUserDataSource("");
            EditTextFechDocG.AsignaValorUserDataSource("");
            EditTextMontoG.AsignaValorUserDataSource("0");
            EditTextObservacionesG.AsignaValorUserDataSource("");

            MatrixGastos.Matrix.FlushToDataSource();

            for (int i = 0; i <= MatrixGastos.Matrix.RowCount - 1; i++)
            {
                strMonto = FormularioSBO.DataSources.DBDataSources.Item(tablaSeguimiento).GetValue("U_Monto", i);
                strMonto = strMonto.Trim();
                
                if (!string.IsNullOrEmpty(strMonto))
                {
                    decMonto = decimal.Parse(strMonto, n);
                    totalGasto = totalGasto + decMonto;
                }
            }

            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Total", 0, totalGasto.ToString(n));
        }

        /**
         * Método que se de actualizar el estado de los datos maestros del vehículo con el estado configurado al evento
         */
        private void actualizarEstadoVehiculo(string unidad, string codigoEvento)
        {
            if(!string.IsNullOrEmpty(codigoEvento))
            {
                Company m_oCompany = (Company)CompanySBO;
                CompanyService companyService;
                GeneralService generalService;
                GeneralData generalData;
                GeneralDataParams generalDataParams;

                string codigoVehiculo = General.EjecutarConsulta(string.Format("Select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'", unidad), Conexion);
                string estadoEvento = General.EjecutarConsulta(string.Format("Select U_Estado from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);

                if (!string.IsNullOrEmpty(estadoEvento) && (!string.IsNullOrEmpty(codigoVehiculo)))
                {
                    companyService = m_oCompany.GetCompanyService();
                    generalService = companyService.GetGeneralService("SCGD_VEH");
                    generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    generalDataParams.SetProperty("Code", codigoVehiculo);
                    generalData = generalService.GetByParams(generalDataParams);

                    generalData.SetProperty("U_Estatus", estadoEvento);
                    generalService.Update(generalData);

                    ultimoEventoAgregado = "";
                    panelUltimoEvento = 1;
                }
            }
        }

        /**
         * Método que se de actualizar la placa del vehículo en el maestro del vehículo
         */
        private void actualizarPlacaVehiculo(string unidad, string numPlaca)
        {
            if (!string.IsNullOrEmpty(numPlaca))
            {
                Company m_oCompany = (Company)CompanySBO;
                CompanyService companyService;
                GeneralService generalService;
                GeneralData generalData;
                GeneralDataParams generalDataParams;

                string codigoVehiculo = General.EjecutarConsulta(string.Format("Select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'", unidad), Conexion);

                if (!string.IsNullOrEmpty(codigoVehiculo))
                {
                    companyService = m_oCompany.GetCompanyService();
                    generalService = companyService.GetGeneralService("SCGD_VEH");
                    generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    generalDataParams.SetProperty("Code", codigoVehiculo);
                    generalData = generalService.GetByParams(generalDataParams);

                    generalData.SetProperty("U_Num_Plac", numPlaca);
                    generalService.Update(generalData);
                }
            }
        }

        /**
         * Método que se encarga de limpiar lo referente al encabezado del UDO de placas
         */
        private void LimpiarExpPlacas()
        {
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Unid",0,"");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_VIN", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Moto", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Color", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Marca", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Estilo", 0,"");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Modelo", 0,"");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_CV", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Num_Fact", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Anno", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Cod_Clie", 0, "");
            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PLACA").SetValue("U_Nom_Clie", 0, "");
        }

        /**
         * Método que se encarga de cargar en la matrix de la pestañan de reportes los reportes del módulo de placas que estan configurados para el módulo y se encuentra en la tabla respectiva
         */
        private void CargarReportes()
        {
            DataTable dataTableconsultaR = FormularioSBO.DataSources.DataTables.Add("ConsultaR");
            dataTableconsultaR.ExecuteQuery("Select Code, Name, U_Descrip from [@SCGD_RPT_PLACAS]");

            int tammannoDTCR = dataTableconsultaR.Rows.Count;

            if (tammannoDTCR > 0)
            {
                for (int i = 0; i <= tammannoDTCR - 1; i++)
                {
                    DataTableReportes.Rows.Add();
                    DataTableReportes.SetValue("codeR", i, dataTableconsultaR.GetValue("Code", i).ToString().Trim());
                    DataTableReportes.SetValue("nameR", i, dataTableconsultaR.GetValue("Name", i).ToString().Trim());
                    DataTableReportes.SetValue("descripR", i, dataTableconsultaR.GetValue("U_Descrip", i).ToString().Trim());
                }

                MatrixReportes.Matrix.LoadFromDataSource();
            }
        }

        /**
         * Método que se encarga de buscar los permisos asociados al usuario para le uso del formulario de placas y en base a la configuración de estos deshabilitar las diferentes pestañas
         */
        public void PermisosPlacas()
        {
            string[] seguridadE = General.ObtenerSeguridadEventos(ApplicationSBO, Conexion);
            int tamanoArrayE = seguridadE.Length;
            string revisionV = "";
            string documentosL = "";
            string inscripcion = "";

            if (tamanoArrayE > 0)
            {
                for (int i = 0; i <= tamanoArrayE - 1; i++)
                {
                    if (seguridadE[i].Equals("1"))
                    {
                        revisionV = seguridadE[i];
                    }

                    else if (seguridadE[i].Equals("2"))
                    {
                        documentosL = seguridadE[i];
                    }

                    else if (seguridadE[i].Equals("3"))
                    {
                        inscripcion = seguridadE[i];
                    }
                }

                if (string.IsNullOrEmpty(revisionV))
                {
                    FormularioSBO.Items.Item("fldRevVehi").Enabled = false;
                }

                if (string.IsNullOrEmpty(documentosL))
                {
                    FormularioSBO.Items.Item("fldRocLeg").Enabled = false;
                }

                if (string.IsNullOrEmpty(inscripcion))
                {
                    FormularioSBO.Items.Item("fldInscrip").Enabled = false;
                }
            }

            else if (tamanoArrayE == 0)
            {
                FormularioSBO.Items.Item("fldRevVehi").Enabled = false;
                FormularioSBO.Items.Item("fldRocLeg").Enabled = false;
                FormularioSBO.Items.Item("fldInscrip").Enabled = false;
            }


            string seguridadG = General.ObtenerSeguridadGastos(ApplicationSBO, Conexion);

            if (seguridadG.Equals("0"))
            {
                FormularioSBO.Items.Item("fldGastos").Enabled = false;
            }
        }

    }
}
