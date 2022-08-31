using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizIngresos : MatrixSBO
    {
        public MatrizIngresos(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaApro { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodi { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCant { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMone { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPrec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCost { get; set; }

        public override void LigaColumnas()
        {
            ColumnaApro.AsignaBindingDataTable();
            ColumnaCodi.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
            ColumnaMone.AsignaBindingDataTable();
            ColumnaPrec.AsignaBindingDataTable();
            ColumnaCost.AsignaBindingDataTable();
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
        }
    }
}
