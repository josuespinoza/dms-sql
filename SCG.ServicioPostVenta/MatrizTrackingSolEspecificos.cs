using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizTrackingSolEspecificos : MatrixSBO
    {
        public ColumnaMatrixSBOEditText<string> ColumnaSolicitud { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCantidad { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaItemC { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaSol { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaHoraSol { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaItemR { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDescR { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaResp { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaHoraResp { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaUsarioRes { get; set; }

        
        public MatrizTrackingSolEspecificos(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public override void LigaColumnas()
        {
            ColumnaSolicitud.AsignaBindingDataTable();
            ColumnaCantidad.AsignaBindingDataTable();
            ColumnaItemC.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaFechaSol.AsignaBindingDataTable();
            ColumnaHoraSol.AsignaBindingDataTable();
            ColumnaItemR.AsignaBindingDataTable();
            ColumnaDescR.AsignaBindingDataTable();
            ColumnaFechaResp.AsignaBindingDataTable();
            ColumnaHoraResp.AsignaBindingDataTable();
            ColumnaUsarioRes.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaSolicitud = new ColumnaMatrixSBOEditText<string>("col_Solic", true, "Solic", this);
            ColumnaCantidad = new ColumnaMatrixSBOEditText<string>("col_Canti", true, "Canti", this);
            ColumnaItemC = new ColumnaMatrixSBOEditText<string>("col_ItemC", true, "ItemC", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("col_Desc", true, "Descrip", this);
            ColumnaFechaSol = new ColumnaMatrixSBOEditText<string>("col_FecSol", true, "FecSol", this);
            ColumnaHoraSol = new ColumnaMatrixSBOEditText<string>("col_HorSol", true, "HoraSol", this);
            ColumnaItemR = new ColumnaMatrixSBOEditText<string>("col_ItemR", true, "ItemR", this);
            ColumnaDescR = new ColumnaMatrixSBOEditText<string>("col_DescR", true, "DescRe", this);
            ColumnaFechaResp = new ColumnaMatrixSBOEditText<string>("col_FechRe", true, "FecRes", this);
            ColumnaHoraResp = new ColumnaMatrixSBOEditText<string>("col_HoraRe", true, "HoraRes", this);
            ColumnaUsarioRes = new ColumnaMatrixSBOEditText<string>("col_Usuar", true, "Usuario", this);

            ColumnaSolicitud.Columna.Editable = false;
            ColumnaCantidad.Columna.Editable = false;
            ColumnaItemC.Columna.Editable = false;
            ColumnaDesc.Columna.Editable = false;
            ColumnaFechaSol.Columna.Editable = false;
            ColumnaHoraSol.Columna.Editable = false;
            ColumnaItemR.Columna.Editable = false;
            ColumnaDescR.Columna.Editable = false;
            ColumnaFechaResp.Columna.Editable = false;
            ColumnaHoraResp.Columna.Editable = false;
            ColumnaUsarioRes.Columna.Editable = false;
        }
    }
}

