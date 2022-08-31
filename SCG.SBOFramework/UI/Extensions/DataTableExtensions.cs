using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI.Extensions
{
    public static class DataTableExtensions
    {
        public static double GetDoubleValue(this DataTable dataTable, string column, int rowIndex)
        {
            return (double) dataTable.GetValue(column, rowIndex);
        }

        public static DateTime GetDateTimeValue(this DataTable dataTable, string column, int rowIndex)
        {
            return (DateTime) dataTable.GetValue(column, rowIndex);
        }
    }
}
