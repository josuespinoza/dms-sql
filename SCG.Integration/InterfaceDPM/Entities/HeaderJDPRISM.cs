using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;

namespace SCG.Integration.InterfaceDPM.Entities
{
    public class HeaderJDPRISM
    {
        public String HeaderRecordCode { get; set; }
        public DateTime DateOfExtract { get; set; }
        public DateTime TimeOfExtract { get; set; }
        public String TypeOfExtract { get; set; }
        public String InterfaceVersion { get; set; }
        public String DBSName { get; set; }
        public String DBSVersion { get; set; }
        public int OrdenCoordinationData { get; set; }
        public int TransferCoordinationData { get; set; }
        public int OrderAndTransferFilesProcessed { get; set; }

        public void ToString(ref StringBuilder p_sb)
        {
            String espacio = "\t";
            String vacio = "";
            try
            {
                p_sb.Append(HeaderRecordCode).Append(espacio);
                p_sb.Append(DateOfExtract.ToString("yyyy-MM-dd")).Append(espacio);
                p_sb.Append(TimeOfExtract.ToString("hh:mm:ss")).Append(espacio);
                p_sb.Append(TypeOfExtract).Append(espacio);
                p_sb.Append(InterfaceVersion).Append(espacio);
                p_sb.Append(DBSName).Append(espacio);
                p_sb.Append(DBSVersion).Append(espacio);
                p_sb.Append((OrdenCoordinationData > 0) ? OrdenCoordinationData.ToString() : vacio).Append(espacio);
                p_sb.Append((TransferCoordinationData > 0) ? TransferCoordinationData.ToString() : vacio).Append(espacio);
                p_sb.Append((OrderAndTransferFilesProcessed > 0) ? OrderAndTransferFilesProcessed.ToString() : vacio).Append(espacio);
                p_sb.Append("\r\n");
            }
            catch (Exception ex)
            {
                DMS_Connector.Helpers.ManejoErrores(ex);
            }
        }

    }
}
