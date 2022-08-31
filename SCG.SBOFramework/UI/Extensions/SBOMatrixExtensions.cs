using SAPbouiCOM;

namespace SCG.SBOFramework.UI.Extensions
{
    public static class SboMatrixExtensions
    {
        public static SboMatrixXmlManager MatrixXmlManager(this IMatrix matrix)
        {
            return new SboMatrixXmlManager(matrix.SerializeAsXML(BoMatrixXmlSelect.mxs_All));
        }
    }
}