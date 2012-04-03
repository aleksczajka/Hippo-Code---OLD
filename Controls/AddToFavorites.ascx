<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddToFavorites.ascx.cs" 
Inherits="AddToFavorites" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="300"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="../image/AddIcon.png" VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Web20" Height="200" VisibleStatusbar="false" 
        ID="MessageRadWindow" Title="Add to Favorites" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    function OnClientClose(radWindow)
                {                  
//                   var oValue = radWindow.argument;
//                   var oArea = document.getElementById("InfoArea");
//                   oArea.value = oValue;
                   window.location.reload();
                    //Another option for passing a callback value
                    //Set the radWindow.argument property in the dialog
                    //And read it here --> var oValue = radWindow.argument;                                        
                    //Do cleanup if necessary
                } 
                
                function CallBackFunction(radWindow, returnValue)
                {                  
//                   var oValue = radWindow.argument;
//                   var oArea = document.getElementById("InfoArea");
//                   oArea.value = oValue;
                    if(returnValue != null && returnValue != undefined)
                        window.location.reload();
                    //Another option for passing a callback value
                    //Set the radWindow.argument property in the dialog
                    //And read it here --> var oValue = radWindow.argument;                                        
                    //Do cleanup if necessary
                } 
</script>
<div style="float: left;padding-right: 10px;">
    <asp:Label runat="server" id="AddToLabel" CssClass="Green12LinkNF" Visible="false"></asp:Label>
    <asp:LinkButton runat="server" CssClass="NavyLink12" OnClick="ShowMessage" 
    ID="AddToLink" Visible="false"></asp:LinkButton>
</div>

