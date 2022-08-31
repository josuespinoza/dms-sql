using System;
using System.IO;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.ServicioPostVenta
{
    public class GestorFormularios
    {
        private Application g_ApplicationSBO;
        public GestorFormularios(ref Application p_SBOApplication)
        {
            g_ApplicationSBO = p_SBOApplication;
        }

        public bool FormularioAbierto(IFormularioSBO formulario, Boolean ActivarSiEstaAbierto )
        {
            Form sboForm;

            for (int indice = 0; indice < g_ApplicationSBO.Forms.Count; indice++ )
            {
                sboForm = g_ApplicationSBO.Forms.Item(indice);
                if (sboForm.TypeEx == formulario.FormType )
                {
                    if (ActivarSiEstaAbierto )
                    {
                        sboForm.Select();
                    }
                    return true;
                }
            }
            return false;
        }

        public Form CargarFormulario(IFormularioSBO formulario)
        {
            FormCreationParams fcp;
            fcp = (FormCreationParams) g_ApplicationSBO.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
            fcp.FormType = formulario.FormType;

            Form sboForm = CargarDesdeXML(fcp, formulario);
            formulario.FormularioSBO = sboForm;
            formulario.Inicializado = true;
            formulario.InicializarControles();
            formulario.InicializaFormulario();
            return sboForm;
        }

        private Form CargarDesdeXML(FormCreationParams fcp, IFormularioSBO formulario )
        {
            fcp.XmlData = File.ReadAllText(formulario.NombreXml);
            return g_ApplicationSBO.Forms.AddEx(fcp);
        }

    }
}
