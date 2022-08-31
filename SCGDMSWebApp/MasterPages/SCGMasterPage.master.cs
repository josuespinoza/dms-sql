using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MasterPages_SCGMasterPage : System.Web.UI.MasterPage
{

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    

        //verificar cultura
        try
        {
            if (Session["Cultura"].ToString().ToLower() == "en-us")
            {
                SCGJqueryAccordionMenu1.Provider = SiteMap.Providers["SCGSiteMapProviderEN"].Name;
                SCGSitemapPath.SiteMapProvider = SiteMap.Providers["SCGSiteMapProviderEN"].Name;
                SCGLoginStatus.LogoutText = (string) System.Web.HttpContext.GetLocalResourceObject("~/SCGInicio.aspx", "MasterPages_SCGMasterPage_Page_Load_Log_Out");
            }
            else
            {
                SCGJqueryAccordionMenu1.Provider = SiteMap.Providers["SCGSiteMapProvider"].Name;
                SCGSitemapPath.SiteMapProvider = SiteMap.Providers["SCGSiteMapProvider"].Name;
                SCGLoginStatus.LogoutText = (string)System.Web.HttpContext.GetLocalResourceObject("~/SCGInicio.aspx", "MasterPages_SCGMasterPage_Page_Load_Cerrar_Sesi\xf3n");
            }
        }
        catch
        {
            Response.Redirect("~/SCGInicio.aspx");
        }
      
     
    }


    
 
}
