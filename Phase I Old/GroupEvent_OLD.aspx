<%@ Page Language="C#" ValidateRequest="false" EnableSessionState="true" MasterPageFile="~/MasterPage.master" 
AutoEventWireup="true" EnableEventValidation="false" 
CodeFile="GroupEvent_OLD.aspx.cs" Inherits="GroupEvent" Title="Hippo Group Event" %>
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
                <rad:RadWindow Width="350" Modal="true" ReloadOnShow="true"
                ClientCallBackFunction="CallBackFunctionOwner"  IconUrl="./image/AddIcon.png"  
                VisibleTitlebar="false"
                EnableEmbeddedSkins="true" Skin="Black" Height="300" VisibleStatusbar="false" 
                ID="RadWindow5" Title="Invite Members" runat="server">
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
            win.setUrl('SendMessageToParticipants.aspx?type=e&ID='+theID);
            win.show(); 
            win.center(); 
         }
         
         function OpenShare(theID)
         {
            var win = $find("<%=RadWindow4.ClientID %>");
            win.setUrl('MessageAlert.aspx?T=Message&ID='+ theID + '&A=ge');
            win.show(); 
            win.center(); 
         }
         
         function OpenMess(theID, reoccurrID)
         {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('PostEventComment.aspx?ID='+theID+'&O='+reoccurrID);
            win.show(); 
            win.center(); 
         }

         
         function OpenRemoveSticky(theID, rID, reoccurrID)
         {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('RemoveEventSticky.aspx?ID='+theID+'&RID='+rID+'&O='+reoccurrID);
            win.show(); 
            win.center(); 
         }
        
        function OpenParticipants(theID, theO)
        {
            var win = $find("<%=RadWindow1.ClientID %>");
            win.setUrl('ManageParticipants.aspx?ID='+theID+'&O='+theO);
            win.show(); 
            win.center(); 
        }

        function OpenGroupEventDelete(theID, theO)
        {
            var win = $find("<%=RadWindow5.ClientID %>");
            win.setUrl('DeleteGroupEvent.aspx?ID='+ theID + '&O='+theO);
            win.show(); 
            win.center(); 
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
            <div style="float: left;clear: both;margin-bottom: 40px;background-position: bottom;
             width: 880px; height: 100px; background-repeat: no-repeat;" align="left">
                <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
                <table style="padding-left: 10px;">
                    <tr>
                        <td width="566px" valign="top"><h1 class="GroupHeader"><asp:Label runat="server" ID="EventName"></asp:Label></h1></td>
                        <td valign="top" style="padding-top: 8px;">
                            <asp:Label CssClass="AddGreenLink" runat="server" ID="PrivateLabel" ForeColor="#90bf2a"></asp:Label>
                            
                        </td>
                        <td style="padding-left: 20px;padding-top: 8px;" valign="top">
<%--                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                    <ContentTemplate>
--%>                                    <asp:Panel runat="server" ID="GoingPanel" Visible="false">
                                                    <asp:Label runat="server" CssClass="AddGreenLink">Are you going to this event </asp:Label>
                                                    <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="CheckGoing" ID="GoingYesCheckBox" />
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="AreGoingPanel" Visible="false">
                                        <asp:Label runat="server" CssClass="AddGreenLink">You are going to this event</asp:Label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="RegistrationEndedPanel" Visible="false">
                                        <span class="AddGreenLink">The registration for this event has ended</span>
                                    </asp:Panel>
<%--                             </ContentTemplate>
                                </asp:UpdatePanel>
--%>                        </td>
                    </tr>
                </table>
                <div style="position: relative; bottom: -18px; padding-left: 10px;">
                    <asp:Panel CssClass="OptionTitle" runat="server" ID="HostHeaderPanel">
                        <label class="OptionTitle">Host Options: </label>
                        <label ID="LinkButton1" runat="server" class="EventOptionLink">Edit event's details</label>
                        -- <label ID="LinkButton2" runat="server" class="EventOptionLink">Copy Event</label>
                        -- <label ID="Label1" runat="server" class="EventOptionLink">Send message to participants</label>
                        -- <label ID="Label3" runat="server" class="EventOptionLink">Manage participants</label>
                        -- <label ID="Label2" runat="server" class="EventOptionLink">Delete Event</label>
                    </asp:Panel>
                    <asp:Panel runat="server" CssClass="OptionTitle" ID="MemberHeaderPanel" Visible="false">
                    </asp:Panel>
                    
                </div>
            </div>
           <div  style="padding-left: 5px; width: 900px; clear: both;">
           <asp:Panel runat="server" Visible="false" ID="LoggedInPanel">
                    <div class="topDiv" style="position: absolute; top: 100px; float: left;">
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
              <div style="width: 420px; float: left;">
                <div style=" width: 410px; position: relative; float: left;">
                    <div style="padding-bottom: 15px; padding-left: 8px;">
                        <span style="color: White; font-family: Arial; font-size: 14px;">Creator: 
                        <asp:HyperLink CssClass="AddLink" runat="server" ID="HostLabel"></asp:HyperLink></span><span style="color: White; font-family: Arial; font-size: 14px;">, Group: <asp:HyperLink runat="server" CssClass="AddGreenLink" 
                            ID="GroupLabel"></asp:HyperLink>
                        </span><br /><br />
                        <span style="color: #cccccc; font-style: italic; font-family: Arial; font-size: 14px;">Date & Time: 
                        <asp:Label runat="server" ID="DateAndTimeLabel"></asp:Label></span>
                    </div>
                    <asp:Label runat="server" ForeColor="#cccccc" CssClass="GroupDescription" ID="GroupDescriptionLabel"></asp:Label>
                    <br />
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
                                <td>
                                    <asp:Literal runat="server" ID="DiggLiteral"></asp:Literal>
                                </td>
                                <td>
                                    <a href="http://delicious.com/save" onclick="window.open('http://delicious.com/save?v=5&noui&jump=close&url='+encodeURIComponent(location.href)+'&title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;">
                                        <img border="0" src="http://static.delicious.com/img/delicious.small.gif" height="10" width="10" alt="Delicious" />
                                    </a>
                                </td>
                                <td>
                                     <script src="http://www.stumbleupon.com/hostedbadge.php?s=4"></script>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                </div>
                    
                           
                <div class="topDiv" style="float: left; width: 420px; padding-top: 10px;">
                    <div class="GroupTitles" style="padding-bottom: 3px;">Participants &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                     <asp:Label runat="server" ID="GroupingLabel"></asp:Label></div>
                    <div style="background-image: url('images/GreenMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                        <div style="min-height: 230px;background-image: url('images/GreenTop2.png'); background-repeat: no-repeat; background-position: top left;">
                                <table>
                                    <tr>
                                        <td width="226px" valign="top">
                                            <asp:Literal runat="server" ID="MembersLiteral"></asp:Literal>
                                            <asp:Panel Width="225px" runat="server" ID="MembersPanel" ScrollBars="Vertical" Visible="false" Height="300px"></asp:Panel>
                                        </td>
                                        <td width="210px" valign="top">
                                            <asp:Panel runat="server" ID="GroupingPanel"></asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                        </div>
                    </div>
                    <div style="height: 18px;background-image: url('images/GreenBottom.png'); background-position: top left; background-repeat: no-repeat;">
                    </div>
                </div>
             
             
                <asp:Panel runat="server" ID="AgendaPanel" Visible="false">
                <div style="width: 420px; padding-top: 10px; padding-left: 5px; float: left;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;clear: both;">Agenda</div>
                            <div style="float: left; clear: both;padding-left: 5px; padding-right: 5px;width: 350px; padding-top: 10px; padding-bottom: 10px;">
                                <asp:Literal runat="server" ID="AgendaLiteral"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                
                   
                
                
                <div class="topDiv" style="width: 426px; padding-top: 10px; float: left;">
                    <div class="GroupTitles">Event Directions</div>
                    <div style="background-image: url('images/GreenMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                        <div class="topDiv" style="min-height: 230px;background-image: url('images/GreenTop2.png'); background-repeat: no-repeat; background-position: top left;">
                            <div style="float: left; width: 245px; padding-left: 5px;">
                                
                                <div style="width: 245px; height: 305px; padding-top: 10px;padding-left: 5px; padding-bottom: 10px;">
                                    <div id="map_canvas" style="width: 245px; height: 305px; display: block;"></div>
                                </div>
                            </div>
                            <div style="width: 150px;float: left; padding-top: 15px; padding-left: 13px;">
                                <div class="AddressTitle"><asp:label runat="server" ID="AddressLabel">Address</asp:label></div>
                                <asp:Label CssClass="AddressText" runat="server" ID="NameOfPlaceLabel"></asp:Label>
                                <asp:Label CssClass="AddressText" runat="server" ID="Address1Label"></asp:Label><br />
                                <asp:Label CssClass="AddressText" runat="server" ID="Address2Label"></asp:Label>
                                <asp:Label CssClass="AddressText" runat="server" ID="CityStateZipLabel"></asp:Label>
                                <div class="AddressTitle">Get Directions From:</div>
                                <input onkeypress="{if (event.keyCode==13)GoToDirections();}" type="text" id="FromInput" maxlength="100" />
                                <br />
                                <a style="text-decoration: underline;" class="AddLink" onclick="GoToDirections();">go</a>
                            </div>
                        </div>
                    </div>
                    <div style="height: 18px;background-image: url('images/GreenBottom.png'); background-position: top left; background-repeat: no-repeat;">
                    </div>
                </div>
                
              </div>
              <div style="width: 420px; float: left; padding-left: 30px;">
                <div style="width: 420px; padding-top: 10px; padding-left: 15px; float: left;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;">Event Message Board</div>
                        <asp:Panel runat="server" ID="PostMessagePanel">
                        <div style="float: right; padding-right: 15px; padding-top: 20px;"><a class="AddLink" runat="server" id="PostMessageID">post a message</a></div>
                        </asp:Panel>
                        </div>
                        <div style="background-image: url('images/BlueMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                            <div class="topDiv" style="min-height: 230px;background-image: url('images/BlueTop.png'); background-repeat: no-repeat; background-position: top left;">
                                <div style="width: 390px; padding-left: 15px;" class="GroupDescription">
                                    <asp:Literal runat="server" ID="StickyMessageLiteral"></asp:Literal>
                                    <asp:Panel runat="server" CssClass="MessageBoard" ID="MessagesPanel" Height="200px" Width="390px" ScrollBars="Vertical"></asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div style="height: 18px;background-image: url('images/BlueBottom.png'); background-position: top left; background-repeat: no-repeat;">
                        </div>
                    </div>
                 

                
                <div style="width: 420px; padding-top: 10px; float: left;">
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
                
                <asp:Panel runat="server" ID="StuffsPanel" Visible="false">
                    <script type="text/javascript">
                        function RemoveStuffUser(theID)
                        {
                            __doPostBack('ctl00$ContentPlaceHolder1$RemoveStuffs', theID);
                        }
                    </script>
                    <asp:UpdatePanel runat="server" UpdateMode="conditional">
                        <ContentTemplate>
                            <asp:LinkButton runat="server" ID="RemoveStuffs" OnClick="RemoveUserStuff"></asp:LinkButton>
                            <div class="topDiv" style="float: left; padding-left: 15px;">
                                <div class="GroupTitles" style="padding-bottom: 3px;">Stuff we need</div>
                                <div style="background-image: url('images/BlueMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                                    <div style="min-height: 230px;background-image: url('images/BlueTop.png'); background-repeat: no-repeat; background-position: top left;">
                                        <div style="width: 401px; padding-left: 8px;" class="GroupDescription">
                                            <asp:Panel runat="server" ID="StuffWeNeedPanel"></asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <div style="height: 18px;background-image: url('images/BlueBottom.png'); background-position: top left; background-repeat: no-repeat;">
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:Panel>
                
                <div class="topDiv" style="width: 430px; float: left; padding-left: 15px;">                    
                    <div class="topDiv" style="width: 460px; padding-top: 10px;">
                        <div class="topDiv">
                        <div class="GroupTitles" style="padding-bottom: 3px; float: left;">Group Event's Categories</div>
                        </div>
                        <div style="background-image: url('images/BlueMiddle.png'); background-repeat: no-repeat; background-repeat: repeat-y;">
                            <div class="topDiv" style="min-height: 230px;background-image: url('images/BlueTop.png'); background-repeat: no-repeat; background-position: top left;">
                                <div style="width: 390px; padding-left: 15px;" class="GroupDescription">
                                    <ctrl:TagCloud runat="server" TAG_TYPE="GROUP_EVENT" ID="TagCloud" />
                                </div>
                            </div>
                        </div>
                        <div style="height: 18px;background-image: url('images/BlueBottom.png'); background-position: top left; background-repeat: no-repeat;">
                        </div>
                    </div>
              </div>
                
              </div>
           </div>
        
    </div>
        
      
        <script type="text/javascript">
            ReturnURL();
        </script>

</asp:Content>

