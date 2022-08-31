using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public class UserDefinedObjectsManager
    {
        public ICompany Company { get; protected set; }
        public UserObjectsMD SboUserObjectsMD { get; set; }

        /// <summary>
        /// Sets or returns the Object Unique ID. The Unique ID is the primary key of the user defined object and its child objects.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="company"></param>
        /// <param name="sboUserObjectsMD"></param>
        public UserDefinedObjectsManager(ICompany company, UserObjectsMD sboUserObjectsMD)
        {
            Company = company;
            SboUserObjectsMD = sboUserObjectsMD;
        }

        /// <summary>
        /// Deletes the specified UDO.
        /// </summary>
        public void Remove()
        {
            SboUserObjectsMD.GetByKey(Code);
            var code = SboUserObjectsMD.Remove();
            SboUserObjectsMD.ReleaseComObject();
            if (code != 0)
                throw new SboUncessfullOperationException(code, Company.GetLastErrorDescription(), "UserObjectsMD.Remove");

        }
    }
}