<%@ Control Language="C#" AutoEventWireup="true" Inherits="SCGDMSWebApp.UserControl.UserControl_SCGWOStatusDisplay" Codebehind="SCGWOStatusDisplay.ascx.cs" %>
 <style type="text/css">

  


 

#homebanner{

    list-style-image: none; 

    list-style-type: none;

}

 

#homebanner ul{

    display: none; 

}
  
  
 

      .itemTemplate
        {
          width: 302px;
          height: 90px;
          margin-top: 4px;
          margin-bottom: 4px;
          margin-left: 15px;
          cursor: pointer;
          font-family: Verdana;
          float: left;
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
          width: 100px;
          height: 56px;
          float: left;
      }
      .title
        {
          padding-left: 5px;
          width: 180px;
          margin-top: 15px;
          float: right;
          font-size: 12px;
      }
      .date
        {
          margin-top: 10px;
          color: White; 
          text-align: center;
          font-size: 1.2em;
      }
        .time
        {
            margin-top: 20px;
            text-align: center;
            font-size: 30px;
           
        }
  </style>
  
  
    <!--desarrollado por thuertas --05-01-2011-->
    <div id="homebanner">
      
     <asp:Repeater ID="Repeater2" runat="server">
     
   <HeaderTemplate>
    <ul id="1">
   </HeaderTemplate>
     <ItemTemplate>
      
        
  

        <div class="itemTemplate"   onclick="window.open('<%# this.Redirect(DataBinder.Eval(Container.DataItem, "orderID").ToString(),DataBinder.Eval(Container.DataItem, "estado").ToString() ) %>')"  style= ' background-image: url(<%# this.DevolverColor( Convert.ToInt32( DataBinder.Eval( Container.DataItem, "PorcentajeAvance"))) %>)'   >
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
                       
                           <div style=" padding-left:100px; " ><img width="25px" height="25px"  src=<%# this.DisplayWarning( Convert.ToInt32( DataBinder.Eval( Container.DataItem, "PorcentajeAvance"))) %>   /></div> 
                        </div>
                    </div>
        </ItemTemplate>
<FooterTemplate>
    </ul>
</FooterTemplate>                   
        </asp:Repeater>
       
     
        

</div>
 
 <asp:Timer ID="scgtimer" Enabled="true" runat="server"   ontick="scgtimer_Tick"
  >
 
 </asp:Timer>
 