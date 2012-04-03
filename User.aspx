<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" 
CodeFile="User.aspx.cs" 
Inherits="User" Title="Your HippoHappenings Account" EnableEventValidation="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/UserMessage.ascx" TagName="UserMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/GlanceDay.ascx" TagName="GlanceDay" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Pager.ascx" TagPrefix="ctrl" TagName="Pager" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%--<rad:RadAjaxPanel runat="server">--%>
<script type="text/javascript">
    function OpenAdCategories()
        {
            var d = document.getElementById('AdDiv');
            var c = document.getElementById('OpenAdDiv');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&rArr;';
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&dArr;';
            }
        }
        
        function OpenEventCategories()
        {
            var d = document.getElementById('EventDiv');
            var c = document.getElementById('Span1');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&rArr;';
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&dArr;';
            }
        }
        
        function OpenVenueCategories()
        {
            var d = document.getElementById('VenueDiv');
            var c = document.getElementById('Span2');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&rArr;';
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&dArr;';
            }
        }
</script>
<div class="Text12 topDiv" style="margin-left: -10px;">
    <rad:RadCodeBlock runat="server">
        
        <script type="text/javascript">
        function OpenPoints()
        {
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('HippoPointsEdit.aspx');
            win.show(); 
            win.center(); 
        }
        
        var myArray = [];
        var count = 0;
        var hasIt = false;
        function MakeUnBold(theID)
        {
            hasIt = false;
            var theI = document.getElementById(theID);
            theI.style.color = '#535252';
            theI.style.fontWeight = 'normal';
            
            var inboxInnerDiv = document.getElementById("InboxInnerDiv");
            var inboxDiv = document.getElementById("InboxDiv");
            
            for(var j=0;j<count;j++)
            {
                if(myArray[j] == theID)
                {
                    hasIt = true;
                    break;
                }
            }
            
            if(!hasIt)
            {
                myArray[count] = theID;
                count++;
                var i = parseInt(inboxInnerDiv.innerHTML);
                if(i > 1)
                    inboxInnerDiv.innerHTML = (i - 1).toString();
                else
                    inboxDiv.innerHTML = '';
                    
                return true;
            }
            else
            {
                return false;
            }
        }
        function CheckCheck(checkID)
        {
            var theCheck = document.getElementById(checkID);
            if(theCheck.checked)
            {
                theCheck.checked = false;
            }
            else
            {
                theCheck.checked = true;
            }
        }
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
                theDiv.style.color = '#535252';
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
        
        function OpenRemove(theID)
        {
            var win = $find("<%=RadWindow3.ClientID %>");
            win.setUrl('RemoveFriend.aspx?ID='+theID);
            win.show(); 
            win.center(); 
               
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
//                message.innerHTML = 'Your message could not be sent. Please <a class="AddGreenLink" href="contact-us">contact us</a> for assistance!';
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
         function CallBackFunctionOwner(radWindow, returnValue)
                {
                    
                    if(returnValue != null && returnValue != undefined)
                        window.location = returnValue;
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
                 VisibleTitlebar="false"
                VisibleOnPageLoad="false" Height="500" ClientCallBackFunction="OnClientClose" Width="780px" 
                Skin="Web20"
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
                <rad:RadWindow ID="RadWindow2" DestroyOnClose="false" VisibleStatusbar="false"
                 BorderStyle="Solid"
                 VisibleTitlebar="false"
                VisibleOnPageLoad="false" Height="500" ClientCallBackFunction="OnClientClose" Width="550px" 
                Skin="Web20"
                Title="Send event information to your friends"
                     runat="server">
                </rad:RadWindow>
            </Windows>
   </rad:RadWindowManager>
   <telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false" Skin="Web20" 
                BorderStyle="solid"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <telerik:RadWindow Width="300" ClientCallBackFunction="OnClientClose" 
                    EnableEmbeddedSkins="true" VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Web20" Height="200" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                    <telerik:RadWindow Width="300" ClientCallBackFunction="CallBackFunctionOwner" 
                    EnableEmbeddedSkins="true" 
                    VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Web20" Height="200" ID="RadWindow3" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        <div style="float: left; ">
            <div class="topDiv"><%--<asp:Button runat="server" ID="sendwelcomebutton" OnClick="SendWelcome" Text="send it" />--%>
                <div style="float: left;margin-top: -5px;margin-bottom: 5px;">
                    <h1 style="margin: 0; padding-bottom: 0px;">Welcome <asp:Label runat="server" ID="UserLabel"></asp:Label>!</h1>
                </div>
            </div>
                                <asp:Label runat="server" ForeColor="Red" ID="UserErrorLabel"></asp:Label>     
            <%--<h1 class="SideColumn">Your Status & Badges</h1>
            <div class="Text12">
                <div style="float: left;">
                    
                    <div class="topDiv" style="border: solid 1px #dedbdb; padding: 10px; padding-top: 5px; width: 361px;margin-right: 9px;">
                        <div style="clear: both; ">
                            <div style="width:169px;float: left;">
                                <h2>You Have Posted</h2>
                                <asp:Label runat="server" ID="NumEventsLabel"></asp:Label>
                                
                            </div>
                            <div style="float: left;width: 187px;padding-left: 4px;border-left: solid 1px #dedbdb;">
                                <h2>You Are Currently In</h2>
                                <div style="clear: both;padding-left: 5px;">
                                    <asp:Panel runat="server" ID="EnteredPanel" Visible="false">
                                        <div style="clear: both;padding-bottom: 10px;">
                                            <asp:Label runat="server" ID="PlaceLabel"></asp:Label>
                                        </div>
                                        <div style="clear: both;padding-bottom: 10px;">
                                            <asp:Label runat="server" ID="FinalRatingLabel"></asp:Label>
                                        </div>
                                        <div style="clear: both;">
                                            <div class="topDiv">
                                                <div style="float: left; width: 168px;">
                                                    <span class='Asterisk'>*</span>You are entered in the Hippo Points.
                                                        <asp:Label runat="server" ID="EnteredEditLabel" Text="<span onclick='OpenPoints();' class='NavyLink12UD'><b>Edit Your Info</b></span>"></asp:Label>
                                                        <asp:Label runat="server" ID="EnteredNotEditLabel" Text="<b>You cannot edit your information at this moment as we are currently calculating the winners of this months contest. This process should take less than an hour</b>"></asp:Label>.
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="NotEnteredPanelOn" Visible="false">
                                        You are currently <b>NOT ENTERED</b> for Hippo Points. You are <b>missing out</b> on a <b>totally free, complimentary</b> chance to be featured on our home page for a full month.
                                        <span onclick="OpenPoints();" class="NavyLink12">Edit your Hippo Points status here</span>.
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="NotEnteredPanelOff" Visible="false">
                                        You are currently <b>NOT ENTERED</b> for Hippo Points. You are <b>missing out</b> on a <b>totally free, complimentary</b> chance to be featured on our home page for a full month.
                                        <b>You cannot enter into Hippo Points at this moment as we are currently 
                                        calculating the winners of this months contest. This process should take less than an hour.</b>
                                    </asp:Panel>
                                </div>
                                
                            </div>
                        </div>
                        <div style="clear: both; padding-top: 10px;">
                            <h2>Your Badges Wall</h2>
                            <asp:Panel runat="server" ID="BadgesPanel"></asp:Panel>
                        </div>
                    </div>
                </div>
                <div style="float: left;">
                    <div class="topDiv" style="border: solid 1px #dedbdb; padding: 10px; padding-top: 5px; width: 416px;">
                        <h2>Your Resources</h2>
                        <div style="padding-right: 10px;float: left;padding-left: 30px;">
                            <asp:HyperLink CssClass="NormalLink" runat="server" ID="CalendarLink">Your Calendar</asp:HyperLink>
                        </div>
                        <div style="float: left;">   
                            <a class="NormalLink" href="SearchesAndPages.aspx">Your Posts & Saved Searches</a>
                        </div>
                    </div>
                    <div class="topDiv" style="padding-top: 10px; padding-bottom: 5px; padding-left: 15px;">
                        <ctrl:GlanceDay ID="GlanceDay1" NUM_OF_EVENTS="0" THE_DAY="Sun" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay2" NUM_OF_EVENTS="1" THE_DAY="Mon" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay3" NUM_OF_EVENTS="0" THE_DAY="Tues" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay4" NUM_OF_EVENTS="0" THE_DAY="Wed" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay5" NUM_OF_EVENTS="1" THE_DAY="Thurs" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay6" NUM_OF_EVENTS="0" THE_DAY="Fri" runat="server" />
                        <ctrl:GlanceDay ID="GlanceDay7" NUM_OF_EVENTS="0" THE_DAY="Sat" runat="server" />
                    </div>
                    <div style="float: left;">
                        <div style="padding-top: 5px;float: left;padding-left: 10px;">
                            <ctrl:Ads ID="Ads1" THE_WIDTH="444" runat="server" />
                            <div style="float: right;padding-right: 10px;">
                                <a class="NavyLinkSmall" href="post-bulletin">+Add Bulletin</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div>
         </div>   
        </div>
    </div>
    



   <div style="width: 900px; display: block; padding-top: 20px;" >

   <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <rad:RadTabStrip Width="280px" Orientation="VerticalLeft" Align="right" 
                            ID="RadTabStrip2" Skin="Black"
                             runat="server" MultiPageID="TheMultipage"
                               style="float:left">
                                <Tabs>
                                    <rad:RadTab CssClass="MyTabsMessages" Width="280px" Text="friend updates"
                                        TabIndex="0" Selected="true"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsMessages" Text="my messages" Width="280px" 
                                        TabIndex="1" Selected="true"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsMessages" Width="280px" Text="my friends" TabIndex="2"></rad:RadTab>
                                    <rad:RadTab CssClass="MyTabsMessages" Width="280px" Text="my preferences"  TabIndex="3"></rad:RadTab>
                                </Tabs>
                            </rad:RadTabStrip>
                        </td>
                     </tr>
                     <tr>   
                        <td valign="top">
                              <div class="topDiv" style="padding-top: 20px;">
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
                                        <rad:RadPageView runat="server" BorderColor="#dedbdb" BorderStyle="solid" BorderWidth="1px"  Width="280px" ID="RadPageView1" TabIndex="0">
                                        <asp:UpdatePanel ID="RecomUpdate" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        <asp:Panel runat="server" ID="RecommendedEvents" Wrap="true" Width="260px" 
                                        CssClass="FavoritesPanel"></asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </rad:RadPageView>
                                    <rad:RadPageView runat="server" BorderColor="#dedbdb" BorderStyle="solid" BorderWidth="1px" Width="280px" ID="RadPageView2" TabIndex="1">
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
                            <rad:RadPageView runat="server" TabIndex="0">
                                <div style="position: relative; margin-right: -1px;margin-left: -2px;height: 45px;border-top: solid 1px #dedbdb;border-bottom: solid 1px #dedbdb; margin-top: 1px; background-color: white; width: 7px; float: left;"></div>
                                <div style="float: left; width: 545px;margin-top: 1px;"> 
                                   <a id="middle"></a>
                                   <div id="HelloDiv" style="font-size: 20px; color: White; display: block;"></div>
                                   <div class="topDiv" style="padding-top: 5px;border: solid 1px #dedbdb;">
                                   
                                        <div align="center" style=" padding-left: 15px; padding-bottom: 20px;">
                                            <h1 class="SideColumn">Look what your friends have been up to!</h1>
                                            <asp:Literal runat="server" ID="PostingLiteral"></asp:Literal>
                                        </div>
                                   </div>
                                </div>
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab1" TabIndex="1" runat="server">
                               <script type="text/javascript" language="javascript">
                                        function ShowLoadingCalendar()
                                        {
                                            var theDiv = document.getElementById("MessagesDiv");
                                            var theDiv2 = document.getElementById("innerRight");
                                            var theDiv3 = document.getElementById("innerLoading2");
                                            
                                            if(theDiv.offsetHeight > 200)
                                            {
                                                theDiv2.style.height = theDiv.offsetHeight + 'px';
                                                theDiv3.style.height = theDiv.offsetHeight + 'px';
                                            }
                                            
                                            
                                            var theDiv4 = document.getElementById("innerBottom");
                                            
                                            if(theDiv.offsetHeight > 170)
                                            {
                                                theDiv4.style.height = (theDiv.offsetHeight - 150) + 'px';
                                            }
                                            
                                            var d = document.getElementById('Div1');
                                            d.style.display = 'block';
                                        }
                                    </script>
                                
                                <div style="top: 48px;position: relative; margin-right: -1px; margin-left: -2px;height: 45px;border-top: solid 1px #dedbdb;border-bottom: solid 1px #dedbdb; margin-top: 1px; background-color: white; width: 7px; float: left;"></div>
                                <div style="padding-left: 5px;float: left; width: 538px;margin-top: 1px;border: solid 1px #dedbdb;"> 
                                   <div style="margin-top: -10px;padding-bottom: 10px;" class="topDiv">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional" ChildrenAsTriggers="false">
                                                  
                                                    <ContentTemplate>
                                                    <script type="text/javascript">
                                                function ShowMesDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthMesDiv');
                                                    var loadDiv = document.getElementById('LoadMesDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                                function HideDiv()
                                                {
                                                    var loadDiv = document.getElementById('LoadDetailsDiv');
                                                    
                                                    loadDiv.style.display = 'none';
                                                }
                                            </script>
                                            
                                                
                                                    <asp:Label runat="server" ForeColor="Red" ID="MessagesErrorLabel"></asp:Label> 
                                                    <div style="z-index: 10000;float: right; position: relative; top: 30px;right: 6px;">
                                                    <asp:LinkButton ID="LinkButton1" OnClientClick="if(confirm('Are you sure you want to delete these messages? Deletion will happen from messages you have checked in your Inbox and Send Messages.')){ShowMesDiv();return true;}else{return false;}" CssClass="AddLink" OnClick="DeleteMessages" runat="server" Text="delete checked messages"></asp:LinkButton></div>
                                                    <asp:Label runat="server" ID="AleksLabel" ForeColor="red"></asp:Label>
                                                    <asp:Label runat="server" ForeColor="Red" ID="MessagesLabel"></asp:Label>
                                  <div class="topDiv"> 
                                  <div  id="WidthMesDiv" style="width: 540px; display: block; float: left;">
                                    <div id="LoadMesDiv" style="z-index: 10000;width: 532px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="top">
                                                                <div style="padding-top: 60px;"><img src="image/ajax-loader.gif" /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        <rad:RadTabStrip Orientation="HorizontalTop"
                                        ID="RadTabStrip3" Skin="Vista"
                                             runat="server" MultiPageID="RadMultiPage2">
                                            <Tabs>
                                                <rad:RadTab Selected="true" Text="Inbox"></rad:RadTab>
                                                <rad:RadTab Text="Sent Messages"></rad:RadTab>
                                            </Tabs>
                                        </rad:RadTabStrip><%--
                                        <div align="center" id="Div1"  style="display: none;z-index: 3000;position: absolute; width: 540px;" >
                                                <div id="innerLoading2" style="height: 200px;float: left;opacity: .5; filter: alpha(opacity=50);
                                                width: 100px; background-color: #cccccc;">
                                                </div>
                                                <div style="float: left;width: 375px;">
                                                    <div style=" float: left;opacity: .5; filter: alpha(opacity=50); background-color: #cccccc; 
                                                    width: 375px; height: 50px;"></div>
                                                    <div style="padding-top: 30px;clear: both;position: relative;top: 45; height: 70px;
                                                    background-color: #363636; opacity: 1; filter: alpha(opacity=100);">
                                                            <img src="image/ajax-loaderBig.gif" />
                                                            <span class="updateProgressMessage">Loading ...</span>
                                                    </div>
                                                    <div id="innerBottom" style="float: left;opacity: .5; filter: alpha(opacity=50); 
                                                    background-color: #cccccc; width: 375px; height: 50px;"></div>
                                                </div>
                                                <div id="innerRight" style="float: left;opacity: .5; filter: alpha(opacity=50);width: 100px; 
                                                height: 200px; background-color: #cccccc;"></div>
                                            </div>--%>
                                            <div class="topDiv" id="MessagesDiv" style="min-height: 200px;">
                                        <rad:RadMultiPage ID="RadMultiPage2" SelectedIndex="0" runat="server">
                                            <rad:RadPageView ID="RadPageView3" runat="server">
                                                
                                                <asp:Panel runat="server" ID="MessagesPanel"></asp:Panel>
                                                   
                                            </rad:RadPageView>
                                            <rad:RadPageView ID="RadPageView4" runat="server">
                                            
                                                <asp:Panel runat="server" ID="UsedMessagesPanel"></asp:Panel>
                                                 
                                            </rad:RadPageView>
                                        </rad:RadMultiPage>
                                            </div>
                                    </div>
                                                </div>   
                                                 </ContentTemplate>
                                </asp:UpdatePanel>
                                   </div>
                                </div>
                               
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab2" TabIndex="2" runat="server" >
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
                                    
                                    <ContentTemplate>
                                <asp:Panel runat="server" ID="FriendPanel">
                                    <div class="topDiv">
                                <div style="top: 96px;position: relative; margin-right: -1px; margin-left: -2px;height: 45px;border-top: solid 1px #dedbdb;border-bottom: solid 1px #dedbdb; margin-top: 1px; background-color: white; width: 7px; float: left;"></div>

                                    <div style="min-height: 200px;margin-top: 1px;border: solid 1px #dedbdb; margin-top: 1px; float: left; width: 543px;">
                                    <div class="Text12" style="width: 530px; display: block; float: left; clear: both; margin-left: 5px;">
                                      <table width="100%">
                                        <tr>
                                            <td>
                                                <span style="font-family: Arial; font-size: 12px; padding-left: 5px;">
                                                (<asp:Label runat="server" ID="NumFriendsLabel"></asp:Label> friends)
                                                </span>
                               
                                            </td>
                                            <td align="right" style="padding-right: 10px;">
                                              <a class="NavyLink" href="javascript:OpenRad2();">+ Invite Friends To Hippo</a> 
                                                &nbsp;&nbsp;&nbsp;<a class="NavyLink" href="javascript:OpenRad();">+ Search for Friends On Hippo</a>
                                            </td>
                                        </tr>
                                      </table>  
                                    </div>
                                    <div style="width: 530px;clear: both;">
                                        <asp:Panel BorderColor="#1fb6e7"  CssClass="SayFriendPanel FloatRight" BorderStyle="solid" 
                                        BorderWidth="2px" 
                                        Height="100px" ScrollBars="Vertical" Width="300px" runat="server" Visible="false" 
                                        ID="WhatMyFriendsDidPanel"></asp:Panel>
                                    </div>
                                    <div class="topDiv" style="width: 560px; display: block; float: left; clear: both; margin-left: 5px; margin-bottom: 5px; padding-bottom: 5px;">
                                    <asp:Panel Width="510px" ID="MyFriendsPanel" runat="server">
            
                                </asp:Panel>
                                </div>
                                    </div>
                                    </div>
                                </asp:Panel>
                                
                                    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </rad:RadPageView>
                            <rad:RadPageView ID="Tab3" TabIndex="3" runat="server" >
                            <a id="pfs"></a>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="UploadButton"  />
                                </Triggers>
                                <ContentTemplate>
                                <div style="z-index: 10000;top: 144px;position: relative; margin-right: -1px; margin-left: -2px;height: 45px;border-top: solid 1px #dedbdb;border-bottom: solid 1px #dedbdb; margin-top: 1px; background-color: white; width: 7px; float: left;"></div>
                                    <div style="z-index: 1;position: relative;float: left;">
                                        <script type="text/javascript">
                                                function ShowPrefDiv()
                                                {
                                                    var theDiv = document.getElementById('WidthPrefDiv');
                                                    var loadDiv = document.getElementById('LoadPrefDiv');
                                                    
                                                    loadDiv.style.height = theDiv.offsetHeight +'px';
                                                    loadDiv.style.display = 'block';
                                                    
                                                    return true;
                                                }
                                            </script>
                                    <div id="LoadPrefDiv" style="z-index: 10000;width: 545px;padding-top: 1px;display: none;position: absolute;">
                                                <div style="position: absolute; width: 100%; height: 100%; background-color: #09718F;opacity: .2;filter: alpha(opacity=20);"></div>
                                                <div style="position: absolute; width: 100%; height: 100%; ">
                                                    <table height="100%" width="100%" align="center">
                                                        <tr>
                                                            <td height="100%" width="100%" align="center" valign="top">
                                                                <div style="padding-top: 60px;"><img src="image/ajax-loader.gif" /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                    
                                <div class="topDiv" id="WidthPrefDiv" style="margin-top: 1px;border: solid 1px #dedbdb; margin-top: 1px; float: left; width: 543px;">
                                    <div style="display: block; float: left; margin-left: 5px; padding-top: 5px;">
                                        <div class="topDiv">
                                            <div style="float: left;padding-right: 20px;">
                                                <ctrl:BlueButton WIDTH="107px" runat="server" ID="TopSaveButton" CLIENT_LINK_CLICK="ShowPrefDiv()" BUTTON_TEXT="Save Changes" />
                                            </div>
                                            <div class="EventDiv" style="float: left;padding-top: 5px;">
                                                <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        </div>
                                        <div class="Prefs EventDiv" style="width: 534px; display: block; float: left; margin-left: 5px; padding-top: 5px;">
                                             <div class="topDiv">
                                            <div style="float:right; width: 380px; height: 171px; border: solid 1px #dedbdb;">
                                                <div align="center" style="width: 152px; height: 152px;vertical-align: middle;padding-left: 10px; padding-top: 10px; float: left;">
                                                    <table cellpadding="0" cellspacing="0" width="100%" height="100%">
                                                        <tr>
                                                            <td valign="middle" align="center" style="border: solid 1px #dedbdb;">
                                                                <asp:Image runat="server" ID="FriendImage" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                
                                                <div style="float: left; padding-left: 10px; padding-top: 10px;  padding-right:40px;">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-bottom: 5px;">
                                                                <label>Sex</label>
                                                            </td>
                                                            <td style="padding-bottom: 5px;">
                                                                <asp:TextBox runat="server" ID="SexTextBox" Width="80px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-right: 5px;">
                                                                <label>Location</label>
                                                            </td>
                                                            <td>
                                                               <asp:TextBox runat="server" ID="LocationTextBox" Width="80px" />
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="Pad" style="width: 380px; float: right;clear: both;">
                                                <br />
                                                <asp:Label ID="Label7" runat="server" Text="Calendar Preferences" CssClass="PreferencesTitle">
                                                    Upload a new Photo</asp:Label><br />
                                                <div style="padding-left: 20px;" class="topDiv">
                                                    <div style="float: left;">
                                                        <asp:FileUpload runat="server" ID="PictureUpload" EnableViewState="true" />
                                                    </div>
                                                    <div style=" float: left;margin-top: -3px;padding-left: 5px;">
                                                    <ctrl:BlueButton CLIENT_LINK_CLICK="ShowPrefDiv()" runat="server" WIDTH="100px" ID="UploadButton" BUTTON_TEXT="Upload" />
                                                    
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="Pad" style="clear: both;">
                                                <div style="border-bottom: solid 1px #dedbdb;margin-bottom: 10px;">
                                                    <asp:Label ID="Label8" runat="server" Text="Weekly Newsletter" CssClass="PreferencesTitleAlt"></asp:Label>
                                                </div>
                                                <asp:CheckBox id="WeeklyCheckBox" runat="server" Text="I want to get a weekly newsletter with updates to the site and event recommendations" />
                                            </div>
                                            <div style="border-bottom: solid 1px #dedbdb;">
                                                <asp:Label ID="Label12" runat="server" Text="Name" CssClass="PreferencesTitle"></asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; "> (Help your friends find you by your name.)</span>
                                            </div>
                                             <div style="clear: both;padding-top: 5px;"  class="Pad">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <label> First Name</label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="FirstNameTextBox" WIDTH="150" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label> Last Name</label> 
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="LastNameTextBox" WIDTH="150" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div style="border-bottom: solid 1px #dedbdb;">
                                                <asp:Label ID="Label11" runat="server" Text="Location" CssClass="PreferencesTitleAlt"></asp:Label>
                                                <span style="font-style: italic; font-family: Arial; font-size: 12px;"> (Your searches will default to this location.)</span>
                                            </div>
                                            <div style="clear: both;padding-top: 5px;"  class="Pad">
                                                <table>
                                                     <tr>
                                                        <td colspan="4">
                                                            <label style="padding-right: 10px;"><span class="Asterisk">*</span>Country</label><br />
                                                            <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <label><span class="Asterisk">*</span>State</label>
                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                <asp:TextBox ID="StateTextBox" runat="server" WIDTH="100"></asp:TextBox>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                <asp:DropDownList onchange="ShowPrefDiv()" runat="server" ID="StateDropDown" OnSelectedIndexChanged="ChangeCity"></asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                        <td valign="top">
                                                            <label><span class="Asterisk">*</span>City</label><br />
                                                                <asp:TextBox ID="CityTextBox" runat="server" WIDTH="100"></asp:TextBox>
                                                        </td>
                                                        <td valign="top">
                                                            <label><span class="Asterisk">*</span>Zip</label><br />
                                                                <asp:TextBox ID="CatZipTextBox" runat="server" WIDTH="100"></asp:TextBox>
                                                               <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Panel runat="server" ID="CityDropDownPanel" Visible="false">
                                                                <label><span class="Asterisk">*</span>Major City Area</label><br />
                                                                <asp:DropDownList runat="server" ID="MajorCityDrop"></asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                               ControlToValidate="StateTextBox" ErrorMessage="State is Required"></asp:RequiredFieldValidator>
                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                               ControlToValidate="CityTextBox" ErrorMessage="City is Required"></asp:RequiredFieldValidator>
                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CatZipTextBox" ErrorMessage="Zip is Required"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ControlToValidate="CatZipTextBox" ForeColor="red" 
                                                                ErrorMessage="Zip must be a 5 digit number." runat="server" ID="ZipValidator" Type="Integer" MaximumValue="99999" MinimumValue="00000"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="border-bottom: solid 1px #dedbdb;">
                                                <asp:Label ID="Label1" runat="server" Text="Profile Preferences" CssClass="PreferencesTitle"></asp:Label>
                                            </div>
                                            <div style="clear: both;padding-top: 5px;"  class="Pad">
                                                <div style="float:left;">
                                                    <asp:RadioButtonList runat="server" ID="PublicPrivateCheckList">
                                                        <asp:ListItem Value="1">Make Public <span style="font-style: italic; font-family: Arial; font-size: 12px;">(your events and your full profile will be viewable by everyone)</span></asp:ListItem>
                                                        <asp:ListItem Value="2">Show Only To Friends</asp:ListItem>
                                                        <asp:ListItem Value="3">Make Private</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <div style="border-bottom: solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label2" runat="server" Text="Communication Preferences" CssClass="PreferencesTitleAlt"></asp:Label>
                                            </div>
                                            <div  class="Pad" style="padding-top: 5px;">
                                                <asp:RadioButtonList runat="server" ID="CommunicationPrefsRadioList">
                                                    <asp:ListItem Value="1">On for Everyone <span style="font-style: italic; font-family: Arial; font-size: 12px;">(this means users can contact you to carpool, etc. when you have added an event to your calendar. Great choice for meeting new friends!)</span></asp:ListItem>
                                                    <asp:ListItem Value="2">I Will Set Per Event</asp:ListItem>
                                                    <asp:ListItem Value="3">On Only for Friends</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div style="border-bottom: solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <%--<a id="adPfs"></a>
                                            <div style="width: 530px;padding-top: 10px;">
                                                <div style='cursor: pointer; text-decoration: underline;' 
                                                onclick="OpenAdCategories();" class='PreferencesTitle'><div 
                                                style='float: left;' ID='Label13'>Ad Category Interests <span id="OpenAdDiv">&rArr;</span></div>
                                                </div>
                                                <div id="AdDiv" style="clear: both; width: 530px;display: none;">
                                                    <div  class="Pad2">
                                                <div class="Text12 Text12" style="padding-left: 40px;padding-top: 10px;">
                                                     <span style="width: 300px; font-family: Arial; font-size: 14px;">
                                                     *You can also receive email with featured ads matching your criteria by going to 
                                                     <a class='NavyLink12' href='ad-search'>Ads & Classifieds</a> and creating a saved search.</span><br /><br />
                                                     <span style="width: 300px; font-family: Arial; font-size: 14px;">
                                                     *Use this option to turn on/off your category preferences while still keeping them saved here.</span>
                                                    <asp:RadioButtonList runat="server" ID="CategoriesOnOffRadioList" RepeatDirection="horizontal">
                                                        <asp:ListItem Value="1" Selected="true">On&nbsp;&nbsp;</asp:ListItem>
                                                        <asp:ListItem Value="2">Off </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                <table height="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="CategoryTree" DataFieldID="ID"
                                                                                        Font-Size="14px" DataSourceID="SqlDataSource2" 
                                                                                        DataFieldParentID="ParentID" Skin="Vista"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID <= 99 OR ID=221 OR ID=222 ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                                    
                                                                               
                                                                            </td>
                                                                           
                                                                            <td valign="top">
                                                                              
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="RadTreeView2" DataFieldID="ID" Font-Size="14px"  DataSourceID="SqlDataSource3" 
                                                                                        DataFieldParentID="ParentID" Skin="Vista"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                                            SelectCommand="SELECT * FROM AdCategories WHERE ID > 99 AND ID <> 221 AND ID <> 222 ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                            </td>
                                                                             </tr>
                                                                    </table>            </div>
                                                </div>
                                            </div>--%>
                                           
                                            
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <div style="width: 530px;padding-top: 10px;">
                                                <div style='cursor: pointer; text-decoration: underline;' 
                                                onclick="OpenEventCategories();" class='PreferencesTitleAlt'><div 
                                                style='float: left;' ID='Div2'>Event Recommendation Categories <span id="Span1">&rArr;</span></div>
                                                </div>
                                                <div id="EventDiv" style="clear: both; width: 530px;display: none;padding-top: 10px;">
                                            <div  class="Pad2">
                                                            <div  class="Pad2">
                                                                <table height="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px"  runat="server"  
                                                                                        ID="RadTreeView1" DataFieldID="ID"
                                                                                        Font-Size="14px" DataSourceID="SqlDataSource4" 
                                                                                        DataFieldParentID="ParentID" Skin="Vista"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID" />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" 
                                                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID < 26 OR ID=52 OR ID=53 OR ID= 34 OR ID=40 OR ID=50 OR ID=51 OR ID=35 OR ID=47 OR ID=56 OR ID=57 OR ID=58 OR ID=61 OR ID=62 ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                                    
                                                                               
                                                                            </td>
                                                                           
                                                                            <td valign="top">
                                                                              
                                                                                    <div class="FloatLeft" style="width: 275px;">
                                                                                        <rad:RadTreeView Width="275px" runat="server"  
                                                                                        ID="RadTreeView3" DataFieldID="ID" Font-Size="14px" 
                                                                                         DataSourceID="SqlDataSource5" Skin="Vista" 
                                                                                        DataFieldParentID="ParentID"
                                                                                        CheckBoxes="true">
                                                                                        <DataBindings>
                                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                             TextField="Name" Expanded="false" ValueField="ID"  />
                                                                                         </DataBindings>
                                                                                        </rad:RadTreeView>
                                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource5" 
                                                                                            SelectCommand="SELECT * FROM EventCategories WHERE ID >= 26 AND  NOT(ID= 52) AND  NOT(ID= 53) AND NOT(ID= 34) AND NOT(ID=40) AND NOT(ID=50) AND NOT(ID=51) AND NOT(ID=61) AND NOT(ID=62) AND NOT (ID=35) AND NOT (ID=47) AND NOT (ID=56) AND NOT (ID=57) AND NOT (ID=58) ORDER BY Name ASC"
                                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                        </asp:SqlDataSource>
                                                                                    </div>
                                                                            </td>
                                                                             </tr>
                                                                    </table>  
                                                            </div>
                                                            
                                                
                                          </div>
                                                </div>
                                                
                                            </div>
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <div style="width: 530px;padding-top: 10px;">
                                                <div style='cursor: pointer; text-decoration: underline;' 
                                                onclick="OpenVenueCategories();" class='PreferencesTitle'><div 
                                                style='float: left;' ID='Div3'>Favorite Locales <span id="Span2">&rArr;</span></div>
                                                </div>
                                                <div id="VenueDiv" style="clear: both; width: 530px;display: none;">
                                            <div  class="Pad2" style="padding-top: 10px;">
                                                <asp:Panel runat="server" ID="VenuesChecksPanel"></asp:Panel>
                                            </div>
                                            </div>
                                            </div>
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                
                                            </div>
                                            <div style="border-bottom: solid 1px #dedbdb; clear: both;padding-top: 30px;">
                                                <asp:Label ID="Label3" runat="server" CssClass="PreferencesTitleAlt"> Event Recommendations Preferences </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px;">(recommend events when they are ...)</span>
                                            </div>
                                            <div  class="Pad topDiv" style="padding-top: 10px;">
                                                <asp:CheckBoxList runat="server" ID="RecommendationsCheckList">
                                                    <asp:ListItem Value="1">in a Favorite Locale</asp:ListItem>
                                                    <asp:ListItem Value="2">in a Favorite Category (only in your specified Filter Location above)</asp:ListItem>
                                                    <asp:ListItem Value="3">Similar to events in your calendar</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <div style="float: right; width: 300px;">
                                                    <span class="NavyLink" style="font-size: 11px;cursor: text;">*These choices apply to event recommendations shown both on the My Account page under
                                                    'New Events' and to any recommendation emails/text that you choose to receive. To make sure you receive the latter, please 
                                                    specify the options below under 'Text Preferences' and 'Email Preferences'.
                                                    </span>
                                                </div>
                                            </div>
                                            
                                            
                                            <%--<div style="border-bottom:solid 1px #dedbdb; clear: both;">
                                                <asp:Label ID="Label4" runat="server" CssClass="PreferencesTitle"> Billing Address </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; ">(used for featured content purchases only)</span>
                                            </div>
                                            <div  class="Pad" style="padding-top: 10px;">
                                                <table>
                                                    <tr>
                                                        <td><label>Address</label></td>
                                                        <td  style="padding-left: 20px;" colspan="2">
                                                            <label>Country</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="AddressTextBox" WIDTH="150px"></asp:TextBox>
                                                            
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
                                                            <asp:TextBox runat="server" ID="BillCityTextBox" WIDTH="150px"></asp:TextBox>
                                                        </td>
                                                        <td  style="padding-left: 20px;">
                                                            <asp:TextBox runat="server" ID="ZipTextBox" WIDTH="100" ></asp:TextBox><br />
                                                        </td>
                                                        <td>
                                                            <asp:Panel runat="server" ID="BillStateTextPanel">
                                                                <asp:TextBox ID="BillStateTextBox" runat="server" WIDTH="100" ></asp:TextBox>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="BillStateDropPanel" Visible="false">
                                                                <asp:DropDownList runat="server" ID="BillStateDropDown"></asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                               

                                            </div>--%>
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label5" runat="server" CssClass="PreferencesTitleAlt"> Phone Number & Provider </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; ">(used when you want to text event/locale/adventure/bulletin info to your phone, i.e. not used by HippoHappenings for any promotional texts.)</span>
                                            </div>
                                            <div  class="Pad" style="padding-top: 10px;">
                                                <table>
                                                    <tr>
                                                        <td><label>Phone Number</label></td>
                                                        <td  style="padding-left: 20px;">
                                                            <label>Provider</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="PhoneTextBox" WIDTH="259" ></asp:TextBox>
                                                            
                                                        </td>
                                                        <td  style="padding-left: 20px;">
                                                            <asp:DropDownList runat="server" ID="ProviderDropDown"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                
                                                </table>
                                               

                                            </div>
                                            <div style="display: block;">
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label6" runat="server" CssClass="PreferencesTitle"> Texting Preferences </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; ">(send notifications to your phone when...)</span>
                                            </div>
                                            <div  class="Pad"  style="padding-top: 10px;">
                                                <asp:CheckBoxList runat="server" ID="TextingCheckBoxList">
                                                    <asp:ListItem Value="1">When new events matching your recommendation criteria have been posted (text once every day if events to recommend exist).</asp:ListItem>
<%--                                                    <asp:ListItem Value="2">When an event in your calendar has been updated (text anytime an event is updated).</asp:ListItem>
--%>                                                </asp:CheckBoxList>
                                            </div>
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label10" runat="server" CssClass="PreferencesTitleAlt"> Email Preferences </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; ">(send notifications to your email address when...)</span>
                                            </div>

                                            
                                            <div  class="Pad" style="padding-top: 10px;">
                                                                <label>Email</label>
                                          
                                               <asp:TextBox runat="server"  ID="EmailTextBox" Width="200px" />
                                                <br /><br />
                                                <label>Automatic Emails</label>
                                                <asp:CheckBoxList runat="server" ID="EmailCheckList">
                                                    <asp:ListItem Value="1">When new events matching your recommendation criteria have been posted (email once every day if events to recommend exist.)</asp:ListItem>
<%--                                                    <asp:ListItem Value="2">When an event in your calendar has been updated (email anytime an event is updated.)</asp:ListItem>
--%>                                                </asp:CheckBoxList>
                                                <br />
                                                <label >User Generated Emails  <span style=" font-family: Arial; font-size: 11px; color: #ff6b09;">(applied to all users. to turn preferences on per friend, un-check these preferences and go to My Friends tab.)</span></label>
                                                
                                                <asp:CheckBoxList runat="server" ID="EmailUserCheckList1">
                                                    <asp:ListItem Value="3" Text="When a friend posts an event/locale/adventure."></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="When a friend sends you a Hippo Mail."></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="When someone sends a friend request to you."></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="When someone accepts your friend request."></asp:ListItem>
                                                    
                                                </asp:CheckBoxList>
                                                <asp:CheckBoxList runat="server" ID="EmailUserCheckList2">
<%--                                                    <asp:ListItem Value="6" Text="When a friend adds an event to their calendar."></asp:ListItem>
--%>                                                    <asp:ListItem Value="7" Text="When someone sends you a Hippo Mail."></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="When a friend shares an event/locale/adventure with you."></asp:ListItem>
                                                </asp:CheckBoxList>
                                                
                                            </div>
                                            <div style="border-bottom:solid 1px #dedbdb; clear: both; padding-top: 10px;">
                                                <asp:Label ID="Label9" runat="server" CssClass="PreferencesTitle"> Comments Preferences </asp:Label><span style="font-style: italic; font-family: Arial; font-size: 12px; ">(The comment list for events, locales, etc. can get long. You can decrease it's size by selecting to only view your friends' comments.)</span>
                                            </div>
                                            <div style="clear: both;padding-top: 10px;"  class="Pad">
                                                <div style="float:left;">
                                                    <asp:RadioButtonList runat="server" ID="CommentsRadioList">
                                                        <asp:ListItem Value="1">See Everyone's </asp:ListItem>
                                                        <asp:ListItem Value="2">See Only Friends'</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="topDiv" style="clear: both;">
                                            <div style="padding-top: 40px; float: left; padding-right: 20px;">
                                                <ctrl:BlueButton runat="server" ID="BottomSaveButton" CLIENT_LINK_CLICK="ShowPrefDiv()" WIDTH="107px" BUTTON_TEXT="Save Changes" />
                                            </div>
                                            <div class="EventDiv" style="padding-top: 45px; float: left;">
                                                <asp:Label runat="server" ID="BottomErrorLabel" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        </div>
                                </div>
                                </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </rad:RadPageView>
                        </rad:RadMultiPage>

            </td>
        </tr>
   </table>
    
   
   </div>

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

