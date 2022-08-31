using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizServicios : MatrixSBO
    {
        public MatrizServicios(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }
        public ColumnaMatrixSBOEditText<string> ColumnaTras { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaApro { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPerm { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaSele { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodi { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCant { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPrec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMone { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEsta { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDrAp { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoFa { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAdic { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIDIt { get; set; }

        public override void LigaColumnas()
        {
            ColumnaTras.AsignaBindingDataTable();
            ColumnaApro.AsignaBindingDataTable();
            ColumnaPerm.AsignaBindingDataTable();
            ColumnaSele.AsignaBindingDataTable();
            ColumnaCodi.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
            ColumnaPrec.AsignaBindingDataTable();
            ColumnaMone.AsignaBindingDataTable();
            ColumnaEsta.AsignaBindingDataTable();
            ColumnaDrAp.AsignaBindingDataTable();
            ColumnaNoFa.AsignaBindingDataTable();
            ColumnaAdic.AsignaBindingDataTable();
            ColumnaIDIt.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaTras = new ColumnaMatrixSBOEditText<string>("Col_tras", true, "tras", this);
            ColumnaApro = new ColumnaMatrixSBOEditText<string>("Col_apro", true, "apro", this);
            ColumnaPerm = new ColumnaMatrixSBOEditText<string>("Col_perm", true, "perm", this);
            ColumnaSele = new ColumnaMatrixSBOEditText<string>("Col_sele", true, "sele", this);
            ColumnaCodi = new ColumnaMatrixSBOEditText<string>("Col_code", true, "code", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("Col_desc", true, "desc", this);
            ColumnaCant = new ColumnaMatrixSBOEditText<string>("Col_cant", true, "cant", this);
            ColumnaPrec = new ColumnaMatrixSBOEditText<string>("Col_prec", true, "prec", this);
            ColumnaMone = new ColumnaMatrixSBOEditText<string>("Col_mone", true, "mone", this);
            ColumnaEsta = new ColumnaMatrixSBOEditText<string>("Col_esta", true, "esta", this);
            ColumnaDrAp = new ColumnaMatrixSBOEditText<string>("Col_drap", true, "dura", this);
            ColumnaNoFa = new ColumnaMatrixSBOEditText<string>("Col_nofa", true, "nofa", this);
            ColumnaAdic = new ColumnaMatrixSBOEditText<string>("Col_adic", true, "adic", this);
            ColumnaIDIt = new ColumnaMatrixSBOEditText<string>("Col_idit", true, "idit", this);

        }
    }
}
