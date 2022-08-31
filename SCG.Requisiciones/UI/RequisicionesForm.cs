//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using DMS_Connector;
//using DMS_Connector.Business_Logic.DataContract.Requisiciones;
//using DMS_Connector.Data_Access;
//using SAPbobsCOM;
//using SAPbouiCOM;
//using SCG.SBOFramework;
//using SCG.SBOFramework.UI;
//using ICompany = SAPbobsCOM.ICompany;

//namespace SCG.Requisiciones.UI
//{
//    public class RequisicionesForm : IFormularioSBO, IUsaMenu
//    {
//        #region ...Declaracioes...

//        public EditTextSBO EditTextCodigoCliente;
//        public EditTextSBO EditTextEstado;
//        public EditTextSBO EditTextFecha;
//        public EditTextSBO EditTextHora;
//        public EditTextSBO EditTextNoOrden;
//        public EditTextSBO EditTextNoRequisicion;
//        public EditTextSBO EditTextNombreCliente;
//        public EditTextSBO EditTextTipoDocumento;
//        public EditTextSBO EditTextTipoRequisicion;
//        public EditTextSBO EditTextUsuario;
//        public EditTextSBO EditTextComentariosUsuario;
//        public FolderSBO FolderMovimientos;
//        public FolderSBO FolderRequisiciones;
//        public MatrixSBOMovimientosRequisiciones MatrixMovimientos;
//        public MatrixSBOLineasRequisiciones MatrixRequisiciones;
//        public StaticTextSBO StaticTextCodigoCliente;
//        public StaticTextSBO StaticTextEstado;
//        public StaticTextSBO StaticTextFecha;
//        public StaticTextSBO StaticTextNoOrden;
//        public StaticTextSBO StaticTextNoRequisicion;
//        public StaticTextSBO StaticTextNombreCliente;
//        public StaticTextSBO StaticTextTipoDocumento;
//        public StaticTextSBO StaticTextTipoRequisicion;
//        public StaticTextSBO StaticTextUsuario;
//        public ButtonSBO ButtonSBOTrasladar;
//        public ButtonSBO ButtonSBOAjusteCantidad;
//        public ButtonSBO ButtonOk;
//        public ButtonSBO ButtonCancelar;
//        //boton Generar reportes
//        public ButtonSBO ButtonGenerarReporte;

//        public CheckBoxSBO CheckBoxSelTodo;
//        public CheckBoxSBO CheckBoxEntregado;
//        public UserDataSource udsForm;

//        public SAPbouiCOM.DataTable dtLocal;
//        public Boolean m_blnActualizaCot;
//        public Boolean m_blnValidaEntregado;

//        public ComboBoxSBO ComboBoxSucursal;
//        public SAPbouiCOM.DataTable dtCantidadesUbicacion;

//        private GestorFormularios oGestorFormularios;
//        private ListaUbicaciones oFormListaUbi;

//        public NumberFormatInfo n;
//        public ManejadorRequisicionesTraslados manejaRequisicionesTras;
//        private RequisicionData oRequisicionData;

//        public const string mc_strAprobado = "U_SCGD_Aprobado";
//        public const string mc_strTraslad = "U_SCGD_Traslad";
//        public const string mc_strU_NoOrden = "U_SCGD_Numero_OT";
//        public const string mc_strU_Placa = "U_SCGD_Num_Placa";
//        public const string mc_strU_VIN = "U_SCGD_Num_VIN";
//        public const string mc_strU_Marca = "U_SCGD_Des_Marc";
//        public const string mc_strU_Estilo = "U_SCGD_Des_Esti";
//        public const string mc_strU_Modelo = "U_SCGD_Des_Mode";
//        public const string mc_strEmpRealiza = "U_SCGD_Emp_Realiza";
//        public const string mc_strTipoTransferenciaUdf = "U_SCGD_TipoTransf";
//        public const string mc_strNombEmpleado = "U_SCGD_NombEmpleado";
//        public const string mc_strIntCodigoCotizacion = "U_SCGD_CodCotizacion";

//        private SAPbouiCOM.DataTable dtQuery;
//        #endregion

//        #region ...Propiedades...

//        public ICompany CompanySBO { get; private set; }
//        public IApplication ApplicationSBO { get; private set; }
//        public Requisicion Requisicion { get; private set; }

//        #endregion

//        #region ...Constructor...
//        //public RequisicionesForm(Application applicationSBO, ICompany companySBO, Requisicion requisicion)
//        //{
//        //    ApplicationSBO = applicationSBO;
//        //    CompanySBO = companySBO;
//        //    Requisicion = requisicion;
//        //    n = DIHelper.GetNumberFormatInfo(CompanySBO);
//        //    manejaRequisicionesTras = new ManejadorRequisicionesTraslados((SAPbobsCOM.Company)CompanySBO, (SAPbouiCOM.Application)ApplicationSBO, true);
//        //    oRequisicionData = new RequisicionData();

//        //}
//        #endregion

//        #region IFormularioSBO Members

//        public string FormType { get; set; }

//        public string NombreXml { get; set; }
//        public string Titulo { get; set; }

//        public IForm FormularioSBO { get; set; }

//        public bool Inicializado { get; set; }

//        //public void InicializarControles()
//        //{
//        //    if (FormularioSBO != null)
//        //    {
//        //        dtLocal = FormularioSBO.DataSources.DataTables.Add("dtConsulta");
//        //        dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal");
//        //        dtCantidadesUbicacion = FormularioSBO.DataSources.DataTables.Add("dtCantidadesUbicacion");

//        //        FolderRequisiciones = new FolderSBO("fldReq");
//        //        FolderMovimientos = new FolderSBO("fldMov");

//        //        StaticTextNoOrden = new StaticTextSBO("stNoOrden");
//        //        StaticTextCodigoCliente = new StaticTextSBO("stCodCl");
//        //        StaticTextNombreCliente = new StaticTextSBO("stNombCl");
//        //        StaticTextTipoRequisicion = new StaticTextSBO("stTipoReq");
//        //        StaticTextTipoDocumento = new StaticTextSBO("stTipoDoc");
//        //        StaticTextNoRequisicion = new StaticTextSBO("stNoReq");
//        //        StaticTextFecha = new StaticTextSBO("stFecha");
//        //        StaticTextUsuario = new StaticTextSBO("stUsuario");
//        //        StaticTextEstado = new StaticTextSBO("stEstado");

//        //        ButtonOk = new ButtonSBO("1", FormularioSBO);
//        //        ButtonCancelar = new ButtonSBO("btnCanc", FormularioSBO);
//        //        ButtonSBOTrasladar = new ButtonSBO("btnTrasl", FormularioSBO);

//        //        ButtonSBOAjusteCantidad = new ButtonSBO("btnAjuste", FormularioSBO);

//        //        ButtonGenerarReporte = new ButtonSBO("btnGnRpt", FormularioSBO);

//        //        EditTextNoOrden = new EditTextSBO("edtNoOrden", true, UDORequisiciones.TablaEncabezado, "U_SCGD_NoOrden", FormularioSBO);
//        //        EditTextCodigoCliente = new EditTextSBO("edtCodCl", true, UDORequisiciones.TablaEncabezado, "U_SCGD_CodCliente", FormularioSBO);
//        //        EditTextNombreCliente = new EditTextSBO("edtNombCl", true, UDORequisiciones.TablaEncabezado, "U_SCGD_NombCliente", FormularioSBO);
//        //        EditTextTipoRequisicion = new EditTextSBO("edtTipoReq", true, UDORequisiciones.TablaEncabezado, "U_SCGD_TipoReq", FormularioSBO);
//        //        EditTextTipoDocumento = new EditTextSBO("edtTipoDoc", true, UDORequisiciones.TablaEncabezado, "U_SCGD_TipoDoc", FormularioSBO);
//        //        EditTextNoRequisicion = new EditTextSBO("edtNoReq", true, UDORequisiciones.TablaEncabezado, "DocNum", FormularioSBO);
//        //        EditTextFecha = new EditTextSBO("edtFecha", true, UDORequisiciones.TablaEncabezado, "CreateDate", FormularioSBO);
//        //        EditTextHora = new EditTextSBO("edtHora", true, UDORequisiciones.TablaEncabezado, "CreateTime", FormularioSBO);
//        //        EditTextUsuario = new EditTextSBO("edtUsuario", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Usuario", FormularioSBO);
//        //        EditTextComentariosUsuario = new EditTextSBO("txtComen", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Comen", FormularioSBO);
//        //        EditTextEstado = new EditTextSBO("edtEstado", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Est", FormularioSBO);
//        //        CheckBoxEntregado = new CheckBoxSBO("chkEnt", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Entregado", FormularioSBO);

//        //        udsForm = FormularioSBO.DataSources.UserDataSources.Add("Sel", BoDataType.dt_SHORT_TEXT, 10);
//        //        CheckBoxSelTodo = new CheckBoxSBO("chkSelTodo", true, "", "Sel", FormularioSBO);

//        //        ComboBoxSucursal = new ComboBoxSBO("cboSucur", FormularioSBO, true, UDORequisiciones.TablaEncabezado, "U_SCGD_IDSuc");

//        //        var numberFormatInfo = DIHelper.GetNumberFormatInfo(CompanySBO);
//        //        MatrixRequisiciones = new MatrixSBOLineasRequisiciones("mtxReq", FormularioSBO) { TablaLigada = UDORequisiciones.TablaLineas, NumberFormatInfo = numberFormatInfo };
//        //        //if (manejaRequisicionesTras != null)
//        //        //    MatrixRequisiciones.CopiarLineasMatriz += CopiarLineasMatriz;
//        //        MatrixRequisiciones.CreaColumnas();
//        //        MatrixRequisiciones.LigaColumnas();
//        //        CargarValidValuesEnCombos();

//        //        MatrixRequisiciones.Especifico.SelectionMode = BoMatrixSelect.ms_None;

//        //        MatrixMovimientos = new MatrixSBOMovimientosRequisiciones("mtxMov", FormularioSBO) { TablaLigada = UDORequisiciones.TablaMovimientos, NumberFormatInfo = numberFormatInfo };
//        //        MatrixMovimientos.CreaColumnas();
//        //        MatrixMovimientos.LigaColumnas();

//        //        ILinkedButton linked = (ILinkedButton)MatrixMovimientos.ColumnaCodigoDocumento.Columna.ExtendedObject;
//        //        linked.LinkedObjectType = Requisicion.TipoDocumentoMovimiento;

