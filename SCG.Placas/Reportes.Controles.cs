using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using SCG.SBOFramework.UI.Extensions;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Placas
{
    public partial class Reportes : IUsaMenu
    {
        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        #endregion

        public ICompany CompanySBO { get; private set; }

        public IApplication ApplicationSBO { get; private set; }
    }
}