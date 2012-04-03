<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendMessage.ascx.cs" Inherits="Controls_SendMessage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="560" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Black" Height="536" VisibleStatusbar="false" ID="MessageRadWindow" 
        VisibleTitlebar="false" Title="Send Message" 
        runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<div style="height: 20px; float: left;clear:both;">
    <script type="text/javascript">
            function OpenRadBefore_SendMessage(reocurr)
            {
                var idDiv = document.getElementById('idDiv2');
                var typeDiv = document.getElementById('typeDiv2');
                var win = $find("<%=MessageRadWindow.ClientID %>");
                var str;
                if(reocurr == 'na')
                    str = 'MessageAlert.aspx?T=Message&ID='+ idDiv.innerHTML + '&A=' + typeDiv.innerHTML;
                else
                    str = 'MessageAlert.aspx?T=Message&ID='+ idDiv.innerHTML + '&A=' + typeDiv.innerHTML+'&O='+reocurr;
                win.setUrl(str);
                win.show(); 
                win.center(); 
                
            }
            function OpenRad_SendMessage(response)
            {
                
                   
            }
    </script>
    <div style="float: left; height: 20px;"><img src="./image/ContactIcon.png" /><asp:Literal runat="server" ID="TheLiteral"></asp:Literal></div>
    
   <%-- <rad:RadToolTip CssClass="SendMessageModal" ID="ToolTip1" runat="server" BorderColor="black" BorderStyle="solid" BorderWidth="2px" 
    Skin="WebBlue" ManualClose="true" TargetControlID="txtLink" Modal="true" Title="Your Message"
     Animation="Resize" RelativeTo="BrowserWindow" Position="Center" Height="370px" Width="570px">
        <div align="center">
            <div align="left"><asp:Label CssClass="ReLabel" runat="server" ID="RegardingLabel"></asp:Label></div>
            <asp:TextBox runat="server" ID="MessageTextBox" TextMode="multiLine" Wrap="true" Width="528" Height="240"></asp:TextBox>
            <asp:ImageButton runat="server" ID="SendItButton" ImageUrl="~/image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'" onmouseover="this.src='image/SendItButtonSelected.png'" />
        </div>
    </rad:RadToolTip> --%>
    <asp:SqlDataSource ID="ProviderDataSource" runat="server" SelectCommand="SELECT * FROM PhoneProviders" 
    ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
</div>
