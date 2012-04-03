<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="User_OLD2.aspx.cs" 
Inherits="User" Title="Hipp Happ User" Debug="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BigAd.ascx" TagName="BigAd" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<rad:RadAjaxPanel runat="server">--%>
<div class="topDiv">
    <rad:RadCodeBlock runat="server">
        
        <script type="text/javascript">
        function CloseToolTip()
        {
            var controller = Telerik.Web.UI.RadToolTipController.getInstance(); 
            var tooltip = controller.get_activeToolTip(); 
            if (tooltip) tooltip.hide();
        }
        function SelectPreferences()
        {
            var tabStrip = $find("<%=RadTabStrip2.ClientID%>");
            tabStrip.set_selectedIndex(2);
        }
        function setRead(theID)
        {
            var theDiv = document.getElementById(theID);
            var newDiv = document.getElementById('newDiv');
            var newSpan = document.getElementById('newSpan');
            var unreadcount = parseInt(newDiv.innerHTML) - 1;
            if(theDiv.style.fontWeight == 'bold')
            {
                theDiv.style.fontWeight = 'normal';
                theDiv.style.color = '#cccccc';
                newSpan.innerHTML = '('+unreadcount.toString() + ' New)';
            }
        }
        function SetWorking(theID)
        {
            var theButt = document.getElementById(theID);
            theButt.innerHTML = 'Working...';
            theButt.disabled = true;
        }
        function setBackground(theID)
        {
            document.getElementById(theID).style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';
        }
        function onTabSelected(sender, args)
        {
            if(args.get_tab().get_index() == 0)
            {
   	            args.get_tab().set_cssClass('MyTabsMessagesSelected');
   	            
   	        }
   	        else if(args.get_tab().get_index() == 1)
   	        {
   	            args.get_tab().set_cssClass('MyTabsFriendsSelected');
   	        }
   	        else if(args.get_tab().get_index() == 2)
   	        {
   	            args.get_tab().set_cssClass('MyTabsPreferencesSelected');
   	        }
   	        else if(args.get_tab().get_index() == 3)
   	        {
   	            args.get_tab().set_cssClass('MyTabsGroupsSelected');
   	        }
        }
        
        function onTabUnSelected(sender, args)
        {
           if(args.get_tab().get_index() == 0)
            {
   	            args.get_tab().set_cssClass('MyTabsMessages');
   	            
   	        }else if(args.get_tab().get_index() == 1)
   	        {
   	            args.get_tab().set_cssClass('MyTabsFriends');
   	        }else if(args.get_tab().get_index() == 2)
   	        {
   	            args.get_tab().set_cssClass('MyTabsPreferences');
   	        }
   	        else if(args.get_tab().get_index() == 3)
   	        {
   	            args.get_tab().set_cssClass('MyTabsGroups');
   	        }
        }
        function onTabMouseOver(sender, args)
        {
            if(args.get_tab().get_selected())
            {
            
            }else
            {
                if(args.get_tab().get_index() == 0)
                {
   	                args.get_tab().set_cssClass('MyTabsMessagesHover');
       	            
   	            }else if(args.get_tab().get_index() == 1)
   	            {
   	                args.get_tab().set_cssClass('MyTabsFriendsHover');
   	            }else if(args.get_tab().get_index() == 2)
   	            {
   	                args.get_tab().set_cssClass('MyTabsPreferencesHover');
   	            }
   	            else if(args.get_tab().get_index() == 3)
   	            {
   	                args.get_tab().set_cssClass('MyTabsGroupsHover');
   	            }
            }
        }
        function onTabMouseOut(sender, args)
        {
            if(args.get_tab().get_selected())
            {
            
            }else
            {
                if(args.get_tab().get_index() == 0)
                {
   	                args.get_tab().set_cssClass('MyTabsMessages');
       	            
   	            }else if(args.get_tab().get_index() == 1)
   	            {
   	                args.get_tab().set_cssClass('MyTabsFriends');
   	            }else if(args.get_tab().get_index() == 2)
   	            {
   	                args.get_tab().set_cssClass('MyTabsPreferences');
   	            }
   	            else if(args.get_tab().get_index() == 3)
   	            {
   	                args.get_tab().set_cssClass('MyTabsGroups');
   	            }
            }
        }
        function Archive(i)
        {
            var theDiv = document.getElementById('div'+i);
            theDiv.style.display = 'none';
            User.ArchiveMessage(i);
        }
        function OpenRad()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('AddFriendAlert.aspx');
            win.show(); 
            win.center(); 
               
         }
         function OpenRad2()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('InviteFriendAlert.aspx');
            win.show(); 
            win.center(); 
               
         }
          function OpenRadRecom()
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('Recommendations.aspx');
            win.show(); 
            win.center(); 
               
         }
        function OpenRevoke(theID)
        {
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('RevokeMembership.aspx?ID='+theID);
            win.show(); 
            win.center(); 
               
        }
        
        function DoFillGroups()
        {
            __doPostBack('<%=DoFillGroups.ClientID %>','');
        }
        
        function OpenEmail(num)
        {
            var contentDiv = document.getElementById('contentDiv'+num);
            var readDiv = document.getElementById('readDiv'+num);
            var idDiv = document.getElementById('idDiv'+num);
            var headerDiv = document.getElementById('header'+num);
            
            if(contentDiv.style.display == 'block')
                contentDiv.style.display = 'none';
            else
            {
                contentDiv.style.display = 'block';
                if(readDiv.innerHTML == 'False')
                {
                    headerDiv.style.fontWeight = 'normal';
                    headerDiv.style.fontSize = '12px';
                    readDiv.innerHTML = 'True';
                    var messagesCount = document.getElementById('messagesCount');
                    messagesCount.innerHTML = messagesCount.innerHTML - 1;
                    //User.MarkAsRead(idDiv.innerHTML);
                }
            }
        }
        function AcceptFriend(num)
        {
            var fromDiv = document.getElementById('fromDiv'+num);
            var userDiv = document.getElementById('userDiv'+num);
            
            User.AcceptFriend(userDiv.innerHTML,fromDiv.innerHTML, num, Callback_Accept);
        }
        function Callback_Accept(response)
        {
            var acceptDiv = document.getElementById('accept'+response.value);
            acceptDiv.innerHTML = "You have accepted this user as a friend";
        }
        function ReplyMessage(num)
        {
            var textDiv = document.getElementById('textDiv'+num);
            if(textDiv.value == "")
            {
                var message = document.getElementById('message'+num);
                message.innerHTML = 'Please include the message!';
            }else
            {
                var fromDiv = document.getElementById('fromDiv'+num);
                var userDiv = document.getElementById('userDiv'+num);
                
                var subject = document.getElementById('subject'+num);
                var idDiv = document.getElementById('idDiv'+num);
                User.Reply(textDiv.value, subject.innerHTML, fromDiv.innerHTML, userDiv.innerHTML, idDiv.innerHTML, num, Callback_Reply);
            }
        }
        function Callback_Reply(response)
        {
            var message = document.getElementById('message'+response.value);
            message.innerHTML = 'Your message has been sent!';
//            }else
//            {
//                message.innerHTML = 'Your message could not be sent. Please <a class="AddGreenLink" href="ContactUs.aspx">contact us</a> for assistance!';
//            }
        }
        function DeleteEmail(num)
        {
            var contentDiv = document.getElementById('contentDiv'+num);
            var idDiv = document.getElementById('idDiv'+num);
            var headerDiv = document.getElementById('header'+num);
            contentDiv.style.display = 'none';
            headerDiv.style.display = 'none';
            var globalMessage = document.getElementById('globalMessage');
            globalMessage.innerHTML = "Your message has been deleted";
            User.ArchiveMessage(idDiv.innerHTML);
        }
        
        function OnClientClose(oWnd, args)
            {
                if(args != "null" && args != undefined && args != null && args != "")
                    window.location = args;
            }
            function OpenPostingDiv()
            {
                var theDiv = document.getElementById('HiddenPostDiv');
                theDiv.style.display = 'block';
            }
            function ClosePostingDiv()
            {
                var theDiv = document.getElementById('HiddenPostDiv');
                theDiv.style.display = 'none';
            }
            function OpenEventDiv()
            {
                var theDiv = document.getElementById('HiddenEventDiv');
                theDiv.style.display = 'block';
            }
            function CloseEventDiv()
            {
                var theDiv = document.getElementById('HiddenEventDiv');
                theDiv.style.display = 'none';
            }
            function OpenCommentDiv()
            {
                var theDiv = document.getElementById('HiddenCommentDiv');
                theDiv.style.display = 'block';
            }
            function CloseCommentDiv()
            {
                var theDiv = document.getElementById('HiddenCommentDiv');
                theDiv.style.display = 'none';
            }
    </script>
    </rad:RadCodeBlock>

    <div runat="server" id="TheDiv"></div>
    <div id="topDiv" style="padding-top: 10px;">
    <rad:RadWindowManager Behaviors="Close"
            ID="RadWindowManager" Modal="true" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" DestroyOnClose="false" VisibleStatusbar="false"
                 BorderStyle="Solid"
                 VisibleTitlebar="false" BorderWidth="3px" BorderColor="black"
                VisibleOnPageLoad="false" Height="500" ClientCallBackFunction="OnClientClose" Width="750px" Skin="Black"
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
            </Windows>
   </rad:RadWindowManager>
   <telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false" Skin="Black" 
                BorderStyle="solid"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <telerik:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Black" Height="200" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                    <telerik:RadWindow Width="300" ClientCallBackFunction="DoFillGroups" EnableEmbeddedSkins="true" 
                    VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Black" Height="200" ID="RadWindow2" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        <div style="float: left; width: 450px; padding-bottom: 10px;margin-top: -35px;">
            <div class="topDiv">
                <div style="float: left; padding-right: 5px; padding-top: 5px;">
                    <div id="ImageDiv" style="width: 30px;" runat="server">
                    <asp:Image runat="server" ToolTip="What's this page for?" CssClass="HelpImage" 
                    AlternateText="What's this page for?" ID="QuestionMark6" 
                    ImageUrl="~/image/helpIcon.png"></asp:Image></div>
                    <rad:RadToolTip Skin="Black" ID="RadToolTip1" runat="server" ManualClose="true" TargetControlID="ImageDiv" Position="MiddleRight" RelativeTo="element" ShowEvent="OnClick">
                        <div style="width: 350px; color: #cccccc; font-size: 14px; font-family: Arial; padding-bottom: 10px;">
                          <label>Check out the events calendar to <span style="font-weight: bold; font-size: 14px;">send events to your friends, view the event's information and connect with people going to the events. </span>
                          Add your friends and check/send messages below. You will also find your recommended events and venues you have specified as your favorite.</label>
                        </div>
                    </rad:RadToolTip>
                </div>
                <div style="float: left;">
                    <h1 class="EventHeader" style="margin: 0; padding-bottom: 0px;">Welcome <asp:Label runat="server" ID="UserLabel"></asp:Label>!</h1>
                </div>
            </div>
                                <asp:Label runat="server" ForeColor="Red" ID="UserErrorLabel"></asp:Label>     
            <div>
            <span style="font-family: Arial; font-size: 12px; color: White; font-weight: bold;">Your Week at a glance</span>
            <asp:HyperLink runat="server" ID="CalendarLink" CssClass="SmallLink">(Full Calendar)</asp:HyperLink>
                    <asp:Button runat="server" ID="Button2" Text="Pages & Searches" CssClass="SearchButton" OnClick="GoToSearches"
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
                  
                    <asp:Button runat="server" ID="Button3" Text="Ad Statistics" CssClass="SearchButton" 
                    OnClick="GoToAdStatistics" onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" />
         </div>   
            <div class="topDiv" style="padding-top: 2px; padding-bottom: 5px;">
                <ctrl:GlanceDay ID="GlanceDay1" NUM_OF_EVENTS="0" THE_DAY="Sun" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay2" NUM_OF_EVENTS="1" THE_DAY="Mon" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay3" NUM_OF_EVENTS="0" THE_DAY="Tues" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay4" NUM_OF_EVENTS="0" THE_DAY="Wed" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay5" NUM_OF_EVENTS="1" THE_DAY="Thurs" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay6" NUM_OF_EVENTS="0" THE_DAY="Fri" runat="server" />
                <ctrl:GlanceDay ID="GlanceDay7" NUM_OF_EVENTS="0" THE_DAY="Sat" runat="server" />
            </div>
          <%--  <asp:Panel Visible="false" runat="server" ID="FriendMessagesPanel" Width="440px" ScrollBars="Vertical">
                <asp:Literal runat="server" ID="FriendMessagesLiteral"></asp:Literal>
            </asp:Panel>--%>
            <div class="topDiv EventDiv" style="width: 450px;">
                    <span style="font-family: Arial; font-size: 12px; color: White;
                        font-weight: bold; padding-right: 20px;"><asp:Label runat="server" ID="YourGroupsLabel"></asp:Label></span>
                    <asp:Panel Visible="false" Height="65px" ScrollBars="Vertical" runat="server" 
                    ID="GroupUpdatesPanel">

                    </asp:Panel>
                
            </div>
            
        </div>
         <div style="float: right; padding-bottom: 5px; padding-right: 5px;">
            
            <ctrl:BigAd runat="server" />
            
            
            
        </div>
    </div>
    <a id="middle"></a>
   <%--<div >
        
        <table height="170px">
            <tr>
                <td height="170px" valign="bottom">
                    <asp:ImageButton runat="server" ImageAlign="Bottom" ID="MessagesButton" PostBackUrl="#middle" 
                    OnClick="GoTo" CommandArgument="M" ImageUrl="image/MyMessagesHover.png" Width="280px"
                     />
                </td>
                <td height="170px" valign="bottom">
                    <asp:ImageButton runat="server" ID="FriendsButton" PostBackUrl="#middle" ImageUrl="image/MyFriends.png" 
                    OnClick="GoTo" CommandArgument="F" Width="280px" />
                </td>
                <td height="170px" valign="bottom">
                    <asp:ImageButton runat="server" ID="PreferencesButton" OnClick="GoTo" CommandArgument="P" 
                    ImageUrl="image/MyPreferences.png" Width="280px" 
                    onmouseout="this.src='image/MyPreferences.png'" onmouseover="this.src='image/MyPreferencesHover.png'"/>
                </td>
            </tr>
        </table>
   </div>--%>
   <div id="HelloDiv" style="font-size: 20px; color: White; display: block;"></div>
  <span style="font-family: Arial; font-size: 12px; color: White; font-weight: bold;">Your Friends' Updates</span>
   <div class="topDiv" style="padding-top: 5px;">
        <div style="float: left;width: 250px; height: 160px;">
            <div style="margin-top: -5px;padding-left: 17px;">
                <img src="image/PostingAction.png" />
            </div>
            <div style="margin-top: -28px;padding-left: 35px;">
                <img src="image/EventGoingAction.png" />
            </div>
            <div style="margin-top: -53px;padding-left: 57px;">
                <img src="image/CommentingAction.png" />
            </div>
        </div>
        <script type="text/javascript">
