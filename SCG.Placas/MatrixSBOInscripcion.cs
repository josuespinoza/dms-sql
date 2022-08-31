using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOInscripcion : MatrixSBO
    {
        public MatrixSBOInscripcion(string uniqueId, IForm formularioSBO, string tablaligada)
            : base(uniqueId, formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaTipoGestion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoEvento { get; set; }
        public ColumnaMatrixSBOEditText<DateTime> ColumnaFechaEveto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia1 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia2 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaObservaciones { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaIngresado { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModificado { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoGestion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoEvento { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaCreacion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaModificacion { get; set; }


        public override void LigaColumnas()
        {
            ColumnaTipoGestion.AsignaBinding();
            ColumnaTipoEvento.AsignaBinding();
            ColumnaFechaEveto.AsignaBinding();
            ColumnaNoReferencia1.AsignaBinding();
            ColumnaNoReferencia2.AsignaBinding();
            ColumnaObservaciones.AsignaBinding();
            ColumnaIngresado.AsignaBinding();
            ColumnaModificado.AsignaBinding();
            ColumnaCodigoGestion.AsignaBinding();
            ColumnaCodigoEvento.AsignaBinding();
            ColumnaFechaCreacion.AsignaBinding();
            ColumnaFechaModificacion.AsignaBinding();
        }

        public override void CreaColumnas()
        {
            ColumnaTipoGestion = new ColumnaMatrixSBOEditText<string>("col_GestSI", true, "U_Gestion", this);
            ColumnaTipoEvento = new ColumnaMatrixSBOEditText<string>("col_EvenSI", true, "U_Evento", this);
            ColumnaFechaEveto = new ColumnaMatrixSBOEditText<DateTime>("col_FchESI", true, "U_Fech_Ev", this);
            ColumnaNoReferencia1 = new ColumnaMatrixSBOEditText<string>("col_NoR1SI", true, "U_Num_Ref1", this);
            ColumnaNoReferencia2 = new ColumnaMatrixSBOEditText<string>("col_NoR2SI", true, "U_Num_Ref2", this);
            ColumnaObservaciones = new ColumnaMatrixSBOEditText<string>("col_ObvsSI", true, "U_Observ", this);
            ColumnaIngresado = new ColumnaMatrixSBOEditText<string>("col_IngrSI", true, "U_Ingresa", this);
            ColumnaModificado = new ColumnaMatrixSBOEditText<string>("col_ModiSI", true, "U_Modific", this);
            ColumnaCodigoGestion = new ColumnaMatrixSBOEditText<string>("col_CodGes", true, "U_Cod_Ges", this);
            ColumnaCodigoEvento = new ColumnaMatrixSBOEditText<string>("col_CodEve", true, "U_Cod_Eve", this);
            ColumnaFechaCreacion = new ColumnaMatrixSBOEditText<string>("col_FchCre", true, "U_Fech_Cre", this);
            ColumnaFechaModificacion = new ColumnaMatrixSBOEditText<string>("col_FchMod", true, "U_Fech_Mod", this);
        }
    }
}
