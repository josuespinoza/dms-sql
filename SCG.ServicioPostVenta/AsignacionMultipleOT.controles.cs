using System;
using SAPbouiCOM;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.ServicioPostVenta
{
    public partial class AsignacionMultipleOT : IFormularioSBO
    {

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public SAPbouiCOM.IForm FormularioSBO { get; set; }
        public Boolean ConfgUniMec { get; set; }
        public ICompany CompanySBO { get; private set; }
        public IApplication ApplicationSBO { get; private set; }
        public OrdenTrabajo OrdenTrabajo { get; set; }

        public int intBranch { get; set; }

        public static SAPbouiCOM.DataTable m_dtConsultaCombos;
        public static SAPbouiCOM.DataTable g_dtAsignaciones;
        public static SAPbouiCOM.DataTable g_dtControlColaborador;
        public MatrizServiciosAsignación g_objMatrizServAsignados;

        public string g_strdtAsignacion = "tAsignacion";
        public string g_strdtActividadesIngCtrlCol = "tControlCol";
        public string g_strmtxTareas = "mtxTareas";
        public const String g_strCboColab = "cboColabor";

        public const string g_strConsultaAsignacion =
            " select '' as sele, qut.ItemCode as codi, qut.Dscription as 'desc', qut.U_SCGD_EstAct as esta, fas.Name as fase, 'Asignaciones' as asig, qut.U_SCGD_ID as idac, oit.U_SCGD_Duracion as dura, fas.Code as cfas" +
            " FROM QUT1 as qut with (nolock) inner join OITM as oit with (nolock) on qut.ItemCode = oit.ItemCode inner join [@SCGD_FASEPRODUCCION] as fas with (nolock) on fas.Code = oit.U_SCGD_T_Fase  " +
            " where qut.DocEntry = {0} and oit.U_SCGD_TipoArticulo = '2' and qut.U_SCGD_Aprobado = '1' AND U_SCGD_EstAct IN (1,3 {1})  ";

        public const string g_strConsultaCtrColab = "select Code, LineId, U_Colab, U_Estad, U_IdAct, U_CodFas from [@SCGD_CTRLCOL] with (nolock) where code = '{0}'  ";

        public AsignacionMultipleOT(IApplication applicationSBO, ICompany companySBO, int p_intBranch)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                intBranch = p_intBranch;

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }

        }

        public void InicializarControles()
        {

            g_dtAsignaciones = FormularioSBO.DataSources.DataTables.Add(g_strdtAsignacion);
            g_dtAsignaciones.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("codi", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("esta", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("fase", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("asig", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("idac", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtAsignaciones.Columns.Add("cfas", BoFieldsType.ft_AlphaNumeric, 100);

            g_objMatrizServAsignados = new MatrizServiciosAsignación(g_strmtxTareas, FormularioSBO, g_strdtAsignacion);
            g_objMatrizServAsignados.CreaColumnas();
            g_objMatrizServAsignados.LigaColumnas();

            g_dtControlColaborador = FormularioSBO.DataSources.DataTables.Add(g_strdtActividadesIngCtrlCol);
            g_dtControlColaborador.Columns.Add("cola", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("esta", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("nofa", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("idac", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("cose", BoFieldsType.ft_AlphaNumeric, 100);
            g_dtControlColaborador.Columns.Add("cfas", BoFieldsType.ft_AlphaNumeric, 100);

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
                m_dtConsultaCombos = FormularioSBO.DataSources.DataTables.Add("dtConsulCbo");
                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboColabor").Specific;
                var query = string.Format(" select empID as Code,ISNULL(firstName,'')  + ' ' + isnull(middleName,'')  + ' ' + ISNULL(lastName,'') as Name from OHEM T0 where U_SCGD_T_Fase is not null AND Active = 'Y' AND (branch = {0} OR U_SCGD_MultiBranch = 'Y') ", p_intBranch);
               
                if (DMS_Connector.Company.AdminInfo.EnableBranches == SAPbobsCOM.BoYesNoEnum.tYES)
                    query = string.Format("{0} or BPLId = '{1}' ", query, p_intBranch);

                Utilitarios.CargaComboBox(query, "Code", "Name", m_dtConsultaCombos, ref m_objCombo, false,true);

                m_objMatrix = (Matrix)FormularioSBO.Items.Item(g_strmtxTareas).Specific;
                m_objColumnEstado = m_objMatrix.Columns.Item("Col_esta");

                Utilitarios.CargaComboBox(" SELECT Code, Name FROM [@SCGD_ESTADOS_ACTOT] order by Code ", "Code", "Name", m_dtConsultaCombos, ref m_objColumnEstado);
                FormularioSBO.Freeze(false);
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
    }

}
