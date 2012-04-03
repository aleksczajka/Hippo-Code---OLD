<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
AutoEventWireup="true" CodeFile="SearchesAndPages.aspx.cs" Inherits="SearchesAndPages"
 Title="HipHapp Searches and Pages" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager" Modal="true" Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" VisibleStatusbar="false"  
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="506" Width="560" 
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow3"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" ClientCallBackFunction="DeleteAdCallback" 
                VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow4"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" ClientCallBackFunction="DeleteEventCallback" 
                VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow5"  VisibleStatusbar="false" BorderWidth="0" 
                VisibleTitlebar="false" ClientCallBackFunction="DeleteVenueCallback" 
                VisibleOnPageLoad="false" Height="200" Width="438" 
                Title="Set Communication Preferences" runat="server">
                </rad:RadWindow>
            </Windows>
            
        </rad:RadWindowManager>
        <script type="text/javascript">
            function DeleteAdCallback(radWindow, returnValue)
            {
                if(returnValue != undefined && returnValue != null){
                    __doPostBack('ctl00$ContentPlaceHolder1$ReloadAds','');
                }
            }
            function DeleteEventCallback(radWindow, returnValue)
            {
                if(returnValue != undefined && returnValue != null){
                    __doPostBack('ctl00$ContentPlaceHolder1$ReloadEvents','');
                }
            }
            function DeleteVenueCallback(radWindow, returnValue)
            {
                if(returnValue != undefined && returnValue != null){
                    __doPostBack('ctl00$ContentPlaceHolder1$ReloadVenues','');
                }
            }
             function OpenRadDeleteAd(theID)
                {
                    var win = $find("<%=RadWindow3.ClientID %>");
                    win.setUrl('DeleteAd.aspx?type=delete&ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
                 function OpenRadDeleteEvent(theID)
                {
                    var win = $find("<%=RadWindow4.ClientID %>");
                    win.setUrl('DisableEvent.aspx?type=delete&ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
                  function OpenRadDeleteVenue(theID)
                {
                    var win = $find("<%=RadWindow5.ClientID %>");
                    win.setUrl('DisableVenue.aspx?ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
                 function OpenRadEnableAd(theID)
                 {
                    var win = $find("<%=RadWindow3.ClientID %>");
                    win.setUrl('EnableAd.aspx?ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
                 
                 function OpenRadEnableEvent(theID)
                 {
                    var win = $find("<%=RadWindow4.ClientID %>");
                    win.setUrl('EnableEvent.aspx?ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
                 
                 function OpenRadEnableVenue(theID)
                 {
                    var win = $find("<%=RadWindow5.ClientID %>");
                    win.setUrl('EnableVenue.aspx?ID='+theID);
                    win.show(); 
                    win.center(); 
                 }
        </script>
    <div class="EventDiv" style="width: 900px;">
 
        <div class="topDiv" ><h1 class="searches" style="float: left">Your Saved Searches</h1>
        <div style="float: left; padding-left: 5px; ">
        <div id="ImageDiv" style="width: 50px;" runat="server">
        <asp:Image runat="server" ToolTip="?" CssClass="HelpImage" 
            AlternateText="What's this page for?" ID="QuestionMark6" 
            ImageUrl="~/image/helpIcon.png"></asp:Image></div>
            <rad:RadToolTip Skin="Black" ID="RadToolTip1" runat="server" ManualClose="true" TargetControlID="ImageDiv" Position="MiddleRight" RelativeTo="element" ShowEvent="OnClick">
                <div style="width: 350px; color: #145769; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                  <label style="color: #cccccc; float: left;">To add any search, please visit 
                        the corresponding search page, enter all the desired search criteria and click 'Save Search'.</label>
                </div>
            </rad:RadToolTip>
        </div></div>
        <div class="topDiv" ><h2 class="searches"  style="float: left;">Saved Ad Searches</h2>
        <div style="float: left; padding-left: 5px; width: 550px;">
        <div id="Div1" style="width: 50px;" runat="server">
        <asp:Image runat="server" ToolTip="?" CssClass="HelpImage" 
            AlternateText="What's this page for?" ID="Image1" 
            ImageUrl="~/image/helpIcon.png"></asp:Image></div>
            <rad:RadToolTip Skin="Black" ID="RadToolTip2" runat="server" ManualClose="true" TargetControlID="Div1" 
            Position="MiddleRight" RelativeTo="element" ShowEvent="OnClick">
                <div style="width: 350px; color: #145769; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                  <label style="color: #cccccc; float: left;">Your saved ad searches actively 
            search the site for new ads matching your criteria <span style="font-weight: bold;">Even if you are not logged in!</span> 
            An email of the new ads is sent to you periodically.</label>
                </div>
            </rad:RadToolTip>
        
            
        </div></div>
        
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" RenderMode="block" runat="server">
            <ContentTemplate>
            
            <div class="EventDiv" style="padding-left: 20px;">
        <asp:Panel runat="server" ID="AdSearchesPanel"></asp:Panel>
        </div>
        
        </ContentTemplate>
        </asp:UpdatePanel>

        <div class="topDiv" ><h1 class="searches"  style="float: left;">Your Pages</h1>
        <div style="float: left; padding-left: 5px; ">
        <div id="Div2" style="width: 50px;" runat="server">
        <asp:Image runat="server" ToolTip="?" CssClass="HelpImage" 
            AlternateText="What's this page for?" ID="Image2" 
            ImageUrl="~/image/helpIcon.png"></asp:Image></div>
            <rad:RadToolTip Skin="Black" ID="RadToolTip3" runat="server" ManualClose="true" TargetControlID="Div2" 
            Position="MiddleRight" RelativeTo="element" ShowEvent="OnClick">
                <div style="width: 350px; color: #145769; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                  <label style="color: #cccccc;">Lists all pages you are the owner of. You can edit any one of these by clicking on their link.</label>
                </div>
            </rad:RadToolTip>
        </div></div>
        <h2 class="searches" >Ads</h2>
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="conditional" runat="server">
            <ContentTemplate>
            <asp:LinkButton runat="server" ID="ReloadAds" OnClick="ReloadTheAds"></asp:LinkButton>
            <div class="EventDiv" style="padding-left: 20px;">
            
        <asp:Panel runat="server" ID="AdPanel"></asp:Panel>
        </div>
         </ContentTemplate>
        </asp:UpdatePanel>
        <h2 class="searches" >Events</h2>
        <asp:UpdatePanel runat="server" UpdateMode="conditional">
            <ContentTemplate>
            <asp:LinkButton runat="server" ID="ReloadEvents" OnClick="ReloadTheEvents"></asp:LinkButton>
<div class="EventDiv" style="padding-left: 20px;">
        <asp:Panel runat="server" ID="EventsPanel" Width="576px"></asp:Panel>
             </div>       
            </ContentTemplate>
        </asp:UpdatePanel>
        <h2 class="searches" >Venues</h2>
        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="conditional" runat="server">
            <ContentTemplate>
            <asp:LinkButton runat="server" ID="ReloadVenues" OnClick="ReloadTheVenes"></asp:LinkButton>
            <div class="EventDiv" style="padding-left: 20px;">
        <asp:Panel runat="server" ID="VenuesPanel" Width="576px"></asp:Panel>
        </div>
         </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

