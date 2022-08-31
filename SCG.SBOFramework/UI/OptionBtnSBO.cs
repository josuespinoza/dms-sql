using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class OptionBtnSBO : ControlSBO<IOptionBtn> , ISBOBindable
    {
        public OptionBtnSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        public OptionBtnSBO(string uniqueId) : this(uniqueId, false, string.Empty, string.Empty, null)
        {
        }

        public OptionBtnSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public OptionBtnSBO(string uniqueId, bool ligada, string tablaLigada, string columnaLigada, IForm formularioSBO)
            : base(uniqueId, formularioSBO)
        {
            Ligada = ligada;
            TablaLigada = tablaLigada;
            ColumnaLigada = columnaLigada;
        }

        #region ISBOBindable

        public bool Ligada{ get; set;}

        public string ColumnaLigada{ get; set;}

        public string TablaLigada{ get; set;}

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

        public void AsignaValorUI(string valor)
        {
            throw new NotImplementedException();
        }

        public string ObtieneValorUI()
        {
            throw new NotImplementedException();
        }

        #endregion 

    }
}
