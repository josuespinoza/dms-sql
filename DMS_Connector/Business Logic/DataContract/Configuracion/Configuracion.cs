using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DMS_Connector.Business_Logic;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Configuracion_Sucursal;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Dimensiones;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Generales;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Numeraciones;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Parametrizaciones_Generales;
using SAPbobsCOM;

namespace DMS_Connector
{
    public static class Configuracion
    {
        #region "Atributos"

        public static Admin ParamGenAddon { get; set; }

        public static List<Configuracion_Sucursal> ConfiguracionSucursales { get; set; }

        public static List<TipoOT> TipoOt { get; set; }

        public static List<Aprobado> Aprobado { get; set; }

        public static List<Trasladado> Trasladado { get; set; }

        public static List<Mensajeria> ConfMensajeria { get; set; }

        public static List<Numeracion> Numeracion { get; set; }

        public static List<Dimensiones> Dimensiones { get; set; }

        public static List<DimensionesOT> DimensionesOT { get; set; }

        public static List<String> LtPermisosMenu { get; set; }

        #endregion

        #region "Carga de Parametrizaciones Generales Admin"

        /// <summary>
        /// Método que inicializa las Parametrizaciones Generales del Addon
        /// </summary>
        public static void Carga_ParametrizacionesGenerales()
        {

            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);

