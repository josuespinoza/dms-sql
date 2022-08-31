using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOEventosGrupo : MatrixSBO
    {
        public MatrixSBOEventosGrupo(string uniqueId, IForm formularioSBO, string tablaligada)
            : base(uniqueId, formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaNumChasis { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumMotor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstilo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaEvento { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia1 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia2 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia3 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia4 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia5 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoReferencia6 { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechaIngreso { get; set; }
        public ColumnaMatrixSBOCheckBox<string> ColumnaPrenda { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaInstFinanciera { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaObservaciones { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaUnidad { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMarca { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModelo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaColor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAnno { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaContratoV { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumFactura { get; set; }

        public override void LigaColumnas()
        {
            ColumnaNumChasis.AsignaBindingDataTable();
            ColumnaNumMotor.AsignaBindingDataTable();
            ColumnaEstilo.AsignaBindingDataTable();
            ColumnaFechaEvento.AsignaBindingDataTable();
            ColumnaNoReferencia1.AsignaBindingDataTable();
            ColumnaNoReferencia2.AsignaBindingDataTable();
            ColumnaNoReferencia3.AsignaBindingDataTable();
            ColumnaNoReferencia4.AsignaBindingDataTable();
            ColumnaNoReferencia5.AsignaBindingDataTable();
            ColumnaNoReferencia6.AsignaBindingDataTable();
            ColumnaFechaIngreso.AsignaBindingDataTable();
            ColumnaPrenda.AsignaBindingDataTable();
            ColumnaInstFinanciera.AsignaBindingDataTable();
            ColumnaObservaciones.AsignaBindingDataTable();
            ColumnaUnidad.AsignaBindingDataTable();
            ColumnaMarca.AsignaBindingDataTable();
            ColumnaModelo.AsignaBindingDataTable();
            ColumnaColor.AsignaBindingDataTable();
            ColumnaAnno.AsignaBindingDataTable();
            ColumnaContratoV.AsignaBindingDataTable();
            ColumnaNumFactura.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaNumChasis = new ColumnaMatrixSBOEditText<string>("col_Num_Ch", true, "numChasisE", this);
            ColumnaNumMotor = new ColumnaMatrixSBOEditText<string>("col_Num_Mo", true, "numMotorE", this);
            ColumnaEstilo = new ColumnaMatrixSBOEditText<string>("col_Estilo", true, "estiloE", this);
            ColumnaFechaEvento = new ColumnaMatrixSBOEditText<string>("col_Fch_Ev", true, "fechaEventoE", this);
            ColumnaNoReferencia1 = new ColumnaMatrixSBOEditText<string>("col_NRef1", true, "noRef1E", this);
            ColumnaNoReferencia2 = new ColumnaMatrixSBOEditText<string>("col_NRef2", true, "noRef2E", this);
            ColumnaNoReferencia3 = new ColumnaMatrixSBOEditText<string>("col_NRef3", true, "noRef3E", this);
            ColumnaNoReferencia4 = new ColumnaMatrixSBOEditText<string>("col_NRef4", true, "noRef4E", this);
            ColumnaNoReferencia5 = new ColumnaMatrixSBOEditText<string>("col_NRef5", true, "noRef5E", this);
            ColumnaNoReferencia6 = new ColumnaMatrixSBOEditText<string>("col_NRef6", true, "noRef6E", this);
            ColumnaFechaIngreso = new ColumnaMatrixSBOEditText<string>("col_FechIn", true, "fechaIngresoE", this);
            ColumnaPrenda = new ColumnaMatrixSBOCheckBox<string>("col_Prenda", true, "prendaE", this);
            ColumnaInstFinanciera = new ColumnaMatrixSBOEditText<string>("col_InsFin", true, "instFinanE", this);
            ColumnaObservaciones = new ColumnaMatrixSBOEditText<string>("col_Observ", true, "observE", this);
            ColumnaUnidad = new ColumnaMatrixSBOEditText<string>("col_Unidad", true, "unidadE", this);
            ColumnaMarca = new ColumnaMatrixSBOEditText<string>("col_Marca", true, "marcaE", this);
            ColumnaModelo = new ColumnaMatrixSBOEditText<string>("col_Modelo", true, "modeloE", this);
            ColumnaColor = new ColumnaMatrixSBOEditText<string>("col_Color", true, "colorE", this);
            ColumnaAnno = new ColumnaMatrixSBOEditText<string>("col_Anno", true, "annoE", this);
            ColumnaContratoV = new ColumnaMatrixSBOEditText<string>("col_ContV", true, "contVentaE", this);
            ColumnaNumFactura = new ColumnaMatrixSBOEditText<string>("col_NumF", true, "numFactE", this);
        }
    }
}
