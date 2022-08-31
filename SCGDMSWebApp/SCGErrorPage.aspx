<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SCGMasterPage.master" AutoEventWireup="true" Inherits="SCGErrorPage" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" Codebehind="SCGErrorPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphAddonBase" Runat="Server">
    <div id= "MessageContent" style="margin-left:auto; margin-right:auto;  margin-top:100px; width:700px; text-align:center">
    <asp:Label ID="lblError" runat="server" Text="Se ha producido un error. Contacte al Administrador" 
        meta:resourcekey="lblErrorResource1" Font-Size="Medium" ForeColor="#FF6600"></asp:Label>
</div>
</asp:Content>

