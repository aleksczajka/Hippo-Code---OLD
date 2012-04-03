<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendEmail.ascx.cs" Inherits="SendEmail" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div style="float: left;">
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="false" RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="450" EnableEmbeddedSkins="true" 
        IconUrl="./image/EmailIcon.png" Skin="Web20" Height="500" VisibleTitlebar="false"
        VisibleStatusbar="false" ClientCallBackFunction="clientEmailCallback" ID="MessageRadWindow" Title="Send Email" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
function clientEmailCallback(somtin, thisOne)
         {
            if(thisOne != null && thisOne != undefined)
            {
                window.location = thisOne;
            }
         }
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


        <span  class="Green12LinkNFUD" onclick="OpenRadBefore_SendEmail();">Send Email</span>
</div>