//            function startUpScripts()
//            {
//                var newsTickerRotator = $find('<%= PostingRotator.ClientID %>');
//                newsTickerRotator.repaint();
//                startRotator(newsTickerRotator, Telerik.Web.UI.RotatorScrollDirection.Down);
//            }
            function goPostNext()
            {
                var mainRotator = $find('<%=PostingRotator.ClientID %>');
                mainRotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Up); 
            }
        </script>
        <div id="HiddenPostDiv" style="margin-top: 10px;background-color: #1b1b1b;position: absolute;border: solid 2px #ff6b09;float: left; margin-left: 245px;width: 620px; height: 180px; display: none;" >
            <asp:Image ImageUrl="image/ArrowDown.png" CssClass="RotatorArrow" ID="imgUp" 
            AlternateText="right" runat="server" />
            <asp:Image ImageUrl="image/ArrowUp.png" CssClass="RotatorArrowDown" ID="imgDown" 
            AlternateText="right" runat="server" />
            <rad:RadRotator runat="server" WrapFrames="false" 
            ID="PostingRotator" ScrollDuration="300" ControlButtons-UpButtonID="imgDown" 
             ControlButtons-DownButtonID="imgUp"  ItemHeight="159px" ItemWidth="620px"
            Width="620px" 
            Height="159px" RotatorType="Buttons" ScrollDirection="Up,Down">
            </rad:RadRotator>
            <div style="float: right; margin-top: 6px; margin-right: 5px; cursor: pointer;"><img onclick="ClosePostingDiv();" src="image/OrangeX.png" /></div>
        </div>
        <script type="text/javascript">
            function goEventNext()
            {
                var mainRotator = $find('<%=EventRotator.ClientID %>');
                mainRotator.showNext(Telerik.Web.UI.RotatorScrollDirection.Down); 
            }
        </script>
        <div id="HiddenEventDiv" style="margin-top: 10px;background-color: #1b1b1b;position: absolute;border: solid 2px #1fb6e7;float: left; margin-left: 245px;width: 620px; height: 180px; display: none;" >
            <asp:Image ImageUrl="image/ArrowDownBlue.png" CssClass="RotatorArrow" ID="Image1" 
            AlternateText="right" runat="server" />
            <asp:Image ImageUrl="image/ArrowUpBlue.png" CssClass="RotatorArrowDown" ID="Image2" 
            AlternateText="right" runat="server" />
            <rad:RadRotator runat="server" WrapFrames="false" 
            ID="EventRotator" ScrollDuration="300" ControlButtons-UpButtonID="Image2" 
             ControlButtons-DownButtonID="Image1" ItemHeight="159px" ItemWidth="620px"  
            Width="620px" 
            Height="159px" RotatorType="Buttons" ScrollDirection="Up,Down">
            </rad:RadRotator>
            <div style="float: right; margin-top: 6px; margin-right: 5px; cursor: pointer;"><img onclick="CloseEventDiv();" src="image/BlueX.png" /></div>
        </div>
        <div id="HiddenCommentDiv" style="margin-top: 10px;background-color: #1b1b1b;position: absolute;border: solid 2px #628e02;float: left; margin-left: 245px;width: 620px; height: 180px; display: none;" >
            <asp:Image ImageUrl="image/ArrowDownGreen.png" CssClass="RotatorArrow" ID="Image3" 
            AlternateText="right" runat="server" />
            <asp:Image ImageUrl="image/ArrowUpGreen.png" CssClass="RotatorArrowDown" ID="Image4" 
            AlternateText="right" runat="server" />
            <rad:RadRotator runat="server" WrapFrames="false" 
            ID="CommentRotator" ScrollDuration="300" ControlButtons-UpButtonID="Image4" 
             ControlButtons-DownButtonID="Image3" ItemHeight="159px" ItemWidth="620px"   
            Width="620px" 
            Height="159px" RotatorType="Buttons" ScrollDirection="Up,Down">
            </rad:RadRotator>
            <div style="float: right; margin-top: 6px; margin-right: 5px; cursor: pointer;"><img onclick="CloseCommentDiv();" src="image/GreenX.png" /></div>
        </div>
        <div style="float: left; padding-left: 15px;width: 170px; padding-bottom: 20px;">
            <asp:Literal runat="server" ID="PostingLiteral"></asp:Literal>
        </div>
        <div style="float: left; padding-left: 30px;width: 170px;">
            <asp:Literal runat="server" ID="EventGoingLiteral"></asp:Literal>
        </div>
        <div style="float: left; padding-left: 40px;width: 170px;">
            <asp:Literal runat="server" ID="CommentingLiteral"></asp:Literal>
        </div>
   </div>
  
   <asp:UpdatePanel runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
   <Triggers>
    <asp:PostBackTrigger ControlID="Button4" />
   </Triggers>
    <ContentTemplate>
   <div style="width: 900px; display: block;" >
   <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <rad:RadTabStrip Width="280px" Orientation="VerticalLeft" Align="right" ID="RadTabStrip2" Skin="Black"
                             runat="server" MultiPageID="TheMultipage" OnClientTabUnSelected="onTabUnSelected" 
                             OnClientTabSelected="onTabSelected" OnClientMouseOut="onTabMouseOut" 
                             OnClientMouseOver="onTabMouseOver"
                               style="float:left">
                                <Tabs>
                                    <rad:RadTab CssClass="MyTabsMessagesSelected" Width="280px" TabIndex="0" Selected="true"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsFriends" Width="280px"  TabIndex="1"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsPreferences" Width="280px"  TabIndex="2"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsGroups" Width="280px"  TabIndex="3"></rad:RadTab>
                                </Tabs>
                            </rad:RadTabStrip>
                        </td>
                     </tr>
                     <tr>   
                        <td valign="top">
                              <div class="topDiv">
                                <rad:RadTabStrip runat="server" ID="RadTabStrip1" Orientation="HorizontalTop"
                                     MultiPageID="RadMultiPage1" Skin="Black" SelectedIndex="0" >
                                    <Tabs>
                                        <rad:RadTab Text="New Events" TabIndex="0" PageViewID="RadPageView1">
                                        </rad:RadTab>
                                        <rad:RadTab Text="Favorite Venues" TabIndex="1" PageViewID="RadPageView2">
                                        </rad:RadTab>
                                    </Tabs>
                                </rad:RadTabStrip>
                                <rad:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                                        <rad:RadPageView  BackColor="#1b1b1b" runat="server"  Width="280px" ID="RadPageView1" TabIndex="0">
                                        <asp:UpdatePanel ID="RecomUpdate" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        <asp:Panel runat="server" ID="RecommendedEvents" Wrap="true" Width="260px" 
                                        CssClass="FavoritesPanel"></asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </rad:RadPageView>
                                    <rad:RadPageView runat="server"  BackColor="#1b1b1b"  Width="280px" ID="RadPageView2" TabIndex="1">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        <asp:Panel Wrap="true" runat="server" Width="260px" ID="FavoriteVenues" 
                                        CssClass="FavoritesPanel"></asp:Panel>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </rad:RadPageView>
                                </rad:RadMultiPage>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                     <rad:RadMultiPage ID="TheMultipage" 
                         SelectedIndex="0" runat="server" >
                            <rad:RadPageView ID="Tab1" runat="server" >
                                <div style="height: 45px; margin-top: 1px; background-color: #1b1b1b; width: 7px; float: left;"></div>
                                <div style="float: left; width: 570px;"> 
                                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
                                                    
                                                    <ContentTemplate>
                                                    <asp:Label runat="server" ForeColor="Red" ID="MessagesLabel"></asp:Label>
                                  <div style="width: 570px; display: block; float: left;">
                                        <rad:RadTabStrip Orientation="HorizontalTop"
                                        ID="RadTabStrip3" Skin="Black"
                                             runat="server" MultiPageID="RadMultiPage2">
                                            <Tabs>
                                                <rad:RadTab Selected="true" Text="Inbox"></rad:RadTab>
                                                <rad:RadTab Text="Sent Messages"></rad:RadTab>
                                            </Tabs>
                                        </rad:RadTabStrip>
                                        <rad:RadMultiPage ID="RadMultiPage2" SelectedIndex="0" runat="server">
                                            <rad:RadPageView ID="RadPageView3" runat="server">
                                                <%--<rad:RadPanelBar  BorderColor="#363636" BorderStyle="solid" BorderWidth="3px" ExpandAnimation-Type="Linear" 
                                                ExpandAnimation-Duration="50" AllowCollapseAllItems="true" 
                                                    ExpandMode="SingleExpandedItem" Width="570px" 
                                                    ID="RadMessagePanel" runat="server">
                                                </rad:RadPanelBar>--%>
                                                
                                                <asp:Panel runat="server" ID="MessagesPanel"></asp:Panel>
                                                   
                                            </rad:RadPageView>
                                            <rad:RadPageView ID="RadPageView4" runat="server">
                                            
                                                <asp:Panel runat="server" ID="UsedMessagesPanel"></asp:Panel>
                                                <%--<rad:RadPanelBar  BorderColor="#363636" BorderStyle="solid" BorderWidth="3px"  AllowCollapseAllItems="true" 
                                                    ExpandMode="SingleExpandedItem" Width="570px" 
                                                    ID="RadSentMessagePanel" runat="server">
                                                </rad:RadPanelBar>--%>
                                                 
                                            </rad:RadPageView>
                                        </rad:RadMultiPage>
                                    </div>
                                                 </ContentTemplate>
                                </asp:UpdatePanel>
                                </div>
                               
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab2" runat="server" >
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
                                   
                                    <ContentTemplate>
                                <asp:Panel runat="server" ID="FriendPanel">
                                    <div class="topDiv">
                                    <div style="height: 45px; margin-top: 47px; background-color: #1b1b1b; 
                                    width: 7px; float: left;"></div>
                                    <div style="border: solid 3px #1b1b1b; margin-top: 1px; float: left; width: 570px;">
                                    <div style="width: 560px; display: block; float: left; clear: both; margin-left: 5px;">
                                      <table width="100%">
                                        <tr>
                                            <td>
                                                <span style="font-family: Arial; font-size: 12px; color: #cccccc; padding-left: 5px;">
                                                (<asp:Label runat="server" ID="NumFriendsLabel"></asp:Label> friends )
                                                </span>
                               
                                            </td>
                                            <td align="right" style="padding-right: 10px;">
                                              <img src="image/AddIcon.png" /><a class="AddLink" href="javascript:OpenRad2();">Invite Friends To HippoHappenings</a> 
                                                <img src="image/AddIcon.png" /><a class="AddLink" href="javascript:OpenRad();">Search for Friends On HippoHappenings</a>
                                            </td>
                                        </tr>
                                      </table>  
                                    </div>
                                    <div style="width: 570px;clear: both;">
                                        <asp:Panel BorderColor="#1fb6e7"  CssClass="SayFriendPanel FloatRight" BorderStyle="solid" 
                                        BorderWidth="2px" 
                                        Height="100px" ScrollBars="Vertical" Width="300px" runat="server" Visible="false" 
                                        ID="WhatMyFriendsDidPanel"></asp:Panel>
                                    </div>
                                    <div class="topDiv" style="width: 560px; background-color: #1b1b1b; display: block; float: left; clear: both; margin-left: 5px; margin-bottom: 5px; padding-bottom: 5px;">
                                    <asp:Panel Width="560px" ID="MyFriendsPanel" runat="server">
                                    <%--<div align="right" style="padding-right: 5px; padding-bottom: 5px;">
                                        <asp:Panel Width="300px" BackColor="#000000" runat="server" ID="SearchFriendPanel" Visible="false">
                                            <div align="left">
                                                <span style="color: #1fb6e7; font-weight: bold;; font-family: Arial; font-size: 14px;">Search for a friend by username.</span>
                                                <br />
                                                <ctrl:HippoTextBox ID="FriendSearchTextBox" runat="server" TEXTBOX_WIDTH="200" LITERAL_CSS_CLASS="TextBoxLeftImage25" CSS_CLASS="TextBox25" />
                                                <br />
                                                <asp:ImageButton CssClass="NavBarImage" ID="SearchButton" runat="server" OnClick="FriendSearch" CommandArgument="A" ImageUrl="~/image/SearchButton.png"  onmouseout="this.src='image/SearchButton.png'" onmouseover="this.src='image/SearchButtonSelected.png'"  />
                                                <asp:LinkButton ID="LinkButton1" CssClass="AddLink" OnClick="CancelFriendSearch" runat="server" Text="Cancel"></asp:LinkButton>
                                                <asp:Label runat="server" ID="FriendMessageLabel"></asp:Label>
                                                <asp:Panel runat="server" ID="SearchResultsPanel"></asp:Panel>
                                            </div>
                                        </asp:Panel>
                                        
                                                 
                                    </div>--%>
                                </asp:Panel>
                                </div>
                                    </div>
                                    </div>
                                </asp:Panel>
                                
                                    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab3" runat="server" >
                            <a id="pfs"></a>
                                <div style="height: 45px; margin-top: 93px; background-color: #1b1b1b; width: 7px; float: left;"></div>
                                <div style="border: solid 3px #1b1b1b; margin-top: 1px; float: left; width: 570px;">
                                    <div style="width: 560px; display: block; float: left; margin-left: 5px;">
                                        <div class="EventDiv">
                                            <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                                        </div><asp:Button runat="server" ID="Button1" Text="Save Changes" CssClass="SearchButton" OnClick="Save"
onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
 onclientclick="this.value = 'Working...';" 
 /></div>
                                        <div class="Prefs EventDiv" style="width: 560px; display: block; float: left; margin-left: 5px; padding-top: 5px;">
                                             <div class="topDiv">
                                            <div style="float:left; width: 380px ;height: 171px;background-image: url('image/FriendBackground.png'); background-repeat: no-repeat;">
                                                <div align="center" style="width: 152px; height: 152px;vertical-align: middle;padding-left: 10px; padding-top: 10px; float: left;">
                                                    <table cellpadding="0" bgcolor="#666666" cellspacing="0" width="100%" height="100%">
                                                        <tr>
                                                            <td valign="middle" align="center">
                                                                <asp:Image runat="server" ID="FriendImage" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </div>
                                                
                                                <div style="float: left; padding-left: 10px; padding-top: 10px;  padding-right:40px;" class="FriendDiv">
                                                    <table><%--
                                                        <tr>
                                                            <td>
                                                                <label>Age</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b" ID="AgeTextBox" Width="80px" />
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td>
                                                                <label>Sex</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="SexTextBox" Width="80px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>Location</label>
                                                            </td>
                                                            <td>
                                                               <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="LocationTextBox" Width="80px" />
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>


                                                   <div>
                                                                <label>Events Posted</label>
                                                            
                                                               <asp:Label CssClass="AddLinkBig" runat="server" ID="EventsLabel"></asp:Label>
                                                    </div>
                                                   <div>
                                                                <label>Events Attended</label>
                                                           
                                                               <asp:Label CssClass="AddLinkBig" runat="server" ID="AttendedLabel"></asp:Label>
                                                   </div>
                                               
                                  
                                                </div>
                                            </div>
                                            </div>
                                            <div class="Pad">
                                                <br />
                                                <asp:Label ID="Label7" runat="server" Text="Calendar Preferences" CssClass="PreferencesTitle">
                                                    Upload a new Photo</asp:Label><br />
                                                <div style="padding-left: 20px;" class="topDiv">
                                                    <div style="float: left;">
                                                        <asp:FileUpload runat="server" ID="PictureUpload" EnableViewState="true" />
                                                    </div>
                                                    <div style="margin-top: -5px; float: left;">
                                                    <asp:Button runat="server" ID="Button4" Text="Upload!" CssClass="SearchButton" OnClick="UploadPhoto"
                                                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                                        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                                        <%--<asp:ImageButton runat="server" PostBackUrl="#top" ID="AdsLink" ImageUrl="~/image/MakeItSoButton.png" 
                                                        OnClick="UploadPhoto" onmouseout="this.src='image/MakeItSoButton.png'" 
                                                        onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="Pad">
                                                <asp:Label ID="Label8" runat="server" Text="Weekly Newsletter" CssClass="PreferencesTitleAlt"></asp:Label>
                                                <br /><br />
                                                <asp:CheckBox id="WeeklyCheckBox" runat="server" Text="I want to get a weekly newsletter with updates to the site and event recommendations" />
                                            </div>
                                            <div style="border-bottom: dotted 1px black;">
                                                <asp:Label ID="Label12" runat="server" Text="Name" CssClass="PreferencesTitle"></asp:Label><span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;"> (This name is only used when your friends are trying to find you on the site. You need to provide both first and last name for the search to find you. You don't have to include it if you don't want anyone to find you by your real name. People will still be able to search for you by your user name.)</span>
                                            </div>
                                             <div style="clear: both;"  class="Pad">
                                                        <br /><label> First Name</label> <br />
                                                        <ctrl:HippoTextBox runat="server" ID="FirstNameTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                                                        <br /><label> Last Name</label> <br />
                                                        <ctrl:HippoTextBox runat="server" ID="LastNameTextBox" TEXTBOX_WIDTH="150" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />

                                            </div>

                                            <div style="border-bottom: dotted 1px black;">
                                                <asp:Label ID="Label11" runat="server" Text="Filter Location" CssClass="PreferencesTitleAlt"></asp:Label>
                                                <span style="font-style: italic; font-family: Arial; font-size: 14px; color: ##000000;"> (Events and Ads on 
                                                this site are filtered base on this preference. You will still be able to search on everything else, 
                                                however, your recommendations and the ads you see will be base on this preference. If you do not 
                                                include a zip code you will get ads in the city you choose. However, you can still receive ads not 
                                                in your zip code if there is not enough for the zip code preference you choose.)</span>
                                            </div>
                                            <div style="clear: both;"  class="Pad">
                                                <table>
                                                     <tr>
                                                        <td colspan="4">
                                                            <label style="padding-right: 10px;">*Country</label><br />
                                                            <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            <label>State</label>
                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <label>City</label><br />
                                                                <ctrl:HippoTextBox ID="CityTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                        </td>
                                                        <td>
                                                            <label>Zip</label><br />
                                                                <ctrl:HippoTextBox ID="CatZipTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                        </td>
                                                        <td>
                                                        <asp:Panel runat="server" ID="RadiusPanel">
                                                        <label>Radius</label><br />
                                                        <asp:DropDownList runat="server" ID="RadiusDropDown">
                                                            <asp:ListItem Value="0" Text="Just Zip Code (0 miles)"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="1 mile"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="5 miles"></asp:ListItem>
                                                            <asp:ListItem Value="10" Text="10 miles"></asp:ListItem>
                                                            <asp:ListItem Value="15" Text="15 miles"></asp:ListItem>
                                                            <asp:ListItem Value="30" Text="30 miles"></asp:ListItem>
                                                            <asp:ListItem Value="50" Text="50 miles"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="border-bottom: dotted 1px black;">
                                                <asp:Label ID="Label1" runat="server" Text="Profile Preferences" CssClass="PreferencesTitle"></asp:Label>
                                            </div>
                                            <div style="clear: both;"  class="Pad">
                                                <div style="float:left;">
                                                    <asp:RadioButtonList runat="server" ID="PublicPrivateCheckList">
                                                        <asp:ListItem Value="1">Make Public <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(others will be able to see if you are going to an event and see your full profile)</span></asp:ListItem>
                                                        <asp:ListItem Value="2">Show Only To Friends</asp:ListItem>
                                                        <asp:ListItem Value="3">Make Private</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label2" runat="server" Text="Communication Preferences" CssClass="PreferencesTitleAlt"></asp:Label>
                                            </div>
                                            <div  class="Pad">
                                                <asp:RadioButtonList runat="server" ID="CommunicationPrefsRadioList">
                                                    <asp:ListItem Value="1">On for Everyone <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(this means users can contact you to carpool, etc. when you have added an event to your calendar. Great choice for meeting new friends!)</span></asp:ListItem>
                                                    <asp:ListItem Value="2">I Will Set Per Event</asp:ListItem>
                                                    <asp:ListItem Value="3">On Only for Friends</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <telerik:RadPanelBar CssClass="VenueCheckBoxes" EnableAjaxSkinRendering="false" Width="560px"  
                                            AllowCollapseAllItems="true"  runat="server" ID="AdCategoryRadPanel">
                                                    <Items>
                                                        <telerik:RadPanelItem SelectedCssClass="" Text="<div style='cursor: pointer;'><label style='cursor:pointer;' ID='Label12' class='PreferencesTitle'>Ad Category Interests <span style='font-style: italic; font-family: Arial; font-size: 14px; color: #cccccc;'>(Ads throughout the site will be filtered based on the categories you select here.)</span></label></div>" Height="60px" CssClass="VenueCheckBoxes" Expanded="false">
                                                            <Items>
                                                            <telerik:RadPanelItem CssClass="VenueCheckBoxes" 
                                                             runat="server">
                                                            <ItemTemplate>
                                                                
                                            <div  class="Pad">
                                                <div style="padding-left: 40px;">
                                                     <span style="width: 300px; font-family: Arial; font-size: 14px; color: #cccccc;">
                                                     *You can also receive email with featured ads matching your criteria by going to 
                                                     <a class='AddLin' href='AdSearch.aspx'>Ads & Classifieds</a> and creating a saved search.</span><br /><br />
                                                     <span style="width: 300px; font-family: Arial; font-size: 14px; color: #cccccc;">
                                                     *Use this option to turn on/off your category preferences while still keeping them saved here.</span>
                                                    <asp:RadioButtonList runat="server" ID="CategoriesOnOffRadioList" RepeatDirection="horizontal">
                                                        <asp:ListItem Value="1" Selected="true">On&nbsp;&nbsp;</asp:ListItem>
                                                        <asp:ListItem Value="2">Off </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                <table height="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                                                    
                                                                                    </asp:CheckBoxList>--%>
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="CategoryTree" DataFieldID="ID" ForeColor="#cccccc" 
                                                                                        Font-Size="14px" DataSourceID="SqlDataSource2" 
                                                                                        DataFieldParentID="ParentID"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID <= 99 ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                                    
                                                                               
                                                                            </td>
                                                                           
                                                                            <td valign="top">
                                                                              
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="RadTreeView2" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  DataSourceID="SqlDataSource3" 
                                                                                        DataFieldParentID="ParentID"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID > 99 ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                            </td>
                                                                             </tr>
                                                                            <%--<tr>
                                                                            <td valign="top">
                                                                                <div style=" width: 280px;">
                                                                                    <rad:RadTreeView Width="280px"  runat="server"  
                                                                                    ID="RadTreeView1" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  DataSourceID="SqlDataSource4" 
                                                                                    DataFieldParentID="ParentID"
                                                                                    CheckBoxes="true">
                                                                                    <DataBindings>
                                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                     </DataBindings>
                                                                                    </rad:RadTreeView>
                                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                                                        SelectCommand="SELECT * FROM AdCategories WHERE  ID <= 99 AND ID > 24"
                                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                    </asp:SqlDataSource>
                                                                                </div>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <div style=" width: 280px;">
                                                                                    <rad:RadTreeView Width="280px" runat="server"  
                                                                                    ID="RadTreeView3" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  
                                                                                    DataSourceID="SqlDataSource5" 
                                                                                    DataFieldParentID="ParentID"
                                                                                    CheckBoxes="true">
                                                                                    <DataBindings>
                                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                     </DataBindings>
                                                                                    </rad:RadTreeView>
                                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource5" 
                                                                                        SelectCommand="SELECT * FROM AdCategories WHERE ID > 99"
                                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                    </asp:SqlDataSource>
                                                                                </div>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>            </div>
                                                            </ItemTemplate>
                                                            </telerik:RadPanelItem></Items>
                                                            
                                                        </telerik:RadPanelItem>
                                                    </Items>
                                                    
                                                </telerik:RadPanelBar>
                                           
                                            
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <div  class="Pad">
                                            <telerik:RadPanelBar CssClass="VenueCheckBoxes" EnableAjaxSkinRendering="false" Width="560px"  
                                            AllowCollapseAllItems="true"  runat="server" ID="EventPanelBar">
                                                    <Items>
                                                        <telerik:RadPanelItem SelectedCssClass="" Text="<div style='cursor: pointer;'><label style='cursor:pointer;' ID='Label12' class='PreferencesTitleAlt'> Event Category Interests <span style='cursor: pointer;font-style: italic; font-family: Arial; font-size: 14px; color: #cccccc;'>(Event recommendations on your <b>Account Page</b> and in the <b>Footer</b> will be filtered based on the categories you select)</span></label></div>" Height="60px" CssClass="VenueCheckBoxes" Expanded="false">
                                                            <Items>
                                                            <telerik:RadPanelItem CssClass="VenueCheckBoxes" 
                                                             runat="server">
                                                            <ItemTemplate>
                                                                <table height="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                                                    
                                                                                    </asp:CheckBoxList>--%>
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px"  runat="server"  
                                                                                        ID="RadTreeView1" DataFieldID="ID" ForeColor="#cccccc" 
                                                                                        Font-Size="14px" DataSourceID="SqlDataSource4" 
                                                                                        DataFieldParentID="ParentID"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID < 26 OR (ID= 34 OR ID=35) ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                                    
                                                                               
                                                                            </td>
                                                                           
                                                                            <td valign="top">
                                                                              
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="RadTreeView3" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px" 
                                                                                         DataSourceID="SqlDataSource5" 
                                                                                        DataFieldParentID="ParentID"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource5" 
                                                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID >= 26 AND  NOT(ID= 34 OR ID=35) ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                            </td>
                                                                             </tr>
                                                                            <%--<tr>
                                                                            <td valign="top">
                                                                                <div style=" width: 280px;">
                                                                                    <rad:RadTreeView Width="280px"  runat="server"  
                                                                                    ID="RadTreeView1" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  DataSourceID="SqlDataSource4" 
                                                                                    DataFieldParentID="ParentID"
                                                                                    CheckBoxes="true">
                                                                                    <DataBindings>
                                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                     </DataBindings>
                                                                                    </rad:RadTreeView>
                                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                                                        SelectCommand="SELECT * FROM AdCategories WHERE  ID <= 99 AND ID > 24"
                                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                    </asp:SqlDataSource>
                                                                                </div>
                                                                            </td>
                                                                            <td valign="top">
                                                                                <div style=" width: 280px;">
                                                                                    <rad:RadTreeView Width="280px" runat="server"  
                                                                                    ID="RadTreeView3" DataFieldID="ID" ForeColor="#cccccc" Font-Size="14px"  
                                                                                    DataSourceID="SqlDataSource5" 
                                                                                    DataFieldParentID="ParentID"
                                                                                    CheckBoxes="true">
                                                                                    <DataBindings>
                                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                         TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                     </DataBindings>
                                                                                    </rad:RadTreeView>
                                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource5" 
                                                                                        SelectCommand="SELECT * FROM AdCategories WHERE ID > 99"
                                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                    </asp:SqlDataSource>
                                                                                </div>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>  
                                                            </ItemTemplate>
                                                            </telerik:RadPanelItem></Items>
                                                            
                                                        </telerik:RadPanelItem>
                                                    </Items>
                                                    
                                                </telerik:RadPanelBar>
                                                
                                          </div>
                                           <%-- <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label3" runat="server" CssClass="PreferencesTitle">  </asp:Label>
                                            </div>--%>
                                            <div  class="Pad">
                                                <telerik:RadPanelBar CssClass="VenueCheckBoxes" EnableAjaxSkinRendering="false" Width="560px"  AllowCollapseAllItems="true"  runat="server" ID="VenuesRadPanel">
                                                    <Items>
                                                        <telerik:RadPanelItem SelectedCssClass="" Height="60px" CssClass="VenueCheckBoxes" Expanded="false">
                                                            <Items><telerik:RadPanelItem CssClass="VenueCheckBoxes" runat="server"></telerik:RadPanelItem></Items>
                                                        </telerik:RadPanelItem>
                                                    </Items>
                                                    
                                                </telerik:RadPanelBar>
                                            </div>
                                            
                                            <div style="border-bottom: dotted 1px black; clear: both;">
                                                <asp:Label ID="Label3" runat="server" CssClass="PreferencesTitle"> Event Recommendations Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(Show recommend events in the <b>User Page</b> and in the <b>Footer</b> when they are ...)</span></asp:Label>
                                            </div>
                                            <div  class="Pad">
                                                <asp:CheckBoxList runat="server" ID="RecommendationsCheckList">
                                                    <asp:ListItem Value="1">in a Favorite Venue</asp:ListItem>
                                                    <asp:ListItem Value="2">in a Favorite Category (only in your specified Filter Location above)</asp:ListItem>
                                                    <asp:ListItem Value="3">Similar to events in your calendar</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                            
                                            <div style="border-bottom: dotted 1px black; clear: both;">
                                                <asp:Label ID="Label4" runat="server" CssClass="PreferencesTitleAlt"> Billing Address <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(this is only used for purchased ads on the site)</span></asp:Label>
                                            </div>
                                            <div  class="Pad">
                                                <table>
                                                    <tr>
                                                        <td><label>Address</label></td>
                                                        <td  style="padding-left: 20px;" colspan="2">
                                                            <label>Country</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <ctrl:HippoTextBox runat="server" ID="AddressTextBox" TEXTBOX_WIDTH="259" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                            
                                                        </td>
                                                        <td  style="padding-left: 20px;" colspan="2">
                                                            <asp:DropDownList OnSelectedIndexChanged="ChangeBillState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="BillCountryDropDown"></asp:DropDownList>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" SelectCommand="SELECT * FROM Countries"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>City</label>
                                                        </td>
                                                        <td  style="padding-left: 20px;">
                                                            <label>ZIP</label>
                                                        </td>
                                                        <td>
                                                            <label>State</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <ctrl:HippoTextBox runat="server" ID="BillCityTextBox" TEXTBOX_WIDTH="259" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                        </td>
                                                        <td  style="padding-left: 20px;">
                                                            <ctrl:HippoTextBox runat="server" ID="ZipTextBox" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /><br />
                                                        </td>
                                                        <td>
                                                            <asp:Panel runat="server" ID="BillStateTextPanel">
                                                                <ctrl:HippoTextBox ID="BillStateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="BillStateDropPanel" Visible="false">
                                                                <asp:DropDownList runat="server" ID="BillStateDropDown"></asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                               

                                            </div>
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label5" runat="server" CssClass="PreferencesTitle"> Phone Number & Provider <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(this is only used for texting event info)</span></asp:Label>
                                            </div>
                                            <div  class="Pad">
                                                <table>
                                                    <tr>
                                                        <td><label>Phone Number</label></td>
                                                        <td  style="padding-left: 20px;">
                                                            <label>Provider</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <ctrl:HippoTextBox runat="server" ID="PhoneTextBox" TEXTBOX_WIDTH="259" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                            
                                                        </td>
                                                        <td  style="padding-left: 20px;">
                                                            <asp:DropDownList runat="server" ID="ProviderDropDown"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                
                                                </table>
                                               

                                            </div>
                                            <div style="display: block;">
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label6" runat="server" CssClass="PreferencesTitleAlt"> Texting Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(send notifications to your phone when...)</span></asp:Label>
                                            </div>
                                            <div  class="Pad">
                                                <asp:CheckBoxList runat="server" ID="TextingCheckBoxList">
                                                    <asp:ListItem Value="1">When events in any of your categories have been added to the site.</asp:ListItem>
                                                    <asp:ListItem Value="2">When one of your friends has added an event to their calendar.</asp:ListItem>
                                                    <asp:ListItem Value="3">When an event is at a favorite venue.</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label10" runat="server" CssClass="PreferencesTitle"> Email Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(send notifications to your email address when...)</span></asp:Label>
                                            </div>

                                            
                                            <div  class="Pad">
                                                                <label>Email</label>
                                          
                                               <asp:TextBox runat="server" BackColor="#666666" BorderColor="#1b1b1b"  ID="EmailTextBox" Width="200px" />
                                                <br /><br />
                                                <label>Automatic Emails</label>
                                                <asp:CheckBoxList runat="server" ID="EmailCheckList3">
                                                    <asp:ListItem Value="C" Text="When an event in your calendar has been updated"></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:CheckBoxList runat="server" ID="EmailCheckList">
                                                
                                                    <asp:ListItem Value="1">When events in any of your categories have been added.</asp:ListItem>
                                                    <asp:ListItem Value="2">When one of your friends has added an event.</asp:ListItem>
                                                    <asp:ListItem Value="3">When an event is at a favorite venue.</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                                <label>User Generated Emails  <span style=" font-family: Arial; font-size: 11px; color: #ff6b09;">(applied to all users. to turn preferences on per friend, un-check these preferences and go to My Friends tab.)</span></label>
                                                
                                                <asp:CheckBoxList runat="server" ID="EmailUserCheckList1">
                                                    <asp:ListItem Value="4" Text="When someone replies to your Hippo Mail."></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="When someone sends a friend request to you."></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="When someone accepts your friend request."></asp:ListItem>
                                                    
                                                </asp:CheckBoxList>
                                                <asp:CheckBoxList runat="server" ID="EmailUserCheckList2">
                                                    <asp:ListItem Value="6" Text="When someone contacts you about an event."></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="When someone sends you a Hippo Mail through your profile."></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="When a friend shares an event/venue/ad with you."></asp:ListItem>
                                                </asp:CheckBoxList>
                                                
                                            </div>
                                            <div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label9" runat="server" CssClass="PreferencesTitleAlt"> Comments Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(The comment list for events, venues, etc. can get long. You can decrease it's size by selecting to only view your friends' comments.)</span></asp:Label>
                                            </div>
                                            <div style="clear: both;"  class="Pad">
                                                <div style="float:left;">
                                                    <asp:RadioButtonList runat="server" ID="CommentsRadioList">
                                                        <asp:ListItem Value="1">See Everyone's </asp:ListItem>
                                                        <asp:ListItem Value="2">See Only Friends'</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                           
                                            <%--<div style="border-bottom: dotted 1px black; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label8" runat="server" CssClass="PreferencesTitle"> Poll Answer Preferences <span style="font-style: italic; font-family: Arial; font-size: 14px; color: #000000;">(Answers to a poll could be a huge list. You can decrease it's size by selecting to only view your friends' answers.)</span></asp:Label>
                                            </div>
                                            <div style="clear: both;"  class="Pad">
                                                <div style="float:left;">
                                                    <asp:RadioButtonList runat="server" ID="PollRadioList">
                                                        <asp:ListItem Value="1">See Everyone's </asp:ListItem>
                                                        <asp:ListItem Value="2">See Only Friends'</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>--%>
                                            </div>
                                            <div style="width: 560px; padding-top: 20px; display: block;" align="right">
                                                <asp:Button runat="server" ID="SearchButton" 
                                                    Text="Save Changes" CssClass="SearchButton" OnClick="Save"
                                                    onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                    onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                    onclientclick="this.value = 'Working...';" />
                                            </div>
                                        </div>
                                </div>
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab4" runat="server" >
                            <a id="groups"></a>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                    <ContentTemplate>
                                    <asp:LinkButton runat="server" ID="DoFillGroups" OnClick="AfterGroupDelete"></asp:LinkButton>
                                    <div style="height: 45px; margin-top: 139px; background-color: #1b1b1b; 
                                    width: 7px; float: left;"></div>
                                    <div class="EventDiv" style="padding: 10px;border: solid 3px #1b1b1b; margin-top: 1px; float: left; width: 550px;">
                                        <asp:Panel runat="server" ID="GroupsPanel"></asp:Panel>
                                    </div>
                                    <asp:Label runat="server" ID="GroupsErrorLabel" ForeColor="red"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </rad:RadPageView>
                        </rad:RadMultiPage>

            </td>
        </tr>
   </table>
    
   
   </div>
    </ContentTemplate>
   </asp:UpdatePanel>
    <div class="topDiv">
        <div style="display: block;">
            
            
        </div>
        
    </div>
    
    <br />
    <div style="width: 860px; padding-right: 5px; font-family: Arial; font-size: 12px; 
    font-weight: bold; color: #1fb6e7;" align="right">
        <asp:Label runat="server" ID="MessageLabel"></asp:Label>
    </div>
    <br />
</div>  
 <%-- </rad:RadAjaxPanel>--%>
</asp:Content>

