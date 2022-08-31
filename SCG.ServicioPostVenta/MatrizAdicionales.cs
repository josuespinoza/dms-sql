using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class MatrizAdicionales : MatrixSBO
    {
        public MatrizAdicionales(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaSele { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCode { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaBode { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCSto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCant { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaPrec { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMone { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDura { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoFa { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodBar { get; set; } 
        
        public override void LigaColumnas()
        {
            ColumnaSele.AsignaBindingDataTable();
            ColumnaCode.AsignaBindingDataTable();
            ColumnaDesc.AsignaBindingDataTable();
            ColumnaBode.AsignaBindingDataTable();
            ColumnaCSto.AsignaBindingDataTable();
            ColumnaCant.AsignaBindingDataTable();
            ColumnaPrec.AsignaBindingDataTable();
            ColumnaMone.AsignaBindingDataTable();
            ColumnaDura.AsignaBindingDataTable();
            ColumnaNoFa.AsignaBindingDataTable();
            ColumnaCodBar.AsignaBindingDataTable(); 
        }

        public override void CreaColumnas()
        {
            ColumnaSele = new ColumnaMatrixSBOEditText<string>("Col_sele", true, "sele", this);
            ColumnaCode = new ColumnaMatrixSBOEditText<string>("Col_code", true, "code", this);
            ColumnaDesc = new ColumnaMatrixSBOEditText<string>("Col_desc", true, "desc", this);
            ColumnaBode = new ColumnaMatrixSBOEditText<string>("Col_bode", true, "bode", this);
            ColumnaCSto = new ColumnaMatrixSBOEditText<string>("Col_csto", true, "csto", this);
            ColumnaCant = new ColumnaMatrixSBOEditText<string>("Col_cant", true, "cant", this);
            ColumnaPrec = new ColumnaMatrixSBOEditText<string>("Col_prec", true, "prec", this);
            ColumnaMone = new ColumnaMatrixSBOEditText<string>("Col_mone", true, "mone", this);
            ColumnaDura = new ColumnaMatrixSBOEditText<string>("Col_dura", true, "dura", this);
            ColumnaNoFa = new ColumnaMatrixSBOEditText<string>("Col_nofa", true, "nofa", this);
            ColumnaCodBar = new ColumnaMatrixSBOEditText<string>("Col_CodBar", true, "CodBar", this);

            // Se Bloquean las columnas
            ColumnaCode.Columna.Editable = false;
            ColumnaBode.Columna.Editable = false;
            ColumnaCSto.Columna.Editable = false;
            ColumnaMone.Columna.Editable = false;
            ColumnaCodBar.Columna.Editable = false;

            // Se Oculta
            ColumnaNoFa.Columna.Visible = false;
        }
    }
}
