<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_SCGAccordionMenu" Codebehind="SCGAccordionMenu.ascx.cs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="act" %>

<asp:Panel ID="panelMenu" runat="server">
</asp:Panel>

<act:Accordion ID="SCGAccordionMenu" runat="server"  FadeTransitions="false" FramesPerSecond="40" 
                        TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true"
                        HeaderCssClass="HeaderAccordionPane"  HeaderSelectedCssClass="HeaderAccordionPane" ContentCssClass="AccordionContentPane">

</act:Accordion>
