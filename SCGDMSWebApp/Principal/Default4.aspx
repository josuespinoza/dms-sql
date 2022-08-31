<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SCGUserDescMasterPage.master" AutoEventWireup="true" Inherits="Principal_Default4" Codebehind="Default4.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAddonContent" Runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
 
    <div id="ContentPanel">
    <div class="Container">
<div class="tab">
<div class="izq"></div>
<div class="centro">
<h2>
<asp:Label runat="server" ID="TituloPagina" AssociatedControlID="txtNumOT" Text="Detalle Orden de Trabajo"></asp:Label></h2>
   </div>
   <div class="der"></div>
    </div>
        <div class="FilterPanel" >
 <asp:UpdatePanel ID="updGMapPostVenta"  runat="server">
<ContentTemplate>

<div class="Medidas">
<asp:Label ID="lblNumeroOT" runat="server" Text="No.Orden:" AssociatedControlID="txtNumOT" ></asp:Label>
       <asp:TextBox ID="txtNumOT" ReadOnly="true" runat="server"></asp:TextBox>
  <asp:Label ID="lblFecha" runat="server" Text="Fecha de Apertura:" AssociatedControlID="txtNumOT" ></asp:Label>
  
<asp:TextBox ID="txtFecha" Width="160px" Text="1/12/2010" ReadOnly="true" runat="server" ></asp:TextBox>

  </div>   
  
  </ContentTemplate>
  </asp:UpdatePanel>  
<div class="Medidas">

<asp:Label ID="lblplaca" runat="server" Text="No.Placa:" AssociatedControlID="txtNumPlaca" ></asp:Label>
<asp:TextBox ID="txtNumPlaca" runat="server" Width="160px" ReadOnly="True">226895</asp:TextBox>
<%----%>
<asp:Label ID="lblEstado" runat="server" Text="Estado de OT:" 
        AssociatedControlID="txtObservaciones" ></asp:Label>
  
   <asp:TextBox ID="txtStatus" Width="160px" Text=""  ReadOnly="true" 
        runat="server" ></asp:TextBox>
</div>
<div class="Medidas">
<asp:Label ID="lblMrca" runat="server" Text="Marca:" AssociatedControlID="txtMarca" ></asp:Label>
    
   <asp:TextBox ID="txtMarca" Width="160px" Text="Toyota" ReadOnly="true" runat="server" ></asp:TextBox>
 
<%----%>
  <asp:Label ID="lblOrden" runat="server" Text="Estilo:" AssociatedControlID="txtEstilo" ></asp:Label>
<asp:TextBox ID="txtEstilo" Width="160px" Text="4x4" ReadOnly="true" runat="server" ></asp:TextBox>
</div>

<div class="Medidas">
<asp:Label ID="lblModelo" runat="server"  Text="Modelo:" AssociatedControlID="txtModelo" ></asp:Label>
  <asp:TextBox ID="txtModelo" Width="160px" Text="Land Cruiser" ReadOnly="true" runat="server" ></asp:TextBox>
</div>
<div class="Medidas">
<asp:Label ID="lblObserbvaciones" runat="server" Text="Observaciones:" AssociatedControlID="txtObservaciones" ></asp:Label>
  
   <asp:TextBox ID="txtObservaciones" Width="560px" Text="" Height="50px" ReadOnly="true" runat="server" ></asp:TextBox>
    
</div>


</div>
 <br />
 <div  style="  width:825px">
 <table class="Table" width="825px">
 <tr  >
 
    <th>Actividades</th>
    <th>Status</th>
       <th>T. Estandar</th>
         <th>T. Real</th>
           <th>Mecanico</th>
 </tr>
 <tr>
    <td>Alineado</td>
    <td>En Proceso</td>
       <td>30 min</td>
         <td>38 min</td>
           <td>Rassiel Rebustillo</td>
 </tr>
 <tr style="background-color:#DEDCD3">
    <td>Tramado</td>
    <td>En Proceso</td>
       <td>30 min</td>
         <td>47 min</td>
           <td>Werner Flores</td>
 </tr>
 <tr>
    <td>Pintura</td>
    <td>Finalizado</td>
       <td>150 min</td>
         <td>180 min</td>
           <td>Roberto Campos</td>
 </tr>
 <tr style="background-color:#DEDCD3">
    <td>Cambio de Aceite</td>
    <td>Finalizado</td>
       <td>20 min</td>
         <td>20 min</td>
           <td>Hugo Araya</td>
 </tr>
 </table>
 
 </div>
 </div>
 

</div>


</asp:Content>

