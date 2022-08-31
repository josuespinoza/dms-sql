using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using SCG.ServicioPostVenta.CreaciónOTEspecial;
using ICompany = SAPbobsCOM.ICompany;
using Application = SAPbouiCOM.Application;
using ComboBox = SAPbouiCOM.ComboBox;

namespace SCG.ServicioPostVenta
{
    public partial class OrdenTrabajo : IFormularioSBO, IUsaMenu
    {
        #region ...Declaraciones...
        private static NumberFormatInfo n;
        public MatrizRepuestos g_objMatrizRepuestos;
        public MatrizSuministros g_objMatrizSuministros;
        public MatrizServicios g_objMatrizServicios;
        public MatrizServiciosExternos g_objMatrizServiciosExt;
        public MatrizGastos g_objMatrizGastos;
        public MatrizIngresos g_objMatrizIngresos;

        public static bool g_blnIniciarActividad;
        public static bool g_blnSuspenderActividad;
        public static bool g_blnFinalizarActividad;
        public static string g_StrNoOT;
        public static Matrix g_objMatrix;
        public static SAPbouiCOM.DataTable g_dtAdicionalesColaborador;
        public static SAPbouiCOM.DataTable g_dtConfiguracionSucursal;

        public ComboBoxSBO cboEstadoOrden;
        public ComboBoxSBO cboFasesProduccion;

        public ComboBoxSBO cboEstadoRepuestos;
        public ComboBoxSBO cboFasesRepuestos;
        public ComboBoxSBO cboEstadoServiciosExt;

        private UserDataSources uds_OrdenTrabajo;

        public static SAPbouiCOM.DataTable g_dtConsultaCombos;
        private SAPbouiCOM.DataTable g_dtEstadosOT;
        private SAPbouiCOM.DataTable g_dtBodxCC;


        private SAPbouiCOM.DataTable g_dtTemporalRepuestos;
        private SAPbouiCOM.DataTable g_dtTemporalServicios;
        private SAPbouiCOM.DataTable g_dtTemporalSuministros;
        private SAPbouiCOM.DataTable g_dtTemporalServiciosExternos;

        private SAPbouiCOM.DataTable g_dtRepuestos;
        private SAPbouiCOM.DataTable g_dtSuministros;
        private SAPbouiCOM.DataTable g_dtServicios;
        private SAPbouiCOM.DataTable g_dtServiciosExt;
        private SAPbouiCOM.DataTable g_dtGastos;
        private SAPbouiCOM.DataTable g_dtIngresos;
        private SAPbouiCOM.DataTable g_dtCostos;
        private SAPbouiCOM.DataTable g_dtConsulta;
        private SAPbouiCOM.DataTable g_dtAdmin;
        private SAPbouiCOM.DataTable g_dtConfSucursal;
        private SAPbouiCOM.DataTable g_dtEmpleado;
        private SAPbouiCOM.DataTable g_dtAprobacion;
        private SAPbouiCOM.DataTable g_dtTraRepuestos;
        private SAPbouiCOM.DataTable g_dtTraSuministros;
        private SAPbouiCOM.DataTable g_dtValidaOTEspecial;

        private SAPbouiCOM.DataTable g_dtConfGeneral;


        public static SAPbouiCOM.DataTable g_dtRepuestosSeleccionados;
        public static SAPbouiCOM.DataTable g_dtSuministrosSeleccionados;
        public static SAPbouiCOM.DataTable g_dtServiciosExternosSeleccionados;

        public AsignacionMultipleOT g_FormularioAsignacionMultiple;
        public OTEspecial g_FormularioOTEspecial;
        public AdicionalesOT g_FormularioAdicionalesOT;
        public TrackingRepuestos g_FormularioTracking;
        public TrackingSolEspecificos g_formularioTrackingSolEspecificos;
        public FinalizaActividad g_FormularioFinalizaAct;
        public DocumentoCompra g_FormularioDocumentoCompra;
        public RazonesSuspension g_FormularioRazonesSuspension;
        private BuscadorProveedores g_FormularioBuscadorProveedores;
        public GestorFormularios g_objGestorFormularios;
        public RealizarTraslado g_intRealizarTraslados;

        public int g_intEstadoCotizacion;

        public bool g_blnLineaEliminada;

        private bool g_blnProcesarSi;
        private bool g_blnProcesarNo;
        private bool g_blnTipoNoAdmitido;
        private bool g_blnTiempoStandar;

        public string strConfiguracion;


        private List<TransferenciasStock.LineasTransferenciasStock> g_listRepuestos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listSuministros = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listServiciosExternos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listEliminarRepuestos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listEliminarSuministros = new List<TransferenciasStock.LineasTransferenciasStock>();

        public const string g_strConsultaRepuestos =
            " select '' as sele, qut.U_SCGD_Traslad as tras, qut.U_SCGD_Aprobado as apro, 'Y' as perm, qut.ItemCode as code, Dscription as 'desc', Quantity as cant, WhsCode as alma, Price as prec, Currency as mone, " +
            " isnull(U_SCGD_Adic,0) as adic, isnull(U_SCGD_CPen,0) as pend, isnull(U_SCGD_CSol,0) as soli, isnull(U_SCGD_CRec,0) as reci, isnull(U_SCGD_CPDe,0) as pdev, " +
            " isnull(U_SCGD_CPTr,0) as ptra, isnull(U_SCGD_CPBo,0) as pbod, qut.U_SCGD_ID as idit, " +
            " CAST( CASE WHEN U_SCGD_Compra = 'Y' THEN 1 ELSE 0 END AS bit) as esco " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '1' and U_SCGD_OTHija = '2' and qut.U_SCGD_Aprobado <> 2 and isnull(qut.U_SCGD_ID,'') != '' ORDER BY LineNum ASC";


        public const string g_strConsultaSuministros =
            " select '' as sele, qut.U_SCGD_Traslad as tras, qut.U_SCGD_Aprobado as apro, 'Y' as perm, qut.ItemCode as code, Dscription as 'desc', Quantity as cant, WhsCode as alma, Price as prec, Currency as mone, " +
            " isnull(U_SCGD_Adic,0) as adic, isnull(U_SCGD_CPen,0) as pend, isnull(U_SCGD_CSol,0) as soli, isnull(U_SCGD_CRec,0) as reci, isnull(U_SCGD_CPDe,0) as pdev, " +
            " isnull(U_SCGD_CPTr,0) as ptra, isnull(U_SCGD_CPBo,0) as pbod, qut.U_SCGD_ID as idit " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '3' and U_SCGD_OTHija = '2' and qut.U_SCGD_Aprobado <> 2 and isnull(qut.U_SCGD_ID,'') != '' ORDER BY LineNum ASC ";


        public const string g_strConsultaServicios =
            " select '' as sele, qut.U_SCGD_Traslad as tras, qut.U_SCGD_Aprobado as apro, 'Y' as perm, qut.ItemCode as code, Dscription as 'desc', Quantity as cant, Price as prec, Currency as mone, qut.U_SCGD_EstAct as esta, " +
            " isnull(U_SCGD_DurSt,0) as dura, U_SCGD_FasePro as nofa, U_SCGD_Adic as adic, qut.U_SCGD_ID as idit " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '2' and U_SCGD_OTHija = '2' and (qut.U_SCGD_Aprobado = 1 or qut.U_SCGD_Aprobado = 3) and isnull(qut.U_SCGD_ID,'') != '' ORDER BY LineNum ASC ";

