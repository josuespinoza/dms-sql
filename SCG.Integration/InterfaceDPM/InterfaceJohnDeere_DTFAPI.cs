using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Globalization;
using DMS_Connector.Business_Logic.DataContract.Articulo;
using DMS_Connector.Business_Logic.DataContract.SAPDocumento;
using SAPbobsCOM;
using SCG.Integration.InterfaceDPM.Entities;
using SCG.SBOFramework;
using DMS_Connector;
using DMS_Connector.Data_Access;
using System.Data;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using SCG.Integration.InterfaceDPM.Entities.URecords;
using ICompany = SAPbobsCOM.ICompany;
using RestSharp;
using Newtonsoft.Json.Linq;
using DMS_Connector.Business_Logic.DataContract;
using DMS_Connector.Business_Logic.DataContract.Integracion;
using InterfazJohnDeereDC = SCG.Integration.InterfaceDPM.Entities.InterfazJohnDeereDC;

namespace SCG.Integration.InterfaceDPM
{
    public class InterfaceJohnDeere_DTFAPI
    {
        public IApplication oApplicationSBO { get; private set; }
        public ICompany oCompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;

        public SAPbouiCOM.Form oForm { get; set; }

        private static NumberFormatInfo n;


        public enum Accion
        {
            CargarArchivo,
            DescargarArchivo
        }



        #region Constructor
        public InterfaceJohnDeere_DTFAPI(IApplication applicationSBO, ICompany companySBO, SAPbouiCOM.Form p_oForm)
        {
            try
            {
                oApplicationSBO = applicationSBO;
                oCompanySBO = companySBO;
                SBOCompany = (SAPbobsCOM.Company)companySBO;
                oForm = p_oForm;

                n = DIHelper.GetNumberFormatInfo(companySBO);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Metodos
        public void ManejaInterfaceJohnDeere_DTFAPI(ref string p_strAccion, ref string p_strRuta )
        {
            InterfazJohnDeereDC oInterfazJohnDeere;
            try
            {
                oInterfazJohnDeere = new SCG.Integration.InterfaceDPM.Entities.InterfazJohnDeereDC();
                CargarConfiguracion(ref oInterfazJohnDeere, ref p_strRuta);
                GetAccessToken(ref oInterfazJohnDeere);

                switch (p_strAccion)
                {
                    case "CargarArchivo":
                        if (!string.IsNullOrEmpty(oInterfazJohnDeere.RutaArchivoCarga))
                        {
                            PUTLoadFile(ref oInterfazJohnDeere);
                        }
                        else
                        {
                            oApplicationSBO.StatusBar.SetText("Debe seleccionar un archivo para realizar la carga", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        }
                        break;
                    case "DescargarArchivo":
                        break;
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void CargarConfiguracion(ref InterfazJohnDeereDC p_oInterfazJohnDeereDC, ref string p_strRuta )
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                oForm.DataSources.DBDataSources.Add("@SCGD_JD");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);
                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        p_oInterfazJohnDeereDC.TokenURL = !string.IsNullOrEmpty(dsInformation.GetValue("U_TokenURL", index)) ? dsInformation.GetValue("U_TokenURL", index).ToString().Trim() : string.Empty;
                        p_oInterfazJohnDeereDC.ClientID = !string.IsNullOrEmpty(dsInformation.GetValue("U_ClientID", index)) ? dsInformation.GetValue("U_ClientID", index).ToString().Trim() : string.Empty;
                        p_oInterfazJohnDeereDC.Secret = !string.IsNullOrEmpty(dsInformation.GetValue("U_Secret", index)) ? dsInformation.GetValue("U_Secret", index).ToString().Trim() : string.Empty;
                        p_oInterfazJohnDeereDC.BaseURL = !string.IsNullOrEmpty(dsInformation.GetValue("U_BaseURL", index)) ? dsInformation.GetValue("U_BaseURL", index).ToString().Trim() : string.Empty;
                        p_oInterfazJohnDeereDC.DealerAccount = !string.IsNullOrEmpty(dsInformation.GetValue("U_DAcc", index)) ? dsInformation.GetValue("U_DAcc", index).ToString().Trim() : string.Empty;
                    }
                    p_oInterfazJohnDeereDC.RutaArchivoCarga = !string.IsNullOrEmpty(p_strRuta) ? p_strRuta : string.Empty;
                }
            }
            
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void GetAccessToken(ref InterfazJohnDeereDC p_oInterfazJohnDeereDC)
        {
            RestClient client;
            RestRequest request;
            IRestResponse response;
            JObject resp;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                client = new RestClient(p_oInterfazJohnDeereDC.TokenURL);
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded",
                    "grant_type=client_credentials&scope=dtf:dbs:file:read dtf:dbs:file:write&client_id=" +
                    p_oInterfazJohnDeereDC.ClientID + "&client_secret=" + p_oInterfazJohnDeereDC.Secret,
                    ParameterType.RequestBody);
                response = client.Execute(request);
                resp = JObject.Parse(response.Content);
                p_oInterfazJohnDeereDC.AccessToken = resp.SelectToken("access_token").ToString();
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void PUTLoadFile(ref InterfazJohnDeereDC p_oInterfazJohnDeereDC )
        {
            RestClient client;
            RestRequest request;
            IRestResponse response;
            JObject resp;
            try
            {
                client = new RestClient(p_oInterfazJohnDeereDC.BaseURL);
                client.Timeout = -1;
                request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", "Bearer " + p_oInterfazJohnDeereDC.AccessToken);
                request.AddFile("file", p_oInterfazJohnDeereDC.RutaArchivoCarga);
                response = client.Execute(request);

                //resp = JObject.Parse(response.Content);

                oApplicationSBO.StatusBar.SetText("Process Complete", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
    #endregion
    }
}
