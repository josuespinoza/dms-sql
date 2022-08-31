using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones
{
    public class MatrixSBOListReq : MatrixSBO
    {

        #region ...Propiedades...
        public ColumnaMatrixSBOEditText<string> ColumnaNoRequisicion { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoOT { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoArticulo { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoRequisicion { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFecha { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaHora { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstado { get; private set; }
        #endregion

        #region ...Constructor...
        public MatrixSBOListReq(string uniqueId, IForm formularioSBO, string tablaLigada)
            : base(uniqueId, formularioSBO)
        {
            this.TablaLigada = tablaLigada;
        }
        #endregion

        #region ...Metodos...

        public override void LigaColumnas()
        {
            ColumnaNoRequisicion.AsignaBindingDataTable();
            ColumnaNoOT.AsignaBindingDataTable();
            ColumnaTipoArticulo.AsignaBindingDataTable();
            ColumnaTipoRequisicion.AsignaBindingDataTable();
            ColumnaFecha.AsignaBindingDataTable();
            ColumnaHora.AsignaBindingDataTable();
            ColumnaEstado.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaNoRequisicion = new ColumnaMatrixSBOEditText<string>("ColNoReq", true, "ColNoReq", this);
            ColumnaNoOT = new ColumnaMatrixSBOEditText<string>("ColNoOT", true, "ColNoOT", this);
            ColumnaTipoArticulo = new ColumnaMatrixSBOEditText<string>("ColTipArt", true, "ColTipArt", this);
            ColumnaTipoRequisicion = new ColumnaMatrixSBOEditText<string>("ColTipReq", true, "ColTipReq", this);
            ColumnaFecha = new ColumnaMatrixSBOEditText<string>("ColDate", true, "ColDate", this);
            ColumnaHora = new ColumnaMatrixSBOEditText<string>("ColHora", true, "ColHora", this); 
            ColumnaEstado = new ColumnaMatrixSBOEditText<string>("ColStatus", true, "ColStatus", this);
        }

        #endregion

    }
}
