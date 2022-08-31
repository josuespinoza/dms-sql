using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOSolOTEsp : IEncabezadoUDO
    {
        #region IEncabezadoUDO Members
        public string TablaLigada { get; private set; }
        #endregion

        public EncabezadoUDOSolOTEsp()
        {
            TablaLigada = "SCGD_SOT_ESP";
        }

        #region Propiedades
        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("DocNum")]
        public int? DocNum { get; set; }

        [UDOBind("U_Cod_Clie")]
        public String CodigoCliente { get; set; }

        [UDOBind("U_Nom_Clie")]
        public String NombreCliente { get; set; }

        [UDOBind("U_Cod_Ases")]
        public String CodigoAsesor { get; set; }

        [UDOBind("U_Num_Coti")]
        public int? NumeroCotizacion { get; set; }

        [UDOBind("U_TipoOrd")]
        public String TipoOrden { get; set; }

        [UDOBind("U_OTRefer")]
        public String OTReferencia { get; set; }

        [UDOBind("U_Cod_Uni")]
        public String CodigoUnidad { get; set; }

        [UDOBind("U_Id_Vehi")]
        public int? IdVehiculo { get; set; }

        [UDOBind("U_VIN")]
        public String VIN { get; set; }

        [UDOBind("U_Placa")]
        public String Placa { get; set; }

        [UDOBind("U_Anno")]
        public Int16? Anno { get; set; }

        [UDOBind("U_klm")]
        public Int32? Kilometraje { get; set; }

        [UDOBind("U_Cod_Mar")]
        public String CodigoMarca { get; set; }

        [UDOBind("U_Cod_Mod")]
        public String CodigoModelo { get; set; }

        [UDOBind("U_Cod_Est")]
        public String CodigoEstilo { get; set; }

        [UDOBind("U_Des_Mar")]
        public String DescripcionMarca { get; set; }

        [UDOBind("U_Des_Mod")]
        public String DescripcionModelo { get; set; }

        [UDOBind("U_Des_Est")]
        public String DescripcionEstilo { get; set; }

        [UDOBind("U_Fec_Ape")]
        public DateTime? FechaApertura { get; set; }

        [UDOBind("U_Fec_Com")]
        public DateTime? FechaCompromiso { get; set; }

        [UDOBind("U_No_Vis")]
        public String NumeroVisita { get; set; }

        [UDOBind("U_CardCodeOrig")]
        public String CardCodeOrigen { get; set; }

        [UDOBind("U_CardNameOrig")]
        public String CardNameOrigen { get; set; }

        [UDOBind("U_OTPadre")]
        public String NumeroOTPadre { get; set; }

        [UDOBind("U_Estad_OT")]
        public String EstadoOT { get; set; }

        [UDOBind("U_Series")]
        public String Series { get; set; }

        [UDOBind("U_Comment")]
        public String Comentarios { get; set; }

        [UDOBind("U_CotCread", ValorPredeterminado = "N")]
        public String CotizacionCreada { get; set; }

        [UDOBind("U_Status", ValorPredeterminado = "A")]
        public String Estatus { get; set; }

        [UDOBind("U_CotRef")]
        public int? CotizacionReferencia { get; set; }

        [UDOBind("U_NomTipOT")]
        public String NombreTipoOT { get; set; }

        [UDOBind("U_NomAse")]
        public String NombreAsesor { get; set; }

        [UDOBind("U_ImpRecp", ValorPredeterminado = "N")]
        public String ImprimeRecepcion { get; set; }

        [UDOBind("U_HoraServicio")]
        public int HorasServicio { get; set; }

        #endregion
    }
}
