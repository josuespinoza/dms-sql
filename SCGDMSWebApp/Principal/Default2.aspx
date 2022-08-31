<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SCGMasterPage.master" AutoEventWireup="false" Inherits="Principal_Default2" Codebehind="Default2.aspx.vb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #header
        {
            height: 100px;
            background-color: #003366;
        }
        #LeftPanel
        {
            width: 200px;
            height: 600px;
            background-color: #006699;
        }
        #Content
        {
            background-color: #FFFF99;
            width: 1000%;
            height: 600px;
        }
        
           .itemTemplate
        {
            width: 493px;
            height: 90px;
            margin-top: 4px;
            margin-bottom: 4px;
            cursor: pointer;
            font-family:Verdana;
        }
        .backElement
        {
            background-image: url('IMAGES/main_back.jpg');
            width: 569px;
            height: 242px;
            margin-left: auto;
            margin-right: auto;
        }
        .titleText
        {
            width: 314px;
            float: right;
            margin-right: 30px;
            padding-top: 10px;
        }
        .rotator
        {
            margin-left: 40px;
            margin-top: 15px;
            width: 493px;
            height: 200px;
        }
        .dateTime
        {
            width: 150px;
            height: 56px;
            float: left;
        
        }
        .title
        {
            width: 280px;
            margin-top: 14px;
            float: right;
            font-size: 12px;
        }
        .date
        {
            margin-top: 10px;
            color: White;
            /*float: right;*/
          text-align:center;
        }
        .time
        {
            margin-top: 20px;
            text-align: center;
            font-size: 30px;
           
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphAddonBase" Runat="Server">

    <div>
      <telerik:radscriptmanager ID="RadScriptManager1" runat="server">
    </telerik:radscriptmanager>
   <div id="header"></div>
    <telerik:radsplitter ID="RadSplitter1" Width="100%" runat="server"  Height="600" 
            Skin="Web20">
        <telerik:RadPane ID="LeftPane" Height="630px" Width="200px" runat="server">
        <%--<div id="LeftPanel"></div>--%>
        </telerik:RadPane>

        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Forward" />

        <telerik:RadPane ID="MiddlePane1" runat="server" Height="600" Width="1200px">
           <div style= "float:right; margin-right:30px;  margin-left:auto; ">
                <telerik:RadRotator ID="RadRotator1" ScrollDirection="Up"
                ScrollDuration="4000" runat="server"  Width="493px"
                ItemWidth="493px" Height="600px" ItemHeight="100px" 
        FrameDuration="1" InitialItemIndex="-1" DataSourceID="SqlDataSource1" Skin=""
               >
                <ItemTemplate>
              
                 
                <div class="itemTemplate"  
                        style= ' background-image: url(<%# Me.DevolverColor(DataBinder.Eval(Container.DataItem, "PorcentajeAvance")) %>)'>
                        <div class="dateTime"> 
                            <div class="time"  >
                          <%# DataBinder.Eval(Container.DataItem, "PorcentajeAvance").ToString() + "%"%> 
                      
                              

                            </div>
                          <div class="date"  >
                            <%# DataBinder.Eval(Container.DataItem, "estado")%> 
                          </div>
                          
                            
                        </div>
                        <div class="title">
                            <span  style=" font-size:x-large">
                               <%# DataBinder.Eval(Container.DataItem, "orderID") %> 
                            </span><br />
                           <%-- <span>
                           stage:  <%#  DataBinder.Eval(Container.DataItem, "fase") %> --%>
                         <%--   </span><br />
                             <span>
                           Mechanic: <%# DataBinder.Eval(Container.DataItem, "Tecnico")%> 
                            </span> --%>
                           <%-- <span>Work Space:<%# DataBinder.Eval(Container.DataItem, "Workspace")%> </span>--%>
                           <span style=" text-align:right"> <asp:Image ID="Image1"  ImageUrl="~/TRIANGULO2.gif" runat="server" Width="25px" Height="25px"  /></span> 
                        </div>
                    </div>
                   
                </ItemTemplate>
            </telerik:RadRotator>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:OrdenTrabajoConnectionString %>" 
        
        SelectCommand="SELECT [OrderID], [PorcentajeAvance], [estado], [fase], [tecnico], [ID],[Workspace] FROM [OT]">
    </asp:SqlDataSource>

  <asp:XmlDataSource ID="XmlDataSource1" XPath="rss/channel/item" runat="server" DataFile="http://blogs.telerik.com/Blogs.rss">
    </asp:XmlDataSource>

     </div>
      <div style= "float:left; margin-left:30px">
      <telerik:RadRotator ID="RadRotator2" ScrollDirection="Up"
                ScrollDuration="4000" runat="server"  Width="493px"
                ItemWidth="493px" Height="600px" ItemHeight="100px" 
        FrameDuration="1" InitialItemIndex="-1" DataSourceID="SqlDataSource2" Skin=""
               >
                <ItemTemplate>
              
                 
                <div class="itemTemplate"  
                        style= ' background-image: url(<%# Me.DevolverColor(DataBinder.Eval(Container.DataItem, "PorcentajeAvance")) %>)'>
                        <div class="dateTime"> 
                            <div class="time"  >
                          <%# DataBinder.Eval(Container.DataItem, "PorcentajeAvance").ToString() + "%"%> 
                      
                    

                            </div>
                          <div class="date"  >
                            <%# DataBinder.Eval(Container.DataItem, "estado")%> 
                          </div>
                          
                            
                        </div>
                        <div class="title">
                            <span  style=" font-size:x-large">
                               <%# DataBinder.Eval(Container.DataItem, "orderID") %> 
                            </span><br />
                            <span>
                           stage:  <%#  DataBinder.Eval(Container.DataItem, "fase") %> 
                            </span><br />
                             <span>
                           Mechanic: <%# DataBinder.Eval(Container.DataItem, "Tecnico")%> 
                            </span> <br />
                            <span>Work Space:<%# DataBinder.Eval(Container.DataItem, "Workspace")%> </span>
                        </div>
                    </div>
                   
                </ItemTemplate>
            </telerik:RadRotator>
            </div>
 
      <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
              ConnectionString="<%$ ConnectionStrings:OrdenTrabajoConnectionString %>" 
              SelectCommand="SELECT [ID], [OrderID], [PorcentajeAvance], [estado], [tecnico], [fase], [Workspace] FROM [OT] ORDER BY [estado] DESC">
          </asp:SqlDataSource>
        </telerik:RadPane>
     
          
    </telerik:radsplitter>
    </div>


</asp:Content>

