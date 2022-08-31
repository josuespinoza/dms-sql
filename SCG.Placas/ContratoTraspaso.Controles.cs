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
    partial class ContratoTraspaso : IFormularioSBO, IUsaMenu
    {
        public ContratoTraspaso(Application applicationSBO, ICompany companySBO)
        {
            ApplicationSBO = applicationSBO;
            CompanySBO = companySBO;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo cultureInfo = My.Resources.Resource.Culture;
            DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            My.Resources.Resource.Culture = cultureInfo;
        }

        public EditTextSBO EditTextContratoV;
        public ButtonSBO ButtonBuscar;
        public ButtonSBO ButtonImprimir;

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
                userDataSources.Add("numeroCV", BoDataType.dt_LONG_TEXT, 200);

                EditTextContratoV = new EditTextSBO("txtContV", true, "", "numeroCV", FormularioSBO);
                EditTextContratoV.AsignaBinding();

                ButtonBuscar = new ButtonSBO("btnBuscar", FormularioSBO);
                ButtonImprimir = new ButtonSBO("btnImpr", FormularioSBO);

                FormularioSBO.Freeze(false);
            }
        }

        public void InicializaFormulario()
        {
        }

        public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.FormTypeEx != FormType) return;

            if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                if (pVal.ItemUID == ButtonBuscar.UniqueId)
                {
                    CFLCargaContratoVenta(formUid, pVal);
                }
            }

            else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
            {
                if (pVal.ItemUID == ButtonImprimir.UniqueId)
                {
                    if (pVal.ItemUID == ButtonImprimir.UniqueId)
                    {
                        ButtonSBOImprimirReporteItemPressed(formUid, pVal, ref bubbleEvent);
                    }
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
