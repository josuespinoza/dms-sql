using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    class MatrizProveedores: MatrixSBO
    {
        public MatrizProveedores(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaSele { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodi { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNomb { get; set; }
        
        public override void LigaColumnas()
        {
            ColumnaSele.AsignaBindingDataTable();
            ColumnaCodi.AsignaBindingDataTable();
            ColumnaNomb.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaSele = new ColumnaMatrixSBOEditText<string>("Col_sele", true, "sele", this);
            ColumnaCodi = new ColumnaMatrixSBOEditText<string>("Col_codi", true, "codi", this);
            ColumnaNomb = new ColumnaMatrixSBOEditText<string>("Col_nomb", true, "nomb", this);
        }
    }
}