            try
            {
                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_ADMIN");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", "DMS");
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                ParamGenAddon = Carga_Admin(ref oGeneralData);
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO Admin</param>
        /// <returns>DataContract del UDO Admin</returns>
        private static Admin Carga_Admin(ref GeneralData p_oGeneralData)
        {
            Admin admin = default(Admin);
            try
            {
                admin = new Admin
                {
                    Code = (String)p_oGeneralData.GetProperty("Code"),
                    Name = (String)p_oGeneralData.GetProperty("Name"),
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_Etap_CRM = (Int32)p_oGeneralData.GetProperty("U_Etap_CRM"),
                    U_Disp_V = (String)p_oGeneralData.GetProperty("U_Disp_V"),
                    U_Disp_R = (String)p_oGeneralData.GetProperty("U_Disp_R"),
                    U_Inven_V = (String)p_oGeneralData.GetProperty("U_Inven_V"),
                    U_Inven_R = (String)p_oGeneralData.GetProperty("U_Inven_R"),
                    U_Placa_Pr = (String)p_oGeneralData.GetProperty("U_Placa_Pr"),
                    U_Serie_U = (String)p_oGeneralData.GetProperty("U_Serie_U"),
                    U_CFL_Vehi = (String)p_oGeneralData.GetProperty("U_CFL_Vehi"),
                    U_Reportes = (String)p_oGeneralData.GetProperty("U_Reportes"),
                    U_Tipo_DD = (Int32)p_oGeneralData.GetProperty("U_Tipo_DD"),
                    U_Monto_GL = (Double)p_oGeneralData.GetProperty("U_Monto_GL"),
                    U_OTROS_AC = (String)p_oGeneralData.GetProperty("U_OTROS_AC"),
                    U_TRAN_AA = (String)p_oGeneralData.GetProperty("U_TRAN_AA"),
                    U_SCGD_EtapaCV = (Int32)p_oGeneralData.GetProperty("U_SCGD_EtapaCV"),
                    U_SCGD_ModPrecio = (String)p_oGeneralData.GetProperty("U_SCGD_ModPrecio"),
                    U_SCGD_AccInv = (String)p_oGeneralData.GetProperty("U_SCGD_AccInv"),
                    U_SCGD_BodAcc = (String)p_oGeneralData.GetProperty("U_SCGD_BodAcc"),
                    U_GeSalAut = (String)p_oGeneralData.GetProperty("U_GeSalAut"),
                    U_NCSalNeg = (String)p_oGeneralData.GetProperty("U_NCSalNeg"),
                    U_SCGD_VIN = (String)p_oGeneralData.GetProperty("U_SCGD_VIN"),
                    U_SCGD_Uni = (String)p_oGeneralData.GetProperty("U_SCGD_Uni"),
                    U_Perm_Fac = (String)p_oGeneralData.GetProperty("U_Perm_Fac"),
                    U_Disp_Res = (String)p_oGeneralData.GetProperty("U_Disp_Res"),
                    U_Usa_Fin = (String)p_oGeneralData.GetProperty("U_Usa_Fin"),
                    U_Usa_Plc = (String)p_oGeneralData.GetProperty("U_Usa_Plc"),
                    U_Periodo = (String)p_oGeneralData.GetProperty("U_Periodo"),
                    U_Niv_Fin = (String)p_oGeneralData.GetProperty("U_Niv_Fin"),
                    U_Niv_Usa = (String)p_oGeneralData.GetProperty("U_Niv_Usa"),
                    U_NivAprob = (String)p_oGeneralData.GetProperty("U_NivAprob"),
                    U_Desc_Fac = (String)p_oGeneralData.GetProperty("U_Desc_Fac"),
                    U_Mul_Fac = (String)p_oGeneralData.GetProperty("U_Mul_Fac"),
                    U_AxEst = (String)p_oGeneralData.GetProperty("U_AxEst"),
                    U_Mon_Def = (String)p_oGeneralData.GetProperty("U_Mon_Def"),
                    U_Usa_FiEx = (String)p_oGeneralData.GetProperty("U_Usa_FiEx"),
                    U_AlmGeTra = (String)p_oGeneralData.GetProperty("U_AlmGeTra"),
                    U_CostSExFP = (String)p_oGeneralData.GetProperty("U_CostSExFP"),
                    U_Pag_Prim = (String)p_oGeneralData.GetProperty("U_Pag_Prim"),
                    U_UsaLed = (String)p_oGeneralData.GetProperty("U_UsaLed"),
                    U_EspVehic = (String)p_oGeneralData.GetProperty("U_EspVehic"),
                    U_UsaAXEV = (String)p_oGeneralData.GetProperty("U_UsaAXEV"),
                    U_SCGD_Pla = (String)p_oGeneralData.GetProperty("U_SCGD_Pla"),
                    U_TiemEsta = (String)p_oGeneralData.GetProperty("U_TiemEsta"),
                    U_AdcApr = (String)p_oGeneralData.GetProperty("U_AdcApr"),
                    U_RepPre = (String)p_oGeneralData.GetProperty("U_RepPre"),
                    U_CTCosAcc = (String)p_oGeneralData.GetProperty("U_CTCosAcc"),
                    U_UsaAxC = (String)p_oGeneralData.GetProperty("U_UsaAxC"),
                    U_ModCDes = (String)p_oGeneralData.GetProperty("U_ModCDes"),
                    U_CnpDMS = (String)p_oGeneralData.GetProperty("U_CnpDMS"),
                    U_ValTipoInv = (String)p_oGeneralData.GetProperty("U_ValTipoInv"),
                    U_TransCostCero = (String)p_oGeneralData.GetProperty("U_TransCostCero"),
                    U_FechaRes = (String)p_oGeneralData.GetProperty("U_FechaRes"),
                    U_BO_Parc = (String)p_oGeneralData.GetProperty("U_BO_Parc"),
                    U_TCCont = (String)p_oGeneralData.GetProperty("U_TCCont"),
                    U_Ramv = (String)p_oGeneralData.GetProperty("U_Ramv"),
                    U_Pagos_Rec = (String)p_oGeneralData.GetProperty("U_Pagos_Rec"),
                    U_MoPreVta = (String)p_oGeneralData.GetProperty("U_MoPreVta"),
                    U_CtaFiDe = (String)p_oGeneralData.GetProperty("U_CtaFiDe"),
                    U_CtaFiHa = (String)p_oGeneralData.GetProperty("U_CtaFiHa"),
                    U_CtaAsDe = (String)p_oGeneralData.GetProperty("U_CtaAsDe"),
                    U_CtaAsHa = (String)p_oGeneralData.GetProperty("U_CtaAsHa"),
                    U_AlGenOC = (String)p_oGeneralData.GetProperty("U_AlGenOC"),
                    U_UtCos = (String)p_oGeneralData.GetProperty("U_UtCos"),
                    U_UsaDimC = (String)p_oGeneralData.GetProperty("U_UsaDimC"),
                    U_UsaComi = (String)p_oGeneralData.GetProperty("U_UsaComi"),
                    U_CosteoLocal = (String)p_oGeneralData.GetProperty("U_CosteoLocal"),
                    U_ReduceCant = (String)p_oGeneralData.GetProperty("U_ReduceCant"),
                    U_Gen_Draft_Cost = (String)p_oGeneralData.GetProperty("U_Gen_Draft_Cost"),
                    U_ValongVIN = (String)p_oGeneralData.GetProperty("U_ValongVIN"),
                    U_SCGD_AnoUs = (String)p_oGeneralData.GetProperty("U_SCGD_AnoUs"),
                    U_OT_SAP = (String)p_oGeneralData.GetProperty("U_OT_SAP"),
                    U_ConCUCV = (String)p_oGeneralData.GetProperty("U_ConCUCV"),
                    U_ValCompAE = (String)p_oGeneralData.GetProperty("U_ValCompAE"),
                    U_NewDMUCV = (String)p_oGeneralData.GetProperty("U_NewDMUCV"),
                    U_UsaConfI = (String)p_oGeneralData.GetProperty("U_UsaConfI"),
                    U_UsaWHTramites = (String)p_oGeneralData.GetProperty("U_UsaWHTramites"),
                    U_ValLeaSN = (String)p_oGeneralData.GetProperty("U_ValLeaSN"),
                    U_CodLeaSN = (String)p_oGeneralData.GetProperty("U_CodLeaSN"),
                    U_UsaTrmFac = (String)p_oGeneralData.GetProperty("U_UsaTrmFac"),
                    U_GenAsSE = (String)p_oGeneralData.GetProperty("U_GenAsSE"),
                    U_UsaCostAuto = (String)p_oGeneralData.GetProperty("U_UsaCostAuto"),
                    U_UsaFactPorUnid = (String)p_oGeneralData.GetProperty("U_UsaFactPorUnid"),
                    U_UsaFExVU = (String)p_oGeneralData.GetProperty("U_UsaFExVU"),
                    U_UsaDSNRU = (String)p_oGeneralData.GetProperty("U_UsaDSNRU"),
                    U_SeTra = (String)p_oGeneralData.GetProperty("U_SeTra"),
                    U_UsaFPVU = (String)p_oGeneralData.GetProperty("U_UsaFPVU"),
                    U_TipoTransCostAuto = (String)p_oGeneralData.GetProperty("U_TipoTransCostAuto"),
                    U_Usa_IFord = (String)p_oGeneralData.GetProperty("U_Usa_IFord"),
                    U_UsaUbicD = (String)p_oGeneralData.GetProperty("U_UsaUbicD"),
                    U_UsaFilRep = (String)p_oGeneralData.GetProperty("U_UsaFilRep"),
                    U_UsaFilSer = (String)p_oGeneralData.GetProperty("U_UsaFilSer"),
                    U_Devol_Veh = (String)p_oGeneralData.GetProperty("U_Devol_Veh"),
                    U_UsaTAva = (String)p_oGeneralData.GetProperty("U_UsaTAva"),
                    U_Busq_exac = (String)p_oGeneralData.GetProperty("U_Busq_exac"),
                    U_UsaSegPV = (String)p_oGeneralData.GetProperty("U_UsaSegPV"),
                    U_ValReVen = (String)p_oGeneralData.GetProperty("U_ValReVen"),
                    U_SCGD_CSLoc = (String)p_oGeneralData.GetProperty("U_SCGD_CSLoc"),
                    U_EditKmCV = (String)p_oGeneralData.GetProperty("U_EditKmCV"),
                    U_UsaVATGroup = (String)p_oGeneralData.GetProperty("U_UsaVATGroup"),
                    U_LocCR = (String)p_oGeneralData.GetProperty("U_LocCR"),
                    U_CodLisPre = (String)p_oGeneralData.GetProperty("U_CodLisPre"),
                    U_CtrlAcc = (String)p_oGeneralData.GetProperty("U_CtrlAcc"),
                    U_UsaBanCl = (String)p_oGeneralData.GetProperty("U_UsaBanCl"),
                    U_GenAsSeg = (String)p_oGeneralData.GetProperty("U_GenAsSeg"),
                    U_GenFacCns = (String)p_oGeneralData.GetProperty("U_GenFacCns"),
                    U_UsaPrecioSalida = (String)p_oGeneralData.GetProperty("U_UsaPrecioSalida"),
                    U_BloqEntradaSA = (String)p_oGeneralData.GetProperty("U_BloqEntradaSA"),
                    U_FieldsPosition = (String)p_oGeneralData.GetProperty("U_FieldsPosition"),
                    U_SchSizeMode = (String)p_oGeneralData.GetProperty("U_SchSizeMode"),
                    U_ScheduleType = (String)p_oGeneralData.GetProperty("U_ScheduleType") ,
                    U_VerCostoS = (String)p_oGeneralData.GetProperty("U_VerCostoS"),
                    U_CABYS_CR = (String)p_oGeneralData.GetProperty("U_CABYS_CR"),
                    Admin1 = Carga_Admin1(p_oGeneralData.Child("SCGD_ADMIN1")),
                    Admin2 = Carga_Admin2(p_oGeneralData.Child("SCGD_ADMIN2")),
                    Admin3 = Carga_Admin3(p_oGeneralData.Child("SCGD_ADMIN3")),
                    Admin4 = Carga_Admin4(p_oGeneralData.Child("SCGD_ADMIN4")),
                    Admin5 = Carga_Admin5(p_oGeneralData.Child("SCGD_ADMIN5")),
                    Admin6 = Carga_Admin6(p_oGeneralData.Child("SCGD_ADMIN6")),
                    Admin7 = Carga_Admin7(p_oGeneralData.Child("SCGD_ADMIN7")),
                    Admin8 = Carga_Admin8(p_oGeneralData.Child("SCGD_ADMIN8")),
                    Admin9 = Carga_Admin9(p_oGeneralData.Child("SCGD_ADMIN9"))
                };
            }
            catch (Exception)
            {
                throw;
            }
            return admin;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin1
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin1</param>
        /// <returns>Lista con configuraciones de la Admin1</returns>
        private static List<Admin1> Carga_Admin1(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc = default(GeneralData);
            List<Admin1> admin1List = default(List<Admin1>);

            try
            {
                admin1List = new List<Admin1>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin1List.Add(new Admin1
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_ItemCode = (String)oChildCc.GetProperty("U_ItemCode"),
                        U_ItemName = (String)oChildCc.GetProperty("U_ItemName")
                    });

                }
                return admin1List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin2
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin2</param>
        /// <returns>Lista con configuraciones de la Admin2</returns>
        private static List<Admin2> Carga_Admin2(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin2> admin2List;
            try
            {
                admin2List = new List<Admin2>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin2List.Add(new Admin2
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_Cod_GA = (Int32)oChildCc.GetProperty("U_Cod_GA")
                    });

                }
                return admin2List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin3
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin3</param>
        /// <returns>Lista con configuraciones de la Admin3</returns>
        private static List<Admin3> Carga_Admin3(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin3> admin3List;
            try
            {
                admin3List = new List<Admin3>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin3List.Add(new Admin3
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_Cod_Imp = (String)oChildCc.GetProperty("U_Cod_Imp")
                    });

                }
                return admin3List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin4
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin4</param>
        /// <returns>Lista con configuraciones de la Admin4</returns>
        private static List<Admin4> Carga_Admin4(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin4> admin4List;
            try
            {
                admin4List = new List<Admin4>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin4List.Add(new Admin4
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Transito = (String)oChildCc.GetProperty("U_Transito"),
                        U_Stock = (String)oChildCc.GetProperty("U_Stock"),
                        U_Costo = (String)oChildCc.GetProperty("U_Costo"),
                        U_Ingreso = (String)oChildCc.GetProperty("U_Ingreso"),
                        U_AccXAlm = (String)oChildCc.GetProperty("U_AccXAlm"),
                        U_Bod_Tram = (String)oChildCc.GetProperty("U_Bod_Tram"),
                        U_Bod_Log = (String)oChildCc.GetProperty("U_Bod_Log"),
                        U_Devolucion = (String)oChildCc.GetProperty("U_Devolucion")
                    });

                }
                return admin4List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin5
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin5</param>
        /// <returns>Lista con configuraciones de la Admin5</returns>
        private static List<Admin5> Carga_Admin5(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin5> admin5List;
            try
            {
                admin5List = new List<Admin5>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin5List.Add(new Admin5
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_Cuenta = (String)oChildCc.GetProperty("U_Cuenta")
                    });
                }
                return admin5List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin6
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin6</param>
        /// <returns>Lista con configuraciones de la Admin6</returns>
        private static List<Admin6> Carga_Admin6(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin6> admin6List;
            try
            {
                admin6List = new List<Admin6>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin6List.Add(new Admin6
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_Serie = (Int32)oChildCc.GetProperty("U_Serie"),
                        U_SerieEx = (Int32)oChildCc.GetProperty("U_SerieEx")
                    });

                }
                return admin6List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin7
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin7</param>
        /// <returns>Lista con configuraciones de la Admin7</returns>
        private static List<Admin7> Carga_Admin7(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin7> admin7List;
            try
            {
                admin7List = new List<Admin7>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin7List.Add(new Admin7
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                        U_Moneda = (String)oChildCc.GetProperty("U_Moneda")
                    });

                }
                return admin7List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin8
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin8</param>
        /// <returns>Lista con configuraciones de la Admin8</returns>
        private static List<Admin8> Carga_Admin8(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin8> admin8List;
            try
            {
                admin8List = new List<Admin8>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin8List.Add(new Admin8
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                        U_Cod_Ind = (String)oChildCc.GetProperty("U_Cod_Ind")
                    });

                }
                return admin8List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de la Admin9
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de la Admin9</param>
        /// <returns>Lista con configuraciones de la Admin9</returns>
        private static List<Admin9> Carga_Admin9(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Admin9> admin9List;
            try
            {
                admin9List = new List<Admin9>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    admin9List.Add(new Admin9
                    {
                        Code = (String)oChildCc.GetProperty("Code"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Codigo = (String)oChildCc.GetProperty("U_Codigo"),
                        U_Prio = (Int32)oChildCc.GetProperty("U_Prio"),
                        U_Name = (String)oChildCc.GetProperty("U_Name"),
                        U_PEmp = (String)oChildCc.GetProperty("U_PEmp"),
                        U_Estado = (String)oChildCc.GetProperty("U_Estado"),
                        U_UMenu = (String)oChildCc.GetProperty("U_UMenu"),
                        U_ValTI = (String)oChildCc.GetProperty("U_ValTI"),
                        U_EsUsad = (String)oChildCc.GetProperty("U_EsUsad")
                    });

                }
                return admin9List;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #region "Carga de Configuraciones por Sucursal"

        /// <summary>
        /// Método que inicializa las Configuraciones de Sucursal
        /// </summary>
        public static void Carga_Configuracion_Sucursal()
        {
            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);
            List<Configuracion_Sucursal> lstConfSucursal;
            List<int> lstDocEntrySucursal;
            try
            {
                lstDocEntrySucursal = new List<int>();
                lstConfSucursal = new List<Configuracion_Sucursal>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strDocEntrySucursales")).Rows)
                    if (drRow["DocEntry"] != DBNull.Value)
                        lstDocEntrySucursal.Add(Convert.ToInt32(drRow["DocEntry"]));

                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_ConfSuc");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                foreach (int index in lstDocEntrySucursal)
                {
                    oGeneralParams.SetProperty("DocEntry", index);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    lstConfSucursal.Add(Carga_ConfSucursal(ref oGeneralData));
                }
                ConfiguracionSucursales = lstConfSucursal;
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// Recarga una configuracion de sucrusal especifica al realizar un cambio
        /// </summary>
        /// <param name="p_strSucur">Docentry de la sucursal</param>
        public static void Carga_Configuracion_SucursalEspecifica(string p_strSucur)
        {
            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);

            try
            {
                if (ConfiguracionSucursales.Any(confS => confS.U_Sucurs.Equals(p_strSucur)))
                {
                    oCompanyService = Company.CompanySBO.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_ConfSuc");
                    oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("DocEntry", ConfiguracionSucursales.FirstOrDefault(confS => confS.U_Sucurs.Equals(p_strSucur)).DocEntry);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    for (int index = 0; index <= ConfiguracionSucursales.Count - 1; index++)
                    {
                        if (ConfiguracionSucursales[index].DocEntry != Convert.ToInt32(oGeneralData.GetProperty("DocEntry"))) continue;
                        ConfiguracionSucursales[index] = Carga_ConfSucursal(ref oGeneralData);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO Configuración de Sucursal</param>
        /// <returns>DataContract del UDO Configuración de Sucursal</returns>
        private static Configuracion_Sucursal Carga_ConfSucursal(ref GeneralData p_oGeneralData)
        {
            Configuracion_Sucursal confiSucursal;
            try
            {
                confiSucursal = new Configuracion_Sucursal
                {
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    DocNum = (Int32)p_oGeneralData.GetProperty("DocNum"),
                    Period = (Int32)p_oGeneralData.GetProperty("Period"),
                    Instance = (Int32)p_oGeneralData.GetProperty("Instance"),
                    Series = (Int32)p_oGeneralData.GetProperty("Series"),
                    Handwrtten = (String)p_oGeneralData.GetProperty("Handwrtten"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    Status = (String)p_oGeneralData.GetProperty("Status"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_Sucurs = (String)p_oGeneralData.GetProperty("U_Sucurs"),
                    U_SerOfC = (String)p_oGeneralData.GetProperty("U_SerOfC"),
                    U_SerOrC = (String)p_oGeneralData.GetProperty("U_SerOrC"),
                    U_SerOfV = (String)p_oGeneralData.GetProperty("U_SerOfV"),
                    U_SerOrV = (String)p_oGeneralData.GetProperty("U_SerOrV"),
                    U_SerInv = (String)p_oGeneralData.GetProperty("U_SerInv"),
                    U_ArtCita = (String)p_oGeneralData.GetProperty("U_ArtCita"),
                    U_DesSOfC = (String)p_oGeneralData.GetProperty("U_DesSOfC"),
                    U_DesSOfV = (String)p_oGeneralData.GetProperty("U_DesSOfV"),
                    U_DesSOrV = (String)p_oGeneralData.GetProperty("U_DesSOrV"),
                    U_DesSOrC = (String)p_oGeneralData.GetProperty("U_DesSOrC"),
                    U_DesSInv = (String)p_oGeneralData.GetProperty("U_DesSInv"),
                    U_HoraInicio = (DateTime?)p_oGeneralData.GetProperty("U_HoraInicio"),
                    U_HoraFin = (DateTime?)p_oGeneralData.GetProperty("U_HoraFin"),
                    U_HoraIS = (DateTime?)p_oGeneralData.GetProperty("U_HoraIS"),
                    U_HoraFS = (DateTime?)p_oGeneralData.GetProperty("U_HoraFS"),
                    U_HoraID = (DateTime?)p_oGeneralData.GetProperty("U_HoraID"),
                    U_HoraFD = (DateTime?)p_oGeneralData.GetProperty("U_HoraFD"),
                    U_CodCitaCancel = (String)p_oGeneralData.GetProperty("U_CodCitaCancel"),
                    U_CodCitaNueva = (String)p_oGeneralData.GetProperty("U_CodCitaNueva"),
                    U_CodCitaTarde = (String)p_oGeneralData.GetProperty("U_CodCitaTarde"),
                    U_CodCitaAnula = (String)p_oGeneralData.GetProperty("U_CodCitaAnula"),
                    U_CantMinTarde = (String)p_oGeneralData.GetProperty("U_CantMinTarde"),
                    U_CantHorasValida = (String)p_oGeneralData.GetProperty("U_CantHorasValida"),
                    U_UsaDurEC = (String)p_oGeneralData.GetProperty("U_UsaDurEC"),
                    U_Imp_Serv = (String)p_oGeneralData.GetProperty("U_Imp_Serv"),
                    U_Imp_Repuestos = (String)p_oGeneralData.GetProperty("U_Imp_Repuestos"),
                    U_Imp_Suminis = (String)p_oGeneralData.GetProperty("U_Imp_Suminis"),
                    U_Imp_ServExt = (String)p_oGeneralData.GetProperty("U_Imp_ServExt"),
                    U_CosteoMO_C = (String)p_oGeneralData.GetProperty("U_CosteoMO_C"),
                    U_CosteoMO_I = (String)p_oGeneralData.GetProperty("U_CosteoMO_I"),
                    U_TiempoEst_C = (String)p_oGeneralData.GetProperty("U_TiempoEst_C"),
                    U_TiempoReal_C = (String)p_oGeneralData.GetProperty("U_TiempoReal_C"),
                    U_Moneda_C = (String)p_oGeneralData.GetProperty("U_Moneda_C"),
                    U_CuentaSys_C = (String)p_oGeneralData.GetProperty("U_CuentaSys_C"),
                    U_DescCuenta_C = (String)p_oGeneralData.GetProperty("U_DescCuenta_C"),
                    U_CtaAcreGast = (String)p_oGeneralData.GetProperty("U_CtaAcreGast"),
                    U_CtaDebGast = (String)p_oGeneralData.GetProperty("U_CtaDebGast"),
                    U_MonDocGastos = (String)p_oGeneralData.GetProperty("U_MonDocGastos"),
                    U_GenASGastos = (String)p_oGeneralData.GetProperty("U_GenASGastos"),
                    U_GenFAGastos = (String)p_oGeneralData.GetProperty("U_GenFAGastos"),
                    U_DescCtaAcreGast = (String)p_oGeneralData.GetProperty("U_DescCtaAcreGast"),
                    U_DescCtaDebGast = (String)p_oGeneralData.GetProperty("U_DescCtaDebGast"),
                    U_Imp_Gastos = (String)p_oGeneralData.GetProperty("U_Imp_Gastos"),
                    U_USolOTEsp = (String)p_oGeneralData.GetProperty("U_USolOTEsp"),
                    U_ValKm = (String)p_oGeneralData.GetProperty("U_ValKm"),
                    U_SDocCot = (String)p_oGeneralData.GetProperty("U_SDocCot"),
                    U_ValHS = (String)p_oGeneralData.GetProperty("U_ValHS"),
                    U_Entrega_Rep = (String)p_oGeneralData.GetProperty("U_Entrega_Rep"),
                    U_ValReqPen = (String)p_oGeneralData.GetProperty("U_ValReqPen"),
                    U_TiempoOFV_C = (String)p_oGeneralData.GetProperty("U_TiempoOFV_C"),
                    U_Requis = (String)p_oGeneralData.GetProperty("U_Requis"),
                    U_UseParts = (String)p_oGeneralData.GetProperty("U_UseParts"),
                    U_UseServ = (String)p_oGeneralData.GetProperty("U_UseServ"),
                    U_UseSE = (String)p_oGeneralData.GetProperty("U_UseSE"),
                    U_UseSum = (String)p_oGeneralData.GetProperty("U_UseSum"),
                    U_AsigTecOT = (String)p_oGeneralData.GetProperty("U_AsigTecOT"),
                    U_ValTiemEst = (String)p_oGeneralData.GetProperty("U_ValTiemEst"),
                    U_FinOTCanSol = (String)p_oGeneralData.GetProperty("U_FinOTCanSol"),
                    U_CambPreTall = (String)p_oGeneralData.GetProperty("U_CambPreTall"),
                    U_AsigUniMec = (String)p_oGeneralData.GetProperty("U_AsigUniMec"),
                    U_UseLisPreCli = (String)p_oGeneralData.GetProperty("U_UseLisPreCli"),
                    U_GenOTEsp = (String)p_oGeneralData.GetProperty("U_GenOTEsp"),
                    U_ValOTCreEsp = (String)p_oGeneralData.GetProperty("U_ValOTCreEsp"),
                    U_CopiasOT = (Double)p_oGeneralData.GetProperty("U_CopiasOT"),
                    U_UnidadTiemp = (String)p_oGeneralData.GetProperty("U_UnidadTiemp"),
                    U_UseCliFilter = (String)p_oGeneralData.GetProperty("U_UseCliFilter"),
                    U_CitCliInac = (String)p_oGeneralData.GetProperty("U_CitCliInac"),
                    U_UsaOrdVenta = (String)p_oGeneralData.GetProperty("U_UsaOrdVenta"),
                    U_UsaOfeVenta = (String)p_oGeneralData.GetProperty("U_UsaOfeVenta"),
                    U_ListaPrecios = (String)p_oGeneralData.GetProperty("U_ListaPrecios"),
                    U_CodLisPre = (String)p_oGeneralData.GetProperty("U_CodLisPre"),
                    U_UniTpMint = (Double)p_oGeneralData.GetProperty("U_UniTpMint"),
                    U_NoBodRep = (String)p_oGeneralData.GetProperty("U_NoBodRep"),
                    U_NoBodPro = (String)p_oGeneralData.GetProperty("U_NoBodPro"),
                    U_NoBodSE = (String)p_oGeneralData.GetProperty("U_NoBodSE"),
                    U_NoBodSum = (String)p_oGeneralData.GetProperty("U_NoBodSum"),
                    U_AsigAutCol = (String)p_oGeneralData.GetProperty("U_AsigAutCol"),
                    U_SEInvent = (String)p_oGeneralData.GetProperty("U_SEInvent"),
                    U_CostoSimp = (String)p_oGeneralData.GetProperty("U_CostoSimp"),
                    U_CostoDet = (String)p_oGeneralData.GetProperty("U_CostoDet"),
                    U_MsjXCC = (String)p_oGeneralData.GetProperty("U_MsjXCC"),
                    U_HorAlI = (DateTime?)p_oGeneralData.GetProperty("U_HorAlI"),
                    U_HoraAlF = (DateTime?)p_oGeneralData.GetProperty("U_HoraAlF"),
                    U_GrpTrabajo = (String)p_oGeneralData.GetProperty("U_GrpTrabajo"),
                    U_FOTAPen = (String)p_oGeneralData.GetProperty("U_FOTAPen"),
                    U_HjaCanPen = (String)p_oGeneralData.GetProperty("U_HjaCanPen"),
                    U_PerCanOT = (String)p_oGeneralData.GetProperty("U_PerCanOT"),
                    U_CanOTSer = (String)p_oGeneralData.GetProperty("U_CanOTSer"),
                    U_CanOTArAp = (String)p_oGeneralData.GetProperty("U_CanOTArAp"),
                    U_Imp_RepVenta = (String)p_oGeneralData.GetProperty("U_Imp_RepVenta"),
                    U_SolaUna = (String)p_oGeneralData.GetProperty("U_SolaUna"),
                    U_Dir_Img_Ot = (String)p_oGeneralData.GetProperty("U_Dir_Img_Ot"),
                    U_CitaSinAsesor = (String)p_oGeneralData.GetProperty("U_CitaSinAsesor"),
                    U_UsaSolEsp = (String)p_oGeneralData.GetProperty("U_UsaSolEsp"),
                    U_CtaDebitoMO = (String)p_oGeneralData.GetProperty("U_CtaDebitoMO"),
                    U_DesCtaDebitoMO = (String)p_oGeneralData.GetProperty("U_DesCtaDebitoMO"),
                    U_CtaDebitoCosto = (String)p_oGeneralData.GetProperty("U_CtaDebitoCosto"),
                    U_DesCtaDebitoCosto = (String)p_oGeneralData.GetProperty("U_DesCtaDebitoCosto"),
                    U_CtaDotacionSE = (String)p_oGeneralData.GetProperty("U_CtaDotacionSE"),
                    U_DesCtaDotacionSE = (String)p_oGeneralData.GetProperty("U_DesCtaDotacionSE"),
                    U_CtaGastosSE = (String)p_oGeneralData.GetProperty("U_CtaGastosSE"),
                    U_DesCtaGastosSE = (String)p_oGeneralData.GetProperty("U_DesCtaGastosSE"),
                    U_CtaDifPrecioSE = (String)p_oGeneralData.GetProperty("U_CtaDifPrecioSE"),
                    U_DesCtaDifPrecioSE = (String)p_oGeneralData.GetProperty("U_DesCtaDifPrecioSE"),
                    U_CtaCostosBVSE = (String)p_oGeneralData.GetProperty("U_CtaCostosBVSE"),
                    U_DesCtaCostosBVSE = (String)p_oGeneralData.GetProperty("U_DesCtaCostosBVSE"),
                    U_ImpRepCom = (String)p_oGeneralData.GetProperty("U_ImpRepCom"),
                    U_ImpSECom = (String)p_oGeneralData.GetProperty("U_ImpSECom"),
                    U_FinalizaAct2Click = (String)p_oGeneralData.GetProperty("U_FinalizaAct2Click"),
                    U_AgrgTiempFin = (String)p_oGeneralData.GetProperty("U_AgrgTiempFin"),
                    U_AddMecNEstado = (String)p_oGeneralData.GetProperty("U_AddMecNEstado"),
                    U_DesAproSE = (String)p_oGeneralData.GetProperty("U_DesAproSE"),
                    U_DesAproSER = (String)p_oGeneralData.GetProperty("U_DesAproSER"),
                    U_AgendaColor = (String)p_oGeneralData.GetProperty("U_AgendaColor"),
                    U_GenReqCOV = (String)p_oGeneralData.GetProperty("U_GenReqCOV"),
                    U_CloseSOL = (String)p_oGeneralData.GetProperty("U_CloseSOL"),
                    U_UsaLAOV = (String)p_oGeneralData.GetProperty("U_UsaLAOV"),
                    U_UsaPreAutSE = (String)p_oGeneralData.GetProperty("U_UsaPreAutSE"),
                    U_PCanOTAct = (String)p_oGeneralData.GetProperty("U_PCanOTAct"),
                    U_ManageColorBy = (String)p_oGeneralData.GetProperty("U_ManageColorBy"),
                    U_UsePrepicking = (String)p_oGeneralData.GetProperty("U_UsePrepicking"),
                    U_MTechnician = (String)p_oGeneralData.GetProperty("U_MTechnician"),
                    U_PrepickingSS = (String)p_oGeneralData.GetProperty("U_PrepickingSS"),
                    U_PrepickingCS = (String)p_oGeneralData.GetProperty("U_PrepickingCS"),
                    U_UsaEstadosOTP = (String)p_oGeneralData.GetProperty("U_UsaEstadosOTP"),
                    U_ExtraHourRate = (Double)p_oGeneralData.GetProperty("U_ExtraHourRate"),
                    U_CalHT = (String)p_oGeneralData.GetProperty("U_CalHT"),
                    U_DraftD = (String)p_oGeneralData.GetProperty("U_DraftD"),
                    U_DirectTransfer = (String)p_oGeneralData.GetProperty("U_DirectTransfer"),
                    Aprobaciones_Sucursal = Carga_Aprobaciones_Sucursal(p_oGeneralData.Child("SCGD_CONF_APROBAC")),
                    Bodegas_CentroCosto = Carga_Bodegas_CentroCosto(p_oGeneralData.Child("SCGD_CONF_BODXCC")),
                    Configuracion_OT_Interna = Carga_OT_Interna(p_oGeneralData.Child("SCGD_CONF_OT_INT")),
                    Configuracion_Tipo_Orden = Carga_Tipo_Orden(p_oGeneralData.Child("SCGD_CONF_TIP_ORDEN"))
                    
                };
                InicializarHorarioSucursal(ref confiSucursal);
                return confiSucursal;

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public static void InicializarHorarioSucursal(ref Configuracion_Sucursal ConfiguracionSucursal)
        {
            DateTime HorarioApertura;
            DateTime HorarioCierre;
            DateTime HoraInicioAlmuerzo;
            DateTime HoraFinAlmuerzo;
            try
            {
                if (ConfiguracionSucursal != null)
                {
                    ConfiguracionSucursal.HorarioSucursal = new Dictionary<DayOfWeek, Horario>();
                    //Horario de semana laboral (Lunes a Viernes)
                    HorarioApertura = (DateTime)ConfiguracionSucursal.U_HoraInicio;
                    HorarioCierre = (DateTime)ConfiguracionSucursal.U_HoraFin;
                    HoraInicioAlmuerzo = (DateTime)ConfiguracionSucursal.U_HorAlI;
                    HoraFinAlmuerzo = (DateTime)ConfiguracionSucursal.U_HoraAlF;

                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Monday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));
                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Tuesday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));
                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Wednesday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));
                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Thursday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));
                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Friday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));

                    //Horario Sábados                   
                    HorarioApertura = (DateTime)ConfiguracionSucursal.U_HoraIS;
                    HorarioCierre = (DateTime)ConfiguracionSucursal.U_HoraFS;
                    HoraInicioAlmuerzo = (DateTime)ConfiguracionSucursal.U_HorAlI;
                    HoraFinAlmuerzo = (DateTime)ConfiguracionSucursal.U_HoraAlF;

                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Saturday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));

                    //Horario Domingos
                    HorarioApertura = (DateTime)ConfiguracionSucursal.U_HoraID;
                    HorarioCierre = (DateTime)ConfiguracionSucursal.U_HoraFD;
                    HoraInicioAlmuerzo = (DateTime)ConfiguracionSucursal.U_HorAlI;
                    HoraFinAlmuerzo = (DateTime)ConfiguracionSucursal.U_HoraAlF;

                    ConfiguracionSucursal.HorarioSucursal.Add(DayOfWeek.Sunday, new Horario(HorarioApertura, HorarioCierre, HoraInicioAlmuerzo, HoraFinAlmuerzo));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de Aprobaciones
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de aprobaciones</param>
        /// <returns>Lista con configuraciones de aprobaciones</returns>
        private static List<Aprobaciones_Sucursal> Carga_Aprobaciones_Sucursal(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Aprobaciones_Sucursal> aprobacionesSucursalList;

            try
            {
                aprobacionesSucursalList = new List<Aprobaciones_Sucursal>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    aprobacionesSucursalList.Add(new Aprobaciones_Sucursal
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_TipoOT = (String)oChildCc.GetProperty("U_TipoOT"),
                        U_ItmAprob = (String)oChildCc.GetProperty("U_ItmAprob"),
                        U_EspAprob = (String)oChildCc.GetProperty("U_EspAprob")
                    });

                }
                return aprobacionesSucursalList;
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de Bodegas por Centro de Costo
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de bodegas por centro de costo</param>
        /// <returns>Lista con configuraciones de bodegas por centro de costo</returns>
        private static List<Bodegas_CentroCosto> Carga_Bodegas_CentroCosto(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Bodegas_CentroCosto> bodegasCentroCostoList;

            try
            {
                bodegasCentroCostoList = new List<Bodegas_CentroCosto>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    bodegasCentroCostoList.Add(new Bodegas_CentroCosto
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_CC = (String)oChildCc.GetProperty("U_CC"),
                        U_Rep = (String)oChildCc.GetProperty("U_Rep"),
                        U_Ser = (String)oChildCc.GetProperty("U_Ser"),
                        U_Sum = (String)oChildCc.GetProperty("U_Sum"),
                        U_SE = (String)oChildCc.GetProperty("U_SE"),
                        U_Pro = (String)oChildCc.GetProperty("U_Pro"),
                        U_Res = (String)oChildCc.GetProperty("U_Res"),
                        U_UsaUbic = (String)oChildCc.GetProperty("U_UsaUbic"),
                        U_UbiDBP = (String)oChildCc.GetProperty("U_UbiDBP")
                    });

                }
                return bodegasCentroCostoList;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de OTs Internas
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de OTs internas</param>
        /// <returns>Lista con configuraciones de OTs internas</returns>
        private static List<Configuracion_OT_Interna> Carga_OT_Interna(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Configuracion_OT_Interna> configuracionOtInternaList;

            try
            {
                configuracionOtInternaList = new List<Configuracion_OT_Interna>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    configuracionOtInternaList.Add(new Configuracion_OT_Interna
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Tipo_OT = (String)oChildCc.GetProperty("U_Tipo_OT"),
                        U_Tran_Com = (String)oChildCc.GetProperty("U_Tran_Com"),
                        U_NumCuent = (String)oChildCc.GetProperty("U_NumCuent")
                    });

                }
                return configuracionOtInternaList;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de configuraciones de Tipo de Orden
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de tipo de orden</param>
        /// <returns>Lista con configuraciones de tipo de orden</returns>
        private static List<Configuracion_Tipo_Orden> Carga_Tipo_Orden(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Configuracion_Tipo_Orden> configuracionTipoOrdenList;

            try
            {
                configuracionTipoOrdenList = new List<Configuracion_Tipo_Orden>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    configuracionTipoOrdenList.Add(new Configuracion_Tipo_Orden
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                        U_Code = (Int32)oChildCc.GetProperty("U_Code"),
                        U_Name = (String)oChildCc.GetProperty("U_Name"),
                        U_UsaDim = (String)oChildCc.GetProperty("U_UsaDim"),
                        U_Interna = (String)oChildCc.GetProperty("U_Interna"),
                        U_UsDmAEM = (String)oChildCc.GetProperty("U_UsDmAEM"),
                        U_UsDmAFP = (String)oChildCc.GetProperty("U_UsDmAFP"),
                        U_UsaDOFV = (String)oChildCc.GetProperty("U_UsaDOFV"),
                        U_CodCtCos = (String)oChildCc.GetProperty("U_CodCtCos"),
                        U_CodClien = (String)oChildCc.GetProperty("U_CodClien"),
                        U_UsaLstPre = (String)oChildCc.GetProperty("U_UsaListaPre")
                    });

                }
                return configuracionTipoOrdenList;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #region "Carga de Configuraciones Generales"

        /// <summary>
        /// Método que inicializa la Lista con los Tipos de Orden de Trabajo
        /// </summary>
        public static void CargaTipoOT()
        {
            try
            {
                TipoOt = new List<TipoOT>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strTipoOT")).Rows)
                {
                    TipoOt.Add(new TipoOT
                    {
                        Code = drRow[0].ToString(),
                        Name = drRow[1].ToString(),
                        U_Interna = drRow[2].ToString(),
                        U_UsaDim = drRow[3].ToString(),
                        U_UsaDimAEM = drRow[4].ToString(),
                        U_UsaDimAFP = drRow[5].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
            }
        }

        /// <summary>
        /// Método que inicializa la lista con los estados del campo trasladado
        /// </summary>
        public static void CargaEstadosTrasladado()
        {
            try
            {
                Trasladado = new List<Trasladado>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strTrasladado")).Rows)
                {
                    Trasladado.Add(new Trasladado
                    {
                        Code = drRow[0].ToString(),
                        Name = drRow[1].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
            }

        }

        /// <summary>
        /// Método que inicializa la lista con los estados del campo aprobado
        /// </summary>
        public static void CargaEstadosAprobado()
        {
            try
            {
                Aprobado = new List<Aprobado>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strAprobado")).Rows)
                {
                    Aprobado.Add(new Aprobado
                    {
                        Code = drRow[0].ToString(),
                        Name = drRow[1].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
            }
        }

        #endregion

        #region "Carga de configuración de Mensajería"

        /// <summary>
        /// Método que inicializa las Configuraciones Mensajería
        /// </summary>
        public static void Carga_ConfiguracionMensajeria()
        {

            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);
            List<Mensajeria> lstConfMensajeria;
            List<int> lstDocEntryConfMensajeria;
            try
            {
                lstDocEntryConfMensajeria = new List<int>();
                lstConfMensajeria = new List<Mensajeria>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strDocEntryConfMensajeria")).Rows)
                    if (drRow["DocEntry"] != DBNull.Value)
                        lstDocEntryConfMensajeria.Add(Convert.ToInt32(drRow["DocEntry"]));

                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CMSJ");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                foreach (int index in lstDocEntryConfMensajeria)
                {
                    oGeneralParams.SetProperty("DocEntry", index);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    lstConfMensajeria.Add(Carga_ConfMensajeria(ref oGeneralData));
                }
                ConfMensajeria = lstConfMensajeria;
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO Mensajería</param>
        /// <returns>DataContract del UDO Mensajería</returns>
        private static Mensajeria Carga_ConfMensajeria(ref GeneralData p_oGeneralData)
        {
            Mensajeria mensajeria;
            try
            {
                mensajeria = new Mensajeria
                {
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    DocNum = (Int32?)p_oGeneralData.GetProperty("DocNum"),
                    Period = (Int32?)p_oGeneralData.GetProperty("Period"),
                    Series = (Int32?)p_oGeneralData.GetProperty("Series"),
                    Handwrtten = (String)p_oGeneralData.GetProperty("Handwrtten"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32?)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32?)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    Status = (String)p_oGeneralData.GetProperty("Status"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_IdSuc = (String)p_oGeneralData.GetProperty("U_IdSuc"),
                    U_IdRol = (String)p_oGeneralData.GetProperty("U_IdRol"),
                    Mensajeria_Lineas = Carga_LineasMensajeria(p_oGeneralData.Child("SCGD_CONF_MSJLN"))
                };
                return mensajeria;

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de las líneas de Mensajería
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de las líneas de mensajería</param>
        /// <returns>Lista con configuraciones de las líneas de mensajería</returns>
        private static List<Mensajeria_Lineas> Carga_LineasMensajeria(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Mensajeria_Lineas> lineasMensajeria;

            try
            {
                lineasMensajeria = new List<Mensajeria_Lineas>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    lineasMensajeria.Add(new Mensajeria_Lineas
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32?)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_IDRol = (String)oChildCc.GetProperty("U_IDRol"),
                        U_IDUSR = (String)oChildCc.GetProperty("U_IDUSR"),
                        U_Usr_Name = (String)oChildCc.GetProperty("U_Usr_Name"),
                        U_EmpCode = (String)oChildCc.GetProperty("U_EmpCode"),
                        U_Usr_UsrName = (String)oChildCc.GetProperty("U_Usr_UsrName")
                    });

                }
                return lineasMensajeria;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #region "Carga de configuración de Numeraciones"

        /// <summary>
        /// Método que inicializa las Configuraciones de Numeración
        /// </summary>
        public static void Carga_ConfiguracionNumeraciones()
        {

            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);
            List<Numeracion> lstConfNumeracion;
            List<int> lstDocEntryConfNumeracion;
            try
            {
                lstDocEntryConfNumeracion = new List<int>();
                lstConfNumeracion = new List<Numeracion>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strDocEntryConfNumeracion")).Rows)
                    if (drRow["DocEntry"] != DBNull.Value)
                        lstDocEntryConfNumeracion.Add(Convert.ToInt32(drRow["DocEntry"]));

                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_ONNM");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                foreach (int index in lstDocEntryConfNumeracion)
                {
                    oGeneralParams.SetProperty("DocEntry", index);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    lstConfNumeracion.Add(Carga_ConfNumeracion(ref oGeneralData));
                }
                Numeracion = lstConfNumeracion;
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO numeración</param>
        /// <returns>DataContract del UDO numeración</returns>
        private static Numeracion Carga_ConfNumeracion(ref GeneralData p_oGeneralData)
        {
            Numeracion numeracion;
            try
            {
                numeracion = new Numeracion
                {
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    DocNum = (Int32?)p_oGeneralData.GetProperty("DocNum"),
                    Period = (Int32?)p_oGeneralData.GetProperty("Period"),
                    Series = (Int32?)p_oGeneralData.GetProperty("Series"),
                    LogInst = (Int32?)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32?)p_oGeneralData.GetProperty("UserSign"),
                    Status = (String)p_oGeneralData.GetProperty("Status"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_Objeto = (String)p_oGeneralData.GetProperty("U_Objeto"),
                    Numeracion_Lineas = Carga_LineasNumeracion(p_oGeneralData.Child("SCGD_LINEAS_NUM"))
                };
                return numeracion;

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de las líneas de Numeración
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de las líneas de numeración</param>
        /// <returns>Lista con configuraciones de las líneas de numeración</returns>
        private static List<Numeracion_Lineas> Carga_LineasNumeracion(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Numeracion_Lineas> lineasNumeracion;

            try
            {
                lineasNumeracion = new List<Numeracion_Lineas>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    lineasNumeracion.Add(new Numeracion_Lineas
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32?)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_Sucu = (String)oChildCc.GetProperty("U_Sucu"),
                        U_Ini = (String)oChildCc.GetProperty("U_Ini"),
                        U_Fin = (String)oChildCc.GetProperty("U_Fin"),
                        U_Sig = (String)oChildCc.GetProperty("U_Sig"),
                    });

                }
                return lineasNumeracion;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #region "Carga de configuración de Dimensiones Contables"

        /// <summary>
        /// Método que valida si el sistema utiliza dimensiones contables para inicializar las Entidades
        /// </summary>
        public static void Carga_Dimensiones()
        {
            if (ParamGenAddon.U_UsaDimC.Trim().Equals("Y"))
            {
                Carga_ConfiguracionDimensiones();
                Carga_ConfiguracionDimensionesOT();
            }
        }

        #region "Dimensiones"
        /// <summary>
        /// Método que inicializa las Configuraciones de Dimensiones
        /// </summary>
        private static void Carga_ConfiguracionDimensiones()
        {

            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);
            List<Dimensiones> lstConfDimensiones;
            List<int> lstDocEntryConfDimensiones;
            try
            {
                lstDocEntryConfDimensiones = new List<int>();
                lstConfDimensiones = new List<Dimensiones>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strDocEntryConfDimensiones")).Rows)
                    if (drRow["DocEntry"] != DBNull.Value)
                        lstDocEntryConfDimensiones.Add(Convert.ToInt32(drRow["DocEntry"]));

                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_DIM");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                foreach (int index in lstDocEntryConfDimensiones)
                {
                    oGeneralParams.SetProperty("DocEntry", index);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    lstConfDimensiones.Add(Carga_ConfDimensiones(ref oGeneralData));
                }
                Dimensiones = lstConfDimensiones;
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO Dimensiones</param>
        /// <returns>DataContract del UDO Dimensiones</returns>
        private static Dimensiones Carga_ConfDimensiones(ref GeneralData p_oGeneralData)
        {
            Dimensiones dimensiones;
            try
            {
                dimensiones = new Dimensiones
                {
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    DocNum = (Int32?)p_oGeneralData.GetProperty("DocNum"),
                    Period = (Int32?)p_oGeneralData.GetProperty("Period"),
                    Series = (Int32?)p_oGeneralData.GetProperty("Series"),
                    Handwrtten = (String)p_oGeneralData.GetProperty("Handwrtten"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32?)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32?)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    Status = (String)p_oGeneralData.GetProperty("Status"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_Tip_Inv = (String)p_oGeneralData.GetProperty("U_Tip_Inv"),
                    Dimensiones_Lineas = Carga_LineasDimensiones(p_oGeneralData.Child("SCGD_LINEAS_DIMEN"))
                };
                return dimensiones;

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de las líneas de Dimensiones
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de las líneas de dimensiones</param>
        /// <returns>Lista con configuraciones de las líneas de dimensiones</returns>
        private static List<Dimensiones_Lineas> Carga_LineasDimensiones(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<Dimensiones_Lineas> lineasDimensiones;

            try
            {
                lineasDimensiones = new List<Dimensiones_Lineas>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    lineasDimensiones.Add(new Dimensiones_Lineas
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32?)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_CodMar = (String)oChildCc.GetProperty("U_CodMar"),
                        U_DesMar = (String)oChildCc.GetProperty("U_DesMar"),
                        U_Dim1 = (String)oChildCc.GetProperty("U_Dim1"),
                        U_Dim2 = (String)oChildCc.GetProperty("U_Dim2"),
                        U_Dim3 = (String)oChildCc.GetProperty("U_Dim3"),
                        U_Dim4 = (String)oChildCc.GetProperty("U_Dim4"),
                        U_Dim5 = (String)oChildCc.GetProperty("U_Dim5")
                    });

                }
                return lineasDimensiones;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #region "DimensionesOT"
        /// <summary>
        /// Método que inicializa las Configuraciones de Dimensiones
        /// </summary>
        private static void Carga_ConfiguracionDimensionesOT()
        {

            CompanyService oCompanyService = default(CompanyService);
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);
            List<DimensionesOT> lstConfDimensionesOT;
            List<int> lstDocEntryConfDimensiones;
            try
            {
                lstDocEntryConfDimensiones = new List<int>();
                lstConfDimensionesOT = new List<DimensionesOT>();
                foreach (DataRow drRow in Helpers.EjecutarConsultaDataTable(Queries.GetStrSpecificQuery("strDocEntryConfDimensionesOT")).Rows)
                    if (drRow["DocEntry"] != DBNull.Value)
                        lstDocEntryConfDimensiones.Add(Convert.ToInt32(drRow["DocEntry"]));

                oCompanyService = Company.CompanySBO.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SCGD_DOT");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                foreach (int index in lstDocEntryConfDimensiones)
                {
                    oGeneralParams.SetProperty("DocEntry", index);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    lstConfDimensionesOT.Add(Carga_ConfDimensionesOT(ref oGeneralData));
                }
                DimensionesOT = lstConfDimensionesOT;
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
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
        /// <param name="p_oGeneralData">GeneralData del UDO DimensionesOT</param>
        /// <returns>DataContract del UDO DimensionesOT</returns>
        private static DimensionesOT Carga_ConfDimensionesOT(ref GeneralData p_oGeneralData)
        {
            DimensionesOT dimensionesOT;
            try
            {
                dimensionesOT = new DimensionesOT
                {
                    DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                    DocNum = (Int32?)p_oGeneralData.GetProperty("DocNum"),
                    Period = (Int32?)p_oGeneralData.GetProperty("Period"),
                    Series = (Int32?)p_oGeneralData.GetProperty("Series"),
                    Handwrtten = (String)p_oGeneralData.GetProperty("Handwrtten"),
                    Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                    LogInst = (Int32?)p_oGeneralData.GetProperty("LogInst"),
                    UserSign = (Int32?)p_oGeneralData.GetProperty("UserSign"),
                    Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                    Status = (String)p_oGeneralData.GetProperty("Status"),
                    DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                    U_CodSuc = (String)p_oGeneralData.GetProperty("U_CodSuc"),
                    DimensionesOT_Lineas = Carga_LineasDimensionesOT(p_oGeneralData.Child("SCGD_LINEAS_DIMENOT"))
                };
                return dimensionesOT;

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Función que retorna lista de las líneas de Dimensiones
        /// </summary>
        /// <param name="p_oChildrenCtrlCol">GeneralData de las líneas de dimensionesOT</param>
        /// <returns>Lista con configuraciones de las líneas de dimensionesOT</returns>
        private static List<DimensionesOT_Lineas> Carga_LineasDimensionesOT(GeneralDataCollection p_oChildrenCtrlCol)
        {
            GeneralData oChildCc;
            List<DimensionesOT_Lineas> lineasdimensionesOT;
            try
            {
                lineasdimensionesOT = new List<DimensionesOT_Lineas>();
                for (int index = 0; index <= p_oChildrenCtrlCol.Count - 1; index++)
                {
                    oChildCc = p_oChildrenCtrlCol.Item(index);
                    lineasdimensionesOT.Add(new DimensionesOT_Lineas
                    {
                        DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                        LineId = (Int32)oChildCc.GetProperty("LineId"),
                        VisOrder = (Int32?)oChildCc.GetProperty("VisOrder"),
                        LogInst = (Int32?)oChildCc.GetProperty("LogInst"),
                        U_Dim1 = (String)oChildCc.GetProperty("U_Dim1"),
                        U_Dim2 = (String)oChildCc.GetProperty("U_Dim2"),
                        U_Dim3 = (String)oChildCc.GetProperty("U_Dim3"),
                        U_Dim4 = (String)oChildCc.GetProperty("U_Dim4"),
                        U_Dim5 = (String)oChildCc.GetProperty("U_Dim5"),
                        U_CodMar = (String)oChildCc.GetProperty("U_CodMar"),
                        U_DesMar = (String)oChildCc.GetProperty("U_DesMar"),
                    });

                }
                return lineasdimensionesOT;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        #endregion

        #endregion

        public static List<DateTime> ObtenerFeriados()
        {
            List<DateTime> ListaFeriados = new List<DateTime>();
            SAPbobsCOM.Recordset oRecordset;
            string Query = "SELECT \"U_Date\" FROM \"@SCGD_HOLIDAYS\" ";

            try
            {
                oRecordset = (SAPbobsCOM.Recordset)DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(Query);
                if (oRecordset.RecordCount > 0)
                {
                    while (!oRecordset.EoF)
                    {
                        ListaFeriados.Add((DateTime)oRecordset.Fields.Item("U_Date").Value);
                        oRecordset.MoveNext();
                    }
                }

                return ListaFeriados;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return ListaFeriados;
            }
        }
        
    }
}
