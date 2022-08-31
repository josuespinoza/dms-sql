using System;
using System.Collections.Generic;
using DMS_Connector;
using DMS_Connector.Business_Logic.DataContract.Contrato_de_Ventas;

public class Carga_Contrato
{
    /// <summary>
    /// Función que retorna el DataContract del Contrato de Ventas Solicitado
    /// </summary>
    /// <param name="p_intDocEntry">DocEntry del Contrato de Ventas a retornar</param>
    /// <returns>DataContract del Contrato de Ventas solicitado</returns>
    public static CVenta Carga_ContratoVentas(int p_intDocEntry)
    {

        SAPbobsCOM.CompanyService oCompanyService = default(SAPbobsCOM.CompanyService);
        SAPbobsCOM.GeneralService oGeneralService = default(SAPbobsCOM.GeneralService);
        SAPbobsCOM.GeneralData oGeneralData = default(SAPbobsCOM.GeneralData);
        SAPbobsCOM.GeneralDataParams oGeneralParams = default(SAPbobsCOM.GeneralDataParams);

        try
        {
            oCompanyService = Company.CompanySBO.GetCompanyService();
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT");
            oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
            oGeneralParams.SetProperty("Code", p_intDocEntry);
            oGeneralData = oGeneralService.GetByParams(oGeneralParams);

            return Carga_ContratoDT(ref oGeneralData);

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
    /// <param name="p_oGeneralData">GeneralData del Contrato de Ventas consultado</param>
    /// <returns>DataContract del Contrato  de Ventas solicitado</returns>
    private static CVenta Carga_ContratoDT(ref SAPbobsCOM.GeneralData p_oGeneralData)
    {
        CVenta oContrato = default(CVenta);
        try
        {
            oContrato = new CVenta
            {
                DocEntry = (Int32)p_oGeneralData.GetProperty("DocEntry"),
                DocNum = (Int32)p_oGeneralData.GetProperty("DocNum"),
                Period = (Int32)p_oGeneralData.GetProperty("Period"),
                Series = (Int32)p_oGeneralData.GetProperty("Series"),
                Handwrtten = (String)p_oGeneralData.GetProperty("Handwrtten"),
                Canceled = (String)p_oGeneralData.GetProperty("Canceled"),
                Object = (String)p_oGeneralData.GetProperty("Object"),
                LogInst = (Int32)p_oGeneralData.GetProperty("LogInst"),
                UserSign = (Int32)p_oGeneralData.GetProperty("UserSign"),
                Transfered = (String)p_oGeneralData.GetProperty("Transfered"),
                Status = (String)p_oGeneralData.GetProperty("Status"),
                DataSource = (String)p_oGeneralData.GetProperty("DataSource"),
                U_CardCode = (String)p_oGeneralData.GetProperty("U_CardCode"),
                U_CardName = (String)p_oGeneralData.GetProperty("U_CardName"),
                U_Tipo = (Int32)p_oGeneralData.GetProperty("U_Tipo"),
                U_Estado = (Int32)p_oGeneralData.GetProperty("U_Estado"),
                U_Opcion = (String)p_oGeneralData.GetProperty("U_Opcion"),
                U_Pre_Vta = (Double)p_oGeneralData.GetProperty("U_Pre_Vta"),
                U_Ext_Adi = (Double)p_oGeneralData.GetProperty("U_Ext_Adi"),
                U_Gas_Ins = (Double)p_oGeneralData.GetProperty("U_Gas_Ins"),
                U_Gas_Seg = (Double)p_oGeneralData.GetProperty("U_Gas_Seg"),
                U_Gas_Pre = (Double)p_oGeneralData.GetProperty("U_Gas_Pre"),
                U_DocTotal = (Double)p_oGeneralData.GetProperty("U_DocTotal"),
                U_Saldo = (Double)p_oGeneralData.GetProperty("U_Saldo"),
                U_Observ = (String)p_oGeneralData.GetProperty("U_Observ"),
                U_Dat_Pre = (String)p_oGeneralData.GetProperty("U_Dat_Pre"),
                U_SlpCode = (Int16)p_oGeneralData.GetProperty("U_SlpCode"),
                U_SlpName = (String)p_oGeneralData.GetProperty("U_SlpName"),
                U_DocDate = (DateTime)p_oGeneralData.GetProperty("U_DocDate"),
                U_Fec_Ent = (DateTime)p_oGeneralData.GetProperty("U_Fec_Ent"),
                U_Cod_Unid = (String)p_oGeneralData.GetProperty("U_Cod_Unid"),
                U_Cod_Marc = (String)p_oGeneralData.GetProperty("U_Cod_Marc"),
                U_Des_Marc = (String)p_oGeneralData.GetProperty("U_Des_Marc"),
                U_Cod_Mode = (String)p_oGeneralData.GetProperty("U_Cod_Mode"),
                U_Des_Mode = (String)p_oGeneralData.GetProperty("U_Des_Mode"),
                U_Cod_Esti = (String)p_oGeneralData.GetProperty("U_Cod_Esti"),
                U_Des_Esti = (String)p_oGeneralData.GetProperty("U_Des_Esti"),
                U_Ano_Vehi = (Int16)p_oGeneralData.GetProperty("U_Ano_Vehi"),
                U_Num_Plac = (String)p_oGeneralData.GetProperty("U_Num_Plac"),
                U_Cod_Col = (String)p_oGeneralData.GetProperty("U_Cod_Col"),
                U_Des_Col = (String)p_oGeneralData.GetProperty("U_Des_Col"),
                U_Num_VIN = (String)p_oGeneralData.GetProperty("U_Num_VIN"),
                U_Num_Mot = (String)p_oGeneralData.GetProperty("U_Num_Mot"),
                U_Mar_Brt = (Double)p_oGeneralData.GetProperty("U_Mar_Brt"),
                U_Mon_Fin = (Double)p_oGeneralData.GetProperty("U_Mon_Fin"),
                U_Ent_Fin = (String)p_oGeneralData.GetProperty("U_Ent_Fin"),
                U_Mon_pre = (Double)p_oGeneralData.GetProperty("U_Mon_pre"),
                U_Plazo = (Int32)p_oGeneralData.GetProperty("U_Plazo"),
                U_Tas_Anu = (Double)p_oGeneralData.GetProperty("U_Tas_Anu"),
                U_Abo_Men = (Double)p_oGeneralData.GetProperty("U_Abo_Men"),
                U_Cuo_Tot = (Double)p_oGeneralData.GetProperty("U_Cuo_Tot"),
                U_Seg_Pre = (Double)p_oGeneralData.GetProperty("U_Seg_Pre"),
                U_Obs_Pre = (String)p_oGeneralData.GetProperty("U_Obs_Pre"),
                U_Unid_Us = (String)p_oGeneralData.GetProperty("U_Unid_Us"),
                U_Marc_Us = (String)p_oGeneralData.GetProperty("U_Marc_Us"),
                U_Esti_us = (String)p_oGeneralData.GetProperty("U_Esti_us"),
                U_Mot_Us = (String)p_oGeneralData.GetProperty("U_Mot_Us"),
                U_VIN_Us = (String)p_oGeneralData.GetProperty("U_VIN_Us"),
                U_Anio_Us = (Int16)p_oGeneralData.GetProperty("U_Anio_Us"),
                U_Plac_Us = (String)p_oGeneralData.GetProperty("U_Plac_Us"),
                U_Col_Us = (String)p_oGeneralData.GetProperty("U_Col_Us"),
                U_Obs_Us = (String)p_oGeneralData.GetProperty("U_Obs_Us"),
                U_IDVehi = (String)p_oGeneralData.GetProperty("U_IDVehi"),
                U_Deposito = (Double)p_oGeneralData.GetProperty("U_Deposito"),
                U_Mon_Usa = (Double)p_oGeneralData.GetProperty("U_Mon_Usa"),
                U_Deu_Usa = (Double)p_oGeneralData.GetProperty("U_Deu_Usa"),
                U_Nota_Cre = (Double)p_oGeneralData.GetProperty("U_Nota_Cre"),
                U_Pag_ent = (Double)p_oGeneralData.GetProperty("U_Pag_ent"),
                U_Aval_us = (DateTime)p_oGeneralData.GetProperty("U_Aval_us"),
                U_RTV_MM = (Int16)p_oGeneralData.GetProperty("U_RTV_MM"),
                U_RTV_AAAA = (Int16)p_oGeneralData.GetProperty("U_RTV_AAAA"),
                U_Gravamen = (Int16)p_oGeneralData.GetProperty("U_Gravamen"),
                U_Val_Rec = (Double)p_oGeneralData.GetProperty("U_Val_Rec"),
                U_Val_Inv = (Double)p_oGeneralData.GetProperty("U_Val_Inv"),
                U_Usu_Tra = (String)p_oGeneralData.GetProperty("U_Usu_Tra"),
                U_Usu_Ven = (String)p_oGeneralData.GetProperty("U_Usu_Ven"),
                U_Usu_Gen = (String)p_oGeneralData.GetProperty("U_Usu_Gen"),
                U_Usu_Fac = (String)p_oGeneralData.GetProperty("U_Usu_Fac"),
                U_Usu_Can = (String)p_oGeneralData.GetProperty("U_Usu_Can"),
                U_Fec_Tra = (DateTime)p_oGeneralData.GetProperty("U_Fec_Tra"),
                U_Fec_Ven = (DateTime)p_oGeneralData.GetProperty("U_Fec_Ven"),
                U_Fec_Gen = (DateTime)p_oGeneralData.GetProperty("U_Fec_Gen"),
                U_Fec_Fac = (DateTime)p_oGeneralData.GetProperty("U_Fec_Fac"),
                U_Fec_Can = (DateTime)p_oGeneralData.GetProperty("U_Fec_Can"),
                U_Det_Ext = (String)p_oGeneralData.GetProperty("U_Det_Ext"),
                U_Gra_Fec = (DateTime)p_oGeneralData.GetProperty("U_Gra_Fec"),
                U_Financia = (Double)p_oGeneralData.GetProperty("U_Financia"),
                U_Gas_Loc = (Double)p_oGeneralData.GetProperty("U_Gas_Loc"),
                U_Cos_Acc = (Double)p_oGeneralData.GetProperty("U_Cos_Acc"),
                U_No_Fac = (String)p_oGeneralData.GetProperty("U_No_Fac"),
                U_Obs_GV = (String)p_oGeneralData.GetProperty("U_Obs_GV"),
                U_Obs_GG = (String)p_oGeneralData.GetProperty("U_Obs_GG"),
                U_Moneda = (String)p_oGeneralData.GetProperty("U_Moneda"),
                U_Ent_FiP = (String)p_oGeneralData.GetProperty("U_Ent_FiP"),
                U_Tip_Tasa = (String)p_oGeneralData.GetProperty("U_Tip_Tasa"),
                U_Der_Cir = (DateTime)p_oGeneralData.GetProperty("U_Der_Cir"),
                U_Cat_Us = (String)p_oGeneralData.GetProperty("U_Cat_Us"),
                U_Tip_Us = (String)p_oGeneralData.GetProperty("U_Tip_Us"),
                U_Trac_Us = (String)p_oGeneralData.GetProperty("U_Trac_Us"),
                U_Cab_Us = (String)p_oGeneralData.GetProperty("U_Cab_Us"),
                U_Prop_Us = (String)p_oGeneralData.GetProperty("U_Prop_Us"),
                U_CEnt_Fi = (String)p_oGeneralData.GetProperty("U_CEnt_Fi"),
                U_CEnt_FP = (String)p_oGeneralData.GetProperty("U_CEnt_FP"),
                U_CCl_Veh = (String)p_oGeneralData.GetProperty("U_CCl_Veh"),
                U_NCl_Veh = (String)p_oGeneralData.GetProperty("U_NCl_Veh"),
                U_Fec_1Ab = (DateTime)p_oGeneralData.GetProperty("U_Fec_1Ab"),
                U_GroupNum = (Int32)p_oGeneralData.GetProperty("U_GroupNum"),
                U_Cod_FP = (String)p_oGeneralData.GetProperty("U_Cod_FP"),
                U_Nota_Deb = (Double)p_oGeneralData.GetProperty("U_Nota_Deb"),
                U_Cod_NotD = (String)p_oGeneralData.GetProperty("U_Cod_NotD"),
                U_Nam_Acre = (String)p_oGeneralData.GetProperty("U_Nam_Acre"),
                U_Cod_Nota = (String)p_oGeneralData.GetProperty("U_Cod_Nota"),
                U_Cod_N_Us = (String)p_oGeneralData.GetProperty("U_Cod_N_Us"),
                U_ID_VehUs = (String)p_oGeneralData.GetProperty("U_ID_VehUs"),
                U_Reaproba = (String)p_oGeneralData.GetProperty("U_Reaproba"),
                U_Cod_Acre = (String)p_oGeneralData.GetProperty("U_Cod_Acre"),
                U_OwrCode = (String)p_oGeneralData.GetProperty("U_OwrCode"),
                U_OwrName = (String)p_oGeneralData.GetProperty("U_OwrName"),
                U_Transmis = (String)p_oGeneralData.GetProperty("U_Transmis"),
                U_Cod_A_DU = (String)p_oGeneralData.GetProperty("U_Cod_A_DU"),
                U_Cod_A_Co = (String)p_oGeneralData.GetProperty("U_Cod_A_Co"),
                U_Cod_OV = (String)p_oGeneralData.GetProperty("U_Cod_OV"),
                U_Name_OV = (String)p_oGeneralData.GetProperty("U_Name_OV"),
                U_Otros_C = (Double)p_oGeneralData.GetProperty("U_Otros_C"),
                U_Otros_L = (Double)p_oGeneralData.GetProperty("U_Otros_L"),
                U_Pagos = (Double)p_oGeneralData.GetProperty("U_Pagos"),
                U_PrecioVeh = (Double)p_oGeneralData.GetProperty("U_PrecioVeh"),
                U_Foo = (String)p_oGeneralData.GetProperty("U_Foo"),
                U_SCGD_CodCotiz = (String)p_oGeneralData.GetProperty("U_SCGD_CodCotiz"),
                U_SCGD_NameCotiz = (String)p_oGeneralData.GetProperty("U_SCGD_NameCotiz"),
                U_SCGD_TipoCambio = (Double)p_oGeneralData.GetProperty("U_SCGD_TipoCambio"),
                U_SCGD_NoSalida = (String)p_oGeneralData.GetProperty("U_SCGD_NoSalida"),
                U_SCGD_DocPreliminar = (String)p_oGeneralData.GetProperty("U_SCGD_DocPreliminar"),
                U_Reversa = (String)p_oGeneralData.GetProperty("U_Reversa"),
                U_Mon_Cot = (Double)p_oGeneralData.GetProperty("U_Mon_Cot"),
                U_Pre_Us = (Double)p_oGeneralData.GetProperty("U_Pre_Us"),
                U_Gravo = (String)p_oGeneralData.GetProperty("U_Gravo"),
                U_FooVend = (String)p_oGeneralData.GetProperty("U_FooVend"),
                U_Pre_Imp = (Double)p_oGeneralData.GetProperty("U_Pre_Imp"),
                U_SCGD_FDc = (DateTime)p_oGeneralData.GetProperty("U_SCGD_FDc"),
                U_Pre_Vh = (Double)p_oGeneralData.GetProperty("U_Pre_Vh"),
                U_Pago_Vh = (Double)p_oGeneralData.GetProperty("U_Pago_Vh"),
                U_Imp_Vh = (String)p_oGeneralData.GetProperty("U_Imp_Vh"),
                U_Fin_Prop = (String)p_oGeneralData.GetProperty("U_Fin_Prop"),
                U_DiaPago = (Int32)p_oGeneralData.GetProperty("U_DiaPago"),
                U_Int_Mor = (Double)p_oGeneralData.GetProperty("U_Int_Mor"),
                U_Tipo_Cuo = (String)p_oGeneralData.GetProperty("U_Tipo_Cuo"),
                U_Prestamo = (String)p_oGeneralData.GetProperty("U_Prestamo"),
                U_NC_Prima = (String)p_oGeneralData.GetProperty("U_NC_Prima"),
                U_SCGD_FDr = (DateTime)p_oGeneralData.GetProperty("U_SCGD_FDr"),
                U_GenFaAcc = (String)p_oGeneralData.GetProperty("U_GenFaAcc"),
                U_Det_Acc = (String)p_oGeneralData.GetProperty("U_Det_Acc"),
                U_Fact_Acc = (String)p_oGeneralData.GetProperty("U_Fact_Acc"),
                U_Por_Desc = (Double)p_oGeneralData.GetProperty("U_Por_Desc"),
                U_Por_Temp = (Double)p_oGeneralData.GetProperty("U_Por_Temp"),
                U_Acc_Desc = (Double)p_oGeneralData.GetProperty("U_Acc_Desc"),
                U_Acc_Imp = (Double)p_oGeneralData.GetProperty("U_Acc_Imp"),
                U_AcDescPo = (Double)p_oGeneralData.GetProperty("U_AcDescPo"),
                U_AcSinDes = (Double)p_oGeneralData.GetProperty("U_AcSinDes"),
                U_Acc_Temp = (Double)p_oGeneralData.GetProperty("U_Acc_Temp"),
                U_GL_Temp = (Double)p_oGeneralData.GetProperty("U_GL_Temp"),
                U_OG_Temp = (Double)p_oGeneralData.GetProperty("U_OG_Temp"),
                U_Fact_GA = (String)p_oGeneralData.GetProperty("U_Fact_GA"),
                U_TranU = (String)p_oGeneralData.GetProperty("U_TranU"),
                U_ModU = (String)p_oGeneralData.GetProperty("U_ModU"),
                U_ComU = (String)p_oGeneralData.GetProperty("U_ComU"),
                U_As_FExt = (String)p_oGeneralData.GetProperty("U_As_FExt"),
                U_TotalAcc = (Double)p_oGeneralData.GetProperty("U_TotalAcc"),
                U_Sucu = (String)p_oGeneralData.GetProperty("U_Sucu"),
                U_CSucu = (String)p_oGeneralData.GetProperty("U_CSucu"),
                U_Tot_Tram = (Double)p_oGeneralData.GetProperty("U_Tot_Tram"),
                U_As_Tram = (String)p_oGeneralData.GetProperty("U_As_Tram"),
                U_TFin = (String)p_oGeneralData.GetProperty("U_TFin"),
                U_FinE = (String)p_oGeneralData.GetProperty("U_FinE"),
                U_MontoC = (Double)p_oGeneralData.GetProperty("U_MontoC"),
                U_CodClasifUs = (String)p_oGeneralData.GetProperty("U_CodClasifUs"),
                U_ValVeh_Us = (Double)p_oGeneralData.GetProperty("U_ValVeh_Us"),
                U_BonoDV = (Double)p_oGeneralData.GetProperty("U_BonoDV"),
                U_BonoVV = (Double)p_oGeneralData.GetProperty("U_BonoVV"),
                U_BonoDV2 = (Double)p_oGeneralData.GetProperty("U_BonoDV2"),
                U_DescUni = (Double)p_oGeneralData.GetProperty("U_DescUni"),
                U_PreNet = (Double)p_oGeneralData.GetProperty("U_PreNet"),
                U_TipIn = (String)p_oGeneralData.GetProperty("U_TipIn"),
                U_ColIn = (String)p_oGeneralData.GetProperty("U_ColIn"),
                U_Obser = (String)p_oGeneralData.GetProperty("U_Obser"),
                U_PorDes = (String)p_oGeneralData.GetProperty("U_PorDes"),
                U_AntImp = (Double)p_oGeneralData.GetProperty("U_AntImp"),
                U_MonCBo = (Double)p_oGeneralData.GetProperty("U_MonCBo"),
                U_PreLis = (Double)p_oGeneralData.GetProperty("U_PreLis"),
                U_AsBon = (String)p_oGeneralData.GetProperty("U_AsBon"),
                U_MoNAs = (Double)p_oGeneralData.GetProperty("U_MoNAs"),
                U_MoNFi = (Double)p_oGeneralData.GetProperty("U_MoNFi"),
                U_AsCom = (String)p_oGeneralData.GetProperty("U_AsCom"),
                U_OtrCos = (Double)p_oGeneralData.GetProperty("U_OtrCos"),
                U_AsOCos = (String)p_oGeneralData.GetProperty("U_AsOCos"),
                U_ComAse = (String)p_oGeneralData.GetProperty("U_ComAse"),
                U_CorrSe = (String)p_oGeneralData.GetProperty("U_CorrSe"),
                U_CatVeUs = (String)p_oGeneralData.GetProperty("U_CatVeUs"),
                U_KmU = (Double)p_oGeneralData.GetProperty("U_KmU"),
                U_Km_Venta = (Double)p_oGeneralData.GetProperty("U_Km_Venta"),
                U_Cod_ProvUS = (String)p_oGeneralData.GetProperty("U_Cod_ProvUS"),
                U_Nom_ProvUS = (String)p_oGeneralData.GetProperty("U_Nom_ProvUS"),
                U_SCGD_ConEjeBan = (String)p_oGeneralData.GetProperty("U_SCGD_ConEjeBan"),
                U_SCGD_NrOC = (String)p_oGeneralData.GetProperty("U_SCGD_NrOC"),
                U_SCGD_NrOL = (String)p_oGeneralData.GetProperty("U_SCGD_NrOL"),
                U_FacTram = (String)p_oGeneralData.GetProperty("U_FacTram"),
                U_AsTraFc = (String)p_oGeneralData.GetProperty("U_AsTraFc"),
                U_NoFPVU = (String)p_oGeneralData.GetProperty("U_NoFPVU"),
                U_AsFPVU = (String)p_oGeneralData.GetProperty("U_AsFPVU"),
                U_SCGD_FechaOC = (DateTime)p_oGeneralData.GetProperty("U_SCGD_FechaOC"),
                U_SCGD_Detalle1 = (String)p_oGeneralData.GetProperty("U_SCGD_Detalle1"),
                U_SCGD_Detalle2 = (String)p_oGeneralData.GetProperty("U_SCGD_Detalle2"),
                U_TipoCV = (Int16)p_oGeneralData.GetProperty("U_TipoCV"),
                U_FinanciaE = (Double)p_oGeneralData.GetProperty("U_FinanciaE"),
                U_EntFinE = (String)p_oGeneralData.GetProperty("U_EntFinE"),
                U_FinanciaT = (Double)p_oGeneralData.GetProperty("U_FinanciaT"),
                U_FecInViSe = (DateTime)p_oGeneralData.GetProperty("U_FecInViSe"),
                U_CliConCod = (String)p_oGeneralData.GetProperty("U_CliConCod"),
                U_CliConNom = (String)p_oGeneralData.GetProperty("U_CliConNom"),
                U_CliConRUT = (String)p_oGeneralData.GetProperty("U_CliConRUT"),
                U_NumPropSe = (String)p_oGeneralData.GetProperty("U_NumPropSe"),
                U_Seg_FPT = (String)p_oGeneralData.GetProperty("U_Seg_FPT"),
                U_Seg_FPSF = (String)p_oGeneralData.GetProperty("U_Seg_FPSF"),
                U_SeFP_PC = (String)p_oGeneralData.GetProperty("U_SeFP_PC"),
                U_chk_PC = (String)p_oGeneralData.GetProperty("U_chk_PC"),
                U_BanChe_PC = (String)p_oGeneralData.GetProperty("U_BanChe_PC"),
                U_Se_NumIng = (String)p_oGeneralData.GetProperty("U_Se_NumIng"),
                U_Val_PC = (Double)p_oGeneralData.GetProperty("U_Val_PC"),
                U_Tas_AnuEX = (Double)p_oGeneralData.GetProperty("U_Tas_AnuEX"),
                U_PlazoEX = (Int32)p_oGeneralData.GetProperty("U_PlazoEX"),
                U_Fec_1AbEX = (DateTime)p_oGeneralData.GetProperty("U_Fec_1AbEX"),
                U_DiaPagoEX = (Int32)p_oGeneralData.GetProperty("U_DiaPagoEX"),
                U_Int_MorEX = (Double)p_oGeneralData.GetProperty("U_Int_MorEX"),
                U_Tipo_CuoEX = (String)p_oGeneralData.GetProperty("U_Tipo_CuoEX"),
                U_ImpUsado = (Double)p_oGeneralData.GetProperty("U_ImpUsado"),
                U_TotalCImpuest = (Double)p_oGeneralData.GetProperty("U_TotalCImpuest"),
                AccesoriosXContrato = Carga_AccesoriosXContrato(p_oGeneralData.Child("SCGD_ACCXCONT")),
                BonosXContrato = Carga_BonosXContrato(p_oGeneralData.Child("SCGD_BONOXCONT")),
                HistorialContrato = Carga_HistorialContrato(p_oGeneralData.Child("SCGD_HIST_CV")),
                LineasRes = Carga_LineasRes(p_oGeneralData.Child("SCGD_LINEASRES")),
                LineasSum = Carga_LineasSum(p_oGeneralData.Child("SCGD_LINEASSUM")),
                OtrosCostosXContrato = Carga_OtrosCostosXContrato(p_oGeneralData.Child("SCGD_OTROCXCV")),
                TramitesXContrato = Carga_TramitesXContrato(p_oGeneralData.Child("SCGD_TRAMXCONT")),
                UsadosXContrato = Carga_UsadosXContrato(p_oGeneralData.Child("SCGD_USADOXCONT")),
                VehiculosXContrato = Carga_VehiculosXContrato(p_oGeneralData.Child("SCGD_VEHIXCONT"))
            };
            return oContrato;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de Accesorios del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas de accesorios del Contrato de Ventas</param>
    /// <returns>Lista con los Accesosrios del Contrato de Ventas</returns>
    private static List<AccesoriosXContrato> Carga_AccesoriosXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<AccesoriosXContrato> accesoriosXContratoList = default(List<AccesoriosXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            accesoriosXContratoList = new List<AccesoriosXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                accesoriosXContratoList.Add(new AccesoriosXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Acc = (String)oChildCc.GetProperty("U_Acc"),
                    U_N_Acc = (String)oChildCc.GetProperty("U_N_Acc"),
                    U_SCGD_AccPrecio = (Double)oChildCc.GetProperty("U_SCGD_AccPrecio"),
                    U_Imp_Acc = (String)oChildCc.GetProperty("U_Imp_Acc"),
                    U_Cant_Acc = (Int32)oChildCc.GetProperty("U_Cant_Acc"),
                    U_AccPr_I = (Double)oChildCc.GetProperty("U_AccPr_I"),
                    U_Cost_Acc = (Double)oChildCc.GetProperty("U_Cost_Acc"),
                    U_Desc_Acc = (Double)oChildCc.GetProperty("U_Desc_Acc"),
                    U_PrTo_Acc = (Double)oChildCc.GetProperty("U_PrTo_Acc"),
                    U_Prov_Acc = (String)oChildCc.GetProperty("U_Prov_Acc"),
                    U_Comprar = (String)oChildCc.GetProperty("U_Comprar"),
                    U_Ord_Acc = (String)oChildCc.GetProperty("U_Ord_Acc"),
                    U_Imp_Com = (String)oChildCc.GetProperty("U_Imp_Com"),
                    U_CABYS_AE = (String)oChildCc.GetProperty("U_CABYS_AE"),
                    U_CABYS_TI = (String)oChildCc.GetProperty("U_CABYS_TI"),
                    U_CABYS_CH = (String)oChildCc.GetProperty("U_CABYS_CH")
                });

            }
            return accesoriosXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de Bonos del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas de los bonos del Contrato de Ventas</param>
    /// <returns>Lista con los bonos del Contrato de Ventas</returns>
    private static List<BonosXContrato> Carga_BonosXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<BonosXContrato> bonosXContratoList = default(List<BonosXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            bonosXContratoList = new List<BonosXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                bonosXContratoList.Add(new BonosXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Unidad = (String)oChildCc.GetProperty("U_Unidad"),
                    U_Bono = (String)oChildCc.GetProperty("U_Bono"),
                    U_Monto = (Double)oChildCc.GetProperty("U_Monto")
                });
            }
            return bonosXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista del Historial del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas del historial del Contrato de Ventas</param>
    /// <returns>Lista con el historial del Contrato de Ventas</returns>
    private static List<HistorialContrato> Carga_HistorialContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<HistorialContrato> historialContratoList = default(List<HistorialContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            historialContratoList = new List<HistorialContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                historialContratoList.Add(new HistorialContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Usuario = (String)oChildCc.GetProperty("U_Usuario"),
                    U_Hora = (DateTime?)oChildCc.GetProperty("U_Hora"),
                    U_Comentario = (String)oChildCc.GetProperty("U_Comentario"),
                    U_Nivel = (String)oChildCc.GetProperty("U_Nivel"),
                    U_Niv_Code = (String)oChildCc.GetProperty("U_Niv_Code"),
                    U_Fecha = (DateTime?)oChildCc.GetProperty("U_Fecha")
                });

            }
            return historialContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de las Líneas Resta del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas resta del Contrato de Ventas</param>
    /// <returns>Lista con las líneas resta del Contrato de Ventas</returns>
    private static List<LineasRes> Carga_LineasRes(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<LineasRes> lineasResList = default(List<LineasRes>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            lineasResList = new List<LineasRes>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                lineasResList.Add(new LineasRes
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                    U_Nom_Item = (String)oChildCc.GetProperty("U_Nom_Item"),
                    U_Descuent = (Double)oChildCc.GetProperty("U_Descuent"),
                    U_Monto = (Double)oChildCc.GetProperty("U_Monto"),
                    U_No_NC = (Int32)oChildCc.GetProperty("U_No_NC")
                });

            }
            return lineasResList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de las Líneas Suma del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con las líneas suma del Contrato de Ventas</param>
    /// <returns>Lista con las líneas suma del Contrato de Ventas</returns>
    private static List<LineasSum> Carga_LineasSum(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<LineasSum> lineasSumList = default(List<LineasSum>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            lineasSumList = new List<LineasSum>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                lineasSumList.Add(new LineasSum
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Cod_Item = (String)oChildCc.GetProperty("U_Cod_Item"),
                    U_Nom_Item = (String)oChildCc.GetProperty("U_Nom_Item"),
                    U_Descuent = (Double)oChildCc.GetProperty("U_Descuent"),
                    U_Monto = (Double)oChildCc.GetProperty("U_Monto"),
                    U_CodImp = (String)oChildCc.GetProperty("U_CodImp")
                });

            }
            return lineasSumList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de los Otros Costos del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con los otros costos del Contrato de Ventas</param>
    /// <returns>Lista con los otros costos del Contrato de Ventas</returns>
    private static List<OtrosCostosXContrato> Carga_OtrosCostosXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<OtrosCostosXContrato> otrosCostosXContratoList = default(List<OtrosCostosXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            otrosCostosXContratoList = new List<OtrosCostosXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                otrosCostosXContratoList.Add(new OtrosCostosXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_CodCos = (String)oChildCc.GetProperty("U_CodCos"),
                    U_Monto = (Double)oChildCc.GetProperty("U_Monto"),
                    U_Unidad = (String)oChildCc.GetProperty("U_Unidad")
                });

            }
            return otrosCostosXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de Trámites del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con los trámites del Contrato de Ventas</param>
    /// <returns>Lista con los trámites del Contrato de Ventas</returns>
    private static List<TramitesXContrato> Carga_TramitesXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<TramitesXContrato> tramitesXContratoList = default(List<TramitesXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            tramitesXContratoList = new List<TramitesXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                tramitesXContratoList.Add(new TramitesXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Cod_Tram = (String)oChildCc.GetProperty("U_Cod_Tram"),
                    U_Des_Tram = (String)oChildCc.GetProperty("U_Des_Tram"),
                    U_Cant = (Int16)oChildCc.GetProperty("U_Cant"),
                    U_Pre_Uni = (Double)oChildCc.GetProperty("U_Pre_Uni"),
                    U_Costo = (Double)oChildCc.GetProperty("U_Costo"),
                    U_Imp_Com = (String)oChildCc.GetProperty("U_Imp_Com"),
                    U_ProvTram = (String)oChildCc.GetProperty("U_ProvTram"),
                    U_Comprar = (String)oChildCc.GetProperty("U_Comprar"),
                    U_Ord_Comp = (String)oChildCc.GetProperty("U_Ord_Comp"),
                    U_Pre_Tot = (Double)oChildCc.GetProperty("U_Pre_Tot"),
                    U_SCGD_Fct = (String)oChildCc.GetProperty("U_SCGD_Fct"),
                    U_Imp_Vent = (String)oChildCc.GetProperty("U_Imp_Vent"),
                    U_CABYS_AE = (String)oChildCc.GetProperty("U_CABYS_AE"),
                    U_CABYS_TI = (String)oChildCc.GetProperty("U_CABYS_TI"),
                    U_CABYS_CH = (String)oChildCc.GetProperty("U_CABYS_CH")
                });

            }
            return tramitesXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de los Vehículos Usados del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con los vehículos usados del Contrato de Ventas</param>
    /// <returns>Lista con los vehículos usados del Contrato de Ventas</returns>
    private static List<UsadosXContrato> Carga_UsadosXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<UsadosXContrato> usadosXContratoList = default(List<UsadosXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            usadosXContratoList = new List<UsadosXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                usadosXContratoList.Add(new UsadosXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Cod_Unid = (String)oChildCc.GetProperty("U_Cod_Unid"),
                    U_Marca = (String)oChildCc.GetProperty("U_Marca"),
                    U_Estilo = (String)oChildCc.GetProperty("U_Estilo"),
                    U_Motor = (String)oChildCc.GetProperty("U_Motor"),
                    U_VIN = (String)oChildCc.GetProperty("U_VIN"),
                    U_Anio = (String)oChildCc.GetProperty("U_Anio"),
                    U_Placa = (String)oChildCc.GetProperty("U_Placa"),
                    U_Color = (String)oChildCc.GetProperty("U_Color"),
                    U_Tipo = (String)oChildCc.GetProperty("U_Tipo"),
                    U_RTV_MM = (Int32)oChildCc.GetProperty("U_RTV_MM"),
                    U_RTV_AA = (Int32)oChildCc.GetProperty("U_RTV_AA"),
                    U_Val_Rec = (Double)oChildCc.GetProperty("U_Val_Rec"),
                    U_Aj_Cos = (Double)oChildCc.GetProperty("U_Aj_Cos"),
                    U_Gravamen = (String)oChildCc.GetProperty("U_Gravamen"),
                    U_Fec_Av = (DateTime)oChildCc.GetProperty("U_Fec_Av"),
                    U_Der_Cir = (DateTime)oChildCc.GetProperty("U_Der_Cir"),
                    U_Gra_Fec = (DateTime)oChildCc.GetProperty("U_Gra_Fec"),
                    U_TraUs = (String)oChildCc.GetProperty("U_TraUs"),
                    U_MoUs = (String)oChildCc.GetProperty("U_MoUs"),
                    U_CoUs = (String)oChildCc.GetProperty("U_CoUs"),
                    U_Cod_Mod_Us = (String)oChildCc.GetProperty("U_Cod_Mod_Us"),
                    U_Cod_Col_Us = (String)oChildCc.GetProperty("U_Cod_Col_Us"),
                    U_Cod_Trans_Us = (String)oChildCc.GetProperty("U_Cod_Trans_Us"),
                    U_Cod_Comb_Us = (String)oChildCc.GetProperty("U_Cod_Comb_Us"),
                    U_Cod_Estilo_Us = (String)oChildCc.GetProperty("U_Cod_Estilo_Us"),
                    U_Cod_Marca_Us = (String)oChildCc.GetProperty("U_Cod_Marca_Us"),
                    U_Cod_Clasif_Us = (String)oChildCc.GetProperty("U_Cod_Clasif_Us"),
                    U_Des_Clasif_Us = (String)oChildCc.GetProperty("U_Des_Clasif_Us"),
                    U_Val_Venta = (Double)oChildCc.GetProperty("U_Val_Venta"),
                    U_CatUs = (String)oChildCc.GetProperty("U_CatUs"),
                    U_KmUs = (Double)oChildCc.GetProperty("U_KmUs"),
                    U_Cod_Prov = (String)oChildCc.GetProperty("U_Cod_Prov"),
                    U_Nom_Prov = (String)oChildCc.GetProperty("U_Nom_Prov"),
                    U_Existe = (String)oChildCc.GetProperty("U_Existe"),
                    U_N_FP = (String)oChildCc.GetProperty("U_N_FP"),
                    U_N_AsAd = (String)oChildCc.GetProperty("U_N_AsAd"),
                    U_CABYS_AE = (String)oChildCc.GetProperty("U_CABYS_AE"),
                    U_CABYS_TI = (String)oChildCc.GetProperty("U_CABYS_TI"),
                    U_CABYS_CH = (String)oChildCc.GetProperty("U_CABYS_CH")
                });

            }
            return usadosXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Función que retorna Lista de los Vehículos de Venta del Contrato de Ventas
    /// </summary>
    /// <param name="p_generalDataCollection">GeneralDataCollection con los vehículos de venta del Contrato de Ventas</param>
    /// <returns>Lista con los vehículos de venta del Contrato de Ventas</returns>
    private static List<VehiculosXContrato> Carga_VehiculosXContrato(SAPbobsCOM.GeneralDataCollection p_generalDataCollection)
    {
        List<VehiculosXContrato> vehiculosXContratoList = default(List<VehiculosXContrato>);
        SAPbobsCOM.GeneralData oChildCc = default(SAPbobsCOM.GeneralData);

        try
        {
            vehiculosXContratoList = new List<VehiculosXContrato>();
            for (int index = 0; index <= p_generalDataCollection.Count - 1; index++)
            {
                oChildCc = p_generalDataCollection.Item(index);
                vehiculosXContratoList.Add(new VehiculosXContrato
                {
                    DocEntry = (Int32)oChildCc.GetProperty("DocEntry"),
                    LineId = (Int32)oChildCc.GetProperty("LineId"),
                    VisOrder = (Int32)oChildCc.GetProperty("VisOrder"),
                    LogInst = (Int32)oChildCc.GetProperty("LogInst"),
                    U_Cod_Unid = (String)oChildCc.GetProperty("U_Cod_Unid"),
                    U_Des_Marc = (String)oChildCc.GetProperty("U_Des_Marc"),
                    U_Des_Mode = (String)oChildCc.GetProperty("U_Des_Mode"),
                    U_Des_Esti = (String)oChildCc.GetProperty("U_Des_Esti"),
                    U_Ano_Vehi = (Int32)oChildCc.GetProperty("U_Ano_Vehi"),
                    U_Num_Plac = (String)oChildCc.GetProperty("U_Num_Plac"),
                    U_Des_Col = (String)oChildCc.GetProperty("U_Des_Col"),
                    U_Num_VIN = (String)oChildCc.GetProperty("U_Num_VIN"),
                    U_Num_Mot = (String)oChildCc.GetProperty("U_Num_Mot"),
                    U_Transmi = (String)oChildCc.GetProperty("U_Transmi"),
                    U_Pre_Vta = (Double)oChildCc.GetProperty("U_Pre_Vta"),
                    U_Pagos = (Double)oChildCc.GetProperty("U_Pagos"),
                    U_Impuesto = (String)oChildCc.GetProperty("U_Impuesto"),
                    U_Desc_Veh = (Double)oChildCc.GetProperty("U_Desc_Veh"),
                    U_Pre_Tot = (Double)oChildCc.GetProperty("U_Pre_Tot"),
                    U_Mon_Acc = (Double)oChildCc.GetProperty("U_Mon_Acc"),
                    U_Gas_Loc = (Double)oChildCc.GetProperty("U_Gas_Loc"),
                    U_Otro_Gas = (Double)oChildCc.GetProperty("U_Otro_Gas"),
                    U_Bono = (Double)oChildCc.GetProperty("U_Bono"),
                    U_MDesc = (Double)oChildCc.GetProperty("U_MDesc"),
                    U_PreNet = (Double)oChildCc.GetProperty("U_PreNet"),
                    U_TipIn = (String)oChildCc.GetProperty("U_TipIn"),
                    U_ColIn = (String)oChildCc.GetProperty("U_ColIn"),
                    U_Obser = (String)oChildCc.GetProperty("U_Obser"),
                    U_Km_Venta = (Double)oChildCc.GetProperty("U_Km_Venta"),
                    U_CABYS_AE = (String)oChildCc.GetProperty("U_CABYS_AE"),
                    U_CABYS_TI = (String)oChildCc.GetProperty("U_CABYS_TI"),
                    U_CABYS_CH = (String)oChildCc.GetProperty("U_CABYS_CH")
                });

            }
            return vehiculosXContratoList;
        }
        catch (Exception)
        {
            return null;
        }
    }
}