using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOReportes : MatrixSBO
    {
        public MatrixSBOReportes(string uniqueId, IForm formularioSBO, string tablaligada)
            : base(uniqueId, formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaId { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaName { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDescripcion { get; set; }

        public override void LigaColumnas()
        {
            ColumnaId.AsignaBindingDataTable();
            ColumnaName.AsignaBindingDataTable();
            ColumnaDescripcion.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaId = new ColumnaMatrixSBOEditText<string>("col_Id", true, "codeR", this);
            ColumnaName = new ColumnaMatrixSBOEditText<string>("col_Name", true, "nameR", this);
            ColumnaDescripcion = new ColumnaMatrixSBOEditText<string>("col_Rept", true, "descripR", this);
        }

    }
}
