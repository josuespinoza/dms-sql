using System;
using SAPbouiCOM;
using SCG.Requisiciones.UI;

namespace SCG.Requisiciones
{
    public partial class ListaUbicaciones
    {
        #region ...Declaraciones...

        private Form oForm;
        public DataTable dtUbicaciones;
        public DataTable dtConsulta;

        //private MatrizSelListEmp As MatrizListaEmpSel
        private const String strDtUbicaciones = "dtUbicaciones";
        private const String strDtConsulta = "dtLocal";
        private const String strMtxUbi = "mtxUbi";
        private const String strUFormRequisicion = "SCGD_FormRequisicion";
        //private const String strFormUbiID = "SCGD_SLUB";

        private MatrixSBOUbicaciones mtxUbicaciones;
        //public MatrixSBOLineasRequisiciones MatrixRequisiciones;
        private Matrix g_oMtxUbicaciones;
        private Matrix g_oMtxRequisiciones;
        private EditText g_oEditCodBod;
        private EditText g_oEditItemCode;
        private EditText g_oEditBusqueda;
        private EditText g_oEditLineNum;

        #endregion

        #region  ...Propiedades...

        public MatrixSBOLineasRequisiciones MatrixRequisiciones { get; set; }

        public SAPbouiCOM.IApplication ApplicationSBO { get; private set; }

        public SAPbobsCOM.ICompany CompanySBO { get; private set; }

        public string FormType { get; set; }

        public string NombreXml { get; set; }

        public string Titulo { get; set; }

        public IForm FormularioSBO { get; set; }

        public bool Inicializado { get; set; }

        #endregion

        #region  "constructor"

        public ListaUbicaciones(Application application, SAPbobsCOM.ICompany companySbo)
        {
            CompanySBO = companySbo;
            ApplicationSBO = application;
        }

        #endregion

        #region ...Metodos...
        
        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);
                dtUbicaciones = FormularioSBO.DataSources.DataTables.Add(strDtUbicaciones);
                dtUbicaciones.Columns.Add("colCodUbi", BoFieldsType.ft_AlphaNumeric, 100);
                dtUbicaciones.Columns.Add("colDesUbi", BoFieldsType.ft_AlphaNumeric, 100);
                dtUbicaciones.Columns.Add("colQtyHnd", BoFieldsType.ft_AlphaNumeric, 100);

                mtxUbicaciones = new MatrixSBOUbicaciones(strMtxUbi, FormularioSBO, strDtUbicaciones);
                mtxUbicaciones.CreaColumnas();
                mtxUbicaciones.LigaColumnas();

                UserDataSources userDS = FormularioSBO.DataSources.UserDataSources;
                userDS.Add("idBod", BoDataType.dt_LONG_TEXT, 100);
                userDS.Add("itmCode", BoDataType.dt_LONG_TEXT, 100);
                userDS.Add("lineNum", BoDataType.dt_LONG_TEXT, 100);

                g_oEditCodBod = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtIDBod").Specific;
                g_oEditItemCode = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtItmCode").Specific;
                g_oEditLineNum = (SAPbouiCOM.EditText)FormularioSBO.Items.Item("txtLineNum").Specific;
                
                g_oMtxUbicaciones = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(strMtxUbi).Specific;

                g_oEditCodBod.DataBind.SetBound(true, "", "idBod");
                g_oEditItemCode.DataBind.SetBound(true, "", "itmCode");
                g_oEditLineNum.DataBind.SetBound(true, "", "lineNum");
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
