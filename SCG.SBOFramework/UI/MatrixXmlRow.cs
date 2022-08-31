using System;
using System.Globalization;
using System.Xml;

namespace SCG.SBOFramework.UI
{
    public class MatrixXmlRow
    {
        public XmlNode XmlNode { get; set; }

        public MatrixXmlRow(XmlNode xmlNode)
        {
            XmlNode = xmlNode;
        }

        public int GetIntegerRow(string columnName)
        {
            return int.Parse(GetStringRow(columnName));
        }

        public float GetSingleRow(string columnName)
        {
            return float.Parse(GetStringRow(columnName));
        }

        public float GetSingleRow(string columnName, NumberFormatInfo numberFormatInfo)
        {
            return float.Parse(GetStringRow(columnName), numberFormatInfo);
        }

        public virtual string GetStringRow(string columnName)
        {
            var node = XmlNode.SelectSingleNode(string.Format(@"Columns/Column/Value[../ID = '{0}']", columnName));
            if (node == null || string.IsNullOrEmpty(node.InnerText))
                return string.Empty;
            return node.InnerText;
        }
    }
}