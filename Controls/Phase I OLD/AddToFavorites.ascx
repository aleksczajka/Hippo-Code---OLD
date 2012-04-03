<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddToFavorites.ascx.cs" 
Inherits="AddToFavorites" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div>
<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
    KeepInScreenBounds="true" VisibleOnPageLoad="false" Behaviors="none"
ID="MessageRadWindowManager" BackColor="#1b1b1b" Modal="true" DestroyOnClose="true" 
RestrictionZoneID="RestrictionZone"
runat="server">
    <Windows>
        <telerik:RadWindow Width="300"  ReloadOnShow="true"
        ClientCallBackFunction="CallBackFunction"  IconUrl="../image/AddIcon.png" VisibleTitlebar="false"
        EnableEmbeddedSkins="true" Skin="Black" Height="200" VisibleStatusbar="false" 
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
<div style="color: White; font-family: Arial; font-weight: bold; height: 18px;">
    <asp:Panel runat="server" ID="ImagePanel"><div style="float: left; padding-bottom: 2px;">
    <img src="../image/AddIcon.png" /></div></asp:Panel>
    <asp:Label runat="server" id="AddToLabel" CssClass="AddLinkGoing" Visible="false"></asp:Label>
    <asp:LinkButton runat="server" ForeColor="White" CssClass="AddLink" OnClick="ShowMessage" 
    ID="AddToLink" Visible="false"></asp:LinkButton>
   </div>
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
