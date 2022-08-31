using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOSalidaVehiculo : IEncabezadoUDO
    {

        public EncabezadoUDOSalidaVehiculo()
        {
            TablaLigada = "SCGD_GOODISSUE";
        }

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("DocNum")]
        public int DocNum { get; set; }

        [UDOBind("CreateDate", SoloLectura = true)]
        public DateTime CreateDate { get; set; }

        [UDOBind("CreateTime", SoloLectura = true)]
        public int CreateTime { get; set; }

        [UDOBind("U_Unidad")]
        public string CodigoUnidad { get; set; }

        [UDOBind("U_Marca")]
        public string Marca { get; set; }

        [UDOBind("U_Estilo")]
        public string Estilo { get; set; }

        [UDOBind("U_Modelo")]
        public string Modelo { get; set; }

        [UDOBind("U_Doc_Entr")]
        public string DocumentoEntrada { get; set; }

        [UDOBind("U_As_Entr")]
        public string AsientoEntrada { get; set; }

        [UDOBind("U_As_Sali")]
        public string AsientoSalida { get; set; }

        [UDOBind("U_Fec_Cont")]
        public string FechaContabilizacionString { get; set; }

        [UDOBind("U_VIN")]
        public string VIN { get; set; }

        [UDOBind("U_Cos_Loc")]
        public float  CostoMonedaLocal { get; set; }

        [UDOBind("U_Cos_Sis")]
        public float CostoMonedaSistema { get; set; }

        [UDOBind("U_NoCont")]
        public string NumeroContrato { get; set; }

        [UDOBind("U_NoFact")]
        public string NumeroFactura { get; set; }
        
        [UDOBind("U_ID_Veh")]
        public string NumeroVehiculo { get; set; }

        [UDOBind("U_Fech_Con")]
        public DateTime FechaContabilizacion { get; set; }

        [UDOBind("U_NCuenCnt")]
        public string NumeroCuenta { get; set; }
        


        #region IEncabezadoUDO Members

        public string TablaLigada { get; private set; }
        //public string TablaLigada
        //{
        //    get { return "SCGD_GOODISSUE"; }
        //}

        #endregion
    }
}