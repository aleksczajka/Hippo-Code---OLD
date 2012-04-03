<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Copy of Home.aspx.cs" Inherits="Home" smartnavigation="true"
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

            <div style="margin-top: -4px;margin-left: -20px;background-image: url(images/HomeMountainBackground.gif); background-repeat: no-repeat; width: 900px;">
               <div style="margin-left: 7px;">
                    <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="345px" valign="top">
                        <asp:Timer ID="MissionTimer" OnTick="ChangeMission" runat="server" Interval="15000">
                        </asp:Timer>
                            <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="MissionTimer" EventName="Tick" />
                                </Triggers>
                                <ContentTemplate>
                                    <div style="margin-top: -6px; background-image: url(images/MissionBack.png); width: 358px; height: 294px;">
                                        <asp:Literal runat="server" ID="MissionLiteral"></asp:Literal>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </td>
                        <td width="540px" valign="top">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 24px;">
                                        <div style="background-image: url(images/HomeBlueRectangle.gif); width: 249px; height: 125px;">
                                            <div style="padding: 8px;">
                                                <div style=" float: left;padding-right: 22px;">
                                                    <img src="images/HomeEventsIcon.gif" />
                                                </div>
                                                <div style="float: left; width: 150px; color: White; font-size: 14px; padding-top: 18px; line-height: 18px; font-weight: bold;">
                                                    Search <a class="AddLink" href="EventSearch.aspx">Events</a>
                                                    <br />
                                                    Post <a class="AddLink" href="EnterEvent.aspx">Events</a>
                                                    <br />
                                                    View Recommended <a class="AddLink" href="User.aspx">Events</a>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="background-image: url(images/HomeOrangeRectangle.gif); width: 249px; height: 125px;">
                                            <div style="padding: 8px;">
                                                <div style="padding-right: 22px; float: left;">
                                                    <img src="images/HomeAdsIcon.gif" />
                                                </div>
                                                <div style="float: left; width: 150px; color: White; font-size: 14px; padding-top: 18px; line-height: 18px; font-weight: bold;">
                                                    Search <a class="AddOrangeLink" href="AdSearch.aspx">Classifieds</a>
                                                    <br />
                                                    Post <a class="AddOrangeLink" href="PostAnAd.aspx">Classifieds</a>
                                                    <br />
                                                    View Statistics on your <a class="AddOrangeLink" href="AdStatistics.aspx">Classifieds</a>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 13px;">
                                        <div style="background-image: url(images/HomeGreenRectangle.gif); width: 249px; height: 125px;">
                                            <div style="padding: 8px;">
                                                <div style="padding-right: 22px; float: left;">
                                                    <img src="images/HomeVenuesIcon.gif" />
                                                </div>
                                                <div style="float: left; width: 150px; color: White; font-size: 14px; padding-top: 18px; line-height: 18px; font-weight: bold;">
                                                    Search <a class="AddLightGreenLink" href="VenueSearch.aspx">Venues</a>
                                                    <br />
                                                    Post <a class="AddLightGreenLink" href="EnterVenue.aspx">Venues</a>
                                                    <br />
                                                    Go to your favorite <a class="AddLightGreenLink" href="User.aspx">Venues</a>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="padding-top: 13px;">
                                        <div style="background-image: url(images/HomePurpleRectangle.png); width: 249px; height: 125px;">
                                            <div style="padding: 8px;">
                                                <div style="padding-right: 22px; float: left;">
                                                    <img src="images/HomeGroupsIcon.gif" />
                                                </div>
                                                <div style="float: left; width: 150px; color: White; font-size: 14px; padding-top: 18px; line-height: 18px; font-weight: bold;">
                                                    Search <a class="AddPurpleLink" href="GroupSearch.aspx">Groups</a>
                                                    <br />
                                                    Create <a class="AddPurpleLink" href="EnterGroup.aspx">Groups</a>
                                                    <br />
                                                    Search <a class="AddPurpleLink" href="EventSearch.aspx">Group Events</a>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
               </div>
            </div>
            <div class="topDiv" style="padding-top: 20px;">
            
            <div style="float: left; width: 440px;">
            
            <asp:UpdatePanel runat="server" UpdateMode="conditional">
                <ContentTemplate>
            <div style="float: left;width: 440px; padding-top: 6px;" align="left">
            <%--<div id="topDiv" >
                
                <div style="float: left; width: 200px;">

                     <div class="EventHeader" style="font-size: 12px; padding-top: 30px;">Choose a Date for Your Top Events<br />
                     For all events visit the <a class="AddLink" href="EventSearch.aspx">search page</a></div>

                </div>
                <div style="float: right; padding-bottom: 30px;">
                    <rad:RadCalendar runat="server" ID="RadCalendar1" EnableMultiSelect="false" AutoPostBack="true" OnSelectionChanged="GoToSearch" Skin="Hay"></rad:RadCalendar>
                </div>
            </div>--%>
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
            <div style="padding-left: 5px;  width: 419px; padding-top: 30px;">
                <asp:Label runat="server" CssClass="PreferencesTitleAlt" ForeColor="#999999" ID="GroupsLabel" 
                Text="Latest Public Group Activity in Your Area"></asp:Label>
                <asp:Panel runat="server" ID="GroupsPanel"></asp:Panel>
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

