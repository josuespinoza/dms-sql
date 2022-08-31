using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class CustomersDataTable
    {
        public IDataTable DataTable { get; set; }

        public CustomersDataTable(IDataTable dataTable)
        {
            DataTable = dataTable;
        }

        public string CardCode
        {
            get { return DataTable.GetValue("CardCode", 0).ToString(); }
            set { DataTable.SetValue("CardCode", 0, value); }
        }

        public string CardName
        {
            get { return DataTable.GetValue("CardName", 0).ToString(); }
            set { DataTable.SetValue("CardName", 0, value); }
        }
    }
}