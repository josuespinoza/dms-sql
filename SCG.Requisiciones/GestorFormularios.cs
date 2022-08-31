using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SCG.SBOFramework.UI;

namespace SCG.Requisiciones
{
    public class GestorFormularios
    {
        private SAPbouiCOM.Application _sboApplication;

        public GestorFormularios(SAPbouiCOM.Application sboApplication)
        {
            _sboApplication = sboApplication;
        }

        public bool FormularioAbierto(IFormularioSBO formulario, Boolean activarSiEstaAbierto)
        {
            SAPbouiCOM.Form sboForm;

            for (int indice = 0; indice < _sboApplication.Forms.Count - 1; indice++)
            {
                sboForm = _sboApplication.Forms.Item(indice);
                if (sboForm.TypeEx == formulario.FormType)
                {
                    if (activarSiEstaAbierto)
                        sboForm.Select();
                    return true;
                }
            }
            return false;
        }

        public SAPbouiCOM.Form CargaFormulario(IFormularioSBO formulario)
        {
            FormCreationParams fcp;

            fcp = (FormCreationParams)(_sboApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams));
            fcp.FormType = formulario.FormType;
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;
            fcp.UniqueID = formulario.FormType;
            Form sboForm = CargarDesdeXML(ref fcp, formulario);
            formulario.FormularioSBO = sboForm;
            formulario.Inicializado = false;
            formulario.InicializarControles();
            formulario.InicializaFormulario();
            return sboForm;
        }

        private SAPbouiCOM.Form CargarDesdeXML(ref FormCreationParams fcp, IFormularioSBO formulario)
        {
            SAPbouiCOM.Form oForm = null;
            try
            {
                fcp.XmlData = File.ReadAllText(formulario.NombreXml);
                oForm = _sboApplication.Forms.AddEx(fcp);
            }
            catch (Exception ex)
            {
                throw;
            }
            return oForm;
        }


    }
}
