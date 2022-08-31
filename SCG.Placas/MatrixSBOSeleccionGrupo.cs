using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Placas
{
    public class MatrixSBOSeleccionGrupo : MatrixSBO
    {
        public MatrixSBOSeleccionGrupo(string uniqueId, IForm formularioSBO, string tablaligada)
            : base(uniqueId, formularioSBO)
        {
            TablaLigada = tablaligada;
        }

        public ColumnaMatrixSBOCheckBox<string> ColumnaSeleccion { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumChasis { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumMotor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaMarca { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaEstilo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaModelo { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaColor { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaAnno { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaUnidad { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaContratoV { get; set; }
        public ColumnaMatrixSBOEditText<string> ColumnaNumFactura { get; set; }

        public override void LigaColumnas()
        {
            ColumnaSeleccion.AsignaBindingDataTable();
            ColumnaNumChasis.AsignaBindingDataTable();
            ColumnaNumMotor.AsignaBindingDataTable();
            ColumnaMarca.AsignaBindingDataTable();
            ColumnaEstilo.AsignaBindingDataTable();
            ColumnaModelo.AsignaBindingDataTable();
            ColumnaColor.AsignaBindingDataTable();
            ColumnaAnno.AsignaBindingDataTable();
            ColumnaUnidad.AsignaBindingDataTable();
            ColumnaContratoV.AsignaBindingDataTable();
            ColumnaNumFactura.AsignaBindingDataTable();
        }

        public override void CreaColumnas()
        {
            ColumnaSeleccion = new ColumnaMatrixSBOCheckBox<string>("col_Selec", true, "seleccionS", this);
            ColumnaNumChasis = new ColumnaMatrixSBOEditText<string>("col_Chasis", true, "numChasisS", this);
            ColumnaNumMotor = new ColumnaMatrixSBOEditText<string>("col_Motor", true, "numMotorS", this);
            ColumnaMarca = new ColumnaMatrixSBOEditText<string>("col_Marca", true, "marcaS", this);
            ColumnaEstilo = new ColumnaMatrixSBOEditText<string>("col_Estilo", true, "estiloS", this);
            ColumnaModelo = new ColumnaMatrixSBOEditText<string>("col_Modelo", true, "modeloS", this);
            ColumnaColor = new ColumnaMatrixSBOEditText<string>("col_Color", true, "colorS", this);
            ColumnaAnno = new ColumnaMatrixSBOEditText<string>("col_Anno", true, "annoS", this);
            ColumnaUnidad = new ColumnaMatrixSBOEditText<string>("col_Unidad", true, "unidadS", this);
            ColumnaContratoV = new ColumnaMatrixSBOEditText<string>("col_ContV", true, "contVentaS", this);
            ColumnaNumFactura = new ColumnaMatrixSBOEditText<string>("col_NumF", true, "numFactS", this);
        }
    }
}
