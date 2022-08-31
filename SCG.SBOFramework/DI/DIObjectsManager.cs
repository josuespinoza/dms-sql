using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public class DIObjectsManager
    {
        public ICompany Company { get; set; }

        public DIObjectsManager(ICompany company)
        {
            Company = company;
        }

        public UserDefinedFieldsManager GetUserDefinedFieldsManager()
        {
            return new UserDefinedFieldsManager(Company, (UserFieldsMD)Company.GetBusinessObject(BoObjectTypes.oUserFields));
        }

        public UserDefinedObjectsManager GetUserDefinedObjectsManager()
        {
            return new UserDefinedObjectsManager(Company, (UserObjectsMD)Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD));
        }

        public UserDefinedTablesManager GetUserDefinedTablesManager()
        {
            return new UserDefinedTablesManager(Company, (UserTablesMD) Company.GetBusinessObject(BoObjectTypes.oUserTables));
        }
    }
}