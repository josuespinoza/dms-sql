using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOPrestamo : IEncabezadoUDO
    {

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("DocNum", SoloLectura = true)]
        public int DocNum { get; set; }

        [UDOBind("U_Cont_Ven")]
        public string CodigoContrato { get; set; }

        [UDOBind("U_Cod_Cli")]
        public string CodigoCliente { get; set; }

        [UDOBind("U_Des_Cli")]
        public string DescripcionCliente { get; set; }

        [UDOBind("U_Cod_Emp")]
        public string CodigoEmpleado { get; set; }

        [UDOBind("U_Des_Emp")]
        public string DescripcionEmpleado { get; set; }

        [UDOBind("U_Moneda")]
        public string Moneda { get; set; }

        [UDOBind("U_Pre_Vta")]
        public double PrecioVenta { get; set; }

        [UDOBind("U_Ent_Fin")]
        public string EnteFinanciero { get; set; }

        [UDOBind("U_Mon_Fin")]
        public double MontoFinanciar { get; set; }

        [UDOBind("U_Interes")]
        public double Interes { get; set; }

        [UDOBind("U_Plazo")]
        public int Plazo { get; set; }

        [UDOBind("U_Fec_Pres")]
        public DateTime FechaPrestamo { get; set; }

        [UDOBind("U_DiaPago")]
        public int DiaPago { get; set; }

        [UDOBind("U_Int_Mora")]
        public double InteresMoratorio { get; set; }

        [UDOBind("U_Tipo_Cuo")]
        public string TipoCuota { get; set; }

        [UDOBind("U_Estado")]
        public string Estado { get; set; }

        [UDOBind("U_Des_Mon")]
        public string DescMoneda { get; set; }

        [UDOBind("U_Des_Est")]
        public string DescEstado { get; set; }

        [UDOBind("U_Des_Tipo")]
        public string DescTipo { get; set; }

        [UDOBind("U_Asiento")]
        public string Asiento { get; set; }

        [UDOBind("U_Cod_Unid")]
        public string Unidad { get; set; }

        [UDOBind("U_Prima")]
        public double Prima { get; set; }

        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_PRESTAMO"; }
        }

        #endregion

    }
}
