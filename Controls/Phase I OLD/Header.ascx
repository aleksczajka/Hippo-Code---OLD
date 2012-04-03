<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<script src="js/TotalJS.js" type="text/javascript"></script>
<script type="text/javascript">timedout();</script> 
<div class="topDiv" style=" width: 900px; font-family: Arial;">
    <div style="border-top: solid 6px black; padding-bottom: 30px;">
        <div align="left" style="height: 88px; background-image: url('image/HeaderBackground.gif'); background-repeat: repeat-x; padding-bottom: 0px; margin-bottom: 0px;">
            <div style="float: left;"><a href="Home.aspx">
            <img style="border: 0;" src="images/HippoLogo2.gif"/></a></div>
            <div style="float: right; padding: 0px; margin: 0px;">
                
                <asp:Panel runat="server" ID="LoginPanel" Visible="false" Height="15px" BackColor="Black">
                    <div align="right">
                        <a href="User.aspx"><img style="height: 12px; width: 69px; border: 0;" class="TopButton" src="../image/MyAccountImage.gif" /></a>
                        <asp:ImageButton Height="12px" Width="48px" runat="server" ID="LogOutButton" CssClass="TopButton" ImageUrl="~/image/LogOut.gif" OnClick="ImageGoTo" CommandArgument="O" />
                    </div>
                </asp:Panel>
                <div style="padding-top: 12px;">
                    <asp:Panel runat="server" ID="LoginButtonPanel" Visible="true">
                        <a href="Register.aspx"><img class="TopButton" style="border: 0;" src="../image/RegisterButton.png" onmouseout="this.src='image/RegisterButton.png'" onmouseover="this.src='image/RegisterButtonSelected.png'" /></a>
                        <a href="User.aspx"><img class="TopButton" style="border: 0;"  src="../image/LoginButton.png" onmouseout="this.src='image/LoginButton.png'" onmouseover="this.src='image/LoginButtonSelected.png'" /></a>
                    </asp:Panel>
                </div>
            </div>
             <div style="height: 15px; float: right; margin-top: -4px;">
                    <a href="About.aspx"><img class="TopHeaderImage" style="border: 0; padding-right: 3px;" src="../images/AboutButton.gif"/></a>
                    <a href="Feedback.aspx"><img class="TopHeaderImage" style="border: 0; padding-right: 3px;" src="../images/FeedbackButton.gif"  /></a>
                    <a href="Vote.aspx"><img class="TopHeaderImage" style="border: 0; padding-right: 5px;" src="../images/VoteButton.gif"  /></a>
                </div>
        </div>
        <div align="left" style="color: White;padding-top: 0px; margin-top: 0px; height: 29px; background-image: url('image/NavBackgroundDouble.png'); background-repeat: repeat-x;">
           
                <div style="float: left; padding-top: 1px; padding-bottom: 0px; margin-bottom: 0px; height: 27px;">
                    <div onclick="this.style.cursor = 'pointer';"  style="padding: 0px; margin: 0px; float: left;">
                             <a href="../Home.aspx"><img id="homeImg" onmouseout="this.src='../image/HomeLink.png'" onmouseover="this.src='../image/HomeLinkHover.png'" src="../image/HomeLink.png" class="NavBarImageHome" /></a>

<%--                        <asp:Button CssClass="NavBarImageHome" runat="server" ID="HomeLink" OnClick="GoTo" CommandArgument="H" />
--%>                    </div>
                    <%--<div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageBlog" runat="server" ID="BlogAnEventLink" OnClick="GoTo" CommandArgument="B" />
                    </div>--%>
                    <div style="padding: 0px; margin: 0px; float: left;">
                    <a onclick="this.style.cursor = 'pointer';"  href="EventSearch.aspx"><img id="eventImg" onclick="this.style.cursor = 'pointer';" onmouseout="this.src='../image/Events.png'" onmouseover="this.src='../image/EventsSelected.png'" src="../image/Events.png" class="NavBarImageEvent" /></a>
