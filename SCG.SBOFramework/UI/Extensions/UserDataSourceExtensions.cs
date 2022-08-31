using System;
using System.Globalization;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI.Extensions
{
    public static class UserDataSourceExtensions
    {
        public static void SetValue(this UserDataSources userDataSource, string column, DateTime date)
        {
            userDataSource.Item(column).ValueEx = date.ToString("yyyyMMdd");
        }

        public static void SetValue(this UserDataSources userDataSources, string column, float value,
                                    NumberFormatInfo numberFormatInfo)
        {
            userDataSources.Item(column).ValueEx = value.ToString(numberFormatInfo);
        }

        public static void SetValue(this UserDataSources userDataSources, string column, float value)
        {
            userDataSources.Item(column).ValueEx = value.ToString();
        }

        public static DateTime GetDateTimeValue(this UserDataSources userDataSources, string column)
        {
            return DateTime.ParseExact(userDataSources.Item(column).ValueEx, "yyyyMMdd", null);
        }

        public static float GetSingleValue(this UserDataSources userDataSources, string column,
                                           NumberFormatInfo numberFormatInfo)
        {
            return float.Parse(userDataSources.Item(column).ValueEx, numberFormatInfo);
        }

        public static float GetSingleValue(this UserDataSources userDataSources, string column)
        {
            return float.Parse(userDataSources.Item(column).ValueEx);
        }
    }
}