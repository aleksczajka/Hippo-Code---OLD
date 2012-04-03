<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactAd.ascx.cs" Inherits="ContactAd" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" Modal="true" DestroyOnClose="false" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="400" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Web20" Height="450" VisibleStatusbar="false" ClientCallBackFunction="clientAdCallback" ID="MessageRadWindow" VisibleTitlebar="false" Title="Send Message" 
        runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<div style="float: left; clear: both;">
    <script type="text/javascript">
    function clientAdCallback(somtin, thisOne)
         {
            if(thisOne != null && thisOne != undefined)
            {
                window.location = thisOne;
            }
         }
            function OpenRadBefore_ContactAd()
            {
                var idDiv = document.getElementById('idDiv');
                var typeDiv = document.getElementById('typeDiv');
                var win = $find("<%=MessageRadWindow.ClientID %>");
                var str = 'Controls/MessageAlert.aspx?T=Message&ID='+ idDiv.innerHTML + '&A=' + typeDiv.innerHTML;
                win.setUrl(str);
                win.show(); 
                win.center(); 
                
            }
            function OpenRad_SendMessage(response)
            {
                
                   
            }
    </script>
    <div style="float: left;padding-right: 10px;">
        <asp:Literal runat="server" ID="TheLiteral"></asp:Literal>
    </div>
</div>
