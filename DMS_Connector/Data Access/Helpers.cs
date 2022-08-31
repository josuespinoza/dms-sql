using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria;
using DMS_Connector.Business_Logic.DataContract.SAPDocumento;
using DMS_Connector.Data_Access;
using SAPbobsCOM;
using SAPbouiCOM;
using DataTable = System.Data.DataTable;
using Items = SAPbobsCOM.Items;

namespace DMS_Connector
{
    public class Helpers
    {
        /// <summary>
        /// Función que ejecuta consulta para obtener valor específico
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>String con resultado</returns>
        public static string EjecutarConsulta(string p_strConsulta)
        {
            Recordset rSet = null;
            string strValor = "";
            try
            {
                rSet = (Recordset)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                rSet.DoQuery(p_strConsulta);
                if (rSet.RecordCount != 0)
                    strValor = rSet.Fields.Item(0).Value.ToString();
                return strValor;
                //if (Company.CompanySBO.DbServerType != BoDataServerTypes.dst_HANADB)
                //    return EjecutarConsultaSQL(p_strConsulta);
                //return EjecutarConsultaHANA(p_strConsulta);
                //SAPbouiCOM.DataTable dtDataTableSbo = GetDataTable("dtEjecutarConsulta");
                //dtDataTableSbo.ExecuteQuery(p_strConsulta);
                //if (!string.IsNullOrEmpty(dtDataTableSbo.GetValue(0, 0).ToString()))
                //    return dtDataTableSbo.GetValue(0, 0).ToString();
                //return "";
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                throw;
            }
            finally
            {
                DestruirObjeto(ref rSet);
            }

        }

