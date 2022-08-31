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
    public class InterfaceJohnDeere_XFER
    {
        public IApplication oApplicationSBO { get; private set; }
        public ICompany oCompanySBO { get; private set; }
        public SAPbobsCOM.Company SBOCompany;

        public SAPbouiCOM.Form oForm { get; set; }

        private static NumberFormatInfo n;

        private String g_strTransferCoordination = string.Empty;

        #region Constructor
        public InterfaceJohnDeere_XFER(IApplication applicationSBO, ICompany companySBO, SAPbouiCOM.Form p_oForm)
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
        public void ManejaInterfaceJohnDeere_XFER(ref string p_strRuta, ref string p_strDocEntry)
        {
            List<DPMXFER> oListDPMXFER;
            try
            {
                if (!string.IsNullOrEmpty(p_strRuta))
                {
                    oListDPMXFER = new List<DPMXFER>();
                    LecturaArchivo_XFER(ref oListDPMXFER, ref p_strRuta);
                    if (ValidarProcesaArchivo(ref g_strTransferCoordination))
                    {
                        if (oListDPMXFER.Count > 0)
                        {
                            CreacionTransferenciaStock_XFER(ref oListDPMXFER, ref p_strDocEntry);
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

        public void CreacionTransferenciaStock_XFER(ref List<DPMXFER> p_oListDMPXFER, ref string p_strDocEntry)
        {
            StockTransfer oStockTransfer;
            Int32 intErrorCode;
            String strErrorMensaje;
            try
            {
                oApplicationSBO.StatusBar.SetText("Creando Transferencia de Stock", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                oStockTransfer = (StockTransfer)SBOCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);

                // Información Encabezado
                oStockTransfer.UserFields.Fields.Item("U_SCGD_TranC").Value = g_strTransferCoordination;
                // Información Lineas
                foreach (DPMXFER  oRow in p_oListDMPXFER)
                {
                    if (!string.IsNullOrEmpty(oRow.PartNumber))
                    {
                        oStockTransfer.Lines.ItemCode = oRow.PartNumber;
                        oStockTransfer.Lines.Quantity = oRow.TransferQuantity;
                        oStockTransfer.Lines.FromWarehouseCode = oRow.FromWarehouse;
                        oStockTransfer.Lines.WarehouseCode= oRow.ToWarehouse;

                        oStockTransfer.Lines.Add();
                    }
                }

                StartTransaction();

                if (oStockTransfer.Add() == 0)
                {
                    p_strDocEntry = SBOCompany.GetNewObjectKey();
                    CommitTransaction();
                    oApplicationSBO.StatusBar.SetText("Proceso Exitoso", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    RollBackTransaction();
                    intErrorCode = SBOCompany.GetLastErrorCode();
                    strErrorMensaje = SBOCompany.GetLastErrorDescription();
                    oApplicationSBO.StatusBar.SetText(string.Format("{0}: {1}", intErrorCode, strErrorMensaje), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }

            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

        public Boolean ValidarProcesaArchivo(ref String p_strTransferCoordination)
        {
            SAPbouiCOM.Condition oCondition;
            SAPbouiCOM.Conditions oConditions;
            DBDataSource dsInformation;
            try
            {
                if (oForm != null)
                {
                    //*** Carga Encabezado ******
                    oForm.DataSources.DBDataSources.Add("OWTR");
                    dsInformation = oForm.DataSources.DBDataSources.Item("OWTR");

                    oConditions = (SAPbouiCOM.Conditions)oApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);

                    oCondition = oConditions.Add();
                    oCondition.BracketOpenNum = 1;
                    oCondition.Alias = "U_SCGD_TranC";
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCondition.CondVal = p_strTransferCoordination;
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

        public void LecturaArchivo_XFER(ref List< DPMXFER> p_oListDMPXFER, ref string p_strRuta)
        {
            DPMXFER oDpmXFER;
            string strLine;
            string[] mtxValores;
            try
            {
                using (StreamReader ReaderObject = new StreamReader(p_strRuta))
                {
                    while ((strLine = ReaderObject.ReadLine()) != null)
                    {
                        oDpmXFER = new DPMXFER();

                        mtxValores = strLine.Split('\t');

                        if (mtxValores.GetValue(0).ToString() == "TRNSFR")
                        {
                            oApplicationSBO.StatusBar.SetText("Cargando archivo", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                            oDpmXFER.FileHeaderID = mtxValores.GetValue(0).ToString();
                            oDpmXFER.TransferCoordination = Convert.ToInt32(mtxValores.GetValue(1));
                            g_strTransferCoordination = oDpmXFER.TransferCoordination.ToString();
                        }
                        else
                        {
                            oDpmXFER.PartNumber = mtxValores.GetValue(0).ToString();
                            String aaa = mtxValores.GetValue(1).ToString();
                            oDpmXFER.TransferQuantity = Convert.ToDouble(mtxValores.GetValue(1));
                            //oDpmXFER.TransferDate = DateTime.ParseExact(mtxValores.GetValue(2).ToString(), "yyyyMMdd", n);
                            //oDpmXFER.TransferTime = DateTime.ParseExact(mtxValores.GetValue(3).ToString(), "HH:MM:SS", n);
                            oDpmXFER.FromDealerAccount = mtxValores.GetValue(4).ToString();
                            oDpmXFER.FromWarehouse = mtxValores.GetValue(5).ToString();
                            oDpmXFER.ToDealerAccount = mtxValores.GetValue(6).ToString();
                            oDpmXFER.ToWarehouse = mtxValores.GetValue(7).ToString();
                        }
                        p_oListDMPXFER.Add(oDpmXFER);
                    }
                }

                if (p_oListDMPXFER.Count == 0)
                {
                    oApplicationSBO.StatusBar.SetText("Problemas con la carga del archivo", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
                oApplicationSBO.StatusBar.SetText("Problemas con la carga del archivo", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error );
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
