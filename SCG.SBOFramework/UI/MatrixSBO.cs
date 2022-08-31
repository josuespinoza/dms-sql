using System.Collections.Generic;
using System.Globalization;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public abstract class MatrixSBO : ControlSBO<IMatrix>
    {
        public string TablaLigada { get; set; }

        protected MatrixSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        protected MatrixSBO(string uniqueId) : base(uniqueId)
        {
        }

        protected MatrixSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public void AsignaValorColumnaEditText(string valor, string nombreColumna, int posicion)
        {
            ((EditText) Especifico.Columns.Item(nombreColumna).Cells.Item(posicion).Specific).Value = valor;
        }

        public string ObtieneValorColumnaEditText(string nombreColumna, int posicion)
        {
            return ((EditText) Especifico.Columns.Item(nombreColumna).Cells.Item(posicion).Specific).Value;
        }

        public IMatrix Matrix 
        {
            get { return Especifico; }
        }

        public NumberFormatInfo NumberFormatInfo { get; set; }

        public abstract void LigaColumnas();
        public abstract void CreaColumnas();

    }
}