using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public abstract class ColumnaMatrixSBO<TTipoValor>
    {
        protected ColumnaMatrixSBO(string uniqueId, MatrixSBO matrixSBO) : this(uniqueId, false, string.Empty, matrixSBO)
        {
        }

        protected ColumnaMatrixSBO(string uniqueId, string columnaLigada) : this(uniqueId, true, columnaLigada, null)
        {
        }

        protected ColumnaMatrixSBO(string uniqueId, bool ligada, string columnaLigada, MatrixSBO matrixSBO)
        {
            UniqueId = uniqueId;
            Ligada = ligada;
            ColumnaLigada = columnaLigada;
            TablaLigada = matrixSBO.TablaLigada;
            MatrixSBO = matrixSBO;
        }


        public string UniqueId { get; private set; }
        public bool Ligada { get; set; }
        public string ColumnaLigada { get; set; }
        public string TablaLigada { get; set; }
        public MatrixSBO MatrixSBO { get; set; }

        public IColumn Columna
        {
            get { return MatrixSBO != null ? MatrixSBO.Especifico.Columns.Item(UniqueId) : null; }
        }

        public void AsignaBinding()
        {
            if (MatrixSBO != null)
                MatrixSBO.Especifico.Columns.Item(UniqueId).DataBind.SetBound(Ligada, TablaLigada,
                                                                              ColumnaLigada);
        }

        public void AsignaBindingDataTable()
        {
            if (MatrixSBO != null)
                MatrixSBO.Especifico.Columns.Item(UniqueId).DataBind.Bind(MatrixSBO.TablaLigada, ColumnaLigada);
        }

        public virtual object ObtieneColumnaMatrixUIInterno(int fila)
        {
            return MatrixSBO != null ? MatrixSBO.Especifico.Columns.Item(UniqueId).Cells.Item(fila).Specific : null;
        }

        protected virtual string ObtieneColumnaMatrixDataTableInterno(int fila, DBDataSource dbDataSource)
        {
            return dbDataSource != null ? dbDataSource.GetValue(ColumnaLigada, fila).TrimEnd() : string.Empty;
        }

//        public void AsignaValorUI(string valor)
//        {
//            throw new NotSupportedException("This method is not supported for this class");
//        }
//
//        public void AsignaValorDataSource(string valor)
//        {
//            throw new NotSupportedException("This method is not supported for this class");
//        }

        public abstract string ObtieneValorColumnaMatrix(int fila);
        public abstract string ObtieneValorColumnaDataTable(int fila, DBDataSource dbDataSource);
        public abstract void AsignaValorUI(TTipoValor valor, int fila);
        public abstract void AsignaValorDataSource(TTipoValor valor, int fila, DBDataSource dataTable);
        public abstract void AsignaValorDataSource(TTipoValor valor, int fila);
        public virtual void AsignaValorDataTable(TTipoValor valor, int fila)
        {
            var dataTable = MatrixSBO.FormularioSBO.DataSources.DataTables.Item(MatrixSBO.TablaLigada);
            dataTable.SetValue(ColumnaLigada, fila, valor);
        }

        public virtual TTipoValor ObtieneValorDataTable(int fila)
        {
            var dataTable = MatrixSBO.FormularioSBO.DataSources.DataTables.Item(MatrixSBO.TablaLigada);
            return (TTipoValor)dataTable.GetValue(ColumnaLigada, fila);
        }
    }
}