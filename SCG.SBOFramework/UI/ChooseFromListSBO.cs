using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public delegate void ItemEvent<TSender>(TSender sender, IItemEvent pVal, out bool bubbleEvent);

    /// <summary>
    /// Represents a choose from list control in a SBO form.
    /// </summary>
    public class ChooseFromListSBO
    {
        private IItem _itemSBO;

        public ChooseFromListSBO(IItem itemSBO)
        {
            ItemSBO = itemSBO;
        }

        public ChooseFromListSBO(string uniqueId)
        {
            UniqueId = uniqueId;
        }

        public string UniqueId { get; set; }

        public IItem ItemSBO
        {
            get { return _itemSBO; }
            set
            {
                _itemSBO = value;
                UniqueId = _itemSBO.UniqueID;
            }
        }
    }
}