using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Server;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public partial class DocumentoCompra : IFormularioSBO
    {

        private static NumberFormatInfo n;

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO { get; set; }
        public bool Inicializado { get; set; }

        public IApplication ApplicationSBO { get; private set; }
        public ICompany CompanySBO { get; private set; }

        public static SAPbouiCOM.DataTable g_dtItemsCompra;
        public static SAPbouiCOM.DataTable g_dtCantidades;
        public static SAPbouiCOM.DataTable g_dtSerie;
        public static SAPbouiCOM.DataTable g_dtIDCompra;
        public static SAPbouiCOM.DataTable dtConsulta;
        public string g_strdtDocCompra = "tItCompra";
        public string g_strdtCantidad = "tCantidad";
        public string g_strmtxDocCompra = "mtxDocCom";
        public static string g_strdtSerie = "tSerie";
        public static string g_strdtCompra = "tCompra";
        public static string g_strdtConsulta = "dtLocal";
        public static string g_strOT = "SCGD_ORDT";
        public const string g_dtArt = "tArt";
        public IForm g_oformOT;
        public static TipoAdicional g_tipoAdicional;
        
        public static string g_strOfertaCompra;
        public static string g_strOrdenCompra;
        public static string g_strSerieOrden;
        public static string g_strSerieOferta;
        public static string g_strDocEntry;
        public static string g_strUsaDimension;
        public OrdenTrabajo g_objOrdentrabajo;

        public static string g_ProvCode;

       
        public static string g_ProvName;


        private SAPbouiCOM.DataTable g_dtBodxCC;

        public GestorFormularios g_objGestorFormularios;

        public const string g_strdtBodegasCentroCosto = "tBodxCC";

        public BuscadorProveedores g_FormularioBuscadorProveedores;

        public const string g_strConsultaBodegasCentroCosto =
            " select cnfs.U_Sucurs as Sucursal,U_CC as CentroCosto, U_Rep as Repuestos, U_Ser as Servicios, U_Sum as Suministros, U_SE as ServExt, U_Pro as Proceso " +
            " from [@SCGD_CONF_BODXCC] as bxcc inner join [@SCGD_CONF_SUCURSAL] as cnfs on bxcc.DocEntry = cnfs.DocEntry ";

        public const string g_strConsultaORdenCompra = " Select TOP 1 op.DocNum,op.Series from {0} as op" +
                                                     " inner join OQUT as oq on op.U_SCGD_Numero_OT = oq.U_SCGD_Numero_OT" +
                                                     " inner join QUT1 as qu on oq.U_SCGD_Numero_OT = qu.U_SCGD_NoOT" +
                                                     " where   qu.U_SCGD_ID = '{1}' order by op.DocNum DESC";

        public const string g_strConsultaIndicadorImpuestos =
            "select ISNULL(U_Imp_Repuestos, '') Repuestos, ISNULL(U_Imp_Serv, '') Servicios, ISNULL(U_Imp_ServExt, '') ServExt, ISNULL(U_Imp_Suminis, '') Suministros, ISNULL(U_Imp_Gastos, '') Gastos, ISNULL(U_ImpRepCom, '') CompraRep, ISNULL(U_ImpSECom, '') CompraSE from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'";
        private MatrizDocumentoCompra g_objMatrizDocCompra;

        public DocumentoCompra(IApplication applicationSBO, ICompany companySBO)
        {
            Application application;
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                application = (Application) applicationSBO;
                n = DIHelper.GetNumberFormatInfo(companySBO);
                g_objGestorFormularios = new GestorFormularios(ref application);
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        public enum TipoAdicional
        {
            Repuesto = 1,
            Servicio = 2,
            ServicioExterno = 3,
            Suministro = 4,
            Gastos = 5
        }


        public void InicializarControles()
        {
            dtConsulta = FormularioSBO.DataSources.DataTables.Add(g_strdtConsulta);

            g_dtItemsCompra = FormularioSBO.DataSources.DataTables.Add(g_strdtDocCompra);
            g_dtItemsCompra.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtItemsCompra.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtItemsCompra.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtItemsCompra.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtItemsCompra.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
            g_dtItemsCompra.Columns.Add("prec", BoFieldsType.ft_Price, 100);
            g_dtItemsCompra.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtItemsCompra.Columns.Add("tax", BoFieldsType.ft_AlphaNumeric, 10);
            g_dtItemsCompra.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);

            g_objMatrizDocCompra = new MatrizDocumentoCompra(g_strmtxDocCompra, FormularioSBO, g_strdtDocCompra);
            g_objMatrizDocCompra.CreaColumnas();
            g_objMatrizDocCompra.LigaColumnas();

            //Seteo el impuesto segun configuracion
            g_objMatrizDocCompra.ColumnaTax.Columna.ChooseFromListUID = DMS_Connector.Helpers.TipodeImpuesto("CFLTAX").Trim();
            g_objMatrizDocCompra.ColumnaTax.Columna.ChooseFromListAlias  = "Code";
            //***** Localizacion Costa Rica - IVA *****
            if (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR == "Y")
            {
                g_objMatrizDocCompra.ColumnaTax.Columna.Visible = false;
            }
            g_dtSerie = FormularioSBO.DataSources.DataTables.Add(g_strdtSerie);
            g_dtSerie.Columns.Add("Serie", BoFieldsType.ft_AlphaNumeric, 100);

            g_dtIDCompra = FormularioSBO.DataSources.DataTables.Add(g_strdtCompra);
            g_dtIDCompra.Columns.Add("DocNum", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtIDCompra.Columns.Add("Series", BoFieldsType.ft_AlphaNumeric, 100);


            g_dtCantidades = FormularioSBO.DataSources.DataTables.Add(g_strdtCantidad);
            g_dtCantidades.Columns.Add("Id", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtCantidades.Columns.Add("Cant", BoFieldsType.ft_Quantity, 100);

            g_dtBodxCC = FormularioSBO.DataSources.DataTables.Add(g_strdtBodegasCentroCosto);
        }

        public void InicializaFormulario()
        {
            try
            {
                CargarFormulario();

                g_dtBodxCC = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                g_dtBodxCC.ExecuteQuery(g_strConsultaBodegasCentroCosto);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargarFormulario()
        {
            SAPbouiCOM.DataTable dtLocal;
            Matrix m_objMatrix;

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

        private void CargarFormularioBuscadorProveedores(ItemEvent pVal, ref bool bubbleEvent)
        {
            try
            {
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                g_FormularioBuscadorProveedores.NombreXml = Environment.CurrentDirectory + Resource.frmBuscadorProveedores;  
                
                g_FormularioBuscadorProveedores.m_objDocCompra = this;
                //g_FormularioBuscadorProveedores.NombreXml = System.Environment.CurrentDirectory + Resource.frmBuscadorProveedores;
                g_FormularioBuscadorProveedores.FormType = "SCGD_BPRO";

                if (!g_objGestorFormularios.FormularioAbierto(g_FormularioBuscadorProveedores, true))
                {
                    g_FormularioBuscadorProveedores.FormularioSBO =
                        g_objGestorFormularios.CargarFormulario(g_FormularioBuscadorProveedores);

                    g_FormularioBuscadorProveedores.ManejadorEventoFormDataLoad(pVal, bubbleEvent);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
