<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SCGUserDescMasterPage.master" AutoEventWireup="true" Inherits="Principal_EstadoPostVenta" Codebehind="EstadoPostVenta.aspx.cs" %>

<%@ Register assembly="FlashControl" namespace="Bewise.Web.UI.WebControls" tagprefix="Bewise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAddonContent" Runat="Server">
    
    
     <div style="height:600px">
    <Bewise:FlashControl ID="FlashControl1" runat="server" 
        MovieUrl="~/SWF/DMS/Serv_PostVenta.swf" Height="100%" Width="100%" />
      </div>
</asp:Content>

