using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SCG.SBOFramework.UI
{
    public class SboMatrixXmlManager
    {
        public XmlDocument XmlDocument { get; protected set; }

        public SboMatrixXmlManager(string xml) 
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XmlDocument = xmlDocument;
        }

        public SboMatrixXmlManager(XmlDocument xmlDocument)
        {
            XmlDocument = xmlDocument;
        }

        public List<MatrixXmlRow> MatrixXmlRows()
        {
            List<MatrixXmlRow> matrixXmlRows = new List<MatrixXmlRow>();
            var xmlNodeList = XmlDocument.SelectNodes("/Matrix/Rows/Row");
            if (xmlNodeList != null)
                matrixXmlRows.AddRange(from XmlNode node in xmlNodeList select new MatrixXmlRow(node));
            matrixXmlRows.TrimExcess();
            return matrixXmlRows;
        }
    }
}