<%@ Page Language="C#" ValidateRequest="false" EnableSessionState="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="Group_OLD.aspx.cs" Inherits="Group" Title="Hippo Group" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Poll.ascx" TagName="Poll" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/TagCloud.ascx" TagName="TagCloud" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/PlayerXML/SongPlayer.ascx" TagName="Songs" TagPrefix="ctrl" %><%@ Register Src="~/Controls/AddToCalendar.ascx" TagName="AddTo" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendTxtToCell.ascx" TagName="SendTxt" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendMessage.ascx" TagName="SendMessage" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SendEmail.ascx" TagName="SendEmail" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/HippoRating.ascx" TagName="HippoRating" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/FlagItem.ascx" TagName="Flag" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/BigAd.ascx" TagName="BigAd" TagPrefix="ctrl" %>
<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager Behaviors="Close, Move, Resize,Maximize"
            ID="RadWindowManager"  Skin="Black" DestroyOnClose="false" runat="server">
            <Windows>
                <rad:RadWindow ID="RadWindow1" Modal="true" VisibleStatusbar="false"  VisibleTitlebar="false" 
                VisibleOnPageLoad="false" ClientCallBackFunction="CallBackFunctionOwner" Height="400" Width="500" Skin="Black"
                     runat="server">
                </rad:RadWindow>  
                <rad:RadWindow Width="800" Modal="true" ReloadOnShow="true"
                ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
                EnableEmbeddedSkins="true" Skin="Black" Height="600" VisibleStatusbar="false" 
                ID="RadWindow2" Title="Invite Members" runat="server">
                </rad:RadWindow>    
                <rad:RadWindow Width="500" Modal="true" ReloadOnShow="true"
                ClientCallBackFunction="FillThemComments"  IconUrl="./image/AddIcon.png"  VisibleTitlebar="false"
                EnableEmbeddedSkins="true" Skin="Black" Height="400" VisibleStatusbar="false" 
                ID="RadWindow3" Title="Invite Members" runat="server">
                </rad:RadWindow>           
            </Windows>
        </rad:RadWindowManager>
         <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true"
            RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="600"
                    EnableEmbeddedSkins="true"  VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Black" Height="500" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </rad:RadWindow>
                    <rad:RadWindow Width="560" EnableEmbeddedSkins="true" IconUrl="image/ContactIcon.png" 
                    Skin="Black" Height="536" VisibleStatusbar="false" ID="RadWindow4" 
                    VisibleTitlebar="false" Title="Send Message" 
                    runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager> 
<rad:RadAjaxPanel runat="server" EnableAJAX="true">


<rad:RadScriptBlock ID="RadScriptBlock1" runat="server">
<rad:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">
    var map = null;
    var geocoder = null;

    function initialize() {
      if (GBrowserIsCompatible()) 
      {
        map = new GMap2(document.getElementById("map_canvas"));
        if(map != null && map != undefined){
            //map.setCenter(new GLatLng(37.4419, -122.1419), 13);
            map.setUIToDefault();
            geocoder = new GClientGeocoder();
            var address = getCookie('addressParameter')
            showAddress(address, 0, address, 1);
        }
      }
    }
