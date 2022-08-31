<%@ Page Language="C#" AutoEventWireup="true" Inherits="Principal_DetalleOrdenTrabajo" Codebehind="DetalleOrdenTrabajo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/MiscelaneosLayOutStyles/TabsStyle.css" 
        rel="stylesheet" type="text/css" />
    <link href="../App_Themes/SCGRoundedDeepBlueTheme/SCGRoundedDeepBlueStyle.css" 
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
  
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    
    <div id="ContentPanel">
    <div class="Container">
<div class="tab">
<div class="izq"></div>
<div class="centro">
<h2>
<asp:Label runat="server" ID="TituloPagina" AssociatedControlID="txtNumOT" Text="Consulta Post-Venta"></asp:Label></h2>
   </div>
   <div class="der"></div>
    </div>
        <div class="FilterPanel" >
 <asp:UpdatePanel ID="updGMapPostVenta"  runat="server">
<ContentTemplate>

<div class="Medidas">
<asp:Label ID="lblNumeroOT" runat="server" Text="No.Orden:" AssociatedControlID="txtNumOT" ></asp:Label>
       <asp:TextBox ID="txtNumOT" ReadOnly="true" runat="server"></asp:TextBox>
  <asp:Label ID="lblFecha" runat="server" Text="Fecha:" AssociatedControlID="txtNumOT" ></asp:Label>
  
<asp:TextBox ID="txtFecha" Width="160px" Text="1/12/2010" ReadOnly="true" runat="server" ></asp:TextBox>

  </div>   
  
  </ContentTemplate>
  </asp:UpdatePanel>  
<%--<div class="Medidas">

<asp:Label ID="lblplaca" runat="server" Text="No.Placa:" AssociatedControlID="txtNumPlaca" ></asp:Label>
<asp:TextBox ID="txtNumPlaca" runat="server" Width="160px" ReadOnly="True">226895</asp:TextBox>
<asp:Label ID="lblOrden" runat="server" Text="Estilo:" AssociatedControlID="txtEstilo" ></asp:Label>
<asp:TextBox ID="txtEstilo" Width="160px" Text="4x4" ReadOnly="true" runat="server" ></asp:TextBox>
</div>
<div class="Medidas">
<asp:Label ID="lblMrca" runat="server" Text="Marca:" AssociatedControlID="txtMarca" ></asp:Label>
    
   <asp:TextBox ID="txtMarca" Width="160px" Text="Toyota" ReadOnly="true" runat="server" ></asp:TextBox>
 
<asp:Label ID="lblModelo" runat="server"  Text="Modelo:" AssociatedControlID="txtModelo" ></asp:Label>
  <asp:TextBox ID="txtModelo" Width="160px" Text="Land Cruiser" ReadOnly="true" runat="server" ></asp:TextBox>
</div>
<div class="Medidas">
<asp:Label ID="lblObserbvaciones" runat="server" Text="Observaciones:" AssociatedControlID="txtObservaciones" ></asp:Label>
  
   <asp:TextBox ID="txtObservaciones" Width="560px" Text="" Height="50px" ReadOnly="true" runat="server" ></asp:TextBox>
    
</div>
--%>
</div>

 </div>
</div>

    </form>
</body>
</html>
