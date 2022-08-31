using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    /// <summary>
    /// Represents an SBO EditText
    /// </summary>
    public class EditTextSBO : ControlSBO<IEditText>, ISBOBindable
    {
        public EditTextSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        public EditTextSBO(string uniqueId) : this(uniqueId, false, string.Empty, string.Empty, null)
        {
        }

        public EditTextSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public EditTextSBO(string uniqueId, bool ligada, string tablaLigada, string columnaLigada, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
            Ligada = ligada;
            TablaLigada = tablaLigada;
            ColumnaLigada = columnaLigada;
        }

        #region ISBOBindable Members

        public bool Ligada { get; set; }

        public string ColumnaLigada { get; set; }

        public string TablaLigada { get; set; }

        public void AsignaBinding()
        {
           Especifico.DataBind.SetBound(Ligada, TablaLigada, ColumnaLigada);
        }

        public void AsignaValorUI(string valor)
        {
            Especifico.Value = valor;
        }

        public void AsignaValorDataSource(string valor)
        {
            if (FormularioSBO != null)
            {
                DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
                dbDataSource.SetValue(ColumnaLigada, dbDataSource.Offset, valor);
            }
        }

        public string ObtieneValorDataSource()
        {
            string result = string.Empty;
            if (FormularioSBO != null)
            {
                DBDataSource dbDataSource = FormularioSBO.DataSources.DBDataSources.Item(TablaLigada);
                result = dbDataSource.GetValue(ColumnaLigada, dbDataSource.Offset).TrimEnd();
            }
            return result;
        }

        public string ObtieneValorUI()
        {
            string result = string.Empty;
            if (FormularioSBO != null)
            {
                result = Especifico.Value;
            }
            return result;
        }

        public void AsignaValorUserDataSource(string valor)
        {
            if (FormularioSBO != null)
            {
                UserDataSources userDataSources = FormularioSBO.DataSources.UserDataSources;
                userDataSources.Item(ColumnaLigada).ValueEx = valor;
            }
        }

        public string ObtieneValorUserDataSource()
        {
            UserDataSources userDataSources = FormularioSBO.DataSources.UserDataSources;
            return userDataSources.Item(ColumnaLigada).ValueEx;
        }

        #endregion
    }
}
