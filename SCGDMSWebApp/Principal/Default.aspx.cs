using System;
using System.Web;


public partial class Principal_Default : SCGSeguridadWeb.SCGPageBehavior
{
    protected void Page_Load(object sender, EventArgs e)
    {
      

        this.FlashControl1.FlashVars = "LanguageID="+ HttpContext.Current.Session["Cultura"].ToString();
        this.FlashControl1.FlashVars += "&URL=" +
                                        System.Configuration.ConfigurationManager.AppSettings["DashboardVirtualPath"];
        FlashControl1.MovieUrl = "~/SWF/DMS/DMSAnalytics_3.5.swf";
    }
}