//        //        EditTextNoOrden.AsignaBinding();
//        //        EditTextCodigoCliente.AsignaBinding();
//        //        EditTextNombreCliente.AsignaBinding();
//        //        EditTextTipoRequisicion.AsignaBinding();
//        //        EditTextTipoDocumento.AsignaBinding();
//        //        EditTextNoRequisicion.AsignaBinding();
//        //        EditTextFecha.AsignaBinding();
//        //        EditTextHora.AsignaBinding();
//        //        EditTextEstado.AsignaBinding();
//        //        EditTextUsuario.AsignaBinding();
//        //        EditTextComentariosUsuario.AsignaBinding();

//        //        CheckBoxSelTodo.AsignaBinding();
//        //        CheckBoxEntregado.AsignaBinding();

//        //        EditTextNoOrden.HabilitarBuscar();
//        //        EditTextCodigoCliente.HabilitarBuscar();
//        //        EditTextNombreCliente.HabilitarBuscar();
//        //        EditTextNoRequisicion.HabilitarBuscar();
//        //        EditTextFecha.HabilitarBuscar();
//        //        EditTextUsuario.HabilitarBuscar();
//        //        EditTextEstado.HabilitarBuscar();

//        //        ButtonSBOTrasladar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
//        //        ButtonCancelar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
//        //        ButtonGenerarReporte.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
//        //        ButtonSBOAjusteCantidad.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);

//        //        ComboBoxSucursal.AsignaBinding();

//        //        dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
//        //        dtLocal.Clear();
//        //        string l_strSQL = " select \"Branch\" from OUSR where \"USER_CODE\" = '{0}' ";

//        //        dtLocal.ExecuteQuery(string.Format(l_strSQL, ApplicationSBO.Company.UserName));

//        //        if (!string.IsNullOrEmpty(dtLocal.GetValue("Branch", 0).ToString()))
//        //        {
//        //            if (DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == dtLocal.GetValue("Branch", 0).ToString()).U_Entrega_Rep.Equals("Y"))
//        //            {
//        //                FormularioSBO.Items.Item(CheckBoxEntregado.UniqueId).Visible = true;
//        //                m_blnValidaEntregado = true;
//        //            }
//        //            else
//        //            {
//        //                FormularioSBO.Items.Item(CheckBoxEntregado.UniqueId).Visible = false;
//        //                m_blnValidaEntregado = false;
//        //            }
//        //        }

//        //        LlenarComboSucursal();

//        //        //Valida si usa ubicaciones
//        //        if (DMS_Connector.Configuracion.ParamGenAddon.U_UsaUbicD.Trim() != "Y")
//        //        {
//        //            MatrixRequisiciones.ColumnaDeUbicacion.Columna.Visible = false;
//        //            MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;

//        //            MatrixRequisiciones.ColumnaAUbicacion.Columna.Visible = false;
//        //            MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
//        //        }
//        //        else
//        //        {
//        //            //si usa pero tiene sap 8 o inferior
//        //            if (CompanySBO.Version < 900000)
//        //            {
//        //                MatrixRequisiciones.ColumnaDeUbicacion.Columna.Visible = false;
//        //                MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;

//        //                MatrixRequisiciones.ColumnaAUbicacion.Columna.Visible = false;
//        //                MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
//        //            }
//        //        }
//        //    }
//        //}

//        public void InicializaFormulario()
//        {
//            if (FormularioSBO != null)
//            {
//                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
//                CultureInfo cultureInfo = Resource.Culture;
//                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
//                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
//                Resource.Culture = cultureInfo;

//                FormType = FormularioSBO.TypeEx;
//                FormularioSBO.DataBrowser.BrowseBy = "edtNoReq";
//                FormularioSBO.PaneLevel = 1;
//                FormularioSBO.Mode = BoFormMode.fm_FIND_MODE;
//                FormularioSBO.Title = Titulo;

//                foreach (SAPbouiCOM.Item oItem in FormularioSBO.Items)
//                {
//                    if (oItem.UniqueID == "chkSelTodo")
//                    {
//                        oItem.AffectsFormMode = false;
//                    }
//                }
//            }
//        }

//        #endregion

//        #region IUsaMenu Members

//        public string IdMenu { get; set; }
//        public string MenuPadre { get; set; }
//        public int Posicion { get; set; }
//        public string Nombre { get; set; }

//        //Manejo de reportes
//        public string DireccionReportes { get; set; }
//        public string BDUser { get; set; }
//        public string BDPass { get; set; }

//        #endregion

//        #region ...Metodos...

//        //public void CargarValidValuesEnCombos()
//        //{
//        //    List<GeneralStructs.ListadoValidValues> lstValidValues = new List<GeneralStructs.ListadoValidValues>();
//        //    SAPbouiCOM.ValidValues validaValuesColumn;
//        //    try
//        //    {
//        //        lstValidValues = Utilitarios.GetListadoValidValues(" SELECT \"Code\", \"Name\" FROM \"@SCGD_OBSER_REQ\" ");
//        //        validaValuesColumn = MatrixRequisiciones.ColumnaLineaObservcacion.Columna.ValidValues;
//        //        Utilitarios.CargarValidValuesEnCombos(ref validaValuesColumn, lstValidValues);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Utilitarios.ManejadorErrores(ex);
//        //    }
//        //}

//        //public void LlenarComboSucursal()
//        //{
//        //    SAPbouiCOM.ComboBox cboCombo;
//        //    SAPbouiCOM.Item oItem;

//        //    oItem = FormularioSBO.Items.Item("cboSucur");
//        //    cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

//        //    Utilitarios.CargarValidValuesEnCombos(ref cboCombo, "SELECT \"Code\", \"Name\" FROM \"@SCGD_SUCURSALES\" ");
//        //}

//        private Boolean ValidaUsaUbicaciones(string FormUID)
//        {
//            return DMS_Connector.Configuracion.ParamGenAddon.U_UsaUbicD.Trim() == "Y";
//        }

//        private string ValidaUbicacionesLinea(LineaRequisicion inf, ref ItemEvent pVal)
//        {
//            string error = string.Empty;
//            Boolean usaUbicaciones = false;
//            try
//            {
//                SAPbouiCOM.DataTable dtConsulta;
//                SAPbouiCOM.Form oForm = ApplicationSBO.Forms.Item(pVal.FormUID);
//                dtConsulta = Utilitarios.ValidarDataTable(ref oForm, "dtConsulta") ? oForm.DataSources.DataTables.Item("dtConsulta") : oForm.DataSources.DataTables.Add("dtConsulta");

//                //Validacion para ubicacion de origen
//                dtConsulta.ExecuteQuery(string.Format("select \"BinActivat\" from \"OWHS\" where \"WhsCode\" = '{0}'", inf.U_SCGD_CodBodOrigen));
//                if (dtConsulta.Rows.Count > 0)
//                {
//                    //Valida si la bodega de origen usa ubicaciones
//                    if (dtConsulta.GetValue("BinActivat", 0).ToString() == "Y")
//                    {
//                        if (string.IsNullOrEmpty(inf.U_DeUbic))
//                        {
//                            error = String.Format(Resource.txtErrorLnUbicOri, inf.DataSourceOffset + 1);
//                            return error;
//                        }
//                    }
//                    else
//                    {
//                        if (!string.IsNullOrEmpty(inf.U_DeUbic))
//                        {
//                            error = String.Format(Resource.txtErrorNoBodUbicOri, inf.DataSourceOffset + 1);
//                            return error;
//                        }
//                    }
//                }

//                //Validacion para ubicacion destino
//                dtConsulta.ExecuteQuery(string.Format("select \"BinActivat\" from \"OWHS\" where \"WhsCode\" = '{0}'", inf.U_SCGD_CodBodDest));
//                if (dtConsulta.Rows.Count > 0)
//                {
//                    //Valida si la bodega de origen usa ubicaciones
//                    if (dtConsulta.GetValue("BinActivat", 0).ToString() == "Y")
//                    {
//                        if (string.IsNullOrEmpty(inf.U_AUbic))
//                        {
//                            error = String.Format(Resource.txtErrorLnUbicDest, inf.DataSourceOffset + 1);
//                            return error;
//                        }
//                    }
//                    else
//                    {
//                        if (!string.IsNullOrEmpty(inf.U_AUbic))
//                        {
//                            error = String.Format(Resource.txtErrorNoBodUbicDest, inf.DataSourceOffset + 1);
//                            return error;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//            }
//            return error;
//        }

//        //public void ButtonPrincipal(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        //{
//        //    if (pVal.BeforeAction)
//        //    {
//        //        FormularioSBO.Items.Item("cboSucur").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False);
//        //    }
//        //    else if (pVal.ActionSuccess)
//        //    {
//        //        if (m_blnActualizaCot)
//        //        {
//        //            m_blnActualizaCot = false;
//        //            ActualizaCotizacion();
//        //            ActualizaTansferenciaS();
//        //        }
//        //    }
//        //}

//        //protected virtual void DataLoadEvent(BusinessObjectInfo businessObjectInfo, ref bool bubbleEvent)
//        //{
//        //    //bubbleEvent = true;
//        //    CheckBoxSelTodo.AsignaValorUserDataSource("N");

//        //    if (!businessObjectInfo.BeforeAction && businessObjectInfo.ActionSuccess)
//        //    {
//        //        ActualizaLineasAlCargar();
//        //        MatrixRequisiciones.Especifico.LoadFromDataSource();
//        //        MatrixMovimientos.EliminaPrimeraLinea();
//        //        MatrixMovimientos.Especifico.LoadFromDataSource();
//        //        CargarObjRequisicion();
//        //    }
//        //}

//        protected void ActualizaLineasAlCargar()
//        {
//            ICompany company = CompanySBO;
//            ManejadorArticulos manejadorArticulos = new ManejadorArticulos(company);
//            ManejadorEstadoLinea manejadorEstadoLinea = new ManejadorEstadoLinea(company);
//            if (MatrixRequisiciones.FormularioSBO != null)
//            {
//                DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);
//                DBDataSource formDataSource = FormularioSBO.DataSources.DBDataSources.Item((UDORequisiciones.TablaEncabezado));
//                bool algunaPendiente = false;
//                bool todasCanceladas = true;
//                // bool algunaTrasladada = false;
//                string estado;
//                float cantidadDisponible;
//                float cantidadPendiente;
//                string codEst;
//                LineaRequisicion infLinea;

