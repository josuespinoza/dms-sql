using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM;

namespace SCG.ServicioPostVenta
{
    public partial class SolicitudEspecificos
    {
        #region ...Varialbles...
        private SAPbouiCOM.DataTable dtQueryConf;
        private SAPbouiCOM.DataTable dtQuery;
        private SAPbouiCOM.DataTable dtSolicitud;
        private SAPbouiCOM.DataTable g_dtSolicitudes;
        private SAPbouiCOM.DataTable g_dtConfSucursal;
        private SAPbouiCOM.DataTable g_dtBodxCC;
        private SAPbouiCOM.DataTable g_dtAdmin;
        private SAPbouiCOM.DataTable g_dtEstadosOT;
        private SAPbouiCOM.DataTable g_dtAprobacion;

        private SAPbouiCOM.EditText g_oEditNoSol;
        private SAPbouiCOM.EditText g_oEditNoOT;
        private SAPbouiCOM.EditText g_oEditSolicita;
        private SAPbouiCOM.EditText g_oEditResponde;
        private SAPbouiCOM.EditText g_oEditFechSol;
        private SAPbouiCOM.EditText g_oEditAsesor;
        private SAPbouiCOM.EditText g_oEditAsesorCode;
        private SAPbouiCOM.EditText g_oEditPlaca;
        private SAPbouiCOM.EditText g_oEditMarca;
        private SAPbouiCOM.EditText g_oEditEstilo;
        private SAPbouiCOM.EditText g_oEditVIN;
        private SAPbouiCOM.EditText g_oEditNoUnid;
        private SAPbouiCOM.EditText g_oEditAno;
        private SAPbouiCOM.EditText g_oEditNoVisita;
        private SAPbouiCOM.EditText g_oEditEstado;
        private SAPbouiCOM.EditText g_oEditTipoOT;
        private SAPbouiCOM.EditText g_oEditFechaRes;
        private SAPbouiCOM.EditText g_oEditCliOT;
        private SAPbouiCOM.EditText g_oEditCliOTCode;
        private SAPbouiCOM.EditText g_oEditPreTot;
        private SAPbouiCOM.EditText g_oEditComments;
        private SAPbouiCOM.StaticText g_oStaticCurr;

        public OrdenTrabajo.RealizarTraslado g_intRealizarTraslados;
        public NumberFormatInfo n;

        private List<TransferenciasStock.LineasTransferenciasStock> g_listRepuestos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listSuministros = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listServiciosExternos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listEliminarRepuestos = new List<TransferenciasStock.LineasTransferenciasStock>();
        private List<TransferenciasStock.LineasTransferenciasStock> g_listEliminarSuministros = new List<TransferenciasStock.LineasTransferenciasStock>();

        private int g_intEstadoCotizacion;
        public bool g_blnPaqueteNoAprobado;

        public List<OrdenTrabajo.ListaCantidadesAnteriores> g_lstCantidadesAnteriores = new List<OrdenTrabajo.ListaCantidadesAnteriores>();


        #endregion

        #region ...Constantes...
        public const string g_strdtSolicitudes = "dtSolicitudes";
        public const string g_strdtConfSucursal = "tConfSuc";
        public const string g_strmtxLineasSol = "mtxLines";
        private const String strDtConsulta = "dtConsulta";
        private const String strDtSolicitud = "dtCSolicitud";
        private const String strDtQueryConf = "dtQueryConf";
        public const string g_strdtBodegasCentroCosto = "tBodxCC";
        public const string g_strdtADMIN = "tAdmin";
        public const string g_strdtEstadosOT = "tEstadosOT";

        public const string g_strColCantPendiente = "U_SCGD_CPen";
        public const string g_strColCantSolicitada = "U_SCGD_CSol";
        public const string g_strColCantRecibida = "U_SCGD_CRec";
        public const string g_strColCantPendienteDevolucion = "U_SCGD_CPDe";
        public const string g_strColCantPendienteTraslado = "U_SCGD_CPTr";
        public const string g_strColCantPendienteBodega = "U_SCGD_CPBo";
        public const string g_strdtAprobacion = "tAprob";

