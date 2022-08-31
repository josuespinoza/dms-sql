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

/// <summary>
/// Creado por thuertas -05-07-2010
/// Menu Acordion HTML-CSS-Jquery
/// </summary>
public partial class UserControl_SCGJqueryAccordionMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ltrAcordion.Text= "<div  id='firstpane' class='menu_list'> ";
        ltrAcordion.Text+= DevuelveNodos(SiteMap.Providers[Provider].RootNode);
        ltrAcordion.Text += "</div>";
     
    }




    private string strProvider;

    public string Provider
    {
        get { return strProvider; }
        set { strProvider = value; }
    }



    /// <summary>
    /// crea la estructura HTML del Accordion
    /// </summary>
    private string DevuelveNodos(SiteMapNode stmapNode)
    {
        string str = "";

        if (stmapNode.HasChildNodes)
        {
            IEnumerator rootChildNodes = stmapNode.ChildNodes.GetEnumerator();

            while(rootChildNodes.MoveNext())
            {
                SiteMapNode NodoHijo = (SiteMapNode)rootChildNodes.Current;

                if (NodoHijo.HasChildNodes)
                {
                    if (NodoHijo.ParentNode.Equals(SiteMap.RootNode))
                    {
                        str += "<p  class= 'menu_head'>" + NodoHijo.Title + "</p>";
                        str += "<div class='menu_body'> ";
                        str += DevuelveNodos(NodoHijo);
                        str += "</div>";
                    }
                    else
                    {
                        str += "<p  class= 'menu_head2'>" + NodoHijo.Title + "</p>";
                        str += "<div class='menu_body'> ";
                        str += DevuelveNodos(NodoHijo);
                        str += "</div>";
                    }
                }
                else
                {
                    if (NodoHijo.ParentNode.Equals(SiteMap.RootNode))
                    {
                        str += "<p class= 'menu_head'>" + "<a href='" +AbsolutePath( NodoHijo.Url) + "' >" + NodoHijo.Title + "</a></p>";
                    }
                    else
                    {
                        str += "<a href='" + AbsolutePath(NodoHijo.Url) + "'>" + NodoHijo.Title + "</a>";
                    }
                }

            }
        }
        return str;
    }


  /// <summary>
  /// arma la ruta absoluta de la pagina web
  /// esto para no restar compatibilidad a otros controles ASP.NET que si entienden ~/archivo
  /// </summary>
  /// <param name="file"></param>
  /// <returns></returns>
    protected string AbsolutePath(String file)
   {
       if (file != "")
       {
           String end = (Request.ApplicationPath.EndsWith("/")) ? "" : "/";
           String path = Request.ApplicationPath + end;

           file = file.Remove(0, 2);

           return String.Format("http://{0}{1}{2}", Request.Url.Authority, path, file);
       }
       return "";
   }

}
