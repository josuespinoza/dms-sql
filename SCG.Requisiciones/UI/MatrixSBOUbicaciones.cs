using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones.UI
{
    public class MatrixSBOUbicaciones : MatrixSBO
    {
        #region ...Propiedades...
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoUbicacion { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaDescripcionUbicacion { get; private set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCantidadDisponible { get; private set; }
        #endregion

        #region ...Constructor...
        public MatrixSBOUbicaciones(string uniqueId, IForm formularioSBO, string tablaLigada)
            : base(uniqueId, formularioSBO)
        {
            this.TablaLigada = tablaLigada;
        }
        #endregion
        
        #region ...Metodos...

        public override void LigaColumnas()
        {
            ColumnaCodigoUbicacion.AsignaBindingDataTable();
            ColumnaDescripcionUbicacion.AsignaBindingDataTable();
            ColumnaCantidadDisponible.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaCodigoUbicacion = new ColumnaMatrixSBOEditText<string>("colCodUbi", true, "colCodUbi", this);
            ColumnaDescripcionUbicacion = new ColumnaMatrixSBOEditText<string>("colDesUbi", true, "colDesUbi", this);
            ColumnaCantidadDisponible = new ColumnaMatrixSBOEditText<string>("colQtyHnd", true, "colQtyHnd", this);
        }

        #endregion

    }
}