//                for (int i = 0; i < dbDataSource.Size; i++)
//                {
//                    manejadorArticulos.ItemCode = MatrixRequisiciones.ColumnaCodigoArticulo.ObtieneValorColumnaDataTable(i, dbDataSource);
//                    manejadorArticulos.WhsCode = MatrixRequisiciones.ColumnaCodigoBodegaOrigen.ObtieneValorColumnaDataTable(i, dbDataSource);
//                    cantidadDisponible = manejadorArticulos.CantidadDisponible();
//                    MatrixRequisiciones.ColumnaDisponible.AsignaValorDataSource(cantidadDisponible, i, dbDataSource);

//                    manejadorEstadoLinea.CantidadSolicitada = float.Parse(MatrixRequisiciones.ColumnaCantidadSolicitada.ObtieneValorColumnaDataTable(i, dbDataSource), n);
//                    manejadorEstadoLinea.CantidadRecibida = float.Parse(MatrixRequisiciones.ColumnaCantidadRecibida.ObtieneValorColumnaDataTable(i, dbDataSource), n);

//                    cantidadPendiente = manejadorEstadoLinea.CantidadSolicitada - manejadorEstadoLinea.CantidadRecibida;

//                    MatrixRequisiciones.ColumnaCantidadPendiente.AsignaValorDataSource(cantidadPendiente, i, dbDataSource);
//                    MatrixRequisiciones.ColumnaCantidadAjuste.AsignaValorDataSource(0, i, dbDataSource);

//                    codEst = MatrixRequisiciones.ColumnaCodigoEstado.ObtieneValorColumnaDataTable(i, dbDataSource);
//                    manejadorEstadoLinea.EstadoActual = (EstadosLineas)Enum.Parse(typeof(EstadosLineas), (String.IsNullOrEmpty(codEst) ? "1" : codEst));
//                    manejadorEstadoLinea.CalculaEstado();

//                    if (manejadorEstadoLinea.EstadoActual == EstadosLineas.Pendiente)
//                        algunaPendiente = true;

//                    if (manejadorEstadoLinea.EstadoActual != EstadosLineas.Cancelado)
//                        todasCanceladas = false;
//                    //else if (manejadorEstadoLinea.EstadoActual == EstadosLineas.Trasladado)
//                    //    algunaTrasladada = true;

//                    MatrixRequisiciones.ColumnaCodigoEstado.AsignaValorDataSource((int)(manejadorEstadoLinea.EstadoActual), i, dbDataSource);
//                    estado = manejadorEstadoLinea.EstadoActual.ToString();
//                    infLinea = new LineaRequisicion();
//                    MatrixRequisiciones.LineaFromDBDataSource(i, ref infLinea);
//                    estado = Localize(infLinea, TipoMensaje.EstadoLinea, estado);
//                    MatrixRequisiciones.ColumnaEstado.AsignaValorDataSource(estado, i, dbDataSource);
//                    MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(0, i, dbDataSource);
//                }
//                EstadosLineas estadoFormulario;

//                if (todasCanceladas)
//                    estadoFormulario = EstadosLineas.Cancelado;
//                else if (algunaPendiente)
//                    estadoFormulario = EstadosLineas.Pendiente;
//                else
//                    estadoFormulario = EstadosLineas.Trasladado;

//                BoModeVisualBehavior behavior = estadoFormulario == EstadosLineas.Pendiente ? BoModeVisualBehavior.mvb_True : BoModeVisualBehavior.mvb_False;
//                ButtonSBOTrasladar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, behavior);
//                ButtonSBOTrasladar.ItemSBO.Enabled = estadoFormulario == EstadosLineas.Pendiente;
//                ButtonCancelar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, behavior);
//                ButtonCancelar.ItemSBO.Enabled = estadoFormulario == EstadosLineas.Pendiente;

//                ButtonGenerarReporte.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Ok, BoModeVisualBehavior.mvb_True);
//                ButtonGenerarReporte.ItemSBO.Enabled = true;

//                if (formDataSource.GetValue("U_SCGD_CodTipoReq", 0).Trim() == "2")
//                {
//                    MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;
//                    MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = true;
//                }
//                else
//                {
//                    MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = true;
//                    MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
//                }

//                estado = Localize(new LineaRequisicion { U_SCGD_CodEst = (int)estadoFormulario }, TipoMensaje.EstadoFormulario, estadoFormulario.ToString());
//                EditTextEstado.AsignaValorDataSource(estado);
//                formDataSource.SetValue("U_SCGD_CodEst", 0, ((int)estadoFormulario).ToString());
//            }
//        }

//        protected virtual string Localize(LineaRequisicion p_LineaRequisicion, TipoMensaje tipoMensaje, string mensaje)
//        {
//            string m = string.Empty;
//            if (manejaRequisicionesTras != null)
//                m = manejaRequisicionesTras.LocalizationNeeded(p_LineaRequisicion, tipoMensaje);
//            return string.IsNullOrEmpty(m) ? mensaje : m;
//        }

//        public void ActualizaTansferenciaS()
//        {
//            int l_intDocEntry = 0;
//            string l_strDocEntry = "";

//            SAPbobsCOM.StockTransfer oTrans;

//            string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();

//            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
//            dtLocal.Clear();

//            if (l_strEntregado == "")
//                l_strEntregado = "N";

//            if (m_blnValidaEntregado)
//            {

//                oTrans = (SAPbobsCOM.StockTransfer)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

//                DBDataSource dbTranferencias = MatrixMovimientos.FormularioSBO.DataSources.DBDataSources.Item(MatrixMovimientos.TablaLigada);

//                for (int i = 0; i < dbTranferencias.Size; i++)
//                {
//                    l_strDocEntry = dbTranferencias.GetValue("U_SCGD_DocEntry", i).Trim();
//                    l_intDocEntry = int.Parse(l_strDocEntry);

//                    oTrans.GetByKey(l_intDocEntry);

//                    oTrans.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
//                    oTrans.Update();
//                }
//            }
//        }

//        protected virtual DateTime DateTimeFromString(string fecha, string hora)
//        {
//            //validacion para manejo de hora en requisiciones
//            string s = string.Empty;
//            switch (hora.Length)
//            {
//                case 1:
//                    s = "000" + hora;
//                    break;
//                case 2:
//                    s = "00" + hora;
//                    break;
//                case 3:
//                    s = "0" + hora;
//                    break;
//                case 4:
//                    s = hora;
//                    break;
//            }
//            DateTime dateTimeFromString = DateTime.ParseExact(fecha + s, "yyyyMMddHHmm", null);
//            return dateTimeFromString;
//        }

//        public static void ImprimirReporte(SAPbobsCOM.ICompany company, string direccionReporte, string barraTitulo, string parametros, string usuarioBD, string contraseñaBD, string BD, string servidor)
//        {
//            string pathExe;
//            string parametrosExe;

//            if (string.IsNullOrEmpty(barraTitulo))
//            {
//                barraTitulo = Resource.rptReporteRequisicion;
//            }

//            barraTitulo = barraTitulo.Replace(" ", "°");
//            direccionReporte = direccionReporte.Replace(" ", "°");
//            parametros = parametros.Replace(" ", "°");
//            parametros = String.Format("{0},{1}", parametros, DMS_Connector.Helpers.GetReportSchema());

//            pathExe = Directory.GetCurrentDirectory() + "\\SCG Visualizador de Reportes.exe";

//            if (DMS_Connector.Helpers.IsHANAConnection())
//                BD = BD + "," + "1";
//            parametrosExe = barraTitulo + " " + direccionReporte + " " + usuarioBD + "," + contraseñaBD + "," + servidor + "," + BD + " " + parametros;

//            ProcessStartInfo startInfo = new ProcessStartInfo(pathExe) { WindowStyle = ProcessWindowStyle.Maximized, Arguments = parametrosExe };

//            Process.Start(startInfo);
//        }

//        public void SeleccionaTodo(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        {
//            if (pVal.BeforeAction)
//            {
//                string strSeleccionTodas = string.Empty;
//                DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);

//                strSeleccionTodas = CheckBoxSelTodo.ObtieneValorUserDataSource();

//                if (strSeleccionTodas == "Y")
//                {
//                    for (int i = 0; i < dbDataSource.Size; i++)
//                    {
//                        MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(1, i, dbDataSource);
//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < dbDataSource.Size; i++)
//                    {
//                        MatrixRequisiciones.ColumnaCheck.AsignaValorDataSource(0, i, dbDataSource);
//                    }
//                }
//                MatrixRequisiciones.Especifico.LoadFromDataSource();
//            }

//            else
//            {
//                bubbleEvent = false;
//            }
//        }

//        public void ActualizaCotizacion(ref SAPbobsCOM.Documents oCotizacion)
//        {
//            int l_intDocEntry = 0;

//            //SAPbobsCOM.Documents oCotizacion;
//            SAPbobsCOM.Document_Lines oLineasCotizacion;

//            string l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();
//            string l_strNumOT = EditTextNoOrden.ObtieneValorDataSource();

//            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
//            dtLocal.Clear();

//            dtLocal.ExecuteQuery(string.Format(DMS_Connector.Queries.GetStrSpecificQuery("strFRCotDocEntry"), l_strNumOT));
//            l_intDocEntry = (int)dtLocal.GetValue("DocEntry", 0);

//            if (l_strEntregado == "")
//                l_strEntregado = "N";

//            string m_strConfOT = "N";
//            bool m_blnConfOTSAP = false;
//            int m_intLineNum = 0;

//            double dblCantidadDM = 0;
//            double dblCantidadSolicitado = 0;
//            double dblCantidadPendienteDev = 0;

//            m_blnConfOTSAP = Utilitarios.ValidaUsaOTSap();

//            oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
//            if (oCotizacion.GetByKey(l_intDocEntry))
//            {
//                oLineasCotizacion = oCotizacion.Lines;
//                DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);
//                for (int i = 0; i < dbDataSource.Size; i++)
//                {
//                    for (int j = 0; j < oLineasCotizacion.Count; j++)
//                    {
//                        oLineasCotizacion.SetCurrentLine(j);
//                        if (int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_LNumOr", i).Trim()) == oLineasCotizacion.LineNum)
//                        {
//                            if (m_blnValidaEntregado)
//                            {
//                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
//                            }
//                            if (m_blnConfOTSAP)
//                            {
//                                dblCantidadDM = oLineasCotizacion.Quantity;
//                                dblCantidadSolicitado = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value;

