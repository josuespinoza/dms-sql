using System.Collections.Generic;
using SAPbobsCOM;
using SCG.SBOFramework.UI;

namespace SCG.SBOFramework.DI
{
    public class DIConnectionManager
    {
        public ICompany Company { get; protected set; }
        public CompanyConnectionInfo CompanyConnectionInfo { get; protected set; }

        public void Connect(CompanyConnectionInfo connectionInfo)
        {
            Company = new Company
                          {
                              Server = connectionInfo.Server,
                              CompanyDB = connectionInfo.CompanyDataBase,
                              UserName = connectionInfo.UserName,
                              Password = connectionInfo.Password,
                              DbUserName = connectionInfo.DataBaseUserName,
                              DbPassword = connectionInfo.DataBaseUserPassword,
                              LicenseServer = connectionInfo.LicenseServer,
                              DbServerType = (BoDataServerTypes) connectionInfo.DataBaseServerType,
                              UseTrusted = connectionInfo.UseTrustedConnection,
                              AddonIdentifier = connectionInfo.AddonIndentifier
                          }
                ;
            int code = Company.Connect();
            if (code != 0)
            {
                throw new SboUncessfullOperationException(code, Company.GetLastErrorDescription(), "Company.Connect");
            }
        }

        /// <summary>
        /// Connects using Single Sign On
        /// </summary>
        /// <param name="uiConnectionManager">UI Connection provider</param>
        public void Connect(UIConnectionManager uiConnectionManager)
        {
            Company = new Company();
            string contextCookie = Company.GetContextCookie();
            string connectionContext = uiConnectionManager.Application.Company.GetConnectionContext(contextCookie);
            if (Company.Connected)
                Company.Disconnect();
            int sboLoginContext = Company.SetSboLoginContext(connectionContext);
            if (sboLoginContext != 0)
                throw new SboUncessfullOperationException(sboLoginContext, Company.GetLastErrorDescription(),
                                                          "Company.SetSboLoginContext");
        }

        public void Disconnect()
        {
            if (Company.Connected)
                Company.Disconnect();
        }

        public static List<string> CompanyList(CompanyConnectionInfo connectionInfo)
        {
            ICompany company = new Company
                                   {
                                       Server = connectionInfo.Server,
                                       CompanyDB = connectionInfo.CompanyDataBase,
                                       UserName = connectionInfo.UserName,
                                       Password = connectionInfo.Password,
                                       DbUserName = connectionInfo.DataBaseUserName,
                                       DbPassword = connectionInfo.DataBaseUserPassword,
                                       LicenseServer = connectionInfo.LicenseServer,
                                       DbServerType = (BoDataServerTypes) connectionInfo.DataBaseServerType,
                                       UseTrusted = connectionInfo.UseTrustedConnection,
                                       AddonIdentifier = connectionInfo.AddonIndentifier
                                   };

            Recordset recordset = company.GetCompanyList();
            var result = new List<string>();
            while (!recordset.EoF)
            {
                result.Add(recordset.Fields.Item(0).Value.ToString());
                recordset.MoveNext();
            }
            return result;
        }
    }
}