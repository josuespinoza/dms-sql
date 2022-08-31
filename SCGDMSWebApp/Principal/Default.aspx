<%@ Page Language="C#" MasterPageFile="~/MasterPages/SCGUserDescMasterPage.master" AutoEventWireup="true" Inherits="Principal_Default" Title="SCG Web App" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" Codebehind="Default.aspx.cs" %>

<%@ Register Assembly="FlashControl" Namespace="Bewise.Web.UI.WebControls" TagPrefix="Bewise" %>



<asp:Content ID="Content2" ContentPlaceHolderID="cphAddonContent" Runat="Server">

    <div style="height:600px">
 
      <Bewise:FlashControl ID="FlashControl1" runat="server" Height="100%" 
            Width="100%" />
 
      </div>  
        
    
</asp:Content>

