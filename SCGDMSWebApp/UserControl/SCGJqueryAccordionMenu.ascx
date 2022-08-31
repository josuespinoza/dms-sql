<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_SCGJqueryAccordionMenu" Codebehind="SCGJqueryAccordionMenu.ascx.cs" %>




<script src="../Javascript/jquery-1.4.1.js" type="text/javascript"></script>


<script type="text/javascript"> 
<!--//---------------------------------+
    //Creado por thuertas
    //Funcion jquery Que maneja el funcionamiento del menu acordeon
// --------------------------------->
$(document).ready(function()
{
	
	$("#firstpane p.menu_head").click(function()
    {
		$(this).next("div.menu_body").slideToggle(300).siblings("div.menu_body").slideUp("slow");
       //	$(this).siblings().css({backgroundImage:"url(left.png)"});
    });

    $("#firstpane p.menu_head2").click(function() {
        $(this).next("div.menu_body").slideToggle(300).siblings("div.menu_body").slideUp("slow");
        $(this).siblings();
    });

});
</script>



<asp:Literal ID="ltrAcordion" runat="server"></asp:Literal>

