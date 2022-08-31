using System.Globalization;
using System.Threading;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using SCG.SBOFramework.UI.Extensions;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Placas
{
    public delegate Form CargaFormularioRevisionVDelegate(IFormularioSBO form);

    public partial class ExpedienteFormulario : IFormularioSBO, IUsaMenu
    {
        /// <summary>
        /// Variables para le manejo de los direferentes atributos de los compontentes de la interfaz del formulario
        /// </summary>

        public EditTextSBO EditTextCodigo;
        public EditTextSBO EditTextCodigoCliente;
        public EditTextSBO EditTextNombreCliente;
        public EditTextSBO EditTextUnidad;
        public EditTextSBO EditTextPlaca;
        public EditTextSBO EditTextPlacaAGV;
        public EditTextSBO EditTextTomo;
        public EditTextSBO EditTextAsiento;
        public EditTextSBO EditTextNoChasis;
        public EditTextSBO EditTextNoMotor;
        public EditTextSBO EditTextMarca;
        public EditTextSBO EditTextEstilo;
        public EditTextSBO EditTextModelo;
        public EditTextSBO EditTextColor;
        public EditTextSBO EditTextAnno;
        public EditTextSBO EditTextNoCV;
        public EditTextSBO EditTextNoFactura;
        public EditTextSBO EditTextSucursal;
        public EditTextSBO EditTextObservacion;
        
        public CheckBoxSBO CheckBoxFinalizado;

        public ComboBoxSBO ComboBoxProblema;

        public FolderSBO FolderRevVehicular;
        public FolderSBO FolderDocLegales;
        public FolderSBO FolderInscripcion;
        public FolderSBO FolderGastos;

        public MatrixSBORevVehicular MatrixRevVehicular;
        public MatrixSBODocLegales MatrixDocLegales;
        public MatrixSBOInscripcion MatrixInscripcion;
        public MatrixSBOGastos MatrixGastos;

        public ButtonSBO ButtomAgregar;
        public ButtonSBO ButtomBorrar;
        public ButtonSBO ButtomImpAcuerdo;
        public ButtonSBO ButtonUnidad;
        public ButtonSBO ButtonEnteFin;
        public ButtonSBO ButtonRefrescar;

        /// <summary>
        /// Variables para le manejo de los direferentes atributos de los compontentes de la interfaz del folder de Revisión Vehícular
        /// </summary>

        public ComboBoxSBO ComboBoxGestionRV;
        public ComboBoxSBO ComboBoxEventoRV;
        public EditTextSBO EditTextFechEventoRV;
        public EditTextSBO EditTextNoRef1RV;
        public EditTextSBO EditTextNoRef2RV;
        public EditTextSBO EditTextObservacionesRV;
        
        public EditTextSBO EditTextNoRef3RV;
        public EditTextSBO EditTextNoRef4RV;
        public EditTextSBO EditTextNoRef5RV;
        public EditTextSBO EditTextNoRef6RV;
        public EditTextSBO EditTextFechIngresoRV;

        public ButtonSBO ButtomAgregarRV;
        public ButtonSBO ButtomEditarRV;
        public ButtonSBO ButtomBorrarRV;

        //Agregar Matrix

        /// <summary>
        /// Variables para le manejo de los direferentes atributos de los compontentes de la interfaz del folder de Documentos Legales
        /// </summary>

        public ComboBoxSBO ComboBoxGestionDL;
        public ComboBoxSBO ComboBoxEventoDL;
        public EditTextSBO EditTextFechEventoDL;
        public EditTextSBO EditTextNoRef1DL;
        public EditTextSBO EditTextNoRef2DL;
        public CheckBoxSBO CheckBoxPrenda;
        public EditTextSBO EditTextEnteFinanciero;
        public EditTextSBO EditTextObservacionesDL;

        public ButtonSBO ButtomAgregarDL;
        public ButtonSBO ButtomEditarDL;
        public ButtonSBO ButtomBorrarDL;

        /// <summary>
        /// Variables para le manejo de los direferentes atributos de los compontentes de la interfaz del folder de Seguimiento de Inscripción
        /// </summary>

        public ComboBoxSBO ComboBoxGestionIns;
        public ComboBoxSBO ComboBoxEventoIns;
        public EditTextSBO EditTextFechEventoIns;
        public EditTextSBO EditTextNoRef1Ins;
        public EditTextSBO EditTextNoRef2Ins;
        public EditTextSBO EditTextObservacionesIns;

        public ButtonSBO ButtomAgregarIns;
        public ButtonSBO ButtomEditarIns;
        public ButtonSBO ButtomBorrarIns;

        /// <summary>
        /// Variables para le manejo de los direferentes atributos de los compontentes de la interfaz del folder de Gastos de Inscripción
        /// </summary>

        public ComboBoxSBO ComboBoxGastoG;
        public EditTextSBO EditTextNoDocG;
        public EditTextSBO EditTextFechDocG;
        public EditTextSBO EditTextMontoG;
        public EditTextSBO EditTextObservacionesG;
        public EditTextSBO EditTextTotalG;

        public ButtonSBO ButtomAgregarG;
        public ButtonSBO ButtomEditarG;
        public ButtonSBO ButtomBorrarG;

        public ButtonSBO ButtonExpediente;


        public FolderSBO FolderReportes;

        public DataTable DataTableReportes;

        public ButtonSBO ButtonImprimirRep;

        public MatrixSBOReportes MatrixReportes;

        public string ultimoEventoAgregado;
        public string penultimoEventoAgregado;
        public int panelUltimoEvento;
        public bool eventoFinal;

        //variable que permite manterner el la unidad para el expediente para el momento de crear el expediente, con el fin de poder realizar las validaciones y ingresar la modificacion del estado en los datos maestros del vehiculo
        public string unidadExpediente;

        public ExpedienteFormulario(Application applicationSBO, ICompany companySBO)
        {
            ApplicationSBO = applicationSBO;
            CompanySBO = companySBO;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = My.Resources.Resource.Culture;
            DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            My.Resources.Resource.Culture = cultureInfo;
            
        }

        #region IFormularioSBO Members

        public string FormType { get; set; }

        public string NombreXml { get; set; }

        public string Titulo { get; set; }

        public IForm FormularioSBO { get; set; }

        public bool Inicializado { get; set; }

        public ICompany CompanySBO { get; private set; }

        public IApplication ApplicationSBO { get; private set; }

        public CargaFormularioRevisionVDelegate CargaFormulario { get; set; }

        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {

                FormularioSBO.Freeze(true);

                UserDataSources userDataSources = FormularioSBO.DataSources.UserDataSources;
                userDataSources.Add("enteFin", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("gestRV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("gestDL", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("gestSI", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("eveRV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("eveDL", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("eveSI", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("fechaRV", BoDataType.dt_DATE, 100);
                userDataSources.Add("ref1RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref2RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref3RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref4RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref5RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref6RV", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("fechaIRV", BoDataType.dt_DATE, 100);
                userDataSources.Add("obsRV", BoDataType.dt_LONG_TEXT, 500);
                userDataSources.Add("fechaDL", BoDataType.dt_DATE, 100);
                userDataSources.Add("ref1DL", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref2DL", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("obsDL", BoDataType.dt_LONG_TEXT, 500);
                userDataSources.Add("fechaSI", BoDataType.dt_DATE, 100);
                userDataSources.Add("ref1SI", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("ref2SI", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("obsSI", BoDataType.dt_LONG_TEXT, 500);
                userDataSources.Add("numDoc", BoDataType.dt_LONG_TEXT, 100);
                userDataSources.Add("fechaGas", BoDataType.dt_DATE, 100);
                userDataSources.Add("montoGas", BoDataType.dt_PRICE, 100);
                userDataSources.Add("obsGas", BoDataType.dt_LONG_TEXT, 500);
                userDataSources.Add("prenda", BoDataType.dt_SHORT_TEXT, 4);
                userDataSources.Add("gasto", BoDataType.dt_LONG_TEXT, 200);

                EditTextCodigo = new EditTextSBO("txtCodigo", true, UDOPlaca.TablaEncabezado, "DocNum", FormularioSBO);
                EditTextCodigoCliente = new EditTextSBO("txtClient", true, UDOPlaca.TablaEncabezado, "U_Cod_Clie", FormularioSBO);
                EditTextNombreCliente = new EditTextSBO("txtNomClie", true, UDOPlaca.TablaEncabezado, "U_Nom_Clie", FormularioSBO);
                EditTextUnidad = new EditTextSBO("txtUnidad", true, UDOPlaca.TablaEncabezado, "U_Num_Unid", FormularioSBO);
                EditTextPlaca = new EditTextSBO("txtPlaca", true, UDOPlaca.TablaEncabezado, "U_Placa", FormularioSBO);
                EditTextPlacaAGV = new EditTextSBO("txtPlcAGV", true, UDOPlaca.TablaEncabezado, "U_Plac_AGV", FormularioSBO);
                EditTextTomo = new EditTextSBO("txtTomo", true, UDOPlaca.TablaEncabezado, "U_Tomo", FormularioSBO);
                EditTextAsiento = new EditTextSBO("txtAsiento", true, UDOPlaca.TablaEncabezado, "U_Asiento", FormularioSBO);
                EditTextNoChasis = new EditTextSBO("txtVIN", true, UDOPlaca.TablaEncabezado, "U_Num_VIN", FormularioSBO);
                EditTextNoMotor = new EditTextSBO("txtNoMotor", true, UDOPlaca.TablaEncabezado, "U_Num_Moto", FormularioSBO);
                EditTextMarca = new EditTextSBO("txtMarca", true, UDOPlaca.TablaEncabezado, "U_Marca", FormularioSBO);
                EditTextEstilo = new EditTextSBO("txtEstilo", true, UDOPlaca.TablaEncabezado, "U_Estilo", FormularioSBO);
                EditTextModelo = new EditTextSBO("txtModelo", true, UDOPlaca.TablaEncabezado, "U_Modelo", FormularioSBO);
                EditTextColor = new EditTextSBO("txtColor", true, UDOPlaca.TablaEncabezado, "U_Color", FormularioSBO);
                EditTextAnno = new EditTextSBO("txtAnno", true, UDOPlaca.TablaEncabezado, "U_Anno", FormularioSBO);
                EditTextNoCV = new EditTextSBO("txtNoCV", true, UDOPlaca.TablaEncabezado, "U_Num_CV", FormularioSBO);
                EditTextNoFactura = new EditTextSBO("txtFacVent", true, UDOPlaca.TablaEncabezado, "U_Num_Fact", FormularioSBO);
                EditTextTotalG = new EditTextSBO("txtTotalG",true,UDOPlaca.TablaEncabezado,"U_Total",FormularioSBO);
                EditTextSucursal = new EditTextSBO("txtSucurs", true, UDOPlaca.TablaEncabezado, "U_Sucurs", FormularioSBO);
                EditTextObservacion = new EditTextSBO("txtObserv", true, UDOPlaca.TablaEncabezado, "U_Observ", FormularioSBO);
                CheckBoxFinalizado = new CheckBoxSBO("chkFinaliz", true, UDOPlaca.TablaEncabezado, "U_Finaliz", FormularioSBO);
                ComboBoxProblema = new ComboBoxSBO("cmbProblem", FormularioSBO, true, UDOPlaca.TablaEncabezado, "U_Cod_Prob");
                
                EditTextCodigo.AsignaBinding();
                EditTextCodigoCliente.AsignaBinding();
                EditTextNombreCliente.AsignaBinding();
                EditTextUnidad.AsignaBinding();
                EditTextPlaca.AsignaBinding();
                EditTextPlacaAGV.AsignaBinding();
                EditTextTomo.AsignaBinding();
                EditTextAsiento.AsignaBinding();
                EditTextNoChasis.AsignaBinding();
                EditTextNoMotor.AsignaBinding();
                EditTextMarca.AsignaBinding();
                EditTextEstilo.AsignaBinding();
                EditTextModelo.AsignaBinding();
                EditTextColor.AsignaBinding();
                EditTextAnno.AsignaBinding();
                EditTextNoCV.AsignaBinding();
                EditTextNoFactura.AsignaBinding();
                EditTextTotalG.AsignaBinding();
                EditTextSucursal.AsignaBinding();
                EditTextObservacion.AsignaBinding();
                CheckBoxFinalizado.AsignaBinding();
                ComboBoxProblema.AsignaBinding();

                EditTextEnteFinanciero = new EditTextSBO("txtIntFDL", true, "", "enteFin", FormularioSBO);
                EditTextFechEventoRV = new EditTextSBO("txtFecEvRV", true, "", "fechaRV", FormularioSBO);
                EditTextNoRef1RV = new EditTextSBO("txtRef1RV", true, "", "ref1RV", FormularioSBO);
                EditTextNoRef2RV = new EditTextSBO("txtRef2RV", true, "", "ref2RV", FormularioSBO);
                EditTextNoRef3RV = new EditTextSBO("txtRef3RV", true, "", "ref3RV", FormularioSBO);
                EditTextNoRef4RV = new EditTextSBO("txtRef4RV", true, "", "ref4RV", FormularioSBO);
                EditTextNoRef5RV = new EditTextSBO("txtRef5RV", true, "", "ref5RV", FormularioSBO);
                EditTextNoRef6RV = new EditTextSBO("txtRef6RV", true, "", "ref6RV", FormularioSBO);
                EditTextFechIngresoRV = new EditTextSBO("txtFecInRV", true, "", "fechaIRV", FormularioSBO);
                EditTextObservacionesRV = new EditTextSBO("txtObsRV", true, "", "obsRV", FormularioSBO);
                EditTextFechEventoDL = new EditTextSBO("txtFecEvDL", true, "", "fechaDL", FormularioSBO);
                EditTextNoRef1DL = new EditTextSBO("txtRef1DL", true, "", "ref1DL", FormularioSBO);
                EditTextNoRef2DL = new EditTextSBO("txtRef2DL", true, "", "ref2DL", FormularioSBO);
                EditTextObservacionesDL = new EditTextSBO("txtObsDL", true, "", "obsDL", FormularioSBO);
                EditTextFechEventoIns = new EditTextSBO("txtFecEvSI", true, "", "fechaSI", FormularioSBO);
                EditTextNoRef1Ins = new EditTextSBO("txtRef1SI", true, "", "ref1SI", FormularioSBO);
                EditTextNoRef2Ins = new EditTextSBO("txtRef2SI", true, "", "ref2SI", FormularioSBO);
                EditTextObservacionesIns = new EditTextSBO("txtObsSI", true, "", "obsSI", FormularioSBO);
                EditTextNoDocG = new EditTextSBO("txtNoDocG", true, "", "numDoc", FormularioSBO);
                EditTextFechDocG = new EditTextSBO("txtFchDocG", true, "", "fechaGas", FormularioSBO);
                EditTextMontoG = new EditTextSBO("txtMontoG", true, "", "montoGas", FormularioSBO);
                EditTextObservacionesG = new EditTextSBO("txtObsG", true, "", "obsGas", FormularioSBO);
                CheckBoxPrenda = new CheckBoxSBO("chConPDL", true, "", "prenda", FormularioSBO);
                
                EditTextEnteFinanciero.AsignaBinding();
                EditTextFechEventoRV.AsignaBinding();
                EditTextNoRef1RV.AsignaBinding();
                EditTextNoRef2RV.AsignaBinding();
                EditTextNoRef3RV.AsignaBinding();
                EditTextNoRef4RV.AsignaBinding();
                EditTextNoRef5RV.AsignaBinding();
                EditTextNoRef6RV.AsignaBinding();
                EditTextFechIngresoRV.AsignaBinding();
                EditTextObservacionesRV.AsignaBinding();
                EditTextFechEventoDL.AsignaBinding();
                EditTextNoRef1DL.AsignaBinding();
                EditTextNoRef2DL.AsignaBinding();
                EditTextObservacionesDL.AsignaBinding();
                EditTextFechEventoIns.AsignaBinding();
                EditTextNoRef1Ins.AsignaBinding();
                EditTextNoRef2Ins.AsignaBinding();
                EditTextObservacionesIns.AsignaBinding();
                EditTextNoDocG.AsignaBinding();
                EditTextFechDocG.AsignaBinding();
                EditTextMontoG.AsignaBinding();
                EditTextObservacionesG.AsignaBinding();
                CheckBoxPrenda.AsignaBinding();

                FolderRevVehicular = new FolderSBO("fldRevVehi");
                FolderDocLegales = new FolderSBO("fldRocLeg");
                FolderInscripcion = new FolderSBO("fldInscrip");
                FolderGastos = new FolderSBO("fldGastos");
                FolderReportes = new FolderSBO("fldReport");

                MatrixRevVehicular = new MatrixSBORevVehicular("mtx_RevVeh", FormularioSBO, "@SCGD_REV_VEH");
                MatrixRevVehicular.CreaColumnas();
                MatrixRevVehicular.LigaColumnas();

                MatrixDocLegales = new MatrixSBODocLegales("mtx_DocLeg", FormularioSBO, "@SCGD_DOC_LEG");
                MatrixDocLegales.CreaColumnas();
                MatrixDocLegales.LigaColumnas();

                MatrixInscripcion = new MatrixSBOInscripcion("mtx_SegIns", FormularioSBO, "@SCGD_INSCRIP");
                MatrixInscripcion.CreaColumnas();
                MatrixInscripcion.LigaColumnas();

                MatrixGastos = new MatrixSBOGastos("mtx_Gasto", FormularioSBO, "@SCGD_GAS_INS");
                MatrixGastos.CreaColumnas();
                MatrixGastos.LigaColumnas();

                ButtonUnidad = new ButtonSBO("btnUnidad",FormularioSBO);
                ButtonEnteFin = new ButtonSBO("btnEntFin",FormularioSBO);

                ButtomAgregarRV = new ButtonSBO("btnAgreRV",FormularioSBO);
                ButtomEditarRV = new ButtonSBO("btnEditRV", FormularioSBO);
                ButtomBorrarRV = new ButtonSBO("btnBorrRV", FormularioSBO);

                ButtomAgregarDL = new ButtonSBO("btnAgreDL", FormularioSBO);
                ButtomEditarDL = new ButtonSBO("btnEditDL", FormularioSBO);
                ButtomBorrarDL = new ButtonSBO("btnBorrDL", FormularioSBO);

                ButtomAgregarIns = new ButtonSBO("btnAgreSI", FormularioSBO);
                ButtomEditarIns = new ButtonSBO("btnEditSI", FormularioSBO);
                ButtomBorrarIns = new ButtonSBO("btnBorrSI", FormularioSBO);

                ButtomAgregarG = new ButtonSBO("btnAgreG", FormularioSBO);
                ButtomEditarG = new ButtonSBO("btnEditG", FormularioSBO);
                ButtomBorrarG = new ButtonSBO("btnBorrG",FormularioSBO);

                ButtonExpediente = new ButtonSBO("1", FormularioSBO);
                ButtonRefrescar = new ButtonSBO("btnRefres", FormularioSBO);

                ComboBoxGestionRV = new ComboBoxSBO("cmbTipGtRV", FormularioSBO, true, "", "gestRV");
                ComboBoxGestionDL = new ComboBoxSBO("cmbTipGtDL", FormularioSBO, true, "", "gestDL");
                ComboBoxGestionIns = new ComboBoxSBO("cmbTipGtSI", FormularioSBO, true, "", "gestSI");
                ComboBoxEventoRV = new ComboBoxSBO("cmbTipEvRV", FormularioSBO, true, "", "eveRV");
                ComboBoxEventoDL = new ComboBoxSBO("cmbTipEvDL", FormularioSBO, true, "", "eveDL");
                ComboBoxEventoIns = new ComboBoxSBO("cmbTipEvSI", FormularioSBO, true, "", "eveSI");
                ComboBoxGastoG = new ComboBoxSBO("cmbTipGasG",FormularioSBO,true,"","gasto");
                
                ComboBoxGestionRV.AsignaBinding();
                ComboBoxGestionDL.AsignaBinding();
                ComboBoxGestionIns.AsignaBinding();
                ComboBoxEventoRV.AsignaBinding();
                ComboBoxEventoDL.AsignaBinding();
                ComboBoxEventoIns.AsignaBinding();
                ComboBoxGastoG.AsignaBinding();

                EditTextCodigo.HabilitarBuscar();
                EditTextCodigoCliente.HabilitarBuscar();
                EditTextNombreCliente.HabilitarBuscar();
                EditTextUnidad.HabilitarBuscar();
                EditTextNoChasis.HabilitarBuscar();
                EditTextNoMotor.HabilitarBuscar();
                EditTextPlaca.HabilitarBuscar();
                EditTextPlacaAGV.HabilitarBuscar();
                EditTextTomo.HabilitarBuscar();
                EditTextAsiento.HabilitarBuscar();
                EditTextNoCV.HabilitarBuscar();
                EditTextNoFactura.HabilitarBuscar();

                EditTextObservacion.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);
                ComboBoxProblema.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False);

                ButtomAgregarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int) BoAutoFormMode.afm_All,BoModeVisualBehavior.mvb_True);
                ButtomEditarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomBorrarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomAgregarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                ButtomEditarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomBorrarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomAgregarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                ButtomEditarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomBorrarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                ButtomAgregarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                ButtomEditarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

                DataTableReportes = FormularioSBO.DataSources.DataTables.Add("Reportes");
                DataTableReportes.Columns.Add("codeR", BoFieldsType.ft_AlphaNumeric, 20);
                DataTableReportes.Columns.Add("nameR", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableReportes.Columns.Add("descripR", BoFieldsType.ft_AlphaNumeric, 100);

                MatrixReportes = new MatrixSBOReportes("mtx_Report", FormularioSBO, "Reportes");
                MatrixReportes.CreaColumnas();
                MatrixReportes.LigaColumnas();

                ButtonImprimirRep = new ButtonSBO("btnImpRep", FormularioSBO);

                ultimoEventoAgregado = "";
                penultimoEventoAgregado = "";

                FormularioSBO.Freeze(false);

            }
        }

        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {

                FormularioSBO.Freeze(true);

                foreach (Item oItem in FormularioSBO.Items)
                {
                    if (oItem.UniqueID == "fldRevVehi" || oItem.UniqueID == "fldRocLeg" || oItem.UniqueID == "fldInscrip" || oItem.UniqueID == "fldGastos" || oItem.UniqueID == "fldReport")
                    {
                        oItem.AffectsFormMode = false;
                    }
                }

                CargarFormulario();

                FormularioSBO.Freeze(false);

            }

        }

        private void CargarFormulario()
        {
            FormularioSBO.Freeze(true);

            Item sboItem;
            ComboBox sboCombo;

            FormType = FormularioSBO.TypeEx;
            
            PermisosPlacas();

            sboItem = FormularioSBO.Items.Item("cmbProblem");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_PROBLEM_PLC]", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbTipGtRV");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GESTION] where U_Seguimiento = '1'", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbTipGtDL");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GESTION] where U_Seguimiento = '2'", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbTipGtSI");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GESTION] where U_Seguimiento = '3'", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbTipGasG");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GASTOS]", Conexion);

            
            if (FormularioSBO.Items.Item("fldRevVehi").Enabled)
            {
                FormularioSBO.Items.Item("fldRevVehi").Click();
            }

            else if (FormularioSBO.Items.Item("fldRocLeg").Enabled)
            {
                FormularioSBO.Items.Item("fldRocLeg").Click();
            }

            else if (FormularioSBO.Items.Item("fldInscrip").Enabled)
            {
                FormularioSBO.Items.Item("fldInscrip").Click();
            }

            else if(FormularioSBO.Items.Item("fldGastos").Enabled)
            {
                FormularioSBO.Items.Item("fldGastos").Click();
            }

            else
            {
                FormularioSBO.Items.Item("1").Enabled = false;
                FormularioSBO.Items.Item("fldReport").Click();
            }

            CargarReportes();

            FormularioSBO.Freeze(false);
        }

        public virtual void ApplicationSBOOnFormDataEvent(ref BusinessObjectInfo businessObjectInfo,
                                                            ref bool bubbleEvent)
        {
            if (businessObjectInfo.FormTypeEx != FormType) return;
            if (businessObjectInfo.BeforeAction == false && businessObjectInfo.ActionSuccess)
            {
                switch (businessObjectInfo.EventType)
                {
                    case BoEventTypes.et_FORM_DATA_LOAD:
                        DataLoadEvent(businessObjectInfo, ref bubbleEvent);
                        break;
                }
            }
        }

        public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent)
        {

            if (pVal.FormTypeEx != FormType) return;

            if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {

                if (pVal.ItemUID == EditTextCodigoCliente.UniqueId)
                {
                    if (FormularioSBO.Mode != BoFormMode.fm_FIND_MODE)
                    {
                        CFLCliente(formUid, pVal);
                    }
                }

                else if (pVal.ItemUID == ButtonUnidad.UniqueId)
                {
                    LimpiarExpPlacas();
                    CFLUnidad(formUid, pVal);
                }

                else if (pVal.ItemUID == ButtonEnteFin.UniqueId)
                {
                    CFLEnteFinanciero(formUid, pVal);
                }

            }

            else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
            {

                if (pVal.ItemUID == FolderRevVehicular.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 1;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderDocLegales.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 2;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderInscripcion.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 3;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderGastos.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 4;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderReportes.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 5;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID==ButtomAgregarRV.UniqueId)
                {

                    ButtonSBOAgregarRVItemPressed(formUid, pVal, ref bubbleEvent);

                }

                else if (pVal.ItemUID == ButtomAgregarDL.UniqueId)
                {

                    ButtonSBOAgregarDLItemPressed(formUid, pVal, ref bubbleEvent);

                }

                else if (pVal.ItemUID == ButtomAgregarIns.UniqueId)
                {

                    ButtonSBOAgregarSIItemPressed(formUid, pVal, ref bubbleEvent);

                }

                else if (pVal.ItemUID == ButtomAgregarG.UniqueId)
                {

                    ButtonSBOAgregarGastosItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtomAgregarG.UniqueId)
                {

                    ButtonSBOAgregarGastosItemPressed(formUid, pVal, ref bubbleEvent);

                }

                else if (pVal.ItemUID == ButtomEditarRV.UniqueId)
                {
                    ButtonSBOEditarRVItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomEditarDL.UniqueId)
                {
                    ButtonSBOEditarDLItemPressed(formUid,pVal,ref bubbleEvent);

                    ButtomAgregarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomEditarIns.UniqueId)
                {
                    ButtonSBOEditarInsItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomEditarG.UniqueId)
                {
                    ButtonSBOEditarGItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomBorrarRV.UniqueId)
                {
                    ButtonSBOBorrarRVItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomBorrarDL.UniqueId)
                {
                    ButtonSBOBorrarDLItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomBorrarIns.UniqueId)
                {
                    ButtonSBOBorrarInsItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }

                else if (pVal.ItemUID == ButtomBorrarG.UniqueId)
                {
                    ButtonSBOBorrarGItemPressed(formUid, pVal, ref bubbleEvent);

                    ButtomAgregarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    ButtomBorrarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                }
                
                else if (pVal.ItemUID == ButtonImprimirRep.UniqueId)
                {
                    ButtonSBOImprimirRepItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtonExpediente.UniqueId)
                {
                    ButtonSBOCrearExpedienteItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtonRefrescar.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOActualizarExpedienteItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

            }

            else if (pVal.EventType == BoEventTypes.et_COMBO_SELECT)
            {
                if (pVal.BeforeAction == false && pVal.ActionSuccess)
                {
                    if (pVal.ItemUID == ComboBoxGestionRV.UniqueId || pVal.ItemUID == ComboBoxGestionDL.UniqueId || pVal.ItemUID == ComboBoxGestionIns.UniqueId)
                    {
                        ComboBoxGestionSelected(pVal);
                    }

                    PermisosPlacas();
                }
            }

            else if (pVal.EventType == BoEventTypes.et_CLICK)
            {
                if (pVal.ItemUID ==  MatrixRevVehicular.UniqueId)
                {
                    CargarInformaciondesdeMatrix(pVal, MatrixRevVehicular, "@SCGD_REV_VEH", "cmbTipEvRV", "U_Cod_Ges",
                                                 "U_Cod_Eve", "U_Fech_EV", "U_Num_Ref1", "U_Num_Ref2", "U_Observ");

                    ButtomAgregarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomBorrarRV.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE || FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        ButtonUnidad.ItemSBO.Enabled = false;
                    }
                }

                else if (pVal.ItemUID == MatrixDocLegales.UniqueId)
                {
                    CargarInformaciondesdeMatrix(pVal, MatrixDocLegales, "@SCGD_DOC_LEG", "cmbTipEvDL", "U_Cod_Ges",
                                                 "U_Cod_Eve", "U_Fech_EV", "U_Num_Ref1", "U_Num_Ref2", "U_Observ");

                    ButtomAgregarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomBorrarDL.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE || FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        ButtonUnidad.ItemSBO.Enabled = false;
                    }
                }

                else if (pVal.ItemUID == MatrixInscripcion.UniqueId)
                {
                    CargarInformaciondesdeMatrix(pVal, MatrixInscripcion, "@SCGD_INSCRIP", "cmbTipEvSI", "U_Cod_Ges",
                                                 "U_Cod_Eve", "U_Fech_EV", "U_Num_Ref1", "U_Num_Ref2", "U_Observ");

                    ButtomAgregarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomBorrarIns.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE || FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        ButtonUnidad.ItemSBO.Enabled = false;
                    }
                }

                else if (pVal.ItemUID == MatrixGastos.UniqueId)
                {
                    CargarInformaciondesdeMatrixGasto(pVal, MatrixGastos, "@SCGD_GAS_INS", "cmbTipGasG","U_Cod_Gas","U_Num_Doc","U_Fech_Doc","U_Monto","U_Observ");

                    ButtomAgregarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomEditarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);
                    ButtomBorrarG.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True);

                    if (FormularioSBO.Mode == BoFormMode.fm_OK_MODE || FormularioSBO.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        ButtonUnidad.ItemSBO.Enabled = false;
                    }
                }

                else if (pVal.ItemUID == FolderRevVehicular.UniqueId)
                {
                    if(FormularioSBO.Items.Item("fldRevVehi").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }

                else if (pVal.ItemUID == FolderDocLegales.UniqueId)
                {
                    if (FormularioSBO.Items.Item("fldRocLeg").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }

                else if (pVal.ItemUID == FolderInscripcion.UniqueId)
                {
                    if (FormularioSBO.Items.Item("fldInscrip").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }

                else if (pVal.ItemUID == FolderGastos.UniqueId)
                {
                    if (FormularioSBO.Items.Item("fldGastos").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }
            }

            else if (pVal.FormMode == (decimal)BoFormMode.fm_OK_MODE)
            {
                ButtonUnidad.ItemSBO.Enabled = false;
                
            }

            else if (pVal.FormMode == (decimal)BoFormMode.fm_UPDATE_MODE)
            {
                ButtonUnidad.ItemSBO.Enabled = false;
            }

            else if (pVal.FormMode == (decimal)BoFormMode.fm_FIND_MODE)
            {
                if (pVal.BeforeAction && pVal.ActionSuccess == false)
                {
                    FormularioSBO.Items.Item("lkUnidad").Enabled = true;
                    FormularioSBO.Items.Item("lkUnidad").Visible = true;
                    FormularioSBO.Items.Item("btnRefres").Enabled = false;
                }
            }
        }

        public virtual void ApplicationSBOOnDataEvent(ref BusinessObjectInfo businessObjectInfo)
        {
            if (businessObjectInfo.FormTypeEx != FormType) return;

            if (businessObjectInfo.BeforeAction == false && businessObjectInfo.ActionSuccess)
            {
                if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    PermisosPlacas();
                    
                }
            }
        }

        #endregion

        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        #endregion

        #region Others Members

        public string Conexion { get; set; }
        public string DireccionReportes { get; set; }
        public string UsuarioBD { get; set; }
        public string ContraseñaBD { get; set; }

        #endregion

    }
}