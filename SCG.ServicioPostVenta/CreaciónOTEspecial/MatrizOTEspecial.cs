using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta.CreaciónOTEspecial
{
    public class MatrizOTEspecial : MatrixSBO
    {
        public MatrizOTEspecial(string UniqueId, IForm formularioSBO, string tablaLigada)
            : base(UniqueId, formularioSBO)
        {
            TablaLigada = tablaLigada;
        }
        public ColumnaMatrixSBOCheckBox<string> _columnaCol_sel { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Code { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Name { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Qty { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Curr { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Price { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Obs { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_PorcDesc { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_IdRepXOrd { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Costo { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_IndImpuestos { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CPen { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CSol { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CRec { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CPDe { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CPTr { get; set; }
        public ColumnaMatrixSBOEditText<Decimal> _columnaCol_CPBo { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_Compra { get; set; }
        public ColumnaMatrixSBOEditText<string> _columnaCol_IDLineas { get; set; }

        public override void LigaColumnas()
        {
            _columnaCol_sel.AsignaBindingDataTable();
            _columnaCol_Code.AsignaBindingDataTable();
            _columnaCol_Name.AsignaBindingDataTable();
            _columnaCol_Qty.AsignaBindingDataTable();
            _columnaCol_Curr.AsignaBindingDataTable();
            _columnaCol_Price.AsignaBindingDataTable();
            _columnaCol_Obs.AsignaBindingDataTable();
            _columnaCol_PorcDesc.AsignaBindingDataTable();
            _columnaCol_IdRepXOrd.AsignaBindingDataTable();
            _columnaCol_Costo.AsignaBindingDataTable();
            _columnaCol_IndImpuestos.AsignaBindingDataTable();
            _columnaCol_CPen.AsignaBindingDataTable();
            _columnaCol_CSol.AsignaBindingDataTable();
            _columnaCol_CRec.AsignaBindingDataTable();
            _columnaCol_CPDe.AsignaBindingDataTable();
            _columnaCol_CPTr.AsignaBindingDataTable();
            _columnaCol_CPBo.AsignaBindingDataTable();
            _columnaCol_Compra.AsignaBindingDataTable();
            _columnaCol_IDLineas.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            _columnaCol_sel = new ColumnaMatrixSBOCheckBox<string>("col_Sel", true, "col_Sel", this);
            _columnaCol_Code = new ColumnaMatrixSBOEditText<string>("col_Code", true, "col_Code", this);
            _columnaCol_Name = new ColumnaMatrixSBOEditText<string>("col_Name", true, "col_Name", this);
            _columnaCol_Qty = new ColumnaMatrixSBOEditText<string>("col_Quant", true, "col_Quant", this);
            _columnaCol_Curr = new ColumnaMatrixSBOEditText<string>("col_Curr", true, "col_Curr", this);
            _columnaCol_Price = new ColumnaMatrixSBOEditText<string>("col_Price", true, "col_Price", this);
            _columnaCol_Obs = new ColumnaMatrixSBOEditText<string>("col_Obs", true, "col_Obs", this);
            _columnaCol_PorcDesc = new ColumnaMatrixSBOEditText<string>("col_PrcDes", true, "col_PrcDes", this);
            _columnaCol_IdRepXOrd = new ColumnaMatrixSBOEditText<string>("col_IdRXOr", true, "col_IdRXOr", this);
            _columnaCol_Costo = new ColumnaMatrixSBOEditText<string>("col_Costo", true, "col_Costo", this);
            _columnaCol_IndImpuestos = new ColumnaMatrixSBOEditText<string>("col_IndImp", true, "col_IndImp", this);
            _columnaCol_CPen = new ColumnaMatrixSBOEditText<Decimal>("col_CPend", true, "col_CPend", this);
            _columnaCol_CSol = new ColumnaMatrixSBOEditText<Decimal>("col_CSol", true, "col_CSol", this);
            _columnaCol_CRec = new ColumnaMatrixSBOEditText<Decimal>("col_CRec", true, "col_CRec", this);
            _columnaCol_CPDe = new ColumnaMatrixSBOEditText<Decimal>("col_PenDev", true, "col_PenDev", this);
            _columnaCol_CPTr = new ColumnaMatrixSBOEditText<Decimal>("col_PenTra", true, "col_PenTra", this);
            _columnaCol_CPBo = new ColumnaMatrixSBOEditText<Decimal>("col_PenBod", true, "col_PenBod", this);
            _columnaCol_Compra = new ColumnaMatrixSBOEditText<string>("col_Compra", true, "col_Compra", this);
            _columnaCol_IDLineas = new ColumnaMatrixSBOEditText<string>("col_IDLine", true, "col_IDLine", this);
        }
    }
}
