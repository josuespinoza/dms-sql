using System.Collections.Generic;
using SAPbouiCOM;

namespace SCG.DMSOne.Framework.MenuManager
{
    public class MenuEntry
    {
        public MenuEntry(string id, BoMenuType type, string description, int position, bool isChecked, bool isEnabled,
                         string fatherUid)
        {
            Id = id;
            Type = type;
            Description = description;
            Position = position;
            IsChecked = isChecked;
            IsEnabled = isEnabled;
            FatherUid = fatherUid;
            SubMenus = new List<MenuEntry>();
            IsSystemMenu = false;
            WasAdded = false;
        }

        public MenuEntry(string id, BoMenuType type, string description, int position, bool isChecked, bool isEnabled,
                         string imagePath, string fatherUid)
            : this(id, type, description, position, isChecked, isEnabled, fatherUid)
        {
            ImagePath = imagePath;
        }

        public BoMenuType Type { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public bool IsChecked { get; set; }
        public bool IsEnabled { get; set; }
        public string FatherUid { get; set; }
        public List<MenuEntry> SubMenus { get; set; }
        public string ImagePath { get; set; }
        public bool IsSystemMenu { get; set; }
        public bool WasAdded { get; set; }
    }
}