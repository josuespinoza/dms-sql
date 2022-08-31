<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SCGUserDescMasterPage.master" AutoEventWireup="true" Inherits="Principal_PruebaCookie" Codebehind="PruebaCookie.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAddonContent" Runat="Server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../Javascript/jquery.cookie.js"></script>
<script type="text/javascript" language="javascript"> 

$(document).ready(function() {
// LEFT COLUMN:
	// When the collapse button is clicked:
	$('.collapseLeft').click(function() {
		$('.collapseLeft').css("display","none");
		$('.expandLeft').css("display","block");
		$('#leftCol').css("height","20px");
		$.cookie('leftCol', 'collapsed');
	});
	// When the expand button is clicked:
	$('.expandLeft').click(function() {
		$('.expandLeft').css("display","none");
		$('.collapseLeft').css("display","block");
		$('#leftCol').css("height","500px");
		$.cookie('leftCol', 'expanded');
	});
// RIGHT COLUMN:
	// When the collapse button is clicked:
	$('.collapseRight').click(function() {
		$('.collapseRight').css("display","none");
		$('.expandRight').css("display","block");
		$('#rightCol').css("height","20px");
		$.cookie('rightCol', 'collapsed');
	});
	// When the expand button is clicked:
	$('.expandRight').click(function() {
		$('.expandRight').css("display","none");
		$('.collapseRight').css("display","block");
		$('#rightCol').css("height","500px");
		$.cookie('rightCol', 'expanded');
	});
// COOKIES
	// Left column state
	var leftCol = $.cookie('leftCol');
	// Right column state
	var rightCol = $.cookie('rightCol');
	// Set the user's selection for the left column
	if (leftCol == 'collapsed') {
		$('.collapseLeft').css("display","none");
		$('.expandLeft').css("display","block");
		$('#leftCol').css("height","20px");
	};
	// Set the user's selection for the right column
	if (rightCol == 'collapsed') {
		$('.collapseRight').css("display","none");
		$('.expandRight').css("display","block");
		$('#rightCol').css("height","20px");
	};
});
</script> 
<style> 
body {
	background-color:#444444;
	position:relative;
}
#page {
	width:500px;
	margin:0 auto;
	text-align:center;
	background-color:#222222;
	padding:1px;
}
#leftCol {
	width:149px;
	margin-right:1px;
	height:500px;
	float:left;
	background-color:#666666;
	position:relative;
}
#rightCol {
	width:350px;
	height:500px;
	float:left;
	background-color:#333333;
	position:relative;
}
.expandLeft, .expandRight {
	width:11px; 
	height:11px;
	background:url(expand.gif) no-repeat;
	position:absolute;
	right:5px;
	top:4px;
	display:none;
}
.collapseLeft, .collapseRight {
	width:11px; 
	height:11px;
	background-color:Red;
	position:absolute;
	right:5px;
	top:4px;
}
</style> 

  <div id="page"> 
        <div id="leftCol"> 
        <div class="collapseLeft"></div> 
        <div class="expandLeft"></div> 
        </div> 
        <div id="rightCol"> 
        <div class="collapseRight"></div> 
        <div class="expandRight"></div> 
        </div> 
        <br clear="all" /> 
    </div>
</asp:Content>

