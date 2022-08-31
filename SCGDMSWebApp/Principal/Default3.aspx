<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SCGUserDescMasterPage.master" AutoEventWireup="true" Inherits="Principal_Default3" Codebehind="Default3.aspx.cs" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<%@ Register src="../UserControl/SCGWOStatusDisplay.ascx" tagname="SCGWOStatusDisplay" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAddonContent" Runat="Server">
     <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../Javascript/jquery.cookie.js"></script>
    <script language="javascript" >
        $(document).ready(function () {



            // COOKIES
            // Left column state
            var leftCol = $.cookie('estado');
            var valor = 0;
            // Set the user's selection for the left column
            if (leftCol == 'show') {
                //obtiene el width en`pixeles del content

                $("#Collapsible").addClass("imgAbajo");




            }
            else {
            //tamaño definido del leftpanel
                valor = 200;


                var contentWidth = $("#Content").innerWidth() + valor;
                $("#LeftPanel").css('display', 'none');
                $("#LeftPanel").css('width', 0);
                $("#Content").css('width', contentWidth);
                $("#Content").css('margin-Left', '0px');
                $("#Collapsible").addClass("imgLado");

            };

            $("#Collapsible").toggle(


                function () {
                    //validar si ya se escondio el leftpanel
                    var conWidth = $("#Content").width() + valor;
                    var innWidth = $("#Wrapper").innerWidth();
                    if (conWidth < innWidth) {

                        valor = 0;
                    }
                   
                    //obtiene el width en pixeles del content
                    var contentWidth = $("#Content").width() - valor;
                    $("#LeftPanel").animate({ width: 'show' });
                    $("#LeftPanel").css('width', 200);
                    $("#Content").animate({ marginLeft: "200px" }, 400);
                    $("#Content").animate({ width: contentWidth }, 100);
                    $("#Collapsible").removeClass("imgLado").addClass("imgAbajo");

                    $.cookie('estado', 'show');
                    //                 


                },

                 function () {

                     var conWidth = $("#Content").width()+ valor;
                     var innWidth = $("#Wrapper").innerWidth();
                     if (conWidth < innWidth ) {

                         valor = 200;
                     }
                    
                     //obtiene el width enpixeles del content
                     var contentWidth = $("#Content").width() + valor;
                     $("#Content").animate({ marginLeft: "0px" }, 400);
                     $("#Content").animate({ width: contentWidth }, 100);
                     $("#LeftPanel").animate({ width: 'hide' });
                     $("#LeftPanel").css('width', 0);
                     $("#Collapsible").removeClass("imgAbajo").addClass("imgLado");
                     $.cookie('estado', 'hide');


                 }
             );

            $("#homebanner ul").fadeIn(5000);
        });          
        </script>
    <div>
   
    
      <telerik:radscriptmanager ID="RadScriptManager1" runat="server">
          
 </telerik:radscriptmanager>
 
        
        
           <div style= "float:right; margin-right:30px;  margin-left:auto; ">
            


  <asp:XmlDataSource ID="XmlDataSource1" XPath="rss/channel/item" runat="server" DataFile="http://blogs.telerik.com/Blogs.rss">
    </asp:XmlDataSource>

     </div>
      <div style= "float:left; margin-left:30px; height:600px; overflow:scroll">
      

          <uc1:SCGWOStatusDisplay ID="SCGWOStatusDisplay1" runat="server" />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="Data Source=THuertaS-PC;Initial Catalog=OrdenTrabajo;User ID=sa;Password=B1admin" 
        
        
              SelectCommand="SELECT [OrderID], [PorcentajeAvance], [estado], [fase], [tecnico], [ID],[Workspace] FROM [OT]">
    </asp:SqlDataSource>  
    
            <br />
          <br />
            </div>
 
      <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
              ConnectionString="<%$ ConnectionStrings:OrdenTrabajoConnectionString %>" 
              SelectCommand="SELECT [ID], [OrderID], [PorcentajeAvance], [estado], [tecnico], [fase], [Workspace] FROM [OT] ORDER BY [estado] DESC">
          </asp:SqlDataSource>
   
          
 
    </div>

</asp:Content>

