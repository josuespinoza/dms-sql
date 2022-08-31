using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class UIConnectionManager
    {
        public string AddonIdentifier { get; set; }
        protected string ConnectionString { get; set; }
        public IApplication Application { get; protected set; }

        public UIConnectionManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SetApplication()
        {
            SboGuiApi sboGuiApi = new SboGuiApi();
            sboGuiApi.Connect(ConnectionString);
            Application = sboGuiApi.GetApplication();
        }

    }
}