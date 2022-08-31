using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    /// <summary>
    /// Abstract class for defining common members to SBO controls.
    /// </summary>
    /// <typeparam name="TEspecifico">Specific SBO control type</typeparam>
    public abstract class ControlSBO<TEspecifico>
    {
        private IItem _itemSBO;

        protected ControlSBO(IItem itemSBO)
        {
            ItemSBO = itemSBO;
        }

        protected ControlSBO(string uniqueId, IForm formularioSBO)
        {
            UniqueId = uniqueId;
            FormularioSBO = formularioSBO;
            if (formularioSBO != null) 
                ItemSBO = formularioSBO.Items.Item(uniqueId);
        }

        protected ControlSBO(string uniqueId)
        {
            UniqueId = uniqueId;
        }

        public string UniqueId { get; set; }
        public TEspecifico Especifico { get; set; }
        public IForm FormularioSBO { get; set; }

        public IItem ItemSBO
        {
            get { return _itemSBO; }
            set
            {
                _itemSBO = value;
                UniqueId = _itemSBO.UniqueID;
                Especifico = (TEspecifico) _itemSBO.Specific;
            }
        }

        public void HabilitarBuscar()
        {
            if (ItemSBO != null)
            {
                ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            }
        }
    }
}