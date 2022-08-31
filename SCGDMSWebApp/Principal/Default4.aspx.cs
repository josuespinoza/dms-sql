using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Principal_Default4 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.QueryString.Count>0)
        {
            txtNumOT.Text = Request.QueryString["OT"].ToString();
            txtStatus.Text = Request.QueryString["STAT"].ToString();


        }
    }
    protected void ddlTipoPublicidad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlTipoPublicidad_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddlUbicGeo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