function getCookie(c_name)
            {
                if (document.cookie.length>0)
                  {
                  c_start=document.cookie.indexOf(c_name + "=");
                  if (c_start!=-1)
                    {
                    c_start=c_start + c_name.length+1;
                    c_end=document.cookie.indexOf(";",c_start);
                    if (c_end==-1) c_end=document.cookie.length;
                    return unescape(document.cookie.substring(c_start,c_end));
                    }
                  }
                return "";
            }
            function GoToDirections()
            {
                var ToAddress = getCookie('addressParameter').replace(/@&/g, ' ').replace(/ /g, '+');
                var theInput = document.getElementById('FromInput');
                var FromAddress = theInput.value.replace(/,/g, ' ').replace(/ /g, '+');

                window.open('http://mapof.it/'+FromAddress+'/'+ToAddress);
            }
    function showAddress(address, tryCount, wholeAddress, repeatCount) {
       
       // Create our "tiny" marker icon
        var blueIcon = new GIcon(G_DEFAULT_ICON);
        blueIcon.image = "http://hippohappenings.com/image/HippoMapPoint4.png";
		
		// Set up our GMarkerOptions object
		markerOptions = { icon:blueIcon };

       var temp = wholeAddress.replace(/@&/g, " ").split(" ");
      if (geocoder) {
        geocoder.getLatLng(
          address.replace(/@&/g, " "),
          function(point) {
            if (!point) 
            {
              if(tryCount > temp.length && repeatCount == 2)
              {
                alert(address.replace(/@&/g, " ") + tryCount+" not found. Try correcting this address.");
              }
              else
              {
                  var addition = 1;
                  if(tryCount > temp.length || repeatCount == 2)
                  {
                    if(repeatCount == 1)
                    {
                        tryCount = 0;
                    }
                    
                    var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1) && i != temp.length - (tryCount+2))
                                {
                                    
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 2);
                    
                  }
                  else
                  {
                      var nextAddress = "";
                      var i;
                      for(i=0;i<temp.length;i++)
                      {
                            if(i==temp.length-1)
                            {
                                nextAddress += temp[i] + " ";
                            }
                            else
                            {
                                if(i != temp.length - (tryCount+1))
                                {
                                    nextAddress += temp[i]+" ";
                                }
                            }
                      }
                      showAddress(nextAddress, tryCount + 1, wholeAddress, 1);
                  }
              }
            } 
            else {
              map.setCenter(point, 15);
              var marker = new GMarker(point, markerOptions);
              map.addOverlay(marker);
            }
          }
        );
      }
    }

    </script>
<script type="text/javascript">
(function() {
var s = document.createElement('SCRIPT'), s1 = document.getElementsByTagName('SCRIPT')[0];
s.type = 'text/javascript';
s.async = true;
s.src = 'http://widgets.digg.com/buttons.js';
s1.parentNode.insertBefore(s, s1);
})();
</script>
<script type="text/javascript">

       //<![CDATA[
        
        function OpenSendMess(theID)
         {
            var win = $find("<%=RadWindow4.ClientID %>");
            win.setUrl('SendMessageToParticipants.aspx?type=g&ID='+theID);
            win.show(); 
            win.center(); 
         }

        function OpenInvite(theID)
        {
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('InviteMembers.aspx?ID='+theID);
            win.show(); 
            win.center(); 
               
         }
         
         function OpenShare(theID)
         {
            var win = $find("<%=RadWindow4.ClientID %>");
            win.setUrl('MessageAlert.aspx?T=Message&ID='+ theID + '&A=g');
            win.show(); 
            win.center(); 
         }
         
         function OpenPrefs(theID)
         {
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('EditMemberPrefs.aspx?ID='+theID);
            win.show(); 
            win.center(); 
         }
         
         function OpenThread(theID)
         {
            var win = $find("<%=RadWindow2.ClientID %>");
            win.setUrl('StartThread.aspx?ID='+theID);
            win.show(); 
            win.center(); 
         }
         
         function OpenMess(theID)
         {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('PostMemberComment.aspx?ID='+theID);
            win.show(); 
            win.center(); 
         }

        function OpenComment(theID, tid)
         {
            var win = $find("<%=RadWindow3.ClientID %>");
            win.setUrl('PostThreadComment.aspx?ID='+theID+'&TID='+tid);
            win.show(); 
            win.center(); 
         }
        
        function OpenJoin(theID)
         {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('JoinUs.aspx?ID='+theID);
            win.show(); 
            win.center(); 
         }
         
         function OpenRemoveSticky(theID)
         {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('RemoveSticky.aspx?ID='+theID);
            win.show(); 
            win.center(); 
         }
        
        function OpenThreadAdmin(theID, theM)
         {
            var win = $find("<%=RadWindow3.ClientID %>");
            win.setUrl('ThreadAdmin.aspx?ID='+theID+'&M='+theM);
            win.show(); 
            win.center(); 
         }
        
        function FillThemComments()
        {
            __doPostBack('ctl00$ContentPlaceHolder1$CommentDoButton','');
        }

         function OpenEventModal(id, eventID)
        {
            var win = $find("<%=MessageRadWindow.ClientID %>");
            win.setUrl("EventModal.aspx?ID=" + id + "&eventID="+eventID);
            win.show(); 
            win.center(); 
         }
         
         function OpenOwner()
                 {
                    var win = $find("<%=RadWindow2.ClientID %>");
                    win.setUrl('Message.aspx');
                    win.show(); 
                    win.center();
                 }
                 
                 function CallBackFunctionOwner(radWindow, returnValue)
                {
                    
                    if(returnValue != null && returnValue != undefined)
                        window.location = returnValue;
                }
                
                
                                
                function  ReturnURL()
                {
                    var TA = document.getElementById('TweeterA');
                    TA.href = 'http://twitter.com/home?status=' + window.location;
                }
                
        //]]>

    </script>

