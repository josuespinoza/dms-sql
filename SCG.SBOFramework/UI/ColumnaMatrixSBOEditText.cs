using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class ColumnaMatrixSBOEditText<TTipoValor>:ColumnaMatrixSBO<TTipoValor> 
    {
        public ColumnaMatrixSBOEditText(string uniqueId, MatrixSBO matrixSBO)
            : this(uniqueId, false, string.Empty, matrixSBO)
        {
        }

        public ColumnaMatrixSBOEditText(string uniqueId, string columnaLigada) : base(uniqueId, columnaLigada)
        {
        }

        public ColumnaMatrixSBOEditText(string uniqueId, bool ligada, string columnaLigada, MatrixSBO matrixSBO) : base(uniqueId, ligada, columnaLigada, matrixSBO)
        {
        }

        public override string ObtieneValorColumnaMatrix(int fila)
        {
            
            return ((IEditText)ObtieneColumnaMatrixUIInterno(fila)).Value;
        }

        public override string ObtieneValorColumnaDataTable(int fila, DBDataSource dbDataSource)
        {
            return ObtieneColumnaMatrixDataTableInterno(fila, dbDataSource);
        }

        public override void AsignaValorUI(TTipoValor valor, int fila)
        {
            var edit = (IEditText) ObtieneColumnaMatrixUIInterno(fila);
            edit.Value = valor.ToString();
        }

        public override void AsignaValorDataSource(TTipoValor valor, int fila, DBDataSource dbDataSource)
        {
            float valorNumerico;
            dbDataSource.SetValue(ColumnaLigada, fila,
                                  float.TryParse(valor.ToString(), out valorNumerico)
                                      ? valorNumerico.ToString(MatrixSBO.NumberFormatInfo)
                                      : valor.ToString());
        }

        public override void AsignaValorDataSource(TTipoValor valor, int fila)
        {
            AsignaValorDataSource(valor, fila, MatrixSBO.FormularioSBO.DataSources.DBDataSources.Item(TablaLigada));
        }

        public override void AsignaValorDataTable(TTipoValor valor, int fila)
        {
        }
    }
}