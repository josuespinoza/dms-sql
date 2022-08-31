using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public partial class FinalizaActividad : IFormularioSBO
    {
        public static string strCodeEmp;
        public static string strIDAct;
        public static string strNoOT;
        public static SAPbouiCOM.DataTable g_dtConsulta;
        public string g_strdtConsu = "dtCon";
        public string g_ConsultaDes ="Select Dscription, U_SCGD_NombEmpleado from QUT1 with(nolock) where U_SCGD_ID = '{0}' and U_SCGD_EmpAsig = '{1}' ";
        public static string strDocEntry;
        public static int idlinea;
        public static string strCosto;
        public static string strFechaIni;
        public static string strHoraIni;
        public static string AsigUniMec { get; set; }
        public static string CodFase { get; set; }
        public static string NoFase { get; set; }
        public static string EstadoAct { get; set; }

        #region Inicializadores

        public string FormType { get; set; }
        public string idSucursal { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO{get; set; }
        public bool Inicializado { get; set; }
        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }
        public static bool ConfEstandar { get; set; }
        public OrdenTrabajo OrdenTrabajo { get; set; }
        
        #endregion




        #region Metodos

        public FinalizaActividad(IApplication applicationSBO, ICompany companySBO, string p_strPath)
        {
            try
            {
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                NombreXml = p_strPath;
                FormType = "SCGD_FIAct";
            }
            catch (Exception)
            {
            }
        }

        public void InicializarControles()
        {
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Add(g_strdtConsu);
        }

        public void InicializaFormulario()
        {

        }

        #endregion


    }
}
