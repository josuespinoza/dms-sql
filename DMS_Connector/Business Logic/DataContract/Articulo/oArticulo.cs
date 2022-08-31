using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Articulo
{
   public class oArticulo
    {
        public string ItemCode { get; set; } //PrimaryKey not null

        public string ItemName { get; set; }

        public string ForeignName { get; set; }

        public int? ItemsGroupCode { get; set; }

        public int? CustomsGroupCode { get; set; }

        public string SalesVATGroup { get; set; }

        public string BarCode { get; set; }

        public string VatLiable { get; set; }

        public string PurchaseItem { get; set; }

        public string SalesItem { get; set; }

        public string InventoryItem { get; set; }

        public string IncomeAccount { get; set; }

        public string ExemptIncomeAccount { get; set; }

        public string ExpanseAccount { get; set; }

        public string Mainsupplier { get; set; }

        public string SupplierCatalogNo { get; set; }

        public double? DesiredInventory { get; set; }

        public double MinInventory { get; set; }

        public string Picture { get; set; }

        public string User_Text { get; set; }

        public string SerialNum { get; set; }

        public double? CommissionPercent { get; set; }

        public double? CommissionSum { get; set; }

        public int? CommissionGroup { get; set; }

        public string TreeType { get; set; }

        public string AssetItem { get; set; }

        public string DataExportCode { get; set; }

        public int? Manufacturer { get; set; }

        public double? QuantityOnStock { get; set; }

        public double? QuantityOrderedFromVendors { get; set; }

        public double? QuantityOrderedByCustomers { get; set; }

        public string ManageSerialNumbers { get; set; }

        public string ManageBatchNumbers { get; set; }

        public string Valid { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public string ValidRemarks { get; set; }

        public string Frozen { get; set; }

        public DateTime? FrozenFrom { get; set; }

        public DateTime? FrozenTo { get; set; }

        public string FrozenRemarks { get; set; }

        public string SalesUnit { get; set; }

        public double? SalesItemsPerUnit { get; set; }

        public string SalesPackagingUnit { get; set; }

        public double? SalesQtyPerPackUnit { get; set; }

        public double? SalesUnitLength { get; set; }

        public int? SalesLengthUnit { get; set; }

        public double? SalesUnitWidth { get; set; }

        public int? SalesWidthUnit { get; set; }

        public double? SalesUnitHeight { get; set; }

        public int? SalesHeightUnit { get; set; }

        public double? SalesUnitVolume { get; set; }

        public int? SalesVolumeUnit { get; set; }

        public double? SalesUnitWeight { get; set; }

        public int? SalesWeightUnit { get; set; }

        public string PurchaseUnit { get; set; }

        public double? PurchaseItemsPerUnit { get; set; }

        public string PurchasePackagingUnit { get; set; }

        public double? PurchaseQtyPerPackUnit { get; set; }

        public double? PurchaseUnitLength { get; set; }

        public int? PurchaseLengthUnit { get; set; }

        public double? PurchaseUnitWidth { get; set; }

        public int? PurchaseWidthUnit { get; set; }

        public double? PurchaseUnitHeight { get; set; }

        public int? PurchaseHeightUnit { get; set; }

        public double? PurchaseUnitVolume { get; set; }

        public int? PurchaseVolumeUnit { get; set; }

        public double? PurchaseUnitWeight { get; set; }

        public int? PurchaseWeightUnit { get; set; }

        public string PurchaseVATGroup { get; set; }

        public double? SalesFactor1 { get; set; }

        public double? SalesFactor2 { get; set; }

        public double? SalesFactor3 { get; set; }

        public double? SalesFactor4 { get; set; }

        public double? PurchaseFactor1 { get; set; }

        public double? PurchaseFactor2 { get; set; }

        public double? PurchaseFactor3 { get; set; }

        public double? PurchaseFactor4 { get; set; }

        public double? MovingAveragePrice { get; set; }

        public string ForeignRevenuesAccount { get; set; }

        public string ECRevenuesAccount { get; set; }

        public string ForeignExpensesAccount { get; set; }

        public string ECExpensesAccount { get; set; }

        public double? AvgStdPrice { get; set; }

        public string DefaultWarehouse { get; set; }

        public int? ShipType { get; set; }

        public string GLMethod { get; set; }

        public string TaxType { get; set; }

        public double MaxInventory { get; set; }

        public string ManageStockByWarehouse { get; set; }

        public int? PurchaseHeightUnit1 { get; set; }

        public double? PurchaseUnitHeight1 { get; set; }

        public int? PurchaseLengthUnit1 { get; set; }

        public double? PurchaseUnitLength1 { get; set; }

        public int? PurchaseWeightUnit1 { get; set; }

        public double? PurchaseUnitWeight1 { get; set; }

        public int? PurchaseWidthUnit1 { get; set; }

        public double? PurchaseUnitWidth1 { get; set; }

        public int? SalesHeightUnit1 { get; set; }

        public double? SalesUnitHeight1 { get; set; }

        public int? SalesLengthUnit1 { get; set; }

        public double? SalesUnitLength1 { get; set; }

        public int? SalesWeightUnit1 { get; set; }

        public double? SalesUnitWeight1 { get; set; }

        public int? SalesWidthUnit1 { get; set; }

        public double? SalesUnitWidth1 { get; set; }

        public string ForceSelectionOfSerialNumber { get; set; }

        public string ManageSerialNumbersOnReleaseOnly { get; set; }

        public string WTLiable { get; set; }

        public string CostAccountingMethod { get; set; }

        public string SWW { get; set; }

        public string WarrantyTemplate { get; set; }

        public string IndirectTax { get; set; }

        public string ArTaxCode { get; set; }

        public string ApTaxCode { get; set; }

        public string BaseUnitName { get; set; }

        public string ItemCountryOrg { get; set; }

        public string IssueMethod { get; set; }

        public string SRIAndBatchManageMethod { get; set; }

        public string IsPhantom { get; set; }

        public string InventoryUOM { get; set; }

        public string PlanningSystem { get; set; }

        public string ProcurementMethod { get; set; }

        public string ComponentWarehouse { get; set; }

        public int? OrderIntervals { get; set; }

        public double? OrderMultiple { get; set; }

        public int? LeadTime { get; set; }

        public double? MinOrderQuantity { get; set; }

        public string ItemType { get; set; }

        public string ItemClass { get; set; }

        public int? OutgoingServiceCode { get; set; }

        public int? IncomingServiceCode { get; set; }

        public int? ServiceGroup { get; set; }

        public int? NCMCode { get; set; }

        public string MaterialType { get; set; }

        public int? MaterialGroup { get; set; }

        public string ProductSource { get; set; }

        public string Properties1 { get; set; }

        public string Properties2 { get; set; }

        public string Properties3 { get; set; }

        public string Properties4 { get; set; }

        public string Properties5 { get; set; }

        public string Properties6 { get; set; }

        public string Properties7 { get; set; }

        public string Properties8 { get; set; }

        public string Properties9 { get; set; }

        public string Properties10 { get; set; }

        public string Properties11 { get; set; }

        public string Properties12 { get; set; }

        public string Properties13 { get; set; }

        public string Properties14 { get; set; }

        public string Properties15 { get; set; }

        public string Properties16 { get; set; }

        public string Properties17 { get; set; }

        public string Properties18 { get; set; }

        public string Properties19 { get; set; }

        public string Properties20 { get; set; }

        public string Properties21 { get; set; }

        public string Properties22 { get; set; }

        public string Properties23 { get; set; }

        public string Properties24 { get; set; }

        public string Properties25 { get; set; }

        public string Properties26 { get; set; }

        public string Properties27 { get; set; }

        public string Properties28 { get; set; }

        public string Properties29 { get; set; }

        public string Properties30 { get; set; }

        public string Properties31 { get; set; }

        public string Properties32 { get; set; }

        public string Properties33 { get; set; }

        public string Properties34 { get; set; }

        public string Properties35 { get; set; }

        public string Properties36 { get; set; }

        public string Properties37 { get; set; }

        public string Properties38 { get; set; }

        public string Properties39 { get; set; }

        public string Properties40 { get; set; }

        public string Properties41 { get; set; }

        public string Properties42 { get; set; }

        public string Properties43 { get; set; }

        public string Properties44 { get; set; }

        public string Properties45 { get; set; }

        public string Properties46 { get; set; }

        public string Properties47 { get; set; }

        public string Properties48 { get; set; }

        public string Properties49 { get; set; }

        public string Properties50 { get; set; }

        public string Properties51 { get; set; }

        public string Properties52 { get; set; }

        public string Properties53 { get; set; }

        public string Properties54 { get; set; }

        public string Properties55 { get; set; }

        public string Properties56 { get; set; }

        public string Properties57 { get; set; }

        public string Properties58 { get; set; }

        public string Properties59 { get; set; }

        public string Properties60 { get; set; }

        public string Properties61 { get; set; }

        public string Properties62 { get; set; }

        public string Properties63 { get; set; }

        public string Properties64 { get; set; }

        public string AutoCreateSerialNumbersOnRelease { get; set; }

        public int? DNFEntry { get; set; }

        public string GTSItemSpec { get; set; }

        public string GTSItemTaxCategory { get; set; }

        public int? FuelID { get; set; }

        public string BeverageTableCode { get; set; }

        public string BeverageGroupCode { get; set; }

        public int? BeverageCommercialBrandCode { get; set; }

        public int? Series { get; set; }

        public int? ToleranceDays { get; set; }

        public string TypeOfAdvancedRules { get; set; }

        public string IssuePrimarilyBy { get; set; }

        public string NoDiscounts { get; set; }

        public string AssetClass { get; set; }

        public string AssetGroup { get; set; }

        public string InventoryNumber { get; set; }

        public int? Technician { get; set; }

        public int? Employee { get; set; }

        public int? Location { get; set; }

        public string AssetStatus { get; set; }

        public DateTime? CapitalizationDate { get; set; }

        public string StatisticalAsset { get; set; }

        public string Cession { get; set; }

        public string DeactivateAfterUsefulLife { get; set; }

        public string ManageByQuantity { get; set; }

        public int? UoMGroupEntry { get; set; }

        public int? InventoryUoMEntry { get; set; }

        public int? DefaultSalesUoMEntry { get; set; }

        public int? DefaultPurchasingUoMEntry { get; set; }

        public string DepreciationGroup { get; set; }

        public string AssetSerialNumber { get; set; }

        public double? InventoryWeight { get; set; }

        public int? InventoryWeightUnit { get; set; }

        public double? InventoryWeight1 { get; set; }

        public int? InventoryWeightUnit1 { get; set; }

        public string DefaultCountingUnit { get; set; }

        public double? CountingItemsPerUnit { get; set; }

        public int? DefaultCountingUoMEntry { get; set; }

        public string Excisable { get; set; }

        public int? ChapterID { get; set; }

        public string ScsCode { get; set; }

        public string SpProdType { get; set; }

        public double? ProdStdCost { get; set; }

        public string InCostRollup { get; set; }

        public string VirtualAssetItem { get; set; }

        public string EnforceAssetSerialNumbers { get; set; }

        public int? AttachmentEntry { get; set; }

        public string LinkedResource { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string GSTRelevnt { get; set; }

        public int? SACEntry { get; set; }

        public string GSTTaxCategory { get; set; }

        public int? ServiceCategoryEntry { get; set; }

        public double? CapitalGoodsOnHoldPercent { get; set; }

        public double? CapitalGoodsOnHoldLimit { get; set; }

        public double? AssessableValue { get; set; }

        public double? AssVal4WTR { get; set; }

        public string SOIExcisable { get; set; }

        public string TNVED { get; set; }

        public int? PricingUnit { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? CreateTime { get; set; }

        public double NumInSale { get; set; }
        public double AvgPrice { get; set; }

        public string U_SCGD_Location { get; set; }

        public string U_SCGD_Brand { get; set; }

        public string U_SCGD_NoFabrica { get; set; }

        public string U_SCGD_Pais { get; set; }

        public int? U_SCGD_Duracion { get; set; }

        public string U_SCGD_T_Fase { get; set; }

        public string U_SCGD_NomFase { get; set; }

        public string U_SCGD_TipoArticulo { get; set; }

        public int? U_SCGD_Generico { get; set; }

        public string U_SCGD_MODELO { get; set; }

        public string U_SCGD_ANIO { get; set; }

        public string U_SCGD_CodCtroCosto { get; set; }

        public double? U_SCGD_DrcionUndTmpo { get; set; }

        public int? U_SCGD_OrdCy { get; set; }

        public int? U_SCGD_SSDem { get; set; }

        public string U_SCGD_ReplCod { get; set; }

        public string U_SCGD_COD_UBIC { get; set; }

        public string U_SCGD_fechaSync { get; set; }

        public string U_SCGD_CatgrServicio { get; set; }

        public string U_SCGD_Fam { get; set; }
        public string U_SCGD_CadCod { get; set; }

        public string U_SCGD_TrFc { get; set; }

        public double? U_SCGD_PorcSE { get; set; }

        public string U_SCGD_RRep { get; set; }

        public string U_Prueba1 { get; set; }

        //public List<ItemPrice> ItemPrices { get; set; } // not null

        //public List<ItemWarehouseInfo> ItemWarehouseInfoCollection { get; set; } // not null

        //public List<ItemPreferredVendor> ItemPreferredVendors { get; set; } // not null

        //public List<ItemLocalizationInfo> ItemLocalizationInfos { get; set; } // not null

        //public List<ItemProject> ItemProjects { get; set; } // not null

        //public List<ItemDistributionRule> ItemDistributionRules { get; set; } // not null

        //public List<ItemAttributeGroups> ItemAttributeGroups { get; set; } // not null

        //public List<ItemDepreciationParameter> ItemDepreciationParameters { get; set; } // not null

        //public List<ItemPeriodControl> ItemPeriodControls { get; set; } // not null

        //public List<ItemUnitOfMeasurement> ItemUnitOfMeasurementCollection { get; set; } // not null

        //public List<ItemBarCode> ItemBarCodeCollection { get; set; } // not null

        //public ItemIntrastatExtension ItemIntrastatExtension { get; set; } 
    }
}
