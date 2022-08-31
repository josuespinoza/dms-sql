using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public partial class RazonesSuspension : IFormularioSBO
    {
        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public SAPbouiCOM.IForm FormularioSBO { get; set; }

        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }
        public OrdenTrabajo OrdenTrabajo { get; set; }
        public static string idActividad { get; set; }
        public static string strFechaIni { get; set; }
        public static string strHoraIni { get; set; }

        public RazonesSuspension(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void InicializarControles()
        {
           
        }

        public void InicializaFormulario()
        {
            
        }
    }
}
