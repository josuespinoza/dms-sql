using System;
using System.Collections.Generic;

namespace SCG.ServicioPostVenta.DataContract.Orden_de_Trabajo
{
    public class OrdenDeTrabajo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Int32 DocEntry { get; set; }
        public string Canceled { get; set; }
        public Int32 LogInst { get; set; }
        public Int32 UserSign { get; set; }
        public string Transfered { get; set; }
        public string DataSource { get; set; }
        public string U_NoOT { get; set; }
        public string U_NoUni { get; set; }
        public string U_NoCon { get; set; }
        public string U_Plac { get; set; }
        public string U_Marc { get; set; }
        public string U_Esti { get; set; }
        public string U_NoVis { get; set; }
        public string U_EstVis { get; set; }
        public string U_VIN { get; set; }
        public string U_TipOT { get; set; }
        public string U_EstW { get; set; }
        public DateTime U_FCom { get; set; }
        public DateTime U_FApe { get; set; }
        public DateTime U_FFin { get; set; }
        public string U_EstO { get; set; }
        public string U_Ase { get; set; }
        public string U_EncO { get; set; }
        public string U_Obse { get; set; }
        public string U_CodEst { get; set; }
        public string U_CodMar { get; set; }
        public string U_Cotiz { get; set; }
        public string U_RCot { get; set; }
        public string U_DocEntry { get; set; }
        public string U_OTRef { get; set; }
        public string U_NGas { get; set; }
        public string U_Sucu { get; set; }
        public string U_Mode { get; set; }
        public string U_CEst { get; set; }
        public string U_CMod { get; set; }
        public string U_CMar { get; set; }
        public string U_Ano { get; set; }
        public string U_CodCli { get; set; }
        public string U_NCli { get; set; }
        public string U_CodCOT { get; set; }
        public string U_NCliOT { get; set; }
        public string U_Cor { get; set; }
        public string U_Tel { get; set; }
        public double U_MOReal { get; set; }
        public double U_MOEsta { get; set; }
        public string U_NoCita { get; set; }
        public DateTime U_FecVta { get; set; }
        public string U_Color { get; set; }
        public string U_DEstO { get; set; }
        public int U_Esp_Re { get; set; }
        public DateTime U_FechPro { get; set; }
        public int U_Repro { get; set; }
        public int U_km { get; set; }
        public DateTime U_HCom { get; set; }
        public DateTime U_HApe { get; set; }
        public DateTime U_HFin { get; set; }
        public DateTime U_FCerr { get; set; }
        public DateTime U_FFact { get; set; }
        public DateTime U_FEntr { get; set; }
        public DateTime U_FRec { get; set; }
        public DateTime U_HRec { get; set; }
        public int U_HMot { get; set; }
        public String U_IdEsTC { get; set; }
        public List<ControlColaborador> ControlColaborador { get; set; }
        public List<ImagenesOT> ImagenesOt { get; set; }
        public List<TrackingArticulos> TrackingArticulos { get; set; }
    }
}
