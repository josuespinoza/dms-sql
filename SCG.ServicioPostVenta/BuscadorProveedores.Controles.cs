using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public partial class BuscadorProveedores : IFormularioSBO
    {

        private static NumberFormatInfo n;

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO { get; set; }
        public bool Inicializado { get; set; }

        public IApplication ApplicationSBO { get; private set; }
        public ICompany CompanySBO { get; private set; }

        public GestorFormularios g_objGestorFormularios;

        private MatrizProveedores g_objMatrizProveedores;
        public static SAPbouiCOM.DataTable g_dtProveedor;
        public string g_strdtProveedores = "dtProvee";
        public string g_strmtxProveedores = "mtxProv";

        public string g_strConsulta = " select '' as sele, CardCode as codi, CardName as nomb from OCRD where CardType = 'S' and validFor = 'Y' ";
        public string g_strConsultaFiltros = " select '' as sele, CardCode as codi, CardName as nomb from OCRD where CardType = 'S' and validFor = 'Y' ";
        public string g_strConsultaFiltrosCode = " and CardCode like '{0}%' ";
        public string g_strConsultaFiltrosName = " and CardName like '{0}%' ";

        public BuscadorProveedores(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;

                //g_objGestorFormularios = new GestorFormularios(applicationSBO);

                n = DIHelper.GetNumberFormatInfo(companySBO);
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void InicializarControles()
        {
            try
            {
                g_dtProveedor = FormularioSBO.DataSources.DataTables.Add(g_strdtProveedores);
                g_dtProveedor.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtProveedor.Columns.Add("codi", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtProveedor.Columns.Add("nomb", BoFieldsType.ft_AlphaNumeric, 100);

                g_objMatrizProveedores = new MatrizProveedores(g_strmtxProveedores, FormularioSBO, g_strdtProveedores);
                g_objMatrizProveedores.CreaColumnas();
                g_objMatrizProveedores.LigaColumnas();
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void InicializaFormulario()
        {
            try
            {
                CargarFormulario();
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormulario()
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtTabla;
            try
            {
                oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(g_strmtxProveedores).Specific;
                oMatrix.FlushToDataSource();

                dtTabla = FormularioSBO.DataSources.DataTables.Item(g_strdtProveedores);

                oMatrix.FlushToDataSource();
                dtTabla.ExecuteQuery(g_strConsulta);
                oMatrix.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
    }
}
