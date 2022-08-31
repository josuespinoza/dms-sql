using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Threading;
using System.Globalization;
using SAPbobsCOM;

namespace SCG.Requisiciones
{
    public partial class ListadoRequisiciones
    {

        #region ...Declaraciones...

        private SAPbobsCOM.Company m_oCompany;
        private SAPbouiCOM.Application m_oApplication;

        private SAPbouiCOM.DataTable dtFechas;
        private SAPbouiCOM.DataTable dtResultados;
        private SAPbouiCOM.DataTable dtConsulta;
        private SAPbouiCOM.DataTable dtSucursales;
        private SAPbouiCOM.DataTable dtTipoArticulos;
        private SAPbouiCOM.DataTable dtEncBodega;
        private SAPbouiCOM.DataTable dtTipoRequisicion;
        private SAPbouiCOM.DataTable dtEstado;
        private SAPbouiCOM.DataTable dtListCanc;

        private const String strDtSucursales = "dtSucursales";
        private const String strDtTipoArticulos = "dtTipoArticulos";
        private const String strDtEncBodega = "dtEncBodega";
        private const String strDtTipoRequisicion = "dtTipoRequisicion";
        private const String strDtEstado = "dtEstado";
        private const String strDtResultados = "dtResultados";
        private const String strDtConsulta = "dtLocal";
        private const String strDtFechas = "dtFechas";
        private const String strMtxLsReq = "mtxListReq";
        private const String strMtxListCanc = "mtxListCan";//revisar nombre
        private const String strDtListCanc = "dtListCanc";

        private MatrixSBOListReq mtxListReq;
        private MatrixSBOListCan mtxListCanc;
        private SAPbouiCOM.EditText g_oEditNoReq;
        private SAPbouiCOM.EditText g_oEditNoOT;
        private SAPbouiCOM.EditText g_oEditNoCot;
        private SAPbouiCOM.EditText g_oEditFecIni;
        private SAPbouiCOM.EditText g_oEditFecFin;
        private SAPbouiCOM.Matrix g_oMatrixListaReq;
        private SAPbouiCOM.Matrix g_oMatrixListaCanc;

        private SAPbouiCOM.ComboBox g_oComboEstado;
        private SAPbouiCOM.ComboBox g_oComboTipoArticulo;
        private SAPbouiCOM.ComboBox g_oComboTipoRequisicion;
        private SAPbouiCOM.ComboBox g_oComboSucursal;

        private SAPbouiCOM.CheckBox g_oChkDate;
        
        #endregion

        #region ...Propiedades...

        public SAPbobsCOM.ICompany CompanySBO { get; set; }
        public SAPbouiCOM.IApplication ApplicationSBO { get; set; }
        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO { get; set; }
        public bool Inicializado { get; set; }

        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        #endregion

        #endregion

        #region ...Constructor...
        public ListadoRequisiciones(Application application, SAPbobsCOM.ICompany companySbo)
        {
            CompanySBO = companySbo;
            ApplicationSBO = application;
            m_oCompany = (SAPbobsCOM.Company)companySbo;
            m_oApplication = application;
        }
        #endregion

        #region ...Metodos...

        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);
                dtSucursales = FormularioSBO.DataSources.DataTables.Add(strDtSucursales);
                dtTipoArticulos = FormularioSBO.DataSources.DataTables.Add(strDtTipoArticulos);
                dtEncBodega = FormularioSBO.DataSources.DataTables.Add(strDtEncBodega);
                dtTipoRequisicion = FormularioSBO.DataSources.DataTables.Add(strDtTipoRequisicion);
                dtEstado = FormularioSBO.DataSources.DataTables.Add(strDtEstado);

                var _udsFormulario = FormularioSBO.DataSources.UserDataSources;
                _udsFormulario.Add("txt_DateS", BoDataType.dt_DATE, 50);
                _udsFormulario.Add("txt_DateF", BoDataType.dt_DATE, 50);
                _udsFormulario.Add("txt_NoReq", BoDataType.dt_LONG_NUMBER, 50);
                _udsFormulario.Add("chkDate", BoDataType.dt_SHORT_TEXT, 1);
                
                dtResultados = FormularioSBO.DataSources.DataTables.Add(strDtResultados);
                dtResultados.Columns.Add("ColNoReq", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColNoOT", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColTipArt", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColTipReq", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColDate", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColHora", BoFieldsType.ft_AlphaNumeric, 100);
                dtResultados.Columns.Add("ColStatus", BoFieldsType.ft_AlphaNumeric, 100);

                mtxListReq = new MatrixSBOListReq(strMtxLsReq, FormularioSBO, strDtResultados);
                mtxListReq.CreaColumnas();
                mtxListReq.LigaColumnas();

                dtListCanc = FormularioSBO.DataSources.DataTables.Add(strDtListCanc);
                dtListCanc.Columns.Add("ColNoReq", BoFieldsType.ft_AlphaNumeric, 20);
                dtListCanc.Columns.Add("ColNoOT", BoFieldsType.ft_AlphaNumeric, 10);
                dtListCanc.Columns.Add("ColCod", BoFieldsType.ft_AlphaNumeric, 100);
                dtListCanc.Columns.Add("ColDes", BoFieldsType.ft_AlphaNumeric, 150);
                dtListCanc.Columns.Add("ColCant", BoFieldsType.ft_Quantity,10);

                mtxListCanc = new MatrixSBOListCan(strMtxListCanc, FormularioSBO, strDtListCanc);
                mtxListCanc.CreaColumnas();
                mtxListCanc.LigaColumnas(); 

                g_oEditFecIni = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtFecIni").Specific;
                g_oEditFecFin = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtFecFin").Specific;
                g_oEditNoReq = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtNoReq").Specific;
                g_oChkDate = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkDate").Specific;
                
                _udsFormulario.Item("txt_DateS").Value = DateTime.Now.ToString("yyyyMMdd");
                _udsFormulario.Item("txt_DateF").Value = DateTime.Now.ToString("yyyyMMdd");
                _udsFormulario.Item("chkDate").Value = "1";
                
                g_oEditFecIni.DataBind.SetBound(true, "", "txt_DateS");
                g_oEditFecFin.DataBind.SetBound(true, "", "txt_DateF");
                g_oEditNoReq.DataBind.SetBound(true, "", "txt_NoReq");
                g_oChkDate.ValOn = "1";
                g_oChkDate.ValOff = "0";
                g_oChkDate.DataBind.SetBound(true, "", "chkDate");

                CargaCombos();
                CargarMatriz();
                CargarMatrizCanc();
                FormularioSBO.PaneLevel = 1;
            }
        }

        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {
                FormType = FormularioSBO.TypeEx;
                FormularioSBO.Title = Titulo;
            }
        }


        #endregion

    }
}
