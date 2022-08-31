<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta al iniciarse la aplicación

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Código que se ejecuta cuando se cierra la aplicación

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Código que se ejecuta al producirse un error no controlado

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando se inicia una nueva sesión

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando finaliza una sesión. 
        // Nota: El evento Session_End se desencadena sólo con el modo sessionstate
        // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer 
        // o SQLServer, el evento no se genera.

    }

    /// <summary>
    /// maneja la validacion de los usuarios a la hora de
    /// accesar a una pagina
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_BeginRequest(object sender, EventArgs e)
    {

      //  string path = Request.Path.ToLower();

       
        
      //path=  path.Replace(Request.ApplicationPath.ToLower(), "");
        
      //  //validar que compruebe contra bd solamente las paginas aspx y nunca la pagina de login 
      //if (path.Contains(".aspx") && !FormsAuthentication.LoginUrl.ToLower().Contains(path) && !path.Contains("scgerrorpage"))
      //{
         
      //        //validar si tiene querystring

      //        if (Request.QueryString.HasKeys())
      //        {
      //            char[] spliter = { '?' };
      //            path = path.Split(spliter)[0];
      //        }

      //        if (HttpContext.Current.Session != null)
      //        { }
      //        else
      //        { }
         
      //}
    }
</script>
