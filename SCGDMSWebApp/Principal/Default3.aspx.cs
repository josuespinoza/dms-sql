using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Principal_Default3 : SCGSeguridadWeb.SCGPageBehavior
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        this.SCGWOStatusDisplay1.MillisecondsLoop = 60000;
       
        this.SCGWOStatusDisplay1.RedirectUrl = "Default4.aspx";
        
        this.SCGWOStatusDisplay1.Datasource = SqlDataSource1;
    }


    //public string DevolverColor(int porcentaje )
    //{

    //      if (porcentaje == 0)
    //    {
    //        return "barraceleste.png";
    //    }
    //      else  if (porcentaje > 0 && porcentaje < 75)
    //    {
    //        return
      
    //          "barraverde.png";
    //    }
    //    else if
    //        (porcentaje >= 100)
    //    {
    //        return
    //            "barraroja.png";
    //    }
     
    //    else
    //    {
    //        return
    //            "barraamarilla.png";
    //    }
    //}



    /// <summary>
    /// despliega el warning pegado a lso estados
    /// </summary>
    /// <param name="porcentaje"></param>
    /// <returns></returns>
    //public string DisplayWarning(int porcentaje)
    //{

    //    if (porcentaje == 102 || porcentaje == 76 || porcentaje == 20)
    //    {
    //        return "'warning.gif'";

    //    }

    //    else 
    //    {
    //        return "'warningBlanco.png'";
    //    }

    //}



    //public string Redirect(string orderID, string  Status)
    //{




    //    return "Default4.aspx?OT=" + orderID.Split('#')[1] + "&STAT=" + Status;

      
    //}


    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        RebindRadRotator(new DataTable());// New data source
    }
 
    private void RebindRadRotator(DataTable dataSource)
    {
        //RadRotator1.DataSource = dataSource;
       // RadRotator1.DataBind();
     
    }
}

