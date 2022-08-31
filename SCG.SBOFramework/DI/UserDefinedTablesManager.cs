using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public class UserDefinedTablesManager
    {
        protected UserTablesMD SBOUserTablesMD { get; set; }
        protected ICompany Company { get; set; }

        /// <summary>
        /// Sets or returns the name for the user defined table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A string that describes the name and functionality of the table.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sets or returns type of the user table.
        /// </summary>
        public UserTableType TableType { get; set; }

        public UserDefinedTablesManager(ICompany company, UserTablesMD sboUserTablesMD)
        {
            Company = company;
            SBOUserTablesMD = sboUserTablesMD;
        }

        /// <summary>
        /// Deletes a specified table.
        /// </summary>
        public void Remove()
        {
            SBOUserTablesMD.GetByKey(Name);
            var code = SBOUserTablesMD.Remove();
            SBOUserTablesMD.ReleaseComObject();
            if (code != 0)
                throw new SboUncessfullOperationException(code, Company.GetLastErrorDescription(), "UserTablesMD.Remove");
        }

    }
}