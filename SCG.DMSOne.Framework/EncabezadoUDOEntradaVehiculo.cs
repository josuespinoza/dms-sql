using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOEntradaVehiculo : IEncabezadoUDO
    {
        public string TablaLigada
        {
            get { return "SCGD_GOODRECEIVE"; }
        }

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("DocNum")]
        public int DocNum { get; set; }

        [UDOBind("Series")]
        public int Series { get; set; }

        [UDOBind("CreateDate", SoloLectura = true)]
        public DateTime  CreateDate { get; set; }

         [UDOBind("U_Fec_Cont")]
        public DateTime Fec_Cont { get; set; }

        [UDOBind("U_Unidad")]
        public string NoUnidad { get; set; }

        [UDOBind("U_Marca")]
        public string Marca { get; set; }

        [UDOBind("U_Estilo")]
        public string Estilo { get; set; }

        [UDOBind("U_Modelo")]
        public string Modelo { get; set; }

        [UDOBind("U_As_Entr")]
        public string AsientoEntrada { get; set; }

        [UDOBind("U_VIN")]
        public string Vin { get; set; }

        [UDOBind("U_Tot_Loc")]
        public float Tot_Loc { get; set; }

        [UDOBind("U_Tot_Sis")]
        public float Tot_Sis { get; set; }

        [UDOBind("U_COMAPE")]
        public float COMAPE { get; set; }

        [UDOBind("U_SEGLOC")]
        public float SEGLOC { get; set; }

        [UDOBind("U_COMAPE_S")]
        public float COMAPE_S { get; set; }

        [UDOBind("U_SEGLOC_S")]
        public float SEGLOC_S { get; set; }

        [UDOBind("U_FOB")]
        public float FOB { get; set; }

        [UDOBind("U_FOB_S")]
        public float FOB_S { get; set; }

        [UDOBind("U_FLETE")]
        public float FLETE { get; set; }

        [UDOBind("U_FLETE_S")]
        public float FLETE_S { get; set; }

        [UDOBind("U_SEGFAC")]
        public float SEGFAC { get; set; }

        [UDOBind("U_SEGFAC_S")]
        public float SEGFAC_S { get; set; }

        [UDOBind("U_COMFOR")]
        public float COMFOR { get; set; }

        [UDOBind("U_COMFOR_S")]
        public float COMFOR_S { get; set; }

        [UDOBind("U_COMNEG")]
        public float COMNEG { get; set; }

        [UDOBind("U_COMNEG_S")]
        public float COMNEG_S { get; set; }

        [UDOBind("U_CIF_S")]
        public float CIF_S { get; set; }

        [UDOBind("U_CIF_L")]
        public float CIF_L { get; set; }

        [UDOBind("U_TRASLA")]
        public float TRASLA { get; set; }

        [UDOBind("U_TRASLA_S")]
        public float TRASLA_S { get; set; }

        [UDOBind("U_REDEST")]
        public float REDEST { get; set; }

        [UDOBind("U_REDEST_S")]
        public float REDEST_S { get; set; }

        [UDOBind("U_BODALM")]
        public float BODALM { get; set; }

        [UDOBind("U_BODALM_S")]
        public float BODALM_S { get; set; }

        [UDOBind("U_DESALM")]
        public float DESALM { get; set; }

        [UDOBind("U_DESALM_S")]
        public float DESALM_S { get; set; }

        [UDOBind("U_IMPVTA")]
        public float IMPVTA { get; set; }

        [UDOBind("U_IMPVTA_S")]
        public float IMPVTA_S { get; set; }

        [UDOBind("U_AGENCIA")]
        public float AGENCIA { get; set; }

        [UDOBind("U_RESERVA")]
        public float RESERVA { get; set; }

        [UDOBind("U_RESERVA_S")]
        public float RESERVA_S { get; set; }

        [UDOBind("U_ACCINT")]
        public float ACCINT { get; set; }

        [UDOBind("U_ACCINT_S")]
        public float ACCINT_S { get; set; }

        [UDOBind("U_ACCEXT")]
        public float ACCEXT { get; set; }

        [UDOBind("U_ACCEXT_S")]
        public float ACCEXT_S { get; set; }

        [UDOBind("U_OTROS")]
        public float OTROS { get; set; }

        [UDOBind("U_OTROS_S")]
        public float OTROS_S { get; set; }

        [UDOBind("U_TALLER")]
        public float TALLER { get; set; }

        [UDOBind("U_FLELOC")]
        public float FLELOC { get; set; }

        [UDOBind("U_FLELOC_S")]
        public float FLELOC_S { get; set; }

        [UDOBind("U_VALHAC")]
        public float VALHAC { get; set; }

        [UDOBind("U_VALHAC_S")]
        public float VALHAC_S { get; set; }

        [UDOBind("U_GASTRA")]
        public float GASTRA { get; set; }

        [UDOBind("U_GASTRA_S")]
        public float GASTRA_S { get; set; }

        [UDOBind("U_TALLER_S")]
        public float TALLER_S { get; set; }

        [UDOBind("U_AGENCI_S")]
        public float AGENCI_S { get; set; }

        [UDOBind("U_RESERV_S")]
        public float RESERV_S { get; set; }

        [UDOBind("U_ID_Vehiculo")]
        public string ID_Vehiculo { get; set; }

        [UDOBind("U_Cambio")]
        public float Cambio { get; set; }

        [UDOBind("U_Tipo")]
        public string  Tipo { get; set; }

        [UDOBind("U_SCGD_DocSalida")]
        public string  SCGD_DocSalida { get; set; }

        [UDOBind("U_Num_Cont")]
        public string ContratoVenta { get; set; }

        [UDOBind("U_DocRecep")]
        public string DocRecepcion { get; set; }

        [UDOBind("U_DocPedido")]
        public string DocPedido { get; set; }

        [UDOBind("U_EsTrasl")]
        public string EsTraslado { get; set; }
  
    }
}