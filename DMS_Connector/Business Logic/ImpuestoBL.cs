using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMS_Connector;
using DMS_Connector.Business_Logic.DataContract.Impuesto;
using SAPbobsCOM;
using SAPbouiCOM;
using DataTable = System.Data.DataTable;
using Items = SAPbobsCOM.Items;

namespace DMS_Connector.Business_Logic
{
    public class ImpuestoBL
    {
        #region "Atributos"
        public List<TaxCodeDeterminationDC> ConfiguracionSucursales { get; set; }

        #endregion

        /// <summary>
        /// Función para obtener Impuesto
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>String con resultado</returns>
        public static string ObtenerImpuesto(Form p_oForm, string p_strCardCode, string p_strItemCode)
        {
            String strImpuesto;
            try
            {
                strImpuesto = TaxCodeDetermination(p_oForm, p_strCardCode, p_strItemCode);
                if (!string.IsNullOrEmpty(strImpuesto))
                {
                    return strImpuesto;
                }
                strImpuesto = ImpuestoSN(p_strCardCode);
                if (!string.IsNullOrEmpty(strImpuesto))
                {
                    return strImpuesto;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Función para obtener Impuesto
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>String con resultado</returns>
        public static string TaxCodeDetermination(Form p_oForm, string p_strCardCode, string p_strItemCode)
        {
            try
            {
                SAPbouiCOM.Condition oCondition;
                SAPbouiCOM.Conditions oConditions;
                DBDataSource dsImpuesto;
                //SAPbouiCOM.Form oForm;
                //oForm = DMS_Connector.Company.ApplicationSBO.Forms.Item("OTCX");
                p_oForm.DataSources.DBDataSources.Add("OTCX");
                dsImpuesto = p_oForm.DataSources.DBDataSources.Item("OTCX");
                oConditions = (SAPbouiCOM.Conditions)DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                if (!string.IsNullOrEmpty(p_strCardCode))
                {
                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "StrVal1";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_strCardCode;
                    oCondition.BracketCloseNum = 1;
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;
                }
                if (!string.IsNullOrEmpty(p_strItemCode))
                {
                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "StrVal2";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_strItemCode;
                    oCondition.BracketCloseNum = 1;
                }
                dsImpuesto.Query(oConditions);
                if (dsImpuesto.Size > 0)
                {
                    if (!string.IsNullOrEmpty(dsImpuesto.GetValue("LnTaxCode", 0)))
                    {
                        return dsImpuesto.GetValue("LnTaxCode", 0).ToString().Trim();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return string.Empty;
            }


        }
        /// <summary>
        /// Función para obtener Impuesto
        /// </summary>
        /// <param name="p_strConsulta">Query a ejecutar</param>
        /// <returns>String con resultado</returns>
        public static string ImpuestoSN(string p_strCardCode)
        {
            BusinessPartners oBusinessPartners = default(BusinessPartners);
            try
            {
                oBusinessPartners =
                    (BusinessPartners)Company.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (oBusinessPartners.GetByKey(p_strCardCode))
                {
                    for (int index = 0; index < oBusinessPartners.Addresses.Count; index++)
                    {
                        oBusinessPartners.Addresses.SetCurrentLine(index);
                        if (oBusinessPartners.Addresses.AddressName == oBusinessPartners.ShipToDefault)
                        {
                            return oBusinessPartners.Addresses.TaxCode;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return string.Empty;
            }
        }


        public static string ObtenerMarcaComercialVehiculo(string p_strCode)
        {
            GeneralService oGeneralService = default(GeneralService);
            GeneralData oGeneralData = default(GeneralData);
            GeneralDataParams oGeneralParams = default(GeneralDataParams);

            try
            {
                oGeneralService = Company.CompanySBO.GetCompanyService().GetGeneralService("SCGD_VEH");
                oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", p_strCode);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                return oGeneralData.GetProperty("U_TIPINV").ToString().Trim();
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(ex);
                return string.Empty;
            }
            finally
            {
                Helpers.DestruirObjeto(ref oGeneralService);
                Helpers.DestruirObjeto(ref oGeneralData);
                Helpers.DestruirObjeto(ref oGeneralParams);
            }
        }
    }
}
