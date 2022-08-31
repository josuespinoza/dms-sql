using System;
using SAPbouiCOM;

namespace SCG.SBOFramework.UI
{
 
    public class FolderSBO:ControlSBO<IFolder>
    {
        public FolderSBO(IItem itemSBO) : base(itemSBO)
        {
        }

        public FolderSBO(string uniqueId, IForm formularioSBO) : base(uniqueId, formularioSBO)
        {
        }

        public FolderSBO(string uniqueId) : base(uniqueId)
        {
        }
    }
}