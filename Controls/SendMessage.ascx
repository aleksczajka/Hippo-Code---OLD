<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendMessage.ascx.cs" 
Inherits="SendMessage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="560" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
        Skin="Web20" Height="536" VisibleStatusbar="false" ID="MessageRadWindow" 
        VisibleTitlebar="false" Title="Send Message" ClientCallBackFunction="clientSendCallback" 
        runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<div style="float: left;;">
    <script type="text/javascript">
    function clientSendCallback(somtin, thisOne)
         {
            if(thisOne != null && thisOne != undefined)
            {
                window.location = thisOne;
            }
         }
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
    <asp:Literal runat="server" ID="TheLiteral"></asp:Literal>
</div>
