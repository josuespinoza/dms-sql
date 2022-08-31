<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_SCGBasicMenu" Codebehind="SCGBasicMenu.ascx.cs" %>
   <asp:Menu ID="MenuSCG"  runat="server" DataSourceID="SCGSiteMapSource" 
                    StaticEnableDefaultPopOutImage="false"
                    >
           <StaticMenuStyle CssClass="MenuItemStyle" />
    
        <StaticMenuItemStyle Height="30px" />
    
    
 
   
        <DynamicMenuItemStyle Height="30px" CssClass="MenuItemStyle"  />
      <DynamicHoverStyle CssClass="MenuItemHoverStyle" Height="30px" />
    </asp:Menu>
                <asp:SiteMapDataSource ID="SCGSiteMapSource" runat="server"  ShowStartingNode="false"/>