//                                if (dblCantidadDM > dblCantidadSolicitado)
//                                {
//                                    dblCantidadDM = dblCantidadSolicitado;
//                                }

//                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = (double)(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value) + dblCantidadDM;

//                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = dblCantidadSolicitado - dblCantidadDM;

//                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0;

//                                dblCantidadPendienteDev = (double)oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value;

//                                if (dblCantidadPendienteDev > 0)
//                                {
//                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0;
//                                }
//                            }
//                            if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim() != oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString().Trim())
//                            {
//                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim();
//                            }
//                        }
//                    }
//                }
//            }
//            //oCotizacion.Update();
//        }

//        public void CargarObjRequisicion()
//        {
//            try
//            {
//                if (FormularioSBO != null)
//                {
//                    oRequisicionData.CodigoCliente = EditTextCodigoCliente.ObtieneValorDataSource();
//                    oRequisicionData.NombreCliente = EditTextNombreCliente.ObtieneValorDataSource();
//                    oRequisicionData.DocEntry = int.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("DocEntry", 0));
//                    oRequisicionData.DocNum = Convert.ToInt32(EditTextNoRequisicion.ObtieneValorDataSource());
//                    oRequisicionData.CreateDate = DateTimeFromString(EditTextFecha.ObtieneValorDataSource(), EditTextHora.ObtieneValorDataSource());
//                    oRequisicionData.NoOrden = EditTextNoOrden.ObtieneValorDataSource();
//                    oRequisicionData.Usuario = EditTextUsuario.ObtieneValorDataSource();
//                    oRequisicionData.TipoDocumento = EditTextTipoDocumento.ObtieneValorDataSource();
//                    oRequisicionData.TipoRequisicion = EditTextTipoRequisicion.ObtieneValorDataSource();
//                    oRequisicionData.Data = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Data", 0);
//                    oRequisicionData.Comentario = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comm", 0);
//                    oRequisicionData.ComentariosUser = EditTextComentariosUsuario.ObtieneValorDataSource();// FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Comen", 0);
//                    oRequisicionData.Placa = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Placa", 0).Trim();
//                    oRequisicionData.Marca = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Marca", 0).Trim();
//                    oRequisicionData.Estilo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_Estilo", 0).Trim();
//                    oRequisicionData.VIN = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_VIN", 0).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_Serie", 0)))
//                        oRequisicionData.Serie = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_Serie", 0).Trim());
//                    oRequisicionData.CodigoTipoRequisicion = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_CodTipoReq", 0).Trim());
//                    oRequisicionData.SucursalID = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_IDSuc", 0).Trim();
//                    oRequisicionData.EstadoRequisicion = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaEncabezado).GetValue("U_SCGD_CodEst", 0).Trim();

//                    oRequisicionData.LineasRequisicion = CargarLineasReq();
//                }
//            }
//            catch (Exception ex)
//            {
//                //Revisar
//                //Utilitarios.ManejadorErrores(ex);
//            }
//        }

//        private List<LineaRequisicion> CargarLineasReq()
//        {
//            List<LineaRequisicion> lstLineas = new List<LineaRequisicion>();
//            LineaRequisicion linea;

//            try
//            {
//                for (int i = 0; i <= FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).Size - 1; i++)
//                {
//                    linea = new LineaRequisicion();
//                    linea.DataSourceOffset = i;
//                    linea.DocEntry = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("DocEntry", i).Trim());
//                    linea.LineId = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("LineId", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_AUbic", i)))
//                        linea.U_AUbic = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_AUbic", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_DeUbic", i)))
//                        linea.U_DeUbic = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_DeUbic", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_FechaM", i)))
//                        linea.U_FechaM = DateTime.ParseExact(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_FechaM", i).Trim(), "yyyyMMdd", CultureInfo.InvariantCulture);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_HoraM", i)))
//                        linea.U_HoraM = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_HoraM", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_Obs_Req", i)))
//                        linea.U_Obs_Req = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_Obs_Req", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_ReqOriPen", i)))
//                        linea.U_ReqOriPen = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_ReqOriPen", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CAju", i)))
//                        linea.U_SCGD_CAju = Convert.ToDouble(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CAju", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CCosto", i)))
//                        linea.U_SCGD_CCosto = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CCosto", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_COrig", i)))
//                        linea.U_SCGD_COrig = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_COrig", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantATransf", i)))
//                        linea.U_SCGD_CantATransf = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantATransf", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantDispo", i)))
//                        linea.U_SCGD_CantDispo = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantDispo", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantPen", i)))
//                        linea.U_SCGD_CantPen = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantPen", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantRec", i)))
//                        linea.U_SCGD_CantRec = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantRec", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantSol", i)))
//                        linea.U_SCGD_CantSol = Double.Parse(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CantSol", i).Trim(), n);
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Chk", i)))
//                        linea.U_SCGD_Chk = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Chk", i).Trim() == "0" ? 0 : 1;
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodArticulo", i)))
//                        linea.U_SCGD_CodArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodArticulo", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodDest", i)))
//                        linea.U_SCGD_CodBodDest = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodDest", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodOrigen", i)))
//                        linea.U_SCGD_CodBodOrigen = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodBodOrigen", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodEst", i)))
//                        linea.U_SCGD_CodEst = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodEst", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodTipoArt", i)))
//                        linea.U_SCGD_CodTipoArt = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_CodTipoArt", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DescArticulo", i)))
//                        linea.U_SCGD_DescArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DescArticulo", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DocOr", i)))
//                        linea.U_SCGD_DocOr = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_DocOr", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Estado", i)))
//                        linea.U_SCGD_Estado = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Estado", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_ID", i)))
//                        linea.U_SCGD_ID = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_ID", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_LNumOr", i)))
//                        linea.U_SCGD_LNumOr = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_LNumOr", i).Trim());
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Lidsuc", i)))
//                        linea.U_SCGD_Lidsuc = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_Lidsuc", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_TipoArticulo", i)))
//                        linea.U_SCGD_TipoArticulo = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_SCGD_TipoArticulo", i).Trim();
//                    if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_TipoM", i)))
//                        linea.U_TipoM = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("U_TipoM", i).Trim());

//                    linea.VisOrder = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas).GetValue("VisOrder", i).Trim());
//                    lstLineas.Add(linea);
//                }
//                return lstLineas;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            return lstLineas;
//        }

//        private void FolderMovimientosItemPressed(string formUId, IItemEvent pVal, ref bool bubbleEvent)
//        {
//            if (pVal.ActionSuccess)
//            {
//                if (pVal.ItemUID == FolderRequisiciones.UniqueId && FormularioSBO.PaneLevel != 1)
//                    FormularioSBO.PaneLevel = 1;
//                else if (pVal.ItemUID == FolderMovimientos.UniqueId && FormularioSBO.PaneLevel != 2)
//                    FormularioSBO.PaneLevel = 2;
//            }
//        }

//        public Boolean ValidaCotizacionAbierta()
//        {
//            bool result = true;
//            try
//            {
//                String strNumOT = ((SAPbouiCOM.EditText)FormularioSBO.Items.Item("edtNoOrden").Specific).Value.Trim();
//                String query = string.Format(DMS_Connector.Queries.GetStrSpecificQuery("strConsultaFRCotDocCur"), strNumOT);
//                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
//                dtLocal.ExecuteQuery(query);
//                if (dtLocal.Rows.Count > 0)
//                {
//                    if (dtLocal.GetValue("DocStatus", 0).ToString().Trim() == "C")
//                    {
//                        result = false;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                result = false;
//                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//            }
//            return result;
//        }

//        public Boolean ValidaOTAbierta()
//        {
//            bool result = true;
//            var estado = string.Empty;
//            String strNumOT;
//            String idSucursal;
//            try
//            {
//                strNumOT = ((SAPbouiCOM.EditText)FormularioSBO.Items.Item("edtNoOrden").Specific).Value.Trim();
//                idSucursal = ((SAPbouiCOM.ComboBox)FormularioSBO.Items.Item("cboSucur").Specific).Value.Trim();

//                if (Utilitarios.ValidaUsaOTSap())
//                {
//                    if (Configuracion.ConfiguracionSucursales.Any(x => x.U_Sucurs == idSucursal))
//                    {
//                        if (Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == idSucursal).U_ValReqPen == "Y")
//                        {
//                            var query = string.Format("select \"U_EstO\" from \"@SCGD_OT\" where \"U_NoOT\" = '{0}' ", strNumOT);
//                            estado = DMS_Connector.Helpers.EjecutarConsulta(query);
//                            if (!string.IsNullOrEmpty(estado))
//                            {
//                                if (estado != "1" && estado != "2" && estado != "3")
//                                    result = false;
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                result = false;
//                ApplicationSBO.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//            }
//            return result;
//        }

//        //Manejo del evento item pressed del boton Generar Reporte de requisiciones
//        private void ButtonSBOGenerarReporteItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        {
//            //Declaracion de variables 
//            string strParametros = "";

//            if (!pVal.BeforeAction)
//            {
//                strParametros = EditTextNoRequisicion.ObtieneValorDataSource();

//                string direccionR = DireccionReportes + Resource.rptReporteRequisicion;

//                ImprimirReporte(CompanySBO, direccionR, Resource.TitulorptRequisiciones, strParametros, BDUser, BDPass, CompanySBO.CompanyDB, CompanySBO.Server);
//            }
//        }

//        //private void ButtonSBOTrasladarItemPress(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        //{
//        //    SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
//        //    SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
//        //    SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
//        //    SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);
//        //    SAPbobsCOM.GeneralData oChild = default(SAPbobsCOM.GeneralData);
//        //    SAPbobsCOM.GeneralDataCollection oChildren = default(SAPbobsCOM.GeneralDataCollection);

//        //    SAPbobsCOM.Documents oCotizacion = default(SAPbobsCOM.Documents);
//        //    SAPbouiCOM.Matrix matrixReq = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxReq").Specific;

//        //    string error = string.Empty;
//        //    ManejadorArticulos manejadorArticulos = new ManejadorArticulos(CompanySBO);
//        //    Boolean transferida = true;

