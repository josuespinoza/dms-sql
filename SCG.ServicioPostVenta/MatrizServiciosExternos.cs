using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizServiciosExternos : MatrixSBO
    {
        public MatrizServiciosExternos(string UniqueId, IForm formularioSBO, string tablaLigada)
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
        public ColumnaMatrixSBOEditText<string> ColumnaAdic { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPend { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaSoli { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaReci { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPDev { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPTra { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPBod { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIDIt { get; set; }
        public ColumnaMatrixSBOEditText<string> Columnaesco { get; set; }

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
            ColumnaAdic.AsignaBindingDataTable();
            ColumnaPend.AsignaBindingDataTable();
            ColumnaSoli.AsignaBindingDataTable();
            ColumnaReci.AsignaBindingDataTable();
            ColumnaPDev.AsignaBindingDataTable();
            ColumnaPTra.AsignaBindingDataTable();
            ColumnaPBod.AsignaBindingDataTable();
            ColumnaIDIt.AsignaBindingDataTable();
            Columnaesco.AsignaBindingDataTable();
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
            ColumnaAdic = new ColumnaMatrixSBOEditText<string>("Col_adic", true, "adic", this);
            ColumnaPend = new ColumnaMatrixSBOEditText<string>("Col_pend", true, "pend", this);
            ColumnaSoli = new ColumnaMatrixSBOEditText<string>("Col_soli", true, "soli", this);
            ColumnaReci = new ColumnaMatrixSBOEditText<string>("Col_reci", true, "reci", this);
            ColumnaPDev = new ColumnaMatrixSBOEditText<string>("Col_pdev", true, "pdev", this);
            ColumnaPTra = new ColumnaMatrixSBOEditText<string>("Col_ptra", true, "ptra", this);
            ColumnaPBod = new ColumnaMatrixSBOEditText<string>("Col_pbod", true, "pbod", this);
            ColumnaIDIt = new ColumnaMatrixSBOEditText<string>("Col_idit", true, "idit", this);
            Columnaesco = new ColumnaMatrixSBOEditText<string>("Col_esco", true, "esco", this);
        }
    }
}
