using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizServiciosAsignación : MatrixSBO
    {
        public MatrizServiciosAsignación(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaSele { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodi { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEsta { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFase { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAsig { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIdAc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDura { get; set; }
        public ColumnaMatrixSBOEditText<string> Columnacfas { get; set; }

        public override void LigaColumnas()
        {
            ColumnaSele.AsignaBindingDataTable();
            ColumnaCodi.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaEsta.AsignaBindingDataTable();
            ColumnaFase.AsignaBindingDataTable();
            ColumnaAsig.AsignaBindingDataTable();
            ColumnaIdAc.AsignaBindingDataTable();
            ColumnaDura.AsignaBindingDataTable();
            Columnacfas.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaSele = new ColumnaMatrixSBOEditText<string>("Col_sele", true, "sele", this);
            ColumnaCodi = new ColumnaMatrixSBOEditText<string>("Col_code", true, "codi", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("Col_desc", true, "desc", this);
            ColumnaEsta = new ColumnaMatrixSBOEditText<string>("Col_esta", true, "esta", this);
            ColumnaFase = new ColumnaMatrixSBOEditText<string>("Col_fase", true, "fase", this);
            ColumnaAsig = new ColumnaMatrixSBOEditText<string>("Col_asig", true, "asig", this);
            ColumnaIdAc = new ColumnaMatrixSBOEditText<string>("Col_idac", true, "idac", this);
            ColumnaDura = new ColumnaMatrixSBOEditText<string>("Col_dura", true, "dura", this);
            Columnacfas = new ColumnaMatrixSBOEditText<string>("Col_cfas", true, "cfas", this);
         }
    }
}