        /// <summary>
        /// Función que carga las líneas y posiciones de una cotización para ser manipulada
        /// </summary>
        /// <param name="p_intDocEntry">DocEntry de la cotización</param>
        /// <param name="p_sapDocuments">Parametro opcional para cargar la cotización</param>
        /// <returns>Entidad de la cotización</returns>
        public static oDocumento CargaCotizacionConVisOrder(int p_intDocEntry, ref Documents p_sapDocuments)
        {
            oDocumento oDocumento = null;
            Documents sapDocuments = null;
            try
            {
                sapDocuments = GetQuotation(p_intDocEntry);
                if (sapDocuments != null)
                {
                    oDocumento = new oDocumento();
                    oDocumento.Lineas = new List<oLineasDocumento>();
                    oDocumento.DocEntry = sapDocuments.DocEntry;
                    oDocumento.CardCode = sapDocuments.CardCode;
                    for (int index = 0; index < sapDocuments.Lines.Count; index++)
                    {
                        sapDocuments.Lines.SetCurrentLine(index);
                        oDocumento.Lineas.Add(new oLineasDocumento
                        {
                            LineNum = sapDocuments.Lines.LineNum,
                            ID = sapDocuments.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim(),
                            intPosicion = sapDocuments.Lines.VisualOrder
                        });
                    }
                    p_sapDocuments = sapDocuments;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // DestruirObjeto(ref sapDocuments);
            }
            return oDocumento;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strConsulta"></param>
        /// <returns></returns>
        private static string EjecutarConsultaSQL(string p_strConsulta)
        {
            SqlCommand cmdEjecutarConsulta = new SqlCommand();
            string strValor = "";
            using (SqlConnection cnConeccion = new SqlConnection())
            {
                cnConeccion.ConnectionString = Company.StrConectionString;
                cnConeccion.Open();
                cmdEjecutarConsulta.Connection = cnConeccion;
                cmdEjecutarConsulta.CommandType = CommandType.Text;
                cmdEjecutarConsulta.CommandText = p_strConsulta;
                using (SqlDataReader drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader())
                {
                    while (drdResultadoConsulta.Read())
                    {
                        if (!ReferenceEquals(drdResultadoConsulta[0], DBNull.Value))
                        {
                            strValor = drdResultadoConsulta[0].ToString();
                            break;
                        }
                    }
                }
            }
            return strValor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strConsulta"></param>
        /// <returns></returns>
        //private static string EjecutarConsultaHANA(string p_strConsulta)
        //{
        //    HanaCommand cmdEjecutarConsulta = new HanaCommand();
        //    string strValor = "";
        //    using (HanaConnection cnConeccion = new HanaConnection())
        //    {
        //        cnConeccion.ConnectionString = Company.StrConectionString;
        //        cnConeccion.Open();
        //        cmdEjecutarConsulta.Connection = cnConeccion;
        //        cmdEjecutarConsulta.CommandType = CommandType.Text;
        //        cmdEjecutarConsulta.CommandText = p_strConsulta;
        //        using (HanaDataReader drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader())
        //        {
        //            while (drdResultadoConsulta.Read())
        //            {
        //                if (!ReferenceEquals(drdResultadoConsulta[0], DBNull.Value))
        //                {
        //                    strValor = drdResultadoConsulta[0].ToString();
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return strValor;
        //}

        /// <summary>
        /// Función que ejecuta consulta para obtener valor específico en formato decimal
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>Decimal con resultado</returns>
        public static decimal EjecutarConsultaDecimal(string p_strConsulta)
        {
            Recordset rSet = null;
            decimal decValor = 0;
            try
            {
                rSet = (Recordset)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                rSet.DoQuery(p_strConsulta);
                if (rSet.RecordCount != 0)
                {
                    decValor = Convert.ToDecimal(rSet.Fields.Item(0).Value);
                }
                return decValor;
                //if (Company.CompanySBO.DbServerType != BoDataServerTypes.dst_HANADB)
                //    return EjecutarConsultaDecimalSQL(p_strConsulta);
                //return EjecutarConsultaDecimalHANA(p_strConsulta);
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                throw;
            }
            finally
            {
                DestruirObjeto(ref rSet);
            }
        }

        /// <summary>
        /// Función que ejecuta consulta para obtener valor específico en formato double
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>Double con resultado</returns>
        public static double EjecutarConsultaDouble(string p_strConsulta)
        {
            Recordset rSet = null;
            double dblValor = 0;
            try
            {
                rSet = (Recordset)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                rSet.DoQuery(p_strConsulta);
                if (rSet.RecordCount != 0)
                {
                    dblValor = Convert.ToDouble(rSet.Fields.Item(0).Value);
                }
                return dblValor;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                throw;
            }
            finally
            {
                DestruirObjeto(ref rSet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStrConsulta"></param>
        /// <returns></returns>
        //private static decimal EjecutarConsultaDecimalHANA(string pStrConsulta)
        //{
        //    HanaCommand cmdEjecutarConsulta = new HanaCommand();
        //    decimal decValor = 0;
        //    using (HanaConnection cnConeccion = new HanaConnection())
        //    {
        //        cnConeccion.ConnectionString = Company.StrConectionString;
        //        cnConeccion.Open();
        //        cmdEjecutarConsulta.Connection = cnConeccion;
        //        cmdEjecutarConsulta.CommandType = CommandType.Text;
        //        cmdEjecutarConsulta.CommandText = pStrConsulta;
        //        using (HanaDataReader drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader())
        //        {
        //            while (drdResultadoConsulta.Read())
        //            {
        //                if (!ReferenceEquals(drdResultadoConsulta[0], DBNull.Value))
        //                {
        //                    decValor = Convert.ToDecimal(drdResultadoConsulta[0]);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return decValor;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStrConsulta"></param>
        /// <returns></returns>
        private static decimal EjecutarConsultaDecimalSQL(string pStrConsulta)
        {
            SqlCommand cmdEjecutarConsulta = new SqlCommand();
            decimal decValor = 0;
            using (SqlConnection cnConeccion = new SqlConnection())
            {
                cnConeccion.ConnectionString = Company.StrConectionString;
                cnConeccion.Open();
                cmdEjecutarConsulta.Connection = cnConeccion;
                cmdEjecutarConsulta.CommandType = CommandType.Text;
                cmdEjecutarConsulta.CommandText = pStrConsulta;
                using (SqlDataReader drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader())
                {
                    while (drdResultadoConsulta.Read())
                    {
                        if (!ReferenceEquals(drdResultadoConsulta[0], DBNull.Value))
                        {
                            decValor = Convert.ToDecimal(drdResultadoConsulta[0]);
                            break;
                        }
                    }
                }
            }
            return decValor;
        }

        /// <summary>
        /// Función que retorna DataTable basado en consulta realizada
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>DataTable con resultado</returns>
        public static DataTable EjecutarConsultaDataTable(string p_strConsulta)
        {
            Recordset rSet = null;
            try
            {
                rSet = (Recordset)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                rSet.DoQuery(p_strConsulta);
                return ConvertDataTableSboToSystem(rSet);
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                throw;
            }
            finally
            {
                DestruirObjeto(ref rSet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strConsulta"></param>
        /// <returns></returns>
        private static DataTable EjecutarConsultaDataTableSQL(string p_strConsulta)
        {
            SqlDataReader drdResultadoConsulta;
            SqlCommand cmdEjecutarConsulta;
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn_Coneccion = new SqlConnection(Company.StrConectionString))
                {
                    cn_Coneccion.Open();
                    using (cmdEjecutarConsulta = new SqlCommand())
                    {
                        cmdEjecutarConsulta.Connection = cn_Coneccion;
                        cmdEjecutarConsulta.CommandType = CommandType.Text;
                        cmdEjecutarConsulta.CommandText = p_strConsulta;
                        drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader();
                        dt.Load(drdResultadoConsulta);
                    }
                }
            }
            catch
            {
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strConsulta"></param>
        /// <returns></returns>
        //private static DataTable EjecutarConsultaDataTableHANA(string p_strConsulta)
        //{
        //    HanaDataReader drdResultadoConsulta;
        //    HanaCommand cmdEjecutarConsulta;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        using (HanaConnection cn_Coneccion = new HanaConnection(Company.StrConectionString))
        //        {
        //            cn_Coneccion.Open();
        //            using (cmdEjecutarConsulta = new HanaCommand())
        //            {
        //                cmdEjecutarConsulta.Connection = cn_Coneccion;
        //                cmdEjecutarConsulta.CommandType = CommandType.Text;
        //                cmdEjecutarConsulta.CommandText = p_strConsulta;
        //                drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader();
        //                dt.Load(drdResultadoConsulta);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return dt;
        //}

        /// <summary>
        /// Función que convierte DataTable de SAP en DataTable de System
        /// </summary>
        /// <param name="p_rSet">RocordSet a convertir</param>
        /// <returns>System.DataTablr</returns>
        public static DataTable ConvertDataTableSboToSystem(Recordset p_rSet)
        {
            bool blnLineaVacia;
            DataTable dtDataTableSystem = new DataTable();
            DataRow dtDataRow;

            for (int index = 0; index < p_rSet.Fields.Count; index++)
            {
                switch (p_rSet.Fields.Item(index).Type)
                {
                    case BoFieldTypes.db_Alpha:
                    case BoFieldTypes.db_Memo:
                        dtDataTableSystem.Columns.Add(p_rSet.Fields.Item(index).Name, typeof(string));
                        dtDataTableSystem.Columns[0].AllowDBNull = true;
                        break;
                    case BoFieldTypes.db_Date:
                        dtDataTableSystem.Columns.Add(p_rSet.Fields.Item(index).Name, typeof(DateTime));
                        dtDataTableSystem.Columns[0].AllowDBNull = true;
                        break;
                    case BoFieldTypes.db_Numeric:
                        dtDataTableSystem.Columns.Add(p_rSet.Fields.Item(index).Name, typeof(int));
                        dtDataTableSystem.Columns[0].AllowDBNull = true;
                        break;
                    case BoFieldTypes.db_Float:
                        dtDataTableSystem.Columns.Add(p_rSet.Fields.Item(index).Name, typeof(double));
                        dtDataTableSystem.Columns[0].AllowDBNull = true;
                        break;
                }

            }

            while (!p_rSet.EoF)
            {
                dtDataRow = dtDataTableSystem.NewRow();
                for (int index = 0; index < p_rSet.Fields.Count; index++)
                    if (p_rSet.Fields.Item(index).Value != null)
                        dtDataRow[p_rSet.Fields.Item(index).Name] = p_rSet.Fields.Item(index).Value;
                dtDataTableSystem.Rows.Add(dtDataRow);
                p_rSet.MoveNext();
            }
            //if (dtDataTableSystem.Rows.Count == 1)
            //{
            //    blnLineaVacia = true;
            //    foreach (System.Data.DataColumn dataColumn in dtDataTableSystem.Columns)
            //    {
            //        string sfd = dtDataTableSystem.Rows[0][dataColumn.ColumnName].ToString();
            //        if (string.IsNullOrEmpty(sfd.ToString())) sfd = "0";
            //        if (sfd != "0")
            //        {
            //            blnLineaVacia = false;
            //            break;
            //        }
            //    }
            //    if (blnLineaVacia)
            //        dtDataTableSystem.Rows.Clear();
            //}

            return dtDataTableSystem;

        }

        /// <summary>
        /// Función que retorna instancia de DataTable de SAP
        /// </summary>
        /// <param name="p_strDataTable">Nombre de DataTable a consultar</param>
        /// <returns>Instancia de DataTable</returns>
        private static SAPbouiCOM.DataTable GetDataTable(string p_strDataTable)
        {
            if (ValidaSiDataTableExiste(Company.ApplicationSBO.Forms.Item(0), p_strDataTable))
                return Company.ApplicationSBO.Forms.Item(0).DataSources.DataTables.Item(p_strDataTable);
            return Company.ApplicationSBO.Forms.Item(0).DataSources.DataTables.Add(p_strDataTable);
        }

        /// <summary>
        /// Valida si existe un data table con el nombre especificado
        /// </summary>
        /// <param name="p_oForm">Formulario SBO</param>
        /// <param name="dtName">Nombre Data Table a consultar</param>
        /// <returns>Bool que indica si existe o no un data table con ese nombre</returns>
        public static Boolean ValidaSiDataTableExiste(Form p_oForm, string dtName)
        {
            var result = false;
            for (int i = 0; i < p_oForm.DataSources.DataTables.Count; i++)
            {
                if (p_oForm.DataSources.DataTables.Item(i).UniqueID == dtName)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Libera memoria al destruir objetos despues de su uso
        /// </summary>
        /// <param name="objDocumento">Objeto a destruir</param>
        public static void DestruirObjeto<T>(ref T objDocumento)
        {
            if ((objDocumento != null))
            {
                Marshal.ReleaseComObject(objDocumento);
                objDocumento = default(T);
            }
        }

        /// <summary>
        /// Retorna moneda Local y Sistema de la compañía
        /// </summary>
        /// <param name="p_strLocalCurrency">Variable que almacena moneda local</param>
        /// <param name="p_strSysCurrency">Variable que almacena moneda sistema</param>
        public static void GetCurrencies(ref string p_strLocalCurrency, ref string p_strSysCurrency)
        {
            p_strLocalCurrency = Company.AdminInfo.LocalCurrency;
            p_strSysCurrency = Company.AdminInfo.SystemCurrency;
        }

        /// <summary>
        /// Función que retorna tipo de cambio de una fecha específica
        /// </summary>
        /// <param name="p_strCurrency">Moneda a consultar</param>
        /// <param name="p_dtDate">Fecha a consultar</param>
        /// <returns>Double con tipo de cambio</returns>
        public static double GetCurrencyRate(string p_strCurrency, DateTime p_dtDate)
        {
            SBObob sbObob = null;
            Recordset rSet = null;
            double dbRate;
            try
            {
                dbRate = 0;
                sbObob = (SBObob)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoBridge);
                rSet = (Recordset)Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
                rSet = sbObob.GetCurrencyRate(p_strCurrency, p_dtDate);
                if (rSet.RecordCount > 0)
                    dbRate = Convert.ToDouble(rSet.Fields.Item(0).Value);
                return dbRate;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                DestruirObjeto(ref rSet);
                DestruirObjeto(ref sbObob);
            }
        }

        /// <summary>
        /// Retorna separadores de numeros en SAP
        /// </summary>
        /// <param name="p_strSeparadorMiles">Variable que almacena Separador de Miles</param>
        /// <param name="p_strSeparadorDecimales">Variable que almacena Separador de Decimales</param>
        public static void GetSeparadoresSAP(ref string p_strSeparadorMiles, ref string p_strSeparadorDecimales)
        {
            p_strSeparadorMiles = Company.AdminInfo.ThousandsSeparator;
            p_strSeparadorDecimales = Company.AdminInfo.DecimalSeparator;
        }

        /// <summary>
        /// Función que retorna Nombre de una Moneda
        /// </summary>
        /// <param name="p_strCode">Code de Moneda a consultar</param>
        /// <returns>String con nombre de moneda</returns>
        public static string GetCurrencyName(string p_strCode)
        {
            Currencies currencies = default(Currencies);
            string strValor = string.Empty;
            try
            {
                currencies = (Currencies)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oCurrencyCodes);
                if (currencies.GetByKey(p_strCode))
                    strValor = currencies.Name;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
            }
            finally
            {
                DestruirObjeto(ref currencies);
            }
            return strValor;
        }

        /// <summary>
        /// Función que retorna el campo solicitado de un socio de negocios
        /// </summary>
        /// <param name="p_strCardCode">Código del Socio de Negocios</param>
        /// <param name="p_strField">Campo solicitado del Socio de negocios</param>
        /// <returns>Campo solicitado del socio de negocios</returns>
        /// <remarks></remarks>
        public static string GetFieldBP(string p_strCardCode, string p_strField)
        {
            BusinessPartners oBusinessPartners = default(BusinessPartners);
            string strField;
            try
            {
                strField = string.Empty;
                oBusinessPartners = (BusinessPartners)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (oBusinessPartners.GetByKey(p_strCardCode))
                {
                    switch (p_strField)
                    {
                        case "Currency":
                            strField = oBusinessPartners.Currency;
                            break;
                        case "ListNum":
                            strField = oBusinessPartners.PriceListNum.ToString();
                            break;
                        case "U_SCGD_Dealer":
                            if (oBusinessPartners.UserFields.Fields.Item("U_SCGD_Dealer").Value != null)
                                strField = oBusinessPartners.UserFields.Fields.Item("U_SCGD_Dealer").Value.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                strField = string.Empty;
            }
            finally
            {
                DestruirObjeto(ref oBusinessPartners);
            }
            return strField;
        }

        /// <summary>
        /// Metodo que consulta parametros de inicialización del Addon
        /// </summary>
        /// <param name="p_strUserSAP">Variable que almacena usuario de SAP</param>
        /// <param name="p_str_PasswordSAP">Variable que almacena contraseña de SAP</param>
        /// <param name="p_strUserSQL">Variable que almacena usuario de SQL</param>
        /// <param name="p_strPasswordSQL">Variable que almacena contraseña de SQL</param>
        /// <param name="p_intTypeServer">Variable que almacena tipo de Servidor</param>
        /// <param name="p_strLicenseServer">Variable que almacena servidor de licencias</param>
        /// <returns>Bool Verdadero si encontro parámetros</returns>
        public static bool GetUserAndPassword(ref string p_strUserSAP, ref string p_str_PasswordSAP, ref string p_strUserSQL, ref string p_strPasswordSQL, ref int p_intTypeServer, ref string p_strLicenseServer, ref string p_strSingleSignOn)
        {
            SAPbouiCOM.DataTable dtTable;
            dtTable = GetDataTable("UserAndPassword");
            dtTable.ExecuteQuery("SELECT \"Code\", \"U_UserSAP\", \"U_PasswordSAP\", \"U_UserDB\", \"U_PasswordDB\", \"U_ServerType\", \"U_LicenseServer\", \"U_SingleSignOn\" FROM \"@SCGD_PARAMCONEXION\" WHERE \"Code\" = 'DMS'");
            if (dtTable.Rows.Count > 0 && !string.IsNullOrEmpty(dtTable.GetValue("Code", 0).ToString()))
            {
                p_strUserSAP = dtTable.GetValue("U_UserSAP", 0).ToString();
                p_str_PasswordSAP = dtTable.GetValue("U_PasswordSAP", 0).ToString();
                p_strUserSQL = dtTable.GetValue("U_UserDB", 0).ToString();
                p_strPasswordSQL = dtTable.GetValue("U_PasswordDB", 0).ToString();
                p_intTypeServer = Convert.ToInt32(dtTable.GetValue("U_ServerType", 0));
                p_strLicenseServer = dtTable.GetValue("U_LicenseServer", 0).ToString();
                if (!string.IsNullOrEmpty(dtTable.GetValue("U_SingleSignOn", 0).ToString()))
                {
                    p_strSingleSignOn = dtTable.GetValue("U_SingleSignOn", 0).ToString();
                }
                else
                {
                    p_strSingleSignOn = "N";
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Función que retorna Fecha del Servidor
        /// </summary>
        /// <returns>DateTime con fecha del servidor</returns>
        public static DateTime GetDBServerDate()
        {
            try
            {
                return Company.CompanySBO.GetCompanyDate();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo que asigna la cultura según región
        /// </summary>
        /// <param name="currentUiCulture">Información de cultura del sitema</param>
        /// <param name="cultureInfo">Información de cultura del archivo de recursos</param>
        public static void SetCulture(ref CultureInfo currentUiCulture, ref CultureInfo cultureInfo)
        {
            switch (Company.ApplicationSBO.Language)
            {
                case BoLanguages.ln_English:
                case BoLanguages.ln_English_Cy:
                case BoLanguages.ln_English_Gb:
                case BoLanguages.ln_English_Sg:
                    currentUiCulture = new CultureInfo("en-Us");
                    cultureInfo = currentUiCulture;
                    break;
                default:
                    currentUiCulture = new CultureInfo("es-Cr");
                    cultureInfo = currentUiCulture;
                    break;
            }
        }

        /// <summary>
        /// Retorna booleano con condición de mostrar menu a un usuario
        /// </summary>
        /// <param name="pStrMenu">Menú a consultar</param>
        /// <param name="pStrUsuario">Usuario a consultar</param>
        /// <returns>Boolenano con permiso</returns>
        public static bool PermisosMenu(string pStrMenu)
        {
            DataTable dtMostrarMenu = default(DataTable);

            if (Configuracion.LtPermisosMenu == null)
            {
                Configuracion.LtPermisosMenu = new List<string>();
                dtMostrarMenu = EjecutarConsultaDataTable(string.Format(Queries.GetStrSpecificQuery("strPermisosUsuario"), Company.ApplicationSBO.Company.UserName.Trim()));
                foreach (DataRow drMostrarMenus in dtMostrarMenu.Rows)
                {
                    Configuracion.LtPermisosMenu.Add(drMostrarMenus[0].ToString().Trim());
                }
            }

            return Configuracion.LtPermisosMenu.Any(x => x == pStrMenu.Trim());
        }

        #region "Funciones Contrato de Ventas"
        /// <summary>
        /// Función que retorna la serie de numeración para documento específico
        /// </summary>
        /// <param name="p_strTipo">Tipo de Inventario a consultar</param>
        /// <param name="p_TipoSeries">Tipo de documento a consultar</param>
        /// <returns>Entero con la serie de numeración solicitada</returns>
        public static int GetSerie(string p_strTipo, GeneralEnums.scgTipoSeries p_TipoSeries, bool blnExenta)
        {
            int intSerie = -1;
            if (Configuracion.ParamGenAddon.Admin6.Any(admin6 => admin6.U_Tipo.Trim().Equals(p_strTipo)))
            {
                if (Configuracion.ParamGenAddon.Admin6.Any(admin6 => admin6.U_Tipo.Trim().Equals(p_strTipo) && admin6.U_Cod_Item == Convert.ToInt32(p_TipoSeries).ToString()))
                    if (blnExenta)
                        intSerie = (int)Configuracion.ParamGenAddon.Admin6.FirstOrDefault(admin6 => admin6.U_Tipo.Trim().Equals(p_strTipo) && admin6.U_Cod_Item == Convert.ToInt32(p_TipoSeries).ToString()).U_SerieEx;
                    else
                        intSerie = (int)Configuracion.ParamGenAddon.Admin6.FirstOrDefault(admin6 => admin6.U_Tipo.Trim().Equals(p_strTipo) && admin6.U_Cod_Item == Convert.ToInt32(p_TipoSeries).ToString()).U_Serie;
                if (0 == intSerie) intSerie = -1;
            }
            else
                throw new ApplicationException("No se ha definido un tipo");
            return intSerie;

        }
        /// <summary>
        /// Función que retorna el impuesto para el tipo de inventario solicitado
        /// </summary>
        /// <param name="p_strTipo">Tipo de Inventario a consultar</param>
        /// <param name="p_TipoSeries">Tipo de documento a consultar</param>
        /// <returns>Cadena con el impuesto solicitado</returns>
        public static string GetImpuesto(string p_strTipo, GeneralEnums.scgTipoSeries p_TipoSeries)
        {
            string strImpuesto = string.Empty;
            if (Configuracion.ParamGenAddon.Admin3.Any(admin3 => admin3.U_Tipo.Trim().Equals(p_strTipo)))
            {
                if (Configuracion.ParamGenAddon.Admin3.Any(admin3 => admin3.U_Tipo.Trim().Equals(p_strTipo) && admin3.U_Cod_Item.Equals(Convert.ToInt32(p_TipoSeries).ToString())))
                    strImpuesto = Configuracion.ParamGenAddon.Admin3.FirstOrDefault(admin3 => admin3.U_Tipo.Trim().Equals(p_strTipo) && admin3.U_Cod_Item.Equals(Convert.ToInt32(p_TipoSeries).ToString())).U_Cod_Imp.Trim();
            }
            else
                throw new ApplicationException("No se ha definido un tipo");
            return strImpuesto;

        }

        /// <summary>
        /// Función que carga las líneas y posiciones de una cotización para ser manipulada
        /// </summary>
        /// <param name="p_intDocEntry">DocEntry de la cotización</param>
        /// <param name="p_sapDocuments">Parametro opcional para cargar la cotización</param>
        /// <returns>Entidad de la cotización</returns>
        public static oDocumento CargaCotizacionConPosiciones(int p_intDocEntry, ref Documents p_sapDocuments)
        {
            oDocumento oDocumento = null;
            Documents sapDocuments = null;
            try
            {
                sapDocuments = GetQuotation(p_intDocEntry);
                if (sapDocuments != null)
                {
                    oDocumento = new oDocumento();
                    oDocumento.Lineas = new List<oLineasDocumento>();
                    oDocumento.DocEntry = sapDocuments.DocEntry;
                    oDocumento.CardCode = sapDocuments.CardCode;
                    for (int index = 0; index < sapDocuments.Lines.Count; index++)
                    {
                        sapDocuments.Lines.SetCurrentLine(index);
                        oDocumento.Lineas.Add(new oLineasDocumento
                        {
                            LineNum = sapDocuments.Lines.LineNum,
                            ID = sapDocuments.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim(),
                            intPosicion = index
                        });
                    }
                    p_sapDocuments = sapDocuments;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // DestruirObjeto(ref sapDocuments);
            }
            return oDocumento;
        }

        /// <summary>
        /// Función que retorna la posición en la cotización de un LineNum
        /// </summary>
        /// <param name="p_lineas">Lista de Líneas de la cotización</param>
        /// <param name="p_intLineNum">LineNume a consultar</param>
        /// <returns>Posición del LineNum</returns>
        public static int GetLinePosition(List<oLineasDocumento> p_lineas, int p_intLineNum)
        {
            int intPosition;
            try
            {
                if (p_lineas.Any(x => x.LineNum == p_intLineNum))
                    intPosition = p_lineas.First(x => x.LineNum == p_intLineNum).intPosicion;
                else
                    intPosition = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intPosition;
        }

        /// <summary>
        /// Función que retorna la posición en la cotización de un ID
        /// </summary>
        /// <param name="p_lineas">Lista de Líneas de la cotización</param>
        /// <param name="p_strID">ID a consultar</param>
        /// <returns>Posición del ID</returns>
        public static int GetLinePosition(List<oLineasDocumento> p_lineas, string p_strID)
        {
            int intPosition;
            try
            {
                if (p_lineas.Any(x => x.ID.Trim().Equals(p_strID)))
                    intPosition = p_lineas.First(x => x.ID.Trim().Equals(p_strID)).intPosicion;
                else
                    intPosition = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intPosition;
        }

        /// <summary>
        /// Función que retorna la propieda de tabla admin4
        /// </summary>
        /// <param name="p_strTipoInventario">Tipo de Inventario a consultar</param>
        /// <returns>Cadena con el alamcén solicitado</returns>
        public static string GetPropertyAdmin4(string p_strTipoInventario, GeneralEnums.scgTipoPropiedadAdmin4 p_Propiedad)
        {
            string strBodegaTipoVeh = String.Empty;
            try
            {
                if (Configuracion.ParamGenAddon.Admin4.Any(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)))
                    switch (p_Propiedad)
                    {
                        case GeneralEnums.scgTipoPropiedadAdmin4.Transito:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Transito;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Stock:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Stock;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Costo:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Costo;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Ingreso:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Ingreso;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.AccXAlm:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_AccXAlm;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Bod_Tram:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Bod_Tram;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Bod_Log:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Bod_Log;
                            break;
                        case GeneralEnums.scgTipoPropiedadAdmin4.Devolucion:
                            strBodegaTipoVeh = Configuracion.ParamGenAddon.Admin4.FirstOrDefault(admin4 => admin4.U_Tipo.Trim().Equals(p_strTipoInventario)).U_Devolucion;
                            break;
                    }
            }
            catch (Exception)
            {
                throw;
            }
            return strBodegaTipoVeh;
        }

        /// <summary>
        /// Función que retorna la cuenta de adicionales solicitada
        /// </summary>
        /// <param name="p_strTipoInventario">Tipo de Inventario a consultar</param>
        /// <param name="p_TipoSeries">Tipo de documento a consultar</param>
        /// <returns>Cadena con cuenta solicitada</returns>
        public static string GetCuentaAdicional(string p_strTipoInventario, GeneralEnums.scgTipoSeries p_TipoSeries)
        {
            string strCuentasAdicionales = string.Empty;
            if (Configuracion.ParamGenAddon.Admin5.Any(admin5 => admin5.U_Tipo.Trim().Equals(p_strTipoInventario)))
            {
                if (Configuracion.ParamGenAddon.Admin5.Any(admin5 => admin5.U_Tipo.Trim().Equals(p_strTipoInventario) && admin5.U_Cod_Item.Equals(Convert.ToInt32(p_TipoSeries).ToString())))
                    strCuentasAdicionales = Configuracion.ParamGenAddon.Admin5.FirstOrDefault(admin5 => admin5.U_Tipo.Trim().Equals(p_strTipoInventario) && admin5.U_Cod_Item.Equals(Convert.ToInt32(p_TipoSeries).ToString())).U_Cuenta.Trim();
            }
            else
                throw new ApplicationException("No se ha definido un tipo");
            return strCuentasAdicionales;
        }

        /// <summary>
        /// Función que retorna monto de gastos adicionales
        /// </summary>
        /// <param name="p_strTipoInventario">Tipo de Inventario a consultar</param>
        /// <param name="p_Items">Tipo de item de la factura</param>
        /// <returns>Entero con gasto adicional</returns>
        public static int GetGastoAdicional(string p_strTipoInventario, GeneralEnums.scgItemsFactura p_Items)
        {
            int intGastosAdicionales = 0;
            if (Configuracion.ParamGenAddon.Admin2.Any(admin2 => admin2.U_Tipo.Trim().Equals(p_strTipoInventario)))
            {
                if (Configuracion.ParamGenAddon.Admin2.Any(admin2 => admin2.U_Tipo.Trim().Equals(p_strTipoInventario) && admin2.U_Cod_Item.Equals(Convert.ToInt32(p_Items).ToString())))
                    intGastosAdicionales = (int)Configuracion.ParamGenAddon.Admin2.FirstOrDefault(admin2 => admin2.U_Tipo.Trim().Equals(p_strTipoInventario) && admin2.U_Cod_Item.Equals(Convert.ToInt32(p_Items).ToString())).U_Cod_GA;
            }
            else
                throw new ApplicationException("No se ha definido un tipo");
            return intGastosAdicionales;
        }

        /// <summary>
        /// Retorna el valor de un impuesto
        /// </summary>
        /// <param name="p_strTax">Impuesto a solicitar</param>
        /// <param name="p_blnVatGruoup">Tipo de impuesto solicitado</param>
        /// <returns>Doble con impuesto solicitado</returns>
        public static double GetTaxRate(string p_strTax,DateTime p_dtFecha, bool p_blnVatGruoup)
        {
            SalesTaxCodes oSalesTaxCodes = default(SalesTaxCodes);
            VatGroups oVatGroup = default(VatGroups);
            double dbRate = 0.0;
            try
            {
                if (p_blnVatGruoup)
                {
                    oVatGroup = (VatGroups)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oVatGroups);
                    if (oVatGroup.GetByKey(p_strTax))
                        for (int index = oVatGroup.VatGroups_Lines.Count-1; index >= 0; index--)
                        {
                           oVatGroup.VatGroups_Lines.SetCurrentLine(index);
                           if (p_dtFecha >= oVatGroup.VatGroups_Lines.Effectivefrom)
                            {
                                dbRate = oVatGroup.VatGroups_Lines.Rate;   
                                break;
                            }
                        }
                }
                else
                {
                    oSalesTaxCodes = (SalesTaxCodes)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oSalesTaxCodes);
                    if (oSalesTaxCodes.GetByKey(p_strTax))
                        dbRate = oSalesTaxCodes.Rate;   
                }
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
            }
            finally
            {
                DestruirObjeto(ref oSalesTaxCodes);
                DestruirObjeto(ref oVatGroup);
            }
            return dbRate;
        }

        /// <summary>
        /// Valida desbloqueado del periodo contable
        /// </summary>
        /// <param name="p_dtFechaContrato">Fecha del prestamo</param>
        /// <returns>bool indicando si se encuentra bloqueado</returns>
        public static bool GetFinancePeriodStat(DateTime p_dtFecha)
        {
            string strResult = string.Empty;
            PeriodCategoryParamsCollection oPeriodCategoryColl = default(PeriodCategoryParamsCollection);
            FinancePeriods oFinancePeriods = default(FinancePeriods);
            FinancePeriod oFinancePeriod = default(FinancePeriod);

            try
            {
                oPeriodCategoryColl = Company.CompanyService.GetPeriods();
                for (int i = oPeriodCategoryColl.Count - 1; i >= 0; i--)
                {
                    oFinancePeriods = Company.CompanyService.GetFinancePeriods(oPeriodCategoryColl.Item(i));
                    for (int j = oFinancePeriods.Count - 1; j >= 0; j--)
                    {
                        oFinancePeriod = oFinancePeriods.Item(j);
                        if (p_dtFecha >= oFinancePeriod.PostingDateFrom &&
                            p_dtFecha <= oFinancePeriod.PostingDateTo)
                        {
                            strResult = oFinancePeriod.PeriodStatus == PeriodStatusEnum.ltUnlocked ? "Y" : "N";
                            break;
                        }
                        if (!string.IsNullOrEmpty(strResult))
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
            }
            finally
            {
                DestruirObjeto(ref oPeriodCategoryColl);
                DestruirObjeto(ref oFinancePeriods);
                DestruirObjeto(ref oFinancePeriod);
            }
            return !strResult.Equals("N");
        }

        /// <summary>
        /// Función que retorna item solicitado
        /// </summary>
        /// <param name="p_strItem">ItemCode del item</param>
        /// <returns>Objeto Item solicitado</returns>
        public static Items GetItem(string p_strItem)
        {
            SAPbobsCOM.Items oItems;
            try
            {
                oItems = (Items)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oItems);
                if (!oItems.GetByKey(p_strItem))
                    throw new Exception(String.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                return oItems;

            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Método que gestiona la mensajería del SAP
        /// </summary>
        /// <param name="p_strTitulo">Titulo del mensaje</param>
        /// <param name="p_strMensaje">Cuerpo del mensaje</param>
        /// <param name="p_strLista">Lista de usuarios a enviar el mensaje</param>
        public static void SendMessage(string p_strTitulo, string p_strMensaje, List<string> p_strLista)
        {
            Messages oMsg = default(Messages);
            try
            {
                oMsg = (Messages)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oMessages);
                oMsg.Priority = BoMsgPriorities.pr_High;
                oMsg.Subject = p_strTitulo;
                oMsg.MessageText = p_strMensaje;

                foreach (string strUsuario in p_strLista)
                {
                    oMsg.Recipients.Add();
                    oMsg.Recipients.UserCode = strUsuario;
                    oMsg.Recipients.NameTo = strUsuario;
                    oMsg.Recipients.SendInternal = BoYesNoEnum.tYES;
                }
                if (0 != oMsg.Add())
                {
                    throw new Exception(string.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                }
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
            }
            finally
            {
                DestruirObjeto(ref oMsg);
            }
        }

        /// <summary>
        /// Función que retorna el socio de negocios solicitado
        /// </summary>
        /// <param name="p_strCardCode">EmpID del Socio de Negocios</param>
        /// <returns>Objeto BusinessPartners solicitado</returns>
        public static BusinessPartners GetBusinessPartners(string p_strCardCode)
        {
            SAPbobsCOM.BusinessPartners oBusinessPartner;
            try
            {
                oBusinessPartner = (BusinessPartners)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (!oBusinessPartner.GetByKey(p_strCardCode))
                    throw new Exception(String.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                return oBusinessPartner;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Función que retorna el empleado solicitado
        /// </summary>
        /// <param name="p_strEmpID">ID del Empleado</param>
        /// <returns>Objeto EmployeesInfo solicitado</returns>
        public static EmployeesInfo GetEmployeesInfo(int p_strEmpID)
        {
            SAPbobsCOM.EmployeesInfo oEmployeesInfo;
            try
            {
                oEmployeesInfo = (EmployeesInfo)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oEmployeesInfo);
                if (!oEmployeesInfo.GetByKey(p_strEmpID))
                    throw new Exception(String.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                return oEmployeesInfo;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Función que retorna el ProductTree solicitado
        /// </summary>
        /// <param name="p_strCode">Code del Producto</param>
        /// <returns>Objeto ProductTrees solicitado</returns>
        public static ProductTrees GetProductTrees(string p_strCode)
        {
            SAPbobsCOM.ProductTrees oProductTree;
            try
            {
                oProductTree = (ProductTrees)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oProductTrees);
                if (!oProductTree.GetByKey(p_strCode))
                    throw new Exception(String.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                return oProductTree;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Función que retorna el ProductTree solicitado
        /// </summary>
        /// <param name="p_strCodEntry">Code del Producto</param>
        /// <returns>Objeto ProductTrees solicitado</returns>
        public static Documents GetQuotation(int p_strCodEntry)
        {
            SAPbobsCOM.Documents oQuotation;
            try
            {
                oQuotation = (Documents)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
                if (!oQuotation.GetByKey(p_strCodEntry))
                    throw new Exception(String.Format("{0}: {1}", Company.CompanySBO.GetLastErrorCode(), Company.CompanySBO.GetLastErrorDescription()));
                return oQuotation;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Función que retorna el esquema de la base de datos
        /// </summary>
        /// <returns>string Esquema</returns>
        public static string GetReportSchema()
        {
            try
            {
                if (Company.CompanySBO.DbServerType != SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                    return "dbo";
                else
                    return Company.ApplicationSBO.Company.DatabaseName;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return null;
            }
        }

        /// <summary>
        /// Función que indica si la conexion de BD es a HANA
        /// </summary>
        /// <returns>Conexion a HANA True/False</returns>
        public static Boolean IsHANAConnection()
        {
            try
            {
                return Company.CompanySBO.DbServerType == BoDataServerTypes.dst_HANADB;
            }
            catch (Exception ex)
            {
                ManejoErrores(ex);
                return false;
            }
        }

        #endregion

        #region "ManejoErrores"
        /// <summary>
        /// Método que maneja los errores del sistema
        /// </summary>
        /// <param name="ex">Excepción lanzada por el sistema</param>
        public static void ManejoErrores(Exception ex)
        {
            var description = String.Empty;
            try
            {
                Exception exception = new Exception();
                exception = ex;
                Company.ApplicationSBO.StatusBar.SetText(exception.Message);
                description = exception.InnerException != null ? String.Format("{0} <br /><br />InnerException: {1} <br /><br /> Stack trace: {2}", exception.Message, exception.InnerException.Message, exception.StackTrace) : String.Format("{0} <br /><br /> Stack trace: {1}", exception.Message, exception.StackTrace);
                String mensaje = "<html><body><fieldset><legend><b>Detalle: </b></legend>" + description + " <br /></fieldset>" + "</body></html>";

                //EnviarMailLocal(mensaje, _application);
                LogText(exception);
            }
            catch (Exception)
            {
            }

        }

        public static void ManejoErroresWinForms(Exception ex)
        {
            String Descripcion = String.Empty;
            try
            {
                Descripcion = ex.InnerException != null ? String.Format("{0} <br /><br />InnerException: {1} <br /><br /> Stack trace: {2}", ex.Message, ex.InnerException.Message, ex.StackTrace) : String.Format("{0} <br /><br /> Stack trace: {1}", ex.Message, ex.StackTrace);
                String mensaje = "<html><body><fieldset><legend><b>Detalle: </b></legend>" + Descripcion + " <br /></fieldset>" + "</body></html>";
                LogText(ex);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Método para el envío de Email en caso de error
        /// </summary>
        /// <param name="mensaje">Mensaje a enviar</param>
        private static void EnviarMailLocal(string mensaje)
        {
            string fromEmail = String.Empty;
            string portEmail = String.Empty;
            string smtpServer = String.Empty;
            string subject = String.Empty;
            string emailList = String.Empty;
            string siteName = String.Empty;
            string emailAccount = String.Empty;
            string emailPass = String.Empty;
            string useSSL = String.Empty;

            var dtconfiguraciones = CargaConfiguracionErrores();
            if (dtconfiguraciones.Rows.Count > 0)
            {
                fromEmail = dtconfiguraciones.GetValue("U_FromEmail", 0).ToString();
                portEmail = dtconfiguraciones.GetValue("U_PortEmail", 0).ToString();
                smtpServer = dtconfiguraciones.GetValue("U_SmtpServer", 0).ToString();
                subject = dtconfiguraciones.GetValue("U_EmailSubject", 0).ToString();
                emailList = dtconfiguraciones.GetValue("U_ErrorEmail", 0).ToString();
                siteName = dtconfiguraciones.GetValue("U_CompanyName", 0).ToString();
                emailAccount = dtconfiguraciones.GetValue("U_EmailAccount", 0).ToString();
                emailPass = dtconfiguraciones.GetValue("U_EmailPass", 0).ToString();
                useSSL = dtconfiguraciones.GetValue("U_ServUseSSL", 0).ToString();

                MailMessage objCorreo = new MailMessage();
                objCorreo.Body = mensaje;
                objCorreo.IsBodyHtml = true;
                objCorreo.Priority = MailPriority.Normal;
                objCorreo.From = new MailAddress(fromEmail, siteName);

                objCorreo.Subject = subject;
                objCorreo.To.Add(emailList);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpServer;
                smtp.Port = Convert.ToInt32(portEmail);
                smtp.EnableSsl = useSSL == "1" ? true : false;
                smtp.Credentials = new NetworkCredential(emailAccount, emailPass);

                string output = String.Empty;

                try
                {
                    smtp.Send(objCorreo);
                    objCorreo.Dispose();
                }
                catch (Exception ex)
                {
                    LogText(ex);
                }

            }
        }

        /// <summary>
        /// Método que almacena en log XML los errores lanzados por el sistema
        /// </summary>
        /// <param name="ex">Excepción lanzada por el sistema</param>
        private static void LogText(Exception ex)
        {
            //obtenemos sólo la carpeta (quitamos el ejecutable) 
            string carpeta = Path.GetTempPath();
            //string defaultFileName = String.Format("{0}{1}.txt", "Logs - ", DateTime.Now.ToString("dd-MM-yyyy"));
            string defaultXmlName = String.Format("{0}{1}.xml", "XML Logs - ", DateTime.Now.ToString("dd-MM-yyyy"));

            //String rutaFichero = Path.Combine(carpeta, defaultFileName);
            String rutaXML = Path.Combine(carpeta, defaultXmlName);
            List<string> error = new List<string>();

            error.Add(String.Format("[{0}] -- Error: {1}", DateTime.Now, ex.Message));
            if (ex.InnerException != null)
                error.Add("InnerException: " + ex.InnerException);
            error.Add("StackTrace: " + ex.StackTrace);
            var lines = new List<string>();
            try
            {

                var writer = new XmlSerializer(typeof(LogErrores));
                var logErrores = new LogErrores();
                if (File.Exists(rutaXML))
                {
                    using (var reader = File.OpenText(rutaXML))
                    {
                        logErrores = (LogErrores)writer.Deserialize(reader);
                        var err = new Error();
                        err.TipoExcepcion = ex.GetType().Name;
                        err.Aplicacion = "SCG DMS One";
                        err.Codigo = Marshal.GetExceptionCode().ToString();
                        err.CompañiaSBO = "";
                        err.Mensaje = ex.Message;
                        err.StackTrace = ex.StackTrace;
                        err.Fecha = DateTime.Now.ToString();
                        if (logErrores.Errores.Count > 0)
                        {
                            logErrores.Errores.Add(err);
                        }
                        else
                        {
                            var errs = new List<Error>();
                            errs.Add(err);
                            logErrores.Errores = errs;
                        }
                    }
                }
                else
                {
                    logErrores.Idioma = CultureInfo.CurrentCulture.Name;
                    var errs = new List<Error>();
                    var err = new Error();
                    err.TipoExcepcion = ex.GetType().Name;
                    err.Aplicacion = "SCG DMS One";
                    err.Codigo = Marshal.GetExceptionCode().ToString();
                    err.CompañiaSBO = "";
                    err.Mensaje = ex.Message;
                    err.StackTrace = ex.StackTrace;
                    err.Fecha = DateTime.Now.ToString("dd-MM-yyyy");
                    if (ex.InnerException != null)
                    {
                        err.InnerException = ex.InnerException.Message;
                    }
                    else
                    {
                        err.InnerException = String.Empty;
                    }
                    errs.Add(err);
                    logErrores.Errores = errs;
                }


                var file = new StreamWriter(rutaXML);
                writer.Serialize(file, logErrores);
                file.Close();
            }
            catch (Exception exception)
            {

            }
        }

        /// <summary>
        /// Función que retorna DataTable con configuraciones para el manejo de Errores
        /// </summary>
        /// <returns>DataTable con configuración</returns>
        public static SAPbouiCOM.DataTable CargaConfiguracionErrores()
        {
            SAPbouiCOM.DataTable dtSap;
            Form oForm;
            oForm = Company.ApplicationSBO.Forms.ActiveForm;
            var dtConErr = "dtConsultaErrores";

            if (oForm.DataSources.DataTables.Count > 0)
                if (ValidaSiDataTableExiste(oForm, dtConErr))
                    dtSap = oForm.DataSources.DataTables.Item(dtConErr);
                else
                    dtSap = oForm.DataSources.DataTables.Add(dtConErr);
            else
                dtSap = oForm.DataSources.DataTables.Add(dtConErr);

            var query = "select U_FromEmail, U_PortEmail, U_SmtpServer, U_EmailSubject, U_ErrorEmail, U_CompanyName, U_ServUseSSL, U_EmailAccount, U_EmailPass from [@SCGD_CONF_ERROR]";
            try
            {
                dtSap.ExecuteQuery(query);
                return dtSap;
            }
            catch (Exception ex)
            {
                LogText(ex);
                return null;
            }
        }


        public static string TipodeImpuesto(string p_strIDChooseFromList)
        {
            try
            {
                var usaVATGroup = Configuracion.ParamGenAddon.U_UsaVATGroup;

                if (usaVATGroup == "Y")
                {
                    switch (p_strIDChooseFromList)
                    {
                        //Configuracion de Sucursal
                        case "CFL_ImpSer":
                            return "CFL_SerVAT";
                        case "CFL_ImpRep":
                            return "CFL_RepVAT";
                        case "CFL_ImpSum":
                            return "CFL_SumVAT";
                        case "CFL_ImpSeE":
                            return "CFL_SEVAT";
                        case "CFL_Imp_G":
                            return "CFL_GVAT";
                        case "CFL_ImpReC":
                            return "CFL_ReCVAT";
                        case "CFL_ImpSEC":
                            return "CFL_SECVAT";

                        //Ordenes de Compra de la OT -- Taller
                        case "CFLTAX":
                            return "CFLTAXVAT";

                        //Contrato de Ventas
                        case "CFL_Imp": //Vehiculo
                            return "CFL_ImpVAT";
                        case "CFL_ImpAcc": // Venta Accesorio
                            return "CFL_ImpAccVAT";
                        case "CFL_IC": // Compra Accesorio
                            return "CFL_ICVAT";
                        case "CFL_IVTra":
                            return "CFL_IVTraVAT"; // Venta Tramite
                        case "CFL_ICTra":
                            return "CFL_ICTraVAT"; // Compra Tramite

                        //Financiamento
                        case "CFL_CodImp":
                            return "CFL_CodImpVAT"; //Configuracion de Financiamiento
                    }
                }
                return p_strIDChooseFromList;
            }

            catch (Exception ex)
            {
                LogText(ex);
            }
            return p_strIDChooseFromList;
        }

        public static string FormatoFecha(DateTime p_dtFecha)
        {
            try
            {
                var n = DMS_Connector.Company.AdminInfo.DateTemplate;

                switch (n)
                {
                    case BoDateTemplate.dt_CCYYMMDD:
                        return p_dtFecha.ToString("yyyyMMdd");
                    case BoDateTemplate.dt_DDMMCCYY:
                        return p_dtFecha.ToString("ddMMyyyy");
                    case BoDateTemplate.dt_DDMMYY:
                        return p_dtFecha.ToString("ddMMyy");
                    case BoDateTemplate.dt_DDMonthYYYY:
                        return p_dtFecha.ToString("ddMyyyy");
                    case BoDateTemplate.dt_MMDDCCYY:
                        return p_dtFecha.ToString("MMddyyyy");
                    case BoDateTemplate.dt_MMDDYY:
                        return p_dtFecha.ToString("MMddyy");
                }
            }

            catch (Exception ex)
            {
                LogText(ex);
            }
            return String.Empty;
        }


        #endregion
    }
    #region ...Errores...
    [Serializable]
    public class LogErrores
    {
        public string Idioma;
        public List<Error> Errores;
    }
    public class Error
    {
        public string TipoExcepcion;
        public string Codigo;
        public string Mensaje;
        public string Aplicacion;
        public string CompañiaSBO;
        public string Fecha;
        public string StackTrace;
        public string InnerException;
    }
    #endregion
}
