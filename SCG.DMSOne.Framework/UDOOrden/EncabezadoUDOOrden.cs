using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework.UDOOrden 
{
    public class EncabezadoUDOOrden : IEncabezadoUDO
    {
        public EncabezadoUDOOrden()
        {
            TablaLigada = "SCGD_OT";
        }

        [UDOBind("Code", Key = true)]
        public string Code { get; set; }

        [UDOBind("DocEntry", SoloLectura = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_NoOT")]
        public string U_NoOT { get; set; }

        [UDOBind("U_NoUni")]
        public string U_NoUni { get; set; }

        [UDOBind("U_NoCon")]
        public string U_NoCon { get; set; }

        [UDOBind("U_Plac")]
        public string U_Plac { get; set; }

        [UDOBind("U_Marc")]
        public string U_Marc { get; set; }

        [UDOBind("U_Esti")]
        public string U_Esti { get; set; }

        [UDOBind("U_NoVis")]
        public string U_NoVis { get; set; }

        [UDOBind("U_EstVis")]
        public string U_EstVis { get; set; }

        [UDOBind("U_VIN")]
        public string U_VIN { get; set; }

        [UDOBind("U_km")]
        public int  U_km { get; set; }

        [UDOBind("U_TipOT")]
        public string U_TipOT { get; set; }

        [UDOBind("U_EstW")]
        public string U_EstW { get; set; }

        [UDOBind("U_FCom")]
        public DateTime U_FCom { get; set; }

        [UDOBind("U_HCom")]
        public DateTime U_HCom { get; set; }

        [UDOBind("U_FApe")]
        public DateTime U_FApe { get; set; }

        [UDOBind("U_HApe")]
        public DateTime  U_HApe { get; set; }

        [UDOBind("U_FFin")]
        public DateTime U_FFin { get; set; }
        
        [UDOBind("U_HFin")]
        public DateTime U_HFin { get; set; }
        
        [UDOBind("U_FCerr")]
        public DateTime U_FCerr { get; set; }
        
        [UDOBind("U_FFact")]
        public DateTime  U_FFact { get; set; }
        
        [UDOBind("U_FEntr")]
        public DateTime  U_FEntr { get; set; }
        
        [UDOBind("U_EstO")]
        public string U_EstO { get; set; }

        [UDOBind("U_DEstO")]
        public string U_DEstO { get; set; }
        
        [UDOBind("U_Ase")]
        public string U_Ase { get; set; }
        
        [UDOBind("U_EncO")]
        public string U_EncO { get; set; }

        [UDOBind("U_Obse")]
        public string U_Obse { get; set; }

        [UDOBind("U_DocEntry")]
        public string U_DocEntry { get; set; }

        [UDOBind("U_Sucu")]
        public string U_Sucu { get; set; }

        [UDOBind("U_Mode")]
        public string U_Mode { get; set; }

        [UDOBind("U_CMar")]
        public string U_CMar { get; set; }

        [UDOBind("U_CEst")]
        public string U_CEst { get; set; }

        [UDOBind("U_CMod")]
        public string U_CMod { get; set; }

        [UDOBind("U_CodCli")]
        public string U_CodCli { get; set; }

        [UDOBind("U_NCli")]
        public string U_NCli { get; set; }

        [UDOBind("U_CodCOT")]
        public string U_CodCOT { get; set; }

        [UDOBind("U_NCliOT")]
        public string U_NCliOT { get; set; }

        [UDOBind("U_Ano")]
        public string U_Ano { get; set; }

        [UDOBind("U_OTRef")]
        public string U_OTRef { get; set; }

        [UDOBind("U_NGas")]
        public string U_NGas { get; set; }

        [UDOBind("U_FRec")]
        public DateTime  U_FRec { get; set; }

        [UDOBind("U_HRec")]
        public DateTime  U_HRec { get; set; }

        [UDOBind("U_HMot")]
        public int U_HMot { get; set; }

        [UDOBind("U_MOReal")]
        public double U_HMOReal { get; set; }

        [UDOBind("U_MOEsta")]
        public double U_HMOEsta { get; set; }

        [UDOBind("U_NoCita")]
        public String U_NoCita { get; set; }
        
        #region IEncabezadoUDO Members

        public string TablaLigada { get; private set; }

        #endregion
    }
}
