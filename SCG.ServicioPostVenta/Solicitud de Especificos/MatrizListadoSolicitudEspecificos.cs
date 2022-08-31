using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizListadoSolicitudEspecificos: MatrixSBO
    {
        public MatrizListadoSolicitudEspecificos(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaDocE { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDocN { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoOT { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaS { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaHoraS { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaSolBy { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMarca { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstilo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModelo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaUnidad { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPlaca { get; set; }

        public override void LigaColumnas()
        {
            ColumnaDocE.AsignaBindingDataTable();
            ColumnaDocN.AsignaBindingDataTable();
            ColumnaNoOT.AsignaBindingDataTable();
            ColumnaFechaS.AsignaBindingDataTable();
            ColumnaHoraS.AsignaBindingDataTable();
            ColumnaSolBy.AsignaBindingDataTable();
            ColumnaMarca.AsignaBindingDataTable();
            ColumnaEstilo.AsignaBindingDataTable();
            ColumnaModelo.AsignaBindingDataTable();
            ColumnaUnidad.AsignaBindingDataTable();
            ColumnaPlaca.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaDocE = new ColumnaMatrixSBOEditText<string>("ColDocE", true, "ColDocE", this);
            ColumnaDocN = new ColumnaMatrixSBOEditText<string>("ColDocN", true, "ColDocN", this);
            ColumnaNoOT = new ColumnaMatrixSBOEditText<string>("ColNoOT", true, "ColNoOT", this);
            ColumnaFechaS = new ColumnaMatrixSBOEditText<string>("ColFecha", true, "ColFecha", this);
            ColumnaHoraS = new ColumnaMatrixSBOEditText<string>("ColHora", true, "ColHora", this);
            ColumnaSolBy = new ColumnaMatrixSBOEditText<string>("ColSolBy", true, "ColSolBy", this);
            ColumnaMarca = new ColumnaMatrixSBOEditText<string>("ColMarca", true, "ColMarca", this);
            ColumnaEstilo = new ColumnaMatrixSBOEditText<string>("ColEstilo", true, "ColEstilo", this);
            ColumnaModelo = new ColumnaMatrixSBOEditText<string>("ColModelo", true, "ColModelo", this);
            ColumnaUnidad = new ColumnaMatrixSBOEditText<string>("ColUnidad",true,"ColUnidad",this);
            ColumnaPlaca = new ColumnaMatrixSBOEditText<string>("ColPlaca",true,"ColPlaca",this);
         }
    }
}
