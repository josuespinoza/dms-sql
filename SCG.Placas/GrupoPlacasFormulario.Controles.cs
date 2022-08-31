using System;
using System.Globalization;
using System.Threading;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using SCG.SBOFramework.UI.Extensions;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Placas
{
    public partial class GrupoPlacasFormulario : IFormularioSBO, IUsaMenu 
    {
        public GrupoPlacasFormulario(Application applicationSBO, ICompany companySBO)
        {
            ApplicationSBO = applicationSBO;
            CompanySBO = companySBO;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = My.Resources.Resource.Culture;
            DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            My.Resources.Resource.Culture = cultureInfo;
        }

        public FolderSBO FolderSeleccion;
        public FolderSBO FolderEventos;
        public FolderSBO FolderGastos;

        public EditTextSBO EditTextUnidad;
        public EditTextSBO EditTextNumChasis;
        public EditTextSBO EditTextNumMotor;
        public EditTextSBO EditTextAnno;
        public EditTextSBO EditTextTotal;

        public ComboBoxSBO ComboBoxMarca;
        public ComboBoxSBO ComboBoxEstilo;
        public ComboBoxSBO ComboBoxModelo;
        public ComboBoxSBO ComboBoxColor;
        public ComboBoxSBO ComboBoxEstado;
        public ComboBoxSBO ComboBoxCondicion;
        public ComboBoxSBO ComboBoxUbicacion;

        public ButtonSBO ButtonUnidad;
        public ButtonSBO ButtonBuscarGrupo;
        public ButtonSBO ButtonLimpiarGrupo;
        public ButtonSBO ButtonGrupoPlacas;
        public ButtonSBO ButtonCancelar;
        public ButtonSBO ButtonLimpiar;
        public ButtonSBO ButtonBuscarCargaBG;
        
        public MatrixSBOSeleccionGrupo MatrixSeleccionGrupo;
        public MatrixSBOEventosGrupo MatrixEventosGrupo;

        public ButtonSBO ButtonAgregarGrupoE;

        public ComboBoxSBO ComboBoxGestionE;
        public ComboBoxSBO ComboBoxEventoE;
        
        public EditTextSBO EditTextFechaEventoE;
        public EditTextSBO EditTextNoGrupoE;
        public EditTextSBO EditTextFechaGrupoE;
        public EditTextSBO EditTextDescGrupoE;

        public ButtonSBO ButtonCopiarFechaE;
        public ButtonSBO ButtonCargarEnBaseGrupoE;
        public ButtonSBO ButtonBorrarE;
        public ButtonSBO ButtonAplicarEventoE;

        public ComboBoxSBO ComboBoxGastoG;

        public EditTextSBO EditTextFechaDocumentoG;
        public EditTextSBO EditTextMontoG;
        public EditTextSBO EditTextNoGrupoG;
        public EditTextSBO EditTextFechaGrupoG;
        public EditTextSBO EditTextDescGrupoG;

        public ButtonSBO ButtonCopiarFechaG;
        public ButtonSBO ButtonCopiarMontoG;
        public ButtonSBO ButtonCopiarG;
        public ButtonSBO ButtonBorrarG;
        public ButtonSBO ButtonAplicarGastoG;
        public ButtonSBO ButtonBuscarGrupoG;
        public ButtonSBO ButtonCargarEnBaseGG;
        public ButtonSBO ButtonCalcularMG;


        public MatrixSBOGastosGrupo MatrixGastosGrupo;

        //Variable global que mantiene el index a ingresar en el Datable de Selección
        public int IndexDataTableE;

        //Variable global que mantiene el index a ingresar en el Datable de Gastos
        public int IndexDataTableG;
        
        #region IFormularioSBO Members

        public string FormType { get; set; }

        public string NombreXml { get; set; }

        public string Titulo { get; set; }

        public IForm FormularioSBO { get; set; }

        public bool Inicializado { get; set; }

        public ICompany CompanySBO { get; private set; }

        public IApplication ApplicationSBO { get; private set; }

        private DataTable DataTableSeleccion;

        private DataTable DataTableEventos;

        private DataTable DataTableGastos;

        private DataTable DataTableConsulta;

        private DataTable DataTableCargarEnBase;

        public CargaFormularioRevisionVDelegate CargaFormulario { get; set; }
        
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

        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {
                FormularioSBO.Freeze(true);

                FolderSeleccion = new FolderSBO("fldSelUnid");
                FolderEventos = new FolderSBO("fldEveGrup");
                FolderGastos = new FolderSBO("fldGasGrup");

                ButtonUnidad = new ButtonSBO("btnUnidad", FormularioSBO);
                ButtonBuscarGrupo = new ButtonSBO("btnBuscar", FormularioSBO);
                ButtonLimpiarGrupo = new ButtonSBO("btnLimpG", FormularioSBO);
                ButtonGrupoPlacas = new ButtonSBO("1", FormularioSBO);
                ButtonLimpiar = new ButtonSBO("btnLimpiar", FormularioSBO);
                ButtonCancelar = new ButtonSBO("2", FormularioSBO);
                ButtonAgregarGrupoE = new ButtonSBO("btnAgrGrup", FormularioSBO);
                ButtonCopiarFechaE = new ButtonSBO("btnCopFech", FormularioSBO);
                ButtonCargarEnBaseGrupoE = new ButtonSBO("btnCargG", FormularioSBO);
                ButtonBorrarE = new ButtonSBO("btnBorrEG", FormularioSBO);
                ButtonAplicarEventoE = new ButtonSBO("btnAplEG", FormularioSBO);
                ButtonCopiarFechaG = new ButtonSBO("btnCopFeG", FormularioSBO);
                ButtonCopiarMontoG = new ButtonSBO("btnCopMG", FormularioSBO);
                ButtonCopiarG = new ButtonSBO("btnCopG", FormularioSBO);
                ButtonBorrarG = new ButtonSBO("btnBorrG", FormularioSBO);
                ButtonAplicarGastoG = new ButtonSBO("btnApliG", FormularioSBO);
                ButtonBuscarCargaBG = new ButtonSBO("btnCargaBG", FormularioSBO);
                ButtonCalcularMG = new ButtonSBO("btnCalcMG", FormularioSBO);
                ButtonCargarEnBaseGG = new ButtonSBO("btnCargGG", FormularioSBO);
                ButtonBuscarGrupoG = new ButtonSBO("btnCargBGG", FormularioSBO);

                UserDataSources userDataSource = FormularioSBO.DataSources.UserDataSources;
                userDataSource.Add("unidad", BoDataType.dt_SHORT_TEXT, 100);
                userDataSource.Add("chasis", BoDataType.dt_SHORT_TEXT, 100);
                userDataSource.Add("motor", BoDataType.dt_SHORT_TEXT, 100);
                userDataSource.Add("marca", BoDataType.dt_LONG_TEXT, 100);
                userDataSource.Add("estilo", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("modelo", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("color", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("anno", BoDataType.dt_SHORT_NUMBER, 4);
                userDataSource.Add("estado", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("condicion", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("ubicacion", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("gestionE", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("eventoE", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("fechaE", BoDataType.dt_DATE, 100);
                userDataSource.Add("numGrupo", BoDataType.dt_LONG_TEXT, 9);
                userDataSource.Add("fechaGrupo", BoDataType.dt_SHORT_TEXT, 100);
                userDataSource.Add("descGrupo", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("totaGrupo", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("gastoG", BoDataType.dt_LONG_TEXT, 200);
                userDataSource.Add("fechaDocG", BoDataType.dt_DATE, 100);
                userDataSource.Add("monto", BoDataType.dt_PRICE, 100);
                userDataSource.Add("numGrupoG", BoDataType.dt_LONG_TEXT, 9);
                userDataSource.Add("fechGrupoG", BoDataType.dt_SHORT_TEXT, 100);
                userDataSource.Add("descGrupoG", BoDataType.dt_LONG_TEXT, 200);

                EditTextNoGrupoE = new EditTextSBO("txtNumGrup", true, "", "numGrupo", FormularioSBO);
                EditTextFechaGrupoE = new EditTextSBO("txtFechGru", true, "", "fechaGrupo", FormularioSBO);
                EditTextDescGrupoE = new EditTextSBO("txtDescG", true, "", "descGrupo", FormularioSBO);
                EditTextTotal = new EditTextSBO("txtTotalG", true, "", "totaGrupo", FormularioSBO);

                EditTextNoGrupoE.AsignaBinding();
                EditTextFechaGrupoE.AsignaBinding();
                EditTextDescGrupoE.AsignaBinding();
                EditTextTotal.AsignaBinding();

                EditTextUnidad = new EditTextSBO("txtUnidad", true, "", "unidad", FormularioSBO);
                EditTextNumChasis = new EditTextSBO("txtChasis", true, "", "chasis", FormularioSBO);
                EditTextNumMotor = new EditTextSBO("txtNoMotor", true, "", "motor", FormularioSBO);
                EditTextAnno = new EditTextSBO("txtAnno", true, "", "anno", FormularioSBO);
                EditTextFechaEventoE = new EditTextSBO("txtFechEv", true, "", "fechaE", FormularioSBO);
                EditTextFechaDocumentoG = new EditTextSBO("txtFchDocG", true, "", "fechaDocG", FormularioSBO);
                EditTextMontoG = new EditTextSBO("txtMontoG", true, "", "monto", FormularioSBO);

                EditTextNoGrupoG = new EditTextSBO("txtNumGruG", true, "", "numGrupoG", FormularioSBO);
                EditTextFechaGrupoG = new EditTextSBO("txtFchGruG", true, "", "fechGrupoG", FormularioSBO);
                EditTextDescGrupoG = new EditTextSBO("txtDescGG", true, "", "descGrupoG", FormularioSBO);

                EditTextUnidad.AsignaBinding();
                EditTextNumChasis.AsignaBinding();
                EditTextNumChasis.AsignaBinding();
                EditTextNumMotor.AsignaBinding();
                EditTextAnno.AsignaBinding();
                EditTextFechaEventoE.AsignaBinding();
                EditTextFechaDocumentoG.AsignaBinding();
                EditTextMontoG.AsignaBinding();
                EditTextNoGrupoG.AsignaBinding();
                EditTextFechaGrupoG.AsignaBinding();
                EditTextDescGrupoG.AsignaBinding();

                ComboBoxMarca = new ComboBoxSBO("cmbMarca", FormularioSBO, true, "", "marca");
                ComboBoxEstilo = new ComboBoxSBO("cmbEstilo", FormularioSBO, true, "", "estilo");
                ComboBoxModelo = new ComboBoxSBO("cmbModelo", FormularioSBO, true, "", "modelo");
                ComboBoxColor = new ComboBoxSBO("cmbColor", FormularioSBO, true, "", "color");

                ComboBoxEstado = new ComboBoxSBO("cmbEstado", FormularioSBO, true, "", "estado");
                ComboBoxCondicion = new ComboBoxSBO("cmbCondi", FormularioSBO, true, "", "condicion");
                ComboBoxUbicacion = new ComboBoxSBO("cmbUbica", FormularioSBO, true, "", "ubicacion");
                ComboBoxGestionE = new ComboBoxSBO("cmbTipGest", FormularioSBO, true, "", "gestionE");
                ComboBoxEventoE = new ComboBoxSBO("cmbTipEven", FormularioSBO, true, "", "eventoE");
                ComboBoxGastoG = new ComboBoxSBO("cmbTipoG", FormularioSBO, true, "", "gastoG");

                ComboBoxMarca.AsignaBinding();
                ComboBoxEstilo.AsignaBinding();
                ComboBoxModelo.AsignaBinding();
                ComboBoxColor.AsignaBinding();

                ComboBoxEstado.AsignaBinding();
                ComboBoxCondicion.AsignaBinding();
                ComboBoxUbicacion.AsignaBinding();
                ComboBoxGestionE.AsignaBinding();
                ComboBoxEventoE.AsignaBinding();
                ComboBoxGastoG.AsignaBinding();

                DataTableSeleccion = FormularioSBO.DataSources.DataTables.Add("SeleccionU");
                DataTableSeleccion.Columns.Add("seleccionS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("numChasisS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("numMotorS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("marcaS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("estiloS", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableSeleccion.Columns.Add("modeloS", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableSeleccion.Columns.Add("colorS", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableSeleccion.Columns.Add("annoS", BoFieldsType.ft_AlphaNumeric, 4);
                DataTableSeleccion.Columns.Add("unidadS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("contVentaS", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableSeleccion.Columns.Add("numFactS", BoFieldsType.ft_AlphaNumeric, 100);

                DataTableEventos = FormularioSBO.DataSources.DataTables.Add("EventosGrupo");
                DataTableEventos.Columns.Add("numChasisE", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("numMotorE", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("estiloE", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableEventos.Columns.Add("fechaEventoE", BoFieldsType.ft_Date, 100);
                DataTableEventos.Columns.Add("noRef1E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("noRef2E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("noRef3E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("noRef4E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("noRef5E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("noRef6E", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("fechaIngresoE", BoFieldsType.ft_Date, 100);
                DataTableEventos.Columns.Add("prendaE", BoFieldsType.ft_AlphaNumeric, 4);
                DataTableEventos.Columns.Add("instFinanE", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableEventos.Columns.Add("observE", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableEventos.Columns.Add("marcaE", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("modeloE", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableEventos.Columns.Add("colorE", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableEventos.Columns.Add("annoE", BoFieldsType.ft_AlphaNumeric, 4);
                DataTableEventos.Columns.Add("unidadE", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("contVentaE", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableEventos.Columns.Add("numFactE", BoFieldsType.ft_AlphaNumeric, 100);

                DataTableGastos = FormularioSBO.DataSources.DataTables.Add("GastosGrupo");
                DataTableGastos.Columns.Add("tipoGastoG", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableGastos.Columns.Add("numChasisG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("numMotorG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("estiloG", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableGastos.Columns.Add("numDocumG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("fechaDocumG", BoFieldsType.ft_Date, 100);
                DataTableGastos.Columns.Add("montoG", BoFieldsType.ft_Float, 100);
                DataTableGastos.Columns.Add("observG", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableGastos.Columns.Add("CodGastG", BoFieldsType.ft_AlphaNumeric, 4);
                DataTableGastos.Columns.Add("marcaG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("modeloG", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableGastos.Columns.Add("colorG", BoFieldsType.ft_AlphaNumeric, 200);
                DataTableGastos.Columns.Add("annoG", BoFieldsType.ft_AlphaNumeric, 4);
                DataTableGastos.Columns.Add("unidadG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("contVentaG", BoFieldsType.ft_AlphaNumeric, 100);
                DataTableGastos.Columns.Add("numFactG", BoFieldsType.ft_AlphaNumeric, 100);

                DataTableConsulta = FormularioSBO.DataSources.DataTables.Add("Consulta");

                DataTableCargarEnBase = FormularioSBO.DataSources.DataTables.Add("CargarGrupo");

                MatrixSeleccionGrupo = new MatrixSBOSeleccionGrupo("mtxSelUnid", FormularioSBO, "SeleccionU");
                MatrixSeleccionGrupo.CreaColumnas();
                MatrixSeleccionGrupo.LigaColumnas();

                MatrixEventosGrupo = new MatrixSBOEventosGrupo("mtxEventG", FormularioSBO, "EventosGrupo");
                MatrixEventosGrupo.CreaColumnas();
                MatrixEventosGrupo.LigaColumnas();

                MatrixGastosGrupo = new MatrixSBOGastosGrupo("mtxGastG", FormularioSBO, "GastosGrupo");
                MatrixGastosGrupo.CreaColumnas();
                MatrixGastosGrupo.LigaColumnas();

                IndexDataTableE = 0;
                IndexDataTableG = 0;

                FormularioSBO.Freeze(false);
            }
        }

        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {
                foreach (Item oItem in FormularioSBO.Items)
                {
                    oItem.AffectsFormMode = false;
                }

                foreach (Column oColumn in MatrixSeleccionGrupo.Matrix.Columns)
                {
                    oColumn.AffectsFormMode = false;
                }

                CargarFormulario();
            }
        }

        private void CargarFormulario()
        {
            FormularioSBO.Freeze(true);

            Item sboItem;
            ComboBox sboComboBox;

            FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
            
            FormType = FormularioSBO.TypeEx;
            FormularioSBO.PaneLevel = 1;
            FormularioSBO.Items.Item("fldSelUnid").Click();

            sboItem = FormularioSBO.Items.Item("cmbMarca");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_MARCA]", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbEstilo");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_ESTILO]", Conexion);

            sboItem = FormularioSBO.Items.Item("cmbModelo");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_MODELO]", Conexion);
            
            sboItem = FormularioSBO.Items.Item("cmbColor");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_COLOR]", Conexion);
           
            sboItem = FormularioSBO.Items.Item("cmbEstado");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_ESTADO]", Conexion);
            
            sboItem = FormularioSBO.Items.Item("cmbCondi");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_DISPONIBILIDAD]", Conexion);
            
            sboItem = FormularioSBO.Items.Item("cmbUbica");
            sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, Name from [@SCGD_UBICACIONES]", Conexion);

            //Carga el ComboBox de Gestión en base a la configuración de seguridad establecida por usuario

            string[] seguridadE = General.ObtenerSeguridadEventos(ApplicationSBO, Conexion);
            int tamanoArrayE = seguridadE.Length;
            string revisionV = "";
            string documentosL = "";
            string inscripcion = "";

            if (tamanoArrayE > 0)
            {
                for (int i = 0; i <= tamanoArrayE - 1; i++)
                {
                    if(seguridadE[i].Equals("1"))
                    {
                        revisionV = seguridadE[i];
                    }

                    else if(seguridadE[i].Equals("2"))
                    {
                        documentosL = seguridadE[i];
                    }

                    else if(seguridadE[i].Equals("3"))
                    {
                        inscripcion = seguridadE[i];
                    }
                }

                sboItem = FormularioSBO.Items.Item("cmbTipGest");
                sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
                General.CargarValidValuesEnCombos(sboComboBox.ValidValues,
                                                  string.Format(
                                                      "Select Code,U_Descrip from [@SCGD_GESTION] where U_Seguimiento = '{0}' OR U_Seguimiento = '{1}' OR U_Seguimiento = '{2}'",
                                                      revisionV, documentosL, inscripcion), Conexion);
            }

            else if (tamanoArrayE == 0)
            {
                FormularioSBO.Items.Item("fldEveGrup").Enabled = false;
               

            }

            //Carga el ComboBox de Gastos en base a la configuración de seguridad establecida por usuario

            string seguridadG = General.ObtenerSeguridadGastos(ApplicationSBO, Conexion);

            if(!seguridadG.Equals("0"))
            {
                sboItem = FormularioSBO.Items.Item("cmbTipoG");
                sboComboBox = (SAPbouiCOM.ComboBox)sboItem.Specific;
                General.CargarValidValuesEnCombos(sboComboBox.ValidValues, "Select Code, U_Descrip from [@SCGD_GASTOS]", Conexion);
            }

            else if (seguridadG.Equals("0"))
            {
                FormularioSBO.Items.Item("fldGasGrup").Enabled = false;
            }


            string anno = DateTime.Today.Year.ToString();
            EditTextAnno.AsignaValorUserDataSource(anno);

            FormularioSBO.Freeze(false);
        }

        public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.FormTypeEx != FormType) return;

            if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                if (pVal.ItemUID == ButtonUnidad.UniqueId)
                {
                    LimpiarBusquedaGrupos();
                    CFLUnidad(formUid, pVal);
                }

                else if (pVal.ItemUID == ButtonBuscarCargaBG.UniqueId)
                {
                    LimpiarBusquedaCargaBG();
                    CFLCargaGrupo(formUid, pVal);
                }

                else if (pVal.ItemUID == ButtonBuscarGrupoG.UniqueId)
                {
                    LimpiarBusquedaCargaBGGastos();
                    CFLCargaGrupoGasto(formUid, pVal);
                }
            }

            else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED )
            {
                if (pVal.ItemUID == FolderSeleccion.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 1;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderEventos.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 2;
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == FolderGastos.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.PaneLevel = 3;
                    FormularioSBO.Freeze(false);
                }
                
                else if (pVal.ItemUID == ButtonLimpiar.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOLimpiarItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == ButtonBuscarGrupo.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOBuscarItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == ButtonLimpiarGrupo.UniqueId)
                {
                    LimpiarBusquedaGrupos();
                }

                else if (pVal.ItemUID == ButtonAgregarGrupoE.UniqueId)
                {
                    ButtonSBOAgregarGItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtonCopiarFechaE.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCopiarFechaItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == ButtonBorrarE.UniqueId)
                {
                    ButtonSBOBorrarEventoItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtonAplicarEventoE.UniqueId)
                {
                    ButtonSBOApplicarEventosItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if (pVal.ItemUID == ButtonCargarEnBaseGrupoE.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCargarEnBaseItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if(pVal.ItemUID == ButtonCopiarFechaG.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCopiarFechaGastoItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if(pVal.ItemUID == ButtonCopiarMontoG.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCopiarMontoGastoItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if(pVal.ItemUID == ButtonCopiarG.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCopiarGastoItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if(pVal.ItemUID == ButtonBorrarG.UniqueId)
                {
                    ButtonSBOBorrarGastoItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if(pVal.ItemUID == ButtonAplicarGastoG.UniqueId)
                {
                    ButtonSBOApplicarGastosItemPressed(formUid, pVal, ref bubbleEvent);
                }

                else if(pVal.ItemUID == ButtonCargarEnBaseGG.UniqueId)
                {
                    FormularioSBO.Freeze(true);
                    ButtonSBOCargarEnBaseGastoItemPressed(formUid, pVal, ref bubbleEvent);
                    FormularioSBO.Freeze(false);
                }

                else if (pVal.ItemUID == ButtonCalcularMG.UniqueId)
                {
                    ButtonSBOCalcularGastosItemPressed(formUid, pVal, ref bubbleEvent);
                }
            }

            else if(pVal.EventType == BoEventTypes.et_CLICK)
            {
                if (pVal.ItemUID == FolderEventos.UniqueId)
                {
                    if (FormularioSBO.Items.Item("fldEveGrup").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }

                else if (pVal.ItemUID == FolderGastos.UniqueId)
                {
                    if (FormularioSBO.Items.Item("fldGasGrup").Enabled == false)
                    {
                        bubbleEvent = false;
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorPermisos, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }

            }

            else if(pVal.EventType == BoEventTypes.et_COMBO_SELECT)
            {
                if(pVal.ItemUID == ComboBoxGestionE.UniqueId)
                {
                    ComboBoxGestionSelected(pVal);
                    FormularioSBO.Freeze(true);
                    VisualizarCamposMatrix(pVal, MatrixEventosGrupo);
                    FormularioSBO.Freeze(false);
                }

                else if(pVal.ItemUID == ComboBoxMarca.UniqueId)
                {
                    ComboBoxMarcaSelected(pVal);
                }

                else if(pVal.ItemUID == ComboBoxEstilo.UniqueId)
                {
                    ComboBoxEstiloSelected(pVal);
                }
            }
        }
    }
}
