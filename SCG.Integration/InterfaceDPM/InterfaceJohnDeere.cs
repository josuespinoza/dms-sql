using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Globalization;
using DMS_Connector.Business_Logic.DataContract.Articulo;
using DMS_Connector.Business_Logic.DataContract.SAPDocumento;
using SAPbobsCOM;
using SCG.Integration.InterfaceDPM.Entities;
using SCG.SBOFramework;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Integration.InterfaceDPM
{
    public class InterfaceJohnDeere 
    {
        public IApplication oApplicationSBO { get; private set; }
        public ICompany oCompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;

        public SAPbouiCOM.Form oForm { get; set; }

        private static NumberFormatInfo n;
        private List<String> monthHistoricList;
        #region Constructor

        public enum MonthAgo
        {
            ActualMonth, 
            MonthAgo1, 
            MonthAgo2,
            MonthAgo3,
            MonthAgo4,
            MonthAgo5,
            MonthAgo6,
            MonthAgo7,
            MonthAgo8,
            MonthAgo9,
            MonthAgo10,
            MonthAgo11,
            MonthAgo12,
            MonthAgo13,
            MonthAgo14,
            MonthAgo15,
            MonthAgo16,
            MonthAgo17,
            MonthAgo18,
            MonthAgo19,
            MonthAgo20,
            MonthAgo21,
            MonthAgo22,
            MonthAgo23,
            MonthAgo24,
            MonthAgo25,
            MonthAgo26,
            MonthAgo27,
            MonthAgo28,
            MonthAgo29,
            MonthAgo30,
            MonthAgo31,
            MonthAgo32,
            MonthAgo33,
            MonthAgo34,
            MonthAgo35,
            MonthAgo36,
            MonthAgo37,
            MonthAgo38,
            MonthAgo39,
            MonthAgo40,
            MonthAgo41,
            MonthAgo42,
            MonthAgo43,
            MonthAgo44,
            MonthAgo45,
            MonthAgo46,
            MonthAgo47,
            MonthAgo48
        }

        public enum TotalSalesHits
        {
            Total_1To12,
            Total_13To24,
            Total_25To36,
            Total_37To48
        }
        #endregion
        #region Constructor
        public InterfaceJohnDeere(IApplication applicationSBO, ICompany companySBO, SAPbouiCOM.Form p_oForm)
        {
            try
            {
                oApplicationSBO = applicationSBO;
                oCompanySBO = companySBO;
                SBOCompany = (SAPbobsCOM.Company)companySBO;
                oForm = p_oForm;

                n = DIHelper.GetNumberFormatInfo(companySBO);
            }
            catch (Exception ex)
            {
                throw;
                //Utilitarios.ManejadorErrores(ex, (SAPbouiCOM.Application)ApplicationSBO);
            }
        }
        #endregion

        #region Metodos
        public void ManejaInterfaceJohnDeere_JDPRISM(ref DataTable p_dtMatrix, ref String p_strLoadType )
        {
            var sb= new StringBuilder();
            JDPRISM oJDPRISM;
            InfoWarehouse oWarehouse;
            List<InfoWarehouse> listWarehouse;
            try
            {
                oJDPRISM = new JDPRISM();
                LoadGeneralConfiguration(ref oJDPRISM, ref p_strLoadType);
                //****** Crear Encabezado ******* 
                CreateFileHeaderJDPRISM(ref sb, ref oJDPRISM);
                //****** Crear Encabezado Almacen ******* 
                CreateWarehousetHeaderJDPRISM(ref sb);
                //****** Crear Detail Record ******* 
                CreateDetailJDPRISM(ref sb, ref oJDPRISM, ref p_dtMatrix);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        //*************** Crear Encabezado ***************** 
        public void CreateFileHeaderJDPRISM(ref StringBuilder p_sb, ref JDPRISM p_jdprism)
        {
            HeaderJDPRISM oHeader;
            HeaderJDPRISM oHeaderInformation;
            try
            {
                oHeaderInformation = new HeaderJDPRISM();
                //*************** Crear Encabezado ***************** 
                oHeader = new HeaderJDPRISM();
                LoadConfigurationJohnDeereHeader(ref oHeaderInformation);

                oHeader.HeaderRecordCode = p_jdprism.MainAccount;
                oHeader.DateOfExtract = DateTime.Today;
                oHeader.TimeOfExtract = DateTime.Parse(DateTime.Now.ToString("hh:mm"));
                p_jdprism.FileDate = DateTime.Now;
                if (p_jdprism.LoadType == "I")
                {
                    oHeader.TypeOfExtract = "I";
                }
                else if (p_jdprism.LoadType == "D")
                {
                    oHeader.TypeOfExtract = "D";
                }
                oHeader.InterfaceVersion = oHeaderInformation.InterfaceVersion;
                oHeader.DBSName = oHeaderInformation.DBSName ;
                oHeader.DBSVersion = oHeaderInformation.DBSVersion;
                oHeader.OrdenCoordinationData = oHeaderInformation.OrdenCoordinationData;
                oHeader.TransferCoordinationData = oHeaderInformation.TransferCoordinationData;
                oHeader.OrderAndTransferFilesProcessed = oHeaderInformation.OrderAndTransferFilesProcessed;
                oHeader.ToString(ref p_sb);
                
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        //*************** Crear Encabezado Almacen ***************** 
        public void CreateWarehousetHeaderJDPRISM(ref StringBuilder p_sb)
        {
            WarehouseJDPRISM oHeaderWarehouse;
            WarehouseJDPRISM oHeaderWarehouseInformation;
            try
            {
                oHeaderWarehouseInformation = new WarehouseJDPRISM();
                //*************** Crear Encabezado ***************** 
                oHeaderWarehouse = new WarehouseJDPRISM();
                LoadConfigurationJohnDeereWarehouseHeader(ref oHeaderWarehouseInformation);

                oHeaderWarehouse.WHHeaderRecordCode = "~H~";
                oHeaderWarehouse.DealerAccount = oHeaderWarehouseInformation.DealerAccount.Trim('~');
                oHeaderWarehouse.DBSWarehouse = oHeaderWarehouseInformation.DBSWarehouse;
                oHeaderWarehouse.FiscalMonth = DateTime.Today.Month;
                //oHeaderWarehouse.NextPartsMonthEndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
                oHeaderWarehouse.WarehouseType = 1;
                oHeaderWarehouse.WhereDataIsToBeLoaded = oHeaderWarehouseInformation.WhereDataIsToBeLoaded;

                oHeaderWarehouse.ToString(ref p_sb);

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        //**************Detail ***************** 
        public void CreateDetailJDPRISM(ref StringBuilder p_sb, ref JDPRISM p_jdprism,  ref DataTable p_dtMatrix)
        {
            List <DetailJDPRISM> oRecordList;
            List<oArticulo> oItems;//(A)
            DetailJDPRISM oRecord;
            List<String> itemList;
            oArticulo oItem;
            InfoWarehouse infoWarehouse;//(B)
            InfoWarehouse infoWarehouseProcess;//(C)
            oLineasDocumento rowSalesOrder;//(D)
            oLineasDocumento rowInvoice;//(E)
            oLineasDocumento rowStockTransaction;//(F)
            int intContador = 0;
            DataTable dtDataRecord;
            bool blnCargaInicial = true;
            monthHistoricList = new List<string>();
            int intNumberOfMonthHistory = 0;
            try
            {
                oRecordList = new List <DetailJDPRISM>();
                oItems = new List<oArticulo>();
                //*** Carga Articulos (A)***
                if (p_jdprism.LoadType == "I")
                {
                    LoadItemInitialization(ref oItems, ref p_jdprism);
                }
                else if (p_jdprism.LoadType == "D")
                {
                    LoadItemDelta(ref oItems, ref p_jdprism);
                }
                //*** Crear Detalle *** 
                foreach (oArticulo rowItem  in oItems)
                {
                    intContador += 1;
                    if (!blnCargaInicial)
                    {
                        oApplicationSBO.StatusBar.SetText("Processing item      " + intContador.ToString() + "      to     " + oItems.Count.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    }

                    oItem = rowItem;
                    infoWarehouse = new InfoWarehouse();
                    infoWarehouseProcess = new InfoWarehouse();
                    rowSalesOrder = new oLineasDocumento();

                    //***(B)***
                    LoadInfoWarehouse(ref oItem, ref p_jdprism, ref infoWarehouse);
                    //***(C)***
                    LoadInfoWarehouse(ref oItem, ref p_jdprism, ref infoWarehouseProcess, true);
                    //***(D)***
                    LoadInfoSalesOrder(ref oItem, ref p_jdprism, ref rowSalesOrder);
                    //***(E)***
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice,  MonthAgo.ActualMonth);
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo1);
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo3);
                    //**(F)***
                    //LoadInfoStockTransaction(ref oItem, ref p_jdprism, ref rowInvoice);

                    //*** Detail Record ***
                    oRecord = new DetailJDPRISM();
                    oRecord.RecordCode = "~P~";//1
                    //***(A)***
                    oRecord.PartNumber = !string.IsNullOrEmpty(oItem.ItemCode) ? oItem.ItemCode.Trim() : string.Empty;//2
                    //***(B)***
                    oRecord.AvailableQuantity = infoWarehouse.Available;//3
                    oRecord.OOQuantity = infoWarehouse.OnOrder ;//4
                    //***(C)***
                    oRecord.ReserveQ_WO = infoWarehouseProcess.Available;//5
                    //***(D)***
                    oRecord.ReserveQ_PT = rowSalesOrder.ReserveQ_PT;//6
                    //***(E)***
                    rowInvoice = new oLineasDocumento();
                    LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.ActualMonth);
                    oRecord.CurrrentMTDSales = rowInvoice.Sales_Month;//7
                    oRecord.CurrentMTDHits = rowInvoice.Hits_Month;//8
                    oRecord.CurrentMTDLostSales = rowInvoice.LostSales_Month;//9
                    oRecord.CurrentMTDLostHits = rowInvoice.LostHits_Month;//10
                    //***(A)***
                    oRecord.DealerPPP = oItem.NumInSale;//11
                    //*****
                    oRecord.BinLocation = string.Empty;//12
                    oRecord.AlternateBinLocation = string.Empty;//13
                    oRecord.VendorPartCost = -1;//14
                    oRecord.VendorPackageQuantity = -1;//15
                    oRecord.VendorCode = string.Empty;//16
                    oRecord.VendorSubstitutionInfo = string.Empty;//17
                    oRecord.PricingBase = "C";//18
                    //***(I)***
                    oRecord.PricingAdditive = -1;//19
                    oRecord.DealerPrice = -1;//20   
                    //*****
                    oRecord.OrderFormulaCode = string.Empty;//21
                    oRecord.DeleteIndicator = string.Empty;//22
                    //***(D)***
                    oRecord.ReservedHits_WO = rowSalesOrder.ReservedHits_WO;//23
                    oRecord.ReservedHits_PT = rowSalesOrder.ReservedHits_PT;//24
                    //***(A)***
                    oRecord.AverageCost = oItem.AvgPrice;//25

                    //******************************************************
                    // valid according to load type
                    //******************************************************
                    //if (p_jdprism.LoadType == "D")
                    //{
                    //    oRecord.ToString(ref p_sb, ref p_jdprism );
                    //    DataLinkToMatrix(ref oRecord, ref p_dtMatrix, ref p_jdprism);
                    //    continue;
                    //}
                    //***(%%%)***
                    oRecord.Start_I_Records = "%%%";//26
                    //*****
                    oRecord.PartDescription = string.Empty;//27
                    oRecord.DealerPartNote = string.Empty;//28
                    oRecord.OrderIndicator = string.Empty;//29
                    //***(F)***
                    rowStockTransaction = new oLineasDocumento();
                    LoadInfoStockTransaction(ref oItem, ref p_jdprism, ref rowStockTransaction);
                    oRecord.DateAdded = rowStockTransaction.DateAdded;//30
                    //*****
                    oRecord.DealerGroupCode = string.Empty;//31
                    //***(A)***
                    oRecord.MinOrderQuantity = rowItem.MinInventory;//32
                    oRecord.MaxOrderQuantity = rowItem.MaxInventory;//33
                    //***(Months)***
                    if (rowStockTransaction.DateAdded == new DateTime(1900, 01, 01))
                    {
                        intNumberOfMonthHistory = 36;
                    }
                    else
                    {
                        intNumberOfMonthHistory = Convert.ToInt32(MonthDifference(DateTime.Today, rowStockTransaction.DateAdded));
                    }
                    if (intNumberOfMonthHistory >= 36)
                    {
                        oRecord.NumberOfMonthlyHistory = 36;//34
                    }
                    else
                    {
                        oRecord.NumberOfMonthlyHistory = intNumberOfMonthHistory;//34
                    }
                    //*****
                    oRecord.PiecesInSet = -1;//35
                    //***(G)***Month 1 ***
                    if (intNumberOfMonthHistory >= 1)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo1);
                        oRecord.Sales_Month_1 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_1 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_1 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_1 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_1 = 0;
                        oRecord.Hits_Month_1 = 0;
                        oRecord.LostSales_Month_1 = 0;
                        oRecord.LostHits_Month_1 = 0;
                    }
                    //***(G)***Month 2 ***
                    if (intNumberOfMonthHistory >= 2)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo2);
                        oRecord.Sales_Month_2 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_2 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_2 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_2 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_2 = 0;
                        oRecord.Hits_Month_2 = 0;
                        oRecord.LostSales_Month_2 = 0;
                        oRecord.LostHits_Month_2 = 0;
                    }

                    //***(G)***Month 3 ***
                    if (intNumberOfMonthHistory >= 3)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo3);
                        oRecord.Sales_Month_3 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_3 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_3 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_3 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_3 = 0;
                        oRecord.Hits_Month_3 = 0;
                        oRecord.LostSales_Month_3 =0;
                        oRecord.LostHits_Month_3 = 0;
                    }

                    //***(G)***Month 4 ***
                    if (intNumberOfMonthHistory >= 4)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo4);
                        oRecord.Sales_Month_4 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_4 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_4 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_4 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_4 = 0;
                        oRecord.Hits_Month_4 = 0;
                        oRecord.LostSales_Month_4 = 0;
                        oRecord.LostHits_Month_4 = 0;
                    }

                    //***(G)***Month 5 ***
                    if (intNumberOfMonthHistory >= 5)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo5);
                        oRecord.Sales_Month_5 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_5 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_5 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_5 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_5 = 0;
                        oRecord.Hits_Month_5 = 0;
                        oRecord.LostSales_Month_5 = 0;
                        oRecord.LostHits_Month_5 = 0;
                    }

                    //***(G)***Month 6 ***
                    if (intNumberOfMonthHistory >= 6)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo6);
                        oRecord.Sales_Month_6 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_6 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_6 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_6 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_6 = 0;
                        oRecord.Hits_Month_6 = 0;
                        oRecord.LostSales_Month_6 = 0;
                        oRecord.LostHits_Month_6 = 0;
                    }

                    //***(G)***Month 7 ***
                    if (intNumberOfMonthHistory >= 7)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo7);
                        oRecord.Sales_Month_7 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_7 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_7 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_7 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_7 = 0;
                        oRecord.Hits_Month_7 = 0;
                        oRecord.LostSales_Month_7 = 0;
                        oRecord.LostHits_Month_7 = 0;
                    }

                    //***(G)***Month 8 ***
                    if (intNumberOfMonthHistory >= 8)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo8);
                        oRecord.Sales_Month_8 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_8 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_8 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_8 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_8 = 0;
                        oRecord.Hits_Month_8 = 0;
                        oRecord.LostSales_Month_8 = 0;
                        oRecord.LostHits_Month_8 = 0;
                    }

                    //***(G)***Month 9 ***
                    if (intNumberOfMonthHistory >= 9)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo9);
                        oRecord.Sales_Month_9 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_9 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_9 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_9 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_9 = 0;
                        oRecord.Hits_Month_9 =0;
                        oRecord.LostSales_Month_9 = 0;
                        oRecord.LostHits_Month_9 = 0;
                    }

                    //***(G)***Month 10 ***
                    if (intNumberOfMonthHistory >= 10)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo10);
                        oRecord.Sales_Month_10 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_10 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_10 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_10 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_10 = 0;
                        oRecord.Hits_Month_10 = 0;
                        oRecord.LostSales_Month_10 = 0;
                        oRecord.LostHits_Month_10 = 0;
                    }
                    //***(G)***Month 11 ***
                    if (intNumberOfMonthHistory >= 11)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo11);
                        oRecord.Sales_Month_11 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_11 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_11 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_11 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_11 = 0;
                        oRecord.Hits_Month_11 = 0;
                        oRecord.LostSales_Month_11 = 0;
                        oRecord.LostHits_Month_11 = 0;
                    }

                    //***(G)***Month 12 ***
                    if (intNumberOfMonthHistory >= 12)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo12);
                        oRecord.Sales_Month_12 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_12 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_12 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_12 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_12 = 0;
                        oRecord.Hits_Month_12 = 0;
                        oRecord.LostSales_Month_12 = 0;
                        oRecord.LostHits_Month_12 = 0;
                    }

                    //***(G)***Month 13 ***
                    if (intNumberOfMonthHistory >= 13)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo13);
                        oRecord.Sales_Month_13 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_13 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_13 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_13 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_13 =0;
                        oRecord.Hits_Month_13 = 0;
                        oRecord.LostSales_Month_13 =0;
                        oRecord.LostHits_Month_13 = 0;
                    }

                    //***(G)***Month 14 ***
                    if (intNumberOfMonthHistory >= 14)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo14);
                        oRecord.Sales_Month_14 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_14 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_14 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_14 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_14 =0;
                        oRecord.Hits_Month_14 = 0;
                        oRecord.LostSales_Month_14 = 0;
                        oRecord.LostHits_Month_14 = 0;
                    }

                    //***(G)***Month 15 ***
                    if (intNumberOfMonthHistory >= 15)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo15);
                        oRecord.Sales_Month_15 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_15 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_15 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_15 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_15 = 0;
                        oRecord.Hits_Month_15 = 0;
                        oRecord.LostSales_Month_15 = 0;
                        oRecord.LostHits_Month_15 = 0;
                    }

                    //***(G)***Month 16 ***
                    if (intNumberOfMonthHistory >= 16)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo16);
                        oRecord.Sales_Month_16 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_16 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_16 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_16 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_16 =0;
                        oRecord.Hits_Month_16 = 0;
                        oRecord.LostSales_Month_16 = 0;
                        oRecord.LostHits_Month_16 = 0;
                    }

                    //***(G)***Month 17 ***
                    if (intNumberOfMonthHistory >= 17)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo17);
                        oRecord.Sales_Month_17 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_17 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_17 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_17 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_17 = 0;
                        oRecord.Hits_Month_17 = 0;
                        oRecord.LostSales_Month_17 = 0;
                        oRecord.LostHits_Month_17 = 0;
                    }

                    //***(G)***Month 18 ***
                    if (intNumberOfMonthHistory >= 18)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo18);
                        oRecord.Sales_Month_18 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_18 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_18 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_18 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_18 = 0;
                        oRecord.Hits_Month_18 =0;
                        oRecord.LostSales_Month_18 = 0;
                        oRecord.LostHits_Month_18 = 0;
                    }

                    //***(G)***Month 19 ***
                    if (intNumberOfMonthHistory >= 19)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo19);
                        oRecord.Sales_Month_19 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_19 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_19 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_19 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_19 = 0;
                        oRecord.Hits_Month_19 = 0;
                        oRecord.LostSales_Month_19 = 0;
                        oRecord.LostHits_Month_19 = 0;
                    }

                    //***(G)***Month 20 ***
                    if (intNumberOfMonthHistory >= 20)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo20);
                        oRecord.Sales_Month_20 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_20 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_20 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_20 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_20 =0;
                        oRecord.Hits_Month_20 = 0;
                        oRecord.LostSales_Month_20 =0;
                        oRecord.LostHits_Month_20 = 0;
                    }

                    //***(G)***Month 21 ***
                    if (intNumberOfMonthHistory >= 21)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo21);
                        oRecord.Sales_Month_21 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_21 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_21 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_21 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_21 = 0;
                        oRecord.Hits_Month_21 = 0;
                        oRecord.LostSales_Month_21 =0;
                        oRecord.LostHits_Month_21 = 0;
                    }

                    //***(G)***Month 22 ***
                    if (intNumberOfMonthHistory >= 22)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo22);
                        oRecord.Sales_Month_22 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_22 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_22 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_22 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_22 =0;
                        oRecord.Hits_Month_22 = 0;
                        oRecord.LostSales_Month_22 = 0;
                        oRecord.LostHits_Month_22 = 0;
                    }

                    //***(G)***Month 23 ***
                    if (intNumberOfMonthHistory >= 23)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo23);
                        oRecord.Sales_Month_23 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_23 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_23 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_23 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_23 = 0;
                        oRecord.Hits_Month_23 = 0;
                        oRecord.LostSales_Month_23 = 0;
                        oRecord.LostHits_Month_23 = 0;
                    }

                    //***(G)***Month 24 ***
                    if (intNumberOfMonthHistory >= 24)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo24);
                        oRecord.Sales_Month_24 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_24 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_24 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_24 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_24 = 0;
                        oRecord.Hits_Month_24 = 0;
                        oRecord.LostSales_Month_24 = 0;
                        oRecord.LostHits_Month_24 = 0;
                    }

                    //***(G)***Month 25 ***
                    if (intNumberOfMonthHistory >= 25)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo25);
                        oRecord.Sales_Month_25 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_25 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_25 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_25 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_25 = 0;
                        oRecord.Hits_Month_25 = 0;
                        oRecord.LostSales_Month_25 = 0;
                        oRecord.LostHits_Month_25 =0;
                    }

                    //***(G)***Month 26 ***
                    if (intNumberOfMonthHistory >= 26)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo26);
                        oRecord.Sales_Month_26 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_26 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_26 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_26 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_26 = 0;
                        oRecord.Hits_Month_26 = 0;
                        oRecord.LostSales_Month_26 =0;
                        oRecord.LostHits_Month_26 = 0;
                    }

                    //***(G)***Month 27 ***
                    if (intNumberOfMonthHistory >= 27)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo27);
                        oRecord.Sales_Month_27 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_27 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_27 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_27 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_27 = 0;
                        oRecord.Hits_Month_27 = 0;
                        oRecord.LostSales_Month_27 =0;
                        oRecord.LostHits_Month_27 = 0;
                    }

                    //***(G)***Month 28 ***
                    if (intNumberOfMonthHistory >= 28)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo28);
                        oRecord.Sales_Month_28 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_28 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_28 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_28 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_28 = 0;
                        oRecord.Hits_Month_28 = 0;
                        oRecord.LostSales_Month_28 = 0;
                        oRecord.LostHits_Month_28 = 0;
                    }

                    //***(G)***Month 29 ***
                    if (intNumberOfMonthHistory >= 29)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo29);
                        oRecord.Sales_Month_29 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_29 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_29 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_29 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_29 = 0;
                        oRecord.Hits_Month_29 = 0;
                        oRecord.LostSales_Month_29 =0;
                        oRecord.LostHits_Month_29 = 0;
                    }

                    //***(G)***Month 30 ***
                    if (intNumberOfMonthHistory >= 30)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo30);
                        oRecord.Sales_Month_30 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_30 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_30 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_30 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_30 = 0;
                        oRecord.Hits_Month_30 = 0;
                        oRecord.LostSales_Month_30 = 0;
                        oRecord.LostHits_Month_30 = 0;
                    }

                    //***(G)***Month 31 ***
                    if (intNumberOfMonthHistory >= 31)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo31);
                        oRecord.Sales_Month_31 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_31 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_31 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_31 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_31 = 0;
                        oRecord.Hits_Month_31 = 0;
                        oRecord.LostSales_Month_31 = 0;
                        oRecord.LostHits_Month_31 = 0;
                    }

                    //***(G)***Month 32 ***
                    if (intNumberOfMonthHistory >= 32)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo32);
                        oRecord.Sales_Month_32 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_32 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_32 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_32 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_32 =0;
                        oRecord.Hits_Month_32 = 0;
                        oRecord.LostSales_Month_32 = 0;
                        oRecord.LostHits_Month_32 = 0;
                    }

                    //***(G)***Month 33 ***
                    if (intNumberOfMonthHistory >= 33)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo33);
                        oRecord.Sales_Month_33 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_33 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_33 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_33 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_33 =0;
                        oRecord.Hits_Month_33 = 0;
                        oRecord.LostSales_Month_33 = 0;
                        oRecord.LostHits_Month_33 =0;
                    }

                    //***(G)***Month 34 ***
                    if (intNumberOfMonthHistory >= 34)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo34);
                        oRecord.Sales_Month_34 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_34 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_34 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_34 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_34 = 0;
                        oRecord.Hits_Month_34 = 0;
                        oRecord.LostSales_Month_34 = 0;
                        oRecord.LostHits_Month_34 = 0;
                    }

                    //***(G)***Month 35 ***
                    if (intNumberOfMonthHistory >= 35)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo35);
                        oRecord.Sales_Month_35 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_35 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_35 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_35 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_35 = 0;
                        oRecord.Hits_Month_35 =0;
                        oRecord.LostSales_Month_35 = 0;
                        oRecord.LostHits_Month_35 = 0;
                    }

                    //***(G)***Month 36 ***
                    if (intNumberOfMonthHistory >= 36)
                    {
                        rowInvoice = new oLineasDocumento();
                        LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo36);
                        oRecord.Sales_Month_36 = rowInvoice.Sales_Month;
                        oRecord.Hits_Month_36 = rowInvoice.Hits_Month;
                        oRecord.LostSales_Month_36 = rowInvoice.LostSales_Month;
                        oRecord.LostHits_Month_36 = rowInvoice.LostHits_Month;
                    }
                    else
                    {
                        oRecord.Sales_Month_36 = 0;
                        oRecord.Hits_Month_36 = 0;
                        oRecord.LostSales_Month_36 = 0;
                        oRecord.LostHits_Month_36 = 0;
                    }
                    ////***(G)***Month 37 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo37);
                    //oRecord.Sales_Month_37 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_37 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_37 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_37 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 38 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo38);
                    //oRecord.Sales_Month_38 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_38 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_38 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_38 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 39 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo39);
                    //oRecord.Sales_Month_39 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_39 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_39 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_39 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 40 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo40);
                    //oRecord.Sales_Month_40 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_40 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_40= rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_40 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 41 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo41);
                    //oRecord.Sales_Month_41 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_41 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_41 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_41 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 42 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo42);
                    //oRecord.Sales_Month_42 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_42 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_42 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_42 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 43 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo43);
                    //oRecord.Sales_Month_43 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_43 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_43 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_43 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 44 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo44);
                    //oRecord.Sales_Month_44 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_44 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_44 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_44 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 45 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo45);
                    //oRecord.Sales_Month_45 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_45 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_45 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_45 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 46 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo46);
                    //oRecord.Sales_Month_46 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_46 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_46 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_46 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 47 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo47);
                    //oRecord.Sales_Month_47 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_47 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_47 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_47 = rowInvoice.LostHits_Month;
                    ////***(G)***Month 48 ***
                    //rowInvoice = new oLineasDocumento();
                    //LoadInfoInvoice(ref oItem, ref p_jdprism, ref rowInvoice, MonthAgo.MonthAgo48);
                    //oRecord.Sales_Month_48 = rowInvoice.Sales_Month;
                    //oRecord.Hits_Month_48 = rowInvoice.Hits_Month;
                    //oRecord.LostSales_Month_48 = rowInvoice.LostSales_Month;
                    //oRecord.LostHits_Month_48 = rowInvoice.LostHits_Month;
                    //***(H)***Month 1 To 12 ***
                    //TotalSalesAndHits(ref oRecord,TotalSalesHits.Total_1To12);
                    ////***(H)***Month 13 To 24 ***
                    //TotalSalesAndHits(ref oRecord, TotalSalesHits.Total_13To24);
                    ////***(H)***Month 25 To 36 ***
                    //TotalSalesAndHits(ref oRecord, TotalSalesHits.Total_25To36);
                    ////***(H)***Month 37 To 48 ***
                    //TotalSalesAndHits(ref oRecord, TotalSalesHits.Total_37To48);
                    //****** Field #34
                    //if ( oRecord.DateAdded == default(DateTime))
                    //{
                    //    oRecord.DateAdded = new DateTime(1900,01,01);
                    //    oRecord.NumberOfMonthlyHistory = 36;
                    //}
                    //else{
                    //    oRecord.NumberOfMonthlyHistory = monthHistoricList.Count();//34
                    //}
                    //monthHistoricList.Clear();

                    oRecord.ToString(ref p_sb, ref p_jdprism);
                    if (!blnCargaInicial)
                    {
                        DataLinkToMatrix(ref oRecord, ref p_dtMatrix, ref p_jdprism);
                    }
                }

                SaveFile(ref p_jdprism, ref p_sb);
                oApplicationSBO.StatusBar.SetText("Process Complete", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        public void SaveFile(ref JDPRISM p_jdprism, ref StringBuilder p_sb)
        {
            String strFileName = string.Empty;
            String strFileType = string.Empty;
            try
            {
                if (p_jdprism.LoadType == "I")
                {
                    strFileType = "I";
                }
                else if (p_jdprism.LoadType == "D")
                {
                    strFileType = "D";
                }
                if (!string.IsNullOrEmpty(p_jdprism.Path))
                {
                    //strFileName = "DLR2JD_DPMEXT_"+strFileType+"_" + p_jdprism.MainAccount.Trim('~') + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHMMss") + ".DPMBRA";
                    strFileName = "DLR2JD_DPMEXT_" + strFileType + "_" + p_jdprism.MainAccount.Trim('~') + "_" + p_jdprism.FileDate.ToString("yyyyMMdd") + "_" + p_jdprism.FileDate.ToString("HHMMss") + ".DPMBRA";
                    p_jdprism.Path += strFileName;
                    System.IO.File.WriteAllText(@p_jdprism.Path, p_sb.ToString());
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadItemInitialization(ref List<oArticulo> p_oItems, ref JDPRISM p_jdprism)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsItem;
            oArticulo oItem;
            try
            {
                if (oForm != null)
                {
                    oForm.DataSources.DBDataSources.Add("OITM");
                    dsItem = oForm.DataSources.DBDataSources.Item("OITM");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "FirmCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_jdprism.FirmCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "validFor";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "Y";
                    oCondition.BracketCloseNum = 1;

                    //oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    //oCondition = oConditions.Add();
                    //oCondition.BracketOpenNum = 1;
                    //oCondition.Alias = "U_SCGD_CargaI";
                    //oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    //oCondition.CondVal = "P";
                    //oCondition.BracketCloseNum = 1;

                    dsItem.Query(oConditions);
                    oApplicationSBO.StatusBar.SetText("Load Items    " + dsItem.Size.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    for (int index = 0; index < dsItem.Size; index++)
                    {
                        try
                        {
                            oItem = new oArticulo();
                            //oApplicationSBO.StatusBar.SetText("Load Item    " + index.ToString() + "      to    " + dsItem.Size.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            oItem.ItemCode = !string.IsNullOrEmpty(dsItem.GetValue("ItemCode", index)) ? dsItem.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                            oItem.ItemName = !string.IsNullOrEmpty(dsItem.GetValue("ItemName", index)) ? dsItem.GetValue("ItemName", index).ToString().Trim() : string.Empty;
                            if (!string.IsNullOrEmpty(dsItem.GetValue("NumInSale", index)))
                            {
                                oItem.NumInSale = double.Parse(dsItem.GetValue("NumInSale", index));
                            }
                            if (!string.IsNullOrEmpty(dsItem.GetValue("AvgPrice", index)))
                            {
                                oItem.AvgPrice = double.Parse(dsItem.GetValue("AvgPrice", index));
                            }
                            if (!string.IsNullOrEmpty(dsItem.GetValue("MinLevel", index)))
                            {
                                oItem.MinInventory = double.Parse(dsItem.GetValue("MinLevel", index));
                            }
                            if (!string.IsNullOrEmpty(dsItem.GetValue("MaxLevel", index)))
                            {
                                oItem.MaxInventory = double.Parse(dsItem.GetValue("MaxLevel", index));
                            }
                            p_oItems.Add(oItem);
                        }
                        catch (Exception e)
                        {
                            DMS_Connector.Helpers.ManejoErrores(e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadItemDelta(ref List<oArticulo> p_oItems, ref JDPRISM p_jdprism)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            oArticulo oItem;
            DateTime fromDate;
            DateTime toDate;
            List<String> itemListTemp;
            string strItemCode = string.Empty;
            int intContador = 0;
            try
            {
                if (oForm != null)
                {
                    itemListTemp = new List<string>();

                    //fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    fromDate = DateTime.Now.AddDays(-1);
                    toDate = DateTime.Today;

                    //********** Load Invoices ******
                    oForm.DataSources.DBDataSources.Add("INV1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("INV1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    //oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    //oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    //oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strItemCode) & !itemListTemp.Contains(strItemCode))
                        {
                            itemListTemp.Add(strItemCode );
                        }
                    }

                    //********** Load Saler Orders ******
                    oForm.DataSources.DBDataSources.Add("RDR1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("RDR1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strItemCode) & !itemListTemp.Contains(strItemCode))
                        {
                            itemListTemp.Add(strItemCode);
                        }
                    }

                    //********** Load Quotation ******
                    oForm.DataSources.DBDataSources.Add("QUT1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("QUT1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strItemCode) & !itemListTemp.Contains(strItemCode))
                        {
                            itemListTemp.Add(strItemCode);
                        }
                    }

                    //********** Load Good Receive ******
                    oForm.DataSources.DBDataSources.Add("IGN1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("IGN1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strItemCode) & !itemListTemp.Contains(strItemCode))
                        {
                            itemListTemp.Add(strItemCode);
                        }
                    }

                    //********** Load Tranfers ******
                    oForm.DataSources.DBDataSources.Add("WTR1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("WTR1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strItemCode) & !itemListTemp.Contains(strItemCode))
                        {
                            itemListTemp.Add(strItemCode);
                        }
                    }

                    //****** Load info Item
                    foreach (var rowItem in itemListTemp)
                    {
                        intContador += 1;
                        oApplicationSBO.StatusBar.SetText("Load Item  " + intContador.ToString() + "  to  " + itemListTemp.Count.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        oForm.DataSources.DBDataSources.Add("OITM");
                        dsInformation = oForm.DataSources.DBDataSources.Item("OITM");

                        oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                        oCondition = oConditions.Add();
                        oCondition.BracketOpenNum = 1;
                        oCondition.Alias = "ItemCode";
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                        oCondition.CondVal = rowItem;
                        oCondition.BracketCloseNum = 1;
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                        oCondition = oConditions.Add();
                        oCondition.BracketOpenNum = 1;
                        oCondition.Alias = "FirmCode";
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                        oCondition.CondVal = p_jdprism.FirmCode;
                        oCondition.BracketCloseNum = 1;
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                        oCondition = oConditions.Add();
                        oCondition.BracketOpenNum = 1;
                        oCondition.Alias = "validFor";
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                        oCondition.CondVal = "Y";
                        oCondition.BracketCloseNum = 1;

                        dsInformation.Query(oConditions);
                        for (int index = 0; index < dsInformation.Size; index++)
                        {
                            oItem = new oArticulo();
                            oItem.ItemCode = !string.IsNullOrEmpty(dsInformation.GetValue("ItemCode", index)) ? dsInformation.GetValue("ItemCode", index).ToString().Trim() : string.Empty;
                            oItem.ItemName = !string.IsNullOrEmpty(dsInformation.GetValue("ItemName", index)) ? dsInformation.GetValue("ItemName", index).ToString().Trim() : string.Empty;
                            oItem.NumInSale = double.Parse(dsInformation.GetValue("NumInSale", index));
                            oItem.AvgPrice = double.Parse(dsInformation.GetValue("AvgPrice", index));
                            oItem.MinInventory = double.Parse(dsInformation.GetValue("MinLevel", index));
                            oItem.MaxInventory = double.Parse(dsInformation.GetValue("MaxLevel", index));
                            p_oItems.Add(oItem);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadConfigurationJohnDeereHeader(ref HeaderJDPRISM p_headerJDPRISM)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                    oForm.DataSources.DBDataSources.Add("@SCGD_JD");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        p_headerJDPRISM.DBSName = !string.IsNullOrEmpty(dsInformation.GetValue("U_DBSName", index)) ? dsInformation.GetValue("U_DBSName", index).ToString().Trim() : string.Empty;
                        p_headerJDPRISM.InterfaceVersion = !string.IsNullOrEmpty(dsInformation.GetValue("U_Version", index)) ? dsInformation.GetValue("U_Version", index).ToString().Trim() : string.Empty;
                        p_headerJDPRISM.DBSVersion = !string.IsNullOrEmpty(dsInformation.GetValue("U_DBSVer", index)) ? dsInformation.GetValue("U_DBSVer", index).ToString().Trim() : string.Empty;
                        p_headerJDPRISM.OrdenCoordinationData = Convert.ToInt32(dsInformation.GetValue("U_OrdenC", index));
                        p_headerJDPRISM.TransferCoordinationData = Convert.ToInt32(dsInformation.GetValue("U_TranC", index));
                        p_headerJDPRISM.OrderAndTransferFilesProcessed = Convert.ToInt32(dsInformation.GetValue("U_OrTraF", index));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadConfigurationJohnDeereWarehouseHeader(ref WarehouseJDPRISM p_warehouseJDPRISM)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                    oForm.DataSources.DBDataSources.Add("@SCGD_JD");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        p_warehouseJDPRISM.DealerAccount = !string.IsNullOrEmpty(dsInformation.GetValue("U_DAcc", index)) ? dsInformation.GetValue("U_DAcc", index).ToString().Trim() : string.Empty;
                        p_warehouseJDPRISM.DBSWarehouse = !string.IsNullOrEmpty(dsInformation.GetValue("U_DBSWhs", index)) ? dsInformation.GetValue("U_DBSWhs", index).ToString().Trim() : string.Empty;
                        p_warehouseJDPRISM.WhereDataIsToBeLoaded = Convert.ToInt32(dsInformation.GetValue("U_DataL", index));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadGeneralConfiguration(ref JDPRISM p_jdprism, ref String p_strLoadType)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            List<InfoWarehouse> listWarehouse;
            InfoWarehouse infoWarehouse;
            string strDate;
            try
            {
                if (oForm != null)
                {
                    //*** Carga Encabezado ******
                    oForm.DataSources.DBDataSources.Add("@SCGD_JD");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);
                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        p_jdprism.FirmCode = !string.IsNullOrEmpty(dsInformation.GetValue("U_FCode", index)) ? dsInformation.GetValue("U_FCode", index).ToString().Trim() : string.Empty;
                        strDate = dsInformation.GetValue("U_IniDate", index).ToString();
                        p_jdprism.DataInitialDate = DateTime.ParseExact(strDate, "yyyyMMdd", n);
                        p_jdprism.MainAccount = !string.IsNullOrEmpty(dsInformation.GetValue("U_DAcc", index)) ? dsInformation.GetValue("U_DAcc", index).ToString().Trim() : string.Empty;
                        p_jdprism.Path = !string.IsNullOrEmpty(dsInformation.GetValue("U_Path", index)) ? dsInformation.GetValue("U_Path", index).ToString().Trim() : string.Empty;
                        switch (p_strLoadType)
                        {
                            case "D" :
                                p_jdprism.LoadType = "D";
                                break;
                            case "I" :
                                p_jdprism.LoadType = "I";
                                break;
                            default:
                                p_jdprism.LoadType = "D";
                                break;
                        }
                    }

                    dsInformation.Clear();
                    listWarehouse = new List<InfoWarehouse>();
                    //*** Carga Warehouse ******
                    oForm.DataSources.DBDataSources.Add("@SCGD_JD1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);
                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        infoWarehouse = new InfoWarehouse();
                        infoWarehouse.WhsCode= !string.IsNullOrEmpty(dsInformation.GetValue("U_WhsCod", index)) ? dsInformation.GetValue("U_WhsCod", index).ToString().Trim() : string.Empty;
                        infoWarehouse.WhsProcess= !string.IsNullOrEmpty(dsInformation.GetValue("U_WhsPro", index)) ? dsInformation.GetValue("U_WhsPro", index).ToString().Trim() : string.Empty;
                        listWarehouse.Add(infoWarehouse);
                    }
                    p_jdprism.infoWarehouse = listWarehouse;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
        public void LoadInfoWarehouse(ref oArticulo p_oItem, ref JDPRISM p_jdprism, ref InfoWarehouse p_infoWarehouse, bool p_blnWhsProcess=false)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInfoWarehouse;
            int intContador = 0;
            List<String> tempWarehouses;
            String strWarehouse;
            try
            {
                if (oForm != null)
                {
                    tempWarehouses = new List<string>();
                    oForm.DataSources.DBDataSources.Add("OITW");
                    dsInfoWarehouse = oForm.DataSources.DBDataSources.Item("OITW");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_oItem.ItemCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    
                    foreach (InfoWarehouse row in p_jdprism.infoWarehouse)
                    {
                        intContador += 1;
                        strWarehouse=(!p_blnWhsProcess) ? row.WhsCode : row.WhsProcess;

                        if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                        {
                            if (intContador != 1)
                            {
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                            }
                            oCondition = oConditions.Add();
                            oCondition.BracketOpenNum = 1;
                            oCondition.Alias = "WhsCode";
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                            oCondition.CondVal = strWarehouse;
                            oCondition.BracketCloseNum = 1;

                            if (intContador == 1)
                            {
                                oCondition.BracketOpenNum = 2;
                            }

                            tempWarehouses.Add(strWarehouse);
                        }
                    }
                    
                    oCondition.BracketCloseNum = 2;

                    dsInfoWarehouse.Query(oConditions);

                    for (int index = 0; index < dsInfoWarehouse.Size; index++)
                    {
                        p_infoWarehouse.OnHand += double.Parse( dsInfoWarehouse.GetValue("OnHand", index));
                        p_infoWarehouse.IsCommited += double.Parse(dsInfoWarehouse.GetValue("IsCommited", index));
                        p_infoWarehouse.OnOrder += double.Parse(dsInfoWarehouse.GetValue("OnOrder", index));
                    }

                    p_infoWarehouse.Available = p_infoWarehouse.OnHand - p_infoWarehouse.IsCommited + p_infoWarehouse.OnOrder;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        public void LoadInfoSalesOrder(ref oArticulo p_oItem, ref JDPRISM p_jdprism, ref oLineasDocumento  p_rowSalesOrder)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            int intContador = 0;
            List<String> tempWarehouses;
            String strWarehouse;
            List<String> tempWithWO;
            List<String> tempWithOutWO;
            String strDocEntry;
            String strWO;
            try
            {
                if (oForm != null)
                {
                    tempWarehouses = new List<string>();
                    tempWithWO = new List<string>();
                    tempWithOutWO =new List<string>();
                    oForm.DataSources.DBDataSources.Add("RDR1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("RDR1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_oItem.ItemCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "LineStatus";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "O";
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;


                    foreach (InfoWarehouse row in p_jdprism.infoWarehouse)
                    {
                        strWarehouse =  row.WhsCode;

                        if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                        {
                            intContador += 1;
                            if (intContador != 1)
                            {
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                            }
                            oCondition = oConditions.Add();
                            oCondition.BracketOpenNum = 1;
                            oCondition.Alias = "WhsCode";
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                            oCondition.CondVal = strWarehouse;
                            oCondition.BracketCloseNum = 1;

                            if (intContador == 1)
                            {
                                oCondition.BracketOpenNum = 2;
                            }

                            tempWarehouses.Add(strWarehouse);
                        }

                        strWarehouse = row.WhsProcess;

                        if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                        {
                            intContador += 1;
                            if (intContador != 1)
                            {
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                            }
                            oCondition = oConditions.Add();
                            oCondition.BracketOpenNum = 1;
                            oCondition.Alias = "WhsCode";
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                            oCondition.CondVal = strWarehouse;
                            oCondition.BracketCloseNum = 1;

                            if (intContador == 1)
                            {
                                oCondition.BracketOpenNum = 2;
                            }

                            tempWarehouses.Add(strWarehouse);
                        }
                    }

                    oCondition.BracketCloseNum = 2;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strWO = !string.IsNullOrEmpty(dsInformation.GetValue("U_SCGD_NoOT", index)) ? dsInformation.GetValue("U_SCGD_NoOT", index).ToString().Trim() : string.Empty;
                        strDocEntry = !string.IsNullOrEmpty(dsInformation.GetValue("DocEntry", index)) ? dsInformation.GetValue("DocEntry", index).ToString().Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(strWO))
                        {
                            if (!tempWithWO.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempWithWO.Add(strDocEntry);
                            }
                        }
                        else
                        {
                            if (!tempWithOutWO.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempWithOutWO.Add(strDocEntry);
                            }
                            p_rowSalesOrder.ReserveQ_PT += double.Parse(dsInformation.GetValue("Quantity", index));
                        }
                    }
                    p_rowSalesOrder.ReservedHits_WO = tempWithWO.Count;
                    p_rowSalesOrder.ReservedHits_PT = tempWithOutWO.Count;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }


        public void LoadInfoInvoice(ref oArticulo p_oItem, ref JDPRISM p_jdprism, ref oLineasDocumento p_rowInvoice,  MonthAgo p_MonthAgo)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            int intContador = 0;
            List<String> tempWarehouses;
            String strWarehouse;
            List<String> tempWithWO;
            List<String> tempWithOutWO;
            List<String> tempHitsMonth;
            List<String> tempLostHitsMonth;
            String strDocEntry;
            String strWO;
            DateTime fromDate;
            DateTime toDate;
            String strLineStatus;
            String strMonthHistoric = string.Empty;
            DateTime dateHistoric;
            try
            {
                if (oForm != null)
                {
                    tempWarehouses = new List<string>();
                    tempWithWO = new List<string>();
                    tempWithOutWO = new List<string>();
                    tempHitsMonth = new List<string>();
                    tempLostHitsMonth = new List<string>();
                    fromDate = DateTime.Today;
                    toDate = DateTime.Today;

                    GetDateFromAndTo(ref fromDate, ref toDate, ref p_MonthAgo);
                    
                    oForm.DataSources.DBDataSources.Add("INV1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("INV1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_oItem.ItemCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    //if (MonthAgo.ActualMonth == p_MonthAgo)
                    //{
                    //    oCondition = oConditions.Add();
                    //    oCondition.BracketOpenNum = 1;
                    //    oCondition.Alias = "LineStatus";
                    //    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    //    oCondition.CondVal = "O";
                    //    oCondition.BracketCloseNum = 1;
                    //    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;
                    //}

                    //foreach (InfoWarehouse row in p_jdprism.infoWarehouse)
                    //{
                    //    intContador += 1;
                    //    strWarehouse = row.WhsCode;

                    //    if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                    //    {
                    //        if (intContador != 1)
                    //        {
                    //            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                    //        }
                    //        oCondition = oConditions.Add();
                    //        oCondition.BracketOpenNum = 1;
                    //        oCondition.Alias = "WhsCode";
                    //        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    //        oCondition.CondVal = strWarehouse;
                    //        oCondition.BracketCloseNum = 1;

                    //        if (intContador == 1)
                    //        {
                    //            oCondition.BracketOpenNum = 2;
                    //        }

                    //        tempWarehouses.Add(strWarehouse);
                    //    }
                    //}
                    foreach (InfoWarehouse row in p_jdprism.infoWarehouse)
                    {
                        strWarehouse = row.WhsCode;

                        if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                        {
                            intContador += 1;
                            if (intContador != 1)
                            {
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                            }
                            oCondition = oConditions.Add();
                            oCondition.BracketOpenNum = 1;
                            oCondition.Alias = "WhsCode";
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                            oCondition.CondVal = strWarehouse;
                            oCondition.BracketCloseNum = 1;

                            if (intContador == 1)
                            {
                                oCondition.BracketOpenNum = 2;
                            }

                            tempWarehouses.Add(strWarehouse);
                        }

                        strWarehouse = row.WhsProcess;

                        if (!tempWarehouses.Contains(strWarehouse) & !string.IsNullOrEmpty(strWarehouse))
                        {
                            intContador += 1;
                            if (intContador != 1)
                            {
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                            }
                            oCondition = oConditions.Add();
                            oCondition.BracketOpenNum = 1;
                            oCondition.Alias = "WhsCode";
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                            oCondition.CondVal = strWarehouse;
                            oCondition.BracketCloseNum = 1;

                            if (intContador == 1)
                            {
                                oCondition.BracketOpenNum = 2;
                            }

                            tempWarehouses.Add(strWarehouse);
                        }
                    }

                    oCondition.BracketCloseNum = 2;

                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strWO = !string.IsNullOrEmpty(dsInformation.GetValue("U_SCGD_NoOT", index)) ? dsInformation.GetValue("U_SCGD_NoOT", index).ToString().Trim() : string.Empty;
                        strDocEntry = !string.IsNullOrEmpty(dsInformation.GetValue("DocEntry", index)) ? dsInformation.GetValue("DocEntry", index).ToString().Trim() : string.Empty;
                        strMonthHistoric = dsInformation.GetValue("DocDate", index).ToString();
                        dateHistoric = DateTime.ParseExact(strMonthHistoric, "yyyyMMdd", n);
                        strMonthHistoric = dateHistoric.ToString("yyyy") + dateHistoric.ToString("MM");
                        if (!string.IsNullOrEmpty(strMonthHistoric))
                        {
                            if (!monthHistoricList.Contains(strMonthHistoric))
                            {
                                monthHistoricList.Add(strMonthHistoric);
                            }
                        }
                        //*** valida si tiene OT o no
                        if (!string.IsNullOrEmpty(strWO))
                        {
                            if (!tempWithWO.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempWithWO.Add(strDocEntry);
                            }
                        }
                        else
                        {
                            if (!tempWithOutWO.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempWithOutWO.Add(strDocEntry);
                            }
                            p_rowInvoice.ReserveQ_PT += double.Parse(dsInformation.GetValue("Quantity", index));
                        }
                        strLineStatus = !string.IsNullOrEmpty(dsInformation.GetValue("LineStatus", index)) ? dsInformation.GetValue("LineStatus", index).ToString().Trim() : string.Empty;
                        //*** Valida si a linea esta abierta
                        if (strLineStatus == "O")
                        {
                            p_rowInvoice.Sales_Month += double.Parse(dsInformation.GetValue("Quantity", index));
                            if (!tempHitsMonth.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempHitsMonth.Add(strDocEntry);
                            }
                        }
                        else if (strLineStatus == "C")
                        {
                            p_rowInvoice.LostSales_Month += double.Parse(dsInformation.GetValue("Quantity", index));
                            if (!tempLostHitsMonth.Contains(strDocEntry) & !string.IsNullOrEmpty(strDocEntry))
                            {
                                tempLostHitsMonth.Add(strDocEntry);
                            }
                        }
                    }
                    p_rowInvoice.ReservedHits_WO = tempWithWO.Count;
                    p_rowInvoice.ReservedHits_PT = tempWithOutWO.Count;
                    p_rowInvoice.Hits_Month = tempHitsMonth.Count;
                    p_rowInvoice.LostHits_Month = tempLostHitsMonth.Count;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadInfoStockTransaction(ref oArticulo p_oItem, ref JDPRISM p_jdprism, ref oLineasDocumento p_rowStockTransaction)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            int intContador = 0;
            String strDate;
            try
            {
                if (oForm != null)
                {
                    oForm.DataSources.DBDataSources.Add("OINM");
                    dsInformation = oForm.DataSources.DBDataSources.Item("OINM");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_oItem.ItemCode;
                    oCondition.BracketCloseNum = 1;


                    dsInformation.Query(oConditions);

                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        strDate = dsInformation.GetValue("DocDate", index).ToString();
                        p_rowStockTransaction.DateAdded = DateTime.ParseExact(strDate, "yyyyMMdd", n);
                        break;
                    }

                    if (p_rowStockTransaction.DateAdded == System.DateTime.MinValue)
                    {
                        p_rowStockTransaction.DateAdded = new DateTime(1900,01,01);

                    ////Si no se encuentra la fecha del primer registro en un almacen se asigna la fecha de la creación del item
                    //oForm.DataSources.DBDataSources.Add("OITM");
                    //dsInformation = oForm.DataSources.DBDataSources.Item("OITM");

                    //oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    //oCondition = oConditions.Add();
                    //oCondition.BracketOpenNum = 1;
                    //oCondition.Alias = "ItemCode";
                    //oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    //oCondition.CondVal = p_oItem.ItemCode;
                    //oCondition.BracketCloseNum = 1;

                    //dsInformation.Query(oConditions);
                    //for (int index = 0; index < dsInformation.Size; index++)
                    //{
                    //    strDate = dsInformation.GetValue("CreateDate", index).ToString();
                    //    p_rowStockTransaction.DateAdded = DateTime.ParseExact(strDate, "yyyyMMdd", n);
                    //    break;
                    //}
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void GetDateFromAndTo(ref DateTime p_fromDate, ref DateTime p_toDate, ref MonthAgo p_MonthAgo)
        {
            try
            {
                switch (p_MonthAgo)
                {
                    case MonthAgo.ActualMonth:
                        p_fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        p_toDate = DateTime.Today;
                        break;
                    case MonthAgo.MonthAgo1:
                        CalculateDate(ref p_fromDate,ref p_toDate,  1);
                        break;
                    case MonthAgo.MonthAgo2:
                        CalculateDate(ref p_fromDate, ref p_toDate, 2);
                        break;
                    case MonthAgo.MonthAgo3:
                        CalculateDate(ref p_fromDate, ref p_toDate, 3);
                        break;
                    case MonthAgo.MonthAgo4:
                        CalculateDate(ref p_fromDate, ref p_toDate, 4);
                        break;
                    case MonthAgo.MonthAgo5:
                        CalculateDate(ref p_fromDate, ref p_toDate, 5);
                        break;
                    case MonthAgo.MonthAgo6:
                        CalculateDate(ref p_fromDate, ref p_toDate, 6);
                        break;
                    case MonthAgo.MonthAgo7:
                        CalculateDate(ref p_fromDate, ref p_toDate, 7);
                        break;
                    case MonthAgo.MonthAgo8:
                        CalculateDate(ref p_fromDate, ref p_toDate, 8);
                        break;
                    case MonthAgo.MonthAgo9:
                        CalculateDate(ref p_fromDate, ref p_toDate, 9);
                        break;
                    case MonthAgo.MonthAgo10:
                        CalculateDate(ref p_fromDate, ref p_toDate, 10);
                        break;
                    case MonthAgo.MonthAgo11:
                        CalculateDate(ref p_fromDate, ref p_toDate, 11);
                        break;
                    case MonthAgo.MonthAgo12:
                        CalculateDate(ref p_fromDate, ref p_toDate, 12);
                        break;
                    case MonthAgo.MonthAgo13:
                        CalculateDate(ref p_fromDate, ref p_toDate, 13);
                        break;
                    case MonthAgo.MonthAgo14:
                        CalculateDate(ref p_fromDate, ref p_toDate, 14);
                        break;
                    case MonthAgo.MonthAgo15:
                        CalculateDate(ref p_fromDate, ref p_toDate, 15);
                        break;
                    case MonthAgo.MonthAgo16:
                        CalculateDate(ref p_fromDate, ref p_toDate, 16);
                        break;
                    case MonthAgo.MonthAgo17:
                        CalculateDate(ref p_fromDate, ref p_toDate, 17);
                        break;
                    case MonthAgo.MonthAgo18:
                        CalculateDate(ref p_fromDate, ref p_toDate, 18);
                        break;
                    case MonthAgo.MonthAgo19:
                        CalculateDate(ref p_fromDate, ref p_toDate, 19);
                        break;
                    case MonthAgo.MonthAgo20:
                        CalculateDate(ref p_fromDate, ref p_toDate, 20);
                        break;
                    case MonthAgo.MonthAgo21:
                        CalculateDate(ref p_fromDate, ref p_toDate, 21);
                        break;
                    case MonthAgo.MonthAgo22:
                        CalculateDate(ref p_fromDate, ref p_toDate, 22);
                        break;
                    case MonthAgo.MonthAgo23:
                        CalculateDate(ref p_fromDate, ref p_toDate, 23);
                        break;
                    case MonthAgo.MonthAgo24:
                        CalculateDate(ref p_fromDate, ref p_toDate, 24);
                        break;
                    case MonthAgo.MonthAgo25:
                        CalculateDate(ref p_fromDate, ref p_toDate, 25);
                        break;
                    case MonthAgo.MonthAgo26:
                        CalculateDate(ref p_fromDate, ref p_toDate, 26);
                        break;
                    case MonthAgo.MonthAgo27:
                        CalculateDate(ref p_fromDate, ref p_toDate, 27);
                        break;
                    case MonthAgo.MonthAgo28:
                        CalculateDate(ref p_fromDate, ref p_toDate, 28);
                        break;
                    case MonthAgo.MonthAgo29:
                        CalculateDate(ref p_fromDate, ref p_toDate, 29);
                        break;
                    case MonthAgo.MonthAgo30:
                        CalculateDate(ref p_fromDate, ref p_toDate, 30);
                        break;
                    case MonthAgo.MonthAgo31:
                        CalculateDate(ref p_fromDate, ref p_toDate, 31);
                        break;
                    case MonthAgo.MonthAgo32:
                        CalculateDate(ref p_fromDate, ref p_toDate, 32);
                        break;
                    case MonthAgo.MonthAgo33:
                        CalculateDate(ref p_fromDate, ref p_toDate, 33);
                        break;
                    case MonthAgo.MonthAgo34:
                        CalculateDate(ref p_fromDate, ref p_toDate, 34);
                        break;
                    case MonthAgo.MonthAgo35:
                        CalculateDate(ref p_fromDate, ref p_toDate, 35);
                        break;
                    case MonthAgo.MonthAgo36:
                        CalculateDate(ref p_fromDate, ref p_toDate, 36);
                        break;
                    case MonthAgo.MonthAgo37:
                        CalculateDate(ref p_fromDate, ref p_toDate, 37);
                        break;
                    case MonthAgo.MonthAgo38:
                        CalculateDate(ref p_fromDate, ref p_toDate, 38);
                        break;
                    case MonthAgo.MonthAgo39:
                        CalculateDate(ref p_fromDate, ref p_toDate, 39);
                        break;
                    case MonthAgo.MonthAgo40:
                        CalculateDate(ref p_fromDate, ref p_toDate, 40);
                        break;
                    case MonthAgo.MonthAgo41:
                        CalculateDate(ref p_fromDate, ref p_toDate, 41);
                        break;
                    case MonthAgo.MonthAgo42:
                        CalculateDate(ref p_fromDate, ref p_toDate, 42);
                        break;
                    case MonthAgo.MonthAgo43:
                        CalculateDate(ref p_fromDate, ref p_toDate, 43);
                        break;
                    case MonthAgo.MonthAgo44:
                        CalculateDate(ref p_fromDate, ref p_toDate, 44);
                        break;
                    case MonthAgo.MonthAgo45:
                        CalculateDate(ref p_fromDate, ref p_toDate, 45);
                        break;
                    case MonthAgo.MonthAgo46:
                        CalculateDate(ref p_fromDate, ref p_toDate, 46);
                        break;
                    case MonthAgo.MonthAgo47:
                        CalculateDate(ref p_fromDate, ref p_toDate, 47);
                        break;
                    case MonthAgo.MonthAgo48:
                        CalculateDate(ref p_fromDate, ref p_toDate, 48);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void TotalSalesAndHits(ref DetailJDPRISM p_oRecord,  TotalSalesHits p_TotalSalesHits)
        {
            try
            {
                switch (p_TotalSalesHits)
                {
                    case TotalSalesHits.Total_1To12:
                        //*** Month 1 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_1;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_1;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_1;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_1;
                        //*** Month 2 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_2;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_2;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_2;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_2;
                        //*** Month 3 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_3;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_3;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_3;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_3;
                        //*** Month 4 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_4;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_4;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_4;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_4;
                        //*** Month 5 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_5;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_5;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_5;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_5;
                        //*** Month 6 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_6;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_6;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_6;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_6;
                        //*** Month 7 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_7;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_7;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_7;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_7;
                        //*** Month 8 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_8;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_8;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_8;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_8;
                        //*** Month 9 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_9;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_9;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_9;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_9;
                        //*** Month 10 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_10;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_10;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_10;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_10;
                        //*** Month 11 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_11;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_11;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_11;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_11;
                        //*** Month 12 ***
                        p_oRecord.TotalSales_1To12 += p_oRecord.Sales_Month_12;
                        p_oRecord.TotalHits_1To12 += p_oRecord.Hits_Month_12;
                        p_oRecord.TotalLostSales_1To12 += p_oRecord.LostSales_Month_12;
                        p_oRecord.TotalLostHits_1To12 += p_oRecord.LostHits_Month_12;
                        break;
                    case TotalSalesHits.Total_13To24:
                        //*** Month 13 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_13;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_13;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_13;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_13;
                        //*** Month 14 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_14;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_14;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_14;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_14;
                        //*** Month 15 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_15;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_15;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_15;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_15;
                        //*** Month 16 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_16;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_16;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_16;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_16;
                        //*** Month 17 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_17;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_17;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_17;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_17;
                        //*** Month 18 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_18;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_18;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_18;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_18;
                        //*** Month 19 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_19;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_19;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_19;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_19;
                        //*** Month 20 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_20;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_20;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_20;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_20;
                        //*** Month 21 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_21;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_21;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_21;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_21;
                        //*** Month 22 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_22;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_22;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_22;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_22;
                        //*** Month 23 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_23;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_23;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_23;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_23;
                        //*** Month 24 ***
                        p_oRecord.TotalSales_13To24 += p_oRecord.Sales_Month_24;
                        p_oRecord.TotalHits_13To24 += p_oRecord.Hits_Month_24;
                        p_oRecord.TotalLostSales_13To24 += p_oRecord.LostSales_Month_24;
                        p_oRecord.TotalLostHits_13To24 += p_oRecord.LostHits_Month_24;
                        break;
                    case TotalSalesHits.Total_25To36:
                        //*** Month 25 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_25;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_25;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_25;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_25;
                        //*** Month 26 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_26;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_26;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_26;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_26;
                        //*** Month 27 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_27;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_27;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_27;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_27;
                        //*** Month 28 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_28;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_28;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_28;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_28;
                        //*** Month 29 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_29;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_29;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_29;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_29;
                        //*** Month 30 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_30;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_30;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_30;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_30;
                        //*** Month 31 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_31;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_31;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_31;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_31;
                        //*** Month 32 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_32;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_32;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_32;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_32;
                        //*** Month 33 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_33;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_33;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_33;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_33;
                        //*** Month 34 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_34;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_34;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_34;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_34;
                        //*** Month 35 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_35;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_35;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_35;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_35;
                        //*** Month 36 ***
                        p_oRecord.TotalSales_25To36 += p_oRecord.Sales_Month_36;
                        p_oRecord.TotalHits_25To36 += p_oRecord.Hits_Month_36;
                        p_oRecord.TotalLostSales_25To36 += p_oRecord.LostSales_Month_36;
                        p_oRecord.TotalLostHits_25To36 += p_oRecord.LostHits_Month_36;
                        break;
                    case TotalSalesHits.Total_37To48:
                        //*** Month 37 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_37;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_37;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_37;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_37;
                        //*** Month 38 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_38;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_38;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_38;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_38;
                        //*** Month 39 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_39;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_39;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_39;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_39;
                        //*** Month 40 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_40;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_40;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_40;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_40;
                        //*** Month 41 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_41;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_41;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_41;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_41;
                        //*** Month 42 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_42;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_42;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_42;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_42;
                        //*** Month 43 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_43;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_43;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_43;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_43;
                        //*** Month 44 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_44;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_44;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_44;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_44;
                        //*** Month 45 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_45;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_45;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_45;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_45;
                        //*** Month 46 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_46;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_46;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_46;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_46;
                        //*** Month 47 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_47;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_47;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_47;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_47;
                        //*** Month 48 ***
                        p_oRecord.TotalSales_37To48 += p_oRecord.Sales_Month_48;
                        p_oRecord.TotalHits_37To48 += p_oRecord.Hits_Month_48;
                        p_oRecord.TotalLostSales_37To48 += p_oRecord.LostSales_Month_48;
                        p_oRecord.TotalLostHits_37To48 += p_oRecord.LostHits_Month_48;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void AddColumnsToMatrix(ref DataTable p_dtMatriz)
        {
            DetailJDPRISM oRecord;
            try
            {
                //p_dtMatrix.Columns.Add("Columna1", BoFieldsType.ft_AlphaNumeric);
                //p_dtMatrix.Columns.Add("Columna2", BoFieldsType.ft_AlphaNumeric);
                //p_dtMatrix.Columns.Add("Columna3", BoFieldsType.ft_AlphaNumeric);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void DataLinkToMatrix(ref DetailJDPRISM p_oRecord, ref DataTable  p_dtMatriz,ref JDPRISM p_jdprism)
        {
            DetailJDPRISM oRecordEntity;
            List<DetailJDPRISM> detailList = new List<DetailJDPRISM>();
            try
            {
                p_dtMatriz.Rows.Add(); // Agregar una nueva linea
                int lastRowIndex = p_dtMatriz.Rows.Count - 1;
                p_dtMatriz.SetValue("RecordCode", lastRowIndex, p_oRecord.RecordCode.ToString());//1
                p_dtMatriz.SetValue("PartNumber", lastRowIndex, p_oRecord.PartNumber.ToString());//2
                p_dtMatriz.SetValue("AvailableQuantity", lastRowIndex, (p_oRecord.AvailableQuantity >= 0) ? p_oRecord.AvailableQuantity.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("OOQuantity", lastRowIndex, (p_oRecord.OOQuantity >= 0) ? p_oRecord.OOQuantity.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("ReserveQ_WO", lastRowIndex, (p_oRecord.ReserveQ_WO >= 0) ? p_oRecord.ReserveQ_WO.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("ReserveQ_PT", lastRowIndex, (p_oRecord.ReserveQ_PT >= 0) ? p_oRecord.ReserveQ_PT.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("CurrrentMTDSales", lastRowIndex, (p_oRecord.CurrrentMTDSales >= 0) ? p_oRecord.CurrrentMTDSales.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("CurrentMTDHits", lastRowIndex, (p_oRecord.CurrentMTDHits >= 0) ? p_oRecord.CurrentMTDHits.ToString() : string.Empty);//
                p_dtMatriz.SetValue("CurrentMTDLostSales", lastRowIndex, (p_oRecord.CurrentMTDLostSales >= 0) ? p_oRecord.CurrentMTDLostSales.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("CurrentMTDLostHits", lastRowIndex, (p_oRecord.CurrentMTDLostHits >= 0) ? p_oRecord.CurrentMTDLostHits.ToString() : string.Empty);//
                p_dtMatriz.SetValue("DealerPPP", lastRowIndex, (p_oRecord.DealerPPP >= 0) ? p_oRecord.DealerPPP.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("BinLocation", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.BinLocation)) ? p_oRecord.BinLocation : string.Empty);//
                p_dtMatriz.SetValue("AlternateBinLocation", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.AlternateBinLocation)) ? p_oRecord.AlternateBinLocation : string.Empty);//
                p_dtMatriz.SetValue("VendorPartCost", lastRowIndex, (p_oRecord.VendorPartCost >= 0) ? p_oRecord.VendorPartCost.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("VendorPackageQuantity", lastRowIndex, (p_oRecord.VendorPackageQuantity >= 0) ? p_oRecord.VendorPackageQuantity.ToString() : string.Empty);//
                p_dtMatriz.SetValue("VendorCode", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.VendorCode)) ? p_oRecord.VendorCode : string.Empty);//
                p_dtMatriz.SetValue("VendorSubstitutionInfo", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.VendorSubstitutionInfo)) ? p_oRecord.VendorSubstitutionInfo : string.Empty);//
                p_dtMatriz.SetValue("PricingBase", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.PricingBase)) ? p_oRecord.PricingBase : string.Empty);//
                p_dtMatriz.SetValue("PricingAdditive", lastRowIndex, (p_oRecord.PricingAdditive >= 0) ? p_oRecord.PricingAdditive.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("DealerPrice", lastRowIndex, (p_oRecord.DealerPrice >= 0) ? p_oRecord.DealerPrice.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("OrderFormulaCode", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.OrderFormulaCode)) ? p_oRecord.OrderFormulaCode : string.Empty);//
                p_dtMatriz.SetValue("DeleteIndicator", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.DeleteIndicator)) ? p_oRecord.DeleteIndicator : string.Empty);//
                p_dtMatriz.SetValue("ReservedHits_WO", lastRowIndex, (p_oRecord.ReservedHits_WO >= 0) ? p_oRecord.ReservedHits_WO.ToString() : string.Empty);//
                p_dtMatriz.SetValue("ReservedHits_PT", lastRowIndex, (p_oRecord.ReservedHits_PT >= 0) ? p_oRecord.ReservedHits_PT.ToString() : string.Empty);//
                p_dtMatriz.SetValue("AverageCost", lastRowIndex, (p_oRecord.AverageCost >= 0) ? p_oRecord.AverageCost.ToString("N2") : string.Empty);//
                //*************** valid according to load type ********************
                //if (p_jdprism.LoadType == "D")
                //{
                //    return;
                //}
                //*****************************************************************
                p_dtMatriz.SetValue("Start_I_Records", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.Start_I_Records)) ? p_oRecord.Start_I_Records : string.Empty);//
                p_dtMatriz.SetValue("PartDescription", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.PartDescription)) ? p_oRecord.PartDescription : string.Empty);//
                p_dtMatriz.SetValue("DealerPartNote", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.DealerPartNote)) ? p_oRecord.DealerPartNote : string.Empty);//
                p_dtMatriz.SetValue("OrderIndicator", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.OrderIndicator)) ? p_oRecord.OrderIndicator : string.Empty);//
                p_dtMatriz.SetValue("DateAdded", lastRowIndex, p_oRecord.DateAdded.ToString("dd/MM/yyyy"));//
                p_dtMatriz.SetValue("DealerGroupCode", lastRowIndex, (!string.IsNullOrEmpty(p_oRecord.DealerGroupCode)) ? p_oRecord.DealerGroupCode : string.Empty);//
                p_dtMatriz.SetValue("MinOrderQuantity", lastRowIndex, (p_oRecord.MinOrderQuantity >= 0) ? p_oRecord.MinOrderQuantity.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("MaxOrderQuantity", lastRowIndex, (p_oRecord.MaxOrderQuantity >= 0) ? p_oRecord.MaxOrderQuantity.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("NumberOfMonthlyHistory", lastRowIndex, (p_oRecord.NumberOfMonthlyHistory >= 0) ? p_oRecord.NumberOfMonthlyHistory.ToString() : string.Empty);//
                p_dtMatriz.SetValue("PiecesInSet", lastRowIndex, (p_oRecord.PiecesInSet >= 0) ? p_oRecord.PiecesInSet.ToString() : string.Empty);//
                //***** Month Ago 1 ***
                p_dtMatriz.SetValue("Sales_Month_1", lastRowIndex, (p_oRecord.Sales_Month_1 >= 0) ? p_oRecord.Sales_Month_1.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_1", lastRowIndex, (p_oRecord.Hits_Month_1 >= 0) ? p_oRecord.Hits_Month_1.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_1", lastRowIndex, (p_oRecord.LostSales_Month_1 >= 0) ? p_oRecord.LostSales_Month_1.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_1", lastRowIndex, (p_oRecord.LostHits_Month_1 >= 0) ? p_oRecord.LostHits_Month_1.ToString() : string.Empty);//
                //***** Month Ago 2 ***
                p_dtMatriz.SetValue("Sales_Month_2", lastRowIndex, (p_oRecord.Sales_Month_2 >= 0) ? p_oRecord.Sales_Month_2.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_2", lastRowIndex, (p_oRecord.Hits_Month_2 >= 0) ? p_oRecord.Hits_Month_2.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_2", lastRowIndex, (p_oRecord.LostSales_Month_2 >= 0) ? p_oRecord.LostSales_Month_2.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_2", lastRowIndex, (p_oRecord.LostHits_Month_2 >= 0) ? p_oRecord.LostHits_Month_2.ToString() : string.Empty);//
                //***** Month Ago 3 ***
                p_dtMatriz.SetValue("Sales_Month_3", lastRowIndex, (p_oRecord.Sales_Month_3 >= 0) ? p_oRecord.Sales_Month_3.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_3", lastRowIndex, (p_oRecord.Hits_Month_3 >= 0) ? p_oRecord.Hits_Month_3.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_3", lastRowIndex, (p_oRecord.LostSales_Month_3 >= 0) ? p_oRecord.LostSales_Month_3.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_3", lastRowIndex, (p_oRecord.LostHits_Month_3 >= 0) ? p_oRecord.LostHits_Month_3.ToString() : string.Empty);//
                //***** Month Ago 4 ***
                p_dtMatriz.SetValue("Sales_Month_4", lastRowIndex, (p_oRecord.Sales_Month_4 >= 0) ? p_oRecord.Sales_Month_4.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_4", lastRowIndex, (p_oRecord.Hits_Month_4 >= 0) ? p_oRecord.Hits_Month_4.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_4", lastRowIndex, (p_oRecord.LostSales_Month_4 >= 0) ? p_oRecord.LostSales_Month_4.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_4", lastRowIndex, (p_oRecord.LostHits_Month_4 >= 0) ? p_oRecord.LostHits_Month_4.ToString() : string.Empty);//
                //***** Month Ago 5 ***
                p_dtMatriz.SetValue("Sales_Month_5", lastRowIndex, (p_oRecord.Sales_Month_5 >= 0) ? p_oRecord.Sales_Month_5.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_5", lastRowIndex, (p_oRecord.Hits_Month_5 >= 0) ? p_oRecord.Hits_Month_5.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_5", lastRowIndex, (p_oRecord.LostSales_Month_5 >= 0) ? p_oRecord.LostSales_Month_5.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_5", lastRowIndex, (p_oRecord.LostHits_Month_5 >= 0) ? p_oRecord.LostHits_Month_5.ToString() : string.Empty);//
                //***** Month Ago 6 ***
                p_dtMatriz.SetValue("Sales_Month_6", lastRowIndex, (p_oRecord.Sales_Month_6 >= 0) ? p_oRecord.Sales_Month_6.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_6", lastRowIndex, (p_oRecord.Hits_Month_6 >= 0) ? p_oRecord.Hits_Month_6.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_6", lastRowIndex, (p_oRecord.LostSales_Month_6 >= 0) ? p_oRecord.LostSales_Month_6.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_6", lastRowIndex, (p_oRecord.LostHits_Month_6 >= 0) ? p_oRecord.LostHits_Month_6.ToString() : string.Empty);//
                //***** Month Ago 7 ***
                p_dtMatriz.SetValue("Sales_Month_7", lastRowIndex, (p_oRecord.Sales_Month_7 >= 0) ? p_oRecord.Sales_Month_7.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_7", lastRowIndex, (p_oRecord.Hits_Month_7 >= 0) ? p_oRecord.Hits_Month_7.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_7", lastRowIndex, (p_oRecord.LostSales_Month_7 >= 0) ? p_oRecord.LostSales_Month_7.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_7", lastRowIndex, (p_oRecord.LostHits_Month_7 >= 0) ? p_oRecord.LostHits_Month_7.ToString() : string.Empty);//
                //***** Month Ago 8 ***
                p_dtMatriz.SetValue("Sales_Month_8", lastRowIndex, (p_oRecord.Sales_Month_8 >= 0) ? p_oRecord.Sales_Month_8.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_8", lastRowIndex, (p_oRecord.Hits_Month_8 >= 0) ? p_oRecord.Hits_Month_8.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_8", lastRowIndex, (p_oRecord.LostSales_Month_8 >= 0) ? p_oRecord.LostSales_Month_8.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_8", lastRowIndex, (p_oRecord.LostHits_Month_8 >= 0) ? p_oRecord.LostHits_Month_8.ToString() : string.Empty);//
                //***** Month Ago 9 ***
                p_dtMatriz.SetValue("Sales_Month_9", lastRowIndex, (p_oRecord.Sales_Month_9 >= 0) ? p_oRecord.Sales_Month_9.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_9", lastRowIndex, (p_oRecord.Hits_Month_9 >= 0) ? p_oRecord.Hits_Month_9.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_9", lastRowIndex, (p_oRecord.LostSales_Month_9 >= 0) ? p_oRecord.LostSales_Month_9.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_9", lastRowIndex, (p_oRecord.LostHits_Month_9 >= 0) ? p_oRecord.LostHits_Month_9.ToString() : string.Empty);//
                //***** Month Ago 10 ***
                p_dtMatriz.SetValue("Sales_Month_10", lastRowIndex, (p_oRecord.Sales_Month_10 >= 0) ? p_oRecord.Sales_Month_10.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_10", lastRowIndex, (p_oRecord.Hits_Month_10 >= 0) ? p_oRecord.Hits_Month_10.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_10", lastRowIndex, (p_oRecord.LostSales_Month_10 >= 0) ? p_oRecord.LostSales_Month_10.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_10", lastRowIndex, (p_oRecord.LostHits_Month_10 >= 0) ? p_oRecord.LostHits_Month_10.ToString() : string.Empty);//
                //***** Month Ago 11 ***
                p_dtMatriz.SetValue("Sales_Month_11", lastRowIndex, (p_oRecord.Sales_Month_11 >= 0) ? p_oRecord.Sales_Month_11.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_11", lastRowIndex, (p_oRecord.Hits_Month_11 >= 0) ? p_oRecord.Hits_Month_11.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_11", lastRowIndex, (p_oRecord.LostSales_Month_11 >= 0) ? p_oRecord.LostSales_Month_11.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_11", lastRowIndex, (p_oRecord.LostHits_Month_11 >= 0) ? p_oRecord.LostHits_Month_11.ToString() : string.Empty);//
                //***** Month Ago 12 ***
                p_dtMatriz.SetValue("Sales_Month_12", lastRowIndex, (p_oRecord.Sales_Month_12 >= 0) ? p_oRecord.Sales_Month_12.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_12", lastRowIndex, (p_oRecord.Hits_Month_12 >= 0) ? p_oRecord.Hits_Month_12.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_12", lastRowIndex, (p_oRecord.LostSales_Month_12 >= 0) ? p_oRecord.LostSales_Month_12.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_12", lastRowIndex, (p_oRecord.LostHits_Month_12 >= 0) ? p_oRecord.LostHits_Month_12.ToString() : string.Empty);//
                //***** Month Ago 13 ***
                p_dtMatriz.SetValue("Sales_Month_13", lastRowIndex, (p_oRecord.Sales_Month_13 >= 0) ? p_oRecord.Sales_Month_13.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_13", lastRowIndex, (p_oRecord.Hits_Month_13 >= 0) ? p_oRecord.Hits_Month_13.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_13", lastRowIndex, (p_oRecord.LostSales_Month_13 >= 0) ? p_oRecord.LostSales_Month_13.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_13", lastRowIndex, (p_oRecord.LostHits_Month_13 >= 0) ? p_oRecord.LostHits_Month_13.ToString() : string.Empty);//
                //***** Month Ago 14 ***
                p_dtMatriz.SetValue("Sales_Month_14", lastRowIndex, (p_oRecord.Sales_Month_14 >= 0) ? p_oRecord.Sales_Month_14.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_14", lastRowIndex, (p_oRecord.Hits_Month_14 >= 0) ? p_oRecord.Hits_Month_14.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_14", lastRowIndex, (p_oRecord.LostSales_Month_14 >= 0) ? p_oRecord.LostSales_Month_14.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_14", lastRowIndex, (p_oRecord.LostHits_Month_14 >= 0) ? p_oRecord.LostHits_Month_14.ToString() : string.Empty);//
                //***** Month Ago 15 ***
                p_dtMatriz.SetValue("Sales_Month_15", lastRowIndex, (p_oRecord.Sales_Month_15 >= 0) ? p_oRecord.Sales_Month_15.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_15", lastRowIndex, (p_oRecord.Hits_Month_15 >= 0) ? p_oRecord.Hits_Month_15.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_15", lastRowIndex, (p_oRecord.LostSales_Month_15 >= 0) ? p_oRecord.LostSales_Month_15.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_15", lastRowIndex, (p_oRecord.LostHits_Month_15 >= 0) ? p_oRecord.LostHits_Month_15.ToString() : string.Empty);//
                //***** Month Ago 16 ***
                p_dtMatriz.SetValue("Sales_Month_16", lastRowIndex, (p_oRecord.Sales_Month_16 >= 0) ? p_oRecord.Sales_Month_16.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_16", lastRowIndex, (p_oRecord.Hits_Month_16 >= 0) ? p_oRecord.Hits_Month_16.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_16", lastRowIndex, (p_oRecord.LostSales_Month_16 >= 0) ? p_oRecord.LostSales_Month_16.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_16", lastRowIndex, (p_oRecord.LostHits_Month_16 >= 0) ? p_oRecord.LostHits_Month_16.ToString() : string.Empty);//
                //***** Month Ago 17 ***
                p_dtMatriz.SetValue("Sales_Month_17", lastRowIndex, (p_oRecord.Sales_Month_17 >= 0) ? p_oRecord.Sales_Month_17.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_17", lastRowIndex, (p_oRecord.Hits_Month_17 >= 0) ? p_oRecord.Hits_Month_17.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_17", lastRowIndex, (p_oRecord.LostSales_Month_17 >= 0) ? p_oRecord.LostSales_Month_17.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_17", lastRowIndex, (p_oRecord.LostHits_Month_17 >= 0) ? p_oRecord.LostHits_Month_17.ToString() : string.Empty);//
                //***** Month Ago 18 ***
                p_dtMatriz.SetValue("Sales_Month_18", lastRowIndex, (p_oRecord.Sales_Month_18 >= 0) ? p_oRecord.Sales_Month_18.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_18", lastRowIndex, (p_oRecord.Hits_Month_18 >= 0) ? p_oRecord.Hits_Month_18.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_18", lastRowIndex, (p_oRecord.LostSales_Month_18 >= 0) ? p_oRecord.LostSales_Month_18.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_18", lastRowIndex, (p_oRecord.LostHits_Month_18 >= 0) ? p_oRecord.LostHits_Month_18.ToString() : string.Empty);//
                //***** Month Ago 19 ***
                p_dtMatriz.SetValue("Sales_Month_19", lastRowIndex, (p_oRecord.Sales_Month_19 >= 0) ? p_oRecord.Sales_Month_19.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_19", lastRowIndex, (p_oRecord.Hits_Month_19 >= 0) ? p_oRecord.Hits_Month_19.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_19", lastRowIndex, (p_oRecord.LostSales_Month_19 >= 0) ? p_oRecord.LostSales_Month_19.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_19", lastRowIndex, (p_oRecord.LostHits_Month_19 >= 0) ? p_oRecord.LostHits_Month_19.ToString() : string.Empty);//
                //***** Month Ago 20 ***
                p_dtMatriz.SetValue("Sales_Month_20", lastRowIndex, (p_oRecord.Sales_Month_20 >= 0) ? p_oRecord.Sales_Month_20.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_20", lastRowIndex, (p_oRecord.Hits_Month_20 >= 0) ? p_oRecord.Hits_Month_20.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_20", lastRowIndex, (p_oRecord.LostSales_Month_20 >= 0) ? p_oRecord.LostSales_Month_20.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_20", lastRowIndex, (p_oRecord.LostHits_Month_20 >= 0) ? p_oRecord.LostHits_Month_20.ToString() : string.Empty);//
                //***** Month Ago 21 ***
                p_dtMatriz.SetValue("Sales_Month_21", lastRowIndex, (p_oRecord.Sales_Month_21 >= 0) ? p_oRecord.Sales_Month_21.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_21", lastRowIndex, (p_oRecord.Hits_Month_21 >= 0) ? p_oRecord.Hits_Month_21.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_21", lastRowIndex, (p_oRecord.LostSales_Month_21 >= 0) ? p_oRecord.LostSales_Month_21.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_21", lastRowIndex, (p_oRecord.LostHits_Month_21 >= 0) ? p_oRecord.LostHits_Month_21.ToString() : string.Empty);//
                //***** Month Ago 22 ***
                p_dtMatriz.SetValue("Sales_Month_22", lastRowIndex, (p_oRecord.Sales_Month_22 >= 0) ? p_oRecord.Sales_Month_22.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_22", lastRowIndex, (p_oRecord.Hits_Month_22 >= 0) ? p_oRecord.Hits_Month_22.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_22", lastRowIndex, (p_oRecord.LostSales_Month_22 >= 0) ? p_oRecord.LostSales_Month_22.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_22", lastRowIndex, (p_oRecord.LostHits_Month_22 >= 0) ? p_oRecord.LostHits_Month_22.ToString() : string.Empty);//
                //***** Month Ago 23 ***
                p_dtMatriz.SetValue("Sales_Month_23", lastRowIndex, (p_oRecord.Sales_Month_23 >= 0) ? p_oRecord.Sales_Month_23.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_23", lastRowIndex, (p_oRecord.Hits_Month_23 >= 0) ? p_oRecord.Hits_Month_23.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_23", lastRowIndex, (p_oRecord.LostSales_Month_23 >= 0) ? p_oRecord.LostSales_Month_23.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_23", lastRowIndex, (p_oRecord.LostHits_Month_23 >= 0) ? p_oRecord.LostHits_Month_23.ToString() : string.Empty);//
                //***** Month Ago 24 ***
                p_dtMatriz.SetValue("Sales_Month_24", lastRowIndex, (p_oRecord.Sales_Month_24 >= 0) ? p_oRecord.Sales_Month_24.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_24", lastRowIndex, (p_oRecord.Hits_Month_24 >= 0) ? p_oRecord.Hits_Month_24.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_24", lastRowIndex, (p_oRecord.LostSales_Month_24 >= 0) ? p_oRecord.LostSales_Month_24.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_24", lastRowIndex, (p_oRecord.LostHits_Month_24 >= 0) ? p_oRecord.LostHits_Month_24.ToString() : string.Empty);//
                //***** Month Ago 25 ***
                p_dtMatriz.SetValue("Sales_Month_25", lastRowIndex, (p_oRecord.Sales_Month_25 >= 0) ? p_oRecord.Sales_Month_25.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_25", lastRowIndex, (p_oRecord.Hits_Month_25 >= 0) ? p_oRecord.Hits_Month_25.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_25", lastRowIndex, (p_oRecord.LostSales_Month_25 >= 0) ? p_oRecord.LostSales_Month_25.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_25", lastRowIndex, (p_oRecord.LostHits_Month_25 >= 0) ? p_oRecord.LostHits_Month_25.ToString() : string.Empty);//
                //***** Month Ago 26 ***
                p_dtMatriz.SetValue("Sales_Month_26", lastRowIndex, (p_oRecord.Sales_Month_26 >= 0) ? p_oRecord.Sales_Month_26.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_26", lastRowIndex, (p_oRecord.Hits_Month_26 >= 0) ? p_oRecord.Hits_Month_26.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_26", lastRowIndex, (p_oRecord.LostSales_Month_26 >= 0) ? p_oRecord.LostSales_Month_26.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_26", lastRowIndex, (p_oRecord.LostHits_Month_26 >= 0) ? p_oRecord.LostHits_Month_26.ToString() : string.Empty);//
                //***** Month Ago 27 ***
                p_dtMatriz.SetValue("Sales_Month_27", lastRowIndex, (p_oRecord.Sales_Month_27 >= 0) ? p_oRecord.Sales_Month_27.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_27", lastRowIndex, (p_oRecord.Hits_Month_27 >= 0) ? p_oRecord.Hits_Month_27.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_27", lastRowIndex, (p_oRecord.LostSales_Month_27 >= 0) ? p_oRecord.LostSales_Month_27.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_27", lastRowIndex, (p_oRecord.LostHits_Month_27 >= 0) ? p_oRecord.LostHits_Month_27.ToString() : string.Empty);//
                //***** Month Ago 28 ***
                p_dtMatriz.SetValue("Sales_Month_28", lastRowIndex, (p_oRecord.Sales_Month_28 >= 0) ? p_oRecord.Sales_Month_28.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_28", lastRowIndex, (p_oRecord.Hits_Month_28 >= 0) ? p_oRecord.Hits_Month_28.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_28", lastRowIndex, (p_oRecord.LostSales_Month_28 >= 0) ? p_oRecord.LostSales_Month_28.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_28", lastRowIndex, (p_oRecord.LostHits_Month_28 >= 0) ? p_oRecord.LostHits_Month_28.ToString() : string.Empty);//
                //***** Month Ago 29 ***
                p_dtMatriz.SetValue("Sales_Month_29", lastRowIndex, (p_oRecord.Sales_Month_29 >= 0) ? p_oRecord.Sales_Month_29.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_29", lastRowIndex, (p_oRecord.Hits_Month_29 >= 0) ? p_oRecord.Hits_Month_29.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_29", lastRowIndex, (p_oRecord.LostSales_Month_29 >= 0) ? p_oRecord.LostSales_Month_29.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_29", lastRowIndex, (p_oRecord.LostHits_Month_29 >= 0) ? p_oRecord.LostHits_Month_29.ToString() : string.Empty);//
                //***** Month Ago 30 ***
                p_dtMatriz.SetValue("Sales_Month_30", lastRowIndex, (p_oRecord.Sales_Month_30 >= 0) ? p_oRecord.Sales_Month_30.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_30", lastRowIndex, (p_oRecord.Hits_Month_30 >= 0) ? p_oRecord.Hits_Month_30.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_30", lastRowIndex, (p_oRecord.LostSales_Month_30 >= 0) ? p_oRecord.LostSales_Month_30.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_30", lastRowIndex, (p_oRecord.LostHits_Month_30 >= 0) ? p_oRecord.LostHits_Month_30.ToString() : string.Empty);//
                //***** Month Ago 31 ***
                p_dtMatriz.SetValue("Sales_Month_31", lastRowIndex, (p_oRecord.Sales_Month_31 >= 0) ? p_oRecord.Sales_Month_31.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_31", lastRowIndex, (p_oRecord.Hits_Month_31 >= 0) ? p_oRecord.Hits_Month_31.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_31", lastRowIndex, (p_oRecord.LostSales_Month_31 >= 0) ? p_oRecord.LostSales_Month_31.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_31", lastRowIndex, (p_oRecord.LostHits_Month_31 >= 0) ? p_oRecord.LostHits_Month_31.ToString() : string.Empty);//
                //***** Month Ago 32 ***
                p_dtMatriz.SetValue("Sales_Month_32", lastRowIndex, (p_oRecord.Sales_Month_32 >= 0) ? p_oRecord.Sales_Month_32.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_32", lastRowIndex, (p_oRecord.Hits_Month_32 >= 0) ? p_oRecord.Hits_Month_32.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_32", lastRowIndex, (p_oRecord.LostSales_Month_32 >= 0) ? p_oRecord.LostSales_Month_32.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_32", lastRowIndex, (p_oRecord.LostHits_Month_32 >= 0) ? p_oRecord.LostHits_Month_32.ToString() : string.Empty);//
                //***** Month Ago 33 ***
                p_dtMatriz.SetValue("Sales_Month_33", lastRowIndex, (p_oRecord.Sales_Month_33 >= 0) ? p_oRecord.Sales_Month_33.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_33", lastRowIndex, (p_oRecord.Hits_Month_33 >= 0) ? p_oRecord.Hits_Month_33.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_33", lastRowIndex, (p_oRecord.LostSales_Month_33 >= 0) ? p_oRecord.LostSales_Month_33.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_33", lastRowIndex, (p_oRecord.LostHits_Month_33 >= 0) ? p_oRecord.LostHits_Month_33.ToString() : string.Empty);//
                //***** Month Ago 34 ***
                p_dtMatriz.SetValue("Sales_Month_34", lastRowIndex, (p_oRecord.Sales_Month_34 >= 0) ? p_oRecord.Sales_Month_34.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_34", lastRowIndex, (p_oRecord.Hits_Month_34 >= 0) ? p_oRecord.Hits_Month_34.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_34", lastRowIndex, (p_oRecord.LostSales_Month_34 >= 0) ? p_oRecord.LostSales_Month_34.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_34", lastRowIndex, (p_oRecord.LostHits_Month_34 >= 0) ? p_oRecord.LostHits_Month_34.ToString() : string.Empty);//
                //***** Month Ago 35 ***
                p_dtMatriz.SetValue("Sales_Month_35", lastRowIndex, (p_oRecord.Sales_Month_35 >= 0) ? p_oRecord.Sales_Month_35.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_35", lastRowIndex, (p_oRecord.Hits_Month_35 >= 0) ? p_oRecord.Hits_Month_35.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_35", lastRowIndex, (p_oRecord.LostSales_Month_35 >= 0) ? p_oRecord.LostSales_Month_35.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_35", lastRowIndex, (p_oRecord.LostHits_Month_35 >= 0) ? p_oRecord.LostHits_Month_35.ToString() : string.Empty);//
                //***** Month Ago 36 ***
                p_dtMatriz.SetValue("Sales_Month_36", lastRowIndex, (p_oRecord.Sales_Month_36 >= 0) ? p_oRecord.Sales_Month_36.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("Hits_Month_36", lastRowIndex, (p_oRecord.Hits_Month_36 >= 0) ? p_oRecord.Hits_Month_36.ToString() : string.Empty);//
                p_dtMatriz.SetValue("LostSales_Month_36", lastRowIndex, (p_oRecord.LostSales_Month_36 >= 0) ? p_oRecord.LostSales_Month_36.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("LostHits_Month_36", lastRowIndex, (p_oRecord.LostHits_Month_36 >= 0) ? p_oRecord.LostHits_Month_36.ToString() : string.Empty);//
                //***** Total 1 a 12 ***
                p_dtMatriz.SetValue("TotalSales_1To12", lastRowIndex, (p_oRecord.TotalSales_1To12 >= 0) ? p_oRecord.TotalSales_1To12.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalHits_1To12", lastRowIndex, (p_oRecord.TotalHits_1To12 >= 0) ? p_oRecord.TotalHits_1To12.ToString() : string.Empty);//
                p_dtMatriz.SetValue("TotalLostSales_1To12", lastRowIndex, (p_oRecord.TotalLostSales_1To12 >= 0) ? p_oRecord.TotalLostSales_1To12.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalLostHits_1To12", lastRowIndex, (p_oRecord.TotalLostHits_1To12 >= 0) ? p_oRecord.TotalLostHits_1To12.ToString() : string.Empty);//
                //***** Total 13 a 24 ***
                p_dtMatriz.SetValue("TotalSales_13To24", lastRowIndex, (p_oRecord.TotalSales_13To24 >= 0) ? p_oRecord.TotalSales_13To24.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalHits_13To24", lastRowIndex, (p_oRecord.TotalHits_13To24 >= 0) ? p_oRecord.TotalHits_13To24.ToString() : string.Empty);//
                p_dtMatriz.SetValue("TotalLostSales_13To24", lastRowIndex, (p_oRecord.TotalLostSales_13To24 >= 0) ? p_oRecord.TotalLostSales_13To24.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalLostHits_13To24", lastRowIndex, (p_oRecord.TotalLostHits_13To24 >= 0) ? p_oRecord.TotalLostHits_13To24.ToString() : string.Empty);//
                //***** Total 25 a 36 ***
                p_dtMatriz.SetValue("TotalSales_25To36", lastRowIndex, (p_oRecord.TotalSales_25To36 >= 0) ? p_oRecord.TotalSales_25To36.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalHits_25To36", lastRowIndex, (p_oRecord.TotalHits_25To36 >= 0) ? p_oRecord.TotalHits_25To36.ToString() : string.Empty);//
                p_dtMatriz.SetValue("TotalLostSales_25To36", lastRowIndex, (p_oRecord.TotalLostSales_25To36 >= 0) ? p_oRecord.TotalLostSales_25To36.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalLostHits_25To36", lastRowIndex, (p_oRecord.TotalLostHits_25To36 >= 0) ? p_oRecord.TotalLostHits_25To36.ToString() : string.Empty);//
                //***** Total 37 a 48 ***
                p_dtMatriz.SetValue("TotalSales_37To48", lastRowIndex, (p_oRecord.TotalSales_37To48 >= 0) ? p_oRecord.TotalSales_37To48.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalHits_37To48", lastRowIndex, (p_oRecord.TotalHits_37To48 >= 0) ? p_oRecord.TotalHits_37To48.ToString() : string.Empty);//
                p_dtMatriz.SetValue("TotalLostSales_37To48", lastRowIndex, (p_oRecord.TotalLostSales_37To48 >= 0) ? p_oRecord.TotalLostSales_37To48.ToString("N2") : string.Empty);//
                p_dtMatriz.SetValue("TotalLostHits_37To48", lastRowIndex, (p_oRecord.TotalLostHits_37To48 >= 0) ? p_oRecord.TotalLostHits_37To48.ToString() : string.Empty);//
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
        //public void DataLinkToMatrix(ref List<DetailJDPRISM> p_oRecordList, ref List<oArticulo> p_oArticulos)
        //{
        //    DetailJDPRISM oRecord;
        //    try
        //    {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            DataGridViewColumn col = new DataGridViewTextBoxColumn();
        //            col.HeaderText = "Hi there " + i.ToString();
        //            int colIndex = dataGridView1.Columns.Add(col);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DMS_Connector.Helpers.ManejoErrores(ex);
        //    }
        //}

        public void CalculateDate(ref DateTime p_fromDate, ref DateTime p_toDate,  Int32 p_intMonthAgo)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime month = new DateTime(today.Year, today.Month, 1);
                p_fromDate = month.AddMonths(- p_intMonthAgo);
                DateTime tempDate = p_fromDate.AddMonths(1);
                p_toDate = tempDate.AddDays(-1);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public static decimal MonthDifference(DateTime p_EdnDate, DateTime p_StartDate)
        {
            return Math.Abs((p_EdnDate.Month - p_StartDate.Month) + 12 * (p_EdnDate.Year - p_StartDate.Year));

        }
        #endregion 
    }
}
