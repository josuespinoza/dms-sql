using System;
using System.IO;
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
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Integration.InterfaceDPM
{
    public class InterfaceJohnDeere_ORD
    {
        public IApplication oApplicationSBO { get; private set; }
        public ICompany oCompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;

        public SAPbouiCOM.Form oForm { get; set; }

        private static NumberFormatInfo n;

        private String g_strOrderCoordination= string.Empty ;

        #region Constructor

        public InterfaceJohnDeere_ORD(IApplication applicationSBO, ICompany companySBO, SAPbouiCOM.Form p_oForm)
        {
            try
            {
                oApplicationSBO = applicationSBO;
                oCompanySBO = companySBO;
                SBOCompany = (SAPbobsCOM.Company) companySBO;
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

        public void ManejaInterfaceJohnDeere_ORD(ref string p_strRuta, ref string p_strDocEntry)
        {
            List<DPMORD> oListDPMORD;
            try
            {
                if (!string.IsNullOrEmpty(p_strRuta))
                {
                    oListDPMORD = new List<DPMORD>();
                    LecturaArchivo_ORD(ref oListDPMORD, ref p_strRuta);
                    if (ValidarProcesaArchivo(ref g_strOrderCoordination))
                    {
                        if (oListDPMORD.Count > 0)
                        {
                            CreacionOrdenCompra_ORD(ref oListDPMORD, ref p_strDocEntry);
                        }
                    }
                    else
                    {
                        oApplicationSBO.StatusBar.SetText("El archivo seleccionado ya se encuentra procesado en el sistema", BoMessageTime.bmt_Short,
                            BoStatusBarMessageType.smt_Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void LecturaArchivo_ORD(ref List<DPMORD> p_oListDMPORD, ref string p_strRuta)
        {
            DPMORD oDpmord;
            string strLine;
            string[] mtxValores;
            try
            {
                using (StreamReader ReaderObject = new StreamReader(p_strRuta))
                {
                    while ((strLine = ReaderObject.ReadLine()) != null)
                    {
                        oDpmord = new DPMORD();

                        mtxValores = strLine.Split('\t');

                        if (mtxValores.GetValue(0).ToString() == "ORDER")
                        {
                            oApplicationSBO.StatusBar.SetText("Cargando archivo", BoMessageTime.bmt_Short,
                                BoStatusBarMessageType.smt_Warning);
                            oDpmord.FileHeaderID = mtxValores.GetValue(0).ToString();
                            oDpmord.OrderCoordination = Convert.ToInt32(mtxValores.GetValue(1));
                            g_strOrderCoordination = oDpmord.OrderCoordination.ToString();
                        }
                        else
                        {
                            oDpmord.DealerAccount = mtxValores.GetValue(0).ToString();
                            oDpmord.DBSWarehouse = mtxValores.GetValue(1).ToString();
                            oDpmord.OrderActivity = mtxValores.GetValue(2).ToString();
                            //oDpmord.OrderDate = DateTime.ParseExact(mtxValores.GetValue(3).ToString(), "yyyyMMdd", n);
                            //oDpmord.OrderTime = DateTime.ParseExact(mtxValores.GetValue(4).ToString(), "HH:MM:SS", n);
                            oDpmord.OrderType = mtxValores.GetValue(5).ToString();
                            oDpmord.OrderSource = Convert.ToInt32(mtxValores.GetValue(6));
                            oDpmord.OriginalOrderLineID = mtxValores.GetValue(7).ToString();
                            oDpmord.PartNumber = mtxValores.GetValue(8).ToString();
                            oDpmord.OrderQuantity = Convert.ToDouble(mtxValores.GetValue(9).ToString());
                            oDpmord.OrderReferenceID = mtxValores.GetValue(10).ToString();
                        }

                        p_oListDMPORD.Add(oDpmord);
                    }
                }

                if (p_oListDMPORD.Count == 0)
                {
                    oApplicationSBO.StatusBar.SetText("Problemas con la carga del archivo", BoMessageTime.bmt_Short,
                        BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                oApplicationSBO.StatusBar.SetText("Problemas con la carga del archivo", BoMessageTime.bmt_Short,
                    BoStatusBarMessageType.smt_Error);
            }
        }

        public void CreacionOrdenCompra_ORD(ref List<DPMORD> p_oListDMPORD, ref string p_strDocEntry)
        {
            Documents oOrdenCompra;
            Int32 intErrorCode;
            String strErrorMensaje;
            String strCardCode = string.Empty;
            try
            {
                oApplicationSBO.StatusBar.SetText("Creando Orden de Compra", BoMessageTime.bmt_Short,
                    BoStatusBarMessageType.smt_Warning);
                oOrdenCompra = (Documents) SBOCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);

                // Información Encabezado
                CargaSocioNegocioCompra(ref strCardCode);
                oOrdenCompra.CardCode = strCardCode;
                oOrdenCompra.UserFields.Fields.Item("U_SCGD_OrderC").Value = g_strOrderCoordination;
                // Información Lineas
                foreach (DPMORD oRow in p_oListDMPORD)
                {
                    if (!string.IsNullOrEmpty(oRow.PartNumber))
                    {
                        oOrdenCompra.Lines.ItemCode = oRow.PartNumber;
                        oOrdenCompra.Lines.Quantity = oRow.OrderQuantity;
                        oOrdenCompra.Lines.WarehouseCode = oRow.DBSWarehouse;

                        oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_JD_OA").Value = oRow.OrderActivity;
                        oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_JD_OT").Value = oRow.OrderType;
                        oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_JD_OS").Value = oRow.OrderSource.ToString();
                        oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_JD_LineID").Value = oRow.OriginalOrderLineID;
                        oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_JD_RefID").Value = oRow.OrderReferenceID;

                        oOrdenCompra.Lines.Add();
                    }
                }

                StartTransaction();

                if (oOrdenCompra.Add() == 0)
                {
                    p_strDocEntry = SBOCompany.GetNewObjectKey();
                    CommitTransaction();
                    oApplicationSBO.StatusBar.SetText("Proceso Exitoso", BoMessageTime.bmt_Short,
                        BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    RollBackTransaction();
                    intErrorCode = SBOCompany.GetLastErrorCode();
                    strErrorMensaje = SBOCompany.GetLastErrorDescription();
                    oApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intErrorCode, strErrorMensaje),
                        BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void CargaSocioNegocioCompra(ref String p_strCardCode)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                    //*** Carga Encabezado ******
                    oForm.DataSources.DBDataSources.Add("@SCGD_JD");
                    dsInformation = oForm.DataSources.DBDataSources.Item("@SCGD_JD");

                    oConditions =
                        (SAPbouiCOM.Conditions) oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType
                            .cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "Code";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = "JD1";
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);
                    for (int index = 0; index < dsInformation.Size; index++)
                    {
                        p_strCardCode = !string.IsNullOrEmpty(dsInformation.GetValue("U_CardCodeP", index))
                            ? dsInformation.GetValue("U_CardCodeP", index).ToString().Trim()
                            : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public Boolean ValidarProcesaArchivo(ref String p_strOrderCoordination)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                    //*** Carga Encabezado ******
                    oForm.DataSources.DBDataSources.Add("OPOR");
                    dsInformation = oForm.DataSources.DBDataSources.Item("OPOR");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "U_SCGD_OrderC";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_strOrderCoordination;
                    oCondition.BracketCloseNum = 1;

                    dsInformation.Query(oConditions);
                    if (dsInformation.Size > 0)
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                return false;
            }
        }

        public void StartTransaction()
        {
            try
            {
                if (!SBOCompany.InTransaction)
                {
                    SBOCompany.StartTransaction();
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void RollBackTransaction()
        {
            try
            {
                if (SBOCompany.InTransaction)
                {
                    SBOCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (SBOCompany.InTransaction)
                {
                    SBOCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }
        #endregion 
    }
}
