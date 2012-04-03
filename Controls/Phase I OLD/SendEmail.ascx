<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendEmail.ascx.cs" Inherits="Controls_SendEmail" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div style="height: 25px; float: left; clear: both;">
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="450" EnableEmbeddedSkins="true" 
        IconUrl="./image/EmailIcon.png" Skin="Black" Height="500" VisibleTitlebar="false"
        VisibleStatusbar="false" ID="MessageRadWindow" Title="Send Email" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    function OpenRadBefore_SendEmail()
    {
        var win = $find("<%=MessageRadWindow.ClientID %>");
            win.setUrl('Controls/MessageAlert.aspx?T=Email');
            win.show(); 
            win.center();
    }
    function OpenRad_SendEmail(response)
        {
             
               
         }
</script>

    <div style="padding-top: 3px; height: 19px;" id="StartDiv">
        <div style="float: left;"><img src="image/EmailIcon.png" /></div>
        <label  class="AddWhiteLink" onclick="OpenRadBefore_SendEmail();">Send Email with this Info</label>
    </div> 
<%--    <span style="color: White; font-weight: bold; font-size: 16px; font-family: Arial;">+</span>
    <asp:LinkButton CssClass="AddLink" runat="server" OnClick="OpenMessage" ID="txtLink"></asp:LinkButton>
--%>   <%-- <rad:RadToolTip CssClass="SendMessageModal" ID="ToolTip1" runat="server" BorderColor="black" BorderStyle="solid" BorderWidth="2px" 
    Skin="WebBlue" ManualClose="true" TargetControlID="txtLink" Modal="true" Title="Your Message"
     Animation="Resize" RelativeTo="BrowserWindow" Position="Center" Height="370px" Width="570px">
        <div align="center">
            <div align="left"><asp:Label CssClass="ReLabel" runat="server" ID="RegardingLabel"></asp:Label></div>
            <asp:TextBox runat="server" ID="MessageTextBox" TextMode="multiLine" Wrap="true" Width="528" Height="240"></asp:TextBox>
            <asp:ImageButton runat="server" ID="SendItButton" ImageUrl="~/image/SendItButton.png" onmouseout="this.src='image/SendItButton.png'" onmouseover="this.src='image/SendItButtonSelected.png'" />
        </div>
    </rad:RadToolTip> --%>
    <asp:SqlDataSource ID="ProviderDataSource" runat="server" SelectCommand="SELECT * FROM PhoneProviders" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
</div>