//        //    try
//        //    {
//        //        if (pVal.BeforeAction)
//        //        {
//        //            matrixReq.FlushToDataSource();
//        //            CargarObjRequisicion();
//        //            if (oRequisicionData.LineasRequisicion.Any(x => x.U_SCGD_Chk == 1))
//        //            {
//        //                foreach (LineaRequisicion linea in oRequisicionData.LineasRequisicion.Where(x => x.U_SCGD_Chk == 1).ToList())
//        //                {
//        //                    if (linea.U_SCGD_Chk == 1)
//        //                    {
//        //                        if (manejadorArticulos.CantidadDisponibleItemEspecifico(linea.U_SCGD_CodArticulo, linea.U_SCGD_CodBodOrigen))
//        //                        {
//        //                            error = string.Format(Resource.txtErrorNoTrasReq, linea.DataSourceOffset + 1, linea.U_SCGD_CodBodOrigen);
//        //                            error = Localize(linea, TipoMensaje.NoSePuedenBodegasIguales, error);
//        //                            break;
//        //                        }
//        //                        else if (linea.U_SCGD_CodBodOrigen == linea.U_SCGD_CodBodDest)
//        //                        {
//        //                            error = string.Format(Resource.txtErrorBodegasIguales, linea.U_SCGD_CodBodDest, linea.U_SCGD_CodBodOrigen);
//        //                            error = Localize(linea, TipoMensaje.NoSePuedenBodegasIguales, error);
//        //                            break;
//        //                        }
//        //                        else if ((EstadosLineas)linea.U_SCGD_CodEst != EstadosLineas.Pendiente)
//        //                        {
//        //                            error = string.Format(Resource.txtErrorTraslLinea, linea.DataSourceOffset + 1);
//        //                            error = Localize(linea, TipoMensaje.ErrorNoSePuedeTrasladar, error);
//        //                            break;
//        //                        }
//        //                        else if (linea.U_SCGD_CantATransf == 0 || linea.U_SCGD_CantATransf > linea.U_SCGD_CantPen)
//        //                        {
//        //                            error = string.Format(Resource.txtErrorTrasQti, linea.DataSourceOffset + 1);
//        //                            error = Localize(linea, TipoMensaje.MayorQueCantidadPendiente, error);
//        //                            break;
//        //                        }
//        //                        else if (ValidaUsaUbicaciones(pVal.FormUID))//string.IsNullOrEmpty(inf.DeUbicacion) || string.IsNullOrEmpty(inf.AUbicacion))
//        //                        {
//        //                            error = ValidaUbicacionesLinea(linea, ref pVal);
//        //                            if (!string.IsNullOrEmpty(error))
//        //                            {
//        //                                break;
//        //                            }
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //            else
//        //            {
//        //                error = Resource.ErrNoLineaSel;
//        //            }

//        //            if (!string.IsNullOrEmpty(error))
//        //            {
//        //                ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//        //                bubbleEvent = false;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            List<LineaRequisicion> lineasTransferidas = new List<LineaRequisicion>();
//        //            List<StockTransfer> transferList = new List<StockTransfer>();

//        //            if (Traslada(ref transferList, ref lineasTransferidas))
//        //            {
//        //                foreach (var linea in lineasTransferidas)
//        //                {
//        //                    if (linea.U_SCGD_CodEst != (int)GeneralEnums.EstadoRequisicion.Cancelado)
//        //                    {
//        //                        linea.U_SCGD_CantRec += linea.U_SCGD_CantATransf;
//        //                        linea.U_SCGD_CantPen = linea.U_SCGD_CantSol - linea.U_SCGD_CantRec;
//        //                        linea.U_SCGD_CodEst = linea.U_SCGD_CantSol == linea.U_SCGD_CantRec ?
//        //                            (int)GeneralEnums.EstadoRequisicion.Trasladado : linea.U_SCGD_CodEst;
//        //                        linea.U_SCGD_Estado = ((GeneralEnums.EstadoRequisicion)linea.U_SCGD_CodEst).ToString();
//        //                    }

//        //                    foreach (LineaRequisicion lineaRequisicion in oRequisicionData.LineasRequisicion)
//        //                    {
//        //                        if (lineaRequisicion.U_SCGD_CodArticulo == linea.U_SCGD_CodArticulo && lineaRequisicion.U_SCGD_LNumOr == linea.U_SCGD_LNumOr)
//        //                        {
//        //                            lineaRequisicion.U_SCGD_CantRec = linea.U_SCGD_CantRec;
//        //                            lineaRequisicion.U_SCGD_CantPen = linea.U_SCGD_CantPen;
//        //                            lineaRequisicion.U_SCGD_CodEst = linea.U_SCGD_CodEst;
//        //                            lineaRequisicion.U_SCGD_Estado = linea.U_SCGD_Estado;
//        //                            break;
//        //                        }
//        //                    }
//        //                }

//        //                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
//        //                oCotizacion.GetByKey(lineasTransferidas[0].U_SCGD_DocOr);

//        //                if (manejaRequisicionesTras != null)
//        //                {
//        //                    if (manejaRequisicionesTras.TrasladoRealizado(ref oCotizacion, ref lineasTransferidas, transferList, oRequisicionData.TipoRequisicion))
//        //                    {
//        //                        oCompanyService = CompanySBO.GetCompanyService();
//        //                        oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");

//        //                        oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
//        //                        oGeneralParams.SetProperty("DocEntry", lineasTransferidas[0].DocEntry);
//        //                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);

//        //                        oChildren = oGeneralData.Child("SCGD_LINEAS_REQ");

//        //                        foreach (LineaRequisicion lineaRequisicion in lineasTransferidas)
//        //                        {
//        //                            for (int i = 0; i <= oChildren.Count - 1; i++)
//        //                            {
//        //                                oChild = oChildren.Item(i);

//        //                                if (oChild.GetProperty("U_SCGD_CodArticulo").ToString() == lineaRequisicion.U_SCGD_CodArticulo && oChild.GetProperty("U_SCGD_LNumOr").ToString().Trim() == lineaRequisicion.U_SCGD_LNumOr.ToString().Trim())
//        //                                {
//        //                                    oChild.SetProperty("U_SCGD_CantRec", lineaRequisicion.U_SCGD_CantRec);
//        //                                    oChild.SetProperty("U_SCGD_CantPen", lineaRequisicion.U_SCGD_CantPen);
//        //                                    oChild.SetProperty("U_SCGD_CodEst", lineaRequisicion.U_SCGD_CodEst);
//        //                                    oChild.SetProperty("U_SCGD_Estado", lineaRequisicion.U_SCGD_Estado);
//        //                                    break;
//        //                                }
//        //                            }
//        //                        }

//        //                        oChildren = oGeneralData.Child("SCGD_MOVS_REQ");

//        //                        foreach (StockTransfer transfer in transferList)
//        //                        {
//        //                            for (int i = 0; i <= transfer.Lines.Count - 1; i++)
//        //                            {
//        //                                transfer.Lines.SetCurrentLine(i);
//        //                                oChild = oChildren.Add();

//        //                                oChild.SetProperty("U_SCGD_CodArticulo", transfer.Lines.ItemCode);
//        //                                oChild.SetProperty("U_SCGD_DescArticulo", transfer.Lines.ItemDescription);
//        //                                oChild.SetProperty("U_SCGD_DocEntry", transfer.DocEntry.ToString());
//        //                                oChild.SetProperty("U_SCGD_DocNum", transfer.DocNum.ToString());
//        //                                oChild.SetProperty("U_SCGD_TipoDoc", "67");
//        //                                oChild.SetProperty("U_SCGD_CantTransf", transfer.Lines.Quantity.ToString(n));
//        //                                oChild.SetProperty("U_SCGD_FechaDoc", transfer.CreationDate);
//        //                            }

//        //                        }

//        //                        foreach (LineaRequisicion lineaRequisicion in oRequisicionData.LineasRequisicion)
//        //                        {
//        //                            if (lineaRequisicion.U_SCGD_CodEst != 3 && lineaRequisicion.U_SCGD_CantRec != lineaRequisicion.U_SCGD_CantSol)
//        //                            {
//        //                                transferida = false;
//        //                                break;
//        //                            }

//        //                        }

//        //                        if (transferida)
//        //                        {
//        //                            oGeneralData.SetProperty("U_SCGD_CodEst", ((int)GeneralEnums.EstadoRequisicion.Trasladado).ToString());
//        //                            oGeneralData.SetProperty("U_SCGD_Est", GeneralEnums.EstadoRequisicion.Trasladado.ToString());
//        //                        }

//        //                        if (!CompanySBO.InTransaction)
//        //                            CompanySBO.StartTransaction();

//        //                        if (oCotizacion.Update() == 0)
//        //                        {
//        //                            if (m_blnActualizaCot)
//        //                                m_blnActualizaCot = false;

//        //                            oGeneralService.Update(oGeneralData);
//        //                            CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
//        //                            ApplicationSBO.StatusBar.SetText(Resource.MensajeTrasladoSatisfactorio, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
//        //                            CargaRequisicion(oRequisicionData.DocEntry.ToString());
//        //                            ActualizaLineasAlCargar();
//        //                        }
//        //                        else
//        //                        {
//        //                            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
//        //                            throw new Exception(string.Format("Error:{0} - {1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Utilitarios.ManejadorErrores(ex);
//        //    }
//        //}

//        //protected virtual void CancelarItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        //{
//        //    SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
//        //    SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
//        //    SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
//        //    SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);
//        //    SAPbobsCOM.Documents oCotizacion = default(SAPbobsCOM.Documents);
//        //    string error = string.Empty;
//        //    SAPbouiCOM.Matrix matrixReq = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxReq").Specific;

//        //    try
//        //    {
//        //        if (pVal.BeforeAction)
//        //        {
//        //            if (!ValidaCotizacionAbierta())
//        //                error = Resource.txtErrorCotizacionNoAbierta;
//        //            else if (!ValidaOTAbierta())
//        //            {
//        //                error = Resource.txtErrorOTNoAbierta;
//        //            }

//        //            if (!string.IsNullOrEmpty(error))
//        //            {
//        //                ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//        //                bubbleEvent = false;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            matrixReq.FlushToDataSource();
//        //            CargarObjRequisicion();

