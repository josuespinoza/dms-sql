using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Impuesto
{
   public class Impuesto
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineNum { get; set; }
        public Int32 DocType { get; set; }
        public Int32 BusArea { get; set; }
        public Int32 Cond1 { get; set; }
        public String UDFTable1 { get; set; }
        public Int32 NumVal1 { get; set; }
        public String StrVal1 { get; set; }
        public double MnyVal1 { get; set; }
        public Int32 Cond2 { get; set; }
        public String UDFTable2 { get; set; }
        public Int32 NumVal2 { get; set; }
        public String StrVal2 { get; set; }
        public double MnyVal2 { get; set; }
        public Int32 Cond3 { get; set; }
        public String UDFTable3 { get; set; }
        public Int32 NumVal3 { get; set; }
        public String StrVal3 { get; set; }
        public double MnyVal3 { get; set; }
        public Int32 Cond4 { get; set; }
        public String UDFTable4 { get; set; }
        public Int32 NumVal4 { get; set; }
        public String StrVal4 { get; set; }
        public double MnyVal4 { get; set; }
        public Int32 Cond5 { get; set; }
        public String UDFTable5 { get; set; }
        public Int32 NumVal5 { get; set; }
        public String StrVal5 { get; set; }
        public double MnyVal5 { get; set; }
        public String Descr { get; set; }
        public String LnTaxCode { get; set; }
        public String FrLnTax { get; set; }
        public String FrHdrTax { get; set; }
        public String UDFAlias1 { get; set; }
        public String UDFAlias2 { get; set; }
        public String UDFAlias3 { get; set; }
        public String UDFAlias4 { get; set; }
        public String UDFAlias5 { get; set; }
    }
}
