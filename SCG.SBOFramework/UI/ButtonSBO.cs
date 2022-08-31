using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    /// <summary>
    /// Represents a button in a SBO form.
    /// </summary>
    public class ButtonSBO : ControlSBO<IButton>
    {
        public ButtonSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        public ButtonSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public ButtonSBO(string uniqueId) : base(uniqueId)
        {
        }
    }
}