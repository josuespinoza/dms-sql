using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
    public class StaticTextSBO : ControlSBO<IStaticText>
    {
        public StaticTextSBO(IItem itemSBO)
            : base(itemSBO)
        {
        }

        public StaticTextSBO(string uniqueId) : base(uniqueId)
        {
        }

        public StaticTextSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }
    }
}