using System.Data.EntityClient;
using System.Data.SqlClient;

namespace SCG.SBOFramework.DI
{
    /// <summary>
    /// Structure containing the information used to connect to de SBO company.
    /// <seealso cref="DIConnectionManager.Connect(SCG.SBOFramework.DI.CompanyConnectionInfo)"/>
    /// </summary>
    public struct CompanyConnectionInfo
    {
        /// <summary>
        /// Sets or returns the database server to which the object connects. 
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Sets or returns the name of the company database .
        /// </summary>
        public string CompanyDataBase { get; set; }

        /// <summary>
        /// Sets or returns the user ID, which is used for log on to the SAP Business One application. 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Sets or returns the SAP Business One password issued to the user. 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sets or returns the user name for establishing a connection to the database server. 
        /// The field is not mandatory, as the database credentials are stored in the license server and you can use these values
        /// instead.
        /// </summary>
        public string DataBaseUserName { get; set; }

        /// <summary>
        /// Sets or returns the password for establishing a connection to the database server. 
        /// The field is not mandatory, as the database credentials are stored in the license server and you can use these values
        /// instead.
        /// </summary>
        public string DataBaseUserPassword { get; set; }

        /// <summary>
        /// Sets or returns a Boolean value that specifies whether the Company object uses NT authentication, 
        /// or the internal SQL Server user ObsCommon, to establish a connection with the SQL Server.
        /// </summary>
        public bool UseTrustedConnection { get; set; }

        /// <summary>
        /// The license server name and port for connecting to the company database. The value is in the format <b>myServer:30000</b>. 
        /// If no value is given, the default license server and port are used. If a server is given but no port is given, 30000 is used
        /// for the port.
        /// </summary>
        public string LicenseServer { get; set; }

        /// <summary>
        /// The database type.
        /// </summary>
        public DataBaseServerType DataBaseServerType { get; set; }

        /// <summary>
        /// Sets or returns a string identifier that your add-on must use to connect to SAP Business One database.
        /// </summary>
        public string AddonIndentifier { get; set; }

        /// <summary>
        /// Returns a Sql Connection String
        /// </summary>
        /// <returns></returns>
        public string GetSqlConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
                                                                     {
                                                                         DataSource = Server,
                                                                         InitialCatalog = CompanyDataBase,
                                                                         IntegratedSecurity = UseTrustedConnection,
                                                                         UserID = DataBaseUserName,
                                                                         Password = DataBaseUserPassword,
                                                                     };
            return connectionStringBuilder.ToString();
        }

        /// <summary>
        /// Return a EntityFramework Connection String using the SqlClient provider.
        /// </summary>
        /// <param name="entityMetada">
        /// Entity Metadata
        /// </param>
        /// <returns></returns>
        public string GetEntityFrameworkConnectionString(string entityMetada)
        {
            EntityConnectionStringBuilder entityConnectionStringBuilder = new EntityConnectionStringBuilder
                                                                              {
                                                                                  Provider = "System.Data.SqlClient",
                                                                                  ProviderConnectionString =
                                                                                      GetSqlConnectionString()
                                                                                  ,
                                                                                  Metadata = entityMetada
                                                                              };
            return entityConnectionStringBuilder.ToString();
        }
    }
}