//        //            List<LineaRequisicion> canceladas = new List<LineaRequisicion>();
//        //            foreach (var linea in oRequisicionData.LineasRequisicion)
//        //            {
//        //                if (linea.U_SCGD_Chk == 1)
//        //                {
//        //                    EstadosLineas estadoLinea = (EstadosLineas)linea.U_SCGD_CodEst;
//        //                    if (estadoLinea != EstadosLineas.Pendiente && linea.U_SCGD_CantRec != 0)
//        //                    {
//        //                        error = string.Format(Resource.ErrorCancelaLinea, linea.DataSourceOffset + 1);
//        //                        Localize(linea, TipoMensaje.NoSePuedeCancelarLinea, error);
//        //                        ApplicationSBO.StatusBar.SetText(error);
//        //                        return;
//        //                    }
//        //                    DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas);
//        //                    linea.U_SCGD_CodEst = (int)EstadosLineas.Cancelado;
//        //                    MatrixRequisiciones.ColumnaCodigoEstado.AsignaValorDataSource(linea.U_SCGD_CodEst, linea.DataSourceOffset, dbDataSource);
//        //                    string estado = EstadosLineas.Cancelado.ToString();
//        //                    estado = Localize(linea, TipoMensaje.EstadoLinea, estado);
//        //                    MatrixRequisiciones.ColumnaEstado.AsignaValorDataSource(estado, linea.DataSourceOffset, dbDataSource);
//        //                    dbDataSource.SetValue(MatrixRequisiciones.ColumnaLineaFechaMovimiento.ColumnaLigada, linea.DataSourceOffset, DateTime.Now.ToString("yyyyMMdd"));
//        //                    dbDataSource.SetValue(MatrixRequisiciones.ColumnaLineaHoraMovimiento.ColumnaLigada, linea.DataSourceOffset, DateTime.Now.ToString("HHmm"));
//        //                    dbDataSource.SetValue(MatrixRequisiciones.ColumnaLineaTipoMovimiento.ColumnaLigada, linea.DataSourceOffset, "2");
//        //                    canceladas.Add(linea);

//        //                    CheckBoxSelTodo.AsignaValorUserDataSource("N");
//        //                }
//        //            }

//        //            if (canceladas.Count != 0)
//        //            {
//        //                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
//        //                oCotizacion.GetByKey(canceladas[0].U_SCGD_DocOr);

//        //                oCompanyService = CompanySBO.GetCompanyService();
//        //                oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");

//        //                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
//        //                oGeneralParams.SetProperty("DocEntry", canceladas[0].DocEntry.ToString());
//        //                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

//        //                if (ActualizaEstadoLineasRequisicion(ref oGeneralData, ref canceladas))
//        //                {
//        //                    switch (AsignaEstadoReq(oRequisicionData.LineasRequisicion))
//        //                    {
//        //                        case 2:
//        //                            oGeneralData.SetProperty("U_SCGD_CodEst", ((int)GeneralEnums.EstadoRequisicion.Trasladado).ToString());
//        //                            oGeneralData.SetProperty("U_SCGD_Est", Resource.strTrasladado);
//        //                            break;
//        //                        case 3:
//        //                            oGeneralData.SetProperty("U_SCGD_CodEst", ((int)GeneralEnums.EstadoRequisicion.Cancelado).ToString());
//        //                            oGeneralData.SetProperty("U_SCGD_Est", Resource.strCancelado);
//        //                            break;
//        //                    }

//        //                    if (manejaRequisicionesTras != null)
//        //                    {
//        //                        manejaRequisicionesTras.LineasCanceladas(ref oCotizacion, oRequisicionData, ref canceladas);
//        //                        if (!CompanySBO.InTransaction)
//        //                            CompanySBO.StartTransaction();

//        //                        if (oCotizacion.Update() == 0)
//        //                        {
//        //                            if (m_blnActualizaCot)
//        //                                m_blnActualizaCot = false;

//        //                            oGeneralService.Update(oGeneralData);

//        //                            CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
//        //                            ApplicationSBO.StatusBar.SetText(Resource.MSJLineasCanceladas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
//        //                        }
//        //                        else
//        //                        {
//        //                            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
//        //                            throw new Exception(string.Format("Error:{0} - {1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
//        //                        }
//        //                    }
//        //                }
//        //                ActualizaLineasAlCargar();
//        //                MatrixRequisiciones.Matrix.LoadFromDataSource();
//        //                MatrixMovimientos.EliminaPrimeraLinea();
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        if (CompanySBO.InTransaction)
//        //            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
//        //        Utilitarios.ManejadorErrores(ex);
//        //    }
//        //    finally
//        //    {
//        //        Helpers.DestruirObjeto(ref oCotizacion);
//        //        Helpers.DestruirObjeto(ref oGeneralData);
//        //        Helpers.DestruirObjeto(ref oGeneralParams);
//        //        Helpers.DestruirObjeto(ref oGeneralService);
//        //        Helpers.DestruirObjeto(ref oCompanyService);
//        //    }
//        //}

//        private int AsignaEstadoReq(List<LineaRequisicion> p_oLineasRequisicion)
//        {
//            int intCanceladas, intTrasladadas, intTotal, intResult;
//            try
//            {
//                intResult = 1;
//                intCanceladas = p_oLineasRequisicion.Count(x => x.U_SCGD_CodEst == 3);
//                intTrasladadas = p_oLineasRequisicion.Count(x => x.U_SCGD_CodEst == 2);
//                intTotal = p_oLineasRequisicion.Count();
//                if (intTotal == intCanceladas)
//                    intResult = 3;
//                else
//                    if (intTotal == intTrasladadas || intTotal == (intCanceladas + intTrasladadas))
//                        intResult = 2;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            return intResult;
//        }

//        //manejo del boton de ajuste de cantidades en la requisicion y en la cotizacion
//        //protected virtual void AjusteCantidadesItemPressed(string formUid, ItemEvent pVal, ref bool bubbleEvent)
//        //{
//        //    SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
//        //    SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
//        //    SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
//        //    SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);
//        //    SAPbobsCOM.GeneralData oChild = default(SAPbobsCOM.GeneralData);
//        //    SAPbobsCOM.GeneralDataCollection oChildren = default(SAPbobsCOM.GeneralDataCollection);

//        //    SAPbobsCOM.Documents oCotizacion = default(SAPbobsCOM.Documents);
//        //    SAPbobsCOM.Document_Lines m_oLineasCotizacion = default(SAPbobsCOM.Document_Lines);
//        //    SAPbouiCOM.Matrix matrixReq = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxReq").Specific;

//        //    string error = string.Empty;
//        //    ManejadorArticulos manejadorArticulos = new ManejadorArticulos(CompanySBO);
//        //    Boolean transferida = true;

//        //    //List<LineaRequisicion> lineasTransferidas = new List<LineaRequisicion>();
//        //    //List<StockTransfer> transferList = new List<StockTransfer>();

//        //    try
//        //    {
//        //        if (pVal.BeforeAction)
//        //        {
//        //            matrixReq.FlushToDataSource();
//        //            CargarObjRequisicion();
//        //            if (oRequisicionData.LineasRequisicion.Any(x => x.U_SCGD_Chk == 1))
//        //            {
//        //                foreach (LineaRequisicion linea in oRequisicionData.LineasRequisicion.Where(x => x.U_SCGD_Chk == 1).ToList())
//        //                {
//        //                    if (linea.U_SCGD_CodEst != (int)EstadosLineas.Pendiente)
//        //                    {
//        //                        error = string.Format(Resource.msjLineaCanceladaTrasladada, linea.DataSourceOffset + 1);
//        //                        break;
//        //                    }
//        //                    else if (linea.U_SCGD_CAju > linea.U_SCGD_CantPen)
//        //                    {
//        //                        error = string.Format(Resource.ErrorAjuste, linea.DataSourceOffset + 1);
//        //                        ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
//        //                    }
//        //                }
//        //            }
//        //            else
//        //            {
//        //                error = Resource.ErrNoLineaSel;
//        //            }

//        //            if (!string.IsNullOrEmpty(error))
//        //            {
//        //                ApplicationSBO.StatusBar.SetText(error, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
//        //                bubbleEvent = false;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            //if (Traslada(ref transferList, ref lineasTransferidas))
//        //            //{
//        //            oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
//        //            if (oCotizacion.GetByKey(oRequisicionData.LineasRequisicion.First(x => x.U_SCGD_Chk == 1).U_SCGD_DocOr))
//        //            {
//        //                m_oLineasCotizacion = oCotizacion.Lines;

//        //                foreach (LineaRequisicion lineaTransferida in oRequisicionData.LineasRequisicion.Where(x => x.U_SCGD_Chk == 1).ToList())
//        //                {
//        //                    EstadosLineas estadoLinea = (EstadosLineas)lineaTransferida.U_SCGD_CodEst;
//        //                    if (estadoLinea != EstadosLineas.Trasladado)//  && linea.CantidadRecibida != 0)
//        //                    {
//        //                        if (lineaTransferida.U_SCGD_CAju < lineaTransferida.U_SCGD_CantSol && lineaTransferida.U_SCGD_CAju <= lineaTransferida.U_SCGD_CantPen)
//        //                        {
//        //                            lineaTransferida.U_SCGD_CantSol = lineaTransferida.U_SCGD_CantSol - lineaTransferida.U_SCGD_CAju;
//        //                            lineaTransferida.U_SCGD_CantPen = lineaTransferida.U_SCGD_CantSol - lineaTransferida.U_SCGD_CantRec;

//        //                            if (lineaTransferida.U_SCGD_CantSol == lineaTransferida.U_SCGD_CantRec)
//        //                            {
//        //                                lineaTransferida.U_SCGD_CodEst = (int)GeneralEnums.EstadoRequisicion.Trasladado;
//        //                                lineaTransferida.U_SCGD_Estado = Resource.strTrasladado;
//        //                            }
//        //                        }
//        //                        //else if (lineaTransferida.U_SCGD_CAju == lineaTransferida.U_SCGD_CantPen)
//        //                        //{
//        //                        //    lineaTransferida.U_SCGD_CantSol = lineaTransferida.U_SCGD_CantSol - lineaTransferida.U_SCGD_CAju;
//        //                        //    lineaTransferida.U_SCGD_CantPen = lineaTransferida.U_SCGD_CantSol - lineaTransferida.U_SCGD_CantRec;
//        //                        //}
//        //                    }

