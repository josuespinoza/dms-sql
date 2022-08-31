using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizTrackingRepuestos : MatrixSBO
    {
        public ColumnaMatrixSBOEditText<string> ColumnaProv { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechSol { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoDocD { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoDoc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoDoc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaObserv { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCantRec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCantSol { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDocEntry { get; set; }

        public MatrizTrackingRepuestos(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public override void LigaColumnas()
        {
            ColumnaProv.AsignaBindingDataTable();
            ColumnaFechSol.AsignaBindingDataTable();
            ColumnaTipoDocD.AsignaBindingDataTable();
            ColumnaTipoDoc.AsignaBindingDataTable();
            ColumnaNoDoc.AsignaBindingDataTable();
            ColumnaDocEntry.AsignaBindingDataTable();
            ColumnaObserv.AsignaBindingDataTable();
            ColumnaCantRec.AsignaBindingDataTable();
            if (ColumnaCantSol != null) ColumnaCantSol.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaProv = new ColumnaMatrixSBOEditText<string>("Col_Prov", true, "Prov", this);
            ColumnaFechSol = new ColumnaMatrixSBOEditText<string>("Col_FeSo", true, "FeSo", this);
            ColumnaTipoDocD = new ColumnaMatrixSBOEditText<string>("Col_TDocD", true, "TDocD", this);
            ColumnaTipoDoc = new ColumnaMatrixSBOEditText<string>("Col_TDoc", true, "TDoc", this);
            ColumnaNoDoc = new ColumnaMatrixSBOEditText<string>("Col_NDoc", true, "DocN", this);
            ColumnaObserv = new ColumnaMatrixSBOEditText<string>("Col_Obs", true, "Obse", this);
            ColumnaCantRec = new ColumnaMatrixSBOEditText<string>("Col_CRec", true, "CanEn", this);
            ColumnaCantSol = new ColumnaMatrixSBOEditText<string>("Col_CSol", true, "CanSo", this);
            ColumnaDocEntry = new ColumnaMatrixSBOEditText<string>("ColID", true, "DocE", this);
        }
    }
}
