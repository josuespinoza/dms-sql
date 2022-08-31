using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta.CreaciónOTEspecial
{
    public partial class OTEspecial : IFormularioSBO
    {
        private static NumberFormatInfo n;
        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO { get; set; }
        public bool Inicializado { get; set; }
        public IApplication ApplicationSBO { get; private set; }
        public ICompany CompanySBO { get; private set; }
        private EditText g_oEditNoOT;
        private EditText g_oEditNoCot;
        private Matrix g_oMtxOtLines;
        private Item g_sboItem;
        private ComboBox g_sboCombo;
        private SAPbouiCOM.DataTable dtLineas;
        private MatrizOTEspecial g_oMatrixOTEspecial;
        private UserDataSources UDS_SeleccionaRepuestos;
        private ComboBoxSBO cboTOT;
        private SAPbouiCOM.DataTable g_dtEstadosOT;
        private const string mc_strMatizCotLines = "mtxOTLines";
        private const string mc_strTipoOtEspeciales = "cboTipOtE";
        private const string strDataTableLineas = "tTodosLineas";
        private const string strDataTableConsulta = "dtConsulta";
        private const string strDataTableConsultaClienteTipoOrden = "dtConsultaClienteTipoOrden";
        public const string g_strdtEstadosOT = "tEstadosOT";

        public static string g_strNOOT { get; set; }
        public static string g_strDocE { get; set; }
        public static Boolean g_SOOTEsp { get; set; }

        public void InicializarControles()
        {
            FormularioSBO.Freeze(true);
            CargarTiposOtEspeciales();
            FormularioSBO.Freeze(false);
        }

        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {
                CargarFormulario();

                UserDataSources userDS = FormularioSBO.DataSources.UserDataSources;
                userDS.Add("noOT", BoDataType.dt_LONG_TEXT, 100);
                userDS.Add("noCot", BoDataType.dt_LONG_TEXT, 100);

                g_oEditNoOT = (EditText)FormularioSBO.Items.Item("txtNoOT").Specific;
                g_oEditNoCot = (EditText)FormularioSBO.Items.Item("txtNoCot").Specific;
                g_oMtxOtLines  = (Matrix)FormularioSBO.Items.Item("mtxOTLines").Specific;

                g_oEditNoOT.DataBind.SetBound(true, "", "noOT");
                g_oEditNoCot.DataBind.SetBound(true, "", "noCot");

                g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Add(g_strdtEstadosOT);
                g_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ");
            }
        }

        private void CargarFormulario()
        {
            SAPbouiCOM.DataTable dtLocal;

            try
            {
                FormularioSBO.Freeze(true);

                AsociacionControlesInterfaz();

                dtLocal = FormularioSBO.DataSources.DataTables.Item(strDataTableConsulta);
                dtLineas = FormularioSBO.DataSources.DataTables.Add(strDataTableLineas);
                dtLineas.Columns.Add("col_Sel", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Code", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Name", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Quant", BoFieldsType.ft_Quantity, 100);
                dtLineas.Columns.Add("col_Curr", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Price", BoFieldsType.ft_Price, 100);
                dtLineas.Columns.Add("col_Obs", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_DEnt", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_LNum", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_PrcDes", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_IdRXOr", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Costo", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_IndImp", BoFieldsType.ft_AlphaNumeric, 100);
                dtLineas.Columns.Add("col_Compra", BoFieldsType.ft_AlphaNumeric, 10);
                dtLineas.Columns.Add("col_CPend", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_CSol", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_CRec", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_PenDev", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_PenTra", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_PenBod", BoFieldsType.ft_Quantity);
                dtLineas.Columns.Add("col_IDLine", BoFieldsType.ft_AlphaNumeric);
                dtLineas.Columns.Add("col_TipArt", BoFieldsType.ft_AlphaNumeric);
                dtLineas.Columns.Add("col_Comprar", BoFieldsType.ft_AlphaNumeric); 

                g_oMatrixOTEspecial = new MatrizOTEspecial(mc_strMatizCotLines, FormularioSBO, strDataTableLineas);
                g_oMatrixOTEspecial.CreaColumnas();
                g_oMatrixOTEspecial.LigaColumnas();

                g_oMatrixOTEspecial.Matrix.Columns.Item("col_Sel").Editable = true;

                FormularioSBO.Freeze(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AsociacionControlesInterfaz()
        {
            try
            {
                UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources;
                UDS_SeleccionaRepuestos.Add("TOT", BoDataType.dt_LONG_TEXT, 100);

                cboTOT = new ComboBoxSBO("cboTipOtE", FormularioSBO, true, "", "TOT");
                cboTOT.AsignaBinding();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