<%--                        <asp:Button CssClass="NavBarImageEvent" runat="server" ID="EventLink" OnClick="GoTo" CommandArgument="E" />
--%>                    </div>
                    
                        <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Panel runat="server" ID="CalendarPanel" Visible="false">
                            <a onclick="this.style.cursor = 'pointer';"  href="UserCalendar.aspx"><img onclick="this.style.cursor = 'pointer';"  onmouseout="this.src='../image/MyCalendarSelected.png'" onmouseover="this.src='../image/MyCalendar.png'" src="../image/MyCalendarSelected.png" class="NavBarImageCalendar" id="CalendarImg" /></a>
                        </asp:Panel>
                        </div>
                    
                    <div onclick="this.style.cursor = 'pointer';"  style="padding: 0px; margin: 0px; float: left;">
                        <a href="VenueSearch.aspx"><img id="VenueImg" onclick="this.style.cursor = 'pointer';"  
                        onmouseout="this.src='../image/VenuesLink.png'" onmouseover="this.src='../image/VenuesLinkHover.png'" src="../image/VenuesLink.png" class="NavBarImageVenue" /></a>
                    </div>
                    <div onclick="this.style.cursor = 'pointer';"  style="padding: 0px; margin: 0px; float: left;">
                                        <a href="AdSearch.aspx"><img id="adImg" onclick="this.style.cursor = 'pointer';"  
                                        src="../image/AdsClassifiedsLink.png"  onmouseout="this.src='../image/AdsClassifiedsLink.png'" onmouseover="this.src='../image/AdsClassifiedsSelected.png'" class="NavBarImageAd" /></a>

