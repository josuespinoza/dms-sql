using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace DMS_Connector
{
    public static class Company
    {
        public static SAPbobsCOM.Company CompanySBO { get; set; }
        public static Application ApplicationSBO { get; set; }
        public static CompanyService CompanyService { get; set; }
        public static AdminInfo AdminInfo { get; set; }

         
        public static string StrConectionString { get; set; }

        ///// <summary>
        ///// Función para conexión inicial del Addon
        ///// </summary>
        ///// <param name="p_strDatabaseName">Base de Datos a conectar</param>
        ///// <param name="p_strServerName">Servidor de Base de Datos a conectar</param>
        ///// <param name="p_strLicenseServer">Servidor de Licencias a Conectar</param>
        ///// <param name="p_strUsuarioSBO">Usuario de SAP</param>
        ///// <param name="p_strContrasenaSBO">Contraseña de SAP</param>
        ///// <param name="p_strDBUser">Usuario de SQL</param>
        ///// <param name="p_strDBPassword">Contraseña de SQL</param>
        ///// <param name="p_intTipoServidor">Tipo de Servidor a conectar</param>
        ///// <returns></returns>
        //public static int ConnectCompany(string p_strDatabaseName, string p_strServerName, string p_strLicenseServer,
        //    string p_strUsuarioSBO, string p_strContrasenaSBO, string p_strDBUser, string p_strDBPassword,
        //    int p_intTipoServidor)
        //{
        //    int intStatus;
        //    //Helpers.CrearConectionString(p_strDatabaseName, p_strServerName, p_strDBUser, p_strDBPassword);
        //    CompanySBO = new SAPbobsCOM.Company
        //    {
        //        CompanyDB = p_strDatabaseName,
        //        Server = p_strServerName,
        //        LicenseServer = p_strLicenseServer,
        //        UserName = p_strUsuarioSBO,
        //        Password = p_strContrasenaSBO,
        //        DbUserName = p_strDBUser,
        //        DbPassword = p_strDBPassword,
        //        DbServerType = (BoDataServerTypes)p_intTipoServidor
        //    };
        //    intStatus = CompanySBO.Connect();
        //    if (intStatus == 0)
        //    {
        //        CompanyService = CompanySBO.GetCompanyService();
        //        AdminInfo = CompanyService.GetAdminInfo();
        //    }
        //    return intStatus;
        //}

        /// <summary>
        /// Función para conexión inicial del Addon
        /// </summary>
        /// <param name="p_strDatabaseName">Base de Datos a conectar</param>
        /// <param name="p_strServerName">Servidor de Base de Datos a conectar</param>
        /// <param name="p_strLicenseServer">Servidor de Licencias a Conectar</param>
        /// <param name="p_strUsuarioSBO">Usuario de SAP</param>
        /// <param name="p_strContrasenaSBO">Contraseña de SAP</param>
        /// <param name="p_strDBUser">Usuario de SQL</param>
        /// <param name="p_strDBPassword">Contraseña de SQL</param>
        /// <param name="p_intTipoServidor">Tipo de Servidor a conectar</param>
        /// <returns></returns>
        public static int ConnectCompany(string p_strDatabaseName, string p_strServerName, string p_strLicenseServer,
            string p_strUsuarioSBO, string p_strContrasenaSBO, string p_strDBUser, string p_strDBPassword,
            int p_intTipoServidor, string p_strSingleSignOn)
        {
            int intStatus;
         
            //Valida el modelo de conexion a utilizar
            if (p_strSingleSignOn.ToUpper() == "Y")
            {
                //Conexion Single Sign On utilizando el UI API
                //compatible con modelo de SAP 9.2
                CompanySBO = new SAPbobsCOM.Company();
                string cookie = CompanySBO.GetContextCookie();
                string conString = ApplicationSBO.Company.GetConnectionContext(cookie);
                intStatus = CompanySBO.SetSboLoginContext(conString);

                if (intStatus != 0)
                {
                    throw new Exception(string.Format("{0}-{1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
                }

                intStatus = CompanySBO.Connect();
                if (intStatus != 0)
                {
                    throw new Exception(string.Format("{0}-{1}", CompanySBO.GetLastErrorCode(), CompanySBO.GetLastErrorDescription()));
                }
                else
                {
                    CompanyService = CompanySBO.GetCompanyService();
                    AdminInfo = CompanyService.GetAdminInfo();
                }
            }
            else
            {
                //Conexion Standard a través del DI API
                //compatible con SAP 9.1, 9.0 y 8
                CompanySBO = new SAPbobsCOM.Company
                {
                    CompanyDB = p_strDatabaseName,
                    Server = p_strServerName,
                    LicenseServer = p_strLicenseServer,
                    UserName = p_strUsuarioSBO,
                    Password = p_strContrasenaSBO,
                    DbUserName = p_strDBUser,
                    DbPassword = p_strDBPassword,
                    DbServerType = (BoDataServerTypes)p_intTipoServidor
                };

                intStatus = CompanySBO.Connect();

                if (intStatus == 0)
                {
                    CompanyService = CompanySBO.GetCompanyService();
                    AdminInfo = CompanyService.GetAdminInfo();
                }
            }

            return intStatus;

        }
    }
}
