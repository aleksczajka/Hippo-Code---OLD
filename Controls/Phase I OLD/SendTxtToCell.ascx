<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendTxtToCell.ascx.cs" Inherits="Controls_SendTxtToCell" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div style="float: left; clear: both; width: 200px;">
<script type="text/javascript">
//    function OpenRadBefore_SendTxt()
//    {
//        Controls_SendTxtToCell.encryption(OpenRad_SendTxt);
//    }
    function OpenRadBefore_SendTxt()
        {
            var win = $find("<%=MessageRadWindow2.ClientID %>");
            win.setUrl("Controls/MessageAlert.aspx?T=Txt");
            win.show(); 
            win.center(); 
               
         }
</script>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow IconUrl="./image/PhoneIcon.png" Width="420" EnableEmbeddedSkins="true" 
        Skin="Black" Height="480" VisibleStatusbar="false" ID="MessageRadWindow2" 
        Title="Send Text" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<div style="float: left; clear: both; width: 200px;">
    <div style="padding-top: 3px; float: left;"><img src="image/PhoneIcon.png" /></div>
    <div style="padding-top: 3px;"><label  class="AddWhiteLink" onclick="OpenRadBefore_SendTxt();">Send Txt To Cell</label>
    </div>
</div>
<%--<%--    <rad:RadToolTip ID="ToolTip1" runat="server" BorderColor="black" BorderStyle="solid" BorderWidth="2px" Skin="WebBlue" ManualClose="true" TargetControlID="txtLink"
     Animation="Slide" RelativeTo="element" AutoCloseDelay="10000" Position="middleright" Height="50px" Width="200px">
        <div align="center">
            <%--style=" background-color: #363636; border: solid 1px #1b1b1b;"
            Choose your number and provider:
            <asp:TextBox runat="server" ID="PhoneTextBox"></asp:TextBox>
            <asp:DropDownList runat="server" ID="ProvidersDropDown" DataSourceID="ProviderDataSource" DataTextField="Provider" DataValueField="Extension">
            </asp:DropDownList>
            <asp:ImageButton runat="server" ID="SendButton" ImageUrl="~/image/SendItButton.png" OnClick="SendMessage"  onmouseout="this.src='image/SendItButton.png'" onmouseover="this.src='image/SendItButtonSelected.png'"  />
            <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
        </div>
    </rad:RadToolTip>
    <asp:SqlDataSource ID="ProviderDataSource" runat="server" SelectCommand="SELECT * FROM PhoneProviders" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
--%></div>
