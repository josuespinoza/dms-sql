using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOGastos : MatrixSBO
    {
        public MatrixSBOGastos(string uniqueId, IForm FormularioSBO, string tablaligada)
            : base(uniqueId, FormularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaTipoGasto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoDocumento { get; set; }
        public ColumnaMatrixSBOEditText<DateTime> ColumnaFechaDocumento { get; set; }
        public ColumnaMatrixSBOEditText<float> ColumnaMonto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaObservaciones { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIngresado { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModificado { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoGasto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaCreacion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaModificacion { get; set; }
        
        public override void LigaColumnas()
        {
            ColumnaTipoGasto.AsignaBinding();
            ColumnaNoDocumento.AsignaBinding();
            ColumnaFechaDocumento.AsignaBinding();
            ColumnaMonto.AsignaBinding();
            ColumnaObservaciones.AsignaBinding();
            ColumnaIngresado.AsignaBinding();
            ColumnaModificado.AsignaBinding();
            ColumnaCodigoGasto.AsignaBinding();
            ColumnaFechaCreacion.AsignaBinding();
            ColumnaFechaModificacion.AsignaBinding();
        }

        public override void CreaColumnas()
        {
            ColumnaTipoGasto = new ColumnaMatrixSBOEditText<string>("col_GastoG", true, "U_Gasto", this);
            ColumnaNoDocumento = new ColumnaMatrixSBOEditText<string>("col_NoDocG", true, "U_Num_Doc", this);
            ColumnaFechaDocumento = new ColumnaMatrixSBOEditText<DateTime>("col_FchDoG", true, "U_Fech_Doc", this);
            ColumnaMonto = new ColumnaMatrixSBOEditText<float>("col_MontoG", true, "U_Monto", this);
            ColumnaObservaciones = new ColumnaMatrixSBOEditText<string>("col_ObvsG", true, "U_Observ", this);
            ColumnaIngresado = new ColumnaMatrixSBOEditText<string>("col_IngrG", true, "U_Ingresa", this);
            ColumnaModificado = new ColumnaMatrixSBOEditText<string>("col_ModifG", true, "U_Modific", this);
            ColumnaCodigoGasto = new ColumnaMatrixSBOEditText<string>("col_CodGas", true, "U_Cod_Gas", this);
            ColumnaFechaCreacion = new ColumnaMatrixSBOEditText<string>("col_FchCre", true, "U_Fech_Cre", this);
            ColumnaFechaModificacion = new ColumnaMatrixSBOEditText<string>("col_FchMod", true, "U_Fech_Mod", this);
        }
    }
}
