using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones.UI
{
    public class MatrixSBOMovimientosRequisiciones : MatrixSBO
    {
        public ColumnaMatrixSBOEditText<float> ColumnaCantidadTransferida;
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoArticulo;
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoDocumento;
        public ColumnaMatrixSBOEditText<string> ColumnaNumeroDocumento;
        public ColumnaMatrixSBOEditText<string> ColumnaDescripcionArticulo;
        public ColumnaMatrixSBOEditText<DateTime> ColumnaFechaDocumento;
        public ColumnaMatrixSBOEditText<string> ColumnaTipoDocumento;
        public NumberFormatInfo NumberFormatInfo { get; set; }

        public MatrixSBOMovimientosRequisiciones(IItem itemSBO) : base(itemSBO)
        {
        }

        public MatrixSBOMovimientosRequisiciones(string uniqueId) : base(uniqueId)
        {
        }

        public MatrixSBOMovimientosRequisiciones(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public override void LigaColumnas()
        {
            ColumnaCodigoArticulo.AsignaBinding();
            ColumnaDescripcionArticulo.AsignaBinding();
            ColumnaCantidadTransferida.AsignaBinding();
            ColumnaCodigoDocumento.AsignaBinding();
            ColumnaNumeroDocumento.AsignaBinding();
            ColumnaTipoDocumento.AsignaBinding();
            ColumnaFechaDocumento.AsignaBinding();
        }

        public override void CreaColumnas()
        {
            ColumnaCodigoArticulo = new ColumnaMatrixSBOEditText<string>("colCodArt", true, "U_SCGD_CodArticulo", this);
            ColumnaDescripcionArticulo = new ColumnaMatrixSBOEditText<string>("colDescArt", true, "U_SCGD_DescArticulo",
                                                                              this);
            ColumnaCantidadTransferida = new ColumnaMatrixSBOEditText<float>("colCant", true, "U_SCGD_CantTransf", this);
            ColumnaCodigoDocumento = new ColumnaMatrixSBOEditText<string>("colDocEnt1", true, "U_SCGD_DocEntry", this);
            ColumnaNumeroDocumento = new ColumnaMatrixSBOEditText<string>("colDocNum1", true, "U_SCGD_DocNum", this);
            ColumnaTipoDocumento = new ColumnaMatrixSBOEditText<string>("colTipoDoc", true, "U_SCGD_TipoDoc", this);
            ColumnaFechaDocumento = new ColumnaMatrixSBOEditText<DateTime>("colFecDoc", true, "U_SCGD_FechaDoc", this);
        }

        public List<InformacionLineasMovimientos> DataTable2Collection()
        {
            List<InformacionLineasMovimientos> result = null;
            if (FormularioSBO != null)
            {
                DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);

                result = new List<InformacionLineasMovimientos>(dbDataSource.Size);
                for (int offset = 0; offset < dbDataSource.Size; offset++)
                {
                    InformacionLineasMovimientos informacionLineasMovimientos = LineaFromDBDataSource(offset);
                    result.Add(informacionLineasMovimientos);
                }
            }
            return result;
        }
        protected virtual InformacionLineasMovimientos LineaFromDBDataSource(int offset)
        {
            DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
            InformacionLineasMovimientos informacionLineasMovimientos = new InformacionLineasMovimientos();
            informacionLineasMovimientos.DocEntry =
                Convert.ToInt32(dbDataSource.GetValue("DocEntry", offset).TrimEnd());
            informacionLineasMovimientos.LineId =
                Convert.ToInt32(dbDataSource.GetValue("LineId", offset).TrimEnd());
            informacionLineasMovimientos.VisOrder =
                Convert.ToInt32(dbDataSource.GetValue("VisOrder", offset).TrimEnd());
            informacionLineasMovimientos.CodigoArticulo =
                dbDataSource.GetValue("U_SCGD_CodArticulo", offset).TrimEnd();
            informacionLineasMovimientos.DescripcionArticulo =
                dbDataSource.GetValue("U_SCGD_DescArticulo", offset).TrimEnd();
            informacionLineasMovimientos.CantidadTransferida =
                Convert.ToSingle(dbDataSource.GetValue("U_SCGD_CantTransf", offset).TrimEnd(), NumberFormatInfo);
            informacionLineasMovimientos.TipoDocumento =
                dbDataSource.GetValue("U_SCGD_TipoDoc", offset).TrimEnd();
            return informacionLineasMovimientos;
        }
        public virtual void InsertaEnDBDataSource(InformacionLineasMovimientos informacionLineasMovimientos)
        {
            DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
            int pos = dbDataSource.Size;
            dbDataSource.InsertRecord(pos);
            //pos = dbDataSource.Size;
            dbDataSource.SetValue(ColumnaCodigoArticulo.ColumnaLigada, pos, informacionLineasMovimientos.CodigoArticulo);
            dbDataSource.SetValue(ColumnaDescripcionArticulo.ColumnaLigada, pos, informacionLineasMovimientos.DescripcionArticulo);
            dbDataSource.SetValue(ColumnaCodigoDocumento.ColumnaLigada, pos, informacionLineasMovimientos.CodigoDocumento.ToString());
            dbDataSource.SetValue(ColumnaNumeroDocumento.ColumnaLigada, pos, informacionLineasMovimientos.NumeroDocumento.ToString());
            dbDataSource.SetValue(ColumnaTipoDocumento.ColumnaLigada, pos, informacionLineasMovimientos.TipoDocumento);
            dbDataSource.SetValue(ColumnaCantidadTransferida.ColumnaLigada, pos, informacionLineasMovimientos.CantidadTransferida.ToString(NumberFormatInfo));
            dbDataSource.SetValue(ColumnaFechaDocumento.ColumnaLigada, pos, informacionLineasMovimientos.Fecha.ToString("yyyyMMdd"));
            dbDataSource.SetValue("VisOrder",pos, pos.ToString());
            dbDataSource.SetValue("LineId", pos, (pos + 1).ToString());
        }
        public virtual void EliminaPrimeraLinea()
        {
            DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
            if (dbDataSource.Size == 1 && dbDataSource.GetValue("U_SCGD_CodArticulo",0).TrimEnd()=="-1")
                dbDataSource.RemoveRecord(0);
        }
    }
}