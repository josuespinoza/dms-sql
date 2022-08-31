using System;
using SAPbobsCOM;
using SAPbouiCOM;

namespace SCG.ServicioPostVenta
{
    public partial class TrackingSolEspecificos
    {
        public void ManejadorEventoFormDataLoad(ref string p_NoOT)
        {
            Matrix m_objMatrix;

            try
            {
                FormularioSBO.Freeze(true);                
         
                var query =
                    " SELECT SOLLIN.DocEntry Solic, SOLLIN.U_Cantidad Canti, SOLLIN.U_ItmCodeG ItemC, SOLLIN.U_ItmNomG Descrip, cast(SOL.U_FechaSol as nvarchar(12))  FecSol, cast(SOL.U_HoraSol as nvarchar(12)) HoraSol, SOLLIN.U_ItmCodeE ItemR, SOLLIN.U_NombEsp DescR," +
                    " isnull(cast(SOLLIN.U_FechResp as nvarchar(12)),'') FecRes, isnull(cast(SOLLIN.U_HoraResp as nvarchar(12)),'') HoraRes, SOLLIN.U_UserResp Usuario" +
                    " FROM [@SCGD_SOL_ESP_LIN] SOLLIN" +
                    " INNER JOIN [@SCGD_SOL_ESPEC] SOL on SOL.docentry = SOLLIN.docentry" +
                    " WHERE SOL.U_NumeroOT = '{0}'";

                g_dtTrack = FormularioSBO.DataSources.DataTables.Item(g_strdtTrack);
                g_dtTrack.ExecuteQuery(string.Format(query, p_NoOT));

                m_objMatrix = (Matrix)FormularioSBO.Items.Item(g_strmtxTrack).Specific;
                m_objMatrix.LoadFromDataSource();

                FormularioSBO.Freeze(false);
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
            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    if (pVal.BeforeAction)
                    {

                    }
                    else if (pVal.ActionSuccess)
                    {
                      
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
            try
            {
                if (string.IsNullOrEmpty(formUID) == false)
                {
                    if (pVal.BeforeAction)
                    {

                    }
                    else if (pVal.ActionSuccess)
                    {

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