</rad:RadCodeBlock>
</rad:RadScriptBlock>
   </rad:RadAjaxPanel>
    
     <div id="topDiv" style="position: relative; top: -25px; left: -15px;">
            <div style="float: left;clear: both;margin-bottom: 30px;background-position: bottom;
             width: 880px; height: 70px; background-repeat: no-repeat;" align="left">
                <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
                <table style="padding-left: 10px;">
                    <tr>
                        <td width="620px" valign="top"><h1 class="GroupHeader"><asp:Label runat="server" ID="EventName"></asp:Label></h1></td>
                        <td valign="top" style="padding-top: 8px;">
                            <asp:Label CssClass="AddGreenLink" runat="server" ID="PrivateLabel" ForeColor="#90bf2a"></asp:Label>
                            
                        </td>
                        <td style="padding-left: 20px;" valign="top">
                            <div ID="Button2" style="padding-top: 6px; padding-left: 25px;" class="SearchButton" runat="server"
                                onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >join group</div>
                        </td>
                    </tr>
                </table>
                <div style="position: relative; bottom: -25px; padding-left: 10px;">
                    <asp:Panel CssClass="OptionTitle" runat="server" ID="HostHeaderPanel">
                        <label class="OptionTitle">Host Options: </label>
                        <label ID="LinkButton1" runat="server" class="OptionLink">Invite members</label>
                        - <label ID="LinkButton2" runat="server" class="OptionLink">Edit members' prefs</label>
                        - <label ID="Label1" runat="server" class="OptionLink">Send message to members</label>
                        - <asp:LinkButton ID="LinkButton3" runat="server" CssClass="OptionLink" Text="Edit group"></asp:LinkButton>
                        - <asp:LinkButton ID="LinkButton4" runat="server" CssClass="OptionLink" Text="Add an event"></asp:LinkButton>
                        - <label ID="LinkButton8" runat="server" class="OptionLink">Start a discussion thread</label>
                        - <label onclick="window.location = 'http://hippohappenings.com/User.aspx?G=true#groups';" class="OptionLink">Edit Your Email Prefs</label>
                    </asp:Panel>
                    <asp:Panel runat="server" CssClass="OptionTitle" ID="MemberHeaderPanel" Visible="false">
                        <label class="OptionTitle">Member Options: </label>
                        <label ID="Label2" runat="server" class="OptionLink">Start a discussion thread</label>
                      - <label onclick="window.location = 'http://hippohappenings.com/User.aspx?G=true#groups';" class="OptionLink">Edit Your Email Prefs</label>
                    </asp:Panel>
                </div>
                
            </div>
            <rad:RadTabStrip runat="server" ID="RadTabStrip1" Width="900px" Orientation="HorizontalTop"
                                     MultiPageID="RadMultiPage1" Skin="Telerik" SelectedIndex="0" >
                                    <Tabs>
                                        <rad:RadTab Text="-Home-" TabIndex="0" PageViewID="RadPageView1">
                                        </rad:RadTab>
                                        <rad:RadTab Text="-Stuff We Talk About-" TabIndex="1" 
                                        PageViewID="RadPageView2">
                                        </rad:RadTab>
                                    </Tabs>
                                </rad:RadTabStrip>
                                <rad:RadMultiPage runat="server" ID="RadMultiPage1" 
                                SelectedIndex="0">
                                        <rad:RadPageView  runat="server" ID="RadPageView1" TabIndex="0">
                                       
                                       
                                            
           <div  style="padding-left: 5px; width: 900px; clear: both;">
           <asp:Panel runat="server" Visible="false" ID="LoggedInPanel">
                    <div class="topDiv" style=" float: left;">
                        <div style="float: left;">
                            <ctrl:SendMessage runat="server" ID="ShareFriends" THE_TEXT="Share with friends" />
                        </div>
                        <div style="float: left; padding-left: 8px;">
                            <ctrl:SendEmail ID="SendEmail1" runat="server" THE_TEXT="Share though email" />
                        </div>
                        <div style="float: left; padding-left: 11px;">
                            <ctrl:Flag runat="server" ID="Flag1" />
                        </div>
                    </div>
                </asp:Panel>
              <div style="width: 460px; float: left;">
                <div style=" width: 460px; position: relative;">
                    <div style="padding-bottom: 15px; padding-left: 8px;">
                        <span style="color: White; font-family: Arial; font-size: 14px;">Host: 
                        <asp:HyperLink runat="server" ID="HostLabel" CssClass="AddLink"></asp:HyperLink></span>
                    </div>
                    <div style=" top: 0; right: 0; width: 200px; position: absolute;">
                        
                            <asp:Literal runat="server" ID="ImageLiteral"></asp:Literal>
                            <asp:Image runat="server" ID="GroupMainImage" />
                            <asp:Literal runat="server" ID="ImageLiteralBottom" />
                        <asp:Panel runat="server" ID="SocialsHorizontal">
                        <table style="padding-top: 5px;">
                            <tr>
                                <td valign="bottom" style="padding-right: 0px;">
                                    <a name="fb_share" type="button" href="http://www.facebook.com/sharer.php">Share</a><script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share" type="text/javascript"></script>
                                </td>
                                <td valign="bottom" style="padding-right: 0px;">
                                    <a style="border: 0; padding: 0; margin: 0;" id="TweeterA" title="Click to send this page to Twitter!" target="_blank" rel="nofollow"><img style="border: 0; padding: 0; margin: 0;" src="http://twitter-badges.s3.amazonaws.com/twitter-a.png" alt="Share on Twitter"/></a>
                                </td>
                                <td valign="bottom">
                                    <a href="javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())">
                                        <img src="http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png" border="0" alt="Share on MySpace" />
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <td valign="bottom" colspan="3">
                                    <asp:Literal runat="server" ID="DiggLiteral"></asp:Literal>
                                
                                    <a href="http://delicious.com/save" onclick="window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;">
                                        <img border="0" src="http://static.delicious.com/img/delicious.small.gif" height="10" width="10" alt="Delicious" />
                                    </a>
                                     <script src="http://www.stumbleupon.com/hostedbadge.php?s=4"></script>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="SocialsVertical">
                            <table style="padding-top: 50px; padding-left: 70px;">
                                <tr>
                                    <td valign="bottom" style="padding-right: 0px;">
                                        <a name="fb_share" type="button" href="http://www.facebook.com/sharer.php">Share</a><script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share" type="text/javascript"></script>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" style="padding-right: 0px;">
                                        <a style="border: 0; padding: 0; margin: 0;" id="A1" title="Click to send this page to Twitter!" target="_blank" rel="nofollow"><img style="border: 0; padding: 0; margin: 0;" src="http://twitter-badges.s3.amazonaws.com/twitter-a.png" alt="Share on Twitter"/></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom">
                                        <a href="javascript:void(window.open('http://www.myspace.com/Modules/PostTo/Pages/?u='+encodeURIComponent(document.location.toString()),'ptm','height=450,width=440').focus())">
                                            <img src="http://cms.myspacecdn.com/cms/ShareOnMySpace/small.png" border="0" alt="Share on MySpace" />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom">
                                        <asp:Literal runat="server" ID="DiggLiteralVertical"></asp:Literal>
                                    
                                        <a href="http://delicious.com/save" onclick="window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;">
                                            <img border="0" src="http://static.delicious.com/img/delicious.small.gif" height="10" width="10" alt="Delicious" />
                                        </a>
                                         <script src="http://www.stumbleupon.com/hostedbadge.php?s=4"></script>
                                    </td>
                                </tr>
                               
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="background-image: url('images/PurpleMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                        <div style="min-height: 211px;background-image: url('images/PurpleTop2.png'); background-repeat: no-repeat; background-position: top left;">
                            <div style="width: 243px; padding-left: 8px;" class="GroupDescription">
                                <asp:Label runat="server" ID="GroupDescriptionLabel"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div style="height: 18px;background-image: url('images/PurpleBottom.png'); background-position: top left; background-repeat: no-repeat;">
                    </div>
                </div>
                <div style="width: 460px; padding-top: 10px;">
                    <asp:Panel runat="server" ID="RotatorPanel">
                    <div class="GroupTitles" style="padding-bottom: 10px;">Us In Action</div>
                        <div>
                            <rad:RadRotator runat="server" AutoPostBack="false"  
                            AppendDataBoundItems="false" ScrollDuration="200" ID="Rotator1" 
                            ItemHeight="250" ItemWidth="400"  
                            Width="440px" Height="300px" RotatorType="Buttons">
                            </rad:RadRotator>
                        </div>
                    </asp:Panel>
                </div>
                <div style="width: 460px; padding-top: 10px;">
                    <div class="GroupTitles" style="padding-bottom: 3px;">The Usual Suspects</div>
                    <div style="background-image: url('images/PurpleMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                        <div class="topDiv" style="min-height: 211px;background-image: url('images/PurpleTop2.png'); background-repeat: no-repeat; background-position: top left;">
                            <div style="width: 425px; padding-left: 8px;" class="GroupDescription">
                                <asp:Literal runat="server" ID="MembersLiteral"></asp:Literal>
                                <asp:Panel runat="server" ID="MembersPanel" Width="415px" ScrollBars="Vertical" Visible="false" Height="300px"></asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div style="height: 18px;background-image: url('images/PurpleBottom.png'); background-position: top left; background-repeat: no-repeat;">
                    </div>
                </div>
                
                <div class="topDiv" style="clear: both;">
                    <div style="float: left; width: 245px;">
                        <div class="GroupTitles">Group Location</div>
                        <div style="width: 245px; height: 305px; padding-top: 3px;padding-left: 5px;">
                            <div id="map_canvas" style="width: 245px; height: 305px; display: block;"></div>
                        </div>
                    </div>
                    <div style="float: left; width: 190px; padding-top: 15px; padding-left: 25px;">
                        <div class="AddressTitle"><asp:label runat="server" ID="AddressLabel">Address</asp:label></div>
                        <asp:Label CssClass="AddressText" runat="server" ID="NameOfPlaceLabel"></asp:Label>
                        <asp:Label CssClass="AddressText" runat="server" ID="Address1Label"></asp:Label>
                        <asp:Label CssClass="AddressText" runat="server" ID="Address2Label"></asp:Label>
                        <asp:Label CssClass="AddressText" runat="server" ID="CityStateZipLabel"></asp:Label>
                        <div class="AddressTitle"><asp:label Visible="false" runat="server" ID="GroupContactInfoLabel">Group Contact Info</asp:label></div>
                        <asp:Label CssClass="AddressText" runat="server" ID="PhoneLabel"></asp:Label>
                        <asp:Label CssClass="AddressText" runat="server" ID="EmailLabel"></asp:Label>
                        <asp:Label CssClass="AddressText" runat="server" ID="WebLabel"></asp:Label>
                        <div class="AddressTitle"><asp:label runat="server" Visible="false" ID="HostTitleInstructionsLabel">Host's Instructions</asp:label></div>
                        <asp:Label CssClass="AddressText" runat="server" ID="HostInstructionsLabel"></asp:Label>
                        <div class="AddressTitle">Get Directions From:</div>
                        <input onkeypress="{if (event.keyCode==13)GoToDirections();}" type="text" id="FromInput" maxlength="100" />
                        <a style="text-decoration: underline;" class="AddLink" onclick="GoToDirections();">go</a>
                    </div>
                </div>
              </div>
              <div class="topDiv" style="width: 430px; padding-left: 5px; float: left;">
               
                    <div style="margin-top: 40px;">
                        <ctrl:BigAd runat="server" />
                    </div>
                    <div style="width: 460px; padding-top: 10px;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;">Group Message Board</div>
                        <asp:Panel runat="server" ID="PostMessagePanel">
                        <div style="float: right; padding-right: 50px; padding-top: 20px;"><a class="AddLink" runat="server" id="PostMessageID">post a message</a></div>
                        </asp:Panel>
                        </div>
                        <div style="background-image: url('images/GreenMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                            <div class="topDiv" style="min-height: 230px;background-image: url('images/GreenTop2.png'); background-repeat: no-repeat; background-position: top left;">
                                <div style="width: 390px; padding-left: 15px;" class="GroupDescription">
                                    <asp:Literal runat="server" ID="StickyMessageLiteral"></asp:Literal>
                                    <asp:Panel runat="server" CssClass="MessageBoard" ID="MessagesPanel" Height="200px" Width="390px" ScrollBars="Vertical"></asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div style="height: 18px;background-image: url('images/GreenBottom.png'); background-position: top left; background-repeat: no-repeat;">
                        </div>
                    </div>
                    <div class="topDiv" style="width: 460px; padding-top: 10px;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;">Our Events</div>
                        </div>
                        <div>
                        
                                <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                    <ContentTemplate>
                                    <script type="text/javascript" language="javascript">
                                        function ShowLoadingCalendar()
                                        {
                                            var d = document.getElementById('Div1');
                                            d.style.display = 'block';
                                        }
                                    </script> 
                                         <asp:Panel runat="server" ID="ToolTipPanel"></asp:Panel>
                                         <table width="100%">
                                            <tr>
                                                <td width="50px">
                                                    <asp:LinkButton OnClientClick="ShowLoadingCalendar()" CssClass="AddOrangeLink" ID="LinkButton5" runat="server" Text="< prev" OnClick="DevanceMonth"></asp:LinkButton>
                                                </td>
                                                <td width="50px">
                                                    <asp:LinkButton OnClientClick="ShowLoadingCalendar()" CssClass="AddOrangeLink"  runat="server" ID="LinkBzatton" Text="next >" OnClick="AdvanceMonth"></asp:LinkButton>
                                                </td>
                                                <td align="center">
                                                    <asp:Label CssClass="EventHeader" runat="server" ID="MonthLabel"></asp:Label>
                                                </td>
                                            </tr>
                                         </table>
                                         <div class="topDiv" style=" position: relative;">
                                            <div align="center" id="Div1"  style="display: none;z-index: 3000; padding-left: 30px;margin-top: 50px; position: absolute; width: 400px; height: 200px;margin-left: -20px;" >
                                                <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 200px; width: 100px; background-color: #cccccc;">
                                                </div>
                                                <div style="float: left;height: 200px;width: 200px;">
                                                    <div style=" float: left;opacity: .5; filter: alpha(opacity=50); background-color: #cccccc; width: 200px; height: 50px;"></div>
                                                    <div style="padding-top: 30px;clear: both;position: relative;top: 45; height: 70px;background-color: #363636; opacity: 1; filter: alpha(opacity=100);">
                                                            <img src="image/ajax-loaderBig.gif" />
                                                            <span class="updateProgressMessage">Loading ...</span>
                                                    </div>
                                                    <div style="float: left;opacity: .5; filter: alpha(opacity=50); background-color: #cccccc; width: 200px; height: 50px;"></div>
                                                </div>
                                                <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 200px; width: 100px; background-color: #cccccc;"></div>
                                            </div>
                                        </div>
                                             <div style="width: 300px; float: left;">
                                                 <rad:RadScheduler BorderColor="Orange" BorderWidth="1px" BorderStyle="solid" runat="server" 
                                                 ID="RadScheduler1" Width="420px" Height="320px" 
                                                        OverflowBehavior="expand" BackColor="#1b1b1b"
                                                        Skin="Black" DataKeyField="DateStart"  DataStartField="DateStart"
                                                        DataEndField="DateStart" DataSubjectField="Name" DayEndTime="23:59:59"
                                                        DayStartTime="00:00:00" SelectedView="monthView"
                                                         ShowAllDayRow="false" OnAppointmentCreated="RadScheduler1_AppointmentCreated" 
                                                         AllowEdit="false" AllowDelete="false" ShowHeader="false" ShowFullTime="true" 
                                                         StartInFullTime="true" AllowInsert="false">
                                                        <WeekView UserSelectable="false" />
                                                        <DayView UserSelectable="false" />
                                                        <TimelineView UserSelectable="false" />
                                                        <MonthView UserSelectable="false" MinimumRowHeight="5"/>
                                                 </rad:RadScheduler>
                                             </div>
                                         
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                       </div>
                    </div>
                    <div class="topDiv" style="width: 460px; padding-top: 10px;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;">Group's Categories</div>
                        </div>
                        <div style="background-image: url('images/GreenMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                            <div class="topDiv" style="min-height: 230px;background-image: url('images/GreenTop2.png'); background-repeat: no-repeat; background-position: top left;">
                                <div style="width: 390px; padding-left: 15px;" class="GroupDescription">
                                    <ctrl:TagCloud runat="server" TAG_TYPE="GROUP" ID="TagCloud" />
                                </div>
                            </div>
                        </div>
                        <div style="height: 18px;background-image: url('images/GreenBottom.png'); background-position: top left; background-repeat: no-repeat;">
                        </div>
                    </div>
              </div>
           </div>
         
                                    </rad:RadPageView>
                                    <rad:RadPageView runat="server" ID="RadPageView2" 
                                    TabIndex="1">
                                   
                                        
           <div style="width: 900px; clear: both; padding-top: 20px; padding-left: 10px;">
                <div style="width: 880px;">
                        <div class="topDiv">
                        </div>
                        <script type="text/javascript" language="javascript">
                            function ShowLoading()
                            {
                                var d = document.getElementById('insideUpdateProgressDiv');
                                d.style.display = 'block';
                            }
                        </script> 
                        <div style="background-image: url('images/BigPurpleMiddle2.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                            <div class="topDiv" style="min-height: 403px;background-image: url('images/BigPurpleTop2.png');background-repeat: no-repeat; background-position: top left;">
                                <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton runat="server" ID="CommentDoButton" OnClientClick="ShowLoading()" OnClick="FillC"></asp:LinkButton>
                                        <asp:Label runat="server" ID="ThreadErrorLabel" ForeColor="red"></asp:Label>
                                        <div class="topDiv" style="padding-top: 20px; padding-left: 20px; position: relative;">
                                            <div align="center" id="insideUpdateProgressDiv"  style="margin-top: 50px;display:none; position: absolute; width: 873px; height: 50px;margin-left: -20px;" >
                                                <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 100px; width: 336px; background-color: #363636;">
                                                </div>
                                                <div style="padding-top: 30px;float: left;height: 70px;background-color: #363636;width: 200px; opacity: 1; filter: alpha(opacity=100);">
                                                <img src="image/ajax-loaderBig.gif" />
                                                <span class="updateProgressMessage">Loading ...</span>
                                                </div>
                                                <div style="float: left;opacity: .5; filter: alpha(opacity=50);height: 100px; width: 337px; background-color: #363636;"></div>
                                                </div>
                                            </div>
                                            <div style="width: 300px; float: left; padding-left: 10px;">
                                                <asp:Panel runat="server" ID="SortPanel">
                                                    <asp:DropDownList AutoPostBack="true" onclick="ShowLoading();" OnSelectedIndexChanged="SortThreads" 
                                                    runat="server" ID="SortDropDown" >
                                                        <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Name"></asp:ListItem>
                                                        <asp:ListItem Text="Date" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Most Recent Comment" Value="3"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="ThreadsPanel"></asp:Panel>
                                            </div>
                                            <div style="width: 530px; float: left; padding-left: 10px;">
                                                <asp:Panel ID="CommentsPanel" runat="server"></asp:Panel>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div style="height: 30px;background-image: url('images/BigPurpleBottom2.png'); background-position: top left; background-repeat: no-repeat;">
                        </div>
                    </div>
           </div>
          
                                    </rad:RadPageView>
                                </rad:RadMultiPage>
    </div>
         <rad:RadCodeBlock runat="server">
            <rad:RadScriptBlock runat="server">
                <script type="text/javascript">
                        var rad1 = document.getElementById("<%=RadScheduler1.ClientID %>");
                        var ab = "2";
              </script>
            </rad:RadScriptBlock>
         </rad:RadCodeBlock>
      
        <script type="text/javascript">
            ReturnURL();
        </script>

</asp:Content>

