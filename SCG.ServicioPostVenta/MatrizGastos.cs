using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizGastos : MatrixSBO
    {
        public MatrizGastos(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaCodi { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCant { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMone { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPrec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCost { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFPro { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAsie { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaApro { get; set; }

        public override void LigaColumnas()
        {
            ColumnaCodi.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
            ColumnaMone.AsignaBindingDataTable();
            ColumnaPrec.AsignaBindingDataTable();
            ColumnaCost.AsignaBindingDataTable();
            ColumnaFPro.AsignaBindingDataTable();
            ColumnaAsie.AsignaBindingDataTable();
            ColumnaApro.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaApro = new ColumnaMatrixSBOEditText<string>("Col_apro", true, "apro", this);
            ColumnaCodi = new ColumnaMatrixSBOEditText<string>("Col_code", true, "code", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("Col_desc", true, "desc", this);
            ColumnaCant = new ColumnaMatrixSBOEditText<string>("Col_cant", true, "cant", this);
            ColumnaMone = new ColumnaMatrixSBOEditText<string>("Col_mone", true, "mone", this);
            ColumnaPrec = new ColumnaMatrixSBOEditText<string>("Col_prec", true, "prec", this);
            ColumnaCost = new ColumnaMatrixSBOEditText<string>("Col_cost", true, "cost", this);
            ColumnaFPro = new ColumnaMatrixSBOEditText<string>("Col_fpro", true, "fpro", this);
            ColumnaAsie = new ColumnaMatrixSBOEditText<string>("Col_asie", true, "asie", this);
        }
    }
}
