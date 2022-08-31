using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public partial class TrackingRepuestos
    {
        private UserDataSources UDS_Track;
        public static EditTextSBO txtRep;
        public static EditTextSBO txtNoOT;

        public void ManejadorEventoFormDataLoad()
        {
            Matrix m_objMatrix;

            try
            {
                UDS_Track = FormularioSBO.DataSources.UserDataSources;
                UDS_Track.Add("Rep", BoDataType.dt_LONG_TEXT, 100);
                UDS_Track.Add("ord", BoDataType.dt_LONG_TEXT, 100);

                txtRep = new EditTextSBO("txtRep", true, "", "Rep", FormularioSBO);
                txtRep.AsignaBinding();
                txtNoOT = new EditTextSBO("txtOrd", true, "", "ord", FormularioSBO);
                txtNoOT.AsignaBinding();

                txtNoOT.AsignaValorUserDataSource(strNoOT);
                txtRep.AsignaValorUserDataSource(strCode);

                m_objMatrix = (Matrix)FormularioSBO.Items.Item(g_strmtxTrack).Specific;
                m_objMatrix.LoadFromDataSource();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void ApplicationSBOOnItemEvent(String FormUID, ItemEvent pVal, ref Boolean BubbleEvent)
        {
            switch (pVal.EventType)
            {
                case BoEventTypes.et_ITEM_PRESSED:
                    ManejadorEventosItemPressed(FormUID, pVal, ref BubbleEvent);
                    break;
                case BoEventTypes.et_MATRIX_LINK_PRESSED:
                    ManejadorEventosMatrixLinkPress(FormUID, pVal, ref BubbleEvent);
                    break;
            }
        }


        private void ManejadorEventosItemPressed(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {
            SAPbouiCOM.Form oForm;
            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {

                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "btnAcep":
                                oForm.Close();
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ManejadorEventosMatrixLinkPress(string formUID, ItemEvent pVal, ref bool bubbleEvent)
        {
            SAPbouiCOM.Form oForm;
            string strDocNum;
            string strDocEntry;
            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    oForm = ApplicationSBO.Forms.Item(formUID);

                    if (pVal.BeforeAction)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "mtxTr":
                                if (pVal.ColUID == "ColID")
                                {
                                    oForm.Freeze(true);
                                    Matrix m_objMatrix = (Matrix)oForm.Items.Item(g_strmtxTrack).Specific;
                                    var editObjType = (SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_TDoc").Cells.Item(pVal.Row).Specific;
                                    //strDocNum = ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_NDoc").Cells.Item(pVal.Row).Specific).Value.Trim();
                                    //strDocEntry = ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_DocE").Cells.Item(pVal.Row).Specific).Value.Trim();
                                    SAPbouiCOM.LinkedButton oLink = (SAPbouiCOM.LinkedButton)m_objMatrix.Columns.Item("ColID").ExtendedObject;

                                    oLink.LinkedObjectType = editObjType.Value.Trim();
                                    var BoLinkedObject = (SAPbouiCOM.BoLinkedObject)Convert.ToInt32(editObjType.Value.Trim());
                                    oLink.LinkedObject=BoLinkedObject;
                                    oForm.Freeze(false);
                                }
                                break;
                        }
                    }
                    else if (pVal.ActionSuccess)
                    {
                        switch (pVal.ItemUID)
                        {
                            case "mtxTr":
                                //if (pVal.ColUID == "Col_NDoc")
                                //{
                                //    oForm.Freeze(true);
                                //    Matrix m_objMatrix = (Matrix)oForm.Items.Item(g_strmtxTrack).Specific;
                                //    strDocNum = ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_NDoc").Cells.Item(pVal.Row).Specific).Value.Trim();
                                //    strDocEntry = ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_DocE").Cells.Item(pVal.Row).Specific).Value.Trim();
                                //    ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_NDoc").Cells.Item(pVal.Row).Specific).Value = strDocEntry;
                                //    ((SAPbouiCOM.EditText)m_objMatrix.Columns.Item("Col_DocE").Cells.Item(pVal.Row).Specific).Value = strDocNum;
                                //    oForm.Freeze(false);
                                //}
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
