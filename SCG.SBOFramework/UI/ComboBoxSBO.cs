using System;
using System.Collections.Generic;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class ComboBoxSBO : ControlSBO<IComboBox>, ISBOBindable
    {
        public ComboBoxSBO(IItem itemSBO) : base(itemSBO)
        {
        }

        public ComboBoxSBO(string uniqueId) : base(uniqueId)
        {
        }

        public ComboBoxSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public ComboBoxSBO(string uniqueId, IForm formularioSBO, bool ligada, string tablaLigada, string columnaLigada) : base(uniqueId, formularioSBO)
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
            if (Ligada)
                Especifico.DataBind.SetBound(Ligada, TablaLigada, ColumnaLigada);
        }

        public void AsignaValorUI(string valor)
        {
            Especifico.Select(valor, BoSearchKey.psk_ByValue);
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
                result = Especifico.Selected.Value;
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


        public virtual void CargaValoresValidos(IEnumerable<SboValidValue> valoresValidos,
                                                bool seleccionarPrimero)
        {
            if (Especifico != null)
            {
                foreach (SboValidValue valorValido in valoresValidos)
                {
                    Especifico.ValidValues.Add(valorValido.Value, valorValido.Description);
                }
                if (Especifico.ValidValues.Count != 0 && seleccionarPrimero)
                    Especifico.Select(0, BoSearchKey.psk_Index);
            }
        }
    }
}