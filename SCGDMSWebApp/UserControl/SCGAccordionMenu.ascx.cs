using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UserControl_SCGAccordionMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CrearMenuAccordion();

    }

    private string strProvider;

    public string Provider
    {
        get { return strProvider; }
        set { strProvider = value; }
    }


    /// <summary>
    /// crea el control de accordion
    /// </summary>
    private void CrearMenuAccordion()
    {
        
         for (int i = 0; i < SiteMap.Providers[Provider].RootNode.ChildNodes.Count; i++)
        {
            //GRABS SITEMAP MAIN ITEMS (UNDER HOME)
            SiteMapNode smn = (SiteMapNode)SiteMap.Providers[Provider].RootNode.ChildNodes[i];

            //CREATES ACCORDION PANE
            AjaxControlToolkit.AccordionPane p = new AjaxControlToolkit.AccordionPane();

            //CREATE UNIQUE PANE ID
            p.ID = "Pane" + i;
             p.HeaderCssClass="HeaderAccordionPane";
             p.ContentContainer.ScrollBars = ScrollBars.None;

       
            //CREATE HEADER ITEM
            HyperLink hlHeader = new HyperLink();
            hlHeader.NavigateUrl = SiteMap.Providers[Provider].RootNode.ChildNodes[i].Url.ToString();
            hlHeader.Text = SiteMap.Providers[Provider].RootNode.ChildNodes[i].Title.ToString();

            //ADDS HEADER LINK TO PANE (HEADER)
            p.HeaderContainer.Controls.Add(hlHeader);

            //CHECKS IF HEADER ITEM HAS CHILDREN
                if (smn.HasChildNodes)
                {
                    //CREATE BULLETED LIST OF CHILDREN
                    BulletedList blMenu = new BulletedList();
                    blMenu.DisplayMode = BulletedListDisplayMode.HyperLink;
                 
                    //CREATES LIST ITEMS WITHIN BULLETED LIST FOR CHILDREN
                    for (int j = 0; j < smn.ChildNodes.Count; j++)
                    {
                     
                     //   if smn.HasChildNodes()
                        blMenu.Items.Insert(0, (new ListItem(smn.ChildNodes[j].Title.ToString(), smn.ChildNodes[j].Url.ToString())));
                    }
                    //ADDS BULLETED LIST TO PANE (CONTAINER)
                    p.ContentContainer.Controls.Add(blMenu);
                }
                //ADDS PANE TO ACCORDION
     
                SCGAccordionMenu.Panes.Add(p);

        }
    }



}
