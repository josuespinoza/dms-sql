using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SCGInicio : System.Web.UI.Page
{
    /// <summary>
    /// controla si se actualiza o no el cache del sitemap
    /// </summary>
    bool ActualizarCache
    {
        get
        {
            if (Session["ResetCache"] != null)
            {
                return Convert.ToBoolean(Session["ResetCache"].ToString());
            }
            else
            {
                return false;
            }
        }
        set
        {
            Session["ResetCache"] = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    /// <summary>
    /// inicializa la pagina en la cultura correspondiente
    /// </summary>
    protected override void InitializeCulture()
    {
        string culture="";
        try
        {
         culture = Request.Form["ddlIdioma"];

         if (string.IsNullOrEmpty(culture) || culture.ToLower() == "auto") culture = "es-cr";
            //Use this
            this.UICulture = culture;
            this.Culture = culture;
            //OR This
            if (culture.ToLower() != "es-cr" )
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            }
        }
        catch
        {
            throw new  Exception();
        }
        finally
        {
            if (culture.ToLower() != "es-cr")
            {
                Session["Cultura"] = culture;
                //agregar info para el dashboard
             
            }
            else
            {
                Session["Cultura"] = "es-cr";

            }
            base.InitializeCulture();
        }

    }
 
    protected void logSCG_LoggingIn(object sender, LoginCancelEventArgs e)
    {

    }
    /// <summary>
    /// actualiza el cache del sitemap
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void loginSCG_LoggedIn(object sender, EventArgs e)
    {
        ActualizarCache = true;
    }
}
