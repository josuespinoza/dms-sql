using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBODocLegales : MatrixSBO
    {
        public MatrixSBODocLegales(string uniqueId, IForm formularioSBO, string tablaligada ) 
            : base (uniqueId,formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaTipoGestion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaTipoEvento { get; set; }
        public ColumnaMatrixSBOEditText<DateTime> ColumnaFechaEvento { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia1 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia2 { get; set; }
        public ColumnaMatrixSBOCheckBox<string> ColumnaPrenda { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaInstFinanciera { get; set; }
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
            ColumnaFechaEvento.AsignaBinding();
            ColumnaNoReferencia1.AsignaBinding();
            ColumnaNoReferencia2.AsignaBinding();
            ColumnaPrenda.AsignaBinding();
            ColumnaInstFinanciera.AsignaBinding();
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
            ColumnaTipoGestion = new ColumnaMatrixSBOEditText<string>("col_GestDL", true, "U_Gestion", this);
            ColumnaTipoEvento = new ColumnaMatrixSBOEditText<string>("col_EvenDL", true, "U_Evento", this);
            ColumnaFechaEvento = new ColumnaMatrixSBOEditText<DateTime>("col_FchEDL", true, "U_Fech_Ev", this);
            ColumnaNoReferencia1 = new ColumnaMatrixSBOEditText<string>("col_NoR1DL", true, "U_Num_Ref1", this);
            ColumnaNoReferencia2 = new ColumnaMatrixSBOEditText<string>("col_NoR2DL", true, "U_Num_Ref2", this);
            ColumnaPrenda = new ColumnaMatrixSBOCheckBox<string>("col_PrenDL", true, "U_Prenda", this);
            ColumnaObservaciones = new ColumnaMatrixSBOEditText<string>("col_ObvsDL", true, "U_Observ", this);
            ColumnaInstFinanciera = new ColumnaMatrixSBOEditText<string>("col_InsFDL", true, "U_Ins_Fin", this);
            ColumnaIngresado = new ColumnaMatrixSBOEditText<string>("col_IngrDL", true, "U_Ingresa", this);
            ColumnaModificado = new ColumnaMatrixSBOEditText<string>("col_ModfDL", true, "U_Modific", this);
            ColumnaCodigoGestion = new ColumnaMatrixSBOEditText<string>("col_CodGes",true,"U_Cod_Ges",this);
            ColumnaCodigoEvento = new ColumnaMatrixSBOEditText<string>("col_CodEve", true, "U_Cod_Eve", this);
            ColumnaFechaCreacion = new ColumnaMatrixSBOEditText<string>("col_FchCre", true, "U_Fech_Cre", this);
            ColumnaFechaModificacion = new ColumnaMatrixSBOEditText<string>("col_FchMod", true, "U_Fech_Mod", this);
        }
    }
}
