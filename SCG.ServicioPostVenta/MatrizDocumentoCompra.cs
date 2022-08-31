using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizDocumentoCompra : MatrixSBO
    {
        public MatrizDocumentoCompra(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaSele { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCode { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCant { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAlma { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPrec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMone { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIdIt { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTax { get; set; }
        
        public override void LigaColumnas()
        {
            ColumnaSele.AsignaBindingDataTable();
            ColumnaCode.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
            ColumnaAlma.AsignaBindingDataTable();
            ColumnaPrec.AsignaBindingDataTable();
            ColumnaMone.AsignaBindingDataTable();
            ColumnaIdIt.AsignaBindingDataTable();
            ColumnaTax.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaSele = new ColumnaMatrixSBOEditText<string>("Col_sele", true, "sele", this);
            ColumnaCode = new ColumnaMatrixSBOEditText<string>("Col_code", true, "code", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("Col_desc", true, "desc", this);
            ColumnaCant = new ColumnaMatrixSBOEditText<string>("Col_cant", true, "cant", this);
            ColumnaAlma = new ColumnaMatrixSBOEditText<string>("Col_alma", true, "alma", this);
            ColumnaPrec = new ColumnaMatrixSBOEditText<string>("Col_prec", true, "prec", this);
            ColumnaMone = new ColumnaMatrixSBOEditText<string>("Col_mone", true, "mone", this);
            ColumnaTax = new ColumnaMatrixSBOEditText<string>("Col_tax", true, "tax", this);
            ColumnaIdIt = new ColumnaMatrixSBOEditText<string>("Col_idit", true, "idit", this);
        }
    }
}