        public const int g_strRepuesto = 1;
        public const int g_strServicio = 2;
        public const int g_strSuministro = 3;
        public const int g_strServExterno = 4;
        public const int g_strPaquete = 5;
        public const int g_strNinguno = 0;
        public const int g_strOtrosGastos_Costos = 11;
        public const int g_strOtrosIngresos = 12;

        public const string m_strConsultaListaPreciosCliente = "Select ListNum from OCRD where CardCode = '{0}'";

        public const string g_strConsultaArti =
                " select top(1) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                " 1 as cant, it.Price as prec, it.Currency as mone, oi.U_SCGD_TipoArticulo as tiar, oi.U_SCGD_CodCtroCosto as ccos " +
                " from OITM as oi with (nolock) " +
                " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' and oi.ItemCode = '{2}' ";

        public const string g_strConsultaConfSucursal =
                " select U_DesSInv, U_Imp_Repuestos, U_Imp_Serv, U_Imp_ServExt, U_Imp_Suminis, U_Requis, U_UsaOfeVenta, U_UsaOrdVenta, U_SerOfC, U_SerOrC, U_USolOTEsp, U_ValReqPen, U_Entrega_Rep, U_FinOTCanSol, U_FOTAPen, U_TiempoEst_C, U_TiempoReal_C, U_SerInv, ISNULL(U_AsigUniMec,'N') U_AsigUniMec, U_CanOTSer, U_CanOTArAp, ISNULL(U_SolaUna,'N') as U_SolaUna , ISNULL(U_CamBodEsp,0) as U_CamBodEsp" +
                " from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}' "; 

        public const string g_strConsultaBodegasCentroCosto =
           " select cnfs.U_Sucurs as Sucursal,U_CC as CentroCosto, U_Rep as Repuestos, U_Ser as Servicios, U_Sum as Suministros, U_SE as ServExt, U_Pro as Proceso " +
           " from [@SCGD_CONF_BODXCC] as bxcc  with (nolock)  inner join [@SCGD_CONF_SUCURSAL] as cnfs   with (nolock)  on bxcc.DocEntry = cnfs.DocEntry ";
        
        public const string g_strConsultaAdmin =
            " select U_ReduceCant, U_UsaAXEV, U_UsaLed, U_EspVehic, U_TiemEsta, U_UsaDimC from [@SCGD_ADMIN]  with (nolock)  ";
        
        public const string g_strConsultaAprobacion =
           " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap  with (nolock)  inner join [@SCGD_CONF_SUCURSAL] as csu  with (nolock)  on csu.DocEntry = cap.DocEntry " +
           " where csu.U_Sucurs  = '{1}' and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] where U_SCGD_Numero_OT = '{0}' and U_SCGD_idSucursal = '{1}')";

        public const string g_strConsultaTipoOT = " select Name from [@SCGD_TIPO_ORDEN] where Code='{0}' ";

        private enum CotizacionEstado
        {
            Creada = 1,
            Modificada = 2,
            SinCambio = 3
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

        public enum EstadosAprobacion
        {
            Aprobado = 1,
            NoAprobado = 2,
            FaltoAprobacion = 3
        }


        public enum EstadosTraslado
        {
            NoProcesado = 0,
            No = 1,
            Si = 2,
            PendienteTraslado = 3,
            PendienteBodega = 4
        }

        public enum EstadosSolicitudEspecíficos
        {
            SinResponder = 0,
            Respondido = 1,
            Cancelado =2
        }

        #endregion

        #region ...Propiedades...
        public string FormType { get; set; }
        public string NombreXml { get; set; }
        public string Titulo { get; set; }
        public bool Inicializado { get; set; }
        public SAPbouiCOM.IForm FormularioSBO { get; set; }
        public Boolean ConfgUniMec { get; set; }
        public SAPbobsCOM.ICompany CompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;
        public IApplication ApplicationSBO { get; private set; }
        public string SolNum { get; set; }
        public string CotNum { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }
        public string BodegaProceso { get; set;}
        #region ...IUsaMenu Members...

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        //Manejo de reportes
        public string DireccionReportes { get; set; }
        public string BDUser { get; set; }
        public string BDPass { get; set; }

