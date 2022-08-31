using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOGastosGrupo : MatrixSBO
    {
        public MatrixSBOGastosGrupo(string uniqueId, IForm formularioSBO, string tablaligada)
            : base(uniqueId, formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOEditText<string> ColumnaTipoGasto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumChasis { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumMotor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstilo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNoDocumento { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaFechDocumento { get; set; }
        public ColumnaMatrixSBOEditText<float> ColumnaMonto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaObservaciones { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaCodigoGasto { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaUnidad { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMarca { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModelo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaColor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAnno { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaContratoV { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumFactura { get; set; }

        public override void LigaColumnas()
        {
            ColumnaTipoGasto.AsignaBindingDataTable();
            ColumnaNumChasis.AsignaBindingDataTable();
            ColumnaNumMotor.AsignaBindingDataTable();
            ColumnaEstilo.AsignaBindingDataTable();
            ColumnaNoDocumento.AsignaBindingDataTable();
            ColumnaFechDocumento.AsignaBindingDataTable();
            ColumnaMonto.AsignaBindingDataTable();
            ColumnaObservaciones.AsignaBindingDataTable();
            ColumnaCodigoGasto.AsignaBindingDataTable();
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
            ColumnaTipoGasto = new ColumnaMatrixSBOEditText<string>("col_TipoG", true, "tipoGastoG", this);
            ColumnaNumChasis = new ColumnaMatrixSBOEditText<string>("col_Num_Ch", true, "numChasisG", this);
            ColumnaNumMotor = new ColumnaMatrixSBOEditText<string>("col_Num_Mo", true, "numMotorG", this);
            ColumnaEstilo = new ColumnaMatrixSBOEditText<string>("col_Estilo", true, "estiloG", this);
            ColumnaNoDocumento = new ColumnaMatrixSBOEditText<string>("col_No_Doc", true, "numDocumG", this);
            ColumnaFechDocumento = new ColumnaMatrixSBOEditText<string>("col_Fch_Do", true, "fechaDocumG", this);
            ColumnaMonto = new ColumnaMatrixSBOEditText<float>("col_Monto", true, "montoG", this);
            ColumnaObservaciones = new ColumnaMatrixSBOEditText<string>("col_Observ", true, "observG", this);
            ColumnaCodigoGasto = new ColumnaMatrixSBOEditText<string>("col_CodGas", true, "CodGastG", this); 
            ColumnaUnidad = new ColumnaMatrixSBOEditText<string>("col_Unidad", true, "unidadG", this);
            ColumnaMarca = new ColumnaMatrixSBOEditText<string>("col_Marca", true, "marcaG", this);
            ColumnaModelo = new ColumnaMatrixSBOEditText<string>("col_Modelo", true, "modeloG", this);
            ColumnaColor = new ColumnaMatrixSBOEditText<string>("col_Color", true, "colorG", this);
            ColumnaAnno = new ColumnaMatrixSBOEditText<string>("col_Anno", true, "annoG", this);
            ColumnaContratoV = new ColumnaMatrixSBOEditText<string>("col_ContV", true, "contVentaG", this);
            ColumnaNumFactura = new ColumnaMatrixSBOEditText<string>("col_NumF", true, "numFactG", this);
        }
    }
}
