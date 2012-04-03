<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddToCalendar.ascx.cs" Inherits="Controls_AddToCalendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true"
runat="server">
    <Windows>
        <telerik:RadWindow Width="350"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="./image/CalendarIcon.png" 
        EnableEmbeddedSkins="true" Skin="Black" height="200px" VisibleTitlebar="false" 
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
                        window.location.reload();
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
<div style="color: White; font-family: Arial; font-weight: bold; height: 30px;">
    <div style="padding-top: 3px;" id="StartDiv">
        <asp:Panel runat="server" ID="LoggedInPanel">
        <div style=" float: left;"><asp:Panel runat="server" ID="ImagePanel"><img src="image/CalendarIcon.png" /></asp:Panel></div>
        <asp:Label runat="server" id="AddToLabel" CssClass="AddLinkGoing" Visible="false"></asp:Label>
        <asp:Literal runat="server" ID="AddLiteral"></asp:Literal>
            </asp:Panel>
    </div> 
    <div id="EndDiv" style="display: none;" class="AddGreenLink">This event has been added to your calendar</div>

<%--    <rad:RadToolTip ID="ToolTip1" runat="server" BorderColor="black" BorderStyle="solid" BorderWidth="2px" Skin="WebBlue" 
    ManualClose="true"
     Animation="Slide" RelativeTo="element" AutoCloseDelay="10000" Position="middleright" Height="50px" Width="200px">
        <div align="center">
            <asp:Panel runat="server" ID="Panel1">
                Add this event to your calendar <br />
                Choose your excitation level: 
                <asp:DropDownList runat="server" ID="EventExcitmentDropDown" DataSourceID="LevelDataSource" DataTextField="ExcitmentLevel" DataValueField="ID"></asp:DropDownList>
                <asp:SqlDataSource ID="LevelDataSource" runat="server" SelectCommand="SELECT * FROM Event_ExcitmentLevel" ConnectionString="<%$ ConnectionStrings:Connection%>"></asp:SqlDataSource>
                <br />
                <asp:ImageButton runat="server" ID="SendButton" ImageUrl="~/image/MakeItSoButton.png" OnClick="AddTo" onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                <asp:Label runat="server" ID="ErrorLabel"></asp:Label>
            </asp:Panel>
            
        </div>
    </rad:RadToolTip> --%>
</div>
