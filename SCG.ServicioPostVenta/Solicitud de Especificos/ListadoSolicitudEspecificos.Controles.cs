using System;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class ListadoSolicitudEspecificos
    {
        #region ...Propiedades...
        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public SAPbouiCOM.IForm FormularioSBO { get; set; }
        public Boolean ConfgUniMec { get; set; }
        public SAPbobsCOM.ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }
        public int intBranch { get; set; }

        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        #endregion

        #endregion

        #region ...Variables...

        public static SAPbouiCOM.DataTable g_dtSolicitudes;
        private SAPbouiCOM.DataTable dtConsulta;
        private SAPbouiCOM.DataTable dtQueryLista;
        public MatrizListadoSolicitudEspecificos g_objMatrizSolicitudes;

        private SCG.SBOFramework.UI.EditTextSBO EditTextFechaSIni;
        private SCG.SBOFramework.UI.EditTextSBO EditTextFechaSFin;
        private SCG.SBOFramework.UI.EditTextSBO EditTextFechaRIni;
        private SCG.SBOFramework.UI.EditTextSBO EditTextFechaRFin;

        private SAPbouiCOM.EditText g_oEditNoSol;
        private SAPbouiCOM.EditText g_oEditNoOT;
        private SAPbouiCOM.EditText g_oEditUnidad;
        private SAPbouiCOM.EditText g_oEditPlaca;

        private SAPbouiCOM.Matrix g_oMatrixListaSol;

        private SAPbouiCOM.ComboBox g_oComboMarca;
        private SAPbouiCOM.ComboBox g_oComboEstilo;
        private SAPbouiCOM.ComboBox g_oComboModelo;
        private SAPbouiCOM.ComboBox g_oComboSucursal;
        private SAPbouiCOM.ComboBox g_oComboEstado;

        private SAPbouiCOM.CheckBox g_oChkDateSol;
        private SAPbouiCOM.CheckBox g_oChkDateRes;
        private SAPbouiCOM.CheckBox g_oChkMarca;
        private SAPbouiCOM.CheckBox g_oChkEstilo;
        private SAPbouiCOM.CheckBox g_oChkModelo;
        private SAPbouiCOM.CheckBox g_oChkEstado;
        #endregion

        #region ...Constantes...
        public const string g_strdtSolicitudes = "dtSolicitudes";
        public const string g_strmtxTareas = "mtxListSoE";
        private const String strDtConsulta = "dtConsulta";
        private const String strDtQueryLista = "dtQueryLista";

        private const string queryListSol =
           "select se.DocEntry ColDocE, se.DocNum ColDocN, se.U_NumeroOT ColNoOT, se.U_FechaSol ColFecha, se.U_HoraSol ColHora, se.U_UserSol ColSolByC, " +
           "emp.firstName+' ' +isnull(emp.middleName, '')+' '+emp.lastName as ColSolBy, q.U_SCGD_Des_Marc ColMarca, q.U_SCGD_Des_Esti ColEstilo, " +
           "q.U_SCGD_Des_Mode ColModelo, " +
           "q.U_SCGD_Num_Placa ColPlaca, q.U_SCGD_Cod_Unidad ColUnidad " +
           "from [@SCGD_SOL_ESPEC] se with (nolock) " +
           "left join OQUT q with (nolock) on se.U_NumeroOT=q.U_SCGD_Numero_OT " +
           "left join OUSR usr with (nolock) on se.U_UserSol=usr.USER_CODE " +
           "left join OHEM emp with (nolock) on usr.userId=emp.userId ";

        #endregion

        #region ...Constructor...

        public ListadoSolicitudEspecificos(IApplication applicationSBO, SAPbobsCOM.ICompany companySBO)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

        }

        #endregion

        #region ...Metodos...

        public void InicializarControles()
        {
            dtQueryLista = FormularioSBO.DataSources.DataTables.Add(strDtQueryLista);

            g_dtSolicitudes = FormularioSBO.DataSources.DataTables.Add(g_strdtSolicitudes);
            g_dtSolicitudes.Columns.Add("ColDocE", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColDocN", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColNoOT", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColFecha", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColHora", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColSolBy", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColMarca", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColEstilo", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColModelo", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColUnidad", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtSolicitudes.Columns.Add("ColPlaca", BoFieldsType.ft_AlphaNumeric, 100);

            g_objMatrizSolicitudes = new MatrizListadoSolicitudEspecificos(g_strmtxTareas, FormularioSBO, g_strdtSolicitudes);
            g_objMatrizSolicitudes.CreaColumnas();
            g_objMatrizSolicitudes.LigaColumnas();

            FormularioSBO.DataSources.UserDataSources.Add("SStartDate", BoDataType.dt_DATE);
            FormularioSBO.DataSources.UserDataSources.Add("SEndDate", BoDataType.dt_DATE);
            FormularioSBO.DataSources.UserDataSources.Add("RStartDate", BoDataType.dt_DATE);
            FormularioSBO.DataSources.UserDataSources.Add("REndDate", BoDataType.dt_DATE);
            FormularioSBO.DataSources.UserDataSources.Add("chkDateS", BoDataType.dt_SHORT_TEXT, 1);
            FormularioSBO.DataSources.UserDataSources.Add("chkDateR", BoDataType.dt_SHORT_TEXT, 1);

            FormularioSBO.DataSources.UserDataSources.Add("chkMarca", BoDataType.dt_SHORT_TEXT, 1);
            FormularioSBO.DataSources.UserDataSources.Add("chkEstilo", BoDataType.dt_SHORT_TEXT, 1);
            FormularioSBO.DataSources.UserDataSources.Add("chkModelo", BoDataType.dt_SHORT_TEXT, 1);
            FormularioSBO.DataSources.UserDataSources.Add("chkStatus", BoDataType.dt_SHORT_TEXT, 1);

            EditTextFechaSIni = new SCG.SBOFramework.UI.EditTextSBO("txtFecIniS", true, "", "SStartDate", FormularioSBO);
            EditTextFechaSIni.AsignaBinding();
            EditTextFechaSFin = new SCG.SBOFramework.UI.EditTextSBO("txtFecFinS", true, "", "SEndDate", FormularioSBO);
            EditTextFechaSFin.AsignaBinding();

            EditTextFechaRIni = new SCG.SBOFramework.UI.EditTextSBO("txtFecIniR", true, "", "RStartDate", FormularioSBO);
            EditTextFechaRIni.AsignaBinding();
            EditTextFechaRFin = new SCG.SBOFramework.UI.EditTextSBO("txtFecFinR", true, "", "REndDate", FormularioSBO);
            EditTextFechaRFin.AsignaBinding();

            g_oChkDateSol = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkDateS").Specific;
            g_oChkDateRes = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkDateR").Specific;
            g_oChkMarca = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkMarca").Specific;
            g_oChkEstilo = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkEstilo").Specific;
            g_oChkModelo = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkModelo").Specific;
            g_oChkEstado = (SAPbouiCOM.CheckBox)FormularioSBO.Items.Item("chkStatus").Specific;

            FormularioSBO.DataSources.UserDataSources.Item("chkDateS").Value = "0";
            FormularioSBO.DataSources.UserDataSources.Item("chkDateR").Value = "0";
            FormularioSBO.DataSources.UserDataSources.Item("chkMarca").Value = "0";
            FormularioSBO.DataSources.UserDataSources.Item("chkEstilo").Value = "0";
            FormularioSBO.DataSources.UserDataSources.Item("chkModelo").Value = "0";
            FormularioSBO.DataSources.UserDataSources.Item("chkStatus").Value = "0";

            g_oChkDateSol.ValOn = "1";
            g_oChkDateSol.ValOff = "0";
            g_oChkDateSol.DataBind.SetBound(true, "", "chkDateS");

            g_oChkDateRes.ValOn = "1";
            g_oChkDateRes.ValOff = "0";
            g_oChkDateRes.DataBind.SetBound(true, "", "chkDateR");

            g_oChkMarca.ValOn = "1";
            g_oChkMarca.ValOff = "0";
            g_oChkMarca.DataBind.SetBound(true, "", "chkMarca");

            g_oChkEstilo.ValOn = "1";
            g_oChkEstilo.ValOff = "0";
            g_oChkEstilo.DataBind.SetBound(true, "", "chkEstilo");

            g_oChkModelo.ValOn = "1";
            g_oChkModelo.ValOff = "0";
            g_oChkModelo.DataBind.SetBound(true, "", "chkModelo");

            g_oChkEstado.ValOn = "1";
            g_oChkEstado.ValOff = "0";
            g_oChkEstado.DataBind.SetBound(true, "", "chkStatus");

            //CargarMatriz();
        }

        public void InicializaFormulario()
        {
            try
            {
                CargarFormulario(intBranch);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormulario(int p_intBranch)
        {
            ComboBox m_objCombo;
            Matrix m_objMatrix;
            Column m_objColumnEstado;

            try
            {
                FormularioSBO.Freeze(true);
                CargaSucursales();
                CargaMarcas();
                CargaEstados();
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void CargaSucursales()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbSucu");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            if (Utilitarios.ValidaSiDataTableExiste((SAPbouiCOM.Form)FormularioSBO, strDtConsulta))
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
            else
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);

            dtConsulta.Clear();
            dtConsulta.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_SUCURSALES]"));

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtConsulta.Rows.Count != 0)
            {
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtConsulta.GetValue("Code", i));
                    strNombre = Convert.ToString(dtConsulta.GetValue("Name", i));
                    cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }

            dtConsulta.Clear();
            var user = ApplicationSBO.Company.UserName;
            dtConsulta.ExecuteQuery(String.Format("select Branch from OUSR with (nolock) where USER_CODE  = '{0}'", user));

            if (dtConsulta.Rows.Count > 0)
            {
                foreach (SAPbouiCOM.ValidValue validValue in cboCombo.ValidValues)
                {
                    if (validValue.Value == dtConsulta.GetValue("Branch", 0).ToString().Trim())
                    {
                        cboCombo.Select(validValue.Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                    }
                }
            }
        }

        public void CargaMarcas()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbMarca");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            if (Utilitarios.ValidaSiDataTableExiste((SAPbouiCOM.Form)FormularioSBO, strDtConsulta))
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
            else
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);

            dtConsulta.Clear();
            dtConsulta.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_Marca]"));

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtConsulta.Rows.Count != 0)
            {
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtConsulta.GetValue("Code", i));
                    strNombre = Convert.ToString(dtConsulta.GetValue("Name", i));
                    cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }
        }

        public void CargaEstados()
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbStatus");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }
            
            cboCombo.ValidValues.Add(string.Empty, string.Empty);
            cboCombo.ValidValues.Add(((int)SolicitudEspecificos.EstadosSolicitudEspecíficos.SinResponder).ToString(), Resource.txtSolicitado);
            cboCombo.ValidValues.Add(((int)SolicitudEspecificos.EstadosSolicitudEspecíficos.Respondido).ToString(), Resource.txtRespondido);
            cboCombo.ValidValues.Add(((int)SolicitudEspecificos.EstadosSolicitudEspecíficos.Cancelado).ToString(), Resource.txtCancelado);
        }

        public void CargaEstilos(string p_strIdMarca)
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbEstilo");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            if (Utilitarios.ValidaSiDataTableExiste((SAPbouiCOM.Form)FormularioSBO, strDtConsulta))
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
            else
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);

            dtConsulta.Clear();
            dtConsulta.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_ESTILO] where U_Cod_Marc = '{0}' ", p_strIdMarca));

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtConsulta.Rows.Count != 0)
            {
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtConsulta.GetValue("Code", i));
                    strNombre = Convert.ToString(dtConsulta.GetValue("Name", i));
                    if (!string.IsNullOrEmpty(strCodigo))
                        cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }
        }

        public void CargaModelos(string p_strIdEstilo)
        {
            SAPbouiCOM.ComboBox cboCombo;
            SAPbouiCOM.Item oItem;
            Boolean blnExisteTabla = false;
            String strCodigo;
            String strNombre;

            oItem = FormularioSBO.Items.Item("cbModelo");
            cboCombo = (SAPbouiCOM.ComboBox)(oItem.Specific);

            if (cboCombo.ValidValues.Count > 0)
            {
                int CantidadValidValues = cboCombo.ValidValues.Count - 1;
                for (int i = 0; i <= CantidadValidValues; i++)
                {
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue);
                }
            }

            if (Utilitarios.ValidaSiDataTableExiste((SAPbouiCOM.Form)FormularioSBO, strDtConsulta))
                dtConsulta = FormularioSBO.DataSources.DataTables.Item(strDtConsulta);
            else
                dtConsulta = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);

            dtConsulta.Clear();
            dtConsulta.ExecuteQuery(String.Format("SELECT Code, Name FROM [@SCGD_MODELO] where U_Cod_Esti = '{0}' ", p_strIdEstilo));

            cboCombo.ValidValues.Add(string.Empty, string.Empty);

            if (dtConsulta.Rows.Count != 0)
            {
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    strCodigo = Convert.ToString(dtConsulta.GetValue("Code", i));
                    strNombre = Convert.ToString(dtConsulta.GetValue("Name", i));
                    if (!string.IsNullOrEmpty(strCodigo))
                        cboCombo.ValidValues.Add(strCodigo, strNombre);
                }
            }
        }

        #endregion

    }

}
