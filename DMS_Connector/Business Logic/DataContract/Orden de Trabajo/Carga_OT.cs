using System;
using System.Collections.Generic;

namespace DMS_Connector.Business_Logic.DataContract.Orden_de_Trabajo
{
    public class Carga_OT
    {
        /// <summary>
        /// Función que retorna el DataContract de la Orden de Trabajo Solicitada
        /// </summary>
        /// <param name="p_strDocEntry">DocEntry de Orden de Trabajo a retornar</param>
        /// <returns>DataContract de la Orden de Trabajo solicitada</returns>
        public static OrdenDeTrabajo Carga_OrdenDeTrabajo(string p_strDocEntry)
        {

            SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
            SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
            SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
            SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);

            try
            {
                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT");
                oGeneralParams =
                    (SAPbobsCOM.GeneralDataParams)
                        oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", p_strDocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                return Carga_OrdenDeTrabajoDT(ref oGeneralData);

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Helpers.DestruirObjeto(ref oCompanyService);
                Helpers.DestruirObjeto(ref oGeneralService);
                Helpers.DestruirObjeto(ref oGeneralData);
                Helpers.DestruirObjeto(ref oGeneralParams);
            }
        }

        /// <summary>
        /// Función que asigna los valores del GeneralData al DataContact
        /// </summary>
        /// <param name="p_oGeneralData">GeneralData de la Orden de Trabajo consultada</param>
        /// <returns>DataContract de Orden de Trabajo solicitada</returns>
        private static OrdenDeTrabajo Carga_OrdenDeTrabajoDT(ref SAPbobsCOM.GeneralData p_oGeneralData)
        {
            OrdenDeTrabajo oOrdenDeTrabajo = default(OrdenDeTrabajo);
            try
            {
                oOrdenDeTrabajo = new OrdenDeTrabajo
                {
                    Code = (String)p_oGeneralData.GetProperty("Code"),
                    Name = (String)p_oGeneralData.GetProperty("Name"),
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_NoOT = (String)p_oGeneralData.GetProperty("U_NoOT"),
                    U_NoUni = (String)p_oGeneralData.GetProperty("U_NoUni"),
                    U_NoCon = (String)p_oGeneralData.GetProperty("U_NoCon"),
                    U_Plac = (String)p_oGeneralData.GetProperty("U_Plac"),
                    U_Marc = (String)p_oGeneralData.GetProperty("U_Marc"),
                    U_Esti = (String)p_oGeneralData.GetProperty("U_Esti"),
                    U_NoVis = (String)p_oGeneralData.GetProperty("U_NoVis"),
                    U_EstVis = (String)p_oGeneralData.GetProperty("U_EstVis"),
                    U_VIN = (String)p_oGeneralData.GetProperty("U_VIN"),
                    U_TipOT = (String)p_oGeneralData.GetProperty("U_TipOT"),
                    U_EstW = (String)p_oGeneralData.GetProperty("U_EstW"),
                    U_FCom = (DateTime?)p_oGeneralData.GetProperty("U_FCom"),
                    U_FApe = (DateTime?)p_oGeneralData.GetProperty("U_FApe"),
                    U_FFin = (DateTime?)p_oGeneralData.GetProperty("U_FFin"),
                    U_EstO = (String)p_oGeneralData.GetProperty("U_EstO"),
                    U_Ase = (String)p_oGeneralData.GetProperty("U_Ase"),
                    U_EncO = (String)p_oGeneralData.GetProperty("U_EncO"),
                    U_Obse = (String)p_oGeneralData.GetProperty("U_Obse"),
                    U_CodEst = (String)p_oGeneralData.GetProperty("U_CodEst"),
                    U_CodMar = (String)p_oGeneralData.GetProperty("U_CodMar"),
                    U_Cotiz = (String)p_oGeneralData.GetProperty("U_Cotiz"),
                    U_RCot = (String)p_oGeneralData.GetProperty("U_RCot"),
                    U_DocEntry = (String)p_oGeneralData.GetProperty("U_DocEntry"),
                    U_OTRef = (String)p_oGeneralData.GetProperty("U_OTRef"),
                    U_NGas = (String)p_oGeneralData.GetProperty("U_NGas"),
                    U_Sucu = (String)p_oGeneralData.GetProperty("U_Sucu"),
                    U_Mode = (String)p_oGeneralData.GetProperty("U_Mode"),
                    U_CEst = (String)p_oGeneralData.GetProperty("U_CEst"),
                    U_CMod = (String)p_oGeneralData.GetProperty("U_CMod"),
                    U_CMar = (String)p_oGeneralData.GetProperty("U_CMar"),
                    U_Ano = (String)p_oGeneralData.GetProperty("U_Ano"),
                    U_CodCli = (String)p_oGeneralData.GetProperty("U_CodCli"),
                    U_NCli = (String)p_oGeneralData.GetProperty("U_NCli"),
                    U_CodCOT = (String)p_oGeneralData.GetProperty("U_CodCOT"),
                    U_NCliOT = (String)p_oGeneralData.GetProperty("U_NCliOT"),
                    U_Cor = (String)p_oGeneralData.GetProperty("U_Cor"),
                    U_Tel = (String)p_oGeneralData.GetProperty("U_Tel"),
                    U_MOReal = (Double)p_oGeneralData.GetProperty("U_MOReal"),
                    U_MOEsta = (Double)p_oGeneralData.GetProperty("U_MOEsta"),
                    U_NoCita = (String)p_oGeneralData.GetProperty("U_NoCita"),
                    U_FecVta = (DateTime?)p_oGeneralData.GetProperty("U_FecVta"),
                    U_Color = (String)p_oGeneralData.GetProperty("U_Color"),
                    U_DEstO = (String)p_oGeneralData.GetProperty("U_DEstO"),
                    U_Esp_Re = (int)p_oGeneralData.GetProperty("U_Esp_Re"),
                    U_FechPro = (DateTime?)p_oGeneralData.GetProperty("U_FechPro"),
                    U_Repro = (int)p_oGeneralData.GetProperty("U_Repro"),
                    U_km = (Double)p_oGeneralData.GetProperty("U_km"),
                    U_HCom = (DateTime?)p_oGeneralData.GetProperty("U_HCom"),
                    U_HApe = (DateTime?)p_oGeneralData.GetProperty("U_HApe"),
                    U_HFin = (DateTime?)p_oGeneralData.GetProperty("U_HFin"),
                    U_FCerr = (DateTime?)p_oGeneralData.GetProperty("U_FCerr"),
                    U_FFact = (DateTime?)p_oGeneralData.GetProperty("U_FFact"),
                    U_FEntr = (DateTime?)p_oGeneralData.GetProperty("U_FEntr"),
                    U_FRec = (DateTime?)p_oGeneralData.GetProperty("U_FRec"),
                    U_HRec = (DateTime?)p_oGeneralData.GetProperty("U_HRec"),
                    U_HMot = (int)p_oGeneralData.GetProperty("U_HMot"),
                    ControlColaborador = Carga_ControlColaborador(p_oGeneralData.Child("SCGD_CTRLCOL")),
                    ImagenesOt = Carga_ImagenesOT(p_oGeneralData.Child("SCGD_IMG_OT")),
                    TrackingArticulos = Carga_TrackingArticulos(p_oGeneralData.Child("SCGD_TRACKXOT"))
                };
                return oOrdenDeTrabajo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista de control colaborador de la Orden de Trabajo
        /// </summary>
        /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas de control colaborador de la Orden de Trabajo</param>
        /// <returns>Lista con el control colaborador de la Orden de Trabajo</returns>
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
                    controlColaborador.Add(new ControlColaborador
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Colab = (String)oChildCc.GetProperty("U_Colab"),
                        U_TMin = (Double)oChildCc.GetProperty("U_TMin"),
                        U_RePro = (String)oChildCc.GetProperty("U_RePro"),
                        U_NoFas = (String)oChildCc.GetProperty("U_NoFas"),
                        U_Estad = (String)oChildCc.GetProperty("U_Estad"),
                        U_IdAct = (String)oChildCc.GetProperty("U_IdAct"),
                        U_CosRe = (Double)oChildCc.GetProperty("U_CosRe"),
                        U_CosEst = (Double)oChildCc.GetProperty("U_CosEst"),
                        U_ReAsig = (String)oChildCc.GetProperty("U_ReAsig"),
                        U_HoraIni = (String)oChildCc.GetProperty("U_HoraIni"),
                        U_FechPro = (DateTime?)oChildCc.GetProperty("U_FechPro"),
                        U_CodFas = (String)oChildCc.GetProperty("U_CodFas"),
                        U_DFIni = (DateTime?)oChildCc.GetProperty("U_DFIni"),
                        U_HFIni = (DateTime?)oChildCc.GetProperty("U_HFIni"),
                        U_DFFin = (DateTime?)oChildCc.GetProperty("U_DFFin"),
                        U_HFFin = (DateTime?)oChildCc.GetProperty("U_HFFin")
                    });

                }
                return controlColaborador;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista de imágenes de la Orden de Trabajo
        /// </summary>
        /// <param name="p_generalDataCollection">GeneralDataCollection con las imágenes de la Orden de Trabajo</param>
        /// <returns>Lista con las imágenes de la Orden de Trabajo</returns>
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
                    imagenesOT.Add(new ImagenesOT
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Direccion = (String)oChildCc.GetProperty("U_Direccion"),
                        U_UbiImagen = (String)oChildCc.GetProperty("U_UbiImagen")
                    });

                }
                return imagenesOT;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista del Tracking de Repuestos de la Orden de Trabajo
        /// </summary>
        /// <param name="p_generalDataCollection">GeneralDataCollection el Tracking de Repuestos de la Orden de Trabajo</param>
        /// <returns>Lista con el Tracking de Repuestos de la Orden de Trabajo</returns>
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
                    trackingArticulos.Add(new TrackingArticulos
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_NoOrden = (String)oChildCc.GetProperty("U_NoOrden"),
                        U_ItemCode = (String)oChildCc.GetProperty("U_ItemCode"),
                        U_ID = (String)oChildCc.GetProperty("U_ID"),
                        U_FechaSol = (DateTime?)oChildCc.GetProperty("U_FechaSol"),
                        U_FechaCom = (DateTime?)oChildCc.GetProperty("U_FechaCom"),
                        U_FechaEnt = (DateTime?)oChildCc.GetProperty("U_FechaEnt"),
                        U_CardCode = (String)oChildCc.GetProperty("U_CardCode"),
                        U_CardName = (String)oChildCc.GetProperty("U_CardName"),
                        U_DocEntry = (Int32)oChildCc.GetProperty("U_DocEntry"),
                        U_DocNum = (Int32)oChildCc.GetProperty("U_DocNum"),
                        U_Descripcion = (String)oChildCc.GetProperty("U_Descripcion"),
                        U_Observ = (String)oChildCc.GetProperty("U_Observ"),
                        U_CanSol = (Double)oChildCc.GetProperty("U_CanSol"),
                        U_CanRec = (Double)oChildCc.GetProperty("U_CanRec"),
                        U_FechaDoc = (DateTime?)oChildCc.GetProperty("U_FechaDoc"),
                        U_TipoDoc = (Int32)oChildCc.GetProperty("U_TipoDoc")
                    });

                }
                return trackingArticulos;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