//        //                    for (int i = 0; i <= m_oLineasCotizacion.Count - 1; i++)
//        //                    {
//        //                        m_oLineasCotizacion.SetCurrentLine(i);
//        //                        if (m_oLineasCotizacion.LineNum == lineaTransferida.U_SCGD_LNumOr && m_oLineasCotizacion.ItemCode == lineaTransferida.U_SCGD_CodArticulo)
//        //                        {
//        //                            if (lineaTransferida.U_SCGD_CantRec == lineaTransferida.U_SCGD_CantSol)
//        //                            {
//        //                                m_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2;
//        //                            }
//        //                            m_oLineasCotizacion.Quantity = lineaTransferida.U_SCGD_CantSol;
//        //                            break;
//        //                        }
//        //                    }
//        //                }

//        //            }

//        //            oCompanyService = CompanySBO.GetCompanyService();
//        //            oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ");

//        //            oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
//        //            oGeneralParams.SetProperty("DocEntry", oRequisicionData.LineasRequisicion.First(x => x.U_SCGD_Chk == 1).DocEntry);
//        //            oGeneralData = oGeneralService.GetByParams(oGeneralParams);

//        //            oChildren = oGeneralData.Child("SCGD_LINEAS_REQ");

//        //            foreach (LineaRequisicion lineaRequisicion in oRequisicionData.LineasRequisicion.Where(x => x.U_SCGD_Chk == 1).ToList())
//        //            {
//        //                for (int i = 0; i <= oChildren.Count - 1; i++)
//        //                {
//        //                    oChild = oChildren.Item(i);

//        //                    if (oChild.GetProperty("U_SCGD_CodArticulo").ToString() == lineaRequisicion.U_SCGD_CodArticulo && oChild.GetProperty("U_SCGD_LNumOr").ToString().Trim() == lineaRequisicion.U_SCGD_LNumOr.ToString().Trim())
//        //                    {
//        //                        oChild.SetProperty("U_SCGD_CantSol", lineaRequisicion.U_SCGD_CantSol);
//        //                        oChild.SetProperty("U_SCGD_CantPen", lineaRequisicion.U_SCGD_CantPen);
//        //                        oChild.SetProperty("U_SCGD_CodEst", lineaRequisicion.U_SCGD_CodEst);
//        //                        oChild.SetProperty("U_SCGD_Estado", lineaRequisicion.U_SCGD_Estado);
//        //                        break;
//        //                    }
//        //                }
//        //            }

//        //            foreach (LineaRequisicion lineaRequisicion in oRequisicionData.LineasRequisicion)
//        //            {
//        //                if (lineaRequisicion.U_SCGD_CantRec != lineaRequisicion.U_SCGD_CantSol)
//        //                    transferida = false;
//        //            }

//        //            if (transferida)
//        //            {
//        //                oGeneralData.SetProperty("U_SCGD_CodEst", ((int)GeneralEnums.EstadoRequisicion.Trasladado).ToString());
//        //                oGeneralData.SetProperty("U_SCGD_Est", GeneralEnums.EstadoRequisicion.Trasladado.ToString());
//        //            }

//        //            if (!CompanySBO.InTransaction)
//        //                CompanySBO.StartTransaction();

//        //            if (oCotizacion.Update() == 0)
//        //            {
//        //                if (m_blnActualizaCot)
//        //                    m_blnActualizaCot = false;

//        //                oGeneralService.Update(oGeneralData);
//        //                CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
//        //                ApplicationSBO.StatusBar.SetText(Resource.MensajeTrasladoSatisfactorio, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
//        //                CargaRequisicion(oRequisicionData.DocEntry.ToString());
//        //                ActualizaLineasAlCargar();
//        //            }
//        //            else
//        //            {
//        //                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
//        //                throw new Exception(string.Format("Error:{0} - {1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
//        //            }

//        //            CheckBoxSelTodo.AsignaValorUserDataSource("N");
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        //Revisar
//        //        //Utilitarios.ManejadorErrores(ex);
//        //    }
//        //}

//        public void ActualizaEntregadoLineas()
//        {
//            string l_strEntregado;
//            string l_strNumOT;
//            int l_intDocEntry;

//            SAPbobsCOM.Documents oCotizacion;
//            SAPbobsCOM.Document_Lines oLineasCotizacion;

//            try
//            {
//                l_strEntregado = CheckBoxEntregado.ObtieneValorDataSource();

//                if (l_strEntregado == "")
//                    l_strEntregado = "N";

//                l_strNumOT = EditTextNoOrden.ObtieneValorDataSource();

//                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
//                dtLocal.Clear();

//                dtLocal.ExecuteQuery(string.Format(DMS_Connector.Queries.GetStrSpecificQuery("strFRCotDocEntry"), l_strNumOT));
//                l_intDocEntry = (int)dtLocal.GetValue("DocEntry", 0);

//                oCotizacion = (SAPbobsCOM.Documents)CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
//                if (oCotizacion.GetByKey(l_intDocEntry))
//                {
//                    oLineasCotizacion = oCotizacion.Lines;

//                    DBDataSource dbDataSource = MatrixRequisiciones.FormularioSBO.DataSources.DBDataSources.Item(MatrixRequisiciones.TablaLigada);

//                    for (int i = 0; i < dbDataSource.Size; i++)
//                    {
//                        for (int j = 0; j < oLineasCotizacion.Count; j++)
//                        {
//                            oLineasCotizacion.SetCurrentLine(j);
//                            if (int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_SCGD_LNumOr", i).Trim()) == oLineasCotizacion.LineNum)
//                            {
//                                if (Convert.ToDouble(oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value, n) == oLineasCotizacion.Quantity)
//                                {
//                                    if (m_blnValidaEntregado)
//                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Entregado").Value = l_strEntregado;
//                                }
//                                if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim() != oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value.ToString().Trim())
//                                    oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Obs_Req").Value = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").GetValue("U_Obs_Req", i).Trim();
//                            }
//                        }
//                    }

//                    if (!CompanySBO.InTransaction)
//                        CompanySBO.StartTransaction();

//                    if (oCotizacion.Update() == 0)
//                    {
//                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit);
//                        m_blnActualizaCot = false;
//                    }
//                    else
//                    {
//                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack);
//                        throw new Exception(string.Format("Error:{0} - {1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                //Revisar
//                //Utilitarios.ManejadorErrores(ex);
//            }
//        }

//        public void CargaRequisicion(string strReqDocEntry)
//        {
//            SAPbouiCOM.Conditions oConditions;
//            SAPbouiCOM.Condition oCondition;

//            try
//            {
//                if (FormularioSBO != null)
//                {
//                    FormularioSBO.Freeze(true);
//                    oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
//                    oCondition = oConditions.Add();

//                    oCondition.Alias = "DocEntry";
//                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
//                    oCondition.CondVal = strReqDocEntry;

//                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").Query(oConditions);
//                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").Query(oConditions);
//                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_MOVS_REQ").Query(oConditions);
//                    //ManejadorEventoFormDataLoad((SAPbouiCOM.Form)FormularioSBO);

//                    MatrixRequisiciones.Matrix.LoadFromDataSource();
//                    MatrixMovimientos.Matrix.LoadFromDataSource();

//                    FormularioSBO.Refresh();
//                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
//                    ActualizaLineasAlCargar();

//                    MatrixRequisiciones.Especifico.LoadFromDataSource();
//                    MatrixMovimientos.EliminaPrimeraLinea();
//                    MatrixMovimientos.Especifico.LoadFromDataSource();
//                    CargarObjRequisicion();

//                    FormularioSBO.Freeze(false);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
//            }
//        }

//        private bool ActualizaEstadoLineasRequisicion(ref SAPbobsCOM.GeneralData p_oGeneralData, ref List<LineaRequisicion> p_LineasCanceladas)
//        {
//            SAPbobsCOM.GeneralData oChild = default(SAPbobsCOM.GeneralData);
//            SAPbobsCOM.GeneralDataCollection oChildren = default(SAPbobsCOM.GeneralDataCollection);
//            var result = false;
//            try
//            {
//                oChildren = p_oGeneralData.Child("SCGD_LINEAS_REQ");

//                foreach (LineaRequisicion lineaRequisicion in p_LineasCanceladas)
//                {
//                    for (int i = 0; i <= oChildren.Count - 1; i++)
//                    {
//                        oChild = oChildren.Item(i);

//                        if (oChild.GetProperty("U_SCGD_CodArticulo").ToString() == lineaRequisicion.U_SCGD_CodArticulo && oChild.GetProperty("U_SCGD_LNumOr").ToString().Trim() == lineaRequisicion.U_SCGD_LNumOr.ToString().Trim())
//                        {
//                            oChild.SetProperty("U_SCGD_CodEst", lineaRequisicion.U_SCGD_CodEst);
//                            result = true;
//                            break;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                Helpers.DestruirObjeto(ref oChildren);
//                Helpers.DestruirObjeto(ref oChild);
//            }
//            return result;
//        }

//        /// <summary>
//        /// Realiza la transferencia de Stock
//        /// </summary>
//        /// <returns></returns>
//        public Boolean Traslada(ref List<StockTransfer> p_transferencias, ref List<LineaRequisicion> p_lineasTrasnfer)
//        {
//            var creaTrans = false;
//            StockTransfer stockTransfer = (StockTransfer)CompanySBO.GetBusinessObject(BoObjectTypes.oStockTransfer);
//            List<string> listaBodegasOrigen = new List<string>();
//            try
//            {
//                foreach (var linea in oRequisicionData.LineasRequisicion)
//                {
//                    if (linea.U_SCGD_Chk == 1)
//                    {
//                        if (!listaBodegasOrigen.Contains(linea.U_SCGD_CodBodOrigen))
//                        {
//                            listaBodegasOrigen.Add(linea.U_SCGD_CodBodOrigen);
//                            stockTransfer.CardCode = oRequisicionData.CodigoCliente;
//                            stockTransfer.FromWarehouse = linea.U_SCGD_CodBodOrigen;
//                            stockTransfer.ToWarehouse = linea.U_SCGD_CodBodDest;
//                            stockTransfer.Comments = oRequisicionData.Comentario;
//                            stockTransfer.SetUdf(oRequisicionData.NoOrden, "U_SCGD_Numero_OT");

//                            if (!string.IsNullOrEmpty(oRequisicionData.Placa))
//                            {
//                                stockTransfer.SetUdf(oRequisicionData.Placa, "U_SCGD_Num_Placa");
//                            }
//                            if (!string.IsNullOrEmpty(oRequisicionData.Marca))
//                            {
//                                stockTransfer.SetUdf(oRequisicionData.Marca, "U_SCGD_Des_Marc");
//                            }
//                            if (!string.IsNullOrEmpty(oRequisicionData.Estilo))
//                            {
//                                stockTransfer.SetUdf(oRequisicionData.Estilo, "U_SCGD_Des_Esti");
//                            }
//                            if (!string.IsNullOrEmpty(oRequisicionData.VIN))
//                            {
//                                stockTransfer.SetUdf(oRequisicionData.VIN, "U_SCGD_Num_VIN");
//                            }

