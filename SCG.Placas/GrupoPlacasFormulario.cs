using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.DI;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;
using SCG.DMSOne.Framework;
using ChooseFromList = SAPbouiCOM.ChooseFromList;
using Company = SAPbobsCOM.Company;

namespace SCG.Placas
{
    public partial class GrupoPlacasFormulario : IFormularioSBO, IUsaMenu 
    {
        protected void DataLoadEvent(BusinessObjectInfo businessObjectInfo, ref bool bubbleEvent)
        {
            if (!businessObjectInfo.BeforeAction && businessObjectInfo.ActionSuccess)
            {

            }
        }

        /**
         * Método que se encarga de cargar el choose from list con sus respectivas condiciones, esto para las unidades
         */
        public void CFLUnidad(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent CFLUnidad = (SAPbouiCOM.IChooseFromListEvent) pval;
            string sCFL_ID = CFLUnidad.ChooseFromListUID;
            ChooseFromList oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID);

            SAPbouiCOM.DataTable oDataTable;

            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;

            //string strTipoVendido;
            //string strDispVendido;
            
            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;

            if (pval.ActionSuccess)
            {

                if (CFLUnidad.SelectedObjects != null)
                {
                    oDataTable = CFLUnidad.SelectedObjects;

                    EditTextUnidad.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Unid", 0).ToString());
                    EditTextNumChasis.AsignaValorUserDataSource(oDataTable.GetValue("U_Num_VIN", 0).ToString());
                    EditTextNumMotor.AsignaValorUserDataSource(oDataTable.GetValue("U_Num_Mot", 0).ToString());
                    EditTextAnno.AsignaValorUserDataSource(oDataTable.GetValue("U_Ano_Vehi", 0).ToString());

                    ComboBoxMarca.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Marc", 0).ToString());
                    ComboBoxEstilo.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Esti", 0).ToString());
                    ComboBoxModelo.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Mode", 0).ToString());
                    ComboBoxColor.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Col", 0).ToString());
                    ComboBoxEstado.AsignaValorUserDataSource(oDataTable.GetValue("U_Estatus", 0).ToString());
                    ComboBoxCondicion.AsignaValorUserDataSource(oDataTable.GetValue("U_Dispo", 0).ToString());
                    ComboBoxUbicacion.AsignaValorUserDataSource(oDataTable.GetValue("U_Cod_Ubic", 0).ToString());
                }
            }

            else if (pval.BeforeAction)
            {
                //strTipoVendido = General.EjecutarConsulta("Select U_Inven_V from [@SCGD_ADMIN] where Code = 'DMS'", Conexion);
                //strDispVendido = General.EjecutarConsulta("Select U_Disp_V from [@SCGD_ADMIN] where Code = 'DMS'", Conexion);

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
                //oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

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


                oCFL.SetConditions(oConditions);
            }
        }

        public void CFLCargaGrupo(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent CFLCargaGrupo = (IChooseFromListEvent) pval;
            
            DataTable DataTable;

            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;

            if (pval.ActionSuccess)
            {

                if (CFLCargaGrupo.SelectedObjects != null)
                {
                    DataTable = CFLCargaGrupo.SelectedObjects;

                    string fecha = DataTable.GetValue("U_Fech_G", 0).ToString().Trim();

                    string[] fStrings = fecha.Split(' ');
                    
                    EditTextNoGrupoE.AsignaValorUserDataSource(DataTable.GetValue("DocNum", 0).ToString().Trim());
                    EditTextFechaGrupoE.AsignaValorUserDataSource(fStrings[0]);
                    EditTextDescGrupoE.AsignaValorUserDataSource(DataTable.GetValue("U_Desc_G", 0).ToString().Trim());
                }
                
            }
        }

        public void CFLCargaGrupoGasto(string FormUID, ItemEvent pval)
        {
            IChooseFromListEvent CFLCargaGrupo = (IChooseFromListEvent)pval;

            DataTable DataTable;

            if (FormularioSBO.Mode == BoFormMode.fm_FIND_MODE) return;

            if (pval.ActionSuccess)
            {

                if (CFLCargaGrupo.SelectedObjects != null)
                {
                    DataTable = CFLCargaGrupo.SelectedObjects;

                    string fecha = DataTable.GetValue("U_Fech_G", 0).ToString().Trim();

                    string[] fStrings = fecha.Split(' ');

                    EditTextNoGrupoG.AsignaValorUserDataSource(DataTable.GetValue("DocNum", 0).ToString().Trim());
                    EditTextFechaGrupoG.AsignaValorUserDataSource(fStrings[0]);
                    EditTextDescGrupoG.AsignaValorUserDataSource(DataTable.GetValue("U_Desc_G", 0).ToString().Trim());
                }

            }
        }

        public void  ButtonSBOLimpiarItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if(pval.BeforeAction==false && pval.ActionSuccess)
            {
                LimpiarBusquedaGrupos();
                LimpiarSeleccion();
                LimpiarEventos();
                LimpiarGastos();
            }
        }

        public void ButtonSBOBuscarItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction==false && pval.ActionSuccess)
            {
                DataTableSeleccion = FormularioSBO.DataSources.DataTables.Item("SeleccionU");
                DataTableConsulta = FormularioSBO.DataSources.DataTables.Item("Consulta");

                CargarMatrixSeleccion(MatrixSeleccionGrupo, DataTableConsulta, DataTableSeleccion);

                FormularioSBO.PaneLevel = 1;
                FormularioSBO.Items.Item("fldSelUnid").Click();

                LimpiarBusquedaGrupos();

            }
        }

        public void ButtonSBOAgregarGItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                DataTableSeleccion = FormularioSBO.DataSources.DataTables.Item("SeleccionU");
                DataTableEventos = FormularioSBO.DataSources.DataTables.Item("EventosGrupo");
                DataTableGastos = FormularioSBO.DataSources.DataTables.Item("GastosGrupo");

                CargarMatrixEventos(MatrixSeleccionGrupo, MatrixEventosGrupo, DataTableSeleccion, DataTableEventos);
                CargarMatrixGastos(MatrixSeleccionGrupo, MatrixGastosGrupo, DataTableSeleccion, DataTableGastos);
                
            }
        }

        public void ButtonSBOCopiarFechaItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int tamanoME = MatrixEventosGrupo.Matrix.RowCount;

                if (tamanoME == 0)
                {
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error);
                }

                else
                {
                    string fechaE = EditTextFechaEventoE.ObtieneValorUserDataSource();

                    MatrixEventosGrupo.Matrix.FlushToDataSource();

                    AsignarDatoDatatable(DataTableEventos, "fechaEventoE", fechaE);
                    
                    MatrixEventosGrupo.Matrix.LoadFromDataSource();
                }
                
            }
        }

        public void ButtonSBOBorrarEventoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if(pval.BeforeAction == false && pval.ActionSuccess) 
            {
                int index = MatrixEventosGrupo.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                MatrixEventosGrupo.Matrix.FlushToDataSource();
                
                if (BorrarElementoMatrix(MatrixEventosGrupo, DataTableEventos, index))
                {
                    IndexDataTableE = IndexDataTableE - 1;
                    MatrixEventosGrupo.Matrix.LoadFromDataSource();
                }
            }
        }

        public void ButtonSBOApplicarEventosItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string gestion = ComboBoxGestionE.ObtieneValorUserDataSource();
            string evento = ComboBoxEventoE.ObtieneValorUserDataSource();
            
            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                int tamannoME = MatrixEventosGrupo.Matrix.RowCount;

                if(string.IsNullOrEmpty(gestion))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaGestion, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if(string.IsNullOrEmpty(evento))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (tamannoME == 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorMatrixEventoVacia, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if (tamannoME > 0)
                {
                    string fecha;
                    string fila;
                    int Numfila;
                    
                    for (int i = 0; i <= tamannoME - 1; i++)
                    {
                        MatrixEventosGrupo.Matrix.FlushToDataSource();
                        try
                        {
                            fecha = DataTableEventos.GetValue("fechaEventoE", i).ToString();
                        }
                        catch
                        {
                            fecha = "";
                        }

                        Numfila = i + 1;
                        fila = Numfila.ToString();

                        if (string.IsNullOrEmpty(fecha))
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFechaEventoGrupo + fila, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            break;
                        }
                    }
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                string descGrupo = EditTextDescGrupoE.ObtieneValorUserDataSource();
                string[] expedientes;

                if (string.IsNullOrEmpty(descGrupo))
                {
                    if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGrupo, DefaultBtn: 1,
                                                  Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                    {
                        if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGestion, DefaultBtn: 1,
                                              Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                        {
                            expedientes = AplicarEvento();
                            int numeroGrupo = CrearGrupo(descGrupo, "0");
                            CrearGrupoRelacion(numeroGrupo, DataTableEventos, "unidadE", expedientes);
                            MatrixSeleccionGrupo.Matrix.Clear();

                            LimpiarEventos();
                            LimpiarGastos();

                            ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNumeroGrupo + numeroGrupo, DefaultBtn: 1,
                                                          Btn1Caption: "Ok");
                        }
                    }
                }

                else
                {
                    if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGestion, DefaultBtn: 1,
                                              Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                    {
                        expedientes = AplicarEvento();
                        int numeroGrupo = CrearGrupo(descGrupo, "0");
                        CrearGrupoRelacion(numeroGrupo, DataTableEventos, "unidadE", expedientes);
                        MatrixSeleccionGrupo.Matrix.Clear();

                        LimpiarEventos();
                        LimpiarGastos();

                        ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNumeroGrupo + numeroGrupo, DefaultBtn: 1,
                                                      Btn1Caption: "Ok");
                    }
                }
            }
        }

        public void ButtonSBOCargarEnBaseItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string idGrupo = EditTextNoGrupoE.ObtieneValorUserDataSource();

            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                if(string.IsNullOrEmpty(idGrupo))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaNumGrupo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                var columnasDG = new string[] { "unidadE", "numChasisE", "numMotorE", "marcaE", "estiloE", "modeloE", "colorE", "annoE", "contVentaE", "numFactE", "fechaEventoE" };
                var columnasDC = new string[] { "U_Num_Unid", "U_Num_VIN", "U_Num_Moto", "U_Marca", "U_Estilo", "U_Modelo", "U_Color", "U_Anno", "U_Num_CV", "U_Num_Fact","" };

                

                BuscarGrupo(MatrixEventosGrupo, DataTableEventos,ref IndexDataTableE, columnasDG, DataTableConsulta, columnasDC,"instFinanE","col_InsFin" ,"Select BankCode, BankName from [ODSC]",idGrupo);
                //BuscarGrupo(idGrupo);
            }
        }

        public void ButtonSBOCopiarFechaGastoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int tamanoMG = MatrixGastosGrupo.Matrix.RowCount;

                if (tamanoMG == 0)
                {
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error);
                }

                else
                {
                    string fechaG = EditTextFechaDocumentoG.ObtieneValorUserDataSource();

                    MatrixGastosGrupo.Matrix.FlushToDataSource();

                    AsignarDatoDatatable(DataTableGastos, "fechaDocumG", fechaG);

                    MatrixGastosGrupo.Matrix.LoadFromDataSource();
                }

            }
        }

        public void ButtonSBOCopiarMontoGastoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int tamanoMG = MatrixGastosGrupo.Matrix.RowCount;

                if (tamanoMG == 0)
                {
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error);
                }

                else
                {
                    string montoG = EditTextMontoG.ObtieneValorUserDataSource();

                    MatrixGastosGrupo.Matrix.FlushToDataSource();

                    AsignarDatoDatatable(DataTableGastos, "montoG", montoG);

                    MatrixGastosGrupo.Matrix.LoadFromDataSource();

                    int cantidad = MatrixGastosGrupo.Matrix.RowCount;
                   
                    NumberFormatInfo n = DIHelper.GetNumberFormatInfo(CompanySBO);
                    
                    if(cantidad > 0)
                    {
                        decimal montoAplicar = decimal.Parse(montoG, n);
                        montoAplicar = cantidad * montoAplicar;

                        EditTextTotal.AsignaValorUserDataSource(montoAplicar.ToString(n));
                    }
                }
            }
        }

        public void ButtonSBOCopiarGastoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string gasto = ComboBoxGastoG.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if(string.IsNullOrEmpty(gasto))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaTipoGasto, SAPbouiCOM.BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error);
                }
            }

            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                int tamanoMG = MatrixGastosGrupo.Matrix.RowCount;

                if (tamanoMG == 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error);
                }

                else
                {
                    var aColumnas = new string[] { "tipoGastoG"};
                    var aDatos = new string[] {gasto};

                    MatrixGastosGrupo.Matrix.FlushToDataSource();

                    AsignarDatosDatatable(DataTableGastos, aColumnas, aDatos);

                    MatrixGastosGrupo.Matrix.LoadFromDataSource();
                }

            }
        }

        public void ButtonSBOBorrarGastoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            int tamannoMG = MatrixGastosGrupo.Matrix.RowCount;

            if(pval.BeforeAction && pval.ActionSuccess == false)
            {
                if(tamannoMG <= 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorBorrarEvento, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                NumberFormatInfo n = DIHelper.GetNumberFormatInfo(CompanySBO);

                int index = MatrixGastosGrupo.Matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                MatrixGastosGrupo.Matrix.FlushToDataSource();
                
                string montoDTG = DataTableGastos.GetValue("montoG", index - 1).ToString().Trim();
                string totalGrupo = EditTextTotal.ObtieneValorUserDataSource();

                if (BorrarElementoMatrix(MatrixGastosGrupo, DataTableGastos, index))
                {
                    IndexDataTableG = IndexDataTableG - 1;
                    MatrixGastosGrupo.Matrix.LoadFromDataSource();

                    decimal monto = decimal.Parse(montoDTG, n);
                    decimal total = decimal.Parse(totalGrupo, n);

                    total = total - monto;

                    EditTextTotal.AsignaValorUserDataSource(total.ToString(n));

                }
            }
        }

        public void ButtonSBOApplicarGastosItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            
            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                int tamannoMG = MatrixGastosGrupo.Matrix.RowCount;

                if(tamannoMG == 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorMatrixGastoVacia, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

                else if(tamannoMG > 0)
                {
                    string montoDTG;
                    string gasto;
                    decimal monto;
                    string fila;
                    int Numfila;
                    string fecha;

                    for (int i = 0; i <= tamannoMG - 1; i++)
                    {
                        MatrixGastosGrupo.Matrix.FlushToDataSource();
                        gasto = DataTableGastos.GetValue("tipoGastoG", i).ToString();
                        montoDTG = DataTableGastos.GetValue("montoG", i).ToString();
                        try
                        {
                            fecha = DataTableGastos.GetValue("fechaDocumG", i).ToString();
                        }
                        catch
                        {
                            fecha = "";
                        }


                        monto = decimal.Parse(montoDTG);
                        
                        Numfila = i + 1;
                        fila = Numfila.ToString();

                        if(string.IsNullOrEmpty(gasto))
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorGastoVacioGrupo + fila, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            break;
                        }

                        if(monto <= 0 )
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorMontoGrupo + fila, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            break;
                        }

                        if(string.IsNullOrEmpty(fecha))
                        {
                            BubbleEvent = false;
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFechaGastoGrupo + fila, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            break;
                        }


                    }
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                string descGrupo = EditTextDescGrupoG.ObtieneValorUserDataSource();
                string totalGrupo = EditTextTotal.ObtieneValorUserDataSource();
                
                if(string.IsNullOrEmpty(descGrupo))
                {
                    if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGrupo, DefaultBtn: 1,
                                              Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                    {
                        if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGasto, DefaultBtn: 1,
                                                  Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                        {
                            string[] expedientes = new string[] { };
                            int gastosAsignados = AplicarGastos(ref expedientes);

                            if (gastosAsignados > 0)
                            {

                                int numeroGrupo = CrearGrupo(descGrupo, totalGrupo);
                                CrearGrupoRelacion(numeroGrupo, DataTableGastos, "unidadG", expedientes);
                                MatrixSeleccionGrupo.Matrix.Clear();

                                LimpiarGastos();
                                LimpiarEventos();

                                ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNumeroGrupo + numeroGrupo, DefaultBtn: 1,
                                                          Btn1Caption: "Ok");
                            }
                        }
                    }
                }
                else
                {
                    if (ApplicationSBO.MessageBox(My.Resources.Resource.MensajeConfirmacionGasto, DefaultBtn: 1,
                                            Btn1Caption: My.Resources.Resource.BotonSi, Btn2Caption: "No") == 1)
                    {
                        string[] expedientes = new string[] {};
                        int gastosAsignados = AplicarGastos(ref expedientes);

                        if (gastosAsignados > 0)
                        {

                            int numeroGrupo = CrearGrupo(descGrupo, totalGrupo);
                            CrearGrupoRelacion(numeroGrupo, DataTableGastos, "unidadG",expedientes);
                            MatrixSeleccionGrupo.Matrix.Clear();

                            LimpiarGastos();
                            LimpiarEventos();

                            ApplicationSBO.MessageBox(My.Resources.Resource.MensajeNumeroGrupo + numeroGrupo, DefaultBtn: 1,
                                                        Btn1Caption: "Ok");
                        }
                    }
                }
            }
        }

        public void ButtonSBOCargarEnBaseGastoItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            string idGrupo = EditTextNoGrupoG.ObtieneValorUserDataSource();

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if (string.IsNullOrEmpty(idGrupo))
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaNumGrupo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                var columnasDG = new string[] { "unidadG", "numChasisG", "numMotorG", "marcaG", "estiloG", "modeloG", "colorG", "annoG", "contVentaG", "numFactG", "fechaDocumG" };
                var columnasDC = new string[] { "U_Num_Unid", "U_Num_VIN", "U_Num_Moto", "U_Marca", "U_Estilo", "U_Modelo", "U_Color", "U_Anno", "U_Num_CV", "U_Num_Fact", "" };
                
                BuscarGrupo(MatrixGastosGrupo, DataTableGastos,ref IndexDataTableG, columnasDG, DataTableConsulta, columnasDC,"tipoGastoG", "col_TipoG" ,"Select Code, U_Descrip from [@SCGD_GASTOS]" ,idGrupo);
                
            }
        }

        public void ButtonSBOCalcularGastosItemPressed(string FormUID, ItemEvent pval, ref bool BubbleEvent)
        {
            int tamanoMG = MatrixGastosGrupo.Matrix.RowCount;

            if (pval.BeforeAction && pval.ActionSuccess == false)
            {
                if(tamanoMG == 0)
                {
                    BubbleEvent = false;
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaNumGrupo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                CalcularGastos(MatrixGastosGrupo, "col_Monto");
            }
        }

        /**
         * Método que se encarga de cargar el ComboBox de eventos en base al tipo del ComboBox de gestión que se seleccionó 
         */
        public void ComboBoxGestionSelected(ItemEvent pval)
        {
            string codGestion;
            Item sboItem;
            ComboBox sboCombo;

            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                if (pval.ItemUID == ComboBoxGestionE.UniqueId)
                {
                    codGestion = ComboBoxGestionE.ObtieneValorUserDataSource();

                    sboItem = FormularioSBO.Items.Item("cmbTipEven");
                    sboCombo = (SAPbouiCOM.ComboBox) sboItem.Specific;
                    General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code, U_Descrip from [@SCGD_EVENTO] where U_Gestion = {0}", codGestion), Conexion);
                }
            }
        }

        public void ComboBoxMarcaSelected(ItemEvent pval)
        {
            string codMarca;
            Item sboItem;
            ComboBox sboCombo;

            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                if (pval.ItemUID == ComboBoxMarca.UniqueId)
                {
                    codMarca = ComboBoxMarca.ObtieneValorUserDataSource();

                    ComboBoxEstilo.AsignaValorUserDataSource("");
                    ComboBoxModelo.AsignaValorUserDataSource("");
                    
                    sboItem = FormularioSBO.Items.Item("cmbEstilo");
                    sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                    General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code,Name from [@SCGD_ESTILO] where U_Cod_Marc = '{0}'", codMarca), Conexion);
                }
            }
        }

        public void ComboBoxEstiloSelected(ItemEvent pval)
        {
            string codEstilo;
            Item sboItem;
            ComboBox sboCombo;

            if (pval.BeforeAction == false && pval.ActionSuccess)
            {
                if (pval.ItemUID == ComboBoxEstilo.UniqueId)
                {
                    codEstilo = ComboBoxEstilo.ObtieneValorUserDataSource();

                    sboItem = FormularioSBO.Items.Item("cmbModelo");
                    sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
                    General.CargarValidValuesEnCombos(sboCombo.ValidValues, string.Format("Select Code, Name from [@SCGD_MODELO] where U_Cod_Esti = '{0}'", codEstilo), Conexion);
                }
            }
        }

        /**
         * Método que se encarga de obtener los parámetros de busqueda, formular la consulta y cargar la matrix de selección de vehículos
         */
        public void CargarMatrixSeleccion(MatrixSBO matrix,  DataTable dataTableConsulta, DataTable dataTableSeleccion)
        {

            string consulta = "Select U_Cod_Unid, U_Num_VIN, U_Num_Mot, U_Des_Marc, U_Des_Esti, U_Des_Mode, U_Des_Col, U_Ano_Vehi, U_CTOVTA, U_NUMFAC  from [@SCGD_VEHICULO] as VEH where VEH.U_Dispo is not null";

            string unidadUDF = "U_Cod_Unid";
            string chasisUDF = "U_Num_VIN";
            string motorUDF = "U_Num_Mot";
            string marcaUDF = "U_Cod_Marc";//"U_Des_Marc";//"U_Cod_Marc"
            string estiloUDF = "U_Cod_Esti";//"U_Des_Esti";//U_Cod_Esti
            string modeloUDF = "U_Cod_Mode";//"U_Des_Mode";//U_Cod_Mode
            string colorUDF = "U_Cod_Col";//"U_Des_Col";//U_Cod_Col
            string annoUDF = "U_Ano_Vehi";
            string estadoUDF = "U_Estatus";
            string condicionUDF = "U_Dispo";
            string ubicacionUDF = "U_Cod_Ubic";

            string unidad = EditTextUnidad.ObtieneValorUserDataSource().Trim();
            string numChasis = EditTextNumChasis.ObtieneValorUserDataSource().Trim();
            string numMotor = EditTextNumMotor.ObtieneValorUserDataSource().Trim();
            string marca = ComboBoxMarca.ObtieneValorUserDataSource().Trim();
            string estilo = ComboBoxEstilo.ObtieneValorUserDataSource().Trim();
            string modelo = ComboBoxModelo.ObtieneValorUserDataSource().Trim();
            string color = ComboBoxColor.ObtieneValorUserDataSource().Trim();
            string anno = EditTextAnno.ObtieneValorUserDataSource().Trim();
            string estado = ComboBoxEstado.ObtieneValorUserDataSource().Trim();
            string condicion = ComboBoxCondicion.ObtieneValorUserDataSource().Trim();
            string ubicacion = ComboBoxUbicacion.ObtieneValorUserDataSource().Trim();

            string filtros = "";

            if(!string.IsNullOrEmpty(unidad))
            {
                filtros += " and " + unidadUDF + " = '" + unidad + "'";
            }

            if (!string.IsNullOrEmpty(numChasis))
            {
                filtros += " and " + chasisUDF + " = '" + numChasis + "'";
            }

            if (!string.IsNullOrEmpty(numMotor))
            {
                filtros += " and " + motorUDF + " = '" + numMotor + "'";
            }

            if (!string.IsNullOrEmpty(marca))
            {
                filtros += " and " + marcaUDF + " = '" + marca + "'";
            }

            if (!string.IsNullOrEmpty(estilo))
            {
                filtros += " and " + estiloUDF + " = '" + estilo + "'";
            }

            if (!string.IsNullOrEmpty(modelo))
            {
                filtros += " and " + modeloUDF + " = '" + modelo + "'";
            }

            if (!string.IsNullOrEmpty(color))
            {
                filtros += " and " + colorUDF + " = '" + color + "'";
            }

            if (!string.IsNullOrEmpty(anno))
            {
                filtros += " and " + annoUDF + " = '" + anno + "'";
            }

            if (!string.IsNullOrEmpty(estado) && !estado.Equals("0"))
            {
                filtros += " and " + estadoUDF + " = '" + estado + "'";
            }

            if (!string.IsNullOrEmpty(condicion) && !condicion.Equals("0"))
            {
                filtros += " and " + condicionUDF + " = '" + condicion + "'";
            }

            if (!string.IsNullOrEmpty(ubicacion) && !ubicacion.Equals("0"))
            {
                filtros += " and " + ubicacionUDF + " = '" + ubicacion + "'";
            }

            consulta = consulta + filtros;

            matrix.Matrix.Clear();

            dataTableConsulta.Clear();
            dataTableConsulta.ExecuteQuery(consulta);

            int tamanoC = dataTableConsulta.Rows.Count;
            int tamanoS = dataTableSeleccion.Rows.Count;

            //Borrar las filas del dataset, esto para el caso de permitir realizar más de una búsqueda
            if (tamanoS > 0)
            {
                LimpiarDataTable(dataTableSeleccion, tamanoS);
            }

            for (int i = 0; i <= tamanoC - 1; i++)
            {
                if (!string.IsNullOrEmpty(dataTableConsulta.GetValue("U_Cod_Unid", 0).ToString()))
                {
                    dataTableSeleccion.Rows.Add();
                    dataTableSeleccion.SetValue("numChasisS", i, dataTableConsulta.GetValue("U_Num_VIN", i).ToString().Trim());
                    dataTableSeleccion.SetValue("numMotorS", i, dataTableConsulta.GetValue("U_Num_Mot", i).ToString().Trim());
                    dataTableSeleccion.SetValue("marcaS", i, dataTableConsulta.GetValue("U_Des_Marc", i).ToString().Trim());
                    dataTableSeleccion.SetValue("estiloS", i, dataTableConsulta.GetValue("U_Des_Esti", i).ToString().Trim());
                    dataTableSeleccion.SetValue("modeloS", i, dataTableConsulta.GetValue("U_Des_Mode", i).ToString().Trim());
                    dataTableSeleccion.SetValue("colorS", i, dataTableConsulta.GetValue("U_Des_Col", i).ToString().Trim());
                    dataTableSeleccion.SetValue("annoS", i, dataTableConsulta.GetValue("U_Ano_Vehi", i).ToString().Trim());

                    dataTableSeleccion.SetValue("unidadS", i, dataTableConsulta.GetValue("U_Cod_Unid", i).ToString().Trim());
                    dataTableSeleccion.SetValue("contVentaS", i, dataTableConsulta.GetValue("U_CTOVTA", i).ToString().Trim());
                    dataTableSeleccion.SetValue("numFactS", i, dataTableConsulta.GetValue("U_NUMFAC", i).ToString().Trim());
                }
            }

            matrix.Matrix.LoadFromDataSource();
        }
        
        /**
         * Métdo que se encarga de obtener los vehículos seleccionados de la matrix de Seleccion y cargarlos en la matrix de Eventos
         */
        public void CargarMatrixEventos(MatrixSBO matrixSele, MatrixSBO matrixEve, DataTable dataTableSelec, DataTable dataTableEven)
        {
            matrixEve.Matrix.FlushToDataSource();

            int tamanoS = matrixSele.Matrix.RowCount;

            if (tamanoS > 0)
            {
                matrixSele.Matrix.FlushToDataSource();

               int tamanoDTE = dataTableEven.Rows.Count;

                //Valida que el datatable no contenga un campo vacio, con el fin de no borrar los datos de la matrix al agregar un nuevo vehículo
               if (tamanoDTE == 1 && string.IsNullOrEmpty(dataTableEven.GetValue("numChasisE", 0).ToString()))
                {
                    dataTableEven.Rows.Remove(0);
                    tamanoDTE = 0;
                }

                for (int i = 0; i <= tamanoS - 1; i++)
                {
                    string seleccion =
                        dataTableSelec.GetValue("seleccionS", i).ToString().Trim();
                    if (seleccion.Equals("Y"))
                    {
                        if (tamanoDTE == 0)
                        {
                            dataTableEven.Rows.Add();
                            dataTableEven.SetValue("numChasisE", IndexDataTableE,
                                                   dataTableSelec.GetValue("numChasisS", i).ToString().Trim());
                            dataTableEven.SetValue("numMotorE", IndexDataTableE, dataTableSelec.GetValue("numMotorS", i).ToString().Trim());
                            dataTableEven.SetValue("estiloE", IndexDataTableE, dataTableSelec.GetValue("estiloS", i).ToString().Trim());
                            dataTableEven.SetValue("marcaE", IndexDataTableE, dataTableSelec.GetValue("marcaS", i).ToString().Trim());
                            dataTableEven.SetValue("modeloE", IndexDataTableE, dataTableSelec.GetValue("modeloS", i).ToString().Trim());
                            dataTableEven.SetValue("colorE", IndexDataTableE, dataTableSelec.GetValue("colorS", i).ToString().Trim());
                            dataTableEven.SetValue("annoE", IndexDataTableE, dataTableSelec.GetValue("annoS", i).ToString().Trim());
                            dataTableEven.SetValue("unidadE", IndexDataTableE, dataTableSelec.GetValue("unidadS", i).ToString().Trim());
                            dataTableEven.SetValue("contVentaE", IndexDataTableE, dataTableSelec.GetValue("contVentaS", i).ToString().Trim());
                            dataTableEven.SetValue("numFactE", IndexDataTableE, dataTableSelec.GetValue("numFactS", i).ToString().Trim());
                            dataTableEven.SetValue("fechaEventoE", IndexDataTableE, DateTime.Now.ToString("yyyyMMdd"));


                            IndexDataTableE += 1;
                        }

                        else if (tamanoDTE > 0)
                        {
                            bool existe = false;

                            for (int z = 0; z <= tamanoDTE - 1; z++)
                            {
                                if (dataTableSelec.GetValue("numChasisS", i).Equals(dataTableEven.GetValue("numChasisE", z)))
                                {
                                    existe = true;
                                    break;
                                }
                            }

                            if (existe == false)
                            {
                                
                                dataTableEven.Rows.Add();
                                dataTableEven.SetValue("numChasisE", IndexDataTableE,
                                                        dataTableSelec.GetValue("numChasisS", i).ToString().Trim());
                                dataTableEven.SetValue("numMotorE", IndexDataTableE, dataTableSelec.GetValue("numMotorS", i).ToString().Trim());
                                dataTableEven.SetValue("estiloE", IndexDataTableE, dataTableSelec.GetValue("estiloS", i).ToString().Trim());

                                dataTableEven.SetValue("marcaE", IndexDataTableE, dataTableSelec.GetValue("marcaS", i).ToString().Trim());
                                dataTableEven.SetValue("modeloE", IndexDataTableE, dataTableSelec.GetValue("modeloS", i).ToString().Trim());
                                dataTableEven.SetValue("colorE", IndexDataTableE, dataTableSelec.GetValue("colorS", i).ToString().Trim());
                                dataTableEven.SetValue("annoE", IndexDataTableE, dataTableSelec.GetValue("annoS", i).ToString().Trim());
                                dataTableEven.SetValue("unidadE", IndexDataTableE, dataTableSelec.GetValue("unidadS", i).ToString().Trim());
                                dataTableEven.SetValue("contVentaE", IndexDataTableE, dataTableSelec.GetValue("contVentaS", i).ToString().Trim());
                                dataTableEven.SetValue("numFactE", IndexDataTableE, dataTableSelec.GetValue("numFactS", i).ToString().Trim());
                                dataTableEven.SetValue("fechaEventoE", IndexDataTableE, DateTime.Now.ToString("yyyyMMdd"));

                                IndexDataTableE += 1;
                                
                            }
                        }
                    }
                }

                if (IndexDataTableE > 0)
                {
                    matrixEve.Matrix.LoadFromDataSource();

                    ComboBox sboCombo;

                    for (int i = 1; i <= matrixEve.Matrix.RowCount; i++)
                    {
                        string instanciaFin =  dataTableEven.GetValue("instFinanE", i - 1).ToString();

                        if(string.IsNullOrEmpty(instanciaFin))
                        {
                            sboCombo = (ComboBox)MatrixEventosGrupo.Matrix.Columns.Item("col_InsFin").Cells.Item(i).Specific;
                            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select BankCode, BankName from [ODSC]", Conexion);
                        }
                    }
                }

                else if (IndexDataTableE == 0)
                {
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else
            {
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        public void CargarMatrixGastos(MatrixSBO matrixSele, MatrixSBO matrixGast, DataTable dataTableSelec, DataTable dataTableGasto)
        {
            matrixGast.Matrix.FlushToDataSource();

            int tamanoS = matrixSele.Matrix.RowCount;

            if (tamanoS > 0)
            {
                matrixSele.Matrix.FlushToDataSource();

                int tamanoDTG = dataTableGasto.Rows.Count;

                //Valida que el datatable no contenga un campo vacio, con el fin de no borrar los datos de la matrix al agregar un nuevo vehículo
                if (tamanoDTG == 1 && string.IsNullOrEmpty(dataTableGasto.GetValue("numChasisG", 0).ToString()))
                {
                    dataTableGasto.Rows.Remove(0);
                    tamanoDTG = 0;
                }

                for (int i = 0; i <= tamanoS - 1; i++)
                {
                    string seleccion =
                        dataTableSelec.GetValue("seleccionS", i).ToString().Trim();
                    if (seleccion.Equals("Y"))
                    {
                        if (tamanoDTG == 0)
                        {
                            dataTableGasto.Rows.Add();
                            dataTableGasto.SetValue("numChasisG", IndexDataTableG,
                                                   dataTableSelec.GetValue("numChasisS", i).ToString().Trim());
                            dataTableGasto.SetValue("numMotorG", IndexDataTableG, dataTableSelec.GetValue("numMotorS", i).ToString().Trim());
                            dataTableGasto.SetValue("estiloG", IndexDataTableG, dataTableSelec.GetValue("estiloS", i).ToString().Trim());

                            dataTableGasto.SetValue("marcaG", IndexDataTableG, dataTableSelec.GetValue("marcaS", i).ToString().Trim());
                            dataTableGasto.SetValue("modeloG", IndexDataTableG, dataTableSelec.GetValue("modeloS", i).ToString().Trim());
                            dataTableGasto.SetValue("colorG", IndexDataTableG, dataTableSelec.GetValue("colorS", i).ToString().Trim());
                            dataTableGasto.SetValue("annoG", IndexDataTableG, dataTableSelec.GetValue("annoS", i).ToString().Trim());
                            dataTableGasto.SetValue("unidadG", IndexDataTableG, dataTableSelec.GetValue("unidadS", i).ToString().Trim());
                            dataTableGasto.SetValue("contVentaG", IndexDataTableG, dataTableSelec.GetValue("contVentaS", i).ToString().Trim());
                            dataTableGasto.SetValue("numFactG", IndexDataTableG, dataTableSelec.GetValue("numFactS", i).ToString().Trim());
                            dataTableGasto.SetValue("fechaDocumG", IndexDataTableG, DateTime.Now.ToString("yyyyMMdd"));

                            IndexDataTableG += 1;
                        }

                        else if (tamanoDTG > 0)
                        {
                            bool existe = false;

                            for (int z = 0; z <= tamanoDTG - 1; z++)
                            {
                                if (dataTableSelec.GetValue("numChasisS", i).Equals(dataTableGasto.GetValue("numChasisG", z)))
                                {
                                    existe = true;
                                    break;
                                }
                            }

                            if (existe == false)
                            {

                                dataTableGasto.Rows.Add();
                                dataTableGasto.SetValue("numChasisG", IndexDataTableG,
                                                        dataTableSelec.GetValue("numChasisS", i).ToString().Trim());
                                dataTableGasto.SetValue("numMotorG", IndexDataTableG, dataTableSelec.GetValue("numMotorS", i).ToString().Trim());
                                dataTableGasto.SetValue("estiloG", IndexDataTableG, dataTableSelec.GetValue("estiloS", i).ToString().Trim());

                                dataTableGasto.SetValue("marcaG", IndexDataTableG, dataTableSelec.GetValue("marcaS", i).ToString().Trim());
                                dataTableGasto.SetValue("modeloG", IndexDataTableG, dataTableSelec.GetValue("modeloS", i).ToString().Trim());
                                dataTableGasto.SetValue("colorG", IndexDataTableG, dataTableSelec.GetValue("colorS", i).ToString().Trim());
                                dataTableGasto.SetValue("annoG", IndexDataTableG, dataTableSelec.GetValue("annoS", i).ToString().Trim());
                                dataTableGasto.SetValue("unidadG", IndexDataTableG, dataTableSelec.GetValue("unidadS", i).ToString().Trim());
                                dataTableGasto.SetValue("contVentaG", IndexDataTableG, dataTableSelec.GetValue("contVentaS", i).ToString().Trim());
                                dataTableGasto.SetValue("numFactG", IndexDataTableG, dataTableSelec.GetValue("numFactS", i).ToString().Trim());
                                dataTableGasto.SetValue("fechaDocumG", IndexDataTableG, DateTime.Now.ToString("yyyyMMdd"));

                                IndexDataTableG += 1;

                            }
                        }
                    }
                }

                if (IndexDataTableG > 0)
                {
                    matrixGast.Matrix.LoadFromDataSource();

                    ComboBox sboCombo;

                    for (int i = 1; i <= matrixGast.Matrix.RowCount; i++)
                    {
                        string tipoGasto = dataTableGasto.GetValue("tipoGastoG", i - 1).ToString();

                        if (string.IsNullOrEmpty(tipoGasto))
                        {
                            sboCombo = (ComboBox)MatrixGastosGrupo.Matrix.Columns.Item("col_TipoG").Cells.Item(i).Specific;
                            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GASTOS]", Conexion);
                        }
                    }
                }

                else if (IndexDataTableG == 0)
                {
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            else
            {
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.SeleccionVehiculos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        /**
         * Método que se encarga de asignar un dato a uan columna en todos los campos del data table
         */
        public void AsignarDatoDatatable(DataTable dataTable, string columna,string dato)
        {
            int tamanoDT = dataTable.Rows.Count;

            if(tamanoDT > 0)
            {
                for (int i = 0; i <= tamanoDT - 1; i++)
                {
                    dataTable.SetValue(columna, i, dato);
                }
            }
        }

        public void AsignarDatosDatatable(DataTable dataTable, string[] columnas, string[] datos)
        {
            int tamanoDT = dataTable.Rows.Count;

            if (tamanoDT > 0)
            {
                int acolumnas = columnas.Count();
                int adatos = datos.Count();

                if(acolumnas == adatos)
                {
                    for (int i = 0; i <= tamanoDT - 1; i++)
                    {
                        for (int j = 0; j <= acolumnas - 1; j++)
                        {
                            dataTable.SetValue(columnas[j], i, datos[j]);   
                        }
                        
                    }
                }
            }
        }

        /**
         * Método que se encarga de ocultar o mostrar diferentes campos de la matrix dependiendo del tipo del evento seleccionado 
         */
        public void VisualizarCamposMatrix(ItemEvent pval, MatrixSBO matrix)
        {
            if(pval.BeforeAction == false && pval.ActionSuccess)
            {
                string codigoGestion = ComboBoxGestionE.ObtieneValorUserDataSource();
                codigoGestion = codigoGestion.Trim();
                string codigoSegimiento = General.EjecutarConsulta(string.Format("Select U_Seguimiento from [@SCGD_GESTION] where Code = {0}", codigoGestion), Conexion);

                // 1 simboliza que es de tipo Revisión vehicular
                if (codigoSegimiento.Equals("1"))
                {
                    matrix.Matrix.Columns.Item("col_Prenda").Width = 0;
                    matrix.Matrix.Columns.Item("col_InsFin").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef3").Width = 100;
                    matrix.Matrix.Columns.Item("col_NRef4").Width = 100;
                    matrix.Matrix.Columns.Item("col_NRef5").Width = 100;
                    matrix.Matrix.Columns.Item("col_NRef6").Width = 100;
                    matrix.Matrix.Columns.Item("col_FechIn").Width = 100;
                }

                else if (codigoSegimiento.Equals("2"))
                {
                    matrix.Matrix.Columns.Item("col_Prenda").Width = 80;
                    matrix.Matrix.Columns.Item("col_InsFin").Width = 125;
                    matrix.Matrix.Columns.Item("col_NRef3").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef4").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef5").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef6").Width = 0;
                    matrix.Matrix.Columns.Item("col_FechIn").Width = 0;
                }

                else if (codigoSegimiento.Equals("3"))
                {
                    matrix.Matrix.Columns.Item("col_Prenda").Width = 0;
                    matrix.Matrix.Columns.Item("col_InsFin").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef3").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef4").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef5").Width = 0;
                    matrix.Matrix.Columns.Item("col_NRef6").Width = 0;
                    matrix.Matrix.Columns.Item("col_FechIn").Width = 0;
                }
            }
        }

        /**
         * Método que se encarga de borrar un eletemento de un data table
         */
        public bool BorrarElementoMatrix(MatrixSBO matrix,DataTable dataTable, int index)
        {
            bool resultado = false;
            
            if(index > 0)
            {
                dataTable.Rows.Remove(index - 1);
                resultado = true;
            }

            return resultado;
        }

        /**
         * Método que se encarga de crear un expediente si la unidad no lo tiene, o agregar el evento si la misma unidad tiene expediente creado
         */
        public string[] AplicarEvento()
        {

            Company m_oCompany = (Company) CompanySBO;
            UDOPlacas udoPlacas;
            EncabezadoUDOPlacas encabezadoPlacas;
            RevisionVehicularUDOPlacas revisionVehicular;
            DocumentosLegalesUDOPlacas documentosLegales;
            InscripcionUDOPlacas inscripcion;
            GastosInscripcionUDOPlacas gastos;

            string codCliente;
            string nombCliente;
            string unidad;
            string chasis;
            string motor;
            string marca;
            string estilo;
            string modelo;
            string color;
            string anno;
            string contratoVenta;
            string numFactura;
            string sucursal;
            //string codigoSucursal;
            string docEntryE;
            string codigoGestion;
            string codigoEvento;
            string descripGestion;
            string descripEvento;
            string tipoSeguimiento;
            DateTime fechaEvento;
            string numeroRef1;
            string numeroRef2;
            string observacion;
            string estadoEvento;
            string codigoVehiculo;
            string usuarioSBO;
            string[] expedientesArray;

            try
            {
                int tamannoDT = DataTableEventos.Rows.Count;

                expedientesArray = new string[tamannoDT];

                MatrixEventosGrupo.Matrix.FlushToDataSource();

                for (int i = 0; i <= tamannoDT - 1; i++)
                {
                    udoPlacas = new UDOPlacas(m_oCompany, "SCGD_PLACA");

                    encabezadoPlacas = new EncabezadoUDOPlacas();

                    udoPlacas.ListaRevisionVehicular = new ListaRevisionVehicularUDOPlacas();
                    udoPlacas.ListaRevisionVehicular.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaDocumentosLegales = new ListaDocumentosLegalesUDOPlacas();
                    udoPlacas.ListaDocumentosLegales.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaInscripcion = new ListaInscripcionUDOPlacas();
                    udoPlacas.ListaInscripcion.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaGastosInscripcion = new ListaGastosInscripcionUDOPlacas();
                    udoPlacas.ListaGastosInscripcion.LineasUDO = new List<ILineaUDO>();

                    unidad = DataTableEventos.GetValue("unidadE", i).ToString().Trim();
                    chasis = DataTableEventos.GetValue("numChasisE", i).ToString().Trim();
                    motor = DataTableEventos.GetValue("numMotorE", i).ToString().Trim();
                    marca = DataTableEventos.GetValue("marcaE", i).ToString().Trim();
                    estilo = DataTableEventos.GetValue("estiloE", i).ToString().Trim();
                    modelo = DataTableEventos.GetValue("modeloE", i).ToString().Trim();
                    color = DataTableEventos.GetValue("colorE", i).ToString().Trim();
                    anno = DataTableEventos.GetValue("annoE", i).ToString().Trim();
                    contratoVenta = DataTableEventos.GetValue("contVentaE", i).ToString().Trim();
                    numFactura = DataTableEventos.GetValue("numFactE", i).ToString().Trim();
                    codigoVehiculo = General.EjecutarConsulta(string.Format("Select Code from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'", unidad), Conexion);

                    sucursal = General.EjecutarConsulta(string.Format("select U_SlpName from [@SCGD_CVENTA] where DocNum = '{0}'", contratoVenta), Conexion);
                    //string codigoVendedor = General.EjecutarConsulta(string.Format("select U_FooVend from [@SCGD_CVENTA] where DocNum = '{0}'", numFactura), Conexion);
                    //codigoSucursal = General.EjecutarConsulta(string.Format("select U_SCGD_SucVta from OSLP where SlpCode = '{0}'", codigoVendedor), Conexion);
                    //sucursal = General.EjecutarConsulta(string.Format("select Name from [@SCGD_SUC_VENTA] where Code = '{0}'", codigoSucursal), Conexion);
                   
                    if(!string.IsNullOrEmpty(unidad))
                    {
                        codigoGestion = ComboBoxGestionE.ObtieneValorUserDataSource().Trim();
                        descripGestion = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GESTION] where Code = '{0}'", codigoGestion), Conexion);
                        codigoEvento = ComboBoxEventoE.ObtieneValorUserDataSource().Trim();
                        descripEvento = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);
                        tipoSeguimiento = General.EjecutarConsulta(string.Format("Select U_Seguimiento from [@SCGD_GESTION] where Code = '{0}'", codigoGestion), Conexion);
                        estadoEvento = General.EjecutarConsulta(string.Format("Select U_Estado from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);
                        fechaEvento = Convert.ToDateTime(DataTableEventos.GetValue("fechaEventoE", i).ToString().Trim());
                        numeroRef1 = DataTableEventos.GetValue("noRef1E", i).ToString().Trim();
                        numeroRef2 = DataTableEventos.GetValue("noRef2E", i).ToString().Trim();
                        observacion = DataTableEventos.GetValue("observE", i).ToString().Trim();

                        docEntryE = General.EjecutarConsulta(string.Format("select top(1) DocEntry from [@SCGD_PLACA] where U_Num_Unid= '{0}' order by DocEntry desc", unidad), Conexion);

                        usuarioSBO = ApplicationSBO.Company.UserName;
                        
                        string estadoExpediente = General.EjecutarConsulta(string.Format(
                            "select TOP(1) U_Finaliz  from [@SCGD_PLACA] where U_Num_Unid = '{0}' order by DocEntry desc", unidad),
                        Conexion);

                        //no existe expediente
                        if (string.IsNullOrEmpty(docEntryE) || (estadoExpediente.Equals("Y")))
                        {
                            //Valida si tiene un contrato de ventas asociado la unidad 
                            string resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("select top 1 OC.CardCode, OC.CardName from [@SCGD_CVENTA] as CV left outer join [@SCGD_VEHIXCONT] as VXC on CV.DocEntry = VXC.DocEntry inner join [@SCGD_VEHICULO] as VEH on VXC.U_Cod_Unid = VEH.U_Cod_Unid left outer join OCRD as OC on CV.U_CCl_Veh = OC.CardCode where VEH.U_Cod_Unid = '{0}' and CV.U_Reversa = 'N'  order by DocNum desc", unidad), Conexion);

                            //Carga el cliente a inscribir
                            if(!string.IsNullOrEmpty(resultado))
                            {
                                string[] resultadoArray = resultado.Split('@');

                                string[] parametros = resultadoArray[0].Split('*');

                                codCliente = parametros[1];
                                nombCliente = parametros[2];
                            }
                            
                            //Carga el cliente del vehículo
                            else
                            {
                                resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("Select U_CardCode, U_CardName from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'", unidad), Conexion);

                                string[] resultadoArray = resultado.Split('@');

                                string[] parametros = resultadoArray[0].Split('*');

                                codCliente = parametros[1];
                                nombCliente = parametros[2];
                            }
                            
                            encabezadoPlacas.CodigoCliente = codCliente;
                            encabezadoPlacas.NombreCliente = nombCliente;
                            encabezadoPlacas.NumeroUnidad = unidad;
                            encabezadoPlacas.NumeroVIN = chasis;
                            encabezadoPlacas.NumeroMotor = motor;
                            encabezadoPlacas.Marca = marca;
                            encabezadoPlacas.Estilo = estilo;
                            encabezadoPlacas.Modelo = modelo;
                            encabezadoPlacas.Color = color;
                            encabezadoPlacas.Anno = int.Parse(anno);
                            encabezadoPlacas.NumeroCV = contratoVenta;
                            encabezadoPlacas.NumeroFactura = numFactura;
                            //encabezadoPlacas.CodigoSucursal = codigoSucursal;
                            encabezadoPlacas.Sucursal = sucursal;

                            //valida si es el evento es catalogado como evento de finalización, si este lo es marca como finalizado el expedietne
                            string eventoFinalizacion = General.EjecutarConsulta(string.Format("select U_EvenFin from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);

                            if(eventoFinalizacion.Equals("Y"))
                            {
                                encabezadoPlacas.Finalizacion = "Y";
                            }

                            else if (!eventoFinalizacion.Equals("Y"))
                            {
                                encabezadoPlacas.Finalizacion = "N";
                            }

                            udoPlacas.encabezado = encabezadoPlacas;

                           
                            if (tipoSeguimiento.Equals("1"))
                            {
                                string numeroRef3 = DataTableEventos.GetValue("noRef3E", i).ToString().Trim();
                                string numeroRef4 = DataTableEventos.GetValue("noRef4E", i).ToString().Trim();
                                string numeroRef5 = DataTableEventos.GetValue("noRef5E", i).ToString().Trim();
                                string numeroRef6 = DataTableEventos.GetValue("noRef6E", i).ToString().Trim();

                                string fechaIngreso;

                                try
                                {
                                    fechaIngreso = DataTableEventos.GetValue("fechaIngresoE", i).ToString().Trim();
                                }
                                catch
                                {
                                    fechaIngreso = "";
                                }

                                revisionVehicular = new RevisionVehicularUDOPlacas();
                                revisionVehicular.Gestion = descripGestion;
                                revisionVehicular.CodigoGestion = codigoGestion;
                                revisionVehicular.Evento = descripEvento;
                                revisionVehicular.CodigoEvento = codigoEvento;
                                revisionVehicular.FechaEvento = fechaEvento;
                                revisionVehicular.NumeroReferencia1 = numeroRef1;
                                revisionVehicular.NumeroReferencia2 = numeroRef2;
                                revisionVehicular.NumeroReferencia3 = numeroRef3;
                                revisionVehicular.NumeroReferencia4 = numeroRef4;
                                revisionVehicular.NumeroReferencia5 = numeroRef5;
                                revisionVehicular.NumeroReferencia6 = numeroRef6;
                                revisionVehicular.FechaIngreso = fechaIngreso;
                                revisionVehicular.Observacion = observacion;
                                revisionVehicular.FechaCreacion = DateTime.Now;
                                revisionVehicular.UsuarioIngresa = usuarioSBO;

                                udoPlacas.ListaRevisionVehicular.LineasUDO.Add(revisionVehicular);

                               //Líneas vacías agregadas para que no presente el error en el UDO por las tablas hijas

                                documentosLegales = new DocumentosLegalesUDOPlacas();
                                documentosLegales.Prenda = "N";
                                documentosLegales.NumeroReferencia1 = " ";
                                udoPlacas.ListaDocumentosLegales.LineasUDO.Add(documentosLegales);
                                
                                inscripcion = new InscripcionUDOPlacas();
                                inscripcion.NumeroReferencia1 = " ";
                                udoPlacas.ListaInscripcion.LineasUDO.Add(inscripcion);

                                gastos = new GastosInscripcionUDOPlacas();
                                gastos.Observacion = " ";
                                udoPlacas.ListaGastosInscripcion.LineasUDO.Add(gastos);

                            }

                            else if (tipoSeguimiento.Equals("2"))
                            {
                                string prenda = DataTableEventos.GetValue("prendaE", i).ToString().Trim();
                                string instanciaF = DataTableEventos.GetValue("instFinanE", i).ToString().Trim();

                                if(string.IsNullOrEmpty(prenda))
                                {
                                    prenda = "N";
                                }

                                documentosLegales = new DocumentosLegalesUDOPlacas();
                                documentosLegales.Gestion = descripGestion;
                                documentosLegales.CodigoGestion = codigoGestion;
                                documentosLegales.Evento = descripEvento;
                                documentosLegales.CodigoEvento = codigoEvento;
                                documentosLegales.FechaEvento = fechaEvento;
                                documentosLegales.Prenda = prenda;
                                documentosLegales.InstanciaFinanciera = instanciaF;
                                documentosLegales.NumeroReferencia1 = numeroRef1;
                                documentosLegales.NumeroReferencia2 = numeroRef2;
                                documentosLegales.Observacion = observacion;
                                documentosLegales.FechaCreacion = DateTime.Now;
                                documentosLegales.UsuarioIngresa = usuarioSBO;

                                udoPlacas.ListaDocumentosLegales.LineasUDO.Add(documentosLegales);

                                //Líneas vacías agregadas para que no presente el error en el UDO por las tablas hijas

                                revisionVehicular = new RevisionVehicularUDOPlacas();
                                revisionVehicular.NumeroReferencia1 = " ";
                                udoPlacas.ListaRevisionVehicular.LineasUDO.Add(revisionVehicular);

                                inscripcion = new InscripcionUDOPlacas();
                                inscripcion.NumeroReferencia1 = " ";
                                udoPlacas.ListaInscripcion.LineasUDO.Add(inscripcion);

                                gastos = new GastosInscripcionUDOPlacas();
                                gastos.Observacion = " ";
                                udoPlacas.ListaGastosInscripcion.LineasUDO.Add(gastos);
                            }

                            else if (tipoSeguimiento.Equals("3"))
                            {
                                inscripcion = new InscripcionUDOPlacas();
                                inscripcion.Gestion = descripGestion;
                                inscripcion.CodigoGestion = codigoGestion;
                                inscripcion.Evento = descripEvento;
                                inscripcion.CodigoEvento = codigoEvento;
                                inscripcion.FechaEvento = fechaEvento;
                                inscripcion.NumeroReferencia1 = numeroRef1;
                                inscripcion.NumeroReferencia2 = numeroRef2;
                                inscripcion.Observacion = observacion;
                                inscripcion.FechaCreacion = DateTime.Now;
                                inscripcion.UsuarioIngresa = usuarioSBO;

                                udoPlacas.ListaInscripcion.LineasUDO.Add(inscripcion);

                                //Líneas vacías agregadas para que no presente el error en el UDO por las tablas hijas

                                revisionVehicular = new RevisionVehicularUDOPlacas();
                                revisionVehicular.NumeroReferencia1 = " ";
                                udoPlacas.ListaRevisionVehicular.LineasUDO.Add(revisionVehicular);

                                documentosLegales = new DocumentosLegalesUDOPlacas();
                                documentosLegales.Prenda = "N";
                                documentosLegales.NumeroReferencia1 = " ";
                                udoPlacas.ListaDocumentosLegales.LineasUDO.Add(documentosLegales);

                                gastos = new GastosInscripcionUDOPlacas();
                                gastos.Observacion = " ";
                                udoPlacas.ListaGastosInscripcion.LineasUDO.Add(gastos);
                            }

                            udoPlacas.Insert();

                            string numeroExpediente = udoPlacas.encabezado.DocEntry.ToString();
                            expedientesArray[i] = numeroExpediente;

                            if(!string.IsNullOrEmpty(estadoEvento))
                            {
                                CompanyService companyService;
                                GeneralService generalService;
                                GeneralData generalData;
                                GeneralDataParams generalDataParams;

                                companyService = m_oCompany.GetCompanyService();
                                generalService = companyService.GetGeneralService("SCGD_VEH");
                                generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                generalDataParams.SetProperty("Code", codigoVehiculo);
                                generalData = generalService.GetByParams(generalDataParams);

                                generalData.SetProperty("U_Estatus", estadoEvento);
                                generalService.Update(generalData);

                                if (udoPlacas.LastErrorCode != 0)
                                {
                                    if (m_oCompany.InTransaction)
                                    {
                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                    }
                                }
                            }
                        }

                            //Si existe el expediente, solo crea las lineas en la tabla hija respectiva
                        else
                        {
                            CompanyService companyService;
                            GeneralService generalService;
                            GeneralData generalData;
                            GeneralDataParams generalDataParams;

                            GeneralData child = null;
                            GeneralDataCollection children;

                            companyService = m_oCompany.GetCompanyService();
                            generalService = companyService.GetGeneralService("SCGD_PLACA");
                            generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                            generalDataParams.SetProperty("DocEntry", docEntryE);
                            generalData = generalService.GetByParams(generalDataParams);

                            string eventoFinalizacion = General.EjecutarConsulta(string.Format("select U_EvenFin from [@SCGD_EVENTO] where Code = '{0}'", codigoEvento), Conexion);

                            if (eventoFinalizacion.Equals("Y"))
                            {
                                generalData.SetProperty("U_Finaliz", "Y");
                            }

                            else if (!eventoFinalizacion.Equals("Y"))
                            {
                                generalData.SetProperty("U_Finaliz", "N");
                            }

                            if (tipoSeguimiento.Equals("1"))
                            {
                                string tamañoTabla = General.EjecutarConsulta(string.Format("Select COUNT(*) From [@SCGD_REV_VEH] Where DocEntry = {0}", docEntryE), Conexion);
                                int intTamaño = int.Parse(tamañoTabla);
                                string validaEvento = General.EjecutarConsulta(string.Format("Select TOP 1 U_Evento From [@SCGD_REV_VEH] Where DocEntry = {0} Order By LineId", docEntryE), Conexion);

                                children = generalData.Child("SCGD_REV_VEH");

                                if (intTamaño != 1 || !string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Add();
                                }

                                else if (intTamaño == 1 && string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Item(0);
                                }

                                string numeroRef3 = DataTableEventos.GetValue("noRef3E", i).ToString().Trim();
                                string numeroRef4 = DataTableEventos.GetValue("noRef4E", i).ToString().Trim();
                                string numeroRef5 = DataTableEventos.GetValue("noRef5E", i).ToString().Trim();
                                string numeroRef6 = DataTableEventos.GetValue("noRef6E", i).ToString().Trim();

                                string fechaIngreso;

                                try
                                {
                                    fechaIngreso = DataTableEventos.GetValue("fechaIngresoE", i).ToString().Trim();
                                }
                                catch
                                {
                                    fechaIngreso = "";
                                }

                                child.SetProperty("U_Gestion", descripGestion);
                                child.SetProperty("U_Evento", descripEvento);
                                child.SetProperty("U_Fech_Ev", fechaEvento);
                                child.SetProperty("U_Num_Ref1", numeroRef1);
                                child.SetProperty("U_Num_Ref2", numeroRef2);
                                child.SetProperty("U_Num_Ref3", numeroRef3);
                                child.SetProperty("U_Num_Ref4", numeroRef4);
                                child.SetProperty("U_Num_Ref5", numeroRef5);
                                child.SetProperty("U_Num_Ref6", numeroRef6);
                                child.SetProperty("U_Fech_In", fechaIngreso);
                                child.SetProperty("U_Observ", observacion);
                                child.SetProperty("U_Fech_Cre", DateTime.Now);
                                child.SetProperty("U_Ingresa", usuarioSBO);
                                child.SetProperty("U_Cod_Ges", codigoGestion);
                                child.SetProperty("U_Cod_Eve", codigoEvento);
                            }

                            else if (tipoSeguimiento.Equals("2"))
                            {
                                string tamañoTabla = General.EjecutarConsulta(string.Format("Select COUNT(*) From [@SCGD_DOC_LEG] Where DocEntry = {0}", docEntryE), Conexion);
                                int intTamaño = int.Parse(tamañoTabla);
                                string validaEvento = General.EjecutarConsulta(string.Format("Select TOP 1 U_Evento From [@SCGD_DOC_LEG] Where DocEntry = {0} Order By LineId", docEntryE), Conexion);

                                children = generalData.Child("SCGD_DOC_LEG");

                                if (intTamaño != 1 || !string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Add();
                                }

                                else if (intTamaño == 1 && string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Item(0);
                                }

                                string prenda = DataTableEventos.GetValue("prendaE", i).ToString().Trim();
                                string instanciaF = DataTableEventos.GetValue("instFinanE", i).ToString().Trim();

                                if (string.IsNullOrEmpty(prenda))
                                {
                                    prenda = "N";
                                }

                                child.SetProperty("U_Gestion", descripGestion);
                                child.SetProperty("U_Evento", descripEvento);
                                child.SetProperty("U_Fech_Ev", fechaEvento);
                                child.SetProperty("U_Prenda", prenda);
                                child.SetProperty("U_Ins_Fin", instanciaF);
                                child.SetProperty("U_Num_Ref1", numeroRef1);
                                child.SetProperty("U_Num_Ref2", numeroRef2);
                                child.SetProperty("U_Observ", observacion);
                                child.SetProperty("U_Fech_Cre", DateTime.Now);
                                child.SetProperty("U_Ingresa", usuarioSBO);
                                child.SetProperty("U_Cod_Ges", codigoGestion);
                                child.SetProperty("U_Cod_Eve", codigoEvento);
                            }

                            else if (tipoSeguimiento.Equals("3"))
                            {
                                string tamañoTabla = General.EjecutarConsulta(string.Format("Select COUNT(*) From [@SCGD_INSCRIP] Where DocEntry = {0}", docEntryE), Conexion);
                                int intTamaño = int.Parse(tamañoTabla);
                                string validaEvento = General.EjecutarConsulta(string.Format("Select TOP 1 U_Evento From [@SCGD_INSCRIP] Where DocEntry = {0} Order By LineId", docEntryE), Conexion);

                                children = generalData.Child("SCGD_INSCRIP");

                                if (intTamaño != 1 || !string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Add();
                                }

                                else if (intTamaño == 1 && string.IsNullOrEmpty(validaEvento))
                                {
                                    child = children.Item(0);
                                }

                                child.SetProperty("U_Gestion", descripGestion);
                                child.SetProperty("U_Evento", descripEvento);
                                child.SetProperty("U_Fech_Ev", fechaEvento);
                                child.SetProperty("U_Num_Ref1", numeroRef1);
                                child.SetProperty("U_Num_Ref2", numeroRef2);
                                child.SetProperty("U_Observ", observacion);
                                child.SetProperty("U_Fech_Cre", DateTime.Now);//.ToString("yyyyMMdd")
                                child.SetProperty("U_Ingresa", usuarioSBO);
                                child.SetProperty("U_Cod_Ges", codigoGestion);
                                child.SetProperty("U_Cod_Eve", codigoEvento);
                            }

                            generalService.Update(generalData);

                            expedientesArray[i] = docEntryE;

                            if (!string.IsNullOrEmpty(estadoEvento))
                            {
                                companyService = m_oCompany.GetCompanyService();
                                generalService = companyService.GetGeneralService("SCGD_VEH");
                                generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                generalDataParams.SetProperty("Code", codigoVehiculo);
                                generalData = generalService.GetByParams(generalDataParams);
                                generalData.SetProperty("U_Estatus", estadoEvento);
                                generalService.Update(generalData);
                            }
                        }
                    }
                }
            }

            catch (Exception)
            {
                if (m_oCompany.InTransaction)
                {
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }

                throw;
            }

            return expedientesArray;
        }
        
        /**
         * Método que se encarga de crear un expediente si la unidad no posee, o agregar el gasto a la unidad si el expediente fue creado, devuelve la cantidad de unidades que se les asignaron gastos
         */
        public int AplicarGastos(ref string[] expedientesGastosArray)
        {
            Company m_oCompany = (Company)CompanySBO;
            UDOPlacas udoPlacas;
            EncabezadoUDOPlacas encabezadoPlacas;
            RevisionVehicularUDOPlacas revisionVehicular;
            DocumentosLegalesUDOPlacas documentosLegales;
            InscripcionUDOPlacas inscripcion;
            GastosInscripcionUDOPlacas gastos;

            int gastosAsignados = 0;
            string codCliente;
            string nombCliente;
            string unidad;
            string chasis;
            string motor;
            string marca;
            string estilo;
            string modelo;
            string color;
            string anno;
            string contratoVenta;
            string numFactura;
            string sucursal;
            //string codigoSucursal;
            string docEntryE;
            string codigoGasto;
            string descripGasto;
            DateTime fechaDoc=new DateTime();
            string numeroDoc;
            string monto;
            string observacion;
            string usuarioSBO;
            //string[] expedientesGastosArray;

            try
            {

                int tamannoDT = DataTableGastos.Rows.Count;

                expedientesGastosArray = new string[tamannoDT];

                MatrixGastosGrupo.Matrix.FlushToDataSource();

                for (int i = 0; i <= tamannoDT - 1; i++)
                {
                    udoPlacas = new UDOPlacas(m_oCompany, "SCGD_PLACA");

                    encabezadoPlacas = new EncabezadoUDOPlacas();

                    udoPlacas.ListaRevisionVehicular = new ListaRevisionVehicularUDOPlacas();
                    udoPlacas.ListaRevisionVehicular.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaDocumentosLegales = new ListaDocumentosLegalesUDOPlacas();
                    udoPlacas.ListaDocumentosLegales.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaInscripcion = new ListaInscripcionUDOPlacas();
                    udoPlacas.ListaInscripcion.LineasUDO = new List<ILineaUDO>();

                    udoPlacas.ListaGastosInscripcion = new ListaGastosInscripcionUDOPlacas();
                    udoPlacas.ListaGastosInscripcion.LineasUDO = new List<ILineaUDO>();


                    unidad = DataTableGastos.GetValue("unidadG", i).ToString().Trim();
                    chasis = DataTableGastos.GetValue("numChasisG", i).ToString().Trim();
                    motor = DataTableGastos.GetValue("numMotorG", i).ToString().Trim();
                    marca = DataTableGastos.GetValue("marcaG", i).ToString().Trim();
                    estilo = DataTableGastos.GetValue("estiloG", i).ToString().Trim();
                    modelo = DataTableGastos.GetValue("modeloG", i).ToString().Trim();
                    color = DataTableGastos.GetValue("colorG", i).ToString().Trim();
                    anno = DataTableGastos.GetValue("annoG", i).ToString().Trim();
                    contratoVenta = DataTableGastos.GetValue("contVentaG", i).ToString().Trim();
                    numFactura = DataTableGastos.GetValue("numFactG", i).ToString().Trim();

                    sucursal = General.EjecutarConsulta(string.Format("select U_SlpName from [@SCGD_CVENTA] where DocNum = '{0}'", contratoVenta), Conexion);

                    //string codigoVendedor = General.EjecutarConsulta(string.Format("select U_FooVend from [@SCGD_CVENTA] where DocNum = '{0}'", numFactura), Conexion);
                    //codigoSucursal = General.EjecutarConsulta(string.Format("select U_SCGD_SucVta from OSLP where SlpCode = '{0}'", codigoVendedor), Conexion);
                    //sucursal = General.EjecutarConsulta(string.Format("select Name from [@SCGD_SUC_VENTA] where Code = '{0}'", codigoSucursal), Conexion);


                    if (!string.IsNullOrEmpty(unidad))
                    {
                        codigoGasto = DataTableGastos.GetValue("tipoGastoG", i).ToString().Trim();
                        descripGasto = General.EjecutarConsulta(string.Format("Select U_Descrip from [@SCGD_GASTOS] where code = '{0}'", codigoGasto), Conexion);
                        
                        if (!string.IsNullOrEmpty(codigoGasto))
                        {
                            numeroDoc = DataTableGastos.GetValue("numDocumG", i).ToString().Trim();
                            string strFecha = DataTableGastos.GetValue("fechaDocumG", i).ToString();
                            
                            if (!String.IsNullOrEmpty(strFecha))
                            {
                                fechaDoc = Convert.ToDateTime(strFecha);
                            }
                            
                            monto = DataTableGastos.GetValue("montoG", i).ToString().Trim();
                            observacion = DataTableGastos.GetValue("observG", i).ToString().Trim();

                            docEntryE = General.EjecutarConsulta(string.Format("select top(1) DocEntry from [@SCGD_PLACA] where U_Num_Unid= '{0}' order by DocEntry desc", unidad), Conexion);

                            usuarioSBO = ApplicationSBO.Company.UserName;
                            
                            string estadoExpediente = General.EjecutarConsulta(string.Format(
                            "select TOP(1) U_Finaliz  from [@SCGD_PLACA] where U_Num_Unid = '{0}' order by DocEntry desc", unidad),
                        Conexion);

                            //no existe expediente
                            if (string.IsNullOrEmpty(docEntryE) || (estadoExpediente.Equals("Y")))
                            {
                                //Valida si tiene un contrato de ventas asociado la unidad 
                                string resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("Select top 1 OC.CardCode, OC.CardName from [@SCGD_CVENTA] as CV left outer join [@SCGD_VEHIXCONT] as VXC on CV.DocEntry = VXC.DocEntry inner join [@SCGD_VEHICULO] as VEH on VXC.U_Cod_Unid = VEH.U_Cod_Unid left outer join OCRD as OC on CV.U_CCl_Veh = OC.CardCode where VEH.U_Cod_Unid = '{0}' and CV.U_Reversa = 'N'  order by DocNum desc", unidad), Conexion);

                                //Carga el cliente a inscribir
                                if (!string.IsNullOrEmpty(resultado))
                                {
                                    string[] resultadoArray = resultado.Split('@');

                                    string[] parametros = resultadoArray[0].Split('*');

                                    codCliente = parametros[1];
                                    nombCliente = parametros[2];
                                }

                                //Carga el cliente del vehículo
                                else
                                {
                                    resultado = General.EjecutarConsultaMultipleResultadosFilasColumnas(string.Format("Select U_CardCode, U_CardName from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'", unidad), Conexion);

                                    string[] resultadoArray = resultado.Split('@');

                                    string[] parametros = resultadoArray[0].Split('*');

                                    codCliente = parametros[1];
                                    nombCliente = parametros[2];
                                }

                                encabezadoPlacas.CodigoCliente = codCliente;
                                encabezadoPlacas.NombreCliente = nombCliente;
                                encabezadoPlacas.NumeroUnidad = unidad;
                                encabezadoPlacas.NumeroVIN = chasis;
                                encabezadoPlacas.NumeroMotor = motor;
                                encabezadoPlacas.Marca = marca;
                                encabezadoPlacas.Estilo = estilo;
                                encabezadoPlacas.Modelo = modelo;
                                encabezadoPlacas.Color = color;
                                encabezadoPlacas.Anno = int.Parse(anno);
                                encabezadoPlacas.NumeroCV = contratoVenta;
                                encabezadoPlacas.NumeroFactura = numFactura;
                                //encabezadoPlacas.CodigoSucursal = codigoSucursal;
                                encabezadoPlacas.Sucursal = sucursal;
                                encabezadoPlacas.Finalizacion = "N";
                                
                                encabezadoPlacas.Total = monto;
                                
                                udoPlacas.encabezado = encabezadoPlacas;

                                gastos = new GastosInscripcionUDOPlacas();
                                gastos.Gasto = descripGasto;
                                gastos.CodigoGasto = codigoGasto;
                                gastos.NumeroDocumento = numeroDoc;
                                if (!String.IsNullOrEmpty(strFecha))
                                {
                                    gastos.FechaDocumento = fechaDoc;
                                }
                                gastos.Monto = monto;
                                gastos.Observacion = observacion;
                                gastos.FechaCreacion = DateTime.Now;
                                gastos.UsuarioIngresa = usuarioSBO;

                                udoPlacas.ListaGastosInscripcion.LineasUDO.Add(gastos);

                                //Líneas vacías agregadas para que no presente el error en el UDO por las tablas hijas

                                revisionVehicular = new RevisionVehicularUDOPlacas();
                                revisionVehicular.NumeroReferencia1 = " ";
                                udoPlacas.ListaRevisionVehicular.LineasUDO.Add(revisionVehicular);

                                documentosLegales = new DocumentosLegalesUDOPlacas();
                                documentosLegales.Prenda = "N";
                                documentosLegales.NumeroReferencia1 = " ";
                                udoPlacas.ListaDocumentosLegales.LineasUDO.Add(documentosLegales);

                                inscripcion = new InscripcionUDOPlacas();
                                inscripcion.NumeroReferencia1 = " ";
                                udoPlacas.ListaInscripcion.LineasUDO.Add(inscripcion);

                                udoPlacas.Insert();

                                string numeroExpediente = udoPlacas.encabezado.DocEntry.ToString();
                                expedientesGastosArray[i] = numeroExpediente;

                                gastosAsignados = gastosAsignados + 1;

                                if (udoPlacas.LastErrorCode != 0)
                                {
                                    if (m_oCompany.InTransaction)
                                    {
                                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                    }

                                    gastosAsignados = gastosAsignados - 1;
                                }

                            }

                                //Si existe el expediente, solo crea las lineas en la tabla hija respectiva
                            else
                            {
                                NumberFormatInfo n = DIHelper.GetNumberFormatInfo(CompanySBO);

                                CompanyService companyService;
                                GeneralService generalService;
                                GeneralData generalData;
                                GeneralDataParams generalDataParams;

                                GeneralData child = null;
                                GeneralDataCollection children;

                                companyService = m_oCompany.GetCompanyService();
                                generalService = companyService.GetGeneralService("SCGD_PLACA");
                                generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                                generalDataParams.SetProperty("DocEntry", docEntryE);
                                generalData = generalService.GetByParams(generalDataParams);

                                string tamañoTabla = General.EjecutarConsulta(string.Format("Select COUNT(*) From [@SCGD_GAS_INS] Where DocEntry = {0}", docEntryE), Conexion);
                                int intTamaño = int.Parse(tamañoTabla);
                                string validaGasto = General.EjecutarConsulta(string.Format("Select TOP 1 U_Gasto From [@SCGD_GAS_INS] Where DocEntry = {0} Order By LineId", docEntryE), Conexion);
                                string validaExistencia =
                                    General.EjecutarConsulta(
                                        string.Format(
                                            "Select COUNT(*) from [@SCGD_PLACA] as PL inner join [@SCGD_GAS_INS] as GI on PL.DocEntry = GI.DocEntry where PL.DocEntry = '{0}' and GI.U_Cod_Gas = '{1}'",
                                            docEntryE, codigoGasto), Conexion);

                                int existencia = int.Parse(validaExistencia);

                                //No existe el gasto para esa unidad
                                if (existencia == 0)
                                {
                                    children = generalData.Child("SCGD_GAS_INS");

                                    if (intTamaño != 1 || !string.IsNullOrEmpty(validaGasto))
                                    {
                                        child = children.Add();
                                    }

                                    else if (intTamaño == 1 && string.IsNullOrEmpty(validaGasto))
                                    {
                                        child = children.Item(0);
                                    }


                                    string montoExpediente = generalData.GetProperty("U_Total").ToString();
                                    string separardorMilesSAP = "";
                                    string separadorDecimalesSAP = "";
                                    General.ObtenerSeparadoresNumerosSAP(ref separardorMilesSAP, ref separadorDecimalesSAP, Conexion);
                                    montoExpediente = General.CambiarValoresACultureActual(montoExpediente, separardorMilesSAP, separadorDecimalesSAP);
                                    decimal decMontoExpediente = decimal.Parse(montoExpediente, n);
                                    monto = General.CambiarValoresACultureActual(monto, separardorMilesSAP, separadorDecimalesSAP);
                                    decimal decMontoGasto = decimal.Parse(monto,n);
                                    decimal totalExpediente = decMontoExpediente + decMontoGasto;

                                    generalData.SetProperty("U_Total",totalExpediente.ToString(n));

                                    child.SetProperty("U_Gasto", descripGasto);
                                    child.SetProperty("U_Num_Doc", numeroDoc);
                                    child.SetProperty("U_Fech_Doc", fechaDoc);
                                    child.SetProperty("U_Monto", monto);
                                    child.SetProperty("U_Observ", observacion);
                                    child.SetProperty("U_Fech_Cre", DateTime.Now);
                                    child.SetProperty("U_Ingresa", usuarioSBO);
                                    child.SetProperty("U_Cod_Gas", codigoGasto);

                                    generalService.Update(generalData);

                                    expedientesGastosArray[i] = docEntryE;

                                    gastosAsignados = gastosAsignados + 1;
                                    
                                }

                                else if (existencia > 0)
                                {
                                    //Existe el gasto
                                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorExisteGasto + unidad, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                }
                            }
                        }

                        else if(string.IsNullOrEmpty(descripGasto))
                        {
                            //NO posee gasto asignado
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorFaltaTipoGasto, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
            }

            catch (Exception)
            {
                if (m_oCompany.InTransaction)
                {
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }

                throw;
            }

            return gastosAsignados;
        }

        /**
         * Métdo que se encarga de crear un grupo
         */
        public int CrearGrupo(string descGrupo, string monto)
        {
            Company m_oCompany = (Company)CompanySBO;
            UDOGrupoPlacas udoGrupo;
            EncabezadoUDOGrupoPlacas encabezadoGrupo;

            DateTime fechaSistema = System.DateTime.Now;
            
            udoGrupo = new UDOGrupoPlacas(m_oCompany, "SCGD_GRUPO_PLACA");
            encabezadoGrupo = new EncabezadoUDOGrupoPlacas();

            encabezadoGrupo.FechaGrupo = fechaSistema;
            encabezadoGrupo.DescGrupo = descGrupo;
            encabezadoGrupo.TotalGrupo = monto;

            udoGrupo.Encabezado = encabezadoGrupo;
            udoGrupo.Insert();

            if (udoGrupo.LastErrorCode != 0)
            {
                if (m_oCompany.InTransaction)
                {
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }

                return 0;
            }

            else
            {
                return udoGrupo.Encabezado.DocEntry;
            }
        }

        /**
         * Método que se encarga de insertar la relación del grupo creado con las unidades
         */
        public void CrearGrupoRelacion(int numeroGrupo, DataTable dataTable, string columna, string[] expedientes)
        {
            Company m_oCompany = (Company) CompanySBO;
            UDOGrupoPlacasRelacion udoGrupoRelacion;
            EncabezadoUDOGrupoPlacasRelacion encabezadoUdoGrupoRelacion;

            int tamañoDT = dataTable.Rows.Count;
            
            udoGrupoRelacion = new UDOGrupoPlacasRelacion(m_oCompany, "SCGD_GRUPO_REL");
            encabezadoUdoGrupoRelacion = new EncabezadoUDOGrupoPlacasRelacion();

            if(numeroGrupo != 0)
            {
                for (int i = 0; i <= tamañoDT - 1; i++)
                {
                    encabezadoUdoGrupoRelacion.NumeroGrupo = numeroGrupo;
                    encabezadoUdoGrupoRelacion.Unidad = dataTable.GetValue(columna, i).ToString().Trim();
                    encabezadoUdoGrupoRelacion.NumeroExpediente = expedientes[i];

                    udoGrupoRelacion.Encabezado = encabezadoUdoGrupoRelacion;
                    udoGrupoRelacion.Insert();

                    if (udoGrupoRelacion.LastErrorCode != 0)
                    {
                        if (m_oCompany.InTransaction)
                        {
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                        
                        break;
                    }
                }
            }
        }

        /**
         * Método que se encarga de buscar las unidades y cargarlas en la matrix de eventos. Esto para que pertenecen al grupo dado
         */
        public void BuscarGrupo(MatrixSBO matrix, DataTable dataTable, ref int indexDataTable, string[] columnas,DataTable dataTableConsulta, string [] columnasConsulta, string campoCombo, string columnaCombo, string consultaCombo,string idGrupo)
        {
            string consulta = "";

            if (!string.IsNullOrEmpty(idGrupo))
            {
                consulta = "Select PL.DocNum, PL.U_Num_Unid, PL.U_Num_VIN, PL.U_Num_Moto, PL.U_Marca, PL.U_Estilo, PL.U_Modelo, PL.U_Color, PL.U_Anno, PL.U_Num_CV, PL.U_Num_Fact from [@SCGD_GRUPO_REL] as GPR inner join [@SCGD_GRUPO_PLACA] as GP on GP.DocEntry = GPR.U_Num_GRUPO inner join [@SCGD_PLACA] as PL on GPR.U_Num_Exp = PL.DocEntry where GP.DocNum = ' " + idGrupo + " '";
            }

            matrix.Matrix.Clear();

            indexDataTable = 0;

            dataTableConsulta.Clear();
            dataTableConsulta.ExecuteQuery(consulta);

            int tamannoDT = dataTableConsulta.Rows.Count;

            int tamannoDTE = dataTable.Rows.Count;

            int tamannoArray = columnas.Count();

            if (tamannoDTE > 0)
            {
                LimpiarDataTable(dataTable, tamannoDTE);
            }


            for (int i = 0; i <= tamannoDT - 1; i++)
            {

                if (!string.IsNullOrEmpty(dataTableConsulta.GetValue(columnasConsulta[0], 0).ToString())) 
                {
                    dataTable.Rows.Add();

                    for (int j = 0; j <= tamannoArray - 1; j++)
                    {
                        if (columnas[j].Equals("fechaDocumG") || columnas[j].Equals("fechaEventoE"))
                        {
                            dataTable.SetValue(columnas[j], indexDataTable, DateTime.Now.ToString("yyyyMMdd"));
                        }

                        else
                        {
                            dataTable.SetValue(columnas[j], indexDataTable, dataTableConsulta.GetValue(columnasConsulta[j], i).ToString().Trim());
                        }

                    }

                    indexDataTable = indexDataTable + 1;
                }
                
            }
            
            matrix.Matrix.LoadFromDataSource();

            CargarValidValuesMatrix(matrix, dataTable, campoCombo, columnaCombo, consultaCombo);

        }


        public void CalcularGastos(MatrixSBO matrix, string columna)
        {
            int tamannoM = matrix.Matrix.RowCount;

            if (tamannoM > 0)
            {
                NumberFormatInfo n = DIHelper.GetNumberFormatInfo(CompanySBO);
                decimal montoAplicar = 0;


                for (int i = 1; i <= tamannoM; i++)
                {
                    string montoG = matrix.ObtieneValorColumnaEditText(columna, i);
                    decimal montoColumna = decimal.Parse(montoG, n);
                    montoAplicar = montoAplicar + montoColumna;
                }

                EditTextTotal.AsignaValorUserDataSource(montoAplicar.ToString(n));
            }
        }

        public void CargarValidValuesMatrix(MatrixSBO matrix, DataTable dataTable, string campoDataTable, string columnaMatrix, string consulta)
        {
            ComboBox sboCombo;

            for (int i = 1; i <= matrix.Matrix.RowCount; i++)
            {
                string tipoGasto = dataTable.GetValue(campoDataTable, i - 1).ToString();

                if (string.IsNullOrEmpty(tipoGasto))
                {
                    sboCombo = (ComboBox)matrix.Matrix.Columns.Item(columnaMatrix).Cells.Item(i).Specific;
                    General.CargarValidValuesEnCombos(sboCombo.ValidValues, consulta, Conexion);
                }
            }
        }

        /**
         * Método que se encarga de limpiar los todos los parámetros de búsqueda de los vehículos
         */
        public void LimpiarBusquedaGrupos()
        {
            string anno = DateTime.Today.Year.ToString();

            EditTextUnidad.AsignaValorUserDataSource("");
            EditTextNumChasis.AsignaValorUserDataSource("");
            EditTextNumMotor.AsignaValorUserDataSource("");
            EditTextAnno.AsignaValorUserDataSource(anno);

            ComboBoxMarca.AsignaValorUserDataSource("");
            ComboBoxEstilo.AsignaValorUserDataSource("");
            ComboBoxModelo.AsignaValorUserDataSource("");
            ComboBoxColor.AsignaValorUserDataSource("");
            ComboBoxEstado.AsignaValorUserDataSource("");
            ComboBoxCondicion.AsignaValorUserDataSource("");
            ComboBoxUbicacion.AsignaValorUserDataSource("");
        }

        /**
         * Método que se encarga de limpiar los todos los parámetros para la búsqueda de cargar en base a grupo de Eventos
         */
        public void LimpiarBusquedaCargaBG()
        {
            EditTextNoGrupoE.AsignaValorUserDataSource("");
            EditTextFechaGrupoE.AsignaValorUserDataSource("");
            EditTextDescGrupoE.AsignaValorUserDataSource("");
        }

        /**
         * Método que se encarga de limpiar los todos los parámetros para la búsqueda de cargar en base a grupo de Gastos
         */
        public void LimpiarBusquedaCargaBGGastos()
        {
            EditTextNoGrupoG.AsignaValorUserDataSource("");
            EditTextFechaGrupoG.AsignaValorUserDataSource("");
            EditTextDescGrupoG.AsignaValorUserDataSource("");
        }

        /**
         * Método que se encarga de limpiar la matrix de seleccion así como el datatable de seleccion
         */
        public void LimpiarSeleccion()
        {
            MatrixSeleccionGrupo.Matrix.Clear();

            int tamannoDTS = DataTableSeleccion.Rows.Count;

            LimpiarDataTable(DataTableSeleccion,tamannoDTS);
        }

        /**
         * Método que se encarga de limpiar todos los parámetros de eventos así como el datatable de eventos
         */
        public void LimpiarEventos()
        {
            ComboBoxGestionE.AsignaValorUserDataSource("");
            ComboBoxEventoE.AsignaValorUserDataSource("");

            EditTextFechaEventoE.AsignaValorUserDataSource("");
            EditTextNoGrupoE.AsignaValorUserDataSource("");
            EditTextFechaGrupoE.AsignaValorUserDataSource("");
            EditTextDescGrupoE.AsignaValorUserDataSource("");

            MatrixEventosGrupo.Matrix.Clear();

            int tamannoDTE = DataTableEventos.Rows.Count;

            LimpiarDataTable(DataTableEventos, tamannoDTE);

            IndexDataTableE = 0;
        }

        /**
         * Método que se encarga de limpiar todos los parámetros de gastos así como el datatable de gastos
         */
        public void LimpiarGastos()
        {
            ComboBoxGastoG.AsignaValorUserDataSource("");

            EditTextFechaDocumentoG.AsignaValorUserDataSource("");
            EditTextMontoG.AsignaValorUserDataSource("0");
            EditTextTotal.AsignaValorUserDataSource("");
            
            EditTextFechaDocumentoG.AsignaValorUserDataSource("");
            EditTextNoGrupoG.AsignaValorUserDataSource("");
            EditTextFechaGrupoG.AsignaValorUserDataSource("");
            EditTextDescGrupoG.AsignaValorUserDataSource("");

            MatrixGastosGrupo.Matrix.Clear();

            int tamannoDTG = DataTableGastos.Rows.Count;

            LimpiarDataTable(DataTableGastos, tamannoDTG);

            IndexDataTableG = 0;
        }

        /**
         * Método que se encarga de limpiar una datatable
         */
        public void LimpiarDataTable(DataTable dataTable, int tamano)
        {
            for (int z = tamano - 1; z >= 0; z--)
            {
                dataTable.Rows.Remove(z);
            }
        }
        
    }
}