        #endregion

        #endregion

        public void InicializarControles()
        {
            dtQueryConf = FormularioSBO.DataSources.DataTables.Add(strDtQueryConf);
            dtQuery = FormularioSBO.DataSources.DataTables.Add(strDtConsulta);
            dtSolicitud = FormularioSBO.DataSources.DataTables.Add(strDtSolicitud);
            g_dtConfSucursal = FormularioSBO.DataSources.DataTables.Add(g_strdtConfSucursal);
            g_dtBodxCC = FormularioSBO.DataSources.DataTables.Add(g_strdtBodegasCentroCosto);
            g_dtAdmin = FormularioSBO.DataSources.DataTables.Add(g_strdtADMIN);
            g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Add(g_strdtEstadosOT);
            g_dtAprobacion = FormularioSBO.DataSources.DataTables.Add(g_strdtAprobacion);
        }

        public void InicializaFormulario()
        {
            try
            {
                if (FormularioSBO != null)
                {
                    FormularioSBO.Freeze(true);
                    FormularioSBO.Mode = BoFormMode.fm_FIND_MODE;
                    FormularioSBO.EnableMenu("1282", false);

                    g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item(g_strdtEstadosOT);
                    g_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ");

                    g_dtBodxCC = FormularioSBO.DataSources.DataTables.Item(g_strdtBodegasCentroCosto);
                    g_dtBodxCC.ExecuteQuery(g_strConsultaBodegasCentroCosto);

                    g_dtAdmin = FormularioSBO.DataSources.DataTables.Item(g_strdtADMIN);
                    g_dtAdmin.ExecuteQuery(g_strConsultaAdmin);

                    CreaColumnaDt();

                    FormularioSBO.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }


        public void ApplicationSBOOnItemEvent(ref SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            if (pVal.EventType != BoEventTypes.et_FORM_ACTIVATE && pVal.EventType != BoEventTypes.et_VALIDATE && pVal.EventType != BoEventTypes.et_FORM_DEACTIVATE)
            {
                if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    ManejadorEventoChooseFromList(ref pVal, ref BubbleEvent);
                }
                else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    ManejadorEventoItemPress(ref pVal, ref BubbleEvent);
                }
                else if (pVal.EventType == BoEventTypes.et_LOST_FOCUS && pVal.ColUID=="ColPrec" && pVal.Action_Success)
                {
                    RecalcularTotal(ref pVal);
                }
            }
        }

        public void CargarFormulario(string SolNum)
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

                    oCondition.Alias = "DocEntry";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = SolNum;

                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESPEC").Query(oConditions);
                    FormularioSBO.DataSources.DBDataSources.Item("@SCGD_SOL_ESP_LIN").Query(oConditions);
                    m_objMatrix = (Matrix)FormularioSBO.Items.Item(g_strmtxLineasSol).Specific;
                    m_objMatrix.LoadFromDataSource();
                    FormularioSBO.Refresh();
                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE;
                    FormularioSBO.Freeze(false);

                    bool bubble = false;
                    ManejadorEventoFormDataLoad(ref bubble);
                }
            }
            catch (Exception ex)
            {
                throw; //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }

        public void CreaColumnaDt()
        {
            dtSolicitud.Columns.Add("U_Moneda",BoFieldsType.ft_AlphaNumeric, 10);
            dtSolicitud.Columns.Add("U_PrecAcor", BoFieldsType.ft_Price,10);
            dtSolicitud.Columns.Add("U_NombEsp", BoFieldsType.ft_AlphaNumeric, 50);
            dtSolicitud.Columns.Add("U_ItmCodeE", BoFieldsType.ft_AlphaNumeric, 50);
            dtSolicitud.Columns.Add("U_BodeEsp", BoFieldsType.ft_AlphaNumeric, 10);
            dtSolicitud.Columns.Add("U_TipoArt", BoFieldsType.ft_AlphaNumeric, 5);
            dtSolicitud.Columns.Add("U_CCosEsp", BoFieldsType.ft_AlphaNumeric, 50);
            dtSolicitud.Columns.Add("Linea", BoFieldsType.ft_Integer , 10);
        }
    }
}
