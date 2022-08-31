///Autor: Werner F.R.
///Fecha: 09/03/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SBOFramework.DI;
using System.Data;
using System.Data.SqlClient;

namespace SCG.SBOFramework
{
    public class Services
    {
        CompanyConnectionInfo ConnectionInfo { get; set; }

        public Services(CompanyConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
        }

        public int GetDocumentNumber(string udoID)
        {
            return (int)SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select AutoKey from ONNM where ObjectCode = '" + udoID + "'");
        }

        public DataTable GetCurrencies()
        {
            return SqlHelper.ExecuteDataset(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select CurrCode, CurrName from OCRN").Tables[0];
        }

        public DataTable GetPurchaseInvoiceSeries()
        {
            return SqlHelper.ExecuteDataset(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select Series, SeriesName from NNM1 where ObjectCode = '18' and DocSubType <> 'DM'").Tables[0];
        }

        public DataTable GetTransitAccounts()
        {
            return SqlHelper.ExecuteDataset(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select t.Name as VechicleType, a.U_Transito as Account from [@SCGD_ADMIN4] a left outer join [@SCGD_TIPOVEHICULO] t on a.U_Tipo = t.Code").Tables[0];
        }

        public int GetPurchaceInvoiceDocNum(int docEntry)
        {
            return (int)SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select DocNum from OPCH where DocEntry = " + docEntry.ToString());
        }

        public DataTable GetPurchaseTransactions()
        {
            return SqlHelper.ExecuteDataset(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select Code, Name from [@SCGD_TRAN_COMP]").Tables[0];
        }

        public DataTable GetSalesTax()
        {
            return SqlHelper.ExecuteDataset(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select Code, Name from OSTA").Tables[0];
        }

        public int GetReceivedAvailabilityCode()
        {
            try
            {
                return int.Parse(SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select U_Disp_R from [@SCGD_ADMIN]").ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting received availability code. Error: " + ex.Message); 
            }
        }

        public int GetVehicleSaleStatus(string code)
        {
            object result = SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select U_Dispo from [@SCGD_VEHICULO] where code = '" + code + "'");
            if (result == null)
                return -1;
            else
                return int.Parse(result.ToString());
        }

        public bool VehicleExists(string unitCode)
        {
            object result = SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select U_Cod_Unid from [@SCGD_VEHICULO] where U_Cod_Unid = '" + unitCode.ToString() + "'");

            if (result == null)
                return false;
            else
                return true; 
        }

        public string GetDescriptionFieldValue(DMSCatalog catalogType, string codeFieldValue)
        {
            string catalog = "";

            switch (catalogType)
            {
                case DMSCatalog.Marcas:
                    catalog = "[@SCGD_MARCA]";
                    break;
                case DMSCatalog.Estilos:
                    catalog = "[@SCGD_ESTILO]";
                    break;
                case DMSCatalog.Modelos:
                    catalog = "[@SCGD_MODELO]";
                    break;
                case DMSCatalog.MarcasMotor:
                    catalog = "[@SCGD_MARCA_MOTOR]";
                    break;
                case DMSCatalog.Cabinas:
                    catalog = "[@SCGD_CABINA]";
                    break;
                case DMSCatalog.Carrocerias:
                    catalog = "[@SCGD_CARROCERIA]";
                    break;
                case DMSCatalog.Categorias:
                    catalog = "[@SCGD_CATEGORIA_VEHI]";
                    break;
                case DMSCatalog.Colores:
                    catalog = "[@SCGD_COLOR]";
                    break;
                case DMSCatalog.Combustible:
                    catalog = "[@SCGD_COMBUSTIBLE]";
                    break;
                case DMSCatalog.Disponibilidad:
                    catalog = "[@SCGD_DISPONIBILIDAD]";
                    break;
                case DMSCatalog.Estados:
                    catalog = "[@SCGD_ESTADO]";
                    break;
                case DMSCatalog.Techo:
                    catalog = "[@SCGD_TECHO]";
                    break;
                case DMSCatalog.Tipos:
                    catalog = "[@SCGD_TIPOVEHICULO]";
                    break;
                case DMSCatalog.Tracciones:
                    catalog = "[@SCGD_TRACCION]";
                    break;
                case DMSCatalog.Transmisiones:
                    catalog = "[@SCGD_TRANSMISION]";
                    break;
                case DMSCatalog.Ubicaciones:
                    catalog = "[@SCGD_UBICACIONES]";
                    break;
                case DMSCatalog.MarcasComerciales:
                    catalog = "[@SCGD_CONF_ART_VENTA]";
                    break;
            }

            return SqlHelper.ExecuteScalar(ConnectionInfo.GetSqlConnectionString(), CommandType.Text, "select name from " + catalog + " where code = '" + codeFieldValue + "'").ToString();
        }

    }
}