        public const string g_strConsultaServiciosExt =
            " select '' as sele, qut.U_SCGD_Traslad as tras, qut.U_SCGD_Aprobado as apro, 'Y' as perm, qut.ItemCode as code, Dscription as 'desc', Quantity as cant, Price as prec, Currency as mone, isnull(U_SCGD_Adic,0) as adic, isnull(U_SCGD_CPen,0) as pend, isnull(U_SCGD_CSol,0) as soli, " +
            " isnull(U_SCGD_CRec,0) as reci, isnull(U_SCGD_CPDe,0) as pdev, isnull(U_SCGD_CPTr,0) as ptra, isnull(U_SCGD_CPBo,0) as pbod, qut.U_SCGD_ID as idit, " +
            " CAST( CASE WHEN U_SCGD_Compra = 'Y' and U_SCGD_CPen > 0 THEN 1 ELSE 0 END AS bit) as esco " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '4' and U_SCGD_OTHija = '2' and (qut.U_SCGD_Aprobado = 1 or qut.U_SCGD_Aprobado = 3) and isnull(qut.U_SCGD_ID,'') != '' ORDER BY LineNum ASC ";

        public const string g_strConsultaGastos =
            " select qut.U_SCGD_Aprobado as apro, qut.ItemCode, Dscription, Quantity, Currency, Price, U_SCGD_Costo, qut.U_SCGD_Aprobado " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '11' and U_SCGD_OTHija = '2' and qut.U_SCGD_Aprobado <> 2 ORDER BY LineNum ASC ";

        public const string g_strConsultaIngresos =
            " select qut.U_SCGD_Aprobado as apro, qut.ItemCode, Dscription, Quantity, Currency, Price, U_SCGD_Costo, '', '', qut.U_SCGD_Aprobado " +
            " from QUT1  as qut with (nolock) where DocEntry = {0} and qut.U_SCGD_Sucur = '{1}' and qut.U_SCGD_TipArt = '12' and U_SCGD_OTHija = '2' and qut.U_SCGD_Aprobado <> 2 ORDER BY LineNum ASC ";

        public const string g_strConsultaAprobacion =
            " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap  with (nolock)  inner join [@SCGD_CONF_SUCURSAL] as csu  with (nolock)  on csu.DocEntry = cap.DocEntry " +
            " where csu.U_Sucurs  = '{1}' and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] where U_SCGD_Numero_OT = '{0}' and U_SCGD_idSucursal = '{1}')";

        public const string g_strConsultaBodegasCentroCosto =
            " select cnfs.U_Sucurs as Sucursal,U_CC as CentroCosto, U_Rep as Repuestos, U_Ser as Servicios, U_Sum as Suministros, U_SE as ServExt, U_Pro as Proceso " +
            " from [@SCGD_CONF_BODXCC] as bxcc  with (nolock)  inner join [@SCGD_CONF_SUCURSAL] as cnfs   with (nolock)  on bxcc.DocEntry = cnfs.DocEntry ";

        public const string g_strConsultaAdmin = " select U_ReduceCant, U_UsaAXEV, U_UsaLed, U_EspVehic, U_TiemEsta, U_UsaDimC from [@SCGD_ADMIN]  with (nolock)  ";

        public const string g_strConsultaConfSucursal =
                  " select U_DesSInv, U_Imp_Repuestos, U_Imp_Serv, U_Imp_ServExt, U_Imp_Suminis, U_Requis, U_UsaOfeVenta, U_UsaOrdVenta, U_SerOfC, U_SerOrC, U_USolOTEsp, U_ValReqPen, U_Entrega_Rep, U_FinOTCanSol, U_FOTAPen, U_TiempoEst_C, U_TiempoReal_C, U_SerInv, ISNULL(U_AsigUniMec,'N') U_AsigUniMec, U_CanOTSer, U_CanOTArAp, ISNULL(U_SolaUna,'N') as U_SolaUna,U_PerCanOT, U_PCanOTAct " +
                  " from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}' ";

        public const string g_strConsultaValidacionOTEspecial =
          " select count(1) as Count from QUT1 as qut with (nolock) " +
          " where DocEntry = '{0}' and qut.U_SCGD_TipArt   in ('1', '3', '4') and U_SCGD_OTHija = '2' and " +
          " ( (U_SCGD_Aprobado =1 AND (ISNULL(U_SCGD_CPen,0) > 0 or ISNULL(U_SCGD_CPDe,0) > 0 or ISNULL(U_SCGD_CPTr,0) > 0 or ISNULL(U_SCGD_CPBo,0) > 0)) " +
          "  OR (U_SCGD_Aprobado =1 AND U_SCGD_Compra ='Y' AND ISNULL(U_SCGD_CRec,0) <> Quantity ) ) ";

        public const string g_strConsultaRequisicionesPendientes = " select Count(DocEntry) from [@SCGD_REQUISICIONES]  with (nolock)  where U_SCGD_CodEst != '2' and  U_SCGD_CodEst != '3' and U_SCGD_NoOrden='{0}' and U_SCGD_IDSuc = '{1}' ";

        public const string g_strConsultaActividadessinMecanico = "SELECT count(ItemCode) FROM QUT1 with(nolock) WHERE U_SCGD_TipArt = 2 and U_SCGD_Aprobado = 1 and U_SCGD_EmpAsig is null and U_SCGD_NoOT = '{0}'";

        public const string g_strConsultaActividadesSinFinalizar = "SELECT COUNT(ItemCode) FROM QUT1 with(nolock) WHERE U_SCGD_TipArt = 2 AND U_SCGD_Aprobado = 1 AND U_SCGD_EstAct != 4 AND U_SCGD_NoOT = '{0}'";

        public const string g_strConsultaValidacionInterfazFord = "select U_SCGD_ServDpto,U_SCGD_TipoPago from OQUT with(nolock) where DocEntry = {0}";

        public const string g_strValidaCompraRecibidos = "Select Count(U_SCGD_CRec) From QUT1 as qu with(nolock) where qu.docentry = '{0}' and qu.U_SCGD_CRec <> 0 and qu.U_SCGD_Compra = 'Y' and qu.U_SCGD_Aprobado = '1'";



        public Boolean g_blnFinalizar;
        public Boolean g_blnCancelar;
        public static Boolean g_realizofiltroRepuestos;
        public static Boolean g_realizofiltroServicios;
        public static Boolean g_realizofiltroServiciosExter;
        public static Boolean g_realizofiltroSuministros;

        public const string g_strEstado_NoIniciado = "1";
        public const string g_strEstado_Iniciado = "2";
        public const string g_strEstado_Suspendido = "3";
        public const string g_strEstado_Finalizado = "4";

        public const int g_strRepuesto = 1;
        public const int g_strServicio = 2;
        public const int g_strSuministro = 3;
        public const int g_strServExterno = 4;
        public const int g_strPaquete = 5;
        public const int g_strNinguno = 0;
        public const int g_strOtrosGastos_Costos = 11;
        public const int g_strOtrosIngresos = 12;

        public TipoAdicional g_TipoDocCompra;

        public const string g_strdtRepuestos = "tRepuestos";
        public const string g_strdtRepuestosTemporal = "tRepuestosTemporal";
        public const string g_strdtSuministros = "tSuministros";
        public const string g_strdtSuministrosTemporal = "tSuministrosTemporal";
        public const string g_strdtServicios = "tServicios";
        public const string g_strdtServiciosTemporal = "tServiciosTemporal";
        public const string g_strdtServiciosExternos = "tServiciosExt";
        public const string g_strdtServiciosExternosTemporal = "tServiciosExtTemporal";

        public const string g_strdtRepuestosSeleccionados = "tRepueSel";
        public const string g_strdtAdcionalesServ = "tAdServ";
        public const string g_strdtSuministrosSeleccionados = "tSuminSel";
        public const string g_strdtServiciosExternosSeleccionados = "tServExSel";
        public const string g_strdtConsulta = "tConsulta";
        public const string g_strdtBodegasCentroCosto = "tBodxCC";
        //public const string g_strdtConsultaNombreEmpledo = "tConsultaOHEM";
        public const string g_strdtADMIN = "tAdmin";
        public const string g_strdtConfSucursal = "tConfSuc";
        public const string g_strdtPermisos = "tPermisos";
        public const string g_strdtEstadosOT = "tEstadosOT";
        public const string g_strdtAprobacion = "tAprob";
        public const string g_strdtEmpleado = "tEmplea";
        public const string g_strdtTraRepues = "tTraRe";
        public const string g_strdtTraSuminis = "tTraSu";
        public const string g_strdtValOTEspecial = "tValOT";

