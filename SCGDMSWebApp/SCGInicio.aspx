<%@ Page Language="C#" AutoEventWireup="true" Inherits="SCGInicio" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" Codebehind="SCGInicio.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/SCGInicioTheme/SCGInicio.css" rel="stylesheet" 
        type="text/css" />
        <link rel="alternate" type="application/rss+xml" title="Tecnova Soluciones RSS" href="RSS/rss-prueba.xml"/>
       <link rel="shortcut icon" href="App_Themes/SCGBackGroundFillTheme/icno 3.ico"/>
</head>
<body>
<div id= "Wrapper">   
 <form id="form1" runat="server">
   <div id= "Banner">
       <asp:Image ID="imgLogo"  ImageUrl="~/App_Themes/SCGRoundedDeepBlueTheme/Carrodmsone.png"
           runat="server" meta:resourcekey="imgLogoResource1" Height="115px" 
           Width="250px" />
   </div>

   

    <div id = "Content">
    
    <div id= "ADSpanel">
        <asp:Literal ID="ltrAdsCustomer" runat="server" 
            meta:resourcekey="ltrAdsCustomerResource1"></asp:Literal>
    </div>
   <div id= "LoginPanel">
                <div id= "LoginFrame">
               
                 <div class= "ContenidodeBorde">
                <div id= "BordeSupIzq">
                <div id= "BordeSupDer">
                
                </div>
                </div>
                </div>
            <div style= "margin-left:auto; margin-right:auto; width:250px">
                <div id= "Login" > 
                  <asp:Login ID="loginSCG" runat="server"  Width="250px"  onloggingin="logSCG_LoggingIn" 
                      DestinationPageUrl="~/Principal/Default.aspx" 
                      meta:resourcekey="logSCGResource1" onloggedin="loginSCG_LoggedIn">
                        <LayoutTemplate>
                            
                                
                                
                                                 <span class="DisplayBlock"  style="text-align:center;"> 
                                                 <asp:Label ID="lblIngreso" runat="server" 
                                                     meta:resourcekey="lblIngresoResource1" Text="Ingreso al Sistema"></asp:Label>
                                                 </span>
                                               <div style="width:100%;">
                                                  <div class= "alinearIzquierda" >  <asp:Label ID="UserNameLabel" runat="server" 
                                                          AssociatedControlID="UserName" meta:resourcekey="UserNameLabelResource1">User Name:</asp:Label>
                                                  </div>
                                               
                                                   <div class="alinearDerecha"> <asp:TextBox ID="UserName" runat="server" 
                                                           meta:resourcekey="UserNameResource1"></asp:TextBox>
                                                                                 <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                                                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                                                 ToolTip="User Name is required." 
                                                           ValidationGroup="Login1" meta:resourcekey="UserNameRequiredResource1">*</asp:RequiredFieldValidator>
                                                         </div>
                                           
                                                  <div class= "alinearIzquierda" >   <asp:Label ID="PasswordLabel" runat="server" 
                                                          AssociatedControlID="Password" meta:resourcekey="PasswordLabelResource1">Password:</asp:Label> 
                                                  </div>
                                            
                                                   <div class="alinearDerecha">   <asp:TextBox ID="Password" runat="server" 
                                                           TextMode="Password" meta:resourcekey="PasswordResource1"></asp:TextBox>
                                                                                  <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                                                                    ToolTip="Password is required." 
                                                           ValidationGroup="Login1" meta:resourcekey="PasswordRequiredResource1">*</asp:RequiredFieldValidator> 
                                                  </div>
                                        </div>
                                                   <div class="DisplayBlock" > <asp:CheckBox ID="RememberMe" runat="server" 
                                                           Text=" Remember me next time." meta:resourcekey="RememberMeResource1" /></div>
                                           
                                                  <div class="DisplayBlock">  <asp:Literal ID="FailureText" runat="server" 
                                                          EnableViewState="False" meta:resourcekey="FailureTextResource1"></asp:Literal></div>
                                           
                                              <div style="width:100%;">
                                                 <div class="alinearDerecha">  <asp:Button ID="LoginButton" runat="server" 
                                                         CommandName="Login" Text="Log In" 
                                                        ValidationGroup="Login1" meta:resourcekey="LoginButtonResource1" /></div>
                                                 
                                                  
                                             </div>
                                           
                                             
                                       
                       
                                        
                       </LayoutTemplate>
                    </asp:Login>
                                          
                                                  
                                             <div style="width:80%; margin-top:20px; clear:both">
                                             <div class="alinearIzquierda"> 
                                                 <asp:Label ID="lblIdioma" runat="server" Text="Idioma:" 
                                                     meta:resourcekey="lblIdiomaResource1"></asp:Label></div>
                                                 <div class="alinearDerecha">
                                                     <asp:DropDownList ID="ddlIdioma"  Width="100px" runat="server" 
                                                         meta:resourcekey="ddlIdiomaResource1" AutoPostBack="True">
                                                     <asp:ListItem meta:resourcekey="ListItemResource1" Value=auto>Español</asp:ListItem>
                                                     <asp:ListItem meta:resourcekey="ListItemResource2" Value=en-US>Inglés</asp:ListItem>
                                                     </asp:DropDownList> </div>
                                                     </div>
                                     <br  style= "clear:both;"/>  
                       </div>
                  </div>

                <div class= "ContenidodeBorde">
                <div id= "BordeInfIzq">
                <div id= "BordeInfDer">
                
                </div>
                </div>
                  </div>
                    <asp:Image ID="imgLogoAddon" AlternateText="Logo Addon" 
                        ImageUrl="~/App_Themes/SCGInicioTheme/LogoDMSLogingrande.png" 
                        runat="server" Width="349px" 
                        meta:resourcekey="imgLogoAddonResource1"  /> 
               </div>
     
      </div>
    </div>
    
    <div id= "Footer">
    
        <asp:Image ID="imglogoSCG" AlternateText="Software & Consulting Group"   
            runat="server"  ImageUrl="~/App_Themes/SCGInicioTheme/logoscg.png" Height="27px" 
            Width="60
            px" meta:resourcekey="imglogoSCGResource1"/>
     
       
         &nbsp;&nbsp;
         Derechos Reservados 2003-2010 Software & Consulting Group   </div>
    </form>
    
    </div>

</body>
</html>
