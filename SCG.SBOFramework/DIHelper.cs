using System.Globalization;
using SAPbobsCOM;
using DMS_Connector;

namespace SCG.SBOFramework
{
    public static class DIHelper
    {
        public static NumberFormatInfo GetNumberFormatInfo(ICompany company)
        {
            //var companyService = company.GetCompanyService();
            //var adminInfo = companyService.GetAdminInfo();
            var adminInfo = DMS_Connector.Company.AdminInfo; 
            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.CurrencyDecimalSeparator = adminInfo.DecimalSeparator;
            numberFormatInfo.CurrencyGroupSeparator = adminInfo.ThousandsSeparator;
            numberFormatInfo.CurrencyDecimalDigits = adminInfo.PriceAccuracy;
            numberFormatInfo.NumberDecimalDigits = adminInfo.AccuracyofQuantities;
            return numberFormatInfo;
        }
    }
}