        public const string g_strdtConfGeneral = "tConfGen";

        public const string g_strColCantPendiente = "U_SCGD_CPen";
        public const string g_strColCantSolicitada = "U_SCGD_CSol";
        public const string g_strColCantRecibida = "U_SCGD_CRec";
        public const string g_strColCantPendienteDevolucion = "U_SCGD_CPDe";
        public const string g_strColCantPendienteTraslado = "U_SCGD_CPTr";
        public const string g_strColCantPendienteBodega = "U_SCGD_CPBo";

        public const string g_mtxProduccion = "mtxColab";
        public const string g_mtxRepuestos = "mtxRep";
        public const string g_mtxSuministros = "mtxSum";
        public const string g_mtxServicios = "mtxSer";
        public const string g_mtxServiciosExternos = "mtxServE";

        public List<ListaCantidadesAnteriores> g_lstCantidadesAnteriores = new List<ListaCantidadesAnteriores>();

        public String g_strCreaHjaCanPend = String.Empty;

        public struct ListaCantidadesAnteriores
        {
            public string ItemCode;
            public int LineNum;
            public double Cantidad;
        }

        #endregion

        #region ...Constructor...
        public OrdenTrabajo(Application applicationSBO, ICompany companySBO, ref AsignacionMultipleOT p_AsignacionMultipleConPermisos, ref RazonesSuspension p_RazonesSuspension, ref FinalizaActividad p_FinalizaActividad, ref TrackingRepuestos p_FormularioTracking, ref DocumentoCompra p_FormularioDocumentoCompra, ref BuscadorProveedores p_BuscadorProveedores, ref TrackingSolEspecificos p_FormularioTrackingSolEspe)
        {
            try
            {
                ApplicationSBO = applicationSBO;
                CompanySBO = companySBO;
                n = DIHelper.GetNumberFormatInfo(companySBO);
                m_AsignacionMultipleOT = p_AsignacionMultipleConPermisos;
                m_RazonesSuspension = p_RazonesSuspension;
                m_FinalizaActividad = p_FinalizaActividad;
                g_FormularioTracking = p_FormularioTracking;
                g_formularioTrackingSolEspecificos = p_FormularioTrackingSolEspe;
                g_FormularioDocumentoCompra = p_FormularioDocumentoCompra;
                g_FormularioBuscadorProveedores = p_BuscadorProveedores;
                g_objGestorFormularios = new GestorFormularios(ref applicationSBO);
            }
            catch (Exception ex)
            {
                Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public OrdenTrabajo()
        {

        }

        #endregion

        #region ...Enums...
        private enum CotizacionEstado
        {
            Creada = 1,
            Modificada = 2,
            SinCambio = 3
        }

        private enum EstadoActividades
        {
            NoIniciado = 1,
            Iniciado = 2,
            Suspendido = 3,
            Finalizado = 4
        }

        public enum TipoAdicional
        {
            Repuesto = 1,
            Servicio = 2,
            ServicioExterno = 4,
            Suministro = 3
        }

        public enum TipoArticulo
        {
            Repuesto = 1,
            Servicio = 2,
            Suministro = 3,
            ServExterno = 4,
            Paquete = 5,
            Ninguno = 0,
            OtrosGastos_Costos = 11,
            OtrosIngresos = 12
        }

        public enum EstadosTraslado
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4
        }

        public enum EstadosAprobacion
        {
            Aprobado = 1,
            NoAprobado = 2,
            FaltoAprobacion = 3
        }

        public enum ResultadoValidacionPorItem
        {
            SinCambio = 0,
            NoAprobar = 1,
            ModifCantiCotizacion = 2,
            PendTransf = 3,
            PendBodega = 4,
            Comprar = 5
        }

        public enum RealizarTraslado
        {
            No = 0,
            Si = 1
        }

        public enum EstadoOT
        {
            NoIniciada = 1,
            Iniciada = 2,
            Suspendida = 3,
            Finalizada = 4,
            Cancelada = 5,
            Cerrada = 6,
            Facturada = 7,
            Entregada = 8
        }

        public enum LineaAProcesar
        {
            Si = 1,
            No = 2
        }
        #endregion


        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }
        public string PasswordBD { get; set; }

        #endregion

        #region IFormularioSBO Members

        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public IForm FormularioSBO { get; set; }
        public bool Inicializado { get; set; }

        public ICompany CompanySBO { get; private set; }
        private AsignacionMultipleOT m_AsignacionMultipleOT { get; set; }
        private RazonesSuspension m_RazonesSuspension { get; set; }
        private FinalizaActividad m_FinalizaActividad { get; set; }
        public IApplication ApplicationSBO { get; private set; }

        public static string NoOT { get; set; }
        public static string CardCode { get; set; }
        public static string IdActividad { get; set; }

        public bool g_blnPaqueteNoAprobado;

        #endregion

        #region ...Metodos...

        public void InicializarControles()
        {

            if (FormularioSBO != null)
            {
                // DATA TABLE REPUESTOS Y TEMPORAL DE REPUESTOS
                g_dtRepuestos = FormularioSBO.DataSources.DataTables.Add(g_strdtRepuestos);
                g_dtRepuestos.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtRepuestos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestos.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestos.Columns.Add("esco", BoFieldsType.ft_AlphaNumeric, 100);

                g_objMatrizRepuestos = new MatrizRepuestos(g_mtxRepuestos, FormularioSBO, g_strdtRepuestos);
                g_objMatrizRepuestos.CreaColumnas();
                g_objMatrizRepuestos.LigaColumnas();

                g_dtTemporalRepuestos = FormularioSBO.DataSources.DataTables.Add(g_strdtRepuestosTemporal);
                g_dtTemporalRepuestos.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtTemporalRepuestos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalRepuestos.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalRepuestos.Columns.Add("esco", BoFieldsType.ft_AlphaNumeric, 100);





                // DATA TABLE SUMINISTROS Y TEMPORAL DE SUMINISTROS
                g_dtSuministros = FormularioSBO.DataSources.DataTables.Add(g_strdtSuministros);
                g_dtSuministros.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtSuministros.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministros.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtSuministros.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);

                g_objMatrizSuministros = new MatrizSuministros(g_mtxSuministros, FormularioSBO, g_strdtSuministros);
                g_objMatrizSuministros.CreaColumnas();
                g_objMatrizSuministros.LigaColumnas();


                g_dtTemporalSuministros = FormularioSBO.DataSources.DataTables.Add(g_strdtSuministrosTemporal);
                g_dtTemporalSuministros.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtTemporalSuministros.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalSuministros.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalSuministros.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);


