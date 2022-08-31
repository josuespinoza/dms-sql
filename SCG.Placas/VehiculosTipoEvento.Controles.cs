using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Placas
{
    public partial class VehiculosTipoEvento : IFormularioSBO, IUsaMenu
    {
        public VehiculosTipoEvento(Application applicationSBO, ICompany companySBO)
        {
            ApplicationSBO = applicationSBO;
            CompanySBO = companySBO;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = My.Resources.Resource.Culture;
            DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            My.Resources.Resource.Culture = cultureInfo;
        }

        public EditTextSBO EditTextFechInicio;
        public EditTextSBO EditTextFechaFin;
        public EditTextSBO EditTextNumGrupo;

        public ComboBoxSBO ComboBoxGestion;
        public ComboBoxSBO ComboBoxEvento;

        public ButtonSBO ButtonBuscar;
        public ButtonSBO ButtonImprimir;

        public CheckBoxSBO CheckBoxTipo;


        #region IFormularioSBO Members

        public string FormType { get; set; }

        public string NombreXml { get; set; }

        public string Titulo { get; set; }

        public IForm FormularioSBO { get; set; }

        public bool Inicializado { get; set; }

        public ICompany CompanySBO { get; private set; }

        public IApplication ApplicationSBO { get; private set; }
        
        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {
                FormularioSBO.Freeze(true);
                
                UserDataSources userDataSources = FormularioSBO.DataSources.UserDataSources;
                userDataSources.Add("fchInicio", BoDataType.dt_DATE, 100);
                userDataSources.Add("fchFin", BoDataType.dt_DATE, 100);
                userDataSources.Add("tipGestion", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("tipEvento", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("numGrupo", BoDataType.dt_LONG_TEXT, 200);
                userDataSources.Add("tipReporte", BoDataType.dt_SHORT_TEXT, 1);

                EditTextFechInicio = new EditTextSBO("txtFchIni", true, "", "fchInicio", FormularioSBO);
                EditTextFechaFin = new EditTextSBO("txtFchFin", true, "", "fchFin", FormularioSBO);
                EditTextNumGrupo = new EditTextSBO("txtNumGrup", true, "", "numGrupo", FormularioSBO);

                ComboBoxGestion = new ComboBoxSBO("cmbTipGest", FormularioSBO, true, "", "tipGestion");
                ComboBoxEvento = new ComboBoxSBO("cmbTipEven", FormularioSBO, true, "", "tipEvento");

                CheckBoxTipo = new CheckBoxSBO("chkTipo", true, "", "tipReporte",FormularioSBO);

                ButtonBuscar = new ButtonSBO("btnBuscar", FormularioSBO);
                ButtonImprimir = new ButtonSBO("btnImpr", FormularioSBO);

                EditTextFechInicio.AsignaBinding();
                EditTextFechaFin.AsignaBinding();
                EditTextNumGrupo.AsignaBinding();

                ComboBoxGestion.AsignaBinding();
                ComboBoxEvento.AsignaBinding();

                CheckBoxTipo.AsignaBinding();

                FormularioSBO.Freeze(false);
            }
        }

        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {
                CargarFormulario();
            }
            
        }

        private void CargarFormulario()
        {
            FormularioSBO.Freeze(true);

            Item sboItem;
            ComboBox sboCombo;

            sboItem = FormularioSBO.Items.Item("cmbTipGest");
            sboCombo = (SAPbouiCOM.ComboBox)sboItem.Specific;
            General.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select Code, U_Descrip from [@SCGD_GESTION]", Conexion);

            FormularioSBO.Freeze(false);
        }

        public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.FormTypeEx != FormType) return;

            if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                if (pVal.ItemUID == ButtonBuscar.UniqueId)
                {
                    CFLCargaGrupo(formUid, pVal);
                }
            }

            else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED )
            {
                if (pVal.ItemUID == ButtonImprimir.UniqueId)
                {
                    if (pVal.ItemUID == ButtonImprimir.UniqueId)
                    {
                        ButtonSBOImprimirReporteItemPressed(formUid, pVal, ref bubbleEvent);
                        
                    }
                }
            }

            else if (pVal.EventType == BoEventTypes.et_COMBO_SELECT)
            {
                if (pVal.ItemUID == ComboBoxGestion.UniqueId)
                {
                    ComboBoxGestionSelected(pVal);
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
