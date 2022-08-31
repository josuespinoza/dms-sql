using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SAPbouiCOM;

namespace SCG.DMSOne.Framework.MenuManager
{
    public class DmsOneMenusManager
    {
        public DmsOneMenusManager()
        {
            MenuEntries = new Dictionary<string, MenuEntry>();
        }

        protected Dictionary<string, MenuEntry> MenuEntries { get; private set; }

        public void AddMenuEntry(MenuEntry menuEntry)
        {
            if (menuEntry.FatherUid == menuEntry.Id)
                throw new RecursiveMenuException();
            MenuEntries.Add(menuEntry.Id, menuEntry);
        }

        protected void GenerateSubMenus()
        {
            List<MenuEntry> systemMenus = (from menuEntry in MenuEntries.Values
                                           where
                                               !MenuEntries.ContainsKey(menuEntry.FatherUid)
                                           select
                                               new MenuEntry(menuEntry.FatherUid, BoMenuType.mt_POPUP, "<None>", -1,
                                                             false, true, "-1") {IsSystemMenu = true}).ToList();
            foreach (MenuEntry systemMenu in systemMenus)
            {
                if (!MenuEntries.ContainsKey(systemMenu.Id))
                    MenuEntries.Add(systemMenu.Id, systemMenu);
            }
            foreach (MenuEntry menuEntry in MenuEntries.Values.OrderBy(entry => entry.Position))
            {
                if (!menuEntry.IsSystemMenu)
                    MenuEntries[menuEntry.FatherUid].SubMenus.Add(menuEntry);
            }
        }

        public string GenerateXml(MenuAction menuAction)
        {
            GenerateSubMenus();
            string xml = string.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter);
                xmlWriter.WriteStartElement("Application");
                xmlWriter.WriteStartElement("Menus");
                xmlWriter.WriteStartElement("action");
                xmlWriter.WriteAttributeString("type", menuAction == MenuAction.Add ? "add" : "remove");
                foreach (MenuEntry menuEntry in MenuEntries.Values)
                {
                    GenerateMenuEntry(xmlWriter, menuEntry, menuAction);
                }
                xmlWriter.WriteFullEndElement();
                xmlWriter.Close();
                xml = stringWriter.ToString();
            }
            return xml;
        }

        protected void GenerateMenuEntry(XmlWriter writer, MenuEntry menuEntry, MenuAction menuAction)
        {
            if (!menuEntry.WasAdded)
            {
                if (menuAction == MenuAction.Add)
                {
                    AddMenuTag(menuEntry, writer);
                    menuEntry.WasAdded = true;
                }
                foreach (MenuEntry subMenu in menuEntry.SubMenus)
                {
                    GenerateMenuEntry(writer, subMenu, menuAction);
                }
                if (menuAction == MenuAction.Remove)
                {
                    AddMenuTag(menuEntry, writer);
                    menuEntry.WasAdded = true;
                }
            }
        }

        protected void AddMenuTag(MenuEntry menuEntry, XmlWriter writer)
        {
            if (!menuEntry.IsSystemMenu)
            {
                writer.WriteStartElement("Menu");
                writer.WriteAttributeString("Checked", menuEntry.IsChecked ? "1" : "0");
                writer.WriteAttributeString("UniqueID", menuEntry.Id);
                writer.WriteAttributeString("String", menuEntry.Description);
                writer.WriteAttributeString("FatherUID", menuEntry.FatherUid);
                writer.WriteAttributeString("Position", menuEntry.Position.ToString());
                if (!string.IsNullOrEmpty(menuEntry.ImagePath))
                    writer.WriteAttributeString("Image", menuEntry.ImagePath);
                writer.WriteAttributeString("Type", ((int)menuEntry.Type).ToString());
                writer.WriteAttributeString("Enabled", menuEntry.IsEnabled ? "1" : "0");
                writer.WriteEndElement();
            }
        }
    }
}