<%--                        <asp:Button CssClass="NavBarImageAd" runat="server" ID="AdsLink" OnClick="GoTo" CommandArgument="A" AlternateText="Ads And Clasifieds" />
--%>                    </div>
                    <div onclick="this.style.cursor = 'pointer';"  style="padding: 0px; margin: 0px; float: left;">
                        <a href="GroupSearch.aspx"><img id="Img1" onclick="this.style.cursor = 'pointer';"  
                        src="../images/Groups.png"  onmouseout="this.src='../images/Groups.png'" 
                        onmouseover="this.src='../images/GroupsSelected.png'" class="NavBarImageGroup" /></a>

                    </div>
                    
                </div>
                <script type="text/javascript">
                    if(window.location.pathname == '/Home.aspx')
                    {
                        var homeImg = document.getElementById('homeImg');
                        homeImg.src = '../image/HomeLinkHover.png';
                        homeImg.onmouseout = "";
                    }
                    else if(window.location.pathname == '/Event.aspx' || window.location.pathname == '/EventSearch.aspx')
                    {
                        var homeImg = document.getElementById('eventImg');
                        homeImg.src = '../image/EventsSelected.png';
                        homeImg.onmouseout = "";
                    }
                    else if(window.location.pathname == '/Venue.aspx' || window.location.pathname == '/VenueSearch.aspx')
                    {
                        var homeImg = document.getElementById('VenueImg');
                        homeImg.src = '../image/VenuesLinkHover.png';
                        homeImg.onmouseout = "";
                    }
                    else if(window.location.pathname == '/Ad.aspx' || window.location.pathname == '/AdSearch.aspx')
                    {
                        var homeImg = document.getElementById('adImg');
                        homeImg.src = '../image/AdsClassifiedsSelected.png';
                        homeImg.onmouseout = "";
                    }
                    else if(window.location.pathname == '/UserCalendar.aspx')
                    {
                        var homeImg = document.getElementById('CalendarImg');
                        homeImg.src = '../image/MyCalendar.png';
                        homeImg.onmouseout = "";
                    }
                    else if(window.location.pathname == '/GroupSearch.aspx')
                    {
                        var homeImg = document.getElementById('Img1');
                        homeImg.src = '../images/GroupsSelected.png';
                        homeImg.onmouseout = "";
                    }
                </script>
                <div style="float: right; padding-top: 1px; padding-bottom: 0px; margin-bottom: 0px; height: 27px;">
                    <%--<div style="padding: 0px; margin: 0px; float: left;">
                        <asp:ImageButton runat="server" onmouseover="this.src='~/image/PostAndWinButton.png';"  
                         ID="ImageButton3" 
                        OnClick="ImageGoTo" CommandArgument="W" ImageUrl="~/image/PostAndWinButton.png" />
                    </div>--%>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageAddGroup" runat="server" 
                        ID="Button4" OnClick="GoTo" CommandArgument="AddGroup" AlternateText="Make A Group" />
                    </div>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageAddAd" runat="server" ID="Button1" OnClick="GoTo" CommandArgument="AddAd" AlternateText="Post An Ad" />
                    </div>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageAddVenue" runat="server" ID="Button2" OnClick="GoTo" CommandArgument="AddVenue" AlternateText="Submit A Venue" />
                    </div>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageAddEvent" runat="server" ID="Button3" OnClick="GoTo" CommandArgument="AddEvent" AlternateText="Enter An Event" />
                    </div>
                    <%--<div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageAbout" runat="server" ID="AboutLink" OnClick="GoTo" CommandArgument="Ab" AlternateText="About" />
                    </div>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageFeedback" runat="server" ID="Button1" OnClick="GoTo" CommandArgument="Fb" AlternateText="Give us Feedback!" />
                    </div>
                    <div style="padding: 0px; margin: 0px; float: left;">
                        <asp:Button CssClass="NavBarImageVote" runat="server" ID="Button2" OnClick="GoTo" CommandArgument="Vo" AlternateText="Vote!" />
                    </div>--%>
                </div>
                <%--<asp:ImageButton runat="server" ID="AdsClassifiedsLink" OnClick="GoTo" CommandArgument="A"  />--%>
                <asp:Panel runat="server" DefaultButton="GoButton" Visible="false">
                    <div style="float: right; width: 250px; clear: none; padding: 0px; margin: 0px;">
                        <div style="height: 29px; clear: none; float: left; padding:0px; margin: 0px;">
                            
                            <div style="clear: none; height: 29px; padding: 0px; margin: 0px; padding-top: 6px;">
                                <div style="float: left; padding: 0px; margin: 0px ;padding-right: 3px; margin-top: 5px;"><img style="padding: 0px; margin: 0px;" src="image/SearchText.gif" /> </div>
                                <div class="TextBoxLeftImage">
                                <asp:TextBox runat="server" CssClass="TextBox" ID="SearchTextBox" ></asp:TextBox></div>
                            </div>
                            
                            
                        </div>
                        <div style="float: right; padding-top: 1px;">
                            <asp:ImageButton OnClick="ImageGoTo" CommandArgument="G" runat="server" ID="GoButton" ImageUrl="~/image/GoButton.png"  onmouseout="this.src='image/GoButton.png'" onmouseover="this.src='image/GoButtonSelected.png'"  />
                        </div>
                    </div>
                </asp:Panel>
          </div> 
    </div>
    <%--<div style="background-color: #1b1b1b; height: 32px; padding:0px; margin: 0px;">
        <div style="float: right; background-color: #363636; width: 380px; color: White; font-size: 10px; height: 32px;">
            <img src="image/PostsLeftBack.gif" style="float: left;" />
            <div style=" padding-top: 8px; font-weight: bold ; float: left; font-family: Arial; color: White; font-size: 12px;">
                <asp:Panel ID="NumberPanel" runat="server">Only <asp:Label runat="server" CssClass="AddLink" ID="PostsNumberLabel" Text="20"></asp:Label> Posts left until your free gift!!
                </asp:Panel>
            </div>
            
            <asp:ImageButton runat="server" ID="PostButton" ImageUrl="~/image/PostAndWinButton.png" OnClick="ImageGoTo" CommandArgument="P" onmouseout="this.src='image/PostAndWinButton.png'" onmouseover="this.src='image/PostAndWinButtonSelected.png'"  />
<%--            <asp:ImageButton runat="server" ID="RSSButton" ImageUrl="~/image/RssButton.png" onmouseout="this.src='image/RSSButton.png'" onmouseover="this.src='image/RSSButtonSelected.png'"  />
        </div>
    </div>--%>
</div>