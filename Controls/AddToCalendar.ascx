<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddToCalendar.ascx.cs" 
Inherits="AddToCalendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true"
runat="server">
    <Windows>
        <telerik:RadWindow Width="350"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="./image/CalendarIcon.png" 
        EnableEmbeddedSkins="true" Skin="Web20" height="200px" VisibleTitlebar="false" 
        VisibleStatusbar="false" 
        ID="MessageRadWindow" Title="Add To Calendar" runat="server">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
<script type="text/javascript">
    function Added(oWnd,args)
    {
        var startDiv = document.getElementById('StartDiv');
        startDiv.style.display = 'none';
     
        var endDiv = document.getElementById('EndDiv');
        endDiv.style.display = 'block';   
    }
    function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
    // Called when a window is being closed.
                function OnClientClose(radWindow)
                {                  
//                   var oValue = radWindow.argument;
//                   var oArea = document.getElementById("InfoArea");
//                   oArea.value = oValue;
                   window.location.reload(true);
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
                    {
                        if(returnValue == 'anything')
                            window.location.reload(true);
                        else
                            window.location = returnValue;
                    }
                    //Another option for passing a callback value
                    //Set the radWindow.argument property in the dialog
                    //And read it here --> var oValue = radWindow.argument;                                        
                    //Do cleanup if necessary
                } 
                function OpenRad_Add()
                {
                    var win = $find("<%=MessageRadWindow.ClientID %>");
                    win.setUrl('Controls/AddEventAlert.aspx');
                    win.show(); 
                    win.center(); 
                       
                 }
</script>
<div style="float: left;padding-right: 10px;">
    <div id="StartDiv">
        <asp:Panel runat="server" ID="LoggedInPanel">
        
        <asp:Label runat="server" id="AddToLabel" CssClass="Green12LinkNFUD" Visible="false"></asp:Label>
        <asp:Literal runat="server" ID="AddLiteral"></asp:Literal>
            </asp:Panel>
    </div> 
    <div id="EndDiv" style="display: none;" class="Green12LinkNF">You're Going!</div>
</div>
