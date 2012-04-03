<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendTxtToCell.ascx.cs" Inherits="SendTxtToCell" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div style="float: left;">
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
         function clientCalback(somtin, thisOne)
         {
            if(thisOne != null && thisOne != undefined)
            {
                window.location = thisOne;
            }
         }
</script>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" ClientCallBackFunction="clientCalback" Modal="true" DestroyOnClose="false" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow IconUrl="./image/PhoneIcon.png" Width="420" EnableEmbeddedSkins="true" 
        Skin="Web20" Height="480" VisibleStatusbar="false" ID="MessageRadWindow2" 
        Title="Send Text" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>

<span  class="Green12LinkNFUD" onclick="OpenRadBefore_SendTxt();">Send Txt</span>


</div>
