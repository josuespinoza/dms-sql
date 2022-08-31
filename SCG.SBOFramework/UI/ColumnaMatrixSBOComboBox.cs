using System;
using System.Collections.Generic;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class ColumnaMatrixSBOComboBox<TTipoValor> : ColumnaMatrixSBO<TTipoValor>
    {
        public ColumnaMatrixSBOComboBox(string uniqueId, MatrixSBO matrixSBO)
            : this(uniqueId, false, string.Empty, matrixSBO)
        {
        }

        public ColumnaMatrixSBOComboBox(string uniqueId, string columnaLigada)
            : base(uniqueId, columnaLigada)
        {
        }

        public ColumnaMatrixSBOComboBox(string uniqueId, bool ligada, string columnaLigada, MatrixSBO matrixSBO)
            : base(uniqueId, ligada, columnaLigada, matrixSBO)
        {
        }

        public override string ObtieneValorColumnaMatrix(int fila)
        {

            return ((IComboBox)ObtieneColumnaMatrixUIInterno(fila)).Value;
        }

        public override string ObtieneValorColumnaDataTable(int fila, DBDataSource dbDataSource)
        {
            return ObtieneColumnaMatrixDataTableInterno(fila, dbDataSource);
        }

        public override void AsignaValorUI(TTipoValor valor, int fila)
        {
            var oCombo = (IComboBox)ObtieneColumnaMatrixUIInterno(fila);
            oCombo.Select(valor);
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