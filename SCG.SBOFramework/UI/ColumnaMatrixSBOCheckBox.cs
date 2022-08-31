using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class ColumnaMatrixSBOCheckBox<TTipoValor>:ColumnaMatrixSBO<TTipoValor> 
    {
        public ColumnaMatrixSBOCheckBox(string uniqueId, MatrixSBO matrixSBO)
            : this(uniqueId, false, string.Empty, matrixSBO)
        {
        }

        public ColumnaMatrixSBOCheckBox(string uniqueId, string columnaLigada) : base(uniqueId, columnaLigada)
        {
        }

        public ColumnaMatrixSBOCheckBox(string uniqueId, bool ligada, string columnaLigada, MatrixSBO matrixSBO) : base(uniqueId, ligada, columnaLigada, matrixSBO)
        {
        }

        public override string ObtieneValorColumnaMatrix(int fila)
        {
            
            return ((ICheckBox)ObtieneColumnaMatrixUIInterno(fila)).Caption;
        }

        public override string ObtieneValorColumnaDataTable(int fila, DBDataSource dbDataSource)
        {
            return ObtieneColumnaMatrixDataTableInterno(fila, dbDataSource);
        }

        public override void AsignaValorUI(TTipoValor valor, int fila)
        {
            var edit = (ICheckBox) ObtieneColumnaMatrixUIInterno(fila);
            edit.Caption = valor.ToString();
        }

        public override void AsignaValorDataSource(TTipoValor valor, int fila, DBDataSource dbDataSource)
        {
            dbDataSource.SetValue(ColumnaLigada, fila, valor.ToString());
        }

        public override void AsignaValorDataSource(TTipoValor valor, int fila)
        {
            AsignaValorDataSource(valor, fila, MatrixSBO.FormularioSBO.DataSources.DBDataSources.Item(TablaLigada));
        }
    }
}