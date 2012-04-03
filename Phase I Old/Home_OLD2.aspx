<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Home_OLD2.aspx.cs" Inherits="Home" smartnavigation="true"
Title="events venues ads community neighborhood friends hip happenings | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Src="~/Controls/AddToCalendar.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Poll.ascx" TagName="Poll" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Footer.ascx" TagName="Footer" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HomeEvent.ascx" TagName="HomeEvent" TagPrefix="ctrl" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <%-- <rad:RadAjaxPanel runat="server" >  --%> 
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
        
            <div>
                <div style="margin-left: -17px; margin-top: -20px; background-image: url(images/CalloutBack2.png); width: 897px; height: 454px;">
                    <div style="padding-left: 26px; padding-top: 18px;">
                        <div style="font-family: Baveuse; color: White; font-size: 24px;"><img src="images/TurnToHippo.png" alt="Turn to the hippo for all things local" /></div>
                        <div style="font-family: Arial; color: White; font-weight: normal; font-size: 12px; width: 800px; 
                        padding-top: 17px;padding-bottom: 33px;">
                        HippoHappenings aims to strengthen the community by bringing you closer to what is going 
                        on in the neighborhood. <br />We bring you event, venue, and classifieds postings, 
                        do not allow non-local big-corporations posts and much more:</div>
                        <div class="topDiv">
                            <div style="float: left; width: 211px;padding-left: 12px; text-align: center;">
                                <img src="images/TotalUserControl.png" alt="Total User Control" />
                                <span style="font-family: Arial; color: White; font-size: 14px; text-align: center; line-height: 14px;">
                                    <br /><br /><a class="AddLink" href="EnterEvent.aspx" style="font-weight: bold; font-size: 15px;">Event</a>, <a style="font-weight: bold; font-size: 15px;" href="EnterVenueIntro.aspx" class="AddLightGreenLink">Venue</a>
                                     and <a class="AddOrangeLink" href="PostAnAd.aspx" style="font-weight: bold; font-size: 15px;">Classifieds postings</a> 
                                    are FREE!<br /> <br /> 

                                    No Administrators: Users manage <br /> 
                                    posting and editing of <a class="AddLink" href="About.aspx#tag4" style="font-weight: bold; font-size: 15px;">events</a>, <br /> 
                                    <a class="AddLightGreenLink" href="About.aspx#tag10" style="font-weight: bold; font-size: 15px;">venues</a> or <a class="AddOrangeLink" href="About.aspx#tag16" style="font-weight: bold; font-size: 15px;">classifieds</a><br /> <br /> 

                                    Full control of the local <a class="AddOrangeLink" href="About.aspx#tag23" style="font-weight: bold; font-size: 15px;">classifieds</a>  
                                    appearing on the site<br /> <br /> 

                                    <a class="AddLightGreenLink" href="Vote.aspx" style="font-weight: bold; font-size: 15px;">Vote</a> on new functionality for <br /> 
                                    the Hippo!
                                </span>
                            </div>
                            <div style="float: left; width: 225px; padding-left: 79px; padding-top: 55px; text-align: center;">
                                <img src="images/UnparalleledFunctionality.png" alt="Unparalleled User functionality" />
                                <span style="font-family: Arial; color: White; font-size: 14px; text-align: center; line-height: 14px;">
                                    <br /><br />Integrated with: <a class="AddLink" href="EventSearch.aspx" style="font-weight: bold; font-size: 15px;">Ticketmaster</a>,<br />
                                    <a class="AddOrangeLink" href="EventSearch.aspx" style="font-weight: bold; font-size: 15px;">Facebook</a>, 
                                    <a class="AddWhiteLink" href="EventSearch.aspx" style="font-weight: bold; font-size: 15px;">Myspace</a> and 
                                    <a class="AddLightGreenLink" href="EventSearch.aspx" style="font-weight: bold; font-size: 15px;">Twitter</a><br /><br />
                                     
                                    Get <a class="AddOrangeLink" href="About.aspx#tag25" style="font-weight: bold; font-size: 15px;">Statistics</a> on the Featured Classifieds you post<br /><br />

                                    Never see non-local big-corporation ad
                                    here—only local people,
                                    businesses and <a class="AddLink" href="PostAnAd.aspx" style="font-weight: bold; font-size: 15px;">you can post</a> <br /><br />

                                    <a class="AddLightGreenLink" href="User.aspx" style="font-weight: bold; font-size: 15px;">Customize</a> event <br />
                                    recommendations and friend <br />
                                    update alerts<br />
                                </span>
                            </div>
                            <div style="float: left; width: 206px; padding-left: 73px; text-align: center;">
                                <img src="images/TightKnit.png" alt="Tight-Knit Neignborhood" />
                                <span style="font-family: Arial; color: White; font-size: 14px; text-align: center; line-height: 14px;">
                                    <br /><br /> <a href="User.aspx" class="AddLightGreenLink" style="font-weight: bold; font-size: 15px;">
                                    Keep up</a> with your  <a href="User.aspx" class="AddOrangeLink" style="font-weight: bold; font-size: 15px;">
                                    neighbors</a> <br />
                                    and their posts<br /><br />

                                    Receive  <a class="AddLink" href="AdSearch.aspx" style="font-weight: bold; font-size: 15px;">notices</a> for a new classified in
                                    your search criteria<br /><br />

                                    Receive <a class="AddOrangeLink" href="About.aspx#tag32" style="font-weight: bold; font-size: 15px;">text message</a> and/or <br />
                                    <a class="AddLightGreenLink" href="About.aspx#tag32" style="font-weight: bold; font-size: 15px;">email alerts</a> for your favorite <br />
                                    venues and events<br /><br />

                                    <a class="AddOrangeLink" href="User.aspx" style="font-weight: bold; font-size: 15px;">Connect</a> with others going to <br />
                                    the same events<br />
                                </span>
                            </div>
                        </div>
                    </div>
                </div> 
            </div>
            <div class="topDiv">
            <div style="float: left; width: 440px;">
            <asp:UpdatePanel runat="server" UpdateMode="conditional">
                <ContentTemplate>
            <div style="float: left;width: 440px; padding-top: 6px;" align="left">
            <div id="topDiv" >
                
                <div style="float: left; width: 200px;">
                     <%--<ctrl:HippoTextBox runat="server" ID="SearchTextBox" TEXTBOX_WIDTH="100" 
                     CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                     <asp:ImageButton CssClass="NavBarImage" ID="SearchButton" runat="server" 
                     OnClick="Search" CommandArgument="A" ImageUrl="~/image/SearchButton.png"  
                     onmouseout="this.src='image/SearchButton.png'" 
                     onmouseover="this.src='image/SearchButtonSelected.png'"  />--%>
                    <div style="font-weight: bold;background-color: #333333; padding: 10px; border: solid 2px #1fb6e7; height: 100px;">
                        <span style="color: White; font-size: 14px;"> Tip of the week: </span><br />
                        <span style="color: #cccccc; font-size: 12px;">Featured Classifieds are currently free. Plus, since there is low classifieds traffic, 
                        your classifieds ad will show more frequently to each user. <a class="AddLink" href="PostAnAd.aspx">Post Away!</a></span>
                    </div>
                     <div class="EventHeader" style="font-size: 12px; padding-top: 30px;">Choose a Date for Your Top Events<br />
                     For all events visit the <a class="AddLink" href="EventSearch.aspx">search page</a></div>
                     <%--<div class="EventBody" style="width: 200px;">
                        Select date to filter below events.<br /><br />
                        <span style=" font-style: italic;">Search</span> 
                        <a href="eventsearch.aspx" class="AddLink" style="color: #a467c2">Events</a>,
                        <a href="venuesearch.aspx" class="AddLink">Venues</a>,
                        <a href="adsearch.aspx" class="AddLink" style="color: #ff7704">Ads</a>
                        <br /><br />
                        <span style=" font-style: italic;">Post</span>
                        <a href="enterevent.aspx" class="AddLink" style="color: #a467c2">Events</a>,
                        <a href="entervenue.aspx" class="AddLink"> Venues</a>,
                        <a href="postanad.aspx" class="AddLink" style="color: #ff7704">Ads</a>
                        <br /><br />
                        <asp:Panel runat="server" ID="logInPanel">
                        To filter the user ads on right, and much more, <a href="userlogin.aspx" class="AddLink">log in</a>.
                        </asp:Panel>
                        <asp:Panel runat="server" ID="loggedInPanel">
                            <a class="AddLightGreenLink" href="Vote.aspx">Vote! on new functionality</a>
                        </asp:Panel>
                     </div>--%>
                </div>
                <div style="float: right; padding-bottom: 30px;">
                    <rad:RadCalendar runat="server" ID="RadCalendar1" EnableMultiSelect="false" AutoPostBack="true" OnSelectionChanged="GoToSearch" Skin="Hay"></rad:RadCalendar>
                </div>
            </div>
            <div class="EventHeader" style="font-size: 25px;">Top Events in <asp:Label runat="server" id="LocationLabel"></asp:Label></div>
            <asp:Panel runat="server" ID="EventPanel"></asp:Panel>
            </div>
            
                
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<div style="float: left;width: 440px;" align="left">
                
                
                <h1><asp:HyperLink runat="server" CssClass="EventHeader" ID="EventName"></asp:HyperLink></h1>
                
                <asp:HyperLink runat="server" ID="VenueName" CssClass="VenueName"></asp:HyperLink><br />
                <asp:Label runat="server" ID="DateAndTimeLabel" CssClass="DateTimeLabel"></asp:Label><br />
                <ctrl:HippoRating ID="HippoRating1" runat="server" />
                <br />
                <asp:Panel runat="server" ID="SongPanel"></asp:Panel>
                <asp:Panel runat="server" ID="CalendarPanel"></asp:Panel>
               
               <ctrl:AddTo runat="server" />
                <ctrl:SendTxt runat="server" ID="SendTxtID" />
                <ctrl:SendEmail runat="server" THE_TEXT="Send Email with this Info" ID="SendEmailID" />
                <div style="padding-top: 5px;">
                    <script type="text/javascript" src="http://widgets.amung.us/classic.js"></script><script type="text/javascript">WAU_classic('k3nyx4trfy6a')</script>
                </div>
                <div style="padding-top: 5px;">
                    <asp:Label runat="server" ID="ShowDescriptionBegining" CssClass="EventBody"></asp:Label>
                    <asp:Literal runat="server" ID="ShowVideoPictureLiteral">
                    </asp:Literal>
                    <asp:Panel runat="server" ID="RotatorPanel" Visible="false">
                        <rad:RadRotator runat="server" ID="Rotator1" ItemHeight="200" ItemWidth="200"  Width="440px" Height="200px" RotatorType="Buttons" >
                            <ItemTemplate>
                                <div style="border: solid 2px blue; padding: 3px; margin: 3px;">
                                    <img width="184px" height="184px" src='<%# Container.DataItem %>' alt="Customer Image" />
                                </div>            
                            </ItemTemplate>
                        </rad:RadRotator>
                    </asp:Panel>
                     <asp:Label runat="server" ID="ShowRestOfDescription" CssClass="EventBody"></asp:Label>
                </div>
                <ctrl:Comments CUT_OFF="5" ID="TheComments" runat="server" />
            </div>--%>
            
        </div>
        <div style="float: right; padding-right: 12px;  width: 419px; padding-top: 30px;">
            <div style="clear: both;">
             <%--<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
                <ctrl:Ads ID="Ads1" runat="server" />
            </div>
            <div style="padding-left: 5px; width: 419px;">
               
                <%--<div style="padding-top: 15px;">
                    <ctrl:Poll runat="server"  />
                </div>--%>
                <%--<div style="padding-top: 28px;">
                    <ctrl:TagCloud runat="server" ID="TagCloud" />
                </div>--%>
            </div>
        </div>
        </div>
 <%-- </rad:RadAjaxPanel>  --%>
        
</asp:Content>

