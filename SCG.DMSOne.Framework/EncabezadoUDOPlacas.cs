using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOPlacas : IEncabezadoUDO
    {
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_Cod_Clie")]
        public string CodigoCliente { get; set; }

        [UDOBind("U_Nom_Clie")]
        public string NombreCliente { get; set; }

        [UDOBind("U_Num_Unid")]
        public string NumeroUnidad { get; set; }

        [UDOBind("U_Placa")]
        public string NumeroPlaca { get; set; }

        [UDOBind("U_Plac_AGV")]
        public string NumeroPlacaAGV { get; set; }

        [UDOBind("U_Tomo")]
        public string Tomo { get; set; }

        [UDOBind("U_Asiento")]
        public string Asiento { get; set; }

        [UDOBind("U_Num_VIN")]
        public string NumeroVIN { get; set; }

        [UDOBind("U_Num_Moto")]
        public string NumeroMotor { get; set; }

        [UDOBind("U_Marca")]
        public string Marca { get; set; }

        [UDOBind("U_Estilo")]
        public string Estilo { get; set; }

        [UDOBind("U_Modelo")]
        public string Modelo { get; set; }

        [UDOBind("U_Anno")]
        public int Anno { get; set; }

        [UDOBind("U_Color")]
        public string Color { get; set; }

        [UDOBind("U_Num_CV")]
        public string NumeroCV { get; set; }

        [UDOBind("U_Num_Fact")]
        public string NumeroFactura { get; set; }

        [UDOBind("U_Total")]
        public string Total { get; set; }

        [UDOBind("U_Sucurs")]
        public string Sucursal { get; set; }

        //[UDOBind("U_Cod_Suc")]
        //public string CodigoSucursal { get; set; }

        [UDOBind("U_Finaliz")]
        public string Finalizacion { get; set; }
        
        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_PLACA"; }
        }

        #endregion

    }
}
