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
    public partial class AdicionalesOT : IFormularioSBO
    {

        #region ...Estructuras...
        public struct ItemsSeleccionados
        {
            public string ItemCode;
            public Int32 Posicion;
        }
        #endregion

        #region ....Variables....

        public List<ItemsSeleccionados> lstSeleccionados;

        private UserDataSources UDS_SeleccionaRepuestos;
        public static EditTextSBO txtCode;
        public static EditTextSBO txtDescripcion;
        public static EditTextSBO txtCodeBar;

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public SAPbouiCOM.IForm FormularioSBO { get; set; }
        public SAPbobsCOM.Company SBOCompany;

        public string DocCurrency { get; set; }

        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }

        public static int g_IntTipoAdicional { get; set; }

        public string g_strmtxAdicionales = "mtxAdic";
        public string g_strdtAdicionales = "tAdicionales";
        public string g_strdtAdicionalesSel = "tAdicionalesSel";
        public string g_strdtExisteArt = "tExistArt";
        public string g_strConfAdmin = "tconfAdmin";
        public string g_strConfSucu = "tconfSucu";
        public string g_strEstMod = "tEstmod";
        public string g_strLisPrecCliente = "tLisPreCli";
        public string g_strdtConsulta = "dtConsulta";

        public MatrizAdicionales g_objMatrizAdicionales;
        public static SAPbouiCOM.DataTable g_dtAdicionales;
        public static SAPbouiCOM.DataTable g_dtAdicionalesSeleccionados;
        public static SAPbouiCOM.DataTable g_dtExisteArt;
        public static SAPbouiCOM.DataTable g_dtConfAdmin;
        public static SAPbouiCOM.DataTable g_dtAConfSucu;
        public static SAPbouiCOM.DataTable g_dtEstMod;
        public static SAPbouiCOM.DataTable g_dtLisPrecCliente;
        public static SAPbouiCOM.DataTable g_dtConsulta;

        public static SAPbouiCOM.DataTable dtConfAdmin;
        public static SAPbouiCOM.DataTable dtConf;
        public static SAPbouiCOM.DataTable dtListPreCliente;
        public static SAPbouiCOM.DataTable dtCodeEstiMode;
        public static string m_strUsaAsocxEspecif;
        public static string m_strEspecifVehi;
        public static string m_strCodEstilo;
        public static string m_strCodModelo;
        public static string m_strUsaFilRep;
        public static string m_strUsaFilSer;
        public static bool g_bExisteArticulos = false;
        public static string strConsulta;
        public string g_strConsultaArtiEspXModeEsti =
                 " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                 " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                 " 1 as cant, it.Price as prec, it.Currency as mone, Art.U_Duracion as dura, oi.U_SCGD_T_Fase as nofa,oi.CodeBars" +
                 " from OITM as oi with (nolock) " +
                 " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                 " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                 " inner join [@SCGD_ARTXESP] as Art with(nolock) on oi.ItemCode = art.U_ItemCode  " +
                 " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}'  {2}";
        public static string g_strConsultaEstiModConf = string.Empty;
        public static bool g_UsaConsultaEstiMod = false;
        public static string g_strUsaConsultaSegunConf;
        public static string g_strEsti;
        public static string g_strMod;
        public string m_strConsultaListaPreciosCliente = "Select ListNum from OCRD where CardCode = '{0}'";
        public string m_strConsultaExistenciaArt = "Select Count(U_ItemCode) as U_ItemCode from [@SCGD_ARTXESP] where U_TipoArt = '{0}'";
        public static string m_strUsaListaPrecCliente;
        public static string g_strDocEntry;
        public static string g_strCodListPrecio;
        public static string g_strEspecifVehif;


        public string g_strConsultaRepuestos =
                    " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                    " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                    " 1.0 as cant, it.Price as prec, it.Currency as mone, '' as dura, '' as nofa,oi.CodeBars" +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " where cfnb.DocEntry = ( select DocEntry from [@SCGD_CONF_SUCURSAL] where U_Sucurs = ( select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}' ) ) " +
                    " and oi.U_SCGD_TipoArticulo = '1' " +
                    " and it.PriceList = '{1}' and oi.validFor = 'Y' ";

        public string g_strConsultaSuministros =
                    " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Sum as bode, " +
                    " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Sum and oitw.ItemCode = oi.ItemCode) as csto, " +
                    " 1.0 as cant, it.Price as prec, it.Currency as mone, '' as dura, '' as nofa,oi.CodeBars" +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " where cfnb.DocEntry = ( select DocEntry from [@SCGD_CONF_SUCURSAL] where U_Sucurs = ( select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}' ) ) " +
                    " and oi.U_SCGD_TipoArticulo = '3' " +
                    " and it.PriceList = '{1}' and oi.validFor = 'Y' ";

        public string g_strConsultaServicios =
                    " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', '' as bode, " +
                    " '' as csto, 1.0 as cant, it.Price as prec, it.Currency as mone, oi.U_SCGD_Duracion as dura, oi.U_SCGD_T_Fase as nofa,oi.CodeBars " +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " where cfnb.DocEntry = ( select DocEntry from [@SCGD_CONF_SUCURSAL] where U_Sucurs = ( select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}' ) ) " +
                    " and oi.U_SCGD_TipoArticulo = '2' " +
                    " and it.PriceList = '{1}' and oi.validFor = 'Y' ";

        public string g_strConsultaServiciosExternos =
                    " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', '' as bode, " +
                    " '' as csto, 1.0 as cant, it.Price as prec, it.Currency as mone,'' as dura, '' as nofa,oi.CodeBars " +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " where cfnb.DocEntry = ( select DocEntry from [@SCGD_CONF_SUCURSAL] where U_Sucurs = ( select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}' ) ) " +
                    " and oi.U_SCGD_TipoArticulo = '4' " +
                    " and it.PriceList = '{1}' and oi.validFor = 'Y' ";

        public string strConsultaConfEspec = "select ISNULL(U_UsaSolEsp, 'N') as U_UsaSolEsp  from [@SCGD_CONF_SUCURSAL] where U_Sucurs=(select U_SCGD_idSucursal from OQUT where U_SCGD_Numero_OT = '{0}') ";
        
        public static string strCodCliente;
        public static string strNoOT;
        public static string strDocCur;
        public SAPbouiCOM.Button btnSolEspec;
        public NumberFormatInfo n;

        #endregion

        #region ...Enums...
        private enum TipoAdicional
        {
            Repuesto = 1,
            Servicio = 2,
            ServicioExterno = 4,
            Suministros = 3
        }
        #endregion

        #region ...Constructor...
        public AdicionalesOT(IApplication applicationSBO, ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                SBOCompany = (SAPbobsCOM.Company)companySBO;

                n = DIHelper.GetNumberFormatInfo(companySBO);
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        #endregion

        #region ...Metodos...
        public void InicializarControles()
        {
            g_dtAdicionales = FormularioSBO.DataSources.DataTables.Add(g_strdtAdicionales);
            g_dtAdicionales.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("bode", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("csto", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
            g_dtAdicionales.Columns.Add("prec", BoFieldsType.ft_Price, 100);
            g_dtAdicionales.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("nofa", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionales.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100);
            
            g_objMatrizAdicionales = new MatrizAdicionales(g_strmtxAdicionales, FormularioSBO, g_strdtAdicionales);
            g_objMatrizAdicionales.CreaColumnas();
            g_objMatrizAdicionales.LigaColumnas();

            g_dtAdicionalesSeleccionados = FormularioSBO.DataSources.DataTables.Add(g_strdtAdicionalesSel);
            g_dtAdicionalesSeleccionados.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("bode", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("csto", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("prec", BoFieldsType.ft_Price, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("nofa", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAdicionalesSeleccionados.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100);

            g_dtConfAdmin = FormularioSBO.DataSources.DataTables.Add(g_strConfAdmin);
            //g_dtConfAdmin.Columns.Add("U_UsaAXEV", BoFieldsType.ft_AlphaNumeric, 100);
            //g_dtConfAdmin.Columns.Add("U_EspVehic", BoFieldsType.ft_AlphaNumeric, 100);
            //g_dtConfAdmin.Columns.Add("U_UsaFilRep", BoFieldsType.ft_AlphaNumeric, 100);

            g_dtAConfSucu = FormularioSBO.DataSources.DataTables.Add(g_strConfSucu);

            g_dtEstMod = FormularioSBO.DataSources.DataTables.Add(g_strEstMod);
            g_dtLisPrecCliente = FormularioSBO.DataSources.DataTables.Add(g_strLisPrecCliente);

            g_dtExisteArt = FormularioSBO.DataSources.DataTables.Add(g_strdtExisteArt);
            g_dtExisteArt.Columns.Add("U_ItemCode", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtConsulta = FormularioSBO.DataSources.DataTables.Add(g_strdtConsulta);
            //g_dtAConfSucu.Columns.Add("DocEntry", BoFieldsType.ft_AlphaNumeric, 100);
            //g_dtAConfSucu.Columns.Add("U_CodLisPre", BoFieldsType.ft_AlphaNumeric, 100);
            //g_dtAConfSucu.Columns.Add("U_UseLisPreCli", BoFieldsType.ft_AlphaNumeric, 100);
            lstSeleccionados=new List<ItemsSeleccionados>();
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
            SAPbouiCOM.DataTable dtLocal;
            Matrix m_objMatrix;

            try
            {
                FormularioSBO.Freeze(true);
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        #endregion

    }
}
