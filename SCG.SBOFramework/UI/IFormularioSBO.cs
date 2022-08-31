using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public interface IFormularioSBO
    {
        string FormType { get; set; }
        string NombreXml { get; set; }
        string Titulo { get; set; }
        IForm FormularioSBO { get; set; }
        bool Inicializado { get; set; }
        void InicializarControles();
        void InicializaFormulario();
        IApplication ApplicationSBO { get; }
        SAPbobsCOM.ICompany CompanySBO { get; }
    }
}