//                            stockTransfer.SetUdf(oRequisicionData.Marca, mc_strU_Marca);
//                            stockTransfer.SetUdf(oRequisicionData.Estilo, mc_strU_Estilo);
//                            stockTransfer.SetUdf(oRequisicionData.VIN, mc_strU_VIN);
//                            stockTransfer.SetUdf(oRequisicionData.DocumentoOrigen, mc_strIntCodigoCotizacion);
//                            stockTransfer.SetUdf(oRequisicionData.CodigoTipoRequisicion, mc_strTipoTransferenciaUdf);
//                            if (oRequisicionData.Serie > 0)
//                            {
//                                stockTransfer.Series = oRequisicionData.Serie;
//                            }
//                            else
//                                stockTransfer.Series = Convert.ToInt32(DMS_Connector.Configuracion.ConfiguracionSucursales.First(x => x.U_Sucurs == oRequisicionData.SucursalID).U_SerInv);

//                            stockTransfer.Lines.ItemCode = linea.U_SCGD_CodArticulo;
//                            stockTransfer.Lines.ItemDescription = linea.U_SCGD_DescArticulo;
//                            stockTransfer.Lines.WarehouseCode = linea.U_SCGD_CodBodDest;
//                            stockTransfer.Lines.FromWarehouseCode = linea.U_SCGD_CodBodOrigen;
//                            stockTransfer.Lines.Quantity = linea.U_SCGD_CantATransf;
//                            stockTransfer.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = linea.U_SCGD_CodTipoArt.ToString();

//                            //*******************************para Ubicaciones**********************************
//                            if (CompanySBO.Version >= 900000)
//                            {
//                                AgregarUbicaciones(stockTransfer, linea);
//                            }

//                            stockTransfer.Lines.Add();
//                            p_transferencias.Add(stockTransfer);
//                        }
//                        else
//                        {
//                            stockTransfer = p_transferencias.FirstOrDefault(x => x.FromWarehouse == linea.U_SCGD_CodBodOrigen);
//                            if (stockTransfer != null)
//                            {
//                                p_transferencias.Remove(stockTransfer);

//                                stockTransfer.Lines.ItemCode = linea.U_SCGD_CodArticulo;
//                                stockTransfer.Lines.ItemDescription = linea.U_SCGD_DescArticulo;
//                                stockTransfer.Lines.WarehouseCode = linea.U_SCGD_CodBodDest;
//                                stockTransfer.Lines.FromWarehouseCode = linea.U_SCGD_CodBodOrigen;
//                                stockTransfer.Lines.Quantity = linea.U_SCGD_CantATransf;
//                                stockTransfer.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = linea.U_SCGD_CodTipoArt.ToString();

//                                //*******************************para Ubicaciones**********************************
//                                if (CompanySBO.Version >= 900000)
//                                {
//                                    AgregarUbicaciones(stockTransfer, linea);
//                                }

//                                stockTransfer.Lines.Add();

//                                p_transferencias.Add(stockTransfer);
//                            }
//                        }

//                        p_lineasTrasnfer.Add(linea);
//                    }
//                }

//                foreach (StockTransfer transfer in p_transferencias)
//                {
//                    int error = transfer.Add();
//                    if (error == 0)
//                    {
//                        string newObjectKey = CompanySBO.GetNewObjectKey();
//                        transfer.GetByKey(int.Parse(newObjectKey));
//                        creaTrans = true;
//                    }
//                    else
//                    {
//                        throw new Exception(string.Format("Error: {0} - {1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                //Revisar
//                //Utilitarios.ManejadorErrores(ex);
//                creaTrans = false;
//            }

//            return creaTrans;
//        }

//        public void AgregarUbicaciones(StockTransfer p_stockTransfer, LineaRequisicion p_LineaRequisicion)
//        {
//            if (!string.IsNullOrEmpty(p_LineaRequisicion.U_DeUbic))
//            {
//                p_stockTransfer.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
//                p_stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt16(p_LineaRequisicion.U_DeUbic);
//                p_stockTransfer.Lines.BinAllocations.Quantity = p_LineaRequisicion.U_SCGD_CantATransf;
//                p_stockTransfer.Lines.BinAllocations.Add();
//            }

//            if (!string.IsNullOrEmpty(p_LineaRequisicion.U_AUbic))
//            {
//                p_stockTransfer.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
//                p_stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt16(p_LineaRequisicion.U_AUbic);
//                p_stockTransfer.Lines.BinAllocations.Quantity = p_LineaRequisicion.U_SCGD_CantATransf;
//                p_stockTransfer.Lines.BinAllocations.Add();
//            }
//        }

//        private void AsignarValoresUbicaciones(ref SAPbouiCOM.DataTable p_dtValues, int p_rowNum)
//        {
//            string strBinCode = String.Empty;
//            int intTipoRequisicion = 0;
//            try
//            {
//                strBinCode = p_dtValues.GetValue("BinCode", 0).ToString().Trim();
//                if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_CodTipoReq", 0)))
//                    intTipoRequisicion = Convert.ToInt16(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_REQUISICIONES").GetValue("U_SCGD_CodTipoReq", 0));
//                if (!string.IsNullOrEmpty(strBinCode))
//                    switch (intTipoRequisicion)
//                    {
//                        case 1:
//                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").SetValue("U_DesDeUbic", p_rowNum - 1, strBinCode);
//                            break;
//                        case 2:
//                            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_REQ").SetValue("U_DesAUbic", p_rowNum - 1, strBinCode);
//                            break;
//                    }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        #endregion

//        #region ...Eventos...

//        //public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent, ref ListaUbicaciones m_oFormSeleccionUbicaciones)
//        //{

//        //    //bubbleEvent = true;
//        //    if (pVal.FormTypeEx != FormType) return;

//        //    if (!string.IsNullOrEmpty(pVal.ItemUID))
//        //    {
//        //        Item item = FormularioSBO.Items.Item(pVal.ItemUID);
//        //        if (item != null && !item.Enabled) return;
//        //    }
//        //    switch (pVal.EventType)
//        //    {
//        //        case BoEventTypes.et_ITEM_PRESSED:

//        //            FormularioSBO.Freeze(true);

//        //            if (pVal.ItemUID == FolderMovimientos.UniqueId || pVal.ItemUID == FolderRequisiciones.UniqueId)
//        //                FolderMovimientosItemPressed(formUid, pVal, ref bubbleEvent);
//        //            else if (pVal.ItemUID == ButtonSBOTrasladar.UniqueId)
//        //                ButtonSBOTrasladarItemPress(formUid, pVal, ref bubbleEvent);
//        //            else if (pVal.ItemUID == ButtonCancelar.UniqueId)
//        //                CancelarItemPressed(formUid, pVal, ref bubbleEvent);
//        //            else if (pVal.ItemUID == ButtonGenerarReporte.UniqueId)
//        //                ButtonSBOGenerarReporteItemPressed(formUid, pVal, ref bubbleEvent);
//        //            else if (pVal.ItemUID == CheckBoxSelTodo.UniqueId)
//        //                SeleccionaTodo(formUid, pVal, ref bubbleEvent);
//        //            else if (pVal.ItemUID == CheckBoxEntregado.UniqueId || pVal.ColUID == MatrixRequisiciones.ColumnaLineaObservcacion.UniqueId)
//        //            {
//        //                if (pVal.ActionSuccess) m_blnActualizaCot = true;
//        //            }
//        //            else if (pVal.ItemUID == "btnAjuste")
//        //            {
//        //                AjusteCantidadesItemPressed(formUid, pVal, ref bubbleEvent);
//        //            }
//        //            else if (pVal.ItemUID == "1")
//        //            {
//        //                if (pVal.ActionSuccess && m_blnActualizaCot)
//        //                    ActualizaEntregadoLineas();
//        //            }

//        //            FormularioSBO.Freeze(false);
//        //            break;
//        //        case BoEventTypes.et_CHOOSE_FROM_LIST:
//        //            ManejadorEventoChooseFromList(ref pVal, ref bubbleEvent);
//        //            break;
//        //    }

//        //}


//        //public virtual void ApplicationSBOOnFormDataEvent(ref BusinessObjectInfo businessObjectInfo,
//        //                                                     ref bool bubbleEvent)
//        //{
//        //    //bubbleEvent = true;
//        //    if (businessObjectInfo.FormTypeEx != FormType) return;
//        //    if (businessObjectInfo.ActionSuccess)
//        //    {
//        //        switch (businessObjectInfo.EventType)
//        //        {
//        //            case BoEventTypes.et_FORM_DATA_LOAD:
//        //                DataLoadEvent(businessObjectInfo, ref bubbleEvent);
//        //                break;

//        //            case BoEventTypes.et_FORM_LOAD:
//        //                //LlenarComboSucursal();
//        //                break;
//        //        }
//        //    }
//        //}

//        public void ManejadorEventoChooseFromList(ref SAPbouiCOM.ItemEvent pval, ref bool BubbleEvent)
//        {
//            try
//            {
//                SAPbouiCOM.IChooseFromListEvent oCFLEvento;
//                oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pval;
//                string sCFL_ID = null;
//                sCFL_ID = oCFLEvento.ChooseFromListUID;
//                SAPbouiCOM.Form oForm;
//                oForm = (SAPbouiCOM.Form)FormularioSBO;
//                SAPbouiCOM.ChooseFromList oCFL = oForm.ChooseFromLists.Item(sCFL_ID);

//                SAPbouiCOM.Condition oCondition;
//                SAPbouiCOM.Conditions oConditions;

//                if (oCFLEvento.ActionSuccess)
//                {
//                    SAPbouiCOM.DataTable oDataTable = default(SAPbouiCOM.DataTable);
//                    oDataTable = oCFLEvento.SelectedObjects;

//                    if ((pval.ItemUID == "mtxReq"))
//                    {
//                        if ((oCFLEvento.SelectedObjects != null))
//                        {
//                            AsignarValoresUbicaciones(ref oDataTable, pval.Row);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
//            }
//        }
//        #endregion

//    }
//}
