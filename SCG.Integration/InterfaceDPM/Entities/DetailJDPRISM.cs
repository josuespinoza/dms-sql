using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SAPbouiCOM;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class DetailJDPRISM
    {
        public DetailJDPRISM()
        {

        }
        public String RecordCode { get; set; }
        public String PartNumber { get; set; }
        public double AvailableQuantity { get; set; }
        public double OOQuantity { get; set; }
        public double ReserveQ_WO { get; set; }
        public double ReserveQ_PT { get; set; }
        public double CurrrentMTDSales { get; set; }
        public int CurrentMTDHits { get; set; }
        public double CurrentMTDLostSales { get; set; }
        public int CurrentMTDLostHits { get; set; }
        public double DealerPPP { get; set; }
        public String BinLocation { get; set; }
        public String AlternateBinLocation { get; set; }
        public double VendorPartCost { get; set; }
        public int VendorPackageQuantity { get; set; }
        public String VendorCode { get; set; }
        public String VendorSubstitutionInfo { get; set; }
        public String PricingBase { get; set; }
        public double PricingAdditive { get; set; }
        public double DealerPrice { get; set; }
        public String OrderFormulaCode { get; set; }
        public String DeleteIndicator { get; set; }
        public int ReservedHits_WO { get; set; }
        public int ReservedHits_PT { get; set; }
        public double AverageCost { get; set; }
        public String Start_I_Records { get; set; }
        public String PartDescription { get; set; }
        public String DealerPartNote { get; set; }
        public String OrderIndicator { get; set; }
        public DateTime DateAdded { get; set; }
        public String DealerGroupCode { get; set; }
        public double MinOrderQuantity { get; set; }
        public double MaxOrderQuantity { get; set; }
        public int NumberOfMonthlyHistory { get; set; }
        public int PiecesInSet { get; set; }
        //*******************************************
        public double Sales_Month_1 { get; set; }
        public int Hits_Month_1 { get; set; }
        public double LostSales_Month_1 { get; set; }
        public int LostHits_Month_1 { get; set; }
        //*******************************************
        public double Sales_Month_2 { get; set; }
        public int Hits_Month_2 { get; set; }
        public double LostSales_Month_2 { get; set; }
        public int LostHits_Month_2 { get; set; }
        //*******************************************
        public double Sales_Month_3 { get; set; }
        public int Hits_Month_3 { get; set; }
        public double LostSales_Month_3 { get; set; }
        public int LostHits_Month_3 { get; set; }
        //*******************************************
        public double Sales_Month_4 { get; set; }
        public int Hits_Month_4 { get; set; }
        public double LostSales_Month_4 { get; set; }
        public int LostHits_Month_4 { get; set; }
        //*******************************************
        public double Sales_Month_5 { get; set; }
        public int Hits_Month_5 { get; set; }
        public double LostSales_Month_5 { get; set; }
        public int LostHits_Month_5 { get; set; }
        //*******************************************
        public double Sales_Month_6 { get; set; }
        public int Hits_Month_6 { get; set; }
        public double LostSales_Month_6 { get; set; }
        public int LostHits_Month_6 { get; set; }
        //*******************************************
        public double Sales_Month_7 { get; set; }
        public int Hits_Month_7 { get; set; }
        public double LostSales_Month_7 { get; set; }
        public int LostHits_Month_7 { get; set; }
        //*******************************************
        public double Sales_Month_8 { get; set; }
        public int Hits_Month_8 { get; set; }
        public double LostSales_Month_8 { get; set; }
        public int LostHits_Month_8 { get; set; }
        //*******************************************
        public double Sales_Month_9 { get; set; }
        public int Hits_Month_9 { get; set; }
        public double LostSales_Month_9 { get; set; }
        public int LostHits_Month_9 { get; set; }
        //*******************************************
        public double Sales_Month_10 { get; set; }
        public int Hits_Month_10 { get; set; }
        public double LostSales_Month_10 { get; set; }
        public int LostHits_Month_10 { get; set; }
        //*******************************************
        public double Sales_Month_11 { get; set; }
        public int Hits_Month_11 { get; set; }
        public double LostSales_Month_11 { get; set; }
        public int LostHits_Month_11 { get; set; }
        //*******************************************
        public double Sales_Month_12 { get; set; }
        public int Hits_Month_12 { get; set; }
        public double LostSales_Month_12 { get; set; }
        public int LostHits_Month_12 { get; set; }
        //*******************************************
        public double Sales_Month_13 { get; set; }
        public int Hits_Month_13 { get; set; }
        public double LostSales_Month_13 { get; set; }
        public int LostHits_Month_13 { get; set; }
        //*******************************************
        public double Sales_Month_14 { get; set; }
        public int Hits_Month_14 { get; set; }
        public double LostSales_Month_14 { get; set; }
        public int LostHits_Month_14 { get; set; }
        //*******************************************
        public double Sales_Month_15 { get; set; }
        public int Hits_Month_15 { get; set; }
        public double LostSales_Month_15 { get; set; }
        public int LostHits_Month_15 { get; set; }
        //*******************************************
        public double Sales_Month_16 { get; set; }
        public int Hits_Month_16 { get; set; }
        public double LostSales_Month_16 { get; set; }
        public int LostHits_Month_16 { get; set; }
        //*******************************************
        public double Sales_Month_17 { get; set; }
        public int Hits_Month_17 { get; set; }
        public double LostSales_Month_17 { get; set; }
        public int LostHits_Month_17 { get; set; }
        //*******************************************
        public double Sales_Month_18 { get; set; }
        public int Hits_Month_18 { get; set; }
        public double LostSales_Month_18 { get; set; }
        public int LostHits_Month_18 { get; set; }
        //*******************************************
        public double Sales_Month_19 { get; set; }
        public int Hits_Month_19 { get; set; }
        public double LostSales_Month_19 { get; set; }
        public int LostHits_Month_19 { get; set; }
        //*******************************************
        public double Sales_Month_20 { get; set; }
        public int Hits_Month_20 { get; set; }
        public double LostSales_Month_20 { get; set; }
        public int LostHits_Month_20 { get; set; }
        //*******************************************
        public double Sales_Month_21 { get; set; }
        public int Hits_Month_21 { get; set; }
        public double LostSales_Month_21 { get; set; }
        public int LostHits_Month_21 { get; set; }
        //*******************************************
        public double Sales_Month_22 { get; set; }
        public int Hits_Month_22 { get; set; }
        public double LostSales_Month_22 { get; set; }
        public int LostHits_Month_22 { get; set; }
        //*******************************************
        public double Sales_Month_23 { get; set; }
        public int Hits_Month_23 { get; set; }
        public double LostSales_Month_23 { get; set; }
        public int LostHits_Month_23 { get; set; }
        //*******************************************
        public double Sales_Month_24 { get; set; }
        public int Hits_Month_24 { get; set; }
        public double LostSales_Month_24 { get; set; }
        public int LostHits_Month_24 { get; set; }
        //*******************************************
        public double Sales_Month_25 { get; set; }
        public int Hits_Month_25 { get; set; }
        public double LostSales_Month_25 { get; set; }
        public int LostHits_Month_25 { get; set; }
        //*******************************************
        public double Sales_Month_26 { get; set; }
        public int Hits_Month_26 { get; set; }
        public double LostSales_Month_26 { get; set; }
        public int LostHits_Month_26 { get; set; }
        //*******************************************
        public double Sales_Month_27 { get; set; }
        public int Hits_Month_27 { get; set; }
        public double LostSales_Month_27 { get; set; }
        public int LostHits_Month_27 { get; set; }
        //*******************************************
        public double Sales_Month_28 { get; set; }
        public int Hits_Month_28 { get; set; }
        public double LostSales_Month_28 { get; set; }
        public int LostHits_Month_28 { get; set; }
        //*******************************************
        public double Sales_Month_29 { get; set; }
        public int Hits_Month_29 { get; set; }
        public double LostSales_Month_29 { get; set; }
        public int LostHits_Month_29 { get; set; }
        //*******************************************
        public double Sales_Month_30 { get; set; }
        public int Hits_Month_30 { get; set; }
        public double LostSales_Month_30 { get; set; }
        public int LostHits_Month_30 { get; set; }
        //*******************************************
        public double Sales_Month_31 { get; set; }
        public int Hits_Month_31 { get; set; }
        public double LostSales_Month_31 { get; set; }
        public int LostHits_Month_31 { get; set; }
        //*******************************************
        public double Sales_Month_32 { get; set; }
        public int Hits_Month_32 { get; set; }
        public double LostSales_Month_32 { get; set; }
        public int LostHits_Month_32 { get; set; }
        //*******************************************
        public double Sales_Month_33 { get; set; }
        public int Hits_Month_33 { get; set; }
        public double LostSales_Month_33 { get; set; }
        public int LostHits_Month_33 { get; set; }
        //*******************************************
        public double Sales_Month_34 { get; set; }
        public int Hits_Month_34 { get; set; }
        public double LostSales_Month_34 { get; set; }
        public int LostHits_Month_34 { get; set; }
        //*******************************************
        public double Sales_Month_35 { get; set; }
        public int Hits_Month_35 { get; set; }
        public double LostSales_Month_35 { get; set; }
        public int LostHits_Month_35 { get; set; }
        //*******************************************
        public double Sales_Month_36 { get; set; }
        public int Hits_Month_36 { get; set; }
        public double LostSales_Month_36 { get; set; }
        public int LostHits_Month_36 { get; set; }
        //*******************************************
        public double Sales_Month_37 { get; set; }
        public int Hits_Month_37 { get; set; }
        public double LostSales_Month_37 { get; set; }
        public int LostHits_Month_37 { get; set; }
        //*******************************************
        public double Sales_Month_38 { get; set; }
        public int Hits_Month_38 { get; set; }
        public double LostSales_Month_38 { get; set; }
        public int LostHits_Month_38 { get; set; }
        //*******************************************
        public double Sales_Month_39 { get; set; }
        public int Hits_Month_39 { get; set; }
        public double LostSales_Month_39 { get; set; }
        public int LostHits_Month_39 { get; set; }
        //*******************************************
        public double Sales_Month_40 { get; set; }
        public int Hits_Month_40 { get; set; }
        public double LostSales_Month_40 { get; set; }
        public int LostHits_Month_40 { get; set; }
        //*******************************************
        public double Sales_Month_41 { get; set; }
        public int Hits_Month_41 { get; set; }
        public double LostSales_Month_41 { get; set; }
        public int LostHits_Month_41 { get; set; }
        //*******************************************
        public double Sales_Month_42 { get; set; }
        public int Hits_Month_42 { get; set; }
        public double LostSales_Month_42 { get; set; }
        public int LostHits_Month_42 { get; set; }
        //*******************************************
        public double Sales_Month_43 { get; set; }
        public int Hits_Month_43 { get; set; }
        public double LostSales_Month_43 { get; set; }
        public int LostHits_Month_43 { get; set; }
        //*******************************************
        public double Sales_Month_44 { get; set; }
        public int Hits_Month_44 { get; set; }
        public double LostSales_Month_44 { get; set; }
        public int LostHits_Month_44 { get; set; }
        //*******************************************
        public double Sales_Month_45 { get; set; }
        public int Hits_Month_45 { get; set; }
        public double LostSales_Month_45 { get; set; }
        public int LostHits_Month_45 { get; set; }
        //*******************************************
        public double Sales_Month_46 { get; set; }
        public int Hits_Month_46 { get; set; }
        public double LostSales_Month_46 { get; set; }
        public int LostHits_Month_46 { get; set; }
        //*******************************************
        public double Sales_Month_47 { get; set; }
        public int Hits_Month_47 { get; set; }
        public double LostSales_Month_47 { get; set; }
        public int LostHits_Month_47 { get; set; }
        //*******************************************
        public double Sales_Month_48 { get; set; }
        public int Hits_Month_48 { get; set; }
        public double LostSales_Month_48 { get; set; }
        public int LostHits_Month_48 { get; set; }
        //*******************************************
        public double TotalSales_1To12 { get; set; }
        public int TotalHits_1To12 { get; set; }
        public double TotalLostSales_1To12 { get; set; }
        public int TotalLostHits_1To12 { get; set; }
        //*******************************************
        public double TotalSales_13To24 { get; set; }
        public int TotalHits_13To24 { get; set; }
        public double TotalLostSales_13To24 { get; set; }
        public int TotalLostHits_13To24 { get; set; }
        //*******************************************
        public double TotalSales_25To36 { get; set; }
        public int TotalHits_25To36 { get; set; }
        public double TotalLostSales_25To36 { get; set; }
        public int TotalLostHits_25To36 { get; set; }
        //*******************************************
        public double TotalSales_37To48 { get; set; }
        public int TotalHits_37To48 { get; set; }
        public double TotalLostSales_37To48 { get; set; }
        public int TotalLostHits_37To48 { get; set; }


        public void ToString(ref StringBuilder p_sb, ref JDPRISM p_jdprism)
        {
            String espacio = "\t";
            String vacio = "";
            String cero = "0";
            try
            {
                p_sb.Append(RecordCode).Append(espacio);//1
                p_sb.Append(PartNumber).Append(espacio);//2
                p_sb.Append((AvailableQuantity > 0) ? AvailableQuantity.ToString("G") : cero).Append(espacio);//3
                //p_sb.Append(AvailableQuantity.ToString("G")).Append(espacio);//3
                p_sb.Append(OOQuantity.ToString("G")).Append(espacio);//4
                p_sb.Append((ReserveQ_WO >= 0) ? ReserveQ_WO.ToString("G") : vacio).Append(espacio);//5
                p_sb.Append((ReserveQ_PT >= 0) ? ReserveQ_PT.ToString("G") : vacio).Append(espacio);//6
                p_sb.Append(CurrrentMTDSales.ToString("G")).Append(espacio);//7
                p_sb.Append(CurrentMTDHits.ToString()).Append(espacio);//8
                p_sb.Append((CurrentMTDLostSales >= 0) ? CurrentMTDLostSales.ToString("G") : vacio).Append(espacio);//9
                p_sb.Append((CurrentMTDLostHits >= 0) ? CurrentMTDLostHits.ToString() : vacio).Append(espacio);//10
                p_sb.Append(DealerPPP.ToString("G")).Append(espacio);//11
                p_sb.Append((!string.IsNullOrEmpty(BinLocation)) ? BinLocation : vacio).Append(espacio);//12
                p_sb.Append((!string.IsNullOrEmpty(AlternateBinLocation)) ? AlternateBinLocation : vacio).Append(espacio);//13
                p_sb.Append((VendorPartCost >= 0) ? VendorPartCost.ToString("G") : vacio).Append(espacio);//14
                p_sb.Append((VendorPackageQuantity >= 0) ? VendorPackageQuantity.ToString() : vacio).Append(espacio);//15
                p_sb.Append((!string.IsNullOrEmpty(VendorCode)) ? VendorCode : vacio).Append(espacio);//16
                p_sb.Append((!string.IsNullOrEmpty(VendorSubstitutionInfo)) ? VendorSubstitutionInfo : vacio).Append(espacio);//17
                p_sb.Append((!string.IsNullOrEmpty(PricingBase)) ? PricingBase : vacio).Append(espacio);//18
                p_sb.Append((PricingAdditive >= 0) ? PricingAdditive.ToString("G") : vacio).Append(espacio);//19
                p_sb.Append((DealerPrice >= 0) ? DealerPrice.ToString("G") : vacio).Append(espacio);//20
                p_sb.Append((!string.IsNullOrEmpty(OrderFormulaCode)) ? OrderFormulaCode : vacio).Append(espacio);//21
                p_sb.Append((!string.IsNullOrEmpty(DeleteIndicator)) ? DeleteIndicator : vacio).Append(espacio);//22
                p_sb.Append((ReservedHits_WO >= 0) ? ReservedHits_WO.ToString() : vacio).Append(espacio);//23
                p_sb.Append((ReservedHits_PT >= 0) ? ReservedHits_PT.ToString() : vacio).Append(espacio);//24
                p_sb.Append((AverageCost >= 0) ? AverageCost.ToString("G") : vacio).Append(espacio);//25
                //*************** valid according to load type ********************
                //if (p_jdprism.LoadType == "D")
                //{
                //    p_sb.Append("\n");
                //    return;
                //}
                //*****************************************************************
                p_sb.Append(Start_I_Records).Append(espacio);//26
                p_sb.Append((!string.IsNullOrEmpty(PartDescription)) ? PartDescription : vacio).Append(espacio);//27
                p_sb.Append((!string.IsNullOrEmpty(DealerPartNote)) ? DealerPartNote : vacio).Append(espacio);//28
                p_sb.Append((!string.IsNullOrEmpty(OrderIndicator)) ? OrderIndicator : vacio).Append(espacio);//29
                p_sb.Append(DateAdded.ToString("yyyy-MM-dd")).Append(espacio);//30
                p_sb.Append((!string.IsNullOrEmpty(DealerGroupCode)) ? DealerGroupCode : vacio).Append(espacio);//31
                p_sb.Append((MinOrderQuantity >= 0) ? MinOrderQuantity.ToString("G") : vacio).Append(espacio);//32
                p_sb.Append((MaxOrderQuantity >= 0) ? MaxOrderQuantity.ToString("G") : vacio).Append(espacio);//33
                p_sb.Append(NumberOfMonthlyHistory.ToString()).Append(espacio);//34
                p_sb.Append((PiecesInSet >= 0) ? PiecesInSet.ToString() : vacio).Append(espacio);//35
                //*** 1 month ago ***
                if (NumberOfMonthlyHistory >= 1)
                {
                    p_sb.Append((Sales_Month_1 >= 0) ? Sales_Month_1.ToString("G") : vacio).Append(espacio);//36
                    p_sb.Append((Hits_Month_1 >= 0) ? Hits_Month_1.ToString() : vacio).Append(espacio);//37
                    p_sb.Append((LostSales_Month_1 >= 0) ? LostSales_Month_1.ToString("G") : vacio).Append(espacio);//38
                    p_sb.Append((LostHits_Month_1 >= 0) ? LostHits_Month_1.ToString() : vacio).Append(espacio);//39
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }

                //*** 2 month ago ***
                if (NumberOfMonthlyHistory >= 2)
                {
                    p_sb.Append((Sales_Month_2 >= 0) ? Sales_Month_2.ToString("G") : vacio).Append(espacio);//40
                    p_sb.Append((Hits_Month_2 >= 0) ? Hits_Month_2.ToString() : vacio).Append(espacio);//41
                    p_sb.Append((LostSales_Month_2 >= 0) ? LostSales_Month_2.ToString("G") : vacio).Append(espacio);//42
                    p_sb.Append((LostHits_Month_2 >= 0) ? LostHits_Month_2.ToString() : vacio).Append(espacio);//43
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 3 month ago ***
                if (NumberOfMonthlyHistory >= 3)
                {
                    p_sb.Append((Sales_Month_3 >= 0) ? Sales_Month_3.ToString("G") : vacio).Append(espacio);//44
                    p_sb.Append((Hits_Month_3 >= 0) ? Hits_Month_3.ToString() : vacio).Append(espacio);//45
                    p_sb.Append((LostSales_Month_3 >= 0) ? LostSales_Month_3.ToString("G") : vacio).Append(espacio);//46
                    p_sb.Append((LostHits_Month_3 >= 0) ? LostHits_Month_3.ToString() : vacio).Append(espacio);//47
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 4 month ago ***
                if (NumberOfMonthlyHistory >= 4)
                {
                    p_sb.Append((Sales_Month_4 >= 0) ? Sales_Month_4.ToString("G") : vacio).Append(espacio);//48
                    p_sb.Append((Hits_Month_4 >= 0) ? Hits_Month_4.ToString() : vacio).Append(espacio);//49
                    p_sb.Append((LostSales_Month_4 >= 0) ? LostSales_Month_4.ToString("G") : vacio).Append(espacio);//50
                    p_sb.Append((LostHits_Month_4 >= 0) ? LostHits_Month_4.ToString() : vacio).Append(espacio);//51
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 5 month ago ***
                if (NumberOfMonthlyHistory >= 5)
                {
                    p_sb.Append((Sales_Month_5 >= 0) ? Sales_Month_5.ToString("G") : vacio).Append(espacio);//52
                    p_sb.Append((Hits_Month_5 >= 0) ? Hits_Month_5.ToString() : vacio).Append(espacio);//53
                    p_sb.Append((LostSales_Month_5 >= 0) ? LostSales_Month_5.ToString("G") : vacio).Append(espacio);//54
                    p_sb.Append((LostHits_Month_5 >= 0) ? LostHits_Month_5.ToString() : vacio).Append(espacio);//55
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 6 month ago ***
                if (NumberOfMonthlyHistory >= 6)
                {
                    p_sb.Append((Sales_Month_6 >= 0) ? Sales_Month_6.ToString("G") : vacio).Append(espacio);//56
                    p_sb.Append((Hits_Month_6 >= 0) ? Hits_Month_6.ToString() : vacio).Append(espacio);//57
                    p_sb.Append((LostSales_Month_6 >= 0) ? LostSales_Month_6.ToString("G") : vacio).Append(espacio);//58
                    p_sb.Append((LostHits_Month_6 >= 0) ? LostHits_Month_6.ToString() : vacio).Append(espacio);//59
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 7 month ago ***
                if (NumberOfMonthlyHistory >= 7)
                {
                    p_sb.Append((Sales_Month_7 >= 0) ? Sales_Month_7.ToString("G") : vacio).Append(espacio);//60
                    p_sb.Append((Hits_Month_7 >= 0) ? Hits_Month_7.ToString() : vacio).Append(espacio);//61
                    p_sb.Append((LostSales_Month_7 >= 0) ? LostSales_Month_7.ToString("G") : vacio).Append(espacio);//62
                    p_sb.Append((LostHits_Month_7 >= 0) ? LostHits_Month_7.ToString() : vacio).Append(espacio);//63
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 8 month ago ***
                if (NumberOfMonthlyHistory >= 8)
                {
                    p_sb.Append((Sales_Month_8 >= 0) ? Sales_Month_8.ToString("G") : vacio).Append(espacio);//64
                    p_sb.Append((Hits_Month_8 >= 0) ? Hits_Month_8.ToString() : vacio).Append(espacio);//65
                    p_sb.Append((LostSales_Month_8 >= 0) ? LostSales_Month_8.ToString("G") : vacio).Append(espacio);//66
                    p_sb.Append((LostHits_Month_8 >= 0) ? LostHits_Month_8.ToString() : vacio).Append(espacio);//67
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 9 month ago ***
                if (NumberOfMonthlyHistory >= 9)
                {
                    p_sb.Append((Sales_Month_9 >= 0) ? Sales_Month_9.ToString("G") : vacio).Append(espacio);//68
                    p_sb.Append((Hits_Month_9 >= 0) ? Hits_Month_9.ToString() : vacio).Append(espacio);//69
                    p_sb.Append((LostSales_Month_9 >= 0) ? LostSales_Month_9.ToString("G") : vacio).Append(espacio);//70
                    p_sb.Append((LostHits_Month_9 >= 0) ? LostHits_Month_9.ToString() : vacio).Append(espacio);//71
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 10 month ago ***
                if (NumberOfMonthlyHistory >= 10)
                {
                    p_sb.Append((Sales_Month_10 >= 0) ? Sales_Month_10.ToString("G") : vacio).Append(espacio);//72
                    p_sb.Append((Hits_Month_10 >= 0) ? Hits_Month_10.ToString() : vacio).Append(espacio);//73
                    p_sb.Append((LostSales_Month_10 >= 0) ? LostSales_Month_10.ToString("G") : vacio).Append(espacio);//74
                    p_sb.Append((LostHits_Month_10 >= 0) ? LostHits_Month_10.ToString() : vacio).Append(espacio);//75
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 11 month ago ***
                if (NumberOfMonthlyHistory >= 11)
                {
                    p_sb.Append((Sales_Month_11 >= 0) ? Sales_Month_11.ToString("G") : vacio).Append(espacio);//76
                    p_sb.Append((Hits_Month_11 >= 0) ? Hits_Month_11.ToString() : vacio).Append(espacio);//77
                    p_sb.Append((LostSales_Month_11 >= 0) ? LostSales_Month_11.ToString("G") : vacio).Append(espacio);//78
                    p_sb.Append((LostHits_Month_11 >= 0) ? LostHits_Month_11.ToString() : vacio).Append(espacio);//79
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 12 month ago ***
                if (NumberOfMonthlyHistory >= 12)
                {
                    p_sb.Append((Sales_Month_12 >= 0) ? Sales_Month_12.ToString("G") : vacio).Append(espacio);//80
                    p_sb.Append((Hits_Month_12 >= 0) ? Hits_Month_12.ToString() : vacio).Append(espacio);//81
                    p_sb.Append((LostSales_Month_12 >= 0) ? LostSales_Month_12.ToString("G") : vacio).Append(espacio);//82
                    p_sb.Append((LostHits_Month_12 >= 0) ? LostHits_Month_12.ToString() : vacio).Append(espacio);//83
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 13 month ago ***
                if (NumberOfMonthlyHistory >= 13)
                {
                    p_sb.Append((Sales_Month_13 >= 0) ? Sales_Month_13.ToString("G") : vacio).Append(espacio);//84
                    p_sb.Append((Hits_Month_13 >= 0) ? Hits_Month_13.ToString() : vacio).Append(espacio);//85
                    p_sb.Append((LostSales_Month_13 >= 0) ? LostSales_Month_13.ToString("G") : vacio).Append(espacio);//86
                    p_sb.Append((LostHits_Month_13 >= 0) ? LostHits_Month_13.ToString() : vacio).Append(espacio);//87
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 14 month ago ***
                if (NumberOfMonthlyHistory >= 14)
                {
                    p_sb.Append((Sales_Month_14 >= 0) ? Sales_Month_14.ToString("G") : vacio).Append(espacio);//88
                    p_sb.Append((Hits_Month_14 >= 0) ? Hits_Month_14.ToString() : vacio).Append(espacio);//89
                    p_sb.Append((LostSales_Month_14 >= 0) ? LostSales_Month_14.ToString("G") : vacio).Append(espacio);//90
                    p_sb.Append((LostHits_Month_14 >= 0) ? LostHits_Month_14.ToString() : vacio).Append(espacio);//91
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 15 month ago ***
                if (NumberOfMonthlyHistory >= 15)
                {
                    p_sb.Append((Sales_Month_15 >= 0) ? Sales_Month_15.ToString("G") : vacio).Append(espacio);//92
                    p_sb.Append((Hits_Month_15 >= 0) ? Hits_Month_15.ToString() : vacio).Append(espacio);//93
                    p_sb.Append((LostSales_Month_15 >= 0) ? LostSales_Month_15.ToString("G") : vacio).Append(espacio);//94
                    p_sb.Append((LostHits_Month_15 >= 0) ? LostHits_Month_15.ToString() : vacio).Append(espacio);//95
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 16 month ago ***
                if (NumberOfMonthlyHistory >= 16)
                {
                    p_sb.Append((Sales_Month_16 >= 0) ? Sales_Month_16.ToString("G") : vacio).Append(espacio);//96
                    p_sb.Append((Hits_Month_16 >= 0) ? Hits_Month_16.ToString() : vacio).Append(espacio);//97
                    p_sb.Append((LostSales_Month_16 >= 0) ? LostSales_Month_16.ToString("G") : vacio).Append(espacio);//98
                    p_sb.Append((LostHits_Month_16 >= 0) ? LostHits_Month_16.ToString() : vacio).Append(espacio);//99
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 17 month ago ***
                if (NumberOfMonthlyHistory >= 17)
                {
                    p_sb.Append((Sales_Month_17 >= 0) ? Sales_Month_17.ToString("G") : vacio).Append(espacio);//100
                    p_sb.Append((Hits_Month_17 >= 0) ? Hits_Month_17.ToString() : vacio).Append(espacio);//101
                    p_sb.Append((LostSales_Month_17 >= 0) ? LostSales_Month_17.ToString("G") : vacio).Append(espacio);//102
                    p_sb.Append((LostHits_Month_17 >= 0) ? LostHits_Month_17.ToString() : vacio).Append(espacio);//103
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 18 month ago ***
                if (NumberOfMonthlyHistory >= 18)
                {
                    p_sb.Append((Sales_Month_18 >= 0) ? Sales_Month_18.ToString("G") : vacio).Append(espacio);//104
                    p_sb.Append((Hits_Month_18 >= 0) ? Hits_Month_18.ToString() : vacio).Append(espacio);//105
                    p_sb.Append((LostSales_Month_18 >= 0) ? LostSales_Month_18.ToString("G") : vacio).Append(espacio);//106
                    p_sb.Append((LostHits_Month_18 >= 0) ? LostHits_Month_18.ToString() : vacio).Append(espacio);//107
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 19 month ago ***
                if (NumberOfMonthlyHistory >= 19)
                {
                    p_sb.Append((Sales_Month_19 >= 0) ? Sales_Month_19.ToString("G") : vacio).Append(espacio);//108
                    p_sb.Append((Hits_Month_19 >= 0) ? Hits_Month_19.ToString() : vacio).Append(espacio);//109
                    p_sb.Append((LostSales_Month_19 >= 0) ? LostSales_Month_19.ToString("G") : vacio).Append(espacio);//110
                    p_sb.Append((LostHits_Month_19 >= 0) ? LostHits_Month_19.ToString() : vacio).Append(espacio);//111
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 20 month ago ***
                if (NumberOfMonthlyHistory >= 20)
                {
                    p_sb.Append((Sales_Month_20 >= 0) ? Sales_Month_20.ToString("G") : vacio).Append(espacio);//112
                    p_sb.Append((Hits_Month_20 >= 0) ? Hits_Month_20.ToString() : vacio).Append(espacio);//113
                    p_sb.Append((LostSales_Month_20 >= 0) ? LostSales_Month_20.ToString("G") : vacio).Append(espacio);//114
                    p_sb.Append((LostHits_Month_20 >= 0) ? LostHits_Month_20.ToString() : vacio).Append(espacio);//115
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }

                //*** 21 month ago ***
                if (NumberOfMonthlyHistory >= 21)
                {
                    p_sb.Append((Sales_Month_21 >= 0) ? Sales_Month_21.ToString("G") : vacio).Append(espacio);//116
                    p_sb.Append((Hits_Month_21 >= 0) ? Hits_Month_21.ToString() : vacio).Append(espacio);//117
                    p_sb.Append((LostSales_Month_21 >= 0) ? LostSales_Month_21.ToString("G") : vacio).Append(espacio);//118
                    p_sb.Append((LostHits_Month_21 >= 0) ? LostHits_Month_21.ToString() : vacio).Append(espacio);//119
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 22 month ago ***
                if (NumberOfMonthlyHistory >= 22)
                {
                    p_sb.Append((Sales_Month_22 >= 0) ? Sales_Month_22.ToString("G") : vacio).Append(espacio);//120
                    p_sb.Append((Hits_Month_22 >= 0) ? Hits_Month_22.ToString() : vacio).Append(espacio);//121
                    p_sb.Append((LostSales_Month_22 >= 0) ? LostSales_Month_22.ToString("G") : vacio).Append(espacio);//122
                    p_sb.Append((LostHits_Month_22 >= 0) ? LostHits_Month_22.ToString() : vacio).Append(espacio);//123
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 23 month ago ***
                if (NumberOfMonthlyHistory >= 23)
                {
                    p_sb.Append((Sales_Month_23 >= 0) ? Sales_Month_23.ToString("G") : vacio).Append(espacio);//124
                    p_sb.Append((Hits_Month_23 >= 0) ? Hits_Month_23.ToString() : vacio).Append(espacio);//125
                    p_sb.Append((LostSales_Month_23 >= 0) ? LostSales_Month_23.ToString("G") : vacio).Append(espacio);//126
                    p_sb.Append((LostHits_Month_23 >= 0) ? LostHits_Month_23.ToString() : vacio).Append(espacio);//127
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 24 month ago ***
                if (NumberOfMonthlyHistory >= 24)
                {
                    p_sb.Append((Sales_Month_24 >= 0) ? Sales_Month_24.ToString("G") : vacio).Append(espacio);//128
                    p_sb.Append((Hits_Month_24 >= 0) ? Hits_Month_24.ToString() : vacio).Append(espacio);//129
                    p_sb.Append((LostSales_Month_24 >= 0) ? LostSales_Month_24.ToString("G") : vacio).Append(espacio);//130
                    p_sb.Append((LostHits_Month_24 >= 0) ? LostHits_Month_24.ToString() : vacio).Append(espacio);//131
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 25 month ago ***
                if (NumberOfMonthlyHistory >= 25)
                {
                    p_sb.Append((Sales_Month_25 >= 0) ? Sales_Month_25.ToString("G") : vacio).Append(espacio);//132
                    p_sb.Append((Hits_Month_25 >= 0) ? Hits_Month_25.ToString() : vacio).Append(espacio);//133
                    p_sb.Append((LostSales_Month_25 >= 0) ? LostSales_Month_25.ToString("G") : vacio).Append(espacio);//134
                    p_sb.Append((LostHits_Month_25 >= 0) ? LostHits_Month_25.ToString() : vacio).Append(espacio);//135
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 26 month ago ***
                if (NumberOfMonthlyHistory >= 26)
                {
                    p_sb.Append((Sales_Month_26 >= 0) ? Sales_Month_26.ToString("G") : vacio).Append(espacio);//136
                    p_sb.Append((Hits_Month_26 >= 0) ? Hits_Month_26.ToString() : vacio).Append(espacio);//137
                    p_sb.Append((LostSales_Month_26 >= 0) ? LostSales_Month_26.ToString("G") : vacio).Append(espacio);//138
                    p_sb.Append((LostHits_Month_26 >= 0) ? LostHits_Month_26.ToString() : vacio).Append(espacio);//139
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 27 month ago ***
                if (NumberOfMonthlyHistory >= 27)
                {
                    p_sb.Append((Sales_Month_27 >= 0) ? Sales_Month_27.ToString("G") : vacio).Append(espacio);//140
                    p_sb.Append((Hits_Month_27 >= 0) ? Hits_Month_27.ToString() : vacio).Append(espacio);//141
                    p_sb.Append((LostSales_Month_27 >= 0) ? LostSales_Month_27.ToString("G") : vacio).Append(espacio);//142
                    p_sb.Append((LostHits_Month_27 >= 0) ? LostHits_Month_27.ToString() : vacio).Append(espacio);//143
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 28 month ago ***
                if (NumberOfMonthlyHistory >= 28)
                {
                    p_sb.Append((Sales_Month_28 >= 0) ? Sales_Month_28.ToString("G") : vacio).Append(espacio);//144
                    p_sb.Append((Hits_Month_28 >= 0) ? Hits_Month_28.ToString() : vacio).Append(espacio);//145
                    p_sb.Append((LostSales_Month_28 >= 0) ? LostSales_Month_28.ToString("G") : vacio).Append(espacio);//146
                    p_sb.Append((LostHits_Month_28 >= 0) ? LostHits_Month_28.ToString() : vacio).Append(espacio);//147
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 29 month ago ***
                if (NumberOfMonthlyHistory >= 29)
                {
                    p_sb.Append((Sales_Month_29 >= 0) ? Sales_Month_29.ToString("G") : vacio).Append(espacio);//148
                    p_sb.Append((Hits_Month_29 >= 0) ? Hits_Month_29.ToString() : vacio).Append(espacio);//149
                    p_sb.Append((LostSales_Month_29 >= 0) ? LostSales_Month_29.ToString("G") : vacio).Append(espacio);//150
                    p_sb.Append((LostHits_Month_29 >= 0) ? LostHits_Month_29.ToString() : vacio).Append(espacio);//151
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 30 month ago ***
                if (NumberOfMonthlyHistory >= 30)
                {
                    p_sb.Append((Sales_Month_30 >= 0) ? Sales_Month_30.ToString("G") : vacio).Append(espacio);//152
                    p_sb.Append((Hits_Month_30 >= 0) ? Hits_Month_30.ToString() : vacio).Append(espacio);//153
                    p_sb.Append((LostSales_Month_30 >= 0) ? LostSales_Month_30.ToString("G") : vacio).Append(espacio);//154
                    p_sb.Append((LostHits_Month_30 >= 0) ? LostHits_Month_30.ToString() : vacio).Append(espacio);//155
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 31 month ago ***
                if (NumberOfMonthlyHistory >= 31)
                {
                    p_sb.Append((Sales_Month_31 >= 0) ? Sales_Month_31.ToString("G") : vacio).Append(espacio);//156
                    p_sb.Append((Hits_Month_31 >= 0) ? Hits_Month_31.ToString() : vacio).Append(espacio);//157
                    p_sb.Append((LostSales_Month_31 >= 0) ? LostSales_Month_31.ToString("G") : vacio).Append(espacio);//158
                    p_sb.Append((LostHits_Month_31 >= 0) ? LostHits_Month_31.ToString() : vacio).Append(espacio);//159
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 32 month ago ***
                if (NumberOfMonthlyHistory >= 32)
                {
                    p_sb.Append((Sales_Month_32 >= 0) ? Sales_Month_32.ToString("G") : vacio).Append(espacio);//160
                    p_sb.Append((Hits_Month_32 >= 0) ? Hits_Month_32.ToString() : vacio).Append(espacio);//161
                    p_sb.Append((LostSales_Month_32 >= 0) ? LostSales_Month_32.ToString("G") : vacio).Append(espacio);//162
                    p_sb.Append((LostHits_Month_32 >= 0) ? LostHits_Month_32.ToString() : vacio).Append(espacio);//163
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 33 month ago ***
                if (NumberOfMonthlyHistory >= 33)
                {
                    p_sb.Append((Sales_Month_33 >= 0) ? Sales_Month_33.ToString("G") : vacio).Append(espacio);//164
                    p_sb.Append((Hits_Month_33 >= 0) ? Hits_Month_33.ToString() : vacio).Append(espacio);//165
                    p_sb.Append((LostSales_Month_33 >= 0) ? LostSales_Month_33.ToString("G") : vacio).Append(espacio);//166
                    p_sb.Append((LostHits_Month_33 >= 0) ? LostHits_Month_33.ToString() : vacio).Append(espacio);//167
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 34 month ago ***
                if (NumberOfMonthlyHistory >= 34)
                {
                    p_sb.Append((Sales_Month_34 >= 0) ? Sales_Month_34.ToString("G") : vacio).Append(espacio);//168
                    p_sb.Append((Hits_Month_34 >= 0) ? Hits_Month_34.ToString() : vacio).Append(espacio);//169
                    p_sb.Append((LostSales_Month_34 >= 0) ? LostSales_Month_34.ToString("G") : vacio).Append(espacio);//170
                    p_sb.Append((LostHits_Month_34 >= 0) ? LostHits_Month_34.ToString() : vacio).Append(espacio);//171
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 35 month ago ***
                if (NumberOfMonthlyHistory >= 35)
                {
                    p_sb.Append((Sales_Month_35 >= 0) ? Sales_Month_35.ToString("G") : vacio).Append(espacio);//172
                    p_sb.Append((Hits_Month_35 >= 0) ? Hits_Month_35.ToString() : vacio).Append(espacio);//173
                    p_sb.Append((LostSales_Month_35 >= 0) ? LostSales_Month_35.ToString("G") : vacio).Append(espacio);//174
                    p_sb.Append((LostHits_Month_35 >= 0) ? LostHits_Month_35.ToString() : vacio).Append(espacio);//175
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** 36 month ago ***
                if (NumberOfMonthlyHistory >= 36)
                {
                    p_sb.Append((Sales_Month_36 >= 0) ? Sales_Month_36.ToString("G") : vacio).Append(espacio);//176
                    p_sb.Append((Hits_Month_36 >= 0) ? Hits_Month_36.ToString() : vacio).Append(espacio);//177
                    p_sb.Append((LostSales_Month_36 >= 0) ? LostSales_Month_36.ToString("G") : vacio).Append(espacio);//178
                    p_sb.Append((LostHits_Month_36 >= 0) ? LostHits_Month_36.ToString() : vacio).Append(espacio);//179
                }
                else
                {
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                    p_sb.Append(vacio).Append(espacio);
                }
                //*** Sales range 1-12 month ago ***
                p_sb.Append(espacio);//p_sb.Append((TotalSales_1To12 >= 0) ? TotalSales_1To12.ToString("G") : vacio).Append(espacio);//180
                p_sb.Append(espacio);//p_sb.Append((TotalHits_1To12 >= 0) ? TotalHits_1To12.ToString() : vacio).Append(espacio);//181
                p_sb.Append(espacio);//p_sb.Append((TotalLostSales_1To12 >= 0) ? TotalLostSales_1To12.ToString("G") : vacio).Append(espacio);//182
                p_sb.Append(espacio);//p_sb.Append((TotalLostHits_1To12 >= 0) ? TotalLostHits_1To12.ToString() : vacio).Append(espacio);//183
                ////*** Sales range 13-24 month ago ***
                p_sb.Append(espacio);//p_sb.Append((TotalSales_13To24 >= 0) ? TotalSales_13To24.ToString("G") : vacio).Append(espacio);//184
                p_sb.Append(espacio);//p_sb.Append((TotalHits_13To24 >= 0) ? TotalHits_13To24.ToString() : vacio).Append(espacio);//185
                p_sb.Append(espacio);//p_sb.Append((TotalLostSales_13To24 >= 0) ? TotalLostSales_13To24.ToString("G") : vacio).Append(espacio);//186
                p_sb.Append(espacio);//p_sb.Append((TotalLostHits_13To24 >= 0) ? TotalLostHits_13To24.ToString() : vacio).Append(espacio);//187
                ////*** Sales range 25-36 month ago ***
                p_sb.Append(espacio);//p_sb.Append((TotalSales_25To36 >= 0) ? TotalSales_25To36.ToString("G") : vacio).Append(espacio);//188
                p_sb.Append(espacio);//p_sb.Append((TotalHits_25To36 >= 0) ? TotalHits_25To36.ToString() : vacio).Append(espacio);//189
                p_sb.Append(espacio);//p_sb.Append((TotalLostSales_25To36 >= 0) ? TotalLostSales_25To36.ToString("G") : vacio).Append(espacio);//190
                p_sb.Append(espacio);//p_sb.Append((TotalLostHits_25To36 >= 0) ? TotalLostHits_25To36.ToString() : vacio).Append(espacio);//191
                ////*** Sales range 37-48 month ago ***
                p_sb.Append(espacio);//p_sb.Append((TotalSales_37To48 >= 0) ? TotalSales_37To48.ToString("G") : vacio).Append(espacio);//192
                p_sb.Append(espacio);//p_sb.Append((TotalHits_37To48 >= 0) ? TotalHits_37To48.ToString() : vacio).Append(espacio);//193
                p_sb.Append(espacio);//p_sb.Append((TotalLostSales_37To48 >= 0) ? TotalLostSales_37To48.ToString("G") : vacio).Append(espacio);//194
                p_sb.Append(espacio);//p_sb.Append((TotalLostHits_37To48 >= 0) ? TotalLostHits_37To48.ToString() : vacio).Append(espacio);//195

                p_sb.Append("\r\n");
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    }
}