                // DATA TABLE SERVICIOS Y TEMPORAL DE SERVICIOS
                g_dtServicios = FormularioSBO.DataSources.DataTables.Add(g_strdtServicios);
                g_dtServicios.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtServicios.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtServicios.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("esta", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("nofa", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServicios.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);

                g_objMatrizServicios = new MatrizServicios(g_mtxServicios, FormularioSBO, g_strdtServicios);
                g_objMatrizServicios.CreaColumnas();
                g_objMatrizServicios.LigaColumnas();



                g_dtTemporalServicios = FormularioSBO.DataSources.DataTables.Add(g_strdtServiciosTemporal);
                g_dtTemporalServicios.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServicios.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtTemporalServicios.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("esta", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("dura", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("nofa", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServicios.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);

                // DATA TABLE SERVICIOS EXTERNOS Y TEMPORAL DE SERVICIOS EXTERNOS
                g_dtServiciosExt = FormularioSBO.DataSources.DataTables.Add(g_strdtServiciosExternos);
                g_dtServiciosExt.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtServiciosExt.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExt.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExt.Columns.Add("esco", BoFieldsType.ft_AlphaNumeric, 100);
                
                g_objMatrizServiciosExt = new MatrizServiciosExternos(g_mtxServiciosExternos, FormularioSBO, g_strdtServiciosExternos);
                g_objMatrizServiciosExt.CreaColumnas();
                g_objMatrizServiciosExt.LigaColumnas();

                g_dtTemporalServiciosExternos = FormularioSBO.DataSources.DataTables.Add(g_strdtServiciosExternosTemporal);
                g_dtTemporalServiciosExternos.Columns.Add("sele", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("tras", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("perm", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtTemporalServiciosExternos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("adic", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("pend", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("soli", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("reci", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("pdev", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("ptra", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("pbod", BoFieldsType.ft_Quantity, 100);
                g_dtTemporalServiciosExternos.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtTemporalServiciosExternos.Columns.Add("esco", BoFieldsType.ft_AlphaNumeric, 100);
                
                // DATA TABLE GASTOS
                g_dtGastos = FormularioSBO.DataSources.DataTables.Add("tGastos");
                g_dtGastos.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtGastos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtGastos.Columns.Add("cost", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("fpro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtGastos.Columns.Add("asie", BoFieldsType.ft_AlphaNumeric, 100);


                g_objMatrizGastos = new MatrizGastos("mtxGas", FormularioSBO, "tGastos");
                g_objMatrizGastos.CreaColumnas();
                g_objMatrizGastos.LigaColumnas();

                // DATA TABLE INGRESOS
                g_dtIngresos = FormularioSBO.DataSources.DataTables.Add("tIngresos");
                g_dtIngresos.Columns.Add("apro", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtIngresos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtIngresos.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtIngresos.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtIngresos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtIngresos.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtIngresos.Columns.Add("cost", BoFieldsType.ft_AlphaNumeric, 100);


                g_objMatrizIngresos = new MatrizIngresos("mtxIng", FormularioSBO, "tIngresos");
                g_objMatrizIngresos.CreaColumnas();
                g_objMatrizIngresos.LigaColumnas();

                g_dtCostos = FormularioSBO.DataSources.DataTables.Add("tCostos");
                g_dtCostos.Columns.Add("idac", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtCostos.Columns.Add("sala", BoFieldsType.ft_Float, 100);

                g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Add(g_strdtEstadosOT);
                g_dtConsulta = FormularioSBO.DataSources.DataTables.Add(g_strdtConsulta);
                g_dtAdmin = FormularioSBO.DataSources.DataTables.Add(g_strdtADMIN);
                g_dtBodxCC = FormularioSBO.DataSources.DataTables.Add(g_strdtBodegasCentroCosto);
                g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Add(g_strdtConfSucursal);
                g_dtEmpleado = FormularioSBO.DataSources.DataTables.Add(g_strdtEmpleado);
                g_dtAprobacion = FormularioSBO.DataSources.DataTables.Add(g_strdtAprobacion);
                g_dtTraRepuestos = FormularioSBO.DataSources.DataTables.Add(g_strdtTraRepues);
                g_dtTraSuministros = FormularioSBO.DataSources.DataTables.Add(g_strdtTraSuminis);
                g_dtValidaOTEspecial = FormularioSBO.DataSources.DataTables.Add(g_strdtValOTEspecial);

                g_dtConfGeneral = FormularioSBO.DataSources.DataTables.Add(g_strdtConfGeneral);

                g_dtRepuestosSeleccionados = FormularioSBO.DataSources.DataTables.Add(g_strdtRepuestosSeleccionados);
                g_dtRepuestosSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestosSeleccionados.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestosSeleccionados.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtRepuestosSeleccionados.Columns.Add("pend", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestosSeleccionados.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestosSeleccionados.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtRepuestosSeleccionados.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtRepuestosSeleccionados.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministrosSeleccionados = FormularioSBO.DataSources.DataTables.Add(g_strdtSuministrosSeleccionados);
                g_dtSuministrosSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministrosSeleccionados.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministrosSeleccionados.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtSuministrosSeleccionados.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministrosSeleccionados.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtSuministrosSeleccionados.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtSuministrosSeleccionados.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExternosSeleccionados = FormularioSBO.DataSources.DataTables.Add(g_strdtServiciosExternosSeleccionados);
                g_dtServiciosExternosSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("cant", BoFieldsType.ft_Quantity, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("alma", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("prec", BoFieldsType.ft_Price, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtServiciosExternosSeleccionados.Columns.Add("idit", BoFieldsType.ft_AlphaNumeric, 100);


                g_dtAdicionalesColaborador = FormularioSBO.DataSources.DataTables.Add(g_strdtAdcionalesServ);
                g_dtAdicionalesColaborador.Columns.Add("IdAct", BoFieldsType.ft_AlphaNumeric, 100);
                g_dtAdicionalesColaborador.Columns.Add("IdCol", BoFieldsType.ft_AlphaNumeric, 100);

                SAPbouiCOM.Item itmBtnDetalle;
                itmBtnDetalle = FormularioSBO.Items.Item("btnRepD");
                itmBtnDetalle.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, 1, BoModeVisualBehavior.mvb_True);
                itmBtnDetalle.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, 14, BoModeVisualBehavior.mvb_False);

                SAPbouiCOM.Item itmBtnGeneral;
                itmBtnGeneral = FormularioSBO.Items.Item("btnRepG");
                itmBtnGeneral.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, 1, BoModeVisualBehavior.mvb_True);
                itmBtnGeneral.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, 14, BoModeVisualBehavior.mvb_False);

                SAPbouiCOM.Item itmKilometraje;
                itmKilometraje = FormularioSBO.Items.Item("txtVis");
                itmKilometraje.Enabled = true;

            }
        }

        public void InicializaFormulario()
        {
            Item m_objItem;
            ComboBox m_objCombo;
            Matrix m_objMatriz;
            Column m_objColumnEstado;


            Column m_objColumnTrasladado;
            Column m_objColumnAprobado;
            SAPbouiCOM.Column m_objColumnEstadoCompra;

            if (FormularioSBO != null)
            {
                FormularioSBO.Freeze(true);

                CargarFormulario();

                FormularioSBO.Mode = BoFormMode.fm_FIND_MODE;
                FormularioSBO.EnableMenu("1288", true);
                FormularioSBO.EnableMenu("1289", true);
                FormularioSBO.EnableMenu("1290", true);
                FormularioSBO.EnableMenu("1291", true);
                ManejadorEventoMenuEvent(false, true);
                g_dtConsultaCombos = FormularioSBO.DataSources.DataTables.Add("dtConsulCbo");


                m_objItem = FormularioSBO.Items.Item("Folder1");
                m_objItem.Click();

                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboSucu").Specific;

                Utilitarios.CargaComboBox(" select Code, Name from [@SCGD_SUCURSALES] with(nolock) order by Code ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objCombo, false);


                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboTipOT").Specific;

                Utilitarios.CargaComboBox(" SELECT Code, Name FROM [@SCGD_TIPO_ORDEN] with(nolock) order by Code  ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objCombo, false);


                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboFProS").Specific;

                Utilitarios.CargaComboBox(" SELECT Code, Name FROM [@SCGD_FASEPRODUCCION] with(nolock) order by Code  ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objCombo, true);


                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboEstR").Specific;

                Utilitarios.CargaComboBox(" Select Code,Name from [@SCGD_ESTADOS_REPOT] with(nolock) order by code  ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objCombo, true);

                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboEstSE").Specific;

                Utilitarios.CargaComboBox(" Select Code, Name from [@SCGD_ESTADOS_REPOT] with(nolock) order by code  ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objCombo, true);

                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                m_objMatriz.SelectionMode = BoMatrixSelect.ms_Auto;
                m_objColumnEstado = m_objMatriz.Columns.Item("Col_est");

                Utilitarios.CargaComboBox(" select Code, Name from [@SCGD_ESTADOS_ACTOT]  with(nolock) order by Code ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objColumnEstado);



                //Servicios
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxSer").Specific;
                m_objMatriz.SelectionMode = BoMatrixSelect.ms_Auto;
                m_objColumnEstado = m_objMatriz.Columns.Item("Col_esta");
                Utilitarios.CargaComboBox(" SELECT Code, Name FROM [@SCGD_ESTADOS_ACTOT]  with(nolock) order by Code ",
                    "Code", "Name", g_dtConsultaCombos, ref m_objColumnEstado);

                m_objColumnTrasladado = m_objMatriz.Columns.Item("Col_tras");
                Utilitarios.CargaComboTraslado(ref m_objColumnTrasladado);

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                //Repuestos
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxRep").Specific;

                m_objColumnTrasladado = m_objMatriz.Columns.Item("Col_tras");
                Utilitarios.CargaComboTraslado(ref m_objColumnTrasladado);

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                m_objColumnEstadoCompra = m_objMatriz.Columns.Item("Col_esco");
                Utilitarios.CargaComboEstadoCompra(ref m_objColumnEstadoCompra);

                //Suministros
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxSum").Specific;

                m_objColumnTrasladado = m_objMatriz.Columns.Item("Col_tras");
                Utilitarios.CargaComboTraslado(ref m_objColumnTrasladado);

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                //Servicios externos
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxServE").Specific;

                m_objColumnTrasladado = m_objMatriz.Columns.Item("Col_tras");
                Utilitarios.CargaComboTraslado(ref m_objColumnTrasladado);

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                m_objColumnEstadoCompra = m_objMatriz.Columns.Item("Col_esco");
                Utilitarios.CargaComboEstadoCompra(ref m_objColumnEstadoCompra);

                //Gastos
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxGas").Specific;

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                //Ingresos
                m_objMatriz = (Matrix)FormularioSBO.Items.Item("mtxIng").Specific;

                m_objColumnAprobado = m_objMatriz.Columns.Item("Col_apro");
                Utilitarios.CargaComboAprobado(ref m_objColumnAprobado);

                m_objCombo = (ComboBox)FormularioSBO.Items.Item("136").Specific;
                Utilitarios.CargaComboProduccion(ref m_objCombo);


                m_objCombo = (ComboBox)FormularioSBO.Items.Item("cboEstSu").Specific;
                Utilitarios.CargaComboBox(" Select Code, Name from [@SCGD_ESTADOS_REPOT] with(nolock) order by code  ",
                   "Code", "Name", g_dtConsultaCombos, ref m_objCombo, true);

                g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item(g_strdtEstadosOT);
                g_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ");

                g_dtBodxCC = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                g_dtBodxCC.ExecuteQuery(g_strConsultaBodegasCentroCosto);

                g_dtAdmin = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);
                g_dtAdmin.ExecuteQuery(g_strConsultaAdmin);

                ManejaPermisosTab();

                m_objItem = FormularioSBO.Items.Item("mtxRep");
                m_objMatriz = (SAPbouiCOM.Matrix)m_objItem.Specific;

                for (int i = 0; i <= m_objMatriz.Columns.Count - 1; i++)
                {
                    string m_strNombre = m_objMatriz.Columns.Item(i).UniqueID.Trim();
                    if (m_strNombre == "Col_Perm")
                    {
                        m_objMatriz.Columns.Item(i).Visible = false;
                    }
                }

                SAPbouiCOM.Item itmNumeroVisita;
                itmNumeroVisita = FormularioSBO.Items.Item("txtVis");
                itmNumeroVisita.Enabled = false;

                SAPbouiCOM.Item itmEstadoVisita;
                itmEstadoVisita = FormularioSBO.Items.Item("txtEstVi");
                itmEstadoVisita.Enabled = false;

                SAPbouiCOM.Item itmNumeroCono;
                itmNumeroCono = FormularioSBO.Items.Item("txtNoCon");
                itmNumeroCono.Enabled = false;

                SAPbouiCOM.Item itmcboTipoOrden;
                itmcboTipoOrden = FormularioSBO.Items.Item("cboTipOT");
                itmcboTipoOrden.Enabled = false;

                SAPbouiCOM.Item itmKilometraje;
                itmKilometraje = FormularioSBO.Items.Item("txtkm");
                itmKilometraje.Enabled = false;
                FormularioSBO.Freeze(false);
            }
        }

        private void CargarFormulario()
        {
            try
            {
                uds_OrdenTrabajo = FormularioSBO.DataSources.UserDataSources;
                uds_OrdenTrabajo.Add("estRe", BoDataType.dt_LONG_TEXT, 100);
                uds_OrdenTrabajo.Add("fasAc", BoDataType.dt_LONG_TEXT, 100);
                uds_OrdenTrabajo.Add("estSE", BoDataType.dt_LONG_TEXT, 100);

                cboEstadoRepuestos = new ComboBoxSBO("cboEstR", FormularioSBO, true, "", "estRe");
                cboFasesRepuestos = new ComboBoxSBO("cboFProS", FormularioSBO, true, "", "fasAc");
                cboEstadoServiciosExt = new ComboBoxSBO("cboEstSE", FormularioSBO, true, "", "estSE");


                cboEstadoRepuestos.AsignaBinding();
                cboFasesRepuestos.AsignaBinding();

                cboEstadoServiciosExt.AsignaBinding();

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void CargarOT(string strNumOT)
        {
            SAPbouiCOM.Conditions oConditions;
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Matrix m_objMatrix;

            try
            {
                if (FormularioSBO != null)
                {
                    FormularioSBO.Freeze(true);
                    oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    oCondition = oConditions.Add();

                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = strNumOT;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").Query(oConditions);
                    ManejadorEventoFormDataLoad((SAPbouiCOM.Form)FormularioSBO);
                    m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                    m_objMatrix.LoadFromDataSource();
                    FormularioSBO.Refresh();
                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                    ValidaModoVistaOT((SAPbouiCOM.Form)FormularioSBO, true);
                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void ValidaModoVistaOT(SAPbouiCOM.Form p_oForm,  bool p_bolModoVista)
        {
            ComboBox objCombo;
            String query;
            String strBloqueoOT;
            String strDocEntry;
            try
            {
                if (p_oForm != null)
                {
                    query = DMS_Connector.Queries.GetStrSpecificQuery("strBloqueoOTCotizacion");
                    if (DMS_Connector.Helpers.PermisosMenu("SCGD_OVV"))
                    {
                        if (p_oForm.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_EstO", 0).Trim() != g_strEstado_Finalizado || p_bolModoVista== true)
                        {
                            strDocEntry = p_oForm.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                            if (!string.IsNullOrEmpty(strDocEntry))
                            {
                                strBloqueoOT = DMS_Connector.Helpers.EjecutarConsulta( String.Format(query, strDocEntry));
                                if (strBloqueoOT=="Y")
                                {
                                    p_oForm.Mode = BoFormMode.fm_VIEW_MODE;
                                    objCombo = (ComboBox)p_oForm.Items.Item("136").Specific;
                                    objCombo.Item.Enabled = true;
                                } 
                            }
                            
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        /// <summary>
        /// Metodo que recarga la matriz de actividades
        /// </summary>
        /// <param name="strNumOT">Número de OT</param>
        public void recargarActividades(string strNumOT, IApplication applicationSbo)
        {
            SAPbouiCOM.Conditions oConditions;
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Matrix oMatrix;

            try
            {
                if (FormularioSBO == null) FormularioSBO = applicationSbo.Forms.Item("SCGD_ORDT");

                oConditions = (SAPbouiCOM.Conditions)applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                oCondition = oConditions.Add();

                oCondition.Alias = "Code";
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCondition.CondVal = strNumOT;

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").Query(oConditions);
                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Query(oConditions);
                oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                oMatrix.LoadFromDataSource();
                CargaMatrices(false, true, false, false, false, false);

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        public void RecargarFormulario(string strNumOT)
        {
            SAPbouiCOM.Conditions oConditions;
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Matrix m_objMatrix;

            try
            {
                if (FormularioSBO != null)
                {
                    oConditions = (SAPbouiCOM.Conditions)ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    oCondition = oConditions.Add();

                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = strNumOT;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_IMG_OT").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").Query(oConditions);
                    CargaMatrices(true, true, true, true, true, true);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormularioAsignacionMultiple(SAPbouiCOM.ItemEvent pval)
        {
            int innDocEntry;
            try
            {
                m_AsignacionMultipleOT.intBranch = Convert.ToInt32(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim());
                m_AsignacionMultipleOT.OrdenTrabajo = this;
                NoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                innDocEntry = int.Parse(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Docentry", 0).Trim());

                if (!g_objGestorFormularios.FormularioAbierto(m_AsignacionMultipleOT, true))
                {
                    m_AsignacionMultipleOT.FormularioSBO = g_objGestorFormularios.CargarFormulario(m_AsignacionMultipleOT);
                    m_AsignacionMultipleOT.ManejadorEventoFormDataLoad(g_dtConfSucursal.GetValue("U_AsigUniMec", 0).ToString().Trim(), innDocEntry);
                }

            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormularioRazonesSuspension(ItemEvent pval, Boolean p_suspendeOT = false)
        {
            SAPbouiCOM.Matrix m_objMatrix;
            string m_strIdActividad = string.Empty;

            try
            {
                m_RazonesSuspension.OrdenTrabajo = this;
                m_objMatrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                g_blnFinalizarActividad = true;
                m_objMatrix.FlushToDataSource();

                for (int i = 1; i <= m_objMatrix.RowCount; i++)
                {
                    if (m_objMatrix.IsRowSelected(i) && FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_Estad", i - 1).ToString().Trim() == g_strEstado_Iniciado)
                    {
                        IdActividad = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_IdAct", i - 1).Trim();
                        RazonesSuspension.strFechaIni = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_DFIni", i - 1).Trim();
                        RazonesSuspension.strHoraIni = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CTRLCOL").GetValue("U_HFIni", i - 1).Trim();
                        break;
                    }
                }

                if (!g_objGestorFormularios.FormularioAbierto(m_RazonesSuspension, true))
                {
                    m_RazonesSuspension.FormularioSBO = g_objGestorFormularios.CargarFormulario(m_RazonesSuspension);
                    m_RazonesSuspension.ManejadorEventoFormLoad(p_suspendeOT);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormularioOTEspecial()
        {
            string strDocEntry, strSucursal = string.Empty;
            SAPbouiCOM.DataTable m_dtConfigSucursal;

            string strSOTESP;

            try
            {
                g_FormularioOTEspecial = new OTEspecial(ApplicationSBO, CompanySBO);
                g_FormularioOTEspecial.NombreXml = System.Environment.CurrentDirectory + Resource.frmOTEspecial;
                g_FormularioOTEspecial.FormType = "SCGD_OTES";

                NoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                strSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                if (g_objGestorFormularios.FormularioAbierto(g_FormularioOTEspecial, true) == false)
                {
                    m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                    strSOTESP = m_dtConfigSucursal.GetValue("U_USolOTEsp", 0).ToString().Trim();

                    OTEspecial.g_strNOOT = NoOT;
                    OTEspecial.g_strDocE = strDocEntry;
                    if (!string.IsNullOrEmpty(strSOTESP))
                    {
                        if (strSOTESP == "Y")
                        {
                            OTEspecial.g_SOOTEsp = true;
                        }
                        else
                        {
                            OTEspecial.g_SOOTEsp = false;
                        }
                    }
                    if (!g_objGestorFormularios.FormularioAbierto(g_FormularioOTEspecial, true))
                    {
                        g_FormularioOTEspecial.FormularioSBO = g_objGestorFormularios.CargarFormulario(g_FormularioOTEspecial);
                        g_FormularioOTEspecial.ManejadorEventoFormDataLoad(strSucursal);
                    }

                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargarFormularioAdicionalesOT(SAPbouiCOM.ItemEvent pval, int p_intTipoFormulario)
        {
            try
            {
                g_FormularioAdicionalesOT = new AdicionalesOT(ApplicationSBO, CompanySBO);
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                g_FormularioAdicionalesOT.NombreXml = Environment.CurrentDirectory + Resource.frmBuscadorAdicionales;

                g_FormularioAdicionalesOT.FormType = "SCGD_ADIC";

                NoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                CardCode = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_CodCli", 0).Trim();

                if (g_objGestorFormularios.FormularioAbierto(g_FormularioAdicionalesOT, true) == false)
                {
                    g_FormularioAdicionalesOT.FormularioSBO = g_objGestorFormularios.CargarFormulario(g_FormularioAdicionalesOT);

                    AdicionalesOT.strCodCliente = CardCode;
                    AdicionalesOT.strNoOT = NoOT;

                    g_dtConsulta.ExecuteQuery(string.Format("SELECT DocCur FROM OQUT with (nolock) where docentry = {0}", FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim()));
                    if (g_dtConsulta.Rows.Count > 0 && !String.IsNullOrEmpty(g_dtConsulta.GetValue("DocCur", 0).ToString().Trim()))
                    {
                        AdicionalesOT.strDocCur = g_dtConsulta.GetValue("DocCur", 0).ToString().Trim();
                    }

                    g_FormularioAdicionalesOT.ManejadorEventoFormDataLoad(pval, p_intTipoFormulario);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        private void CargaFormularioTracking(string m_strItemCode, string m_strID)
        {
            var strID = string.Empty;
            var strItemCode = string.Empty;
            var strObjectType = string.Empty;
            try
            {
                NoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).ToString().Trim();

                if (!g_objGestorFormularios.FormularioAbierto(g_FormularioTracking, true))
                {
                    g_FormularioTracking.FormularioSBO = g_objGestorFormularios.CargarFormulario(g_FormularioTracking);
                    TrackingRepuestos.strCode = m_strItemCode;
                    TrackingRepuestos.strID = m_strID;
                    TrackingRepuestos.strNoOT = NoOT;
                    SAPbouiCOM.DataTable dtTracking;
                    if (FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").Size > 0)
                    {

                        dtTracking = g_FormularioTracking.FormularioSBO.DataSources.DataTables.Item("dtTracking");
                        for (int i = 0; i <= FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").Size - 1; i++)
                        {
                            if (String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_DocEntry", i)))
                                continue;

                            strID = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_ID", i).Trim();
                            strItemCode = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_ItemCode", i).Trim();
                            if (strID != m_strID || strItemCode != m_strItemCode)
                                continue;
                            dtTracking.Rows.Add(1);
                            var index = dtTracking.Rows.Count - 1;

                            dtTracking.SetValue("Prov", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_CardName", i).Trim());
                            dtTracking.SetValue("FeSo", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_FechaDoc", i).Trim());
                            dtTracking.SetValue("DocE", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_DocEntry", i).Trim());
                            dtTracking.SetValue("DocN", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_DocNum", i).Trim());
                            dtTracking.SetValue("Obse", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_Observ", i).Trim());
                            dtTracking.SetValue("CanEn", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_CanRec", i).Trim());
                            dtTracking.SetValue("CanSo", index, FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_CanSol", i).Trim());

                            if (!string.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_TipoDoc", i)))
                            {
                                strObjectType = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_TRACKXOT").GetValue("U_TipoDoc", i).Trim();
                                dtTracking.SetValue("TDoc", index, strObjectType);
                                switch (strObjectType)
                                {
                                    case "540000006":// Utilitarios.TipoDocumentoMarketing.OfertaCompra.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.txtOfertaCompra);
                                        break;
                                    case "22":// Utilitarios.TipoDocumentoMarketing.OrdenCompra.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.txtOrdenCompra);
                                        break;
                                    case "20":// Utilitarios.TipoDocumentoMarketing.EntradaMercancia.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.txtEntradaMercancia);
                                        break;
                                    case "18":// Utilitarios.TipoDocumentoMarketing.FacturaProveedor.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.txtFacturaProveedor);
                                        break;
                                    case "19":// Utilitarios.TipoDocumentoMarketing.NotaCredito.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.txtNotaCredito);
                                        break;
                                    case "21":// Utilitarios.TipoDocumentoMarketing.DevolucionMercancia.ToString()
                                        dtTracking.SetValue("TDocD", index, Resource.Devolucion);
                                        break;
                                }
                            }
                        }
                    }

                    g_FormularioTracking.ManejadorEventoFormDataLoad();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargaFormularioFinalizaAct(string m_strCodeEmp, string m_strID, int p_IdLinea, bool p_ConfEstandar, string p_strCosto, string p_strFechaIni, string p_strHoraIni, string p_strCodFase, string p_strNoFase, string p_strEstado, int p_lineaclick)
        {
            string m_strOT;
            string m_strDocEntry;
            string strAsigUniMec;
            Matrix o_Matrix;
            Column o_ColumnaCol;
            Column o_ColumnaDesc;


            try
            {
                o_Matrix = (Matrix)FormularioSBO.Items.Item("mtxColab").Specific;
                o_ColumnaCol = o_Matrix.Columns.Item("Col_col");
                o_ColumnaDesc = o_Matrix.Columns.Item("Col_IdAct");
                m_strOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();
                m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();
                strAsigUniMec = g_dtConfSucursal.GetValue("U_SolaUna", 0).ToString().Trim();
                if (p_strHoraIni.Length == 3) p_strHoraIni = string.Format("0{0}", p_strHoraIni);
                m_FinalizaActividad.OrdenTrabajo = this;
                m_FinalizaActividad.idSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);

                if (!g_objGestorFormularios.FormularioAbierto(m_FinalizaActividad, true))
                {
                    m_FinalizaActividad.FormularioSBO = g_objGestorFormularios.CargarFormulario(m_FinalizaActividad);
                    m_FinalizaActividad.ManejadorEventoFormDataLoad();
                }
                    
                m_FinalizaActividad.FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                FinalizaActividad.strCodeEmp = m_strCodeEmp;
                FinalizaActividad.strIDAct = m_strID;
                FinalizaActividad.strNoOT = m_strOT;
                FinalizaActividad.strDocEntry = m_strDocEntry;
                FinalizaActividad.idlinea = p_IdLinea;
                FinalizaActividad.ConfEstandar = p_ConfEstandar;
                FinalizaActividad.strCosto = p_strCosto;
                FinalizaActividad.strFechaIni = p_strFechaIni;
                FinalizaActividad.strHoraIni = p_strHoraIni;
                FinalizaActividad.AsigUniMec = strAsigUniMec;
                FinalizaActividad.CodFase = p_strCodFase;
                FinalizaActividad.NoFase = p_strNoFase;
                FinalizaActividad.EstadoAct = p_strEstado;
                m_FinalizaActividad.AsignaValoresTxtFinalizaAct(
                    ((ComboBox) o_ColumnaCol.Cells.Item(p_lineaclick).Specific).Selected.Description,
                    ((ComboBox) o_ColumnaDesc.Cells.Item(p_lineaclick).Specific).Selected.Description);
            }
            catch (Exception)
            {

                throw;
            }
        }

        



        private void CargarFormularioDocumentoCompra(ItemEvent pVal, ref bool bubbleEvent, TipoAdicional p_TipoDocCompra)
        {
            SAPbouiCOM.DataTable m_dtConfigSucursal;
            string m_strOferta = string.Empty;
            string m_strOrden = string.Empty;
            string m_strSerieOrden = string.Empty;
            string m_strSerieOferta = string.Empty;
            string m_strDocEntry = string.Empty;
            SAPbouiCOM.DataTable m_dtItemsSeleccionados;
            string m_strMatriz = string.Empty;
            string m_strDT = string.Empty;
            string m_strIdSucursal = string.Empty;
            string m_strUsaDimension = string.Empty;
            SAPbouiCOM.DataTable m_dtConfigGenerales;

            try
            {
                g_TipoDocCompra = p_TipoDocCompra;
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                g_FormularioDocumentoCompra.NombreXml = Environment.CurrentDirectory + Resource.frmDocumentoCompra;

                g_FormularioDocumentoCompra.FormType = "SCGD_DOCC";

                m_dtConfigSucursal = FormularioSBO.DataSources.DataTables.Item(g_strdtConfSucursal);
                m_dtConfigGenerales = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);


                switch (p_TipoDocCompra)
                {
                    case TipoAdicional.Repuesto:
                        m_dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestosSeleccionados);
                        m_strMatriz = g_mtxRepuestos;
                        m_strDT = g_strdtRepuestos;
                        break;
                    case TipoAdicional.Suministro:
                        m_dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtSuministrosSeleccionados);
                        m_strMatriz = g_mtxSuministros;
                        m_strDT = g_strdtSuministros;
                        break;
                    case TipoAdicional.ServicioExterno:
                        m_dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtServiciosExternosSeleccionados);
                        m_strMatriz = g_mtxServiciosExternos;
                        m_strDT = g_strdtServiciosExternos;
                        break;
                    default:
                        m_dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(g_strdtRepuestosSeleccionados);
                        m_strMatriz = g_mtxRepuestos;
                        m_strDT = g_strdtRepuestos;
                        break;
                }

                m_strOferta = m_dtConfigSucursal.GetValue("U_UsaOfeVenta", 0).ToString().Trim();
                m_strOrden = m_dtConfigSucursal.GetValue("U_UsaOrdVenta", 0).ToString().Trim();
                m_strSerieOrden = m_dtConfigSucursal.GetValue("U_SerOrC", 0).ToString().Trim();
                m_strSerieOferta = m_dtConfigSucursal.GetValue("U_SerOfC", 0).ToString().Trim();
                m_strUsaDimension = m_dtConfigGenerales.GetValue("U_UsaDimC", 0).ToString().Trim();
                m_strDocEntry = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_DocEntry", 0).Trim();

                cargarDTSeleccionados(pVal, ref bubbleEvent, p_TipoDocCompra);

                if (!g_objGestorFormularios.FormularioAbierto(g_FormularioDocumentoCompra, true))
                {
                    g_FormularioDocumentoCompra.FormularioSBO = g_objGestorFormularios.CargarFormulario(g_FormularioDocumentoCompra);

                    DocumentoCompra.g_strOfertaCompra = m_strOferta;
                    DocumentoCompra.g_strOrdenCompra = m_strOrden;
                    DocumentoCompra.g_strSerieOrden = m_strSerieOrden;
                    DocumentoCompra.g_strSerieOferta = m_strSerieOferta;
                    DocumentoCompra.g_strDocEntry = m_strDocEntry;
                    DocumentoCompra.g_strUsaDimension = m_strUsaDimension;
                    g_FormularioDocumentoCompra.g_objOrdentrabajo = this;
                    g_FormularioDocumentoCompra.g_FormularioBuscadorProveedores = g_FormularioBuscadorProveedores;
                    m_strIdSucursal = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_Sucu", 0).Trim();
                    switch (FormularioSBO.PaneLevel)
                    {
                        case 3:
                            g_FormularioDocumentoCompra.ManejadorEventoFormDataLoad(pVal, bubbleEvent, ref m_dtItemsSeleccionados, ref m_strIdSucursal, DocumentoCompra.TipoAdicional.Repuesto, FormularioSBO);
                            break;
                        case 4:
                            g_FormularioDocumentoCompra.ManejadorEventoFormDataLoad(pVal, bubbleEvent, ref m_dtItemsSeleccionados, ref m_strIdSucursal, DocumentoCompra.TipoAdicional.Servicio, FormularioSBO);
                            break;
                        case 5:
                            g_FormularioDocumentoCompra.ManejadorEventoFormDataLoad(pVal, bubbleEvent, ref m_dtItemsSeleccionados, ref m_strIdSucursal, DocumentoCompra.TipoAdicional.ServicioExterno, FormularioSBO);
                            break;
                        case 6:
                            g_FormularioDocumentoCompra.ManejadorEventoFormDataLoad(pVal, bubbleEvent, ref m_dtItemsSeleccionados, ref m_strIdSucursal, DocumentoCompra.TipoAdicional.Gastos, FormularioSBO);
                            break;
                        case 7:
                            g_FormularioDocumentoCompra.ManejadorEventoFormDataLoad(pVal, bubbleEvent, ref m_dtItemsSeleccionados, ref m_strIdSucursal, DocumentoCompra.TipoAdicional.Suministro, FormularioSBO);
                            break;
                    }

                }


                FormularioSBO.Mode = BoFormMode.fm_OK_MODE;


            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

         private void CargarFormularioTrackingSolEspecificos()
         {
             string NoOT = string.Empty;

            try
            {
                g_formularioTrackingSolEspecificos = new TrackingSolEspecificos(ApplicationSBO, CompanySBO);
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                CultureInfo cultureInfo = Resource.Culture;
                DMS_Connector.Helpers.SetCulture(ref currentUiCulture, ref cultureInfo);
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                Resource.Culture = cultureInfo;
                g_formularioTrackingSolEspecificos.NombreXml = Environment.CurrentDirectory + Resource.frmTrackingSolEspecificos;

                g_formularioTrackingSolEspecificos.FormType = "SCGD_TRASOL";

                NoOT = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_OT").GetValue("U_NoOT", 0).Trim();

                if (g_objGestorFormularios.FormularioAbierto(g_formularioTrackingSolEspecificos, true) == false)
                {
                    g_formularioTrackingSolEspecificos.FormularioSBO = g_objGestorFormularios.CargarFormulario(g_formularioTrackingSolEspecificos);
                    g_formularioTrackingSolEspecificos.ManejadorEventoFormDataLoad(ref NoOT);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void cargarDTSeleccionados(ItemEvent pVal, ref bool bubbleEvent, TipoAdicional p_TipoDocCompra)
        {
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.DataTable dtItemsSeleccionados;
            SAPbouiCOM.DataTable dtItems;
            SAPbouiCOM.Item m_objItem;
            SAPbouiCOM.ComboBox m_objCombo;
            string m_strValorCombo = string.Empty;
            string m_strItems = string.Empty;
            string m_strItemsSeleccionados = string.Empty;

            string m_strMatriz = string.Empty;
            string m_strCode = string.Empty;
            bool m_blnEsServExterno = false;

            switch (p_TipoDocCompra)
            {
                case TipoAdicional.Repuesto:
                    m_strMatriz = g_mtxRepuestos;
                    m_strItemsSeleccionados = g_strdtRepuestosSeleccionados;
                    m_objItem = FormularioSBO.Items.Item("cboEstR");
                    m_objCombo = (ComboBox)m_objItem.Specific;
                    m_strValorCombo = m_objCombo.Value.Trim();
                    if (string.IsNullOrEmpty(m_strValorCombo))
                        m_strItems = g_strdtRepuestos;
                    else
                        m_strItems = g_strdtRepuestosTemporal;
                    break;
                    break;
                case TipoAdicional.Suministro:
                    m_strMatriz = g_mtxSuministros;
                    m_strItemsSeleccionados = g_strdtSuministrosSeleccionados;
                    m_objItem = FormularioSBO.Items.Item("cboEstSu");
                    m_objCombo = (ComboBox)m_objItem.Specific;
                    m_strValorCombo = m_objCombo.Value.Trim();
                    if (string.IsNullOrEmpty(m_strValorCombo))
                        m_strItems = g_strdtSuministros;
                    else
                        m_strItems = g_strdtSuministrosTemporal;
                    break;
                    break;
                case TipoAdicional.ServicioExterno:
                    m_strMatriz = g_mtxServiciosExternos;
                    m_strItemsSeleccionados = g_strdtServiciosExternosSeleccionados;
                    m_blnEsServExterno = true;
                    m_objItem = FormularioSBO.Items.Item("cboEstSE");
                    m_objCombo = (ComboBox)m_objItem.Specific;
                    m_strValorCombo = m_objCombo.Value.Trim();
                    if (string.IsNullOrEmpty(m_strValorCombo))
                        m_strItems = g_strdtServiciosExternos;
                    else
                        m_strItems = g_strdtServiciosExternosTemporal;
                    break;
            }
            oMatrix = (SAPbouiCOM.Matrix)FormularioSBO.Items.Item(m_strMatriz).Specific;
            dtItems = FormularioSBO.DataSources.DataTables.Item(m_strItems);
            dtItemsSeleccionados = FormularioSBO.DataSources.DataTables.Item(m_strItemsSeleccionados);
            dtItemsSeleccionados.Rows.Clear();
            SeleccionarAdicionales(ref dtItems, ref dtItemsSeleccionados, m_blnEsServExterno, oMatrix);
        }

        #endregion

        /// <summary>
        /// Maneja el estado de la columna precio en OT para ser editable o no según configuración sucursal
        /// </summary>
        /// <param name="p_strPreCamSe"></param> Configuración de Cambio de Precio en OT
        /// <param name="p_formularioSbo"></param> Formulario OT
        /// <param name="p_EstadoOt"></param> Estado de OT
        private void ManejoEstadoColumnas(string p_strCambPreTall, IForm p_formularioSbo,int p_EstadoOt)
        {

            Matrix oMatrix;
            Column oColumn;
   
            try
            {
                oMatrix = ((Matrix) p_formularioSbo.Items.Item("mtxServE").Specific);
                
                oColumn = oMatrix.Columns.Item("Col_prec");
                oColumn.Editable =( p_strCambPreTall == "Y" && p_EstadoOt < 4);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Maneja el evento Lost Focus y evalúa de donde proviene el evento: Matrix y Columna
        /// </summary>
        /// <param name="pval">Item del Evento</param>
        private void ManejadorEventoLostFocus(ItemEvent pval)
        {
            try
            {
                switch (pval.ItemUID )
                {
                    case "mtxServE":
                        string strValorColumna = pval.ColUID;
                        switch (strValorColumna)
                        {
                            case "Col_prec":
                                ActualizaPrecioDataTable(pval);
                                break;
                        }
                       
                        break;
                }
                 
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        /// <summary>
        /// Actualiza los precios de la Matrix en el DataTable
        /// </summary>
        /// <param name="pval">Item del evento</param>
        private void ActualizaPrecioDataTable(ItemEvent pval)
        {
            int intRow;
            string strDataTable;
            DataTable dtDataTable;
            Matrix oMatrix;
            
            try
            {
                intRow = pval.Row;
                if (g_realizofiltroServiciosExter)
                {
                    strDataTable = g_strdtServiciosExternosTemporal;
                }
                else
                {
                    strDataTable = g_strdtServiciosExternos;
                }
                dtDataTable = FormularioSBO.DataSources.DataTables.Item(strDataTable);
                oMatrix = (Matrix)FormularioSBO.Items.Item("mtxServE").Specific;

                if (dtDataTable.GetValue("perm", intRow - 1).ToString() == "Y" || dtDataTable.GetValue("perm", intRow - 1).ToString() == "U")
                {
                    oMatrix.FlushToDataSource();
                    dtDataTable.SetValue("perm", intRow - 1, "U");
                    oMatrix.LoadFromDataSource();
                }
                
                

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
