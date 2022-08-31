using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Vehiculos
{
    public class Carga_Vehiculo
    {
        /// <summary>
        /// Función que retorna el DataContract de el Vehiculo Solicitado
        /// </summary>
        /// <param name="p_strCode">Código de Vehículo a retornar</param>
        /// <returns>DataContract de Vehiculo solicitado</returns>
        public static Vehiculo Carga_VehiculoDatoMaestro(string p_strCode)
        {
            SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
            SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
            SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
            SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);

            try
            {
                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH");
                oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", p_strCode);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                return Carga_VehiculoDT(ref oGeneralData);
            }
            catch (Exception ex)
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
        /// <param name="p_oGeneralData">GeneralData del vehículo consultado</param>
        /// <returns>DataContract de Vehículo solicitado</returns>
        private static Vehiculo Carga_VehiculoDT(ref SAPbobsCOM.GeneralData p_oGeneralData)
        {
            Vehiculo vehiculo = default(Vehiculo);
            try
            {
                vehiculo = new Vehiculo
                {
                    Code = (String)p_oGeneralData.GetProperty("Code"),
                    Name = (String)p_oGeneralData.GetProperty("Name"),
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    Object = (String)p_oGeneralData.GetProperty("Object"),
                    LogInst = (Int32?)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32?)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_Cod_Unid = (String)p_oGeneralData.GetProperty("U_Cod_Unid"),
                    U_Cod_Marc = (String)p_oGeneralData.GetProperty("U_Cod_Marc"),
                    U_Des_Marc = (String)p_oGeneralData.GetProperty("U_Des_Marc"),
                    U_Cod_Mode = (String)p_oGeneralData.GetProperty("U_Cod_Mode"),
                    U_Des_Mode = (String)p_oGeneralData.GetProperty("U_Des_Mode"),
                    U_Cod_Esti = (String)p_oGeneralData.GetProperty("U_Cod_Esti"),
                    U_Des_Esti = (String)p_oGeneralData.GetProperty("U_Des_Esti"),
                    U_Ano_Vehi = (Int16?)p_oGeneralData.GetProperty("U_Ano_Vehi"),
                    U_Num_Plac = (String)p_oGeneralData.GetProperty("U_Num_Plac"),
                    U_Cod_Col = (String)p_oGeneralData.GetProperty("U_Cod_Col"),
                    U_Des_Col = (String)p_oGeneralData.GetProperty("U_Des_Col"),
                    U_ColorTap = (String)p_oGeneralData.GetProperty("U_ColorTap"),
                    U_Num_VIN = (String)p_oGeneralData.GetProperty("U_Num_VIN"),
                    U_Num_Mot = (String)p_oGeneralData.GetProperty("U_Num_Mot"),
                    U_MarcaMot = (String)p_oGeneralData.GetProperty("U_MarcaMot"),
                    U_Cant_Pas = (Int16?)p_oGeneralData.GetProperty("U_Cant_Pas"),
                    U_Cod_Ubic = (String)p_oGeneralData.GetProperty("U_Cod_Ubic"),
                    U_Tipo = (String)p_oGeneralData.GetProperty("U_Tipo"),
                    U_Estatus = (String)p_oGeneralData.GetProperty("U_Estatus"),
                    U_Tipo_Tra = (String)p_oGeneralData.GetProperty("U_Tipo_Tra"),
                    U_Num_Cili = (Int32?)p_oGeneralData.GetProperty("U_Num_Cili"),
                    U_TipTecho = (String)p_oGeneralData.GetProperty("U_TipTecho"),
                    U_Carrocer = (String)p_oGeneralData.GetProperty("U_Carrocer"),
                    U_CantPuer = (Int16?)p_oGeneralData.GetProperty("U_CantPuer"),
                    U_Peso = (Int32?)p_oGeneralData.GetProperty("U_Peso"),
                    U_Cilindra = (Int32?)p_oGeneralData.GetProperty("U_Cilindra"),
                    U_Categori = (String)p_oGeneralData.GetProperty("U_Categori"),
                    U_Combusti = (String)p_oGeneralData.GetProperty("U_Combusti"),
                    U_Tip_Cabi = (String)p_oGeneralData.GetProperty("U_Tip_Cabi"),
                    U_Potencia = (Int32?)p_oGeneralData.GetProperty("U_Potencia"),
                    U_Transmis = (String)p_oGeneralData.GetProperty("U_Transmis"),
                    U_Accesori = (String)p_oGeneralData.GetProperty("U_Accesori"),
                    U_GarantKM = (Int32?)p_oGeneralData.GetProperty("U_GarantKM"),
                    U_GarantTM = (Int16?)p_oGeneralData.GetProperty("U_GarantTM"),
                    U_CardCode = (String)p_oGeneralData.GetProperty("U_CardCode"),
                    U_CardName = (String)p_oGeneralData.GetProperty("U_CardName"),
                    U_FechaVen = (DateTime?)p_oGeneralData.GetProperty("U_FechaVen"),
                    U_CTOVTA = (Int32?)p_oGeneralData.GetProperty("U_CTOVTA"),
                    U_VTADOL = (Double?)p_oGeneralData.GetProperty("U_VTADOL"),
                    U_VTACOL = (Double?)p_oGeneralData.GetProperty("U_VTACOL"),
                    U_FCHINV = (DateTime?)p_oGeneralData.GetProperty("U_FCHINV"),
                    U_NUMFAC = (Int32?)p_oGeneralData.GetProperty("U_NUMFAC"),
                    U_TIPINV = (String)p_oGeneralData.GetProperty("U_TIPINV"),
                    U_FCHRES = (DateTime?)p_oGeneralData.GetProperty("U_FCHRES"),
                    U_OBSRES = (String)p_oGeneralData.GetProperty("U_OBSRES"),
                    U_ARREST = (String)p_oGeneralData.GetProperty("U_ARREST"),
                    U_FECFINR = (DateTime?)p_oGeneralData.GetProperty("U_FECFINR"),
                    U_SALINID = (Double?)p_oGeneralData.GetProperty("U_SALINID"),
                    U_SALINIC = (Double?)p_oGeneralData.GetProperty("U_SALINIC"),
                    U_FLELOC = (Double?)p_oGeneralData.GetProperty("U_FLELOC"),
                    U_TIPCAM = (Double?)p_oGeneralData.GetProperty("U_TIPCAM"),
                    U_COSINV = (Double?)p_oGeneralData.GetProperty("U_COSINV"),
                    U_VALHAC = (Double?)p_oGeneralData.GetProperty("U_VALHAC"),
                    U_GASTRA = (Double?)p_oGeneralData.GetProperty("U_GASTRA"),
                    U_Dispo = (Int32?)p_oGeneralData.GetProperty("U_Dispo"),
                    U_VENRES = (String)p_oGeneralData.GetProperty("U_VENRES"),
                    U_Cod_Fab = (String)p_oGeneralData.GetProperty("U_Cod_Fab"),
                    U_Tipo_Ven = (String)p_oGeneralData.GetProperty("U_Tipo_Ven"),
                    U_Precio = (Double?)p_oGeneralData.GetProperty("U_Precio"),
                    U_FchUSv = (DateTime?)p_oGeneralData.GetProperty("U_FchUSv"),
                    U_FchPrSv = (DateTime?)p_oGeneralData.GetProperty("U_FchPrSv"),
                    U_FchRsva = (DateTime?)p_oGeneralData.GetProperty("U_FchRsva"),
                    U_FchVcRva = (DateTime?)p_oGeneralData.GetProperty("U_FchVcRva"),
                    U_NoPedFb = (String)p_oGeneralData.GetProperty("U_NoPedFb"),
                    U_FrecSvc = (Int32?)p_oGeneralData.GetProperty("U_FrecSvc"),
                    U_fechaSync = (String)p_oGeneralData.GetProperty("U_fechaSync"),
                    U_ArtVent = (String)p_oGeneralData.GetProperty("U_ArtVent"),
                    U_Cli_Ven = (String)p_oGeneralData.GetProperty("U_Cli_Ven"),
                    U_Tipo_Reing = (String)p_oGeneralData.GetProperty("U_Tipo_Reing"),
                    U_ClNo_Ven = (String)p_oGeneralData.GetProperty("U_ClNo_Ven"),
                    U_CosPro = (Double?)p_oGeneralData.GetProperty("U_CosPro"),
                    U_Moneda = (String)p_oGeneralData.GetProperty("U_Moneda"),
                    U_ValorNet = (Double?)p_oGeneralData.GetProperty("U_ValorNet"),
                    U_ArtVentDesc = (String)p_oGeneralData.GetProperty("U_ArtVentDesc"),
                    U_Des_Col_Tap = (String)p_oGeneralData.GetProperty("U_Des_Col_Tap"),
                    U_Clasificacion = (String)p_oGeneralData.GetProperty("U_Clasificacion"),
                    U_Estado_Nuevo = (String)p_oGeneralData.GetProperty("U_Estado_Nuevo"),
                    U_Fha_Ing_Inv = (DateTime?)p_oGeneralData.GetProperty("U_Fha_Ing_Inv"),
                    U_CCar = (String)p_oGeneralData.GetProperty("U_CCar"),
                    U_Pote = (String)p_oGeneralData.GetProperty("U_Pote"),
                    U_DiEje = (String)p_oGeneralData.GetProperty("U_DiEje"),
                    U_Ramv = (String)p_oGeneralData.GetProperty("U_Ramv"),
                    U_Cant_Eje = (String)p_oGeneralData.GetProperty("U_Cant_Eje"),
                    U_Bono = (Double?)p_oGeneralData.GetProperty("U_Bono"),
                    U_HorSer = (Int32?)p_oGeneralData.GetProperty("U_HorSer"),
                    U_DocRecepcion = (String)p_oGeneralData.GetProperty("U_DocRecepcion"),
                    U_Comentarios = (String)p_oGeneralData.GetProperty("U_Comentarios"),
                    U_Km_Unid = (Double?)p_oGeneralData.GetProperty("U_Km_Unid"),
                    U_Cod_Prov = (String)p_oGeneralData.GetProperty("U_Cod_Prov"),
                    U_Nom_Prov = (String)p_oGeneralData.GetProperty("U_Nom_Prov"),
                    U_ContratoV = (String)p_oGeneralData.GetProperty("U_ContratoV"),
                    U_DocPedido = (String)p_oGeneralData.GetProperty("U_DocPedido"),
                    U_TCRSalIni = (Double?)p_oGeneralData.GetProperty("U_TCRSalIni"),
                    U_Cod_Tec = (String)p_oGeneralData.GetProperty("U_Cod_Tec"),
                    U_Consig = (String)p_oGeneralData.GetProperty("U_Consig"),
                    U_GaranIni = (DateTime?)p_oGeneralData.GetProperty("U_GaranIni"),
                    U_GaranFin = (DateTime?)p_oGeneralData.GetProperty("U_GaranFin"),
                    AccesoriosxVehiculo = Carga_AccesoriosXVehiculo(p_oGeneralData.Child("SCGD_ACCXVEH")),
                    BonosXVehiculo = Carga_BonosXVehiculo(p_oGeneralData.Child("SCGD_BONOXVEH")),
                    TrazabilizadXVehiculo = Carga_TrazabilizadXVehiculo(p_oGeneralData.Child("SCGD_VEHITRAZA"))
                };
                return vehiculo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista de los accesorios del vehículo
        /// </summary>
        /// <param name="p_oChildrenAccXVeh">GeneralDataCollection con los accesorios del vehículo</param>
        /// <returns>Lista con los accesorios del vehículo</returns>
        private static List<AccesoriosXVehiculo> Carga_AccesoriosXVehiculo(SAPbobsCOM.GeneralDataCollection p_oChildrenAccXVeh)
        {
            List<AccesoriosXVehiculo> accesoriosxVehiculoList = default(List<AccesoriosXVehiculo>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                accesoriosxVehiculoList = new List<AccesoriosXVehiculo>();
                for (int index = 0; index <= p_oChildrenAccXVeh.Count - 1; index++)
                {
                    oChildCc = p_oChildrenAccXVeh.Item(index);
                    accesoriosxVehiculoList.Add(new AccesoriosXVehiculo
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_Acc = (String)oChildCc.GetProperty("U_Acc"),
                        U_N_Acc = (String)oChildCc.GetProperty("U_N_Acc"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo")
                    });

                }
                return accesoriosxVehiculoList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista de los bonos del vehículo
        /// </summary>
        /// <param name="p_oChildrenBonosXVeh">GeneralDataCollection con los bonos del vehículo</param>
        /// <returns>Lista con los bonos del vehículo</returns>
        private static List<BonosXVehiculo> Carga_BonosXVehiculo(SAPbobsCOM.GeneralDataCollection p_oChildrenBonosXVeh)
        {
            List<BonosXVehiculo> bonosXVehiculoList = default(List<BonosXVehiculo>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                bonosXVehiculoList = new List<BonosXVehiculo>();
                for (int index = 0; index <= p_oChildrenBonosXVeh.Count - 1; index++)
                {
                    oChildCc = p_oChildrenBonosXVeh.Item(index);
                    bonosXVehiculoList.Add(new BonosXVehiculo
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_Bono = (String)oChildCc.GetProperty("U_Bono"),
                        U_Monto = (Double?)oChildCc.GetProperty("U_Monto")
                    });

                }
                return bonosXVehiculoList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que retorna Lista de la trazabilidad del vehículo
        /// </summary>
        /// <param name="p_oChildrenTraXVeh">GeneralDataCollection con la trazabilidad del vehículo</param>
        /// <returns>Lista con la trazabilidad del vehículo</returns>
        private static List<TrazabilizadXVehiculo> Carga_TrazabilizadXVehiculo(SAPbobsCOM.GeneralDataCollection p_oChildrenTraXVeh)
        {
            List<TrazabilizadXVehiculo> trazabilizadXVehiculoList = default(List<TrazabilizadXVehiculo>);
            SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

            try
            {
                trazabilizadXVehiculoList = new List<TrazabilizadXVehiculo>();
                for (int index = 0; index <= p_oChildrenTraXVeh.Count - 1; index++)
                {
                    oChildCc = p_oChildrenTraXVeh.Item(index);
                    trazabilizadXVehiculoList.Add(new TrazabilizadXVehiculo
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_Cod_Unid = (String)oChildCc.GetProperty("U_Cod_Unid"),
                        U_NumDoc_I = (String)oChildCc.GetProperty("U_NumDoc_I"),
                        U_FhaDoc_I = (DateTime?)oChildCc.GetProperty("U_FhaDoc_I"),
                        U_NumCV_I = (String)oChildCc.GetProperty("U_NumCV_I"),
                        U_FhaCV_I = (DateTime?)oChildCc.GetProperty("U_FhaCV_I"),
                        U_CodVen_I = (String)oChildCc.GetProperty("U_CodVen_I"),
                        U_TotDoc_I = (Double?)oChildCc.GetProperty("U_TotDoc_I"),
                        U_Obs_I = (String)oChildCc.GetProperty("U_Obs_I"),
                        U_NumCV_V = (String)oChildCc.GetProperty("U_NumCV_V"),
                        U_FhaCV_V = (DateTime?)oChildCc.GetProperty("U_FhaCV_V"),
                        U_CodCli_V = (String)oChildCc.GetProperty("U_CodCli_V"),
                        U_CodVen_V = (String)oChildCc.GetProperty("U_CodVen_V"),
                        U_NumFac_V = (String)oChildCc.GetProperty("U_NumFac_V"),
                        U_Obs_V = (String)oChildCc.GetProperty("U_Obs_V"),
                        U_TotCV_V = (Double?)oChildCc.GetProperty("U_TotCV_V"),
                        U_ValVeh = (Double?)oChildCc.GetProperty("U_ValVeh"),
                        U_FhaFac_V = (DateTime?)oChildCc.GetProperty("U_FhaFac_V"),
                        U_FFCom = (DateTime?)oChildCc.GetProperty("U_FFCom"),
                        U_FGuia = (DateTime?)oChildCc.GetProperty("U_FGuia"),
                        U_NoGuia = (String)oChildCc.GetProperty("U_NoGuia"),
                        U_NumCo = (String)oChildCc.GetProperty("U_NumCo"),
                        U_FecEntCV = (DateTime?)oChildCc.GetProperty("U_FecEntCV"),
                        U_Km_Ingreso = (Double?)oChildCc.GetProperty("U_Km_Ingreso"),
                        U_Km_Venta = (Double?)oChildCc.GetProperty("U_Km_Venta")
                    });

                }
                return trazabilizadXVehiculoList;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
