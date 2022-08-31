using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using ICompany = SAPbobsCOM.ICompany;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta 
{

    public partial class TrackingRepuestos : IFormularioSBO 
    {

        public MatrizTrackingRepuestos   g_objMatriztrack;
        public static SAPbouiCOM.DataTable g_dtTrack;
        public OrdenTrabajo OrdenTrabajo { get; set; }
        public string g_strdtTrack = "dtTracking";
        public string g_strmtxTrack = "mtxTr";
        public static string strCode;
        public static string strNoOT;
        public static string strID;

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public IForm FormularioSBO { get; set; }

        public ICompany CompanySBO { get; private set; }
        
        public TrackingRepuestos(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                NombreXml = Environment.CurrentDirectory + Resource.FrmTrackRepuestos;
                FormType = "SCG_TRA";
            }
            catch (Exception)
            {
            }
        }

        public void InicializarControles()
        {

            SAPbouiCOM.DataTable dtTracking;
            if (!Utilitarios.ValidaSiDataTableExiste((SAPbouiCOM.Form)FormularioSBO, g_strdtTrack))
                dtTracking = FormularioSBO.DataSources.DataTables.Add(g_strdtTrack);
            else
                dtTracking = FormularioSBO.DataSources.DataTables.Item(g_strdtTrack);

            dtTracking.Columns.Add("Prov", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("FeSo", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("TDocD", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("TDoc", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("DocE", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("DocN", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("Obse", BoFieldsType.ft_AlphaNumeric, 254);
            dtTracking.Columns.Add("CanEn", BoFieldsType.ft_Quantity, 100);
            dtTracking.Columns.Add("CanSo", BoFieldsType.ft_Quantity, 100);
            g_objMatriztrack = new MatrizTrackingRepuestos(g_strmtxTrack, FormularioSBO, g_strdtTrack);
            g_objMatriztrack.CreaColumnas();
            g_objMatriztrack.LigaColumnas();

        }

        public void InicializaFormulario()
        {
            try
            {
                CargarFormulario();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void CargarFormulario()
        {
           try
            {
                FormularioSBO.Freeze(true);
                FormularioSBO.Freeze(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IApplication ApplicationSBO { get; private set; }
    }
}
