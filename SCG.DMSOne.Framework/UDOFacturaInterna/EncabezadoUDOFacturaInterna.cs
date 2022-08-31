using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOFacturaInterna : IEncabezadoUDO
    {
        public EncabezadoUDOFacturaInterna()
        {
            TablaLigada = "SCGD_FACTURAINTERNA";
        }

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("DocNum")]
        public int DocNum { get; set; }

        [UDOBind("U_No_OT")]
        public string NoOT { get; set; }

        [UDOBind("U_Cod_Unid")]
        public string CodigoUnidad { get; set; }

        [UDOBind("U_ID_Vehi")]
        public string CodigoVehiculo { get; set; }

        [UDOBind("U_Cod_Marc")]
        public string CodigoMarca { get; set; }

        [UDOBind("U_Cod_Esti")]
        public string CodigoEstilo { get; set; }

        [UDOBind("U_Cod_Mode")]
        public string CodigoModelo { get; set; }

        [UDOBind("U_CardCode")]
        public string CardCode { get; set; }

        [UDOBind("U_CardName")]
        public string CardName { get; set; }

        [UDOBind("U_Monto")]
        public float Monto { get; set; }

        [UDOBind("U_Moneda")]
        public string Moneda { get; set; }

        [UDOBind("U_No_Sal")]
        public string NoDocumentoSalida { get; set; }

        [UDOBind("U_Asiento")]
        public int? Asiento { get; set; }

        [UDOBind("U_VIN")]
        public string VIN { get; set; }

        [UDOBind("U_Ano")]
        public int Ano { get; set; }

        [UDOBind("U_Placa")]
        public string Placa { get; set; }

        [UDOBind("U_No_Cot")]
        public string NoCotización { get; set; }

        [UDOBind("U_Tipo")]
        public string TipoOrden { get; set; }

        [UDOBind("U_Asien_SE")]
        public int Asiento_SE { get; set; }

        [UDOBind("U_AsientoGastos")]
        public int AsientoGastos { get; set; }

        [UDOBind("U_No_OV")]
        public string NumeroOrdenVenta { get; set; }

        #region IEncabezadoUDO Members

        public string TablaLigada { get; private set; }

        #endregion
    }
}
