using System;
using System.Globalization;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI.Extensions
{
    public static class DataSourceExtensions
    {
        public static void SetValue(this DBDataSource dbDataSource, string fieldName, int rowIndex, DateTime date)
        {
            dbDataSource.SetValue(fieldName, rowIndex, date.ToString("yyyyMMdd"));
        }

        public static void SetValue(this DBDataSource dbDataSource, string fieldName, int rowIndex, float value,
                                    NumberFormatInfo numberFormatInfo)
        {
            dbDataSource.SetValue(fieldName, rowIndex, value.ToString(numberFormatInfo));
        }

        public static void SetValue(this DBDataSource dbDataSource, string fieldName, int rowIndex, float value)
        {
            dbDataSource.SetValue(fieldName, rowIndex, value.ToString());
        }

        public static DateTime GetDateTimeValue(this DBDataSource dbDataSource, string fieldName, int rowIndex)
        {
            return DateTime.ParseExact(dbDataSource.GetValue(fieldName, rowIndex), "yyyyMMdd", null);
        }

        public static float GetSingleValue(this DBDataSource dbDataSource, string fieldName, int rowIndex,
                                           NumberFormatInfo numberFormatInfo)
        {
            return float.Parse(dbDataSource.GetValue(fieldName, rowIndex), numberFormatInfo);
        }

        public static float GetSingleValue(this DBDataSource dbDataSource, string fieldName, int rowIndex)
        {
            return float.Parse(dbDataSource.GetValue(fieldName, rowIndex));
        }
    }
}