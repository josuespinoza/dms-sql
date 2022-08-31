using System;
using SAPbouiCOM;
using ICompany = SAPbobsCOM.ICompany;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class TrackingSolEspecificos : IFormularioSBO
    {

        public MatrizTrackingSolEspecificos g_objMatriztrack;
        public static SAPbouiCOM.DataTable g_dtTrack;
        public OrdenTrabajo OrdenTrabajo { get; set; }
        public string g_strdtTrack = "dtTrackingSolEsp";
        public string g_strmtxTrack = "mtxTraSOLE";

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public IForm FormularioSBO { get; set; }
        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }

        public TrackingSolEspecificos(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                NombreXml = Environment.CurrentDirectory + Resource.frmTrackingSolEspecificos;
                FormType = "SCGD_TRASOL";
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

            dtTracking.Columns.Add("Solic", BoFieldsType.ft_Integer, 10);
            dtTracking.Columns.Add("Canti", BoFieldsType.ft_Quantity, 20);
            dtTracking.Columns.Add("ItemC", BoFieldsType.ft_AlphaNumeric, 50);
            dtTracking.Columns.Add("Descrip", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("FecSol", BoFieldsType.ft_AlphaNumeric, 50);
            dtTracking.Columns.Add("HoraSol", BoFieldsType.ft_AlphaNumeric, 10);
            dtTracking.Columns.Add("ItemR", BoFieldsType.ft_AlphaNumeric, 50);
            dtTracking.Columns.Add("DescRe", BoFieldsType.ft_AlphaNumeric, 100);
            dtTracking.Columns.Add("FecRes", BoFieldsType.ft_AlphaNumeric, 50);
            dtTracking.Columns.Add("HoraRes", BoFieldsType.ft_AlphaNumeric, 10);
            dtTracking.Columns.Add("Usuario", BoFieldsType.ft_AlphaNumeric, 15);
            
            g_objMatriztrack = new MatrizTrackingSolEspecificos(g_strmtxTrack, FormularioSBO, g_strdtTrack);
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
    }
}
