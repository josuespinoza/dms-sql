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
using DMS_Connector;
using DMS_Connector.Data_Access;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using SCG.Integration.InterfaceDPM.Entities.URecords;
using ICompany = SAPbobsCOM.ICompany;
using RestSharp;

namespace SCG.Integration.InterfaceDPM
{
    public class InterfaceJohnDeere_PMM
    {
        public IApplication oApplicationSBO { get; private set; }
        public ICompany oCompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;

        public SAPbouiCOM.Form oForm { get; set; }

        private static NumberFormatInfo n;
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

        public enum BetweenMonth
        {
            ActualMonth,
            MonthAgo1,
            MonthAgo1To12,
            MonthAgo13To24
        }

        public enum TypeValue
        {
            Cost,
            Price
        }

        public enum TypeURecord
        {
            U0Record,
            UIRecord,
            UJRecord,
            UKRecord,
            ULRecord,
            UMRecord,
            UNRecord,
            UORecord,
            UPRecord,
            UQRecord,
            URRecord,
            USRecord,
            UTRecord
        }
        #endregion
        #region Constructor
        public InterfaceJohnDeere_PMM(IApplication applicationSBO, ICompany companySBO, SAPbouiCOM.Form p_oForm)
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
            }
        }
        #endregion
        #region Metodos
        public void ManejaInterfaceJohnDeere_PMM()
        {
            PMM oPMM;
            InfoWarehouse oWarehouse;
            List<InfoWarehouse> listWarehouse;
            try
            {
                oPMM = new PMM();
                LoadGeneralConfigurationPMM(ref oPMM);
                //****** Crear URecords ******* 
                URecordManager(ref oPMM);

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void URecordManager(ref PMM p_oPMM)
        {
            var sb = new StringBuilder();
            List<oArticulo> oItems;

            try
            {
                oItems = new List<oArticulo>();
                LoadItemList(ref oItems, ref p_oPMM, MonthAgo.MonthAgo1);
                //****** U0Record ******* 
                Load_U0Record(ref sb, ref p_oPMM, ref oItems);
                //****** UIRecord ******* 
                Load_UIRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UJRecord ******* 
                Load_UJRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UKRecord
                Load_UKRecord(ref sb, ref p_oPMM, ref oItems);
                //****** ULRecord
                Load_ULRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UMRecord
                Load_UMRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UNRecord
                Load_UNRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UORecord
                Load_UORecord(ref sb, ref p_oPMM, ref oItems);
                //****** UPRecord
                Load_UPRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UQRecord
                Load_UQRecord(ref sb, ref p_oPMM, ref oItems);
                //****** URRecord
                Load_URRecord(ref sb, ref p_oPMM, ref oItems);
                //****** USRecord
                Load_USRecord(ref sb, ref p_oPMM, ref oItems);
                //****** UTRecord
                Load_UTRecord(ref sb, ref p_oPMM, ref oItems);

                SaveFile(ref p_oPMM, ref sb);
                oApplicationSBO.StatusBar.SetText("Process Complete", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadGeneralConfigurationPMM(ref PMM p_PMM)
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
                        p_PMM.FirmCode = !string.IsNullOrEmpty(dsInformation.GetValue("U_FCode", index)) ? dsInformation.GetValue("U_FCode", index).ToString().Trim() : string.Empty;
                        p_PMM.U_FirstMainAcc = !string.IsNullOrEmpty(dsInformation.GetValue("U_FirstMainAcc", index)) ? dsInformation.GetValue("U_FirstMainAcc", index).ToString().Trim() : string.Empty;
                        p_PMM.U_FirstSourceAcc = !string.IsNullOrEmpty(dsInformation.GetValue("U_FirstSourceAcc", index)) ? dsInformation.GetValue("U_FirstSourceAcc", index).ToString().Trim() : string.Empty;
                        p_PMM.U_LastMainAcc = !string.IsNullOrEmpty(dsInformation.GetValue("U_LastMainAcc", index)) ? dsInformation.GetValue("U_LastMainAcc", index).ToString().Trim() : string.Empty;
                        p_PMM.U_LastSourceAcc = !string.IsNullOrEmpty(dsInformation.GetValue("U_LastSourceAcc", index)) ? dsInformation.GetValue("U_LastSourceAcc", index).ToString().Trim() : string.Empty;
                        p_PMM.U_PMMVer = !string.IsNullOrEmpty(dsInformation.GetValue("U_PMMVer", index)) ? dsInformation.GetValue("U_PMMVer", index).ToString().Trim() : string.Empty;
                        p_PMM.U_CriticalCode = !string.IsNullOrEmpty(dsInformation.GetValue("U_CriticalCode", index)) ? dsInformation.GetValue("U_CriticalCode", index).ToString().Trim() : string.Empty;
                        p_PMM.U_InvenClass = !string.IsNullOrEmpty(dsInformation.GetValue("U_InvenClass", index)) ? dsInformation.GetValue("U_InvenClass", index).ToString().Trim() : string.Empty;
                        p_PMM.Path = !string.IsNullOrEmpty(dsInformation.GetValue("U_Path", index)) ? dsInformation.GetValue("U_Path", index).ToString().Trim() : string.Empty;
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
                        infoWarehouse.WhsCode = !string.IsNullOrEmpty(dsInformation.GetValue("U_WhsCod", index)) ? dsInformation.GetValue("U_WhsCod", index).ToString().Trim() : string.Empty;
                        infoWarehouse.WhsProcess = !string.IsNullOrEmpty(dsInformation.GetValue("U_WhsPro", index)) ? dsInformation.GetValue("U_WhsPro", index).ToString().Trim() : string.Empty;
                        listWarehouse.Add(infoWarehouse);
                    }
                    p_PMM.infoWarehouse = listWarehouse;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_U0Record(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            U0Record oU0Record;
            URecordInfo oURecordInfo;
            DateTime dateFile;
            try
            {
                oURecordInfo = new URecordInfo();

                oURecordInfo.SalesCounter = 0;
                oURecordInfo.SalesShop = 0;
                oURecordInfo.SalesInternal = 0;
                oURecordInfo.ReturnCounter = 0;
                oURecordInfo.ReturnShop = 0;

                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoice(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth);
                    
                }
                

                oU0Record = new U0Record();

                oU0Record.RecordCode = "U";
                oU0Record.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oU0Record.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oU0Record.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oU0Record.IDRecord = "0";
                oU0Record.InterfaceVersion = p_oPMM.U_PMMVer;
                dateFile = DateTime.Now ;
                oU0Record.Date = dateFile.ToString("yyyyMM");
                oU0Record.TypeRecord = "P";
                oU0Record.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;
                oU0Record.SalesCounter = oURecordInfo.SalesCounter.ToString("D9");
                oU0Record.SalesShop = oURecordInfo.SalesShop.ToString("D9");
                oU0Record.SalesInternal = oURecordInfo.SalesInternal.ToString("D9");
                oU0Record.ReturnCounter = oURecordInfo.ReturnCounter.ToString("D9");
                oU0Record.ReturnShop = oURecordInfo.ReturnShop.ToString("D9");
                oU0Record.Warehouse = "A1";
                oU0Record.WarehouseType = "M";
                oU0Record.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UIRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UIRecord oUIRecord;
            URecordInfo oURecordInfo;
            DateTime dateFile;
            try
            {
                oURecordInfo = new URecordInfo();

                oURecordInfo.ReturnInternal = 0;

                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    //LoadInfoInvoice(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth);

                }

                oUIRecord = new UIRecord();

                oUIRecord.RecordCode = "U";
                oUIRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUIRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUIRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUIRecord.IDRecord = "0";
                oUIRecord.TypeRecord = "P";
                oUIRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUIRecord.ReturnInternal = oURecordInfo.ReturnInternal.ToString("D9");

                oUIRecord.Warehouse = "A1";
                oUIRecord.WarehouseType = "M";
                oUIRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UJRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UJRecord oUJRecord;
            URecordInfo oURecordInfo;
            Double dblBalance=0;
            String strItemCode = String.Empty;
            DateTime fromDate;
            DateTime toDate;
            Int32 intAverageMonthlyInventoryLast12=0;
            Int32 intAverageMonthlyInventoryLast13to24 = 0;
            try
            {
                oURecordInfo = new URecordInfo();
                oURecordInfo.TotalPartsSalesLast12 = 0;
                oURecordInfo.TotalPartsSalesLast13to24 = 0;
                oURecordInfo.TotalPartsSalesMonth = 0;

                //*** Load 1 to 12 
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 12);
                CalculateToDateByMonth(ref toDate, 1);

                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    strItemCode=oItem.ItemCode;

                    AuditStock(ref strItemCode, ref dblBalance, ref fromDate, ref toDate);
                }

                intAverageMonthlyInventoryLast12 = Convert.ToInt32(dblBalance);
                //*** Load 13 to 24
                dblBalance = 0;
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 24);
                CalculateToDateByMonth(ref toDate, 13);

                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    strItemCode = oItem.ItemCode;

                    AuditStock(ref strItemCode, ref dblBalance, ref fromDate, ref toDate);
                }

                intAverageMonthlyInventoryLast13to24 = Convert.ToInt32(dblBalance);

                //** Load Invoice 1 To 12
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 12);
                CalculateToDateByMonth(ref toDate, 1);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate,BetweenMonth.MonthAgo1To12,TypeValue.Price);

                }
                //** Load Invoice 13 To 24
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 24);
                CalculateToDateByMonth(ref toDate, 13);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate, BetweenMonth.MonthAgo13To24, TypeValue.Price);

                }
                //** Load Invoice Last Month
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 1);
                CalculateToDateByMonth(ref toDate, 1);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate, BetweenMonth.MonthAgo1, TypeValue.Price);

                }
                oUJRecord = new UJRecord();

                oUJRecord.RecordCode = "U";
                oUJRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUJRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUJRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUJRecord.IDRecord = "0";
                oUJRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUJRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUJRecord.TypeRecord = "P";
                oUJRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUJRecord.AverageMonthlyInventoryLast12 = intAverageMonthlyInventoryLast12.ToString("D9");
                oUJRecord.AverageMonthlyInventoryLast13to24 = intAverageMonthlyInventoryLast13to24.ToString("D9");
                oUJRecord.TotalPartsSalesLast12 = oURecordInfo.TotalPartsSalesLast12.ToString("D9");
                oUJRecord.TotalPartsSalesLast13to24 = oURecordInfo.TotalPartsSalesLast13to24.ToString("D9");
                oUJRecord.TotalPartsSalesMonth = oURecordInfo.TotalPartsSalesMonth.ToString("D9");

                oUJRecord.Warehouse = "A1";
                oUJRecord.WarehouseType = "M";
                oUJRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UKRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UKRecord oUKRecord;
            URecordInfo oURecordInfo;
            Double dblBalance = 0;
            String strItemCode = String.Empty;
            DateTime fromDate;
            DateTime toDate;
            Int32 intCurrentInventory = 0;
            try
            {
                oURecordInfo = new URecordInfo();
                oURecordInfo.TotalPartsCostLast12 = 0;
                oURecordInfo.TotalPartsCostLast13to24 = 0;
                oURecordInfo.TotalCostMonth = 0;

                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    strItemCode = oItem.ItemCode;

                    AuditStockBalance(ref strItemCode, ref dblBalance);
                }

                intCurrentInventory = Convert.ToInt32(dblBalance);

                //** Load Invoice 1 To 12
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 12);
                CalculateToDateByMonth(ref toDate, 1);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate, BetweenMonth.MonthAgo1To12, TypeValue.Cost);

                }
                //** Load Invoice 13 To 24
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 24);
                CalculateToDateByMonth(ref toDate, 13);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate, BetweenMonth.MonthAgo13To24, TypeValue.Cost);

                }
                //** Load Invoice Last Month
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
                CalculateFromDateByMonth(ref fromDate, 1);
                CalculateToDateByMonth(ref toDate, 1);
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceBetweenDate(ref oURecordInfo, ref p_oPMM, ref fromDate, ref toDate, BetweenMonth.MonthAgo1, TypeValue.Cost);

                }
                oUKRecord = new UKRecord();

                oUKRecord.RecordCode = "U";
                oUKRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUKRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUKRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUKRecord.IDRecord = "0";
                oUKRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUKRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUKRecord.TypeRecord = "P";
                oUKRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUKRecord.TotalPartsCostLast12 = oURecordInfo.TotalPartsCostLast12.ToString("D9");
                oUKRecord.TotalPartsCostLast13to24 = oURecordInfo.TotalPartsCostLast13to24.ToString("D9");
                oUKRecord.TotalCostMonth = oURecordInfo.TotalCostMonth.ToString("D9");
                oUKRecord.CurrentInventory = intCurrentInventory.ToString("D9");
                oUKRecord.NoSalesInventory = 0.ToString("D9");

                oUKRecord.Warehouse = "A1";
                oUKRecord.WarehouseType = "M";
                oUKRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_ULRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            ULRecord oULRecord;
            URecordInfo oURecordInfo;
            Double dblBalance = 0;
            String strItemCode = String.Empty;
            DateTime fromDate;
            DateTime toDate;
            Int32 intCurrentInventory = 0;
            try
            {
                oURecordInfo = new URecordInfo();
                oURecordInfo.CounterStockedTotalHits = 0;
                oURecordInfo.CounterStockedHits1Pass = 0;
                oURecordInfo.CounterStockedHitsLostSales = 0;
                //** Load Invoice Actual Month
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth,TypeURecord.ULRecord );

                }

                //** Load Quotation Actual Month
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoQuotationCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.ULRecord);

                }
                oULRecord = new ULRecord();

                oULRecord.RecordCode = "U";
                oULRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oULRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oULRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oULRecord.IDRecord = "0";
                oULRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oULRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oULRecord.TypeRecord = "P";
                oULRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oULRecord.CounterStockedTotalHits = oURecordInfo.CounterStockedTotalHits.ToString("D5");
                oULRecord.CounterStockedHits1Pass = oURecordInfo.CounterStockedHits1Pass.ToString("D5");
                oULRecord.CounterStockedHitsLostSales = oURecordInfo.CounterStockedHitsLostSales.ToString("D5");

                oULRecord.Warehouse = "A1";
                oULRecord.WarehouseType = "M";
                oULRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UMRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UMRecord oUMRecord;
            URecordInfo oURecordInfo;   
            String strItemCode = String.Empty;
            DateTime fromDate;
            DateTime toDate;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                //** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth);

                //}
                oUMRecord = new UMRecord();

                oUMRecord.RecordCode = "U";
                oUMRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUMRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUMRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUMRecord.IDRecord = "0";
                oUMRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUMRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUMRecord.TypeRecord = "P";
                oUMRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUMRecord.CounterNonStockedTotalHits = 0.ToString("D5");
                oUMRecord.CounterNonStocked1Pass = 0.ToString("D5");
                oUMRecord.CounterNonStockedLostSales = 0.ToString("D5");

                oUMRecord.Warehouse = "A1";
                oUMRecord.WarehouseType = "M";
                oUMRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UNRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UNRecord oUNRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                oURecordInfo.CounterStockedTotalHits = 0;
                oURecordInfo.ShopStocked1Pass = 0;
                //** Load Invoice Actual Month
                foreach (var oItem in p_oItems)
                {
                    oURecordInfo.ItemCode = oItem.ItemCode;
                    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth,TypeURecord.UNRecord);

                }
                oUNRecord = new UNRecord();

                oUNRecord.RecordCode = "U";
                oUNRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUNRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUNRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUNRecord.IDRecord = "0";
                oUNRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUNRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUNRecord.TypeRecord = "P";
                oUNRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUNRecord.ShopStockedTotalHits = oURecordInfo.ShopStockedTotalHits.ToString("D5");
                oUNRecord.ShopStocked1Pass = oURecordInfo.ShopStocked1Pass.ToString("D5");
                oUNRecord.ShopStockedLostSales = 0.ToString("D5");

                oUNRecord.Warehouse = "A1";
                oUNRecord.WarehouseType = "M";
                oUNRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UORecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UORecord oUORecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oUORecord = new UORecord();

                oUORecord.RecordCode = "U";
                oUORecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUORecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUORecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUORecord.IDRecord = "0";
                oUORecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUORecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUORecord.TypeRecord = "P";
                oUORecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUORecord.ShopNonStockedTotalHits = 0.ToString("D5");
                oUORecord.ShopNonStocked1Pass = 0.ToString("D5");
                oUORecord.ShopNonStockedLostSales = 0.ToString("D5");

                oUORecord.Warehouse = "A1";
                oUORecord.WarehouseType = "M";
                oUORecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UPRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UPRecord oUPRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oUPRecord = new UPRecord();

                oUPRecord.RecordCode = "U";
                oUPRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUPRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUPRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUPRecord.IDRecord = "0";
                oUPRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUPRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUPRecord.TypeRecord = "P";
                oUPRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUPRecord.InternalStockedTotalHits = 0.ToString("D5");
                oUPRecord.InternalStocked1Pass = 0.ToString("D5");

                oUPRecord.Warehouse = "A1";
                oUPRecord.WarehouseType = "M";
                oUPRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UQRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UQRecord oUQRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oUQRecord = new UQRecord();

                oUQRecord.RecordCode = "U";
                oUQRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUQRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUQRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUQRecord.IDRecord = "0";
                oUQRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUQRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUQRecord.TypeRecord = "P";
                oUQRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUQRecord.InternalStockedLostSales = 0.ToString("D5");

                oUQRecord.Warehouse = "A1";
                oUQRecord.WarehouseType = "M";
                oUQRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_URRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            URRecord oURRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oURRecord = new URRecord();

                oURRecord.RecordCode = "U";
                oURRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oURRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oURRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oURRecord.IDRecord = "0";
                oURRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oURRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oURRecord.TypeRecord = "P";
                oURRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oURRecord.InternalNonStockedTotalHits = 0.ToString("D5");
                oURRecord.InternalNonStocked1Pass = 0.ToString("D5");
                oURRecord.InternalNonStockedLostSales = 0.ToString("D5");

                oURRecord.Warehouse = "A1"; 
                oURRecord.WarehouseType = "M";
                oURRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_USRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            USRecord oUSRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oUSRecord = new USRecord();

                oUSRecord.RecordCode = "U";
                oUSRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUSRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUSRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUSRecord.IDRecord = "0";
                oUSRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUSRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUSRecord.TypeRecord = "P";
                oUSRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUSRecord.MTDTotalParts = 0.ToString("D9");

                oUSRecord.Warehouse = "A1";
                oUSRecord.WarehouseType = "M";
                oUSRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void Load_UTRecord(ref StringBuilder p_sb, ref PMM p_oPMM, ref List<oArticulo> p_oItems)
        {
            UTRecord oUTRecord;
            URecordInfo oURecordInfo;
            String strItemCode = String.Empty;
            try
            {
                oURecordInfo = new URecordInfo();
                //oURecordInfo.CounterStockedTotalHits = 0;
                ////** Load Invoice Actual Month
                //foreach (var oItem in p_oItems)
                //{
                //    oURecordInfo.ItemCode = oItem.ItemCode;
                //    LoadInfoInvoiceCounter(ref oURecordInfo, ref p_oPMM, MonthAgo.ActualMonth, TypeURecord.UNRecord);

                //}
                oUTRecord = new UTRecord();

                oUTRecord.RecordCode = "U";
                oUTRecord.MainAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstMainAcc) ? p_oPMM.U_FirstMainAcc.ToString().Trim() : string.Empty;
                oUTRecord.SourceAccount1_2 = !string.IsNullOrEmpty(p_oPMM.U_FirstSourceAcc) ? p_oPMM.U_FirstSourceAcc.ToString().Trim() : string.Empty;
                oUTRecord.MainAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastMainAcc) ? p_oPMM.U_LastMainAcc.ToString().Trim() : string.Empty;
                oUTRecord.IDRecord = "0";
                oUTRecord.CriticalCode = !string.IsNullOrEmpty(p_oPMM.U_CriticalCode) ? p_oPMM.U_CriticalCode.ToString().Trim() : string.Empty;
                oUTRecord.InventoryClass = !string.IsNullOrEmpty(p_oPMM.U_InvenClass) ? p_oPMM.U_InvenClass.ToString().Trim() : string.Empty;
                oUTRecord.TypeRecord = "P";
                oUTRecord.SourceAccount3_6 = !string.IsNullOrEmpty(p_oPMM.U_LastSourceAcc) ? p_oPMM.U_LastSourceAcc.ToString().Trim() : string.Empty;

                oUTRecord.Warehouse = "A1";
                oUTRecord.WarehouseType = "M";
                oUTRecord.ToString(ref p_sb);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void AuditStock(ref String p_ItemCode, ref Double p_dblBalance, ref DateTime p_FromDate, ref DateTime p_toDate)
        {
            System.Data.DataTable dtAuditStock = default(System.Data.DataTable);
            Double dblInStock = 0;
            Double dblOutStock = 0;
            Double dblBalance = 0;
            Double dblBalanceTemp = 0;
            int countLine;
            try
            {
                countLine = 0;
                dblInStock = 0;
                dblOutStock = 0;
                dblBalanceTemp = 0;
                dtAuditStock = DMS_Connector.Helpers.EjecutarConsultaDataTable(string.Format("Select ItemCode, InQty,OutQty, Price,TransValue,Balance, DocDate From OINM  where ItemCode= '{0}' AND DocDate between '{1}' AND '{2}'", p_ItemCode, p_FromDate.ToString("yyyyMMdd"), p_toDate.ToString("yyyyMMdd")));
                foreach (DataRow rowAudit in dtAuditStock.Rows)
                {
                    if (countLine == 0)
                    {
                        dblBalanceTemp = double.Parse(rowAudit["Balance"].ToString());
                        countLine += 1;
                    }
                    else
                    {
                        dblInStock += double.Parse(rowAudit["InQty"].ToString());
                        dblOutStock = double.Parse(rowAudit["OutQty"].ToString());
                        dblBalanceTemp += double.Parse(rowAudit["TransValue"].ToString());
                    }
                }
                p_dblBalance += (dblBalanceTemp / 12);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void AuditStockBalance(ref String p_ItemCode, ref Double p_dblBalance)
        {
            System.Data.DataTable dtAuditStock = default(System.Data.DataTable);
            try
            {
                dtAuditStock = DMS_Connector.Helpers.EjecutarConsultaDataTable(string.Format("Select Balance From OINM  where ItemCode= '{0}' Order By DocDate desc", p_ItemCode));
                foreach (DataRow rowAudit in dtAuditStock.Rows)
                {
                    p_dblBalance += double.Parse(rowAudit["Balance"].ToString());
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadItemList(ref List<oArticulo> p_oItems, ref PMM p_PMM, MonthAgo p_MonthAgo)
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

                    fromDate = DateTime.Now;
                    toDate = DateTime.Today;
                    GetDateFromAndTo(ref fromDate, ref toDate, ref p_MonthAgo);
                    //********** Load Invoices ******
                    oForm.DataSources.DBDataSources.Add("INV1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("INV1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
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

                    //********** Load Saler Orders ******
                    oForm.DataSources.DBDataSources.Add("RDR1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("RDR1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
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
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
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
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
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
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = toDate.ToString("yyyyMMdd");
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
                        oCondition.CondVal = p_PMM.FirmCode;
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


        public void LoadInfoInvoice(ref URecordInfo p_URecordInfo, ref PMM p_PMM, MonthAgo p_MonthAgo)
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
                    oCondition.CondVal = p_URecordInfo.ItemCode;
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

                    foreach (InfoWarehouse row in p_PMM.infoWarehouse)
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
                        strLineStatus = !string.IsNullOrEmpty(dsInformation.GetValue("LineStatus", index)) ? dsInformation.GetValue("LineStatus", index).ToString().Trim() : string.Empty;
                        //*** valida si tiene OT o no
                        if (!string.IsNullOrEmpty(strWO))
                        {
                            //*** Valida si a linea esta abierta
                            if (strLineStatus == "O")
                            {
                                p_URecordInfo.SalesShop += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                            }
                            else if (strLineStatus == "C")
                            {
                                p_URecordInfo.ReturnShop += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                            }
                        }
                        else
                        {
                            //*** Valida si a linea esta abierta
                            if (strLineStatus == "O")
                            {
                                p_URecordInfo.SalesCounter += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                            }
                            else if (strLineStatus == "C")
                            {
                                p_URecordInfo.ReturnCounter += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadInfoInvoiceBetweenDate(ref URecordInfo p_URecordInfo, ref PMM p_PMM, ref DateTime p_fromDate, ref DateTime p_toDate,  BetweenMonth p_BetweenMonth, TypeValue p_TypeValue )
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            int intContador = 0;
            List<String> tempWarehouses;
            String strWarehouse;
            String strDocEntry;
            String strWO;
            try
            {
                if (oForm != null)
                {
                    tempWarehouses = new List<string>();

                    oForm.DataSources.DBDataSources.Add("INV1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("INV1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_URecordInfo.ItemCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "DocDate";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_BETWEEN;
                    oCondition.CondVal = p_fromDate.ToString("yyyyMMdd");
                    oCondition.CondEndVal = p_toDate.ToString("yyyyMMdd");
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    foreach (InfoWarehouse row in p_PMM.infoWarehouse)
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
                        switch (p_BetweenMonth)
                        {
                            case BetweenMonth.ActualMonth:
                                
                                break;
                            case BetweenMonth.MonthAgo1:
                                switch (p_TypeValue)
                                {
                                    case TypeValue.Price:
                                        p_URecordInfo.TotalPartsSalesMonth += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                                        break;
                                    case TypeValue.Cost:
                                        p_URecordInfo.TotalCostMonth += Convert.ToInt32(double.Parse(dsInformation.GetValue("StockPrice", index))) * Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case BetweenMonth.MonthAgo1To12:
                                switch (p_TypeValue)
                                {
                                    case TypeValue.Price:
                                        p_URecordInfo.TotalPartsSalesLast12 += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                                        break;
                                    case TypeValue.Cost:
                                        p_URecordInfo.TotalPartsCostLast12 += Convert.ToInt32(double.Parse(dsInformation.GetValue("StockPrice", index))) * Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case BetweenMonth.MonthAgo13To24:
                                switch (p_TypeValue)
                                {
                                    case TypeValue.Price:
                                        p_URecordInfo.TotalPartsSalesLast13to24 += Convert.ToInt32(double.Parse(dsInformation.GetValue("LineTotal", index)));
                                        break;
                                    case TypeValue.Cost:
                                        p_URecordInfo.TotalPartsCostLast13to24 += Convert.ToInt32(double.Parse(dsInformation.GetValue("StockPrice", index))) * Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
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

        public void LoadInfoInvoiceCounter(ref URecordInfo p_URecordInfo, ref PMM p_PMM, MonthAgo p_MonthAgo, TypeURecord p_TypeURecord)
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
            String strPrimerPasada = string.Empty;
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
                    oCondition.CondVal = p_URecordInfo.ItemCode;
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

                    foreach (InfoWarehouse row in p_PMM.infoWarehouse)
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
                        strLineStatus = !string.IsNullOrEmpty(dsInformation.GetValue("LineStatus", index)) ? dsInformation.GetValue("LineStatus", index).ToString().Trim() : string.Empty;
                        //*** valida si tiene OT o no
                        if (!string.IsNullOrEmpty(strWO))
                        {
                            //*** Valida si a linea esta abierta
                            if (strLineStatus == "O")
                            {
                                switch (p_TypeURecord)
                                {
                                    case TypeURecord.UNRecord:
                                        p_URecordInfo.ShopStockedTotalHits += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        strPrimerPasada = dsInformation.GetValue("U_SCGD_OnePass", index).ToString();
                                        if (strPrimerPasada == "Y")
                                        {
                                            p_URecordInfo.ShopStocked1Pass += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            //*** Valida si a linea esta abierta
                            if (strLineStatus == "O")
                            {
                                switch (p_TypeURecord)
                                {
                                    case TypeURecord.ULRecord:
                                        p_URecordInfo.CounterStockedTotalHits += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        strPrimerPasada = dsInformation.GetValue("U_SCGD_OnePass", index).ToString();
                                        if (strPrimerPasada == "Y")
                                        {
                                            p_URecordInfo.CounterStockedHits1Pass += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LoadInfoQuotationCounter(ref URecordInfo p_URecordInfo, ref PMM p_PMM, MonthAgo p_MonthAgo, TypeURecord p_TypeURecord)
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
            String strVentaPerdida = string.Empty;
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

                    oForm.DataSources.DBDataSources.Add("QUT1");
                    dsInformation = oForm.DataSources.DBDataSources.Item("QUT1");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "ItemCode";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_URecordInfo.ItemCode;
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

                    foreach (InfoWarehouse row in p_PMM.infoWarehouse)
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
                        strLineStatus = !string.IsNullOrEmpty(dsInformation.GetValue("LineStatus", index)) ? dsInformation.GetValue("LineStatus", index).ToString().Trim() : string.Empty;
                        //*** valida si tiene OT o no
                        if (!string.IsNullOrEmpty(strWO))
                        {
                            //*** Valida si a linea esta abierta
                            //if (strLineStatus == "O")
                            //{
                            //    switch (p_TypeURecord)
                            //    {
                            //        case TypeURecord.UNRecord:
                            //            p_URecordInfo.ShopStockedTotalHits += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                            //            break;
                            //        default:
                            //            break;
                            //    }
                            //}
                        }
                        else
                        {
                            //*** Valida si a linea esta abierta
                            if (strLineStatus == "O")
                            {
                                switch (p_TypeURecord)
                                {
                                    case TypeURecord.ULRecord:
                                        strVentaPerdida = dsInformation.GetValue("SCGD_VPerdida", index).ToString();
                                        if (strVentaPerdida == "Y")
                                        {
                                            p_URecordInfo.CounterStockedHitsLostSales += Convert.ToInt32(double.Parse(dsInformation.GetValue("Quantity", index)));
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
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
                        CalculateDate(ref p_fromDate, ref p_toDate, 1);
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

        public void CalculateDate(ref DateTime p_fromDate, ref DateTime p_toDate, Int32 p_intMonthAgo)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime month = new DateTime(today.Year, today.Month, 1);
                p_fromDate = month.AddMonths(-p_intMonthAgo);
                DateTime tempDate = p_fromDate.AddMonths(1);
                p_toDate = tempDate.AddDays(-1);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void CalculateFromDateByMonth(ref DateTime p_fromDate,  Int32 p_intMonthAgo)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime month = new DateTime(today.Year, today.Month, 1);
                p_fromDate = month.AddMonths(-p_intMonthAgo);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void CalculateToDateByMonth(ref DateTime p_toDate, Int32 p_intMonthAgo)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime month = new DateTime(today.Year, today.Month, 1);
                p_toDate = month.AddMonths(-p_intMonthAgo);
                p_toDate = p_toDate.AddMonths(1);
                p_toDate = p_toDate.AddDays(-1);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void SaveFile(ref PMM p_oPMM, ref StringBuilder p_sb)
        {
            String strFileName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(p_oPMM.Path))
                {
                    strFileName = "DLR2JD" + "_" + DateTime.Now.ToString("ddMMMyyyy") + "_" + DateTime.Now.ToString("HHMMss") + ".DAT";
                    p_oPMM.Path += strFileName;
                    System.IO.File.WriteAllText(@p_oPMM.Path, p_sb.ToString());
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
        #endregion
    }
}
