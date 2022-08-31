using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones
{
    public class MatrixSBOListCan : MatrixSBO
    {

        #region ...Propiedades...
        public ColumnaMatrixSBOEditText<string> ColumnaNoReq { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoOT { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCod { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDes { get; private set; }
        public ColumnaMatrixSBOEditText<double> ColumnaCant { get; private set; }
        #endregion


        //public MatrixSBOListCan(IItem itemSBO) : base(itemSBO)
        //{
        //}

        //public MatrixSBOListCan(string uniqueId) : base(uniqueId)
        //{
        //}

        #region Constructor
        public MatrixSBOListCan(string uniqueId, IForm formularioSBO, string tablaligada) : base(uniqueId, formularioSBO)
        {
            this.TablaLigada = tablaligada;
        }
        #endregion


        #region Metodos
        public override void LigaColumnas()
        {
            ColumnaNoReq.AsignaBindingDataTable();
            ColumnaNoOT.AsignaBindingDataTable();
            ColumnaCod.AsignaBindingDataTable();
            ColumnaDes.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaNoReq = new ColumnaMatrixSBOEditText<string>("ColNoReq", true, "ColNoReq", this);
            ColumnaNoOT = new ColumnaMatrixSBOEditText<string>("ColNoOT", true, "ColNoOT", this);
            ColumnaCod = new ColumnaMatrixSBOEditText<string>("ColCod", true, "ColCod", this);
            ColumnaDes = new ColumnaMatrixSBOEditText<string>("ColDes", true, "ColDes", this);
            ColumnaCant = new ColumnaMatrixSBOEditText<double>("ColCant", true, "ColCant", this);
        }
        #endregion
    }
}
