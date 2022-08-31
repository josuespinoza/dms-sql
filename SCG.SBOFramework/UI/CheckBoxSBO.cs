using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    /// <summary>
    /// Represents a check box in a SBO form.
    /// </summary>
    public class CheckBoxSBO : ControlSBO<ICheckBox>
    {
        public CheckBoxSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        public CheckBoxSBO(string uniqueId, IForm formularioSBO)
            : base(uniqueId, formularioSBO)
        {
        }

        public CheckBoxSBO(string uniqueId)
            : base(uniqueId)
        {
        }

        public CheckBoxSBO(string uniqueId, bool ligada, string tablaLigada, string columnaLigada, IForm formularioSBO)
            : base(uniqueId, formularioSBO)
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
