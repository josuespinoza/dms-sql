using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCG.ServicioPostVenta.DataContract.Orden_de_Trabajo
{
    public class Carga_OT
    {
        public static OrdenDeTrabajo Carga_OrdenDeTrabajo(SAPbobsCOM.Company p_Company, string p_strDocEntry)
        {

            SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
            SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
            SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
            SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);

            try
            {
                oCompanyService = p_Company.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", p_strDocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                return Carga_OrdenDeTrabajoDT(ref oGeneralData);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static OrdenDeTrabajo Carga_OrdenDeTrabajoDT(ref SAPbobsCOM.GeneralData p_oGeneralData)
        {
            OrdenDeTrabajo oOrdenDeTrabajo = default(OrdenDeTrabajo);
            try
            {
                oOrdenDeTrabajo = new OrdenDeTrabajo();
                oOrdenDeTrabajo.Code = (String)p_oGeneralData.GetProperty("Code");
                oOrdenDeTrabajo.Name = (String)p_oGeneralData.GetProperty("Name");
                oOrdenDeTrabajo.DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry");
                oOrdenDeTrabajo.Canceled = (String)p_oGeneralData.GetProperty("Canceled");
                oOrdenDeTrabajo.LogInst = (Int32)p_oGeneralData.GetProperty("LogInst");
                oOrdenDeTrabajo.UserSign = (Int32)p_oGeneralData.GetProperty("UserSign");
                oOrdenDeTrabajo.Transfered = (String)p_oGeneralData.GetProperty("Transfered");
                oOrdenDeTrabajo.DataSource = (String)p_oGeneralData.GetProperty("DataSource");
                oOrdenDeTrabajo.U_NoOT = (String)p_oGeneralData.GetProperty("U_NoOT");
                oOrdenDeTrabajo.U_NoUni = (String)p_oGeneralData.GetProperty("U_NoUni");
                oOrdenDeTrabajo.U_NoCon = (String)p_oGeneralData.GetProperty("U_NoCon");
                oOrdenDeTrabajo.U_Plac = (String)p_oGeneralData.GetProperty("U_Plac");
                oOrdenDeTrabajo.U_Marc = (String)p_oGeneralData.GetProperty("U_Marc");
                oOrdenDeTrabajo.U_Esti = (String)p_oGeneralData.GetProperty("U_Esti");
                oOrdenDeTrabajo.U_NoVis = (String)p_oGeneralData.GetProperty("U_NoVis");
                oOrdenDeTrabajo.U_EstVis = (String)p_oGeneralData.GetProperty("U_EstVis");
                oOrdenDeTrabajo.U_VIN = (String)p_oGeneralData.GetProperty("U_VIN");
                oOrdenDeTrabajo.U_TipOT = (String)p_oGeneralData.GetProperty("U_TipOT");
                oOrdenDeTrabajo.U_EstW = (String)p_oGeneralData.GetProperty("U_EstW");
                oOrdenDeTrabajo.U_FCom = (DateTime)p_oGeneralData.GetProperty("U_FCom");
                oOrdenDeTrabajo.U_FApe = (DateTime)p_oGeneralData.GetProperty("U_FApe");
                oOrdenDeTrabajo.U_FFin = (DateTime)p_oGeneralData.GetProperty("U_FFin");
                oOrdenDeTrabajo.U_EstO = (String)p_oGeneralData.GetProperty("U_EstO");
                oOrdenDeTrabajo.U_Ase = (String)p_oGeneralData.GetProperty("U_Ase");
                oOrdenDeTrabajo.U_EncO = (String)p_oGeneralData.GetProperty("U_EncO");
                oOrdenDeTrabajo.U_Obse = (String)p_oGeneralData.GetProperty("U_Obse");
                oOrdenDeTrabajo.U_CodEst = (String)p_oGeneralData.GetProperty("U_CodEst");
                oOrdenDeTrabajo.U_CodMar = (String)p_oGeneralData.GetProperty("U_CodMar");
                oOrdenDeTrabajo.U_Cotiz = (String)p_oGeneralData.GetProperty("U_Cotiz");
                oOrdenDeTrabajo.U_RCot = (String)p_oGeneralData.GetProperty("U_RCot");
                oOrdenDeTrabajo.U_DocEntry = (String)p_oGeneralData.GetProperty("U_DocEntry");
                oOrdenDeTrabajo.U_OTRef = (String)p_oGeneralData.GetProperty("U_OTRef");
                oOrdenDeTrabajo.U_NGas = (String)p_oGeneralData.GetProperty("U_NGas");
                oOrdenDeTrabajo.U_Sucu = (String)p_oGeneralData.GetProperty("U_Sucu");
                oOrdenDeTrabajo.U_Mode = (String)p_oGeneralData.GetProperty("U_Mode");
                oOrdenDeTrabajo.U_CEst = (String)p_oGeneralData.GetProperty("U_CEst");
                oOrdenDeTrabajo.U_CMod = (String)p_oGeneralData.GetProperty("U_CMod");
                oOrdenDeTrabajo.U_CMar = (String)p_oGeneralData.GetProperty("U_CMar");
                oOrdenDeTrabajo.U_Ano = (String)p_oGeneralData.GetProperty("U_Ano");
                oOrdenDeTrabajo.U_CodCli = (String)p_oGeneralData.GetProperty("U_CodCli");
                oOrdenDeTrabajo.U_NCli = (String)p_oGeneralData.GetProperty("U_NCli");
                oOrdenDeTrabajo.U_CodCOT = (String)p_oGeneralData.GetProperty("U_CodCOT");
                oOrdenDeTrabajo.U_NCliOT = (String)p_oGeneralData.GetProperty("U_NCliOT");
                oOrdenDeTrabajo.U_Cor = (String)p_oGeneralData.GetProperty("U_Cor");
                oOrdenDeTrabajo.U_Tel = (String)p_oGeneralData.GetProperty("U_Tel");
                oOrdenDeTrabajo.U_MOReal = (Double)p_oGeneralData.GetProperty("U_MOReal");
                oOrdenDeTrabajo.U_MOEsta = (Double)p_oGeneralData.GetProperty("U_MOEsta");
                oOrdenDeTrabajo.U_NoCita = (String)p_oGeneralData.GetProperty("U_NoCita");
                oOrdenDeTrabajo.U_FecVta = (DateTime)p_oGeneralData.GetProperty("U_FecVta");
                oOrdenDeTrabajo.U_Color = (String)p_oGeneralData.GetProperty("U_Color");
                oOrdenDeTrabajo.U_DEstO = (String)p_oGeneralData.GetProperty("U_DEstO");
                oOrdenDeTrabajo.U_Esp_Re = (int)p_oGeneralData.GetProperty("U_Esp_Re");
                oOrdenDeTrabajo.U_FechPro = (DateTime)p_oGeneralData.GetProperty("U_FechPro");
                oOrdenDeTrabajo.U_Repro = (int)p_oGeneralData.GetProperty("U_Repro");
                oOrdenDeTrabajo.U_km = (int)p_oGeneralData.GetProperty("U_km");
                oOrdenDeTrabajo.U_HCom = (DateTime)p_oGeneralData.GetProperty("U_HCom");
                oOrdenDeTrabajo.U_HApe = (DateTime)p_oGeneralData.GetProperty("U_HApe");
                oOrdenDeTrabajo.U_HFin = (DateTime)p_oGeneralData.GetProperty("U_HFin");
                oOrdenDeTrabajo.U_FCerr = (DateTime)p_oGeneralData.GetProperty("U_FCerr");
                oOrdenDeTrabajo.U_FFact = (DateTime)p_oGeneralData.GetProperty("U_FFact");
                oOrdenDeTrabajo.U_FEntr = (DateTime)p_oGeneralData.GetProperty("U_FEntr");
                oOrdenDeTrabajo.U_FRec = (DateTime)p_oGeneralData.GetProperty("U_FRec");
                oOrdenDeTrabajo.U_HRec = (DateTime)p_oGeneralData.GetProperty("U_HRec");
                oOrdenDeTrabajo.U_HMot = (int)p_oGeneralData.GetProperty("U_HMot");
                //oOrdenDeTrabajo.U_IdEsTC = (String)p_oGeneralData.GetProperty("U_IdEstOTTC");
                oOrdenDeTrabajo.ControlColaborador = Carga_ControlColaborador(p_oGeneralData.Child("SCGD_CTRLCOL"));
                oOrdenDeTrabajo.ImagenesOt = Carga_ImagenesOT(p_oGeneralData.Child("SCGD_IMG_OT"));
                oOrdenDeTrabajo.TrackingArticulos = Carga_TrackingArticulos(p_oGeneralData.Child("SCGD_TRACKXOT"));
                return oOrdenDeTrabajo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static List<ControlColaborador> Carga_ControlColaborador(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
        {
            List<ControlColaborador> controlColaborador = default(List<ControlColaborador>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                controlColaborador = new List<ControlColaborador>();
                for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
                {
                    oChildCc = p_generalDataCollection.Item(index);
                    controlColaborador.Add(new ControlColaborador());
                    controlColaborador[index].Code = (String)oChildCc.GetProperty("Code");
                    controlColaborador[index].LineId = (Int32)oChildCc.GetProperty("LineId");
                    controlColaborador[index].LogInst = (Int32)oChildCc.GetProperty("LogInst");
                    controlColaborador[index].U_Colab = (String)oChildCc.GetProperty("U_Colab");
                    controlColaborador[index].U_TMin = (Double)oChildCc.GetProperty("U_TMin");
                    controlColaborador[index].U_RePro = (String)oChildCc.GetProperty("U_RePro");
                    controlColaborador[index].U_NoFas = (String)oChildCc.GetProperty("U_NoFas");
                    controlColaborador[index].U_Estad = (String)oChildCc.GetProperty("U_Estad");
                    controlColaborador[index].U_IdAct = (String)oChildCc.GetProperty("U_IdAct");
                    controlColaborador[index].U_CosRe = (Double)oChildCc.GetProperty("U_CosRe");
                    controlColaborador[index].U_CosEst = (Double)oChildCc.GetProperty("U_CosEst");
                    controlColaborador[index].U_ReAsig = (String)oChildCc.GetProperty("U_ReAsig");
                    controlColaborador[index].U_FIni = (String)oChildCc.GetProperty("U_FIni");
                    controlColaborador[index].U_FFin = (String)oChildCc.GetProperty("U_FFin");
                    controlColaborador[index].U_HoraIni = (String)oChildCc.GetProperty("U_HoraIni");
                    controlColaborador[index].U_FechPro = (DateTime)oChildCc.GetProperty("U_FechPro");
                    controlColaborador[index].U_CodFas = (String)oChildCc.GetProperty("U_CodFas");
                    controlColaborador[index].U_DFIni = (DateTime)oChildCc.GetProperty("U_DFIni");
                    controlColaborador[index].U_HFIni = (DateTime)oChildCc.GetProperty("U_HFIni");
                    controlColaborador[index].U_DFFin = (DateTime)oChildCc.GetProperty("U_DFFin");
                    controlColaborador[index].U_HFFin = (DateTime)oChildCc.GetProperty("U_HFFin");
                }
                return controlColaborador;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static List<ImagenesOT> Carga_ImagenesOT(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
        {
            List<ImagenesOT> imagenesOT = default(List<ImagenesOT>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                imagenesOT = new List<ImagenesOT>();
                for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
                {
                    oChildCc = p_generalDataCollection.Item(index);
                    imagenesOT.Add(new ImagenesOT());
                    imagenesOT[index].Code = (String)oChildCc.GetProperty("Code");
                    imagenesOT[index].LineId = (Int32)oChildCc.GetProperty("LineId");
                    imagenesOT[index].LogInst = (Int32)oChildCc.GetProperty("LogInst");
                    imagenesOT[index].U_Direccion = (String)oChildCc.GetProperty("U_Direccion");
                    imagenesOT[index].U_UbiImagen = (String)oChildCc.GetProperty("U_UbiImagen");
                }
                return imagenesOT;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static List<TrackingArticulos> Carga_TrackingArticulos(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
        {
            List<TrackingArticulos> trackingArticulos = default(List<TrackingArticulos>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                trackingArticulos = new List<TrackingArticulos>();
                for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
                {
                    oChildCc = p_generalDataCollection.Item(index);
                    trackingArticulos.Add(new TrackingArticulos());
                    trackingArticulos[index].Code = (String)oChildCc.GetProperty("Code");
                    trackingArticulos[index].LineId = (Int32)oChildCc.GetProperty("LineId");
                    trackingArticulos[index].LogInst = (Int32)oChildCc.GetProperty("LogInst");
                    trackingArticulos[index].U_NoOrden = (String)oChildCc.GetProperty("U_NoOrden");
                    trackingArticulos[index].U_ItemCode = (String)oChildCc.GetProperty("U_ItemCode");
                    trackingArticulos[index].U_ID = (String)oChildCc.GetProperty("U_ID");
                    trackingArticulos[index].U_FechaSol = (DateTime)oChildCc.GetProperty("U_FechaSol");
                    trackingArticulos[index].U_FechaCom = (DateTime)oChildCc.GetProperty("U_FechaCom");
                    trackingArticulos[index].U_FechaEnt = (DateTime)oChildCc.GetProperty("U_FechaEnt");
                    trackingArticulos[index].U_CardCode = (String)oChildCc.GetProperty("U_CardCode");
                    trackingArticulos[index].U_CardName = (String)oChildCc.GetProperty("U_CardName");
                    trackingArticulos[index].U_DocEntry = (Int32)oChildCc.GetProperty("U_DocEntry");
                    trackingArticulos[index].U_DocNum = (Int32)oChildCc.GetProperty("U_DocNum");
                    trackingArticulos[index].U_Descripcion = (String)oChildCc.GetProperty("U_Descripcion");
                    trackingArticulos[index].U_Observ = (String)oChildCc.GetProperty("U_Observ");
                    trackingArticulos[index].U_CanSol = (Double)oChildCc.GetProperty("U_CanSol");
                    trackingArticulos[index].U_CanRec = (Double)oChildCc.GetProperty("U_CanRec");
                    trackingArticulos[index].U_FechaDoc = (DateTime)oChildCc.GetProperty("U_FechaDoc");
                    trackingArticulos[index].U_TipoDoc = (Int32)oChildCc.GetProperty("U_TipoDoc");
                }
                return trackingArticulos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
