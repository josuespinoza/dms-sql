using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using DMS_Connector.Business_Logic.DataContract.Requisiciones;

namespace SCG.Requisiciones.UI
{
    public class MatrixSBOLineasRequisiciones : MatrixSBO
    {
        public event CopiarLineasMatrizHandler CopiarLineasMatriz;

        public MatrixSBOLineasRequisiciones(IItem itemSBO) : base(itemSBO)
        {
        }

        public MatrixSBOLineasRequisiciones(string uniqueId) : base(uniqueId)
        {
        }

        public MatrixSBOLineasRequisiciones(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public ColumnaMatrixSBOEditText<string> ColumnaCodigoArticulo { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDescripcionArticulo { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoBodegaOrigen { get; private set; }
        public ColumnaMatrixSBOEditText<float> ColumnaDisponible { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoBodegaDestino { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoArticulo { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadSolicitada { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadOriginal { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadAjuste { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadRecibida { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadPendiente { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCantidadATransferir { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaCodigoEstado { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstado { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaCentroCosto { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaCodigoTipoArticulo { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaDocumentoOrigen { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaLineNumOrigen { get; private set; }
        public ColumnaMatrixSBOCheckBox<int> ColumnaCheck { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaDeUbicacion { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaAUbicacion { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaLineaIDSucursal { get; private set; }
        public ColumnaMatrixSBOEditText<DateTime> ColumnaLineaFechaMovimiento { get; private set; }
        public ColumnaMatrixSBOEditText<DateTime> ColumnaLineaHoraMovimiento { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaLineaTipoMovimiento { get; private set; }
        public ColumnaMatrixSBOEditText<int> ColumnaLineaReqOrigenPenndiente { get; private set; }
        public ColumnaMatrixSBOComboBox< string> ColumnaLineaObservcacion { get; private set; }
        
        public ResourceManager ResourceManager { get; set; }
        public CultureInfo CultureInfo { get; set; }

        public override void LigaColumnas()
        {
            ColumnaCodigoArticulo.AsignaBinding();
            ColumnaDescripcionArticulo.AsignaBinding();
            ColumnaCodigoBodegaOrigen.AsignaBinding();
            ColumnaDisponible.AsignaBinding();
            ColumnaCodigoBodegaDestino.AsignaBinding();
            ColumnaTipoArticulo.AsignaBinding();
            ColumnaCantidadSolicitada.AsignaBinding();
            ColumnaCantidadRecibida.AsignaBinding();
            ColumnaCantidadPendiente.AsignaBinding();
            ColumnaCantidadATransferir.AsignaBinding();
            ColumnaCantidadOriginal.AsignaBinding();
            ColumnaCantidadAjuste.AsignaBinding(); 
            ColumnaCodigoEstado.AsignaBinding();
            ColumnaEstado.AsignaBinding();
            ColumnaCentroCosto.AsignaBinding();
            ColumnaCodigoTipoArticulo.AsignaBinding();
            ColumnaDocumentoOrigen.AsignaBinding();
            ColumnaLineNumOrigen.AsignaBinding();
            ColumnaCheck.AsignaBinding();
            ColumnaLineaIDSucursal.AsignaBinding();
            ColumnaLineaFechaMovimiento .AsignaBinding();
            ColumnaLineaHoraMovimiento .AsignaBinding();
            ColumnaLineaTipoMovimiento.AsignaBinding();
            ColumnaLineaReqOrigenPenndiente .AsignaBinding();
            ColumnaLineaObservcacion.AsignaBinding() ;
            //SE COMENTA PARA EL PROCESO DE UBICACIONES
            ColumnaDeUbicacion.AsignaBinding();
            ColumnaAUbicacion.AsignaBinding();
 
        }

        public override void CreaColumnas()
        {
            ColumnaCodigoArticulo = new ColumnaMatrixSBOEditText<string>("colCodArt", true, "U_SCGD_CodArticulo", this);
            ColumnaDescripcionArticulo = new ColumnaMatrixSBOEditText<string>("colDescArt", true, "U_SCGD_DescArticulo",
                                                                              this);
            ColumnaCodigoBodegaOrigen = new ColumnaMatrixSBOEditText<string>("colCdBOr", true, "U_SCGD_CodBodOrigen",
                                                                             this);
            ColumnaDisponible = new ColumnaMatrixSBOEditText<float>("colDisp", true, "U_SCGD_CantDispo", this);
            ColumnaCodigoBodegaDestino = new ColumnaMatrixSBOEditText<string>("colCdBDest", true, "U_SCGD_CodBodDest",
                                                                              this);
            ColumnaTipoArticulo = new ColumnaMatrixSBOEditText<string>("colTipoArt", true, "U_SCGD_TipoArticulo", this);
            ColumnaCantidadSolicitada = new ColumnaMatrixSBOEditText<double>("colCantSol", true, "U_SCGD_CantSol", this);
            ColumnaCantidadRecibida = new ColumnaMatrixSBOEditText<double>("colCantRec", true, "U_SCGD_CantRec", this);
            ColumnaCantidadPendiente = new ColumnaMatrixSBOEditText<double>("colCantPen", true, "U_SCGD_CantPen", this);
            ColumnaCantidadATransferir = new ColumnaMatrixSBOEditText<double>("colCantATr", true, "U_SCGD_CantATransf",
                                                                             this);
            ColumnaCantidadOriginal = new ColumnaMatrixSBOEditText<double>("colCantOr", true, "U_SCGD_COrig", this);
            ColumnaCantidadAjuste = new ColumnaMatrixSBOEditText<double>("colCantAj", true, "U_SCGD_CAju", this); 

            ColumnaCodigoEstado = new ColumnaMatrixSBOEditText<int>("colCodEst", true, "U_SCGD_CodEst", this);
            ColumnaEstado = new ColumnaMatrixSBOEditText<string>("colEstado", true, "U_SCGD_Estado", this);
            ColumnaCentroCosto = new ColumnaMatrixSBOEditText<int>("colCCosto", true, "U_SCGD_CCosto", this);
            ColumnaCodigoTipoArticulo = new ColumnaMatrixSBOEditText<int>("colCodTArt", true, "U_SCGD_CodTipoArt", this);
            ColumnaDocumentoOrigen = new ColumnaMatrixSBOEditText<int>("colDocOr", true, "U_SCGD_DocOr", this);
            ColumnaLineNumOrigen = new ColumnaMatrixSBOEditText<int>("colLNumOr", true, "U_SCGD_LNumOr", this);

            ColumnaDeUbicacion = new ColumnaMatrixSBOEditText<int>("colDeUbic", true, "U_DeUbic", this);
            ColumnaAUbicacion = new ColumnaMatrixSBOEditText<int>("colAUbic", true, "U_AUbic", this);
            ColumnaLineaIDSucursal = new ColumnaMatrixSBOEditText<string>("colLidsuc", true, "U_SCGD_Lidsuc", this);
            ColumnaLineaTipoMovimiento = new ColumnaMatrixSBOEditText<int>("colTipo", true, "U_TipoM", this);
            ColumnaLineaFechaMovimiento = new ColumnaMatrixSBOEditText<DateTime>("colFecha", true, "U_FechaM", this);
            ColumnaLineaHoraMovimiento = new ColumnaMatrixSBOEditText<DateTime>("colHora", true, "U_HoraM", this);
            ColumnaLineaReqOrigenPenndiente = new ColumnaMatrixSBOEditText<int>("colReqOrP", true, "U_ReqOriPen", this);
            ColumnaLineaObservcacion = new ColumnaMatrixSBOComboBox<string>("colObse", true, "U_Obs_Req", this);
            
            ColumnaCheck = new ColumnaMatrixSBOCheckBox<int>("colChk", true, "U_SCGD_Chk", this);

            ColumnaCantidadATransferir.Columna.AffectsFormMode = false;
            ColumnaCheck.Columna.ValOn = "1";
            ColumnaCheck.Columna.ValOff = "0";
        }

        public List<InformacionLineaRequisicion> DataTable2Collection(bool seleccionadas)
        {
            List<InformacionLineaRequisicion> result = null;
            if (FormularioSBO != null)
            {
                DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
                
                result = new List<InformacionLineaRequisicion>(dbDataSource.Size);
                for (int i = 0; i < dbDataSource.Size; i++)
                {
                    InformacionLineaRequisicion informacionLineasReq = null;
                    if (CopiarLineasMatriz != null)
                         informacionLineasReq = CopiarLineasMatriz(dbDataSource, i);
                    if (informacionLineasReq == null)
                        informacionLineasReq = new InformacionLineaRequisicion();
                    LineaFromDBDataSource(i, informacionLineasReq);
                    if (seleccionadas && informacionLineasReq.Seleccionada)
                        result.Add(informacionLineasReq);
                }
            }
            return result;
        }

        public void LineaFromDBDataSource(int offset, ref LineaRequisicion p_LineaReq)
        {
            IDBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas);
            p_LineaReq.DocEntry = Convert.ToInt32(dbDataSource.GetValue("DocEntry", offset).TrimEnd());
            p_LineaReq.LineId = Convert.ToInt32(dbDataSource.GetValue("LineId", offset).TrimEnd());
            p_LineaReq.VisOrder = Convert.ToInt32(dbDataSource.GetValue("VisOrder", offset).TrimEnd());
            p_LineaReq.U_SCGD_CodArticulo = dbDataSource.GetValue("U_SCGD_CodArticulo", offset).TrimEnd();
            p_LineaReq.U_SCGD_DescArticulo = dbDataSource.GetValue("U_SCGD_DescArticulo", offset).TrimEnd();
            p_LineaReq.U_SCGD_CodBodOrigen = dbDataSource.GetValue("U_SCGD_CodBodOrigen", offset).TrimEnd();
            p_LineaReq.U_SCGD_CodBodDest = dbDataSource.GetValue("U_SCGD_CodBodDest", offset).TrimEnd();
            p_LineaReq.U_SCGD_CantRec = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantRec", offset).TrimEnd(), NumberFormatInfo);
            p_LineaReq.U_SCGD_CantPen = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantPen", offset).TrimEnd(), NumberFormatInfo);
            p_LineaReq.U_SCGD_CantSol = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantSol", offset).TrimEnd(), NumberFormatInfo);
            p_LineaReq.U_SCGD_CantATransf = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantATransf", offset).TrimEnd(), NumberFormatInfo);
            p_LineaReq.U_SCGD_CodTipoArt = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CodTipoArt", offset).TrimEnd());
            p_LineaReq.U_SCGD_COrig = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_COrig", offset).TrimEnd(), NumberFormatInfo);
            p_LineaReq.U_SCGD_CAju = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CAju", offset).TrimEnd(), NumberFormatInfo);

            //SE COMENTA PARA EL PROCESO DE UBICACIONES
            p_LineaReq.U_DeUbic = dbDataSource.GetValue("U_DeUbic", offset).TrimEnd();
            p_LineaReq.U_AUbic = dbDataSource.GetValue("U_AUbic", offset).TrimEnd();
            p_LineaReq.U_SCGD_TipoArticulo = dbDataSource.GetValue("U_SCGD_TipoArticulo", offset).TrimEnd();
            p_LineaReq.U_SCGD_CodEst = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CodEst", offset).TrimEnd());
            p_LineaReq.U_SCGD_Estado = dbDataSource.GetValue("U_SCGD_Estado", offset).TrimEnd();
            p_LineaReq.U_SCGD_CCosto = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CCosto", offset).TrimEnd());
            p_LineaReq.DataSourceOffset = offset;
            p_LineaReq.U_SCGD_Chk = int.Parse(dbDataSource.GetValue("U_SCGD_Chk", offset).TrimEnd());
            p_LineaReq.U_SCGD_DocOr = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_DocOr", offset).TrimEnd());
            p_LineaReq.U_SCGD_LNumOr = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_LNumOr", offset).TrimEnd());
            p_LineaReq.U_SCGD_Lidsuc = dbDataSource.GetValue("U_SCGD_Lidsuc", offset).TrimEnd();
            p_LineaReq.U_SCGD_ID = dbDataSource.GetValue("U_SCGD_ID", offset).TrimEnd();
        }

        public void LineaFromDBDataSource(int offset, InformacionLineaRequisicion informacionLineaReq)
        {
            IDBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(UDORequisiciones.TablaLineas);
            informacionLineaReq.DocEntry = Convert.ToInt32(dbDataSource.GetValue("DocEntry", offset).TrimEnd());
            informacionLineaReq.LineId = Convert.ToInt32(dbDataSource.GetValue("LineId", offset).TrimEnd());
            informacionLineaReq.VisOrder = Convert.ToInt32(dbDataSource.GetValue("VisOrder", offset).TrimEnd());
            informacionLineaReq.CodigoArticulo = dbDataSource.GetValue("U_SCGD_CodArticulo", offset).TrimEnd();
            informacionLineaReq.DescripcionArticulo = dbDataSource.GetValue("U_SCGD_DescArticulo", offset).TrimEnd();
            informacionLineaReq.CodigoBodegaOrigen = dbDataSource.GetValue("U_SCGD_CodBodOrigen", offset).TrimEnd();
            informacionLineaReq.CodigoBodegaDestino = dbDataSource.GetValue("U_SCGD_CodBodDest", offset).TrimEnd();
            informacionLineaReq.CantidadRecibida = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantRec", offset).TrimEnd(), NumberFormatInfo);
            informacionLineaReq.CantidadPendiente = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantPen", offset).TrimEnd(), NumberFormatInfo);
                //Math.Round(Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantPen", offset).TrimEnd()), NumberFormatInfo.CurrencyDecimalDigits);
            informacionLineaReq.CantidadSolicitada = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantSol", offset).TrimEnd(), NumberFormatInfo);
            informacionLineaReq.CantidadATransferir = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CantATransf", offset).TrimEnd(), NumberFormatInfo);
            informacionLineaReq.CodigoTipoArticulo = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CodTipoArt", offset).TrimEnd());
            informacionLineaReq.CantidadOriginal = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_COrig", offset).TrimEnd(), NumberFormatInfo);
            informacionLineaReq.CantidadAjuste = Convert.ToDouble(dbDataSource.GetValue("U_SCGD_CAju", offset).TrimEnd(), NumberFormatInfo);
            
            //SE COMENTA PARA EL PROCESO DE UBICACIONES
            informacionLineaReq.DeUbicacion = dbDataSource.GetValue("U_DeUbic", offset).TrimEnd();
            informacionLineaReq.AUbicacion = dbDataSource.GetValue("U_AUbic", offset).TrimEnd();
            informacionLineaReq.DescripcionTipoArticulo = dbDataSource.GetValue("U_SCGD_TipoArticulo", offset).TrimEnd();
            informacionLineaReq.CodigoEstado = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CodEst", offset).TrimEnd());
            informacionLineaReq.Estado = dbDataSource.GetValue("U_SCGD_Estado", offset).TrimEnd();
            informacionLineaReq.CentroCosto = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_CCosto", offset).TrimEnd());
            informacionLineaReq.DataSourceOffset = offset;
            informacionLineaReq.Seleccionada = Convert.ToBoolean(int.Parse(dbDataSource.GetValue("U_SCGD_Chk", offset).TrimEnd()));
            informacionLineaReq.DocumentoOrigen = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_DocOr", offset).TrimEnd());
            informacionLineaReq.LineNumOrigen = Convert.ToInt32(dbDataSource.GetValue("U_SCGD_LNumOr", offset).TrimEnd());
            informacionLineaReq.LineaIDSucursal = dbDataSource.GetValue("U_SCGD_Lidsuc", offset).TrimEnd();
            informacionLineaReq.IDLinea = dbDataSource.GetValue("U_SCGD_ID", offset).TrimEnd();
        }

        public List<InformacionLineaRequisicion> SelectedRows2Collection()
        {
            List<InformacionLineaRequisicion> result = null;
            if (FormularioSBO != null)
            {
                Especifico.FlushToDataSource();
                result = DataTable2Collection(true);
            }
            return result;
        }
    }
}
